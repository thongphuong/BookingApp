using UserService.Domain;
using UserService.Service.Interface.IRepository;

namespace UserService.Service.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IMongoRepository<Log> Log { get; }
        IRedisRepository Redis { get; }

        IUserRepository User { get; }
        IRoleRepository Role { get; }
        IRoleDetailRepository RoleDetail { get; }
        IFunctionRepository Function { get; }
        IMenuAttrRepository MenuAttr { get; }
        IMenuRepository Menu { get; }
        IStoreRepository Store { get; }
        IDeparmentRepository Department { get; }
        IDivisionRepository Division { get; }
        ITimeFrameRepository TimeFrame { get; }

        IReturnRepository Return { get; }
        IReturnNewRepository ReturnNew { get; }
        IReturnDetailRepository ReturnDetail { get; }
        ISystemParameterRepository SystemParameter { get; }
        Task SaveAsync(CancellationToken token = default);
        Task BeginTransactionAsync(CancellationToken token = default);
        Task RollBackAsync(CancellationToken token = default);
        Task CommitAsync(CancellationToken token = default);
    }
}
