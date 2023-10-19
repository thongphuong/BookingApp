using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Attribute;
using UserService.Service;
using UserService.Service.Interface;

namespace UserService.Controllers
{
    [BaseAuthorize]
    [Route("systemparameter")]
    [ApiController]
    public class SystemParameterController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public SystemParameterController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost]
        [Route("add-systemparameter")]
        public async Task<IActionResult> AddSystemParameter([FromBody] SystemParameterDTO systemparameterDto)
        {
            try
            {
                var checkDto = await _serviceManager.SystemParameterService.AddSystemParameter(systemparameterDto);
                if (checkDto.StatusCode == System.Net.HttpStatusCode.Conflict)
                    return Conflict(checkDto.Message);
                if (checkDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest(checkDto.Message);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("edit-systemparameter")]
        public async Task<IActionResult> EditSystemParameter(SystemParameterDTO systemparameterDto)
        {
            try
            {
                var checkDto = await _serviceManager.SystemParameterService.EditSystemParameter(systemparameterDto);
                if (checkDto.StatusCode == System.Net.HttpStatusCode.Conflict)
                    return Conflict(checkDto.Message);
                if (checkDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest(checkDto.Message);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("delete-systemparameter")]
        public async Task<IActionResult> DeleteSystemParameter([FromBody] Guid referenceID)
        {
            try
            {
                var checkDto = await _serviceManager.TimeFrameService.Delete(referenceID);
                if (checkDto.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound();
                if (checkDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest(checkDto.Message);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("changeactive-systemparameter")]
        public async Task<IActionResult> ChangeActive([FromBody] Guid referenceID)
        {
            try
            {
                var checkDto = await _serviceManager.SystemParameterService.ChangeActive(referenceID);
                if (checkDto.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound();
                if (checkDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest(checkDto.Message);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("table-systemparameter")]
        public async Task<IActionResult> TableSystemParameter([FromQuery] DataTableParams param)
        {
            try
            {
                var table = await _serviceManager.SystemParameterService.TableSystemParameter(param);
                if (table.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest(table.Message);
                return Ok(table.value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("get-systemparameter")]
        public async Task<IActionResult> GetSystemParameter([FromQuery(Name = "reference_id")] Guid referenceId)
        {
            try
            {
                var checkDto = await _serviceManager.SystemParameterService.GetSystemParameter(referenceId);
                if (checkDto.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound(checkDto.Message);
                if (checkDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest(checkDto.Message);
                return Ok(checkDto.value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("get-systemparameter-redis")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSystemParameterRedis()
        {
            try
            {
                var checkDto = await _serviceManager.SystemParameterService.GetRedisSystemParameter();
                if (checkDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest(checkDto.Message);
                return Ok(checkDto.value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
