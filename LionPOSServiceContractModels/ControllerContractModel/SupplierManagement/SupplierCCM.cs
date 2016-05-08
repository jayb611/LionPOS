using LionPOSServiceContractModels.DomainContractsModel.Supplier;
using LionPOSServiceContractModels.ErrorContactModel;
using LionUtilities.SQLUtilitiesPkg.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.ControllerContractModel.SupplierManagement
{
    [DataContract]
    public class SupplierCCM : ErrorCM
    {
        [DataMember]
        public List<supplierDCM> supplierList { get; set; }

        [DataMember]
        public PaginationCM Pagination { get; set; }

        [DataMember]
        public string FilterFieldModelJson { get; set; }

        [DataMember]
        public string SQLDataTypeJosn { get; set; }

        [DataMember]
        public List<FilterFieldsModel> FilterFieldsModel { get; set; }

        [DataMember]
        public string InsertActionName { get; set; }

        [DataMember]
        public string bulkActionName { get; set; }

        [DataMember]
        public string supplierCodes { get; set; }


        public SupplierCCM()
        {
            supplierList = new List<supplierDCM>();
            Pagination = new PaginationCM();
            SQLDataTypeJosn = JsonConvert.SerializeObject(new SQLDataTypConversionModel());
            FilterFieldsModel = new List<FilterFieldsModel>();
        }
    }
}
