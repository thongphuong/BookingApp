using BookingService.Domain;

namespace BookingService.Service.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IMongoRepository<Domain.Log> Log { get; }
        IRedisRepository Redis { get; }
        IOrderRepository Order { get; }
        IOrderDeliveryRepository OrderDelivery { get; }
        IOrderReceiptRepository OrderReceipt { get; }
        IOrderProductRepository OrderProduct { get; }
        ISupplierRepository Supplier { get; }
        IHistoryRepository History { get; }
        ILineRepository Line { get; }
        ILineDepartmentRepository LineDepartment { get; }
        ISystemParameterRepository SystemParameter { get; }
        ITimeFrameRepository TimeFrame { get; }
        IStoreRepository Store { get; }

        IReturnRepository Return { get; }
        IReturnNewRepository ReturnNew { get; }
        IReturnDetailRepository ReturnDetail { get; }
        IProductRepository Product { get; }
        Task SaveAsync(CancellationToken token = default);
        Task BeginTransactionAsync(CancellationToken token = default);
        Task RollBackAsync(CancellationToken token = default);
        Task CommitAsync(CancellationToken token = default);
    }
}
