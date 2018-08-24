namespace Service.LabelQuery.RepositoryQueryResponse
{
    public sealed class Node
    {
        public Owner Owner { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public Labels Labels { get; set; }
    }
}