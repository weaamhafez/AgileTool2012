using Engineer.EMF;
using Engineer.Service;
using EngineerWeb.Project;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Providers.Entities;
using System.Web.Script.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity.Owin;

namespace EngineerWeb.History
{
    public partial class List : System.Web.UI.Page
    {
        static DiagramService service = (DiagramService)new ServiceLocator<Attachment>().locate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClientScriptManager cs = Page.ClientScript;
                if (!cs.IsStartupScriptRegistered("listHistory"))
                    cs.RegisterStartupScript(this.GetType(), "listHistory",
                        "<script type=\"text/javascript\" src=\"" + ResolveClientUrl("~/Scripts/Modules/History/list.js") + "\" />", false);
            }

        }

        [System.Web.Services.WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static object ShowHistory(IDictionary<string, object> diagram)
        {
            var diagramObject = Utils.ToObject<Engineer.EMF.UserStoryAttachment>(diagram);
            var history = service.FindHistory(diagramObject);
            return Utils.SerializeObject(history);
        }
        private string GetUserId()
        {
            return User.Identity.GetUserId();
        }
        private bool IsAdmin()
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            return manager.IsInRole(GetUserId(), "Admin");
        }
        [System.Web.Services.WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static object GetAllDiagrams()
        {
            List<UserStoryAttachment> diagrams = null;
            if (new List().IsAdmin())
                diagrams = service.ListAll();
            else
                diagrams = service.FindByUsers(new string[] { new List().GetUserId() });

            return Utils.SerializeObject(diagrams);
        }
    }
}