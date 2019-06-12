namespace Pulse.Core.Mapper.WebApi
{
    using AutoMapper;
    public static class AutoMapperConfiguration
    {
        public static void Config()
        {
            Mapper.Initialize(cfg => {
                cfg.AddProfile(new WebApiMapperProfile());
                cfg.AddProfile(new MongoMaperProfile());
            });
        }
    }
}
