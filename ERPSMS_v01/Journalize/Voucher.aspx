<%@ Page Title="<%$ Resources:Captions,Title_DirectPayment %>" Language="C#" Theme="ClassicExt"
    EnableEventValidation="false" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="Voucher.aspx.cs" ValidateRequest="false" Inherits="ERPSMS_v01.Journalize.Voucher" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/Journalize/UserControls/JournalizeControlNew.ascx" TagName="Journalize"
    TagPrefix="uc1" %>
<%@ Register Src="~/Journalize/UserControls/TransactionImport.ascx" TagName="TransactionImport"
    TagPrefix="uc1" %>
<%@ Register Src="~/Journalize/UserControls/AuditLogList.ascx" TagName="AuditLogList"
    TagPrefix="uc1" %>



<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%= (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString()) %>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        //var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtVoucherDate");
            GrandScriptUtils.DatePickerCommon("txtVatTaxInvDate");
            GrandScriptUtils.DatePickerCommon("txtRefDate");
            GrandScriptUtils.DatePickerCommon("txtInstrDate");
            GrandScriptUtils.DatePickerCommon("txtInstrDate1");

            GrandScriptUtils.MakeAutoCompleteDDL("txtCurrency", url, "hdfCurrency", true, true, "CURRENCY");
            //GrandScriptUtils.MakeAutoCompleteDDL("txtAccount", url + "?VoucherType=" + $("[id$='hdfVoucherType']").val() + "&AccType=1", "hdfAccount", true, true, "ACCOUNT");
            //GrandScriptUtils.MakeAutoCompleteDDL("txtAccount1", url + "?VoucherType=" + $("[id$='hdfVoucherType']").val() + "&AccType=2", "hdfAccount1", true, true, "ACCOUNT");
            GrandScriptUtils.MakeAutoCompleteDDL("txtAccount", url + "&AccType=" + $("[id$='hdfAccountType']").val(), "hdfAccount", true, true, "JOURNALACCOUNT");
            GrandScriptUtils.MakeAutoCompleteDDL("txtAccount1", url + "&AccType=" + $("[id$='hdfAccountType']").val(), "hdfAccount1", true, true, "JOURNALACCOUNT");
            GrandScriptUtils.MakeAutoCompleteDDL("txtWHTAccountPopup", url, "hdfWHTAccountPopup", true, true, "WHTACCOUNTS");
            GrandScriptUtils.MakeAutoCompleteDDL("txtTo", url + "&IsSBUVendor=" + $("[id$='hdfIsSBUVendor']").val(), "hdfVendor", true, true, "VENDOR", false, false, false, true);
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendorPopup", url, "hdfVendorPopup", true, false, "VENDOR");
            //GrandScriptUtils.MakeAutoCompleteDDL("txtVATAccountPopup", url, "hdfVATAccountPopup", true, true, "VATBUYACCOUNTS");
            GrandScriptUtils.MakeAutoCompleteDDL("txtAddressType", url + "&VendorPk=" + $("[id$='hdfVendorPopup']").val(), "hdfAddressType", true, false, "VENDORCONTACTS");
            GrandScriptUtils.MakeAutoCompleteDDL("txtWthAddressType", url + "&VendorPk=" + $("[id$='hdfVendor']").val(), "hdfWthAddressType", true, true, "VENDORCONTACTS");

            $("[id$='txtAmount']").ForceNumericOnly();
            $("[id$='txtDebit1']").ForceNumericOnly();
            $("[id$='txtCredit1']").ForceNumericOnly();

            HideAllValidations();
            $("[id$='txtCurrency']").attr("disabled", "disabled");
            $("[id$='txtCurrency']").next().hide();

            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();

            if ($('[id$=btnApply]').is(":visible"))
                $('[id$=btnWhtTaxSave]').hide();
            if ($('[id$=btnVatTaxApply]').is(":visible"))
                $('[id$=btnVatTaxSave]').hide();

            if ($("[id$='hdfVoucherType']").val() != '' && $("[id$='hdfVoucherType']").val() != 'DPVJ' && $("[id$='hdfVoucherType']").val() != 'PCVJ' && $("[id$='hdfVoucherType']").val() != 'PCRVJ') {
                $('[id$=trTo]').hide();
            }

            if ($('[id$=btnReturnDetail]').is(":visible")) {
                if ($('[id$=hdfShowChequeReturn]').val() == 0) {
                    $('[id$=btnReturnDetail]').hide();
                }
            }

            if ($('[id$=btnReverseDetail]').is(":visible")) {
                if ($('[id$=hdfShowPdc]').val() == 0) {
                    $('[id$=btnReverseDetail]').hide();
                }
            }

            if ($("[id$='ddlMode1']").val() == '202') {
                $("[id$='chkPDC1']").show();
                $("[id$='lblPDC1']").show();
            }
            else {
                $("[id$='chkPDC1']").hide();
                $("[id$='lblPDC1']").hide();
                $("[id$='chkPDC1']").attr("checked", false);
            }
            if ($('[id$=hdfShowCashAccountDiv]').val() == 0) {
                $('#divCashAcount').hide();
            }
            else {
                $('#divCashAcount').show();
            }

        }
        var currencyDecimal = 2;
        function pageLoad() {
            ShowInstrDetailsWithoutClear();

            if (!isNaN(parseInt($("[id$=hdfDecimalDigits]").val()))) {
                currencyDecimal = parseInt($("[id$=hdfDecimalDigits]").val());
            }
        }
        function PrintCheque() {
            var printURL = $('[id$=hdfPrintCheque]').val().split(',');
            for (var i = 0; i < printURL.length; i++)
                OpenPDF(printURL[i]);
        }
        function HideAllValidations() {
            $("[id$='lblValidAccount']").hide();
            $("[id$='lblValidMode']").hide();
            $("[id$='lblValidAmount']").hide();
            $("[id$='lblValidInstrNo']").hide();
            $("[id$='lblValidInstrDate']").hide();
            $("[id$='lblValidFavourOf']").hide();

            $("[id$='lblValidAccount1']").hide();
            $("[id$='lblValidMode1']").hide();
            $("[id$='lblValidDebit1']").hide();
            $("[id$='lblValidCredit1']").hide();
            $("[id$='lblValidInstrNo1']").hide();
            $("[id$='lblValidInstrDate1']").hide();
            $("[id$='lblValidFavourOf1']").hide();

            $("[id$='vrfWHTAccountPopup']").hide();
            $("[id$='vrfFormno']").hide();


        }

        function ShowInstrDetails() {
            if ($("[id$='ddlMode']").val() == "201" || $("[id$='ddlMode']").val() == "-1") {
                $("[id$='txtInstrNo']").val('');
                $("[id$='txtInstrDate']").val('');
                $("[id$='txtFavourOf']").val('');
                //                $("[id$='chkPDC']").show();
                //                $("[id$='lblPDC']").show();

                $("[id$='trInstrDet']").hide();
            }
            else {
                $("[id$='trInstrDet']").show();
            }
        }

        function ShowInstrDetailsWithoutClear() {
            if ($("[id$='ddlMode1']").val() == "201" || $("[id$='ddlMode1']").val() == "-1") {
                //                $("[id$='txtInstrNo1']").val('');
                //                $("[id$='txtInstrDate1']").val('');
                //                $("[id$='txtFavourOf1']").val('');
                $("[id$='trInstrDet1']").hide();
            }
            else {
                $("[id$='trInstrDet1']").show();
            }
        }
        function ShowInstrDetails1() {
            if ($("[id$='ddlMode1']").val() == "201" || $("[id$='ddlMode1']").val() == "-1") {
                $("[id$='txtInstrNo1']").val('');
                $("[id$='txtInstrDate1']").val('');
                $("[id$='txtFavourOf1']").val('');
                $("[id$='trInstrDet1']").hide();
                $("[id$='chkPDC1']").show();
                $("[id$='lblPDC1']").show();
            }
            else {
                $("[id$='trInstrDet1']").show();
            }

            if ($("[id$='ddlMode1']").val() == '202') {
                $("[id$='chkPDC1']").show();
                $("[id$='lblPDC1']").show();
                if ($("[id$=hdfItemEdit]").val() == "0") {
                    if ($('[id$=txtInstrDate1]').val() != "" && $('[id$=txtVoucherDate]').val() != "") {
                        var ChequeDate = parseDateFormat($('[id$=txtInstrDate1]').val());
                        var VoucherDate = parseDateFormat($('[id$=txtVoucherDate]').val());
                        if (ChequeDate > VoucherDate) {
                            $("[id$='chkPDC1']").attr("checked", true);
                        }
                        else {
                            $("[id$='chkPDC1']").attr("checked", false);
                        }
                    }
                    else {
                        $("[id$='chkPDC1']").attr("checked", false);
                    }
                }
            }
            else {
                $("[id$='chkPDC1']").hide();
                $("[id$='lblPDC1']").hide();
                $("[id$='chkPDC1']").attr("checked", false);
            }

            var ppcReconStatus = '<%= GetGlobalResourceObject("ConfigurationsRes","PPCReconciliation").ToString() %>';
            if (ppcReconStatus == 1) {
                if ($("[id$='ddlMode1']").val() == '202') {
                    $("[id$='chkPDC1']").attr("checked", true);
                    $("[id$='chkPDC1']").attr("disabled", true);
                }
            }
        }

        function parseDateFormat(s) {
            return new Date(s.replace(/^(\d+)\W+(\w+)\W+/, '$2 $1 '));
        }

        function AfterDateSelect(controlID) {
            //            if (controlID == "txtInstrDate1") {
            //                if ($('[id$=txtInstrDate1]').val() != "") {
            //                    var ChequeDate = parseDateFormat($('[id$=txtInstrDate1]').val());
            //                    var ppcReconStatus = '<%= GetGlobalResourceObject("ConfigurationsRes","PPCReconciliation").ToString() %>';
            //                    var Todt = new Date();
            //                    //var Todate = parseDateFormat(Todt.getDate() + "-" + Todt.getMonth() + "-" + Todt.getFullYear());
            //                    if (ppcReconStatus == 1) {                       
            //                        if ($("[id$='ddlMode1']").val() == '202') {
            //                            $("[id$='chkPDC1']").attr("checked", true);
            //                            $("[id$='chkPDC1']").attr("disabled", true);
            //                        }
            //                        else {
            //                            $("[id$='chkPDC1']").attr("checked", false);
            //                        }
            //                    }
            //                    else {
            //                        if ($("[id$='ddlMode1']").val() == '202') {
            //                            if (Todt <= ChequeDate) {
            //                                $("[id$='chkPDC1']").attr("checked", true);
            //                            }
            //                            else {
            //                                $("[id$='chkPDC1']").attr("checked", false);
            //                            }
            //                        }
            //                        else {
            //                            $("[id$='chkPDC1']").attr("checked", false);
            //                        }
            //                    }
            //                }
            //            }

            if (controlID == "txtInstrDate1" || controlID == "txtVoucherDate") {
                if ($('[id$=txtInstrDate1]').val() != "" && $('[id$=txtVoucherDate]').val() != "") {
                    var ChequeDate = parseDateFormat($('[id$=txtInstrDate1]').val());
                    var VoucherDate = parseDateFormat($('[id$=txtVoucherDate]').val());
                    var ppcReconStatus = '<%= GetGlobalResourceObject("ConfigurationsRes","PPCReconciliation").ToString() %>';
                    //var Todt = new Date();
                    //var Todate = parseDateFormat(Todt.getDate() + "-" + Todt.getMonth() + "-" + Todt.getFullYear());
                    if (ppcReconStatus == 1) {
                        if ($("[id$='ddlMode1']").val() == '202') {
                            $("[id$='chkPDC1']").attr("checked", true);
                            $("[id$='chkPDC1']").attr("disabled", true);
                        }
                        else {
                            $("[id$='chkPDC1']").attr("checked", false);
                        }
                    }
                    else {
                        if ($("[id$='ddlMode1']").val() == '202') {
                            if (ChequeDate > VoucherDate) {
                                $("[id$='chkPDC1']").attr("checked", true);
                            }
                            else {
                                $("[id$='chkPDC1']").attr("checked", false);
                            }
                        }
                        else {
                            $("[id$='chkPDC1']").attr("checked", false);
                        }
                    }
                }
                else {
                    $("[id$='chkPDC1']").attr("checked", false);
                }
            }
        }

        function ShowPDCforCheque() {
            var ppcReconStatus = '<%= GetGlobalResourceObject("ConfigurationsRes","PPCReconciliation").ToString() %>';
            if (ppcReconStatus == 1) {
                if ($("[id$='ddlMode1']").val() == '202') {
                    $("[id$='chkPDC1']").attr("checked", true);
                    $("[id$='chkPDC1']").attr("disabled", true);
                }
                else {
                    $("[id$='chkPDC1']").attr("checked", false);
                }
            }
        }

        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For Amount validation
                //For finding and removing duplicate and other group validation controls
                CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                if (valGroup == 'wht') {
                    $("[id$=litErrorMsg]").hide();
                    ShowErrorMessage($("#diverrorwht").html(), '<%= Resources.Messages.Information %>');
                    return false;
                }
                if (valGroup == 'vatbuy') {
                    $("[id$=litErrorMsg]").hide();
                    ShowErrorMessage($("#diverrorvatbuy").html(), '<%= Resources.Messages.Information %>');
                    return false;
                }
                else {
                    $("[id$=litErrorMsg]").hide();
                    ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
                    return false;  //Page is invalid -- stop right here
                }
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
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

        function ValidateAddItems(valGroup) {
            var isValid = true;
            var msg = "";
            HideAllValidations();

            if (valGroup == "AddItem") {
                if ($("[id$='hdfAccount']").val() == '' || $("[id$='hdfAccount']").val() == '-1') {
                    $("[id$='lblValidAccount']").show();
                    isValid = false;
                    msg += '<ul><li><%= GetLocalResourceObject("Err_Account") %></ul></li>';
                }

                if ($("[id$='ddlMode']").val() == "-1" && $("[id$='hdfJournalType']").val() != 'JV') {
                    $("[id$='lblValidMode']").show();
                    isValid = false;
                    msg += '<ul><li><%= GetLocalResourceObject("Err_Mode") %></ul></li>';
                }

                if ($("[id$='txtAmount']").val() == '' || $("[id$='txtAmount']").val() == '0') {
                    $("[id$='lblValidAmount']").show();
                    isValid = false;
                    msg += '<ul><li><%= GetLocalResourceObject("Err_Amount") %></ul></li>';
                }

                if ($("[id$='ddlMode']").val() != "201" && $("[id$='ddlMode']").val() != "-1") {
                    if ($("[id$='txtInstrNo']").val() == '') {
                        $("[id$='lblValidInstrNo']").show();
                        isValid = false;
                        msg += '<ul><li><%= GetLocalResourceObject("Err_InstrNo") %></ul></li>';
                    }

                    if ($("[id$='txtInstrDate']").val() == '') {
                        $("[id$='lblValidInstrDate']").show();
                        isValid = false;
                        msg += '<ul><li><%= GetLocalResourceObject("Err_InstrDate") %></ul></li>';
                    }

                    if ($("[id$='txtFavourOf']").val() == '') {
                        $("[id$='lblValidFavourOf']").show();
                        isValid = false;
                        msg += '<ul><li><%= GetLocalResourceObject("Err_FavourOf") %></ul></li>';
                    }
                }
            }
            else {
                if ($("[id$='hdfAccount1']").val() == '' || $("[id$='hdfAccount1']").val() == '-1') {
                    $("[id$='lblValidAccount1']").show();
                    isValid = false;
                    msg += '<ul><li><%= GetLocalResourceObject("Err_Account") %></ul></li>';
                }

                if ($("[id$='ddlMode1']").val() == "-1" && $("[id$='hdfJournalType']").val() != 'JV') {
                    $("[id$='lblValidMode1']").show();
                    isValid = false;
                    msg += '<ul><li><%= GetLocalResourceObject("Err_Mode") %></ul></li>';
                }

                if (($("[id$='txtDebit1']").val() == '' || $("[id$='txtDebit1']").val() == '0')
                    && ($("[id$='txtCredit1']").val() == '' || $("[id$='txtCredit1']").val() == '0')) {
                    $("[id$='lblValidDebit1']").show();
                    $("[id$='lblValidCredit1']").show();
                    isValid = false;
                    msg += '<ul><li><%= GetLocalResourceObject("Err_Amount") %></ul></li>';
                }

                if ($("[id$='ddlMode1']").val() != "201" && $("[id$='ddlMode1']").val() != "-1") {
                    //                    if ($("[id$='txtInstrNo1']").val() == '') {
                    //                        $("[id$='lblValidInstrNo1']").show();
                    //                        isValid = false;
                    //                        msg += '<ul><li><%= GetLocalResourceObject("Err_InstrNo") %></ul></li>';
                    //                    }

                    //                    if ($("[id$='txtInstrDate1']").val() == '') {
                    //                        $("[id$='lblValidInstrDate1']").show();
                    //                        isValid = false;
                    //                        msg += '<ul><li><%= GetLocalResourceObject("Err_InstrDate") %></ul></li>';
                    //                    }

                    //                    if ($("[id$='txtFavourOf1']").val() == '') {
                    //                        $("[id$='lblValidFavourOf1']").show();
                    //                        isValid = false;
                    //                        msg += '<ul><li><%= GetLocalResourceObject("Err_FavourOf") %></ul></li>';
                    //                    }
                }
            }

            if (!isValid) {
                $("[id$=litErrorMsg]").show();
                $("[id$=litErrorMsg]").html(msg);
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
            }
            return isValid;
        }

        function SetDebitCreditAmt(val) {
            //val = 1 then Debit change else Credit change
            if (val && $("[id$='txtCredit1']").val() != '') {
                $("[id$='txtCredit1']").val('0');
            }
            else if (!val && $("[id$='txtDebit1']").val() != '') {
                $("[id$='txtDebit1']").val('0');
            }
        }

        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtCurrency") {
                $("[id$='btnCurrency']").click();
            }
            else if (targetControlID == "txtAccount1") {
                $("[id$='btnAccount1']").click();
            }
            else if (targetControlID == "txtAccount") {
                $("[id$='btnAccount']").click();
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

        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtWHTAccountPopup") {
                $("[id$=btnWHTAccountPopup]").click();
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
                    //whtTaxAmount = toFixed(whtTaxAmount, DecimalDigits + 1);
                } catch (e) {
                    whtTaxAmount = 0;
                }


            }
            whtTaxAmount = round(whtTaxAmount, DecimalDigits);
            $("#[id*=txtWHTTaxAmountPopup]").val(toFixed(whtTaxAmount, DecimalDigits));
        }

        function CalculateVatBuyTotal(sender) {
            var vatAmt = 0;
            var DecimalDigits = 2;
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
                    //vatTaxAmount = toFixed(vatTaxAmount, DecimalDigits + 1);

                } catch (e) {
                    vatTaxAmount = 0;
                }
            }
            vatTaxAmount = round(vatTaxAmount, DecimalDigits);
            $("#[id*=txtVATTaxAmountPopup]").val(toFixed(vatTaxAmount, DecimalDigits));
        }

        function HideOverlay() {
            $('#divmodel').hide();
        }

        function ShowHideSections(type) {
            if (type == 1) {
                $("[id$='hHead2']").html("<%= Resources.Controls.Accounts %>");
                $("[id$='divFirstPart']").hide();
                $("[id$='thDebit']").show();
                $("[id$='thDebit']").attr("width", "10%");
                $("[id$='tdDebit1']").show();
                $("[id$='tdDebit2']").show();
                $("[id$='thCredit']").show();
                $("[id$='thCredit']").attr("width", "10%");
                $("[id$='tdCredit1']").show();
                $("[id$='tdCredit2']").show();
            }
            if (type == 2) {
                $("[id$='hHead1']").html("<%= Resources.Controls.DebitAccounts %>");
                $("[id$='hHead2']").html("<%= Resources.Controls.CreditAccounts %>");
                $("[id$='thAmount']").html("<%= Resources.Controls.DebitAmt %>");
                $("[id$='divFirstPart']").show();
                $("[id$='thDebit']").hide();
                $("[id$='tdDebit1']").hide();
                $("[id$='tdDebit2']").hide();
                $("[id$='thCredit']").show();
                $("[id$='thCredit']").attr("width", "20%");
                $("[id$='tdCredit1']").show();
                $("[id$='tdCredit2']").show();
            }
            else if (type == 3) {
                $("[id$='hHead1']").html("<%= Resources.Controls.CreditAccounts %>");
                $("[id$='hHead2']").html("<%= Resources.Controls.DebitAccounts %>");
                $("[id$='thAmount']").html("<%= Resources.Controls.CreditAmt %>");
                $("[id$='divFirstPart']").show();
                $("[id$='thDebit']").show();
                $("[id$='thDebit']").attr("width", "20%");
                $("[id$='tdDebit1']").show();
                $("[id$='tdDebit2']").show();
                $("[id$='thCredit']").hide();
                $("[id$='tdCredit1']").hide();
                $("[id$='tdCredit2']").hide();
            }
        }

        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode
            //Mode = 3 Indicates OBV
            //mode = 4 Entry Mode
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                //$("[id$=pnlSubmit]").hide();
                $("[id$=pnlSaveSubmit]").hide();
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlReturn]").hide();
                $("[id$=btnLoad]").hide();
                $("[id$=btnVatTaxSave]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlSubmit]").hide();
                $("[id$=pnlPrint]").hide();
                $("[id$=btnVatTaxSave]").hide();
                $("[id$=pnlReturn]").hide();
                $("[id$=pnlChequePrint]").hide();
            }
            else if (mode == 3) {
                $("[id$=pnlSubmit]").hide();
                $("[id$=pnlPrint]").hide();
                $("[id$=pnlReturn]").hide();
                $("[id$=pnlChequePrint]").hide();
            }
            else if (mode == 4) {
                $("[id$=btnLoad]").hide();
                //                $("[id$=btnVatTaxApply]").hide();
                //                $("[id$=btnApply]").hide();
            }


        }

        function VatAmtMismatch() {

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            // msg = '<%= GetLocalResourceObject("Err_WHTMissmatch").ToString() %>';
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
        function WhtTaxAmtMismatch() {

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
                        $("[id$=btnWhtTaxSave]").click();
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

        function WhtAmtMismatch() {

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            // msg = '<%= GetLocalResourceObject("Err_WHTMissmatch").ToString() %>';
            msg = $("[id$=hdfAmntMissmatch]").val();
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
                        $("[id$=btnSave]").click();
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
        function WhtAmtMismatchSubmit() {

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            //            msg = '<%= GetLocalResourceObject("Err_WHTMissmatch").ToString() %>';
            msg = $("[id$=hdfAmntMissmatch]").val();
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

        function AfterClose(containerID) {
            if (containerID == "[id$=divJournalize]") {
                $("[id$=btnJournalizeUpdate]").click();
            }
            else if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
                if ($("[id$=hdfJournalizeWorkFlow]").val() == "1") {
                    ShowContainerDiv('[id$=divJournalize]', $("[id$=hdfJournalName]").val(), '1000', '550');
                    AfterCloseWkfInJournal();
                    //$("[id$=btnJournalize_Action]").click();
                }
            }
        }

        function ValidateWhtAmnt() {
            //val = 1 then Debit change else Credit change
            if (val && $("[id$='txtCredit1']").val() != '') {
                $("[id$='txtCredit1']").val('0');
            }
            else if (!val && $("[id$='txtDebit1']").val() != '') {
                $("[id$='txtDebit1']").val('0');
            }

            var whtAmt = 0;
            var whtAmtSplit = 0;
            var whtTaxAmount = 0;
            if (!isNaN(parseFloat($("[id$=hdfDecimalDigits]").val()))) {
                whtAmt = parseFloat($("[id$=hdfDecimalDigits]").val());
            }
            if (!isNaN(parseFloat($("[id$=txtPopupWHTAmount]").val()))) {
                whtAmt = parseFloat($("[id$=txtPopupWHTAmount]").val());
            }
            taxformula = taxformula.replace("#SUBTOTAL#", whtAmt);
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
            currencyDecimal = 2;
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

        function toFixed(num, precision) {
            return (+(Math.round(+(num + 'e' + precision)) + 'e' + -precision)).toFixed(precision);
        }
        function round(value, decimals) {
            return Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
        }
        function ClearVendorContacts() {
            $("[id$=hdfVendorPopup]").val("");
            $("[id$=txtAddressType]").val("");
            $("[id$=hdfAddressType]").val("");
            $("[id$=txtVatTaxId]").val("");
            $("[id$=txtBranchCode]").val("");
            $("[id$=chkHeadOffice]").attr('checked', false);

        }
        function ClearWhtVendorContacts() {
            $("[id$=hdfWhtAddressType]").val("");
            $("[id$=txtWthBranchCode]").val("");
            $("[id$=chkWthHeadOffice]").attr('checked', false);

        }

        function CalculateTotalCCAmount() {
            var TotalCCSplit = 0;
            var amtCC = 0;
            $("#[id*=grdVoucherCC] input[type=text][id*=txtVoucherCCAmountTC]").each(function (index) {
                if (!isNaN(parseFloat($(this).val()))) {
                    amtCC = parseFloat($(this).val().replace(new RegExp(',', 'g'), ''));
                    TotalCCSplit = TotalCCSplit + amtCC;
                }
            });
            $("#[id*=grdVoucherCC] [id*=lblTotalVoucherCCAmnt]").html((TotalCCSplit).toFixed(CurrencyDigits));
        }
        function FormatNumberWithComma(number) {
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

        function ShowCostCenterAllocationClear(val) {

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Msg_CCAllocationClear_Confirm").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfIsContCCAllocDeletion]").val(1);
                        $(this).dialog("close");
                        $("[id$='btnAccount1']").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfIsContCCAllocDeletion]").val(0);
                        $("[id$=txtAccount1]").val($("[id$=hdfPreviousAccount1Name]").val());
                        $("[id$=hdfAccount1]").val($("[id$=hdfPreviousAccount1Pk]").val());
                        $(this).dialog("close");
                        return false;
                    }
                },
                close: function (event) {
                    if ($("[id$=hdfIsContCCAllocDeletion]").val() == 0) {
                        $("[id$=txtAccount1]").val($("[id$=hdfPreviousAccount1Name]").val());
                        $("[id$=hdfAccount1]").val($("[id$=hdfPreviousAccount1Pk]").val());
                    }
                }
            });
            return false;
        }
        //function ChangeRowColor(row, version, pk, rowIndex) {
        //    var rows = row.parentNode.getElementsByTagName('TR');
        //    //loop over all rows and set there colors to default
        //    for (var i = 0; i < rows.length; i++) {
        //        rows[i].style.backgroundColor = 'White'; //if its your default color 
        //    }
        //    //if ($("[id$=hdfSelRow]").val() != "0") {
        //    //    row = parseInt($("[id$=hdfSelRow]").val());
        //    //}
        //    //set the current row to be with the needed color
        //    row.style.backgroundColor = "YELLOW";

        //    $("[id$=hdfSelRowVer]").val(version);
        //    $("[id$=hdfSelRowTranPk]").val(pk);
        //    $("[id$=hdfSelRow]").val(rowIndex);
        //    $("[id$='btnComparision']").click();
        //}
        function RedirectToComparisonPage() {
            window.open("LogVersionComparision.aspx?PK=" + $("[id$=hdfSelRowTranPk]").val() + "&Version=" + $("[id$=hdfSelRowVer]").val() + "&FromPosting=0");
        }
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
                        // case "month":                                                                                          
                        // //if the mode is month, then stop switching to day mode.                                                                                          
                        // if (target.month == visibleDate.getMonth()) {                                                                                          
                        // //this._switchMode("days");                                                                                          
                        // } else {                                                                                          
                        // cal._visibleDate = target.date;                                                                                          
                        // //this._switchMode("days");                                                                                          
                        // }                                                                                          
                        // cal.set_selectedDate(target.date);                                                                                          
                        // cal._switchMonth(target.date);                                                                                          
                        // cal._blur.post(true);                                                                                          
                        // cal.raiseDateSelectionChanged();                                                                                          
                        // break;                                                                                          
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
                        // case "day":                                                                                          
                        // this.set_selectedDate(target.date);                                                                                          
                        // this._switchMonth(target.date);                                                                                          
                        // this._blur.post(true);                                                                                          
                        // this.raiseDateSelectionChanged();                                                                                          
                        // break;                                                                                          
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
            //                        if (sender.get_selectedDate()) {
            //                            if (sender.get_selectedDate() && sender.get_selectedDate() && cal1.get_selectedDate() > cal2.get_selectedDate()) {
            //                                alert('The "From" Date should smaller than the "To" Date, please reselect!');
            //                                sender.show();
            //                                return;
            //                            }
            //                            //get the final date
            //                            var finalDate = new Date(sender.get_selectedDate());
            //                            var selectedMonth = finalDate.getMonth();
            //                            finalDate.setDate(1);
            //                            if (sender == cal2) {
            //                                // set the calender2's default date as the last day
            //                                finalDate.setMonth(selectedMonth + 1);
            //                                finalDate = new Date(finalDate - 1);
            //                            }
            //                            //set the date to the TextBox
            //                            sender.get_element().value = finalDate.format(sender._format);
            //                        }
        }




    </script>
</asp:Content>
<asp:Content ID="cntMain" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="aupdpnlVoucher" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <div id="divSBUCompany" class="buttoncontainer-fields floatLeft">
                                    <asp:DropDownList ID="ddlCompany" class="medium" runat="server" TabIndex="1" onmouseover="javascript:ShowTooltip('ddlCompany');">
                                    </asp:DropDownList>
                                </div>
                                <ul class="bredcrum">
                                    <asp:Label ID="lblBreadCrum" runat="server" />
                                </ul>
                                <ul id="pnlEntry" runat="server">
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="17"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li id="pnlSubmit" runat="server">
                                        <asp:Button ID="btnSubmit" runat="server" CommandName="SUBMIT" Text="<%$ resources:ErpRes,Submit %>"
                                            OnClientClick="javascript:ValidatePageNow('Voucher')" ValidationGroup="Voucher"
                                            SkinID="btnInner-submit" CommandArgument="SEC_ActionPanel" ToolTip="<%$ resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" TabIndex="29" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="23"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('Voucher')"
                                            ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li id="pnlSave" runat="server">
                                        <asp:Button ID="btnSave" runat="server" CommandName="SAVE" Text="<%$ resources:Controls,Save %>"
                                            OnClientClick="javascript:ValidatePageNow('Voucher')" ValidationGroup="Voucher"
                                            SkinID="btnInner-Save" CommandArgument="SEC_ActionPanel" ToolTip="<% $resources:Controls,Save %>"
                                            OnClick="ActionHandler" TabIndex="27" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnVocherDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="15" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" />
                                    </li>
                                    <%--  <li runat="server" id="pnlEditforCancel">
                                        <asp:Button runat="server" ID="btnEditforCancel" CommandName="EDITFORCANCEL" TabIndex="30"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>--%>
                                    <li runat="server" id="pnlPrint">
                                        <asp:Button runat="server" TabIndex="28" ID="btnPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li runat="server" id="pnlChequePrint">
                                        <asp:Button runat="server" ID="btnChequePrint" Text="<%$resources:Controls,ChqPrint %>"
                                            OnClick="ActionHandler" CommandName="CHEQUEPRINT" TabIndex="29" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,ChqPrint %>" />
                                    </li>
                                    <li id="pnlReverse">
                                        <asp:Button runat="server" TabIndex="30" ID="btnReverseDetail" CommandName="REVERSE"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Reverse %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-journalize" ToolTip="<%$resources:Controls,Reverse %>" />
                                    </li>
                                    <li id="pnlReturn">
                                        <asp:Button runat="server" TabIndex="30" ID="btnReturnDetail" CommandName="CHEQUERETURN"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Return %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-journalize" ToolTip="<%$resources:Controls,Return %>" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" CommandName="CANCEL" Text="<%$ resources:Controls,Cancel %>"
                                            SkinID="btnInner-Cancel" CommandArgument="SEC_ActionPanel" ToolTip="<%$ resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" TabIndex="31" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:HiddenField ID="hdfVoucherType" runat="server" />
                <asp:HiddenField ID="hdfVecPk" runat="server" />
                <asp:HiddenField ID="hdfDebitTotal" runat="server" />
                <asp:HiddenField ID="hdfCreditTotal" runat="server" />
                <asp:HiddenField ID="hdfVoucherNo" runat="server" />
                <asp:HiddenField ID="hdfWithHoldTax" runat="server" />
                <asp:HiddenField ID="hdfVatBuy" runat="server" />
                <asp:HiddenField ID="hdfTaxformula" runat="server" Value="0" />
                <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
                <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                <asp:HiddenField ID="hdfAmntMissmatch" runat="server" />
                <asp:Table ID="tblTemplate" runat="server" CssClass="tablelayout asptbllinks">
                    <asp:TableRow>
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblVoucherNo" runat="server" Text="<%$ resources:Controls,VoucherNo %>"
                                                AssociatedControlID="txtVoucherNo" />
                                            <asp:TextBox ID="txtVoucherNo" runat="server" MaxLength="100" Enabled="false" CssClass="medium input-disabled input-small"
                                                TabIndex="1" />
                                            <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                                            <div style="display: none">
                                                <asp:LinkButton ID="lnkAuditLog" runat="server" Text="<%$resources:Controls,AuditLog%>" ToolTip="<%$resources:Controls,AuditLog%>"
                                                    CssClass="text-underline" Visible="false" CommandName="AUDITLOG" OnClick="ActionHandler" />
                                            </div>
                                            <asp:Label ID="lblVoucherDate" runat="server" Text="<%$ resources:Controls,VoucherDate %>"
                                                CssClass="middle-lbl-a" AssociatedControlID="txtVoucherDate" />
                                            <asp:TextBox ID="txtVoucherDate" runat="server" MaxLength="200" CssClass="input-small"
                                                TabIndex="2" onkeydown="return CheckKey(event)" onpaste="return false;" />
                                            <asp:HiddenField ID="hdfVoucherDate" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfVoucherDate" runat="server" CssClass="star" SetFocusOnError="true"
                                                InitialValue="" ValidationGroup="Voucher" EnableClientScript="true" Display="Dynamic"
                                                Text="*" ControlToValidate="txtVoucherDate" ErrorMessage="<%$ resources:Err_Voucherdate %>" />
                                            <asp:RequiredFieldValidator ID="vrfYCVVoucherDate" runat="server" CssClass="star"
                                                SetFocusOnError="true" InitialValue="" ValidationGroup="ycvLoad" EnableClientScript="true"
                                                Display="Dynamic" Text="*" ControlToValidate="txtVoucherDate" ErrorMessage="<%$ resources:Err_Voucherdate %>" />
                                            <asp:Button runat="server" ID="btnLoad" CommandName="LOADFROMTEMPLATE" ValidationGroup="ycvLoad"
                                                OnClick="ActionHandler" Text="<%$resources:Controls,LoadFromTemplate %>" CommandArgument="PageAction_Entry"
                                                SkinID="btnInner-journalize" ToolTip="<%$resources:Controls,LoadFromTemplate %>"
                                                OnClientClick="javascript:ValidatePageNow('ycvLoad')" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblCurrency" runat="server" Text="<%$ resources:Controls,Currency %>"
                                                AssociatedControlID="txtCurrency" />
                                            <asp:TextBox ID="txtCurrency" runat="server" MaxLength="100" CssClass="input-small"
                                                TabIndex="5" />
                                            <asp:RequiredFieldValidator ID="vrfCurrency" runat="server" CssClass="star" SetFocusOnError="true"
                                                InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>" ValidationGroup="Voucher"
                                                EnableClientScript="true" Display="Dynamic" Text="*" ControlToValidate="txtCurrency"
                                                ErrorMessage="<%$ resources:Err_Currency %>" />
                                            <asp:Button ID="btnCurrency" runat="server" EnableTheming="false" Style="display: none"
                                                OnClick="ActionHandler" CommandName="ACTIVATE" />
                                            <asp:HiddenField ID="hdfCurrency" runat="server" />
                                            <asp:Label ID="lblExchangeRate" runat="server" Text="<%$ resources:Controls,ExchangeRate %>"
                                                CssClass="middle-lbl-a" AssociatedControlID="txtExchangeRate" />
                                            <asp:TextBox ID="txtExchangeRate" runat="server" Enabled="false" CssClass="medium input-disabled input-small"
                                                TabIndex="6" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblRefNo" runat="server" Text="<%$ resources:Controls,RefNo %>" AssociatedControlID="txtRefNo" />
                                            <asp:TextBox ID="txtRefNo" runat="server" CssClass="input-small" TabIndex="3" MaxLength="100"
                                                onkeydown="limitText(this,100);" onkeyup="limitText(this,100);" />
                                            <%--<asp:RequiredFieldValidator ID="vrfRefNo" runat="server" CssClass="star" SetFocusOnError="true"
                                                InitialValue="" ValidationGroup="Voucher" EnableClientScript="true" Display="Dynamic"
                                                Text="*" ControlToValidate="txtRefNo" ErrorMessage="<%$ resources:Err_RefNo %>" />--%>
                                            <asp:Label ID="lblRefDate" runat="server" Text="<%$ resources:Controls,refDate %>"
                                                CssClass="middle-lbl-small-d" AssociatedControlID="txtRefDate" />
                                            <asp:TextBox ID="txtRefDate" runat="server" MaxLength="100" CssClass="input-small"
                                                TabIndex="4" onkeydown="return CheckKey(event)" onpaste="return false;" />
                                            <asp:RequiredFieldValidator ID="vrfRefDate" runat="server" CssClass="star" SetFocusOnError="true"
                                                InitialValue="" ValidationGroup="Voucher" EnableClientScript="true" Display="Dynamic"
                                                Text="*" ControlToValidate="txtRefDate" ErrorMessage="<%$ resources:Err_Refdate %>" />
                                            <div id="divCashAcount">
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblCashAccount" runat="server" Text="<%$ resources:CashAccount %>"
                                                    AssociatedControlID="ddlCashAccount" />
                                                <asp:DropDownList ID="ddlCashAccount" runat="server" AutoPostBack="true" CssClass="medium"
                                                    OnSelectedIndexChanged="ActionHandler">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCashBalance" runat="server" Visible="false" class="middle-lbl-small-d"
                                                    Text="<%$ resources:Balance %>" AssociatedControlID="lblBalance" />
                                                <asp:Label ID="lblBalance" runat="server" Visible="false" class="medium input-disabled input-small"
                                                    Text=""></asp:Label>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trTo">
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblTo" runat="server" Text="<%$ resources:Controls,To %>" AssociatedControlID="txtTo" />
                                            <asp:TextBox ID="txtTo" runat="server" MaxLength="100" TabIndex="7"> </asp:TextBox>
                                            <%--<asp:TextBox ID="txtTo" runat="server" TextMode="MultiLine" MaxLength="500" TabIndex="7"
                                                CssClass="multiline-1col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" />--%>
                                            <asp:HiddenField ID="hdfVendor" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblRemarks" runat="server" Text="<%$ resources:Controls,Remarks %>"
                                                AssociatedControlID="txtRemarks" />
                                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" MaxLength="500"
                                                TabIndex="8" CssClass="multiline-3line" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div id="divDIRPaymentImport" class="div2col-S" runat="server">

                                            <uc1:TransactionImport ID="ucrTransactionImport" runat="server" />

                                            <%-- <asp:Label ID="lblSourceFile" runat="server" Text="<%$ resources: SourceFile %>"
                                                AssociatedControlID="fupImport" /> 
                                            <asp:FileUpload ID="fupImport" runat="server" TabIndex="7" />
                                            <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupImport"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_File_Upload %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Button Text="<%$ resources: Import %>" ToolTip="<%$ resources: Import %>" runat="server"
                                                ID="btnImport" CommandName="SAVE" ValidationGroup="Land" TabIndex="8" OnClientClick="javascript:ValidatePageNow('upload')" />
                                            <div style="font-weight: bold; margin-bottom: 3px;">
                                                <asp:Literal ID="lblNote" runat="server" Visible="false" EnableTheming="false"></asp:Literal>
                                            </div>--%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div id="divFirstPart" runat="server" class="gridwrap grid-group">
                                <h3 id="hHead1" runat="server"></h3>
                                <div class="clear">
                                </div>
                                <div class="grid-group-table">
                                    <table class="gridwraptable gridwrap">
                                        <tr>
                                            <th align="left" width="33%">
                                                <%= GetLocalResourceObject("Account").ToString()%>
                                            </th>
                                            <th align="left" width="12%">
                                                <%= Resources.Controls.Mode %>
                                            </th>
                                            <th align="left" width="25%">
                                                <%= Resources.Controls.Narration %>
                                            </th>
                                            <th id="thAmount" runat="server" align="left" width="20%">
                                                <%--<asp:Label ID="lblThAmount" runat="server" />--%>
                                            </th>
                                            <th align="left" width="10%">
                                                <%= Resources.Controls.Action %>
                                            </th>
                                        </tr>
                                        <tr class="grd-rowhead">
                                            <td>
                                                <asp:TextBox ID="txtAccount" runat="server" Width="95%" TabIndex="8" onfocus="this.select();"
                                                    onMouseUp="return false;" />
                                                <asp:Button ID="btnAccount" runat="server" EnableTheming="false" Style="display: none"
                                                    OnClick="ActionHandler" CommandName="CHANGE" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidAccount" runat="server" CssClass="star" Text="*" />
                                                </div>
                                                <asp:HiddenField ID="hdfAccount" runat="server" />
                                                <asp:HiddenField ID="hdfSubTypePk" runat="server" Value="0" />
                                                <div class="clear">
                                                </div>
                                                <asp:DropDownList ID="ddlSubTypeAccount" runat="server" Visible="false" Width="97%">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlMode" runat="server" Width="95%" onchange="ShowInstrDetails()"
                                                    TabIndex="9" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidMode" runat="server" CssClass="star" Text="*" />
                                                </div>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNarration" runat="server" Width="95%" TabIndex="10" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAmount" runat="server" CssClass="input-w70 numeric" TabIndex="11" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidAmount" runat="server" CssClass="star" Text="*" />
                                                </div>
                                            </td>
                                            <td rowspan="2" style="text-align: center; vertical-align: middle">
                                                <asp:ImageButton ID="imbAddItem" runat="server" SkinID="imbaddnew" CommandName="ADDLITEM"
                                                    TabIndex="12" OnClick="ActionHandler" ValidationGroup="AddItem" OnClientClick="return ValidateAddItems('AddItem');" />
                                            </td>
                                        </tr>
                                        <tr id="trInstrDet" class="grd-rowhead" style="display: none">
                                            <td>
                                                <asp:Label ID="lblInstrNo" runat="server" AssociatedControlID="txtInstrNo" Text="<%$ resources:Controls,InstrNo %>" />
                                                <div class="clear">
                                                </div>
                                                <asp:TextBox ID="txtInstrNo" runat="server" Width="95%" TabIndex="13" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidInstrNo" runat="server" CssClass="star" Text="*" />
                                                </div>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblInstrDate" runat="server" AssociatedControlID="txtInstrDate" Text="<%$ resources:Controls,Date %>" />
                                                <div class="clear">
                                                </div>
                                                <asp:TextBox ID="txtInstrDate" runat="server" Width="90%" TabIndex="14" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidInstrDate" runat="server" CssClass="star" Text="*" />
                                                </div>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFavourOf" runat="server" AssociatedControlID="txtFavourOf" Text="<%$ resources:Controls,FavourOf %>" />
                                                <div class="clear">
                                                </div>
                                                <asp:TextBox ID="txtFavourOf" runat="server" Width="95%" TabIndex="15" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidFavourOf" runat="server" CssClass="star" Text="*" />
                                                </div>
                                            </td>
                                            <td>
                                                <%--<input type="checkbox" id="chkPDC" runat="server" tabindex="15" /><label id="lblPDC"
                                                    runat="server" style="text-align: left;"><%=GetLocalResourceObject("PDC") %></label>   --%>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div id="divSecondPart" runat="server" class="gridwrap grid-group">
                                <h3 id="hHead2" runat="server"></h3>
                                <div class="clear">
                                </div>
                                <div class="grid-group-table">
                                    <table class="gridwraptable gridwrap">
                                        <tr>
                                            <th align="left" width="40%">
                                                <%= GetLocalResourceObject("Account").ToString()%>
                                            </th>
                                            <th align="left" width="11%">
                                                <%= Resources.Controls.Mode %>
                                            </th>
                                            <th align="left" width="23%">
                                                <%= Resources.Controls.Narration %>
                                            </th>
                                            <th id="thDebit" runat="server" align="left" width="8%">
                                                <%= Resources.Controls.DebitAmt %>
                                            </th>
                                            <th id="thCredit" runat="server" align="left" width="8%">
                                                <%= Resources.Controls.CreditAmt %>
                                            </th>
                                            <th id="thBankChrg" runat="server" align="left">
                                                <%-- <%= Resources.Controls.IsBnkChrg %>--%>
                                            </th>
                                            <th align="center" width="10%">
                                                <%= Resources.Controls.Action %>
                                            </th>
                                        </tr>
                                        <tr class="grd-rowhead">
                                            <td>
                                                <asp:TextBox ID="txtAccount1" runat="server" Width="96%" TabIndex="16" onfocus="this.select();"
                                                    onMouseUp="return false;" />
                                                <asp:Button ID="btnAccount1" runat="server" EnableTheming="false" Style="display: none"
                                                    OnClick="ActionHandler" CommandName="CHANGE" />
                                                <asp:HiddenField ID="hdfAccount1" runat="server" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidAccount1" runat="server" CssClass="star" Text="*" />
                                                </div>
                                                <asp:HiddenField ID="hdfSubTypePk1" runat="server" Value="0" />
                                                <div class="clear">
                                                </div>
                                                <asp:DropDownList ID="ddlSubTypeAccount1" runat="server" Visible="false" Width="97.5%">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlMode1" runat="server" Width="95%" onchange="ShowInstrDetails1()"
                                                    TabIndex="17" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidMode1" runat="server" CssClass="star" Text="*" />
                                                </div>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNarration1" runat="server" Width="95%" TabIndex="18" MaxLength="500"
                                                    onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" />
                                            </td>
                                            <td id="tdDebit1" runat="server">
                                                <asp:TextBox ID="txtDebit1" runat="server" CssClass="input-w70 numeric" onchange="SetDebitCreditAmt(1)"
                                                    TabIndex="19" onkeypress="return validateFloatKeyPress(this,event);" MaxLength="14" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidDebit1" runat="server" CssClass="star" Text="*" />
                                                </div>
                                            </td>
                                            <td id="tdCredit1" runat="server">
                                                <asp:TextBox ID="txtCredit1" runat="server" CssClass="input-w70 numeric" onchange="SetDebitCreditAmt()"
                                                    TabIndex="20" onkeypress="return validateFloatKeyPress(this,event);" MaxLength="14" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidCredit1" runat="server" CssClass="star" Text="*" />
                                                </div>
                                            </td>
                                            <td id="tdBankCharge1">
                                                <asp:CheckBox ID="chkIsBankCharge1" runat="server" TabIndex="21" ToolTip="<%$ resources:AmntBeforeVat %>" />
                                                <%--<input type="checkbox" id="Checkbox1" runat="server" tabindex="15" />--%>
                                            </td>
                                            <td style="text-align: center; vertical-align: middle;">
                                                <asp:Button ID="imbShowCostCenterAllocPopup" runat="server" SkinID="costcenter-icon"
                                                    Visible="false" CommandName="COSTCENTER" OnClick="ActionHandler" TabIndex="25"
                                                    ToolTip="<%$ resources:CCButtonToolTip%>" ValidationGroup="AddItem1" OnClientClick="return ValidateAddItems('AddItem1');" />
                                                <asp:ImageButton ID="imbAddItem1" runat="server" SkinID="imbaddnew" CommandName="ADDLITEM"
                                                    TabIndex="21" OnClick="ActionHandler" ValidationGroup="AddItem1" OnClientClick="return ValidateAddItems('AddItem1');" />
                                            </td>
                                        </tr>
                                        <tr id="trInstrDet1" class="grd-rowhead" style="display: none" runat="server">
                                            <td>
                                                <asp:Label ID="lblInstrNo1" runat="server" AssociatedControlID="txtInstrNo1" Text="<%$ resources:Controls,InstrNo %>" />
                                                <div class="clear">
                                                </div>
                                                <asp:TextBox ID="txtInstrNo1" runat="server" Width="95%" TabIndex="23" MaxLength="100"
                                                    onkeydown="limitText(this,100);" onkeyup="limitText(this,100);" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidInstrNo1" runat="server" CssClass="star" Text="*" />
                                                </div>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblInstrDate1" runat="server" AssociatedControlID="txtInstrDate1"
                                                    Text="<%$ resources:Controls,Date %>" />
                                                <div class="clear">
                                                </div>
                                                <asp:TextBox ID="txtInstrDate1" runat="server" Width="90%" TabIndex="24" onkeydown="return CheckKey(event)"
                                                    onpaste="return false;" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidInstrDate1" runat="server" CssClass="star" Text="*" />
                                                </div>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFavourOf1" runat="server" AssociatedControlID="txtFavourOf1" Text="<%$ resources:Controls,FavourOf %>" />
                                                <div class="clear">
                                                </div>
                                                <asp:TextBox ID="txtFavourOf1" runat="server" Width="95%" TabIndex="25" MaxLength="200"
                                                    onkeydown="limitText(this,200);" onkeyup="limitText(this,200);" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidFavourOf1" runat="server" CssClass="star" Text="*" />
                                                </div>
                                            </td>
                                            <td>
                                                <label id="lblPDC1" runat="server" style="text-align: left;">
                                                    <%=GetLocalResourceObject("PDC") %></label>
                                                <div class="clear">
                                                </div>
                                                <input type="checkbox" id="chkPDC1" runat="server" tabindex="15" />
                                            </td>
                                            <td id="tdDebit2" runat="server"></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="gridwrap grid-group">
                                <h3>
                                    <%= Resources.Controls.SelectedAccounts %>
                                </h3>
                                <div class="clear">
                                </div>
                                <div class="grid-group-table">
                                    <asp:GridView runat="server" ID="grdVoucher" Width="100%" AutoGenerateColumns="false"
                                        EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler" ShowFooter="true">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:Controls,AccountCode %>" SortExpression="<%$ resources:DataFieldRes,PackingCode %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAccountCode" runat="server" ToolTip='<%# Eval(Resources.DataTableRes.AccountMst + "." + Resources.DataFieldRes.AccountCode) %>'
                                                        Text='<%# Eval(Resources.DataTableRes.AccountMst + "." + Resources.DataFieldRes.AccountCode) %>' />
                                                    <asp:HiddenField ID="hdfDtlPK" runat="server" Value='<%# Eval(Resources.DataFieldRes.FinTrxPk) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="11%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Controls,AccountName %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAccountName" runat="server" ToolTip='<%# Eval(Resources.DataTableRes.AccountMst + "." + Resources.DataFieldRes.AccountName) %>'
                                                        Text='<%# Eval(Resources.DataTableRes.AccountMst + "." + Resources.DataFieldRes.AccountName) %>' />
                                                    <asp:HiddenField ID="hdfAccntID" runat="server" Value='<%# Eval(Resources.DataFieldRes.FinTrxAccount) %>' />
                                                    <asp:HiddenField ID="hdfAccountSubType" runat="server" Value='<%# Eval(Resources.DataFieldRes.FinTrxSubType) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="30%" />
                                                <FooterTemplate>
                                                    <asp:Panel ID="pnlWHT" runat="server" Visible="false">
                                                        <div class="div2col-S">
                                                            <asp:Label runat="server" ID="lblWHTAmount" Text="<%$ resources:WHTAmount%>" AssociatedControlID="txtWHTAmount"
                                                                Width="83"></asp:Label>
                                                            <asp:TextBox ID="txtWHTAmount" runat="server" TabIndex="20" MaxLength="17" Enabled="false"
                                                                CssClass="Uiinput-amount numeric input-disabled"></asp:TextBox>
                                                            <asp:ImageButton ID="imgHdrTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                                ToolTip="<%$ resources:Tax %>" CommandName="WHTTAXHEADER" />
                                                            <asp:ImageButton SkinID="btnPrint" runat="server" ID="imgbtnPrint" TabIndex="20"
                                                                OnClick="ActionHandler" CommandName="PRINTWHT" ToolTip="<%$ resources:WHTPrint%>" />
                                                        </div>
                                                    </asp:Panel>
                                                </FooterTemplate>
                                                <FooterStyle Width="25%" Wrap="false" HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Controls,Mode %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMode" runat="server" ToolTip='<%# Eval(Resources.DataTableRes.ConfigMst + "." + Resources.DataFieldRes.ConfigName) %>'
                                                        Text='<%# Eval(Resources.DataTableRes.ConfigMst + "." + Resources.DataFieldRes.ConfigName) %>' />
                                                    <asp:HiddenField ID="hdfMode" runat="server" Value='<%# Eval(Resources.DataFieldRes.FinTrxPaymentMode) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Controls,Narration %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNarration" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.FinTrxNarration) %>'
                                                        Text='<%# Eval(Resources.DataFieldRes.FinTrxNarration) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="30%" />
                                                <FooterTemplate>
                                                    <asp:Panel ID="pnlVatBuy" runat="server" Visible="false">
                                                        <div class="div2col-S" style="float: left; width: 64%">
                                                            <asp:Label runat="server" ID="lblVatBuyAmount" Text="<%$ resources:VatBuy%>" AssociatedControlID="txtVatBuy"
                                                                Width="55"></asp:Label>
                                                            <asp:TextBox ID="txtVatBuy" runat="server" TabIndex="20" MaxLength="17" Enabled="false"
                                                                CssClass="Uiinput-amount numeric input-disabled"></asp:TextBox>
                                                            <asp:ImageButton ID="imgVatBuy" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                                ValidationGroup="taxDate" ToolTip="<%$ resources:Tax %>" CommandName="VATTAXHEADER" />
                                                        </div>
                                                    </asp:Panel>
                                                    <div style="float: right; text-align: left; width: 20%">
                                                        <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:Controls,Total %>" Width="55" />
                                                    </div>
                                                </FooterTemplate>
                                                <FooterStyle Font-Bold="true" Wrap="false" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Controls,DebitAmt %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDebit" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.FinTrxDrAmount,"{0:c}") %>'
                                                        Text='<%# Eval(Resources.DataFieldRes.FinTrxDrAmount,"{0:c}") %>' CssClass="input-w70 numeric" />
                                                </ItemTemplate>
                                                <ItemStyle Width="8%" HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblDebitTotal" runat="server" />
                                                </FooterTemplate>
                                                <FooterStyle Font-Bold="true" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Controls,CreditAmt %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCredit" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.FinTrxCrAmount,"{0:c}") %>'
                                                        Text='<%# Eval(Resources.DataFieldRes.FinTrxCrAmount,"{0:c}") %>' CssClass="input-w70 numeric" />
                                                </ItemTemplate>
                                                <ItemStyle Width="8%" HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblCreditTotal" runat="server" />
                                                </FooterTemplate>
                                                <FooterStyle Font-Bold="true" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Controls,Action %>">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imbEditGrid" runat="server" SkinID="imbeditgrid" CommandName="GRIDEDIT"
                                                        OnClick="ActionHandler" TabIndex="25" ToolTip="Edit" />
                                                    <asp:ImageButton ID="imbDeleteGrid" runat="server" SkinID="imbdeletegrid" CommandName="GRIDDELETE"
                                                        OnClientClick="return ShowDeleteConfirm(this);" OnClick="ActionHandler" TabIndex="26"
                                                        ToolTip="Delete" />
                                                </ItemTemplate>
                                                <ItemStyle Width="8%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <%----------WHT Popup Start--------%><div id="divItemTax" style="display: none">
                    <div class="Button-container-popup">
                        <asp:Button runat="server" ID="btnWhtTaxSave" CommandName="WHTTAXSAVE" Text="<%$resources:Controls,Save %>"
                            OnClick="ActionHandler" ToolTip="<%$resources:Controls,Save %>" CommandArgument="PageAction_Entry"
                            SkinID="btnInner-Save" Visible="false" />
                        <asp:Button ID="btnApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply" OnClick="ActionHandler"
                            CommandArgument="PageAction_Entry" CommandName="WHTTAXAPPLY" TabIndex="59" />
                    </div>
                    <div class="content-wrapper">
                        <table class="table-devide">
                            <tr>
                                <td>
                                    <div class="div2col-P">
                                        <asp:Label runat="server" ID="lblformno" Text="<%$ resources:formno%>" AssociatedControlID="ddlFormno"></asp:Label><asp:DropDownList ID="ddlFormno" runat="server" Width="121px" TabIndex="50">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="vrfFormno" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="wht" EnableClientScript="true" InitialValue="-1" runat="server"
                                            ControlToValidate="ddlFormno" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Form %>">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-P">
                                        <asp:Label runat="server" ID="lblTaxid" Text="<%$ resources:Taxid%>" AssociatedControlID="txtTaxid"></asp:Label><asp:TextBox runat="server" ID="txtTaxid" TabIndex="51" MaxLength="50" onkeydown="limitText(this,50);"
                                            onkeyup="limitText(this,50);"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="div2col-P">
                                        <asp:HiddenField ID="hdfWthAddressType" runat="server" Value="" />
                                        <asp:Label runat="server" ID="lblWhtHoBr" Text="<%$ resources:HoBr%>" AssociatedControlID="txtWthAddressType"></asp:Label><asp:TextBox ID="txtWthAddressType" runat="server" MaxLength="100" TabIndex="62"
                                            onkeydown="ClearWhtVendorContacts();" Width="190" OnTextChanged="ActionHandler"> </asp:TextBox><%--<asp:DropDownList ID="ddlAddressType" runat="server" TabIndex="62" OnSelectedIndexChanged="ActionHandler"
                                                        AutoPostBack="true" Width="125">
                                                    </asp:DropDownList>--%><asp:RequiredFieldValidator ID="vrfWthAddressType" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="wht" EnableClientScript="true" runat="server" ControlToValidate="txtWthAddressType"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_HoBr %>" InitialValue="<%$ resources:Messages, AutoDefaultValue %>">
                                                    </asp:RequiredFieldValidator><asp:Label runat="server" ID="lblWhtHo" Text="<%$ resources:Ho%>" AssociatedControlID="chkWthHeadOffice"
                                                        Width="30"></asp:Label><asp:CheckBox ID="chkWthHeadOffice" runat="server" Width="13" CssClass="check-inline"></asp:CheckBox>
                                        <%--<asp:RequiredFieldValidator ID="vrfType" CssClass="star" SetFocusOnError="true" ValidationGroup="VatBuy"
                                            EnableClientScript="true" runat="server" ControlToValidate="txtAddressType" Display="Dynamic"
                                            Text="*" ErrorMessage="<%$ resources:Err_Type %>">
                                        </asp:RequiredFieldValidator>--%>
                                        <asp:Button ID="btnWHTVendor" runat="server" OnClick="ActionHandler" CommandName="WHTCHANGETYPE"
                                            Style="display: none" EnableTheming="false" />
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-P">
                                        <asp:Label runat="server" ID="lblCRTNo" Text="<%$ resources:CRTNO%>"
                                            AssociatedControlID="txtCRTNo" Visible="false" CssClass="input-w29per"></asp:Label><asp:TextBox ID="txtCRTNo" runat="server" MaxLength="10" TabIndex="63" Width="100px" Visible="false" />
                                        <asp:RequiredFieldValidator ID="rfvCRTNO" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="wht" EnableClientScript="true" runat="server" Visible="false"
                                            ControlToValidate="txtCRTNo" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_CRTNo %>">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="div2col-P">
                                        <asp:Label runat="server" ID="lblWthBranchCode" Text="<%$ resources:BranchCode%>"
                                            AssociatedControlID="txtWthBranchCode"></asp:Label><asp:TextBox ID="txtWthBranchCode" runat="server" MaxLength="5" TabIndex="63" Width="100px" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="divcol-P">
                                        <asp:Label ID="lblvendorWHT" runat="server" Text="<%$ resources:Party%>" AssociatedControlID="txtCustomerTxtWHT"></asp:Label><asp:TextBox runat="server" ID="txtCustomerTxtWHT" TabIndex="52" MaxLength="200"
                                            onkeydown="limitText(this,200);" onkeyup="limitText(this,200);"></asp:TextBox><asp:HiddenField ID="hdfvendorWHTPK" runat="server" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="divcol-P">
                                        <asp:Label runat="server" ID="lbladdress" Text="<%$ resources:PartyAds %>" AssociatedControlID="txtpartyads"></asp:Label><asp:TextBox runat="server" ID="txtpartyads" TabIndex="53" TextMode="MultiLine" CssClass="multiline-2line"
                                            MaxLength="500" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                    </div>
                                    <hr />
                                </td>
                            </tr>
                        </table>
                        <table class="table-3devide">
                            <tr>
                                <td align="right" style="width: 31%;">
                                    <div class="div3col-S">
                                        <asp:HiddenField ID="hdfWHTNOPopup" runat="server" Value="" />
                                        <asp:Label runat="server" ID="lblWhtAccountPopup" Text="<%$ resources:WHTAccount%>"
                                            AssociatedControlID="txtWHTAccountPopup"></asp:Label><asp:TextBox ID="txtWHTAccountPopup" runat="server" MaxLength="100" TabIndex="54"> </asp:TextBox><asp:HiddenField ID="hdfWHTAccountPopup" runat="server" Value="" />
                                        <asp:RequiredFieldValidator ID="vrfWHTAccountPopup" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="wht" EnableClientScript="true" runat="server" ControlToValidate="txtWHTAccountPopup"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_WHTAccount %>" InitialValue="<%$ resources:Messages, AutoDefaultValue %>">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td style="width: 22%;">
                                    <div>
                                        <asp:Label runat="server" ID="lbPayType" Text="<%$ resources:PaymentType %>" AssociatedControlID="ddlPayType"
                                            Width="92"></asp:Label><asp:DropDownList ID="ddlPayType" runat="server" Width="80px">
                                            </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="vrfddlPayType" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="wht" EnableClientScript="true" runat="server" ControlToValidate="ddlPayType"
                                            InitialValue="-1" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ValidPayType %>">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td align="center" style="width: 17%;">
                                    <div>
                                        <asp:Label ID="lblWHTAmountPopup" runat="server" Text="<%$ resources:Amount %>" AssociatedControlID="txtPopupWHTAmount"
                                            Width="53"></asp:Label><asp:TextBox ID="txtPopupWHTAmount" TabIndex="55" runat="server" CssClass="input-w70 numeric"
                                                onkeypress="return validateFloatKeyPress(this,event);" EnableViewState="false"
                                                MaxLength="11" onkeyup="CalculateWHTTotal(this);"></asp:TextBox><cc1:AmountValidation ID="vamWHTAmountPopup" runat="server" ControlToValidate="txtPopupWHTAmount"
                                                    ErrorMessage="<%$ resources:Err_ValidWHTAmount %>" NumberDigits="11" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="wht" NonZero="true"></cc1:AmountValidation><asp:RequiredFieldValidator ID="vrfWHTAmountPopup" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="wht" EnableClientScript="true" runat="server" ControlToValidate="txtPopupWHTAmount"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ValidWHTAmount %>">
                                                    </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td style="width: 20%;">
                                    <div>
                                        <asp:Label ID="lblWHTTaxAmountPopup" runat="server" Text="<%$ resources:WHTAmount %>"
                                            AssociatedControlID="txtWHTTaxAmountPopup" Width="80"></asp:Label><asp:TextBox ID="txtWHTTaxAmountPopup" runat="server" CssClass="input-w70 numeric"
                                                EnableViewState="false" onkeypress="return validateFloatKeyPress(this,event);"
                                                MaxLength="11"></asp:TextBox><cc1:AmountValidation ID="vamWHTTaxAmountPopup" runat="server" ControlToValidate="txtWHTTaxAmountPopup"
                                                    ErrorMessage="<%$ resources:Err_WHTTaxAmount %>" NumberDigits="11" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="wht" NonZero="true"></cc1:AmountValidation><asp:RequiredFieldValidator ID="vrfWHTTaxAmountPopup" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="wht" EnableClientScript="true" runat="server" ControlToValidate="txtWHTTaxAmountPopup"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_WHTTaxAmount %>">
                                                    </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <asp:Button runat="server" ID="btnWHTAccountPopup" CommandName="WHT_ACCOUNT_INDEX_CHANGED_POPUP"
                                    OnClick="ActionHandler" EnableTheming="false" Style="display: none" TabIndex="57" />
                            </tr>
                        </table>
                        <div class="divcol-P">
                            <asp:HiddenField ID="hdfWHTTaxCategory" runat="server" />
                            <asp:HiddenField ID="hdfWHTTaxName" runat="server" />
                            <asp:Label ID="lblDescriptionPopup" runat="server" Text="<%$ resources:Description %>"
                                AssociatedControlID="txtDescriptionPopup"></asp:Label><asp:TextBox ID="txtDescriptionPopup" TabIndex="56" runat="server" EnableViewState="false"
                                    MaxLength="500" Width="70%" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox><asp:ImageButton ID="imgPopupAdd" SkinID="imbaddnew" runat="server" OnClick="ActionHandler"
                                        TabIndex="57" CommandArgument="PageAction_Entry" ValidationGroup="wht" ToolTip="Add"
                                        CommandName="WHTTAXADD" OnClientClick="javascript:ValidatePageNow('wht')" />
                        </div>
                        <div class="error" id="divErrorLabel" runat="server" visible="false">
                            <ul>
                                <li>
                                    <asp:Literal runat="server" ID="lblSplitErrorMessage" Text=""></asp:Literal></li>
                            </ul>
                        </div>
                        <div class="gridwrap">
                            <asp:GridView runat="server" ID="grdWHTTaxDetails" Width="100%" AllowSorting="false"
                                TabIndex="58" AutoGenerateColumns="false" OnRowDataBound="ActionHandler" EmptyDataRowStyle-CssClass="emptytable">
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ resources:formno %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblformnoGRD" runat="server"></asp:Label><asp:HiddenField ID="hdfWHTFormNo" runat="server" Value='<%#Eval("WTH_FORM_NO") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="13%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Party %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblParty" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("WTH_PARTY_NAME"),17) %>'
                                                ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("WTH_PARTY_NAME")) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="27%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:PartyAds %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrtyAddress" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("WTH_ADDRESS"),30) %>'
                                                ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("WTH_ADDRESS")) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="27%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Type %>">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdfWhtBranchType" runat="server" Value='<%#Eval("WTH_BRANCH_TYPE") %>' />
                                            <asp:Label ID="lblWhtTye" runat="server" Text=""></asp:Label><%-- <asp:Label ID="lblTye" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("WTH_BRANCH_TEXT"),30) %>'
                                                ToolTip='<%# Eval("WTH_BRANCH_TEXT") %>'></asp:Label>--%>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:HoBr %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWhtBranchName" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("WTH_BRANCH_NAME"),30) %>'
                                                ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("WTH_BRANCH_NAME")) %>'></asp:Label>
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
                                                ToolTip='<%# GetFormattedCurrency(Eval("WTH_AMOUNT")) %>'></asp:Label><asp:HiddenField ID="hdfWHTTaxName1" runat="server" Value='<%#Eval("WTH_NAME") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:WHTAmount %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWHTTaxAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("WTH_TAX_AMT")) %>'
                                                ToolTip='<%#GetFormattedCurrency(Eval("WTH_TAX_AMT")) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Desc %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWHTDesc" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("WTH_DESC")) %>'
                                                ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("WTH_DESC")) %>'></asp:Label>
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
                            SkinID="btnInner-Save" TabIndex="74" />
                        <asp:Button ID="btnVatTaxApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply"
                            OnClick="ActionHandler" CommandArgument="PageAction_Entry" CommandName="VATTAXAPPLY"
                            TabIndex="74" />
                    </div>
                    <div class="content-wrapper">
                        <table class="table-devide">
                            <tr>
                                <td>
                                    <div class="div2col-P">
                                        <asp:Label ID="lblVendor" runat="server" Text="<%$ resources:Vendor %>" AssociatedControlID="txtVendorPopup"></asp:Label><asp:TextBox ID="txtVendorPopup" runat="server" MaxLength="100" TabIndex="60" onkeydown="ClearVendorContacts();"
                                            OnTextChanged="ActionHandler" AutoPostBack="true"> </asp:TextBox><%--<asp:TextBox ID="txtTo" runat="server" TextMode="MultiLine" MaxLength="500" TabIndex="7"
                                                CssClass="multiline-1col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" />--%><asp:HiddenField ID="hdfVendorPopup" runat="server" />
                                        <%-- <asp:HiddenField ID="hdfVatVendorPK" runat="server" />--%>
                                        <asp:Button ID="btnVendorPopup" runat="server" OnClick="ActionHandler" CommandName="VENDORSELECTEDDTL"
                                            Style="display: none" EnableTheming="false" />
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-P">
                                        <asp:Label runat="server" ID="lblVatBuyTaxId" Text="<%$ resources:Taxid%>" AssociatedControlID="txtVatTaxId"></asp:Label><asp:TextBox runat="server" ID="txtVatTaxId" TabIndex="61" MaxLength="50" onkeydown="limitText(this,50);"
                                            onkeyup="limitText(this,50);"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="div2col-P">
                                        <asp:HiddenField ID="hdfAddressType" runat="server" Value="" />
                                        <asp:Label runat="server" ID="lblAddressType" Text="<%$ resources:HoBr%>" AssociatedControlID="txtAddressType"></asp:Label><asp:TextBox ID="txtAddressType" runat="server" MaxLength="100" TabIndex="62" Width="190"
                                            OnTextChanged="ActionHandler" AutoPostBack="true"> </asp:TextBox><%-- <asp:DropDownList ID="ddlAddressType" runat="server" TabIndex="62" OnSelectedIndexChanged="ActionHandler"
                                            AutoPostBack="true" Width="200">
                                        </asp:DropDownList>--%><asp:RequiredFieldValidator ID="vrfAddressType" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="vatbuy" EnableClientScript="true" runat="server" ControlToValidate="txtAddressType"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_HoBr %>">
                                        </asp:RequiredFieldValidator><asp:Label runat="server" ID="lblHO" Text="<%$ resources:Ho%>" AssociatedControlID="chkHeadOffice"
                                            Width="50"></asp:Label><asp:CheckBox ID="chkHeadOffice" runat="server" Width="13" CssClass="check-inline"
                                                OnCheckedChanged="ActionHandler" AutoPostBack="true"></asp:CheckBox>
                                        <%--<asp:RequiredFieldValidator ID="vrfType" CssClass="star" SetFocusOnError="true" ValidationGroup="VatBuy"
                                            EnableClientScript="true" runat="server" ControlToValidate="txtAddressType" Display="Dynamic"
                                            Text="*" ErrorMessage="<%$ resources:Err_Type %>">
                                        </asp:RequiredFieldValidator>--%>
                                        <asp:Button ID="btnVendorContDtl" runat="server" OnClick="ActionHandler" CommandName="CHANGETYPE"
                                            Style="display: none" EnableTheming="false" />
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-P">
                                        <asp:Label runat="server" ID="lblBranchCode" Text="<%$ resources:BranchCode%>" AssociatedControlID="txtBranchCode"></asp:Label><asp:TextBox ID="txtBranchCode" runat="server" MaxLength="5" TabIndex="63" Width="97" />
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
                                        <asp:Label runat="server" ID="lblVatTaxInvDate" Text="<%$ resources:TaxInvoiceDate%>"
                                            AssociatedControlID="txtVatTaxInvDate"></asp:Label><asp:TextBox ID="txtVatTaxInvDate" runat="server" MaxLength="200" CssClass="medium"
                                                TabIndex="64" onkeydown="return CheckKey(event)" onpaste="return false;" />
                                        <asp:RequiredFieldValidator ID="vrfVatTaxInvDate" runat="server" CssClass="star"
                                            SetFocusOnError="true" InitialValue="" ValidationGroup="vatbuy" EnableClientScript="true"
                                            Display="Dynamic" Text="*" ControlToValidate="txtVatTaxInvDate" ErrorMessage="<%$ resources:Err_Invoicedate %>" />
                                        <asp:Label runat="server" ID="lblOriginalInv" Text="<%$ resources:OriginalInvReceived%>"
                                            AssociatedControlID="chkOriginalinvoice" Width="125"></asp:Label><asp:CheckBox ID="chkOriginalinvoice" runat="server" Width="13" CssClass="check-inline"></asp:CheckBox>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-P">
                                        <asp:Label runat="server" ID="lblVatTaxInvNo" Text="<%$ resources:TaxInvoiceNo%>"
                                            AssociatedControlID="txtVatTaxInvNo"></asp:Label><asp:TextBox runat="server" ID="txtVatTaxInvNo" TabIndex="65" MaxLength="100" onkeydown="limitText(this,100);"
                                                onkeyup="limitText(this,100);"></asp:TextBox><asp:RequiredFieldValidator ID="vrfTaxInvNo" CssClass="star" SetFocusOnError="true"
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
                                            AssociatedControlID="txtBeforeTaxAmount"></asp:Label><asp:TextBox ID="txtBeforeTaxAmount" TabIndex="66" runat="server" CssClass="input-w80 numeric"
                                                onkeypress="return validateFloatKeyPress(this,event);" EnableViewState="false"
                                                MaxLength="11" onkeyup="CalculateVatBuyTotal(this);"></asp:TextBox><cc1:AmountValidation ID="vamAmtBeforeTax" runat="server" ControlToValidate="txtBeforeTaxAmount"
                                                    ErrorMessage="<%$ resources:Err_ValidVatAmount %>" NumberDigits="11" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="vatbuy" NonZero="true"></cc1:AmountValidation><asp:RequiredFieldValidator ID="vrfBeforeTaxAmount" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="vatbuy" EnableClientScript="true" runat="server" ControlToValidate="txtBeforeTaxAmount"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_BeforeTaxAmount %>">
                                                    </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-P">
                                        <asp:HiddenField ID="hdfVATNOPopup" runat="server" Value="" />
                                        <asp:Label runat="server" ID="lblVatAccountPopup" Text="<%$ resources:VatBuy%>" AssociatedControlID="ddlVATAccountPopup"></asp:Label><%--<asp:TextBox ID="txtVATAccountPopup" runat="server" MaxLength="100" TabIndex="54"> </asp:TextBox>--%><asp:DropDownList ID="ddlVATAccountPopup" TabIndex="67" runat="server" EnableViewState="true"
                                            CssClass="medium" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hdfVATAccountPopup" runat="server" Value="" />
                                        <asp:RequiredFieldValidator ID="vrfVATAccountPopup" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="vatbuy" EnableClientScript="true" runat="server" ControlToValidate="ddlVATAccountPopup"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_VatBuyAccount %>"
                                            InitialValue="-1">
                                        </asp:RequiredFieldValidator><%--<asp:Label ID="lblVatTax" runat="server" Text="<%$ resources:Tax %>" AssociatedControlID="txtVATTaxAmountPopup"></asp:Label>--%><asp:TextBox ID="txtVATTaxAmountPopup" runat="server" CssClass="input-w80 numeric"
                                            onkeypress="return validateFloatKeyPress(this,event);" EnableViewState="false"
                                            MaxLength="11" TabIndex="68"></asp:TextBox><cc1:AmountValidation ID="vamVATTaxAmountPopup" runat="server" ControlToValidate="txtVATTaxAmountPopup"
                                                ErrorMessage="<%$ resources:Err_VATTaxAmount %>" NumberDigits="11" Display="Dynamic"
                                                Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="vatbuy" NonZero="true"></cc1:AmountValidation><asp:RequiredFieldValidator ID="vrfVATTaxAmountPopup" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="vatbuy" EnableClientScript="true" runat="server" ControlToValidate="txtVATTaxAmountPopup"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Tax %>">
                                                </asp:RequiredFieldValidator>
                                    </div>
                                    <asp:Button runat="server" ID="btnVatBuyAccountPopup" CommandName="VAT_ACCOUNT_INDEX_CHANGED_POPUP"
                                        OnClick="ActionHandler" EnableTheming="false" Style="display: none" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="div2col-P">
                                        <asp:Label ID="lblMaterial" runat="server" Text="<%$ resources:Material %>" AssociatedControlID="txtMaterial"></asp:Label><asp:TextBox ID="txtMaterial" TabIndex="69" runat="server" EnableViewState="false"
                                            MaxLength="200" Width="60%" onkeydown="limitText(this,200);" onkeyup="limitText(this,200);"></asp:TextBox><asp:RequiredFieldValidator ID="vrfMaterial" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="vatbuy" EnableClientScript="true" runat="server" ControlToValidate="txtMaterial"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Material %>">
                                            </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-P">
                                        <asp:Label runat="server" ID="lblRefundDate" Text="<%$ resources:RefundMonth %>"
                                            AssociatedControlID="txtVatRefundDate"></asp:Label><asp:TextBox ID="txtVatRefundDate" runat="server" MaxLength="200" CssClass="medium"
                                                TabIndex="65" onkeydown="return CheckKey(event)" onpaste="return false;" />
                                        <cc1:CalendarExtender runat="server" ID="txtVatRefundDate_CalendarExtender" BehaviorID="calendar1"
                                            TargetControlID="txtVatRefundDate" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                                            ClientIDMode="Static" OnClientHidden="onCalendarHidden">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="vrfVatRefundDate" runat="server" CssClass="star"
                                            SetFocusOnError="true" InitialValue="" ValidationGroup="vatbuy" EnableClientScript="true"
                                            Display="Dynamic" Text="*" ControlToValidate="txtVatRefundDate" ErrorMessage="<%$ resources:Err_RefundDate %>" />
                                        <asp:ImageButton ID="imgVatPopupAdd" SkinID="imbaddnew" runat="server" OnClick="ActionHandler"
                                            TabIndex="70" CommandArgument="PageAction_Entry" ValidationGroup="vatbuy" ToolTip="Add"
                                            CommandName="VATTAXADD" OnClientClick="javascript:ValidatePageNow('vatbuy')" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div class="divcol-P">
                            <%-- <asp:Label ID="lblMaterial" runat="server" Text="<%$ resources:Material %>" AssociatedControlID="txtMaterial"></asp:Label>
                            <asp:TextBox ID="txtMaterial" TabIndex="69" runat="server" EnableViewState="false"
                                MaxLength="400" Width="70%"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vrfMaterial" CssClass="star" SetFocusOnError="true"
                                ValidationGroup="vatbuy" EnableClientScript="true" runat="server" ControlToValidate="txtMaterial"
                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Material %>">
                            </asp:RequiredFieldValidator>
                            <asp:ImageButton ID="imgVatPopupAdd" SkinID="imbaddnew" runat="server" OnClick="ActionHandler"
                                TabIndex="70" CommandArgument="PageAction_Entry" ValidationGroup="vatbuy" ToolTip="Add"
                                CommandName="VATTAXADD" OnClientClick="javascript:ValidatePageNow('vatbuy')" />--%>
                        </div>
                        <div class="error" id="divVatErrorLabel" runat="server" visible="false">
                            <ul>
                                <li>
                                    <asp:Literal runat="server" ID="lblVatSplitErrorMessage" Text=""></asp:Literal></li>
                            </ul>
                        </div>
                        <div class="gridwrap">
                            <asp:GridView runat="server" ID="grdVATTaxDetails" Width="100%" AllowSorting="false"
                                TabIndex="71" AutoGenerateColumns="false" OnRowDataBound="ActionHandler" EmptyDataRowStyle-CssClass="emptytable"
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
                                        <ItemStyle Width="27%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Party %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblParty" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("WTH_PARTY_NAME"),17) %>'
                                                ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("WTH_PARTY_NAME")) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="27%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Type %>">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdfBranchType" runat="server" Value='<%#Eval("WTH_BRANCH_TYPE") %>' />
                                            <asp:Label ID="lblTye" runat="server" Text=""></asp:Label><%-- <asp:Label ID="lblTye" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("WTH_BRANCH_TEXT"),30) %>'
                                                ToolTip='<%# Eval("WTH_BRANCH_TEXT") %>'></asp:Label>--%>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:HoBr %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBranchName" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("WTH_BRANCH_NAME"),30) %>'
                                                ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("WTH_BRANCH_NAME")) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="30%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Taxid %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTxId" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("WTH_TAX_ID"),15) %>'
                                                ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("WTH_TAX_ID")) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:Total %>" />
                                        </FooterTemplate>
                                        <FooterStyle Font-Bold="true" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:VatBuyAccount %>">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdfVATTax" runat="server" Value='<%#Eval("WTH_TAX") %>' />
                                            <asp:HiddenField ID="hdfVATTaxCategoryGrid" runat="server" Value='<%#Eval("WTH_TAX_CATEGORY") %>' />
                                            <asp:Label ID="lblVATAccountGrid" runat="server" Text='<%# Convert.ToString(Eval("WTH_NAME")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("WTH_NAME")) %>'
                                                ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("WTH_NAME")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("WTH_NAME"))) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="22%" />
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
                                                ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("WTH_ITEM_TEXT")) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="30%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imbVatTaxEdit" runat="server" SkinID="imbeditgrid" CommandName="POPUPGRIDEDIT"
                                                OnClick="ActionHandler" TabIndex="72" ToolTip="Edit" />
                                            <asp:ImageButton ID="imbVatTaxRemove" runat="server" OnClick="ActionHandler" CommandName="VATTAXDELETE"
                                                CommandArgument="PageAction_Entry" SkinID="btnclose" ToolTip="Remove" TabIndex="73" />
                                        </ItemTemplate>
                                        <ItemStyle Width="8%" Wrap="false" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <%----------VAT BUY Popup End--------%>
                <%----------Cost Center Popup Start--------%>
                <div id="divVoucherCC" style="display: none">
                    <div class="content-wrapper">
                        <div class="Button-container-popup">
                            <asp:Button ID="btnVoucherCCApply" SkinID="btnInner-add-dsd" runat="server" Text="<%$resources:ErpRes,Apply %>"
                                OnClick="ActionHandler" TabIndex="54" CommandName="COSTCENTERAPPLY" ToolTip="<%$resources:ErpRes,Apply %>"
                                ValidationGroup="VoucherCostCenter" OnClientClick="javascript:ValidatePageNow('VoucherCostCenter')" />
                            <asp:Button ID="btnVoucherCCCancel" SkinID="btnInner-cancel-dsd" runat="server" Text="<%$resources:ErpRes,Cancel %>"
                                ToolTip="<%$resources:ErpRes,Cancel %>" OnClick="ActionHandler" TabIndex="54"
                                CommandName="COSTCENTERCANCEL" />
                        </div>
                        <div class="gridwrap">
                            <asp:GridView runat="server" ID="grdVoucherCC" Width="100%" AutoGenerateColumns="false"
                                EmptyDataRowStyle-CssClass="emptytable" ShowFooter="true">
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ resources:CostCenter %>">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdfVoucherCCPk" runat="server" Value='<%# Eval("FCM_CNM_PK")%>' />
                                            <asp:Label ID="lblVoucherCC" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("FCM_COST_CENTER_TEXT"),40)%>'
                                                ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("FCM_COST_CENTER_TEXT"))%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="80%" Wrap="false" />
                                        <FooterTemplate>
                                            <asp:Label ID="lblVoucherCCTotal" runat="server" Text="<%$ resources:Total %>"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:CostCenterAmount %>">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtVoucherCCAmountTC" runat="server" CssClass="input-full numeric margn-rgt0"
                                                ValidationGroup="VoucherCostCenter" MaxLength="15" Text='<%# GetFormattedCurrency(Eval("FTD_AMT_BC"))%>'
                                                onkeyup="CalculateTotalCCAmount();" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);">
                                            </asp:TextBox><cc1:AmountValidation ID="vccVoucherAmountTC" runat="server" ControlToValidate="txtVoucherCCAmountTC"
                                                ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                                Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="VoucherCostCenter"
                                                NonZero="false"></cc1:AmountValidation>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" CssClass="amount-numeric" />
                                        <ItemStyle Width="20%" Wrap="false" HorizontalAlign="Right" />
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalVoucherCCAmnt" runat="server"></asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <%----------Cost Center Popup End--------%>
                <%----------Audit Log Popup Start--------%>
                <div id="divJournalAuditLog" style="display: none" class="content-wrapper">
                    <%--<div class="gridwrap" style="overflow-x: hidden !important;">
                        <asp:GridView runat="server" ID="grdAuditLog" Width="100%" AutoGenerateColumns="false"
                            EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="grdAuditLog_RowDataBound">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label></EmptyDataTemplate><Columns>
                                <asp:TemplateField HeaderText="<%$ resources:Version %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAuditVersion" runat="server" Text='<%# Eval("FTH_AUDIT_VERSION") %>'
                                            ToolTip='<%# Eval("FTH_AUDIT_VERSION") %>'></asp:Label><asp:HiddenField ID="hdfFTH_PK" runat="server" Value='<%# Eval("FTH_PK") %>' />
                                        <asp:HiddenField ID="hdfRowIndex" runat="server" Value='<%#Container.DataItemIndex %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Activity %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActivity" runat="server" Text='<%# Eval("FTH_ACTIVITY") %>'
                                            ToolTip='<%# Eval("FTH_ACTIVITY") %>'></asp:Label></ItemTemplate><ItemStyle Width="8%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:UserName %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lbUsername" runat="server" Text='<%# Eval("FTH_MOD_BY_TEXT") %>'
                                            ToolTip='<%# Eval("FTH_MOD_BY_TEXT") %>'></asp:Label></ItemTemplate><ItemStyle Width="8%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:DateTime %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDateTime" runat="server" Text='<%# Eval("FTH_MOD_DT",Resources.ErpRes.GridFormatDatetime) %>'
                                            ToolTip='<%# Eval("FTH_MOD_DT",Resources.ErpRes.GridFormatDatetime) %>'></asp:Label></ItemTemplate><ItemStyle Width="15%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>--%>
                    <uc1:AuditLogList ID="ucrAuditLogList" runat="server" />
                </div>
                <%----------Audit Log Popup End--------%>
            </div>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star">
                </asp:Label><asp:ValidationSummary ID="vsPage" ValidationGroup="Voucher" runat="server" />
                <asp:ValidationSummary ID="vsAddItem" ValidationGroup="AddItem" runat="server" />
                <asp:ValidationSummary ID="vsAddItem1" ValidationGroup="AddItem1" runat="server" />
                <asp:ValidationSummary ID="vsLoad" ValidationGroup="ycvLoad" runat="server" />
                <asp:ValidationSummary ID="vsVoucherCostCenter" ValidationGroup="VoucherCostCenter"
                    runat="server" />
                <%-- <asp:ValidationSummary ID="vvsLanding" ValidationGroup="upload" runat="server" />--%>
                <asp:ValidationSummary ID="vvsLandingDtl" ValidationGroup="LandDtl" runat="server" />
            </div>
            <div id="diverrorwht" style="display: none">
                <asp:ValidationSummary ID="vsWht" ValidationGroup="wht" runat="server" />
            </div>
            <div id="diverrorvatbuy" style="display: none">
                <asp:ValidationSummary ID="vsVatbuy" ValidationGroup="vatbuy" runat="server" />
            </div>
            <div id="divScriptButtons">
                <asp:Button runat="server" ID="btnJournalize_Action" CommandName="JOURNALIZE" OnClick="ActionHandler"
                    EnableTheming="false" Style="display: none" />
                <asp:Button ID="btnJournalizeUpdate" runat="server" OnClick="ActionHandler" CommandName="JOURNALIZEUPDATE"
                    EnableTheming="false" Style="display: none" />
                <asp:Button ID="btnVoucherComparision" runat="server" OnClick="ActionHandler" CommandName="COMPARE"
                    EnableTheming="false" Style="display: none" />
                <a id="anchorFile" runat="server" target="_blank" tabindex="15" visible="false" style="margin-left: 25.5% !important"
                    href="LogVersionComparision.aspx"></a>
            </div>
            <div id="divJournalize" style="display: none">
                <uc1:Journalize ID="ucrJournalize" runat="server" />
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="inv">
                </uc1:WorkflowUserComments>
            </div>
            <asp:HiddenField ID="hdfIscontYes" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIscontYesVat" runat="server" Value="0" />
            <asp:HiddenField ID="hdfJournalizeWorkFlow" Value="0" runat="server" />
            <asp:HiddenField ID="hdfJournalName" runat="server" />
            <asp:HiddenField ID="hdfShowChequeReturn" runat="server" Value="0" />
            <asp:HiddenField ID="hdfShowPdc" runat="server" Value="0" />
            <asp:HiddenField ID="hdfShowCashAccountDiv" runat="server" Value="0" />
            <asp:HiddenField ID="hdfShowOthChrgChkBox" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCRTNoAuto" runat="server" Value="0" />
            <asp:HiddenField ID="hdfShowCRTNo" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsSBUVendor" runat="server" Value="0" />
            <asp:HiddenField ID="hdfItemEdit" runat="server" Value="0" />
            <asp:HiddenField ID="hdfPrintCheque" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
            <asp:HiddenField ID="hdfCurrencyGroup2" Value="3" runat="server" />
            <asp:HiddenField ID="hdfIsContCCAllocDeletion" Value="0" runat="server" />
            <asp:HiddenField ID="hdfPreviousAccount1Name" Value="" runat="server" />
            <asp:HiddenField ID="hdfPreviousAccount1Pk" Value="0" runat="server" />
            <asp:HiddenField ID="hdfAccountType" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsShowVoucherDataImport" runat="server" Value="0" />
            <asp:HiddenField ID="hdfJournalType" runat="server" Value="" />
            <asp:HiddenField ID="hdfStatus" runat="server" Value="0" />
            <asp:HiddenField ID="hdfSelRowVer" runat="server" Value="0" />
            <asp:HiddenField ID="hdfSelRowTranPk" runat="server" Value="0" />
            <asp:HiddenField ID="hdfSelRow" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
