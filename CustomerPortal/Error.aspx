<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="ERPSMS_v01.Error" Theme="Classic" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="shortcut icon" type="image/x-icon" href="images/favicon.ico" />
    <title>gERP</title>
    <asp:Literal ID="scriptSrc" Text="" runat="server"></asp:Literal>

</head>

<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scrMenu" runat="server">
    </asp:ScriptManager>
    <div class="custom-errorpage">
        <img alt="gERP" src="../Images/Classic/layout/gerp-logo.png" />
        <div class="error-page">
            <p>
                <span class="error-large"></span>
                <asp:Literal ID="lblErrorMessage" runat="server" Text="<%$resources:Constants,Errormessage %>"></asp:Literal></p>
       <div class="clear">
        </div>
        </div>
        <div class="clear">
        </div>
    </div>
    </form>
</body>
</html>
