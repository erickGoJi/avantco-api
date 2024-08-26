using biz.avantco.Repository.NewLetter;
using dal.avantco.DBContext;
using dal.avantco.Repository.Generic;
using Microsoft.Extensions.Configuration;


namespace dal.avantco.Repository.NewsLetter
{
    public class NewsLetterRepository : GenericRepository<biz.avantco.Entities.Newsletter>, INewsLetter
    {
        private IConfiguration _config;

        public NewsLetterRepository(AvantcoContext context, IConfiguration config) : base(context)
        {
            _config = config;
        }
    }
}