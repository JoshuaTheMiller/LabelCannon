namespace Service.Query
{
    public interface IMapper<TFrom, TTo>
    {
        TTo MapData(TFrom data);
    }
}