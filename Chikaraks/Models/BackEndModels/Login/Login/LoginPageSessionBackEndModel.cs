using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chikaraks.Models.BackEndModels.Login.Login
{

    public class LoginPageSessionBackEndModel
    {
        public string username { get; set; }
        public int requestCount { get; set; }
        public string captchaString { get; set; }
        public bool isCaptchaActive { get; set; }
        public bool captch { get; set; }
        public bool block { get; set; }
        public int captachCount { get; set; }
        public int blockCount { get; set; }
        //public string branchCode { get; set; }

    }
}