using LionPOSDbContracts.DomainModels.User;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace LionPOS.Models.ValidationModels.userDB
{
    [MetadataType(typeof(access_role_in_access_areaEntityValidationModel))]
    public partial class access_role_in_access_area
    {
    }
    [Serializable]
    [DataContract]
    public class access_role_in_access_areaEntityValidationModel
    {
        [DataMember]
        public string domain { get; set; }
        [DataMember]
        public string controller { get; set; }
        [DataMember]
        public string view { get; set; }
        [DataMember]
        public string accessRoleTitle { get; set; }
        [DataMember]
        public bool canAccess { get; set; }
        [DataMember]
        public bool visible { get; set; }
        [DataMember]
        public bool underMaintenance { get; set; }
        [DataMember]
        public bool showBranchWise { get; set; }
        [DataMember]
        public string remarks { get; set; }
        [DataMember]
        public bool isActive { get; set; }
        [DataMember]
        public bool isDeleted { get; set; }
        [DataMember]
        public System.DateTime entryDate { get; set; }
        [DataMember]
        public System.DateTime lastChangeDate { get; set; }
        [DataMember]
        public bool recordByLion { get; set; }
        [DataMember]
        public string entryByUserName { get; set; }
        [DataMember]
        public string changeByUserName { get; set; }
        [DataMember]
        public string entryBranchCode { get; set; }
        [DataMember]
        public string insertRoutePoint { get; set; }
        [DataMember]
        public string updateRoutePoint { get; set; }
        [DataMember]
        public string syncGUID { get; set; }


        public virtual access_area access_area { get; set; }
        public virtual access_role_master access_role_master { get; set; }
    }  
}