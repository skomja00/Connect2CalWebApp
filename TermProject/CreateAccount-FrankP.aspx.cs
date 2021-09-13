using CalendarClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utilities;

namespace TermProject
{
    public partial class CreateAccount : System.Web.UI.Page
    {

        DBConnect objDb = new DBConnect();

        protected void Page_Load(object sender, EventArgs e)
        {
            //This allows me to do the validation on the radio buttons client side
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

        }

        protected void btnCreateAccount_Click(object sender, EventArgs e)
        {
            if(txtUserName.Text != "" && 
               txtCreatePassword.Text != "" &&
                txtCreatedEmailAddress.Text != "" && 
               txtContactEmailAddress.Text != "" && 
               txtQCity.Text != "" && 
               txtQPhone.Text != "" && 
               txtQSchool.Text != "")
            {
                Account acc = new Account();
                
                acc.AccountRoleType = rblListType.SelectedValue;
                acc.UserName = txtUserName.Text;
                acc.UserAddress = txtAddress.Text;
                acc.PhoneNumber = txtPhoneNumber.Text;
                acc.CreatedEmailAddress = txtCreatedEmailAddress.Text;
                acc.ContactEmailAddress = txtContactEmailAddress.Text;
                acc.Avatar = 0;
                acc.AccountPassword = System.Text.Encoding.UTF8.GetBytes(txtCreatePassword.Text);
                Boolean result = acc.AddAccount();

                if (result == false)
                {
                    lblErrorStatus.Text = "There was a problem with adding your account";
                }


                //DataSet ds = new DataSet();
                // catch a dataset
                //ds = GetUsersId(txtCreatedEmailAddress.Text);
                // int accountId = int.Parse("Account", 0, GetUsersId(txtCreatedEmailAddress.Text)).ToString)()
                int accountId = int.Parse(objDb.GetField("AccountId", 0, GetUsersId(txtCreatedEmailAddress.Text)).ToString());

                securityQuestion.AccountId = accountId;
                securityQuestion.Question = "In what town or city was your first full time job?";
                securityQuestion.QuestionType = "City";
                securityQuestion.Response = txtQCity.Text;
                securityQuestion.AddSecurityQuestion(txtCreatedEmailAddress.Text);

                securityQuestion.AccountId = accountId;
                securityQuestion.Question = "What were the last four digits of your childhood telephone number?";
                securityQuestion.QuestionType = "Phone";
                securityQuestion.Response = txtQPhone.Text;
                securityQuestion.AddSecurityQuestion(txtCreatedEmailAddress.Text);

                securityQuestion.AccountId = accountId;
                securityQuestion.Question = "What primary school did you attend?";
                securityQuestion.QuestionType = "School";
                securityQuestion.Response = txtQSchool.Text;
                securityQuestion.AddSecurityQuestion(txtCreatedEmailAddress.Text);

            }
            else
            {
                lblErrorStatus.Text = "Please enter valid information in All the textboxes";
                return;
            }
        }

        public DataSet GetUsersId(String email)
        {
            
            SqlCommand objCommand = new SqlCommand();

            //Set the SQL COmmand object'sproperties for executing a stored procedure
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "TP_SecurityQuestion_SP";

            //Pass an input parameter value to the stored procedure that is used for the built-in parameter
            SqlParameter inputParameter = new SqlParameter("@theEmail", email);
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.SqlDbType = SqlDbType.VarChar;
            inputParameter.Size = 50;
            objCommand.Parameters.Add(inputParameter);

            //Execute the stored procedure using the DBConnect object and the SqlCommand object
            DataSet myDs = objDb.GetDataSetUsingCmdObj(objCommand);
            return myDs;


        }
    }
}