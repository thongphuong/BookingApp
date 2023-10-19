using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Service
{
    public interface IRoleService
    {
        Task<ResponseMessage<List<SelectResponseDTO>>> DropDownRole(string query);
        Task<ResponseMessage<RoleDTO>> AddRole(RoleDTO roleDto);
        Task<ResponseMessage<RoleDTO>> EditRole(RoleDTO roleDto);
        Task<ResponseMessage<RoleDTO>> DeleteRole(int Id);
        Task<ResponseMessage<RoleDTO>> ChangeActiveRole(int Id);
        Task<ResponseMessage<DataTableResponse>> TableRole(DataTableParams datatableParams);

        Task<ResponseMessage<RoleDTO>> GetFunction(int roleId);
    }
}
