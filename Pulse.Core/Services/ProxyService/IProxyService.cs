namespace Pulse.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IProxyService: IDisposable
    {
        string Bearer { get; set; }

        Task<IDictionary<string, string>> EnsureApiTokenAsync(string userName, string password);

        Task<IDictionary<string, string>> RefreshApiTokenAsync(string refreshId);

        Task<HttpResponseMessage> GetAsync(string endpoint);

        Task<HttpResponseMessage> PostJsonAsync(string endpoint, object data);

        Task<HttpResponseMessage> PutJsonAsync(string endpoint, object data);

        Task<HttpResponseMessage> DeleteAsync(string endpoint);

    }
}
