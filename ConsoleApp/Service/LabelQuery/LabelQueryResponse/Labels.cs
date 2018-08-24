using System.Collections.Generic;

namespace Service.LabelQuery.LabelQueryResponse
{
    public sealed class Labels : IPageable
    {
        public List<Edge> Edges { get; set; }
        public PageInfo PageInfo { get; set; }
        public int TotalCount { get; set; }
    }
}