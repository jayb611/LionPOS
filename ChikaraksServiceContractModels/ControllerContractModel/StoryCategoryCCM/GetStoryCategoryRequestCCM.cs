using ChikaraksServiceContractModels.DomainContractsModel;
using ChikaraksServiceContractModels.ErrorContactModel;
using LionUtilities.SQLUtilitiesPkg.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.StoryCategoryCCM
{
    [DataContract]
    public class GetStoryCategoryRequestCCM : BasicQueryContractModel
    {
        public GetStoryCategoryRequestCCM()
        {
            LoadAsDefaultFilter = true;
            SaveAsDefaultFilter = false;
        }
       
    }
}
