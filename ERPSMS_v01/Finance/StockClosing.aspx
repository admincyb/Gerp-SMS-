<%@ Page Title="<%$ Resources:Captions,Title_ClosingStock %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="StockClosing.aspx.cs" Inherits="ERPSMS_v01.Finance.StockClosing"
    Theme="ClassicExt" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        label.font-bold-700 {
            min-width: 13% !important;
        }

        span#ctl00_MainContent_lblTotal {
            background: none;
            border: none;
        }
    </style>
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
            SetGridScroll();
            InitFinYear();
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
            //To set visibility of Hierarchical grid expand button
            ShowHideExpand();
            //End

        }
        function InitFinYear() {

            GrandScriptUtils.MakeAutoComplete("txtPackingCat", "MaterialCategory.do?Action=GetMaterialCategoryListAuto" + "&Type=3", "hdfPackingCat", true, false, "BizUnitPk", true);
        }

        function ShowHideExpand() {
            ///<summary>
            /// Used to Show/Hide Grid Expad Button
            ///</summary>

            $("[id*=hdfHasChildren]").each(function () {
                $(this).parent().parent().find('a.GridExpandCollapseButton').css("visibility", ($(this).val() == "1" ? "visible" : "hidden"));
            });
        }

        //function AfterGridExpand(row) {

        //    if ($("[id$=grdDeptList]").attr('id') == $(row).parent().parent().attr('id')) {
        //        var hdf = $(row).find("[id*=hdfIsExpandedDepartment]");
        //        if (hdf.val() == "0") {
        //            $(row).find("input[id*=btnOrderDetails]").click();
        //        }
        //    }
        //    else {
        //        var gridType = 0;
        //        var names = $(row).parent().parent().attr('id').split('_');

        //        if (names.length == 1) {
        //            gridType = names[0] == "grdItem" ? 1 : names[0] == "grdGINList" ? 2 : 0;
        //        }
        //        else if (names.length > 1) {
        //            gridType = (names[names.length - 1] == "grdGRNList" || names[names.length - 2] == "grdGRNList") ? 1
        //                : (names[names.length - 1] == "grdGINList" || names[names.length - 2] == "grdGINList") ? 2 : 0;
        //        }
        //        if (gridType == 1) {
        //            var hdf = $(row).find("[id*=hdfIsExpandedGRNList]");
        //            if (hdf.val() == "0") {
        //                $(row).find("input[id*=btnGetGIN]").click();
        //            }
        //        }
        //        if (gridType == 2) {
        //            var hdf = $(row).find("[id*=hdfIsExpandedGinList]");
        //            if (hdf.val() == "0") {
        //                $(row).find("input[id*=btnGetStockTransfer]").click();
        //            }
        //        }
        //    }

        //    if ($("[id$=grdItemCat]").attr('id') == $(row).parent().parent().attr('id')) {
        //        var hdf = $(row).find("[id*=hdfIsExpandedItem]");
        //        if (hdf.val() == "0") {
        //            $(row).find("input[id*=btnGetGrn]").click();
        //        }
        //    }
        //}

        function ItemListSelection() {
            var selectedRowColor;
            selectedRowColor = '<%= Resources.ErpRes.selectedRowColor %>';

            $("#[id*=grdDeptList] input[type=hidden][id*=hdfDeptPK]").each(function (index) {
                if ($(this).val() == $("[id$=hdfSelectedDeptPK]").val()) {
                    $(this).closest('tr').css('background-color', selectedRowColor);
                }
            });
        }

        function AfterGridExpandOld(row) {
            //if ($("[id$=grdDOHdr]").attr('id') == $(row).parent().parent().attr('id')) {
            //    $(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").val($(row)[0].rowIndex);
            //    var hdf = $(row).find("[id*=hdfIsExpandedDOItem]");
            //    if (hdf.val() == "0") {
            //        $(row).find("input[id*=btnGetDoDetails]").click();
            //    }
            //    else
            //        SetGridScroll($(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").attr("id"));
            //}

            if ($("[id$=grdDeptList]").attr('id') == $(row).parent().parent().attr('id')) {
                $(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").val($(row)[0].rowIndex);
                var hdf = $(row).find("[id*=hdfIsExpandedDepartment]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnOrderDetails]").click();
                }
                else
                    SetGridScroll($(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").attr("id"));
            }
            if ($("[id$=grdItemCat]").attr('id') == $(row).parent().parent().attr('id')) {
                $(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").val($(row)[0].rowIndex);
                var hdf = $(row).find("[id*=hdfIsExpandedItem]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnGetstockDetails]").click();
                }
                else
                    SetGridScroll($(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").attr("id"));
            }
        }

        function AfterGridExpand(row) {

            if ($("[id$=grdDeptList]").attr('id') == $(row).parent().parent().attr('id')) {
                $(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").val($(row)[0].rowIndex);
                var hdf = $(row).find("[id*=hdfIsExpandedDepartment]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnOrderDetails]").click();
                }
                else
                    SetGridScroll($(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").attr("id"));
            }

            //   
            else {
                var gridType = 0;
                var names = $(row).parent().parent().attr('id').split('_');

                if (names.length == 1) {
                    gridType = names[0] == "grdItemCat" ? 1 : names[0] == "grdItem" ? 2 : 0;
                } else if (names.length > 1) {
                    gridType = (names[names.length - 1] == "grdItemCat" || names[names.length - 2] == "grdItemCat") ? 1
                        : (names[names.length - 1] == "grdItem" || names[names.length - 2] == "grdItem") ? 2 : 0;
                }
                if (gridType == 1) {
                    var hdf = $(row).find("[id*=hdfIsExpandedItem]");
                    if (hdf.val() == "0") {
                        $(row).find("input[id*=btnGetstockDetails]").click();
                    }
                }
                else
                    SetGridScroll($(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").attr("id"));
                //if (gridType == 2) {
                //    var hdf = $(row).find("[id*=hdfIsExpandedGinList]");
                //    if (hdf.val() == "0") {
                //        $(row).find("input[id*=btnGetStockTransfer]").click();
                //    }
                //}
            }

        }

        function SetGridScroll(rowId) {
            if (rowId)
                rowArray = $("[id$=" + rowId + "]");
            else
                rowArray = $("[id$=_ExpandPosition]");
            rowArray.each(function () {
                if ($.trim($(this).val()) != "") {
                    var containerDiv = $(this).parent("[id$=_ScrollContainer]");
                    if (containerDiv != null) {
                        $(containerDiv).scrollTop(document.getElementById($(containerDiv).attr('id')).querySelectorAll('[id$=' + $(containerDiv).attr('grid') + ']')[0].children[0].children[$(this).val()].offsetTop);
                    }
                }
                $(this).val("")
            });
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
                //$("[id$=pnlDelete]").hide();
                $("[id$=pnlPost]").hide();
                $("[id$=pnlSubmit]").show();
                $("[id$=pnlSaveSubmit]").hide();
            }
            else if (mode == 3) {
                $("[id$=pnlSubmit]").show();
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
                                        TabIndex="1" onmouseover="javascript:ShowTooltip('ddlCompany');" Visible="false">
                                    </asp:DropDownList>
                                </div>
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <%--<li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="50"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>--%>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="15"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="ClosingStock" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="16" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="ClosingStock" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <%--<asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="53" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="ClosingStock" ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />--%>
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="17" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li runat="server" id="pnlPost">
                                        <asp:Button runat="server" ID="btnJournalize" CommandName="JOURNALIZE" TabIndex="55"
                                            Text="<%$resources:Journalize %>" OnClick="ActionHandler" ToolTip="<%$resources:Journalize %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-journalize" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="18" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>

                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <%--<li runat="server" id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="57" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelCLST %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelCLST %>" />
                                    </li>--%>
                                    <li>
                                        <asp:Button runat="server" TabIndex="7" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="8" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="9" Text="<%$resources:Controls,View %>"
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
                                            <%--<asp:Label ID="lblMnth" runat="server" Text="<%$resources:AsOnMonth %>" AssociatedControlID="txtMonth"></asp:Label>
                                            <asp:TextBox ID="txtMonth" runat="server" TabIndex="2" CssClass="input-small margnbotm0"
                                                MaxLength="17" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <cc1:CalendarExtender runat="server" ID="txtMonth_CalendarExtender" BehaviorID="calendar2"
                                                TargetControlID="txtMonth" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                                                ClientIDMode="Static" OnClientHidden="onCalendarHidden">
                                            </cc1:CalendarExtender>--%>
                                            <asp:Label runat="server" ID="lblFInYr" Text="<%$ resources:FinYr%>" AssociatedControlID="drpfinyear"></asp:Label>
                                            <asp:DropDownList ID="drpfinyear" runat="server" CssClass="select-small-a margnbotm0" TabIndex="2">
                                                <%-- <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,NotPosted %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Posted %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="-1"></asp:ListItem>--%>
                                            </asp:DropDownList>

                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <%--<asp:Label ID="lblStkNo" runat="server" Text="<%$resources:StockNo %>" AssociatedControlID="txtStockNo"></asp:Label>
                                            <asp:TextBox ID="txtStockNo" runat="server" CssClass="input-small margnbotm0" MaxLength="100"
                                                TabIndex="4"> </asp:TextBox>--%>
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
                                                    ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" TabIndex="6" />
                                                <asp:HiddenField runat="server" ID="hdf_Iohpk" Value='<%# Eval("IOH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfFinYearPk" Value='<%# Eval("IOH_FIN_YEAR") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TrnDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("IOH_DATE", Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval("IOH_DATE", Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:StockNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblfinyear" runat="server" Text='<%# (Eval("IOH_NO")).ToString()==string.Empty?"[NEW]": (Eval("IOH_NO")).ToString() %>'
                                                    ToolTip='<%# (Eval("IOH_NO")).ToString()==string.Empty?"[NEW]": (Eval("IOH_NO")).ToString() %>'></asp:Label>
                                            </ItemTemplate>

                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:FinYr %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblfinyeartext" runat="server" Text='<%# Eval("IOH_FIN_YEAR_TEXT")%>'></asp:Label>
                                            </ItemTemplate>

                                            <ItemStyle Width="19%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Status %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSCStatus" runat="server" Text='<%# Convert.ToString(Eval("STATUS")) == "1" ?"Closed" : "Not Closed" %>'
                                                    ToolTip='<%# Convert.ToString(Eval("STATUS")) == "1" ?"Closed" : "Not Closed" %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
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
                                            <asp:Label runat="server" ID="lblStockNo" Text="<%$ resources:StockNo%>" AssociatedControlID="lblClosingStockNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblClosingStockNo" CssClass="input-small"></asp:Label>
                                            <div class="clear">
                                            </div>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblFin_Yr" Text="<%$ resources:FinYr%>" AssociatedControlID="drpFinyr"></asp:Label>
                                            <asp:DropDownList ID="drpFinyr" runat="server" CssClass="select-small-a margnbotm0" TabIndex="11" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>
                                            <asp:Button runat="server" ID="btnLoad" CommandName="LOADFROMTEMPLATE"
                                                OnClick="ActionHandler" Text="<%$resources:Controls,LoadFromTemplate %>" CommandArgument="PageAction_Entry"
                                                SkinID="btnInner-journalize" ToolTip="<%$resources:Controls,LoadFromTemplate %>"
                                                ValidationGroup="Load" OnClientClick="ValidatePageNow('Load');" TabIndex="12" />
                                            <asp:Label runat="server" ID="lblDept" Text="<%$ resources:Department%>" AssociatedControlID="ddlDept" Visible="false"></asp:Label>
                                            <asp:DropDownList ID="ddlDept" runat="server" CssClass="lbl-27perc  margnbotm0" TabIndex="2" ValidationGroup="Load" Visible="false">
                                            </asp:DropDownList>
                                            <%-- <asp:RequiredFieldValidator ID="vrfDept" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Load" EnableClientScript="true" runat="server" ControlToValidate="ddlDept"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Department %>" InitialValue="-1">
                                            </asp:RequiredFieldValidator>--%>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">

                                            <%--<asp:DropDownList ID="drpFinyr" runat="server" CssClass="select-small-a margnbotm0" TabIndex="2">
                                            </asp:DropDownList>--%>
                                            <asp:Label runat="server" ID="lblStockDate" Text="<%$ resources:TrnDate%>" AssociatedControlID="txtStockDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtStockDate" CssClass="input-small" TabIndex="10"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfStockDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="ClosingStock" EnableClientScript="true" runat="server" ControlToValidate="txtStockDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_StockDate %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Button runat="server" ID="btnRefresh" CommandName="REFRESH"
                                                OnClick="ActionHandler" Text="<%$resources:Controls,Refresh %>" CommandArgument="PageAction_Entry"
                                                SkinID="btnInner-refresh" ToolTip="<%$resources:Controls,Refresh %>"
                                                class="btnInner-refresh btnInner-size" Visible="false" />
                                            <%--   </div>--%>
                                    </td>
                                </tr>
                            </table>
                            <%--  old grid--%>
                            <%--<div class="gridwrap">
                                <asp:GridView ID="grdItemDetails" runat="server" AutoGenerateColumns="False" Width="100%"
                                    AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                    ShowFooter="true">
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
                            </div>--%>
                            <div class="content-wrapper">
                                <%--use the width property of the below table corresponding to the contents in the page--%>
                                <%--<asp:Table runat="server" ID="Table2" CssClass="tablelayout asptbllinks">--%>
                                <%--Rename this ID Page_Entry with the corresponding section Id in the documet--%>
                                <%--<asp:TableRow ID="TableRow1" runat="server">--%>
                                <%--Align table cell according to design--%>
                                <%--<asp:TableCell>--%>
                                <div class="clear">
                                </div>
                                <%--  <div class="clear">
                            </div>--%>

                                <div class="gridwrap hierarchical-wrap maxh-290" id="divDpt_ScrollContainer" grid="grdDeptList">
                                    <asp:HiddenField ID="hdfDpt_ExpandPosition" runat="server" />
                                    <cc1:ExtGridView runat="server" ID="grdDeptList" AutoGenerateColumns="False" Width="100%"
                                        GridLines="None" EmptyDataRowStyle-CssClass="emptytable"
                                        PageSize="<%$ resources:PageSize %>" ShowFooter="true" OnRowDataBound="ActionHandler"
                                        ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                        ExpandButtonText="+" CollapseButtonText="-">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:RadioButton CssClass="rdoSelection" runat="server" Checked="false" TabIndex="13"
                                                        GroupName="SelectOne" ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping2(this);"
                                                        OnCheckedChanged="ActionHandler" AutoPostBack="false" />
                                                    <asp:Button runat="server" ID="btnOrderDetails" EnableTheming="false"
                                                        Style="display: none" OnClick="ActionHandler" CommandName="CATEGORYDETAIL"
                                                        CommandArgument='<%# Eval("PK") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfIsExpandedDepartment" Value="0" />
                                                    <asp:HiddenField runat="server" ID="hdfDeptPK" Value='<%# Eval("PK") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval("FYD_STATUS") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfIohStatus" Value='<%# Eval("IOH_FLAG") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="2%" />
                                                <FooterStyle Width="2%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Department %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldeptname" runat="server" Text='<%# Eval("VALUE") %>'
                                                        ToolTip='<%# Eval("VALUE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="30%" />
                                                <FooterStyle Width="1%" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:OpeningStock %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOpeningStock" runat="server" Text='<%# GetFormattedCurrency(Eval("FYD_QTY_STK")) %>'
                                                        ToolTip='<%# GetFormattedCurrency(Eval("FYD_QTY_STK")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="7%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="rate-numeric" />
                                                <FooterStyle Width="5%" />
                                                <FooterTemplate>
                                                    <%--<div style="text-align: left; font-weight: bold; padding-left: 94%; font-size: larger;">
                                                        <asp:Label ID="lblTotalHead" Text="<%$ Resources:Total %>" runat="server" Width="527%"/>
                                                    </div>--%>
                                                </FooterTemplate>
                                                <FooterStyle Width="5%" />
                                                <HeaderStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Controls,Action %>">
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btnRefreshSave" TabIndex="14" Text="<%$resources:Controls,RefreshSave %>"
                                                        ToolTip="<%$resources:Controls,RefreshSave %>" CommandArgument="SEC_ActionPanel"
                                                        SkinID="btnInner-Save" OnClick="ActionHandler" CommandName="REFRESHANDSAVE" />
                                                    <%--OnClientClick="javascript:ValidatePageNow('invoice')"--%>
                                                    <%--Visible='<%# Convert.ToString(Eval("IOH_FLAG ")) == "1" ?false : true %>'--%>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                                <FooterStyle Width="10%" />
                                                <FooterTemplate>
                                                    <%--<div style="text-align: left; font-weight: bold; font-size: larger;">
                                                        <asp:Label ID="lblTotal" runat="server" />
                                                    </div>--%>
                                                </FooterTemplate>
                                                <FooterStyle Width="10%" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Status %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Convert.ToString(Eval("FYD_STATUS ")) == "1" ?"Closed" : "New" %>'
                                                        ToolTip='<%# Convert.ToString(Eval("FYD_STATUS ")) == "1" ?"Closed" : "New" %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                                <FooterStyle Width="10%" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <div class="hierarchical-gridwrap" id="divCat_ScrollContainer" grid="grdItemCat">
                                                        <asp:HiddenField ID="hdfCat_ExpandPosition" runat="server" />
                                                        <cc1:ExtGridView runat="server" ID="grdItemCat" AutoGenerateColumns="False"
                                                            Width="100%" ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                                            GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                                            AllowPaging="false" OnRowDataBound="ActionHandler">
                                                            <EmptyDataTemplate>
                                                                <asp:Label ID="Label3" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Button runat="server" ID="btnGetstockDetails" OnClick="ActionHandler" CommandName="ITEMCATEGORYSELECTED"
                                                                            CommandArgument='<%# Eval("ItemCat_Pk") %>' EnableTheming="false"
                                                                            Style="display: none" />
                                                                        <asp:HiddenField runat="server" ID="hdfIsExpandedItem" Value="0" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="2%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:ItemCategory %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblItemCat" runat="server" Text='<%# Eval("ItemCat_Text").ToString() %>'
                                                                            ToolTip='<%# Eval("ItemCat_Text") %>'></asp:Label>

                                                                        <asp:HiddenField runat="server" ID="hdfItemCat" Value='<%# Eval("ItemCat_Pk") %>' />
                                                                        <asp:HiddenField runat="server" ID="hdfDepartmentPK" Value='<%# Eval("Dept_pk") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="97%" />
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <div class="hierarchical-gridwrap" id="divItem_ScrollContainer" grid="grdItem">
                                                                            <asp:HiddenField ID="hdfItem_ExpandPosition" runat="server" />
                                                                            <asp:GridView runat="server" ID="grdItem" AutoGenerateColumns="False"
                                                                                GridLines="None" CellPadding="3"
                                                                                ForeColor="#333333" AllowPaging="false">
                                                                                <EmptyDataTemplate>
                                                                                    <asp:Label ID="lblItemList" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                                                </EmptyDataTemplate>
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="<%$ resources:Item %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblItem" runat="server" Text='<%# Eval("Item_Text") %>' ToolTip='<%# Eval("Item_Text") %>'></asp:Label>
                                                                                            <asp:HiddenField runat="server" ID="hdfItemPk" Value='<%# Eval("Item_pk") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="14%" />
                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ resources:Rate %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblRate" runat="server" Text='<%# Eval("Item_rate") %>' ToolTip='<%# Eval("Item_rate") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="10%" />
                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ resources:OpeningStock %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtStock" runat="server" Enabled="false" Text='<%# Eval("opening_stock") %>'></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="10%" />
                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                    </asp:TemplateField>

                                                                                </Columns>
                                                                                <RowStyle CssClass="table-thirdlevel" />
                                                                                <HeaderStyle CssClass="table-thirdlevela" />
                                                                                <FooterStyle CssClass="table-thirdlevela-total" />
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="nopadding" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <RowStyle CssClass="table-secondlevel" />
                                                            <HeaderStyle CssClass="table-secondlevela" />
                                                            <FooterStyle CssClass="table-seconela-total" />
                                                        </cc1:ExtGridView>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="nopadding" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <%--First--%>
                                        <RowStyle CssClass="table-firstlevel" />
                                        <HeaderStyle CssClass="table-firstlevela" />
                                        <FooterStyle CssClass="table-firstlevela-total" />
                                    </cc1:ExtGridView>
                                    <%--  <uc1:PagerControl ID="PagerControl1" runat="server" Visible="false" />--%>
                                </div>
                                <div id="DivTotal" runat="server">
                                    <table class="table-devide">
                                        <tr>
                                            <%--<td>
                                            <div class="div2col-S padgtop7">
                                            </div>
                                        </td>--%>
                                            <td>
                                                <div class="div2col-S padgtop7 margin-left-102" style="">
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S padgtop7 margin-left-40" style="text-align: left; font-weight: bold; font-size: larger;">
                                                    <label for="txtTrxNo" class="font-bold-700" id="lblfooter" runat="server">
                                                        <asp:Literal ID="lblTot" runat="server" Text="Total:"></asp:Literal>
                                                    </label>
                                                    <asp:Label ID="lblTotal" runat="server" CssClass="input-disabled numeric">

                                                    </asp:Label>
                                                </div>
                                            </td>

                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <%--  Shortclose region Start--------------------%>
                            <div id="divShortClose" title="<%=Resources.Controls.ShortClose%>" style="display: none">
                                <div class="divcolmiddle-S">
                                    <label for="lblPONumber">
                                        <%=Resources.Controls.PoNumber%></label>
                                    <asp:Label ID="lblPOH_NO" class="lbl-22perc" runat="server"></asp:Label>
                                    <label for="Remarks">
                                        <%=Resources.Controls.Remark%>*</label>
                                    <asp:TextBox runat="server" ID="Remarks" TabIndex="18" MaxLength="200" TextMode="MultiLine" Height="40px">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqRemarks" CssClass="star" SetFocusOnError="true" EnableClientScript="true"
                                        ValidationGroup="ShortCloseSave" runat="server" ControlToValidate="Remarks" Display="Dynamic"
                                        Text="*" ErrorMessage="<%$ resources:EnterRemarks %>">
                                    </asp:RequiredFieldValidator>
                                    <label for="RefNo">
                                        <%=Resources.Controls.RefNo%></label>
                                    <asp:TextBox runat="server" ID="RefNo" TabIndex="18" MaxLength="14">
                                    </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <asp:Label ID="lbnSpace" runat="server" AssociatedControlID="btnAddConv"></asp:Label>
                                    <asp:HiddenField ID="POID" runat="server" Value="0"></asp:HiddenField>
                                    <asp:Button runat="server" ID="btnAddConv" Text="<%$Resources:Controls,ShortClose%>" CommandName="SHORTCLOSESAVE" OnClick="ActionHandler" ValidationGroup="ShortCloseSave"
                                        class="inputbtn" Width="100px" Height="20px" OnClientClick="ValidatePageNow('ShortCloseSave');" />
                                    <div class="clear">
                                    </div>
                                </div>
                            </div>
                            <%-- End Shortclose region -----------------%>
                            <div id="diverror" style="display: none">
                                <asp:Label runat="server" ID="Label4" ClientIDMode="Static" CssClass="star"></asp:Label>
                                <asp:ValidationSummary ID="vsShortCloseSave" ValidationGroup="ShortCloseSave" runat="server" />
                                <asp:ValidationSummary ID="vsLoad" ValidationGroup="Load" runat="server" />
                            </div>
                            <asp:HiddenField ID="hdnRoleID" runat="server" Value="0"></asp:HiddenField>
                            <asp:HiddenField ID="hdnShortCloseGroup" runat="server" Value="0"></asp:HiddenField>
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                            <asp:HiddenField ID="hdfAppSubType" runat="server" />
                            <asp:HiddenField ID="hdfCmntTrxNo" runat="server" Value=""></asp:HiddenField>
                            <asp:HiddenField ID="hdfCmntTrxPk" runat="server" Value=""></asp:HiddenField>
                            <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfDeptID" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfDecimalFormatWithSeperation" runat="server" />
                            <asp:HiddenField ID="hdnClosePO" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfSelectedDeptPK" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfClient" runat="server" Value="" />
                            <asp:HiddenField ID="hdfUcrTransCommentsPanelShow" runat="server" Value="1" />
                            <asp:HiddenField ID="hdfCheckNonStockPO" runat="server" Value="1" />
                            <asp:HiddenField ID="hdfPRtypeEnabled" runat="server" Value="0" />
                            <asp:HiddenField runat="server" ID="POH_MENU_TYPE" Value="1" />
                            <asp:HiddenField ID="hdfIsShowInvNo" runat="server" Value="" />
            </div>

            </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
            </asp:Table>
                <%-----------Closing stock history Popup Start----------------------------------------%>

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
