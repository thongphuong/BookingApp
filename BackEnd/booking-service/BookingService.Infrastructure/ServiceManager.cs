using BookingService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using BookingService.Service.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using BookingService.Service.Service;
using UserService.Service;
using UserService.Service.Store;

namespace BookingService.Infrastructure
{
    public class ServiceManager : IServiceManager
    {

        private readonly Lazy<IOrderService> _lazyOrderService;
        private readonly Lazy<ISupplierService> _lazySupplierService;

        private readonly Lazy<IHistoryService> _lazyHistoryService;
        private readonly Lazy<ILineService> _lazyLineService;

        private readonly Lazy<IReturnService> _lazyReturnService;


        private readonly Lazy<IProductService> _lazyProductService;
        private readonly Lazy<IStoreService> _lazyStoreService;
        public ServiceManager(IUnitOfWork uom, IMapper mapper, ICurrentUserService currentUser, IHttpContextAccessor httpContextAccessor, IHostEnvironment hostEnvironment)
        {
            _lazyOrderService = new Lazy<IOrderService>(() => new OrderService(uom, mapper, currentUser, httpContextAccessor));
            _lazyHistoryService = new Lazy<IHistoryService>(() => new HistoryService(uom, mapper, currentUser));
            _lazySupplierService = new Lazy<ISupplierService>(() => new SupplierService(uom, mapper, currentUser, httpContextAccessor));
            _lazyLineService = new Lazy<ILineService>(() => new LineService(uom, mapper, currentUser));
            _lazyProductService = new Lazy<IProductService>(() => new ProductService(uom, mapper, httpContextAccessor));
            _lazyReturnService = new Lazy<IReturnService>(() => new ReturnService(uom, mapper, currentUser, httpContextAccessor, hostEnvironment));
            _lazyStoreService = new Lazy<IStoreService>(() => new StoreService(uom, mapper));

        }



        public ILineService LineService => _lazyLineService.Value;

        public Service.IOrderService OrderService => _lazyOrderService.Value;

        public Service.ISupplierService SupplierService => _lazySupplierService.Value;

        public IHistoryService HistoryService => _lazyHistoryService.Value;
        public IReturnService ReturnService => _lazyReturnService.Value;

        public IProductService ProductService => _lazyProductService.Value;
        public IStoreService StoreService => _lazyStoreService.Value;
    }
}
