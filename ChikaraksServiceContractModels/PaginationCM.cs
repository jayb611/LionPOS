using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels
{
    [DataContract]
    public class PaginationCM 
    {
        [DataMember]
        public int RecordsPerPage { get; set; }
        [DataMember]
        public int PageNumber { get; set; }
        [DataMember]
        public int TotalPages { get; set; }
        [DataMember]
        public int TotalRecords { get; set; }
        [DataMember]
        public string FilterActionName { get; set; }
        [DataMember]
        public string controllerName { get; set; }
        [DataMember]
        public string actionName { get; set; }
        [DataMember]
        public string SaveAsDefaultURLLink { get; set; }
        
    }
}