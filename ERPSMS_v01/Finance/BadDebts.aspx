<%@ Page Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="BadDebts.aspx.cs" Inherits="ERPSMS_v01.Finance.BadDebts" EnableEventValidation="false"
    ValidateRequest="false" Theme="ClassicExt" Title="<%$ Resources:Captions,Title_BadDebts %>" %>

<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/Journalize/UserControls/JournalizeControlNew.ascx" TagName="Journalize"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", "dd-M-yy", false, false, false);
            GrandScriptUtils.AddDateRangeCommon("txtFromDateDetail", "hdfFromDateDetail", "txtToDateDetail", "hdfToDateDetail", "dd-M-yy", false, false, false);
            GrandScriptUtils.DatePickerCommon("txtPVDate");
        }
        function ShowListing(flag) {
            if (flag) {
                $("[id$=PageAction_List]").show();
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlListing]").show();
                $("[id$=pnlEntry]").hide();
                //$("[id$=ddlCompany]").hide();
            }
            else {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
                // $("[id$=ddlCompany]").show();
            }
            return false;
        }
        function PageViewMode(mode) {
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlDelete]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlAlert]").hide();
                $("[id$=pnlPrint]").hide();
            }
        }

        function ShowHideAdvancedSearch(flag) {
            //If flag then Show AdvancedSearch
            if (flag) {
                $("[id$=tbladvancedSearch]").show();
                $("[id$=imbShowFilter]").hide();
                $("[id$=imbHideFilter]").show();
            }
            else {
                $("[id$=tbladvancedSearch]").hide();
                $("[id$=imbShowFilter]").show();
                $("[id$=imbHideFilter]").hide();
            }
            return false;
        }
        function SelectedCheckBoxCount(mode) {
            var count = $('[id$=grdSavedBadDebit]').find('tr td input:input:radio[id$=rbtSelect]:checked').length;
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            if (mode == 1) {
                if (count == 0) {
                    msg = '<%= GetLocalResourceObject("Msg_SelectItem").ToString() %>';
                    GrandScriptUtils.ShowModal(msg, msgTitle);
                    return false;
                }
                else if (count > 1) {
                    msg = '<%= GetLocalResourceObject("Msg_SelectoneItem").ToString() %>';
                    GrandScriptUtils.ShowModal(msg, msgTitle);
                    return false;
                }
            }
            else if (mode == 0) {
                if (count > 1) {
                    msg = '<%= GetLocalResourceObject("Msg_SelectoneItem").ToString() %>';
                    GrandScriptUtils.ShowModal(msg, msgTitle);
                    return false;
                }
            }
            else if (mode == 2) {
                if (count == 0) {
                    msg = '<%= GetLocalResourceObject("Msg_SelectItem").ToString() %>';
                    GrandScriptUtils.ShowModal(msg, msgTitle);
                    return false;
                }
            }
        }

        function AfterClose(containerID) {
            if (containerID == "[id$=divJournalize]") {
                $("[id$=btnJournalizeUpdate]").click();
            }
        }
        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {           
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteSelect(targetControlID);
            }
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {            
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteSelect(targetControlID);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="updpnlBadDebit">
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
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="30"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="23"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,SaveSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="9" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="32" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" ValidationGroup="invoice" ToolTip="<%$resources:Controls,Save %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteBD" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="33" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="35" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnJournalize" CommandName="JOURNALIZE" TabIndex="36"
                                            Text="<%$resources:Journalize %>" OnClick="ActionHandler" ToolTip="<%$resources:Journalize %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-journalize" />
                                    </li>
                                    <li id="pnlAlert" runat="server" style="display: none">
                                        <asp:Button runat="server" ID="btnAlert" CommandName="ALERT" TabIndex="37" Text="<%$resources:Controls,Alert %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:Controls,Alert %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-alert" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="38" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="8" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:Cancel %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:Cancel %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="39" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="40" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="45" OnClick="ActionHandler" CommandName="BADDEBITLIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="46" OnClick="ActionHandler" CommandName="BADDEBITDETAIL"
                                OnClientClick="javascript:return SelectedCheckBoxCount(1);" CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div style="display: none">
                                <div class="search-colapse">
                                    <table>
                                        <tr>
                                            <td>
                                                <h1>
                                                    <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                                            </td>
                                            <td>
                                                <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                    ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter" />
                                                <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                    ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <%--------------colpase btn----------%>
                                <div class="clear">
                                </div>
                                <table class="table-devide" id="tbladvancedSearch" style="margin-top: 8px;">
                                    <tr id="Tr1" runat="server">
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblFrmDate" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                                <asp:TextBox ID="txtFromDate" runat="server" TabIndex="49" CssClass="medium" MaxLength="11"
                                                    onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                                <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtToDate"></asp:Label>
                                                <asp:TextBox ID="txtToDate" runat="server" TabIndex="50" CssClass="medium" MaxLength="11"
                                                    onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                                <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblSearch" runat="server" AssociatedControlID="btnSearch"></asp:Label>
                                                <asp:Button ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                                    ToolTip="<%$ resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="52"
                                                    CommandName="RELIEFCLAIMSEARCH" SkinID="btnInner-search" />
                                                <asp:Button ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear %>" ToolTip="<%$ resources:Controls,Clear %>"
                                                    TabIndex="53" OnClick="ActionHandler" CommandName="CLEAR" SkinID="btnInner-cancel-dsd" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="clear">
                                </div>
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdSavedBadDebit" Width="100%" AllowPaging="true"
                                    OnPageIndexChanging="ActionHandler" PageSize="<%$ resources:PageSize%>" AllowSorting="True"
                                    OnSorting="ActionHandler" OnRowDataBound="ActionHandler" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" runat="server" GroupName="SelectOne" AutoPostBack="true"
                                                    OnCheckedChanged="ActionHandler" ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfIBD_PK" Value='<%# Eval("ICH_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceNo %>" SortExpression="ICH_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("ICH_NO") %>' ToolTip='<%# Eval("ICH_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="22%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:OutStandingAmt %>" SortExpression="ICH_OUTSTANDING_TC">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOutstandingAmt" runat="server" Text='<%# Eval("ICH_OUTSTANDING_TC", "{0:c}") %>'
                                                    ToolTip='<%# Eval("ICH_OUTSTANDING_TC", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="23%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:OutStandingAmtClaim %>" SortExpression="ICH_OUTSTANDING_CLAIMABLE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOutAmtClaimable" runat="server" Text='<%# Eval("ICH_OUTSTANDING_CLAIMABLE", "{0:c}") %>'
                                                    ToolTip='<%# Eval("ICH_OUTSTANDING_CLAIMABLE", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="23%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BadDebitReliefClaim %>" SortExpression="ICH_BAD_DEBIT_CLAIMABLE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBDReliefClaim" runat="server" Text='<%# Eval("ICH_BAD_DEBIT_CLAIMABLE", "{0:c}") %>'
                                                    ToolTip='<%# Eval("ICH_BAD_DEBIT_CLAIMABLE", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="23%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("ASC_CSS_CLASS") %>' ToolTip='<%# Eval("STATUS_TEXT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval("STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgPosted" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? GetLocalResourceObject("unposted").ToString() : Eval("FTH_CSS_CLASS")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? Resources.Captions.NotPosted : Eval("FTH_STATUS_TEXT")%>' />
                                                <asp:HiddenField runat="server" ID="hdfPosted" Value='<%# Eval(Resources.DataFieldRes.HasJRNLEntry) %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="3%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <asp:Panel ID="pnlDetailSearch" runat="server">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblFromDateDetail" runat="server" Text="<%$resources:FromDate %>"
                                                    AssociatedControlID="txtFromDateDetail"></asp:Label>
                                                <asp:TextBox ID="txtFromDateDetail" runat="server" TabIndex="49" CssClass="input-small"
                                                    MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                                <asp:HiddenField ID="hdfFromDateDetail" runat="server" Value="" />
                                                <asp:Label ID="lblToDateDetail" runat="server" Text="<%$resources:ToDate %>" CssClass="middle-lbl"
                                                    AssociatedControlID="txtToDateDetail"></asp:Label>
                                                <asp:TextBox ID="txtToDateDetail" runat="server" TabIndex="49" CssClass="input-small"
                                                    MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                                <asp:HiddenField ID="hdfToDateDetail" runat="server" Value="" />
                                                <asp:ImageButton ID="btnShowBadDebit" runat="server" Text="<%$ resources:Controls,Search %>"
                                                    ToolTip="<%$ resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="52"
                                                    CommandName="NEW" SkinID="search-ext" Style="margin-bottom: 0px!important; margin-top: 2px;" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <div class="gridwrap" style="width: 100%; overflow: auto; max-width: 1200px;">
                                <asp:GridView ID="grdBadDebitDetails" runat="server" AutoGenerateColumns="False"
                                    EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false" ShowFooter="true"
                                    OnRowDataBound="ActionHandler" Width="100%">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" runat="server" GroupName="SelectOne" AutoPostBack="true"
                                                    OnCheckedChanged="ActionHandler" ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfInvoicePk" Value='<%# Eval("ICH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfLastModDate" Value='<%#Eval("IBD_MOD_DT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceNo %>" SortExpression="IVH_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("ICH_NO") ==""?"[NEW]":Eval("ICH_NO")%>'
                                                    ToolTip='<%# Eval("ICH_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceDate %>" SortExpression="IVH_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%#  Eval("ICH_DATE")!=""? Convert.ToDateTime( Eval("ICH_DATE")).ToString(Resources.Constants.ReportDateFormat):"" %>'
                                                    ToolTip='<%# Eval("ICH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="false" />
                                            <ItemStyle Width="5%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvAmt %>" SortExpression="ICH_AMOUNT_TC"
                                            HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceAmt" runat="server" Text='<%# Eval("ICH_AMOUNT_TC", "{0:c}")%>'
                                                    ToolTip='<%# Eval("ICH_AMOUNT_TC", "{0:c}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GSTPaid %>" SortExpression="IVH_TAX_TC"
                                            HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGSTPaid" runat="server" Text='<%# Eval("ICH_TAX_TC", "{0:c}")%>'
                                                    ToolTip='<%# Eval("ICH_TAX_TC", "{0:c}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:OutStandingAmt %>" SortExpression="IVH_OUTSTANDING_TC"
                                            HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOutStandingAmt" runat="server" Text='<%# Eval("ICH_OUTSTANDING_TC", "{0:c}")%>'
                                                    ToolTip='<%# Eval("ICH_OUTSTANDING_TC", "{0:c}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:OutStandingAmtClaim %>" SortExpression="IVH_OUTSTANDING_CLAIMABLE"
                                            HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOutStandingAmtClaimable" runat="server" Text='<%# Eval("ICH_OUTSTANDING_CLAIMABLE", "{0:c}")%>'
                                                    ToolTip='<%# Eval("ICH_OUTSTANDING_CLAIMABLE", "{0:c}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="21%" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BadDebitRelief %>" SortExpression="ICH_BAD_DEBIT_CLAIMABLE"
                                            HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBDReliefClaimable" runat="server" Text='<%# Eval("ICH_BAD_DEBIT_CLAIMABLE", "{0:c}")%>'
                                                    ToolTip='<%# Eval("ICH_BAD_DEBIT_CLAIMABLE", "{0:c}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="divScriptButtons">
                    <asp:Button ID="btnJournalizeUpdate" runat="server" OnClick="ActionHandler" CommandName="JOURNALIZEUPDATE"
                        EnableTheming="false" Style="display: none" />
                </div>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="invoice" runat="server" />
                </div>
            </div>
            <%--User Control--%>
            <div id="divJournalize" style="display: none">
                <uc1:Journalize ID="ucrJournalize" runat="server" />
            </div>
            <%--<div id="divAlert" style="display: none">
                <uc2:Alert ID="ucrAlert" runat="server" />
            </div>--%>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <asp:HiddenField ID="hdfindate" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="so" />
            </div>
            <asp:HiddenField ID="hdfJournalizeWorkFlow" Value="0" runat="server" />
            <asp:HiddenField ID="hdfSBUcompany" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCompany" runat="server" Value="0" />
            <asp:HiddenField ID="hdfMonthLimit" runat="server" Value="0" />
        </ContentTemplate>
        <%-- <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
