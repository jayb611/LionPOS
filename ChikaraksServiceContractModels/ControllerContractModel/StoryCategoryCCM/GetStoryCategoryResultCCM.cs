using ChikaraksServiceContractModels.DomainContractsModel;
using ChikaraksServiceContractModels.ErrorContactModel;
using LionUtilities.SQLUtilitiesPkg.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.StoryCategoryCCM
{
    [DataContract]
    public class GetStoryCategoryResultCCM : ErrorCM
    {
        

        [DataMember]
        public List<StoryCategoryDCM> StoryCategoryDCMList { get; set; }

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


        public GetStoryCategoryResultCCM()
        {
            FilterFieldsModel = new List<FilterFieldsModel>();
            StoryCategoryDCMList = new List<StoryCategoryDCM>();
            Pagination = new PaginationCM();

            SQLDataTypeJosn = JsonConvert.SerializeObject(new SQLDataTypConversionModel());
        }
    }
}
