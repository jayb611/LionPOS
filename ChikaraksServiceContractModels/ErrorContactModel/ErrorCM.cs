using System;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ErrorContactModel
{
    [DataContract]
    public class ErrorCM
    {
        [DataMember]
        public int errorLogId { get; set; }
        [DataMember]
        public string error { get; set; }
        [DataMember]
        public string errorDetails { get; set; }
    }
}
