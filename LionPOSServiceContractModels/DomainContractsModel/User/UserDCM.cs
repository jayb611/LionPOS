using LionPOSServiceContractModels.ErrorContactModel;
using System;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.DomainContractsModel.User
{
    [DataContract]
    public class UserDCM : ErrorCM
    {
        [DataMember]
        public SessionCM SessionCM { get; set; }


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
        public DateTime lastLogin { get; set; }

        [DataMember]
        public DateTime? lastPasswordResetDate { get; set; }

        [DataMember]
        public DateTime? blockExpiry { get; set; }

        [DataMember]
        public int warnOnFailedLoginAfterAtttempt { get; set; }

        [DataMember]
        public bool isLion { get; set; }

        [DataMember]
        public bool isActive { get; set; }

        [DataMember]
        public bool isDeleted { get; set; }

        [DataMember]
        public DateTime entryDate { get; set; }

        [DataMember]
        public DateTime lastChangeDate { get; set; }

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
        public DateTime? sessionExpireyDateTime { get; set; }

        [DataMember]
        public DateTime? sessionCreateTime { get; set; }
    }

}
