using ChikaraksServiceContractModels.ErrorContactModel;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.DomainContractsModel
{
    [DataContract]
    public class UserDCM : ErrorCM
    {
        [DataMember]
        public int iduser { get; set; }
        [DataMember]
        public string userName { get; set; }
        [DataMember]
        public string password { get; set; }
        [DataMember]
        public bool isActive { get; set; }
    }
}
