using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain;
using UserService.Service.DTO;

namespace UserService.Service.Interface
{
    public interface IDeparmentRepository : IGenericRepository<Department>
    {
        public Task<List<SelectResponseDTO>> SelectDepartment(string query);
    }
}
