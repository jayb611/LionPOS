using ChikaraksServiceContractModels.DomainContractsModel;
using System.Runtime.Serialization;

namespace Chikaraks.Models.ViewModels.Assets
{
    [DataContract]
    public class AssetsCrudOperationViewModel : CRUDViewModel
    {
        [DataMember]
        public AssetsDCM AssetsDCM { get; set; }
        [DataMember]
        public bool LoadFormFields { get; set; }
        [DataMember]
        public string DateFormat { get; set; }
    

        public AssetsCrudOperationViewModel()
        {
            AssetsDCM = new AssetsDCM();
        }

    }
}