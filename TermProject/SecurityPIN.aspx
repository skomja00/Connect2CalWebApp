<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SecurityPIN.aspx.cs" Inherits="TermProject.SecurityPIN" %>

<%@ Register src="ModalUserControl.ascx" TagName="ModalControl" TagPrefix="mc"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Connect2Cal</title>
	<link rel="icon" type="image/svg" href="Images/calendar.svg" />
	<meta name="viewport" content="width=device-width, initial-scale=1" />
	
    <%--Bootstrap Icons CDN--%>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.4.1/font/bootstrap-icons.css">

	<%--Bootstrap CSS CDN--%>
	<link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
	
	<%--local <style> & CSS--%>
	<link href="Style/Modal.css" rel="stylesheet" />
	<link href="Style/Common.css" rel="stylesheet" />

	<script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
	<script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
	<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
</head>
<body class="d-flex align-items-center justify-content-center">
    <form id="form1" runat="server">
        <!--Start NavBar-->
        <nav class="navbar fixed-top navigation border border-primary rounded" id="mainNav">
            <%--Navigation Logo--%>
            <span class="border border-primary rounded bg-white p-3 ml-3">Connect2Cal</span>
            <div class="btn-toolbar ml-2">
                <div class="btn-group">
                    <div>
                        <asp:LinkButton runat="server" CssClass="btn btn-default dropdown-toggle m-1" data-toggle="dropdown">
                            Menu<span class="caret"></span></asp:LinkButton>
                        <ul class="dropdown-menu">
                            <li><asp:Button ID="btnNavLogIn" class="btn btn-default" runat="server" 
                                Text="Login" OnClick="btnLogIn_Click"></asp:Button></li>
                        </ul>
                    </div>
                </div>
            </div>
            <mc:ModalControl id="mcActivateAcct" runat="server"></mc:ModalControl>
            <div class="form-inline mt-2 mt-md-0">
                <div class="text-center p-1">
                    <%--viewBox is an attribute of the <svg> element. 
                            x coordinate in the scaled viewBox coordinate system to use for the top left corner of the SVG viewport
                            y coordinate in the scaled viewBox coordinate system to use for the top left corner of the SVG viewport 
                            width in coordinates/px units within the SVG code/doc scaled to fill the available width
                            height in coordinates/px units within the SVG code/doc scaled to fill the available height
                            For more info see: https://css-tricks.com/scale-svg/--%>        
                    <div id="svgAvatar" runat="server">
                        <svg viewBox="0 0 534 534" width="50" height="50">
                            <use href="Images/..."/>
                        </svg>
                    </div>
                    <div>
				        <asp:Label ID="lblUserName" runat="server" Width="50" Height="50"></asp:Label>
                    </div>
			    </div>
            </div>
        </nav>
        <%--End NavBar--%>
        <%--Start Account Security PIN Card--%>
        <div class="w-100">
            <div id="securityPINCard" class="card" runat="server">
                <div class="card-body">
                    <div class="container">
                        <div class="row justify-content-center mb-3">
                            <div class="col-md-12">
                                <asp:Label ID="lblSecurityPINMessage" CssClass="h4" runat="server" Text="Please enter the security PIN."></asp:Label>
                            </div>
                        </div>
                        <%--CreatedEmailAddress text box--%>
                        <div class="row mb-2 text-center">
                            <div class="col-md-4 text-right">
                                <asp:Label id="lblCreatedEmailAddress" runat="server" Text="Created Email Address:" for="txtCreatedEmailAddress"></asp:Label>
                            </div>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtCreatedEmailAddress" runat="server" CssClass="text-left" placeholder=""></asp:TextBox>
                                <div class="invalid-feedback">Please provide your contact email address.</div>
                            </div>
                        </div>
                        <%--SecurityPIN text box--%>
                        <div class="row mb-2 text-center">
                            <div class="col-md-4 text-right">
                                  <asp:Label id="lblSecurityPIN" runat="server" Text="SecurityPIN:"></asp:Label>
                            </div>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtSecurityPIN" runat="server" CssClass="text-left" placeholder=""></asp:TextBox>
                                <div class="invalid-feedback">Please provide security PIN from your email.</div>
                            </div>
                        </div>
                        <div class="row mb-1 text-center">
                            <%--SecurityPIN Button--%>
                            <div class="col-md-12">
                                <asp:Button ID="activateAccount" runat="server" Text="Activate Account" CssClass="btn btn-primary" OnClick="btnActivateAccount_Click" />
                            </div> 
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--End Account Security PIN Card--%>
    </form>
</body>
</html>
