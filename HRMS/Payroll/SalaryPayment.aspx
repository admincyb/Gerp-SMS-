<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalaryPayment.aspx.cs"
    Title="<%$ Resources:Captions,Title_SalaryPayment %>" Inherits="HRMS.Payroll.SalaryPayment"
    MasterPageFile="~/ERPSMS_2.Master" Theme="ClassicExt" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/Journalize/UserControls/JournalizeControlNew.ascx" TagName="Journalize"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtListFromDate", "hdfListFromDate", "txtListToDate", "hdfListToDate", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtTrxNo", url, "hdfTrxPk", true, true, "SALPYMTNUMBER");
            GrandScriptUtils.DatePickerCommon("txtDate");
            GrandScriptUtils.DatePickerCommon("txtChequeDate");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCurrency", url, "hdfCurrency", true, true, "CURRENCY");
            //For Voucher
            GrandScriptUtils.DatePickerCommon("txtPVDate");
            $("[id*=txtExchangeRate]").ForceNumericOnly();

            if ($('[id$=btnJournalSaveSubmit]').is(":visible"))
                $('[id$=btnJournalSubmit]').hide();
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();

            //Set a stamp for cancelled 
            if ($("[id$=hdfIsCancelled]").val() == "1") {
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
                $("[id$='btnSave']").hide();
            }
            else {
                $("[id$=tblDetailHdr]").addClass("table-devide");
            }
            //End
            if ($("[id$=txtExchangeRate]").attr("disabled") == true) {
                $("[id$=txtExchangeRate]").addClass("input-disabled");
            }
            else {
                $("[id$=txtExchangeRate]").removeClass("input-disabled");
            }

            // To Disable Cuurency When Grid Contains Data
            var totalRowCount = $("[id*=grdEmpPaymentList] tr").length;
            if (totalRowCount <= "1" && $("[id$=hdfCurrencyMode]").val() == "1") {
                EnableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
            }
            else {
                DisableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
            }

            //            if ($("[id$=txtCurrency]").attr("disabled") == true) {
            //                DisableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
            //            }
            //            else {
            //                EnableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
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
                $("[id$='pnlDelete']").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
            }
        }
        function AfterDateSelect(controlID) {
            if (controlID == "txtDate") {
                $("[id$=btnCurrency]").click();
            }
        }


        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtCurrency") {
                $("[id$=btnCurrency]").click();
            }
        }

        function ShowHideAdvancedSearch(flag) {
            if (flag) {
                $("[id$=tbladvancedSearch]").hide();
                $("[id$=imbShowFilter]").hide();
                $("[id$=imbHideFilter]").show();
            }
            else {
                $("[id$=tbladvancedSearch]").show();
                $("[id$=imbShowFilter]").show();
                $("[id$=imbHideFilter]").hide();
            }
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
        //Validation Summary
        function ValidatePageNow(valGroup) {
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

        function onAfterDateChangeCallBack() {
            $("[id$=lblDateDay]").show();
            var date = GrandScriptUtils.ConvertDateFormat($("[id$=txtDate]").val());
            var d = new Date(date.split("-").reverse().join("-"));
            var weekdays = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
            $("[id$=lblDateDay]").html(weekdays[d.getDay()]);
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

        // To Disable/Enable Bank ddl Based on Payment Mode
        function SetBankEnable() {
            if ($("[id$=ddlListPaymentMode]").val() > 1) {
                $('#<%= ddlListBankName.ClientID %>').attr('disabled', false);
            }
            else {
                $('#<%= ddlListBankName.ClientID %>').attr('disabled', true);
            }

            var bankvaldtr = document.getElementById("<%= rfvBankName.ClientID %>");
            var chequevaldtr = document.getElementById("<%= rfvChequeDate.ClientID %>");
            var chqdatevaldtr = document.getElementById("<%= rfvChequeNo.ClientID %>");
            if ($("[id$=ddlPaymentMode]").val() < 0 || $("[id$=ddlPaymentMode]").val() == 1 || $("[id$=ddlPaymentMode]").val() == 6) { //1:cash,2:Others
                $('#<%= ddlBankName.ClientID %>').attr('disabled', true);
                $("[id$=ddlBankName]").val(0);
                $("[id$=txtChequeNo]").val("");
                $("[id$=txtChequeDate]").val("");
                ValidatorEnable(bankvaldtr, false);
                ValidatorEnable(chequevaldtr, false);
                ValidatorEnable(chqdatevaldtr, false);

            }
            else {
                $('#<%= ddlBankName.ClientID %>').attr('disabled', false);
                ValidatorEnable(bankvaldtr, true);
                if ($("[id$=ddlPaymentMode]").val() == 2) { //2:Cheque
                    ValidatorEnable(chequevaldtr, true);
                    ValidatorEnable(chqdatevaldtr, true);
                }
            }
            if ($("[id$=ddlFilterPaymentMode]").val() > 1) {
                $('#<%= ddlFilterBankName.ClientID %>').attr('disabled', false);
            }
            else {
                $('#<%= ddlFilterBankName.ClientID %>').attr('disabled', true);
                $("[id$=ddlFilterBankName]").val(0);
            }
        }

        function AfterClose(containerID) {
            if (containerID == "[id$=divJournalize]") {
                $("[id$=btnJournalizeUpdate]").click();
            }
            else if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
                if ($("[id$=hdfJournalizeWorkFlow]").val() == "1") {
                    ShowCommonCotainerDiv('[id$=divJournalize]', '<%= GetLocalResourceObject("Salary_Payment_Journal") %>', "1%");
                    AfterCloseWkfInJournal();
                }
            }
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
    <asp:UpdatePanel runat="server" ID="aupdpnlBasicInfo">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum" Text="<%$ resources:Breadcrumb%>"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="147"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="148"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('Save')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="149" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('Save')" ValidationGroup="invoice"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$resources:Controls,Save %>"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save"
                                            ValidationGroup="Save" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('Save')"
                                            TabIndex="150" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteNew" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="151" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <%--<li runat="server" id="pnlPrint">
                                        <asp:Button runat="server" TabIndex="152" ID="btnPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>--%>
                                    <%--<li runat="server" id="pnlDat">
                                        <asp:Button runat="server" TabIndex="153" ID="btnDat" CommandName="DAT" OnClick="ActionHandler"
                                            Text="<%$resources:DAT %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:DAT %>" />
                                    </li>--%>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            CssClass="popupclose" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" OnClick="ActionHandler"
                                            TabIndex="154" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnJournalize" CommandName="JOURNALIZE" TabIndex="154"
                                            Text="<%$resources:Journalize %>" OnClick="ActionHandler" ToolTip="<%$resources:Journalize %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-journalize" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="155" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="155" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelSP %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelSP %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="156" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="155" ID="btnView" CommandName="VIEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,View %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                    <%--<li>
                                        <asp:Button runat="server" TabIndex="157" ID="Button1" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
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
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblPage" CssClass="asptbllinks">
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
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:ShowFilter%>" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table class="table-devide tablelayout" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S padgtop7 ">
                                            <asp:Label ID="lblFromDate" runat="server" Text="<%$ resources:FromDate%>" AssociatedControlID="txtListFromDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtListFromDate" TabIndex="1" onkeydown="return CheckKey(event)"
                                                onpaste="return false;" CssClass="input-small"></asp:TextBox>
                                            <asp:HiddenField ID="hdfListFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$ resources:ToDate%>" AssociatedControlID="txtListToDate"
                                                CssClass="middle-lbl-small"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtListToDate" TabIndex="2" CssClass="input-small-b"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfListToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7 ">
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" CssClass="middle-lbl-small"
                                                AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-w16per" TabIndex="3">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,NotPosted %>" Value="0" Enabled="<%$ resources:ConfigurationsRes,FinModuleEnabled %>"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Posted %>" Value="1" Enabled="<%$ resources:ConfigurationsRes,FinModuleEnabled %>"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblSearchName" runat="server" Text="<%$ resources:InstrNo%>" AssociatedControlID="txtChequeSrch"></asp:Label>
                                            <asp:TextBox ID="txtChequeSrch" runat="server" CssClass="input-small margnbotm0"
                                                MaxLength="200" onkeydown="limitText(this,200);" onkeyup="limitText(this,200);"
                                                TabIndex="4"></asp:TextBox>
                                            <asp:Label ID="lblTrxNoSearch" runat="server" Text="<%$ resources:TrxNoList%>" AssociatedControlID="txtTrxNo"
                                                CssClass="middle-lbl-small"></asp:Label>
                                            <asp:TextBox ID="txtTrxNo" runat="server" CssClass="input-small-b margnbotm0" TabIndex="5"></asp:TextBox>
                                            <asp:HiddenField ID="hdfTrxPk" runat="server" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label runat="server" ID="lblListPaymentMode" AssociatedControlID="ddlListPaymentMode"
                                                CssClass="middle-lbl-small" Text="<%$resources:PaymentMode %>"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlListPaymentMode" CssClass="select-w16per margnbotm0"
                                                onchange="SetBankEnable();" TabIndex="6">
                                            </asp:DropDownList>
                                            <asp:Label runat="server" ID="lblListBankName" AssociatedControlID="ddlListBankName"
                                                CssClass="middle-lbl-xsmall-a2 margnbotm0" Text="<%$resources:BankName %>"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlListBankName" CssClass="select-w33per margnbotm0"
                                                TabIndex="7" Enabled="false">
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="ImageButton1" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="8"
                                                CommandName="FILTER" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="ImageButton2" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="9" OnClick="ActionHandler"
                                                CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
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
                                                    TabIndex="10" />
                                                <asp:HiddenField runat="server" ID="hdfPSH_PKListPage" Value='<%# Eval("PSH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval("PSH_STATUS") %>' />
                                                <asp:HiddenField ID="hdfDelStatus" runat="server" Value='<%# Eval("PSH_DEL_STATUS") %>' />
                                                <asp:HiddenField ID="hdfJournalStatus" runat="server" Value='<%# Eval("PSH_HAS_JRNL_ENTRY") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                            <HeaderStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TrxNo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("PSH_NO")))?Resources.ErpRes.Draft:Eval("PSH_NO")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("PSH_NO")))?Resources.ErpRes.Draft:Eval("PSH_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvDate" runat="server" Text='<%# Convert.ToDateTime(Eval("PSH_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat)%>'
                                                    ToolTip='<%# Convert.ToDateTime(Eval("PSH_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PaymentMode%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvPaymentMode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("PSH_PAYMNT_MODE_TEXT")),35) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PSH_PAYMNT_MODE_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                            <HeaderStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BankName%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvBankName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PSH_BANK_TEXT"))),45) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PSH_BANK_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                            <HeaderStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Remarks%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvRemarks" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("PSH_REMARKS")),55) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PSH_REMARKS")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="35%" />
                                            <ItemStyle Width="35%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgStatus" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("PSH_CSS_CLASS") %>' ToolTip='<%# Eval("PSH_STATUS_TEXT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval("PSH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgPosted" runat="server" OnClientClick="javascript:return false;"
                                                    Visible="<%$ resources:ConfigurationsRes,FinModuleEnabled %>" CssClass='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? GetLocalResourceObject("unposted").ToString() : Eval("FTH_CSS_CLASS")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("FTH_CSS_CLASS"))) ? Resources.Captions.NotPosted : Eval("FTH_STATUS_TEXT")%>' />
                                                <asp:HiddenField runat="server" ID="hdfPosted" Value='<%# Eval("PSH_HAS_JRNL_ENTRY") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" TabIndex="11" />
                                <div class="clear">
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide tablelayout" id="tblDetailHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblTrxNoHdr" runat="server" Text="<%$ resources:TrxNo%>" AssociatedControlID="lblTrxNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblTrxNo" CssClass="input-small"></asp:Label>
                                            <asp:Label ID="lblDate" runat="server" Text="<%$ resources:DateStar%>" AssociatedControlID="txtDate"
                                                CssClass="lbl-24-3perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDate" TabIndex="5" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDate"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_EnterDate%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="Label2" runat="server" Text="<%$ resources:Controls,CompanyReq%>"
                                                AssociatedControlID="ddlCompany"></asp:Label>
                                            <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="5" CssClass="select-w61per">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvCompany" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlCompany" Display="Dynamic" Text="*" InitialValue="-1"
                                                ValidationGroup="Save" ErrorMessage="<%$ resources:Err_EnterComapany %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblhdrCurrency" Text="<%$ resources:CurrencyReq%>"
                                                AssociatedControlID="txtCurrency"></asp:Label>
                                            <asp:TextBox ID="txtCurrency" runat="server" CssClass="input-small" TabIndex="5"
                                                MaxLength="100" Enabled="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvCurrency" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" EnableClientScript="true" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>"
                                                runat="server" ControlToValidate="txtCurrency" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_Currency %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfCurrency" runat="server" />
                                            <asp:Button ID="btnCurrency" runat="server" OnClick="ActionHandler" CommandName="CURRENCYSELECTED"
                                                Style="display: none" EnableTheming="false" />
                                            <asp:Label runat="server" ID="lblExchangeRate" Text="<%$ resources:ExchangeRateReq%>"
                                                AssociatedControlID="txtExchangeRate" CssClass="lbl-22-6perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtExchangeRate" Text="" TabIndex="5" CssClass="input-small numeric medium"
                                                onkeypress="return validateRateFloatKeyPress(this,event);"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfExchangeRate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" EnableClientScript="true" runat="server" ControlToValidate="txtExchangeRate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExchangeRate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cmpExchangeRate" CssClass="star" SetFocusOnError="true"
                                                Type="Double" Operator="GreaterThan" ValueToCompare="0" ValidationGroup="Save"
                                                EnableClientScript="true" InitialValue="0" runat="server" ControlToValidate="txtExchangeRate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExchangeRate %>">
                                            </asp:CompareValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="Label1" Text="<%$ resources:Remarks%>" AssociatedControlID="txtRemarksHdr"></asp:Label>
                                            <asp:TextBox ID="txtRemarksHdr" runat="server" TabIndex="5" MaxLength="500" CssClass="input-half"
                                                onkeyup="limitText(this,500);"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <div class="search-colapse-b">
                                                <h1>
                                                    <%= GetLocalResourceObject("Details").ToString()%></h1>
                                                <%--<asp:ImageButton runat="server" ID="imbDetShowImportSec" OnClientClick="javascript:return ShowHideDetImportSec(1);"
                                        SkinID="imbArrowInactive" ToolTip="<%$ resources:Show%>" />
                                    <asp:ImageButton runat="server" ID="imbDetHideImportSec" OnClientClick="javascript:return ShowHideDetImportSec(0);"
                                        Style="display: none" SkinID="imbArrowActive" ToolTip="<%$ resources:Hide%>" />--%>
                                            </div>
                                            <%--<div class="clear">
                                </div>--%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblPaymentMode" AssociatedControlID="ddlPaymentMode"
                                                Text="<%$resources:PaymentModeStar %>"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlPaymentMode" onchange="SetBankEnable();"
                                                CssClass="select-small-a1" TabIndex="5">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvPaymentMode" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="add" EnableClientScript="true" InitialValue="-1" runat="server"
                                                ControlToValidate="ddlPaymentMode" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_EnterPaymentMode %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblBankName" AssociatedControlID="ddlBankName" Text="<%$resources:BankName %>"
                                                Enabled="false" CssClass="middle-lbl-b"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlBankName" Enabled="false" CssClass="select-small-a1"
                                                TabIndex="5">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvBankName" CssClass="star" SetFocusOnError="true"
                                                Enabled="false" InitialValue="-1" ValidationGroup="add" EnableClientScript="true"
                                                runat="server" ControlToValidate="ddlBankName" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_EnterBankName %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblChequeNo" runat="server" Text="<%$ resources:InstrNo%>" AssociatedControlID="txtChequeNo"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtChequeNo" TabIndex="5" CssClass="input-small"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvChequeNo" CssClass="star" SetFocusOnError="true"
                                                Enabled="false" ValidationGroup="add" EnableClientScript="true" runat="server"
                                                ControlToValidate="txtChequeNo" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_EnterInstrNo %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblChequeDate" AssociatedControlID="txtChequeDate"
                                                CssClass="lbl-19-6perc" Text="<%$resources:InstrDate %>"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtChequeDate" TabIndex="5" CssClass="input-small"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvChequeDate" CssClass="star" SetFocusOnError="true"
                                                Enabled="false" ValidationGroup="add" EnableClientScript="true" runat="server"
                                                ControlToValidate="txtChequeDate" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_InstrDate %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks%>" AssociatedControlID="txtRemarks"></asp:Label>
                                            <asp:TextBox ID="txtRemarks" runat="server" TabIndex="5" MaxLength="500" TextMode="MultiLine"
                                                Height="30" CssClass="input-full" onkeyup="limitText(this,500);"></asp:TextBox>
                                            <asp:ImageButton ID="imbAdd" runat="server" OnClick="ActionHandler" CommandName="ADD"
                                                ToolTip="Add" OnClientClick="javascript:ValidatePageNow('add')" ValidationGroup="add"
                                                TabIndex="19" SkinID="plus" CssClass="margntop18 margnbotm0 margn-rgt4" />
                                            <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear%>"
                                                ToolTip="<%$ resources:Controls,Clear%>" TabIndex="20" OnClick="ActionHandler"
                                                CommandName="CLEARADD" SkinID="clear-ext" CssClass="margntop18 margnbotm0 margn-rgt4" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <%-- <div class="clear">
                            </div>
                            <div class="inner-tabgroup" style="margin-top: 15px;">
                                <div class="col-md-2 col-sm-2 col-xs-2" style="height: 32px; float: right;">
                                    <asp:Button runat="server" SkinID="btnInner-add" ID="btnShowPopUp" TabIndex="5" OnClick="ActionHandler"
                                        CommandName="SHOWPOPUP" Text="<%$resources:AddEmployee %>" ToolTip="<%$resources:AddEmployee %>"
                                        ValidationGroup="save" CssClass="margntop-1 margn-rgt0" Style="margin-top: -2.5px;" />
                                </div>
                            </div>--%>
                            <div class="clear">
                            </div>
                            <div runat="server" class="gridwrap">
                                <asp:GridView runat="server" ID="grdEmpPaymentList" Width="100%" AllowPaging="false"
                                    OnRowDataBound="ActionHandler" AllowSorting="false" AutoGenerateColumns="false"
                                    ShowFooter="true" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Mode%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdPaymentMode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PSP_MODE_TEXT"))),10) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PSP_MODE_TEXT"))) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfPaymentModPk" Value='<%# Eval("PSP_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfPaymentMode" Value='<%# Eval("PSP_MODE") %>' />
                                                <asp:HiddenField runat="server" ID="hdfBank" Value='<%# Eval("PSP_BANK") %>' />
                                                <%-- <asp:HiddenField runat="server" ID="hdfROW_NO" Value='<%# Eval("ROW_NO") %>' />
                                                <asp:HiddenField runat="server" ID="hdfPSL_FROM_DATE" Value='<%# Eval("PSL_FROM_DATE") %>' />
                                                <asp:HiddenField runat="server" ID="hdfPSL_TO_DATE" Value='<%# Eval("PSL_TO_DATE") %>' />
                                                <asp:HiddenField runat="server" ID="hdfPSL_EPS_PK" Value='<%# Eval("PSL_EPS_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfPSL_PAYROLL_MONTH" Value='<%# Eval("PSL_PAYROLL_MONTH") %>' />--%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblgrdTotal" runat="server" Text="<%$ resources:Total%>">
                                                </asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Bank%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdBank" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PSP_BANK_TEXT"))),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PSP_BANK_TEXT"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InstrNo%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployeeText" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PSP_INSTR_NO"))),15) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PSP_INSTR_NO"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InstrDate%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdInstrDate" runat="server" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("PSP_DATE")))? "": Convert.ToDateTime(Eval("PSP_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat) %>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("PSP_DATE")))? "": Convert.ToDateTime(Eval("PSP_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Remarks%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdRemarks" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PSP_REMARK"))),38) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PSP_REMARK"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="22%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Total%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdTotalSalary" runat="server" Text="" ToolTip=""></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="amount-numeric" />
                                            <HeaderStyle Width="10%" CssClass="amount-numeric" />
                                            <FooterStyle Width="10%" CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblgrdFooterTotalSalary" runat="server" Text="" CssClass="amount-numeric">
                                                </asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:NoOfEmp%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdEmpCount" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblgrdFooterEmpCount" runat="server" Text="" CssClass="amount-numeric">
                                                </asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="5%" Wrap="false" />
                                            <FooterStyle HorizontalAlign="Center" Font-Bold="true" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Actions%> ">
                                            <ItemTemplate>
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_edit" runat="server" ID="imbAddEmp"
                                                    TabIndex="21" SkinID="imbaddnew" EnableViewState="false" CommandName="SHOWPOPUP"
                                                    OnClick="ActionHandler" ToolTip="<%$ resources:AddEmp %>" />
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_edit" runat="server" ID="imbEditDetails"
                                                    TabIndex="21" SkinID="imbeditgrid" EnableViewState="false" CommandName="GRIDEDIT"
                                                    OnClick="ActionHandler" ToolTip="<%$ resources:Controls,Edit %>" />
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbViewAttendance"
                                                    ToolTip="<%$ resources:Controls,View %>" SkinID="btnview" CommandName="GRIDVIEW"
                                                    OnClick="ActionHandler" TabIndex="21" />
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_edit" runat="server" ID="imbgrdPrint"
                                                    TabIndex="21" SkinID="btnPrint" CommandName="GRIDPRINT"
                                                    OnClick="ActionHandler" ToolTip="<%$ resources:Print %>" 
                                                    Visible='<%# Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes","HrmsSalaryPaymentExcelPrint"))==0 && Convert.ToInt32(Eval("PSP_PK")) > 0 ?  true : false %>' />
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_edit" runat="server" ID="imbgrdExcPrint"
                                                    TabIndex="21" SkinID="excel"  CommandName="EXCELPRINT"
                                                    OnClick="ActionHandler" ToolTip="<%$ resources:Excel %>"
                                                     Visible='<%# Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes","HrmsSalaryPaymentExcelPrint"))==1 && Convert.ToInt32(Eval("PSP_PK")) > 0 ?  true : false  %>' />
                                                <asp:ImageButton Width="16px" Height="16px" runat="server" ID="imbDat" TabIndex="21"
                                                    SkinID="imbPayment" CommandName="GRIDDAT" OnClick="ActionHandler" ToolTip="<%$ resources:DAT %>"
                                                    Visible='<%# Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes","hrmsIsSalaryPaymentSIF"))==0 && (Convert.ToInt32(Eval("PSP_PK")) > 0 && !String.IsNullOrEmpty(Convert.ToString(Eval("PSP_BANK"))))?  true : false %>' />
                                                <asp:ImageButton Width="16px" Height="16px" runat="server" ID="imbSif" TabIndex="21"
                                                    SkinID="imbSif" CommandName="GRIDSIF" OnClick="ActionHandler" ToolTip="<%$ resources:SIF %>"
                                                    Visible='<%# Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes","hrmsIsSalaryPaymentSIF"))==1 && (Convert.ToInt32(Eval("PSP_PK")) > 0 )?  true : false %>' />
                                                <asp:ImageButton Width="16px" Height="16px" runat="server" ID="imbDeleteEmployee"
                                                    TabIndex="21" SkinID="imbdeletegrid" CommandName="GRIDDELETE" ToolTip="<%$ resources:Delete%>"
                                                    OnClick="ActionHandler" OnClientClick="return ShowDeleteConfirm(this);" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <ItemStyle Width="10%" HorizontalAlign="Right" Wrap="false" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
            </div>
            <div id="divPopUpSalaryPaymentDetails" style="display: none">
                <div class="content-wrapper">
                    <div class="Button-container-popup">
                        <asp:Button ID="btnAddMenu" SkinID="btnInner-add-dsd" runat="server" Text="<%$resources:Controls,Add_Add %>"
                            CommandName="SAVE_ACTIONPOPUP" OnClick="ActionHandler" TabIndex="10" />
                        <asp:Button ID="btnCancelPopUp" runat="server" CommandName="CANCELPOPUP" OnClick="ActionHandler"
                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Close %>" TabIndex="11"
                            Text="<%$Resources:Controls,Close%>" />
                    </div>
                    <div id="divPaymentFilterDetails" runat="server">
                        <table class="table-devide tablelayout">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label runat="server" ID="lblCompany" AssociatedControlID="ddlFilterCompany"
                                            Text="<%$resources:Company%>"></asp:Label><asp:DropDownList ID="ddlFilterCompany"
                                                runat="server" TabIndex="6" CssClass="select-small-h">
                                            </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblProcessMode" runat="server" Text="<%$ resources:ProcessModeReq%>"
                                            AssociatedControlID="ddlProcessMode"></asp:Label>
                                        <asp:DropDownList ID="ddlProcessMode" runat="server" TabIndex="4" CssClass="select-small-c"
                                            AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvProcessMode" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="Search" EnableClientScript="true" InitialValue="" runat="server"
                                            ControlToValidate="ddlProcessMode" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ProcessMode %>">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblPayrollType" runat="server" Text="<%$ resources:PayrollTypeReq%>"
                                            AssociatedControlID="ddlPayrollType"></asp:Label>
                                        <asp:DropDownList ID="ddlPayrollType" runat="server" TabIndex="5" CssClass="select-small-h">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvPayrollType" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="Search" EnableClientScript="true" InitialValue="-1" runat="server"
                                            ControlToValidate="ddlPayrollType" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PayrollType %>">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label runat="server" ID="lblEmployeeType" AssociatedControlID="ddlFilterEmployeeType"
                                            Text="<%$resources:EmployeeType%>"></asp:Label><asp:DropDownList runat="server" ID="ddlFilterEmployeeType"
                                                CssClass="select-small-h" TabIndex="6">
                                            </asp:DropDownList>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblBranchLocation" runat="server" Text="<%$ resources:BranchLocation%>"
                                            AssociatedControlID="ddlFilterBranchLocation"></asp:Label><asp:DropDownList ID="ddlFilterBranchLocation"
                                                runat="server" TabIndex="6" CssClass="select-small-h">
                                            </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblEmploymentType" runat="server" Text="<%$ resources:EmploymentType%>"
                                            AssociatedControlID="ddlFilterEmploymentType"></asp:Label><asp:DropDownList ID="ddlFilterEmploymentType"
                                                runat="server" TabIndex="6" CssClass="select-small-h">
                                            </asp:DropDownList>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label runat="server" ID="lblFilterPaymentMode" AssociatedControlID="ddlFilterPaymentMode"
                                            Text="<%$resources:PaymentMode %>"></asp:Label>
                                        <asp:DropDownList runat="server" ID="ddlFilterPaymentMode" CssClass="select-small-ax"
                                            TabIndex="6" onchange="SetBankEnable();">
                                        </asp:DropDownList>
                                        <asp:Label runat="server" ID="SalaryMonth" Text="<%$ resources:SalaryMonth%>" AssociatedControlID="txtSalaryMonth"
                                            CssClass="lbl-23perc"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtSalaryMonth" TabIndex="6" CssClass="input-w13-8per"
                                            onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                        <cc2:CalendarExtender runat="server" ID="txtSalaryMonth_CalendarExtender" BehaviorID="calendar1"
                                            TargetControlID="txtSalaryMonth" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                                            ClientIDMode="Static" OnClientHidden="onCalendarHidden">
                                        </cc2:CalendarExtender>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label runat="server" ID="lblFilterBankName" Text="<%$ resources:EmployeeBank%>"
                                            AssociatedControlID="ddlFilterBankName"></asp:Label><asp:DropDownList runat="server"
                                                ID="ddlFilterBankName" CssClass="select-small-h" TabIndex="7">
                                            </asp:DropDownList>
                                        <div class="display-inline">
                                            <asp:ImageButton ID="btnFilterSearch" runat="server" Text="<%$ resources:Search%>"
                                                ToolTip="<%$ resources:Search%>" OnClick="ActionHandler" TabIndex="8" CommandName="SEARCH"
                                                SkinID="search-ext" CssClass="margntop2 margnlft-minus4" ValidationGroup="Search"
                                                OnClientClick="javascript:ValidatePageNow('Search')" />
                                            <asp:ImageButton ID="btnFilterClear" runat="server" Text="<%$ resources:Clear%>"
                                                ToolTip="<%$ resources:Clear%>" TabIndex="9" OnClick="ActionHandler" CommandName="CLEARDETAIL"
                                                SkinID="clear-ext" CssClass="margntop2 margnlft-minus4 margnrgt1-2per" />
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divEmpSalPopup" runat="server" class="gridwrap maxh-290">
                        <asp:GridView runat="server" ID="grdEmpPayment_PopUp" Width="100%" AllowPaging="false"
                            AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                            EmptyDataRowStyle-CssClass="emptytable" OnRowCommand="ActionHandler">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty_PopUp" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label></EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkEmpHeader_PopUp" runat="server" ToolTip="Select for Process"
                                            TabIndex="15" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkEmpselect_PopUp" TabIndex="16" Checked='<%# Convert.ToInt32(Eval("PSL_PK")) > 0 ? true : false %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="2%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:SalaryMonth%> " SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPayroll_Month_PopUp" runat="server" Text='<%# Convert.ToDateTime(Eval("PSL_PAYROLL_MONTH")).ToString(Resources.Constants.DateFormatMonthYear)%>'
                                            ToolTip='<%# Convert.ToDateTime(Eval("PSL_PAYROLL_MONTH")).ToString(Resources.Constants.DateFormatMonthYear) %>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdfPSL_PAYROLL_MONTH_PopUp" Value='<%# Eval("PSL_PAYROLL_MONTH") %>' />
                                        <asp:HiddenField runat="server" ID="hdfPSL_PK_PopUp" Value='<%# Eval("PSL_PK") %>' />
                                        <asp:HiddenField runat="server" ID="hdfPSL_PSH_PK_PopUp" Value='<%# Eval("PSL_PSH_PK") %>' />
                                        <asp:HiddenField runat="server" ID="hdfPSL_EMPLOYEE_PK_PopUp" Value='<%# Eval("PSL_EMPLOYEE") %>' />
                                        <asp:HiddenField runat="server" ID="hdfROW_NO_PopUp" Value='<%# Eval("ROW_NO") %>' />
                                        <asp:HiddenField runat="server" ID="hdfPSL_TO_DATE_PopUp" Value='<%# Eval("PSL_TO_DATE") %>' />
                                        <asp:HiddenField runat="server" ID="hdfPSL_FROM_DATE_PopUp" Value='<%# Eval("PSL_FROM_DATE") %>' />
                                        <asp:HiddenField runat="server" ID="hdfPSL_BANK_PopUp" Value='<%# Eval("PSL_BANK") %>' />
                                        <asp:HiddenField runat="server" ID="hdfPSL_BANK_TEXT_PopUp" Value='<%# Eval("PSL_BANK_TEXT") %>' />
                                        <asp:HiddenField runat="server" ID="hdfPSL_ACCOUNT_NO_PopUp" Value='<%# Eval("PSL_ACCOUNT_NO") %>' />
                                        <asp:HiddenField runat="server" ID="hdfPSL_BANK_IFSC_PopUp" Value='<%# Eval("PSL_BANK_IFSC") %>' />
                                        <asp:HiddenField runat="server" ID="hdfPSL_EPS_PK_PopUp" Value='<%# Eval("PSL_EPS_PK") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle Width="8%" Wrap="false" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Period%> " SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPeriod_PopUp" runat="server" Text='<%# Convert.ToDateTime(Eval("PSL_FROM_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat) + " To " + Convert.ToDateTime(Eval("PSL_TO_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat)%>'
                                            ToolTip='<%#  Convert.ToDateTime(Eval("PSL_FROM_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat) + " To " + Convert.ToDateTime(Eval("PSL_TO_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat)%>'></asp:Label></ItemTemplate>
                                    <HeaderStyle Width="18%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Employee%> " SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmployeeText_PopUp" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PSL_EMPLOYEE_TEXT"))),30) %>'
                                            ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PSL_EMPLOYEE_TEXT"))) %>'></asp:Label></ItemTemplate>
                                    <HeaderStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Designation%> " SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDesignation_PopUp" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empDesignationText"))),22) %>'
                                            ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empDesignationText"))) %>'></asp:Label></ItemTemplate>
                                    <HeaderStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:NetSalary%> " SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPSL_NET_SAL_PopUp" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("PSL_NET_SAL"))%>'
                                            ToolTip='<%#GetFormattedCurrencyWithComma(Eval("PSL_NET_SAL"))%>'></asp:Label></ItemTemplate>
                                    <ItemStyle CssClass="amount-numeric" />
                                    <HeaderStyle Width="10%" CssClass="amount-numeric" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Actions%>" SortExpression="" Visible="false">
                                    <ItemTemplate>
                                        <asp:ImageButton Width="16px" Height="16px" runat="server" ID="imbDeleteEmployeePopup"
                                            SkinID="imbdeletegrid" CommandName="DELETE_ACTION" ToolTip="<%$ resources:Delete%>"
                                            OnClick="ActionHandler" OnClientClick="return ShowDeleteConfirm(this);" /></ItemTemplate>
                                    <ItemStyle CssClass="amount-numeric" />
                                    <HeaderStyle Width="5%" CssClass="amount-numeric" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div id="divScriptButtons">
                <asp:Button runat="server" ID="btnJournalize_Action" CommandName="JOURNALIZE" OnClick="ActionHandler"
                    EnableTheming="false" Style="display: none" />
                <asp:Button ID="btnJournalizeUpdate" runat="server" OnClick="ActionHandler" CommandName="JOURNALIZEUPDATE"
                    EnableTheming="false" Style="display: none" />
            </div>
            <div id="diverrorAlert" style="display: none">
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
                <asp:ValidationSummary ID="vsAddToList" ValidationGroup="add" runat="server" />
                <asp:ValidationSummary ID="vsEmpSearch" ValidationGroup="Search" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label></div>
            <div id="divJournalize" style="display: none">
                <uc1:Journalize ID="ucrJournalize" runat="server" />
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="SaveAddDed">
                </uc1:WorkflowUserComments>
            </div>
            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormatWithComma" runat="server" />
            <asp:HiddenField ID="hdfAdvSearch" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
            <asp:HiddenField ID="hdfJournalizeWorkFlow" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIsCancelled" Value="0" runat="server" />
            <asp:HiddenField ID="hdfExchangeRateFormat" runat="server" />
            <asp:HiddenField ID="hdfCurrencyMode" runat="server" />
        </ContentTemplate>
        <%--  <Triggers>            
            <asp:PostBackTrigger ControlID="btnDat" />           
        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
