namespace Pulse.Core.Security.Identity
{
    using Microsoft.AspNet.Identity;
    using Settings;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;

    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            var ms = new MailMessage();
            ms.To.Add(new MailAddress(message.Destination));
            ms.Subject = message.Subject;
            ms.Body = message.Body;
            ms.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                //https://www.google.com/settings/u/1/security/lesssecureapps?pageId=none
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(ms);
            }
        }

    }
}
