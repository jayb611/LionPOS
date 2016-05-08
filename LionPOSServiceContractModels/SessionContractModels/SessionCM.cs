using LionPOSServiceContractModels.ErrorContactModel;

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels
{
    [DataContract]
    public class SessionCM : ErrorCM
    {
        [DataMember]
        public List<settingSCM> settings { get; set; }
        [DataMember]
        public List<access_areaSCM> access_areas { get; set; }
        [DataMember]
        public List<access_role_in_access_areaSCM> access_role_in_access_area { get; set; }
        //Branch
        [DataMember]
        public branchSCM branch { get; set; }

        //Employee
        [DataMember]
        public employeeSCM employee { get; set; }

        //SessionDetails
        [DataMember]
        public bool isAuthorised { get; set; }

        //Userdetails
        [DataMember]
        public userSCM user { get; set; }

        [DataMember]
        public List<override_access_roleSCM> override_access_role { get; set; }

        [DataMember]
        public List<user_settingsSCM> user_settings { get; set; }
        public SessionCM()
        {
            settings = new List<settingSCM>();
            access_areas = new List<access_areaSCM>();
            access_role_in_access_area = new List<access_role_in_access_areaSCM>();
            branch = new branchSCM();
            employee = new employeeSCM();
            user = new userSCM();
            override_access_role = new List<override_access_roleSCM>();
            user_settings = new List<user_settingsSCM>();
        }
    }
}