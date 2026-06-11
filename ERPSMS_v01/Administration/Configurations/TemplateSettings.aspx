<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/Configurations/AdminConfigMaster.Master" Theme="ERP-Admin"
AutoEventWireup="true" CodeBehind="TemplateSettings.aspx.cs" Inherits="ERPSMS_v01.Administration.Configurations.TemplateSettings" %>
<%@ Register src="../../UserControls/Template.ascx" tagname="Template" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:Template ID="Template1" runat="server" />
</asp:Content>
