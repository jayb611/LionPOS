using LionPOSServiceContractModels.DomainContractsModel.Chikaraks;
using LionPOSServiceContractModels.ErrorContactModel;
using LionUtilities.SQLUtilitiesPkg.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.ControllerContractModel.StoryAreaTypeCCM
{
    [DataContract]
    public class GetStoryAreaTypeResultCCM : ErrorCM
    {
        [DataMember]
        public List<StoryAreaTypeDCM> StoryAreaTypeDCMList { get; set; }
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

        public GetStoryAreaTypeResultCCM()
        {
            StoryAreaTypeDCMList = new List<StoryAreaTypeDCM>();
            Pagination = new PaginationCM();
            SQLDataTypeJosn = JsonConvert.SerializeObject(new SQLDataTypConversionModel());
        }

    }
}
