using Microsoft.AspNetCore.Mvc;
using UserService.Attribute;
using UserService.Service;
using UserService.Service.Interface;

namespace UserService.Controllers
{
    [BaseAuthorize]
    [Route("role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public RoleController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost]
        [Route("add-role")]
        public async Task<IActionResult> AddRole(RoleDTO roleDto)
        {
            try
            {
                var accountDto = await _serviceManager.RoleService.AddRole(roleDto);
                if (accountDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest(accountDto.Message);
                return Ok(accountDto.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("edit-role")]
        public async Task<IActionResult> EditRole(RoleDTO roleDto)
        {
            try
            {
                var accountDto = await _serviceManager.RoleService.EditRole(roleDto);
                if (accountDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest(accountDto.Message);
                return Ok(accountDto.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("delete-role")]
        public async Task<IActionResult> DeleteRole([FromBody] int Id)
        {
            try
            {
                var roleDto = await _serviceManager.RoleService.DeleteRole(Id);
                if (roleDto.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound();
                if (roleDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest();
                return Ok(roleDto.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("changeactive-role")]
        public async Task<IActionResult> ChangeActiveRole([FromBody] int Id)
        {
            try
            {
                var roleDto = await _serviceManager.RoleService.ChangeActiveRole(Id);
                if (roleDto.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound();
                if (roleDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest();
                return Ok(roleDto.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("table-role")]
        public async Task<IActionResult> TableRole([FromQuery] DataTableParams param)
        {
            try
            {
                var table = await _serviceManager.RoleService.TableRole(param);
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
        [Route("dropdown-role")]
        public async Task<IActionResult> DropDownRole(string? q = "")
        {
            try
            {
                var select = await _serviceManager.RoleService.DropDownRole(q);
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
        [Route("get-function")]
        public async Task<IActionResult> GetFunction([FromQuery(Name = "role_id")] int? roleId)
        {
            try
            {
                var roleDto = await _serviceManager.RoleService.GetFunction(roleId ?? 0);
                if (roleDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest();
                return Ok(roleDto.value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }


    }
}
