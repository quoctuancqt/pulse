namespace Pulse.Core.Services
{
    using Common.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public class ProxyService : BaseProxy
    {
        protected override HttpClient GetHttpClient(bool isAuthorization = false)
        {
            var retryHandler = new RetryHandler(new HttpClientHandler());
            retryHandler.OnGetAsync += GetAsync;
            retryHandler.OnDeleteAsync += DeleteAsync;
            retryHandler.OnPostAsync += PostJsonAsync;
            retryHandler.OnPutAsync += PutJsonAsync;
            retryHandler.OnHandleRefresh += RefreshToken;

            HttpClient client = new HttpClient(retryHandler);
            client.BaseAddress = ServiceUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (isAuthorization)
            {
                if (Bearer == null) BindBearer();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Bearer);
            }

            return client;
        }

        private void BindBearer()
        {
            Bearer = KioskConfigurationHepler.GetValueFromSecurity("Token");
            if (string.IsNullOrEmpty(Bearer))
            {
                var result = ProcessLoginAsync().Result;
                if (result != null) Bearer = result["access_token"];
            }
        }

        private void RefreshToken()
        {
            string refreshToken = KioskConfigurationHepler.GetValueFromSecurity("RefreshToken");
            try
            {
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    var result = RefreshApiTokenAsync(refreshToken).Result;
                    KioskConfigurationHepler.UpdateValueForElement("Token", result["access_token"]);
                    KioskConfigurationHepler.UpdateValueForElement("RefreshToken", result["refresh_token"]);
                    Bearer = result["access_token"];
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                KioskConfigurationHepler.UpdateValueForElement("Token", "");
                BindBearer();
                throw new Exception("There is an exception that was happening at server when refreshing token. Please check the error log: " + ex.Message);
            }
        }

        private async Task<IDictionary<string, string>> ProcessLoginAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(KioskConfigurationHepler.GetValueFromSecurity("UserEncrypted")))
                {

                    var user = GetUserInfos();

                    var result = await EnsureApiTokenAsync(user[0], user[1]);

                    if (result.ContainsKey("error"))
                    {
                        _log.Error("ProxyService" + result["error"]);

                        throw new Exception("ProxyService ==> ProcessLogin got Exception please check log" + result["error"]);
                    }

                    KioskConfigurationHepler.UpdateValueForElement("Token", result["access_token"]);

                    KioskConfigurationHepler.UpdateValueForElement("RefreshToken", result["refresh_token"]);

                    return result;
                }

                return null;
            }
            catch (Exception ex)
            {
                _log.Error("ProxyService" + ex);
                throw new Exception("ProxyService ==> ProcessLogin got Exception please check log", ex);
            }
        }

        private string[] GetUserInfos()
        {
            string userEncrypted = KioskConfigurationHepler.GetValueFromSecurity("UserEncrypted");
            string saltKey = KioskConfigurationHepler.GetValueFromSecurity("SaltKey");
            string viKey = KioskConfigurationHepler.GetValueFromSecurity("VIKey");
            string passwordHash = KioskConfigurationHepler.GetValueFromSecurity("PasswordHash");

            return UnitHelper.Decrypt(userEncrypted, passwordHash, saltKey, viKey).Split('&');
        }

    }
}
