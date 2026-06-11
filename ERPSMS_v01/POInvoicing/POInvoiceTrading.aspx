<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="POInvoiceTrading.aspx.cs" Inherits="ERPSMS_v01.POInvoicing.POInvoiceTrading" Theme="ClassicExt" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/Journalize/UserControls/JournalizeControlNew.ascx" TagName="Journalize"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/AlertControl.ascx" TagName="Alert" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        var NumberDigits = 0;
        var CurrencyDigits = 0;
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';        
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        $(document).ready(function () {
            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
        });

        function ScrollDown() {
            window.scroll(400, 800);
            return false;
        }
        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            GrandScriptUtils.AddDateRangeCommon("txtPaybydateFrom", "hdfPaybydateFrom", "txtPayByToDate", "hdfPayByToDate", false, false, false, false);
            GrandScriptUtils.AddDateRangeCommon("txtInvoiceReceivedon", "hdfInvoiceReceivedonat", "txtInvdate", "hdfInvdateat", false, false);
            GrandScriptUtils.AddDateRange("txtInvdate", "hdfInvdateat", "txtPaybydate", "hdfPaybydateat", false, false, false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendor", url, "hdfVendorID", true, true, "VENDOR");
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendorHd", url, "hdfVendorHd", true, true, "VENDOR");
            GrandScriptUtils.MakeAutoCompleteDDL("txtInvoiceNumber", url + "?InvCategory=" + $("[id$=hdfInvCategory]").val(), "hdfIVHPK", true, true, "GETDIRECTINVOICENOAUTO");
            GrandScriptUtils.AddDateRangeCommon("txtPendingFromDate", "hdfPendingFromDate", "txtPendingToDate", "hdfPendingToDate", false, false);               
            $("[id$=txtInvoiceAmt]").ForceNumericOnly();
               GrandScriptUtils.DatePickerCommon("txtPVDate");            
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();
            if ($('[id$=btnJournalSaveSubmit]').is(":visible"))
                $('[id$=btnJournalSubmit]').hide();

            if ($('[id$=btnJournalize]').is(":visible")) {
                if ($('[id$=hdfPostingSettings]').val() == "0") {
                    $('[id$=btnJournalize]').hide();
                }
            }

            //Set a stamp for cancelled invoice
            if ($("[id$=hdfIsInvCancelled]").val() == "1") {
                $("[id$=btnAddItem]").hide();
                $("[id$=btnSave]").hide();
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
            }
            else
                $("[id$=tblDetailHdr]").addClass("table-devide");

            if ($("[id$=hdfSelRecordStatus]").val() == 0) {
                $("[id$=btnEditforCancel]").hide();
            }
            else {
                $("[id$=btnEditforCancel]").show();
            }

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
        function AfterClose(containerID) {
            if (containerID == "[id$=divJournalize]") {
                $("[id$=btnJournalizeUpdate]").click();
            }
            else if (containerID == "#divWkfSubmit") {
                if ($("[id$=hdfJournalizeWorkFlow]").val() == "1") {
                    '<%= GetLocalResourceObject("Purchase_Invoice_Journal") %>'
                    ShowContainerDiv('[id$=divJournalize]', '<%= GetLocalResourceObject("Purchase_Invoice_Journal") %>', '1000', '550');
                    AfterCloseWkfInJournal();                   
                }
            } else if (containerID == "[id$=divTemplate]") {
                ShowContainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), '1000', '550');
            }
        }

        function AfterDateSelect(controlID) {
            if (typeof AfterAlertControlDateSelect == "function") {
                AfterAlertControlDateSelect(controlID);
            }
        }
        function CalculateTotal(ID) {
            var Isexceed = ID;
            var Amount = 0;
            var BalancetoInvoice = 0;
            var TaxAmt = 0;
            var NetAmount = 0;
            var DecimalDigits = 0;
            var DiscAmt = 0;
            var Tax = 0;
            var Discount = 0;
            var InvoiceAmt = 0;
            var tempTotal = 0;
            var tempTax = 0;
            var tempDisc = 0;
            var tempInvAmt = 0;
            var TotalAmount = 0;

            if (!isNaN(parseFloat($("#[id*=txtInvoiceAmt]").val()))) {
                InvoiceAmt = parseFloat($("#[id*=txtInvoiceAmt]").val());
            }         
            if (!isNaN(parseFloat($("#[id*=hdfTaxAmt]").val()))) {
                TaxAmt = parseFloat($("#[id*=hdfTaxAmt]").val());
            }
            if (!isNaN(parseFloat($("#[id*=hdfDiscAmt]").val()))) {
                DiscAmt = parseFloat($("#[id*=hdfDiscAmt]").val());
            }
            if (Tax > TaxAmt) {
                $("[id$=litErrorMsg]").show();
                $("[id$=litErrorMsg]").html("Tax should be less than total PO tax values");
                ShowErrorMessage($("#diverror").html(), "Information");
                $("#[id*=txtTaxAmount]").val('0');
            }
            else if (Discount > DiscAmt) {
                $("[id$=litErrorMsg]").show();
                $("[id$=litErrorMsg]").html("Discount should be less than total PO discount values");
                ShowErrorMessage($("#diverror").html(), "Information");
                $("#[id*=txtDiscount]").val('0');
            }
            else if (Discount > InvoiceAmt) {
                $("[id$=litErrorMsg]").show();
                $("[id$=litErrorMsg]").html("Discount should be less than Invoice Amount");
                ShowErrorMessage($("#diverror").html(), "Information");               
            }
            else {
                var totalOCfooter = 0;
                $("#[id*=grdPOList] input[type=text][id*=txtPayNow]").each(function (index) {
                    //Check if number is not empty                   
                    if (!isNaN(parseFloat($(this).closest('tr').find('.BalancetoInvoice').text()))) {
                        BalancetoInvoice = parseFloat($(this).closest('tr').find('.BalancetoInvoice').text().replace(new RegExp(',', 'g'), ''));
                    }
                    // Check whether it is empty or zero
                    if ($(this).val() <= 0.0) {
                        var tempEmptyZero = 0;
                        $(this).closest('tr').find("#[id*=lblPOTax]").text(addCommas(tempEmptyZero.toFixed(CurrencyDigits)));
                        $(this).closest('tr').find("#[id*=hdfPOTax]").val(tempEmptyZero.toFixed(CurrencyDigits));
                    }
                    if (Isexceed == 5) {
                        if (parseFloat($(this).val()) > BalancetoInvoice) {
                            $("[id$=litErrorMsg]").show();
                            $("[id$=litErrorMsg]").html('<%= GetLocalResourceObject("Msg_Cont_Confirm_Bal").ToString() %>');
                            ShowErrorMessage($("#diverror").html(), "Information");
                        }
                    }
                    if (!isNaN(parseFloat($(this).closest('tr').find('[id*=lblGrossAmount]').html().replace(new RegExp(',', 'g'), '')))) {
                        tempTotal = parseFloat($(this).closest('tr').find('[id*=lblGrossAmount]').html().replace(new RegExp(',', 'g'), ''));
                        if (tempTotal > 0) {
                            if ($.trim($(this).val()) != "" && !isNaN(parseFloat($(this).val()))) {

                                if ($(this).closest('tr').find('[id*=txtOthercharges]').val() != undefined) {
                                    if (!isNaN(parseFloat($(this).closest('tr').find('[id*=txtOthercharges]').val().replace(new RegExp(',', 'g'), '')))) {
                                        TotalAmount = parseFloat($(this).closest('tr').find('[id*=lblTotalAmount]').html().replace(new RegExp(',', 'g'), ''));
                                        var tempOC = parseFloat($(this).closest('tr').find('[id*=txtOthercharges]').val().replace(new RegExp(',', 'g'), ''));
                                        totalOCfooter = totalOCfooter + tempOC;
                                        var priceAdjustment = parseFloat($(this).closest('tr').find("#[id*=hdfPriceAdjustment]").val());
                                                                      
                                        if ($("[id$=hdfIsTaxForOtherCharge]").val() == "1") {
                                            var OtherAmount = parseFloat($(this).closest('tr').find('[id*=lblOtherAmount]').html().replace(new RegExp(',', 'g'), ''));
                                            tempTotal = parseFloat(tempTotal) + OtherAmount;
                                            tempOC = 0.0;
                                        }                                        
                                        var priceAdjustmentPerInvAmt = (priceAdjustment / TotalAmount) * parseFloat($(this).val());
                                        $(this).closest('tr').find("#[id*=hdfAdjustPerInvAmt]").val(priceAdjustmentPerInvAmt.toFixed(CurrencyDigits));
                                        tempInvAmt = parseFloat($(this).val()) - (tempOC + priceAdjustmentPerInvAmt);
                                    } else {
                                        tempInvAmt = parseFloat($(this).val());
                                    }
                                }
                                else {
                                    tempInvAmt = parseFloat($(this).val());
                                }
                                if (tempInvAmt < 0) {
                                    $("[id$=litErrorMsg]").show();
                                    $("[id$=litErrorMsg]").html("Other charges should be less than Invoice Amount");
                                    ShowErrorMessage($("#diverror").html(), "Information");
                                }
                                if (tempInvAmt > 0) {
                                    if ($("[id$=hdfTaxSettings]").val() == "1") {
                                        if (!isNaN(parseFloat($(this).closest('tr').find('[id*=lblTax]').html().replace(new RegExp(',', 'g'), '')))) {
                                            tempTax = parseFloat($(this).closest('tr').find('[id*=lblTax]').html().replace(new RegExp(',', 'g'), ''));
                                            tempDisc = parseFloat($(this).closest('tr').find('[id*=lblDiscount]').html().replace(new RegExp(',', 'g'), '')); //?
                                            if (tempTax > 0) {
                                                var tempResult = 0;                                              
                                                var tempResult1 = tempTax / (tempTotal - tempDisc)
                                                tempResult = tempInvAmt - (tempInvAmt / (1 + tempResult1));

                                                var taxResult = (Math.round(tempResult * 100) / 100).toFixed(CurrencyDigits);
                                                $(this).closest('tr').find("#[id*=lblPOTax]").text(addCommas(taxResult));
                                                $(this).closest('tr').find("#[id*=hdfPOTax]").val(taxResult);
                                                Tax = Tax + tempResult;
                                            }
                                        }
                                    }

                                    if (!isNaN(parseFloat($(this).closest('tr').find('[id*=lblDiscount]').html().replace(new RegExp(',', 'g'), '')))) {
                                        tempDisc = parseFloat($(this).closest('tr').find('[id*=lblDiscount]').html().replace(new RegExp(',', 'g'), ''));
                                        var TotalAmount = parseFloat($(this).closest('tr').find('[id*=lblTotalAmount]').html().replace(new RegExp(',', 'g'), ''));
                                        var PayNow = parseFloat($(this).closest('tr').find('[id*=txtPayNow]').val().replace(new RegExp(',', 'g'), ''));
                                        if (tempDisc > 0) {                                           
                                            var tempResult = (tempDisc / TotalAmount) * PayNow;
                                            $(this).closest('tr').find("#[id*=hdfPODiscount]").val(tempResult.toFixed(CurrencyDigits));
                                            Discount = Discount + tempResult;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if ($.trim($(this).val()) != "") {
                        //Check if number is a valid integer
                        if (!isNaN(parseFloat($(this).val()))) {
                            Amount = Amount + parseFloat($(this).val());                           
                        }
                    }
                });

                $("#[id*=grdPOList] [id*=lblTotalPayNowFooter]").html(addCommas(Amount.toFixed(CurrencyDigits)));
                $("#[id*=hdfTotalPayNowFooter]").val(Amount.toFixed(CurrencyDigits));

                $("#[id*=grdPOList] [id*=lblOtherchargesFooter]").html(addCommas(totalOCfooter.toFixed(CurrencyDigits)));
                $("#[id*=hdfOCFooter]").val(totalOCfooter.toFixed(CurrencyDigits));               
                $("#[id*=txtInvoiceAmt]").val(Amount.toFixed(CurrencyDigits));
                InvoiceAmt = parseFloat($("#[id*=txtInvoiceAmt]").val());
                NetAmount = InvoiceAmt; //  - Discount;               
                if ($("[id$=hdfTaxSettings]").val() == "1") {
                    if (!isNaN(parseFloat(Tax.toFixed(CurrencyDigits)))) {
                        $("#[id$=txtTaxAmount]").val(Tax.toFixed(CurrencyDigits));
                    }
                    else {
                        $("#[id$=txtTaxAmount]").val('0');
                    }
                } else {
                    $("#[id$=txtTaxAmount]").val('0');
                }


                if (!isNaN(parseFloat(Discount.toFixed(CurrencyDigits)))) {
                    $("#[id$=txtDiscount]").val(Discount.toFixed(CurrencyDigits));
                }
                else {
                    $("#[id$=txtDiscount]").val('0');
                }


                if (!isNaN(parseFloat(NetAmount.toFixed(CurrencyDigits)))) {
                    $("#[id*=txtNetAmount]").val(NetAmount.toFixed(CurrencyDigits));
                }
                else {
                    $("#[id*=txtNetAmount]").val('0');
                }                
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
                $("[id$=ModifiedDatePnl]").hide();
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

        function ValidateNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                CheckValidationDuplicate(valGroup);
                Page_ClientValidate(valGroup);

            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html(), "Information");
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlSubmit]").hide();
                $("[id$=pnlDelete]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlAlert]").hide();
            }
        }

        function ResetSelection() {
            $('[id$=grdPOInvoiceList]').find('tr td input:radio[id$=rbtSelect]').removeAttr('checked');
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


        //For Setting/Resetting Colour of a selected InvoiceNo
        function SetSelectedRowColor() {
            var selectedIds;
            var selectedIdsArray = new Array();
            selectedIds = $("[id$=hdfSelectedItemPk]").val();
            selectedIdsArray = selectedIds.split(',');

            for (i = 0; i < selectedIdsArray.length; ++i) {

                if (selectedIdsArray[i] != 0) {
                    $("#<%= grdPOInvoiceList.ClientID %> input[type=hidden][id*=hdfInvoiceID]").each(function (index) {
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
        
        //Remove hyper link if Invoice amount and Balance amount are same
        function RemoveBalAmntHyperLink() {
            $("#<%= grdPOInvoiceList.ClientID %> input[type=hidden][id*=hdfInvoiceID]").each(function (index) {
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
            }
            FormattedNumber = NumericPart + LastNumericPart + DecimalPart;
            return FormattedNumber;
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
         
        function InitPONumberAuto() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtPONumber", url + "?customerid=" + $("[id$=hdfVendorHd]").val() + "&InvoicePk=" + $("[id$=hdfInvPk]").val(), "hdfPoPK", true, true, "PURCHASEORDERNO");           
        }
        function ResetPONumberAuto() {
            var defText = '<%= Resources.ErpRes.AutoDefaultValue %>';
            $("[id$=txtPONumber]").val(defText);
            $("[id$=hdfPoPK]").val('-1');
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

        $("[id*=chkSelectAllSo]").live("click", function () {
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
            var chkHeader = $("[id*=chkSelectAllSo]", grid);
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
                                    <asp:DropDownList ID="ddlCompany" class="select-full-a margnbotm0" runat="server"
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
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="30"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('invoice')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="31" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('invoice')" ValidationGroup="invoice"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="32" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('invoice')" ValidationGroup="invoice"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteInv" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="33" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="34" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnJournalize" CommandName="JOURNALIZE" TabIndex="35"
                                            Text="<%$resources:Journalize %>" OnClick="ActionHandler" ToolTip="<%$resources:Journalize %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-journalize" />
                                    </li>
                                    <li id="pnlAlert" runat="server" style="display: none;">
                                        <asp:Button runat="server" ID="btnAlert" CommandName="ALERT" TabIndex="36" Text="<%$resources:Controls,Alert %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:Controls,Alert %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-alert" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="55" ID="btnListPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li id="pnlNew">
                                        <asp:Button runat="server" TabIndex="37" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="8" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelPI %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelPI %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="38" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="39" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>                                   
                                    <li runat="server" id="pnlResetSelection">
                                        <asp:Button runat="server" ID="btnResetSelection" CommandName="RESET" TabIndex="42"
                                            Text="<%$resources:ResetSelection %>" OnClick="ActionHandler" ToolTip="<%$resources:ResetSelection %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" OnClientClick="ResetSelection()" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="43" ID="btnPrint" CommandName="PRINTLISTING"
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
                <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
                <asp:HiddenField ID="hdfCurrencyDigits" runat="server" Value="3" />
                <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                <asp:HiddenField ID="hdfCurrencyFormatWithSeperator" runat="server" />
                <asp:HiddenField ID="hdfRateFormat" runat="server" />
                <asp:HiddenField ID="hdfTaxSettings" runat="server" />
                <asp:HiddenField ID="hdfPostingSettings" runat="server" />
                <asp:HiddenField ID="hdfDelStatus" runat="server" Value="0" />
                <%--  //For SelectedItemId Keeping--%>
                <asp:HiddenField ID="hdfSelectedItemPk" runat="server" Value="0" />
                <div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="57" OnClick="ActionHandler" CommandName="INVOICELIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="58" OnClick="ActionHandler" CommandName="INVOICEDETAIL"
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
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblFrmDate" runat="server" Text="<%$resources:InvoiceDateFrom %>"
                                                AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="3" CssClass="input-small"
                                                MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtToDate"
                                                CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="4" CssClass="input-small" MaxLength="11"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                            <div class="clear">
                                            </div>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-a" TabIndex="7">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,NotPosted %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Posted %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="lblpoNo" runat="server" Text="<%$resources:PONo %>" AssociatedControlID="txtpoNo"
                                                CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:TextBox ID="txtpoNo" runat="server" CssClass="input-small" MaxLength="100" TabIndex="8"> </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblPaybydateFrom" runat="server" Text="<%$resources:PaybydateFrom %>"
                                                AssociatedControlID="txtPaybydateFrom"></asp:Label>
                                            <asp:TextBox ID="txtPaybydateFrom" runat="server" TabIndex="5" CssClass="input-small"
                                                MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfPaybydateFrom" runat="server" Value="" />
                                            <asp:Label ID="lblPayByToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtPayByToDate"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox ID="txtPayByToDate" runat="server" TabIndex="6" CssClass="input-small"
                                                MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfPayByToDate" runat="server" Value="" />
                                            <div class="clear">
                                            </div>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblCompany" runat="server" Text="<%$ resources:Controls, CompanyPlant %>" AssociatedControlID="ddlCompanySrch"></asp:Label>
                                            <asp:DropDownList ID="ddlCompanySrch" runat="server" CssClass="select-small-a1" TabIndex="3">
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
                                                TabIndex="8"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfVendorID" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblInvoiceNoAdvSearch" runat="server" Text="<%$resources:InvoiceNumber %>"
                                                AssociatedControlID="txtInvoiceNumber" CssClass="margnbotm0"></asp:Label>
                                            <asp:TextBox ID="txtInvoiceNumber" runat="server" CssClass="input-small margnbotm0"
                                                MaxLength="100" TabIndex="8"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfIVHPK" runat="server" Value="" />
                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="9" CommandName="SEARCH" SkinID="search-ext"
                                                Style="margin-top: 2px; margin-bottom: 0px;" />
                                            <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="9" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext" Style="margin-top: 2px;
                                                margin-bottom: 0px;" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdPOInvoiceList" Width="100%" PageSize="<%$ resources:PageSize%>" OnRowDataBound="ActionHandler"
                                    AllowSorting="True" OnSorting="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" TabIndex="10" runat="server" GroupName="SelectOne"
                                                    AutoPostBack="true" OnCheckedChanged="ActionHandler" ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                    <asp:HiddenField runat="server" ID="hdfInvoiceID" Value='<%# Eval("IVH_PK") %>' />
                                                    <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval("IVH_DEPT") %>' />
                                                    <asp:HiddenField ID="hdfDelStatus" runat="server" Value='<%# Eval("IVH_DEL_STATUS") %>' />
                                                    <asp:HiddenField ID="hdfTaxAmount" runat="server" Value='<%# Eval("IVH_TAX_TC") %>' />                                                
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>                                        
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceDate %>" SortExpression="<%$ resources:DataFieldRes,POInvoiceDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAdvInvoiceDate" runat="server" Text='<%# Eval("IVH_DATE", Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval("IVH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceNo %>" SortExpression="<%$ resources:DataFieldRes,InvoiceNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAdvInvNo" runat="server" Text='<%# Eval("IVH_NO")==""?"[NEW]":Eval("IVH_NO") %>'
                                                    ToolTip='<%# Eval("IVH_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCmpName"  Font-Bold="true" runat="server" Text='<%# Eval("IVH_COMPANY_TEXT") %>'
                                                    ToolTip='<%# Eval("IVH_COMPANY_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Vendor %>" SortExpression="<%$ Resources:DataFieldRes,VendorName%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendor" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_VENDOR_TEXT"),50) %>'
                                                    ToolTip='<%# Eval("IVH_VENDOR_TEXT") %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfVendorPK" Value='<%# Eval("IVH_VND_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="40%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='<%#Eval("IVH_CURRENCY_TEXT")  %>'
                                                    ToolTip='<%#Eval("IVH_CURRENCY_TEXT")  %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfPOCurrency" Value='<%# Eval("IVH_CURRENCY") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceValue %>" SortExpression="<%$ resources:DataFieldRes,POInvoiceNetAmount %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceValue" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval("IVH_AMOUNT")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("IVH_AMOUNT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BalAmt %>">
                                            <ItemTemplate>                                              
                                                  <asp:LinkButton ID="lbnBalAmt" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval(Resources.DataFieldRes.PurchaseBalAmt))%>'
                                                    CssClass="text-underline" ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval(Resources.DataFieldRes.PurchaseBalAmt))%>'
                                                    OnClick="ActionHandler" CommandName="AMOUNTDETAILS"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Paybydate %>" SortExpression="<%$ resources:DataFieldRes,POPaybydate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaybydate" runat="server" Text='<%# Eval(Resources.DataFieldRes.PurchaseDueDate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.PurchaseDueDate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("ASC_CSS_CLASS") %>' ToolTip='<%# Eval("IVH_STATUS_TEXT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval("IVH_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfDelete" Value='<%# Eval("IVH_DEL_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval("IVH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgPosted" runat="server" OnClientClick="javascript:return false;" />
                                                <asp:HiddenField runat="server" ID="hdfPosted" Value='<%# Eval("IVH_HAS_JRNL_ENTRY") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>

                         <table class="table-devide tablelayout" id="Table2">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblInvNo" runat="server" Text="<%$ resources:InvoiceNo%>" AssociatedControlID="lblInvoiceNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblInvoiceNo" CssClass="input-small"></asp:Label>                                            
                                            <asp:HiddenField ID="HiddenField1" runat="server" />
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
                                              <asp:TextBox ID="txtPendingFromDate" runat="server" TabIndex="1"  Width="100px"  MaxLength="13" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                              <asp:HiddenField ID="hdfPendingFromDate" runat="server" Value="" />
                                              <asp:Label ID="lblPndngToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtPendingToDate"
                                                  CssClass="margn-lft30"></asp:Label>
                                              <asp:TextBox ID="txtPendingToDate" runat="server" TabIndex="2"   Width="100px"
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
                                                            <asp:CheckBox ID="chkSelectAllSo" runat="server" ToolTip="Select All" TabIndex="5" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" ID="chkPoPendSelect" TabIndex="6" />
                                                            <%-- <asp:HiddenField ID="hdfIVH_PK" runat="server" Value='<%# Eval("IVH_PK") %>' />--%>
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
                                                            <asp:Label ID="lblBalAmt" runat="server" Text='<%# Eval("POH_TOTAL_VALUE", "{0:c}") %>'
                                                                ToolTip='<%# Eval("POH_TOTAL_VALUE", "{0:c}") %>' ></asp:Label>
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



                            <table class="table-devide" id="tblDetailHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                         <asp:Label runat="server" ID="lblInvdate" Text="<%$ resources:Invdate%>" AssociatedControlID="txtInvdate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInvdate" TabIndex="11" CssClass="input-small"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfInvdateat" runat="server" Value="" />
                                            <asp:RequiredFieldValidator ID="vrfInvdate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" InitialValue="" runat="server"
                                                ControlToValidate="txtInvdate" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Invdate %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                                                                        
                                            <asp:Label runat="server" ID="lblAddressType" Text="<%$ resources:Type%>" AssociatedControlID="ddlAddressType"></asp:Label>
                                            <asp:DropDownList ID="ddlAddressType" runat="server" CssClass="select-small-g margnrgt1-5per"
                                                TabIndex="14" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfAddressType" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="vatbuy" EnableClientScript="true" InitialValue="-1" runat="server"
                                                ControlToValidate="ddlAddressType" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Type %>">
                                            </asp:RequiredFieldValidator>
                                            &nbsp&nbsp&nbsp
                                            <asp:TextBox ID="txtBranchCode" runat="server" CssClass="input-small" MaxLength="5"
                                                TabIndex="14" />
                                            <asp:RequiredFieldValidator ID="vrfBranchCode" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="vatbuy" EnableClientScript="true" runat="server" ControlToValidate="txtBranchCode"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_BranchCode %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <div id="divVendorBranch" runat="server">
                                                <asp:Label runat="server" ID="lblVendorBranch" Visible="false" Text="<%$ resources:Branch%>"
                                                    AssociatedControlID="ddlVendorBranch"></asp:Label><asp:DropDownList ID="ddlVendorBranch"
                                                        runat="server" Visible="false">
                                                    </asp:DropDownList>
                                            </div>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                           
                                            <asp:Label runat="server" ID="lblSupplierInvNo" Text="<%$ resources:SupplierInvNo %>"
                                                AssociatedControlID="txtSupplierInvNo"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSupplierInvNo" CssClass="input-small" MaxLength="100"
                                                TabIndex="12"></asp:TextBox>
                                           <div class="starwrap">
                                            <asp:RequiredFieldValidator ID="vrfSupplierInvNo" CssClass="star" SetFocusOnError="true"
                                                InitialValue="" ValidationGroup="invoice" EnableClientScript="true" runat="server"
                                                ControlToValidate="txtSupplierInvNo" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_SupplierInvNo %>">
                                            </asp:RequiredFieldValidator>
                                            </div>
                                            <asp:Label runat="server" ID="lblInvoiceReceivedon" Text="<%$ resources:InvoiceReceivedon%>"
                                                AssociatedControlID="txtInvoiceReceivedon" CssClass="middle-lbl-c"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInvoiceReceivedon" CssClass="input-small" TabIndex="13"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfInvoiceReceivedonat" runat="server" Value="" />
                                           <div class="starwrap">
                                            <asp:RequiredFieldValidator ID="vrfInvoiceReceivedon" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" InitialValue="" runat="server"
                                                ControlToValidate="txtInvoiceReceivedon" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_InvoiceReceivedon %>">
                                            </asp:RequiredFieldValidator>
                                           </div>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblVatBuyTaxId" Text="<%$ resources:Taxid%>" AssociatedControlID="txtVatTaxId"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtVatTaxId" TabIndex="14" CssClass="input-small"></asp:TextBox>

                                            <asp:Label runat="server" ID="lblPaybydate" Text="<%$ resources:Paybydate%>" AssociatedControlID="txtPaybydate"
                                                CssClass="middle-lbl-c"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPaybydate" CssClass="input-small" TabIndex="17"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfPaybydateat" runat="server" Value="" />
                                            <asp:RequiredFieldValidator ID="vrfPaybydate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" InitialValue="" runat="server"
                                                ControlToValidate="txtPaybydate" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Paybydate %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblOriginal" runat="server" Text="<%$ resources:OriginalInvReceived%>"
                                                AssociatedControlID="chkOriginalinvoice" TabIndex="18"></asp:Label>
                                            <asp:CheckBox ID="chkOriginalinvoice" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <asp:GridView ID="grdPOList" runat="server" AutoGenerateColumns="False" Width="100%" OnRowDataBound="ActionHandler"
                                    PageSize="<%$ resources:PageSize %>" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable"
                                    AllowSorting="false" ShowFooter="true">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:PONo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnPONo" runat="server" CssClass="text-underline nomargin" OnClick="ActionHandler"
                                                    CommandName="SHOWPOPUP" Text='<%# Eval("POH_NO") %>' CommandArgument='<%# Eval("POH_PK") %>'></asp:LinkButton>
                                               <asp:HiddenField ID="hdfPONumber" Value='<%# Eval("POH_PK") %>' runat="server" />
                                                <asp:HiddenField ID="hdfPriceAdjustment" Value='<%# Eval("POH_AJUST_AMT") %>' runat="server" />
                                                <asp:HiddenField ID="hdfOtherChargesPrev" Value='<%# Eval("IVM_PREV_OTHER_CHARGE") %>' runat="server" />
                                                <asp:HiddenField ID="hdfPurchaseOrderType" runat="server" Value='<%# Eval("POH_TYPE") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField  HeaderText="<%$ resources:POdate %>" SortExpression="" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPODate" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Vendor %>" SortExpression="" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendorInv" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency %>" SortExpression=""  Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GrossAmount %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrossAmount" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval("POH_GROSS_AMT")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("POH_GROSS_AMT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Discount %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDiscount" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval("POH_DISCOUNT_AMT")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("POH_DISCOUNT_AMT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Tax %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblTax" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval("POH_TAX_AMT")) %>'
                                                    CssClass="text-underline nomargin" OnClick="ActionHandler" CommandName="TAXDETAILSSPLITUP"
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("POH_TAX_AMT")) %>' CommandArgument='<%# Eval("POH_PK") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:POothers %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOtherAmount" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval("POH_OTHER_CHARGES")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("POH_OTHER_CHARGES")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                             <%-- <asp:TemplateField HeaderText="Amt Adj." SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAdj" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval("POH_AJUST_AMT")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("POH_AJUST_AMT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:POAmt %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalAmount" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval("POH_TOTAL_AMT")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("POH_TOTAL_AMT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Invoiced %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiced" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval("IVM_INVOICED_AMT")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("IVM_INVOICED_AMT")) %>'></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfInvoiced" />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AdvInvoiced %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAdvInvoiced" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval("IVM_ADV_INV_AMT")) %>' ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("IVM_ADV_INV_AMT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BalancetoInvoice %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalancetoInvoice" CssClass="BalancetoInvoice" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval("IVM_BAL_TO_INVOICE")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("IVM_BAL_TO_INVOICE")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblfooter" Text="<%$ resources:Total %>"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoicedNow %>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPayNow" runat="server" onkeyup="CalculateTotal(5);" CssClass="input-w70 numeric"
                                                    MaxLength="15" TabIndex="15" Width="90%" Text='<%# GetFormattedCurrency(Eval("IVM_AMOUNT")) %>'></asp:TextBox>
                                                <asp:HiddenField runat="server" ID="hdfPayNow" Value='<%# GetFormattedCurrency(Eval("IVM_AMOUNT")) %>' />
                                                <asp:RequiredFieldValidator ID="vrfPaybyNowReq" CssClass="star" SetFocusOnError="false"
                                                    ValidationGroup="invoice" EnableClientScript="true" InitialValue="" runat="server"
                                                    ControlToValidate="txtPayNow" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Invamount %>">
                                                </asp:RequiredFieldValidator>
                                                <cc1:AmountValidation ID="vrePayNow" runat="server" ControlToValidate="txtPayNow"
                                                    ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"
                                                    NonZero="true"></cc1:AmountValidation>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" Width="12%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalPayNowFooter"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:POotherChrg %>">
                                             <ItemTemplate>
                                                <asp:TextBox ID="txtOthercharges" ToolTip="Non taxable" runat="server" Text='<%# GetFormattedCurrency(Eval("IVM_OTHER_AMOUNT")) %>'
                                                    CssClass="input-w70 numeric" onkeyup="CalculateTotal(0);" MaxLength="15" TabIndex="16"></asp:TextBox>
                                                <asp:HiddenField ID="hdfOtherchargeOLD" Value='' runat="server" />
                                                <cc1:AmountValidation ID="vreOthercharges" runat="server" ControlToValidate="txtOthercharges"
                                                    ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"></cc1:AmountValidation>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" Width="12%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblOtherchargesFooter"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Tax %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPOTax" runat="server" ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("IVM_TAX_AMOUNT")) %>'></asp:Label>
                                                <asp:HiddenField ID="hdfPOTax" Value='0' runat="server" />
                                                <asp:HiddenField ID="hdfPODiscount" Value='0' runat="server" />
                                                <asp:HiddenField ID="hdfAdjustPerInvAmt" Value='0' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>                                      
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" SkinID="delete-icon"
                                                    CommandName="REMOVE" ToolTip="Remove" OnClientClick="return ShowDeleteConfirm(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <%-- ----------Tax Splitup Popup --------------------%>
                            <div id="divTaxsplitup" style="display: none">
                                <div class="Button-container-popup">
                                </div>
                                <div class="content-wrapper">
                                    <div class="gridwrap" id="divTaxSplitupDetails">
                                        <asp:GridView runat="server" ID="grdTaxSplitupDetails" Width="100%" AllowSorting="false"
                                            AutoGenerateColumns="false" TabIndex="106" EmptyDataRowStyle-CssClass="emptytable"
                                            PageSize="<%$ resources:PageSize%>">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>                                                
                                                <asp:TemplateField HeaderText="<%$ resources:TaxName %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxName" runat="server" Text='<%#Eval("PTH_NAME") %>' ToolTip='<%# HttpUtility.HtmlDecode(Eval("PTH_NAME").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTaxName" runat="server" Value='<%#Eval("PTH_NAME") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:TaxAmount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrencyWithSeperator(Eval("PTH_TAX_AMT")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithSeperator(Eval("PTH_TAX_AMT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
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
                                                    <ItemStyle Width="30%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate" runat="server" Text='<%#  Eval("PVH_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("PVH_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                            ToolTip='<%# Eval("PVH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="25%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:TrxAmount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("PAID_AMOUNT")) %>'
                                                            ToolTip='<%#GetFormattedCurrency(Eval("PAID_AMOUNT")) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfAmountSplit" runat="server" Value='<%#Eval("PAID_AMOUNT") %>' />
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
                            <%-- End --------------------------------------------%>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblInvoiceAmt" Text="<%$ resources:InvoiceAmt%>" AssociatedControlID="txtInvoiceAmt"></asp:Label>
                                            <asp:TextBox ID="txtInvoiceAmt" runat="server" MaxLength="17" onkeydown="return EnableArrowKey(event);"
                                                onpaste="return false;" CssClass="input-small numeric input-disabled"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfInvoiceAmt" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtInvoiceAmt"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_InvoiceAmt%>"></asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblTaxAmount" Text="<%$ resources:TaxAmount%>" AssociatedControlID="txtTaxAmount"
                                                CssClass="middle-lbl-a"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTaxAmount" MaxLength="17" onkeydown="return EnableArrowKey(event);"
                                                onpaste="return false;" CssClass="input-small numeric input-disabled"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfTaxAmount" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" InitialValue="" runat="server"
                                                ControlToValidate="txtTaxAmount" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TaxAmount %>">
                                            </asp:RequiredFieldValidator>
                                            <div class=" clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblDiscount" Text="<%$ resources:InvDiscount%>" AssociatedControlID="txtDiscount"></asp:Label>
                                            <asp:TextBox ID="txtDiscount" runat="server" MaxLength="17" onkeydown="return EnableArrowKey(event);"
                                                onpaste="return false;" CssClass="input-small numeric input-disabled margnrgt1-5per"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfDiscount" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtDiscount"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Discount%>"></asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblNetAmount" Text="<%$ resources:NetAmount%>" AssociatedControlID="txtNetAmount"
                                                CssClass="middle-lbl-a"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtNetAmount" MaxLength="16" onkeydown="return EnableArrowKey(event);"
                                                onpaste="return false;" CssClass="input-small numeric input-disabled"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfNetAmount" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" InitialValue="" runat="server"
                                                ControlToValidate="txtNetAmount" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_NetAmount %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks %>" AssociatedControlID="txtRemarks"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRemarks" TabIndex="21" TextMode="MultiLine" CssClass="multiline-2line"
                                                onkeydown="limitText(this,450);" onchange="limitText(this,450);"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfRemarks" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" InitialValue="" runat="server"
                                                ControlToValidate="txtRemarks" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Remarks %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="divcol-S">
                                <asp:Label ID="lblFileUpload" runat="server" Text="AttachFile" AssociatedControlID="fupUpload"></asp:Label>
                                <asp:FileUpload ID="fupUpload" runat="server" TabIndex="22" Style="width: 15.6%;" />
                                <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupUpload"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_File_Upload %>">                                                        
                                </asp:RequiredFieldValidator>
                                <a id="anchorFile" runat="server" target="_blank" tabindex="22"></a>
                                <asp:Button runat="server" ID="btnAddItem" CommandName="ADDITEM" TabIndex="23" OnClick="ActionHandler"
                                    ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="PageAction_Entry" ValidationGroup="upload"
                                    Text="<%$resources:ErpRes,Add %>" SkinID="btnInner-add" />                               
                                <div class="clear">
                                </div>
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdUploads" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="false" AllowPaging="false" OnRowDataBound="ActionHandler" AutoGenerateColumns="false"
                                    TabIndex="24" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:SlNo %>">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
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
                                                    SkinID="delete-icon" ToolTip="Delete" CommandArgument="PageAction_Entry" OnClientClick="return ShowDeleteConfirm(this);"
                                                    OnLoad="btnAction_Load" OnPreRender="btnAction_PreRender" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
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
                    <asp:ValidationSummary ID="vsUpload" ValidationGroup="upload" runat="server" />
                </div>
                <asp:HiddenField ID="hdfInvoiceNo" runat="server" />
                <asp:HiddenField ID="hdfIsJournalize" runat="server" />
                <asp:HiddenField ID="hdfInvPaidAmt" runat="server" />
                <asp:HiddenField ID="hdfExchangeRate" runat="server" />
                <asp:HiddenField ID="hdfTaxAmt" runat="server" />
                <asp:HiddenField ID="hdfDiscAmt" runat="server" />
                <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
                <asp:HiddenField ID="hdfIscontYes" runat="server" />
                <asp:HiddenField runat="server" ID="hdfOCFooter" />
                <asp:HiddenField runat="server" ID="hdfTotalPayNowFooter" />
                <asp:HiddenField ID="hdfIsContDupVenInvNo" Value="0" runat="server" />
                <asp:HiddenField ID="hdfIsTaxForOtherCharge" runat="server" Value="0" />
                <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
                <asp:HiddenField ID="hdfCurrencyGroup2" Value="2" runat="server" />
                <asp:HiddenField ID="hdfIsInvCancelled" Value="0" runat="server" />
                <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
                <asp:HiddenField runat="server" ID="hdfInvPk" Value="0" />
                <asp:HiddenField ID="hdfCurrentPk" runat="server" Value="0" />
                <asp:HiddenField ID="hdfPOItemType" runat="server" />
                 <asp:HiddenField ID="hdfSelRecordStatus" runat="server" />
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
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="invoice">
                </uc1:WorkflowUserComments>
            </div>
            <asp:HiddenField ID="hdfJournalizeWorkFlow" Value="0" runat="server" />
            <asp:HiddenField ID="hdfType" Value="0" runat="server" />
            <asp:HiddenField ID="hdfVendorContactType" Value="0" runat="server" />
             <asp:HiddenField runat="server" ID="hdfInvCategory" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAddItem" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>