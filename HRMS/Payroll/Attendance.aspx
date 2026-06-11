<%@ Page Title="<%$ Resources:Captions,Title_Attendance %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="Attendance.aspx.cs" Inherits="HRMS.Payroll.Attendance"
    Theme="ClassicExt" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            $(document).ready(function () {
                GrandScriptUtils.DatePickerCommon("txtDate");
                GrandScriptUtils.DatePickerCommon("txtImportDate");
                GrandScriptUtils.AddDateRangeCommon("txtFilterFromDate", "hdfFilterFromDate", "txtFilterToDate", "hdfFilterToDate", false, false);
                GrandScriptUtils.MakeAutoCompleteDDL("txtDesignation", url, "hdfDesignation", true, true, "DESIGNATION");
                GrandScriptUtils.MakeAutoCompleteDDL("txtHdrBranchLocation", url, "hdfHdrBranchLocation", true, true, "BRANCHLOCATION");
                GrandScriptUtils.MakeAutoCompleteDDL("txtFilterBranch", url, "hdfFilterBranch", true, true, "BRANCHLOCATION");
                GrandScriptUtils.MakeAutoCompleteDDL("txtHdrDepartment", url, "hdfHdrDepartment", true, true, "DEPARTMENT");
                //GrandScriptUtils.MakeAutoCompleteDDL("txtFilterDept", url, "hdfFilterDept", true, true, "DEPARTMENT");
                GrandScriptUtils.MakeAutoCompleteDDL("txtImportBranchLocation", url, "hdfImportBranchLocation", true, true, "BRANCHLOCATION");
                BindEmployee();
                if ($("[id$=txtDate]").attr("disabled") == true) {
                    $("[id$=txtDate]").addClass("input-disabled");
                }
                else {
                    $("[id$=txtDate]").removeClass("input-disabled");
                }


                if ($("[id$=txtHdrBranchLocation]").attr("disabled") == true) {
                    DisableAuto($("[id$=txtHdrBranchLocation]"), $("[id$=hdfBranchLocation]"));
                }
                else {
                    EnableAuto($("[id$=txtHdrBranchLocation]"), $("[id$=hdfBranchLocation]"));
                }

                $("[id$=txtInTime]").timepicker({
                    onSelect: function (dateText, inst) {
                        afterTimeSelect('txtInTime', inst.id);
                    }
                });
                $("[id$=txtOutTime]").timepicker({
                    onSelect: function (dateText, inst) {
                        afterTimeSelect('txtOutTime', inst.id);
                    }
                });

            });

            BindEmployee();

        }

        var msgTitle = '<%= Resources.ErpRes.Information %>';
        var msgContent = "";

        function BindEmployee() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee ", url + "?EmpCategory=2" + "&EmpDept=" + $("[id$=hdfHdrDepartment]").val() + "&EmpBranch=" + $("[id$=hdfHdrBranchLocation]").val() + "&EmploymentType=" + $("[id$=ddlEmploymentType]").val() + "&EmpCompany=" + $("[id$=ddlCompany]").val() + "&ToDate=" + $("[id$=txtDate]").val() + "&EmpDesignation=" + $("[id$=hdfDesignation]").val(), "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETE", "", true, true, false, 1, '<%= Resources.ErpRes.All_Small %>');
            ResetEmployee();
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
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
                //$("[id$=btnPrint]").hide();
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

        function ValidatePage(valGroup) {
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

        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtHdrDepartment" || targetControlID == "txtHdrBranchLocation" || targetControlID == "txtDesignation") {
                BindEmployee();
            }
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtHdrDepartment") {
                $("[id$=hdfHdrDepartment]").val("-1");
                BindEmployee();
            }
           else if (targetControlID == "txtHdrBranchLocation") {
                $("[id$=hdfHdrBranchLocation]").val("-1");
                BindEmployee();
            }
            else if (targetControlID == "txtDesignation") {
                $("[id$=hdfDesignation]").val("-1");
                BindEmployee();
            }
        }

        function ResetEmployee() {
            var defText = '<%= Resources.ErpRes.All_Small %>';
            $("[id$=txtEmployee]").val(defText);
            $("[id$=hdfEmployee]").val('-1');
        }


        function afterTimeSelect(from, id) {
            if (from == 'txtOTHours') {
                var otHrsId = id.replace("txtOTHours", "hdfOTHours");
                var otHrs = $("#" + id).val();
                var diff = timeDiff('00:00', otHrs);
                $("#" + otHrsId).val(diff);
            }
            else if (from == 'txtShortHours') {
                var shortHrsId = id.replace("txtShortHours", "hdfShortHours");
                var shortHrs = $("#" + id).val();
                var diff = timeDiff('00:00', shortHrs);
                $("#" + shortHrsId).val(diff);
            }
            else if (from == 'txtInTime') {
                var inVal = $("#" + id).val();
                var outId = id.replace("txtInTime", "txtOutTime");
                var outVal = $("#" + outId).val();
                if (outVal != '') {
                    // calculate diff
                    var normalHrsId = outId.replace("txtOutTime", "txtTotalHours");
                    var diff = timeDiff(inVal, outVal, normalHrsId);
                    if (isNaN(diff) || diff == "") {
                        $("#" + normalHrsId).val('');
                    }
                    normalHrsId = outId.replace("txtOutTime", "hdfTotalHours");
                    $("#" + normalHrsId).val(diff);
                    calculateExcess_Short_Hrs(id, diff);

                }
            }
            else {
                var outVal = $("#" + id).val();
                var inId = id.replace("txtOutTime", "txtInTime");
                var inVal = $("#" + inId).val();
                if (inVal != '') {
                    // calculate diff
                    var normalHrsId = inId.replace("txtInTime", "txtTotalHours");
                    var diff = timeDiff(inVal, outVal, normalHrsId);
                    if (isNaN(diff) || diff == "") {
                        $("#" + normalHrsId).val('');
                    }
                    normalHrsId = inId.replace("txtInTime", "hdfTotalHours");
                    $("#" + normalHrsId).val(diff);
                    calculateExcess_Short_Hrs(id, diff);
                }
            }
        }

        function timeDiff(inTime, outTime, normalHrsId) {
            //"06:45"
            var arrIn = inTime.split(":");
            var dateIn = new Date(2000, 0, 1, arrIn[0], arrIn[1]); // 9:00 AM
            var arrOut = outTime.split(":");
            var dateOut = new Date(2000, 0, 1, arrOut[0], arrOut[1]); // 5:00 PM
            if (dateOut < dateIn) {
                dateOut = new Date(2000, 0, 2, arrOut[0], arrOut[1]);
            }

            var diff = dateOut - dateIn;
            var msec = diff;
            var hh = Math.floor(msec / 1000 / 60 / 60);
            msec -= hh * 1000 * 60 * 60;
            var mm = Math.floor(msec / 1000 / 60);
            msec -= mm * 1000 * 60;
            // return hh + "." + mm;
            var m = mm;
            mm = mm / 60;
            mm = Math.round(mm * 100) / 100;
            var result = hh + "." + mm;
            result = result.replace("0.", "");
            var strHh = '';
            var strMm = '';
            if (hh < 1) {
                strHh = '00';
            }
            else if (hh < 10) {
                strHh = '0' + hh.toString();
            }
            else {
                strHh = hh.toString();
            }

            if (m < 1) {
                strMm = '00';
            }
            else if (m < 10) {
                strMm = '0' + m.toString();
            }
            else {
                strMm = m.toString();
            }
            if (normalHrsId != "")
                $("#" + normalHrsId).val(strHh + ":" + strMm);
            return result;
        }

        function calculateExcess_Short_Hrs(id, diff1) {
            id = id.replace("txtOutTime", "txtInTime");
            //new Begin
            var workHrId = id.replace("txtInTime", "hdfNormalHours");
            var workHrs1 = $("#" + workHrId).val();

            var diff = parseFloat(diff1);
            var workHrs = parseFloat(workHrs1);
            if (diff != workHrs) {
                if (diff < workHrs) { // Short Hrs
                    var diffTime = workHrs - diff;
                    var shortHrHidId = id.replace("txtInTime", "hdfShortHours");
                    if (parseInt($("[id$=hdfAttnEntryMode]").val()) == 1) {
                        $("#" + shortHrHidId).val(diffTime);
                        var shortHrTextId = id.replace("txtInTime", "txtShortHours");
                        var workHrs = DecimalToHHMM(diffTime);
                        $("#" + shortHrTextId).val(workHrs);
                    }

                    // Resets Excess Hrs
                    var otHrHidId = id.replace("txtInTime", "hdfOTHours");
                    $("#" + otHrHidId).val('');
                    var otHrTextId = id.replace("txtInTime", "txtOTHours");
                    $("#" + otHrTextId).val('');
                }
                else { // Excess Hrs
                    var diffTime = diff - workHrs;
                    var otHrHidId = id.replace("txtInTime", "hdfOTHours");
                    var otHrTextId = id.replace("txtInTime", "txtOTHours");
                    if (!isNaN(diffTime)) {
                        if (parseInt($("[id$=hdfAttnEntryMode]").val()) == 1) {
                            $("#" + otHrHidId).val(diffTime);
                            var workHrs = DecimalToHHMM(diffTime);
                            $("#" + otHrTextId).val(workHrs);
                        }
                    }
                    else {
                        $("#" + otHrHidId).val('');
                        $("#" + otHrTextId).val('');
                    }

                    // Resets Short Hrs
                    var shortHrHidId = id.replace("txtInTime", "hdfShortHours");
                    $("#" + shortHrHidId).val('');
                    var shortHrTextId = id.replace("txtInTime", "txtShortHours");
                    $("#" + shortHrTextId).val('');

                }

            }
            else {
                // Resets Excess Hrs
                var otHrHidId = id.replace("txtInTime", "hdfOTHours");
                $("#" + otHrHidId).val('');
                var otHrTextId = id.replace("txtInTime", "txtOTHours");
                $("#" + otHrTextId).val('');

                // Resets Short Hrs
                var shortHrHidId = id.replace("txtInTime", "hdfShortHours");
                $("#" + shortHrHidId).val('');
                var shortHrTextId = id.replace("txtInTime", "txtShortHours");
                $("#" + shortHrTextId).val('');
            }
            //new Ends
        }

        function DecimalToHHMM(decVal) {
            // 10.5 to 10:30,  10.75 to 10:45
            var arrTime = decVal.toString().split(".");
            var h = arrTime[0];
            var m = "0.";
            if (arrTime.length > 1) m = m + arrTime[1];
            else m = "0.0";

            var minute = Math.round(m * 60);

            if (h < 1) h = '00';
            else if (h < 10) h = '0' + h.toString();
            else h = h.toString();

            if (minute < 1) minute = '00';
            else if (minute < 10) minute = '0' + minute.toString();
            else minute = minute.toString();

            var arrMinute = minute.toString().split(".");
            return (h + ":" + arrMinute[0]);
            //return decVal;
        }

        function HHMMToDecimal(hourminutes) {
            var result = 0;
            if (hourminutes != "") {
                var arrTime = hourminutes.split(":");
                var hours = parseFloat(arrTime[0]);
                var minutes = 0;
                if (arrTime[1].length > 1)
                    minutes = parseFloat(arrTime[1]);
                result = (minutes / 60) + hours;
                //result = Math.round(result, 2);
                result = Math.round(result * 100) / 100;
            }
            return result;
        }

        // Method to get day name 
        // Parameter date format : 10-Feb-2011
        function GetDayName(dateString) {
            if (dateString != "" && dateString != undefined) {
                var curDate = $.datepicker.parseDate("dd-M-yy", dateString);
                var formatedDate = $.datepicker.formatDate("mm/dd/yy", curDate);
                var strDate = new Date(formatedDate);
                var weekday = new Array(7);
                weekday[0] = "SUN";
                weekday[1] = "MON";
                weekday[2] = "TUE";
                weekday[3] = "WED";
                weekday[4] = "THU";
                weekday[5] = "FRI";
                weekday[6] = "SAT";
                return weekday[strDate.getDay()];
            }
            else {
                return "";
            }
        }

        function GetEmpWorkHours(EmpPk, date) {
            $.ajax({
                url: "Attendance.aspx/GetEmpWorkHours",
                data: "{ 'EmployeePk': '" + EmpPk + "', 'AttenDate': '" + date + "' }",
                dataType: "json",
                type: "POST",
                async: false,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    empWorkHrs = data.d;
                }
            });
        }

        function ResetShortAndOT(controlID) {
            //Reset Short and OT
            var objInTime = $($("[id$='" + controlID + "']")).closest('tr').find("#[id*=txtInTime]");
            var inId = objInTime[0].id;
            var totalHrs = $($("[id$='" + controlID + "']")).closest('tr').find("#[id*=txtTotalHours]").val();
            if (totalHrs != '') {
                // calculate diff   
                var totalHrsDecimal = HHMMToDecimal(totalHrs);
                var outid = inId.replace("txtInTime", "txtOutTime");
                calculateExcess_Short_Hrs(outid, totalHrsDecimal);
            }
        }

        function ResetShortAndOTPopup() {
            //Reset Short and OT             
            var totalHrs = $("[id$=txtTotalTimePopup]").val();
            if (totalHrs != '') {
                var totalHrsDecimal = HHMMToDecimal(totalHrs);
                calculateExcess_Short_HrsPopUp(totalHrsDecimal);
            }
        }

        function ShowHideImportSec(flag) {
            ///<summary>
            /// Used to Show/Hide Import Section div
            ///</summary>
            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divImportSec]").show();
                $("[id$=imbShowImportSec]").hide();
                $("[id$=imbHideImportSec]").show();
            }
            else {
                $("[id$=divImportSec]").hide();
                $("[id$=imbShowImportSec]").show();
                $("[id$=imbHideImportSec]").hide();
            }
            return false;
        }

        function ShowHideFilterSec(flag) {
            ///<summary>
            /// Used to Show/Hide Filter Section div
            ///</summary>
            //If flag then Show Items
            $("[id$=hdfShowHideFilterSec]").val(flag);
            if (flag == 1) {
                $("[id$=divFilterSec]").show();
                $("[id$=imbShowFilterSec]").hide();
                $("[id$=imbHideFilterSec]").show();
            }
            else {
                var showhide = parseInt($("[id$=hdfShowHideFilterSec]").val());
                if (showhide != 1) {
                    $("[id$=divFilterSec]").hide();
                    $("[id$=imbShowFilterSec]").show();
                    $("[id$=imbHideFilterSec]").hide();
                }
            }
            return false;
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

        /// Used to disable Autocomplete
        function DisableAuto(extender, hfield) {
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
            $(extender).autocomplete("option", "disabled", true);
            $(extender).attr("disabled", true);
        }

        /// Used to disable Autocomplete
        function EnableAuto(extender, hfield) {
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect-disable").addClass("ddlSelect");
            $(extender).autocomplete("option", "disabled", false);
            $(extender).attr("disabled", false);
        }

        function AfterDateSelect(controlID) {
            if (controlID == "txtDate") {
                BindEmployee();
            }
        }
        function ClearDateSelect() {
            if ($("[id$=txtDate]").val() == '') {
                BindEmployee();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlExpenses">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdfDisablefDate" Value="0" />
            <div class="fixed-buttons-normal">
                <%--Top Buttons "Save", ...--%>
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
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="150" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePage('save')" ValidationGroup="save"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="151" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
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
                <%--Page Datas--%>
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
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("ImportAttendance").ToString()%></h1>
                                <asp:ImageButton runat="server" ID="imbShowImportSec" OnClientClick="javascript:return ShowHideImportSec(1);"
                                    SkinID="imbArrowInactive" ToolTip="<%$ resources:Show%>" TabIndex="5" />
                                <asp:ImageButton runat="server" ID="imbHideImportSec" OnClientClick="javascript:return ShowHideImportSec();"
                                    Style="display: none" SkinID="imbArrowActive" TabIndex="5" ToolTip="<%$ resources:Hide%>" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divImportSec" style="display: none">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S padgtop7">
                                                <asp:Label ID="lblImprtDate" runat="server" Text="<%$ resources:DateReq%>" AssociatedControlID="txtImportDate"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtImportDate" TabIndex="67" CssClass="input-small"
                                                    onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfImportDate" CssClass="star input-medium" SetFocusOnError="false"
                                                    ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="txtImportDate"
                                                    Display="Static" Text="*" ErrorMessage="<%$ resources:Err_ImportDate %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:Label ID="lblOverwrite" runat="server" Text="<%$ resources:OverwriteExisting%>"
                                                    AssociatedControlID="lblOverwrite" CssClass="lbl-35-2perc"></asp:Label>
                                                <asp:CheckBox ID="chkOverwrite" runat="server" TabIndex="68" Checked="false" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S padgtop7">
                                                <asp:Label ID="lblImprtBrLoc" runat="server" Text="<%$ resources:Branch/LocationReq%>"
                                                    AssociatedControlID="txtImportBranchLocation"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtImportBranchLocation" Text="" TabIndex="69" CssClass="select-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfImportBranchLocation" Value="" runat="server" />
                                                <asp:RequiredFieldValidator ID="vrfImportBranchLocation" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="txtImportBranchLocation"
                                                    Display="Dynamic" Text="*" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>"
                                                    ErrorMessage="<%$ resources:Err_SelectImportBranchLocation %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label runat="server" ID="lblImportRemarks" Text="<%$ resources:Remarks%>" AssociatedControlID="txtImportRemarks"
                                                    CssClass="input-w12-5per"></asp:Label>
                                                <asp:TextBox ID="txtImportRemarks" runat="server" TextMode="MultiLine" CssClass="multiline-2a-line input-full"
                                                    MaxLength="500" TabIndex="70"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblAttnTemplate" Text="<%$ resources:AttnTemplate%>"
                                                    AssociatedControlID="ddlAttnTemplate"></asp:Label>
                                                <asp:DropDownList ID="ddlAttnTemplate" runat="server" TabIndex="71" CssClass="select-small-g margnbotm5">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="VrfAttnTemplate" CssClass="star input-medium" SetFocusOnError="true"
                                                    ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="ddlAttnTemplate"
                                                    Display="Static" Text="*" ErrorMessage="<%$ resources:Err_AttnTemplate %>">
                                                </asp:RequiredFieldValidator>
                                                <a id="aTmpDwn" class="btnInner-dwn btnInner-med-size decoration-none margnrgt13-5per"
                                                    href='<%= Page.ResolveClientUrl((string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()) ? "~/Upload/" : System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() )+ "Template/" + GetGlobalResourceObject("ConfigurationsRes", "HrmsImportTemplateAttendance").ToString())%>'>
                                                    <%= Resources.Controls.Template.ToString() %>
                                                </a>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S padgbotm3 margnbotm5">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Label runat="server" ID="lblSourceFile" Text="<%$ resources: SourceFileStar %>"
                                                            AssociatedControlID="fupImport"></asp:Label>
                                                        <div class="fileupload-main">
                                                            <asp:FileUpload ID="fupImport" runat="server" TabIndex="72" CssClass="margnbotm0 margn-rgt0 upload-area3" />
                                                            <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star input-w27per" SetFocusOnError="true"
                                                                ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupImport"
                                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_File_Upload %>">
                                                            </asp:RequiredFieldValidator>
                                                        </div>
                                                        <asp:CheckBox ID="chkUpdateImportAttendance" Text="<%$ resources: Update %>" runat="server"
                                                            Style="display: none;" />
                                                        <asp:Button runat="server" ID="btnImport" CommandName="SAVEIMPORT" TabIndex="73"
                                                            Text="<%$resources:Import %>" OnClick="ActionHandler" ToolTip="<%$resources:Import %>"
                                                            SkinID="btnInner-add" ValidationGroup="upload" OnClientClick="javascript:ValidatePage('upload')"
                                                            Style="margin-bottom: 3px !important;" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnImport" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <div class="search-colapse" id="divAdvanceSearch" style="margin-top: 0px;">
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
                            <table class="table-devide" id="tbladvancedSearch">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn padgtop7">
                                            <asp:Label ID="lblFilterFromDate" runat="server" Text="<%$ resources:FromDate%>"
                                                AssociatedControlID="txtFilterFromDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterFromDate" TabIndex="50" CssClass="input-small margnbotm0"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblFilterToDate" runat="server" Text="<%$ resources:ToDate%>" AssociatedControlID="txtFilterToDate"
                                                CssClass="middle-lbl-a"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterToDate" TabIndex="51" CssClass="input-small margnbotm0"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn padgtop7">
                                            <asp:Label ID="lblfilterBranch" runat="server" Text="<%$ resources:BrLoc%>" AssociatedControlID="txtFilterBranch"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterBranch" Text="" TabIndex="52" CssClass="input-half margnbotm0"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterBranch" Value="" runat="server" />
                                            <%-- <asp:Label ID="lblFilterDept" runat="server" Text="<%$ resources:Dept%>" AssociatedControlID="txtFilterDept"
                                                CssClass="middle-lbl-xsmall-b"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterDept" Text="" TabIndex="53" CssClass="select-small-c margnbotm0"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterDept" Value="" runat="server" />--%>
                                            <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search%>"
                                                ToolTip="<%$ resources:Controls,Search%>" ValidationGroup="Search" OnClick="ActionHandler"
                                                TabIndex="54" CommandName="SEARCH" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClearSearch" runat="server" Text="<%$ resources:Controls,Clear%>"
                                                ToolTip="<%$ resources:Controls,Clear%>" TabIndex="55" OnClick="ActionHandler"
                                                CommandName="CLEARSEARCH" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdList" Width="100%" AllowPaging="false" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" TabIndex="56"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfPk" Value='<%# Eval("EAR_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TrxNo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnNo" runat="server" CssClass="text-underline" CommandArgument='<%# Eval("EAR_PK") %>'
                                                    OnClick="ActionHandler" CommandName="ATTDETAILS" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("EAR_NO")))?Resources.ErpRes.Draft:Eval("EAR_NO")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("EAR_NO")))?Resources.ErpRes.Draft:Eval("EAR_NO")%>'></asp:LinkButton>
                                                <asp:HiddenField runat="server" ID="hdfPk1" Value='<%# Eval("EAR_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLstDate" runat="server" Text='<%# Eval("EAR_TO_DATE", Resources.Constants.HRMSDateFormatGrid)  %>'
                                                    ToolTip='<%# Eval("EAR_TO_DATE", Resources.Constants.HRMSDateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Branch/Location%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLstBranch" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EAR_BRANCH_TEXT"))),55) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EAR_BRANCH_TEXT")))  %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="37%" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:Department%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLstDept" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EAR_EMPDEPARTMENT_TEXT"))),35) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EAR_EMPDEPARTMENT_TEXT"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Remarks%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLstRemarks" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EAR_REMARKS"))),70) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EAR_REMARKS"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="40%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" TabIndex="4" />
                                <div class="clear">
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <%--EntryPage Table Row--%>
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblTrxNoHdr" runat="server" Text="<%$ resources:TrxNo%>" AssociatedControlID="lblTrxNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblTrxNo" CssClass="input-small"></asp:Label>
                                            <asp:Label runat="server" ID="lblDate" Text="<%$ resources:DateReq%>" AssociatedControlID="txtDate"
                                                CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDate" CssClass="input-small" TabIndex="1" onkeydown="return CheckKey(event)"
                                                onblur="ClearDateSelect()" onpaste="return false;" MaxLength="11"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfDate" CssClass="star" SetFocusOnError="true" ValidationGroup="save"
                                                EnableClientScript="true" runat="server" ControlToValidate="txtDate" Display="Dynamic"
                                                Text="*" ErrorMessage="<%$ resources:Err_Date %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblHdrBranchLocation" runat="server" Text="<%$ resources:Branch/LocationReq%>"
                                                AssociatedControlID="txtHdrBranchLocation"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtHdrBranchLocation" Text="" TabIndex="2" CssClass="select-half"></asp:TextBox>
                                            <asp:HiddenField ID="hdfHdrBranchLocation" Value="" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfHdrBranchLocation" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="save" EnableClientScript="true" runat="server" ControlToValidate="txtHdrBranchLocation"
                                                Display="Dynamic" Text="*" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>"
                                                ErrorMessage="<%$ resources:Err_SelectBranchLocation %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="div2col-S">
                                            <asp:Label ID="lblRemarks" runat="server" Text="<%$ resources:Remarks%>" AssociatedControlID="txtRemarks"
                                                CssClass="input-w12-5per"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRemarks" Text="" TabIndex="4" TextMode="MultiLine"
                                                CssClass="multiline-2a-line input-full" MaxLength="500" onkeydown="limitText(this,500);"
                                                onkeyup="limitText(this,500);" onpase="limitText(this,500);">
                                            </asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("AdvSearch").ToString()%></h1>
                                <asp:ImageButton runat="server" ID="imbShowFilterSec" OnClientClick="javascript:return ShowHideFilterSec(1);"
                                    SkinID="imbArrowInactive" ToolTip="<%$ resources:Show%>" TabIndex="9" />
                                <asp:ImageButton runat="server" ID="imbHideFilterSec" OnClientClick="javascript:return ShowHideFilterSec(0);"
                                    Style="display: none" SkinID="imbArrowActive" TabIndex="9" ToolTip="<%$ resources:Hide%>" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divFilterSec" style="display: none">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblCompany" Text="<%$ resources:Company%>" AssociatedControlID="ddlCompany"></asp:Label>
                                                <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="10" CssClass="select-half-a"
                                                    onchange="javascript:BindEmployee();">
                                                </asp:DropDownList>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblDesignation" runat="server" Text="<%$ resources:Designation%>"
                                                    AssociatedControlID="txtDesignation"></asp:Label>
                                                <asp:HiddenField ID="hdfDesignation" runat="server" />
                                                <asp:TextBox runat="server" TabIndex="14" ID="txtDesignation" CssClass="select-half" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblEmploymentType" runat="server" Text="<%$ resources:EmploymentType%>"
                                                    AssociatedControlID="ddlEmploymentType"></asp:Label>
                                                <asp:DropDownList ID="ddlEmploymentType" runat="server" TabIndex="13" CssClass="select-small-a"
                                                    onchange="javascript:BindEmployee();">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblHdrDepartment" runat="server" Text="<%$ resources:Dept%>" AssociatedControlID="txtHdrDepartment"
                                                    CssClass="middle-lbl-xsmall-c"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtHdrDepartment" Text="" TabIndex="13" CssClass="input-w28per"></asp:TextBox>
                                                <asp:HiddenField ID="hdfHdrDepartment" Value="" runat="server" />
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblEmployee" runat="server" Text="<%$ resources:Employee%>" AssociatedControlID="txtEmployee"></asp:Label>
                                                <%--                                            <asp:DropDownList ID="ddlEmployee" runat="server" TabIndex="6" CssClass="select-half-a">
                                            </asp:DropDownList>--%>
                                                <asp:TextBox ID="txtEmployee" runat="server" TabIndex="15" CssClass="input-half"
                                                    MaxLength="100"> </asp:TextBox>
                                                <asp:HiddenField ID="hdfEmployee" runat="server" Value="0" />
                                                <%-- <label class="span-normal">
                                            </label>--%>
                                                <%-- <asp:Label ID="Label1" runat="server" Text="" CssClass="span-normal" style="width:300px!important;"></asp:Label>--%>
                                                <%--<asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="9" CommandName="GO" SkinID="search-ext"
                                                Style="margin-bottom: 0px!important; margin-top: 2px;" OnClientClick="javascript:ValidatePage('Go')" 
                                                ValidationGroup="Go"/>
                                            <asp:ImageButton ID="btnClr" runat="server" ToolTip="<%$resources:Controls,Clear %>"
                                                TabIndex="10" OnClick="ActionHandler" CommandName="CLEAR" Style="margin-bottom: 0px!important;
                                                margin-top: 2px;" SkinID="clear-ext" OnClientClick="javascript:ValidatePage('save')" 
                                                ValidationGroup="save" />--%>
                                                <%--<asp:Button runat="server" ID="btnGo" CommandName="GO" TabIndex="16" Text="<%$resources:Controls,Go %>"
                                                    OnClick="ActionHandler" OnClientClick="javascript:ValidatePage('save')" ValidationGroup="save"
                                                    ToolTip="<%$resources:Controls,Go %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-search" />
                                                <asp:Button runat="server" ID="btnClear" CommandName="CLEAR" TabIndex="17" Text="<%$resources:Controls,Clear %>"
                                                    OnClick="ActionHandler" ToolTip="<%$resources:Controls,Clear %>" CommandArgument="SEC_ActionPanel"
                                                    SkinID="btnInner-cancel-dsd" />--%>
                                                <%--<asp:Button runat="server" ID="btnNewAttendance" CommandName="ADDNEW" Visible="false"
                                                    OnClientClick="javascript:ValidatePage('save')" ValidationGroup="save" OnClick="ActionHandler"
                                                    Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                                    ToolTip="<%$resources:Controls,New %>" Style="margin-right: 0px!important;" />--%>
                                                <asp:ImageButton ID="btnGo" runat="server" Text="<%$ resources:Controls,Search%>"
                                                    ToolTip="<%$ resources:Controls,Search%>" OnClientClick="javascript:ValidatePage('save')"
                                                    ValidationGroup="save" OnClick="ActionHandler" TabIndex="16" CommandName="GO"
                                                    SkinID="search-ext" CssClass="margntop2 margnbotm0 margn-rgt4" />
                                                <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear%>"
                                                    OnClientClick="javascript:ValidatePage('save')" ToolTip="<%$ resources:Controls,Clear%>"
                                                    TabIndex="17" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext"
                                                    CssClass="margntop2 margnbotm0 margn-rgt4" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdEmployeeAttendance" Width="100%" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" Text="" runat="server" Checked='<%# (Convert.ToInt32(Eval("EAT_PK")) > 0) ? true : false %>'
                                                    TabIndex="18" Enabled='<%# (Convert.ToInt32(Eval("EAT_PK")) > 0) ? false : true %>' />
                                                <asp:HiddenField runat="server" ID="hdfEAT_PK" Value='<%# Eval("EAT_PK") %>' />
                                                <asp:HiddenField ID="hdfEmployeePk" runat="server" Value='<%# Eval("EAT_EMPLOYEE") %>' />
                                                <asp:HiddenField ID="hdfNormalHours" runat="server" Value='<%# Eval("EAT_NORMAL_HRS") %>' />
                                                <asp:HiddenField ID="hdfBrLocPk" runat="server" Value='<%# Eval("empBranch") %>' />
                                                <asp:HiddenField ID="hdfDeptPk" runat="server" Value='<%# Eval("DPT_PK") %>' />
                                                <%--<asp:HiddenField ID="hdfCheckedFlag" runat="server" Value='<%# Eval("EMP_FL") %>' />--%>
                                                <%--<asp:HiddenField ID="hdfModDate" runat="server" Value='<%# Eval("ModifiedDate") %>' />--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Employee %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("EAT_EMPLOYEE_TEXT") ,50) %>'
                                                    ToolTip='<%# Eval("EAT_EMPLOYEE_TEXT")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="45%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Location %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocationGridView" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("empBranchCode") ,13) %>'
                                                    ToolTip='<%# Eval("empBranchText")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Dept %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDept" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("DPT_CODE") ,19) %>'
                                                    ToolTip='<%# Eval("empDepartmentText")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:Designation %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpDesignation" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("empDesignationText") ,19) %>'
                                                    ToolTip='<%# Eval("empDesignationText")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>--%>
                                        <%-- <asp:TemplateField HeaderText="<%$ resources:First %>">
                                            <ItemTemplate>
                                                <asp:CheckBox Text="" runat="server" ID="chkFirst" Checked="<%# Eval(Resources.DataFieldRes.EAT_FIRST)!=null?((Eval(Resources.DataFieldRes.EAT_FIRST)).ToString()==1.ToString() ? true:false):false %>" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Second %>">
                                            <ItemTemplate>
                                                <asp:CheckBox Text="" runat="server" ID="chkSecond" Checked="<%# Eval(Resources.DataFieldRes.EAT_SECOND)!=null?((Eval(Resources.DataFieldRes.EAT_SECOND)).ToString()==1.ToString() ? true:false):false %>" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:InTime %>">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtInTime" TabIndex="18" MaxLength="12" CssClass="small"
                                                    onkeydown="return CheckKey(event)" onpaste="return false;" OnTextChanged="ActionHandler"
                                                    AutoPostBack="false" EnableViewState="true" Text='<%# GetTimeSpan(Eval("EAT_IN_TIME")) %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:OutTime %>">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtOutTime" TabIndex="18" MaxLength="12" CssClass="small"
                                                    onkeydown="return CheckKey(event)" onpaste="return false;" OnTextChanged="ActionHandler"
                                                    Text='<%# GetTimeSpan(Eval("EAT_OUT_TIME")) %>' AutoPostBack="false" EnableViewState="true"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TotalHours %>">
                                            <ItemTemplate>
                                                <%--  placeholder="<%$ resources:timeFormat%>"--%>
                                                <asp:TextBox runat="server" ID="txtTotalHours" CssClass="small" ValidationGroup="save"
                                                    TabIndex="18" />
                                                <cc1:MaskedEditExtender ID="meeNormalHours" runat="server" AutoComplete="false" Mask="<%$ resources:ConfigurationsRes,HrmsTimeMask %>"
                                                    MaskType="Time" TargetControlID="txtTotalHours">
                                                </cc1:MaskedEditExtender>
                                                <asp:RegularExpressionValidator runat="server" ID="regNormalHours" CssClass="star"
                                                    SetFocusOnError="true" ValidationGroup="save" ControlToValidate="txtTotalHours"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ValidTotalHour %>"
                                                    ValidationExpression="<%$ resources:ConfigurationsRes,HrmsTimeMaskValidationExp %>">
                                                </asp:RegularExpressionValidator>
                                                <%--Text="<%# Eval(Resources.DataFieldRes.EAT_NORMAL_HRS) %>"--%>
                                                <%--<asp:RegularExpressionValidator runat="server" ID="regNormalHours" CssClass="star"
                                                    SetFocusOnError="true" ValidationGroup="save" ControlToValidate="txtTotalHours"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ValidHour %>" ValidationExpression="[0-9]+(\.[0-9][0-9]?)?">
                                                </asp:RegularExpressionValidator>--%>
                                                <asp:HiddenField ID="hdfTotalHours" runat="server" Value='<%# Eval("EAT_WORKED_HRS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ShortHours %>">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtShortHours" CssClass="small" ValidationGroup="save"
                                                    TabIndex="18" />
                                                <cc1:MaskedEditExtender ID="meeShortHours" runat="server" AutoComplete="false" Mask="<%$ resources:ConfigurationsRes,HrmsTimeMask %>"
                                                    MaskType="Time" TargetControlID="txtShortHours">
                                                </cc1:MaskedEditExtender>
                                                <asp:RegularExpressionValidator runat="server" ID="regShortHours" CssClass="star"
                                                    SetFocusOnError="true" ValidationGroup="save" ControlToValidate="txtShortHours"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ValidShortHour %>"
                                                    ValidationExpression="<%$ resources:ConfigurationsRes,HrmsTimeMaskValidationExp %>">
                                                </asp:RegularExpressionValidator>
                                                <%--Text="<%# Eval(Resources.DataFieldRes.EAT_NORMAL_HRS) %>"--%>
                                                <%--<asp:RegularExpressionValidator runat="server" ID="regNormalHours" CssClass="star"
                                                    SetFocusOnError="true" ValidationGroup="save" ControlToValidate="txtTotalHours"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ValidHour %>" ValidationExpression="[0-9]+(\.[0-9][0-9]?)?">
                                                </asp:RegularExpressionValidator>--%>
                                                <asp:HiddenField ID="hdfShortHours" runat="server" Value='<%# Eval("EAT_SHORT_HRS")%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:OTHours %>">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtOTHours" CssClass="small" ValidationGroup="save"
                                                    TabIndex="18" />
                                                <cc1:MaskedEditExtender ID="meeOTHours" runat="server" AutoComplete="false" Mask="<%$ resources:ConfigurationsRes,HrmsTimeMask %>"
                                                    MaskType="Time" TargetControlID="txtOTHours">
                                                </cc1:MaskedEditExtender>
                                                <asp:RegularExpressionValidator runat="server" ID="regOTHours" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="save" ControlToValidate="txtOTHours" Display="Dynamic" Text="*"
                                                    ErrorMessage="<%$ resources:Err_ValidOTHour %>" ValidationExpression="<%$ resources:ConfigurationsRes,HrmsTimeMaskValidationExp %>">
                                                </asp:RegularExpressionValidator>
                                                <%-- <asp:RegularExpressionValidator runat="server" ID="regOTHours" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="save" ControlToValidate="txtOTHours" Display="Dynamic" Text="*"
                                                    ErrorMessage="<%$ resources:Err_ValidHour %>" ValidationExpression="[0-9]+(\.[0-9][0-9]?)?">
                                                </asp:RegularExpressionValidator>--%>
                                                <asp:HiddenField ID="hdfOTHours" runat="server" Value='<%# Eval("EAT_OT_HRS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbViewAttendance"
                                                    ToolTip="<%$ resources:Controls,View %>" SkinID="btnview" CommandName="VIEWATTENDANCEDETAILS"
                                                    OnClick="ActionHandler" TabIndex="18" Visible='<%# (Convert.ToInt32(Eval("EAT_PK")) > 0 && Convert.ToInt32(Eval("EMP_IS_SAL_PRCD")) == 0) ? true : false %>' />
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbDeleteAttendance"
                                                    ToolTip="<%$ resources:Controls,Delete %>" SkinID="imbdeletegrid" CommandName="GRIDDELETE"
                                                    OnClick="ActionHandler" OnClientClick="return ShowDeleteConfirm(this);" TabIndex="18"
                                                    Visible='<%# (Convert.ToInt32(Eval("EAT_PK")) > 0 && Convert.ToInt32(Eval("EMP_IS_SAL_PRCD")) == 0) ? true : false %>' />
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" HorizontalAlign="Center" Width="3%" />
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
            <div id="divEmpAttendanceDetails" style="display: none">
                <asp:GridView runat="server" ID="grdEmpAttndance" Width="100%" AutoGenerateColumns="false"
                    EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler">
                    <EmptyDataTemplate>
                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField></asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ resources:Time %>">
                            <ItemTemplate>
                                <asp:Label ID="lblTime" runat="server" Text='<%# Eval("EAL_TIME", Resources.Constants.DateTimeFormatGrid) %>'
                                    ToolTip='<%# Eval("EAL_TIME", Resources.Constants.DateTimeFormatGrid) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ resources:InOut %>">
                            <ItemTemplate>
                                <asp:Label ID="lblAction" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("EAL_ACTION") ,30) %>'
                                    ToolTip='<%# Eval("EAL_ACTION")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="diverror" style="display: none">
                <%--Use this label to bind the server errors--%>
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsGo" ValidationGroup="Go" runat="server" />
                <asp:ValidationSummary ID="vsPage" ValidationGroup="save" runat="server" />
                <asp:ValidationSummary ID="vsUpload" ValidationGroup="upload" runat="server" />
                <asp:ValidationSummary ID="vsAddToList" ValidationGroup="AddToList" runat="server" />
            </div>
            <asp:HiddenField runat="server" ID="hdfEmpWorkHrs" />
            <asp:HiddenField runat="server" ID="hdfTemp" />
            <asp:HiddenField runat="server" ID="hdfAttnEntryMode" Value="2" />
            <asp:HiddenField runat="server" ID="hdfShowHideFilterSec" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
