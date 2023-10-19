using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Attribute;
using UserService.Service;
using UserService.Service.Interface;

namespace UserService.Controllers
{
    [BaseAuthorize]
    [Route("timeframe")]
    [ApiController]
    public class TimeFrameController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public TimeFrameController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost]
        [Route("add-timeframe")]
        public async Task<IActionResult> AddTimeFrame([FromBody] TimeFrameDTO timeFrameDto)
        {
            try
            {
                var checkDto = await _serviceManager.TimeFrameService.AddTimeFrame(timeFrameDto);
                if (checkDto.StatusCode == System.Net.HttpStatusCode.Conflict)
                    return Conflict(checkDto.Message);
                if (checkDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("edit-timeframe")]
        public async Task<IActionResult> EditTimeFrame(TimeFrameDTO timeFrameDto)
        {
            try
            {
                var checkDto = await _serviceManager.TimeFrameService.EditTimeFrame(timeFrameDto);
                if (checkDto.StatusCode == System.Net.HttpStatusCode.Conflict)
                    return Conflict(checkDto.Message);
                if (checkDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("delete-timeframe")]
        public async Task<IActionResult> DeleteTimeFrame([FromBody] Guid referenceID)
        {
            try
            {
                var checkDto = await _serviceManager.TimeFrameService.Delete(referenceID);
                if (checkDto.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound();
                if (checkDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("changeactive-timeframe")]
        public async Task<IActionResult> ChangeActive([FromBody] Guid referenceID)
        {
            try
            {
                var checkDto = await _serviceManager.TimeFrameService.ChangeActive(referenceID);
                if (checkDto.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound();
                if (checkDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("table-timeframe")]
        public async Task<IActionResult> TableTimeFrame([FromQuery] DataTableParams param)
        {
            try
            {
                var table = await _serviceManager.TimeFrameService.TableTimeFrame(param);
                if (table.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest();
                return Ok(table.value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("get-timeframe")]
        public async Task<IActionResult> GetTimeFrame([FromQuery(Name = "reference_id")] Guid referenceId)
        {
            try
            {
                var checkDto = await _serviceManager.TimeFrameService.GetTimeFrame(referenceId);
                if (checkDto.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound(checkDto.Message);
                if (checkDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest();
                return Ok(checkDto.value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("dropdown-timeframe")]
        [AllowAnonymous]
        public async Task<IActionResult> DropDownTimeFrame(string? query, Guid? s)
        {
            try
            {
                var checkDto = await _serviceManager.TimeFrameService.DropDownTimeFrame(query ?? "", s ?? Guid.Empty);
                if (checkDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest();
                return Ok(checkDto.value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
