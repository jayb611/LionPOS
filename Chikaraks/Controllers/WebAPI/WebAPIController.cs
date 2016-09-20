using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chikaraks.Controllers.WebAPI
{
    public class WebAPIController : Controller
    {
        // GET: WebAPI
        public ActionResult Index()
        {
            return View();
        }
    }
}