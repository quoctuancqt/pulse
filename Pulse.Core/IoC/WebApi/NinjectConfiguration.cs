namespace Pulse.Core.IoC.WebApi
{
    using Ninject;
    using Services;
    using Common.ResolverFactories;
    using Connection.Entity;
    using Ninject.Web.Common;
    using System.Web;
    using Microsoft.AspNet.Identity;
    using Security.Identity.IdentityModels;
    using Microsoft.Owin.Security;
    using Security.Identity;
    using System.Reflection;
    using System;
    using HandlerEvent;
    using EmailTemplete;

    public static class NinjectConfiguration
    {
        public static IKernel CreateKernel()
        {
            IKernel kernel = new StandardKernel();
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            #region Services
            kernel.Bind<UserManager<PulseIdentityUser>>().To<PulseUserManager>().InRequestScope();
            kernel.Bind<RoleManager<PulseIdentityRole>>().To<PulseRoleManager>().InRequestScope();
            kernel.Bind<IAuthenticationManager>().ToMethod(ctx => HttpContext.Current.GetOwinContext().Authentication);
            kernel.Bind<IUserStore<PulseIdentityUser>>().To<PulseUserStore>().InRequestScope();
            kernel.Bind<IRoleStore<PulseIdentityRole>>().To<PulseRoleStore>().InRequestScope();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
            kernel.Bind<IKioskService>().To<KioskService>();
            kernel.Bind<ICountryService>().To<CountryService>();
            kernel.Bind<IGroupService>().To<GroupService>();
            kernel.Bind<IProxyService>().To<ProxyService>();
            kernel.Bind<IHistoryService>().To<HistoryService>();
            kernel.Bind<IRefreshTokenService>().To<RefreshTokenService>();
            kernel.Bind<IUserProfileService>().To<UserProfileService>();
            kernel.Bind<IEncryptionUserService>().To<EncryptionUserService>();
            kernel.Bind<IKioskSecurityService>().To<KioskSecurityService>();
            kernel.Bind<IClientService>().To<ClientService>();
            kernel.Bind<IClientsCountriesService>().To<ClientsCountriesService>();
            kernel.Bind<IMongoService>().To<MongoService>().InRequestScope();
            kernel.Bind<IIdentityMessageService>().To<EmailService>();
            kernel.Bind<IMongoKioskService>().To<MongoKioskService>();
            #endregion
            #region Process Email
            kernel.Bind<IProcessEmailTemplate>().To<SimpleEmailTemplate>();
            #endregion
            kernel.Bind<EventsBase>().To<SignalREventHandlers>();
            kernel.Bind<HttpContext>().ToMethod(context => HttpContext.Current).InRequestScope();
            kernel.Bind<HttpContextBase>().ToMethod(context => new HttpContextWrapper(HttpContext.Current)).InRequestScope();
            kernel.Load(Assembly.GetExecutingAssembly());
            ResolverFactory.SetKernel(kernel);

            return kernel;
        }
    }
}
