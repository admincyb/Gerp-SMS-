<%@ Page Title="<%$ Resources:Captions,Title_GSTR_Excel %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="GSTRReports.aspx.cs" Inherits="ERPSMS_v01.Finance.GSTRReports"
    Theme="ClassicExt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
        }


        //For finding and removing duplicate and other group validation controls
        //Array of present validations
        var validationArrayGroup;
        function CheckValidationDuplicate(valGroup) {
            validationArrayGroup = new Array();
            //Traversing from bottom through all the validation controls in the page
            for (var i = Page_Validators.length - 1; i >= 0; i--) {
                if (typeof (Page_Validators[i].validationGroup) == "string") {
                    if (valGroup == Page_Validators[i].validationGroup) {
                        //checks if the control is already in the validation array
                        if (!CheckValidationExists(Page_Validators[i].id)) {
                            //insert new conrol to the Array of present validations
                            validationArrayGroup.push(Page_Validators[i].id);
                        }
                        //remove if control is already in Array of present validations
                        else {
                            Page_Validators.splice(i, 1);
                        }
                    }
                    //remove control if not in group
                    else {
                        Page_Validators.splice(i, 1);
                    }
                }
            }
        }
        //For checking if validation control in Array of present validations
        function CheckValidationExists(id) {
            for (var i in validationArrayGroup) {
                if (validationArrayGroup[i] == id) {
                    return true;
                }
            }
            return false;
        }
        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlGst">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry">
                                    <li>
                                        <asp:Button runat="server" ID="btnDownload" CommandName="DOWNLOAD" TabIndex="100" Text="<%$resources:Download %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:Download %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Excel" ValidationGroup="gst" OnClientClick="javascript:ValidatePageNow('gst')" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Refresh %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="101" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-refresh" ToolTip="<%$resources:Controls,Refresh %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide tablelayout">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblRptType" runat="server" Text="<%$resources:RptType %>" AssociatedControlID="ddlReportType" CssClass="lbl-62perc"></asp:Label>
                                            <asp:DropDownList ID="ddlReportType" runat="server" CssClass="select-small-a1" TabIndex="1">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfRptType" CssClass="star" SetFocusOnError="true"
                                                InitialValue="-1" ValidationGroup="gst" EnableClientScript="true" runat="server"
                                                ControlToValidate="ddlReportType" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_RptType %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblFromDate" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtFromDate" CssClass="lbl-62perc"></asp:Label>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" />
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="2" CssClass="input-small"
                                                MaxLength="13" onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfFromDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="gst" EnableClientScript="true" runat="server" ControlToValidate="txtFromDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_FromDate %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtToDate"></asp:Label>
                                            <asp:HiddenField ID="hdfToDate" runat="server" />
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="3" CssClass="input-small" MaxLength="13"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfToDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="gst" EnableClientScript="true" runat="server" ControlToValidate="txtToDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ToDate %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsSave" ValidationGroup="gst" runat="server" />
                </div>
            </div>
        </ContentTemplate>
         <Triggers>
            <asp:PostBackTrigger ControlID="btnDownload" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
