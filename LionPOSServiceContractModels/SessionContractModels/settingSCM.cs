using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LionPOSServiceContractModels
{
    [DataContract]
    public class settingSCM
    {
        [DataMember]
        public string branchCode { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string values { get; set; }
        [DataMember]
        public string description { get; set; }
    }
}
