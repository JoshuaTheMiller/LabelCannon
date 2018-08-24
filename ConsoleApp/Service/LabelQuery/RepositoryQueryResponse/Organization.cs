namespace Service.LabelQuery.RepositoryQueryResponse
{
    public sealed class Organization
    {
        public Repositories Repositories { get; set; }
        public string Name { get; }
    }
}