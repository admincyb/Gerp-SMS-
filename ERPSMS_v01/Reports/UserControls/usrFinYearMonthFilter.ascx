<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="usrFinYearMonthFilter.ascx.cs" Inherits="ERPSMS_v01.Reports.UserControls.usrFinYearMonthFilter" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript">
    function usrInitComponents() {
        GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
        RestrictDate();
    }
    function RestrictDate() {
        GrandScriptUtils.RestrictedDatePicker("txtFromDate", false, true, true, $("[id$=txtFrom]").val(), $("[id$=txtTo]").val());
        GrandScriptUtils.RestrictedDatePicker("txtToDate", false, true, true, $("[id$=txtFrom]").val(), $("[id$=txtTo]").val());
    }
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
        
    }
</script>

<div id="divUsrYearMonth" runat="server">
    <table class="table-devide tablelayout">
        <tr>
            <td width="50%">
                <div class="padgtop7">
                    <div id="divDate" runat="server">
                        <asp:Label ID="Label2" runat="server" Text="<%$ resources:MISFilterLabel,Finyear %>" CssClass="middle-lbl-xsmall-c4"
                            AssociatedControlID="ddlFinYear"></asp:Label>
                        <asp:DropDownList ID="ddlFinYear" runat="server" CssClass="medium"
                            OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                        </asp:DropDownList>

                        <asp:TextBox ID="txtMonth" runat="server" TabIndex="2" CssClass="input-small margnbotm0"
                            MaxLength="17" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                        <cc1:CalendarExtender runat="server" ID="txtMonth_CalendarExtender" BehaviorID="calendar2"
                            TargetControlID="txtMonth" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                            OnClientHidden="onCalendarHidden" ClientIDMode="Static">
                        </cc1:CalendarExtender>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>
<div style="display: none">
    <asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtTo" runat="server"></asp:TextBox>
</div>
