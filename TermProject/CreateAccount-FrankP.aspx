<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateAccount-FrankP.aspx.cs" Inherits="TermProject.CreateAccount" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create An Account</title>
</head>
<body>
    <h3>Thank You for Creating an Account</h3>
    <h3>Please fill out all the forms. 
    </h3>
    <form id="form1" runat="server">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblUserName" runat="server" Text="" for="txtUserName">User Name: </asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtUserName" runat="server" placeholder="User name"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rftxtUserName" runat="server" ErrorMessage="Field is Required" ControlToValidate="txtUserName" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCreatePassword" runat="server" Text="" for="txtCreatePassword">Create Password: </asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCreatePassword" runat="server" placeholder="Create Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rftxtCreatePassword" runat="server" ErrorMessage="Field is Required" ControlToValidate="txtCreatePassword" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblAddress" runat="server" Text="" for="txtAddress">Address: </asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtAddress" runat="server" placeholder="Address"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPhoneNumber" runat="server" Text="" for="txtPhoneNumber">Phone Number(Include the dashes): </asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPhoneNumber" runat="server" placeholder="Phone Number"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="regexPhoneNumberValid" runat="server" ErrorMessage="You entered an invalid phone number format" ValidationExpression="^(\(?\s*\d{3}\s*[\)\-\.]?\s*)?[2-9]\d{2}\s*[\-\.]\s*\d{4}$" ControlToValidate="txtPhoneNumber" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCreatedEmailAddress" runat="server" Text="" for="txtCreatedEmailAddress">Login Email Address: </asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCreatedEmailAddress" runat="server" placeholder="Login Email Address"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfttxtCreatedEmailAddress" runat="server" ErrorMessage="Field is Required" ControlToValidate="txtCreatedEmailAddress" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regexCreatedEmailAddressValid" runat="server" ErrorMessage="You entered an invalid email format" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtCreatedEmailAddress" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblContactEmailAddress" runat="server" Text="" for="txtContactEmailAddress">Contact Email Address: </asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtContactEmailAddress" runat="server" placeholder="Contact Email Address"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfttxtContactEmailAddress" runat="server" ErrorMessage="Field is Required" ControlToValidate="txtContactEmailAddress" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regexContactEmailAddressValid" runat="server" ErrorMessage="You entered an invalid email format" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtContactEmailAddress" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblQCity" runat="server" Text="" for="txtQCity"> In what town or city ws your first full time job in?</asp:Label>
                </td>

                <td>
                    <asp:TextBox ID="txtQCity" runat="server" placeholder="Security Question 1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfttxtQCity" runat="server" ErrorMessage="Field is Required" ControlToValidate="txtQCity" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblQPhone" runat="server" Text="" for="txtQPhone">What were the last four digints of your childhood telephone number?</asp:Label>
                </td>

                <td>
                    <asp:TextBox ID="txtQPhone" runat="server" placeholder="Security Question 2"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfttxtQPhone" runat="server" ErrorMessage="Field is Required" ControlToValidate="txtQPhone" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblQSchool" runat="server" Text="" for="txtQSchool">What primary school did you attend? </asp:Label>
                </td>


                <td>
                    <asp:TextBox ID="txtQSchool" runat="server" placeholder="Security Question 3"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfttxtQSchool" runat="server" ErrorMessage="Field is Required" ControlToValidate="txtQSchool" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblrblListType" runat="server" Text="" for="rblListType">Choose Account Type: </asp:Label>
                </td>


                <td>
                    <asp:RadioButtonList ID="rblListType" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True">User</asp:ListItem>
                        <asp:ListItem>Admin</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>

            <tr>
                <td>
                    <asp:Button ID="btnCreateAccount" runat="server" Text="Create Button" OnClick="btnCreateAccount_Click" />
                </td>
            </tr>



        </table>
        <div>
            <asp:Label ID="lblErrorStatus" runat="server" Text=""></asp:Label>
        </div>
    </form>
</body>
</html>
