using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LionPOS.Models.ViewModels
{
    public class AlertViewModel
    {
        public string type { get; set; }
        public string messsage { get; set; }
        public string colour { get; set; }
        public bool canDismissable { get; set; }
    }
}