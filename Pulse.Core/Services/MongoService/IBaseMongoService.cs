namespace Pulse.Core.Services
{
    using System.Security.Principal;

    public interface IBaseMongoService
    {
         IPrincipal PrincipalUser { get; set; }
    }
}
