<%@ Page Title="<%$ Resources:Captions,Title_SalesReceiptTrading %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="SalesReceiptTrading.aspx.cs"
    Theme="ClassicExt" Inherits="ERPSMS_v01.Sales.SalesReceiptTrading" %>

<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/Journalize/UserControls/JournalizeControlNew.ascx" TagName="Journalize"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var NumberDigits = 0;
        var CurrencyDigits = 0;
        $(document).ready(function () {
            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
            CurrencyDigits = parseInt($("[id$=hdfDecimalDigits]").val());
        });


        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        //var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function ModeAutoComplete() {
            if ($("[id$=ddlMode]").val() > 0) {
                if ($("[id$=ddlMode]").val() == "2" || $("[id$=ddlMode]").val() == "4") {
                    if ($('[id$=txtBank]').val() != "" && $('[id$=txtBank]').val() != "Select/Type") {
                        GrandScriptUtils.MakeAutoCompleteDDL("txtBank", url + "?Type=" + $("[id$=ddlMode]").val(), "hdfBank", true, true, "BANK");
                    }
                    else {
                        GrandScriptUtils.MakeAutoCompleteDDL("txtBank", url + "?Type=" + $("[id$=ddlMode]").val(), "hdfBank", true, true, "BANK", false, false, true);
                    }
                }
                else {
                    if ($("[id$=ddlMode]").val() != "6")
                        GrandScriptUtils.MakeAutoCompleteDDL("txtBank", url + "?Type=" + $("[id$=ddlMode]").val(), "hdfBank", true, true, "BANK", false, false, true);
                }
            }
        }

        function InitComponents() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomerSearch", url, "hdfCustomerSearchPk", true, true, "CUSTOMERLIST");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url, "hdfCustomerPk", true, true, "CUSTOMERLIST");
            //GrandScriptUtils.MakeAutoCompleteDDL("txtBankChargeCurrency", url, "hdfBankChargeCurrency", true, true, "CURRENCY");
            GrandScriptUtils.AddDateRangeCommon("txtReceiptDate", "hdnReceiptDate", "txtReturnDate", "hdnReturnDate", false, false); // GrandScriptUtils.DatePickerCommon("txtReceiptDate");            
            GrandScriptUtils.MakeAutoCompleteDDL("txtReceiptNumber", url, "hdfReceiptPK", true, true, "SALESRECEIPTNUMBER");
            GrandScriptUtils.AddDateRangeCommon("txtSearchDateFrom", "hdfSearchDateFrom", "txtSearchDateTo", "hdfSearchDateTo", false, false);
            GrandScriptUtils.AddDateRangeCommon("txtPendingFromDate", "hdfPendingFromDate", "txtPendingToDate", "hdfPendingToDate", false, false);
            GrandScriptUtils.DatePickerCommon("txtInstrumentDate");
            GrandScriptUtils.DatePickerCommon("txtPVDate");
            //$("[id$=txtJournalExchangeRate]").ForceNumericOnly();
            $("[id*=txtReceivedNow]").ForceNumericOnly();
            $("[id*=txtPayNowSplit]").ForceNumericOnly();
            $("[id$=txtExchangeRate]").ForceNumericOnly();
            $("[id*=txtCrdrReceiveNow]").ForceNumericOnly();
            $("[id*=txtCrdrAdjAmount]").ForceNumericOnly();
            $("[id*=txtOthercharges]").ForceNumericOnly();
            $("[id*=txtAdjustments]").ForceNumericOnly();

            InitPendingInvNo();
            if ($("[id$=txtPendingInvNumber]").attr("disabled") == true) {
                DisableAuto($("[id$=txtPendingInvNumber]"), $("[id$=hdfPendingInvPk]"));
            }
            else {
                EnableAuto($("[id$=txtPendingInvNumber]"), $("[id$=hdfPendingInvPk]"));
            }

            ShowHidePendingInv($("[id$=hdfIsPendingInvVisible]").val());

            if ($('[id$=btnJournalSaveSubmit]').is(":visible"))
                $('[id$=btnJournalSubmit]').hide();
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();


            if ($('[id$=btnEditforCancel]').is(":visible")) {
                if ($('[id$=hdfShowCancel]').val() == 0) {
                    $('[id$=btnEditforCancel]').hide();
                }
            }


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
                    $('[id$=btnJournalize]').show();

                }
                else if ($('[id$=hdfShowChequeReturn]').val() == 5) {
                    $('[id$=btnReturnDetail]').show();
                }
                else if ($('[id$=hdfShowChequeReturn]').val() == 6) {
                    $('[id$=btnReturnDetail]').hide();
                }
            }
            if (($('[id$=chkPDC]').is(':checked')) && ($('[id$=hdfShowChequeReturn]').val() == 5)) {
                $('[id$=btnReturnDetail]').show();
            } else {
                $('[id$=btnReturnDetail]').hide();
            }


            //Set a stamp for cancelled record
            if ($("[id$=hdfIsCancelled]").val() == "1")
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
            else
                $("[id$=tblDetailHdr]").addClass("table-devide");

            //End
            ReceiptReturnChecked();

            if ($("[id$=txtCustomer]").attr("disabled") == true) {
                DisableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomerPk]"));
            }
            else {
                EnableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomerPk]"));
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

        function parseDateFormat(s) {
            return new Date(s.replace(/^(\d+)\W+(\w+)\W+/, '$2 $1 '));
        }


        function InitPendingInvNo() {
            if (parseInt($("[id$=ddlPendingInvCategory]").val()) == 3)//Misc Invoice
            {
                GrandScriptUtils.MakeAutoCompleteDDL("txtPendingInvNumber", url + "?CustomerID=" + $("[id$=hdfCustomerPk]").val() + "&InvGroup=3&InvType=" + $("[id$=ddlPendingInvType]").val(), "hdfPendingInvPk", true, true, "SALINVNUMBERTRADING");
            }
            else {
                GrandScriptUtils.MakeAutoCompleteDDL("txtPendingInvNumber", url + "?CustomerID=" + $("[id$=hdfCustomerPk]").val() + "&InvCategory=" + $("[id$=ddlPendingInvCategory]").val() + "&InvType=" + $("[id$=ddlPendingInvType]").val(), "hdfPendingInvPk", true, true, "SALINVNUMBERTRADING");
            }
        }

        function ResetPendingInvNo() {
            var defText = '<%= Resources.ErpRes.AutoDefaultValue %>';
            $("[id$=txtPendingInvNumber]").val(defText);
            $("[id$=hdfPendingInvPk]").val('-1');
        }

        function AfterDateSelect(controlID) {
            //            if (controlID == "txtInstrumentDate") {
            //                if ($('[id$=txtInstrumentDate]').val() != "") {
            //                    var ChequeDate = parseDateFormat($('[id$=txtInstrumentDate]').val());
            //                    var pdcReconStatus = '<%= GetGlobalResourceObject("ConfigurationsRes","PDCReconciliation").ToString() %>';
            //                    var Todt = new Date();
            //                    //var Todate = parseDateFormat(Todt.getDate() + "-" + Todt.getMonth() + "-" + Todt.getFullYear());
            //                    if (pdcReconStatus == 1) {
            //                        $("[id$='chkPDC']").attr("checked", true);
            //                    }
            //                    else {
            //                        if (Todt <= ChequeDate) {
            //                            $("[id$='chkPDC']").attr("checked", true);
            //                        }
            //                        else {
            //                            $("[id$='chkPDC']").attr("checked", false);
            //                        }
            //                    }
            //                }
            //            }

            if (controlID == "txtInstrumentDate" || controlID == "txtReceiptDate") {
                if ($('[id$=txtInstrumentDate]').val() != "" && $('[id$=txtReceiptDate]').val() != "") {
                    var ChequeDate = parseDateFormat($('[id$=txtInstrumentDate]').val());
                    var ReceiptDate = parseDateFormat($('[id$=txtReceiptDate]').val());
                    var pdcReconStatus = '<%= GetGlobalResourceObject("ConfigurationsRes","PDCReconciliation").ToString() %>';
                    //var Todt = new Date();
                    //var Todate = parseDateFormat(Todt.getDate() + "-" + Todt.getMonth() + "-" + Todt.getFullYear());
                    if (pdcReconStatus == 1) {
                        $("[id$='chkPDC']").attr("checked", true);
                    }
                    else {
                        if (ChequeDate > ReceiptDate) {
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
                        $("[id$=hdfSaveWithoutBankCharge]").val("1");
                        $(this).dialog("close");
                        $("[id$=btnSaveSubmit]").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfIscontYes]").val(0);
                        $("[id$=hdfSaveWithoutBankCharge]").val("1");
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
                        $("[id$=hdfSaveWithoutBankCharge]").val("1");
                        $(this).dialog("close");
                        $("[id$=btnSave]").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfIscontYes]").val(0);
                        $("[id$=hdfSaveWithoutBankCharge]").val("1");
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
                    //ShowContainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), '1000', '550');
                    //                    ShowContainerDiv('[id$=divJournalize]', $("[id$=hdfJournalName]").val(), '1000', '550');
                    ShowCommonCotainerDiv('[id$=divJournalize]', $("[id$=hdfJournalName]").val(), "1%");
                    AfterCloseWkfInJournal();
                    //$("[id$=btnJournalize_Action]").click();
                }
            } else if (containerID == "[id$=divTemplate]") {
                //ShowContainerDiv('[id$=divJournalize]', $("[id$=hdfJournalName]").val(), '1000', '550');
                ShowCommonCotainerDiv('[id$=divJournalize]', $("[id$=hdfJournalName]").val(), "1%");
            }
        }

        function ChangeMode() {
            if ($("[id$=ddlMode]").val() > 0) {
                GrandScriptUtils.MakeAutoCompleteDDL("txtBank", url + "?Type=" + $("[id$=ddlMode]").val(), "hdfBank", true, true, "BANK");
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
                $("[id$=PageAction_Entry_Return]").hide();
            }
            else {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
                $("[id$=ddlCompany]").show();
                $("[id$=PageAction_Entry_Return]").show();

                //                if ($('[id$=chkPDC]').is(':checked')) {
                //                    $('[id$=btnReturnDetail]').show();
                //                } else {
                //                    $('[id$=btnReturnDetail]').hide();
                //                }
            }
            if ($("[id$=hdfReceiptReturnHide]").val() == "1" || $("[id$=hdfCurrStatus]").val() == "0" || $("[id$=hdfEditForReturn]").val() == "0") {
                $("[id$=PageAction_Entry_Return]").hide();
            }
            if ($("[id$=hdfIsReceiptReturned]").val() == "1") { //For showing saved retrun details
                $("[id$=PageAction_Entry_Return]").show();
            }
            return false;
        }
        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlDeleteNew]").hide();
                $("[id$=btnSavePaymentSplit]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDeleteNew]").hide();
                //$("[id*=lnkAllocation]").hide();
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
            if (targetControlID == "txtBank") {
                $("[id$=btnAccountNo]").click();
            }
            else if (targetControlID == "txtReceiptCurrency") {
                $("[id$=btnCurrency]").click();
            }
            else if (targetControlID == "txtCustomer") {
                ResetPendingInvNo();
                $("[id$=btnCustomer]").click();
            }
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteSelect(targetControlID);
            }

        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtBank") {
                $("[id$=btnAccountNo]").click();
            }
            else if (targetControlID == "txtReceiptCurrency") {
                $("[id$=btnCurrency]").click();
            }
            else if (targetControlID == "txtCustomer") {
                ResetPendingInvNo();
                $("[id$=btnCustomer]").click();
            }
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteSelect(targetControlID);
            }

        }

        function DisableValidationgroupSaveSavesubmit() {

            var BalReceiveTot = 0;
            var ReceiveNowTot = 0;
            $("#[id*=grdInvoiceList] input[type=text][id*=txtReceivedNow]").each(function (index) {

                if ($(this).closest('tr').find("#[id*=lblBaltoReceive]").html() != undefined) {
                    BalReceiveTot = BalReceiveTot + parseFloat(($(this).closest('tr').find("#[id*=lblBaltoReceive]").html().replace(new RegExp(',', 'g'), '')));
                }
                if ($(this).closest('tr').find("#[id*=txtReceivedNow]").val() != undefined) {
                    ReceiveNowTot = ReceiveNowTot + parseFloat(($(this).closest('tr').find("#[id*=txtReceivedNow]").val().replace(new RegExp(',', 'g'), '')));
                }

            });

            if (BalReceiveTot == 0 && ReceiveNowTot == 0) {

            } else {
                //                Page_ClientValidate('receipt');
                javascript: ValidatePageNow('receipt');
            }
        }

        function CalculateTotal() {

            //var val1 = parseFloat($(sender).val());
            var Amount = 0;
            var taxAmount = 0;
            var AdjAmount = 0;
            var BaltoReceive = 0;
            var DecimalDigits = 0;

            var taxpercentage = 0;
            var basevalue = 0;
            var ttaxamt = 0;
            var hdftax = 0;
            var hdftotalamt = 0;

            var totAmtSO = 0;
            var OAmtSO = 0;
            var totTaxAmtSO = 0;
            var txtOthercharges = 0;

            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }



            //Calc AdjAmount
            $("#[id*=grdInvoiceList] input[type=text][id*=txtAdjustments]").each(function (index) {

                //Check if number is not empty
                if ($.trim($(this).val()) != "") {
                    //Check if number is a valid integer
                    if (!isNaN(parseFloat($(this).val()))) {

                        AdjAmount = AdjAmount + parseFloat($(this).val().replace(new RegExp(',', 'g'), ''));
                        //}
                    }
                }
            });
            $("#[id*=grdInvoiceList] [id*=lblTotalAdjustmentsFooter]").html(addCommas(AdjAmount.toFixed(DecimalDigits)));
            $("#[id*=txtAdjAmount]").val(AdjAmount.toFixed(DecimalDigits));

            var totalOCfooter = 0;
            $("#[id*=grdInvoiceList] input[type=text][id*=txtReceivedNow]").each(function (index) {
                if (isNaN(parseFloat($(this).val()))) {
                    var defNo = 0;
                    parseFloat($(this).val(defNo.toFixed(DecimalDigits)));
                    $(this).attr('title', $(this).val());
                }
                if ($(this).closest('tr').find("#[id*=txtOthercharges]").html() != undefined) {
                    if (isNaN(parseFloat(($(this).closest('tr').find("#[id*=txtOthercharges]").val().replace(new RegExp(',', 'g'), ''))))) {
                        var defNo = 0;
                        parseFloat($(this).closest('tr').find("#[id*=txtOthercharges]").val(defNo.toFixed(DecimalDigits)));
                    }
                }

                if (!isNaN(parseFloat($(this).closest('tr').find('.BaltoReceive').text()))) {
                    var number = Number($(this).closest('tr').find('.BaltoReceive').text().replace(/[^0-9\.]+/g, ""));
                    BaltoReceive = parseFloat(number);
                }
                if ($(this).closest('tr').find("#[id*=hdfTaxAmt]").html() != undefined) {
                    if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=hdfTaxAmt]").val()))) {
                        var number = Number($(this).closest('tr').find("#[id*=hdfTaxAmt]").val().replace(/[^0-9\.]+/g, ""));
                        hdftax = parseFloat(number);
                    }
                }
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=hdfTotalAmt]").val()))) {
                    var number = Number($(this).closest('tr').find("#[id*=hdfTotalAmt]").val().replace(/[^0-9\.]+/g, ""));
                    hdftotalamt = parseFloat(number);
                }
                var Category = $(this).closest('tr').find("#[id*=hdfCategory]").val();
                var group = $(this).closest('tr').find("#[id*=hdfGroup]").val();
                //Check if number is not empty
                if ($.trim($(this).val()) != "") {
                    //Check if number is a valid integer
                    if (!isNaN(parseFloat($(this).val()))) {

                        if (Category == "2" || group == "3" || group == "2" || group == "1") {

                            var TotAmt = ($(this).closest('tr').find("#[id*=lblTotalAmount]").html().replace(new RegExp(',', 'g'), ''));
                            TotAmt = TotAmt.replace(new RegExp(',', 'g'), '');
                            totAmtSO = parseFloat(TotAmt);

                            if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=lblOtherAmount]").html()))) {
                                var othrAmt = ($(this).closest('tr').find("#[id*=lblOtherAmount]").html().replace(new RegExp(',', 'g'), ''));
                                othrAmt = othrAmt.replace(new RegExp(',', 'g'), '');
                                OAmtSO = parseFloat(othrAmt);
                            }

                            if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=lblTotalTaxAmount]").html()))) {
                                var TotTaxAmt = ($(this).closest('tr').find("#[id*=lblTotalTaxAmount]").html().replace(new RegExp(',', 'g'), ''));
                                TotTaxAmt = TotTaxAmt.replace(new RegExp(',', 'g'), '');
                                totTaxAmtSO = parseFloat(TotTaxAmt);
                            }


                            if ($(this).closest('tr').find("#[id*=txtOthercharges]").html() != undefined) {
                                txtOthercharges = parseFloat(($(this).closest('tr').find("#[id*=txtOthercharges]").val().replace(new RegExp(',', 'g'), '')));
                                //                            txtOthercharges = parseFloat($(this).closest('tr').find("#[id*=txtOtherCharges]").val().replace(/[^0-9\.]+/g, ""));
                            }
                            totalOCfooter = totalOCfooter + txtOthercharges;
                            if (totAmtSO != 0 || OAmtSO != 0 || hdftax != 0)
                                taxpercentage = parseFloat((((totAmtSO - OAmtSO) / ((totAmtSO - OAmtSO) - hdftax)) - 1));
                            //                            taxpercentage = parseFloat(taxpercentage.toFixed(DecimalDigits));

                            //taxpercentage = hdftax / (hdftotalamt == 0 ? 1 : hdftotalamt);
                            //Change based on configuration, Done By Juno
                            if ($("[id$=hdfIsTaxForOtherCharge]").val() == "1") {
                                {
                                    tamt = parseFloat($(this).val());
                                    taxpercentage = parseFloat((((totAmtSO) / ((totAmtSO) - hdftax)) - 1));
                                }
                            }
                            else
                                tamt = parseFloat($(this).val()) - txtOthercharges;

                            basevalue = (tamt) / (1 + taxpercentage);
                            ttaxamt = (tamt - basevalue);
                            var ta = addCommas(ttaxamt.toFixed(DecimalDigits).toString());

                            $(this).closest('tr').find("#[id*=lblTax]").text(ta);
                            $(this).closest('tr').find("#[id*=lblTax]").attr("title", ta);
                            $(this).closest('tr').find("#[id*=hdfTotalTax]").val(ta);

                            $("[id$=divTax]").show();
                            $(this).closest("table").find("tr:first th:eq(8)").show();
                            $(this).closest("tr").find("td:eq(8)").show();
                            $(this).closest("table").find("tr:last td:eq(8)").show();
                            taxAmount = taxAmount + parseFloat(ttaxamt.toFixed(DecimalDigits).toString());
                        }
                        else {
                            ttaxamt = hdftax;
                            $("[id$=divTax]").hide();
                            $(this).closest("table").find("tr:first th:eq(8)").hide();
                            $(this).closest("tr").find("td:eq(8)").hide();
                            $(this).closest("table").find("tr:last td:eq(8)").hide();
                        }
                        Amount = Amount + parseFloat($(this).val().replace(new RegExp(',', 'g'), ''));
                        //}

                    }
                }
            });
            $("#[id*=grdInvoiceList] [id*=lblTotalReceivedFooter]").html(addCommas(Amount.toFixed(DecimalDigits)));

            $("#[id*=grdInvoiceList] [id*=lblOtherchargesFooter]").html(addCommas(totalOCfooter.toFixed(DecimalDigits)));
            $("[id$=hdfTotalOtherCharges]").val(totalOCfooter.toFixed(DecimalDigits));


            var exchangeRate = $("[id$=hdfExchangeCurr]").val() == "" ? 1 : $("[id$=hdfExchangeCurr]").val();
            var revAmount = (Amount * exchangeRate).toFixed(DecimalDigits);
            revAmount = revAmount - AdjAmount;
            $("#[id*=txtReceivedAmount]").val(revAmount.toFixed(DecimalDigits));

            $("#[id*=txtTaxAmount]").val((!isNaN(taxAmount) ? taxAmount.toFixed(DecimalDigits) : 0));

            $("#[id*=grdInvoiceList] [id*=lblTotalTaxFooter]").html((!isNaN(taxAmount) ? (addCommas(taxAmount.toFixed(DecimalDigits))) : 0));
            $("[id$=hdfTottaxHDR]").val((!isNaN(taxAmount) ? taxAmount.toFixed(DecimalDigits) : 0));

            //            $("[id$=txtReceivedNow]").attr('title', $("[id$=txtReceivedNow]").val());
        }
        function CalculateTotalAdjn(sender) {

            var DecimalDigits = 0;
            var TotalPayNowFooterSplit = 0;
            var amtAdjnSplit = 0;
            var totalBalAmnt = 0;
            var totalRcvdAmnt = 0;
            var totalAmnt = 0;
            if (!isNaN(parseFloat($("[id$=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("[id$=hdfDecimalDigits]").val());
            }


            $("#[id*=grdReceiptSplitAdjn] input[type=text][id*=txtAllocateAdjn]").each(function (index) {

                if (!isNaN(parseFloat($(this).val()))) {
                    amtAdjnSplit = parseFloat($(this).val().replace(new RegExp(',', 'g'), ''));
                    TotalPayNowFooterSplit = TotalPayNowFooterSplit + amtAdjnSplit;
                }

                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=lblAmountAdjn]").html().replace(new RegExp(',', 'g'), '')))) {
                    var trxAmnt = ($(this).closest('tr').find("#[id*=lblAmountAdjn]").html().replace(new RegExp(',', 'g'), ''));
                    totalAmnt = totalAmnt + parseFloat(trxAmnt);
                }

                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=lblAllocatedAdjn]").html().replace(new RegExp(',', 'g'), '')))) {
                    var trxRcvdAmnt = ($(this).closest('tr').find("#[id*=lblAllocatedAdjn]").html().replace(new RegExp(',', 'g'), ''));
                    totalRcvdAmnt = totalRcvdAmnt + parseFloat(trxRcvdAmnt);
                }

                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=lblBalanceAdjn]").html().replace(new RegExp(',', 'g'), '')))) {
                    var trxBalAmnt = ($(this).closest('tr').find("#[id*=lblBalanceAdjn]").html().replace(new RegExp(',', 'g'), ''));
                    totalBalAmnt = totalBalAmnt + parseFloat(trxBalAmnt);
                }

            });
            $("#[id*=grdReceiptSplitAdjn] [id*=lblTotalAllocateAdjn]").html(addCommas(TotalPayNowFooterSplit.toFixed(DecimalDigits)));
            $("#[id*=grdReceiptSplitAdjn] [id*=hdfTotalAllocateAdjn]").val(TotalPayNowFooterSplit.toFixed(DecimalDigits));

            $("#[id*=grdReceiptSplitAdjn] [id*=lblTotalAmountAdjn]").html(addCommas(totalAmnt.toFixed(DecimalDigits)));
            $("#[id*=grdReceiptSplitAdjn] [id*=lblTotalReceivedAdjn]").html(addCommas(totalRcvdAmnt.toFixed(DecimalDigits)));
            $("#[id*=grdReceiptSplitAdjn] [id*=lblTotalBalanceAdjn]").html(addCommas(totalBalAmnt.toFixed(DecimalDigits)));
            $("#[id*=grdReceiptSplitAdjn] [id*=hdfBalanceAdjn]").val(totalBalAmnt.toFixed(DecimalDigits));
        }


        function CheckSuspenceAmount(sender, args) {
            var ReceivedAmt = $(sender).closest('tr').find('[id*=txtReceivedAmount]').val() != '' ? $(sender).closest('tr').find('[id*=txtReceivedAmount]').val() : 0;
            var SuspenceAmt = $(sender).closest('tr').find('[id*=txtSuspenceAmt]').val() != '' ? $(sender).closest('tr').find('[id*=txtSuspenceAmt]').val() : 0;
            if (parseFloat(SuspenceAmt) > 0 && parseFloat(SuspenceAmt) != parseFloat(ReceivedAmt))
                args.IsValid = false;
            else
                args.IsValid = true;
        }
        function CalculateAdjnFooter(sender) {


            var DecimalDigits = 0;
            var TotaladjnFooter = 0;
            var amtAdjnSplit = 0;

            var TotalRecFooter = 0;
            var amtRecSplit = 0;

            var TotalBalRecFooter = 0;
            var amtBalRecSplit = 0;

            if (!isNaN(parseFloat($("[id$=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("[id$=hdfDecimalDigits]").val());
            }

            $("#[id*=grdInvoiceList] input[type=text][id*=txtAdjustments]").each(function (index) {

                if (!isNaN(parseFloat($(this).closest('tr').find('[id*=lblAdjAmount]').html())))
                    amtAdjnSplit = parseFloat($(this).closest('tr').find('[id*=lblAdjAmount]').html().replace(new RegExp(',', 'g'), ''));

                TotaladjnFooter = TotaladjnFooter + amtAdjnSplit;

                if (!isNaN(parseFloat($(this).closest('tr').find('[id*=txtReceivedNow]').val())))
                    amtRecSplit = parseFloat($(this).closest('tr').find('[id*=txtReceivedNow]').val().replace(new RegExp(',', 'g'), ''));

                TotalRecFooter = TotalRecFooter + amtRecSplit;

                var jhh = parseFloat($(this).closest('tr').find('[id*=lblBaltoReceive]').html());
                if (!isNaN(parseFloat($(this).closest('tr').find('[id*=lblBaltoReceive]').html())))
                    amtBalRecSplit = parseFloat($(this).closest('tr').find('[id*=lblBaltoReceive]').html().replace(new RegExp(',', 'g'), ''));

                TotalBalRecFooter = TotalBalRecFooter + amtBalRecSplit;

            });
            if (TotaladjnFooter > 0)
                $("#[id*=grdInvoiceList] [id*=lblAdjAmountFooter]").html(addCommas(TotaladjnFooter.toFixed(DecimalDigits)));
            else
                $("#[id*=grdInvoiceList] [id*=lblAdjAmountFooter]").html("0.00");

            if (TotalRecFooter > 0)
                $("#[id*=grdInvoiceList] [id*=lblTotalReceivedFooter]").html(addCommas(TotalRecFooter.toFixed(DecimalDigits)));
            else
                $("#[id*=grdInvoiceList] [id*=lblTotalReceivedFooter]").html("0.00");

            if (TotalBalRecFooter > 0)
                $("#[id*=grdInvoiceList] [id*=lblBaltoReceiveFooter]").html(addCommas(TotalBalRecFooter.toFixed(DecimalDigits)));
            else
                $("#[id*=grdInvoiceList] [id*=lblBaltoReceiveFooter]").html("0.00");
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
            var poTaxFoot = 0;
            var poOthercharge = 0;
            var PoCharge = 0;

            if (!isNaN(parseFloat($("[id$=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("[id$=hdfDecimalDigits]").val());
            }
            if (!isNaN(parseFloat($("[id$=hdfTaxPer]").val()))) {
                taxPer = parseFloat($("[id$=hdfTaxPer]").val());
            }
            if (!isNaN(parseFloat($("[id$=hdfOtherPer]").val()))) {
                otherChargePer = parseFloat($("[id$=hdfOtherPer]").val());
            }
            var TotalPayNowFooterSplit = 0;
            $("#[id*=grdReceiptSplit] input[type=text][id*=txtPayNowSplit]").each(function (index) {
                if (isNaN(parseFloat($(this).closest('tr').find('.BalancetoPay').text()))) {
                    parseFloat($(this).val("0".toFixed(CurrencyDigits)));
                }
                if (isNaN(parseFloat($(this).val()))) {
                    var defNo = 0;
                    parseFloat($(this).val(defNo.toFixed(CurrencyDigits)));
                }
                if (!isNaN(parseFloat($(this).closest('tr').find('.BalancetoPay').text()))) {
                    var number = Number($(this).closest('tr').find('.BalancetoPay').text().replace(/[^0-9\.]+/g, ""));
                    BalancetoPay = parseFloat(number);

                }
                //Other Charges
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=txtOtherChargesSplit]").val()))) {
                    var number = Number($(this).closest('tr').find("#[id*=txtOtherChargesSplit]").val().replace(/[^0-9\.]+/g, ""));
                    otherCharges = parseFloat(number);
                    TotalOtherCharges = TotalOtherCharges + otherCharges;
                }

                //Total Amount
                tempTotal = parseFloat($(this).closest('tr').find('[id*=lblAmountSplit]').html().replace(/[^0-9\.]+/g, ""));
                //Total Discount
                tempDisc = parseFloat($(this).closest('tr').find('[id*=lblDiscountSplit]').html().replace(/[^0-9\.]+/g, ""));
                tempTax = parseFloat($(this).closest('tr').find('[id*=lblTaxSplit]').html().replace(/[^0-9\.]+/g, ""));

                tempInvAmt = parseFloat($(this).val()) - otherCharges;
                TotalPayNowFooterSplit = TotalPayNowFooterSplit + parseFloat($(this).val());
                //Check if number is not empty
                if ($.trim($(this).val()) != "") {
                    //Check if number is a valid integer
                    if (!isNaN(parseFloat($(this).val()))) {
                        $(this).parent("td").find('input[type=hidden][id$=hdfPayNowSplit]').val($(this).val());
                        Amount = Amount + parseFloat($(this).val());
                        var tempResult = 0;
                        poTax = parseFloat($(this).val()) * taxPer;
                        poTaxFoot = poTaxFoot + poTax;

                        //  poOthercharge = parseFloat($(this).val()) * otherChargePer; 
                        var otherChargeSplitPercent = 1;
                        if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=hdfShippingrChargesSplitPercent]").val()))) {

                            otherChargeSplitPercent = Number($(this).closest('tr').find("#[id*=hdfShippingrChargesSplitPercent]").val().replace(/[^0-9\.]+/g, ""));
                        }
                        poOthercharge = parseFloat($(this).val()) * otherChargePer;
                        //poOthercharge = parseFloat($(this).val()) * otherChargeSplitPercent;
                        $(this).closest('tr').find("#[id*=lblTotalTaxSplit]").text(poTax.toFixed(CurrencyDigits));
                        $("#[id*=lblTotalTaxSplit]").attr("title", (poTax).toFixed(CurrencyDigits));
                        $(this).closest('tr').find("#[id*=hdfTaxSplit]").val(poTax.toFixed(CurrencyDigits));
                        $(this).closest('tr').find("#[id*=txtOtherChargesSplit]").val(poOthercharge.toFixed(CurrencyDigits));
                        $(this).closest('tr').find("#[id*=hdfOtherChargesSplit]").val(poOthercharge.toFixed(CurrencyDigits));
                        Tax = Tax + poTax;
                        PoCharge = PoCharge + poOthercharge;
                    }
                }
            });
            Tax = Tax < 0 ? 0 : Tax;
            $("#[id*=grdReceiptSplit] [id*=lblTotalPayNowFooterSplit]").html(addCommas(TotalPayNowFooterSplit.toFixed(DecimalDigits)));
            $("#[id*=grdReceiptSplit] [id*=hdfTotalPayNowFooterSplit]").val(TotalPayNowFooterSplit.toFixed(DecimalDigits));
            $("#[id*=grdReceiptSplit] [id*=hdfTotalOtherChargesFooterSplit]").val(PoCharge);
            $("#[id*=grdReceiptSplit] [id*=hdfTotalOtherChargesSplit]").val(PoCharge);
            $("#[id*=grdReceiptSplit] [id*=lblTotalOtherChargesFooterSplit]").html(addCommas(PoCharge.toFixed(DecimalDigits)));
            $("#[id*=grdReceiptSplit] [id*=lblTotalTaxFooterSplit]").html(addCommas(poTaxFoot.toFixed(DecimalDigits)));

            //$("#[id*=grdInvoiceList] input[type=text][id*=lblTotalPayNowFooter]").val(Amount);
            var exchangeRate = $("[id$=hdfExchangeCurr]").val() == "" ? 1 : $("[id$=hdfExchangeCurr]").val();


        }


        function CalculateTotalSplitOLD(sender) {
            var val1 = parseFloat($(sender).val());
            var Amount = 0;
            var BalancetoPay = 0;
            var DecimalDigits = 0;


            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }

            $("#[id*=grdReceiptSplit] input[type=text][id*=txtPayNowSplit]").each(function (index) {
                if (!isNaN(parseFloat($(this).closest('tr').find('.BalancetoPay').text()))) {
                    var number = Number($(this).closest('tr').find('.BalancetoPay').text().replace(/[^0-9\.]+/g, ""));
                    BalancetoPay = parseFloat(number);

                }
                //Check if number is not empty
                if ($.trim($(this).val()) != "") {
                    //Check if number is a valid integer
                    if (!isNaN(parseFloat($(this).val()))) {

                        $(this).parent("td").find('input[type=hidden][id$=hdfPayNowSplit]').val($(this).val());
                        Amount = Amount + parseFloat($(this).val());
                    }
                }
            });
            $("#[id*=grdReceiptSplit] [id*=lblTotalPayNowFooterSplit]").html(addCommas(Amount.toFixed(DecimalDigits)));
            $("#[id*=grdReceiptSplit] [id*=hdfTotalPayNowFooterSplit]").val(Amount);
            //$("#[id*=grdInvoiceList] input[type=text][id*=lblTotalPayNowFooter]").val(Amount);
            var exchangeRate = $("[id$=hdfExchangeCurr]").val() == "" ? 1 : $("[id$=hdfExchangeCurr]").val();


        }

        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                CheckValidationDuplicate(valGroup);
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").html("");
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;

            }
        }

        function CalculateSuspenceGridTotal() {
            var DecimalDigits = parseFloat($("[id$=hdfDecimalDigits]").val());
            var TotalAmt = 0;
            $("#[id*=grdSuspenceList] input[type=checkbox][id*=chkItem]").each(function (index) {
                var Amt = 0;
                var chkSelected = $(this).closest('tr').find("#[id*=chkItem]");
                if (chkSelected.is(":checked")) {
                    Amt = $(this).closest('tr').find("#[id*=lblAmount]").html().replace(new RegExp(',', 'g'), '');
                    TotalAmt = parseFloat(TotalAmt) + parseFloat(Amt);
                }
            });

            $("#[id*=grdSuspenceList] [id*=lblTotalSuspenceAmount]").html(addCommas(parseFloat(TotalAmt).toFixed(DecimalDigits)));
        }

        function CheckBankCharge(oSrc, args) {
            if (($("table[id*=grdInvoiceList]").find("TR").length - 1) > 0) {
                var value1 = $('input:text[id$=txtReceivedAmount]').val();
                var value2 = $('input:text[id$=txtBankCharge]').val();
                var exchangeRate = $("[id$=hdfExchRate]").val() == "" ? 1 : $("[id$=hdfExchRate]").val();
                var BC = $("[id$=hdfReceiptCurrency]").val();
                if (value1 != "" && value2 != "") {
                    var EBankCharge = 0;
                    if ($("[id$='ddlBankChargeCurrency']").val() != BC) {
                        EBankCharge = parseFloat(value1) * parseFloat(exchangeRate);
                    }
                    else {
                        EBankCharge = parseFloat(value1);
                    }
                    if (EBankCharge <= parseFloat(value2)) {
                        args.IsValid = false;
                    }
                    else {
                        args.IsValid = true;
                    }
                }
                else {
                    args.IsValid = true;
                }
            } else {
                args.IsValid = true;
            }
        }

        ////

        function ValidateReceiveNow(sender, args) {

            var receivedNow = $(sender).closest('tr').find('[id*=txtReceivedNow]').val();
            var pattern = new RegExp($(sender).closest('tr').find('[id*=vcmReceiveNow]')[0].validationexpression);
            var receivedNowAmt = parseFloat(receivedNow);
            if (!pattern.test(receivedNow) || isNaN(receivedNowAmt) || receivedNowAmt == 0) {
                args.IsValid = false;
            }
            else {
                args.IsValid = true;
            }
        }
        function MessageRemoveSplit(sender) {
            //if ($(sender).closest('tr').find('[id*=hdfHasSplit]').val() == "1") {
            //hdfGroup = 3 : miscellaneous invoice
            if ($(sender).closest('tr').find('[id*=hdfGroup]').val() != "3") {
                ShowDeleteConfirm($(sender).closest('tr').find('[id*=btnReceiveNow]')[0], '<%=GetLocalResourceObject("Msg_Split_Delete").ToString() %>');
            }

        }
        function AfterDeleteConfirmationCancel(controlID) {
            var isPayNow = false;
            var Amount = parseFloat("0");
            var DecimalDigits = 0;
            if (!isNaN(parseFloat($("[id$=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("[id$=hdfDecimalDigits]").val());
            }

            $("#[id*=grdInvoiceList] [id*=btnReceiveNow]").each(function () {
                if ($(this).attr('id') == controlID) {
                    isPayNow = true;
                    $(this).closest('tr').find('[id*=txtReceivedNow]').val($(this).closest('tr').find('[id*=hdfReceivedNowPrev]').val());
                }
                Amount = Amount + parseFloat($(this).closest('tr').find('[id*=txtReceivedNow]').val());

            });
            if (isPayNow) {
                $("[id$=grdInvoiceList] [id*=lblTotalReceivedFooter]").html(addCommas(Amount.toFixed(DecimalDigits)));
                CloseMsgPopup();
            }
            else {
                if (controlID == "") {
                }
            }
        }
        function SetPayNowPrev(sender) {
            $(sender).closest('tr').find('[id*=hdfReceivedNowPrev]').val($(sender).val());
        }
        function CloseMsgPopup() {
            //            $('html').css('overflow', 'auto');
            //            $('body').css('overflow', 'visible');
            //            if ($('#divmodel').length > 0) {
            //                $('#divmodel').hide();
            //            }
            //$(".ui-widget-overlay:visible").hide();
            $(".ui-widget-overlay:visible").each(function () {
                if ($(this).parent().attr("id") != "updateProgress") {
                    $(this).hide();
                }
            });
        }




        function ShowSaveWithoutBankChargeConfirm(btn) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%=GetLocalResourceObject("Msg_Save_Without_BankCharge").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $("[id$=hdfSaveWithoutBankCharge]").val("1");
                        $(this).dialog("close");
                        //__doPostBack(btn.name, '');
                        $("[id$=" + btn + "]").click();
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        //CalculateTotal();
                        return false;
                    }
                }
            });
            return false;
        }

        //receipt allocation  shows validation if give amount in allocate now more than balance amount.
        function CheckReceiptAllocation(sender, args) {
            var split = $(sender).closest('tr').find('[id*=txtAllocateAdjn]').val();
            var pattern = new RegExp($(sender).closest('tr').find('[id*=vreDedAllocateNowSplit]')[0].validationexpression);
            var spliAmount = parseFloat(split);
            if (pattern.test(split) && !isNaN(spliAmount)) {
                var bal = 0;
                var allocate = 0;
                if (!isNaN(parseFloat($(sender).closest('tr').find('[id*=lblBalanceAdjn]').html()))) {
                    var number = Number($(sender).closest('tr').find('[id*=lblBalanceAdjn]').html().replace(/[^0-9\.]+/g, ""));
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

        function CalculateTotalDebitSplit() {
            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }
            var TotalCrdrReceivenow = 0;
            var TotCrdrAdj = 0;

            $("#[id*=grdCrdrAllocation] input[type=text][id*=txtCrdrReceiveNow]").each(function (index) {
                var CrdrReceivenow = 0;
                var CrdrAdj = 0;

                if (!isNaN(parseFloat($(this).val()))) {
                    CrdrReceivenow = parseFloat($(this).val());
                    TotalCrdrReceivenow += CrdrReceivenow;
                }
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=txtCrdrAdjAmount]").val().replace(/[^0-9\.]+/g, "")))) {
                    CrdrAdj = parseFloat($(this).closest('tr').find("#[id*=txtCrdrAdjAmount]").val().replace(/[^0-9\.]+/g, ""));
                    TotCrdrAdj += CrdrAdj;
                }
            });
            $("#[id*=grdCrdrAllocation] [id*=lblCrdrReceiveNowFooter]").html(addCommas(TotalCrdrReceivenow.toFixed(DecimalDigits)));
            $("#[id*=grdCrdrAllocation] [id*=hdfCrdrPayNowFooter]").val(TotalCrdrReceivenow.toFixed(DecimalDigits));
            $("#[id*=grdCrdrAllocation] [id*=lblCrdrAdjAmountFooter]").html(addCommas(TotCrdrAdj.toFixed(DecimalDigits)));
            $("#[id*=grdCrdrAllocation] [id*=hdfCrdrAdjAmountFooter]").val(TotCrdrAdj.toFixed(DecimalDigits));
        }


        function ValidateDebitSplit(sender, args) {
            var CrReceiveNowAmt = 0;
            var CrAdjAmt = 0;
            var CrBalanceAmt = 0;
            var CrReceiveNow = $(sender).closest('tr').find('[id*=txtCrdrReceiveNow]').val();
            var CrAdj = $(sender).closest('tr').find('[id*=txtCrdrAdjAmount]').val();
            var CrBalance = $(sender).closest('tr').find('[id*=lblCrdrBalanceAmount]').html();
            if (!isNaN(parseFloat(CrReceiveNow.replace(new RegExp(',', 'g'), '')))) {
                CrReceiveNowAmt = parseFloat(CrReceiveNow.replace(new RegExp(',', 'g'), ''));
            }
            if (!isNaN(parseFloat(CrAdj.replace(new RegExp(',', 'g'), '')))) {
                CrAdjAmt = parseFloat(CrAdj.replace(new RegExp(',', 'g'), ''));
            }
            if (!isNaN(parseFloat(CrBalance.replace(new RegExp(',', 'g'), '')))) {
                CrBalanceAmt = parseFloat(CrBalance.replace(new RegExp(',', 'g'), ''));
            }
            if (CrBalanceAmt < (CrReceiveNowAmt + CrAdjAmt)) {
                args.IsValid = false;
            }
            else {
                args.IsValid = true;
            }
        }

        function CalTotalRecdAmtSplitUp(sender) {
            var TotalAmountSplit = 0;
            $("#[id*=grdReceivedAmtSplitup] input[type=hidden][id*=hdfAmountSplit]").each(function (index) {
                TotalAmountSplit = TotalAmountSplit + parseFloat($(this).val());
            });
            $("#[id*=grdReceivedAmtSplitup] [id*=lblTotalAmountSplit]").html(TotalAmountSplit.toFixed(CurrencyDigits));

        }

        function ReceiptReturnChecked() {
            if ($("[id$='chkReturn']").is(':checked') == true) {
                $("[id$='txtReturnDate']").attr("disabled", false);
                $("[id$='txtReturnDate']").removeClass("input-disabled");
                if ($("[id$='rfvReturnDate']")[0] != undefined) {
                    ValidatorEnable($("[id$='rfvReturnDate']")[0], true);
                }
                $("[id$='txtReturnRemarks']").attr("disabled", false);
                $("[id$='txtReturnRemarks']").removeClass("input-disabled");
            }
            else {
                $("[id$='txtReturnDate']").val("");
                $("[id$='txtReturnDate']").attr("disabled", true);
                $("[id$='txtReturnDate']").addClass("input-disabled");
                if ($("[id$='rfvReturnDate']")[0] != undefined) {
                    ValidatorEnable($("[id$='rfvReturnDate']")[0], false);
                }
                $("[id$='txtReturnRemarks']").val("");
                $("[id$='txtReturnRemarks']").attr("disabled", true);
                $("[id$='txtReturnRemarks']").addClass("input-disabled");
            }
        }



        function ShowHidePendingInv(flag) {
            ///<summary>
            /// Used to Show/Hide ItemDetails Div
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

        $("[id*=chkInvselect]").live("click", function () {
            var grid = $(this).closest("table");
            var chkHeader = $("[id*=chkSelectAllInv]", grid);
            if (!$(this).is(":checked")) {
                $("td", $(this).closest("tr")).removeClass("selected");
                chkHeader.removeAttr("checked");
            } else {
                $("td", $(this).closest("tr")).addClass("selected");
                if ($("[id*=chkInvselect]", grid).length == $("[id*=chkInvselect]:checked", grid).length) {
                    chkHeader.attr("checked", "checked");
                }
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlSalesReceipt">
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
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="100"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="101" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="DisableValidationgroupSaveSavesubmit()"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="102"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="DisableValidationgroupSaveSavesubmit()"
                                            ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="103" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="DisableValidationgroupSaveSavesubmit()"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDeleteNew">
                                        <asp:Button runat="server" ID="btnDeleteNew" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="104" OnClientClick="return ShowDeleteConfirm(this);"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" ToolTip="<%$resources:Controls,Delete %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="105" ID="btnEntryPrint" CommandName="PRINT"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="106" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnJournalize" CommandName="JOURNALIZE" TabIndex="107"
                                            Text="<%$resources:Journalize %>" OnClick="ActionHandler" ToolTip="<%$resources:Journalize %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-journalize" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="108" ID="btnReturnDetail" CommandName="CHEQUERETURN"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Return %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-journalize" ToolTip="<%$resources:Controls,Return %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="109" ID="btnReverseDetail" CommandName="REVERSE"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Reverse %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-journalize" ToolTip="<%$resources:Controls,Reverse %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="110" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="111" ID="btnReturn" CommandName="CHEQUERETURN"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Return %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-journalize" ToolTip="<%$resources:Controls,Return %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="112" ID="btnReverse" CommandName="REVERSE" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Reverse %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-journalize"
                                            ToolTip="<%$resources:Controls,Reverse %>" />
                                    </li>
                                    <li style="display: none">
                                        <%--ReturnReceipt--%>
                                        <asp:Button runat="server" TabIndex="113" ID="btnEditForReturn" CommandName="EDITFORRETURN"
                                            OnClick="ActionHandler" Text="<%$resources:ReturnSR %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-return" ToolTip="<%$resources:ReturnSR %>" />
                                    </li>
                                    <li id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="114" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelSR %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelSR %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="115" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="116" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="117" ID="btnPrint" CommandName="PRINTLISTING"
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
                                OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="RECEIPTLIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="RECEIPTDETAIL"
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
                                            <asp:TextBox runat="server" ID="txtSearchDateFrom" CssClass="input-small" TabIndex="100"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfSearchDateFrom" runat="server" />
                                            <asp:Label runat="server" ID="lblSearchDateTo" Text="<%$ resources:ToDate %>" AssociatedControlID="txtSearchDateTo"
                                                CssClass="middle-lbl-a"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSearchDateTo" CssClass="input-small" TabIndex="101"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfSearchDateTo" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblPDCstatus" Text="<%$ resources:PDCStatus%>" AssociatedControlID="ddlPDCStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlPDCStatus" runat="server" CssClass="select-small-b" TabIndex="102">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Reversed %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,NotReversed %>" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="lblSIno" runat="server" Text="<%$resources:InvNo %>" AssociatedControlID="txtSINo"
                                                CssClass="middle-lbl-xsmall-b"></asp:Label>
                                            <asp:TextBox ID="txtSINo" runat="server" CssClass="medium" MaxLength="100" TabIndex="103"> </asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblCustomerSearch" runat="server" Text="<%$resources:Customer %>"
                                                AssociatedControlID="txtCustomerSearch"></asp:Label>
                                            <asp:TextBox ID="txtCustomerSearch" runat="server" CssClass="select-half margnbotm0"
                                                MaxLength="100" TabIndex="104"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerSearchPk" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblReceiptNumber" runat="server" Text="<%$resources:ReceiptNumber %>"
                                                AssociatedControlID="txtReceiptNumber"></asp:Label>
                                            <asp:TextBox ID="txtReceiptNumber" runat="server" CssClass="medium margnbotm0" MaxLength="100"
                                                TabIndex="105"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfReceiptPK" runat="server" Value="" />
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" AssociatedControlID="ddlStatus"
                                                CssClass="middle-lbl-xsmall-b"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-b margnbotm0"
                                                TabIndex="106">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,NotPosted %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Posted %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="btnSearchHdr" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="107" CommandName="SEARCH" SkinID="search-ext"
                                                CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="108" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext"
                                                CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdReceiptList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                    OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" TabIndex="109" runat="server" GroupName="SelectOne"
                                                    ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" AutoPostBack="true"
                                                    OnCheckedChanged="ActionHandler" />
                                                <asp:HiddenField runat="server" ID="hdfReceiptID" Value='<%# Eval("RCH_PK") %>' />
                                                <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval("RCH_DEPT") %>' />
                                                <asp:HiddenField ID="hdfDelStatus" runat="server" Value='<%# Eval("RCH_DEL_STATUS") %>' />
                                                <asp:HiddenField ID="hdfReceiptStatus" runat="server" Value='<%# Eval("RCH_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfSalesInvoiceCategory" Value='<%# Eval("RCH_CATEGORY") %>' />
                                                <asp:HiddenField runat="server" ID="hdfReturnStatus" Value='<%# Eval("RCH_RETURN_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfCustomerPK" Value='<%# Eval("RCH_CUSTOMER") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ReceiptDate %>" SortExpression="<%$ resources:DataFieldRes,ReceiptDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReceiptDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.ReceiptDate, Resources.Constants.DateFormatGrid).ToString()  %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.ReceiptDate, Resources.Constants.DateFormatGrid).ToString() %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ReceiptNo %>" SortExpression="<%$ resources:DataFieldRes,ReceiptNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTransactionId" runat="server" Text='<%# Eval(Resources.DataFieldRes.ReceiptNo) ==""?"[NEW]":Eval(Resources.DataFieldRes.ReceiptNo) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.ReceiptNo)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblPlantRcptList" runat="server" CssClass="<%# Eval(Resources.DataFieldRes.CompnayLineColor) %>"
                                                    Text='<%# Eval(Resources.DataFieldRes.CMP_DISPLAY_CODE) %>' ToolTip='<%# Eval(Resources.DataFieldRes.CMP_DISPLAY_CODE)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Customer %>" SortExpression="<%$ resources:DataFieldRes,ReceiptCustomerName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString( Eval("RCH_CUSTOMER_TEXT"),30) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString( Eval("RCH_CUSTOMER_TEXT"))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="23%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="btnReceiptReturnFlag" runat="server" Visible='<%# Convert.ToInt32(Eval("RCH_RETURN_STATUS")) == 1? true : false %>'
                                                    OnClientClick="javascript:return false;" CssClass='<%# "receiptreturn-icon" %>'
                                                    ToolTip='<%# GetLocalResourceObject("ReceiptReturned").ToString() %>' />
                                                <asp:Button ID="btnReturnFlag" runat="server" Visible='<%# (Convert.ToInt32(Eval("RCH_MODE")) == 2 && Convert.ToInt32(Eval("RCH_BOUNCED")) == 1) ? true : false %>'
                                                    OnClientClick="javascript:return false;" CssClass='<%# "return-icon" %>' ToolTip='<%# GetLocalResourceObject("ChequeReturned").ToString() %>' />
                                                <asp:Button ID="btnPDCFlag" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# (Convert.ToInt32(Eval("RCH_PDC")) == 1 || Convert.ToInt32(Eval("RCH_PDC")) == 3) ? "flaggrey-icon" : "flaggreen-icon"%>'
                                                    Visible='<%# Convert.ToInt32(Eval("RCH_PDC")) != 0? true : false %>' ToolTip='<%# (Convert.ToInt32(Eval("RCH_PDC")) == 1 || Convert.ToInt32(Eval("RCH_PDC")) == 3) ? GetLocalResourceObject("PDC_Cheque").ToString() : GetLocalResourceObject("Cheque_Reversed").ToString() %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvNo %>">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkInvnos" runat="server" CssClass="text-underline" OnClick="ActionHandler"
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetEncodedString(Eval("RCH_INV_NO")) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("RCH_INV_NO"),15) %>'
                                                    Style="text-align: left!important;" CommandName="PRINTINVOICE"></asp:LinkButton>
                                                <asp:HiddenField runat="server" ID="hdfInvNos" Value='<%# Eval("RCH_INV_NO") %>' />
                                                <asp:HiddenField runat="server" ID="hdfListinvPK" Value='<%# Eval("ICH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfListInvType" Value='<%# Eval("ICH_TYPE") %>' />
                                                <asp:HiddenField runat="server" ID="hdfListinvCategory" Value='<%# Eval("ICH_CATEGORY") %>' />
                                                <asp:HiddenField runat="server" ID="hdfListGroup" Value='<%# Eval("ICH_GROUP") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Mode %>" SortExpression="<%$ resources:DataFieldRes,ReceiptMode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblModeofReceipt" runat="server" Text='<%# Eval("RCH_MODE_TEXT")%>'
                                                    ToolTip='<%# Eval("RCH_MODE_TEXT")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BankName %>" SortExpression="<%$ resources:DataFieldRes,ReceiptBankName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBankName" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString( Eval("RCH_BANK_TEXT"),23) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("RCH_BANK_TEXT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="16%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Cur %>" SortExpression="<%$ resources:DataFieldRes,ReceiptCurrencyCode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("RCH_CURRENCY_TEXT")) %>'
                                                    ToolTip='<%#  ERP.Utilities.CommonFunctions.GetDecodedString(Eval("RCH_CURRENCY_TEXT")) %>'></asp:Label>
                                                <itemstyle width="3%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %>" SortExpression="<%$ resources:DataFieldRes,ReceivedAmount %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("RCH_RCVD_AMOUNT")) %>'
                                                    ToolTip='<%#  GetFormattedCurrencyWithComma(Eval("RCH_RCVD_AMOUNT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("ASC_CSS_CLASS") %>' ToolTip='<%# Eval("RCH_STATUS_TEXT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval(Resources.DataFieldRes.ReceiptApproved) %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgPosted" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? GetLocalResourceObject("unposted").ToString() : Eval("FTH_CSS_CLASS")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? Resources.Captions.NotPosted : Eval("FTH_STATUS_TEXT")%>'
                                                    Visible='<%# (Convert.ToDecimal(Eval(Resources.DataFieldRes.ReceivedAmount)) > 0 ? true : false) %>' />
                                                <asp:HiddenField runat="server" ID="hdfPosted" Value='<%# Eval(Resources.DataFieldRes.ReceiptPosted) %>' />
                                                <asp:HiddenField runat="server" ID="hdfPDC" Value='<%# Eval("RCH_PDC") %>' />
                                                <asp:HiddenField runat="server" ID="hdfMode" Value='<%# Eval("RCH_MODE") %>' />
                                                <asp:HiddenField runat="server" ID="hdfRcptStatus" Value='<%# Eval("RCH_GROUP") %>' />
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
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="Label1" runat="server" Text="<%$ resources:ReceiptNo%>" AssociatedControlID="lblReceiptNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblReceiptNo" CssClass="input-small"></asp:Label>
                                            <asp:HiddenField ID="hdfReceiptNo" runat="server" />
                                            <asp:Label runat="server" ID="lblReceiptDate1" Text="<%$ resources:ReceiptDate%>"
                                                AssociatedControlID="txtReceiptDate" CssClass="middle-lbl-c"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtReceiptDate" CssClass="input-small" TabIndex="1"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfReceiptDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="receipt" EnableClientScript="true" runat="server" ControlToValidate="txtReceiptDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Receipt_Date %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdnReceiptDate" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCust" runat="server" Text="<%$ resources:Customer%>" AssociatedControlID="txtCustomer"></asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="select-half margnbotm0" MaxLength="100"
                                                TabIndex="2"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerPk" runat="server" />
                                            <asp:Button ID="btnCustomer" runat="server" EnableTheming="false" Style="display: none"
                                                OnClick="ActionHandler" CommandName="CUSTOMERCHANGE" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                <h1>
                                    <%= GetLocalResourceObject("PendingInv").ToString() %></h1>
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
                                                <asp:TextBox ID="txtPendingFromDate" runat="server" TabIndex="3" Width="100px" MaxLength="13"
                                                    onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                                <asp:HiddenField ID="hdfPendingFromDate" runat="server" Value="" />
                                                <asp:Label ID="lblPndngToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtPendingToDate"
                                                    CssClass="margn-lft5"></asp:Label>
                                                <asp:TextBox ID="txtPendingToDate" runat="server" TabIndex="4" Width="100px" MaxLength="14"
                                                    onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                                <asp:HiddenField ID="hdfPendingToDate" runat="server" Value="" />
                                                <asp:Label ID="lblPndngInvCategory" runat="server" Text="<%$ resources:InvCategory %>"
                                                    CssClass="margn-lft5" AssociatedControlID="ddlPendingInvCategory"></asp:Label>
                                                <asp:DropDownList ID="ddlPendingInvCategory" runat="server" TabIndex="5" Width="150px"
                                                    onchange="ResetPendingInvNo();InitPendingInvNo();">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblPndngInvType" runat="server" Text="<%$resources:InvoiceType %>"
                                                    AssociatedControlID="ddlPendingInvType" CssClass="margn-lft5"></asp:Label>
                                                <asp:DropDownList ID="ddlPendingInvType" runat="server" TabIndex="6" Width="100px"
                                                    onchange="ResetPendingInvNo();InitPendingInvNo();">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblSoNo" runat="server" Text="<%$ resources:InvNo %>" CssClass="lbl-6perc margn-lft5"
                                                    AssociatedControlID="txtPendingInvNumber"></asp:Label>
                                                <div id="divSearchDtls">
                                                    <asp:TextBox ID="txtPendingInvNumber" runat="server" TabIndex="7"> </asp:TextBox>
                                                    <asp:HiddenField ID="hdfPendingInvPk" runat="server" Value="" />
                                                </div>
                                                <asp:ImageButton ID="imbDtlSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                    ValidationGroup="Search" OnClick="ActionHandler" TabIndex="8" CommandName="DTLSEARCH"
                                                    SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                                <asp:ImageButton ID="imbDtlClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                    TabIndex="9" OnClick="ActionHandler" CommandName="DTLCLEARSEARCH" SkinID="clear-ext"
                                                    CssClass="margntop2 margnbotm0" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                            <%--<table class="table-devide">
                                                <tr>
                                                    <td>
                                                        <div class="div2col-S">
                                                            <asp:Label ID="lblPndngFrmDate" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtPendingFromDate"></asp:Label>
                                                            <asp:TextBox ID="txtPendingFromDate" runat="server" TabIndex="1" CssClass="input-small"
                                                                MaxLength="13" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                                            <asp:HiddenField ID="hdfPendingFromDate" runat="server" Value="" />
                                                            <asp:Label ID="lblPndngToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtPendingToDate"
                                                                CssClass="middle-lbl-b"></asp:Label>
                                                            <asp:TextBox ID="txtPendingToDate" runat="server" TabIndex="2" CssClass="input-small"
                                                                MaxLength="14" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                                            <asp:HiddenField ID="hdfPendingToDate" runat="server" Value="" />
                                                            <div class="clear">
                                                            </div>
                                                            <asp:Label ID="lblSoNo" runat="server" Text="<%$ resources:InvNo %>" AssociatedControlID="txtPendingInvNumber"></asp:Label>
                                                            <asp:TextBox ID="txtPendingInvNumber" runat="server" TabIndex="2" CssClass="input-small-c"> </asp:TextBox>
                                                            <asp:HiddenField ID="hdfPendingInvPk" runat="server" Value="" />
                                                            <asp:ImageButton ID="imbDtlSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                                ValidationGroup="Search" OnClick="ActionHandler" TabIndex="3" CommandName="DTLSEARCH"
                                                                SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                                            <asp:ImageButton ID="imbDtlClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                                TabIndex="4" OnClick="ActionHandler" CommandName="DTLCLEARSEARCH" SkinID="clear-ext"
                                                                CssClass="margntop2 margnbotm0" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="div2col-S">
                                                            <asp:Label runat="server" ID="lblPndngInvCategory" Text="<%$ resources:InvCategory%>"
                                                                AssociatedControlID="ddlPendingInvCategory"></asp:Label>
                                                            <asp:DropDownList ID="ddlPendingInvCategory" runat="server" TabIndex="22" CssClass="select-small-d margnbotm0">                                                             
                                                            </asp:DropDownList>
                                                            <asp:Label ID="lblPndngInvType" runat="server" Text="<%$resources:InvoiceType %>"
                                                                AssociatedControlID="ddlPendingInvType" CssClass="middle-lbl-xsmall-a4 margnbotm0"></asp:Label>
                                                            <asp:DropDownList ID="ddlPendingInvType" runat="server" TabIndex="22" CssClass="select-small-b margnbotm0">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>--%>
                                            <asp:GridView runat="server" ID="grdPendingInvList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                                AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkSelectAllInv" runat="server" ToolTip="Select All" TabIndex="5" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" ID="chkInvselect" TabIndex="10" />
                                                            <asp:HiddenField runat="server" ID="hdfInvoicePk" Value='<%# Eval("ICH_PK") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfInvCustomerPK" Value='<%# Eval("ICH_CUSTOMER") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfInvType" Value='<%# Eval("ICH_TYPE") %>' />
                                                            <asp:HiddenField ID="hdfInvCurrency" runat="server" Value='<%# Eval("ICH_CURRENCY") %>' />
                                                            <asp:HiddenField ID="hdfPndInvCategory" runat="server" Value='<%# Eval("ICH_CATEGORY") %>' />
                                                            <asp:HiddenField ID="hdfPndInvGroup" runat="server" Value='<%# Eval("ICH_GROUP") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="2%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:InvDate %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval( Resources.DataFieldRes.SalesInvoiceDate)!=""? Convert.ToDateTime( Eval( Resources.DataFieldRes.SalesInvoiceDate)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                                ToolTip='<%# Eval(Resources.DataFieldRes.SalesInvoiceDate, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="6%" Wrap="false" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:InvNo %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLInvoiceNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.SalesInvoiceNo) ==""?"[NEW]":Eval(Resources.DataFieldRes.SalesInvoiceNo)%>'
                                                                ToolTip='<%# Eval(Resources.DataFieldRes.SalesInvoiceNo)%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="8%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPlant" runat="server" CssClass="<%# Eval(Resources.DataFieldRes.CompnayLineColor) %>"
                                                                Text='<%# Eval(Resources.DataFieldRes.CMP_DISPLAY_CODE) %>' ToolTip='<%# Eval(Resources.DataFieldRes.CMP_DISPLAY_CODE)%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="2%" Wrap="false" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:InvoiceType %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLInvoiceType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SIType),3,"") %>'
                                                                ToolTip='<%# Eval(Resources.DataFieldRes.SIType)%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:Customer %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCustomer" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SICustomer),30) %>'
                                                                ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SICustomer),250) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="24%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:SONo %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSoNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.SISONo) ==""?"[NEW]": ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SISONo),25) %>'
                                                                ToolTip='<%# Eval(Resources.DataFieldRes.SISONo)%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:SODate %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSoDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SISOdate, Resources.Constants.DateFormatGrid) %>'
                                                                ToolTip='<%# Eval(Resources.DataFieldRes.SISOdate, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="6%" Wrap="false" />
                                                    </asp:TemplateField>
                                                    <%-- <asp:TemplateField HeaderText="<%$ resources:ETD %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInvoiceETD" runat="server" Text='<%# Eval(Resources.DataFieldRes.ICH_ETD, Resources.Constants.DateFormatGrid) %>'
                                                                ToolTip='<%# Eval(Resources.DataFieldRes.ICH_ETD, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="6%" Wrap="false" />
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="<%$ resources:Currency %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCurrency" runat="server" Text='<%#Eval(Resources.DataFieldRes.SICurrency)  %>'
                                                                ToolTip='<%#Eval(Resources.DataFieldRes.SICurrency)  %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="2%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:TrxAmount %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInvoiceValue" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval(Resources.DataFieldRes.SIValue)) %>'
                                                                ToolTip='<%# GetFormattedCurrencyWithComma(Eval(Resources.DataFieldRes.SIValue)) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="11%" CssClass="amount-numeric" />
                                                        <HeaderStyle CssClass="amount-numeric" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:BalAmt %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBalAmt" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval(Resources.DataFieldRes.SalesBalAmt)) %>'
                                                                ToolTip='<%# GetFormattedCurrencyWithComma(Eval(Resources.DataFieldRes.SalesBalAmt)) %>'></asp:Label>
                                                            <%--<asp:LinkButton ID="lbnBalAmt" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval(Resources.DataFieldRes.SalesBalAmt))%>'
                                                                CssClass="text-underline" ToolTip='<%# GetFormattedCurrencyWithComma(Eval(Resources.DataFieldRes.SalesBalAmt))%>'
                                                                OnClick="ActionHandler" CommandName="AMOUNTDETAILS"></asp:LinkButton>--%>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="12%" CssClass="amount-numeric" />
                                                        <HeaderStyle CssClass="amount-numeric" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:DueDate %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDueDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SalesDueDate, Resources.Constants.DateFormatGrid) %>'
                                                                ToolTip='<%# Eval(Resources.DataFieldRes.SalesDueDate, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Wrap="false" />
                                                        <ItemStyle Width="6%" Wrap="false" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <div class="button-wrap-right margntop-minus3 margnbotm10">
                                            <asp:Button ID="btnAddSelectedItems" runat="server" ToolTip="<%$resources:Controls,AddToList %>"
                                                Text="<%$ Resources:Controls, AddToList%>" OnClick="ActionHandler" CommandName="ADDTOLIST"
                                                TabIndex="11" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <%--grid-w930--%>
                                <asp:HiddenField ID="hdfExchRate" runat="server" />
                                <asp:HiddenField ID="hdfIsTaxForOtherCharge" runat="server" Value="0" />
                                <%-- <asp:HiddenField ID="hdfCustomerPK" runat="server" />--%>
                                <asp:HiddenField ID="hdfCustomerAccountNo" runat="server" />
                                <asp:HiddenField ID="hdfInvoiceCurr" runat="server" />
                                <asp:HiddenField ID="hdfExchangeCurr" runat="server" />
                                <asp:HiddenField ID="hdfExchangeCurrBC" runat="server" />
                                <asp:HiddenField ID="hdfRecStatus" runat="server" Value="0" />
                                <asp:HiddenField ID="hdfRecWKFStatus" runat="server" Value="0" />
                                <asp:HiddenField ID="hdfShowPDC" runat="server" Value="0" />
                                <asp:HiddenField ID="hdfPDCStatusWKF" runat="server" Value="0" />
                                <asp:HiddenField ID="hdfShowChequeReturn" runat="server" Value="1" />
                                <asp:HiddenField ID="hdfDelStatus" runat="server" Value="0" />
                                <asp:HiddenField ID="hdfJournalName" runat="server" />
                                <asp:HiddenField ID="hdfSaveWithoutBankCharge" runat="server" />
                                <asp:GridView ID="grdInvoiceList" runat="server" AutoGenerateColumns="False" PageSize="<%$ resources:PageSize %>"
                                    AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                    ShowFooter="true" Width="100%">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceNo %>" SortExpression="<%$ resources:DataFieldRes,SalesInvoiceNo %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfCategory" runat="server" Value='<%#Eval("ICH_CATEGORY") %>' />
                                                <asp:HiddenField ID="hdfGroup" runat="server" Value='<%#Eval("ICH_GROUP") %>' />
                                                <asp:HiddenField ID="hdfInvType" runat="server" Value='<%#Eval("ICH_TYPE") %>' />
                                                <asp:HiddenField ID="hdfIsApply" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdfTotalAmt" runat="server" Value='<%#Eval("ICH_TOTAL_AMOUNT") %>' />
                                                <asp:HiddenField ID="hdfTaxAmt" runat="server" Value='<%#Eval("ICH_TAX_TC") %>' />
                                                <asp:HiddenField ID="hdfInvoicePK" runat="server" Value='<%#Eval("RCM_INVOICE_HDR") %>' />
                                                <asp:HiddenField ID="hdfReceiptMpgPK" runat="server" Value='<%#Eval("RCM_PK") %>' />
                                                <asp:LinkButton ID="lnkInvoiceNo" runat="server" CssClass="text-underline" OnClick="ActionHandler"
                                                    CommandName="SHOWPOPUP" Text='<%#Eval("ICH_NO") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle />
                                            <FooterStyle HorizontalAlign="left" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblfooter" Text="<%$ resources:Total %>"></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCmpDisplayCode" runat="server" CssClass='<%# Eval("CMP_LINE_COLOUR") %>'
                                                    Text='<%# Eval("CMP_DISPLAY_CODE") %>' ToolTip='<%# Eval("CMP_DISPLAY_CODE")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TotalAmount %>" SortExpression="<%$ resources:DataFieldRes,SalesInvoiceNetAmount %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalAmount" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("ICH_AMOUNT_NET_TC")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalAmountFooter"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Tax %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalTaxAmount" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("ICH_TAX_TC")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTaxAmountFooter"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SOothers %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOtherAmount" Text='<%# GetFormattedCurrencyWithComma(Eval("ICH_SHIP_CHARGE")) %>'
                                                    runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblOtherAmountFooter"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:RecvdAmt %>" SortExpression="<%$ resources:DataFieldRes,SalesInvoiceReceivedAmount %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReceived" runat="server" Visible="false" Text='<%# GetFormattedCurrencyWithComma(Eval("ICH_AMOUNT_RCVD_TC")) %>'></asp:Label>
                                                <asp:LinkButton ID="lnkReceived" runat="server" CssClass="text-underline" OnClick="ActionHandler"
                                                    CommandName="RECEIVEDAMTSPLITUP" Text='<%# GetFormattedCurrencyWithComma(Eval("ICH_AMOUNT_RCVD_TC")) %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblReceivedFooter"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Alloc1 %>" SortExpression="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAdjAmount" Text='<%# GetFormattedCurrencyWithComma(Eval("RCM_ADJUST_AMOUNT")) %>'
                                                    runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblAdjAmountFooter" Text="0.00"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkAllocationAdjn" runat="server" OnClick="ActionHandler" CommandName="ADJNINVOICEDETAIL"
                                                    SkinID="adjustallocation-icon" ToolTip="Allocation" CommandArgument="PageAction_Entry"
                                                    TabIndex="2" /><%--OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load" --%>
                                            </ItemTemplate>
                                            <ItemStyle Width="1.5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BaltoReceive %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBaltoReceive" CssClass="BaltoReceive" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("ICH_BAL_AMOUNT")) %>'></asp:Label>
                                                <asp:HiddenField ID="hdfBaltoReceive" runat="server" Value='<%# GetFormattedCurrency(Eval("ICH_BAL_AMOUNT")) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblBaltoReceiveFooter"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ReceiveNow %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtReceivedNow" runat="server" CssClass="input-w70 numeric" MaxLength="15"
                                                    Text='<%# GetFormattedCurrency(Eval("RCM_RCVD_AMOUNT")) %>' TabIndex="2" onkeyup="CalculateTotal();"
                                                    onfocus="SetPayNowPrev(this);" onchange="MessageRemoveSplit(this);" Width="90%"></asp:TextBox>
                                                <asp:Button runat="server" ID="btnReceiveNow" OnClick="ActionHandler" CommandName="CHANGERECEIVENOW"
                                                    EnableTheming="false" Style="display: none;" />
                                                <asp:HiddenField ID="hdfReceivedNow" runat="server" />
                                                <asp:HiddenField ID="hdfReceivedNowPrev" runat="server" />
                                                <asp:HiddenField ID="hdfHasSplit" runat="server" Value="0" />
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="vrfReceivedNow" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="receipt" EnableClientScript="true" runat="server" ControlToValidate="txtReceivedNow"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ReceivedAmount1 %>">
                                                    </asp:RequiredFieldValidator>
                                                    <cc1:AmountValidation ID="vamReceivedNow" runat="server" ControlToValidate="txtReceivedNow"
                                                        ErrorMessage="<%$ resources:Err_ReceivedAmount %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="receipt"></cc1:AmountValidation>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Center" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalReceivedFooter" class="input-w70 numeric"></asp:Label></FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkAllocation" runat="server" OnClick="ActionHandler" CommandName="INVOICEDETAIL"
                                                    SkinID="allocation-icon" ToolTip="Allocation" CommandArgument="PageAction_Entry"
                                                    TabIndex="2" OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load" ValidationGroup="split"
                                                    OnClientClick="javascript:ValidatePageNow('split')" />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CrdrAlcnAmt %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCrdrAlcnAmount" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("RCM_DN_AMOUNT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <ItemStyle Wrap="false" Width="6%" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="btnCrdrAllocation" runat="server" OnClick="ActionHandler" CommandName="CRDRALLOCATION"
                                                    TabIndex="2" SkinID="debitnote-icon" ToolTip="<%$ resources:DebitAllocation %>"
                                                    CommandArgument="PageAction_Entry" />
                                            </ItemTemplate>
                                            <ItemStyle Width="1.5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SOotherChrg %>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtOthercharges" ToolTip="Non taxable" Text='<%# GetFormattedCurrency(Eval("RCM_OTHER_AMOUNT")) %>'
                                                    runat="server" CssClass="input-w70 numeric" onkeyup="CalculateTotal();" MaxLength="15"
                                                    TabIndex="3"></asp:TextBox>
                                                <asp:HiddenField ID="hdfOtherchargeOLD" Value='<%# GetFormattedCurrency(Eval("RCM_PREV_OTHER_AMOUNT")) %>'
                                                    runat="server" />
                                                <cc1:AmountValidation ID="vreOthercharges" runat="server" ControlToValidate="txtOthercharges"
                                                    ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"></cc1:AmountValidation>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" Width="5%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblOtherchargesFooter"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Tax %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTax" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("RCM_TAX_AMOUNT")) %>'></asp:Label>
                                                <asp:HiddenField ID="hdfTotalTax" runat="server" Value='<%# GetFormattedCurrency(Eval("RCM_TAX_AMOUNT")) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalTaxFooter"></asp:Label></FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Adjustments %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAdjustments" runat="server" CssClass="input-w70 numeric" MaxLength="15"
                                                    Text='<%# GetFormattedCurrency(Eval("RCM_DISC_AMOUNT")) %>' TabIndex="4" onkeyup="CalculateTotal();"
                                                    OnTextChanged="ActionHandler" AutoPostBack="true"></asp:TextBox>
                                                <asp:HiddenField ID="hdfAdjustments" runat="server" Value='<%# GetFormattedCurrency(Eval("RCM_DISC_AMOUNT")) %>' />
                                                <cc1:AmountValidation ID="vamAdjustments" runat="server" ControlToValidate="txtAdjustments"
                                                    ErrorMessage="<%$ resources:Err_InvalidAdjustments %>" NumberDigits="11" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="receipt"></cc1:AmountValidation>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Center" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalAdjustmentsFooter" CssClass="input-w70 numeric"></asp:Label></FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" SkinID="delete-icon"
                                                    TabIndex="12" CommandName="REMOVE" ToolTip="Remove" OnClientClick="return ShowDeleteConfirm(this);" />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <table class="table-devide" id="tblDetailHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblReceiptCurrency" runat="server" Text="<%$resources:Currency %>"
                                                AssociatedControlID="txtReceiptCurrency"></asp:Label>
                                            <asp:TextBox ID="txtReceiptCurrency" runat="server" Enabled="false" MaxLength="3"
                                                TabIndex="13" CssClass="Uidate-picker input-disabled"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfReceiptCurrency" runat="server" Value="" />
                                            <asp:Button ID="btnCurrency" runat="server" OnClick="ActionHandler" CommandName="EXCHANGERATE"
                                                EnableTheming="false" Style="display: none" />
                                            <asp:Label runat="server" ID="lblMode" Text="<%$ resources:Mode%>" AssociatedControlID="ddlMode"
                                                CssClass="lbl-25-5perc"></asp:Label>
                                            <asp:DropDownList ID="ddlMode" runat="server" AutoPostBack="true" CssClass="select-small-b"
                                                TabIndex="14" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfMode" CssClass="star" SetFocusOnError="true" ValidationGroup="receipt"
                                                EnableClientScript="true" InitialValue="-1" runat="server" ControlToValidate="ddlMode"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Mode %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblBranch" Text="<%$ resources:Branch%>" AssociatedControlID="txtBranch"></asp:Label>
                                            <asp:TextBox ID="txtBranch" runat="server" TabIndex="16" Enabled="false" CssClass="input-half input-disabled"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfBranch" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="receipt" EnableClientScript="true" runat="server" ControlToValidate="txtBranch"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Branch%>"></asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblInstrumentNo" Text="<%$ resources:InstrumentNo_Mand%>"
                                                AssociatedControlID="txtInstrumentNo"></asp:Label>
                                            <asp:TextBox ID="txtInstrumentNo" runat="server" TabIndex="18" onkeydown="limitText(this,90);"
                                                onkeyup="limitText(this,90);" onblur="limitText(this,90);" CssClass="input-small"></asp:TextBox>
                                            <div style="width: 1.6%; display: inline-block;">
                                                <asp:RequiredFieldValidator ID="vrfInstrumentNo" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="receipt" EnableClientScript="true" runat="server" ControlToValidate="txtInstrumentNo"
                                                    Display="Static" Text="*" ErrorMessage="<%$ resources:Err_InstrumentNo%>" Enabled="false"></asp:RequiredFieldValidator></div>
                                            <asp:Label runat="server" ID="lblInstrumentDate" Text="<%$ resources:InstrumentDate%>"
                                                AssociatedControlID="txtInstrumentDate" CssClass="middle-lbl-a0"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInstrumentDate" CssClass="input-small margn-rgt0"
                                                TabIndex="19" onpaste="return false;" onkeydown="return CheckKey(event)"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfInstrumentDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="receipt" EnableClientScript="true" runat="server" ControlToValidate="txtInstrumentDate"
                                                Display="Static" Text="*" Enabled="false" ErrorMessage="<%$ resources:Err_InstrumentDate %>">
                                            </asp:RequiredFieldValidator>
                                            <input type="checkbox" id="chkPDC" runat="server" tabindex="9" /><label id="lblPDC"
                                                class="middle-lbl-xsmall-i margn-rgt0" runat="server" style="text-align: left;"><%=GetLocalResourceObject("PDC") %></label>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblBank" Text="<%$ resources:BankName %>" AssociatedControlID="txtBank"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtBank" TabIndex="15" CssClass="input-half"></asp:TextBox>
                                            <asp:ImageButton ID="imbSuspencelist" runat="server" OnClick="ActionHandler" CommandName="SHOWACCOUNT"
                                                SkinID="view-batches" ToolTip="<%$ resources:SuspenceList %>" CommandArgument="PageAction_Entry"
                                                TabIndex="7" Visible="false" />
                                            <asp:HiddenField ID="hdfBank" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfBankName" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="receipt" EnableClientScript="true" runat="server" ControlToValidate="txtBank"
                                                Display="Dynamic" Text="*" InitialValue="<%$ resources:Messages, AutoDefaultValue %>"
                                                ErrorMessage="<%$ resources:Err_BankName %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblAccountNo" Text="<%$ resources:AccNo%>" AssociatedControlID="txtAccountNo"></asp:Label>
                                            <asp:TextBox ID="txtAccountNo" runat="server" CssClass="input-half input-disabled"
                                                TabIndex="17" Enabled="false">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfBankAccount" runat="server" />
                                            <asp:Button ID="btnAccountNo" runat="server" Text="<%$ resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="3" CommandName="SEARCHACCOUNTNO" SkinID="btnInner-search"
                                                EnableTheming="false" Style="display: none" />
                                            <asp:RequiredFieldValidator ID="vrfAccountNo" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="receipt" EnableClientScript="true" runat="server" ControlToValidate="txtAccountNo"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_AccNo%>"></asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblAdjType" Text="<%$ resources:AdjType%>" AssociatedControlID="ddlAdjType"></asp:Label>
                                            <asp:DropDownList ID="ddlAdjType" runat="server" CssClass="select-small-b" TabIndex="20">
                                            </asp:DropDownList>
                                            <%--<asp:RequiredFieldValidator ID="vrfAdjType" CssClass="star" SetFocusOnError="true" InitialValue ="-1"
                                                ValidationGroup="receipt" EnableClientScript="true" runat="server" ControlToValidate="ddlAdjType"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_AdjType%>"></asp:RequiredFieldValidator>--%>
                                            <asp:Label runat="server" ID="lblAdjAmount" Text="<%$ resources:AdjustAmount%>" CssClass="middle-lbl-a"
                                                AssociatedControlID="txtAdjAmount"></asp:Label>
                                            <asp:TextBox ID="txtAdjAmount" runat="server" MaxLength="17" Enabled="false" CssClass=" numeric input-disabled input-small margn-rgt0"
                                                TabIndex="21"></asp:TextBox>
                                            <div style="width: 1.6%; display: inline-block;">
                                                <asp:RequiredFieldValidator ID="vrfAdjAmount" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="receipt" EnableClientScript="true" runat="server" ControlToValidate="txtAdjAmount"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Adjustments%>"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <%--  <asp:Label runat="server" ID="lblExchangeRate" Text="<%$ resources:ExchangeRate%>"
                                                AssociatedControlID="txtExchangeRate"></asp:Label>
                                            <asp:TextBox ID="txtExchangeRate" runat="server" TabIndex="19" MaxLength="12"
                                                CssClass="numeric medium" onkeyup="CalculateTotalBC();" ></asp:TextBox>
                                            <cc1:ExchangeRateValidation ID="vreExchangeRate" ControlToValidate="txtExchangeRate"
                                                ErrorMessage="<%$ resources:Err_ExchangeRate%>" NumberDigits="5" Display="Dynamic"
                                                Text="*" EnableClientScript="true" CssClass="star" SetFocusOnError="true" ValidationGroup="receipt"
                                                runat="server" NonZero="true"></cc1:ExchangeRateValidation>
                                                <div class="clear"></div>--%>
                                            <asp:Label runat="server" ID="lblBankCurrency" Text="<%$ resources:BankCurrency%>"
                                                AssociatedControlID="ddlBankChargeCurrency"></asp:Label>
                                            <asp:DropDownList ID="ddlBankChargeCurrency" CssClass="select-small-g margnrgt1-5per"
                                                TabIndex="22" runat="server">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtBankCharge" runat="server" TabIndex="23" MaxLength="17" CssClass="input-small Uiinput-amount numeric"></asp:TextBox>
                                            <cc1:AmountValidation ID="vamBankCharge" runat="server" ControlToValidate="txtBankCharge"
                                                ErrorMessage="<%$ resources:Err_BankCharge %>" NumberDigits="11" Display="Dynamic"
                                                Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="receipt"></cc1:AmountValidation>
                                            <asp:CustomValidator ID="csvBankCharge" runat="server" Display="Dynamic" CssClass="star"
                                                SetFocusOnError="true" Text="*" ControlToValidate="txtBankCharge" EnableClientScript="true"
                                                ClientValidationFunction="CheckBankCharge" ErrorMessage="<%$ resources:Err_ValidBankCharge %> "
                                                ValidationGroup="receipt"></asp:CustomValidator>
                                            <div class="clear">
                                            </div>
                                            <div id="divTax" runat="server">
                                                <asp:Label runat="server" ID="lblTaxAmount" Text="<%$ resources:TaxAmount%>" AssociatedControlID="txtTaxAmount"
                                                    CssClass="margn-rgt0"></asp:Label>
                                                <asp:TextBox ID="txtTaxAmount" runat="server" MaxLength="17" Enabled="false" CssClass="medium numeric input-disabled"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfTaxAmount" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="receipt" EnableClientScript="true" runat="server" ControlToValidate="txtTaxAmount"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TaxAmount%>"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <%-- <asp:Label runat="server" ID="lblTotalAmountBC" Text="<%$ resources:TotalAmountBC%>"
                                                AssociatedControlID="txtTotalAmountBC"></asp:Label>
                                            <asp:TextBox ID="txtTotalAmountBC" runat="server" TabIndex="19" MaxLength="17" Enabled="false"
                                                CssClass="Uiinput-amount numeric input-disabled"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfTotalAmountBC" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="receipt" EnableClientScript="true" runat="server" ControlToValidate="txtTotalAmountBC"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TotalAmountBC%>"></asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>--%>
                                            <asp:Label runat="server" ID="lblBankCheque" Text="<%$ resources:BankOfCheque%>"
                                                AssociatedControlID="txtbankOfCheque"></asp:Label>
                                            <asp:TextBox ID="txtbankOfCheque" runat="server" TabIndex="24" onkeydown="limitText(this,190);"
                                                onkeyup="limitText(this,190);" onblur="limitText(this,190);" CssClass="input-half">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfBankOfCheque" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="receipt" EnableClientScript="true" runat="server" ControlToValidate="txtbankOfCheque"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_BankOfCheque%>" Enabled="false"></asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblReceivedAmount" Text="<%$ resources:ReceivedAmount%>"
                                                AssociatedControlID="txtReceivedAmount"></asp:Label>
                                            <asp:TextBox ID="txtReceivedAmount" runat="server" TabIndex="19" MaxLength="17" Enabled="false"
                                                CssClass="input-small Uiinput-amount numeric input-disabled"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfReceivedAmount" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="receipt" EnableClientScript="true" runat="server" ControlToValidate="txtReceivedAmount"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ReceiveAmount%>"></asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblSuspAmt" Text="<%$ resources:SuspenceAmount%>" AssociatedControlID="txtSuspenceAmt"
                                                CssClass="middle-lbl-c"></asp:Label>
                                            <asp:TextBox ID="txtSuspenceAmt" runat="server" TabIndex="19" MaxLength="17" Enabled="false"
                                                CssClass="input-small Uiinput-amount numeric input-disabled"></asp:TextBox>
                                            <asp:CustomValidator ID="customSuspAmt" runat="server" ValidateEmptyText="true" ClientValidationFunction="CheckSuspenceAmount"
                                                ErrorMessage="<%$ resources:Msg_NotEqual_SuspenceAmount %>" Text="*" EnableClientScript="true"
                                                ControlToValidate="txtSuspenceAmt" CssClass="star" Display="Dynamic" ValidationGroup="receipt"></asp:CustomValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks %>" AssociatedControlID="txtRemarks"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRemarks" TabIndex="25" TextMode="MultiLine" CssClass="multiline-2line"
                                                onkeydown="limitText(this,400);" onkeyup="limitText(this,400);" onblur="limitText(this,400);"></asp:TextBox>
                                            <%-- <asp:RequiredFieldValidator ID="vrfRemarks" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="receipt" EnableClientScript="true" runat="server" ControlToValidate="txtRemarks"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Remarks%>">
                                            </asp:RequiredFieldValidator>--%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                    <%--Return area--%>
                    <asp:TableRow ID="PageAction_Entry_Return" runat="server" Style="display: none">
                        <asp:TableCell>
                            <h3>
                                <%= GetLocalResourceObject("ReturnDetails").ToString() + " :"%></h3>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblReturn" Text="<%$ resources:Return%>" AssociatedControlID="chkReturn"></asp:Label>
                                            <asp:CheckBox ID="chkReturn" runat="server" TabIndex="8" onclick="ReceiptReturnChecked();" />
                                            <asp:Label runat="server" ID="lblReturnDate" Text="<%$ resources:ReturnDate%>" CssClass="lbl-38-3perc"
                                                AssociatedControlID="txtReturnDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtReturnDate" CssClass="input-small" TabIndex="2"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdnReturnDate" runat="server" />
                                            <asp:RequiredFieldValidator ID="rfvReturnDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="receipt" EnableClientScript="true" runat="server" ControlToValidate="txtReturnDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ReturnDate %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblReturnRemarks" Text="<%$ resources:ReturnRemarks%>"
                                                AssociatedControlID="txtReturnRemarks"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtReturnRemarks" TabIndex="8" Width="356px" TextMode="MultiLine"
                                                CssClass="multiline-1col"></asp:TextBox>
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
                <%--SO Split UP Popup Start--%>
                <div id="divSoSplitUp" style="display: none">
                    <div class="content-wrapper">
                        <asp:Panel runat="server" ID="Allocation_Section" CssClass="Button-container-popup">
                            <%--<div id="Allocation_Section" runat="server" class="Button-container-popup">--%>
                            <asp:Button ID="btnSavePaymentSplit" runat="server" Text="<%$ resources:Controls,Apply %>"
                                ToolTip="<%$ resources:Controls,Apply %>" OnClick="ActionHandler" CommandName="PAYMENTSPLITSAVE"
                                SkinID="btnInner-add-dsd" CommandArgument="Allocation_Section" />
                            <%--</div>--%>
                        </asp:Panel>
                        <div class="detail-co3">
                            <div class="div3col-S">
                                <asp:Label ID="LabelInv" runat="server" Text="<%$resources:InvoiceNo1 %>" AssociatedControlID="lblInvSplitNo"
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblInvSplitNo" runat="server" CssClass="medium"></asp:Label>
                                <asp:Label ID="Label6" runat="server" Text="<%$resources:Amount1 %>" AssociatedControlID="lblInvSplitAmount"
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblInvSplitAmount" runat="server" CssClass="medium"></asp:Label>
                            </div>
                            <div class="div3col-S">
                                <asp:Label ID="Label2" runat="server" Text="<%$resources:Date1 %>" AssociatedControlID="lblInvSplitDate"
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblInvSplitDate" runat="server" CssClass="medium"></asp:Label>
                                <asp:Label ID="Label8" runat="server" Text="<%$resources:Received1 %>" AssociatedControlID="lblInvSplitReceived"
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblInvSplitReceived" runat="server" CssClass="medium"></asp:Label>
                            </div>
                            <div class="div3col-S">
                                <asp:Label ID="Label4" runat="server" Text="<%$resources:Customer1 %>" AssociatedControlID="lblInvSplitSupplier"
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblInvSplitSupplier" runat="server" CssClass="medium"></asp:Label>
                                <asp:Label ID="Label10" runat="server" Text="<%$resources:ReceiveNow1 %>" AssociatedControlID="lblInvSplitReceiveNow"
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblInvSplitReceiveNow" runat="server" CssClass="medium"></asp:Label>
                            </div>
                            <div class="clear">
                            </div>
                        </div>
                        <div class="error" id="divErrorLabel" runat="server" visible="false">
                            <ul>
                                <li>
                                    <asp:Literal runat="server" ID="lblSplitErrorMessage" Text="<%$resources:error_allocation %>"></asp:Literal>
                                </li>
                            </ul>
                        </div>
                        <div class="gridwrap">
                            <asp:TableCell>
                                <div class="gridwrap">
                                    <asp:GridView ID="grdReceiptSplit" runat="server" AutoGenerateColumns="False" Width="100%"
                                        PageSize="<%$ resources:PageSize %>" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable"
                                        AllowSorting="false" ShowFooter="true">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$resources:SONO %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPONOSplit" runat="server" Text='<%# Eval("SOH_NO") %>'></asp:Label>
                                                    <asp:HiddenField ID="hdfReceiptSplitPK" runat="server" Value='<%# Eval("RSO_PK") %>' />
                                                    <asp:HiddenField ID="hdfSOPK" runat="server" Value='<%# Eval("RSO_SO_HDR") %>' />
                                                    <asp:HiddenField ID="hdfReceiptTRXPK" runat="server" Value='<%# Eval("RSO_RECEIPT_TRX_MPG") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="25%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:SODate %>" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPODateSplit" runat="server" Text='<%# Eval(Resources.DataFieldRes.SaleOrderHDate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:CurrencyGrdh %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCurrSplit" runat="server" Text='<%# Eval("SOH_CURRENCY_TEXT") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:Amount %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmountSplit" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("SOH_NET_AMOUNT")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Tax %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxSplit" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("SOH_TOTAL_TAX")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Discount %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDiscountSplit" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("SOH_TOTAL_DISCOUNT")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:Received %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaidSplit" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("SOH_AMT_RECEIVED")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:Balance %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBalanceSplit" CssClass="BalancetoPay" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("SOH_BALANCE")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <%--Pay Now--%>
                                            <asp:TemplateField HeaderText="<%$ resources:AllocateNow %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPayNowSplit" runat="server" CssClass="Uiinput-amount numeric"
                                                        MaxLength="15" onkeyup="CalculateTotalSplit(this);" Text='<%# GetFormattedCurrency(Eval("RSO_RECEIVED_AMOUNT")) %>'>
                                                    </asp:TextBox>
                                                    <asp:HiddenField ID="hdfPayNowSplit" runat="server" Value='<%# GetFormattedCurrency(Eval("RSO_RECEIVED_AMOUNT")) %>' />
                                                    <asp:HiddenField ID="hdfShippingrChargesSplitPercent" runat="server" Value='<%# GetFormattedCurrency( Convert.ToDecimal(Eval("SOH_TOTAL_SHIP_CHARGE"))/Convert.ToDecimal(Eval("SOH_NET_AMOUNT"))) %>' />
                                                    <cc1:AmountValidation ID="vamPayNowSplit" runat="server" ControlToValidate="txtPayNowSplit"
                                                        ErrorMessage="<%$ resources:Err_ReceivedAmount %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="receipt"></cc1:AmountValidation>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" CssClass="amount-numeric" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="center" />
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalPayNowFooterSplit" CssClass="input-w70 numeric"></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfTotalPayNowFooterSplit" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:OtherCharges %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtOtherChargesSplit" runat="server" CssClass="Uiinput-amount numeric"
                                                        Enabled="false" MaxLength="16" onkeyup="CalculateTotalSplit(this);" Text='<%# GetFormattedCurrency(Eval("RSO_OTHER_AMOUNT")) %>'>
                                                    </asp:TextBox>
                                                    <asp:HiddenField ID="hdfOtherChargesSplit" runat="server" Value='<%# GetFormattedCurrency(Eval("RSO_OTHER_AMOUNT")) %>' />
                                                    <asp:HiddenField ID="hdfTaxSplit" Value='<%# GetFormattedCurrency(Eval("RSO_TAX_AMOUNT")) %>'
                                                        runat="server" />
                                                    <asp:RequiredFieldValidator ID="vrfOtherChargesSplit" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="payment" EnableClientScript="true" runat="server" ControlToValidate="txtOtherChargesSplit"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PaymentAmount1 %>">
                                                    </asp:RequiredFieldValidator>
                                                    <cc1:AmountValidation ID="vreOtherChargesSplit" runat="server" ControlToValidate="txtOtherChargesSplit"
                                                        ErrorMessage="<%$ resources:Err_PaymentAmount %>" NumberDigits="12" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="payment"></cc1:AmountValidation>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalOtherChargesFooterSplit"></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfTotalOtherChargesFooterSplit" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Tax %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalTaxSplit" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("RSO_TAX_AMOUNT")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="4%" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalTaxFooterSplit"></asp:Label></FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:TableCell>
                        </div>
                    </div>
                </div>
                <%--SO Split UP Popup End--%>
                <%--------------------------Suspence List Pop Up Start----------------------%>
                <div id="divSuspenceList" style="display: none">
                    <div class="content-wrapper">
                        <div class="Button-container-popup">
                            <asp:Button ID="btnListApply" runat="server" Text="<%$ resources:Controls,Apply %>"
                                OnClick="ActionHandler" CommandName="APPLY" SkinID="btnInner-add-dsd" />
                        </div>
                        <div class="gridwrap" id="divList">
                            <asp:GridView runat="server" ID="grdSuspenceList" Width="100%" AllowSorting="false"
                                AutoGenerateColumns="false" TabIndex="107" EmptyDataRowStyle-CssClass="emptytable"
                                OnRowDataBound="ActionHandler" ShowFooter="true">
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkItem" Checked='<%# Convert.ToInt32(Eval("FTH_SUSP_FLAG")) != 0? true:false %>'
                                                onclick="CalculateSuspenceGridTotal();" />
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSDate" runat="server" Text='<%# Eval("FTH_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("FTH_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                ToolTip='<%# Eval("FTH_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("FTH_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'></asp:Label>
                                            <asp:HiddenField runat="server" ID="hdfFTHPK" Value='<%# Eval("FTH_PK") %>' />
                                            <asp:HiddenField runat="server" ID="hdfEntryPK" Value='<%# Eval("FTH_RSC_PK") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:VoucherNo %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVoucherNo" runat="server" Text='<%# Eval("FTH_VOUCHER_NO")  %>'
                                                ToolTip='<%# Eval("FTH_VOUCHER_NO")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:RefNo %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRefNo" runat="server" Text='<%# Eval("FTH_REF_NO")  %>' ToolTip='<%# Eval("FTH_REF_NO")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:RefDate %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRefDate" runat="server" Text='<%# Eval("FTH_REF_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("FTH_REF_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                ToolTip='<%# Eval("FTH_REF_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("FTH_REF_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Narration %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNarration" runat="server" Text='<%# Eval("FTH_NARRATION")  %>'
                                                ToolTip='<%# Eval("FTH_NARRATION")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:TrxAmount %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("FTH_AMOUNT")) %>'
                                                ToolTip='<%#GetFormattedCurrency(Eval("FTH_AMOUNT")) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="30%" HorizontalAlign="Right" />
                                        <HeaderStyle CssClass="amount-numeric" />
                                        <FooterStyle HorizontalAlign="Right" />
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalSuspenceAmount" runat="server" Text=""></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <%--------------------------Suspence List Pop Up End------------------------%>
                <%--AmtAdj Popup Start--%>
                <div id="divSoSplitUpAdjn" style="display: none">
                    <div class="content-wrapper">
                        <asp:Panel runat="server" ID="Allocation_SectionAdjn" CssClass="Button-container-popup">
                            <asp:Button ID="btnSaveSplitAdjn" runat="server" Text="<%$ resources:Controls,Apply %>"
                                ValidationGroup="SplitAdjn" OnClientClick="javascript:ValidatePageNow('SplitAdjn')"
                                ToolTip="<%$ resources:Controls,Apply %>" OnClick="ActionHandler" CommandName="ADJNSPLITSAVE"
                                SkinID="btnInner-add-dsd" CommandArgument="Allocation_SectionAdjn" />
                            <%--</div>--%>
                        </asp:Panel>
                        <div class="detail-co3">
                            <div class="detail-co3-2">
                                <asp:Label ID="lblcustomer" runat="server" Text="<%$resources:Customer1 %>" AssociatedControlID="lblInvSplitSupplier"
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblCusname" CssClass="w-74" runat="server"></asp:Label>
                                <div class="clear">
                                </div>
                                <asp:Label ID="lblCurr" runat="server" Text="<%$resources:Currency1 %>" AssociatedControlID="lblInvSplitReceived"
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
                        <div class="error" id="divErrorLabelAdjnTotamt" runat="server" visible="false">
                            <ul>
                                <li>
                                    <asp:Literal runat="server" ID="Literal2" Text="<%$resources:error_allocationAdjnTotAmt %>"></asp:Literal>
                                </li>
                            </ul>
                        </div>
                        <div class="error" id="divBaltoAll" runat="server" visible="false">
                            <ul>
                                <li>
                                    <asp:Literal runat="server" ID="Literal1" Text="<%$resources:error_allocationAdjnBalAmt %>"></asp:Literal>
                                </li>
                            </ul>
                        </div>
                        <div class="error" id="divWrongAdj" runat="server" visible="false">
                            <ul>
                                <li>
                                    <asp:Literal runat="server" ID="LtrWrongAdj" Text="<%$resources:adj_Allocated %>"></asp:Literal>
                                </li>
                            </ul>
                        </div>
                        <div class="gridwrap">
                            <asp:TableCell>
                                <div class="gridwrap">
                                    <asp:GridView ID="grdReceiptSplitAdjn" runat="server" AutoGenerateColumns="False"
                                        Width="100%" PageSize="<%$ resources:PageSize %>" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable"
                                        AllowSorting="false" ShowFooter="true">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$resources:no %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAdjNo" runat="server" Text='<%# Eval("RAD_NO") %>'></asp:Label>
                                                    <asp:HiddenField ID="hdfReceiptMpgPK" runat="server" Value='<%# Eval("RAD_RECEIPT_TRX") %>' />
                                                    <asp:HiddenField ID="hdfInvPK" runat="server" Value='<%# Eval("RAD_RCM_INVOICE_HDR") %>' />
                                                    <asp:HiddenField ID="hdfAdjnPK" runat="server" Value='<%# Eval("RAD_PK") %>' />
                                                    <asp:HiddenField ID="hdfCrDrPK" runat="server" Value='<%# Eval("RAD_ALCN_CDH") %>' />
                                                    <asp:HiddenField ID="hdfReceiptAdjnPK" runat="server" Value='<%# Eval("RAD_ALCN_RECEIPT_TRX") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:PageType %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPageType" runat="server" Text='<%# Eval("RAD_TYPE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:Date %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDate" runat="server" Text='<%# Eval("RAD_DATE", Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:Amount %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmountAdjn" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("RAD_AMOUNT_TOTAL")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblTotalAmountAdjn" runat="server"></asp:Label>
                                                </FooterTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:Alloc %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAllocatedAdjn" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("RAD_AMOUNT_RCVD")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblTotalReceivedAdjn" runat="server"></asp:Label>
                                                </FooterTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:Balance %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBalanceAdjn" CssClass="BalancetoPay" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("RAD_AMOUNT_BAL")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblTotalBalanceAdjn" runat="server"></asp:Label>
                                                    <asp:HiddenField ID="hdfBalanceAdjn" runat="server" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%--Pay Now--%>
                                            <asp:TemplateField HeaderText="<%$ resources:AllocateNow %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAllocateAdjn" runat="server" autocomplete="off" CssClass="Uiinput-amount numeric"
                                                        Text='<%# GetFormattedCurrency(Eval("RAD_AMOUNT")) %>' MaxLength="15" onkeyup="CalculateTotalAdjn(this);">  
                                                    </asp:TextBox>
                                                    <asp:HiddenField ID="hdfPayNowSplit" runat="server" />
                                                    <div class="starwrap">
                                                        <cc1:AmountValidation ID="vreDedAllocateNowSplit" runat="server" ControlToValidate="txtAllocateAdjn"
                                                            ErrorMessage="<%$ resources:Err_InvalidAllocation %>" NumberDigits="11" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="SplitAdjn"></cc1:AmountValidation>
                                                        <asp:CustomValidator ID="customQty" runat="server" ValidateEmptyText="true" ClientValidationFunction="CheckReceiptAllocation"
                                                            ErrorMessage="<%$ resources:Err_InvalidAllocation %>" Text="*" EnableClientScript="true"
                                                            ControlToValidate="txtAllocateAdjn" CssClass="star" Display="Dynamic" ValidationGroup="SplitAdjn"></asp:CustomValidator>
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
                <%------------- Receipt CR/DR Allocation Popup Start ---------------------%>
                <div id="divCrdrAllocation" style="display: none">
                    <div class="content-wrapper">
                        <asp:Panel runat="server" ID="pnlCrdrAllocation" CssClass="Button-container-popup">
                            <%-- <div id="divbtnSavePaymentSplit" runat="server" class="Button-container-popup">--%>
                            <asp:Button ID="btnCrdrAllocationApply" runat="server" Text="<%$ resources:Controls,Apply %>"
                                OnClick="ActionHandler" CommandName="CRDRALLOCATIONSAVE" SkinID="btnInner-add-dsd"
                                CommandArgument="Allocation_Section" ValidationGroup="crdrAllocation" OnClientClick="javascript:ValidatePageNow('crdrAllocation')" />
                            <%--</div>--%>
                        </asp:Panel>
                        <div class="detail-co3">
                            <div class="div3col-S">
                                <asp:Label ID="Label5" runat="server" Text="<%$resources:InvoiceNo1 %>" AssociatedControlID="lblInvSplitNo_CrdrAlcn"
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblInvSplitNo_CrdrAlcn" runat="server" CssClass="medium"></asp:Label>
                                <asp:Label ID="Label9" runat="server" Text="<%$resources:Amount1 %>" AssociatedControlID="lblInvSplitAmount_CrdrAlcn"
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblInvSplitAmount_CrdrAlcn" runat="server" CssClass="medium"></asp:Label>
                            </div>
                            <div class="div3col-S">
                                <asp:Label ID="Label12" runat="server" Text="<%$resources:Date1 %>" AssociatedControlID="lblInvSplitDate_CrdrAlcn"
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblInvSplitDate_CrdrAlcn" runat="server" CssClass="medium"></asp:Label>
                                <asp:Label ID="Label14" runat="server" Text="<%$resources:Received1 %>" AssociatedControlID="lblInvSplitReceived_CrdrAlcn"
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblInvSplitReceived_CrdrAlcn" runat="server" CssClass="medium"></asp:Label>
                            </div>
                            <div class="div3col-S">
                                <asp:Label ID="Label16" runat="server" Text="<%$resources:Customer1 %>" AssociatedControlID="lblInvSplitSupplier_CrdrAlcn"
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblInvSplitSupplier_CrdrAlcn" runat="server" CssClass="medium"></asp:Label>
                                <asp:Label ID="Label18" runat="server" Text="<%$resources:ReceiveNow1 %>" AssociatedControlID="lblInvSplitReceiveNow_CrdrAlcn"
                                    Font-Bold="true"></asp:Label>
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
                                        AllowSorting="false" ShowFooter="true">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:CrDrNO %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrdrNo" runat="server" CssClass="medium" Text='<%#Eval("RNM_CRDR_NO") %>'
                                                        ToolTip='<%#Eval("RNM_CRDR_NO") %>'></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfCrdrMpgPk" Value='<%#Eval("RNM_CRDR_MPG") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:CrDrDate %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrdrDate" runat="server" Text='<%# Eval("RNM_CRDR_DATE", Resources.Constants.DateFormatGrid)%>'
                                                        ToolTip='<%# Eval("RNM_CRDR_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:CrdrCurrency %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrdrCurrency" runat="server" Text='<%#Eval("RNM_CRDR_CURRENCY_TEXT") %>'
                                                        ToolTip='<%#Eval("RNM_CRDR_CURRENCY_TEXT") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:CrdrAmount %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrdrAmount" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("RNM_CRDR_AMOUNT")) %>'
                                                        ToolTip='<%# GetFormattedCurrencyWithComma(Eval("RNM_CRDR_AMOUNT")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:CrdrReceivedAmount %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrdrPaidAmount" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("RNM_ALLOCATED_AMOUNT")) %>'
                                                        ToolTip='<%# GetFormattedCurrencyWithComma(Eval("RNM_ALLOCATED_AMOUNT")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:CrdrBalanceAmount %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrdrBalanceAmount" runat="server" Text='<%# GetFormattedCurrencyWithComma( Convert.ToDecimal(Eval("RNM_CRDR_AMOUNT")) - Convert.ToDecimal(Eval("RNM_ALLOCATED_AMOUNT"))) %>'
                                                        ToolTip='<%# GetFormattedCurrencyWithComma(Convert.ToDecimal(Eval("RNM_CRDR_AMOUNT")) - Convert.ToDecimal(Eval("RNM_ALLOCATED_AMOUNT"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:CrdrReceiveNow %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCrdrReceiveNow" runat="server" CssClass="Uiinput-amount numeric"
                                                        onkeyup="CalculateTotalDebitSplit(this);" MaxLength="16" Text='<%# GetFormattedCurrency(Eval("RNM_PAID_AMOUNT")) %>'>
                                                    </asp:TextBox>
                                                    <cc1:AmountValidation ID="vamCrdrReceiveNow" runat="server" ControlToValidate="txtCrdrReceiveNow"
                                                        ErrorMessage="<%$ resources:Err_PaymentAmount %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="crdrAllocation"></cc1:AmountValidation>
                                                    <%--<asp:CustomValidator ID="vcmCrdrPayNow" CssClass="star" SetFocusOnError="true" ClientValidationFunction="ValidateDebitSplit"
                                                        ValidationGroup="crdrAllocation" EnableClientScript="true" runat="server" ControlToValidate="txtCrdrReceiveNow"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_CrdrAmntExceeds %>"></asp:CustomValidator>--%>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" Wrap="false" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblCrdrReceiveNowFooter"></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfCrdrPayNowFooter" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:CrdrAdjAmount %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCrdrAdjAmount" runat="server" CssClass="Uiinput-amount numeric"
                                                        onkeyup="CalculateTotalDebitSplit(this);" MaxLength="16" Text='<%# GetFormattedCurrency(Eval("RNM_ADJ_AMOUNT")) %>'>
                                                    </asp:TextBox>
                                                    <cc1:AmountValidation ID="vamCrdrAdjAmount" runat="server" ControlToValidate="txtCrdrAdjAmount"
                                                        ErrorMessage="<%$ resources:Err_CrdrAdjAmnt %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="crdrAllocation"></cc1:AmountValidation>
                                                    <asp:CustomValidator ID="vcmCrdrAdjAmount" CssClass="star" SetFocusOnError="true"
                                                        ClientValidationFunction="ValidateDebitSplit" ValidationGroup="crdrAllocation"
                                                        EnableClientScript="true" runat="server" ControlToValidate="txtCrdrReceiveNow"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_CrdrAmntExceeds %>"></asp:CustomValidator>
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
                <%------------- Receipt CR/DR Allocation Popup End ---------------------%>
                <%--------------Received amount splitup popup start-----------------------%>
                <div id="divReceivedAmtsplitup" style="display: none">
                    <div class="Button-container-popup">
                    </div>
                    <div class="content-wrapper">
                        <div class="gridwrap" id="divReceivedDetails">
                            <asp:GridView runat="server" ID="grdReceivedAmtSplitup" Width="100%" AllowSorting="false"
                                AutoGenerateColumns="false" TabIndex="106" EmptyDataRowStyle-CssClass="emptytable"
                                ShowFooter="true">
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ resources:TrxNo %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTrxNo" runat="server" Text='<%# Convert.ToString(Eval("TrxNo")) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="50%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("TrxDate", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("TrxDate", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                ToolTip='<%# Eval("TrxDate", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:TrxAmount %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("TrxAmt")) %>'
                                                ToolTip='<%#GetFormattedCurrency(Eval("TrxAmt")) %>'></asp:Label>
                                            <asp:HiddenField ID="hdfAmountSplit" runat="server" Value='<%#(Eval("TrxAmt")) %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="30%" HorizontalAlign="Right" />
                                        <HeaderStyle CssClass="amount-numeric" />
                                        <FooterStyle HorizontalAlign="Right" />
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalAmountSplit" runat="server" Text=""></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <%--------------------------Received amount splitup popup end---------------%>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="receipt" runat="server" />
                    <asp:ValidationSummary ID="vsSplit" ValidationGroup="split" runat="server" />
                    <asp:ValidationSummary ID="vsSplitAdjn" ValidationGroup="SplitAdjn" runat="server" />
                    <asp:ValidationSummary ID="vscrdrAllocation" ValidationGroup="crdrAllocation" runat="server" />
                </div>
                <div id="divScriptButtons">
                    <asp:Button runat="server" ID="btnJournalize_Action" CommandName="JOURNALIZE" OnClick="ActionHandler"
                        EnableTheming="false" Style="display: none" />
                    <asp:Button ID="btnJournalizeUpdate" runat="server" OnClick="ActionHandler" CommandName="JOURNALIZEUPDATE"
                        EnableTheming="false" Style="display: none" />
                </div>
                <div id="divJournalize" style="display: none;">
                    <uc1:Journalize ID="ucrJournalize" runat="server" />
                </div>
                <div id="divWkfSubmit" style="display: none;">
                    <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                    <uc1:WorkflowUserComments ID="ucrWrkf" runat="server">
                    </uc1:WorkflowUserComments>
                </div>
            </div>
            <asp:HiddenField ID="hdfOtherPer" Value="0" runat="server" />
            <asp:HiddenField ID="hdfTaxPer" Value="0" runat="server" />
            <asp:HiddenField ID="hdfJournalizeWorkFlow" Value="0" runat="server" />
            <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIscontYes" runat="server" />
            <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
            <asp:HiddenField ID="hdfCurrencyGroup2" Value="2" runat="server" />
            <asp:HiddenField ID="hdfTotalOtherCharges" runat="server" />
            <asp:HiddenField ID="hdfTotalOtherChargesSplit" runat="server" />
            <asp:HiddenField ID="hdfTottaxHDR" Value="0" runat="server" />
            <asp:HiddenField ID="hdfTottaxSplit" runat="server" />
            <asp:HiddenField ID="hdfTrial" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
            <asp:HiddenField ID="hdfTotalAmtDtl" Value="0" runat="server" />
            <asp:HiddenField ID="hdfBaltoAlloc" Value="0" runat="server" />
            <asp:HiddenField ID="hdfCategoryDtl" runat="server" />
            <asp:HiddenField ID="hdfTypeDtl" runat="server" />
            <asp:HiddenField ID="hdfShowCancel" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsCancelled" runat="server" Value="0" />
            <asp:HiddenField ID="hdfSavedBankPk" runat="server" Value="0" />
            <asp:HiddenField ID="hdfReceiptReturnHide" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCurrStatus" runat="server" Value="0" />
            <asp:HiddenField ID="hdfEditForReturn" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsReceiptReturned" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCurrencyFormatWithSeperator" runat="server" />
            <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
            <asp:HiddenField ID="AST_CODE" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
