using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LionPOSServiceContractModels
{
    [DataContract]
    public class branchSCM
    {
        [DataMember]
        public string branchCode { get; set; }
        [DataMember]
        public string branchName { get; set; }
        [DataMember]
        public string GroupCode { get; set; }
        [DataMember]
        public string logitudeLocation { get; set; }
        [DataMember]
        public string latitudeLoaction { get; set; }
        [DataMember]
        public string branchType { get; set; }
    }
}
