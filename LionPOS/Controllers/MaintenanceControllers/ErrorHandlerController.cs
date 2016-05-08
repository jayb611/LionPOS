using System.Web.Mvc;
using LionPOSServiceOperationLayer.Maintenance;
using LionPOSServiceContractModels;

namespace LionPOS.Controllers.MaintenanceControllers
{
    public class ErrorHandlerController : Controller
    {
        //
        // GET: /ErrorHandler/

        public ActionResult Index(int logid)
        {
            MaintenanceServices m = new MaintenanceServices();
            //Need to change data contract
            logContractModel log = m.getLog(logid);
            return View("~/Views/Maintenance/ErrorHandler/Index.cshtml", log);
        }
    }
}
