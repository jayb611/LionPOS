using ChikaraksServiceContractModels.DomainContractsModel;
using ChikaraksServiceContractModels.ErrorContactModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChikaraksServiceContractModels.ControllerContractModel.StoryScenePagesCCM
{
    [DataContract]
    public class UpdateStoryScenePageRequestCCM : ErrorCM
    {
        [DataMember]
        public StoryScenePagesDCM StoryScenePagesDCM { get; set; }

        public UpdateStoryScenePageRequestCCM()
        {
            StoryScenePagesDCM = new StoryScenePagesDCM();
        }
    }
}
