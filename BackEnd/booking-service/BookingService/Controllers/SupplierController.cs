using BookingService.Attribute;
using BookingService.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{
    [BaseAuthorize]
    [Route("supplier")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public SupplierController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        [Route("dropdown-supplier")]
        [AllowAnonymous]
        public async Task<IActionResult> DropDownSupplier(string? q)
        {
            try
            {
                var select = await _serviceManager.SupplierService.DropDownSupplier(q ?? "");
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
        [Route("get-supplier")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSupplier([FromQuery(Name = "reference_id")] Guid? referenceId)
        {
            try
            {
                var suplierDto = await _serviceManager.SupplierService.GetSupplier(referenceId ?? Guid.Empty);
                if (suplierDto.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound(suplierDto.Message);
                if (suplierDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest();
                return Ok(suplierDto.value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

      
    }
}
