using LionPOSServiceContractModels.ErrorContactModel;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.ControllerContractModel.StoryAreaTypeCCM
{
    [DataContract]
    public class GetStoryAreaTypeByStoryAreaTypeIDRequestCCM : ErrorCM
    {
        [DataMember]
        public int idStoryAreaType { get; set; }
    }
}
