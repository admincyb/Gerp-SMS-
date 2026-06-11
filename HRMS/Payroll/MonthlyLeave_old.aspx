<%@ Page Title="<%$ Resources:Captions,Title_MonthlyLeave %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="MonthlyLeave_old.aspx.cs" Inherits="HRMS.Payroll.MonthlyLeave_old"
    Theme="ClassicExt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            $(document).ready(function () {
                $("[id$=btnDateChanged]").hide();
                GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee", url + "?EmpCategory=2", "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETE");
                GrandScriptUtils.DatePickerCommon("txtMonth");
            });
        }
        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtEmployee") {
                $("[id$=btnEmployee]").click();
            }
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtEmployee") {
                $("[id$=hdfEmployee]").val("0");
            }
        }

        function AfterDateSelect(controlID) {
            //            if (controlID == "txtMonth") {
            //                $("[id$=btnDateChanged]").click();
            //            }
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
                //                $("[id$='pnlDelete']").hide();
            }
            else if (mode == 2) {
                //                $("[id$=pnlDelete]").hide();
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
        function ValidateNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                CheckValidationDuplicate(valGroup); //For finding and removing duplicate and other group validation controls
                Page_ClientValidate(valGroup); //For Script validating the Page
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverrorAlert").html());
                return false; //Page is invalid -- stop right here
            }
            else {
                return true; //everythings ok --- Call your function & do your stuff
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
    <asp:UpdatePanel ID="aupdpnlTaskHome" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdfCurrentELD_PK" runat="server" />
            <asp:HiddenField ID="hdfCurrentROW_NO" runat="server" />
            <div class="fixed-buttons-normal">
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
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$resources:Controls,Save %>"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save"
                                            ValidationGroup="Save" OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('Save')"
                                            TabIndex="150" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            CssClass="popupclose" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" OnClick="ActionHandler"
                                            TabIndex="151" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="152" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="153" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <div class="tab-container-floating">
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
                                        <div class="div2col-S padgtop7 div-separatn">
                                            <asp:Label ID="Label1" runat="server" Text="<%$ resources:YearStar%>" AssociatedControlID="ddlYearListPage"></asp:Label>
                                            <asp:DropDownList ID="ddlYearListPage" runat="server" TabIndex="3" CssClass="select-medium margnbotm0">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="reqYearListPage" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlYearListPage" Display="Static" Text="*"
                                                InitialValue="0" ValidationGroup="Search" ErrorMessage="<%$ resources:Err_SelectYear %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Search%>" ToolTip="<%$ resources:Search%>"
                                                OnClick="ActionHandler" TabIndex="4" CommandName="FILTER" SkinID="search-ext"
                                                ValidationGroup="Search" OnClientClick="javascript:ValidateNow('Search')" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Clear%>" ToolTip="<%$ resources:Clear%>"
                                                TabIndex="5" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7 div-separatn">
                                            <asp:Label ID="lbldummy" runat="server" Text="" AssociatedControlID="ddlYearListPage"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdList" Width="100%" AllowPaging="false" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" OnSorting="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    TabIndex="6" />
                                                <asp:HiddenField runat="server" ID="hdfRowNoListPage" Value='<%# Eval("ROW_NO") %>' />
                                                <asp:HiddenField runat="server" ID="hdfYearListPage" Value='<%# Eval("ELD_YEAR_TEXT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Month%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMonthListPage" runat="server" Text='<%# GetSubstring(Eval("ELD_MONTH_TEXT")) + "-" + Eval("ELD_YEAR_TEXT")%>'
                                                    ToolTip='<%# GetSubstring(Eval("ELD_MONTH_TEXT")) + "-" + Eval("ELD_YEAR_TEXT")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TotalLeaves%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" Text='<%# GetFormattedNumber(Eval("ELD_LEAVE_COUNT")) %>'
                                                    ToolTip='<%# GetFormattedNumber(Eval("ELD_LEAVE_COUNT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
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
                                            <asp:Label ID="lblEmployee" runat="server" Text="<%$ resources:EmployeeStar%>" AssociatedControlID="txtEmployee"></asp:Label>
                                            <%-- <asp:DropDownList ID="ddlEmployee" runat="server" TabIndex="4" OnSelectedIndexChanged="ActionHandler"
                                                AutoPostBack="true" CssClass="select-half">
                                            </asp:DropDownList>--%>
                                            <asp:TextBox ID="txtEmployee" runat="server" TabIndex="1" CssClass="input-half" MaxLength="200"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfEmployee" runat="server" Value="0" />
                                            <asp:RequiredFieldValidator ID="reqEmployee" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="txtEmployee" Display="Dynamic" Text="*" ValidationGroup="AddToList"
                                                ErrorMessage="<%$ resources:Err_SelectEmployee %>" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Button ID="btnEmployee" runat="server" OnClick="ActionHandler" CommandName="CHANGEEMPLOYEE"
                                                EnableTheming="false" Style="display: none" />
                                            <asp:Button ID="btnChange" runat="server" OnClick="ActionHandler" CommandName="CHANGE"
                                                EnableTheming="false" Style="display: none" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="Label2" runat="server" Text="<%$ resources:LeaveTypeStar %>" AssociatedControlID="ddlLeaveType"></asp:Label>
                                            <asp:DropDownList ID="ddlLeaveType" runat="server" TabIndex="2" CssClass="select-small-c"
                                                AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>
                                            <%-- OnSelectedIndexChanged="ActionHandler"
                                                    AutoPostBack="true"
                                               <asp:Label ID="lblCurrentELH_LEAVE_BAL" runat="server" Text="CurrentELH_LEAVE_BAL"></asp:Label>--%>
                                            <asp:RequiredFieldValidator ID="reqLeaveType" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlLeaveType" Display="Static" Text="*" InitialValue="0"
                                                ValidationGroup="AddToList" ErrorMessage="<%$ resources:Err_SelectLeaveType %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label ID="lblBalance" runat="server" Text="<%$ resources:Balance %>" CssClass="middle-lbl-xsmall-a4"
                                                AssociatedControlID="txtBalance"></asp:Label>
                                            <asp:TextBox ID="txtBalance" runat="server" CssClass="input-small input-disabled"
                                                Enabled="false" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblMonth" runat="server" Text="<%$ resources:DateReq%>" AssociatedControlID="txtMonth"></asp:Label>
                                            <asp:TextBox ID="txtMonth" runat="server" MaxLength="200" CssClass="input-small"
                                                TabIndex="3" onkeydown="return CheckKey(event)" onpaste="return false;" />
                                            <%--<cc1:CalendarExtender runat="server" ID="txtMonth_CalendarExtender" BehaviorID="calendar1"
                                                TargetControlID="txtMonth" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                                                ClientIDMode="Static" OnClientHidden="onCalendarHidden">
                                            </cc1:CalendarExtender>--%>
                                            <asp:RequiredFieldValidator ID="rfvMonth" runat="server" ControlToValidate="txtMonth"
                                                CssClass="star" ValidationGroup="AddToList" Text="*" ErrorMessage="<%$ resources:Err_Date%>"></asp:RequiredFieldValidator>
                                            <asp:Label ID="lblNoOfLeaves" runat="server" Text="<%$ resources:NoOfLeavesStar%>"
                                                AssociatedControlID="txtNoOfLeaves" CssClass="lbl-19-6perc"></asp:Label>
                                            <asp:TextBox ID="txtNoOfLeaves" runat="server" CssClass="input-small numeric" TabIndex="4"
                                                onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);" MaxLength="4"
                                                onkeyup="limitText(this,10);" onkeydown="limitText(this,10);" onDrop="return false;"
                                                onPaste="return false;"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvNoOfLeaves" runat="server" ControlToValidate="txtNoOfLeaves"
                                                Display="Dynamic" CssClass="star" ValidationGroup="AddToList" Text="*" ErrorMessage="<%$ resources:Err_txtNoOfLeaves%>">
                                            </asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cmpNoOfLeaves" CssClass="star" SetFocusOnError="true" Enabled="false"
                                                runat="server" ControlToValidate="txtNoOfLeaves" ControlToCompare="txtBalance"
                                                Operator="LessThanEqual" Type="Double" Display="Dynamic" Text="*" ValidationGroup="AddToList"
                                                ErrorMessage="<%$ resources:Err_NoOfLeaves %>">
                                            </asp:CompareValidator>o
                                            <asp:RangeValidator ID="rngNoOfLeaves" runat="server" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="AddToList" EnableClientScript="true" Display="Dynamic" Text="*"
                                                MaximumValue="1000" ControlToValidate="txtNoOfLeaves" Type="Double" ErrorMessage="<%$ resources:Err_ZeroNoOfLeaves %>">
                                            </asp:RangeValidator>
                                            <asp:Button ID="btnDateChanged" Text="" runat="server" Style="display: none !important;"
                                                OnClick="ActionHandler" CommandName="CHANGE" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblRemarks" runat="server" Text="<%$ resources:Reason%>" AssociatedControlID="txtRemarks"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRemarks" MaxLength="500" TabIndex="5" CssClass="input-half"
                                                onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                            <asp:ImageButton ID="btnPlus2" runat="server" OnClick="ActionHandler" CommandName="ADD"
                                                ValidationGroup="AddToList" OnClientClick="javascript:ValidateNow('AddToList')"
                                                TabIndex="5" SkinID="imbaddnew" CssClass="margntop2" />
                                            <asp:ImageButton ID="imbDetSearch" runat="server" Text="<%$ resources:Search%>" ToolTip="<%$ resources:Search%>"
                                                OnClick="ActionHandler" TabIndex="5" CommandName="SEARCH" SkinID="search-ext"
                                                CssClass="margntop2" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <%--class="grdTable"--%>
                                <asp:GridView runat="server" ID="grdLeaveList" Width="100%" AllowPaging="false" AllowSorting="True"
                                    AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-CssClass="emptytable"
                                    OnRowCommand="ActionHandler" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Employee%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ELD_EMPLOYEE_TEXT")),49) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELD_EMPLOYEE_TEXT")))%>'></asp:Label>
                                                <asp:HiddenField ID="hdfELD_PK" runat="server" Value='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELD_PK")))%>' />
                                                <asp:HiddenField ID="hdfROW_NO" runat="server" Value='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ROW_NO")))%>' />
                                                <asp:HiddenField ID="hdfELD_EMPLOYEE" runat="server" Value='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELD_EMPLOYEE")))%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="38%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.ELD_MONTH, Resources.Constants.HRMSDateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.ELD_MONTH, Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWeekDay" runat="server" Text=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:LeaveType%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblELD_LEAVE_TYPE_CODE_TEXT" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ELD_LEAVE_TYPE_TEXT")),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELD_LEAVE_TYPE_TEXT")))%>'></asp:Label>
                                                <asp:HiddenField ID="hdfELD_LEAVE_TYPE" runat="server" Value='<%# Eval("ELD_LEAVE_TYPE")%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Reason%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdRemarks" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ELD_REMARKS")),50) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELD_REMARKS")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:NoOfLeaves%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblELD_LEAVE_COUNTGridview" runat="server" Text='<%# Eval("ELD_LEAVE_COUNT") %>'
                                                    ToolTip='<%# Eval("ELD_LEAVE_COUNT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_edit" runat="server" ID="imbEditDetails"
                                                    SkinID="imbeditgrid" EnableViewState="false" CommandName="EDIT_ACTION" TabIndex="6"
                                                    ToolTip="<%$resources:Controls,Edit %>" Visible='<%# string.IsNullOrEmpty(Convert.ToString(Eval("ELD_PAYROLL_DTL")))?true:false %>' />
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbDeleteDetails"
                                                    SkinID="imbdeletegrid" EnableViewState="false" ToolTip="<%$resources:Controls,Delete %>"
                                                    CommandName="DELETE_ACTION" OnClientClick="return ShowDeleteConfirm(this);" TabIndex="6"
                                                    Visible='<%# string.IsNullOrEmpty(Convert.ToString(Eval("ELD_PAYROLL_DTL")))?true:false %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" Wrap="false" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverrorAlert" style="display: none">
                <asp:ValidationSummary ID="vsListSearch" ValidationGroup="Search" runat="server" />
                <asp:ValidationSummary ID="vsAddToList" ValidationGroup="AddToList" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
            </div>
            <asp:HiddenField ID="hdfIscontYes" runat="server" />
            <asp:HiddenField ID="hdfNoOfLeaveValidation" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
