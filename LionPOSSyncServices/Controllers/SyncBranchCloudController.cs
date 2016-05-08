using LionPOSServiceContractModels;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using LionPOSServiceContractModels.ErrorContactModel;
using LionPOSSyncServices.Maintenance;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Http;
using LionPOSDbContracts.DomainModels.Branch;
using System.Transactions;
using System.Linq;
using LionPOSDbContracts.DomainModels.Configuration;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace LionPOSSyncServices.Controllers
{
    public class SyncBranchCloudController : ApiController
    {
        public SyncBranchCloudController()
        {

        }
        [System.Web.Http.HttpGet]
        public string test()
        {
            return "runnig";
        }
        /// <summary>
        /// 1)Get New records to Add from sneder node
        /// 2)Add Current Cloude Node to new records
        /// 3)Add those records
        /// 4)Return new records in which senderNode is not in insert route Node.
        /// 5)in between any error then send error logid in responds
        /// </summary>
        /// <param name="JsonParamString"></param>
        /// <returns>string</returns>
        [System.Web.Http.HttpPost]
        public string NewBranches(FormDataCollection value)
        {
           string JsonParamString = value.Get("JsonParamString");
            SyncResponseModel ResultModel = new SyncResponseModel();
            try
            {
                Guid g = Guid.NewGuid();
                ResultModel.senderSyncGUID = g.ToString();
                SyncRequestModel RequesModel = new SyncRequestModel();
                RequesModel = JsonConvert.DeserializeObject<SyncRequestModel>(JsonParamString);
                ResultModel.receiverSyncGUID = RequesModel.senderSyncGUID;
                List<branch> branches = JsonConvert.DeserializeObject<List<branch>>(RequesModel.JsonParamString);
                using (TransactionScope ts = new TransactionScope())
                {
                    using (branchlvitsposdbEntities db = new DomainModels.UniversDbContext().branchDbContext(false, false))
                    {
                        string routeAdded = "|" + ConstantDictionaryCM.centralCloudRouteNodeName;
                        foreach (branch branch in branches)
                        {
                            if (db.branches.Where(a => a.branchCode == branch.branchCode).Count() <= 0)
                            {
                                branch.insertRoutePoint = branch.insertRoutePoint + routeAdded;
                                db.branches.Add(branch);
                            }
                        }
                        db.SaveChanges();
                        ts.Complete();
                    }
                }

                branches = new List<branch>();
                using (branchlvitsposdbEntities db = new DomainModels.UniversDbContext().branchDbContext(false, false))
                {
                    branches = db.branches.Where(a => !a.insertRoutePoint.Contains(RequesModel.SenderSyncNode)).ToList();
                    foreach (branch bra in branches)
                    {
                        bra.syncGUID = ResultModel.senderSyncGUID;
                    }
                    db.SaveChanges();
                }
                ResultModel.JsonParamString = JsonConvert.SerializeObject(branches);
                return JsonConvert.SerializeObject(ResultModel);
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                                                    "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    ConstantDictionaryCM.ErrorMessageForUser,
                                                    new LionUtilities.ConversionUtilitise().ObjectToString(ex),
                                                    null,
                                                    "",
                                                    this.GetType().Name,
                                                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName(),
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber(),
                                                    true,
                                                    "System",
                                                    ConstantDictionaryCM.SyncRoutesTypes.Cloud
                                                    );

                ResultModel.errorLogId = logid;
                return JsonConvert.SerializeObject(ResultModel);
            }
        }

        /// <summary>
        /// 1)Receive Acknowledgement from sender and add his routes to records
        /// </summary>
        /// <param name="JsonParamString"></param>
        /// <returns></returns>
        public string NewBranchesAcknowledge(FormDataCollection value)
        {
            string JsonParamString = value.Get("JsonParamString");
            try
            {
                SyncRequestModel RequesModel = new SyncRequestModel();
                RequesModel = JsonConvert.DeserializeObject<SyncRequestModel>(JsonParamString);
                using (branchlvitsposdbEntities db = new DomainModels.UniversDbContext().branchDbContext(false, false))
                {
                    List<branch> branches = db.branches.Where(a => !a.insertRoutePoint.Contains(RequesModel.SenderSyncNode) && a.syncGUID == RequesModel.receiverSyncGUID).ToList();
                    string routeAdded = "|" + RequesModel.senderSyncGUID;
                    foreach (branch bra in branches)
                    {
                        bra.insertRoutePoint = bra.insertRoutePoint + routeAdded;
                    }
                    db.SaveChanges();
                }
                return JsonConvert.SerializeObject(new ErrorCM() { errorLogId = 0 });
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                                                    "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    ConstantDictionaryCM.ErrorMessageForUser,
                                                    new LionUtilities.ConversionUtilitise().ObjectToString(ex),
                                                    null,
                                                    "",
                                                    this.GetType().Name,
                                                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName(),
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber(),
                                                    true,
                                                    "System",
                                                    ConstantDictionaryCM.SyncRoutesTypes.Cloud
                                                    );

                return JsonConvert.SerializeObject(new ErrorCM() { errorLogId = logid });
            }
        }

        /// <summary>
        /// 1)Get updated records to update,from sneder node
        /// 2)Add Current Cloude Node to update records
        /// 3)Add those records
        /// 4)Return update records in which senderNode is not in update route Node.
        /// 5)in between any error then send error logid in responds
        /// </summary>
        /// <param name="JsonParamString"></param>
        /// <returns>string</returns>
        public string UpdateBranches(FormDataCollection value)
        {
            string JsonParamString = value.Get("JsonParamString");
            SyncResponseModel ResultModel = new SyncResponseModel();
            try
            {
                Guid g = Guid.NewGuid();
                ResultModel.senderSyncGUID = g.ToString();
                SyncRequestModel RequesModel = new SyncRequestModel();
                RequesModel = JsonConvert.DeserializeObject<SyncRequestModel>(JsonParamString);
                ResultModel.receiverSyncGUID = RequesModel.senderSyncGUID;
                List<branch> branches = JsonConvert.DeserializeObject<List<branch>>(RequesModel.JsonParamString);
                using (TransactionScope ts = new TransactionScope())
                {
                    using (branchlvitsposdbEntities db = new DomainModels.UniversDbContext().branchDbContext(false, false))
                    {
                        string routeAdded = "|" + ConstantDictionaryCM.centralCloudRouteNodeName;
                        foreach (branch branch in branches)
                        {
                            branch bran = db.branches.Where(a => a.branchCode == branch.branchCode && a.lastChangeDate < branch.lastChangeDate).SingleOrDefault();
                            string InsertRouteNodes = bran.insertRoutePoint;
                            if (bran != null)
                            {
                                bran = branch;
                                bran.insertRoutePoint = InsertRouteNodes;
                                bran.updateRoutePoint = branch.updateRoutePoint + routeAdded;
                                //db.Entry(branch).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                        db.SaveChanges();
                        ts.Complete();
                    }
                }

                branches = new List<branch>();
                using (branchlvitsposdbEntities db = new DomainModels.UniversDbContext().branchDbContext(false, false))
                {
                    branches = db.branches.Where(a => !a.updateRoutePoint.Contains(RequesModel.SenderSyncNode)).ToList();
                    foreach (branch bra in branches)
                    {
                        bra.syncGUID = ResultModel.senderSyncGUID;
                    }
                    db.SaveChanges();
                }
                ResultModel.JsonParamString = JsonConvert.SerializeObject(branches);
                return JsonConvert.SerializeObject(ResultModel);
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                                                    "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    ConstantDictionaryCM.ErrorMessageForUser,
                                                    new LionUtilities.ConversionUtilitise().ObjectToString(ex),
                                                    null,
                                                    "",
                                                    this.GetType().Name,
                                                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName(),
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber(),
                                                    true,
                                                    "System",
                                                    ConstantDictionaryCM.SyncRoutesTypes.Cloud
                                                    );

                ResultModel.errorLogId = logid;
                return JsonConvert.SerializeObject(ResultModel);
            }
        }

        /// <summary>
        /// 1)Receive Acknowledgement from sender and add his routes to records
        /// </summary>
        /// <param name="JsonParamString"></param>
        /// <returns></returns>
        public string UpdateBranchesAcknowledge(FormDataCollection value)
        {
            string JsonParamString = value.Get("JsonParamString");
            try
            {
                SyncRequestModel RequesModel = new SyncRequestModel();
                RequesModel = JsonConvert.DeserializeObject<SyncRequestModel>(JsonParamString);
                using (branchlvitsposdbEntities db = new DomainModels.UniversDbContext().branchDbContext(false, false))
                {
                    List<branch> branches = db.branches.Where(a => !a.updateRoutePoint.Contains(RequesModel.SenderSyncNode) && a.syncGUID == RequesModel.receiverSyncGUID).ToList();
                    string routeAdded = "|" + RequesModel.senderSyncGUID;
                    foreach (branch bra in branches)
                    {
                        bra.updateRoutePoint = bra.updateRoutePoint + routeAdded;
                    }
                    db.SaveChanges();
                }
                return JsonConvert.SerializeObject(new ErrorCM() { errorLogId = 0 });
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                                                    "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    ConstantDictionaryCM.ErrorMessageForUser,
                                                    new LionUtilities.ConversionUtilitise().ObjectToString(ex),
                                                    null,
                                                    "",
                                                    this.GetType().Name,
                                                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName(),
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber(),
                                                    true,
                                                    "System",
                                                    ConstantDictionaryCM.SyncRoutesTypes.Cloud
                                                    );

                return JsonConvert.SerializeObject(new ErrorCM() { errorLogId = logid });
            }
        }

        /// <summary>
        /// 1)Get delete logs with table name ,from sneder node
        /// 2)add delete logs to cloud delete logs if not found with cloud route node
        /// 3)delete records if found 
        /// 4)return deletelogs record in which sender node is not in delete log route node
        /// 5)in between any error then send error logid in responds
        /// </summary>
        /// <param name="JsonParamString"></param>
        /// <returns>string</returns>
        public string DeleteBranches(FormDataCollection value)
        {
            string JsonParamString = value.Get("JsonParamString");
            SyncResponseModel ResultModel = new SyncResponseModel();
            try
            {
                Guid g = Guid.NewGuid();
                ResultModel.senderSyncGUID = g.ToString();
                SyncRequestModel RequesModel = new SyncRequestModel();
                RequesModel = JsonConvert.DeserializeObject<SyncRequestModel>(JsonParamString);
                ResultModel.receiverSyncGUID = RequesModel.senderSyncGUID;
                List<delete_logs> senderDeleteLogs = JsonConvert.DeserializeObject<List<delete_logs>>(RequesModel.JsonParamString);
                using (TransactionScope ts = new TransactionScope())
                {
                    using (branchlvitsposdbEntities db = new DomainModels.UniversDbContext().branchDbContext(false, false))
                    {
                        string routeAdded = "|" + ConstantDictionaryCM.centralCloudRouteNodeName;
                        foreach (delete_logs senderDeleteLog in senderDeleteLogs)
                        {
                            branch keyBranch = JsonConvert.DeserializeObject<branch>(senderDeleteLog.deleteJSON);
                            branch branch = db.branches.Where(a => a.branchCode == keyBranch.branchCode).SingleOrDefault();
                            if (branch != null)
                            {
                                db.branches.Remove(branch);
                            }
                        }
                        db.SaveChanges();
                        ts.Complete();
                    }
                }
                using (TransactionScope ts = new TransactionScope())
                {
                    using (configurationlvitsposdbEntities db = new DomainModels.UniversDbContext().configurationDbContext(false, false))
                    {
                        string routeAdded = "|" + ConstantDictionaryCM.centralCloudRouteNodeName;
                        foreach (delete_logs senderDeleteLog in senderDeleteLogs)
                        {

                            delete_logs log = db.delete_logs.Where(a => a.keys == senderDeleteLog.keys && a.databaseName == senderDeleteLog.databaseName && a.tableName == senderDeleteLog.tableName).SingleOrDefault();
                            if (log != null)
                            {
                                log.deleteRoutePoint = log.deleteRoutePoint + routeAdded;
                                db.delete_logs.Add(log);
                            }
                        }
                        db.SaveChanges();
                        ts.Complete();
                    }
                }

                List<delete_logs> delete_logs = new List<delete_logs>();
                using (configurationlvitsposdbEntities db = new DomainModels.UniversDbContext().configurationDbContext(false, false))
                {
                    delete_logs = db.delete_logs.Where(a => !a.deleteRoutePoint.Contains(RequesModel.SenderSyncNode)).ToList();
                    foreach (delete_logs log in delete_logs)
                    {
                        log.syncGUID = ResultModel.senderSyncGUID;
                    }
                    db.SaveChanges();
                }
                ResultModel.JsonParamString = JsonConvert.SerializeObject(delete_logs);
                return JsonConvert.SerializeObject(ResultModel);


            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                                                    "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    ConstantDictionaryCM.ErrorMessageForUser,
                                                    new LionUtilities.ConversionUtilitise().ObjectToString(ex),
                                                    null,
                                                    "",
                                                    this.GetType().Name,
                                                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName(),
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber(),
                                                    true,
                                                    "System",
                                                    ConstantDictionaryCM.SyncRoutesTypes.Cloud
                                                    );

                ResultModel.errorLogId = logid;
                return JsonConvert.SerializeObject(ResultModel);
            }
        }

        /// <summary>
        /// 1)Receive Acknowledgement from sender and add his routes to records
        /// </summary>
        /// <param name="JsonParamString"></param>
        /// <returns></returns>
        public string DeleteBranchesAcknowledge(FormDataCollection value)
        {
            string JsonParamString = value.Get("JsonParamString");
            try
            {
                SyncRequestModel RequesModel = new SyncRequestModel();
                RequesModel = JsonConvert.DeserializeObject<SyncRequestModel>(JsonParamString);
                List<string> branchCode;
                using (TransactionScope ts = new TransactionScope())
                {
                    using (branchlvitsposdbEntities db = new DomainModels.UniversDbContext().branchDbContext(false, false))
                    {
                        branchCode = db.branches.Where(a => a.isActive == true).Select(a => a.branchCode).ToList();
                    }
                }
                using (configurationlvitsposdbEntities db = new DomainModels.UniversDbContext().configurationDbContext(false, false))
                {
                    List<delete_logs> temp = db.delete_logs.Where(a => a.databaseName == RequesModel.database && a.tableName == RequesModel.table && a.syncGUID == RequesModel.receiverSyncGUID).ToList();
                    for (int i = 0; i < temp.Count(); i++)
                    {
                        int count = 0;
                        foreach (string brancha in branchCode)
                        {
                            if (temp[i].deleteRoutePoint.Contains(brancha))
                            {
                                count++;
                            }
                        }
                        if (count == branchCode.Count())
                        {
                            db.delete_logs.Remove(temp[i]);
                        }

                    }
                }
                return JsonConvert.SerializeObject(new ErrorCM() { errorLogId = 0 });
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                                                    "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    ConstantDictionaryCM.ErrorMessageForUser,
                                                    new LionUtilities.ConversionUtilitise().ObjectToString(ex),
                                                    null,
                                                    "",
                                                    this.GetType().Name,
                                                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName(),
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber(),
                                                    true,
                                                    "System",
                                                    ConstantDictionaryCM.SyncRoutesTypes.Cloud
                                                    );

                return JsonConvert.SerializeObject(new ErrorCM() { errorLogId = logid });
            }
        }
    }
}
