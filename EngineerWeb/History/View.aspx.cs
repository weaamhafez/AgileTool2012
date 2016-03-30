using Engineer.EMF;
using Engineer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EngineerWeb.History
{
    public partial class View : System.Web.UI.Page
    {
        DiagramService service = (DiagramService)new ServiceLocator<Attachment>().locate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClientScriptManager cs = Page.ClientScript;
                if (!cs.IsStartupScriptRegistered("viewHistoryImage"))
                    cs.RegisterStartupScript(this.GetType(), "viewHistoryImage",
                        "<script type=\"text/javascript\" src=\"" + ResolveClientUrl("~/Scripts/Modules/History/view.js") + "\" />", false);
                if (!string.IsNullOrEmpty(Request.Params["id"]))
                {
                    var diagram = service.FindHistoryByIDAndUserStory(int.Parse(Request.Params["id"]));
                    Response.Write(diagram.Graph);
                    Response.Flush();
                }
            }
        }
    }
}