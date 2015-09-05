using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SampleHttpClientTests.Services
{
    public class GoogleClient
    {
        private readonly HttpClient _httpClient;

        public GoogleClient()
        {
            _httpClient = new HttpClient();
        }

        public GoogleClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetStringFromUrl(Uri uri)
        {
            var response = await _httpClient.GetAsync(uri.ToString());
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
