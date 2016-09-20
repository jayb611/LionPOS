using ChikaraksServiceContractModels;
using ChikaraksServiceContractModels.DomainContractsModel;
using ChikaraksServiceContractModels.ErrorContactModel;
using LionUtilities.SQLUtilitiesPkg.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.AssetsCCM
{
    [DataContract]
    public class GetAssetsResultCCM : ErrorCM
    {
        [DataMember]
        public string SSID{ get; set; }
        [DataMember]
        public List<AssetsDCM> AssetsList { get; set; }
        [DataMember]
        public PaginationCM Pagination { get; set; }
        [DataMember]
        public string FilterFieldModelJson { get; set; }
        [DataMember]
        public string SQLDataTypeJosn { get; set; }
        [DataMember]
        public List<FilterFieldsModel> FilterFieldsModel { get; set; }
        [DataMember]
        public string BulkActionName { get; set; }
        [DataMember]
        public string InsertActionName { get; set; }
        [DataMember]
        public string DetailsActionName { get; set; }
        public GetAssetsResultCCM()
        {
            FilterFieldsModel = new List<FilterFieldsModel>();
            AssetsList = new List<AssetsDCM>();
            Pagination = new PaginationCM();
            SQLDataTypeJosn = JsonConvert.SerializeObject(new SQLDataTypConversionModel());
        }
    }
}
