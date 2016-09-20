using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chikaraks.Models.ViewModels.Login.NeedHelp
{
    public class AccountProfileViewModel
    {
        public string profilePicture { get; set; }
        public string name { get; set; }
        public string emailAddress { get; set; }
        public string recoveryMode { get; set; }
        public string contactNo { get; set; }
        public string branchCode { get; set; }
    }
}