







using BookingService.Infrastructure;
using BookingService.Service;
using BookingService.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReturnController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        //private readonly IRedisCacheService _redisCache;
        private readonly BookingDbContext _bookingDbContext;


        public ReturnController(IServiceManager serviceManager, BookingDbContext bookingDbContext)
        {
            _serviceManager = serviceManager;
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
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> UpdateData([FromForm] ReturnParam ReturnParam)
        {
            try {
                var result = await _serviceManager.ReturnService.UpdateReturn(ReturnParam);
                if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return BadRequest();
                }
                if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                return Ok(result.value);
            }
            catch (Exception ex) {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPost]
        [Route("Change")]
        public async Task<IActionResult> ChangeActiveReturn([FromBody] ReturnSearch param)
        {
            try {
                var result =  await _serviceManager.ReturnService.ChangeActiveReturn(param);
                if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return BadRequest();
                }
                if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                return Ok(result.value);
            }
            catch (Exception ex) {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> DeleteReturn([FromBody] ReturnSearch param)
        {
            try {
                var result = await _serviceManager.ReturnService.DeleteReturn(param);
                if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound(result.Message);
                }
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
        [HttpGet]
        [Route("GetEdit")]
        public async Task<IActionResult> GetEdit(string reference_id)
        {
            try {
                var result = await _serviceManager.ReturnService.GetReturn(reference_id);
                if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound(result.Message);
                }
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
