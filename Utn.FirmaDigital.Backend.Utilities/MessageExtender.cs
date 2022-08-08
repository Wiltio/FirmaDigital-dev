using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Utn.FirmaDigital.Backend.Common;

namespace Utn.FirmaDigital.Backend.Utilities
{
    public static class MessageExtender
    {
        public static T DeSerializeObject<T>(this Message xMessage)
        {
            return JsonConvert.DeserializeObject<T>(xMessage.MessageInfo, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy HH:mm" });
        }

        public static T DeSerializeObject<T>(this String jsonParameter)
        {
            return JsonConvert.DeserializeObject<T>(jsonParameter, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy HH:mm" });
        }

        public static string SerializeObject<T>(this T inputObject)
        {
            var returnValue = new Message();
            CultureInfo culture = new CultureInfo("es-ES");
            var culture2 = CultureInfo.CurrentCulture;

            return JsonConvert.SerializeObject(inputObject, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy HH:mm" });
        }
        public static object DeSerializeObject(string value)
        {
            return JsonConvert.DeserializeObject(value);
        }

    }
}