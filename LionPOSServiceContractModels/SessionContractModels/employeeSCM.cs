using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LionPOSServiceContractModels
{
    [DataContract]
    public  class employeeSCM
    {
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
        public string employeeCode { get; set; }
        [DataMember]
        public string employeeEntryBranchCode { get; set; }
        [DataMember]
        public string employeeEntryGroupCode { get; set; }
        [DataMember]
        public string contactNo { get; set; }
        [DataMember]
        public string emialAddress { get; set; }
        
    }
}
