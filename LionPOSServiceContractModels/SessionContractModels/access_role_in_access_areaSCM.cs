using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LionPOSServiceContractModels
{
    [DataContract]
   public class access_role_in_access_areaSCM
    {
        [DataMember]
        public string domain { get; set; }
        [DataMember]
        public string controller { get; set; }
        [DataMember]
        public string view { get; set; }
        [DataMember]
        public bool canAccess { get; set; }
        [DataMember]
        public bool visible { get; set; }
        [DataMember]
        public bool underMaintenance { get; set; }
        [DataMember]
        public bool showBranchWise { get; set; }
    }
}
