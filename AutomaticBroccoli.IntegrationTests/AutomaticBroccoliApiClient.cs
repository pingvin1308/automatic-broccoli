using System.Net.Http.Json;
using AutomaticBroccoli.API.Contracts;

namespace AutomaticBroccoli.IntegrationTests
{
    public class AutomaticBroccoliApiClient
    {
        private readonly HttpClient _httpClient;

        public AutomaticBroccoliApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetOpenLoopsResponse> GetOpenLoops(int id)
        {
            var response = await _httpClient
                .GetFromJsonAsync<GetOpenLoopsResponse>($"openLoops/{id}");
            Assert.NotNull(response);
            return response;
        }
    }
}