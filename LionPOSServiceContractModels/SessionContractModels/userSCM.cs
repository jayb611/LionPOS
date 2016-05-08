using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LionPOSServiceContractModels
{
    [DataContract]
    public  class userSCM
    {
        [DataMember]
        public string userName { get; set; }
        [DataMember]
        public string accessRoleTitle { get; set; }
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
        public string userStatus { get; set; }
        [DataMember]
        public string userAccountStatus { get; set; }
        [DataMember]
        public System.DateTime lastLogin { get; set; }
        [DataMember]
        public Nullable<System.DateTime> lastPasswordResetDate { get; set; }
        [DataMember]
        public Nullable<System.DateTime> blockExpiry { get; set; }
        [DataMember]
        public string sessionID { get; set; }
        [DataMember]
        public string sessionValue { get; set; }
        [DataMember]
        public Nullable<System.DateTime> sessionExpireyDateTime { get; set; }
        [DataMember]
        public Nullable<System.DateTime> sessionCreateTime { get; set; }
        [DataMember]
        public string passwordEncryptionKey { get; set; }
        [DataMember]
        public string password { get; set; }

    }
}
