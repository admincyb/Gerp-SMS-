<%@ Page Title="<%$ Resources:Captions,Title_PayrollProcess %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="PayrollProcess.aspx.cs" Inherits="HRMS.Payroll.PayrollProcess"
    ValidateRequest="false" Theme="ClassicExt" MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/ERPSMS_2.Master" %>
<%@ Register Src="~/Journalize/UserControls/JournalizeControlNew.ascx" TagName="Journalize"
    TagPrefix="uc4" %>
<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc3" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Src="UserControls/PayrollTabControl.ascx" TagName="PayrollTabControl"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <script src="../Scripts/Payroll/PayrollProcess.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtProcessDate");
            GrandScriptUtils.AddDateRangeCommon("txtProcessFromDate", "hdfProcessFromDate", "txtProcessToDate", "hdfProcessToDate", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtDepartment", url, "hdfDepartment", true, true, "DEPARTMENT");
          //  GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee ", url + "?EmpCategory=2&EmpBranch=" + $("[id$=ddlBranchLocation]").val() + "&EmpType=" + $("[id$=ddlEmployeeType]").val() + "&EmploymentType=" + $("[id$=ddlEmploymentType]").val() + "&EmpCompany=" + $("[id$=ddlCompany]").val() + "&EmpDept=" + $("[id$=hdfDepartment]").val() + "&FromDate=" + $("[id$=txtProcessFromDate]").val() + "&ToDate=" + $("[id$=txtProcessToDate]").val() + "&EmpPayrollType=" + $("[id$=ddlPayrollType]").val() + "&EmpCurrency=" + $("[id$=hdfCurrency]").val(), "hdfEmployee", true, true, "EMPLOYEEPAYROLLAUTOCOMPLETE");
            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee ", url + "?EmpCategory=2&EmpCompany=" + $("[id$=ddlCompany]").val() + "&FromDate=" + $("[id$=txtProcessFromDate]").val() + "&ToDate=" + $("[id$=txtProcessToDate]").val() + "&EmpPayrollType=" + $("[id$=ddlPayrollType]").val() + "&EmpCurrency=" + $("[id$=hdfCurrency]").val(), "hdfEmployee", true, true, "EMPLOYEEPAYROLLAUTOCOMPLETE");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCurrency", url, "hdfCurrency", true, true, "CURRENCY");
            //For Voucher
           // InitEmployeeAuto();
            GrandScriptUtils.DatePickerCommon("txtPVDate");

            $("[id*=txtEarnEligibleAmount]").ForceNumericOnly();
            $("[id*=txtDeductEligibleAmount]").ForceNumericOnly();
            $("[id*=txtWorkDays]").ForceNumericOnly();
            $("[id*=txtEmpWorkDays]").ForceNumericOnly();
            $("[id*=txtExchangeRate]").ForceNumericOnly();

            //            if ($("[id$=hdfFilter]").val() == "1") {
            //                ShowHideFilter(1);
            //            }
            //            else {
            //                ShowHideFilter(0);
            //            }   

            if ($("[id$=txtExchangeRate]").attr("disabled") == true) {
                $("[id$=txtExchangeRate]").addClass("input-disabled");
            }
            else {
                $("[id$=txtExchangeRate]").removeClass("input-disabled");
            }

            if ($("[id$=txtCurrency]").attr("disabled") == true) {
                DisableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
            }
            else {
                EnableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
            }

            $("[id*=grdPeriodList] td").hover(function () {
                $("td", $(this).closest("tr")).addClass("hover_row");
            }, function () {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });

            //Set a stamp for cancelled invoice
            if ($("[id$=hdfIsCancelled]").val() == "1") {
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
                $("[id$='pnlCancelSubmit']").hide();
                $("[id$='pnlProcess']").hide();
                $("[id$='pnlReprocess']").hide();
                $("[id$='pnlPost']").hide();
            }
            else
                $("[id$=tblDetailHdr]").addClass("table-devide");

            //End

            SetPayrollBtnVisibility();
            Popup();
        }
        function AfterMessageClose() {
            $("[id$=btnRebindPayroll]").click();
        }

        /// Used to disable Autocomplete
        function DisableAuto(extender, hfield) {
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
            $(extender).autocomplete("option", "disabled", true);
            $(extender).attr("disabled", true);
        }

        /// Used to disable Autocomplete
        function EnableAuto(extender, hfield) {
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect-disable").addClass("ddlSelect");
            $(extender).autocomplete("option", "disabled", false);
            $(extender).attr("disabled", false);
        }

        function AfterDateSelect(controlID) {
            if (controlID == "txtProcessFromDate") {
                if ($("[id$=ddlProcessMode]").val() == $("[id$=hdfMonthlyMode]").val()) {
                    var fromDate = $.datepicker.parseDate("dd-M-yy", $('input[id$=txtProcessFromDate]').val());
                    fromDate = new Date(fromDate.getFullYear(), fromDate.getMonth() + 1, fromDate.getDate() - 1);
                    var toDateText = $.datepicker.formatDate("dd-M-yy", fromDate);
                    var toDate = $.datepicker.formatDate("dd-M-yy", fromDate);
                    $("[id$=txtProcessToDate]").val(toDateText);
                    $("[id$=hdfProcessToDate]").val(toDate);
                }
                else {
                    $("[id$=txtProcessToDate]").val('');
                }
                SetPayrollCaption();
            }
            else if (controlID == "txtProcessToDate") {
                SetPayrollCaption();
            }
            else if (controlID == "txtProcessDate") {
                $("[id$=btnCurrency]").click();
            }
            InitEmployeeAuto();
        }

        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtDepartment") {
                InitEmployeeAuto();
            }
            else if (targetControlID == "txtCurrency") {
                $("[id$=btnCurrency]").click();
            }
        }

        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtDepartment") {
                InitEmployeeAuto();
            }
        }

        function AfterClose(containerID) {
            if (containerID == "[id$=divJournalize]") {
                $("[id$=btnJournalizeUpdate]").click();
            }
            else if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
                if ($("[id$=hdfJournalizeWorkFlow]").val() == "1") {
                    ShowCommonCotainerDiv('[id$=divJournalize]', '<%= GetLocalResourceObject("Payroll_Journal") %>', "1%");
                    AfterCloseWkfInJournal();
                }
            }
        }

        function InitEmployeeAuto() {
            // some time show daily emp in monthly  
            // GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee ", url + "?EmpCategory=2&EmpBranch=" + $("[id$=ddlBranchLocation]").val() + "&EmpType=" + $("[id$=ddlEmployeeType]").val() + "&EmploymentType=" + $("[id$=ddlEmploymentType]").val() + "&EmpCompany=" + $("[id$=ddlCompany]").val() + "&EmpDept=" + $("[id$=hdfDepartment]").val(), "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETE");
            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee ", url + "?EmpCategory=2&EmpBranch=" + $("[id$=ddlBranchLocation]").val() + "&EmpType=" + $("[id$=ddlEmployeeType]").val() + "&EmploymentType=" + $("[id$=ddlEmploymentType]").val() + "&EmpCompany=" + $("[id$=ddlCompany]").val() + "&EmpDept=" + $("[id$=hdfDepartment]").val() + "&FromDate=" + $("[id$=txtProcessFromDate]").val() + "&ToDate=" + $("[id$=txtProcessToDate]").val() + "&EmpPayrollType=" + $("[id$=ddlPayrollType]").val() + "&EmpCurrency=" + $("[id$=hdfCurrency]").val(), "hdfEmployee", true, true, "EMPLOYEEPAYROLLAUTOCOMPLETE");
            ResetEmployee();
        }

        function ResetEmployee() {
            var defText = '<%= Resources.ErpRes.AutoDefaultValue %>';
            $("[id$=txtEmployee]").val(defText);
            $("[id$=hdfEmployee]").val('-1');
        }

        function ShowListing(flag) {
            if (flag) {
                $("[id$='PageAction_List']").show();
                $("[id$='PageAction_Entry']").hide();
                $("[id$='pnlListing']").show();
                $("[id$='pnlEntry']").hide();
            }
            else {
                $("[id$='PageAction_List']").hide();
                $("[id$='PageAction_Entry']").show();
                $("[id$='pnlListing']").hide();
                $("[id$='pnlEntry']").show();
            }
            return false;
        }

        function ViewMode(mode) {
            ///<summary>
            /// Used to handle the view Mode
            ///</summary>
            /// <param name="mode" optional="true" type="String">
            /// Mode = 1 Determins ites on View Mode
            /// Mode = 2 Indicates its on New Mode
            /// </param>         
            if (mode == 1) {
                $("[id$='pnlSave']").hide();

            }
            else if (mode == 2) {
            }
        }

        var validationArrayGroup;
        function CheckValidationDuplicate(valGroup) {
            validationArrayGroup = new Array();
            for (var i = Page_Validators.length - 1; i >= 0; i--) {
                if (typeof (Page_Validators[i].validationGroup) == "string") {
                    if (valGroup == Page_Validators[i].validationGroup) {
                        if (!CheckValidationExists(Page_Validators[i].id)) {
                            validationArrayGroup.push(Page_Validators[i].id);
                        }
                        else {
                            Page_Validators.splice(i, 1);
                        }
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
        function ValidatePayrollPage(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }

        function ShowHideFilter(flag) {
            if (flag == 1) {
                $("[id$=divFiterDetail").show();
                $("[id$=imbShowFilter").hide();
                $("[id$=imbHideFilter").show();
            }
            else {
                $("[id$=divFiterDetail").hide();
                $("[id$=imbShowFilter").show();
                $("[id$=imbHideFilter").hide();
                $("[id$=hdfFilter]").val("0");
            }
            return false;
        }

        function ShowDeleteConfirmationMsg(btn, message) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = message ? message : '<%= Resources.ErpRes.MsgDeleteConfirm %>';
            $("#popupHolder").html("");
            //To set fit to screen
            $('html, body').animate({ scrollTop: '0px' }, 0);
            $('html, body').css('overflow', 'hidden');
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $('#divmodel').hide();
                        $(this).dialog("close");
                        __doPostBack(btn.name, '');
                    },
                    Cancel: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $('#divmodel').hide();
                        $(this).dialog("close");
                        if (typeof AfterDeleteConfirmationCancel == "function") {
                            AfterDeleteConfirmationCancel(btn.id);
                        }
                        return false;
                    }
                }
            });
            return false;
        }

        function CalculateTotalEarnings(ctrl) {
            var TotalAmount = 0;
            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }
            $("#[id*=grdEarnPayDetails] input[type=text][id*=txtEarnEligibleAmount]").each(function (index) {
                var amount = 0;
                var pelInSalary = 0;
                pelInSalary = $(this).closest('tr').find("#[id*=hdfPayElmtInSalary]").val();
                if (!isNaN(pelInSalary) && pelInSalary == 1) {//Calculate Total if the Pay Element included in Salary               
                    //Check if number is not empty
                    if ($.trim($(this).val()) != "") {
                        //Check if number is a valid integer
                        if (!isNaN(parseFloat($(this).val()))) {
                            amount = parseFloat($(this).val());
                            TotalAmount = TotalAmount + amount;
                        }
                    }
                }
            });
            $("#[id*=grdEarnPayDetails] [id*=lblTotalEarnPay]").html(TotalAmount.toFixed(DecimalDigits));
            //ctrl.Focus();
        }

        function CalculateTotalDeductions(ctrl) {
            var TotalAmount = 0;
            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }
            $("#[id*=grdDeductPayDetails] input[type=text][id*=txtDeductEligibleAmount]").each(function (index) {
                var amount = 0;
                var pelInSalary = 0;
                pelInSalary = $(this).closest('tr').find("#[id*=hdfPayElmtInSalary]").val();
                if (!isNaN(pelInSalary) && pelInSalary == 1) {//Calculate Total if the Pay Element included in Salary   
                    //Check if number is not empty
                    if ($.trim($(this).val()) != "") {
                        //Check if number is a valid integer
                        if (!isNaN(parseFloat($(this).val()))) {
                            amount = parseFloat($(this).val());
                            TotalAmount = TotalAmount + amount;
                        }
                    }
                }
            });
            $("#[id*=grdDeductPayDetails] [id*=lblTotalDeductPay]").html(TotalAmount.toFixed(DecimalDigits));
            //ctrl.Focus();
        }

        //Validate Work days
        function CheckTotalProcessDays(sender, args) {
            var wrkdays = $(sender).closest('tr').find('[id*=txtWorkDays]').val();
            var totDays = parseFloat($("[id$=hdfTotalProcessDays]").val());
            if (parseFloat(wrkdays) > totDays) {
                args.IsValid = false;
            }
            else {
                args.IsValid = true;
            }
        }

        function CheckEmpWorkDays(sender, args) {
            var wrkdays = parseFloat($("[id$=txtEmpWorkDays]").val());
            var totDays = parseFloat($("[id$=hdfTotalProcessDays]").val());
            if (parseFloat(wrkdays) > parseFloat(totDays)) {
                args.IsValid = false;
            }
            else {
                args.IsValid = true;
            }
        }

        function ShowHidePayrollHistory(flag) {
            ///<summary>
            /// Used to Show/Hide Payroll History Div
            ///</summary>

            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divPayrollHistory]").show();
                $("[id$=imbShowHistory]").hide();
                $("[id$=imbHideHistory]").show();
            }
            else {
                $("[id$=divPayrollHistory]").hide();
                $("[id$=imbShowHistory]").show();
                $("[id$=imbHideHistory]").hide();
            }
            return false;
        }

        function SetPayrollCaption() {
            var caption = '<%= GetLocalResourceObject("PayrollCaption").ToString() %>';
            caption = String.format(caption, $("[id$=txtProcessFromDate]").val(), $("[id$=txtProcessToDate]").val());
            $("[id$=txtCaption]").val(caption);
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

        function SetFilterCompany() {
            $("#<%=ddlCompany.ClientID%>").val($("[id$=ddlCompanyHdr]").val());
            InitEmployeeAuto();
        }


        function SetPayrollBtnVisibility() {
            $("#[id*=grdEmpPayrollList] input[type=hidden][id*=hdfItemPK]").each(function (index) {
                if ($("[id$=hdfIsCancelled]").val() == "1") {
                    $(this).closest('tr').find("#[id*=imbEditPayroll]").hide();
                    $(this).closest('tr').find("#[id*=imbDeletePayroll]").hide();
                }
                else if (!isNaN(parseInt($(this).val()))) {
                    var itemPk = parseInt($(this).val());
                    var paymentPk = $(this).closest('tr').find("#[id*=hdfPaymentPk]").val();
                    if (itemPk == 0 || (!isNaN(paymentPk) && parseInt(paymentPk) > 0) || $("[id$=hdfIsPosted]").val() == "1") {
                        $(this).closest('tr').find("#[id*=imbEditPayroll]").hide();
                        $(this).closest('tr').find("#[id*=imbDeletePayroll]").hide();
                    }
                }
            });
            if ($("[id$=hdfIsCancelled]").val() == "1" || $("[id$=hdfIsPosted]").val() == "1") {
                $("#[id*=grdEmpPayrollList] [id*=imbDeleteAll]").hide();
            }

        }

        function ChangeHeight(height) {
            $("#divPeriodSearch").css("height", height + "px");
        }
        function SetToDate() {
            var fromdate = $("[id$=txtMonthFrom]").val();
            $("[id$=txtMonthTo]").val(fromdate);
        }

        var objSender;
        function showSettingsPopup(sender, targetId) {
            objSender = sender;
            var title = '<%= GetLocalResourceObject("EmpSettings").ToString() %>';
            var hdfEmpPK = $(sender).closest('tr').find("#[id*=hdfEmpPK]").val();
            $("[id$=hdfSettingsEmpPK]").val(hdfEmpPK);
            $("[id$=hdfSender]").val(targetId);
            var lblEmployeeText = $(sender).closest('tr').find("#[id*=lblEmployeeText]").html();
            $("[id$=lblEmpName]").html(GrandScriptUtils.GetShortString(lblEmployeeText, 40));
            $("[id$=lblEmpName]").attr('title', lblEmployeeText);
            var hdfEmpDOJ = $(sender).closest('tr').find("#[id*=hdfEmpDOJ]").val();
            if (hdfEmpDOJ != "") {
                var joindate = new Date(hdfEmpDOJ);
                var formatedJoinDate = $.datepicker.formatDate("dd-M-yy", joindate);
                $("[id$=lblEmpDOJ]").html(formatedJoinDate);
                $("[id$=lblEmpDOJ]").attr('title', formatedJoinDate);
            }
            var hdfWorkDays = $(sender).closest('tr').find("#[id*=hdfWorkDays]").val();
            if (hdfWorkDays != "") {
                $("[id$=txtEmpWorkDays]").val(hdfWorkDays);
            }
            else
                $("[id$=txtEmpWorkDays]").val(0);

            var processToDate = $("[id$=txtProcessToDate]").val();
            var processFromDate = $("[id$=txtProcessFromDate]").val();
            var prcToDate = $.datepicker.parseDate("dd-M-yy", processToDate);
            var prcFromDate = $.datepicker.parseDate("dd-M-yy", processFromDate);
            var totalDays = daydiff(new Date(prcFromDate), new Date(prcToDate)) + 1;
            $("[id$=hdfTotalProcessDays]").val(totalDays);
            //ShowContainerDiv('[id$=divPopupEmpSettings]', title, '550', '130');
            $("#divPopupEmpSettings").dialog("open");
            $("#divPopupEmpSettings").dialog(
            {
                width: 550,
                title: title
            });
            return false;
        }

        function SettingsApply() {
            if (ValidatePayrollPage('Settings')) {
                var workDays = 0;
                var senderid = $("[id$=hdfSender]").val();
                var objempworkdays = $("[id$=txtEmpWorkDays]");
                if (objempworkdays.length > 1) {
                    // workDays = objempworkdays.eq(objempworkdays.length - 1).val();
                    workDays = objempworkdays.eq(0).val();
                }
                else
                    workDays = objempworkdays.val();
                $("[id$=" + senderid + "]").val(workDays);
                //                $('[id$=divPopupEmpSettings]').dialog('distroy');
                //                $("#popupHolder").html("");               
                //                ClosePopup();               
                SetEmpRowColor(senderid, workDays);
                $("#divPopupEmpSettings").dialog("close");
            }
            return false;
        }

        //For getting total days between two dates
        function daydiff(firstDate, secondDate) {
            return Math.round((secondDate - firstDate) / (1000 * 60 * 60 * 24));
        }

        //For Setting  grid Colour of employee list
        function SetEmpRowColor(sender, workdays) {
            var selectedRowColor;
            if (workdays == "0" || workdays == "") {
                selectedRowColor = '<%= GetLocalResourceObject("NoWorkDaysRowColor").ToString() %>';
                $(objSender).closest('tr').css('background-color', selectedRowColor);

            }
            else {
                selectedRowColor = '<%= GetLocalResourceObject("RowColor").ToString() %>';
                $(objSender).closest('tr').css('background-color', selectedRowColor);
            }
        }

        function Popup() {
            $("#divPopupEmpSettings").dialog({
                autoOpen: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#popupHolder");
                }
            });
        }
    </script>
    <script type="text/javascript">
        $(window).load(function EndRequest() {
            FormatCalendar('4');
        });
        var ControlID = 'calendar1|calendar2|calendar3';
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upPayrollProcess" runat="server">
        <ContentTemplate>
            <uc2:PayrollTabControl ID="PayrollTabControl1" runat="server" CurrentTab="2" />
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVEEMPLOYEEPAYROLL" Text="<%$resources:Controls,Save %>"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save"
                                            ValidationGroup="Pay" OnClick="ActionHandler" OnClientClick="javascript:ValidatePayrollPage('Pay')"
                                            TabIndex="150" />
                                    </li>
                                    <li runat="server" id="pnlPrint">
                                        <asp:Button runat="server" TabIndex="151" ID="btnPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:PaySlip %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:PaySlip %>" />
                                    </li>
                                    <li style="display: none">
                                        <asp:Button runat="server" TabIndex="152" ID="btnIncomeTax" CommandName="PRINTINCOMETAX"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,IT %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,IncomeTax %>" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            CssClass="popupclose" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" OnClick="ActionHandler"
                                            TabIndex="152" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="154" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="147"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="149" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePayrollPage('Process')"
                                            ValidationGroup="Process" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlReprocess">
                                        <asp:Button runat="server" TabIndex="153" ID="btnReProcessPayroll" CommandName="REPROCESSPAYROLL"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,ReProcessPayroll %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-ReProcess" ToolTip="<%$resources:Controls,ReProcessPayroll %>"
                                            ValidationGroup="ReProcess" OnClientClick="javascript:ValidatePayrollPage('Process');" />
                                    </li>
                                    <li runat="server" id="pnlProcess">
                                        <asp:Button runat="server" TabIndex="152" ID="btnProcessPayroll" CommandName="PROCESSPAYROLL"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,ProcessPayroll %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-payroll" ToolTip="<%$resources:Controls,ProcessPayroll %>" ValidationGroup="ReProcess"
                                            OnClientClick="javascript:ValidatePayrollPage('Process');" />
                                    </li>
                                    <li runat="server" id="pnlPrintAll">
                                        <asp:Button runat="server" TabIndex="153" ID="btnPrintAll" CommandName="PRINTALL"
                                            OnClick="ActionHandler" Text="<%$resources:ConfigurationsRes,PayrollOldPrint  %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print" ToolTip="<%$resources:ConfigurationsRes,PayrollOldPrint %>" />
                                    </li>
                                    <li runat="server" id="pnlPrintAllNew">
                                        <asp:Button runat="server" TabIndex="153" ID="btnPrintAllNew" CommandName="PRINTPAYROLL"
                                            OnClick="ActionHandler" Text="<%$resources:Preview %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Preview %>" />
                                    </li>
                                    <li runat="server" id="pnlPrintMultiple">
                                        <asp:Button runat="server" TabIndex="153" ID="btnPrintMultiple" CommandName="PRINTMULTIPLE"
                                            OnClick="ActionHandler" Text="<%$resources:PaySlip %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:PaySlip %>" />
                                    </li>
                                    <li runat="server" id="pnlEXCELPRINT">
                                        <asp:Button runat="server" TabIndex="153" ID="btnExport" CommandName="EXCELPRINT"
                                            OnClick="ActionHandler" Text="<%$resources:Export  %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Excel" ToolTip="<%$resources:Export %>" />
                                    </li>
                                    <li runat="server" id="pnlPost">
                                        <asp:Button runat="server" ID="btnJournalize" CommandName="JOURNALIZE" TabIndex="154"
                                            Text="<%$resources:Journalize %>" OnClick="ActionHandler" ToolTip="<%$resources:Journalize %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-journalize" />
                                    </li>
                                    <%--<li>
                                        <asp:Button runat="server" TabIndex="1" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" OnClientClick="javascript:return SelectedCheckBoxCount(1);"/>
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="1" ID="btnView" CommandName="VIEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,View %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" OnClientClick="javascript:return SelectedCheckBoxCount(1);"/>
                                    </li>--%>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <div class="tab-container-floating">
                    <%--Container for List and Detail tabs--%>
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="DETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkIncomeTax" Text="<%$resources:IncomeTax %>"
                                Visible="false" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="INCOMETAX"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblPage" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="clear">
                            </div>
                            <table class="table-devide tablelayout" id="tblDetailHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblTrxNoHdr" runat="server" Text="<%$ resources:TrxNo%>" AssociatedControlID="lblTrxNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblTrxNo" CssClass="input-small"></asp:Label>
                                            <asp:Label ID="lblProcessDate" runat="server" Text="<%$ resources:ProcessDate%>"
                                                AssociatedControlID="txtProcessDate" CssClass="middle-lbl-d"></asp:Label>
                                            <asp:TextBox ID="txtProcessDate" runat="server" CssClass="input-small" TabIndex="1"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvProcessDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Process" EnableClientScript="true" runat="server" ControlToValidate="txtProcessDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ProcessDate %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblProcessMode" runat="server" Text="<%$ resources:ProcessMode%>"
                                                AssociatedControlID="ddlProcessMode"></asp:Label>
                                            <asp:DropDownList ID="ddlProcessMode" runat="server" TabIndex="4" CssClass="select-small-a1"
                                                AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvProcessMode" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Process" EnableClientScript="true" InitialValue="" runat="server"
                                                ControlToValidate="ddlProcessMode" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ProcessMode %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvSrchProcessMode" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Search" EnableClientScript="true" InitialValue="" runat="server"
                                                ControlToValidate="ddlProcessMode" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ProcessMode %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label ID="lblPayrollType" runat="server" Text="<%$ resources:PayrollType%>"
                                                AssociatedControlID="ddlPayrollType" CssClass="middle-lbl-d"></asp:Label>
                                            <asp:DropDownList ID="ddlPayrollType" runat="server" TabIndex="5" CssClass="lbl-19-3perc"
                                                AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvPayrollType" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Process" EnableClientScript="true" InitialValue="-1" runat="server"
                                                ControlToValidate="ddlPayrollType" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PayrollType %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvSrchPayrollType" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Search" EnableClientScript="true" InitialValue="-1" runat="server"
                                                ControlToValidate="ddlPayrollType" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PayrollType %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvPayrollTypeNonPayroll" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="NonPayroll" EnableClientScript="true" InitialValue="-1" runat="server"
                                                ControlToValidate="ddlPayrollType" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_PayrollType %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblhdrCurrency" Text="<%$ resources:CurrencyReq%>"
                                                AssociatedControlID="txtCurrency"></asp:Label>
                                            <asp:TextBox ID="txtCurrency" runat="server" CssClass="input-small" TabIndex="9"
                                                MaxLength="100" Enabled="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvCurrency" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Process" EnableClientScript="true" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>"
                                                runat="server" ControlToValidate="txtCurrency" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_Currency %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvCurrencySearch" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Search" EnableClientScript="true" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>"
                                                runat="server" ControlToValidate="txtCurrency" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_Currency %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfCurrency" runat="server" />
                                            <asp:Button ID="btnCurrency" runat="server" OnClick="ActionHandler" CommandName="CURRENCYSELECTED"
                                                Style="display: none" EnableTheming="false" />
                                            <asp:Button ID="btnRebindPayroll" runat="server" OnClick="ActionHandler" CommandName="REBIND"
                                                Style="display: none" EnableTheming="false" />
                                            <asp:Label runat="server" ID="lblExchangeRate" Text="<%$ resources:ExchangeRateReq%>"
                                                AssociatedControlID="txtExchangeRate" CssClass="lbl-21-5perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtExchangeRate" Text="" TabIndex="9" CssClass="input-small numeric medium"
                                                onkeypress="return validateRateFloatKeyPress(this,event);"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfExchangeRate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Process" EnableClientScript="true" runat="server" ControlToValidate="txtExchangeRate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExchangeRate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cmpExchangeRate" CssClass="star" SetFocusOnError="true"
                                                Type="Double" Operator="GreaterThan" ValueToCompare="0" ValidationGroup="Process"
                                                EnableClientScript="true" InitialValue="0" runat="server" ControlToValidate="txtExchangeRate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExchangeRate %>">
                                            </asp:CompareValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblSalMonth" runat="server" Text="<%$ resources:SalaryMonthReq%>"
                                                AssociatedControlID="txtSalaryMonth" CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox ID="txtSalaryMonth" runat="server" CssClass="input-w10per" TabIndex="2"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" AutoPostBack="true"
                                                OnTextChanged="ActionHandler"></asp:TextBox>
                                            <cc2:CalendarExtender runat="server" ID="txtSalaryMonth_CalendarExtender" BehaviorID="calendar1"
                                                TargetControlID="txtSalaryMonth" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                                                ClientIDMode="Static" OnClientHidden="onCalendarHidden">
                                            </cc2:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="vrfSalMonth" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Process" EnableClientScript="true" runat="server" ControlToValidate="txtSalaryMonth"
                                                Display="Static" Text="*" ErrorMessage="<%$ resources:Err_SalaryMonth %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label ID="lblCaption" runat="server" Text="<%$ resources:Caption%>" AssociatedControlID="txtCaption"
                                                CssClass="middle-lbl-xsmall-e"></asp:Label>
                                            <asp:TextBox ID="txtCaption" runat="server" CssClass="input-w41-5per" TabIndex="3"
                                                MaxLength="200"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvCaption" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Process" EnableClientScript="true" runat="server" ControlToValidate="txtCaption"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Caption %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblProcessFromDate" Text="<%$ resources:ProcessFromDate%>"
                                                AssociatedControlID="txtProcessFromDate" CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtProcessFromDate" CssClass="input-small" TabIndex="6"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvProcessFromDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Process" EnableClientScript="true" runat="server" ControlToValidate="txtProcessFromDate"
                                                Display="Static" Text="*" ErrorMessage="<%$ resources:Err_ProcessFromDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvSrchProcessFrom" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Search" EnableClientScript="true" runat="server" ControlToValidate="txtProcessFromDate"
                                                Display="Static" Text="*" ErrorMessage="<%$ resources:Err_ProcessFromDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvPrFromWorkingDays" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="workingDays" EnableClientScript="true" runat="server" ControlToValidate="txtProcessFromDate"
                                                Display="Static" Text="*" ErrorMessage="<%$ resources:Err_ProcessFromDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvFromNonPayroll" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="NonPayroll" EnableClientScript="true" runat="server" ControlToValidate="txtProcessFromDate"
                                                Display="Static" Text="*" ErrorMessage="<%$ resources:Err_ProcessFromDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfProcessFromDate" runat="server" Value="" />
                                            <asp:Label runat="server" ID="lblProcessToDate" Text="<%$ resources:ProcessToDate%>"
                                                AssociatedControlID="txtProcessToDate" CssClass="lbl-19-2perc "></asp:Label>
                                            <asp:TextBox runat="server" ID="txtProcessToDate" CssClass="input-small" TabIndex="7"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfProcessToDate" runat="server" Value="" />
                                            <asp:RequiredFieldValidator ID="rfvProcessToDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Process" EnableClientScript="true" runat="server" ControlToValidate="txtProcessToDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ProcessToDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvSrchProcessToDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Search" EnableClientScript="true" runat="server" ControlToValidate="txtProcessToDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ProcessToDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfPrToWorkingDays" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="workingDays" EnableClientScript="true" runat="server" ControlToValidate="txtProcessToDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ProcessToDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfPrToNonPayroll" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="NonPayroll" EnableClientScript="true" runat="server" ControlToValidate="txtProcessToDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ProcessToDate %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="display-inline">
                                                <asp:ImageButton runat="server" ID="imbWorkingDays" SkinID="show-workingdays" ToolTip="<%$resources:WorkingDays %>"
                                                    OnClick="ActionHandler" CommandName="WORKINGDAYSPOPUP" CssClass="margntop1" ValidationGroup="workingDays"
                                                    OnClientClick="javascript:ValidatePayrollPage('workingDays')" TabIndex="8" Visible="false" />
                                                <asp:ImageButton SkinID="btnview" runat="server" ID="imbUnPayrollEmpPopUp" CommandName="DETAILS"
                                                    ValidationGroup="NonPayroll" OnClientClick="javascript:ValidatePayrollPage('NonPayroll')"
                                                    CssClass="btnInner-View margntop1" OnClick="ActionHandler" TabIndex="8" ToolTip="<%$ resources:NonPayrolllEmp%>" />
                                            </div>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblCompanyHdr" AssociatedControlID="ddlCompany" CssClass="middle-lbl"
                                                Text="<%$resources:Controls,CompanyReq%>"></asp:Label>
                                            <asp:DropDownList ID="ddlCompanyHdr" runat="server" TabIndex="9" CssClass="select-w66per"
                                                onchange="javascript:SetFilterCompany();">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvCompanyHdr" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlCompanyHdr" Display="Static" Text="*" InitialValue="-1"
                                                ValidationGroup="Process" ErrorMessage="<%$ resources:Err_Company %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div id="divNonPayrollDetails" style="display: none">
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            <div class="content-wrapper" runat="server" id="div1">
                                                <asp:Label ID="lblNonPayrollInfo" runat="server" Text="<%$ resources:NonPayrolllEmpInfo%>"></asp:Label>
                                                <%--<asp:Label ID="lblhdrEmployeeNoTxtPopup" runat="server" Text="emptext" Font-Bold="True"></asp:Label>--%>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="content-wrapper">
                                    <asp:GridView runat="server" ID="grdNonPayrollEmps" Width="100%" AutoGenerateColumns="false"
                                        Style="margin-top: -13px!important;" EmptyDataRowStyle-CssClass="emptytable">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:SlNo%>">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                                <ItemStyle Width="6%" HorizontalAlign="Center" Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Employee%>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPopItem" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EMP_EMPLOYEE_TEXT")),25) %>'
                                                        runat="server" ToolTip='<%# Eval("EMP_EMPLOYEE_TEXT") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="28%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:BranchLocation%>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPopBranch" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EMP_BRANCH_TEXT")),25) %>'
                                                        runat="server" ToolTip='<%# Eval("EMP_BRANCH_TEXT") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="false" />
                                                <ItemStyle Width="25%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:DepartmentHd%>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPopDept" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EMP_DEP_TEXT")),25) %>'
                                                        runat="server" ToolTip='<%# Eval("EMP_DEP_TEXT") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="false" />
                                                <ItemStyle Width="25%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:SalaryTemplate%>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPopTemplate" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EMP_SAL_TEMPLATE")),15) %>'
                                                        runat="server" ToolTip='<%# Eval("EMP_SAL_TEMPLATE") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="false" />
                                                <ItemStyle Width="15%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    <asp:Literal ID="Literal3" runat="server" Text="<%$ resources: EmployeeFilter %>" /></h1>
                                <%--  <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideFilter(1);"
                                    SkinID="imbArrowShow" ToolTip="<%$ resources:Controls,ShowDetails%>" />
                                <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideFilter();"
                                    Style="display: none" SkinID="imbArrowHide" ToolTip="<%$ resources:Controls,HideDetails%>" />--%>
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divFiterDetail">
                                <table class="table-devide tablelayout">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblCompany" AssociatedControlID="ddlCompany" Text="<%$resources:Company%>"></asp:Label>
                                                <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="9" CssClass="select-half-b"
                                                    onchange="javascript:InitEmployeeAuto();">
                                                </asp:DropDownList>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblEmployeeType" AssociatedControlID="ddlEmployeeType"
                                                    Text="<%$resources:EmployeeType%>" CssClass="middle-lbl"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlEmployeeType" CssClass="select-small-c" TabIndex="10"
                                                    onchange="javascript:InitEmployeeAuto();">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblDepartment" runat="server" Text="<%$ resources:Department%>" AssociatedControlID="txtDepartment"
                                                    CssClass="middle-lbl-xsmall-c"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDepartment" Text="" TabIndex="10" CssClass="select-small-d"></asp:TextBox>
                                                <asp:HiddenField ID="hdfDepartment" Value="" runat="server" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblBranchLocation" runat="server" Text="<%$ resources:BranchLocation%>"
                                                    AssociatedControlID="ddlBranchLocation"></asp:Label>
                                                <asp:DropDownList ID="ddlBranchLocation" runat="server" TabIndex="11" CssClass="select-w26-6per"
                                                    onchange="javascript:InitEmployeeAuto();">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblEmploymentType" runat="server" Text="<%$ resources:EmploymentType%>"
                                                    AssociatedControlID="ddlEmploymentType" CssClass="middle-lbl"></asp:Label>
                                                <asp:DropDownList ID="ddlEmploymentType" runat="server" TabIndex="12" CssClass="select-small-a"
                                                    onchange="javascript:InitEmployeeAuto();">
                                                </asp:DropDownList>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblEmployee" Text="<%$ resources:Employee%>" AssociatedControlID="txtEmployee"
                                                    CssClass="middle-lbl"></asp:Label>
                                                <%-- <asp:DropDownList ID="ddlEmployee" runat="server" TabIndex="4" CssClass="select-half-b margnbotm0">
                                            </asp:DropDownList>--%>
                                                <asp:TextBox ID="txtEmployee" runat="server" TabIndex="13" CssClass="input-w65-1per"
                                                    MaxLength="100"> </asp:TextBox>
                                                <asp:HiddenField ID="hdfEmployee" runat="server" />
                                                <div class="display-inline">
                                                    <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Search%>" ToolTip="<%$ resources:Search%>"
                                                        OnClick="ActionHandler" TabIndex="14" CommandName="SEARCH" SkinID="search-ext"
                                                        CssClass="margntop2" ValidationGroup="Search" OnClientClick="javascript:ValidatePayrollPage('Search')" />
                                                    <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Clear%>" ToolTip="<%$ resources:Clear%>"
                                                        TabIndex="15" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext"
                                                        CssClass="margntop2" />
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <div>
                                <div class="search-colapse-b">
                                    <table style="width: 100%">
                                        <%-- class="popup-header"--%>
                                        <tr>
                                            <td style="width: 2%">
                                            </td>
                                            <td style="width: 23%">
                                                <asp:Label runat="server" ID="lblPeriodH" Text="Period:" AssociatedControlID="lblPeriodHdr"></asp:Label>
                                                <asp:Label ID="lblPeriodHdr" runat="server" CssClass="input-full bold"></asp:Label>
                                            </td>
                                            <td style="width: 21%">
                                                <asp:Label runat="server" ID="lblProcessDateH" Text="Process Date:" AssociatedControlID="lblProcessDateHdr"></asp:Label>
                                                <asp:Label ID="lblProcessDateHdr" runat="server" CssClass="bold"></asp:Label>
                                            </td>
                                            <td style="width: 40%">
                                                <asp:Label runat="server" ID="lblCaptionH" Text="Caption:" AssociatedControlID="lblCaptionHdr"></asp:Label>
                                                <asp:Label ID="lblCaptionHdr" runat="server" CssClass="input-full bold"></asp:Label>
                                            </td>
                                            <%--<td style="width: 10%">
                                               <asp:Label runat="server" ID="lblProcs" Text="<%$ resources:ProcessCount%>" AssociatedControlID="lblProcessCount"></asp:Label>
                                                <asp:Label runat="server" ID="lblProcessCount" Text="" CssClass="bold"></asp:Label>
                                            </td>--%>
                                        </tr>
                                    </table>
                                </div>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 25%" class="grid-left">
                                            <div class="gridwrap margntop-minus3" style="overflow-x: hidden;">
                                                <asp:GridView runat="server" ID="grdPeriodList" Width="100%" AllowPaging="false"
                                                    AllowSorting="false" PageSize="<%$ resources:PageSize%>" AutoGenerateColumns="false"
                                                    EmptyDataRowStyle-CssClass="emptytable" EmptyDataRowStyle-HorizontalAlign="Center"
                                                    OnRowDataBound="ActionHandler">
                                                    <%--AutoGenerateSelectButton="True" OnSelectedIndexChanged="ActionHandler" --%>
                                                    <EmptyDataTemplate>
                                                        <div style="float: left; width: 100%;">
                                                            <asp:Label ID="lblEmpty" runat="server" CssClass="emptydata-custom" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                            <asp:ImageButton ID="btnPeriodSearchEmptyTmp" runat="server" Text="<%$ resources:Search%>"
                                                                ToolTip="<%$ resources:Search%>" OnClick="ActionHandler" CommandName="PERIODSEARCHPOPUP"
                                                                SkinID="search-ext" CssClass="margntop2" />
                                                            <asp:ImageButton ID="btnPeriodClearEmptyTmp" runat="server" Text="<%$ resources:Clear%>"
                                                                ToolTip="<%$ resources:Clear%>" OnClick="ActionHandler" CommandName="PERIODCLEAR"
                                                                SkinID="clear-ext" CssClass="margntop2" />
                                                        </div>
                                                    </EmptyDataTemplate>
                                                    <EmptyDataRowStyle BackColor="Bisque" />
                                                    <%--<SelectedRowStyle BackColor="#bde6f5" />--%>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$ resources:Period%> " SortExpression="">
                                                            <ItemTemplate>
                                                                <asp:HiddenField runat="server" ID="hdfDept" Value='<%# Eval("EPH_DEPT") %>' />
                                                                <asp:HiddenField runat="server" ID="hdfDelStatus" Value='<%# Eval("EPH_DEL_STATUS") %>' />
                                                                <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval("EPH_STATUS") %>' />
                                                                <asp:HiddenField runat="server" ID="hdfJournalStatus" Value='<%# Eval("EPH_HAS_JRNL_ENTRY") %>' />
                                                                <asp:HiddenField runat="server" ID="hdfProcessMode" Value='<%# Eval("EPH_PRC_MODE") %>' />
                                                                <asp:HiddenField runat="server" ID="hdfEphPK" Value='<%#Eval("EPH_PK")%>' />
                                                                <asp:HiddenField runat="server" ID="hdfProcessLastModDate" Value='<%#Eval("EPH_MOD_DT") %>' />
                                                                <asp:Label ID="lblPeriodText" runat="server" Text='<%# Convert.ToDateTime(Eval("EPH_PAYROLL_MONTH")).ToString(Resources.Constants.DateFormatMonthYear) + " : " + Convert.ToString(Eval("EPH_NO")) + " <br/>" + Eval("EPH_FROM_DATE", Resources.Constants.HRMSDateFormatGrid) + " to " + Eval("EPH_TO_DATE", Resources.Constants.HRMSDateFormatGrid)%>'
                                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EPH_PRC_NAME"))) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="80%" />
                                                            <ItemStyle Width="80%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <%-- HeaderText="<%$ resources:Controls,Edit %>"--%>
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="imbEditProcess" SkinID="imbeditgrid" EnableViewState="true"
                                                                    ToolTip="<%$ resources:Controls,Edit %>" CommandName="PROCESSEDIT" OnClick="ActionHandler" />
                                                                <asp:ImageButton runat="server" ID="imbDeleteProcess" SkinID="imbdeletegrid" CommandName="DELETEPERIOD"
                                                                    OnClick="ActionHandler" ToolTip="<%$ resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirmationMsg(this);"
                                                                    Visible='<%# Convert.ToInt32(Eval("EPH_STATUS")) > 0 ? false : true  %>' />
                                                                <asp:Button ID="imgStatus" runat="server" OnClientClick="javascript:return false;"
                                                                    CssClass='<%# Convert.ToString(Eval("EPH_CSS_CLASS"))  +" "+ "margn-rgt0" %>'
                                                                    ToolTip='<%# Eval("EPH_STATUS_TEXT") %>' />
                                                                <asp:Button ID="imgPosted" runat="server" OnClientClick="javascript:return false;"
                                                                    Visible="<%$ resources:ConfigurationsRes,FinModuleEnabled %>" CssClass='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? GetLocalResourceObject("unposted").ToString() : Eval("FTH_CSS_CLASS")%>'
                                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? Resources.Captions.NotPosted : Eval("FTH_STATUS_TEXT")%>' />
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:ImageButton ID="btnPeriodSearch" runat="server" Text="<%$ resources:Search%>"
                                                                    ToolTip="<%$ resources:Search%>" OnClick="ActionHandler" CommandName="PERIODSEARCHPOPUP"
                                                                    SkinID="search-ext" CssClass="margntop2 margn-rgt0" />
                                                                <asp:ImageButton ID="btnPeriodClear" runat="server" Text="<%$ resources:Clear%>"
                                                                    ToolTip="<%$ resources:Clear%>" OnClick="ActionHandler" CommandName="PERIODCLEAR"
                                                                    SkinID="clear-ext" CssClass="margntop2 margn-rgt0" />
                                                            </HeaderTemplate>
                                                            <HeaderStyle Width="10%" CssClass="amount-numeric" Wrap="false" />
                                                            <ItemStyle Width="10%" HorizontalAlign="Right" CssClass="padgrgt2 padglft1" Wrap="false" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <uc1:PagerControl ID="uclPaging" runat="server" />
                                            </div>
                                        </td>
                                        <td style="width: 1%">
                                        </td>
                                        <td style="width: 74%" class="grid-rgt relative">
                                            <div class="gridwrap scroll-h300 margntop-minus3">
                                                <asp:GridView runat="server" ID="grdEmpPayrollList" Width="100%" AllowPaging="false"
                                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                                    EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler" OnSorting="ActionHandler">
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkEmpHeader" runat="server" ToolTip="Select for Process" TabIndex="15" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox runat="server" ID="chkEmpselect" TabIndex="16" Checked='<%# Convert.ToInt32(Eval("EPS_PK")) > 0 ? true : false %>'
                                                                    Enabled='<%# string.IsNullOrEmpty(Convert.ToString(Eval("PSL_PK"))) ? true : false %>' />
                                                                <asp:HiddenField runat="server" ID="hdfEmpPK" Value='<%# Eval("EPS_EMPLOYEE") %>' />
                                                                <asp:HiddenField runat="server" ID="hdfItemPK" Value='<%#Eval("EPS_PK")%>' />
                                                                <asp:HiddenField runat="server" ID="hdfPaymentPk" Value='<%#Eval("PSL_PK")%>' />
                                                                <asp:HiddenField runat="server" ID="hdfPayrollLastModDate" Value='<%#Eval("EPS_MOD_DT") %>' />
                                                                <asp:HiddenField runat="server" ID="hdfIsLJ" Value='<%#Eval("EPS_IS_LJ") %>' />
                                                                <asp:HiddenField runat="server" ID="hdfWorkDays" Value='<%#Eval("EPS_WORK_DAYS") %>' />
                                                                <asp:HiddenField runat="server" ID="hdfEmpDOJ" Value='<%#Eval("empDOJ") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="2%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Employee%> " SortExpression="EPS_TEXT">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmployeeText" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Convert.ToString(Eval("EPS_TEXT"))),47) %>'
                                                                    ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("EPS_TEXT"))) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="40%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Earnings%> " SortExpression="">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAllowance" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("EPS_ALW_AMT"))%>'
                                                                    ToolTip='<%#GetFormattedCurrencyWithComma(Eval("EPS_ALW_AMT"))%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="amount-numeric" />
                                                            <HeaderStyle Width="10%" CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Deduction%> " SortExpression="">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDeduction" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("EPS_DED_AMT"))%>'
                                                                    ToolTip='<%#GetFormattedCurrencyWithComma(Eval("EPS_DED_AMT"))%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="amount-numeric" />
                                                            <HeaderStyle Width="10%" CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:GrossSalary%> " SortExpression="">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGrossSalary" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("EPS_GROSS_AMT")) %>'
                                                                    ToolTip='<%# GetFormattedCurrencyWithComma(Eval("EPS_GROSS_AMT")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="amount-numeric" />
                                                            <HeaderStyle Width="13%" CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:NetSalary%> " SortExpression="EPS_NET_AMT">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNetSalary" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("EPS_NET_AMT")) %>'
                                                                    ToolTip='<%# GetFormattedCurrencyWithComma(Eval("EPS_NET_AMT")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="amount-numeric" />
                                                            <HeaderStyle Width="10%" CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <%--<asp:TemplateField HeaderText="<%$ resources:CTC%> " SortExpression="">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCtc" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("EPS_CTC_AMT")) %>'
                                                                    ToolTip='<%# GetFormattedCurrencyWithComma(Eval("EPS_CTC_AMT")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="amount-numeric" />
                                                            <HeaderStyle Width="10%" CssClass="amount-numeric" />
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField HeaderText="">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="imbEmpSettings" ToolTip="<%$Resources:EmpSettings %>"
                                                                    SkinID="imbactiongrid" Visible='<%# Convert.ToInt32(Eval("EPS_IS_LJ")) == 1 ? true : false %>' />
                                                                <%--OnClick="ActionHandler" CommandName="EMPSETTINGSPOPUP"--%>
                                                                <asp:ImageButton ID="imbPrintPayroll" runat="server" OnClick="ActionHandler" CommandName="PRINTGRID"
                                                                    SkinID="btnPrint" ToolTip="<%$resources:PaySlip %>" Visible='<%# Convert.ToInt32(Eval("EPS_PK")) > 0 ? true : false %>' />
                                                                <asp:ImageButton runat="server" ID="imbEditPayroll" SkinID="imbeditgrid" CommandName="EDITPAYROLL"
                                                                    ToolTip="<%$ resources:EditPayroll%>" OnClick="ActionHandler" Visible='<%# Convert.ToInt32(Eval("EPS_PK")) > 0 ? (string.IsNullOrEmpty(Convert.ToString(Eval("PSL_PK"))) ? true : false) : false %>'
                                                                    CommandArgument="PageAction_List" OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load" />
                                                                <asp:ImageButton runat="server" ID="imbViewPayroll" SkinID="btnview" CommandName="VIEWPAYROLL"
                                                                    ToolTip="<%$ resources:ViewPayroll%>" OnClick="ActionHandler" Visible='<%# Convert.ToInt32(Eval("EPS_PK")) > 0 ? true : false %>' />
                                                                <asp:ImageButton runat="server" ID="imbDeletePayroll" SkinID="imbdeletegrid" CommandName="DELETEPAYROLL"
                                                                    ToolTip="<%$ resources:DeletePayroll%>" OnClick="ActionHandler" Visible='<%# Convert.ToInt32(Eval("EPS_PK")) > 0 ? (string.IsNullOrEmpty(Convert.ToString(Eval("PSL_PK"))) ? true : false) : false %>'
                                                                    OnClientClick="return ShowDeleteConfirmationMsg(this);" CommandArgument="PageAction_List"
                                                                    OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load" />
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:ImageButton runat="server" ID="imbDeleteAll" SkinID="imbdeletegrid" CommandName="DELETEALL"
                                                                    Style="margin-right: 0px!important;" ToolTip="<%$resources:Controls,DeleteAll %>"
                                                                    OnClick="ActionHandler" OnClientClick="return ShowDeleteConfirmationMsg(this);"
                                                                    CommandArgument="PageAction_List" OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load" />
                                                            </HeaderTemplate>
                                                            <HeaderStyle Width="10%" HorizontalAlign="Right" CssClass="amount-numeric" />
                                                            <ItemStyle HorizontalAlign="Right" CssClass="amount-numeric" Wrap="false" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <div class="clear">
                                                </div>
                                            </div>
                                            <div id="divProcessFooter" class="processdetails" runat="server">
                                                <asp:Label runat="server" ID="lblTotEmpHdr" Text="<%$ resources:TotalEmpCaption%>"
                                                    AssociatedControlID="lblTotalEmployees" class="margnbotm0"></asp:Label>
                                                <asp:Label runat="server" ID="lblTotalEmployees" Text="0/0" class="margnbotm0 bold"></asp:Label>
                                                <asp:Label runat="server" ID="lblCurrentProc" class="middle-lbl-a margnbotm0" Text="<%$ resources:Current%>"
                                                    AssociatedControlID="lblCurrProcessCount"></asp:Label>
                                                <asp:Label runat="server" ID="lblCurrProcessCount" Text="" CssClass="margnbotm0  bold"></asp:Label>
                                                <%--<asp:Label runat="server" ID="lblNewJoinersHdr" Text="<%$ resources:NewJoinersCaption%>"
                                                AssociatedControlID="lblTotalNewJoiners" class="middle-lbl-a margnbotm0"></asp:Label>
                                                <asp:Label runat="server" ID="lblTotalNewJoiners" Text="0"
                                                class="margnbotm0 bold"></asp:Label>

                                                 <asp:Label runat="server" ID="lblResignedHdr" Text="<%$ resources:ResignedCaption%>"
                                                AssociatedControlID="lblTotalResigned" class="middle-lbl-a margnbotm0"></asp:Label>
                                                <asp:Label runat="server" ID="lblTotalResigned" Text="0"
                                                class="margnbotm0 bold"></asp:Label>--%>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <div id="divPayrollHeader" runat="server" border="0" cellpadding="0" cellspacing="0"
                                class="head-info">
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 1%;">
                                        </td>
                                        <td style="width: 45%;">
                                            <asp:Label runat="server" ID="lblPayrollEmployeeH" Text="<%$ resources:EmployeeH%>"
                                                AssociatedControlID="lblPayrollEmployee" class="margnbotm0"></asp:Label>
                                            <asp:Label ID="lblPayrollEmployee" runat="server" CssClass="margnbotm0 bold"></asp:Label>
                                            <asp:HiddenField runat="server" ID="hdfPayrollEmployee" Value='<%#Eval("empPK")%>' />
                                        </td>
                                        <td style="width: 39%;">
                                            <asp:Label runat="server" ID="lblPayrollProcessH" Text="<%$ resources:PayrollProcessH%>"
                                                AssociatedControlID="lblPayrollProcess" class="margnbotm0"></asp:Label>
                                            <asp:Label ID="lblPayrollProcess" runat="server" CssClass="margnbotm0 bold"></asp:Label>
                                        </td>
                                        <td style="width: 15%;">
                                            <asp:Label runat="server" ID="lblPayrollProcessDateH" Text="<%$ resources:ProcessDateH%>"
                                                AssociatedControlID="lblPayrollProcessDate" class="margnbotm0"></asp:Label>
                                            <asp:Label ID="lblPayrollProcessDate" runat="server" CssClass="margnbotm0 bold"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                <h1>
                                    <%= GetLocalResourceObject("Employee_Details").ToString()%></h1>
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblPaymentModeH" Text="<%$ resources:PaymentMode%>"
                                                        AssociatedControlID="lblPaymentMode"></asp:Label>
                                                    <asp:Label ID="lblPaymentMode" runat="server" CssClass="input-small"></asp:Label>
                                                    <asp:Label runat="server" ID="lblCurrencyH" Text="<%$ resources:Currency%>" CssClass="middle-lbl"
                                                        AssociatedControlID="lblCurrency"></asp:Label>
                                                    <asp:Label ID="lblCurrency" runat="server" CssClass="input-small"></asp:Label>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblAccountNoH" Text="<%$ resources:AccountNO%>" AssociatedControlID="lblAccountNO"></asp:Label>
                                                    <asp:Label ID="lblAccountNO" runat="server" CssClass="input-small"></asp:Label>
                                                    <asp:Label runat="server" ID="lblAccountNameH" Text="<%$ resources:AccountName%>"
                                                        AssociatedControlID="lblAccountName" CssClass="middle-lbl"></asp:Label>
                                                    <asp:Label ID="lblAccountName" runat="server" CssClass="input-medium"></asp:Label>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblBankNameH" Text="<%$ resources:BankName%>" AssociatedControlID="lblBankName"></asp:Label>
                                                    <asp:Label ID="lblBankName" runat="server" CssClass="input-w64per"></asp:Label>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblBranchH" Text="<%$ resources:Branch%>" AssociatedControlID="lblBranch"></asp:Label>
                                                    <asp:Label ID="lblBranch" runat="server" CssClass="input-small-c"></asp:Label>
                                                    <asp:Label runat="server" ID="lblWorkingDaysH" Text="<%$ resources:WorkingDays%>"
                                                        AssociatedControlID="lblWorkingDays" CssClass="middle-lbl-xsmall-a2"></asp:Label>
                                                    <asp:Label ID="lblWorkingDays" runat="server" CssClass="input-w7per numeric"></asp:Label>
                                                    <asp:Label runat="server" ID="lblLOPH" Text="<%$ resources:LOP%>" AssociatedControlID="lblLOP"
                                                        CssClass="lft-lbl"></asp:Label>
                                                    <asp:Label ID="lblLOP" runat="server" CssClass="input-w7per numeric"></asp:Label>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblPFNoH" Text="<%$ resources:PFNo%>" AssociatedControlID="lblPFNo"
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblPFNo" runat="server" CssClass="input-small-c" Visible="false"></asp:Label>
                                                    <asp:Label runat="server" ID="lblSOSCONoH" Text="<%$ resources:SOSCONo%>" AssociatedControlID="lblSOSCONo"
                                                        CssClass="middle-lbl-xsmall-c" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblSOSCONo" runat="server" CssClass="input-small-c" Visible="false"></asp:Label>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                            <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                <h1>
                                    <%-- <%= GetLocalResourceObject("Employee_Details").ToString()%>--%>
                                    Salary Details</h1>
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label ID="lblEmpCTCH" runat="server" Text="<%$ resources:CTC%>" AssociatedControlID="lblEmpCTC"
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblEmpCTC" runat="server" CssClass="input-small-c numeric" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblEmpTotEarningsH" runat="server" Text="<%$ resources:Earnings%>"
                                                        AssociatedControlID="lblEmpTotEarnings"></asp:Label>
                                                    <asp:Label ID="lblEmpTotEarnings" runat="server" CssClass="input-small numeric"></asp:Label>
                                                    <asp:Label ID="lblEmpTotDeductionH" runat="server" Text="<%$ resources:Deduction%>"
                                                        AssociatedControlID="lblEmpTotDeduction" CssClass="middle-lbl"></asp:Label>
                                                    <asp:Label ID="lblEmpTotDeduction" runat="server" CssClass="input-small numeric"></asp:Label>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label ID="lblEmpGrossSalaryH" runat="server" Text="<%$ resources:GrossSalary%>"
                                                        AssociatedControlID="lblEmpGrossSalary"></asp:Label>
                                                    <asp:Label ID="lblEmpGrossSalary" runat="server" CssClass="input-small numeric bold"></asp:Label>
                                                    <asp:Label ID="lblEmpNetSalaryH" runat="server" Text="<%$ resources:NetSalary%>"
                                                        AssociatedControlID="lblEmpNetSalary" CssClass="middle-lbl-d"></asp:Label>
                                                    <asp:Label ID="lblEmpNetSalary" runat="server" CssClass="input-small numeric bold"></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <div class="clear">
                                </div>
                                <div class="gridwrap floatLeft" style="width: 49%;">
                                    <div class="search-colapse-b">
                                        <h1>
                                            <%= GetGlobalResourceObject("Controls", "Earnings").ToString()%></h1>
                                        <div class="clear">
                                        </div>
                                    </div>
                                    <div id="divEarningsPay">
                                        <asp:GridView ID="grdEarnPayDetails" runat="server" AutoGenerateColumns="False" Width="100%"
                                            AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                            ShowFooter="true" OnRowDataBound="ActionHandler">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:PayElement %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfEarnSlNo" runat="server" Value='<%#Eval("EPP_SL_NO")%>' />
                                                        <asp:HiddenField ID="hdfEarnPayPK" runat="server" Value='<%#Eval("EPP_PK")%>' />
                                                        <asp:HiddenField ID="hdfEarnPayElementPK" runat="server" Value='<%#Eval("EPP_PAY_ELEMENT")%>' />
                                                        <asp:HiddenField ID="hdfIsEarn" runat="server" Value='<%#Eval("EPP_IS_DEDUCTION")%>' />
                                                        <asp:HiddenField ID="hdfPayElmtInSalary" runat="server" Value='<%#Eval("PEL_IN_SALARY")%>' />
                                                        <asp:Label ID="lblEarnPayElement" runat="server" Text='<%#Eval("EPP_PAY_ELEMENT_TEXT")%>'
                                                            ToolTip='<%#Eval("EPP_PAY_ELEMENT_TEXT")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="75%" />
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblEarnFooterH" Text="<%$ resources:Total %>"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Actual %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblActualEarnings" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("EPP_ACT_PAY_AMT"))%>'
                                                            ToolTip='<%#GetFormattedCurrencyWithComma(Eval("EPP_ACT_PAY_AMT"))%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="amount-numeric" />
                                                    <HeaderStyle Width="10%" CssClass="amount-numeric" />
                                                    <FooterStyle CssClass="amount-numeric" />
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalActualEarn"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Eligible %>">
                                                    <ItemTemplate>
                                                        <asp:RequiredFieldValidator ID="rfvEarnEligibleAmount" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="Pay" EnableClientScript="true" runat="server" ControlToValidate="txtEarnEligibleAmount"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:ErrorMessages,Err_EarnAmount %>">
                                                        </asp:RequiredFieldValidator>
                                                        <cc1:AmountValidation ID="vamEarnEligibleAmount" runat="server" ControlToValidate="txtEarnEligibleAmount"
                                                            ErrorMessage="<%$ resources:ErrorMessages,EarnValidAmount %>" NumberDigits="11"
                                                            Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="Pay"></cc1:AmountValidation>
                                                        <asp:TextBox ID="txtEarnEligibleAmount" CssClass='<%# Convert.ToInt32(Eval("EPP_PAY_ELEMENT_EDITABLE")) == 1 ? "input-w80 numeric" : "input-w80 numeric input-disabled" %>'
                                                            runat="server" Enabled='<%# Convert.ToInt32(Eval("EPP_PAY_ELEMENT_EDITABLE")) == 1 ? true : false %>'
                                                            MaxLength="12" Text='<%#GetFormattedCurrency(Eval("EPP_PAY_AMT"))%>' onkeyup="CalculateTotalEarnings(this);"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="10%" CssClass="amount-numeric" />
                                                    <ItemStyle CssClass="amount-numeric" />
                                                    <FooterStyle CssClass="amount-numeric" />
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalEarnPay"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEEARNINGSPAY"
                                                            SkinID="delete-icon" ToolTip="<%$ resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirmationMsg(this);"
                                                            Visible='<%# Convert.ToInt32(Eval("EPP_PAY_ELEMENT_EDITABLE")) == 1 ? true : false %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="5%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div style="width: 2%;">
                                </div>
                                <div class="gridwrap floatRight" style="width: 49%;">
                                    <div class="search-colapse-b">
                                        <h1>
                                            <%= GetGlobalResourceObject("Controls", "Deductions").ToString()%></h1>
                                        <div class="clear">
                                        </div>
                                    </div>
                                    <div id="divDeductionsPay">
                                        <asp:GridView ID="grdDeductPayDetails" runat="server" AutoGenerateColumns="False"
                                            Width="100%" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                            ShowFooter="true" OnRowDataBound="ActionHandler">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:PayElement %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfDeductSlNo" runat="server" Value='<%#Eval("EPP_SL_NO")%>' />
                                                        <asp:HiddenField ID="hdfDeductPayPK" runat="server" Value='<%#Eval("EPP_PK")%>' />
                                                        <asp:HiddenField ID="hdfDeductPayElementPK" runat="server" Value='<%#Eval("EPP_PAY_ELEMENT")%>' />
                                                        <asp:HiddenField ID="hdfIsDeduct" runat="server" Value='<%#Eval("EPP_IS_DEDUCTION")%>' />
                                                        <asp:HiddenField ID="hdfPayElmtInSalary" runat="server" Value='<%#Eval("PEL_IN_SALARY")%>' />
                                                        <asp:Label ID="lblDeductPayElement" runat="server" Text='<%#Eval("EPP_PAY_ELEMENT_TEXT")%>'
                                                            ToolTip='<%#Eval("EPP_PAY_ELEMENT_TEXT")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="75%" />
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblDeductFooterH" Text="<%$ resources:Total %>"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Actual %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblActualDeduct" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("EPP_ACT_PAY_AMT"))%>'
                                                            ToolTip='<%#GetFormattedCurrencyWithComma(Eval("EPP_ACT_PAY_AMT"))%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="amount-numeric" />
                                                    <HeaderStyle Width="10%" CssClass="amount-numeric" />
                                                    <FooterStyle CssClass="amount-numeric" />
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalActualDeduct"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Applied %>">
                                                    <ItemTemplate>
                                                        <asp:RequiredFieldValidator ID="rfvDeductEligibleAmount" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="Pay" EnableClientScript="true" runat="server" ControlToValidate="txtDeductEligibleAmount"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:ErrorMessages,Err_DeductAmount %>">
                                                        </asp:RequiredFieldValidator>
                                                        <cc1:AmountValidation ID="vamDeductEligibleAmount" runat="server" ControlToValidate="txtDeductEligibleAmount"
                                                            ErrorMessage="<%$ resources:ErrorMessages,DeductValidAmount %>" NumberDigits="11"
                                                            Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="Pay"></cc1:AmountValidation>
                                                        <asp:TextBox ID="txtDeductEligibleAmount" CssClass='<%# Convert.ToInt32(Eval("EPP_PAY_ELEMENT_EDITABLE")) == 1 ? "input-w80 numeric" : "input-w80 numeric input-disabled" %>'
                                                            runat="server" Enabled='<%# Convert.ToInt32(Eval("EPP_PAY_ELEMENT_EDITABLE")) == 1 ? true : false %>'
                                                            MaxLength="12" Text='<%#GetFormattedCurrency(Eval("EPP_PAY_AMT"))%>' onkeyup="CalculateTotalDeductions(this);"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="10%" CssClass="amount-numeric" />
                                                    <ItemStyle CssClass="amount-numeric" />
                                                    <FooterStyle CssClass="amount-numeric" />
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalDeductPay"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEDEDUCTPAY"
                                                            SkinID="delete-icon" ToolTip="<%$ resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirmationMsg(this);"
                                                            Visible='<%# Convert.ToInt32(Eval("EPP_PAY_ELEMENT_EDITABLE")) == 1 ? true : false %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="5%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    <% = GetLocalResourceObject("PayrollHistory").ToString() %>
                                </h1>
                                <asp:ImageButton runat="server" ID="imbShowHistory" OnClientClick="javascript:return ShowHidePayrollHistory(1);"
                                    SkinID="imbArrowInactive" ToolTip="<%$ resources:ShowPayrollHistory %>" TabIndex="65" />
                                <asp:ImageButton runat="server" ID="imbHideHistory" OnClientClick="javascript:return ShowHidePayrollHistory();"
                                    Style="display: none" SkinID="imbArrowActive" TabIndex="65" ToolTip="<%$ resources:HidePayrollHistory%>" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divPayrollHistory" style="display: none">
                                <div class="gridwrap">
                                    <asp:GridView runat="server" ID="grdPayrollHistory" Width="100%" AllowPaging="false"
                                        AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                        EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:Month%> " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgrdProcesDate" runat="server" Text='<%# Eval("EPH_PAYROLL_MONTH", Resources.Constants.MonthFormatGrid) %>'
                                                        ToolTip='<%# Eval("EPH_PAYROLL_MONTH", Resources.Constants.MonthFormatGrid) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Wrap="false" />
                                                <HeaderStyle Width="7%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Period%> " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgrdPeriod" runat="server" Text='<%# Convert.ToDateTime(Eval("EPH_FROM_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat) + " to " + Convert.ToDateTime(Eval("EPH_TO_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat)%>'
                                                        ToolTip='<%# Convert.ToDateTime(Eval("EPH_FROM_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat) + " to " + Convert.ToDateTime(Eval("EPH_TO_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat)%>'></asp:Label>
                                                    <asp:HiddenField ID="hdfDetPk" runat="server" Value='<%# Eval("EPS_PK") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Wrap="false" />
                                                <HeaderStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:ProcessCaption%> " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgrdCaption" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EPH_PRC_NAME")),55) %>'
                                                        ToolTip='<%# Eval("EPH_PRC_NAME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                                <HeaderStyle Width="40%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:GrossSalary%> " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgrdGrossSalary" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("EPS_GROSS_AMT")) %>'
                                                        ToolTip='<%# GetFormattedCurrencyWithComma(Eval("EPS_GROSS_AMT")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="amount-numeric" />
                                                <HeaderStyle Width="10%" CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Earnings%> " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgrdEarnings" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("EPS_ALW_AMT")) %>'
                                                        ToolTip='<%# GetFormattedCurrencyWithComma(Eval("EPS_ALW_AMT")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="amount-numeric" />
                                                <HeaderStyle Width="10%" CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Deduction%> " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgrdDeduction" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("EPS_DED_AMT")) %>'
                                                        ToolTip='<%# GetFormattedCurrencyWithComma(Eval("EPS_DED_AMT")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="amount-numeric" />
                                                <HeaderStyle Width="10%" CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:NetSalary%> " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgrdNetSalary" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("EPS_NET_AMT")) %>'
                                                        ToolTip='<%# GetFormattedCurrencyWithComma(Eval("EPS_NET_AMT")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="amount-numeric" />
                                                <HeaderStyle Width="10%" CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Actions%> ">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btngrdPrint" runat="server" OnClick="ActionHandler" CommandName="PRINTPAYSLIP"
                                                        SkinID="btnPrint" ToolTip="<%$ resources:Controls,Print %>" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="3%" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
            </div>
            <div id="divScriptButtons">
                <asp:Button runat="server" ID="btnJournalize_Action" CommandName="JOURNALIZE" OnClick="ActionHandler"
                    EnableTheming="false" Style="display: none" />
                <asp:Button ID="btnJournalizeUpdate" runat="server" OnClick="ActionHandler" CommandName="JOURNALIZEUPDATE"
                    EnableTheming="false" Style="display: none" />
            </div>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsSearch" ValidationGroup="Search" runat="server" />
                <asp:ValidationSummary ID="vsPayrollProcess" ValidationGroup="Process" runat="server" />
                <asp:ValidationSummary ID="vsReProcess" ValidationGroup="ReProcess" runat="server" />
                <asp:ValidationSummary ID="vsPay" ValidationGroup="Pay" runat="server" />
                <asp:ValidationSummary ID="vsSettings" ValidationGroup="Settings" runat="server" />
                <asp:ValidationSummary ID="vsWorkingDays" ValidationGroup="workingDays" runat="server" />
                <asp:ValidationSummary ID="vsWrkDaysCheck" ValidationGroup="WrkDaysCheck" runat="server" />
                <asp:ValidationSummary ID="vsNonPayroll" ValidationGroup="NonPayroll" runat="server" />
            </div>
            <div id="divPopUpWorkingDays" style="display: none">
                <div class="content-wrapper">
                    <div class="Button-container-popup">
                        <asp:Button ID="btnWorkDaysApply" SkinID="btnInner-add-dsd" runat="server" Text="<%$resources:Controls,Apply %>"
                            OnClick="ActionHandler" CommandName="WORKDAYSAPPLY" ValidationGroup="WrkDaysCheck"
                            OnClientClick="javascript:ValidatePayrollPage('WrkDaysCheck')" />
                    </div>
                    <div class="gridwrap">
                        <asp:GridView runat="server" ID="grdWorkingDays" Width="100%" AllowPaging="false"
                            AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                            EmptyDataRowStyle-CssClass="emptytable">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label></EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ resources:EmployeeType%> " SortExpression="">
                                    <ItemTemplate>
                                        <asp:HiddenField runat="server" ID="hdfEmploymentTypePK" Value='<%# Eval("EPW_EMP_TYPE") %>' />
                                        <asp:Label ID="lblEmploymentType" runat="server" Text='<%# Eval("EMT_NAME") %>' ToolTip='<%# Eval("EMT_NAME") %>'></asp:Label></ItemTemplate>
                                    <HeaderStyle Width="60%" />
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ resources:PaidHolidays%> " SortExpression="">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPaidHolidays" runat="server" Text='<%# Eval("EPW_HOLIDAYS") %>'
                                            CssClass="input-half numeric" MaxLength="8"></asp:TextBox></ItemTemplate>
                                    <ItemStyle Width="20%" CssClass="amount-numeric" />
                                    <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ resources:WorkDays%> " SortExpression="">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtWorkDays" runat="server" Text='<%# Eval("EPW_WORK_DAYS") %>'
                                            CssClass="input-half numeric" MaxLength="8"></asp:TextBox><%-- onblur="CheckTotalProcessDays();"--%><asp:CustomValidator
                                                ID="csvWorkDays" runat="server" Display="None" Text="*" ControlToValidate="txtWorkDays"
                                                ClientValidationFunction="CheckTotalProcessDays" ErrorMessage="<%$resources:Msg_Err_WorkDays %>"
                                                ValidationGroup="WrkDaysCheck"></asp:CustomValidator></ItemTemplate>
                                    <ItemStyle Width="20%" CssClass="amount-numeric" />
                                    <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div id="divPopupEmpSettings" style="display: none">
                <div class="content-wrapper">
                    <div class="popup-header padgbotm10">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 77%;">
                                    <asp:Label ID="lblEmpNameH" runat="server" AssociatedControlID="lblEmpName" Text='<%$ resources:EmployeeH%>'
                                        class="margnbotm0"></asp:Label><asp:Label ID="lblEmpName" runat="server" Text=""
                                            class="margnbotm0"></asp:Label>
                                </td>
                                <td style="width: 23%;">
                                    <asp:Label ID="lblEmpDOJH" runat="server" AssociatedControlID="lblEmpDOJ" Text="<%$ resources:DOJ%>"
                                        class="margnbotm0"></asp:Label><asp:Label ID="lblEmpDOJ" runat="server" Text="" class="margnbotm0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="lblEmpWorkDays" runat="server" Text="<%$ resources:WorkDays%>" AssociatedControlID="txtEmpWorkDays"></asp:Label>
                                    <asp:TextBox ID="txtEmpWorkDays" runat="server" TabIndex="9" MaxLength="8" CssClass="input-medium"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvSettings" CssClass="star" SetFocusOnError="true"
                                        ValidationGroup="Settings" EnableClientScript="true" runat="server" ControlToValidate="txtEmpWorkDays"
                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_EmpWorkDays %>">
                                    </asp:RequiredFieldValidator><asp:CustomValidator ID="csvEmpWorkDays" runat="server"
                                        Display="None" Text="*" ControlToValidate="txtEmpWorkDays" ClientValidationFunction="CheckEmpWorkDays"
                                        ErrorMessage="<%$resources:Msg_Err_WorkDays %>" ValidationGroup="Settings"></asp:CustomValidator>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <asp:HiddenField ID="hdfSettingsEmpPK" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdfSender" runat="server" Value="" />
                                    <div class="Button-container-popup">
                                        <asp:Button ID="btnApplyEmpSettings" SkinID="btnInner-add-dsd" runat="server" Text="<%$resources:Controls,Apply %>"
                                            ValidationGroup="Settings" OnClientClick="javascript:return SettingsApply();" /><%--OnClick="ActionHandler" CommandName="EMPSETTINGSAPPLY"--%>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="divPeriodSearch" style="display: none">
                <div class="content-wrapper">
                    <div class="Button-container-popup">
                        <asp:Button ID="btnperiodPopSearch" SkinID="btnInner-search" runat="server" Text="<%$resources:Controls,Search %>"
                            OnClick="ActionHandler" CommandName="PERIODSEARCH" TabIndex="203" ToolTip="<%$resources:Controls,Search %>" />
                    </div>
                    <table class="table-devide tablelayout">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="lblSalMonthFrom" runat="server" Text="<%$ resources:MonthFrom%>" AssociatedControlID="txtMonthFrom"></asp:Label>
                                    <asp:TextBox ID="txtMonthFrom" runat="server" CssClass="input-w12-5per" TabIndex="200"
                                        onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" onclick="ChangeHeight(250)"
                                        onblur="ChangeHeight(185)" onchange="SetToDate()"></asp:TextBox>
                                    <cc2:CalendarExtender runat="server" ID="txtMonthFrom_CalendarExtender" BehaviorID="calendar2"
                                        TargetControlID="txtMonthFrom" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                                        ClientIDMode="Static" OnClientHidden="onCalendarHidden">
                                    </cc2:CalendarExtender>
                                    <asp:Label ID="lblMonthTo" runat="server" Text="<%$ resources:MonthTo%>" AssociatedControlID="txtMonthTo"
                                        CssClass="middle-lbl-xsmall-a"></asp:Label>
                                    <asp:TextBox ID="txtMonthTo" runat="server" CssClass="input-w12-5per" TabIndex="201"
                                        onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" onclick="ChangeHeight(250)"
                                        onblur="ChangeHeight(185)"></asp:TextBox>
                                    <cc2:CalendarExtender runat="server" ID="txtMonthTo_CalendarExtender" BehaviorID="calendar3"
                                        TargetControlID="txtMonthTo" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                                        ClientIDMode="Static" OnClientHidden="onCalendarHidden">
                                    </cc2:CalendarExtender>
                                    <div class="clear">
                                    </div>
                                    <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Captions,Status%>" AssociatedControlID="ddlStatus"></asp:Label>
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-w22-6per" TabIndex="202">
                                        <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Captions,NotPosted %>" Value="0" Enabled="<%$ resources:ConfigurationsRes,FinModuleEnabled %>"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Captions,Posted %>" Value="1" Enabled="<%$ resources:ConfigurationsRes,FinModuleEnabled %>"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <%-- <td>
                                <div class="div2col-S">
                                   
                                </div>
                            </td>--%>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="divJournalize" style="display: none">
                <uc4:Journalize ID="ucrJournalize" runat="server" />
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <uc3:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="Process">
                </uc3:WorkflowUserComments>
            </div>
            <div id="divData">
            </div>
            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormatWithComma" runat="server" />
            <asp:HiddenField ID="hdfCompany" runat="server" Value="0" />
            <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
            <asp:HiddenField ID="hdfLastModDate" runat="server" />
            <asp:HiddenField ID="hdfFilter" Value="0" runat="server" />
            <asp:HiddenField ID="hdfMonthlyMode" Value="0" runat="server" />
            <asp:HiddenField ID="hdfTotalProcessDays" Value="0" runat="server" />
            <asp:HiddenField ID="hdfExchangeRateFormat" runat="server" />
            <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
            <asp:HiddenField ID="hdfJournalizeWorkFlow" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIsCancelled" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIsPosted" Value="0" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
