<%@ Page Title="<%$ Resources:Captions,Title_OtherLeaveEntry %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="OtherLeaveEntry.aspx.cs"
    Inherits="HRMS.Payroll.OtherLeaveEntry" Theme="ClassicExt" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            $(document).ready(function () {
                ShowHideImport($("[id$=hdfImportVisible]").val());
                GrandScriptUtils.AddDateRangeCommon("txtFilterFromDate", "hdfFilterFromDate", "txtFilterToDate", "hdfFilterToDate", false, false);
                GrandScriptUtils.MakeAutoCompleteDDL("txtFilterBranch", url, "hdfFilterBranch", true, true, "BRANCHLOCATION");
                GrandScriptUtils.MakeAutoCompleteDDL("txtFilterDept", url, "hdfFilterDept", true, true, "DEPARTMENTAUTOCOMPLETE");
                GrandScriptUtils.DatePickerCommon("txtDate");
                GrandScriptUtils.MakeAutoCompleteDDL("txtHdDepartment", url, "hdfHdDepartment", true, true, "DEPARTMENTAUTOCOMPLETE");
                GrandScriptUtils.MakeAutoCompleteDDL("txtHdBranchLocation", url, "hdfHdBranchLocation", true, true, "BRANCHLOCATION");
                GrandScriptUtils.MakeAutoCompleteDDL("txtDesignation", url, "hdfDesignation", true, true, "DESIGNATION");
                GrandScriptUtils.MakeAutoCompleteDDL("txtBranchLocation", url, "hdfBranchLocation", true, true, "BRANCHLOCATION");
                GrandScriptUtils.MakeAutoCompleteDDL("txtDepartment", url, "hdfDepartment", true, true, "DEPARTMENTAUTOCOMPLETE");
                GrandScriptUtils.MakeAutoCompleteDDL("txtTrxNo", url, "hdfTrxPk", true, true, "EOLNUMBER");
                //GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee", url + "?EmpCategory=2", "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETE");
                $("[id$=btnDateChanged]").hide();

                var deptid = parseInt($("[id$=hdfHdDepartment]").val());
                if (deptid > 0) {
                    DisableAuto($("[id$=txtDepartment]"), $("[id$=hdfDepartment]"));
                    //BindEmployee();
                }
                var branchid = parseInt($("[id$=hdfHdBranchLocation]").val());
                if (branchid > 0)
                    DisableAuto($("[id$=txtBranchLocation]"), $("[id$=hdfBranchLocation]"));
                InitEmployeeAuto();

            });

        }
        function ShowHideImport(flag) {
            if (flag == 1) {
                $("[id$=divImport").show();
                $("[id$=imbShowImport").hide();
                $("[id$=imbHideImport").show();
            }
            else {
                $("[id$=divImport").hide();
                $("[id$=imbShowImport").show();
                $("[id$=imbHideImport").hide();
            }
            $("[id$=hdfImportVisible]").val(flag);
            return false;
        }
        function BindEmployee() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee", url + "?EmpCategory=2" + "&EmpDept=" + $("[id$=hdfHdDepartment]").val() + "&EmpBranch=" + $("[id$=hdfHdBranchLocation]").val() + "&EmpCompany=" + $("[id$=ddlCompanyHd]").val() + "&EmpDesignation=" + $("[id$=hdfDesignation]").val() + "&ToDate=" + $("[id$=txtDate]").val(), "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETE", "", true, true, false, 1, '<%= Resources.ErpRes.All_Small %>');

        }

        function BindEmployeeSearch() {

            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployeeSearch", url + "?Type=" + $("[id$=hdfFilterBranch]").val() + "&EmpCategory=2    &ToDate=" + $("[id$=hdfNextYearDate]").val(), "hdfEmployeeSearch", true, true, "EMPLOYEEAUTOCOMPLETEBYFILTER", "", true, true, false, 1);
        }

        function InitEmployeeAuto() {
            BindEmployee();
            BindEmployeeSearch();
            ResetEmployee();
        }

        //        function BindEmployee() {
        //            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee ", url + "?EmpCategory=2" + "&EmpDept=" + $("[id$=hdfDepartment]").val(), "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETE", "", true, true, false, 1, '<%= Resources.ErpRes.All_Small %>');
        //        }
        //To excecute after  auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtHdBranchLocation") {
                SetBranchDeptSearch(1, 0);
                BindEmployee();
                ResetEmployee();
            }
            if (targetControlID == "txtEmployeeSearch") {
                $("[id$=txtEmployeeSearch]").val(defText);
                $("[id$=hdfEmployeeSearch]").val('-1');
            }
            if (targetControlID == "txtHdDepartment") {
                SetBranchDeptSearch(0, 0);
                BindEmployee();
                ResetEmployee();
            }
            if (targetControlID == "txtDepartment" || targetControlID == "txtBranchLocation" || targetControlID == "txtDesignation") {
                BindEmployee();
                ResetEmployee();
            }
        }

        //To excecute after  auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtHdBranchLocation") {
                $("[id$=hdfHdBranchLocation]").val("-1");
                SetBranchDeptSearch(1, 1);
                BindEmployee();
                ResetEmployee();
            }
            if (targetControlID == "txtHdDepartment") {
                $("[id$=hdfHdDepartment]").val("-1");
                SetBranchDeptSearch(0, 1);
                BindEmployee();
                ResetEmployee();
            }
            if (targetControlID == "txtDepartment" || targetControlID == "txtBranchLocation" || targetControlID == "txtDesignation") {
                BindEmployee();
                ResetEmployee();
            }
        }

        function ResetEmployee() {
            var defText = '<%= Resources.ErpRes.All_Small %>';
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
        function ShowHideAdvanceSearch(flag) {
            if (flag == 1) {
                $("[id$=divEmployeeFilterDetails").show();
                $("[id$=imbShowFilterDetails").hide();
                $("[id$=imbHideFilterDetails").show();
            }
            else {
                $("[id$=divEmployeeFilterDetails").hide();
                $("[id$=imbShowFilterDetails").show();
                $("[id$=imbHideFilterDetails").hide();
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
                $("[id$='pnlDelete']").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
            }
        }
        function ShowHideAdvancedSearch(flag) {
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


        function AfterDateSelect(controlID) {
            if (controlID == "txtDate") {
                //                alert('Last:' + $("[id$=hdfLastSelectedDate]").val());
                //                alert('New:' + $("[id$=txtDate]").val());
                if ($("[id$=hdfLastSelectedDate]").val().slice(3, 11) != $("[id$=txtDate]").val().slice(3, 11)) {
                    $("[id$=hdfLastSelectedDate]").val($("[id$=txtDate]").val());
                    var PrevMonthNo = '<%= GetGlobalResourceObject("ConfigurationsRes", "hrmsLeaveEntryPrevMonthDiff").ToString() %>';
                    var fromDate = $.datepicker.parseDate("dd-M-yy", $('input[id$=txtDate]').val());
                    fromDate = new Date(fromDate.getFullYear(), fromDate.getMonth() - PrevMonthNo, fromDate.getDate());
                    $("[id$=hdfNextYearDate]").val($.datepicker.formatDate("dd-M-yy", fromDate))
                    // $("[id$=btnDateChanged]").click();
                }
                InitEmployeeAuto();
            }
        }

        function ClearDateSelect() {
            if ($("[id$=txtDate]").val() == '') {
                InitEmployeeAuto();
            }
        }
        //show confirmation msg for unsaved records
        function ShowMsgUnsaved() {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Msg_Unsaved_Exist").ToString() %>';
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
                        $("[id$=imbDetSearch]").click();
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


        function SetBranchDeptSearch(mode, isInvalid) {
            //mode  0:dept,1:Branch
            var defaultText = '<%= Resources.ErpRes.AutoDefaultValue %>';
            if (parseInt(mode) == 0) {
                if (parseInt(isInvalid) == 1) {
                    $("[id$=txtDepartment]").val(defaultText);
                    $("[id$=hdfDepartment]").val("-1");
                    EnableAuto($("[id$=txtDepartment]"), $("[id$=hdfDepartment]"));
                }
                else {
                    var dept = $("[id$=txtHdDepartment]").val();
                    var deptid = parseInt($("[id$=hdfHdDepartment]").val());
                    $("[id$=txtDepartment]").val(dept);
                    $("[id$=hdfDepartment]").val(deptid);
                    DisableAuto($("[id$=txtDepartment]"), $("[id$=hdfDepartment]"));
                }
            }
            else if (parseInt(mode) == 1) {
                if (parseInt(isInvalid) == 1) {
                    $("[id$=txtBranchLocation]").val(defaultText);
                    $("[id$=hdfBranchLocation]").val("-1");
                    EnableAuto($("[id$=txtBranchLocation]"), $("[id$=hdfBranchLocation]"));
                }
                else {
                    var branch = $("[id$=txtHdBranchLocation]").val();
                    var branchid = parseInt($("[id$=hdfHdBranchLocation]").val());
                    $("[id$=txtBranchLocation]").val(branch);
                    $("[id$=hdfBranchLocation]").val(branchid);
                    DisableAuto($("[id$=txtBranchLocation]"), $("[id$=hdfBranchLocation]"));
                }
            }
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

        function ValidatePage(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                CheckValidationDuplicate(valGroup);
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html());
                return false;
            }
            else {
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

        $("[id*=chkEmpHeader]").live("click", function () {
            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (chkHeader.is(":checked")) {
                    $(this).attr("checked", "checked");
                    $("td", $(this).closest("tr")).addClass("selected");
                } else {
                    $(this).removeAttr("checked");
                    $("td", $(this).closest("tr")).removeClass("selected");
                }
            });
        });

        $("[id*=chkEmpselect]").live("click", function () {
            var grid = $(this).closest("table");
            var chkHeader = $("[id*=chkEmpHeader]", grid);
            if (!$(this).is(":checked")) {
                $("td", $(this).closest("tr")).removeClass("selected");
                chkHeader.removeAttr("checked");
            } else {
                $("td", $(this).closest("tr")).addClass("selected");
                if ($("[id*=chkEmpselect]", grid).length == $("[id*=chkEmpselect]:checked", grid).length) {
                    chkHeader.attr("checked", "checked");
                }
            }
        });



        function HideTextBox(ddlId) {
            var selectedValue = ddlId.value;
            $("[id$=ddlCompany]").val(selectedValue);
            if (selectedValue > 0) {
                $("[id$=ddlCompany]").attr("disabled", true);
            } else {
                $("[id$=ddlCompany]").attr("disabled", false);
            }
            BindEmployee();
            ResetEmployee();
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
    <asp:UpdatePanel ID="aupdpnlTaskHome" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdfCurrentELD_PK" runat="server" />
            <asp:HiddenField ID="hdfCurrentROW_NO" runat="server" />
            <asp:HiddenField ID="hdfLastSelectedDate" runat="server" />
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="10" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="11" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$resources:Controls,Save %>"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save"
                                            ValidationGroup="Save" OnClick="ActionHandler" OnClientClick="javascript:ValidatePage('save')"
                                            TabIndex="39" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="40" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            CssClass="popupclose" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" OnClick="ActionHandler"
                                            TabIndex="41" />
                                    </li>
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
                                CommandArgument="SEC_ActionPanel" TabIndex="99" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="99" OnClick="ActionHandler" CommandName="DETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblPage" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-colapse" id="divAdvanceSearch">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:ShowFilter%>"
                                                TabIndex="2" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>"
                                                TabIndex="2" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7 ">
                                            <asp:Label ID="lblfilterBranch" runat="server" Text="<%$ resources:BrLoc%>" AssociatedControlID="txtFilterBranch"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterBranch" Text="" TabIndex="1" CssClass="select-small-i"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterBranch" Value="-1" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7 ">
                                            <asp:Label ID="lblFilterDept" runat="server" Text="<%$ resources:Dept%>" AssociatedControlID="txtFilterDept"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterDept" Text="" TabIndex="2" CssClass="select-small-i"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterDept" Value="-1" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblFilterFromDate" runat="server" Text="<%$ resources:FromDate%>"
                                                AssociatedControlID="txtFilterFromDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterFromDate" TabIndex="3" CssClass="input-small margnbotm0"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblFilterToDate" runat="server" Text="<%$ resources:ToDate%>" AssociatedControlID="txtFilterToDate"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterToDate" TabIndex="4" CssClass="input-small margnbotm0"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblTrxNoSearch" runat="server" Text="<%$ resources:TrxNo%>" AssociatedControlID="txtTrxNo"></asp:Label>
                                            <asp:TextBox ID="txtTrxNo" runat="server" CssClass="input-small-b1 margnbotm0" TabIndex="5" Width="24.5%"></asp:TextBox>
                                            <asp:HiddenField ID="hdfTrxPk" runat="server" />
                                            <asp:Label ID="lblEmployeeSearch" runat="server" Text="<%$ resources:Employee%>"
                                                AssociatedControlID="txtEmployeeSearch" CssClass="lbl-9perc"></asp:Label>
                                            <asp:TextBox ID="txtEmployeeSearch" runat="server" TabIndex="6" CssClass="input-w21-6per margnbotm0"
                                                MaxLength="200"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfEmployeeSearch" runat="server" Value="0" />
                                            <asp:ImageButton ID="imgListFilter" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="7"
                                                CommandName="FILTER" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="imgListClear" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="8" OnClick="ActionHandler"
                                                CommandName="CLEARSEARCH" SkinID="clear-ext" CssClass="margntop2 margnlft-minus2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdList" Width="100%" AllowPaging="false" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" OnSorting="ActionHandler" TabIndex="13">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" TabIndex="9"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfRowNoListPage" Value='<%# Eval("ROW_NO") %>' />
                                                <asp:HiddenField runat="server" ID="hdfEOLPKList" Value='<%# Eval("EOL_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TrxNo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("EOL_DOC_NO")))?Resources.ErpRes.Draft:Eval("EOL_DOC_NO")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("EOL_DOC_NO")))?Resources.ErpRes.Draft:Eval("EOL_DOC_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEOL_DATE" runat="server" Text='<%#Eval("EOL_DATE", Resources.Constants.HRMSDateFormatGrid) %>'
                                                    ToolTip='<%# Eval("EOL_DATE", Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Branch/Location%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEOL_BRANCH_TEXT" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EOL_BRANCH_TEXT")),35) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EOL_BRANCH_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                            <HeaderStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Department%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblempDepartment_TEXT" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EOL_DEPT_TEXT")),35) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EOL_DEPT_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                            <HeaderStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Remarks %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEOL_REMARKS" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("EOL_REMARKS"),60) %>'
                                                    ToolTip='<%# Eval("EOL_REMARKS") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" TabIndex="4" />
                                <div class="clear">
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblTrxNoHdr" runat="server" Text="<%$ resources:TrxNo%>" AssociatedControlID="lblTrxNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblTrxNo" CssClass="input-small"></asp:Label>
                                            <asp:Label ID="lblDate" runat="server" Text="<%$ resources:DateStar%>" AssociatedControlID="txtDate"
                                                CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:TextBox ID="txtDate" runat="server" MaxLength="200" CssClass="input-small" TabIndex="12"
                                                onblur="ClearDateSelect()" onkeydown="return CheckKey(event)" onpaste="return false;" />
                                            <asp:RequiredFieldValidator ID="vrfDate" CssClass="star" SetFocusOnError="true" ValidationGroup="save"
                                                EnableClientScript="true" runat="server" ControlToValidate="txtDate" Display="Dynamic"
                                                Text="*" ErrorMessage="<%$ resources:Err_Date %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCompanyHd" runat="server" Text="<%$ resources:Controls,CompanyReq%>"
                                                AssociatedControlID="ddlCompanyHd"></asp:Label>
                                            <asp:DropDownList ID="ddlCompanyHd" runat="server" TabIndex="13" CssClass="select-w61per" onchange="HideTextBox(this);">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvCompanyHd" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlCompanyHd" Display="Dynamic" Text="*" InitialValue="-1"
                                                ValidationGroup="save" ErrorMessage="<%$ resources:Err_SelectCompanay %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblHdBranchLocation" runat="server" Text="<%$ resources:Branch/Location%>"
                                                AssociatedControlID="txtHdBranchLocation"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtHdBranchLocation" Text="" TabIndex="14" CssClass="select-half"></asp:TextBox>
                                            <asp:HiddenField ID="hdfHdBranchLocation" Value="-1" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblHdDepartment" runat="server" Text="<%$ resources:Department%>"
                                                AssociatedControlID="txtHdDepartment"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtHdDepartment" Text="" TabIndex="15" CssClass="select-half"></asp:TextBox>
                                            <asp:HiddenField ID="hdfHdDepartment" Value="-1" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">

                                            <asp:Label ID="lblDesignation" runat="server" Text="<%$ resources:Designation%>"
                                                AssociatedControlID="txtDesignation"></asp:Label>
                                            <asp:HiddenField ID="hdfDesignation" runat="server" Value="-1" />
                                            <asp:TextBox runat="server" TabIndex="16" ID="txtDesignation" CssClass="select-half" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblEmployee" runat="server" Text="<%$ resources:Employee%>" AssociatedControlID="txtEmployee"></asp:Label>
                                            <asp:TextBox ID="txtEmployee" runat="server" TabIndex="17" CssClass="input-half" MaxLength="100"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfEmployee" runat="server" Value="-1" />
                                            <asp:ImageButton ID="btnGo" runat="server" Text="<%$ resources:Controls,Search%>"
                                                ToolTip="<%$ resources:Controls,Search%>" OnClientClick="javascript:ValidatePage('save')"
                                                ValidationGroup="save" OnClick="ActionHandler" TabIndex="18" CommandName="GO"
                                                SkinID="search-ext" CssClass="margntop2 margnbotm0 margn-rgt4" />
                                            <asp:ImageButton ID="btnClearDetails" runat="server" Text="<%$ resources:Controls,Clear%>"
                                                ToolTip="<%$ resources:Controls,Clear%>" TabIndex="19" OnClick="ActionHandler"
                                                CommandName="CLEARDETAIL" SkinID="clear-ext" CssClass="margntop2 margnbotm0 margn-rgt4" />
                                        </div>
                                    </td>

                                </tr>
                            </table>

                            <div class="clear">
                            </div>
                            <div class="employ-time">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S txtAlign-left">
                                                <asp:Label runat="server" ID="lblYearly" Text="Yearly"
                                                    AssociatedControlID="lblYearly"></asp:Label>
                                                <asp:CheckBox ID="chkYearly" TabIndex="20" runat="server" OnCheckedChanged="ActionHandler" AutoPostBack="true" Checked="true" />
                                                <asp:Label runat="server" ID="lblMonthly" CssClass="lbl-9perc" Text="Monthly"
                                                    AssociatedControlID="lblMonthly"></asp:Label>
                                                <asp:CheckBox ID="chkMonthly" TabIndex="21" runat="server" OnCheckedChanged="ActionHandler" AutoPostBack="true" />
                                                <asp:TextBox ID="txtMonth" runat="server" CssClass="input-w10per" TabIndex="22"
                                                    onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" AutoPostBack="true"></asp:TextBox>
                                                <cc1:CalendarExtender runat="server" ID="txtMonth_CalendarExtender" BehaviorID="calendar1"
                                                    TargetControlID="txtMonth" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                                                    ClientIDMode="Static" OnClientHidden="onCalendarHidden">
                                                </cc1:CalendarExtender>
                                                <%--<asp:RequiredFieldValidator ID="rfvMonth" CssClass="star" SetFocusOnError="true"
                                                    runat="server" ControlToValidate="txtMonth" Display="Dynamic" Text="*" EnableClientScript="true"
                                                    ValidationGroup="Add" ErrorMessage="<%$ resources:Err_SelectMonth %>" Enabled="false">
                                                </asp:RequiredFieldValidator>--%>

                                                <asp:Label runat="server" ID="lblSpecialHoliday" CssClass="lbl-31-7perc" Text="Special Holiday In Month"
                                                    AssociatedControlID="lblSpecialHoliday"></asp:Label>
                                                <asp:CheckBox ID="chkSpecilHoliday" TabIndex="23" runat="server" OnCheckedChanged="ActionHandler" AutoPostBack="true" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S txtAlign-left">
                                                <asp:DropDownList ID="ddlQuarter" runat="server" TabIndex="24" CssClass="ddl-dropdown">
                                                </asp:DropDownList>
                                                <%--<asp:RequiredFieldValidator ID="rfvQuarter" CssClass="star" SetFocusOnError="true" EnableClientScript="true"
                                                    runat="server" ControlToValidate="ddlQuarter" Display="Dynamic" Text="*" InitialValue="-1"
                                                    ValidationGroup="Add" ErrorMessage="<%$ resources:Err_SelectQuarter %>" Enabled="false">
                                                </asp:RequiredFieldValidator>--%>
                                            </div>
                                        </td>
                                    </tr>

                                </table>
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S txtAlign-left">
                                                <div class="date-strip">
                                                    <asp:Label runat="server" ID="lblSun" Text="Sun"
                                                        AssociatedControlID="lblSun"></asp:Label>
                                                    <asp:CheckBox ID="chkSun" TabIndex="25" runat="server" OnCheckedChanged="ActionHandler" AutoPostBack="true" />
                                                    <asp:Label runat="server" CssClass="lbl-6-3perc" ID="lblMon" Text="Mon"
                                                        AssociatedControlID="lblMon"></asp:Label>
                                                    <asp:CheckBox ID="chkMon" TabIndex="26" runat="server" OnCheckedChanged="ActionHandler" AutoPostBack="true" />
                                                    <asp:Label runat="server" CssClass="lbl-6-3perc" ID="lblTue" Text="Tue"
                                                        AssociatedControlID="lblTue"></asp:Label>
                                                    <asp:CheckBox ID="chkTue" TabIndex="27" runat="server" OnCheckedChanged="ActionHandler" AutoPostBack="true" />
                                                    <asp:Label runat="server" CssClass="lbl-6-3perc" ID="lblWed" Text="Wed"
                                                        AssociatedControlID="lblWed"></asp:Label>
                                                    <asp:CheckBox ID="chkWed" TabIndex="28" runat="server" OnCheckedChanged="ActionHandler" AutoPostBack="true" />
                                                    <asp:Label runat="server" CssClass="lbl-6-3perc" ID="lblThur" Text="Thu"
                                                        AssociatedControlID="lblThur"></asp:Label>
                                                    <asp:CheckBox ID="chkThu" TabIndex="29" runat="server" OnCheckedChanged="ActionHandler" AutoPostBack="true" />
                                                    <asp:Label runat="server" CssClass="lbl-6-3perc" ID="lblFri" Text="Fri"
                                                        AssociatedControlID="lblFri"></asp:Label>
                                                    <asp:CheckBox ID="chkFri" TabIndex="30" runat="server" OnCheckedChanged="ActionHandler" AutoPostBack="true" />
                                                    <asp:Label runat="server" CssClass="minwidth-7" ID="lblSat" Text="Sat"
                                                        AssociatedControlID="lblSat"></asp:Label>
                                                    <asp:CheckBox ID="chkSat" TabIndex="31" runat="server" OnCheckedChanged="ActionHandler" AutoPostBack="true" />
                                                    <asp:ImageButton runat="server" ID="btnAddItem" CommandName="ADD" TabIndex="32"
                                                        OnClick="ActionHandler" OnClientClick="ValidatePageNow('Add');"
                                                        ToolTip="<%$resources:ErpRes,Apply %>" CommandArgument="PageAction_Entry" ValidationGroup="Add"
                                                        SkinID="plus" />
                                                    <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear%>"
                                                        ToolTip="<%$ resources:Controls,Clear%>" TabIndex="33" OnClick="ActionHandler"
                                                        CommandName="CLEARADD" SkinID="clear-ext" CssClass="margnbotm0 margn-rgt4" />
                                                </div>
                                            </div>
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clear">
                            </div>

                            <div class="gridwrap">
                                <%--class="grdTable"--%>
                                <asp:GridView runat="server" ID="grdOTList" Width="100%" AllowPaging="false" AllowSorting="True"
                                    AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-CssClass="emptytable"
                                    OnRowCommand="ActionHandler" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField Visible="false">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkEmpHeader" runat="server" ToolTip="Select All Employee" TabIndex="15" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox CssClass="checkbox" runat="server" ID="chkEmpselect" Checked='<%# (Convert.ToInt32(Eval("EOT_PK")) > 0) ? true : false %>'
                                                    TabIndex="8" Enabled='<%# (Convert.ToInt32(Eval("EOT_PK")) > 0) ? false : true %>' />

                                            </ItemTemplate>
                                            <ItemStyle Width="2%" Wrap="false" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" Text="" runat="server" Checked='<%# (Convert.ToInt32(Eval("IsChecked")) > 0? true : (Eval("Pk")!=null?(GetInt(Eval("Pk").ToString())>0?true:false):(false))) %>'
                                                    TabIndex="8" />
                                                <asp:HiddenField runat="server" ID="hdfEOT_PK" Value='<%# Eval("EOT_PK") %>' />
                                                <asp:HiddenField ID="hdfEmployeePk" runat="server" Value='<%# Eval("EOT_EMPLOYEE_PK") %>' />
                                                <asp:HiddenField ID="hdfCheckedFlag" runat="server" Value='<%# Eval("CheckedFlag") %>' />
                                                <asp:HiddenField ID="hdfModDate" runat="server" Value='<%# Eval("EOT_MOD_DT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:EmployeeCode%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EOT_EMPLOYEE_CODE")),100) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EOT_EMPLOYEE_CODE")))%>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfEOT_PK" Value='<%# Eval("EOT_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfEOE_PK" Value='<%# Eval("EOE_PK") %>' />
                                                <asp:HiddenField ID="hdfEmployeePk" runat="server" Value='<%# Eval("EOT_EMPLOYEE") %>' />
                                                <asp:HiddenField ID="hdfModDate" runat="server" Value='<%# Eval("EOT_MOD_DT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="35%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:EmployeeName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocationGridView" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("EOT_EMPLOYEE_NAME") ,18) %>'
                                                    ToolTip='<%# Eval("EOT_EMPLOYEE_NAME")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:LeaveType %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLeaveType" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString( Eval("LeaveType") ,18)%>'
                                                    ToolTip='<%# Eval("LeaveType")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SpecialHoliday %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSpecialHoliday" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString( Eval("SpecialHoliday") ,18) %>'
                                                    ToolTip='<%# Eval("SpecialHoliday")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" Wrap="false" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="<%$ resources:Action %>">
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$resources:ErpRes,Edit %>" OnClick="ActionHandler"
                                                    TabIndex="14" SkinID="imbeditgrid" CommandName="EDITITEM" CommandArgument='<%# Eval("EOT_EMPLOYEE") %>' Visible="false" />
                                                <asp:ImageButton runat="server" TabIndex="34" ID="imbDelete" ToolTip="<%$Resources:Controls,Delete %>" OnClick="ActionHandler"
                                                    SkinID="imbdeletegrid" CommandName="GRIDDELETE" CommandArgument='<%# Eval("EOT_EMPLOYEE") %>' OnClientClick="return ShowDeleteConfirm(this);" />

                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="clear">
                            </div>
                            <div id="divImportSec" runat="server" style="display: none;">
                                <div class="search-colapse-b">
                                    <h1>
                                        <asp:Literal ID="litEmployeeAddDedImport" runat="server" Text="<%$ resources: EmployeeImport %>" /></h1>
                                    <asp:ImageButton runat="server" ID="imbShowImport" OnClientClick="javascript:return ShowHideImport(1);"
                                        SkinID="imbArrowShow" ToolTip="<%$ resources:Controls,ShowDetails%>" TabIndex="35"/>
                                    <asp:ImageButton runat="server" ID="imbHideImport" OnClientClick="javascript:return ShowHideImport();"
                                        Style="display: none" SkinID="imbArrowHide" ToolTip="<%$ resources:Controls,HideDetails%>" TabIndex="35"/>
                                    <asp:HiddenField ID="hdfImportVisible" runat="server" Value="0" />
                                    <div class="clear">
                                    </div>
                                </div>
                                <div id="divImport" style="display: none;">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-S padgtop7 padgbotm3 margnbotm5">
                                                    <asp:UpdatePanel ID="aupdpnlImport" runat="server">
                                                        <ContentTemplate>

                                                            <asp:Label runat="server" ID="lblMonth" Text="<%$ resources: Month %>"
                                                                AssociatedControlID="txtMon"></asp:Label>
                                                            <asp:TextBox ID="txtMon" runat="server" CssClass="input-w10per" TabIndex="36"
                                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" AutoPostBack="true"></asp:TextBox>
                                                            <cc2:CalendarExtender runat="server" ID="txtMon_CalendarExtender" BehaviorID="calendar2"
                                                                TargetControlID="txtMon" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                                                                ClientIDMode="Static" OnClientHidden="onCalendarHidden">
                                                            </cc2:CalendarExtender>
                                                            <asp:Label runat="server" ID="lblSourceFile" Text="<%$ resources: SourceFileStar %>"
                                                                AssociatedControlID="fupImport" CssClass="lbl-12perc"></asp:Label>
                                                            <div class="fileupload-main">
                                                                <asp:FileUpload ID="fupImport" runat="server" TabIndex="37" CssClass="margnbotm0 margn-rgt0 upload-area3" />
                                                                <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star input-w27per" SetFocusOnError="true"
                                                                    ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupImport"
                                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_File_Upload %>">
                                                                </asp:RequiredFieldValidator>
                                                            </div>
                                                            <asp:Button runat="server" ID="btnImport" CommandName="IMPORT" TabIndex="38" Text="<%$resources:Import %>"
                                                                OnClick="ActionHandler" ToolTip="<%$resources:Import %>" SkinID="btnInner-add"
                                                                ValidationGroup="SaveAddDed" OnClientClick="javascript:ValidatePage('SaveAddDed')"
                                                                Style="margin-bottom: 3px !important;" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnImport" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S txt-rgt">
                                                    <a id="aTmpDwn" class="btnInner-dwn btnInner-med-size decoration-none margnrgt13-5per"
                                                        href='<%= Page.ResolveClientUrl((string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()) ? "~/Upload/" : System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() )+ "Template/" + GetGlobalResourceObject("ConfigurationsRes", "HrmsImportTemplateAddDed").ToString())%>'>
                                                        <%= Resources.Controls.Template.ToString() %>
                                                    </a>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </asp:TableCell></asp:TableRow></asp:Table></div><div id="diverror" style="display: none">
                <asp:ValidationSummary
                    ID="ValidationSummary1" ValidationGroup="Add" runat="server" />
                <asp:ValidationSummary ID="vsPage" ValidationGroup="save" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:HiddenField
                    ID="hdfIscontYes" runat="server" />
            </div>
            <asp:HiddenField ID="hdfAdvSearch" Value="0" runat="server" />
            <asp:HiddenField runat="server" ID="hdfOTEntry" Value="1" />
            <asp:HiddenField runat="server" ID="hdfShowHideFilterSec" Value="0" />
            <asp:HiddenField ID="hdfNextYearDate" runat="server" Value="" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
