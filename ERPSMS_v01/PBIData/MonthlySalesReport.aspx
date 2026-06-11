<%@ Page Title="<%$ Resources:Captions,Title_MonthlySales %>" Language="C#" MasterPageFile="~/ERPSMS_Dashboard.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="MonthlySalesReport.aspx.cs"
    Inherits="ERPSMS_v01.PBIData.MonthlySalesReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlAvtivity" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <asp:HiddenField runat="server" ID="hdfDefaultSubmit" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <h1>
                <%= GetLocalResourceObject("MonthlySalesReport").ToString()%></h1>
            <div class="gridwrap">
                <asp:GridView runat="server" ID="grdMonthlySalesList" AutoGenerateColumns="true" Width="100%" EmptyDataRowStyle-CssClass="emptytable">
                    <EmptyDataTemplate>
                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                    </EmptyDataTemplate>
                 
                </asp:GridView>
                <%-- <uc1:PagerControl ID="uclPaging" runat="server" />--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
