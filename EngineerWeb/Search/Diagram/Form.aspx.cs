using Engineer.EMF;
using Engineer.EMF.Utils.Exceptions;
using Engineer.Service;
using EngineerWeb.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EngineerWeb.Search.Diagram
{
    public partial class Form : System.Web.UI.Page
    {
        static DiagramService service = (DiagramService)new ServiceLocator<Attachment>().locate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindUsersAndStories();
                ClientScriptManager cs = Page.ClientScript;
                if (!cs.IsStartupScriptRegistered("searchDiagrams"))
                    cs.RegisterStartupScript(this.GetType(), "searchDiagrams",
                        "<script type=\"text/javascript\" src=\"" + ResolveClientUrl("~/Scripts/Modules/Search/Diagram/form.js") + "\" />", false);
            }
        }

        [System.Web.Services.WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static object Search(IDictionary<string, object> usersAndStory)
        {
            try
            {
                string[] users = null;
                string[] stories = null;
                if(usersAndStory.Keys.Contains("Users"))
                    users = usersAndStory["Users"].ToString().Split(',');

                if (usersAndStory.Keys.Contains("Stories"))
                    stories = usersAndStory["Stories"].ToString().Split(',');

                List<Attachment> attachments = service.FindByUsersAndStories(users,stories);
                return Utils.SerializeObject(attachments);
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

        private void BindUsersAndStories()
        {
            UserService userService = (UserService)new ServiceLocator<AspNetUser>().locate();
            Users.DataSource = userService.ListAll();
            Users.DataBind();

            UserStoryService userStoryService = (UserStoryService)new ServiceLocator<UserStory>().locate();
            Stories.DataSource = userStoryService.ListAll();
            Stories.DataBind();
        }
    }
}