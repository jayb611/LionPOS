using LionPOSServiceContractModels;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using LionPOSServiceContractModels.DomainContractsModel.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace LionPOS.Models.ViewModels.AccountManagement.Taxmaster
{
    public class taxmasterCrudOperationViewModel : CRUDViewModel
    {
        public taxmasterDCM taxmasterDCM { get; set; }
        public bool LoadFormFields { get; set; }
        public List<SelectListItem> IsProductTax { get; set; }
        public string DateFormat { get; set; }
        public taxmasterCrudOperationViewModel()
        {
        }
        public taxmasterCrudOperationViewModel(SessionCM cm)
        {

            DateFormat = new UtilitiesForAll.FormatHelpers(cm).DateFormatShort();

            taxmasterDCM = new taxmasterDCM();
            SelectListItem bt = new SelectListItem();

            IsProductTax = new List<SelectListItem>();
            bt.Text = ConstantDictionaryCM.isProductTax.Yes;
            bt.Value = "True";
            bt.Selected = true;
            IsProductTax.Add(bt);

            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.isProductTax.No;
            bt.Value = "False";
            IsProductTax.Add(bt);
        }
    }
}