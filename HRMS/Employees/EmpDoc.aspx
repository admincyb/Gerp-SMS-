<%@ Page Title="<%$ Resources:Captions,Title_HRMS_EmpDoc %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="EmpDoc.aspx.cs" Inherits="HRMS.Employees.EmpDoc"
    Theme="ClassicExt" %>

<%@ Register Src="~/Employees/UserControls/EmpDocument.ascx" TagName="EmpDocument"
    TagPrefix="ucEmpDoc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="divEmpDocumentContainer">
                <ucEmpDoc:EmpDocument ID="ucEmpDocument" runat="server" ParentPage="EmpDoc" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
