using Newtonsoft.Json;

namespace Service
{
    public sealed class StringDeserializer : IStringDeserializer
    {
        public T Deserialize<T>(string text)
        {
            return JsonConvert.DeserializeObject<T>(text);
        }
    }
}
