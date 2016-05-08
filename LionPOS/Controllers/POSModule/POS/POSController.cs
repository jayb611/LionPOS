using LionPOS.Models.ViewModels.Login.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LionPOS.Controllers.POSModule.POS
{
    public class POSController : Controller
    {
        // GET: POS
        public ActionResult Index()
        {
            return View("~/Views/POSModule/POS/Index.cshtml", new LoginViewModel());
        }
    }
}