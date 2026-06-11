<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="usrStockCardReportMonthFilter.ascx.cs" Inherits="ERPSMS_v01.Reports.UserControls.usrStockCardReportMonthFilter" %>

<%@ Register Src="~/UserControls/CheckListSearchControl.ascx" TagPrefix="uc1" TagName="CheckListSearchControl" %>
<%@ Register Src="~/UserControls/CheckListSearchControlNew.ascx" TagPrefix="uc2" TagName="CheckListSearchControlNew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript">
    function usrInitComponents() {
        //usrDateInit();
        //GrandScriptUtils.AddDateRangeCommon("FromDate", "hdfFromDate", "ToDate", "hdfToDate", false, false);
        GrandScriptUtils.RestrictedYearDatePicker("txtMonth", false, true, true, $("[id$=hdfFromDate]").val(), $("[id$=hdfToDate]").val());

    }
    function usrDateInit() {
        //<summary>function used to make datepicker</summary>
        GrandScriptUtils.DatePicker("FromDate", false, false);
        GrandScriptUtils.DatePicker("ToDate", false, false);
    }
    //Month picker start
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
    //Month picker end
        
</script>
<style>
    #ctl00_MainContent_userFilter_CheckListSearchControlNew_txtSearchItem + span {
        background: none;
        padding: 0;
        min-height: 0;
        border: none;
        margin-top: 2px !important;
    }

    #spnCount {
        display: none
    }

    #ctl00_MainContent_userFilter_CheckListSearchControl1_txtSearch + span {
        background: none;
        padding: 0;
        min-height: 0;
        border: none;
        margin-top: 2px !important;
    }

    .input-w81per {
        min-width: 79% !important;
        max-width: 79% !important;
    }

    .treelist-scroll {
        height: 100px;
        overflow: auto;
        margin-bottom: 10px;
        width: 362px;
    }
</style>

<asp:UpdatePanel ID="pnlTestFilter" runat="server" class="">
    <ContentTemplate>
        <div class="fields-grpwrap color-grey grp-before pad-t10 color-white">
            <div class="fields-group">
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <asp:Label ID="Label2" runat="server" Text="<%$ resources:MISFilterLabel,Finyear %>" AssociatedControlID="ddlFinYear"></asp:Label>
                                <asp:DropDownList ID="ddlFinYear" runat="server" CssClass="select-half"
                                    OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="rfvFinYear" InitialValue="-1" CssClass="star" SetFocusOnError="true"
                                    EnableClientScript="true" runat="server" ControlToValidate="ddlFinYear" ValidationGroup="fltr"
                                    Display="Static" Text="*" ErrorMessage="<%$ resources:MISFilterLabel,Err_SelectFinYear%>">
                                </asp:RequiredFieldValidator>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">
                                <asp:Label ID="lblMonth" runat="server" Text="<%$ resources:MISFilterLabel,Month %>" AssociatedControlID="txtMonth"></asp:Label>
                                <asp:TextBox ID="txtMonth" runat="server" TabIndex="2" CssClass="input-small margnbotm0"
                                    MaxLength="17" onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                <cc1:CalendarExtender runat="server" ID="txtMonth_CalendarExtender" BehaviorID="calendar2"
                                    TargetControlID="txtMonth" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                                    OnClientHidden="onCalendarHidden" ClientIDMode="Static">
                                </cc1:CalendarExtender>
                                <asp:HiddenField runat="server" ID="hdfFromDate" />
                                <asp:HiddenField runat="server" ID="hdfToDate" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <asp:Label ID="lblItemCategory" runat="server" Text="<%$ resources:MISFilterLabel,ItemCategory %>" AssociatedControlID="ddlItemCategory"></asp:Label>
                                <asp:DropDownList ID="ddlItemCategory" runat="server" CssClass="select-half"
                                    OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                </asp:DropDownList>

                                <%--<div class="tree-label2M w600">
                                    <label class="lbl-24perc float-left"><%=Resources.MISFilterLabel.Store %></label>
                                    <div>
                                        <asp:UpdatePanel ID="pnlStores" runat="server">
                                            <ContentTemplate>
                                                <uc1:CheckListSearchControl runat="server" ID="CheckListSearchControl1" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>--%>

                                <asp:Label ID="Label3" runat="server" Text="<%$ resources:MISFilterLabel,Store %>" AssociatedControlID="ddlStore"></asp:Label>
                                <asp:DropDownList ID="ddlStore" runat="server" CssClass="select-half"
                                    OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="vrfStore" CssClass="star" SetFocusOnError="true" ValidationGroup="fltr"
                                    EnableClientScript="true" InitialValue="-1" runat="server" ControlToValidate="ddlStore"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:MISFilterLabel,Err_Store %>">
                                </asp:RequiredFieldValidator>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">

                                <asp:Label ID="Label1" runat="server" Text="<%$ resources:MISFilterLabel,Classification %>" AssociatedControlID="ddlClassification"></asp:Label>
                                <asp:DropDownList ID="ddlClassification" runat="server" CssClass="select-half"></asp:DropDownList>

                                <asp:Label ID="lblTransaction" runat="server" Text="<%$ resources:MISFilterLabel,TransactionsOnly %>"
                                    CssClass="label-11-26" AssociatedControlID="chkTransaction"></asp:Label>
                                <asp:CheckBox ID="chkTransaction" runat="server" />

                                <asp:Label ID="lblExcludeMatReturn" runat="server" Text="<%$ resources:MISFilterLabel,ExcludeMaterialReturn %>"
                                    AssociatedControlID="chkExcludeMatReturn"></asp:Label>
                                <asp:CheckBox ID="chkExcludeMatReturn" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="div2col-S">                                
                                <div class="tree-label2M w600">
                                    <label class="lbl-24perc float-left"><%=Resources.MISFilterLabel.Items %></label>
                                    <div>
                                        <asp:UpdatePanel ID="pnlItemChecklist" runat="server">
                                            <ContentTemplate>
                                                <uc2:CheckListSearchControlNew runat="server" ID="CheckListSearchControlNew" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div> 
                            </div>
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="diverror" style="display: none">
            <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
            <asp:ValidationSummary ID="vsPage" ValidationGroup="fltr" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
