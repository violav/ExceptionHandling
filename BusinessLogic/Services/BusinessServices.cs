using BusinessLogic.Options;
using Data.Data.EF.Context;
using Data.Data.EF.Output;
using Microsoft.Extensions.Options;
using Vi.Service.Exception.Extensions;

namespace BusinessLogic.Services
{
    public class BusinessServices
    {
        private readonly NorthwindContext _dbContext;
        private readonly IOptions<BusinessOptions> _conf;

        public BusinessServices(NorthwindContext dbContext, IOptions<BusinessOptions> conf)
        {
            _dbContext = dbContext;
            _conf = conf;
        }

        public async Task<IEnumerable<Category>> GetData()
        {
            var dati = _dbContext
                .Categories
                .AsEnumerable();
            return dati;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            var dati = _dbContext
                .QuerySqlDirectAsync<Category>("Select top(5) * from Categories", null);
            return await dati;
        }

        public async Task<IEnumerable<Category>> ThrowErrors(int value)
        {
            var exc = new System.Exception();
            exc.Data.Add(BusinessLogic.DomainDataError.BusinessError.BusinessMatErrors.CategoryAlreadyExists);
            exc.Data.Add(BusinessLogic.DomainDataError.BusinessError.BusinessMatErrors.CategoryAlreadyExists2);
            throw exc;
        }

        internal int GetNumber()
        {
            var tt = _conf.Value.Occurrences;
            return tt;
        }
    }
}
