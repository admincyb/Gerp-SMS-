<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    Theme="Classic" CodeBehind="CumilativeStockReport.aspx.cs" Inherits="Production.Reports.Production.CumilativeSttockReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/Reports/CumilativeStockReport.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%-- <div id="webwizard-wrap">
        <div class="button-wrap">
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();" />
        </div>
    </div>--%>
    <div class="fixed-buttons-normal">
        <div class="Button-container">
            <asp:Table runat="server" ID="tblButton">
                <asp:TableRow>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul class="bredcrum">
                            <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                        </ul>
                        <ul runat="server" id="pnlListing">
                            <li>
                                <asp:Button ID="btnCancel" runat="server" OnClientClick="javascript:return CancelFun();"
                                    SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Close%>" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
        </div>
    </div>
    <div id="searchwrap" class="search-wrap-custom1">
        <div id="divSearch">
            <label for="Item">
                <asp:Literal ID="Literal1" runat="server" Text="<%$ resources:Item%>" /></label>
            <asp:DropDownList ID="ddlItem" runat="server" CssClass="srchboxtextbx" TabIndex="2"
                Width="300px">
            </asp:DropDownList>
            <asp:Label ID="lblSearchText" AssociatedControlID="txtFromDate" runat="server" Text="<%$ resources:FromDate%>"></asp:Label>
            <asp:TextBox ID="txtFromDate" runat="server" ClientIDMode="Static" TabIndex="14"
                Width="20%"></asp:TextBox>
            <asp:Label ID="lblToDate" AssociatedControlID="txtToDate" runat="server" Text="<%$ resources:ToDate%>"></asp:Label>
            <asp:TextBox ID="txtToDate" runat="server" ClientIDMode="Static" TabIndex="14" Width="20%"></asp:TextBox>
            <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" OnClick="imbSearch_Click"
                CommandName="SEARCH" TabIndex="4" />
        </div>
        <div class="clear">
        </div>
    </div>
    <%-- <div class="reportviewer">
        <rsweb:ReportViewer ID="rvStockReport" runat="server" Width="99%" BorderWidth="0"
            BorderStyle="None" SizeToReportContent="true">
        </rsweb:ReportViewer>
    </div>--%>
    <div class="content-wrapper">
        <div class="reportviewer">
            <rsweb:ReportViewer ID="rvStockReport" runat="server" BorderWidth="0" SizeToReportContent="true"
                Width="98%">
            </rsweb:ReportViewer>
        </div>
    </div>
    <div class="no-data">
        <asp:Literal ID="ltNodata" runat="server" Text="<%$  resources:Msg_Sorry_No_Data_Available %>"
            Visible="false" /></div>
    <div class="clear">
    </div>
</asp:Content>
