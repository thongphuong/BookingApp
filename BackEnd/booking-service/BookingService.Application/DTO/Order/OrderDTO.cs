using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public class OrderDTO
    {
        [JsonPropertyName("reference_id")]
        public System.Guid ReferenceId { get; set; }

        [JsonPropertyName("store_reference")]
        public System.Guid Store_Reference { get; set; }

        [JsonPropertyName("store_code")]
        public int? Store_Code { get; set; }

        [JsonPropertyName("store_name")]
        public string? Store_Name { get; set; }

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

        [JsonPropertyName("line_code")]
        public string? Line_Code { get; set; }

        [JsonPropertyName("line_name")]
        public string? Line_Name { get; set; }

        [JsonPropertyName("department_reference")]
        public System.Guid Department_Reference { get; set; }

        [JsonPropertyName("department_code")]
        public string? Department_Code { get; set; }

        [JsonPropertyName("department_name")]
        public string? Department_Name { get; set; }

        [JsonPropertyName("delivery_date")]
        public DateTime? Delivery_Date { get; set; }

        [JsonPropertyName("delivery_status_name")]
        public string? Delivery_Status_Name { get; set; }

        [JsonPropertyName("delivery_status_value")]
        public int? Delivery_Status_Value { get; set; }

        [JsonPropertyName("receipt_status_name")]
        public string? Receipt_Status_Name { get; set; }
        [JsonPropertyName("receipt_status_value")]
        public int? Receipt_Status_Value { get; set; }

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

        [JsonPropertyName("registime_from")]
        public string? register_from { get; set; }

        [JsonPropertyName("registime_to")]
        public string? register_to { get; set; }

        [JsonPropertyName("invoice_number")]
        public int? Invoice_Number { get; set; }

        [JsonPropertyName("delivery_invoice_date")]
        public DateTime? Delivery_Invoice_Date { get; set; }

        [JsonPropertyName("miss_invoice_number")]
        public int? Miss_Invoice_Number { get; set; }

        [JsonPropertyName("receiver_name")]
        public string? Receiver_Name { get; set; }

        [JsonPropertyName("receiver_department")]
        public string? Receiver_Department { get; set; }

        [JsonPropertyName("refuse_status")]
        public int? Refuse_Status { get; set; }

        [JsonPropertyName("refuse_date")]
        public DateTime? Refuse_Date { get; set; }

        [JsonPropertyName("reason")]
        public string? Reason { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }

        [JsonPropertyName("order_receipt")]
        public List<OrderReceiptDTO> orderReceiptDTOs { get; set; } = new List<OrderReceiptDTO>();

        [JsonPropertyName("order_delivery")]
        public List<OrderDeliveryDTO> orderDeliveryDTOs { get; set; } = new List<OrderDeliveryDTO>();

        [JsonPropertyName("order_product")]
        public List<OrderProductDTO> orderProductDTOs { get; set; } = new List<OrderProductDTO>();
    }

    public class OrderRegisterResponse
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }
        [JsonPropertyName("qr_image")]
        public string? QRImage { get; set; }
    }
}
