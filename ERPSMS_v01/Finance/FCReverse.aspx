<%@ Page Title="<%$ Resources:Captions,Title_FCReverse %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="FCReverse.aspx.cs" Inherits="ERPSMS_v01.Finance.FCReverse"
    Theme="ClassicExt" %>

<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/Journalize/UserControls/JournalizeControlNew.ascx" TagName="Journalize"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var NumberDgiits = 0;
        var CurrencyDigits = 0;
        $(document).ready(function () {
            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
        });
        function ScrollDown() {
            window.scroll(400, 400);
            return false;
        }
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtVoucherNumber", url + "?Type=" + $("[id$=hdfgroup]").val(), "hdfFCRPK", true, true, "FCVOUCHERNUMBER");
            GrandScriptUtils.DatePickerCommon("txtFCDate");
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", "dd-M-yy", false, false, false);
            //For Voucher
            GrandScriptUtils.DatePickerCommon("txtPVDate");
            CalculateTotal();
            $("[id$=txtJournalExchangeRate]").ForceNumericOnly();
            $("[id*=txtReverseNow]").ForceNumericOnly();
            if ($('[id$=btnJournalSaveSubmit]').is(":visible"))
                $('[id$=btnJournalSubmit]').hide();
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();
            //Set a stamp for cancelled record
            if ($("[id$=hdfIsCancelled]").val() == "1")
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
            else
                $("[id$=tblDetailHdr]").addClass("table-devide");
            //End
        }
        function ShowListing(flag) {
            if (flag) {
                $("[id$=PageAction_List]").show();
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlListing]").show();
                $("[id$=pnlEntry]").hide();
                $("[id$=ddlCompany]").hide();
            }
            else {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
                $("[id$=ddlCompany]").show();
            }
            return false;
        }
        //For   check  Already Paid
        function ShowAlreadyPaid() {
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
        function DisableAuto(extender, hfield) {
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
            $(extender).autocomplete("option", "disabled", true);
            $(extender).attr("disabled", true);
        }

        function EnableAuto(extender) {
            $(extender).removeAttr("disabled");
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect-disable").addClass("ddlSelect");
            $(extender).autocomplete("option", "disabled", false);
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
                AfterJournalControlAutoCompleteInvalidSelect(targetControlID);
            }
        }
        function CalculateTotal(sender) {
            var val1 = parseFloat($(sender).val());
            var Amount = 0;
            var ReverseNow = 0;
            var DecimalDigits = 0;
            if (!isNaN(parseFloat($("[id$=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("[id$=hdfDecimalDigits]").val());
            }
            //Calc Total
            $("#[id*=grdFcHoldDetasils] input[type=text][id*=txtReverseNow]").each(function (index) {
                //Check if number is not empty
                if ($.trim($(this).val()) != "") {
                    //Check if number is a valid integer
                    if (!isNaN(parseFloat($(this).val()))) {
                        ReverseNow = ReverseNow + parseFloat($(this).val());
                    }
                }
            });
            $("#[id*=grdFcHoldDetasils] [id*=lblTotalReverseNowFooter]").html(ReverseNow.toFixed(DecimalDigits));
            $("[id$=hdfAmtTC]").val(ReverseNow);
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
        function AfterClose(containerID) {
            if (containerID == "[id$=divJournalize]") {
                $("[id$=btnJournalizeUpdate]").click();
            }
            else if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
                if ($("[id$=hdfJournalizeWorkFlow]").val() == "1") {
                    //ShowContainerDiv('[id$=divJournalize]', '<%= GetLocalResourceObject("Purchase_Invoice_Journal") %>', '1000', '550');
                    ShowCommonCotainerDiv('[id$=divJournalize]', '<%= GetLocalResourceObject("Purchase_Invoice_Journal") %>', "1%");
                    AfterCloseWkfInJournal();
                }
            } else if (containerID == "[id$=divTemplate]") {
                //ShowContainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), '1000', '550');
                ShowCommonCotainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), "1%");
            }
        }
        function CalculateDueDate() {
            var vendDate = $.datepicker.parseDate("dd-M-yy", $("[id$=txtInvoiceDate]").val());
            var dueDays = parseInt($("[id$=txtCreditDays]").val());
            if (vendDate != null && !isNaN(vendDate) && dueDays != null && !isNaN(dueDays)) {
                vendDate.setDate(vendDate.getDate() + dueDays);
                $("[id$=txtInvoiceDueDate]").val($.datepicker.formatDate("dd-M-yy", vendDate));
            }
        }
        function CalculateDueDays() {
            var vendDate = $.datepicker.parseDate("dd-M-yy", $("[id$=txtInvoiceDate]").val());
            var dueDate = $.datepicker.parseDate("dd-M-yy", $("[id$=txtInvoiceDueDate]").val());
            if (vendDate != null && !isNaN(vendDate) && dueDate != null && !isNaN(dueDate)) {
                var dueDays = (dueDate - vendDate) / (1000 * 60 * 60 * 24);
                if (dueDays >= 0)
                    $("[id$=txtCreditDays]").val(dueDays);
            }
        }
        function AfterDateSelect(controlID) {
        }
        function CalculateInvNow() {
            $("[id$=grdInvoice] tr").each(function () {
                var orderQty = 0;
                var invQty = 0;
                if ($(this).find("[id*=lblOrderQuantity]").length > 0)
                    orderQty = parseFloat($(this).find("[id*=lblOrderQuantity]").html());
                if ($(this).find("[id*=lblInvQuantity]").length > 0)
                    invQty = parseFloat($(this).find("[id*=lblInvQuantity]").html());
                if (!isNaN(orderQty) && !isNaN(invQty) && orderQty > invQty) {
                    var invNow = parseFloat(orderQty - invQty);
                    $(this).find("[id*=txtInvNow]").val(invNow.toFixed(NumberDigits));
                }
            });
            $("[id$=btnRecalculate]").click();
        }
        function ResetSelection() {
            $('[id$=grdInvoiceList]').find('tr td input:radio[id$=rbtSelect]').removeAttr('checked');
        }
        function CheckAllocation(sender, args) {
            var split = $(sender).closest('tr').find('[id*=txtDedAllocateNowSplit]').val();
            var pattern = new RegExp($(sender).closest('tr').find('[id*=vreDedAllocateNowSplit]')[0].validationexpression);
            var spliAmount = parseFloat(split);
            if (pattern.test(split) && !isNaN(spliAmount)) {
                var bal = 0;
                var allocate = 0;
                if (!isNaN(parseFloat($(sender).closest('tr').find('[id*=lblDedInvoiceBal]').html()))) {
                    var number = Number($(sender).closest('tr').find('[id*=lblDedInvoiceBal]').html().replace(/[^0-9\.]+/g, ""));
                    bal = parseFloat(number);
                }
                allocate = parseFloat(args.Value);
                if (bal < allocate) {
                    args.IsValid = false;
                } else {
                    args.IsValid = true;
                }
            }
            else {
                args.IsValid = true;
            }
        }
        function CalculateTotalSplit(sender) {
            var val1 = parseFloat($(sender).val());
            var Amount = 0;
            var BalancetoPay = 0;
            var DecimalDigits = 0;
            $("#[id*=grdDeduction] input[type=text][id*=txtDedAllocateNowSplit]").each(function (index) {
                if (!isNaN(parseFloat($(this).closest('tr').find('.BalancetoAllocate').text()))) {
                    var number = Number($(this).closest('tr').find('.BalancetoAllocate').text().replace(/[^0-9\.]+/g, ""));
                    BalancetoPay = parseFloat(number);

                }
                if ($.trim($(this).val()) != "") {
                    if (!isNaN(parseFloat($(this).val()))) {

                        $(this).parent("td").find('input[type=hidden][id$=hdfDedAllocateNowSplit]').val($(this).val());
                        Amount = Amount + parseFloat($(this).val());
                    }
                }
            });
            $("#[id*=grdDeduction] [id*=lblDedTotalAllocateNowFooterSplit]").html(Amount.toFixed(CurrencyDigits));
            $("#[id*=grdDeduction] [id*=hdfDedTotalAllocateNowFooterSplit]").val(Amount);
        }
        function ShowSaveWithoutAllocationConfirm(btn) {
        }
    </script>
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
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="51" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="52" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" TabIndex="53" Text="<%$resources:ErpRes,Delete %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Delete %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Delete" />
                                    </li>
                                    <li id="pnlPrint">
                                        <asp:Button runat="server" TabIndex="54" ID="btnPrint" Visible="false" CommandName="PRINT"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
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
                                            OnClick="ActionHandler" Text="<%$resources:CancelFC %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelFC %>" />
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
                                            <h1>
                                                <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
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
                                            <asp:Label ID="lblHold" runat="server" Text="<%$resources:HoldAccount %>" AssociatedControlID="ddlHold"></asp:Label>
                                            <asp:DropDownList ID="ddlHold" runat="server" TabIndex="4" CssClass="input-small">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblVoucherNumber" runat="server" Text="<%$resources:FCNo %>" AssociatedControlID="txtVoucherNumber"
                                                ></asp:Label>
                                            <asp:TextBox ID="txtVoucherNumber" runat="server" CssClass="input-small margnbotm0"
                                                MaxLength="100" TabIndex="4"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFCRPK" runat="server" Value="" />
                                            <asp:Label ID="lblVno" runat="server" Text="<%$resources:FCVoucherNo %>" AssociatedControlID="txtVno"
                                                CssClass="middle-lbl margnbotm0"></asp:Label>
                                            <asp:TextBox ID="txtVno" runat="server" CssClass="input-small margnbotm0" MaxLength="100"
                                                TabIndex="4"> </asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" AssociatedControlID="ddlStatus" CssClass="margnbotm0"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="input-small margnbotm0"
                                                TabIndex="4">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,NotPosted %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Posted %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="4"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="5" CommandName="SEARCH" SkinID="search-ext"
                                                Style="margin-bottom: 0px; margin-top: 2px" />
                                            <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="5" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext" Style="margin-bottom: 0px;
                                                margin-top: 2px" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdFCRList" Width="100%" PageSize="<%$ resources:PageSize%>"
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
                                                <asp:HiddenField runat="server" ID="hdfHRHpk" Value='<%# Eval("HRH_PK") %>' />
                                                <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval("HRH_DEPT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval("HRH_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfDelStatus" Value='<%# Eval("HRH_DEL_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:FCDate %>" SortExpression="HRH_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblfcrDate" runat="server" Text='<%#  Eval("HRH_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("HRH_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                    ToolTip='<%# Eval("HRH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:FCNo %>" SortExpression="HRH_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("HRH_NO") =="0" || Eval("HRH_NO") ==""?"[NEW]":Eval("HRH_NO")%>'
                                                    ToolTip='<%# Eval("HRH_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:FCVoucherNo %>" SortExpression="FTH_DTL_VOUCHER_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVoucherNo" runat="server" Text='<%# Eval("FTH_DTL_VOUCHER_NO") =="0" || Eval("FTH_DTL_VOUCHER_NO") ==""?"[NEW]":Eval("FTH_DTL_VOUCHER_NO")%>'
                                                    ToolTip='<%# Eval("FTH_DTL_VOUCHER_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:FCBank %>" SortExpression="HRH_BANK_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFCBank" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("HRH_BANK_TEXT"),45)%>'
                                                    ToolTip='<%# Eval("HRH_BANK_TEXT")%>'></asp:Label>
                                                <asp:HiddenField ID="hdfRateFormat" Value='<%# Eval("HRH_BANK_TEXT")%>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="35%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:FCCurrency %>" SortExpression="HRH_CURRENCY_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval("HRH_CURRENCY_TEXT") %>'
                                                    ToolTip='<%# Eval("HRH_CURRENCY_TEXT") %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfCurrPK" Value='<%# Eval("HRH_CURRENCY") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:FCExcnng %>" SortExpression="HRH_EXCHG_RATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExchngRate" runat="server" Text='<%#GetFormattedExchangeRate(Eval("HRH_EXCHG_RATE"))  %>'
                                                    ToolTip='<%#GetFormattedExchangeRate(Eval("HRH_EXCHG_RATE"))  %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:FCAmount %>" SortExpression="HRH_AMOUNT_TC">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceValue" runat="server" Text='<%# Eval("HRH_AMOUNT_TC", "{0:c}") %>'
                                                    ToolTip='<%# Eval("HRH_AMOUNT_TC", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:FCAmountthb %>" SortExpression="HRH_AMOUNT_BC">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalAmt" runat="server" Text='<%# Eval("HRH_AMOUNT_BC", "{0:c}") %>'
                                                    ToolTip='<%# Eval("HRH_AMOUNT_BC", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("ASC_CSS_CLASS")%>' ToolTip='<%# Eval("HRH_STATUS_TEXT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfApproved1" Value='<%# Eval("HRH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgPosted" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? GetLocalResourceObject("unposted").ToString() : Eval("FTH_CSS_CLASS")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? Resources.Captions.NotPosted : Eval("FTH_STATUS_TEXT")%>' />
                                                <asp:HiddenField runat="server" ID="hdfPosted" Value='<%# Eval("HRH_HAS_JRNL_ENTRY") %>' />
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
                            <table class="table-devide" id="tblDetailHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
                                            <asp:HiddenField ID="hdfCurrencyDigits" runat="server" Value="3" />
                                            <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                                            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                                            <asp:HiddenField ID="hdfRateFormat" runat="server" />
                                            <asp:HiddenField ID="hdfExchangeRateFormat" runat="server" />
                                            <asp:HiddenField ID="hdfInvoicePK" runat="server" />
                                            <asp:HiddenField ID="hdfExchangeRate" runat="server" />
                                            <asp:HiddenField ID="hdfTaxCategory" runat="server" />
                                            <asp:HiddenField ID="hdfTaxSettings" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdfPOItemType" runat="server" />
                                            <asp:HiddenField ID="hdfFCReverseNo" runat="server" />
                                            <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                                            <asp:HiddenField ID="AST_CODE" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblReversalNo" Text="<%$ resources:ReversalNo%>" AssociatedControlID="lblFcReversalNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblFcReversalNo" CssClass="input-small"></asp:Label>
                                            <div class="clear">
                                            </div>
                                            <div id="divVendorBranch" class="div2col-S" runat="server">
                                                <asp:Label runat="server" ID="lblVendorBranch" Text="<%$ resources:HoldAccount%>"
                                                    AssociatedControlID="ddlHoldAccount"></asp:Label>
                                                <asp:DropDownList ID="ddlHoldAccount" OnSelectedIndexChanged="ActionHandler" runat="server"
                                                    AutoPostBack="true" TabIndex="7" CssClass="select-half">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="vrfMode" CssClass="star" SetFocusOnError="true" ValidationGroup="invoice"
                                                    EnableClientScript="true" InitialValue="-1" runat="server" ControlToValidate="ddlHoldAccount"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_HoldAccount %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblFCDate" Text="<%$ resources:Date%>" AssociatedControlID="txtFCDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFCDate" CssClass="input-small" TabIndex="6" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfInvoiceDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtFCDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_InvoiceDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="vrfTaxDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="taxDate" EnableClientScript="true" runat="server" ControlToValidate="txtFCDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_InvoiceDate %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblCurrency" Text="<%$ resources:Currency%>" AssociatedControlID="ddlCurrency"></asp:Label>
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
                                                NonZero="true"></cc1:RateValidation>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <asp:Button ID="btnRecalculate" runat="server" EnableTheming="false" Style="display: none"
                                    OnClick="ActionHandler" CommandName="RECALCULATE" />
                                <asp:GridView ID="grdFcHoldDetasils" runat="server" AutoGenerateColumns="False" Width="100%"
                                    AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                    ShowFooter="true" OnRowDataBound="ActionHandler" TabIndex="11">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfHRDpk" runat="server" Value='<%#Eval("HRD_PK") %>' />
                                                <asp:HiddenField ID="hdfFtrPK" runat="server" Value='<%#Eval("FTR_PK") %>' />
                                                <asp:Label ID="lblDate" runat="server" Text='<%#  Eval("FTH_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("FTH_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                    ToolTip='<%# Eval("FTH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ReceiptVoucherShort %>">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkReceiptVoucher" runat="server" CssClass="text-underline" OnClick="ActionHandler"
                                                    CommandName="SHOW" CommandArgument='<%#Eval("FTH_PK") %>' Text='<%#Eval("FTH_VOUCHER_NO") %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("FTH_VOUCHER_NO").ToString()) %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Recno %>">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkRecno" runat="server" CssClass="text-underline" OnClick="ActionHandler"
                                                    CommandName="SHOWPOPUP" CommandArgument='<%#Eval("FTH_REF_PK") %>' Text='<%# HttpUtility.HtmlDecode(Eval("FTH_REF_NO").ToString()) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("FTH_REF_NO").ToString()) %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Customer %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPartyName" runat="server" Text='<%# HttpUtility.HtmlDecode(Eval("FTH_PARTY_NAME").ToString()) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("FTH_PARTY_NAME").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="32%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='<%# HttpUtility.HtmlDecode(Eval("CUR_CODE").ToString()) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("CUR_CODE").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:FCExcnng %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExchangeRate" runat="server" Text='<%#GetFormattedRate(Eval("FTR_EXCHG_RATE")) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("FTR_EXCHG_RATE").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%#Eval("FTR_AMOUNT_TC","{0:c}") %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("FTR_AMOUNT_TC","{0:c}").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Reversed %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReversed" runat="server" Text='<%#Eval("FTR_REVERTED","{0:c}") %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("FTR_REVERTED","{0:c}").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ReverseNow%>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtReverseNow" runat="server" Text='<%#GetFormattedNumber(Eval("HRD_AMOUNT")) %>'
                                                    CssClass="input-w97 numeric" MaxLength="13" TabIndex="11" onkeyup="CalculateTotal(this);"
                                                    OnTextChanged="ActionHandler" AutoPostBack="true"></asp:TextBox>
                                                <asp:HiddenField ID="hdfInvNow" runat="server" />
                                                <asp:RequiredFieldValidator ID="vrfInvNow" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtReverseNow"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                </asp:RequiredFieldValidator>
                                                <cc1:QuantityValidation ID="vreInvNow" runat="server" ControlToValidate="txtReverseNow"
                                                    NumberDigits="9" ErrorMessage="<%$ resources:Err_Invalid_InvNow %>" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"></cc1:QuantityValidation>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" Width="10%" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalReverseNowFooter"></asp:Label></FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lbnBankCharge" Text="<%$ resources:BankCharge%>" AssociatedControlID="ddlBankCharge"></asp:Label>
                                            <asp:DropDownList ID="ddlBankCharge" runat="server" CssClass="input-small" TabIndex="12">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" InitialValue="-1" runat="server"
                                                ControlToValidate="ddlBankCharge" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ddlBankCharge %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtBankcharge" runat="server" CssClass="input-small numeric" MaxLength="13"
                                                TabIndex="13"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfBankcharge" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtBankcharge"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Bankcharge %>">
                                            </asp:RequiredFieldValidator>
                                            <cc1:RateValidation ID="RateValidation1" runat="server" ControlToValidate="txtBankcharge"
                                                ErrorMessage="<%$ resources:Err_Bankcharge_Valid %>" NumberDigits="10" Display="Dynamic"
                                                Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="details"
                                                NonZero="true"></cc1:RateValidation>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblDescription" Text="<%$ resources:Description %>"
                                                AssociatedControlID="txtDescription"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDescription" TabIndex="13" TextMode="MultiLine"
                                                CssClass="multiline-2line" onkeydown="limitText(this,500);" onchange="limitText(this,500);"></asp:TextBox>
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
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="invoice" runat="server" />
                    <asp:ValidationSummary ID="vsTax" ValidationGroup="tax" runat="server" />
                    <asp:ValidationSummary ID="vsTaxDate" ValidationGroup="taxDate" runat="server" />
                    <asp:ValidationSummary ID="vsUpload" ValidationGroup="upload" runat="server" />
                    <asp:ValidationSummary ID="vsDeduction" ValidationGroup="deduction" runat="server" />
                    <asp:HiddenField ID="hdfAppType" runat="server" />
                    <asp:HiddenField ID="hdfAppSubType" runat="server" />
                    <asp:HiddenField ID="hdfSaveWithoutAllocation" runat="server" />
                    <asp:HiddenField ID="hdfgroup" runat="server" Value="0" />
                </div>
                <%--User Control--%>
                <div id="divJournalize" style="display: none">
                    <uc1:Journalize ID="ucrJournalize" runat="server" />
                </div>
                <div id="divWkfSubmit" style="display: none;">
                    <asp:HiddenField ID="HiddenField1" Value="0" runat="server" />
                    <asp:HiddenField ID="HiddenField2" runat="server" />
                    <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="invoice">
                    </uc1:WorkflowUserComments>
                </div>
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <asp:HiddenField ID="hdfIscontYes" runat="server" />
                <asp:HiddenField ID="hdfAmtTC" runat="server" Value="0.0" />
                <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
                <asp:HiddenField ID="hdfJournalizeWorkFlow" Value="0" runat="server" />
                <asp:HiddenField ID="hdfIsCancelled" runat="server" Value="0" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
