
using ChikaraksServiceContractModels.ErrorContactModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace KakubhaiAndSonsServiceContractModels.ControllerContractModel.WebAPIModels.GetDataByType
{
    [DataContract]
    public class RequestModel : ErrorCM
    {
        [DataMember]
        public int pageRequest { get; set; }
        [DataMember]
        public int pageSize { get; set; }
        [DataMember]
        public List<int> idExists { get; set; }
        [DataMember]
        public string type{ get; set; }


        public RequestModel()
        {
            idExists = new List<int>();
        }
    }
}
