using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utilities;
using CalendarClassLibrary.Model;
using AjaxControlToolkit;

namespace TermProject
{
    public partial class Login : System.Web.UI.Page
    {
        HttpCookie objCookie;
        string message;
        /// <summary>
        /// LoadHelpModalContent() updates the Modal Async Custom Control with 
        /// information regarding the Login page.
        /// 
        /// If !IsPostBack check if the client previously checked "Remember Me"
        /// If we remember the user log in using the credentials from the cookie. 
        /// Otherwise the user can manually log in.
        /// 
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadHelpModalContent();
            objCookie = Request.Cookies["CIS3342_Calendar"];
            if (!IsPostBack)
            {
                if (objCookie != null)
                {
                    Byte[] accountPassword = Convert.FromBase64String(objCookie["AccountPassword"]);
                    LogInUpdateSession(objCookie["CreatedEmailAddress"], accountPassword);
                    switch (Session["AccountRoleType"].ToString())
                    {
                        case "User":
                            Response.Redirect("UserClient.aspx");
                            break;
                        case "Administrator":
                            Response.Redirect("AdminClient.aspx");
                            break;
                    }
                }
                message = "Please log in.";
            }
            if (Session["message"] == null)
                lblMessage.Text = "Please log in.";
            else
                lblMessage.Text = Session["message"].ToString();
        }
        /// <summary>
        /// Read the DB to check if provided both the CreatedEmailAddress 
        /// and AccountPassword. If so encrypt the password and call the 
        /// LogIn method. Otherwise alert the user to enter their credentials.
        /// </summary>
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtEmail.Text != null && txtPass.Text != null)
            {
                Byte[] encryptPass = Account.Encrypt(txtPass.Text);
                LogInUpdateSession(txtEmail.Text, encryptPass);
            }
            else
            {
                Response.Write("<script>alert('Please enter your credentials.')</script>");
            }
        }
        protected void btnActivateAccount_Click(object sender, EventArgs e)
        {
            Response.Redirect("SecurityPIN.aspx");
        }
        /// <summary>
        /// Call the LogIn service. If successful value the Session object
        /// with the Account data. Otherwise alert the user the LogIn was unsuccessful.
        /// </summary>
        /// <param name="theCreatedEmailAddress"></param>
        /// <param name="theAccountPassword"></param>
        protected void LogInUpdateSession (string theCreatedEmailAddress, Byte[] theAccountPassword)
        {
            Account newAcct = new Account();
            newAcct.CreatedEmailAddress = txtEmail.Text;
            newAcct.AccountPassword = Account.Encrypt(txtPass.Text);
            DataSet accountData = newAcct.LogIn();

            if (accountData.Tables[0].Rows.Count > 0)
            {
                DataRow objRow = accountData.Tables[0].Rows[0];
                Session["AccountId"] = objRow["AccountId"];
                Session["UserName"] = objRow["UserName"];
                Session["UserAddress"] = objRow["UserAddress"];
                Session["PhoneNumber"] = objRow["PhoneNumber"];
                Session["CreatedEmailAddress"] = objRow["CreatedEmailAddress"];
                Session["ContactEmailAddress"] = objRow["ContactEmailAddress"];
                Session["Avatar"] = objRow["Avatar"];
                Session["AccountPassword"] = objRow["AccountPassword"];
                Session["DateTimeStamp"] = objRow["DateTimeStamp"];
                Session["AccountRoleType"] = objRow["AccountRoleType"];
                if (chkRemember.Checked)
                {
                    CreateCookie(Session["CreatedEmailAddress"].ToString(), theAccountPassword);
                }
                switch (Session["AccountRoleType"].ToString())
                {
                    case "User":
                        Response.Redirect("UserClient.aspx");
                        break;
                    case "Administrator":
                        Response.Redirect("AdminClient.aspx");
                        break;
                }
            }
            else
            {
                Response.Write("<script>alert('Login failed. Please check your credentials.')</script>");
            }
        }
        private void CreateCookie(string theCreatedEmailAddress, Byte[] theAccountPassword)
        {
            HttpCookie myCookie = new HttpCookie("CIS3342_Calendar");
            myCookie.Values["Name"] = "CIS3342_Calendar";
            myCookie.Values["CreatedEmailAddress"] = theCreatedEmailAddress;
            myCookie.Values["AccountPassword"] = Convert.ToBase64String(theAccountPassword);
            DateTime expires = new DateTime();
            expires = DateTime.Now.AddMonths(1);
            myCookie.Expires = expires;
            Response.Cookies.Add(myCookie);
        }
        protected void btnForgotPassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("ForgotPassword.aspx");
        }
        protected void btnCreateAccount_Click(object sender, EventArgs e)
        {
            Response.Redirect("CreateAcct.aspx");
        }
        protected void LoadHelpModalContent()
        {
            hmLoginHelpModal.ButtonText = "About";
            hmLoginHelpModal.Content =
                "<h1>About Connect2Cal</h1> " +
                "<p>Conect2Cal provides appointment scheduling using a basic calendar system. Create an account, and login to explore the options.</p>" +
                "<p>The Connect2Cal web app implements many .NET technologies. Please explore the app, and be sure to click \"Help\" to see more .NET features on the other pages.</p> " +
                "<ul> " +
                "    <li>For instance, the content you are reading is a Modal <b>Custom Control</b> using UpdatePanels. The UpdatePanel implements <b>AJAX</b> with Javascript provided by a ScriptManager for Partial page updates necessary to prevent a Full Page Postback from \"closing\" the Modal. Since only one \"Help\" option is on each page a ScriptManagerProxy is not necessary.</li>" +
                "</ul> " +
                "<h3>LogIn Help</h3> " +
                "<p><img src=Images/Help/RememberMe.png />Check this to store your encrypted password in a <b>cookie</b> to automatically log into future sessions. Otherwise you have to type your credentials. Note: Logout <b>deletes the cookie</b> by setting the Expires property to a past date then adding the expired cookie to the Response.Cookies collection.<p>" +
                "<p><img src=Images/Help/ForgotPassword.png />Takes you to a page to answer your security questions. If all responses are correct you can enter a new password.</p>" +
                "<h3>The Account customer User Data Type is built on SOAP Web Service</h3> " +
                "<p>The Account.asmx <b>Web Service</b> facilitates datalayer access for all the custom Account datatype related activity such as LogIn, SecurityPIN Activation, Update Password, etc.</p> " +
                "<p>The <b>Web Service</b> has the following methods:</p> " +
                "<ul> " +
                "    <li>The AddAccount web method calls the \"TP_Account_Insert_SP\" <b>stored procedure</b> and returns a Boolean showing if the given credentials are correct.</li>" +
                "    <li>The LogIn web method calls the \"TP_Account_Login_SP\" <b>stored procedure</b> and returns a Boolean showing if the given credentials are correct.</li>" +
                "    <li>The SecurityQuestions method executes the \"TP_Account_Security_Questions_SP\" <b>stored procedure</b> and returns an Integer of the number of matching responses.</li> " +
                "    <li>The UpdatePassword method calls the \"TP_Account_Update_Password_SP\" <b>stored procedure</b> which returns a Boolean indicating if the new password was stored.</li> " +
                "    <li>The UpdateSecurityPIN method calls the \"TP_Account_SecurityPIN_SP\" <b>stored procedure</b> which returns a Boolean showing the result of the update.</li> " +           
                "</ul> " +
                "<p>Click <a target=\"_blank\" href=\"PDF/calendar-db-install.pdf\">here</a> for the database scripts, and <a target=\"_blank\" href=\"PDF/Calendar-ERD.pdf\">here</a> for a simple database ERD diagram.";
        }
    }
}