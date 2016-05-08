using System.Runtime.Serialization;

namespace LionPOSServiceContractModels
{
    [DataContract]
    public class BasicQueryContractModel
    {
        [DataMember]
        public bool showBranchWise { get; set; }
        [DataMember]
        public string OrderByFields { get; set; }
        [DataMember]
        public string FilterFieldAndValues { get; set; }
        [DataMember]
        public SessionCM sessionObj { get; set; }
        [DataMember]
        public int pageNumber { get; set; }
        [DataMember]
        public int recordPerPage { get; set; }
        [DataMember]
        public bool SaveAsDefaultFilter { get; set; }
        [DataMember]
        public bool LoadAsDefaultFilter { get; set; }
        [DataMember]
        public FilterFieldsContractModel FilterFieldsContractModel { get; set; }

        public BasicQueryContractModel()
        {
            sessionObj = new SessionCM();
            pageNumber = 1;
            recordPerPage = 7;
            FilterFieldsContractModel = new FilterFieldsContractModel();
        }

    }
}