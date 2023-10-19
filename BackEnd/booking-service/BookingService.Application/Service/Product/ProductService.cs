using AutoMapper;
using BookingService.Domain;
using BookingService.Service.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Service
{

    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uom;
        private readonly IMapper _mapper;
        private readonly HttpRequest request;

        public ProductService(IUnitOfWork uom, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _uom = uom;
            _mapper = mapper;
            request = httpContextAccessor.HttpContext.Request;
        }
        public async Task<ResponseMessage<ProductDTO>> GetProduct(Guid referenceId)
        {
            try
            {
                var product = await _uom.Product.FirstOrDefault(p => p.ReferenceId == referenceId);
                ProductDTO productDTO = _mapper.Map<ProductDTO>(product);
                if (productDTO == null)
                {
                    return new ResponseMessage<ProductDTO>("PRODUCT_NOT_FOUND", HttpStatusCode.NotFound, new ProductDTO());
                }
                return new ResponseMessage<ProductDTO>("", HttpStatusCode.OK, productDTO);
            }
            catch (Exception ex)
            {
                return new ResponseMessage<ProductDTO>("Server Error", HttpStatusCode.BadRequest, new ProductDTO());
            }
        }
        public async Task<ResponseMessage<List<SelectResponseDTO>>> DropDownProduct(string query)
        {
            try
            {
                var page = int.TryParse(request.Query["page"].ToString(), out int v) ? v : 1;
                return new ResponseMessage<List<SelectResponseDTO>>("", System.Net.HttpStatusCode.OK, await _uom.Product.SelectProduct(query, page));
            }
            catch
            {
                return new ResponseMessage<List<SelectResponseDTO>>("", System.Net.HttpStatusCode.BadRequest, new List<SelectResponseDTO>());
            }
        }
        public async Task<ResponseMessage<List<SelectResponseDTO>>> DropDownProductPaging(string query, int skip, int top)
        {
            try
            {
                return new ResponseMessage<List<SelectResponseDTO>>("", System.Net.HttpStatusCode.OK, await _uom.Product.SelectProductPaging(query,skip,top));
            }
            catch
            {
                return new ResponseMessage<List<SelectResponseDTO>>("", System.Net.HttpStatusCode.BadRequest, new List<SelectResponseDTO>());
            }
        }
        public async Task<List<Product>> GetListProduct(List<string?> lst_qurey)
        {
            try
            {
                Expression<Func<Product, bool>> query = (o =>
                !string.IsNullOrEmpty(o.Product_Code)
                && lst_qurey.Contains(o.Product_Code));
                return await _uom.Product.GetByConditionTask(query);
            }
            catch (Exception)
            {
                return new List<Product>();
            }
        }

        public async Task<ResponseMessage<ProductDownloadDTO>> DownloadProduct(List<ProductDownloadDTO> lst_param, List<ProductDownloadDTO> lst_param_new, List<Product> entitys)
        {
            try
            {
                var lst_product = new List<Product>();
                foreach (var item_param in lst_param_new)
                {
                    if (lst_product.Any(a => a.Product_Code == item_param.Product_Code))
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(item_param.Product_Code) || string.IsNullOrEmpty(item_param.Product_Name))
                    {
                        return new ResponseMessage<ProductDownloadDTO>("Code/Name is empty !!!", HttpStatusCode.BadRequest, new ProductDownloadDTO());
                    }
                    var supplier = _mapper.Map<ProductDownloadDTO, Product>(item_param);
                    supplier.CreatedAt = DateTime.Now;
                    //supplier.CreatedBy = _currentUser.User.Id;
                    supplier.Status = item_param.Status ?? (int)Domain.Enum.Status.Active;
                    supplier.ReferenceId = Guid.NewGuid();
                    lst_product.Add(supplier);
                }

                var lst_product_edit = new List<Product>();
                foreach (var item_param in entitys)
                {
                    var record_temp = lst_param.FirstOrDefault(a => a.Product_Code == item_param.Product_Code);
                    if (record_temp != null)
                    {
                        if (string.IsNullOrEmpty(record_temp.Product_Code) || string.IsNullOrEmpty(record_temp.Product_Name))
                        {
                            return new ResponseMessage<ProductDownloadDTO>("Code/Name is empty !!!", HttpStatusCode.BadRequest, new ProductDownloadDTO());
                        }
                        item_param.Product_Name = record_temp.Product_Name;
                        item_param.ModifiedAt = DateTime.Now;
                        //item_param.ModifiedBy = _currentUser.User.Id;
                        item_param.Status = record_temp.Status ?? (int)Domain.Enum.Status.Active;//(int)Domain.Enum.Status.Active;
                        lst_product_edit.Add(item_param);
                    }
                }

                await _uom.BeginTransactionAsync();
                _uom.Product.CreateRange(lst_product);
                _uom.Product.UpdateRange(lst_product_edit);
                await _uom.SaveAsync();
                await _uom.CommitAsync();

                return new ResponseMessage<ProductDownloadDTO>("Download Success", HttpStatusCode.OK, new ProductDownloadDTO());

            }
            catch (Exception ex)
            {
                await _uom.RollBackAsync();
                return new ResponseMessage<ProductDownloadDTO>("Download Error", HttpStatusCode.BadRequest, new ProductDownloadDTO());
            }
        }
    }
}
