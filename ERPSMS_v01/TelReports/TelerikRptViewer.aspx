<%@ Page Title="<%$ Resources:Captions,Title_MISReports %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" ValidateRequest="false"
    AutoEventWireup="true" CodeBehind="TelerikRptViewer.aspx.cs" Inherits="ERPSMS_v01.TelReports.TelerikRptViewer" Theme="ClassicExt" %>



<%@ Register Assembly="Telerik.ReportViewer.Html5.WebForms, Version=18.1.24.709, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.Html5.WebForms" TagPrefix="telerik" %>
<%--EnableEventValidation="false"--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../TelDll/Scripts/jquery-3.3.1.min.js"></script>
    <script>
        $.noConflict();
// Code that uses other library's $ can follow here.
    </script>
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
            ShowHideTelerikDiv($("[id$=hdfShowTelDev]").val());

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
        //function ValidatePageNow(valGroup) {
        //    return true;

        //}

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

        function ShowHideTelerikDiv(flag) {
            //If flag then Show AdvancedSearch
            if (flag == 1) {
                $("[id$=divTelReport]").show();
            }
            else {
                $("[id$=divTelReport]").hide();
            }
            return false;
        }
        function ShowProgress() {
            $('#updateProgress').show();
        }


        (function (trv, $) {
            "use strict";
            var sr = {
                //warning and error string resources
                loadingReport: 'Loading report....',
                loadingReportPagesInProgress: 'Report Loading, {0} pages loaded so far...',
                loadedReportPagesComplete: 'Done. Total {0} pages loaded.',
                noPageToDisplay: "No page to display.",
            };
            trv.sr = $.extend(trv.sr, sr);
        }(window.telerikReportViewer = window.telerikReportViewer || {}, jQuery));

    </script>

    <style>
        #reportViewer1 {
            position: absolute;
            left: 5px;
            right: 5px;
            top: 5px;
            bottom: 5px;
            overflow: hidden;
            font-family: Verdana, Arial;
            width: 100%;
        }

        .content-wrapper {
            padding: 0px 10px 10px 10px !important;
        }

        .k-menu .k-item, .k-menu.k-header {
            border-color: #a3d0e4;
            height: 28px !important;
        }

            .k-menu .k-item > .k-link, .k-menu-scroll-wrapper .k-item > .k-link, .k-popups-wrapper .k-item > .k-link {
                padding: 5px 1.1em .4em !important;
                line-height: 15px !important;
            }

        .trv-nav input.k-textbox {
            margin: 2px 0 0 0 !important;
        }

        .trv-page-wrapper.active {
            background-image: none;
            background-color: rgb(255, 255, 255);
            height: 100% !important;
            width: 100% !important;
            position: relative;
        }

        .trv-pages-area.printpreview .trv-page-container .trv-page-wrapper .trv-report-page {
            border-right: 0;
        }

        .trv-page-overlay {
            border-color: lightgray !important;
            background-color: lightgray !important;
            color: #000;
        }

        .trv-pages-area.printpreview .trv-page-container .trv-page-wrapper.active .trv-report-page:not(.k-state-default) {
            border-color: #000 !important;
            border-right-color: rgb(0, 0, 0) !important;
            border-right: 0 !important;
        }

        .s1-1b805b50454aa863dcff53 {
            border-bottom: 1px solid #4F81BD !important;
        }

        /*.k-widget.k-tooltip-validation {
         border-color: red;
         background-color: red;
         color: #000;
     }*/
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%-- <asp:UpdatePanel ID="aupdpnlAvtivity" runat="server">
        <ContentTemplate>--%>
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
                                    ToolTip="View Report" OnClientClick="javascript:ValidatePageNow('fltr')" CommandArgument="SEC_ActionPanel" />

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
                    <div id="divTelReport">
                        <telerik:ReportViewer
                            ID="ReportViewer1"
                            Width="100%"
                            Height="900px"
                            EnableAccessibility="false"
                            PageMode="SinglePage"
                            Scale="1"
                            ScaleMode="Specific" ViewStateMode="Enabled"
                            PageNumber="1" EnableViewState="true" ViewMode="PrintPreview"
                            runat="server">
                            <ReportSource IdentifierType="UriReportSource">
                            </ReportSource>
                        </telerik:ReportViewer>
                    </div>
                    <div id="divReportViewer" runat="server">


                        <%--<telerik:ReportViewer
                                    ID="ReportViewer1"
                                    Width="99%"
                                    Height="900px"
                                    EnableAccessibility="false"
                                    PageMode="SinglePage"
                                    runat="server">
                                    <ReportSource IdentifierType="UriReportSource">
                                    </ReportSource>
                                </telerik:ReportViewer>--%>
                    </div>

                    <%-- <telerik:ReportViewer ID="ReportViewer1" runat="server"></telerik:ReportViewer>--%>
                    <div id="divNodata" class="nodata" runat="server" visible="false">
                        No Record Found
                    </div>
                    <asp:HiddenField ID="hdfShowFilter" runat="server" Value="1" />
                    <asp:HiddenField ID="hdfShowTelDev" runat="server" Value="0" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <div id="divHiddenFields">
            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" Value="#0." />

        </div>
        <div id="diverror" style="display: none">
            <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><div
                id="divValidationSummary" runat="server">
                <%--The ValidationSummary controls will bind here--%>
            </div>
        </div>
    </div>

    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
