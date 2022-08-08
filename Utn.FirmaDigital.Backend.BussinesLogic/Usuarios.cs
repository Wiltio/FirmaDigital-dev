using System;
using System.Threading.Tasks;
using Utn.FirmaDigital.Backend.Common;
using Utn.FirmaDigital.Backend.DataAccess.Repository;
using Utn.FirmaDigital.Backend.Utilities;
using static Utn.FirmaDigital.Backend.Common.Enum;
using System.Security.Cryptography;
using System.Text;

namespace Utn.FirmaDigital.Backend.BussinesLogic
{
    public class Usuarios
    {
        #region Region [Methods]
        /// <summary>
        /// Nombre: DoWork
        /// Descripcion: Metodo encargado de orquestar las solicitudes de operaciones para el objeto "usuarios".
        /// Fecha de creación: 7/3/2022.
        /// Autor: jnmcgregor.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<Common.Message> DoWork(Message message)
        {
            try
            {
                switch (message.Operation)
                {
                    case Operation.List:
                        return await List(message);
                    case Operation.Get:
                        return await Get(message);
                    case Operation.SaveGet:
                        return await SaveGet(message);
                    case Operation.Save:
                        return await Save(message);
                    case Operation.Delete:
                        return await Delete(message);
                    default:
                        var resultMessage = new Message();
                        resultMessage.Status = Status.Failed;
                        resultMessage.Result = "Operación no soportada";
                        resultMessage.MessageInfo = string.Empty;
                        return resultMessage;
                }
            }
            catch (Exception ex)
            {
                var resultMessage = new Message();
                resultMessage.Status = Status.Failed;
                resultMessage.Result = string.Format("{0}", ex.Message);
                resultMessage.MessageInfo = string.Empty;
                return resultMessage;
            }
        }
        public async virtual Task<Message> List(Message message)
        {
            try
            {
                var resultMessage = new Common.Message();
                var model = message.DeSerializeObject<Common.Usuarios>();
                using (var repository = new Usuarios_Repository(message.Connection))
                {
                    var returnObject = await repository.List(model);
                    resultMessage.Status = Status.Success;
                    resultMessage.Result = "Proceso efectuado satisfactoriamente...";
                    resultMessage.MessageInfo = returnObject.SerializeObject();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                var resultMessage = new Common.Message();
                resultMessage.Status = Status.Failed;
                resultMessage.Result = string.Format("{0}", ex.Message);
                resultMessage.MessageInfo = string.Empty;
                return resultMessage;
            }
        }
        public async virtual Task<Message> Get(Message message)
        {
            try
            {
                var resultMessage = new Message();
                var model = message.DeSerializeObject<Common.Usuarios>();
                using (var repository = new Usuarios_Repository(message.Connection))
                {
                    var returnObject = await repository.Get(model);
                    resultMessage.Status = Status.Success;
                    resultMessage.Result = "Proceso efectuado satisfactoriamente...";
                    resultMessage.MessageInfo = returnObject.SerializeObject();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                var resultMessage = new Message();
                resultMessage.Status = Status.Failed;
                resultMessage.Result = string.Format("{0}", ex.Message);
                resultMessage.MessageInfo = string.Empty;
                return resultMessage;
            }
        }
        public async virtual Task<Message> SaveGet(Message message)
        {
            try
            {
                var resultMessage = new Message();
                var model = message.DeSerializeObject<Common.Usuarios>();
                using (var repository = new Usuarios_Repository(message.Connection))
                {
                    //generacion de llave publica
                    model.LlavePublica = await generacionLlavePublica();
                    //generacion de llave privada
                    model.LlavePrivada = await generacionLlavePrivada();
                    //generacion de certificado
                    model.Certificado = await generacionCertificado(model);

                    var returnObject = await repository.SaveGet(model);
                    resultMessage.Status = Status.Success;
                    resultMessage.Result = "Proceso efectuado satisfactoriamente...";
                    resultMessage.MessageInfo = returnObject.SerializeObject();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                var resultMessage = new Message();
                resultMessage.Status = Status.Failed;
                resultMessage.Result = string.Format("{0}", ex.Message);
                resultMessage.MessageInfo = string.Empty;
                return resultMessage;
            }
        }


        public async virtual Task<Message> Save(Message message)
        {
            try
            {
                var resultMessage = new Message();
                var model = message.DeSerializeObject<Common.Usuarios>();
                using (var repository = new Usuarios_Repository(message.Connection))
                {
                    //generacion de llave publica
                    model.LlavePublica = await generacionLlavePublica();
                    //generacion de llave privada
                    model.LlavePrivada = await generacionLlavePrivada();
                    //generacion de certificado
                    model.Certificado = await generacionCertificado(model);

                    await repository.Save(model);
                    resultMessage.Status = Status.Success;
                    resultMessage.Result = "Proceso efectuado satisfactoriamente...";
                    resultMessage.MessageInfo = String.Empty;
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                var resultMessage = new Message();
                resultMessage.Status = Status.Failed;
                resultMessage.Result = string.Format("{0}", ex.Message);
                resultMessage.MessageInfo = string.Empty;
                return resultMessage;
            }
        }
        public async virtual Task<Common.Message> Delete(Message message)
        {
            try
            {
                var resultMessage = new Message();
                var model = message.DeSerializeObject<Common.Usuarios>();
                using (var repository = new Usuarios_Repository(message.Connection))
                {
                    await repository.Delete(model);
                    resultMessage.Status = Status.Success;
                    resultMessage.Result = "Proceso efectuado satisfactoriamente...";
                    resultMessage.MessageInfo = String.Empty;
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                var resultMessage = new Common.Message();
                resultMessage.Status = Status.Failed;
                resultMessage.Result = string.Format("{0}", ex.Message);
                resultMessage.MessageInfo = string.Empty;
                return resultMessage;
            }
        }

        public async virtual Task<string> generacionCertificado(Common.Usuarios model)
        {
            if (model.LlavePrivada == string.Empty)
            {
                throw new Exception("No se proporciono la llave de encryptacion");
            }

            TripleDESCryptoServiceProvider des;
            MD5CryptoServiceProvider hashmd5;

            byte[] keyhash, buff;
            string stEncrypted;

            hashmd5 = new MD5CryptoServiceProvider();
            keyhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(model.LlavePrivada));

            hashmd5 = null;
            des = new TripleDESCryptoServiceProvider();

            des.Key = keyhash;
            des.Mode = CipherMode.ECB;

            buff = ASCIIEncoding.ASCII.GetBytes(model.SerializeObject());
            stEncrypted = Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buff, 0, buff.Length));

            return stEncrypted;
        }

        public async virtual Task<string> generacionLlavePublica()
        {
            int maxLength = 342;
            string allCharacteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwxyz";
            string public_key = "";

            for (int i = 0; i < maxLength; i++)
            {
                Random rnd = new Random();
                public_key += allCharacteres[rnd.Next(allCharacteres.Length)];
            }
            return string.Concat(public_key, "==");
        }

        public async virtual Task<string> generacionLlavePrivada()
        {
            int maxLength = 342;
            string allCharacteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwxyz";
            string public_key = "";

            for (int i = 0; i < maxLength; i++)
            {
                Random rnd = new Random();
                public_key += allCharacteres[rnd.Next(allCharacteres.Length)];
            }
            return string.Concat(public_key, "==");
        }

        #endregion
        #region Region [Dispose]
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
        }
        ~Usuarios()
        {
            this.Dispose(false);
        }
        #endregion

    }
}
