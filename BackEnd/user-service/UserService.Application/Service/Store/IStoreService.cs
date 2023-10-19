using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain;
using UserService.Service.DTO;
using UserService.Service.Interface;
using UserService.Service.Store;

namespace UserService.Service
{
    public interface IStoreService
    {
        Task<IEnumerable<UserService.Domain.Store>> GetAllAsync(CancellationToken token = default);
        Task<UserService.Domain.Store> GetByIdAsync(Guid ownerId);
        Task<UserService.Domain.Store> CreateAsync(UserService.Domain.Store ownerForCreationDto);
        Task UpdateAsync(Guid ownerId, UserService.Domain.Store ownerForUpdateDto);
        Task DeleteAsync(Guid ownerId);

        IQueryable<UserService.Domain.Store> GetAllStoreByCondition(Expression<Func<UserService.Domain.Store, bool>> expression);
        Task<ResponseMessage<Domain.Store>> GetStoreById(string? reference_id);
        Task<ResponseMessage<List<Domain.Store>>> InsertStore(List<StoreModel> model);
        Task<ResponseMessage<List<Domain.Store>>> UpdateStore(List<StoreModel> model);
        Task<ResponseMessage<List<Domain.Store>>> DeleteStore(Param param);
        Task<ResponseMessage<List<Domain.Store>>> ChangeActiveStore(Param param);
        Task<ResponseMessage<DataTableResponse>> TableStore(DataTableParams datatableParams);
        IQueryable<UserService.Domain.Store> GetAll(Func<UserService.Domain.Store, bool> query = null);
        //IQueryable<UserService.Domain.Store> GetAllLangCode(Func<UserService.Domain.Store, bool> query = null);
        public Task<ResponseMessage<Domain.Store>> GetStore(Guid referenceId);
        public Task<ResponseMessage<List<SelectResponseDTO>>> DropDownStore(string query);

    }
}
