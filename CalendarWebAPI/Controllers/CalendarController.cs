using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CalendarClassLibrary.Model;
using System.Data.SqlClient;
using Utilities;

namespace CalendarWebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CalendarController : Controller
    {
        DBConnect objDB = new DBConnect();
        int calendarListCount= 0;
        List<Calendar> calendarList = new List<Calendar>();

        [HttpGet("GetByEmailAddress")] // GET api/calendar/GetByEmailAddress?EmailAddress=abc@email.com
        //public IActionResult Get([FromQuery]string EmailAddress)
        public List<Calendar> Get([FromQuery]string EmailAddress)
        {
            if (EmailAddress != null)
            {
                SqlCommand objSqlCmd = new SqlCommand();
                objSqlCmd.CommandType = CommandType.StoredProcedure;
                objSqlCmd.CommandText = "TP_Calendar_Select_SP";

                SqlParameter emailParm = new SqlParameter("@CreatedEmailAddress", EmailAddress);
                emailParm.Direction = ParameterDirection.Input;
                emailParm.SqlDbType = SqlDbType.VarChar;
                emailParm.Size = 254;
                objSqlCmd.Parameters.Add(emailParm);

                DataSet calendarDS = objDB.GetDataSetUsingCmdObj(objSqlCmd);

                calendarListCount = calendarDS.Tables[0].Rows.Count;

                MoveToCalendarList();

                if (calendarListCount > 0)
                    return calendarList;
                else
                    return null;
            }
            else
                return null;
        }

        [HttpGet("GetByCalName")] // GET api/calendar/GetByCalName?EmailAddress=abc@email.com&CalendarName=Urgent Care
        public List<Calendar> GetByName([FromQuery]string EmailAddress,
                                 [FromQuery]string CalendarName)
        {
            if (EmailAddress != null && CalendarName != null)
            {
                SqlCommand objSqlCmd = new SqlCommand();
                objSqlCmd.CommandType = CommandType.StoredProcedure;
                objSqlCmd.CommandText = "TP_Calendar_Select_SP";

                SqlParameter emailParm = new SqlParameter("@CreatedEmailAddress", EmailAddress);
                emailParm.Direction = ParameterDirection.Input;
                emailParm.SqlDbType = SqlDbType.VarChar;
                emailParm.Size = 254;
                objSqlCmd.Parameters.Add(emailParm);

                SqlParameter nameParm = new SqlParameter("@CalendarName", CalendarName);
                nameParm.Direction = ParameterDirection.Input;
                nameParm.SqlDbType = SqlDbType.VarChar;
                nameParm.Size = 254;
                objSqlCmd.Parameters.Add(nameParm);

                DataSet calendarDS = objDB.GetDataSetUsingCmdObj(objSqlCmd);

                calendarListCount = calendarDS.Tables[0].Rows.Count;

                MoveToCalendarList();

                if (calendarListCount > 0)
                    return calendarList;
                else
                    return null;
            }
            else
                return null;
        }

        
        [HttpGet("GetByRequiredDocType")] // GET api/calendar/GetByRequiredDoc?RequiredDocType=buspass
        public List<Calendar> GetByRequiredDoc([FromQuery]string RequiredDocType)
        {
            if (RequiredDocType != null)
            {
                SqlCommand objSqlCmd = new SqlCommand();
                objSqlCmd.CommandType = CommandType.StoredProcedure;
                objSqlCmd.CommandText = "TP_Calendar_Select_SP";

                SqlParameter requiredDocType = new SqlParameter("@RequiredDocType", RequiredDocType);
                requiredDocType.Direction = ParameterDirection.Input;
                requiredDocType.SqlDbType = SqlDbType.VarChar;
                requiredDocType.Size = 254;
                objSqlCmd.Parameters.Add(requiredDocType);

                DataSet calendarDS = objDB.GetDataSetUsingCmdObj(objSqlCmd);

                calendarListCount = calendarDS.Tables[0].Rows.Count;

                MoveToCalendarList();

                if (calendarListCount > 0)
                    return calendarList;
                else
                    return null;
            }
            else
                return null;
        }
        [HttpPost("AddCalendar")] // POST api/calendar/AddCalendar?EmailAddress=abc@email.com
        [Consumes("application/json")]
        public Boolean AddCalendar([FromQuery] string EmailAddress, [FromBody] Calendar theCalendar)
        {
            SqlCommand objSqlCmd = new SqlCommand();
            objSqlCmd.CommandType = CommandType.StoredProcedure;
            objSqlCmd.CommandText = "TP_Calendar_Insert_SP";

            SqlParameter emailParm = new SqlParameter("@CreatedEmailAddress", EmailAddress);
            emailParm.Direction = ParameterDirection.Input;
            emailParm.SqlDbType = SqlDbType.VarChar;
            emailParm.Size = 254;
            objSqlCmd.Parameters.Add(emailParm);

            SqlParameter nameParm = new SqlParameter("@Name", theCalendar.Name);
            nameParm.Direction = ParameterDirection.Input;
            nameParm.SqlDbType = SqlDbType.VarChar;
            nameParm.Size = 254;
            objSqlCmd.Parameters.Add(nameParm);

            //TODO: Future feature (require documentation to schedule an event)
            //SqlParameter reqDocParm = new SqlParameter("@RequiredDoc", theCalendar.RequiredDoc);
            //reqDocParm.Direction = ParameterDirection.Input;
            //reqDocParm.SqlDbType = SqlDbType.VarChar;
            //reqDocParm.Size = 3;
            //objSqlCmd.Parameters.Add(reqDocParm);

            //SqlParameter reqDocType = new SqlParameter("@RequiredDocType", theCalendar.RequiredDocType);
            //reqDocType.Direction = ParameterDirection.Input;
            //reqDocType.SqlDbType = SqlDbType.VarChar;
            //reqDocType.Size = 254;
            //objSqlCmd.Parameters.Add(reqDocType);

            SqlParameter locParm = new SqlParameter("@Location", theCalendar.Location);
            locParm.Direction = ParameterDirection.Input;
            locParm.SqlDbType = SqlDbType.VarChar;
            locParm.Size = 254;
            objSqlCmd.Parameters.Add(locParm);

            SqlParameter descParm = new SqlParameter("@Description", theCalendar.Description);
            descParm.Direction = ParameterDirection.Input;
            descParm.SqlDbType = SqlDbType.VarChar;
            descParm.Size = 4094;
            objSqlCmd.Parameters.Add(descParm);

            SqlParameter isCheckedParm = new SqlParameter("@IsChecked", SqlDbType.Char);
            isCheckedParm.Direction = ParameterDirection.Input;
            isCheckedParm.Value = theCalendar.IsChecked;
            isCheckedParm.Size = 1;
            objSqlCmd.Parameters.Add(isCheckedParm);

            SqlParameter colorParm = new SqlParameter("@Color", theCalendar.Color);
            colorParm.Direction = ParameterDirection.Input;
            colorParm.SqlDbType = SqlDbType.VarChar;
            colorParm.Size = 22;
            objSqlCmd.Parameters.Add(colorParm);

            int returnValue = objDB.DoUpdateUsingCmdObj(objSqlCmd);

            if (returnValue > 0)
                return true;
            else
                return false;
        }
        [HttpDelete("DelCalendar")] // DELETE api/calendar/DelCalendar?EmailAddress=abc@email.com&CalendarName=abcd
        public Boolean DelCalendar([FromQuery]string EmailAddress, [FromQuery]string CalendarName)
        {
            SqlCommand objSqlCmd = new SqlCommand();
            objSqlCmd.CommandType = CommandType.StoredProcedure;
            objSqlCmd.CommandText = "TP_Calendar_Delete_SP";

            SqlParameter emailParm = new SqlParameter("@CreatedEmailAddress", EmailAddress);
            emailParm.Direction = ParameterDirection.Input;
            emailParm.SqlDbType = SqlDbType.VarChar;
            emailParm.Size = 254;
            objSqlCmd.Parameters.Add(emailParm);

            SqlParameter nameParm = new SqlParameter("@Name", CalendarName);
            nameParm.Direction = ParameterDirection.Input;
            nameParm.SqlDbType = SqlDbType.VarChar;
            nameParm.Size = 254;
            objSqlCmd.Parameters.Add(nameParm);

            int returnValue = objDB.DoUpdateUsingCmdObj(objSqlCmd);

            if (returnValue > 0)
                return true;
            else
                return false;
        }
        [HttpPut("UpdCalendar")] // PUT api/calendar/UpdCalendar?EmailAddress=abc@email.com&CalendarName=abcd
        public Boolean UpdCalendar([FromQuery]string EmailAddress, [FromQuery]string CalendarName, [FromBody] Calendar theCalendar)
        {
            SqlCommand objSqlCmd = new SqlCommand();
            objSqlCmd.CommandType = CommandType.StoredProcedure;
            objSqlCmd.CommandText = "TP_Calendar_Update_SP";

            SqlParameter emailParm = new SqlParameter("@CreatedEmailAddress", EmailAddress);
            emailParm.Direction = ParameterDirection.Input;
            emailParm.SqlDbType = SqlDbType.VarChar;
            emailParm.Size = 254;
            objSqlCmd.Parameters.Add(emailParm);

            SqlParameter nameParm = new SqlParameter("@Name", CalendarName);
            nameParm.Direction = ParameterDirection.Input;
            nameParm.SqlDbType = SqlDbType.VarChar;
            nameParm.Size = 254;
            objSqlCmd.Parameters.Add(nameParm);

            //TODO: Future feature (require documentation to schedule an event)
            //SqlParameter reqDocParm = new SqlParameter("@RequiredDoc", theCalendar.RequiredDoc);
            //reqDocParm.Direction = ParameterDirection.Input;
            //reqDocParm.SqlDbType = SqlDbType.VarChar;
            //reqDocParm.Size = 3;
            //objSqlCmd.Parameters.Add(reqDocParm);

            //SqlParameter reqDocType = new SqlParameter("@RequiredDocType", theCalendar.RequiredDocType);
            //reqDocType.Direction = ParameterDirection.Input;
            //reqDocType.SqlDbType = SqlDbType.VarChar;
            //reqDocType.Size = 254;
            //objSqlCmd.Parameters.Add(reqDocType);

            SqlParameter locParm = new SqlParameter("@Location", theCalendar.Location);
            locParm.Direction = ParameterDirection.Input;
            locParm.SqlDbType = SqlDbType.VarChar;
            locParm.Size = 254;
            objSqlCmd.Parameters.Add(locParm);

            SqlParameter descParm = new SqlParameter("@Description", theCalendar.Description);
            descParm.Direction = ParameterDirection.Input;
            descParm.SqlDbType = SqlDbType.VarChar;
            descParm.Size = 4094;
            objSqlCmd.Parameters.Add(descParm);

            SqlParameter DateTimeStampParm = new SqlParameter("@DateTimeStamp", theCalendar.DateTimeStamp);
            DateTimeStampParm.Direction = ParameterDirection.Input;
            DateTimeStampParm.SqlDbType = SqlDbType.DateTime;
            objSqlCmd.Parameters.Add(DateTimeStampParm);

            SqlParameter colorParm = new SqlParameter("@Color", theCalendar.Color);
            colorParm.Direction = ParameterDirection.Input;
            colorParm.SqlDbType = SqlDbType.VarChar;
            colorParm.Size = 22;
            objSqlCmd.Parameters.Add(colorParm);

            SqlParameter isCheckedParm = new SqlParameter("@IsChecked", SqlDbType.Char);
            isCheckedParm.Direction = ParameterDirection.Input;
            isCheckedParm.Value = theCalendar.IsChecked;
            isCheckedParm.Size = 1;
            objSqlCmd.Parameters.Add(isCheckedParm);

            int returnValue = objDB.DoUpdateUsingCmdObj(objSqlCmd);

            if (returnValue > 0)
                return true;
            else
                return false;
        }
        [HttpPut("UpdIsChecked")] // PUT api/calendar/updischecked
        public Boolean UpdIsChecked([FromBody] Calendar theCalendar)
        {
            SqlCommand objSqlCmd = new SqlCommand();
            objSqlCmd.CommandType = CommandType.StoredProcedure;
            objSqlCmd.CommandText = "TP_Calendar_Update_IsChecked_SP";

            SqlParameter acctIdParm = new SqlParameter("@AccountId", SqlDbType.Int);
            acctIdParm.Direction = ParameterDirection.Input;
            acctIdParm.Value = theCalendar.AccountId;
            acctIdParm.Size = 4;
            objSqlCmd.Parameters.Add(acctIdParm);

            SqlParameter nameParm = new SqlParameter("@Name", theCalendar.Name);
            nameParm.Direction = ParameterDirection.Input;
            nameParm.SqlDbType = SqlDbType.VarChar;
            nameParm.Size = 254;
            objSqlCmd.Parameters.Add(nameParm);

            SqlParameter isCheckedParm = new SqlParameter("@IsChecked", SqlDbType.Char);
            isCheckedParm.Direction = ParameterDirection.Input;
            isCheckedParm.Value = theCalendar.IsChecked;
            isCheckedParm.Size = 1;
            objSqlCmd.Parameters.Add(isCheckedParm);

            int returnValue = objDB.DoUpdateUsingCmdObj(objSqlCmd);

            if (returnValue > 0)
                return true;
            else
                return false;
        }

        private void MoveToCalendarList ()
        {
            for (int i = 0; i < calendarListCount; i++)
            {
                Calendar cal = new Calendar();
                cal.CalendarId = Convert.ToInt32(objDB.GetField("CalendarId", i));
                cal.AccountId = Convert.ToInt32(objDB.GetField("AccountId", i));
                cal.Name = objDB.GetField("Name", i).ToString();
                //TODO: Future feature (require documentation to schedule an event)
                //cal.RequiredDoc = objDB.GetField("RequiredDoc", i).ToString();
                //cal.RequiredDocType = objDB.GetField("RequiredDocType", i).ToString();
                cal.Location = objDB.GetField("Location", i).ToString();
                cal.Description = objDB.GetField("Description", i).ToString();
                cal.Color = objDB.GetField("Color", i).ToString();
                cal.IsChecked = objDB.GetField("IsChecked", i).ToString();
                cal.DateTimeStamp = Convert.ToDateTime(objDB.GetField("DateTimeStamp", i));
                calendarList.Add(cal);
            }
        }
    }
}
