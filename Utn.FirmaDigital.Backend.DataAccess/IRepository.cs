using System.Collections.Generic;
using System.Threading.Tasks;

namespace Utn.FirmaDigital.Backend.DataAccess.Repository
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> List(T entity);
        Task Save(T entity);
        Task Delete(T entity);
        Task<T> Get(T entity);
        System.Data.IDbConnection Connection { get; set; }
        System.Data.IDbTransaction Transaction { get; set; }
    }
}
