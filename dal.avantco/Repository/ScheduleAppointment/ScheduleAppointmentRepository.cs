

using biz.avantco.Models.Email;
using biz.avantco.Repository.ScheduleAppointment;
using biz.avantco.Services.Email;
using dal.avantco.DBContext;
using dal.avantco.Repository.Generic;
using Microsoft.Extensions.Configuration;

namespace dal.avantco.Repository.User
{
    public class ScheduleAppointmentRepository : GenericRepository<biz.avantco.Entities.ScheduleAppointment>, IScheduleAppointmentrRepository
    {
        private IConfiguration _config;
        private IEmailService _emailservice;

        public ScheduleAppointmentRepository(AvantcoContext context, IConfiguration config, IEmailService emailService) : base(context)
        {
            _config = config;
            _emailservice = emailService;
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