using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain;
using UserService.Service.DTO;
using UserService.Service.Interface;

namespace UserService.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _uom;

        public DepartmentService(IUnitOfWork uom)
        {
            _uom = uom;
        }
        public async Task<ResponseMessage<List<SelectResponseDTO>>> DropDownDepartment(string query)
        {
            try
            {
                return new ResponseMessage<List<SelectResponseDTO>>("", System.Net.HttpStatusCode.OK, await _uom.Department.SelectDepartment(query));
            }
            catch
            {
                return new ResponseMessage<List<SelectResponseDTO>>("", System.Net.HttpStatusCode.BadRequest, new List<SelectResponseDTO>());
            }

        }

        public async Task<ResponseMessage<List<SelectResponseDTO>>> DropDownDivision(string query, Guid department)
        {
            try
            {
                return new ResponseMessage<List<SelectResponseDTO>>("", System.Net.HttpStatusCode.OK, await _uom.Division.SelectDivision(query, department));
            }
            catch
            {
                return new ResponseMessage<List<SelectResponseDTO>>("", System.Net.HttpStatusCode.BadRequest, new List<SelectResponseDTO>());
            }
        }
    }
}
