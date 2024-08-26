using System.Net;
using System.Net.Mail;
using biz.avantco.Models.Email;
using biz.avantco.Services.Email;

namespace dal.avantco.Services.Email
{
    public class EmailService : IEmailService
    {
        public string SendEmail(EmailModel email)
        {
            var response = "";
            try
            {
                SmtpClient smtpClient = new SmtpClient("Smtp.Gmail.com", 587);
                // smtpClient.TargetName = "STARTTLS/smtp.office365.com";
                smtpClient.EnableSsl = true;
                smtpClient.Timeout = 10000;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("julian.munguia@zumit.tech", "Duv06766");
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("julian.munguia@zumit.tech", "Avantco");
                mailMessage.To.Add(email.To);
                mailMessage.Subject = email.Subject;
                mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                mailMessage.IsBodyHtml = email.IsBodyHtml;
                mailMessage.Body = email.Body;
                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                smtpClient.Send(mailMessage);

                response = "Correo enviado";
            }
            catch (Exception ex)
            {
                response = ex.ToString();
            }

            return response;
        }

        public string SendEmailAttach(EmailModelAttach email)
        {
            var response = "";
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = "Smtp.Gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.Timeout = 10000;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("julian.munguia@zumit.tech", "Duv06766");

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("julian.munguia@zumit.tech", "Avantco");
                mailMessage.To.Add(email.To);
                mailMessage.Subject = email.Subject;
                mailMessage.IsBodyHtml = email.IsBodyHtml;
                mailMessage.Body = email.Body;

                foreach (var data in email.File)
                {
                    //string path = HttpContext.Current.Server.MapPath();
                    string random = Path.GetFullPath(data.attach);
                    Attachment attachment;
                    attachment = new Attachment(random);
                    mailMessage.Attachments.Add(attachment);
                }

                smtpClient.Send(mailMessage);

                response = "Correo enviado";
            }
            catch (Exception ex)
            {
                response = ex.ToString();
            }

            return response;
        }
    }
}