using ChikaraksServiceContractModels.DomainContractsModel;
using ChikaraksServiceContractModels.ErrorContactModel;
using LionUtilities.SQLUtilitiesPkg.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChikaraksServiceContractModels.ControllerContractModel.StoryScenePagesCCM
{
    [DataContract]
    public class GetStoryScenePagesResultCCM : ErrorCM
    {
        [DataMember]
        public List<StoryScenePagesDCM> StoryScenePagesDCMList { get; set; }
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

        public GetStoryScenePagesResultCCM()
        {
            StoryScenePagesDCMList = new List<StoryScenePagesDCM>();
            FilterFieldsModel = new List<FilterFieldsModel>();
            Pagination = new PaginationCM();

            SQLDataTypeJosn = JsonConvert.SerializeObject(new SQLDataTypConversionModel());
        }
    }
}
