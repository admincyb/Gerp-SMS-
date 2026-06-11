<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    Theme="Classic" CodeBehind="PRReport.aspx.cs" Inherits="ERPSMS_v01.Reports.PRReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/Reports/PurchaseRequestReport.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%-- <div id="webwizard-wrap">
                <h1>
                    Purchase Request Report
                </h1>
                <div class="button-wrap">
                    <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();"
                        TabIndex="6" />
                    <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" PostBackUrl="~/Reports/PRReport.aspx"
                        TabIndex="7" />
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
                                        <%--<asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" PostBackUrl="PurchaseOrderListing.aspx" />--%>
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
                <label for="DateFrom">
                    Date From</label>
                <asp:TextBox ID="DateFrom" runat="server" EnableViewState="false" TabIndex="3" MaxLength="12">
                </asp:TextBox>
                <asp:HiddenField ID="hdfFromDate" runat="server" />
                <label for="DateTo">
                    Date To</label>
                <asp:TextBox ID="DateTo" runat="server" EnableViewState="false" TabIndex="4" MaxLength="12">
                </asp:TextBox>
                <asp:HiddenField ID="hdfToDate" runat="server" />
                <label for="ddlFilterBy">
                    Filter By</label>
                <asp:DropDownList ID="ddlFilterBy" runat="server" TabIndex="1" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlFilterBy_SelectedIndexChanged">
                    <asp:ListItem Text="All" Value="0"></asp:ListItem>
                    <asp:ListItem Text="PR#" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Status" Value="2"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlFilterValue" runat="server" TabIndex="2">
                </asp:DropDownList>
                <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" TabIndex="5" EnableViewState="false"
                    OnClick="imbSearch_Click" />
                <div class="clear">
                </div>
            </div>
            <%--
            <div class="reportviewer" style="width: 98%; overflow-y: scroll;">
                <rsweb:ReportViewer ID="RptViewerReport" runat="server" Visible="false" EnableTheming="false"
                    OnDrillthrough="RptViewerReport_Drillthrough" Width="100%" BorderWidth="0" BorderStyle="None">
                </rsweb:ReportViewer>
            </div>--%>
            <div class="content-wrapper">
                <div class="reportviewer">
                    <%-- <rsweb:ReportViewer ID="rvStockReport" runat="server" BorderWidth="0" SizeToReportContent="true"
                        Width="98%">
                    </rsweb:ReportViewer>--%>
                    <rsweb:ReportViewer ID="RptViewerReport" runat="server" Visible="false" OnDrillthrough="RptViewerReport_Drillthrough"
                        Width="98%" BorderWidth="0" SizeToReportContent="true">
                    </rsweb:ReportViewer>
                </div>
            </div>
            <div class="clear">
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
