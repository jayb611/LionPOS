using LionPOSServiceContractModels.DomainContractsModel.Chikaraks;
using LionPOSServiceContractModels.ErrorContactModel;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.ControllerContractModel.StoryAreaTypeCCM
{
    [DataContract]
    public class UpdateStoryAreaTypeRequestCCM : ErrorCM
    {
        [DataMember]
        public StoryAreaTypeDCM StoryAreaTypeDCM { get; set; }


        public UpdateStoryAreaTypeRequestCCM()
        {
            StoryAreaTypeDCM = new StoryAreaTypeDCM();
        }
    }
}
