using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LionPOS.Models.ViewModels.Maintenance
{
    public class MaintenanceViewModel
    {
        public int logid { get; set; }
        public string TypeOfPage { get; set; }

        public MaintenanceViewModel(int logid)
        {
            this.logid = logid;
            this.TypeOfPage = TypeOfPage;
        }
      
    }
}