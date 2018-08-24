using System.Collections.Generic;

namespace Service.LabelQuery.RepositoryQueryResponse
{
    public sealed class Repositories : IPageable
    {
        public List<Edge> Edges { get; set; }
        public PageInfo PageInfo { get; set; }
        public int TotalCount { get; set; }        
    }
}