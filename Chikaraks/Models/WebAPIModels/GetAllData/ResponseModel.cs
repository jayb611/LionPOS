using ChikaraksServiceContractModels.DomainContractsModel;
using ChikaraksServiceContractModels.ErrorContactModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChikaraksAPI.Models.WebAPIModels.GetAllData
{
    public class ResponseModel : ErrorCM
    {

        [DataMember]
        public List<StoryCategoryDCM> StoryCategoryDCMList { get; set; }
             
    }
}