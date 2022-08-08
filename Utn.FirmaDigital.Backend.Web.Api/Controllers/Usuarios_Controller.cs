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
    public class UsuariosController : Controller
    {
        private IConfiguration configuration;
        public UsuariosController(IConfiguration iConfiguration)
        {
            configuration = iConfiguration;
        }
        #region Region [Methods]
        /// <summary>
        /// Nombre: ListarUsuarios
        /// Descripcion: Metodo utilizado para ontener una lista de modelos usuarios y retornar un objeto datatable
        /// Fecha de creacion: 7/3/2022
        /// Autor:
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [Route("List")]
        [HttpPost]
        public async Task<IActionResult> List([FromBody] Common.Usuarios model)
        {
            try
            {
                var message = new Message();
                message.BusinessLogic = configuration.GetValue<string>("AppSettings:BusinessLogic:Usuarios");
                message.Connection = configuration.GetValue<string>("ConnectionStrings:FIRMA");
                message.Operation = Operation.List;
                message.MessageInfo = model.SerializeObject();
                using (var businessLgic = new DoWorkService())
                {
                    var result = await businessLgic.DoWork(message);
                    if (result.Status == Status.Failed)
                    {
                        return BadRequest(result.Result);
                    }
                    var list = result.DeSerializeObject<IEnumerable<Common.Usuarios>>();
                    return Ok(list);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// Nombre: ObtenerUsuarios
        /// Descripcion: Metodo utilizado para ontener una lista de modelos usuarios y retornar un objeto datatable
        /// Fecha de creacion: 7/3/2022
        /// Autor:
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [Route("Get")]
        [HttpPost]
        public async Task<IActionResult> Get([FromBody] Common.Usuarios model)
        {
            try
            {
                var message = new Message();
                message.BusinessLogic = configuration.GetValue<string>("AppSettings:BusinessLogic:Usuarios");
                message.Connection = configuration.GetValue<string>("ConnectionStrings:FIRMA");
                message.Operation = Operation.Get;
                message.MessageInfo = model.SerializeObject();
                using (var businessLgic = new DoWorkService())
                {
                    var result = await businessLgic.DoWork(message);
                    if (result.Status == Status.Failed)
                    {
                        return BadRequest(result.Result);
                    }
                    var resultModel = result.DeSerializeObject<Common.Usuarios>();
                    return Ok(resultModel);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Route("SaveGet")]
        [HttpPost]
        public async Task<IActionResult> SaveGet([FromBody] Common.Usuarios model)
        {
            try
            {
                var message = new Message();
                message.BusinessLogic = configuration.GetValue<string>("AppSettings:BusinessLogic:Usuarios");
                message.Connection = configuration.GetValue<string>("ConnectionStrings:FIRMA");
                message.Operation = Operation.SaveGet;
                message.MessageInfo = model.SerializeObject();
                using (var businessLgic = new DoWorkService())
                {
                    var result = await businessLgic.DoWork(message);
                    if (result.Status == Status.Failed)
                    {
                        return BadRequest(result.Result);
                    }
                    var resultModel = result.DeSerializeObject<Common.Usuarios>();
                    return Ok(resultModel);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// Nombre: GuardarUsuarios
        /// Descripcion: Metodo utilizado para ontener una lista de modelos usuarios y retornar un objeto datatable
        /// Fecha de creacion: 7/3/2022
        /// Autor:
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [Route("Save")]
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Common.Usuarios model)
        {
            try
            {
                var message = new Message();
                message.BusinessLogic = configuration.GetValue<string>("AppSettings:BusinessLogic:Usuarios");
                message.Connection = configuration.GetValue<string>("ConnectionStrings:FIRMA");
                message.Operation = Operation.Save;
                message.MessageInfo = model.SerializeObject();
                using (var businessLgic = new DoWorkService())
                {
                    var result = await businessLgic.DoWork(message);
                    if (result.Status == Status.Failed)
                    {
                        return BadRequest(result.Result);
                    }
                    var resultModel = result.DeSerializeObject<Common.Usuarios>();
                    return Ok(resultModel);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [Route("ValidarUsuario")]
        [HttpPost]
        public async Task<IActionResult> ValidarUsuario([FromBody] Common.Usuarios model)
        {
            try
            {
                var message = new Message();
                message.BusinessLogic = configuration.GetValue<string>("AppSettings:BusinessLogic:Usuarios");
                message.Connection = configuration.GetValue<string>("ConnectionStrings:FIRMA");
                message.Operation = Operation.Get;
                message.MessageInfo = model.SerializeObject();
                using (var businessLgic = new DoWorkService())
                {
                    var result = await businessLgic.DoWork(message);
                    if (result.Status == Status.Failed)
                    {
                        var responseError = new
                        {
                            item = new
                            {
                                isValid = false,
                                code = 200,
                                message = result.Result
                            }
                        };
                        return BadRequest(responseError);
                    }
                    var resultModel = result.DeSerializeObject<Common.Usuarios>();

                    var responseSuccess = new
                    {
                        item = new
                        {
                            isValid = resultModel.Id > 0 ? true : false,
                            code = 200,
                            message = "Proceso efectuado satisfactoriamente..."
                        }
                    };

                    return Ok(responseSuccess);
                }
            }
            catch (Exception ex)
            {
                var responseErrorExeption = new
                {
                    item = new
                    {
                        isValid = false,
                        code = 200,
                        message = ex
                    }
                };
                return BadRequest(responseErrorExeption);
            }
        }
        #endregion Region [Methods]
    }
}
