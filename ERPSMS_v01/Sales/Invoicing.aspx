<%@ Page Title="<%$ Resources:Captions,Title_Invoicing %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="Invoicing.aspx.cs" Inherits="ERPSMS_v01.Sales.Invoicing"
    Theme="ClassicExt" ValidateRequest="false" %>

<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/Journalize/UserControls/JournalizeControlNew.ascx" TagName="Journalize"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/AlertControl.ascx" TagName="Alert" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/InvoicePrintDocs.ascx" TagName="PrinterControl"
    TagPrefix="pc1" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        [class="ui-widget-overlay"] {
            position: fixed !important;
        }

        [aria-labelledby^="ui-dialog"] {
            position: fixed !important;
        }
    </style>
    <script type="text/javascript">
        var NumberDigits = 0;
        var CurrencyDigits = 0;
        $(document).ready(function () {
            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
        });

        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
             var customerSelectText = "Type min 4 characters";
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            //            GrandScriptUtils.DatePickerCommon("txtInvoiceDate");
            //            GrandScriptUtils.DatePickerCommon("txtInvoiceDueDate");
            //            GrandScriptUtils.AddDateRangeCommon("txtInvoiceDate", "hdfInvoiceDate", "txtInvoiceDueDate", "hdfInvoiceDueDate", "dd-M-yy", false, false, false);
            GrandScriptUtils.AddDateRange("txtInvoiceDate", "hdfInvoiceDate", "txtInvoiceDueDate", "hdfInvoiceDueDate", false, false, false, false);
            GrandScriptUtils.AddDateRangeCommon("txtETD", "hdfETD", "txtETA", "hdfETA", "dd-M-yy", false, false, false);
            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtCustomer", url + "?IsSBUCustomer=" + $("[id$='hdfIsSBUCustomer']").val(), "hdfCustomerID", true, true, 4, "CUSTOMERLIST", "", "", "", "", customerSelectText);
            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtInvoiceNumber", url + "?Type=" + $("[id$=hdfgroup]").val(), "hdfIVHPK", true, true, 4, "SALINVOICENUMBER", "", "", "", "", customerSelectText);

            //$("[id*=txtPriceAdj]").ForceNumericOnly();
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();

            GrandScriptUtils.DatePickerCommon("txtEffectDate");
            GrandScriptUtils.DatePickerCommon("txtPVDate");
            GrandScriptUtils.DatePickerCommon("txtDueAson");
            $("[id*=txtInvNow]").ForceNumericOnly();
            $("[id*=txtInvNowSales]").ForceNumericOnly();
            if ($('[id$=btnJournalSaveSubmit]').is(":visible"))
                $('[id$=btnJournalSubmit]').hide();
            $("[id*=txtDedAllocateNowSplit]").ForceNumericOnly();

            setTableWidth();

            //Set a stamp for cancelled invoice
            if ($("[id$=hdfIsInvCancelled]").val() == "1")
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
            else
                $("[id$=tblDetailHdr]").addClass("table-devide");

            //End

        }

        function ValidationMessageSetting() {
            ///To prevent Jquery Validation Init. for hide unnecessory validation message
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
                        $("[id$=btnPickForReceipt]").click();
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

        function setTableWidth() {
            $("#[id*=grdInvoice] input[type=text][id*=txtInvNow]").each(function (index) {
                var perfoma = $(this).closest('tr').find("#[id*=lblInvProformaQuantity]").html();
                if (perfoma == null) {
                    $(this).closest("table").find("tr:first th:eq(0)").css("width", "8%");
                    $(this).closest("tr").find("td:eq(0)").css("width", "8%");
                    $(this).closest("table").find("tr:last td:eq(0)").css("width", "8%");
                }
                else {
                    $(this).closest("table").find("tr:first th:eq(0)").css("width", "8%");
                    $(this).closest("tr").find("td:eq(0)").css("width", "8%");
                    $(this).closest("table").find("tr:last td:eq(0)").css("width", "8%");

                }

            });
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
                $("[id$=pnlInActive]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlInActive]").hide();
                $("[id$=pnlAlert]").hide();
                $("[id$=pnlPrint]").hide();
                $("[id$=pnlCIPrint]").hide();
                $("[id$=btnShowDueDetails]").hide();

            }
        }
        function SetPrintDocsVisibility() {
            var CurrentPk = 0;
            CurrentPk = parseInt($("[id$=hdfCurrentPk]").val());
            if (CurrentPk == 0) {
                $("[id$=lnkPrintDocs]").attr("disabled", true);
                $("[id$=lnkPrintDocs]").removeAttr('href');
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
            if (targetControlID == "txtVendor") {
                $("[id$=btnVendor]").click();
            }
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteSelect(targetControlID);
            }
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtVendor") {
                $("[id$=btnVendor]").click();
            }
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteInvalidSelect(targetControlID);
            }
        }
        function CalculateTotal(sender) {
            var subTotal = parseFloat($("#[id*=grdInvoice]").find('input[type=Text][id$=txtSubTotalFooter]').val());
            subTotal = isNaN(subTotal) ? 0 : subTotal;
            var netTotal = 0;
            var tax = 0;
            var SaleOrderType = parseFloat($("[id$=hdfSaleOrderType]").val());
            var IsAdvInvHasTax = parseFloat($("[id$=hdfIsAdvInvHasTax]").val());
            $("[id$=grdInvoice] tr").each(function () {
                if ($(this).find("[id*=txtTax]").length > 0) {
                    var tempTax = 0;
                    tempTax = parseFloat($(this).find("[id*=txtTax]").val());
                    tax = tax + tempTax;
                }
            });
            if (SaleOrderType == 1 && IsAdvInvHasTax != 0) {
                //            subTotal = subTotal - tax;
                var totalDiscount = parseFloat($("[id$=txtHdrDiscount]").val());
                totalDiscount = isNaN(totalDiscount) ? 0 : totalDiscount;
                ////
                $("[id$=txtHdrTotal]").val((subTotal - totalDiscount).toFixed(CurrencyDigits));
                var totalDeduction = parseFloat($("[id$=txtHdrDeduction]").val());
                var AdvDeduction = parseFloat($("[id$=txtDiscDeducted]").val());
                totalDeduction = isNaN(totalDeduction) ? 0 : totalDeduction;
                $("[id$=txtHdrBalBeforeVat]").val((((subTotal - totalDiscount) - totalDeduction) - AdvDeduction).toFixed(CurrencyDigits));
                ////
                var totalTax = parseFloat($("[id$=txtHdrTax]").val());
                totalTax = isNaN(totalTax) ? 0 : totalTax;
                var totalShipping = parseFloat($("[id$=txtShipping]").val());
                totalShipping = isNaN(totalShipping) ? 0 : totalShipping;
                var totalPriceAdj = parseFloat($("[id$=txtPriceAdj]").val());
                totalPriceAdj = isNaN(totalPriceAdj) ? 0 : totalPriceAdj;
                var BalBeforeVat = parseFloat($("[id$=txtHdrBalBeforeVat]").val());
                var DeductOtherCharges = parseFloat($("[id$=txtdor]").val());

                //var netTotal = (subTotal + totalTax + totalShipping + totalPriceAdj) - totalDiscount;
                netTotal = (BalBeforeVat + totalTax + totalShipping + totalPriceAdj); // - DeductOtherCharges;
            }
            else {
                //                var totalPriceAdjOld = parseFloat($("[id$=hdfPriceAdj]").val()); 
                //                var totalPriceAdj = parseFloat($("[id$=txtPriceAdj]").val());
                //                var totalExp = parseFloat($("[id$=txtTotalExp]").val());
                //                $("[id$=txtTotalExp]").val(((isNaN(totalExp) ? 0 : totalExp )+( isNaN(totalPriceAdj) ? 0 : totalPriceAdj)).toFixed(CurrencyDigits));
                //                var totalDeductionExp = parseFloat($("[id$=txtTotalDeductionExp]").val());
                //                netTotal = ((isNaN(totalExp) ? 0 : totalExp )+ (isNaN(totalPriceAdj) ? 0 : totalPriceAdj)) - (isNaN(totalDeductionExp) ? 0 : totalDeductionExp); // - DeductOtherCharges;
                var totalPriceAdjOld = parseFloat($("[id$=hdfPriceAdj]").val());
                var totalPriceAdj = parseFloat($("[id$=txtPriceAdj]").val());
                var totalExp = parseFloat($("[id$=txtTotalExp]").val());
                $("[id$=txtTotalExp]").val((((isNaN(totalExp) ? 0 : totalExp) + (isNaN(totalPriceAdj) ? 0 : totalPriceAdj)) - (isNaN(totalPriceAdjOld) ? 0 : totalPriceAdjOld)).toFixed(CurrencyDigits));
                var totalDeductionExp = parseFloat($("[id$=txtTotalDeductionExp]").val());
                netTotal = (((isNaN(totalExp) ? 0 : totalExp) + (isNaN(totalPriceAdj) ? 0 : totalPriceAdj)) - totalDeductionExp) - (isNaN(totalPriceAdjOld) ? 0 : totalPriceAdjOld); // - DeductOtherCharges;
            }
            //            //netTotal += dor; 
            //            $("[id$=txtHdrNetTotal]").val((isNaN(netTotal) ? 0 : netTotal).toFixed(CurrencyDigits));
            //            $("[id$=txtHdrNetTotal]").attr("title", ((isNaN(netTotal) ? 0 : netTotal)).toFixed(CurrencyDigits));
            //            var invoiceTotal = ($("[id$=ddlInvoiceType]").val() == "2" ? ((isNaN(totalExp) ? 0 : totalExp) + (isNaN(totalPriceAdj) ? 0 : totalPriceAdj)) : netTotal) //+ DeductOtherCharges;
            //            //Edit 07_08_2014

            //            //            invoiceTotal = invoiceTotal - totalPriceAdj;
            //            $("[id$=txtHdrInvoiceTotal]").val(($("[id$=ddlInvoiceType]").val() == "2" ? invoiceTotal : netTotal).toFixed(CurrencyDigits)); //if domestic show nettotal (as per manoj sir :while testing 'LOCAL SALE +ADVANCE RECEIVE 100%')
            //            $("[id$=txtHdrInvoiceTotal]").attr("title", ($("[id$=ddlInvoiceType]").val() == "2" ? invoiceTotal : netTotal).toFixed(CurrencyDigits));


            //            //End
            //            //End

            $("[id$=txtHdrNetTotal]").val((netTotal).toFixed(CurrencyDigits));
            $("[id$=txtHdrNetTotal]").attr("title", (netTotal).toFixed(CurrencyDigits));
            var invoiceTotal = ($("[id$=ddlInvoiceType]").val() == "2" ? (((isNaN(totalExp) ? 0 : totalExp) + (isNaN(totalPriceAdj) ? 0 : totalPriceAdj))) - (isNaN(totalPriceAdjOld) ? 0 : totalPriceAdjOld) : netTotal) //+ DeductOtherCharges;
            //Edit 07_08_2014
            $("[id$=hdfPriceAdj]").val(((isNaN(totalPriceAdj) ? 0 : totalPriceAdj)).toFixed(CurrencyDigits));
            //            invoiceTotal = invoiceTotal - totalPriceAdj;
            if ($("[id$=ddlInvoiceType]").val() == "1" && IsAdvInvHasTax == 0)//Domestic WithOut Advance Tax
            {
                var totalTax = parseFloat($("[id$=txtHdrTax]").val());
                totalTax = isNaN(totalTax) ? 0 : totalTax;
                var totalShipping = parseFloat($("[id$=txtShipping]").val());
                totalShipping = isNaN(totalShipping) ? 0 : totalShipping;
                var totalPriceAdj = parseFloat($("[id$=txtPriceAdj]").val());
                totalPriceAdj = isNaN(totalPriceAdj) ? 0 : totalPriceAdj;
                var BalBeforeVat = parseFloat($("[id$=txtHdrBalBeforeVat]").val());
                var invoiceTotal = (BalBeforeVat + totalTax + totalShipping + totalPriceAdj);
                $("[id$=txtHdrInvoiceTotal]").val(invoiceTotal.toFixed(CurrencyDigits));
                $("[id$=txtHdrInvoiceTotal]").attr("title", (invoiceTotal).toFixed(CurrencyDigits));

            }
            else {
                $("[id$=txtHdrInvoiceTotal]").val(netTotal.toFixed(CurrencyDigits)); //if domestic show nettotal (as per manoj sir :while testing 'LOCAL SALE +ADVANCE RECEIVE 100%')
                $("[id$=txtHdrInvoiceTotal]").attr("title", (netTotal).toFixed(CurrencyDigits));
            }

            // $("[id$=txtHdrInvoiceTotal]").val((invoiceTotal).toFixed(CurrencyDigits));
            //$("[id$=txtHdrInvoiceTotal]").attr("title", (invoiceTotal).toFixed(CurrencyDigits));
            if (totalShipping == 0)
                $("[id$=txtShipping]").val((totalShipping).toFixed(CurrencyDigits));
            if (totalPriceAdj == 0)
                $("[id$=txtPriceAdj]").val((totalPriceAdj).toFixed(CurrencyDigits));
        }

        function AfterClose(containerID) {
            if (containerID == "[id$=divJournalize]") {
                $("[id$=btnJournalizeUpdate]").click();
            }
            else if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
                if ($("[id$=hdfJournalizeWorkFlow]").val() == "1") {
                    //ShowContainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), '1000', '550');
                    //ShowContainerDiv('[id$=divJournalize]', '<%= GetLocalResourceObject("Sales_Invoice_Journal") %>', '1000', '550');
                    ShowCommonCotainerDiv('[id$=divJournalize]', '<%= GetLocalResourceObject("Sales_Invoice_Journal") %>', "1%");
                    AfterCloseWkfInJournal();
                    //$("[id$=btnJournalize_Action]").click();
                }
            } else if (containerID == "[id$=divTemplate]") {
                //ShowContainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), '1000', '550');
                ShowCommonCotainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), "1%");
            }
        }

        function AfterDateSelect(controlID) {
            if (controlID == "txtInvoiceDate") {
                var invoiceDueDate = $.datepicker.parseDate("dd-M-yy", $("[id$=txtInvoiceDate]").val());
                invoiceDueDate.setDate(invoiceDueDate.getDate() + parseInt($("[id$=hdfCreditDays]").val()));
                $("[id$=hdfInvoiceDueDate]").val($.datepicker.formatDate("mm/dd/yy", invoiceDueDate));
                $("[id$=txtInvoiceDueDate]").val($.datepicker.formatDate("dd-M-yy", invoiceDueDate));
                //                if ($("[id$=hdfHasTax]").val() != "0") {
                //                    ShowErrorMessage('<%=Resources.Messages.TaxDateChanged %>', '<%=Resources.Messages.Information %>');
                //                }
                //$("[id$=btnInvoiceDate]").click();

                //if ($("[id$=hdfShippingTermValue]").val() == "1" /*Ex-Work*/
                //    || $("[id$=hdfShippingTermValue]").val() == "5") /*FCA*/ {
                //    $("[id$=txtEffectDate]").val($.datepicker.formatDate("dd-M-yy", invoiceDueDate));
                //}
                //$("[id$=btnDateChange]").click();
            }

            if (typeof AfterAlertControlDateSelect == "function") {
                AfterAlertControlDateSelect(controlID);
            }

            if (controlID == "txtETA" || controlID == "txtETD") {
                $("[id$=btnDateChange]").click();
            }
            else if (controlID != "txtInvoiceDueDate" && controlID != "txtPVDate" && controlID != "txtFromDate" && controlID != "txtToDate") {
                if (controlID == "txtInvoiceDate") {
                    $("[id$=btnDateChange]").click();
                }
                else {
                    $("[id$=btnTermCheck]").click();
                }
            }
        }
        function AfterExchangeRate() {

            $("[id$=btnExchangeRate]").click();
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
        function CalculateInvNow(hasInvNow) {
            if (!hasInvNow) {
                $("[id$=grdInvoice] tr").each(function () {
                    var orderQty = 0;
                    var invQty = 0;
                    if ($(this).find("[id*=lblOrderQuantity]").length > 0)
                        orderQty = parseFloat($(this).find("[id*=lblOrderQuantity]").html().replace(/[^0-9\.]+/g, ""));
                    if ($(this).find("[id*=lblInvQuantity]").length > 0)
                        invQty = parseFloat($(this).find("[id*=lblInvQuantity]").html().replace(/[^0-9\.]+/g, ""));
                    if (!isNaN(orderQty) && !isNaN(invQty) && orderQty > invQty) {
                        var invNow = parseFloat(orderQty - invQty);
                        //$(this).find("[id*=txtInvNow]").val(invNow.toFixed(NumberDigits));
                        $(this).find("[id*=txtInvNowSales]").val(invNow.toFixed(NumberDigits));
                    }
                });
            }
            $("[id$=btnRecalculate]").click();
        }
        function ResetSelection() {
            $('[id$=grdInvoiceList]').find('tr td input:radio[id$=rbtSelect]').removeAttr('checked');
            $('[id$=grdInvoiceList]').find('tr td input:checkbox[id$=chkInvselect]').removeAttr('checked');
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

            var taxInUnit = 0;

            var discInUnit = 0;

            var otherChargeInUnit = 0;

            var allocateNow = 0;

            var totOtherAmount = 0;

            var totTax = 0;

            var totDisc = 0;

            var PaidAmount = 0;
            var DecimalDigitsCount = 2;
            // if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {

            // DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());

            // }

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

                        // PaidAmount = parseFloat($(this).closest('tr').find('[id*=lblDedTaxAmount]').html().replace(/[^0-9\.]+/g, ""));
                        PaidAmount = parseFloat($(this).closest('tr').find('[id*=lblDedReceiptAmount]').html().replace(/[^0-9\.]+/g, "")); //reciept against corres SC ,Therefor lblDedReceiptAmount instead of lblDedTaxAmount 
                        allocateNow = parseFloat($(this).val());

                        otherChargeInUnit = prevOtherAmount / PaidAmount;

                        taxInUnit = prevTax / PaidAmount;
                        curOtherAmount = allocateNow * otherChargeInUnit;
                        discInUnit = prevDisc / PaidAmount;

                        curTax = allocateNow * taxInUnit;
                        curDisc = allocateNow * discInUnit;
                        totOtherAmount += curOtherAmount;
                        //                        curTax=parseFloat(curTax.toFixed(DecimalDigitsCount));
                        curDisc = parseFloat(curDisc.toFixed(DecimalDigitsCount));
                        totTax += curTax;
                        totDisc += curDisc;



                        parseFloat($(this).closest('tr').find("#[id*=txtOthercharges]").val(curOtherAmount.toFixed(DecimalDigitsCount)));
                        parseFloat($(this).closest('tr').find("#[id*=lblOthercharges]").html(curOtherAmount.toFixed(DecimalDigitsCount)));
                        $(this).closest('tr').find("#[id*=hdfCurPaidOtherAmount]").val(curOtherAmount.toFixed(DecimalDigitsCount));



                        parseFloat($(this).closest('tr').find("#[id*=lblTax]").html(curTax.toFixed(DecimalDigitsCount)));
                        $(this).closest('tr').find("#[id*=hdfCurPaidTax]").val(curTax.toFixed(DecimalDigitsCount));

                        $(this).closest('tr').find("#[id*=hdfCurPaidDisc]").val(curDisc.toFixed(DecimalDigitsCount));

                        $(this).parent("td").find('input[type=hidden][id$=hdfDedAllocateNowSplit]').val($(this).val());

                        $(this).closest('tr').find("#[id*=txtTaxSplit]").val(curTax.toFixed(DecimalDigitsCount));
                        $(this).closest('tr').find("#[id*=txtOtherAmountSplit]").val(curOtherAmount.toFixed(DecimalDigitsCount));

                        $(this).closest('tr').find("#[id*=hdfDedTaxSplitBalance]").val(curTax.toFixed(DecimalDigitsCount));
                        $(this).closest('tr').find("#[id*=hdfDedOtherChargeSplitBalance]").val(curOtherAmount.toFixed(DecimalDigitsCount));
                        $("[id$=hdfAllocNowAmount]").val($(this).val());


                        Amount = Amount + parseFloat($(this).val());




                    }

                }
                else {
                    var clearVal = 0.00;
                    parseFloat($(this).closest('tr').find("#[id*=txtOthercharges]").val(clearVal.toFixed(DecimalDigitsCount)));
                    parseFloat($(this).closest('tr').find("#[id*=lblOthercharges]").html(clearVal.toFixed(DecimalDigitsCount)));
                    $(this).closest('tr').find("#[id*=hdfCurPaidOtherAmount]").val(clearVal.toFixed(DecimalDigitsCount));
                    parseFloat($(this).closest('tr').find("#[id*=lblTax]").html(clearVal.toFixed(DecimalDigitsCount)));
                    $(this).closest('tr').find("#[id*=hdfCurPaidTax]").val(clearVal.toFixed(DecimalDigitsCount));

                    $(this).closest('tr').find("#[id*=hdfCurPaidDisc]").val(clearVal.toFixed(DecimalDigitsCount));

                    $(this).parent("td").find('input[type=hidden][id$=hdfDedAllocateNowSplit]').val($(this).val());

                    $(this).closest('tr').find("#[id*=txtTaxSplit]").val(clearVal.toFixed(DecimalDigitsCount));
                    $(this).closest('tr').find("#[id*=txtOtherAmountSplit]").val(clearVal.toFixed(DecimalDigitsCount));

                    $(this).closest('tr').find("#[id*=hdfDedTaxSplitBalance]").val(clearVal.toFixed(DecimalDigitsCount));
                    $(this).closest('tr').find("#[id*=hdfDedOtherChargeSplitBalance]").val(clearVal.toFixed(DecimalDigitsCount));
                    $("[id$=hdfAllocNowAmount]").val($(this).val());

                }

            });



            $("#[id*=grdDeduction] [id*=lblDedTotalAllocateNowFooterSplit]").html(addCommas(Amount.toFixed(CurrencyDigits)));

            //            $("#[id*=grdDeduction] [id*=lblDedTotalAllocateNowFooterSplit]").title(Amount.toFixed(CurrencyDigits));

            $("#[id*=grdDeduction] [id*=hdfDedTotalAllocateNowFooterSplit]").val(Amount);

            $("#[id*=grdDeduction] [id*=hdfOtherTotalFooterSplit]").val(totOtherAmount);

            //            $("#[id*=grdDeduction] [id*=lblOthercharges]").html(totOtherAmount.toFixed(CurrencyDigits));

            $("#[id*=grdDeduction] [id*=hdfTaxTotalFooterSplit]").val(totTax);

            $("#[id*=grdDeduction] [id*=lblTotalTaxFooter]").html(addCommas(totTax.toFixed(CurrencyDigits)));

            //var exchangeRate = $("[id$=hdfExchangeCurr]").val() == "" ? 1 : $("[id$=hdfExchangeCurr]").val();

        }

        function OLDCalculateTotalSplit(sender) {
            var val1 = parseFloat($(sender).val());
            var Amount = 0;
            var BalancetoPay = 0;
            var DecimalDigits = 0;


            //            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
            //                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            //            }

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
            $("#[id*=grdDeduction] [id*=lblDedTotalAllocateNowFooterSplit]").html(addCommas(Amount.toFixed(CurrencyDigits)));
            //            $("#[id*=grdDeduction] [id*=lblDedTotalAllocateNowFooterSplit]").title(Amount.toFixed(CurrencyDigits));
            $("#[id*=grdDeduction] [id*=hdfDedTotalAllocateNowFooterSplit]").val(Amount);
            //var exchangeRate = $("[id$=hdfExchangeCurr]").val() == "" ? 1 : $("[id$=hdfExchangeCurr]").val();


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

        function ShowDeleteConfirmWithReason() {
            ShowContainerDiv('[id$=divConfirmationWithReason]', 'Information', '550', '220');
            return false;
        }

        function closeDeletePopup() {
            $("[id$=txtReason]").val("");
            $('#divConfirmationWithReason').dialog('close');
            ClosePopup();
            return false;
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
                        //__doPostBack(btn.name, '');
                        $("[id$=" + btn + "]").click();
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        if ($("[id$=hdfSalOrderType]").val() == "2") {
                            $("[id$=imgAllocateExp]").click();
                        }
                        else {
                            var IsAdvInvHasTax = parseFloat($("[id$=hdfIsAdvInvHasTax]").val());
                            if (IsAdvInvHasTax == 0) {
                                $("[id$=imgAllocateExp]").click();
                            }
                            else {
                                $("[id$=imgHdrDeduction]").click();
                            }
                        }

                        return false;
                    }
                }
            });
            return false;
        }




        //For Setting/Resetting Colour of a selected Row
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
                    TotalAmountSplit = TotalAmountSplit + parseFloat($(this).val().replace(/[^0-9\.]+/g, ""));
                var InvCrAmount = $(this).closest('tr').find("#[id*=hdfInvCrAmountSplit]").val();
                if (!isNaN(InvCrAmount))
                    TotalInvCrAmountSplit = TotalInvCrAmountSplit + parseFloat(InvCrAmount);
            });
            $("#[id*=grdPaidAmntSplitup] [id*=lblTotalAmountSplit]").html(addCommas(TotalAmountSplit.toFixed(CurrencyDigits)));
            $("#[id*=grdPaidAmntSplitup] [id*=lblTotalInvCrAmountSplit]").html(addCommas(TotalInvCrAmountSplit.toFixed(CurrencyDigits)));
            $("#[id*=lblTotalBalToPay]").html(addCommas(Math.abs(TotalInvCrAmountSplit.toFixed(CurrencyDigits) - TotalAmountSplit.toFixed(CurrencyDigits)).toFixed(CurrencyDigits)));

        }



        //Enable/Disable tax,OtherCharge textbox in Allocation popup

        function EnableDisableTaxOtherCharge() {

            if ($("[id$=hdfIsTaxOCEditable]").val() == 1) {

                $("#[id*=grdDeduction] input[type=text][id*=txtDedAllocateNowSplit]").each(function (index) {

                    $(this).closest('tr').find("#[id*=txtOtherAmountSplit]").attr("disabled", false);
                    $(this).closest('tr').find("#[id*=txtTaxSplit]").attr("disabled", true); //In Any Cases Tax Must be Disabled
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
            var AllocAmount = 0;
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
                //  allocateNow = parseFloat($("[id$=hdfAllocNowAmount]").val());
                if (!isNaN($(this).closest('tr').find("#[id*=txtDedAllocateNowSplit]").val())) {
                    var DedAllocateNow = Number($(this).closest('tr').find("#[id*=txtDedAllocateNowSplit]").val().replace(/[^0-9\.]+/g, ""));
                    allocateNow = parseFloat(DedAllocateNow);
                }

                if (!isNaN(allocateNow)) {
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
                    AllocAmount = AllocAmount + allocateNow;
                    allocateNow = allocateNow - OtherChargeReduced - taxReduced;

                    Amount = Amount + allocateNow;
                    if (sender.id == $(this).id) { //For Current Row Updation
                        $(this).parent("td").find('input[type=hidden][id$=hdfDedAllocateNowSplit]').val(allocateNow);
                        $(this).val(allocateNow.toFixed(CurrencyDigits));
                    }
                    $(this).closest('tr').find("#[id*=hdfCurPaidOtherAmount]").val(curOtherAmount.toFixed(CurrencyDigits));
                    $(this).closest('tr').find("#[id*=hdfCurPaidTax]").val(curTax.toFixed(CurrencyDigits));
                    $(this).closest('tr').find("#[id*=hdfCurPaidDisc]").val(curDisc.toFixed(CurrencyDigits));

                    //                $(this).closest('tr').find("#[id*=hdfDedOtherChargeSplitBalance]").val(curOtherAmount);
                    //  
                    $(this).closest('tr').find("#[id*=hdfDedTaxSplitBalance]").val(curTax);
                }

            });
            $("#[id*=grdDeduction] [id*=lblDedTotalAllocateNowFooterSplit]").html(addCommas(AllocAmount.toFixed(CurrencyDigits)));
            //            $("#[id*=grdDeduction] [id*=lblDedTotalAllocateNowFooterSplit]").title(Amount.toFixed(CurrencyDigits));
            $("#[id*=grdDeduction] [id*=hdfDedTotalAllocateNowFooterSplit]").val(Amount);
            $("#[id*=grdDeduction] [id*=hdfOtherTotalFooterSplit]").val(totOtherAmount);
            $("#[id*=grdDeduction] [id*=lblOtherAmountFooterSplit]").html(totOtherAmount.toFixed(CurrencyDigits));
            $("#[id*=grdDeduction] [id*=hdfTaxTotalFooterSplit]").val(totTax);
            $("#[id*=grdDeduction] [id*=lblTotalTaxFooter]").html(addCommas(totTax.toFixed(CurrencyDigits)));


        }


        //For Setting  Colour for Current SC Advance Invoice (deduct)
        function SetCurrentAdvInvoiceRowColor() {

            $("#<%= grdDeduction.ClientID %> input[type=hidden][id*=hdfCusAdvFlag]").each(function (index) {
                if ($.trim($(this).val()) == "1") {
                    var selectedRowColor;
                    selectedRowColor = '<%= Resources.ErpRes.selectedRowColor %>';
                    $(this).closest('tr').css('background-color', selectedRowColor);

                }

            });
        }
        //End

        function SelectedCheckBoxCount(mode) {
            var count = $('[id$=grdInvoiceList]').find('tr td input:checkbox[id$=chkInvselect]:checked').length;
            var msgTitle;
            var msg;
            msg = '<%= ERP.Utilities.CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Select_Invoice").ToString()) %>';
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            if (mode == 1) {
                if (count == 0) {
                    //msg = '<%= GetLocalResourceObject("Msg_SelectItem").ToString() %>';
                    //GrandScriptUtils.ShowModal(msg, msgTitle);
                    ShowErrorMessage(msg, msgTitle);
                    return false;
                }
                else if (count > 1) {
                    msg = '<%= ERP.Utilities.CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_SelectoneItem").ToString()) %>';
                    //GrandScriptUtils.ShowModal(msg, msgTitle);
                    ShowErrorMessage(msg, msgTitle);
                    return false;
                }
            }
            else if (mode == 0) {
                if (count > 1) {
                    msg = '<%= ERP.Utilities.CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_SelectoneItem").ToString()) %>';
                    //GrandScriptUtils.ShowModal(msg, msgTitle);
                    ShowErrorMessage(msg, msgTitle);
                    return false;
                }
            }
            else if (mode == 2) {
                if (count == 0) {
                    //msg = '<%= GetLocalResourceObject("Msg_SelectItem").ToString() %>';
                    //GrandScriptUtils.ShowModal(msg, msgTitle);
                    ShowErrorMessage(msg, msgTitle);
                    return false;
                }
            }
        }

        function CheckPrintDocs() {
            var count = $('[id$=grdInvoiceList]').find('tr td input:checkbox[id$=chkInvselect]:checked').length;
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';

            if ($("[id$=hdfMode]").val() == "LISTMODE") {
                if (count == 0) {
                    msg = '<%= ERP.Utilities.CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_SelectItem").ToString()) %>';
                    //GrandScriptUtils.ShowModal(msg, msgTitle);
                    ShowErrorMessage(msg, msgTitle);
                    return false;
                }
                else if (count > 1) {
                    msg = '<%= ERP.Utilities.CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_SelectoneItem").ToString()) %>';
                    //GrandScriptUtils.ShowModal(msg, msgTitle);
                    ShowErrorMessage(msg, msgTitle);
                    return false;
                }
            }
            return true;
        }

        function ValidationCheckOtherCharge(sender, args) {
            var split = $(sender).closest('tr').find('[id*=txtAdjustNowAmount]').val();
            var pattern = new RegExp($(sender).closest('tr').find('[id*=vreOtherCharge]')[0].validationexpression);
            var spliAmount = parseFloat(split);
            if ($("[id$=hdfEnableAddlOtherCharge]").val() == "1") {
                args.IsValid = true;
            }
            else if (pattern.test(split) && !isNaN(spliAmount)) {
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

        function CalculateTotalOtherCharge() {
            //var val1 = parseFloat($(sender).val());
            var TotalOtherAmount = 0;

            $("#[id*=grdOtherchargeSplit] input[type=text][id*=txtAdjustNowAmount]").each(function (index) {
                //                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=txtAdjustNowAmount]").val()))) {
                //                    Amount = Amount + parseFloat($(this).closest('tr').find("#[id*=txtAdjustNowAmount]").val().replace(/[^0-9\.]+/g, ""));                    
                //                }

                if ($.trim($(this).val()) != "") {
                    if (!isNaN(parseFloat($(this).val()))) {
                        TotalOtherAmount = TotalOtherAmount + parseFloat($(this).val());
                    }
                }
            });
            $("#[id*=grdOtherchargeSplit] [id*=lblTotalAdjustNow]").html(addCommas(TotalOtherAmount.toFixed(CurrencyDigits)));
        }


        //Remove hyper link if Invoice amount and Balance amount are same
        function RemoveBalAmntHyperLink() {
            $("#<%= grdInvoiceList.ClientID %> input[type=hidden][id*=hdfInvoiceID]").each(function (index) {
                var balance = Number($(this).closest('tr').find('[id*=lbnBalAmt]').html().replace(/[^0-9\.]+/g, ""));
                var balAmnt = parseFloat(balance);
                var invoiceAmnt = Number($(this).closest('tr').find('[id*=lblInvoiceValue]').html().replace(/[^0-9\.]+/g, ""));
                var invAmnt = parseFloat(invoiceAmnt);
                if (balAmnt == invAmnt) {
                    $(this).closest('tr').find('[id*=lbnBalAmt]').removeAttr("href");
                    $(this).closest('tr').find('[id*=lbnBalAmt]').removeAttr("class");
                }
            });
        }
        //End


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
                if (isNaN(NumericPart) && NumericPart.length == 1) {
                    LastNumericPart = LastNumericPart.substr(1, LastNumericPart.length);
                }
            }
            FormattedNumber = NumericPart + LastNumericPart + DecimalPart;
            return FormattedNumber;
        }

        function ShowHideUploadDocDetails(flag) {
            //If flag then Show Upload Doc Details
            if (flag) {
                $("[id$=tblUploadDocDetails]").show();
                $("[id$=imbShowDetails]").hide();
                $("[id$=imbHideDetails]").show();
            }
            else {
                $("[id$=tblUploadDocDetails]").hide();
                $("[id$=imbShowDetails]").show();
                $("[id$=imbHideDetails]").hide();
            }
            return false;
        }
        //Invoicing Amount should not be greater than SC Amount
        function ShowInvoiceAmtGreaterSCAmtConfirm(btn) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%=GetLocalResourceObject("Msg_InvAmtGreaterSCAmt_Confirm").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $("[id$=hdfSaveWithGreaterInvAmount]").val("1");
                        $(this).dialog("close");
                        $("[id$=" + btn + "]").click();
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }

        function ShowHideCategoryWiseTotal() {
            if ($("[id$=hdfIsMultiplePlant]").val() == "1" && $("[id$=hdfIsMultipleCategory]").val() == "1") {
                $("#divCategoryWiseTotal").show();
            }
            else {
                $("#divCategoryWiseTotal").hide();
            }
        }

        //For   confirm to aditional other charges
        function ConfirmAdditionalOtherCharges() {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Msc_ConfirmAdditionalOtherCharge").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfConfirmOtherCharges]").val(1);
                        $(this).dialog("close");
                        $("[id$=btnConfirmOtherCharges]").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfConfirmOtherCharges]").val(0);
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }

        function ValidateETD(sender, args) {
            if ($("[id$=hdfShippingTermValue]").val() == "2") { // FOB - Date of ETD
                if (args.Value) {//if ($("[id$=txtETD]").val()) {
                    args.IsValid = true;
                }
                else {
                    args.IsValid = false;
                }
            }
            else
                args.IsValid = true;
        }
        function ValidateETA(sender, args) {
            debugger;
            if ($("[id$=hdfShippingTermValue]").val() == "4") { // 4 - DAP - Date of ETA
                if (args.Value) {//if ($("[id$=txtETA]").val()) {
                    args.IsValid = true;
                }
                else {
                    args.IsValid = false;
                }
            }
            else
                args.IsValid = true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlSOInvoice">
        <ContentTemplate>
            <div class="fixed-buttons">
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
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="50"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="51"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnInvSubmit" CommandName="SUBMIT" TabIndex="52" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="inv" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="53" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteNew" CommandName="DELETE" TabIndex="54" Text="<%$resources:ErpRes,Delete %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Delete %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Delete" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li runat="server" id="pnlInActive">
                                        <asp:Button runat="server" ID="btnInActive" CommandName="INACTIVE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="55" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirmWithReason();" />
                                    </li>
                                    <li id="pnlPrint">
                                        <asp:Button runat="server" TabIndex="56" ID="btnPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li id="pnlCIPrint" runat="server" visible="true">
                                        <asp:Button runat="server" TabIndex="49" ID="btnCIPrint" CommandName="PRINTCI" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,PrintCI %>" CssClass="btnInner-Print-green-btn" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,PrintCI %>" />
                                    </li>
                                    <li id="pnlExcelPrint">
                                        <asp:Button runat="server" TabIndex="56" ID="btnExcelPrint" CommandName="EXCELPRINT"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,ExcelPrint %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Excel" ToolTip="<%$resources:Controls,ExcelPrint %>" Visible="false" /><%--Visibility Settings in Page_PreRender--%>
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="57" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnJournalize" CommandName="JOURNALIZE" TabIndex="58"
                                            Text="<%$resources:Journalize %>" OnClick="ActionHandler" ToolTip="<%$resources:Journalize %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-journalize" />
                                    </li>
                                     <li style="display:none;">
                                        <asp:Button runat="server" ID="btnVerification" CommandName="VERIFICATION" TabIndex="58"
                                            Text="<%$resources:Verification %>" OnClick="ActionHandler" ToolTip="<%$resources:Verification %>"
                                              SkinID="btnInner-submit" Visible="false"/><%--CommandArgument="SEC_ActionPanel" SkinID="btnInner-journalize"--%>
                                    </li>
                                    <li id="pnlAlert" runat="server" style="display: none;">
                                        <asp:Button runat="server" ID="btnAlert" CommandName="ALERT" TabIndex="59" Text="<%$resources:Controls,Alert %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:Controls,Alert %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-alert" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li style="display: none;">
                                        <asp:Button runat="server" TabIndex="65" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li id="pnlListPrint" runat="server" visible="false">
                                        <asp:Button runat="server" TabIndex="60" ID="btnPrintLst" CommandName="PRINTCILISTING"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,PrintCI %>" CssClass="btnInner-Print-green-btn" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:Controls,PrintCI %>" OnClientClick="javascript:return SelectedCheckBoxCount(1);" />
                                    </li>
                                    <li id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="60" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelSI %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelSI %>" OnClientClick="javascript:return SelectedCheckBoxCount(1);" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="61" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" OnClientClick="javascript:return SelectedCheckBoxCount(1);" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="62" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" OnClientClick="javascript:return SelectedCheckBoxCount(1);" />
                                    </li>
                                    <li id="liPrintSI">
                                        <asp:Button runat="server" TabIndex="63" ID="btnPrintSI" CommandName="PRINTLISTING"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" OnClientClick="javascript:return SelectedCheckBoxCount(1);" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnPickForReceipt" CommandName="PICKFORRECEIPT" TabIndex="64"
                                            Text="<%$resources:PickInvforReceipt %>" OnClick="ActionHandler" ToolTip="<%$resources:PickInvforReceipt %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-receipt" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnPickForCrDrNote" CommandName="PICKFORCRDRNOTE"
                                            OnClientClick="javascript:return SelectedCheckBoxCount(2);" TabIndex="65" Text="<%$resources:PickForCrDrNote %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:PickForCrDrNote %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-moneycredit" />
                                    </li>
                                    <li runat="server" id="pnlResetSelection">
                                        <asp:Button runat="server" ID="btnResetSelection" CommandName="RESET" TabIndex="66"
                                            Text="<%$resources:ResetSelection %>" OnClick="ActionHandler" ToolTip="<%$resources:ResetSelection %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" OnClientClick="ResetSelection()" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnPOListing" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowSC %>">
                            <asp:LinkButton runat="server" ID="lbnSOListing" Text="<%$resources:PageNameRes,SalesOrder %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="67" CssClass="tab-inactive" OnClick="ActionHandler"
                                CommandName="DEFAULT"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnDeliveryOrder" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowDO %>">
                            <asp:LinkButton runat="server" ID="lnbDeliveryOrder" Text="<%$resources:PageNameRes,DeliveryOrder %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="68" OnClick="ActionHandler" CommandName="DELIVERYORDER"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnSalesInvoice" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowSalesAdvInvoice %>">
                            <asp:LinkButton runat="server" ID="lnbSalesInvoice" Text="<%$resources:PageNameRes,AdvanceInvoice %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="69" OnClick="ActionHandler" CommandName="SALESINVOICE"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnAdvanceInvoice" runat="server" class="tab-active" visible="<%$ resources:ConfigurationsRes,TabShowSalesInvoice %>">
                            <asp:LinkButton runat="server" ID="lnbAdvanceInvoice" Text="<%$resources:PageNameRes,SalesInvoice %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="70" OnClick="ActionHandler" CommandName="INVOICE"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="Spnmiscellaneous" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowMiscInvoice %>">
                            <asp:LinkButton runat="server" ID="lnbMiscellaneous" Text="<%$resources:PageNameRes,miscellaneous %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="71" OnClick="ActionHandler" CommandName="MISC"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnSalesReceipt" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowReceipt %>">
                            <asp:LinkButton runat="server" ID="lbnSalesReceipt" Text="<%$resources:PageNameRes,SalesReceipt %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="72" OnClick="ActionHandler" CommandName="SALESRECEIPT"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnCrDrNote" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowSalesCRDR %>">
                            <asp:LinkButton runat="server" ID="lnbCrDrNote" Text="<%$resources:PageNameRes,CreditDebitNotes %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="73" OnClick="ActionHandler" CommandName="CRDRNOTE"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnAcPayables" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowAR %>">
                            <asp:LinkButton runat="server" ID="lnbAcPayables" Text="<%$resources:PageNameRes,AccountReceivables %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="74" OnClick="ActionHandler" CommandName="ACRECEIVABLE"
                                CssClass="tab-inactive" OnClientClick="javascript:return SelectedCheckBoxCount(0);"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <%--  //For SelectedItemId Keeping--%>
                <asp:HiddenField ID="hdfSelectedItemPk" runat="server" Value="0" />
                <div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="75" OnClick="ActionHandler" CommandName="INVOICELIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="76" OnClick="ActionHandler" CommandName="INVOICEDETAIL"
                                CssClass="tab-inactive" OnClientClick="javascript:return SelectedCheckBoxCount(1);"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkPrintDocs" Text="<%$resources:PageNameRes,PrintInvoiceDocs %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="77" OnClick="ActionHandler" CommandName="PRINTINVOICE"
                                CssClass="tab-inactive" OnClientClick="javascript:return CheckPrintDocs();"></asp:LinkButton>
                            <%----%>
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
                                            <asp:Label ID="lblFrmDate" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtFromDate"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="1" CssClass="input-small-19-11-3"
                                                MaxLength="13" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtToDate"
                                                CssClass="lbl-9perc-19-11-3"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="2" CssClass="input-small" MaxLength="14"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" AssociatedControlID="ddlStatus"
                                                CssClass="lbl-9perc-19-11 margnbotm0"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-b margnbotm0"
                                                TabIndex="15">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,NotPosted %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Posted %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblPlantName" Text="<%$ resources:Controls, CompanyPlant %>"
                                                AssociatedControlID="ddlPlantName" CssClass="lbl-18-3perc-19-11-2"></asp:Label>
                                            <asp:DropDownList ID="ddlPlantName" runat="server" CssClass="select-small-a2" TabIndex="15">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblSCno" runat="server" Text="<%$resources:SONo %>" AssociatedControlID="txtSCno"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox ID="txtSCno" runat="server" CssClass="input-small-a" MaxLength="100"
                                                TabIndex="16"> </asp:TextBox>
                                            <asp:Label ID="lblDrCrNo" runat="server" Text="<%$resources:CrDrNo %>" AssociatedControlID="txtDrCrNo"
                                                CssClass="lbl-24-2perc-19-11-3"></asp:Label>
                                            <asp:TextBox ID="txtDrCrNo" runat="server" CssClass="input-small" MaxLength="100"
                                                TabIndex="17"> </asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblICustomer" runat="server" Text="<%$resources:Customer %>" AssociatedControlID="txtCustomer"
                                                CssClass="middle-lbl margnbotm0"></asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="input-w47-7per margnbotm0"
                                                MaxLength="100" TabIndex="18"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerID" runat="server" />
                                            <asp:Label ID="lblInvoiceNumber" runat="server" Text="<%$resources:InvoiceNumber %>"
                                                AssociatedControlID="txtInvoiceNumber" CssClass="lbl-9perc"></asp:Label>
                                            <asp:TextBox ID="txtInvoiceNumber" runat="server" CssClass="input-small-a margnbotm0"
                                                MaxLength="100" TabIndex="19"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfIVHPK" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label runat="server" ID="lblDue" Text="<%$ resources:DueAson%>" AssociatedControlID="txtDueAson"
                                                CssClass="middle-lbl margnbotm0"></asp:Label>
                                            <asp:TextBox ID="txtDueAson" runat="server" TabIndex="20" CssClass="input-small  margnbotm0"
                                                MaxLength="17" onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:Label runat="server" ID="lblchkBalAmt" Text="<%$ resources:PendingBal%>" AssociatedControlID="chkBalAmt"
                                                CssClass="middle-lbl-xsmall-c2"></asp:Label>
                                            <asp:CheckBox ID="chkBalAmt" runat="server" Checked="true" TabIndex="21" />
                                            <asp:Label ID="lblInType" runat="server" Text="<%$resources:InvoiceType %>" AssociatedControlID="ddlSaleOrderType"
                                                CssClass="middle-lbl-xsmall-b margnbotm0"></asp:Label>
                                            <asp:DropDownList ID="ddlSaleOrderType" runat="server" TabIndex="22" CssClass="select-small-b margnbotm0">
                                            </asp:DropDownList>
                                            <%--<asp:Label ID="lblSearch" runat="server" Width="5px" AssociatedControlID="btnSearch"></asp:Label>--%>
                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="23" CssClass="margntop2 margnbotm0" CommandName="SEARCH"
                                                SkinID="search-ext" />
                                            <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="24" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext"
                                                CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdInvoiceList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                    AllowPaging="true" OnPageIndexChanging="ActionHandler" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" GroupName="SelectOne" AutoPostBack="true"
                                                    OnCheckedChanged="ActionHandler" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chkInvselect" TabIndex="25" OnCheckedChanged="ActionHandler" AutoPostBack="true" CommandName="LISTITEMSELECTED" />
                                                <asp:HiddenField runat="server" ID="hdfInvoiceID" Value='<%# Eval(Resources.DataFieldRes.SalesInvoicePK) %>' />
                                                <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval("ICH_DEPT") %>' />
                                                <asp:HiddenField ID="hdfInvTermList" runat="server" Value='<%# Eval("ICH_INV_TERM_VALUE") %>' />
                                                <asp:HiddenField ID="hdfDelStatus" runat="server" Value='<%# Eval("ICH_DEL_STATUS") %>' />
                                                <asp:HiddenField ID="hdfTaxAmount" runat="server" Value='<%# Eval("ICH_TAX_TC") %>' />
                                                <asp:HiddenField ID="hdfInvDtlCount" runat="server" Value='<%# Eval("ICH_DTL_COUNT") %>' />
                                                <asp:HiddenField ID="hdfInvStatus" runat="server" Value='<%# Eval("ICH_STATUS") %>' />
                                                <asp:HiddenField ID="hdfInOpeningInv" runat="server" Value='<%# Eval("ICH_IS_OPENING") %>' />
                                                <asp:HiddenField ID="hdfIsContainerRelease" runat="server" Value='<%# Eval("ICH_CONT_RELEASED") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvDate %>" SortExpression="<%$ resources:DataFieldRes,SalesInvoiceDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval( Resources.DataFieldRes.SalesInvoiceDate)!=""? Convert.ToDateTime( Eval( Resources.DataFieldRes.SalesInvoiceDate)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SalesInvoiceDate, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvNo %>" SortExpression="<%$ resources:DataFieldRes,SalesInvoiceNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLInvoiceNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.SalesInvoiceNo) ==""?"[NEW]":Eval(Resources.DataFieldRes.SalesInvoiceNo)%>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SalesInvoiceNo)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="<%$ resources:DataFieldRes,SISOdate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPlant" runat="server" CssClass="<%# Eval(Resources.DataFieldRes.CompnayLineColor) %>"
                                                    Text='<%# Eval(Resources.DataFieldRes.CMP_DISPLAY_CODE) %>' ToolTip='<%# Eval(Resources.DataFieldRes.CMP_DISPLAY_CODE)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceType %>" SortExpression="<%$ resources:DataFieldRes,SIType %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLInvoiceType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SIType),3,"") %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SIType)%>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfInvTypeText" Value="<%# Eval(Resources.DataFieldRes.SIType) %>" />
                                                <asp:HiddenField runat="server" ID="hdfInvType" Value='<%# Eval("ICH_TYPE") %>' />
                                                <asp:HiddenField runat="server" ID="hdfSalesContractType" Value='<%# Eval("SOH_TYPE") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Customer %>" SortExpression="<%$ Resources:DataFieldRes,SICustomer%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SICustomer),30) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SICustomer),250) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfCustomerPK" Value='<%# Eval("ICH_CUS_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="23%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SONo %>" SortExpression="<%$ resources:DataFieldRes,SISONo %>">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblSoNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.SISONo) ==""?"[NEW]":Eval(Resources.DataFieldRes.SISONo)%>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SISONo)%>'></asp:Label>--%>
                                                <asp:Label ID="lblSoNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.SISONo) ==""?"[NEW]": ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SISONo),25) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SISONo)  +" "+ "(Cust.PO.No:" +" " + Eval(Resources.DataFieldRes.CustPo) +" " +")" %>'>                                        
                                                </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <%--  <asp:TemplateField>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:SODate %>" SortExpression="<%$ resources:DataFieldRes,SISOdate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSoDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SISOdate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SISOdate, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ETD %>" SortExpression="<%$ resources:DataFieldRes,ICH_ETD %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceETD" runat="server" Text='<%# Eval(Resources.DataFieldRes.ICH_ETD, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.ICH_ETD, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='<%#Eval(Resources.DataFieldRes.SICurrency)  %>'
                                                    ToolTip='<%#Eval(Resources.DataFieldRes.SICurrency)  %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfSOCurrency" Value='<%# Eval("ICH_CURRENCY") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %>" SortExpression="<%$ resources:DataFieldRes,SIValue %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceValue" runat="server" Text='<%# Eval(Resources.DataFieldRes.SIValue,"{0:c}") %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SIValue,"{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BalAmt %>" SortExpression="<%$ resources:DataFieldRes,SalesBalAmt %>">
                                            <ItemTemplate>
                                                <%--   <asp:Label ID="lblBalAmt" runat="server" Text='<%# Eval(Resources.DataFieldRes.SalesBalAmt,"{0:c}")%>'
                                                    ToolTip='<%#Eval(Resources.DataFieldRes.SalesBalAmt,"{0:c}")%>'></asp:Label>--%>
                                                <asp:LinkButton ID="lbnBalAmt" runat="server" Text='<%# Eval(Resources.DataFieldRes.SalesBalAmt,"{0:c}")%>'
                                                    CssClass="text-underline" ToolTip='<%#Eval(Resources.DataFieldRes.SalesBalAmt,"{0:c}")%>'
                                                    OnClick="ActionHandler" CommandName="AMOUNTDETAILS"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:DueDate %>" SortExpression="<%$ resources:DataFieldRes,SalesDueDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDueDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SalesDueDate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SalesDueDate, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="false" />
                                            <ItemStyle Width="6%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("ASC_CSS_CLASS") %>' ToolTip='<%# Eval("ICH_STATUS_TEXT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval(Resources.DataFieldRes.SApproved) %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgPosted" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? GetLocalResourceObject("unposted").ToString() : Eval("FTH_CSS_CLASS")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? Resources.Captions.NotPosted : Eval("FTH_STATUS_TEXT")%>' />
                                                <asp:HiddenField runat="server" ID="hdfPosted" Value='<%# Eval(Resources.DataFieldRes.SPosted) %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="2%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <%-- <uc1:PagerControl ID="uclPaging" runat="server" />--%>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <div class="inv-headr" runat="server" id="div1">
                                <div class="invoice-col">
                                    <asp:Label ID="lblCustomer" runat="server" AssociatedControlID="lblCustomerTxt" Text="<%$resources:HdrCustomer %>"></asp:Label>
                                    <asp:Label ID="lblCustomerTxt" runat="server" Text=""></asp:Label>
                                    <asp:Label ID="lblPageDept" runat="server" Text="<%$ resources:Controls,Department_Col %>"
                                        AssociatedControlID="lblPageDeptText" Visible="false"></asp:Label>
                                    <asp:Label ID="lblPageDeptText" runat="server" Visible="false"></asp:Label>
                                </div>
                                <div class="invoice-col w30perc">
                                    <asp:Label ID="lblSONo" runat="server" AssociatedControlID="lnkDoNo" Text="DO No. :"></asp:Label>
                                    <%--<asp:Label ID="lblSONo" runat="server" AssociatedControlID="lblSONoTxt" Text="<%$resources:HdrSONo %>"></asp:Label>--%>
                                    <asp:LinkButton ID="lnkDoNo" runat="server" CssClass="text-underline bold" OnClick="ActionHandler"
                                        CommandName="SHOWPOPUP"></asp:LinkButton>
                                    <asp:HiddenField ID="hdfDoPk" runat="server" Value="" />
                                    <%-- <asp:Label ID="lblSONoTxt" runat="server" Text=""></asp:Label>--%>
                                    <asp:Label ID="lblSOAmt" runat="server" AssociatedControlID="lblSOAmtTxt" Text="<%$resources:HdrSOAmt %>"
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblSOAmtTxt" runat="server" Text="" Visible="false"></asp:Label>
                                </div>
                                <div class="invoice-col">
                                    <asp:Label ID="lblSODate" runat="server" AssociatedControlID="lblSODateTxt" Text="DO Date :"></asp:Label>
                                    <asp:Label ID="lblSODateTxt" runat="server" Text=""></asp:Label>
                                    <asp:Label ID="lblInvAmt" Visible="false" runat="server" AssociatedControlID="lblInvAmtTxt"
                                        Text="<%$resources:HdrInvAmt %>"></asp:Label>
                                    <asp:Label ID="lblInvAmtTxt" Visible="false" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="invoice-col w20perc">
                                    <asp:Label ID="lblCurrency" runat="server" AssociatedControlID="lblCurrencyTxt" Text="<%$resources:HdrCurrency %>"></asp:Label>
                                    <asp:HiddenField ID="hdfCurrency" runat="server" />
                                    <asp:Label ID="lblCurrencyTxt" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="clear">
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
                                            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                                            <asp:HiddenField ID="hdfRateFormat" runat="server" />
                                            <asp:HiddenField ID="hdfInvoicePK" runat="server" />
                                            <asp:HiddenField ID="hdfExchangeRate" runat="server" />
                                            <asp:HiddenField ID="hdfTaxCategory" runat="server" />
                                            <asp:HiddenField ID="hdfTaxSettings" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdfDetailTax" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdfDetalDiscount" runat="server" Value="0" />
                                            <%--<asp:HiddenField ID="hdfHdrCurrency" runat="server" />--%>
                                            <asp:HiddenField ID="hdfCurrentPk" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdfCurrCustomerPK" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdfCustomerTypeId" runat="server" Value="0" />
                                            <asp:Label ID="lblInvNo" runat="server" Text="<%$ resources:InvoiceNo%>" AssociatedControlID="lblInvoiceNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblInvoiceNo" CssClass="input-small"></asp:Label>
                                            <asp:Button ID="btnRefreshInv" runat="server" OnClick="ActionHandler" Visible="false"
                                                CommandName="RELOADINVDETAILS" SkinID="refresh-invoice" ToolTip="<%$ resources:RefreshInv%>" />
                                            <asp:HiddenField ID="hdfInvoiceNo" runat="server" Value="" />
                                            <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                                            <asp:HiddenField ID="AST_CODE" runat="server" />
                                            <asp:Label runat="server" ID="lblInvoiceDate" Text="<%$ resources:InvoiceDate%>"
                                                AssociatedControlID="txtInvoiceDate" CssClass="middle-lbl-a-20-11-7"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInvoiceDate" CssClass="date-picker" TabIndex="1"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(txtInvoiceDate)"></asp:TextBox>
                                            <div style="display: none">
                                                <asp:Button ID="btnInvoiceDate" runat="server" CommandName="VALIDATEINVOICEDATE"
                                                    OnClick="ActionHandler" />
                                            </div>
                                            <asp:HiddenField ID="hdfInvoiceDate" runat="server" />
                                            <asp:HiddenField ID="hdfCreditDays" Value="0" runat="server" />
                                            <asp:HiddenField ID="hdfHasTax" runat="server" Value="0" />
                                            <asp:RequiredFieldValidator ID="vrfInvoiceDate" CssClass="star" SetFocusOnError="false"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtInvoiceDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_InvoiceDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="vrfTaxDate" CssClass="star" SetFocusOnError="false"
                                                ValidationGroup="taxDate" EnableClientScript="true" runat="server" ControlToValidate="txtInvoiceDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_InvoiceDate %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblInvoiceType" runat="server" AssociatedControlID="ddlInvoiceType"
                                                Text="<%$ resources:InvoiceType %>">
                                            </asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlInvoiceType" AutoPostBack="true" TabIndex="4"
                                                Enabled="false" OnSelectedIndexChanged="ActionHandler" CssClass="select-small-a">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfInvoiceType" runat="server" ControlToValidate="ddlInvoiceType"
                                                Text="*" CssClass="star" InitialValue="-1" ValidationGroup="invoice" EnableClientScript="true"
                                                SetFocusOnError="true" ErrorMessage="<%$ resources:Err_InvoiceType %>"></asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblContainerNo" Text="<%$ resources:ContainerNo%>"
                                                AssociatedControlID="txtContainerNo" CssClass="middle-lbl-small-a-20-11-7"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtContainerNo" Text="" TabIndex="5" MaxLength="200"
                                                CssClass="input-small"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblFeederVessel" Text="<%$ resources:FeederVessel%>"
                                                AssociatedControlID="txtFeederVessel"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFeederVessel" Text="" TabIndex="7" MaxLength="200"
                                                CssClass="input-halfsmall-a"></asp:TextBox>
                                            <%--<asp:Label ID="lblInvType" runat="server" Text="<%$ resources:InvoiceType%>" AssociatedControlID="txtInvoiceType" ></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInvoiceType" CssClass="medium" Enabled ="false"  ></asp:TextBox>--%>
                                            <div class="clear">
                                            </div>
                                            <asp:HiddenField ID="hdfPaymentTerms" runat="server" Value="" />
                                            <asp:Label ID="lblPaymentTerms" runat="server" AssociatedControlID="ddlPaymentTerms"
                                                Text="<%$ resources:PaymentTerms %>">
                                            </asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlPaymentTerms" AutoPostBack="true" TabIndex="9"
                                                OnSelectedIndexChanged="ActionHandler" CssClass="select-half">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfReqTerm" Enabled="false" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" InitialValue="-1" runat="server"
                                                ControlToValidate="ddlPaymentTerms" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Terms %>">
                                            </asp:RequiredFieldValidator>
                                            <%--  Adding Customer Type & Type ID--%>
                                            <asp:Label ID="lblCustomerType" runat="server" AssociatedControlID="ddlCustomerType"
                                                Text="<%$ resources:CustomerType %>">
                                            </asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlCustomerType" CssClass="select-small-g" AutoPostBack="true"
                                                TabIndex="13" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfCustomerType" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="vatbuy" EnableClientScript="true" InitialValue="-1" runat="server"
                                                ControlToValidate="ddlCustomerType" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Type %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:TextBox runat="server" ID="txtTypeID" Text="" CssClass="input-small" TabIndex="14"
                                                MaxLength="5"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfBranchCode" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="vatbuy" EnableClientScript="true" runat="server" ControlToValidate="txtTypeID"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_BranchCode %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblInvoiceGstType" Text="<%$ resources:InvoiceGstType%>"
                                                AssociatedControlID="ddlInvoiceGstType"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlInvoiceGstType" TabIndex="16" CssClass="input-medium">
                                            </asp:DropDownList>
                                            <%--   //Adding Exchange rate--%>
                                            <div class="clear">
                                            </div>
                                            <%--       //End--%>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblETD" Text="<%$ resources:ETD%>" AssociatedControlID="txtETD"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtETD" CssClass="input-small" TabIndex="2" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(txtETD)"></asp:TextBox>
                                            <asp:HiddenField ID="hdfETD" runat="server" />
                                            <asp:CustomValidator ID="cvETD" runat="server" ErrorMessage="<%$ resources:Err_ETDDate%>"
                                                ValidateEmptyText="true" ClientValidationFunction="ValidateETD"
                                                Text="*" EnableClientScript="true" ControlToValidate="txtETD" CssClass="star"
                                                Display="Dynamic" ValidationGroup="invoice"></asp:CustomValidator>

                                            <asp:Label runat="server" ID="lblETA" Text="<%$ resources:ETA%>" AssociatedControlID="txtETA"
                                                CssClass="middle-lbl-small-d-19-11-3"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtETA" CssClass="input-small" TabIndex="3" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(txtETA)"></asp:TextBox>
                                            <asp:HiddenField ID="hdfETA" runat="server" />
                                            <asp:CustomValidator ID="cvETA" runat="server" ErrorMessage="<%$ resources:Err_ETADate%>"
                                                ValidateEmptyText="true" ClientValidationFunction="ValidateETA"
                                                Text="*" EnableClientScript="true" ControlToValidate="txtETA" CssClass="star"
                                                Display="Dynamic" ValidationGroup="invoice"></asp:CustomValidator>

                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblReference" Text="<%$ resources:Reference%>" AssociatedControlID="txtReference"></asp:Label>
                                            <asp:TextBox ID="txtReference" runat="server" TabIndex="6" CssClass="input-half-20-11-8"></asp:TextBox>
                                            <asp:HiddenField ID="hdfType" runat="server" Value="" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblMotherVessel" Text="<%$ resources:MotherVessel%>"
                                                AssociatedControlID="txtMotherVessel"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtMotherVessel" Text="" TabIndex="8" MaxLength="200"
                                                CssClass="input-half-20-11-8"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblInvoiceDueDate" Text="<%$ resources:InvoiceDueDate%>"
                                                AssociatedControlID="txtInvoiceDueDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInvoiceDueDate" CssClass="date-picker" TabIndex="10"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(null)"></asp:TextBox>
                                            <asp:HiddenField ID="hdfInvoiceDueDate" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfInvoiceDueDate" CssClass="star" SetFocusOnError="false"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtInvoiceDueDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_InvoiceDueDate %>">
                                            </asp:RequiredFieldValidator>
                                            <%-- Invoice due Details Popup--%>
                                            <div style="position: absolute; width: 18px; display: inline;">
                                                <asp:Button ID="btnShowDueDetails" Style="margin-top: 2px;" runat="server" OnClick="ActionHandler"
                                                    TabIndex="11" CommandName="SHOWDUEDETAILS" SkinID="adjustallocation-icon" ToolTip="Invoice Due Date Details" />
                                            </div>
                                            <%--      Adding TaxID -------------------------------%>
                                            <asp:Label ID="lblExchangeRate" runat="server" Text="<%$ resources:Controls,ExchangeRate %>"
                                                AssociatedControlID="txtExchangeRate" CssClass="middle-lbl-small-d-19-11-3" />
                                            <asp:TextBox ID="txtExchangeRate" runat="server" TabIndex="12" MaxLength="20" CssClass="input-small numeric"
                                                onkeypress="return isFloatNumberKey(event);" onchange="AfterExchangeRate()" />
                                            <asp:Button ID="btnExchangeRate" runat="server" EnableTheming="false" Style="display: none"
                                                OnClick="ActionHandler" CommandName="CHANGEEXRATE" />
                                            <asp:RequiredFieldValidator ID="vrfExchangeRate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtExchangeRate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExchangeRate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cmpExchangeRate" CssClass="star" SetFocusOnError="true"
                                                Type="Double" Operator="GreaterThan" ValueToCompare="0" ValidationGroup="invoice"
                                                EnableClientScript="true" InitialValue="0" runat="server" ControlToValidate="txtExchangeRate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExchangeRate %>">
                                            </asp:CompareValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblTaxID" Text="<%$ resources:TaxId%>" AssociatedControlID="txtTaxID"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTaxID" Text="" ToolTip="" TabIndex="15" MaxLength="100"
                                                CssClass="input-half-20-11-8"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblDeliveryTerms" Text="<%$ resources:DeliveryTerms%>"
                                                AssociatedControlID="txtDeliveryTerms"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDeliveryTerms" Text="" ToolTip="" TabIndex="16"
                                                MaxLength="100"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblCompanyView" Visible="false" runat="server" Text="<%$ resources:Controls, CompanyPlant %>"
                                                AssociatedControlID="ddlCompanyView"></asp:Label>
                                            <asp:DropDownList ID="ddlCompanyView" Visible="false" Enabled="false" runat="server"
                                                CssClass="select-small-a">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblSubType" Text="<%$ resources:SubType%>" AssociatedControlID="ddlSubType"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlSubType" TabIndex="16" CssClass="input-medium">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <asp:Button ID="btnRecalculate" runat="server" EnableTheming="false" Style="display: none"
                                    OnClick="ActionHandler" CommandName="RECALCULATE" />
                                <asp:GridView ID="grdInvoice" runat="server" AutoGenerateColumns="False" Width="100%"
                                    AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                    ShowFooter="true" OnRowDataBound="ActionHandler" PageSize="<%$ resources:PageSize%>">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:SONo %>" SortExpression="<%$ resources:DataFieldRes,SISONo %>">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkSoNo" CssClass="text-underline" runat="server" OnClick="ActionHandler"
                                                    CommandName="SHOW" Text='' ToolTip='' CommandArgument=''></asp:LinkButton>
                                                <%--  <asp:Label ID="lblSoNo" runat="server" Text='' ToolTip=''  CommandArgument=''></asp:Label>--%>
                                                <%--   <asp:Label ID="lblPlt" runat="server" Text='<%# Eval(Resources.DataFieldRes.CMP_DISPLAY_CODE) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.CMP_DISPLAY_CODE) %>'></asp:Label>--%>
                                                <asp:Label ID="lblPlt" runat="server" Text='<%#GetFormattedString((Eval("CID_SO_NO").ToString()),( Convert.ToString(Eval(Resources.DataFieldRes.CMP_DISPLAY_CODE))),20) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.CMP_DISPLAY_CODE) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" Wrap="false" />
                                        </asp:TemplateField>
                                        <%--   <asp:TemplateField>
                                                                <ItemTemplate>
                                                                     <asp:Label ID="lblPlt" runat="server" Text='<%# Eval(Resources.DataFieldRes.CMP_DISPLAY_CODE) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.CMP_DISPLAY_CODE) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="1%" />
                                                            </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Product %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfSODPK" runat="server" Value='<%#Eval("CID_SO_DTL") %>' />
                                                <asp:HiddenField ID="hdfSODtlPK" runat="server" Value='<%#Eval("CID_SO") %>' />
                                                <asp:HiddenField ID="hdfInvoiceDtlPK" runat="server" Value='<%#Eval("CID_PK") %>' />
                                                <asp:HiddenField ID="hdfInvoiceDtlDummyPK" runat="server" Value='<%#Eval("CID_SL_UK") %>' />
                                                <asp:HiddenField ID="hdfItemPK" runat="server" Value='<%#Eval("CID_ITEM") %>' />
                                                <%--<asp:Label ID="lblItem" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("CID_ITEM_TEXT"),18) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("CID_ITEM_TEXT").ToString()) %>'></asp:Label>--%>
                                                <%-- <asp:Label ID="lblItem" runat="server" Text='<%# HttpUtility.HtmlDecode(Eval("CID_CUST_ITEM_TEXT").ToString()) %>'
                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("CID_CUST_ITEM_TEXT").ToString()) %>'></asp:Label>--%>
                                                <%--      <asp:Label ID="lblItem" runat="server" Text='<%# HttpUtility.HtmlDecode(Eval("CID_CUST_ITEM_TEXT").ToString()) %>'
                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("CID_CUST_ITEM_TEXT").ToString()) %>'></asp:Label>--%>
                                                <asp:Label ID="lblItem" runat="server" Text='<%# Eval("SOD_IS_PACK_MAT").ToString() == "1" || Eval("SOD_IS_PACK_MAT").ToString()=="2" ?
                                                   ERP.Utilities.CommonFunctions.GetShortString(Eval("CID_ITEM_TEXT"),300)
                                                  : ERP.Utilities.CommonFunctions.GetShortString(Eval("CID_CUST_ITEM_TEXT"),300) %>'
                                                    ToolTip='<%# Eval("SOD_IS_PACK_MAT").ToString() == "1" ||Eval("SOD_IS_PACK_MAT").ToString()=="2" ?
                                                     ERP.Utilities.CommonFunctions.GetShortString(Eval("CID_ITEM_TEXT"),300) :
                                                    HttpUtility.HtmlDecode(Convert.ToString(Eval("CID_CUST_ITEM_TEXT"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="27%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Rate %>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRate" runat="server" Text='<%#GetFormattedRate(Eval("CID_RATE")) %>'
                                                    CssClass="input-w70 numeric input-disabled" MaxLength="14" ToolTip='<%#GetFormattedRate(Eval("CID_RATE")) %>'
                                                    OnTextChanged="ActionHandler" Enabled="false" AutoPostBack="true"></asp:TextBox>
                                                <asp:HiddenField ID="hdfRate" runat="server" Value='<%#Eval("CID_RATE") %>' />
                                                <asp:RequiredFieldValidator ID="vrfRate" CssClass="star" SetFocusOnError="true" ValidationGroup="invoice"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtRate" Display="Dynamic"
                                                    Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                </asp:RequiredFieldValidator>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:UoM %>">
                                            <ItemTemplate>
                                                <%-- <asp:Label ID="lblUoM" runat="server" Text='<%#Eval("CID_UOM_TEXT") %>' ToolTip='<%# HttpUtility.HtmlDecode(Eval("CID_UOM_TEXT").ToString()) %>'></asp:Label>
                                                <asp:HiddenField ID="hdfUoM" runat="server" Value='<%#Eval("CID_UOM") %>' />--%>
                                                <asp:Label ID="lblUoM" runat="server" Text='<%#Eval("CID_SALE_UOM_TEXT") %>' ToolTip='<%# HttpUtility.HtmlDecode(Eval("CID_SALE_UOM_TEXT").ToString()) %>'></asp:Label>
                                                <asp:HiddenField ID="hdfUoM" runat="server" Value='<%#Eval("CID_UOM") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:OrderQuantity%>">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblOrderQuantity" runat="server" CssClass="ItemQuantity" Text='<%#GetFormattedNumber(Eval("CID_ORDERED_QTY")) %>'
                                                    ToolTip='<%#GetFormattedNumber(Eval("CID_ORDERED_QTY")) %>'></asp:Label>--%>
                                                <asp:Label ID="lblOrderQuantity" runat="server" CssClass="ItemQuantity" Text='<%#GetFormattedNumberWithComma(Eval("CID_ORDERED_QTY")) %>'
                                                    ToolTip='<%#GetFormattedNumberWithComma(Eval("CID_ORDERED_QTY")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvQuantity%>">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblInvQuantity" runat="server" CssClass="ItemQuantity" Text='<%#GetFormattedNumber(Eval("CID_INV_QTY")) %>'
                                                    ToolTip='<%#GetFormattedNumber(Eval("CID_INV_QTY")) %>'></asp:Label>--%>
                                                <asp:Label ID="lblInvQuantity" runat="server" CssClass="ItemQuantity" Text='<%#GetFormattedNumberWithComma(Eval("CID_INV_QTY")) %>'
                                                    ToolTip='<%#GetFormattedNumberWithComma(Eval("CID_INV_QTY")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvProformaQuantity%>">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblInvProformaQuantity" runat="server" CssClass="ItemProformaQuantity"
                                                    Text='<%#GetFormattedNumber(Eval("CID_PINV_QTY")) %>' ToolTip='<%#GetFormattedNumber(Eval("CID_PINV_QTY")) %>'></asp:Label>--%>
                                                <asp:Label ID="lblInvProformaQuantity" runat="server" CssClass="ItemProformaQuantity"
                                                    Text='<%#GetFormattedNumberWithComma(Eval("CID_PINV_QTY")) %>' ToolTip='<%#GetFormattedNumberWithComma(Eval("CID_PINV_QTY")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvNow%>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtInvNowSales" runat="server" Text='<%#GetFormattedNumber(Eval("CID_SALE_QTY")) %>'
                                                    CssClass="input-w70 numeric" MaxLength="13" OnTextChanged="ActionHandler" AutoPostBack="true"
                                                    TabIndex="23"></asp:TextBox>
                                                <asp:HiddenField ID="hdfUomConv" runat="server" Value='<%#Eval("CID_SALE_UOM_CONV") %>' />
                                                <asp:HiddenField ID="hdfInvNowSales" runat="server" />
                                                <asp:HiddenField ID="hdfUoMSales" runat="server" Value='<%#Eval("CID_SALE_UOM") %>' />
                                                <asp:Label ID="lblInvNowMFSSales" runat="server" Visible="false" Text='<%#GetFormattedNumber(Eval("CID_SALE_QTY")) %>'></asp:Label>
                                                <asp:RequiredFieldValidator ID="vrfInvNowSales" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtInvNowSales"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                </asp:RequiredFieldValidator>
                                                <cc1:QuantityValidation ID="vreInvNowSales" runat="server" ControlToValidate="txtInvNowSales"
                                                    NumberDigits="9" ErrorMessage="<%$ resources:Err_Invalid_InvNow %>" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"></cc1:QuantityValidation>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvNowPcs%>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtInvNow" runat="server" Text='<%#GetFormattedNumber(Eval("CID_INV_QTY_NOW")) %>'
                                                    CssClass="input-w70 numeric" MaxLength="13" OnTextChanged="ActionHandler" AutoPostBack="true"
                                                    TabIndex="23"></asp:TextBox>
                                                <asp:HiddenField ID="hdfInvNow" runat="server" />
                                                <asp:Label ID="lblInvNowMFS" runat="server" Visible="false" Text='<%#GetFormattedNumber(Eval("CID_INV_QTY_NOW")) %>'></asp:Label>
                                                <asp:RequiredFieldValidator ID="vrfInvNow" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtInvNow"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                </asp:RequiredFieldValidator>
                                                <cc1:QuantityValidation ID="vreInvNow" runat="server" ControlToValidate="txtInvNow"
                                                    NumberDigits="9" ErrorMessage="<%$ resources:Err_Invalid_InvNow %>" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"></cc1:QuantityValidation>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("CID_AMOUNT")) %>'
                                                    CssClass="input-w70 numeric input-disabled" MaxLength="15" Enabled="false" ToolTip='<%#GetFormattedCurrency(Eval("CID_AMOUNT")) %>'></asp:TextBox>
                                                <asp:HiddenField ID="hdfAmount" runat="server" Value='<%#Eval("CID_AMOUNT") %>' />
                                                <asp:RequiredFieldValidator ID="vrfAmount" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtAmount"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                </asp:RequiredFieldValidator>
                                                <cc1:AmountValidation ID="vamAmount" runat="server" ControlToValidate="txtAmount"
                                                    ErrorMessage="<%$ resources:Err_Invalid_Amount %>" NumberDigits="11" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"></cc1:AmountValidation>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Discount %>">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                    ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')"
                                                    TabIndex="24" ToolTip="<%$ resources:Controls,Discounts %>" CommandName="DISCDETAILS" />
                                                <asp:TextBox ID="txtDiscount" runat="server" Text='<%#GetFormattedCurrency(Eval("CID_DISCOUNT")) %>'
                                                    CssClass="input-w70 numeric input-disabled" MaxLength="15" ToolTip='<%#GetFormattedCurrency(Eval("CID_DISCOUNT")) %>'
                                                    Enabled="false"></asp:TextBox>
                                                <asp:HiddenField ID="hdfDiscount" runat="server" />
                                                <asp:RequiredFieldValidator ID="vrfDiscount" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtDiscount"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Discount %>">
                                                </asp:RequiredFieldValidator>
                                                <cc1:AmountValidation ID="vamDiscount" runat="server" ControlToValidate="txtDiscount"
                                                    ErrorMessage="<%$ resources:Err_Invliad_Discount %>" NumberDigits="11" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"></cc1:AmountValidation>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" CssClass="amount-numeric" Wrap="false" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Tax %>">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                    ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')"
                                                    TabIndex="25" ToolTip="<%$ resources:Tax %>" CommandName="TAXDETAILS" />
                                                <asp:TextBox ID="txtTax" runat="server" Text='<%#GetFormattedCurrency(Eval("CID_TAX")) %>'
                                                    CssClass="input-w70 numeric input-disabled" MaxLength="15" ToolTip='<%#GetFormattedCurrency(Eval("CID_TAX")) %>'
                                                    Enabled="false"></asp:TextBox>
                                                <asp:HiddenField ID="hdfTax" runat="server" />
                                                <asp:HiddenField ID="hdrOrgTax" Value='<%#GetFormattedCurrency(Eval("CID_TAX")) %>'
                                                    runat="server" />
                                                <asp:RequiredFieldValidator ID="vrfTax" CssClass="star" SetFocusOnError="true" ValidationGroup="invoice"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtTax" Display="Dynamic"
                                                    Text="*" ErrorMessage="<%$ resources:Err_Tax %>">
                                                </asp:RequiredFieldValidator>
                                                <cc1:AmountValidation ID="vamTax" runat="server" ControlToValidate="txtTax" ErrorMessage="<%$ resources:Err_Invliad_Tax %>"
                                                    NumberDigits="11" Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star"
                                                    ValidationGroup="invoice"></cc1:AmountValidation>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" CssClass="amount-numeric" Wrap="false" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblfooter" Text="<%$ resources:SubTotal %>"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Total %>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTotal" runat="server" CssClass="input-w70 numeric input-disabled"
                                                    MaxLength="15" Text='<%#GetFormattedCurrency(Eval("CID_NET_AMOUNT")) %>' ToolTip='<%#GetFormattedCurrency(Eval("CID_NET_AMOUNT")) %>'
                                                    Enabled="false"></asp:TextBox>
                                                <asp:HiddenField ID="hdfTotal" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:TextBox runat="server" ID="txtSubTotalFooter" CssClass="input-w70 numeric input-disabled"
                                                    Enabled="false" AutoPostBack="true"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <%--Remove--%>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div id="divCalc">
                                <div class="gridwrap">
                                    <table id="tblCalc" class="gridwraptable gridwrap">
                                        <tr>
                                            <td style="width: 45%; padding: 2px 2px 0px 3px;">
                                                <%----------------------------------CategoryWise Total Start-------------------------------------%>
                                                <div id="divCategoryWiseTotal" class="w60perc floatLeft" style="display: none; max-height: 36px; overflow-y: auto;">
                                                    <asp:GridView ID="grdCategoryWiseTotal" runat="server" AutoGenerateColumns="False"
                                                        Width="100%" ShowHeader="false" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable"
                                                        AllowSorting="false" Style="margin-top: 0px!important;">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <%--HeaderText="<%$ resources:Category %>"--%>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItemCategory" runat="server" Text='<%# HttpUtility.HtmlDecode(Eval("CID_ITEM_CATEGORY_TEXT").ToString()) %>'
                                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval("CID_ITEM_CATEGORY_TEXT").ToString()) %>'></asp:Label>
                                                                    <asp:HiddenField ID="hdfItemCategoryPk" runat="server" Value='<%#Eval("CID_ITEM_CATEGORY") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="65%" BackColor="White" Font-Size="10px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <%--HeaderText="<%$ resources:Total %>"--%>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdCurrencyText" runat="server" Text='<%#Eval("CURRENCY_TEXT") %>'
                                                                        ToolTip='<%#Eval("CURRENCY_TEXT") %>' CssClass="disp-inline"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" CssClass="txt-rgt" />
                                                                <%-- <ItemStyle CssClass="currencycolumn" BackColor="White" Font-Size="10px" Width="30%" />--%>
                                                                <%--<HeaderStyle />--%>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <%--HeaderText="<%$ resources:Total %>"--%>
                                                                <ItemTemplate>
                                                                    <%--<asp:Label ID="lblgdCurrencyText" runat="server" Text='<%#Eval("CURRENCY_TEXT") %>'
                                                                        ToolTip='<%#Eval("CURRENCY_TEXT") %>' CssClass="disp-inline"></asp:Label>--%>
                                                                    <asp:Label ID="lblCategoryTotal" runat="server" CssClass="currencycolumn" Text='<%#(GetFormattedCurrencyWithComa(Eval("CID_NET_AMOUNT"))) %>'
                                                                        ToolTip='<%#GetFormattedCurrencyWithComa(Eval("CID_NET_AMOUNT")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="txt-rgt" BackColor="White" Font-Size="10px" Width="20%" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                                <%--------------------------------- End CategoryWise Total Region---------------------------------%>
                                            </td>
                                            <td style="text-align: right; width: 40%;">
                                                <asp:Label runat="server" ID="lblDiscount" Text="<%$ resources:Discount%>" AssociatedControlID="txtHdrDiscount"
                                                    CssClass="margnbotm0"></asp:Label>
                                            </td>
                                            <td style="text-align: right; width: 13%;" class="btn-margin">
                                                <asp:ImageButton ID="imgHdrDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                    ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')"
                                                    TabIndex="18" ToolTip="<%$ resources:Controls,Discounts %>" CommandName="DISCHEADER" />
                                                <asp:TextBox ID="txtHdrDiscount" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    MaxLength="16" Enabled="false"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfDiscount" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtHdrDiscount"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TotalDiscount%>"></asp:RequiredFieldValidator>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtTerms" runat="server" MaxLength="100" TabIndex="19"></asp:TextBox>
                                                <asp:Label runat="server" ID="Label1" Text="<%$ resources:GrossTotal%>" AssociatedControlID="txtHdrTotal"
                                                    CssClass="margnbotm0"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtHdrTotal" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    MaxLength="16" Enabled="false"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfHdrTotal" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtHdrTotal"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Gross_Total%>"></asp:RequiredFieldValidator>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trDeduction">
                                            <td></td>
                                            <td style="text-align: right; width: 40%;">
                                                <asp:Label runat="server" ID="lblDeduction" Text="<%$ resources:Deduction%>" AssociatedControlID="txtHdrDeduction"
                                                    CssClass="margnbotm0"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgHdrDeduction" SkinID="allocation" runat="server" OnClick="ActionHandler"
                                                    ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')"
                                                    TabIndex="20" ToolTip="Deduction" CommandName="DEDUCTIONHEADER" CssClass="margnrgt0-7per" />
                                                <asp:TextBox ID="txtHdrDeduction" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    MaxLength="16" Enabled="false"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfDeduction" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtHdrDeduction"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TotalDeduction%>"></asp:RequiredFieldValidator>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trDiscDeducted">
                                            <td></td>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblDiscDeducted" Text="<%$ resources:DedDisc%>" AssociatedControlID="txtDiscDeducted"
                                                    CssClass="margnbotm0"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtDiscDeducted" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    TabIndex="21" MaxLength="16" Text="0.00" Enabled="false"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtHdrBalBeforeVat"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_BalBeforeVat%>"></asp:RequiredFieldValidator>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="Label2" Text="<%$ resources:Balance%>" AssociatedControlID="txtHdrBalBeforeVat"
                                                    CssClass="margnbotm0"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtHdrBalBeforeVat" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    TabIndex="22" MaxLength="16" Enabled="false"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfHdrBalBeforeVat" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtHdrBalBeforeVat"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_BalBeforeVat%>"></asp:RequiredFieldValidator>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblShipping" Text="<%$ resources:OtherCharges%>" AssociatedControlID="txtShipping"
                                                    CssClass="margnbotm0"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgShippingCharge" SkinID="shipping" runat="server" OnClick="ActionHandler"
                                                    TabIndex="23" ToolTip="<%$ resources:OtherCharges %>" CommandName="SHIPPINGHEADER" />
                                                <asp:TextBox ID="txtShipping" runat="server" CssClass="input-w80 numeric  input-disabled"
                                                    MaxLength="16" Enabled="false" TabIndex="24"></asp:TextBox>
                                                <%--<asp:TextBox ID="txtShipping" runat="server" CssClass="input-w80 numeric" TabIndex="31"
                                                    onchange="CalculateTotal(this);" MaxLength="16"></asp:TextBox>--%>
                                                <asp:RequiredFieldValidator ID="vrfShipping" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtShipping"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Shipping%>"></asp:RequiredFieldValidator>
                                                <cc1:AmountValidation ID="vamShipping" runat="server" ControlToValidate="txtShipping"
                                                    ErrorMessage="<%$ resources:Err_Invalid_Shipping %>" NumberDigits="12" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"></cc1:AmountValidation>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <%--  Deduct : Other charge recieved   --%>
                                        <tr runat="server" id="trdor">
                                            <td></td>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lbldor" Text="<%$ resources:DOR%>" AssociatedControlID="txtPriceAdj"
                                                    CssClass="margnbotm0"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtdor" runat="server" Text="0.00" CssClass="input-w80 numeric input-disabled"
                                                    TabIndex="25" onchange="CalculateTotal(this);" MaxLength="16"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <%---END Deduct : Other charge recieved--%>
                                        <tr>
                                            <td></td>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblTax" Text="<%$ resources:Tax%>" AssociatedControlID="txtHdrTax"
                                                    CssClass="margnbotm0"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgHdrTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                    ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')"
                                                    TabIndex="26" ToolTip="<%$ resources:Tax %>" CommandName="TAXHEADER" />
                                                <asp:TextBox ID="txtHdrTax" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    Enabled="false" MaxLength="16"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfTax" CssClass="star" SetFocusOnError="true" ValidationGroup="invoice"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtHdrTax" Display="Dynamic"
                                                    Text="*" ErrorMessage="<%$ resources:Err_TotalTax%>"></asp:RequiredFieldValidator>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="trAdvAdjDed" runat="server">
                                            <td></td>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblAdvAdjDed" Text="<%$ resources:AmountAdvAdjDed%>"
                                                    AssociatedControlID="txtAdvAdjustDeductAmount" CssClass="margnbotm0"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtAdvAdjustDeductAmount" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    TabIndex="27" MaxLength="16"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblPriceAdj" Text="<%$ resources:AmountAdj%>" AssociatedControlID="txtPriceAdj"
                                                    CssClass="margnbotm0"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtPriceAdj" runat="server" CssClass="input-w80 numeric" TabIndex="27"
                                                    onchange="CalculateTotal(this);" MaxLength="16"></asp:TextBox>
                                                <asp:HiddenField ID="hdfPriceAdj" Value="0" runat="server" />
                                                <%--<asp:RequiredFieldValidator ID="vrfPriceAdj" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtPriceAdj"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PriceAdj%>"></asp:RequiredFieldValidator>--%>
                                                <cc1:AmountValidation ID="vamPriceAdj" runat="server" ControlToValidate="txtPriceAdj"
                                                    ErrorMessage="<%$ resources:Err_Invalid_PriceAdj %>" NumberDigits="12" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"
                                                    AllowNegative="true"></cc1:AmountValidation>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trTotalExp">
                                            <td></td>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblTotalExp" Text="<%$ resources:total%>" AssociatedControlID="txtTotalExp"
                                                    CssClass="margnbotm0"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtTotalExp" runat="server" Text="0.00" CssClass="input-w80 numeric input-disabled"
                                                    TabIndex="28" MaxLength="16"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trTotalDedExp">
                                            <td></td>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblTotalDeductionExp" Text="<%$ resources:TotalDeductionExp%>"
                                                    AssociatedControlID="txtTotalDeductionExp" CssClass="margnbotm0"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgAllocateExp" SkinID="allocation" runat="server" OnClick="ActionHandler"
                                                    ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')"
                                                    TabIndex="29" ToolTip="Deduction" CommandName="DEDUCTIONHEADER" CssClass="margnrgt0-7per" />
                                                <asp:TextBox ID="txtTotalDeductionExp" runat="server" Text="0.00" CssClass="input-w80 numeric input-disabled"
                                                    TabIndex="30" MaxLength="16"></asp:TextBox><%--onchange="CalculateTotal(this);"--%>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtTotalDeductionExp"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TotalDeduction%>"></asp:RequiredFieldValidator>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="floatLeft">
                                                    <asp:Label runat="server" ID="lblInvoiceTotal" Text="<%$ resources:Invoice_Total%>"
                                                        AssociatedControlID="txtHdrInvoiceTotal" CssClass="margnbotm0"></asp:Label>
                                                    <asp:TextBox ID="txtHdrInvoiceTotal" runat="server" CssClass="input-w80 numeric input-disabled"
                                                        Enabled="false" MaxLength="16"></asp:TextBox>
                                                    <asp:TextBox ID="txtTotalTerms" runat="server" TabIndex="31" MaxLength="100"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblTotal" Text="<%$ resources:Net_Receivable%>" AssociatedControlID="txtHdrNetTotal"
                                                    CssClass="margnbotm0"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtHdrNetTotal" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    Enabled="false" MaxLength="16"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>

                                        <div id="divCustomsCharge" runat="server">

                                            <tr>
                                                <td></td>
                                                <td style="text-align: right">
                                                    <asp:Label runat="server" ID="label" Text="<%$ resources:OtherCustomesCharges%>"
                                                        AssociatedControlID="txtcustOtherCharge" CssClass="margnbotm0"></asp:Label>
                                                </td>
                                                <td style="text-align: right" class="btn-margin">
                                                    <asp:ImageButton ID="ImgOtherCustomesCharges" SkinID="shipping" runat="server" OnClick="ActionHandler"
                                                        TabIndex="23" ToolTip="<%$ resources:OtherCustomesCharges %>" CommandName="CUSTOMSOTHERCHARGESHEADER" />
                                                    <asp:TextBox ID="txtcustOtherCharge" runat="server" CssClass="input-w80 numeric  input-disabled"
                                                        MaxLength="16" Enabled="false" TabIndex="24"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtcustOtherCharge"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Shipping%>"></asp:RequiredFieldValidator>
                                                    <cc1:AmountValidation ID="AmountValidation1" runat="server" ControlToValidate="txtcustOtherCharge"
                                                        ErrorMessage="<%$ resources:Err_Invalid_Shipping %>" NumberDigits="12" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"></cc1:AmountValidation>
                                                    <div class="clear">
                                                    </div>
                                                </td>
                                            </tr>

                                        </div>

                                    </table>
                                </div>
                            </div>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblPortofLoading" Text="<%$ resources:PortofLoading%>"
                                                AssociatedControlID="txtPortofLoading"></asp:Label>
                                            <asp:TextBox ID="txtPortofLoading" CssClass="input-half input-disabled" runat="server"
                                                MaxLength="100" Enabled="false"></asp:TextBox>
                                            <asp:HiddenField ID="hdfPortofLoading" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblFinalDest" CssClass="fineldest-22-03-2021" Text="<%$ resources:FinalDest%>" AssociatedControlID="txtFinalDest"></asp:Label>
                                            <asp:TextBox ID="txtFinalDest" CssClass="input-half-dest-22-03-2021 input-disabled" runat="server"
                                                MaxLength="100" Enabled="false"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblShippingMark" Text="<%$ resources:ShippingMark%>"
                                                AssociatedControlID="txtShippingMark" CssClass="input-w24-9per-3-19"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtShippingMark" CssClass="input-half-mark"
                                                onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" TabIndex="32"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="divcol-S">
                                            <asp:Label runat="server" CssClass="shipmentterms-label" ID="lblShipmentTerms" Text="<%$ resources:ShipmentTerms%>"
                                                AssociatedControlID="txtShipmentTerms"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtShipmentTerms" CssClass="input-half-22-03-2021 input-disabled"
                                                onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" TabIndex="32" Enabled="false"></asp:TextBox>
                                            <asp:HiddenField ID="hdfShippingTerms" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdfShippingTermValue" runat="server" Value="0" />

                                            <asp:Label runat="server" ID="lblEffectDate" CssClass="ibl-effectdate-label" Text="<%$ resources:EffectDate%>"
                                                AssociatedControlID="txtEffectDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtEffectDate" CssClass="input-small-22-03-2021 input-disabled"
                                                onkeydown="return CheckKey(event)" onpaste="return false;" TabIndex="32" Enabled="false"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblSpecialNotes" Text="<%$ resources:SpecialNotes%>"
                                                AssociatedControlID="txtSpecialNotes"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSpecialNotes" TextMode="MultiLine" CssClass="multiline-2line"
                                                TabIndex="33"></asp:TextBox>
                                            <%-- onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"--%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks%>" AssociatedControlID="txtRemarks"></asp:Label>
                                            <asp:TextBox ID="txtRemarks" runat="server" TabIndex="34" MaxLength="500" TextMode="MultiLine"
                                                CssClass="multiline-2line"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <%--    Invoice Terms Remarks Region Start      --%>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblInvoiceTerms" Text="<%$ resources:InvoiceTermsRemarks%>"
                                                AssociatedControlID="ddlInvoiceTerms"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlInvoiceTerms" AutoPostBack="true" TabIndex="35"
                                                CssClass="select-w20" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtInvoiceTermRemarks" runat="server" TabIndex="35" onkeydown="limitText(this,500);"
                                                onchange="limitText(this,500);" TextMode="MultiLine" CssClass="multiline-2line input-w49-7per"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <%--   End Invoice Terms remarks region          --%>
                            </table>
                            <%-- file upload start here--%>
                            <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                <h1>
                                    <%= GetLocalResourceObject("Fileupload_Details").ToString()%></h1>
                                <div class="button-wrap-right ">
                                    <asp:ImageButton runat="server" ID="imbShowDetails" OnClientClick="javascript:return ShowHideUploadDocDetails(1);"
                                        ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                        TabIndex="65" />
                                    <asp:ImageButton runat="server" ID="imbHideDetails" OnClientClick="javascript:return ShowHideUploadDocDetails();"
                                        ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                        TabIndex="66" />
                                </div>
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <table class="table-devide" id="tblUploadDocDetails">
                                        <tr>
                                            <td colspan="2">
                                                <div class="divcol-S">
                                                    <asp:Label ID="lblFileUpload" runat="server" Text="AttachFile" AssociatedControlID="fupUpload"></asp:Label>
                                                    <div class="fileupload-main">
                                                        <asp:FileUpload ID="fupUpload" runat="server" CssClass="margn-rgt0 upload-area" TabIndex="26" />
                                                        <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupUpload"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_File_Upload %>">                                                        
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                    <a id="anchorFile" runat="server" target="_blank" tabindex="11"></a>
                                                    <asp:Button runat="server" ID="btnUpload" CommandName="ADDITEMUPLOAD" TabIndex="12"
                                                        OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('upload')"
                                                        ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="PageAction_Entry" ValidationGroup="upload"
                                                        Text="<%$resources:ErpRes,Add %>" SkinID="btnInner-add" />
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="gridwrap">
                                                    <asp:GridView runat="server" ID="grdUploads" Width="95%" PageSize="<%$ resources:PageSize%>"
                                                        AllowSorting="false" AllowPaging="false" OnSorting="ActionHandler" OnPageIndexChanging="ActionHandler"
                                                        OnRowDataBound="ActionHandler" AutoGenerateColumns="false" TabIndex="8" EmptyDataRowStyle-CssClass="emptytable">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ resources:SlNo %>">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                    <asp:HiddenField runat="server" ID="hdfPK" Value='<%# Eval("DOC_PK") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="2%" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:File %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFile" runat="server" Text='<%# Eval("DOC_NAME") %>' ToolTip='<%# Eval("DOC_NAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="92%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <a runat="server" id="fileView" class="download-icon nomargin" title="<%$ resources:View %>"
                                                                        target="_blank" href='<%# Page.ResolveClientUrl(Eval("DOC_PATH").ToString()) %>'></a>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="2%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="lnkEdit" runat="server" OnClick="ActionHandler" CommandName="EDITITEMUPLOAD"
                                                                        SkinID="edit-icon" ToolTip="Edit" CommandArgument="PageAction_Entry" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="2%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEMUPLOAD"
                                                                        SkinID="delete-icon" ToolTip="Delete" CommandArgument="PageAction_Entry" OnClientClick="return ShowDeleteConfirm(this);" /><%--OnLoad="btnAction_Load" OnPreRender="btnAction_PreRender"--%>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="2%" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <%-- file upload end here--%>
                            <div id="divItemTax" style="display: none">
                                <div class="Button-container-popup">
                                    <asp:Button ID="btnApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply" OnClick="ActionHandler"
                                        TabIndex="100" CommandName="TAXAPPLY" CommandArgument="PageAction_Entry" />
                                </div>
                                <div class="content-wrapper">
                                    <%-- *********************Checkbox Region Start*******************************************************************--%>
                                    <div id="divTaxApplicableAmount" visible="false" runat="server">
                                        <label for="chkSubTotal" style="width: 130px">
                                            <%=Resources.Controls.SubTotal%>
                                        </label>
                                        <asp:CheckBox ID="chkSubTotal" runat="server" Checked="false" OnCheckedChanged="ActionHandler"
                                            AutoPostBack="true" />
                                        <label for="chkDiscount" style="width: 130px">
                                            <%=Resources.Controls.Discounts%>
                                        </label>
                                        <asp:CheckBox ID="chkDiscount" runat="server" Checked="false" OnCheckedChanged="ActionHandler"
                                            AutoPostBack="true" />
                                        <label for="chkOtherCharges" style="width: 145px">
                                            <%=Resources.Controls.OtherCharges%>
                                        </label>
                                        <asp:CheckBox ID="chkOtherCharges" runat="server" Checked="true" OnCheckedChanged="ActionHandler"
                                            AutoPostBack="true" />
                                    </div>
                                    <%--   *******************End Check Box Region ****************************************************************--%>
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:HiddenField ID="hdfTaxFormula" runat="server" />
                                                    <asp:HiddenField ID="hdfTaxCode" runat="server" />
                                                    <asp:HiddenField ID="hdfTaxRate" runat="server" />
                                                    <asp:Label ID="lblPopupItemAmount" runat="server" Text="<%$ resources:ItemAmount %>"
                                                        AssociatedControlID="txtPopupItemAmount"></asp:Label>
                                                    <asp:TextBox ID="txtPopupItemAmount" CssClass="input-w70 numeric" runat="server"
                                                        EnableViewState="false" Enabled="false" MaxLength="11" TabIndex="101"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblPopupAmount" runat="server" Text="<%$ resources:Charges %>" AssociatedControlID="txtPopupAmount"></asp:Label>
                                                    <asp:TextBox ID="txtPopupAmount" TabIndex="103" runat="server" CssClass="input-w70 numeric"
                                                        EnableViewState="false" Enabled="false" MaxLength="11"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfTaxSlNo" Value="-1" runat="server" />
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfTaxAmt" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="tax" EnableClientScript="true" runat="server" ControlToValidate="txtPopupAmount"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                        </asp:RequiredFieldValidator>
                                                        <cc1:AmountValidation ID="vreTaxAmt" runat="server" ControlToValidate="txtPopupAmount"
                                                            ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="tax"></cc1:AmountValidation>
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-P">
                                                    <div id="divTax" runat="server">
                                                        <asp:Label ID="lblPopupTaxType" runat="server" Text="<%$ resources:TaxType %>" AssociatedControlID="ddlPopupTaxType"></asp:Label>
                                                        <asp:DropDownList ID="ddlPopupTaxType" TabIndex="102" runat="server" CssClass="medium"
                                                            EnableViewState="true" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <div class="clear">
                                                        </div>
                                                    </div>
                                                    <asp:Label ID="lblPopupOther" runat="server" Text="<%$ resources:TaxName %>" AssociatedControlID="txtPopupOther"></asp:Label>
                                                    <asp:TextBox ID="txtPopupOther" runat="server" TabIndex="104" CssClass="medium" EnableViewState="false"
                                                        MaxLength="100" Enabled="false"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfPopupOther" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="tax" EnableClientScript="true" runat="server" ControlToValidate="txtPopupOther"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TaxName %>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:ImageButton ID="imgPopupAdd" SkinID="imbaddnew" runat="server" OnClick="ActionHandler"
                                                        CommandArgument="PageAction_Entry" TabIndex="105" ValidationGroup="tax" ToolTip="Add"
                                                        CommandName="TAXADD" OnClientClick="javascript:ValidatePageNow('tax')" />
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdTaxDetails" Width="100%" AllowSorting="false"
                                            AutoGenerateColumns="false" TabIndex="106" EmptyDataRowStyle-CssClass="emptytable"
                                            PageSize="<%$ resources:PageSize%>">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:TaxType %>">
                                                    <ItemTemplate>
                                                        <%--<asp:HiddenField ID="hdfInvoicePK" runat="server" Value='<%#Eval("RRD_PK") %>' />--%>
                                                        <asp:HiddenField ID="hdfTaxSplitPK" runat="server" Value='<%#Eval("CIT_PK") %>' />
                                                        <asp:HiddenField ID="hdfTaxPK" runat="server" Value='<%#Eval("CIT_TAX") %>' />
                                                        <%--<asp:HiddenField ID="hdfTaxSlNo" runat="server" Value='<%#Eval("RRD_PK") %>' />--%>
                                                        <%-- POT_SL_NO
                                                POT_PK
                                                POT_TAX--%>
                                                        <asp:Label ID="lblTaxText" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("CIT_TAX_TEXT")) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("CIT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("CIT_TAX_TEXT"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:TaxName %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxName" runat="server" Text='<%#Eval("CIT_NAME") %>' ToolTip='<%# HttpUtility.HtmlDecode(Eval("CIT_NAME").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTaxName" runat="server" Value='<%#Eval("CIT_NAME") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Amount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrencyWithComa(Eval("CIT_TAX_AMT")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithComa(Eval("CIT_TAX_AMT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imbTaxRemove" runat="server" OnClick="ActionHandler" CommandName="TAXDELETE"
                                                            CommandArgument="PageAction_Entry" OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load"
                                                            TabIndex="53" SkinID="btnclose" ToolTip="Remove" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgTaxEdit" runat="server" OnClick="ActionHandler" CommandName="TAXEDIT"
                                                            CommandArgument="PageAction_Entry" TabIndex="53" SkinID="edit-row" ToolTip="Edit" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>

                            <div id="DivCustOtherChagres" style="display: none">
                                <div class="Button-container-popup">
                                    <asp:Button ID="btncustChargeApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply"
                                        OnClick="ActionHandler" TabIndex="100" CommandName="CUSTOMSOTHERCHRGAPPLY" CommandArgument="PageAction_Entry" />
                                </div>
                                <div class="content-wrapper">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                                    <asp:HiddenField ID="HiddenField2" runat="server" />
                                                    <asp:HiddenField ID="HiddenField3" runat="server" />
                                                    <asp:Label ID="lbldescription" runat="server" Text="<%$ resources:Description %>" AssociatedControlID="txtdescription"></asp:Label>
                                                    <asp:TextBox ID="txtdescription" runat="server"
                                                        MaxLength="50" TabIndex="101"></asp:TextBox>
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfCustDescription" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="Customs" EnableClientScript="true" runat="server" ControlToValidate="txtdescription"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_CustomsDesc %>">
                                                        </asp:RequiredFieldValidator>
                                                        <%--  <cc1:AmountValidation ID="vreCustDescription" runat="server" ControlToValidate="txtdescription"
                                                            ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="Customs"></cc1:AmountValidation>--%>
                                                    </div>
                                                    <div class="clear">
                                                    </div>

                                                </div>
                                            </td>
                                            <td class="div2col-P1">
                                                <asp:Label ID="lblcustothercharges" runat="server" Text="<%$ resources:Charges %>" AssociatedControlID="txtcustcharges"></asp:Label>
                                                <asp:TextBox ID="txtcustcharges" TabIndex="103" runat="server" CssClass="input-w70-11-11-2 numeric"
                                                    EnableViewState="false" Enabled="True" onkeypress="return isFloatNumberKey(event);" MaxLength="11"></asp:TextBox>
                                                <asp:HiddenField ID="HiddenField4" Value="-1" runat="server" />
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="vrfCustomsAmt" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="Customs" EnableClientScript="true" runat="server" ControlToValidate="txtcustcharges"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                    </asp:RequiredFieldValidator>
                                                    <cc1:AmountValidation ID="vreCustomsAmt" runat="server" ControlToValidate="txtcustcharges"
                                                        ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="Customs"></cc1:AmountValidation>
                                                    <asp:ImageButton ID="imbAddCustCharge" SkinID="imbaddnew" runat="server" OnClick="ActionHandler"
                                                        CommandArgument="PageAction_Entry" TabIndex="105" ValidationGroup="Customs" ToolTip="Add"
                                                        CommandName="CUSTCHARGE" OnClientClick="javascript:ValidatePageNow('Customs')" />
                                                </div>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdCustOtherCharges" Width="100%" AllowSorting="false"
                                            AutoGenerateColumns="false" TabIndex="106" EmptyDataRowStyle-CssClass="emptytable"
                                            PageSize="<%$ resources:PageSize%>">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:Description %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDesc" runat="server" Text='<%#Eval("ICC_TYPE_NAME") %>' ToolTip='<%# HttpUtility.HtmlDecode(Eval("ICC_TYPE_NAME").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfDesc" runat="server" Value='<%#Eval("ICC_TYPE_NAME") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Charges %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCharges" runat="server" Text='<%#GetFormattedCurrencyWithComa(Eval("ICC_AMT")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithComa(Eval("ICC_AMT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imbDescRemove" runat="server" OnClick="ActionHandler" CommandName="CUSTDELETE"
                                                            TabIndex="53" SkinID="btnclose" ToolTip="Remove" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5%" />
                                                </asp:TemplateField>
                                                <%-- <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgTaxEdit" runat="server" OnClick="ActionHandler" CommandName="TAXEDIT"
                                                            CommandArgument="PageAction_Entry" TabIndex="53" SkinID="edit-row" ToolTip="Edit" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5%" />
                                                </asp:TemplateField>--%>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <%-- customes other charges end here--%>
                            <div id="divDeduction" style="display: none">
                                <div class="Button-container-popup">
                                    <asp:Button ID="btnSaveDuduction" SkinID="btnInner-add-dsd" runat="server" Text="Apply"
                                        OnClick="ActionHandler" TabIndex="107" CommandName="DEDUCTIONAPPLY" CommandArgument="PageAction_Entry"
                                        ValidationGroup="deduction" OnClientClick="javascript:ValidatePageNow('deduction')" />
                                </div>
                                <div class="content-wrapper">
                                    <div class="detail-co2">
                                        <div class="div2col-S">
                                            <asp:Label ID="Label5" runat="server" Text="<%$ resources:InvoiceNo1%>" AssociatedControlID="lblDedInvoiceNo"
                                                Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblDedInvoiceNo" runat="server"></asp:Label>
                                            <asp:Label ID="Label10" runat="server" Text="<%$ resources:InvoiceDate1 %>" AssociatedControlID="lblDedSaleInvoiceDate"
                                                Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblDedSaleInvoiceDate" runat="server"></asp:Label>
                                        </div>
                                        <div class="div2col-S">
                                            <asp:Label ID="Label8" runat="server" Text="<%$ resources:Customer1 %>" AssociatedControlID="lblDedCustomer"
                                                Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblDedCustomer" runat="server"></asp:Label>
                                            <asp:Label ID="Label6" runat="server" Text="<%$ resources:Currency1 %>" AssociatedControlID="lblDedCurrency"
                                                Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblDedCurrency" runat="server"></asp:Label>
                                        </div>
                                        <div style="display: none">
                                            <asp:Label ID="Label4" runat="server" Text="<%$ resources:SoDate1 %>" AssociatedControlID="lblDedSaleOrderDate"
                                                Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblDedSaleOrderDate" runat="server"></asp:Label>
                                            <asp:Label ID="Label3" runat="server" Text="<%$ resources:SoNo1 %>" AssociatedControlID="lblDedSaleOrderNo"
                                                Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblDedSaleOrderNo" runat="server"></asp:Label>
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </div>
                                    <%--     checkbox --%>
                                    <div style="display: none">
                                        <asp:CheckBox ID="chkDedAll" runat="server" Text="All" TabIndex="8" AutoPostBack="true"
                                            OnCheckedChanged="ActionHandler" />
                                    </div>
                                    <div class="gridwrap  ">
                                        <asp:GridView ID="grdDeduction" runat="server" AutoGenerateColumns="False" Width="140%"
                                            PageSize="<%$ resources:PageSize %>" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable"
                                            AllowSorting="false" ShowFooter="true" OnRowDataBound="ActionHandler">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:InvoiceNo2 %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDedInvoiceNo" runat="server" Text='<%#Eval("ICH_NO") %>' ToolTip='<%#Eval("ICH_NO") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfDedInvoicePK" runat="server" Value='<%#Eval("IAD_INVOICE_ADV") %>' />
                                                        <asp:HiddenField ID="hdfDeductionPK" runat="server" Value='<%#Eval("IAD_PK") %>' />
                                                        <asp:HiddenField ID="hdfDedSoPk" runat="server" Value='<%#Eval("IAD_PK") %>' />
                                                        <asp:HiddenField ID="hdfPaidOtherAmount" runat="server" Value='<%#Eval("RCH_SO_RCVD_OTHER_AMT") %>' />
                                                        <asp:HiddenField ID="hdfPaidTax" runat="server" Value='<%#Eval("RCH_SO_RCVD_TAX_AMT") %>' />
                                                        <asp:HiddenField ID="hdfpaidDisc" runat="server" Value='<%#Eval("ICM_DISCOUNT_AMOUNT") %>' />
                                                        <asp:HiddenField ID="hdfCurPaidOtherAmount" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hdfCurPaidTax" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hdfCurPaidDisc" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hdfScPk" runat="server" Value='<%#Eval("IAD_SO") %>' />
                                                        <asp:HiddenField ID="hdfIcmSoHdr" runat="server" Value='<%#Eval("ICM_SO_HDR") %>' />
                                                        <%-- //For identifying  Current Invoice Dedution--%>
                                                        <asp:HiddenField ID="hdfCusAdvFlag" runat="server" Value='<%#Eval("CUS_ADV_FLAG") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="8%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:InvoiceDate2 %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfAdvInvoicePK" runat="server" Value='<%#Eval("IAD_INVOICE_ADV") %>' />
                                                        <asp:Label ID="lblDedInvoiceDate" runat="server" Text='<%#Eval("ICH_DATE") %>' ToolTip='<%#Eval("ICH_DATE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="7%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ReceiptNo %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDedReceiptNo" runat="server" Text='<%#Eval("RCH_NO") %>' ToolTip='<%#Eval("RCH_NO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="9%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ReceiptDate %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfReceiptPK" runat="server" Value='<%#Eval("IAD_RECEIPT_HDR") %>' />
                                                        <asp:HiddenField ID="hdfReceiptDTLPK" runat="server" Value='<%#Eval("RCM_PK") %>' />
                                                        <asp:Label ID="lblDedReceiptDate" runat="server" Text='<%#Eval("RCH_DATE") %>' ToolTip='<%#Eval("RCH_DATE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="7%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Reference %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReference" runat="server" Text='<%#Eval("ICH_REFERENCE") %>' ToolTip='<%#Eval("ICH_REFERENCE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:TotalAmount %>" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <%-- <asp:Label ID="lblDedInvoiceAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("ICH_AMOUNT_TC")) %>'
                                                            ToolTip='<%#GetFormattedCurrency(Eval("ICH_AMOUNT_TC")) %>'></asp:Label>--%>
                                                        <asp:Label ID="lblDedInvoiceAmount" runat="server" Text='<%#GetFormattedCurrencyWithComa(Eval("ICH_AMOUNT_TC")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithComa(Eval("ICH_AMOUNT_TC")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="9%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:InvoiceAmount %>" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <%-- <asp:Label ID="lblDedTaxAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("ICH_AMOUNT_NET_TC")) %>'
                                                            ToolTip='<%#GetFormattedCurrency(Eval("ICH_AMOUNT_NET_TC")) %>'></asp:Label>--%>
                                                        <asp:Label ID="lblDedTaxAmount" runat="server" Text='<%#GetFormattedCurrencyWithComa(Eval("ICH_AMOUNT_NET_TC")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithComa(Eval("ICH_AMOUNT_NET_TC")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="8%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ReceiptAmount %>" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <%-- <asp:Label ID="lblDedReceiptAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("RCH_SO_RCVD_AMT")) %>'
                                                            ToolTip='<%#GetFormattedCurrency(Eval("RCH_SO_RCVD_AMT")) %>'></asp:Label>--%>
                                                        <asp:Label ID="lblDedReceiptAmount" runat="server" Text='<%#GetFormattedCurrencyWithComa(Eval("RCH_SO_RCVD_AMT")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithComa(Eval("RCH_SO_RCVD_AMT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="9%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField HeaderText="<%$ resources:Tax %>" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDedTaxAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("RCM_TAX_AMOUNT")) %>'
                                                            ToolTip='<%#GetFormattedCurrency(Eval("RCM_TAX_AMOUNT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="7%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="<%$ resources:Allocated %>" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <%--<asp:Label ID="lblDedInvoiceAllocated" runat="server" Text='<%#GetFormattedCurrency(Eval("ICH_AMOUNT_ALLOCATED")) %>'
                                                            ToolTip='<%#GetFormattedCurrency(Eval("ICH_AMOUNT_ALLOCATED")) %>'></asp:Label>--%>
                                                        <asp:Label ID="lblDedInvoiceAllocated" runat="server" Text='<%#GetFormattedCurrencyWithComa(Eval("ICH_AMOUNT_ALLOCATED")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithComa(Eval("ICH_AMOUNT_ALLOCATED")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="8%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Balance %>" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <%--<asp:Label ID="lblDedInvoiceBal" runat="server" CssClass="BalancetoAllocate" Text='<%#GetFormattedCurrency(( Convert.ToDecimal(Eval("ICH_AMOUNT_NET_TC").ToString())< Convert.ToDecimal(Eval("RCH_SO_RCVD_AMT").ToString())?Convert.ToDecimal(Eval("ICH_AMOUNT_NET_TC").ToString()):Convert.ToDecimal(Eval("RCH_SO_RCVD_AMT").ToString()))-Convert.ToDecimal(Eval("ICH_AMOUNT_ALLOCATED").ToString())) %>'
                                                            ToolTip='<%#GetFormattedCurrency(( Convert.ToDecimal(Eval("ICH_AMOUNT_NET_TC").ToString())< Convert.ToDecimal(Eval("RCH_SO_RCVD_AMT").ToString())?Convert.ToDecimal(Eval("ICH_AMOUNT_NET_TC").ToString()):Convert.ToDecimal(Eval("RCH_SO_RCVD_AMT").ToString()))-Convert.ToDecimal(Eval("ICH_AMOUNT_ALLOCATED").ToString())) %>'></asp:Label>--%>
                                                        <asp:Label ID="lblDedInvoiceBal" runat="server" CssClass="BalancetoAllocate" Text='<%#GetFormattedCurrencyWithComa(( Convert.ToDecimal(Eval("ICH_AMOUNT_NET_TC").ToString())< Convert.ToDecimal(Eval("RCH_SO_RCVD_AMT").ToString())?Convert.ToDecimal(Eval("ICH_AMOUNT_NET_TC").ToString()):Convert.ToDecimal(Eval("RCH_SO_RCVD_AMT").ToString()))-Convert.ToDecimal(Eval("ICH_AMOUNT_ALLOCATED").ToString())) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithComa(( Convert.ToDecimal(Eval("ICH_AMOUNT_NET_TC").ToString())< Convert.ToDecimal(Eval("RCH_SO_RCVD_AMT").ToString())?Convert.ToDecimal(Eval("ICH_AMOUNT_NET_TC").ToString()):Convert.ToDecimal(Eval("RCH_SO_RCVD_AMT").ToString()))-Convert.ToDecimal(Eval("ICH_AMOUNT_ALLOCATED").ToString())) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="9%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:AllocateNow %>" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <%-- <asp:TextBox ID="txtDedAllocateNowSplit" runat="server" CssClass="Uiinput-amount numeric"
                                                            TabIndex="108" MaxLength="15" Width="90%" onkeyup="CalculateTotalSplit(this);"
                                                            Text='<%# GetFormattedCurrency(Convert.ToDecimal(Eval("IAD_AMOUNT")) > 0 ? Eval("IAD_AMOUNT") : (Convert.ToDecimal(Eval("RCH_SO_RCVD_AMT").ToString())-Convert.ToDecimal(Eval("ICH_AMOUNT_ALLOCATED").ToString())))%>'>
                                                        </asp:TextBox>--%>
                                                        <asp:TextBox ID="txtDedAllocateNowSplit" runat="server" CssClass="Uiinput-amount numeric"
                                                            TabIndex="108" MaxLength="15" Width="90%" onkeyup="CalculateTotalSplit(this);"
                                                            Text="0">
                                                        </asp:TextBox>
                                                        <asp:HiddenField ID="hdfDedAllocateNowSplit" runat="server" />
                                                        <div class="starwrap">
                                                            <cc1:AmountValidation ID="vreDedAllocateNowSplit" runat="server" ControlToValidate="txtDedAllocateNowSplit"
                                                                ErrorMessage="<%$ resources:Err_InvalidAllocation %>" NumberDigits="11" Display="Dynamic"
                                                                Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="deduction"></cc1:AmountValidation>
                                                            <asp:CustomValidator ID="customQty" runat="server" ValidateEmptyText="true" ClientValidationFunction="CheckAllocation"
                                                                ErrorMessage="<%$ resources:Err_InvalidAllocation %>" Text="*" EnableClientScript="true"
                                                                ControlToValidate="txtDedAllocateNowSplit" CssClass="star" Display="Dynamic"
                                                                ValidationGroup="deduction"></asp:CustomValidator>
                                                        </div>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblDedTotalAllocateNowFooterSplit"></asp:Label>
                                                        <asp:HiddenField runat="server" ID="hdfDedTotalAllocateNowFooterSplit" />
                                                        <asp:HiddenField runat="server" ID="hdfOtherTotalFooterSplit" />
                                                        <asp:HiddenField runat="server" ID="hdfTaxTotalFooterSplit" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:SOotherChrg %>">
                                                    <ItemTemplate>
                                                        <%--<asp:Label ID="lblOthercharges" Text="0.00" runat="server"></asp:Label>--%>
                                                        <asp:TextBox ID="txtOtherAmountSplit" runat="server" CssClass="Uiinput-amount numeric"
                                                            onkeyup="CalculateTaxOCTotalSplit(this);" Text='<%#GetFormattedCurrency(Convert.ToDecimal(Eval("IAD_OTHER_AMOUNT").ToString()))%>'></asp:TextBox>
                                                        <asp:HiddenField ID="hdfDedOtherChargeSplitBalance" runat="server" Value='<%#GetFormattedCurrency(Convert.ToDecimal(Eval("RCH_SO_RCVD_OTHER_AMT").ToString()) - Convert.ToDecimal(Eval("ICH_OTHER_AMT_ALLOCATED").ToString()))%>' />
                                                        <cc1:AmountValidation ID="vreDedOtherChargeNowSplit" runat="server" ControlToValidate="txtOtherAmountSplit"
                                                            ErrorMessage="<%$ resources:Err_InvalidAllocation %>" NumberDigits="11" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="deduction"></cc1:AmountValidation>
                                                        <asp:CustomValidator ID="customOtherCharge" runat="server" ValidateEmptyText="true"
                                                            ClientValidationFunction="CheckAllocationOtherCharge" ErrorMessage="<%$ resources:Err_InvalidAllocation %>"
                                                            Text="*" EnableClientScript="true" ControlToValidate="txtOtherAmountSplit" CssClass="star"
                                                            Display="Dynamic" ValidationGroup="deduction"></asp:CustomValidator>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" Width="10%" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblOtherchargesFooter"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Tax %>" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <%--<asp:Label ID="lblTax" Text="0.00" runat="server"></asp:Label>--%>
                                                        <asp:TextBox ID="txtTaxSplit" runat="server" CssClass="Uiinput-amount numeric input-disabled"
                                                            onkeyup="CalculateTaxOCTotalSplit(this);" Text='<%#GetFormattedCurrency(Convert.ToDecimal(Eval("IAD_TAX_AMOUNT").ToString()))%>'></asp:TextBox>
                                                        <asp:HiddenField ID="hdfDedTaxSplitBalance" runat="server" Value='<%#GetFormattedCurrency(Convert.ToDecimal(Eval("RCH_SO_RCVD_TAX_AMT").ToString()) - Convert.ToDecimal(Eval("ICH_TAX_AMT_ALLOCATED").ToString()))%>' />
                                                        <cc1:AmountValidation ID="vreDedTaxNowSplit" runat="server" ControlToValidate="txtTaxSplit"
                                                            ErrorMessage="<%$ resources:Err_InvalidAllocation %>" NumberDigits="11" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="deduction"></cc1:AmountValidation>
                                                        <asp:CustomValidator ID="customTax" runat="server" ValidateEmptyText="true" ClientValidationFunction="CheckAllocationTax"
                                                            ErrorMessage="<%$ resources:Err_InvalidAllocation %>" Text="*" EnableClientScript="true"
                                                            ControlToValidate="txtTaxSplit" CssClass="star" Display="Dynamic" ValidationGroup="deduction"></asp:CustomValidator>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5%" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalTaxFooter"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <div id="divConfirmationWithReason" style="display: none">
                                <div class="content-wrapper">
                                    <div class="divcol-P">
                                        <h5>
                                            <%= GetLocalResourceObject("DeleteConfirmation").ToString() %></h5>
                                        <div class="clear">
                                        </div>
                                        <asp:Label ID="lblReason" Text="<%$ resources:Reason %>" runat="server" AssociatedControlID="txtReason"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtReason" TextMode="MultiLine" CssClass="multiline-3col"
                                            onkeydown="limitText(this,450);" onchange="limitText(this,450);"></asp:TextBox>
                                        <asp:Label ID="lbl" runat="server" AssociatedControlID="lbl"></asp:Label>
                                        <asp:Button ID="btnDeleteOK" runat="server" Text="<%$ resources:Ok %>" SkinID="btnInner-ok"
                                            OnClick="ActionHandler" CommandName="INACTIVE" CommandArgument="SEC_ActionPanel" />
                                        <asp:Button ID="btnDeleteCancel" runat="server" Text="<%$ resources:Cancel %>" SkinID="btnInner-Cancel"
                                            OnClientClick="return closeDeletePopup();" />
                                    </div>
                                </div>
                            </div>
                            <%-- -------Start Other Charge Popup --------------------------%>
                            <div id="divOtherchargeSplitUp" style="display: none">
                                <div class="content-wrapper">
                                    <div class="Button-container-popup">
                                        <asp:Button ID="btnSavePaymentSplit" runat="server" Text="<%$ resources:Controls,Apply %>"
                                            OnClick="ActionHandler" CommandName="OTHERCHARGEAPPLY" SkinID="btnInner-add-dsd"
                                            CommandArgument="Allocation_Section" ValidationGroup="Valothercharge" OnClientClick="javascript:ValidatePageNow('Valothercharge')" />
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
                                                        <%-- <asp:TemplateField HeaderText="<%$ resources:TaxType %>">
                                                            <ItemTemplate>                                                               
                                                                <asp:Label ID="lblOtherChargeType" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("CIT_TAX_TEXT")) %>'
                                                                    ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("CIT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("CIT_TAX_TEXT"))) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="35%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:TaxName %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOtherchargeName" runat="server" Text='<%#Eval("CIT_NAME") %>' ToolTip='<%# HttpUtility.HtmlDecode(Eval("CIT_NAME").ToString()) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="35%" />
                                                        </asp:TemplateField>      --%>
                                                        <asp:TemplateField HeaderText="<%$ resources:ScNo %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOtherChargeSCNO" runat="server" Text='<%#Eval("CIT_SO_NO")%>'></asp:Label>
                                                                <asp:HiddenField ID="hdfSCPK" runat="server" Value='<%#Eval("CIT_SO")%>' />
                                                                <asp:HiddenField ID="hdfSCOtherchargePK" runat="server" Value='<%#Eval("CIT_PK") %>' />
                                                                <asp:HiddenField ID="hdfTaxPK" runat="server" Value='<%#Eval("CIT_TAX") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" />
                                                            <HeaderStyle Width="15%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:SODate %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSCDate" runat="server" Text='<%#DateTime.Parse(Eval("CIT_SOH_DT").ToString()).ToString(Resources.Constants.DateFormatShort)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                            <HeaderStyle Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:TaxName %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOtherChargeName" runat="server" Text='<%#Eval("CIT_NAME")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" />
                                                            <HeaderStyle Width="15%" />
                                                        </asp:TemplateField>
                                                        <%--<asp:TemplateField HeaderText="<%$ resources:FOB %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFOB" runat="server" Text='<%#Eval("CIT_TAX_IS_FOB_CAL").ToString() == "1" ? GetLocalResourceObject("Excluded").ToString() : GetLocalResourceObject("Included").ToString() %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                            <HeaderStyle Width="10%" />
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField HeaderText="<%$ resources:FOB %>">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkFob" runat="server" Checked='<%# Convert.ToBoolean(Convert.ToInt32(Eval("CIT_IS_FOB"))) %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="5%" />
                                                            <HeaderStyle Width="5%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Total %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSCTotalOtherCharge" runat="server" Text='<%#GetFormattedCurrencyWithComa(Eval("CIT_SO_TAX_AMT"))%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="amount-numeric" Width="10%" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <asp:Label runat="server" ID="lblTotalOtherCharge"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceAmount %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSCInvdAmt" runat="server" Text='<%#GetFormattedCurrencyWithComa(Eval("CIT_INV_TAX_AMT"))%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="amount-numeric" Width="15%" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <asp:Label runat="server" ID="lblTotalInvOtherCharge"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Balance %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBalanceSplit" CssClass="BalancetoPay" runat="server" Text='<%#GetFormattedCurrencyWithComa(Convert.ToDecimal(Eval("CIT_SO_TAX_AMT").ToString())-Convert.ToDecimal(Eval("CIT_INV_TAX_AMT").ToString()))%>'></asp:Label>
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
                                                                    onkeyup="CalculateTotalOtherCharge();" MaxLength="16" Text='<%#GetFormattedCurrency(Eval("CIT_TAX_AMT"))%>'>
                                                                </asp:TextBox>
                                                                <asp:CustomValidator ID="customvalOtherCharge" runat="server" ValidateEmptyText="true"
                                                                    ClientValidationFunction="ValidationCheckOtherCharge" ErrorMessage="<%$ resources:Err_InvalidOthercharge%>"
                                                                    Text="*" EnableClientScript="true" ControlToValidate="txtAdjustNowAmount" CssClass="star"
                                                                    Display="Dynamic" ValidationGroup="Valothercharge"></asp:CustomValidator>
                                                                <asp:RequiredFieldValidator ID="vrfPayNowSplit" CssClass="star" SetFocusOnError="true"
                                                                    ValidationGroup="Valothercharge" EnableClientScript="true" runat="server" ControlToValidate="txtAdjustNowAmount"
                                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_AdjNowAmount %>">
                                                                </asp:RequiredFieldValidator>
                                                                <cc1:AmountValidation ID="vreOtherCharge" runat="server" ControlToValidate="txtAdjustNowAmount"
                                                                    ErrorMessage="<%$ resources:Err_InvalidAdjNowAmount %>" NumberDigits="12" Display="Dynamic"
                                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="Valothercharge"></cc1:AmountValidation>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" />
                                                            <HeaderStyle CssClass="amount-numeric" Width="15%" />
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
                            <%--Print popup window --%>
                            <pc1:PrinterControl ID="PrinterControl1" runat="server" />
                            <%-- ------- TAX Payable in MYR--------------------------%>
                            <div id="divOuterTaxPayable">
                                <h1 class="search-colapse-normal">
                                    <%=Resources.Controls.TaxPayable%>
                                    <img id="imgShowTaxPayable" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                                        alt="<%= Resources.Controls.Show%>" title="<%= Resources.Controls.Show%>" style="display: none; cursor: pointer"
                                        onclick="javascript:ShowTaxPayable();" />
                                    <img id="imgHideTaxPayable" src="../Images/Classic/Icons/arrow-colapse-active.png"
                                        alt="<%= Resources.Controls.Hide%>" title="<%= Resources.Controls.Hide%>" style="cursor: pointer"
                                        onclick="javascript:HideTaxPayable();" />
                                </h1>
                                <div id="divTaxPayable" class="gridwrap">
                                    <asp:GridView runat="server" ID="grdTaxPayable" Width="100%" AllowSorting="false"
                                        OnRowDataBound="ActionHandler" AutoGenerateColumns="false" TabIndex="42" EmptyDataRowStyle-CssClass="emptytable">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:TaxType %>" Visible="false">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdfTaxSplitPK" runat="server" Value='<%#Eval("CIT_PK") %>' />
                                                    <asp:HiddenField ID="hdfTaxPK" runat="server" Value='<%#Eval("CIT_TAX") %>' />
                                                    <asp:Label ID="lblTaxText" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("CIT_TAX_TEXT")) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("CIT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("CIT_TAX_TEXT"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:TaxCode %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxCode" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_CODE")) == string.Empty ?  string.Empty : Convert.ToString(Eval("CIT_TAX_CODE")) %>'
                                                        ToolTip='<%# Convert.ToString(Eval("CIT_TAX_CODE")) == string.Empty ?  string.Empty : HttpUtility.HtmlDecode(Eval("CIT_TAX_CODE").ToString()) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="25%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:TaxName %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxName" runat="server" Text='<%#Eval("CIT_NAME") %>' ToolTip='<%# HttpUtility.HtmlDecode(Eval("CIT_NAME").ToString()) %>'></asp:Label>
                                                    <asp:HiddenField ID="hdfTaxName" runat="server" Value='<%#Eval("CIT_NAME") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="25%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:TaxRate %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxRate" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_RATE")) == string.Empty ? string.Empty : Convert.ToString(Eval("CIT_TAX_RATE")) %>'
                                                        ToolTip='<%# (Convert.ToString(Eval("CIT_TAX_RATE")) == string.Empty ? string.Empty : Convert.ToString(Eval("CIT_TAX_RATE")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:SubTotal %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmountBeforeTax" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_CID_AMOUNT")) == string.Empty ? string.Empty : GetFormattedCurrencyWithComa(Eval("CIT_TAX_CID_AMOUNT")) %>'
                                                        ToolTip='<%# (Convert.ToString(Eval("CIT_TAX_CID_AMOUNT")) == string.Empty ? HttpUtility.HtmlDecode(Eval("CIT_TAX_CID_AMOUNT").ToString()) : GetFormattedCurrencyWithComa(Eval("CIT_TAX_CID_AMOUNT")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:TaxAmount %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrencyWithComa(Eval("CIT_TAX_AMT")) %>'
                                                        ToolTip='<%#GetFormattedCurrencyWithComa(Eval("CIT_TAX_AMT")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="40%" HorizontalAlign="Right" />
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
                                                        <asp:Label ID="lblTrxNo" runat="server" Text='<%# Convert.ToString(Eval("RCH_NO")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="36%" />
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalText" runat="server" Text="<%$ resources:Total %>"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate" runat="server" Text='<%#  Eval("RCH_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("RCH_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                            ToolTip='<%# Eval("RCH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="14%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:TrxAmount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInvCrAmount" runat="server" Text='<%#GetFormattedCurrencyWithComa(Eval("INV_AMOUNT")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithComa(Eval("INV_AMOUNT")) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfInvCrAmountSplit" runat="server" Value='<%#Eval("INV_AMOUNT") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="25%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalInvCrAmountSplit" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ReceivedAmt %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmount" runat="server" Text='<%#GetFormattedCurrencyWithComa(Eval("RCVD_AMOUNT")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithComa(Eval("RCVD_AMOUNT")) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfAmountSplit" runat="server" Value='<%#Eval("RCVD_AMOUNT") %>' />
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
                                                <tr align="left" style="color: #122648; background-color: #DEE3ED; border-width: 0px; font-family: Verdana; font-size: 10px; font-weight: bold; height: 30px;">
                                                    <td>
                                                        <asp:Label ID="lblBalPay" runat="server" Text='<%$ resources:BalanceAmount %>'></asp:Label>
                                                    </td>
                                                    <td></td>
                                                    <td></td>
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
                            <%---------------------------------Start Invoice Due Date details Popup-----------------------------------%>
                            <div id="divDueDatePopup" style="display: none">
                                <div class="content-wrapper">
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdDueDateDetails" Width="100%" AllowSorting="false"
                                            AutoGenerateColumns="false" TabIndex="106" EmptyDataRowStyle-CssClass="emptytable">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:PaymentTerms %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTerm" runat="server" Text='<%#Eval("TCD_NAME") %>' ToolTip='<%# HttpUtility.HtmlDecode(Eval("TCD_NAME").ToString())%>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTCDPK" runat="server" Value='<%#Eval("TCD_PK") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="50%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:DueDate %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate" runat="server" Text='<%#  Eval("DUE_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("DUE_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                            ToolTip='<%# Eval("DUE_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:DueAmount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDueAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("DUE_AMOUNT")) %>'
                                                            ToolTip='<%#GetFormattedCurrency(Eval("DUE_AMOUNT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <%---------------------------------End Invoice Due Date details Popup-----------------------------------%>
                        </asp:TableCell></asp:TableRow><asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell></asp:TableRow></asp:Table><div id="divScriptButtons">
                    <asp:Button runat="server" ID="btnJournalize_Action" CommandName="JOURNALIZE" OnClick="ActionHandler"
                        EnableTheming="false" Style="display: none" />
                    <asp:Button ID="btnJournalizeUpdate" runat="server" OnClick="ActionHandler" CommandName="JOURNALIZEUPDATE"
                        EnableTheming="false" Style="display: none" />
                </div>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary
                        ID="vsPage" ValidationGroup="invoice" runat="server" />
                    <asp:ValidationSummary ID="vsTax" ValidationGroup="tax" runat="server" />
                    <asp:ValidationSummary ID="vsCustoms" ValidationGroup="Customs" runat="server" />
                    <asp:ValidationSummary ID="vsDeduction" ValidationGroup="deduction" runat="server" />
                    <asp:ValidationSummary ID="vsTaxDate" ValidationGroup="taxDate" runat="server" />
                    <asp:ValidationSummary ID="vsOthercharge" ValidationGroup="Valothercharge" runat="server" />
                    <asp:ValidationSummary ID="vsUpload" ValidationGroup="upload" runat="server" />
                    <asp:HiddenField ID="hdfAppType" runat="server" />
                    <asp:HiddenField ID="hdfAppSubType" runat="server" />
                    <asp:HiddenField ID="hdfSaveWithoutAllocation" runat="server" />
                    <asp:HiddenField ID="hdfSaveWithGreaterInvAmount" Value="0" runat="server" />
                    <asp:HiddenField ID="hdfSaleOrderType" Value="0" runat="server" />
                    <asp:HiddenField ID="hdfIsJournalize" runat="server" Value="False" />
                </div>
            </div>
            <div style="display: none;">
                <asp:Button ID="btnTermCheck" runat="server" CommandName="GETDUEDATE" OnClick="ActionHandler"
                    EnableTheming="false" Style="display: none;" />
                <asp:Button ID="btnConfirmOtherCharges" runat="server" OnClick="ActionHandler" CommandName="CONFIRMOTHERCHARGES" />
                <asp:Button ID="btnDateChange" runat="server" OnClick="ActionHandler" CommandName="CHANGEDATE" />
            </div>
            <%--User Control--%>
            <asp:HiddenField ID="hdfgroup" runat="server" Value="1" />
            <div id="divJournalize" style="display: none">
                <uc1:Journalize ID="ucrJournalize" runat="server" />
            </div>
            <div id="divAlert" style="display: none">
                <uc2:Alert ID="ucrAlert" runat="server" />
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="invoice">
                </uc1:WorkflowUserComments>
            </div>
            <asp:HiddenField ID="hdfJournalizeWorkFlow" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIscontYes" runat="server" />
            <asp:HiddenField runat="server" ID="hdfIsTaxPayable" Value="0" />
            <asp:HiddenField runat="server" ID="hdfIsTaxOCEditable" Value="0" />
            <asp:HiddenField runat="server" ID="hdfAllocNowAmount" Value="0" />
            <asp:HiddenField runat="server" ID="hdfIsDedApplyClick" Value="0" />
            <asp:HiddenField runat="server" ID="hdfIsCusAllAdv" Value="0" />
            <asp:HiddenField runat="server" ID="hdfSalOrderType" Value="0" />
            <asp:HiddenField runat="server" ID="hdfMode" />
            <asp:HiddenField runat="server" ID="hdfOtherCharge" Value="0" />
            <asp:HiddenField runat="server" ID="hdfIsAdvInvHasTax" Value="1" />
            <asp:HiddenField ID="hdfDecimalFormatWithSeperator" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormatWithSeperator" runat="server" />
            <asp:HiddenField runat="server" ID="hdfCurrencyGroup1" Value="3" />
            <asp:HiddenField runat="server" ID="hdfCurrencyGroup2" Value="2" />
            <asp:HiddenField runat="server" ID="hdfIsInvCancelled" Value="0" />
            <asp:HiddenField runat="server" ID="hdfSCDate" />
            <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsMultipleCategory" runat="server" Value="0" />
            <asp:HiddenField ID="hdfToPortPk" runat="server" />
            <asp:HiddenField runat="server" ID="hdfTaxDiscountApplied" Value="0" />
            <asp:HiddenField ID="hdfIsSBUCustomer" runat="server" Value="0" />
            <asp:HiddenField runat="server" ID="hdfInvTerm" />
            <asp:HiddenField ID="hdfEnableAddlOtherCharge" runat="server" Value="0" />
            <asp:HiddenField ID="hdfConfirmOtherCharges" Value="0" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
