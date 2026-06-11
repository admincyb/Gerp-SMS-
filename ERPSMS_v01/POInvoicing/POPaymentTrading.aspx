<%@ Page Title="<%$ Resources:Captions,Title_POPaymentTrading %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="POPaymentTrading.aspx.cs" Inherits="ERPSMS_v01.POInvoicing.POPaymentTrading"
    Theme="ClassicExt" ValidateRequest="false" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/Journalize/UserControls/JournalizeControlNew.ascx" TagName="Journalize"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        a.downloadClass
        {
            cursor: pointer;
            text-decoration: underline;
        }
        a.removedownloadClass
        {
            cursor: auto;
            text-decoration: none;
        }
    </style>
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function ModeAutoComplete() {
            if ($("[id$=ddlMode]").val() > 0) {
                if ($("[id$=ddlMode]").val() != "6")
                    GrandScriptUtils.MakeAutoCompleteDDL("txtPaymentBank", url + "?Type=" + $("[id$=ddlMode]").val(), "hdfPaymentBank", true, true, "BANK", false, false, true);
            }
        }
        function PrintCheque() {
            var printURL = $('[id$=hdfPrintCheque]').val().split(',');
            for (var i = 0; i < printURL.length; i++)
                OpenPDF(printURL[i]);
        }
        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtPaymentDate");
            GrandScriptUtils.DatePickerCommon("txtInstrumentDate");
            GrandScriptUtils.DatePickerCommon("txtVatTaxInvDate");
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendorHd", url, "hdfVendorHd", true, true, "VENDOR");
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendor", url, "hdfVendorID", true, true, "VENDOR");
            GrandScriptUtils.MakeAutoCompleteDDL("txtPaymentNumber", url, "hdfPaymentPK", true, true, "GETPAYMENTNOTRADINGAUTO");
            GrandScriptUtils.AddDateRangeCommon("txtSearchDateFrom", "hdfSearchDateFrom", "txtSearchDateTo", "hdfSearchDateTo", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtWHTAccountPopup", url, "hdfWHTAccountPopup", true, true, "WHTACCOUNTS");
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendorPopup", url, "hdfVendorPopup", true, false, "VENDOR");
            GrandScriptUtils.MakeAutoCompleteDDL("txtAddressType", url + "?VendorPk=" + $("[id$='hdfVendorPopup']").val(), "hdfAddressType", true, false, "VENDORCONTACTS");
            GrandScriptUtils.MakeAutoCompleteDDL("txtWthAddressType", url + "?VendorPk=" + $("[id$='hdfVendorHd']").val(), "hdfWthAddressType", true, true, "VENDORCONTACTS");
            GrandScriptUtils.DatePickerCommon("txtPVDate");
            GrandScriptUtils.AddDateRangeCommon("txtPendingFromDate", "hdfPendingFromDate", "txtPendingToDate", "hdfPendingToDate", false, false);
            $("[id*=txtPayNow]").ForceNumericOnly();
            $("[id*=txtPayNowSplit]").ForceNumericOnly();
            $("[id*=txtTaxSplit]").ForceNumericOnly();
            $("[id*=txtTotalAmountBC]").ForceNumericOnly();
            $("[id*=txtCrdrPayNow]").ForceNumericOnly();
            $("[id*=txtCrdrAdjAmount]").ForceNumericOnly();
            $("[id*=txtHdrExchangeRate]").ForceNumericOnly();
            $("[id*=txtPaymentAmount]").ForceNumericOnly();
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();
            if ($('[id$=btnJournalSaveSubmit]').is(":visible"))
                $('[id$=btnJournalSubmit]').hide();
            if ($('[id$=btnApply]').is(":visible"))
                $('[id$=btnWhtSave]').hide();
            if ($('[id$=btnVatTaxApply]').is(":visible"))
                $('[id$=btnVatTaxSave]').hide();
            $("[id$='chkPDC']").hide();
            $("[id$='lblPDC']").hide();
            $("[id$='ddlMode']").live("change", function () {
                if ($(this).val() == "2") {
                    $("[id$='chkPDC']").show();
                    $("[id$='lblPDC']").show();
                }
            });
            if ($("[id$='ddlMode']").val() == '2') {
                $("[id$='chkPDC']").show();
                $("[id$='lblPDC']").show();
            }
            else {
                $("[id$='chkPDC']").hide();
                $("[id$='lblPDC']").hide();
            }
            if ($('[id$=btnReverse]').is(":visible")) {
                if ($('[id$=hdfShowPDC]').val() == 0) {
                    $('[id$=btnReverse]').hide();
                }
            }
            if ($('[id$=btnReverseDetail]').is(":visible")) {
                if ($('[id$=hdfShowPDC]').val() == 0) {
                    $('[id$=btnReverseDetail]').hide();
                }
            }
            if ($('[id$=btnReturn]').is(":visible")) {
                if ($('[id$=hdfShowChequeReturn]').val() == 1) {
                    $('[id$=btnReturn]').hide();
                }
            }
            if ($('[id$=btnReturnDetail]').is(":visible")) {
                if ($('[id$=hdfShowChequeReturn]').val() == 1) {
                    $('[id$=btnReturnDetail]').hide();
                }
            }
            $("[id$='chkVendorforpayemnt']").next("label").css("text-align", "left");
            $("[id$='chkBankCharge']").next("label").css("text-align", "left", "display", "inline");
            $("[id$='chkVendorforpayemnt']").attr("disabled", "disabled");
            //Set a stamp for cancelled invoice
            if ($("[id$=hdfIsCancelled]").val() == "1")
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
            else
                $("[id$=tblDetailHdr]").addClass("table-devide");
            //End
            InitPInvNumberAuto();
            if ($("[id$=txtPurchaseInvNumber]").attr("disabled") == true) {
                DisableAuto($("[id$=txtPurchaseInvNumber]"), $("[id$=hdfPurchaseInvPK]"));
            }
            else {
                EnableAuto($("[id$=txtPurchaseInvNumber]"), $("[id$=hdfPurchaseInvPK]"));
            }
            ShowHidePendingInv($("[id$=hdfIsPendingInvVisible]").val());
            if ($("[id$=txtVendorHd]").attr("disabled") == true) {
                DisableAuto($("[id$=txtVendorHd]"), $("[id$=hdfVendorHd]"));
            }
        }
        //For   check  Already Paid while savesubmit
        function ShowAlreadyPaidWKF() {
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
                        $("[id$=btnSaveSubmit]").click();
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
        //For   check  Already Paid while save
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
                        $("[id$=btnSavePmnt]").click();
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
        function parseDateFormat(s) {
            return new Date(s.replace(/^(\d+)\W+(\w+)\W+/, '$2 $1 '));
        }
        function AfterDateSelect(controlID) {
            if (controlID == "txtInstrumentDate" || controlID == "txtPaymentDate") {
                if ($('[id$=txtInstrumentDate]').val() != "" && $('[id$=txtPaymentDate]').val() != "") {
                    var ChequeDate = parseDateFormat($('[id$=txtInstrumentDate]').val());
                    var PaymentDate = parseDateFormat($('[id$=txtPaymentDate]').val());
                    var ppcReconStatus = '<%= GetGlobalResourceObject("ConfigurationsRes","PPCReconciliation").ToString() %>';
                    if (ppcReconStatus == 1) {
                        $("[id$='chkPDC']").attr("checked", true);
                    }
                    else {
                        if (ChequeDate > PaymentDate) {
                            $("[id$='chkPDC']").attr("checked", true);
                        }
                        else {
                            $("[id$='chkPDC']").attr("checked", false);
                        }
                    }
                }
                else {
                    $("[id$='chkPDC']").attr("checked", false);
                }
            }
        }
        function AfterClose(containerID) {
            if (containerID == "[id$=divJournalize]") {
                $("[id$=btnJournalizeUpdate]").click();
            }
            else if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
                if ($("[id$=hdfJournalizeWorkFlow]").val() == "1") {                  
                    ShowCommonCotainerDiv('[id$=divJournalize]', $("[id$=hdfJournalName]").val(), "1%");
                    AfterCloseWkfInJournal();
                }
            }
            else if (containerID == "[id$=divTemplate]") {               
                ShowCommonCotainerDiv('[id$=divJournalize]', $("[id$=hdfJournalName]").val(), "1%");
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
        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                $("[id$=pnlSavePmnt]").hide();
                $("[id$=pnlDeletePmnt]").hide();
                //                $("[id$=pnlSubmit]").hide();
                $("[id$=btnSavePaymentSplit]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDeletePmnt]").hide();
                $("[id$=pnlPrint]").hide();
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
            if (targetControlID == "txtPaymentBank") {
                $("[id$=btnPaymentAccountNo]").click();
            }
            else if (targetControlID == "txtPaymentCurrency") {
                $("[id$=btnCurrency]").click();
            }
            else if (targetControlID == "txtWHTAccountPopup") {
                $("[id$=btnWHTAccountPopup]").click();
            }
            else if (targetControlID == "txtVATAccountPopup") {
                $("[id$=btnVatBuyAccountPopup]").click();
            }
            else if (targetControlID == "txtVendorPopup") {
                $("[id$=btnVendorPopup]").click();
            }
            else if (targetControlID == "txtAddressType") {
                $("[id$=btnVendorContDtl]").click();
            }
            else if (targetControlID == "txtWthAddressType") {
                $("[id$=btnWHTVendor]").click();
            }
            else if (targetControlID == "txtVendorHd") {
                $("[id$=btnVendor]").click();
            }
            else if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteSelect(targetControlID);
            }
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtPaymentBank") {
                $("[id$=btnPaymentAccountNo]").click();
            }
            else if (targetControlID == "txtPaymentCurrency") {
                $("[id$=btnCurrency]").click();
            }
            else if (targetControlID == "txtVendor") {
                $("[id$=hdfVendorID]").val("0");
            }
            else if (targetControlID == "txtWHTAccountPopup") {
                $("[id$=btnWHTAccountPopup]").click();
            }
            else if (targetControlID == "txtVendorHd") {
                $("[id$=btnVendor]").click();
            }
            else if (targetControlID == "txtVATAccountPopup") {
                $("[id$=btnVatBuyAccountPopup]").click();
            }
            else if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteSelect(targetControlID);
            }
        }
        function Bindtaxid() {
            var Vendorid = parseFloat($("[id$=hdfVendorPopup]").val());
            $("[id$=txtVatTaxId]").val("");
        }
        function toFixed(num, precision) {
            return (+(Math.round(+(num + 'e' + precision)) + 'e' + -precision)).toFixed(precision);
        }
        function round(value, decimals) {
            return Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
        }
        function CalculateWHTTotal(sender) {
            var whtAmt = 0;
            var DecimalDigits = 2;
            var whtTaxAmount = 0;
            if (!isNaN(parseFloat($("[id$=hdfWHTAccountPopup]").val()))) {
                var taxformula = $("#[id$=hdfTaxformula]").val();

                if (!isNaN(parseFloat($("[id$=hdfDecimalDigits]").val()))) {
                    DecimalDigits = parseFloat($("[id$=hdfDecimalDigits]").val());
                }
                if (!isNaN(parseFloat($("[id$=txtPopupWHTAmount]").val()))) {
                    whtAmt = parseFloat($("[id$=txtPopupWHTAmount]").val());
                }
                taxformula = taxformula.replace("#SUBTOTAL#", whtAmt);
                try {
                    whtTaxAmount = eval(taxformula);
                } catch (e) {
                    whtTaxAmount = 0;
                }
            }
            whtTaxAmount = round(whtTaxAmount, DecimalDigits);
            $("#[id*=txtWHTTaxAmountPopup]").val(toFixed(whtTaxAmount, DecimalDigits));
        }
        function CalculateVatBuyTotal(sender) {
            var vatAmt = 0;
            var DecimalDigits = 0;
            var vatTaxAmount = 0;
            if (!isNaN(parseFloat($("[id$=hdfVATAccountPopup]").val()))) {
                var taxformula = $("#[id$=hdfTaxformula]").val();
                if (!isNaN(parseFloat($("[id$=hdfDecimalDigits]").val()))) {
                    DecimalDigits = parseFloat($("[id$=hdfDecimalDigits]").val());
                }
                if (!isNaN(parseFloat($("[id$=txtBeforeTaxAmount]").val()))) {
                    vatAmt = parseFloat($("[id$=txtBeforeTaxAmount]").val());
                }
                taxformula = taxformula.replace("#SUBTOTAL#", vatAmt);
                try {
                    vatTaxAmount = eval(taxformula);
                } catch (e) {
                    vatTaxAmount = 0;
                }
            }
            vatTaxAmount = round(vatTaxAmount, DecimalDigits);
            $("#[id*=txtVATTaxAmountPopup]").val(toFixed(vatTaxAmount, DecimalDigits));
        }
        function WhtAmtMismatch() {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = $("[id$=hdfAmntMissmatch]").val();
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfIscontYesWht]").val(1);
                        $(this).dialog("close");
                        $("[id$=btnSavePmnt]").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfIscontYesWht]").val(0);
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }
        function WhtAmtMismatchSubmit() {

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = $("[id$=hdfAmntMissmatch]").val();
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfIscontYesWht]").val(1);
                        $(this).dialog("close");
                        $("[id$=btnSaveSubmit]").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfIscontYesWht]").val(0);
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }
        function VatAmtMismatch() {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = $("[id$=hdfAmntMissmatch]").val();
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfIscontYesVat]").val(1);
                        $(this).dialog("close");
                        $("[id$=btnVatTaxSave]").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfIscontYesVat]").val(0);
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }
        function VatAmtMismatchApply() {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = $("[id$=hdfAmntMissmatch]").val();
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfIscontYesVat]").val(1);
                        $(this).dialog("close");
                        $("[id$=btnVatTaxApply]").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfIscontYesVat]").val(0);
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }
        function HideOverlay() {
            $('#divmodel').hide();
        }
        function CalculateTotal(sender) {
            var val1 = parseFloat($(sender).val());
            var Amount = 0;
            var taxAmount = 0;
            var TotaltaxAmount = 0;
            var AdjAmount = 0;
            var BalancetoPay = 0;
            var DecimalDigits = 0;
            var taxpercentage = 0;
            var basevalue = 0;
            var ttaxamt = 0;
            var hdftax = 0;
            var hdftotalamt = 0;
            var whtAmount = 0;
            var otherCharges = 0;
            var TotalOtherCharges = 0;
            if (!isNaN(parseFloat($("[id$=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("[id$=hdfDecimalDigits]").val());
            }
            //Calc AdjAmount
            $("#[id*=grdInvoiceList] input[type=text][id*=txtAdjustments]").each(function (index) {
                //Check if number is not empty
                if ($.trim($(this).val()) != "") {
                    //Check if number is a valid integer
                    if (!isNaN(parseFloat($(this).val()))) {
                        AdjAmount = AdjAmount + parseFloat($(this).val());
                    }
                }
            });
            $("#[id*=grdInvoiceList] [id*=lblTotalAdjustmentsFooter]").html(addCommas(AdjAmount.toFixed(DecimalDigits)));
            $("#[id*=txtAdjAmount]").val(AdjAmount.toFixed(DecimalDigits));
            $("#[id*=grdInvoiceList] input[type=text][id*=txtPayNow]").each(function (index) {
                if (!isNaN(parseFloat($(this).closest('tr').find('.BalancetoPay').text()))) {
                    var number = Number($(this).closest('tr').find('.BalancetoPay').text().replace(/[^0-9\.]+/g, ""));
                    BalancetoPay = parseFloat(number);
                }
                //Other Charges
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=txtOtherCharges]").val()))) {
                    var number = Number($(this).closest('tr').find("#[id*=txtOtherCharges]").val().replace(/[^0-9\.]+/g, ""));
                    otherCharges = parseFloat(number);
                    TotalOtherCharges = TotalOtherCharges + otherCharges;
                }
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=hdfTaxHdrDtlAmt]").val()))) {
                    var number = Number($(this).closest('tr').find("#[id*=hdfTaxHdrDtlAmt]").val().replace(/[^0-9\.]+/g, ""));
                    hdftax = parseFloat(number);
                }
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=hdfTotalAmt]").val()))) {
                    var number = Number($(this).closest('tr').find("#[id*=hdfTotalAmt]").val().replace(/[^0-9\.]+/g, ""));
                    hdftotalamt = parseFloat(number);
                }
                var othrAmt = ($(this).closest('tr').find("#[id*=lblOtherCharges]").html().replace(new RegExp(',', 'g'), ''));
                othrAmt = othrAmt.replace(new RegExp(',', 'g'), '');
                OAmtSO = parseFloat(othrAmt);
                var adjAmt = 0;
                if ($(this).closest('tr').find("#[id*=lblAdjAmount]").html() != undefined) {
                    adjAmt = ($(this).closest('tr').find("#[id*=lblAdjAmount]").html().replace(new RegExp(',', 'g'), ''));
                    adjAmt = parseFloat(adjAmt.replace(new RegExp(',', 'g'), ''));
                }
                var Category = $(this).closest('tr').find("#[id*=hdfCategory]").val();
                var group = $(this).closest('tr').find("#[id*=hdfGroup]").val();
                var GrosAmount = $(this).closest('tr').find("#[id*=hdfPayable]").val();
                //Check if number is not empty
                if ($.trim($(this).val()) != "") {
                    //Check if number is a valid integer
                    if (!isNaN(parseFloat($(this).val()))) {
                        taxpercentage = parseFloat(parseFloat((GrosAmount - OAmtSO)) / parseFloat((((GrosAmount - OAmtSO) - hdftax) == 0.00) ? 1.00 : ((GrosAmount - OAmtSO) - hdftax))) - 1;
                        //Change based on configuration, Done By Riyas
                        if ($("[id$=hdfIsTaxForOtherCharge]").val() == "1") {
                            {
                                tamt = parseFloat($(this).val());
                                taxpercentage = parseFloat((((GrosAmount) / ((GrosAmount) - hdftax)) - 1));
                            }
                        }
                        else
                            tamt = (parseFloat($(this).val()) + parseFloat(adjAmt)) - otherCharges;
                        tamt = tamt <= 0 ? 0.00 : tamt;
                        basevalue = (tamt) / (((1 + taxpercentage) == 0) ? 1 : (1 + taxpercentage));
                        ttaxamt = (tamt - basevalue);
                        var ta = addCommas(ttaxamt.toFixed(DecimalDigits).toString());
                        $(this).closest('tr').find("#[id*=lblTotalTax]").text(ta);
                        $(this).closest('tr').find("#[id*=lblTotalTax]").attr("title", ta);
                        $(this).closest('tr').find("#[id*=hdfTotalTax]").val(ta);
                        $("[id$=divTax]").show();
                        $(this).closest("table").find("tr:first th:eq(12)").show();
                        $(this).closest("tr").find("td:eq(12)").show();
                        $(this).closest("table").find("tr:last td:eq(12)").show();
                        taxAmount = taxAmount + parseFloat(ttaxamt.toFixed(DecimalDigits).toString());
                        TotaltaxAmount = TotaltaxAmount + parseFloat(ttaxamt.toFixed(DecimalDigits).toString());
                        Amount = Amount + parseFloat($(this).val());
                        if (Category == "2" || group == "3" || group == "2") {
                            $("[id$=hdfSaveTax]").val(1);
                        }
                        else {
                            $("[id$=hdfSaveTax]").val(0);

                        }
                    }
                }
            });
            $("#[id*=grdInvoiceList] [id*=lblTotalPayNowFooter]").html(addCommas(Amount.toFixed(DecimalDigits)));
            $("#[id*=grdInvoiceList] [id*=lblTotalOtherCharges]").html(addCommas(TotalOtherCharges.toFixed(DecimalDigits)));
            $("[id$=hdfTotalOtherCharges]").val(TotalOtherCharges.toFixed(DecimalDigits));
            var exchangeRate = $("[id$=hdfExchangeCurr]").val() == "" ? 1 : $("[id$=hdfExchangeCurr]").val();
            var paidAmt = (Amount * exchangeRate);
            var whtAmt = (paidAmt - TotaltaxAmount) - AdjAmount;
            paidAmt = paidAmt - AdjAmount;
            if (!isNaN(parseFloat($("[id$=txtWHTAmount]").val()))) {
                whtAmount = parseFloat($("[id$=txtWHTAmount]").val());
            }
            if (whtAmount > 0 && $("[id$=hdfEdit]").val() == "1") {
                $("[id$=imgbtnPrint]").show();
            }
            else {
                $("[id$=imgbtnPrint]").hide();
            }
            var WHTAmountRound = (Math.round(whtAmount * 100) / 100).toFixed(DecimalDigits);
            $("#[id*=txtWHTAmount]").val(WHTAmountRound);
            if ($("[id$='chkVendorforpayemnt']").is(":checked") == true && whtAmount > 0)// && whtAccount != -1)
            {
                if (!isNaN(parseFloat($("[id$=txtHdrExchangeRate]").val())))
                    exchangeRate = parseFloat($("[id$=txtHdrExchangeRate]").val());
                paidAmt = paidAmt - (whtAmount / exchangeRate);
            }
            var paidAmountRound = (Math.round(paidAmt * 100) / 100).toFixed(DecimalDigits);
            $("#[id*=txtPaidAmount]").val(paidAmountRound);
            var BalanceAmnt = 0;
            var TotPaidAmnt = 0;
            var PaymentModeRowIndex = -1;
            PaymentModeRowIndex = parseFloat($("[id$=hdfPaymentModeRowIndex]").val());
            $("#[id*=grdPaymentModes] input[type=hidden][id*=hdfPymntMode]").each(function (index) {
                var Amount = 0;
                var ExchngRate = 1;
                if (index != PaymentModeRowIndex) {
                    if (!isNaN(parseFloat($(this).closest('tr').find("[id$=lblPymntTotalAmount]").text().replace(/[^0-9\.]+/g, "")))) {
                        Amount = parseFloat($(this).closest('tr').find("[id$=lblPymntTotalAmount]").text().replace(/[^0-9\.]+/g, ""));
                        TotPaidAmnt += Amount;
                    }
                }
            });
            BalanceAmnt = (paidAmountRound - TotPaidAmnt) < 0 ? 0 : (paidAmountRound - TotPaidAmnt.toFixed(DecimalDigits));
            var paymentModeCount = $("#<%=grdPaymentModes.ClientID %> tr").length;
            if ((parseInt($("[id$=hdfPaymentModeRowIndex]").val()) < 0 || paymentModeCount <= 3) && parseInt($("[id$=hdfIsEdited]").val()) == 0) {
                $("#[id*=txtPaymentAmount]").val(BalanceAmnt.toFixed(DecimalDigits));
            }
            if (!isNaN(parseFloat($("[id$=txtHdrExchangeRate]").val())) && !isNaN(parseFloat($("[id$=txtPaidAmount]").val()))) {
                var exchangeRate = parseFloat($("[id$=txtHdrExchangeRate]").val());
                var paidAmount = parseFloat($("[id$=txtPaymentAmount]").val());
                $("#[id*=txtTotalAmountBC]").val(toFixed((exchangeRate * paidAmount), DecimalDigits));
            }
            $("#[id*=txtTaxAmount]").val(taxAmount.toFixed(DecimalDigits));
            $("#[id*=grdInvoiceList] [id*=lblTotalTaxFooter]").html(addCommas(taxAmount.toFixed(DecimalDigits)));
        }
        function CalculateTotalBC() {
            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }
            if (!isNaN(parseFloat($("[id$=txtHdrExchangeRate]").val())) && !isNaN(parseFloat($("[id$=txtPaymentAmount]").val()))) {
                var exchangeRate = parseFloat($("[id$=txtHdrExchangeRate]").val());
                var receivedAmount = parseFloat($("[id$=txtPaymentAmount]").val());
                $("#[id*=txtTotalAmountBC]").val(toFixed((exchangeRate * receivedAmount), DecimalDigits));
            }
        }
        function CalculateTotalSplit(sender) {
            var val1 = parseFloat($(sender).val());
            var Amount = 0;
            var BalancetoPay = 0;
            var DecimalDigits = 0;
            var otherCharges = 0.0;
            var TotalOtherCharges = 0.0;
            var Discount = 0;
            var tempTax = 0;
            var tempTotal = 0;
            var tempDisc = 0;
            var tempInvAmt = 0;
            var Tax = 0;
            var taxPer = 1;
            var otherChargePer = 1;
            var poTax = 0;
            var poOthercharge = 0;
            var PoCharge = 0;
            var InvTotalOtherCharge = 0;
            var InvOtherAmount = 0;
            var TotalOtherAmnt = 0;
            if (!isNaN(parseFloat($("[id$=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("[id$=hdfDecimalDigits]").val());
            }
            if (!isNaN(parseFloat($("[id$=hdfTaxPer]").val()))) {
                taxPer = parseFloat($("[id$=hdfTaxPer]").val());
            }
            if (!isNaN(parseFloat($("[id$=hdfOtherPer]").val()))) {
                otherChargePer = parseFloat($("[id$=hdfOtherPer]").val());
            }
            if (!isNaN(parseFloat($("[id$=hdfInvTotalOtherCharge]").val()))) {
                InvTotalOtherCharge = parseFloat($("[id$=hdfInvTotalOtherCharge]").val());
            }
            if (!isNaN(parseFloat($("[id$=hdfInvOtherCharge]").val()))) {
                TotalOtherAmnt = parseFloat($("[id$=hdfInvOtherCharge]").val());
            }
            $("#[id*=grdPaymentSplit] input[type=text][id*=txtPayNowSplit]").each(function (index) {
                if (!isNaN(parseFloat($(this).closest('tr').find('.BalancetoPay').text()))) {
                    var number = Number($(this).closest('tr').find('.BalancetoPay').text().replace(/[^0-9\.]+/g, ""));
                    BalancetoPay = parseFloat(number);
                }
                //Other Charges                                
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=lblOtherChargesSplit]").text()))) {
                    var number = Number($(this).closest('tr').find("#[id*=lblOtherChargesSplit]").text().replace(/[^0-9\.]+/g, ""));
                    otherCharges = parseFloat(number);
                    TotalOtherCharges = TotalOtherCharges + otherCharges;
                }
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=hdfInvPoOtherAmnt]").val()))) {
                    var number = Number($(this).closest('tr').find("#[id*=hdfInvPoOtherAmnt]").val().replace(/[^0-9\.]+/g, ""));
                    InvOtherAmount = parseFloat(number);
                }
                //Total Amount
                tempTotal = parseFloat($(this).closest('tr').find('[id*=lblAmountSplit]').html().replace(/[^0-9\.]+/g, ""));
                //Total Discount
                tempDisc = parseFloat($(this).closest('tr').find('[id*=lblDiscountSplit]').html().replace(/[^0-9\.]+/g, ""));
                tempTax = parseFloat($(this).closest('tr').find('[id*=lblTaxSplit]').html().replace(/[^0-9\.]+/g, ""));
                tempInvAmt = parseFloat($(this).val()) - otherCharges;
                //Check if number is not empty
                if ($.trim($(this).val()) != "") {
                    //Check if number is a valid integer
                    if (!isNaN(parseFloat($(this).val()))) {
                        $(this).parent("td").find('input[type=hidden][id$=hdfPayNowSplit]').val($(this).val());
                        Amount = Amount + parseFloat($(this).val());
                        var tempResult = 0;
                        poTax = parseFloat($(this).val()) * taxPer;
                        poOthercharge = parseFloat($(this).val()) * otherChargePer;
                        $(this).closest('tr').find("#[id*=lblTotalTaxSplit]").text(poTax.toFixed(CurrencyDigits));
                        $(this).closest('tr').find("#[id*=txtTaxSplit]").val(poTax.toFixed(CurrencyDigits));
                        $(this).closest('tr').find("#[id*=hdfTaxSplit]").val(poTax.toFixed(CurrencyDigits));
                        var otheramnt = 0;
                        if (InvTotalOtherCharge > 0) {
                            otheramnt = (InvOtherAmount / InvTotalOtherCharge) * TotalOtherAmnt;
                        }
                        $(this).closest('tr').find("#[id*=lblOtherChargesSplit]").text(addCommas(otheramnt.toFixed(DecimalDigits)));
                        Tax = Tax + parseFloat($(this).closest('tr').find("#[id*=txtTaxSplit]").val());
                        PoCharge = PoCharge + otheramnt;
                    }
                }
            });
            Tax = Tax < 0 ? 0 : Tax;
            $("#[id*=grdPaymentSplit] [id*=lblTotalPayNowFooterSplit]").html(Amount.toFixed(DecimalDigits));
            $("#[id*=grdPaymentSplit] [id*=hdfTotalPayNowFooterSplit]").val(Amount);
            $("#[id*=grdPaymentSplit] [id*=hdfTotalOtherChargesFooterSplit]").val(PoCharge);
            $("#[id*=grdPaymentSplit] [id*=lblTotalOtherChargesFooterSplit]").html(addCommas(PoCharge.toFixed(DecimalDigits)));
            //----------------------------------------
            var invtax = $("[id$=hdfTax]").val();
            var invtaxamnt = parseFloat(invtax);
            if (Tax != invtaxamnt && Tax > 0 && invtaxamnt > 0) {
                var totalRows = $("#<%=grdPaymentSplit.ClientID %> tr").length;
                rowcount = parseInt(totalRows);
                $("#[id*=grdPaymentSplit] input[type=text][id*=txtPayNowSplit]").each(function (index) {
                    var lastsplittax = 0;
                    if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=txtTaxSplit]").val()))) {
                        lastsplittax = $(this).closest('tr').find("#[id*=txtTaxSplit]").val().replace(/[^0-9\.]+/g, "");
                        lastsplittax = parseFloat(lastsplittax);
                    }
                    if (index == (rowcount - 3)) {
                        if ((invtaxamnt - Tax) < 1 && (invtaxamnt - Tax) > -1) {
                            var lastsplittaxamnt = lastsplittax + (invtaxamnt - Tax);
                            $(this).closest('tr').find('[id*=txtTaxSplit]').val(lastsplittaxamnt.toFixed(DecimalDigits));
                            Tax = Tax + (invtaxamnt - Tax);
                        }
                    }
                });
            }
            //----------------------------------------
            $("#[id*=grdPaymentSplit] [id*=lblTotalTaxFooterSplit]").html(addCommas(Tax.toFixed(DecimalDigits)));
            $("#[id*=grdPaymentSplit] [id*=hdfTotalTaxFooterSplit]").val(Tax.toFixed(DecimalDigits));
            var exchangeRate = $("[id$=hdfExchangeCurr]").val() == "" ? 1 : $("[id$=hdfExchangeCurr]").val();
        }
        function CalculateTotalFooterTaxSplit(sender) {
            var DecimalDigits = 0;
            var TotalTax = 0;
            var TotalOtherCharge = 0;
            if (!isNaN(parseFloat($("[id$=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("[id$=hdfDecimalDigits]").val());
            }
            $("#[id*=grdPaymentSplit] input[type=text][id*=txtTaxSplit]").each(function (index) {
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=txtTaxSplit]").val()))) {
                    var tax = $(this).closest('tr').find("#[id*=txtTaxSplit]").val().replace(/[^0-9\.]+/g, "");
                    TotalTax = TotalTax + parseFloat(tax);
                }
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=lblOtherChargesSplit]").text()))) {
                    var number = Number($(this).closest('tr').find("#[id*=lblOtherChargesSplit]").text().replace(/[^0-9\.]+/g, ""));
                    var otherCharges = parseFloat(number);
                    TotalOtherCharge = TotalOtherCharge + otherCharges;
                }
            });
            //----------------------------------------
            var invtax = $("[id$=hdfTax]").val();
            var invtaxamnt = parseFloat(invtax);
            if (TotalTax != invtaxamnt && TotalTax > 0 && invtaxamnt > 0) {
                var totalRows = $("#<%=grdPaymentSplit.ClientID %> tr").length;
                rowcount = parseInt(totalRows);
                $("#[id*=grdPaymentSplit] input[type=text][id*=txtPayNowSplit]").each(function (index) {
                    var lastsplittax = 0;
                    if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=txtTaxSplit]").val()))) {
                        lastsplittax = $(this).closest('tr').find("#[id*=txtTaxSplit]").val().replace(/[^0-9\.]+/g, "");
                        lastsplittax = parseFloat(lastsplittax);
                    }
                    if (index == (rowcount - 3)) {
                        if ((invtaxamnt - TotalTax) < 1 && (invtaxamnt - TotalTax) > -1) {
                            var lastsplittaxamnt = lastsplittax + (invtaxamnt - TotalTax);
                            $(this).closest('tr').find('[id*=txtTaxSplit]').val(lastsplittaxamnt.toFixed(DecimalDigits));
                            TotalTax = TotalTax + (invtaxamnt - TotalTax);
                        }
                    }
                });
            }
            //----------------------------------------
            $("#[id*=grdPaymentSplit] [id*=lblTotalTaxFooterSplit]").html(addCommas(TotalTax.toFixed(DecimalDigits)));
            $("#[id*=grdPaymentSplit] [id*=hdfTotalTaxFooterSplit]").val(TotalTax.toFixed(DecimalDigits));
            $("#[id*=grdPaymentSplit] [id*=lblTotalOtherChargesFooterSplit]").html(addCommas(TotalOtherCharge.toFixed(DecimalDigits)));
            $("#[id*=grdPaymentSplit] [id*=hdfTotalOtherChargesFooterSplit]").val(TotalOtherCharge.toFixed(DecimalDigits));
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
                        //Page_Validators.splice(i, 1);
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
                CheckValidationDuplicate(valGroup);
                Page_ClientValidate(valGroup);
                //For Avoiding Wht textbox value
                $("[id$=hdfIsBtnUpload]").val("1");
                if ($("[id$='chkVendorforpayemnt']").is(":checked") == true) {
                    $("[id$=hdftest]").val("1");
                }
            }
            if (!Page_IsValid) {
                if (valGroup == 'vatbuy') {
                    $("[id$=litErrorMsg]").hide();
                    ShowErrorMessage($("#diverrorvatbuy").html(), '<%= Resources.Messages.Information %>');
                    return false;
                }
                else {
                    $("[id$=litErrorMsg]").hide();
                    ShowErrorMessage($("#diverror").html());
                    return false;  //Page is invalid -- stop right here
                }
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
        function ValidatePayNow(sender, args) {
            var payNow = $(sender).closest('tr').find('[id*=txtPayNow]').val();
            var pattern = new RegExp($(sender).closest('tr').find('[id*=vamPayNow]')[0].validationexpression);
            var payNowAmt = parseFloat(payNow);
            if (!pattern.test(payNow) || isNaN(payNowAmt) || payNowAmt == 0) {
                args.IsValid = false;
            }
            else {
                args.IsValid = true;
            }
        }
        function ValidateOtherCharges(sender, args) {
            var payNow = $(sender).closest('tr').find('[id*=txtOtherCharges]').val();
            var pattern = new RegExp($(sender).closest('tr').find('[id*=vamOtherChatges]')[0].validationexpression);
            var payNowAmt = parseFloat(payNow);
            if (!pattern.test(payNow) || isNaN(payNowAmt) || payNowAmt == 0) {
                args.IsValid = false;
            }
            else {
                args.IsValid = true;
            }
        }
        function ValidateOtherCharge(sender, args) {
            var payNow = $(sender).closest('tr').find('[id*=txtPayNow]').val();
            var otherCharge = $(sender).closest('tr').find('[id*=txtOtherCharges]').val();
            var invOtherCharge = $(sender).closest('tr').find('[id*=lblOtherCharges]').html();
            var payNowAmt = parseFloat(payNow.replace(new RegExp(',', 'g'), ''));
            var otherChargeAmt = parseFloat(otherCharge.replace(new RegExp(',', 'g'), ''));
            var invOtherChargeAmt = parseFloat(invOtherCharge.replace(new RegExp(',', 'g'), ''));
            if (payNowAmt == 0 && otherChargeAmt > 0) {
                args.IsValid = false;
            }
            else if ((otherChargeAmt > invOtherChargeAmt) || (otherChargeAmt > payNowAmt)) {
                args.IsValid = false;
            }
            else {
                args.IsValid = true;
            }
        }
        function ValidateCreditSplit(sender, args) {
            var CrpayNowAmt = 0;
            var CrAdjAmt = 0;
            var CrBalanceAmt = 0;
            var CrpayNow = $(sender).closest('tr').find('[id*=txtCrdrPayNow]').val();
            var CrAdj = $(sender).closest('tr').find('[id*=txtCrdrAdjAmount]').val();
            var CrBalance = $(sender).closest('tr').find('[id*=lblCrdrBalanceAmount]').html();
            if (!isNaN(parseFloat(CrpayNow.replace(new RegExp(',', 'g'), '')))) {
                CrpayNowAmt = parseFloat(CrpayNow.replace(new RegExp(',', 'g'), ''));
            }
            if (!isNaN(parseFloat(CrAdj.replace(new RegExp(',', 'g'), '')))) {
                CrAdjAmt = parseFloat(CrAdj.replace(new RegExp(',', 'g'), ''));
            }
            if (!isNaN(parseFloat(CrBalance.replace(new RegExp(',', 'g'), '')))) {
                CrBalanceAmt = parseFloat(CrBalance.replace(new RegExp(',', 'g'), ''));
            }
            if (CrBalanceAmt < (CrpayNowAmt + CrAdjAmt)) {
                args.IsValid = false;
            }
            else {
                args.IsValid = true;
            }
        }
        function MessageRemoveSplit(sender) {
            var invGroup = parseInt($(sender).closest('tr').find('[id*=hdfGroup]').val());
            if (invGroup != 3) { // 3:Expense Invoice
                ShowDeleteConfirm($(sender).closest('tr').find('[id*=btnPayNow]')[0], '<%=GetLocalResourceObject("Msg_Split_Delete").ToString() %>');
            }
        }
        function AfterDeleteConfirmationCancel(controlID) {
            var isPayNow = false;
            var Amount = parseFloat("0");
            var DecimalDigits = 0;
            if (!isNaN(parseFloat($("[id$=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("[id$=hdfDecimalDigits]").val());
            }
            $("#[id*=grdInvoiceList] [id*=btnPayNow]").each(function () {
                if ($(this).attr('id') == controlID) {
                    isPayNow = true;
                    $(this).closest('tr').find('[id*=txtPayNow]').val($(this).closest('tr').find('[id*=hdfPayNowPrev]').val());
                }
                Amount = Amount + parseFloat($(this).closest('tr').find('[id*=txtPayNow]').val());
            });
            if (isPayNow) {
                $("[id$=grdInvoiceList] [id*=lblTotalPayNowFooter]").html(addCommas(Amount.toFixed(DecimalDigits)));
                CalculateTotal();
                CloseMsgPopup();
            }
            else {
                if (controlID == "") {
                }
            }
        }
        function SetPayNowPrev(sender) {
            $(sender).closest('tr').find('[id*=hdfPayNowPrev]').val($(sender).val());
        }
        function CloseMsgPopup() {
            $(".ui-widget-overlay:visible").each(function () {
                if ($(this).parent().attr("id") != "updateProgress") {
                    $(this).hide();
                }
            });
        }
        function CheckBankCharge(oSrc, args) {
            var value1 = $('input:text[id$=txtPaidAmount]').val();
            var value2 = $('input:text[id$=txtBankCharge]').val();
            var exchangeRate = $("[id$=hdfExchRate]").val() == "" ? 1 : $("[id$=hdfExchRate]").val();
            var BC = $("[id$=hdfPaymentCurrency]").val();
            if (value1 != "" && value2 != "") {
                var EBankCharge = 0;
                if ($("[id$='ddlBankChargeCurrency']").val() != BC) {
                    EBankCharge = parseFloat(value1) * parseFloat(exchangeRate);
                }
                else {
                    EBankCharge = parseFloat(value1);
                }
                if ((EBankCharge <= parseFloat(value2)) && parseFloat(value2) > 0) {
                    args.IsValid = false;
                }
                else {
                    args.IsValid = true;
                }
            }
            else {
                args.IsValid = true;
            }
        }
        function validateFloatKeyPress(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            var number = el.value.split('.');
            if (charCode == 8) {
                return true;
            }
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            //get the carat position
            var currencyDecimal = 2;
            if (!isNaN(parseInt($("[id$=hdfDecimalDigits]").val()))) {
                currencyDecimal = parseInt($("[id$=hdfDecimalDigits]").val());
            }
            var caratPos = getSelectionStart(el);
            var dotPos = el.value.indexOf(".");
            if (caratPos > dotPos && dotPos > -1 && (number[1].length > currencyDecimal - 1)) {
                return false;
            }
            return true;
        }
        function getSelectionStart(o) {
            if (o.createTextRange) {
                var r = document.selection.createRange().duplicate()
                r.moveEnd('character', o.value.length)
                if (r.text == '') return o.value.length
                return o.value.lastIndexOf(r.text)
            } else return o.selectionStart
        }
        //For Setting/Resetting Colour of a selected InvoiceNo
        function SetVatbuyNotYetDueRowColor() {
            $("#<%= grdVATTaxDetails.ClientID %> input[type=hidden][id*=hdfIsVatbuyNotDue]").each(function (index) {
                if ($.trim($(this).val()) == "1") {
                    var selectedRowColor;
                    selectedRowColor = '<%= Resources.ErpRes.selectedRowColor %>';
                    $(this).closest('tr').css('background-color', selectedRowColor);
                }

            });
        }
        //End
        function ClearVendorContacts() {
            $("[id$=hdfVendorPopup]").val("");
            $("[id$=txtAddressType]").val("");
            $("[id$=hdfAddressType]").val("");
            $("[id$=txtVatTaxId]").val("");
            $("[id$=txtBranchCode]").val("");
            $("[id$=chkHeadOffice]").attr('checked', false);
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
        function CalculateTotalAdjn(sender) {
            var DecimalDigits = 0;
            var TotalPayNowFooterSplit = 0;
            var amtAdjnSplit = 0;
            if (!isNaN(parseFloat($("[id$=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("[id$=hdfDecimalDigits]").val());
            }
            $("#[id*=grdPaymentSplitAdjn] input[type=text][id*=txtAllocateAdjn]").each(function (index) {
                if (!isNaN(parseFloat($(this).val()))) {
                    amtAdjnSplit = parseFloat($(this).val().replace(new RegExp(',', 'g'), ''));
                    TotalPayNowFooterSplit = TotalPayNowFooterSplit + amtAdjnSplit;
                }
            });
            $("#[id*=grdPaymentSplitAdjn] [id*=lblTotalAllocateAdjn]").html(addCommas(TotalPayNowFooterSplit.toFixed(DecimalDigits)));
            $("#[id*=grdPaymentSplitAdjn] [id*=hdfTotalAllocateAdjn]").val(TotalPayNowFooterSplit.toFixed(DecimalDigits));
        }
        function ClearWhtVendorContacts() {
            $("[id$=hdfWhtAddressType]").val("");
            $("[id$=txtWthBranchCode]").val("");
            $("[id$=chkWthHeadOffice]").attr('checked', false);
        }
        function CalculateAdjnFooter(sender) {
            var DecimalDigits = 0;
            var TotaladjnFooter = 0;
            var amtAdjnSplit = 0;
            if (!isNaN(parseFloat($("[id$=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("[id$=hdfDecimalDigits]").val());
            }
            $("#[id*=grdInvoiceList] input[type=text][id*=txtAdjustments]").each(function (index) {
                if (!isNaN(parseFloat($(this).closest('tr').find('[id*=lblAdjAmount]').html())))
                    amtAdjnSplit = parseFloat($(this).closest('tr').find('[id*=lblAdjAmount]').html().replace(new RegExp(',', 'g'), ''));
                TotaladjnFooter = TotaladjnFooter + amtAdjnSplit;
            });
            if (TotaladjnFooter > 0)
                $("#[id*=grdInvoiceList] [id*=lblAdjAmountFooter]").html(TotaladjnFooter.toFixed(DecimalDigits));
            else
                $("#[id*=grdInvoiceList] [id*=lblAdjAmountFooter]").html("0.00");
        }
        function CalculatePymntSplitBalFooter() {
            var DecimalDigits = 0;
            var TotalBalFooter = 0;
            var amtBalSplit = 0;
            if (!isNaN(parseFloat($("[id$=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("[id$=hdfDecimalDigits]").val());
            }
            $("#[id*=grdPaymentSplit] input[type=text][id*=txtPayNowSplit]").each(function (index) {

                amtBalSplit = parseFloat($(this).closest('tr').find('[id*=lblBalanceSplit]').html().replace(/[^0-9\.]+/g, ""));
                TotalBalFooter = TotalBalFooter + amtBalSplit;
            });
            $("#[id*=grdPaymentSplit] [id*=lblTotalBalFooterSplit]").html(TotalBalFooter.toFixed(DecimalDigits));
            $("#[id*=grdPaymentSplit] [id*=hdfTotalBalFooterSplit]").val(TotalBalFooter);
        }
        function CalculateTotalCreditSplit() {
            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }
            var TotalCrdrPaynow = 0;
            var TotCrdrAdj = 0;
            $("#[id*=grdCrdrAllocation] input[type=text][id*=txtCrdrPayNow]").each(function (index) {
                var CrdrPaynow = 0;
                var CrdrAdj = 0;
                if (!isNaN(parseFloat($(this).val()))) {
                    CrdrPaynow = parseFloat($(this).val());
                    TotalCrdrPaynow += CrdrPaynow;
                }
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=txtCrdrAdjAmount]").val().replace(/[^0-9\.]+/g, "")))) {
                    CrdrAdj = parseFloat($(this).closest('tr').find("#[id*=txtCrdrAdjAmount]").val().replace(/[^0-9\.]+/g, ""));
                    TotCrdrAdj += CrdrAdj;
                }
            });
            $("#[id*=grdCrdrAllocation] [id*=lblCrdrPayNowFooter]").html(addCommas(TotalCrdrPaynow.toFixed(DecimalDigits)));
            $("#[id*=grdCrdrAllocation] [id*=hdfCrdrPayNowFooter]").val(TotalCrdrPaynow.toFixed(DecimalDigits));
            $("#[id*=grdCrdrAllocation] [id*=lblCrdrAdjAmountFooter]").html(addCommas(TotCrdrAdj.toFixed(DecimalDigits)));
            $("#[id*=grdCrdrAllocation] [id*=hdfCrdrAdjAmountFooter]").val(TotCrdrAdj.toFixed(DecimalDigits));
        }
        function CalculatePymntModeTotalFooter() {
            var PaymentAmnt = 0;
            var PaymentAmntTotalFooter = 0;
            var PaymentAmntBC = 0;
            var PaymentAmntBCTotalFooter = 0;
            if (!isNaN(parseFloat($("[id$=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("[id$=hdfDecimalDigits]").val());
            }
            $("#[id*=grdPaymentModes] input[type=hidden][id*=hdfPymntMode]").each(function (index) {
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=lblPymntTotalAmount]").html().replace(/[^0-9\.]+/g, "")))) {
                    PaymentAmnt = parseFloat($(this).closest('tr').find('[id*=lblPymntTotalAmount]').html().replace(/[^0-9\.]+/g, ""));
                    PaymentAmntTotalFooter += PaymentAmnt;
                }
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=lblPymntTotalAmountBC]").html().replace(/[^0-9\.]+/g, "")))) {
                    PaymentAmntBC = parseFloat($(this).closest('tr').find('[id*=lblPymntTotalAmountBC]').html().replace(/[^0-9\.]+/g, ""));
                    PaymentAmntBCTotalFooter += PaymentAmntBC;
                }
            });
            $("#[id*=grdPaymentModes] [id*=lblPymntTotalAmountFooter]").html(addCommas(PaymentAmntTotalFooter.toFixed(DecimalDigits)));
            $("#[id*=grdPaymentModes] [id*=lblPymntTotalAmountBCFooter]").html(addCommas(PaymentAmntBCTotalFooter.toFixed(DecimalDigits)));
        }

        //For Setting/Resetting Colour of a not tallied InvoiceNo
        function SetNotTalliedRowColor() {
            var selectedIds;
            var selectedIdsArray = new Array();
            selectedIds = $("[id$=hdfNotTalliedInvoicePk]").val();
            selectedIdsArray = selectedIds.split(',');
            for (i = 0; i < selectedIdsArray.length; ++i) {
                if (selectedIdsArray[i] != 0) {
                    $("#<%= grdInvoiceList.ClientID %> input[type=hidden][id*=hdfInvoicePK]").each(function (index) {
                        if ($.trim($(this).val()) == selectedIdsArray[i]) {
                            var selectedRowColor;
                            selectedRowColor = '<%= Resources.ErpRes.RowColourPink %>';
                            $(this).closest('tr').css('background-color', selectedRowColor);
                        }
                    });
                }
            }
        }
        //End

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

        //set pending po checkbox as checked,when select all check box is checked.Similarly unchecked
        $("[id*=chkSelectAllInv]").live("click", function () {
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

        $("[id*=chkInvPendSelect]").live("click", function () {
            var grid = $(this).closest("table");
            var chkHeader = $("[id*=chkSelectAllInv]", grid);
            if (!$(this).is(":checked")) {
                $("td", $(this).closest("tr")).removeClass("selected");
                chkHeader.removeAttr("checked");
            } else {
                $("td", $(this).closest("tr")).addClass("selected");
                if ($("[id*=chkInvPendSelect]", grid).length == $("[id*=chkInvPendSelect]:checked", grid).length) {
                    chkHeader.attr("checked", "checked");
                }
            }
        });
    </script>
    <script type="text/javascript">
        $(window).load(function EndRequest() {
            FormatCalendar('4');
        });
        var ControlID = 'calendar1|calendar2';
        function EndRequest() { FormatCalendar('4'); }
        function FormatCalendar(type) {
            var ctrlBehaviourarray = ControlID.split('|');
            for (var i = 0; i < ctrlBehaviourarray.length; i++) {
                var calenderCtrl = $find(ctrlBehaviourarray[i]);
                if (calenderCtrl) {
                    switch (type) {
                        case "6":
                            return;
                            break;
                        case "4":
                            $(calenderCtrl).attr('CalenderType', '2');
                            modifyMontDelegates(calenderCtrl);
                            break;
                        case "1":
                            $(calenderCtrl).attr('CalenderType', '3');
                            modifyYearDelegates(calenderCtrl);
                            break;
                    }
                }
            }
        }
        function modifyMontDelegates(cal) {
            //we need to modify the original delegate of the month cell.
            cal._cell$delegates = {
                mouseover: Function.createDelegate(cal, cal._cell_onmouseover),
                mouseout: Function.createDelegate(cal, cal._cell_onmouseout),
                click: Function.createDelegate(cal, function (e) {
                    /// <summary>
                    /// Handles the click event of a cell
                    /// </summary>
                    /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param>
                    e.stopPropagation();
                    e.preventDefault();
                    if (!cal._enabled) return;
                    var target = e.target;
                    var visibleDate = cal._getEffectiveVisibleDate();
                    Sys.UI.DomElement.removeCssClass(target.parentNode, "ajax__calendar_hover");
                    switch (target.mode) {
                        case "prev":
                        case "next":
                            cal._switchMonth(target.date);
                            break;
                        case "title":
                            switch (cal._mode) {
                                case "days": cal._switchMode("months"); break;
                                case "months": cal._switchMode("years"); break;
                            }
                            break;
                        case "month":
                            //if the mode is month, then stop switching to day mode.
                            if (target.month == visibleDate.getMonth()) {
                                //this._switchMode("days");
                            } else {
                                cal._visibleDate = target.date;
                                //this._switchMode("days");
                            }
                            cal.set_selectedDate(target.date);
                            cal._switchMonth(target.date);
                            cal._blur.post(true);
                            cal.raiseDateSelectionChanged();
                            break;
                        case "year":
                            if (target.date.getFullYear() == visibleDate.getFullYear()) {
                                cal._switchMode("months");
                            } else {
                                cal._visibleDate = target.date;
                                cal._switchMode("months");
                            }
                            break;
                        // case "day":                                                                                                                                             
                        // this.set_selectedDate(target.date);                                                                                                                                             
                        // this._switchMonth(target.date);                                                                                                                                             
                        // this._blur.post(true);                                                                                                                                             
                        // this.raiseDateSelectionChanged();                                                                                                                                             
                        // break;                                                                                                                                             
                        case "today":
                            cal.set_selectedDate(target.date);
                            cal._switchMonth(target.date);
                            cal._blur.post(true);
                            cal.raiseDateSelectionChanged();
                            break;
                    }
                })
            }
        }
        function modifyYearDelegates(cal) {
            //we need to modify the original delegate of the month cell.
            cal._cell$delegates = {
                mouseover: Function.createDelegate(cal, cal._cell_onmouseover),
                mouseout: Function.createDelegate(cal, cal._cell_onmouseout),
                click: Function.createDelegate(cal, function (e) {
                    /// <summary>
                    /// Handles the click event of a cell
                    /// </summary>
                    /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param>
                    e.stopPropagation();
                    e.preventDefault();
                    if (!cal._enabled) return;
                    var target = e.target;
                    var visibleDate = cal._getEffectiveVisibleDate();
                    Sys.UI.DomElement.removeCssClass(target.parentNode, "ajax__calendar_hover");
                    switch (target.mode) {
                        case "prev":
                        case "next":
                            cal._switchMonth(target.date);
                            break;
                        case "title":
                            switch (cal._mode) {
                                case "days": cal._switchMode("months"); break;
                                case "months": cal._switchMode("years"); break;
                            }
                            break;
                        case "year":
                            if (target.date.getFullYear() == visibleDate.getFullYear()) {
                                // cal._switchMode("months");
                            } else {
                                cal._visibleDate = target.date;
                                //cal._switchMode("months");
                            }
                            cal.set_selectedDate(target.date);
                            //cal._switchYear(target.date);
                            cal._blur.post(true);
                            cal.raiseDateSelectionChanged();
                            break;
                        case "today":
                            cal.set_selectedDate(target.date);
                            //cal._switchYear(target.date);
                            cal._blur.post(true);
                            cal.raiseDateSelectionChanged();
                            break;
                    }
                })
            }
        }

        function changeMonthCellHandlers(cal) {
            if (cal._monthsBody) {
                //remove the old handler of each month body.
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $common.removeHandlers(row.cells[j].firstChild, cal._cell$delegates);
                    }
                }
                //add the new handler of each month body.
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $addHandlers(row.cells[j].firstChild, cal._cell$delegates);
                    }
                }
            }
        }
        function changeYearCellHandlers(cal) {
            if (cal._monthsBody) {
                //remove the old handler of each month body.
                for (var i = 0; i < cal._yearsBody.rows.length; i++) {
                    var row = cal._yearsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $common.removeHandlers(row.cells[j].firstChild, cal._cell$delegates);
                    }
                }
                //add the new handler of each month body.
                for (var i = 0; i < cal._yearsBody.rows.length; i++) {
                    var row = cal._yearsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $addHandlers(row.cells[j].firstChild, cal._cell$delegates);
                    }
                }
            }
        }
        function onCalendarShown(cal, args) {
            cal._switchMode("months", true);
            cal._popupBehavior._element.style.zIndex = 10005;
        }
        function onCalendarHidden(sender, args) {

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
        function CheckAllPI(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById("<%=grdNewInvList.ClientID %>");
            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }

        function ShowHidePendingInv(flag) {
            ///<summary>
            /// Used to Show/Hide Pending Invoice List Div
            ///</summary>
            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divPendingInvDetails]").show();
                $("[id$=imbShowPendingInv]").hide();
                $("[id$=imbHidePendingInv]").show();
            }
            else {
                $("[id$=divPendingInvDetails]").hide();
                $("[id$=imbShowPendingInv]").show();
                $("[id$=imbHidePendingInv]").hide();
            }
            $("[id$=hdfIsPendingInvVisible]").val(flag);
            return false;
        }
        function ScrollDown() {
            window.scroll(400, 800);
            return false;
        }
        function InitPInvNumberAuto() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtPurchaseInvNumber", url + "?customerid=" + $("[id$=hdfVendorHd]").val() + "&InvCategory=" + $("[id$=ddlPendingInvCategory]").val() + "&InvType=" + $("[id$=ddlPendingInvType]").val(), "hdfPurchaseInvPK", true, true, "PENDINGPURCHASEINVNOTRADING");
            ResetPInvNumberAuto();
        }
        function ResetPInvNumberAuto() {
            var defText = '<%= Resources.ErpRes.AutoDefaultValue %>';
            $("[id$=txtPurchaseInvNumber]").val(defText);
            $("[id$=hdfPurchaseInvPK]").val('-1');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlPOPayment">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <div class="buttoncontainer-fields floatLeft" id="divSBUCompany">
                                    <asp:DropDownList ID="ddlCompany" class="select-full-a margnbotm0" runat="server"
                                        onmouseover="javascript:ShowTooltip('ddlCompany');">
                                    </asp:DropDownList>
                                </div>
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="17"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="20" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('payment')"
                                            ValidationGroup="payment" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="13"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('payment')"
                                            ValidationGroup="payment" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSavePmnt">
                                        <asp:Button runat="server" ID="btnSavePmnt" CommandName="SAVE" TabIndex="21" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('payment')"
                                            ValidationGroup="payment" ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDeletePmnt">
                                        <asp:Button runat="server" ID="btnDeletePmnt" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="22" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" />
                                    </li>
                                    <li runat="server" id="pnlPrint">
                                        <asp:Button runat="server" ID="btnPrint" Text="<%$resources:Controls,ChqPrint %>"
                                            OnClick="ActionHandler" CommandName="PRINT" TabIndex="24" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,ChqPrint %>" />
                                    </li>
                                    <li id="pnlPrintDT">
                                        <asp:Button runat="server" TabIndex="56" ID="btnPrintdt" Visible="false" CommandName="PRINTDT"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="23" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnJournalize" CommandName="JOURNALIZE" TabIndex="28"
                                            Text="<%$resources:Journalize %>" OnClick="ActionHandler" ToolTip="<%$resources:Journalize %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-journalize" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="29" ID="btnReturnDetail" CommandName="CHEQUERETURN"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Return %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-journalize" ToolTip="<%$resources:Controls,Return %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="29" ID="btnReverseDetail" CommandName="REVERSE"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Reverse %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-journalize" ToolTip="<%$resources:Controls,Reverse %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="25" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="28" ID="btnReturn" CommandName="CHEQUERETURN"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Return %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-journalize" ToolTip="<%$resources:Controls,Return %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="26" ID="btnReverse" CommandName="REVERSE" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Reverse %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-journalize"
                                            ToolTip="<%$resources:Controls,Reverse %>" />
                                    </li>
                                    <li id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="8" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelPP %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelPP %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="26" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="27" Text="<%$resources:Controls,View %>"
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
                                TabIndex="6" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="PAYMENTLIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                TabIndex="7" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="PAYMENTDETAIL"
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
                                                <%= GetGlobalResourceObject("Captions", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="65" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="66" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn----------%>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblSearchDateFrom" Text="<%$ resources:FromDate %>"
                                                AssociatedControlID="txtSearchDateFrom"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSearchDateFrom" CssClass="input-small margnrgt1-5per"
                                                TabIndex="1" onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfSearchDateFrom" runat="server" />
                                            <asp:Label runat="server" ID="lblSearchDateTo" Text="<%$ resources:ToDate %>" AssociatedControlID="txtSearchDateTo"
                                                CssClass="middle-lbl-a"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSearchDateTo" CssClass="input-small" TabIndex="2"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfSearchDateTo" runat="server" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" AssociatedControlID="ddlStatus"
                                                CssClass="middle-lbl-small margnrgt3"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-a margnrgt3"
                                                TabIndex="3">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,NotPosted %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Posted %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label runat="server" ID="lblPDCstatus" Text="<%$ resources:PDCStatus%>" AssociatedControlID="ddlPDCStatus"
                                                CssClass="middle-lbl-small"></asp:Label>
                                            <asp:DropDownList ID="ddlPDCStatus" runat="server" CssClass="select-small-a" TabIndex="4">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Reversed %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,NotReversed %>" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblVendor" runat="server" Text="<%$resources:Vendor %>" AssociatedControlID="txtVendor"
                                                CssClass="margnbotm0"></asp:Label>
                                            <asp:TextBox ID="txtVendor" runat="server" CssClass="select-half margnbotm0" MaxLength="100"
                                                TabIndex="5"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfVendorID" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblPaymentNumber" runat="server" Text="<%$resources:TransactionId %>"
                                                AssociatedControlID="txtPaymentNumber" CssClass="middle-lbl-small margnbotm0"></asp:Label>
                                            <asp:TextBox ID="txtPaymentNumber" runat="server" CssClass="input-small margnbotm0 "
                                                MaxLength="100" TabIndex="6"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfPaymentPK" runat="server" Value="" />
                                            <asp:Label ID="lblSIno" runat="server" Text="<%$resources:SINo %>" AssociatedControlID="txtSINo"
                                                CssClass="middle-lbl-small margnbotm0 margn-rgt0"></asp:Label>
                                            <asp:TextBox ID="txtSINo" runat="server" CssClass="input-small margnbotm0" MaxLength="100"
                                                TabIndex="7"> </asp:TextBox>
                                            <asp:Label ID="lblSearchHdr" runat="server" AssociatedControlID="btnSearchHdr" CssClass="middle-lbl-xsmall-d margnbotm0"></asp:Label>
                                            <asp:ImageButton ID="btnSearchHdr" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                OnClick="ActionHandler" CommandName="SEARCH" SkinID="search-ext" Style="margin-bottom: 0px!important;
                                                margin-top: 2px;" />
                                            <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="9" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext" Style="margin-bottom: 0px!important;
                                                margin-top: 2px;" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdPOPaymentHdr" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" AutoGenerateColumns="false" OnRowDataBound="ActionHandler"
                                    EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" TabIndex="7" runat="server" GroupName="SelectOne"
                                                    ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" AutoPostBack="true"
                                                    OnCheckedChanged="ActionHandler" />
                                                <asp:HiddenField runat="server" ID="hdfPaymentID" Value='<%# Eval("PVH_PK") %>' />
                                                <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval("PVH_DEPT") %>' />
                                                <asp:HiddenField ID="hdfDelStatus" runat="server" Value='<%# Eval("PVH_DEL_STATUS") %>' />
                                                <asp:HiddenField ID="hdfInvGroup" runat="server" Value='<%# Eval("PVH_GROUP") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PaymentDate %>" SortExpression="<%$ resources:DataFieldRes,POPaymentDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaymentDate" runat="server" Text='<%# Eval("PVH_DATE", Resources.Constants.DateFormatGrid).ToString()  %>'
                                                    ToolTip='<%# Eval("PVH_DATE", Resources.Constants.DateFormatGrid).ToString() %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TransactionId %>" SortExpression="<%$ resources:DataFieldRes,POPaymentNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTransactionId" runat="server" Text='<%# Eval("PVH_NO")==""?"[NEW]":Eval("PVH_NO") %>'
                                                    ToolTip='<%# Eval("PVH_NO")==""?"[NEW]":Eval("PVH_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Vendor %>" SortExpression="<%$ Resources:DataFieldRes,PaymentVendorPK%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendor" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("PVH_VENDOR_TEXT"),30) %>'
                                                    ToolTip='<%# Eval("PVH_VENDOR_TEXT") %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfVendorPK" Value='<%# Eval("PVH_VENDOR") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="22%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceNo %>" SortExpression="<%$ resources:DataFieldRes,INVNo %>">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkInvnos" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("PVH_INVOICE_NO"),15) %>'
                                                    ToolTip='<%# Eval("PVH_INVOICE_NO")%>' CssClass="text-underline" OnClick="ActionHandler"
                                                    Style="text-align: left!important;" CommandName="PRINTINVOICE"></asp:LinkButton>
                                                <asp:HiddenField runat="server" ID="hdfInvType" Value='<%# Eval("IVH_GROUP")%>' />
                                                <asp:HiddenField runat="server" ID="hdfinvPK" Value='<%# Eval("IVH_PK")%>' />
                                                <asp:HiddenField runat="server" ID="hdfinvCategory" Value='<%# Eval("IVH_CATEGORY")%>' />
                                                <asp:HiddenField runat="server" ID="hdfinvCategoryType" Value='<%# Eval("IVH_TYPE")%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="btnPDCFlag" runat="server" OnClientClick="javascript:return false;" />
                                                <asp:Button ID="btnPDCReturn" runat="server" OnClientClick="javascript:return false;" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ModeofPayment %>" SortExpression="<%$ resources:DataFieldRes,POPaymentMode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblModeofPayment" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("PDM_MODE_TEXT"),7) %>'
                                                    ToolTip='<%# Eval("PDM_MODE_TEXT")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BankName %>" SortExpression="<%$ resources:DataFieldRes,PaymentBank %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBankName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("PDM_BANK_TEXT"),27) %>'
                                                    ToolTip='<%# Eval("PDM_BANK_TEXT")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="18%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CurrencyH %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='<%#Eval("PVH_BASE_CURR_TEXT")  %>'
                                                    ToolTip='<%#Eval("PVH_BASE_CURR_TEXT")  %>'></asp:Label>
                                                <asp:HiddenField ID="hdfJCurrency" runat="server" Value='<%# Eval("PVH_BASE_CURR") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %>" SortExpression="<%$ resources:DataFieldRes,POPaidAmount %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%#  Eval("PVH_PAID_AMOUNT", "{0:c}") %>'
                                                    ToolTip='<%#  Eval("PVH_PAID_AMOUNT", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblHdrAmountBaseCur" runat="server" Text='' ToolTip=''></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmountTHB" runat="server" Text='<%#  Eval("PVH_PAID_AMOUNT_BC", "{0:c}") %>'
                                                    ToolTip='<%#  Eval("PVH_PAID_AMOUNT_BC", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("ASC_CSS_CLASS") %>' ToolTip='<%# Eval("PVH_STATUS_TEXT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval("PVH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgPosted" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? GetLocalResourceObject("unposted").ToString() : Eval("FTH_CSS_CLASS")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? Resources.Captions.NotPosted : Eval("FTH_STATUS_TEXT")%>'
                                                    Visible='<%# (Convert.ToDecimal(Eval("PVH_PAID_AMOUNT")) > 0 ? true : false) %>' />
                                                <asp:HiddenField runat="server" ID="hdfPosted" Value='<%# Eval("PVH_HAS_JRNL_ENTRY") %>' />
                                                <asp:HiddenField runat="server" ID="hdfPDC" Value='<%#Eval("PDM_PDC_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfMode" Value='<%#Eval("PDM_MODE_IS_CHEQUE") %>' />
                                                <asp:HiddenField runat="server" ID="hdfBouncedStatus" Value='<%#Eval("PDM_BOUNCED_STATUS") %>' />
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
                                            <asp:Label ID="Label1" runat="server" Text="<%$ resources:PaymentNo%>" AssociatedControlID="lblPaymentNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblPaymentNo" CssClass="input-small"></asp:Label>
                                            <asp:HiddenField ID="hdfPaymentNo" runat="server" />
                                            <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                                            <asp:HiddenField ID="AST_CODE" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblVendorHd" runat="server" Text="<%$resources:Vendor %>" AssociatedControlID="txtVendorHd"></asp:Label>
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
                                    <%= GetLocalResourceObject("PendingInvHdr").ToString()%></h1>
                                <div class="button-wrap-right ">
                                    <asp:ImageButton runat="server" ID="imbShowPendingInv" OnClientClick="javascript:return ShowHidePendingInv(1);"
                                        SkinID="imbArrowInactive" ToolTip="<%$ resources:Controls,Show %>" />
                                    <asp:ImageButton runat="server" ID="imbHidePendingInv" OnClientClick="javascript:return ShowHidePendingInv();"
                                        Style="display: none" SkinID="imbArrowActive" ToolTip="<%$ resources:Controls,Hide %>" />
                                    <asp:HiddenField ID="hdfIsPendingInvVisible" runat="server" Value="0" />
                                </div>
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <div id="divPendingInvDetails" style="display: none">
                                        <div class="gridwrap">
                                            <div id="searchwrap" class="search-wrap-custom1">
                                                <asp:Label ID="lblPndngFrmDate" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtPendingFromDate"></asp:Label>
                                                <asp:TextBox ID="txtPendingFromDate" runat="server" TabIndex="1" Width="100px" MaxLength="13"
                                                    onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                                <asp:HiddenField ID="hdfPendingFromDate" runat="server" Value="" />
                                                <asp:Label ID="lblPndngToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtPendingToDate"
                                                    CssClass="margn-lft5"></asp:Label>
                                                <asp:TextBox ID="txtPendingToDate" runat="server" TabIndex="2" Width="100px" MaxLength="14"
                                                    onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                                <asp:HiddenField ID="hdfPendingToDate" runat="server" Value="" />
                                                <asp:Label ID="lblPendingInvCategory" runat="server" Text="<%$ resources:InvCategory %>"
                                                    CssClass="margn-lft5" AssociatedControlID="ddlPendingInvCategory"></asp:Label>
                                                <asp:DropDownList ID="ddlPendingInvCategory" runat="server" TabIndex="2" Width="150px" onchange="InitPInvNumberAuto();">                                                    
                                                </asp:DropDownList>
                                                <asp:Label ID="lblPndngInvType" runat="server" Text="<%$resources:InvoiceType %>"
                                                    AssociatedControlID="ddlPendingInvType" CssClass="margn-lft5"></asp:Label>
                                                <asp:DropDownList ID="ddlPendingInvType" runat="server" TabIndex="2" Width="100px" onchange="InitPInvNumberAuto();">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblPoNoSr" runat="server" Text="<%$ resources:InvoiceNoHdr %>" CssClass="lbl-6perc margn-lft5"
                                                    AssociatedControlID="txtPurchaseInvNumber"></asp:Label>
                                                <div id="divSearchDtls">
                                                    <asp:TextBox ID="txtPurchaseInvNumber" runat="server" TabIndex="2"> </asp:TextBox>
                                                    <asp:HiddenField ID="hdfPurchaseInvPK" runat="server" Value="" />
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
                                            <asp:GridView runat="server" ID="grdPendingInvList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                                AllowPaging="false" OnRowDataBound="ActionHandler"  AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkSelectAllInv" runat="server" ToolTip="Select All" TabIndex="5" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" ID="chkInvPendSelect" TabIndex="6" />
                                                            <asp:HiddenField runat="server" ID="hdfInvoiceID" Value='<%# Eval("IVH_PK") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfPOVendorPK" Value='<%# Eval("IVH_VND_PK") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfInvCurrency" Value='<%# Eval("IVH_CURRENCY") %>' />
                                                            <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval("IVH_DEPT") %>' />
                                                            <asp:HiddenField ID="hdfPOType" runat="server" Value='<%# Eval("POH_ITEM_TYPE") %>' />
                                                            <asp:HiddenField ID="hdfTaxAmount" runat="server" Value='<%# Eval("IVH_TAX_TC") %>' />
                                                            <asp:HiddenField ID="hdfDelStatus" runat="server" Value='<%# Eval("IVH_DEL_STATUS") %>' />
                                                            <asp:HiddenField ID="hdfInOpeningInv" runat="server" Value='<%# Eval("IVH_IS_OPENING") %>' />
                                                            <asp:HiddenField ID="hdfInvStatus" runat="server" Value='<%# Eval("IVH_STATUS") %>' />
                                                            <asp:HiddenField ID="hdfPndInvCategory" runat="server" Value='<%# Eval("IVH_CATEGORY") %>' />
                                                            <asp:HiddenField ID="hdfPndInvGroup" runat="server" Value='<%# Eval("IVH_GROUP") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="2%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:Invdate %>" SortExpression="IVH_DATE">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInvoiceDateGd" runat="server" Text='<%#  Eval("IVH_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("IVH_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                                ToolTip='<%# Eval("IVH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="7%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:InvoiceNoHdr %>" SortExpression="IVH_NO">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("IVH_NO") ==""?"[NEW]":Eval("IVH_NO")%>'
                                                                ToolTip='<%# Eval("IVH_NO")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="7%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCmpName" Font-Bold="true" runat="server" Text='<%# Eval("IVH_COMPANY_TEXT")%>'
                                                                ToolTip='<%# Eval("IVH_COMPANY_TEXT")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:InvoiceType %>" SortExpression="IVH_TYPE_TEXT">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInvoiceType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_TYPE_TEXT"),3,"")%>'
                                                                ToolTip='<%# Eval("IVH_TYPE_TEXT")%>'></asp:Label>
                                                            <asp:HiddenField ID="hdfInvType" runat="server" Value='<%# Eval("IVH_TYPE") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:SupplierShortInvNO %>" SortExpression="IVH_VENDOR_INV_NO"
                                                        Visible="true">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSupplierInvNO" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_VENDOR_INV_NO"),10) %>'
                                                                ToolTip='<%# Eval("IVH_VENDOR_INV_NO") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:PONo %>" SortExpression="IVH_POH_NO">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSoNo" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_POH_NO"),14)%>'
                                                                ToolTip='<%# Eval("IVH_POH_NO")%>'></asp:Label>
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
                                                    <asp:TemplateField HeaderText="<%$ resources:PODate %>" SortExpression="IVH_POH_DT">
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
                                                            <asp:Label ID="lblInvoiceValue" runat="server" Text='<%# Eval("IVH_AMOUNT", "{0:c}") %>'
                                                                ToolTip='<%# Eval("IVH_AMOUNT", "{0:c}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                        <HeaderStyle CssClass="amount-numeric" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:BalAmt %>" SortExpression="IVH_BAL_AMNT_TC">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBalAmt" runat="server" Text='<%# Eval("IVH_BAL_AMNT_TC", "{0:c}") %>'
                                                                ToolTip='<%# Eval("IVH_BAL_AMNT_TC", "{0:c}") %>' Visible='<%# GetBalanceLableVisibility(Eval("IVH_AMOUNT").ToString(),Eval("IVH_BAL_AMNT_TC").ToString())%>'></asp:Label>
                                                            <asp:LinkButton ID="lbnBalAmt" runat="server" Text='<%# Eval("IVH_BAL_AMNT_TC", "{0:c}") %>'
                                                                CssClass="text-underline" ToolTip='<%# Eval("IVH_BAL_AMNT_TC", "{0:c}") %>' OnClick="ActionHandler"
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
                                                </Columns>
                                            </asp:GridView>
                                            <uc1:PagerControl ID="uclPendingInvPaging" runat="server" />
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
                            <table class="table-devide" id="tblDetailHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblPaymentDate1" Text="<%$ resources:PaymentDate%>"
                                                AssociatedControlID="txtPaymentDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPaymentDate" CssClass="input-small Uidate-picker"
                                                TabIndex="9" onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfPaymentDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="payment" EnableClientScript="true" runat="server" ControlToValidate="txtPaymentDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Payment_Date %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label ID="lblPaymentCurrency" runat="server" Text="<%$resources:Currency %>"
                                                AssociatedControlID="txtPaymentCurrency"></asp:Label>
                                            <asp:TextBox ID="txtPaymentCurrency" Enabled="false" runat="server" MaxLength="3"
                                                TabIndex="16" CssClass="Uidate-picker input-disabled"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfPaymentCurrency" runat="server" Value="" />
                                            <asp:RequiredFieldValidator ID="vrfPaymentCurrency" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="payment" EnableClientScript="true" runat="server" ControlToValidate="txtPaymentCurrency"
                                                Display="Dynamic" Text="*" InitialValue="<%$ resources:Messages, AutoDefaultValue %>"
                                                ErrorMessage="<%$ resources:Err_Currency %>">
                                            </asp:RequiredFieldValidator>
                                            <div style="width: 18px; display: inline-block;">
                                                <asp:Button ID="btnCurrency" runat="server" Text="<%$ resources:Controls,Search %>"
                                                    OnClick="ActionHandler" TabIndex="3" CommandName="CALCURRENCY" SkinID="btnInner-search"
                                                    EnableTheming="false" Style="display: none" />
                                            </div>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblHdrExchngRate" Text="<%$ resources:ExchangeRate%>"
                                                AssociatedControlID="txtHdrExchangeRate"></asp:Label>
                                            <asp:TextBox ID="txtHdrExchangeRate" runat="server" MaxLength="12" CssClass="numeric input-small"
                                                onkeyup="CalculateTotal();"></asp:TextBox>
                                            <asp:Label ID="lblVendorbank" runat="server" Text="<%$ resources:VendorBank%>" class="middle-lbl-xsmall-c1"
                                                AssociatedControlID="ddlvendorBank"></asp:Label>
                                            <asp:DropDownList ID="ddlvendorBank" runat="server" CssClass="select-w33per">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <asp:HiddenField ID="hdfSaveTax" runat="server" Value="0" />
                                <asp:HiddenField ID="hdfPrintCheque" runat="server" Value="" />
                                <asp:HiddenField ID="hdfEdit" runat="server" Value="0" />
                                <asp:HiddenField ID="hdfExchRate" runat="server" />
                                <asp:HiddenField ID="hdfTaxformula" runat="server" Value="0" />
                                <asp:HiddenField ID="hdfVendorPK" runat="server" />
                                <asp:HiddenField ID="hdfVendorAccountNo" runat="server" />
                                <asp:HiddenField ID="hdfInvoiceCurr" runat="server" />
                                <asp:HiddenField ID="hdfExchangeCurr" runat="server" />
                                <asp:HiddenField ID="hdfExchangeCurrBC" runat="server" />
                                <asp:HiddenField ID="hdfShowPDC" runat="server" Value="0" />
                                <asp:HiddenField ID="hdfShowChequeReturn" runat="server" Value="1" />
                                <asp:HiddenField ID="hdfDelStatus" runat="server" Value="0" />
                                <asp:HiddenField ID="hdfJournalName" runat="server" />
                                <asp:GridView ID="grdInvoiceList" runat="server" AutoGenerateColumns="False" PageSize="<%$ resources:PageSize %>"
                                    AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                    Width="100%" ShowFooter="true" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceNo %>" SortExpression="<%$ resources:DataFieldRes,InvoiceNo %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfCategory" runat="server" Value='<%#Eval("IVH_CATEGORY")%>' />
                                                <asp:HiddenField ID="hdfGroup" runat="server" Value='<%#Eval("IVH_GROUP")%>' />
                                                <asp:HiddenField ID="hdfTotalAmt" runat="server" Value='<%#Eval("IVH_TOTAL_AMT")%>' />
                                                <asp:HiddenField ID="hdfTaxAmt" runat="server" Value='<%#Eval("IVH_TAX_AMT")%>' />
                                                <asp:HiddenField ID="hdfGrossAmt" runat="server" Value='<%#Eval("IVH_GROSS_AMT")%>' />
                                                <asp:HiddenField ID="hdfTaxHdrDtlAmt" runat="server" Value='<%#Eval("VTL_TAX_AMOUNT")%>' />
                                                <asp:HiddenField ID="hdfIsApply" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdfInvoicePK" runat="server" Value='<%#Eval("IVH_PK")%>' />
                                                <asp:HiddenField ID="hdfPaymentMpgPK" runat="server" Value='<%#Eval("PVM_PK")%>' />
                                                <asp:HiddenField ID="hdfInvCategory" runat="server" Value='<%#Eval("IVH_CATEGORY")%>' />
                                                <asp:HiddenField ID="hdfInvGroup" runat="server" Value='<%#Eval("IVH_GROUP")%>' />
                                                <asp:HiddenField ID="hdfInvoiceType" runat="server" Value='<%#Eval("IVH_TYPE")%>' />
                                                <asp:Label ID="lblInvoiceNo" runat="server" Visible="false"></asp:Label>
                                                <asp:LinkButton ID="lnkInvoiceNo" CssClass="text-underline" runat="server" OnClick="ActionHandler"
                                                    CommandName="SHOWPOPUP"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblfooter" Text="<%$ resources:Total %>"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <%--  CMP_DISPLAY_CODE--%>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCmpDisplayCode" runat="server" Text='<%# Eval("CMP_DISPLAY_CODE") %>'
                                                    ToolTip='<%# Eval("CMP_DISPLAY_CODE") %>' CssClass='<%# Eval("CMP_LINE_COLOUR") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false" HeaderText="<%$ resources:Vendor %>" SortExpression="<%$ Resources:DataFieldRes,POInvoiceVendorText%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendorInv" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_VENDOR_TEXT"),80) %>'
                                                    ToolTip='<%# Eval("IVH_VENDOR_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:VendInvNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendInvNo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_VENDOR_INV_NO"),80) %>'
                                                    ToolTip='<%# Eval("IVH_VENDOR_INV_NO") %>'>></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false" HeaderText="<%$ resources:CurrencyH %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvCurrency" runat="server" Text='<%# Eval("IVH_CURRENCY_TEXT") %>'
                                                    ToolTip='<%# Eval("IVH_CURRENCY_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GrossAmount %>" SortExpression="<%$ resources:DataFieldRes,POInvoiceGrossAmountBC %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrossAmount" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("IVH_GROSS_AMT")))%>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("IVH_GROSS_AMT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Discount %>" SortExpression="<%$ resources:DataFieldRes,POInvoiceDiscountBC %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDiscount" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("VTL_DISC_AMOUNT")))%>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("VTL_DISC_AMOUNT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:OtherCharges %>" SortExpression="<%$ resources:DataFieldRes,POInvoiceTaxtBC %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOtherCharges" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("IVH_SHIP_CHARGE")))%>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("IVH_SHIP_CHARGE")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Tax %>" SortExpression="<%$ resources:DataFieldRes,POInvoiceTaxtBC %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTax" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("IVH_TAX_AMT")))%>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("IVH_TAX_AMT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Payable %>" SortExpression="<%$ resources:DataFieldRes,POInvoiceNetAmount %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalAmount" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("IVH_AMOUNT_NET_TC")))%>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("IVH_AMOUNT_NET_TC")))%>'></asp:Label>
                                                <asp:HiddenField ID="hdfPayable" runat="server" Value='<%# GetFormattedCurrency(Eval("IVH_AMOUNT_NET_TC")) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CNAmount %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCnAmount" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("PVM_CN_AMOUNT")))%>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("PVM_CN_AMOUNT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Paid %>" SortExpression="<%$ resources:DataFieldRes,POInvoicePaidAmount %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaid" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("IVH_PAID_AMT")))%>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("IVH_PAID_AMT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AdjAmount %>" SortExpression="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAdjAmount" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("PVM_ADJUST_AMOUNT")))%>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("PVM_ADJUST_AMOUNT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkAllocationAdjn" runat="server" OnClick="ActionHandler" CommandName="ADJNINVOICEDETAIL"
                                                    SkinID="adjustallocation-icon" ToolTip="<%$ resources:AdjAllocation %>" CommandArgument="PageAction_Entry"
                                                    TabIndex="4" ValidationGroup="split" OnClientClick="javascript:ValidatePageNow('split')" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Baltopay %>" SortExpression="<%$ resources:DataFieldRes,POInvoiceBalAmount %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBaltopay" CssClass="BalancetoPay" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("IVH_BAL_TO_PAY")))%>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("IVH_BAL_TO_PAY")))%>'></asp:Label>
                                                <asp:HiddenField ID="hdfBaltopay" runat="server" Value='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("IVH_BAL_TO_PAY")))%>' />
                                                <asp:HiddenField ID="hdfInitialBaltoPay" runat="server" Value='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("IVH_BAL_TO_PAY")))%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <%--Pay Now--%>
                                        <asp:TemplateField HeaderText="<%$ resources:PayNow %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPayNow" runat="server" CssClass="input-w70 numeric" MaxLength="15"
                                                    Text='<%# GetFormattedCurrency(Convert.ToDouble(Eval("PVM_PAID_AMOUNT")))%>'
                                                    ToolTip='<%# GetFormattedCurrency(Convert.ToDouble(Eval("PVM_PAID_AMOUNT")))%>'
                                                    onfocus="SetPayNowPrev(this);" onkeyup="CalculateTotal(this);" onchange="MessageRemoveSplit(this);"
                                                    TabIndex="8"></asp:TextBox>
                                                <asp:Button runat="server" ID="btnPayNow" OnClick="ActionHandler" CommandName="CHANGEPAYNOW"
                                                    EnableTheming="false" Style="display: none;" />
                                                <asp:HiddenField ID="hdfPayNow" runat="server" Value='<%# GetFormattedCurrency(Convert.ToDouble(Eval("PVM_PAID_AMOUNT")))%>' />
                                                <asp:HiddenField ID="hdfOtherChargesPrev" runat="server" Value="0.0" />
                                                <asp:HiddenField ID="hdfPayNowPrev" runat="server" />
                                                <asp:HiddenField ID="hdfHasSplit" runat="server" Value="0" />
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="vrfPayNow" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="payment" EnableClientScript="true" runat="server" ControlToValidate="txtPayNow"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PaymentAmount1 %>">
                                                    </asp:RequiredFieldValidator>
                                                    <cc1:AmountValidation ID="vamPayNow" runat="server" ControlToValidate="txtPayNow"
                                                        ErrorMessage="<%$ resources:Err_PaymentAmount %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="payment"></cc1:AmountValidation>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalPayNowFooter"></asp:Label></FooterTemplate>
                                        </asp:TemplateField>
                                        <%--Remove--%>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkAllocation" runat="server" OnClick="ActionHandler" CommandName="INVOICEDETAIL"
                                                    SkinID="allocation-icon" ToolTip="Allocation" CommandArgument="PageAction_Entry"
                                                    ValidationGroup="split" OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load"
                                                    OnClientClick="javascript:ValidatePageNow('split')" />
                                            </ItemTemplate>
                                            <ItemStyle Width="10px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CrdrAlcnAmt %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCrdrAlcnAmount" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" Width="8px" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="btnCrdrAllocation" runat="server" OnClick="ActionHandler" CommandName="CRDRALLOCATION"
                                                    SkinID="creditnote-icon" ToolTip="<%$ resources:CreditAllocation %>" CommandArgument="PageAction_Entry" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:OtherCharges %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtOtherCharges" runat="server" CssClass="input-w70 numeric" MaxLength="15"
                                                    onkeyup="CalculateTotal(this);" TabIndex="8"></asp:TextBox>
                                                <div class="starwrap">
                                                    <asp:CustomValidator ID="vcmOtherChatges" CssClass="star" SetFocusOnError="true"
                                                        ClientValidationFunction="ValidateOtherCharge" ValidationGroup="payment" EnableClientScript="true"
                                                        runat="server" ControlToValidate="txtOtherCharges" Display="Dynamic" Text="*"
                                                        ErrorMessage="<%$ resources:Err_OthherCharges %>"></asp:CustomValidator>
                                                    <cc1:AmountValidation ID="vamOtherChatges" runat="server" ControlToValidate="txtOtherCharges"
                                                        ErrorMessage="<%$ resources:Err_OtherCharges %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="payment"></cc1:AmountValidation>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalOtherCharges"></asp:Label></FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Tax %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalTax" runat="server"></asp:Label>
                                                <asp:HiddenField ID="hdfTotalTax" runat="server" Value="0" />
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalTaxFooter"></asp:Label></FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Adjustments %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAdjustments" runat="server" CssClass="input-w70 numeric" MaxLength="15"
                                                    TabIndex="10" onkeyup="CalculateTotal();"></asp:TextBox>
                                                <asp:HiddenField ID="hdfAdjustments" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalAdjustmentsFooter"></asp:Label></FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVE"
                                                    CommandArgument='<%#Eval("IVH_PK")%>' SkinID="delete-icon" ToolTip="Remove" OnClientClick="return ShowDeleteConfirm(this);" />
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblAdjType" Text="<%$ resources:AdjType%>" AssociatedControlID="ddlAdjType"></asp:Label>
                                            <asp:DropDownList ID="ddlAdjType" runat="server" CssClass="select-small-b" TabIndex="16">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblAdjAmount" Text="<%$ resources:AdjustAmount%>" AssociatedControlID="txtAdjAmount"></asp:Label>
                                            <asp:TextBox ID="txtAdjAmount" runat="server" TabIndex="17" MaxLength="17" Enabled="false"
                                                CssClass="input-small numeric input-disabled"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfAdjAmount" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="payment" EnableClientScript="true" runat="server" ControlToValidate="txtAdjAmount"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Adjustments%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trVendorAccount" runat="server">
                                    <td>
                                        <div class="div2col-S">
                                            <asp:HiddenField ID="hdfWHTNO" runat="server" Value="" />
                                            <asp:Label runat="server" ID="lblWHTAccount" Text="<%$ resources:WHTAccount%>" AssociatedControlID="ddlWHTAccount"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlWHTAccount" TabIndex="18" CssClass="select-small-b"
                                                Enabled="false">
                                                <asp:ListItem Value="-1" Text="<%$ resources:Custom %>"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:CheckBox ID="chkVendorforpayemnt" runat="server" TabIndex="19" OnCheckedChanged="ActionHandler"
                                                Checked="true" AutoPostBack="true" Text="<%$ resources:WHTFromVendor %>" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblWHTAmount" Text="<%$ resources:WHTAmount%>" AssociatedControlID="txtWHTAmount"></asp:Label>
                                            <asp:TextBox ID="txtWHTAmount" runat="server" TabIndex="20" MaxLength="17" Enabled="false"
                                                CssClass="input-small Uiinput-amount numeric input-disabled"></asp:TextBox>
                                            <asp:ImageButton ID="imgHdrTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                ValidationGroup="taxDate" ToolTip="<%$ resources:Tax %>" CommandName="WHTTAXHEADER" />
                                            <asp:ImageButton SkinID="btnPrint" runat="server" ID="imgbtnPrint" TabIndex="20"
                                                OnClick="ActionHandler" CommandName="PRINTWHT" ToolTip="<%$ resources:WHTPrint%>" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <div id="divTax">
                                                <asp:Label runat="server" ID="lblTaxAmount" Text="<%$ resources:TaxAmount%>" AssociatedControlID="txtTaxAmount"></asp:Label>
                                                <asp:TextBox ID="txtTaxAmount" runat="server" TabIndex="21" MaxLength="17" Enabled="false"
                                                    CssClass="medium numeric input-disabled"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfTaxAmount" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="payment" EnableClientScript="true" runat="server" ControlToValidate="txtTaxAmount"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TaxAmount%>"></asp:RequiredFieldValidator>
                                                <asp:ImageButton ID="imgVatBuy" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                    ToolTip="<%$ resources:Tax %>" CommandName="VATTAXHEADER" />
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblPaidAmount" Text="<%$ resources:PaidAmount%>" AssociatedControlID="txtPaidAmount"></asp:Label>
                                            <asp:TextBox ID="txtPaidAmount" runat="server" TabIndex="22" MaxLength="17" Enabled="false"
                                                CssClass="input-small numeric input-disabled"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfPaidAmount" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="payment" EnableClientScript="true" runat="server" ControlToValidate="txtPaidAmount"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PaidAmount%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                <h1>
                                    <%= GetLocalResourceObject("PaymentDetails").ToString()%></h1>
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblMode" Text="<%$ resources:Mode%>" AssociatedControlID="ddlMode"></asp:Label>
                                                    <asp:DropDownList ID="ddlMode" runat="server" CssClass="select-small-b" AutoPostBack="true"
                                                        TabIndex="10" OnSelectedIndexChanged="ActionHandler">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="vrfMode" CssClass="star" SetFocusOnError="true" ValidationGroup="paymentDet"
                                                        EnableClientScript="true" InitialValue="-1" runat="server" ControlToValidate="ddlMode"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Mode %>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="vrfModeHdr" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="payment" EnableClientScript="true" InitialValue="-1" runat="server"
                                                        ControlToValidate="ddlMode" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Mode %>">
                                                    </asp:RequiredFieldValidator>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblBranch" Text="<%$ resources:Branch%>" AssociatedControlID="txtBranch"></asp:Label>
                                                    <asp:TextBox ID="txtBranch" runat="server" TabIndex="12" Enabled="false" CssClass="input-disabled select-half"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfBranch" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="paymentDet" EnableClientScript="true" runat="server" ControlToValidate="txtBranch"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Branch%>"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="vrfBranchHdr" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="payment" EnableClientScript="true" runat="server" ControlToValidate="txtBranch"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Branch%>"></asp:RequiredFieldValidator>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblInstrumentNo" Text="<%$ resources:InstrumentNo%>"
                                                        AssociatedControlID="txtInstrumentNo"></asp:Label>
                                                    <asp:TextBox ID="txtInstrumentNo" runat="server" MaxLength="50" TabIndex="14" CssClass="input-small"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfInstrumentNo" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="paymentDet" EnableClientScript="true" runat="server" ControlToValidate="txtInstrumentNo"
                                                        Display="Dynamic" Text="*" Enabled="false" ErrorMessage="<%$ resources:Err_InstrumentNo%>"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="vrfInstrumentNoHdr" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="payment" EnableClientScript="true" runat="server" ControlToValidate="txtInstrumentNo"
                                                        Display="Dynamic" Text="*" Enabled="false" ErrorMessage="<%$ resources:Err_InstrumentNo%>"></asp:RequiredFieldValidator>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblBank" Text="<%$ resources:BankName %>" AssociatedControlID="txtPaymentBank"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtPaymentBank" TabIndex="11" CssClass="input-half"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfPaymentBank" runat="server" />
                                                    <asp:RequiredFieldValidator ID="vrfBankName" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="paymentDet" EnableClientScript="true" runat="server" ControlToValidate="txtPaymentBank"
                                                        Display="Dynamic" Text="*" InitialValue="<%$ resources:Messages, AutoDefaultValue %>"
                                                        ErrorMessage="<%$ resources:Err_BankName %>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="vrfBankNameHdr" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="payment" EnableClientScript="true" runat="server" ControlToValidate="txtPaymentBank"
                                                        Display="Dynamic" Text="*" InitialValue="<%$ resources:Messages, AutoDefaultValue %>"
                                                        ErrorMessage="<%$ resources:Err_BankName %>">
                                                    </asp:RequiredFieldValidator>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblAccountNo" Text="<%$ resources:AccNo%>" AssociatedControlID="txtAccountNo"></asp:Label>
                                                    <asp:TextBox ID="txtAccountNo" runat="server" CssClass="input-disabled select-half"
                                                        TabIndex="13" Enabled="false">
                                                    </asp:TextBox>
                                                    <asp:HiddenField ID="hdfBankAccount" runat="server" />
                                                    <asp:Button ID="btnPaymentAccountNo" runat="server" Text="<%$ resources:Controls,Search %>"
                                                        OnClick="ActionHandler" TabIndex="3" CommandName="SEARCHACCOUNTNO" SkinID="btnInner-search"
                                                        EnableTheming="false" Style="display: none" />
                                                    <asp:RequiredFieldValidator ID="vrfAccountNo" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="paymentDet" EnableClientScript="true" runat="server" ControlToValidate="txtAccountNo"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_AccNo%>"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="vrfAccountNoHdr" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="payment" EnableClientScript="true" runat="server" ControlToValidate="txtAccountNo"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_AccNo%>"></asp:RequiredFieldValidator>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblInstrumentDate" Text="<%$ resources:InstrumentDate%>"
                                                        AssociatedControlID="txtInstrumentDate"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtInstrumentDate" Enabled="false" CssClass="select-small"
                                                        TabIndex="15" onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfInstrumentDate" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="paymentDet" EnableClientScript="true" runat="server" ControlToValidate="txtInstrumentDate"
                                                        Display="Dynamic" Text="*" Enabled="false" ErrorMessage="<%$ resources:Err_InstrumentDate %>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="vrfInstrumentDateHdr" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="payment" EnableClientScript="true" runat="server" ControlToValidate="txtInstrumentDate"
                                                        Display="Dynamic" Text="*" Enabled="false" ErrorMessage="<%$ resources:Err_InstrumentDate %>">
                                                    </asp:RequiredFieldValidator>
                                                    <input type="checkbox" id="chkPDC" runat="server" />
                                                    <label id="lblPDC" runat="server" style="text-align: left;">
                                                        <%=GetLocalResourceObject("PDC") %></label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="trBankCharge" runat="server">
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="Label5" Text="<%$ resources:BankCharge%>" AssociatedControlID="txtBankCharge"></asp:Label>
                                                    <asp:TextBox ID="txtBankCharge" runat="server" TabIndex="21" MaxLength="17" CssClass="numeric medium input-small"></asp:TextBox>
                                                    <cc1:AmountValidation ID="vamBankCharge" runat="server" ControlToValidate="txtBankCharge"
                                                        ErrorMessage="<%$ resources:Err_BankCharge %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="paymentDet"></cc1:AmountValidation>
                                                    <cc1:AmountValidation ID="vamBankChargeHdr" runat="server" ControlToValidate="txtBankCharge"
                                                        ErrorMessage="<%$ resources:Err_BankCharge %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="payment"></cc1:AmountValidation>
                                                    <asp:CustomValidator ID="csvBankCharge" runat="server" Display="Dynamic" CssClass="star"
                                                        SetFocusOnError="true" Text="*" ControlToValidate="txtBankCharge" EnableClientScript="true"
                                                        ClientValidationFunction="CheckBankCharge" ErrorMessage="<%$ resources:Err_ValidBankCharge %> "
                                                        ValidationGroup="paymentDet"></asp:CustomValidator>
                                                    <asp:CustomValidator ID="csvBankChargeHdr" runat="server" Display="Dynamic" CssClass="star"
                                                        SetFocusOnError="true" Text="*" ControlToValidate="txtBankCharge" EnableClientScript="true"
                                                        ClientValidationFunction="CheckBankCharge" ErrorMessage="<%$ resources:Err_ValidBankCharge %> "
                                                        ValidationGroup="payment"></asp:CustomValidator>
                                                    <asp:CheckBox ID="chkBankCharge" runat="server" TabIndex="21" Checked="false" Text="<%$ resources:FromVendor%>"
                                                        CssClass="disable lft-lbl" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblBankCurrency" Text="<%$ resources:BankCurrency%>"
                                                        AssociatedControlID="ddlBankChargeCurrency"></asp:Label>
                                                    <asp:DropDownList ID="ddlBankChargeCurrency" CssClass="select-small-b" TabIndex="20"
                                                        runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblPaymentAmount" Text="<%$ resources:PaymentAmount%>"
                                                        AssociatedControlID="txtPaymentAmount"></asp:Label>
                                                    <asp:TextBox ID="txtPaymentAmount" runat="server" TabIndex="19" MaxLength="12" CssClass="numeric input-small"
                                                        onkeyup="CalculateTotalBC();"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblTotalAmountBC" Text="<%$ resources:TotalAmountBC%>"
                                                        AssociatedControlID="txtTotalAmountBC"></asp:Label>
                                                    <asp:TextBox ID="txtTotalAmountBC" runat="server" Enabled="false" TabIndex="19" MaxLength="17"
                                                        CssClass="input-small numeric input-disabled"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfTotalAmountBC" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="paymentDet" EnableClientScript="true" runat="server" ControlToValidate="txtTotalAmountBC"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TotalAmountBC%>"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="vrfTotalAmountBCHdr" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="payment" EnableClientScript="true" runat="server" ControlToValidate="txtTotalAmountBC"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TotalAmountBC%>"></asp:RequiredFieldValidator>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="divcol-S">
                                                    <asp:HiddenField ID="hdfFavourof" runat="server" Value="" />
                                                    <asp:Label runat="server" ID="lblFavourof" Text="<%$ resources:Favourof %>" AssociatedControlID="txtRemarks"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtFavourof" Enabled="false" TabIndex="23" TextMode="MultiLine"
                                                        Width="549" CssClass="multiline-2line" onkeydown="limitText(this,150);" onkeyup="limitText(this,150);"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfFavourof" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="paymentDet" EnableClientScript="true" runat="server" ControlToValidate="txtFavourof"
                                                        Display="Dynamic" Text="*" Enabled="false" ErrorMessage="<%$ resources:Err_Favourof%>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="vrfFavourofHdr" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="payment" EnableClientScript="true" runat="server" ControlToValidate="txtFavourof"
                                                        Display="Dynamic" Text="*" Enabled="false" ErrorMessage="<%$ resources:Err_Favourof%>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:ImageButton SkinID="add_small-icon" runat="server" ID="imgAddPayemntMode" OnClick="ActionHandler"
                                                        OnClientClick="javascript:ValidatePageNow('paymentDet')" CommandName="ADDPAYMENTMODE"
                                                        ToolTip="<%$ resources:PaymentDetails%>" ValidationGroup="paymentDet" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                            <asp:Panel ID="pnlPaymentModesList" runat="server" Visible="false">
                                <div class="gridwrap">
                                    <asp:GridView runat="server" ID="grdPaymentModes" Width="100%" AllowSorting="false"
                                        AutoGenerateColumns="false" OnRowDataBound="ActionHandler" EmptyDataRowStyle-CssClass="emptytable"
                                        ShowFooter="true">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:Mode %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPymntMode" runat="server"></asp:Label>
                                                    <asp:HiddenField ID="hdfPymntMode" runat="server" Value='<%#Eval("PDM_MODE") %>' />
                                                    <asp:HiddenField ID="hdfPymntBankChargeType" runat="server" Value='<%#Eval("PDM_BANK_CHARGE_TYPE") %>' />
                                                    <asp:HiddenField ID="hdfPymntAccount" runat="server" Value='<%#Eval("PDM_ACCOUNT") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Button ID="btnPymntModePDCFlag" runat="server" OnClientClick="javascript:return false;"
                                                        CssClass='<%# (Eval("PDM_PDC").ToString() != "0" ? (((Eval("PDM_PDC").ToString() == "1") || (Eval("PDM_PDC").ToString() == "3")) ? "flaggrey-icon" : "flaggreen-icon"): "")  %>'
                                                        ToolTip='<%# (Eval("PDM_PDC").ToString() != "0" ? (((Eval("PDM_PDC").ToString() == "1") || (Eval("PDM_PDC").ToString() == "3")) ? GetLocalResourceObject("PDC_Cheque").ToString() : GetLocalResourceObject("Cheque_Reversed").ToString()) : "") %>'
                                                        Visible='<%# (Eval("PDM_MODE").ToString() != "2" ? false : (Eval("PDM_BOUNCED").ToString() != "0" ? false : (Eval("PDM_PDC").ToString() != "0" ? true : false)))%>' />
                                                    <asp:Button ID="btnPymntModePDCReturn" runat="server" OnClientClick="javascript:return false;"
                                                        CssClass='<%#(Eval("PDM_BOUNCED").ToString() != "0" ? "return-icon" : "")  %>'
                                                        ToolTip='<%# (Eval("PDM_BOUNCED").ToString() != "0" ? GetLocalResourceObject("PDC_Return").ToString() : "") %>'
                                                        Visible='<%# (Eval("PDM_BOUNCED").ToString() != "0" ? true : false) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:BankName %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPymntBankName" runat="server"></asp:Label>
                                                    <asp:HiddenField ID="hdfPymntBankPk" runat="server" Value='<%#Eval("PDM_BANK") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="18%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Branch %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPymntBranch" runat="server" Text='<%#Eval("PDM_BRANCH") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:AccNo %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPymntAccountNo" runat="server" Text=""></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:InstrNo %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPymntInstrumentNo" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("PDM_INSTR_NO"),15) %>'
                                                        ToolTip='<%#Eval("PDM_INSTR_NO") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="17%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:InstrDate %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPymntInstrumentDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.PDM_INSTR_DATE, Resources.Constants.DateFormatGrid).ToString() %>'
                                                        ToolTip='<%# Eval(Resources.DataFieldRes.PDM_INSTR_DATE, Resources.Constants.DateFormatGrid).ToString() %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:ExchRate %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPymntExchangeRate" runat="server" Text='<%# Eval("PDM_EXCHG_RATE") %>'
                                                        ToolTip='<%# Eval("PDM_EXCHG_RATE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:BankCur %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPymntBankCurrency" runat="server"></asp:Label>
                                                    <asp:HiddenField ID="hdfPymntBankCurrency" runat="server" Value='<%#Eval("PDM_BANK_CHARGE_CURR") %>' />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblPmntfooterTotal" Text="<%$ resources:Total %>"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="5%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:BankCharge %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPymntBankCharge" runat="server" Text='<%# Eval("PDM_BANK_CHARGE","{0:c}") %>'
                                                        ToolTip='<%# Eval("PDM_BANK_CHARGE","{0:c}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Right" Wrap="false" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblHdrPymntTotalAmount" runat="server"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPymntTotalAmount" runat="server" Text='<%# Eval("PDM_PAID_AMOUNT","{0:c}") %>'
                                                        ToolTip='<%# Eval("PDM_PAID_AMOUNT","{0:c}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblPymntTotalAmountFooter" runat="server" Text="" ToolTip=""></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle Width="20%" HorizontalAlign="Right" Wrap="false" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblHdrPymntTotalAmountBC" runat="server"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPymntTotalAmountBC" runat="server" Text='<%# Eval("PDM_PAID_AMOUNT_BC","{0:c}") %>'
                                                        ToolTip='<%# Eval("PDM_PAID_AMOUNT_BC","{0:c}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblPymntTotalAmountBCFooter" runat="server" Text="" ToolTip=""></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle Width="20%" HorizontalAlign="Right" Wrap="false" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imbWhtTaxEdit" runat="server" SkinID="imbeditgrid" CommandName="EDITGRID"
                                                        OnClick="ActionHandler" ToolTip="Edit" />
                                                    <asp:ImageButton ID="imbTaxRemove" runat="server" OnClick="ActionHandler" CommandName="DELETEGRID"
                                                        SkinID="btnclose" ToolTip="Remove" />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" Wrap="false" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                            <div class="clear">
                            </div>
                            <table class="table-devide">
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks %>" AssociatedControlID="txtRemarks"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRemarks" TabIndex="24" TextMode="MultiLine" CssClass="multiline-2line"
                                                onkeydown="limitText(this,450);" onkeyup="limitText(this,450);"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
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
                                        TabIndex="66" /></div>
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <table class="table-devide" id="tblUploadDocDetails">
                                        <tr>
                                            <td colspan="2">
                                                <div class="divcol-S">
                                                    <asp:Label ID="lblFileUpload" runat="server" Text="AttachFile" AssociatedControlID="fupUpload"></asp:Label>
                                                    <div class="fileupload-main">
                                                        <asp:FileUpload ID="fupUpload" runat="server" TabIndex="26" CssClass="margn-rgt0 upload-area" />
                                                        <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupUpload"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_File_Upload %>">                                                        
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                    <a id="anchorFile" runat="server" target="_blank" tabindex="11"></a>
                                                    <asp:Button runat="server" ID="btnUpload" CommandName="ADDITEM" TabIndex="12" OnClick="ActionHandler"
                                                        OnClientClick="javascript:ValidatePageNow('upload')" ToolTip="<%$resources:ErpRes,Add %>"
                                                        CommandArgument="PageAction_Entry" ValidationGroup="upload" Text="<%$resources:ErpRes,Add %>"
                                                        SkinID="btnInner-add" />
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
                                                                        target="_blank" href='<%# Page.ResolveClientUrl(Eval("DOC_PATH").ToString()) %>'>
                                                                    </a>
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
                                                                    <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
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
                            <%----------WHT Popup Start--------%>
                            <div id="divItemTax" style="display: none">
                                <div class="Button-container-popup">
                                    <asp:Button runat="server" ID="btnWhtSave" CommandName="WHTTAXSAVE" Text="<%$resources:Controls,Save %>"
                                        OnClick="ActionHandler" ToolTip="<%$resources:Controls,Save %>" CommandArgument="PageAction_Entry"
                                        SkinID="btnInner-Save" />
                                    <asp:Button ID="btnApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply" OnClick="ActionHandler"
                                        CommandArgument="PageAction_Entry" CommandName="WHTTAXAPPLY" />
                                </div>
                                <div class="content-wrapper">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:Label runat="server" ID="lblformno" Text="<%$ resources:formno%>" AssociatedControlID="ddlFormno"></asp:Label>
                                                    <asp:DropDownList ID="ddlFormno" runat="server" Width="121px" TabIndex="16">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="vrfFormno" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="wht" EnableClientScript="true" InitialValue="-1" runat="server"
                                                        ControlToValidate="ddlFormno" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Form %>">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:Label runat="server" ID="lblTaxid" Text="<%$ resources:Taxid%>" AssociatedControlID="txtTaxid"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtTaxid" TabIndex="5" MaxLength="100"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:HiddenField ID="hdfWthAddressType" runat="server" Value="" />
                                                    <asp:Label runat="server" ID="lblWhtHoBr" Text="<%$ resources:HoBr%>" AssociatedControlID="txtWthAddressType"></asp:Label>
                                                    <asp:TextBox ID="txtWthAddressType" runat="server" MaxLength="100" TabIndex="62"
                                                        onkeydown="ClearWhtVendorContacts();" Width="190" OnTextChanged="ActionHandler"> </asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfWthAddressType" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="wht" EnableClientScript="true" runat="server" ControlToValidate="txtWthAddressType"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_HoBr %>" InitialValue="<%$ resources:Messages, AutoDefaultValue %>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:Label runat="server" ID="lblWhtHo" Text="<%$ resources:Ho%>" AssociatedControlID="chkWthHeadOffice"
                                                        Width="30"></asp:Label>
                                                    <asp:CheckBox ID="chkWthHeadOffice" runat="server" Width="13" CssClass="check-inline">
                                                    </asp:CheckBox>
                                                    <asp:Button ID="btnWHTVendor" runat="server" OnClick="ActionHandler" CommandName="WHTCHANGETYPE"
                                                        Style="display: none" EnableTheming="false" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:Label runat="server" ID="lblWthBranchCode" Text="<%$ resources:BranchCode%>"
                                                        AssociatedControlID="txtWthBranchCode"></asp:Label>
                                                    <asp:TextBox ID="txtWthBranchCode" runat="server" MaxLength="5" TabIndex="63" Width="100px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="divcol-P">
                                                    <asp:Label ID="lblvendorWHT" runat="server" Text="<%$ resources:Party%>" AssociatedControlID="txtCustomerTxtWHT"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtCustomerTxtWHT" TabIndex="5"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfvendorWHTPK" runat="server" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="divcol-P">
                                                    <asp:Label runat="server" ID="lbladdress" Text="<%$ resources:PartyAds %>" AssociatedControlID="txtpartyads"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtpartyads" TabIndex="23" TextMode="MultiLine" CssClass="multiline-2line"
                                                        onkeydown="limitText(this,150);" onkeyup="limitText(this,150);"></asp:TextBox>
                                                </div>
                                                <hr />
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="table-3devide">
                                        <tr>
                                            <td align="right" style="width: 30%;">
                                                <div class="div3col-S">
                                                    <asp:HiddenField ID="hdfWHTNOPopup" runat="server" Value="" />
                                                    <asp:Label runat="server" ID="lblWhtAccountPopup" Text="<%$ resources:WHTAccount%>"
                                                        AssociatedControlID="txtWHTAccountPopup"></asp:Label>
                                                    <asp:TextBox ID="txtWHTAccountPopup" runat="server" MaxLength="100" TabIndex="2"> </asp:TextBox>
                                                    <asp:HiddenField ID="hdfWHTAccountPopup" runat="server" Value="" />
                                                    <asp:RequiredFieldValidator ID="vrfWHTAccountPopup" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="wht" EnableClientScript="true" runat="server" ControlToValidate="txtWHTAccountPopup"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_WHTAccount %>" InitialValue="<%$ resources:Messages, AutoDefaultValue %>">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </td>
                                            <td style="width: 22%;">
                                                <div>
                                                    <asp:Label runat="server" ID="lbPayType" Text="<%$ resources:PaymentType %>" AssociatedControlID="ddlPayType"
                                                        Width="92"></asp:Label>
                                                    <asp:DropDownList ID="ddlPayType" runat="server" Width="80px">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="vrfddlPayType" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="wht" EnableClientScript="true" runat="server" ControlToValidate="ddlPayType"
                                                        InitialValue="-1" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ValidPayType %>">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </td>
                                            <td align="center" style="width: 17%;">
                                                <div>
                                                    <asp:Label ID="lblWHTAmountPopup" runat="server" Text="<%$ resources:Amount %>" AssociatedControlID="txtPopupWHTAmount"></asp:Label>
                                                    <asp:TextBox ID="txtPopupWHTAmount" TabIndex="20" runat="server" CssClass="input-w70 numeric"
                                                        onkeypress="return validateFloatKeyPress(this,event);" EnableViewState="false"
                                                        MaxLength="11" onkeyup="CalculateWHTTotal(this);"></asp:TextBox>
                                                    <cc1:AmountValidation ID="vamWHTAmountPopup" runat="server" ControlToValidate="txtPopupWHTAmount"
                                                        ErrorMessage="<%$ resources:Err_ValidWHTAmount %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="wht" NonZero="true"></cc1:AmountValidation>
                                                    <asp:RequiredFieldValidator ID="vrfWHTAmountPopup" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="wht" EnableClientScript="true" runat="server" ControlToValidate="txtPopupWHTAmount"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ValidWHTAmount %>">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </td>
                                            <td style="width: 20%;">
                                                <div>
                                                    <asp:Label ID="lblWHTTaxAmountPopup" runat="server" Text="<%$ resources:WHTAmount %>"
                                                        AssociatedControlID="txtWHTTaxAmountPopup"></asp:Label>
                                                    <asp:TextBox ID="txtWHTTaxAmountPopup" TabIndex="20" runat="server" CssClass="input-w70 numeric"
                                                        EnableViewState="false" onkeypress="return validateFloatKeyPress(this,event);"
                                                        MaxLength="11"></asp:TextBox>
                                                    <cc1:AmountValidation ID="vamWHTTaxAmountPopup" runat="server" ControlToValidate="txtWHTTaxAmountPopup"
                                                        ErrorMessage="<%$ resources:Err_WHTTaxAmount %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="wht" NonZero="true"></cc1:AmountValidation>
                                                    <asp:RequiredFieldValidator ID="vrfWHTTaxAmountPopup" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="wht" EnableClientScript="true" runat="server" ControlToValidate="txtWHTTaxAmountPopup"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_WHTTaxAmount %>">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </td>
                                            <asp:Button runat="server" ID="btnWHTAccountPopup" CommandName="WHT_ACCOUNT_INDEX_CHANGED_POPUP"
                                                OnClick="ActionHandler" EnableTheming="false" Style="display: none" />
                                        </tr>
                                    </table>
                                    <div class="divcol-P">
                                        <asp:HiddenField ID="hdfWHTTaxCategory" runat="server" />
                                        <asp:HiddenField ID="hdfWHTTaxName" runat="server" />
                                        <asp:Label ID="lblDescriptionPopup" runat="server" Text="<%$ resources:Description %>"
                                            AssociatedControlID="txtDescriptionPopup"></asp:Label>
                                        <asp:TextBox ID="txtDescriptionPopup" TabIndex="20" runat="server" EnableViewState="false"
                                            MaxLength="400" Width="70%"></asp:TextBox>
                                        <asp:ImageButton ID="imgPopupAdd" SkinID="imbaddnew" runat="server" OnClick="ActionHandler"
                                            CommandArgument="PageAction_Entry" ValidationGroup="wht" ToolTip="Add" CommandName="WHTTAXADD"
                                            OnClientClick="javascript:ValidatePageNow('wht')" />
                                    </div>
                                    <div class="error" id="divWhtErrorLabel" runat="server" visible="false">
                                        <ul>
                                            <li>
                                                <asp:Literal runat="server" ID="lblWhtErrorMessage" Text=""></asp:Literal>
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdWHTTaxDetails" Width="100%" AllowSorting="false"
                                            AutoGenerateColumns="false" OnRowDataBound="ActionHandler" TabIndex="22" EmptyDataRowStyle-CssClass="emptytable"
                                            ShowFooter="true">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:formno %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblformnoGRD" runat="server"></asp:Label>
                                                        <asp:HiddenField ID="hdfWHTFormNo" runat="server" Value='<%#Eval("WTH_FORM_NO") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Party %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblParty" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("WTH_PARTY_NAME"),17) %>'
                                                            ToolTip='<%# Eval("WTH_PARTY_NAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="27%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:PartyAds %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPrtyAddress" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("WTH_ADDRESS"),30) %>'
                                                            ToolTip='<%# Eval("WTH_ADDRESS") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="27%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Type %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfWhtBranchType" runat="server" Value='<%#Eval("WTH_BRANCH_TYPE") %>' />
                                                        <asp:Label ID="lblWhtTye" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:HoBr %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWhtBranchName" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("WTH_BRANCH_NAME"),30) %>'
                                                            ToolTip='<%#Eval("WTH_BRANCH_NAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Taxid %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTxId" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("WTH_TAX_ID"),30) %>'
                                                            ToolTip='<%# Eval("WTH_TAX_ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:WHTAccount %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfWHTTaxPK" runat="server" Value='<%#Eval("WTH_PK") %>' />
                                                        <asp:HiddenField ID="hdfWHTTax" runat="server" Value='<%#Eval("WTH_TAX") %>' />
                                                        <asp:HiddenField ID="hdfWHTTaxCategoryGrid" runat="server" Value='<%#Eval("WTH_TAX_CATEGORY") %>' />
                                                        <asp:Label ID="lblWHTAccountGrid" runat="server" Text='<%# Convert.ToString(Eval("WTH_NAME")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("WTH_NAME")) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("WTH_NAME")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("WTH_NAME"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="22%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Amount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("WTH_AMOUNT")) %>'
                                                            ToolTip='<%# GetFormattedCurrency(Eval("WTH_AMOUNT")) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfWHTTaxName" runat="server" Value='<%#Eval("WTH_NAME") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:WHTAmount %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfWhtTaxAmount" runat="server" Value='<%#Eval("WTH_TAX_AMT") %>' />
                                                        <asp:Label ID="lblWHTTaxAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("WTH_TAX_AMT")) %>'
                                                            ToolTip='<%#GetFormattedCurrency(Eval("WTH_TAX_AMT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblWhtTaxTotal" runat="server" />
                                                    </FooterTemplate>
                                                    <FooterStyle Font-Bold="true" HorizontalAlign="Right" />
                                                    <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Desc %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWHTDesc" runat="server" Text='<%#Eval("WTH_DESC") %>' ToolTip='<%#Eval("WTH_DESC") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imbWhtTaxEdit" runat="server" SkinID="imbeditgrid" CommandName="POPUPGRIDEDITWHT"
                                                            OnClick="ActionHandler" ToolTip="Edit" />
                                                        <asp:ImageButton ID="imbTaxRemove" runat="server" OnClick="ActionHandler" CommandName="WHTTAXDELETE"
                                                            CommandArgument="PageAction_Entry" SkinID="btnclose" ToolTip="Remove" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="8%" Wrap="false" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <%----------WHT Popup End--------%>
                            <%----------VAT BUY Popup Start--------%>
                            <div id="divVatBuy" style="display: none">
                                <div class="Button-container-popup">
                                    <asp:Button runat="server" ID="btnVatTaxSave" CommandName="VATTAXSAVE" Text="<%$resources:Controls,Save %>"
                                        OnClick="ActionHandler" ToolTip="<%$resources:Controls,Save %>" CommandArgument="PageAction_Entry"
                                        SkinID="btnInner-Save" TabIndex="75" />
                                    <asp:Button ID="btnVatTaxApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply"
                                        OnClick="ActionHandler" CommandArgument="PageAction_Entry" CommandName="VATTAXAPPLY"
                                        TabIndex="75" />
                                </div>
                                <div id="Div1" class="detail-co2" runat="server">
                                    <div class="div2col-S">
                                        <asp:Label ID="lblVatVendorPopup" runat="server" AssociatedControlID="lblVatVendorHdr"
                                            Text="<%$resources:Vendor %>"></asp:Label>
                                        <asp:Label ID="lblVatVendorHdr" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblTaxAmnt" runat="server" AssociatedControlID="lblVatTaxAmountHdr"
                                            Text="<%$resources:TaxNyd %>"></asp:Label>
                                        <asp:Label ID="lblVatTaxAmountHdr" runat="server"></asp:Label>
                                        <asp:HiddenField ID="hdfVatTaxAmountHdr" runat="server" />
                                    </div>
                                    <div class="clear">
                                    </div>
                                </div>
                                <div class="content-wrapper">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:Label ID="Label7" runat="server" Text="<%$ resources:Vendor %>" AssociatedControlID="txtVendorPopup"></asp:Label>
                                                    <asp:TextBox ID="txtVendorPopup" runat="server" MaxLength="100" TabIndex="60" onkeydown="ClearVendorContacts();"
                                                        OnTextChanged="ActionHandler" AutoPostBack="true"> </asp:TextBox>
                                                    <asp:HiddenField ID="hdfVendorPopup" runat="server" />
                                                    <asp:HiddenField ID="hdfVendorVatPK" runat="server" />
                                                    <asp:Button ID="btnVendorPopup" runat="server" OnClick="ActionHandler" CommandName="VENDORSELECTEDDTL"
                                                        Style="display: none" EnableTheming="false" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:Label runat="server" ID="lblPurInvNo" Text="<%$ resources:PurInvoiceNo%>" AssociatedControlID="ddlPurInvNo"></asp:Label>
                                                    <asp:DropDownList ID="ddlPurInvNo" runat="server" TabIndex="61">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="vrfddlPurInvNo" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="vatbuy" EnableClientScript="true" InitialValue="" runat="server"
                                                        ControlToValidate="ddlPurInvNo" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PurInvoiceNo %>">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:HiddenField ID="hdfAddressType" runat="server" Value="" />
                                                    <asp:Label runat="server" ID="lblAddressType" Text="<%$ resources:HoBr%>" AssociatedControlID="txtAddressType"></asp:Label>
                                                    <asp:TextBox ID="txtAddressType" runat="server" MaxLength="100" TabIndex="62" Width="190"
                                                        OnTextChanged="ActionHandler" AutoPostBack="true"> </asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfAddressType" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="vatbuy" EnableClientScript="true" runat="server" ControlToValidate="txtAddressType"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_HoBr %>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:Label runat="server" ID="lblHO" Text="<%$ resources:Ho%>" AssociatedControlID="chkHeadOffice"
                                                        Width="62"></asp:Label>
                                                    <asp:CheckBox ID="chkHeadOffice" runat="server" Width="13" CssClass="check-inline"
                                                        OnCheckedChanged="ActionHandler" AutoPostBack="true"></asp:CheckBox>
                                                    <asp:Button ID="btnVendorContDtl" runat="server" OnClick="ActionHandler" CommandName="CHANGETYPE"
                                                        Style="display: none" EnableTheming="false" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:Label runat="server" ID="lblBranchCode" Text="<%$ resources:BranchCode%>" AssociatedControlID="txtBranchCode"></asp:Label>
                                                    <asp:TextBox ID="txtBranchCode" runat="server" MaxLength="5" TabIndex="63" Width="87" />
                                                    <asp:RequiredFieldValidator ID="vrfBranchCode" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="vatbuy" EnableClientScript="true" runat="server" ControlToValidate="txtBranchCode"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_BranchCode %>">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:Label runat="server" ID="lblRefundDate" Text="<%$ resources:RefundMonth %>"
                                                        AssociatedControlID="txtVatRefundDate"></asp:Label>
                                                    <asp:TextBox ID="txtVatRefundDate" runat="server" MaxLength="200" CssClass="medium"
                                                        TabIndex="65" onkeydown="return CheckKey(event)" onpaste="return false;" />
                                                    <cc1:CalendarExtender runat="server" ID="txtVatRefundDate_CalendarExtender" BehaviorID="calendar1"
                                                        TargetControlID="txtVatRefundDate" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                                                        ClientIDMode="Static" OnClientHidden="onCalendarHidden">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="vrfVatRefundDate" runat="server" CssClass="star"
                                                        SetFocusOnError="true" InitialValue="" ValidationGroup="vatbuy" EnableClientScript="true"
                                                        Display="Dynamic" Text="*" ControlToValidate="txtVatRefundDate" ErrorMessage="<%$ resources:Err_RefundDate %>" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:Label runat="server" ID="lblVatBuyTaxId" Text="<%$ resources:Taxid%>" AssociatedControlID="txtVatTaxId"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtVatTaxId" TabIndex="64" MaxLength="100"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:Label runat="server" ID="lblVatTaxInvDate" Text="<%$ resources:TaxInvoiceDate%>"
                                                        AssociatedControlID="txtVatTaxInvDate"></asp:Label>
                                                    <asp:TextBox ID="txtVatTaxInvDate" runat="server" MaxLength="200" CssClass="medium"
                                                        TabIndex="65" onkeydown="return CheckKey(event)" onpaste="return false;" />
                                                    <asp:RequiredFieldValidator ID="vrfVatTaxInvDate" runat="server" CssClass="star"
                                                        SetFocusOnError="true" InitialValue="" ValidationGroup="vatbuy" EnableClientScript="true"
                                                        Display="Dynamic" Text="*" ControlToValidate="txtVatTaxInvDate" ErrorMessage="<%$ resources:Err_Invoicedate %>" />
                                                    <asp:Label runat="server" ID="lblOriginalInv" Text="<%$ resources:OriginalInvReceived%>"
                                                        AssociatedControlID="chkOriginalinvoice" Width="136"></asp:Label>
                                                    <asp:CheckBox ID="chkOriginalinvoice" runat="server" Width="13" CssClass="check-inline">
                                                    </asp:CheckBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:Label runat="server" ID="lblVatTaxInvNo" Text="<%$ resources:TaxInvoiceNo%>"
                                                        AssociatedControlID="txtVatTaxInvNo"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtVatTaxInvNo" TabIndex="66" MaxLength="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfTaxInvNo" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="vatbuy" EnableClientScript="true" runat="server" ControlToValidate="txtVatTaxInvNo"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TaxInvoiceNo %>">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:Label ID="lblAmtBeforeTax" runat="server" Text="<%$ resources:AmountBeforeTax %>"
                                                        AssociatedControlID="txtBeforeTaxAmount"></asp:Label>
                                                    <asp:TextBox ID="txtBeforeTaxAmount" TabIndex="67" runat="server" CssClass="input-w80 numeric"
                                                        onkeypress="return validateFloatKeyPress(this,event);" EnableViewState="false"
                                                        MaxLength="11" onkeyup="CalculateVatBuyTotal(this);"></asp:TextBox>
                                                    <cc1:AmountValidation ID="vamAmtBeforeTax" runat="server" ControlToValidate="txtBeforeTaxAmount"
                                                        ErrorMessage="<%$ resources:Err_ValidVatAmount %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="vatbuy" NonZero="true"></cc1:AmountValidation>
                                                    <asp:RequiredFieldValidator ID="vrfBeforeTaxAmount" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="vatbuy" EnableClientScript="true" runat="server" ControlToValidate="txtBeforeTaxAmount"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_BeforeTaxAmount %>">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:HiddenField ID="hdfVATNOPopup" runat="server" Value="" />
                                                    <asp:Label runat="server" ID="lblVatAccountPopup" Text="<%$ resources:VatBuy%>" AssociatedControlID="ddlVATAccountPopup"></asp:Label>
                                                    <asp:DropDownList ID="ddlVATAccountPopup" TabIndex="68" runat="server" EnableViewState="true"
                                                        CssClass="medium" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <asp:HiddenField ID="hdfVATAccountPopup" runat="server" Value="" />
                                                    <asp:RequiredFieldValidator ID="vrfVATAccountPopup" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="vatbuy" EnableClientScript="true" runat="server" ControlToValidate="ddlVATAccountPopup"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_VatBuyAccount %>"
                                                        InitialValue="-1">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtVATTaxAmountPopup" runat="server" CssClass="input-w80 numeric"
                                                        onkeypress="return validateFloatKeyPress(this,event);" EnableViewState="false"
                                                        MaxLength="11" TabIndex="69"></asp:TextBox>
                                                    <cc1:AmountValidation ID="vamVATTaxAmountPopup" runat="server" ControlToValidate="txtVATTaxAmountPopup"
                                                        ErrorMessage="<%$ resources:Err_VATTaxAmount %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="vatbuy" NonZero="true"></cc1:AmountValidation>
                                                    <asp:RequiredFieldValidator ID="vrfVATTaxAmountPopup" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="vatbuy" EnableClientScript="true" runat="server" ControlToValidate="txtVATTaxAmountPopup"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Tax %>">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                                <asp:Button runat="server" ID="btnVatBuyAccountPopup" CommandName="VAT_ACCOUNT_INDEX_CHANGED_POPUP"
                                                    OnClick="ActionHandler" EnableTheming="false" Style="display: none" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="divcol-P">
                                        <asp:HiddenField ID="hdfVATBUYTaxCategory" runat="server" />
                                        <asp:HiddenField ID="hdfVATBUYTaxName" runat="server" />
                                        <asp:Label ID="lblMaterial" runat="server" Text="<%$ resources:Material %>" AssociatedControlID="txtMaterial"></asp:Label>
                                        <asp:TextBox ID="txtMaterial" TabIndex="70" runat="server" EnableViewState="false"
                                            MaxLength="400" Width="70%"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="vrfMaterial" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="vatbuy" EnableClientScript="true" runat="server" ControlToValidate="txtMaterial"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Material %>">
                                        </asp:RequiredFieldValidator>
                                        <asp:ImageButton ID="imgVatPopupAdd" SkinID="imbaddnew" runat="server" OnClick="ActionHandler"
                                            TabIndex="71" CommandArgument="PageAction_Entry" ValidationGroup="vatbuy" ToolTip="Add"
                                            CommandName="VATTAXADD" OnClientClick="javascript:ValidatePageNow('vatbuy')" />
                                    </div>
                                    <div class="error" id="divVatErrorLabel" runat="server" visible="false">
                                        <ul>
                                            <li>
                                                <asp:Literal runat="server" ID="lblVatSplitErrorMessage" Text=""></asp:Literal>
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdVATTaxDetails" Width="100%" AllowSorting="false"
                                            TabIndex="72" AutoGenerateColumns="false" OnRowDataBound="ActionHandler" EmptyDataRowStyle-CssClass="emptytable"
                                            ShowFooter="true">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:TaxInvDate %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfVATTaxPK" runat="server" Value='<%#Eval("WTH_PK") %>' />
                                                        <asp:HiddenField ID="hdfVATTaxName" runat="server" Value='<%#Eval("WTH_NAME") %>' />
                                                        <asp:HiddenField ID="hdfTaxDate" runat="server" Value='<%#Eval("WTH_TAX_DATE") %>' />
                                                        <asp:HiddenField ID="hdfTaxId" runat="server" Value='<%#Eval("WTH_TAX_ID") %>' />
                                                        <asp:HiddenField ID="hdfIvnPk" runat="server" Value='<%#Eval("WTH_PUR_INVOICE") %>' />
                                                        <asp:HiddenField ID="hdfVendorPk" runat="server" Value='<%#Eval("WTH_VENDOR") %>' />
                                                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("WTH_TAX_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:RefundMonth %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRefundDate" runat="server" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("WTH_REFUND_DATE"))) ? string.Empty : Eval("WTH_REFUND_DATE", Resources.Constants.DateFormatGridMonthYear)%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:TaxInvNo %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVatTaxInvNo" runat="server" Text='<%# Eval("WTH_TAX_INV_NO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:PurInvoiceNo %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPurInvNo" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Party %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblParty" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("WTH_PARTY_NAME"),17) %>'
                                                            ToolTip='<%# Eval("WTH_PARTY_NAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="27%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Type %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfBranchType" runat="server" Value='<%#Eval("WTH_BRANCH_TYPE") %>' />
                                                        <asp:Label ID="lblTye" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:HoBr %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBranchName" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("WTH_BRANCH_NAME"),30) %>'
                                                            ToolTip='<%#Eval("WTH_BRANCH_NAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Taxid %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTxId" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("WTH_TAX_ID"),15) %>'
                                                            ToolTip='<%# Eval("WTH_TAX_ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                    <HeaderStyle Wrap="false" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:VatBuyAccount %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfIsVatbuyNotDue" runat="server" />
                                                        <asp:HiddenField ID="hdfVATTax" runat="server" Value='<%#Eval("WTH_TAX") %>' />
                                                        <asp:HiddenField ID="hdfVATTaxCategoryGrid" runat="server" Value='<%#Eval("WTH_TAX_CATEGORY") %>' />
                                                        <asp:Label ID="lblVATAccountGrid" runat="server" Text='<%# Convert.ToString(Eval("WTH_NAME")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("WTH_NAME")) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("WTH_NAME")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("WTH_NAME"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:Total %>" />
                                                    </FooterTemplate>
                                                    <FooterStyle Font-Bold="true" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Amount %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfAmount" runat="server" Value='<%#Eval("WTH_AMOUNT") %>' />
                                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("WTH_AMOUNT")) %>'
                                                            ToolTip='<%# GetFormattedCurrency(Eval("WTH_AMOUNT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblAmountTotal" runat="server" />
                                                    </FooterTemplate>
                                                    <FooterStyle Font-Bold="true" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:VatBuy %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfTaxAmount" runat="server" Value='<%#Eval("WTH_TAX_AMT") %>' />
                                                        <asp:Label ID="lblWHTTaxAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("WTH_TAX_AMT")) %>'
                                                            ToolTip='<%#GetFormattedCurrency(Eval("WTH_TAX_AMT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTaxTotal" runat="server" />
                                                    </FooterTemplate>
                                                    <FooterStyle Font-Bold="true" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Material %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMaterial" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("WTH_ITEM_TEXT"),30) %>'
                                                            ToolTip='<%#Eval("WTH_ITEM_TEXT") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imbVatTaxEdit" runat="server" SkinID="imbeditgrid" CommandName="POPUPGRIDEDIT"
                                                            OnClick="ActionHandler" TabIndex="73" ToolTip="Edit" />
                                                        <asp:ImageButton ID="imbVatTaxRemove" runat="server" OnClick="ActionHandler" CommandName="VATTAXDELETE"
                                                            CommandArgument="PageAction_Entry" SkinID="btnclose" ToolTip="Remove" TabIndex="74" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="8%" Wrap="false" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <%----------VAT BUY Popup End--------%>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <%--SO Split UP Popup Start--%>
                <div id="divPoSplitUp" style="display: none">
                    <div class="content-wrapper">
                        <asp:Panel runat="server" ID="Allocation_Section" CssClass="Button-container-popup">
                            <asp:Button ID="btnSavePaymentSplit" runat="server" Text="<%$ resources:Controls,Apply %>"
                                OnClick="ActionHandler" CommandName="PAYMENTSPLITSAVE" SkinID="btnInner-add-dsd"
                                CommandArgument="Allocation_Section" ValidationGroup="payemntsplit" OnClientClick="javascript:ValidatePageNow('payemntsplit')" />
                        </asp:Panel>
                        <div class="detail-co3">
                            <div class="div3col-S">
                                <asp:Label ID="LabelInv" runat="server" Text="<%$ resources:InvoiceNo1 %>" AssociatedControlID="lblInvSplitNo"></asp:Label>
                                <asp:Label ID="lblInvSplitNo" runat="server" CssClass="medium"></asp:Label>
                                <asp:Label ID="Label6" runat="server" Text="<%$ resources:Amount1 %>" AssociatedControlID="lblInvSplitAmount"></asp:Label>
                                <asp:Label ID="lblInvSplitAmount" runat="server" CssClass="medium"></asp:Label>
                            </div>
                            <div class="div3col-S">
                                <asp:Label ID="Label2" runat="server" Text="<%$ resources:Date1 %>" AssociatedControlID="lblInvSplitDate"></asp:Label>
                                <asp:Label ID="lblInvSplitDate" runat="server" CssClass="medium"></asp:Label>
                                <asp:Label ID="Label8" runat="server" Text="Paid:" AssociatedControlID="lblInvSplitReceived"></asp:Label>
                                <asp:Label ID="lblInvSplitReceived" runat="server" CssClass="medium"></asp:Label>
                            </div>
                            <div class="div3col-S">
                                <asp:Label ID="Label4" runat="server" Text="<%$ resources:Supplier1 %>" AssociatedControlID="lblInvSplitSupplier"></asp:Label>
                                <asp:Label ID="lblInvSplitSupplier" runat="server" CssClass="medium"></asp:Label>
                                <asp:Label ID="Label10" runat="server" Text="<%$ resources:PayNow1 %>" AssociatedControlID="lblInvSplitReceiveNow"></asp:Label>
                                <asp:Label ID="lblInvSplitReceiveNow" runat="server" CssClass="medium"></asp:Label>
                            </div>
                            <div class="clear">
                            </div>
                        </div>
                        <div class="error" id="divErrorLabel" runat="server" visible="false">
                            <ul>
                                <li>
                                    <asp:Literal runat="server" ID="lblSplitErrorMessage" Text="<%$ resources:error_allocation %>"></asp:Literal>
                                </li>
                            </ul>
                        </div>
                        <div class="gridwrap">
                            <asp:TableCell>
                                <div class="gridwrap">
                                    <asp:GridView ID="grdPaymentSplit" runat="server" AutoGenerateColumns="False" Width="100%"
                                        PageSize="<%$ resources:PageSize %>" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable"
                                        AllowSorting="false" ShowFooter="true" OnRowDataBound="ActionHandler">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:PONO %>">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkPONOSplit" CssClass="text-underline" runat="server" OnClick="ActionHandler"  CommandArgument='<%#Eval("PPO_PO_HDR")%>'
                                                        Text='<%#Eval("POH_NO") %>' ToolTip='<%#Eval("POH_NO") %>'  CommandName="SHOW"></asp:LinkButton>
                                                    <asp:HiddenField ID="hdfPaymentSplitPK" runat="server" Value='<%#Eval("PPO_PK") %>' />
                                                    <asp:HiddenField ID="hdfPOPK" runat="server" Value='<%#Eval("PPO_PO_HDR") %>' />
                                                    <asp:HiddenField ID="hdfCategory" runat="server" />
                                                    <asp:HiddenField ID="hdfGroup" runat="server" />
                                                    <asp:HiddenField ID="hdfTaxAmt" runat="server" />
                                                    <asp:HiddenField ID="hdfTaxHdrDtlAmt" runat="server" />
                                                    <asp:HiddenField ID="hdfInvPoOtherAmnt" runat="server" Value='<%# GetFormattedCurrency(Convert.ToDouble(Eval("POH_INVOICE_OTH_AMT")))%>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:PODate %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPODateSplit" runat="server" Text='<%#Eval("POH_DATE", Resources.Constants.DateFormatGrid) %>'
                                                        ToolTip='<%#Eval("POH_DATE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:CurrencyH %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCurrSplit" runat="server" Text='<%#Eval("POH_CURRENCY_TEXT") %>'
                                                        ToolTip='<%#Eval("POH_CURRENCY_TEXT") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:AmountPO %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmountSplit" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("POH_TOTAL_VALUE")))%>'
                                                        ToolTip='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("POH_TOTAL_VALUE")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:TaxPO %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxSplit" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("POD_TAX")))%>'
                                                        ToolTip='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("POD_TAX")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:DiscountPO %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDiscountSplit" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("POD_DISC_AMT")))%>'
                                                        ToolTip='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("POD_DISC_AMT")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:InvdAmt %>" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInvdAmtSplit" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Paid %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaidSplit" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("POH_AMT_PAID")))%>'
                                                        ToolTip='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("POH_AMT_PAID")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Balance %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBalanceSplit" CssClass="BalancetoPay" Text='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("POH_BAL_TO_PAY_AMT")))%>'
                                                        ToolTip='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("POH_BAL_TO_PAY_AMT")))%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalBalFooterSplit"></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfTotalBalFooterSplit" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%--Pay Now--%>
                                            <asp:TemplateField HeaderText="<%$ resources:PayNow %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPayNowSplit" runat="server" CssClass="Uiinput-amount numeric"
                                                        MaxLength="16" onkeyup="CalculateTotalSplit(this);">
                                                    </asp:TextBox>
                                                    <asp:HiddenField ID="hdfPayNowSplit" runat="server" />
                                                    <asp:RequiredFieldValidator ID="vrfPayNowSplit" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="payemntsplit" EnableClientScript="true" runat="server" ControlToValidate="txtPayNowSplit"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PaymentAmount1 %>">
                                                    </asp:RequiredFieldValidator>
                                                    <cc1:AmountValidation ID="vrePayNowSplit" runat="server" ControlToValidate="txtPayNowSplit"
                                                        ErrorMessage="<%$ resources:Err_PaymentAmount %>" NumberDigits="12" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="payemntsplit"></cc1:AmountValidation>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalPayNowFooterSplit"></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfTotalPayNowFooterSplit" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:OtherCharges %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOtherChargesSplit" runat="server" onkeyup="CalculateTotalSplit(this);">
                                                    </asp:Label>
                                                    <asp:HiddenField ID="hdfOtherChargesSplit" runat="server" />
                                                    <asp:HiddenField ID="hdfTaxSplit" Value="0.0" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalOtherChargesFooterSplit"></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfTotalOtherChargesFooterSplit" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Tax %>" ItemStyle-HorizontalAlign="Right"
                                                Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalTaxSplit" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="4%" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalTaxFooterSplit"></asp:Label></FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Tax %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTaxSplit" runat="server" CssClass="Uiinput-amount numeric" MaxLength="16"
                                                        onkeyup="CalculateTotalFooterTaxSplit(this);">
                                                    </asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfTaxSplit" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="payemntsplit" EnableClientScript="true" runat="server" ControlToValidate="txtTaxSplit"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TaxAmount %>">
                                                    </asp:RequiredFieldValidator>
                                                    <cc1:AmountValidation ID="vreTaxSplit" runat="server" ControlToValidate="txtTaxSplit"
                                                        ErrorMessage="<%$ resources:Err_InvalidTax %>" NumberDigits="12" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="payemntsplit"></cc1:AmountValidation>
                                                </ItemTemplate>
                                                <ItemStyle Width="4%" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalTaxFooterSplit1"></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfTotalTaxFooterSplit1" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:TableCell>
                        </div>
                    </div>
                </div>
                <%-- End--%>
                <%--AmtAdj Popup Start--%>
                <div id="divSoSplitUpAdjn" style="display: none">
                    <div class="content-wrapper">
                        <asp:Panel runat="server" ID="Allocation_SectionAdjn" CssClass="Button-container-popup">
                            <asp:Button ID="btnSaveSplitAdjn" runat="server" Text="<%$ resources:Controls,Apply %>"
                                ToolTip="<%$ resources:Controls,Apply %>" OnClick="ActionHandler" CommandName="ADJNSPLITSAVE"
                                SkinID="btnInner-add-dsd" CommandArgument="Allocation_SectionAdjn" ValidationGroup="SplitAdjn"
                                OnClientClick="javascript:ValidatePageNow('SplitAdjn')" />
                        </asp:Panel>
                        <div class="detail-co3">
                            <div class="divcol2-S">
                                <asp:Label ID="lblcustomer" runat="server" Text="<%$resources:Vendor1 %>" AssociatedControlID="lblVndname"
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblVndname" runat="server"></asp:Label>
                                <div class="clear">
                                </div>
                                <asp:Label ID="lblCurr" runat="server" Text="<%$resources:Currency1 %>" AssociatedControlID="lblcurrencyname"
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblcurrencyname" runat="server" CssClass="large"></asp:Label>
                            </div>
                            <div class="clear">
                            </div>
                        </div>
                        <div class="error" id="divErrorLabelAdjn" runat="server" visible="false">
                            <ul>
                                <li>
                                    <asp:Literal runat="server" ID="lblSplitErrorMessageAdjn" Text="<%$resources:error_allocationAdjn %>"></asp:Literal>
                                </li>
                            </ul>
                        </div>
                        <div class="error" id="divBaltoAll" runat="server" visible="false">
                            <ul>
                                <li>
                                    <asp:Literal runat="server" ID="Literal2" Text="<%$resources:error_allocationAdjnBalAmt %>"></asp:Literal>
                                </li>
                            </ul>
                        </div>
                        <div class="error" id="divErrorAdj" runat="server" visible="false">
                            <ul>
                                <li>
                                    <asp:Literal runat="server" ID="Literal1" Text="<%$resources:adj_Allocated %>"></asp:Literal>
                                </li>
                            </ul>
                        </div>
                        <div class="gridwrap">
                            <asp:TableCell>
                                <div class="gridwrap">
                                    <asp:GridView ID="grdPaymentSplitAdjn" runat="server" AutoGenerateColumns="False"
                                        Width="100%" PageSize="<%$ resources:PageSize %>" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable"
                                        AllowSorting="false" ShowFooter="true" OnRowDataBound="ActionHandler">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$resources:No %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrDrNOAdjn" runat="server" Text='<%#Eval("PAA_NO") %>'></asp:Label>
                                                    <asp:HiddenField ID="hdfReceiptTRXAdjnPK" runat="server" />
                                                    <asp:HiddenField ID="hdfAdjnPK" runat="server" />
                                                    <asp:HiddenField ID="hdfCrDrPK" runat="server" Value='<%#Eval("PAA_CRDRPK") %>' />
                                                    <asp:HiddenField ID="hdfReceiptAdjnPK" runat="server" Value='<%#Eval("PAA_TRXPK") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:PageType %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPageType" runat="server" Text='<%#Eval("PAA_TYPE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:Date %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDate" runat="server" Text='<%#Eval("PAA_DATE", Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:Amount %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotAmountAdjn" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("PAA_AMOUNT")))%>'
                                                        ToolTip='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("PAA_AMOUNT")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:Alloc %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAllocatedAdjn" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("PAA_AMOUNT_PAID")))%>'
                                                        ToolTip='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("PAA_AMOUNT_PAID")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:Balance %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBalanceAdjn" CssClass="BalancetoPay" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("PAA_AMOUNT_BAL")))%>'
                                                        ToolTip='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("PAA_AMOUNT_BAL")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterTemplate>
                                                    <asp:HiddenField ID="hdfBalanceAdjn" runat="server" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%--Pay Now--%>
                                            <asp:TemplateField HeaderText="<%$ resources:AllocateNow %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAllocateAdjn" runat="server" CssClass="Uiinput-amount numeric"
                                                        Text='<%# GetFormattedCurrencyWithSeperation(Convert.ToDouble(Eval("PAA_AMOUNT_PAID")))%>'
                                                        MaxLength="15" onkeyup="CalculateTotalAdjn(this);"> >
                                                    </asp:TextBox>
                                                    <asp:HiddenField ID="hdfPayNowSplit" runat="server" />
                                                    <div class="starwrap">
                                                        <cc1:AmountValidation ID="vreDedAllocateNowSplit" runat="server" ControlToValidate="txtAllocateAdjn"
                                                            ErrorMessage="<%$ resources:Err_InvalidAllocation %>" NumberDigits="11" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="SplitAdjn"></cc1:AmountValidation>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalAllocateAdjn"></asp:Label>
                                                    <asp:HiddenField ID="hdfTotalAllocateAdjn" runat="server" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:TableCell>
                        </div>
                    </div>
                </div>
                <%--AmtAdj Popup End--%>
                <%------------- Payment CR/DR Allocation Popup Start ---------------------%>
                <div id="divCrdrAllocation" style="display: none">
                    <div class="content-wrapper">
                        <asp:Panel runat="server" ID="pnlCrdrAllocation" CssClass="Button-container-popup">
                            <asp:Button ID="btnCrdrAllocationApply" runat="server" Text="<%$ resources:Controls,Apply %>"
                                OnClick="ActionHandler" CommandName="CRDRALLOCATIONSAVE" SkinID="btnInner-add-dsd"
                                CommandArgument="Allocation_Section" ValidationGroup="crdrAllocation" OnClientClick="javascript:ValidatePageNow('crdrAllocation')" />
                        </asp:Panel>
                        <div class="detail-co3">
                            <div class="div3col-S">
                                <asp:Label ID="Label9" runat="server" Text="<%$ resources:InvoiceNo1 %>" AssociatedControlID="lblInvSplitNo_CrdrAlcn"></asp:Label>
                                <asp:Label ID="lblInvSplitNo_CrdrAlcn" runat="server" CssClass="medium"></asp:Label>
                                <asp:Label ID="Label12" runat="server" Text="<%$ resources:Amount1 %>" AssociatedControlID="lblInvSplitAmount_CrdrAlcn"></asp:Label>
                                <asp:Label ID="lblInvSplitAmount_CrdrAlcn" runat="server" CssClass="medium"></asp:Label>
                            </div>
                            <div class="div3col-S">
                                <asp:Label ID="Label14" runat="server" Text="<%$ resources:Date1 %>" AssociatedControlID="lblInvSplitDate_CrdrAlcn"></asp:Label>
                                <asp:Label ID="lblInvSplitDate_CrdrAlcn" runat="server" CssClass="medium"></asp:Label>
                                <asp:Label ID="Label16" runat="server" Text="Paid:" AssociatedControlID="lblInvSplitReceived_CrdrAlcn"></asp:Label>
                                <asp:Label ID="lblInvSplitReceived_CrdrAlcn" runat="server" CssClass="medium"></asp:Label>
                            </div>
                            <div class="div3col-S">
                                <asp:Label ID="Label18" runat="server" Text="<%$ resources:Supplier1 %>" AssociatedControlID="lblInvSplitSupplier_CrdrAlcn"></asp:Label>
                                <asp:Label ID="lblInvSplitSupplier_CrdrAlcn" runat="server" CssClass="medium"></asp:Label>
                                <asp:Label ID="Label20" runat="server" Text="<%$ resources:PayNow1 %>" AssociatedControlID="lblInvSplitReceiveNow_CrdrAlcn"></asp:Label>
                                <asp:Label ID="lblInvSplitReceiveNow_CrdrAlcn" runat="server" CssClass="medium"></asp:Label>
                            </div>
                            <div class="clear">
                            </div>
                        </div>
                        <div class="error" id="divCrdrErrorMsg" runat="server" visible="false">
                            <ul>
                                <li>
                                    <asp:Literal runat="server" ID="lblSplitErrorMessage_crdrAllocation" Text="<%$ resources:error_allocation %>"></asp:Literal>
                                </li>
                            </ul>
                        </div>
                        <div class="gridwrap">
                            <asp:TableCell>
                                <div class="gridwrap">
                                    <asp:GridView ID="grdCrdrAllocation" runat="server" AutoGenerateColumns="False" Width="100%"
                                        PageSize="<%$ resources:PageSize %>" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable"
                                        AllowSorting="false" ShowFooter="true" OnRowDataBound="ActionHandler">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:CrDrNO %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrdrNo" runat="server" CssClass="medium" Text='<%#Eval("PNM_CRDR_NO") %>'
                                                        ToolTip='<%#Eval("PNM_CRDR_NO") %>'></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfCrdrMpgPk" Value='<%#Eval("PNM_CRDR_MPG") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfCRDRHdr" Value='<%#Eval("PNM_CRDR_HDR") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:CrDrDate %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrdrDate" runat="server" Text='<%# Eval("PNM_CRDR_DATE", Resources.Constants.DateFormatGrid)%>'
                                                        ToolTip='<%# Eval("PNM_CRDR_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:CrdrCurrency %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrdrCurrency" runat="server" Text='<%#Eval("PNM_CRDR_CURRENCY_TEXT") %>'
                                                        ToolTip='<%#Eval("PNM_CRDR_CURRENCY_TEXT") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:CrdrAmount %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrdrAmount" runat="server" Text='<%#Eval("PNM_CRDR_AMOUNT","{0:c}") %>'
                                                        ToolTip='<%#Eval("PNM_CRDR_AMOUNT","{0:c}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:CrdrPaidAmount %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrdrPaidAmount" runat="server" Text='<%#Eval("PNM_ALLOCATED_AMOUNT","{0:c}") %>'
                                                        ToolTip='<%#Eval("PNM_ALLOCATED_AMOUNT","{0:c}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:CrdrBalanceAmount %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrdrBalanceAmount" runat="server" Text='<%#Eval("PNM_BALANCE_AMOUNT","{0:c}") %>'
                                                        ToolTip='<%#Eval("PNM_BALANCE_AMOUNT","{0:c}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:CrdrPayNow %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCrdrPayNow" runat="server" CssClass="Uiinput-amount numeric"
                                                        onkeyup="CalculateTotalCreditSplit(this);" MaxLength="16" Text='<%# GetFormattedCurrency(Eval("PNM_PAID_AMOUNT")) %>'>
                                                    </asp:TextBox>
                                                    <cc1:AmountValidation ID="vamCrdrPayNow" runat="server" ControlToValidate="txtCrdrPayNow"
                                                        ErrorMessage="<%$ resources:Err_PaymentAmount %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="crdrAllocation"></cc1:AmountValidation>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" Wrap="false" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblCrdrPayNowFooter"></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfCrdrPayNowFooter" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:CrdrAdjAmount %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCrdrAdjAmount" runat="server" CssClass="Uiinput-amount numeric"
                                                        onkeyup="CalculateTotalCreditSplit(this);" MaxLength="16" Text='<%# GetFormattedCurrency(Eval("PNM_ADJ_AMOUNT")) %>'>
                                                    </asp:TextBox>
                                                    <cc1:AmountValidation ID="vamCrdrAdjAmount" runat="server" ControlToValidate="txtCrdrAdjAmount"
                                                        ErrorMessage="<%$ resources:Err_CrdrAdjAmnt %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="crdrAllocation"></cc1:AmountValidation>
                                                    <asp:CustomValidator ID="vcmCrdrAdjAmount" CssClass="star" SetFocusOnError="true"
                                                        ClientValidationFunction="ValidateCreditSplit" ValidationGroup="crdrAllocation"
                                                        EnableClientScript="true" runat="server" ControlToValidate="txtCrdrPayNow" Display="Dynamic"
                                                        Text="*" ErrorMessage="<%$ resources:Err_CrdrAmntExceeds %>"></asp:CustomValidator>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" Wrap="false" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblCrdrAdjAmountFooter"></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfCrdrAdjAmountFooter" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:TableCell>
                        </div>
                    </div>
                </div>
                <%------------- Payment CR/DR Allocation Popup End ---------------------%>
                <%------------- New Invoice List Popup Start ---------------------%>
                <div id="divNewInvList" style="display: none">
                    <div class="content-wrapper">
                        <asp:Panel runat="server" ID="pnlInvList" CssClass="Button-container-popup">
                            <asp:Button ID="btnNewInvAdd" runat="server" Text="<%$ resources:Controls,Apply %>"
                                OnClick="ActionHandler" CommandName="NEWINVOICEAPPLY" SkinID="btnInner-add-dsd"
                                CommandArgument="Allocation_Section" />
                        </asp:Panel>
                        <div class="gridwrap">
                            <asp:TableCell>
                                <div class="gridwrap">
                                    <asp:GridView runat="server" ID="grdNewInvList" Width="100%" AllowSorting="True"
                                        OnSorting="ActionHandler" OnRowDataBound="ActionHandler" AutoGenerateColumns="false"
                                        EmptyDataRowStyle-CssClass="emptytable">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAllPI(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" ID="chkPIselect" TabIndex="17" />
                                                    <asp:HiddenField runat="server" ID="hdfInvoiceID" Value='<%# Eval("IVH_PK") %>' />
                                                    <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval("IVH_DEPT") %>' />
                                                    <asp:HiddenField ID="hdfPOType" runat="server" Value='<%# Eval("POH_ITEM_TYPE") %>' />
                                                    <asp:HiddenField ID="hdfTaxAmount" runat="server" Value='<%# Eval("IVH_TAX_TC") %>' />
                                                    <asp:HiddenField ID="hdfDelStatus" runat="server" Value='<%# Eval("IVH_DEL_STATUS") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="1%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:InvoiceDate %>" SortExpression="IVH_DATE">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInvoiceDate" runat="server" Text='<%#  Eval("IVH_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("IVH_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                        ToolTip='<%# Eval("IVH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="8%" Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:InvoiceNo %>" SortExpression="IVH_NO">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("IVH_NO") ==""?"[NEW]":Eval("IVH_NO")%>'
                                                        ToolTip='<%# Eval("IVH_NO")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="12%" Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:InvoiceType %>" SortExpression="IVH_TYPE_TEXT">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInvoiceType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_TYPE_TEXT"),3,"")%>'
                                                        ToolTip='<%# Eval("IVH_TYPE_TEXT")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="6%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Vendor %>" SortExpression="IVH_VENDOR_TEXT">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVendor" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Convert.ToString(Eval("IVH_VENDOR_TEXT")) + " - " + Convert.ToString(Eval("IVH_VENDOR_CODE")),42) %>'
                                                        ToolTip='<%# Convert.ToString(Eval("IVH_VENDOR_TEXT")) + " - " + Convert.ToString(Eval("IVH_VENDOR_CODE")) %>'></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfVendorPK" Value='<%# Eval("IVH_VND_PK") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="9%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:SupplierInvNO %>" SortExpression="IVH_VENDOR_INV_NO"
                                                Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSupplierInvNO" runat="server" Text='<%# Eval("IVH_VENDOR_INV_NO") %>'
                                                        ToolTip='<%# Eval("IVH_VENDOR_INV_NO") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:PONO %>" SortExpression="IVH_PO_NO">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSoNo" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_PO_NO"),12)%>'
                                                        ToolTip='<%# Eval("IVH_PO_NO")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="18%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:PODate %>" SortExpression="IVH_POH_DT">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSoDate" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_POH_DT", Resources.Constants.DateFormatGrid),12)%>'
                                                        ToolTip='<%# Eval("IVH_POH_DT", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" Wrap="false" />
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
                                                    <asp:Label ID="lblInvoiceValue" runat="server" Text='<%# Eval("IVH_AMOUNT", "{0:c}") %>'
                                                        ToolTip='<%# Eval("IVH_AMOUNT", "{0:c}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:BalAmt %>" SortExpression="IVH_BAL_AMNT_TC">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBalAmt" runat="server" Text='<%# Eval("IVH_BAL_AMNT_TC", "{0:c}") %>'
                                                        ToolTip='<%# Eval("IVH_BAL_AMNT_TC", "{0:c}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="12%" CssClass="amount-numeric" />
                                                <HeaderStyle CssClass="amount-numeric" Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                </ItemTemplate>
                                                <ItemStyle Width="1%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:DueDate %>" SortExpression="IVH_DUE_DT">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDueDate" runat="server" Text='<%# Eval("IVH_DUE_DT", Resources.Constants.DateFormatGrid) %>'
                                                        ToolTip='<%# Eval("IVH_DUE_DT", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="9%" Wrap="false" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:TableCell>
                        </div>
                    </div>
                </div>
                <%------------- New Invoice List Popup End ---------------------%>
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
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="payment" runat="server" />
                    <asp:ValidationSummary ID="vsSplit" ValidationGroup="split" runat="server" />
                    <asp:ValidationSummary ID="vsPaymentSplit" ValidationGroup="payemntsplit" runat="server" />
                    <asp:ValidationSummary ID="vsWht" ValidationGroup="wht" runat="server" />
                    <asp:ValidationSummary ID="vsUpload" ValidationGroup="upload" runat="server" />
                    <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="SplitAdjn" runat="server" />
                    <asp:ValidationSummary ID="vsPaymentModes" ValidationGroup="paymentDet" runat="server" />
                    <asp:ValidationSummary ID="vscrdrAllocation" ValidationGroup="crdrAllocation" runat="server" />
                </div>
                <div id="diverrorvatbuy" style="display: none">
                    <asp:ValidationSummary ID="vsVatbuy" ValidationGroup="vatbuy" runat="server" />
                </div>
            </div>
            <div id="divScriptButtons">
                <asp:Button runat="server" ID="btnJournalize_Action" CommandName="JOURNALIZE" OnClick="ActionHandler"
                    EnableTheming="false" Style="display: none" />
                <asp:Button ID="btnJournalizeUpdate" runat="server" OnClick="ActionHandler" CommandName="JOURNALIZEUPDATE"
                    EnableTheming="false" Style="display: none" />
            </div>
            <div id="divJournalize" style="display: none">
                <uc1:Journalize ID="ucrJournalize" runat="server" />
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server">
                </uc1:WorkflowUserComments>
            </div>
            <asp:HiddenField ID="hdfOtherPer" Value="0" runat="server" />
            <asp:HiddenField ID="hdfTaxPer" Value="0" runat="server" />
            <asp:HiddenField ID="hdfJournalizeWorkFlow" Value="0" runat="server" />
            <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
            <asp:HiddenField ID="hdfBaseCurrency" runat="server" />
            <asp:HiddenField ID="hdfPaymentMode" runat="server" />
            <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
            <asp:HiddenField ID="hdfCurrencyGroup2" Value="2" runat="server" />
            <asp:HiddenField ID="hdfIscontYes" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
            <asp:HiddenField ID="hdfTotalOtherCharges" runat="server" />
            <asp:HiddenField ID="hdfInvTotalOtherCharge" runat="server" />
            <asp:HiddenField ID="hdfAmntMissmatch" runat="server" />
            <asp:HiddenField ID="hdfBaltoAlloc" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIscontYesVat" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIscontYesWht" runat="server" />
            <asp:HiddenField ID="hdfIsBtnUpload" runat="server" Value="0" />
            <asp:HiddenField ID="hdftest" runat="server" Value="0" />
            <asp:HiddenField ID="hdfTax" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsTaxForOtherCharge" runat="server" Value="0" />
            <asp:HiddenField ID="hdfExchangeRateFormat" runat="server" />
            <asp:HiddenField ID="HiddenField1" Value="3" runat="server" />
            <asp:HiddenField ID="HiddenField2" Value="2" runat="server" />
            <asp:HiddenField ID="hdfIsPaymentModeAdded" Value="0" runat="server" />
            <asp:HiddenField ID="hdfHdrExchangeRate" Value="0" runat="server" />
            <asp:HiddenField ID="hdfPaymentModeRowIndex" Value="-1" runat="server" />
            <asp:HiddenField ID="hdfPaymentBankName" Value="" runat="server" />
            <asp:HiddenField ID="hdfIsEdited" Value="0" runat="server" />
            <asp:HiddenField ID="hdfInvOtherCharge" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIsCancelled" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIsMultipleCheque" Value="0" runat="server" />
            <asp:HiddenField ID="hdfAppType" runat="server" />
            <asp:HiddenField ID="hdfAppSubType" runat="server" />
            <asp:HiddenField ID="hdfNotTalliedInvoicePk" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCurrencyFormatWithSeperation" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
