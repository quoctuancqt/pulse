namespace Pulse.Core.SignalR.Pipeline
{
    using System;
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;

    public class PulseHubPipelineModule: HubPipelineModule
    {
        public override Func<HubDescriptor, IRequest, bool> BuildAuthorizeConnect(Func<HubDescriptor, IRequest, bool> authorizeConnect)
        {
            return base.BuildAuthorizeConnect(authorizeConnect);
        }

        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            if (exceptionContext.Error.InnerException != null)
            {
            }
            base.OnIncomingError(exceptionContext, invokerContext);
        }

        protected override bool OnBeforeIncoming(IHubIncomingInvokerContext context)
        {
            return base.OnBeforeIncoming(context);
        }

        protected override bool OnBeforeOutgoing(IHubOutgoingInvokerContext context)
        {
            return base.OnBeforeOutgoing(context);
        }
    }
}
