<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenerateReport.aspx.cs"
    Inherits="HRMS.Reports.GenerateReport" MasterPageFile="~/ERPSMS_2.Master" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" 
     Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script src="../Scripts/GrantPrintUtility.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
                                <asp:Button ID="btnCancel" runat="server" OnClick="ActionHandler" CommandName="CANCEL"
                                    SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Close%>" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="reportviewer" id="divReportViewer" runat="server">
        <rsweb:ReportViewer ID="rvViewReport" runat="server" BorderWidth="0" SizeToReportContent="true"
            Width="98%" HyperlinkTarget="_blank">
        </rsweb:ReportViewer>
    </div>
    <div id="divCrystalReportViewer" runat="server" visible="false">
        <CR:CrystalReportViewer ID="GERP_OutputReport" HasToggleParameterPanelButton="false"
            runat="server" AutoDataBind="true" />
    </div>
    <div id="divNodata" class="nodata" runat="server" visible="false">
        No Record Found
    </div>
    <div class="visiblefalse">
        <asp:HiddenField ID="hdfAppType" runat="server" />
        <asp:HiddenField ID="hdfRefUrl" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdfShowCrReportDiv" runat="server" Value="0" />
    </div>
</asp:Content>
