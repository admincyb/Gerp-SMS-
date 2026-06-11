<%@ Page Title="<%$ Resources:Captions,Title_SOMiscInv %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="Miscellaneous.aspx.cs"
    Inherits="ERPSMS_v01.Sales.Miscellaneous" %>

<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/AlertControl.ascx" TagName="Alert" TagPrefix="uc2" %>
<%@ Register Src="~/Journalize/UserControls/JournalizeControlNew.ascx" TagName="Journalize"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .grayText
        {
            color: Gray !important;
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
        var uiUrl = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");

        function InitComponents() {

            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url + "?IsSBUCustomer=" + $("[id$='hdfIsSBUCustomer']").val(), "hdfCustomerID", true, true, "CUSTOMERLIST");
            //            GrandScriptUtils.MakeAutoCompleteDDL("txtItem", url + "?Type=ITM_NAME", "hdfItemID", true, true, "GETALLITEMS", "", false, false, false, 3, "Type min. 3 characters");
            GrandScriptUtils.MakeAutoCompleteDDL("txtItem", url + "?Type=''", "hdfItemID", true, true, "GETALLITEMSWITHCODENAME", "", false, false, false, 3, "Type min. 3 characters");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomerLIST", url + "?IsSBUCustomer=" + $("[id$='hdfIsSBUCustomer']").val(), "hdfCustomerLIST", true, true, "CUSTOMERLIST");
            GrandScriptUtils.MakeAutoCompleteDDL("txtInvoiceNumber", url + "?Type=" + $("[id$=hdfgroup]").val(), "hdfIVHPK", true, true, "SALINVOICENUMBER");

            //GrandScriptUtils.AddDateRangeCommon("txtInvoiceDate", "hdfInvoiceDate", "txtInvoiceDueDate", "hdfInvoiceDueDate", "dd-M-yy", false, false, false);
            GrandScriptUtils.AddDateRange("txtInvoiceDate", "hdfInvoiceDate", "txtInvoiceDueDate", "hdfInvoiceDueDate", false, false, false, false);

            GrandScriptUtils.MakeAutoCompleteDDL("txtFromPort", uiUrl + "?SIType=" + $("[id$=ddlInvoiceType]").val() + "&SaleFromPort=1", "hdfFromPortID", true, true, "FILLPORTDETAILS");

            GrandScriptUtils.DatePickerCommon("txtExpenseDate");
            //GrandScriptUtils.DatePickerCommon("txtInvoiceDueDate");
            GrandScriptUtils.DatePickerCommon("txtInvoiceDate");
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false, false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendorSch", url, "hdfVendorSch", true, true, "PARTY");
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendor", url, "hdfVendor", true, true, "PARTY");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCurrency", url, "hdfCurrency", true, true, "CURRENCY");
            GrandScriptUtils.MakeAutoCompleteDDL("txtExpenseNumber", url, "hdfExpenseNumberPK", true, true, "EXPENSEINVNUMBER");
            GrandScriptUtils.MakeAutoCompleteDDL("txtUOM", url, "hdfUOM", true, true, "UOM");
            GrandScriptUtils.MakeAutoCompleteDDL("txtUOMwt", url + "?Type=" + "1", "hdfUOMwt", true, true, "UOM");
            ShowHideItemDetails($("[id$=hdfIsItemDetailsVisible]").val());
            ShowHideOtherDetails($("[id$=hdfIsOtherDetailsVisible]").val());
            GrandScriptUtils.AddDateRangeCommon("txtETD", "hdfETD", "txtETA", "hdfETA", "dd-M-yy", false, false, false);
            //For Voucher
            GrandScriptUtils.DatePickerCommon("txtPVDate");
            //$("[id$=txtJournalExchangeRate]").ForceNumericOnly();

            $("[id*=txtNetWt]").ForceNumericOnly();
            $("[id*=txtGrossWt]").ForceNumericOnly();


            if ($('[id$=btnJournalSaveSubmit]').is(":visible"))
                $('[id$=btnJournalSubmit]').hide();
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();

            if ($("[id$=txtItem]").val() == 'Type min. 3 characters') {
                $("[id$=txtItem]").addClass("grayText");
            }
            else {
                $("[id$=txtItem]").removeClass("grayText");
            }
            if ($("[id$=hfCancelInv]").val() == "1") {
                $("[id$=btnAddItem]").hide();
                $("[id$=btnSave]").hide();
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
            }
            else
                $("[id$=tblDetailHdr]").addClass("table-devide");

            if ($("[id$=hdfSelRecordStatus]").val() == 0) {
                $("[id$=btnEditforCancel]").hide();
            } else {
                $("[id$=btnEditforCancel]").show();
            }
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
                $("[id$=btnAddItem]").hide();
                $("[id$=pnlSaveSubmit]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlPrint]").hide();
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlAlert]").hide();
                $("[id$=btnJournalize]").hide();
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
            if (targetControlID == "txtCustomer") {
                $("[id$=btnCustomer]").click();
            }
            if (targetControlID == "txtCurrency") {
                $("[id$=btnCurrency]").click();
            }
            if (targetControlID == "txtItem") {
                $("[id$=txtItem]").removeClass("grayText");
                $("[id$=txtDesc]").val($("[id$=txtItem]").val());
                FillMaterialUoM($("[id$=hdfItemID]").val());
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
            if (targetControlID == "txtItem") {
                $("[id$=txtItem]").addClass("grayText");
            }
        }
        function CalculateAmount() {
            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }
            if (!isNaN(parseFloat($("#[id*=hdfMiscNumberDigits]").val()))) {
                MiscRateDecimalDigits = parseFloat($("#[id*=hdfMiscNumberDigits]").val());
            }
            var qty = 0;
            var rate = 0;
            qty = parseFloat($("[id$=txtQty]").val());
            rate = parseFloat($("[id$=txtRate]").val());
            if (!isNaN(qty) && !isNaN(rate)) {
                var amount = qty * rate;
                if (((amount.toFixed(NumberDigits)).length) <= 15) {
                    //                    $("[id$=txtAmount]").val(amount.toFixed(MiscRateDecimalDigits).replace(new RegExp(',', 'g'), ''));
                    $("[id$=txtAmount]").val(amount.toFixed(DecimalDigits).replace(new RegExp(',', 'g'), ''));
                } else {

                    //                ShowErrorMessage('<%= GetLocalResourceObject("Err_Amount_Valid").ToString() %>');
                }
            }
            else {
                if (((parseFloat(0).toFixed(NumberDigits)).length) <= 15) {
                    //                    $("[id$=txtAmount]").val(parseFloat(0).toFixed(MiscRateDecimalDigits).replace(new RegExp(',', 'g'), ''));
                    $("[id$=txtAmount]").val(parseFloat(0).toFixed(DecimalDigits).replace(new RegExp(',', 'g'), ''));
                } else {

                    //                 ShowErrorMessage('<%= GetLocalResourceObject("Err_Amount_Valid").ToString() %>');
                }
            }
        }
        function CalculateTotal(sender) {
            var subTotal = $("#[id*=grdItemDetails]").find('[id$=lblSubTotalFooter]').length > 0 ? parseFloat($("#[id*=grdItemDetails]").find('[id$=lblSubTotalFooter]').html().replace(new RegExp(',', 'g'), '')) : parseFloat(0);
            subTotal = isNaN(subTotal) ? 0 : subTotal;
            var totalDiscount = parseFloat($("[id$=txtHdrDiscount]").val().replace(new RegExp(',', 'g'), ''));
            totalDiscount = isNaN(totalDiscount) ? 0 : totalDiscount;           
            var totalShipping = parseFloat($("[id$=txtShipping]").val().replace(new RegExp(',', 'g'), ''));
            totalShipping = isNaN(totalShipping) ? 0 : totalShipping;           
            var totalTax = parseFloat($("[id$=txtHdrTax]").val().replace(new RegExp(',', 'g'), ''));
            totalTax = isNaN(totalTax) ? 0 : totalTax;
            var totalPriceAdj = parseFloat($("[id$=txtPriceAdj]").val().replace(new RegExp(',', 'g'), ''));
            totalPriceAdj = isNaN(totalPriceAdj) ? 0 : totalPriceAdj;

            var netTotal = (subTotal + totalTax + totalPriceAdj + totalShipping) - totalDiscount;
            $("[id$=txtHdrTotal]").val((netTotal).toFixed(CurrencyDigits));
            $("[id$=hdfSubTotal]").val((subTotal).toFixed(CurrencyDigits));
            $("[id$=txtHdrTotal]").attr("title", (netTotal).toFixed(CurrencyDigits));
            if (totalPriceAdj == 0)
                $("[id$=txtPriceAdj]").val((totalPriceAdj).toFixed(CurrencyDigits));
            $("[id$=txtPriceAdj]").val().replace(new RegExp(',', 'g'), '');
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
                    //ShowContainerDiv('[id$=divJournalize]', '<%= GetLocalResourceObject("Purchase_Expense_Journal") %>', '1000', '550');
                    ShowCommonCotainerDiv('[id$=divJournalize]', '<%= GetLocalResourceObject("Purchase_Expense_Journal") %>', "1%");
                    AfterCloseWkfInJournal();
                    //$("[id$=btnJournalize_Action]").click();
                }
            }
        }
        //        function CalculateDueDate() {
        //            var vendDate = $.datepicker.parseDate("dd-M-yy", $("[id$=txtVendorInvDate]").val());
        //            var dueDays = parseInt($("[id$=txtCreditDays]").val());
        //            if (vendDate != null && !isNaN(vendDate) && dueDays != null && !isNaN(dueDays)) {
        //                vendDate.setDate(vendDate.getDate() + dueDays);
        //                $("[id$=txtInvoiceDueDate]").val($.datepicker.formatDate("dd-M-yy", vendDate));
        //            }
        //        }
        function CalculateDueDays() {
            var vendDate = $.datepicker.parseDate("dd-M-yy", $("[id$=txtInvoiceDate]").val());
            var dueDate = $.datepicker.parseDate("dd-M-yy", $("[id$=txtInvoiceDueDate]").val());
            if (vendDate != null && !isNaN(vendDate) && dueDate != null && !isNaN(dueDate)) {
                var dueDays = (dueDate - vendDate) / (1000 * 60 * 60 * 24);
                if (dueDays >= 0)
                    $("[id$=txtCreditDays]").val(dueDays);
            }
        }
        function AfterDateSelect(controlID) {
            if (controlID == "txtInvoiceDate") {
                var invoiceDueDate = $.datepicker.parseDate("dd-M-yy", $("[id$=txtInvoiceDate]").val());
                invoiceDueDate.setDate(invoiceDueDate.getDate() + parseInt($("[id$=hdfCreditDays]").val()));
                $("[id$=hdfInvoiceDueDate]").val($.datepicker.formatDate("mm/dd/yy", invoiceDueDate));
                $("[id$=txtInvoiceDueDate]").val($.datepicker.formatDate("dd-M-yy", invoiceDueDate));
                if ($("[id$=hdfHasTax]").val() != "0") {
                    if ($("[id$=hdfTaxFrmDt]").val() != "" && $("[id$=hdfTaxToDt]").val() != "") {

                        var taxfromDate = $.datepicker.parseDate("dd-M-yy", $("[id$=hdfTaxFrmDt]").val());
                        var taxtoDate = $.datepicker.parseDate("dd-M-yy", $("[id$=hdfTaxToDt]").val());
                        if (invoiceDueDate < taxfromDate || invoiceDueDate > taxtoDate) {
                            $("[id$=txtInvoiceDate]").val($("[id$=hdfOldInvDate]").val());
                            ShowErrorMessage('<%=Resources.Messages.TaxDateChanged %>', '<%=Resources.Messages.Information %>');
                        }
                    }

                }

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
            else if ((controlID != "txtFromDate") && (controlID != "txtInvoiceDueDate") && (controlID != "txtToDate") && (controlID != "txtToDate")) {
                $("[id$=btnTermCheck]").click();
            }
        }
        function ResetSelection() {
            $('[id$=grdMISCList]').find('tr td input:radio[id$=rbtSelect]').removeAttr('checked');
        }
        function ShowHideItemDetails(flag) {
            ///<summary>
            /// Used to Show/Hide ItemDetails Div
            ///</summary>

            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divItemDetails]").show();
                $("[id$=imbShowItemDetails]").hide();
                $("[id$=imbHideItemDetails]").show();
            }
            else {
                $("[id$=divItemDetails]").hide();
                $("[id$=imbShowItemDetails]").show();
                $("[id$=imbHideItemDetails]").hide();
            }
            $("[id$=hdfIsItemDetailsVisible]").val(flag);
            return false;
        }
        function ShowHideOtherDetails(flag) {
            ///<summary>
            /// Used to Show/Hide ItemDetails Div
            ///</summary>

            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divOtherDetails]").show();
                $("[id$=imbShowOtherDetails]").hide();
                $("[id$=imbHideOtherDetails]").show();
            }
            else {
                $("[id$=divOtherDetails]").hide();
                $("[id$=imbShowOtherDetails]").show();
                $("[id$=imbHideOtherDetails]").hide();
            }
            $("[id$=hdfIsOtherDetailsVisible]").val(flag);
            return false;
        }
         function CalculateTotalOtherCharge() {
            //var val1 = parseFloat($(sender).val());
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
        function ShowHideTaxAmtinBaseCurrency(flag) {
            ///<summary>
            /// Used to Show/Hide ItemDetails Div
            ///</summary>

            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divTaxAmtBaseCurr]").show();
                $("[id$=imbShowBaseTaxDetails]").hide();
                $("[id$=imbHideBaseTaxDetails]").show();
            }
            else {
                $("[id$=divTaxAmtBaseCurr]").hide();
                $("[id$=imbShowBaseTaxDetails]").show();
                $("[id$=imbHideBaseTaxDetails]").hide();
            }
            $("[id$=hdfIsItemDetailsVisible]").val(flag);
            return false;
        }

        function SetOldDate() {
            var OldDate = $.datepicker.parseDate("dd-M-yy", $("[id$=txtInvoiceDate]").val());
            $("[id$=hdfOldInvDate]").val($.datepicker.formatDate("dd-M-yy", OldDate));
        }



        //For Setting/Resetting Colour of a selected Row
        function SetSelectedRowColor() {
            var selectedIds;
            var selectedIdsArray = new Array();
            selectedIds = $("[id$=hdfSelectedItemPk]").val();
            selectedIdsArray = selectedIds.split(',');

            for (i = 0; i < selectedIdsArray.length; ++i) {

                if (selectedIdsArray[i] != 0) {
                    $("#<%= grdMiscList.ClientID %> input[type=hidden][id*=hdfInvoiceID]").each(function (index) {
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



        //For Setting Enable & disable of Requiredfield UOM Weight (vrfUOMwt)

        function SetEnableDisableReqfld() {
            var valName = document.getElementById("<%=vrfUOMwt.ClientID%>");
            if ($("[id*=txtNetWt]").val() != "") {
                ValidatorEnable(valName, true);
            }
            else {
                ValidatorEnable(valName, false);
            }
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


        //TAx Payable in MYR Div hide show

        function ShowHideTaxPayableOuter() {

            if ($("[id$=hdfIsTaxPayable]").val() == 1) {
                $("#divOuterTaxPayable").show();
            }
            else {
                $("#divOuterTaxPayable").hide();
            }
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

        //For Checking Rate
        function ShowRateConfirm() {
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
                        $("[id$=hdfIsRatecont]").val(1);
                        $(this).dialog("close");
                        $("[id$=btnAddItem]").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfIsRatecont]").val(0);
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
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

        //For   check  Inv.Now Qty is greater than grn quantity (grnqty-Invoiced qty) while apply        
        function ShowIssueNowQtyExceeds() {

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Msg_IssueNowQty_Confirm").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfIssueExceed]").val(1);
                        $(this).dialog("close");
                        $("[id$=btnIssueQtyApply]").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfIssueExceed]").val(0);
                        $(this).dialog("close");
                        ShowContainerDiv('[id$=divIssueQtySpilup]', '<%= GetLocalResourceObject("IssueAllocationDetails") %>', '700', '300');
                        return false;
                    }
                }
            });
            return false;
        }


        function ResetAllocation() {
            $("[id$=btnResetAllocation]").click();
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

        function CalculateTotalIssue() {
            var TotalIssueQty = 0;
            $("#[id*=grdIssueDetails] input[type=text][id*=txtInvNowIssueQty]").each(function (index) {
                if ($.trim($(this).val()) != "") {
                    if (!isNaN(parseFloat($(this).val()))) {
                        TotalIssueQty = TotalIssueQty + parseFloat($(this).val().replace(/[^0-9\.]+/g, ""));
                    }
                }
            });
            $("#[id*=grdIssueDetails] [id*=lblTotalIssueInvNow]").html(addCommas(TotalIssueQty.toFixed(NumberDigits)));

        }

        function DisableUOMAutocomplete() {
            DisableAuto($("[id$=txtUOM]"), $("[id$=hdfUOM]"));
        }
        function EnableUOMAutocomplete() {
            EnableAuto($("[id$=txtUOM]"), $("[id$=hdfUOM]"));
        }

        function FillMaterialUoM(materialID) {
            ///<summary>Function Used Fill the material Details corresponding to the id </summary>
            // $("[id$=UOMConversion]").val("1");
            $.get("MaterialManagement.do?Action=GetMaterialDetails&SBUPk=" + $("[id$=BizUnitPk]").val() + "&MaterialID=" + materialID, function (data) {
                if (data) {
                    if (materialID != 0) {
                        $("[id$=hdfUOM]").val(data[0].ITM_UOM);
                        $("[id$=txtUOM]").val(data[0].UOM_CODE);

                    }
                }
            });
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

        //Remove hyper link if Invoice amount and Balance amount are same
        function RemoveBalAmntHyperLink() {
            $("#<%= grdMiscList.ClientID %> input[type=hidden][id*=hdfInvoiceID]").each(function (index) {
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlExpenses">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <div id="divSBUCompany" class="buttoncontainer-fields floatLeft">
                                    <asp:DropDownList ID="ddlCompany" class="medium" TabIndex="1" runat="server" onmouseover="javascript:ShowTooltip('ddlCompany');">
                                    </asp:DropDownList>
                                </div>
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="55"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="56"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('invoice')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="57" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('invoice')" ValidationGroup="invoice"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="58" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('invoice')" ValidationGroup="invoice"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteNew" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="59" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li id="pnlPrint" runat="server">
                                        <asp:Button runat="server" TabIndex="60" ID="btnPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Visible="true" Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="61" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnJournalize" CommandName="JOURNALIZE" TabIndex="62"
                                            Text="<%$resources:Journalize %>" OnClick="ActionHandler" ToolTip="<%$resources:Journalize %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-journalize" />
                                    </li>
                                    <li id="pnlAlert" runat="server">
                                        <asp:Button runat="server" ID="btnAlert" CommandName="ALERT" TabIndex="63" Text="<%$resources:Controls,Alert %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:Controls,Alert %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-alert" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li runat="server" id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="64" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelMisc %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelMisc %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="65" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="66" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="67" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                    <li runat="server" id="Li1">
                                        <asp:Button runat="server" TabIndex="68" ID="btnListPrint" CommandName="PRINTLISTING"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li runat="server" id="Li2">
                                        <asp:Button runat="server" TabIndex="68" ID="btnDOPrint" CommandName="DOPRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,PrintDO %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,PrintDO %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnPickForReceipt" CommandName="PICKFORRECEIPT" TabIndex="69"
                                            Text="<%$resources:PickInvforReceipt %>" OnClick="ActionHandler" ToolTip="<%$resources:PickInvforReceipt %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-receipt" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnPickForCrDrNote" CommandName="PICKFORCRDRNOTE"
                                            TabIndex="69" Text="<%$resources:PickForCrDrNote %>" OnClick="ActionHandler"
                                            ToolTip="<%$resources:PickForCrDrNote %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-moneycredit" />
                                    </li>
                                    <li runat="server" id="pnlResetSelection">
                                        <asp:Button runat="server" ID="btnResetSelection" CommandName="RESET" TabIndex="70"
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
                                CommandArgument="SEC_ActionPanel" TabIndex="71" CssClass="tab-inactive" OnClick="ActionHandler"
                                CommandName="DEFAULT"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnDeliveryOrder" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowDO %>">
                            <asp:LinkButton runat="server" ID="lnbDeliveryOrder" Text="<%$resources:PageNameRes,DeliveryOrder %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="72" OnClick="ActionHandler" CommandName="DELIVERYORDER"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnSalesInvoice" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowSalesAdvInvoice %>">
                            <asp:LinkButton runat="server" ID="lnbSalesInvoice" Text="<%$resources:PageNameRes,AdvanceInvoice %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="73" OnClick="ActionHandler" CommandName="SALESINVOICE"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnAdvanceInvoice" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowSalesInvoice %>">
                            <asp:LinkButton runat="server" ID="lnbAdvanceInvoice" Text="<%$resources:PageNameRes,SalesInvoice %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="74" OnClick="ActionHandler" CommandName="INVOICE"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="Spnmiscellaneous" runat="server" class="tab-active" visible="<%$ resources:ConfigurationsRes,TabShowMiscInvoice %>">
                            <asp:LinkButton runat="server" ID="lnbMiscellaneous" Text="<%$resources:PageNameRes,miscellaneous %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="75" OnClick="ActionHandler" CommandName="MISC"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnSalesReceipt" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowReceipt %>">
                            <asp:LinkButton runat="server" ID="lbnSalesReceipt" Text="<%$resources:PageNameRes,SalesReceipt %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="76" OnClick="ActionHandler" CommandName="SALESRECEIPT"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnCrDrNote" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowSalesCRDR %>">
                            <asp:LinkButton runat="server" ID="lnbCrDrNote" Text="<%$resources:PageNameRes,CreditDebitNotes %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="77" OnClick="ActionHandler" CommandName="CRDRNOTE"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnAcPayables" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowAR %>">
                            <asp:LinkButton runat="server" ID="lnbAcPayables" Text="<%$resources:PageNameRes,AccountReceivables %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="78" OnClick="ActionHandler" CommandName="ACRECEIVABLE"
                                CssClass="tab-inactive"></asp:LinkButton>
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
                                CommandArgument="SEC_ActionPanel" TabIndex="79" OnClick="ActionHandler" CommandName="INVOICELIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="80" OnClick="ActionHandler" CommandName="INVOICEDETAIL"
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
                                                TabIndex="45" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="45" />
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
                                            <asp:Label ID="lblFrmDate" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="46" CssClass="input-small"
                                                MaxLength="13" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$resources:ToDate %>" CssClass="middle-lbl-small-c-20-11"
                                                AssociatedControlID="txtToDate"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="47" CssClass="input-small" MaxLength="13"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblSCno" Visible="false" runat="server" Text="<%$resources:SONo %>"
                                                AssociatedControlID="txtSCno"></asp:Label>
                                            <asp:TextBox ID="txtSCno" Visible="false" runat="server" CssClass="input-small" MaxLength="100"
                                                TabIndex="50"> </asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" class="middle-lbl-small"
                                                AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-d" TabIndex="48">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,NotPosted %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Posted %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label runat="server" ID="lblchkBalAmt" CssClass="middle-lbl-xsmall-b margnbotm0"
                                                Text="<%$ resources:PendingBal%>" AssociatedControlID="chkBalAmt"></asp:Label>
                                            <asp:CheckBox ID="chkBalAmt" runat="server" Checked="true" TabIndex="49" />
                                            <asp:Label ID="lblCompany" runat="server" Text="<%$ resources:Controls, CompanyPlant %>"
                                                AssociatedControlID="ddlCompanySrch" CssClass="lbl-9perc-20-11-8"></asp:Label>
                                            <asp:DropDownList ID="ddlCompanySrch" runat="server" CssClass="select-small-ax" TabIndex="3">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblCustomerLIST" runat="server" Text="<%$resources:Customer %>" AssociatedControlID="txtCustomerLIST"></asp:Label>
                                            <asp:TextBox ID="txtCustomerLIST" runat="server" CssClass="select-half margnbotm0"
                                                MaxLength="100" TabIndex="51"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerLIST" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblInvoiceNumber" runat="server" Text="<%$resources:InvoiceNumber %>"
                                                CssClass="middle-lbl-small" AssociatedControlID="txtInvoiceNumber"></asp:Label>
                                            <asp:TextBox ID="txtInvoiceNumber" runat="server" CssClass="select-small-c1 margnbotm0"
                                                MaxLength="100" TabIndex="52"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfIVHPK" runat="server" Value="" />
                                            <asp:Label ID="lblInType" runat="server" Text="<%$resources:InvoiceType %>" CssClass="middle-lbl-xsmall-b margnbotm0"
                                                AssociatedControlID="ddlSaleOrderType"></asp:Label>
                                            <asp:DropDownList ID="ddlSaleOrderType" runat="server" TabIndex="52" CssClass="select-small-a-20-11 margnbotm0">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblSearch" runat="server" CssClass="middle-lbl-xsmall-d style-none margnbotm0"
                                                Width="5px" AssociatedControlID="btnSearch"></asp:Label>
                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="53" CommandName="SEARCH" SkinID="search-ext"
                                                Style="margin-bottom: 0px!important; margin-top: 2px;" />
                                            <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$resources:Controls,Clear %>"
                                                TabIndex="53" OnClick="ActionHandler" CommandName="CLEAR" Style="margin-bottom: 0px!important;
                                                margin-top: 2px;" SkinID="clear-ext" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdMiscList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                    AllowPaging="true" OnPageIndexChanging="ActionHandler" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" TabIndex="54" runat="server" GroupName="SelectOne"
                                                    AutoPostBack="true" OnCheckedChanged="ActionHandler" ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfInvoiceID" Value='<%# Eval(Resources.DataFieldRes.SalesInvoicePK) %>' />
                                                <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval("ICH_DEPT") %>' />
                                                <asp:HiddenField ID="hdfDelStatus" runat="server" Value='<%# Eval("ICH_DEL_STATUS") %>' />
                                                <asp:HiddenField ID="hdfStatus" runat="server" Value='<%# Eval("ICH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvDate %>" SortExpression="<%$ resources:DataFieldRes,SalesInvoiceDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval( Resources.DataFieldRes.SalesInvoiceDate)!=""? Convert.ToDateTime( Eval( Resources.DataFieldRes.SalesInvoiceDate)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SalesInvoiceDate, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvNo %>" SortExpression="<%$ resources:DataFieldRes,SalesInvoiceNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLInvoiceNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.SalesInvoiceNo) ==""?"[NEW]":Eval(Resources.DataFieldRes.SalesInvoiceNo)%>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SalesInvoiceNo)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCmpName" CssClass="<%# Eval(Resources.DataFieldRes.CompnayLineColor) %>"
                                                    runat="server" Text='<%# Eval(Resources.DataFieldRes.CMP_DISPLAY_CODE) %>' ToolTip='<%# Eval(Resources.DataFieldRes.CMP_DISPLAY_CODE) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceType %>" SortExpression="<%$ resources:DataFieldRes,SIType %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLInvoiceType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SIType),3,"") %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SIType)%>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfInvTypeText" Value="<%# Eval(Resources.DataFieldRes.SIType) %>" />
                                                <asp:HiddenField runat="server" ID="hdfInvType" Value='<%# Eval("ICH_TYPE") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Customer %>" SortExpression="<%$ Resources:DataFieldRes,SICustomer%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SICustomerCode),20) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SICustomer),250) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfCustomerPK" Value='<%# Eval("ICH_CUS_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SONo %>" SortExpression="<%$ resources:DataFieldRes,SISONo %>"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSoNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.SISONo) ==""?"[NEW]":Eval(Resources.DataFieldRes.SISONo)%>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SISONo)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SODate %>" SortExpression="<%$ resources:DataFieldRes,SISOdate %>"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSoDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SISOdate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SISOdate, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='<%#Eval(Resources.DataFieldRes.SICurrency)  %>'
                                                    ToolTip='<%#Eval(Resources.DataFieldRes.SICurrency)  %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfSOCurrency" Value='<%# Eval("ICH_CURRENCY") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %>" SortExpression="<%$ resources:DataFieldRes,SIValue %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceValue" runat="server" Text='<%# Eval(Resources.DataFieldRes.SIValue, "{0:c}") %>'
                                                    ToolTip='<%#Eval(Resources.DataFieldRes.SIValue, "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BalAmt %>" SortExpression="<%$ resources:DataFieldRes,SalesBalAmt %>">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblBalAmt" runat="server" Text='<%# Eval(Resources.DataFieldRes.SalesBalAmt, "{0:c}")%>'
                                                    ToolTip='<%#Eval(Resources.DataFieldRes.SalesBalAmt, "{0:c}")%>'></asp:Label>--%>
                                                    <asp:LinkButton ID="lbnBalAmt" runat="server" Text='<%# Eval(Resources.DataFieldRes.SalesBalAmt,"{0:c}")%>'
                                                    CssClass="text-underline" ToolTip='<%#Eval(Resources.DataFieldRes.SalesBalAmt,"{0:c}")%>'
                                                    OnClick="ActionHandler" CommandName="AMOUNTDETAILS"></asp:LinkButton>

                                            </ItemTemplate>
                                            <ItemStyle Width="12%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DueDate %>" SortExpression="<%$ resources:DataFieldRes,SalesDueDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDueDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SalesDueDate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SalesDueDate, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("ASC_CSS_CLASS") %>' ToolTip='<%# Eval("ICH_STATUS_TEXT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval(Resources.DataFieldRes.SApproved) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <%-- <asp:Button ID="imgPosted" runat="server" OnClientClick="javascript:return false;" />--%>
                                                <asp:Button ID="imgPosted" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? GetLocalResourceObject("unposted").ToString() : Eval("FTH_CSS_CLASS")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? Resources.Captions.NotPosted : Eval("FTH_STATUS_TEXT")%>' />
                                                <asp:HiddenField runat="server" ID="hdfPosted" Value='<%# Eval(Resources.DataFieldRes.SPosted) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <%-- <uc1:PagerControl ID="uclPaging" runat="server" />--%>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <table class="table-devide tablelayout" id="tblDetailHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
                                            <asp:HiddenField ID="hdfCurrencyDigits" runat="server" Value="3" />
                                            <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                                            <asp:HiddenField ID="hdfDecimalFormatWithSeperation" runat="server" />
                                            <asp:HiddenField ID="hdfCurrencyFormatWithSeperator" runat="server" />
                                            <asp:HiddenField runat="server" ID="hdfOtherCharge" Value="0" />
                                            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                                            <asp:HiddenField ID="hdfRateFormat" runat="server" />
                                            <asp:HiddenField ID="hdfMiscRateFormat" runat="server" />
                                            <asp:HiddenField ID="hdfInvoicePK" runat="server" />
                                            <asp:HiddenField ID="hdfExchangeRate" runat="server" />
                                            <asp:HiddenField ID="hdfTaxCategory" runat="server" />
                                            <asp:HiddenField ID="hdfTaxSettings" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdfEnableAddlOtherCharge" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdfConfirmOtherCharges" Value="0" runat="server" />
                                            <asp:Label ID="lblICustomer" runat="server" Text="<%$resources:Customer %>" AssociatedControlID="txtCustomer"></asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="input-half" MaxLength="100"
                                                TabIndex="2" ValidationGroup="invoice"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerID" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfCustomer" CssClass="star" SetFocusOnError="true"
                                                InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>" ValidationGroup="invoice"
                                                EnableClientScript="true" runat="server" ControlToValidate="txtCustomer" Display="Dynamic"
                                                Text="*" ErrorMessage="<%$ resources:Err_Vendor %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Button ID="btnCustomer" runat="server" OnClick="ActionHandler" CommandName="CUSTOMERSELECTED"
                                                Style="display: none" EnableTheming="false" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblShipTo" Text="<%$ resources:ShipTo%>" AssociatedControlID="txtShipTo"></asp:Label>
                                            <asp:TextBox ID="txtShipTo" runat="server" TextMode="MultiLine" CssClass="input-half"
                                                TabIndex="4"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <%--  Adding Customer Type & Type ID--%>
                                            <asp:Label ID="lblCustomerType" runat="server" AssociatedControlID="ddlCustomerType"
                                                Text="<%$ resources:CustomerType %>">
                                            </asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlCustomerType" AutoPostBack="true" CssClass="select-small-g-20-11 margnrgt1-5per"
                                                TabIndex="10" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfCustomerType" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="vatbuy" EnableClientScript="true" InitialValue="-1" runat="server"
                                                ControlToValidate="ddlCustomerType" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Type %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:TextBox runat="server" ID="txtTypeID" Text="" CssClass="input-small" TabIndex="10"
                                                MaxLength="5"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfBranchCode" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="vatbuy" EnableClientScript="true" runat="server" ControlToValidate="txtTypeID"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_BranchCode %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <%--  End --%>
                                            <asp:HiddenField ID="hdfPaymentTerms" runat="server" Value="" />
                                            <asp:Label ID="lblPaymentTerms" runat="server" AssociatedControlID="ddlPaymentTerms"
                                                Text="<%$ resources:PaymentTerms %>">
                                            </asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlPaymentTerms" AutoPostBack="true" TabIndex="12"
                                                CssClass="select-half-a-20-11" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblTaxID" Text="<%$ resources:TaxId%>" AssociatedControlID="txtTaxID"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTaxID" Text="" TabIndex="20" MaxLength="100" CssClass="input-half"></asp:TextBox>
                                            <asp:Label ID="lblCompanyView" Visible="false" runat="server" Text="<%$ resources:Controls, CompanyPlant %>"
                                                AssociatedControlID="ddlCompanyView"></asp:Label>
                                            <asp:DropDownList ID="ddlCompanyView" Visible="false" Enabled="false" runat="server"
                                                CssClass="select-small-e">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblInvNo" runat="server" Text="<%$ resources:InvoiceNo%>" AssociatedControlID="lblInvoiceNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblInvoiceNo" CssClass="input-small margnrgt0-8per"></asp:Label>
                                            <asp:HiddenField ID="hdfInvoiceNo" runat="server" Value="" />
                                            <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                                            <asp:HiddenField ID="AST_CODE" runat="server" />
                                            <asp:HiddenField ID="hdfCurrCustomerPK" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdfCustomerTypeId" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdfIsTaxPayable" runat="server" Value="0" />
                                            <asp:Label runat="server" ID="lblInvoiceDate" Text="<%$ resources:InvoiceDate%>"
                                                CssClass="middle-lbl-small-c" AssociatedControlID="txtInvoiceDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInvoiceDate" CssClass="input-small" TabIndex="3"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(null)"
                                                onclick="SetOldDate()"></asp:TextBox>
                                            <asp:HiddenField ID="hdfInvoiceDate" runat="server" />
                                            <asp:HiddenField ID="hdfCreditDays" Value="0" runat="server" />
                                            <asp:HiddenField ID="hdfHasTax" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdfOldInvDate" runat="server" />
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
                                            <asp:Label runat="server" ID="lblCurrency" Text="<%$ resources:Currency%>" AssociatedControlID="txtCurrency"></asp:Label>
                                            <asp:TextBox ID="txtCurrency" runat="server" CssClass="input-small" TabIndex="5"
                                                MaxLength="100" Enabled="true"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCurrency" runat="server" />
                                            <asp:HiddenField ID="hdfVendorInvType" runat="server" />
                                            <asp:Button ID="btnCurrency" runat="server" OnClick="ActionHandler" CommandName="CURRENCYSELECTED"
                                                Style="display: none" EnableTheming="false" />
                                            <asp:HiddenField ID="hdfType" runat="server" Value="" />
                                            <asp:Label ID="lblInvoiceType" runat="server" AssociatedControlID="ddlInvoiceType"
                                                Text="<%$ resources:InvoiceType %>" CssClass="middle-lbl-small-d">
                                            </asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlInvoiceType" TabIndex="6" AutoPostBack="true"
                                                CssClass="select-xsmall-a3" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblExchangeRate" Text="<%$ resources:ExchangeRate%>"
                                                AssociatedControlID="txtExchangeRate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtExchangeRate" Text="" TabIndex="7" AutoPostBack="true"
                                                OnTextChanged="ActionHandler" CssClass="input-small numeric medium" onkeypress="return validateRateFloatKeyPress(this,event);"></asp:TextBox>
                                            <asp:Label runat="server" ID="lblInvoiceDueDate" Text="<%$ resources:InvoiceDueDate%>"
                                                AssociatedControlID="txtInvoiceDueDate" CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInvoiceDueDate" CssClass="input-small" TabIndex="8"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(null)"></asp:TextBox>
                                            <asp:HiddenField ID="hdfInvoiceDueDate" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfInvoiceDueDate" CssClass="star" SetFocusOnError="false"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtInvoiceDueDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_InvoiceDueDate %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblETD" Text="<%$ resources:ETD%>" AssociatedControlID="txtETD"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtETD" CssClass="input-small" TabIndex="8" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(null)"></asp:TextBox>
                                            <asp:HiddenField ID="hdfETD" runat="server" />
                                            <asp:Label runat="server" ID="lblETA" Text="<%$ resources:ETA%>" CssClass="middle-lbl-small-d"
                                                AssociatedControlID="txtETA"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtETA" CssClass="input-small" TabIndex="9" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(null)"></asp:TextBox>
                                            <asp:HiddenField ID="hdfETA" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblShipmentTerm" Text="<%$ resources:ShipmentTerm%>"
                                                AssociatedControlID="txtShipmentTerm"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtShipmentTerm" Text="" CssClass="input-half-20-11" TabIndex="11"
                                                MaxLength="100"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblReference" Text="<%$ resources:Reference%>" AssociatedControlID="txtReference"></asp:Label>
                                            <asp:TextBox ID="txtReference" runat="server" MaxLength="100" TabIndex="16" CssClass="input-half-20-11"></asp:TextBox>
                                            <asp:Label runat="server" ID="lblInvoiceGstType" Text="<%$ resources:InvoiceGstType%>"
                                                AssociatedControlID="ddlInvoiceGstType"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlInvoiceGstType" TabIndex="16" CssClass="select-medium">
                                            </asp:DropDownList>

                                              <asp:Label runat="server" ID="lblSubType" Text="<%$ resources:SubType%>"
                                                AssociatedControlID="ddlSubType" CssClass="middle-lbl-xsmall-c1"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlSubType" TabIndex="16" CssClass="select-small-a2">
                                            </asp:DropDownList>

                                            <div class="clear">
                                            </div>

                                              <asp:Label runat="server" ID="lblSaleCategory" Text="<%$ resources:SaleCategory%>"
                                                AssociatedControlID="ddlSaleCategory" ></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlSaleCategory" TabIndex="16" CssClass="select-small-a2">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrtSaleCategory" CssClass="star" SetFocusOnError="true" ValidationGroup="invoice"
                                                EnableClientScript="true" InitialValue="-1" runat="server" ControlToValidate="ddlSaleCategory"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_SaleCategory %>">
                                            </asp:RequiredFieldValidator>
                                               <div class="clear">
                                            </div>

                                            <div id="divDummy" runat="server" visible="false">
                                                <asp:Label runat="server" ID="lblIsDummy" Text="<%$ resources:Dummy%>" AssociatedControlID="chkDummy"
                                                    CssClass="margn-rgt0"></asp:Label>
                                                <asp:CheckBox ID="chkDummy" runat="server" Checked="true" TabIndex="20" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("OtherDetailsHead").ToString() + " :"%></h1>
                                <asp:ImageButton runat="server" ID="imbShowOtherDetails" OnClientClick="javascript:return ShowHideOtherDetails(1);"
                                    SkinID="imbArrowInactive" ToolTip="<%$ resources:ShowOtherDetails %>" TabIndex="107" />
                                <asp:ImageButton runat="server" ID="imbHideOtherDetails" OnClientClick="javascript:return ShowHideOtherDetails();"
                                    Style="display: none" SkinID="imbArrowActive" TabIndex="170" ToolTip="<%$ resources:HideOtherDetails %>" />
                                <asp:HiddenField ID="hdfIsOtherDetailsVisible" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divOtherDetails" style="display: none">
                                <table class="table-devide" id="tblOtherDetails">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="Label2" Text="<%$ resources:PLoading%>" AssociatedControlID="txtFromPort"></asp:Label>
                                                <%--<asp:DropDownList ID="ddlFromPortID" runat="server" OnSelectedIndexChanged="ActionHandler"
                                                    CssClass="select-half-a" TabIndex="20">
                                                </asp:DropDownList>--%>
                                                <asp:TextBox runat="server" ID="txtFromPort" CssClass="select-half-20-11-7" TabIndex="20"></asp:TextBox>
                                                <asp:HiddenField runat="server" ID="hdfFromPortID" />
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblFeederVessel" Text="<%$ resources:FeederVessel%>"
                                                    AssociatedControlID="txtFeederVessel"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtFeederVessel" Text="" TabIndex="22" MaxLength="100"
                                                    CssClass="input-half"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblContainerNo" Text="<%$ resources:ContainerNo%>"
                                                    AssociatedControlID="txtContainerNo"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtContainerNo" Text="" TabIndex="24" MaxLength="100"
                                                    CssClass="input-half"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblFDest" Text="<%$ resources:FDest%>" AssociatedControlID="txtFDest"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtFDest" Text="" TabIndex="21" MaxLength="100" CssClass="input-half"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblMotherVessel" Text="<%$ resources:MotherVessel%>"
                                                    AssociatedControlID="txtMotherVessel"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtMotherVessel" Text="" CssClass="input-half" TabIndex="23"
                                                    MaxLength="100"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("ExpenseDetails").ToString() + " :"%></h1>
                                <asp:ImageButton runat="server" ID="imbShowItemDetails" OnClientClick="javascript:return ShowHideItemDetails(1);"
                                    SkinID="imbArrowInactive" ToolTip="<%$ resources:ShowItemDetails %>" TabIndex="107" />
                                <asp:ImageButton runat="server" ID="imbHideItemDetails" OnClientClick="javascript:return ShowHideItemDetails();"
                                    Style="display: none" SkinID="imbArrowActive" TabIndex="170" ToolTip="<%$ resources:HideItemDetails %>" />
                                <asp:HiddenField ID="hdfIsItemDetailsVisible" runat="server" Value="1" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divItemDetails" style="display: none">
                                <table class="table-devide" id="tblDetails">
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label ID="lblItem" runat="server" AssociatedControlID="txtItem" Text="<%$ resources:Item %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtItem" runat="server" TabIndex="25" CssClass="input-full" MaxLength="400"
                                                    ValidationGroup="invoice"></asp:TextBox>
                                                <asp:HiddenField ID="hdfItemID" runat="server" />
                                                <asp:Button ID="btnItem" runat="server" Style="display: none" EnableTheming="false" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:HiddenField ID="hdfDetailPK" runat="server" Value="0" />
                                                <asp:Label ID="lblDesc" runat="server" AssociatedControlID="txtDesc" Text="<%$ resources:Desc_Mand %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtDesc" runat="server" TabIndex="26" MaxLength="400" CssClass="input-full"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfDesc" CssClass="star" SetFocusOnError="true" ValidationGroup="details"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtDesc" Display="Dynamic"
                                                    Text="*" ErrorMessage="<%$ resources:Err_Desc %>"></asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblQty" runat="server" AssociatedControlID="txtQty" Text="<%$ resources:Quantity_Mand %>"><%--<%$ resources:Quantity %>--%>
                                                </asp:Label>
                                                <asp:TextBox ID="txtQty" runat="server" CssClass="input-small numeric" MaxLength="11"
                                                    TabIndex="26" onchange="CalculateAmount();ResetAllocation();" Text="1"></asp:TextBox>
                                                <%--  Allocation Popup--%>
                                                <asp:ImageButton ID="imgISSUE" runat="server" CommandName="SHOWISSUEPOPUP" OnClick="ActionHandler"
                                                    TabIndex="26" ToolTip="<%$ resources:Controls,IssueAllocDetails %>" SkinID="grn-icon" />
                                                <span style="width: 10px; border: 0 none; background: none;" class="nomargin">
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfQuantity" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="details" EnableClientScript="true" runat="server" ControlToValidate="txtQty"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Quantity %>">
                                                        </asp:RequiredFieldValidator>
                                                        <cc1:QuantityValidation ID="vreQuantity" runat="server" ControlToValidate="txtQty"
                                                            NumberDigits="7" ErrorMessage="<%$ resources:Err_Quantity_Valid %>" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="details"
                                                            NonZero="true"></cc1:QuantityValidation>
                                                        <asp:RequiredFieldValidator ID="vrfQuantityTaxDate" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="taxDate" EnableClientScript="true" runat="server" ControlToValidate="txtQty"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Quantity %>">
                                                        </asp:RequiredFieldValidator>
                                                        <cc1:QuantityValidation ID="vreQuantityTaxDate" runat="server" ControlToValidate="txtQty"
                                                            NumberDigits="7" ErrorMessage="<%$ resources:Err_Quantity_Valid %>" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="taxDate"
                                                            NonZero="true"></cc1:QuantityValidation>
                                                    </div>
                                                </span>
                                                <asp:TextBox runat="server" ID="txtUOM" CssClass="Uiinput-uom input-small" TabIndex="27"></asp:TextBox>
                                                <asp:HiddenField ID="hdfUOM" runat="server" />
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="vrfUOM" CssClass="star" SetFocusOnError="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                        ValidationGroup="details" EnableClientScript="true" runat="server" ControlToValidate="txtUOM"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_UOM %>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="vrfUOMTaxDate" CssClass="star" SetFocusOnError="true"
                                                        InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>" ValidationGroup="taxDate"
                                                        EnableClientScript="true" runat="server" ControlToValidate="txtUOM" Display="Dynamic"
                                                        Text="*" ErrorMessage="<%$ resources:Err_UOM %>">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblItemDiscount" runat="server" AssociatedControlID="txtDiscount"
                                                    Text="<%$ resources:Discount %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtDiscount" runat="server" CssClass="input-small input-disabled numeric"
                                                    MaxLength="14" onkeydown="return EnableArrowKey(event)" onpaste="return false;"></asp:TextBox>
                                                <asp:ImageButton ID="imgDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                    TabIndex="29" ToolTip="<%$ resources:Controls,Discounts %>" CommandName="DISCDETAILS"
                                                    ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S marginLft18-03-21">
                                                <asp:Label ID="lblRate" runat="server" AssociatedControlID="txtRate" Text="<%$ resources:Rate_Mand %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtRate" runat="server" CssClass="input-small numeric" MaxLength="28"
                                                    TabIndex="28" onchange="CalculateAmount();"></asp:TextBox>
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="vrfRate" CssClass="star" SetFocusOnError="true" ValidationGroup="details"
                                                        EnableClientScript="true" runat="server" ControlToValidate="txtRate" Display="Dynamic"
                                                        Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                    </asp:RequiredFieldValidator>
                                                    <cc1:RateValidation ID="vreRate" runat="server" ControlToValidate="txtRate" ErrorMessage="<%$ resources:Err_Rate_Valid %>"
                                                        NumberDigits="10" Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star"
                                                        ValidationGroup="details" NonZero="false"></cc1:RateValidation>
                                                    <asp:RequiredFieldValidator ID="vrfRateTaxDate" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="taxDate" EnableClientScript="true" runat="server" ControlToValidate="txtRate"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                    </asp:RequiredFieldValidator>
                                                    <cc1:RateValidation ID="vreRateTaxDate" runat="server" ControlToValidate="txtRate"
                                                        ErrorMessage="<%$ resources:Err_Rate_Valid %>" NumberDigits="10" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="taxDate"
                                                        NonZero="false"></cc1:RateValidation>
                                                </div>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" Text="<%$ resources:Amount_Mand %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtAmount" runat="server" Text="0.00" CssClass="input-small input-disabled numeric"
                                                    MaxLength="15" onkeydown="return EnableArrowKey(event);" onpaste="return false;"
                                                    Visible="true"></asp:TextBox>
                                                <asp:Button ID="btnRecalculateTax" runat="server" EnableTheming="false" Style="display: none"
                                                    OnClick="ActionHandler" CommandName="CALCULATEDTLTAX" />
                                                <div class="starwrap">
                                                    <%-- <asp:RequiredFieldValidator ID="vrfAmount" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="details" EnableClientScript="true" runat="server" ControlToValidate="txtAmount"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                    </asp:RequiredFieldValidator> no validation need ,Auto generating field--%>
                                                    <cc1:AmountValidation ID="vamAmount" runat="server" ControlToValidate="txtAmount"
                                                        ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="details"></cc1:AmountValidation>
                                                    <%--  <asp:RequiredFieldValidator ID="vrfAmountTaxDate" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="taxDate" EnableClientScript="true" runat="server" ControlToValidate="txtAmount"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                    </asp:RequiredFieldValidator>--%>
                                                    <cc1:AmountValidation ID="vreAmountTaxDate" runat="server" ControlToValidate="txtAmount"
                                                        ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="taxDate"></cc1:AmountValidation>
                                                </div>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblItemTax" runat="server" AssociatedControlID="txtTax" Text="<%$ resources:Tax %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtTax" runat="server" CssClass="input-small input-disabled numeric"
                                                    MaxLength="14" onkeydown="return EnableArrowKey(event)" onpaste="return false;"></asp:TextBox>
                                                <asp:ImageButton ID="imgTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                    TabIndex="30" ToolTip="<%$ resources:Tax %>" CommandName="TAXDETAILS" ValidationGroup="taxDate"
                                                    OnClientClick="javascript:ValidatePageNow('taxDate')" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:TextBox ID="txtTotal" runat="server" EnableTheming="false" Text="0" Style="display: none" />
                                                <asp:Label runat="server" ID="lblDtlRemark" Text="<%$ resources:Remarks %>" AssociatedControlID="txtDtlRemark"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDtlRemark" TabIndex="28" CssClass="input-full"
                                                    MaxLength="480"></asp:TextBox>
                                                <asp:ImageButton runat="server" ID="btnAddItem" CommandName="ADDITEM" TabIndex="29"
                                                    OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('details')"
                                                    ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="PageAction_Entry" ValidationGroup="details"
                                                    SkinID="plus" />
                                                <asp:ImageButton runat="server" ID="btnClearItem" CommandName="CLEARITEM" TabIndex="30"
                                                    OnClick="ActionHandler" ToolTip="<%$resources:Controls,Clear %>" CommandArgument="PageAction_Entry"
                                                    SkinID="cancel" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="gridwrap">
                                <asp:GridView ID="grdItemDetails" runat="server" AutoGenerateColumns="False" PageSize="<%$ resources:PageSize %>"
                                    AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                    Width="100%" ShowFooter="true" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Item %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfGrdItemID" runat="server" Value='<%#Eval("CID_ITEM") %>' />
                                                <asp:Label ID="lblGrdItem" runat="server" Text='<%# Eval("CID_ITEM_TEXT") %>' ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("CID_ITEM_TEXT"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <%--                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblfooterTot" Text="<%$ resources:Total %>"></asp:Label>
                                            </FooterTemplate>
                                            --%>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Description %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfSODPK" runat="server" Value='<%#Eval("CID_PK") %>' />
                                                <asp:Label ID="lblExpenseDescription" runat="server" Text='<%# Eval("CID_INSTRUCTIONS") %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("CID_INSTRUCTIONS"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblfooterTot" Text="<%$ resources:Total %>"></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle Width="18%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Qty%>" HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuantity" runat="server" CssClass="ItemQuantity" Text='<%# Eval("CID_QTY_INVOICED", "{0:n}") %>'
                                                    ToolTip='<%# Eval("CID_QTY_INVOICED", "{0:n}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblItemTotalQty" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:UOM%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUOM" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("CID_UOM_TEXT"),10) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("CID_UOM_TEXT"))) %>'></asp:Label>
                                                <asp:HiddenField ID="hdfUoM" runat="server" Value='<%#Eval("CID_UOM") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Rate %>" HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemRate" runat="server" Text='<%# GetFormattedMiscRate(Eval("CID_RATE")) %>'
                                                    ToolTip='<%#GetFormattedMiscRate(Eval("CID_RATE")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" CssClass="amount-numeric" />
                                            <FooterTemplate>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %>" HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemAmount" runat="server" Text='<%# Eval("CID_AMOUNT", "{0:c}") %>'
                                                    ToolTip='<%# Eval("CID_AMOUNT", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblItemTotalAmount" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Discount %>" HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrdItemDiscount" runat="server" Text='<%# Eval("CID_DISCOUNT", "{0:c}") %>'
                                                    ToolTip='<%# Eval("CID_DISCOUNT", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblDiscountTotal"></asp:Label></FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Tax %>" HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrdItemTax" runat="server" Text='<%# Eval("CID_TAX", "{0:c}") %>'
                                                    ToolTip='<%# Eval("CID_TAX", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTaxTotal"></asp:Label></FooterTemplate>
                                        </asp:TemplateField>
                                        <%--Pay Now--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Total %>" HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemTotal" runat="server" Text='<%# Eval("CID_NET_AMOUNT", "{0:c}") %>'
                                                    ToolTip='<%# Eval("CID_NET_AMOUNT", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblSubTotalFooter" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:remarks %>">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CID_REMARKS"), 10) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("CID_REMARKS"))) %>'
                                                    ID="lblItemRemarks"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnEditItem" runat="server" OnClick="ActionHandler" CommandName="EDITITEM"
                                                    CommandArgument="PageAction_Entry" SkinID="imbeditgrid" ToolTip="Edit" TabIndex="28"
                                                    OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load" />
                                                <asp:ImageButton ID="btnRemoveItem" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                                                    CommandArgument="PageAction_Entry" OnClientClick="return ShowDeleteConfirm(this);"
                                                    SkinID="imbdeletegrid" ToolTip="Delete" TabIndex="29" OnPreRender="btnAction_PreRender"
                                                    OnLoad="btnAction_Load" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div id="divCalc">
                                <div class="gridwrap">
                                    <table id="tblCalc" class="table-devide" style="margin-top: 8px;">
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblNetWt" Text="<%$ resources:NetWt%>" AssociatedControlID="txtNetWt"></asp:Label>
                                                    <asp:TextBox ID="txtNetWt" runat="server" CssClass="input-small numeric medium" MaxLength="27"
                                                        onchange="SetEnableDisableReqfld()" TabIndex="30"></asp:TextBox>
                                                    <asp:TextBox runat="server" ID="txtUOMwt" CssClass="Uiinput-uom medium input-small"
                                                        TabIndex="31"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfUOMwt" runat="server" />
                                                    <asp:RequiredFieldValidator ID="vrfUOMwt" CssClass="star" SetFocusOnError="true"
                                                        Enabled="false" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>" ValidationGroup="invoice"
                                                        EnableClientScript="true" runat="server" ControlToValidate="txtUOMwt" Display="Dynamic"
                                                        Text="*" ErrorMessage="<%$ resources:Err_Type %>">
                                                    </asp:RequiredFieldValidator>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblGrossWt" Text="<%$ resources:GrossWt%>" AssociatedControlID="txtGrossWt"></asp:Label>
                                                    <asp:TextBox ID="txtGrossWt" runat="server" CssClass="input-small numeric medium"
                                                        MaxLength="16" TabIndex="33"></asp:TextBox>
                                                    <asp:Label runat="server" ID="lblUOMGrosswt" CssClass="input-small"></asp:Label>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblterm1" Text="<%$ resources:Ship_mark%>" AssociatedControlID="txtTerm1"></asp:Label>
                                                    <asp:TextBox ID="txtTerm1" runat="server" TextMode="MultiLine" CssClass="input-half"
                                                        TabIndex="35"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-P">
                                                    <table id="Table2">
                                                        <tr>
                                                            <td style="text-align: right; width: 87%;">
                                                                <asp:Label runat="server" ID="lblDiscount" Text="<%$ resources:Discount%>" AssociatedControlID="txtHdrDiscount"></asp:Label>
                                                            </td>
                                                            <td style="text-align: right; width: 87%;">
                                                                <asp:ImageButton ID="imgHdrDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                                    ValidationGroup="taxHdrDate" OnClientClick="javascript:ValidatePageNow('taxHdrDate')"
                                                                    TabIndex="32" ToolTip="<%$ resources:Controls,Discounts %>" CommandName="DISCHEADER" />
                                                            </td>
                                                            <td style="text-align: right; width: 87%;">
                                                                <asp:TextBox ID="txtHdrDiscount" runat="server" CssClass="input-w80 numeric input-disabled medium"
                                                                    MaxLength="32" Enabled="false"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div class="clear">
                                                    </div>
                                                    <div id="divOC" runat="server">
                                                    <table id="Table8">
                                                        <tr>
                                                            <td style="text-align: right; width: 87%;">
                                                                <asp:Label runat="server" ID="lblShipping" Text="<%$ resources:OtherCharges%>" AssociatedControlID="txtShipping"></asp:Label>
                                                            </td>
                                                            <td style="text-align: right" class="btn-margin">
                                                                 <asp:ImageButton ID="imgShippingCharge" SkinID="shipping" runat="server" OnClick="ActionHandler"
                                                                     TabIndex="33" ToolTip="<%$ resources:OtherCharges %>" CommandName="SHIPPINGHEADER" />
                                                            </td>
                                                             <td style="text-align: right; width: 87%;">
                                                                 <asp:TextBox ID="txtShipping" runat="server" CssClass="input-w80 numeric input-disabled medium"
                                                                     MaxLength="16" Enabled="false" TabIndex="25"></asp:TextBox>
                                                          
                                                            <div class="starwrap">
                                                                <cc1:AmountValidation ID="vamShipping" runat="server" ControlToValidate="txtShipping"
                                                                    ErrorMessage="<%$ resources:Err_Valid_Shipping %>" NumberDigits="12" Display="Dynamic"
                                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="so"></cc1:AmountValidation>
                                                   
                                                            </div>                                               
                                                           </td>                                                         
                                                        </tr>
                                                        </table>

                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                    

                                                    <table id="Table3">
                                                        <tr>
                                                            <td style="text-align: right; width: 87%;">
                                                                <asp:Label runat="server" ID="lblTax" Text="<%$ resources:Tax%>" AssociatedControlID="txtHdrTax"></asp:Label>
                                                            </td>
                                                            <td style="text-align: right; width: 87%;">
                                                                <asp:ImageButton ID="imgHdrTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                                    ValidationGroup="taxHdrDate" OnClientClick="javascript:ValidatePageNow('taxHdrDate')"
                                                                    TabIndex="34" ToolTip="<%$ resources:Tax %>" CommandName="TAXHEADER" />
                                                            </td>
                                                            <td style="text-align: right; width: 87%;">
                                                                <asp:TextBox ID="txtHdrTax" runat="server" CssClass="input-w80 numeric input-disabled medium"
                                                                    Enabled="false" MaxLength="16"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                     
                                                    <div class="clear">
                                                    </div>
                                                    <table id="Table4">
                                                        <tr>
                                                            <td style="text-align: right; width: 87%;">
                                                                <asp:Label runat="server" ID="lblPriceAdj" Text="<%$ resources:PriceAdj%>" AssociatedControlID="txtPriceAdj"></asp:Label>
                                                            </td>
                                                            <td style="text-align: right; width: 87%;">
                                                            </td>
                                                            <td style="text-align: right; width: 87%;">
                                                                <asp:TextBox ID="txtPriceAdj" runat="server" CssClass="input-w80 numeric medium"
                                                                    onchange="CalculateTotal(this);" MaxLength="16" TabIndex="36"></asp:TextBox>
                                                                <div class="starwrap">
                                                                    <cc1:AmountValidation ID="vamPriceAdj" runat="server" ControlToValidate="txtPriceAdj"
                                                                        ErrorMessage="<%$ resources:Err_Valid_PriceAdj %>" NumberDigits="12" AllowNegative="true"
                                                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"></cc1:AmountValidation>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div class="clear">
                                                    </div>
                                                    <table id="Table5">
                                                        <tr>
                                                            <td style="text-align: right; width: 87%;">
                                                                <%-- <asp:Label runat="server" ID="lblterm2" Text="" AssociatedControlID="txtHdrDiscount"></asp:Label>--%>
                                                                <asp:TextBox ID="txtTerm2" runat="server" CssClass="input-medium medium " TabIndex="37"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lblTotal" Text="<%$ resources:Total%>" AssociatedControlID="txtHdrTotal"
                                                                    Width="30px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: right; width: 87%;">
                                                            </td>
                                                            <td style="text-align: right; width: 87%;">
                                                                <asp:TextBox ID="txtHdrTotal" runat="server" CssClass="input-w80 numeric input-disabled medium"
                                                                    Enabled="false" MaxLength="16"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div>
                                        <%--class="divcol-S"--%>
                                        <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks%>" AssociatedControlID="txtRemarks"
                                            Width="12.7%"></asp:Label>
                                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="input-full" TabIndex="50" MaxLength="500"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div id="divOuterTaxPayable">
                                <div class="search-colapse-b">
                                    <h1>
                                        <%= GetLocalResourceObject("TaxDtlBC").ToString() + " :"%>
                                    </h1>
                                    <asp:ImageButton runat="server" ID="imbShowBaseTaxDetails" OnClientClick="javascript:return ShowHideTaxAmtinBaseCurrency(1);"
                                        SkinID="imbArrowInactive" ToolTip="<%$ resources:ShowItemDetails %>" Style="display: none;
                                        cursor: pointer" />
                                    <asp:ImageButton runat="server" ID="imbHideBaseTaxDetails" OnClientClick="javascript:return ShowHideTaxAmtinBaseCurrency();"
                                        SkinID="imbArrowActive" ToolTip="<%$ resources:HideItemDetails %>" />
                                    <asp:HiddenField ID="hdfShowTaxAmtBC" runat="server" Value="0" />
                                    <div class="clear">
                                    </div>
                                </div>
                                <div id="divTaxAmtBaseCurr" class="gridwrap">
                                    <div class="content-wrapper">
                                        <div class="gridwrap">
                                            <asp:GridView runat="server" ID="grdTaxBaseCUr" Width="100%" AllowSorting="false"
                                                OnRowDataBound="ActionHandler" AutoGenerateColumns="false" TabIndex="22" EmptyDataRowStyle-CssClass="emptytable">
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ resources:divTaxCode %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTaxHdrCode" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_CODE")) == string.Empty ? string.Empty : Convert.ToString(Eval("CIT_TAX_CODE")) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="20%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:divTaxName %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTaxHdrName" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("CIT_TAX_TEXT")) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="25%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:divTaxRate %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTaxRate" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_RATE")) == "0" ? string.Empty : Convert.ToString(Eval("CIT_TAX_RATE")+"%") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:divsubTotal %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSubTotal" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                        <HeaderStyle CssClass="amount-numeric" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:divTaxAmount %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTaxAmountBC" runat="server" Text='<%# GetFormattedCurrency(Convert.ToDouble(Eval("CIT_TAX_AMT")) * Convert.ToDouble(hdfExchangeRate.Value))%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                        <HeaderStyle CssClass="amount-numeric" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divItemTax" style="display: none">
                                <div class="Button-container-popup">
                                    <asp:Button ID="btnApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply" OnClick="ActionHandler"
                                        CommandArgument="PageAction_Entry" CommandName="TAXAPPLY" TabIndex="29" />
                                </div>
                                <div class="content-wrapper">
                                     <%-- *********************Checkbox Region Start*******************************************************************--%>
                            <div id="divTaxApplicableAmount" style="display: none" runat="server">
                                <label for="chkSubTotal" style="width: 148px">
                                    <%=Resources.Controls.SubTotal%>
                                </label>
                                <asp:CheckBox ID="chkSubTotal" runat="server" Checked="false" OnCheckedChanged="ActionHandler"
                                    AutoPostBack="true" />
                                <label for="chkDiscount" style="width: 120px">
                                    <%=Resources.Controls.Discounts%>
                                </label>
                                <asp:CheckBox ID="chkDiscount" runat="server" Checked="false" OnCheckedChanged="ActionHandler"
                                    AutoPostBack="true" />
                                <label for="chkOtherCharges" style="width: 125px">
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
                                                    <asp:Label ID="lblPopupItemAmount" runat="server" Text="Item Amount" AssociatedControlID="txtPopupItemAmount"></asp:Label>
                                                    <asp:TextBox ID="txtPopupItemAmount" CssClass="input-w70 numeric" runat="server"
                                                        EnableViewState="false" Enabled="false" MaxLength="11"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblPopupAmount" runat="server" Text="Amount" AssociatedControlID="txtPopupAmount"></asp:Label>
                                                    <asp:TextBox ID="txtPopupAmount" TabIndex="25" runat="server" CssClass="input-w70 numeric"
                                                        EnableViewState="false" Enabled="false" MaxLength="15"></asp:TextBox>
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfTaxAmt" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="tax" EnableClientScript="true" runat="server" ControlToValidate="txtPopupAmount"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                        </asp:RequiredFieldValidator>
                                                       <%-- <cc1:AmountValidation ID="vamTaxAmt" runat="server" ControlToValidate="txtPopupAmount"
                                                            ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="tax"></cc1:AmountValidation>--%>
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:Label ID="lblPopupTaxType" runat="server" Text="Type" AssociatedControlID="ddlPopupTaxType"></asp:Label>
                                                    <asp:DropDownList ID="ddlPopupTaxType" TabIndex="24" runat="server" CssClass="medium"
                                                        EnableViewState="true" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblPopupOther" runat="server" Text="Name" AssociatedControlID="txtPopupOther"></asp:Label>
                                                    <asp:TextBox ID="txtPopupOther" runat="server" TabIndex="26" CssClass="medium" EnableViewState="false"
                                                        MaxLength="100" Enabled="false"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfPopupOther" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="tax" EnableClientScript="true" runat="server" ControlToValidate="txtPopupOther"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TaxName %>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:ImageButton ID="imgPopupAdd" SkinID="imbaddnew" runat="server" OnClick="ActionHandler"
                                                        CommandArgument="PageAction_Entry" ValidationGroup="tax" CommandName="TAXADD"
                                                        TabIndex="27" OnClientClick="javascript:ValidatePageNow('tax')" />
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdTaxDetails" Width="100%" AllowSorting="false"
                                            OnRowDataBound="ActionHandler" AutoGenerateColumns="false" TabIndex="22" EmptyDataRowStyle-CssClass="emptytable">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Type">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfTaxSplitPK" runat="server" Value='<%#Eval("CIT_PK") %>' />
                                                        <asp:HiddenField ID="hdfTaxPK" runat="server" Value='<%#Eval("CIT_TAX") %>' />
                                                        <asp:Label ID="lblTaxText" runat="server" Text='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("CIT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("CIT_TAX_TEXT"))) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("CIT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("CIT_TAX_TEXT"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxName" runat="server" Text='<%#Eval("CIT_NAME") %>' ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("CIT_NAME"))) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTaxName" runat="server" Value='<%#Eval("CIT_NAME") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("CIT_TAX_AMT")) %>'
                                                            ToolTip='<%#GetFormattedCurrency(Eval("CIT_TAX_AMT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="22%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imbTaxRemove" runat="server" OnClick="ActionHandler" CommandName="TAXDELETE"
                                                            TabIndex="28" CommandArgument="PageAction_Entry" OnPreRender="btnAction_PreRender"
                                                            OnLoad="btnAction_Load" SkinID="btnclose" ToolTip="Remove" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="8%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>

                
                            <%-- -------Start ISSSUE Splitup Popup --------------------------%>
                            <div id="divIssueQtySpilup" style="display: none">
                                <div class="Button-container-popup">
                                    <asp:Button ID="btnIssueQtyApply" runat="server" Text="<%$ resources:Controls,Apply %>"
                                        TabIndex="27" ToolTip="<%$ resources:Controls,Apply %>" OnClick="ActionHandler"
                                        CommandName="ISSUEAPPLY" SkinID="btnInner-add-dsd" CommandArgument="Allocation_Section"
                                        ValidationGroup="ISSUESplit" OnClientClick="javascript:ValidatePageNow('ISSUESplit')" />
                                    &nbsp;
                                </div>
                                <div class="content-wrapper">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:Label ID="lblIssueType" runat="server" Text="<%$resources:InvoiceType %>" CssClass="middle-lbl-xsmall-b margnbotm0"
                                                        AssociatedControlID="ddlIssueType"></asp:Label>
                                                    <asp:DropDownList ID="ddlIssueType" runat="server" TabIndex="26" CssClass="select-half margnbotm0"
                                                        OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="Issueadd" EnableClientScript="true" runat="server" ControlToValidate="ddlIssueType"
                                                            Display="Dynamic" Text="*" ErrorMessage="*">
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                    <%--<asp:ImageButton ID="imgbtnPopupAdd" SkinID="imbaddnew" runat="server" OnClick="ActionHandler"
                                                        CommandArgument="PageAction_Entry" ValidationGroup="Issueadd" ToolTip="Add" CommandName="SHOWISSUEDETAILS"
                                                        OnClientClick="javascript:ValidatePageNow('Issueadd')" />--%>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-P">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="gridwrap">
                                        <asp:TableCell>
                                            <div class="gridwrap">
                                                <asp:GridView ID="grdIssueDetails" runat="server" AutoGenerateColumns="False" Width="96%"
                                                    PageSize="<%$ resources:PageSize %>" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable"
                                                    AllowSorting="false" ShowFooter="true" OnRowDataBound="ActionHandler">
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$ resources:ISSUENO %>">
                                                            <ItemTemplate>
                                                                <%--<asp:LinkButton ID="lnkISSUENO" CssClass="text-underline" runat="server" Text='<%#Eval("CII_ISSUE_NO")%>'
                                                                    ToolTip='<%#Eval("CII_ISSUE_NO")%>' OnClick="ActionHandler" CommandName="SHOW"></asp:LinkButton>--%>
                                                                <asp:Label ID="lnkISSUENO" runat="server" Text='<%#Eval("CII_ISSUE_NO")%>' ToolTip='<%#Eval("CII_ISSUE_NO")%>'></asp:Label>
                                                                <asp:HiddenField ID="hdfCII_PK" runat="server" Value='<%#Eval("CII_PK")%>' />
                                                                <asp:HiddenField ID="hdfIssueHdrPK" runat="server" Value='<%#Eval("CII_ISSUE_PK")%>' />
                                                                <%--(HBC_PK)PRD_BIN_CARD_CONS_HDR--%>
                                                                <%-- <asp:HiddenField ID="hdfVGLPOPK" runat="server" Value='<%#Eval("VGL_INVOICE_DTL")%>' />--%>
                                                                <asp:HiddenField ID="hdfIssueDtlPK" runat="server" Value='<%#Eval("CII_ISSUE_DTL")%>' />
                                                                <asp:HiddenField ID="hdfIssuedProductUOMPK" runat="server" Value='<%#Eval("CII_UOM")%>' />
                                                                <%--IssuedProductUOM--%>
                                                                <asp:HiddenField ID="hdfIssuedProductUOMCODE" runat="server" Value='<%#Eval("CII_UOM_CODE")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="20%" />
                                                            <HeaderStyle Width="20%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:IssueDate %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblIssueDate" runat="server" Text='<%#DateTime.Parse(Eval("CII_ISSUE_DATE").ToString()).ToString(Resources.Constants.ReportDateFormat)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" />
                                                            <HeaderStyle Width="15%" />
                                                            <FooterTemplate>
                                                                <asp:Label runat="server" ID="lblTotal" Text="<%$ resources:Total %>"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:IssueQty %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblIssueQty" runat="server" CssClass="amount-numeric" Text='<%#(Eval("CII_QTY_ISSUED"))%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="amount-numeric" Width="15%" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <asp:Label runat="server" ID="lblTotalIssueQty"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:IssueInvdQty %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblIssueInvdQty" runat="server" CssClass="amount-numeric" Text='<%#(Eval("CII_QTY_PRV_INVOICED"))%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="amount-numeric" Width="15%" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <asp:Label runat="server" ID="lblTotalIssueInvdQty"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:BalanceQty %>">
                                                            <ItemTemplate>
                                                                <%--<asp:Label ID="lblIssueBalance" CssClass="BalancetoPay" runat="server" Text='<%#(Convert.ToDecimal(Eval("CII_QTY_ISSUED").ToString())-Convert.ToDecimal(Eval("CII_QTY_PRV_INVOICED").ToString()))%>'></asp:Label>--%>
                                                                <asp:Label ID="lblIssueBalance" CssClass="BalancetoPay" runat="server" Text='<%#(Eval("CII_QTY_BALANCE"))%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="amount-numeric" Width="15%" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <asp:Label runat="server" ID="lblTotalISSUEBalance"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:InvNow %>" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtInvNowIssueQty" runat="server" CssClass="Uiinput-amount numeric"
                                                                    TabIndex="26" onkeyup="CalculateTotalIssue();" onmousedown="CalculateTotalIssue();"
                                                                    MaxLength="16" Text='<%#Eval("CII_QTY_INVOICED")%>'>
                                                                </asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="vrfISSUEQtySplit" CssClass="star" SetFocusOnError="true"
                                                                    ValidationGroup="ISSUESplit" EnableClientScript="true" runat="server" ControlToValidate="txtInvNowISSUEQty"
                                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Quantity_Valid %>">
                                                                </asp:RequiredFieldValidator>
                                                                <cc1:QuantityValidationP2P ID="vreISSUEQtyInvNow" runat="server" ControlToValidate="txtInvNowISSUEQty"
                                                                    NumberDigits="9" ErrorMessage="<%$ resources:Err_Invalid_InvNow %>" Display="Dynamic"
                                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="ISSUESplit"></cc1:QuantityValidationP2P>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="20%" />
                                                            <HeaderStyle CssClass="amount-numeric" Width="20%" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <asp:Label runat="server" ID="lblTotalIssueInvNow"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </asp:TableCell>
                                    </div>
                                </div>
                            </div>
                            <%-- -------End ISSUE Splitup Popup --------------------------%>

                             <%---------------------------------Start Paid Amount details Popup-----------------------------------%>
            <div id="divPaidAmntSplitup" style="display: none">
                <div class="content-wrapper">
                    <div class="gridwrap">
                        <asp:GridView runat="server" ID="grdPaidAmntSplitup" Width="100%" AllowSorting="false"
                            AutoGenerateColumns="false" TabIndex="106" EmptyDataRowStyle-CssClass="emptytable"
                            ShowFooter="true">
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
                                        <asp:Label ID="lblInvCrAmount" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("INV_AMOUNT")) %>'
                                            ToolTip='<%#GetFormattedNumberWithSeperation(Eval("INV_AMOUNT")) %>'></asp:Label>
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
                                        <asp:Label ID="lblAmount" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("RCVD_AMOUNT")) %>'
                                            ToolTip='<%#GetFormattedNumberWithSeperation(Eval("RCVD_AMOUNT")) %>'></asp:Label>
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
                                <tr align="left" style="color: #122648; background-color: #DEE3ED; border-width: 0px;
                                    font-family: Verdana; font-size: 10px; font-weight: bold; height: 30px;">
                                    <td>
                                        <asp:Label ID="lblBalPay" runat="server" Text='<%$ resources:BalanceAmount %>'></asp:Label>
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

                            <div style="display: none">
                                <asp:Button ID="btnResetAllocation" runat="server" OnClick="ActionHandler" CommandName="RESETISSUEALLOCATION" />
                            </div>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
                <div id="divScriptButtons">
                    <asp:Button runat="server" ID="btnJournalize_Action" CommandName="JOURNALIZE" OnClick="ActionHandler"
                        EnableTheming="false" Style="display: none" />
                    <asp:Button ID="btnJournalizeUpdate" runat="server" OnClick="ActionHandler" CommandName="JOURNALIZEUPDATE"
                        EnableTheming="false" Style="display: none" />
                </div>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary
                        ID="vsPage" ValidationGroup="invoice" runat="server" />
                    <asp:ValidationSummary ID="vsDetails" ValidationGroup="details" runat="server" />
                    <asp:ValidationSummary ID="vsTax" ValidationGroup="tax" runat="server" />
                    <asp:ValidationSummary ID="vsTaxDate" ValidationGroup="taxDate" runat="server" />
                    <asp:ValidationSummary ID="vsHdrTaxDate" ValidationGroup="taxHdrDate" runat="server" />
                    <asp:ValidationSummary ID="vsDeduction" ValidationGroup="deduction" runat="server" />
                    <asp:ValidationSummary ID="vsIssueSplit" ValidationGroup="ISSUESplit" runat="server" />
                    <asp:HiddenField ID="hdfAppType" runat="server" />
                    <asp:HiddenField ID="hdfAppSubType" runat="server" />
                    <asp:HiddenField ID="hdfgroup" runat="server" Value="0" />
                    <asp:HiddenField ID="hdfSelRecordStatus" runat="server" />
                </div>
                <asp:Button ID="btnTermCheck" runat="server" CommandName="GETDUEDATE" OnClick="ActionHandler"
                    EnableTheming="false" Style="display: none;" />
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
            <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
            <asp:HiddenField ID="hdfTxPk" Value="0" runat="server" />
            <asp:HiddenField ID="hdfSubTotal" Value="0" runat="server" />
            <asp:HiddenField ID="hdfTaxFrmDt" Value="" runat="server" />
            <asp:HiddenField ID="hdfTaxToDt" Value="" runat="server" />
            <asp:HiddenField ID="hdfMiscNumberDigits" runat="server" Value="0" />
            <asp:HiddenField ID="hfCancelInv" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIscontYes" runat="server" />
            <asp:HiddenField ID="hdfIsRatecont" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIssueExceed" Value="0" runat="server" />
            <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
            <asp:HiddenField ID="hdfCurrencyGroup2" Value="2" runat="server" />
            <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" />
            <asp:HiddenField ID="hdfIsSBUCustomer" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
