using LionPOSServiceContractModels.ErrorContactModel;
using System;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.DomainContractsModel.Configuration
{
    [DataContract]
    public class CountryDCM : ErrorCM 
    {
        [DataMember]
        public string countryName { get; set; }
        [DataMember]
        public string a2 { get; set; }
        [DataMember]
        public string a3 { get; set; }
        [DataMember]
        public Nullable<int> phoneCode { get; set; }
        [DataMember]
        public string capitols { get; set; }
        [DataMember]
        public string latitude { get; set; }
        [DataMember]
        public string logitude { get; set; }
        [DataMember]
        public string isoUnm49Code { get; set; }
        [DataMember]
        public string currencyCode { get; set; }
        [DataMember]
        public string currencyName { get; set; }




    }
}
