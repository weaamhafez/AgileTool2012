using Engineer.EMF;
using System;
using System.Web.Script.Services;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Engineer.Service;
using System.Collections.Generic;
using EngineerWeb.Project;
using Engineer.EMF.Utils.Exceptions;

namespace EngineerWeb.Diagram
{
    public partial class List : System.Web.UI.Page
    {
        
        static DiagramRepository dRepository = new DiagramRepository();
        static DiagramService service = (DiagramService)new ServiceLocator<Attachment>().locate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                ClientScriptManager cs = Page.ClientScript;
                if (!cs.IsStartupScriptRegistered("listDiagrams"))
                    cs.RegisterStartupScript(this.GetType(), "listDiagrams",
                        "<script type=\"text/javascript\" src=\"" + ResolveClientUrl("~/Scripts/Modules/Diagrams/list.js") + "\" />", false);
            }
            
        }
        private string GetUserId()
        {
            return User.Identity.GetUserId();
        }
        [System.Web.Services.WebMethod]
        [ScriptMethod(UseHttpGet =false,ResponseFormat =ResponseFormat.Json)]
        public static object GetAllDiagrams()
        {
            var diagrams = dRepository.ListAll(new List().GetUserId());
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var diagramsDynamic = JsonConvert.SerializeObject(diagrams, Formatting.Indented, jss);
            return new JavaScriptSerializer().Deserialize(diagramsDynamic,typeof(object));
        }
        [System.Web.Services.WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static void Delete(IDictionary<string, object> diagram)
        {
            try
            {
                service.Delete(Utils.ToObject<Engineer.EMF.Attachment>(diagram));
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
        public static object FindStoriesByDiagram(IDictionary<string, object> diagram)
        {
            UserStoryService service = (UserStoryService)new ServiceLocator<UserStory>().locate();
            var diagrams = service.FindByDiagramID(int.Parse(diagram["Id"].ToString()));
            return Utils.SerializeObject(diagrams);
        }
    }
}