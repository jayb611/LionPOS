using ChikaraksServiceContractModels.ErrorContactModel;
using LionUtilities.SQLUtilitiesPkg.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels
{
    [DataContract]
    public class FilterFieldsContractModel : ErrorCM
    {
        [DataMember]
        public List<FilterFieldsModel> FilterFieldsModel { get; set; }
        public FilterFieldsContractModel()
        {
            FilterFieldsModel = new List<LionUtilities.SQLUtilitiesPkg.Models.FilterFieldsModel>();
        }
    }
}