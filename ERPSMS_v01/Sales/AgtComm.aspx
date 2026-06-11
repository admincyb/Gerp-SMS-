<%@ Page Title="<%$ Resources:Captions,Title_AgtComm %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="AgtComm.aspx.cs" Inherits="ERPSMS_v01.Sales.AgtComm"
    Theme="ClassicExt" ValidateRequest="false" %>

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
            GrandScriptUtils.DatePickerCommon("txtFCDate");
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", "dd-M-yy", false, false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtHdrAddType", url + "?VendorPk=" + $("[id$='hdfVendor']").val(), "hdfAddTypeHdr", true, false, "VENDORCONTACTS");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url, "hdfCustomerID", true, true, "CUSTOMERLIST");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url + "?SearchBy=" + "1", "hdfCustomerID", true, true, "GETAUTOAGTCOMM");
            GrandScriptUtils.MakeAutoCompleteDDL("txtAgtnInvNo", url + "?SearchBy=" + "2", "hdfFCRPK", true, true, "GETAUTOAGTCOMM");
            //For Voucher
            GrandScriptUtils.DatePickerCommon("txtInvoiceDueDate");
            GrandScriptUtils.DatePickerCommon("txtVendorInvDate");
            GrandScriptUtils.DatePickerCommon("txtPVDate");
            GrandScriptUtils.DatePickerCommon("txtAgtCommDate");

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
        function AfterClose(containerID) {
            if (containerID == "[id$=divJournalize]") {
                $("[id$=btnJournalizeUpdate]").click();
            }
            else if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
                if ($("[id$=hdfJournalizeWorkFlow]").val() == "1") {
                    //ShowContainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), '1000', '550');
                    //ShowContainerDiv('[id$=divJournalize]', $("[id$=hdfJournalName]").val(), '1000', '550');
                    ShowCommonCotainerDiv('[id$=divJournalize]', $("[id$=hdfJournalName]").val(), "1%");
                    AfterCloseWkfInJournal();
                    //$("[id$=btnJournalize_Action]").click();
                }
            } else if (containerID == "[id$=divTemplate]") {
                //ShowContainerDiv('[id$=divJournalize]', $("[id$=hdfJournalName]").val(), '1000', '550');
                ShowCommonCotainerDiv('[id$=divJournalize]', $("[id$=hdfJournalName]").val(), "1%");
            }
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
            //            if (targetControlID == "txtVendor") {
            //                $("[id$=btnVendor]").click();
            //            }
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteSelect(targetControlID);
            }
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            //            if (targetControlID == "txtVendor") {
            //                $("[id$=btnVendor]").click();
            //            }
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteInvalidSelect(targetControlID);
            }
        }

        function CalculateTotal(sender) {
            var val1 = parseFloat($(sender).val());
            //var taxformula = $("#[id*=hdfTaxformula]").val();
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
            $("[id$=hdfCommAmtTotalFooter]").val(ReverseNow);
        }
        function CalculateTotFooter() {
            var totFooter = 0;
            var Amount = 0;
            var ActAmount = 0;
            var totActAmount = 0;
            var CurrPK = $("[id$=hdfCurrentPk]").val();
            $("[id$=grdAgtCommDetails] tr").each(function () {
                if ($(this).find("[id*=txtCommAmt]").length > 0) {
                    Amount = parseFloat($(this).find("[id*=txtCommAmt]").val());
                    ActAmount = parseFloat($(this).find("[id*=hdfCommAmt]").val());
                    totFooter = totFooter + Amount;
                    totActAmount = totActAmount + ActAmount;
                }
            });

            if (CurrPK == "0") {
                //                if (totActAmount < totFooter) {
                if (totFooter < totActAmount) {
                    document.getElementById("chkIsSetteled").checked = false;
                }
                else {
                    document.getElementById("chkIsSetteled").checked = true;
                }
            }
            $("#[id*=grdAgtCommDetails] [id*=lblCommAmtTotalFooter]").html(addCommas(totFooter.toFixed(CurrencyDigits)));
            $("[id$=hdfCommAmtTotalFooter]").val(totFooter);
        }
        var CommType = 0; function CalculateInvNow() {
            var totFooter = 0;
            //            $("[id$=grdAgtCommDetails] tr").each(function () {
            //                var CommType = 0;
            //                var Commrate = 0;
            //                var OrderQty = 0;
            //                var TaxAmount = 0;
            //                var invNow = 0;
            //                if ($(this).find("[id*=hdfCommType]").length > 0)
            //                    CommType = parseFloat($(this).find("[id*=hdfCommType]").val());
            //                if ($(this).find("[id*=lblOrderQuantity]").length > 0)
            //                    OrderQty = parseFloat($(this).find("[id*=lblOrderQuantity]").html().replace(/[^0-9\.]+/g, ""));
            //                if ($(this).find("[id*=lblTaxAmount]").length > 0)
            //                    TaxAmount = parseFloat($(this).find("[id*=lblTaxAmount]").html().replace(/[^0-9\.]+/g, ""));
            //                if ($(this).find("[id*=hdfCommrate]").length > 0)
            //                    Commrate = parseFloat($(this).find("[id*=hdfCommrate]").val());
            //                if (CommType == 1) { invNow = (TaxAmount * Commrate) / 100; } else { invNow = OrderQty * Commrate; }
            //                totFooter = totFooter + invNow;
            //                $(this).find("[id*=txtCommAmt]").val(invNow.toFixed(CurrencyDigits));
            //                if (CommType == 0) { $(this).find("[id*=txtCommAmt]").attr("disabled", true); } else { $(this).find("[id*=txtCommAmt]").attr("disabled", false); }
            //            });
            $("#[id*=grdAgtCommDetails] [id*=lblCommAmtTotalFooter]").html(addCommas(totFooter.toFixed(CurrencyDigits)));
            $("[id$=hdfCommAmtTotalFooter]").val(totFooter);
        }

        function AfterDateSelect(controlID) {
            //            if (controlID == "txtInvoiceDueDate") {
            //                var invoiceDueDate = $.datepicker.parseDate("dd-M-yy", $("[id$=txtInvoiceDueDate]").val());
            //                invoiceDueDate.setDate(invoiceDueDate.getDate() + parseInt($("[id$=txtInvoiceDueDate]").val()));
            //                $("[id$=txtInvoiceDueDate]").val($.datepicker.formatDate("mm/dd/yy", invoiceDueDate));
            //                
            //            } 
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
        function CalculateDueDate() {


            var creditDays;
            creditDays = $("[id$=txtCreditDays]").val();
            if (jQuery.trim(creditDays).length > 0) {
                var invoiceDueDate = $.datepicker.parseDate("dd-M-yy", $("[id$=hdfInvoiceDueDate]").val());
                invoiceDueDate.setDate(invoiceDueDate.getDate() + parseInt($("[id$=txtCreditDays]").val()));
                $("[id$=txtInvoiceDueDate]").val($.datepicker.formatDate("dd-M-yy", invoiceDueDate));
            }
            else {
                var invoiceDueDate = $.datepicker.parseDate("dd-M-yy", $("[id$=hdfInvoiceDueDate]").val());
                invoiceDueDate.setDate(invoiceDueDate.getDate());
                $("[id$=txtInvoiceDueDate]").val($.datepicker.formatDate("dd-M-yy", invoiceDueDate));

            }

            //            var vendDate = $.datepicker.parseDate("dd-M-yy", $("[id$=txtInvoiceDueDate]").val());
            //            var dueDays = parseInt($("[id$=txtCreditDays]").val());
            //            if (vendDate != null && !isNaN(vendDate) && dueDays != null && !isNaN(dueDays)) {
            //                vendDate.setDate(vendDate.getDate() + dueDays);
            //                $("[id$=txtInvoiceDueDate]").val($.datepicker.formatDate("dd-M-yy", vendDate));
            //            }
        }

        function CalculateDueDays() {
            var invoiceDueDate = $.datepicker.parseDate("dd-M-yy", $("[id$=hdfInvoiceDueDate]").val());
            invoiceDueDate.setDate(invoiceDueDate.getDate() + parseInt($("[id$=txtCreditDays]").val()));
            $("[id$=txtInvoiceDueDate]").val($.datepicker.formatDate("mm/dd/yy", invoiceDueDate));


            var vendDate = $.datepicker.parseDate("dd-M-yy", $("[id$=txtInvoiceDate]").val());
            var dueDate = $.datepicker.parseDate("dd-M-yy", $("[id$=txtInvoiceDueDate]").val());
            if (vendDate != null && !isNaN(vendDate) && dueDate != null && !isNaN(dueDate)) {
                var dueDays = (dueDate - vendDate) / (1000 * 60 * 60 * 24);
                if (dueDays >= 0)
                    $("[id$=txtCreditDays]").val(dueDays);
            }
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
        function SetTabs(tab) {
            if (tab == 1) {
                $("[id$='lnkList']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lnkDetails']").removeClass("tab-active").addClass("tab-inactive");
            }
            else {
                ;
                $("[id$='lnkList']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lnkDetails']").removeClass("tab-inactive").addClass("tab-active");
            }
        }

        function addCommas(number) {
            var FormattedNumber = number;
            var curGroup1 = 3;
            var curGroup2 = 3;
            var NumericPart = "", LastNumericPart = "", DecimalPart = "";
            if (!isNaN(parseFloat($("#[id*=hdfCurrencyGroup1]").val()))) {
                curGroup1 = parseFloat($("#[id*=hdfCurrencyGroup1]").val());
            }
            if (!isNaN(parseFloat($("#[id*=hdfCurrencyGroup2]").val()))) {
                curGroup2 = parseFloat($("#[id*=hdfCurrencyGroup2]").val());
            }

            DecimalPart = number.split('.')[1];
            (DecimalPart) ? DecimalPart = "." + DecimalPart : DecimalPart = "";
            NumericPart = number.split('.')[0];
            if (NumericPart.length > curGroup1) {
                LastNumericPart = NumericPart.substr(NumericPart.length - curGroup1, curGroup1);
                (LastNumericPart) ? LastNumericPart = "," + LastNumericPart : LastNumericPart = "";
            }
            if ((NumericPart.length - curGroup1) > 0) {
                NumericPart = NumericPart.substr(0, NumericPart.length - curGroup1);
                var pattern = "\\B(?=(\\d{" + curGroup2 + "})+(?!\\d))";
                var expression = new RegExp(pattern, "g");
                NumericPart = NumericPart.toString().replace(expression, ",");
            }
            FormattedNumber = NumericPart + LastNumericPart + DecimalPart;
            return FormattedNumber;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlPOInvoice">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <div id="divSBUCompany" class="buttoncontainer-fields floatLeft">
                                    <asp:DropDownList ID="ddlCompany" class="medium" runat="server" TabIndex="1" onmouseover="javascript:ShowTooltip('ddlCompany');">
                                    </asp:DropDownList>
                                </div>
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="36"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="37"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="38" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="39" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteNew" CommandName="DELETE" TabIndex="40" Text="<%$resources:ErpRes,Delete %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Delete %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Delete" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li id="pnlPrint">
                                        <asp:Button runat="server" TabIndex="41" ID="btnPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="42" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnJournalize" CommandName="JOURNALIZE" TabIndex="43"
                                            Text="<%$resources:Journalize %>" OnClick="ActionHandler" ToolTip="<%$resources:Journalize %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-journalize" />
                                    </li>
                                    <%--  <li id="pnlAlert" runat="server">
                                        <asp:Button runat="server" ID="btnAlert" CommandName="ALERT" TabIndex="10" Text="<%$resources:Controls,Alert %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:Controls,Alert %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-alert" />
                                    </li>--%>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li runat="server" id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="44" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="Cancel Agent Commission Invoice" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="Cancel Agent Commission Invoice" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="45" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="46" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                    <li id="liPrintSI">
                                        <asp:Button runat="server" TabIndex="47" ID="btnPrintSI" CommandName="PRINTLISTING"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnPOListing" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnSOListing" Text="<%$resources:PageNameRes,SalesOrder %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="48" CssClass="tab-inactive" OnClick="ActionHandler"
                                CommandName="DEFAULT"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnDeliveryOrder" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lnbDeliveryOrder" Text="<%$resources:PageNameRes,AGTINVOICE %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="49" OnClick="ActionHandler" CommandName="AGTINVOICE"
                                CssClass="tab-active"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:HiddenField ID="hdfSelectedItemPk" runat="server" Value="0" />
                <div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="1" OnClick="ActionHandler" CommandName="AGTLIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="2" OnClick="ActionHandler" CommandName="AGTDETAIL"
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
                                                TabIndex="3" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="3" />
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
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$resources:ToDate %>" CssClass="middle-lbl-small-d"
                                                AssociatedControlID="txtToDate"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="2" CssClass="input-small" MaxLength="17"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" class="middle-lbl-small"
                                                AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-b" TabIndex="3">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Posted %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,NotPosted %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="lblVno" runat="server" Text="Voucher No" AssociatedControlID="txtVno"
                                                Visible="false"></asp:Label>
                                            <asp:TextBox ID="txtVno" runat="server" CssClass="medium" MaxLength="100" TabIndex="9"
                                                Visible="false"> </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <%-- <asp:Label runat="server" ID="lblPending" Text="<%$ resources:Pending%>" AssociatedControlID="chkPending"></asp:Label>
                                            <asp:CheckBox ID="chkPending" runat="server" Checked="true" />
                                            --%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblCustomer" runat="server" Text="Agent" AssociatedControlID="txtCustomer"></asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="select-half margnbotm0" MaxLength="100"
                                                TabIndex="4"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerID" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblVoucherNumber" runat="server" CssClass="middle-lbl-small" Text="Agent Inv.No."
                                                AssociatedControlID="txtAgtnInvNo"></asp:Label>
                                            <asp:TextBox ID="txtAgtnInvNo" runat="server" CssClass="select-small-a margnbotm0"
                                                MaxLength="100" TabIndex="5"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFCRPK" runat="server" Value="" />
                                            <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="6" CommandName="SEARCH" SkinID="search-ext"
                                                Style="margin-bottom: 0px!important; margin-top: 2px;" />
                                            <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                TabIndex="7" OnClick="ActionHandler" CommandName="CLEAR" Style="margin-bottom: 0px!important;
                                                margin-top: 2px;" SkinID="clear-ext" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdAGTLIST" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                    AllowPaging="true" OnPageIndexChanging="ActionHandler" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" TabIndex="11" runat="server" GroupName="SelectOne"
                                                    AutoPostBack="true" OnCheckedChanged="ActionHandler" ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfAGTpk" Value='<%# Eval("IVH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="HiddenField3" Value='<%# Eval("IVH_DEPT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfdels" Value='<%# Eval("IVH_DEL_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:Date %>" SortExpression="IVH_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblfcrDate" runat="server" Text='<%#  Eval("IVH_DATE1", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("IVH_DATE1", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                    ToolTip='<%# Eval("IVH_DATE1", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:CommNo %>" SortExpression="IVH_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("IVH_NO") =="0" || Eval("IVH_NO") ==""?"[NEW]":Eval("IVH_NO")%>'
                                                    ToolTip='<%# Eval("IVH_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Agent" SortExpression="IVH_VENDOR_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendor" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_VENDOR_TEXT"), 30) %>'
                                                    ToolTip='<%# Eval("IVH_VENDOR_TEXT") %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfVendorPK" Value='<%# Eval("IVH_VND_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="26%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Totpcs%>" SortExpression="IVH_QTY_INVOICED"
                                            HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblTotpcs" runat="server" CssClass="ItemQuantity" Text='<%# Eval("IVH_QTY_INVOICED", "{0:n}") %>'
                                                    ToolTip='<%# Eval("IVH_QTY_INVOICED", "{0:n}") %>'></asp:Label>--%>
                                                <asp:Label ID="lblTotpcs" runat="server" CssClass="ItemQuantity" Text='<%# GetFormattedNumberWithSeperator(Eval("IVH_QTY_INVOICED")) %>'
                                                    ToolTip='<%# GetFormattedNumberWithSeperator(Eval("IVH_QTY_INVOICED")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" CssClass="amount-numeric" HorizontalAlign="Right" />
                                            <%--  <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblItemTotalQty" runat="server"></asp:Label>
                                            </FooterTemplate>--%>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:OrderQuantity%>" SortExpression="IVH_QTY_CARTON"
                                            HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblQuantity" runat="server" CssClass="ItemQuantity" Text='<%# Eval("IVH_QTY_CARTON", "{0:n}") %>'
                                                    ToolTip='<%# Eval("IVH_QTY_CARTON", "{0:n}") %>'></asp:Label>--%>
                                                <asp:Label ID="lblQuantity" runat="server" CssClass="ItemQuantity" Text='<%# GetFormattedNumberWithSeperator(Eval("IVH_QTY_CARTON")) %>'
                                                    ToolTip='<%# GetFormattedNumberWithSeperator(Eval("IVH_QTY_CARTON")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" CssClass="amount-numeric" HorizontalAlign="Right" />
                                            <%--   <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblItemTotalQty" runat="server"></asp:Label>
                                            </FooterTemplate>--%>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency%>" SortExpression="IVH_CURRENCY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCur" runat="server" Text='<%# Eval("IVH_CURRENCY_TEXT") %>' ToolTip='<%# Eval("IVH_CURRENCY_TEXT") %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfSOCurrency" Value='<%# Eval("IVH_CURRENCY") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CommAmt%>" HeaderStyle-CssClass="amount-numeric"
                                            SortExpression="IVH_AMOUNT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCommAmt" runat="server" Text='<%#GetFormattedCurrencyWithSeperator(Eval("IVH_AMOUNT")) %>'
                                                    ToolTip='<%#GetFormattedCurrencyWithSeperator(Eval("IVH_AMOUNT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PaidAmt%>" HeaderStyle-CssClass="amount-numeric"
                                            SortExpression="IVH_AMOUNT_PAID_TC">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaidCommAmt" runat="server" Text='<%#GetFormattedCurrencyWithSeperator(Eval("IVH_AMOUNT_PAID_TC")) %>'
                                                    ToolTip='<%#GetFormattedCurrencyWithSeperator(Eval("IVH_AMOUNT_PAID_TC")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BalAmt%>" HeaderStyle-CssClass="amount-numeric"
                                            SortExpression="IVH_BAL_AMNT_TC">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalCommAmt" runat="server" Text='<%#GetFormattedCurrencyWithSeperator(Eval("IVH_BAL_AMNT_TC")) %>'
                                                    ToolTip='<%#GetFormattedCurrencyWithSeperator(Eval("IVH_BAL_AMNT_TC")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("ASC_CSS_CLASS") %>' ToolTip='<%# Eval("IVH_STATUS_TEXT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval("IVH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgPosted" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? GetLocalResourceObject("unposted").ToString() : Eval("FTH_CSS_CLASS")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? Resources.Captions.NotPosted : Eval("FTH_STATUS_TEXT")%>' />
                                                <asp:HiddenField runat="server" ID="hdfPosted" Value='<%# Eval("IVH_HAS_JRNL_ENTRY") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="3%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <%--  <uc1:PagerControl ID="uclPaging" runat="server" />--%>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <table class="table-devide" id="tblDetailHdr">
                                <tr>
                                    <td style="height: 15;">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
                                            <asp:HiddenField ID="hdfCurrencyDigits" runat="server" Value="3" />
                                            <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                                            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                                            <asp:HiddenField ID="hdfAstDocMode" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdfAstCode" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdfInvoiceNoComm" runat="server" Value="" />
                                            <asp:HiddenField ID="hdfRateFormat" runat="server" />
                                            <asp:HiddenField ID="hdfAgtCommNo" runat="server" />
                                            <asp:HiddenField ID="hdfExchangeRate" runat="server" />
                                            <asp:HiddenField ID="hdfCommAmtTotalFooter" runat="server" />
                                            <asp:HiddenField ID="hdfCurrentPk" runat="server" />
                                            <asp:HiddenField ID="hdfdelsAl" Value="0" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblinvNo" Text="Invoice No." AssociatedControlID="lblInvoiceNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblInvoiceNo" CssClass="input-small"></asp:Label>
                                            <asp:Label runat="server" ID="lblAgtCommDate" Text="<%$ resources:Date%>" AssociatedControlID="txtAgtCommDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtAgtCommDate" CssClass="date-picker input-small"
                                                TabIndex="12" onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfInvoiceDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtAgtCommDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_InvoiceDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="vrfTaxDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="taxDate" EnableClientScript="true" runat="server" ControlToValidate="txtAgtCommDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_InvoiceDate %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblVendorInvNO" Text="Ref. No." AssociatedControlID="txtVendorInvNO"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtVendorInvNO" CssClass="input-small" TabIndex="13"
                                                MaxLength="100"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfVendorInvNO" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtVendorInvNO"
                                                Display="Dynamic" Text="*" ErrorMessage="Enter Ref. No.">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblVendorInvDate" Text="Ref. Date" AssociatedControlID="txtVendorInvDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtVendorInvDate" CssClass="date-picker" TabIndex="14"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfVendorInvDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtVendorInvDate"
                                                Display="Dynamic" Text="*" ErrorMessage="Enter Ref. Date">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblInvoiceDueDate" Text="Inv. Due Date" AssociatedControlID="txtInvoiceDueDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInvoiceDueDate" CssClass="date-picker input-small"
                                                TabIndex="17" onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfInvoiceDueDate" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfInvoiceDueDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtInvoiceDueDate"
                                                Display="Dynamic" Text="*" ErrorMessage="Enter Inv. Due Date">
                                            </asp:RequiredFieldValidator>
                                            <%--Start--%>
                                            <asp:DropDownList ID="ddlAddressTypeHdr" runat="server" TabIndex="16" OnSelectedIndexChanged="ActionHandler"
                                                AutoPostBack="true" CssClass="medium" Visible="false">
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hdfAddTypeHdr" runat="server" Value="" />
                                            <asp:Label runat="server" ID="lbHdrAddType" Text="Head Office/Branch" AssociatedControlID="txtHdrAddType"></asp:Label>
                                            <asp:TextBox ID="txtHdrAddType" runat="server" MaxLength="100" TabIndex="18" CssClass="input-small"
                                                OnTextChanged="ActionHandler" AutoPostBack="true"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfVendor" runat="server" Value="" />
                                            <asp:Button ID="btnHdrVendorCont" runat="server" OnClick="ActionHandler" CommandName="CHANGE"
                                                Style="display: none" EnableTheming="false" />
                                            <asp:RequiredFieldValidator ID="vrfHdrAddressType" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtHdrAddType"
                                                Display="Dynamic" Text="*" ErrorMessage="Please select type">
                                            </asp:RequiredFieldValidator>
                                            <%--End--%>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblAgtComm" runat="server" AssociatedControlID="lblAgtCommTxt" Text="Agent"></asp:Label>
                                            <asp:Label ID="lblAgtCommTxt" runat="server" CssClass="input-half-20-11-9" Text=""></asp:Label>
                                            <asp:HiddenField ID="hdfAgtComm" runat="server" />
                                            <asp:HiddenField ID="hdfSoType" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblCurrency" runat="server" AssociatedControlID="lblCurrencyTxt" Text="<%$resources:HdrCurrency %>"></asp:Label>
                                            <asp:HiddenField ID="hdfCurrency" runat="server" />
                                            <asp:Label ID="lblCurrencyTxt" runat="server" Text="" CssClass="input-small"></asp:Label>
                                            <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" InitialValue="-1" runat="server"
                                                ControlToValidate="ddlCurrency" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ddlcurr %>">
                                            </asp:RequiredFieldValidator>--%>
                                            <asp:Label runat="server" ID="lblCreditDays" CssClass="middle-lbl-small-d" Text="Credit Days"
                                                AssociatedControlID="txtCreditDays"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtCreditDays" TabIndex="15" MaxLength="100" CssClass="medium amount-numeric input-small"
                                                onchange="CalculateDueDate();"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="vreCreditDays" runat="server" ControlToValidate="txtCreditDays"
                                                ErrorMessage="Enter valid Credit Days" ValidationExpression="^\$?([0-9]{0,10})?$"
                                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice">
                                            </asp:RegularExpressionValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblHdrchkBH" Width="22px" Text="H/O" AssociatedControlID="HdrchkHO"></asp:Label>
                                            <asp:CheckBox ID="HdrchkHO" runat="server" Checked="false" TabIndex="20" OnCheckedChanged="ActionHandler"
                                                AutoPostBack="true" />
                                            <%--<asp:Label runat="server" ID="lblHdrBrCode" Text="<%$ resources:BranchCode%>" AssociatedControlID="txtHdrBranchCode"></asp:Label>--%>
                                            <asp:TextBox ID="txtHdrBranchCode" runat="server" MaxLength="5" TabIndex="19" Width="93px"
                                                CssClass="input-disabled" Enabled="false" />
                                            <asp:RequiredFieldValidator ID="vrfHdrBranchCode" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtHdrBranchCode"
                                                Display="Dynamic" Text="*" ErrorMessage="Enter Branch ID">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblHdrTaxId" Text="Tax ID" CssClass="middle-lbl-small-d"
                                                AssociatedControlID="txtHdrTaxId"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtHdrTaxId" TabIndex="20" CssClass="input-small"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks %>" AssociatedControlID="txtRemarks"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRemarks" TabIndex="20" TextMode="MultiLine" CssClass="multiline-3line"
                                                onkeydown="limitText(this,500);" onchange="limitText(this,500);"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <%-- <td style="height: 35px;">--%>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblchkIsSetteled" Width="22px" Text="Is Settled" AssociatedControlID="chkIsSetteled"></asp:Label>
                                            <asp:CheckBox ID="chkIsSetteled" runat="server" Checked="false" TabIndex="20" ClientIDMode="Static" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap scroll-container">
                                <asp:Button ID="btnRecalculate" runat="server" EnableTheming="false" Style="display: none"
                                    OnClick="ActionHandler" CommandName="RECALCULATE" />
                                <asp:GridView ID="grdAgtCommDetails" runat="server" AutoGenerateColumns="False" Width="1300px"
                                    AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                    ShowFooter="true" OnRowDataBound="ActionHandler" TabIndex="23">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfUOM" runat="server" Value='<%#Eval("VID_UOM") %>' />
                                                <asp:HiddenField ID="hdfDtlPK" runat="server" Value='<%#Eval("AVD_INV_CUS_DTL") %>' />
                                                <asp:Label ID="lblDate" runat="server" Text='<%#  Eval("AVD_INV_CUS_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("AVD_INV_CUS_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                    ToolTip='<%# Eval("AVD_INV_CUS_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="1px" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvNo %>">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkRecno" runat="server" CssClass="text-underline" OnClick="ActionHandler"
                                                    CommandName="SHOWPOPUP" CommandArgument='<%#Eval("ICH_PK") %>' Text='<%# HttpUtility.HtmlDecode(Eval("AVD_INV_CUS_NO").ToString()) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("AVD_INV_CUS_NO").ToString()) %>'></asp:LinkButton>
                                                <%--   <asp:Label ID="lblInvNo" runat="server" Text='<%#Eval("AVD_INV_CUS_NO") %>' ToolTip='<%# HttpUtility.HtmlDecode(Eval("AVD_INV_CUS_NO").ToString()) %>'></asp:Label>--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="1px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Customer %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerGrd" runat="server" Text='<%# HttpUtility.HtmlDecode(Eval("AVD_CUSTOMER_CODE").ToString()) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("AVD_CUSTOMER_TEXT").ToString()) %>'></asp:Label>
                                                <asp:HiddenField ID="hdfCus" runat="server" Value='<%#Eval("AVD_CUSTOMER") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="25px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SONo %>">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkSoNo" runat="server" CssClass="text-underline" OnClick="ActionHandler"
                                                    CommandName="SHOWSC" CommandArgument='<%#Eval("AVD_SO_HDR") %>' Text='<%# HttpUtility.HtmlDecode(Eval("AVD_SO_NO").ToString()) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("AVD_SO_NO").ToString()) %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="4px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Brand %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfItem" runat="server" Value='<%#Eval("AVD_CUST_ITEM") %>' />
                                                <asp:Label ID="lblBrandName" runat="server" Text='<%# HttpUtility.HtmlDecode(Eval("AVD_BRAND_NAME").ToString()) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("AVD_BRAND_NAME").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="170px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:OrderQuantity%>" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderQuantity" runat="server" CssClass="ItemQuantity" Text='<%#GetFormattedNumberWithSeperator(Eval("VID_QTY_CARTON")) %>'
                                                    ToolTip='<%#GetFormattedNumberWithSeperator(Eval("VID_QTY_CARTON")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="6px" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Totpcs %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotPcs" runat="server" CssClass="ItemQuantity" Text='<%#GetFormattedNumberWithSeperator(Eval("VID_SALE_QTY"))  +" "+ Eval("VID_UOM_TEXT") %>'
                                                    ToolTip='<%#GetFormattedNumberWithSeperator(Eval("VID_SALE_QTY")) +" "+ Eval("VID_UOM_TEXT") %>'></asp:Label>
                                                <asp:HiddenField ID="hdfTotPcs" runat="server" Value='<%#GetFormattedNumber(Eval("VID_SALE_QTY")) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="6px" HorizontalAlign="Right" Wrap="false" />
                                            <HeaderStyle HorizontalAlign="Right" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Rate %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdCommissionRate" runat="server" Value='<%#Eval("AVD_INV_CUS_RATE") %>' />
                                                <asp:Label ID="lblTaxRate" runat="server" Text='<%# Convert.ToString(Eval("AVD_INV_CUS_RATE")) == string.Empty ? string.Empty : GetFormattedRate(Eval("AVD_INV_CUS_RATE")) %>'
                                                    ToolTip='<%#Eval("AVD_INV_CUS_RATE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5px" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TrxAmount %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrencyWithSeperator(Eval("AVD_INV_CUS_AMOUNT")) %>'
                                                    ToolTip='<%#GetFormattedCurrencyWithSeperator(Eval("AVD_INV_CUS_AMOUNT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5px" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:CommType %>"  Visible="false" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblCommType" runat="server" Text='<%#Eval("AVD_COMMISION_TYPE") %>' ToolTip='<%#Eval("AVD_COMMISION_TYPE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="40%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Commrate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCommrate" runat="server" Text='<%#Eval("AVD_COMMISSION_FORMULA_TEXT")%>'
                                                    ToolTip='<%#(Eval("AVD_COMMISSION_FORMULA")) %>'></asp:Label>
                                                <%--  <asp:Label ID="lblCommrate" runat="server" Text='<%#Eval("AVD_COMMISSION_RATE")  +  (Convert.ToInt16(Eval("AVD_COMMISSION_TYPE"))==1?" %" :(Convert.ToInt16(Eval("AVD_COMMISSION_TYPE"))==2)? "/Ctn" :" ")  %>'
                                                    ToolTip='<%#GetFormattedRate(Eval("AVD_COMMISSION_FORMULA")) %>'></asp:Label>--%>
                                                <%--<asp:Label ID="lblCommrate" runat="server" Text='<%#Eval("AVD_COMMISSION_RATE")  +  (Convert.ToInt16(Eval("AVD_COMMISSION_TYPE"))==1?" %" :(Convert.ToInt16(Eval("AVD_COMMISSION_TYPE"))==2)? " Per Ctn" :" ")  %>'
                                                    ToolTip='<%#GetFormattedRate(Eval("AVD_COMMISSION_RATE")) %>'></asp:Label>--%>
                                                <asp:HiddenField ID="hdfCommrate" runat="server" Value='<%#Eval("AVD_COMMISSION_RATE") %>' />
                                                <asp:HiddenField ID="hdfCommType" runat="server" Value='<%#Eval("AVD_COMMISSION_TYPE") %>' />
                                                <asp:HiddenField ID="hdfCommFormulaText" runat="server" Value='<%#Eval("AVD_COMMISSION_FORMULA") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="15px" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalText" Text="<%$ resources:TotalFooterText %>" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CommAmt %>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCommAmt" runat="server" Text='<%#GetFormattedCurrency(Eval("AVD_COMMISSION_AMT")) %>'
                                                    CssClass="input-w70 numeric " MaxLength="15" onchange="CalculateTotFooter(this);"
                                                    TabIndex="24" ToolTip='<%#GetFormattedCurrency(Eval("AVD_COMMISSION_AMT")) %>'></asp:TextBox>
                                                <asp:HiddenField ID="hdfCommAmt" runat="server" Value='<%#GetFormattedCurrency(Eval("AVD_COMMISSION_AMT")) %>' />
                                                <asp:RequiredFieldValidator ID="vrfAmount" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtCommAmt"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                </asp:RequiredFieldValidator>
                                                <cc1:AmountValidation ID="vamAmount" runat="server" ControlToValidate="txtCommAmt"
                                                    ErrorMessage="<%$ resources:Err_Invalid_Amount %>" NumberDigits="11" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"></cc1:AmountValidation>
                                              <%--  <asp:CompareValidator ID="cmpAmount" runat="server" ValidationGroup="invoice" ControlToValidate="txtCommAmt"
                                                    Type="Double" Operator="GreaterThan" ValueToCompare="0" Text="*" EnableClientScript="true"
                                                    Display="Dynamic" ErrorMessage="<%$ resources:Err_AmtGreaterZero %>" CssClass="star"></asp:CompareValidator>--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="8px" HorizontalAlign="Right" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblCommAmtTotalFooter" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoicedAmount %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoicedamt" runat="server" Text='<%#GetFormattedCurrencyWithSeperator(Eval("AVD_INVOICED_AMT")) %>'
                                                 ToolTip='<%#GetFormattedCurrencyWithSeperator(Eval("AVD_INVOICED_AMT")) %>' >
                                                  </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="75px" HorizontalAlign="Right" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="gridwrap">
                                <table id="tblCalc" style="display: none">
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="lblTax" Text="<%$ resources:Tax%>" AssociatedControlID="txtHdrTax"></asp:Label>
                                        </td>
                                        <td style="text-align: right" class="btn-margin">
                                            <asp:ImageButton ID="imgHdrTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                ValidationGroup="taxHdrDate" OnClientClick="javascript:ValidatePageNow('taxHdrDate')"
                                                TabIndex="25" ToolTip="<%$ resources:Tax %>" CommandName="TAXHEADER" />
                                            <asp:TextBox ID="txtHdrTax" runat="server" CssClass="input-w80 numeric input-disabled"
                                                Enabled="false" MaxLength="16" TabIndex="26"></asp:TextBox>
                                            <div class="clear">
                                                <div class="clear">
                                                </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="lblTotal" Text="<%$ resources:Total%>" AssociatedControlID="txtHdrTotal"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:TextBox ID="txtHdrTotal" runat="server" CssClass="input-w80 numeric input-disabled"
                                                Enabled="false" MaxLength="16" TabIndex="27"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="divcol-S" style="display: none">
                                <asp:Label ID="lblFileUpload" runat="server" Text="AttachFile" AssociatedControlID="fupUpload"></asp:Label>
                                <asp:FileUpload ID="fupUpload" runat="server" TabIndex="28" Style="width: 15.6%;" />
                                <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupUpload"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_File_Upload %>">
                                                        
                                </asp:RequiredFieldValidator>
                                <a id="anchorFile" runat="server" target="_blank" tabindex="11"></a>
                                <asp:Button runat="server" ID="btnUpload" CommandName="ADDITEMUPLOAD" TabIndex="29"
                                    OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('upload')"
                                    ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="PageAction_Entry" ValidationGroup="upload"
                                    Text="<%$resources:ErpRes,Add %>" SkinID="btnInner-add" />
                                <%--  <div class="btnwrap-divcol">
                                            
                                        </div>--%>
                                <div class="clear">
                                </div>
                            </div>
                            <div class="gridwrap" style="display: none">
                                <asp:GridView runat="server" ID="grdUploads" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="false" AllowPaging="false" OnSorting="ActionHandler" OnPageIndexChanging="ActionHandler"
                                    OnRowDataBound="ActionHandler" AutoGenerateColumns="false" TabIndex="30" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:SlNo %>">
                                            <ItemTemplate>
                                                <%-- <%# Container.DataItemIndex + 1 %>--%>
                                                <%-- <asp:Label ID="lblSlNo" runat="server" Text='<%# Eval("DOC_SEQ_NO") %>' ToolTip='<%# Eval("DOC_SEQ_NO") %>'></asp:Label>--%>
                                                <asp:HiddenField runat="server" ID="hdfPK" Value='<%# Eval("DOC_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:File %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFile" runat="server" Text='<%# Eval("DOC_NAME") %>' ToolTip='<%# Eval("DOC_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="82%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <a runat="server" id="fileView" class="download-icon nomargin" title="<%$ resources:View %>"
                                                    target="_blank" href='<%# Page.ResolveClientUrl(Eval("DOC_PATH").ToString()) %>'
                                                    tabindex="31"></a>
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkEdit" runat="server" OnClick="ActionHandler" CommandName="EDITITEMUPLOAD"
                                                    SkinID="edit-icon" ToolTip="Edit" CommandArgument="PageAction_Entry" OnLoad="btnAction_Load"
                                                    OnPreRender="btnAction_PreRender" TabIndex="32" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEMUPLOAD"
                                                    SkinID="delete-icon" ToolTip="Delete" CommandArgument="PageAction_Entry" OnClientClick="return ShowDeleteConfirm(this);"
                                                    OnLoad="btnAction_Load" OnPreRender="btnAction_PreRender" TabIndex="33" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
                <div id="divScriptButtons">
                    <asp:Button runat="server" ID="btnJournalize_Action" CommandName="JOURNALIZE" OnClick="ActionHandler"
                        EnableTheming="false" Style="display: none" TabIndex="34" />
                    <asp:Button ID="btnJournalizeUpdate" runat="server" OnClick="ActionHandler" CommandName="JOURNALIZEUPDATE"
                        EnableTheming="false" Style="display: none" TabIndex="35" />
                </div>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary
                        ID="vsPage" ValidationGroup="invoice" runat="server" />
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
                <div id="divAlert" style="display: none">
                    <%-- <uc2:Alert ID="ucrAlert" runat="server" />--%>
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
                <asp:HiddenField ID="hdfDecimalFormatWithSeperator" runat="server" />
                <asp:HiddenField ID="hdfCurrencyFormatWithSeperator" runat="server" />
                <asp:HiddenField runat="server" ID="hdfCurrencyGroup1" Value="3" />
                <asp:HiddenField runat="server" ID="hdfCurrencyGroup2" Value="2" />
                <asp:HiddenField ID="hdfIsCancelled" runat="server" Value="0" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
