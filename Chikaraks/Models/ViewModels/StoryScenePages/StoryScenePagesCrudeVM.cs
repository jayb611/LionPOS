using ChikaraksServiceContractModels.DomainContractsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Chikaraks.Models.ViewModels.StoryScenePages
{
    [DataContract]
    public class StoryScenePagesCrudeVM : CRUDViewModel
    {
        [DataMember]
        public bool LoadFormFields { get; set; }
        [DataMember]
        public StoryScenePagesDCM StoryScenePagesDCM { get; set; }


        public StoryScenePagesCrudeVM()
        {
            LoadFormFields = true;
            StoryScenePagesDCM = new StoryScenePagesDCM();
        }
    }
}