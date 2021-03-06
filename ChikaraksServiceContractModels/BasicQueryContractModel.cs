﻿using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels
{
    [DataContract]
    public class BasicQueryContractModel
    {
        [DataMember]
        public string SSID { get; set; }
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
        [DataMember]
        public bool showBranchWise { get; set; }

        public BasicQueryContractModel()
        {
            sessionObj = new SessionCM();
            pageNumber = 1;
            recordPerPage = 7;
            FilterFieldsContractModel = new FilterFieldsContractModel();
        }

    }
}