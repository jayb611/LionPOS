using LionPOSServiceContractModels.DomainContractsModel.User;
using LionPOSServiceContractModels.ErrorContactModel;
using LionUtilities.SQLUtilitiesPkg.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.ControllerContractModel.UserManagement
{
    [DataContract]
    public class UserCCM : ErrorCM
    {
        [DataMember]
        public List<UserDCM> userList { get; set; }
        [DataMember]
        public PaginationCM Pagination { get; set; }
        [DataMember]
        public string FilterFieldModelJson { get; set; }
        [DataMember]
        public string SQLDataTypeJosn { get; set; }
        [DataMember]
        public List<FilterFieldsModel> FilterFieldsModel { get; set; }


        [DataMember]
        public string bulkActionName { get; set; }
        [DataMember]
        public string userNames { get; set; }


        [DataMember]
        public string InsertActionName { get; set; }

        public UserCCM()
        {
            FilterFieldsModel = new List<LionUtilities.SQLUtilitiesPkg.Models.FilterFieldsModel>();
            userList = new List<UserDCM>();
            Pagination = new PaginationCM();

            SQLDataTypeJosn = JsonConvert.SerializeObject(new SQLDataTypConversionModel());
        }
    }
}
