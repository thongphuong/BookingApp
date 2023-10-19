using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Service.DTO;

namespace UserService.Service
{
    public interface IMenuService
    {
        public Task<ResponseMessage<List<MenuDTO>>> GetMenu(int roleId);

    }
}
