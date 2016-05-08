using LionPOSServiceContractModels.ConstantDictionaryContractModel;

namespace LionPOSServiceContractModels.ConstantDictionaryViewModel
{
    public static class ConstantDictionaryVM
    {
        //HC 24-02-2016 Created
       
        public const string branchCodeSession_string = "branchCodeSession_string";
        
        public const string captchaAjaxUrl_string = ConstantDictionaryCM.WebsiteURL_string + "/Login/Captcha";
        public const string LogoutCheckURL_string = ConstantDictionaryCM.WebsiteURL_string + "/Login/LogoutCheck";

        public const string LoginBackgroundServerMapPath_string = "~/Images/Wallpapers/";
        public const string LoginBackgroundViewPath_string = ConstantDictionaryCM.WebsiteURL_string + "/Images/Wallpapers/";
        public const string LoginPageSession_string = "LoginPageSession";
        public const string NeedHelpPageSession_string = "NeedHelpPageSession";
        public const string CaptchaValueSession_string = "CaptchaValue";
        public const string RememberMe_string = "SSID";
        public const string PreviousAccountsLogin_string = "PreviousAccountLogin";
        public const string MainSession_string = "MainSession";
        public const string RecoverySession_string = "RecoverySession_string";
        public const string SupportContactDetails_string = "support@lionvisionits.com , (M):-+91 99 796 08294";
        public const string ErrorMessageForUser = "Contact developer as soon as possible,If you are on internet connection then an email is already sent to support team.\n Thank you";
        public const string DeveloperContactUsWebsite_string = "http://www.lionvisionits.com/ContactUs.aspx";
        public const string PasswordReset_Email_Template_File_Path = "~/Views/Shared/PasswordResetTemplate.cshtml";
        
       
       
        public const string OTPSessionValue_string = "OTPSessionValue";
        public class RecoveryOption
        {
            public const string RecoverLostPassword_string = "RecoverLostPassword";
            public const string RecoverLostUsername_string = "RecoverLostUsername";
            public const string OtherQueries_string = "OtherQueries";
        }
       
        public class AlertCssModel
        {
            public  class AlertTypesCssClass
            {
                public const string primary = "alert-primary";
                public const string success = "alert-success";
                public const string information = "alert-info";
                public const string warning = "alert-warning";
                public const string danger = "alert-danger";
            }
            //Demo
            //<div class="col-xs-12 text-center">
            //    <div class="alert alert-warning">
            //        <a class="close" data-dismiss="alert">×</a>
            //        <strong>Oh Snap!</strong> Change a few things up and try submitting again.
            //    </div>
            //</div>
            public string buildAlertString(string type, string message, bool dismissable)
            {
                return (@"<div class='col-xs-12 text-center'> " +
                            "<div class='alert " + type + "' > " +
                                  ((dismissable) ? "<a class='close' data-dismiss='alert'>×</a>" : "") +
                                        "<strong>Oh Snap!</strong>" + message +
                              "</div>" +
                        "</div>");
            }
        }
    }
}