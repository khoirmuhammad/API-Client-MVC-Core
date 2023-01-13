namespace VoxTestApplication.Helpers
{
    public interface IHttpClientHelper<T> where T : class
    {
        Task<HttpResponseMessage> GetAsync(string? accessToken, string apiUrl);
        Task<HttpResponseMessage> PostAsync(string? accessToken, string apiUrl, T body);
        Task<HttpResponseMessage> PutAsync(string? accessToken, string apiUrl, T body);
        Task<HttpResponseMessage> DeleteAsync(string? accessToken, string apiUrl, T? body);
    }
}
