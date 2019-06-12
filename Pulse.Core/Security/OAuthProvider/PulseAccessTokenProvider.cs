namespace Pulse.Core.Security.OAuthProvider
{
    using Microsoft.Owin.Security.Infrastructure;
    public class PulseAccessTokenProvider : AuthenticationTokenProvider
    {
        public override void Create(AuthenticationTokenCreateContext context)
        {
            context.SetToken(context.SerializeTicket());
        }

        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            base.Receive(context);
        }
    }
}
