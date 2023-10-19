using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UserService.Domain;

namespace UserService.Service.DTO.Return
{
    public class ReturnParam
    {
        public ReturnModel? ReturnModel { get; set; }

        public List<ReturnDetailModel>? ReturnDetailModel { get; set; }

    }  
}
