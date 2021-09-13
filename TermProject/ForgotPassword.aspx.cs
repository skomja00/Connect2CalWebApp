using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CalendarClassLibrary.Model;

namespace TermProject
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadForgotModalContent();
            if (!IsPostBack)
            {
                MakeVisible("securityCard");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
        /// <summary>
        /// Check whether all responses have been provided and call the SecurityQuestions WebMethod.
        /// If provided the correct number of responses show the "New Password" card on the page. 
        /// Otherwise alert the user check incorrect responses.
        /// </summary>
        protected void btnCheckQuestions_Click(object sender, EventArgs e)
        {
            if (txtResponseCity.Text.Length > 0 &&
                txtResponsePhone.Text.Length > 0 &&
                txtResponseSchool.Text.Length > 0 &&
                txtEmailAddress.Text.Length > 0)
            {
                Account acctObj = new Account();
                acctObj.CreatedEmailAddress = txtEmailAddress.Text;
                acctObj.ResponsePhone = txtResponsePhone.Text;
                acctObj.ResponseCity = txtResponseCity.Text;
                acctObj.ResponseSchool = txtResponseSchool.Text;
                int numOfCorrectResponses = acctObj.SecurityQuestions();
                if (numOfCorrectResponses == 3)
                {
                    MakeVisible("newPasswordCard");
                }
                else
                {
                    Response.Write("<script>alert('Incorrect. Please check your Security Questions and try again.')</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('You must answer ALL security questions.')</script>");
            }
        }
        /// <summary>
        /// Check whether the new password/password confirm match and call the UpdatePassword WebMethod.
        /// If the update was successful redirect to the Login page. Otherwise alert the user.".
        /// </summary>
        protected void btnSaveNewPassword_Click(object sender, EventArgs e)
        {
            if (txtNewPassword.Text.Length > 0 &&
                txtPasswordConfirm.Text.Length > 0 &&
                txtNewPassword.Text == txtPasswordConfirm.Text)
            {
                Account acct = new Account();
                acct.CreatedEmailAddress = txtEmailAddress.Text;
                acct.AccountPassword = Account.Encrypt(txtNewPassword.Text);
                Boolean returnValue = acct.UpdatePassword();
                if (returnValue == true)
                {
                    Session["message"] = "Password reset success. Please log in.";
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    Response.Write("<script>alert('Problem updating password in the database. Please contact the HelpDesk.')</script>");
                }

            }
        }
        private void MakeVisible(string theContent)
        {
            newPasswordCard.Visible = false;
            securityCard.Visible = false;
 
            switch (theContent)
            {
                case "newPasswordCard":
                    newPasswordCard.Visible = true;
                    break;
                case "securityCard":
                    securityCard.Visible = true;
                    break;
            }
        }
        protected void LoadForgotModalContent()
        {
            hmForgotHelpModal.ButtonText = "Help";
            hmForgotHelpModal.Content =
                "<h1>Forgot Password Help Page</h1> " +
                "<h3>Forgot Password</h3> " +
                "<p>Forgot Password uses the <b>SOAP WebMethod SecurityQuestions</b> to execute an SqlCommand object for the \"TP_Account_Security_Questions_SP\" stored procedure. The procedure returns an integer of the number of matching responses." +
                "<p>If you provide all correct responses the server-side code will allow you to update the password." +
                "<h3>New Password</h3> " +
                "<p>When the new and confirm passwords match, the new password is encrypted, and passed to the UpdatePassword() method using the <b>Web Service Proxy</b>. To protect user authentication the encrypted version of the password is stored in the database.</p>" +
                "<p>Click <a target=\"_blank\" href=\"PDF/calendar-db-install.pdf\">here</a> for the database scripts, and <a target=\"_blank\" href=\"PDF/Calendar-ERD.pdf\">here</a> for a simple database ERD diagram.";
        }

        protected void btnCreateAccount_Click(object sender, EventArgs e)
        {

        }
    }
}