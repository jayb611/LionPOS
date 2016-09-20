using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chikaraks.Models.WebAPIModels
{
    public class BaseResponse
    {
        public string Error { get; set; }
        public string responseResult { get; set; }
        public string HttpResponse { get; set; }
    }
}