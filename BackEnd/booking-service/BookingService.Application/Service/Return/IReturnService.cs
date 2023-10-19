using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace BookingService.Service
{
    public interface IReturnService
    {
        Task<ResponseMessage<DataTableResponse>> TableReturn(DataTableParams datatableParams);
        Task<ResponseMessage<ReturnParam>> InsertReturn (ReturnParam param);
        Task<ResponseMessage<ReturnParam>> GetReturn (string? reference_id);
        Task<ResponseMessage<Domain.Return>> ChangeActiveReturn(ReturnSearch param);
        Task<ResponseMessage<Domain.Return>> DeleteReturn(ReturnSearch param);
        Task<ResponseMessage<ReturnParam>> UpdateReturn(ReturnParam param);
    }
}
