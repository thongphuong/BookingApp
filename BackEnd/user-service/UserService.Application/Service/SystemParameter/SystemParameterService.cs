using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using System.Net;
using UserService.Domain;
using UserService.Service.DTO;
using UserService.Service.Interface;
using UserService.Service.Service;

namespace UserService.Service.SystemParameter
{
    public class SystemParameterService : BaseService, ISystemParameterService
    {
        private readonly IMapper _mapper;
        private readonly HttpRequest request;
        public SystemParameterService(IUnitOfWork uom, IMapper mapper, ICurrentUserService currentUser, IHttpContextAccessor httpContextAccessor) : base(uom, currentUser)
        {
            _mapper = mapper;
            request = httpContextAccessor.HttpContext.Request;
        }

        public async Task<ResponseMessage<SystemParameterDTO>> GetSystemParameter(Guid? referenceId)
        {
            try
            {
                var systemParameter = await _uom.SystemParameter.FirstOrDefault(p => p.ReferenceId == referenceId);
                if (systemParameter is null)
                    return new ResponseMessage<SystemParameterDTO>("SYSTEM_PARAMETER_NOT_FOUND", HttpStatusCode.NotFound, new SystemParameterDTO());
                SystemParameterDTO systemParameterDto = new SystemParameterDTO()
                {
                    Id = systemParameter.Id,
                    ReferenceId = systemParameter.ReferenceId,
                    ParameterName = systemParameter.ParameterName,
                    Note = systemParameter.Note ?? string.Empty,
                    Value = systemParameter.Value ?? string.Empty,
                    Status = systemParameter.Status
                };

                return new ResponseMessage<SystemParameterDTO>("", HttpStatusCode.OK, systemParameterDto);
            }
            catch
            {
                return new ResponseMessage<SystemParameterDTO>("Server Error", HttpStatusCode.BadRequest, new SystemParameterDTO());
            }

        }

        public async Task<ResponseMessage<SystemParameterDTO>> AddSystemParameter(SystemParameterDTO systemParameterDto)
        {
            //var isExist = await _uom.User.IsExist(p => p.StaffId == systemParameterDto.StaffId || p.UserName == systemParameterDto.UserName);
            //if (isExist)
            //    return new ResponseMessage<SystemParameterDTO>("USER_IS_EXIST", HttpStatusCode.Conflict, new SystemParameterDTO());
            Domain.SystemParameter systemParameter = _mapper.Map<SystemParameterDTO, Domain.SystemParameter>(systemParameterDto);
            systemParameter.ReferenceId = Guid.NewGuid();
            systemParameter.CreatedAt = DateTime.Now;
            systemParameter.Status = (int)Domain.Enum.Status.Active;
            try
            {
                await _uom.BeginTransactionAsync();
                _uom.SystemParameter.Create(systemParameter);
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                await AddToRedis();
                await Log(Domain.Enum.Menu.SYSTEMPARAMETER.ToString(), Domain.Enum.Action.ADD.ToString(), "Add success", Domain.Enum.LogStatus.SUCCESS.ToString());
                return new ResponseMessage<SystemParameterDTO>("Add Success", HttpStatusCode.OK, systemParameterDto);
            }
            catch (Exception ex)
            {
                await _uom.RollBackAsync();
                await Log(Domain.Enum.Menu.SYSTEMPARAMETER.ToString(), Domain.Enum.Action.ADD.ToString(), "Add false", Domain.Enum.LogStatus.SUCCESS.ToString());
                return new ResponseMessage<SystemParameterDTO>(ex.ToString(), HttpStatusCode.BadRequest, new SystemParameterDTO());
            }
        }

        public async Task<ResponseMessage<SystemParameterDTO>> EditSystemParameter(SystemParameterDTO systemParameterDto)
        {

            try
            {
                var systemParameter = await _uom.SystemParameter.FirstOrDefault(p => p.ReferenceId == systemParameterDto.ReferenceId);

                //var isExist = await _uom.User.IsExist(p => p.Id != user.Id && (p.StaffId == userDto.StaffId || p.UserName == userDto.UserName));
                //if (isExist)
                //    return new ResponseMessage<UserDTO>("USER_IS_EXIST", HttpStatusCode.Conflict, new UserDTO());
                if (systemParameter is null)
                {
                    await Log(Domain.Enum.Menu.SYSTEMPARAMETER.ToString(), Domain.Enum.Action.EDIT.ToString(), "System parameter not found", Domain.Enum.LogStatus.FALSE.ToString());
                    return new ResponseMessage<SystemParameterDTO>("SYSTEM_PARAMETER_NOT_FOUND", HttpStatusCode.NotFound, new SystemParameterDTO());
                }
                systemParameter = _mapper.Map(systemParameterDto, systemParameter);
                systemParameter.ModifiedAt = DateTime.Now;
                systemParameter.ModifiedBy = _currentUser.User.Id;
                //systemParameter.Status = (int)Domain.Enum.Status.Active;
                await _uom.BeginTransactionAsync();
                _uom.SystemParameter.Update(systemParameter);
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                await AddToRedis();
                await Log(Domain.Enum.Menu.SYSTEMPARAMETER.ToString(), Domain.Enum.Action.EDIT.ToString(), "Edit success", Domain.Enum.LogStatus.SUCCESS.ToString());
                return new ResponseMessage<SystemParameterDTO>("Edit Success", HttpStatusCode.OK, systemParameterDto);
            }
            catch (Exception ex)
            {
                await _uom.RollBackAsync();
                await Log(Domain.Enum.Menu.SYSTEMPARAMETER.ToString(), Domain.Enum.Action.EDIT.ToString(), "Edit false", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<SystemParameterDTO>(ex.ToString(), HttpStatusCode.BadRequest, new SystemParameterDTO());
            }
        }

        public async Task<ResponseMessage<SystemParameterDTO>> Delete(Guid referenceId)
        {

            try
            {
                Domain.SystemParameter systemParameter = await _uom.SystemParameter.FirstOrDefault(p => p.ReferenceId == referenceId);
                if (systemParameter is null)
                {
                    await Log(Domain.Enum.Menu.SYSTEMPARAMETER.ToString(), Domain.Enum.Action.DELETED.ToString(), "System parameter not found", Domain.Enum.LogStatus.FALSE.ToString());
                    return new ResponseMessage<SystemParameterDTO>("SYSTEM_PARAMETER_NOT_FOUND", HttpStatusCode.NotFound, new SystemParameterDTO());
                }
                systemParameter.DeletedAt = DateTime.Now;
                systemParameter.DeteleBy = _currentUser.User.Id;
                systemParameter.Status = (int)Domain.Enum.Status.Delete;
                await _uom.BeginTransactionAsync();
                _uom.SystemParameter.Update(systemParameter);
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                await AddToRedis();
                await Log(Domain.Enum.Menu.SYSTEMPARAMETER.ToString(), Domain.Enum.Action.DELETED.ToString(), "Delete success", Domain.Enum.LogStatus.SUCCESS.ToString());
                return new ResponseMessage<SystemParameterDTO>("Delete Success", HttpStatusCode.OK, new SystemParameterDTO());
            }
            catch (Exception ex)
            {
                await _uom.RollBackAsync();
                await Log(Domain.Enum.Menu.SYSTEMPARAMETER.ToString(), Domain.Enum.Action.DELETED.ToString(), "Delete false", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<SystemParameterDTO>(ex.ToString(), HttpStatusCode.BadRequest, new SystemParameterDTO());
            }
        }

        public async Task<ResponseMessage<SystemParameterDTO>> ChangeActive(Guid referenceId)
        {

            try
            {
                Domain.SystemParameter systemParameter = await _uom.SystemParameter.FirstOrDefault(p => p.ReferenceId == referenceId);
                if (systemParameter is null)
                {
                    await Log(Domain.Enum.Menu.SYSTEMPARAMETER.ToString(), Domain.Enum.Action.CHANGEACTIVE.ToString(), "System paremeter not found", Domain.Enum.LogStatus.FALSE.ToString());
                    return new ResponseMessage<SystemParameterDTO>("SYSTEM_PARAMETER_NOT_FOUND", HttpStatusCode.NotFound, new SystemParameterDTO());
                }
                systemParameter.ModifiedAt = DateTime.Now;
                systemParameter.ModifiedBy = _currentUser.User.Id;
                if (systemParameter.Status == (int)Domain.Enum.Status.Inactive)
                    systemParameter.Status = (int)Domain.Enum.Status.Active;
                else
                    systemParameter.Status = (int)Domain.Enum.Status.Inactive;
                await _uom.BeginTransactionAsync();
                _uom.SystemParameter.Update(systemParameter);
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                await Log(Domain.Enum.Menu.SYSTEMPARAMETER.ToString(), Domain.Enum.Action.CHANGEACTIVE.ToString(), "Change active success", Domain.Enum.LogStatus.SUCCESS.ToString());
                return new ResponseMessage<SystemParameterDTO>("Change active Success", HttpStatusCode.OK, new SystemParameterDTO());
            }
            catch
            {
                await _uom.RollBackAsync();
                await Log(Domain.Enum.Menu.SYSTEMPARAMETER.ToString(), Domain.Enum.Action.CHANGEACTIVE.ToString(), "Change active false", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<SystemParameterDTO>("ServerError", HttpStatusCode.BadRequest, new SystemParameterDTO());
            }
        }

        public async Task<ResponseMessage<DataTableResponse>> TableSystemParameter(DataTableParams datatableParams)
        {
            try
            {
                var qParamName = request.Query["ParamName"].ToString() ?? "";
                var qStatus = int.TryParse(request.Query["Status"].ToString() ?? "", out int v) ? v : -1;

                Expression<Func<Domain.SystemParameter, bool>> query = (q => q.Status != (int)Domain.Enum.Status.Delete
                && (string.IsNullOrEmpty(qParamName) || qParamName.ToLower().Contains((q.ParameterName ?? "").ToLower()))
                && (qStatus == -1 || qStatus == q.Status));

                Expression<Func<Domain.SystemParameter, object>> sort = (s => (object)s.CreatedAt);
                (var model, int total) = await _uom.SystemParameter.PagingData(query, sort, null, "asc", datatableParams.iDisplayStart, datatableParams.iDisplayLength);

                var result = from c in model.ToArray()
                             select new object[] {
                                 string.Empty,
                                 c.ParameterName ?? string.Empty,
                                 c.Value ?? string.Empty,
                                 c?.Note ?? string.Empty,
                                 Utils.GetStaus(c.Status),
                                 Utils.CommandButton(new List<string>(){"Detail", "Delete", "Edit","ChangeActive"},c.ReferenceId ?? Guid.Empty,c.Status)
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

        private async Task AddToRedis()
        {
            await _uom.Redis.RemoveAsync("SYSTEMPARAMETER");
            var systemParameter = _uom.SystemParameter.FindByCondition(p => p.Status == (int)Domain.Enum.Status.Active && !string.IsNullOrEmpty(p.Value)).Select(p => new SystemDictionaryDTO
            {
                Key = p.ParameterName,
                Value = p.Value ?? string.Empty,
            }).ToList();
            await _uom.Redis.SetNotExpiredAsync("SYSTEMPARAMETER", systemParameter);
        }

        public async Task<ResponseMessage<List<SystemDictionaryDTO>>> GetRedisSystemParameter()
        {
            try
            {
                if (await _uom.Redis.isExsit("SYSTEMPARAMETER"))
                {
                    return
                        new ResponseMessage<List<SystemDictionaryDTO>>("success", HttpStatusCode.OK, await _uom.Redis.GetAsync<List<SystemDictionaryDTO>>("SYSTEMPARAMETER"));
                }
                else
                {
                    var systemParameter = _uom.SystemParameter.FindByCondition(p => p.Status == (int)Domain.Enum.Status.Active && !string.IsNullOrEmpty(p.Value)).Select(p => new SystemDictionaryDTO
                    {
                        Key = p.ParameterName,
                        Value = p.Value ?? string.Empty,
                    }).ToList();
                    await _uom.Redis.RemoveAsync("SYSTEMPARAMETER");
                    await _uom.Redis.SetNotExpiredAsync("SYSTEMPARAMETER", systemParameter);
                    return new ResponseMessage<List<SystemDictionaryDTO>>("success", HttpStatusCode.OK, systemParameter);
                }
            }
            catch (Exception ex)
            {
                return new ResponseMessage<List<SystemDictionaryDTO>>(ex.ToString(), HttpStatusCode.BadRequest, new List<SystemDictionaryDTO>());
            }
        }
    }
}
