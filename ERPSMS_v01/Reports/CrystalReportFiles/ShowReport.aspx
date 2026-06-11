<%@ Page Title="" Theme="Classic" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="ShowReport.aspx.cs" EnableViewState="true" Inherits="ERPSMS_v01.Reports.CrystalReportFiles.ShowReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
var prm = Sys.WebForms.PageRequestManager.getInstance();
//Raised before processing of an asynchronous postback starts and the postback request is sent to the server.
prm.add_beginRequest(BeginRequestHandler);
// Raised after an asynchronous postback is finished and control has been returned to the browser.
prm.add_endRequest(EndRequestHandler);
function BeginRequestHandler(sender, args) {
    //Shows the modal popup - the update progress
    $("#updateProgress").show();
}
function EndRequestHandler(sender, args) {
    $("#updateProgress").hide();
}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%--    <asp:UpdatePanel runat="server" ID="aupdpnlCommonReportViewer" ChildrenAsTriggers="true">
        <ContentTemplate>--%>
            <%--<asp:Literal runat="server" ID="ltrScriptContent" Text="<script type='text/javascript'>function InitComponents(flag){}</script>"></asp:Literal>--%>
            <div class="fixed-buttons-normal" id="divFixedTab">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlListing">
                                    <li>
                                        <asp:Button ID="btnSearch" runat="server" ClientIDMode="Static" OnClick="ActionHandler"
                                            Text="View" SkinID="btnInner-View" CommandName="VIEW" ValidationGroup="report"
                                            CommandArgument="SEC_ActionPanel" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                
                <div id="diverror" style="display: none">
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><div
                        id="divValidationSummary" runat="server">
                        <%--The ValidationSummary controls will bind here--%>
                    </div>
                </div>
            </div>
        <%--</ContentTemplate>--%>
        <%--<Triggers>
            <asp:PostBackTrigger  ControlID="btnSearch" />
        </Triggers>
    </asp:UpdatePanel>--%>
    <div id="divCrystalReportViewer" runat="server">
                    <CR:CrystalReportViewer EnableTheming="false" ID="crReportViewer" runat="server" AutoDataBind="true" />
                </div>
</asp:Content>
