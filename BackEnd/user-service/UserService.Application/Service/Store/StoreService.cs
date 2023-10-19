using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserService.Service.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Net;
using UserService.Service.DTO;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using UserService.Service.Store;

namespace UserService.Service.Store
{
    public class StoreService : IStoreService
    {
        private readonly IUnitOfWork _uom;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        private readonly HttpRequest request;
        private readonly IHostEnvironment _HostEnvironment;

        private readonly string BASE_DOMAIN = "https://localhost:7278";

        public StoreService(IUnitOfWork uom, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper, ICurrentUserService currentUser, IHttpContextAccessor httpContextAccessor, IHostEnvironment HostEnvironment)
        {
            _uom = uom;
            _jwtTokenGenerator = jwtTokenGenerator;
            _HostEnvironment = HostEnvironment;
            _currentUser = currentUser;
            request = httpContextAccessor.HttpContext.Request;

        }

        public Task<Domain.Store> CreateAsync(Domain.Store ownerForCreationDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid ownerId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Domain.Store> GetAllStoreByCondition(Expression<Func<Domain.Store, bool>> expression)
        {
            return _uom.Store.GetAllStoreByCondition(expression);
        }

        public Task<IEnumerable<Domain.Store>> GetAllAsync(CancellationToken token = default)
        {
            throw new NotImplementedException();
        }




        public IQueryable<Domain.Store> GetAllLangCode(Func<Domain.Store, bool> query = null)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseMessage<Domain.Store>> GetStoreById(string reference_id)
        {
            try
            {
                Expression<Func<Domain.Store, bool>> searchFunction = (s => (s.reference_id.ToString() == reference_id));
                var store = _uom.Store.GetAllStoreByCondition(searchFunction).FirstOrDefault();
                if (store == null)
                {
                    return new ResponseMessage<Domain.Store>("STORE NOT FOUND", HttpStatusCode.NotFound, new Domain.Store());
                }
                return new ResponseMessage<Domain.Store>("", HttpStatusCode.OK, store);
            }
            catch (Exception ex)
            {
                return new ResponseMessage<Domain.Store>("Server Error", HttpStatusCode.BadRequest, new Domain.Store());

            }


        }

        public Task UpdateAsync(Guid ownerId, Domain.Store ownerForUpdateDto)
        {
            throw new NotImplementedException();
        }

        public Task<Domain.Store> GetByIdAsync(Guid ownerId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Domain.Store> GetAll(Func<Domain.Store, bool> query = null)
        {
            return _uom.Store.GetAll();
        }

        public async Task<ResponseMessage<DataTableResponse>> TableStore(DataTableParams datatableParams)
        {
            try
            {
                var name = string.IsNullOrEmpty(request.Query["Name"]) ? "" : request.Query["Name"].ToString();
                var address = string.IsNullOrEmpty(request.Query["Address"]) ? "" : request.Query["Address"].ToString();
                var phonenum = string.IsNullOrEmpty(request.Query["PhoneNumber"]) ? "" : request.Query["PhoneNumber"].ToString();
                var email = string.IsNullOrEmpty(request.Query["Email"]) ? "" : request.Query["Email"].ToString();
                var status = string.IsNullOrEmpty(request.Query["Status"]) ? -1 : int.Parse(request.Query["Status"]);
                var code = string.IsNullOrEmpty(request.Query["Code"]) ? "" : request.Query["Code"].ToString();

                Expression<Func<Domain.Store, bool>> searchFunction = (s => (s.name.ToLower().Trim().Contains(name.ToLower().Trim()) || name == "")
                  && (s.address.ToLower().Trim().Contains(address.ToLower().Trim()) || address == "")
                     && (s.phone_number.ToLower().Contains(phonenum.ToLower()) || phonenum == "")
                     && (s.email.ToLower().Trim().Contains(email.ToLower().Trim()) || email == "")
                     && (s.status == status || status == -1)
                     && (s.code.ToString() == code || code == ""));

                var model = _uom.Store.GetAllStoreByCondition(searchFunction).Where(p => p.status != (int)Domain.Enum.Status.Delete);
                IEnumerable<Domain.Store> filtered = model;
                var sortColumnIndex = Convert.ToInt32(request.Query["iSortCol_0"]);

                Func<Domain.Store, object> orderingFunction = (s => sortColumnIndex == 1 ? s.code
                                                            : sortColumnIndex == 3 ? s.name
                                                            : sortColumnIndex == 4 ? s.phone_number
                                                            : sortColumnIndex == 5 ? address
                                                            : (object)s.name);

                var sortDirection = (request.Query["sSortDir_0"].ToString() ?? "asc"); // asc or desc

                filtered = (sortDirection == "asc") ? filtered.OrderBy(orderingFunction)
                                       : filtered.OrderByDescending(orderingFunction);

                var displayed = filtered.Skip(datatableParams.iDisplayStart).Take(datatableParams.iDisplayLength);
                var result = from c in displayed.ToArray()
                             select new object[] {
                                string.Empty,
                                c?.code,
                                //$"<img src=\"{c.url_image}\" class=\"img-responsive center-block\" style=\"width:100px;height:70px;\" />",
                                c?.name,
                                c?.email+"</br>"+ c?.phone_number,
                                c?.address,
                                //c?.phone_number,
                                //c?.status == 1 ? "<span class='badge bg-orange'>Active</span>" : "<span class='badge bg-pink'>Inactive</span>",
                                $"<div style=\"min-width:70px;\"><button title=\"Detail\" type=\"button\" class=\"btn btn-action btn-info waves-effect\" onclick='ShowDetail(\"{c.reference_id}\")'><i class='material-icons'>view_list</i></button><button title=\"Edit\" type=\"button\" class=\"btn btn-action btn-warning waves-effect\" onclick='Edit(\"{c.reference_id}\")'><i class=\"material-icons\">edit</i></button><button type=\"button\" title=\"Change Active\" class=\"btn btn-action btn-primary waves-effect\" onclick='callchange(\"{c.reference_id}\")'><i class=\"material-icons\">{(c.status==1?"visibility":"visibility_off")}</i></button>" +
                                $"<button title=\"Delete\" type=\"button\" class=\"btn btn-action btn-danger  waves-effect\" onclick='calldelete(\"{c.reference_id}\")'><i class=\"material-icons\">delete</i></button></div>"
                        };

                return new ResponseMessage<DataTableResponse>("", HttpStatusCode.OK,
                  new DataTableResponse
                  {
                      sEcho = datatableParams.sEcho,
                      iTotalRecords = filtered.Count(),
                      iTotalDisplayRecords = filtered.Count(),
                      aaData = result
                  });
            }
            catch (OperationCanceledException ex)
            {
                return new ResponseMessage<DataTableResponse>("Server Error", HttpStatusCode.BadRequest, new DataTableResponse()
                {
                    iTotalDisplayRecords = 0,
                    iTotalRecords = 0,
                    sEcho = datatableParams.sEcho,
                    aaData = new List<object>()
                });
            }
        }

        public async Task<ResponseMessage<List<Domain.Store>>> ChangeActiveStore(Param param)
        {

            Expression<Func<Domain.Store, bool>> searchFunction = (s => (s.reference_id.ToString() == param.referenceId));
            var listdata = _uom.Store.GetAllStoreByCondition(searchFunction).ToList();

            try
            {
                if (listdata.Count() == 0)
                {
                    return new ResponseMessage<List<Domain.Store>>("Delete Error", HttpStatusCode.BadRequest, listdata);
                }

                foreach (var entity in listdata)
                {
                    var flag = entity.status;
                    if (flag == 0)
                    {
                        entity.status = (int)Domain.Enum.Status.Active;
                    }
                    else
                    {
                        if (flag == 1)
                        {
                            entity.status = (int)Domain.Enum.Status.Inactive;
                        }
                    }
                    entity.modify_at = DateTime.Now;
                    entity.modify_by = _currentUser.User.Id;
                }
                await _uom.BeginTransactionAsync();
                _uom.Store.UpdateRange(listdata.ToList());
                await _uom.SaveAsync();
                await _uom.CommitAsync();

                return new ResponseMessage<List<Domain.Store>>("Change Active Success", HttpStatusCode.OK, listdata);
            }
            catch (Exception)
            {
                return new ResponseMessage<List<Domain.Store>>("Change Active Error", HttpStatusCode.OK, listdata);
            }
        }
        public async Task<ResponseMessage<List<Domain.Store>>> InsertStore(List<StoreModel> model)
        {
            var entity = new Domain.Store();

            var stores = new List<Domain.Store>();
            var itemvn = model.FirstOrDefault();
            Expression<Func<Domain.Store, bool>> searchbycondition = (s => (s.code == Convert.ToInt32(itemvn.code)));
            var check = _uom.Store.FindByCondition(searchbycondition).Where(p =>p.status != (int)Domain.Enum.Status.Delete).Any();
            if (check)
            {
                return new ResponseMessage<List<Domain.Store>>("Code Exist", HttpStatusCode.Created, stores);
            }
            entity.reference_id = Guid.NewGuid();
            entity.address = itemvn.address;
            entity.email = itemvn.email;
            entity.phone_number = itemvn.phone_number;
            entity.code = Convert.ToInt32(itemvn.code);
            entity.name = itemvn.name;
            if (itemvn.ImageFile != null)
            {
                entity.url_image = Utils.CreateImageURL(itemvn.ImageFile, "Store");
            }
            entity.url_image = "";
            entity.status = (int)Domain.Enum.Status.Active;
            entity.created_at = DateTime.Now;
            entity.created_by = _currentUser.User.Id;

            stores.Add(entity);
            try
            {
                await _uom.BeginTransactionAsync();
                _uom.Store.CreateRange(stores);
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                //return new ResponseMessage<Domain.Store>("Add Success", HttpStatusCode.OK, entity);
                return new ResponseMessage<List<Domain.Store>>("Add Success", HttpStatusCode.OK, stores);
            }
            catch (Exception ex)
            {
                await _uom.RollBackAsync();
                return new ResponseMessage<List<Domain.Store>>("Add Error", HttpStatusCode.BadRequest, stores);
            }
        }

        public async Task<ResponseMessage<List<Domain.Store>>> UpdateStore(List<StoreModel> data)
        {

            string urlimg = string.Empty;
            var stores = new List<Domain.Store>();
            var stores_new = new List<Domain.Store>();
            var itemvn = data.FirstOrDefault();
            Expression<Func<Domain.Store, bool>> searchbycondition = (s => (s.code == Convert.ToInt32(itemvn.code) 
            && ((s.reference_id != itemvn.reference_id) || (s.id != itemvn.id))));

            var check = _uom.Store.FindByCondition(searchbycondition).Where(p => p.status != (int)Domain.Enum.Status.Delete).Any();
            if (check)
            {
                return new ResponseMessage<List<Domain.Store>>("Code Exist", HttpStatusCode.Created, stores);
            }
            foreach (var item in data)
            {
                Expression<Func<Domain.Store, bool>> searchFunction = (s => (s.id == Convert.ToInt32(item.id)));
                var entity = _uom.Store.GetAllStoreByCondition(searchFunction).FirstOrDefault();
                if (entity != null)
                {
                    entity.name = item.name ?? itemvn.name;
                    entity.address = item.address ?? itemvn.address;
                    entity.email = item.email;
                    entity.phone_number = item.phone_number;
                    entity.code = Convert.ToInt32(item.code);
                    if (item.ImageFile != null && string.IsNullOrEmpty(urlimg))
                    {
                        urlimg = Utils.CreateImageURL(item.ImageFile, "Store");
                    }
                    if (!string.IsNullOrEmpty(urlimg))
                    {
                        entity.url_image = urlimg;
                    }
                    entity.modify_at = DateTime.Now;
                    entity.modify_by = _currentUser.User.Id;

                    stores.Add(entity);
                }
                else
                {
                    var _entity = JsonConvert.DeserializeObject<Domain.Store>(JsonConvert.SerializeObject(itemvn));
                    if (!string.IsNullOrEmpty(item.name))
                    {
                        _entity.name = item.name;
                    }

                    if (!string.IsNullOrEmpty(item.address))
                    {
                        _entity.address = item.address;
                    }
                    stores_new.Add(_entity);
                }


            }
            try
            {
                await _uom.BeginTransactionAsync();
                _uom.Store.UpdateRange(stores);
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                return new ResponseMessage<List<Domain.Store>>("update Success", HttpStatusCode.OK, stores);

            }
            catch (Exception ex)
            {
                return new ResponseMessage<List<Domain.Store>>("update error", HttpStatusCode.BadRequest, stores);
            }
        }

        public async Task<ResponseMessage<List<Domain.Store>>> DeleteStore(Param param)
        {

            Expression<Func<Domain.Store, bool>> searchFunction = (s => (s.reference_id.ToString() == param.referenceId));
            var listdata = _uom.Store.GetAllStoreByCondition(searchFunction).ToList();
            try
            {

                if (listdata.Count() == 0)
                {
                    return new ResponseMessage<List<Domain.Store>>("Delete Error", HttpStatusCode.BadRequest, listdata);
                }

                foreach (var entity in listdata)
                {
                    entity.status = (int)Domain.Enum.Status.Delete;
                    entity.deleted_at = DateTime.Now;
                    entity.deleted_by = _currentUser.User.Id;
                }
                await _uom.BeginTransactionAsync();
                _uom.Store.UpdateRange(listdata.ToList());
                await _uom.SaveAsync();
                await _uom.CommitAsync();

                return new ResponseMessage<List<Domain.Store>>("Delete Success", HttpStatusCode.OK, listdata);
            }
            catch (Exception)
            {
                return new ResponseMessage<List<Domain.Store>>("Delete Error", HttpStatusCode.BadRequest, listdata);
            }
        }
        public async Task<ResponseMessage<Domain.Store>> GetStore(Guid referenceId)
        {
            try {
                var entity = await _uom.Store.FirstOrDefault(p => p.reference_id == referenceId);
                if (entity == null)
                {
                    return new ResponseMessage<Domain.Store>("STORE_NOT_FOUND", HttpStatusCode.NotFound, new Domain.Store());
                }
                return new ResponseMessage<Domain.Store>("", HttpStatusCode.OK, entity);
            }
            catch (Exception ex) {
                return new ResponseMessage<Domain.Store>("Server Error", HttpStatusCode.BadRequest, new Domain.Store());
            }
            
        }
        public async Task<ResponseMessage<List<SelectResponseDTO>>> DropDownStore(string query)
        {
            try
            {
                return new ResponseMessage<List<SelectResponseDTO>>("", System.Net.HttpStatusCode.OK, await _uom.Store.SelectStore(query));
            }
            catch
            {
                return new ResponseMessage<List<SelectResponseDTO>>("", System.Net.HttpStatusCode.BadRequest, new List<SelectResponseDTO>());
            }
        }
    }
}
