namespace LionPOSServiceContractModels.ConstantDictionaryContractModel
{
    public static class ConstantDictionaryCM
    {
        public const string WebsiteURL_string = "http://localhost:2913";
        public const string LionVisionBranchCode_string = "LionVision";
        public const string LionVisionBranchName_string = "Lion";
        public const string LionVisionAccessRole_string = "Lion Role";
        public const string ApplicationGroupCode = "LO";
        

        public const string ProfilePictureServerMapPath_string = "~/ProfilePictures/";
        public const string ProfilePictureViewPath_string = WebsiteURL_string + "/ProfilePictures/";
        public const string applicationUniqueIdentifier = "LionVision";
        public const string MaintenanceWebsiteURL_string = WebsiteURL_string + "/ErrorHandler/Index/<logid>";
        public const string JsonParamString_string = "JsonParamString";
        public const int recordPerPage_int = 50;
        public const string checkServerErrorLogNo_string = "Check Server Error Log : ";
        public static int StartUpErrorLog_int = 0;
        
        public const string ErrorMessageForUser = "Contact developer as soon as possible,If you are on internet connection then an email is already sent to support team.\n Thank you";

        public static class SyncRoutesTypes
        {
            public const string HO = "HO";
            public const string Cloud = "Cloud";
            public const string Outlet = "Outlet";
        }
        
        public static string currentRouteNodeName = "TeaCafeHeadOffice";
        public static string centralCloudRouteNodeName = "Cloud";


        public const string thisRouteNode = SyncRoutesTypes.HO;
        public const string thisCentralCloudRouteNode = SyncRoutesTypes.Cloud;


        public const string DefaultSelection_string = "--Select--";


        public class contact_type
        {
            public static string HomeLandLine = "Home Land Line";
            public static string OfficeLandLine = "Office Land Line";
            public static string HomeMobile = "Home Mobile";
            public static string OfficeMobile = "Office Mobile";
            public static string NeighboursHome = "Neighbours Home";
            public static string NeighboursMobile = "Neighbours Mobile";
            public static string WhatsApp = "What's App";
            public static string PrimaryMobile = "Primary Mobile";
            public static string PrimaryEmailAddress = "Primary Email Address";
        }


        

        public class gender
        {
            public static string Male = "Male";
            public static string Female = "Female";
        }

        public class married
        {
            public static string Yes = "Yes";
            public static string No = "No";
        }

        public class canBeSold
        {
            public static string Yes = "Yes";
            public static string No = "No";
        }
        public class isProductTax
        {
            public static string Yes = "Yes";
            public static string No = "No";
        }

        public class isCombo
        {
            public static string Yes = "Yes";
            public static string No = "No";
        }

        public class canPurchase
        {
            public static string Yes = "Yes";
            public static string No = "No";
        }

        public class isCategory
        {
            public static string Yes = "Yes";
            public static string No = "No";
        }


        public class castCategory
        {
            public static string General = "General";
            public static string OBC = "OBC";
            public static string SC = "SC";
            public static string ST = "ST";
        }
        public class userStatus
        {
            public const string Offline = "Offline";
            public const string Online = "Online";
        }
        public class AccountStatus
        {
            public const string Blocked = "Blocked";
            public const string Unblocked = "Unblocked";

        }
        public class branchTypes
        {
            public const string ITSupportOffice = "IT Support Office";
            public const string HeadOffice = "Head Office";
            public const string Warehouse = "Warehouse";
            public const string Outlet = "Outlet";
        }
        public class AccountOTPRecoveryTypes
        {
            public const string Mobile = "Mobile";
            public const string Email = "Email";
        }
        public class crudOprationTypes
        {
            public const string Insert = "Insert";
            public const string Update = "Update";
            public const string Delete = "Delete";
        }



        public class DbEmployeeLvitsPos
        {
            public const string name = "EmployeeLvitsPosDb";
            public const string employee = "employee";
            public const string employee_merge = "employee_merge";
        }
        public class DbBranchLvitsPos
        {
            public const string name = "BranchLvitsPosDb";
            public const string branches = "branches";
            public const string branches_payments = "branches_payments";
            public const string employee_underbranch = "employee_underbranch";

            public static class SyncAPI
            {
                public const string APIUrl = "http://localhost:50454/api/";
                public const string NewBranchSync = APIUrl + "SyncBranchCloud/NewBranches";
                public const string NewBranchSyncAcknowledge = APIUrl + "SyncBranchCloud/NewBranchesAcknowledge";
                public const string UpdateBranchSync = APIUrl + "SyncBranchCloud/UpdateBranches";
                public const string UpdateBranchSyncAcknowledge = APIUrl + "SyncBranchCloud/UpdateBranchesAcknowledge";
                public const string DeleteBranchSync = APIUrl + "SyncBranchCloud/DeleteBranches";
                public const string DeleteBranchSyncAcknowledge = APIUrl + "SyncBranchCloud/DeleteBranchesAcknowledge";
            }
        }
        public class DbSupplierLvitsPos
        {
            public const string name = "SupplierLvitsPosDb";
            public const string supplier_invoices = "supplier_invoices";
            public const string supplier_invoices_items = "supplier_invoices_items";
            public const string supplier_payments = "supplier_payments";
            public const string supplier_purchase_orders = "supplier_purchase_orders";
            public const string supplier_purchase_orders_items = "supplier_purchase_orders_items";
            public const string supplier_purchase_return = "supplier_purchase_return";
            public const string supplier_purchase_return_items = "supplier_purchase_return_items";
            public const string suppliers = "suppliers";
            public const string suppliers_merge = "suppliers_merge";
        }


        public class DbMaintenanceLvitsPos
        {
            public const string name = "SupplierLvitsPosDb";
            public const string logs = "logs";
        }
        public class DbConfigurationLvitsPos
        {
            public const string name = "ConfigurationLvitsPosDb";
            public const string country = "country";
            public const string delete_logs = "delete_logs";
            public const string last_change_tables = "last_change_tables";
            public const string settings = "settings";
            public const string state = "state";
        }
        public class DbWarehouseLvitsPos
        {
            public const string name = "WarehouseLvitsPosDb";
            public const string delivery_challan_receive = "delivery_challan_receive";
            public const string delivery_challan_send = "delivery_challan_send";
            public const string warehouse_stock_master = "warehouse_stock_master";
            public const string warehouse_structure = "warehouse_structure";
            public const string warehouses = "warehouses";
            public const string warehouses_merge = "warehouses_merge";
            public const string wearhouse_under_branches = "wearhouse_under_branches";
        }
        public class DbProductLvitsPos
        {
            public const string name = "ProductLvitsPosDb";
            public const string combo_items = "combo_items";
            public const string product_price_master = "product_price_master";
            public const string products = "products";
            public const string products_merge = "products_merge";
            public const string supplier_product_mapping = "supplier_product_mapping";
            public const string unit_of_mesurement = "unit_of_mesurement";
            public const string product_class = "product_class";
            
        }

        public class DbAccountingLvitsPos
        {
            public const string name = "AccountingLvitsPosDb";
            public const string account_entry_note = "account_entry_note";
            public const string account_entry_type = "account_entry_type";
            public const string discount = "discount";
            public const string payment_accounts = "payment_accounts";
            public const string payment_accounts_merge = "payment_accounts_merge";
            public const string recipts = "recipts";
            public const string service_charge = "service_charge";
            public const string tax_details = "tax_details";
            public const string tax_master = "tax_master";
        }

        public class DbPosLvitsPos
        {
            public const string name = "PosLvitsPosDb";
            public const string end_customer = "end_customer";
            public const string live_counter_session = "live_counter_session";
            public const string live_counter_session_items = "live_counter_session_items";
            public const string pos_invoice_return_items = "pos_invoice_return_items";
            public const string pos_invoices = "pos_invoices";
            public const string pos_invoices_items = "pos_invoices_items";
            public const string pos_purchase_orders = "pos_purchase_orders";
            public const string pos_purchase_orders_items = "pos_purchase_orders_items";
            public const string pos_return = "pos_return";
            public const string sales_invoices = "sales_invoices";
            public const string sales_invoices_items = "sales_invoices_items";
            public const string sales_return = "sales_return";
            public const string sales_stock_invoices_return_items = "sales_stock_invoices_return_items";
        }


    }
}