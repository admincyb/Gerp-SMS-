<%@ Page Title="LatexERP - Stock Transactions" Language="C#" MasterPageFile="~/ERPSMS.Master" Theme="ERP-Blue"
    AutoEventWireup="true" CodeBehind="StockTransactions.aspx.cs" Inherits="ERPSMS_v01.DashboardSMS.StockTransactions" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <rsweb:ReportViewer ID="rvrReport" runat="server" SizeToReportContent="true" class="chart-border  reportviewer dash-reportviewer">
    </rsweb:ReportViewer>
</asp:Content>
