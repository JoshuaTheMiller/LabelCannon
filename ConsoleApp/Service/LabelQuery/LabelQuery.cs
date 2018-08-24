using System.Text.RegularExpressions;

namespace Service.LabelQuery
{
    public sealed class LabelQuery
    {
        private readonly IStringSerializer stringSerializer;
        private readonly string queryText;
        private readonly string owner;
        private readonly string repository;
        private string pageId = null;

        public LabelQuery(IStringSerializer stringSerializer, string queryText, string owner, string repository, string pageId)
        {
            this.stringSerializer = stringSerializer;
            this.queryText = queryText;
            this.owner = owner;
            this.repository = repository;
            this.pageId = pageId;
        }

        public void SetPageId(string pageId)
        {
            this.pageId = pageId;
        }

        public string GetQueryAsString()
        {
            return GetLabelQuery(owner, repository, pageId);
        }

        private string GetLabelQuery(string owner, string repository, string pageId)
        {
            var labelQuery = FormatQuery(queryText);

            var variables = new QueryVariables(owner, repository, pageId);

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
            public QueryVariables(string owner, string repository, string labelPage)
            {
                Owner = owner;
                Repository = repository;
                LabelPage = labelPage;
            }

            public string Owner { get; }
            public string Repository { get; }
            public string LabelPage { get; }
        }
    }
}
