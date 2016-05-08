using LionPOSServiceContractModels.DomainContractsModel.Branch;
using LionPOSServiceContractModels.DomainContractsModel.Configuration;
using LionPOSServiceContractModels.ErrorContactModel;
using LionUtilities.SQLUtilitiesPkg.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.ControllerContractModel.BranchManagement
{
    [DataContract]
    public class BranchCCM : ErrorCM
    {
        [DataMember]
        public List<branchDCM> branchList { get; set; }
        [DataMember]
        public PaginationCM Pagination { get; set; }
        [DataMember]
        public string FilterFieldModelJson { get; set; }
        [DataMember]
        public string SQLDataTypeJosn { get; set; }
        [DataMember]
        public List<FilterFieldsModel> FilterFieldsModel { get; set; }

        [DataMember]
        public string branchCodes { get; set; }

        [DataMember]
        public string bulkActionName { get; set; }
       

        [DataMember]
        public string InsertActionName { get; set; }


      

        public BranchCCM()
        {
            FilterFieldsModel = new List<LionUtilities.SQLUtilitiesPkg.Models.FilterFieldsModel>();
            branchList = new List<branchDCM>();
            Pagination = new PaginationCM();

            SQLDataTypeJosn = JsonConvert.SerializeObject(new SQLDataTypConversionModel());
        }
    }
}