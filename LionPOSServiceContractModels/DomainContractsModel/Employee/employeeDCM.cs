using LionPOSServiceContractModels.ErrorContactModel;
using System;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.DomainContractsModel.Employee
{
    [DataContract]
    public class employeeDCM : ErrorCM
    {

        [DataMember]
        public SessionCM SessionCM { get; set; }
        [DataMember]
        public string employeeEntryBranchCode { get; set; }
        [DataMember]
        public string employeeCode { get; set; }
        [DataMember]
        public string employeeEntryGroupCode { get; set; }
        [DataMember]
        public string contactNo1 { get; set; }
        [DataMember]
        public string contactType1 { get; set; }
        [DataMember]
        public string contactNo2 { get; set; }
        [DataMember]
        public string contactType2 { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string firstName { get; set; }
        [DataMember]
        public string middleName { get; set; }
        [DataMember]
        public string sureName { get; set; }
        [DataMember]
        public string profilePicture { get; set; }
        [DataMember]
        public string idproof { get; set; }
        [DataMember]
        public string oneTimePassword { get; set; }
        [DataMember]
        public bool isActiveOneTimePassword { get; set; }
        [DataMember]
        public System.DateTime oneTimePasswordTimeOut { get; set; }
        [DataMember]
        public string designation { get; set; }
        [DataMember]
        public bool gender { get; set; }
        [DataMember]
        public string emialAddress { get; set; }
        [DataMember]
        public bool married { get; set; }
        [DataMember]
        public int employmentStatus { get; set; }
        [DataMember]
        public System.DateTime joiningdate { get; set; }
        [DataMember]
        public Nullable<decimal> currentSalary { get; set; }
        [DataMember]
        public string castCategory { get; set; }
        [DataMember]
        public Nullable<System.DateTime> dateOfBirth { get; set; }
        [DataMember]
        public Nullable<System.DateTime> dateOfAniversary { get; set; }
        [DataMember]
        public string address { get; set; }
        [DataMember]
        public string licenseNo { get; set; }
        [DataMember]
        public string pancardNo { get; set; }
        [DataMember]
        public string employeeWorkShift { get; set; }
        [DataMember]
        public string bankName { get; set; }
        [DataMember]
        public string bankAcNo { get; set; }
        [DataMember]
        public string ifsccode { get; set; }
        [DataMember]
        public string keyResponsibleArea { get; set; }
        [DataMember]
        public Nullable<decimal> salary { get; set; }
        [DataMember]
        public string remarks { get; set; }
        [DataMember]
        public Nullable<System.DateTime> leavingdate { get; set; }
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
    }
}
