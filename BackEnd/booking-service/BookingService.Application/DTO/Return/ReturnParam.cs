using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public class ReturnParam
    {
        public ReturnModel? ReturnModel { get; set; }

        public List<ReturnDetailModel>? ReturnDetailModelCreate { get; set; }
        public List<ReturnDetailModel>? ReturnDetailModelUpdate { get; set; }

        public List<ReturnDetailModel>? ReturnDetailModelDelete { get; set; }

    }  
}
