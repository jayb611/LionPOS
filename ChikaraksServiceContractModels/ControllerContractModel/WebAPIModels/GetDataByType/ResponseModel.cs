using ChikaraksServiceContractModels.DomainContractsModel;
using ChikaraksServiceContractModels.ErrorContactModel;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace KakubhaiAndSonsServiceContractModels.ControllerContractModel.WebAPIModels.GetDataByType
{
    [DataContract]
    public class ResponseModel : ErrorCM
    {
        [DataMember]
        public List<StoryCategoryDCM> StoryCategoryDCMList { get; set; }
        [DataMember]
        public int count { get; set; }
        [DataMember]
        public int pageRequest { get; set; }
        [DataMember]
        public List<int> idRemove { get; set; }


        public ResponseModel()
        {
            StoryCategoryDCMList = new List<StoryCategoryDCM>();
            idRemove = new List<int>();
        }
    }
}
