using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using UserService.Service.Store;
using BookingService.Service.Interface;
using BookingService.Service;

namespace UserService.Service.Store
{
    public class StoreService : IStoreService
    {
        private readonly IUnitOfWork _uom;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        private readonly HttpRequest request;
        private readonly IHostEnvironment _HostEnvironment;

        public StoreService(IUnitOfWork uom, IMapper mapper)
        {
            _uom = uom;
            _mapper = mapper;

        }     
        public async Task<ResponseMessage<BookingService.Domain.Store>> GetStore(Guid referenceId)
        {
            try {
                var entity = await _uom.Store.FirstOrDefault(p => p.reference_id == referenceId);
                if (entity == null)
                {
                    return new ResponseMessage<BookingService.Domain.Store>("STORE_NOT_FOUND", HttpStatusCode.NotFound, new BookingService.Domain.Store());
                }
                return new ResponseMessage<BookingService.Domain.Store>("", HttpStatusCode.OK, entity);
            }
            catch (Exception ex) {
                return new ResponseMessage<BookingService.Domain.Store>("Server Error", HttpStatusCode.BadRequest, new BookingService.Domain.Store());
            }
            
        }
        public async Task<ResponseMessage<List<SelectResponseDTO>>> DropDownStore(string query)
        {
            try
            {
                return new ResponseMessage<List<SelectResponseDTO>>("", System.Net.HttpStatusCode.OK, await _uom.Store.SelectStore(query));
            }
            catch
            {
                return new ResponseMessage<List<SelectResponseDTO>>("", System.Net.HttpStatusCode.BadRequest, new List<SelectResponseDTO>());
            }
        }
    }
}
