using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain;
using UserService.Service.Interface;
using UserService.Service.Service;

namespace UserService.Service
{
    public class RoleService : BaseService, IRoleService
    {
        private readonly IMapper _mapper;
        private readonly HttpRequest request;

        public RoleService(IUnitOfWork uom, IMapper mapper, ICurrentUserService currentUser, IHttpContextAccessor httpContextAccessor) : base(uom, currentUser)
        {
            _mapper = mapper;
            request = httpContextAccessor.HttpContext.Request;
        }

        public async Task<ResponseMessage<List<SelectResponseDTO>>> DropDownRole(string q)
        {
            try
            {
                return new ResponseMessage<List<SelectResponseDTO>>("", System.Net.HttpStatusCode.OK, await _uom.Role.DropDownRole(q));
            }
            catch
            {
                return new ResponseMessage<List<SelectResponseDTO>>("", System.Net.HttpStatusCode.BadRequest, new List<SelectResponseDTO>());
            }
        }
        public async Task<ResponseMessage<RoleDTO>> AddRole(RoleDTO roleDto)
        {
            try
            {
                await _uom.BeginTransactionAsync();
                var role = _mapper.Map<RoleDTO, Role>(roleDto);
                role.CreatedAt = DateTime.Now;
                role.CreatedBy = _currentUser.User.Id;
                role.Status = 1;
                _uom.Role.Create(role);
                foreach (var item in roleDto.functions)
                {
                    role.RoleDetails.Add(new RoleDetail
                    {
                        RoleId = role.Id,
                        Menu = item.Menu,
                        SubMenu = item.SubMenu,
                        Controller = item.Controller,
                        Action = item.Action,
                        CreatedAt = DateTime.Now,
                        CreatedBy = _currentUser.User.Id,
                        Status = 1,
                    });
                }
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                await Log(Domain.Enum.Menu.ROLE.ToString(), Domain.Enum.Action.ADD.ToString(), "Add success", Domain.Enum.LogStatus.SUCCESS.ToString());
                return new ResponseMessage<RoleDTO>("Add Successfully", HttpStatusCode.OK, new RoleDTO());
            }
            catch
            {
                await _uom.RollBackAsync();
                await Log(Domain.Enum.Menu.ROLE.ToString(), Domain.Enum.Action.ADD.ToString(), "Add false", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<RoleDTO>("ServerError", HttpStatusCode.BadRequest, new RoleDTO());
            }
        }

        public async Task<ResponseMessage<RoleDTO>> EditRole(RoleDTO roleDto)
        {
            try
            {
                await _uom.BeginTransactionAsync();
                var role = await _uom.Role.FirstOrDefault(p => p.Id == roleDto.Id);
                role = _mapper.Map(roleDto, role);
                role.ModifiedAt = DateTime.Now;
                role.ModifiedBy = _currentUser.User.Id;
                role.Status = 1;
                var roleDetail = await _uom.RoleDetail.GetRoleDetaisById(roleDto.Id);
                foreach (var item in roleDto.functions)
                {
                    var checkItem = roleDetail.FirstOrDefault(p => p.Menu == item.Menu && p.SubMenu == item.SubMenu && p.Controller == item.Controller && p.Action == item.Action);
                    if (checkItem == null)
                    {
                        _uom.RoleDetail.Create(new RoleDetail
                        {
                            RoleId = role.Id,
                            Menu = item.Menu,
                            SubMenu = item.SubMenu,
                            Controller = item.Controller,
                            Action = item.Action,
                            CreatedAt = DateTime.Now,
                            CreatedBy = _currentUser.User.Id,
                            Status = 1,
                        });
                    }
                    else
                    {
                        checkItem.Status = item.Status;
                        checkItem.ModifiedAt = DateTime.Now;
                        checkItem.ModifiedBy = _currentUser.User.Id;
                        _uom.RoleDetail.Update(checkItem);
                    }

                }
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                await Log(Domain.Enum.Menu.ROLE.ToString(), Domain.Enum.Action.EDIT.ToString(), "Edit success", Domain.Enum.LogStatus.SUCCESS.ToString());
                return new ResponseMessage<RoleDTO>("Edit Successfully", HttpStatusCode.OK, new RoleDTO());
            }
            catch
            {
                await _uom.RollBackAsync();
                await Log(Domain.Enum.Menu.ROLE.ToString(), Domain.Enum.Action.EDIT.ToString(), "Edit false", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<RoleDTO>("ServerError", HttpStatusCode.BadRequest, new RoleDTO());
            }
        }

        public async Task<ResponseMessage<RoleDTO>> DeleteRole(int Id)
        {
            Role role = await _uom.Role.FirstOrDefault(p => p.Id == Id);
            role.DeletedAt = DateTime.Now;
            role.DeteleBy = _currentUser.User.Id;
            role.Status = (int)Domain.Enum.Status.Delete;
            try
            {
                await _uom.BeginTransactionAsync();
                _uom.Role.Update(role);
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                await Log(Domain.Enum.Menu.ROLE.ToString(), Domain.Enum.Action.DELETED.ToString(), "Delete success", Domain.Enum.LogStatus.SUCCESS.ToString());
                return new ResponseMessage<RoleDTO>("Delete Success", HttpStatusCode.OK, new RoleDTO());
            }
            catch
            {
                await _uom.RollBackAsync();
                await Log(Domain.Enum.Menu.ROLE.ToString(), Domain.Enum.Action.DELETED.ToString(), "Delete false", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<RoleDTO>("ServerError", HttpStatusCode.BadRequest, new RoleDTO());
            }
        }

        public async Task<ResponseMessage<RoleDTO>> ChangeActiveRole(int Id)
        {
            Role role = await _uom.Role.FirstOrDefault(p => p.Id == Id);
            role.ModifiedAt = DateTime.Now;
            role.ModifiedBy = _currentUser.User.Id;
            if (role.Status == (int)Domain.Enum.Status.Inactive)
                role.Status = (int)Domain.Enum.Status.Active;
            else
                role.Status = (int)Domain.Enum.Status.Inactive;
            try
            {
                await _uom.BeginTransactionAsync();
                _uom.Role.Update(role);
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                await Log(Domain.Enum.Menu.ROLE.ToString(), Domain.Enum.Action.CHANGEACTIVE.ToString(), "Change active success", Domain.Enum.LogStatus.SUCCESS.ToString());
                return new ResponseMessage<RoleDTO>("Change active Success", HttpStatusCode.OK, new RoleDTO());
            }
            catch
            {
                await _uom.RollBackAsync();
                await Log(Domain.Enum.Menu.ROLE.ToString(), Domain.Enum.Action.CHANGEACTIVE.ToString(), "Change active false", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<RoleDTO>("ServerError", HttpStatusCode.BadRequest, new RoleDTO());
            }
        }

        public async Task<ResponseMessage<DataTableResponse>> TableRole(DataTableParams datatableParams)
        {
            try
            {
                var qRole = request.Query["Role"].ToString() ?? "";
                var qStatus = int.TryParse(request.Query["Status"].ToString() ?? "", out int v) ? v : -1;

                Expression<Func<Role, bool>> query = (q => q.Status != (int)Domain.Enum.Status.Delete
                && (string.IsNullOrEmpty(qRole) || (q.Name ?? "").ToLower().Contains(qRole.ToLower()))
                && (qStatus == -1 || q.Status == qStatus));

                var model = (from a in _uom.Role.FindByCondition(query)
                             join b in _uom.User.FindAll() on a.ModifiedBy equals b.Id into g
                             from f in g.DefaultIfEmpty()
                             select new
                             {
                                 role = a,
                                 user = f.FullName
                             });

                var total = await Task.FromResult(model.Count());
                if (datatableParams.sSortDir_0 == "asc")
                    model = model.OrderBy(p => p.role.CreatedAt).Skip(datatableParams.iDisplayStart).Take(datatableParams.iDisplayLength);
                else
                    model = model.OrderByDescending(p => p.role.CreatedAt).Skip(datatableParams.iDisplayStart).Take(datatableParams.iDisplayLength);

                var result = from c in await Task.FromResult(model.ToArray())
                             select new object[] {
                                 string.Empty,
                                 c.role.Name ?? string.Empty,
                                 c.role.Description ?? string.Empty,
                                 c.user ?? string.Empty,
                                  Utils.CommandButton(new List<string>(){ CheckPermissions("Role","Detail").Result ? "Detail" : "", CheckPermissions("Role", "Delete").Result && c.role.Id != 1 ? "Delete" : "NoDelete", CheckPermissions("Role", "Edit").Result ? "Edit" : "", CheckPermissions("Role", "ChangeActive").Result && c.role.Id != 1 ? "ChangeActive" : "NoChangeActive"},c.role.Id,c.role.Status)
                        };

                return new ResponseMessage<DataTableResponse>("", HttpStatusCode.OK, new DataTableResponse()
                {
                    iTotalDisplayRecords = total,
                    iTotalRecords = total,
                    sEcho = datatableParams.sEcho,
                    aaData = result
                });
            }
            catch
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

        public async Task<ResponseMessage<RoleDTO>> GetFunction(int roleId)
        {
            try
            {
                var function = await Task.FromResult(_uom.Function.FindAll().OrderBy(p => p.Order).ToList());
                var role = await _uom.Role.FirstOrDefault(p => p.Id == roleId);
                var roledetail = await _uom.RoleDetail.GetRoleDetaisById(roleId);
                StringBuilder table = new StringBuilder();
                foreach (var item in function.GroupBy(p => new { p.Menu, p.Submenu }))
                {
                    table.Append($@"<tr>");
                    table.Append($@"<td>{item.Key.Menu}</td>");
                    table.Append($@"<td>{item.Key.Submenu}</td>");
                    table.Append($@"<td class='function-tr'>");
                    foreach (var detail in item)
                    {
                        var check = roledetail.FirstOrDefault(p => p.Menu == item.Key.Menu && p.SubMenu == item.Key.Submenu && p.Controller == detail.Controller && p.Action == detail.Action);
                        table.Append($@"<div class=""function-div""><input type=""checkbox"" id=""{(detail.Controller + "_" + detail.Action + "_" + detail.Submenu?.Replace(" ", ""))}"" 
                                                                class=""filled-in roledetail {(check != null ? "old_role" : "new_role")}""
                                                                data-menu=""{detail.Menu}""
                                                                data-submenu=""{detail.Submenu}""
                                                                data-controller=""{detail.Controller}""
                                                                data-action=""{detail.Action}""
                                                                {((check?.Status ?? 0) == 1 ? "checked" : "")}
                                                                /><label class='function-name' for={(detail.Controller + "_" + detail.Action + "_" + detail.Submenu?.Replace(" ", ""))}>{detail.Funtion}</label></div>");
                    }
                    table.Append($@"</td>");
                    table.Append($@"</tr>");
                }

                return new ResponseMessage<RoleDTO>("", System.Net.HttpStatusCode.OK, new RoleDTO
                {
                    Name = role?.Name ?? "",
                    Description = role?.Description ?? "",
                    FunctionStr = table.ToString()
                });
            }
            catch
            {
                return new ResponseMessage<RoleDTO>("", System.Net.HttpStatusCode.BadRequest, new RoleDTO());
            }
        }
    }
}
