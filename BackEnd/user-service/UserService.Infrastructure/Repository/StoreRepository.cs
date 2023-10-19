using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain;
using UserService.Service;
using UserService.Service.Interface;

namespace UserService.Infrastructure
{
    public class StoreRepository : GenericRepository<Store>, IStoreRepository
    {
        public StoreRepository(BookingDbContext context) : base(context)
        {

        }

        public Task<Store> CreateAsync(Store ownerForCreationDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid ownerId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Domain.Store> GetAllStoreByCondition(Expression<Func<Store, bool>> expression)
        {
            return FindByCondition(expression);
        }
        public void InsertStore(List<Store> entity)
        {
            CreateRange(entity);

        }
        public void UpdateStore(List<Store> entity)
        {
            UpdateRange(entity);
        }

        public void DeleteStore(List<Store> entity)
        {
            DeleteRange(entity);
        }
        public async Task<List<Store>> GetALLAsync(CancellationToken token = default)
        {
            if (token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();
            }
            return await FindAll().ToListAsync(token);
        }

        public Task<IEnumerable<Store>> GetAllAsync(CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Store> GetAllLangCode(Func<Store, bool> query = null)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Store> GetStoreById(Expression<Func<Domain.Store, bool>> expression)
        {
            return FindByCondition(expression);
        }

        public Task UpdateAsync(Guid ownerId, Store ownerForUpdateDto)
        {
            throw new NotImplementedException();
        }

        public Task<Store> GetByIdAsync(Guid ownerId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Store> GetAll(Func<Store, bool> query = null)
        {
            return FindAll();
        }

        public async Task<List<SelectResponseDTO>> SelectStore(string query)
        {
            return await FindByCondition(p => p.status == (int)Domain.Enum.Status.Active && (string.IsNullOrEmpty(query) || p.name.Contains(query))).Select(p => new SelectResponseDTO
            {
                Key = p.reference_id.ToString(),
                Value = p.name
            }).ToListAsync();
        }

        public async Task<List<Store>> GetListStore(Expression<Func<Domain.Store, bool>> expression)
        {
            return await FindByCondition(expression).ToListAsync();
        }
    }
}
