<%@ Page Title="<%$ Resources:Captions,Title_Depreciation %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="Depreciation.aspx.cs" Inherits="ERPSMS_v01.Finance.Depreciation"
    Theme="ClassicExt" ValidateRequest="false" %>

<%@ Register Src="~/Journalize/UserControls/JournalizeControlNew.ascx" TagName="Journalize"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /* This class is used only with cancelled icon if only cancelled icon in that action cell */
        .canceled-grey-margin-adjust { margin-top: 5px; }
    </style>
    <script type="text/javascript">
        var msgTitle = '<%= Resources.ErpRes.Information %>';
        var msgContent = "";


        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");


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
                $("[id$=btnSaveSubmit]").hide();
                $("[id$=pnlDelete]").hide();
                $("[id$=btnJournalize]").hide();
                $("[id$=btnGetAsset]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
                $("[id$=btnJournalize]").hide();
                $("[id$=btnPrint]").hide();
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
        var validationArrayGroup;
        function CheckValidationDuplicate(valGroup) {
            validationArrayGroup = new Array();
            for (var i = Page_Validators.length - 1; i >= 0; i--) {
                if (typeof (Page_Validators[i].validationGroup) == "string") {
                    if (valGroup == Page_Validators[i].validationGroup) {
                        if (!CheckValidationExists(Page_Validators[i].id)) {
                            validationArrayGroup.push(Page_Validators[i].id);
                        }
                        else {
                            Page_Validators.splice(i, 1);
                        }
                    }
                    else {
                        Page_Validators.splice(i, 1);
                    }
                }
            }
        }
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
        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtDepreDate");

            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            GrandScriptUtils.AddDateRangeCommon("txtFromDateEntry", "hdfFromDateEntry", "txtToDateEntry", "hdfToDateEntry", false, false);
            GrandScriptUtils.AddDateRangeCommon("txtPopUpPurchaseDateFrom", "hdfPurchaseFromDate", "txtPopUpPurchaseDateTo", "hdfPurchaseToDate", false, false);

            GrandScriptUtils.DatePickerCommon("txtPVDate");

            GrandScriptUtils.MakeAutoCompleteDDL("txtDepNo", url, "hdfDeprPK", true, true, "DEPRECIATIONAUTOGET");

            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();

            //Set a stamp for cancelled record
            if ($("[id$=hdfIsCancelled]").val() == "1")
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
            else
                $("[id$=tblDetailHdr]").addClass("table-devide");

            //End
        }

        function AfterClose(containerID) {
            if (containerID == "[id$=divJournalize]") {
                $("[id$=btnJournalizeUpdate]").click();
            }
            else if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
                if ($("[id$=hdfJournalizeWorkFlow]").val() == "1") {
                    ShowContainerDiv('[id$=divJournalize]', '<%= GetLocalResourceObject("Purchase_Expense_Journal") %>', '1000', '550');
                    AfterCloseWkfInJournal();
                }
            }
        }

        function CalculateTotal() {
            var DepreciationAmt = 0.0;
            var DecimalDigits = 2;
            var DecimalDigitsFive = 5;


            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }

            $("#[id*=grdSelectedAssets] input[type=text][id*=txtAssetDepreAmt]").each(function (index) {
                if ($.trim($(this).val()) != "") {
                    if (!isNaN(parseFloat($(this).val()))) {
                        var _amt = parseFloat($(this).val().replace(new RegExp(',', 'g'), ''));
                        DepreciationAmt = DepreciationAmt + _amt;
                    }
                }
            });
            $("#[id*=grdSelectedAssets] [id*=lblTotalAssetDepreAmtFooter]").html(GrandScriptUtils.ToFixed(DepreciationAmt, DecimalDigitsFive));
            $("#[id*=grdSelectedAssets] [id*=lblTotalAssetDepreAmtFooter]").attr('title', GrandScriptUtils.ToFixed(DepreciationAmt, DecimalDigitsFive));

        }

        // Select All CheckBox For Grod
        $("[id*=chkHeader]").live("click", function () {
            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (chkHeader.is(":checked")) {
                    if ($(this).closest('tr').find("[id$=hdfIsAlreadyExist]").val() != "1") {
                        $(this).attr("checked", "checked");
                    }
                } else {
                    $(this).removeAttr("checked");
                }
            });
        });
        $("[id*=chkSelect]").live("click", function () {
            var grid = $(this).closest("table");
            var chkHeader = $("[id*=chkHeader]", grid);
            if (!$(this).is(":checked")) {
                chkHeader.removeAttr("checked");
            } else {
                if ($("[id*=chkSelect]", grid).length == $("[id*=chkSelect]:checked", grid).length) {
                    chkHeader.attr("checked", "checked");
                }
            }
        });
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
    <asp:UpdatePanel runat="server" ID="aupdpnlExpenses">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <div id="divSBUCompany" class="buttoncontainer-fields floatLeft">
                                    <asp:DropDownList ID="ddlCompany" class="select-full-a margnbotm0" TabIndex="0" runat="server"
                                        onmouseover="javascript:ShowTooltip('ddlCompany');">
                                    </asp:DropDownList>
                                </div>
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="30"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="31"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('save')"
                                            ValidationGroup="save" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="32" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('save')" ValidationGroup="save"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="33" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('save')" ValidationGroup="save"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteNew" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="34" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="35" SkinID="btnInner-Cancel"
                                            ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnJournalize" CommandName="JOURNALIZE" TabIndex="36"
                                            Text="<%$resources:Journalize %>" OnClick="ActionHandler" ToolTip="<%$resources:Journalize %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-journalize" />
                                    </li>
                                    <li id="pnlPrint" runat="server">
                                        <asp:Button runat="server" TabIndex="37" ID="btnPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Visible="true" Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li runat="server" id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="38" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelDepre %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelDepre %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="39" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="40" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="41" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="42" ID="btnListPrint" CommandName="PRINTLISTING"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
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
                                CommandArgument="SEC_ActionPanel" TabIndex="79" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="80" OnClick="ActionHandler" CommandName="DETAIL"
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
                                                TabIndex="45" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="45" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn----------%>
                            <div class="clear">
                            </div>
                            <table class="table-devide " id="tbladvancedSearch">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblFrmDate" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtFromDate" CssClass="lbl-14-1perc"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="1" CssClass="input-small margnbotm0"
                                                MaxLength="13" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtToDate"
                                                class="middle-lbl-xsmall-c2"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="2" CssClass="input-small margnbotm0"
                                                MaxLength="13" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:Label ID="lblAssetTypeSrch" runat="server" Text="<%$resources:AssetType %>" AssociatedControlID="ddlAssetTypeSearch"
                                                class="middle-lbl-xsmall-c5"></asp:Label>
                                            <asp:DropDownList ID="ddlAssetTypeSearch" runat="server" CssClass="medium3 margnbotm0" TabIndex="3">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblDepreciation" runat="server" Text="<%$resources:DepreciationNo %>"
                                                CssClass="middle-lbl-small-a" AssociatedControlID="txtDepNo"></asp:Label>
                                            <asp:TextBox ID="txtDepNo" runat="server" CssClass="select-small-b margnbotm0" MaxLength="100"
                                                TabIndex="52"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfDeprPK" runat="server" Value="" />
                                            <asp:Label runat="server" ID="lblPlantList" CssClass="lbl-6perc" Text="<%$ resources:Plant%>"
                                                AssociatedControlID="ddlPlantList"></asp:Label>
                                            <asp:DropDownList runat="server" CssClass="margnbotm0" ID="ddlPlantList" TabIndex="14" />
                                            <asp:Label runat="server" ID="Label1" Text="<%$ resources:Status%>" AssociatedControlID="ddlStatus"
                                                CssClass="lbl-6perc"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="medium2 margnbotm0" TabIndex="3">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,NotPosted %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Posted %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="btnSearch1" runat="server" Text="" ToolTip="<%$resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="4" CommandName="SEARCH" SkinID="search-ext"
                                                Style="margin-bottom: 0px!important; margin-top: 2px;" />
                                            <asp:ImageButton ID="btnClear1" runat="server" Text="" ToolTip="<%$resources:Controls,Clear %>"
                                                TabIndex="5" OnClick="ActionHandler" CommandName="CLEAR" CssClass="margntop2"
                                                SkinID="clear-ext" Style="margin-bottom: 0px!important;" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdDepreTransactionList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                    OnPageIndexChanging="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" TabIndex="54" runat="server" GroupName="SelectOne"
                                                    AutoPostBack="true" OnCheckedChanged="ActionHandler" ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfDePreTranID" Value='<%# Eval(Resources.DataFieldRes.DepreTranPK) %>' />
                                                <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval(Resources.DataFieldRes.DepreTranDept) %>' />
                                                <asp:HiddenField ID="hdfDelStatus" runat="server" Value='<%# Eval(Resources.DataFieldRes.DepreTranDelStatus) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DepreTranDate %>" SortExpression="<%$ resources:DataFieldRes,DepreTranDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDepreTranDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.DepreTranDate, Resources.Constants.DateFormatGrid)  %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.DepreTranDate, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DepreTranNo %>" SortExpression="<%$ resources:DataFieldRes,DepreTranNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDepreTranNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.DepreTranNo) ==""?"[NEW]":Eval(Resources.DataFieldRes.DepreTranNo)%>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.DepreTranNo)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DeprePeriodFrom %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeprePeriodFrom" runat="server" Text='<%# Eval(Resources.DataFieldRes.DepreFromDate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%#  Eval(Resources.DataFieldRes.DepreFromDate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DeprePeriodTo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeprePeriodTo" runat="server" Text='<%# Eval(Resources.DataFieldRes.DepreToDate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.DepreToDate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AssetType %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("FDH_ASSET_TYPE_TEXT").ToString()),50)%>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("FDH_ASSET_TYPE_TEXT").ToString())%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="36%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Plant %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPlant" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("CMP_DISPLAY_NAME").ToString()),50)%>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("CMP_DISPLAY_NAME").ToString())%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DepreAmount %>">
                                            <ItemTemplate>
                                                <%--                                                <asp:Label ID="lblDepreAmount" runat="server" Text='<%# Eval(Resources.DataFieldRes.DepreAmount,this.GetCurrencyFormat()) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.DepreAmount,this.GetCurrencyFormat()) %>'></asp:Label>--%>
                                                <asp:Label ID="lblDepreAmount" runat="server" Text='<%# Eval(Resources.DataFieldRes.DepreAmount) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.DepreAmount) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval(Resources.DataFieldRes.DepreWorkFlowCssClas) %>' ToolTip='<%# Eval(Resources.DataFieldRes.DepreWorkflowStatus) %>' />
                                                <asp:Button ID="imgPosted" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# string.IsNullOrEmpty(Convert.ToString(Eval(Resources.DataFieldRes.DepreVoucherStatusCss))) ? GetLocalResourceObject("unposted").ToString() : Eval(Resources.DataFieldRes.DepreVoucherStatusCss)%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval(Resources.DataFieldRes.DepreVoucherStatus))) ? Resources.Captions.NotPosted : Eval(Resources.DataFieldRes.DepreVoucherStatus)%>' />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval(Resources.DataFieldRes.DepreApproved) %>' />
                                                <asp:HiddenField runat="server" ID="hdfPosted" Value='<%# Eval(Resources.DataFieldRes.DeprePosted) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" Visible="true" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <table class="table-devide" id="tblDetailHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblDepreNo" runat="server" Text="<%$ resources:No%>" AssociatedControlID="lblDepreciationNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblDepreciationNo" CssClass="input-small"></asp:Label>
                                            <asp:HiddenField ID="hdfDepreciationNo" runat="server" Value="" />
                                            <asp:Label runat="server" ID="lblDepreDate" Text="<%$ resources:DateReq%>" AssociatedControlID="txtDepreDate"
                                                CssClass="lbl-34perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDepreDate" CssClass="input-small Uidate-picker"
                                                TabIndex="2" onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"
                                                onchange="AfterDateSelect(null)"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="reqInvDateEntry" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="save" EnableClientScript="true" runat="server" ControlToValidate="txtDepreDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ReqInvoiceDt %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">

                                            <div id="divDate" runat="server">
                                                <asp:Label runat="server" ID="lblFromDateEntry" Text="<%$ resources:FromDateReq%>"
                                                    AssociatedControlID="txtFromDateEntry"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtFromDateEntry" CssClass="input-small" TabIndex="3"
                                                    onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(null)"></asp:TextBox>
                                                <asp:Label runat="server" ID="lblToDateEntry" Text="<%$ resources:ToDateReq%>" AssociatedControlID="txtToDateEntry"
                                                    CssClass="lbl-19-4perc"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtToDateEntry" CssClass="input-small" TabIndex="4"
                                                    onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(null)"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="reqFromDateEntry" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="save" EnableClientScript="true" runat="server" ControlToValidate="txtFromDateEntry"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ReqFromDt %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="reqToDateEntry" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="save" EnableClientScript="true" runat="server" ControlToValidate="txtToDateEntry"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ReqToDt %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <div id="divMonth" runat="server">
                                                <asp:Label ID="lblDepreMonth" runat="server" Text="<%$ resources:DepreMonth%>"
                                                    AssociatedControlID="txtDepreMonth" CssClass="lbl-49perc"></asp:Label>
                                                <asp:TextBox ID="txtDepreMonth" runat="server" CssClass="input-w10per" TabIndex="2"
                                                    onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                                <cc2:CalendarExtender runat="server" ID="txtDepreMonth_CalendarExtender" BehaviorID="calendar1"
                                                    TargetControlID="txtDepreMonth" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                                                    ClientIDMode="Static" OnClientHidden="onCalendarHidden">
                                                </cc2:CalendarExtender>
                                                <asp:RequiredFieldValidator ID="vrfSalMonth" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="Depreciation" EnableClientScript="true" runat="server" ControlToValidate="txtDepreMonth"
                                                    Display="Static" Text="*" ErrorMessage="<%$ resources:Err_DepreMonth %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblDescription" runat="server" Text="<%$ resources:Description %>"
                                                AssociatedControlID="txtDescription"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDescription" MaxLength="500" TabIndex="5" TextMode="MultiLine"
                                                CssClass="input-full" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"
                                                onpaste="limitText(this,500);"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblSelectAsset" runat="server" Text="" CssClass="assetcnt-text-style-change"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:Button Text="<%$ resources:AddAssetsToList %>" runat="server" ID="btnGetAsset"
                                            ToolTip="<%$ resources:AddAssetsToList %>" TabIndex="6" CommandName="GETLIST"
                                            SkinID="btnInner-add" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('Depreciation');" CommandArgument="PageAction_Entry"
                                            ValidationGroup="Depreciation" Style="margin-right: 0px!important;" />
                                    </td>
                                </tr>
                            </table>
                            <h3 class="fontWGT-Nrml">
                                <%= GetLocalResourceObject("SelectedAssetsCaption").ToString()%></h3>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdSelectedAssets" Width="100%" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable" ShowFooter="true" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:SlNo %>" ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AssetCode %>">
                                            <ItemTemplate>
                                                <asp:HiddenField runat="server" ID="hdfDepreDetailPK" Value='<%# Eval(Resources.DataFieldRes.DepreDetailsPK) %>' />
                                                <asp:HiddenField runat="server" ID="hdfAssetPK" Value='<%# Eval(Resources.DataFieldRes.AssetPK) %>' />
                                                <asp:Label ID="lblAssetCode" runat="server" Text='<%# Eval(Resources.DataFieldRes.AssetCode) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.AssetCode)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AssetName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.AssetName).ToString()),50) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.AssetName).ToString())%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="23%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AssetType %>">
                                            <ItemTemplate>
                                                <asp:HiddenField runat="server" ID="hdfAssetType" Value='<%# Eval(Resources.DataFieldRes.AssetType) %>' />
                                                <asp:Label ID="lblAssetTypeText" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.AssetTypeText).ToString()),28)%>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.AssetTypeText).ToString())%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="18%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PurchaseDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetPurchaseDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.AssetPurchaseDate, Resources.Constants.DateFormatGridExpanded)%>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.AssetPurchaseDate, Resources.Constants.DateFormatGridExpanded)%>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalCaption" Text="<%$ resources:DepreTotal %>"></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AssetCost %>">
                                            <ItemTemplate>
                                                <asp:HiddenField runat="server" ID="hdfAssetCost" Value='<%# Eval(Resources.DataFieldRes.AssetPurchaseCost) %>' />
                                                <asp:Label ID="lblAssetCost" runat="server" Text='<%# Eval(Resources.DataFieldRes.AssetLandCost,this.GetCurrencyFormat()) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.AssetLandCost,this.GetCurrencyFormat()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalAssetCostAmtFooter"></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle Width="6%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CostDeprePercentage %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCostDeprePercentage" runat="server" Text='<%# Eval(Resources.DataFieldRes.AssetCostDeprePercentage) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.AssetCostDeprePercentage,"{0:f2}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AssetDepreAmt %>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAssetDepreAmt" runat="server" CssClass="medium numeric" Text='<%# Eval(Resources.DataFieldRes.AssetDepreAmount) %>'
                                                    onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);" onkeyup="CalculateTotal();"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalAssetDepreAmtFooter"></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle Width="9%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                                                    SkinID="delete-icon" ToolTip="Delete" CommandArgument="PageAction_Entry" OnClientClick="return ShowDeleteConfirm(this);"
                                                    OnPreRender="btnAction_PreRender" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <%--PopupHeader_SelectAsset--%>
            <div id="divPckAssetPopupContainer" style="display: none;">
                <div class="Button-container-popup">
                    <asp:Button runat="server" ID="btnAddToList" CommandName="ADDTOLIST" TabIndex="18"
                        Text="<%$resources:Controls,AddToList %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('AddToList');"
                        ToolTip="<%$resources:Controls,AddToList %>" ValidationGroup="AddToList" SkinID="btnInner-add" />
                </div>
                <div class="contentwrapper">
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label runat="server" ID="Label4" Text="<%$ resources:PurchaseDateFrom%>" AssociatedControlID="txtPopUpPurchaseDateFrom"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtPopUpPurchaseDateFrom" CssClass="medium" TabIndex="10"
                                        onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(null)"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <asp:Label runat="server" ID="lblPopUpAssetCode" Text="<%$ resources:AssetCodePopup%>"
                                        AssociatedControlID="txtPopUpAssetCode"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtPopUpAssetCode" TabIndex="12"> </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <asp:Label runat="server" ID="lblAssetType" Text="<%$ resources:PopUpAssetType%>"
                                        AssociatedControlID="ddlAssetType"></asp:Label>
                                    <asp:DropDownList runat="server" ID="ddlAssetType" TabIndex="14" />
                                    <div class="clear">
                                    </div>
                                    <asp:Label runat="server" ID="lblPlant" Text="<%$ resources:Plant%>"
                                        AssociatedControlID="ddlPlant"></asp:Label>
                                    <asp:DropDownList runat="server" ID="ddlPlant" TabIndex="14" />
                                    <div class="starwrap">
                                        <asp:RequiredFieldValidator ID="reqPlant" InitialValue="-1" SetFocusOnError="true"
                                            ValidationGroup="Plant" CssClass="star" EnableClientScript="true" runat="server"
                                            ControlToValidate="ddlPlant" Text="*" ErrorMessage="<%$ resources:Err_Plant %>"
                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label runat="server" ID="Label5" Text="<%$ resources:PurchaseDateTo%>" AssociatedControlID="txtPopUpPurchaseDateTo"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtPopUpPurchaseDateTo" CssClass="medium" TabIndex="11"
                                        onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(null)"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <asp:Label runat="server" ID="lblPopUpAssetName" Text="<%$ resources:AssetNamePopup%>"
                                        AssociatedControlID="txtPopUpAssetName"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtPopUpAssetName" TabIndex="13"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <asp:Label runat="server" ID="lblPopupLocation" Text="<%$ resources:Location%>" AssociatedControlID="ddlLocation"></asp:Label>
                                    <asp:DropDownList runat="server" ID="ddlLocation" TabIndex="15" />
                                    <div class="clear">
                                    </div>
                                    <asp:Label runat="server" ID="lblCountAssets" Text="" CssClass="assetcnt-text-style-change"></asp:Label>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <div style="margin-right: 6px !important; display: none;">
                                    <asp:Button ID="btnPrevAssetSearch" runat="server" Text="<%$ resources:Controls,PrevPendAsset %>"
                                        ToolTip="<%$ resources:Controls,PrevPendAsset %>" OnClick="ActionHandler" TabIndex="16" OnClientClick="javascript:ValidatePageNow('Plant')"
                                        CommandName="PREVASSETSEARCH" SkinID="btnInner-search" />
                                </div>
                            </td>
                            <td colspan="2" align="right">
                                <div style="margin-right: 6px !important;">
                                    <asp:Button ID="btnAssetSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                        ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="16" OnClientClick="javascript:ValidatePageNow('Plant')"
                                        CommandName="ASSETSEARCH" SkinID="btnInner-search" />
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="gridwrap">
                        <asp:GridView runat="server" ID="grdAssetList" Width="100%" AutoGenerateColumns="false"
                            TabIndex="17" EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox Text="" runat="server" Checked="false" ID="chkHeader" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox Text="" runat="server" Checked="false" ID="chkSelect" />
                                        <asp:HiddenField ID="hdfIsAlreadyExist" runat="server" Value='<%# Eval("IS_ENTRY_VALIDATE") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="2%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:SlNo %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRowNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.AssetRowNo) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="2%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:AssetCode %>">
                                    <ItemTemplate>
                                        <asp:HiddenField runat="server" ID="hdfAssetPK" Value='<%# Eval(Resources.DataFieldRes.AssetPK) %>' />
                                        <asp:Label ID="lblAssetCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.AssetCode).ToString()),15) %>'
                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.AssetCode).ToString())%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="12%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:AssetName %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.AssetName).ToString()),23) %>'
                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.AssetName).ToString())%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="23%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:AssetTypePopup %>">
                                    <ItemTemplate>
                                        <asp:HiddenField runat="server" ID="hdfAssetType" Value='<%# Eval(Resources.DataFieldRes.AssetType) %>' />
                                        <asp:Label ID="lblAssetTypeText" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.AssetTypeText).ToString()),15)%>'
                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.AssetTypeText).ToString())%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="17%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:PurchaseDate %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetPurchaseDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.AssetPurchaseDate, Resources.Constants.DateFormatGrid)%>'
                                            ToolTip='<%# Eval(Resources.DataFieldRes.AssetPurchaseDate, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="11%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:AssetCost %>">
                                    <ItemTemplate>
                                        <asp:HiddenField runat="server" ID="hdfPurchaseCost" Value='<%# Eval(Resources.DataFieldRes.AssetPurchaseCost) %>' />
                                        <asp:HiddenField runat="server" ID="hdfLandCost" Value='<%# Eval(Resources.DataFieldRes.AssetLandCost) %>' />
                                        <asp:Label ID="lblAssetCost" runat="server" Text='<%# Eval(Resources.DataFieldRes.AssetLandCost,this.GetCurrencyFormat()) %>'
                                            ToolTip='<%# Eval(Resources.DataFieldRes.AssetLandCost,this.GetCurrencyFormat()) %>'></asp:Label>
                                        <%--<asp:Label ID="lblAssetCost" runat="server" Text='<%# Eval(Resources.DataFieldRes.AssetPurchaseCost,this.GetCurrencyFormat()) %>'
                                            ToolTip='<%# Eval(Resources.DataFieldRes.AssetPurchaseCost,this.GetCurrencyFormat()) %>'></asp:Label>--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="9%" CssClass="amount-numeric" />
                                    <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:CostDeprePercentage %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCostDeprePercentage" runat="server" Text='<%# Eval(Resources.DataFieldRes.AssetCostDeprePercentage) %>'
                                            ToolTip='<%# Eval(Resources.DataFieldRes.AssetCostDeprePercentage) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" CssClass="amount-numeric" />
                                    <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:AssetDepreAmtPopup %>">
                                    <ItemTemplate>
                                        <asp:HiddenField runat="server" ID="hdfAssetDepreAmt" Value='<%# Eval(Resources.DataFieldRes.AssetDepreAmount) %>' />
                                        <asp:Label ID="lblAssetDepreAmt" runat="server" Text='<%# Eval(Resources.DataFieldRes.AssetDepreAmount) %>'
                                            ToolTip='<%# Eval(Resources.DataFieldRes.AssetDepreAmount) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" CssClass="amount-numeric" />
                                    <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div id="diverror" style="display: none">
                <%--Use this label to bind the server errors--%>
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="save" runat="server" />
                <asp:ValidationSummary ID="vsAddToGrp" ValidationGroup="AddToList" runat="server" />
                <asp:ValidationSummary ID="vvsPlant" ValidationGroup="Plant" runat="server" />
                <asp:ValidationSummary ID="vsDepreciation" ValidationGroup="Depreciation" runat="server" />
            </div>
            <div id="divJournalize" style="display: none">
                <uc1:Journalize ID="ucrJournalize" runat="server" />
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="invoice">
                </uc1:WorkflowUserComments>
            </div>
            <asp:HiddenField ID="hdfDecimalDigits" runat="server" />
            <asp:HiddenField ID="hdfExchangeRate" runat="server" />
            <asp:HiddenField ID="hdfJournalizeWorkFlow" runat="server" />
            <asp:HiddenField ID="hdfFromDate" runat="server" />
            <asp:HiddenField ID="hdfToDate" runat="server" />
            <asp:HiddenField ID="hdfFromDateEntry" runat="server" />
            <asp:HiddenField ID="hdfToDateEntry" runat="server" />
            <asp:HiddenField ID="hdfPurchaseFromDate" runat="server" />
            <asp:HiddenField ID="hdfPurchaseToDate" runat="server" />
            <asp:HiddenField ID="hdfDspType" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsCancelled" runat="server" Value="0" />
            <asp:Button ID="btnJournalizeUpdate" runat="server" OnClick="ActionHandler" CommandName="JOURNALIZEUPDATE"
                EnableTheming="false" Style="display: none" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
