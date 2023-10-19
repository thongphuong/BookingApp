using BookingService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Service
{
    public interface IStoreService
    {
  
        public Task<ResponseMessage<BookingService.Domain.Store>> GetStore(Guid referenceId);
        public Task<ResponseMessage<List<SelectResponseDTO>>> DropDownStore(string query);

    }
}
