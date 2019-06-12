namespace Pulse.Core.Security.OAuthProvider
{
    using Common.ResolverFactories;
    using Dto.Entity;
    using Microsoft.Owin.Security.Infrastructure;
    using Services;
    using Common.Helpers;
    using System;
    using System.Threading.Tasks;
    using Domain.Entity;

    public class PulseRefreshTokenProvider : IAuthenticationTokenProvider
    {
        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var refreshTokenId = Guid.NewGuid().ToString("n");

            if (context.Ticket == null)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                context.Response.ReasonPhrase = "invalid refresh token";
                return;
            }
            
            IRefreshTokenService refreshTokenService = ResolverFactory.GetService<RefreshTokenService>();

            var client = context.OwinContext.Get<Client>("oauth:client");

           var token = new RefreshTokenDto()
            {
                RefreshTokenId = UnitHelper.GetHash(refreshTokenId),
                Subject = context.Ticket.Identity.Name,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddDays(client.RefreshTokenLifeTime)
            };

            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;

            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

            token.ProtectedTicket = context.SerializeTicket();

            await refreshTokenService.CreateAsync(token);

            context.SetToken(refreshTokenId);
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            IRefreshTokenService refreshTokenService = ResolverFactory.GetService<RefreshTokenService>();

            string refreshTokenId = UnitHelper.GetHash(context.Token);

            var refreshToken = await refreshTokenService.FindAllRefreshTokensAsync(refreshTokenId);

            if (refreshToken != null)
            {
                context.DeserializeTicket(refreshToken.ProtectedTicket);

                await refreshTokenService.DeleteAsync(refreshToken.Id);
            }
        }
    }
}
