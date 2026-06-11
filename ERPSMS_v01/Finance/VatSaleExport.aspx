<%@ Page Title="<%$ Resources:Captions,Title_VatSaleExport %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="VatSaleExport.aspx.cs" Inherits="ERPSMS_v01.Finance.VatSaleExport" Theme="ClassicExt"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtSearchCustomer", url, "hdfSearchCustomerID", true, true, "VATSALEEXPORTCUSTOMER");
            GrandScriptUtils.MakeAutoCompleteDDL("txtSearchInvoiceNo", url, "hdfSearchInvoiceNo", true, true, "VATSALEEXPORTINVOICE");
        }

        var NumberDigits = 0;
        var CurrencyDigits = 0;
        $(document).ready(function () {
            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
        });
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
        function SetDate(controlID) {
            $("[id$=btnSet]").click();
        }
        $(document).ready(function () {

        });
    </script>
    <script type="text/javascript">
        $(window).load(function EndRequest() {
            FormatCalendar('4');

            HideFilter();
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
        function validateFloatKeyPress(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            var number = el.value.split('.');
            if (charCode == 8) {
                return true;
            }
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            currencyDecimal = 2;
            if (!isNaN(parseInt($("[id$=hdfDecimalDigits]").val()))) {
                currencyDecimal = parseInt($("[id$=hdfDecimalDigits]").val());
            }

            var caratPos = getSelectionStart(el);
            var dotPos = el.value.indexOf(".");
            if (caratPos > dotPos && dotPos > -1 && (number[1].length > currencyDecimal - 1)) {
                return false;
            }
            return true;
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
            if (!isNaN(parseInt($("[id$=hdfExchangeDigits]").val()))) {
                rateDecimal = parseInt($("[id$=hdfExchangeDigits]").val());
            }
            var caratPos = getSelectionStart(el);
            var dotPos = el.value.indexOf(".");
            if (caratPos > dotPos && dotPos > -1 && (number[1].length > rateDecimal - 1)) {
                return false;
            }
            return true;
        }
        function getSelectionStart(o) {
            if (o.createTextRange) {
                var r = document.selection.createRange().duplicate()
                r.moveEnd('character', o.value.length)
                if (r.text == '') return o.value.length
                return o.value.lastIndexOf(r.text)
            } else return o.selectionStart
        }
        function toFixed(num, precision) {
            return (+(Math.round(+(num + 'e' + precision)) + 'e' + -precision)).toFixed(precision);
        }
        function round(value, decimals) {
            return Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
        }
        function CalculateTotal(sender) {
            var valueFOB = parseFloat($("[id$=txtItemValueFOB]").val().replace(new RegExp(',', 'g'), ''));
            valueFOB = isNaN(valueFOB) ? 0 : valueFOB;
            var exchangeRate = parseFloat($("[id$=txtItemExchangeRate]").val().replace(new RegExp(',', 'g'), ''));
            exchangeRate = isNaN(exchangeRate) ? 0 : exchangeRate;
            var amountTHB = valueFOB * exchangeRate;
            amountTHB = round(amountTHB, CurrencyDigits);
            $("[id$=txtItemAmountTHB]").val((amountTHB).toFixed(CurrencyDigits));
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlVatSaleExport">
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btnClear"/>--%>
            <asp:AsyncPostBackTrigger ControlID="btnClear"/>
        </Triggers>
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="11" 
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Save %>"
                                            OnClientClick="javascript:ValidatePageNow('invoice')" ValidationGroup="monthView" 
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    
                                     <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                           CommandName="CANCEL" TabIndex="12" CommandArgument="SEC_ActionPanel"
                                           OnClick="ActionHandler" SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>"  />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnPopupSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                            OnClick="ActionHandler" TabIndex="13" CommandName="SHOWPOPUP" SkinID="btnInner-search" ToolTip="<%$ resources:Controls,Search %>"/>
                                    </li>
                                    <li id="pnlPrint"  style="display: none">
                                        <asp:Button runat="server" TabIndex="14" ID="btnPrint" CommandName="PRINT" 
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            OnClick="ActionHandler" ToolTip="<%$resources:Controls,Print %>" ValidationGroup="monthView" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="">
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <table class="table-devide">
            <tr>
                <td>
                    <div class="div2col-S">
                        <asp:HiddenField ID="hdfNumberDigits" runat="server" />
                        <asp:HiddenField ID="hdfCurrencyDigits" runat="server" />
                        <asp:HiddenField ID="hdfExchangeDigits" runat="server" />
                        <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                        <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                        <asp:HiddenField ID="hdfRateFormat" runat="server" />
                        <asp:HiddenField ID="hdfExngRateFormat" runat="server" />
                        <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
                        <asp:Label runat="server" ID="lblControl" Text="<%$ resources:Month %>" CssClass="lbl-12-4perc" AssociatedControlID="txtCalender"></asp:Label>
                        <asp:TextBox runat="server" ID="txtCalender" ClientIDMode="Static" CssClass="medium"
                            TabIndex="1" MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"
                            ValidationGroup="monthView"></asp:TextBox>
                        <cc1:CalendarExtender runat="server" ID="txtCalender_CalendarExtender" BehaviorID="calendar1"
                            TargetControlID="txtCalender" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                            ClientIDMode="Static" OnClientHidden="onCalendarHidden" OnClientDateSelectionChanged="SetDate">
                        </cc1:CalendarExtender>
                        <asp:RequiredFieldValidator ID="vrftxtCalender" CssClass="star" SetFocusOnError="true"
                            ValidationGroup="monthView" EnableClientScript="true" runat="server" ControlToValidate="txtCalender"
                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Month %>">
                        </asp:RequiredFieldValidator>
                        <div style="display:none">
                        <asp:Button runat="server" ID="btnSet" CommandName="SET" Text="<%$resources:ErpRes,Go %>" TabIndex="2"
                                ValidationGroup="monthView" ToolTip="<%$resources:ErpRes,Go %>" CommandArgument="SEC_ActionPanel" 
                                OnClick="ActionHandler" SkinID="btnInner-set" OnClientClick="javascript:ValidatePageNow('monthView')" />
                       </div>
                    </div>
                </td>
            </tr>
            </table>

            <asp:TableRow ID="PageAction_List" runat="server">
                <asp:TableCell>
                    <div class="clear"></div>
                    <%--Show/Hide record for editing the details--%>
                    <div><%--class="search-colapse-b"--%>
                        <asp:HiddenField ID="hdfIsItemDetailsVisible" runat="server" Value="0" />
                        <div class="clear">
                        </div>
                    </div>
                    <div id="divItemDetails" style="display: none">
                        <table class="table-devide" id="tblDetails">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                         <asp:HiddenField ID="hdfDetailPK" runat="server" Value="0" />
                                        <asp:Label runat="server" ID="lblItemInvoiceDate" Text="<%$ resources:ItemInvoiceDate %>"
                                            AssociatedControlID="txtItemInvoiceDate"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtItemInvoiceDate" TabIndex="2" CssClass="input-small"
                                                onkeydown="return CheckKey(event)" MaxLength="11" ValidationGroup="scItem" onpaste="return false;" ></asp:TextBox>
                                        <%--19092014 <asp:TextBox runat="server" ID="txtItemInvoiceDate" Enabled="false" CssClass="input-disabled" TabIndex="2"
                                                    onkeydown="return CheckKey(event)" MaxLength="11" ValidationGroup="scItem" onpaste="return false;" ></asp:TextBox>--%>
                                        <%--<asp:TextBox runat="server" ID="txtItemInvoiceDate" CssClass="date-picker"  TabIndex="2"
                                                onkeydown="return CheckKey(event)" MaxLength="11" ValidationGroup="scItem" onpaste="return false;" ></asp:TextBox>--%>
                                            <cc1:CalendarExtender runat="server" ID="txtItemInvoiceDate_CalendarExtender" BehaviorID="calendar3"
                                                TargetControlID="txtItemInvoiceDate" Format="dd-MMM-yyyy" OnClientShown="onCalendarShown"
                                                ClientIDMode="Static" OnClientHidden="onCalendarHidden" >
                                            </cc1:CalendarExtender>
                                        <asp:HiddenField runat="server" ID="hdfItemInvoiceDate" />
                                        <div class="starwrap">
                                            <asp:RequiredFieldValidator ID="vrfItemInvoiceDate" CssClass="star" SetFocusOnError="false"
                                                ValidationGroup="scItem" EnableClientScript="true" runat="server" 
                                                ControlToValidate="txtItemInvoiceDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Month %>">
                                            </asp:RequiredFieldValidator>
                                        </div>

                                         <asp:Label ID="lblItemInvoiceNo" runat="server" AssociatedControlID="txtItemInvoiceNo" Text="<%$ resources:ItemInvoiceNo %>">
                                        </asp:Label>
                                        <asp:TextBox runat="server" ID="txtItemInvoiceNo" CssClass="input-small-a" MaxLength="100" TabIndex="3"></asp:TextBox>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblItemCustomerName" runat="server" AssociatedControlID="txtItemCustomerName" Text="<%$ resources:ItemCustomerName %>">
                                        </asp:Label>
                                        <asp:TextBox ID="txtItemCustomerName" runat="server" CssClass="input-half" MaxLength="200" TabIndex="4"></asp:TextBox>
                                        <asp:HiddenField runat="server" ID="hdfItemCustomerName" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   <div class="div2col-S">
                                        <asp:Label ID="lblItemProduct" runat="server" AssociatedControlID="txtItemProduct" Text="<%$ resources:ItemProduct %>">
                                        </asp:Label>
                                        <asp:TextBox ID="txtItemProduct" runat="server" TextMode="MultiLine" TabIndex="6" CssClass="multiline-3col" Width="390px"></asp:TextBox>
                                        <asp:HiddenField runat="server" ID="hdfItemProduct" />
                                    </div>
                                </td>
                                <td>                                   
                                      <div class="div2col-S">
                                        <asp:Label ID="lblItemDeclarationNo" runat="server" AssociatedControlID="txtItemDeclarationNo" Text="<%$ resources:ItemDeclarationNo %>">
                                        </asp:Label>
                                        <asp:TextBox runat="server" ID="txtItemDeclarationNo" CssClass="input-half" MaxLength="100" TabIndex="5"></asp:TextBox> 

                                        <div class="clear"></div>
                                        <asp:Label ID="lblItemValueFOB" runat="server" AssociatedControlID="txtItemValueFOB" Text="<%$ resources:ItemValueFOB %>">
                                        </asp:Label>
                                        <asp:TextBox ID="txtItemValueFOB" runat="server" MaxLength="16" TabIndex="7" CssClass="input-small-a numeric" onchange="CalculateTotal(this);" onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="vrfItemValueFOB" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="scItem" EnableClientScript="true" runat="server" ControlToValidate="txtItemValueFOB"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Value %>">
                                        </asp:RequiredFieldValidator>
                                        <cc1:RateValidation ID="vreItemValueFOB" runat="server" ControlToValidate="txtItemValueFOB" 
                                            ErrorMessage="<%$ resources:Err_Value_Valid %>"
                                            NumberDigits="10" Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star"
                                            ValidationGroup="scItem" ></cc1:RateValidation>
                                        <asp:HiddenField runat="server" ID="hdfItemValueFOB" />
                                       
                                        <asp:Label ID="lblItemExchangeRate" runat="server" CssClass="middle-lbl-a0" AssociatedControlID="txtItemExchangeRate" Text="<%$ resources:ItemExchangeRate %>">
                                        </asp:Label>
                                        <asp:TextBox ID="txtItemExchangeRate" runat="server" MaxLength="16" CssClass="input-small-a numeric" TabIndex="8"
                                            onchange="CalculateTotal(this);" onkeypress="return validateRateFloatKeyPress(this,event);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="vrfItemExchangeRate" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="scItem" EnableClientScript="true" runat="server" ControlToValidate="txtItemExchangeRate"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Value %>">
                                        </asp:RequiredFieldValidator>
                                        <%--<cc1:RateValidation ID="vreItemExchangeRate" runat="server" ControlToValidate="txtItemExchangeRate" 
                                            ErrorMessage="<%$ resources:Err_Value_Valid %>"
                                            NumberDigits="10" Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star"
                                            ValidationGroup="scItem" ></cc1:RateValidation>--%>
                                            <cc1:ExchangeRateValidation ID="vreItemExchangeRate" runat="server" ControlToValidate="txtItemExchangeRate" 
                                            ErrorMessage="<%$ resources:Err_Value_Valid %>"
                                            NumberDigits="10" Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star"
                                            ValidationGroup="scItem" ></cc1:ExchangeRateValidation>
                                        <%--change19092014<asp:TextBox ID="txtItemExchangeRate" runat="server" MaxLength="16" Enabled="false" CssClass="medium input-disabled numeric"></asp:TextBox>--%>
                                        <asp:HiddenField runat="server" ID="hdfItemExchangeRate" />

                                        <asp:Label ID="lblItemAmountTHB" runat="server" AssociatedControlID="txtItemAmountTHB" Text="<%$ resources:ItemAmountTHB %>">
                                        </asp:Label>
                                        <asp:TextBox ID="txtItemAmountTHB" runat="server" MaxLength="16" TabIndex="8" CssClass="input-small-a numeric" onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="vrfItemAmountTHB" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="scItem" EnableClientScript="true" runat="server" ControlToValidate="txtItemAmountTHB"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ItemAmountTHB %>">
                                        </asp:RequiredFieldValidator>
                                        <cc1:RateValidation ID="vreItemAmountTHB" runat="server" ControlToValidate="txtItemAmountTHB" ErrorMessage="<%$ resources:Err_Amount_Valid %>"
                                                NumberDigits="10" Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star"
                                                ValidationGroup="scItem" ></cc1:RateValidation>
                                        <asp:HiddenField runat="server" ID="hdfItemAmountTHB" />

                                         <asp:ImageButton runat="server" ID="btnAddItem" CommandName="ADDITEM" TabIndex="9"
                                        OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('scItem')"
                                        ToolTip="<%$resources:ErpRes,Add %>"  ValidationGroup="scItem"
                                        SkinID="plus" />
                                    <asp:ImageButton runat="server" ID="btnClearItem" CommandName="CLEARITEM" TabIndex="10"
                                        OnClick="ActionHandler" ToolTip="<%$resources:Controls,Clear %>" 
                                        SkinID="cancel" />
                                    </div>                                    
                                </td>
                            </tr>                         
                            
                        </table>
                    </div>
                     <div class="content-wrapper">
                    <div class="gridwrap">
                        <asp:GridView runat="server" ID="grdVatSaleList" Width="100%" PageSize="<%$ resources:PageSize%>" OnSorting="ActionHandler" 
                        OnRowDataBound="ActionHandler" OnPageIndexChanging="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" ShowFooter="true">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ resources:GHInvoiceDate %>" SortExpression="TSD_INVOICE_DATE">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceDate" runat="server" Text='<%#  Eval("TSD_INVOICE_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("TSD_INVOICE_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                            ToolTip='<%# Eval("TSD_INVOICE_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                        <asp:HiddenField ID="hdfInvoiceHDR" Value='<%# Eval("TSD_INVOICE_HDR")%>' runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:GHInvoiceNo %>" SortExpression="TSD_INVOICE_NO">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("TSD_INVOICE_NO")%>'
                                            ToolTip='<%# Eval("TSD_INVOICE_NO")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="9%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:GHDeclarationNo %>" SortExpression="TSD_DECLARATION_NO">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDeclarationNo" runat="server" Text='<%# HttpUtility.HtmlDecode(Eval("TSD_DECLARATION_NO").ToString())%>'
                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval("TSD_DECLARATION_NO").ToString())%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:GHCustomerName %>" SortExpression="TSD_CUSTOMER_NAME">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerName" runat="server" Text='<%#HttpUtility.HtmlDecode( Eval("TSD_CUSTOMER_NAME").ToString()) %>'
                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval("TSD_CUSTOMER_NAME").ToString()) %>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdfCustomerPK" Value='<%# Eval("TSD_CUSTOMER") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="19%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:GHProduct %>" SortExpression="TSD_ITEM_TEXT">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProduct" runat="server" Text='<%#Eval("TSD_ITEM_TEXT")!=null?HttpUtility.HtmlDecode(Eval("TSD_ITEM_TEXT").ToString()):""  %>'
                                            ToolTip='<%#Eval("TSD_ITEM_TEXT")!=null?HttpUtility.HtmlDecode(Eval("TSD_ITEM_TEXT").ToString()):""  %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="32.5%"/>
                                    </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:GHValue %>" SortExpression="TSD_NET_VALUE_TC">
                                    <ItemTemplate>
                                        <asp:Label ID="lblValue" runat="server" Text='<%# GetFormattedCurrency(Eval("TSD_NET_VALUE_TC", "{0:c}")) %>'
                                            ToolTip='<%# GetFormattedCurrency(Eval("TSD_NET_VALUE_TC", "{0:c}")) %>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdfCurrency" Value='<%# Eval("TSD_CURRENCY") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="6.5%" HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:GHExchangeRate %>" SortExpression="TSD_EXCHG_RATE">
                                    <ItemTemplate>
                                            <%--<asp:Label ID="lblExchangeRate" runat="server" Text='<%#GetFormattedRate(Eval("TSD_EXCHG_RATE"))  %>'
                                            ToolTip='<%#GetFormattedRate(Eval("TSD_EXCHG_RATE"))  %>'></asp:Label>--%>
                                            <asp:Label ID="lblExchangeRate" runat="server" Text='<%#GetFormattedRateExngRate(Eval("TSD_EXCHG_RATE"))  %>'
                                            ToolTip='<%#GetFormattedRateExngRate(Eval("TSD_EXCHG_RATE"))  %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" CssClass="amount-numeric" />
                                    <HeaderStyle CssClass="amount-numeric" />
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="lblfooter" Text="<%$ resources:GFTotal %>"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:GHAmount %>" SortExpression="TSD_NET_VALUE_BC">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmount" runat="server" Text='<%# GetFormattedCurrency(Eval("TSD_NET_VALUE_BC", "{0:c}")) %>'
                                            ToolTip='<%# GetFormattedCurrency(Eval("TSD_NET_VALUE_BC", "{0:c}")) %>'></asp:Label>
                                        
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" CssClass="amount-numeric" />
                                    <HeaderStyle CssClass="amount-numeric" />
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="lblTotalAmount" ></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField> 
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnEditItem" runat="server" OnClick="ActionHandler" CommandName="EDITITEM"
                                            SkinID="imbeditgrid" ToolTip="Edit" TabIndex="10" />
                                    <asp:Button ID="imgSaved" runat="server" OnClientClick="javascript:return false;"
                                        CssClass='<%# (Eval("TSD_PK").ToString()== "0" ? "flaggrey-icon" : "flaggreen-icon") %>'  
                                        ToolTip='<%# (Eval("TSD_PK").ToString()=="0" ? GetLocalResourceObject("ToolTip_NotSaved").ToString() : GetLocalResourceObject("ToolTip_Saved").ToString()) %>'   />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="5%"/>
                            </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    </div>
                    <%--Search popup--%>
                    <div id="divSearchPopup" style="display:none;">
                        <table class="table-devide" id="tblAdvancedSearch" style="margin-top: 8px;">
                            <tr>
                                <td colspan="2">
                                    <div class="search-colapse">
                                        <table>
                                            <tr>
                                                <td>
                                                    <h1><%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="div2col-S padgtop7">
                                        <asp:Label ID="lblSearchFrmDate" runat="server" Text="<%$resources:SearchFromDate %>" AssociatedControlID="txtSearchFromDate"></asp:Label>
                                        <asp:TextBox ID="txtSearchFromDate" runat="server" TabIndex="1" CssClass="input-small" MaxLength="17"
                                            onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                        <cc1:CalendarExtender runat="server" ID="txtSearchFromDate_CalendarExtender" 
                                            TargetControlID="txtSearchFromDate" Format="dd-MMM-yyyy" ClientIDMode="Static">
                                        </cc1:CalendarExtender>
                                        <asp:HiddenField ID="hdfSearchFromDate" runat="server" Value="" />

                                        <asp:Label ID="lblSearchToDate" runat="server" CssClass="lbl-10-7perc" Text="<%$resources:SearchToDate %>" AssociatedControlID="txtSearchToDate"></asp:Label>
                                        <asp:TextBox ID="txtSearchToDate" runat="server" TabIndex="2" CssClass="input-small" MaxLength="17"
                                            onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                        <cc1:CalendarExtender runat="server" ID="txtSearchToDate_CalendarExtender"
                                                TargetControlID="txtSearchToDate" Format="dd-MMM-yyyy" ClientIDMode="Static">
                                            </cc1:CalendarExtender>
                                        <asp:HiddenField ID="hdfSearchToDate" runat="server" Value="" />                                    
                                        <div class="clear">
                                        </div>                                        
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S padgtop7">
                                           <asp:Label ID="lblSearchCustomer" runat="server" Text="<%$resources:SearchCustomer %>" CssClass="lbl-14-9perc" AssociatedControlID="txtSearchCustomer"></asp:Label>
                                        <asp:TextBox ID="txtSearchCustomer" runat="server" CssClass="input-small-d" MaxLength="100" TabIndex="3"> </asp:TextBox>
                                        <asp:HiddenField ID="hdfSearchCustomerID" runat="server" />

                                         <asp:Label ID="lblSearchInvoiceNo" runat="server" Text="<%$resources:SearchInvoiceNo %>" CssClass="lbl-10-7perc" AssociatedControlID="txtSearchInvoiceNo"></asp:Label>
                                        <asp:TextBox ID="txtSearchInvoiceNo" runat="server" CssClass="input-small" MaxLength="100" TabIndex="4"> </asp:TextBox>
                                        <asp:HiddenField ID="hdfSearchInvoiceNo" runat="server" Value="" />                                      
                                        <asp:ImageButton ID="btnGo" SkinID="search-ext" runat="server" 
                                             ToolTip="<%$ resources:Controls,Search %>" CommandName="SEARCH" OnClick="ActionHandler" TabIndex="5" />
                                        <asp:ImageButton ID="btnClear" runat="server"  TabIndex="6"
                                            OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext" ToolTip="<%$ resources:Controls,Clear %>"/>                                 
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div class="clear">
                        </div>
                       <div class="content-wrapper">
                        <div class="gridwrap">
                        <asp:GridView runat="server" ID="grdVatSaleSearchList" Width="100%" PageSize="<%$ resources:PageSize%>" OnSorting="ActionHandler" 
                            OnRowDataBound="ActionHandler" OnPageIndexChanging="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ resources:GHInvoiceDate %>" SortExpression="TSD_INVOICE_DATE">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceDate" runat="server" Text='<%#  Eval("TSD_INVOICE_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("TSD_INVOICE_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                            ToolTip='<%# Eval("TSD_INVOICE_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                        <asp:HiddenField ID="hdfInvoiceHDR" Value='<%# Eval("TSD_INVOICE_HDR")%>' runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:GHInvoiceNo %>" SortExpression="TSD_INVOICE_NO">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("TSD_INVOICE_NO")%>'
                                            ToolTip='<%# Eval("TSD_INVOICE_NO")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:GHDeclarationNo %>" SortExpression="TSD_DECLARATION_NO">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDeclarationNo" runat="server" Text='<%# Eval("TSD_DECLARATION_NO")%>'
                                            ToolTip='<%# Eval("TSD_DECLARATION_NO")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="9%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:GHCustomerName %>" SortExpression="TSD_CUSTOMER_NAME">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("TSD_CUSTOMER_NAME") %>'
                                            ToolTip='<%# Eval("TSD_CUSTOMER_NAME") %>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdfCustomerPK" Value='<%# Eval("TSD_CUSTOMER") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="18%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:GHProduct %>" SortExpression="TSD_ITEM_TEXT">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProduct" runat="server" Text='<%#Eval("TSD_ITEM_TEXT")  %>'
                                            ToolTip='<%#Eval("TSD_ITEM_TEXT")  %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="34.5%"/>
                                    </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:GHValue %>" SortExpression="TSD_NET_VALUE_TC">
                                    <ItemTemplate>
                                        <asp:Label ID="lblValue" runat="server" Text='<%# GetFormattedCurrency(Eval("TSD_NET_VALUE_TC", "{0:c}")) %>'
                                            ToolTip='<%# GetFormattedCurrency(Eval("TSD_NET_VALUE_TC", "{0:c}")) %>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdfCurrency" Value='<%# Eval("TSD_CURRENCY") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="7.5%" HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:GHExchangeRate %>" SortExpression="TSD_EXCHG_RATE">
                                    <ItemTemplate>
                                            <asp:Label ID="lblExchangeRate" runat="server" Text='<%#GetFormattedRate(Eval("TSD_EXCHG_RATE"))  %>'
                                            ToolTip='<%#GetFormattedRate(Eval("TSD_EXCHG_RATE"))  %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" CssClass="amount-numeric" />
                                    <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:GHAmount %>" SortExpression="TSD_NET_VALUE_BC">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmount" runat="server" Text='<%# GetFormattedCurrency(Eval("TSD_NET_VALUE_BC", "{0:c}")) %>'
                                            ToolTip='<%# GetFormattedCurrency(Eval("TSD_NET_VALUE_BC", "{0:c}")) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" CssClass="amount-numeric" />
                                    <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>
                                
                            </Columns>
                        </asp:GridView>
                    </div>
                       </div>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <div id="diverror" style="display: none">
                <%--Use this label to bind the server errors--%>
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsMonthView" ValidationGroup="monthView" runat="server" />
                <asp:ValidationSummary ID="vsItem" ValidationGroup="scItem" runat="server" />             
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
