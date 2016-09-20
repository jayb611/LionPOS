using LionPOSServiceContractModels.DomainContractsModel.Chikaraks;
using System.Runtime.Serialization;

namespace Chikaraks.Models.ViewModels.StoryAreaType
{
    [DataContract]
    public class StoryAreaTypeCrudeOperationViewModel : CRUDViewModel
    {
        [DataMember]
        public StoryAreaTypeDCM StoryAreaTypeDCM { get; set; }

        [DataMember]
        public bool LoadFormFields { get; set; }


        public StoryAreaTypeCrudeOperationViewModel()
        {
            StoryAreaTypeDCM = new StoryAreaTypeDCM();
        }
    }
}