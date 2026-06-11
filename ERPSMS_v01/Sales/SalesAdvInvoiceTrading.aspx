<%@ Page Title="<%$ Resources:Captions,Title_SalesInvoiceTrading %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="SalesAdvInvoiceTrading.aspx.cs" Inherits="ERPSMS_v01.Sales.SalesAdvInvoiceTrading"
    Theme="ClassicExt" %>

<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc2" %>
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
        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            GrandScriptUtils.AddDateRangeCommon("txtInvdate", "hdfInvdate", "txtPaybydate", "hdfPaybydate", false, false);
            GrandScriptUtils.DatePickerCommon("txtInvoiceReceivedon");
            GrandScriptUtils.DatePickerCommon("txtPVDate");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url, "hdfCustomer", true, true, "CUSTOMERLIST");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomerSearch", url, "hdfCustomerSearchID", true, true, "CUSTOMERLIST");
            //GrandScriptUtils.MakeAutoCompleteDDL("txtInvoiceNumber", url, "hdfIVHPK", true, true, "ADVSALINVOICENUMBER");
            GrandScriptUtils.MakeAutoCompleteDDL("txtInvoiceNumber", url + "?InvCategory=" + $("[id$=hdfInvCategory]").val(), "hdfIVHPK", true, true, "SALINVNUMBERTRADING");
            $("[id*=txtPayNow]").ForceNumericOnly();
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();
            if ($('[id$=btnJournalSaveSubmit]').is(":visible"))
                $('[id$=btnJournalSubmit]').hide();
            if ($('[id$=btnJournalize]').is(":visible")) {
                if ($('[id$=hdfPostingSettings]').val() == "0") {
                    $('[id$=btnJournalize]').hide();
                }
            }

            ShowHidePendingSO($("[id$=hdfIsPendingSOVisible]").val());
            setTableWidth();
            InitSONumberAuto();
            //Set a stamp for cancelled invoice
            if ($("[id$=hdfInvDelStatus]").val() == "1")
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
            else
                $("[id$=tblDetailHdr]").addClass("table-devide");
            //End

            if ($("[id$=txtCustomer]").attr("disabled") == true) {
                DisableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomer]"));
            }
            else {
                EnableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomer]"));
            }
        }

        function ShowHidePendingSO(flag) {
            ///<summary>
            /// Used to Show/Hide ItemDetails Div
            ///</summary>

            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divPendingSODetails]").show();
                $("[id$=imbShowPendingSO]").hide();
                $("[id$=imbHidePendingSO]").show();
            }
            else {
                $("[id$=divPendingSODetails]").hide();
                $("[id$=imbShowPendingSO]").show();
                $("[id$=imbHidePendingSO]").hide();
            }
            $("[id$=hdfIsPendingSOVisible]").val(flag);
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
                    OK: function (e) {
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
            $("#[id*=grdSalesList] input[type=text][id*=txtPayNow]").each(function (index) {
                var perfoma = $(this).closest('tr').find("#[id*=lblProformaInvoiced]").html();
                if (perfoma == null) {
                    $(this).closest("table").find("tr:first th:eq(0)").css("width", "10%");
                    $(this).closest("tr").find("td:eq(0)").css("width", "10%");
                    $(this).closest("table").find("tr:last td:eq(0)").css("width", "10%");

                    $(this).closest("table").find("tr:first th:eq(1)").css("width", "10%");
                    $(this).closest("tr").find("td:eq(1)").css("width", "10%");
                    $(this).closest("table").find("tr:last td:eq(1)").css("width", "10%");

                    $(this).closest("table").find("tr:first th:eq(3)").css("width", "12%");
                    $(this).closest("tr").find("td:eq(3)").css("width", "12%");
                    $(this).closest("table").find("tr:last td:eq(3)").css("width", "12%");

                    $(this).closest("table").find("tr:first th:eq(4)").css("width", "9%");
                    $(this).closest("tr").find("td:eq(4)").css("width", "9%");
                    $(this).closest("table").find("tr:last td:eq(4)").css("width", "9%");

                    $(this).closest("table").find("tr:first th:eq(5)").css("width", "9%");
                    $(this).closest("tr").find("td:eq(5)").css("width", "9%");
                    $(this).closest("table").find("tr:last td:eq(5)").css("width", "9%");

                    $(this).closest("table").find("tr:first th:eq(7)").css("width", "12%");
                    $(this).closest("tr").find("td:eq(7)").css("width", "12%");
                    $(this).closest("table").find("tr:last td:eq(7)").css("width", "12%");
                }
                else {
                    $(this).closest("table").find("tr:first th:eq(0)").css("width", "13%");
                    $(this).closest("tr").find("td:eq(0)").css("width", "13%");
                    $(this).closest("table").find("tr:last td:eq(0)").css("width", "13%");

                    $(this).closest("table").find("tr:first th:eq(1)").css("width", "7%");
                    $(this).closest("tr").find("td:eq(1)").css("width", "7%");
                    $(this).closest("table").find("tr:last td:eq(1)").css("width", "7%");

                    $(this).closest("table").find("tr:first th:eq(3)").css("width", "7%");
                    $(this).closest("tr").find("td:eq(3)").css("width", "7%");
                    $(this).closest("table").find("tr:last td:eq(3)").css("width", "7%");

                    $(this).closest("table").find("tr:first th:eq(4)").css("width", "6%");
                    $(this).closest("tr").find("td:eq(4)").css("width", "6%");
                    $(this).closest("table").find("tr:last td:eq(4)").css("width", "6%");

                    $(this).closest("table").find("tr:first th:eq(5)").css("width", "7%");
                    $(this).closest("tr").find("td:eq(5)").css("width", "7%");
                    $(this).closest("table").find("tr:last td:eq(5)").css("width", "7%");

                    $(this).closest("table").find("tr:first th:eq(7)").css("width", "10%");
                    $(this).closest("tr").find("td:eq(7)").css("width", "10%");
                    $(this).closest("table").find("tr:last td:eq(7)").css("width", "10%");
                }
            });
        }

        function AfterClose(containerID) {
            if (containerID == "[id$=divJournalize]") {
                $("[id$=btnJournalizeUpdate]").click();
            }
            else if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
                if ($("[id$=hdfJournalizeWorkFlow]").val() == "1") {
                    ShowContainerDiv('[id$=divJournalize]', '<%= GetLocalResourceObject("Sales_Invoice_Journal") %>', '1000', '550');
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
        function CalculateTotal() {
            var Amount = 0;
            var BalancetoInvoice = 0;
            var TaxAmt = 0;
            var priceAdjustmentPerInvAmt = 0;
            var NetAmount = 0;
            var DiscAmt = 0;
            var Tax = 0;
            var Discount = 0;
            var InvoiceAmt = 0;
            var tempTotal = 0;
            var tempTax = 0;
            var tempDisc = 0;
            var tempInvAmt = 0;
            var zeroValue = 0;
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
                $("[id$=litErrorMsg]").html("Tax should be less than total SO tax values");
                ShowErrorMessage($("#diverror").html(), "Information");
                $("#[id*=txtTaxAmount]").val('0');
            }
            else if (Discount > DiscAmt) {
                $("[id$=litErrorMsg]").show();
                $("[id$=litErrorMsg]").html("Discount should be less than total SO discount values");
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
                $("#[id*=grdSalesList] input[type=text][id*=txtPayNow]").each(function (index) {
                    //Check if number is not empty
                    if (!isNaN(parseFloat($(this).closest('tr').find('.BalancetoInvoice').text()))) {
                        BalancetoInvoice = parseFloat($(this).closest('tr').find('.BalancetoInvoice').text().replace(new RegExp(',', 'g'), ''));
                    }
                    // Check whether it is empty or zero
                    if ($(this).val() <= 0.0) {
                        var tempEmptyZero = 0;
                        $(this).closest('tr').find("#[id*=lblSOTax]").text(addCommas(tempEmptyZero.toFixed(CurrencyDigits)));
                        $(this).closest('tr').find("#[id*=hdfSOTax]").val(tempEmptyZero.toFixed(CurrencyDigits));
                    }
                    if (!isNaN(parseFloat($(this).closest('tr').find('[id*=lblGrossAmount]').html().replace(new RegExp(',', 'g'), '')))) {
                        tempTotal = $(this).closest('tr').find('[id*=lblGrossAmount]').text().replace(new RegExp(',', 'g'), '');
                        if (tempTotal > 0) {
                            if ($.trim($(this).val()) != "" && !isNaN(parseFloat($(this).val()))) {
                                if ($(this).closest('tr').find("#[id*=txtOthercharges]").html() != undefined) {
                                    TotalAmount = parseFloat($(this).closest('tr').find('[id*=lblTotalAmount]').html().replace(new RegExp(',', 'g'), ''));
                                    if (!isNaN(parseFloat($(this).closest('tr').find('[id*=txtOthercharges]').val().replace(new RegExp(',', 'g'), '')))) {
                                        var tempOC = parseFloat($(this).closest('tr').find('[id*=txtOthercharges]').val().replace(new RegExp(',', 'g'), ''));
                                        var priceAdjustment = parseFloat($(this).closest('tr').find("#[id*=hdfPriceAdjustment]").val());
                                        //Change based on configuration, Done By Juno
                                        totalOCfooter = totalOCfooter + tempOC;
                                        if ($("[id$=hdfIsTaxForOtherCharge]").val() == "1") {
                                            var OtherAmount = parseFloat($(this).closest('tr').find('[id*=lblOtherAmount]').html().replace(new RegExp(',', 'g'), ''));
                                            tempTotal = parseFloat(tempTotal) + OtherAmount;
                                            tempOC = 0.0;
                                        } //End
                                        if (TotalAmount > 0)
                                            priceAdjustmentPerInvAmt = (priceAdjustment / TotalAmount) * parseFloat($(this).val());
                                        $(this).closest('tr').find("#[id*=hdfAdjustPerInvAmt]").val(priceAdjustmentPerInvAmt.toFixed(CurrencyDigits));
                                        tempInvAmt = parseFloat($(this).val()) - (tempOC + priceAdjustmentPerInvAmt);
                                    } else {
                                        tempInvAmt = parseFloat($(this).val());
                                    }
                                    if (tempInvAmt > 0) {
                                        if ($("[id$=hdfTaxSettings]").val() == "1") {
                                            if (!isNaN(parseFloat($(this).closest('tr').find('[id*=lblTax]').html().replace(new RegExp(',', 'g'), '')))) {
                                                tempTax = parseFloat($(this).closest('tr').find('[id*=lblTax]').html().replace(new RegExp(',', 'g'), ''));
                                                tempDisc = parseFloat($(this).closest('tr').find('[id*=lblDiscount]').html().replace(new RegExp(',', 'g'), '')); //?
                                                if (tempTax > 0) {
                                                    var tempResult = 0;
                                                    var tempResult1 = tempTax / (tempTotal - tempDisc); //-priceAdjustmentPerInvAmt //Bug:6377
                                                    tempResult = tempInvAmt - (tempInvAmt / (1 + tempResult1));
                                                    $(this).closest('tr').find("#[id*=lblSOTax]").text(addCommas(tempResult.toFixed(CurrencyDigits)));
                                                    $(this).closest('tr').find("#[id*=hdfSOTax]").val(tempResult.toFixed(CurrencyDigits));
                                                    Tax = Tax + tempResult;
                                                }
                                            }
                                        }
                                        if (!isNaN(parseFloat($(this).closest('tr').find('[id*=lblDiscount]').html().replace(new RegExp(',', 'g'), '')))) {
                                            tempDisc = parseFloat($(this).closest('tr').find('[id*=lblDiscount]').html().replace(new RegExp(',', 'g'), ''));
                                            var PayNow = parseFloat($(this).closest('tr').find('[id*=txtPayNow]').val().replace(new RegExp(',', 'g'), ''));
                                            if (tempDisc > 0) {
                                                var tempResult = (tempDisc / TotalAmount) * PayNow
                                                $(this).closest('tr').find("#[id*=hdfSODiscount]").val(tempResult.toFixed(CurrencyDigits));
                                                Discount = Discount + tempResult;

                                            }
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
                $("#[id*=grdSalesList] [id*=lblTotalPayNowFooter]").html(addCommas(Amount.toFixed(CurrencyDigits)));
                $("#[id*=grdSalesList] [id*=lblTotalPayNowFooter]").attr('title', addCommas(Amount.toFixed(CurrencyDigits)));
                $("#[id*=hdfTotalPayNowFooter]").val(Amount.toFixed(CurrencyDigits));
                $("#[id*=grdPOList] [id*=lblOtherchargesFooter]").html(totalOCfooter.toFixed(CurrencyDigits));
                $("#[id*=hdfOCFooter]").val(totalOCfooter.toFixed(CurrencyDigits));
                $("#[id*=txtInvoiceAmt]").val(Amount.toFixed(CurrencyDigits));
                InvoiceAmt = parseFloat($("#[id*=txtInvoiceAmt]").val());
                NetAmount = InvoiceAmt;  // InvoiceAmt + Tax - Discount;
                if ($("[id$=hdfTaxSettings]").val() == "1") {
                    if (!isNaN(parseFloat(Tax.toFixed(CurrencyDigits)))) {
                        $("[id$=txtTaxAmount]").val(Tax.toFixed(CurrencyDigits));
                    }
                    else {
                        $("[id$=txtTaxAmount]").val(zeroValue.toFixed(CurrencyDigits));
                    }
                } else {
                    $("[id$=txtTaxAmount]").val(zeroValue.toFixed(CurrencyDigits));
                }
                if (!isNaN(parseFloat(Discount.toFixed(CurrencyDigits)))) {
                    $("[id$=txtDiscount]").val(Discount.toFixed(CurrencyDigits));
                }
                else {
                    $("[id$=txtDiscount]").val(zeroValue.toFixed(CurrencyDigits));
                }
                if (!isNaN(parseFloat(NetAmount.toFixed(CurrencyDigits)))) {
                    $("[id$=txtNetAmount]").val(NetAmount.toFixed(CurrencyDigits));
                }
                else {
                    $("[id$=txtNetAmount]").val(zeroValue.toFixed(CurrencyDigits));
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
        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                CheckValidationDuplicate(valGroup);
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
        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlDelete]").hide();               
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();             
                $("[id$=btnListPrint]").hide();
            }
        }
        function ResetSelection() {
            $('[id$=grdSalesInvoiceList]').find('tr td input:radio[id$=rbtSelect]').removeAttr('checked');
            $('[id$=grdSalesInvoiceList]').find('tr td input:checkbox[id$=chkInvselect]').removeAttr('checked');
        }
        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtCustomer") {
                $("[id$=btnCustomer]").click();
            }
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteSelect(targetControlID);
            }
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtCustomer") {
                $("[id$=btnCustomer]").click();
            }
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteInvalidSelect(targetControlID);
            }
        }
        function CheckInvoice(sender, args) {
            if ($('[id$=vrePayNow]')[0].isvalid) {
                if ($("[id$=ddlInvoiceType]").val() != "-1") {
                    var total = 0;
                    var invoiced = 0;
                    var pInvoiced = 0;
                    var invoice = 0;
                    var invType = $("[id$=ddlInvoiceType]").val();
                    invoice = parseFloat(args.Value);
                    if (!isNaN(parseFloat($(sender).closest('tr').find('[id*=lblTotalAmount]').html()))) {
                        var tot = Number($(sender).closest('tr').find('[id*=lblTotalAmount]').html().replace(/[^0-9\.]+/g, ""));
                        total = parseFloat(tot);
                    }
                    if (!isNaN(parseFloat($(sender).closest('tr').find('[id*=lblInvoiced]').html()))) {
                        var tot = Number($(sender).closest('tr').find('[id*=lblInvoiced]').html().replace(/[^0-9\.]+/g, ""));
                        invoiced = parseFloat(tot);
                    }
                    if (invType != "3") {

                        if (invoice > total - invoiced) {
                            args.IsValid = false;
                        } else {
                            args.IsValid = true;
                        }
                    } else {
                        if (!isNaN(parseFloat($(sender).closest('tr').find('[id*=lblProformaInvoiced]').html()))) {
                            var tot = Number($(sender).closest('tr').find('[id*=lblProformaInvoiced]').html().replace(/[^0-9\.]+/g, ""));
                            pInvoiced = parseFloat(tot);
                        }
                        var max = 0;
                        if (invoiced > pInvoiced) {
                            max = invoiced;
                        } else {
                            max = pInvoiced;
                        }

                        if (invoice > total - max) {
                            args.IsValid = false;
                        } else {
                            args.IsValid = true;
                        }
                    }

                } else {
                    args.IsValid = true;
                }
            }
            else {
                args.IsValid = true;
            }
        }
        
        function closeDeletePopup() {
            $("[id$=txtReason]").val("");
            $('#divConfirmationWithReason').dialog('close');
            ClosePopup();
            return false;
        }
        function CalculateTotalAmountSplit(sender) {
            var TotalAmountSplit = 0;
            $("#[id*=grdPaidAmntSplitup] input[type=hidden][id*=hdfAmountSplit]").each(function (index) {
                TotalAmountSplit = TotalAmountSplit + parseFloat($(this).val());
            });
            $("#[id*=grdPaidAmntSplitup] [id*=lblTotalAmountSplit]").html(TotalAmountSplit.toFixed(CurrencyDigits));
        }
        //For Setting/Resetting Colour of a selected Row
        function SetSelectedRowColor() {
            var selectedIds;
            var selectedIdsArray = new Array();
            selectedIds = $("[id$=hdfSelectedItemPk]").val();
            selectedIdsArray = selectedIds.split(',');
            for (i = 0; i < selectedIdsArray.length; ++i) {
                if (selectedIdsArray[i] != 0) {
                    $("#<%= grdSalesInvoiceList.ClientID %> input[type=hidden][id*=hdfInvoiceID]").each(function (index) {
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
        function SelectedCheckBoxCount(mode) {
            var count = $('[id$=grdSalesInvoiceList]').find('tr td input:checkbox[id$=chkInvselect]:checked').length;
            var msgTitle;
            var msg;
            msg = '<%= ERP.Utilities.CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_SelectItem").ToString()) %>';
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            if (mode == 1) {
                if (count == 0) {
                    ShowErrorMessage(msg, msgTitle);
                    return false;
                }
                else if (count > 1) {
                    msg = '<%= ERP.Utilities.CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_SelectoneItem").ToString()) %>';
                    ShowErrorMessage(msg, msgTitle);
                    return false;
                }
            }
            else if (mode == 0) {
                if (count > 1) {
                    msg = '<%= ERP.Utilities.CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_SelectoneItem").ToString()) %>';
                    ShowErrorMessage(msg, msgTitle);
                    return false;
                }
            }
            else if (mode == 2) {
                if (count == 0) {
                    ShowErrorMessage(msg, msgTitle);
                    return false;
                }
            }
        }
        //Remove hyper link if Invoice amount and Balance amount are same
        function RemoveBalAmntHyperLink() {
            $("#<%= grdSalesInvoiceList.ClientID %> input[type=hidden][id*=hdfInvoiceID]").each(function (index) {
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

        $("[id*=chkSoSelect]").live("click", function () {
            var grid = $(this).closest("table");
            var chkHeader = $("[id*=chkSelectAllSo]", grid);
            if (!$(this).is(":checked")) {
                $("td", $(this).closest("tr")).removeClass("selected");
                chkHeader.removeAttr("checked");
            } else {
                $("td", $(this).closest("tr")).addClass("selected");
                if ($("[id*=chkSoSelect]", grid).length == $("[id*=chkSoSelect]:checked", grid).length) {
                    chkHeader.attr("checked", "checked");
                }
            }
        });

        function InitSONumberAuto() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtSONumber", url + "?CustomerID=" + $("[id$=hdfCustomer]").val() + "&InvoicePk=" + $("[id$=hdfInvPk]").val(), "hdfSOPK", true, true, "SCNUMBER");
            ResetSONumberAuto();
        }

        function ResetSONumberAuto() {
            var defText = '<%= Resources.ErpRes.AutoDefaultValue %>';
            $("[id$=txtSONumber]").val(defText);
            $("[id$=hdfSOPK]").val('-1');
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
                                    <asp:DropDownList ID="ddlCompany" class="select-full-a margnbotm0" TabIndex="1" runat="server"
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
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="50"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="51" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="52" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteNew" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="53" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>                                    
                                    <li>
                                        <asp:Button runat="server" TabIndex="55" ID="btnListPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="56" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                  
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="40" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="2" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelSI %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelSI %>" OnClientClick="javascript:return SelectedCheckBoxCount(1);" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="41" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" OnClientClick="javascript:return SelectedCheckBoxCount(1);" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="42" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" OnClientClick="javascript:return SelectedCheckBoxCount(1);" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="43" ID="btnPrint" CommandName="PRINTLISTING"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" OnClientClick="javascript:return SelectedCheckBoxCount(1);" />
                                    </li>
                                    <li runat="server" id="pnlResetSelection">
                                        <asp:Button runat="server" ID="btnResetSelection" CommandName="RESET" TabIndex="46"
                                            Text="<%$resources:ResetSelection %>" OnClick="ActionHandler" ToolTip="<%$resources:ResetSelection %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" OnClientClick="ResetSelection()" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
                <asp:HiddenField ID="hdfIsTaxForOtherCharge" runat="server" Value="0" />
                <asp:HiddenField ID="hdfCurrencyDigits" runat="server" Value="3" />
                <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                <asp:HiddenField ID="hdfCurrencyFormatWithSeperator" runat="server" />
                <asp:HiddenField ID="hdfRateFormat" runat="server" />
                <asp:HiddenField ID="hdfTaxSettings" runat="server" />
                <asp:HiddenField ID="hdfIscontYes" runat="server" />
                <asp:HiddenField ID="hdfPostingSettings" runat="server" />
                <%--  //For SelectedItemId Keeping--%>
                <asp:HiddenField ID="hdfSelectedItemPk" runat="server" Value="0" />
                <asp:HiddenField ID="hdfCustomerTypeId" runat="server" Value="0" />
                <div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="68" OnClick="ActionHandler" CommandName="INVOICELIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="69" OnClick="ActionHandler" CommandName="INVOICEDETAIL"
                                CssClass="tab-inactive" OnClientClick="javascript:return SelectedCheckBoxCount(1);"></asp:LinkButton>
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
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblFrmDate" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="3" CssClass="input-small margnrgt5"
                                                MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtToDate"
                                                CssClass="middle-lbl-small-c"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="3" CssClass="input-small" MaxLength="11"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblPlantName" Text="<%$ resources:Plant%>" AssociatedControlID="ddlPlantName"
                                                CssClass="lbl-24-7perc"></asp:Label>
                                            <asp:DropDownList ID="ddlPlantName" runat="server" CssClass="select-small-a2" TabIndex="15">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" AssociatedControlID="ddlStatus"
                                                CssClass="middle-lbl-small"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-b" TabIndex="3">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="lblSCno" runat="server" Text="<%$resources:SONo %>" AssociatedControlID="txtSCno"
                                                CssClass="middle-lbl-xsmall-b"></asp:Label>
                                            <asp:TextBox ID="txtSCno" runat="server" CssClass="input-small margnrgt1-5per" MaxLength="100"
                                                TabIndex="3"> </asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblCustomer" runat="server" Text="<%$resources:Customer %>" AssociatedControlID="txtCustomerSearch"></asp:Label>
                                            <asp:TextBox ID="txtCustomerSearch" runat="server" CssClass="select-half margnbotm0"
                                                MaxLength="100" TabIndex="3"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerSearchID" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblInvoiceNumber" runat="server" Text="<%$resources:InvNo %>" AssociatedControlID="txtInvoiceNumber"
                                                CssClass="middle-lbl-small"></asp:Label>
                                            <asp:TextBox ID="txtInvoiceNumber" runat="server" CssClass="input-small-a margnbotm0"
                                                MaxLength="100" TabIndex="3"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfIVHPK" runat="server" Value="" />
                                            <asp:Label ID="lblInType" runat="server" Text="<%$resources:SearchType %>" AssociatedControlID="ddlSaleOrderType"
                                                CssClass="middle-lbl-xsmall-b margnbotm0"></asp:Label>
                                            <asp:DropDownList ID="ddlSaleOrderType" runat="server" CssClass="select-small-a margnbotm0"
                                                TabIndex="3">
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="4" CommandName="SEARCH" SkinID="search-ext"
                                                Style="margin-top: 2px!important; margin-right: 4px!important; margin-bottom: 0px;" />
                                            <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="5" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext" Style="margin-top: 2px!important;
                                                margin-bottom: 0px;" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdSalesInvoiceList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" runat="server" GroupName="SelectOne" AutoPostBack="true"
                                                    OnCheckedChanged="ActionHandler" ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chkInvselect" TabIndex="9" />
                                                <asp:HiddenField runat="server" ID="hdfInvoiceID" Value='<%# Eval(Resources.DataFieldRes.SalesInvoicePK) %>' />
                                                <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval("ICH_DEPT") %>' />
                                                <asp:HiddenField ID="hdfInvType" runat="server" Value='<%# Eval("ICH_TYPE") %>' />
                                                <asp:HiddenField ID="hdfDelStatus" runat="server" Value='<%# Eval("ICH_DEL_STATUS") %>' />
                                                <asp:HiddenField ID="hdfTaxAmount" runat="server" Value='<%# Eval("ICH_TAX_TC") %>' />
                                                <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval("ICH_STATUS") %>' />
                                                <asp:HiddenField ID="hdfSalesContractType" runat="server" Value='<%# Eval("SOH_TYPE") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="false" />
                                            <ItemStyle Width="3%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceDate %>" SortExpression="<%$ resources:DataFieldRes,SalesInvoiceDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SalesInvoiceDate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SalesInvoiceDate, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" Wrap="false" />
                                            <HeaderStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceNo %>" SortExpression="<%$ resources:DataFieldRes,SalesInvoiceNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.SalesInvoiceNo) ==""?"[NEW]":Eval(Resources.DataFieldRes.SalesInvoiceNo)%>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SalesInvoiceNo)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblPlant" runat="server" CssClass="<%# Eval(Resources.DataFieldRes.CompnayLineColor) %>"
                                                    Text='<%# Eval(Resources.DataFieldRes.CMP_DISPLAY_CODE) %>' ToolTip='<%# Eval(Resources.DataFieldRes.CMP_DISPLAY_CODE)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Customer %>" SortExpression="<%$ Resources:DataFieldRes,InvoiceCustomerPK%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SICustomer),30) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SICustomer),250) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfCustomerPK" Value='<%# Eval("ICH_CUS_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="45%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvType1 %>" SortExpression="ICH_TYPE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SIType),3,"") %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SIType)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency %>" SortExpression="<%$ Resources:DataFieldRes,CurrencyCode%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='<%#Eval(Resources.DataFieldRes.SICurrency)  %>'
                                                    ToolTip='<%#Eval(Resources.DataFieldRes.SICurrency)  %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfSOCurrency" Value='<%# Eval(Resources.DataFieldRes.SOInvoiceCurrency) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceValue %>" SortExpression="<%$ resources:DataFieldRes,SalesInvoiceNetAmountTC %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceValue" runat="server" Text='<%# Eval(Resources.DataFieldRes.SIValue,"{0:c}") %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SIValue,"{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BalAmt %>">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbnBalAmt" runat="server" Text='<%# Eval(Resources.DataFieldRes.SalesBalAmt,"{0:c}")%>'
                                                    CssClass="text-underline" ToolTip='<%# Eval(Resources.DataFieldRes.SalesBalAmt,"{0:c}")%>'
                                                    OnClick="ActionHandler" CommandName="AMOUNTDETAILS"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Paybydate %>" SortExpression="<%$ resources:DataFieldRes,SalesInvoiceDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaybydate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SalesDueDate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SalesDueDate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="false" />
                                            <ItemStyle Width="8%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("ASC_CSS_CLASS") %>' ToolTip='<%# Eval("ICH_STATUS_TEXT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval(Resources.DataFieldRes.SApproved) %>' />
                                                <asp:HiddenField runat="server" ID="hdfPosted" Value='<%# Eval(Resources.DataFieldRes.SPosted) %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="2%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc2:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide tablelayout">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblInvoiceNo" Text="<%$ resources:InvoiceNo%>" AssociatedControlID="lblDispInvoiceNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblDispInvoiceNo" Text="" CssClass="input-small"></asp:Label>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCus" runat="server" Text="<%$resources:Customer %>" AssociatedControlID="txtCustomer"></asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="input-half" TabIndex="1"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomer" runat="server" />
                                            <%--<asp:RequiredFieldValidator ID="vrfCustomer" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" InitialValue="<%$ resources:ErpRes,AutoDefaultValue %>"
                                                runat="server" ControlToValidate="txtCustomer" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Customer %>">
                                            </asp:RequiredFieldValidator>--%>
                                            <asp:Button ID="btnCustomer" runat="server" EnableTheming="false" Style="display: none"
                                                OnClick="ActionHandler" CommandName="CUSTOMERCHANGE" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                <h1>
                                    <%= GetLocalResourceObject("PendingSO").ToString() %></h1>
                                <div class="button-wrap-right ">
                                    <asp:ImageButton runat="server" ID="imbShowPendingSO" OnClientClick="javascript:return ShowHidePendingSO(1);"
                                        SkinID="imbArrowInactive" ToolTip="<%$ resources:Controls,Show %>" />
                                    <asp:ImageButton runat="server" ID="imbHidePendingSO" OnClientClick="javascript:return ShowHidePendingSO();"
                                        Style="display: none" SkinID="imbArrowActive" ToolTip="<%$ resources:Controls,Hide %>" />
                                    <asp:HiddenField ID="hdfIsPendingSOVisible" runat="server" Value="0" />
                                </div>
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <div id="divPendingSODetails" style="display: none">
                                        <div class="gridwrap">
                                            <div id="searchwrap" class="search-wrap-custom1">
                                                <asp:Label ID="lblSoNo" runat="server" Text="<%$ resources:SalesOrder %>" AssociatedControlID="txtSONumber"></asp:Label>
                                                <div id="divSearchDtls">
                                                    <asp:TextBox ID="txtSONumber" runat="server" TabIndex="2"> </asp:TextBox>
                                                    <asp:HiddenField ID="hdfSOPK" runat="server" Value="" />
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
                                            <asp:GridView runat="server" ID="grdSoList" AutoGenerateColumns="False" GridLines="None"
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
                                                            <asp:CheckBox runat="server" ID="chkSoSelect" TabIndex="6" />
                                                            <asp:HiddenField runat="server" ID="hdfSCPK" Value='<%# Eval(Resources.DataFieldRes.SalesOrderPk) %>' />
                                                            <asp:HiddenField runat="server" ID="hdfSCCustomerPK" Value='<%# Eval(Resources.DataFieldRes.SOCustomerPK) %>' />
                                                            <asp:HiddenField runat="server" ID="hdfSCCurrency" Value='<%# Eval(Resources.DataFieldRes.SOCurrency) %>' />
                                                            <asp:HiddenField runat="server" ID="hdfSCType" Value='<%# Eval("SOH_TYPE") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="2%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:SODate %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSODate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SODate, Resources.Constants.DateFormatGrid) %>'
                                                                ToolTip='<%# Eval(Resources.DataFieldRes.SODate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="6%" Wrap="false" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:SONo %>">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkSoNo" CssClass="text-underline" runat="server" Text='<%# Eval(Resources.DataFieldRes.SONo) %>'
                                                                OnClick="ActionHandler" CommandName="SHOWPOPUP" CommandArgument='<%# Eval(Resources.DataFieldRes.SalesOrderPk) %>'
                                                                ToolTip='<%# Eval(Resources.DataFieldRes.SONo) %>'></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="8%" Wrap="false" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPlant" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("SOH_CMP_DISP_CODE")) %>'
                                                                ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("SOH_CMP_DISP_CODE")) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="2%" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="<%$ resources:CustomerName %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCustomerName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("SOH_CUSTOMER_NAME"),45) %>'
                                                                ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString( Eval("SOH_CUSTOMER_NAME")) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="33%" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:Type %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("SOH_TYPE_TEXT")) %>'
                                                                ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("SOH_TYPE_TEXT")) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="3%" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:Currency %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCurrency" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("SOH_CURRENCY_TEXT")) %>'
                                                                ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("SOH_CURRENCY_TEXT")) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="2%" HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:TotalAmount %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAmount" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval("SOH_NET_AMOUNT")) %>'
                                                                ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("SOH_NET_AMOUNT")) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="11%" HorizontalAlign="Right" Wrap="false" />
                                                        <HeaderStyle CssClass="amount-numeric" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <%--<uc2:PagerControl ID="uclSOPaging" runat="server" />--%>
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
                                            <asp:Label runat="server" ID="lblInvdate" Text="<%$ resources:Invdate%>" AssociatedControlID="txtInvdate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInvdate" CssClass="input-small" TabIndex="10"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfInvdate" runat="server" Value="" />
                                            <asp:RequiredFieldValidator ID="vrfInvdate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" InitialValue="" runat="server"
                                                ControlToValidate="txtInvdate" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Invdate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblPaybydate" Text="<%$ resources:Paybydate%>" AssociatedControlID="txtPaybydate"
                                                CssClass="middle-lbl-small-c"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPaybydate" TabIndex="10" CssClass="input-small"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfPaybydate" runat="server" Value="" />
                                            <asp:RequiredFieldValidator ID="vrfPaybydate" CssClass="star" SetFocusOnError="false"
                                                ValidationGroup="invoice" EnableClientScript="true" InitialValue="" runat="server"
                                                ControlToValidate="txtPaybydate" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Paybydate %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblInvoiceType" runat="server" AssociatedControlID="ddlInvoiceType"
                                                Text="<%$ resources:InvoiceType %>">
                                            </asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlInvoiceType" AutoPostBack="true" TabIndex="11"
                                                OnSelectedIndexChanged="ActionHandler" CssClass="select-small-b">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfInvoiceType" runat="server" ControlToValidate="ddlInvoiceType"
                                                Text="*" CssClass="star" InitialValue="-1" ValidationGroup="invoice" EnableClientScript="true"
                                                SetFocusOnError="true" ErrorMessage="<%$ resources:Err_InvoiceType %>"></asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblCurrency" Text="<%$ resources:Currency%>" AssociatedControlID="txtCurrr"
                                                CssClass="middle-lbl-small-a"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtCurrr" Text="" class="input-small input-disabled" TabIndex="11"
                                                MaxLength="100" Enabled="false"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCustomerType" runat="server" AssociatedControlID="ddlCustomerType"
                                                Text="<%$ resources:CustomerType %>">
                                            </asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlCustomerType" CssClass="select-small-g margnrgt1-5per"
                                                AutoPostBack="true" TabIndex="11" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfCustomerType" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="vatbuy" EnableClientScript="true" InitialValue="-1" runat="server"
                                                ControlToValidate="ddlCustomerType" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Type %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:TextBox runat="server" ID="txtTypeID" Text="" CssClass="input-small" TabIndex="13"
                                                MaxLength="5"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfBranchCode" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="vatbuy" EnableClientScript="true" runat="server" ControlToValidate="txtTypeID"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_BranchCode %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblTaxID" Text="<%$ resources:TaxId%>" AssociatedControlID="txtTaxID"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTaxID" Text="" TabIndex="14" MaxLength="100" CssClass="select-half"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <asp:GridView ID="grdSalesList" runat="server" AutoGenerateColumns="False" Width="100%"
                                    PageSize="<%$ resources:PageSize %>" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable"
                                    AllowSorting="false" ShowFooter="true">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:SONo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkSoNo" CssClass="text-underline" runat="server" OnClick="ActionHandler"
                                                    CommandName="SHOWPOPUP" Text='<%# Eval("SOH_NO") %>' CommandArgument='<%# Eval("SOH_PK") %>'></asp:LinkButton>
                                                <asp:HiddenField ID="hdfSONumber" Value='<%# Eval("SOH_PK") %>' runat="server" />
                                                <asp:HiddenField ID="hdfPriceAdjustment" Value='<%# Eval("SOH_AJUST_AMT") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SOdate %>" Visible="false" SortExpression="">
                                            <ItemTemplate>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Customer %>" SortExpression="" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerInv" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency %>" SortExpression="" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GrossAmount %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrossAmount" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval("SOH_GROSS_AMT")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("SOH_GROSS_AMT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Discount %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDiscount" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval("SOH_DISCOUNT_AMT")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("SOH_DISCOUNT_AMT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Tax %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblTax" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval("SOH_TAX_AMT")) %>'
                                                    CssClass="text-underline nomargin" OnClick="ActionHandler" CommandName="TAXDETAILSSPLITUP"
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("SOH_TAX_AMT")) %>' CommandArgument='<%# Eval("SOH_PK") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" HorizontalAlign="Right" Wrap="false" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SOothers %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOtherAmount" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval("SOH_OTHER_CHARGES")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("SOH_OTHER_CHARGES")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amt Adj." SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAdj" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval("SOH_AJUST_AMT")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("SOH_AJUST_AMT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TotalAmount %>" SortExpression="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalAmount" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval("SOH_TOTAL_AMT")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("SOH_TOTAL_AMT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Invoiced %>" SortExpression="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiced" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval("ICM_INVOICED_AMT")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("ICM_INVOICED_AMT")) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfInvoiced" />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ProformaInvoiced %>" SortExpression=""
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProformaInvoiced" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval("ICM_PINVOICED_AMT")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("ICM_PINVOICED_AMT")) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfProformaInvoiced" />
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BalancetoInvoice %>" SortExpression=""
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalancetoInvoice" CssClass="BalancetoInvoice" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Eval("ICM_BAL_TO_INVOICE")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("ICM_BAL_TO_INVOICE")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblfooter" Text="<%$ resources:Total %>"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoicedNow %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPayNow" runat="server" onkeyup="CalculateTotal();" CssClass="input-w70 numeric"
                                                    MaxLength="15" TabIndex="15" Width="90%" Text='<%# GetFormattedCurrency(Eval("ICM_AMOUNT")) %>'></asp:TextBox>
                                                <asp:HiddenField runat="server" ID="hdfPayNow" />
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
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalPayNowFooter"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SOotherChrg %>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtOthercharges" ToolTip="Non taxable" runat="server" Text='<%# GetFormattedCurrency(Eval("ICM_OTHER_AMOUNT")) %>'
                                                    CssClass="input-w70 numeric" onkeyup="CalculateTotal();" MaxLength="15" TabIndex="16"></asp:TextBox>
                                                <asp:HiddenField ID="hdfOtherchargeOLD" Value='' runat="server" />
                                                <cc1:AmountValidation ID="vreOthercharges" runat="server" ControlToValidate="txtOthercharges"
                                                    ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"></cc1:AmountValidation>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" Width="12%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblOtherchargesFooter" CssClass="amount-numeric"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Tax %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSOTax" runat="server" ToolTip='<%# GetFormattedCurrencyWithSeperator(Eval("ICM_TAX_AMOUNT")) %>'></asp:Label>
                                                <asp:HiddenField ID="hdfSOTax" Value='0' runat="server" />
                                                <asp:HiddenField ID="hdfSODiscount" Value='0' runat="server" />
                                                <asp:HiddenField ID="hdfAdjustPerInvAmt" Value='0' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkRemove" TabIndex="17" runat="server" OnClick="ActionHandler" SkinID="delete-icon"
                                                    CommandName="REMOVE" ToolTip="Remove" OnClientClick="return ShowDeleteConfirm(this);" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
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
                                                        <asp:Label ID="lblTaxName" runat="server" Text='<%#Eval("TSH_NAME") %>' ToolTip='<%# HttpUtility.HtmlDecode(Eval("TSH_NAME").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTaxName" runat="server" Value='<%#Eval("TSH_NAME") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:TaxAmount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("TSH_TAX_AMT")) %>'
                                                            ToolTip='<%#GetFormattedCurrency(Eval("TSH_TAX_AMT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="22%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
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
                                        <div id="DivtaxFooter" runat="server" class="div2col-S">
                                            <asp:Label runat="server" ID="lblTaxAmount" Text="<%$ resources:TaxAmount%>" AssociatedControlID="txtTaxAmount"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTaxAmount" CssClass="input-small numeric input-disabled"
                                                MaxLength="17" onkeydown="return EnableArrowKey(event);" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfTaxAmount" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" InitialValue="" runat="server"
                                                ControlToValidate="txtTaxAmount" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TaxAmount %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div id="DivDiscFooter" runat="server" class="div2col-S">
                                            <asp:Label runat="server" ID="lblDiscount" Text="<%$ resources:Discount%>" AssociatedControlID="txtDiscount"></asp:Label>
                                            <asp:TextBox ID="txtDiscount" runat="server" CssClass="input-small numeric input-disabled"
                                                onkeydown="return EnableArrowKey(event);" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfDiscount" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtDiscount"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Discount%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblInvoiceAmt" Text="<%$ resources:InvoiceAmt%>" AssociatedControlID="txtInvoiceAmt"></asp:Label>
                                            <asp:TextBox ID="txtInvoiceAmt" runat="server" onkeydown="return EnableArrowKey(event);"
                                                onpaste="return false;" CssClass="input-small numeric input-disabled" MaxLength="20"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblNetAmount" Text="<%$ resources:NetAmount%>" AssociatedControlID="txtNetAmount"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtNetAmount" CssClass="input-small numeric input-disabled"
                                                MaxLength="20" onkeydown="return EnableArrowKey(event);" onpaste="return false;"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks %>" AssociatedControlID="txtRemarks"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRemarks" TabIndex="18" TextMode="MultiLine" CssClass="multiline-2line"
                                                onkeydown="limitText(this,450);" onchange="limitText(this,450);"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfRemarks" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" InitialValue="" runat="server"
                                                ControlToValidate="txtRemarks" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Remarks %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div id="divConfirmationWithReason" style="display: none">
                                <div class="content-wrapper">
                                    <div class="divcol-P">
                                        <h5>
                                            Do you want to delete?</h5>
                                        <div class="clear">
                                        </div>
                                        <asp:Label ID="lblReason" Text="Reason" runat="server" AssociatedControlID="txtReason"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtReason" TextMode="MultiLine" CssClass="multiline-3col"
                                            onkeydown="limitText(this,450);" onchange="limitText(this,450);"></asp:TextBox>
                                        <asp:Label ID="lbl" runat="server" AssociatedControlID="btnDeleteOK"></asp:Label>
                                        <asp:Button ID="btnDeleteOK" runat="server" Text="Ok" SkinID="btnInner-ok" OnClick="ActionHandler"
                                            CommandName="INACTIVE" CommandArgument="SEC_ActionPanel" />
                                        <asp:Button ID="btnDeleteCancel" runat="server" Text="Cancel" SkinID="btnInner-Cancel"
                                            OnClientClick="return closeDeletePopup();" />
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
                                                        <asp:Label ID="lblTrxNo" runat="server" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("RCH_NO"))) ? Resources.ErpRes.Draft : Convert.ToString(Eval("RCH_NO")) %>'
                                                            ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("RCH_NO"))) ? Resources.ErpRes.Draft : Convert.ToString(Eval("RCH_NO")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate" runat="server" Text='<%#  Eval("RCH_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("RCH_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                            ToolTip='<%# Eval("RCH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:TrxAmount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("RCVD_AMOUNT")) %>'
                                                            ToolTip='<%#GetFormattedCurrency(Eval("RCVD_AMOUNT")) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfAmountSplit" runat="server" Value='<%#Eval("RCVD_AMOUNT") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" HorizontalAlign="Right" />
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
                            <%---------------------------------End Paid Amount details Popup-------------------------------------%>
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
                </div>
                <asp:HiddenField ID="hdfInvoiceNo" runat="server" />
                <asp:HiddenField ID="hdfInvRcvdAmt" runat="server" />
                <asp:HiddenField ID="hdfExchangeRate" runat="server" />
                <asp:HiddenField ID="hdfTaxAmt" runat="server" />
                <asp:HiddenField ID="hdfDiscAmt" runat="server" />
                <%--  <asp:HiddenField ID="hdfCustomerCountry" runat="server" />--%>
                <asp:HiddenField ID="hdfInvoiceReference" runat="server" />
                <asp:HiddenField runat="server" ID="hdfOCFooter" />
                <asp:HiddenField ID="hdfInvDelStatus" runat="server" Value="0" />
                <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
                <asp:HiddenField ID="hdfCurrencyGroup2" Value="2" runat="server" />
            </div>
            <asp:HiddenField runat="server" ID="hdfTotalPayNowFooter" />
            <asp:HiddenField ID="hdfIsJournalize" runat="server" />
            <%--User Control--%>           
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server">
                </uc1:WorkflowUserComments>
            </div>
            <asp:HiddenField ID="hdfJournalizeWorkFlow" Value="0" runat="server" />
            <asp:HiddenField ID="hdfType" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
            <asp:HiddenField runat="server" ID="hdfInvPk" Value="0" />           
            <asp:HiddenField runat="server" ID="hdfInvCategory" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
