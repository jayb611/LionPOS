using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LionPOSDbContracts.DomainModels.User
{
    [MetadataType(typeof(userEntityValidationModel))]
    public partial class user
    {
    }
    [Serializable]
    [DataContract]
    public class userEntityValidationModel
    {
        [DataMember]
        public string accessRoleTitle { get; set; }
        [DataMember]
        public string userName { get; set; }
        [DataMember]
        public string userEntryGroupCode { get; set; }
        [DataMember]
        public string userEntryBranchCode { get; set; }
        [DataMember]
        public string employeeEntryBranchCode { get; set; }
        [DataMember]
        public string employeeCode { get; set; }
        [DataMember]
        public string employeeEntryGroupCode { get; set; }
        [DataMember]
        public string branchCode { get; set; }
        [DataMember]
        public string userStatus { get; set; }
        [DataMember]
        public string userAccountStatus { get; set; }
        [DataMember]
        public string passwordEncryptionKey { get; set; }
        [DataMember]
        public string password { get; set; }
        [DataMember]
        public System.DateTime lastLogin { get; set; }
        [DataMember]
        public Nullable<System.DateTime> lastPasswordResetDate { get; set; }
        [DataMember]
        public Nullable<System.DateTime> blockExpiry { get; set; }
        [DataMember]
        public int warnOnFailedLoginAfterAtttempt { get; set; }
        [DataMember]
        public bool isLion { get; set; }
        [DataMember]
        public bool isActive { get; set; }
        [DataMember]
        public bool isDeleted { get; set; }
        [DataMember]
        public System.DateTime entryDate { get; set; }
        [DataMember]
        public System.DateTime lastChangeDate { get; set; }
        [DataMember]
        public bool recordByLion { get; set; }
        [DataMember]
        public string entryByUserName { get; set; }
        [DataMember]
        public string changeByUserName { get; set; }
        [DataMember]
        public string sessionID { get; set; }
        [DataMember]
        public string sessionValue { get; set; }
        [DataMember]
        public Nullable<System.DateTime> sessionExpireyDateTime { get; set; }
        [DataMember]
        public Nullable<System.DateTime> sessionCreateTime { get; set; }
        [DataMember]
        public string insertRoutePoint { get; set; }
        [DataMember]
        public string updateRoutePoint { get; set; }
        [DataMember]
        public string syncGUID { get; set; }

        [DataMember]
        public virtual access_role_master access_role_master { get; set; }
        
        public virtual ICollection<override_access_role> override_access_role { get; set; }
        public virtual ICollection<user_settings> user_settings { get; set; }
    }
}