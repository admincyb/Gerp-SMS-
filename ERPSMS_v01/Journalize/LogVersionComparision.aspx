<%@ Page Title="Difference between edit log versions" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="LogVersionComparision.aspx.cs"
    Theme="ClassicExt" Inherits="ERPSMS_v01.Journalize.LogVersionComparision" EnableEventValidation="false" %>
<%@ Register Src="~/Journalize/UserControls/AuditLogVersionComparision.ascx" TagName="VersionComparision" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .no-border {
            border: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <h3 style="text-align: center; padding-top: 3%;">
            <asp:Literal runat="server" Text="<%$ Resources:DiffBwEditLog %>"></asp:Literal></h3>
    </div>
        <uc1:VersionComparision ID="ucrFirstVersion" runat="server" />
    <div class="row" runat="server" id="DivPreviousHead">
        <h3 style="text-align: left; padding-top: 1%;">
            <asp:Literal runat="server" Text="Previous Version"></asp:Literal></h3>
    </div>
        <uc1:VersionComparision ID="ucrSecondVersion" runat="server" />
</asp:Content>
