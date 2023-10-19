using AutoMapper;
using BookingService.Domain;
using BookingService.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Service.Service
{
    public class HistoryService :IHistoryService
    {
        private readonly IUnitOfWork _uom;
        //private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public HistoryService(IUnitOfWork uom, IMapper mapper, ICurrentUserService currentUser)
        {
            _uom = uom;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<ResponseMessage<History>> InsertHistory(History history)
        {
            try {
                await _uom.BeginTransactionAsync();
                _uom.History.Create(history);
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                return new ResponseMessage<History>("", HttpStatusCode.OK, history);
            }
            catch {
                return new ResponseMessage<History>("", HttpStatusCode.BadRequest, history);
            }
        }

        //public async Task<ResponseMessage<History>> UpdateHistory(History history) {
        //    try
        //    {
        //        await _uom.BeginTransactionAsync();
        //        _uom.History.Update(history);
        //        await _uom.SaveAsync();
        //        await _uom.CommitAsync();
        //        return new ResponseMessage<History>("", HttpStatusCode.OK, history);
        //    }
        //    catch
        //    {
        //        return new ResponseMessage<History>("", HttpStatusCode.BadRequest, history);
        //    }
        //}
        public async Task<ResponseMessage<List<HistoryDTO>>> GetHistory(QRCodeDTO dto)
        {
            try
            {
                Expression<Func<Domain.History, bool>> expression = (s => (s.CreatedBy == dto.User)
                );

                var result = _uom.History.GetByCondition(expression).ToList();
                
                if (result == null)
                {
                    return new ResponseMessage<List<HistoryDTO>>("", HttpStatusCode.NotFound, new List<HistoryDTO>());
                }
                else
                {

                    List<HistoryDTO> list = new List<HistoryDTO>();
                    foreach (var item in result)
                    {
                        var historydto = new HistoryDTO
                        {
                            Id = item.Id,
                            Time = item.Time,
                            Supplier_Code = item.Supplier_Code,
                            Supplier_Name = _uom.Supplier.GetByCondition((s => (s.Code == item.Supplier_Code))).Select(m => m.Name).FirstOrDefault(),
                            QR_Code = item.QR_Code,
                            Code = item.Code,
                            User = item.CreatedBy,
                            Status = item.Status
                        };
                        list.Add(historydto);
                    }
                    return new ResponseMessage<List<HistoryDTO>> ("", HttpStatusCode.OK, new List<HistoryDTO>
                    (
                        list
                    ));

                }
            }
            catch (Exception)
            {
                return new ResponseMessage<List<HistoryDTO>>("", HttpStatusCode.BadRequest, new List<HistoryDTO>());
            }

        }
        public async Task<ResponseMessage<List<History>>> DeleteHistory(QRCodeDTO dto)
        {
          
                try
                {
                    Expression<Func<Domain.History, bool>> expression = (s => (s.CreatedBy == dto.User)
                    && (s.Status == dto.QR_Status));
                    var result = _uom.History.GetByCondition(expression).ToList();
                    await _uom.BeginTransactionAsync();
                    _uom.History.DeleteRange(result);
                    await _uom.SaveAsync();
                    await _uom.CommitAsync();
                    return new ResponseMessage<List<History>>("", HttpStatusCode.OK, new List<History>(result));
                }
                catch (Exception ex)
               {
                    return new ResponseMessage<List<History>>("", HttpStatusCode.BadRequest, new List<History>());
                }
         
        }

    }
}
