using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LionPOSServiceOperationLayer.SyncManager;
namespace LionPOS.Controllers
{
    public class SyncHOtoCloudController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {

          
            return View();
        }

        public ActionResult InsertSyncTest()
        {
            SyncBranchHOManager homng = new SyncBranchHOManager();
            homng.SyncNewBranches();
            return RedirectToAction("Index");
        }
        public ActionResult UpdateSyncTest()
        {
            SyncBranchHOManager homng = new SyncBranchHOManager();
            homng.SyncUpdateBranches();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteSyncTest()
        {
            SyncBranchHOManager homng = new SyncBranchHOManager();
            homng.SyncDeleteBranches();
            return RedirectToAction("Index");
        }

    }
}