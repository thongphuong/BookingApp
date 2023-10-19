using BookingService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using UserService.Domain;
using UserService.Service.Interface;
using Newtonsoft.Json;
using UserService.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using UserService.Infrastructure;
using UserService.Service;
using Microsoft.AspNetCore.Authorization;

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
        [Route("GetTable")]
        public async Task<IActionResult> Get([FromQuery] DataTableParams param)
        {
            try
            {
                var table = await _serviceManager.StoreService.TableStore(param);
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
        public async Task<IActionResult> AddData([FromForm] List<Service.Store.StoreModel> model)
        {
            try
            {
                var store = await _serviceManager.StoreService.InsertStore(model);
                if (store.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();

                }
                if (store.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return BadRequest(store.value);
                }
                if (store.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return Conflict("STORE_IS_EXIST");
                }
                return Ok();
            }

            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("EditData")]
        public async Task<IActionResult> EditData([FromForm] List<Service.Store.StoreModel> model)
        {
            try
            {
                var store = await _serviceManager.StoreService.UpdateStore(model);
                if (store.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();

                }
                if (store.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return BadRequest();
                }
                if (store.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return Conflict("STORE_IS_EXIST");
                }
                return Ok(store.value);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromBody] Service.Store.Param param)
        {
            try
            {
                var store = await _serviceManager.StoreService.DeleteStore(param);
                if (store.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();

                }
                if (store.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return BadRequest();
                }
                return Ok(store.value);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetEdit")]
        public async Task<IActionResult> GetEdit(string reference_id)
        {
            try
            {
                
                var result = await _serviceManager.StoreService.GetStoreById(reference_id);
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
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }



        }
        [HttpPost]
        [Route("Change")]
        public async Task<IActionResult> ChangeActive([FromBody] Service.Store.Param param)
        {

            try
            {
                var store = await _serviceManager.StoreService.ChangeActiveStore(param);
                if (store.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();

                }
                if (store.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return BadRequest();
                }
                return Ok(store.value);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

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
