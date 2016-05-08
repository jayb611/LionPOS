using LionPOSServiceContractModels;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using LionPOSServiceContractModels.DomainContractsModel.Branch;
using LionPOSServiceContractModels.DomainContractsModel.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LionPOS.Models.ViewModels.BranchManagement.Branch
{
    public class branchCrudOperationViewModel : CRUDViewModel
    {
        public branchDCM branchDCM { get; set; }
        public bool LoadFormFields { get; set; }
        public List<SelectListItem> BranchTypes { get; set; }
        public List<SelectListItem> ContactTypes { get; set; }
        public List<SelectListItem> ContactMobile { get; set; }
        public List<CountryDCM> CountryList { get; set; }
        public List<StateDCM> stateList { get; set; }

        public string DateFormat { get; set; }
        public branchCrudOperationViewModel()
        {
        }
        public branchCrudOperationViewModel(SessionCM cm) 
        {

            DateFormat = new UtilitiesForAll.FormatHelpers(cm).DateFormatShort();

            branchDCM = new branchDCM();

            BranchTypes = new List<SelectListItem>();
            SelectListItem bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.branchTypes.HeadOffice;
            bt.Value = bt.Text;
            BranchTypes.Add(bt);

            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.branchTypes.Outlet;
            bt.Value = bt.Text;
            BranchTypes.Add(bt);

            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.branchTypes.Warehouse;
            bt.Value = bt.Text;
            BranchTypes.Add(bt);

            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.branchTypes.ITSupportOffice;
            bt.Value = bt.Text;
            BranchTypes.Add(bt);

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


            CountryList = new List<CountryDCM>();
            stateList = new List<StateDCM>();

        }

    }
}