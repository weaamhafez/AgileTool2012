using Engineer.EMF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using EngineerWeb.Project;
using Engineer.EMF.Utils.Exceptions;
using Engineer.Service;

namespace EngineerWeb.User_Story
{
    public partial class List : System.Web.UI.Page
    {
        static UserStoryService service = (UserStoryService)new ServiceLocator<UserStory>().locate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClientScriptManager cs = Page.ClientScript;
                if (!cs.IsStartupScriptRegistered("listStories"))
                    cs.RegisterStartupScript(this.GetType(), "listStories",
                        "<script type=\"text/javascript\" src=\"" + ResolveClientUrl("~/Scripts/Modules/Story/list.js") + "\" />", false);
                BindData();
            }
        }
        private string GetUserId()
        {
            return User.Identity.GetUserId();
        }
        [System.Web.Services.WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static object GetAllStories()
        {
            var stories = service.FindByUser(new List().GetUserId());
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var storiesDynamic = JsonConvert.SerializeObject(stories, Formatting.Indented, jss);
            return new JavaScriptSerializer().Deserialize(storiesDynamic, typeof(object));
        }

        [System.Web.Services.WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static void SaveOrUpdate(IDictionary<string, object> story)
        {
            var storyObject = Utils.ToObject<Engineer.EMF.UserStory>(story);
            service.SaveOrUpdate(storyObject, new List().GetUserId(), story["AspNetUsers"].ToString(), story["projectId"].ToString());
        }

        [System.Web.Services.WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static void Finish(IDictionary<string, object> story)
        {
            var storyObject = Utils.ToObject<Engineer.EMF.UserStory>(story);
            service.FinishStory(storyObject,new List().GetUserId());
        }

        [System.Web.Services.WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static void Delete(IDictionary<string, object> story)
        {
            try
            {
                service.Delete(Utils.ToObject<Engineer.EMF.UserStory>(story));
            }
            catch (BadRequestException e)
            {
                throw new Exception(e.ErrorMessage);
            }
            catch (NotExistItemException e)
            {
                throw new Exception(e.ErrorMessage);
            }
        }
        [System.Web.Services.WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static object FindDiagramsByStory(IDictionary<string, object> story)
        {
            DiagramService service = (DiagramService)new ServiceLocator<Attachment>().locate();
            var diagrams = service.FindByStoryID(int.Parse(story["Id"].ToString()));
            return Utils.SerializeObject(diagrams);
        }

        private void BindData()
        {
            #region projects
            ProjectRepository pRepository = new ProjectRepository();
            projectId.DataSource = pRepository.FindByUserID(new List().GetUserId());
            projectId.DataBind();
            #endregion

            #region users
            var uService = (UserService)new ServiceLocator<AspNetUser>().locate();
            AspNetUsers.DataSource =  uService.ListAll();
            AspNetUsers.DataBind();
            #endregion

        }
    }
}