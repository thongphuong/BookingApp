using BookingService.Service;
using BookingService.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookingService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MasterDataController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public MasterDataController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost]
        [Route("download_supplier")]

        public async Task<IActionResult> DownSupplier([FromBody] List<SupplierDownloadDTO> suppliers)
        {

            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(new ResponseMessage<SupplierDownloadDTO>("Max size 3000 records.", HttpStatusCode.BadRequest, new SupplierDownloadDTO()));
                //}
                if (suppliers.Count == 0)
                {
                    return NotFound();
                }
                if (suppliers.Count > 3000)
                {
                    return BadRequest(new ResponseMessage<SupplierDownloadDTO>("Max size 3000 records.", HttpStatusCode.BadRequest, new SupplierDownloadDTO()));
                }
                var code_list_down = suppliers.Select(x => x.Code).ToList();
                var data_code_exist = await _serviceManager.SupplierService.GetListSupplier(code_list_down);
                var code_exist = data_code_exist.Select(a => a.Code).ToList();
                var list_suppliers = suppliers.Where(a => !code_exist.Contains(a.Code)).ToList();
                var result = await _serviceManager.SupplierService.DownloadSupplier(suppliers, list_suppliers, data_code_exist);

                if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return BadRequest(result);
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseMessage<SupplierDownloadDTO>("Download Error", HttpStatusCode.BadRequest, new SupplierDownloadDTO()));
            }
        }

        [HttpPost]
        [Route("download_line")]

        public async Task<IActionResult> DownLine([FromBody] List<LineDownloadDTO> lines)
        {

            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(new ResponseMessage<LineDownloadDTO>("Max size 3000 records.", HttpStatusCode.BadRequest, new LineDownloadDTO()));
                //}
                if (lines.Count == 0)
                {
                    return NotFound();
                }
                if (lines.Count > 3000)
                {
                    return BadRequest(new ResponseMessage<LineDownloadDTO>("Max size 3000 records.", HttpStatusCode.BadRequest, new LineDownloadDTO()));
                }
                var code_list_down = lines.Select(x => x.Code).ToList();
                var data_code_exist = await _serviceManager.LineService.GetListLine(code_list_down);
                var code_exist = data_code_exist.Select(a => a.Code).ToList();
                var list_lines = lines.Where(a => !code_exist.Contains(a.Code)).ToList();
                var result = await _serviceManager.LineService.DownloadLine(lines, list_lines, data_code_exist);

                if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return BadRequest(result);
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseMessage<LineDownloadDTO>("Download Error", HttpStatusCode.BadRequest, new LineDownloadDTO()));
            }
        }
        [HttpPost]
        [Route("download_department")]

        public async Task<IActionResult> DownDepartment([FromBody] List<DepartmentDownloadDTO> line_deparments)
        {

            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(new ResponseMessage<DepartmentDownloadDTO>("Max size 3000 records.", HttpStatusCode.BadRequest, new DepartmentDownloadDTO()));
                //}
                if (line_deparments.Count == 0)
                {
                    return NotFound();
                }
                if (line_deparments.Count > 3000)
                {
                    return BadRequest(new ResponseMessage<DepartmentDownloadDTO>("Max size 3000 records.", HttpStatusCode.BadRequest, new DepartmentDownloadDTO()));
                }

                var data_line = await _serviceManager.LineService.GetListLineEntity();
                var code_list_down = line_deparments.Select(x => x.Code).ToList();
                var data_code_exist = await _serviceManager.LineService.GetListDepartment(code_list_down);
                var code_exist = data_code_exist.Select(a => a.Code).ToList();
                var list_line_departments = line_deparments.Where(a => !code_exist.Contains(a.Code)).ToList();
                var result = await _serviceManager.LineService.DownloadLineDepartment(data_line, line_deparments, list_line_departments, data_code_exist);

                if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest(new ResponseMessage<DepartmentDownloadDTO>("Download Error", HttpStatusCode.BadRequest, new DepartmentDownloadDTO()));
            }
        }

        [HttpPost]
        [Route("download_product")]

        public async Task<IActionResult> Downproduct([FromBody] List<ProductDownloadDTO> products)
        {

            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(new ResponseMessage<ProductDownloadDTO>("Max size 3000 records.", HttpStatusCode.BadRequest, new ProductDownloadDTO()));
                //}
                if (products.Count == 0)
                {
                    return NotFound();
                }
                if (products.Count > 3000)
                {
                    return BadRequest(new ResponseMessage<ProductDownloadDTO>("Max size 3000 records.", HttpStatusCode.BadRequest, new ProductDownloadDTO()));
                }
                var code_list_down = products.Select(x => x.Product_Code).ToList();
                var data_code_exist = await _serviceManager.ProductService.GetListProduct(code_list_down);
                var code_exist = data_code_exist.Select(a => a.Product_Code).ToList();
                var list_products = products.Where(a => !code_exist.Contains(a.Product_Code)).ToList();
                var result = await _serviceManager.ProductService.DownloadProduct(products, list_products, data_code_exist);

                if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return BadRequest(result);
                }
                return Ok(result);

            }
            catch (Exception)
            {
                return BadRequest(new ResponseMessage<ProductDownloadDTO>("Download Error", HttpStatusCode.BadRequest, new ProductDownloadDTO()));
            }
        }
    }
}
