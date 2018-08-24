namespace Service.LabelQuery
{
    public static class PageableExtensions
    {
        public static bool HasNextPage(this IPageable pageable)
        {
            return pageable.PageInfo.HasNextPage;
        }

        public static string NextPageId(this IPageable pageable)
        {
            return pageable.PageInfo.EndCursor;
        }        
    }
}
