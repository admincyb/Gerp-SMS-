<%@ Page Title="<%$ Resources:Captions,Title_DebitCreditNote %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="DebitCreditNoteTrading.aspx.cs" Theme="ClassicExt"
    Inherits="ERPSMS_v01.Sales.DebitCreditNoteTrading" %>

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
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
            $("[id$=hdfPreviousUrl]").val(document.referrer);
        });
        var crNo = '<%= GetLocalResourceObject("CrNo") %>';
        var drNo = '<%= GetLocalResourceObject("DrNo") %>';
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        //var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustSearch", url, "hdfCustSearchID", true, true, "CUSTOMERLIST");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url, "hdfCustomerPk", true, true, "CUSTOMERLIST");
            if ($("[id$=ddlMode]").val() > 0) {
                GrandScriptUtils.MakeAutoCompleteDDL("txtBank", url + "?Type=" + $("[id$=ddlMode]").val(), "hdfBank", true, true, "BANK");
            }
            GrandScriptUtils.DatePickerCommon("txtDate");
            GrandScriptUtils.DatePickerCommon("txtInstrumentDate");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCrDrNumber", url + "?Type=2", "hdfCrDrNumber", true, true, "GETDRCRNOTRADINGAUTO");
            GrandScriptUtils.AddDateRangeCommon("txtSearchDateFrom", "hdfSearchDateFrom", "txtSearchDateTo", "hdfSearchDateTo", false, false);
            GrandScriptUtils.AddDateRangeCommon("txtPendingFromDate", "hdfPendingFromDate", "txtPendingToDate", "hdfPendingToDate", false, false);
            GrandScriptUtils.DatePickerCommon("txtPVDate");
            //$("[id$=txtJournalExchangeRate]").ForceNumericOnly();
            $("[id*=txtNoteFor]").ForceNumericOnly();
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();
            if ($('[id$=btnJournalSaveSubmit]').is(":visible"))
                $('[id$=btnJournalSubmit]').hide();

            setPaymentLabel();

            $("[id*=txtQtySplit]").ForceNumericOnly();
            $("[id*=txtRateSplit]").ForceNumericOnly();
            $("[id*=txtSumSplit]").ForceNumericOnly();
            $("[id*=txtExchangeRate]").ForceNumericOnly();
            $("[id*=txtShipCharge]").ForceNumericOnly();
            $("[id*=txtPopupAmount]").ForceNumericOnly();

            //Set a stamp for cancelled record
            if ($("[id$=hdfCancelled]").val() == "1")
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
            else
                $("[id$=tblDetailHdr]").addClass("table-devide");

            //End
            ModeChanged();

            InitPendingInvNo();
            if ($("[id$=txtPendingInvNumber]").attr("disabled") == true) {
                DisableAuto($("[id$=txtPendingInvNumber]"), $("[id$=hdfPendingInvPk]"));
            }
            else {
                EnableAuto($("[id$=txtPendingInvNumber]"), $("[id$=hdfPendingInvPk]"));
            }
            ShowHidePendingInv($("[id$=hdfIsPendingInvVisible]").val());

            if ($("[id$=txtCustomer]").attr("disabled") == true) {
                DisableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomerPk]"));
            }
            else {
                EnableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomerPk]"));
            }
        }
        function InitPendingInvNo() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtPendingInvNumber", url + "?CustomerID=" + $("[id$=hdfCustomerPk]").val(), "hdfPendingInvPk", true, true, "SALINVNUMBERTRADING");
        }

        function ResetPendingInvNo() {
            var defText = '<%= Resources.ErpRes.AutoDefaultValue %>';
            $("[id$=txtPendingInvNumber]").val(defText);
            $("[id$=hdfPendingInvPk]").val('-1');
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

        function CalculateAmount(sender) {

            var netamount = 0;
            var amount = 0;
            var DecimalDigits = 0;
            var taxpercentage = 0;
            var basevalue = 0;
            var ttaxamt = 0;
            var hdftax = 0;
            var ExchangeRate = 0;
            var grndTotalTax = 0;


            var CurrencyFormat = 0;
            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }
            if (!isNaN(parseFloat($("#[id*=hdfCurrencyFormat]").val()))) {
                CurrencyFormat = parseFloat($("#[id*=hdfCurrencyFormat]").val());
            }
            if (!isNaN(parseFloat($("#[id*=hdfExchangeRate]").val()))) {
                ExchangeRate = parseFloat($("#[id*=hdfExchangeRate]").val());
            }
            //$("#[id*=grdDCSplit] input[type=text][id*=txtQtySplit]").each(function (index) {
            var qty = 0;
            var rate = 0;
            //                if ($.trim($(sender).val()) != "") {
            //                    //Check if number is a valid integer
            //                    if (!isNaN(parseFloat($(sender).val()))) {

            //                        qty = parseFloat($(sender).val());
            //                    }
            //                }

            if (!isNaN(parseFloat($(sender).closest('tr').find("#[id*=txtQtySplit]").val()))) {
                qty = Number($(sender).closest('tr').find("#[id*=txtQtySplit]").val().replace(/[^0-9\.]+/g, ""));
                qty = parseFloat(qty);
            }
            if (!isNaN(parseFloat($(sender).closest('tr').find("#[id*=txtRateSplit]").val()))) {
                rate = Number($(sender).closest('tr').find("#[id*=txtRateSplit]").val().replace(/[^0-9\.]+/g, ""));
                rate = parseFloat(rate);
            }
            if (!isNaN(parseFloat($(sender).closest('tr').find("#[id*=hdfTAXSplit]").val()))) {
                var number = Number($(sender).closest('tr').find("#[id*=hdfTAXSplit]").val().replace(/[^0-9\.]+/g, ""));
                hdftax = parseFloat(number);
            }
            if (!isNaN(parseFloat($(sender).closest('tr').find("#[id*=hdfNETSplit]").val()))) {
                var number = Number($(sender).closest('tr').find("#[id*=hdfNETSplit]").val().replace(/[^0-9\.]+/g, ""));
                netamount = parseFloat(number);
            }

            if (!isNaN(qty) && !isNaN(rate)) {
                amount = qty * rate;
            }
            else {
                amount = 0;
            }
            if (((amount.toFixed(NumberDigits)).length) <= 15) {
                if (amount > 0) {
                    $(sender).closest('tr').find("#[id*=txtSumSplit]").val(amount.toFixed(CurrencyDigits));
                }
                else {
                    if (!isNaN(parseFloat($(sender).closest('tr').find("#[id*=txtSumSplit]").val()))) {
                        var number = Number($(sender).closest('tr').find("#[id*=txtSumSplit]").val().replace(/[^0-9\.]+/g, ""));
                        amount = parseFloat(number);
                    }
                }

                taxpercentage = hdftax / (netamount == 0 ? 1 : netamount);
                //tamt = parseFloat($(sender).val());
                //basevalue = (amount) / (1 + taxpercentage);
                //ttaxamt = (amount - basevalue);
                ttaxamt = (amount * taxpercentage);
                grndTotalTax = grndTotalTax + ttaxamt;
                $(sender).closest('tr').find("#[id*=lbnTAXSplitTotal]").text(addCommas(ttaxamt.toFixed(CurrencyDigits)));
                $(sender).closest('tr').find("#[id*=lbnTAXSplitTotal]").attr("title", addCommas(ttaxamt.toFixed(CurrencyDigits)));
                $(sender).closest('tr').find("#[id*=hdfTAXSplitTotal]").val(ttaxamt.toFixed(CurrencyDigits));
                CalculateTotalSplit($(sender).closest('tr').find("#[id*=txtSumSplit]").val());
            }
            //            });

            //            $("#[id*=grdDCSplit] [id*=lblTotalTaxFooterSplit]").html(grndTotalTax.toFixed(CurrencyDigits));
            //            $("#[id$=hdfTotalTaxFooterSplit]").val(grndTotalTax.toFixed(CurrencyDigits));
        }

        function CalculateTotalSHP(sender) {

            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }
            var ShipCharge = 0;
            var OtherCharge = 0;
            var TotalTax = 0;
            var SubTotal = 0;
            var SplitCount = 0;
            if (!isNaN(parseInt($("#[id*=hdfSplitCount]").val()))) {
                SplitCount = parseInt($("#[id*=hdfSplitCount]").val());
            }
            if (!isNaN(parseFloat($("#[id*=txtShipCharge]").val()))) {
                ShipCharge = parseFloat($("[id$=txtShipCharge]").val());
            }
            //if (isNaN(ShipCharge)) {
            //var tmp = 0;
            //$("[id$=txtShipCharge]").val(tmp.toFixed());
            //}
            OtherCharge = parseFloat($("[id$=txtOtherCharge]").val());
            lblTotalPayNowFooter = parseFloat($("[id$=lblTotalPayNowFooter]").html().replace(/[^0-9\.]+/g, ""));
            TotalTax = parseFloat($("[id$=lblTotalTaxFooter]").html().replace(/[^0-9\.]+/g, ""));

            if (!isNaN(ShipCharge) || !isNaN(OtherCharge) || !isNaN(lblTotalPayNowFooter || !isNaN(TotalTax))) {
                if (SplitCount == 0) {
                    SubTotal = lblTotalPayNowFooter - TotalTax;
                }
                else {
                    SubTotal = lblTotalPayNowFooter;
                }
                $("[id$=txtSubTotal]").val(SubTotal.toFixed(CurrencyDigits));
                $("[id$=txtSubTotal]").attr("title", SubTotal.toFixed(CurrencyDigits));
                $("[id$=txtTax]").val(TotalTax.toFixed(CurrencyDigits));
                $("[id$=txtTax]").attr("title", TotalTax.toFixed(CurrencyDigits));
                var amount = OtherCharge + ShipCharge + SubTotal + TotalTax;
                if (((amount.toFixed(CurrencyDigits)).length) <= 15) {
                    if (!isNaN(amount)) {
                        $("[id$=txtHdrTotal]").val(amount.toFixed(CurrencyDigits));
                        $("[id$=txtHdrTotal]").attr("title", amount.toFixed(CurrencyDigits));
                    }
                }
            }
        }

        function CalculateTotalSplit(sender) {


            var val1 = parseFloat($(sender).val());
            var Amount = 0.00;
            var BalancetoPay = 0;
            var DecimalDigits = 0;
            var CurrencyFormat = 0;

            var netamount = 0;
            var splitamnt = 0;
            var taxpercentage = 0;
            var basevalue = 0;
            var ttaxamt = 0;
            var hdftax = 0;
            var grndTotalTax = 0;

            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }
            if (!isNaN(parseFloat($("#[id*=hdfCurrencyFormat]").val()))) {
                CurrencyFormat = parseFloat($("#[id*=hdfCurrencyFormat]").val());
            }

            $("#[id*=grdDCSplit] input[type=text][id*=txtSumSplit]").each(function (index) {

                //Check if number is not empty
                if ($.trim($(this).val()) != "") {
                    //   Check if number is a valid integer
                    if (!isNaN(parseFloat($(this).val()))) {

                        $(this).parent("td").find('input[type=hidden][id$=hdfPayNowSplit]').val($(this).val());
                        Amount = Amount + parseFloat($(this).val());

                        if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=hdfTAXSplit]").val()))) {
                            var number = Number($(this).closest('tr').find("#[id*=hdfTAXSplit]").val().replace(/[^0-9\.]+/g, ""));
                            hdftax = parseFloat(number);
                        }
                        if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=hdfNETSplit]").val()))) {
                            var number = Number($(this).closest('tr').find("#[id*=hdfNETSplit]").val().replace(/[^0-9\.]+/g, ""));
                            netamount = parseFloat(number);
                        }


                        splitamnt = parseFloat($(this).val())
                        taxpercentage = hdftax / (netamount == 0 ? 1 : netamount);
                        //tamt = parseFloat($(this).val());
                        //basevalue = (amount) / (1 + taxpercentage);
                        //ttaxamt = (amount - basevalue);
                        // ttaxamt = (splitamnt * taxpercentage);
                        ttaxamt = parseFloat((splitamnt * taxpercentage).toFixed(CurrencyDigits));
                        grndTotalTax = grndTotalTax + ttaxamt;
                        $(this).closest('tr').find("#[id*=lbnTAXSplitTotal]").text(addCommas(ttaxamt.toFixed(CurrencyDigits)));
                        $(this).closest('tr').find("#[id*=lbnTAXSplitTotal]").attr("title", addCommas(ttaxamt.toFixed(CurrencyDigits)));
                        $(this).closest('tr').find("#[id*=hdfTAXSplitTotal]").val(ttaxamt.toFixed(CurrencyDigits));
                    }
                }
            });

            $("#[id*=grdDCSplit] [id*=lblTotalPayNowFooterSplit]").html(addCommas(Amount.toFixed(CurrencyDigits)));
            $("#[id*=grdDCSplit] [id*=hdfTotalPayNowFooterSplit]").val(Amount.toFixed(CurrencyDigits));

            $("#[id*=grdDCSplit] [id*=lblTotalTaxFooterSplit]").html(addCommas(grndTotalTax.toFixed(CurrencyDigits)));
            $("#[id*=grdDCSplit] [id*=hdfTotalTaxFooterSplit]").val(grndTotalTax.toFixed(CurrencyDigits));

        }

        function setPaymentLabel() {
            if ($("[id$='ddlMode']").val() == '1') {
                $("[id$='lblPaymentNo']").text(drNo);
            }
            else if ($("[id$='ddlMode']").val() == '2') {
                $("[id$='lblPaymentNo']").text(crNo);
            }
        }
        $(document).ready(function () {
            $("[id$='ddlMode']").live("change", function () {
                setPaymentLabel();
            });
        });

        function AfterClose(containerID) {
            if (containerID == "[id$=divJournalize]") {
                $("[id$=btnJournalizeUpdate]").click();
            }
            else if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
                if ($("[id$=hdfJournalizeWorkFlow]").val() == "1") {
                    //ShowContainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), '1000', '550');
                    ShowCommonCotainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), "1%");
                    AfterCloseWkfInJournal();
                    //$("[id$=btnJournalize_Action]").click();
                }
            } else if (containerID == "[id$=divTemplate]") {
                //ShowContainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), '1000', '550');
                ShowCommonCotainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), "1%");
            }
            else if (containerID == "[id$=divTaxSplitupLIneItem]") {
                // Showing Splitup Popup
                CalculateAmount();
                CalculateTotalSplit();
                $("[id$=btnShowDetailSplit]").click();
                //ShowContainerDiv('[id$=divCrDrSplitUp]', '<%= GetLocalResourceObject("ReceiptSplit").ToString()%>', '900', '300');
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
                $("[id$=hdfCancelled]").val('0');
            }
            else {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
                $("[id$=ddlCompany]").show();
                if ($("[id$=hdfCancelled]").val() == "1") {
                    $("[id$=pnlSave]").hide();
                }
            }
            return false;
        }
        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlSaveSubmit]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlPrint]").hide();
                //$("[id$=pnlPrintList]").hide();
            }
            if (mode == 3) {
                $("[id$=pnlSave]").hide();
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
            else if (targetControlID == "txtCustomer") {
                ResetPendingInvNo();
                $("[id$=btnCustomer]").click();
            }
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteSelect(targetControlID);
            }

        }

        function RaiseNoteChange(sender) {
            var itemindex = 0;
            itemindex = $(sender).closest('tr').find('[id*=hdfInvRowIndex]').val();
            $("#[id*=hdfRowIndex]").val(itemindex);
            $("[id$=btnRaiseNote]").click();
            sender.focus();
        }

        function CalculateTotal(sender) {
           
            var val1 = parseFloat($(sender).val());
            var Amount = 0;
            var BalancetoPay = 0;
            var DecimalDigits = 0;
            var taxpercentage = 0;
            var basevalue = 0;
            var ttaxamt = 0;
            var hdftax = 0;
            var hdftotalamt = 0;
            var TotalTaxFooter = 0;
            var ItemIncluded = 0;

            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }

            $("#[id*=grdInvoiceList] input[type=text][id*=txtNoteFor]").each(function (index) {
                if (!isNaN(parseFloat($(this).closest('tr').find('.BalancetoPay').text()))) {
                    var number = Number($(this).closest('tr').find('.BalancetoPay').text().replace(/[^0-9\.]+/g, ""));
                    BalancetoPay = parseFloat(number);
                }
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=hdfTaxAmt]").val()))) {
                    var number = Number($(this).closest('tr').find("#[id*=hdfTaxAmt]").val().replace(/[^0-9\.]+/g, ""));
                    hdftax = parseFloat(number);
                }
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=hdfTotalAmt]").val()))) {
                    var number = Number($(this).closest('tr').find("#[id*=hdfTotalAmt]").val().replace(/[^0-9\.]+/g, ""));
                    hdftotalamt = parseFloat(number);
                }
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=hdfItemIncluded]").val()))) {
                    var number = Number($(this).closest('tr').find("#[id*=hdfItemIncluded]").val().replace(/[^0-9\.]+/g, ""));
                    ItemIncluded = parseInt(number);
                }
                //Check if number is not empty
                if ($.trim($(this).val()) != "") {
                    //Check if number is a valid integer
                    if (!isNaN(parseFloat($(this).val()))) {
                        //                        if (BalancetoPay < parseFloat($(this).val())) {
                        //                            $("[id$=litErrorMsg]").show();
                        //                            $("[id$=litErrorMsg]").html("Paid amount should be less than or equal to Balance to Pay");
                        //                            ShowErrorMessage($("#diverror").html());
                        //                            $(this).val('0');
                        //                        }
                        //                        else {
                        taxpercentage = hdftax / (hdftotalamt == 0 ? 1 : hdftotalamt);
                        tamt = parseFloat($(this).val());
                        if (ItemIncluded == 1) {
                            ttaxamt = tamt * taxpercentage;
                        }
                        else {
                            basevalue = (tamt) / (1 + taxpercentage);
                            ttaxamt = (tamt - basevalue);
                        }
                        //                        $(this).closest('tr').find("#[id*=lblTotalTax]").text(ttaxamt.toFixed(NumberDigits));
                        //                        $(this).closest('tr').find("#[id*=lblTotalTax]").attr("title", ttaxamt.toFixed(NumberDigits));
                        if ($("[id$=hdfIsLineItemTaxEnabled]").val() == "1") {
                            if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=hdfTotalTax]").val()))) {
                                ttaxamt = parseFloat($(this).closest('tr').find("#[id*=hdfTotalTax]").val());
                            }
                        }
                        else {
                            $(this).closest('tr').find("#[id*=lbnTotalTax]").text(addCommas(ttaxamt.toFixed(CurrencyDigits)));
                            $(this).closest('tr').find("#[id*=lbnTotalTax]").attr("title", addCommas(ttaxamt.toFixed(CurrencyDigits)));
                            $(this).closest('tr').find("#[id*=hdfTotalTax]").val(ttaxamt.toFixed(CurrencyDigits));
                        }
                        //alert(ttaxamt);
                        //  var m = $("#[id*=hdfTotalTax]").val();

                        //                        $("#[id*=hdfTotalTax]").val(ttaxamt.toFixed(DecimalDigits));
                        //                        var v = $("#[id*=hdfTotalTax]").val();
                        //                        $(this).closest('tr').find("#[id*=hdfTotalTax]").text(ttaxamt.toFixed(DecimalDigits));
                        //                        $(this).closest('tr').find("#[id*=hdfTotalTax]").attr("title", ttaxamt.toFixed(DecimalDigits));
                        Amount = Amount + parseFloat($(this).val());
                        TotalTaxFooter = TotalTaxFooter + ttaxamt;
                        //}
                    }
                }
                else {
                    var taxZero = 0;
                    $(this).closest('tr').find("#[id*=lbnTotalTax]").text(addCommas(taxZero.toFixed(CurrencyDigits)));
                    $(this).closest('tr').find("#[id*=lbnTotalTax]").attr("title", addCommas(taxZero.toFixed(CurrencyDigits)));
                    $(this).closest('tr').find("#[id*=hdfTotalTax]").val(taxZero.toFixed(CurrencyDigits));
                }
            });
            $("#[id*=grdInvoiceList] [id*=lblTotalPayNowFooter]").html(addCommas(Amount.toFixed(CurrencyDigits)));
            $("#[id*=grdInvoiceList] [id*=lblTotalTaxFooter]").html(addCommas(TotalTaxFooter.toFixed(CurrencyDigits)));
            $("#[id*=grdInvoiceList] [id*=hdfPayNowFooter]").val(Amount.toFixed(CurrencyDigits));
            //$("#[id*=grdInvoiceList] input[type=text][id*=lblTotalPayNowFooter]").val(Amount);
            $("#[id*=txtPaidAmount]").val(Amount.toFixed(CurrencyDigits));

            $("#[id$=hdfTotalPayNowFooter]").val(Amount.toFixed(CurrencyDigits));
            CalculateTotalSHP(this);
            //            sender.focus();
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
        function ValidateNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                CheckValidationDuplicate(valGroup);
                Page_ClientValidate();
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

        function RaiseNoteChanged() {

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("RaiseNoteNotTallied") %>';
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
                        $("[id$=btnRaiseNote]").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfIscontYes]").val(0);
                        $("[id$=hdfIscontNo]").val(1);
                        $(this).dialog("close");
                        $("[id$=btnRaiseNote]").click();
                        return false;
                    }
                }
            });
            return false;
        }
        function validateRateFloatKeyPress(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            var number = el.value.split('.');
            if (charCode == 8) {
                return true;
            }
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            rateDecimal = 4;
            if (!isNaN(parseInt($("[id$=hdfRateFormat]").val()))) {
                rateDecimal = parseInt($("[id$=hdfRateFormat]").val());
            }
            var caratPos = getSelectionStart(el);
            var dotPos = el.value.indexOf(".");
            if (caratPos > dotPos && dotPos > -1 && (number[1].length > rateDecimal - 1)) {
                return false;
            }
            return true;
        }

        function ValidateRaiseNote(sender, args) {
            var payNow = $(sender).closest('tr').find('[id*=txtNoteFor]').val();
            var gross = $(sender).closest('tr').find('[id*=lblGrossAmount]').text().replace(/[^0-9\.]+/g, "");
            var tax = $(sender).closest('tr').find('[id*=lblTax]').text().replace(/[^0-9\.]+/g, "");

            var pattern = new RegExp($(sender).closest('tr').find('[id*=vamNoteFor]')[0].validationexpression);
            var payNowAmt = parseFloat(payNow);
            var grossamnt = parseFloat(gross);
            var taxAmnt = parseFloat(tax);
            if (!pattern.test(payNow) || isNaN(payNowAmt) || (payNowAmt > (grossamnt + taxAmnt))) {
                args.IsValid = false;
            }
            else {
                args.IsValid = true;
            }
        }


        function CalculateTotalAmountSplit(sender) {
            var TotalAmountSplit = 0;
            $("#[id*=grdBalAmntSplitup] input[type=hidden][id*=hdfAmountSplit]").each(function (index) {
                TotalAmountSplit = TotalAmountSplit + parseFloat($(this).val());
            });
            $("#[id*=grdBalAmntSplitup] [id*=lblTotalAmountSplit]").html(TotalAmountSplit.toFixed(CurrencyDigits));

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
        function toFixedNumber(num, precision) {
            return (+(Math.round(+(num + 'e' + precision)) + 'e' + -precision)).toFixed(precision);
        }


        function ModeChanged() {
            var mode = parseInt($("[id$=ddlMode]").val());
            if (mode == 2) {//Credit
                $("[id*=chkIsReceiptAlcnReq]").attr('disabled', false);
                $("[id*=chkIsReceiptAlcnReq]").removeClass("input-disabled");
            }
            else {
                $("[id*=chkIsReceiptAlcnReq]").removeAttr("checked");
                $("[id*=chkIsReceiptAlcnReq]").attr('disabled', true);
                $("[id*=chkIsReceiptAlcnReq]").addClass("input-disabled");
            }
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
    <asp:UpdatePanel runat="server" ID="aupdpnlManpowerAttendance">
        <ContentTemplate>
            <asp:HiddenField ID="hdfCancelled" runat="server" Value="0" />
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <div id="divSBUCompany" class="buttoncontainer-fields floatLeft">
                                    <asp:DropDownList ID="ddlCompany" class="select-full-a margnbotm0" runat="server"
                                        TabIndex="1" onmouseover="javascript:ShowTooltip('ddlCompany');">
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
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('drcr')" ValidationGroup="drcr"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="102"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('drcr')"
                                            ValidationGroup="drcr" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="103" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('drcr')" ValidationGroup="drcr"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteCrdr" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="104" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" />
                                    </li>
                                    <li runat="server" id="pnlPrint">
                                        <asp:Button runat="server" TabIndex="105" ID="btnListPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
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
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="108" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="109" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelDrCr %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelDrCr %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="110" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="111" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                    <li id="pnlPrintList">
                                        <asp:Button runat="server" TabIndex="112" ID="btnPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
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
                                OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="CREDITDEBITLIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="CREDITDEBITDETAIL"
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
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn----------%>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="/*margin-top: 8px; */ background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblSearchDateFrom" Text="<%$ resources:FromDate %>"
                                                AssociatedControlID="txtSearchDateFrom"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSearchDateFrom" CssClass="input-small margnrgt0-8per"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfSearchDateFrom" runat="server" />
                                            <asp:Label runat="server" ID="lblSearchDateTo" Text="<%$ resources:ToDate %>" AssociatedControlID="txtSearchDateTo"
                                                class="middle-lbl-small-c"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSearchDateTo" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfSearchDateTo" runat="server" />
                                            <asp:Label ID="lblCompany" runat="server" Text="<%$ resources:Controls, CompanyPlant %>"
                                                AssociatedControlID="ddlCompanySrch"></asp:Label>
                                            <asp:DropDownList ID="ddlCompanySrch" runat="server" CssClass="select-small-a1" TabIndex="3">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblInvoiceNo" Text="<%$ resources:InvNo %>" AssociatedControlID="txtInvoiceNo"
                                                CssClass="lbl-14-2perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInvoiceNo" CssClass="input-small margnrgt0-8per"></asp:TextBox>
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" AssociatedControlID="ddlStatus"
                                                class="lbl-30perc"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="input-small">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,NotPosted %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Posted %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="4"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide ">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblCustomer" runat="server" Text="<%$resources:Customer %>" AssociatedControlID="txtCustSearch"></asp:Label>
                                            <asp:TextBox ID="txtCustSearch" runat="server" CssClass="select-half margnbotm0"
                                                MaxLength="100"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustSearchID" runat="server" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblPaymentNumber" runat="server" Text="<%$resources:CrDrNumber %>"
                                                AssociatedControlID="txtCrDrNumber" CssClass="lbl-14-2perc"></asp:Label>
                                            <asp:TextBox ID="txtCrDrNumber" runat="server" CssClass="select-small-a0 margnbotm0 margnrgt0-7per"
                                                MaxLength="100">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfCrDrNumber" runat="server" Value="" />
                                            <asp:Label ID="lblInType" runat="server" Text="<%$resources:Type %>" AssociatedControlID="ddlInvoiceType"
                                                class="lbl-7-5perc"></asp:Label>
                                            <asp:DropDownList ID="ddlInvoiceType" runat="server" CssClass="input-small margnbotm0">
                                            </asp:DropDownList>
                                            <asp:Label runat="server" ID="lblCnDn" Text="<%$ resources:CreditDebitType%>" AssociatedControlID="ddlCreditDebitType"
                                                CssClass="middle-lbl-xsmall-e"></asp:Label>
                                            <asp:DropDownList ID="ddlCreditDebitType" runat="server" CssClass="select-small margnbotm0">
                                                <asp:ListItem Text="<%$ Resources:Captions,Select %>" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Debit %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Credit %>" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="btnSearchHdr" runat="server" Text="" ToolTip="<%$ resources:Controls,Search %>"
                                                OnClick="ActionHandler" CommandName="SEARCH" SkinID="search-ext" Style="margin-bottom: 0px!important;
                                                margin-top: 2px;" />
                                            <asp:ImageButton ID="btnClear" runat="server" Text="" ToolTip="<%$ resources:Controls,Clear %>"
                                                OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext" Style="margin-bottom: 0px!important;
                                                margin-top: 2px;" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdCrDbHdr" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                    OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" runat="server" GroupName="SelectOne" ID="rbtSelect"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfCrDrPk" Value='<%# Eval(Resources.DataFieldRes.CrDrPk) %>' />
                                                <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval("CDH_DEPT") %>' />
                                                <asp:HiddenField ID="hdfCrDrInvType" runat="server" Value='<%# Eval("ICH_TYPE") %>' />
                                                <asp:HiddenField ID="hdfDelStatus" runat="server" Value='<%# Eval("CDH_IS_DELETED") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DateofIssue %>" SortExpression="<%$ resources:DataFieldRes,CrDrDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaymentDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.CrDrDate, Resources.Constants.DateFormatGrid).ToString()  %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.CrDrDate, Resources.Constants.DateFormatGrid).ToString() %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:No %>" SortExpression="<%$ resources:DataFieldRes,CrDrNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTransactionId" runat="server" Text='<%# Eval(Resources.DataFieldRes.CrDrNo)==""?"[NEW]": Eval(Resources.DataFieldRes.CrDrNo)%>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.CrDrNo)==""?"[NEW]": Eval(Resources.DataFieldRes.CrDrNo) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCmpName" runat="server" CssClass="<%# Eval(Resources.DataFieldRes.CompnayLineColor) %>"
                                                    Text='<%# Eval(Resources.DataFieldRes.CMP_DISPLAY_CODE) %>' ToolTip='<%# Eval(Resources.DataFieldRes.CMP_DISPLAY_CODE)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceNo %>">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkInvnos" runat="server" CssClass="text-underline" OnClick="ActionHandler"
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CDH_INVOICE_NO"),15) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("CDH_INVOICE_NO")) %>'
                                                    Style="text-align: left!important;" CommandName="PRINTCRDRNOTE"></asp:LinkButton>
                                                <asp:HiddenField runat="server" ID="hdfListinvPK" Value='<%# Eval("ICH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfListInvType" Value='<%# Eval("ICH_TYPE") %>' />
                                                <asp:HiddenField runat="server" ID="hdfListinvCategory" Value='<%# Eval("ICH_CATEGORY") %>' />
                                                <asp:HiddenField runat="server" ID="hdfListGroup" Value='<%# Eval("ICH_GROUP") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceDate1 %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvDate" runat="server" Text='<%# Eval("CDH_INVOICE_DATE", Resources.Constants.DateFormatGrid)%>'
                                                    ToolTip='<%# Eval("CDH_INVOICE_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:IssuedTo %>" SortExpression="<%$ Resources:DataFieldRes,CrDrCustomerPK%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CDH_CUSTOMER_TEXT"),28) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("CDH_CUSTOMER_TEXT")) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfCustomerPK" Value='<%# Eval(Resources.DataFieldRes.CrDrCustomerPK) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="26%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CreditDebitType %>" SortExpression="<%$ resources:DataFieldRes,CrDrType %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblModeofPayment" runat="server" Text='<%# Eval("CDH_TYPE_TEXT")%>'
                                                    ToolTip='<%# Eval("CDH_TYPE_TEXT")%>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfCreditDebitType" Value='<%# Eval(Resources.DataFieldRes.CrDrType).ToString()%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CurrencyH %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='<%#Eval("CDH_CURRENCY_TEXT")  %>'
                                                    ToolTip='<%#Eval("CDH_CURRENCY_TEXT")  %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %>" SortExpression="<%$ resources:DataFieldRes,CrDrAmt %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text=' <%# GetFormattedCurrencyWithComma(Eval("CDH_AMOUNT")) %>'
                                                    ToolTip=' <%# GetFormattedCurrencyWithComma(Eval("CDH_AMOUNT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BalanceAmount %>" SortExpression=""
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalanceAmount" runat="server" Text=' <%# GetFormattedCurrencyWithComma(Eval("CDH_BALANCE_AMOUNT")) %>'
                                                    ToolTip=' <%# GetFormattedCurrencyWithComma(Eval("CDH_BALANCE_AMOUNT")) %>' Visible='<%# (Convert.ToDecimal((Eval("CDH_BALANCE_AMOUNT"))) == Convert.ToDecimal(Eval("CDH_AMOUNT"))) ? true : false %>'></asp:Label>
                                                <asp:LinkButton ID="lnkBalanceAmount" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("CDH_BALANCE_AMOUNT")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithComma(Eval("CDH_BALANCE_AMOUNT")) %>' OnClick="ActionHandler"
                                                    CommandName="BALANCEAMOUNTSPLIT" CssClass="text-underline" Visible='<%# (Convert.ToDecimal((Eval("CDH_BALANCE_AMOUNT"))) == Convert.ToDecimal(Eval("CDH_AMOUNT"))) ? false : true %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle CssClass="amount-numeric" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("ASC_CSS_CLASS") %>' ToolTip='<%# Eval("CDH_STATUS_TEXT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval(Resources.DataFieldRes.CrDrApproved) %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgPosted" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? GetLocalResourceObject("unposted").ToString() : Eval("FTH_CSS_CLASS")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? Resources.Captions.NotPosted : Eval("FTH_STATUS_TEXT")%>' />
                                                <asp:HiddenField runat="server" ID="hdfPosted" Value='<%# Eval(Resources.DataFieldRes.CrDrPosted) %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="2%" />
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
                                            <asp:Label ID="lblDebitCreditNo" runat="server" Text="<%$ resources:No%>" AssociatedControlID="lblDrCrNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblDrCrNo" CssClass="input-small"></asp:Label>
                                            <asp:Label runat="server" ID="lblDate" Text="<%$ resources:DateReq%>" AssociatedControlID="txtDate"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDate" CssClass="input-small" TabIndex="1" onkeydown="return CheckKey(event)"
                                                onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrftDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="drcr" EnableClientScript="true" runat="server" ControlToValidate="txtDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Date %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCust" runat="server" Text="<%$ resources:CustomerReq%>" AssociatedControlID="txtCustomer"></asp:Label>
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
                                                <asp:DropDownList ID="ddlPendingInvCategory" runat="server" TabIndex="5" Width="150px">                                              
                                                </asp:DropDownList>
                                                <asp:Label ID="lblPndngInvType" runat="server" Text="<%$resources:InvoiceType %>"
                                                    AssociatedControlID="ddlPendingInvType" CssClass="margn-lft5"></asp:Label>
                                                <asp:DropDownList ID="ddlPendingInvType" runat="server" TabIndex="6" Width="100px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblInPndvNo" runat="server" Text="<%$ resources:InvNo %>" CssClass="lbl-6perc margn-lft5"
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
                                            <asp:GridView runat="server" ID="grdPendingInvList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                                AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkSelectAllInv" runat="server" ToolTip="Select All" TabIndex="10" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" ID="chkInvselect" TabIndex="11" />
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
                                                    <%--  <asp:TemplateField HeaderText="<%$ resources:ETD %>">
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
                                                    <%--<asp:TemplateField HeaderText="<%$ resources:DueDate %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDueDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SalesDueDate, Resources.Constants.DateFormatGrid) %>'
                                                                ToolTip='<%# Eval(Resources.DataFieldRes.SalesDueDate, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Wrap="false" />
                                                        <ItemStyle Width="6%" Wrap="false" />
                                                    </asp:TemplateField>--%>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <div class="button-wrap-right margntop-minus3 margnbotm10">
                                            <asp:Button ID="btnAddSelectedItems" runat="server" ToolTip="<%$resources:Controls,AddToList %>"
                                                Text="<%$ Resources:Controls, AddToList%>" OnClick="ActionHandler" CommandName="ADDTOLIST"
                                                TabIndex="12" />
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
                                            <asp:Label runat="server" ID="lblMode" Text="<%$ resources:ModeReq%>" AssociatedControlID="ddlMode"></asp:Label>
                                            <asp:DropDownList ID="ddlMode" runat="server" CssClass="select-small-a1" TabIndex="13"
                                                onchange="javascript:ModeChanged();">
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hdfPostedJ" runat="server" />
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="vrfMode" CssClass="star" SetFocusOnError="true" ValidationGroup="drcr"
                                                    EnableClientScript="true" InitialValue="-1" runat="server" ControlToValidate="ddlMode"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Mode %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="vrfMode2" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="splitMain" EnableClientScript="true" InitialValue="-1" runat="server"
                                                    ControlToValidate="ddlMode" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Mode %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <asp:Label runat="server" ID="lblInstrumentNo" Text="<%$ resources:RefNo%>" AssociatedControlID="txtInstrumentNo"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox ID="txtInstrumentNo" runat="server" TabIndex="14" CssClass="input-small margnrgt1-5per"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator ID="vrfInstrumentNo" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="receipt" EnableClientScript="true" runat="server" ControlToValidate="txtInstrumentNo"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_InstrumentNo%>"></asp:RequiredFieldValidator>--%>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblInstrumentDate" Text="<%$ resources:RefDate%>" AssociatedControlID="txtInstrumentDate"
                                                CssClass="middle-lbl-a"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInstrumentDate" CssClass="input-small Uidate-picker"
                                                TabIndex="15" onpaste="return false;" onkeydown="return CheckKey(event)"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator ID="vrfInstrumentDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="receipt" EnableClientScript="true" runat="server" ControlToValidate="txtInstrumentDate"
                                                Display="Dynamic" Text="*" Enabled="false" ErrorMessage="<%$ resources:Err_InstrumentDate %>">
                                            </asp:RequiredFieldValidator>--%>
                                            <%--<asp:TextBox ID="txtExchangeRate" runat="server" Enabled="true" CssClass="medium numeric" />--%>
                                            <asp:Label ID="lblRcptAlcn" runat="server" Text="<%$ resources:ReceiptAlcnReq %>"
                                                AssociatedControlID="chkIsReceiptAlcnReq" CssClass="lbl-35-2perc" />
                                            <asp:CheckBox ID="chkIsReceiptAlcnReq" runat="server" CssClass="disable" TabIndex="16" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <div style="display: none;">
                                                <asp:Label runat="server" ID="lblPaidAmount" Text="<%$ resources:Amount%>" AssociatedControlID="txtPaidAmount"></asp:Label>
                                                <asp:TextBox ID="txtPaidAmount" runat="server" CssClass="date-picker numeric" MaxLength="17"
                                                    Enabled="false"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="vrfPaidAmount" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="drcr" EnableClientScript="true" runat="server" ControlToValidate="txtPaidAmount"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PaidAmount%>"></asp:RequiredFieldValidator>--%>
                                            </div>
                                            <asp:Label ID="lblPaymentCurrency" runat="server" Text="<%$resources:Currency %>"
                                                AssociatedControlID="txtPaymentCurrency"></asp:Label>
                                            <asp:TextBox ID="txtPaymentCurrency" Enabled="false" runat="server" MaxLength="3"
                                                CssClass="input-small input-disabled" TabIndex="17"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfPaymentCurrency" runat="server" Value="" />
                                            <asp:Label ID="lblExchangeRate" runat="server" Text="<%$ resources:ExchangeRateReq %>"
                                                AssociatedControlID="txtExchangeRate" CssClass="middle-lbl" />
                                            <asp:TextBox ID="txtExchangeRate" runat="server" Enabled="true" CssClass="input-small numeric"
                                                TabIndex="18" AutoPostBack="true" OnTextChanged="ActionHandler" onkeypress="return validateRateFloatKeyPress(this,event);" />
                                            <asp:RequiredFieldValidator ID="vrfExchangeRate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="drcr" EnableClientScript="true" runat="server" ControlToValidate="txtExchangeRate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExchangeRate %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblCompanyView" Visible="false" runat="server" Text="<%$ resources:Controls, CompanyPlant %>"
                                                AssociatedControlID="ddlCompanyView"></asp:Label>
                                            <asp:DropDownList ID="ddlCompanyView" Visible="false" Enabled="false" runat="server"
                                                CssClass="select-small-a">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <asp:HiddenField ID="hdfInvoiceType" runat="server" />
                                <asp:HiddenField ID="hdfVendorPK" runat="server" />
                                <asp:HiddenField ID="hdfVendorAccountNo" runat="server" />
                                <asp:HiddenField ID="hdfInvoiceCurr" runat="server" />
                                <asp:HiddenField ID="hdfExchangeRate" runat="server" />
                                <asp:Button runat="server" ID="btnRaiseNote" OnClick="ActionHandler" CommandName="RAISENOTECHANGE"
                                    EnableTheming="false" Style="display: none;" />
                                <asp:GridView ID="grdInvoiceList" runat="server" AutoGenerateColumns="False" PageSize="<%$ resources:PageSize %>"
                                    AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                    OnRowDataBound="ActionHandler" ShowFooter="true" Width="100%">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceNo1 %>" SortExpression="<%$ resources:DataFieldRes,InvoiceNo %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfTotalAmt" runat="server" Value='<%#Eval("ICH_AMOUNT")%>' />
                                                <asp:HiddenField ID="hdfTaxAmt" runat="server" Value='<%#Eval("ICH_TAX_AMOUNT")%>' />
                                                <asp:HiddenField ID="hdfInvoicePK" runat="server" Value='<%#Eval("CDM_INVOICE_CUS_HDR")%>' />
                                                <asp:HiddenField ID="hdfCrDbMpgPK" runat="server" Value='<%#Eval("CDM_PK")%>' />
                                                <asp:HiddenField ID="hdfhasjournalized" runat="server" Value='<%#Eval("ICH_HAS_JRNL_ENTRY")%>' />
                                                <asp:HiddenField ID="hdfInvoiceType" runat="server" Value='<%#Eval("ICH_TYPE")%>' />
                                                <asp:HiddenField ID="hdfInvCategory" runat="server" Value='<%#Eval("ICH_CATEGORY")%>' />
                                                <asp:HiddenField ID="hdfTotalCNAmount" runat="server" Value='<%#Eval("ICH_TOTAL_CN_AMOUNT")%>' />
                                                <asp:HiddenField ID="hdfInvGroup" runat="server" Value='<%#Eval("ICH_GROUP")%>' />
                                                <asp:LinkButton ID="lnkInvoiceNo" runat="server" CssClass="text-underline" OnClick="ActionHandler"
                                                    CommandName="SHOWPOPUP" Text='<%#Eval("ICH_NO")%>' CommandArgument='<%#Eval("CDM_INVOICE_CUS_HDR")%>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceDate1 %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval("ICH_DATE", Resources.Constants.DateFormatGrid)%>'
                                                    ToolTip='<%# Eval("ICH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Customer %>" SortExpression="<%$ Resources:DataFieldRes,POInvoiceVendorText%>"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendorInv" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("ICH_CUSTOMER_TEXT"),20) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("ICH_CUSTOMER_TEXT"))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GrossAmount %>" SortExpression="<%$ resources:DataFieldRes,POInvoiceGrossAmountBC %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrossAmount" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("ICH_AMOUNT"))%>'
                                                    ToolTip='<%# GetFormattedCurrencyWithComma(Eval("ICH_AMOUNT"))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Discount %>" SortExpression="<%$ resources:DataFieldRes,POInvoiceDiscountBC %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDiscount" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("ICH_DISC_AMOUNT"))%>'
                                                    ToolTip='<%# GetFormattedCurrencyWithComma(Eval("ICH_DISC_AMOUNT"))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <%-- Other Charges-------------------------------------------------------------%>
                                        <asp:TemplateField HeaderText="<%$ resources:OthCharges %>" SortExpression="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOtherAmount" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("ICH_SHIP_CHARGE"))%>'
                                                    ToolTip='<%# GetFormattedCurrencyWithComma(Eval("ICH_SHIP_CHARGE"))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <%--End Other Charges---------------------------------------------------------%>
                                        <asp:TemplateField HeaderText="<%$ resources:Tax %>" SortExpression="<%$ resources:DataFieldRes,POInvoiceTaxtBC %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTax" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("ICH_TAX_AMOUNT"))%>'
                                                    ToolTip='<%# GetFormattedCurrencyWithComma(Eval("ICH_TAX_AMOUNT"))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CNAmount %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotCNAmount" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("ICH_TOTAL_CN_AMOUNT"))%>'
                                                    ToolTip='<%# GetFormattedCurrencyWithComma(Eval("ICH_TOTAL_CN_AMOUNT"))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvAmt %>" SortExpression="<%$ resources:DataFieldRes,POInvoiceGrossAmountBC %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceAmount" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("ICH_AMOUNT_NET_TC"))%>'
                                                    ToolTip='<%# GetFormattedCurrencyWithComma(Eval("ICH_AMOUNT_NET_TC"))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TotalAmount %>" SortExpression="<%$ resources:DataFieldRes,POInvoiceNetAmount %>"
                                            ItemStyle-HorizontalAlign="Right" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalAmount" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("ICH_NET_VALUE_TC"))%>'
                                                    ToolTip='<%# GetFormattedCurrencyWithComma(Eval("ICH_NET_VALUE_TC"))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblfooter" Text="<%$ resources:Total %>"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Paid %>" Visible="false" SortExpression="<%$ resources:DataFieldRes,POInvoicePaidAmount %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaid" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("ICH_AMOUNT_RCVD_TC"))%>'
                                                    ToolTip='<%# GetFormattedCurrencyWithComma(Eval("ICH_AMOUNT_RCVD_TC"))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AdjAmount %>" Visible="false" SortExpression=""
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAdjAmount" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("ICH_ADJ_AMT"))%>'
                                                    ToolTip='<%# GetFormattedCurrencyWithComma(Eval("ICH_ADJ_AMT"))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Baltopay %>" Visible="false" SortExpression="<%$ resources:DataFieldRes,POInvoiceBalAmount %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBaltopay" CssClass="BalancetoPay" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("ICH_BAL_TO_PAY"))%>'
                                                    ToolTip='<%# GetFormattedCurrencyWithComma(Eval("ICH_BAL_TO_PAY"))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:RaiseNoteFor %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfInvRowIndex" runat="server" Value='<%# Container.DataItemIndex %>' />
                                                <asp:TextBox ID="txtNoteFor" Enabled="false" onkeyup="CalculateTotal(this);" onblur="RaiseNoteChange(this)"
                                                    runat="server" CssClass="small-a numeric" MaxLength="15" TabIndex="19" Width="80%"
                                                    Text='<%# GetFormattedCurrency(Eval("CDM_AMOUNT"))%>'></asp:TextBox>
                                                <asp:HiddenField ID="hdfItemIncluded" runat="server" Value="0" />
                                                <div class="starwrap">
                                                    <%--<asp:RequiredFieldValidator ID="vrfNot" CssClass="star" SetFocusOnError="true" ValidationGroup="drcr"
                                                        EnableClientScript="true" runat="server" ControlToValidate="txtNoteFor" Display="Dynamic"
                                                        Text="*" ErrorMessage="<%$ resources:Err_NotFor %>">
                                                    </asp:RequiredFieldValidator>--%>
                                                    <cc1:AmountValidation ID="vamNoteFor" runat="server" ControlToValidate="txtNoteFor"
                                                        ErrorMessage="<%$ resources:Err_NotFor1 %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="drcr"></cc1:AmountValidation>
                                                    <cc1:AmountValidation ID="vamNoteFor1" runat="server" ControlToValidate="txtNoteFor"
                                                        ErrorMessage="<%$ resources:Err_NotFor1 %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="splitMain"></cc1:AmountValidation>
                                                    <asp:CustomValidator ID="vcmNoteFor" CssClass="star" SetFocusOnError="true" ClientValidationFunction="ValidateRaiseNote"
                                                        ValidationGroup="drcr" EnableClientScript="true" runat="server" ControlToValidate="txtNoteFor"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExcessRaiseNote %>"></asp:CustomValidator>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalPayNowFooter"></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfPayNowFooter" />
                                            </FooterTemplate>
                                            <FooterStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkAllocation" runat="server" TabIndex="19" OnClick="ActionHandler"
                                                    CommandName="DCDETAIL" SkinID="allocation-icon" ToolTip="Allocation" CommandArgument="PageAction_Entry"
                                                    ValidationGroup="splitMain" OnClientClick="javascript:ValidatePageNow('splitMain')" /><%--OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load"--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Tax %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblTotalTax" runat="server"></asp:Label>--%>
                                                <asp:LinkButton ID="lbnTotalTax" runat="server" OnClick="ActionHandler" CssClass="text-underline nomargin"
                                                    CommandName="TAXHEADERSPLITUP" Text='<%# GetFormattedCurrencyWithComma(Eval("CDM_TAX_AMOUNT"))%>'></asp:LinkButton>
                                                <asp:HiddenField ID="hdfTotalTax" runat="server" Value='<%# GetFormattedCurrency(Eval("CDM_TAX_AMOUNT"))%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalTaxFooter"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <%--Remove--%>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" SkinID="delete-icon" TabIndex="19"
                                                    CommandName="REMOVE" ToolTip="Remove" OnClientClick="return ShowDeleteConfirm(this);" />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div id="divCalc">
                                <div class="gridwrap">
                                    <table id="tblCalc" class="gridwraptable gridwrap">
                                        <tr>
                                            <td style="text-align: right; width: 85%;">
                                                <asp:Label runat="server" ID="lblSubTotal" Text="<%$ resources:SubTotal%>" AssociatedControlID="txtSubTotal"></asp:Label>
                                            </td>
                                            <td style="text-align: right; width: 15%;" class="btn-margin">
                                                <asp:TextBox ID="txtSubTotal" Text="0" runat="server" CssClass="input-w80 numeric  input-disabled"
                                                    Enabled="false" TabIndex="20" MaxLength="16"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="vrfShipping" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="drcr" EnableClientScript="true" runat="server" ControlToValidate="txtShipCharge"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Shipping%>"></asp:RequiredFieldValidator>--%>
                                                <%--  <cc1:AmountValidation ID="AmountValidation1" runat="server" ControlToValidate="txtSubTotal"
                                                    ErrorMessage="<%$ resources:Err_Invalid_Subtotal %>" NumberDigits="12" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="drcr"></cc1:AmountValidation>--%>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label runat="server" ID="lblTax" Text="<%$ resources:TotalTax%>" AssociatedControlID="txtTax"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:TextBox ID="txtTax" Text="0" runat="server" CssClass="input-w80 numeric  input-disabled"
                                                    Enabled="false" TabIndex="21" MaxLength="16"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="vrfShipping" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="drcr" EnableClientScript="true" runat="server" ControlToValidate="txtShipCharge"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Shipping%>"></asp:RequiredFieldValidator>--%>
                                                <%--<cc1:AmountValidation ID="AmountValidation2" runat="server" ControlToValidate="txtTax"
                                                    ErrorMessage="<%$ resources:Err_Invalid_Tax %>" NumberDigits="12" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="drcr"></cc1:AmountValidation>--%>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label runat="server" ID="lblShipCharge" Text="<%$ resources:ShipCharge%>" AssociatedControlID="txtShipCharge"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:TextBox ID="txtShipCharge" Text="0" runat="server" onkeyup="CalculateTotalSHP(this);"
                                                    CssClass="input-w80 numeric" TabIndex="22" MaxLength="16"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="vrfShipping" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="drcr" EnableClientScript="true" runat="server" ControlToValidate="txtShipCharge"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Shipping%>"></asp:RequiredFieldValidator>--%>
                                                <cc1:AmountValidation ID="vamShipping" runat="server" ControlToValidate="txtShipCharge"
                                                    ErrorMessage="<%$ resources:Err_Invalid_Shipping %>" NumberDigits="12" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="drcr"></cc1:AmountValidation>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblOtherCharge" Text="<%$ resources:OtherCharge%>"
                                                    AssociatedControlID="txtOtherCharge"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <%--Biju--%>
                                                <asp:ImageButton ID="imgShippingCharge" SkinID="shipping" runat="server" OnClick="ActionHandler"
                                                    TabIndex="23" ToolTip="<%$ resources:OtherCharges %>" CommandName="SHIPPINGHEADER" />&nbsp;
                                                <asp:TextBox ID="txtOtherCharge" Text="0" onkeyup="CalculateTotalSHP(this);" runat="server"
                                                    CssClass="input-w80 numeric  input-disabled" Enabled="false" TabIndex="24" MaxLength="16"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfOtherCharge" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="drcr" EnableClientScript="true" runat="server" ControlToValidate="txtOtherCharge"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_OtherCharge%>"></asp:RequiredFieldValidator>
                                                <cc1:AmountValidation ID="vamOtherCharge" runat="server" ControlToValidate="txtOtherCharge"
                                                    ErrorMessage="<%$ resources:Err_Invalid_OtherCharge %>" NumberDigits="12" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="drcr"></cc1:AmountValidation>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblTotal" Text="<%$ resources:Total%>" AssociatedControlID="txtHdrTotal"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtHdrTotal" runat="server" Text="0" CssClass="input-w80 numeric input-disabled"
                                                    Enabled="false" MaxLength="16"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <table class="table-devide">
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:RemarksDescriptionReq %>"
                                                AssociatedControlID="txtRemarks"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRemarks" TabIndex="25" TextMode="MultiLine" CssClass="multiline-2line"
                                                onkeydown="limitText(this,400);" onkeyup="limitText(this,400);"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfRemarks" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="drcr" EnableClientScript="true" runat="server" ControlToValidate="txtRemarks"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_RemarksDescription%>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblRemarks2" Text="<%$ resources:Remarks %>" AssociatedControlID="txtRemarks2"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRemarks2" TabIndex="26" TextMode="MultiLine" CssClass="multiline-2line"
                                                onkeydown="limitText(this,400);" onkeyup="limitText(this,400);"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div id="divItemTax" style="display: none">
                                <div class="Button-container-popup">
                                    <asp:Button ID="btnApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply" OnClick="ActionHandler"
                                        TabIndex="28" CommandName="TAXAPPLY" CommandArgument="PageAction_Entry" />
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
                                                        EnableViewState="false" Enabled="false" MaxLength="11" TabIndex="29"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblPopupAmount" runat="server" Text="<%$ resources:Charges %>" AssociatedControlID="txtPopupAmount"></asp:Label>
                                                    <asp:TextBox ID="txtPopupAmount" TabIndex="31" runat="server" CssClass="input-w70 numeric"
                                                        MaxLength="11"></asp:TextBox>
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
                                                        <asp:DropDownList ID="ddlPopupTaxType" TabIndex="30" runat="server" CssClass="medium"
                                                            EnableViewState="true" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <div class="clear">
                                                        </div>
                                                    </div>
                                                    <asp:Label ID="lblPopupOther" runat="server" Text="<%$ resources:TaxName %>" AssociatedControlID="txtPopupOther"></asp:Label>
                                                    <asp:TextBox ID="txtPopupOther" runat="server" TabIndex="32" CssClass="medium" EnableViewState="false"
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
                                                        <asp:HiddenField ID="hdfTaxPK" runat="server" Value='<%#Eval("NTH_PK") %>' />
                                                        <asp:Label ID="lblTaxText" runat="server" Text='<%# Convert.ToString(Eval("NTH_TAX")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("NTH_TAX")) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("NTH_TAX")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("NTH_TAX"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:TaxName %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxName" runat="server" Text='<%#Eval("NTH_NAME") %>' ToolTip='<%# HttpUtility.HtmlDecode(Eval("NTH_NAME").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTaxName" runat="server" Value='<%#Eval("NTH_NAME") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Amount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("NTH_TAX_AMT")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithComma(Eval("NTH_TAX_AMT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="22%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imbTaxRemove" runat="server" OnClick="ActionHandler" CommandName="TAXDELETE"
                                                            CommandArgument="PageAction_Entry" OnLoad="btnAction_Load" OnPreRender="btnAction_PreRender"
                                                            TabIndex="33" SkinID="btnclose" ToolTip="Remove" />
                                                        <%--OnPreRender="btnAction_PreRender"--%>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="8%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <%---------------------------------Start TAX Payable in MYR-------------------------------------%>
                            <div id="divOuterTaxPayable">
                                <h1 class="search-colapse-normal">
                                    <%=Resources.Controls.TaxDetails %>
                                    <img id="imgShowTaxPayable" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                                        alt="<%= Resources.Controls.Show%>" title="<%= Resources.Controls.Show%>" style="display: none;
                                        cursor: pointer" onclick="javascript:ShowTaxPayable();" />
                                    <img id="imgHideTaxPayable" src="../Images/Classic/Icons/arrow-colapse-active.png"
                                        alt="<%= Resources.Controls.Hide%>" title="<%= Resources.Controls.Hide%>" style="cursor: pointer"
                                        onclick="javascript:HideTaxPayable();" />
                                </h1>
                                <div id="divTaxPayable" class="gridwrap">
                                    <asp:GridView runat="server" ID="grdTaxPayable" Width="100%" AllowSorting="false"
                                        AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:TaxCode %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxHdrCode" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_CODE")) == string.Empty ? string.Empty : Convert.ToString(Eval("CIT_TAX_CODE")) %>'
                                                        ToolTip='<%# Convert.ToString(Eval("CIT_TAX_CODE")) == string.Empty ? string.Empty : Convert.ToString(Eval("CIT_TAX_CODE")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:TaxName %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxHdrName" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("CIT_TAX_TEXT")) %>'
                                                        ToolTip='<%# Convert.ToString(Eval("CIT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("CIT_TAX_TEXT")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="30%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:TaxRate %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxRate" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_RATE")) == "0" ? string.Empty : Convert.ToString(Eval("CIT_TAX_RATE")+"%") %>'
                                                        ToolTip='<%# Convert.ToString(Eval("CIT_TAX_RATE")) == "0" ? string.Empty : Convert.ToString(Eval("CIT_TAX_RATE")+"%") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:SubTotal %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSubTotal" runat="server" Text='<%# GetFormattedCurrencyWithComma(Convert.ToDouble(Eval("CIT_TAX_CID_AMOUNT")))%>'
                                                        ToolTip='<%# GetFormattedCurrencyWithComma(Convert.ToDouble(Eval("CIT_TAX_CID_AMOUNT")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:TaxAmount %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxAmountBC" runat="server" Text='<%# GetFormattedCurrencyWithComma(Convert.ToDouble(Eval("CIT_TAX_AMT")))%>'
                                                        ToolTip='<%# GetFormattedCurrencyWithComma(Convert.ToDouble(Eval("CIT_TAX_AMT")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" Wrap="false" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <%------------------------------- End TAX Payable in MYR--------------------------------------%>
                            <%---------------------------------Start TAX Splitup Popup-----------------------------------%>
                            <div id="divTaxSplitup" style="display: none">
                                <div class="content-wrapper">
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdTaxSplitup" Width="100%" AllowSorting="false"
                                            AutoGenerateColumns="false" TabIndex="106" EmptyDataRowStyle-CssClass="emptytable"
                                            PageSize="<%$ resources:PageSize%>">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:TaxCode %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxCode" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_CODE")) %>'
                                                            ToolTip='<%# Convert.ToString(Eval("CIT_TAX_CODE")) == string.Empty ? string.Empty : HttpUtility.HtmlDecode(Eval("CIT_TAX_CODE").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTaxCode" runat="server" Value='<%# Convert.ToString(Eval("CIT_TAX_CODE")) %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <%--  <asp:TemplateField HeaderText="<%$ resources:TaxType %>" Visible="false">
                                                    <ItemTemplate>                                                       
                                                                                                  
                                                        <asp:Label ID="lblTaxText" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("CIT_TAX_TEXT")) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("CIT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("CIT_TAX_TEXT"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="<%$ resources:TaxName %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxName" runat="server" Text='<%# Convert.ToString(Eval("CIT_NAME")) == string.Empty ? Resources.Report.Custom : Eval("CIT_NAME") %>'
                                                            ToolTip='<%# Convert.ToString(Eval("CIT_NAME")) == string.Empty ? Resources.Report.Custom : HttpUtility.HtmlDecode(Eval("CIT_NAME").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTaxName" runat="server" Value='<%# Convert.ToString(Eval("CIT_NAME")) == string.Empty ? Resources.Report.Custom : Eval("CIT_NAME") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Amount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("CIT_TAX_AMT")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithComma(Eval("CIT_TAX_AMT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <%---------------------------------End TAX Splitup Popup-------------------------------------%>
                            <%---------------------------------Start TAX SplitupLineItemwise Popup-----------------------------------%>
                            <div id="divTaxSplitupLIneItem" style="display: none">
                                <div class="content-wrapper">
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdTaxSplitupLineItem" Width="100%" AllowSorting="false"
                                            AutoGenerateColumns="false" TabIndex="34" EmptyDataRowStyle-CssClass="emptytable"
                                            PageSize="<%$ resources:PageSize%>">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:TaxCode %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxCode" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_CODE")) %>'
                                                            ToolTip='<%# Convert.ToString(Eval("CIT_TAX_CODE")) == string.Empty ? string.Empty : HttpUtility.HtmlDecode(Eval("CIT_TAX_CODE").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTaxCode" runat="server" Value='<%# Convert.ToString(Eval("CIT_TAX_CODE")) %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <%-- <asp:TemplateField HeaderText="<%$ resources:TaxType %>" Visible="false" >
                                                    <ItemTemplate>                                                       
                                                                                                  
                                                        <asp:Label ID="lblTaxText" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("CIT_TAX_TEXT")) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("CIT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("CIT_TAX_TEXT"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="25%" />
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="<%$ resources:TaxName %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxName" runat="server" Text='<%# Convert.ToString(Eval("CIT_NAME")) == string.Empty ? Resources.Report.Custom : Eval("CIT_NAME") %>'
                                                            ToolTip='<%# Convert.ToString(Eval("CIT_NAME")) == string.Empty ? Resources.Report.Custom : HttpUtility.HtmlDecode(Eval("CIT_NAME").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTaxName" runat="server" Value='<%# Convert.ToString(Eval("CIT_NAME")) == string.Empty ? Resources.Report.Custom : Eval("CIT_NAME") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Amount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("CIT_TAX_AMT")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithComma(Eval("CIT_TAX_AMT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <%---------------------------------End TAX SplitupLineItemwise Popup-------------------------------------%>
                            <%---------------------------------Start Balance Amount details Popup-----------------------------------%>
                            <div id="divBalAmntSplitup" style="display: none">
                                <div class="content-wrapper">
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdBalAmntSplitup" Width="100%" AllowSorting="false"
                                            AutoGenerateColumns="false" TabIndex="106" EmptyDataRowStyle-CssClass="emptytable"
                                            OnRowDataBound="ActionHandler" ShowFooter="true">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:TrxNo %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTrxNo" runat="server" Text='<%# Convert.ToString(Eval("TRX_NO")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="50%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate" runat="server" Text='<%#  Eval("TRX_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("TRX_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                            ToolTip='<%# Eval("TRX_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:TrxAmount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("TRX_AMOUNT")) %>'
                                                            ToolTip='<%#GetFormattedCurrency(Eval("TRX_AMOUNT")) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfAmountSplit" runat="server" Value='<%#Eval("TRX_AMOUNT") %>' />
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
                            <%---------------------------------End Balance Amount details Popup-------------------------------------%>
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
                                                                <HeaderStyle HorizontalAlign="Center" />
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
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
                <asp:HiddenField ID="hdfCrDrTrxNo" runat="server" Value="" />
                <div id="divCrDrSplitUp" style="display: none">
                    <div class="content-wrapper">
                        <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
                        <asp:HiddenField ID="hdfCurrencyDigits" runat="server" Value="3" />
                        <asp:Panel runat="server" ID="Allocation_Section" CssClass="Button-container-popup">
                            <asp:Button ID="btnSaveDCSplit" runat="server" Text="<%$ resources:Controls,Apply %>"
                                ToolTip="<%$ resources:Controls,Apply %>" OnClick="ActionHandler" TabIndex="37"
                                CommandName="DCSPLITSAVE" SkinID="btnInner-add-dsd" OnClientClick="javascript:ValidatePageNow('split')"
                                ValidationGroup="split" CommandArgument="Allocation_Section" />
                        </asp:Panel>
                        <div class="detail-co3">
                            <div class="div3col-S">
                                <asp:Label ID="LabelDC" runat="server" Text="<%$resources:InvoiceNoDC %>" AssociatedControlID="lblDCSplitNo"
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblDCSplitNo" runat="server" CssClass="medium"></asp:Label>
                                <br />
                                <asp:Label ID="Label6" runat="server" Text="<%$resources:AmountDC %>" AssociatedControlID="lblDCSplitAmount"
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblDCSplitAmount" runat="server" CssClass="medium"></asp:Label>
                            </div>
                            <div class="detail-co3-2">
                                <asp:Label ID="Label2" runat="server" Text="<%$resources:DateDC %>" AssociatedControlID="lblDCSplitDate"
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblDCSplitDate" runat="server" CssClass="medium"></asp:Label>
                                <br />
                                <div class=" clear">
                                </div>
                                <asp:Label ID="lblCustomerDC" runat="server" Text="<%$resources:CustomerDC %>" Font-Bold="true"
                                    AssociatedControlID="lblDCSplitSupplier"></asp:Label>
                                <asp:Label ID="lblDCSplitSupplier" runat="server"></asp:Label>
                                <asp:Label ID="Label8" runat="server" Visible="false" Text="<%$resources:ReceivedDC %>"
                                    AssociatedControlID="lblDCSplitReceived"></asp:Label>
                                <asp:Label ID="lblDCSplitReceived" Visible="false" runat="server"></asp:Label>
                                <asp:Label ID="Label10" runat="server" Visible="false" Text="<%$resources:ReceiveNowDC %>"
                                    AssociatedControlID="lblDCSplitReceiveNow"></asp:Label>
                                <asp:Label ID="lblDCSplitReceiveNow" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lbltaxSplitpopup" runat="server" Visible="false"></asp:Label>
                                <%--</div>
                              <div class="div3col-S">--%><%--   <asp:Label ID="Label4" runat="server" Text="<%$resources:CustomerDC %>" AssociatedControlID="lblDCSplitSupplier"
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblDCSplitSupplier" runat="server" CssClass="medium"></asp:Label>--%>
                            </div>
                            <div class=" clear">
                            </div>
                        </div>
                        <div class="error" id="divErrorLabel" runat="server" visible="false">
                            <ul>
                                <li>
                                    <asp:Literal runat="server" ID="lblSplitErrorMessage" Text="<%$resources:error_allocation %>"></asp:Literal></li></ul>
                        </div>
                        <div class="gridwrap">
                            <asp:TableCell>
                                <div class="gridwrap">
                                    <asp:GridView ID="grdDCSplit" runat="server" AutoGenerateColumns="False" Width="100%"
                                        PageSize="<%$ resources:PageSize %>" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable"
                                        AllowSorting="false" ShowFooter="true" OnRowDataBound="ActionHandler">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label></EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$resources:PRODUCT %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPRODUCTSplit" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CDS_ITEM_TEXT"),15) %>'
                                                        ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("CDS_ITEM_TEXT")) %>'></asp:Label>
                                                    <asp:HiddenField ID="hdfDCSplitPK" runat="server" Value='<%# Eval("CDS_PK") %>' />
                                                    <asp:HiddenField ID="hdfInvCusDtlPK" runat="server" Value='<%# Eval("CDS_INVOICE_CUS_DTL") %>' />
                                                    <asp:HiddenField ID="hdfDCTRXPK" runat="server" Value='<%# Eval("CDS_INVOICE_CUS_DTL") %>' />
                                                    <asp:HiddenField ID="hdfCDSTaxPercentage" runat="server" Value='<%#Eval("CDS_TAX_PERC")%>' />
                                                    <asp:HiddenField ID="hdfCDSSlNo" runat="server" Value='<%# Eval("CDS_SL_NO") %>' />
                                                    <asp:HiddenField ID="hdfSODtlPk" runat="server" Value='<%# Eval("CDS_SO_DTL") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="22%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:Uom %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUomSales" runat="server" Text='<%# Eval("CDS_SALE_UOM_TEXT") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="6%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:DCQTY %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQTYSplit" runat="server" Text='<%# Eval("CDS_INVOICED_QTY") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="13%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:DCQTY %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQTYSplitUom" runat="server" Text='<%# Eval("CDS_CID_QTY_INVOICED") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="13%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:RATE %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRATESplit" runat="server" Text='<%# GetFormattedRate(Eval("CDS_CID_RATE")) %>'
                                                        ToolTip='<%# GetFormattedRate(Eval("CDS_CID_RATE")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="13%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:Amount %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmountSplit" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("CDS_CID_AMOUNT")) %>'
                                                        ToolTip='<%# GetFormattedCurrencyWithComma(Eval("CDS_CID_AMOUNT")) %>'></asp:Label>
                                                    <asp:HiddenField ID="hdfNETSplit" runat="server" Value='<%# GetFormattedCurrency(Eval("CDS_CID_AMOUNT")) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:Discount %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDiscountSplit" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("CDS_CID_DISCOUNT")) %>'
                                                        ToolTip='<%# GetFormattedCurrencyWithComma(Eval("CDS_CID_DISCOUNT")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:TAX %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTAXSplit" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("CDS_CID_TAX")) %>'
                                                        ToolTip='<%# GetFormattedCurrencyWithComma(Eval("CDS_CID_TAX")) %>'></asp:Label>
                                                    <asp:HiddenField ID="hdfTAXSplit" runat="server" Value='<%# GetFormattedCurrency(Eval("CDS_CID_TAX")) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:NET %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNETSplit" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("CDS_CID_NET_AMOUNT")) %>'
                                                        ToolTip='<%# GetFormattedCurrencyWithComma(Eval("CDS_CID_NET_AMOUNT")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="13%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:QTY %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtQtySplit" runat="server" Width="13px" CssClass="small numeric"
                                                        onblur="CalculateAmount(this);" MaxLength="13" TabIndex="36" Text='<%#GetFormattedNumber(Eval("CDS_QTY")) %>'
                                                        ToolTip='<%#GetFormattedNumber(Eval("CDS_QTY")) %>'></asp:TextBox>
                                                    <asp:HiddenField ID="hdfQtySplit" runat="server" Value='<%# Eval("CDS_QTY") %>' />
                                                    <cc1:QuantityValidation ID="vreQtySplit" runat="server" ControlToValidate="txtQtySplit"
                                                        NumberDigits="9" ErrorMessage="<%$ resources:Err_Invalid_Qty %>" Display="Dynamic"
                                                        Text="*" NonZero="false" EnableClientScript="true" CssClass="star" ValidationGroup="split"></cc1:QuantityValidation>
                                                </ItemTemplate>
                                                <ItemStyle Width="11%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:RATE %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRateSplit" TabIndex="36" runat="server" CssClass="input-w80 numeric"
                                                        onblur="CalculateAmount(this);" MaxLength="15" Text='<%#GetFormattedRate(Eval("CDS_RATE")) %>'></asp:TextBox>
                                                    <asp:HiddenField ID="hdfRateSplit" runat="server" Value='<%#Eval("CDS_RATE")%>' />
                                                    <cc1:RateValidation ID="vreRate" runat="server" ControlToValidate="txtRateSplit"
                                                        DecimalDigits="8" ErrorMessage="<%$ resources:Err_Invalid_Rate %>" NumberDigits="10"
                                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="split"
                                                        NonZero="false"></cc1:RateValidation>
                                                </ItemTemplate>
                                                <ItemStyle Width="16%" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:SUM %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtSumSplit" Text='<%#GetFormattedCurrency(Eval("CDS_AMOUNT")) %>'
                                                        onblur="CalculateAmount(this);" runat="server" CssClass="input-w80 input-disabled numeric"
                                                        MaxLength="15" TabIndex="36"> </asp:TextBox>
                                                    <asp:HiddenField ID="hdfSumSplit" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalPayNowFooterSplit"></asp:Label><asp:HiddenField
                                                        runat="server" ID="hdfTotalPayNowFooterSplit" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:TAX %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbnTAXSplitTotal" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("CDS_TAX")) %>'
                                                        CssClass="text-underline nomargin" OnClick="ActionHandler" CommandName="TAXLINEITEMSPLITUP"></asp:LinkButton>
                                                    <asp:HiddenField ID="hdfTAXSplitTotal" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" Wrap="false" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalTaxFooterSplit"></asp:Label><asp:HiddenField
                                                        runat="server" ID="hdfTotalTaxFooterSplit" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:TableCell></div>
                    </div>
                </div>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="drcr" runat="server" />
                    <asp:ValidationSummary ID="vsSplit" ValidationGroup="split" runat="server" />
                    <asp:ValidationSummary ID="vsSplitMain" ValidationGroup="splitMain" runat="server" />
                    <asp:ValidationSummary ID="vsTax" ValidationGroup="tax" runat="server" />
                    <asp:ValidationSummary ID="vsUpload" ValidationGroup="upload" runat="server" />
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
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="drcr">
                </uc1:WorkflowUserComments>
            </div>
            <asp:Button runat="server" ID="btnShowDetailSplit" OnClick="ActionHandler" CommandName="SHOWINVITEMS"
                EnableTheming="false" Style="display: none;" />
            <%--Biju--%>
            <asp:HiddenField ID="hdfTaxCategory" runat="server" />
            <%--Biju--%>
            <asp:HiddenField ID="hdfJournalizeWorkFlow" Value="0" runat="server" />
            <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
            <asp:HiddenField ID="hdfRateFormat" Value="0" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormat" Value="0" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormatWithSeperator" runat="server" />
            <asp:HiddenField runat="server" ID="hdfTotalPayNowFooter" />
            <asp:HiddenField ID="hdfJournalHeader" runat="server" />
            <asp:HiddenField ID="hdfIsJournalize" runat="server" />
            <asp:HiddenField ID="hdfBaseCurrency" runat="server" />
            <asp:HiddenField ID="hdfIsTaxPayable" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIscontYes" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIscontNo" runat="server" Value="0" />
            <asp:HiddenField ID="hdfSplitCount" runat="server" Value="0" />
            <asp:HiddenField ID="hdfRowIndex" runat="server" />
            <asp:HiddenField ID="hdfExRateBizUnit" runat="server" Value="0" />
            <asp:HiddenField runat="server" ID="IsTaxForOtherCharge" Value="0" />
            <asp:HiddenField runat="server" ID="hdfIsAdvInvHasTax" Value="1" />
            <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
            <asp:HiddenField ID="hdfCurrencyGroup2" Value="2" runat="server" />
            <asp:HiddenField ID="hdfIsMultiplePlant" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIsLineItemTaxEnabled" Value="0" runat="server" />
            <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
            <asp:HiddenField ID="AST_CODE" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
