namespace Pulse.Core.Services
{
    using System.Net.Http;
    using System.Net.Http.Headers;

    public class SignalRServerProxy : BaseProxy
    {
        protected override HttpClient GetHttpClient(bool isAuthorization = false)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = ServiceUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Bearer);
            return client;
        }
    }
}
