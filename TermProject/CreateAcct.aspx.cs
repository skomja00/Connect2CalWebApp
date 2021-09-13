using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utilities;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using CalendarClassLibrary.Model;

namespace TermProject
{
    public partial class CreateAcct : System.Web.UI.Page
    {
        DBConnect objDB = new DBConnect();
        SqlCommand objCommand = new SqlCommand();

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadModalContent();

            if (!IsPostBack)
            {
                ArrayList arrayListAvatar = (ArrayList)Application["ArrayListAvatar"];
                for (int i = 0; i < arrayListAvatar.Count; i++) 
                {
                    ListItem listItem = new ListItem();
                    listItem.Value = arrayListAvatar[i].ToString();
                    listItem.Text = "Avatar " + (i + 1).ToString();
                    ddlAvatar.Items.Add(listItem);
                }
                ddlAvatar.Items.Insert(0, new ListItem("Select Avatar...", "Select"));
            }
        }

        protected void ddlAvatar_SelectedIndexChanged(object sender, EventArgs e)
        {
            imgAvatar.ImageUrl = "Images/Avatars/" + ddlAvatar.SelectedItem.Value;
        }
        protected void btnCreateAccount_Click(object sender, EventArgs e)
        {
            // Execute stored procedure using DBConnect object and the SQLCommand object
            // If a problem occurs on the DB (ie. -1) raise an alert to the user
            // otherwise reset the fields on the form, and message the user to go to the sign in page.
            if (txtUserName.Text.Length == 0 ||
                txtAddress.Text.Length == 0 ||
                txtPhoneNumber.Text.Length == 0 ||
                txtEmail.Text.Length == 0 ||
                txtEmailSecurity.Text.Length == 0 ||
                txtPassword.Text.Length == 0 ||
                txtPasswordConfirm.Text.Length == 0 ||
                ddlAvatar.SelectedIndex == 0 ||
                rblUserType.SelectedIndex == -1 ||
                txtPassword.Text != txtPasswordConfirm.Text ||
                txtSecQuestionCity.Text.Length == 0 ||
                txtSecQuestionPhone.Text.Length == 0 ||
                txtSecQuestionSchool.Text.Length == 0)
            {
                Response.Write("<script>alert('Somthing is wrong. Please check your input.')</script>");
            }
            else
            {
                Account newAcct = new Account();
                newAcct.UserName = txtUserName.Text;
                newAcct.UserAddress = txtAddress.Text;
                newAcct.PhoneNumber = txtPhoneNumber.Text;
                newAcct.CreatedEmailAddress = txtEmail.Text;
                newAcct.ContactEmailAddress = txtEmailSecurity.Text;
                newAcct.Avatar = ddlAvatar.SelectedIndex - 1;
                newAcct.AccountPassword = Account.Encrypt(txtPassword.Text);
                newAcct.Active = "no";
                newAcct.AccountRoleType = rblUserType.SelectedValue;
                newAcct.ResponseCity = txtSecQuestionCity.Text;
                newAcct.ResponsePhone = txtSecQuestionPhone.Text;
                newAcct.ResponseSchool = txtSecQuestionSchool.Text;
                Boolean result = newAcct.AddAccount();
                if (result == false)
                {
                    Response.Write("<script>alert('Create account unsuccessful. Please check your input.')</script>");
                }
                else
                {
                    Session["Account"] = newAcct;
                    lblTopMessage.Text = "Check your email for you Account Security PIN.";
                    lblTopMessage.CssClass = "text-success font-weight-bold";
                    btnCreateAccount.CssClass = "btn btn-outline-secondary";
                    btnLogIn.CssClass = "btn btn-primary";
                    rblUserType.SelectedIndex = -1;
                    txtUserName.Text = null;
                    txtAddress.Text = null;
                    txtPhoneNumber.Text = null;
                    txtEmail.Text = null;
                    txtEmailSecurity.Text = null;
                    ddlAvatar.SelectedIndex = -1;
                    txtPassword.Text = null;
                    txtPasswordConfirm.Text = null;
                    rblUserType.SelectedIndex = -1;
                    ddlAvatar.SelectedIndex = -1;
                    txtSecQuestionCity.Text = null;
                    txtSecQuestionPhone.Text = null;
                    txtSecQuestionSchool.Text = null;
                    imgAvatar.ImageUrl = "Images/Avatars/default-avatar.svg";
                    Response.Redirect("SecurityPIN.aspx");
                }
            }
        }
        protected void LoadModalContent()
        {
            mcCreateAcctModal.ButtonText = "Help";
            mcCreateAcctModal.Content =
                "<h1 class=\"text-primary\">About Create Account...</h1> " +
                "<p>Provide information to create an account including a unique email address, and a type of account. A <b>user</b> type account has email functionality while an <b>admin</b> type account cannot send or receive email but instead responsible for maintaining certain functions of the web app.</p>" +
                "<p>Choose an avatar from the DropDown. The <b>AutoPostBack</b> property updates the page so you can select an avatar to preview the image. The <b>Application Object</b> is used store an <b>ArrayList</b> of avatars so all users and pages of the web app can access the list." +
                "<h3>Create Account</h3> " +
                "<p>The <input type=\"button\" value=\"Create Account\" class=\"btn btn-primary\"></input> button creates an instance of Account type, invokes the <b>Account WebService Proxy</b> which calls the Account_Insert_SP <b>stored procedure</b> to store the new account." +
                "<p>Click <a target=\"_blank\" href=\"PDF/calendar-db-install.pdf\">here</a> for the database scripts, and <a target=\"_blank\" href=\"PDF/Calendar-ERD.pdf\">here</a> for a simple database ERD diagram.";
        }
        protected void btnLogIn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}