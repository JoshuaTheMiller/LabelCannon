namespace Service.LabelQuery
{
    public interface IPageable
    {
        PageInfo PageInfo { get; set; }
        int TotalCount { get; set; }
    }
}