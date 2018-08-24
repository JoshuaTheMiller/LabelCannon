using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Service.LabelQuery.RepositoryQueryResponse;
using Service.Query;

namespace Service.LabelQuery
{
    public sealed class RepositoryQueryExecutor : IQueryExecutor<RepositoryQuery, Model.Owner>
    {        
        private readonly IStringDeserializer stringDeserializer;
        private readonly IMapper<Data, Model.Owner> mapper;
        private readonly HttpClient httpClient;

        private readonly string hostName = "github.chrobinson.com";        

        public RepositoryQueryExecutor(IStringDeserializer stringDeserializer, IMapper<Data, Model.Owner> mapper)
        {            
            this.stringDeserializer = stringDeserializer;
            this.mapper = mapper;
            this.httpClient = new HttpClient();

            var authorization = "";

            httpClient.DefaultRequestHeaders.Add("Authorization", $"bearer {authorization}");
        }

        public async Task<Model.Owner> ExecuteQuery(RepositoryQuery query, Action<int, int, long> progressReporter)
        {           
            var data = await ExecuteQueryStartAsync(query, progressReporter);

            return mapper.MapData(data);
        }

        private async Task<Data> ExecuteQueryStartAsync(RepositoryQuery query, Action<int, int, long> progressReporter)
        {
            var stopWatch = new Stopwatch();
            
            var data = await GetRepositoryPage(query, null);            

            var currentRepositoryPage = data.Organization.Repositories;

            var totalAmountOfPages = currentRepositoryPage.TotalCount / 100 + 1;
            var currentPage = 1;
            var hasNextPage = currentRepositoryPage.HasNextPage();

            if (!hasNextPage)
            {
                return data;
            }

            List<Edge> allEdges = currentRepositoryPage.Edges;
            var nextPageId = data.Organization.Repositories.NextPageId();
            while (hasNextPage)
            {                
                currentPage++;

                stopWatch.Restart();
                var moreData = await GetRepositoryPage(query, nextPageId);
                stopWatch.Stop();

                progressReporter.Invoke(totalAmountOfPages, currentPage, stopWatch.ElapsedMilliseconds);

                nextPageId = moreData.Organization.Repositories.NextPageId();
                hasNextPage = moreData.Organization.Repositories.HasNextPage();                                                

                allEdges.AddRange(moreData.Organization.Repositories.Edges);
            }

            data.Organization.Repositories.Edges = allEdges;

            return data;
        }

        private async Task<Data> GetRepositoryPage(RepositoryQuery query, string repoPage)
        {            
            var url = $"https://{hostName}/api/graphql";

            query.SetPageId(repoPage);

            var response = await httpClient.PostAsync(url, new StringContent(query.GetQueryAsString()));

            var body = await response.Content.ReadAsStringAsync();

            var rootObject = stringDeserializer.Deserialize<RootObject>(body);

            return rootObject.Data;
        }
    }
}
