using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyModel;
namespace Utn.FirmaDigital.Backend.BussinesLogic
{
    public class DoWorkService : IDisposable
    {
        #region Region [Variables]
        private bool disposed;
      
        #endregion

        #region Region [Metodos]
        public async Task<Common.Message> DoWork(Common.Message message)
        {
            try
            {
                var nameSpace = Assembly.GetExecutingAssembly().GetName().Name;
                var CadenaObjeto = string.Format("{0}.{1}", nameSpace, message.BusinessLogic);
                var asm = AssemblyLoadContext.Default.LoadFromAssemblyPath( Assembly.GetExecutingAssembly().Location);
                var type = asm.GetType(CadenaObjeto);
                dynamic obj = Activator.CreateInstance(type);
                return await obj.DoWork(message);
            }
            catch (Exception ex)
            {
                return new Common.Message(new Exception(string.Format("Mensaje del sistema: {0}", ex.Message)));
            }
        }
        #endregion
        public class AssemblyLoader : AssemblyLoadContext
        {
            // Not exactly sure about this
            protected override Assembly Load(AssemblyName assemblyName)
            {
                var deps = DependencyContext.Default;
                var res = deps.CompileLibraries.Where(d => d.Name.Contains(assemblyName.Name)).ToList();
                var assembly = Assembly.Load(new AssemblyName(res.First().Name));
                return assembly;
            }
        }
      
         #region Region [Dispose]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DoWorkService()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
        }
        #endregion
    }
}

