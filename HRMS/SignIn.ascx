<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SignIn.ascx.cs" Inherits="HRMS.SignIn" %>
<div id="loginwrapper" runat="server" class="loginwrap">
    <asp:Login ID="LgnUser" Visible="false" runat="server" BackColor="Transparent" BorderPadding="4"
        BorderStyle="None" BorderWidth="0px" Font-Names="Verdana" LoginButtonText="LogIn"
        OnAuthenticate="LgnUser_Authenticate" TitleText="">
        <TitleTextStyle BackColor="Transparent" Font-Bold="True" />
        <CheckBoxStyle />
        <InstructionTextStyle Font-Italic="True" />
        <LoginButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px"
            Font-Names="Verdana" />
        <LayoutTemplate>
            <%-- <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" EnableTheming="false"
                Text="User Name *"></asp:Label>--%>
            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                CssClass="login-validator" ForeColor="#2a98d8" ErrorMessage=" Enter User Name"
                ValidationGroup="LgnUser"></asp:RequiredFieldValidator>
            <asp:TextBox ID="UserName" runat="server" EnableTheming="false" placeholder="Username"></asp:TextBox>
            <%--  <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" EnableTheming="false"
                Text="Password *"></asp:Label>--%>
            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                CssClass="login-validator" ForeColor="#2a98d8" ErrorMessage=" Enter Password"
                ToolTip="Password" ValidationGroup="LgnUser"></asp:RequiredFieldValidator>
            <asp:TextBox ID="Password" runat="server" EnableTheming="false" TextMode="Password" placeholder="Password"></asp:TextBox>
            <asp:Button ID="LoginButton" runat="server" CommandName="Login" CssClass="inputbtn"
                Text="Login" ValidationGroup="LgnUser" />
            <div style="margin-top: 10px; color: #c6ff00;">
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
            </div>
        </LayoutTemplate>
        <LabelStyle Font-Bold="True" />
        <FailureTextStyle />
    </asp:Login>
    <div id="NewLogin">
        <div id="errorContainer">
            <asp:ValidationSummary runat="server" ID="ValidationSummary" DisplayMode="BulletList"
                EnableClientScript="true" ValidationGroup="AuthValidation" ClientIDMode="Static" />
            <asp:Label ID="ErrorText" ClientIDMode="Static" runat="server" Font-Size="Small"></asp:Label>
        </div>
        <div id="login-container">
            <asp:TextBox runat="server" ID="gERPUserID" ClientIDMode="Static" MaxLength="30" placeholder="Username"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqUserName" ControlToValidate="gERPUserID" ErrorMessage="UserID cannot be empty!"
                Display="None" EnableClientScript="true" runat="server" ValidationGroup="AuthValidation"></asp:RequiredFieldValidator>
            <%--<asp:RegularExpressionValidator ID="regUserName" runat="server" EnableClientScript="true"
				ControlToValidate="UserID" Display="None" ErrorMessage="Invalid UserID format!"
				ValidationExpression="\w{1,30}\.?\w{1,30}" ValidationGroup="AuthValidation"></asp:RegularExpressionValidator>--%>
            <asp:RegularExpressionValidator ID="regUserName" runat="server" EnableClientScript="true"
                ControlToValidate="gERPUserID" Display="None" ErrorMessage="Invalid UserID format!"
                ValidationExpression="[\w\.\*\@\-]{1,35}" ValidationGroup="AuthValidation"></asp:RegularExpressionValidator>
            <asp:TextBox runat="server" TextMode="Password" ID="gERPPwd" MaxLength="30" ClientIDMode="Static" placeholder="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqPwd" Display="None" ControlToValidate="gERPPwd"
                ErrorMessage="Password cannot be empty!" EnableClientScript="true" runat="server"
                ValidationGroup="AuthValidation"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator Text="" ID="regPwd" runat="server" EnableClientScript="true"
                ControlToValidate="gERPPwd" ErrorMessage="Invalid Password format!" ValidationExpression="[\w\.\*\@\-]{1,30}"
                ValidationGroup="AuthValidation" Display="None"></asp:RegularExpressionValidator>
            <asp:DropDownList runat="server" ID="ddlSBU" CssClass="select-def">
             </asp:DropDownList>
            <asp:RequiredFieldValidator ID="reqSBU" Display="None" ControlToValidate="ddlSBU"
                ErrorMessage="Select SBU" EnableClientScript="true" runat="server" InitialValue="-1"
                ValidationGroup="AuthValidation"></asp:RequiredFieldValidator>
            <asp:Button runat="server" ID="btnLogin" Text="Login" OnClick="btnLogin_Click" ValidationGroup="AuthValidation" />
            <div class="clear">
            </div>
            <a href="AccountManagement/ResetPassword.aspx">Forgot password</a>  <a style="text-decoration: none">  </a>
               <%--<a style="text-decoration: none"> | </a>
        <a href="#">Help</a>--%>
            <div class="error-input">
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        ApplyDropdownClass();
    });
    $("[id$=ddlSBU]").change(function () {
        ApplyDropdownClass();
    });
    function ApplyDropdownClass() {
        if ($("[id$=ddlSBU]").find(":selected").val() == "-1") {
            $("[id$=ddlSBU]").removeClass('select-def-1');
            $("[id$=ddlSBU]").addClass('select-def');
        }
        else {
            $("[id$=ddlSBU]").removeClass('select-def');
            $("[id$=ddlSBU]").addClass('select-def-1');
        }
    }
</script>
