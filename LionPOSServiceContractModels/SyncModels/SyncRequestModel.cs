using LionPOSServiceContractModels.ErrorContactModel;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels
{
    [DataContract]
    public class SyncRequestModel :ErrorCM
    {
        [DataMember]
        public string  table { get; set; }
        [DataMember]
        public string database { get; set; }
        [DataMember]
        public string JsonParamString { get; set; }
        [DataMember]
        public string Exception { get; set; }
        [DataMember]
        public string SenderSyncNode { get; set; }
        [DataMember]
        public string senderSyncGUID { get; set; }
        [DataMember]
        public string receiverSyncGUID { get; set; }


    }
}