using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using Utilities;
using CalendarClassLibrary.Model;
using System.Xml;
using System.Xml.Serialization;

namespace CalendarSOAPWebService
{
    /// <summary>
    /// Account class related WebServices 
    /// </summary>
    [WebService(Namespace = "http://temple.edu/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class AccountWS : System.Web.Services.WebService
    {
        DBConnect objDB = new DBConnect();

        [WebMethod]
        public DataSet LogIn(Account theAccount)
        {
            SqlCommand objSqlCommand = new SqlCommand();
            objSqlCommand.CommandType = CommandType.StoredProcedure;
            objSqlCommand.CommandText = "TP_Account_Login_SP";

            SqlParameter userName = new SqlParameter("@CreatedEmailAddress", theAccount.CreatedEmailAddress);
            userName.Direction = ParameterDirection.Input;
            userName.SqlDbType = SqlDbType.VarChar;
            userName.Size = 254;
            objSqlCommand.Parameters.Add(userName);

            SqlParameter accountPassword = new SqlParameter("@AccountPassword", theAccount.AccountPassword);
            accountPassword.Direction = ParameterDirection.Input;
            accountPassword.SqlDbType = SqlDbType.VarBinary;
            //accountPassword.Size = 32;
            objSqlCommand.Parameters.Add(accountPassword);


            // Execute stored procedure to login. 
            DataSet acctDS =  objDB.GetDataSetUsingCmdObj(objSqlCommand);

            return acctDS;
        }
        /// <summary>
        /// Check the given responses to the Security Question match the database.
        /// Return the number of matching responses.
        /// </summary>
        /// <param name="theCreatedEmailAddress"></param>
        /// <param name="theResponseCity"></param>
        /// <param name="theResponsePhone"></param>
        /// <param name="theResponseSchool"></param>
        /// <returns>int Number of matching responses</returns>
        [WebMethod]
        public int SecurityQuestions(Account theAccount)
        {
            DBConnect objDB = new DBConnect();
            SqlCommand objSqlCmd = new SqlCommand();

            objSqlCmd.CommandText = "TP_Account_Security_Questions_SP";
            objSqlCmd.CommandType = CommandType.StoredProcedure;

            SqlParameter emailParm = new SqlParameter("@CreatedEmailAddress", theAccount.CreatedEmailAddress);
            emailParm.Direction = ParameterDirection.Input;
            emailParm.SqlDbType = SqlDbType.VarChar;
            emailParm.Size = 254;
            objSqlCmd.Parameters.Add(emailParm);

            SqlParameter cityParm = new SqlParameter("@ResponseCity", theAccount.ResponseCity);
            cityParm.Direction = ParameterDirection.Input;
            cityParm.SqlDbType = SqlDbType.VarChar;
            cityParm.Size = 254;
            objSqlCmd.Parameters.Add(cityParm);

            SqlParameter phoneParm = new SqlParameter("@ResponsePhone", theAccount.ResponsePhone);
            phoneParm.Direction = ParameterDirection.Input;
            phoneParm.SqlDbType = SqlDbType.VarChar;
            phoneParm.Size = 254;
            objSqlCmd.Parameters.Add(phoneParm);

            SqlParameter schoolParm = new SqlParameter("@ResponseSchool", theAccount.ResponseSchool);
            schoolParm.Direction = ParameterDirection.Input;
            schoolParm.SqlDbType = SqlDbType.VarChar;
            schoolParm.Size = 254;
            objSqlCmd.Parameters.Add(schoolParm);

            DataSet objDS = objDB.GetDataSetUsingCmdObj(objSqlCmd);

            int numOfCorrectResponses = Convert.ToInt32(objDS.Tables[0].Rows[0].ItemArray[0]);

            return numOfCorrectResponses;
        }
        /// <summary>
        /// Execute the TP_Account_Update_Password_SP stored procedure to update the AccountPassword
        /// </summary>
        /// <param name="theCreatedEmailAddress"></param>
        /// <param name="thePassword"></param>
        /// <returns>Integer number of rows affected by the update, or -1 for an exception</returns>
        [WebMethod]
        public Boolean UpdatePassword(Account theAccount)
        {
            DBConnect objDB = new DBConnect();
            SqlCommand objSqlCmd = new SqlCommand();

            objSqlCmd.CommandText = "TP_Account_Update_Password_SP";
            objSqlCmd.CommandType = CommandType.StoredProcedure;

            SqlParameter emailParm = new SqlParameter("@CreatedEmailAddress", theAccount.CreatedEmailAddress);
            emailParm.Direction = ParameterDirection.Input;
            emailParm.SqlDbType = SqlDbType.VarChar;
            emailParm.Size = 254;
            objSqlCmd.Parameters.Add(emailParm);

            SqlParameter passwordParm = new SqlParameter("@AccountPassword", theAccount.AccountPassword);
            passwordParm.Direction = ParameterDirection.Input;
            passwordParm.SqlDbType = SqlDbType.VarBinary;
            //passwordParm.Size = 32;
            objSqlCmd.Parameters.Add(passwordParm);

            int returnValue = objDB.DoUpdateUsingCmdObj(objSqlCmd);

            if (returnValue > 0)
                return true;
            else
                return false;
        }
        [WebMethod]
        public Boolean UpdateSecurityPIN(Account theAccount)
        {
            DBConnect objDB = new DBConnect();
            SqlCommand objSqlCmd = new SqlCommand();

            objSqlCmd.CommandText = "TP_Account_SecurityPIN_SP";
            objSqlCmd.CommandType = CommandType.StoredProcedure;

            SqlParameter createdEmailAddressParm = new SqlParameter("@CreatedEmailAddress", SqlDbType.VarChar);
            createdEmailAddressParm.Size = 254;
            createdEmailAddressParm.Direction = ParameterDirection.Input;
            createdEmailAddressParm.Value = theAccount.CreatedEmailAddress;
            objSqlCmd.Parameters.Add(createdEmailAddressParm);

            SqlParameter securityPINParm = new SqlParameter("@SecurityPIN", SqlDbType.Int);
            securityPINParm.Size = 4;
            securityPINParm.Direction = ParameterDirection.Input;
            securityPINParm.Value = theAccount.SecurityPIN;
            objSqlCmd.Parameters.Add(securityPINParm);

            int result = objDB.DoUpdateUsingCmdObj(objSqlCmd);

            if (result > 0)
                return true;
            else
                return false;
        }
        [WebMethod]
        public Boolean AddAccount(Account theAccount)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "TP_Account_Insert_SP";

            SqlParameter inputParameter = new SqlParameter("@AccountRoleType", SqlDbType.VarChar);
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.Value = theAccount.AccountRoleType;
            inputParameter.Size = 14;
            objCommand.Parameters.Add(inputParameter);

            inputParameter = new SqlParameter("@UserName", SqlDbType.VarChar);
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.Value = theAccount.UserName;
            inputParameter.Size = 50;
            objCommand.Parameters.Add(inputParameter);

            inputParameter = new SqlParameter("@UserAddress", SqlDbType.VarChar);
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.Value = theAccount.UserAddress;
            inputParameter.Size = 254;
            objCommand.Parameters.Add(inputParameter);

            inputParameter = new SqlParameter("@PhoneNumber", SqlDbType.VarChar);
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.Value = theAccount.PhoneNumber;
            inputParameter.Size = 50;
            objCommand.Parameters.Add(inputParameter);

            inputParameter = new SqlParameter("@CreatedEmailAddress", SqlDbType.VarChar);
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.Value = theAccount.CreatedEmailAddress;
            inputParameter.Size = 254;
            objCommand.Parameters.Add(inputParameter);

            inputParameter = new SqlParameter("@ContactEmailAddress", SqlDbType.VarChar);
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.Value = theAccount.ContactEmailAddress;
            inputParameter.Size = 254;
            objCommand.Parameters.Add(inputParameter);

            inputParameter = new SqlParameter("@Avatar", SqlDbType.Int);
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.Value = theAccount.Avatar;
            inputParameter.Size = 4;
            objCommand.Parameters.Add(inputParameter);

            inputParameter = new SqlParameter("@AccountPassword", SqlDbType.VarBinary);
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.Value = theAccount.AccountPassword;
            // accountPasswordParm.Size = null;
            objCommand.Parameters.Add(inputParameter);

            inputParameter = new SqlParameter("@ResponseCity", SqlDbType.VarChar);
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.Value = theAccount.ResponseCity;
            inputParameter.Size = 254;
            objCommand.Parameters.Add(inputParameter);

            inputParameter = new SqlParameter("@ResponsePhone", SqlDbType.VarChar);
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.Value = theAccount.ResponsePhone;
            inputParameter.Size = 254;
            objCommand.Parameters.Add(inputParameter);

            inputParameter = new SqlParameter("@ResponseSchool", SqlDbType.VarChar);
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.Value = theAccount.ResponseSchool;
            inputParameter.Size = 254;
            objCommand.Parameters.Add(inputParameter);

            //inputParameter = new SqlParameter("@DocumentUrl", "");
            //inputParameter.Direction = ParameterDirection.Input;
            //inputParameter.SqlDbType = SqlDbType.VarChar;
            //inputParameter.Size = 254;
            //objCommand.Parameters.Add(inputParameter);

            //inputParameter = new SqlParameter("@DocumentType", "");
            //inputParameter.Direction = ParameterDirection.Input;
            //inputParameter.SqlDbType = SqlDbType.VarChar;
            //inputParameter.Size = 254;
            //objCommand.Parameters.Add(inputParameter);

            inputParameter = new SqlParameter("@Active", SqlDbType.VarChar);
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.Value = theAccount.Active;
            inputParameter.Size = 6;
            objCommand.Parameters.Add(inputParameter);

            inputParameter = new SqlParameter("@SecurityPIN", SqlDbType.Int);
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.Value = theAccount.SecurityPIN;
            inputParameter.Size = 254;
            objCommand.Parameters.Add(inputParameter);

            int output = objDB.DoUpdateUsingCmdObj(objCommand);

            if (output > 0)
                return true;
            else
                return false;
        }
    }
}
