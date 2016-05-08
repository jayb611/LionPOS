using LionPOSServiceContractModels.DomainContractsModel.Account;
using LionPOSServiceContractModels.ErrorContactModel;
using LionUtilities.SQLUtilitiesPkg.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.ControllerContractModel.AccountManagement
{
    [DataContract]
    public class TaxmasterCCM : ErrorCM
    {
        [DataMember]
        public List<taxmasterDCM> taxmasterList { get; set; }
        [DataMember]
        public PaginationCM Pagination { get; set; }
        [DataMember]
        public string FilterFieldModelJson { get; set; }
        [DataMember]
        public string SQLDataTypeJosn { get; set; }
        [DataMember]
        public List<FilterFieldsModel> FilterFieldsModel { get; set; }


        [DataMember]
        public string bulkActionName { get; set; }
        [DataMember]
        public string taxMasterTitles { get; set; }


        [DataMember]
        public string InsertActionName { get; set; }

        public TaxmasterCCM()
        {
            FilterFieldsModel = new List<LionUtilities.SQLUtilitiesPkg.Models.FilterFieldsModel>();
            taxmasterList = new List<taxmasterDCM>();
            Pagination = new PaginationCM();

            SQLDataTypeJosn = JsonConvert.SerializeObject(new SQLDataTypConversionModel());
        }
    }
}
