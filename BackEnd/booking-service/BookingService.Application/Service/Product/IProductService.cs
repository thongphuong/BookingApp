using BookingService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public interface IProductService
    {
        public Task<ResponseMessage<List<SelectResponseDTO>>> DropDownProduct(string query);
        public Task<ResponseMessage<List<SelectResponseDTO>>> DropDownProductPaging(string query, int skip, int top);
        public Task<ResponseMessage<ProductDTO>> GetProduct(Guid referenceId);
        public Task<List<Product>> GetListProduct(List<string?> lst_qurey);
        Task<ResponseMessage<ProductDownloadDTO>> DownloadProduct(List<ProductDownloadDTO> lst_param, List<ProductDownloadDTO> lst_param_new, List<Product> entitys);
    }
}
