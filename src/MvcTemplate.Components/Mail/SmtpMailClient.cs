using System;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace MvcTemplate.Components.Mail
{
    public class SmtpMailClient : IMailClient
    {
        private SmtpClient Client { get; set; }
        private Boolean Disposed { get; set; }
        private String Sender { get; set; }

        public SmtpMailClient()
        {
            Sender = ((SmtpSection)WebConfigurationManager.GetSection("system.net/mailSettings/smtp")).From;
            Client = new SmtpClient();
        }

        public Task SendAsync(String email, String subject, String body)
        {
            MailMessage mail = new MailMessage(Sender, email, subject, body);
            mail.SubjectEncoding = Encoding.UTF8;
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = true;

            return Client.SendMailAsync(mail);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(Boolean disposing)
        {
            if (Disposed) return;

            Client.Dispose();

            Disposed = true;
        }
    }
}
