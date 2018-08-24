namespace Service
{
    public interface IStringSerializer
    {
        string Serialize<T>(T item);
    }
}