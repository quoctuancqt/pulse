namespace Pulse.Core.Security.OAuthProvider
{
    using Identity.IdentityModels;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.OAuth;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity.Owin;
    using Identity;
    using Common.ResolverFactories;
    using Services;
    using System.Data.Entity;
    using Domain.Entity;
    using System.Linq;
    using Dto.Entity;

    public class PulseOAuthProvider : OAuthAuthorizationServerProvider
    {
        #region override
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {

            try
            {
                string clientId;
                string clientSecret;

                if (context.TryGetBasicCredentials(out clientId, out clientSecret))
                {
                    PulseUserManager userManager = context.OwinContext.GetUserManager<PulseUserManager>();

                    IUnitOfWork unitOfWork = ResolverFactory.GetService<IUnitOfWork>();

                    try
                    {
                        var client = await unitOfWork.Clients.FindAll(c => c.ClientId.Equals(clientId)).FirstOrDefaultAsync();

                        if (client != null &&
                            userManager.PasswordHasher.VerifyHashedPassword(
                                client.Secret, clientSecret) == PasswordVerificationResult.Success)
                        {
                            context.OwinContext.Set("oauth:client", client);

                            string userName = context.Parameters.GetValues("username")[0];

                            var user = await userManager.FindByClientIdAndNameAsync(client.ClientId, userName);

                            if (userManager.IsInRole(user.Id, PulseIdentityRole.Kiosk))
                            {
                                context.Options.AccessTokenExpireTimeSpan = TimeSpan.FromDays(365);
                            }
                            else
                            {
                                context.Options.AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(client.TokenLifeTime);
                            }

                            context.Validated(clientId);
                        }
                        else
                        {
                            context.SetError("invalid_client", "Client credent``ials are invalid.");
                            context.Rejected();
                        }
                    }
                    catch
                    {
                        context.SetError("server_error");
                        context.Rejected();
                    }
                }
                else
                {
                    context.SetError(
                        "invalid_client",
                        "Client credentials could not be retrieved through the Authorization header.");

                    context.Rejected();
                }
            }
            catch (Exception ex)
            {
                context.SetError("ValidateClientAuthentication " + ex.Message);
            }

        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

                PulseUserManager userManager = context.OwinContext.GetUserManager<PulseUserManager>();

                PulseIdentityUser user = await userManager.FindAsync(context.UserName, context.Password);

                var userProfileService = ResolverFactory.GetService<IUserProfileService>();

            if (user == null)
            {
                context.Rejected();

                    context.SetError("Invalid username or password.");

                    return;
                }

            var userProfile = await userProfileService.FindByUserIdAsync(user.Id);

            var authClaimIdentity = await userManager.CreateIdentityAsync(user, OAuthDefaults.AuthenticationType);

                await SaveClaimsAsync(authClaimIdentity, user, userManager);

                var client = context.OwinContext.Get<Client>("oauth:client");

                var roles = await userManager.GetRolesAsync(user.Id);

                var properties = new UserProperties
                {
                    UserName = authClaimIdentity.Name,
                    ClientId = user.ClientId,
                    FullName = userProfile == null ? string.Empty : (string.IsNullOrEmpty(userProfile.FullName.Trim()) ? userProfile.Email : userProfile.FullName),
                    ClientName = client.Name,
                    Role = roles.FirstOrDefault(),
                    AvatarPath = userProfile == null ? string.Empty : (userProfile.AvatarPath == null ? string.Empty : userProfile.AvatarPath),
                    EmailConfirm = user.EmailConfirmed
                };

                var ticket = new AuthenticationTicket(authClaimIdentity,
                    CreateProperties(properties)
                    );

                context.Validated(ticket);
            }
            catch (Exception ex)
            {
                context.SetError("GrantResourceOwnerCredentials " + ex.Message);

                return;
            }

        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);

            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
        #endregion

        #region Private Method

        private AuthenticationProperties CreateProperties(UserProperties properties)
        {
            //TODO
            var data = new Dictionary<string, string>
            {
                { "userName", properties.UserName },
                { "clientId", properties.ClientId },
                { "clientName", properties.ClientName },
                { "fullName", properties.FullName },
                { "role", properties.Role },
                { "avatarPath", properties.AvatarPath },
                { "emailConfirm", properties.EmailConfirm == true ? "1" : "0" }
            };

            var result = new AuthenticationProperties(data);

            return result;
        }

        private async Task SaveClaimsAsync(ClaimsIdentity identity, PulseIdentityUser user, PulseUserManager userManager)
        {
            foreach (var claim in identity.Claims)
            {
                if (identity.HasClaim(claim.Type, claim.Value))
                {
                    await userManager.RemoveClaimAsync(user.Id, claim);
                }
                await userManager.AddClaimAsync(user.Id, claim);
            }
        }
        #endregion
    }
}
