using System;
using System.Threading.Tasks;

namespace MvcTemplate.Components.Mail
{
    public interface IMailClient : IDisposable
    {
        Task SendAsync(String email, String subject, String body);
    }
}
