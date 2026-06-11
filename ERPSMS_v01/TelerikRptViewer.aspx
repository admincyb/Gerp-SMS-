<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="TelerikRptViewer.aspx.cs" Inherits="ERPSMS_v01.TelerikRptViewer" Theme="ClassicExt" %>


<%@ Register Assembly="Telerik.ReportViewer.Html5.WebForms, Version=15.1.21.616, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.Html5.WebForms" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="TelDll/Scripts/jquery-3.3.1.min.js"></script>
    <script>
        $.noConflict();
// Code that uses other library's $ can follow here.
    </script>
    <style>
        #reportViewer1 {
            position: absolute;
            left: 5px;
            right: 5px;
            top: 5px;
            bottom: 5px;
            overflow: hidden;
            font-family: Verdana, Arial;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlAvtivity" runat="server">
        <ContentTemplate>
            <h1>Telerik Report</h1>
            <telerik:ReportViewer
                ID="ReportViewer1"
                Width="99%"
                Height="900px"
                EnableAccessibility="false"
                PageMode="SinglePage"
                runat="server"   >
                <ReportSource IdentifierType="UriReportSource"   >
                </ReportSource>
            </telerik:ReportViewer>
        </ContentTemplate>
    </asp:UpdatePanel>
<%--    Identifier="CompoundDetails.trdp"--%>
</asp:Content>
