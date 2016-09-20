using ChikaraksServiceContractModels.DomainContractsModel;
using ChikaraksServiceContractModels.ErrorContactModel;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.StoryScenePagesCCM
{
    [DataContract]
    public class GetStoryScenePageByPrimaryKeysResultCCM : ErrorCM
    {
        [DataMember]
        public StoryScenePagesDCM StoryScenePagesDCM { get; set; }

        public GetStoryScenePageByPrimaryKeysResultCCM()
        {
            StoryScenePagesDCM = new StoryScenePagesDCM();
        }
    }
}
