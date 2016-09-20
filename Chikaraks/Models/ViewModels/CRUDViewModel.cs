using ChikaraksServiceContractModels.ErrorContactModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chikaraks.Models.ViewModels
{
    public class CRUDViewModel : ErrorCM
    {
        public string crudOprationType { get; set; } // Insert,Update,Delete
        public bool canInsert { get; set; }
        public bool canUpdate { get; set; }
        public bool canDelete { get; set; }
        public string controllerName { get; set; }
        public string SubmitActionName { get; set; }
        public string InsertActionName { get; set; }
        public string UpdateActionName { get; set; }
        public string DeleteActionName { get; set; }
        public string DetailsActionName { get; set; }

        public ControllerContext ControllerContext { get; set; }
    }
}