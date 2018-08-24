namespace Service.LabelQuery
{
    public interface IRepositoryQueryFactory
    {
        RepositoryQuery GetQuery(string orginaztionName);
    }
}