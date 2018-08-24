using System.Collections.Generic;

namespace Service.LabelQuery.RepositoryQueryResponse
{
    public sealed class Labels : IPageable
    {
        public List<Edge2> Edges { get; set; }
        public PageInfo PageInfo { get; set; }
        public int TotalCount { get; set; }
    }
}