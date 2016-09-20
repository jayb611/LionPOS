using System;
using System.Linq;
using DomainModels;
using ChikaraksDbContracts.DomainModels;
using ChikaraksServiceContractModels;
using ChikaraksServiceContractModels.ConstantDictionaryContractModel;

namespace ChikaraksServiceOperationLayer.Maintenance
{
    public class MaintenanceServices
    {


        public int CreateLog(string identityCode, string title, string description, string ex, object objects, string events, string className, string methodName, string fileName, int lineNumber, bool isError = false, string Username = "System", string route = "Cloud")
        {
            try
            {
                errorlog log;
                using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext(false, false))
                {
                    log = new errorlog
                    {

                        exception = ex,
                        objectAttachment = "",
                        eventJson = events,
                        className = className,
                        methodName = methodName,
                        fileName = fileName,
                        lineNumber = lineNumber,
                        isError = isError,
                        lastChangeDate = DateTime.Now,
                        recordBySystem = true,
                        title = title,
                        description = description,
                        isActive = true,
                        isDeleted = false,
                        entryDate = DateTime.Now,
                        entryByUserName = Username,
                        changeByUserName = Username,
                        errorURL = "",
                        insertRoutePoint = route
                    };
                    db.errorlogs.Add(log);
                    db.SaveChanges();
                    log.errorURL = (ConstantDictionaryCM.MaintenanceWebsiteURL_string).Replace("<logid>", log.idlogs.ToString());
                    db.SaveChanges();
                }
                return log.idlogs;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public logContractModel getLog(int idlog)
        {
            try
            {
                using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext())
                {
                    return db.errorlogs.Where(log => idlog == log.idlogs).Select(a =>
                    new logContractModel()
                    {
                        className = a.className,
                        description = a.description,
                        entryDate = a.entryDate,
                        errorURL = a.errorURL,

                        eventJson = a.eventJson,
                        exception = a.exception,
                        fileName = a.fileName,
                        changeByUserName = a.changeByUserName,
                        entryByUserName = a.entryByUserName,
                        idlogs = a.idlogs,
                        isActive = a.isActive,
                        isDeleted = a.isDeleted,
                        isError = a.isError,
                        lastChangeDate = a.lastChangeDate,
                        lineNumber = a.lineNumber,
                        methodName = a.methodName,
                        objectAttachment = a.objectAttachment,
                        recordByLion = a.recordBySystem,
                        title = a.title
                    }
                    ).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                return new logContractModel();
            }
        }
    }
}

