using LionPOSDbContracts.DomainModels.Branch;
using LionPOSDbContracts.DomainModels.Configuration;
using LionPOSServiceContractModels;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using LionPOSServiceContractModels.ErrorContactModel;
using LionPOSServiceOperationLayer.Maintenance;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace LionPOSServiceOperationLayer.SyncManager
{
    public class SyncBranchHOManager
    {
        public string POST(string url, string data)
        {
            //data = "{ User : \"user\" }";
            var postBody = "JsonParamString=" + data;

            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            //req.Proxy = new System.Net.WebProxy(ProxyString, true);
            //Add these, as we're doing a POST
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            //We need to count how many bytes we're sending. Post'ed Faked Forms should be name=value&
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(postBody);
            req.ContentLength = bytes.Length;
            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length); //Push it out there
            os.Close();
            System.Net.WebResponse resp = req.GetResponse();
            if (resp == null) return null;
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();

            //System.Net.WebRequest webRequest = System.Net.WebRequest.Create(url);
            //webRequest.Method = "POST";

            //webRequest.ContentType = "application/x-www-form-urlencoded";
            //webRequest.ContentLength = postBody.Length;
            //using (Stream stream = webRequest.GetRequestStream())
            //{
            //    byte[] content = ASCIIEncoding.ASCII.GetBytes(postBody);
            //    stream.Write(content, 0, content.Length);
            //}
            //System.Net.WebResponse resp = webRequest.GetResponse();
            //string responseData = string.Empty;
            //using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
            //{
            //    responseData = responseReader.ReadToEnd();
            //}
            //return responseData;

        }
        public string GET(string url, string data)
        {
            data = "{ User : \"user\" }";
            data = HttpUtility.UrlEncode(data);
            url = url + "?JsonParamString=" + data;
            //System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            ////req.Proxy = new System.Net.WebProxy(ProxyString, true); //true means no proxy
            //System.Net.WebResponse resp = req.GetResponse();
            //System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            //return sr.ReadToEnd().Trim();

            //data = HttpUtility.UrlEncode(data);
            //url = url + "?JsonParamString=" + data;
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";
            //webRequest.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse resp = (HttpWebResponse)webRequest.GetResponse();
            string responseData = string.Empty;
            using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
            {
                responseData = responseReader.ReadToEnd();
            }
            return responseData;
        }
        public void InitiateSync()
        {
            try
            {
                SyncNewBranches();
                SyncUpdateBranches();
                SyncDeleteBranches();
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog("",
                                                    "Error occured on creating PreConfigurationsServices of " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    "",
                                                    new LionUtilities.ConversionUtilitise().ObjectToString(ex),
                                                    null,
                                                    "",
                                                    this.GetType().Name,
                                                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName(),
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber(),
                                                    true,
                                                    "System"
                                                    );
            }
        }
        /// <summary>
        /// 1)Find new record that have not routed to cloud
        /// 2)Send those new record to cloud
        /// 3)Check Record is processed or some error occured.
        /// 4)if some error then throw it.
        /// 5)No error then edit new records insert route to cloud
        /// 6)Check if any new records from Cloud or not
        /// 7)if found then add current route node and add to database
        /// 8)Return final acknowledgement
        /// </summary>
        public void SyncNewBranches()
        {
            SyncRequestModel req = new SyncRequestModel();
            try
            {
                Guid g = Guid.NewGuid();
                req.senderSyncGUID = g.ToString();
                List<branch> branch;
                using (branchlvitsposdbEntities db = new DomainModels.UniversDbContext().branchDbContext(false, false))
                {
                    branch = db.branches.Where(a => !a.insertRoutePoint.Contains(ConstantDictionaryCM.centralCloudRouteNodeName)).ToList();
                    foreach (branch bra in branch)
                    {
                        bra.syncGUID = req.senderSyncGUID;
                    }
                    db.SaveChanges();
                }
                req.JsonParamString = JsonConvert.SerializeObject(branch);
                req.database = ConstantDictionaryCM.DbBranchLvitsPos.name;
                req.table = ConstantDictionaryCM.DbBranchLvitsPos.branches;
                req.SenderSyncNode = ConstantDictionaryCM.currentRouteNodeName;
                string response = POST(ConstantDictionaryCM.DbBranchLvitsPos.SyncAPI.NewBranchSync, JsonConvert.SerializeObject(req));
                SyncResponseModel ResultModel = JsonConvert.DeserializeObject<SyncResponseModel>(response);
                if (ResultModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultModel.errorLogId);
                }
                else
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        using (branchlvitsposdbEntities db = new DomainModels.UniversDbContext().branchDbContext(false, false))
                        {
                            string routeAdded = "|" + ConstantDictionaryCM.centralCloudRouteNodeName;
                            branch = db.branches.Where(a => !a.insertRoutePoint.Contains(ConstantDictionaryCM.centralCloudRouteNodeName) && a.syncGUID == ResultModel.receiverSyncGUID).ToList();
                            foreach (branch branc in branch)
                            {
                                branc.updateRoutePoint = branc.updateRoutePoint + routeAdded;
                            }
                            db.SaveChanges();
                            ts.Complete();
                        }
                    }

                    List<branch> branches = JsonConvert.DeserializeObject<List<branch>>(ResultModel.JsonParamString);
                    using (TransactionScope ts = new TransactionScope())
                    {
                        using (branchlvitsposdbEntities db = new DomainModels.UniversDbContext().branchDbContext(false, false))
                        {
                            string routeAdded = "|" + ConstantDictionaryCM.currentRouteNodeName;
                            foreach (branch bran in branches)
                            {
                                if (db.branches.Where(a => a.branchCode == bran.branchCode).Count() <= 0)
                                {
                                    bran.insertRoutePoint = bran.insertRoutePoint + routeAdded;
                                    db.branches.Add(bran);
                                }
                            }
                            db.SaveChanges();
                            ts.Complete();
                        }
                    }
                    req = new SyncRequestModel();
                    req.JsonParamString = JsonConvert.SerializeObject(branch);
                    req.database = ConstantDictionaryCM.DbBranchLvitsPos.name;
                    req.table = ConstantDictionaryCM.DbBranchLvitsPos.branches;
                    req.SenderSyncNode = ConstantDictionaryCM.currentRouteNodeName;
                    req.senderSyncGUID = ResultModel.receiverSyncGUID;
                    req.receiverSyncGUID = ResultModel.senderSyncGUID;
                    response = POST(ConstantDictionaryCM.DbBranchLvitsPos.SyncAPI.NewBranchSyncAcknowledge, JsonConvert.SerializeObject(req));
                    ResultModel = JsonConvert.DeserializeObject<SyncResponseModel>(response);
                    if (ResultModel.errorLogId > 0)
                    {
                        throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultModel.errorLogId);
                    }

                }
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog("",
                                                    "Error occured on creating PreConfigurationsServices of " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    "",
                                                    new LionUtilities.ConversionUtilitise().ObjectToString(ex),
                                                    null,
                                                    "",
                                                    this.GetType().Name,
                                                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName(),
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber(),
                                                    true,
                                                    "System"
                                                    );
            }
        }
        /// <summary>
        /// 1)Find update record that have not routed to cloud
        /// 2)Send those update record to cloud
        /// 3)Check Record is processed or some error occured.
        /// 4)if some error then throw it.
        /// 5)No error then edit update records update route to cloud
        /// 6)Check if any update records from Cloud or not and check those need to update or not based on last change date/
        /// 7)if found then add current route node and update to database
        /// 8)Return final acknowledgement
        /// </summary>
        public void SyncUpdateBranches()
        {
            SyncRequestModel req = new SyncRequestModel();
            try
            {
                Guid g = Guid.NewGuid();
                req.senderSyncGUID = g.ToString();
                List<branch> branch;
                using (branchlvitsposdbEntities db = new DomainModels.UniversDbContext().branchDbContext(false, false))
                {
                    branch = db.branches.Where(a => !a.updateRoutePoint.Contains(ConstantDictionaryCM.centralCloudRouteNodeName)).ToList();
                    foreach (branch bra in branch)
                    {
                        bra.syncGUID = req.senderSyncGUID;
                    }
                    db.SaveChanges();
                }
                req.JsonParamString = JsonConvert.SerializeObject(branch);
                req.database = ConstantDictionaryCM.DbBranchLvitsPos.name;
                req.table = ConstantDictionaryCM.DbBranchLvitsPos.branches;
                req.SenderSyncNode = ConstantDictionaryCM.currentRouteNodeName;
                string response = GET(ConstantDictionaryCM.DbBranchLvitsPos.SyncAPI.NewBranchSync, JsonConvert.SerializeObject(req));


                SyncResponseModel ResultModel = JsonConvert.DeserializeObject<SyncResponseModel>(response);
                if (ResultModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultModel.errorLogId);
                }
                else
                {

                    using (TransactionScope ts = new TransactionScope())
                    {
                        using (branchlvitsposdbEntities db = new DomainModels.UniversDbContext().branchDbContext(false, false))
                        {
                            string routeAdded = "|" + ConstantDictionaryCM.centralCloudRouteNodeName;
                            branch = db.branches.Where(a => !a.updateRoutePoint.Contains(ConstantDictionaryCM.centralCloudRouteNodeName) && a.syncGUID == ResultModel.receiverSyncGUID).ToList();
                            foreach (branch branc in branch)
                            {
                                branc.updateRoutePoint = branc.updateRoutePoint + routeAdded;
                            }
                            db.SaveChanges();
                            ts.Complete();
                        }
                    }


                    List<branch> branches = JsonConvert.DeserializeObject<List<branch>>(ResultModel.JsonParamString);
                    using (TransactionScope ts = new TransactionScope())
                    {
                        using (branchlvitsposdbEntities db = new DomainModels.UniversDbContext().branchDbContext(false, false))
                        {
                            string routeAdded = "|" + ConstantDictionaryCM.currentRouteNodeName;
                            foreach (branch branc in branches)
                            {
                                branch bran = db.branches.Where(a => a.branchCode == branc.branchCode && a.lastChangeDate < branc.lastChangeDate).SingleOrDefault();
                                if (bran != null)
                                {
                                    bran = branc;
                                    bran.updateRoutePoint = branc.updateRoutePoint + routeAdded;
                                    //db.Entry(branch).State = System.Data.Entity.EntityState.Modified;
                                }
                            }
                            db.SaveChanges();
                            ts.Complete();
                        }
                    }
                    req = new SyncRequestModel();
                    req.JsonParamString = JsonConvert.SerializeObject(branch);
                    req.database = ConstantDictionaryCM.DbBranchLvitsPos.name;
                    req.table = ConstantDictionaryCM.DbBranchLvitsPos.branches;
                    req.SenderSyncNode = ConstantDictionaryCM.currentRouteNodeName;
                    req.senderSyncGUID = ResultModel.receiverSyncGUID;
                    req.receiverSyncGUID = ResultModel.senderSyncGUID;
                    response = POST(ConstantDictionaryCM.DbBranchLvitsPos.SyncAPI.UpdateBranchSyncAcknowledge, JsonConvert.SerializeObject(req));
                    ResultModel = JsonConvert.DeserializeObject<SyncResponseModel>(response);
                    if (ResultModel.errorLogId > 0)
                    {
                        throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultModel.errorLogId);
                    }
                }
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog("",
                                                    "Error occured on creating PreConfigurationsServices of " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    "",
                                                    new LionUtilities.ConversionUtilitise().ObjectToString(ex),
                                                    null,
                                                    "",
                                                    this.GetType().Name,
                                                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName(),
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber(),
                                                    true,
                                                    "System"
                                                    );
            }
        }
        /// <summary>
        /// 1)Find delete record for perticular table from delete logs
        /// 2)Send those delete record to cloud
        /// 3)Check Record is processed or some error occured.
        /// 4)if some error then throw it.
        /// 5)No error then delete records from delete logs
        /// 6)Check if any delete logs from cloud need to process
        /// 7)if found then delete records
        /// 8)Return final acknowledgement
        /// </summary>
        public void SyncDeleteBranches()
        {
            SyncRequestModel req = new SyncRequestModel();
            try
            {
                Guid g = Guid.NewGuid();
                req.senderSyncGUID = g.ToString();
                List<delete_logs> branchlogs;
                using (configurationlvitsposdbEntities db = new DomainModels.UniversDbContext().configurationDbContext(false, false))
                {
                    branchlogs = db.delete_logs.Where(a =>
                    a.databaseName == ConstantDictionaryCM.DbBranchLvitsPos.name &&
                    a.tableName == ConstantDictionaryCM.DbBranchLvitsPos.branches).ToList();
                    foreach (delete_logs bra in branchlogs)
                    {
                        bra.syncGUID = req.senderSyncGUID;
                    }
                    db.SaveChanges();
                }
                req.JsonParamString = JsonConvert.SerializeObject(branchlogs);
                req.database = ConstantDictionaryCM.DbBranchLvitsPos.name;
                req.table = ConstantDictionaryCM.DbBranchLvitsPos.branches;
                req.SenderSyncNode = ConstantDictionaryCM.currentRouteNodeName;
                string response = POST(ConstantDictionaryCM.DbBranchLvitsPos.SyncAPI.NewBranchSync, JsonConvert.SerializeObject(req));

                SyncResponseModel ResultModel = JsonConvert.DeserializeObject<SyncResponseModel>(response);
                if (ResultModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultModel.errorLogId);
                }
                else
                {

                    using (TransactionScope ts = new TransactionScope())
                    {
                        using (configurationlvitsposdbEntities db = new DomainModels.UniversDbContext().configurationDbContext(false, false))
                        {
                            branchlogs = db.delete_logs.Where(a =>
                            a.databaseName == ConstantDictionaryCM.DbBranchLvitsPos.name &&
                            a.tableName == ConstantDictionaryCM.DbBranchLvitsPos.branches &&
                            a.syncGUID == ResultModel.receiverSyncGUID
                            ).ToList();
                            foreach (delete_logs branc in branchlogs)
                            {
                                db.delete_logs.Remove(branc);
                            }
                            db.SaveChanges();
                            ts.Complete();
                        }
                    }


                    List<delete_logs> brancheslogs = JsonConvert.DeserializeObject<List<delete_logs>>(ResultModel.JsonParamString);
                    using (TransactionScope ts = new TransactionScope())
                    {
                        using (branchlvitsposdbEntities db = new DomainModels.UniversDbContext().branchDbContext(false, false))
                        {
                            foreach (delete_logs branc in brancheslogs)
                            {
                                branch logBranch = JsonConvert.DeserializeObject<branch>(branc.deleteJSON);
                                branch deleteBranch = db.branches.Where(a => a.branchCode == logBranch.branchCode).SingleOrDefault();
                                if (deleteBranch != null)
                                {
                                    db.branches.Remove(deleteBranch);
                                }
                            }
                            db.SaveChanges();
                            ts.Complete();
                        }
                    }
                    req = new SyncRequestModel();
                    req.JsonParamString = JsonConvert.SerializeObject(brancheslogs);
                    req.database = ConstantDictionaryCM.DbBranchLvitsPos.name;
                    req.table = ConstantDictionaryCM.DbBranchLvitsPos.branches;
                    req.SenderSyncNode = ConstantDictionaryCM.currentRouteNodeName;
                    req.senderSyncGUID = ResultModel.receiverSyncGUID;
                    req.receiverSyncGUID = ResultModel.senderSyncGUID;
                    response = POST(ConstantDictionaryCM.DbBranchLvitsPos.SyncAPI.DeleteBranchSyncAcknowledge, JsonConvert.SerializeObject(req));
                    ResultModel = JsonConvert.DeserializeObject<SyncResponseModel>(response);
                    if (ResultModel.errorLogId > 0)
                    {
                        throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultModel.errorLogId);
                    }
                }
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog("",
                                                    "Error occured on creating PreConfigurationsServices of " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    "",
                                                    new LionUtilities.ConversionUtilitise().ObjectToString(ex),
                                                    null,
                                                    "",
                                                    this.GetType().Name,
                                                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName(),
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber(),
                                                    true,
                                                   "System"
                                                    );
            }

        }
    }
}
