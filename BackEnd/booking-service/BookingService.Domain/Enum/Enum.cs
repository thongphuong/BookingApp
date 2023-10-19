using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Domain
{
    public class Enum
    {
        public enum Gender
        {
            Male = 'M',
            Female = 'F'
        }

        public enum Division
        {
            Store = '1',
            Office = '2'
        }

        public enum Status
        {
            Active = 1,
            Inactive = 0,
            Delete = 2
        }
        public enum QR_Status
        {
            CheckIn = 1,
            CheckOut = 2,
        }

        public enum DeliveryStatus
        {
            FullDelivered = 1,
            PartiallyDelivered = 2,
            NotDelivery = 3,
            ReturnDelivery = 4,
        }

        public enum ReceiptStatus
        {
            FullReceipt = 1,
            MissedReceipt = 2,
            ReturedReceipt = 3,
        }
        public enum Refuse
        {
            Full = 0,
            Apart = 1
        }

        public enum Menu
        {
            ORDER,
            RETURN,
        }

        public enum Action
        {
            ADD,
            EDIT,
            DELETED,
            CHANGEACTIVE,
            APPROVE,
            RETURN,
            REFUSE,
            LOGIN,
            LOGOUT,
            PRINT,
            EXPORT
        }

        public enum LogStatus
        {
            SUCCESS,
            FALSE
        }
    }
}
