using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Domain
{
    public class Enum
    {
        public enum Gender
        {
            Male = 'M',
            Female = 'F'
        }

        public enum Status
        {
            Active = 1,
            Inactive = 0,
            Delete = 2
        }

        public enum Menu
        {
            USER,
            ROLE,
            STORE,
            SYSTEMPARAMETER,
            TIMEFRAME
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
