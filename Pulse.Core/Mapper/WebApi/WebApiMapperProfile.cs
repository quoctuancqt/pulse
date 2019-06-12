namespace Pulse.Core.Mapper.WebApi
{
    using AutoMapper;
    using Domain;
    using Domain.Entity;
    using Domain.Enum;
    using Dto.Entity;
    using Pulse.Common.Helpers;
    using System;

    public class WebApiMapperProfile: Profile
    {
        private const string FULL_NAME = "anonymous";

        public WebApiMapperProfile()
        {
            #region Enity To Dto
            CreateMap<Kiosk, KioskDto>()
                .AfterMap((src, dest) =>
            {
                dest.DateFormat = (src.UpdatedAt == null ? src.CreatedAt.FormatDate(): src.UpdatedAt.FormatDate());
                dest.DateFormat1 = (src.UpdatedAt == null ? src.CreatedAt.FormatDate("{0:dd/MM/yyyy}") : src.UpdatedAt.FormatDate("{0:dd/MM/yyyy}"));
                dest.StatusValue = Enum.GetName(typeof(KioskStatus), src.Status);
                dest.CountDate = (src.UpdatedAt == null ? src.CreatedAt.CountDay() : src.CreatedAt.CountDay());
            });
            CreateMap<KioskSecurity, KioskSecurityDto>();
            CreateMap<Group, GroupDto>();
            CreateMap<History, HistoryDto>()
              .AfterMap((src, dest) =>
              {
                  dest.FullName = string.IsNullOrEmpty(src.FullName) ? FULL_NAME : src.FullName;
                  dest.ProcessTypeValue = Enum.GetName(typeof(ProcessType), src.ProcessType);
                  dest.HistoryTypeValue = Enum.GetName(typeof(HistoryType), src.HistoryType);
                  dest.DateFormat = src.CreatedDate.FormatDate();
                  dest.CountDate = src.CreatedDate.CountDay();
              });
            CreateMap<UserProfile, UserProfileDto>()
                .AfterMap((src, dest) =>
                {
                    dest.Password = null;
                    dest.FullName = string.Format("{0} {1}", src.FirstName, src.LastName);
                });
            CreateMap<Country, CountryDto>();
            CreateMap<RefreshToken, RefreshTokenDto>();
            CreateMap<EncryptionUser, EncryptionUserDto>();
            CreateMap<Client, ClientDto>();
            CreateMap<ClientsCountries, ClientsCountriesDto>();
            #endregion

            #region Dto To Entity
            CreateMap<KioskDto, Kiosk>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.ClientId, x => x.Ignore())
                .ForMember(x => x.Country, x => x.Ignore())
                .ForMember(x => x.Group, x => x.Ignore())
                .ForMember(x => x.CreatedAt, x => x.Ignore())
                .ForMember(x => x.CreatedBy, x => x.Ignore())
                .ForMember(x => x.UpdatedAt, x => x.Ignore())
                .ForMember(x => x.UpdatedBy, x => x.Ignore());

            CreateMap<KioskSecurityDto, KioskSecurity>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.EncryptionUser, x => x.Ignore());

            CreateMap<GroupDto, Group>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.CreatedAt, x => x.Ignore())
                .ForMember(x => x.CreatedBy, x => x.Ignore())
                .ForMember(x => x.UpdatedAt, x => x.Ignore())
                .ForMember(x => x.UpdatedBy, x => x.Ignore());

            CreateMap<CountryDto, Country>()
                .ForMember(x => x.Id, x => x.Ignore());

            CreateMap<UserProfileDto, UserProfile>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.CreatedAt, x => x.Ignore())
                .ForMember(x => x.CreatedBy, x => x.Ignore())
                .ForMember(x => x.UpdatedAt, x => x.Ignore())
                .ForMember(x => x.UpdatedBy, x => x.Ignore());

            CreateMap<HistoryDto, History>().ForMember(x => x.Id, x => x.Ignore());
            
            CreateMap<RefreshTokenDto, RefreshToken>().ForMember(x => x.Id, x => x.Ignore());

            CreateMap<EncryptionUserDto, EncryptionUser>()
                .ForMember(x => x.Id, x => x.Ignore());

            CreateMap<ClientDto, Client>();
            CreateMap<ClientsCountriesDto, ClientsCountries>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.Country, x => x.Ignore());
            #endregion
        }
    }
}
