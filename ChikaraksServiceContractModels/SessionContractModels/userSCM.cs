using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChikaraksServiceContractModels
{
    [DataContract]
    public  class userSCM
    {
        [DataMember]
        public string userName { get; set; }
        [DataMember]
        public string passwordEncryptionKey { get; set; }
        [DataMember]
        public string password { get; set; }
        [DataMember]
        public string sessionID { get; set; }
        [DataMember]
        public string sessionValue { get; set; }
        [DataMember]
        public Nullable<System.DateTime> sessionExpireyDateTime { get; set; }
        [DataMember]
        public Nullable<System.DateTime> sessionCreateTime { get; set; }
        [DataMember]
        public Nullable<System.DateTime> blockExpiry { get; set; }
        [DataMember]
        public string userAccountStatus { get; set; }
        [DataMember]
        public bool isActive { get; set; }
    }
}
