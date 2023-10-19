using AutoMapper;
using BookingService.Domain;
using BookingService.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace BookingService.Service
{
    public class ReturnService :BaseService, IReturnService
    {
        private readonly IUnitOfWork _uom;
        //private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        private readonly HttpRequest request;
        private readonly IHostEnvironment _HostEnvironment;

        public ReturnService(IUnitOfWork uom, IMapper mapper, ICurrentUserService currentUser, IHttpContextAccessor httpContextAccessor, IHostEnvironment HostEnvironment) : base(uom, currentUser)
        {
            _uom = uom;
            //_jwtTokenGenerator = jwtTokenGenerator;
            _HostEnvironment = HostEnvironment;
            _currentUser = currentUser;
            request = httpContextAccessor.HttpContext.Request;

        }

        public async Task<ResponseMessage<DataTableResponse>> TableReturn(DataTableParams datatableParams)
        {
            try
            {
                var store = request.Query["store_reference"].ToString() ?? "";
                var line = request.Query["line_reference"].ToString() ?? "";
                var supplier = request.Query["supplier_reference"].ToString() ?? "";
                var department = request.Query["department_reference"].ToString() ?? "";
                var startdate = request.Query["start_date"].ToString() ?? "";
                var enddate = request.Query["end_date"].ToString() ?? "";

                var sqlquery = "  SELECT  RT.id,RT.date,RT.status,RT.reference_id,LN.Code AS 'line_code',LN.name AS 'line_name',DP.Code AS 'department_code',DP.Name AS 'department_name',ST.name AS 'store_name',SP.supplier_name AS 'supplier_name'" +
                                " FROM [dbo].[RETURN] RT" +
                                " LEFT JOIN" +
                                " [dbo].[STORE] ST" +
                                " ON [RT].store_reference = ST.reference_id" +
                                " LEFT JOIN" +
                                " [dbo].[LINE] LN" +
                                " ON [RT].line_reference = LN.reference_id" +
                                " LEFT JOIN" +
                                " [dbo].[LINE_DEPARTMENT] DP" +
                                " ON [RT].department_reference = DP.reference_id" +
                                " LEFT JOIN" +
                                " [dbo].[SUPPLIER] SP" +
                                " ON [RT].supplier_reference = SP.reference_id";

                var where = " WHERE RT.status != '2'";

                if (!string.IsNullOrEmpty(store))
                {
                    where += $" AND RT.store_reference = '{store}'";
                }
                if (!string.IsNullOrEmpty(line))
                {
                    where += $" AND RT.line_reference = '{line}'";
                }
                if (!string.IsNullOrEmpty(supplier))
                {
                    where += $" AND RT.supplier_reference = '{supplier}'";
                }
                if (!string.IsNullOrEmpty(department))
                {
                    where += $" AND RT.department_reference = '{department}'";
                }
                if (!string.IsNullOrEmpty(startdate))
                {
                    var _startdate = DateTime.ParseExact(startdate, "yyyy-MM-dd", null).ToString("yyyy-MM-dd");
                    where += $" AND RT.Date >= '{_startdate}'";
                }
                if (!string.IsNullOrEmpty(enddate))
                {
                    var _enddate = DateTime.ParseExact(enddate, "yyyy-MM-dd", null).ToString("yyyy-MM-dd");
                    where += $" AND RT.Date <= '{_enddate}'";
                }


                var model = _uom.Return.GetBySqlQuery(sqlquery + where);
                IEnumerable<ReturnDTO> filtered = model;

                var sortColumnIndex = Convert.ToInt32(request.Query["iSortCol_0"]);

                Func<ReturnDTO, object> orderingFunction = (s => sortColumnIndex == 1 ? s.Id
                                                            : sortColumnIndex == 2 ? s.Line_Name
                                                            : sortColumnIndex == 3 ? s.Department_Name
                                                            : sortColumnIndex == 4 ? s.Supplier_Name
                                                            : sortColumnIndex == 5 ? s.Store_Name
                                                            : sortColumnIndex == 6 ? s.Date
                                                           : (object)s.Date);

                var sortDirection = (request.Query["sSortDir_0"].ToString() ?? "asc"); // asc or desc

                filtered = (sortDirection == "asc") ? filtered.OrderBy(orderingFunction)
                                       : filtered.OrderByDescending(orderingFunction);

                var displayed = filtered.Skip(datatableParams.iDisplayStart).Take(datatableParams.iDisplayLength);


                var result = from c in displayed.ToArray()
                             select new object[] {
                                   string.Empty,
                                   c?.Line_Code + '-' + c?.Line_Name ,
                                   c?.Department_Code + '-' + c?.Department_Name,
                                   c?.Supplier_Name,
                                   c?.Store_Name,
                                   c.Date.HasValue ? c?.Date.Value.ToString("dd-MM-yyyy") : null,
                                    Utils.CommandButton(new List<string>(){ CheckPermissions("Return","Detail").Result ? "Detail" : "", CheckPermissions("Return", "Print").Result ? "Print" : "", CheckPermissions("Return", "Delete").Result ? "Delete" : "", CheckPermissions("Return", "Edit").Result ? "Edit" : ""},c.Reference_Id ?? Guid.Empty,c.Status)
                                //    $"<div style=\"min-width:70px;\"><button title=\"Detail\" type=\"button\" class=\"btn btn-action btn-info waves-effect\" onclick='ShowDetail(\"{c.Reference_Id}\")'><i class='material-icons'>view_list</i></button><button title=\"Detail\" type=\"button\" class=\"btn btn-action btn-info waves-effect\" onclick='ShowPrint(\"{c.Reference_Id}\")'><i class='material-icons'>print</i></button><button title=\"Edit\" type=\"button\" class=\"btn btn-action btn-warning waves-effect\" onclick='Edit(\"{c.Reference_Id}\")'><i class=\"material-icons\">edit</i></button><button type=\"button\" title=\"Change Active\" class=\"btn btn-action btn-primary waves-effect\" onclick='callchange(\"{c.Reference_Id}\")'><i class=\"material-icons\">{(c.Status==1?"visibility":"visibility_off")}</i></button>" +
                                //$"<button title=\"Delete\" type=\"button\" class=\"btn btn-action btn-danger  waves-effect\" onclick='calldelete(\"{c.Reference_Id}\")'><i class=\"material-icons\">delete</i></button></div>"


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
            catch (Exception ex) {
                return new ResponseMessage<DataTableResponse>("Server Error", HttpStatusCode.BadRequest, new DataTableResponse()
                {
                    iTotalDisplayRecords = 0,
                    iTotalRecords = 0,
                    sEcho = datatableParams.sEcho,
                    aaData = new List<object>()
                });
            }
        }

        public async Task<ResponseMessage<ReturnParam>> InsertReturn(ReturnParam param)
        {
            try {
                if (param.ReturnModel == null)
                {
                    return new ResponseMessage<ReturnParam>("", HttpStatusCode.BadRequest, new ReturnParam());
                }
                else {
                    await _uom.BeginTransactionAsync();
                    var entity = new Domain.Return();
                    var entity_detail = new List<Domain.ReturnDetail>();
                    entity.Store_Reference = param.ReturnModel.Store_Reference;
                    entity.Line_Reference = param.ReturnModel.Line_Reference;
                    entity.Department_Reference = param.ReturnModel.Department_Reference;
                    entity.Supplier_Reference = param.ReturnModel.Supplier_Reference;
                    entity.Supplier_Name = param.ReturnModel.Supplier_Name;
                    entity.Date = param.ReturnModel.Date;
                    entity.Time = param.ReturnModel.Time;
                    entity.CreatedAt = DateTime.Now;
                    entity.CreatedBy = _currentUser.User.Id;
                    entity.Status = (int)Domain.Enum.Status.Active;
                    entity.ReferenceId = Guid.NewGuid();
                    _uom.ReturnNew.Create(entity);
                    if (param.ReturnDetailModelCreate != null)
                    {

                        foreach (var item in param.ReturnDetailModelCreate)
                        {
                            var detail = new Domain.ReturnDetail();
                            detail.Product_Code = item.product_code;
                            detail.Product_Name = item.product_name;
                            detail.Product_Reference = item.product_reference;
                            detail.Number = item.number;
                            detail.Note = item.note;
                            detail.Return_Reference = entity.ReferenceId;
                            detail.CreatedAt = DateTime.Now;
                            detail.CreatedBy = _currentUser.User.Id;
                            detail.Status = (int)Domain.Enum.Status.Active;
                            detail.ReferenceId = Guid.NewGuid();
                            entity_detail.Add(detail);
                        }
                        _uom.ReturnDetail.CreateRange(entity_detail);
                    }
                }
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                return new ResponseMessage<ReturnParam>("Add Success", HttpStatusCode.OK, param);

            }
            catch (Exception ex) {
                await _uom.RollBackAsync();
                return new ResponseMessage<ReturnParam>("Add Error", HttpStatusCode.BadRequest, param);
            }

        }

        public async Task<ResponseMessage<ReturnParam>> GetReturn(string? reference_id) {

            try
            {
                var result = new ReturnParam();
                Expression<Func<Domain.Return, bool>> searchReturn = (s => (s.ReferenceId.ToString() == reference_id));
                Expression<Func<Domain.ReturnDetail, bool>> searchReturnDetail = (s => (s.Return_Reference.ToString() == reference_id));
                var header = _uom.ReturnNew.FindByCondition(searchReturn).FirstOrDefault();
                var detail = _uom.ReturnDetail.FindByCondition(searchReturnDetail).ToList().Take(100);

                if (header != null)
                {
                    var line = _uom.Line.FindByCondition(s => s.ReferenceId == header.Line_Reference).FirstOrDefault();
                    var store = _uom.Store.FindByCondition(s => s.reference_id == header.Store_Reference).FirstOrDefault();
                    var department = _uom.LineDepartment.FindByCondition(s => s.ReferenceId == header.Department_Reference).FirstOrDefault();
                    var supplier = _uom.Supplier.FindByCondition(s => s.ReferenceId == header.Supplier_Reference).FirstOrDefault();
                    var returnmodel = new ReturnModel();
                    returnmodel.Line_Reference = header.Line_Reference;
                    returnmodel.Line_Name = line != null ? (line.Code + '-' +  line.Name)   : null;
                    returnmodel.Store_Reference = header.Store_Reference;
                    returnmodel.Store_Name = store != null ? store.name : null;
                    returnmodel.Supplier_Reference = header.Supplier_Reference;
                    returnmodel.Supplier_Name = supplier != null ? supplier.Name : null;
                    returnmodel.Supplier_Code = supplier != null ? supplier.Code : null;
                    returnmodel.Department_Reference = header.Department_Reference;
                    returnmodel.Department_Name = department != null ? (department.Code + '-' + department.Name) : null;
                    returnmodel.Reference_Id = header.ReferenceId;
                    returnmodel.Date = header.Date;
                    returnmodel.Time = header.Time;
                    returnmodel.Id = header.Id;
                    //var detail =  _uom.ReturnDetail.FindByCondition(searchReturnDetail).ToList();
                    var returnmodeldetail = new List<ReturnDetailModel>();
                    if (detail != null)
                    {

                        foreach (var item in detail)
                        {
                            returnmodeldetail.Add(new ReturnDetailModel
                            {
                                product_code = item.Product_Code,
                                product_name = item.Product_Name,
                                number = item.Number,
                                note = item.Note,
                                product_reference = item.Product_Reference,
                                return_reference = item.Return_Reference,
                                id = item.Id,
                                reference_id = item.ReferenceId
                            });
                        }
                    }
                    result.ReturnModel = returnmodel;
                    result.ReturnDetailModelUpdate = returnmodeldetail;
                    return new ResponseMessage<ReturnParam>("", HttpStatusCode.OK, result);
                }
                else
                {
                    return new ResponseMessage<ReturnParam>("", HttpStatusCode.NotFound, new ReturnParam());
                }
            }
            catch (Exception ex)
            {
                return new ResponseMessage<ReturnParam>("", HttpStatusCode.BadRequest, new ReturnParam());
            }


        }

        public async Task<ResponseMessage<Return>> ChangeActiveReturn(ReturnSearch searchparam)
        {
            Expression<Func<Domain.Return, bool>> searchFunction = (s => (s.ReferenceId.ToString() == searchparam.referenceId));
            var data = _uom.ReturnNew.FindByCondition(searchFunction).FirstOrDefault();
            try {
                if (data == null)
                {
                    return new ResponseMessage<Domain.Return>("Change Active Error", HttpStatusCode.BadRequest, data);
                }
                else {
                    await _uom.BeginTransactionAsync();
                    var flag = data.Status;
                    if (flag == 0)
                    {
                        data.Status = (int)Domain.Enum.Status.Active;
                    }
                    else
                    {
                        if (flag == 1)
                        {
                            data.Status = (int)Domain.Enum.Status.Inactive;
                        }
                        data.ModifiedAt = DateTime.Now;
                        data.ModifiedBy = _currentUser.User.Id;
                    }
                    _uom.ReturnNew.Update(data);
                    await _uom.SaveAsync();
                    await _uom.CommitAsync();
                    return new ResponseMessage<Domain.Return>("Change Active Success", HttpStatusCode.OK, data);
                }
            }
            catch (Exception ex) {
                return new ResponseMessage<Domain.Return>("Change Active Error", HttpStatusCode.OK, data);
            }
        }
        public async Task<ResponseMessage<Return>> DeleteReturn(ReturnSearch searchparam) {
            Expression<Func<Domain.Return, bool>> searchFunction = (s => (s.ReferenceId.ToString() == searchparam.referenceId));
            var data = _uom.ReturnNew.FindByCondition(searchFunction).FirstOrDefault();
            try {
                if (data == null)
                {
                    return new ResponseMessage<Domain.Return>("Delete Error", HttpStatusCode.BadRequest, data);
                }
                else {
                    await _uom.BeginTransactionAsync();
                    data.Status = (int)Domain.Enum.Status.Delete;
                    data.DeletedAt = DateTime.Now;
                    data.DeteleBy = _currentUser.User.Id;
                    _uom.ReturnNew.Update(data);
                    await _uom.SaveAsync();
                    await _uom.CommitAsync();
                    return new ResponseMessage<Return>("Delete Success", HttpStatusCode.OK, data);
                }
            }
            catch (Exception ex) {
                return new ResponseMessage<Domain.Return>("Delete Error", HttpStatusCode.OK, data);
            }
        }

    public async Task<ResponseMessage<ReturnParam>> UpdateReturn(ReturnParam param) {

            try
            {

                if (param.ReturnModel == null)
                {
                    return new ResponseMessage<ReturnParam>("", HttpStatusCode.BadRequest, new ReturnParam());
                }

                //var entity = new Domain.Return();
                var modify_entity = new List<Domain.ReturnDetail>();
                var create_entity = new List<Domain.ReturnDetail>();
                var delete_entity = new List<Domain.ReturnDetail>();
                var result = _uom.ReturnNew.FindByCondition(s => (s.ReferenceId == param.ReturnModel.Reference_Id) ||
                (s.Id == param.ReturnModel.Id)).FirstOrDefault();

                await _uom.BeginTransactionAsync();
                //check if header null
                if (result != null)
                {
                    var header = param.ReturnModel;
                    result.Supplier_Reference = header.Supplier_Reference;
                    result.Store_Reference = header.Store_Reference;
                    result.Line_Reference = header.Line_Reference;
                    result.Department_Reference = header.Department_Reference;
                    result.Supplier_Name = header.Supplier_Name;
                    result.Date = header.Date;
                    result.Time = header.Time;
                    _uom.ReturnNew.Update(result);
                }
                else
                {
                    return new ResponseMessage<ReturnParam>("", HttpStatusCode.NotFound, new ReturnParam());
                }
                // create detail
                if (param.ReturnDetailModelCreate != null)
                {
                    foreach (var detail in param.ReturnDetailModelCreate)
                    {
                        var entity = new Domain.ReturnDetail();
                        entity.Product_Reference = detail.product_reference;
                        entity.Product_Code = detail.product_code;
                        entity.Product_Name = detail.product_name;
                        entity.Number = detail.number;
                        entity.Note = detail.note;
                        entity.Return_Reference = detail.return_reference;
                        entity.CreatedAt = DateTime.Now;
                        entity.CreatedBy = _currentUser.User.Id;
                        entity.Status = (int)Domain.Enum.Status.Active;
                        entity.ReferenceId = Guid.NewGuid();
                        create_entity.Add(entity);
                    }
                    _uom.ReturnDetail.CreateRange(create_entity);
                }
                //update detail
                if (param.ReturnDetailModelUpdate != null)
                {

                    foreach (var detail in param.ReturnDetailModelUpdate)
                    {
                        var resultdetail = _uom.ReturnDetail.FindByCondition(s => (s.ReferenceId == detail.reference_id)).FirstOrDefault();
                        if (resultdetail != null)
                        {
                            resultdetail.Product_Reference = detail.product_reference;
                            resultdetail.Product_Code = detail.product_code;
                            resultdetail.Product_Name = detail.product_name;
                            resultdetail.Number = detail.number;
                            resultdetail.Note = detail.note;
                            resultdetail.ModifiedAt = DateTime.Now;
                            resultdetail.ModifiedBy = _currentUser.User.Id;
                            modify_entity.Add(resultdetail);
                        }

                    }
                    _uom.ReturnDetail.UpdateRange(modify_entity);

                }
                //delete detail
                if (param.ReturnDetailModelDelete != null)
                {
                    foreach (var detail in param.ReturnDetailModelDelete)
                    {
                        var resultdetail = _uom.ReturnDetail.FindByCondition(s => (s.ReferenceId == detail.reference_id)).FirstOrDefault();
                        if (resultdetail != null)
                        {
                            delete_entity.Add(resultdetail);
                        }
                    }
                    _uom.ReturnDetail.DeleteRange(delete_entity);
                }

                await _uom.SaveAsync();
                await _uom.CommitAsync();
                return new ResponseMessage<ReturnParam>("Update Success", HttpStatusCode.OK, param);

            }

            catch (Exception ex) {
                return new ResponseMessage<ReturnParam>("", HttpStatusCode.BadRequest, new ReturnParam());
            }


        }      
    }
}
