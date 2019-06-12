namespace Pulse.Core.Dto.Entity
{
    using Domain;
    public class ClientsCountriesDto : IDto
    {
        public int Id { get; set; }

        public string ClientId { get; set; }

        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
    }
}
