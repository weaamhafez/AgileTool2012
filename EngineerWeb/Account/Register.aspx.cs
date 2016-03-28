using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using EngineerWeb.Models;
using System.Web.UI.WebControls;
using Engineer.Service;
using System.Configuration;

namespace EngineerWeb.Account
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.Params["admin"]) && Request.Params["admin"] == "true")
                    UserAddBtn.CommandName = "Admin";
                   
                else
                    UserAddBtn.CommandName = "User";
            }
        }
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var user = new ApplicationUser() { UserName = UserName.Text, Email = Email.Text };
            IdentityResult result = manager.Create(user, Password.Text);
            if (((Button)sender).CommandName == "Admin")
            {
                if (result.Succeeded)
                {
                    manager.AddToRole(user.Id, "Admin");
                    signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                }
                else
                {
                    ErrorMessage.Text = result.Errors.FirstOrDefault();
                }
            }

            else
            {
                if (result.Succeeded)
                {
                    try
                    {
                        SendRegistrationEmail(user.UserName, Password.Text, user.Email);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    SuccessMessage.Text = "User has been added successfully";
                    ErrorMessage.Text = "";
                }

                else
                    ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }

        private void SendRegistrationEmail(string userName,string password,string email)
        {
            MailService mailService = MailService.Instance;
            string _mailConfigFilePath = AppDomain.CurrentDomain.GetData("DataDirectory") + "\\Mail.xml";
            mailService.LoadFile(_mailConfigFilePath);
            DTOMessage message = mailService.GetMessage("NewEmail");
            string sendFrom = (ConfigurationSettings.AppSettings["MailFromAddress"] != null) ? ConfigurationSettings.AppSettings["MailFromAddress"].ToString() : "";
            string messageSubject = MailService.formatMsg(message.Subject, null);
            string bodyStr = MailService.formatMsg(message.Body, new string[] { userName, password });
            MailService.SendMessageWithAttachment(sendFrom, email, null, messageSubject, bodyStr, null);
        }
    }
}