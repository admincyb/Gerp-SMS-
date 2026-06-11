<%@ Page Title="<%$ Resources:Captions,Title_BrandRates %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    EnableEventValidation="true" AutoEventWireup="true" Theme="ClassicExt" CodeBehind="BrandPriceList.aspx.cs"
    Inherits="CustomerPortal.OrderToCash.BrandPriceList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");

        function InitComponents() {
            if ($("[id$=hdfIsCustomerLog]").val() == "0")
                GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url, "hdfCustomer", true, true, "CUSTOMER");
            else if ($("[id$=hdfIsCustomerLog]").val() == "1")
                DisableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomer]"));
            //  GrandScriptUtils.AddDateRangeCommon("txtFromDt", "hdfFromDt", "txtToDt", "hdfToDt", false, false);
            // GrandScriptUtils
            InitCustomer();
        }

        function AfterAutoCompleteSelect(targetControlID) {
            //            if (targetControlID == "txtCustomer") {
            //                $("[id$=btnGODetails]").click();
            //            }
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
            ///<summary>
            /// Used to disable Autocomplete
            ///</summary>
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
            $(extender).autocomplete("option", "disabled", true);
            $(extender).attr("disabled", true);
        }
        function EnableAuto(extender) {
            ///<summary>
            /// Used to enable Autocomplete
            ///</summary>
            $(extender).removeAttr("disabled");
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect-disable").addClass("ddlSelect");
            $(extender).autocomplete("option", "disabled", false);
        }

        function InitCustomer() {
            var all = '<%= GetLocalResourceObject("All") %>';
            var cus = '<%= GetLocalResourceObject("Customer") %>';
            var pro = '<%= GetLocalResourceObject("Product")%>';
            var pageURL = window.document.URL;
            var virtualPath = $("[id$='hdfAbsolutePath']").val();
            var urlauto = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomerPrint", urlauto + "?CustomerID=0&BrandID=" + $("[id$=hdfbrandRatePK]").val(), "hdfCustomerPrint", true, true, "BRANDCUSTOMER");
            GrandScriptUtils.MakeAutoCompleteDDL("txtProduct", urlauto + "?itemPK=0&BrandID=" + $("[id$=hdfbrandRatePK]").val(), "hdfProduct", true, true, "BRANDPRODUCT");
            if ($("[id$='ddlPrint']").val() == '1') {
                $("[id$='lblBProductCustomer']").html(cus);
                $("[id$=txtCustomerPrint]").show();
                if ($("[id$=txtCustomerPrint]").attr("disabled") == false) {
                    $("[id$=txtCustomerPrint]").val(all);
                }
                $("[id$=txtCustomerPrint]").next($(".ddlSelect")).show();
                $("[id$=txtProduct]").hide();
                $("[id$=txtProduct]").next($(".ddlSelect")).hide();
            }
            else {
                $("[id$='lblBProductCustomer']").html(pro);
                $("[id$=txtCustomerPrint]").hide();
                $("[id$=txtCustomerPrint]").next($(".ddlSelect")).hide();
                $("[id$=txtProduct]").show();
                $("[id$=txtProduct]").val(all);
                $("[id$=txtProduct]").next($(".ddlSelect")).show();
            }
            if ($("[id$=txtCustomerPrint]").attr("disabled") == true) {
                DisableAuto($("[id$=txtCustomerPrint]"), $("[id$=hdfCustomerPrint]"));
            }

        }

        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {

            if (targetControlID == "txtCustomerPrint") {
                $("[id$=txtCustomerPrint]").attr("title", $("[id$=txtCustomerPrint]").val());
            }
            else if (targetControlID == "txtProduct") {
                $("[id$=txtProduct]").attr("title", $("[id$=txtProduct]").val());
            }
        }

        function SelectText(obj) {
            var all = '<%= GetLocalResourceObject("All") %>';
            $(obj).val(all);
        }

        function SelectCustomerProduct() {
            var all = '<%= GetLocalResourceObject("All") %>';
            var cus = '<%= GetLocalResourceObject("Customer") %>';
            var pro = '<%= GetLocalResourceObject("Product")%>';
            if ($("[id$='ddlPrint']").val() == '1') {
                $("[id$='lblBProductCustomer']").html(cus);
                $("[id$=txtCustomerPrint]").show();
                $("[id$=txtCustomerPrint]").val(all);
                $("[id$=hdfCustomerPrint]").val("0");
                $("[id$=txtCustomerPrint]").next($(".ddlSelect")).show();
                $("[id$=txtProduct]").hide();
                $("[id$=txtProduct]").next($(".ddlSelect")).hide();
            }
            else {
                $("[id$='lblBProductCustomer']").html(pro);
                $("[id$=txtCustomerPrint]").hide();
                $("[id$=txtCustomerPrint]").next($(".ddlSelect")).hide();
                $("[id$=txtProduct]").show();
                $("[id$=txtProduct]").val(all);
                $("[id$=hdfProduct]").val("0");
                $("[id$=txtProduct]").next($(".ddlSelect")).show();
            }
        }

        //Validate month Range
        function CheckMonthRange(oSrc, args) {
            var Month1 = $('input:text[id$=txtFromDt]').val();
            var Month2 = $('input:text[id$=txtToDt]').val();
            var Month1Split = Month1.split('-');
            var Month2Split = Month2.split('-');
            if (parseInt(Month2Split[1]) > parseInt(Month1Split[1]))
                args.IsValid = true;
            else if (parseInt(Month2Split[1]) >= parseInt(Month1Split[1]) && MonthGreater(Month1Split[0], Month2Split[0]) == true || Month2 == '')
                args.IsValid = true;
            else
                args.IsValid = false;

        }
        //Check From Month Is Greater than To Month
        function MonthGreater(fromMonth, toMonth) {
            var Months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
            var index1, index2;
            index1 = 0;
            index2 = 0;
            for (var i = 0; i < 12; i++) {
                if (Months[i] == fromMonth)
                    index1 = i;
                if (Months[i] == toMonth)
                    index2 = i;

            }
            return index2 >= index1 ? true : false;
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
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
        function ValidateAndConfirm(btn, valGroup) {
            if (ValidatePageNow(valGroup)) {
                ShowDeleteConfirm(btn, '<%= Resources.ErpRes.MsgCopyConfirm %>');
            }
            return false;

        }

  

 

    </script>
    <script type="text/javascript">

        $(window).load(function EndRequest() {
            FormatCalendar('4');

            // HideFilter();
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
        function AfterClose(containerID) {
            if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
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
    <asp:UpdatePanel runat="server" ID="aupdpnlBrandPriceList">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Captions", "AdvanceSearch").ToString() %></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="65" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="66" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table class="table-3devide" id="tbladvancedSearch" style="margin-top: 8px;">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCustomer" runat="server" Text="<%$resources:Customer %>" AssociatedControlID="txtCustomer" Visible="false"></asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="large" MaxLength="100" TabIndex="1" Visible="false"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomer" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdfIsCustomerLog" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblFromDt" runat="server" Text="<%$resources:FromMonth %>" AssociatedControlID="txtFromDt"></asp:Label>
                                            <asp:TextBox ID="txtFromDt" runat="server" TabIndex="1" CssClass="Uidate-picker"
                                                MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <cc1:CalendarExtender ID="txtCalender_CalendarExtenderFrom" runat="server" BehaviorID="calendar1"
                                                TargetControlID="txtFromDt" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                                                ClientIDMode="Static" OnClientHidden="onCalendarHidden">
                                            </cc1:CalendarExtender>
                                            <asp:HiddenField ID="hdfFromDt" runat="server" />
                                            <asp:CustomValidator ID="csvMonthFrom" runat="server" Display="None" Text="*" ControlToValidate="txtFromDt"
                                                ClientValidationFunction="CheckMonthRange" ErrorMessage="<%$resources:Msg_Err_Month %>"
                                                ValidationGroup="DateCheck"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="vrftxtCalender" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="DateCheck" EnableClientScript="true" runat="server" ControlToValidate="txtFromDt"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Month %>">
                                            </asp:RequiredFieldValidator>
                                            <%--<asp:Button ID="btnGODetails" runat="server" OnClick="ActionHandler" ToolTip="<%$resources:Msg_Err_Month %>" CommandName="SHOW" SkinID="btnInner-Print" />--%>

                                            <asp:Label ID="lblToDt" runat="server" Text="<%$resources:ToMonth %>" CssClass="lbl-14-2perc" AssociatedControlID="txtToDt"></asp:Label>
                                            <asp:TextBox ID="txtToDt" runat="server" TabIndex="1" CssClass="Uidate-picker" MaxLength="11"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <cc1:CalendarExtender ID="txtCalender_CalendarExtenderTo" runat="server" BehaviorID="calendar2"
                                                TargetControlID="txtToDt" Format="MMM-yyyy" OnClientShown="onCalendarShown" ClientIDMode="Static"
                                                OnClientHidden="onCalendarHidden">
                                            </cc1:CalendarExtender>
                                            <asp:HiddenField ID="hdfToDt" runat="server" />
                                         
                                            <asp:Label ID="lblSearch" CssClass="middle-lbl-xsmall-d style-none" runat="server" AssociatedControlID="btnSearch"></asp:Label>
                                            <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$ resources:Controls,Search %>" ValidationGroup="Search" OnClick="ActionHandler"
                                                OnClientClick="javascript:ValidatePageNow('DateCheck')" TabIndex="14" CommandName="SEARCH"
                                                 SkinID="search-ext" />
                                            <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear %>" TabIndex="15"
                                                OnClick="ActionHandler" ToolTip="<%$ resources:Controls,Clear %>" CommandName="CLEAR"
                                                 SkinID="clear-ext" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">                                            
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <h4>
                                <%=GetLocalResourceObject("BrndPriceList").ToString()%>
                            </h4>
                            <div class="gridwrap">
                                <asp:GridView ID="grdBrandPriceList" runat="server" AutoGenerateColumns="False" PageSize="<%$ resources:PageSize %>"
                                    AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                    Width="100%">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:BrhDate %>" SortExpression="BRH_DATE_FROM">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFromDate" runat="server" Text='<%# Eval("BRH_DATE_FROM", "{0:MMM-yyyy}") %>'
                                                    ToolTip='<%# Eval("BRH_DATE_FROM", "{0:MMM-yyyy}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="35%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:UpdatedDate %>" SortExpression="BRH_MOD_DT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUpdatedDate" runat="server" Text='<%# Eval("BRH_MOD_DT", Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval("BRH_MOD_DT", Resources.Constants.DateFormatGrid) %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="35%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Status %>" SortExpression="BRH_STATUS_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("BRH_STATUS_TEXT") %>' ToolTip='<%# Eval("BRH_STATUS_TEXT")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Print %>">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnPrint" runat="server" OnClick="ActionHandler" CommandName="PRINTGRID"
                                                    SkinID="btnPrint" ToolTip="<%$ resources:Print %>" />
                                                <asp:HiddenField runat="server" ID="hdfBrID" Value='<%# Eval("BRH_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="5%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <%--<uc1:PagerControl ID="uclPaging" runat="server" />--%>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="DateCheck" runat="server" />
                    <asp:HiddenField ID="hdfAppType" runat="server" />
                    <asp:HiddenField ID="hdfAppSubType" runat="server" />
                </div>
            </div>
            <%--Print popup window --%>
            <div id="divPrint" style="display: none" class="content-wrapper">
                <asp:HiddenField ID="hdfbrandRatePK" runat="server" />
                <asp:HiddenField ID="hdffromMonth" runat="server" />
                <table class="table-devide">
                    <tr>
                        <td align="right" style="width: 30%;">
                            <asp:Label runat="server" ID="lblPrint" Text="<%$ resources:GroupBy %>" AssociatedControlID="ddlPrint"></asp:Label>
                        </td>
                        <td align="left" style="width: 70%;">
                            <asp:DropDownList ID="ddlPrint" runat="server" Width="200px" CssClass="medium" TabIndex="101"
                                onchange="SelectCustomerProduct()">
                                <asp:ListItem Text="Customer" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Product" Value="2"></asp:ListItem>
                            </asp:DropDownList>
            </div>
            </td> </tr>
            <tr>
                <td align="right" style="width: 30%;">
                    <asp:Label runat="server" ID="lblBProductCustomer" Text="<%$ resources:Customer %>"
                        AssociatedControlID="txtCustomerPrint"></asp:Label>
                </td>
                <td align="left" style="width: 70%;">
                    <asp:TextBox ID="txtCustomerPrint" Width="270px" runat="server" MaxLength="100" TabIndex="102"
                        onchange="javascript:SelectText(this);"></asp:TextBox>
                    <asp:HiddenField ID="hdfCustomerPrint" runat="server" />
                    <asp:TextBox ID="txtProduct" runat="server" Width="270px" TabIndex="102" onchange="javascript:SelectText(this);"></asp:TextBox>
                    <asp:HiddenField ID="hdfProduct" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:Button runat="server" TabIndex="103" ID="btnPrintlist" Text="<%$resources:Controls,Print %>"
                        SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" CommandName="PRINT"
                        OnClick="ActionHandler" />
                    <asp:Button runat="server" ID="btnCancellist" Text="<%$resources:Controls,Cancel %>"
                        TabIndex="104" OnClientClick="javascript:ClosePopup();" SkinID="btnInner-Cancel"
                        ToolTip="<%$resources:Controls,Cancel %>" />
                    </div>
                </td>
            </tr>
            </table> </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
