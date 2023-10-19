using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Domain
{
    [Table("ORDER")]
    public class Order : BaseEntity
    {
        [Column("store_reference")]
        public System.Guid Store_Reference { get; set; }

        [Column("store_code")]
        public int? Store_Code { get; set; }

        [Column("store_name")]
        public string? Store_Name { get; set; }

        [Required]
        [Column("supplier_reference")]
        public Guid Supplier_Reference { get; set; }

        [Required]
        [Column("supplier_code")]
        public string? Supplier_Code { get; set; }

        [Column("supplier_name")]
        public string? Supplier_Name { get; set; }

        [Column("supplier_email")]
        public string? Supplier_Email { get; set; }

        [Column("supplier_phone")]

        public string? Supplier_PhoneNumber { get; set; }

        [Required]
        [Column("line_reference")]
        public System.Guid Line_Reference { get; set; }

        [Required]
        [Column("line_code")]
        public string? Line_Code { get; set; }

        [Required]
        [Column("line_name")]
        public string? Line_Name { get; set; }

        [Required]
        [Column("department_reference")]
        public System.Guid Department_Reference { get; set; }

        [Required]
        [Column("department_code")]
        public string? Department_Code { get; set; }

        [Required]
        [Column("department_name")]
        public string? Department_Name { get; set; }

        [Column("delivery_date")]
        public DateTime? Delivery_Date { get; set; }

        [Column("delivery_status")]
        public int? Delivery_Status { get; set; }

        [Column("receipt_status")]
        public int? Receipt_Status { get; set; }

        [Required]
        [Column("delivery_order_quantity")]
        public int? Delivery_Order_Quantity { get; set; }

        [Column("vehicle_name")]
        public string? Vehicle_Name { get; set; }

        [Column("license_plate")]
        public string? Lisense_Plate { get; set; }

        [Column("driver_name")]
        public string? Driver_Name { get; set; }

        [Column("cmnd")]
        public string? CMND { get; set; }

        [Column("emergn_email")]
        public string? Emergn_Email { get; set; }

        [Column("emergn_phone")]
        public string? Emergn_Phone { get; set; }

        [Column("delivery_item_quantity")]
        public int? Delivery_Item_Quantity { get; set; }

        [Required]
        [Column("delivery_regis_date")]
        public DateTime? Delivery_Regis_Date { get; set; }

        [Required]
        [Column("registime_reference")]
        public System.Guid RegisTime_Reference { get; set; }

        [Required]
        [Column("registime_from")]
        public string? register_from { get; set; }

        [Required]
        [Column("registime_to")]
        public string? register_to { get; set; }

        [Required]
        [Column("invoice_number")]
        public int? Invoice_Number { get; set; }

        [Column("miss_invoice_number")]
        public int? Miss_Invoice_Number { get; set; }

        [Column("delivery_invoice_date")]
        public DateTime? Delivery_Invoice_Date { get; set; }

        [Column("receiver_name")]
        public string? Receiver_Name { get; set; }

        [Column("receiver_department")]
        public string? Receiver_Department { get; set; }

        [Column("receiver_time")]
        public DateTime? Receive_Time { get; set; }

        [Column("check_in")]
        public DateTime? Check_In { get; set; }

        [Column("check_out")]
        public DateTime? Check_Out { get; set; }

        [Column("reason")]
        public string? Reason { get; set; }

        [Column("link")]
        public string? Link { get; set; }

        [Column("qr_code")]
        public string? QR_Code { get; set; }

        [Column("code")]
        public string? Code { get; set; }

        [Column("qr_image")]
        public string? QrImage { get; set; }

        [Column("refuse_status")]
        public int? Refuse_Status { get; set; }

        [Column("refuse_date")]
        public DateTime? Refuse_Date { get; set; }

        [Column("image")]
        public string? Image { get; set; }
    }
}
