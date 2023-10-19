using BookingService.Service;
using BookingService.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QrController : ControllerBase
    {
        public readonly IServiceManager _serviceManager;

        public QrController(IServiceManager serviceManager) {
            _serviceManager = serviceManager;
        }

        [HttpPost]
        [Route("getqr")]
        public async Task<IActionResult> GetQR([FromBody] QRParam param )
        {
            try
            {
                
                var result = await _serviceManager.OrderService.GetByCondition(param);

                if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex){ 
                return BadRequest(ex.ToString());
            }
        }
        [HttpPost]
        [Route("updateqr")]
        public async Task<IActionResult> UpdateQR([FromBody] QRCodeDTO dto) 
        {
           try
            {
                var result = await _serviceManager.OrderService.UpdateQR(dto);
                if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                if (result.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    return Conflict();
                }
                return Ok(result);
            }
            catch (Exception ex) {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPost]
        [Route("gethistory")]
        public async Task<IActionResult> GetHistory([FromBody] QRCodeDTO dto)
        {
            try
            {

                var result = await _serviceManager.HistoryService.GetHistory(dto);

                if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                return Ok(result.value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPost]
        [Route("deletehistory")]
        public async Task<IActionResult> deleteHistory([FromBody] QRCodeDTO dto)
        {
            try {
                var result = await _serviceManager.HistoryService.DeleteHistory(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }



    }
}
