using BookingService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using UserService.Domain;
using UserService.Infrastructure;
using UserService.Service;
using UserService.Service.DTO.Return;
using UserService.Service.Interface;

namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReturnController :  ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        //private readonly IRedisCacheService _redisCache;
        private readonly ILogger<StoreController> _logger;
        private readonly BookingDbContext _bookingDbContext;


        public ReturnController(IServiceManager serviceManager, ILogger<StoreController> logger, BookingDbContext bookingDbContext)
        {
            _serviceManager = serviceManager;
            _logger = logger;
            _bookingDbContext = bookingDbContext;
        }
        [HttpGet]
        [Route("GetTable")]
        public async Task<IActionResult> Get([FromQuery] DataTableParams param)
        {
            try
            {
                var table = await _serviceManager.ReturnService.TableReturn(param);
                if (table.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return BadRequest();
                }
                return Ok(table.value);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPost]
        [Route("Add")]

        public async Task<IActionResult> AddData([FromForm] ReturnParam ReturnParam)
        {

            try
            {
                var result = await _serviceManager.ReturnService.InsertReturn(ReturnParam);
                if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return BadRequest();
                }
                return Ok(result.value);

            }
            catch (Exception ex) {
                return BadRequest(ex.ToString());
            }
        }


    }
}
