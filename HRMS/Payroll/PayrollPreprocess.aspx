<%@ Page Title="<%$ Resources:Captions,Title_Preprocess %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="PayrollPreprocess.aspx.cs" Inherits="HRMS.Payroll.PayrollPreprocess"
    ValidateRequest="false" Theme="ClassicExt" %>

<%--<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>--%>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="UserControls/PayrollTabControl.ascx" TagName="PayrollTabControl"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <style type="text/css">
        .dialogzone {
            width: 100% !important;
            overflow-x: hidden !important;
            overflow-y: auto !important;
        }

            .dialogzone table {
                width: auto !important;
                float: left;
            }

        .dlgBody img {
            margin: 0px !important;
        }


        .trElt img {
            margin-top: 3px !important;
        }

        .reportviewer span {
            display: inline;
        }

        .dialogbox span {
            margin: 3px 7px 0px 0px !important;
        }

        .clear {
            height: 0px !important;
        }
    </style>
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtProcessFromDate", "hdfProcessFromDate", "txtProcessToDate", "hdfProcessToDate", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtDepartment", url, "hdfDepartment", true, true, "DEPARTMENT");
            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee ", url + "?EmpCategory=2&EmpBranch=" + $("[id$=ddlBranchLocation]").val() + "&EmpType=" + $("[id$=ddlEmployeeType]").val() + "&EmploymentType=" + $("[id$=ddlEmploymentType]").val() + "&EmpCompany=" + $("[id$=ddlCompany]").val() + "&EmpDept=" + $("[id$=hdfDepartment]").val(), "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETE");
            ShowHideCrystalReportDiv();
        }

        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtDepartment") {
                InitEmployeeAuto();
            }
        }

        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtDepartment") {
                InitEmployeeAuto();
            }
        }

        function InitEmployeeAuto() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee ", url + "?EmpCategory=2&EmpBranch=" + $("[id$=ddlBranchLocation]").val() + "&EmpType=" + $("[id$=ddlEmployeeType]").val() + "&EmploymentType=" + $("[id$=ddlEmploymentType]").val() + "&EmpCompany=" + $("[id$=ddlCompany]").val() + "&EmpDept=" + $("[id$=hdfDepartment]").val(), "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETE");
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
                $("[id$='pnlListing']").show();
            }
            else {
                $("[id$='PageAction_List']").hide();
                $("[id$='pnlListing']").hide();
            }
            return false;
        }

        function ValidateNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverrorAlert").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }

        function ShowHideCrystalReportDiv() {
            if ($("[id$=hdfShowCrReportDiv]").val() == "0") {
                $("[id$=divCrystalReportViewer]").hide();
                $("[id$=divNodata]").show();
            }
            else {
                $("[id$=divCrystalReportViewer]").show();
                $("[id$=divNodata]").hide();
            }
            return false;
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

            }

        }
    </script>
    <script type="text/javascript">
        $(window).load(function EndRequest() {
            FormatCalendar('4');
        });
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
    <uc2:PayrollTabControl ID="PayrollTabControl1" runat="server" CurrentTab="1" />
    <div class="fixed-buttons">
        <div class="Button-container">
            <asp:Table ID="Table1" runat="server">
                <asp:TableRow>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul class="bredcrum">
                            <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                        </ul>
                        <ul runat="server" id="pnlListing" style="display: none">
                            <li>
                                <asp:Button runat="server" TabIndex="155" ID="btnView" CommandName="VIEW" OnClick="ActionHandler"
                                    Text="<%$resources:Controls,View %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                    ToolTip="<%$resources:Controls,View %>" ValidationGroup="ViewRpt" OnClientClick="javascript:ValidateNow('ViewRpt')" />
                                <li runat="server" id="pnlPrintAll">
                                    <asp:Button runat="server" TabIndex="155" ID="btnExport" CommandName="EXCELPRINT"
                                        OnClick="ActionHandler" Text="<%$resources:Export  %>"
                                        CommandArgument="SEC_ActionPanel" SkinID="btnInner-Excel" ToolTip="<%$resources:Export %>"
                                        OnClientClick="javascript:ValidateNow('ViewRpt')" ValidationGroup="ViewRpt" />

                                </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <asp:Table runat="server" ID="tblPage" CssClass="tablelayout asptbllinks">
            <asp:TableRow ID="PageAction_List" runat="server">
                <asp:TableCell>
                    <div class="clear">
                    </div>
                    <table class="table-devide tablelayout">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="lblSalMonth" runat="server" Text="<%$ resources:SalaryMonth%>" AssociatedControlID="txtSalaryMonth"
                                        CssClass="select-small-b"></asp:Label>
                                    <asp:TextBox ID="txtSalaryMonth" runat="server" CssClass="input-small" TabIndex="2"
                                        onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" AutoPostBack="true"
                                        OnTextChanged="ActionHandler"></asp:TextBox>
                                    <cc2:CalendarExtender runat="server" ID="txtSalaryMonth_CalendarExtender" BehaviorID="calendar1"
                                        TargetControlID="txtSalaryMonth" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                                        ClientIDMode="Static" OnClientHidden="onCalendarHidden">
                                    </cc2:CalendarExtender>
                                    <div class="starwrap">
                                        <asp:RequiredFieldValidator ID="rfvSalMonth" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="ViewRpt" EnableClientScript="true" runat="server" ControlToValidate="txtSalaryMonth"
                                            Display="Dynamic" Text="*" ErrorMessage="Select Salary Month">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <asp:Label ID="lblProcessMode" runat="server" Text="<%$ resources:ProcessMode%>"
                                        AssociatedControlID="ddlProcessMode" CssClass="lbl-25-5perc"></asp:Label>
                                    <asp:DropDownList ID="ddlProcessMode" runat="server" TabIndex="5" CssClass="select-small-a1"
                                        AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                    </asp:DropDownList>
                                    <div class="starwrap">
                                        <asp:RequiredFieldValidator ID="rfvMode" CssClass="star" SetFocusOnError="true" ValidationGroup="ViewRpt"
                                            EnableClientScript="true" InitialValue="-1" runat="server" ControlToValidate="ddlProcessMode"
                                            Display="Dynamic" Text="*" ErrorMessage="Select Process Mode">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="clear">
                                    </div>
                                    <asp:Label runat="server" ID="lblProcessFromDate" Text="<%$ resources:ProcessFromDate%>"
                                        AssociatedControlID="txtProcessFromDate" CssClass="select-small-b"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtProcessFromDate" CssClass="input-small" TabIndex="7"
                                        onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                    <asp:HiddenField ID="hdfProcessFromDate" runat="server" Value="" />
                                    <div class="starwrap">
                                        <asp:RequiredFieldValidator ID="rfvFromDate" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="ViewRpt" EnableClientScript="true" runat="server" ControlToValidate="txtProcessFromDate"
                                            Display="Dynamic" Text="*" ErrorMessage="Select From Date">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <asp:Label runat="server" ID="lblProcessToDate" Text="<%$ resources:ProcessToDate%>"
                                        AssociatedControlID="txtProcessToDate" CssClass="lbl-25-5perc"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtProcessToDate" CssClass="input-small" TabIndex="8"
                                        onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                    <asp:HiddenField ID="hdfProcessToDate" runat="server" Value="" />
                                    <div class="starwrap">
                                        <asp:RequiredFieldValidator ID="rfvToDate" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="ViewRpt" EnableClientScript="true" runat="server" ControlToValidate="txtProcessToDate"
                                            Display="Dynamic" Text="*" ErrorMessage="Select To Date">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="clear">
                                    </div>
                                    <asp:Label ID="lblBranchLocation" runat="server" Text="<%$ resources:BranchLocation%>"
                                        AssociatedControlID="ddlBranchLocation" CssClass="select-small-b"></asp:Label>
                                    <asp:DropDownList ID="ddlBranchLocation" runat="server" TabIndex="9" CssClass="select-w26-6per"
                                        onchange="javascript:InitEmployeeAuto();">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblEmploymentType" runat="server" Text="<%$ resources:EmploymentType%>"
                                        AssociatedControlID="ddlEmploymentType" CssClass="lbl-18-3perc"></asp:Label>
                                    <asp:DropDownList ID="ddlEmploymentType" runat="server" TabIndex="10" CssClass="select-small-a"
                                        onchange="javascript:InitEmployeeAuto();">
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="lblPayrollType" runat="server" Text="<%$ resources:PayrollType%>"
                                        AssociatedControlID="ddlPayrollType" CssClass="middle-lbl-xsmall-a1"></asp:Label>
                                    <asp:DropDownList ID="ddlPayrollType" runat="server" TabIndex="6" CssClass="select-small-c1"
                                        AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                    </asp:DropDownList>
                                    <div class="starwrap">
                                        <asp:RequiredFieldValidator ID="rfvType" CssClass="star" SetFocusOnError="true" ValidationGroup="ViewRpt"
                                            EnableClientScript="true" InitialValue="-1" runat="server" ControlToValidate="ddlPayrollType"
                                            Display="Dynamic" Text="*" ErrorMessage="Select Payroll Type">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <asp:Label runat="server" ID="lblEmployeeType" AssociatedControlID="ddlEmployeeType"
                                        Text="<%$resources:EmployeeType%>" CssClass="middle-lbl"></asp:Label>
                                    <asp:DropDownList runat="server" ID="ddlEmployeeType" CssClass="lbl-29perc" TabIndex="3"
                                        onchange="javascript:InitEmployeeAuto();">
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                    <asp:Label ID="lblDepartment" runat="server" Text="<%$ resources:Department%>" AssociatedControlID="txtDepartment"
                                        CssClass="middle-lbl-xsmall-a1"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtDepartment" Text="" TabIndex="4" CssClass="middle-lbl-d"></asp:TextBox>
                                    <asp:HiddenField ID="hdfDepartment" Value="" runat="server" />
                                    <asp:Label runat="server" ID="lblCompany" AssociatedControlID="ddlCompany" Text="<%$resources:Company%>"
                                        CssClass="lbl-18-3perc"></asp:Label>
                                    <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="1" CssClass="lbl-29perc"
                                        onchange="javascript:InitEmployeeAuto();">
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                    <asp:Label runat="server" ID="lblEmployee" Text="<%$ resources:Employee%>" AssociatedControlID="txtEmployee"
                                        CssClass="lbl-15-4perc"></asp:Label>
                                    <asp:TextBox ID="txtEmployee" runat="server" TabIndex="11" CssClass="input-w65-1per"
                                        MaxLength="100"> </asp:TextBox>
                                    <asp:HiddenField ID="hdfEmployee" runat="server" />
                                    <div class="display-inline">
                                        <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Search%>" ToolTip="<%$ resources:Search%>"
                                            OnClick="ActionHandler" TabIndex="12" CommandName="SEARCH" SkinID="search-ext"
                                            CssClass="margntop2" ValidationGroup="ViewRpt" OnClientClick="javascript:ValidateNow('ViewRpt')" />
                                        <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Clear%>" ToolTip="<%$ resources:Clear%>"
                                            TabIndex="13" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext"
                                            CssClass="margntop2" />
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="clear">
                    </div>
                    <div id="divNodata" class="nodata" runat="server" style="display: none;">
                        <%= GetLocalResourceObject("NoRecordFound").ToString() %>
                    </div>
                    <div class="clear">
                    </div>
                    <asp:HiddenField ID="hdfShowCrReportDiv" runat="server" Value="0" />
                    <div id="divCrystalReportViewer" runat="server">

                        <CR:CrystalReportViewer ID="GERP_OutputReport" HasToggleParameterPanelButton="false"
                            runat="server" AutoDataBind="true" HyperlinkTarget="_blank"  ToolPanelView="None" />

                        <div class="clear">
                        </div>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    <div id="diverrorAlert" style="display: none">
        <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
        <asp:ValidationSummary ID="vsPage" ValidationGroup="Save" runat="server" />
        <asp:ValidationSummary ID="vsView" ValidationGroup="ViewRpt" runat="server" />
        <asp:ValidationSummary ID="vsSearch" ValidationGroup="Search" runat="server" />
    </div>
    <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
    <asp:HiddenField ID="hdfCurrencyFormatWithComma" runat="server" />
    <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
    <asp:HiddenField ID="hdfMonthlyMode" Value="0" runat="server" />
</asp:Content>
