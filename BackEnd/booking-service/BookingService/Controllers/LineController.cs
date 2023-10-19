using BookingService.Attribute;
using BookingService.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{
    [BaseAuthorize]
    [Route("line")]
    [ApiController]
    public class LineController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public LineController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        [Route("dropdown-line")]
        [AllowAnonymous]
        public async Task<IActionResult> DropDownLine(string? q)
        {
            try
            {
                var select = await _serviceManager.LineService.DropDownLine(q ?? "");
                if (select.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest();
                return Ok(select.value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("dropdown-linedepartment")]
        [AllowAnonymous]
        public async Task<IActionResult> DropDownLineDepartment(string? q, Guid? l = null)
        {
            try
            {
                var select = await _serviceManager.LineService.DropDownLineDepartment(q ?? "", l ?? Guid.Empty);
                if (select.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest();
                return Ok(select.value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
