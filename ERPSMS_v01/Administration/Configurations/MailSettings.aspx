<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="MailSettings.aspx.cs" Inherits="ERPSMS_v01.Administration.Configurations.MailSettings" Theme="Classic" %>

<%@ Register Src="~/UserControls/Mail.ascx" TagName="Mail" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="fixed-buttons-normal">
        <div class="Button-container">
            <asp:Table ID="tblButton" runat="server">
                <asp:TableRow>
                    <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul class="bredcrum">
                            <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                        </ul>
                    </asp:TableCell></asp:TableRow>
            </asp:Table>
        </div>
    </div>

    <uc1:Mail ID="Mail1" runat="server" />
</asp:Content>
