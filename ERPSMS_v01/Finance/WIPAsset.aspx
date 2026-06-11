<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="WIPAsset.aspx.cs"
    Inherits="ERPSMS_v01.Finance.WIPAsset" %>

<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc2" %>
<%@ Register Src="~/Journalize/UserControls/JournalizeControlNew.ascx" TagName="Journalize" TagPrefix="uc3" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtCWIPDate");
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", "dd-M-yy", false, false, false);
            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtCWIPNoSearch", url, "hdfCWIPNoSearch", true, true, false, "CWIPNO");
            InitCWIPAccount();
            $("[id$=txtTrnsNow]").ForceNumericOnly();
        }
        function InitCWIPAccount() {
            var CurrPK = '<%=CurrPK%>';
            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtCWIPAccount", url + "?Group=" + $("[id$=ddlAccGroup]").val() + "&itemPK=" + CurrPK, "hdfCWIPAccPK", true, true, false, "CWIPACCOUNT");
        }
        function ShowListing(flag) {
            if (flag) {
                $("[id$=PageAction_List]").show();
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlListing]").show();
                $("[id$=pnlEntry]").hide();
                $("[id$=btnPrint]").hide();
                
            }
            else {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
                $("[id$=btnPrint]").show();
            }
            return false;
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
                }
            }
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
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
        function AfterClose(containerID) {
            if (containerID == "[id$=divJournalize]") {
                $("[id$=btnJournalizeUpdate]").click();
            }
            else if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
                if ($("[id$=hdfJournalizeWorkFlow]").val() == "1") {
                    //ShowContainerDiv('[id$=divJournalize]', '<%= GetLocalResourceObject("CWIPJournal") %>', '1000', '550');
                    ShowCommonCotainerDiv('[id$=divJournalize]', '<%= GetLocalResourceObject("CWIPJournal") %>', "1%");
                    AfterCloseWkfInJournal();
                    //$("[id$=btnJournalize_Action]").click();
                }
            } else if (containerID == "[id$=divTemplate]") {
                //ShowContainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), '1000', '550');
                ShowCommonCotainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), "1%");
            }
        }
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtCWIPAccount") {
                $("[id$=btnChangeValue]").click();
            }
        }
        function EnableDisableCWIPAccount(enable) {
            if (enable == 1) {
                EnableAuto($("[id$=txtCWIPAccount]"), $("[id$=hdfCWIPAccPK]"));
            }
            else {
                DisableAuto($("[id$=txtCWIPAccount]"), $("[id$=hdfCWIPAccPK]"));
            }
        }
        function CheckAllInvoices(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById("<%=grdCOATransactions.ClientID %>");
            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }
        function UpdateTransferNow(targetRow) {
            $("[id$=hdfTrnsRowPK]").val(targetRow.dataset.rowid);
            $("[id$=hdfTrnsNowNew]").val(targetRow.value);
            $("[id$=btnCalculate]").click();
        }
    </script>
    <style>
        #ctl00_MainContent_grdCOATransactions input[type="text"] {
            width: 100px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlPOInvoice">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <div class="buttoncontainer-fields floatLeft" id="divSBUCompany">
                                    <asp:DropDownList ID="ddlCompany" class="select-full-a margnbotm0" runat="server" TabIndex="1"
                                        onmouseover="javascript:ShowTooltip('ddlCompany');">
                                    </asp:DropDownList>
                                </div>
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="13"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="50"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('cwip')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnWIPSubmit" CommandName="SUBMIT" TabIndex="51" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('cwip')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="52" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('cwip')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteCWIP" CommandName="DELETE" TabIndex="53" Text="<%$resources:ErpRes,Delete %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Delete %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Delete" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>

                                    <li id="pnlPrint" runat="server">
                                        <asp:Button runat="server" TabIndex="54" ID="btnPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Visible="true" Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
                                    </li>

                                    <%--<li runat="server" id="pnlPrint" >
                                        <asp:Button runat="server" TabIndex="54" ID="btnPrint" Visible="false" CommandName="PRINT"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
                                    </li>--%>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="15" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnJournalize" CommandName="JOURNALIZE" TabIndex="54"
                                            Text="<%$resources:Journalize %>" OnClick="ActionHandler" ToolTip="<%$resources:Journalize %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-journalize" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li runat="server" id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="64" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelSubmit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="55" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="56" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="57" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                    <li>
                                          <asp:Button runat="server" TabIndex="42" ID="btnListPrint" CommandName="PRINTLISTING"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
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
                                CommandArgument="SEC_ActionPanel" TabIndex="43" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="44" OnClick="ActionHandler" CommandName="DETAILS"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1><%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="1" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="1" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn----------%>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblFrmDate" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="2" CssClass="input-small"
                                                MaxLength="17" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtToDate"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="3" CssClass="input-small" MaxLength="17"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblCWIPAccSearch" runat="server" Text="<%$resources:CWIPAccount %>" AssociatedControlID="ddlCWIPAccSearch"></asp:Label>
                                            <asp:DropDownList ID="ddlCWIPAccSearch" runat="server" TabIndex="4" CssClass="input-half">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblVoucherNumber" runat="server" Text="<%$resources:CWIPNo %>" AssociatedControlID="txtCWIPNoSearch"></asp:Label>
                                            <asp:TextBox ID="txtCWIPNoSearch" runat="server" CssClass="select-small-i margnbotm0"
                                                MaxLength="100" TabIndex="4"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCWIPNoSearch" runat="server" Value="0" />
                                            <%--<asp:Label ID="lblVno" runat="server" Text="<%$resources:FCVoucherNo %>" AssociatedControlID="txtVno"
                                                CssClass="middle-lbl margnbotm0"></asp:Label>
                                            <asp:TextBox ID="txtVno" runat="server" CssClass="input-small margnbotm0" MaxLength="100"
                                                TabIndex="4"> </asp:TextBox>--%>
                                        </div>
                                    </td>
                                  
                                    <td>
                                        <div class="div2col-S div-separatn">
                                    <asp:Label runat="server" ID="lblPoNum" Text="PO Number" AssociatedControlID="txtPoNum" CssClass="margnbotm0"></asp:Label>
                                    <asp:TextBox ID="txtPoNum" runat="server" Width="40px" CssClass="margnbotm0 input-medium" TabIndex="4"></asp:TextBox>

                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" AssociatedControlID="ddlStatus" CssClass="margnbotm0 lbl-9perc"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="input-small margnbotm0" TabIndex="5">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Drafted %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Submitted %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Approved %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,rejected %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="4"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="5" CommandName="SEARCH" SkinID="search-ext"
                                                Style="margin-bottom: 0px; margin-top: 2px" />
                                            <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="5" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext" Style="margin-bottom: 0px; margin-top: 2px" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdCWIPAssetList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                    AllowPaging="true" OnPageIndexChanging="ActionHandler" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" TabIndex="6" runat="server" GroupName="SelectOne"
                                                    AutoPostBack="true" OnCheckedChanged="ActionHandler" ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfCWIPAssetPK" Value='<%# Eval("CWH_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date %>" SortExpression="CWH_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrdDate" runat="server" Text='<%#  Eval("CWH_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("CWH_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                    ToolTip='<%# Eval("CWH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CWIPNo %>" SortExpression="CWH_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrdNo" runat="server" Text='<%# Eval("CWH_NO") =="0" || Eval("CWH_NO") ==""?"[NEW]":Eval("CWH_NO")%>'
                                                    ToolTip='<%# Eval("CWH_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AssetCode %>" SortExpression="CWH_ASSET_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrdAssetCode" runat="server" Text='<%# Eval("CWH_ASSET_CODE")%>'
                                                    ToolTip='<%# Eval("CWH_ASSET_CODE")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AssetCost %>" SortExpression="AssetCost">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrdAssetCost" runat="server" Text='<%# Eval("AssetCost", "{0:c}") %>'
                                                    ToolTip='<%# Eval("AssetCost", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="18%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AccGroup %>" SortExpression="CWH_GROUP">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrdAccGroup" runat="server" Text='<%# Eval("CWH_GROUP_TEXT") %>'
                                                    ToolTip='<%# Eval("CWH_GROUP_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CWIPAccount %>" SortExpression="CWH_ACCOUNT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrdAccount" runat="server" Text='<%# Eval("CWH_ACCOUNT_TEXT") %>'
                                                    ToolTip='<%# Eval("CWH_ACCOUNT_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Status %>" SortExpression="CWH_STATUS_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrdStatus" runat="server" Text='<%# Eval("CWH_STATUS_TEXT") %>'
                                                    ToolTip='<%# Eval("CWH_STATUS_TEXT") %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval("CWH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgPosted" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? GetLocalResourceObject("unposted").ToString() : Eval("FTH_CSS_CLASS")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? Resources.Captions.NotPosted : Eval("FTH_STATUS_TEXT")%>' />
                                                <%--<asp:HiddenField runat="server" ID="hdfPosted" Value='<%# Eval("IVH_HAS_JRNL_ENTRY") %>' />--%>
                                                <asp:HiddenField runat="server" ID="hdfJournalStatus" Value='<%# Eval("FTH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button runat="server" ID="imgInvAmend" ToolTip="<%$resources:InvoiceAmended %>" OnClientClick="javascript:return false;"
                                                    TabIndex="14" Visible='<%# Eval("CWH_INV_AMD_FLAG").ToString() == "1" ? true : false %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc2:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <table class="table-devide" id="tblDetailHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblforCWIPNo" Text="<%$ resources:CWIPNo%>" AssociatedControlID="lblCWIPNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblCWIPNo" CssClass="input-small"></asp:Label>
                                            <asp:Label runat="server" ID="lblAccGroup" Text="<%$ resources: AccGroup %>" CssClass="lbl-20-5perc"
                                                AssociatedControlID="ddlAccGroup">
                                            </asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlAccGroup" OnSelectedIndexChanged="ActionHandler" CssClass="select-small-a" AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfAccGroup" CssClass="star" SetFocusOnError="true" ValidationGroup="cwip"
                                                EnableClientScript="true" InitialValue="-1" runat="server" ControlToValidate="ddlAccGroup"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_AccountGroup %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <div id="divAcc" class="div2col-S" runat="server">
                                                <asp:Label runat="server" ID="lblCWIPAccount" Text="<%$ resources:CWIPAccount%>"
                                                    AssociatedControlID="txtCWIPAccount"></asp:Label>
                                                <%--<asp:DropDownList ID="ddlCWIPAccount" OnSelectedIndexChanged="ActionHandler" runat="server"
                                                    AutoPostBack="true" TabIndex="7" CssClass="select-half">
                                                </asp:DropDownList>--%>
                                                <asp:TextBox ID="txtCWIPAccount" runat="server" CssClass="select-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfCWIPAccPK" runat="server" Value="0" />
                                                <asp:RequiredFieldValidator ID="vrfCWIPAccount" CssClass="star" SetFocusOnError="true" ValidationGroup="cwip"
                                                    EnableClientScript="true" InitialValue="-1" runat="server" ControlToValidate="txtCWIPAccount"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_CWIPAccount %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:ImageButton runat="server" ID="imbRefresh" ToolTip="<%$resources:Refresh %>"
                                                    TabIndex="14" SkinID="imbUpdate" CommandName="REFRESH" OnClick="ActionHandler" Visible="false" />
                                            </div>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblPoNumberDetail" Text="PO Number" AssociatedControlID="txtPoNumberDetail" CssClass="margnbotm0 lbl-12perc"></asp:Label>
                                            <asp:TextBox ID="txtPoNumberDetail" runat="server" Width="40px" CssClass="margnbotm0 input-medium" TabIndex="4"></asp:TextBox>
                                            <asp:ImageButton ID="ImbSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="5" CommandName="SEARCHPO" SkinID="search-ext"
                                                Style="margin-bottom: 0px; margin-top: 2px" />
                                            <asp:Label CssClass="lbl-30perc" runat="server" ID="lblCWIPDate" Text="<%$ resources:Date%>" AssociatedControlID="txtCWIPDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtCWIPDate" CssClass="input-small" TabIndex="6" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfCWIPDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="cwip" EnableClientScript="true" runat="server" ControlToValidate="txtCWIPDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_CWIPDate %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblDeprCalculated" Text="<%$ resources:DeprCalculated %>" Visible="false" CssClass="margn-lft12-5perc"></asp:Label>
                                            <%--<asp:Label runat="server" ID="lblCurrency" Text="<%$ resources:Currency%>" AssociatedControlID="ddlCurrency"></asp:Label>
                                            <asp:DropDownList ID="ddlCurrency" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true"
                                                runat="server" CssClass="select-small-a" TabIndex="8">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" InitialValue="-1" runat="server"
                                                ControlToValidate="ddlCurrency" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ddlcurr %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lbnRate" Text="<%$ resources:Rate%>" AssociatedControlID="txtExchngRate"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtExchngRate" TabIndex="10" CssClass="input-small numeric"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfRate" CssClass="star" SetFocusOnError="true" ValidationGroup="invoice"
                                                EnableClientScript="true" runat="server" ControlToValidate="txtExchngRate" Display="Dynamic"
                                                Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                            </asp:RequiredFieldValidator>
                                            <cc1:RateValidation ID="vreRate" runat="server" ControlToValidate="txtExchngRate"
                                                ErrorMessage="<%$ resources:Err_Rate_Valid %>" NumberDigits="10" Display="Dynamic"
                                                DecimalDigits="5" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"
                                                NonZero="true"></cc1:RateValidation>--%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <asp:GridView ID="grdCOATransactions" runat="server" AutoGenerateColumns="False" Width="100%" PageSize="<%$ resources:PageSize %>"
                                    AllowPaging="true" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false" OnPageIndexChanging="ActionHandler"
                                    ShowFooter="true" OnRowDataBound="ActionHandler" TabIndex="11">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAllInvoice" runat="server" AutoPostBack="true" OnCheckedChanged="ActionHandler" />
                                                <%--onclick="CheckAllInvoices(this);"--%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkInvoice" runat="server" Checked='<%# Convert.ToInt32(Eval("FTR_SELECTED")) == 1 ? true : false %>'
                                                    AutoPostBack="true" OnCheckedChanged="ActionHandler"></asp:CheckBox>
                                                <asp:HiddenField runat="server" Value='<%# Eval("FTR_PK") %>' ID="hdfInvoice" />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PoNumberHdr %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPoNumber" runat="server" Text='<%# (Eval("FTH_PO_NUMBER") == null)? " " : Eval("FTH_PO_NUMBER").ToString() %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("FTH_PO_NUMBER").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvDate" runat="server" Text='<%#  Eval("InvoiceDate", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("InvoiceDate", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                    ToolTip='<%# Eval("InvoiceDate", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceNo %>">
                                            <ItemTemplate>
                                                <%--<asp:LinkButton ID="lnkInvoiceNo" runat="server" CssClass="text-underline" OnClick="ActionHandler"
                                                    CommandName="SHOW" CommandArgument='<%#Eval("InvoicePK") %>' Text='<%#Eval("InvoiceNo") %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("InvoiceNo").ToString()) %>'></asp:LinkButton>--%>
                                                <asp:Label ID="lblInvoiceNo" runat="server" Text='<%#Eval("InvoiceNo") %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("InvoiceNo").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfFTHPK" runat="server" Value='<%#Eval("FTH_PK") %>' />
                                                <asp:Label ID="lblDate" runat="server" Text='<%#  Eval("FTH_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("FTH_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                    ToolTip='<%# Eval("FTH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:VoucherNo %>">
                                            <ItemTemplate>
                                                <%--<asp:LinkButton ID="lnkReceiptVoucher" runat="server" CssClass="text-underline" OnClick="ActionHandler"
                                                    CommandName="SHOW" CommandArgument='<%#Eval("FTH_PK") %>' Text='<%#Eval("VoucherNo") %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("VoucherNo").ToString()) %>'></asp:LinkButton>--%>
                                                <asp:Label ID="lblVoucherNo" runat="server" Text='<%#Eval("VoucherNo") %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("VoucherNo").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:NarrationHdr %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNarration" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("FTR_NARRATION"),50)%>'
                                                    ToolTip='<%# Eval("FTR_NARRATION")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="26%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='<%# HttpUtility.HtmlDecode(Eval("TranCurrencyText").ToString()) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("TranCurrencyText").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:FCExcnng %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExchangeRate" runat="server" Text='<%#GetFormattedRate(Eval("ExchangeRate")) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("ExchangeRate").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" Width="6%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DebitAmt %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDrAmount" runat="server" Text='<%#Eval("DebitAmtBC","{0:c}") %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("DebitAmtBC","{0:c}").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CreditAmt %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCrAmount" runat="server" Text='<%#Eval("CreditAmtBC","{0:c}") %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("CreditAmtBC","{0:c}").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TrnsNow %>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTrnsNow" runat="server" CssClass="numeric" Text='<%#Eval("TransNow","{0:c}") %>'
                                                    onblur="UpdateTransferNow(this);" data-rowID='<%#Eval("FTR_PK") %>'></asp:TextBox>
                                                <asp:HiddenField ID="hdfTrnsAmount" runat="server" Value='<%#Eval("TransAmount") %>' />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="amount-numeric" Width="5%" />
                                            <HeaderStyle CssClass="amount-numeric" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDrCr" runat="server" Text='<%#(Eval("IsDebit").ToString() == "1") ? "Dr" : "Cr" %>'></asp:Label>
                                                <asp:Button runat="server" ID="imgInvAmend" ToolTip="<%$resources:InvoiceAmended %>" OnClientClick="javascript:return false;"
                                                    TabIndex="14" Visible='<%# Eval("AmendFlag").ToString() == "1" ? true : false %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <table class="table-devide">
                                <tr>
                                    <td></td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblAssetCost" Text="<%$ resources:AssetCost%>" CssClass="lbl-75-6perc"
                                                AssociatedControlID="txtAssetCost"></asp:Label>
                                            <asp:TextBox ID="txtAssetCost" runat="server" CssClass="input-small numeric" TabIndex="12" Enabled="false">
                                            </asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="divScriptButtons">
                    <asp:Button runat="server" ID="btnJournalize_Action" CommandName="JOURNALIZE" OnClick="ActionHandler"
                        EnableTheming="false" Style="display: none" />
                    <asp:Button ID="btnJournalizeUpdate" runat="server" OnClick="ActionHandler" CommandName="JOURNALIZEUPDATE"
                        EnableTheming="false" Style="display: none" />
                </div>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="cwip" runat="server" />
                    <asp:HiddenField ID="hdfAppType" runat="server" />
                    <asp:HiddenField ID="hdfAppSubType" runat="server" />
                    <asp:HiddenField ID="hdfSaveWithoutAllocation" runat="server" />
                    <asp:HiddenField ID="hdfgroup" runat="server" Value="0" />
                </div>
                <%--User Control--%>
                <div id="divJournalize" style="display: none">
                    <uc3:Journalize ID="ucrJournalize" runat="server" />
                </div>
                <div id="divWkfSubmit" style="display: none;">
                    <asp:HiddenField ID="HiddenField1" Value="0" runat="server" />
                    <asp:HiddenField ID="HiddenField2" runat="server" />
                    <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="invoice">
                    </uc1:WorkflowUserComments>
                </div>

                <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
                <asp:HiddenField ID="hdfCurrencyDigits" runat="server" Value="3" />
                <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
                <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                <asp:HiddenField ID="hdfRateFormat" runat="server" />
                <asp:HiddenField ID="hdfExchangeRateFormat" runat="server" />
                <asp:HiddenField ID="hdfExchangeRate" runat="server" />

                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <asp:HiddenField ID="hdfRefreshClicked" Value="0" runat="server" />
                <asp:HiddenField ID="hdfIsRedirected" runat="server" Value="0" />
                <asp:HiddenField ID="hdfIsInvCancelled" runat="server" Value="0" />
                <asp:HiddenField ID="hdfJournalizeWorkFlow" Value="0" runat="server" />
                <asp:HiddenField ID="hdfIsCancelled" runat="server" Value="0" />

                <asp:HiddenField ID="hdfTrnsRowPK" runat="server" Value="0" />
                <asp:HiddenField ID="hdfTrnsNowNew" runat="server" Value="0" />

                <div style="display: none">
                    <asp:Button ID="btnChangeValue" runat="server" CommandName="CHANGEVALUE" OnClick="ActionHandler" />
                    <asp:Button ID="btnCalculate" runat="server" CommandName="CALCULATE" OnClick="ActionHandler" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
