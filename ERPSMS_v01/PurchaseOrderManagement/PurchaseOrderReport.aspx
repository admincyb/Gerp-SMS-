<%@ Page Title="<%$ Resources:Captions,Title_PurchaseOrderReport %>" Language="C#"
    Theme="Classic" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="PurchaseOrderReport.aspx.cs"
    Inherits="ERPSMS_v01.PurchaseOrderManagement.PurchaseOrderReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                                <%--<asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" PostBackUrl="PurchaseOrderListing.aspx" />--%>
                                <asp:Button ID="btnCancel" runat="server" PostBackUrl="PurchaseOrderListing.aspx"
                                    SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Close%>" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
        </div>
    </div>
    <%-- <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.PurchaseOrderReport%>
        </h1>
        <div class="button-wrap">
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" PostBackUrl="PurchaseOrderListing.aspx" />
        </div>
    </div>--%>
    <div class="content-wrapper">
        <div class="reportviewer">
            <rsweb:ReportViewer ID="RptViewerReport" runat="server" BorderWidth="0" SizeToReportContent="true" 
            width="98%">
        </rsweb:ReportViewer>
        </div>
    </div>
</asp:Content>
