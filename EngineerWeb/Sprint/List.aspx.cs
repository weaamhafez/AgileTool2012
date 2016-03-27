using Engineer.EMF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using EngineerWeb.Project;
using Engineer.Service;
using Engineer.EMF.Utils.Exceptions;

namespace EngineerWeb.Sprint
{
    public partial class List : System.Web.UI.Page
    {
        static SprintService service = (SprintService)new ServiceLocator<Engineer.EMF.Sprint>().locate();
        static SprintRepository dRepository = new SprintRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClientScriptManager cs = Page.ClientScript;
                if (!cs.IsStartupScriptRegistered("listSprints"))
                    cs.RegisterStartupScript(this.GetType(), "listSprints",
                        "<script type=\"text/javascript\" src=\"" + ResolveClientUrl("~/Scripts/Modules/Sprint/list.js") + "\" />", false);

                BindData();
            }
        }
        private string GetUserId()
        {
            return User.Identity.GetUserId();
        }
        [System.Web.Services.WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static object GetAllSprints()
        {
            try
            {
                var sprints = dRepository.FindByUser(new List().GetUserId());
                return Utils.SerializeObject(sprints);
            }
            catch (BadRequestException ex)
            {
                throw new Exception(ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return null;
        }

        [System.Web.Services.WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static void SaveOrUpdate(IDictionary<string, object> sprint)
        {
            try
            {
                var sprintObject = Utils.ToObject<Engineer.EMF.Sprint>(sprint);
                sprintObject.number = int.Parse(sprint["number"].ToString());
                 service.SaveOrUpdate(sprintObject, new List().GetUserId(),sprint["UserStories"].ToString());
            }
            catch (BadRequestException ex)
            {
                throw new Exception(ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [System.Web.Services.WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static void Close(IDictionary<string, object> sprint)
        {
            try
            {
                var sprintObject = Utils.ToObject<Engineer.EMF.Sprint>(sprint);
                var diagrams = service.CloseSprint(sprintObject);
                if(diagrams != null)
                {

                }

            }
            catch (BadRequestException ex)
            {
                throw new Exception(ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [System.Web.Services.WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static void Open(IDictionary<string, object> sprint)
        {
            try
            {
                var sprintObject = Utils.ToObject<Engineer.EMF.Sprint>(sprint);
                service.OpenSprint(sprintObject);
            }
            catch (BadRequestException ex)
            {
                throw new Exception(ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [System.Web.Services.WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static void Delete(IDictionary<string, object> sprint)
        {
            try
            {
                service.Delete(Utils.ToObject<Engineer.EMF.Sprint>(sprint));
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
        public static object GetSprintStories(string sprint)
        {
            UserStoryService uService = (UserStoryService)new ServiceLocator<Engineer.EMF.UserStory>().locate();
            var stories = uService.FindBySprint(int.Parse(sprint));
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var prjs = JsonConvert.SerializeObject(stories, Formatting.Indented, jss);
            return new JavaScriptSerializer().Deserialize(prjs, typeof(object));
        }

        private void BindData()
        {
            try
            {
                var userStoryService = (UserStoryService)new ServiceLocator<Engineer.EMF.UserStory>().locate();
                UserStories.DataSource = userStoryService.FindByUser(new List().GetUserId());
                UserStories.DataBind();
            }
            catch (BadRequestException ex)
            {
                throw new Exception(ex.ErrorMessage);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}