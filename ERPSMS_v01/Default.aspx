<%@ Page Title="<%$ Resources:Captions,Title_Home %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="Default.aspx.cs" Inherits="ERPSMS_v01.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script type="text/javascript">
     $(document).ready(function () {
         ShowMenu();
     });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

  <div class="content-wrapper">
    <h2>
        <%= GetGlobalResourceObject("Captions", "Welcome").ToString() %>
    </h2>
    </div>
</asp:Content>
