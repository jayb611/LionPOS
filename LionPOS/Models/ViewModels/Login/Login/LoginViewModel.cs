using LionPOS.Models.BackEndModels.Login.Login;
using LionPOSServiceContractModels.DomainContractsModel.Branch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LionPOS.Models.ViewModels.Login.Login
{
    public class LoginViewModel
    {
        public bool? CaptchaActivated_Null_bool { get; set; }
        
        public List<string> AlertList_string { get; set; }
        public string RandomImageStyleURL_string { get; set; }
        public string quote { get; set; }
        public string quoteAuthor { get; set; }
        public string quoteAd { get; set; }
        public string quoteImage { get; set; }
        public string quoteLogo { get; set; }
        public string captchaString { get; set; }
        public bool showLoginWindowDirect { get; set; }

        public string username { get; set; }
        public string password { get; set; }
        public string captcha { get; set; }
        public bool remember { get; set; }

        public string branchCode { get; set; }

        public List<branchDCM> branchDCMList { get; set; }
        public List<PreviousAccountsCookiesBackEndModel> previousAccountsLogins { get; set; }




        public LoginViewModel()
        {
            AlertList_string = new List<string>();
            CaptchaActivated_Null_bool = false;
            quote = "It is in vain to say human beings ought to be satisfied with tranquillity: they must have action; and they will make it if they cannot find it.";
            quoteAuthor = "Charlotte Brontë";
            quoteAd = "A.D 1984";
            quoteImage = "../Images/Wallpapers/india-fantasy-wallpaper-26750-27466-hd-wallpapers.jpg";
            quoteLogo = "../Images/user.png";
            branchDCMList = new List<branchDCM>();
        }
    }
}