using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LionPOS.Models.ViewModels.Login.NeedHelp
{
    public class VerifyOTPViewModel
    {
        public string profilePicture { get; set; }
        public string name { get; set; }
        public string recoveryUsed { get; set; }
        public string verifyOTP { get; set; }
        public List<string> AlertList_string { get; set; }
        public string branchCode { get; set; }

    }
}