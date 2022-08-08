using System;
using System.Xml;

namespace Utn.FirmaDigital.Backend.Common
{
    public class Firmar
    {
        public Firmar()
        {
            document = new XmlDocument();
            identificacion_comercio = "";
        }
        public XmlDocument document { get; set; }
        public string identificacion_comercio { get; set; }
    }
}
