using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Service.LabelQuery.RepositoryQueryResponse;
using Service.Query;

namespace Service.LabelQuery
{
    public sealed class RepositoryQueryExecutor : IQueryExecutor<OrganizationRepositoryQuery, Model.Owner>
    {        
        private readonly IStringDeserializer stringDeserializer;        
        private readonly IWebClient webClient;
        private readonly IMapper<Data, Model.Owner> mapper;                      

        public RepositoryQueryExecutor(IStringDeserializer stringDeserializer, IMapper<Data, Model.Owner> mapper, IWebClient webClient)
        {            
            this.stringDeserializer = stringDeserializer;
            this.mapper = mapper;            
            this.webClient = webClient;                                    
        }

        public async Task<Model.Owner> ExecuteQuery(string hostName, OrganizationRepositoryQuery query, Action<int, int, long> progressReporter)
        {           
            var data = await ExecuteQueryStartAsync(hostName, query, progressReporter);

            return mapper.MapData(data);
        }

        private async Task<Data> ExecuteQueryStartAsync(string hostName, OrganizationRepositoryQuery query, Action<int, int, long> progressReporter)
        {
            var stopWatch = new Stopwatch();
            
            var data = await GetRepositoryPage(hostName, query, null);            

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
                var moreData = await GetRepositoryPage(hostName, query, nextPageId);
                stopWatch.Stop();

                progressReporter.Invoke(totalAmountOfPages, currentPage, stopWatch.ElapsedMilliseconds);

                nextPageId = moreData.Organization.Repositories.NextPageId();
                hasNextPage = moreData.Organization.Repositories.HasNextPage();                                                

                allEdges.AddRange(moreData.Organization.Repositories.Edges);
            }

            data.Organization.Repositories.Edges = allEdges;

            return data;
        }

        private async Task<Data> GetRepositoryPage(string hostName, OrganizationRepositoryQuery query, string repoPage)
        {            
            var url = $"https://{hostName}/api/graphql";

            query.SetPageId(repoPage);

            var response = await webClient.PostAsync<string, RootObject>(url, query.GetQueryAsString());

            return response.Data;
        }
    }
}
