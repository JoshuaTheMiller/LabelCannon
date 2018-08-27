namespace Service.LabelQuery
{
    public interface IRepositoryQueryFactory
    {
        OrganizationRepositoryQuery GetQuery(string orginaztionName);
    }
}