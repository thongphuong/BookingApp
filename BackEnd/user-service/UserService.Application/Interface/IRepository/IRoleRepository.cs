using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain;

namespace UserService.Service.Interface
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        public Task<List<SelectResponseDTO>> DropDownRole(string query);
    }
}
