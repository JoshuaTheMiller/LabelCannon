using System.Net.Http;
using System.Threading.Tasks;

namespace Service
{
    public sealed class WebClient : IWebClient
    {
        private readonly IStringSerializer stringSerializer;
        private readonly IStringDeserializer stringDeserializer;
        private readonly IConfigurationProvider configurationProvider;        

        public WebClient(IStringSerializer stringSerializer, IStringDeserializer stringDeserializer, IConfigurationProvider configurationProvider)
        {
            this.stringSerializer = stringSerializer;
            this.stringDeserializer = stringDeserializer;            
            this.configurationProvider = configurationProvider;
        }

        public async Task<TResponse> PostAsync<TBody, TResponse>(string url, TBody body)
        {
            var httpClient = new HttpClient();
            var authorization = configurationProvider.GetConfigurationValue("authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"bearer {authorization}");

            string bodyAsString = body as string;

            if(bodyAsString == null)
            {
                bodyAsString = stringSerializer.Serialize(body);
            }

            var response = await httpClient.PostAsync(url, new StringContent(bodyAsString));

            var responseBody = await response.Content.ReadAsStringAsync();

            var responseContent = stringDeserializer.Deserialize<TResponse>(responseBody);

            return responseContent;
        }
    }
}
