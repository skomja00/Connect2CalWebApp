<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserClient.aspx.cs" Inherits="TermProject.UserClient" %>

<%@ Register src="ModalUserControl.ascx" TagName="ModalControl" TagPrefix="mc"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Connect2Cal</title>
	<link rel="icon" type="image/svg" href="Images/calendar.svg" />
	<meta name="viewport" content="width=device-width, initial-scale=1" />
	<%--BootstrapCDN CSS--%>
	<link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
	<%--local CSS--%>
	<link href="Style/Common.css" rel="stylesheet" />
	<link href="Style/Modal.css" rel="stylesheet" />

	<script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
	<script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
	<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" class="p-1" runat="server">         
        <!-- Navigation -->
        <div class="d-flex">
            <nav id="mainNav" class="navbar navigation w-100">
                <%--Navigation Logo--%>
                <span class="border border-primary rounded bg-white p-3 ml-3">Connect2Cal</span>
                <%--Navigation: Login/Create Account Dropdown--%>
                <div class="btn-toolbar ml-2">
                    <div class="btn-group">
                        <div>
                            <asp:LinkButton runat="server" CssClass="btn btn-default dropdown-toggle" data-toggle="dropdown">
                                Menu<span class="caret"></span></asp:LinkButton>
                            <ul class="dropdown-menu">
                                <li><asp:Button ID="btnLogout" CssClass="btn btn-default" runat="server" 
                                    Text="Logout" OnClick="btnLogOut_Click"></asp:Button></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <%--Navigation Help--%>
                <mc:ModalControl id="hmUserClientHelpModal" runat="server"></mc:ModalControl>
                <%--Navigation Avatar--%>
                <div class="align-content-center ml-auto">
                    <div id="svgAvatar" runat="server">
                        <svg viewBox="0 0 534 534" width="50" height="50">
                            <use href="..."/>
                        </svg>
                    </div>
                    <div>
				        <asp:Label ID="lblUserName" runat="server"></asp:Label>
                    </div>
                </div>
            </nav>
        </div>
        <%--End Navigation--%>
        <div class="mainContent">
            <div class="container-fluid" runat="server">
                <div class="row">
                    <%--sidebar--%>
                    <div class="col-sm-2 sidebarContent">
                        <div class="sidebar-heading text-center">
                            <asp:Button ID="btnAddCalEvent" runat="server" Text="Event Options" CssClass="btn btn-primary" OnClick="btnAddCalEvent_Click" />
                        </div>
                        <div id="sideBarCalList" class="list-group list-group-flush text-center" runat="server">
                            <asp:Placeholder ID="plcSideBarCalList" runat="server"></asp:Placeholder>
                            <%--The code-behind dynamically generates this content--%>
                        </div>
                        <div id="datetimepicker" runat="server">
                            <asp:Calendar id="calBegDate" runat="server" 
                                OnDayRender="calBegDate_OnDayRender"
                                OnSelectionChanged="calBegDate_SelectionChanged"></asp:Calendar> 
                        </div>
                    </div>
                    <%--main calendar content--%>
                    <div class="col-sm-10">
                        <!--side bar-->
                        <nav class="navbar navbar-expand-lg">
                            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarCollapse">
                                <span class="navbar-toggler-icon"></span>
                            </button>
                            <div class="navbar">
                                <ul class="navbar-nav">
                                    <li class="nav-item">
                                        <div class="form-inline">
                                            <div id="calendarButtons" runat="server">
                                                <asp:Button ID="btnAddCalendar" runat="server" CssClass="btn btn-secondary m-1 p-1" Text="Add Calendar" OnClick="btnAddCalendar_Click" />
                                                <asp:Button ID="btnUpdCalendar" runat="server" CssClass="btn btn-secondary m-1 p-1" Text="Update Calendar" OnClick="btnUpdCalendar_Click" />
                                                <asp:Button ID="btnDelCalendar" runat="server" CssClass="btn btn-secondary m-1 p-1" Text="Delete Calendar" OnClick="btnDelCalendar_Click" />
                                                <asp:Button ID="btnCanCalendar" runat="server" CssClass="btn btn-secondary m-1 p-1" Text="Exit Options" OnClick="DisplayEventContent" />
                                            </div>
                                            <div id="eventButtons" runat="server">
                                                <asp:Button ID="btnAddEvent" runat="server" CssClass="btn btn-secondary m-1 p-1" Text="Add Event" OnClick="btnAddEvent_Click" />
                                                <asp:Button ID="btnUpdEvent" runat="server" CssClass="btn btn-secondary m-1 p-1" Text="Update Event" OnClick="btnUpdEvent_Click" />
                                                <asp:Button ID="btnDelEvent" runat="server" CssClass="btn btn-secondary m-1 p-1" Text="Delete Event" OnClick="btnDelEvent_Click" />
                                                <asp:Button ID="btnCanEvent" runat="server" CssClass="btn btn-secondary m-1 p-1" Text="Exit Options" OnClick="DisplayEventContent" />
                                            </div>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                        </nav>
                        <%--Calendar Options--%> 
                        <div id="calOptions" class="card w-80 text-black" runat="server">
                            <div class="form-group">
                                <div class="container">
                                    <div>
                                        <h3 class="text-center">Calendar Options</h3>
                                    </div>
                                    <div class="row mt-3 mb-3">
                                        <div class="col-sm-3 text-right">
                                            <span class="mr-1">Calendar Name:</span>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="ddlCalOptCalendarName" runat="server" 
                                                OnSelectedIndexChanged="ddlCalOptCalendarName_SelectedIndexChanged"
                                                AutoPostBack="true"></asp:DropDownList>
                                        </div>                                    
                                    </div>
                                    <div class="row mb-3">
                                        <div class="col-sm-3 text-right">
                                            <span class="mr-1">New Calendar Name:</span>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtCalNewName" runat="server" PlaceHolder="If adding new calendar..."></asp:TextBox>
                                        </div>
                                    </div>
                                    <%--TODO: Future feature (require documentation to schedule an event)--%>
                                    <%--<div class="row mb-3">
                                        <div class="col-sm-3 text-right">
                                            <span class="mr-1">Required (Yes/No):</span>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtRequiredDoc" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="col-sm-3 text-right">
                                            <span class="mr-1">Required DocType:</span>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtRequiredDocType" runat="server"></asp:TextBox>
                                        </div>
                                    </div>--%>
                                    <div class="row mb-3">
                                        <div class="col-sm-3 text-right">
                                            <span class="mr-1">Location:</span>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtLocation" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="col-sm-3 text-right">
                                            <span class="mr-1">Description:</span>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtDesc" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="col-sm-3 text-right">
                                            <span class="mr-1">IsChecked (y/n):</span>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtIsChecked" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="col-sm-3 text-right">
                                            <span class="mr-1">Color:</span>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="ddlColor" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--event content--%>
                        <div id="eventOptions" class="card w-80 text-black" runat="server">
                            <div class="form-group w-100">
                                <div class="container">
                                    <div>
                                        <h3 class="text-center">Event Options</h3>
                                    </div>
                                    <div>
                                        <p class="text-center">Calendar name, event begin and end date/time are required to update or delete an event.</p>
                                    </div>
                                    <div class="row mb-3" visible="false">
                                        <div class="col-sm-7">
                                            <asp:Label ID="EventId" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="col-sm-3 text-right">
                                            <span class="mr-1">CalendarName:</span>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="ddlEvtOptCalendarName" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="col-sm-3 text-right">
                                            <span class="mr-1">Title:</span>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtCalEvtTitle" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="col-sm-3 text-right">
                                            <span class="mr-1">Begin (m/d/ccyy hh:mm am/pm):</span>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtCalEvtBegDateTime" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="col-sm-3 text-right">
                                            <span class="mr-1">End (m/d/ccyy hh:mm am/pm):</span>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtCalEvtEndDateTime" runat="server"></asp:TextBox>
                                        </div>
                                    </div>	
                                    <%--TODO: Future feature (attach documents to your Account and/or Calendar Event--%>
                                    <%--<div class="row mb-3">
                                        <div class="col-sm-3 text-right">
                                            <span class="mr-1">AttachmentDocumentUrl:</span>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtCalEvtAttachmentDocumentUrl" runat="server"></asp:TextBox>
                                        </div>
                                    </div>--%>
                                    <div class="row mb-3">
                                        <div class="col-sm-3 text-right">
                                            <span class="mr-1">Location:</span>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtCalEvtLocation" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="col-sm-3 text-right">
                                            <span class="mr-1">Description:</span>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtCalEvtDescription" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--calendar view--%>
                        <div class="row d-flex" id="calView" runat="server" >
                            <div class="container-fluid" runat="server">
                                <asp:GridView ID="gvCalView" runat="server" 
                                    AutoGenerateSelectButton="true"
                                    AutoGenerateColumns="false"
                                    onrowdatabound="gvCalView_RowDataBound"                                    
                                    onselectedindexchanged="gvCalView_SelectedIndexChanged"
                                    CssClass="table table-compressed table-bordered table-striped">	
                                <Columns>
                                    <asp:BoundField HeaderText="Title" DataField="Title" /> 
                                    <asp:BoundField HeaderText="BegDateTime" DataField="BegDateTime" DataFormatString="{0:g}" />
                                    <asp:BoundField HeaderText="EndDateTime" DataField="EndDateTime" DataFormatString="{0:g}" /> 
                                    <%--TODO: Future feature (attach documents to your Account and/or Calendar Event--%>
                                    <%--<asp:BoundField HeaderText="AttachmentDocumentUrl" DataField="AttachmentDocumentUrl" />--%> 
                                    <asp:BoundField HeaderText="Location" DataField="Location" /> 
                                    <asp:BoundField HeaderText="Description" DataField="Description" /> 
                                    <asp:BoundField HeaderText="DateTimeStamp" DataField="DateTimeStamp" DataFormatString="{0:g}" /> 
                                </Columns>                               
                                <EmptyDataTemplate>
                                    <div class="text-center">No events selected.</div>
                                </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
