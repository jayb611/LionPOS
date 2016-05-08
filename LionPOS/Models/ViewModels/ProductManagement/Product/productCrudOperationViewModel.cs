using LionPOSServiceContractModels;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using LionPOSServiceContractModels.DomainContractsModel.Product;
using LionPOSServiceContractModels.DomainContractsModel.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LionPOS.Models.ViewModels.ProductManagement.Product
{
    public class productCrudOperationViewModel : CRUDViewModel
    {
        public productDCM productDCM { get; set; }
        public bool LoadFormFields { get; set; }
        public string DateFormat { get; set; }
        public List<SelectListItem> CanBeSold { get; set; }
        public List<SelectListItem> IsCombo { get; set; }
        public List<SelectListItem> CanPurchase { get; set; }
        public List<SelectListItem> IsCategory { get; set; }

        public productCrudOperationViewModel()
        {
        }
        public productCrudOperationViewModel(SessionCM cm)
        {
            DateFormat = new UtilitiesForAll.FormatHelpers(cm).DateFormatShort();

            productDCM = new productDCM();

            SelectListItem bt = new SelectListItem();

            CanBeSold = new List<SelectListItem>();
            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.canBeSold.Yes;
            bt.Value = "True";
            bt.Selected = true;
            CanBeSold.Add(bt);

            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.canBeSold.No;
            bt.Value = "False";
            CanBeSold.Add(bt);

            IsCombo = new List<SelectListItem>();
            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.isCombo.Yes;
            bt.Value = "True";
            bt.Selected = true;
            IsCombo.Add(bt);

            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.isCombo.No;
            bt.Value = "False";
            IsCombo.Add(bt);

            IsCategory = new List<SelectListItem>();
            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.isCategory.Yes;
            bt.Value = "True";
            bt.Selected = true;
            IsCategory.Add(bt);

            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.isCategory.No;
            bt.Value = "False";
            IsCategory.Add(bt);

            CanPurchase = new List<SelectListItem>();
            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.canPurchase.Yes;
            bt.Value = "True";
            bt.Selected = true;
            CanPurchase.Add(bt);

            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.canPurchase.No;
            bt.Value = "False";
            CanPurchase.Add(bt);
        }
    }
}