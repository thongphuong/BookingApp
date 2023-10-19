using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain;
using UserService.Service.DTO;

namespace UserService.Service
{
    public interface IDepartmentService
    {
        public Task<ResponseMessage<List<SelectResponseDTO>>> DropDownDepartment(string query);

        public Task<ResponseMessage<List<SelectResponseDTO>>> DropDownDivision(string query, Guid Department);
    }
}
