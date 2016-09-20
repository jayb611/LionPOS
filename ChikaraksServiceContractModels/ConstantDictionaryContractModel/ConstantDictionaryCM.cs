namespace ChikaraksServiceContractModels.ConstantDictionaryContractModel
{
    public static class ConstantDictionaryCM
    {
        public const string WebsiteURL_string = "http://chikaraks.lionvisionits.com";
        
        
        public const string ProfilePictureServerMapPath_string = "~/ProfilePictures/";
        public const string AudioServerMapPath_string = "~/Audio/";
        public const string AudioViewPath_string = WebsiteURL_string + "/Audio/";
        public const string ProfilePictureViewPath_string = WebsiteURL_string + "/ProfilePictures/";
        public const string BranchIconViewPath_string = WebsiteURL_string + "/Images/";
        public const int sessionExpiryTimeout = 10000;
        public const string MaintenanceWebsiteURL_string = WebsiteURL_string + "/ErrorHandler/Index/<logid>";
        public const string JsonParamString_string = "JsonParamString";
        public const int recordPerPage_int = 50;
        public const string checkServerErrorLogNo_string = "Check Server Error Log : ";
        public static int StartUpErrorLog_int = 0;
        public static int captachCount = 5;
        public static int blockCount = 10;
        public static int UnblockSecound = 0;
        
        public const string ErrorMessageForUser = "Contact developer as soon as possible,If you are on internet connection then an email is already sent to support team.\n Thank you";


        public static class BulkActionType
        {
            public const string BulkActionDeleteSelected_string = "Delete Selected";
            public const string BulkActionSetActiveToSelected_string = "Set Active to selected";
            public const string BulkActionSetInactiveToSelected_string = "Set Inactive to selected";
        }

        public class AccountStatus
        {
            public const string Blocked = "Blocked";
            public const string Unblocked = "Unblocked";

        }
        public static class SmallImage
        {
            public const int width_int = 100;
            public const int height_int = 100;
            public const int quality_int = 100;
        }

        public const string keysSeparater_string = "_";
        public const string keysListSeparater_string = ",";
        public const string keysDeleteLogSeparater_string = "|";
        public const string DefaultSelection_string = "--Select--";
     
        public class gender
        {
            public static string Male = "Male";
            public static string Female = "Female";
        }
      
        public class ImageStoreType
        {
            public static string url_string = "url";
            public static string base64_string = "base64";
        }
      
        public class crudOprationTypes
        {
            public const string Insert = "Insert";
            public const string Update = "Update";
            public const string Delete = "Delete";
        }

        public class YesNo
        {
            public static string Yes = "Yes";
            public static string No = "No";
        }

    }
}