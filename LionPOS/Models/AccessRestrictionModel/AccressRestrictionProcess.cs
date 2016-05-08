using System.Linq;
using LionPOSServiceContractModels;

namespace LionPOS.Models.AccessRestrictionModel
{
   


    public class AccressRestrictionProcess
    {
    
        public access_role_in_access_areaSCM isAuthorised(string domain, string controller, string view, SessionCM sm)
        {

            if (sm.access_role_in_access_area != null)
            {
                return sm.access_role_in_access_area.Where(a =>
                a.domain.ToLower().Trim() == domain.ToLower().Trim() &&
                a.controller.ToLower().Trim() == controller.ToLower().Trim() &&
                a.view.ToLower().Trim() == view.ToLower().Trim()).Select(a=>a).SingleOrDefault();
            }
            else
            {
                return new access_role_in_access_areaSCM();
            }
            
        }
      
      
    }
}