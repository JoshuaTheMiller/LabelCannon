using Newtonsoft.Json;

namespace Service
{
    public sealed class StringSerializer : IStringSerializer
    {
        public string Serialize<T>(T item)
        {
            return JsonConvert.SerializeObject(item);
        }
    }
}
