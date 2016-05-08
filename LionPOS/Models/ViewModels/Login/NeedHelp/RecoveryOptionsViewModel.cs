using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LionPOS.Models.ViewModels.Login.NeedHelp
{
    public class RecoveryOptionsViewModel
    {
        public List<string> AlertList_string { get; set; }
        public string RecoveryOptions { get; set; }
        public string Username { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string email { get; set; }
        public string branchCode { get; set; }
    }
}