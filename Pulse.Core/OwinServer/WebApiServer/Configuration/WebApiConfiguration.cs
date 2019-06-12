namespace Pulse.Core.WeApiServer
{
    using Owin;
    using Microsoft.Owin.Cors;
    using System.Web.Http;
    using OwinServer.WebApiServer.Configuration;
    using IoC.WebApi;
    using Mapper.WebApi;
    using Microsoft.Owin.Security.OAuth;
    using Security.OAuthProvider;
    using Microsoft.Owin;
    using Connection.Entity;
    using Security.Identity;

    public class WebApiConfiguration
    {
        public void Configuration(IAppBuilder app)
        {
            AutoMapperConfiguration.Config();

            app.CreatePerOwinContext(PulseContext.Create);
            app.CreatePerOwinContext<PulseUserManager>(PulseUserManager.Create);
            ConfigureOAuth(app);

            var config = new HttpConfiguration();


            config.MapHttpAttributeRoutes();

            config.EnableCors();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.DependencyResolver = new NinjectResolver(NinjectConfiguration.CreateKernel());

            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            app.UseCors(CorsOptions.AllowAll);

            app.UseWebApi(config);
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            var oAuthAuthorizationServerOptions = new OAuthAuthorizationServerOptions()
            {
                TokenEndpointPath = new PathString("/token"),
                Provider = new PulseOAuthProvider(),
                AccessTokenProvider = new PulseAccessTokenProvider(),
                RefreshTokenProvider = new PulseRefreshTokenProvider(),
                AllowInsecureHttp = true,
            };
            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthAuthorizationServerOptions);
            app.UseOAuthBearerTokens(oAuthAuthorizationServerOptions);
        }
    }
}
