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
                        "<script type=\"text/javascript\" src=\"" + ResolveClientUrl("~/Scripts/Modules/Diagrams/view.js") + "\" />", false);
                if (!string.IsNullOrEmpty(Request.Params["id"]))
                {
                    var diagram = service.FindByID(int.Parse(Request.Params["id"]));
                    Response.Write(diagram.SVG);
                    Response.Flush();
                }
            }
        }
    }
}