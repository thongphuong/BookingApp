using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain;

namespace UserService.Service.Interface
{
    public interface IStoreRepository : IGenericRepository<UserService.Domain.Store>
    {
        Task<IEnumerable<UserService.Domain.Store>> GetAllAsync(CancellationToken token = default);
        Task<UserService.Domain.Store> GetByIdAsync(Guid ownerId);
        Task<UserService.Domain.Store> CreateAsync(UserService.Domain.Store ownerForCreationDto);
        Task UpdateAsync(Guid ownerId, UserService.Domain.Store ownerForUpdateDto);
        Task DeleteAsync(Guid ownerId);
        IQueryable<Domain.Store> GetAllStoreByCondition(Expression<Func<Domain.Store, bool>> expression);
        void InsertStore(List<UserService.Domain.Store> entity);
        void UpdateStore(List<UserService.Domain.Store> entity);
        void DeleteStore(List<UserService.Domain.Store> entity);
        IQueryable<UserService.Domain.Store> GetStoreById(Expression<Func<Domain.Store, bool>> expression);
        IQueryable<UserService.Domain.Store> GetAll(Func<UserService.Domain.Store, bool> query = null);
        //IQueryable<UserService.Domain.Store> GetAllLangCode(Func<UserService.Domain.Store, bool> query = null);

        public Task<List<SelectResponseDTO>> SelectStore(string query);

        public Task<List<Domain.Store>> GetListStore(Expression<Func<Domain.Store, bool>> expression);
    }
}
