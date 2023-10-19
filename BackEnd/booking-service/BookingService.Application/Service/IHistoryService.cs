using BookingService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Service.Service
{
    public interface IHistoryService
    {
        Task<ResponseMessage<History>> InsertHistory(History history);

        //Task<ResponseMessage<History>> UpdateHistory(History history);
        Task<ResponseMessage<List<History>>> DeleteHistory(QRCodeDTO dto);
        Task<ResponseMessage<List<HistoryDTO>>> GetHistory(QRCodeDTO dto);


    }
}
