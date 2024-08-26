
using biz.avantco.Models.Email;

namespace biz.avantco.Services.Email
{
    public interface IEmailService
    {
        string SendEmail(EmailModel email);
        string SendEmailAttach(EmailModelAttach email);

    }
}
