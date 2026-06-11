<%@ Page Title="<%$ Resources:Captions,Title_ClosingStock %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="ClosingStock.aspx.cs" Inherits="ERPSMS_v01.Finance.ClosingStock"
    Theme="ClassicExt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/Journalize/UserControls/JournalizeControlNew.ascx" TagName="Journalize"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var NumberDgiits = 0;
        var CurrencyDigits = 0; 
        $(document).ready(function () {
            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
        });
        function ScrollDown() {
            window.scroll(400, 400);
            return false;
        }
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtStockDate");
            GrandScriptUtils.DatePickerCommon("txtPVDate");
            CalculateTotal();
            $("[id$=txtJournalExchangeRate]").ForceNumericOnly();
            $("[id*=txtClosingNow]").ForceNumericOnly();
            if ($('[id$=btnJournalSaveSubmit]').is(":visible"))
                $('[id$=btnJournalSubmit]').hide();
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();

            //Set a stamp for cancelled invoice
            if ($("[id$=hdfIsCancelled]").val() == "1") {
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
                $("[id$=pnlSave]").hide();
                $("[id$=pnlPost]").hide();               
            }
            else {
                $("[id$=tblDetailHdr]").addClass("table-devide");
            }

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
                $("[id$=pnlPost]").hide();
                $("[id$=pnlSubmit]").hide();
                $("[id$=pnlSaveSubmit]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
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
            //            if (targetControlID == "txtVendor") {
            //                $("[id$=btnVendor]").click();
            //            }
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteSelect(targetControlID);
            }
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            //            if (targetControlID == "txtVendor") {
            //                $("[id$=btnVendor]").click();
            //            }
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteInvalidSelect(targetControlID);
            }
        }

        function CalculateTotal(sender) {
            var val1 = parseFloat($(sender).val());
            var Amount = 0;
            var CloseNowAmnt = 0;
            var DecimalDigits = 0;
            if (!isNaN(parseFloat($("[id$=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("[id$=hdfDecimalDigits]").val());
            }
            //Calc Total
            $("#[id*=grdItemDetails] input[type=text][id*=txtClosingNow]").each(function (index) {
                //Check if number is not empty
                if ($.trim($(this).val()) != "") {
                    //Check if number is a valid integer
                    if (!isNaN(parseFloat($(this).val()))) {
                        CloseNowAmnt = CloseNowAmnt + parseFloat($(this).val());
                    }
                }
            });
            $("#[id*=grdItemDetails] [id*=lblTotalClosingFooter]").html(addCommas(CloseNowAmnt.toFixed(DecimalDigits)));

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
                    //ShowContainerDiv('[id$=divJournalize]', '<%= GetLocalResourceObject("Closing_Stk_Journal") %>', '1000', '550');
                    ShowCommonCotainerDiv('[id$=divJournalize]', '<%= GetLocalResourceObject("Closing_Stk_Journal") %>', "1%");
                    AfterCloseWkfInJournal();
                    //$("[id$=btnJournalize_Action]").click();
                }
            } else if (containerID == "[id$=divTemplate]") {
                //ShowContainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), '1000', '550');
                ShowCommonCotainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), "1%");
            }
        }

        function AfterDateSelect(controlID) {
            if (controlID == "txtAsOnDate") {
                $("[id$=btnAsOnDateChange]").click();
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
    <asp:UpdatePanel runat="server" ID="aupdpnlPOInvoice">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <div class="buttoncontainer-fields floatLeft" id="divSBUCompany">
                                    <asp:DropDownList ID="ddlCompany" class="select-full-a margnbotm0" runat="server"
                                        TabIndex="1" onmouseover="javascript:ShowTooltip('ddlCompany');">
                                    </asp:DropDownList>
                                </div>
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="50"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="51"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="ClosingStock" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="52" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="ClosingStock" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="53" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="ClosingStock" ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteClosingStk" CommandName="DELETE" TabIndex="54" Text="<%$resources:ErpRes,Delete %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Delete %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Delete" />
                                    </li>
                                    <%--<li id="pnlPrint">
                                        <asp:Button runat="server" TabIndex="54" ID="btnPrint" Visible="false" CommandName="PRINT"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
                                    </li>--%>
                                    <li runat="server" id="pnlPost">
                                        <asp:Button runat="server" ID="btnJournalize" CommandName="JOURNALIZE" TabIndex="55"
                                            Text="<%$resources:Journalize %>" OnClick="ActionHandler" ToolTip="<%$resources:Journalize %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-journalize" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="56" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                    
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li runat="server" id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="57" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelCLST %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelCLST %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="58" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="59" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="60" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
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
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="DETAILS"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
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
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="1" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="1" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn----------%>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblMnth" runat="server" Text="<%$resources:AsOnMonth %>" AssociatedControlID="txtMonth"></asp:Label>
                                            <asp:TextBox ID="txtMonth" runat="server" TabIndex="2" CssClass="input-small margnbotm0"
                                                MaxLength="17" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <cc1:CalendarExtender runat="server" ID="txtMonth_CalendarExtender" BehaviorID="calendar2"
                                                TargetControlID="txtMonth" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                                                ClientIDMode="Static" OnClientHidden="onCalendarHidden">
                                            </cc1:CalendarExtender>
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-a margnbotm0" TabIndex="2">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,NotPosted %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Posted %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblStkNo" runat="server" Text="<%$resources:StockNo %>" AssociatedControlID="txtStockNo"></asp:Label>
                                            <asp:TextBox ID="txtStockNo" runat="server" CssClass="input-small margnbotm0" MaxLength="100"
                                                TabIndex="4"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfStockPk" runat="server" Value="" />
                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="5" CssClass="margntop2 margnbotm0" CommandName="SEARCH"
                                                SkinID="search-ext" />
                                            <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="5" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdClosingStock" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" runat="server" GroupName="SelectOne"
                                                    ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfHdrPk" Value='<%# Eval("LSH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfDept"  Value='<%# Eval("LSH_DEPT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval("LSH_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfDelStatus" Value='<%# Eval("LSH_DEL_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfPosted" Value='<%# Eval("LSH_HAS_JRNL_ENTRY") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AsOnMonth %>" SortExpression="LSH_AS_ON_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAsOnDate" runat="server" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("LSH_AS_ON_DATE"))) ? string.Empty : Eval("LSH_AS_ON_DATE", Resources.Constants.DateFormatGridMonthYear)%>'></asp:Label>
                                                <%--<asp:Label ID="lblAsOnDate" runat="server" Text='<%#  Eval("LSH_AS_ON_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("LSH_AS_ON_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                    ToolTip='<%# Eval("LSH_AS_ON_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:StockNo %>" SortExpression="LSH_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("LSH_NO")))?"[NEW]":Eval("LSH_NO")%>'
                                                    ToolTip='<%# Eval("LSH_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TrnDate %>" SortExpression="LSH_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTrxDate" runat="server" Text='<%#  Eval("LSH_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("LSH_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                    ToolTip='<%# Eval("LSH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Closing %>" SortExpression="LSH_CLOSING">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("LSH_CLOSING", "{0:c}") %>'
                                                    ToolTip='<%# Eval("LSH_CLOSING", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="35%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:Excnng %>" SortExpression="HRH_EXCHG_RATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExchngRate" runat="server" Text='<%#GetFormattedExchangeRate(Eval("LSH_EXCHG_RATE"))  %>'
                                                    ToolTip='<%#GetFormattedExchangeRate(Eval("LSH_EXCHG_RATE"))  %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("LSH_ASC_CSS_CLASS")%>' ToolTip='<%# Eval("LSH_ASC_NAME") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgPosted" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_ASC_CSS_CLASS"))) ? GetLocalResourceObject("unposted").ToString() : Eval("FTH_ASC_CSS_CLASS")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_ASC_CSS_CLASS"))) ? Resources.Captions.NotPosted : Eval("FTH_ASC_NAME")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="3%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
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
                                            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                                            <asp:HiddenField ID="hdfRateFormat" runat="server" />
                                            <asp:HiddenField ID="hdfExchangeRateFormat" runat="server" />
                                            <asp:HiddenField ID="hdfClosingStockNo" runat="server" />
                                            <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                                            <asp:HiddenField ID="AST_CODE" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblStockNo" Text="<%$ resources:StockNo%>" AssociatedControlID="lblClosingStockNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblClosingStockNo" CssClass="input-small"></asp:Label>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblAsOnDate" Text="<%$ resources:AsOnMonth%>" AssociatedControlID="txtAsOnDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtAsOnDate" CssClass="input-small" TabIndex="2"
                                                onkeydown="return CheckKey(event)" onpaste="return false;" MaxLength="11" AutoPostBack="true"
                                                OnTextChanged="ActionHandler"></asp:TextBox>
                                            <cc1:CalendarExtender runat="server" ID="txtAsOnDate_CalendarExtender" BehaviorID="calendar1"
                                                TargetControlID="txtAsOnDate" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                                                ClientIDMode="Static" OnClientHidden="onCalendarHidden">
                                            </cc1:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="ClosingStock" EnableClientScript="true" runat="server" ControlToValidate="txtAsOnDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_AsOnDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Button ID="btnAsOnDateChange" runat="server" EnableTheming="false" Style="display: none"
                                                OnClick="ActionHandler" CommandName="ASONDATECHANGE" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblStockDate" Text="<%$ resources:TrnDate%>" AssociatedControlID="txtStockDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtStockDate" CssClass="input-small" TabIndex="1"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfStockDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="ClosingStock" EnableClientScript="true" runat="server" ControlToValidate="txtStockDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_StockDate %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <asp:GridView ID="grdItemDetails" runat="server" AutoGenerateColumns="False" Width="100%"
                                    AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                    ShowFooter="true" >
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Category %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfDetlPk" runat="server" Value='<%#Eval("LSD_PK") %>' />
                                                <asp:HiddenField ID="hdfItemCatPk" runat="server" Value='<%#Eval("LSD_ITEM_CAT") %>' />
                                                <asp:Label ID="lblCategotyName" runat="server" Text='<%# HttpUtility.HtmlDecode(Eval("ITC_NAME").ToString()) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("ITC_NAME").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="35%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnHistory" runat="server" OnClick="ActionHandler" CommandName="CLOSINGSTOCKHISTORY"
                                                    SkinID="history" ToolTip="<%$ resources:History %>" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Right" CssClass="amount-numeric" />
                                            <HeaderStyle HorizontalAlign="Right" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Purchase %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPurchaseAmount" runat="server" Text='<%#Eval("LSD_PURCHASE","{0:c}") %>'
                                                    ToolTip='<%#Eval("LSD_PURCHASE","{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" HorizontalAlign="Right" CssClass="amount-numeric" />
                                            <HeaderStyle HorizontalAlign="Right" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Stock %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStockAmount" runat="server" Text='<%#Eval("LSD_STOCK","{0:c}") %>'
                                                    ToolTip='<%#Eval("LSD_STOCK","{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" HorizontalAlign="Right" CssClass="amount-numeric" />
                                            <HeaderStyle HorizontalAlign="Right" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Closing%>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtClosingNow" runat="server" Text='<%# GetFormattedCurrency(Eval("LSD_CLOSING")) %>'
                                                    CssClass="input-w125 numeric" MaxLength="16" TabIndex="4" onkeyup="CalculateTotal(this);"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="vrfClosingNow" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="ClosingStock" EnableClientScript="true" runat="server" ControlToValidate="txtClosingNow"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount_Valid %>">
                                                </asp:RequiredFieldValidator>--%>
                                                <%--<cc1:QuantityValidation ID="vreClosing" runat="server" ControlToValidate="txtClosingNow"
                                                    NumberDigits="9" ErrorMessage="<%$ resources:Err_Invalid_Amount %>" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="ClosingStock"></cc1:QuantityValidation>--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalClosingFooter"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <table class="table-devide">
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks %>" AssociatedControlID="txtRemarks"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRemarks" TabIndex="5" TextMode="MultiLine" CssClass="multiline-2line"
                                                onkeydown="limitText(this,500);" onchange="limitText(this,500);"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <%-----------Closing stock history Popup Start----------------------------------------%>
                <div id="divStockHistory" style="display: none">
                    <div class="content-wrapper">
                        <div class="detail-co3">
                            <div class="divcol2-S">
                                <asp:Label ID="lblCat" runat="server" Text="<%$ resources:Category1 %>" AssociatedControlID="lblItemCategotyName"></asp:Label>
                                <asp:Label ID="lblItemCategotyName" runat="server" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="clear">
                            </div>
                        </div>
                        <div class="gridwrap">
                            <asp:TableCell>
                                <div class="gridwrap">
                                    <asp:GridView ID="grdStockHistory" runat="server" AutoGenerateColumns="False" Width="100%"
                                        PageSize="<%$ resources:PageSize %>" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable"
                                        AllowSorting="false" ShowFooter="true">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:AsOnMonth %>" SortExpression="LSH_AS_ON_DATE">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAsOnDate" runat="server" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("LSH_AS_ON_DATE"))) ? string.Empty : Eval("LSH_AS_ON_DATE", Resources.Constants.DateFormatGridMonthYear)%>'>
                                                    </asp:Label>
                                                    <%--<asp:Label ID="lblAsOnDate" runat="server" Text='<%#  Eval("LSH_AS_ON_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("LSH_AS_ON_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                        ToolTip='<%# Eval("LSH_AS_ON_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>--%>
                                                </ItemTemplate>
                                                <ItemStyle Width="12%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:StockNo %>" SortExpression="LSH_NO">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("LSH_NO")))?"[NEW]":Eval("LSH_NO")%>'
                                                        ToolTip='<%# Eval("LSH_NO")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:TrnDate %>" SortExpression="LSH_DATE">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTrxDate" runat="server" Text='<%#  Eval("LSH_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("LSH_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                        ToolTip='<%# Eval("LSH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Purchase %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPurchaseAmount_hstry" runat="server" Text='<%#Eval("LSD_PURCHASE","{0:c}") %>'
                                                        ToolTip='<%#Eval("LSD_PURCHASE","{0:c}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" HorizontalAlign="Right" CssClass="amount-numeric" />
                                                <HeaderStyle HorizontalAlign="Right" CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Stock %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStockAmount_hstry" runat="server" Text='<%#Eval("LSD_STOCK","{0:c}") %>'
                                                        ToolTip='<%#Eval("LSD_STOCK","{0:c}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" HorizontalAlign="Right" CssClass="amount-numeric" />
                                                <HeaderStyle HorizontalAlign="Right" CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Closing%>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblClosing_hstry" runat="server" Text='<%#Eval("LSD_CLOSING","{0:c}") %>'
                                                        ToolTip='<%#Eval("LSD_CLOSING","{0:c}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" CssClass="amount-numeric" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalClosingFooter_hstry"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:TableCell>
                        </div>
                    </div>
                </div>
                <%-----------Closing stock history Popup End----------------------------------------%>
                <div id="divScriptButtons">
                    <asp:Button runat="server" ID="btnJournalize_Action" CommandName="JOURNALIZE" OnClick="ActionHandler"
                        EnableTheming="false" Style="display: none" />
                    <asp:Button ID="btnJournalizeUpdate" runat="server" OnClick="ActionHandler" CommandName="JOURNALIZEUPDATE"
                        EnableTheming="false" Style="display: none" />
                </div>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="ClosingStock" runat="server" />
                    <asp:HiddenField ID="hdfAppType" runat="server" />
                </div>
                <%--User Control--%>
                <div id="divJournalize" style="display: none">
                    <uc1:Journalize ID="ucrJournalize" runat="server" />
                </div>
               
                <div id="divWkfSubmit" style="display: none;">
                    <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                    <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="ClosingStock">
                    </uc1:WorkflowUserComments>
                </div>

                 <asp:HiddenField ID="hdfInventTypeConfig" Value="0" runat="server" />
                <asp:HiddenField ID="hdfIscontYes" runat="server" />
                <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
                <asp:HiddenField ID="hdfJournalizeWorkFlow" Value="0" runat="server" />
                <asp:HiddenField ID="hdfIsCancelled" runat="server" Value="0" />
                <asp:HiddenField ID="hdfExchangeRate" runat="server" />
                <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
                <asp:HiddenField ID="hdfCurrencyGroup2" Value="2" runat="server" />
                <asp:HiddenField runat="server" ID="hdfIsPurStkVisible" />
                <asp:HiddenField runat="server" ID="hdfIsJournalize" />
               
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
