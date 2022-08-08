using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Utn.FirmaDigital.Backend.BussinesLogic;
using Utn.FirmaDigital.Backend.Common;
using Utn.FirmaDigital.Backend.Utilities;
using static Utn.FirmaDigital.Backend.Common.Enum;

namespace Utn.FirmaDigital.Backend.WepApi.Controllers
{
    [Route("api/[controller]")]
    public class FirmarController : Controller
    {
        private IConfiguration configuration;
        public FirmarController(IConfiguration iConfiguration)
        {
            configuration = iConfiguration;
        }
        #region Region [Methods]

        [Route("FirmarXML")]
        [HttpPost]
        public async Task<IActionResult> FirmarXML([FromBody] Common.Firmar model)
        {
            try
            {
                var message = new Message();
                message.BusinessLogic = configuration.GetValue<string>("AppSettings:BusinessLogic:Firmar");
                message.Connection = configuration.GetValue<string>("ConnectionStrings:FIRMA");
                message.Operation = Operation.FirmarXML;
                message.MessageInfo = model.SerializeObject();
                using (var businessLgic = new DoWorkService())
                {
                    var result = await businessLgic.DoWork(message);
                    if (result.Status == Status.Failed)
                    {
                        return BadRequest(result.Result);
                    }
                    var list = result.DeSerializeObject<Common.Firmar>();
                    return Ok(list);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        #endregion Region [Methods]
    }
}
