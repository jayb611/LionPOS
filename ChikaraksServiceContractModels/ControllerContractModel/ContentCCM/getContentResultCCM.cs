using ChikaraksServiceContractModels.DomainContractsModel;
using ChikaraksServiceContractModels.ErrorContactModel;
using LionUtilities.SQLUtilitiesPkg.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.ContentCCM
{
    [DataContract]
    public class GetContentResultCCM : ErrorCM
    {
        [DataMember]
        public List<ContentDCM> ContentDCMList { get; set; }
        [DataMember]
        public PaginationCM Pagination { get; set; }
        [DataMember]
        public string SQLDataTypeJosn { get; set; }
        [DataMember]
        public string BulkActionName { get; set; }
        [DataMember]
        public string InsertActionName { get; set; }
        [DataMember]
        public string DetailsActionName { get; set; }
        [DataMember]
        public string DeleteActionName { get; set; }

        public GetContentResultCCM()
        {
            ContentDCMList = new List<ContentDCM>();
            Pagination = new PaginationCM();
            SQLDataTypeJosn = JsonConvert.SerializeObject(new SQLDataTypConversionModel());
        }
    }
}
