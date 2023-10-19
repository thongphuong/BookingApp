using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net;
using UserService.Domain;
using UserService.Service.DTO;
using UserService.Service.Interface;
using UserService.Service.Service;

namespace UserService.Service.User
{
    public class UserService : BaseService, IUserService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;
        private readonly HttpRequest request;
        public UserService(IUnitOfWork uom, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper, ICurrentUserService currentUser, IHttpContextAccessor httpContextAccessor) : base(uom, currentUser)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
            request = httpContextAccessor.HttpContext.Request;
        }

        public async Task<ResponseMessage<UserLoginResponse>> Authentication(UserLogin userLogin)
        {
            try
            {
                userLogin.Password = Utils.EncryptPassword(userLogin.Password);
                var user = await _uom.User.LoginUser(userLogin);
                if (user.Id == 0)
                {
                    return new ResponseMessage<UserLoginResponse>("USER_NOT_CORRECT", HttpStatusCode.NotFound, new UserLoginResponse());

                }
                var first_login = user.LastOnline.HasValue ? false : true;
                user.LastOnline = DateTime.Now;
                _uom.User.Update(user);
                await _uom.SaveAsync();
                var jwtToken = _jwtTokenGenerator.GenerateJwtToken(user);
                var roleDetail = await _uom.RoleDetail.GetRoleDetaisById(user.RoleId ?? 0);
                List<RoleDetailDTO> roleDetailDTOs = _mapper.Map<List<RoleDetail>, List<RoleDetailDTO>>(roleDetail.Where(p => p.Status == (int)Domain.Enum.Status.Active).ToList());
                await _uom.Redis.RemoveAsync("ROLE_" + user.RoleId);
                await _uom.Redis.SetNotExpiredAsync("ROLE_" + user.RoleId, roleDetailDTOs);

                return new ResponseMessage<UserLoginResponse>("", HttpStatusCode.OK, new UserLoginResponse()
                {
                    Role = user.RoleId,
                    reference_id = user.ReferenceId,
                    UserName = user.UserName ?? "",
                    Id = user.Id,
                    Token = jwtToken,
                    FullName = user.FullName ?? "",
                    RoleName = user.role.Name ?? "",
                    Devision = user.Division ?? "",
                    FirstLogin = first_login,
                    roleDetailDTOs = roleDetailDTOs
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ResponseMessage<UserLoginResponse>(ex.ToString(), HttpStatusCode.BadRequest, new UserLoginResponse());
            }
        }

        public async Task<ResponseMessage<UserDTO>> GetUser(Guid? referenceId)
        {
            try
            {
                var user = await _uom.User.FirstOrDefault(p => p.ReferenceId == referenceId);
                var department = await _uom.Department.FirstOrDefault(p => p.ReferenceId.ToString() == user.Department);
                var division = await _uom.Division.FirstOrDefault(p => p.ReferenceId.ToString() == user.Division);
                var store = await _uom.Store.FirstOrDefault(p => p.reference_id == user.StoreManagement);
                var working = await _uom.Store.FirstOrDefault(p => p.reference_id == user.WorkingLocation);
                var role = await _uom.Role.FirstOrDefault(p => p.Id == user.RoleId);
                UserDTO userDto = new UserDTO()
                {
                    Id = user.Id,
                    StaffId = user.StaffId,
                    ReferenceId = user.ReferenceId,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    StoreManagement = user.StoreManagement,
                    StoreManagementName = store?.name ?? "",
                    Email = user.Email,
                    Phone = user.Phone,
                    Department = user.Department,
                    DepartmentName = department?.Name ?? "",
                    Division = user.Division,
                    DivisionName = division?.Name ?? "",
                    WorkingLocation = user.WorkingLocation,
                    WorkingLocationName = working?.name ?? "",
                    RoleId = user.RoleId,
                    RoleName = role?.Name ?? ""
                };
                if (user is null)
                    return new ResponseMessage<UserDTO>("USER_NOT_FOUND", HttpStatusCode.NotFound, new UserDTO());
                return new ResponseMessage<UserDTO>("", HttpStatusCode.OK, userDto);
            }
            catch (Exception ex)
            {
                return new ResponseMessage<UserDTO>(ex.ToString(), HttpStatusCode.BadRequest, new UserDTO());
            }

        }

        public async Task<ResponseMessage<UserDTO>> AddUser(UserDTO userDto)
        {
            var isExist = await _uom.User.IsExist(p => p.Status != (int)Domain.Enum.Status.Delete && (p.StaffId == userDto.StaffId || p.UserName == userDto.UserName));
            if (isExist)
            {
                await Log(Domain.Enum.Menu.USER.ToString(), Domain.Enum.Action.ADD.ToString(), "User is exist", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<UserDTO>("USER_IS_EXIST", HttpStatusCode.Conflict, new UserDTO());
            }
            Domain.User user = _mapper.Map<UserDTO, Domain.User>(userDto);
            user.ReferenceId = Guid.NewGuid();
            user.Password = Utils.EncryptPassword(userDto.Password ?? "");
            user.CreatedAt = DateTime.Now;
            user.StoreManagement = user.WorkingLocation;
            user.Status = (int)Domain.Enum.Status.Active;
            try
            {
                await _uom.BeginTransactionAsync();
                _uom.User.Create(user);
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                await Log(Domain.Enum.Menu.USER.ToString(), Domain.Enum.Action.ADD.ToString(), "Add success", Domain.Enum.LogStatus.SUCCESS.ToString());
                return new ResponseMessage<UserDTO>("Add Success", HttpStatusCode.OK, userDto);
            }
            catch (Exception ex)
            {
                await _uom.RollBackAsync();
                await Log(Domain.Enum.Menu.USER.ToString(), Domain.Enum.Action.ADD.ToString(), "Add false", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<UserDTO>(ex.ToString(), HttpStatusCode.BadRequest, new UserDTO());
            }
        }

        public async Task<ResponseMessage<UserDTO>> EditUser(UserDTO userDto)
        {
            var user = await _uom.User.FirstOrDefault(p => p.ReferenceId == userDto.ReferenceId);
            var current_password = user.Password;
            var isExist = await _uom.User.IsExist(p => p.Id != user.Id && p.Status != (int)Domain.Enum.Status.Delete && (p.StaffId == userDto.StaffId || p.UserName == userDto.UserName));
            if (isExist)
            {
                await Log(Domain.Enum.Menu.USER.ToString(), Domain.Enum.Action.EDIT.ToString(), "User is exist", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<UserDTO>("USER_IS_EXIST", HttpStatusCode.Conflict, new UserDTO());
            }
            user = _mapper.Map(userDto, user);
            if (!string.IsNullOrEmpty(userDto.Password))
                user.Password = Utils.EncryptPassword(userDto.Password ?? "");
            else
                user.Password = current_password;
            user.StoreManagement = user.WorkingLocation;
            user.ModifiedAt = DateTime.Now;
            user.ModifiedBy = _currentUser.User.Id;
            user.Status = (int)Domain.Enum.Status.Active;
            try
            {
                await _uom.BeginTransactionAsync();
                _uom.User.Update(user);
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                await Log(Domain.Enum.Menu.USER.ToString(), Domain.Enum.Action.EDIT.ToString(), "Edit success", Domain.Enum.LogStatus.SUCCESS.ToString());
                return new ResponseMessage<UserDTO>("Edit Success", HttpStatusCode.OK, userDto);
            }
            catch (Exception ex)
            {
                await _uom.RollBackAsync();
                await Log(Domain.Enum.Menu.USER.ToString(), Domain.Enum.Action.EDIT.ToString(), "Edit false", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<UserDTO>(ex.ToString(), HttpStatusCode.BadRequest, new UserDTO());
            }
        }

        public async Task<ResponseMessage<UserDTO>> Delete(Guid referenceId)
        {
            Domain.User user = await _uom.User.FirstOrDefault(p => p.ReferenceId == referenceId);
            user.DeletedAt = DateTime.Now;
            user.DeteleBy = _currentUser.User.Id;
            user.Status = (int)Domain.Enum.Status.Delete;
            try
            {
                await _uom.BeginTransactionAsync();
                _uom.User.Update(user);
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                await Log(Domain.Enum.Menu.USER.ToString(), Domain.Enum.Action.DELETED.ToString(), "Delete success", Domain.Enum.LogStatus.SUCCESS.ToString());

                return new ResponseMessage<UserDTO>("Delete Success", HttpStatusCode.OK, new UserDTO());
            }
            catch (Exception ex)
            {
                await _uom.RollBackAsync();
                await Log(Domain.Enum.Menu.USER.ToString(), Domain.Enum.Action.DELETED.ToString(), "Delete false", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<UserDTO>(ex.ToString(), HttpStatusCode.BadRequest, new UserDTO());
            }
        }

        public async Task<ResponseMessage<UserDTO>> ChangeActive(Guid referenceId)
        {
            Domain.User user = await _uom.User.FirstOrDefault(p => p.ReferenceId == referenceId);
            user.ModifiedAt = DateTime.Now;
            user.ModifiedBy = _currentUser.User.Id;
            if (user.Status == (int)Domain.Enum.Status.Inactive)
                user.Status = (int)Domain.Enum.Status.Active;
            else
                user.Status = (int)Domain.Enum.Status.Inactive;
            try
            {
                await _uom.BeginTransactionAsync();
                _uom.User.Update(user);
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                await Log(Domain.Enum.Menu.USER.ToString(), Domain.Enum.Action.CHANGEACTIVE.ToString(), "Change active success", Domain.Enum.LogStatus.SUCCESS.ToString());
                return new ResponseMessage<UserDTO>("Change active Success", HttpStatusCode.OK, new UserDTO());
            }
            catch (Exception ex)
            {
                await _uom.RollBackAsync();
                await Log(Domain.Enum.Menu.USER.ToString(), Domain.Enum.Action.CHANGEACTIVE.ToString(), "Change active false", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<UserDTO>(ex.ToString(), HttpStatusCode.BadRequest, new UserDTO());
            }
        }

        public async Task<ResponseMessage<DataTableResponse>> TableUser(DataTableParams datatableParams)
        {
            try
            {
                var qUserName = request.Query["UserName"].ToString() ?? "";
                var qFullName = request.Query["FullName"].ToString() ?? "";
                var qStatus = int.TryParse(request.Query["Status"].ToString() ?? "", out int v) ? v : -1;
                Expression<Func<Domain.User, bool>> query = (q => q.Status != (int)Domain.Enum.Status.Delete
                && (string.IsNullOrEmpty(qUserName) || qUserName.ToLower().Contains((q.UserName ?? "").ToLower()))
                && (string.IsNullOrEmpty(qFullName) || qFullName.ToLower().Contains((q.FullName ?? "").ToLower()))
                && (qStatus == -1 || qStatus == q.Status));

                Expression<Func<Domain.User, object>> sort = (s => (object)s.CreatedAt);
                Expression<Func<Domain.User, object>> include = (s => s.role);
                (var model, int total) = await _uom.User.PagingData(query, sort, include, "asc", datatableParams.iDisplayStart, datatableParams.iDisplayLength);

                var result = from c in model.ToArray()
                             select new object[] {
                                 string.Empty,
                                 c.FullName ?? string.Empty,
                                 c.StaffId ?? string.Empty,
                                 c?.UserName ?? string.Empty,
                                 c.Phone ?? string.Empty,
                                 c.Email ?? string.Empty,
                                 c.role.Name ?? string.Empty,
                                 Utils.GetStaus(c.Status),
                                 c.FullName == "Admin" ? "" :Utils.CommandButton(new List<string>(){ CheckPermissions("User","Detail").Result ? "Detail" : "", CheckPermissions("User", "Delete").Result ? "Delete" : "", CheckPermissions("User", "Edit").Result ? "Edit" : "", CheckPermissions("User","ChangeActive").Result ? "ChangeActive" : ""},c.ReferenceId ?? Guid.Empty,c.Status)
                        };

                return new ResponseMessage<DataTableResponse>("", HttpStatusCode.OK, new DataTableResponse()
                {
                    iTotalDisplayRecords = total,
                    iTotalRecords = total,
                    sEcho = datatableParams.sEcho,
                    aaData = result
                });
            }
            catch (Exception ex)
            {
                return new ResponseMessage<DataTableResponse>(ex.ToString(), HttpStatusCode.BadRequest, new DataTableResponse()
                {
                    iTotalDisplayRecords = 0,
                    iTotalRecords = 0,
                    sEcho = datatableParams.sEcho,
                    aaData = new List<object>()
                });
            }
        }
    }
}
