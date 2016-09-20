using ChikaraksServiceContractModels.DomainContractsModel;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Chikaraks.Models.ViewModels;
using System.Web.Mvc;

namespace Chikaraks.Models.ViewModels.StoryCategory
{
    [DataContract]
    public class CrudOperationStoryCategoryVM : CRUDViewModel
    {
        [DataMember]
        public bool LoadFormFields { get; set; }
        [DataMember]
        public StoryCategoryDCM StoryCategoryDCM { get; set; }
        [DataMember]
        public string story_scene_pages_json { get; set; }
        [DataMember]
        public List<SelectListItem> StoryTypes { get; set; }

        public CrudOperationStoryCategoryVM()
        {
            LoadFormFields = true;
            StoryCategoryDCM = new StoryCategoryDCM();
            //StoryCategoryDCM.story_scene_pages = new List<StoryScenePagesDCM>();

            StoryTypes = new List<SelectListItem>();
            SelectListItem bt = new SelectListItem();
            bt.Text = "Audio Story";
            bt.Value = bt.Text;
            StoryTypes.Add(bt);

            bt = new SelectListItem();
            bt.Text = "Image Story";
            bt.Value = bt.Text;
            StoryTypes.Add(bt);

            bt = new SelectListItem();
            bt.Text = "Image And Audio Story";
            bt.Value = bt.Text;
            StoryTypes.Add(bt);

            bt = new SelectListItem();
            bt.Text = "Video Story";
            bt.Value = bt.Text;
            StoryTypes.Add(bt);
        }
    }
}
