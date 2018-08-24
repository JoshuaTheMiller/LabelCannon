namespace Service.LabelQuery
{
    public sealed class RepositoryQueryFactory : IRepositoryQueryFactory
    {
        private readonly ITextResourceProvider repositoryPageQueryProvider;
        private readonly IStringSerializer stringSerializer;

        private readonly string queryResourceKey = "Service.LabelQuery.repositoryPageQuery.qraphql";

        public RepositoryQueryFactory(ITextResourceProvider repositoryPageQueryProvider, IStringSerializer stringSerializer)
        {
            this.repositoryPageQueryProvider = repositoryPageQueryProvider;
            this.stringSerializer = stringSerializer;
        }

        public RepositoryQuery GetQuery(string orginaztionName)
        {
            var labelQuery = repositoryPageQueryProvider.GetResource(queryResourceKey);

            return new RepositoryQuery(stringSerializer, labelQuery, orginaztionName);            
        }
    }
}
