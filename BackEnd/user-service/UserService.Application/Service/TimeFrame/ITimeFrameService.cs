using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain;

namespace UserService.Service
{
    public interface ITimeFrameService
    {
        Task<ResponseMessage<TimeFrameDTO>> GetTimeFrame(Guid? referenceId);
        Task<ResponseMessage<TimeFrameDTO>> AddTimeFrame(TimeFrameDTO timeFrameDto);
        Task<ResponseMessage<TimeFrameDTO>> EditTimeFrame(TimeFrameDTO timeFrameDto);
        Task<ResponseMessage<TimeFrameDTO>> Delete(Guid referenceID);
        Task<ResponseMessage<TimeFrameDTO>> ChangeActive(Guid referenceID);
        Task<ResponseMessage<DataTableResponse>> TableTimeFrame(DataTableParams datatableParams);
        public Task<ResponseMessage<List<SelectResponseDTO>>> DropDownTimeFrame(string query, Guid store);


    }
}
