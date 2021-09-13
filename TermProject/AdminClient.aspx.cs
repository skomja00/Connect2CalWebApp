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
using System.Collections;
using System.Text.RegularExpressions;

namespace TermProject
{
    public partial class AdminClient : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblUserName.Text = Session["UserName"].ToString();
            ArrayList arrayListAvatar = (ArrayList)Application["arrayListAvatar"];

            //ChangeUrlHrefValue (string theAttribute, string theStringToSearch, string theNewStringValue)
            string newUrlHref = ChangeUrlHrefValue("href",
                                                    svgAvatar.InnerText,
                                                    "Images/Avatars/" + arrayListAvatar[Convert.ToInt32(Session["Avatar"])] + "#Capa_1");
            svgAvatar.InnerHtml = newUrlHref;
        }
        private string ChangeUrlHrefValue(string theAttribute, string theStringToSearch, string theNewStringValue)
        {
            // Define a regular expression for repeated words.
            // ie. href="([^"]*)" matches everything between double-quotes
            return Regex.Replace(theStringToSearch,
                                theAttribute + "=" + "\"([^ \"]*)\"",
                                theAttribute + "=\"" + theNewStringValue + "\"",
                                RegexOptions.Compiled | RegexOptions.Multiline);
        }
        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            HttpCookie delCookie = Request.Cookies["CIS3342_Calendar"];
            //If RememberMe hasn't been selected the cookie will not exist.
            if (delCookie != null)
            {
                delCookie.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(delCookie);
            }
            Response.Redirect("Login.aspx");
        }
    }
}