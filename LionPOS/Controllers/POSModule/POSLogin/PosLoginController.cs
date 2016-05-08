using System.Web.Mvc;
using LionPOSServiceOperationLayer.Maintenance;
using LionPOSServiceContractModels;
using LionStartUp;
using LionStartUp.ControllerHelper;
using LionPOS.Models.ViewModels.Login.Login;

namespace LionPOS.Controllers.MaintenanceControllers
{
    public class POSLoginController : LoginBaseController
    {
        public ActionResult Index()
        {
            return View("~/Views/POSModule/POSLogin/Index.cshtml",new LoginViewModel());
        }
    }
}
