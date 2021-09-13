using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CalendarClassLibrary.Model;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace CalendarWebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class EventController : Controller
    {
        DBConnect objDB = new DBConnect();
        int eventCount = 0;
        //Event evt = new Event();
        List<Event> eventList = new List<Event>();
        [HttpGet("GetByDate")] // GET api/Event/GetByDate?EmailAddress=abc@email.com&CalName=office&BegDate=mm/dd/ccyy&EndDate=mm/dd/ccyy
        public List<Event> Get([FromQuery]string EmailAddress, 
                                 [FromQuery]string CalName,
                                 [FromQuery]string BegDateTime,
                                 [FromQuery]string EndDateTime)
        {

            SqlCommand objSqlCmd = new SqlCommand();
            objSqlCmd.CommandType = CommandType.StoredProcedure;
            objSqlCmd.CommandText = "TP_Event_Select_SP";

            SqlParameter emailParm = new SqlParameter("@CreatedEmailAddress", EmailAddress);
            emailParm.Direction = ParameterDirection.Input;
            emailParm.SqlDbType = SqlDbType.VarChar;
            emailParm.Size = 254;
            objSqlCmd.Parameters.Add(emailParm);

            SqlParameter calNameParm = new SqlParameter("@CalendarName", CalName);
            calNameParm.Direction = ParameterDirection.Input;
            calNameParm.SqlDbType = SqlDbType.VarChar;
            calNameParm.Size = 254;
            objSqlCmd.Parameters.Add(calNameParm);

            SqlParameter begDateParm = new SqlParameter("@BegDateTime", BegDateTime);
            begDateParm.Direction = ParameterDirection.Input;
            begDateParm.SqlDbType = SqlDbType.DateTime;
            objSqlCmd.Parameters.Add(begDateParm);

            SqlParameter endDateParm = new SqlParameter("@EndDateTime", EndDateTime);
            endDateParm.Direction = ParameterDirection.Input;
            endDateParm.SqlDbType = SqlDbType.DateTime;
            objSqlCmd.Parameters.Add(endDateParm);

            DataSet eventDS = objDB.GetDataSetUsingCmdObj(objSqlCmd);

            eventCount = eventDS.Tables[0].Rows.Count;

            MoveToEventList();

            return eventList;
        }
        [HttpGet("GetByEventID")] // GET api/Event/GetByEventId?EventId=123456
        public Event GetByEventId([FromQuery]int EventId)
        {
            SqlCommand objSqlCmd = new SqlCommand();
            objSqlCmd.CommandType = CommandType.StoredProcedure;
            objSqlCmd.CommandText = "TP_Event_Select_By_EventId_SP";

            SqlParameter eventIdParm = new SqlParameter("@EventId", EventId);
            eventIdParm.Direction = ParameterDirection.Input;
            eventIdParm.SqlDbType = SqlDbType.Int;
            eventIdParm.Size = 4;
            objSqlCmd.Parameters.Add(eventIdParm);

            DataSet eventDS = objDB.GetDataSetUsingCmdObj(objSqlCmd);

            eventCount = eventDS.Tables[0].Rows.Count;

            Event evt = new Event();
            if (eventCount > 0)
            {
                evt.EventId = Convert.ToInt32(objDB.GetField("EventId", 0));
                evt.AccountId = Convert.ToInt32(objDB.GetField("AccountId", 0));
                evt.CalendarId = Convert.ToInt32(objDB.GetField("CalendarId", 0));
                evt.Title = objDB.GetField("Title", 0).ToString();
                evt.BegDateTime = Convert.ToDateTime(objDB.GetField("BegDateTime", 0));
                evt.EndDateTime = Convert.ToDateTime(objDB.GetField("EndDateTime", 0));
                evt.AttachmentDocumentUrl = objDB.GetField("AttachmentDocumentUrl", 0).ToString();
                evt.Location = objDB.GetField("Location", 0).ToString();
                evt.Description = objDB.GetField("Description", 0).ToString();
                evt.DateTimeStamp = Convert.ToDateTime(objDB.GetField("DateTimeStamp", 0));
                evt.Color = objDB.GetField("Color", 0).ToString();
            }
            return evt;
        }
        private void MoveToEventList()
        {
            for (int i = 0; i < eventCount; i++)
            {
                Event evt = new Event();
                evt.EventId = Convert.ToInt32(objDB.GetField("EventId", i));
                evt.AccountId = Convert.ToInt32(objDB.GetField("AccountId", i));
                evt.CalendarId = Convert.ToInt32(objDB.GetField("CalendarId", i));
                evt.Title = objDB.GetField("Title", i).ToString();
                evt.BegDateTime = Convert.ToDateTime(objDB.GetField("BegDateTime", i));
                evt.EndDateTime = Convert.ToDateTime(objDB.GetField("EndDateTime", i));
                evt.AttachmentDocumentUrl = objDB.GetField("AttachmentDocumentUrl", i).ToString();
                evt.Location = objDB.GetField("Location", i).ToString();
                evt.Description = objDB.GetField("Description", i).ToString();
                evt.Color = objDB.GetField("Color", i).ToString();
                evt.DateTimeStamp = Convert.ToDateTime(objDB.GetField("DateTimeStamp", i));
                eventList.Add(evt);
            }
        }
        [HttpPost("AddEvent")] // POST api/event/AddEvent?EmailAddress=abc@email.com&CalName=abcd
        [Consumes("application/json")]
        public Boolean AddEvent([FromQuery] string EmailAddress, [FromQuery] string CalName, [FromBody] Event theEvent)
        {
            SqlCommand objSqlCmd = new SqlCommand();
            objSqlCmd.CommandType = CommandType.StoredProcedure;
            objSqlCmd.CommandText = "TP_Event_Insert_SP";

            SqlParameter emailParm = new SqlParameter("@CreatedEmailAddress", EmailAddress);
            emailParm.Direction = ParameterDirection.Input;
            emailParm.SqlDbType = SqlDbType.VarChar;
            emailParm.Size = 254;
            objSqlCmd.Parameters.Add(emailParm);

            SqlParameter nameParm = new SqlParameter("@CalendarName", CalName);
            nameParm.Direction = ParameterDirection.Input;
            nameParm.SqlDbType = SqlDbType.VarChar;
            nameParm.Size = 254;
            objSqlCmd.Parameters.Add(nameParm);

            SqlParameter titleParm = new SqlParameter("@Title", theEvent.Title);
            titleParm.Direction = ParameterDirection.Input;
            titleParm.SqlDbType = SqlDbType.VarChar;
            titleParm.Size = 254;
            objSqlCmd.Parameters.Add(titleParm);

            SqlParameter begDateTimeParm = new SqlParameter("@BegDateTime", theEvent.BegDateTime);
            begDateTimeParm.Direction = ParameterDirection.Input;
            begDateTimeParm.SqlDbType = SqlDbType.DateTime;
            objSqlCmd.Parameters.Add(begDateTimeParm);

            SqlParameter endDateTimeParm = new SqlParameter("@EndDateTime", theEvent.EndDateTime);
            endDateTimeParm.Direction = ParameterDirection.Input;
            endDateTimeParm.SqlDbType = SqlDbType.DateTime;
            objSqlCmd.Parameters.Add(endDateTimeParm);

            SqlParameter attachParm = new SqlParameter("@AttachmentDocumentUrl", theEvent.AttachmentDocumentUrl);
            attachParm.Direction = ParameterDirection.Input;
            attachParm.SqlDbType = SqlDbType.VarChar;
            attachParm.Size = 254;
            objSqlCmd.Parameters.Add(attachParm);

            SqlParameter locParm = new SqlParameter("@Location", theEvent.Location);
            locParm.Direction = ParameterDirection.Input;
            locParm.SqlDbType = SqlDbType.VarChar;
            locParm.Size = 254;
            objSqlCmd.Parameters.Add(locParm);

            SqlParameter descParm = new SqlParameter("@Description", theEvent.Description);
            descParm.Direction = ParameterDirection.Input;
            descParm.SqlDbType = SqlDbType.VarChar;
            descParm.Size = 4094;
            objSqlCmd.Parameters.Add(descParm);

            int returnValue = objDB.DoUpdateUsingCmdObj(objSqlCmd);

            if (returnValue > 0)
                return true;
            else
                return false;
        }
        [HttpPut("UpdEvent")] // POST api/event/UpdEvent?EmailAddress=abc@email.com&CalName=abcd
        [Consumes("application/json")]
        public Boolean UpdEvent([FromQuery] string EmailAddress, [FromQuery] string CalName, [FromBody] Event theEvent)
        {
            SqlCommand objSqlCmd = new SqlCommand();
            objSqlCmd.CommandType = CommandType.StoredProcedure;
            objSqlCmd.CommandText = "TP_Event_Update_SP";

            SqlParameter emailParm = new SqlParameter("@CreatedEmailAddress", EmailAddress);
            emailParm.Direction = ParameterDirection.Input;
            emailParm.SqlDbType = SqlDbType.VarChar;
            emailParm.Size = 254;
            objSqlCmd.Parameters.Add(emailParm);

            SqlParameter nameParm = new SqlParameter("@CalendarName", CalName);
            nameParm.Direction = ParameterDirection.Input;
            nameParm.SqlDbType = SqlDbType.VarChar;
            nameParm.Size = 254;
            objSqlCmd.Parameters.Add(nameParm);

            SqlParameter titleParm = new SqlParameter("@Title", theEvent.Title);
            titleParm.Direction = ParameterDirection.Input;
            titleParm.SqlDbType = SqlDbType.VarChar;
            titleParm.Size = 254;
            objSqlCmd.Parameters.Add(titleParm);

            SqlParameter begDateTimeParm = new SqlParameter("@BegDateTime", theEvent.BegDateTime);
            begDateTimeParm.Direction = ParameterDirection.Input;
            begDateTimeParm.SqlDbType = SqlDbType.DateTime;
            objSqlCmd.Parameters.Add(begDateTimeParm);

            SqlParameter endDateTimeParm = new SqlParameter("@EndDateTime", theEvent.EndDateTime);
            endDateTimeParm.Direction = ParameterDirection.Input;
            endDateTimeParm.SqlDbType = SqlDbType.DateTime;
            objSqlCmd.Parameters.Add(endDateTimeParm);

            SqlParameter attachParm = new SqlParameter("@AttachmentDocumentUrl", theEvent.AttachmentDocumentUrl);
            attachParm.Direction = ParameterDirection.Input;
            attachParm.SqlDbType = SqlDbType.VarChar;
            attachParm.Size = 254;
            objSqlCmd.Parameters.Add(attachParm);

            SqlParameter locParm = new SqlParameter("@Location", theEvent.Location);
            locParm.Direction = ParameterDirection.Input;
            locParm.SqlDbType = SqlDbType.VarChar;
            locParm.Size = 254;
            objSqlCmd.Parameters.Add(locParm);

            SqlParameter descParm = new SqlParameter("@Description", theEvent.Description);
            descParm.Direction = ParameterDirection.Input;
            descParm.SqlDbType = SqlDbType.VarChar;
            descParm.Size = 4094;
            objSqlCmd.Parameters.Add(descParm);

            int returnValue = objDB.DoUpdateUsingCmdObj(objSqlCmd);

            if (returnValue > 0)
                return true;
            else
                return false;
        }
        [HttpDelete("DelEvent")] // DELETE api/event/DelEvent?EmailAddress=abc@email.com&CalName=abcd
        [Consumes("application/json")]
        public Boolean DelEvent([FromQuery] string EmailAddress, [FromQuery] string CalName, [FromBody] Event theEvent)
        {
            SqlCommand objSqlCmd = new SqlCommand();
            objSqlCmd.CommandType = CommandType.StoredProcedure;
            objSqlCmd.CommandText = "TP_Event_Delete_SP";

            SqlParameter emailParm = new SqlParameter("@CreatedEmailAddress", EmailAddress);
            emailParm.Direction = ParameterDirection.Input;
            emailParm.SqlDbType = SqlDbType.VarChar;
            emailParm.Size = 254;
            objSqlCmd.Parameters.Add(emailParm);

            SqlParameter nameParm = new SqlParameter("@CalendarName", CalName);
            nameParm.Direction = ParameterDirection.Input;
            nameParm.SqlDbType = SqlDbType.VarChar;
            nameParm.Size = 254;
            objSqlCmd.Parameters.Add(nameParm);

            SqlParameter begDateTimeParm = new SqlParameter("@BegDateTime", theEvent.BegDateTime);
            begDateTimeParm.Direction = ParameterDirection.Input;
            begDateTimeParm.SqlDbType = SqlDbType.DateTime;
            objSqlCmd.Parameters.Add(begDateTimeParm);

            int returnValue = objDB.DoUpdateUsingCmdObj(objSqlCmd);

            if (returnValue > 0)
                return true;
            else
                return false;
        }
    }
}