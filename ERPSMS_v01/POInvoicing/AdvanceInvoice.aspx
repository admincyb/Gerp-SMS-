<%@ Page Title="<%$ Resources:Captions,Title_POAdvInv %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" 
    AutoEventWireup="true" CodeBehind="AdvanceInvoice.aspx.cs" Inherits="ERPSMS_v01.POInvoicing.AdvanceInvoice" 
    Theme="ClassicExt" ValidateRequest="false" %>
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
        //var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
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
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendor", url + "?IsSBUVendor=" + $("[id$='hdfIsSBUVendor']").val(), "hdfVendorID", true, true, "VENDOR");
            GrandScriptUtils.MakeAutoCompleteDDL("txtInvoiceNumber", url, "hdfIVHPK", true, true, "ADVINVNUMBER");
            //            $("[id$=txtTaxAmount]").ForceNumericOnly();
            //            $("[id$=txtDiscount]").ForceNumericOnly();
            //            $("[id*=txtPayNow]").ForceNumericOnly();
            $("[id$=txtInvoiceAmt]").ForceNumericOnly();

            GrandScriptUtils.DatePickerCommon("txtPVDate");
            //$("[id$=txtJournalExchangeRate]").ForceNumericOnly();
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
            if ($("[id$=hdfIsInvCancelled]").val() == "1")
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
            else
                $("[id$=tblDetailHdr]").addClass("table-devide");

            //End

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
                    OK: function (e) {
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

<%--            function PayByDateConfirm(msg) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%=GetLocalResourceObject("PayByDateConfirmation").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 170,
                width: 390,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $("[id$=hdfPayByDtContinue]").val("1");
                        $(this).dialog("close");
                        $("[id$=btnDummySaveSubmit]").click();
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }--%>

            function PayByDateConfirm(btn) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%=GetLocalResourceObject("PayByDateConfirmation").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $("[id$=hdfPayByDtContinue]").val("1");
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
        function AfterClose(containerID) {
            if (containerID == "[id$=divJournalize]") {
                $("[id$=btnJournalizeUpdate]").click();
            }
            else if (containerID == "#divWkfSubmit") {
                if ($("[id$=hdfJournalizeWorkFlow]").val() == "1") {
                    '<%= GetLocalResourceObject("Purchase_Invoice_Journal") %>'
                    ShowContainerDiv('[id$=divJournalize]', '<%= GetLocalResourceObject("Purchase_Invoice_Journal") %>', '1000', '550');
                    AfterCloseWkfInJournal();
                    //$("[id$=btnJournalize_Action]").click();
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



            if (!isNaN(parseFloat($("#[id*=hdfDecimalFormat]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalFormat]").val());
            }
            if (!isNaN(parseFloat($("#[id*=txtInvoiceAmt]").val()))) {
                InvoiceAmt = parseFloat($("#[id*=txtInvoiceAmt]").val());
            }
            //            if (!isNaN(parseFloat($("#[id*=txtTaxAmount]").val()))) {
            //                Tax = parseFloat($("#[id*=txtTaxAmount]").val());
            //            }
            //            if (!isNaN(parseFloat($("#[id*=txtDiscount]").val()))) {
            //                Discount = parseFloat($("#[id*=txtDiscount]").val());
            //            }
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
                //$("#[id*=txtDiscount]").val('0');
            }
            else {
                var totalOCfooter = 0;
                $("#[id*=grdPOList] input[type=text][id*=txtPayNow]").each(function (index) {
                    //Check if number is not empty
                    //                    $(this).closest('tr').find("#[id*=lblPOTax]").text('0'.toFixed(CurrencyDigits));
                    //                    $(this).closest('tr').find("#[id*=hdfPOTax]").val('0'.toFixed(CurrencyDigits));


                    if (!isNaN(parseFloat($(this).closest('tr').find('.BalancetoInvoice').text()))) {

                        var number = Number($(this).closest('tr').find('.BalancetoInvoice').text().replace(/[^0-9\.]+/g, ""));
                        BalancetoInvoice = parseFloat(number);
                    }

                    if (Isexceed == 5 && $("[id$=hdfIsCancel]").val() != 1) {
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
                                        //Change based on configuration, Done By Riyas                                 
                                        if ($("[id$=hdfIsTaxForOtherCharge]").val() == "1") {
                                            var OtherAmount = parseFloat($(this).closest('tr').find('[id*=lblOtherAmount]').html().replace(new RegExp(',', 'g'), ''));
                                            tempTotal = parseFloat(tempTotal) + OtherAmount;
                                            tempOC = 0.0;
                                        } //End
                                        //tempInvAmt = parseFloat($(this).val()) - (tempOC + priceAdjustment);
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
                                                var taxableAmount = tempTotal - tempDisc;
                                                if (taxableAmount > 0) {
                                                    var tempResult1 = tempTax / taxableAmount;
                                                    tempResult = tempInvAmt - (tempInvAmt / (1 + tempResult1));
                                                }

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
                                            //                                            var tempResult = (tempDisc / tempTotal) * tempInvAmt;
                                            //                                             Discount = Discount + tempResult;
                                            //                                            var tempResult = (tempDisc / TotalAmount)
                                            //                                            Discount = tempResult * PayNow;
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

                            //                            if (BalancetoInvoice < parseFloat($(this).val())) {
                            //                                $("[id$=litErrorMsg]").show();
                            //                                $("[id$=litErrorMsg]").html("Pay Now should be less than or equal to Balance to Invoice");
                            //                                ShowErrorMessage($("#diverror").html(), "Information");
                            //                                $(this).val('0');
                            //                            }
                            //                            else {

                            Amount = Amount + parseFloat($(this).val());
                            //}
                        }
                    }
                });

                $("#[id*=grdPOList] [id*=lblTotalPayNowFooter]").html(addCommas(Amount.toFixed(CurrencyDigits)));
                $("#[id*=hdfTotalPayNowFooter]").val(Amount.toFixed(CurrencyDigits));

                $("#[id*=grdPOList] [id*=lblOtherchargesFooter]").html(addCommas(totalOCfooter.toFixed(CurrencyDigits)));
                $("#[id*=hdfOCFooter]").val(totalOCfooter.toFixed(CurrencyDigits));
                //$("#[id*=txtInvoiceAmt]").val(Amount.toFixed(2));
                $("#[id*=txtInvoiceAmt]").val(Amount.toFixed(CurrencyDigits));
                InvoiceAmt = parseFloat($("#[id*=txtInvoiceAmt]").val());
                NetAmount = InvoiceAmt; //  - Discount;
                //if (NetAmount == Amount) {
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
                //                }
                //                else {
                //                    $("[id$=litErrorMsg]").show();
                //                    $("[id$=litErrorMsg]").html("Net Amount and Total Invoice Amount not tallied");
                //                    ShowErrorMessage($("#diverror").html(), "Information");
                //                    $("#[id*=txtNetAmount]").val(NetAmount.toFixed(2));
                //                    InvoiceAmt = Amount + Discount - Tax;
                ////                    $("#[id*=txtInvoiceAmt]").val(InvoiceAmt.toFixed(2));
                //                }

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
                //                $("[id$=pnlSubmit]").hide();//When taking the transaction in view mode to view the workflow details,submit button must show
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
                                    <li id="pnlAlert" runat="server">
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
                                    <li style="display: none">
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
                                    <li>
                                        <asp:Button runat="server" ID="btnPickForPayment" CommandName="PICKFORPAYMENT" TabIndex="40"
                                            Text="<%$resources:PickPoForPayment %>" OnClick="ActionHandler" ToolTip="Pick Inv & for Paying"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-money" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnPickForCrDrNote" CommandName="PICKFORCRDRNOTE"
                                            TabIndex="41" Text="<%$resources:PickForCrDrNote %>" OnClick="ActionHandler"
                                            ToolTip="Pick Inv & for Cr/Dr. Note" CommandArgument="SEC_ActionPanel" SkinID="btnInner-moneycredit" />
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
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnPOListing" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowPO %>">
                            <asp:LinkButton runat="server" ID="lbnPOListing" Text="<%$resources:PageNameRes,PurchaseOrder %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="50" CssClass="tab-inactive" OnClick="ActionHandler"
                                CommandName="DEFAULT"></asp:LinkButton>
                        </span></li>
                        <%-- <li><span id="spnDirectPurchase" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnDirectPurchase" Text="<%$resources:PageNameRes,DirectPurchase %>"
                                TabIndex="2" CommandName="DIRECTPURCHASE" OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>--%>
                        <li><span id="spnInvoicing" runat="server" class="tab-active" visible="<%$ resources:ConfigurationsRes,TabShowPurchaseAdvInvoice %>">
                            <asp:LinkButton runat="server" ID="lnkInvoicing" Text="<%$resources:PageNameRes,Invoice %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="51" OnClick="ActionHandler" CommandName="INVOICE"
                                CssClass="tab-active"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnPOInvoice" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowPurchaseInvoice %>">
                            <asp:LinkButton runat="server" ID="lbnPOInvoice" Text="<%$resources:PageNameRes,POInvoice %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="52" OnClick="ActionHandler" CommandName="POINVOICE"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnExpenses" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowExpense %>">
                            <asp:LinkButton runat="server" ID="lbnExpenses" Text="<%$resources:PageNameRes,Expenses %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="53" OnClick="ActionHandler" CommandName="EXPENSES"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnPayment" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowPayment %>">
                            <asp:LinkButton runat="server" ID="lnkPayment" Text="<%$resources:PageNameRes,Payment %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="54" OnClick="ActionHandler" CommandName="PAYMENT"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnCrDrNote" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowPurchaseCRDR %>">
                            <asp:LinkButton runat="server" ID="lnbCrDrNote" Text="<%$resources:PageNameRes,CreditDebitNotes %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="55" OnClick="ActionHandler" CommandName="CRDRNOTE"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnAcPayables" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowAP %>">
                            <asp:LinkButton runat="server" ID="lnbAcPayables" Text="<%$resources:PageNameRes,AccountPayables %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="56" OnClick="ActionHandler" CommandName="ACPAYABLES"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
                <asp:HiddenField ID="hdfCurrencyDigits" runat="server" Value="3" />
                <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
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
                                            <asp:Label ID="lblCompany" runat="server" Text="<%$ resources:Controls, CompanyPlant %>"
                                                AssociatedControlID="ddlCompanySrch"></asp:Label>
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
                                            <asp:Label ID="lblInvoiceNumber" runat="server" Text="<%$resources:InvoiceNumber %>"
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
                                <asp:GridView runat="server" ID="grdPOInvoiceList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                    AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" TabIndex="10" runat="server" GroupName="SelectOne"
                                                    AutoPostBack="true" OnCheckedChanged="ActionHandler" ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfInvoiceID" Value='<%# Eval(Resources.DataFieldRes.POInvoicePK) %>' />
                                                <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval("IVH_DEPT") %>' />
                                                <asp:HiddenField ID="hdfDelStatus" runat="server" Value='<%# Eval("IVH_DEL_STATUS") %>' />
                                                <asp:HiddenField ID="hdfTaxAmount" runat="server" Value='<%# Eval("IVH_TAX_TC") %>' />
                                                
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceDate %>" SortExpression="<%$ resources:DataFieldRes,POInvoiceDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.POInvoiceDate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.POInvoiceDate, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceNo %>" SortExpression="<%$ resources:DataFieldRes,InvoiceNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.InvoiceNo)==""?"[NEW]":Eval(Resources.DataFieldRes.InvoiceNo) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.InvoiceNo)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCmpName" CssClass="<%# Eval(Resources.DataFieldRes.CMP_LINE_COLOUR) %>"
                                                    runat="server" Text='<%# Eval(Resources.DataFieldRes.ADM_COMPANY_CMP_DISPLAY_CODE) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.ADM_COMPANY_CMP_DISPLAY_CODE) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Vendor %>" SortExpression="<%$ Resources:DataFieldRes,VendorName%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendor" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataTableRes.VendorMst+"."+Resources.DataFieldRes.VendorName),60) %>'
                                                    ToolTip='<%# Eval(Resources.DataTableRes.VendorMst+"."+Resources.DataFieldRes.VendorName) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfVendorPK" Value='<%# Eval(Resources.DataFieldRes.InvoiceVendorPK) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="45%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='<%#Eval(Resources.DataTableRes.CurrencyMst1+"."+Resources.DataFieldRes.CurrencyCode)  %>'
                                                    ToolTip='<%#Eval(Resources.DataTableRes.CurrencyMst1+"."+Resources.DataFieldRes.CurrencyCode)  %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfPOCurrency" Value='<%# Eval(Resources.DataFieldRes.InvoiceCurrency) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceValue %>" SortExpression="<%$ resources:DataFieldRes,POInvoiceNetAmount %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceValue" runat="server" Text='<%# Eval(Resources.DataFieldRes.POInvoiceNetAmount, "{0:c}") %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.POInvoiceNetAmount, "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BalAmt %>">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBalAmt" runat="server" Text='' CssClass="text-underline" ToolTip=''
                                                    OnClick="ActionHandler" CommandName="AMOUNTDETAILS"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Paybydate %>" SortExpression="<%$ resources:DataFieldRes,POPaybydate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaybydate" runat="server" Text='<%# Eval(Resources.DataFieldRes.POPaybydate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%#  Eval(Resources.DataFieldRes.POPaybydate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;" />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval(Resources.DataFieldRes.Approved) %>' />
                                                <asp:HiddenField runat="server" ID="hdfDelete" Value='<%# Eval("IVH_DEL_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval("IVH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgPosted" runat="server" OnClientClick="javascript:return false;" />
                                                <asp:HiddenField runat="server" ID="hdfPosted" Value='<%# Eval(Resources.DataFieldRes.Posted) %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide" id="tblDetailHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblInvoiceNo" Text="<%$ resources:InvoiceNo%>" AssociatedControlID="txtSupplierInvNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblDispInvoiceNo" CssClass="input-small" Text="<%$ resources:InvoiceNo%>"></asp:Label>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblSupplierName" Text="<%$ resources:SupplierName%>"
                                                AssociatedControlID="txtSupplierInvNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblDispSupplierName" Text="<%$ resources:SupplierName%>"
                                                CssClass="select-half"></asp:Label>
                                            <div class="clear">
                                            </div>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblAddressType" Text="<%$ resources:Type%>" AssociatedControlID="ddlAddressType"></asp:Label>
                                            <%--<asp:TextBox ID="txtAddressType" runat="server" MaxLength="100" TabIndex="7"> </asp:TextBox>--%>
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
                                            <asp:Label runat="server" ID="lblInvoiceGstType" Text="Sub Type" AssociatedControlID="ddlInvoiceGstType"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlInvoiceGstType" CssClass="select-small-e">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblCompanyView" runat="server" Text="<%$ resources:Controls, CompanyPlant %>"
                                                AssociatedControlID="ddlCompanyView"></asp:Label>
                                            <asp:DropDownList ID="ddlCompanyView" Enabled="false" runat="server" CssClass="select-small-e">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
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
                                            <asp:Label runat="server" ID="lblSupplierInvNo" Text="<%$ resources:SupplierInvNo %>"
                                                AssociatedControlID="txtSupplierInvNo"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSupplierInvNo" CssClass="input-small" MaxLength="100"
                                                TabIndex="12"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfSupplierInvNo" CssClass="star" SetFocusOnError="true"
                                                InitialValue="" ValidationGroup="invoice" EnableClientScript="true" runat="server"
                                                ControlToValidate="txtSupplierInvNo" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_SupplierInvNo %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblInvoiceReceivedon" Text="<%$ resources:InvoiceReceivedon%>"
                                                AssociatedControlID="txtInvoiceReceivedon" CssClass="middle-lbl-a"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInvoiceReceivedon" CssClass="input-small" TabIndex="13"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfInvoiceReceivedonat" runat="server" Value="" />
                                            <asp:RequiredFieldValidator ID="vrfInvoiceReceivedon" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" InitialValue="" runat="server"
                                                ControlToValidate="txtInvoiceReceivedon" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_InvoiceReceivedon %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblVatBuyTaxId" Text="<%$ resources:Taxid%>" AssociatedControlID="txtVatTaxId"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtVatTaxId" TabIndex="14" CssClass="input-small"></asp:TextBox>
                                            <asp:Label runat="server" ID="lblPaybydate" Text="<%$ resources:Paybydate%>" AssociatedControlID="txtPaybydate"
                                                CssClass="middle-lbl-a"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPaybydate" CssClass="input-small" TabIndex="17"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>

                                            <asp:Label runat="server" ID="lblInvestor" Text="<%$ resources:Controls,InvestorCode %>" AssociatedControlID="txtInvestor"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInvestor" TabIndex="14" Enabled="false" CssClass="input-small"></asp:TextBox>
                                            
                                             <asp:Label ID="lblOriginal" runat="server" Text="<%$ resources:OriginalInvReceived%>"
                                                AssociatedControlID="chkOriginalinvoice" TabIndex="18" CssClass="ibl-label-cls"></asp:Label>
                                            <asp:CheckBox ID="chkOriginalinvoice" runat="server" />

                                            <asp:HiddenField ID="hdfPaybydateat" runat="server" Value="" />
                                            <asp:RequiredFieldValidator ID="vrfPaybydate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" InitialValue="" runat="server"
                                                ControlToValidate="txtPaybydate" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Paybydate %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                           


                                            <div class="clear">
                                            </div>
                                            <div class="clear">
                                            </div>
                                            <div id="divVendorBranch" class="padgtop7" runat="server">
                                                <asp:Label runat="server" ID="lblVendorBranch" Text="<%$ resources:Branch%>" Visible="false"
                                                    AssociatedControlID="ddlVendorBranch"></asp:Label>
                                                <asp:DropDownList ID="ddlVendorBranch" runat="server" Visible="false">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <asp:GridView ID="grdPOList" runat="server" AutoGenerateColumns="False" Width="100%"
                                    PageSize="<%$ resources:PageSize %>" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable"
                                    AllowSorting="false" ShowFooter="true" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:PONo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnPONo" runat="server" CssClass="text-underline nomargin" OnClick="ActionHandler"
                                                    CommandName="PRINTPO"></asp:LinkButton>
                                                <%-- <asp:Label ID="lblPONo" runat="server" Text='' ToolTip=''></asp:Label>--%>
                                                <asp:HiddenField ID="hdfPoPk" runat="server" />
                                                <asp:HiddenField ID="hdfPONumber" Value='' runat="server" />
                                                <asp:HiddenField ID="hdfPriceAdjustment" Value='' runat="server" />
                                                <asp:HiddenField ID="hdfOtherChargesPrev" Value='' runat="server" />
                                                <asp:HiddenField ID="hdfTaxPercentage" Value='' runat="server" />
                                             <asp:HiddenField runat="server" ID="hdfInvType" Value='<%# Eval("GroupPK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false" HeaderText="<%$ resources:POdate %>" SortExpression="">
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
                                        <asp:TemplateField HeaderText="<%$ resources:Currency %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GrossAmount %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrossAmount" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Discount %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDiscount" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Tax %>" SortExpression="">
                                            <ItemTemplate>
                                                <%--  <asp:Label ID="lblTax" runat="server" Text='' CssClass="Tax" ToolTip=''></asp:Label>
                                                <asp:ImageButton ID="imgTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                   ToolTip="<%$ resources:Tax %>" CommandName="TAXDETAILSSPLITUP"  />--%>
                                                <asp:LinkButton ID="lblTax" runat="server" Text='' CssClass="text-underline nomargin"
                                                    OnClick="ActionHandler" CommandName="TAXDETAILSSPLITUP" ToolTip=''></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:POothers %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOtherAmount" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:POAmt %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalAmount" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Invoiced %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiced" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AdvInvoiced %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAdvInvoiced" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BalancetoInvoice %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalancetoInvoice" CssClass="BalancetoInvoice" runat="server" Text=''
                                                    ToolTip=''></asp:Label>
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
                                                <asp:TextBox ID="txtPayNow" runat="server" CssClass="input-w97 numeric" MaxLength="15"
                                                    onblur="CalculateTotal(5);" TabIndex="19"></asp:TextBox>
                                                <%-- <asp:RegularExpressionValidator ID="vrePayNow" runat="server" ControlToValidate="txtPayNow"
                                                    ErrorMessage="<%$ resources:Err_Amount_Valid %>" ValidationExpression="^\$?([0-9]{0,11})?(\.[0-9]{0,3})?$"
                                                    Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice">
                                                </asp:RegularExpressionValidator>--%>
                                                <asp:HiddenField runat="server" ID="hdfPayNow" />
                                                <cc1:AmountValidation ID="vrePayNow" runat="server" ControlToValidate="txtPayNow"
                                                    ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"
                                                    NonZero="true"></cc1:AmountValidation>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalPayNowFooter"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:POotherChrg %>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtOthercharges" ToolTip="Non taxable" Text="0.00" runat="server"
                                                    CssClass="input-w97 numeric" onblur="CalculateTotal(0);" MaxLength="15" TabIndex="20"></asp:TextBox>
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
                                                <asp:Label ID="lblPOTax" Text="0" runat="server" ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdfPOTax" Value='0' runat="server" />
                                                <asp:HiddenField ID="hdfPODiscount" Value='0' runat="server" />
                                                <asp:HiddenField ID="hdfAdjustPerInvAmt" Value='0' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <%--Remove--%>
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
                                                <%--<asp:TemplateField HeaderText="<%$ resources:TaxType %>">
                                                    <ItemTemplate>
                                                      
                                                        <asp:HiddenField ID="hdfTaxSplitPK" runat="server" Value='<%#Eval("PTH_PK") %>' />
                                                        <asp:HiddenField ID="hdfTaxPK" runat="server" Value='<%#Eval("PTH_TAX") %>' />
                                                        <asp:Label ID="lblTaxText" runat="server" Text='<%# Convert.ToString(Eval("PTH_TAX_TEXT")) == string.Empty ? "" : Convert.ToString(Eval("PTH_TAX_TEXT")) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("PTH_TAX_TEXT")) == string.Empty ? "" : Convert.ToString(Eval("PTH_TAX_TEXT"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="<%$ resources:TaxName %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxName" runat="server" Text='<%#Eval("PTH_NAME") %>' ToolTip='<%# HttpUtility.HtmlDecode(Eval("PTH_NAME").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTaxName" runat="server" Value='<%#Eval("PTH_NAME") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:TaxAmount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrencyWithComa(Eval("PTH_TAX_AMT")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithComa(Eval("PTH_TAX_AMT")) %>'></asp:Label>
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
                                <%--  <div class="btnwrap-divcol">
                                            
                                        </div>--%>
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
                                        <%--<asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkEdit" runat="server" OnClick="ActionHandler" CommandName="EDITITEM"
                                                    SkinID="edit-icon" ToolTip="Edit" CommandArgument="PageAction_Entry" OnLoad="btnAction_Load"
                                                    OnPreRender="btnAction_PreRender" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                                                    SkinID="delete-icon" ToolTip="Delete" CommandArgument="PageAction_Entry" OnClientClick="return ShowDeleteConfirm(this);"
                                                    OnLoad="btnAction_Load" OnPreRender="btnAction_PreRender" />
                                            </ItemTemplate>
                                            <%--<ItemStyle Width="3%" />--%>
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
                <asp:HiddenField ID="hdfShowInvestor" Value="0" runat="server" />
                <asp:HiddenField runat="server" ID="hdfPayByDtContinue" Value="0" />
            </div>
             <div style="display: none">
             <asp:Button runat="server" ID="btnDummySaveSubmit" CommandName="WRKFSUBMIT" TabIndex="64"
              Text="" OnClick="ActionHandler" SkinID="btnInner-submit" />
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
            <asp:HiddenField ID="hdfIsSBUVendor" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsCancel" runat="server" Value="0" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAddItem" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
