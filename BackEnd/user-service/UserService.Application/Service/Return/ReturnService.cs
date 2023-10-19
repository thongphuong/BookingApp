using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UserService.Service.DTO.Return;
using UserService.Service.Interface;
using UserService.Service.Store;

namespace UserService.Service.Return
{
    public class ReturnService : IReturnService
    {
        private readonly IUnitOfWork _uom;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        private readonly HttpRequest request;
        private readonly IHostEnvironment _HostEnvironment;

        public ReturnService(IUnitOfWork uom, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper, ICurrentUserService currentUser, IHttpContextAccessor httpContextAccessor, IHostEnvironment HostEnvironment)
        {
            _uom = uom;
            _jwtTokenGenerator = jwtTokenGenerator;
            _HostEnvironment = HostEnvironment;
            _currentUser = currentUser;
            request = httpContextAccessor.HttpContext.Request;

        }

        public async Task<ResponseMessage<DataTableResponse>> TableReturn(DataTableParams datatableParams)
        {
            try
            {
                var store = request.Query["store_reference"].ToString() ??  "";
                var line = request.Query["line_reference"].ToString() ?? "";
                var supplier = request.Query["supplier_reference"].ToString() ?? "";
                var department= request.Query["department_reference"].ToString() ?? "";
                var startdate = request.Query["start_date"].ToString() ?? "";
                var enddate = request.Query["end_date"].ToString() ?? "";

                var sqlquery = "  SELECT  RT.*,LN.name AS 'line_name',DP.Name AS 'department_name',ST.name AS 'store_name'" +
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
                                                           : (object)s.Date) ;

                var sortDirection = (request.Query["sSortDir_0"].ToString() ?? "asc"); // asc or desc

                filtered = (sortDirection == "asc") ? filtered.OrderBy(orderingFunction)
                                       : filtered.OrderByDescending(orderingFunction);

                var displayed = filtered.Skip(datatableParams.iDisplayStart).Take(datatableParams.iDisplayLength);


                var result = from c in displayed.ToArray()
                             select new object[] {
                                   string.Empty,
                                   c?.Line_Name,
                                   c?.Department_Name,
                                   c?.Supplier_Name,
                                   c?.Store_Name,
                                   c?.Date.ToString("MM-dd-yyyy"),
                                    $"<div style=\"min-width:70px;\"><button title=\"Detail\" type=\"button\" class=\"btn btn-action btn-info waves-effect\" onclick='ShowDetail(\"{c.Reference_Id}\")'><i class='material-icons'>view_list</i></button><button title=\"Edit\" type=\"button\" class=\"btn btn-action btn-warning waves-effect\" onclick='Edit(\"{c.Reference_Id}\")'><i class=\"material-icons\">edit</i></button><button type=\"button\" title=\"Change Active\" class=\"btn btn-action btn-primary waves-effect\" onclick='callchange(\"{c.Reference_Id}\")'><i class=\"material-icons\">{(c.Status==1?"visibility":"visibility_off")}</i></button>" +
                                $"<button title=\"Delete\" type=\"button\" class=\"btn btn-action btn-danger  waves-effect\" onclick='calldelete(\"{c.Reference_Id}\")'><i class=\"material-icons\">delete</i></button></div>"

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
            
                var entity = new Domain.Return();
                var entity_detail = new List<Domain.ReturnDetail>();
                entity.Store_Reference = param.ReturnModel.supplier_Reference;
                entity.Line_Reference = param.ReturnModel.line_Reference;
                entity.Department_Reference = param.ReturnModel.department_Reference;
                entity.Supplier_Reference = param.ReturnModel.supplier_Reference;
                entity.Date = param.ReturnModel.date;
                entity.Time = param.ReturnModel.time;
                entity.CreatedAt = DateTime.Now;
                entity.CreatedBy = _currentUser.User.Id;
                entity.Status = (int)Domain.Enum.Status.Active;
                entity.ReferenceId = Guid.NewGuid();

                foreach (var item in param.ReturnDetailModel)
                {
                    var detail = new Domain.ReturnDetail();
                    detail.Product_Code = item.product_code;
                    detail.Product_Name = item.product_name;
                    detail.Number = item.number;
                    detail.Note = item.note;
                    detail.Return_Reference = entity.ReferenceId;
                    detail.CreatedAt = DateTime.Now;
                    detail.CreatedBy = _currentUser.User.Id;
                    detail.Status = (int)Domain.Enum.Status.Active;
                    detail.ReferenceId = Guid.NewGuid();
                    entity_detail.Add(detail);
                }
               
               try
               {
                     await _uom.BeginTransactionAsync();
                     _uom.ReturnNew.Create(entity);
                     _uom.ReturnDetail.CreateRange(entity_detail);
                     await _uom.SaveAsync();
                     await _uom.CommitAsync();

                     return new ResponseMessage<ReturnParam>("Add Success", HttpStatusCode.OK, param);

               }
               catch (Exception ex) {
                await _uom.RollBackAsync();
                return new ResponseMessage<ReturnParam>("Add Error", HttpStatusCode.BadRequest, param);
            }

        }
    }
}
