<%@ Page Title="<%$ Resources:Captions,Title_MISReports %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    EnableEventValidation="false" Theme="ClassicExt" AutoEventWireup="true" EnableViewState="true"
    CodeBehind="CommonReportViewer.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="ERPSMS_v01.Reports.CommonReportViewer" %>


<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/CheckListSearchControl.ascx" TagName="CheckListSearchControl"
    TagPrefix="uc1" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%--<%@ Register Assembly="AjaxControlToolkit, Version=3.0.11119.25533, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>--%>
<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms"
    TagPrefix="rsweb" %>--%>
<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

 <%@ Register Src="~/Reports/UserControls/TestFilter.ascx" TagName="TestFilter"  TagPrefix="usr1" %> 
<%@ Register Src="~/Reports/UserControls/InProcessReport.ascx"  TagName="InProcess" TagPrefix="usr21" %>
<%@ Register Src="~/Reports/UserControls/OnlineParameterControlSheet.ascx"  TagName="OnlineParameterControl" TagPrefix="usr22" %>
<%@ Register  Src="~/Reports/UserControls/usrStockCardReportFilter.ascx" TagName="tagStockCardReportFilter" TagPrefix="usr10" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        //To bind subreport after select any report
        function BindSubReport() {
            if ($("[id$=hdfReportGroup]").val() != "" && $("[id$=hdfReportGroup]").val() != "0") {
                // $("[id$=hdfReport]").val("0");
                //$("[id$=txtReport]").val("<%= Resources.Messages.AutoDefaultValue %>");
                if (url.indexOf("?") != -1) {
                    GrandScriptUtils.MakeAutoCompleteDDL("txtReport", url + "&Type=" + $("[id$=hdfReportGroup]").val(), "hdfReport", true, true, "REPORT");
                }
                else {
                    GrandScriptUtils.MakeAutoCompleteDDL("txtReport", url + "?Type=" + $("[id$=hdfReportGroup]").val(), "hdfReport", true, true, "REPORT");
                }
            }
            else {
                GrandScriptUtils.MakeAutoCompleteDDL("txtReport", url, "hdfReport", true, true, "REPORT");
            }
        }
        $(document).ready(function () {
            //To Collapse Crystal report group tree at load
            $("[id$=panelHeader_close]").click();
            ShowHideAdvancedSearch($("[id$=hdfShowFilter]").val());

        });

        function ClickSubreportRefesh() {
            $("[id$=hdfShowCrReportDiv]").val(1);
            $("[id$=btnDummy]").click();
        }
        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtReportGroup") {
                if ($("[id$=hdfReportGroup]").val() != "" && $("[id$=hdfReportGroup]").val() != "0") {
                    $("[id$=hdfReport]").val("0");
                    $("[id$=txtReport]").val("<%= Resources.Messages.AutoDefaultValue %>");
                    if (url.indexOf("?") != -1) {
                        GrandScriptUtils.MakeAutoCompleteDDL("txtReport", url + "&Type=" + $("[id$=hdfReportGroup]").val(), "hdfReport", true, true, "REPORT");
                    }
                    else {
                        GrandScriptUtils.MakeAutoCompleteDDL("txtReport", url + "?Type=" + $("[id$=hdfReportGroup]").val(), "hdfReport", true, true, "REPORT");
                    }
                }
            }
            else if (targetControlID == "txtReport") {
                $("[id$=btnReport]").click();
            }
            else {
                $('[id*=hdfAction]').each(function () {
                    var ctrlID = 'txt' + $(this).val();
                    if (targetControlID == ctrlID) {
                        var btnID = '[id$=btn' + $(this).val() + ']';
                        $(btnID).click();
                    }
                });
            }
        }

        function BuildAutoCompleteUrl(parms) {
            if (url.indexOf("?") != -1) {
                return url + '&' + parms;
            } else {
                return url + '?' + parms;
            }
        }

        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtReportGroup") {
                $("[id$=hdfReport]").val("0");
                $("[id$=txtReport]").val("<%= Resources.Messages.AutoDefaultValue %>");
                if (url.indexOf("?") != -1) {
                    GrandScriptUtils.MakeAutoCompleteDDL("txtReport", url + "&Type=" + $("[id$=hdfReportGroup]").val(), "hdfReport", true, true, "REPORT");
                }
                else {
                    GrandScriptUtils.MakeAutoCompleteDDL("txtReport", url + "?Type=" + $("[id$=hdfReportGroup]").val(), "hdfReport", true, true, "REPORT");
                }
            }
            else if (targetControlID == "txtReport") {
                $("[id$=hdfReport]").val("0");
                $("[id$=txtReport]").val("<%= Resources.Messages.AutoDefaultValue %>");
                $("[id$=btnReport]").click();
            }

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

        function ShowListing(flag) {
            ///<summary>
            /// Used to handle the Listing And Enrty Section in Page
            ///</summary>
            /// <param name="flag" optional="true" type="String">
            /// flag Determines the Mode if flag then in Listing else in Edit Mode
            /// </param>           
            if (flag) {
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlEntry]").hide();

            }
            else {
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlEntry]").show();
            }
            ShowHideCrystalReportDiv();

            return false;
        }
        function ShowPrintPopup(rptType) {
            if (rptType == 1) {
                $("[id$=hdfShowCrReportDiv]").val("-1");
                ShowContainerDiv('[id$=divCrystalReportViewer]', 'Print', '1200', '620');
            }
            else if (rptType == 2) {
                $("[id$=hdfShowPopupReport]").val("1");
                ShowContainerDiv('[id$=divReportViewer]', 'Print', '1200', '620');
            }
            return false;
        }

        function AfterClose(containerID) {
            if (containerID == "[id$=divReportViewer]") {
                $("[id$=hdfShowPopupReport]").val("0");
            }
        }

        function ClosePrintPopup() {
            $("[id$=btnClosePrintPopup]").click();
        }

        function InitPopupComponents() {
            if ($("[id$=hdfShowPopupReport]").val() == "1") {
                ShowPrintPopup(2);
            }
        }

        function ShowHideCrystalReportDiv() {
            if ($("[id$=hdfShowCrReportDiv]").val() == "-1") {
                return false;
            }
            if ($("[id$=hdfShowCrReportDiv]").val() == "0") {
                $("[id$=divCrystalReportViewer]").hide();
                $("[id$=crReportViewer]").hide();
            }
            else {
                $("[id$=divCrystalReportViewer]").show();
                $("[id$=crReportViewer]").show();
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
                $("[id$=pnlSave]").hide();
                $("[id$=pnlSubmit]").hide();

            }
            else if (mode == 2) {

            }
            else if (mode == 3) {
                $("[id$=pnlSubmit]").hide();
            }
        }

        function ValidateText(event, ctlName) {
            var explen = 2;
            var decLen = 2;
            var cntNbr = document.getElementById(ctlName.id).value;
            var isDot = 0;
            if (57 < event.keyCode || event.keyCode < 48)
                event.returnValue = false;
            else {
                for (var i = 0; i <= (cntNbr.length - 1); i++) {
                    if (cntNbr.charAt(i) == ':')
                        isDot = 1;
                }

                if (isDot == 0) {
                    var beforeDec = cntNbr;
                    if (beforeDec.length >= explen) {
                        document.getElementById(ctlName.id).value = cntNbr.substring(0, cntNbr.length - 1);
                    }
                    event.returnValue = true;
                }
                else {
                    var afterDec = (cntNbr.split(':', 2)).pop();
                    afterDec = afterDec.replace('_', '');
                    if (afterDec.length >= decLen) {
                        document.getElementById(ctlName.id).value = cntNbr.substring(0, cntNbr.length - 1);
                        event.returnValue = true;
                    }
                }
            }
            if (event.keyCode == 58) {
                for (var i = 0; i <= (cntNbr.length - 1); i++) {
                    if (cntNbr.charAt(i) == ':')
                        isDot = 1;
                }
                if (isDot == 0)
                    event.returnValue = true;
            }

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

        function ShowDeleteConfirm(btn, message) {
            var msgTitle;
            var msg;
            msgTitle = "<%= Resources.ErpRes.Title_Information %>";
            msg = message ? message : "<%= Resources.ErpRes.MsgDeleteConfirm %>";
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $(this).dialog("close");
                        __doPostBack(btn.name, '');
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        if (typeof AfterDeleteConfirmationCancel == "function") {
                            AfterDeleteConfirmationCancel(btn.id);
                        }
                        return false;
                    }
                }
            });
            return false;
        }

        $(document).ready(function () {
            var elementName = "#" + $('input[id$=hdfSelectedNodeID]').val();
            var elem = $(elementName)//$('#CUSTOMER_PKt166');
            var pnlContainer = $('[id$=pnlControls]');
            if (elem != undefined && elem[0] != undefined) {
                elem[0].scrollIntoView(true);
                if (pnlContainer != undefined && pnlContainer[0] != undefined) {
                    pnlContainer[0].scrollLeft = 0;
                }
                $('input[id$=hdfSelectedNodeID]').val('');
            }
        });
        function OpenPDF(url) {
            var win = window.open(url, '_blank', 'location=no,menubar=0,scrollbars=1,titlebar=0,status=0,width=1200,height=800,left=100,top=0');
            return false;
        }

        function postBackByTreeviewCheckBox(e) {
            var evnt = ((window.event) ? (event) : (e));
            var element = evnt.srcElement || evnt.target;
            //alert(element.tagName + "-----" + element.type);

            if (element.tagName == "INPUT" && element.type == "checkbox") {
                $('input[id$=hdfSelectedNodeID]').val(element.id);
                __doPostBack("", "");
            }
        }
    </script>
    <script type="text/javascript">
        var ControlID = '';
        $(window).load(function EndRequest() {
            FormatCalendar('4');
        });
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
            //            if (sender.get_selectedDate()) {
            //                if (sender.get_selectedDate() && sender.get_selectedDate() && cal1.get_selectedDate() > cal2.get_selectedDate()) {
            //                    alert('The "From" Date should smaller than the "To" Date, please reselect!');
            //                    sender.show();
            //                    return;
            //                }
            //                //get the final date
            //                var finalDate = new Date(sender.get_selectedDate());
            //                var selectedMonth = finalDate.getMonth();
            //                finalDate.setDate(1);
            //                if (sender == cal2) {
            //                    // set the calender2's default date as the last day
            //                    finalDate.setMonth(selectedMonth + 1);
            //                    finalDate = new Date(finalDate - 1);
            //                }
            //                //set the date to the TextBox
            //                sender.get_element().value = finalDate.format(sender._format);
            //            }
        }

        //Validate month Range
        function CheckMonthRange(src, args) {
            var Month2 = $('input:text[id$=' + src.controltovalidate + ']').val();
            var month1Id = $('input:hidden[id$=hdfRange' + src.controltovalidate + ']').val();
            var Month1 = $('input:text[id$=' + month1Id + ']').val();
            var Month1Split = Month1.split('-');
            var Month2Split = Month2.split('-');
            if (parseInt(Month2Split[1]) > parseInt(Month1Split[1])) {
                args.IsValid = true;
            } else if (parseInt(Month2Split[1]) < parseInt(Month1Split[1])) {
                args.IsValid = false;
            } else {
                if (MonthGreater(Month1Split[0], Month2Split[0]) == true) {
                    args.IsValid = true;
                } else {
                    args.IsValid = false;
                }
            }
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

        function OnCheckBoxCheckChanged(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;

                if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node 
                {
                    if (nxtSibling.tagName.toLowerCase() == "div") //if node has children           
                    {
                        //check or uncheck children at all levels           
                        CheckUncheckChildren(parentTable.nextSibling, src.checked);
                    }
                }
                //check or uncheck parents at all levels           
                CheckUncheckParents(src, src.checked);
            }
        }
        function CheckUncheckChildren(childContainer, check) {
            var childChkBoxes = childContainer.getElementsByTagName("input");
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                childChkBoxes[i].checked = check;
            }
        }
        function CheckUncheckParents(srcChild, check) {
            var parentDiv = GetParentByTagName("div", srcChild);
            var parentNodeTable = parentDiv.previousSibling;



            if (parentNodeTable) {
                var checkUncheckSwitch;

                if (check) //checkbox checked
                {
                    var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                    if (isAllSiblingsChecked)
                        checkUncheckSwitch = true;
                    else
                        return; //do not need to check parent if any(one or more) child not checked
                }
                else //checkbox unchecked
                {
                    checkUncheckSwitch = false;
                }

                var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                if (inpElemsInParentTable.length > 0) {
                    var parentNodeChkBox = inpElemsInParentTable[0];
                    parentNodeChkBox.checked = checkUncheckSwitch;
                    //do the same recursively
                    CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                }
            }
        }
        function AreAllSiblingsChecked(chkBox) {
            var parentDiv = GetParentByTagName("div", chkBox);
            var childCount = parentDiv.childNodes.length;
            for (var i = 0; i < childCount; i++) {
                if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
                {
                    if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        //if any of sibling nodes are not checked, return false
                        if (!prevChkBox.checked) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        //utility function to get the container of an element by tagname
        function GetParentByTagName(parentTagName, childElementObj) {
            var parent = childElementObj.parentNode;
            while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                parent = parent.parentNode;
            }
            return parent;
        }

        function ShowHideAdvancedSearch(flag) {
            //If flag then Show AdvancedSearch
            if (flag == 1) {
                $("[id$=pnlControls]").show();
                $("[id$=imbShowFilter]").hide();
                $("[id$=imbHideFilter]").show();
            }
            else {
                $("[id$=pnlControls]").hide();
                $("[id$=imbShowFilter]").show();
                $("[id$=imbHideFilter]").hide();
            }
            return false;
        }
        function ShowProgress() {
            $('#updateProgress').show();
        }
    </script>
    <style type="text/css">
        /*.fields-group table        {  width: 100% !important;        }*/

        .ui-datepicker table {
            width: 100% !important;
        }

        .dlgBody img {
            margin: 0px !important;
        }

        .trElt img {
            margin-top: 3px !important;
        }

        .reportviewer span {
            display: inline;
        }

        .dialogbox span {
            margin: 3px 7px 0px 0px !important;
        }

        .clear {
            height: 0px !important;
        }

        #ctl00_MainContent_rvPrintReport .ToolBarBackground {
            background-color: #FFFFFF;
            background-color: #DBD8C9 !important;
            background-image: url(../../images/RedExt/Icons/reserved-strip-bg.jpg);
            background-repeat: repeat-x;
            min-height: 13px !important;
            height: 32px;
            margin-top: -7px;
            width: 101% !important;
            margin-left: -7px;
            padding-left: 7px;
        }
        ui-new-title-bar {
            height: 18px !important;
            line-height: 16px;
            padding: 0 0 0 8px;
            position: fixed;
            width: 1198px;
            left: 70px !important;
            right: 0;
            top: 19px;
            z-index: 9999;
            background: #dbd8c9;
            border-radius: 0;
            border-bottom: 0;
        }

        #ctl00_MainContent_rvPrintReport span.glyphui {
            color: #000000;
            font-size: 13px !important;
        }

        .MSRS-RVC .WidgetSet {
            height: 30px !important;
            text-align: center;
        }

        .MSRS-RVC .DisabledButton {
            height: 29px !important;
            cursor: not-allowed;
        }

        .MSRS-RVC .HoverButton {
            height: 28px !important;
        }
    </style>
    <style type="text/css">
        /* Date picker */

        .ui-datepicker {
            width: 18em;
            padding: .2em .2em 0;
            display: none;
            z-index: 999 !important;
        }

        /* scroll fixed table heaf */

        #data_container {
            border: 1px solid #000;
            width: 100%;
            height: 470px; /*649px;*/
            overflow-x: scroll;
        }

        #data_wrapper {
            width: 100%;
            border-right: 0px;
        }

        #data_headers, #data_body, #data_footer {
            width: 100%;
            padding: 0px;
            border-spacing: 0px;
            table-layout: fixed;
            position: sticky;
            top: 0;
            left: 0;
        }

        #data-footer-wraper {
            width: 100%;
            border-right: 0px;
        }

        #data_headers {
            z-index: 999;
        }

        #data_footer {
            z-index: 999;
            bottom: 0px; /*top: 606px;*/
        }

        #data_body {
        }

            #data_headers th, #data_body td, #data_footer th {
                width: 142px;
                text-align: left;
                padding-left: 10px;
            }

        #data_headers th {
            color: #303030;
            background: #803939;
            border-bottom: 1px solid#D0D7E9;
            border-left: none;
            border-right: none;
            font-size: 11px;
            padding: 3px 5px;
            vertical-align: middle;
            color: #fff; /*#005159;*/
            font-family: Verdana;
        }

        #data_body td {
            line-height: 12px;
            font-size: 11px;
            border-bottom: 1px solid #fff !important;
            border-left: none;
            border-right: none;
            color: #303030;
            padding: 4px 5px;
            padding-left: 5px;
            vertical-align: inherit;
        }

        #data_footer th {
            color: #303030;
            background: #803939;
            border-bottom: 1px solid#D0D7E9;
            border-top: 1px solid#D0D7E9;
            border-left: none;
            border-right: none;
            font-size: 11px;
            padding: 3px 5px;
            vertical-align: middle;
            color: #fff; /*#005159;*/
            font-family: Verdana;
        }

        /* search collapse */

        .search-colapse {
            line-height: 7px;
            height: 20px !important;
        }

            .search-colapse input {
                margin-top: 3px !important;
            }

        /* page grid */

        .fixed-buttons-normal {
            padding-bottom: 21px !important;
        }

        .lastpro-label21-10-2020 {
            min-width: 35% !important;
        }

        .btnInner-size {
            font-size: 10px !important;
            padding: 2px 3px 2px 25px;
            line-height: 1.6em !important;
            border: 0;
            border: solid 1px #c04244;
            cursor: pointer;
            margin-bottom: 0 !important;
        }

        .input-small {
            min-width: 18.3%;
            max-width: 18.3%;
            margin-bottom: 0 !important;
        }

        #content-container {
            min-height: 86vh !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--   <asp:UpdatePanel runat="server" ID="aupdpnlCommonReportViewer">
        <ContentTemplate>--%>
    <%--<asp:Literal runat="server" ID="ltrScriptContent" Text="<script type='text/javascript'>function InitComponents(flag){}</script>"></asp:Literal>--%>
    <div class="fixed-buttons-normal" id="divFixedTab">
        <div class="Button-container">
            <asp:Table ID="Table1" runat="server">
                <%--CssClass="minw-100per"--%>
                <asp:TableRow>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul class="bredcrum">
                            <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                        </ul>
                        <ul runat="server" id="pnlListing">
                            <li>
                                <asp:Button ID="btnProcess" runat="server" Visible="false" ClientIDMode="Static"
                                    OnClick="ActionHandler" Text="Process & View" SkinID="btnInner-schedule" CommandName="PROCESS"
                                    ValidationGroup="report" ToolTip="Process Report" OnClientClick="javascript:ValidatePageNow('report')"
                                    CommandArgument="SEC_ActionPanel" />
                            </li>
                            <li>
                                <asp:Button ID="btnSearch" runat="server" ClientIDMode="Static" OnClick="ActionHandler"
                                    Text="View" SkinID="btnInner-View" CommandName="VIEW" ValidationGroup="report"
                                    ToolTip="View Report" CommandArgument="SEC_ActionPanel" />
                                <%-- OnClientClick="javascript:ValidatePageNow('fltr')"   --%>
                            </li>
                            <li>
                                <asp:Button ID="BtnExcelExport" runat="server" ClientIDMode="Static" OnClick="ActionHandler"
                                    Text="Excel Export" SkinID="btnInner-View" CommandName="EXCEL" ValidationGroup="report"
                                    ToolTip="Export to Excel" OnClientClick="javascript:ValidatePageNow('fltr')"  CommandArgument="SEC_ActionPanel"  />

                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <asp:HiddenField ID="hdfCustomerPK" runat="server" />
        <asp:HiddenField ID="hdfSubTabValue" runat="server" />
        <asp:HiddenField ID="hdfSubtab" runat="server" />
        <asp:HiddenField ID="hdfSelectedNodeID" runat="server" Value="dummy" />
        <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout ">
            <%--minw-100per--%>
            <asp:TableRow ID="PageAction_Entry" runat="server">
                <asp:TableCell>
                    <div class="search-wrap-custom2">
                        <table class="table-devide ">
                            <%--minw-100per--%>
                            <tr>
                                <td>
                                    <div class="div2col-M0">
                                        <asp:Label runat="server" ID="lblReportGroup" CssClass="margn-rgt1" Text="Report Group"
                                            AssociatedControlID="txtReportGroup"></asp:Label>
                                        <asp:TextBox ID="txtReportGroup" runat="server" CssClass="large" MaxLength="100" />
                                        <asp:HiddenField ID="hdfReportGroup" runat="server" />
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-M0">
                                        <asp:Label runat="server" ID="Label1" Text="Report" AssociatedControlID="txtReport"></asp:Label>
                                        <asp:TextBox ID="txtReport" runat="server" MaxLength="100" CssClass="select-halfsmall" />
                                        <asp:HiddenField ID="hdfReport" runat="server" />
                                        <asp:Button ID="btnReport" runat="server" OnClick="ActionHandler" CommandName="REPORTCRITERIA"
                                            EnableTheming="false" Style="display: none" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <%-- <uc1:CheckListSearchControl ID="CheckListSearchControl1" runat="server" />--%>
                    <div class="search-colapse-c margnbotm0" id="tblSearch" runat="server" visible="false">
                        <table>
                            <tr>
                                <td>
                                    <h1>
                                        <%= GetGlobalResourceObject("Controls", "FilterBy").ToString()%></h1>
                                </td>
                                <td>
                                    <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                        ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                        TabIndex="65" />
                                    <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(0);"
                                        ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                        TabIndex="66" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Panel ID="pnlControls" runat="server" CssClass="margntop-minus3">
                        <%--The UI controls will bind here--%>

 
                    </asp:Panel>

 

                    <div id="divReportViewer" class="reportviewer treescroll-x" runat="server">
                        <%--   <rsweb:ReportViewer ID="rvViewReport" runat="server" KeepSessionAlive="false" AsyncRendering="false"
                            BorderWidth="0" SizeToReportContent="true" OnDrillthrough="ActionHandler" Width="100%">
                        </rsweb:ReportViewer>--%>

                        <rsweb:ReportViewer ID="rvViewReport" runat="server" KeepSessionAlive="false" AsyncRendering="false"
                            BorderWidth="0" SizeToReportContent="true" OnDrillthrough="ActionHandler" Width="100%">
                        </rsweb:ReportViewer>

                    </div>

                    <div id="divPrintReport" class="reportviewer treescroll-x popup-new-strip" runat="server" visible="false">

                        <div class="ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix ui-new-title-bar">

                            <span class="ui-dialog-title" id="ui-dialog-title-divMenu" style="float: left"></span>
                            <%--<a href="#" class="ui-dialog-titlebar-close ui-corner-all" role="button">
                                 <span class="ui-icon ui-icon-closethick">close</span></a>--%>

                            <a href="#" cssclass="pull-right" onclick="javascript:ClosePrintPopup()">
                                <asp:Image ID="imgClose" runat="server" CssClass="newcloss-icon" ImageUrl="../Images/RedExt/Icons/close-one-icon.png" />
                            </a>
                        </div>


                        <rsweb:ReportViewer ID="rvPrintReport" runat="server" KeepSessionAlive="false" AsyncRendering="false"
                            BorderWidth="0" SizeToReportContent="true" OnDrillthrough="ActionHandler" Width="100%">
                        </rsweb:ReportViewer>
                    </div>

                    <div style="display: none">
                        <asp:Button ID="btnClosePrintPopup" runat="server"
                            OnClick="ActionHandler" CommandName="CLOSEPOPUP" AlternateText="close" />
                    </div>


                    <div id="divNodata" class="nodata" runat="server" visible="false">
                        No Record Found
                    </div>
                    <asp:HiddenField ID="hdfShowCrReportDiv" runat="server" Value="0" />
                    <asp:HiddenField ID="hdfShowFilter" runat="server" Value="1" />
                </asp:TableCell></asp:TableRow></asp:Table><div id="divCrystalReportViewer" runat="server">
            <CR:CrystalReportViewer ID="GERP_MIS_Report" HasToggleParameterPanelButton="false"
                HasToggleGroupTreeButton="false"
                runat="server" AutoDataBind="true" HyperlinkTarget="_blank" />
        </div>
        <div id="divHTMLReport" runat="server" visible="false">
            <div class="gridwraps">
                <div class="lastprocess-label-main">
                    <asp:Label runat="server" ID="lblLastProcess" CssClass="lastprocess-label"></asp:Label><asp:Label runat="server" ID="lblLastProcessVal" CssClass="" Text=""></asp:Label><asp:Label runat="server" ID="lblBasedOn" CssClass="lastprocess-label-2" Text=""></asp:Label><asp:Label runat="server" ID="lblProcessDate" Text=""></asp:Label><asp:LinkButton runat="server" ID="lnkSummary" OnClick="ActionHandler" Visible="false" OnClientClick="ShowProgress()" CommandName="PRINTSUMMARY" Text="Print Summary"
                        CssClass="fields-right-26-11 btnInner-Print btnInner-size"></asp:LinkButton><asp:LinkButton runat="server" ID="lnkPrint" OnClick="ActionHandler" OnClientClick="ShowProgress()" CommandName="PRINT" Text="Print"
                        CssClass="fields-right-26-11 btnInner-Print btnInner-size"></asp:LinkButton></div><!--lastprocess-label-main--><div id="data_container" runat="server" clientidmode="Static">
                </div>
                <asp:HiddenField ID="hdfCurrencyFormat" runat="server" Value="#0." />

            </div>

        </div>
        <asp:HiddenField ID="hdfShowPopupReport" runat="server" Value="0" />
        <div style="display: none">
            <asp:Button ID="btnDummy" runat="server" ClientIDMode="Static" OnClick="ActionHandler"
                Text="Dummy" SkinID="btnInner-View" CommandName="REFRESH" CommandArgument="SEC_ActionPanel" />
        </div>
        <div id="diverror" style="display: none">
            <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><div
                id="divValidationSummary" runat="server">
                <%--The ValidationSummary controls will bind here--%>
            </div>
            <asp:ValidationSummary ID="vsFilterPage" ValidationGroup="fltr" runat="server" />
        </div>
    </div>
    <%--</ContentTemplate>    </asp:UpdatePanel>--%>
</asp:Content>
