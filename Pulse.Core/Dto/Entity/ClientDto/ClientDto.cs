namespace Pulse.Core.Dto.Entity
{
    using Domain.Enum;
    using FluentValidation.Attributes;

    [Validator(typeof(ClientDtoValidator))]
    public class ClientDto: IDto
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public string Email { get; set; }
        public string Secret { get; set; }
        public string SecretKey { get; set; }
        public string Name { get; set; }
        public string SignalrUrl { get; set; }
        public string MongoName { get; set; }
        public string MongoConnectionString { get; set; }
        public bool Active { get; set; }
        public int RefreshTokenLifeTime { get; set; }
        public int TokenLifeTime { get; set; }
        public OAuthGrant AllowedGrant { get; set; }
        public string AllowedOrigin { get; set; }
    }
}
