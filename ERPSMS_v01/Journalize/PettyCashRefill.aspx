<%@ Page Language="C#" Theme="ClassicExt" EnableEventValidation="false" MasterPageFile="~/ERPSMS_2.Master"
    Title="<%$ Resources:Captions,Title_PettyCashRefill %>" AutoEventWireup="true"
    CodeBehind="PettyCashRefill.aspx.cs" Inherits="ERPSMS_v01.Journalize.PettyCashRefill" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<%@ Register Assembly="AjaxControlToolkit, Version=3.0.11119.25533, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>--%>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript" language="javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            $("[id$='lblErrPettyCashAccount']").hide();
            $("[id$='lblErrRefillExpenseTill']").hide();
            if ($("input[id$='txtLastRefillDate']").val() == "") {
                GrandScriptUtils.DatePickerCommon("txtRefillExpenseTill");
            }
            else {
                GrandScriptUtils.DatePickerCommon("txtRefillExpenseTill", false, true, false, $.datepicker.parseDate("dd-M-yy", $("input[id$='txtLastRefillDate']").val()));
            }


            var pageURL = window.document.URL;
            var virtualPath = '<%= (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString()) %>';
            var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
            if (url.indexOf("?") != -1)
                GrandScriptUtils.MakeAutoCompleteDDL("txtPettyCashAccount", url + "&AccType=10", "hdfAccount", true, true, "ACCOUNTMST");
            else
                GrandScriptUtils.MakeAutoCompleteDDL("txtPettyCashAccount", url + "?AccType=10", "hdfAccount", true, true, "ACCOUNTMST");
            GrandScriptUtils.DatePickerCommon("txtRefillExpenseTill");

            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();

        }

        function ShowListing(flag) {
            if (flag) {
                $("[id$=PageAction_List]").show();
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlListing]").show();
                $("[id$=pnlEntry]").hide();
            }
            else {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
            }
            return false;
        }

        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtPettyCashAccount") {
                $("[id$=btnAccountSelected]").click();
            }

        }
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtPettyCashAccount") {
                $("[id$=hdfAccount]").val("0");
                $("[id$=txtLastRefillDate]").val("");

                $("[id$=btnAccountSelected]").click();
            }
        }
        function ValidatePage() {
            var isValid = true;
            var msg = "";

            $("[id$='lblErrPettyCashAccount']").hide();
            $("[id$='lblErrRefillExpenseTill']").hide();

            if ($("[id$='hdfAccount']").val() == '' || $("[id$='hdfAccount']").val() == "0") {
                $("[id$='lblErrPettyCashAccount']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Account") %></li></ul>';
            }
            if ($("[id$='txtRefillExpenseTill']").val() == '') {
                $("[id$='lblErrRefillExpenseTill']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_TillDate") %></li></ul>';
            }

            if (!isValid) {
                $("[id$=litErrorMsg]").show();
                $("[id$=litErrorMsg]").html(msg);
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
            }

            return isValid;
        }

        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                //$("[id$=pnlDelete]").hide();
            }
            else if (mode == 2) {
                //$("[id$=pnlDelete]").hide();
            }
        }
        function AfterClose(containerID) {
            if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
            }
        }

    </script>
</asp:Content>
<asp:Content ID="cntMain" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel runat="server" ID="aupdpnlPettyCashRefill">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label ID="lblBreadCrum" runat="server" />
                                </ul>
                                <ul id="pnlEntry" runat="server">
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="3" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="return ValidatePage();" ToolTip="<%$resources:ErpRes,Submit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="3"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="return ValidatePage();"
                                            ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li id="pnlSave" runat="server">
                                        <asp:Button ID="btnSave" runat="server" OnClick="ActionHandler" CommandName="SAVE"
                                            Text="<%$ resources:Controls,Save %>" SkinID="btnInner-Save" CommandArgument="SEC_ActionPanel"
                                            ToolTip="<%$ resources:Controls,Save %>" TabIndex="3" OnClientClick="return ValidatePage();" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" OnClick="ActionHandler" CommandName="CANCEL"
                                            Text="<%$resources:Controls,Cancel %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel"
                                            ToolTip="<%$ resources:Controls,Cancel %>" TabIndex="4" />
                                    </li>
                                </ul>
                            </asp:TableCell></asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="79" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="80" OnClick="ActionHandler" CommandName="DETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server" Style="display: none">
                        <asp:TableCell>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdList" Width="100%" AutoGenerateColumns="false"
                                    PageSize="<%$ resources:PageSize%>" EmptyDataRowStyle-CssClass="emptytable" OnPageIndexChanging="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:GridAccount %>">
                                            <ItemTemplate>
                                                <asp:HiddenField runat="server" ID="hdfCVD_PK" Value='<%# Eval(Resources.DataFieldRes.CVD_PK) %>' />
                                                <asp:HiddenField runat="server" ID="hdfCVDAccount" Value='<%# Eval(Resources.DataFieldRes.CVD_ACCOUNT) %>' />
                                                <asp:HiddenField runat="server" ID="hdfFromDate" Value='<%# Eval(Resources.DataFieldRes.CVD_FROM_DATE) %>' />
                                                <asp:HiddenField runat="server" ID="hdfToDate" Value='<%# Eval(Resources.DataFieldRes.CVD_TO_DATE) %>' />
                                                <asp:HiddenField runat="server" ID="hdfLastModDate" Value='<%# Eval(Resources.DataFieldRes.CVD_MOD_DT) %>' />

                                                <asp:Label ID="lblAccount" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.CVDAccountText).ToString()),50) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.CVDAccountText).ToString())%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="87%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GridReFillDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRefillDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.CVD_TO_DATE, Resources.Constants.DateFormatGrid)  %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.CVD_TO_DATE, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" TabIndex="14" ID="imbView" ToolTip="<%$Resources:Controls,View %>"
                                                    SkinID="btnview" CommandName="VIEW" OnClick="ActionHandler" />
                                                     <%-- <asp:ImageButton runat="server" TabIndex="14" ID="ImageButton1" ToolTip="<%$Resources:Controls,View %>"
                                                    SkinID="btnview" CommandName="EDIT" OnClick="ActionHandler" />--%>

                                                <asp:ImageButton runat="server" TabIndex="14" ID="imbEdit" ToolTip="<%$Resources:Controls,Edit %>"
                                                    SkinID="imbeditgrid" CommandName="EDITITEM" OnClick="ActionHandler" 
                                                    Visible='<%# Eval(Resources.DataFieldRes.CVD_EDIT_FLAG).ToString() == "1" ? true: false %>'
                                                    />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" Visible="true" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblPettyCashAccount" runat="server" Text="<%$ resources:Controls,PettyCahAccount %>"
                                                AssociatedControlID="txtPettyCashAccount" />
                                            <asp:TextBox ID="txtPettyCashAccount" runat="server" MaxLength="100"  CssClass="input-half" TabIndex="1" />
                                            <asp:HiddenField ID="hdfAccount" runat="server" />
                                            <asp:Button ID="btnAccountSelected" runat="server" OnClick="ActionHandler" CommandName="PETTYCASHACCOUNTSELECTED"
                                                EnableTheming="false" Style="display: none" />
                                            <asp:Label ID="lblErrPettyCashAccount" runat="server" Text="*" CssClass="star" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblLastRefillDate" runat="server" Text="<%$ resources:Controls,LastRefillDate %>"
                                                AssociatedControlID="txtLastRefillDate" />
                                            <asp:TextBox ID="txtLastRefillDate" runat="server" MaxLength="100" ReadOnly="true"
                                                CssClass="medium input-disabled " />
                                            <asp:HiddenField ID="hdfLastRefillDt" runat="server" Value="" />
                                          
                                          <asp:Label ID="lblRefillExpenseTill" runat="server" CssClass="middle-lbl-a" Text="<%$ resources:Controls,RefillExpensesTill %>"
                                                AssociatedControlID="txtRefillExpenseTill" /><asp:TextBox runat="server" ID="txtRefillExpenseTill"
                                                    Text="" CssClass="Uidate-picker"  onkeydown="return CheckKey(event)" MaxLength="80"
                                                    onpaste="return false;" TabIndex="2"></asp:TextBox>
                                            <asp:ImageButton ID="btnGo" runat="server" OnClick="ActionHandler" CssClass="margntop2" SkinID="search-ext" 
                                                CommandName="REPORT" ToolTip="<%$ resources:Controls,Search %>" OnClientClick="return ValidatePage();"
                                                CommandArgument="SEC_ActionPanel" TabIndex="2" />
                                            <asp:HiddenField ID="hdfRefillExpenseTill" runat="server" Value="" />
                                            <asp:Label ID="lblErrRefillExpenseTill" runat="server" Text="*" CssClass="star" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div id="divReportViewer" class="reportviewer treescroll-x" runat="server">
                                            <rsweb:ReportViewer ID="rvViewReport" runat="server" BorderWidth="0" SizeToReportContent="true"
                                                Width="100%">
                                            </rsweb:ReportViewer>
                                        </div>
                                        <div id="divNodata" class="nodata" runat="server" visible="false">
                                            No Record Found
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                     <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label></div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="PettyCash" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
