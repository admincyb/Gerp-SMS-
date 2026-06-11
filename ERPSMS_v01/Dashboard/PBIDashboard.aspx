<%@ Page Title="<%$ Resources:Captions,Title_PBIDashboard %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="PBIDashboard.aspx.cs" Inherits="ERPSMS_v01.Dashboard.PBIDashboard" Theme="ClassicExt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>

iframe#ctl00_MainContent_iframePBI {
    height: 650px;
}
.content-wrapper {
    padding: 0px 0px 0px 0px !important;
}

</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="content-wrapper">
           <iframe width="100%" height="100%" id="iframePBI" runat="server"   frameborder="0" allowFullScreen="true"></iframe>

    </div>

</asp:Content>
