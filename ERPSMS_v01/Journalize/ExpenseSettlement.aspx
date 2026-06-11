<%@ Page Title="<%$ Resources:Captions,Title_ExpSettlement %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="ExpenseSettlement.aspx.cs" Inherits="ERPSMS_v01.Journalize.ExpenseSettlement"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/AlertControl.ascx" TagName="Alert" TagPrefix="uc2" %>
<%@ Register Src="~/Journalize/UserControls/JournalizeControlNew.ascx" TagName="Journalize"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var NumberDigits = 0;
        var CurrencyDigits = 0;
        $(document).ready(function () {
            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
        });

        function ScrollDown() {
            //            window.scroll(400, 400);
            // return false;
        }

        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtExpenseDate");
            GrandScriptUtils.DatePickerCommon("txtInvoiceDueDate");
            GrandScriptUtils.DatePickerCommon("txtPartyInvDate");
            GrandScriptUtils.DatePickerCommon("txtVendorInvDate");
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false, false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendorSch", url, "hdfVendorSch", true, true, "PARTY");
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendor", url, "hdfVendor", true, true, "PARTY");
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendorDtl", url, "hdfVendorDtl", true, false, "PARTY");
            GrandScriptUtils.MakeAutoCompleteDDL("txtAddressType", url + "?VendorPk=" + $("[id$='hdfVendorDtl']").val(), "hdfAddressType", true, false, "VENDORCONTACTS");
            GrandScriptUtils.MakeAutoCompleteDDL("txtHdrAddType", url + "?VendorPk=" + $("[id$='hdfVendor']").val(), "hdfAddTypeHdr", true, false, "VENDORCONTACTS");


            //GrandScriptUtils.MakeAutoCompleteDDL("txtCurrency", url + "?Type=" + $("[id$=hdfVendor]").val() + "&ExcDate=" + $("[id$=txtExpenseDate]").val(), "hdfCurrency", true, true, "VENDORCURRENCY");
            GrandScriptUtils.MakeAutoCompleteDDL("txtExpenseNumber", url, "hdfExpenseNumberPK", true, true, "EXPENSESETTILEMENTINVNUMBER");
            GrandScriptUtils.MakeAutoCompleteDDL("txtUOM", url, "hdfUOM", true, true, "UOM");
            ShowHideItemDetails($("[id$=hdfIsItemDetailsVisible]").val());
            ShowHidePartyDetails($("[id$=hdfIsPartyVisible]").val());
            //For Voucher
            GrandScriptUtils.DatePickerCommon("txtPVDate");
            //$("[id$=txtJournalExchangeRate]").ForceNumericOnly();
            if ($('[id$=btnJournalSaveSubmit]').is(":visible"))
                $('[id$=btnJournalSubmit]').hide();
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();

            //Set a stamp for cancelled invoice
            if ($("[id$=hdfIsInvCancelled]").val() == "1")
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
            else
                $("[id$=tblDetailHdr]").addClass("table-devide");

            //End
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
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlAlert]").hide();
                $("[id$=pnlPrint]").hide();
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
            if (targetControlID == "txtVendor") {
                $("[id$=hdfIsNewParty]").val("1");
                var txtVendor = $("[id$=txtVendor]").val();
                var hdfVendor = $("[id$=hdfVendor]").val();
                $("[id$=txtVendorDtl]").val(txtVendor);
                $("[id$=hdfVendorDtl]").val(hdfVendor);
                $("[id$=btnVendorDtl]").click();
                $("[id$=btnVendor]").click();
            }


            if (targetControlID == "txtVendorDtl") {
                $("[id$=btnVendorDtl]").click();
            }
            else if (targetControlID == "txtAddressType") {
                $("[id$=btnVendorContDtl]").click();
            }
            else if (targetControlID == "txtHdrAddType") {
                $("[id$=txtAddressType]").val($("[id$=txtHdrAddType]").val());
                $("[id$=hdfAddressType]").val($("[id$=hdfAddTypeHdr]").val());
                $("[id$=txtBranchCode]").val($("[id$=txtHdrBranchCode]").val());

                $("[id$=btnHdrVendorCont]").click();
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
        function ShowExpenseAdvances(title) {
            ShowContainerDiv("[id$=divExpenseAdvances]", title, "900", "400");
        }
        function CalculateAmount() {
            var qty = 0;
            var rate = 0;
            var tax = 0;
            var disc = 0;
            var amount = 0;
            qty = parseFloat($("[id$=txtQty]").val());
            rate = parseFloat($("[id$=txtRate]").val());

            tax = parseFloat($("[id$=txtTax]").val());
            disc = parseFloat($("[id$=txtDiscount]").val());

            if (!isNaN(qty) && !isNaN(rate)) {
                amount = qty * rate;
                $("[id$=txtAmount]").val(amount.toFixed(CurrencyDigits));
            }
            else {
                $("[id$=txtAmount]").val(parseFloat(0).toFixed(CurrencyDigits));
            }
            if (isNaN(tax))
            { tax = 0; }
            if (isNaN(disc)) {
                disc = 0;
            }
            if (!isNaN(tax) || !isNaN(disc)) {
                var amount = (amount + tax) - disc;
                $("[id$=txtTotAmt]").val(amount.toFixed(CurrencyDigits));
            }
            else {
                $("[id$=txtTotAmt]").val(parseFloat(0).toFixed(CurrencyDigits));
            }
            $("[id$=btnRecalculateTax]").click();
        }
        function CalculateTotal(sender) {
            var subTotal = $("#[id*=grdItemDetails]").find('[id$=lblSubTotalFooter]').length > 0 ? parseFloat($("#[id*=grdItemDetails]").find('[id$=lblSubTotalFooter]').html().replace(new RegExp(',', 'g'), '')) : parseFloat(0);
            subTotal = isNaN(subTotal) ? 0 : subTotal;
            var totalDiscount = parseFloat($("[id$=txtHdrDiscount]").val());
            totalDiscount = isNaN(totalDiscount) ? 0 : totalDiscount;
            var totalTax = parseFloat($("[id$=txtHdrTax]").val());
            totalTax = isNaN(totalTax) ? 0 : totalTax;
            var totalPriceAdj = parseFloat($("[id$=txtPriceAdj]").val());
            totalPriceAdj = isNaN(totalPriceAdj) ? 0 : totalPriceAdj;
            // var RefundAmount = parseFloat($("[id$=txtRefundAmount]").val());

            var netTotal = (subTotal + totalTax + totalPriceAdj) - totalDiscount;
            $("[id$=txtHdrTotal]").val((netTotal).toFixed(CurrencyDigits));
            $("[id$=txtHdrTotal]").attr("title", (netTotal).toFixed(CurrencyDigits));
            if (totalPriceAdj == 0)
                $("[id$=txtPriceAdj]").val((totalPriceAdj).toFixed(CurrencyDigits));
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
        function CalculateDueDate() {

            var vendDate;
            if ($("[id$=hdfInvDueDateDependsVenInvDate]").val() == 1) { //Inv. Due Date calculation depends upon Vendor Inv Date,Otherwise Invoice date
                vendDate = $.datepicker.parseDate("dd-M-yy", $("[id$=txtVendorInvDate]").val());
            }
            else {
                vendDate = $.datepicker.parseDate("dd-M-yy", $("[id$=txtExpenseDate]").val());
            }
            var dueDays = parseInt($("[id$=txtCreditDays]").val());
            if (vendDate != null && !isNaN(vendDate) && dueDays != null && !isNaN(dueDays)) {
                vendDate.setDate(vendDate.getDate() + dueDays);
                $("[id$=txtInvoiceDueDate]").val($.datepicker.formatDate("dd-M-yy", vendDate));
            }
            $('#txtPartyInvDate').focus();
        }




        function Bindtaxid() {
            $("[id$=hdfVendorDtl]").val("");
            $("[id$=txtAddressType]").val("");
            $("[id$=hdfAddressType]").val("");
            $("[id$=txtVatTaxId]").val("");
            $("[id$=txtBranchCode]").val("");
            $("[id$=chkHO]").attr('checked', false);

        }
        function CalculateDueDays() {
            var vendDate;
            if ($("[id$=hdfInvDueDateDependsVenInvDate]").val() == 1) { //Inv. Due Date calculation depends upon Vendor Inv Date,Otherwise Invoice date
                vendDate = $.datepicker.parseDate("dd-M-yy", $("[id$=txtVendorInvDate]").val());
            }
            else {
                vendDate = $.datepicker.parseDate("dd-M-yy", $("[id$=txtExpenseDate]").val());
            }
            var dueDate = $.datepicker.parseDate("dd-M-yy", $("[id$=txtInvoiceDueDate]").val());
            if (vendDate != null && !isNaN(vendDate) && dueDate != null && !isNaN(dueDate)) {
                var dueDays = (dueDate - vendDate) / (1000 * 60 * 60 * 24);
                if (dueDays >= 0)
                    $("[id$=txtCreditDays]").val(dueDays);
            }
        }
        function AfterDateSelect(controlID) {
            if (controlID == "txtVendorInvDate") {
                CalculateDueDate();
            }
            else if (controlID == "txtExpenseDate") {
                CalculateDueDate();
            }
            else if (controlID == "txtInvoiceDueDate") {
                CalculateDueDays();
            }
            if (controlID == "txtExpenseDate" && $("[id$=hdfHasTax]").val() != "0") {
                ShowErrorMessage('<%=Resources.Messages.TaxDateChanged %>', '<%=Resources.Messages.Information %>');
            }
            if (typeof AfterAlertControlDateSelect == "function") {
                AfterAlertControlDateSelect(controlID);
            }
        }
        function ResetSelection() {
            $('[id$=grdExpenseList]').find('tr td input:radio[id$=rbtSelect]').removeAttr('checked');
            $('[id$=grdExpenseList]').find('tr td input:checkbox[id$=chkInvselect]').removeAttr('checked');
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
        function ShowHidePartyDetails(flag) {
            if (flag == 1) {
                $("[id$=divShowParty]").show();
                $("[id$=imbShowParty]").hide();
                $("[id$=imbHideParty]").show();
            }
            else {
                $("[id$=divShowParty]").hide();
                $("[id$=imbShowParty]").show();
                $("[id$=imbHideParty]").hide();
            }
            $("[id$=hdfIsPartyVisible]").val(flag);
            return false;
        }
        //For carten check
        function ShowPartyNoCheckConfirming() {

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Msg_PartyNo_AlertMsg").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $("[id$=hdfAddItem]").val(1);
                        $(this).dialog("close");
                        $("[id$=btnAddItem]").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfAddItem]").val(0);
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }


        //For Invoice Number Duplicate confirmation
        function InvoiceNoDheckConfirm(type) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Msg_PartyNo_CheckMsg").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $("[id$=hdfPartyNoCheck]").val(1);
                        $(this).dialog("close");
                        if (type == 'Submit') {
                            $("[id$=btnSubmitContinue]").click();
                        }
                        else
                            if (type == 'Save') {
                                $("[id$=btnSaveContinue]").click();
                            }

                    },
                    Cancel: function (e) {
                        $("[id$=hdfPartyNoCheck]").val(0);
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }


        //For Setting/Resetting Colour of a selected InvoiceNo
        function SetSelectedRowColor() {
            var selectedIds;
            var selectedIdsArray = new Array();
            selectedIds = $("[id$=hdfSelectedItemPk]").val();
            selectedIdsArray = selectedIds.split(',');

            for (i = 0; i < selectedIdsArray.length; ++i) {

                if (selectedIdsArray[i] != 0) {
                    $("#<%= grdExpenseList.ClientID %> input[type=hidden][id*=hdfExpenseID]").each(function (index) {
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
            var count = $('[id$=grdExpenseList]').find('tr td input:checkbox[id$=chkInvselect]:checked').length;
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            if (mode == 1) {
                if (count == 0) {
                    msg = '<%= GetLocalResourceObject("Msg_SelectItem").ToString() %>';
                    GrandScriptUtils.ShowModal(msg, msgTitle);
                    return false;
                }
                else if (count > 1) {
                    msg = '<%= GetLocalResourceObject("Msg_SelectoneItem").ToString() %>';
                    GrandScriptUtils.ShowModal(msg, msgTitle);
                    return false;
                }
            }
            else if (mode == 0) {
                if (count > 1) {
                    msg = '<%= GetLocalResourceObject("Msg_SelectoneItem").ToString() %>';
                    GrandScriptUtils.ShowModal(msg, msgTitle);
                    return false;
                }
            }
            else if (mode == 2) {
                if (count == 0) {
                    msg = '<%= GetLocalResourceObject("Msg_SelectItem").ToString() %>';
                    GrandScriptUtils.ShowModal(msg, msgTitle);
                    return false;
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



        function SetDate(controlID) {
            //  $("[id$=btnSet]").click();
        }

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlExpenses">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <div id="divSBUCompany" class="buttoncontainer-fields floatLeft">
                                    <asp:DropDownList ID="ddlCompany" class="medium" runat="server" TabIndex="1" onmouseover="javascript:ShowTooltip('ddlCompany');">
                                    </asp:DropDownList>
                                </div>
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="30"
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
                                        <asp:Button runat="server" ID="btnDeleteExp" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="33" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li id="pnlPrint">
                                        <asp:Button runat="server" TabIndex="34" ID="btnPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="35" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnJournalize" CommandName="JOURNALIZE" TabIndex="36"
                                            Text="<%$resources:Journalize %>" OnClick="ActionHandler" ToolTip="<%$resources:Journalize %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-journalize" />
                                    </li>
                                    <li id="pnlAlert" runat="server">
                                        <asp:Button runat="server" ID="btnAlert" CommandName="ALERT" TabIndex="37" Text="<%$resources:Controls,Alert %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:Controls,Alert %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-alert" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="38" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="8" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelEI %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelEI %>" OnClientClick="javascript:return SelectedCheckBoxCount(1);" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="39" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" OnClientClick="javascript:return SelectedCheckBoxCount(1);" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="40" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" OnClientClick="javascript:return SelectedCheckBoxCount(1);" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnPickForPayment" CommandName="PICKFORPAYMENT" TabIndex="41"
                                            Text="<%$resources:PickPoForPayment %>" OnClick="ActionHandler" ToolTip="Pick Inv & for Paying"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-money" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnPickForCrDrNote" CommandName="PICKFORCRDRNOTE"
                                            TabIndex="41" Text="<%$resources:PickForCrDrNote %>" OnClick="ActionHandler"
                                            ToolTip="<%$resources:PickForCrDrNote %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <%--  <li runat="server" id="pnlResetSelection">
                                        <asp:Button runat="server" ID="btnResetSelection" CommandName="RESET" TabIndex="42"
                                            Text="<%$resources:ResetSelection %>" OnClick="ActionHandler" ToolTip="<%$resources:ResetSelection %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" OnClientClick="ResetSelection()" />
                                    </li>--%>
                                    <li>
                                        <asp:Button runat="server" TabIndex="15" ID="btnPrintLst" CommandName="PRINTLISTING"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" OnClientClick="javascript:return SelectedCheckBoxCount(1);" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <%--  //For SelectedItemId Keeping--%>
                <asp:HiddenField ID="hdfSelectedItemPk" runat="server" Value="0" />
                <div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="45" OnClick="ActionHandler" CommandName="INVOICELIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="46" OnClick="ActionHandler" CommandName="INVOICEDETAIL"
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
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn----------%>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="/*margin-top: 8px; */ background: #f2f2f2;">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblFrmDate" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="47" CssClass="input-small"
                                                MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$resources:ToDate %>" CssClass="middle-lbl-small-c"
                                                AssociatedControlID="txtToDate"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="48" CssClass="input-small" MaxLength="11"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-d" TabIndex="49">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,NotPosted %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Posted %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="lblCompany" runat="server" Text="<%$ resources:Controls, CompanyPlant %>"
                                                AssociatedControlID="ddlCompanySrch" CssClass="lbl-18-3perc"></asp:Label>
                                            <asp:DropDownList ID="ddlCompanySrch" runat="server" CssClass="select-small-a" TabIndex="3">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide ">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblVendorSch" runat="server" Text="<%$resources:Vendor %>" AssociatedControlID="txtVendorSch"></asp:Label>
                                            <asp:TextBox ID="txtVendorSch" runat="server" CssClass="select-half margnbotm0" MaxLength="100"
                                                TabIndex="50"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfVendorSch" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblExpenseNumber" runat="server" Text="<%$resources:ExpenseNo %>"
                                                AssociatedControlID="txtExpenseNumber"></asp:Label>
                                            <asp:TextBox ID="txtExpenseNumber" runat="server" CssClass="select-small-c1 margnbotm0"
                                                MaxLength="100" TabIndex="51"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfExpenseNumberPK" runat="server" Value="" />
                                            <asp:Label ID="lblSearch" runat="server" CssClass="middle-lbl-xsmall-d style-none margnbotm0"
                                                AssociatedControlID="btnSearch"></asp:Label>
                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="52" CssClass="margntop2 margnbotm0" CommandName="SEARCH"
                                                SkinID="search-ext" />
                                            <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="53" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext"
                                                CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdExpenseList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                    AllowPaging="true" OnPageIndexChanging="ActionHandler" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable">
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
                                                <asp:CheckBox runat="server" ID="chkInvselect" TabIndex="54" />
                                                <asp:HiddenField runat="server" ID="hdfExpenseID" Value='<%# Eval("IVH_PK") %>' />
                                                <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval("IVH_DEPT") %>' />
                                                <asp:HiddenField ID="hdfDelStatus" runat="server" Value='<%# Eval("IVH_DEL_STATUS") %>' />
                                                <asp:HiddenField ID="hdfGroup" runat="server" Value='<%# Eval("IVH_GROUP") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ExpenseDate %>" SortExpression="IVH_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExpenseDate" runat="server" Text='<%#  Eval("IVH_DATE")!=""? Convert.ToDateTime( Eval("IVH_DATE")).ToString(Resources.Constants.ReportDateFormat):"" %>'
                                                    ToolTip='<%# Eval("IVH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="false" />
                                            <ItemStyle Width="8%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ExpenseNo %>" SortExpression="IVH_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExpenseNo" runat="server" Text='<%# Eval("IVH_NO") ==""?"[NEW]":Eval("IVH_NO")%>'
                                                    ToolTip='<%# Eval("IVH_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCmpName" CssClass="<%# Eval(Resources.DataFieldRes.CompnayLineColor) %>"
                                                    runat="server" Text='<%# Eval(Resources.DataFieldRes.IVH_COMPANY_TEXT) %>' ToolTip='<%# Eval(Resources.DataFieldRes.IVH_COMPANY_TEXT) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField HeaderText="<%$ resources:RefNo %>" SortExpression="IVH_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExpenseDatea" runat="server" Text='<%#  Eval("IVH_VENDOR_INV_NO")%>'
                                                    ToolTip='<%# Eval("IVH_VENDOR_INV_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Vendor %>" SortExpression="IVH_VENDOR_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendor" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_VENDOR_TEXT"), 42) %>'
                                                    ToolTip='<%# Eval("IVH_VENDOR_TEXT") %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfVendorPK" Value='<%# Eval("IVH_VND_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="26%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvNo %>" SortExpression="IVH_VENDOR_INV_NO"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendorInvNO" runat="server" Text='<%# Eval("IVH_VENDOR_INV_NO") %>'
                                                    ToolTip='<%# Eval("IVH_VENDOR_INV_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                        </asp:TemplateField>
                                        <%--  <asp:TemplateField HeaderText="<%$ resources:InvDate %>" SortExpression="IVH_DATE_RECEIVED">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSoDate" runat="server" Text='<%# Eval("IVH_DATE_RECEIVED", Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval("IVH_DATE_RECEIVED", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="false" />
                                            <ItemStyle Width="8%" Wrap="false" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Cur %>" SortExpression="IVH_CURRENCY_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='<%#Eval("IVH_CURRENCY_TEXT")  %>'
                                                    ToolTip='<%#Eval("IVH_CURRENCY_TEXT")  %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfPOCurrency" Value='<%# Eval("IVH_CURRENCY") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ToSettle %>" SortExpression="IVH_AMOUNT_SET">
                                            <ItemTemplate>
                                                <asp:Label ID="lblToSettle" runat="server" Text='<%# Eval("IVH_AMOUNT_SET", "{0:c}") %>'
                                                    ToolTip='<%# Eval("IVH_AMOUNT_SET", "{0:c}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="false" />
                                            <ItemStyle Width="8%" HorizontalAlign="Right" />
                                             <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %>" SortExpression="IVH_AMOUNT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExpenseValue" runat="server" Text='<%# Eval("IVH_AMOUNT", "{0:c}") %>'
                                                    ToolTip='<%# Eval("IVH_AMOUNT", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="<%$ resources:Balance %>" SortExpression="IVH_AMOUNT_REFUND">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalance" runat="server" Text='<%# Eval("IVH_AMOUNT_REFUND", "{0:c}") %>'
                                                    ToolTip='<%# Eval("IVH_AMOUNT_REFUND", "{0:c}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="false" />
                                           <ItemStyle Width="8%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <%--  <asp:TemplateField HeaderText="<%$ resources:BalAmt %>" SortExpression="IVH_BAL_AMNT_TC">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalAmt" runat="server" Text='<%# Eval("IVH_BAL_AMNT_TC", "{0:c}") %>'
                                                    ToolTip='<%# Eval("IVH_BAL_AMNT_TC", "{0:c}") %>' Visible='<%# GetBalanceLableVisibility(Eval("IVH_AMOUNT").ToString(),Eval("IVH_BAL_AMNT_TC").ToString())%>'></asp:Label>
                                                <asp:LinkButton ID="lbnBalAmt" runat="server" Text='<%# Eval("IVH_BAL_AMNT_TC", "{0:c}") %>'
                                                    CssClass="text-underline" ToolTip='<%# Eval("IVH_BAL_AMNT_TC", "{0:c}") %>' OnClick="ActionHandler"
                                                    CommandName="AMOUNTDETAILS" Visible='<%# GetBalanceLinkVisibility(Eval("IVH_AMOUNT").ToString(),Eval("IVH_BAL_AMNT_TC").ToString())%>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField HeaderText="<%$ resources:DueDate %>" SortExpression="IVH_DUE_DT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDueDate" runat="server" Text='<%# Eval("IVH_DUE_DT", Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval("IVH_DUE_DT", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="false" />
                                            <ItemStyle Width="7%" Wrap="false" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("ASC_CSS_CLASS") %>' ToolTip='<%# Eval("IVH_STATUS_TEXT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval("IVH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgPosted" runat="server" OnClientClick="javascript:return false;" />
                                                <asp:HiddenField runat="server" ID="hdfPosted" Value='<%# Eval("IVH_HAS_JRNL_ENTRY") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="3%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <%-- <uc1:PagerControl ID="uclPaging" runat="server" />--%>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <table class="table-devide" id="tblDetailHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
                                            <asp:HiddenField ID="hdfCurrencyDigits" runat="server" Value="3" />
                                            <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                                            <asp:HiddenField ID="hdfDecimalFormatWithSeperation" runat="server" />
                                            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                                            <asp:HiddenField ID="hdfCurrencyFormatWithSeperation" runat="server" />
                                            <asp:HiddenField ID="hdfRateFormat" runat="server" />
                                            <asp:HiddenField ID="hdfExpensePK" runat="server" />
                                            <asp:HiddenField ID="hdfExchangeRate" runat="server" />
                                            <asp:HiddenField ID="hdfTaxCategory" runat="server" />
                                            <%--<asp:HiddenField ID="hdfHdrCurrency" runat="server" />--%>
                                            <asp:Label ID="lblExpNo" runat="server" Text="<%$ resources:ExpenseNo%>" AssociatedControlID="lblExpenseNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblExpenseNo" CssClass="input-small" TabIndex="1"></asp:Label>
                                            <asp:HiddenField ID="hdfExpenseNo" runat="server" />
                                            <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                                            <asp:HiddenField ID="AST_CODE" runat="server" />
                                            <asp:HiddenField ID="hdfIsInvCancelled" Value="0" runat="server" />
                                            <asp:Label runat="server" ID="lblExpenseDate" Text="<%$ resources:ExpenseDate%>"
                                                CssClass="middle-lbl" AssociatedControlID="txtExpenseDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtExpenseDate" CssClass=" input-small" TabIndex="2"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfHasTax" runat="server" Value="0" />
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="vrfExpenseDate" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtExpenseDate"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExpenseDate %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="vrfTaxDate" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="taxDate" EnableClientScript="true" runat="server" ControlToValidate="txtExpenseDate"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExpenseDate %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="vrfTaxHdrDate" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="taxHdrDate" EnableClientScript="true" runat="server" ControlToValidate="txtExpenseDate"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExpenseDate %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <asp:Label runat="server" ID="lblCurrency" Text="<%$ resources:Currency%>" AssociatedControlID="txtCurrency"></asp:Label>
                                            <asp:TextBox ID="txtCurrency" runat="server" Enabled="false" CssClass="input-small input-disabled"
                                                TabIndex="4" MaxLength="100"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCurrency" runat="server" />
                                            <asp:HiddenField ID="hdfVendorInvType" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfCurrency" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtCurrency"
                                                Display="Dynamic" InitialValue="<%$ resources:Messages, AutoDefaultValue %>"
                                                Text="*" ErrorMessage="<%$ resources:Err_Currency%>"></asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblVendorInvNO" Text="<%$ resources:RefNo%>" CssClass="middle-lbl"
                                                AssociatedControlID="txtVendorInvNO"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtVendorInvNO" CssClass="input-small" TabIndex="5"
                                                MaxLength="100"></asp:TextBox>
                                            <%-- <asp:RequiredFieldValidator ID="vrfVendorInvNO" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtVendorInvNO"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_VendorInvNO %>">
                                            </asp:RequiredFieldValidator>--%>
                                            <div class="clear">
                                            </div>
                                            <%--Start--%>
                                            <asp:DropDownList ID="ddlAddressTypeHdr" runat="server" TabIndex="13" OnSelectedIndexChanged="ActionHandler"
                                                AutoPostBack="true" CssClass="select-small" Visible="false">
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hdfAddTypeHdr" runat="server" Value="" />
                                            <asp:Label runat="server" ID="lbHdrAddType" Text="<%$ resources:Type%>" AssociatedControlID="txtHdrAddType"></asp:Label>
                                            <asp:TextBox ID="txtHdrAddType" runat="server" MaxLength="100" TabIndex="8" CssClass="input-small"
                                                OnTextChanged="ActionHandler" AutoPostBack="true"> </asp:TextBox>
                                            <asp:Button ID="btnHdrVendorCont" runat="server" OnClick="ActionHandler" CommandName="CHANGE"
                                                Style="display: none" EnableTheming="false" />
                                            <asp:RequiredFieldValidator ID="vrfHdrAddressType" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtHdrAddType"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Type %>">
                                            </asp:RequiredFieldValidator>
                                            &nbsp&nbsp
                                            <asp:Label runat="server" ID="lblHdrchkBH" Text="<%$ resources:HO%>" CssClass="middle-lbl-xsmall-a1"
                                                AssociatedControlID="HdrchkHO"></asp:Label>
                                            <asp:CheckBox ID="HdrchkHO" runat="server" Checked="false" TabIndex="8" OnCheckedChanged="ActionHandler"
                                                AutoPostBack="true" />
                                            <%--<asp:Label runat="server" ID="lblHdrBrCode" Text="<%$ resources:BranchCode%>" AssociatedControlID="txtHdrBranchCode"></asp:Label>--%>
                                            <asp:TextBox ID="txtHdrBranchCode" runat="server" Width="10px" MaxLength="5" TabIndex="8"
                                                CssClass="input-small" />
                                            <asp:RequiredFieldValidator ID="vrfHdrBranchCode" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtHdrBranchCode"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_BranchCode %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblOriginal" runat="server" Text="<%$ resources:OriginalInvReceived%>"
                                                AssociatedControlID="chkOriginalinvoice" CssClass="lbl-25-1perc"></asp:Label>
                                            <asp:CheckBox ID="chkOriginalinvoice" TabIndex="11" runat="server" />
                                            <asp:Label runat="server" ID="lblInvoiceGstType" Text="<%$ resources:InvoiceGstType%>"
                                                AssociatedControlID="ddlInvoiceGstType"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlInvoiceGstType" TabIndex="16" CssClass="input-medium">
                                            </asp:DropDownList>
                                            <%--End--%>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblVendor" runat="server" Text="<%$resources:Vendor %>" AssociatedControlID="txtVendor"></asp:Label>
                                            <asp:TextBox ID="txtVendor" runat="server" MaxLength="100" TabIndex="3" CssClass="input-half"> </asp:TextBox>
                                            <asp:ImageButton ID="imbExpnseAdvances" SkinID="expense" runat="server" OnClick="ActionHandler"
                                                CssClass="margntop2" TabIndex="4" ToolTip="<%$ resources:ExpenseAdvances %>"
                                                CommandName="EXPENSEADVANCES" />
                                            <asp:HiddenField ID="hdfVendor" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfVendor" CssClass="star" SetFocusOnError="true"
                                                InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>" ValidationGroup="invoice"
                                                EnableClientScript="true" runat="server" ControlToValidate="txtVendor" Display="Dynamic"
                                                Text="*" ErrorMessage="<%$ resources:Err_Vendor %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Button ID="btnVendor" runat="server" OnClick="ActionHandler" CommandName="VENDORSELECTED"
                                                Style="display: none" EnableTheming="false" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblVendorInvDate" Text="<%$ resources:RefDate%>" AssociatedControlID="txtVendorInvDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtVendorInvDate" CssClass="date-picker input-small"
                                                TabIndex="6" onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"
                                                onchange="CalculateDueDate();"></asp:TextBox>
                                            <%-- <asp:RequiredFieldValidator ID="vrfVendorInvDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtVendorInvDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_VendorInvDate %>">
                                            </asp:RequiredFieldValidator>--%>
                                            <asp:Label runat="server" ID="lblCreditDays" Text="<%$ resources:CreditDays%>" CssClass="middle-lbl-small-e"
                                                AssociatedControlID="txtCreditDays"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtCreditDays" TabIndex="7" MaxLength="100" CssClass="input-small"
                                                onchange="CalculateDueDate();"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="vreCreditDays" runat="server" ControlToValidate="txtCreditDays"
                                                ErrorMessage="<%$ resources:Err_CreditDays %>" ValidationExpression="^\$?([0-9]{0,10})?$"
                                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice">
                                            </asp:RegularExpressionValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblHdrTaxId" Text="<%$ resources:Taxid%>" AssociatedControlID="txtHdrTaxId"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtHdrTaxId" TabIndex="8" CssClass="input-small"></asp:TextBox>
                                            <asp:Label runat="server" ID="lblInvoiceDueDate" Text="<%$ resources:InvoiceDueDate%>"
                                                CssClass="middle-lbl-small-d" AssociatedControlID="txtInvoiceDueDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInvoiceDueDate" CssClass=" input-small" TabIndex="8"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" onchange="CalculateDueDays();"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfInvoiceDueDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" runat="server" ControlToValidate="txtInvoiceDueDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_InvoiceDueDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblAmountToSettle" Text="<%$ resources:AmountToSettle%>"
                                                AssociatedControlID="txtAmountToSettle"></asp:Label>
                                            <asp:TextBox runat="server" Enabled="false" ID="txtAmountToSettle" Text="0.0" TabIndex="8"
                                                CssClass="input-small input-bgGreen numeric"></asp:TextBox>
                                        </div>
                                    </td>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks %>" AssociatedControlID="txtRemarks"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtRemarks" TabIndex="8" TextMode="MultiLine" CssClass="multiline-2line"
                                                    onkeydown="limitText(this,500);" onchange="limitText(this,500);"></asp:TextBox>
                                            </div>
                                            <div class="div2col-S">
                                                <%--  <asp:Label ID="lblOriginal" runat="server" Text="<%$ resources:OriginalInvReceived%>"
                                                    AssociatedControlID="chkOriginalinvoice" CssClass="input-w12-5per"></asp:Label>
                                                <asp:CheckBox ID="chkOriginalinvoice" TabIndex="11" runat="server" />
                                                <asp:Label ID="lblCompanyView" Visible="true"  runat="server" Text="<%$ resources:Controls, CompanyPlant %>"  AssociatedControlID="ddlCompanyView" CssClass="lbl-4-1perc"></asp:Label>
                                            <asp:DropDownList ID="ddlCompanyView" Visible="true"  Enabled="false"  runat="server" CssClass="input-w13-8per"  >
                                                </asp:DropDownList>--%>
                                            </div>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblCompanyView" Visible="true" runat="server" Text="<%$ resources:Controls, CompanyPlant %>"
                                                    AssociatedControlID="ddlCompanyView" CssClass="lbl-12-5perc"></asp:Label>
                                                <asp:DropDownList ID="ddlCompanyView" Visible="true" Enabled="false" runat="server"
                                                    CssClass="input-w13-8per">
                                                </asp:DropDownList>
                                            </div>
                                        </td>
                                    </tr>
                                </tr>
                            </table>
                            <div class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("ExpenseDetails").ToString() + " :"%></h1>
                                <asp:ImageButton runat="server" ID="imbShowItemDetails" OnClientClick="javascript:return ShowHideItemDetails(1);"
                                    SkinID="imbArrowInactive" ToolTip="<%$ resources:ShowItemDetails %>" TabIndex="9" />
                                <asp:ImageButton runat="server" ID="imbHideItemDetails" OnClientClick="javascript:return ShowHideItemDetails();"
                                    Style="display: none" SkinID="imbArrowActive" ToolTip="<%$ resources:HideItemDetails %>"
                                    TabIndex="9" />
                                <asp:HiddenField ID="hdfIsItemDetailsVisible" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divItemDetails" style="display: none">
                                <table id="tblDetails">
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:HiddenField ID="hdfDetailPK" runat="server" Value="0" />
                                                <asp:Label ID="lblDesc" runat="server" AssociatedControlID="txtDesc" Text="<%$ resources:Desc_Mand %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtDesc" runat="server" TabIndex="9" MaxLength="200"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfDesc" CssClass="star" SetFocusOnError="true" ValidationGroup="details"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtDesc" Display="Dynamic"
                                                    Text="*" ErrorMessage="<%$ resources:Err_Desc %>"></asp:RequiredFieldValidator>
                                                <asp:ImageButton runat="server" ID="imbShowParty" OnClientClick="javascript:return ShowHidePartyDetails(1);"
                                                    SkinID="imbArrowInactiveParty" ToolTip="<%$ resources:ShowParty %>" Visible='<%$ resources:IsShowParty %>'
                                                    TabIndex="9" Style="margin-top: 3px!important; margin-bottom: 0px!important;" />
                                                <asp:ImageButton runat="server" ID="imbHideParty" OnClientClick="javascript:return ShowHidePartyDetails();"
                                                    Style="display: none; margin-top: 3px!important; margin-bottom: 0px!important;"
                                                    SkinID="imbArrowActiveParty" ToolTip="<%$ resources:HideParty %>" Visible='<%$ resources:IsShowParty %>'
                                                    TabIndex="9" />
                                                <asp:HiddenField ID="hdfIsPartyVisible" runat="server" Value="0" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <div id="divShowParty" style="display: none">
                                                <table class="table-devide">
                                                    <tr>
                                                        <td>
                                                            <div class="div2col-S">
                                                                <asp:Label ID="lblPartyInvNo" runat="server" AssociatedControlID="txtPartyNo" Text="<%$ resources:PartyInvNo %>">
                                                                </asp:Label>
                                                                <asp:TextBox runat="server" ID="txtPartyNo" CssClass="input-small" TabIndex="10"
                                                                    MaxLength="100"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lblPartyInvDate" Text="<%$ resources:PartyInvDate%>"
                                                                    CssClass="middle-lbl" AssociatedControlID="txtPartyInvDate"></asp:Label>
                                                                <asp:TextBox runat="server" ID="txtPartyInvDate" CssClass="input-small" TabIndex="10"
                                                                    onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" onchange="CalculateDueDate();"></asp:TextBox>
                                                                <div class="clear">
                                                                </div>
                                                                <asp:DropDownList ID="ddlAddressType" runat="server" TabIndex="12" OnSelectedIndexChanged="ActionHandler"
                                                                    AutoPostBack="true" CssClass="select-small" Visible="false">
                                                                </asp:DropDownList>
                                                                <asp:HiddenField ID="hdfAddressType" runat="server" Value="" />
                                                                <asp:Label runat="server" ID="lblAddressType" Text="<%$ resources:Type%>" AssociatedControlID="txtAddressType"></asp:Label>
                                                                <%--<asp:TextBox ID="txtAddressType" runat="server" MaxLength="100" TabIndex="7"> </asp:TextBox>--%>
                                                                <asp:TextBox ID="txtAddressType" runat="server" MaxLength="100" TabIndex="13" Width="200"
                                                                    CssClass="input-small" OnTextChanged="ActionHandler" AutoPostBack="true"> </asp:TextBox>
                                                                <asp:Button ID="btnVendorContDtl" runat="server" OnClick="ActionHandler" CommandName="CHANGETYPE"
                                                                    Style="display: none" EnableTheming="false" />
                                                                <asp:RequiredFieldValidator ID="vrfAddressType" CssClass="star" SetFocusOnError="true"
                                                                    ValidationGroup="vatbuy" EnableClientScript="true" runat="server" ControlToValidate="txtAddressType"
                                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Type %>">
                                                                </asp:RequiredFieldValidator>
                                                                &nbsp&nbsp
                                                                <asp:Label runat="server" ID="lblchkBH" Width="22px" Text="<%$ resources:HO%>" CssClass="middle-lbl"
                                                                    AssociatedControlID="chkHO"></asp:Label>
                                                                <asp:CheckBox ID="chkHO" runat="server" Checked="false" TabIndex="13" OnCheckedChanged="ActionHandler"
                                                                    AutoPostBack="true" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="div2col-S">
                                                                <asp:Label ID="lblVendorDtl" runat="server" Text="<%$resources:Vendor %>" AssociatedControlID="txtVendorDtl"></asp:Label>
                                                                <asp:TextBox ID="txtVendorDtl" runat="server" MaxLength="100" TabIndex="11" onkeydown="Bindtaxid();"
                                                                    CssClass="select-half" OnTextChanged="ActionHandler" AutoPostBack="true"> </asp:TextBox>
                                                                <asp:HiddenField ID="hdfVendorDtl" runat="server" />
                                                                <asp:RequiredFieldValidator ID="vrfVendorDtl" CssClass="star" SetFocusOnError="true"
                                                                    InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>" ValidationGroup="invoice"
                                                                    EnableClientScript="true" runat="server" ControlToValidate="txtVendorDtl" Display="Dynamic"
                                                                    Text="*" ErrorMessage="<%$ resources:Err_Vendor %>">
                                                                </asp:RequiredFieldValidator>
                                                                <asp:Button ID="btnVendorDtl" runat="server" OnClick="ActionHandler" CommandName="VENDORSELECTEDDTL"
                                                                    Style="display: none" EnableTheming="false" />
                                                                <div class="clear">
                                                                </div>
                                                                <asp:Label runat="server" ID="lblVatBuyTaxId" Text="<%$ resources:Taxid%>" AssociatedControlID="txtVatTaxId"></asp:Label>
                                                                <asp:TextBox runat="server" ID="txtVatTaxId" TabIndex="14" CssClass="input-small"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lblBranchCode" Text="<%$ resources:BranchCode%>" CssClass="middle-lbl-small-d"
                                                                    AssociatedControlID="txtBranchCode"></asp:Label>
                                                                <asp:TextBox ID="txtBranchCode" runat="server" CssClass="input-small" MaxLength="5"
                                                                    TabIndex="15" />
                                                                <asp:RequiredFieldValidator ID="vrfBranchCode" CssClass="star" SetFocusOnError="true"
                                                                    ValidationGroup="vatbuy" EnableClientScript="true" runat="server" ControlToValidate="txtBranchCode"
                                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_BranchCode %>">
                                                                </asp:RequiredFieldValidator>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table class="table-devide">
                                                <tr>
                                                    <td>
                                                        <div class="div2col-S">
                                                            <asp:Label ID="lblQty" runat="server" AssociatedControlID="txtQty" Text="<%$ resources:Quantity_Mand %>"><%--<%$ resources:Quantity %>--%>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtQty" runat="server" CssClass="input-w80 numeric input-small"
                                                                MaxLength="11" TabIndex="17" onchange="CalculateAmount();" Text="1"></asp:TextBox>
                                                            <span style="width: 10px; border: 0 none; background: none;" class="nomargin">
                                                                <div class="starwrap">
                                                                    <asp:RequiredFieldValidator ID="vrfQuantity" CssClass="star" SetFocusOnError="true"
                                                                        ValidationGroup="details" EnableClientScript="true" runat="server" ControlToValidate="txtQty"
                                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Quantity %>">
                                                                    </asp:RequiredFieldValidator>
                                                                    <cc1:QuantityValidationP2P ID="vreQuantity" runat="server" ControlToValidate="txtQty"
                                                                        NumberDigits="7" ErrorMessage="<%$ resources:Err_Quantity_Valid %>" Display="Dynamic"
                                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="details"
                                                                        NonZero="true"></cc1:QuantityValidationP2P>
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
                                                            <asp:TextBox runat="server" ID="txtUOM" CssClass="Uiinput-uom" TabIndex="17"></asp:TextBox>
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
                                                            <asp:TextBox ID="txtDiscount" runat="server" CssClass="input-w80 input-disabled numeric input-small"
                                                                MaxLength="14" onkeydown="return EnableArrowKey(event)" onpaste="return false;"></asp:TextBox>
                                                            <asp:ImageButton ID="imgDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                                CssClass="margntop2" TabIndex="19" ToolTip="<%$ resources:Controls,Discounts %>"
                                                                CommandName="DISCDETAILS" ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')" />
                                                            <asp:Label ID="lblItemTax" runat="server" CssClass="lbl-14-2perc" AssociatedControlID="txtTax"
                                                                Text="<%$ resources:Tax %>">
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtTax" runat="server" CssClass="input-w80 input-disabled numeric medium input-small"
                                                                MaxLength="14" onkeydown="return EnableArrowKey(event)" onpaste="return false;"></asp:TextBox>
                                                            <asp:ImageButton ID="imgTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                                CssClass="margntop2" TabIndex="19" ToolTip="<%$ resources:Tax %>" CommandName="TAXDETAILS"
                                                                ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')" />
                                                            <%-- <asp:TextBox ID="txtPartyNo" runat="server"
                                                    MaxLength="14" onkeydown="return EnableArrowKey(event)" onpaste="return false;"></asp:TextBox>--%>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="div2col-S">
                                                            <asp:Label ID="lblRate" runat="server" AssociatedControlID="txtRate" Text="<%$ resources:Rate_Mand %>">
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtRate" runat="server" CssClass="input-w80 numeric medium input-small"
                                                                MaxLength="18" TabIndex="18" onchange="CalculateAmount();"></asp:TextBox>
                                                            <div class="starwrap">
                                                                <asp:RequiredFieldValidator ID="vrfRate" CssClass="star" SetFocusOnError="true" ValidationGroup="details"
                                                                    EnableClientScript="true" runat="server" ControlToValidate="txtRate" Display="Dynamic"
                                                                    Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                                </asp:RequiredFieldValidator>
                                                                <cc1:RateValidation ID="vreRate" runat="server" ControlToValidate="txtRate" ErrorMessage="<%$ resources:Err_Rate_Valid %>"
                                                                    NumberDigits="10" Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star"
                                                                    ValidationGroup="details" NonZero="true"></cc1:RateValidation>
                                                                <asp:RequiredFieldValidator ID="vrfRateTaxDate" CssClass="star" SetFocusOnError="true"
                                                                    ValidationGroup="taxDate" EnableClientScript="true" runat="server" ControlToValidate="txtRate"
                                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                                </asp:RequiredFieldValidator>
                                                                <cc1:RateValidation ID="vreRateTaxDate" runat="server" ControlToValidate="txtRate"
                                                                    ErrorMessage="<%$ resources:Err_Rate_Valid %>" NumberDigits="10" Display="Dynamic"
                                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="taxDate"
                                                                    NonZero="true"></cc1:RateValidation>
                                                            </div>
                                                            <asp:Label ID="lblAmount" runat="server" CssClass="middle-lbl-small-d" AssociatedControlID="txtAmount"
                                                                Text="<%$ resources:Amount_Mand %>">
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="input-w80 input-disabled numeric medium input-small"
                                                                MaxLength="15" onkeydown="return EnableArrowKey(event);" onpaste="return false;"></asp:TextBox>
                                                            <asp:Button ID="btnRecalculateTax" runat="server" EnableTheming="false" Style="display: none"
                                                                OnClick="ActionHandler" CommandName="CALCULATEDTLTAX" />
                                                            <div class="starwrap">
                                                                <asp:RequiredFieldValidator ID="vrfAmount" CssClass="star" SetFocusOnError="true"
                                                                    ValidationGroup="details" EnableClientScript="true" runat="server" ControlToValidate="txtAmount"
                                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                                </asp:RequiredFieldValidator>
                                                                <cc1:AmountValidation ID="vamAmount" runat="server" ControlToValidate="txtAmount"
                                                                    ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="details"></cc1:AmountValidation>
                                                                <asp:RequiredFieldValidator ID="vrfAmountTaxDate" CssClass="star" SetFocusOnError="true"
                                                                    ValidationGroup="taxDate" EnableClientScript="true" runat="server" ControlToValidate="txtAmount"
                                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                                </asp:RequiredFieldValidator>
                                                                <cc1:AmountValidation ID="vreAmountTaxDate" runat="server" ControlToValidate="txtAmount"
                                                                    ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="taxDate"></cc1:AmountValidation>
                                                            </div>
                                                            <div class="clear">
                                                            </div>
                                                            <asp:Label ID="lbltotamt" runat="server" AssociatedControlID="txtTotAmt" Text="<%$ resources:Tot_Amt %>">
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtTotAmt" runat="server" CssClass="input-w80 input-disabled numeric medium input-small"
                                                                MaxLength="15"></asp:TextBox>
                                                            <asp:Label ID="Label1" runat="server" CssClass="middle-lbl-small-d" AssociatedControlID="txtTaxRefundDate"
                                                                Text="Tax Refund Month">
                                                            </asp:Label>
                                                            <asp:TextBox runat="server" ID="txtTaxRefundDate" ClientIDMode="Static" CssClass="input-small"
                                                                TabIndex="19" MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"
                                                                ValidationGroup="taxDate"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="txtCalender_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                                TargetControlID="txtTaxRefundDate" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                                                                ClientIDMode="Static" OnClientHidden="onCalendarHidden" OnClientDateSelectionChanged="SetDate">
                                                            </cc1:CalendarExtender>
                                                        </div>
                                                        <div class="clear">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td>
                                            <div class="div2col-S">
                                                <%--<asp:HiddenField ID="HiddenField4" runat="server" />
                            <asp:HiddenField ID="HiddenField5" runat="server" />--%>
                                                <asp:Label ID="lblMaterial" runat="server" Text="<%$ resources:Material %>" AssociatedControlID="txtMaterial"></asp:Label>
                                                <asp:TextBox ID="txtMaterial" TabIndex="56" runat="server" EnableViewState="false"
                                                    CssClass="large"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfMaterial" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="vatbuy" EnableClientScript="true" runat="server" ControlToValidate="txtMaterial"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Material %>">
                                                </asp:RequiredFieldValidator>
                                                <%--      <asp:ImageButton ID="imgVatPopupAdd" SkinID="imbaddnew" runat="server" OnClick="ActionHandler"
                                                    TabIndex="57" CommandArgument="PageAction_Entry" ValidationGroup="vatbuy" ToolTip="Add"
                                                    CommandName="VATTAXADD" OnClientClick="javascript:ValidatePageNow('vatbuy')" />--%>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <tr>
                                                </tr>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:HiddenField ID="hdfAddItem" runat="server" Value="0" />
                                                <asp:TextBox ID="txtTotal" runat="server" EnableTheming="false" Text="0" Style="display: none" />
                                                <asp:Label runat="server" ID="lblDtlRemark" Text="<%$ resources:Remarks %>" AssociatedControlID="txtDtlRemark"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDtlRemark" TabIndex="19" MaxLength="200"></asp:TextBox>
                                                <asp:ImageButton runat="server" ID="btnAddItem" CommandName="ADDITEM" TabIndex="21"
                                                    OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('details')"
                                                    ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="PageAction_Entry" ValidationGroup="details"
                                                    CssClass="margntop2" SkinID="plus" />
                                                <asp:ImageButton runat="server" ID="btnClearItem" CommandName="CLEARITEM" TabIndex="22"
                                                    CssClass="margntop2" OnClick="ActionHandler" ToolTip="<%$resources:Controls,Clear %>"
                                                    CommandArgument="PageAction_Entry" SkinID="cancel" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="gridwrap" style="width: 100%; overflow: auto; max-width: 2090px;">
                                <asp:GridView ID="grdItemDetails" runat="server" AutoGenerateColumns="False" PageSize="<%$ resources:PageSize %>"
                                    AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                    TabIndex="22" ShowFooter="true" OnRowDataBound="ActionHandler" Width="1975px">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:ExpenseDescription %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfSODPK" runat="server" Value='<%#Eval("VID_PK") %>' />
                                                <asp:Label ID="lblExpenseDescription" runat="server" Text='<%#  HttpUtility.HtmlDecode(Convert.ToString(Eval("VID_INSTRUCTIONS"))) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("VID_INSTRUCTIONS"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblfooterTot" Text="<%$ resources:Total %>" CssClass="txtAlign-right"></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle Width="650px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Quantity%>" HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblQuantity" runat="server" CssClass="ItemQuantity" Text='<%# GetFormattedNumber(Eval("VID_QTY_INVOICED")) %>'
                                                    ToolTip='<%# GetFormattedNumber(Eval("VID_QTY_INVOICED")) %>'></asp:Label>--%>
                                                <asp:Label ID="lblQuantity" runat="server" CssClass="ItemQuantity" Text='<%# GetFormattedCurrencyWithSeperation(Eval("VID_QTY_INVOICED")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperation(Eval("VID_QTY_INVOICED")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblItemTotalQty" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:UOM%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUOM" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("VID_UOM_TEXT"),15) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("VID_UOM_TEXT"))) %>'></asp:Label>
                                                <asp:HiddenField ID="hdfUoM" runat="server" Value='<%#Eval("VID_UOM") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="30px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Rate %>" HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemRate" runat="server" Text='<%# GetFormattedRate(Eval("VID_RATE")) %>'
                                                    ToolTip='<%# GetFormattedRate(Eval("VID_RATE")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="80px" CssClass="amount-numeric" />
                                            <FooterTemplate>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %>" HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemAmount" runat="server" Text='<%# Eval("VID_AMOUNT", "{0:c}") %>'
                                                    ToolTip='<%# Eval("VID_AMOUNT", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="80px" CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblItemTotalAmount" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Discount %>" HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemDiscount" runat="server" Text='<%# Eval("VID_DISCOUNT", "{0:c}") %>'
                                                    ToolTip='<%# Eval("VID_DISCOUNT", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="80px" CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblDiscountTotal"></asp:Label></FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Tax %>" HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemTax" runat="server" Text='<%# Eval("VID_TAX", "{0:c}") %>'
                                                    ToolTip='<%# Eval("VID_TAX", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="60px" CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTaxTotal"></asp:Label></FooterTemplate>
                                        </asp:TemplateField>
                                        <%--Pay Now--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Total %>" HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemTotal" runat="server" Text='<%# Eval("VID_NET_AMOUNT", "{0:c}") %>'
                                                    ToolTip='<%# Eval("VID_NET_AMOUNT", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblSubTotalFooter" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:remarks %>">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("VID_REMARKS"), 80) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("VID_REMARKS"))) %>'
                                                    ID="lblItemRemarks"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="90px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Vendor %>" Visible='<%$ resources:IsShowParty %>'>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("VID_VENDOR_TEXT"), 10) %>'
                                                    ToolTip='<%# Eval("VID_VENDOR_TEXT", Resources.Constants.DateFormatGrid) %>'
                                                    ID="lblParty"></asp:Label>
                                                <asp:HiddenField ID="hdfVENDORdtl" runat="server" Value='<%#Eval("VID_VENDOR") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="130px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PartyInvNo %>" Visible='<%$ resources:IsShowParty %>'>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("VID_REF_NO"), 10) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("VID_REF_NO"))) %>'
                                                    ID="lblItemPartyInvNo"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="110px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PartyInvDate %>" Visible='<%$ resources:IsShowParty %>'>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("VID_REF_DATE", Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval("VID_REF_DATE", Resources.Constants.DateFormatGrid) %>' ID="lblItemPartyInvDate"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="110px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:taxid %>" Visible='<%$ resources:IsShowParty %>'>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("VID_TAX_ID"), 15) %>'
                                                    ToolTip='<%# Eval("VID_TAX_ID", Resources.Constants.DateFormatGrid) %>' ID="lbltaxid"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="60px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:HO %>" Visible='<%$ resources:IsShowParty %>'>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfBtype" runat="server" Value='<%#Eval("VID_BRANCH_TYPE") %>' />
                                                <asp:Label runat="server" Text='' ToolTip='' ID="lbltype"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:HOBranch %>" Visible='<%$ resources:IsShowParty %>'>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("VID_BRANCH_NAME"), 15) %>'
                                                    ToolTip='<%# Eval("VID_BRANCH_NAME", Resources.Constants.DateFormatGrid) %>'
                                                    ID="lblBRANCHName"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BranchCode %>" Visible='<%$ resources:IsShowParty %>'>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("VID_BRANCH_TEXT"), 10) %>'
                                                    ToolTip='<%# Eval("VID_BRANCH_TEXT", Resources.Constants.DateFormatGrid) %>'
                                                    ID="lblBranchCode"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnEditItem" runat="server" OnClick="ActionHandler" CommandName="EDITITEM"
                                                    CommandArgument="PageAction_Entry" SkinID="imbeditgrid" ToolTip="Edit" TabIndex="22"
                                                    OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load" />
                                                <asp:ImageButton ID="btnRemoveItem" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                                                    CommandArgument="PageAction_Entry" OnClientClick="return ShowDeleteConfirm(this);"
                                                    SkinID="imbdeletegrid" ToolTip="Delete" TabIndex="22" OnPreRender="btnAction_PreRender"
                                                    OnLoad="btnAction_Load" />
                                            </ItemTemplate>
                                            <ItemStyle Width="45px" Wrap="false" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div id="divCalc">
                                <div class="gridwrap">
                                    <table id="tblCalc" class="gridwraptable gridwrap">
                                        <tr>
                                            <td style="width: 85%; text-align: right">
                                                <asp:Label runat="server" ID="lblDiscount" Text="<%$ resources:Discount%>" AssociatedControlID="txtHdrDiscount"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgHdrDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                    ValidationGroup="taxHdrDate" OnClientClick="javascript:ValidatePageNow('taxHdrDate')"
                                                    TabIndex="23" ToolTip="<%$ resources:Controls,Discounts %>" CommandName="DISCHEADER" />
                                                <asp:TextBox ID="txtHdrDiscount" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    MaxLength="16" Enabled="false"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblTax" Text="<%$ resources:Tax%>" AssociatedControlID="txtHdrTax"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgHdrTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                    ValidationGroup="taxHdrDate" OnClientClick="javascript:ValidatePageNow('taxHdrDate')"
                                                    TabIndex="24" ToolTip="<%$ resources:Tax %>" CommandName="TAXHEADER" />
                                                <asp:TextBox ID="txtHdrTax" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    Enabled="false" MaxLength="16"></asp:TextBox>
                                                <div class="clear">
                                                    <div class="clear">
                                                    </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblPriceAdj" Text="<%$ resources:PriceAdj%>" AssociatedControlID="txtPriceAdj"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtPriceAdj" runat="server" CssClass="input-w80 numeric" onchange="CalculateTotal(this);"
                                                    MaxLength="16" TabIndex="25"></asp:TextBox>
                                                <div class="starwrap">
                                                    <cc1:AmountValidation ID="vamPriceAdj" runat="server" ControlToValidate="txtPriceAdj"
                                                        ErrorMessage="<%$ resources:Err_Valid_PriceAdj %>" NumberDigits="12" AllowNegative="true"
                                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"></cc1:AmountValidation>
                                                </div>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblTotal" Text="<%$ resources:Total%>" AssociatedControlID="txtHdrTotal"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtHdrTotal" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    Enabled="false" MaxLength="16"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblRefundAmount" Text="<%$ resources:RefundAmount%>"
                                                    AssociatedControlID="txtRefundAmount"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:TextBox ID="txtRefundAmount" runat="server" Text="0.00" CssClass="input-w80 numeric"
                                                    Enabled="true" MaxLength="16"></asp:TextBox>
                                                <div class="starwrap">
                                                    <cc1:AmountValidation ID="vmRefundAmount" runat="server" ControlToValidate="txtRefundAmount"
                                                        ErrorMessage="<%$ resources:Err_Valid_Refund %>" NumberDigits="12" AllowNegative="true"
                                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"></cc1:AmountValidation>
                                                </div>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="divcol-S">
                                        <asp:Label ID="lblFileUpload" runat="server" Text="AttachFile" AssociatedControlID="fupUpload"></asp:Label>
                                        <asp:FileUpload ID="fupUpload" runat="server" TabIndex="26" Style="width: 15.6%;" />
                                        <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupUpload"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_File_Upload %>">
                                                        
                                        </asp:RequiredFieldValidator>
                                        <a id="anchorFile" runat="server" target="_blank" tabindex="11"></a>
                                        <asp:Button runat="server" ID="btnUpload" CommandName="ADDITEMUPLOAD" TabIndex="26"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('upload')"
                                            ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="PageAction_Entry" ValidationGroup="upload"
                                            Text="<%$resources:ErpRes,Add %>" SkinID="btnInner-add" />
                                        <%--  <div class="btnwrap-divcol">
                                            
                                        </div>--%>
                                        <div class="clear">
                                        </div>
                                    </div>
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdUploads" Width="100%" PageSize="<%$ resources:PageSize%>"
                                            AllowSorting="false" AllowPaging="false" OnSorting="ActionHandler" OnPageIndexChanging="ActionHandler"
                                            OnRowDataBound="ActionHandler" AutoGenerateColumns="false" TabIndex="27" EmptyDataRowStyle-CssClass="emptytable">
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
                                                        <asp:Button ID="lnkEdit" runat="server" OnClick="ActionHandler" CommandName="EDITITEMUPLOAD"
                                                            TabIndex="27" SkinID="edit-icon" ToolTip="Edit" CommandArgument="PageAction_Entry"
                                                            OnLoad="btnAction_Load" OnPreRender="btnAction_PreRender" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="3%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEMUPLOAD"
                                                            TabIndex="27" SkinID="delete-icon" ToolTip="Delete" CommandArgument="PageAction_Entry"
                                                            OnClientClick="return ShowDeleteConfirm(this);" OnLoad="btnAction_Load" OnPreRender="btnAction_PreRender" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="3%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <div id="divItemTax" style="display: none">
                                <div class="Button-container-popup">
                                    <asp:Button ID="btnApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply" OnClick="ActionHandler"
                                        CommandArgument="PageAction_Entry" CommandName="TAXAPPLY" TabIndex="29" />
                                </div>
                                <div class="content-wrapper">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:HiddenField ID="hdfTaxFormula" runat="server" />
                                                    <asp:Label ID="lblPopupItemAmount" runat="server" Text="Item Amount" AssociatedControlID="txtPopupItemAmount"></asp:Label>
                                                    <asp:TextBox ID="txtPopupItemAmount" CssClass="input-w70 numeric" runat="server"
                                                        EnableViewState="false" Enabled="false" MaxLength="11"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblPopupAmount" runat="server" Text="Amount" AssociatedControlID="txtPopupAmount"></asp:Label>
                                                    <asp:TextBox ID="txtPopupAmount" TabIndex="25" runat="server" CssClass="input-w70 numeric"
                                                        EnableViewState="false" MaxLength="15"></asp:TextBox><%--Enabled="false"--%>
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfTaxAmt" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="tax" EnableClientScript="true" runat="server" ControlToValidate="txtPopupAmount"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                        </asp:RequiredFieldValidator>
                                                        <cc1:AmountValidation ID="vamTaxAmt" runat="server" ControlToValidate="txtPopupAmount"
                                                            ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="tax"></cc1:AmountValidation>
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
                                                        <asp:HiddenField ID="hdfTaxSplitPK" runat="server" Value='<%#Eval("VTL_PK") %>' />
                                                        <asp:HiddenField ID="hdfTaxPK" runat="server" Value='<%#Eval("VTL_TAX") %>' />
                                                        <asp:Label ID="lblTaxText" runat="server" Text='<%# Convert.ToString(Eval("VTL_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("VTL_TAX_TEXT")) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("VTL_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("VTL_TAX_TEXT"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxName" runat="server" Text='<%#Eval("VTL_NAME") %>' ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("VTL_NAME"))) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTaxName" runat="server" Value='<%#Eval("VTL_NAME") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrencyWithSeperation(Eval("VTL_TAX_AMT")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithSeperation(Eval("VTL_TAX_AMT")) %>'></asp:Label>
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
                            <div id="divExpenseAdvances" style="display: none">
                                <div class="Button-container-popup">
                                    <asp:Button ID="btnExpenseApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply"
                                        OnClick="ActionHandler" CommandArgument="PageAction_Entry" CommandName="EXPENSEADVAPPLY"
                                        TabIndex="29" />
                                </div>
                                <div class="content-wrapper">
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdExpenseAdv" Width="100%" AllowSorting="false"
                                            OnRowDataBound="ActionHandler" AutoGenerateColumns="false" TabIndex="22" EmptyDataRowStyle-CssClass="emptytable">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:VoucherNo %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfVoucherHdr" runat="server" Value='<%#Eval("IED_VOUCHER_HDR") %>' />
                                                        <asp:HiddenField ID="hdfIedPK" runat="server" Value='<%#Eval("IED_PK") %>' />
                                                        <asp:Label ID="lblVoucherNo" runat="server" Text='<%#Eval("FTH_VOUCHER_NO") %>' ToolTip='<%#Eval("FTH_VOUCHER_NO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate" runat="server" Text='<%#Eval("FTH_DATE", "{0:dd/MMM/yyyy}") %>'
                                                            ToolTip='<%#Eval("FTH_DATE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:RefNo %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRefNo" runat="server" Text='<%#Eval("FTH_REF_NO") %>' ToolTip='<%#Eval("FTH_REF_NO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:RefDate %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRefDate" runat="server" Text='<%#Eval("FTH_REF_DATE", "{0:dd/MMM/yyyy}") %>'
                                                            ToolTip='<%#Eval("FTH_REF_DATE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Naration %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNaration" runat="server" Text='<%#Eval("FTH_NARRATION") %>' ToolTip='<%#Eval("FTH_NARRATION") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="31%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Amount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmount" runat="server" Text='<%#GetFormattedCurrencyWithSeperation(Eval("IED_AMOUNT")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithSeperation(Eval("IED_AMOUNT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Select %>">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Eval("FTH_EXP_ADV_FLAG").ToString() == "1" ? true : false %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
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
                    <asp:ValidationSummary ID="vsDetails" ValidationGroup="details" runat="server" />
                    <asp:ValidationSummary ID="vsTax" ValidationGroup="tax" runat="server" />
                    <asp:ValidationSummary ID="vsTaxDate" ValidationGroup="taxDate" runat="server" />
                    <asp:ValidationSummary ID="vsHdrTaxDate" ValidationGroup="taxHdrDate" runat="server" />
                    <asp:ValidationSummary ID="vsDeduction" ValidationGroup="deduction" runat="server" />
                    <asp:ValidationSummary ID="vsUpload" ValidationGroup="upload" runat="server" />
                    <asp:HiddenField ID="hdfAppType" runat="server" />
                    <asp:HiddenField ID="hdfIsNewParty" runat="server" Value="1" />
                    <asp:HiddenField ID="hdfAppSubType" runat="server" />
                    <asp:HiddenField runat="server" ID="hdfApplyTax" Value="0" />
                    <asp:HiddenField runat="server" ID="hdfApplyHdrTax" Value="0" />
                </div>
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
            <div style="display: none">
                <asp:Button ID="btnSaveContinue" runat="server" OnClick="ActionHandler" CommandName="SAVE"
                    Style="display: none" />
                <asp:Button ID="btnSubmitContinue" runat="server" OnClick="ActionHandler" CommandName="WRKFSUBMIT"
                    Style="display: none" />
            </div>
            <asp:HiddenField ID="hdfPartyNoCheck" runat="server" Value="0" />
            <asp:HiddenField ID="hdfJournalizeWorkFlow" Value="0" runat="server" />
            <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
            <asp:HiddenField ID="hdfCurrencyGroup2" Value="2" runat="server" />
            <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" />
            <asp:HiddenField ID="hdfIsShowAlert" Value="0" runat="server" />
            <asp:HiddenField ID="hdfInvDueDateDependsVenInvDate" runat="server" Value="0" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
