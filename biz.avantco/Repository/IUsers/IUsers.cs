using biz.avantco.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.avantco.Repository.IUsers
{
    public interface IUsers : IGenericRepository<Entities.Users>
    {
        string HashPassword(string password);
        bool VerifyPassword(string hash, string password);
        string SendMail(string emailTo, string body, string subject);
    }
}
