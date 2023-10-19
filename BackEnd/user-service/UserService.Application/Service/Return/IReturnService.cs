using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserService.Service.DTO.Return;

namespace UserService.Service.Return
{
    public interface IReturnService
    {
        Task<ResponseMessage<DataTableResponse>> TableReturn(DataTableParams datatableParams);
        Task<ResponseMessage<ReturnParam>> InsertReturn (ReturnParam param);
    }
}
