using AutoMapper;
using BookingService.Domain;
using BookingService.Service.Interface;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IMapper _mapper;
        private readonly HttpRequest request;

        public OrderService(IUnitOfWork uom, IMapper mapper, ICurrentUserService currentUser, IHttpContextAccessor httpContextAccessor) : base(uom, currentUser)
        {
            _mapper = mapper;
            request = httpContextAccessor.HttpContext.Request;
        }

        public async Task<ResponseMessage<OrderRegisterResponse>> AddOrder(OrderDTO orderDto)
        {
            try
            {
                DateTime Now = DateTime.Now;
                Order order = _mapper.Map<Order>(orderDto);
                List<OrderDelivery> orderDeliveries = _mapper.Map<List<OrderDelivery>>(orderDto.orderDeliveryDTOs);
                List<OrderReceipt> orderReceipts = _mapper.Map<List<OrderReceipt>>(orderDto.orderReceiptDTOs);
                var line = await _uom.Line.FirstOrDefault(p => p.ReferenceId == order.Line_Reference);
                var linedepartment = await _uom.LineDepartment.FirstOrDefault(p => p.ReferenceId == order.Department_Reference);
                var store = await _uom.Store.FirstOrDefault(p => p.reference_id == order.Store_Reference);
                var timeframe = await _uom.TimeFrame.FirstOrDefault(p => p.ReferenceId == order.RegisTime_Reference);
                var supplier = await _uom.Supplier.FirstOrDefault(p => p.ReferenceId == order.Supplier_Reference);
                order.Supplier_Code = supplier.Code;
                order.Line_Code = line.Code;
                order.Line_Name = line.Name;
                order.Department_Code = linedepartment.Code;
                order.Department_Name = linedepartment.Name;
                order.Store_Code = store.code;
                order.Store_Name = store.name;
                order.register_from = timeframe.TimeFrom;
                order.register_to = timeframe.TimeTo;
                order.CreatedAt = Now;
                order.Status = (int)Domain.Enum.Status.Active;
                order.Delivery_Status = (int)Domain.Enum.DeliveryStatus.NotDelivery;
                order.ReferenceId = Guid.NewGuid();
                do
                {
                    order.Code = Utils.RandomString(2, 5);
                }
                while ((await _uom.Order.IsExist(p => p.Code == order.Code)));
                order.QR_Code = order.Code;
                order.Link = order.ReferenceId + "-" + order.Store_Reference + "-" + order.Supplier_Reference;
                orderDeliveries.ForEach(p =>
                {
                    p.Order_Reference = order.ReferenceId;
                    p.ReferenceId = Guid.NewGuid();
                    p.Status = 1;
                    p.CreatedAt = Now;
                });
                orderReceipts.ForEach(p =>
                {
                    p.Order_Reference = order.ReferenceId;
                    p.ReferenceId = Guid.NewGuid();
                    p.Status = 1;
                    p.CreatedAt = Now;
                });

                var count = await _uom.Order.Count(p => p.RegisTime_Reference == order.RegisTime_Reference && p.Delivery_Regis_Date == order.Delivery_Regis_Date);
                if (count > timeframe.Amount)
                    return new ResponseMessage<OrderRegisterResponse>("ORDER_MAX_AMOUNT", HttpStatusCode.Conflict, new OrderRegisterResponse());
                await _uom.BeginTransactionAsync();
                _uom.Order.Create(order);
                if (orderDeliveries.Any())
                    _uom.OrderDelivery.CreateRange(orderDeliveries);
                if (orderReceipts.Any())
                    _uom.OrderReceipt.CreateRange(orderReceipts);
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                return new ResponseMessage<OrderRegisterResponse>("Add Success", HttpStatusCode.OK, new OrderRegisterResponse { Code = order.Code });
            }
            catch (Exception ex)
            {
                return new ResponseMessage<OrderRegisterResponse>(ex.ToString(), HttpStatusCode.BadRequest, new OrderRegisterResponse());
            }

        }

        public async Task<ResponseMessage<DataTableResponse>> TableOrder(DataTableParams datatableParams)
        {
            try
            {
                var qSupplierName = request.Query["supplier_name"].ToString() ?? "";
                var qDepartmentCode = request.Query["department_code"].ToString() ?? "";
                var qDeliveryStatus = int.TryParse(request.Query["delivery_status"].ToString() ?? "", out int v) ? v : -1;
                var qReceiptStatus = int.TryParse(request.Query["receipt_status"].ToString() ?? "", out int v1) ? v1 : -1;
                var qStore = request.Query["store"].ToString() ?? "";
                var qDateFrom = request.Query["date_from"].ToString() ?? "";
                var qDateTo = request.Query["date_To"].ToString() ?? "";

                DateTime? dateFrom = null;
                DateTime? DateTo = null;
                if (!string.IsNullOrEmpty(qDateFrom))
                    dateFrom = DateTime.Parse(qDateFrom);
                if (!string.IsNullOrEmpty(qDateTo))
                    DateTo = DateTime.Parse(qDateTo).AddDays(1);

                Expression<Func<Order, bool>> query = (o => o.Status != (int)Domain.Enum.Status.Delete
                && (string.IsNullOrEmpty(qSupplierName) || (o.Supplier_Name ?? "").ToLower().Contains(qSupplierName.ToLower()))
                && (string.IsNullOrEmpty(qDepartmentCode) || (o.Department_Code ?? "").ToLower().Contains(qDepartmentCode.ToLower()))
                && (string.IsNullOrEmpty(qStore) || (o.Store_Name ?? "").ToLower().Contains(qStore.ToLower()))
                && (string.IsNullOrEmpty(qDateFrom) || o.Delivery_Regis_Date >= dateFrom)
                && (string.IsNullOrEmpty(qDateTo) || o.Delivery_Regis_Date <= DateTo)
                && (qDeliveryStatus == -1 || qDeliveryStatus == o.Delivery_Status)
                && (qReceiptStatus == -1 || qReceiptStatus == o.Receipt_Status)
                && (_currentUser.User.FullName == "Admin" || o.Store_Reference == _currentUser.User.StoreManagement));

                Expression<Func<Order, object>> sort = (s =>
                datatableParams.iSortCol_0 == 1 ? s.Store_Name :
                datatableParams.iSortCol_0 == 2 ? s.Department_Code :
                datatableParams.iSortCol_0 == 3 ? s.Supplier_Name :
                datatableParams.iSortCol_0 == 4 ? s.Driver_Name :
                datatableParams.iSortCol_0 == 5 ? s.Receiver_Name :
                datatableParams.iSortCol_0 == 6 ? s.Delivery_Regis_Date :
                datatableParams.iSortCol_0 == 7 ? s.Receive_Time :
                datatableParams.iSortCol_0 == 8 ? s.Check_In :
                datatableParams.iSortCol_0 == 9 ? s.Check_Out
                : (object)s.Delivery_Regis_Date);

                (var model, int total) = await _uom.Order.PagingData(query, sort, null, datatableParams.sSortDir_0, datatableParams.iDisplayStart, datatableParams.iDisplayLength);

                var result = from c in await Task.FromResult(model.ToArray())
                             select new object[] {
                                 string.Empty,
                                 c.Store_Name ?? string.Empty,
                                 c.Department_Code ?? string.Empty,
                                 c.Supplier_Name ?? string.Empty,
                                 c.Driver_Name ?? string.Empty,
                                 c.Receiver_Name ?? string.Empty,
                                 c.Delivery_Regis_Date?.ToString("dd/MM/yyyy") ?? string.Empty,
                                 c.Receive_Time?.ToString("HH:mm") ?? string.Empty,
                                 c.Check_In?.ToString("HH:mm") ?? string.Empty,
                                 c.Check_Out?.ToString("HH:mm") ?? string.Empty,
                                 Utils.GetStausDelivery(c.Delivery_Status ?? 0),
                                 Utils.GetStausReceipt(c.Receipt_Status ?? 0),
                                 c.Reason ?? string.Empty,
                                 c.Link ?? string.Empty,
                                 Utils.CommandButton(new List<string>(){ CheckPermissions("Order", "Approve").Result ? "Approve" : "", CheckPermissions("Order", "Refuse").Result ? "Refuse" : "", /*CheckPermissions("Order", "Return").Result ? "Return" : ""*/},c.ReferenceId.GetValueOrDefault(),-1),
                                 Utils.CommandButton(new List<string>(){ CheckPermissions("Order", "Detail","Booking").Result ? "Detail" : "", CheckPermissions("Order", "Delete").Result ? "Delete" : "", CheckPermissions("Order", "Edit").Result && c.Delivery_Status == (int)Domain.Enum.DeliveryStatus.NotDelivery ? "Edit" : "NoEdit"},c.ReferenceId ?? Guid.Empty,c.Status)
                        };

                return new ResponseMessage<DataTableResponse>("", HttpStatusCode.OK, new DataTableResponse()
                {
                    iTotalDisplayRecords = total,
                    iTotalRecords = total,
                    sEcho = datatableParams.sEcho,
                    aaData = result,
                });
            }
            catch (Exception ex)
            {
                return new ResponseMessage<DataTableResponse>(ex.ToString(), HttpStatusCode.BadRequest, new DataTableResponse()
                {
                    iTotalDisplayRecords = 0,
                    iTotalRecords = 0,
                    sEcho = datatableParams.sEcho,
                    aaData = new List<object>()
                });
            }
        }

        public async Task<ResponseMessage<DataTableResponse>> TableOrderRefuse(DataTableParams datatableParams)
        {
            try
            {
                var qStoreReference = request.Query["store_reference"].ToString() ?? "";
                var qLineReference = request.Query["line_reference"].ToString() ?? "";
                var qSupplierName = request.Query["supplier_name"].ToString() ?? "";
                var qDateFrom = request.Query["date_from"].ToString() ?? "";
                var qDateTo = request.Query["date_To"].ToString() ?? "";
                var qSupplierCode = request.Query["supplier_code"].ToString() ?? "";


                DateTime? dateFrom = null;
                DateTime? DateTo = null;
                if (!string.IsNullOrEmpty(qDateFrom))
                    dateFrom = DateTime.Parse(qDateFrom);
                if (!string.IsNullOrEmpty(qDateTo))
                    DateTo = DateTime.Parse(qDateTo).AddDays(1);

                Expression<Func<Order, bool>> query = (o => o.Status != (int)Domain.Enum.Status.Delete
                && (o.Delivery_Status == (int)Domain.Enum.DeliveryStatus.ReturnDelivery || o.Delivery_Status == (int)Domain.Enum.DeliveryStatus.PartiallyDelivered)
                && (string.IsNullOrEmpty(qStoreReference) || o.Store_Reference.ToString() == qStoreReference)
                && (string.IsNullOrEmpty(qLineReference) || o.Line_Reference.ToString() == qLineReference)
                && (string.IsNullOrEmpty(qSupplierName) || (o.Supplier_Name ?? "").ToLower().Contains(qSupplierName.ToLower()))
                && (string.IsNullOrEmpty(qSupplierCode) || (o.Supplier_Code ?? "").ToLower().Contains(qSupplierCode.ToLower()))
                && (string.IsNullOrEmpty(qDateFrom) || o.Delivery_Date >= dateFrom)
                && (string.IsNullOrEmpty(qDateTo) || o.Delivery_Date <= DateTo));

                Expression<Func<Order, object>> sort = (s => (object)s.CreatedAt);

                (var model, int total) = await _uom.Order.PagingData(query, sort, null, "asc", datatableParams.iDisplayStart, datatableParams.iDisplayLength);

                var result = from c in await Task.FromResult(model.ToArray())
                             select new object[] {
                                 string.Empty,
                                 c.Line_Code ?? string.Empty,
                                 c.Supplier_Code ?? string.Empty,
                                 c.Supplier_Name ?? string.Empty,
                                 c.Store_Name ?? string.Empty,
                                 c.Refuse_Date?.ToString("dd/MM/yyyy") ?? string.Empty,
                                 Utils.CommandButton(new List<string>(){ CheckPermissions("Order", "Detail", "Refuse").Result ? "Detail" : "", CheckPermissions("Order", "Print").Result ? "Print" : ""},c.ReferenceId ?? Guid.Empty,c.Status)
                        };

                return new ResponseMessage<DataTableResponse>("", HttpStatusCode.OK, new DataTableResponse()
                {
                    iTotalDisplayRecords = total,
                    iTotalRecords = total,
                    sEcho = datatableParams.sEcho,
                    aaData = result,
                });
            }
            catch (Exception ex)
            {
                return new ResponseMessage<DataTableResponse>(ex.ToString(), HttpStatusCode.BadRequest, new DataTableResponse()
                {
                    iTotalDisplayRecords = 0,
                    iTotalRecords = 0,
                    sEcho = datatableParams.sEcho,
                    aaData = new List<object>()
                });
            }
        }

        public async Task<ResponseMessage<OrderDTO>> GetOrder(Guid? referenceId)
        {
            try
            {
                var order = await _uom.Order.FirstOrDefault(p => p.ReferenceId == referenceId);
                if (order is null)
                    return new ResponseMessage<OrderDTO>("ORDER_NOT_FOUND", HttpStatusCode.NotFound, new OrderDTO());

                var orderDelivery = _uom.OrderDelivery.FindByCondition(p => p.Order_Reference == order.ReferenceId).ToList();
                var orderReceipt = _uom.OrderReceipt.FindByCondition(p => p.Order_Reference == order.ReferenceId).ToList();
                var orderProduct = _uom.OrderProduct.FindByCondition(p => p.Order_Reference == order.ReferenceId).ToList();
                OrderDTO userDto = new OrderDTO()
                {
                    ReferenceId = order.ReferenceId.GetValueOrDefault(),
                    Department_Code = order.Department_Code,
                    Store_Reference = order.Store_Reference,
                    Store_Name = order.Store_Name,
                    Supplier_Reference = order.Supplier_Reference,
                    Supplier_Code = order.Supplier_Code,
                    Supplier_Name = order.Supplier_Name,
                    Supplier_Email = order.Supplier_Email,
                    Supplier_PhoneNumber = order.Supplier_PhoneNumber,
                    Line_Reference = order.Line_Reference,
                    Line_Name = order.Line_Code + " - " + order.Line_Name,
                    Department_Reference = order.Department_Reference,
                    Department_Name = order.Department_Name,
                    Delivery_Date = order.Delivery_Date,
                    Delivery_Status_Value = order.Delivery_Status,
                    Delivery_Status_Name = Utils.GetStausDelivery(order.Delivery_Status.GetValueOrDefault()),
                    Receipt_Status_Value = order.Receipt_Status,
                    Receipt_Status_Name = Utils.GetStausReceipt(order.Receipt_Status.GetValueOrDefault()),
                    Delivery_Order_Quantity = order.Delivery_Order_Quantity,
                    Vehicle_Name = order.Vehicle_Name,
                    Lisense_Plate = order.Lisense_Plate,
                    Driver_Name = order.Driver_Name,
                    CMND = order.CMND,
                    Emergn_Email = order.Emergn_Email,
                    Emergn_Phone = order.Emergn_Phone,
                    Delivery_Item_Quantity = order.Delivery_Item_Quantity,
                    Delivery_Regis_Date = order.Delivery_Regis_Date,
                    Miss_Invoice_Number = order.Miss_Invoice_Number,
                    Delivery_Invoice_Date = order.Delivery_Invoice_Date,
                    RegisTime_Reference = order.RegisTime_Reference,
                    register_from = order.register_from,
                    register_to = order.register_to,
                    Invoice_Number = order.Invoice_Number,
                    Receiver_Department = order.Receiver_Department,
                    Receiver_Name = order.Receiver_Name,
                    Refuse_Date = order.Refuse_Date,
                    Refuse_Status = order.Refuse_Status,
                    Reason = order.Reason,
                    Image = order.Image,
                    orderDeliveryDTOs = orderDelivery.Select(p => new OrderDeliveryDTO
                    {
                        Code = p.Code,
                    }).ToList(),
                    orderReceiptDTOs = orderReceipt.Select(p => new OrderReceiptDTO
                    {
                        Code = p.Code
                    }).ToList(),
                    orderProductDTOs = orderProduct.Select(p => new OrderProductDTO
                    {
                        Product_Code = p.Product_Code,
                        Product_Name = p.Product_Name,
                        Product_Total_Order = p.Product_Total_Order,
                        Product_Total_Refuse = p.Product_Total_Refuse,
                        Product_Note = p.Product_Note,
                        Image = p.Image,
                    }).ToList()
                };

                return new ResponseMessage<OrderDTO>("", HttpStatusCode.OK, userDto);
            }
            catch (Exception ex)
            {
                return new ResponseMessage<OrderDTO>(ex.ToString(), HttpStatusCode.BadRequest, new OrderDTO());
            }
        }

        public async Task<ResponseMessage<QRCodeDTO>> GetByCondition(QRParam param)
        {

            try
            {
                Expression<Func<Domain.Order, bool>> expression = (s => (
                ((s.QR_Code == param.QR_Code)
                || (s.Code == param.QR_Code))
                || ((s.Code == param.Code)
                || (s.QR_Code == param.Code)))
                );

                var result = _uom.Order.GetByCondition(expression).FirstOrDefault();

                if (result == null)
                {
                    return new ResponseMessage<QRCodeDTO>("", HttpStatusCode.NotFound, new QRCodeDTO() { });
                }
                Expression<Func<Domain.Supplier, bool>> expression1 = (s => (s.ReferenceId == result.Supplier_Reference));

                var code = _uom.Supplier.GetByCondition(expression1).Select(m => m.Code).FirstOrDefault();

                var qrstatus = -1;
                if (result.Check_In != null)
                {
                    qrstatus = (int)(Domain.Enum.QR_Status.CheckIn);
                }
                if (result.Check_Out != null)
                {
                    qrstatus = (int)(Domain.Enum.QR_Status.CheckOut);
                }
                return new ResponseMessage<QRCodeDTO>("", HttpStatusCode.OK, new QRCodeDTO
                {
                    Id = result.Id,
                    Supplier_Code = code,
                    Supplier_Reference = result.Supplier_Reference,
                    Supplier_Name = result.Supplier_Name,
                    Vehicle_Name = result.Vehicle_Name,
                    Driver_Name = result.Driver_Name,
                    QR_Status = qrstatus,
                    QR_Code = result.QR_Code,
                    Code = result.Code,
                    Lisense_Plate = result.Lisense_Plate

                });
            }
            catch (Exception ex)
            {
                return new ResponseMessage<QRCodeDTO>(ex.ToString(), HttpStatusCode.BadRequest, new QRCodeDTO() { });
            }
        }

        public async Task<ResponseMessage<QRCodeDTO>> UpdateQR(QRCodeDTO dto)
        {
            try
            {
                Expression<Func<Domain.Order, bool>> expression = (s => (s.Id == dto.Id)
                     && (((s.QR_Code == dto.QR_Code) || (s.Code == dto.QR_Code))
                     || ((s.QR_Code == dto.Code) || (s.Code == dto.Code))));

                var result = _uom.Order.GetByCondition(expression).FirstOrDefault();
                if (result == null)
                {
                    return new ResponseMessage<QRCodeDTO>("", HttpStatusCode.NotFound, new QRCodeDTO() { });
                }
                else
                {
                    //check if qrcode exist
                    if (result.Check_In != null && result.Check_Out != null)
                    {
                        return new ResponseMessage<QRCodeDTO>("", HttpStatusCode.Conflict, new QRCodeDTO());
                    }
                    // update time check-in check-out table order
                    if (dto.QR_Status == (int)(Domain.Enum.QR_Status.CheckIn))
                    {
                        result.Check_In = DateTime.Now;
                    }
                    if (dto.QR_Status == (int)(Domain.Enum.QR_Status.CheckOut))
                    {
                        result.Check_Out = DateTime.Now;

                    }

                    //create history when check-in check out
                    var history = new History();
                    history.QR_Code = dto.QR_Code;
                    history.Code = dto.Code;
                    history.Supplier_Code = dto.Supplier_Code;
                    history.Time = DateTime.Now;
                    history.CreatedBy = dto.User;
                    history.Status = dto.QR_Status;

                    await _uom.BeginTransactionAsync();
                    _uom.Order.Update(result);
                    _uom.History.Create(history);
                    await _uom.SaveAsync();
                    await _uom.CommitAsync();
                    return new ResponseMessage<QRCodeDTO>("", HttpStatusCode.OK, new QRCodeDTO());

                }
            }
            catch (Exception ex)
            {
                return new ResponseMessage<QRCodeDTO>(ex.ToString(), HttpStatusCode.BadRequest, new QRCodeDTO());
            }
        }

        public async Task<ResponseMessage<OrderDTO>> DeleteOrder(Guid referenceId)
        {
            Order order = await _uom.Order.FirstOrDefault(p => p.ReferenceId == referenceId);
            order.DeletedAt = DateTime.Now;
            order.DeteleBy = _currentUser.User.Id;
            order.Status = (int)Domain.Enum.Status.Delete;
            try
            {
                await _uom.BeginTransactionAsync();
                _uom.Order.Update(order);
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                await Log(Domain.Enum.Menu.ORDER.ToString(), Domain.Enum.Action.DELETED.ToString(), "Delete success", Domain.Enum.LogStatus.SUCCESS.ToString());
                return new ResponseMessage<OrderDTO>("Delete Success", HttpStatusCode.OK, new OrderDTO());
            }
            catch (Exception ex)
            {
                await _uom.RollBackAsync();
                await Log(Domain.Enum.Menu.ORDER.ToString(), Domain.Enum.Action.DELETED.ToString(), "Delete false", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<OrderDTO>(ex.ToString(), HttpStatusCode.BadRequest, new OrderDTO());
            }
        }

        public async Task<ResponseMessage<OrderRegisterResponse>> EditOrder(OrderDTO orderDto)
        {
            var order = await _uom.Order.FirstOrDefault(p => p.ReferenceId == orderDto.ReferenceId);
            try
            {
                DateTime Now = DateTime.Now;
                var orderDeliveriesOld = _uom.OrderDelivery.FindByCondition(p => p.Order_Reference == orderDto.ReferenceId).ToList();
                var orderReceiptsOld = _uom.OrderReceipt.FindByCondition(p => p.Order_Reference == orderDto.ReferenceId).ToList();

                if (order is null)
                {
                    await Log(Domain.Enum.Menu.ORDER.ToString(), Domain.Enum.Action.EDIT.ToString(), "Order not found", Domain.Enum.LogStatus.FALSE.ToString());
                    return new ResponseMessage<OrderRegisterResponse>("ORDER_NOT_FOUND", HttpStatusCode.NotFound, new OrderRegisterResponse());
                }
                orderDto.Store_Code = order.Store_Code;
                orderDto.Store_Name = order.Store_Name;
                order = _mapper.Map(orderDto, order);
                List<OrderDelivery> orderDeliveries = _mapper.Map<List<OrderDelivery>>(orderDto.orderDeliveryDTOs);
                List<OrderReceipt> orderReceipts = _mapper.Map<List<OrderReceipt>>(orderDto.orderReceiptDTOs);
                var line = await _uom.Line.FirstOrDefault(p => p.ReferenceId == order.Line_Reference);
                if (line is null)
                {
                    await Log(Domain.Enum.Menu.ORDER.ToString(), Domain.Enum.Action.EDIT.ToString(), "Line not found", Domain.Enum.LogStatus.FALSE.ToString());
                    return new ResponseMessage<OrderRegisterResponse>("LINE_NOT_FOUND", HttpStatusCode.NotFound, new OrderRegisterResponse());
                }
                var linedepartment = await _uom.LineDepartment.FirstOrDefault(p => p.ReferenceId == order.Department_Reference);
                if (linedepartment is null)
                {
                    await Log(Domain.Enum.Menu.ORDER.ToString(), Domain.Enum.Action.EDIT.ToString(), "Department not found", Domain.Enum.LogStatus.FALSE.ToString());
                    return new ResponseMessage<OrderRegisterResponse>("DEPARTMENT_NOT_FOUND", HttpStatusCode.NotFound, new OrderRegisterResponse());
                }
                var timeframe = await _uom.TimeFrame.FirstOrDefault(p => p.ReferenceId == order.RegisTime_Reference);
                if (timeframe is null)
                {
                    await Log(Domain.Enum.Menu.ORDER.ToString(), Domain.Enum.Action.EDIT.ToString(), "Timeframe not found", Domain.Enum.LogStatus.FALSE.ToString());
                    return new ResponseMessage<OrderRegisterResponse>("TIMEFRAME_NOT_FOUND", HttpStatusCode.NotFound, new OrderRegisterResponse());
                }
                var supplier = await _uom.Supplier.FirstOrDefault(p => p.ReferenceId == order.Supplier_Reference);
                if (supplier is null)
                {
                    await Log(Domain.Enum.Menu.ORDER.ToString(), Domain.Enum.Action.EDIT.ToString(), "Supplier not found", Domain.Enum.LogStatus.FALSE.ToString());
                    return new ResponseMessage<OrderRegisterResponse>("SUPPLIER_NOT_FOUND", HttpStatusCode.NotFound, new OrderRegisterResponse());
                }
                order.Supplier_Code = supplier.Code;
                order.Line_Code = line.Code;
                order.Line_Name = line.Name;
                order.Department_Code = linedepartment.Code;
                order.Department_Name = linedepartment.Name;
                order.register_from = timeframe.TimeFrom;
                order.register_to = timeframe.TimeTo;
                order.ModifiedAt = Now;
                order.ModifiedBy = _currentUser.User.Id;
                orderDeliveries.ForEach(p =>
                {
                    p.Order_Reference = order.ReferenceId;
                    p.ReferenceId = Guid.NewGuid();
                    p.Status = 1;
                    p.CreatedAt = Now;
                });
                orderReceipts.ForEach(p =>
                {
                    p.Order_Reference = order.ReferenceId;
                    p.ReferenceId = Guid.NewGuid();
                    p.Status = 1;
                    p.CreatedAt = Now;
                });

                var count = await _uom.Order.Count(p => p.Id != order.Id && p.RegisTime_Reference == order.RegisTime_Reference && p.Delivery_Regis_Date == order.Delivery_Regis_Date);
                if (count > timeframe.Amount)
                    return new ResponseMessage<OrderRegisterResponse>("ORDER_MAX_AMOUNT", HttpStatusCode.Forbidden, new OrderRegisterResponse());
                await _uom.BeginTransactionAsync();
                _uom.Order.Update(order);
                if (orderDeliveries.Any())
                {
                    _uom.OrderDelivery.DeleteRange(orderDeliveriesOld);
                    _uom.OrderDelivery.CreateRange(orderDeliveries);
                }
                if (orderReceipts.Any())
                {
                    _uom.OrderReceipt.DeleteRange(orderReceiptsOld);
                    _uom.OrderReceipt.CreateRange(orderReceipts);
                }
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                await Log(Domain.Enum.Menu.ORDER.ToString(), Domain.Enum.Action.EDIT.ToString(), "Edit success", Domain.Enum.LogStatus.SUCCESS.ToString());
                return new ResponseMessage<OrderRegisterResponse>("Edit Success", HttpStatusCode.OK, new OrderRegisterResponse());
            }
            catch (Exception ex)
            {
                await Log(Domain.Enum.Menu.ORDER.ToString(), Domain.Enum.Action.EDIT.ToString(), "Edit false", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<OrderRegisterResponse>(ex.ToString(), HttpStatusCode.BadRequest, new OrderRegisterResponse());
            }
        }

        public async Task<ResponseMessage<OrderApproveDTO>> ApproveOrder(OrderApproveDTO orderApproveDto)
        {
            Order order = await _uom.Order.FirstOrDefault(p => p.ReferenceId == orderApproveDto.ReferenceId);
            if (order is null)
            {
                await Log(Domain.Enum.Menu.ORDER.ToString(), Domain.Enum.Action.APPROVE.ToString(), "Order not found", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<OrderApproveDTO>("ORDER_NOT_FOUND", HttpStatusCode.NotFound, new OrderApproveDTO());
            }
            List<OrderProduct> productOld = _uom.OrderProduct.FindByCondition(p => p.Order_Reference == order.ReferenceId).ToList();
            order.Receiver_Name = orderApproveDto.Receiver_Name;
            order.Receiver_Department = orderApproveDto.Receiver_Department;
            order.Miss_Invoice_Number = orderApproveDto.Miss_Invoice_Number;
            order.Delivery_Invoice_Date = orderApproveDto.Delivery_Invoice_Date;
            order.Receipt_Status = orderApproveDto.Receipt_Status;
            order.Refuse_Status = orderApproveDto.Refuse_Status;
            order.Delivery_Status = order.Refuse_Status == (int)Domain.Enum.Refuse.Full ? (int)Domain.Enum.DeliveryStatus.FullDelivered : (int)Domain.Enum.DeliveryStatus.PartiallyDelivered;
            order.Receive_Time = DateTime.Now;
            List<OrderProduct> orderProduct = new List<OrderProduct>();
            foreach (var p in orderApproveDto.Order_Product)
            {
                var product = new OrderProduct();
                product.Order_Reference = order.ReferenceId;
                product.Product_Name = p.Product_Name;
                product.Product_Code = p.Product_Code;
                product.Product_Note = p.Product_Note;
                product.Status = (int)Domain.Enum.Status.Active;
                product.CreatedAt = DateTime.Now;
                product.CreatedBy = _currentUser.User.Id;
                product.Product_Total_Order = p.Product_Total_Order;
                product.Product_Total_Refuse = p.Product_Total_Refuse;
                if (p.ImageFile != null && p.ImageFile.Any())
                    product.Image = await Utils.UploadAzureStorage(p.ImageFile, "Product");
                orderProduct.Add(product);
            };
            try
            {
                await _uom.BeginTransactionAsync();
                _uom.OrderProduct.DeleteRange(productOld);
                if (orderProduct.Any())
                    _uom.OrderProduct.CreateRange(orderProduct);
                _uom.Order.Update(order);
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                await Log(Domain.Enum.Menu.ORDER.ToString(), Domain.Enum.Action.APPROVE.ToString(), "Approve success", Domain.Enum.LogStatus.SUCCESS.ToString());
                return new ResponseMessage<OrderApproveDTO>("Approve Success", HttpStatusCode.OK, new OrderApproveDTO());
            }
            catch (Exception ex)
            {
                await _uom.RollBackAsync();
                await Log(Domain.Enum.Menu.ORDER.ToString(), Domain.Enum.Action.APPROVE.ToString(), "Approve false", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<OrderApproveDTO>(ex.ToString(), HttpStatusCode.BadRequest, new OrderApproveDTO());
            }
        }

        public async Task<ResponseMessage<OrderReturnDTO>> ReturnOrder(OrderReturnDTO orderReturnDTO)
        {
            Order order = await _uom.Order.FirstOrDefault(p => p.ReferenceId == orderReturnDTO.ReferenceId);
            if (order is null)
            {
                await Log(Domain.Enum.Menu.ORDER.ToString(), Domain.Enum.Action.RETURN.ToString(), "Order not found", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<OrderReturnDTO>("ORDER_NOT_FOUND", HttpStatusCode.OK, new OrderReturnDTO());
            }
            order.Receiver_Name = orderReturnDTO.Receiver_Name;
            order.Receiver_Department = orderReturnDTO.Receiver_Department;
            order.Miss_Invoice_Number = orderReturnDTO.Miss_Invoice_Number;
            order.Delivery_Invoice_Date = orderReturnDTO.Delivery_Invoice_Date;
            order.Receipt_Status = orderReturnDTO.Receipt_Status;
            order.Reason = orderReturnDTO.Reason;
            order.Delivery_Status = (int)Domain.Enum.DeliveryStatus.PartiallyDelivered;
            order.Receive_Time = DateTime.Now;
            try
            {
                await _uom.BeginTransactionAsync();
                _uom.Order.Update(order);
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                await Log(Domain.Enum.Menu.ORDER.ToString(), Domain.Enum.Action.RETURN.ToString(), "Return false", Domain.Enum.LogStatus.SUCCESS.ToString());
                return new ResponseMessage<OrderReturnDTO>("Return Success", HttpStatusCode.OK, new OrderReturnDTO());
            }
            catch (Exception ex)
            {
                await _uom.RollBackAsync();
                await Log(Domain.Enum.Menu.ORDER.ToString(), Domain.Enum.Action.RETURN.ToString(), "Return false", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<OrderReturnDTO>(ex.ToString(), HttpStatusCode.BadRequest, new OrderReturnDTO());
            }
        }

        public async Task<ResponseMessage<OrderReturnDTO>> RefuseOrder(OrderRefuseDTO orderRefuseDTO)
        {
            Order order = await _uom.Order.FirstOrDefault(p => p.ReferenceId == orderRefuseDTO.ReferenceId);
            if (order is null)
            {
                await Log(Domain.Enum.Menu.ORDER.ToString(), Domain.Enum.Action.REFUSE.ToString(), "Order not found", Domain.Enum.LogStatus.FALSE.ToString());
                return new ResponseMessage<OrderReturnDTO>("ORDER_NOT_FOUND", HttpStatusCode.OK, new OrderReturnDTO());
            }
            List<OrderProduct> productOld = _uom.OrderProduct.FindByCondition(p => p.Order_Reference == order.ReferenceId).ToList();
            order.Receiver_Name = orderRefuseDTO.Receiver_Name;
            order.Receiver_Department = orderRefuseDTO.Receiver_Department;
            order.Receipt_Status = orderRefuseDTO.Receipt_Status;
            order.Refuse_Status = orderRefuseDTO.Refuse_Status;
            order.Delivery_Status = (int)Domain.Enum.DeliveryStatus.ReturnDelivery;
            order.Refuse_Date = DateTime.Now;
            if (order.Refuse_Status == (int)Domain.Enum.Refuse.Full && orderRefuseDTO.ImageFile != null && orderRefuseDTO.ImageFile.Any())
            {
                order.Image = await Utils.UploadAzureStorage(orderRefuseDTO.ImageFile, "Product");
            }
            List<OrderProduct> orderProduct = new List<OrderProduct>();
            foreach (var p in orderRefuseDTO.Order_Product)
            {
                var product = new OrderProduct();
                product.Order_Reference = order.ReferenceId;
                product.Product_Name = p.Product_Name;
                product.Product_Code = p.Product_Code;
                product.Product_Note = p.Product_Note;
                product.Status = (int)Domain.Enum.Status.Active;
                product.CreatedAt = DateTime.Now;
                product.CreatedBy = _currentUser.User.Id;
                product.Product_Total_Order = p.Product_Total_Order;
                product.Product_Total_Refuse = p.Product_Total_Refuse;
                if (p.ImageFile != null && p.ImageFile.Any())
                    product.Image = await Utils.UploadAzureStorage(p.ImageFile, "Product");
                orderProduct.Add(product);
            };
            try
            {
                await _uom.BeginTransactionAsync();
                _uom.OrderProduct.DeleteRange(productOld);
                if (orderProduct.Any())
                    _uom.OrderProduct.CreateRange(orderProduct);
                _uom.Order.Update(order);
                await _uom.SaveAsync();
                await _uom.CommitAsync();
                await Log(Domain.Enum.Menu.ORDER.ToString(), Domain.Enum.Action.REFUSE.ToString(), "Refuse false", Domain.Enum.LogStatus.SUCCESS.ToString());
                return new ResponseMessage<OrderReturnDTO>("Return Success", HttpStatusCode.OK, new OrderReturnDTO());
            }
            catch (Exception ex)
            {
                await _uom.RollBackAsync();
                await Log(Domain.Enum.Menu.ORDER.ToString(), Domain.Enum.Action.REFUSE.ToString(), "Refuse false", Domain.Enum.LogStatus.SUCCESS.ToString());
                return new ResponseMessage<OrderReturnDTO>(ex.ToString(), HttpStatusCode.BadRequest, new OrderReturnDTO());
            }
        }

        public async Task<MemoryStream> ExportOrder(string lang, string url)
        {
            try
            {


                var stream = new MemoryStream();
                var qSupplierName = request.Query["supplier_name"].ToString() ?? "";
                var qDepartmentCode = request.Query["department_code"].ToString() ?? "";
                var qDeliveryStatus = int.TryParse(request.Query["delivery_status"].ToString() ?? "", out int v) ? v : -1;
                var qReceiptStatus = int.TryParse(request.Query["receipt_status"].ToString() ?? "", out int v1) ? v1 : -1;
                var qStore = request.Query["store"].ToString() ?? "";
                var qDateFrom = request.Query["date_from"].ToString() ?? "";
                var qDateTo = request.Query["date_To"].ToString() ?? "";

                DateTime? dateFrom = null;
                DateTime? DateTo = null;
                if (!string.IsNullOrEmpty(qDateFrom))
                    dateFrom = DateTime.Parse(qDateFrom);
                if (!string.IsNullOrEmpty(qDateTo))
                    DateTo = DateTime.Parse(qDateTo).AddDays(1);

                Expression<Func<Order, bool>> query = (o => o.Status != (int)Domain.Enum.Status.Delete
                && (string.IsNullOrEmpty(qSupplierName) || (o.Supplier_Name ?? "").ToLower().Contains(qSupplierName.ToLower()))
                && (string.IsNullOrEmpty(qDepartmentCode) || (o.Department_Code ?? "").ToLower().Contains(qDepartmentCode.ToLower()))
                && (string.IsNullOrEmpty(qStore) || (o.Store_Name ?? "").ToLower().Contains(qStore.ToLower()))
                && (string.IsNullOrEmpty(qDateFrom) || o.Delivery_Date >= dateFrom)
                && (string.IsNullOrEmpty(qDateTo) || o.Delivery_Date <= DateTo)
                && (qDeliveryStatus == -1 || qDeliveryStatus == o.Delivery_Status)
                && (qReceiptStatus == -1 || qReceiptStatus == o.Receipt_Status)
                && (_currentUser.User.FullName == "Admin" || o.Store_Reference == _currentUser.User.StoreManagement));

                var model = _uom.Order.FindByCondition(query).ToList().Select((c, index) => new OrderExcelDTO
                {
                    No = index + 1,
                    Store_Name = c.Store_Name ?? string.Empty,
                    Department_Code = c.Department_Code ?? string.Empty,
                    Supplier_Name = c.Supplier_Name ?? string.Empty,
                    Driver_Name = c.Driver_Name ?? string.Empty,
                    Receiver_Name = c.Receiver_Name ?? string.Empty,
                    Delivery_Regis_Date = c.Delivery_Regis_Date?.ToString("dd/MM/yyyy") ?? string.Empty,
                    Receive_Time = c.Receive_Time?.ToString("HH:mm") ?? string.Empty,
                    Check_In = c.Check_In?.ToString("HH:mm") ?? string.Empty,
                    Check_Out = c.Check_Out?.ToString("HH:mm") ?? string.Empty,
                    Delivery_Status = Utils.GetStausDeliveryName(c.Delivery_Status ?? 0, lang),
                    Receipt_Status = Utils.GetStausReceiptName(c.Receipt_Status ?? 0, lang),
                    Reason = c.Reason ?? string.Empty,
                    Link = url + "?t=" + (c.Link ?? string.Empty),
                });
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    var header_vi = new List<object> { "STT", "Tên cửa hàng", "Mã phòng ban", "Tên của nhà cung cấp", "Tên lái xe", "Tên người nhận", "Ngày", "Thời gian nhận", "Check-in", "Check-out", "TT giao hàng", "TT đơn hàng", "Lý do", "Public Link" };
                    var header_en = new List<object> { "No", "Store", "Department Code", "Supplier Name", "Delivery Driver's Name", "Receiver's Name", "Date", "Received Time", "Check-in", "Check-out", "Delivery Status", "Receipt Status", "Reason", "Public Link" };
                    var index = 1;
                    Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#d6d6d6");
                    foreach (var item in lang == "vi" ? header_vi : header_en)
                    {
                        workSheet.Cells[1, index].Value = item;
                        workSheet.Cells[1, index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[1, index].Style.Fill.BackgroundColor.SetColor(colFromHex);
                        workSheet.Cells[1, index].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                        workSheet.Cells[1, index].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                        workSheet.Cells[1, index].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                        workSheet.Cells[1, index].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        index++;
                    }
                    workSheet.Cells["A2"].LoadFromCollection(model, false);
                    await package.SaveAsync();
                }
                stream.Position = 0;
                await Log(Domain.Enum.Menu.ORDER.ToString(), Domain.Enum.Action.EXPORT.ToString(), "Export false", Domain.Enum.LogStatus.SUCCESS.ToString());
                return stream ?? new MemoryStream();
            }
            catch
            {
                await Log(Domain.Enum.Menu.ORDER.ToString(), Domain.Enum.Action.EXPORT.ToString(), "Export false", Domain.Enum.LogStatus.FALSE.ToString());
                return new MemoryStream();
            }
        }

        public async Task<MemoryStream> ExportOrderRefuse(string lang)
        {
            try
            {
                var stream = new MemoryStream();
                var qStoreReference = request.Query["store_reference"].ToString() ?? "";
                var qLineReference = request.Query["line_reference"].ToString() ?? "";
                var qSupplierName = request.Query["supplier_name"].ToString() ?? "";
                var qDateFrom = request.Query["date_from"].ToString() ?? "";
                var qDateTo = request.Query["date_To"].ToString() ?? "";
                var qSupplierCode = request.Query["supplier_code"].ToString() ?? "";


                DateTime? dateFrom = null;
                DateTime? DateTo = null;
                if (!string.IsNullOrEmpty(qDateFrom))
                    dateFrom = DateTime.Parse(qDateFrom);
                if (!string.IsNullOrEmpty(qDateTo))
                    DateTo = DateTime.Parse(qDateTo).AddDays(1);

                Expression<Func<Order, bool>> query = (o => o.Status != (int)Domain.Enum.Status.Delete
                && (o.Delivery_Status == (int)Domain.Enum.DeliveryStatus.ReturnDelivery || o.Delivery_Status == (int)Domain.Enum.DeliveryStatus.PartiallyDelivered)
                && (string.IsNullOrEmpty(qStoreReference) || o.Store_Reference.ToString() == qStoreReference)
                && (string.IsNullOrEmpty(qLineReference) || o.Line_Reference.ToString() == qLineReference)
                && (string.IsNullOrEmpty(qSupplierName) || (o.Supplier_Name ?? "").ToLower().Contains(qSupplierName.ToLower()))
                && (string.IsNullOrEmpty(qSupplierCode) || (o.Supplier_Code ?? "").ToLower().Contains(qSupplierCode.ToLower()))
                && (string.IsNullOrEmpty(qDateFrom) || o.Delivery_Date >= dateFrom)
                && (string.IsNullOrEmpty(qDateTo) || o.Delivery_Date <= DateTo));

                Expression<Func<Order, object>> sort = (s => (object)s.CreatedAt);

                var model = _uom.Order.FindByCondition(query).ToList().Select((c, index) => new OrderRefuseExcelDTO
                {
                    No = index + 1,
                    Line_Code = c.Line_Code ?? string.Empty,
                    Supplier_Code = c.Supplier_Code ?? string.Empty,
                    Supplier_Name = c.Supplier_Name ?? string.Empty,
                    Store_Name = c.Store_Name ?? string.Empty,
                    Refuse_Date = c.Refuse_Date?.ToString("dd/MM/yyyy") ?? string.Empty,
                });
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    var header_vi = new List<object> { "STT", "Ngành hàng", "Mã của nhà cung cấp", "Tên của nhà cung cấp", "Cửa hàng", "Ngày" };
                    var header_en = new List<object> { "No", "Line Code", "Supplier No", "Supplier Name", "Store", "Date" };
                    var index = 1;
                    Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#d6d6d6");
                    foreach (var item in lang == "vi" ? header_vi : header_en)
                    {
                        workSheet.Cells[1, index].Value = item;
                        workSheet.Cells[1, index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[1, index].Style.Fill.BackgroundColor.SetColor(colFromHex);
                        workSheet.Cells[1, index].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                        workSheet.Cells[1, index].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                        workSheet.Cells[1, index].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                        workSheet.Cells[1, index].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        index++;
                    }
                    workSheet.Cells["A2"].LoadFromCollection(model, false);
                    await package.SaveAsync();
                }
                stream.Position = 0;
                await Log(Domain.Enum.Menu.ORDER.ToString(), Domain.Enum.Action.EXPORT.ToString(), "Export refuse false", Domain.Enum.LogStatus.SUCCESS.ToString());
                return stream ?? new MemoryStream();
            }
            catch
            {
                await Log(Domain.Enum.Menu.ORDER.ToString(), Domain.Enum.Action.EXPORT.ToString(), "Export refuse false", Domain.Enum.LogStatus.FALSE.ToString());
                return new MemoryStream();
            }
        }
    }
}
