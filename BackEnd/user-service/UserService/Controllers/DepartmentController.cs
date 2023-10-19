using Microsoft.AspNetCore.Mvc;
using UserService.Attribute;
using UserService.Service.Interface;

namespace UserService.Controllers
{
    [BaseAuthorize]
    [Route("department")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public DepartmentController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        [Route("dropdown-department")]
        public async Task<IActionResult> DropDownDepartment(string? q)
        {
            try
            {
                var select = await _serviceManager.DepartmentService.DropDownDepartment(q);
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
        [Route("dropdown-division")]
        public async Task<IActionResult> DropDownDivision(string? q = "", Guid? d = null)
        {
            try
            {
                var select = await _serviceManager.DepartmentService.DropDownDivision(q, d ?? Guid.Empty);
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
