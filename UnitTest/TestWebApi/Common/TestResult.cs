namespace TestWebApi
{
    using Pulse.Common.Helpers;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Web.Http;

    public class TestResult<TDto>
    {
        public TDto Items { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public object Error { get; private set; }
        public HttpResponseMessage Response { get; private set; }
        public TestResult(IHttpActionResult actionResult)
        {
            Response = actionResult.ExecuteAsync(new CancellationToken(true)).Result;
            StatusCode = Response.StatusCode;
            if (StatusCode == HttpStatusCode.OK)
            {
                Items = Response.Content.ReadAsAsync<TDto>().Result;
            }
            else
            {
                if(Response.Content != null) Error = Response.Content.ReadAsAsync<object>().Result;
            }
        }
    }
}
