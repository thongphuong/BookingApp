using BookingService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public interface ISupplierService
    {
        public Task<ResponseMessage<List<SelectResponseDTO>>> DropDownSupplier(string query);
        public Task<ResponseMessage<SupplierDTO>> GetSupplier(Guid referenceId);

        IQueryable<BookingService.Domain.Supplier> GetByCondition(Expression<Func<Domain.Supplier, bool>> expression);

        public Task<List<Supplier>> GetListSupplier(List<string?> lst_qurey);

        Task<ResponseMessage<SupplierDownloadDTO>> DownloadSupplier(List<SupplierDownloadDTO> lst_param, List<SupplierDownloadDTO> lst_param_new, List<Supplier> entitys);

    }
}
