using Engineer.Service;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EngineerWeb.Reports
{
    public partial class ListStories : System.Web.UI.Page
    {
        static UserStoryService service = (UserStoryService)new ServiceLocator<Engineer.EMF.UserStory>().locate();
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "Admin")]
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindUserStoriesGrid();
            }
        }
        private string GetUserId()
        {
            return User.Identity.GetUserId();
        }
        private void BindUserStoriesGrid()
        {
            var userStories = service.FindByUser(GetUserId());
            gvResult.DataSource = userStories;
            gvResult.DataBind();
        }

        protected void gvResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string Id = gvResult.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();
            DiagramService dService = (DiagramService)new ServiceLocator<Engineer.EMF.Attachment>().locate();
            var diagrams = dService.FindByStoryID(int.Parse(Id));
            Diagrams.DataSource = diagrams != null ? diagrams : new List<Engineer.EMF.UserStoryAttachment>();
            Diagrams.DataBind();
            Diagrams.Visible = true;
        }

        protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button ViewButton = (Button)e.Row.FindControl("btnViewDiagrams");
                ViewButton.CommandArgument = e.Row.RowIndex.ToString();
            }
        }

        protected void gvResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow Row = gvResult.SelectedRow;
            
        }
    }
}