<%@ Page Title="<%$ Resources:Captions,Title_AuditTrail %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="AuditTrailInvoiceReport.aspx.cs" Inherits="ERPSMS_v01.Finance.AuditTrailInvoiceReport"
    Theme="Classic" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtItemFromDate", "hdfItemFromDate", "txtItemToDate", "hdfItemToDate", false, false);
        }
        var NumberDigits = 0;
        var CurrencyDigits = 0;
        $(document).ready(function () {
            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
        });
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

            //HideFilter();
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
        function CalculateTotal(sender) {
            var valueFOB = parseFloat($("[id$=txtItemValueFOB]").val().replace(',', ''));
            valueFOB = isNaN(valueFOB) ? 0 : valueFOB;
            var exchangeRate = parseFloat($("[id$=txtItemExchangeRate]").val().replace(',', ''));
            exchangeRate = isNaN(exchangeRate) ? 0 : exchangeRate;
            var amountTHB = valueFOB * exchangeRate;
            $("[id$=txtItemAmountTHB]").val((amountTHB).toFixed(CurrencyDigits));
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlVatSaleExport">
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
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="42" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" ValidationGroup="item" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            CommandName="CANCEL" TabIndex="12" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
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
                            <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
                        </div>
                    </td>
                </tr>
            </table>
            <asp:TableRow ID="PageAction_List" runat="server">
                <asp:TableCell>
                    <div class="clear">
                    </div>
                    <div id="divItemDetails">
                        <table class="table-devide" id="tblDetails">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <asp:HiddenField ID="hdfDetailPK" runat="server" Value="0" />
                                        <asp:Label runat="server" ID="lblItemFromDate" Text="<%$ resources:ItemFromDate %>"
                                            AssociatedControlID="txtItemFromDate"></asp:Label>
                                        <asp:TextBox ID="txtItemFromDate" runat="server" TabIndex="3" CssClass="medium" MaxLength="11"
                                            onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="vrfItemFromDate" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="item" EnableClientScript="true" InitialValue="" runat="server"
                                            ControlToValidate="txtItemFromDate" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ItemFromDate %>">
                                        </asp:RequiredFieldValidator>
                                        <asp:HiddenField ID="hdfItemFromDate" runat="server" Value="" />
                                        <div class="clear">
                                        </div>
                                        <asp:Label runat="server" ID="lblItemToDate" Text="<%$ resources:ItemToDate %>" AssociatedControlID="txtItemToDate"></asp:Label>
                                        <asp:TextBox ID="txtItemToDate" runat="server" TabIndex="4" CssClass="medium" MaxLength="11"
                                            onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="vrfItemToDate" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="item" EnableClientScript="true" InitialValue="" runat="server"
                                            ControlToValidate="txtItemToDate" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ItemToDate %>">
                                        </asp:RequiredFieldValidator>
                                        <asp:HiddenField ID="hdfItemToDate" runat="server" Value="" />
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">                                        
                                        <asp:Label ID="lblUsers" runat="server" Text="<%$resources:User %>" AssociatedControlID="ddlUsers"></asp:Label>
                                        <asp:DropDownList ID="ddlUsers" runat="server">
                                        </asp:DropDownList>
                                        <div class="clear">
                                        </div>
                                        <asp:Label runat="server" ID="lblModified" Text="<%$ resources:Modified %>" AssociatedControlID="chkModified"></asp:Label>
                                        <asp:CheckBox ID="chkModified" runat="server" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="reportviewer" id="divReportViewer" runat="server">
                        <div style="overflow: auto; width: 98%;" align="center">
                            <rsweb:ReportViewer ID="rvViewReport" runat="server" BorderWidth="0" SizeToReportContent="true"
                                Width="98%">
                            </rsweb:ReportViewer>
                        </div>
                    </div>
                    <div style="height: 15px;">
                    </div>
                    <div id="divNodata" class="nodata" runat="server" visible="false">
                        No Record Found</div>
                    <asp:HiddenField ID="hdfSelectedNodes" runat="server" />
                </asp:TableCell>
            </asp:TableRow>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
