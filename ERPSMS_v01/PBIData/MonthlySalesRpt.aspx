<%@ Page Language="C#" AutoEventWireup="true" Theme="ClassicExt" CodeBehind="MonthlySalesRpt.aspx.cs"
    Inherits="ERPSMS_v01.PBIData.MonthlySalesRpt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>
            <%= GetLocalResourceObject("MonthlySalesReport").ToString()%></h1>
        <div class="gridwrap">
            <asp:GridView runat="server" ID="grdMonthlySalesList" AutoGenerateColumns="true"
                Width="100%" EmptyDataRowStyle-CssClass="emptytable">
                <EmptyDataTemplate>
                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                </EmptyDataTemplate>
            </asp:GridView>
            <%-- <uc1:PagerControl ID="uclPaging" runat="server" />--%>
        </div>
    </div>
    </form>
</body>
</html>
