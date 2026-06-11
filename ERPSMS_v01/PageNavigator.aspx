<%@ Page Title="<%$ Resources:Captions,Title_Home %>" Theme="ClassicExt" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="PageNavigator.aspx.cs"
    Inherits="ERPSMS_v01.PageNavigator" %>

<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%--<%@ Register Src="~/Journalize/UserControls/JournalizeControlNew.ascx" TagName="Journalize"
    TagPrefix="uc1" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-wrapper">
        <h2>
            <%= GetGlobalResourceObject("Captions", "Welcome").ToString() %>
        </h2>
    </div>
   <%-- <div id="divJournalize" style="display: none">
        <uc1:Journalize ID="ucrJournalize" runat="server" />
    </div>--%>
    <div id="divWkfSubmit" style="display: none;">
        <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
        <uc1:WorkflowUserComments ID="ucrWrkf" runat="server">
        </uc1:WorkflowUserComments>
    </div>
    <div id="diverror" style="display: none">
        <%--Use this label to bind the server errors--%>
        <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
        <asp:HiddenField ID="hdfAppType" runat="server" />
        <asp:HiddenField ID="hdfAppSubType" runat="server" />
    </div>
</asp:Content>
