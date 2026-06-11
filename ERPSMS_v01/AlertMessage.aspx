<%@ Page Title="<%$ Resources:Captions,Title_Home %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="AlertMessage.aspx.cs" Theme="Classic" Inherits="ERPSMS_v01.AlertMessage" %>

<%@ Register Src="~/UserControls/AlertControl.ascx" TagName="Alert" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function AfterClose(containerID) {
            if (containerID == "[id$=divAlert]") {
                window.location = $("[id$=hdfRedirectURl]").val();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlSOInvoice">
        <ContentTemplate>
            <div class="content-wrapper">
                <h2>
                    <%= GetGlobalResourceObject("Captions", "Welcome").ToString() %>
                </h2>
                <div id="divAlert" style="display: none">
                    <uc2:Alert ID="ucrAlert" runat="server" />
                </div>
                <asp:HiddenField ID="hdfRedirectURl" runat="server" Value="" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
