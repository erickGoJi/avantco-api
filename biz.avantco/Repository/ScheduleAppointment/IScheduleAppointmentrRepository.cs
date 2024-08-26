using biz.avantco.Repository.Generic;

namespace biz.avantco.Repository.ScheduleAppointment
{
    public interface IScheduleAppointmentrRepository : IGenericRepository<Entities.ScheduleAppointment>
    {
        string SendMail(string emailTo, string body, string subject);
    }
}
