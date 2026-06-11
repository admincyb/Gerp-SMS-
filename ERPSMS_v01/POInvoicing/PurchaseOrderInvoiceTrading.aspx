<%@ Page Title="<%$ Resources:Captions,Title_POInvoice %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="PurchaseOrderInvoiceTrading.aspx.cs" Inherits="ERPSMS_v01.POInvoicing.PurchaseOrderInvoiceTrading"
    Theme="ClassicExt" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/Journalize/UserControls/JournalizeControlNew.ascx" TagName="Journalize"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/AlertControl.ascx" TagName="Alert" TagPrefix="uc2" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var NumberDigits = 0;
        var CurrencyDigits = 0;
        $(document).ready(function () {
            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
        });

        function ScrollDown() {
            window.scroll(400, 800);
            return false;
        }

        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtInvoiceDate");
            GrandScriptUtils.DatePickerCommon("txtInvoiceDueDate");
            GrandScriptUtils.DatePickerCommon("txtSupplierInvDate");            
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", "dd-M-yy", false, false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendorSearch", url, "hdfVendorSearch", true, true, "VENDOR");
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendorHd", url, "hdfVendorHd", true, true, "VENDOR");
            GrandScriptUtils.MakeAutoCompleteDDL("txtInvoiceNumber", url + "?InvCategory=" + $("[id$=hdfInvCategory]").val(), "hdfIVHPK", true, true, "GETDIRECTINVOICENOAUTO");     
            GrandScriptUtils.DatePickerCommon("txtPVDate");
            GrandScriptUtils.DatePickerCommon("txtDueAson");
            GrandScriptUtils.AddDateRangeCommon("txtPendingFromDate", "hdfPendingFromDate", "txtPendingToDate", "hdfPendingToDate", false, false);            
            if ($('[id$=btnJournalSaveSubmit]').is(":visible"))
                $('[id$=btnJournalSubmit]').hide();
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();

            //Set a stamp for cancelled invoice
            if ($("[id$=hdfIsInvCancelled]").val() == "1")
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
            else
                $("[id$=tblDetailHdr]").addClass("table-devide");

            //End
            $("[id*=txtEffRate]").ForceNumericOnly();
            InitPONumberAuto();
            if ($("[id$=txtPONumber]").attr("disabled") == true) {
                DisableAuto($("[id$=txtPONumber]"), $("[id$=hdfPoPK]"));
            }
            else {
                EnableAuto($("[id$=txtPONumber]"), $("[id$=hdfPoPK]"));
            }
            ShowHidePendingPO($("[id$=hdfIsPendingPOVisible]").val());
            if ($("[id$=txtVendorHd]").attr("disabled") == true) {
                DisableAuto($("[id$=txtVendorHd]"), $("[id$=hdfVendorHd]"));
            }
            if ($("[id$=txtPONumber]").attr("disabled") == true) {
                DisableAuto($("[id$=txtPONumber]"), $("[id$=hdfPoPK]"));
            }
        }

        function InitPONumberAuto() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtPONumber", url + "?customerid=" + $("[id$=hdfVendorHd]").val() + "&InvoicePk=" + $("[id$=hdfInvPk]").val(), "hdfPoPK", true, true, "PURCHASEORDERNO");           
        }
        function ResetPONumberAuto() {
            var defText = '<%= Resources.ErpRes.AutoDefaultValue %>';
            $("[id$=txtPONumber]").val(defText);
            $("[id$=hdfPoPK]").val('-1');
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

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Msg_Cont_Confirm").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfIscontYes]").val(1);
                        $(this).dialog("close");
                        $("[id$=btnPickForPayment]").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfIscontYes]").val(0);
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }


        //For   check  Other charges entered exceed the value given in PO while savesubmit
        //1-Save,2-Submit
        function ShowOtherChargesExceeds(val) {

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Msg_OtherChargeExceed_Confirm").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=HdfIsContYesOtherCharges]").val(1);
                        $(this).dialog("close");
                        if (val == '1') {
                            $("[id$=btnSave]").click();
                        }
                        else if (val == '2') {
                            $("[id$=btnSaveSubmit]").click();
                        }
                    },
                    Cancel: function (e) {
                        $("[id$=HdfIsContYesOtherCharges]").val(0);
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }

        //End



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
                $("[id$=pnlPrintdt]").hide();
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
            if (targetControlID == "txtVendorHd") {
                ResetPONumberAuto();
                $("[id$=btnVendor]").click();
            }
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteSelect(targetControlID);
            }
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtVendorHd") {
                ResetPONumberAuto();
                $("[id$=btnVendor]").click();
            }
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteInvalidSelect(targetControlID);
            }
        }
        function CalculateTotal(sender) {
            var subTotal = parseFloat($("#[id*=grdInvoice]").find('input[type=Text][id$=txtSubTotalFooter]').val());
            subTotal = isNaN(subTotal) ? 0 : subTotal;

            var tax = 0;
            $("[id$=grdInvoice] tr").each(function () {
                if ($(this).find("[id*=txtTax]").length > 0) {
                    var tempTax = 0;
                    tempTax = parseFloat($(this).find("[id*=txtTax]").val());
                    tax = tax + tempTax;
                }
            });      

            var subTotalAdj = parseFloat($("#[id*=grdInvoice]").find('input[type=Text][id$=txtAdjustAmountFooter]').val());
            subTotalAdj = isNaN(subTotalAdj) ? 0 : subTotalAdj;
            subTotal = subTotal + subTotalAdj;
            var IsAdvInvHasTax = parseFloat($("[id$=hdfIsAdvHasTax]").val());
            if (IsAdvInvHasTax != 0) {
                var totalDiscount = parseFloat($("[id$=txtHdrDiscount]").val());
                totalDiscount = isNaN(totalDiscount) ? 0 : totalDiscount;
                $("[id$=txtHdrTotal]").val(toFixed((subTotal - totalDiscount), CurrencyDigits));
                var totalDeduction = parseFloat($("[id$=txtHdrDeduction]").val());
                var AdvDeduction = parseFloat($("[id$=txtDiscDeducted]").val());
                totalDeduction = isNaN(totalDeduction) ? 0 : totalDeduction;
                $("[id$=txtHdrBalBeforeVat]").val((((subTotal - totalDiscount) - totalDeduction) - AdvDeduction).toFixed(CurrencyDigits));
                var totalTax = parseFloat($("[id$=txtHdrTax]").val());
                totalTax = isNaN(totalTax) ? 0 : totalTax;
                var totalShipping = parseFloat($("[id$=txtShipping]").val());
                totalShipping = isNaN(totalShipping) ? 0 : totalShipping;
                var totalPriceAdj = parseFloat($("[id$=txtPriceAdj]").val());
                totalPriceAdj = isNaN(totalPriceAdj) ? 0 : totalPriceAdj;
                var BalBeforeVat = parseFloat($("[id$=txtHdrBalBeforeVat]").val());
                //Deducting Other Charges
                var DeductOtherCharges = parseFloat($("[id$=txtDeductOtherCharges]").val());             
                var netTotal = (BalBeforeVat + totalTax + (totalShipping - DeductOtherCharges) + totalPriceAdj); 
                $("[id$=txtHdrNetTotal]").val((netTotal).toFixed(CurrencyDigits));
                $("[id$=txtHdrNetTotal]").attr("title", (netTotal).toFixed(CurrencyDigits));
                if (totalShipping == 0)
                    $("[id$=txtShipping]").val((totalShipping).toFixed(CurrencyDigits));
                if (totalPriceAdj == 0)
                    $("[id$=txtPriceAdj]").val((totalPriceAdj).toFixed(CurrencyDigits));
            }
            else {
                var Total = parseFloat($("[id$=txtTotal]").val());
                var totalDed = parseFloat($("[id$=txtTotalDeduction]").val());

                var totalDiscount = parseFloat($("[id$=txtHdrDiscount]").val());
                totalDiscount = isNaN(totalDiscount) ? 0 : totalDiscount;              
                totalDeduction = isNaN(totalDeduction) ? 0 : totalDeduction;             
                var totalTax = parseFloat($("[id$=txtHdrTax]").val());
                totalTax = isNaN(totalTax) ? 0 : totalTax;
                var totalShipping = parseFloat($("[id$=txtShipping]").val());
                totalShipping = isNaN(totalShipping) ? 0 : totalShipping;
                var totalPriceAdj = parseFloat($("[id$=txtPriceAdj]").val());
                totalPriceAdj = isNaN(totalPriceAdj) ? 0 : totalPriceAdj;
              
                $("[id$=txtTotal]").val((subTotal - totalDiscount + totalTax + totalShipping + totalPriceAdj).toFixed(CurrencyDigits));               
                netTotal = (subTotal - totalDiscount + totalTax + totalShipping + totalPriceAdj + subTotalAdj) - totalDed;
                $("[id$=txtHdrNetTotal]").val((netTotal).toFixed(CurrencyDigits));
                $("[id$=txtHdrNetTotal]").attr("title", (netTotal).toFixed(CurrencyDigits));               
            }
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
                    ShowCommonCotainerDiv('[id$=divJournalize]', '<%= GetLocalResourceObject("Purchase_Invoice_Journal") %>', "1%");
                    AfterCloseWkfInJournal();                  
                }
            } else if (containerID == "[id$=divTemplate]") {               
                ShowCommonCotainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), "1%");
            }
        }
        function CalculateDueDate() {
            var vendDate = $.datepicker.parseDate("dd-M-yy", $("[id$=txtSupplierInvDate]").val());
            var dueDays = parseInt($("[id$=txtCreditDays]").val());
            if (vendDate != null && !isNaN(vendDate) && dueDays != null && !isNaN(dueDays)) {
                vendDate.setDate(vendDate.getDate() + dueDays);
                $("[id$=txtInvoiceDueDate]").val($.datepicker.formatDate("dd-M-yy", vendDate));
            }
        }
        function CalculateDueDays() {
            var vendDate = $.datepicker.parseDate("dd-M-yy", $("[id$=txtSupplierInvDate]").val());
            var dueDate = $.datepicker.parseDate("dd-M-yy", $("[id$=txtInvoiceDueDate]").val());
            if (vendDate != null && !isNaN(vendDate) && dueDate != null && !isNaN(dueDate)) {
                var dueDays = (dueDate - vendDate) / (1000 * 60 * 60 * 24);
                if (dueDays >= 0)
                    $("[id$=txtCreditDays]").val(dueDays);
            }
        }
        function AfterDateSelect(controlID) {
            if (controlID == "txtSupplierInvDate") {
                CalculateDueDate();
            }
            else if (controlID == "txtInvoiceDueDate") {
                CalculateDueDays();
            }
            else if (controlID == "txtInvoiceDate" && $("[id$=hdfHasTax]").val() != "0") {
                ShowErrorMessage('<%=Resources.Messages.TaxDateChanged %>', '<%=Resources.Messages.Information %>');
            }
            if (typeof AfterAlertControlDateSelect == "function") {
                AfterAlertControlDateSelect(controlID);
            }
        }
        function CalculateInvNow() {
            $("[id$=grdInvoice] tr").each(function () {
                var orderQty = 0;
                var invQty = 0;
                if ($(this).find("[id*=lblOrderQuantity]").length > 0)
                    orderQty = parseFloat($(this).find("[id*=lblOrderQuantity]").html().replace(/[^0-9\.]+/g, ""));
                if ($(this).find("[id*=lblInvQuantity]").length > 0)
                    invQty = parseFloat($(this).find("[id*=lblInvQuantity]").html().replace(/[^0-9\.]+/g, ""));
                if (!isNaN(orderQty) && !isNaN(invQty) && orderQty > invQty) {
                    var invNow = parseFloat(orderQty - invQty);
                    $(this).find("[id*=txtInvNow]").val(toFixed(invNow, NumberDigits));
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
            var prevOtherAmount = 0;
            var curOtherAmount = 0;
            var prevTax = 0;
            var prevDisc = 0;
            var curTax = 0;
            var curDisc = 0;
            var taxInUnit = 0;
            var discInUnit = 0;
            var otherChargeInUnit = 0;
            var allocateNow = 0;
            var totOtherAmount = 0;
            var totTax = 0;
            var totDisc = 0;
            var PaidAmount = 0;
            var PaidAmount1 = 0;         

            $("#[id*=grdDeduction] input[type=text][id*=txtDedAllocateNowSplit]").each(function (index) {
                if (!isNaN(parseFloat($(this).closest('tr').find('.BalancetoAllocate').text()))) {
                    var number = Number($(this).closest('tr').find('.BalancetoAllocate').text().replace(/[^0-9\.]+/g, ""));
                    BalancetoPay = parseFloat(number);

                }
                //
                if (!isNaN($(this).closest('tr').find("#[id*=hdfPaidOtherAmount]").val())) {
                    var number = Number($(this).closest('tr').find("#[id*=hdfPaidOtherAmount]").val().replace(/[^0-9\.]+/g, ""));
                    prevOtherAmount = parseFloat(number);
                }
                if (!isNaN($(this).closest('tr').find("#[id*=hdfPaidTax]").val())) {
                    var number = Number($(this).closest('tr').find("#[id*=hdfPaidTax]").val().replace(/[^0-9\.]+/g, ""));
                    prevTax = parseFloat(number);
                }
                if (!isNaN($(this).closest('tr').find("#[id*=hdfpaidDisc]").val())) {

                    var number = Number($(this).closest('tr').find("#[id*=hdfpaidDisc]").val().replace(/[^0-9\.]+/g, ""));

                    prevDisc = parseFloat(number);

                }
                //
                if ($.trim($(this).val()) != "") {
                    if (!isNaN(parseFloat($(this).val()))) {                       
                        PaidAmount = parseFloat($(this).closest('tr').find('[id*=lblDedReceiptAmount]').html().replace(/[^0-9\.]+/g, ""));
                        allocateNow = parseFloat($(this).val());
                        otherChargeInUnit = prevOtherAmount / PaidAmount;
                        taxInUnit = prevTax / PaidAmount;
                        discInUnit = prevDisc / PaidAmount;

                        curOtherAmount = allocateNow * otherChargeInUnit;
                        curTax = allocateNow * taxInUnit;
                        curDisc = allocateNow * discInUnit;
                        totOtherAmount += curOtherAmount;
                        totTax += curTax;
                        totDisc += curDisc;

                        $(this).parent("td").find('input[type=hidden][id$=hdfDedAllocateNowSplit]').val($(this).val());                      
                        $(this).closest('tr').find("#[id*=hdfCurPaidOtherAmount]").val(curOtherAmount.toFixed(CurrencyDigits));
                        $(this).closest('tr').find("#[id*=hdfCurPaidTax]").val(curTax.toFixed(CurrencyDigits));
                        $(this).closest('tr').find("#[id*=hdfCurPaidDisc]").val(curDisc.toFixed(CurrencyDigits));

                        $(this).closest('tr').find("#[id*=txtTaxSplit]").val(curTax.toFixed(CurrencyDigits));
                        $(this).closest('tr').find("#[id*=txtOtherAmountSplit]").val(curOtherAmount.toFixed(CurrencyDigits));

                        $(this).closest('tr').find("#[id*=hdfDedTaxSplitBalance]").val(curTax.toFixed(CurrencyDigits));
                        $(this).closest('tr').find("#[id*=hdfDedOtherChargeSplitBalance]").val(curOtherAmount.toFixed(CurrencyDigits));
                        $("[id$=hdfAllocNowAmount]").val($(this).val());

                        Amount = Amount + parseFloat($(this).val());
                    }
                }
            });
            $("#[id*=grdDeduction] [id*=lblDedTotalAllocateNowFooterSplit]").html(Amount.toFixed(CurrencyDigits));
            $("#[id*=grdDeduction] [id*=hdfDedTotalAllocateNowFooterSplit]").val(Amount);
            $("#[id*=grdDeduction] [id*=hdfOtherTotalFooterSplit]").val(totOtherAmount);
            $("#[id*=grdDeduction] [id*=hdfTaxTotalFooterSplit]").val(totTax);
            $("#[id*=grdDeduction] [id*=lblTaxFooterSplit]").html(totTax.toFixed(CurrencyDigits));
            $("#[id*=grdDeduction] [id*=lblOtherAmountFooterSplit]").html(totOtherAmount.toFixed(CurrencyDigits));
          
        }

        function ShowSaveWithoutAllocationConfirm(btn) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%=GetLocalResourceObject("Msg_Save_Without_Allocation").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $("[id$=hdfSaveWithoutAllocation]").val("1");
                        $(this).dialog("close");                     
                        $("[id$=" + btn + "]").click();
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        if (btn != "btnSubmitInv") {
                            $("[id$=imgHdrDeduction]").click();
                        }
                        return false;
                    }
                }
            });
            return false;
        }

        function SelectAllCheckboxesSpecific(spanChk) {

            var IsChecked = spanChk.checked;

            var Chk = spanChk;
            var txtInvNowValue;

            Parent = $("#[id*=grdInvoice]");
            Parent.find('input:text').each(
             function () {
                 Parent.find("input[type=text][id*=txtInvNow]").val('0.000');
                 Parent.find("input[type=text][id*=hdfInvNow]").val('0.000');

             }
        );
            $("[id$=btnRecalculate]").click();

        }
        //For Setting/Resetting Colour of a selected InvoiceNo
        function SetSelectedRowColor() {
            var selectedIds;
            var selectedIdsArray = new Array();
            selectedIds = $("[id$=hdfSelectedItemPk]").val();
            selectedIdsArray = selectedIds.split(',');

            for (i = 0; i < selectedIdsArray.length; ++i) {

                if (selectedIdsArray[i] != 0) {
                    $("#<%= grdInvoiceList.ClientID %> input[type=hidden][id*=hdfInvoiceID]").each(function (index) {
                        if ($.trim($(this).val()) == selectedIdsArray[i]) {
                            var selectedRowColor;
                            selectedRowColor = '<%= Resources.ErpRes.selectedRowColor %>';
                            $(this).closest('tr').css('background-color', selectedRowColor);
                        }

                    });
                }

            }
        }
        //End

        //For   check  Inv.Now Qty is greater than ordered quantity (orderedqty-Invoiced qty) while savesubmit
        //1-Save,2-Submit
        function ShowInvoiceNowQtyExceeds(val) {

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Msg_InvoiceNowQty_Confirm").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfIsContInvoiceNowQty]").val(1);
                        $(this).dialog("close");
                        if (val == '1') {
                            $("[id$=btnSave]").click();
                        }
                        else if (val == '2') {
                            $("[id$=btnSaveSubmit]").click();
                        }
                    },
                    Cancel: function (e) {
                        $("[id$=hdfIsContInvoiceNowQty]").val(0);
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }

        //End

        //For   check  Inv.Now Qty is greater than grn quantity (grnqty-Invoiced qty) while apply        
        function ShowGRNNowQtyExceeds() {

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Msg_GRNNowQty_Confirm").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfGRNExceed]").val(1);
                        $(this).dialog("close");
                        $("[id$=btnGRNQtyApply]").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfGRNExceed]").val(0);
                        $(this).dialog("close");
                        ShowContainerDiv('[id$=divGRNQtySpilup]', '<%= GetLocalResourceObject("GRNAllocationDetails") %>', '700', '300');
                        return false;
                    }
                }
            });
            return false;
        }

        //For   showing  DuplicateVendorInvNo .do you want to continue or not 
        function ShowDuplicateVendorInvNoContinue(val) {

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Msg_Cont_ConfirmDuplicateVendorInvNo").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfIsContDupVenInvNo]").val(1);
                        $(this).dialog("close");
                        if (val == '1') {
                            $("[id$=btnSave]").click();
                        }
                        else if (val == '2') {
                            $("[id$=btnSaveSubmit]").click();
                        }
                    },
                    Cancel: function (e) {
                        $("[id$=hdfIsContDupVenInvNo]").val(0);
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }

        //End

        function isFloatNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                if (charCode == 46)
                    return true;
                return false;
            }

            return true;
        }
        //TAx Payable in MYR Div hide show

        function ShowHideTaxPayableOuter() {

            if ($("[id$=hdfIsTaxPayable]").val() == 1) {
                $("#divOuterTaxPayable").show();
            }
            else {
                $("#divOuterTaxPayable").hide();
            }
        }


        function HideTaxPayable() {
            //<summary>Function Used to Hide TaxPayable </summary>
            $("#imgHideTaxPayable").hide();
            $("#imgShowTaxPayable").show();
            $("#divTaxPayable").hide();
        }

        function ShowTaxPayable() {
            //<summary>Function Used to Show TaxPayable Panel </summary>
            $("#imgHideTaxPayable").show();
            $("#imgShowTaxPayable").hide();
            $("#divTaxPayable").show();
        }

        function CalculateTotalAmountSplit(sender) {
            var TotalAmountSplit = 0;
            var TotalInvCrAmountSplit = 0;
            $("#[id*=grdPaidAmntSplitup] input[type=hidden][id*=hdfAmountSplit]").each(function (index) {
                if (!isNaN($(this).val()))
                    TotalAmountSplit = TotalAmountSplit + parseFloat($(this).val());
                var InvCrAmount = $(this).closest('tr').find("#[id*=hdfInvCrAmountSplit]").val();
                if (!isNaN(InvCrAmount))
                    TotalInvCrAmountSplit = TotalInvCrAmountSplit + parseFloat(InvCrAmount);
            });
            $("#[id*=grdPaidAmntSplitup] [id*=lblTotalAmountSplit]").html(addCommas(TotalAmountSplit.toFixed(CurrencyDigits)));
            $("#[id*=grdPaidAmntSplitup] [id*=lblTotalInvCrAmountSplit]").html(addCommas(TotalInvCrAmountSplit.toFixed(CurrencyDigits)));
            $("#[id*=lblTotalBalToPay]").html(addCommas((TotalInvCrAmountSplit - TotalAmountSplit).toFixed(CurrencyDigits)));
        }

        //Enable/Disable tax,OtherCharge textbox in Allocation popup

        function EnableDisableTaxOtherCharge() {

            if ($("[id$=hdfIsTaxOCEditable]").val() == 1) {

                $("#[id*=grdDeduction] input[type=text][id*=txtDedAllocateNowSplit]").each(function (index) {

                    $(this).closest('tr').find("#[id*=txtOtherAmountSplit]").attr("disabled", false);
                    $(this).closest('tr').find("#[id*=txtTaxSplit]").attr("disabled", true);
                });
            }
            else {

                $("#[id*=grdDeduction] input[type=text][id*=txtDedAllocateNowSplit]").each(function (index) {

                    $(this).closest('tr').find("#[id*=txtOtherAmountSplit]").attr("disabled", true);
                    $(this).closest('tr').find("#[id*=txtTaxSplit]").attr("disabled", true);
                });
            }
        }

        //Check OtherChargeAllocation in AllocationPopup       
        function CheckAllocationOtherCharge(sender, args) {
            var split = $(sender).parent("td").find('[id*=txtOtherAmountSplit]').val();
            var pattern = new RegExp($(sender).parent("td").find('[id*=vreDedOtherChargeNowSplit]')[0].validationexpression);
            var spliAmount = parseFloat(split);
            if (pattern.test(split) && !isNaN(spliAmount)) {
                var bal = 0;
                var allocate = 0;
                if (!isNaN(parseFloat($(sender).parent("td").find('input[type=hidden][id$=hdfDedOtherChargeSplitBalance]').val()))) {
                    var number = Number($(sender).parent("td").find('input[type=hidden][id$=hdfDedOtherChargeSplitBalance]').val().replace(/[^0-9\.]+/g, ""));
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

        //Check TaxAllocation in AllocationPopup       
        function CheckAllocationTax(sender, args) {
            var split = $(sender).parent("td").find('[id*=txtTaxSplit]').val();
            var pattern = new RegExp($(sender).parent("td").find('[id*=vreDedTaxNowSplit]')[0].validationexpression);
            var spliAmount = parseFloat(split);
            if (pattern.test(split) && !isNaN(spliAmount)) {
                var bal = 0;
                var allocate = 0;
                if (!isNaN(parseFloat($(sender).parent("td").find('input[type=hidden][id$=hdfDedTaxSplitBalance]').val()))) {
                    var number = Number($(sender).parent("td").find('input[type=hidden][id$=hdfDedTaxSplitBalance]').val().replace(/[^0-9\.]+/g, ""));
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

        //Calculate FooterTax and Footer Other amount
        function CalculateTaxOCTotalSplit(sender) {
            var DecimalDigits = 0;
            var Amount = 0;
            var prevOtherAmount = 0;
            var curOtherAmount = 0;
            var prevTax = 0;
            var prevDisc = 0;
            var curTax = 0;
            var curDisc = 0;
            var totOtherAmount = 0;
            var totTax = 0;
            var totDisc = 0;
            var allocateNow = 0;
            var OtherChargeReduced = 0;
            var taxReduced = 0;

            $("#[id*=grdDeduction] input[type=text][id*=txtDedAllocateNowSplit]").each(function (index) {


                allocateNow = parseFloat($("[id$=hdfAllocNowAmount]").val());

                if (!isNaN($(this).closest('tr').find("#[id*=hdfPaidOtherAmount]").val())) {
                    var number = Number($(this).closest('tr').find("#[id*=hdfPaidOtherAmount]").val().replace(/[^0-9\.]+/g, ""));
                    prevOtherAmount = parseFloat(number);
                }
                if (!isNaN($(this).closest('tr').find("#[id*=hdfPaidTax]").val())) {
                    var number = Number($(this).closest('tr').find("#[id*=hdfPaidTax]").val().replace(/[^0-9\.]+/g, ""));
                    prevTax = parseFloat(number);
                }

                if (!isNaN($(this).closest('tr').find("#[id*=txtOtherAmountSplit]").val())) {
                    var number = Number($(this).closest('tr').find("#[id*=txtOtherAmountSplit]").val().replace(/[^0-9\.]+/g, ""));
                    curOtherAmount = parseFloat(number);
                }
                if (!isNaN($(this).closest('tr').find("#[id*=txtTaxSplit]").val())) {
                    var number = Number($(this).closest('tr').find("#[id*=txtTaxSplit]").val().replace(/[^0-9\.]+/g, ""));
                    curTax = parseFloat(number);
                }
                if (!isNaN($(this).closest('tr').find("#[id*=hdfCurPaidDisc]").val())) {

                    var number = Number($(this).closest('tr').find("#[id*=hdfCurPaidDisc]").val().replace(/[^0-9\.]+/g, ""));
                    curDisc = parseFloat(number);
                }
                //               

                totOtherAmount += curOtherAmount;
                totTax += curTax;
                totDisc += curDisc;

                OtherChargeReduced = prevOtherAmount - curOtherAmount;
                taxReduced = prevTax - curTax;
                allocateNow = allocateNow - OtherChargeReduced - taxReduced;


                Amount = Amount + allocateNow;

                $(this).parent("td").find('input[type=hidden][id$=hdfDedAllocateNowSplit]').val(allocateNow);
                $(this).val(allocateNow.toFixed(CurrencyDigits));

                $(this).closest('tr').find("#[id*=hdfCurPaidOtherAmount]").val(curOtherAmount.toFixed(CurrencyDigits));
                $(this).closest('tr').find("#[id*=hdfCurPaidTax]").val(curTax.toFixed(CurrencyDigits));
                $(this).closest('tr').find("#[id*=hdfCurPaidDisc]").val(curDisc.toFixed(CurrencyDigits));
             
            });
            $("#[id*=grdDeduction] [id*=lblDedTotalAllocateNowFooterSplit]").html(Amount.toFixed(CurrencyDigits));
            $("#[id*=grdDeduction] [id*=hdfDedTotalAllocateNowFooterSplit]").val(Amount);
            $("#[id*=grdDeduction] [id*=hdfOtherTotalFooterSplit]").val(totOtherAmount);
            $("#[id*=grdDeduction] [id*=lblOtherAmountFooterSplit]").html(totOtherAmount.toFixed(CurrencyDigits));
            $("#[id*=grdDeduction] [id*=hdfTaxTotalFooterSplit]").val(totTax);
            $("#[id*=grdDeduction] [id*=lblTaxFooterSplit]").html(totTax.toFixed(CurrencyDigits));


        }
        function ValidationCheckOtherCharge(sender, args) {
            var split = $(sender).closest('tr').find('[id*=txtAdjustNowAmount]').val();
            var pattern = new RegExp($(sender).closest('tr').find('[id*=vreOtherCharge]')[0].validationexpression);
            var spliAmount = parseFloat(split);
            if (pattern.test(split) && !isNaN(spliAmount)) {
                var bal = 0;
                var allocate = 0;
                if (!isNaN(parseFloat($(sender).closest('tr').find('[id*=lblBalanceSplit]').html()))) {
                    var number = Number($(sender).closest('tr').find('[id*=lblBalanceSplit]').html().replace(/[^0-9\.]+/g, ""));
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

        function CalculateTotalGRN() {
            var TotalgrnQty = 0;
            $("#[id*=grdGRNDetails] input[type=text][id*=txtInvNowGRNQty]").each(function (index) {
                if ($.trim($(this).val()) != "") {
                    if (!isNaN(parseFloat($(this).val()))) {
                        TotalgrnQty = TotalgrnQty + parseFloat($(this).val().replace(/[^0-9\.]+/g, ""));
                    }
                }
            });
            $("#[id*=grdGRNDetails] [id*=lblTotalGRNInvNow]").html(addCommas(TotalgrnQty.toFixed(NumberDigits)));

        }

        //Calculate Effective Item Rate
        function CalculateEffectiveRate() {
            var RateDecimalDigits = 0;
            var subTotal = 0;
            var totalDiscount = 0;
            var totalAdjustAmount = 0;

            if (!isNaN($("#[id*=grdInvoice] [id*=txtSubTotalFooter]").val())) {
                var number = Number($("#[id*=grdInvoice] [id*=txtSubTotalFooter]").val().replace(/[^0-9\.]+/g, ""));
                subTotal = parseFloat(number);
            }
            if (!isNaN(parseFloat($("#[id*=txtHdrDiscount]").val()))) {
                totalDiscount = parseFloat($("#[id*=txtHdrDiscount]").val());
            }
            if (!isNaN(parseFloat($("#[id*=txtAdjustAmountFooter]").val()))) {
                totalAdjustAmount = parseFloat($("#[id*=txtAdjustAmountFooter]").val());
            }
            if (!isNaN(parseInt($("#[id*=hdfRateDecimalDigits]").val()))) {
                RateDecimalDigits = parseFloat($("#[id*=hdfRateDecimalDigits]").val());
            }
            $("#[id*=grdInvoice] input[type=text][id*=txtRate]").each(function (index) {
                var qty = 0;
                var totalAmount = 0;
                var amountPer = 0;
                var discAmount = 0;
                var adjustAmount = 0;
                var effRate = 0;
                if (totalDiscount > 0 || totalAdjustAmount != 0) {
                    if (!isNaN($(this).closest('tr').find("#[id*=txtTotal]").val())) {
                        var number = Number($(this).closest('tr').find("#[id*=txtTotal]").val().replace(/[^0-9\.]+/g, ""));
                        totalAmount = parseFloat(number);
                    }
                    if (!isNaN($(this).closest('tr').find("#[id*=txtInvNow]").val())) {
                        var number = Number($(this).closest('tr').find("#[id*=txtInvNow]").val().replace(/[^0-9\.]+/g, ""));
                        qty = parseFloat(number);
                    }
                    amountPer = (totalAmount / (subTotal == 0 ? 1 : subTotal));
                    discAmount = amountPer * totalDiscount;
                    adjustAmount = amountPer * totalAdjustAmount;
                    if (qty > 0)
                        effRate = (totalAmount - discAmount + adjustAmount) / qty;
                }
                $(this).closest('tr').find("#[id*=txtEffRate]").val(toFixed(effRate, RateDecimalDigits));
                $(this).closest('tr').find("#[id*=hdfEffRate]").val(toFixed(effRate, RateDecimalDigits));
            });
        }


        function SetEffctiveRate(sender) {
            var effRate = $(sender).closest('tr').find("#[id*=txtEffRate]").val();
            $(sender).closest('tr').find("#[id*=hdfEffRate]").val(effRate);
        }

        function ShowHidePendingPO(flag) {
            ///<summary>
            /// Used to Show/Hide ItemDetails Div
            ///</summary>
            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divPendingPODetails]").show();
                $("[id$=imbShowPendingPO]").hide();
                $("[id$=imbHidePendingPO]").show();
            }
            else {
                $("[id$=divPendingPODetails]").hide();
                $("[id$=imbShowPendingPO]").show();
                $("[id$=imbHidePendingPO]").hide();
            }
            $("[id$=hdfIsPendingPOVisible]").val(flag);
            return false;
        }
        function CalculateTotalOtherCharge() {         
            var TotalOtherAmount = 0;
            $("#[id*=grdOtherchargeSplit] input[type=text][id*=txtAdjustNowAmount]").each(function (index) {    
                if ($.trim($(this).val()) != "") {
                    if (!isNaN(parseFloat($(this).val()))) {
                        TotalOtherAmount = TotalOtherAmount + parseFloat($(this).val());
                    }
                }
            });
            $("#[id*=grdOtherchargeSplit] [id*=lblTotalAdjustNow]").html(addCommas(TotalOtherAmount.toFixed(CurrencyDigits)));
        }

        //set pending po checkbox as checked,when select all check box is checked.Similarly unchecked
        $("[id*=chkSelectAllPO]").live("click", function () {
            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (chkHeader.is(":checked")) {
                    $(this).attr("checked", "checked");
                    $("td", $(this).closest("tr")).addClass("selected");
                } else {
                    if ($(this).is(':disabled') == false) {
                        $(this).removeAttr("checked");
                        $("td", $(this).closest("tr")).removeClass("selected");
                    }
                }


            });
        });

        $("[id*=chkPoPendSelect]").live("click", function () {
            var grid = $(this).closest("table");
            var chkHeader = $("[id*=chkSelectAllPO]", grid);
            if (!$(this).is(":checked")) {
                $("td", $(this).closest("tr")).removeClass("selected");
                chkHeader.removeAttr("checked");
            } else {
                $("td", $(this).closest("tr")).addClass("selected");
                if ($("[id*=chkPoPendSelect]", grid).length == $("[id*=chkPoPendSelect]:checked", grid).length) {
                    chkHeader.attr("checked", "checked");
                }
            }
        });

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
                                    <asp:DropDownList ID="ddlCompany" TabIndex="1" class="select-full-a margnbotm0" runat="server"
                                        onmouseover="javascript:ShowTooltip('ddlCompany');">
                                    </asp:DropDownList>
                                </div>
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="500"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="501"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmitInv" CommandName="SUBMIT" TabIndex="502"
                                            Text="<%$resources:ErpRes,Submit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="503" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteNew" CommandName="DELETE" TabIndex="504"
                                            Text="<%$resources:ErpRes,Delete %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Delete %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" />
                                    </li>
                                    <li id="pnlPrintdt">
                                        <asp:Button runat="server" TabIndex="505" ID="btnPrintdt" CommandName="PRINTDT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnJournalize" CommandName="JOURNALIZE" TabIndex="507"
                                            Text="<%$resources:Journalize %>" OnClick="ActionHandler" ToolTip="<%$resources:Journalize %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-journalize" />
                                    </li>
                                    <li id="pnlAlert" runat="server" style="display:none">
                                        <asp:Button runat="server" ID="btnAlert" CommandName="ALERT" TabIndex="508" Text="<%$resources:Controls,Alert %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:Controls,Alert %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-alert" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="506" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li id="pnlNew">
                                        <asp:Button runat="server" TabIndex="511" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="512" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" OnClientClick="javascript:return SelectedCheckBoxCount(1);" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="513" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            OnClientClick="javascript:return SelectedCheckBoxCount(1);" ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                    <li id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="514" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClientClick="javascript:return SelectedCheckBoxCount(1);" OnClick="ActionHandler"
                                            Text="<%$resources:CancelPI %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-cancel1"
                                            ToolTip="<%$resources:CancelPI %>" />
                                    </li>
                                    <li id="pnlPrint">
                                        <asp:Button runat="server" TabIndex="515" ID="btnPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            OnClientClick="javascript:return SelectedCheckBoxCount(1);" Visible="false" Text="<%$resources:Controls,Print %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <%--  //For SelectedItemId Keeping--%>
                <asp:HiddenField ID="hdfSelectedItemPk" runat="server" Value="0" />
                <div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="INVOICELIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                OnClientClick="javascript:return SelectedCheckBoxCount(1);" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CommandName="INVOICEDETAIL" CssClass="tab-inactive"></asp:LinkButton>
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
                                                TabIndex="65" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="66" />
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
                                            <asp:Label ID="lblFrmDate" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtFromDate"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="100" CssClass="input-small"
                                                MaxLength="17" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtToDate"
                                                CssClass="lbl-9perc"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="101" CssClass="input-small"
                                                MaxLength="17" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" AssociatedControlID="ddlStatus"
                                                CssClass="lbl-9perc"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-a1" TabIndex="102">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,NotPosted %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Posted %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblpoNo" runat="server" Text="<%$resources:PONo %>" AssociatedControlID="txtpoNo"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox ID="txtpoNo" runat="server" CssClass="input-small" MaxLength="100" TabIndex="103"></asp:TextBox>
                                            <asp:Label ID="lblGrnNo" runat="server" Text="<%$resources:GRNNO %>" AssociatedControlID="txtpoNo"
                                                CssClass="lbl-32-2perc"></asp:Label>
                                            <asp:TextBox ID="txtGrnNo" runat="server" CssClass="input-small" MaxLength="100"
                                                TabIndex="104"> </asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCompany" runat="server" Text="<%$ resources:Controls, CompanyPlant %>" AssociatedControlID="ddlCompanySrch"
                                                CssClass="lbl-18-3perc"></asp:Label>
                                            <asp:DropDownList ID="ddlCompanySrch" runat="server" CssClass="select-small-a1" TabIndex="3">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr id="Tr2" runat="server">
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblVendorSearch" runat="server" Text="<%$resources:Customer %>" AssociatedControlID="txtVendorSearch"
                                                CssClass="margnbotm0 middle-lbl"></asp:Label>
                                            <asp:TextBox ID="txtVendorSearch" runat="server" CssClass="input-w47-7per margnbotm0"
                                                MaxLength="100" TabIndex="105"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfVendorSearch" runat="server" />
                                            <asp:Label ID="lblInvoiceNumber" runat="server" Text="<%$resources:InvoiceNo %>"
                                                AssociatedControlID="txtInvoiceNumber" CssClass="lbl-9perc margnbotm0"></asp:Label>
                                            <asp:TextBox ID="txtInvoiceNumber" runat="server" CssClass="input-small margnbotm0"
                                                MaxLength="100" TabIndex="106"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfIVHPK" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label runat="server" ID="lblDue" Text="<%$ resources:DueAson%>" AssociatedControlID="txtDueAson"
                                                CssClass="middle-lbl margnbotm0"></asp:Label>
                                            <asp:TextBox ID="txtDueAson" runat="server" TabIndex="106" CssClass="input-small  margnbotm0"
                                                MaxLength="17" onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:Label runat="server" ID="lblPending" Text="<%$ resources:Pending%>" AssociatedControlID="chkPending"
                                                CssClass="middle-lbl-xsmall-e margnbotm0"></asp:Label>
                                            <asp:CheckBox ID="chkPending" runat="server" Checked="true" />
                                            <asp:Label ID="lblInType" runat="server" Text="<%$resources:InvoiceType %>" AssociatedControlID="ddlOrderType"
                                                CssClass="middle-lbl-small-b margnbotm0"></asp:Label>
                                            <asp:DropDownList ID="ddlOrderType" runat="server" TabIndex="107" CssClass="select-small-a margnbotm0">
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="108" CommandName="SEARCH" SkinID="search-ext"
                                                CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="109" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext"
                                                CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:HiddenField runat="server" ID="hdfCrDrStatus" />
                                <asp:HiddenField ID="hdfCrDrWKFStatus" runat="server" />
                                <asp:GridView runat="server" ID="grdInvoiceList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                    AllowPaging="true" OnPageIndexChanging="ActionHandler" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>                                        
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" CssClass="rdoSelection" TabIndex="10" runat="server" GroupName="SelectOne"
                                                     onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfInvoiceID" Value='<%# Eval("IVH_PK") %>' />
                                                <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval("IVH_DEPT") %>' />
                                                <asp:HiddenField ID="hdfPOType" runat="server" Value='<%# Eval("POH_ITEM_TYPE") %>' />
                                                <asp:HiddenField ID="hdfTaxAmount" runat="server" Value='<%# Eval("IVH_TAX_TC") %>' />
                                                <asp:HiddenField ID="hdfDelStatus" runat="server" Value='<%# Eval("IVH_DEL_STATUS") %>' />
                                                <asp:HiddenField ID="hdfInOpeningInv" runat="server" Value='<%# Eval("IVH_IS_OPENING") %>' />
                                                <asp:HiddenField ID="hdfInvStatus" runat="server" Value='<%# Eval("IVH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceDate %>" SortExpression="IVH_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceDateGd" runat="server" Text='<%#  Eval("IVH_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("IVH_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                    ToolTip='<%# Eval("IVH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceNo %>" SortExpression="IVH_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("IVH_NO") ==""?"[NEW]":Eval("IVH_NO")%>'
                                                    ToolTip='<%# Eval("IVH_NO")%>'></asp:Label>                                               
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCmpName"  Font-Bold="true"  runat="server" Text='<%# Eval("IVH_COMPANY_TEXT")%>' ToolTip='<%# Eval("IVH_COMPANY_TEXT")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceType %>" SortExpression="IVH_TYPE_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_TYPE_TEXT"),3,"")%>'
                                                    ToolTip='<%# Eval("IVH_TYPE_TEXT")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Customer %>" SortExpression="IVH_VENDOR_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendor" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Convert.ToString(Eval("IVH_VENDOR_TEXT")) + " - " + Convert.ToString(Eval("IVH_VENDOR_CODE")),42) %>'
                                                    ToolTip='<%# Convert.ToString(Eval("IVH_VENDOR_TEXT")) + " - " + Convert.ToString(Eval("IVH_VENDOR_CODE")) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfVendorPK" Value='<%# Eval("IVH_VND_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SupplierShortInvNO %>" SortExpression="IVH_VENDOR_INV_NO"
                                            Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSupplierInvNO" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_VENDOR_INV_NO"),10) %>'
                                                    ToolTip='<%# Eval("IVH_VENDOR_INV_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SONo %>" SortExpression="IVH_PO_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSoNo" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_PO_NO"),14)%>'
                                                    ToolTip='<%# Eval("IVH_PO_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GRNNO %>" SortExpression="IVH_GRN_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrnNo" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_GRN_NO"),14)%>'
                                                    ToolTip='<%# Eval("IVH_GRN_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SODate %>" SortExpression="IVH_POH_DT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSoDate" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_POH_DT", Resources.Constants.DateFormatGrid),12)%>'
                                                    ToolTip='<%# Eval("IVH_POH_DT", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency %>" SortExpression="IVH_CURRENCY_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='<%#Eval("IVH_CURRENCY_TEXT")  %>'
                                                    ToolTip='<%#Eval("IVH_CURRENCY_TEXT")  %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfPOCurrency" Value='<%# Eval("IVH_CURRENCY") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Payable %>" SortExpression="IVH_AMOUNT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceValue" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Eval("IVH_AMOUNT")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperation(Eval("IVH_AMOUNT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BalAmt %>" SortExpression="IVH_BAL_AMNT_TC">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalAmt" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Eval("IVH_BAL_AMNT_TC")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperation(Eval("IVH_BAL_AMNT_TC")) %>' Visible='<%# GetBalanceLableVisibility(Eval("IVH_AMOUNT").ToString(),Eval("IVH_BAL_AMNT_TC").ToString())%>'></asp:Label>
                                                <asp:LinkButton ID="lbnBalAmt" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Eval("IVH_BAL_AMNT_TC")) %>'
                                                    CssClass="text-underline" ToolTip='<%# GetFormattedCurrencyWithSeperation(Eval("IVH_BAL_AMNT_TC")) %>' OnClick="ActionHandler"
                                                    CommandName="AMOUNTDETAILS" Visible='<%# GetBalanceLinkVisibility(Eval("IVH_AMOUNT").ToString(),Eval("IVH_BAL_AMNT_TC").ToString())%>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>                                       
                                        <asp:TemplateField HeaderText="<%$ resources:DueDate %>" SortExpression="IVH_DUE_DT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDueDate" runat="server" Text='<%# Eval("IVH_DUE_DT", Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval("IVH_DUE_DT", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("ASC_CSS_CLASS") %>' ToolTip='<%# Eval("IVH_STATUS_TEXT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval("IVH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>                                               
                                                <asp:Button ID="imgPosted" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? GetLocalResourceObject("unposted").ToString() : Eval("FTH_CSS_CLASS")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? Resources.Captions.NotPosted : Eval("FTH_STATUS_TEXT")%>' />
                                                <asp:HiddenField runat="server" ID="hdfPosted" Value='<%# Eval("IVH_HAS_JRNL_ENTRY") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>                            
                            <div class="clear">
                            </div>
                            <table class="table-devide tablelayout" id="Table2">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblInvNo" runat="server" Text="<%$ resources:InvoiceNo%>" AssociatedControlID="lblInvoiceNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblInvoiceNo" CssClass="input-small"></asp:Label>                                           
                                            <asp:HiddenField ID="hdfInvoiceNo" runat="server" />
                                            <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                                            <asp:HiddenField ID="AST_CODE" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblVendorHd" runat="server" Text="<%$resources:Customer %>" AssociatedControlID="txtVendorHd"></asp:Label>
                                            <asp:TextBox ID="txtVendorHd" runat="server" CssClass="input-half" TabIndex="1"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfVendorHd" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfVendor" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" InitialValue="<%$ resources:ErpRes,AutoDefaultValue %>"
                                                runat="server" ControlToValidate="txtVendorHd" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Vendor %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Button ID="btnVendor" runat="server" EnableTheming="false" Style="display: none"
                                                OnClick="ActionHandler" CommandName="VENDORSELECTED" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                <h1>
                                    <%= GetLocalResourceObject("PendingPOHdr").ToString()%></h1>
                                <div class="button-wrap-right ">
                                    <asp:ImageButton runat="server" ID="imbShowPendingPO" OnClientClick="javascript:return ShowHidePendingPO(1);"
                                        SkinID="imbArrowInactive" ToolTip="<%$ resources:Controls,Show %>" />
                                    <asp:ImageButton runat="server" ID="imbHidePendingPO" OnClientClick="javascript:return ShowHidePendingPO();"
                                        Style="display: none" SkinID="imbArrowActive" ToolTip="<%$ resources:Controls,Hide %>" />
                                    <asp:HiddenField ID="hdfIsPendingPOVisible" runat="server" Value="0" />
                                </div>
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <div id="divPendingPODetails" style="display: none">
                                        <div class="gridwrap">
                                            <div id="searchwrap" class="search-wrap-custom1">                                           
                                                <asp:Label ID="lblPndngFrmDate" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtPendingFromDate"></asp:Label>
                                                <asp:TextBox ID="txtPendingFromDate" runat="server" TabIndex="1" Width="100px"
                                                    MaxLength="13" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                                <asp:HiddenField ID="hdfPendingFromDate" runat="server" Value="" />
                                                <asp:Label ID="lblPndngToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtPendingToDate" CssClass="margn-lft30" ></asp:Label>
                                                <asp:TextBox ID="txtPendingToDate" runat="server" TabIndex="2" Width="100px"
                                                    MaxLength="14" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                                <asp:HiddenField ID="hdfPendingToDate" runat="server" Value="" />
                                                      
                                                <asp:Label ID="lblPoNoSr" runat="server" Text="<%$ resources:PurchaseOrderNoHdr %>" CssClass="margn-lft30"
                                                    AssociatedControlID="txtPONumber"></asp:Label>
                                                <div id="divSearchDtls">
                                                    <asp:TextBox ID="txtPONumber" runat="server" TabIndex="2"> </asp:TextBox>
                                                    <asp:HiddenField ID="hdfPoPK" runat="server" Value="" />
                                                </div>
                                                <asp:ImageButton ID="imbDtlSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                    ValidationGroup="Search" OnClick="ActionHandler" TabIndex="3" CommandName="DTLSEARCH"
                                                    SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                                <asp:ImageButton ID="imbDtlClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                    TabIndex="4" OnClick="ActionHandler" CommandName="DTLCLEARSEARCH" SkinID="clear-ext"
                                                    CssClass="margntop2 margnbotm0" />
                                                <div class="clear">
                                                </div>
                                            
                                            </div>
                                              <asp:GridView runat="server" ID="grdPendingPoList" AutoGenerateColumns="False" GridLines="None"
                                                EmptyDataRowStyle-CssClass="emptytable" AllowPaging="false" Width="100%" OnRowDataBound="ActionHandler">
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblInnerEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkSelectAllPO" runat="server" ToolTip="Select All" TabIndex="5" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" ID="chkPoPendSelect" TabIndex="6" />                                                            
                                                            <asp:HiddenField ID="hdfPOPk" runat="server" Value='<%# Eval("POH_PK") %>' />
                                                             <asp:HiddenField runat="server" ID="hdfPOVendorPK" Value='<%# Eval("POH_VENDOR") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfPOCurrency" Value='<%# Eval("POH_CURRENCY") %>' />                                                            
                                                            <asp:HiddenField ID="hdfPOType" runat="server" Value='<%# Eval("POH_TYPE") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="2%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:POdate %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSODate" runat="server" Text='<%# Eval("POH_DATE", Resources.Constants.DateFormatGrid) %>'
                                                                ToolTip='<%# Eval("POH_DATE", Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="8%" HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:PONo %>">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkPONumber" CssClass="text-underline" Text='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("POH_NO")) %>'
                                                                runat="server" OnClick="ActionHandler" CommandName="SHOWPOPUP" CommandArgument='<%# Eval("POH_PK") %>'></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="<%$ resources:VendorType %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblType" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetDecodedString( Eval("POH_TYPE_TEXT")) %>'
                                                                ToolTip='<%#ERP.Utilities.CommonFunctions.GetDecodedString( Eval("POH_TYPE_TEXT")) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="10%" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:Store %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStore" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetDecodedString( Eval("POH_DEPT_TEXT")) %>'
                                                                ToolTip='<%#ERP.Utilities.CommonFunctions.GetDecodedString( Eval("POH_DEPT_TEXT")) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="50%" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:Currency %>" >
                                                         <ItemTemplate>
                                                            <asp:Label ID="lblCurrency" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetDecodedString( Eval("POH_CURRENCY_CODE")) %>'
                                                                ToolTip='<%#Eval("POH_CURRENCY_CODE")  %>'></asp:Label>                                                            
                                                        </ItemTemplate>
                                                        <ItemStyle Width="3%" /> 
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:Amount %>" >
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBalAmt" runat="server" Text='<%# GetFormattedCurrency(Eval("POH_TOTAL_VALUE")) %>'
                                                                ToolTip='<%# GetFormattedCurrency(Eval("POH_TOTAL_VALUE")) %>' ></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="12%" CssClass="amount-numeric" />
                                                        <HeaderStyle CssClass="amount-numeric" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <uc1:PagerControl ID="uclPOPaging" runat="server" />
                                        </div>
                                        <div class="button-wrap-right margntop-minus3 margnbotm10">
                                            <asp:Button ID="btnAddSelectedItems" runat="server" ToolTip="<%$resources:Controls,AddToList %>"
                                                Text="<%$ Resources:Controls, AddToList%>" OnClick="ActionHandler" CommandName="ADDTOLIST"
                                                TabIndex="7" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                            <table class="table-devide tablelayout" id="tblDetailHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
                                            <asp:HiddenField ID="hdfCurrencyDigits" runat="server" Value="3" />
                                            <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                                            <asp:HiddenField ID="hdfDecimalFormatWithSeperation" runat="server" />
                                            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                                            <asp:HiddenField ID="hdfCurrencyFormatWithSeperation" runat="server" />
                                            <asp:HiddenField ID="hdfRateFormat" runat="server" />
                                            <asp:HiddenField ID="hdfInvoicePK" runat="server" />
                                            <asp:HiddenField ID="hdfExchangeRate" runat="server" />
                                            <asp:HiddenField ID="hdfTaxCategory" runat="server" />
                                            <asp:HiddenField ID="hdfTaxSettings" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdfPOItemType" runat="server" /> 
                                            <asp:Label runat="server" ID="lblInvoiceDate" Text="<%$ resources:InvoiceDate%>"
                                                AssociatedControlID="txtInvoiceDate" ></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInvoiceDate" CssClass="input-small" TabIndex="1"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" ></asp:TextBox>
                                            <asp:HiddenField ID="hdfHasTax" runat="server" Value="0" />
                                            <asp:RequiredFieldValidator ID="vrfInvoiceDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtInvoiceDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_InvoiceDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="vrfTaxDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="taxDate" EnableClientScript="true" runat="server" ControlToValidate="txtInvoiceDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_InvoiceDate %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblTransport" Text="<%$ resources:Transport%>" AssociatedControlID="txtTransport"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTransport" TabIndex="4" MaxLength="100" CssClass="input-half"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblAddressType" Text="<%$ resources:Type%>" AssociatedControlID="ddlAddressType"></asp:Label>                                           
                                            <asp:DropDownList ID="ddlAddressType" runat="server" TabIndex="7" OnSelectedIndexChanged="ActionHandler"
                                                AutoPostBack="true" CssClass="select-small-g margnrgt1-5per">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfAddressType" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="vatbuy" EnableClientScript="true" InitialValue="-1" runat="server"
                                                ControlToValidate="ddlAddressType" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Type %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtBranchCode" runat="server" CssClass="input-small" MaxLength="5"
                                                TabIndex="8" />
                                            <asp:RequiredFieldValidator ID="vrfBranchCode" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="vatbuy" EnableClientScript="true" runat="server" ControlToValidate="txtBranchCode"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_BranchCode %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <div id="divVendorBranch" style="display: none" runat="server">
                                                <asp:Label runat="server" ID="lblVendorBranch" Visible="false" Text="<%$ resources:Branch%>"
                                                    AssociatedControlID="ddlVendorBranch"></asp:Label>
                                                <asp:DropDownList ID="ddlVendorBranch" Visible="false" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                            <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks %>" AssociatedControlID="txtRemarks"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRemarks" TabIndex="10" TextMode="MultiLine" CssClass="input-half"
                                                onkeydown="limitText(this,500);" onchange="limitText(this,500);"></asp:TextBox>                                            
                                            <div class="clear">
                                            </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblSupplierInvNO" Text="<%$ resources:SupplierInvNO%>"
                                                AssociatedControlID="txtSupplierInvNO"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSupplierInvNO" CssClass="input-small" TabIndex="2"
                                                MaxLength="100"></asp:TextBox>
                                                  <div class="starwrap">
                                            <asp:RequiredFieldValidator ID="vrfSupplierInvNO" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtSupplierInvNO"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_SupplierInvNO %>">
                                            </asp:RequiredFieldValidator>
                                            </div>
                                            <asp:Label runat="server" ID="lblSupplierInvDate" Text="<%$ resources:SupplierInvDate%>"
                                                AssociatedControlID="txtInvoiceDueDate" CssClass="middle-lbl-small-e"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSupplierInvDate" CssClass="input-small" TabIndex="3" onchange="CalculateDueDate();"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" ></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfSupplierInvDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtSupplierInvDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_SupplierInvDate %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblCreditDays" Text="<%$ resources:CreditDays%>" AssociatedControlID="txtCreditDays"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtCreditDays" TabIndex="5" MaxLength="3" CssClass="input-small numeric"
                                                onchange="CalculateDueDate();"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="vreCreditDays" runat="server" ControlToValidate="txtCreditDays"
                                                ErrorMessage="<%$ resources:Err_CreditDays %>" ValidationExpression="^\$?([0-9]{0,10})?$"
                                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice">
                                            </asp:RegularExpressionValidator>
                                            <asp:Label runat="server" ID="lblInvoiceDueDate" Text="<%$ resources:InvoiceDueDate%>"
                                                AssociatedControlID="txtInvoiceDueDate" CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInvoiceDueDate" CssClass="input-small" TabIndex="6"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" onchange="CalculateDueDays();"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfInvoiceDueDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtInvoiceDueDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_InvoiceDueDate %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblVatBuyTaxId" Text="<%$ resources:Taxid%>" AssociatedControlID="txtVatTaxId"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtVatTaxId" TabIndex="9" CssClass="input-half"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblInvType" runat="server" Text="<%$ resources:InvoiceType%>" AssociatedControlID="txtInvoiceType"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInvoiceType" CssClass="input-small input-disabled"
                                                TabIndex="8" Enabled="false"></asp:TextBox>
                                            <asp:Label ID="lblOriginal" runat="server" Text="<%$ resources:OriginalInvReceived%>"
                                                AssociatedControlID="chkOriginalinvoice" CssClass="middle-lbl-small-e1"></asp:Label>
                                            <asp:CheckBox ID="chkOriginalinvoice" TabIndex="11" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblDeclarationNO" Text="<%$ resources:DeclarationNo%>"
                                                AssociatedControlID="txtDeclarationNO"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDeclarationNO" CssClass="input-small" TabIndex="12"
                                                MaxLength="80"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                             <asp:Label runat="server" ID="lblCurrency" Text="<%$ resources:Currency%>" AssociatedControlID="txtCurrency"></asp:Label>
                                            <asp:TextBox ID="txtCurrency" runat="server" CssClass="input-small input-disabled" TabIndex="14"
                                                MaxLength="100" Enabled="false"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfCurrency" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" InitialValue="<%$ resources:ErpRes,AutoDefaultValue %>"
                                                runat="server" ControlToValidate="txtCurrency" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_Currency %>">
                                            </asp:RequiredFieldValidator>
                                             <asp:HiddenField ID="hdfCurrency" runat="server" />            
                                            <asp:Label ID="lblExchangeRate" runat="server" Text="<%$ resources:Controls,ExchangeRate %>" CssClass="middle-lbl-a0"
                                                AssociatedControlID="txtExchangeRate" />
                                            <asp:TextBox ID="txtExchangeRate" runat="server" TabIndex="13" MaxLength="20" CssClass="input-small numeric"
                                                AutoPostBack="true" OnTextChanged="ActionHandler" onkeypress="return isFloatNumberKey(event);" />
                                         </div>
                                    </td>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">                                                
                                            </div>
                                        </td>
                                    </tr>
                                </tr>
                            </table>
                            <div class="gridwrap scroll-container">
                                <asp:Button ID="btnRecalculate" runat="server" EnableTheming="false" Style="display: none"
                                    OnClick="ActionHandler" CommandName="RECALCULATE" />
                                <asp:GridView ID="grdInvoice" runat="server" AutoGenerateColumns="False" Width="1415px"
                                    AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                    ShowFooter="true" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Item %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfPOPK" runat="server" Value='<%#Eval("VID_PO") %>' />
                                                <asp:HiddenField ID="hdfInvoiceDtlPK" runat="server" Value='<%#Eval("VID_PK") %>' />
                                                <asp:HiddenField ID="hdfInvoiceDtlDummyPK" runat="server" Value='<%#Eval("VID_SL_UK") %>' />
                                                <asp:HiddenField ID="hdfItemPK" runat="server" Value='<%#Eval("VID_ITEM") %>' />
                                                <asp:HiddenField ID="hdfPODetailPK" runat="server" Value='<%#Eval("VID_PO_DTL") %>' />
                                                <asp:Label ID="lblItem" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("VID_ITEM_TEXT"),45) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("VID_ITEM_TEXT").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="40%" />
                                            <HeaderStyle Width="40%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Rate %>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRate" runat="server" Text='<%#GetFormattedRate(Eval("VID_RATE")) %>'
                                                    CssClass="input-w58 numeric input-disabled" MaxLength="14" ToolTip='<%#GetFormattedRate(Eval("VID_RATE")) %>'
                                                    OnTextChanged="ActionHandler" Enabled="false" AutoPostBack="true"></asp:TextBox>
                                                <asp:HiddenField ID="hdfRate" runat="server" Value='<%#Eval("VID_RATE") %>' />
                                                <asp:RequiredFieldValidator ID="vrfRate" CssClass="star" SetFocusOnError="true" ValidationGroup="invoice"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtRate" Display="Dynamic"
                                                    Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:HiddenField ID="hdfEffRate" runat="server" Value='<%# GetFormattedRate(Eval("VID_RATE_EFCT")) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" HorizontalAlign="Right" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:EffRate %>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEffRate" runat="server" Text='<%#GetFormattedRate(Eval("VID_RATE_EFCT")) %>'
                                                    onblur="javascript:SetEffctiveRate(this);" CssClass="input-w58 numeric" MaxLength="14"
                                                    ToolTip='<%#GetFormattedRate(Eval("VID_RATE_EFCT")) %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" HorizontalAlign="Right" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:UoM %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUoM" runat="server" Text='<%#Eval("VID_UOM_TEXT") %>' ToolTip='<%# HttpUtility.HtmlDecode(Eval("VID_UOM_TEXT").ToString()) %>'></asp:Label>
                                                <asp:HiddenField ID="hdfUoM" runat="server" Value='<%#Eval("VID_UOM") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                            <HeaderStyle Width="6%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:OrderQuantity%>">
                                            <ItemTemplate>                                               
                                                <asp:Label ID="lblOrderQuantity" runat="server" CssClass="ItemQuantity" Text='<%#GetFormattedNumberWithSeperation(Eval("VID_ORDERED_QTY")) %>'
                                                    ToolTip='<%#GetFormattedNumberWithSeperation(Eval("VID_ORDERED_QTY")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvQuantity%>">
                                            <ItemTemplate>                                                
                                                <asp:Label ID="lblInvQuantity" runat="server" CssClass="ItemQuantity" Text='<%#GetFormattedNumberWithSeperation(Eval("VID_INV_QTY")) %>'
                                                    ToolTip='<%#GetFormattedNumberWithSeperation(Eval("VID_INV_QTY")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvNow%>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtInvNow" runat="server" Text='<%#GetFormattedNumber(Eval("VID_QTY_INVOICED")) %>'
                                                    CssClass='<%# Eval("VID_HAS_GRN").ToString() == "0" ? "input-w58 numeric" : "input-w58 numeric input-disabled" %>'
                                                    Enabled='<%# Eval("VID_HAS_GRN").ToString() == "0" ? true : false %>' MaxLength="13"
                                                    OnTextChanged="ActionHandler" AutoPostBack="true" TabIndex="15"></asp:TextBox>
                                                <asp:HiddenField ID="hdfInvNow" runat="server" />
                                                <asp:RequiredFieldValidator ID="vrfInvNow" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtInvNow"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                </asp:RequiredFieldValidator>
                                                <cc1:QuantityValidationP2P ID="vreInvNow" runat="server" ControlToValidate="txtInvNow"
                                                    NumberDigits="9" ErrorMessage="<%$ resources:Err_Invalid_InvNow %>" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"></cc1:QuantityValidationP2P>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" Width="20%" />
                                        </asp:TemplateField>
                                        <%--       Reset Check box field start  -------------------%>
                                        <asp:TemplateField HeaderText="<%$ resources:ResetInvQty%>">
                                            <HeaderTemplate>
                                                <img id="imgReset" alt="Reset" runat="server" style="cursor: pointer" src="~/images/Classic/Icons/refresh.png"
                                                    title="<%$ resources:ResetInvNow%>" onclick="javascript:SelectAllCheckboxesSpecific(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="float: left;">
                                                    <asp:ImageButton ID="imgGRN" runat="server" CommandName="GRNDETAILS" OnClick="ActionHandler"
                                                        ToolTip="<%$ resources:Controls,grnAllocation %>" Visible='<%# Eval("VID_HAS_GRN").ToString() == "0" ? false : true %>'
                                                        SkinID="grn-icon" />
                                                </div>
                                            </ItemTemplate>
                                            <HeaderStyle />
                                        </asp:TemplateField>
                                        <%--    End --------------------------------------------------%>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("VID_AMOUNT")) %>'
                                                    CssClass="input-w70 numeric input-disabled" MaxLength="15" Enabled="false" ToolTip='<%#GetFormattedCurrency(Eval("VID_AMOUNT")) %>'></asp:TextBox>
                                                <asp:HiddenField ID="hdfAmount" runat="server" Value='<%#Eval("VID_AMOUNT") %>' />
                                                <asp:RequiredFieldValidator ID="vrfAmount" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtAmount"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                </asp:RequiredFieldValidator>
                                                <cc1:AmountValidation ID="vamAmount" runat="server" ControlToValidate="txtAmount"
                                                    ErrorMessage="<%$ resources:Err_Invalid_Amount %>" NumberDigits="11" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"></cc1:AmountValidation>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Discount %>">
                                            <ItemTemplate>
                                                <div style="float: left;">
                                                    <asp:ImageButton ID="imgDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                        ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')"
                                                        ToolTip="<%$ resources:Controls,Discounts %>" CommandName="DISCDETAILS" />
                                                </div>
                                                <div runat="server" id="divDiscount"> 
                                                    <asp:TextBox ID="txtDiscount" runat="server" Text='<%#GetFormattedCurrency(Eval("VID_DISCOUNT")) %>'
                                                        CssClass="input-w70 numeric input-disabled" MaxLength="15" ToolTip='<%#GetFormattedCurrency(Eval("VID_DISCOUNT")) %>'
                                                        Enabled="false"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfDiscount" runat="server" />
                                                    <asp:RequiredFieldValidator ID="vrfDiscount" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtDiscount"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Discount %>">
                                                    </asp:RequiredFieldValidator>
                                                    <cc1:AmountValidation ID="vamDiscount" runat="server" ControlToValidate="txtDiscount"
                                                        ErrorMessage="<%$ resources:Err_Invliad_Discount %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"></cc1:AmountValidation>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Tax %>">
                                            <ItemTemplate>
                                                <div style="float: left;">
                                                    <asp:ImageButton ID="imgTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                        ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')"
                                                        ToolTip="<%$ resources:Tax %>" CommandName="TAXDETAILS" />
                                                </div>
                                                <div runat="server" id="divTax">
                                                    <asp:TextBox ID="txtTax" runat="server" Text='<%#GetFormattedCurrency(Eval("VID_TAX")) %>'
                                                        CssClass="input-w70 numeric input-disabled" MaxLength="15" ToolTip='<%#GetFormattedCurrency(Eval("VID_TAX")) %>'
                                                        Enabled="false"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfTax" runat="server" />
                                                    <asp:RequiredFieldValidator ID="vrfTax" CssClass="star" SetFocusOnError="true" ValidationGroup="invoice"
                                                        EnableClientScript="true" runat="server" ControlToValidate="txtTax" Display="Dynamic"
                                                        Text="*" ErrorMessage="<%$ resources:Err_Tax %>">
                                                    </asp:RequiredFieldValidator>
                                                    <cc1:AmountValidation ID="vamTax" runat="server" ControlToValidate="txtTax" ErrorMessage="<%$ resources:Err_Invliad_Tax %>"
                                                        NumberDigits="11" Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star"
                                                        ValidationGroup="invoice"></cc1:AmountValidation>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" Width="8%" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblfooter" Text="<%$ resources:SubTotal %>"></asp:Label></FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GrandTotal %>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTotal" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    MaxLength="15" Text='<%#GetFormattedCurrency(Eval("VID_NET_AMOUNT")) %>' Enabled="false"></asp:TextBox>
                                                <asp:HiddenField ID="hdfTotal" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" Width="10%" />
                                            <FooterStyle CssClass="amount-numeric" Width="10%" />
                                            <FooterTemplate>
                                                <asp:TextBox runat="server" ID="txtSubTotalFooter" CssClass="input-w80 numeric input-disabled"
                                                    Enabled="false" AutoPostBack="true"></asp:TextBox></FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:SONo %>">
                                            <ItemTemplate>                                               
                                                <asp:LinkButton ID="lnkPoNo" runat="server" Text='<%# Eval("VID_PO_NO") %>' CssClass="text-underline"
                                                    ToolTip='<%# Eval("VID_PO_NO")%>' OnClick="ActionHandler" CommandName="PRINTLINEITEMPO"
                                                    Style="width: 100px;"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="amount-numeric" Width="25%" />
                                            <HeaderStyle CssClass="amount-numeric" Width="25%" />
                                            <FooterStyle CssClass="amount-numeric" Width="25%" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblAdjustAmount" Text="<%$ resources:AdjustAmount %>"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:PRNo %>">
                                            <ItemTemplate>                                              
                                                <asp:LinkButton ID="lnkPRNo" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("VID_PR_NO"),12) %>'
                                                    ToolTip='<%# Eval("VID_PR_NO")%>' OnClick="ActionHandler" CommandName="PRINTLINEITEMPR"
                                                    Style="width: 100px;"></asp:LinkButton>
                                                <asp:HiddenField ID="hdfPRPKs" runat="server" Value='<%#Eval("VID_PR_PKS")%>' />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="amount-numeric" Width="40%" />
                                            <HeaderStyle CssClass="amount-numeric" Width="40%" />
                                            <FooterStyle CssClass="amount-numeric" Width="40%" />
                                            <FooterTemplate>
                                                <asp:TextBox runat="server" ID="txtAdjustAmountFooter" CssClass="input-w80 numeric"
                                                    OnTextChanged="ActionHandler" MaxLength="12" AutoPostBack="true"></asp:TextBox></FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div id="divCalc">
                                <div class="gridwrap">
                                    <table id="tblCalc" class="gridwraptable gridwrap">
                                        <tr>
                                            <td style="text-align: right; width: 85.5%;">
                                                <asp:Label runat="server" ID="lblDiscount" Text="<%$ resources:Discount%>" AssociatedControlID="txtHdrDiscount"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgHdrDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                    ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')"
                                                    ToolTip="<%$ resources:Controls,Discounts %>" CommandName="DISCHEADER" TabIndex="16" />
                                                <asp:TextBox ID="txtHdrDiscount" runat="server" TabIndex="17" CssClass="input-w80 numeric input-disabled"
                                                    MaxLength="16" Enabled="false"></asp:TextBox>
                                             <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="vrfDiscount" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtHdrDiscount"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TotalDiscount%>"></asp:RequiredFieldValidator>
                                              </div>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="Label1" Text="<%$ resources:GrossTotal%>" AssociatedControlID="txtHdrTotal"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtHdrTotal" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    TabIndex="18" MaxLength="16" Enabled="false"></asp:TextBox>
                                              <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="vrfHdrTotal" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtHdrTotal"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Gross_Total%>"></asp:RequiredFieldValidator>
                                              </div>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="trDeductionTax" runat="server">
                                            <td style="text-align: right; width: 85%;">
                                                <asp:Label runat="server" ID="lblDeduction" Text="<%$ resources:Deduction%>" AssociatedControlID="txtHdrDeduction"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgHdrDeduction" SkinID="allocation" runat="server" OnClick="ActionHandler"
                                                    TabIndex="19" ToolTip="Deduction" CommandName="DEDUCTIONHEADER" />
                                                <asp:TextBox ID="txtHdrDeduction" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    MaxLength="16" Enabled="false"></asp:TextBox>
                                               <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="vrfDeduction" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtHdrDeduction"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TotalDeduction%>"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="trDiscDeductedTax" runat="server">
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblDiscDeducted" Text="<%$ resources:DedDisc%>" AssociatedControlID="txtDiscDeducted"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtDiscDeducted" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    TabIndex="20" MaxLength="16" Text="0.00" Enabled="false"></asp:TextBox>
                                                 <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtHdrBalBeforeVat"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_BalBeforeDisc%>"></asp:RequiredFieldValidator>
                                                  </div>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="Label2" Text="<%$ resources:Balance_Before_Tax%>" AssociatedControlID="txtHdrBalBeforeVat"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtHdrBalBeforeVat" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    TabIndex="21" MaxLength="16" Enabled="false"></asp:TextBox>
                                                 <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="vrfHdrBalBeforeVat" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtHdrBalBeforeVat"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_BalBeforeVat%>"></asp:RequiredFieldValidator>
                                                    </div>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <%--   OtherCharge start--%>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblShipping" Text="<%$ resources:Shipping%>" AssociatedControlID="txtShipping"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:HiddenField ID="hdfOtherchargePO" Value='' runat="server" />
                                                <asp:ImageButton ID="imgOtherCharge" SkinID="shipping" runat="server" OnClick="ActionHandler"
                                                    ToolTip="<%$ resources:Shipping %>" CommandName="OTHERCHARGEHEADER" TabIndex="22" />
                                                <asp:TextBox ID="txtShipping" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    TabIndex="23" onchange="CalculateTotal(this);" MaxLength="16"></asp:TextBox>
                                                 <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="vrfShipping" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtShipping"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Shipping%>"></asp:RequiredFieldValidator>
                                                 </div> 
                                                <cc1:AmountValidation ID="vamShipping" runat="server" ControlToValidate="txtShipping"
                                                    ErrorMessage="<%$ resources:Err_Invalid_Shipping %>" NumberDigits="12" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"></cc1:AmountValidation>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <%-- End OtherCharge--%>
                                        <%-- Other Charge Deduction Start--%>
                                        <tr id="trDeductedOtherChargesTax" runat="server">
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblDeductOtherCharges" Text="<%$ resources:DeductionOtherCharges%>"
                                                    AssociatedControlID="txtHdrNetTotal"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtDeductOtherCharges" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    TabIndex="24" Text="0.00" Enabled="false" MaxLength="16"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <%--   End Other Charge Deduction--%>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblTax" Text="<%$ resources:Tax%>" AssociatedControlID="txtHdrTax"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgHdrTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                    ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')"
                                                    ToolTip="<%$ resources:Tax %>" CommandName="TAXHEADER" TabIndex="25" />
                                                <asp:TextBox ID="txtHdrTax" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    TabIndex="26" Enabled="false" MaxLength="16"></asp:TextBox>
                                          <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="vrfTax" CssClass="star" SetFocusOnError="true" ValidationGroup="invoice"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtHdrTax" Display="Dynamic"
                                                    Text="*" ErrorMessage="<%$ resources:Err_TotalTax%>"></asp:RequiredFieldValidator>
                                           </div>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblPriceAdj" Text="<%$ resources:AmountAdj%>" AssociatedControlID="txtPriceAdj"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtPriceAdj" runat="server" CssClass="input-w80 numeric" TabIndex="27"
                                                    onchange="CalculateTotal(this);" MaxLength="16"></asp:TextBox>
                                                     <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="vrfPriceAdj" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtPriceAdj"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PriceAdj%>"></asp:RequiredFieldValidator>
                                                    </div> 
                                                <cc1:AmountValidation ID="vamPriceAdj" runat="server" ControlToValidate="txtPriceAdj"
                                                    ErrorMessage="<%$ resources:Err_Invalid_PriceAdj %>" NumberDigits="12" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"
                                                    AllowNegative="true"></cc1:AmountValidation>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <%--FOR BWH Start--%>
                                        <tr id="trTotal" runat="server" visible="false">
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="Label3" Text="<%$ resources:GrandTotal%>" AssociatedControlID="txtTotal"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtTotal" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    TabIndex="28" MaxLength="16" Enabled="false"></asp:TextBox>                                               
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="trTotalDeduction" runat="server" visible="false">
                                            <td style="text-align: right; width: 85%;">
                                                <asp:Label runat="server" ID="Label4" Text="<%$ resources:Deduction%>" AssociatedControlID="txtTotalDeduction"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="ImgTotDed" SkinID="allocation" runat="server" OnClick="ActionHandler"
                                                    TabIndex="29" ToolTip="Deduction" CommandName="DEDUCTIONHEADER" />
                                                <asp:TextBox ID="txtTotalDeduction" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    MaxLength="16" Enabled="false"></asp:TextBox>
                                                     <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtTotalDeduction"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TotalDeduction%>"></asp:RequiredFieldValidator>
                                                    </div>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <%-- FOR BWH End --%>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblTotal" Text="<%$ resources:Net_Payable%>" AssociatedControlID="txtHdrNetTotal"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtHdrNetTotal" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    TabIndex="30" Enabled="false" MaxLength="16"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="divcol-S">
                                        <asp:Label ID="lblFileUpload" runat="server" Text="AttachFile" AssociatedControlID="fupUpload"></asp:Label>
                                        <asp:FileUpload ID="fupUpload" runat="server" TabIndex="31" Style="width: 15.6%;" />
                                        <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupUpload"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_File_Upload %>">
                                                        
                                        </asp:RequiredFieldValidator>
                                        <a id="anchorFile" runat="server" target="_blank"></a>
                                        <asp:Button runat="server" ID="btnAddItem" CommandName="ADDITEM" TabIndex="32" OnClick="ActionHandler"
                                            OnClientClick="javascript:ValidatePageNow('upload')" ToolTip="<%$resources:ErpRes,Add %>"
                                            CommandArgument="PageAction_Entry" ValidationGroup="upload" Text="<%$resources:ErpRes,Add %>"
                                            SkinID="btnInner-add" />                                        
                                        <div class="clear">
                                        </div>
                                    </div>
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdUploads" Width="100%" PageSize="<%$ resources:PageSize%>"
                                            AllowSorting="false" AllowPaging="false" OnSorting="ActionHandler" OnPageIndexChanging="ActionHandler"
                                            OnRowDataBound="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:SlNo %>">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>                                                       
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
                                                            target="_blank" href='<%# Page.ResolveClientUrl(Eval("DOC_PATH").ToString()) %>'>
                                                        </a>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="3%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="lnkEdit" runat="server" OnClick="ActionHandler" CommandName="EDITITEM"
                                                            SkinID="edit-icon" ToolTip="Edit" CommandArgument="PageAction_Entry" OnLoad="btnAction_Load"
                                                            OnPreRender="btnAction_PreRender" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="3%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                                                            TabIndex="33" SkinID="delete-icon" ToolTip="Delete" CommandArgument="PageAction_Entry"
                                                            OnClientClick="return ShowDeleteConfirm(this);" OnLoad="btnAction_Load" OnPreRender="btnAction_PreRender" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="3%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <div id="divItemTax" style="display: none">
                                <div class="Button-container-popup">
                                    <asp:Button ID="btnApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply" OnClick="ActionHandler"
                                        CommandArgument="PageAction_Entry" CommandName="TAXAPPLY" />
                                </div>
                                <div class="content-wrapper">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:HiddenField ID="hdfTaxFormula" runat="server" />
                                                    <asp:Label ID="lblPopupItemAmount" runat="server" Text="<%$ resources:ItemAmount %>"
                                                        AssociatedControlID="txtPopupItemAmount"></asp:Label>
                                                    <asp:TextBox ID="txtPopupItemAmount" CssClass="input-w70 numeric" runat="server"
                                                        EnableViewState="false" Enabled="false" MaxLength="11"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblPopupAmount" runat="server" Text="<%$ resources:Amount %>" AssociatedControlID="txtPopupAmount"></asp:Label>
                                                    <asp:TextBox ID="txtPopupAmount" TabIndex="20" runat="server" CssClass="input-w70 numeric"
                                                        EnableViewState="false" MaxLength="11"></asp:TextBox><%--Enabled="false"--%>
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfTaxAmt" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="tax" EnableClientScript="true" runat="server" ControlToValidate="txtPopupAmount"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                        </asp:RequiredFieldValidator>
                                                        <cc1:AmountValidation ID="vamTaxAmt" runat="server" ControlToValidate="txtPopupAmount"
                                                            ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="tax"></cc1:AmountValidation>
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:Label ID="lblPopupTaxType" runat="server" Text="<%$ resources:TaxType %>" AssociatedControlID="ddlPopupTaxType"></asp:Label>
                                                    <asp:DropDownList ID="ddlPopupTaxType" TabIndex="19" runat="server" CssClass="medium"
                                                        EnableViewState="true" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblPopupOther" runat="server" Text="<%$ resources:TaxName %>" AssociatedControlID="txtPopupOther"></asp:Label>
                                                    <asp:TextBox ID="txtPopupOther" runat="server" TabIndex="21" CssClass="medium" EnableViewState="false"
                                                        MaxLength="100" Enabled="false"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfPopupOther" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="tax" EnableClientScript="true" runat="server" ControlToValidate="txtPopupOther"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TaxName %>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:ImageButton ID="imgPopupAdd" SkinID="imbaddnew" runat="server" OnClick="ActionHandler"
                                                        CommandArgument="PageAction_Entry" ValidationGroup="tax" ToolTip="Add" CommandName="TAXADD"
                                                        OnClientClick="javascript:ValidatePageNow('tax')" />
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdTaxDetails" Width="100%" AllowSorting="false"
                                            AutoGenerateColumns="false" TabIndex="22" EmptyDataRowStyle-CssClass="emptytable">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:TaxType %>">
                                                    <ItemTemplate>                                                       
                                                        <asp:HiddenField ID="hdfTaxSplitPK" runat="server" Value='<%#Eval("VTL_PK") %>' />
                                                        <asp:HiddenField ID="hdfTaxPK" runat="server" Value='<%#Eval("VTL_TAX") %>' />                                                       
                                                        <asp:Label ID="lblTaxText" runat="server" Text='<%# Convert.ToString(Eval("VTL_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("VTL_TAX_TEXT")) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("VTL_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("VTL_TAX_TEXT"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:TaxName %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxName" runat="server" Text='<%#Eval("VTL_NAME") %>' ToolTip='<%# HttpUtility.HtmlDecode(Eval("VTL_NAME").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTaxName" runat="server" Value='<%#Eval("VTL_NAME") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Amount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrencyWithSeperation(Eval("VTL_TAX_AMT")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithSeperation(Eval("VTL_TAX_AMT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="22%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imbTaxRemove" runat="server" OnClick="ActionHandler" CommandName="TAXDELETE"
                                                            CommandArgument="PageAction_Entry" OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load"
                                                            SkinID="btnclose" ToolTip="Remove" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="8%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <div id="divDeduction" style="display: none">
                                <div class="Button-container-popup">
                                    <asp:Button ID="btnSaveDuduction" SkinID="btnInner-add-dsd" runat="server" Text="Apply"
                                        OnClick="ActionHandler" TabIndex="54" CommandName="DEDUCTIONAPPLY" CommandArgument="PageAction_Entry"
                                        ValidationGroup="deduction" OnClientClick="javascript:ValidatePageNow('deduction')" />
                                </div>
                                <div class="content-wrapper">
                                    <div class="detail-poi-co2">                                       
                                        <div class="divfirstcol-S">                                           
                                            <asp:Label ID="Label8" runat="server" Text="<%$ resources:Vendor1 %>" AssociatedControlID="lblDedCustomer"
                                                Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblDedCustomer" runat="server"></asp:Label>
                                        </div>
                                        <div class="divseccol-S">                                           
                                            <asp:Label ID="Label6" runat="server" Text="<%$ resources:Currency1 %>" AssociatedControlID="lblDedCurrency"
                                                Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblDedCurrency" runat="server"></asp:Label>
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </div>
                                    <div class="gridwrap">
                                        <asp:GridView ID="grdDeduction" runat="server" AutoGenerateColumns="False" Width="100%"
                                            PageSize="<%$ resources:PageSize %>" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable"
                                            AllowSorting="false" ShowFooter="true" OnRowDataBound="ActionHandler">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:InvoiceNo2 %>">
                                                    <ItemTemplate>                                                       
                                                        <asp:LinkButton ID="lnkDedInvoiceNo" runat="server" Text='<%# Eval("IVH_NO") %>'
                                                            CssClass="text-underline" ToolTip='<%# Eval("IVH_NO")%>' OnClick="ActionHandler"
                                                            CommandName="PRINTDEDINVOICE"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:InvoiceDate %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfDedPOHdr" runat="server" Value='<%#Eval("IVM_PO_HDR") %>' />
                                                        <asp:HiddenField ID="hdfDeductionPK" runat="server" Value='<%#Eval("VAD_PK") %>' />
                                                        <asp:HiddenField ID="hdfAdvInvoicePK" runat="server" Value='<%#Eval("VAD_INVOICE_ADV") %>' />
                                                        <asp:Label ID="lblDedInvoiceDate" runat="server" Text='<%#Eval("IVH_DATE") %>' ToolTip='<%#Eval("IVH_DATE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:PaymentNo %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDedPaymentNo" runat="server" Text='<%#Eval("PVH_NO") %>' ToolTip='<%#Eval("PVH_NO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:PaymentDate %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfPaymentPK" runat="server" Value='<%#Eval("VAD_PAYMENT_HDR") %>' />
                                                        <asp:HiddenField ID="hdfPaymentDTLPK" runat="server" Value='<%#Eval("PVM_PK") %>' />
                                                        <asp:HiddenField ID="hdfPaidOtherAmount" runat="server" Value='<%#Eval("PVH_PO_PAID_OTHER_AMT") %>' />
                                                        <asp:HiddenField ID="hdfPaidTax" runat="server" Value='<%#Eval("PVH_PO_PAID_TAX_AMT") %>' />
                                                        <asp:HiddenField ID="hdfpaidDisc" runat="server" Value='<%#Eval("IVM_DISCOUNT_AMOUNT") %>' />
                                                        <asp:HiddenField ID="hdfCurPaidOtherAmount" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hdfCurPaidTax" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hdfCurPaidDisc" runat="server" Value="0" />
                                                        <asp:Label ID="lblDedPaymentDate" runat="server" Text='<%#Eval("PVH_DATE") %>' ToolTip='<%#Eval("PVH_DATE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:TotalAmt %>" ItemStyle-HorizontalAlign="Right"
                                                    Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDedInvoiceAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("IVH_AMOUNT_TC")) %>'
                                                            ToolTip='<%#GetFormattedCurrency(Eval("IVH_AMOUNT_TC")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Invoice_Amount %>" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDedTaxAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("IVH_AMOUNT_NET_TC")) %>'
                                                            ToolTip='<%#GetFormattedCurrency(Eval("IVH_AMOUNT_NET_TC")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="7%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:PaymentAmount %>" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDedReceiptAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("PVH_PO_PAID_AMT")) %>'
                                                            ToolTip="<%$ resources:POAmt %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>                                             
                                                <asp:TemplateField HeaderText="<%$ resources:Allocated %>" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDedInvoiceAllocated" runat="server" Text='<%#GetFormattedCurrency(Eval("IVH_AMOUNT_ALLOCATED")) %>'
                                                            ToolTip="<%$ resources:POAllocated %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Balance %>" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDedInvoiceBal" runat="server" CssClass="BalancetoAllocate" Text='<%#GetFormattedCurrency(Convert.ToDecimal(Eval("PVH_PO_PAID_AMT").ToString())-Convert.ToDecimal(Eval("IVH_AMOUNT_ALLOCATED").ToString()))%>'
                                                            ToolTip="<%$ resources:PObal %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:AllocateNow %>" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDedAllocateNowSplit" runat="server" CssClass="Uiinput-amount numeric"
                                                            onkeyup="CalculateTotalSplit(this);" MaxLength="15" Width="90%">
                                                        </asp:TextBox>
                                                        <asp:HiddenField ID="hdfDedAllocateNowSplit" runat="server" />
                                                        <asp:RequiredFieldValidator ID="vrfDedAllocateNowSplit" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="deduction" EnableClientScript="true" runat="server" ControlToValidate="txtDedAllocateNowSplit"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ReqAllocation %>">
                                                        </asp:RequiredFieldValidator>
                                                        <cc1:AmountValidation ID="vreDedAllocateNowSplit" runat="server" ControlToValidate="txtDedAllocateNowSplit"
                                                            ErrorMessage="<%$ resources:Err_InvalidAllocation %>" NumberDigits="11" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="deduction"></cc1:AmountValidation>
                                                        <asp:CustomValidator ID="customQty" runat="server" ValidateEmptyText="true" ClientValidationFunction="CheckAllocation"
                                                            ErrorMessage="<%$ resources:Err_InvalidAllocation %>" Text="*" EnableClientScript="true"
                                                            ControlToValidate="txtDedAllocateNowSplit" CssClass="star" Display="Dynamic"
                                                            ValidationGroup="deduction"></asp:CustomValidator>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblDedTotalAllocateNowFooterSplit"></asp:Label>
                                                        <asp:HiddenField runat="server" ID="hdfDedTotalAllocateNowFooterSplit" />
                                                        <asp:HiddenField runat="server" ID="hdfOtherTotalFooterSplit" />
                                                        <asp:HiddenField runat="server" ID="hdfTaxTotalFooterSplit" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:OtherAmount %>" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>                                                       
                                                        <asp:TextBox ID="txtOtherAmountSplit" runat="server" CssClass="Uiinput-amount numeric"
                                                            onkeyup="CalculateTaxOCTotalSplit(this);" Text='<%#GetFormattedCurrency(Convert.ToDecimal(Eval("VAD_OTHER_AMOUNT").ToString()))%>'></asp:TextBox>
                                                        <asp:HiddenField ID="hdfDedOtherChargeSplitBalance" runat="server" Value='<%#GetFormattedCurrency(Convert.ToDecimal(Eval("PVH_PO_PAID_OTHER_AMT").ToString()) - Convert.ToDecimal(Eval("IVH_OTHER_AMT_ALLOCATED").ToString()))%>' />
                                                        <cc1:AmountValidation ID="vreDedOtherChargeNowSplit" runat="server" ControlToValidate="txtOtherAmountSplit"
                                                            ErrorMessage="<%$ resources:Err_InvalidAllocation %>" NumberDigits="11" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="deduction"></cc1:AmountValidation>
                                                        <asp:CustomValidator ID="customOtherCharge" runat="server" ValidateEmptyText="true"
                                                            ClientValidationFunction="CheckAllocationOtherCharge" ErrorMessage="<%$ resources:Err_InvalidAllocation %>"
                                                            Text="*" EnableClientScript="true" ControlToValidate="txtOtherAmountSplit" CssClass="star"
                                                            Display="Dynamic" ValidationGroup="deduction"></asp:CustomValidator>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblOtherAmountFooterSplit"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Tax %>" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>                                                      
                                                        <asp:TextBox ID="txtTaxSplit" runat="server" CssClass="Uiinput-amount numeric" onkeyup="CalculateTaxOCTotalSplit(this);"
                                                            Text='<%#GetFormattedCurrency(Convert.ToDecimal(Eval("VAD_TAX_AMOUNT").ToString()))%>'></asp:TextBox>
                                                        <asp:HiddenField ID="hdfDedTaxSplitBalance" runat="server" Value='<%#GetFormattedCurrency(Convert.ToDecimal(Eval("PVH_PO_PAID_TAX_AMT").ToString()) - Convert.ToDecimal(Eval("IVH_TAX_AMT_ALLOCATED").ToString()))%>' />
                                                        <cc1:AmountValidation ID="vreDedTaxNowSplit" runat="server" ControlToValidate="txtTaxSplit"
                                                            ErrorMessage="<%$ resources:Err_InvalidAllocation %>" NumberDigits="11" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="deduction"></cc1:AmountValidation>
                                                        <asp:CustomValidator ID="customTax" runat="server" ValidateEmptyText="true" ClientValidationFunction="CheckAllocationTax"
                                                            ErrorMessage="<%$ resources:Err_InvalidAllocation %>" Text="*" EnableClientScript="true"
                                                            ControlToValidate="txtTaxSplit" CssClass="star" Display="Dynamic" ValidationGroup="deduction"></asp:CustomValidator>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTaxFooterSplit"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <%-- ------- TAX Payable in MYR--------------------------%>
                            <div id="divOuterTaxPayable">
                                <h1 class="search-colapse-normal">
                                    <%=Resources.Controls.TaxPayable%>
                                    <img id="imgShowTaxPayable" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                                        alt="<%= Resources.Controls.Show%>" title="<%= Resources.Controls.Show%>" style="display: none;
                                        cursor: pointer" onclick="javascript:ShowTaxPayable();" />
                                    <img id="imgHideTaxPayable" src="../Images/Classic/Icons/arrow-colapse-active.png"
                                        alt="<%= Resources.Controls.Hide%>" title="<%= Resources.Controls.Hide%>" style="cursor: pointer"
                                        onclick="javascript:HideTaxPayable();" />
                                </h1>
                                <div id="divTaxPayable" class="gridwrap">
                                    <asp:GridView runat="server" ID="grdTaxPayable" Width="100%" AllowSorting="false"
                                        OnRowDataBound="ActionHandler" AutoGenerateColumns="false" TabIndex="22" EmptyDataRowStyle-CssClass="emptytable">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:TaxType %>" Visible="false">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdfTaxSplitPK" runat="server" Value='<%#Eval("VTL_PK") %>' />
                                                    <asp:HiddenField ID="hdfTaxPK" runat="server" Value='<%#Eval("VTL_TAX") %>' />
                                                    <asp:Label ID="lblTaxText" runat="server" Text='<%# Convert.ToString(Eval("VTL_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("VTL_TAX_TEXT")) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("VTL_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("VTL_TAX_TEXT"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:TaxCode %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxCode" runat="server" Text='<%# Convert.ToString(Eval("VTL_TAX_CODE")) == string.Empty ?  string.Empty : Convert.ToString(Eval("VTL_TAX_CODE")) %>'
                                                        ToolTip='<%# Convert.ToString(Eval("VTL_TAX_CODE")) == string.Empty ?  string.Empty : HttpUtility.HtmlDecode(Eval("VTL_TAX_CODE").ToString()) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:TaxName %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxName" runat="server" Text='<%#Eval("VTL_NAME") %>' ToolTip='<%# HttpUtility.HtmlDecode(Eval("VTL_NAME").ToString()) %>'></asp:Label>
                                                    <asp:HiddenField ID="hdfTaxName" runat="server" Value='<%#Eval("VTL_NAME") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:TaxRate %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxRate" runat="server" Text='<%# Convert.ToString(Eval("VTL_TAX_RATE")) == string.Empty ? string.Empty : Convert.ToString(Eval("VTL_TAX_RATE")) %>'
                                                        ToolTip='<%# (Convert.ToString(Eval("VTL_TAX_RATE")) == string.Empty ? string.Empty : Convert.ToString(Eval("VTL_TAX_RATE")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:SubTotal %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmountBeforeTax" runat="server" Text='<%# Convert.ToString(Eval("VTL_TAX_VID_AMOUNT")) == string.Empty ? string.Empty : GetFormattedCurrencyWithSeperation(Eval("VTL_TAX_VID_AMOUNT")) %>'
                                                        ToolTip='<%# (Convert.ToString(Eval("VTL_TAX_VID_AMOUNT")) == string.Empty ? HttpUtility.HtmlDecode(Eval("VTL_TAX_VID_AMOUNT").ToString()) : GetFormattedCurrencyWithSeperation(Eval("VTL_TAX_VID_AMOUNT")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:TaxAmount %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrencyWithSeperation(Eval("VTL_TAX_AMT")) %>'
                                                        ToolTip='<%#GetFormattedCurrencyWithSeperation(Eval("VTL_TAX_AMT")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <%-- -------- End TAX Payable in MYR-------------------%>
                            <%---------------------------------Start Paid Amount details Popup-----------------------------------%>
                            <div id="divPaidAmntSplitup" style="display: none">
                                <div class="content-wrapper">
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdPaidAmntSplitup" Width="100%" AllowSorting="false"
                                            AutoGenerateColumns="false" TabIndex="106" EmptyDataRowStyle-CssClass="emptytable"
                                            OnRowDataBound="ActionHandler" ShowFooter="true">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:TrxNo %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTrxNo" runat="server" Text='<%# Convert.ToString(Eval("PVH_NO")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="40%" />
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalText" runat="server" Text="<%$ resources:Total %>"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate" runat="server" Text='<%#  Eval("PVH_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("PVH_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                            ToolTip='<%# Eval("PVH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:TrxAmount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInvCrAmount" runat="server" Text='<%#GetFormattedCurrencyWithSeperation(Eval("CR_AMOUNT")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithSeperation(Eval("CR_AMOUNT")) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfInvCrAmountSplit" runat="server" Value='<%#Eval("CR_AMOUNT") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="25%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalInvCrAmountSplit" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:PaymentAmount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmount" runat="server" Text='<%#GetFormattedCurrencyWithSeperation(Eval("PAID_AMOUNT")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithSeperation(Eval("PAID_AMOUNT")) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfAmountSplit" runat="server" Value='<%#Eval("PAID_AMOUNT") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="25%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalAmountSplit" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <table class="gridwrap">
                                            <tbody>
                                                <tr align="left" style="color: #122648; background-color: #DEE3ED; border-width: 0px;
                                                    font-family: Verdana; font-size: 10px; font-weight: bold; height: 30px;">
                                                    <td>
                                                        <asp:Label ID="lblBalPay" runat="server" Text='<%$ resources:BalToPay %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblTotalBalToPay" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <%---------------------------------End Paid Amount details Popup-------------------------------------%>
                            <%-- -------Start Other Charge Popup --------------------------%>
                            <div id="divOtherchargeSplitUp" style="display: none">
                                <div class="content-wrapper">
                                    <div class="Button-container-popup">
                                        <asp:Button ID="btnSavePaymentSplit" runat="server" Text="<%$ resources:Controls,Apply %>"
                                            OnClick="ActionHandler" CommandName="OTHERCHARGEAPPLY" SkinID="btnInner-add-dsd"
                                            CommandArgument="Allocation_Section" ValidationGroup="payment" OnClientClick="javascript:ValidatePageNow('payment')" />
                                    </div>                                    
                                    <div class="error" id="divErrorLabel" runat="server" visible="false">
                                        <ul>
                                            <li>
                                                <asp:Literal runat="server" ID="lblSplitErrorMessage" Text="<%$ resources:Error_OtherChargeAllocation %>"></asp:Literal>
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="gridwrap">
                                        <asp:TableCell>
                                            <div class="gridwrap">
                                                <asp:GridView ID="grdOtherchargeSplit" runat="server" AutoGenerateColumns="False"
                                                    Width="100%" PageSize="<%$ resources:PageSize %>" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable"
                                                    AllowSorting="false" ShowFooter="true" OnRowDataBound="ActionHandler">
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$ resources:PONO %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOtherChargePONO" runat="server" Text='<%#Eval("POH_NO")%>'></asp:Label>
                                                                <asp:HiddenField ID="hdfPOOtherchargePK" runat="server" Value='<%#Eval("IVM_PK")%>' />
                                                                <asp:HiddenField ID="hdfPOPK" runat="server" Value='<%#Eval("IVM_PO_HDR")%>' />                                                                
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" />
                                                            <HeaderStyle Width="15%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:PODate %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPODateSplit" runat="server" Text='<%#DateTime.Parse(Eval("POH_DATE").ToString()).ToString(Resources.Constants.DateFormatShort)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" />
                                                            <HeaderStyle Width="15%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:OtherCharges %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPOTotalOtherCharge" runat="server" Text='<%#GetFormattedCurrencyWithSeperation(Eval("PO_OTHER_AMOUNT"))%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="amount-numeric" Width="15%" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <asp:Label runat="server" ID="lblTotalOtherCharge"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:InvdAmt %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPOInvdAmt" runat="server" Text='<%#GetFormattedCurrencyWithSeperation(Eval("PO_OTHER_AMOUNT_INVOICED"))%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="25%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="amount-numeric" Width="25%" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <asp:Label runat="server" ID="lblTotalInvOtherCharge"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Balance %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBalanceSplit" CssClass="BalancetoPay" runat="server" Text='<%#GetFormattedCurrencyWithSeperation(Convert.ToDecimal(Eval("PO_OTHER_AMOUNT").ToString())-Convert.ToDecimal(Eval("PO_OTHER_AMOUNT_INVOICED").ToString()))%>'></asp:Label>
                                                            </ItemTemplate>                                                           
                                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="amount-numeric" Width="10%" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <asp:Label runat="server" ID="lblTotalBalanceOtherCharge"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:AdjustNow %>" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAdjustNowAmount" runat="server" CssClass="Uiinput-amount numeric"
                                                                     onkeyup="CalculateTotalOtherCharge();" MaxLength="16" Text='<%#GetFormattedCurrency(Eval("IVM_OTHER_AMOUNT"))%>'>
                                                                </asp:TextBox>
                                                                <asp:CustomValidator ID="customvalOtherCharge" runat="server" ValidateEmptyText="true"
                                                                    ClientValidationFunction="ValidationCheckOtherCharge" ErrorMessage="<%$ resources:Err_InvalidOthercharge%>"
                                                                    Text="*" EnableClientScript="true" ControlToValidate="txtAdjustNowAmount" CssClass="star"
                                                                    Display="Dynamic" ValidationGroup="payment"></asp:CustomValidator>
                                                                <asp:RequiredFieldValidator ID="vrfPayNowSplit" CssClass="star" SetFocusOnError="true"
                                                                    ValidationGroup="payment" EnableClientScript="true" runat="server" ControlToValidate="txtAdjustNowAmount"
                                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PaymentAmount1 %>">
                                                                </asp:RequiredFieldValidator>
                                                                <cc1:AmountValidation ID="vreOtherCharge" runat="server" ControlToValidate="txtAdjustNowAmount"
                                                                    ErrorMessage="<%$ resources:Err_PaymentAmount %>" NumberDigits="12" Display="Dynamic"
                                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="payment"></cc1:AmountValidation>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="20%" />
                                                            <HeaderStyle CssClass="amount-numeric" Width="20%" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <asp:Label runat="server" ID="lblTotalAdjustNow"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </asp:TableCell>
                                    </div>
                                </div>
                            </div>
                            <%-- -------End Other Charge Popup --------------------------%>
                            <%-- -------Start GRN Splitup Popup --------------------------%>
                            <div id="divGRNQtySpilup" style="display: none">
                                <div class="content-wrapper">
                                    <div class="Button-container-popup">
                                        <asp:Button ID="btnGRNQtyApply" runat="server" Text="<%$ resources:Controls,Apply %>"
                                            ToolTip="<%$ resources:Controls,Apply %>" OnClick="ActionHandler" CommandName="GRNAPPLY"
                                            SkinID="btnInner-add-dsd" CommandArgument="Allocation_Section" ValidationGroup="grnSplit"
                                            OnClientClick="javascript:ValidatePageNow('grnSplit')" />
                                    </div>                                  
                                    <div class="gridwrap">
                                        <asp:TableCell>
                                            <div class="gridwrap">
                                                <asp:GridView ID="grdGRNDetails" runat="server" AutoGenerateColumns="False" Width="96%"
                                                    PageSize="<%$ resources:PageSize %>" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable"
                                                    AllowSorting="false" ShowFooter="true" OnRowDataBound="ActionHandler">
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$ resources:GRNNO %>">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkGRNNO" CssClass="text-underline" runat="server" Text='<%#Eval("VGL_GRN_NO")%>'
                                                                    ToolTip='<%#Eval("VGL_GRN_NO")%>' CommandArgument='<%#Eval("VGL_GRN_HDR")%>'
                                                                    OnClick="ActionHandler" CommandName="SHOW"></asp:LinkButton>
                                                                <asp:HiddenField ID="hdfVGLPK" runat="server" Value='<%#Eval("VGL_PK")%>' />
                                                                <asp:HiddenField ID="hdfVGLPOPK" runat="server" Value='<%#Eval("VGL_INVOICE_DTL")%>' />
                                                                <asp:HiddenField ID="hdfPOGRNPK" runat="server" Value='<%#Eval("VGL_GRN_DTL")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="20%" />
                                                            <HeaderStyle Width="20%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:GRNDate %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGRNDate" runat="server" Text='<%#DateTime.Parse(Eval("VGL_GRN_DATE").ToString()).ToString(Resources.Constants.DateFormatShort)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" />
                                                            <HeaderStyle Width="15%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:GRNQty %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGRNQty" runat="server" CssClass="amount-numeric" Text='<%#GetFormattedNumberWithSeperation(Eval("GRD_QTY_APPROVED"))%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="amount-numeric" Width="15%" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <asp:Label runat="server" ID="lblTotalGRNQty"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:GRNInvdQty %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGRNInvdQty" runat="server" CssClass="amount-numeric" Text='<%#GetFormattedNumberWithSeperation(Eval("GRN_QTY_INVOICED"))%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="amount-numeric" Width="15%" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <asp:Label runat="server" ID="lblTotalGRNInvdQty"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Balance %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGRNBalance" CssClass="BalancetoPay" runat="server" Text='<%#GetFormattedCurrencyWithSeperation(Convert.ToDecimal(Eval("GRD_QTY_APPROVED").ToString())-Convert.ToDecimal(Eval("GRN_QTY_INVOICED").ToString()))%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="amount-numeric" Width="15%" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <asp:Label runat="server" ID="lblTotalGRNBalance"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:InvNow %>" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtInvNowGRNQty" runat="server" CssClass="Uiinput-amount numeric"
                                                                    onkeyup="CalculateTotalGRN();" onmousedown="CalculateTotalGRN();" MaxLength="16"
                                                                    Text='<%#Eval("VGL_QTY_INVOICED")%>'>
                                                                </asp:TextBox>                                                             
                                                                <asp:RequiredFieldValidator ID="vrfGRNQtySplit" CssClass="star" SetFocusOnError="true"
                                                                    ValidationGroup="grnSplit" EnableClientScript="true" runat="server" ControlToValidate="txtInvNowGRNQty"
                                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_GRNQty %>">
                                                                </asp:RequiredFieldValidator>
                                                                <cc1:QuantityValidationP2P ID="vreGRNQtyInvNow" runat="server" ControlToValidate="txtInvNowGRNQty"
                                                                    NumberDigits="9" ErrorMessage="<%$ resources:Err_Invalid_InvNow %>" Display="Dynamic"
                                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="grnSplit"></cc1:QuantityValidationP2P>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="20%" />
                                                            <HeaderStyle CssClass="amount-numeric" Width="20%" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <asp:Label runat="server" ID="lblTotalGRNInvNow"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </asp:TableCell>
                                    </div>
                                </div>
                            </div>
                            <%-- -------End GRN Splitup Popup --------------------------%>
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
                    <asp:ValidationSummary ID="vsPayment" ValidationGroup="payment" runat="server" />
                    <asp:ValidationSummary ID="vsgrnSplit" ValidationGroup="grnSplit" runat="server" />
                    <asp:HiddenField ID="hdfAppType" runat="server" />
                    <asp:HiddenField ID="hdfAppSubType" runat="server" />
                    <asp:HiddenField ID="hdfSaveWithoutAllocation" runat="server" />
                </div>
            </div>
            <%--User Control--%>
            <div id="divJournalize" style="display: none">
                <uc1:Journalize ID="ucrJournalize" runat="server" />
            </div>
            <div id="divAlert" style="display: none">
                <uc2:Alert ID="ucrAlert" runat="server" />
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <asp:HiddenField ID="hdfIscontYes" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="invoice">
                </uc1:WorkflowUserComments>
            </div>
            <asp:HiddenField ID="hdfJournalizeWorkFlow" Value="0" runat="server" />
            <asp:HiddenField ID="hdfOtherPer" Value="0" runat="server" />
            <asp:HiddenField ID="hdfTaxPer" Value="0" runat="server" />
            <asp:HiddenField ID="HdfIsContYesOtherCharges" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIsContInvoiceNowQty" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIsContDupVenInvNo" Value="0" runat="server" />
            <asp:HiddenField ID="hdfVendorContactType" Value="0" runat="server" />
            <asp:HiddenField runat="server" ID="hdfIsTaxPayable" Value="0" />
            <asp:HiddenField runat="server" ID="hdfIsTaxOCEditable" Value="0" />
            <asp:HiddenField runat="server" ID="hdfAllocNowAmount" Value="0" />
            <asp:HiddenField runat="server" ID="hdfIsDedApplyClick" Value="0" />
            <asp:HiddenField runat="server" ID="hdfOtherCharge" Value="0" />
            <asp:HiddenField runat="server" ID="hdfApplyTax" Value="0" />
            <asp:HiddenField runat="server" ID="hdfisTaxAdd" Value="0" />
            <asp:HiddenField runat="server" ID="hdfisDiscountAdd" Value="0" />
            <asp:HiddenField runat="server" ID="hdfPriceAdjPercentage" Value="0" />
            <asp:HiddenField runat="server" ID="IsTaxForOtherCharge" Value="0" />
            <asp:HiddenField runat="server" ID="hdfIsAdvHasTax" Value="1" />
            <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
            <asp:HiddenField ID="hdfCurrencyGroup2" Value="2" runat="server" />
            <asp:HiddenField ID="hdfIsInvCancelled" Value="0" runat="server" />
            <asp:HiddenField ID="hdfGRNExceed" Value="0" runat="server" />
            <asp:HiddenField ID="hdfRateDecimalDigits" Value="3" runat="server" />
            <asp:HiddenField runat="server" ID="hdfShowEffRateInPI" Value="1" />
            <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
             <asp:HiddenField runat="server" ID="hdfInvPk" Value="0" />
             <asp:HiddenField runat="server" ID="hdfInvCategory" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAddItem" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
