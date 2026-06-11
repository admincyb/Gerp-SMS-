<%@ Page  Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="VendorContactDtls.aspx.cs" Inherits="ERPSMS_v01.DashboardSMS.VendorContactDtls"
    Theme="Classic" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--<rsweb:ReportViewer ID="rvrReport" runat="server" SizeToReportContent="true" class="chart-border  reportviewer dash-reportviewer">
    </rsweb:ReportViewer>--%>
    <div class="reportviewer chart-border dash-reportviewer">
        <rsweb:ReportViewer ID="rvrReport" runat="server" BorderWidth="0" SizeToReportContent="true"
            HyperlinkTarget="" Width="98%">
        </rsweb:ReportViewer>
        </div>
</asp:Content>
