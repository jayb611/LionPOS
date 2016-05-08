using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LionPOSServiceContractModels
{
    [DataContract]
    public class override_access_roleSCM
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
        public string title { get; set; }
        [DataMember]
        public bool permanent { get; set; }
        [DataMember]
        public Nullable<System.DateTime> overrideTill { get; set; }
    }
}
