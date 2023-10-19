using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Service
{
    public class ResponseMessage<T> where T : class
    {
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public T value { get; set; }
        public ResponseMessage(string msg, HttpStatusCode status, T value)
        {
            this.Message = msg;
            this.StatusCode = status;
            this.value = value;
        }
    }
}
