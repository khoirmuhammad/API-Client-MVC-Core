using System.Net.Http;
using System.Net.Http.Headers;
using VoxTestApplication.Models.User;

namespace VoxTestApplication.Helpers
{
    public class HttpClientHelper<T> : IHttpClientHelper<T> where T : class
    {
        public async Task<HttpResponseMessage> DeleteAsync(string? accessToken, string apiUrl, T? body)
        {
            HttpResponseMessage? message = null;

            using (var httpClient = new HttpClient())
            {
                if (!string.IsNullOrEmpty(accessToken))
                {
                    httpClient.DefaultRequestHeaders.Authorization
                             = new AuthenticationHeaderValue("Bearer", accessToken);
                }

                message = await httpClient.DeleteAsync(apiUrl);
            }

            return message;
        }

        public async Task<HttpResponseMessage> GetAsync(string? accessToken, string apiUrl)
        {
            HttpResponseMessage? message = null;

            using (var httpClient = new HttpClient())
            {
                if (!string.IsNullOrEmpty(accessToken))
                {
                    httpClient.DefaultRequestHeaders.Authorization
                             = new AuthenticationHeaderValue("Bearer", accessToken);
                }
                
                message = await httpClient.GetAsync(apiUrl);
            }

            return message;
        }

        public async Task<HttpResponseMessage> PostAsync(string? accessToken, string apiUrl, T body)
        {
            HttpResponseMessage? message = null;

            using (var httpClient = new HttpClient())
            {
                if (!string.IsNullOrEmpty(accessToken))
                {
                    httpClient.DefaultRequestHeaders.Authorization
                             = new AuthenticationHeaderValue("Bearer", accessToken);
                }

                message = await httpClient.PostAsJsonAsync(apiUrl, body);
            }

            return message;
        }

        public async Task<HttpResponseMessage> PutAsync(string? accessToken, string apiUrl, T body)
        {
            HttpResponseMessage? message = null;

            using (var httpClient = new HttpClient())
            {
                if (!string.IsNullOrEmpty(accessToken))
                {
                    httpClient.DefaultRequestHeaders.Authorization
                             = new AuthenticationHeaderValue("Bearer", accessToken);
                }

                message = await httpClient.PutAsJsonAsync(apiUrl, body);
            }

            return message;
        }
    }
}
