using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public class QRCodeDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("store_reference")]
        public System.Guid Store_Reference { get; set; }

        [JsonPropertyName("supplier_reference")]
        public Guid Supplier_Reference { get; set; }

        [JsonPropertyName("supplier_code")]
        public string? Supplier_Code { get; set; }

        [JsonPropertyName("supplier_name")]
        public string? Supplier_Name { get; set; }

        [JsonPropertyName("supplier_email")]
        public string? Supplier_Email { get; set; }

        [JsonPropertyName("supplier_phone")]

        public string? Supplier_PhoneNumber { get; set; }

        [JsonPropertyName("line_reference")]
        public System.Guid Line_Reference { get; set; }

        [JsonPropertyName("department_reference")]
        public System.Guid Department_Reference { get; set; }

        [JsonPropertyName("delivery_date")]
        public DateTime? Delivery_Date { get; set; }


        [JsonPropertyName("delivery_status")]
        public int? Delivery_Status { get; set; }

        [JsonPropertyName("receipt_status")]
        public int? Receipt_Status { get; set; }

        [JsonPropertyName("delivery_order_quantity")]
        public int? Delivery_Order_Quantity { get; set; }

        [JsonPropertyName("vehicle_name")]
        public string? Vehicle_Name { get; set; }

        [JsonPropertyName("license_plate")]
        public string? Lisense_Plate { get; set; }

        [JsonPropertyName("driver_name")]
        public string? Driver_Name { get; set; }

        [JsonPropertyName("cmnd")]
        public string? CMND { get; set; }

        [JsonPropertyName("emergn_email")]
        public string? Emergn_Email { get; set; }


        [JsonPropertyName("emergn_phone")]

        public string? Emergn_Phone { get; set; }


        [JsonPropertyName("delivery_item_quantity")]
        public int? Delivery_Item_Quantity { get; set; }

        [JsonPropertyName("delivery_regis_date")]
        public DateTime? Delivery_Regis_Date { get; set; }

        [JsonPropertyName("registime_reference")]
        public System.Guid RegisTime_Reference { get; set; }

        [JsonPropertyName("invoice_number")]
        public int? Invoice_Number { get; set; }

        [JsonPropertyName("miss_invoice_number")]
        public int? Miss_Invoice_Number { get; set; }

        [JsonPropertyName("delivery_invoice_date")]
        public DateTime? Delivery_Invoice_Date { get; set; }

        [JsonPropertyName("receiver_name")]
        public string? Receiver_Name { get; set; }


        [JsonPropertyName("receiver_time")]

        public DateTime? Receive_Time { get; set; }

        [JsonPropertyName("check_in")]
        public DateTime? Check_In { get; set; }

        [JsonPropertyName("check_out")]
        public DateTime? Check_Out { get; set; }

        [JsonPropertyName("reason")]
        public string? Reason { get; set; }

        [JsonPropertyName("link")]
        public string? Link { get; set; }

        [JsonPropertyName("qr_code")]
        public string? QR_Code { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("qr_status")]

        public int QR_Status { get; set; }

        [JsonPropertyName("user")]

        public int? User { get;set; }

    }
}
