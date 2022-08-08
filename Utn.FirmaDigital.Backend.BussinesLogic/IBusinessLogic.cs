using System;
using System.Threading.Tasks;

namespace Utn.FirmaDigital.Backend.BussinesLogic
{

    public interface IBusinessLogic : IDisposable
    {
        Task<Common.Message> DoWork(Common.Message message);
        Task<Common.Message> List(Common.Message message);
        Task<Common.Message> Get(Common.Message message);
        Task<Common.Message> Save(Common.Message message);
        Task<Common.Message> Delete(Common.Message message);
    }

}