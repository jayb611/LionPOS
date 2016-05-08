using LionPOSServiceContractModels;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using LionPOSServiceContractModels.DomainContractsModel.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LionPOS.Models.ViewModels.EmployeeManagement.Employee
{
    public class employeeCrudOperationViewModel : CRUDViewModel
    {
        public employeeDCM employeeDCM { get; set; }
        public bool LoadFormFields { get; set; }

        public List<SelectListItem> ContactTypes { get; set; }
        public List<SelectListItem> ContactMobile { get; set; }

        public List<SelectListItem> Gender { get; set; }

        public List<SelectListItem> Married { get; set; }

        public List<SelectListItem> CastCategory { get; set; }

        public string DateFormat { get; set; }

        public employeeCrudOperationViewModel()
        {
        }
        public employeeCrudOperationViewModel(SessionCM cm)
        {
            DateFormat = new UtilitiesForAll.FormatHelpers(cm).DateFormatShort();

            employeeDCM = new employeeDCM();
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

            Gender = new List<SelectListItem>();
            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.gender.Male;
            bt.Value = "True";
            bt.Selected = true;
            Gender.Add(bt);

            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.gender.Female;
            bt.Value = "False";
            Gender.Add(bt);

            Married = new List<SelectListItem>();
            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.married.Yes;
            bt.Value = "True";
            bt.Selected = true;
            Married.Add(bt);

            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.married.No;
            bt.Value = "False";
            Married.Add(bt);

            CastCategory = new List<SelectListItem>();
            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.castCategory.General;
            bt.Value = bt.Text;
            bt.Selected = true;
            CastCategory.Add(bt);

            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.castCategory.OBC;
            bt.Value = bt.Text;
            CastCategory.Add(bt);

            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.castCategory.SC;
            bt.Value = bt.Text;
            CastCategory.Add(bt);

            bt = new SelectListItem();
            bt.Text = ConstantDictionaryCM.castCategory.ST;
            bt.Value = bt.Text;
            CastCategory.Add(bt);
        }
    }
}