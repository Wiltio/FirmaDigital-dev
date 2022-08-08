using System;
namespace Utn.FirmaDigital.Backend.Common
{
    public class Usuarios
    {
        public Usuarios()
        {
            Id = 0;
            Nombre = "";
            Contrasena = "";
            Email = "";
            Identificacion = "";
            RazonSocial = "";
            FechaNacimiento = Convert.ToDateTime("1990-01-01");
            Sexo = "";
            Certificado = "";
            LlavePublica = "";
            LlavePrivada = "";

        }
        public Int32 Id { get; set; }
        public String Nombre { get; set; }
        public String Contrasena { get; set; }
        public String Email { get; set; }
        public string Certificado { get; set; }
        public string LlavePublica { get; set; }
        public string LlavePrivada { get; set; }
        public string Identificacion { get; set; }
        public string RazonSocial { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Sexo { get; set; }
    }
}
