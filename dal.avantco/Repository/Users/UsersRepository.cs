using biz.avantco.Models.Email;
using biz.avantco.Repository.IUsers;
using biz.avantco.Services.Email;
using dal.avantco.DBContext;
using dal.avantco.Repository.Generic;
using Microsoft.Extensions.Configuration;
using System.Web.Helpers;

namespace dal.avantco.Repository.Users
{
    public class UsersRepository : GenericRepository<biz.avantco.Entities.Users>, IUsers
    {
        private IConfiguration _config;
        private IEmailService _emailservice;

        public UsersRepository(AvantcoContext context, IConfiguration config, IEmailService emailService) : base(context)
        {
            _config = config;
            _emailservice = emailService;
        }

        public bool VerifyPassword(string hash, string password)
        {
            return Crypto.VerifyHashedPassword(hash, password);
        }


        public string HashPassword(string password)
        {
            return Crypto.HashPassword(password);
        }


        public string SendMail(string emailTo, string body, string subject)
        {
            EmailModel email = new EmailModel();
            email.To = emailTo;
            email.Subject = subject;
            email.Body = body;
            email.IsBodyHtml = true;

            return _emailservice.SendEmail(email);
        }
    }
}
