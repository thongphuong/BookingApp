using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq.Expressions;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using UserService.Domain;
using UserService.Service.DTO;
using UserService.Service.Interface;
using UserService.Service.Service;

namespace UserService.Service.TimeFrame
{
    public class TimeFrameService : BaseService, ITimeFrameService
    {
        private readonly IMapper _mapper;
        private readonly HttpRequest request;
        public TimeFrameService(IUnitOfWork uom, IMapper mapper, ICurrentUserService currentUser, IHttpContextAccessor httpContextAccessor) : base(uom, currentUser)
        {
            _mapper = mapper;
            request = httpContextAccessor.HttpContext.Request;
        }


        public async Task<ResponseMessage<TimeFrameDTO>> GetTimeFrame(Guid? referenceId)
        {
            try
            {
                var timeFrame = await _uom.TimeFrame.FirstOrDefault(p => p.ReferenceId == referenceId);
                if (timeFrame is null)
                    return new ResponseMessage<TimeFrameDTO>("TIMEFRAME_IS_EXIST", HttpStatusCode.NotFound, new TimeFrameDTO());
                var store = await _uom.Store.FirstOrDefault(p => p.reference_id == timeFrame.StoreReference);
                TimeFrameDTO timeFrameDto = new TimeFrameDTO()
                {
                    Id = timeFrame.Id,
                    ReferenceId = timeFrame.ReferenceId,
                    StoreReference = timeFrame.StoreReference,
                    StoreName = store?.name ?? "",
                    TimeFrom = timeFrame.TimeFrom,
                    TimeTo = timeFrame.TimeTo,
                    Amount = timeFrame.Amount,
                    Status = timeFrame.Status,
                    CreatedAt = timeFrame.CreatedAt
                };

                return new ResponseMessage<TimeFrameDTO>("", HttpStatusCode.OK, timeFrameDto);
            }
            catch
            {
                return new ResponseMessage<TimeFrameDTO>("Server Error", HttpStatusCode.BadRequest, new TimeFrameDTO());
            }

        }

        public async Task<ResponseMessage<TimeFrameDTO>> AddTimeFrame(TimeFrameDTO timeFrameDto)
        {
            try
            {
                var isExist = await _uom.TimeFrame.IsExist(p => p.StoreReference == timeFrameDto.StoreReference && p.TimeFrom == timeFrameDto.TimeFrom && p.TimeTo == timeFrameDto.TimeTo && p.Status == (int)Domain.Enum.Status.Active);
                if (isExist)
                {
                    await Log(Domain.Enum.Menu.TIMEFRAME.ToString(), Domain.Enum.Action.ADD.ToString(), "Time frame is exist", Domain.Enum.LogStatus.FALSE.ToString());
                    return new ResponseMessage<TimeFrameDTO>("TIMEFRAME_IS_EXIST", HttpStatusCode.Conflict, new TimeFrameDTO());
                }
                var timefrom = System.Convert.ToDateTime(timeFrameDto.TimeFrom);
                var timeto = System.Convert.ToDateTime(timeFrameDto.TimeTo);
                if (timefrom > timeto)
                    return new ResponseMessage<TimeFrameDTO>("ERROR_TIMEFROM_TIMETO", HttpStatusCode.Conflict, new TimeFrameDTO());

                Domain.TimeFrame timeFrame = _mapper.Map<TimeFrameDTO, Domain.TimeFrame>(timeFrameDto);
                timeFrame.ReferenceId = Guid.NewGuid();
                timeFrame.CreatedAt = DateTime.Now;
                timeFrame.Status = (int)Domain.Enum.Status.Active;

                await _uom.BeginTransactionAsync();
                _uom.TimeFrame.Create(timeFrame);
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                await Log(Domain.Enum.Menu.TIMEFRAME.ToString(), Domain.Enum.Action.ADD.ToString(), "Add success", Domain.Enum.LogStatus.SUCCESS.ToString());
                return new ResponseMessage<TimeFrameDTO>("Add Success", HttpStatusCode.OK, timeFrameDto);
            }
            catch
            {
                await _uom.RollBackAsync();
                await Log(Domain.Enum.Menu.TIMEFRAME.ToString(), Domain.Enum.Action.ADD.ToString(), "Add false", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<TimeFrameDTO>("ServerError", HttpStatusCode.BadRequest, new TimeFrameDTO());
            }
        }

        public async Task<ResponseMessage<TimeFrameDTO>> EditTimeFrame(TimeFrameDTO timeFrameDto)
        {
            try
            {
                var isExist = await _uom.TimeFrame.IsExist(p => p.StoreReference == timeFrameDto.StoreReference && p.TimeFrom == timeFrameDto.TimeFrom && p.TimeTo == timeFrameDto.TimeTo && p.Status == (int)Domain.Enum.Status.Active && p.ReferenceId != timeFrameDto.ReferenceId);
                if (isExist)
                {
                    await Log(Domain.Enum.Menu.TIMEFRAME.ToString(), Domain.Enum.Action.EDIT.ToString(), "Time frame is exist", Domain.Enum.LogStatus.FALSE.ToString());
                    return new ResponseMessage<TimeFrameDTO>("TIMEFRAME_IS_EXIST", HttpStatusCode.Conflict, new TimeFrameDTO());
                }

                var timeFrame = await _uom.TimeFrame.FirstOrDefault(p => p.ReferenceId == timeFrameDto.ReferenceId);
                if (timeFrame is null)
                    return new ResponseMessage<TimeFrameDTO>("TIMEFRAME_IS_NOTFOUND", HttpStatusCode.NotFound, new TimeFrameDTO());

                var timefrom = System.Convert.ToDateTime(timeFrameDto.TimeFrom);
                var timeto = System.Convert.ToDateTime(timeFrameDto.TimeTo);
                if (timefrom > timeto)
                    return new ResponseMessage<TimeFrameDTO>("ERROR_TIMEFROM_TIMETO", HttpStatusCode.Conflict, new TimeFrameDTO());

                timeFrame = _mapper.Map(timeFrameDto, timeFrame);
                timeFrame.ModifiedAt = DateTime.Now;
                timeFrame.ModifiedBy = _currentUser.User.Id;
                //timeFrame.Status = (int)Domain.Enum.Status.Active;
                await _uom.BeginTransactionAsync();
                _uom.TimeFrame.Update(timeFrame);
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                await Log(Domain.Enum.Menu.TIMEFRAME.ToString(), Domain.Enum.Action.EDIT.ToString(), "Edit success", Domain.Enum.LogStatus.SUCCESS.ToString());
                return new ResponseMessage<TimeFrameDTO>("Edit Success", HttpStatusCode.OK, timeFrameDto);
            }
            catch
            {
                await _uom.RollBackAsync();
                await Log(Domain.Enum.Menu.TIMEFRAME.ToString(), Domain.Enum.Action.EDIT.ToString(), "Edit false", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<TimeFrameDTO>("ServerError", HttpStatusCode.BadRequest, new TimeFrameDTO());
            }
        }

        public async Task<ResponseMessage<TimeFrameDTO>> Delete(Guid referenceId)
        {
            try
            {
                Domain.TimeFrame timeFrame = await _uom.TimeFrame.FirstOrDefault(p => p.ReferenceId == referenceId);
                if (timeFrame is null)
                {
                    await Log(Domain.Enum.Menu.TIMEFRAME.ToString(), Domain.Enum.Action.DELETED.ToString(), "Time frame not found", Domain.Enum.LogStatus.FALSE.ToString());
                    return new ResponseMessage<TimeFrameDTO>("TIMEFRAME_IS_NOTFOUND", HttpStatusCode.NotFound, new TimeFrameDTO());
                }
                timeFrame.DeletedAt = DateTime.Now;
                timeFrame.DeteleBy = _currentUser.User.Id;
                timeFrame.Status = (int)Domain.Enum.Status.Delete;
                await _uom.BeginTransactionAsync();
                _uom.TimeFrame.Update(timeFrame);
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                await Log(Domain.Enum.Menu.TIMEFRAME.ToString(), Domain.Enum.Action.DELETED.ToString(), "Delete success", Domain.Enum.LogStatus.SUCCESS.ToString());
                return new ResponseMessage<TimeFrameDTO>("Delete Success", HttpStatusCode.OK, new TimeFrameDTO());
            }
            catch
            {
                await _uom.RollBackAsync();
                await Log(Domain.Enum.Menu.TIMEFRAME.ToString(), Domain.Enum.Action.DELETED.ToString(), "Delete false", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<TimeFrameDTO>("ServerError", HttpStatusCode.BadRequest, new TimeFrameDTO());
            }
        }

        public async Task<ResponseMessage<TimeFrameDTO>> ChangeActive(Guid referenceId)
        {

            try
            {
                Domain.TimeFrame timeFrame = await _uom.TimeFrame.FirstOrDefault(p => p.ReferenceId == referenceId);
                if (timeFrame is null)
                {
                    await Log(Domain.Enum.Menu.TIMEFRAME.ToString(), Domain.Enum.Action.CHANGEACTIVE.ToString(), "Time frame not found", Domain.Enum.LogStatus.FALSE.ToString());
                    return new ResponseMessage<TimeFrameDTO>("TIMEFRAME_IS_NOTFOUND", HttpStatusCode.NotFound, new TimeFrameDTO());
                }
                timeFrame.ModifiedAt = DateTime.Now;
                timeFrame.ModifiedBy = _currentUser.User.Id;
                if (timeFrame.Status == (int)Domain.Enum.Status.Inactive)
                    timeFrame.Status = (int)Domain.Enum.Status.Active;
                else
                    timeFrame.Status = (int)Domain.Enum.Status.Inactive;
                await _uom.BeginTransactionAsync();
                _uom.TimeFrame.Update(timeFrame);
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                await Log(Domain.Enum.Menu.TIMEFRAME.ToString(), Domain.Enum.Action.CHANGEACTIVE.ToString(), "Change active success", Domain.Enum.LogStatus.SUCCESS.ToString());
                return new ResponseMessage<TimeFrameDTO>("Change active Success", HttpStatusCode.OK, new TimeFrameDTO());
            }
            catch
            {
                await _uom.RollBackAsync();
                await Log(Domain.Enum.Menu.TIMEFRAME.ToString(), Domain.Enum.Action.CHANGEACTIVE.ToString(), "Change active false", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<TimeFrameDTO>("ServerError", HttpStatusCode.BadRequest, new TimeFrameDTO());
            }
        }

        public async Task<ResponseMessage<DataTableResponse>> TableTimeFrame(DataTableParams datatableParams)
        {
            try
            {
                var qStoreName = string.IsNullOrEmpty(request.Query["StoreRef"]) ? Guid.Empty : Guid.Parse(request.Query["StoreRef"].ToString());
                var qTimeFrom = request.Query["TimeFrom"].ToString() ?? "";
                var qTimeTo = request.Query["TimeTo"].ToString() ?? "";
                var qStatus = int.TryParse(request.Query["Status"].ToString() ?? "", out int v) ? v : -1;

                var timeFrom = qTimeFrom != "" ? DateTime.ParseExact(qTimeFrom, "HH:mm",
                                        CultureInfo.InvariantCulture) : DateTime.MinValue;

                var timeTo = qTimeTo != "" ? DateTime.ParseExact(qTimeTo, "HH:mm",
                                       CultureInfo.InvariantCulture) : DateTime.MinValue;

                Expression<Func<Domain.TimeFrame, bool>> query = q => q.Status != (int)Domain.Enum.Status.Delete
                && (qStoreName == Guid.Empty || q.StoreReference == qStoreName)
                && (qStatus == -1 || qStatus == q.Status)
                && (timeFrom == DateTime.MinValue || timeFrom <= System.Convert.ToDateTime(q.TimeFrom))
                && (timeTo == DateTime.MinValue || timeTo >= System.Convert.ToDateTime(q.TimeTo));

                //Expression<Func<Domain.TimeFrame, object>> sort = (s => (object)s.CreatedAt);

                var data = _uom.TimeFrame.FindAll().ToArray().Where(q => q.Status != (int)Domain.Enum.Status.Delete
              && (qStoreName == Guid.Empty || q.StoreReference == qStoreName)
              && (qStatus == -1 || qStatus == q.Status)
              && (timeFrom == DateTime.MinValue || timeFrom <= System.Convert.ToDateTime(q.TimeFrom))
              && (timeTo == DateTime.MinValue || timeTo >= System.Convert.ToDateTime(q.TimeTo))).OrderBy(s => (object)s.CreatedAt);
                int total = data.Count();
                if (datatableParams.iDisplayLength == 0)
                    datatableParams.iDisplayLength = 10;
                var model = data.Skip(datatableParams.iDisplayStart).Take(datatableParams.iDisplayLength).ToArray();

                var stores = await _uom.Store.GetListStore(a => a.status == (int)Domain.Enum.Status.Active);
                var result = from c in model
                             let store = stores.FirstOrDefault(x => x.reference_id == c.StoreReference)?.name
                             select new object[] {
                                 string.Empty,
                                 store ?? string.Empty,
                                 c.CreatedAt?.ToString("dd/MM/yyyy") ?? string.Empty,
                                 c.TimeFrom + " - " + c.TimeTo ?? string.Empty,
                                 c.Amount + "" ?? string.Empty,
                                 Utils.CommandButton(new List<string>(){ CheckPermissions("TimeFrame", "Detail").Result ? "Detail" : "", CheckPermissions("TimeFrame", "Delete").Result ? "Delete" : "", CheckPermissions("TimeFrame", "Edit").Result ? "Edit" : ""},c.ReferenceId ?? Guid.Empty,c.Status)
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

        public async Task<ResponseMessage<List<SelectResponseDTO>>> DropDownTimeFrame(string query, Guid store)
        {
            try
            {
                return new ResponseMessage<List<SelectResponseDTO>>("", System.Net.HttpStatusCode.OK, await _uom.TimeFrame.SelectTimeFrame(query, store));
            }
            catch
            {
                return new ResponseMessage<List<SelectResponseDTO>>("", System.Net.HttpStatusCode.BadRequest, new List<SelectResponseDTO>());
            }
        }
    }
}
