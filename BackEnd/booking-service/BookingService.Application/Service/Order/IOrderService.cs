using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public interface IOrderService
    {

        Task<ResponseMessage<OrderRegisterResponse>> AddOrder(OrderDTO orderDto);
        Task<ResponseMessage<OrderRegisterResponse>> EditOrder(OrderDTO orderDto);
        Task<ResponseMessage<DataTableResponse>> TableOrder(DataTableParams datatableParams);
        Task<ResponseMessage<DataTableResponse>> TableOrderRefuse(DataTableParams datatableParams);
        Task<ResponseMessage<OrderDTO>> GetOrder(Guid? referenceId);
        Task<ResponseMessage<OrderDTO>> DeleteOrder(Guid referenceId);
        Task<ResponseMessage<OrderApproveDTO>> ApproveOrder(OrderApproveDTO orderApproveDto);
        Task<ResponseMessage<OrderReturnDTO>> ReturnOrder(OrderReturnDTO orderReturnDTO);
        Task<ResponseMessage<OrderReturnDTO>> RefuseOrder(OrderRefuseDTO orderRefuseDTO);
        public Task<ResponseMessage<QRCodeDTO>> GetByCondition(QRParam param);
        public Task<ResponseMessage<QRCodeDTO>> UpdateQR(QRCodeDTO dto);
        public Task<MemoryStream> ExportOrder(string lang, string url);
        public Task<MemoryStream> ExportOrderRefuse(string lang);

    }
}
