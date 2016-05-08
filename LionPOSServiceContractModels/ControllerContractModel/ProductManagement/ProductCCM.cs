using LionPOSServiceContractModels.DomainContractsModel.Product;
using LionPOSServiceContractModels.ErrorContactModel;
using LionUtilities.SQLUtilitiesPkg.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.ControllerContractModel.ProductManagement
{
    public class ProductCCM : ErrorCM
    {
        [DataMember]
        public List<productDCM> productList { get; set; }
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
        public string skus { get; set; }


        [DataMember]
        public string InsertActionName { get; set; }

        public ProductCCM()
        {
            FilterFieldsModel = new List<LionUtilities.SQLUtilitiesPkg.Models.FilterFieldsModel>();
            productList = new List<productDCM>();
            Pagination = new PaginationCM();

            SQLDataTypeJosn = JsonConvert.SerializeObject(new SQLDataTypConversionModel());
        }
    }
}
