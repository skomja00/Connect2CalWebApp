using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using CalendarClassLibrary.Model;
using Newtonsoft.Json;

namespace CalendarClassLibrary.Model
{
    public class Event
    {
        int eventId;
        int accountId;
        int calendarId;
        string title;
        DateTime begDateTime;
        DateTime endDateTime;
        string attachmentDocumentUrl;
        string location;
        string description;
        DateTime dateTimeStamp;
        string color;
        public Event ()
        {

        }
        /// <summary>
        /// Create a List of the Event class for the given calendar dates
        /// from the api/Event/GetByDate WebAPI for the given
        /// login CreatedEmailAddress.
        /// </summary>
        /// <param name="theCreatedEmailAddress"></param>
        /// <param name="theBegDateTime"></param>
        /// <param name="theEndDateTime"></param>
        /// <returns>string String of ASP:LinkButton</returns>
        public static List<Event> GetByDate(string theCreatedEmailAddress, string theCalendarName, DateTime theBegDateTime, DateTime theEndDateTime)
        {
            WebRequest request = WebRequest.Create("http://cis-iis2.temple.edu/spring2021/cis3342_tun49199/WebAPITest/api/Event/GetByDate?EmailAddress=" + theCreatedEmailAddress + "&CalName=" + theCalendarName + "&BegDateTime=" + theBegDateTime + "&EndDateTime=" + theEndDateTime);
            //WebRequest request = WebRequest.Create("https://localhost:44380/api/Event/GetByDate?EmailAddress=" + theCreatedEmailAddress + "&CalName=" + theCalendarName + "&BegDateTime=" + theBegDateTime + "&EndDateTime=" + theEndDateTime);
            WebResponse response = request.GetResponse();

            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            String data = reader.ReadToEnd();
            reader.Close();
            response.Close();

            JavaScriptSerializer js = new JavaScriptSerializer();
            List<Event> events = js.Deserialize<List<Event>>(data);

            return events;
        }
        public Event GetByEventId(int theEventId)
        {
            WebRequest request = WebRequest.Create("http://cis-iis2.temple.edu/spring2021/cis3342_tun49199/WebAPITest/api/Event/GetByEventId?EventId=" + theEventId);
            //WebRequest request = WebRequest.Create("https://localhost:44380/api/Event/GetByEventId?EventId=" + theEventId);
            WebResponse response = request.GetResponse();

            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            String data = reader.ReadToEnd();
            reader.Close();
            response.Close();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Event evt = js.Deserialize<Event>(data);

            return evt;
        }
        public Boolean AddEvent(string theCreatedEmailAddress, string theCalendarName)
        {
            string data = "";

            if (this.Title.Length > 0 &&
                theCreatedEmailAddress.Length > 0 &&
                theCalendarName.Length > 0 &&
                CheckDateTime(this.BegDateTime.ToString()) &&
                CheckDateTime(this.EndDateTime.ToString()))
            {
                try
                {
                    //serialize the object into a json string.
                    string jsonstring = JsonConvert.SerializeObject(this, Formatting.Indented);

                    WebRequest request = WebRequest.Create("http://cis-iis2.temple.edu/spring2021/cis3342_tun49199/WebAPITest/api/Event/AddEvent?EmailAddress=" + theCreatedEmailAddress + "&CalName=" + theCalendarName);
                    //WebRequest request = WebRequest.Create("https://localhost:44380/api/Event/AddEvent?EmailAddress=" + theCreatedEmailAddress + "&CalName=" + theCalendarName);
                    request.Method = "POST";
                    request.ContentLength = jsonstring.Length;
                    request.ContentType = "application/json";

                    // write the data to the body of the web request
                    StreamWriter writer = new StreamWriter(request.GetRequestStream());
                    writer.Write(jsonstring);
                    writer.Flush();
                    writer.Close();

                    // read the data from the web response
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
            else
            {
                // Something is wrong with this Event properties
                return false;
            }
        }
        public Boolean UpdEvent(string theCreatedEmailAddress, string theCalendarName)
        {
            string data = "";

            if (theCreatedEmailAddress.Length > 0 &&
                theCalendarName.Length > 0 &&
                CheckDateTime(this.BegDateTime.ToString()) &&
                CheckDateTime(this.EndDateTime.ToString()) &&
                this.BegDateTime <= this.EndDateTime)
            {
                try
                {
                    //serialize the object into a json string.
                    string jsonstring = JsonConvert.SerializeObject(this, Formatting.Indented);

                    WebRequest request = WebRequest.Create("http://cis-iis2.temple.edu/spring2021/cis3342_tun49199/WebAPITest/api/Event/UpdEvent?EmailAddress=" + theCreatedEmailAddress + "&CalName=" + theCalendarName);
                    //WebRequest request = WebRequest.Create("https://localhost:44380/api/Event/UpdEvent?EmailAddress=" + theCreatedEmailAddress + "&CalName=" + theCalendarName);
                    request.Method = "PUT";
                    request.ContentLength = jsonstring.Length;
                    request.ContentType = "application/json";

                    // write the data to the body of the web request
                    StreamWriter writer = new StreamWriter(request.GetRequestStream());
                    writer.Write(jsonstring);
                    writer.Flush();
                    writer.Close();

                    // read the data from the web response
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
            else
            {
                // Something is wrong with this Event properties
                return false;
            }
        }
        public Boolean DelEvent(string theCreatedEmailAddress, string theCalendarName)
        {
            string data = "";

            if (theCreatedEmailAddress.Length > 0 &&
                theCalendarName.Length > 0 &&
                CheckDateTime(this.BegDateTime.ToString()))
            {
                try
                {
                    //serialize the object into a json string.
                    string jsonstring = JsonConvert.SerializeObject(this, Formatting.Indented);

                    WebRequest request = WebRequest.Create("http://cis-iis2.temple.edu/spring2021/cis3342_tun49199/WebAPITest/api/Event/DelEvent?EmailAddress=" + theCreatedEmailAddress + "&CalName=" + theCalendarName);
                    //WebRequest request = WebRequest.Create("https://localhost:44380/api/Event/DelEvent?EmailAddress=" + theCreatedEmailAddress + "&CalName=" + theCalendarName);
                    request.Method = "DELETE";
                    request.ContentLength = jsonstring.Length;
                    request.ContentType = "application/json";

                    // write the data to the body of the web request
                    StreamWriter writer = new StreamWriter(request.GetRequestStream());
                    writer.Write(jsonstring);
                    writer.Flush();
                    writer.Close();

                    // read the data from the web response
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
            else
            {
                // Something is wrong with this Event properties
                return false;
            }
        }
        private bool CheckDateTime(String theDateTime)
        {
            try
            {
                DateTime dt = DateTime.Parse(theDateTime);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public int EventId
        {
            get { return eventId; }
            set { eventId = value; }
        }
        public int AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }
        public int CalendarId
        {
            get { return calendarId; }
            set { calendarId = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public DateTime BegDateTime
        {
            get { return begDateTime; }
            set { begDateTime = value; }
        }
        public DateTime EndDateTime
        {
            get { return endDateTime; }
            set { endDateTime = DateTime.SpecifyKind(value, DateTimeKind.Local); ; }
        }
        public string AttachmentDocumentUrl
        {
            get { return attachmentDocumentUrl; }
            set { attachmentDocumentUrl = value; }
        }
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
        public DateTime DateTimeStamp
        {
            get { return dateTimeStamp; }
            set { dateTimeStamp = value; }
        }
        public string Color
        {
            get { return color; }
            set { color = value; }
        }
    }
}
