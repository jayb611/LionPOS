using ChikaraksServiceContractModels.ErrorContactModel;

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels
{
    [DataContract]
    public class SessionCM : ErrorCM
    {
        
      

        //SessionDetails
        [DataMember]
        public bool isAuthorised { get; set; }

        //Userdetails
        [DataMember]
        public userSCM user { get; set; }

      
        public SessionCM()
        {
      
            user = new userSCM();
      
        }
    }
}