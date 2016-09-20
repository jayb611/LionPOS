using LionPOSServiceContractModels.ErrorContactModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LionPOSServiceContractModels.ControllerContractModel.StoryAreaTypeCCM
{
    [DataContract]
    public class DeleteSToryAreaTypeRequestCCM : ErrorCM
    {
        [DataMember]
        public int IDStoryAreaType { get; set; }
    }
}
