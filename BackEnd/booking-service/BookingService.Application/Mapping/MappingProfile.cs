using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingService.Domain;
using BookingService.Service;

namespace BookingService.Service.Mapping
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.User, UserDTO>();
            CreateMap<UserDTO, Domain.User>();

            CreateMap<Supplier, SupplierDTO>();
            CreateMap<Product, ProductDTO>();

            CreateMap<OrderDTO, Order>();
            CreateMap<Order, OrderDTO>();

            CreateMap<OrderDeliveryDTO, OrderDelivery>();
            CreateMap<OrderDelivery, OrderDeliveryDTO>();

            CreateMap<OrderReceiptDTO, OrderReceipt>();
            CreateMap<OrderReceipt, OrderReceiptDTO>();

            CreateMap<OrderProductDTO, OrderProduct>();

            CreateMap<SupplierDownloadDTO, Supplier>();

            CreateMap<LineDownloadDTO, Line>();

            CreateMap<DepartmentDownloadDTO, LineDepartment>();

            CreateMap<ProductDownloadDTO, Product>();
        }
    }
}
