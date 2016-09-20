using ChikaraksServiceContractModels.DomainContractsModel;
using ChikaraksServiceContractModels.ErrorContactModel;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.StoryScenePagesCCM
{
    [DataContract]
    public class CreateStoryScenePageRequestCCM : ErrorCM
    {
        [DataMember]
        public StoryScenePagesDCM StoryScenePagesDCM { get; set; }

        public CreateStoryScenePageRequestCCM()
        {
            StoryScenePagesDCM = new StoryScenePagesDCM();

        }

    }
}
