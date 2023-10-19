using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using BookingService.Infrastructure.Repository;
using BookingService.Service.Interface;
using BookingService.Domain;


namespace BookingService.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private BookingDbContext _context;
        private MongoDbContext _mongoContext;
        private StackExchange.Redis.IDatabase _cacheContext;
        private bool _disposed;
        private IDbContextTransaction trans;


        public async Task BeginTransactionAsync(CancellationToken token = default)
        {
            trans = await _context.Database.BeginTransactionAsync(token);
        }
        public async Task CommitAsync(CancellationToken token = default)
        {
            await trans.CommitAsync(token);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public async Task RollBackAsync(CancellationToken token = default)
        {
            await trans.RollbackAsync(token);
            trans.Dispose();
        }
        public async Task SaveAsync(CancellationToken token = default)
        {
            await _context.SaveChangesAsync(token);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();
            _disposed = true;
        }
        public UnitOfWork(BookingDbContext context, MongoDbContext mongoContext, StackExchange.Redis.IDatabase cache)
        {
            _context = context;
            _mongoContext = mongoContext;
            _cacheContext = cache;
        }

        #region Add repo
        private IMongoRepository<Log> _log;
        private IRedisRepository _cache;
        private ISupplierRepository _supplier;
        private ILineRepository _line;
        private ILineDepartmentRepository _lineDepartment;
        private IOrderRepository _order;
        private IOrderDeliveryRepository _orderDelivery;
        private IOrderReceiptRepository _orderReceipt;
        private IOrderProductRepository _orderProduct;
        private IHistoryRepository _History;
        private ISystemParameterRepository _systemParameter;
        private ITimeFrameRepository _timeFrame;
        private IStoreRepository _store;
        private IReturnRepository _return;
        private IReturnDetailRepository _returnDetail;
        private IReturnNewRepository _returnNew;
        private IProductRepository _product;
        public IRedisRepository Redis
        {
            get
            {
                if (_cache == null)
                {
                    _cache = new RedisRepository(_cacheContext);
                }
                return _cache;
            }
        }
        public IMongoRepository<Log> Log
        {
            get
            {
                if (_log == null)
                {
                    _log = new MongoRepository<Log>(_mongoContext);
                }
                return _log;
            }
        }
        public ISupplierRepository Supplier
        {
            get
            {
                if (_supplier == null)
                {
                    _supplier = new SupplierRepository(_context);
                }
                return _supplier;
            }
        }
        public IHistoryRepository History
        {
            get
            {
                if (_History == null)
                {
                    _History = new HistoryRepository(_context);
                }
                return _History;
            }
        }
        public ILineRepository Line
        {
            get
            {
                if (_line == null)
                {
                    _line = new LineRepository(_context);
                }
                return _line;
            }
        }
        public ILineDepartmentRepository LineDepartment
        {
            get
            {
                if (_lineDepartment == null)
                {
                    _lineDepartment = new LineDepartmentRepository(_context);
                }
                return _lineDepartment;
            }
        }
        public IOrderRepository Order
        {
            get
            {
                if (_order == null)
                {
                    _order = new OrderRepository(_context);
                }
                return _order;
            }

        }
        public IOrderDeliveryRepository OrderDelivery
        {
            get
            {
                if (_orderDelivery == null)
                {
                    _orderDelivery = new OrderDeliveryRepository(_context);
                }
                return _orderDelivery;
            }

        }
        public IOrderReceiptRepository OrderReceipt
        {
            get
            {
                if (_orderReceipt == null)
                {
                    _orderReceipt = new OrderReceiptRepository(_context);
                }
                return _orderReceipt;
            }

        }
        public IOrderProductRepository OrderProduct
        {
            get
            {
                if (_orderProduct == null)
                {
                    _orderProduct = new OrderProductRepository(_context);
                }
                return _orderProduct;
            }
        }
        public ISystemParameterRepository SystemParameter
        {
            get
            {
                if (_systemParameter == null)
                {
                    _systemParameter = new SystemParameterRepository(_context);
                }
                return _systemParameter;
            }

        }
        public ITimeFrameRepository TimeFrame
        {
            get
            {
                if (_timeFrame == null)
                {
                    _timeFrame = new TimeFrameRepository(_context);
                }
                return _timeFrame;
            }

        }
        public IStoreRepository Store
        {
            get
            {
                if (_store == null)
                {
                    _store = new StoreRepository(_context);
                }
                return _store;
            }

        }

        public IReturnRepository Return
        {
            get
            {
                if (_return == null)
                {
                    _return = new ReturnRepository(_context);
                }
                return _return;
            }
        }

        public IReturnDetailRepository ReturnDetail
        {
            get
            {
                if (_returnDetail == null)
                {
                    _returnDetail = new ReturnDetailRepository(_context);
                }
                return _returnDetail;
            }
        }
        public IReturnNewRepository ReturnNew
        {
            get
            {
                if (_returnNew == null)
                {
                    _returnNew = new ReturnNewRepository(_context);
                }
                return _returnNew;
            }
        }

        public IProductRepository Product
        {
            get
            {
                if (_product == null)
                {
                    _product = new ProductRepository(_context);
                }
                return _product;
            }
        }
        #endregion
    }
}
