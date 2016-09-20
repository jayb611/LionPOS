using ChikaraksServiceContractModels.ErrorContactModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChikaraksServiceContractModels.ControllerContractModel.ContentCCM
{
    [DataContract]
    public class SelectedContentActionRequestCCM : ErrorCM
    {
        [DataMember]
        public List<int> IDs { get; set; }

        public SelectedContentActionRequestCCM()
        {
            IDs = new List<int>();
        }
    }
}
