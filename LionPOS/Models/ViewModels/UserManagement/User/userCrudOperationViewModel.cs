using LionPOSServiceContractModels;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using LionPOSServiceContractModels.DomainContractsModel.User;
using System.Collections.Generic;
using System.Web.Mvc;

namespace LionPOS.Models.ViewModels.UserManagement.User
{
    public class userCrudOperationViewModel : CRUDViewModel
    {
        public UserDCM UserDCM { get; set; }

        public bool LoadFormFields { get; set; }

        public string DateFormat { get; set; }

        public List<SelectListItem> userStatus { get; set; }

        public List<SelectListItem> AccountStatus { get; set; }

        public userCrudOperationViewModel() { }

        public userCrudOperationViewModel(SessionCM cm)
        {
            UserDCM = new UserDCM();

            DateFormat = new UtilitiesForAll.FormatHelpers(cm).DateFormatShort();

            userStatus = new List<SelectListItem>();
            SelectListItem ut = new SelectListItem();
            ut.Text = ConstantDictionaryCM.userStatus.Online;
            ut.Value = ut.Text;
            userStatus.Add(ut);

            ut = new SelectListItem();
            ut.Text = ConstantDictionaryCM.userStatus.Offline;
            ut.Value = ut.Text;
            userStatus.Add(ut);

            AccountStatus = new List<SelectListItem>();
            ut = new SelectListItem();
            ut.Text = ConstantDictionaryCM.AccountStatus.Blocked;
            ut.Value = ut.Text;
            AccountStatus.Add(ut);


            ut = new SelectListItem();
            ut.Text = ConstantDictionaryCM.AccountStatus.Unblocked;
            ut.Value = ut.Text;
            AccountStatus.Add(ut);


        }

    }
}