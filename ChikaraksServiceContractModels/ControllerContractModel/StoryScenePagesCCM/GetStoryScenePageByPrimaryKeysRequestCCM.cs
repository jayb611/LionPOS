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
    public class GetStoryScenePageByPrimaryKeysRequestCCM : ErrorCM
    {
        [DataMember]
        public int idStoryCategory { get; set; }
        [DataMember]
        public int idStoryScenePage { get; set; }
    }
}
