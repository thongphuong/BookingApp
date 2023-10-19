using UserService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Service.User;
using UserService.Service.Store;
using UserService.Service.Return;

namespace UserService.Service.Interface
{
    public interface IServiceManager
    {
        IUserService UserService { get; }
        IRoleService RoleService { get; }
        IMenuService MenuService { get; }
        IStoreService StoreService { get; }

        IReturnService ReturnService { get; }
        IDepartmentService DepartmentService { get; }
        ISystemParameterService SystemParameterService { get; }
        ITimeFrameService TimeFrameService { get; }
    }
}
