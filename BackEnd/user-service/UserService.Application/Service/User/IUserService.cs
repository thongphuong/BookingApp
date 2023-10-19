using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain;

namespace UserService.Service
{
    public interface IUserService
    {
        Task<ResponseMessage<UserLoginResponse>> Authentication(UserLogin user);
        Task<ResponseMessage<UserDTO>> GetUser(Guid? referenceId);
        Task<ResponseMessage<UserDTO>> AddUser(UserDTO userDto);
        Task<ResponseMessage<UserDTO>> EditUser(UserDTO userDto);
        Task<ResponseMessage<UserDTO>> Delete(Guid referenceID);
        Task<ResponseMessage<UserDTO>> ChangeActive(Guid referenceID);
        Task<ResponseMessage<DataTableResponse>> TableUser(DataTableParams datatableParams);
    }
}
