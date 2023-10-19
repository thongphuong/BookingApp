using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain;

namespace UserService.Service
{
    public interface ISystemParameterService
    {
        Task<ResponseMessage<SystemParameterDTO>> GetSystemParameter(Guid? referenceId);
        Task<ResponseMessage<SystemParameterDTO>> AddSystemParameter(SystemParameterDTO systemParameterDto);
        Task<ResponseMessage<SystemParameterDTO>> EditSystemParameter(SystemParameterDTO systemParameterDto);
        Task<ResponseMessage<SystemParameterDTO>> Delete(Guid referenceID);
        Task<ResponseMessage<SystemParameterDTO>> ChangeActive(Guid referenceID);
        Task<ResponseMessage<DataTableResponse>> TableSystemParameter(DataTableParams datatableParams);
        Task<ResponseMessage<List<SystemDictionaryDTO>>> GetRedisSystemParameter();
    }
}
