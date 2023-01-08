using System.Net.Http.Json;
using AutoFixture;
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

        public async Task<string> CreateAttachment(byte[] file, string name)
        {
            var fileContent = new ByteArrayContent(file);
            var fileName = new StringContent(name);
            var content = new MultipartFormDataContent
            {
                { fileContent, "file", name },
                { fileName, "name" }
            };

            var response = await _httpClient.PostAsync("inbox/attachments", content);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadAsStringAsync();
        }

        internal Task<object> GetAttachment(string path)
        {
            throw new NotImplementedException();
        }
    }
}