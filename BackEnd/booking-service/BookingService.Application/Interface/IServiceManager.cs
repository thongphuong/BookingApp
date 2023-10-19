using BookingService.Domain;
using BookingService.Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Service;

namespace BookingService.Service.Interface
{
    public interface IServiceManager
    {

        IOrderService OrderService { get; }
        ISupplierService SupplierService { get; }

        IHistoryService HistoryService { get; }

        ILineService LineService { get; }

        IReturnService ReturnService { get; }
        IProductService ProductService { get; }

        IStoreService StoreService { get; }
    }
}
