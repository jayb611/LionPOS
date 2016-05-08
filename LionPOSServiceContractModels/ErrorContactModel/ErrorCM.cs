using System;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.ErrorContactModel
{
    [DataContract]
   public class ErrorCM
    {
        [DataMember]
        public int errorLogId { get; set; }
    }
}
