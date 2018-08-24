using System.Text.RegularExpressions;

namespace Service.LabelQuery
{
    public sealed class RepositoryQuery
    {
        private readonly IStringSerializer stringSerializer;
        private readonly string query;
        private readonly string owner;
        private string pageId = null;        

        public RepositoryQuery(IStringSerializer stringSerializer, string query, string owner)
        {
            this.stringSerializer = stringSerializer;
            this.query = query;
            this.owner = owner;            
        }

        public void SetPageId(string pageId)
        {
            this.pageId = pageId;
        }

        public string GetQueryAsString()
        {
            return GetLabelQuery(owner, pageId);
        }

        private string GetLabelQuery(string orginaztionName, string repoPage)
        {
            var labelQuery = FormatQuery(query);

            var variables = new QueryVariables(orginaztionName, repoPage);

            var variablesAsString = stringSerializer.Serialize(variables);

            var body = $"{{\"query\":\"{labelQuery}\",\"variables\":{variablesAsString}}}";

            return body;
        }

        private string FormatQuery(string query)
        {
            var formattedQuery = query.Trim().Replace('\n', ' ');

            return Regex.Replace(formattedQuery, @"\s+", " ");
        }

        private sealed class QueryVariables
        {
            public QueryVariables(string organizationLogin, string repositoryPage)
            {
                OrganizationLogin = organizationLogin;
                RepositoryPage = repositoryPage;
            }

            public string OrganizationLogin { get; }
            public string RepositoryPage { get; }
        }
    }
}
