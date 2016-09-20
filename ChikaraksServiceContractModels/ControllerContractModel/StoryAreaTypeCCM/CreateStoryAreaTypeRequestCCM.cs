using LionPOSServiceContractModels.DomainContractsModel.Chikaraks;
using LionPOSServiceContractModels.ErrorContactModel;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.ControllerContractModel.StoryAreaTypeCCM
{
    [DataContract]
    public class CreateStoryAreaTypeRequestCCM : ErrorCM
    {
        [DataMember]
        public StoryAreaTypeDCM StoryAreaTypeDCM { get; set; }

        public CreateStoryAreaTypeRequestCCM()
        {
            StoryAreaTypeDCM = new StoryAreaTypeDCM();
        }
    }
}
