using LionPOSServiceContractModels.DomainContractsModel.Employee;
using LionPOSServiceContractModels.ErrorContactModel;
using LionUtilities.SQLUtilitiesPkg.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.ControllerContractModel.EmployeeManagement
{
    [DataContract]
    public class EmployeeCCM : ErrorCM
    {
        [DataMember]
        public List<employeeDCM> employeeList { get; set; }

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
        public string employeeCodes { get; set; }

        [DataMember]
        public string InsertActionName { get; set; }

        public EmployeeCCM()
        {
            FilterFieldsModel = new List<LionUtilities.SQLUtilitiesPkg.Models.FilterFieldsModel>();
            employeeList = new List<employeeDCM>();
            Pagination = new PaginationCM();

            SQLDataTypeJosn = JsonConvert.SerializeObject(new SQLDataTypConversionModel());
        }
    }
}
