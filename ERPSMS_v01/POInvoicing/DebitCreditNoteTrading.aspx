<%@ Page Title="<%$ Resources:Captions,Title_POCrDrNoteTrading %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="DebitCreditNoteTrading.aspx.cs" Inherits="ERPSMS_v01.POInvoicing.DebitCreditNoteTrading" 
         Theme="ClassicExt" %>

<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/Journalize/UserControls/JournalizeControlNew.ascx" TagName="Journalize"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
       
        function InitComponents() {           
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendor", url, "hdfVendorID", true, true, "VENDOR");
            if ($("[id$=ddlMode]").val() > 0) {
                GrandScriptUtils.MakeAutoCompleteDDL("txtBank", url + "?Type=" + $("[id$=ddlMode]").val(), "hdfBank", true, true, "BANK");
            }
            GrandScriptUtils.DatePickerCommon("txtDate");
            GrandScriptUtils.DatePickerCommon("txtInstrumentDate");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCrDrNumber", url + "?Type=1", "hdfCrDrNumber", true, true, "GETDRCRNOTRADINGAUTO"); 
            GrandScriptUtils.AddDateRangeCommon("txtSearchDateFrom", "hdfSearchDateFrom", "txtSearchDateTo", "hdfSearchDateTo", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendorHd", url, "hdfVendorHd", true, true, "VENDOR");
            GrandScriptUtils.DatePickerCommon("txtPVDate");
            GrandScriptUtils.AddDateRangeCommon("txtPendingFromDate", "hdfPendingFromDate", "txtPendingToDate", "hdfPendingToDate", false, false);                 
            $("[id*=txtExchangeRate]").ForceNumericOnly();
            $("[id*=txtNoteFor]").ForceNumericOnly();
            $("[id*=txtTAXSplitTotal]").ForceNumericOnly();
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();
            if ($('[id$=btnJournalSaveSubmit]').is(":visible"))
                $('[id$=btnJournalSubmit]').hide();

            //Set a stamp for cancelled invoice
            if ($("[id$=hdfIsCancelled]").val() == "1")
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
            else
                $("[id$=tblDetailHdr]").addClass("table-devide");

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
            if ($("[id$=txtPurchaseInvNumber]").attr("disabled") == true) {
                DisableAuto($("[id$=txtPurchaseInvNumber]"), $("[id$=hdfPurchaseInvPK]"));
            }
        }
        function InitPInvNumberAuto() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtPurchaseInvNumber", url + "?customerid=" + $("[id$=hdfVendorHd]").val() + "&InvCategory=" + $("[id$=ddlPendingInvCategory]").val() + "&InvType=" + $("[id$=ddlPendingInvType]").val(), "hdfPurchaseInvPK", true, true, "PENDINGPURCHASEINVNO");          
            ResetPInvNumberAuto();
        }
        function ResetPInvNumberAuto() {
            var defText = '<%= Resources.ErpRes.AutoDefaultValue %>';
            $("[id$=txtPurchaseInvNumber]").val(defText);
            $("[id$=hdfPurchaseInvPK]").val('-1');
        }
        function ScrollDown() {
            window.scroll(400, 800);
            return false;
        }
        function AfterClose(containerID) {
            if (containerID == "[id$=divJournalize]") {
                $("[id$=btnJournalizeUpdate]").click();
            }
            else if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
                if ($("[id$=hdfJournalizeWorkFlow]").val() == "1") {                 
                    ShowCommonCotainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), "1%");
                    AfterCloseWkfInJournal();                   
                }
            }
            else if (containerID == "[id$=divTemplate]") {               
                ShowCommonCotainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), "1%");
            }
            else if (containerID == "[id$=divTaxSplitupLIneItem]") {
                //  Showing Splitup Popup
                CalculateAmount();
                CalculateTotalSplit();
                ShowContainerDiv('[id$=divCrDrSplitUp]', '<%= GetLocalResourceObject("ReceiptSplit").ToString()%>', '900', '300');
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
                $("[id$=pnlSave]").hide();
                $("[id$=pnlDelete]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlPrint]").hide();
                $("[id$=btnListPrint]").hide();
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
            if (targetControlID == "txtVendorHd") {
                $("[id$=btnVendor]").click();
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
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteInvalidSelect(targetControlID);
            }
            if (targetControlID == "txtCrDrNumber") {
                $("[id$=hdfCrDrNumber]").val("0");
                $("[id$=txtCrDrNumber]").val("<%= Resources.Messages.AutoDefaultValue %>");
            }
            if (targetControlID == "txtVendorHd") {
                $("[id$=btnVendor]").click();
            }
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
            if (!isNaN(parseFloat($("#[id*=hdfExchangeCurr]").val()))) {
                ExchangeRate = parseFloat($("#[id*=hdfExchangeCurr]").val());
            }         
            var qty = 0;
            var rate = 0;     
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
            if (((amount.toFixed(DecimalDigits)).length) <= 15) {
                if (amount > 0) {
                    $(sender).closest('tr').find("#[id*=txtSumSplit]").val(toFixed(amount, DecimalDigits));
                }
                else {
                    if (!isNaN(parseFloat($(sender).closest('tr').find("#[id*=txtSumSplit]").val()))) {
                        var number = Number($(sender).closest('tr').find("#[id*=txtSumSplit]").val().replace(/[^0-9\.]+/g, ""));
                        amount = parseFloat(number);
                    }
                }

                taxpercentage = hdftax / (netamount == 0 ? 1 : netamount);            
                ttaxamt = (amount * taxpercentage);
                grndTotalTax = grndTotalTax + ttaxamt;            
                $(sender).closest('tr').find("#[id*=txtTAXSplitTotal]").val(toFixed(ttaxamt, DecimalDigits));
                $(sender).closest('tr').find("#[id*=hdfTAXSplitTotal]").val(toFixed(ttaxamt, DecimalDigits));
                CalculateTotalSplit($(sender).closest('tr').find("#[id*=txtSumSplit]").val());

            }
            //If Qty is zero, uncheck and disable stock affect checkbox
            if (qty == 0) {
                $(sender).closest('tr').find("#[id*=chkAffectStkSplit]").removeAttr("checked");
                $(sender).closest('tr').find("#[id*=chkAffectStkSplit]").attr("disabled", "disabled");
                var grid = $(sender).closest("table");
                var chkHeader = $("[id*=chkAffectStkHdr]", grid);
                $("td", $(sender).closest("tr")).removeClass("selected");
                chkHeader.removeAttr("checked");
            }
            else {
                var invGroup = parseInt($("#[id*=hdfInvGroup]").val())
                if (invGroup != 3) { // 3:Expense invoice
                    $(sender).closest('tr').find("#[id*=chkAffectStkSplit]").removeAttr("disabled");
                }
            }           
            CalculateTotalTaxSplit();          
        }

        function CalculateTotalSplit(sender) {
            var val1 = parseFloat($(sender).val());
            var Amount = 0.00;
            var BalancetoPay = 0;
            var DecimalDigits = 0;
            var CurrencyFormat = 0;

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
                    }
                }
            });

            $("#[id*=grdDCSplit] [id*=lblTotalPayNowFooterSplit]").html(toFixed(Amount, DecimalDigits));
            $("#[id*=grdDCSplit] [id*=hdfTotalPayNowFooterSplit]").val(toFixed(Amount, DecimalDigits));

        }
        function RaiseNoteChange(sender) {
            var itemindex = 0;
            itemindex = $(sender).closest('tr').find('[id*=hdfInvRowIndex]').val();
            $("#[id*=hdfRowIndex]").val(itemindex);
            $("[id$=btnRaiseNote]").click();
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
            var TotalPayNowFooter = 0;
            var ItemIncluded = 0;
            var IsDetailTaxExist = 0;

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

                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=hdfIsDetailTaxExist]").val()))) {
                    var number = Number($(this).closest('tr').find("#[id*=hdfIsDetailTaxExist]").val().replace(/[^0-9\.]+/g, ""));
                    IsDetailTaxExist = parseInt(number);
                }

                //Check if number is not empty
                if ($.trim($(this).val()) != "") {
                    //Check if number is a valid integer
                    if (!isNaN(parseFloat($(this).val()))) {
                      
                        taxpercentage = hdftax / (hdftotalamt == 0 ? 1 : hdftotalamt);
                        tamt = parseFloat($(this).val());

                        if (ItemIncluded == 1) {
                            ttaxamt = tamt * taxpercentage;
                        }
                        else {
                            basevalue = (tamt) / (1 + taxpercentage);
                            ttaxamt = (tamt - basevalue);
                        }                    
                        if (IsDetailTaxExist > 0) { // if detail tax exist then no need to calculate tax.Just apply from popup
                            if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=hdfTotalTax]").val()))) {
                                var number = Number($(this).closest('tr').find("#[id*=hdfTotalTax]").val().replace(/[^0-9\.]+/g, ""));
                                ttaxamt = parseFloat(number);
                            }


                        }
                        else {                           
                            $(this).closest('tr').find("#[id*=lbnTotalTax]").text(addCommas(toFixed(ttaxamt, DecimalDigits)));
                            $(this).closest('tr').find("#[id*=lbnTotalTax]").attr("title", addCommas(toFixed(ttaxamt, DecimalDigits)));
                            $(this).closest('tr').find("#[id*=hdfTotalTax]").val(toFixed(ttaxamt, DecimalDigits));
                        }
                        Amount = Amount + parseFloat($(this).val());
                        TotalTaxFooter = TotalTaxFooter + ttaxamt;
                        //}
                    }
                }
            });

            var TotalTax = 0;
            var SubTotal = 0;
            var SplitCount = 0;
            if (!isNaN(parseInt($("#[id*=hdfSplitCount]").val()))) {
                SplitCount = parseInt($("#[id*=hdfSplitCount]").val());
            }

            if (!isNaN(TotalTaxFooter || !isNaN(Amount))) {
                if (SplitCount == 0) {
                    SubTotal = Amount - TotalTaxFooter;
                }
                else {
                    SubTotal = Amount;
                }
                $("[id$=txtSubTotal]").val(toFixed(SubTotal, DecimalDigits));
                $("[id$=txtTax]").val(toFixed(TotalTaxFooter, DecimalDigits));
                var Totalamount = SubTotal + TotalTaxFooter;
                if (((Totalamount.toFixed(DecimalDigits)).length) <= 15) {
                    if (!isNaN(Totalamount)) {
                        $("[id$=txtHdrTotal]").val(toFixed(Totalamount, DecimalDigits));
                    }
                }
            }
            $("#[id*=grdInvoiceList] [id*=lblTotalPayNowFooter]").html(addCommas(toFixed(Amount, DecimalDigits)));
            $("#[id*=grdInvoiceList] [id*=lblTotalTaxFooter]").html(addCommas(toFixed(TotalTaxFooter, DecimalDigits)));          
            $("#[id*=txtPaidAmount]").val(toFixed(Amount, DecimalDigits));
            $("#[id$=hdfTotalPayNowFooter]").val(toFixed(Amount, DecimalDigits));
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
            $("#[id*=grdBalAmntSplitup] [id*=lblTotalAmountSplit]").html(addCommas(toFixed(TotalAmountSplit, CurrencyDigits)));

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
        
    </script>
    <script type="text/javascript">
        $("[id*=chkAffectStkHdr]").live("click", function () {
            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (chkHeader.is(":checked")) {
                    if (!$(this).is(':disabled')) {//do not checked if it's disabled
                        $(this).attr("checked", "checked");
                        $("td", $(this).closest("tr")).addClass("selected");
                    }
                } else {
                    $(this).removeAttr("checked");
                    $("td", $(this).closest("tr")).removeClass("selected");
                }
            });
        });
        $("[id*=chkAffectStkSplit]").live("click", function () {
            var grid = $(this).closest("table");
            var chkHeader = $("[id*=chkAffectStkHdr]", grid);
            if (!$(this).is(":checked")) {
                $("td", $(this).closest("tr")).removeClass("selected");
                chkHeader.removeAttr("checked");
            } else {
                $("td", $(this).closest("tr")).addClass("selected");
                if ($("[id*=chkAffectStkSplit]", grid).length == $("[id*=chkAffectStkSplit]:checked", grid).length) {
                    chkHeader.attr("checked", "checked");
                }
            }
        });
        
        function CalculateTotalTaxSplit() {
            var TotalTaxSplit = 0;
            $("#[id*=grdDCSplit] input[type=text][id*=txtTAXSplitTotal]").each(function (index) {
                if (!isNaN(parseFloat($(this).val()))) {
                    TotalTaxSplit = TotalTaxSplit + parseFloat($(this).val());
                }
            });
            $("#[id*=grdDCSplit] [id*=lblTotalTaxFooterSplit]").html(addCommas(toFixed(TotalTaxSplit, CurrencyDigits)));
            $("#[id*=grdDCSplit] [id*=hdfTotalTaxFooterSplit]").val(toFixed(TotalTaxSplit, CurrencyDigits));

        }
        function CalculateTaxAmount(sender) {

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
            if (!isNaN(parseFloat($("#[id*=hdfExchangeCurr]").val()))) {
                ExchangeRate = parseFloat($("#[id*=hdfExchangeCurr]").val());
            }
            if (!isNaN(parseFloat($(sender).closest('tr').find("#[id*=hdfTAXSplit]").val()))) {
                var number = Number($(sender).closest('tr').find("#[id*=hdfTAXSplit]").val().replace(/[^0-9\.]+/g, ""));
                hdftax = parseFloat(number);
            }
            if (!isNaN(parseFloat($(sender).closest('tr').find("#[id*=hdfNETSplit]").val()))) {
                var number = Number($(sender).closest('tr').find("#[id*=hdfNETSplit]").val().replace(/[^0-9\.]+/g, ""));
                netamount = parseFloat(number);
            }
            if (!isNaN(parseFloat($(sender).closest('tr').find("#[id*=txtSumSplit]").val()))) {
                var number = Number($(sender).closest('tr').find("#[id*=txtSumSplit]").val().replace(/[^0-9\.]+/g, ""));
                amount = parseFloat(number);
            }
            if (((amount.toFixed(DecimalDigits)).length) <= 15) {
                $(sender).closest('tr').find("#[id*=txtSumSplit]").val(toFixed(amount, DecimalDigits));
                taxpercentage = hdftax / (netamount == 0 ? 1 : netamount);            
                ttaxamt = (amount * taxpercentage);            
                $(sender).closest('tr').find("#[id*=txtTAXSplitTotal]").val(toFixed(ttaxamt, DecimalDigits));
                $(sender).closest('tr').find("#[id*=hdfTAXSplitTotal]").val(toFixed(ttaxamt, DecimalDigits));
                CalculateTotalSplit($(sender).closest('tr').find("#[id*=txtSumSplit]").val());

            }
            CalculateTotalTaxSplit();
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlManpowerAttendance">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <div id="divSBUCompany" class="buttoncontainer-fields floatLeft">
                                    <asp:DropDownList ID="ddlCompany" class="select-full-a margnbotm0" runat="server"
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
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="20" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('drcr')" ValidationGroup="drcr"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="20"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('drcr')"
                                            ValidationGroup="drcr" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSaveDrcr" CommandName="SAVE" TabIndex="20" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('drcr')" ValidationGroup="drcr"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteDrcr" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="20" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" />
                                    </li>
                                    <li runat="server" id="pnlPrint">
                                        <asp:Button runat="server" TabIndex="20" ID="btnListPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="20" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnJournalize" CommandName="JOURNALIZE" TabIndex="20"
                                            Text="<%$resources:Journalize %>" OnClick="ActionHandler" ToolTip="<%$resources:Journalize %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-journalize" />
                                    </li>
                                    <%--<li>
                                        <asp:Button runat="server" ID="btnAlert" CommandName="ALERT" TabIndex="10" Text="<%$resources:Controls,Alert %>"
                                            Visible="false" OnClick="ActionHandler" ToolTip="<%$resources:Controls,Alert %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-alert" />
                                    </li>--%>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li id="pnlNew">
                                        <asp:Button runat="server" TabIndex="511" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="61" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelDrCr %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelDrCr %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="19" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="20" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="21" ID="btnPrint" CommandName="PRINT" OnClick="ActionHandler"
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
                <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
                <asp:HiddenField ID="hdfCurrencyDigits" runat="server" Value="3" />
                <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                <asp:HiddenField ID="hdfDecimalFormatWithSeperation" runat="server" />
                <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                <asp:HiddenField ID="hdfCurrencyFormatWithSeperator" runat="server" />
                <asp:HiddenField ID="hdfRateFormat" runat="server" />
                <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
                <asp:HiddenField ID="hdfCurrencyGroup2" Value="2" runat="server" />
                <div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                TabIndex="8" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="CREDITDEBITLIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                TabIndex="9" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="CREDITDEBITDETAIL"
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
                            <table class="table-devide" id="tbladvancedSearch" style="/*margin-top: 8px; */ background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblSearchDateFrom" Text="<%$ resources:FromDate %>"
                                                AssociatedControlID="txtSearchDateFrom"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSearchDateFrom" CssClass="input-small margnrgt0-7per"
                                                TabIndex="100" onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfSearchDateFrom" runat="server" />
                                            <asp:Label runat="server" ID="lblSearchDateTo" Text="<%$ resources:ToDate %>" AssociatedControlID="txtSearchDateTo"
                                                class="middle-lbl-small-c"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSearchDateTo" CssClass="input-small" TabIndex="101"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfSearchDateTo" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblCompany" runat="server" Text="<%$ resources:Controls, CompanyPlant %>"  AssociatedControlID="ddlCompanySrch"></asp:Label>
                                            <asp:DropDownList ID="ddlCompanySrch" runat="server" CssClass="select-small-a1" TabIndex="3">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblInvoiceNo" Text="<%$ resources:InvNo %>" AssociatedControlID="txtInvoiceNo"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInvoiceNo" CssClass="input-small margnrgt0-8per"
                                                TabIndex="102"></asp:TextBox>
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" AssociatedControlID="ddlStatus"
                                                CssClass="middle-lbl-a"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-a" TabIndex="103">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,NotPosted %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Posted %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide ">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                      
                                            <asp:Label ID="lblVendor" runat="server" Text="<%$resources:Vendor %>" AssociatedControlID="txtVendor"></asp:Label>
                                            <asp:TextBox ID="txtVendor" runat="server" MaxLength="100" TabIndex="104" CssClass="select-half margnbotm0"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfVendorID" runat="server" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblPaymentNumber" runat="server" Text="<%$resources:CrDrNumber %>"
                                                AssociatedControlID="txtCrDrNumber"></asp:Label>
                                            <asp:TextBox ID="txtCrDrNumber" runat="server" CssClass="input-small-b margnbotm0"
                                                MaxLength="100" TabIndex="105"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCrDrNumber" runat="server" Value="" />
                                            <asp:Label runat="server" ID="lblCnDn" Text="<%$ resources:CreditDebitType%>" AssociatedControlID="ddlCreditDebitType"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:DropDownList ID="ddlCreditDebitType" runat="server" CssClass="select-small-a margnbotm0" TabIndex="106">
                                                <asp:ListItem Text="<%$ Resources:Captions,Select %>" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Debit %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Credit %>" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <%--<asp:Label ID="lblSearchButton" runat="server" AssociatedControlID="btnSearchHdr" class="middle-lbl"></asp:Label>--%>
                                            <asp:ImageButton ID="btnSearchHdr" runat="server" Text="" ToolTip="<%$ resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="107" CommandName="SEARCH" SkinID="search-ext"
                                                Style="margin-bottom: 0px!important; margin-top: 2px;" />
                                            <asp:ImageButton ID="btnClear" runat="server" Text="" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="108" CssClass="margntop2" OnClick="ActionHandler" CommandName="CLEAR"
                                                SkinID="clear-ext" Style="margin-bottom: 0px!important;" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdCrDbHdr" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                    AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" TabIndex="7" runat="server" GroupName="SelectOne"
                                                   ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfCrDrPk" Value='<%# Eval("CDH_PK") %>' />
                                                <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval("CDH_DEPT") %>' />
                                                <asp:HiddenField ID="hdfDelStatusCrDr" runat="server" Value='<%# Eval("CDH_IS_DELETED") %>' /> 
                                                <asp:HiddenField ID="hdfInvoiceTypeCrDr" runat="server" Value='<%# Eval("IVH_TYPE") %>'/>  <%--'<%#Eval("CDH_IVH_TYPE") %>'--%>                                                                                           
                                                <asp:HiddenField ID="hdfCrDrStatus" runat="server" Value='<%# Eval("CDH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date %>" SortExpression="<%$ resources:DataFieldRes,CrDrDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaymentDate" runat="server" Text='<%# Eval("CDH_DATE", Resources.Constants.DateFormatGrid).ToString()  %>'
                                                    ToolTip='<%# Eval("CDH_DATE", Resources.Constants.DateFormatGrid).ToString() %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:No %>" SortExpression="<%$ resources:DataFieldRes,CrDrNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTransactionId" runat="server" Text='<%# Eval("CDH_NO")==""?"[NEW]": Eval("CDH_NO")%>'
                                                    ToolTip='<%# Eval("CDH_NO")==""?"[NEW]": Eval("CDH_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                           <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCmpName" CssClass='<%# Eval("CDH_CMP_LINE_COLOUR") %>' runat="server" Text='<%# Eval("CDH_CMP_DISPLAY_CODE") %>'
                                                    ToolTip='<%# Eval("CDH_CMP_DISPLAY_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceNo1 %>">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkInvnos" runat="server" CssClass="text-underline" OnClick="ActionHandler" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CDH_INVOICE_NO"),15) %>'
                                                  ToolTip='<%# Eval("CDH_INVOICE_NO")%>'  Style="text-align: left!important;" CommandName="PRINTCRDRNOTE"></asp:LinkButton>
                                                <asp:HiddenField runat="server" ID="hdfListinvPK" Value='<%# Eval("IVH_PK")%>' />
                                                <asp:HiddenField runat="server" ID="hdfListInvType" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdfListinvCategory" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdfListGroup" Value='<%# Eval("IVH_GROUP")%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="<%$ resources:Invdate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvDate" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("CDH_INVOICE_DATE", Resources.Constants.DateFormatGrid),12)%>'
                                                    ToolTip='<%# Eval("CDH_INVOICE_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:IssuedTo %>" SortExpression="<%$ Resources:DataFieldRes,CrDrVendorPk%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendor" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CDH_VENDOR_TEXT"),50) %>'
                                                    ToolTip='<%# Eval("CDH_VENDOR_TEXT") %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfVendorPK" Value='<%# Eval("CDH_VENDOR") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="40%" />
                                        </asp:TemplateField>
                                       
                                        <asp:TemplateField HeaderText="<%$ resources:CreditDebitType %>" SortExpression="<%$ resources:DataFieldRes,CrDrType %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblModeofPayment" runat="server" Text='<%# Eval("CDH_TYPE_TEXT")%>'
                                                    ToolTip='<%# Eval("CDH_TYPE_TEXT")%>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfCreditDebitType" Value='<%# Eval("CDH_TYPE").ToString()%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CurrencyH %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='<%#Eval("CDH_CURRENCY_TEXT")  %>'
                                                    ToolTip='<%#Eval("CDH_CURRENCY_TEXT")  %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %>" SortExpression="<%$ resources:DataFieldRes,CrDrAmt %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>                                                
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval("CDH_AMOUNT")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("CDH_AMOUNT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BalanceAmount %>" SortExpression=""
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalanceAmount" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval("CDH_BALANCE_AMOUNT")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("CDH_BALANCE_AMOUNT")) %>'
                                                    Visible='<%# (Convert.ToDecimal((Eval("CDH_BALANCE_AMOUNT"))) == Convert.ToDecimal(Eval("CDH_AMOUNT"))) ? true : false %>'></asp:Label>
                                                <asp:LinkButton ID="lnkBalanceAmount" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval("CDH_BALANCE_AMOUNT")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("CDH_BALANCE_AMOUNT")) %>'
                                                    OnClick="ActionHandler" CommandName="BALANCEAMOUNTSPLIT" CssClass="text-underline"
                                                    Visible='<%# (Convert.ToDecimal((Eval("CDH_BALANCE_AMOUNT"))) == Convert.ToDecimal(Eval("CDH_AMOUNT"))) ? false : true %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle CssClass="amount-numeric" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                 <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("ASC_CSS_CLASS") %>' ToolTip='<%# Eval("CDH_STATUS_TEXT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval("CDH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                               <asp:Button ID="imgPosted" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? GetLocalResourceObject("unposted").ToString() : Eval("FTH_CSS_CLASS")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? Resources.Captions.NotPosted : Eval("FTH_STATUS_TEXT")%>' />
                                                <asp:HiddenField runat="server" ID="hdfPosted" Value='<%# Eval("CDH_HAS_JRNL_ENTRY") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="2%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>                                                
                                                <asp:Button ID="imgAffectStock" runat="server" OnClick="ActionHandler" CommandName="STOCKUPDATION"
                                                    ToolTip="<%$ resources:StockUpdate %>" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="2%" Wrap="false" />
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
                                          <asp:Label ID="lblPaymentNo" runat="server" Text="<%$ resources:No%>" AssociatedControlID="lblPaymentNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblDrCrNo" CssClass="input-small"></asp:Label>                                         
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
                                          <asp:ImageButton ID="btnResetVendorSelection" runat="server" ToolTip="<%$ resources:Controls,Reset %>"
                                                    TabIndex="1" OnClick="ActionHandler" CommandName="RESET" SkinID="btnrefresh" />
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
                                                <asp:TextBox ID="txtPendingFromDate" runat="server" TabIndex="1" Width="100px"
                                                    MaxLength="13" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                                <asp:HiddenField ID="hdfPendingFromDate" runat="server" Value="" />
                                                <asp:Label ID="lblPndngToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtPendingToDate" CssClass="margn-lft5" ></asp:Label>
                                                <asp:TextBox ID="txtPendingToDate" runat="server" TabIndex="2" Width="100px"
                                                    MaxLength="14" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                                <asp:HiddenField ID="hdfPendingToDate" runat="server" Value="" />

                                              <asp:Label ID="lblPendingInvCategory" runat="server" Text="<%$ resources:InvCategory %>"  CssClass="margn-lft5"
                                                    AssociatedControlID="ddlPendingInvCategory"></asp:Label>                                               
                                              <asp:DropDownList ID="ddlPendingInvCategory" runat="server" TabIndex="3"  Width="150px" onchange="InitPInvNumberAuto();" >                                           
                                              </asp:DropDownList> 

                                              <asp:Label ID="lblPndngInvType" runat="server" Text="<%$resources:InvoiceType %>" AssociatedControlID="ddlPendingInvType" CssClass="margn-lft5" ></asp:Label>
                                               <asp:DropDownList ID="ddlPendingInvType" runat="server" TabIndex="4" Width="100px" onchange="InitPInvNumberAuto();" >
                                               </asp:DropDownList>

                                                <asp:Label ID="lblPoNoSr" runat="server" Text="<%$ resources:InvoiceNoHdr %>" CssClass="margn-lft5"
                                                    AssociatedControlID="txtPurchaseInvNumber"></asp:Label>
                                                <div id="divSearchDtls">
                                                    <asp:TextBox ID="txtPurchaseInvNumber" runat="server" TabIndex="4"> </asp:TextBox>
                                                    <asp:HiddenField ID="hdfPurchaseInvPK" runat="server" Value="" />
                                                </div>
                                                <asp:ImageButton ID="imbDtlSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                    ValidationGroup="Search" OnClick="ActionHandler" TabIndex="4" CommandName="DTLSEARCH"
                                                    SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                                <asp:ImageButton ID="imbDtlClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                    TabIndex="4" OnClick="ActionHandler" CommandName="DTLCLEARSEARCH" SkinID="clear-ext"
                                                    CssClass="margntop2 margnbotm0" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                 <asp:GridView runat="server" ID="grdPendingInvList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                    AllowPaging="true" OnPageIndexChanging="ActionHandler" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable">
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
                                               <asp:HiddenField ID="hdfIVHGroup" runat="server" Value='<%#Eval("IVH_GROUP")%>' />
                                                
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
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceNo1 %>" SortExpression="IVH_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("IVH_NO") ==""?"[NEW]":Eval("IVH_NO")%>'
                                                    ToolTip='<%# Eval("IVH_NO")%>'></asp:Label>                                               
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
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
                                        <asp:TemplateField HeaderText="<%$ resources:PONo %>" SortExpression="IVH_PO_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSoNo" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_PO_NO"),20)%>'
                                                    ToolTip='<%# Eval("IVH_PO_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GRNNO %>" SortExpression="IVH_GRN_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrnNo" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_GRN_NO"),18)%>'
                                                    ToolTip='<%# Eval("IVH_GRN_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PODate %>" SortExpression="IVH_POH_DT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSoDate" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_POH_DT", Resources.Constants.DateFormatGrid),12)%>'
                                                    ToolTip='<%# Eval("IVH_POH_DT", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CurrencyH %>" SortExpression="IVH_CURRENCY_TEXT">
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
                                            <asp:Label runat="server" ID="lblMode" Text="<%$ resources:Mode%>" AssociatedControlID="ddlMode"></asp:Label>
                                            <asp:DropDownList ID="ddlMode" runat="server" CssClass="select-small-a1" TabIndex="8">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfMode" CssClass="star" SetFocusOnError="true" ValidationGroup="drcr"
                                                EnableClientScript="true" InitialValue="-1" runat="server" ControlToValidate="ddlMode"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Mode %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="vrfMode2" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="splitMain" EnableClientScript="true" InitialValue="-1" runat="server"
                                                ControlToValidate="ddlMode" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Mode %>">
                                            </asp:RequiredFieldValidator>

                                             <asp:Label runat="server" ID="lblDate" Text="<%$ resources:Date%>" AssociatedControlID="txtDate"
                                                CssClass="middle-lbl-c"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDate" CssClass="input-small Uidate-picker" TabIndex="9"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrftDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="drcr" EnableClientScript="true" runat="server" ControlToValidate="txtDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Date %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td><%--colspan="2"--%>
                                        <div class="div2col-S">
                                            <div style="display: none;">
                                                <asp:Label runat="server" ID="lblPaidAmount" Text="<%$ resources:Amount%>" AssociatedControlID="txtPaidAmount"></asp:Label>
                                                <asp:TextBox ID="txtPaidAmount" runat="server" TabIndex="11" MaxLength="17" CssClass="Uiinput-amount numeric"
                                                    Enabled="false"></asp:TextBox>                                                
                                            </div>
                                            <asp:Label ID="lblPaymentCurrency" runat="server" Text="<%$resources:Currency %>"
                                                AssociatedControlID="txtPaymentCurrency"></asp:Label>
                                            <asp:TextBox ID="txtPaymentCurrency" Enabled="false" runat="server" MaxLength="3"
                                                TabIndex="10" CssClass="input-small  input-disabled"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfPaymentCurrency" runat="server" Value="" />
                                            <asp:Label ID="lblExchangeRate" runat="server" Text="<%$ resources:Controls,ExchangeRate %>"
                                                AssociatedControlID="txtExchangeRate" CssClass="middle-lbl" />
                                            <asp:TextBox ID="txtExchangeRate" runat="server" Enabled="true" CssClass="input-small numeric" TabIndex="11"
                                                AutoPostBack="true" OnTextChanged="ActionHandler" onkeypress="return validateRateFloatKeyPress(this,event);" />
                                            <asp:RequiredFieldValidator ID="vrfExchangeRate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="drcr" EnableClientScript="true" runat="server" ControlToValidate="txtExchangeRate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExchangeRate %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>                                          
                                           
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">                                            
                                            <asp:Label runat="server" ID="lblInstrumentNo" Text="<%$ resources:RefNo%>" AssociatedControlID="txtInstrumentNo"></asp:Label>
                                            <asp:TextBox ID="txtInstrumentNo" runat="server" TabIndex="12" CssClass="input-small margnrgt1-5per"></asp:TextBox>                                           
                                            <asp:Label runat="server" ID="lblInstrumentDate" Text="<%$ resources:RefDate%>" AssociatedControlID="txtInstrumentDate"
                                                CssClass="middle-lbl-a"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInstrumentDate" CssClass="input-small Uidate-picker"
                                                TabIndex="13" onpaste="return false;" onkeydown="return CheckKey(event)"></asp:TextBox>                                            
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblCompanyView" Visible="false"  runat="server" Text="<%$ resources:Controls, CompanyPlant %>"  AssociatedControlID="ddlCompanyView"></asp:Label>
                                            <asp:DropDownList ID="ddlCompanyView" TabIndex="13" Visible="false"  Enabled="false"  runat="server" CssClass="select-small-e"  >
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>

                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblDeclarationNo" Text="<%$ resources:DeclarationNo%>"
                                                AssociatedControlID="txtDeclarationNo"></asp:Label>
                                            <asp:TextBox ID="txtDeclarationNo" runat="server" TabIndex="14" CssClass="input-small"></asp:TextBox>
                                            <asp:Label ID="lblAffectStk" runat="server" Text="<%$ resources:Controls,AffectStock %>"
                                                AssociatedControlID="txtExchangeRate" CssClass="middle-lbl" />
                                            <asp:CheckBox ID="chkAffectStock" runat="server" Enabled="false" CssClass="disable" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <asp:HiddenField ID="hdfInvoiceCategory" runat="server" />
                                <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" />
                                <asp:HiddenField ID="hdfVendorPK" runat="server" />
                                <asp:HiddenField ID="hdfVendorAccountNo" runat="server" />
                                <asp:HiddenField ID="hdfInvoiceCurr" runat="server" />
                                <asp:HiddenField ID="hdfExchangeCurr" runat="server" />
                                <asp:HiddenField ID="hdfPOType" runat="server" />
                                <asp:HiddenField ID="hdfInvoiceGroup" runat="server" />
                                <asp:HiddenField ID="hdfInvoiceType" runat="server" />
                                  <asp:HiddenField ID="hdfSelectedItemPk" runat="server" Value="0" />
                                <asp:Button runat="server" ID="btnRaiseNote" OnClick="ActionHandler" CommandName="RAISENOTECHANGE"
                                    EnableTheming="false" Style="display: none;" />
                                <asp:GridView ID="grdInvoiceList" runat="server" AutoGenerateColumns="False" PageSize="<%$ resources:PageSize %>"
                                    AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false" TabIndex="15"
                                    ShowFooter="true" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceNo1 %>" SortExpression="<%$ resources:DataFieldRes,InvoiceNo %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfTotalAmt" runat="server" Value='<%#Eval("IVH_AMOUNT")%>'  />
                                                <asp:HiddenField ID="hdfTaxAmt" runat="server" Value='<%#Eval("IVH_TAX_AMOUNT")%>'  />
                                                <asp:HiddenField ID="hdfIsDetailTaxExist" runat="server" Value='<%#Eval("IVH_IS_LINE_ITEM_TAX")%>'  />
                                                <asp:HiddenField ID="hdfInvoicePK" runat="server" Value='<%#Eval("CDM_INVOICE_VND_HDR")%>'  />
                                                <asp:HiddenField ID="hdfCrDbMpgPK" runat="server" Value='<%#Eval("CDM_PK")%>' />
                                                <asp:HiddenField ID="hdfhasjournalized" runat="server" Value='<%#Eval("IVH_HAS_JRNL_ENTRY")%>' />
                                                 <asp:HiddenField ID="hdfInvoiceGrp" runat="server" Value='<%#Eval("IVH_GROUP")%>' />
                                                   <asp:HiddenField ID="hdfInvCategory" runat="server" Value='<%#Eval("IVH_CATEGORY")%>' />
                                                  <asp:HiddenField ID="hdfTaxPercentage" runat="server"  Value='<%#Eval("IVH_TAX_PERC")%>'/>
                                                   <asp:HiddenField runat="server" ID="hdfCDMSlNo" Value='<%# Eval("CDM_SL_NO") %>' />                                                   
                                                <asp:LinkButton ID="lnkInvoiceNo" runat="server" CssClass="text-underline" OnClick="ActionHandler"
                                                 Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_NO"),20) %>' ToolTip='<%# Eval("IVH_NO")%>'  CommandArgument='<%# Eval("CDM_INVOICE_VND_HDR")%>'  CommandName="SHOWPOPUP"></asp:LinkButton>                                               
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceDate1 %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%#  Eval("IVH_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("IVH_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                            ToolTip='<%# Eval("IVH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false" HeaderText="<%$ resources:Vendor %>" SortExpression="CDM_VENDOR_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendorInv" runat="server"  Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_VENDOR_TEXT"),20) %>' ToolTip='<%# Eval("IVH_VENDOR_TEXT")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %>" SortExpression="IVH_AMOUNT"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrossAmount" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Convert.ToDouble(Eval("IVH_AMOUNT")))%>' ToolTip='<%# GetFormattedCurrencyWithSeperator(Convert.ToDouble(Eval("IVH_AMOUNT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Discount %>" SortExpression="IVH_DISC_AMOUNT"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDiscount" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Convert.ToDouble(Eval("IVH_DISC_AMOUNT")))%>' ToolTip='<%# GetFormattedCurrencyWithSeperator(Convert.ToDouble(Eval("IVH_DISC_AMOUNT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <%-- Other Charges-------------------------------------------------------------%>
                                        <asp:TemplateField HeaderText="<%$ resources:OtherCharges %>" SortExpression="IVH_SHIP_CHARGE" ItemStyle-HorizontalAlign="Right"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOtherAmount" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Convert.ToDouble(Eval("IVH_SHIP_CHARGE")))%>' ToolTip='<%# GetFormattedCurrencyWithSeperator(Convert.ToDouble(Eval("IVH_SHIP_CHARGE")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <%--End Other Charges---------------------------------------------------------%>
                                        <asp:TemplateField HeaderText="<%$ resources:Tax %>" SortExpression="IVH_TAX_AMOUNT"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTax" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Convert.ToDouble(Eval("IVH_TAX_AMOUNT")))%>' ToolTip='<%# GetFormattedCurrencyWithSeperator(Convert.ToDouble(Eval("IVH_TAX_AMOUNT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvAmt %>" SortExpression="IVH_INVOICE_AMT"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalAmount" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Convert.ToDouble(Eval("IVH_INVOICE_AMT")))%>' ToolTip='<%# GetFormattedCurrencyWithSeperator(Convert.ToDouble(Eval("IVH_INVOICE_AMT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Paid %>" SortExpression="IVH_AMOUNT_PAID_TC"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaid" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Convert.ToDouble(Eval("IVH_AMOUNT_PAID_TC")))%>' ToolTip='<%# GetFormattedCurrencyWithSeperator(Convert.ToDouble(Eval("IVH_AMOUNT_PAID_TC")))%>' ></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AdjAmount %>" SortExpression="" ItemStyle-HorizontalAlign="Right"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAdjAmount" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Convert.ToDouble(Eval("IVH_ADJ_AMT")))%>' ToolTip='<%# GetFormattedCurrencyWithSeperator(Convert.ToDouble(Eval("IVH_ADJ_AMT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Baltopay %>" SortExpression=""
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBaltopay" CssClass="BalancetoPay" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Convert.ToDouble(Eval("IVH_BAL_TO_PAY")))%>' ToolTip='<%# GetFormattedCurrencyWithSeperator(Convert.ToDouble(Eval("IVH_BAL_TO_PAY")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblfooter" Text="<%$ resources:Total %>"></asp:Label></FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:RaiseNoteFor %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfInvRowIndex" runat="server" Value='<%# Container.DataItemIndex %>' />
                                                <asp:TextBox ID="txtNoteFor" onkeyup="CalculateTotal(this);" onblur="RaiseNoteChange(this)"
                                                    Enabled="false" runat="server" CssClass="input-w70 numeric disabled" MaxLength="15"
                                                    TabIndex="16"></asp:TextBox>
                                                <asp:HiddenField ID="hdfItemIncluded" runat="server" Value="0" />                                               
                                                <cc1:AmountValidation ID="vamNoteFor" runat="server" ControlToValidate="txtNoteFor"
                                                    ErrorMessage="<%$ resources:Err_NotFor1 %>" NumberDigits="11" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="drcr"></cc1:AmountValidation>                                                
                                                <asp:CustomValidator ID="vcmNoteFor" CssClass="star" SetFocusOnError="true" ClientValidationFunction="ValidateRaiseNote"
                                                    ValidationGroup="drcr" EnableClientScript="true" runat="server" ControlToValidate="txtNoteFor"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExcessRaiseNote %>"></asp:CustomValidator>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalPayNowFooter"></asp:Label></FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkAllocation" runat="server" OnClick="ActionHandler" CommandName="DCDETAIL"
                                                    OnClientClick="javascript:ValidatePageNow('splitMain')" SkinID="allocation-icon"
                                                    ToolTip="Allocation" CommandArgument="PageAction_Entry" ValidationGroup="splitMain" />
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Tax %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>                                              
                                                <asp:LinkButton ID="lbnTotalTax" runat="server" OnClick="ActionHandler" 
                                                    CommandName="TAXHEADERSPLITUP"></asp:LinkButton>
                                                <asp:HiddenField ID="hdfTotalTax" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalTaxFooter"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <%--Remove--%>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" SkinID="delete-icon" CommandArgument='<%#Eval("CDM_INVOICE_VND_HDR")%>' 
                                                   TabIndex="17" CommandName="REMOVE" ToolTip="Remove" OnClientClick="return ShowDeleteConfirm(this);" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div id="divCalc">
                                <div class="gridwrap">
                                    <table id="tblCalc" class="gridwraptable gridwrap">
                                        <tr>
                                            <td style="text-align: right; width: 90%;">
                                                <asp:Label runat="server" ID="lblSubTotal" Text="<%$ resources:SubTotal%>" AssociatedControlID="txtSubTotal"></asp:Label>
                                            </td>
                                            <td style="text-align: right;" class="btn-margin">
                                                <asp:TextBox ID="txtSubTotal" Text="0" runat="server" CssClass="input-w80 numeric  input-disabled"
                                                    Enabled="false" TabIndex="17" MaxLength="16"></asp:TextBox>                                               
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
                                                    Enabled="false" TabIndex="17" MaxLength="16"></asp:TextBox>                                               
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
                                            <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Description %>" AssociatedControlID="txtRemarks"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRemarks" TextMode="MultiLine" CssClass="multiline-2line" TabIndex="18"
                                                onkeydown="limitText(this,400);" onkeyup="limitText(this,400);"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfRemarks" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="drcr" EnableClientScript="true" runat="server" ControlToValidate="txtRemarks"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Remarks%>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                            </table>
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
                                                    <asp:Label ID="lblTaxHdrCode" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_CODE")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:TaxName %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxHdrName" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("CIT_TAX_TEXT")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:TaxRate %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxRate" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_RATE")) == "0" ? string.Empty : Convert.ToString(Eval("CIT_TAX_RATE")+"%") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:SubTotal %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSubTotal" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Convert.ToDouble(Eval("CIT_TAX_CID_AMOUNT")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Right" CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:TaxAmount %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxAmountBC" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Convert.ToDouble(Eval("CIT_TAX_AMT")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Right" CssClass="amount-numeric" Wrap="false" />
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
                                                <asp:TemplateField HeaderText="<%$ resources:TaxName %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxName" runat="server" Text='<%#Convert.ToString(Eval("CIT_NAME")) == string.Empty ? Resources.Report.Custom : Eval("CIT_NAME") %>'
                                                            ToolTip='<%# Convert.ToString(Eval("CIT_NAME")) == string.Empty ? Resources.Report.Custom : HttpUtility.HtmlDecode(Eval("CIT_NAME").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTaxName" runat="server" Value='<%#Convert.ToString(Eval("CIT_NAME")) == string.Empty ? Resources.Report.Custom : Eval("CIT_NAME") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Amount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrencyWithSeperator(Eval("CIT_TAX_AMT")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithSeperator(Eval("CIT_TAX_AMT")) %>'></asp:Label>
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
                                            AutoGenerateColumns="false" TabIndex="106" EmptyDataRowStyle-CssClass="emptytable"
                                            PageSize="<%$ resources:PageSize%>">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:TaxCode %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxCode" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_CODE")) %>'
                                                            ToolTip='<%# Convert.ToString(Eval("CIT_TAX_CODE")) == string.Empty ? Resources.Report.Custom :HttpUtility.HtmlDecode(Eval("CIT_TAX_CODE").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTaxCode" runat="server" Value='<%# Convert.ToString(Eval("CIT_TAX_CODE")) %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>                                               
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
                                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("CIT_TAX_AMT")) %>'
                                                            ToolTip='<%#GetFormattedCurrency(Eval("CIT_TAX_AMT")) %>'></asp:Label>
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
                                                        <asp:Label ID="lblAmount" runat="server" Text='<%#GetFormattedCurrencyWithSeperator(Eval("TRX_AMOUNT")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithSeperator(Eval("TRX_AMOUNT")) %>'></asp:Label>
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
                        </asp:TableCell></asp:TableRow><asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell></asp:TableRow></asp:Table><asp:HiddenField ID="hdfCrDrTrxNo" runat="server" Value="" />
                <%--------------Cr/Dr Split Start--------------------------------%>
                <div id="divCrDrSplitUp" style="display: none">
                    <div class="content-wrapper">
                        <asp:HiddenField ID="HiddenField1" runat="server" Value="3" />
                        <asp:HiddenField ID="HiddenField2" runat="server" Value="3" />
                        <asp:Panel runat="server" ID="Allocation_Section" CssClass="Button-container-popup">
                            <asp:Button ID="btnSaveDCSplit" runat="server" Text="<%$ resources:Controls,Apply %>"
                                ToolTip="<%$ resources:Controls,Apply %>" OnClick="ActionHandler" TabIndex="72"
                                CommandName="DCSPLITSAVE" SkinID="btnInner-add-dsd" OnClientClick="javascript:ValidatePageNow('split')"
                                ValidationGroup="split" CommandArgument="Allocation_Section" />
                        </asp:Panel>
                        <div class="detail-co3">
                            <div class="div3col-S">
                                <asp:Label ID="LabelDC" runat="server" Text="<%$resources:InvoiceNoDC %>" AssociatedControlID="lblDCSplitNo"
                                    Font-Bold="true"></asp:Label><asp:Label ID="lblDCSplitNo" runat="server" CssClass="medium"></asp:Label><br /><asp:Label ID="Label6" runat="server" Text="<%$resources:AmountDC %>" AssociatedControlID="lblDCSplitAmount"
                                    Font-Bold="true"></asp:Label><asp:Label ID="lblDCSplitAmount" runat="server" CssClass="medium"></asp:Label></div><div class="detail-co3-2">
                                <asp:Label ID="Label2" runat="server" Text="<%$resources:DateDC %>" AssociatedControlID="lblDCSplitDate"
                                    Font-Bold="true"></asp:Label><asp:Label ID="lblDCSplitDate" runat="server" CssClass="medium"></asp:Label>
                                    <div class="clear">
                                </div>
                                <asp:Label ID="lblCustomerDC" runat="server" Text="<%$resources:VendorDC %>" AssociatedControlID="lblDCSplitSupplier"
                                    Font-Bold="true"></asp:Label><asp:Label ID="lblDCSplitSupplier" runat="server"></asp:Label><asp:Label ID="Label8" runat="server" Visible="false" Text="<%$resources:ReceivedDC %>"
                                    AssociatedControlID="lblDCSplitReceived"></asp:Label><asp:Label ID="lblDCSplitReceived" Visible="false" runat="server"></asp:Label><asp:Label ID="Label10" runat="server" Visible="false" Text="<%$resources:ReceiveNowDC %>"
                                    AssociatedControlID="lblDCSplitReceiveNow"></asp:Label><asp:Label ID="lblDCSplitReceiveNow" runat="server" Visible="false"></asp:Label><asp:Label ID="lbltaxSplitpopup" runat="server" Visible="false"></asp:Label></div><div class=" clear">
                            </div>
                        </div>
                        <div class="error" id="divErrorLabel" runat="server" visible="false">
                            <ul>
                                <li>
                                    <asp:Literal runat="server" ID="lblSplitErrorMessage" Text="<%$resources:error_allocation %>"></asp:Literal></li></ul></div><div class="gridwrap">
                            <asp:TableCell> <div class="gridwrap">
                                    <asp:GridView ID="grdDCSplit" runat="server" AutoGenerateColumns="False" Width="100%"
                                        PageSize="<%$ resources:PageSize %>" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable"
                                        AllowSorting="false" ShowFooter="true" OnRowDataBound="ActionHandler">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label></EmptyDataTemplate><Columns>
                                            <asp:TemplateField HeaderText="<%$resources:PRODUCT %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPRODUCTSplit" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CDS_ITEM_TEXT"),20) %>'
                                                    ToolTip='<%# Eval("CDS_ITEM_TEXT") %>'>>
                                                    </asp:Label>
                                                    <asp:HiddenField ID="hdfDCSplitPK"  runat="server" Value='<%# Eval("CDS_PK") %>' />                                                 
                                                    <asp:HiddenField ID="hdfInvCusDtlPK" runat="server" Value='<%# Eval("CDS_INVOICE_VND_DTL") %>' />
                                                    <asp:HiddenField ID="hdfDCTRXPK" runat="server" Value='<%# Eval("CDS_INVOICE_VND_DTL") %>'  />
                                                    <asp:HiddenField ID="hdfCDSTaxPercentage" runat="server"  Value='<%#Eval("CDS_TAX_PERC")%>'/>
                                                     <asp:HiddenField runat="server" ID="hdfCDSSlNo" Value='<%# Eval("CDS_SL_NO") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="18%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:PoInvNo %>" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPONo" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("CDS_PO_NO"),20) %>' ToolTip='<%# Eval("CDS_PO_NO")%>' ></asp:Label>
                                                 </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:DCQTY %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQTYSplit" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("CDS_VID_QTY_INVOICED")) %>'
                                                    ToolTip='<%#GetFormattedNumberWithSeperation(Eval("CDS_VID_QTY_INVOICED")) %>'>></asp:Label>
                                                 </ItemTemplate>
                                               <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:RATE %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRATESplit" runat="server" Text='<%#GetFormattedRate(Eval("CDS_VID_RATE")) %>' ToolTip='<%#GetFormattedRate(Eval("CDS_VID_RATE")) %>'></asp:Label>
                                                  </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:Amount %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmountSplit" runat="server" Text='<%#GetFormattedCurrency(Eval("CDS_VID_AMOUNT")) %>' ToolTip='<%#GetFormattedCurrency(Eval("CDS_VID_AMOUNT")) %>'></asp:Label>
                                                    <asp:HiddenField ID="hdfNETSplit" runat="server" Value='<%#GetFormattedCurrency(Eval("CDS_VID_AMOUNT")) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="<%$resources:Discount %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDiscountSplit" runat="server" Text='<%#GetFormattedCurrency(Eval("CDS_VID_DISCOUNT")) %>' ToolTip='<%#GetFormattedCurrency(Eval("CDS_VID_DISCOUNT")) %>'></asp:Label>
                                                      <asp:HiddenField ID="hdfDiscSplit" runat="server" Value='<%# Eval("CDS_VID_DISCOUNT") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:TAX %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdfTAXSplit" runat="server" Value='<%# Eval("CDS_VID_TAX") %>' />
                                                    <asp:Label ID="lblTAXSplit" runat="server" Text='<%#GetFormattedCurrency(Eval("CDS_VID_TAX")) %>' ToolTip='<%#GetFormattedCurrency(Eval("CDS_VID_TAX")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                  <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:NET %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNETSplit" runat="server" Text='<%#GetFormattedCurrency(Eval("CDS_VID_NET_AMOUNT")) %>' ToolTip='<%#GetFormattedCurrency(Eval("CDS_VID_NET_AMOUNT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                <ItemStyle Width="13%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:QTY %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtQtySplit" runat="server" Width="13px" CssClass="small numeric" Text='<%#GetFormattedNumber(Eval("CDS_QTY")) %>' ToolTip='<%#GetFormattedNumber(Eval("CDS_QTY")) %>'
                                                        onblur="CalculateAmount(this);" MaxLength="13" TabIndex="70"></asp:TextBox>
                                                        <asp:HiddenField  ID="hdfQtySplit" runat="server" Value='<%# Eval("CDS_QTY") %>' />
                                                    <cc1:QuantityValidationP2P ID="vreQtySplit" runat="server" ControlToValidate="txtQtySplit"
                                                        NumberDigits="9" ErrorMessage="<%$ resources:Err_Invalid_Qty %>" Display="Dynamic"
                                                        Text="*" NonZero="false" EnableClientScript="true" CssClass="star" ValidationGroup="split"></cc1:QuantityValidationP2P>
                                                </ItemTemplate>
                                                <ItemStyle Width="11%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:RATE %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRateSplit" TabIndex="71" runat="server" CssClass="input-w80 numeric"  Text='<%#GetFormattedCurrency(Eval("CDS_RATE")) %>'  ToolTip='<%#GetFormattedCurrency(Eval("CDS_RATE")) %>'
                                                        onblur="CalculateAmount(this);" MaxLength="15"> </asp:TextBox>
                                                        <asp:HiddenField ID="hdfRateSplit"  runat="server" Value='<%#Eval("CDS_RATE")%>' />
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
                                                    <asp:TextBox ID="txtSumSplit" onblur="CalculateTotalSplit(this);CalculateTaxAmount(this);"
                                                        runat="server" CssClass="input-w80 numeric" MaxLength="15"> </asp:TextBox>
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
                                                    <asp:TextBox ID="txtTAXSplitTotal" TabIndex="71" runat="server" CssClass="input-w80 numeric"
                                                        MaxLength="15" onblur="CalculateTotalTaxSplit();"> </asp:TextBox>
                                                        <asp:HiddenField ID="hdfTAXSplitTotal" runat="server" />
                                                    <cc1:AmountValidation ID="vamTAXSplitTotal" runat="server" ControlToValidate="txtTAXSplitTotal"
                                                    ErrorMessage="<%$ resources:Err_Invalid_Tax %>" NumberDigits="11" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="split"></cc1:AmountValidation>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" Wrap="false" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalTaxFooterSplit"></asp:Label>
                                                    <asp:HiddenField  runat="server" ID="hdfTotalTaxFooterSplit" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkAffectStkSplit" runat="server" ToolTip='<%$resources:Controls,AffectStock %>'>
                                                    </asp:CheckBox></ItemTemplate>
                                                <ItemStyle Width="13%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkAffectStkHdr" runat="server" ToolTip='<%$resources:Controls,AffectStock %>'>
                                                    </asp:CheckBox>
                                                </HeaderTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:TableCell></div></div></div><%--------------Cr/Dr Split End----------------------------------%>
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
                                                    <ItemStyle Width="41%" />
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
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:TrxAmount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInvCrAmount" runat="server" Text='<%#GetFormattedCurrencyWithSeperator(Eval("CR_AMOUNT")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithSeperator(Eval("CR_AMOUNT")) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfInvCrAmountSplit" runat="server" Value='<%#Eval("CR_AMOUNT") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="22%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalInvCrAmountSplit" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:PaymentAmount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmount" runat="server" Text='<%#GetFormattedCurrencyWithSeperator(Eval("PAID_AMOUNT")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithSeperator(Eval("PAID_AMOUNT")) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfAmountSplit" runat="server" Value='<%#Eval("PAID_AMOUNT") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="22%" HorizontalAlign="Right" />
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
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary ID="vsPage" ValidationGroup="drcr" runat="server" />
                    <asp:ValidationSummary ID="vsSplit" ValidationGroup="split" runat="server" />
                    <asp:ValidationSummary ID="vsSplitMain" ValidationGroup="splitMain" runat="server" />
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
            <asp:HiddenField ID="hdfJournalizeWorkFlow" Value="0" runat="server" />
            <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
            <asp:HiddenField ID="hdfJournalHeader" runat="server" />
            <asp:HiddenField runat="server" ID="hdfTotalPayNowFooter" />
            <asp:HiddenField ID="hdfIsTaxPayable" runat="server" Value="0" />
            <asp:HiddenField ID="hdfBaseCurrency" runat="server" />
            <asp:HiddenField ID="hdfIscontYes" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIscontNo" runat="server" Value="0" />
            <asp:HiddenField ID="hdfSplitCount" runat="server" Value="0" />
            <asp:HiddenField ID="hdfRowIndex" runat="server" />
            <asp:HiddenField runat="server" ID="IsTaxForOtherCharge" Value="0" />
            <asp:HiddenField ID="hdfIsCancelled" Value="0" runat="server" />
            <asp:HiddenField ID="hdfInvGroup" runat="server" Value="0" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>