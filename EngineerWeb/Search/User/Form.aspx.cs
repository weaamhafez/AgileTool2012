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

namespace EngineerWeb.Search.User
{
    public partial class Form : System.Web.UI.Page
    {
        static UserService service = (UserService)new ServiceLocator<AspNetUser>().locate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindDiagram();
                ClientScriptManager cs = Page.ClientScript;
                if (!cs.IsStartupScriptRegistered("searchUsers"))
                    cs.RegisterStartupScript(this.GetType(), "searchUsers",
                        "<script type=\"text/javascript\" src=\"" + ResolveClientUrl("~/Scripts/Modules/Search/User/form.js") + "\" />", false);
            }
        }

        private void BindDiagram()
        {
            DiagramService diagramService = (DiagramService)new ServiceLocator<Attachment>().locate();
            List<UserStoryAttachment> attachments = diagramService.ListAll();
            Diagrams.DataSource = attachments;
            Diagrams.DataBind();
        }

        [System.Web.Services.WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static object Search(string diagram)
        {
            try
            {
                if (diagram == null)
                    throw new BadRequestException(AppConstants.EXCEPTION_DIAGRAM_CANNOT_EMPTY);
                List<AspNetUser> users = service.FindByDiagram(diagram);
                return Utils.SerializeObject(users);
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
    }
}