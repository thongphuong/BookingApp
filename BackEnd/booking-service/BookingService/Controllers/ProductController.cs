﻿using BookingService.Attribute;
using BookingService.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{
    [BaseAuthorize]
    [Route("product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ProductController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        [Route("get-product")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProduct([FromQuery(Name = "reference_id")] Guid? referenceId)
        {
            try
            {
                var productDTO = await _serviceManager.ProductService.GetProduct(referenceId ?? Guid.Empty);
                if (productDTO.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound(productDTO.Message);
                if (productDTO.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest();
                return Ok(productDTO.value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpGet]
        [Route("dropdown-product")]
        [AllowAnonymous]
        public async Task<IActionResult> DropDownProduct(string? q = "")
        {
            try
            {
                var select = await _serviceManager.ProductService.DropDownProduct(q);
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
        [Route("dropdown-product-paging")]
        [AllowAnonymous]
        public async Task<IActionResult> DropDownProductPaging(string? q = "",int Skip = 0,int Top = 0)
        {
            try
            {
                var select = await _serviceManager.ProductService.DropDownProductPaging(q, Skip, Top);
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
