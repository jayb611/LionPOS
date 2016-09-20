using LionPOSServiceContractModels.DomainContractsModel.Chikaraks;
using LionPOSServiceContractModels.ErrorContactModel;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.ControllerContractModel.StoryAreaTypeCCM
{
    [DataContract]
    public class GetStoryAreaTypeByStoryAreaTypeIDResultCCM : ErrorCM
    {
        [DataMember]
        public StoryAreaTypeDCM StoryAreaTypeDCM { get; set; }

        public GetStoryAreaTypeByStoryAreaTypeIDResultCCM()
        {
            StoryAreaTypeDCM = new StoryAreaTypeDCM();
        }
    }
}
