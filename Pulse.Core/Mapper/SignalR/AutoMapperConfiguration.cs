namespace Pulse.Core.Mapper.SignalR
{
    using AutoMapper;
    using Common;
    using System;

    public static class AutoMapperConfiguration
    {
        public static void Config()
        {
            Mapper.Initialize(cfg => {
                cfg.AddProfile(new CommonMapper());
            });
        }
    }
}
