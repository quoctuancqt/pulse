namespace Pulse.Core.Mapper.WebApi
{
    using AutoMapper;
    using Domain.Mongo;
    using System;
    using Dto.Mongo;
    using Pulse.Common.Helpers;
    using Domain.Mongo.Enum;

    public class MongoMaperProfile : Profile
    {
        public MongoMaperProfile()
        {
            #region Mongo To Dto
            CreateMap<NotifyKiosk, NotifyKioskDto>()
                   .AfterMap((src, dest) =>
                   {
                       dest.CountDate = src.CreateAt.ToLocalTime().CountDay();
                   });

            CreateMap<ClientUser, ClientUserDto>();

            CreateMap<UserActivities, UserActivitiesDto>()
               .AfterMap((src, dest) =>
               {
                   dest.CountDate = src.CreateAt.ToLocalTime().CountDay();
                   dest.ActionName = Enum.GetName(typeof(ActionType), src.Action);
               });

            CreateMap<SystemEvent, SystemEventDto>()
               .AfterMap((src, dest) =>
               {
                   dest.CountDate = src.CreateAt.ToLocalTime().CountDay();
                   dest.ActionName = Enum.GetName(typeof(ActionType), src.Action);
               });

            CreateMap<MongoKiosk, MongoKioskDto>();

            #endregion

            #region Dto To Mongo

            CreateMap<NotifyKioskDto, NotifyKiosk>()
                .ForMember(x => x.Id, x => x.Ignore());

            CreateMap<ClientUserDto, ClientUser>()
                .ForMember(x => x.Id, x => x.Ignore());

            CreateMap<UserActivitiesDto, UserActivities>()
                .ForMember(x => x.Id, x => x.Ignore());

            CreateMap<SystemEventDto, SystemEvent>()
                .ForMember(x => x.Id, x => x.Ignore());

            CreateMap<MongoKioskDto, MongoKiosk>()
                .ForMember(x => x.Id, x => x.Ignore());

            #endregion
        }
    }

}
