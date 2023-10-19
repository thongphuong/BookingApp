using AutoMapper;
using AutoMapper.Internal;
using BookingService.Domain;
using BookingService.Service.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _uom;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        private readonly HttpRequest request;

        public SupplierService(IUnitOfWork uom, IMapper mapper, ICurrentUserService currentUser, IHttpContextAccessor httpContextAccessor)
        {
            _uom = uom;
            _mapper = mapper;
            _currentUser = currentUser;
            request = httpContextAccessor.HttpContext.Request;
        }

        public async Task<ResponseMessage<List<SelectResponseDTO>>> DropDownSupplier(string query)
        {
            try
            {
                var page = int.TryParse(request.Query["page"].ToString(), out int v) ? v : 1;
                return new ResponseMessage<List<SelectResponseDTO>>("", System.Net.HttpStatusCode.OK, await _uom.Supplier.SelectSupplier(query, page));
            }
            catch
            {
                return new ResponseMessage<List<SelectResponseDTO>>("", System.Net.HttpStatusCode.BadRequest, new List<SelectResponseDTO>());
            }
        }

        public async Task<ResponseMessage<SupplierDTO>> GetSupplier(Guid referenceId)
        {
            try
            {
                var supplier = await _uom.Supplier.FirstOrDefault(p => p.ReferenceId == referenceId);
                SupplierDTO supplierDto = _mapper.Map<SupplierDTO>(supplier);
                if (supplierDto is null)
                    return new ResponseMessage<SupplierDTO>("SUPPLIER_NOT_FOUND", HttpStatusCode.NotFound, new SupplierDTO());
                return new ResponseMessage<SupplierDTO>("", HttpStatusCode.OK, supplierDto);
            }
            catch
            {
                return new ResponseMessage<SupplierDTO>("Server Error", HttpStatusCode.BadRequest, new SupplierDTO());
            }
        }

        public IQueryable<BookingService.Domain.Supplier> GetByCondition(System.Linq.Expressions.Expression<Func<Domain.Supplier, bool>> expression)
        {
            return _uom.Supplier.GetByCondition(expression);
        }

        public async Task<List<Supplier>> GetListSupplier(List<string?> lst_qurey)
        {
            try
            {
                Expression<Func<Supplier, bool>> query = (o => !string.IsNullOrEmpty(o.Code)
                && lst_qurey.Contains(o.Code));
                return await _uom.Supplier.GetByConditionTask(query);
            }
            catch (Exception)
            {
                return new List<Supplier>();
            }
        }
        public async Task<ResponseMessage<SupplierDownloadDTO>> DownloadSupplier(List<SupplierDownloadDTO> lst_param, List<SupplierDownloadDTO> lst_param_new, List<Supplier> entitys)
        {
            try
            {
                var lst_Supplier = new List<Supplier>();
                foreach (var item_param in lst_param_new)
                {
                    if (lst_Supplier.Any(a => a.Code == item_param.Code))
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(item_param.Code) || string.IsNullOrEmpty(item_param.Name))
                    {
                        return new ResponseMessage<SupplierDownloadDTO>("Code/Name is empty !!!", HttpStatusCode.BadRequest, new SupplierDownloadDTO());
                    }

                    var supplier = _mapper.Map<SupplierDownloadDTO, Supplier>(item_param);
                    supplier.CreatedAt = DateTime.Now;
                    //supplier.CreatedBy = _currentUser.User.Id;
                    supplier.Status = item_param.Status ?? (int)Domain.Enum.Status.Active;
                    supplier.ReferenceId = Guid.NewGuid();
                    lst_Supplier.Add(supplier);
                }

                var lst_Supplier_edit = new List<Supplier>();
                foreach (var item_param in entitys)
                {

                    var record_temp = lst_param.FirstOrDefault(a => a.Code == item_param.Code);
                    if (record_temp != null)
                    {
                        if (string.IsNullOrEmpty(record_temp.Code) || string.IsNullOrEmpty(record_temp.Name))
                        {
                            return new ResponseMessage<SupplierDownloadDTO>("Code/Name is empty !!!", HttpStatusCode.BadRequest, new SupplierDownloadDTO());
                        }
                        item_param.Phone_Number = record_temp.Phone_Number;
                        item_param.Email = record_temp.Email;
                        item_param.Name = record_temp.Name;
                        item_param.ModifiedAt = DateTime.Now;
                        //item_param.ModifiedBy = _currentUser.User.Id;
                        item_param.Status = record_temp.Status ?? (int)Domain.Enum.Status.Active;//(int)Domain.Enum.Status.Active;
                        lst_Supplier_edit.Add(item_param);
                    }
                }

                await _uom.BeginTransactionAsync();
                _uom.Supplier.CreateRange(lst_Supplier);
                _uom.Supplier.UpdateRange(lst_Supplier_edit);
                await _uom.SaveAsync();
                await _uom.CommitAsync();

                return new ResponseMessage<SupplierDownloadDTO>("Download Success", HttpStatusCode.OK, new SupplierDownloadDTO());

            }
            catch (Exception)
            {
                await _uom.RollBackAsync();
                return new ResponseMessage<SupplierDownloadDTO>("Download Error", HttpStatusCode.BadRequest, new SupplierDownloadDTO());
            }
        }
    }
}
