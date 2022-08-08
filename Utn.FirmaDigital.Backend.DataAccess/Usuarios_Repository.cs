using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using Utn.FirmaDigital.Backend.Common;
using Microsoft.Data.SqlClient;

namespace Utn.FirmaDigital.Backend.DataAccess.Repository
{

    public class Usuarios_Repository : IRepository<Common.Usuarios>, IDisposable
    {

        #region Region [Variables]
        public IDbConnection Connection { get; set; }
        public IDbTransaction Transaction { get; set; }
        private string ConnectionString { get; set; }
        #endregion

        #region Region [Constructor]
        public Usuarios_Repository(string connectionString)
        {
            ConnectionString = connectionString;
        }
        #endregion

        #region Region [Methods]
        public async Task<IEnumerable<Usuarios>> List(Usuarios model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var result = connection.Query<
                    Common.Usuarios>
                    ("PA_CON_usuarios_GET",
                    param: new
                    {
                        P_id = model.Id,
                        P_nombre = model.Nombre,
                        P_contrasena = model.Contrasena,
                        P_email = model.Email,
                    },
                    commandType: CommandType.StoredProcedure);
                return await Task.FromResult<IEnumerable<Usuarios>>(result.ToList());
            }
        }
        public async Task<ICollection<Usuarios>> ListCollection(Usuarios model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var result = connection.Query<
                    Common.Usuarios>
                    ("PA_CON_usuarios_GET",
                    param: new
                    {
                        P_id = model.Id,
                        P_nombre = model.Nombre,
                        P_contrasena = model.Contrasena,
                        P_email = model.Email,
                    },
                    commandType: CommandType.StoredProcedure);
                return await Task.FromResult<ICollection<Common.Usuarios>>(result.ToList());
            }
        }
        public async Task<Common.Usuarios> Get(Common.Usuarios model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var result = connection.Query<
                    Utn.FirmaDigital.Backend.Common.Usuarios>
                    ("PA_CON_usuarios_GET",
                    param: new
                    {
                        P_id = model.Id,
                        P_nombre = model.Nombre,
                        P_contrasena = model.Contrasena,
                        P_email = model.Email,
                    },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
                return await Task.FromResult<Common.Usuarios>(result);
            }
        }

        public async Task<Common.Usuarios> SaveGet(Common.Usuarios model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var result = connection.Query<
                    Utn.FirmaDigital.Backend.Common.Usuarios>
                    ("PA_MAN_usuarios_SAVE",
                    param: new
                    {
                        P_id = model.Id,
                        P_nombre = model.Nombre,
                        P_contrasena = model.Contrasena,
                        P_email = model.Email,
                        P_identificacion = model.Identificacion,
                        P_razonSocial = model.RazonSocial,
                        P_sexo = model.Sexo,
                        P_fecNacimiento = model.FechaNacimiento,
                        P_certificado = model.Certificado,
                        P_llavePublica = model.LlavePublica,
                        P_llavePrivada = model.LlavePrivada
                    },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
                return await Task.FromResult<Common.Usuarios>(result);
            }
        }
        public async Task Save(Common.Usuarios model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.QueryAsync<
                    Utn.FirmaDigital.Backend.Common.Usuarios>
                    ("PA_MAN_usuarios_SAVE",
                    param: new
                    {
                        P_id = model.Id,
                        P_nombre = model.Nombre,
                        P_contrasena = model.Contrasena,
                        P_email = model.Email,
                        P_identificacion = model.Identificacion,
                        P_razonSocial = model.RazonSocial,
                        P_sexo = model.Sexo,
                        P_fecNacimiento = model.FechaNacimiento,
                        P_certificado = model.Certificado,
                        P_llavePublica = model.LlavePublica,
                        P_llavePrivada = model.LlavePrivada
                    },
                    commandType: CommandType.StoredProcedure);
            }
        }
        public async Task Delete(Common.Usuarios model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.ExecuteAsync(
                sql: "PA_usuarios_DELETE",
                param: new
                {
                    P_MAN_id = model.Id
                },
                commandType: CommandType.StoredProcedure);
            }
        }
        #endregion
        #region Region [Dispose]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~Usuarios_Repository()
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
