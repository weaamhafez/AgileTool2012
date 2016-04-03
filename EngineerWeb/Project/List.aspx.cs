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
using Engineer.EMF.Utils.Exceptions;
using System.Web.Security;
using System.Security.Permissions;
using Engineer.Service;

namespace EngineerWeb.Project
{
    
    public partial class List : System.Web.UI.Page
    {
        static ProjectService service = (ProjectService)new ServiceLocator<Engineer.EMF.Project>().locate();
        static ProjectRepository dRepository = new ProjectRepository();
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "Admin")]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUsers();
                ClientScriptManager cs = Page.ClientScript;
                if (!cs.IsStartupScriptRegistered("listProjects"))
                    cs.RegisterStartupScript(this.GetType(), "listProjects",
                        "<script type=\"text/javascript\" src=\"" + ResolveClientUrl("~/Scripts/Modules/Project/list.js") + "\" ></script>", false);
            }
        }

        private void BindUsers()
        {
            UserService uService = (UserService)new ServiceLocator<Engineer.EMF.AspNetUser>().locate();
            AspNetUsers.DataSource = uService.ListAll();
            AspNetUsers.DataBind();
        }

        private string GetUserId()
        {
            return User.Identity.GetUserId();
        }
        [System.Web.Services.WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static object GetAllProjects()
        {
            var projects = dRepository.FindByUserID(new List().GetUserId());
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var prjs = JsonConvert.SerializeObject(projects, Formatting.Indented, jss);
            return new JavaScriptSerializer().Deserialize(prjs, typeof(object));
        }

        [System.Web.Services.WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static object GetProjectUsers(string project)
        {
            UserService uService = (UserService)new ServiceLocator<Engineer.EMF.AspNetUser>().locate();
            var users = uService.FindAssignedUsersByProject(project);
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var prjs = JsonConvert.SerializeObject(users, Formatting.Indented, jss);
            return new JavaScriptSerializer().Deserialize(prjs, typeof(object));
        }

        [System.Web.Services.WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static void SaveOrUpdate(IDictionary<string,object> project)
        {
            var projectObject = Utils.ToObject<Engineer.EMF.Project>(project);
            string users = null;
            if (project.ContainsKey("AspNetUsers"))
                users = project["AspNetUsers"].ToString();
            dRepository.SaveOrUpdate(projectObject,new List().GetUserId(),users);
        }

        [System.Web.Services.WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static void Delete(IDictionary<string, object> project)
        {
            try
            {
                service.Delete(Utils.ToObject<Engineer.EMF.Project>(project),new List().GetUserId());
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
    }
}