namespace Pulse.Core.Services
{
    using log4net;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class RetryHandler : DelegatingHandler
    {
        public delegate Task<HttpResponseMessage> HandleGet(string endpoint);
        public delegate Task<HttpResponseMessage> HandlePost(string endpoint, object data);
        public event HandleGet OnGetAsync;
        public event HandleGet OnDeleteAsync;
        public event HandlePost OnPostAsync;
        public event HandlePost OnPutAsync;
        public delegate void HandleRefresh();
        public event HandleRefresh OnHandleRefresh;
        private static readonly ILog _log = LogManager.GetLogger(typeof(RetryHandler));

        public RetryHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        { }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            try
            {
                HttpResponseMessage response;

                response = await base.SendAsync(request, cancellationToken);

                if (response.StatusCode != HttpStatusCode.Unauthorized)
                {
                    return response;
                }

                if (OnHandleRefresh != null) OnHandleRefresh();

                if (request.Method == HttpMethod.Get && OnGetAsync != null)
                {
                    response = await OnGetAsync(request.RequestUri.LocalPath);
                }

                if (request.Method == HttpMethod.Post && OnPostAsync != null)
                {
                    response = await OnPostAsync(request.RequestUri.LocalPath, GetData(request));
                }

                if (request.Method == HttpMethod.Put && OnPutAsync != null)
                {
                    response = await OnPutAsync(request.RequestUri.LocalPath, GetData(request));
                }

                if (request.Method == HttpMethod.Delete && OnDeleteAsync != null)
                {
                    response = await OnDeleteAsync(request.RequestUri.LocalPath);
                }

                return response;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw new Exception("Please check error log: " + ex.Message);
            }
            
        }

        private object GetData(HttpRequestMessage request)
        {
            var httpContent = request.Content;
            return httpContent.ReadAsStringAsync().Result;
        }
    }
}
