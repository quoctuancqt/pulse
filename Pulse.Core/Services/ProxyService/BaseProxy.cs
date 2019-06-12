namespace Pulse.Core.Services
{
    using log4net;
    using Newtonsoft.Json.Linq;
    using Settings;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public abstract class BaseProxy : IProxyService
    {
        protected Uri ServiceUri { get; set; }

        public string Bearer { get; set; }

        protected static readonly ILog _log = LogManager.GetLogger(typeof(BaseProxy));

        public BaseProxy()
        {
            ServiceUri = new Uri(SettingsConfigurationCommon.WEBAPI_URI);
        }

        public virtual async Task<IDictionary<string, string>> EnsureApiTokenAsync(string userName, string password)
        {
            using (var client = GetHttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", await GenerateClientHeaderAsync(userName));

                HttpResponseMessage response = await client.PostAsync("token", new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type", "password" },
                    { "username", userName },
                    { "password", password }
                }));

                JObject result = JObject.Parse(await response.Content.ReadAsStringAsync());

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();

                    dic.Add("error", (string)result.Property("error").Value);

                    return dic;
                }

                return MapData(result);
            }
        }

        public virtual async Task<IDictionary<string, string>> RefreshApiTokenAsync(string refreshId)
        {
            using (var client = GetHttpClient())
            {
                string userName = await GetUserNameByRefreshIdAsync(refreshId);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", await GenerateClientHeaderAsync(userName));

                HttpResponseMessage response = await client.PostAsync("token", new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "refresh_token", refreshId },
                    { "username", userName },
                    { "grant_type", "refresh_token" }
                }));

                JObject result = JObject.Parse(await response.Content.ReadAsStringAsync());

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();

                    dic.Add("error", (string)result.Property("error").Value);

                    return dic;
                }

                return MapData(result);
            }
        }

        public virtual async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            using (var client = GetHttpClient(SettingsConfigurationCommon.IS_AUTHENTICATION))
            {
                return await client.GetAsync(endpoint);
            }
        }

        public virtual async Task<HttpResponseMessage> PostJsonAsync(string endpoint, object data)
        {
            using (var client = GetHttpClient(SettingsConfigurationCommon.IS_AUTHENTICATION))
            {
                return await client.PostAsJsonAsync(endpoint, data);
            }
        }

        public virtual async Task<HttpResponseMessage> PutJsonAsync(string endpoint, object data)
        {
            using (var client = GetHttpClient(SettingsConfigurationCommon.IS_AUTHENTICATION))
            {
                return await client.PutAsJsonAsync(endpoint, data);
            }
        }

        public virtual async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            using (var client = GetHttpClient(SettingsConfigurationCommon.IS_AUTHENTICATION))
            {
                return await client.DeleteAsync(endpoint);
            }
        }

        public void Dispose()
        {
            //TODO            
        }

        #region protected Method

        protected abstract HttpClient GetHttpClient(bool isAuthorization = false);
        
        protected virtual IDictionary<string, string> MapData(JObject result)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            dic.Add("access_token", (string)result.Property("access_token").Value);
            dic.Add("token_type", (string)result.Property("token_type").Value);
            dic.Add("expires_in", (string)result.Property("expires_in").Value);
            dic.Add("refresh_token", (string)result.Property("refresh_token").Value);
            dic.Add("userName", (string)result.Property("userName").Value);
            dic.Add("clientId", (string)result.Property("clientId").Value);
            dic.Add("clientName", (string)result.Property("clientName").Value);
            dic.Add("role", (string)result.Property("role").Value);
            dic.Add("fullName", (string)result.Property("fullName").Value);
            dic.Add("issued", (string)result.Property(".issued").Value);
            dic.Add("expires", (string)result.Property(".expires").Value);
            dic.Add("avatarPath", (string)result.Property("avatarPath").Value);
            dic.Add("emailConfirm", (string)result.Property("emailConfirm").Value);

            return dic;
        }

        #endregion

        #region private Method

        private async Task<string> GenerateClientHeaderAsync(string userName)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = ServiceUri;

                var response = await client.GetAsync("api/clients/generateclientheader/" + userName);

                if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadAsAsync<string>();

                throw new Exception("GenerateClientHeaderAsync fail with username: " + userName);
            }
        }

        private async Task<string> GetUserNameByRefreshIdAsync(string refreshId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = ServiceUri;

                var response = await client.GetAsync("api/oauths/getusernamebyrefreshId/" + refreshId);

                if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadAsAsync<string>();

                throw new Exception("GetUserNameByRefreshIdAsync fail with refreshId: " + refreshId);
            }
        }

        #endregion
    }
}
