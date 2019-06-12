namespace Pulse.Domain.Entity
{
    public class ClientsCountries : IEntity
    {
        public int Id { get; set; }

        public string ClientId { get; set; }

        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

    }
}
