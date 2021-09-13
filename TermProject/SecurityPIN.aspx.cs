using CalendarClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TermProject
{
    public partial class SecurityPIN : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadHelpModalContent();
            if (!IsPostBack)
            {
                if (Session["Account"] != null)
                {
                    Account acct = (Account)Session["Account"];
                    txtCreatedEmailAddress.Text = acct.CreatedEmailAddress; 
                }
            }
        }
        protected void btnActivateAccount_Click(object sender, EventArgs e)
        {
            if (txtCreatedEmailAddress.Text.Length > 0 &&
                txtSecurityPIN.Text.Length > 0 )
            {
                Account acct = new Account();
                acct.CreatedEmailAddress = txtCreatedEmailAddress.Text;
                acct.SecurityPIN = Convert.ToInt32(txtSecurityPIN.Text);
                Boolean result = acct.UpdateSecurityPIN();
                if (result == false)
                {
                    Response.Write("<script>alert('Activate account unsuccessful. Please check your input.')</script>");
                }
                else
                {
                    Session["message"] = "Account activated. Please log in.";
                    Response.Redirect("Login.aspx");
                }
            }
            else
            {
                Response.Write("<script>alert('Please provide your information.')");
            }

        }
        protected void btnLogIn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
        protected void LoadHelpModalContent()
        {
            mcActivateAcct.ButtonText = "Help";
            mcActivateAcct.Content =
                "<h1>Activate Account Help...</h1> " +
                "<p>For security and user authentication, you cannot LogIn to the webapp until your new account has been activated.</p>" +
                "<p>Check the Inbox of your <b><u>Contact Email Address</u></b> for a one-time SecurityPIN to activate your account.</p>" +
                "<p>The Account.asmx <b>Web Service</b> UpdateSecurityPIN method will call the \"TP_Account_SecurityPIN_SP\" <b>stored procedure</b> returning a Boolean indicating whether the provided SecurityPIN is valid. If so the account will be made Active, and the SecurityPIN will be deleted from the database." +
                "<p>Click <a target=\"_blank\" href=\"PDF/calendar-db-install.pdf\">here</a> for the database scripts, and <a target=\"_blank\" href=\"PDF/Calendar-ERD.pdf\">here</a> for a simple database ERD diagram.";
        }

    }
}