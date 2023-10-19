using UserService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using UserService.Service.Interface;
using UserService.Service.User;
using AutoMapper;
using UserService.Service.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using UserService.Service.SystemParameter;
using UserService.Service.TimeFrame;
using UserService.Service.Return;

namespace UserService.Infrastructure
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IUserService> _lazyUserService;

        private readonly Lazy<IRoleService> _lazyRoleService;
        private readonly Lazy<IMenuService> _lazyMenuService;
        private readonly Lazy<IStoreService> _lazyStoreService;
        private readonly Lazy<IReturnService> _lazyReturnService;
        private readonly Lazy<IDepartmentService> _lazyDepartmentService;
        private readonly Lazy<ISystemParameterService> _lazySystemParameterService;
        private readonly Lazy<ITimeFrameService> _lazyTimeFrameService;

        public ServiceManager(IUnitOfWork uom, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper, ICurrentUserService currentUser, IHttpContextAccessor httpContextAccessor, IHostEnvironment hostEnvironment)
        {
            _lazyUserService = new Lazy<IUserService>(() => new UserService.Service.User.UserService(uom, jwtTokenGenerator, mapper, currentUser, httpContextAccessor));
            _lazyRoleService = new Lazy<IRoleService>(() => new RoleService(uom, mapper, currentUser, httpContextAccessor));
            _lazyMenuService = new Lazy<IMenuService>(() => new MenuService(uom));
            _lazyStoreService = new Lazy<IStoreService>(() => new StoreService(uom, jwtTokenGenerator, mapper, currentUser, httpContextAccessor, hostEnvironment));
            _lazyDepartmentService = new Lazy<IDepartmentService>(() => new DepartmentService(uom));
            _lazySystemParameterService = new Lazy<ISystemParameterService>(() => new SystemParameterService(uom, mapper, currentUser, httpContextAccessor));
            _lazyTimeFrameService = new Lazy<ITimeFrameService>(() => new TimeFrameService(uom, mapper, currentUser, httpContextAccessor));
            _lazyReturnService = new Lazy<IReturnService>(() => new ReturnService(uom, jwtTokenGenerator, mapper, currentUser, httpContextAccessor, hostEnvironment));
        }

        public IUserService UserService => _lazyUserService.Value;
        public IRoleService RoleService => _lazyRoleService.Value;
        public IMenuService MenuService => _lazyMenuService.Value;
        public IStoreService StoreService => _lazyStoreService.Value;
        public IDepartmentService DepartmentService => _lazyDepartmentService.Value;
        public ISystemParameterService SystemParameterService => _lazySystemParameterService.Value;
        public ITimeFrameService TimeFrameService => _lazyTimeFrameService.Value;

        public IReturnService ReturnService => _lazyReturnService.Value;
    }
}
