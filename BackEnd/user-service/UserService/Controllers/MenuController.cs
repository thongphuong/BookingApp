using Microsoft.AspNetCore.Mvc;
using UserService.Attribute;
using UserService.Service.Interface;

namespace UserService.Controllers
{
    [BaseAuthorize]
    [Route("menu")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public MenuController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        [Route("get-menu")]
        public async Task<IActionResult> GetMenu([FromQuery(Name = "role_id")] int roleId)
        {
            try
            {
                var menuDto = await _serviceManager.MenuService.GetMenu(roleId);
                if (menuDto.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest();
                return Ok(menuDto.value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
