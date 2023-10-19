using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UserService.Attribute;
using UserService.Service;
using UserService.Service.Interface;

namespace UserService.Controllers
{
    [BaseAuthorize]
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public UserController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserLogin userLogin)
        {
            try
            {
                var accountDto = await _serviceManager.UserService.Authentication(userLogin);
                if (accountDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest(accountDto.Message);
                if (accountDto.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound(accountDto.Message);
                return Ok(accountDto.value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("mobile-authenticate")]
        public async Task<IActionResult> MobileAuthenticate([FromBody] UserLogin userLogin)
        {
            try
            {
                var accountDto = await _serviceManager.UserService.Authentication(userLogin);
                if (accountDto.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound(accountDto.Message);
                return Ok(accountDto.value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("add-user")]
        public async Task<IActionResult> AddUser([FromBody] UserDTO userDto)
        {
            try
            {
                var accountDto = await _serviceManager.UserService.AddUser(userDto);
                if (accountDto.StatusCode == System.Net.HttpStatusCode.Conflict)
                    return Conflict(accountDto.Message);
                if (accountDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest(accountDto.Message);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("edit-user")]
        public async Task<IActionResult> EditUser(UserDTO userDto)
        {
            try
            {
                var accountDto = await _serviceManager.UserService.EditUser(userDto);
                if (accountDto.StatusCode == System.Net.HttpStatusCode.Conflict)
                    return Conflict(accountDto.Message);
                if (accountDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest(accountDto.Message);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("delete-user")]
        public async Task<IActionResult> DeleteUser([FromBody] Guid referenceID)
        {
            try
            {
                var accountDto = await _serviceManager.UserService.Delete(referenceID);
                if (accountDto.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound();
                if (accountDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest(accountDto.Message);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("changeactive-user")]
        public async Task<IActionResult> ChangeActive([FromBody] Guid referenceID)
        {
            try
            {
                var accountDto = await _serviceManager.UserService.ChangeActive(referenceID);
                if (accountDto.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound();
                if (accountDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest(accountDto.Message);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("table-user")]
        public async Task<IActionResult> TableUser([FromQuery] DataTableParams param)
        {
            try
            {
                var table = await _serviceManager.UserService.TableUser(param);
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
        [Route("get-user")]
        public async Task<IActionResult> GetUser([FromQuery(Name = "reference_id")] Guid referenceId)
        {
            try
            {
                var accountDto = await _serviceManager.UserService.GetUser(referenceId);
                if (accountDto.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound(accountDto.Message);
                if (accountDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest(accountDto.Message);
                return Ok(accountDto.value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

    }
}
