<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminClient.aspx.cs" Inherits="TermProject.AdminClient" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Connect2Cal</title>
	<link rel="icon" type="image/svg" href="Images/favicon.svg" />
	<meta name="viewport" content="width=device-width, initial-scale=1" />
	
	<%--BootstrapCDN CSS--%>
	<link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
	
	<%--local styles--%>
	<link href="Style/Common.css" rel="stylesheet" />
	<link href="Style/Modal.css" rel="stylesheet" />
	<script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
	<script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
	<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script>
        "use strict";

        // Get the modal
        var modal = document.getElementById("myModal");

        // Get the button that opens the modal
        var btn = document.getElementById("btnHelp");

        // Get the span element that closes the modal
        var span = document.getElementsByClassName("close")[0];
    
            // When the user clicks on the button, open the modal
        btn.onclick = function() {
            modal.style.display = "block";
        }
    
        // When the user clicks on span(x), close the modal
        span.onclick = function() {
            modal.style.display = "none";
        }
    
        // When the user clicks anywhere outside of the modal, close it
        window.onclick = function(event) {
        if (event.target == modal) {
            modal.style.display = "none";
            }
        }
    </script>
</head>
<body class="d-flex align-items-center">
    <form id="form1" runat="server" class="center-vert-horiz">
        <%--Start NavBar--%>
        <nav class="navbar fixed-top navigation border border-primary rounded">
            <%--Navigation Logo--%>
			<span class="border border-primary rounded bg-white p-3 m-1">Connect2Cal</span>
            <%--Navigation Help--%>
            <asp:Button ID="btnHelp" Text="Help" runat="server" CssClass="btn btn-default m-1"></asp:Button>
			<%--Navigation: Login/Create Account Dropdown--%>
			<div class="btn-toolbar mr-auto">
				<div class="btn-group">
					<div>
                        <asp:LinkButton runat="server" CssClass="btn btn-default dropdown-toggle m-1" data-toggle="dropdown">
                            Menu<span class="caret"></span></asp:LinkButton>
                        <ul class="dropdown-menu">
                            <li><asp:Button ID="btnLogOut" class="btn btn-default m-1" runat="server" 
                                Text="LogOut" onclick="btnLogOut_Click"></asp:Button></li>
						</ul>
					</div>
				</div>
			</div>
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
                            <use href="Images/Avatars/man-avatar-with-bald-head-sunglasses-and-mustache.svg#Capa_1"/>
                        </svg>
                    </div>
                    <div>
				        <asp:Label ID="lblUserName" runat="server" Width="50" Height="50"></asp:Label>
                    </div>
			    </div>
            </div>
        </nav>
        <%--End NavBar--%>
        <%--Start Login--%> 
        <div id="loginCard w-75" class="text-black">
            <div class="card">
                <div class="card-body w-100">
                    <h3>You are signed in as AdminClient.</h3>
                </div>
            </div>
        </div>
        <%--End Login--%> 
    </form>
</body>
</html>