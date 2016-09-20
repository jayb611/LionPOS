using ChikaraksServiceContractModels.DomainContractsModel;
using ChikaraksServiceContractModels.ErrorContactModel;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.StoryScenePagesCCM
{
    [DataContract]
    public class CreateStoryScenePageResultCCM : ErrorCM
    {
        [DataMember]
        public int idStoryScenePages { get; set; }

        public CreateStoryScenePageResultCCM()
        {


        }
    }
}
