using LionPOSServiceContractModels.ErrorContactModel;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels
{
    [DataContract]
    public class SyncResponseModel :ErrorCM
    {
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