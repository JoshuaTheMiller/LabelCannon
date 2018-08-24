namespace Service
{
    public interface IStringDeserializer
    {
        T Deserialize<T>(string text);
    }
}