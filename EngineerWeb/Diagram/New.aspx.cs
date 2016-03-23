using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Engineer.EMF;
using System.IO;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using System.Web.Script.Services;
using EngineerWeb.Project;
using Engineer.Service;

namespace EngineerWeb.Diagram
{
    public partial class New : System.Web.UI.Page
    {
        static DiagramService service = (DiagramService)new ServiceLocator<Attachment>().locate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindDataToUserStory();
                if (!string.IsNullOrEmpty(Request.Params["Id"]))
                    LoadDiagram(Request.Params["Id"]);

                ClientScriptManager cs = Page.ClientScript;
                if (!cs.IsStartupScriptRegistered("interactions"))
                    cs.RegisterStartupScript(this.GetType(), "interactions",
                        "<script type=\"text/javascript\" src=\"" + ResolveClientUrl("~/Scripts/Modules/Builder/interactions.js") + "\" />", false);

                #region Add templates
                Dictionary<string, Dictionary<string, string>> templates = new Dictionary<string, Dictionary<string, string>>();
                DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Diagram"));
                directory.GetDirectories("*", SearchOption.TopDirectoryOnly).ToList().ForEach(folder =>
                {
                    Dictionary<string, string> folderTemplates = new Dictionary<string, string>();
                    folder.GetFiles().ToList().ForEach(file =>
                    {
                        using (StreamReader sr = new StreamReader(file.FullName))
                        {
                            string line = sr.ReadToEnd();
                            if (!string.IsNullOrEmpty(line))
                                folderTemplates.Add(file.Name.Substring(0,file.Name.LastIndexOf(".")), line);
                        }
                    });
                    templates.Add(folder.Name, folderTemplates);
                    
                });
                // loop throug all snippets to save contents
               
                JavaScriptSerializer ser = new JavaScriptSerializer();
                Templates.Value = ser.Serialize(templates);
                #endregion
            }
        }
        private void BindDataToUserStory()
        {
            List<UserStory> userStories = new UserStoryRepository().FindByUser(User.Identity.GetUserId());
            UserStoriesList.DataSource = userStories;
            UserStoriesList.DataBind();
        }

        private void LoadDiagram(string id)
        {
            if(!string.IsNullOrEmpty(id))
            {
                var diagram = service.FindByID(int.Parse(id));
                diagramID.Value = diagram.Id.ToString();
                diagramName.Text = diagram.name;
                diagramGraph.Value = diagram.activties;
            }
        }

        [System.Web.Services.WebMethod]
        [ScriptMethod(UseHttpGet = false)]
        public static string SaveOrUpdate(IDictionary<string, object> diagram)
        {
            var diagramObject =new Attachment() { name = diagram["name"].ToString()};

            if (diagram.ContainsKey("Id") && !string.IsNullOrEmpty(diagram["Id"].ToString()))
                diagramObject.Id = int.Parse(diagram["Id"].ToString());

            string[] userStoriesIds = diagram.ContainsKey("userStories") ? 
                ((object[])diagram["userStories"]).Where(w=>w != null).Select(s=>s.ToString()).ToArray()
                : null;
            string graph = diagram.ContainsKey("graph") ? diagram["graph"].ToString() : null;
            string svg = diagram.ContainsKey("svg") ? diagram["svg"].ToString() : null;
            diagramObject.SVG = svg;
            return service.SaveOrUpdate(diagramObject,userStoriesIds,graph, new New().GetUserId()).ToString();
        }

        private string GetUserId()
        {
            return User.Identity.GetUserId();
        }
    }
}