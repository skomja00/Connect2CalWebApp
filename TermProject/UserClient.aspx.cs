using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utilities;
using CalendarClassLibrary.Model;
using System.Text.RegularExpressions;
using System.Collections;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing;

namespace TermProject
{
    public partial class UserClient : System.Web.UI.Page
    {
        CheckBox chkBoxSenderCalendar;
        String[] gvEmailDataKeyNames = new string[] { "AccountId", "CalendarId", "EventId" };
        List<CalendarClassLibrary.Model.Calendar> calendars;

        protected void Page_Load(object sender, EventArgs e)
        {
            UpdateUserClientHelpModal();
            //Get the List of calendars used for the sidebar and dropdown control
            CreateSidebarCalLinkButtons(Session["CreatedEmailAddress"].ToString());
            if (!IsPostBack)
            {
                if (Session["CalBegDate"] == null) Session["CalBegDate"] = DateTime.Today;

                MakeVisible();
                DisplayEvents();
                BindCalendarDropDownLists();
            }
            lblUserName.Text = Session["UserName"].ToString();

            // Start of notes on Avatars: 
            // A full ArrayList is created in the Application object in the Application_Start.
            // Include the markup below on any page, and update the Avatar in the code-behind 
            // using the following code for the ArrayList, ChangeUrlHrefValue(), and InnerHtml.
            // Style the width and height as necessary.
            //
            //<div id="svgAvatar" runat="server">
            //    <svg viewBox="0 0 534 534" width="50" height="50">
            //        <use href="..."/>
            //    </svg>
            //</div>
            ArrayList arrayListAvatar = (ArrayList)Application["ArrayListAvatar"];
            //ChangeUrlHrefValue (string theAttribute, string theStringToSearch, string theNewStringValue)
            string newUrlHref = ChangeUrlHrefValue("href",
                                                    svgAvatar.InnerText,
                                                    "Images/Avatars/" + arrayListAvatar[Convert.ToInt32(Session["Avatar"])]);
            svgAvatar.InnerHtml = newUrlHref;
        }
        private string ChangeUrlHrefValue(string theAttribute, string theStringToSearch, string theNewStringValue)
        {
            // Define a regular expression for repeated words.
            // ie. href="([^"]*)" matches everything between double-quotes
            string newURLHref = Regex.Replace(theStringToSearch,
                                theAttribute + "=" + "\"([^ \"]*)\"",
                                theAttribute + "=\"" + theNewStringValue + "#Capa_1" + "\"",
                                RegexOptions.Compiled | RegexOptions.Multiline);
            return newURLHref;
        }
        // End of notes on Avatars: 
        protected void calBegDate_SelectionChanged(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.Calendar cal = (System.Web.UI.WebControls.Calendar)sender;
            DateTime selectedDate = cal.SelectedDate;
            Session["CalBegDate"] = selectedDate;
            DisplayEvents();
        }
        protected void calBegDate_OnDayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.IsToday)
            {
                e.Cell.BackColor = System.Drawing.Color.LightGray;
            }
        }
        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            //Session.Abandon(); 
            HttpCookie delCookie = Request.Cookies["CIS3342_Calendar"];
            //If RememberMe hasn't been selected the cookie will not exist.
            if (delCookie != null)
            {
                delCookie.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(delCookie);
            }
            Response.Redirect("Login.aspx");
        }
        protected void CreateSidebarCalLinkButtons(string theCreatedEmailAddress)
        {
            plcSideBarCalList.Controls.Clear();
            calendars = CalendarClassLibrary.Model.Calendar.GetCalendarList(theCreatedEmailAddress);
            
            for (int i = 0; calendars != null && i < calendars.Count; i++)
            {
                CheckBox chkBox = new CheckBox();
                chkBox.CssClass = "list-group-item list-group-item-action font-weight-normal text-left m-2";
                chkBox.ID = "chkBoxSelectCal" + i.ToString();
                chkBox.Text = calendars[i].Name;
                chkBox.CheckedChanged += DisplayEventContent;
                chkBox.AutoPostBack = true;
                chkBox.BackColor = Color.FromName(calendars[i].Color);
                if (calendars[i].IsChecked == "y")
                    chkBox.Checked = true;
                else
                    chkBox.Checked = false;

                plcSideBarCalList.Controls.Add(chkBox);
            }
            Button btnCreateNewCal = new Button();
            btnCreateNewCal.CssClass = "list-group-item list-group-item-action font-weight-normal text-left text-underline m-2";
            btnCreateNewCal.Font.Underline = true;
            btnCreateNewCal.ID = "btnCreateNewCal";
            btnCreateNewCal.Text = "Calendar Options";
            btnCreateNewCal.Click += btnCalOptions_Click;
            plcSideBarCalList.Controls.Add(btnCreateNewCal);
        }
        protected void btnCalOptions_Click(object sender, EventArgs e)
        {
            MakeVisible("calOptions");
        }
        protected void BuildColorDropDownList ()
        {
            ddlColor.Items.Clear();
            string[] colors = Enum.GetNames(typeof(KnownColor));
            int colorCount = colors.Count();
            int index = 0;
            for (int i = 0; i < colorCount; i++)
            {
                if (!Color.FromName(colors[i]).IsSystemColor)
                {
                    ddlColor.Items.Insert(index, colors[i]);
                    ddlColor.Items[index].Attributes.Add("style", "background-color:" + colors[i]);
                    index++;
                }
            }
            ddlColor.Items.Insert(0, "Select Color...");
        }
        /// <summary>
        /// Create a List of Event type objects and MakeVisible the content on the page.
        /// This can be either called from a Page_Load OR from end event postback so 
        /// make note of the sender in case it is a postback.
        /// </summary>
        protected void DisplayEventContent(object sender, EventArgs e)
        {
            /// Get the sender if the user clicked a sidebar calendar
            // checkbox. other button events also call this
            // method but we only need the sender if it was from 
            // a sidebar calendar checkbox.
            if (sender.GetType().Name == "CheckBox")
            {
                chkBoxSenderCalendar = (CheckBox)sender;
            }
            DisplayEvents();
        }
        /// <summary>
        /// While creating the list of events, update
        /// TP_Calendar.IsChecked if the sender called this
        /// from an event postback where the user
        /// clicked one of the sidebar calendar checkboxes.
        /// </summary>        
        protected void DisplayEvents()
        {
            string createdEmailAddress = Session["CreatedEmailAddress"].ToString();
            DateTime begDateTime = (DateTime) Session["CalBegDate"];
            DateTime endDateTime = begDateTime.AddYears(+2);

            List<Event> allCheckedCalEvents = new List<Event>(); ;
            for (int i = 0; i < plcSideBarCalList.Controls.Count; i++)
            {
                CheckBox check = (CheckBox)plcSideBarCalList.FindControl("chkBoxSelectCal" + i.ToString());
                if (check != null)
                {
                    // for postbacks update IsChecked to maintain the state of the sidebar calendars
                    if (chkBoxSenderCalendar != null && chkBoxSenderCalendar.Text == check.Text)
                    {
                        CalendarClassLibrary.Model.Calendar calCheckUpdate = new CalendarClassLibrary.Model.Calendar();
                        calCheckUpdate.AccountId = Convert.ToInt32(Session["AccountId"]);
                        calCheckUpdate.Name = chkBoxSenderCalendar.Text;
                        if (check.Checked)
                            calCheckUpdate.IsChecked = "y";
                        else
                            calCheckUpdate.IsChecked = "n";
                        calCheckUpdate.UpdCalendarIsChecked();
                    }
                    if (check.Checked)
                    {
                        List<Event> listEvent = Event.GetByDate(Session["CreatedEmailAddress"].ToString(),
                                                                check.Text,
                                                                begDateTime,
                                                                endDateTime);
                        for (int j = 0; j < listEvent.Count; j++)
                        {
                            allCheckedCalEvents.Add(listEvent[j]);
                        }
                    }
                }
            }
            MakeVisible("calView");
            gvCalView.DataSource = allCheckedCalEvents;
            gvCalView.DataKeyNames = gvEmailDataKeyNames;
            gvCalView.DataBind();
        }
        protected void gvCalView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.BackColor = Color.FromName(DataBinder.Eval(e.Row.DataItem, "Color").ToString());
            }
        }
        /// <summary>
        /// Create an instance of Calendar, and call AddCalendar() 
        /// to add the calendar to the database.
        /// Alert the user to the result of the database update.
        /// </summary>
        protected void btnAddCalendar_Click(object sender, EventArgs e)
        {
            if (txtCalNewName.Text.Length > 0 &&
                ddlColor.SelectedIndex > 0)
            {
                CalendarClassLibrary.Model.Calendar calendar = new CalendarClassLibrary.Model.Calendar();
                calendar.Name = txtCalNewName.Text;
                //TODO: Future feature to create documentation requirments to schedule certain events
                //calendar.RequiredDoc = txtRequiredDoc.Text;
                //calendar.RequiredDocType = txtRequiredDocType.Text;
                calendar.Location = txtLocation.Text;
                calendar.Description = txtDesc.Text;
                calendar.IsChecked = txtIsChecked.Text;
                calendar.Color = ddlColor.SelectedValue;
                ddlColor.SelectedIndex = 0;

                Boolean result = calendar.AddCalendar(Session["CreatedEmailAddress"].ToString());

                if (result == true)
                {
                    Response.Write("<script>alert('Save calendar was successful.')</script>");
                    txtCalNewName.Text = "";
                    //TODO: Future feature to create documentation requirments to schedule certain events
                    //txtRequiredDoc.Text = "";
                    //txtRequiredDocType.Text = "";
                    txtLocation.Text = "";
                    txtDesc.Text = "";
                    txtIsChecked.Text = "";
                    CreateSidebarCalLinkButtons(Session["CreatedEmailAddress"].ToString());
                    BindCalendarDropDownLists();
                }
                else
                {
                    Response.Write("<script>alert('A problem occurred. The data was not recorded.')</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Something is wrong. Please check your input.')</script>");
            }
            MakeVisible("calOptions");
        }
        protected void btnUpdCalendar_Click(object sender, EventArgs e)
        {
            if (ddlCalOptCalendarName.SelectedIndex > 0 &&
                ddlColor.SelectedIndex > 0)
            {
                CalendarClassLibrary.Model.Calendar calendar = new CalendarClassLibrary.Model.Calendar();
                calendar.Name = ddlCalOptCalendarName.SelectedItem.Text;
                //TODO: Future feature to create documentation requirements to schedule certain events
                //calendar.RequiredDoc = txtRequiredDoc.Text;
                //calendar.RequiredDocType = txtRequiredDocType.Text;
                calendar.Location = txtLocation.Text;
                calendar.Description = txtDesc.Text;
                calendar.DateTimeStamp = DateTime.Now;
                calendar.Color = ddlColor.SelectedValue;
                calendar.IsChecked = txtIsChecked.Text;
                ddlColor.SelectedIndex = 0;

                Boolean result = calendar.UpdCalendar(Session["CreatedEmailAddress"].ToString(), ddlCalOptCalendarName.SelectedItem.Text);

                if (result == true)
                {
                    Response.Write("<script>alert('Update calendar was successful.')</script>");
                    ddlCalOptCalendarName.SelectedIndex = 0;
                    //TODO: Future feature to create documentation requirments to schedule certain events
                    //txtRequiredDoc.Text = "";
                    //txtRequiredDocType.Text = "";
                    txtLocation.Text = "";
                    txtDesc.Text = "";
                    txtIsChecked.Text = "";
                    CreateSidebarCalLinkButtons(Session["CreatedEmailAddress"].ToString());
                }
                else
                {
                    Response.Write("<script>alert('A problem occurred. The data was not recorded.')</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Something is wrong. Please check your input.')</script>");
            }
            MakeVisible("calOptions");
        }
        protected void btnDelCalendar_Click(object sender, EventArgs e)
        {
            if (ddlCalOptCalendarName.SelectedIndex > 0)
            {
                CalendarClassLibrary.Model.Calendar calendar = new CalendarClassLibrary.Model.Calendar();
                calendar.Name = ddlCalOptCalendarName.SelectedItem.Text; 

                Boolean result = calendar.DelCalendar(Session["CreatedEmailAddress"].ToString(), ddlCalOptCalendarName.SelectedItem.Text);

                if (result == true)
                {
                    Response.Write("<script>alert('Delete calendar was successful.')</script>");
                    //TODO: Future feature to create documentation requirments to schedule certain events
                    //txtRequiredDoc.Text = "";
                    //txtRequiredDocType.Text = "";
                    txtLocation.Text = "";
                    txtDesc.Text = "";
                    txtIsChecked.Text = "";
                    CreateSidebarCalLinkButtons(Session["CreatedEmailAddress"].ToString());
                    BindCalendarDropDownLists();
                }
                else
                {
                    Response.Write("<script>alert('A problem occurred. The data was not recorded.')</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Something is wrong. Please check your input.')</script>");
            }
            MakeVisible("calOptions");
        }
        protected void gvCalView_SelectedIndexChanged(object sender, EventArgs e)
        {
            Event evt = new Event();
            evt.EventId = Convert.ToInt32(gvCalView.DataKeys[gvCalView.SelectedIndex].Values["EventId"]);

            Event result = evt.GetByEventId(evt.EventId);

            if (result.EventId != 0)
            {
                txtCalEvtTitle.Text = result.Title.ToString();
                txtCalEvtBegDateTime.Text = result.BegDateTime.ToString();
                txtCalEvtEndDateTime.Text = result.EndDateTime.ToString();
                //TODO: Future feature (attach documents to your Account and/or Calendar Event)
                //txtCalEvtAttachmentDocumentUrl.Text = result.AttachmentDocumentUrl.ToString();
                txtCalEvtLocation.Text = result.Location.ToString();
                txtCalEvtDescription.Text = result.Description.ToString();
                MakeVisible("eventOptions");
            }
            else
            {
                Response.Write("<script>alert('A problem occurred. The data was not recorded.')</script>");
            }

        }
        protected void btnAddEvent_Click(object sender, EventArgs e)
        {
            if (ddlEvtOptCalendarName.SelectedIndex > 0 &&
                CheckDateTime(txtCalEvtBegDateTime.Text) &&
                CheckDateTime(txtCalEvtEndDateTime.Text) && 
                Convert.ToDateTime(txtCalEvtEndDateTime.Text) >= 
                Convert.ToDateTime(txtCalEvtBegDateTime.Text))
            {
                Event objEvent = new Event();
                objEvent.Title = txtCalEvtTitle.Text;
                objEvent.Description = txtCalEvtDescription.Text;
                objEvent.BegDateTime = Convert.ToDateTime(txtCalEvtBegDateTime.Text);
                objEvent.EndDateTime = Convert.ToDateTime(txtCalEvtEndDateTime.Text);
                //TODO: Future feature(attach documents to your Account and/ or Calendar Event)
                //objEvent.AttachmentDocumentUrl = txtCalEvtAttachmentDocumentUrl.Text;
                objEvent.Location = txtCalEvtLocation.Text;

                Boolean result = objEvent.AddEvent(Session["CreatedEmailAddress"].ToString(),
                                                    ddlEvtOptCalendarName.SelectedItem.Text);

                if (result == true)
                {
                    Response.Write("<script>alert('Save was successful.')</script>");
                    ddlEvtOptCalendarName.SelectedIndex = 0;
                    //TODO: Future feature to create documentation requirments to schedule certain events
                    //txtRequiredDoc.Text = "";
                    //txtRequiredDocType.Text = "";
                    txtCalEvtTitle.Text = "";
                    txtCalEvtBegDateTime.Text = "";
                    txtCalEvtEndDateTime.Text = "";
                    txtCalEvtLocation.Text = "";
                    txtCalEvtDescription.Text = "";
                    CreateSidebarCalLinkButtons(Session["CreatedEmailAddress"].ToString());
                }
                else
                {
                    Response.Write("<script>alert('A problem occurred. The data was not recorded.')</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Something is wrong. Please check your input.')</script>");
            }
            ddlEvtOptCalendarName.SelectedIndex = 0;
            MakeVisible("eventOptions");
        }

        protected void btnUpdEvent_Click(object sender, EventArgs e)
        {
            if (ddlEvtOptCalendarName.SelectedIndex > 0 &&
                CheckDateTime(txtCalEvtBegDateTime.Text) &&
                CheckDateTime(txtCalEvtEndDateTime.Text))
            {
                Event objEvent = new Event();
                objEvent.Title = txtCalEvtTitle.Text;
                objEvent.Description = txtCalEvtDescription.Text;
                objEvent.BegDateTime = Convert.ToDateTime(txtCalEvtBegDateTime.Text);
                objEvent.EndDateTime = Convert.ToDateTime(txtCalEvtEndDateTime.Text);
                //TODO: Future feature (attach documents to your Account and/or Calendar Event)
                //objEvent.AttachmentDocumentUrl = txtCalEvtAttachmentDocumentUrl.Text;
                objEvent.Location = txtCalEvtLocation.Text;

                Boolean result = objEvent.UpdEvent(Session["CreatedEmailAddress"].ToString(),
                                                    ddlEvtOptCalendarName.SelectedItem.Text);

                if (result == true)
                {
                    Response.Write("<script>alert('Save was successful.')</script>");
                    ddlEvtOptCalendarName.SelectedIndex = 0;
                    //TODO: Future feature to create documentation requirments to schedule certain events
                    //txtRequiredDoc.Text = "";
                    //txtRequiredDocType.Text = "";
                    txtCalEvtTitle.Text = "";
                    txtCalEvtBegDateTime.Text = "";
                    txtCalEvtEndDateTime.Text = "";
                    txtCalEvtLocation.Text = "";
                    txtCalEvtDescription.Text = "";
                    CreateSidebarCalLinkButtons(Session["CreatedEmailAddress"].ToString());
                }
                else
                {
                    Response.Write("<script>alert('A problem occurred. The data was not recorded.')</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Something is wrong. Please check your input.')</script>");
            }
            ddlEvtOptCalendarName.SelectedIndex = 0;
            MakeVisible("eventOptions");
        }

        protected void btnDelEvent_Click(object sender, EventArgs e)
        {
            if (ddlEvtOptCalendarName.SelectedIndex > 0 &&
                CheckDateTime(txtCalEvtBegDateTime.Text))
            {
                Event objEvent = new Event();
                objEvent.BegDateTime = Convert.ToDateTime(txtCalEvtBegDateTime.Text);
                
                Boolean result = objEvent.DelEvent(Session["CreatedEmailAddress"].ToString(),
                                                    ddlEvtOptCalendarName.SelectedItem.Text);

                if (result == true)
                {
                    Response.Write("<script>alert('Delete was successful.')</script>");
                    ddlEvtOptCalendarName.SelectedIndex = 0;
                    //TODO: Future feature to create documentation requirments to schedule certain events
                    //txtRequiredDoc.Text = "";
                    //txtRequiredDocType.Text = "";
                    txtCalEvtTitle.Text = "";
                    txtCalEvtBegDateTime.Text = "";
                    txtCalEvtEndDateTime.Text = "";
                    txtCalEvtLocation.Text = "";
                    txtCalEvtDescription.Text = "";
                    CreateSidebarCalLinkButtons(Session["CreatedEmailAddress"].ToString());
                }
                else
                {
                    Response.Write("<script>alert('A problem occurred. The data was not recorded.')</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Something is wrong. Please check your input.')</script>");
            }
            ddlEvtOptCalendarName.SelectedIndex = 0;
            MakeVisible("eventOptions");
        }

        protected void btnAddCalEvent_Click(object sender, EventArgs e)
        {
            MakeVisible("eventOptions");
        }
        protected void MakeVisible(string theId = "")
        {
            calView.Visible = false;
            calOptions.Visible = false;
            calendarButtons.Visible = false;
            eventOptions.Visible = false;
            eventButtons.Visible = false;
            switch (theId)
            {
                case "calOptions":
                    BuildColorDropDownList();
                    calendarButtons.Visible = true;
                    calOptions.Visible = true;
                    break;
                case "eventOptions":
                    eventButtons.Visible = true;
                    eventOptions.Visible = true;
                    break;
                case "calView":
                    calView.Visible = true;
                    break;
                default:
                    break;
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
        protected void UpdateUserClientHelpModal()
        {
            hmUserClientHelpModal.ButtonText = "Help";
            hmUserClientHelpModal.Content =
                "<h1>Connect2Cal Help</h1> " +
                "<p>\"<u>Calendar Options</u>\" Create and maintain your calendars. Choose a color to highlight the calendar and all events in the calendar." +
                "<p>\"<u>Event Options</u>\" Add and maintain events/appointments. For each event, your account, calendar, and event beginning date/time uniquely identify an event." +
                "<h3>Connect2Cal uses .NET Core RESTful WebAPIs</h3> " +
                "<p>Two Controllers are available:</p> " +
                "<ul> " +
                "    <li>Calendar Controller: POST, GET, PUT, and DELETE actions for Calendars.</li> " +
                "    <li>Event Controller: POST, GET, PUT, and DELETE actions for Events/appointments.</li>" +
                "</ul> " +
                "<h3>Dynamic content</h3> " +
                "<p>The calendars in the sidebar are dynamically created CheckBox controls from a list of Calendar objects returned from an HTTP GET request of the Calendar controller. A Button is added to the end for the \"<u>Calendar Options</u>\".</p> " +
                "<p>Click <a target=\"_blank\" href=\"PDF/calendar-db-install.pdf\">here</a> for the database scripts, and <a target=\"_blank\" href=\"PDF/Calendar-ERD.pdf\">here</a> for a simple database ERD diagram.";
        }
        protected void BindCalendarDropDownLists()
        {
            ddlEvtOptCalendarName.DataSource = calendars;
            ddlEvtOptCalendarName.DataTextField = "Name";
            ddlEvtOptCalendarName.DataValueField = "CalendarId";
            ddlEvtOptCalendarName.DataBind();
            ListItem selectCal = new ListItem();
            selectCal.Text = "Select Calendar...";
            ddlEvtOptCalendarName.Items.Insert(0, selectCal);
            ddlEvtOptCalendarName.SelectedIndex = 0;

            ddlCalOptCalendarName.DataSource = calendars;
            ddlCalOptCalendarName.DataTextField = "Name";
            ddlCalOptCalendarName.DataValueField = "CalendarId";
            ddlCalOptCalendarName.DataBind();
            ddlCalOptCalendarName.Items.Insert(0, selectCal);
            ddlCalOptCalendarName.SelectedIndex = 0;
        }

        protected void ddlCalOptCalendarName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCalOptCalendarName.SelectedIndex > 0)
            {
                int selectedIndex = ddlCalOptCalendarName.SelectedIndex - 1;
                txtLocation.Text = calendars[selectedIndex].Location;
                txtDesc.Text = calendars[selectedIndex].Description;
                txtIsChecked.Text = calendars[selectedIndex].IsChecked;
                BuildColorDropDownList();
                for (int i = 0; i < ddlColor.Items.Count; i++)
                {
                    if (ddlColor.Items[i].Text == calendars[selectedIndex].Color)
                    {
                        ddlColor.SelectedIndex = i;
                    }
                }
            }
        }
    }
}