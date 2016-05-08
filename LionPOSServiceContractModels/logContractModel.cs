using LionPOSServiceContractModels.ErrorContactModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LionPOSServiceContractModels
{
    [DataContract]
    public class logContractModel : ErrorCM
    {
        [DataMember]
        public int idlogs { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public bool isError { get; set; }
        [DataMember]
        public string exception { get; set; }
        [DataMember]
        public string objectAttachment { get; set; }
        [DataMember]
        public string className { get; set; }
        [DataMember]
        public string methodName { get; set; }
        [DataMember]
        public int lineNumber { get; set; }
        [DataMember]
        public string fileName { get; set; }
        [DataMember]
        public string eventJson { get; set; }
        [DataMember]
        public string errorURL { get; set; }
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
