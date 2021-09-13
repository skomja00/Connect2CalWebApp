using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Utilities;
using CalendarClassLibrary.Model;
using System.Text.RegularExpressions;
using System.Collections;
using System.Net;
using System.IO;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;

namespace CalendarClassLibrary.Model
{
    public class Calendar
    {
        private int calendarId;
        private int accountId;
        private string name;
        //TODO: Future feature (require documentation to schedule an event)
        //private string requiredDoc;
        //private string requiredDocType;
        private string location;
        private string description;
        private string color;
        private string isChecked;
        private DateTime dateTimeStamp;
        public Calendar ()
        {

        }
        /// <summary>
        /// Create a List of the Calendar class for each calendar returned
        /// from the api/Calendar/GetByEmailAddress WebAPI for the given
        /// login CreatedEmailAddress.
        /// </summary>
        /// <param name="theCreatedEmailAddres"></param>
        /// <returns>string String of ASP:LinkButton</returns>
        public static List<Calendar> GetCalendarList(string theCreatedEmailAddress)
        {
            WebRequest request = WebRequest.Create("http://cis-iis2.temple.edu/spring2021/cis3342_tun49199/WebAPITest/api/calendar/GetByEmailAddress?EmailAddress=" + theCreatedEmailAddress);
            //WebRequest request = WebRequest.Create("https://localhost:44380/api/calendar/GetByEmailAddress?EmailAddress=" + theCreatedEmailAddress);
            request.Method = "GET";
            WebResponse response = request.GetResponse();

            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            String data = reader.ReadToEnd();
            reader.Close();
            response.Close();

            JavaScriptSerializer js = new JavaScriptSerializer();
            List<Calendar> calendars = js.Deserialize<List<Calendar>>(data);
            
            return calendars;
        }
        public Boolean AddCalendar (string theCreatedEmailAddress)
        {
            string data = "";
            //serialize the object into a json string.
            string jsonstring = JsonConvert.SerializeObject(this, Formatting.Indented);

            try
            {
                WebRequest request = WebRequest.Create("http://cis-iis2.temple.edu/spring2021/cis3342_tun49199/WebAPITest/api/calendar/addcalendar?emailaddress=" + theCreatedEmailAddress);
                //WebRequest request = WebRequest.Create("https://localhost:44380/api/calendar/addcalendar?emailaddress=" + theCreatedEmailAddress);
                request.Method = "POST";
                request.ContentLength = jsonstring.Length;
                request.ContentType = "application/json";

                // write the json data to the web request
                StreamWriter writer = new StreamWriter(request.GetRequestStream());
                writer.Write(jsonstring);
                writer.Flush();
                writer.Close();

                // read the data from the web response, which requires working with streams.
                WebResponse response = request.GetResponse();
                Stream thedatastream = response.GetResponseStream();
                StreamReader reader = new StreamReader(thedatastream);
                data = reader.ReadToEnd();
                reader.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                return false;
            }
            if (data == "true")
                return true;
            else
                return false;
        }
        public Boolean DelCalendar(string theCreatedEmailAddress, string theCalendarName)
        {
            string data = "";
            //serialize this instance into a json string.
            //NewtonSoft.json does not serialize dates to UTC
            string jsonstring = JsonConvert.SerializeObject(this, Formatting.Indented);


            try
            {
                WebRequest request = WebRequest.Create("http://cis-iis2.temple.edu/spring2021/cis3342_tun49199/WebAPITest/api/calendar/delcalendar?emailaddress=" + theCreatedEmailAddress + "&" + "CalendarName=" + theCalendarName);
                //WebRequest request = WebRequest.Create("https://localhost:44380/api/calendar/delcalendar?emailaddress=" + theCreatedEmailAddress + "&" + "CalendarName=" + theCalendarName);
                request.Method = "DELETE";
                request.ContentLength = jsonstring.Length;
                request.ContentType = "application/json";

                // write the json data to the web request
                StreamWriter writer = new StreamWriter(request.GetRequestStream());
                writer.Write(jsonstring);
                writer.Flush();
                writer.Close();

                // read the data from the web response, which requires working with streams.
                WebResponse response = request.GetResponse();
                Stream thedatastream = response.GetResponseStream();
                StreamReader reader = new StreamReader(thedatastream);
                data = reader.ReadToEnd();
                reader.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                return false;
            }
            if (data == "true")
                return true;
            else
                return false;
        }
        public Boolean UpdCalendar(string theCreatedEmailAddress, string theCalendarName)
        {
            string data = "";
            //serialize this instance into a json string.
            //NewtonSoft.json does not serialize dates to UTC
            string jsonstring = JsonConvert.SerializeObject(this, Formatting.Indented);


            try
            {
                WebRequest request = WebRequest.Create("http://cis-iis2.temple.edu/spring2021/cis3342_tun49199/WebAPITest/api/calendar/updcalendar?emailaddress=" + theCreatedEmailAddress + "&" + "CalendarName=" + theCalendarName);
                //WebRequest request = WebRequest.Create("https://localhost:44380/api/calendar/updcalendar?emailaddress=" + theCreatedEmailAddress + "&" + "CalendarName=" + theCalendarName);
                request.Method = "PUT";
                request.ContentLength = jsonstring.Length;
                request.ContentType = "application/json";

                // write the json data to the web request
                StreamWriter writer = new StreamWriter(request.GetRequestStream());
                writer.Write(jsonstring);
                writer.Flush();
                writer.Close();

                // read the data from the web response, which requires working with streams.
                WebResponse response = request.GetResponse();
                Stream thedatastream = response.GetResponseStream();
                StreamReader reader = new StreamReader(thedatastream);
                data = reader.ReadToEnd();
                reader.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                return false;
            }
            if (data == "true")
                return true;
            else
                return false;
        }
        public Boolean UpdCalendarIsChecked()
        {
            string data = "";
            //serialize this instance into a json string.
            //NewtonSoft.json does not serialize dates to UTC
            string jsonstring = JsonConvert.SerializeObject(this, Formatting.Indented);

            try
            {
                WebRequest request = WebRequest.Create("http://cis-iis2.temple.edu/spring2021/cis3342_tun49199/WebAPITest/api/calendar/updischecked");
                //WebRequest request = WebRequest.Create("https://localhost:44380/api/calendar/updischecked");
                request.Method = "PUT";
                request.ContentLength = jsonstring.Length;
                request.ContentType = "application/json";

                // write the json data to the web request
                StreamWriter writer = new StreamWriter(request.GetRequestStream());
                writer.Write(jsonstring);
                writer.Flush();
                writer.Close();

                // read the data from the web response, which requires working with streams.
                WebResponse response = request.GetResponse();
                Stream thedatastream = response.GetResponseStream();
                StreamReader reader = new StreamReader(thedatastream);
                data = reader.ReadToEnd();
                reader.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                return false;
            }
            if (data == "true")
                return true;
            else
                return false;
        }
        public int CalendarId
        {
            get { return calendarId; }
            set { calendarId = value; }
        }
        public int AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        //TODO: Future feature (require documentation to schedule an event)
        //public string RequiredDoc
        //{
        //    get { return requiredDoc; }
        //    set { requiredDoc = value; }
        //}
        //public string RequiredDocType
        //{
        //    get { return requiredDocType; }
        //    set { requiredDocType = value; }
        //}
        public string Location
        {
            get { return location; }
            set { location = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string Color
        {
            get { return color; }
            set { color = value; }
        }
        public string IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }
        public DateTime DateTimeStamp
        {
            get { return dateTimeStamp; }
            set { dateTimeStamp = value; }
        }
    }
}
