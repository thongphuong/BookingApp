using BookingService.Attribute;
using BookingService.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookingService.Infrastructure;

namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        //private readonly IRedisCacheService _redisCache;
        private readonly ILogger<StoreController> _logger;
        private readonly BookingDbContext _bookingDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly string BASE_DOMAIN = "https://localhost:7278";

        public StoreController(IServiceManager serviceManager, ILogger<StoreController> logger, BookingDbContext bookingDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _serviceManager = serviceManager;
            //_redisCache = redisCache;
            _logger = logger;
            _bookingDbContext = bookingDbContext;
            _webHostEnvironment = webHostEnvironment;
        }
       
        [HttpGet]
        [Route("get-store")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStore([FromQuery(Name = "reference_id")] Guid? referenceId)
        {
            try
            {
                var store = await _serviceManager.StoreService.GetStore(referenceId ?? Guid.Empty);
                if (store.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound(store.Message);
                if (store.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest();
                return Ok(store.value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("dropdown-store")]
        [AllowAnonymous]
        public async Task<IActionResult> DropDownStore(string? q = "")
        {
            try
            {
                var select = await _serviceManager.StoreService.DropDownStore(q);
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
