using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LionPOS.Models.ViewModels.Login.NeedHelp
{
    public class RecoveryViewModel
    {
        public List<string> listUsers { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
        public string branchCode { get; set; }
        public string username { get; set; }
        public List<string> AlertList_string { get; set; }
    }
}