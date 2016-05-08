using LionPOSServiceContractModels;
using LionPOSServiceContractModels.DomainContractsModel.Supplier;

namespace LionPOS.Models.ViewModels.SupplierManagement.Supplier
{
    public class supplierCrudOperationViewModel : CRUDViewModel
    {
        public supplierDCM supplierDCM { get; set; }

        public bool LoadFormFields { get; set; }
        public string DateFormat { get; set; }

        public supplierCrudOperationViewModel()
        {
        }

        public supplierCrudOperationViewModel(SessionCM scm)
        {
            DateFormat = new UtilitiesForAll.FormatHelpers(scm).DateFormatShort();

            supplierDCM = new supplierDCM();
        }


    }
}