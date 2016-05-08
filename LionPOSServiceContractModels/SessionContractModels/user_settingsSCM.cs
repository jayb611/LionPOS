using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LionPOSServiceContractModels
{
    [DataContract]
    public  class user_settingsSCM
    {
        [DataMember]
        public string userName { get; set; }
        [DataMember]
        public string userEntryGroupCode { get; set; }
        [DataMember]
        public string userEntryBranchCode { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string values { get; set; }
        [DataMember]
        public string description { get; set; }

    }
}
