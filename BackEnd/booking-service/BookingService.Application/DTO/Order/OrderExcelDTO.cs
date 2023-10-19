using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public class OrderExcelDTO
    {
        public int No { get; set; }
        public string? Store_Name { get; set; }
        public string? Department_Code { get; set; }
        public string? Supplier_Name { get; set; }
        public string? Driver_Name { get; set; }
        public string? Receiver_Name { get; set; }
        public string? Delivery_Regis_Date { get; set; }
        public string? Receive_Time { get; set; }
        public string? Check_In { get; set; }
        public string? Check_Out { get; set; }
        public string? Delivery_Status { get; set; }
        public string? Receipt_Status { get; set; }
        public string? Reason { get; set; }
        public string? Link { get; set; }
    }

    public class OrderRefuseExcelDTO
    {
        public int No { get; set; }
        public string? Line_Code { get; set; }
        public string? Supplier_Code { get; set; }
        public string? Supplier_Name { get; set; }
        public string? Store_Name { get; set; }
        public string? Refuse_Date { get; set; }
    }
}
