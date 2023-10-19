using BookingService.Attribute;
using BookingService.Service;
using BookingService.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{
    [BaseAuthorize]
    [Route("order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public OrderController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost]
        [Route("add-order")]
        [AllowAnonymous]
        public async Task<IActionResult> AddOrder([FromBody] OrderDTO? orderDto)
        {
            try
            {
                var order = await _serviceManager.OrderService.AddOrder(orderDto ?? new OrderDTO());
                if (order.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest(order.Message);
                if (order.StatusCode == System.Net.HttpStatusCode.Conflict)
                    return Conflict(order.Message);
                return Ok(order.value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("table-order")]
        public async Task<IActionResult> TableOrder([FromQuery] DataTableParams param)
        {
            try
            {
                var table = await _serviceManager.OrderService.TableOrder(param);
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
        [Route("table-order-refuse")]
        public async Task<IActionResult> TableOrderRefuse([FromQuery] DataTableParams param)
        {
            try
            {
                var table = await _serviceManager.OrderService.TableOrderRefuse(param);
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
        [Route("get-order")]
        public async Task<IActionResult> GetOrder([FromQuery(Name = "reference_id")] Guid referenceId)
        {
            try
            {
                var order = await _serviceManager.OrderService.GetOrder(referenceId);
                if (order.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound(order.Message);
                if (order.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest(order.Message);
                return Ok(order.value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("delete-order")]
        public async Task<IActionResult> DeleteOrder([FromBody] Guid referenceId)
        {
            try
            {
                var order = await _serviceManager.OrderService.DeleteOrder(referenceId);
                if (order.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound();
                if (order.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest(order.Message);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("edit-order")]
        public async Task<IActionResult> EditOrder([FromBody] OrderDTO? orderDto)
        {
            try
            {
                var order = await _serviceManager.OrderService.EditOrder(orderDto ?? new OrderDTO());
                if (order.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest(order.Message);
                if (order.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    return Forbid(order.Message);
                return Ok(order.value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("approve-order")]
        public async Task<IActionResult> ApproveOrder([FromForm] OrderApproveDTO? orderApproveDto)
        {
            try
            {
                var orderApprove = await _serviceManager.OrderService.ApproveOrder(orderApproveDto ?? new OrderApproveDTO());
                if (orderApprove.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest(orderApprove.Message);
                if (orderApprove.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    return Forbid(orderApprove.Message);
                return Ok(orderApprove.value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("return-order")]
        public async Task<IActionResult> ReturnOrder([FromBody] OrderReturnDTO? orderReturnDto)
        {
            try
            {
                var orderReturn = await _serviceManager.OrderService.ReturnOrder(orderReturnDto ?? new OrderReturnDTO());
                if (orderReturn.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest(orderReturn.Message);
                if (orderReturn.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    return Forbid(orderReturn.Message);
                return Ok(orderReturn.value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("refuse-order")]
        public async Task<IActionResult> RefuseOrder([FromForm] OrderRefuseDTO? orderRefuseDto)
        {
            try
            {
                var orderReturn = await _serviceManager.OrderService.RefuseOrder(orderRefuseDto ?? new OrderRefuseDTO());
                if (orderReturn.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest(orderReturn.Message);
                if (orderReturn.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    return Forbid(orderReturn.Message);
                return Ok(orderReturn.value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("export-order")]
        public async Task<IActionResult> ExportOrder([FromQuery(Name = "lang")] string lang, [FromQuery(Name = "url")] string url)
        {
            try
            {
                var stream = await _serviceManager.OrderService.ExportOrder(lang, url);
                string excelName = $"Order-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("export-order-refuse")]
        public async Task<IActionResult> ExportOrderRefuse([FromQuery(Name = "lang")] string lang)
        {
            try
            {
                var stream = await _serviceManager.OrderService.ExportOrderRefuse(lang);
                string excelName = $"OrderRefuse-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
