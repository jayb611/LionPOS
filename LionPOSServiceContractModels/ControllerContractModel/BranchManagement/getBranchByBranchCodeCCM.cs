using LionPOSServiceContractModels.DomainContractsModel.Branch;
using LionPOSServiceContractModels.DomainContractsModel.Configuration;
using LionPOSServiceContractModels.ErrorContactModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LionPOSServiceContractModels.ControllerContractModel.BranchManagement
{
   public class getBranchByBranchCodeCCM :ErrorCM
    {
        [DataMember]
        public branchDCM branchDCM { get; set; }
        [DataMember]
        public List<CountryDCM> CountryList { get; set; }
        [DataMember]
        public List<StateDCM> StateList { get; set; }

        public getBranchByBranchCodeCCM()
        {
            CountryList = new List<CountryDCM>();
            StateList = new List<StateDCM>();
        }


    }
}
