using System;
using System.Threading.Tasks;
using Utn.FirmaDigital.Backend.Common;
using Utn.FirmaDigital.Backend.DataAccess.Repository;
using Utn.FirmaDigital.Backend.Utilities;
using static Utn.FirmaDigital.Backend.Common.Enum;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Security.Cryptography.Xml;

namespace Utn.FirmaDigital.Backend.BussinesLogic
{
    public class Firmar
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
                    case Operation.FirmarXML:
                        return await FirmarXML(message);
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

        public async virtual Task<Message> FirmarXML(Message message)
        {
            try
            {
                var resultMessage = new Message();
                var model = message.DeSerializeObject<Common.Firmar>();
                // Validar que el commercion este afiliado
                var usuarioRepository = new Usuarios_Repository(message.Connection);
                var modelUsuario = usuarioRepository.Get(new Common.Usuarios { Identificacion = model.identificacion_comercio }).Result;
                if (modelUsuario == null || modelUsuario.Id == 0)
                {
                    throw new Exception("Usuario no encontrado");
                }

                //Validar que xml este correcto xds

                // Firmar el xml
                var newFile = FirmarDocumentoXML(model.document, modelUsuario.Certificado, modelUsuario.LlavePrivada);





                return resultMessage;
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


        public static XmlDocument FirmarDocumentoXML(XmlDocument xmlDocument, string rutaArchivoCertificado, string passwordCertificado)
        {
            xmlDocument.PreserveWhitespace = true;
            XmlNode ExtensionContent = xmlDocument.GetElementsByTagName("ExtensionContent", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2").Item(0);
            ExtensionContent.RemoveAll();

            X509Certificate2 x509Certificate2 = new X509Certificate2(rutaArchivoCertificado, passwordCertificado, X509KeyStorageFlags.Exportable);
            RSACryptoServiceProvider key = new RSACryptoServiceProvider(new CspParameters(24));
            SignedXml signedXML = new SignedXml(xmlDocument);
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            KeyInfo keyInfo = new KeyInfo();
            KeyInfoX509Data keyInfoX509Data = new KeyInfoX509Data(x509Certificate2);
            Reference reference = new Reference("");

            string exportarLlave = x509Certificate2.PrivateKey.ToXmlString(true);
            key.PersistKeyInCsp = false;
            key.FromXmlString(exportarLlave);
            reference.AddTransform(env);
            signedXML.SigningKey = key;

            Signature XMLSignature = signedXML.Signature;
            XMLSignature.SignedInfo.AddReference(reference);
            keyInfoX509Data.AddSubjectName(x509Certificate2.Subject);

            keyInfo.AddClause(keyInfoX509Data);

            XMLSignature.KeyInfo = keyInfo;
            XMLSignature.Id = "SignatureKG";
            signedXML.ComputeSignature();

            ExtensionContent.AppendChild(signedXML.GetXml());

            return xmlDocument;
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
        ~Firmar()
        {
            this.Dispose(false);
        }
        #endregion

    }
}
