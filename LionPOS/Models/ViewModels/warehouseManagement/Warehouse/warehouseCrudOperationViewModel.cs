using LionPOSServiceContractModels;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using LionPOSServiceContractModels.DomainContractsModel.warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LionPOS.Models.ViewModels.warehouseManagement.Warehouse
{
    public class warehouseCrudOperationViewModel : CRUDViewModel
    {
        public warehouseDCM warehouseDCM { get; set; }

        public bool LoadFormFields { get; set; }

        public List<SelectListItem> ContactTypes { get; set; }

        public List<SelectListItem> ContactMobile { get; set; }

        public string DateFormat { get; set; }

        public warehouseCrudOperationViewModel()
        {
        }

        public warehouseCrudOperationViewModel(SessionCM cm)
        {

            DateFormat = new UtilitiesForAll.FormatHelpers(cm).DateFormatShort();
            warehouseDCM = new warehouseDCM();
            SelectListItem bt = new SelectListItem();

            ContactTypes = new List<SelectListItem>();
            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.contact_type.HomeLandLine;
            bt.Value = bt.Text;
            ContactTypes.Add(bt);

            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.contact_type.OfficeLandLine;
            bt.Value = bt.Text;
            ContactTypes.Add(bt);

            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.contact_type.HomeMobile;
            bt.Value = bt.Text;
            ContactTypes.Add(bt);

            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.contact_type.OfficeMobile;
            bt.Value = bt.Text;
            ContactTypes.Add(bt);

            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.contact_type.NeighboursHome;
            bt.Value = bt.Text;
            ContactTypes.Add(bt);

            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.contact_type.NeighboursMobile;
            bt.Value = bt.Text;
            ContactTypes.Add(bt);

            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.contact_type.WhatsApp;
            bt.Value = bt.Text;
            ContactTypes.Add(bt);

            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.contact_type.PrimaryMobile;
            bt.Value = bt.Text;
            ContactTypes.Add(bt);
            
            ContactMobile = new List<SelectListItem>();
            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.contact_type.PrimaryMobile;
            bt.Value = bt.Text;
            bt.Selected = true;
            ContactMobile.Add(bt);
        }
    }
}