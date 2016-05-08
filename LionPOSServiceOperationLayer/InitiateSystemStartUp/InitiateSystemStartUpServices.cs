using System;
using System.Linq;

using DomainModels;
using LionUtilities.SecurityPkg;
using LionPOSDbContracts.DomainModels.User;
using LionPOSDbContracts.DomainModels.Branch;
using LionPOSDbContracts.DomainModels.Employee;
using LionPOSDbContracts.DomainModels.Configuration;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using LionPOSServiceOperationLayer.Maintenance;
using Newtonsoft.Json;
using LionPOSServiceContractModels.ErrorContactModel;

namespace LionPOSServiceOperationLayer.InitiateSystemStartUp
{
    //HC 25-02-2016 Created
    public class InitConstantRecordsDictionaryServices
    {
        branchlvitsposdbEntities branchDB = new UniversDbContext().branchDbContext();
        userlvitsposdbEntities userDB = new UniversDbContext().userDbContext();
        employeelvitsposdbEntities empDB = new UniversDbContext().employeeDbContext();
        configurationlvitsposdbEntities confDB = new UniversDbContext().configurationDbContext();
        public static string AESkey { get; set; }
        public static string AESEncPass { get; set; }
        public string branchCode { get; set; }
        public string Initiate()
        {
            try
            {
                // Warning :-   Please Dont Uncomment Following DeleteAllRecordFromAllTables() method unless You need to delete all records from the all databases
                DeleteAllRecordFromAllTalbes();
                this.branchCode = branchCode;
                AESAlgoritham aesEnc = new AESAlgoritham("123");
                AESkey = aesEnc.key;
                AESEncPass = aesEnc.encryptedPwd;
                AccessRole();
                AccessArea();
                AccessRoleInArea();
                Branches();
                employee();
                employee_underbranch();
                users();
                OverrideAccessRole();
                setting();
                return JsonConvert.SerializeObject(new ErrorCM() { errorLogId = 0 });
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
                return JsonConvert.SerializeObject(new ErrorCM() { errorLogId = logid });
            }
        }


        public void DeleteAllRecordFromAllTalbes()
        {

            branchlvitsposdbEntities dbbranch = new UniversDbContext().branchDbContext();
            employeelvitsposdbEntities dbemp = new UniversDbContext().employeeDbContext();
            userlvitsposdbEntities dbuser = new UniversDbContext().userDbContext();
            configurationlvitsposdbEntities dbconf = new UniversDbContext().configurationDbContext();

            using (userlvitsposdbEntities db = new UniversDbContext().userDbContext())
            {
                foreach (access_role_in_access_area a in db.access_role_in_access_area)
                {
                    db.access_role_in_access_area.Remove(a);
                }
                db.SaveChanges();
            }
            using (userlvitsposdbEntities db = new UniversDbContext().userDbContext())
            {
                foreach (override_access_role a in db.override_access_role)
                {
                    db.override_access_role.Remove(a);
                }
                db.SaveChanges();
            }

            using (userlvitsposdbEntities db = new UniversDbContext().userDbContext())
            {
                foreach (access_area a in db.access_area)
                {
                    db.access_area.Remove(a);
                }
                db.SaveChanges();
            }
            using (userlvitsposdbEntities db = new UniversDbContext().userDbContext())
            {
                foreach (user a in db.users)
                {
                    db.users.Remove(a);
                }
                db.SaveChanges();
            }

            using (userlvitsposdbEntities db = new UniversDbContext().userDbContext())
            {
                foreach (access_role_master a in db.access_role_master)
                {
                    db.access_role_master.Remove(a);
                }
                db.SaveChanges();
            }




            using (branchlvitsposdbEntities db = new UniversDbContext().branchDbContext())
            {
                foreach (employee_underbranch a in db.employee_underbranch)
                {
                    db.employee_underbranch.Remove(a);
                }
                db.SaveChanges();
            }


            using (employeelvitsposdbEntities db = new UniversDbContext().employeeDbContext())
            {
                foreach (employee_merge a in db.employee_merge)
                {
                    db.employee_merge.Remove(a);
                }
                db.SaveChanges();
            }

            using (employeelvitsposdbEntities db = new UniversDbContext().employeeDbContext())
            {
                foreach (employee a in db.employees)
                {
                    db.employees.Remove(a);
                }
                db.SaveChanges();
            }


            using (branchlvitsposdbEntities db = new UniversDbContext().branchDbContext())
            {
                foreach (branch a in db.branches)
                {
                    db.branches.Remove(a);
                }
                db.SaveChanges();
            }

            using (configurationlvitsposdbEntities db = new UniversDbContext().configurationDbContext())
            {
                foreach (setting a in db.settings)
                {
                    db.settings.Remove(a);
                }
                db.SaveChanges();
            }
        }

        public class access_role_master_Seeds
        {
            public static access_role_master AdminRole = new access_role_master
            {
                accessRoleTitle = ConstantDictionaryCM.LionVisionAccessRole_string,
                isActive = true,
                isDeleted = false,
                entryDate = DateTime.Now,
                lastChangeDate = DateTime.Now,
                recordByLion = true,
                entryByUserName = ConstantDictionaryCM.LionVisionBranchName_string,
                changeByUserName = ConstantDictionaryCM.LionVisionBranchName_string,
                entryBranchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
        }
        public void AccessRole()
        {

            var seed = userDB.access_role_master.ToList();
            var searchSeed = seed.Where(access_role_master => access_role_master.accessRoleTitle == access_role_master_Seeds.AdminRole.accessRoleTitle).SingleOrDefault();
            if (searchSeed == null)
            {
                userDB.access_role_master.Add(access_role_master_Seeds.AdminRole);
            }
            userDB.SaveChanges();


        }

        public class access_area_Seeds
        {
            public static access_area AdminArea = new access_area
            {
                domain = "::1",
                controller = "Home",
                view = "Index",
                visible = true,
                remarks = "Dashboard home page",
                isActive = true,
                isDeleted = false,
                entryDate = DateTime.Now,
                lastChangeDate = DateTime.Now,
                recordByLion = true,
                entryByUserName = ConstantDictionaryCM.LionVisionBranchName_string,
                changeByUserName = ConstantDictionaryCM.LionVisionBranchName_string,
                entryBranchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
        }
        public void AccessArea()
        {

            var seed = userDB.access_area.ToList();
            var searchSeed = seed.Where(access_area =>
            access_area.domain == access_area_Seeds.AdminArea.domain &&
            access_area.controller == access_area_Seeds.AdminArea.controller &&
            access_area.view == access_area_Seeds.AdminArea.view
            ).SingleOrDefault();
            if (searchSeed == null)
            {
                userDB.access_area.Add(access_area_Seeds.AdminArea);
            }
            userDB.SaveChanges();
        }

        public class override_access_role_Seeds
        {
            public static override_access_role OverrideAccessRole = new override_access_role
            {
                domain = "::1",
                controller = "Home",
                view = "Index",
                userName = ConstantDictionaryCM.LionVisionBranchName_string,
                userEntryGroupCode = ConstantDictionaryCM.ApplicationGroupCode,
                canAccess = true,
                visible = true,
                title = "Override Home Access for user " + ConstantDictionaryCM.LionVisionBranchName_string,
                permanent = true,
                overrideTill = DateTime.Now.Date,
                isActive = true,
                isDeleted = false,
                entryDate = DateTime.Now,
                lastChangeDate = DateTime.Now,
                recordByLion = true,
                entryByUserName = ConstantDictionaryCM.LionVisionBranchName_string,
                changeByUserName = ConstantDictionaryCM.LionVisionBranchName_string,
                userEntryBranchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,

            };
        }
        public void OverrideAccessRole()
        {

            var seed = userDB.override_access_role.ToList();
            var searchSeed = seed.Where(override_access_role =>
            override_access_role.domain == override_access_role_Seeds.OverrideAccessRole.domain &&
            override_access_role.controller == override_access_role_Seeds.OverrideAccessRole.controller &&
            override_access_role.view == override_access_role_Seeds.OverrideAccessRole.view &&
            override_access_role.userName == override_access_role_Seeds.OverrideAccessRole.userName &&
            override_access_role.userEntryBranchCode == override_access_role_Seeds.OverrideAccessRole.userEntryBranchCode &&
            override_access_role.userEntryGroupCode == override_access_role_Seeds.OverrideAccessRole.userEntryGroupCode).SingleOrDefault();
            if (searchSeed == null)
            {
                userDB.override_access_role.Add(override_access_role_Seeds.OverrideAccessRole);
            }
            userDB.SaveChanges();

        }

        public class access_role_in_access_area_Seeds
        {
            public static access_role_in_access_area AdminRoleInArea = new access_role_in_access_area
            {
                domain = "::1",
                controller = "Home",
                view = "Index",
                accessRoleTitle = access_role_master_Seeds.AdminRole.accessRoleTitle,
                canAccess = true,
                visible = true,
                remarks = "Access Role in Access Area Remarks",
                isActive = true,
                isDeleted = false,
                entryDate = DateTime.Now,
                lastChangeDate = DateTime.Now,
                recordByLion = true,
                entryByUserName = ConstantDictionaryCM.LionVisionBranchName_string,
                changeByUserName = ConstantDictionaryCM.LionVisionBranchName_string,
                entryBranchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
        }
        public void AccessRoleInArea()
        {

            var seed = userDB.access_role_in_access_area.ToList();
            var searchSeed = seed.Where(access_role_in_access_area =>
            access_role_in_access_area.accessRoleTitle == access_role_in_access_area_Seeds.AdminRoleInArea.accessRoleTitle &&
            access_role_in_access_area.domain == access_role_in_access_area_Seeds.AdminRoleInArea.domain &&
            access_role_in_access_area.controller == access_role_in_access_area_Seeds.AdminRoleInArea.controller &&
            access_role_in_access_area.view == access_role_in_access_area_Seeds.AdminRoleInArea.view
            ).SingleOrDefault();
            if (searchSeed == null)
            {
                userDB.access_role_in_access_area.Add(access_role_in_access_area_Seeds.AdminRoleInArea);
            }
            userDB.SaveChanges();

        }

        public class branches_Seeds
        {

            public static branch BranchLion = new branch
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                branchName = ConstantDictionaryCM.LionVisionBranchName_string,
                description = "IT Company, Software Development and Web Development",
                address = "330,Khokhra Circle,Opp. Rokadiya Hanuman Temple, Ahmedabad",
                CoveringAreas = "India",
                logitudeLocation = "72.6191921",
                latitudeLoaction = "23.0022559",
                branchType = ConstantDictionaryCM.branchTypes.ITSupportOffice,
                openDate = DateTime.Now.Date,
                closeDate = DateTime.Now.Date,
                experiance = "one",
                investment = 100000,
                deposit = 25000,
                remarks = "Branch Remarks",
                contactType2 = "Mobile",
                contactNo2 = "9876543210",
                contactType1 = "Mobile",
                contactNo1 = "9873216540",
                email = "support@lionvisionits.com",
                city = "Ahmedabad Khokhra",
                state = "Gujrat",
                country = "India",
                isActive = true,
                isDeleted = false,
                entryDate = DateTime.Now,
                lastChangeDate = DateTime.Now,
                recordByLion = true,
                entryByUserName = ConstantDictionaryCM.LionVisionBranchName_string,
                changeByUserName = ConstantDictionaryCM.LionVisionBranchName_string,
                branchCodeEntryBranchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
        }
        public void Branches()
        {

            var seed = branchDB.branches.ToList();
            var searchSeed = seed.Where(Branches => Branches.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                branchDB.branches.Add(branches_Seeds.BranchLion);
            }
            branchDB.SaveChanges();
            for (int i = 0; i <= 10; i++)
            {
                branch b = JsonConvert.DeserializeObject<branch>(JsonConvert.SerializeObject(branches_Seeds.BranchLion));
                b.branchCode = b.branchCode + i;
                b.branchName = b.branchName + i;

                searchSeed = seed.Where(Branches => Branches.branchCode == b.branchCode).SingleOrDefault();

                if (searchSeed == null)
                {
                    branchDB.branches.Add(b);
                }
                branchDB.SaveChanges();
            }
        }

        public class employee_Seeds
        {
            employeelvitsposdbEntities empdb = new UniversDbContext().employeeDbContext();
            public employee_Seeds()
            {
                branchlvitsposdbEntities bdb = new UniversDbContext().branchDbContext();
                userlvitsposdbEntities udb = new UniversDbContext().userDbContext();
                employeelvitsposdbEntities edb = new UniversDbContext().employeeDbContext();

            }

            public static employee emp_detail = new employee
            {
                employeeEntryBranchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                employeeCode = "1",
                employeeEntryGroupCode = ConstantDictionaryCM.ApplicationGroupCode,
                contactNo1 = "9979608294",
                contactType1 = ConstantDictionaryCM.contact_type.HomeMobile,
                contactNo2 = "9979608294",
                contactType2 = ConstantDictionaryCM.contact_type.HomeMobile,
                title = "Employee",
                firstName = "Lion",
                middleName = "Vision",
                sureName = "LionVision",
                profilePicture = ConstantDictionaryCM.ProfilePictureViewPath_string + "1_ProfilePicture.png",
                designation = "Employee",
                gender = true,
                emialAddress = "support@lionvisionits.com",
                married = true,
                employmentStatus = 0,
                joiningdate = DateTime.Now.Date,
                currentSalary = 20000,
                castCategory = "General",
                dateOfBirth = DateTime.Now.Date,
                dateOfAniversary = DateTime.Now.Date,
                address = "Ahmedabad",
                licenseNo = "licenseNo",
                pancardNo = "pancardNo",
                employeeWorkShift = "employeeWorkShift",
                bankName = "SBI",
                bankAcNo = "121111111111",
                oneTimePassword = "9898",
                isActiveOneTimePassword = false,
                oneTimePasswordTimeOut = DateTime.Today,
                ifsccode = "ifs123",
                keyResponsibleArea = "KRA",
                salary = 25000,
                remarks = "This is the employee reamarks",
                leavingdate = DateTime.Now.Date,
                isActive = true,
                isDeleted = false,
                entryDate = DateTime.Now,
                lastChangeDate = DateTime.Now,
                recordByLion = true,
                entryByUserName = ConstantDictionaryCM.LionVisionBranchName_string,
                changeByUserName = ConstantDictionaryCM.LionVisionBranchName_string,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,

            };
        }
        public void employee()
        {
            employee_Seeds ePreConfigurationsServices = new employee_Seeds();

            var searchSeed = empDB.employees.Where(emp => emp.employeeCode == employee_Seeds.emp_detail.employeeCode && emp.employeeEntryBranchCode == employee_Seeds.emp_detail.employeeEntryBranchCode && emp.employeeEntryGroupCode == employee_Seeds.emp_detail.employeeEntryGroupCode).SingleOrDefault();
            if (searchSeed == null)
            {
                empDB.employees.Add(employee_Seeds.emp_detail);
            }
            empDB.SaveChanges();

        }

        public class employee_underbranch_Seeds
        {

            public employee_underbranch_Seeds()
            {
                userlvitsposdbEntities udb = new UniversDbContext().userDbContext();
                branchlvitsposdbEntities bdb = new UniversDbContext().branchDbContext();
                employeelvitsposdbEntities edb = new UniversDbContext().employeeDbContext();


            }

            public static employee_underbranch emp_underBranchDetail = new employee_underbranch
            {
                employeeUnderBranchEntryBranchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                employeeEntryBranchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                employeeCode = employee_Seeds.emp_detail.employeeCode,
                employeeEntryGroupCode = ConstantDictionaryCM.ApplicationGroupCode,
                effectiveDate = DateTime.Now,
                expireDate = DateTime.Now.AddYears(10),
                isActive = true,
                isDeleted = false,
                entryDate = DateTime.Now,
                lastChangeDate = DateTime.Now,
                recordByLion = true,
                entryByUserName = ConstantDictionaryCM.LionVisionBranchName_string,
                changeByUserName = ConstantDictionaryCM.LionVisionBranchName_string,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName


            };
        }
        public void employee_underbranch()
        {

            employee_underbranch_Seeds emp2 = new employee_underbranch_Seeds();

            var searchSeed = branchDB.employee_underbranch.Where(
                emp => emp.branchCode == employee_underbranch_Seeds.emp_underBranchDetail.branchCode &&
                emp.employeeCode == employee_underbranch_Seeds.emp_underBranchDetail.employeeCode &&
                emp.employeeEntryGroupCode == employee_underbranch_Seeds.emp_underBranchDetail.employeeEntryGroupCode &&
                emp.employeeEntryBranchCode == employee_underbranch_Seeds.emp_underBranchDetail.employeeEntryBranchCode
                ).SingleOrDefault();
            if (searchSeed == null)
            {
                branchDB.employee_underbranch.Add(employee_underbranch_Seeds.emp_underBranchDetail);
                branchDB.SaveChanges();
            }


        }

        public class users_Seeds
        {
            public static user userLionPOS = new user
            {
                accessRoleTitle = access_role_master_Seeds.AdminRole.accessRoleTitle,
                userName = ConstantDictionaryCM.LionVisionBranchName_string,
                userEntryGroupCode = ConstantDictionaryCM.ApplicationGroupCode,
                userEntryBranchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                employeeEntryBranchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                employeeCode = employee_Seeds.emp_detail.employeeCode,
                employeeEntryGroupCode = InitConstantRecordsDictionaryServices.employee_Seeds.emp_detail.employeeEntryGroupCode,
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                userStatus = ConstantDictionaryCM.userStatus.Offline,
                userAccountStatus = ConstantDictionaryCM.AccountStatus.Unblocked,
                passwordEncryptionKey = AESkey,
                password = AESEncPass,

                lastLogin = DateTime.Now,
                warnOnFailedLoginAfterAtttempt = 10,
                isLion = true,
                isActive = true,
                isDeleted = false,
                entryDate = DateTime.Now,
                lastChangeDate = DateTime.Now,
                recordByLion = true,
                entryByUserName = ConstantDictionaryCM.LionVisionBranchName_string,
                changeByUserName = ConstantDictionaryCM.LionVisionBranchName_string,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
        }
        public void users()
        {
            users_Seeds useed = new users_Seeds();

            var seed = userDB.users.ToList();

            var searchSeed = seed.Where(users => users.userName == users_Seeds.userLionPOS.userName && users.userEntryBranchCode == users_Seeds.userLionPOS.userEntryBranchCode && users.userEntryGroupCode == users_Seeds.userLionPOS.userEntryGroupCode).SingleOrDefault();
            if (searchSeed == null)
            {
                userDB.users.Add(users_Seeds.userLionPOS);
                userDB.SaveChanges();
            }


        }



        public class Setting_Seeds
        {

            public Setting_Seeds()
            {
                configurationlvitsposdbEntities db = new UniversDbContext().configurationDbContext();
                var seed = db.settings.ToList();


                //seed.Where(b => b.branchCode == Setting_Seeds.setting.branchCode).Select(s => s.branchCode).SingleOrDefault();
            }
            #region setting
            public static setting Incomig_Server_Pop3 = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Incomig_Server_Pop3.title,
                values = "pop.asia.secureserver.net",
                description = "Incoming server (POP3)",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,

            };
            public static setting Incoming_Server_IMAP = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Incoming_Server_IMAP.title,
                values = "imap.asia.secureserver.net",
                description = "Incoming server (IMAP)",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Outgoing_Server_SMTP = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Outgoing_Server_SMTP.title,
                values = "smtpout.asia.secureserver.net",
                description = "Outgoing server (SMTP)",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Incoming_Server_POP3_PORT = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Incoming_Server_POP3_PORT.title,
                values = "110",
                description = "Incoming server (POP3) PORT",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Incoming_Server_IMAP_PORT = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Incoming_Server_IMAP_PORT.title,
                values = "143",
                description = "Incoming server (IMAP) PORT",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Outgoing_Server_SMTP_PORT = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Outgoing_Server_SMTP_PORT.title,
                values = "3535",
                description = "Outgoing server (SMTP) PORT",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Incoming_Server_POP3_PORT_SSL = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Incoming_Server_POP3_PORT_SSL.title,
                values = "995",
                description = "Incoming server (POP3) PORT SSL",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Incoming_Server_IMAP_PORT_SSL = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Incoming_Server_IMAP_PORT_SSL.title,
                values = "993",
                description = "Incoming server (IMAP) PORT SSL",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Outgoing_Server_SMTP_PORT_SSL = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Outgoing_Server_SMTP_PORT_SSL.title,
                values = "465",
                description = "Outgoing server (SMTP) PORT SSL",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Send_Email_Using_SSL = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Send_Email_Using_SSL.title,
                values = "no",
                description = "Send email using SSL",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Forgot_Password_Template_Path = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Forgot_Password_Template_Path.title,
                values = "email templage path",
                description = "Forgot Password Template Path",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting TimeZoneInfoId = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.TimeZoneInfoId.title,
                values = "India Standard Time",
                description = "GMT +5:30 for INDIA",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Date_Format_Short = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Date_Format_Short.title,
                values = "dd/mm/yyyy",
                description = "Indian Format",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Date_Format_Long = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Date_Format_Long.title,
                values = "dd/mm/yyyy HH:mm:ss tt",
                description = "Indian Format",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Culture_Information = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Culture_Information.title,
                values = "en-IN",
                description = "Indian Format",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Currency_Symbol = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Currency_Symbol.title,
                values = "₹",
                description = "Indian Format",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Reset_Password_After_Days = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Reset_Password_After_Days.title,
                values = "30",
                description = "Reset Password",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Notify_User_To_Reset_Password_At_First_Login = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Notify_User_To_Reset_Password_At_First_Login.title,
                values = "false",
                description = "Reset Password at firest login",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Support_Email_Address = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Support_Email_Address.title,
                values = "support@lionvisionits.com",
                description = "support address",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Support_Email_Password = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Support_Email_Password.title,
                values = "emineme12",
                description = "support password",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Head_Office_Static_IP_Address = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Head_Office_Static_IP_Address.title,
                values = "113.20.19.123",
                description = "Head Office Static IP Address",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Supplier_Purchase_Order_Starts_From = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Supplier_Purchase_Order_Starts_From.title,
                values = "1",
                description = "Supplier Purchase Order Starts From",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Supplier_Purchase_Order_Prefix = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Supplier_Purchase_Order_Prefix.title,
                values = "SUPO2016",
                description = "Supplier Purchase Order Prefix",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Supplier_Purchase_Order_Expires = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Supplier_Purchase_Order_Expires.title,
                values = "31/12/2016",
                description = "Supplier Purchase Order Expires",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Recipt_From = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Recipt_From.title,
                values = "1",
                description = "Recipt From",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Recipt_Prefix = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Recipt_Prefix.title,
                values = "RE2016",
                description = "Recipt Prefix",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Recipt_Expiry = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Recipt_Expiry.title,
                values = "31/12/2016",
                description = "Recipt Expiry",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Delivery_Challan_Starts_From = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Delivery_Challan_Starts_From.title,
                values = "1",
                description = "Delivery Challan Starts From",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Delivery_Challan_Starts_Prefix = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Delivery_Challan_Starts_Prefix.title,
                values = "DEL2016",
                description = "Delivery Challan Starts Prefix",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Delivery_Challan_Expires = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Delivery_Challan_Expires.title,
                values = "31/12/2016",
                description = "Delivery Challan Expires",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Supplier_Purchase_Return_Starts_From = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Supplier_Purchase_Return_Starts_From.title,
                values = "1",
                description = "Supplier Purchase Return Starts From",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Supplier_Purchase_Return_Prefix = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Supplier_Purchase_Return_Prefix.title,
                values = "SUPR2016",
                description = "Supplier Purchase Return Prefix",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Supplier_Purchase_Return_Expires = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Supplier_Purchase_Return_Expires.title,
                values = "31/12/2016",
                description = "Supplier Purchase Expires",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Outlet_Purchase_Order_Starts_From = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Outlet_Purchase_Order_Starts_From.title,
                values = "1",
                description = "Outlet Purchase Order Starts From",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Outlet_Purchase_Order_Prefix = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Outlet_Purchase_Order_Prefix.title,
                values = "POSPO2016",
                description = "Outlet Purchase Order Prefix",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Outlet_Purchase_Order_Expires = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Outlet_Purchase_Order_Expires.title,
                values = "31/12/2016",
                description = "Outlet Purchase Order Expires",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Sales_Starts_From = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Sales_Starts_From.title,
                values = "1",
                description = "Sales Starts From",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Sales_Starts_Prefix = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Sales_Starts_Prefix.title,
                values = "SL2016",
                description = "Sales Starts Prefix",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Sales_Expires = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Sales_Expires.title,
                values = "31/12/2016",
                description = "Sales Expires",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting POS_Sales_Starts_From = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.POS_Sales_Starts_From.title,
                values = "1",
                description = "POS Sales Starts From",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting POS_Sales_Prefix = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.POS_Sales_Prefix.title,
                values = "POSSL2016",
                description = "POS Sales Prefix",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting POS_Sales_Expires = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.POS_Sales_Expires.title,
                values = "31/12/2016",
                description = "POS Sales Expires",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Sales_Return_Starts_From = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Sales_Return_Starts_From.title,
                values = "1",
                description = "Sales Return Starts From",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Sales_Return_Prefix = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Sales_Return_Prefix.title,
                values = "SLRE2016",
                description = "Sales Return Prefix",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Sales_Return_Expires = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Sales_Return_Expires.title,
                values = "31/12/2016",
                description = "Sales Return Expires",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting POS_Sales_Returns_Starts_From = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.POS_Sales_Returns_Starts_From.title,
                values = "1",
                description = "POS Sales Returns Starts From",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting POS_Sales_Returns_Prefix = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.POS_Sales_Returns_Prefix.title,
                values = "POSRE2016",
                description = "POS Sales Returns Prefix",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting POS_Sales_Returns_Expires = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.POS_Sales_Returns_Expires.title,
                values = "31/12/2016",
                description = "POS Sales Returns Expires",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Server_Status = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Server_Status.title,
                values = "Active",
                description = "Server Status",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Entry_Group_Code = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Entry_Group_Code.title,
                values = "Online",
                description = "Possible options Online , Local",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Branch_Secret_Key = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Branch_Secret_Key.title,
                values = "123",
                description = "Branch Secret Key",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Captcha_Active = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Captcha_Active.title,
                values = "Yes",
                description = "Decide captch should display after failed attemt for Robot Access check",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Captcha_Count_Start_After_Attempt = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Captcha_Count_Start_After_Attempt.title,
                values = "5",
                description = "After Value amount captcha will be asked to user for robot verification",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Block_Acount_After_Attempt = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Block_Acount_After_Attempt.title,
                values = "Yes",
                description = "Should accoun be block after captcha attempt fails",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Block_Account_After_Attempt_Count = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Block_Account_After_Attempt_Count.title,
                values = "10",
                description = "Block Acount After Number OF Captch Attempt Fails",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Unblock_Account = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Unblock_Account.title,
                values = "5",
                description = "0 Stands for Manual Unblock Grater than 0 stands for After Seconds",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting Session_Expiry_Time_In_Minutes = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.Session_Expiry_Time_In_Minutes.title,
                values = "10000",
                description = "",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            public static setting WebsiteUnderMaintenance = new setting
            {
                branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
                title = ConstantRecordsDictionaryCM.Setting_Seeds.WebsiteUnderMaintenance.title,
                values = "No",
                description = "",
                entryByUserName = users_Seeds.userLionPOS.userName,
                changeByUserName = users_Seeds.userLionPOS.userName,
                insertRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
                updateRoutePoint = ConstantDictionaryCM.currentRouteNodeName,
            };
            //public static setting BranchSortFieldsSettings = new setting
            //{
            //    branchCode = ConstantDictionaryCM.LionVisionBranchCode_string,
            //    title = ConstantRecordsDictionaryCM.Setting_Seeds.BranchFilterSaveAsDefault.title,
            //    values = "|isActive asc,entryDate desc",
            //    description = "",
            //};

            #endregion
        }
        public void setting()
        {
            Setting_Seeds b = new Setting_Seeds();

            var seed = confDB.settings.ToList();
            var searchSeed = seed.Where(Incomig_Server_Pop3 => Incomig_Server_Pop3.title == Setting_Seeds.Incomig_Server_Pop3.title && Incomig_Server_Pop3.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Incomig_Server_Pop3);
            }

            searchSeed = seed.Where(Incoming_Server_IMAP => Incoming_Server_IMAP.title == Setting_Seeds.Incoming_Server_IMAP.title && Incoming_Server_IMAP.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Incoming_Server_IMAP);
            }

            searchSeed = seed.Where(Outgoing_Server_SMTP => Outgoing_Server_SMTP.title == Setting_Seeds.Outgoing_Server_SMTP.title && Outgoing_Server_SMTP.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Outgoing_Server_SMTP);
            }

            searchSeed = seed.Where(Incoming_Server_POP3_PORT => Incoming_Server_POP3_PORT.title == Setting_Seeds.Incoming_Server_POP3_PORT.title && Incoming_Server_POP3_PORT.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Incoming_Server_POP3_PORT);
            }

            searchSeed = seed.Where(Incoming_Server_IMAP_PORT => Incoming_Server_IMAP_PORT.title == Setting_Seeds.Incoming_Server_IMAP_PORT.title && Incoming_Server_IMAP_PORT.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Incoming_Server_IMAP_PORT);
            }

            searchSeed = seed.Where(Outgoing_Server_SMTP_PORT => Outgoing_Server_SMTP_PORT.title == Setting_Seeds.Outgoing_Server_SMTP_PORT.title && Outgoing_Server_SMTP_PORT.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Outgoing_Server_SMTP_PORT);
            }

            searchSeed = seed.Where(Incoming_Server_POP3_PORT_SSL => Incoming_Server_POP3_PORT_SSL.title == Setting_Seeds.Incoming_Server_POP3_PORT_SSL.title && Incoming_Server_POP3_PORT_SSL.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Incoming_Server_POP3_PORT_SSL);
            }

            searchSeed = seed.Where(Incoming_Server_IMAP_PORT_SSL => Incoming_Server_IMAP_PORT_SSL.title == Setting_Seeds.Incoming_Server_IMAP_PORT_SSL.title && Incoming_Server_IMAP_PORT_SSL.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Incoming_Server_IMAP_PORT_SSL);
            }

            searchSeed = seed.Where(Outgoing_Server_SMTP_PORT_SSL => Outgoing_Server_SMTP_PORT_SSL.title == Setting_Seeds.Outgoing_Server_SMTP_PORT_SSL.title && Outgoing_Server_SMTP_PORT_SSL.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Outgoing_Server_SMTP_PORT_SSL);
            }

            searchSeed = seed.Where(Send_Email_Using_SSL => Send_Email_Using_SSL.title == Setting_Seeds.Send_Email_Using_SSL.title && Send_Email_Using_SSL.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Send_Email_Using_SSL);
            }

            searchSeed = seed.Where(Forgot_Password_Template_Path => Forgot_Password_Template_Path.title == Setting_Seeds.Forgot_Password_Template_Path.title && Forgot_Password_Template_Path.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Forgot_Password_Template_Path);
            }

            searchSeed = seed.Where(TimeZoneInfoId => TimeZoneInfoId.title == Setting_Seeds.TimeZoneInfoId.title && TimeZoneInfoId.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.TimeZoneInfoId);
            }

            searchSeed = seed.Where(Date_Format_Short => Date_Format_Short.title == Setting_Seeds.Date_Format_Short.title && Date_Format_Short.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Date_Format_Short);
            }

            searchSeed = seed.Where(Date_Format_Long => Date_Format_Long.title == Setting_Seeds.Date_Format_Long.title && Date_Format_Long.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Date_Format_Long);
            }

            searchSeed = seed.Where(Culture_Information => Culture_Information.title == Setting_Seeds.Culture_Information.title && Culture_Information.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Culture_Information);
            }

            searchSeed = seed.Where(Currency_Symbol => Currency_Symbol.title == Setting_Seeds.Currency_Symbol.title && Currency_Symbol.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Currency_Symbol);
            }

            searchSeed = seed.Where(Reset_Password_After_Days => Reset_Password_After_Days.title == Setting_Seeds.Reset_Password_After_Days.title && Reset_Password_After_Days.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Reset_Password_After_Days);
            }

            searchSeed = seed.Where(Notify_User_To_Reset_Password_At_First_Login => Notify_User_To_Reset_Password_At_First_Login.title == Setting_Seeds.Notify_User_To_Reset_Password_At_First_Login.title && Notify_User_To_Reset_Password_At_First_Login.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Notify_User_To_Reset_Password_At_First_Login);
            }

            searchSeed = seed.Where(Support_Email_Address => Support_Email_Address.title == Setting_Seeds.Support_Email_Address.title && Support_Email_Address.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Support_Email_Address);
            }

            searchSeed = seed.Where(Support_Email_Password => Support_Email_Password.title == Setting_Seeds.Support_Email_Password.title && Support_Email_Password.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Support_Email_Password);
            }

            searchSeed = seed.Where(Head_Office_Static_IP_Address => Head_Office_Static_IP_Address.title == Setting_Seeds.Head_Office_Static_IP_Address.title && Head_Office_Static_IP_Address.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Head_Office_Static_IP_Address);
            }

            searchSeed = seed.Where(Supplier_Purchase_Order_Starts_From => Supplier_Purchase_Order_Starts_From.title == Setting_Seeds.Supplier_Purchase_Order_Starts_From.title && Supplier_Purchase_Order_Starts_From.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Supplier_Purchase_Order_Starts_From);
            }

            searchSeed = seed.Where(Supplier_Purchase_Order_Prefix => Supplier_Purchase_Order_Prefix.title == Setting_Seeds.Supplier_Purchase_Order_Prefix.title && Supplier_Purchase_Order_Prefix.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Supplier_Purchase_Order_Prefix);
            }

            searchSeed = seed.Where(Supplier_Purchase_Order_Expires => Supplier_Purchase_Order_Expires.title == Setting_Seeds.Supplier_Purchase_Order_Expires.title && Supplier_Purchase_Order_Expires.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Supplier_Purchase_Order_Expires);
            }

            searchSeed = seed.Where(Recipt_From => Recipt_From.title == Setting_Seeds.Recipt_From.title && Recipt_From.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Recipt_From);
            }

            searchSeed = seed.Where(Recipt_Prefix => Recipt_Prefix.title == Setting_Seeds.Recipt_Prefix.title && Recipt_Prefix.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Recipt_Prefix);
            }

            searchSeed = seed.Where(Recipt_Expiry => Recipt_Expiry.title == Setting_Seeds.Recipt_Expiry.title && Recipt_Expiry.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Recipt_Expiry);
            }

            searchSeed = seed.Where(Delivery_Challan_Starts_From => Delivery_Challan_Starts_From.title == Setting_Seeds.Delivery_Challan_Starts_From.title && Delivery_Challan_Starts_From.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Delivery_Challan_Starts_From);
            }

            searchSeed = seed.Where(Delivery_Challan_Starts_Prefix => Delivery_Challan_Starts_Prefix.title == Setting_Seeds.Delivery_Challan_Starts_Prefix.title && Delivery_Challan_Starts_Prefix.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Delivery_Challan_Starts_Prefix);
            }

            searchSeed = seed.Where(Delivery_Challan_Expires => Delivery_Challan_Expires.title == Setting_Seeds.Delivery_Challan_Expires.title && Delivery_Challan_Expires.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Delivery_Challan_Expires);
            }

            searchSeed = seed.Where(Supplier_Purchase_Return_Starts_From => Supplier_Purchase_Return_Starts_From.title == Setting_Seeds.Supplier_Purchase_Return_Starts_From.title && Supplier_Purchase_Return_Starts_From.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Supplier_Purchase_Return_Starts_From);
            }

            searchSeed = seed.Where(Supplier_Purchase_Return_Prefix => Supplier_Purchase_Return_Prefix.title == Setting_Seeds.Supplier_Purchase_Return_Prefix.title && Supplier_Purchase_Return_Prefix.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Supplier_Purchase_Return_Prefix);
            }

            searchSeed = seed.Where(Supplier_Purchase_Return_Expires => Supplier_Purchase_Return_Expires.title == Setting_Seeds.Supplier_Purchase_Return_Expires.title && Supplier_Purchase_Return_Expires.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Supplier_Purchase_Return_Expires);
            }

            searchSeed = seed.Where(Outlet_Purchase_Order_Starts_From => Outlet_Purchase_Order_Starts_From.title == Setting_Seeds.Outlet_Purchase_Order_Starts_From.title && Outlet_Purchase_Order_Starts_From.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Outlet_Purchase_Order_Starts_From);
            }

            searchSeed = seed.Where(Outlet_Purchase_Order_Prefix => Outlet_Purchase_Order_Prefix.title == Setting_Seeds.Outlet_Purchase_Order_Prefix.title && Outlet_Purchase_Order_Prefix.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Outlet_Purchase_Order_Prefix);
            }

            searchSeed = seed.Where(Outlet_Purchase_Order_Expires => Outlet_Purchase_Order_Expires.title == Setting_Seeds.Outlet_Purchase_Order_Expires.title && Outlet_Purchase_Order_Expires.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Outlet_Purchase_Order_Expires);
            }

            searchSeed = seed.Where(Sales_Starts_From => Sales_Starts_From.title == Setting_Seeds.Sales_Starts_From.title && Sales_Starts_From.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Sales_Starts_From);
            }

            searchSeed = seed.Where(Sales_Starts_Prefix => Sales_Starts_Prefix.title == Setting_Seeds.Sales_Starts_Prefix.title && Sales_Starts_Prefix.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Sales_Starts_Prefix);
            }

            searchSeed = seed.Where(Sales_Expires => Sales_Expires.title == Setting_Seeds.Sales_Expires.title && Sales_Expires.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Sales_Expires);
            }

            searchSeed = seed.Where(POS_Sales_Starts_From => POS_Sales_Starts_From.title == Setting_Seeds.POS_Sales_Starts_From.title && POS_Sales_Starts_From.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.POS_Sales_Starts_From);
            }

            searchSeed = seed.Where(POS_Sales_Prefix => POS_Sales_Prefix.title == Setting_Seeds.POS_Sales_Prefix.title && POS_Sales_Prefix.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.POS_Sales_Prefix);
            }

            searchSeed = seed.Where(POS_Sales_Expires => POS_Sales_Expires.title == Setting_Seeds.POS_Sales_Expires.title && POS_Sales_Expires.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.POS_Sales_Expires);
            }

            searchSeed = seed.Where(Sales_Return_Starts_From => Sales_Return_Starts_From.title == Setting_Seeds.Sales_Return_Starts_From.title && Sales_Return_Starts_From.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Sales_Return_Starts_From);
            }

            searchSeed = seed.Where(Sales_Return_Prefix => Sales_Return_Prefix.title == Setting_Seeds.Sales_Return_Prefix.title && Sales_Return_Prefix.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Sales_Return_Prefix);
            }

            searchSeed = seed.Where(Sales_Return_Expires => Sales_Return_Expires.title == Setting_Seeds.Sales_Return_Expires.title && Sales_Return_Expires.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Sales_Return_Expires);
            }

            searchSeed = seed.Where(POS_Sales_Returns_Starts_From => POS_Sales_Returns_Starts_From.title == Setting_Seeds.POS_Sales_Returns_Starts_From.title && POS_Sales_Returns_Starts_From.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.POS_Sales_Returns_Starts_From);
            }

            searchSeed = seed.Where(POS_Sales_Returns_Prefix => POS_Sales_Returns_Prefix.title == Setting_Seeds.POS_Sales_Returns_Prefix.title && POS_Sales_Returns_Prefix.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.POS_Sales_Returns_Prefix);
            }

            searchSeed = seed.Where(POS_Sales_Returns_Expires => POS_Sales_Returns_Expires.title == Setting_Seeds.POS_Sales_Returns_Expires.title && POS_Sales_Returns_Expires.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.POS_Sales_Returns_Expires);
            }

            searchSeed = seed.Where(Server_Status => Server_Status.title == Setting_Seeds.Server_Status.title && Server_Status.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Server_Status);
            }

            searchSeed = seed.Where(Entry_Group_Code => Entry_Group_Code.title == Setting_Seeds.Entry_Group_Code.title && Entry_Group_Code.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Entry_Group_Code);
            }

            searchSeed = seed.Where(Branch_Secret_Key => Branch_Secret_Key.title == Setting_Seeds.Branch_Secret_Key.title && Branch_Secret_Key.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Branch_Secret_Key);
            }
            searchSeed = seed.Where(Captcha_Active => Captcha_Active.title == Setting_Seeds.Captcha_Active.title && Captcha_Active.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Captcha_Active);
            }

            searchSeed = seed.Where(Captcha_Attempt_Counts => Captcha_Attempt_Counts.title == Setting_Seeds.Captcha_Count_Start_After_Attempt.title && Captcha_Attempt_Counts.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Captcha_Count_Start_After_Attempt);
            }


            searchSeed = seed.Where(Block_Acount_After_Captch_Attempt_Fail => Block_Acount_After_Captch_Attempt_Fail.title == Setting_Seeds.Block_Acount_After_Attempt.title && Block_Acount_After_Captch_Attempt_Fail.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Block_Acount_After_Attempt);
            }

            searchSeed = seed.Where(Block_Acount_Counts => Block_Acount_Counts.title == Setting_Seeds.Block_Account_After_Attempt_Count.title && Block_Acount_Counts.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Block_Account_After_Attempt_Count);
            }
            searchSeed = seed.Where(Unblock_Account => Unblock_Account.title == Setting_Seeds.Unblock_Account.title && Unblock_Account.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Unblock_Account);
            }

            searchSeed = seed.Where(Session_Expiry_Time_In_Minutes => Session_Expiry_Time_In_Minutes.title == Setting_Seeds.Session_Expiry_Time_In_Minutes.title && Session_Expiry_Time_In_Minutes.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.Session_Expiry_Time_In_Minutes);
            }

            //searchSeed = seed.Where(BranchSortFieldsSettings => BranchSortFieldsSettings.title == Setting_Seeds.BranchSortFieldsSettings.title && BranchSortFieldsSettings.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            //if (searchSeed == null)
            //{
            //    confDB.settings.Add(Setting_Seeds.BranchSortFieldsSettings);
            //}

            searchSeed = seed.Where(WebsiteUnderMaintenance => WebsiteUnderMaintenance.title == Setting_Seeds.WebsiteUnderMaintenance.title && WebsiteUnderMaintenance.branchCode == branches_Seeds.BranchLion.branchCode).SingleOrDefault();
            if (searchSeed == null)
            {
                confDB.settings.Add(Setting_Seeds.WebsiteUnderMaintenance);
            }

            //foreach (setting a in confDB.settings)
            //{
            //    a.branchCode = branchCode;
            //}

            confDB.SaveChanges();

        }
    }
}