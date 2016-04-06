using Engineer.EMF;
using Engineer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EngineerWeb.Diagram
{
    public partial class View : System.Web.UI.Page
    {
        DiagramService service = (DiagramService)new ServiceLocator<Attachment>().locate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                ClientScriptManager cs = Page.ClientScript;
                if (!cs.IsStartupScriptRegistered("viewImage"))
                    cs.RegisterStartupScript(this.GetType(), "viewImage",
                        "<script type=\"text/javascript\" src=\"" + ResolveClientUrl("~/Scripts/Modules/Diagrams/view.js") + "\" ></script>", false);


                // open the attach history in search menu
                if (!string.IsNullOrEmpty(Request.Params["historyId"]))
                {
                    var diagram = service.FindByHistoryID(int.Parse(Request.Params["historyId"]));
                    Response.Write(diagram.Graph);
                    Response.Flush();
                }
                // open original in history menu
                else if (!string.IsNullOrEmpty(Request.Params["id"]) && !string.IsNullOrEmpty(Request.Params["storyId"]))
                {
                    var diagram = service.FindByIDAndUserStory(int.Parse(Request.Params["id"]),int.Parse(Request.Params["storyId"]));
                    Response.Write(diagram.SVG);
                    Response.Flush();
                }
            }
        }
    }
}