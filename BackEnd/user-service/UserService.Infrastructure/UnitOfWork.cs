using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using UserService.Domain;
using UserService.Infrastructure.Repository;
using UserService.Service.Interface;
using UserService.Service.Interface.IRepository;

namespace UserService.Infrastructure
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
        private IUserRepository _user;
        private IRoleRepository _role;
        private IRoleDetailRepository _roleDetail;
        private IFunctionRepository _function;
        private IMenuAttrRepository _menuAttr;
        private IMenuRepository _menu;
        private IStoreRepository _store;
        private IDeparmentRepository _department;
        private IDivisionRepository _division;
        private ISystemParameterRepository _systemParameter;
        private ITimeFrameRepository _timeFrame;
        private IReturnRepository _return;
        private IReturnDetailRepository _returnDetail;
        private IReturnNewRepository _returnNew;

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
        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_context);
                }
                return _user;
            }
        }
        public IRoleRepository Role
        {
            get
            {
                if (_role == null)
                {
                    _role = new RoleRepository(_context);
                }
                return _role;
            }
        }
        public IRoleDetailRepository RoleDetail
        {
            get
            {
                if (_roleDetail == null)
                {
                    _roleDetail = new RoleDetailRepository(_context);
                }
                return _roleDetail;
            }
        }
        public IFunctionRepository Function
        {
            get
            {
                if (_function == null)
                {
                    _function = new FunctionRepository(_context);
                }
                return _function;
            }
        }
        public IMenuAttrRepository MenuAttr
        {
            get
            {
                if (_menuAttr == null)
                {
                    _menuAttr = new MenuAttrRepository(_context);
                }
                return _menuAttr;
            }
        }
        public IMenuRepository Menu
        {
            get
            {
                if (_menu == null)
                {
                    _menu = new MenuRepository(_context);
                }
                return _menu;
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
        public IDeparmentRepository Department
        {
            get
            {
                if (_department == null)
                {
                    _department = new DepartmentRepository(_context);
                }
                return _department;
            }
        }
        public IDivisionRepository Division
        {
            get
            {
                if (_division == null)
                {
                    _division = new DivisionRepository(_context);
                }
                return _division;
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
        #endregion
    }
}
