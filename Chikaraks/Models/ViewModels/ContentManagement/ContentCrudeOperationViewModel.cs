using ChikaraksServiceContractModels.DomainContractsModel;
using System.Runtime.Serialization;

namespace Chikaraks.Models.ViewModels.ContentManagement
{
    [DataContract]
    public class ContentCrudeOperationViewModel : CRUDViewModel
    {
        [DataMember]
        public ContentDCM ContentDCM { get; set; }

        [DataMember]
        public bool LoadFormFields { get; set; }

        [DataMember]
        public string DateFormat { get; set; }


        public ContentCrudeOperationViewModel()
        {
            ContentDCM = new ContentDCM();
        }
    }
}