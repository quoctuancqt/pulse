namespace Pulse.Core.EmailTemplete
{
    using Model;
    public interface IProcessEmailTemplate
    {
        string GenerateEmailTemplate(object model);
    }
}
