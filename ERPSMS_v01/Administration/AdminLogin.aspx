<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminLogin.aspx.cs" Inherits="LatexERPV2.AdminLogin"  Theme="ERP-Blue" %>



<%@ Register src="AdminSignIn.ascx" tagname="AdminSignIn" tagprefix="uc1" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script type="text/javascript" language="JavaScript">
      javascript:window.history.forward(-1);
     </script>


</head>
<body>
    <form id="form1" runat="server">
    <div>
    
  
    
        <uc1:AdminSignIn ID="AdminSignIn1" runat="server" />
    
  
    
    </div>
    </form>
</body>
</html>
