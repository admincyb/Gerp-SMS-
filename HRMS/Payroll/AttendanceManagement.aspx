<%@ Page Title="<%$ Resources:Captions,Title_Attendance %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="AttendanceManagement.aspx.cs" Inherits="HRMS.Payroll.AttendanceManagement"
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
                GrandScriptUtils.DatePickerCommon("txtDetailDate");
                GrandScriptUtils.AddDateRangeCommon("txtFilterFromDate", "hdfFilterFromDate", "txtFilterToDate", "hdfFilterToDate", false, false);
                GrandScriptUtils.MakeAutoCompleteDDL("txtDesignation", url, "hdfDesignation", true, true, "DESIGNATION");
                GrandScriptUtils.MakeAutoCompleteDDL("txtBrLocDtlSearch", url, "hdfBrLocDtlSearch", true, true, "BRANCHLOCATION");
                GrandScriptUtils.MakeAutoCompleteDDL("txtDeptDtlSearch", url, "hdfDeptDtlSearch", true, true, "DEPARTMENT");
                GrandScriptUtils.MakeAutoCompleteComboBox("txtBranchLocation", url, "hdfBranchLocation", "hdfBranchLocationCode", true, true, "BRANCHLOCATION");
                GrandScriptUtils.MakeAutoCompleteComboBox("txtHdrBranchLocation", url, "hdfHdrBranchLocation", "hdfHdrBranchLocationCode", true, true, "BRANCHLOCATION");
                GrandScriptUtils.MakeAutoCompleteDDL("txtFilterBranch", url, "hdfFilterBranch", true, true, "BRANCHLOCATION");
                GrandScriptUtils.MakeAutoCompleteDDL("txtFilterTrxNo", url, "hdfFilterTrxNo", true, true, "TRANSACTIONNO");
                GrandScriptUtils.MakeAutoCompleteComboBox("txtDepartment", url, "hdfDepartment", "hdfDepartmentCode", true, true, "DEPARTMENT");
                GrandScriptUtils.MakeAutoCompleteComboBox("txtHdrDepartment", url, "hdfHdrDepartment", "hdfHdrDepartmentCode", true, true, "DEPARTMENT");
                GrandScriptUtils.MakeAutoCompleteDDL("txtFilterDept", url, "hdfFilterDept", true, true, "DEPARTMENT");
                // GrandScriptUtils.MakeAutoCompleteDDL("txtEmpSearch", url + "?EmpCategory=2", "hdfEmpSearch", true, true, "EMPLOYEEAUTOCOMPLETE");
                //  GrandScriptUtils.MakeAutoCompleteDDL("txtEmpSearch", url + "&EmpCategory=2" + "&EmpDept=" + $("[id$=hdfHdrDepartment]").val(), "hdfEmpSearch", true, true, "EMPLOYEEAUTOCOMPLETE");

                //GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee", url + "?EmpCategory=2", "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETE");
                BindEmployeeFilter();
                BindEmployee();
                SetAttendanceRowColor();
                if ($("[id$=txtDate]").attr("disabled") == true) {
                    $("[id$=txtDate]").addClass("input-disabled");
                }
                else {
                    $("[id$=txtDate]").removeClass("input-disabled");
                }

                var deptid = parseInt($("[id$=hdfHdrDepartment]").val());
                if (deptid > 0)
                    DisableAuto($("[id$=txtDepartment]"), $("[id$=hdfDepartment]"));
                var branchid = parseInt($("[id$=hdfHdrBranchLocation]").val());
                if (branchid > 0)
                    DisableAuto($("[id$=txtBranchLocation]"), $("[id$=hdfBranchLocation]"));

                var dtlrowcount = parseInt($("[id$=hdfDetailsRowCount]").val());
                if (dtlrowcount > 0)
                    DisableAuto($("[id$=txtHdrDepartment]"), $("[id$=hdfHdrDepartment]"));
                else
                    EnableAuto($("[id$=txtHdrDepartment]"), $("[id$=hdfHdrDepartment]"));

                var currpk = parseInt($("[id$=hdfCurrPk]").val());
                if (currpk > 0) {
                    $("[id$=divImportDetails]").show();
                }
                else {
                    $("[id$=divImportDetails]").hide();
                }

                var processMode = parseInt($("[id$=ddlProcessModeHdr]").val());
                if (processMode <= 0)
                    DisableAuto($("[id$=txtEmployee]"), $("[id$=hdfEmployee]"));
                else
                    EnableAuto($("[id$=txtEmployee]"), $("[id$=hdfEmployee]"));

                ShowHideDtlSearch($("[id$=hdfShowHideDtlSearch]").val());

            });
        }

        function ProcessModeChanged() {
            BindEmployeeFilter();
            ResetEmployeeFilter();
            BindEmployee();
            ResetEmployee();
            var processMode = parseInt($("[id$=ddlProcessModeHdr]").val());
            if (processMode <= 0)
                DisableAuto($("[id$=txtEmployee]"), $("[id$=hdfEmployee]"));
            else
                EnableAuto($("[id$=txtEmployee]"), $("[id$=hdfEmployee]"));
        }

        function BindEmployee() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee", url + "?Type=" + $("[id$=hdfBranchLocation]").val() + "&EmpCategory=2" + "&EmpDept=" + $("[id$=hdfDepartment]").val() + "&ProcessMode=" + $("[id$=ddlProcessModeHdr]").val() + "&ToDate=" + $("[id$=txtDate]").val(), "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETEBYFILTER", "", true, true, false, 1);
        }
        function BindEmployeeFilter() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtEmpSearch", url + "?Type=" + $("[id$=hdfBrLocDtlSearch]").val() + "&EmpCategory=2" + "&EmpDept=" + $("[id$=hdfHdrDepartment]").val() + "&ProcessMode=" + $("[id$=ddlProcessModeHdr]").val() + "&ToDate=" + $("[id$=txtDate]").val(), "hdfEmpSearch", true, true, "EMPLOYEEAUTOCOMPLETEBYFILTER", "", true, true, false, 1);
        }

        function ResetEmployee() {
            var defText = '<%= Resources.ErpRes.AutoDefaultValue %>';
            $("[id$=txtEmployee]").val(defText);
            $("[id$=hdfEmployee]").val('-1');
        }
        function ResetEmployeeFilter() {
            var defText = '<%= Resources.ErpRes.AutoDefaultValue %>';
            $("[id$=txtEmpSearch]").val(defText);
            $("[id$=hdfEmpSearch]").val('-1');
        }

        var msgTitle = '<%= Resources.ErpRes.Information %>';
        var msgContent = "";
        function ShowListing(flag) {
            if (flag) {
                $("[id$=PageAction_List]").show();
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlListing]").show();
                $("[id$=pnlPrint]").show();
                $("[id$=pnlEntry]").hide();
            }
            else {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
                $("[id$=pnlPrint]").show();
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
                //$("[id$=pnlPrint]").hide();
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
            if (targetControlID == "txtHdrDepartment") {
                SetBranchDeptSearch(0, 0);
                BindEmployeeFilter();
                ResetEmployeeFilter();
            }
            if (targetControlID == "txtHdrBranchLocation") {
                SetBranchDeptSearch(1, 0);
                BindEmployeeFilter();
                ResetEmployeeFilter();
            }
            if (targetControlID == "txtDepartment" || targetControlID == "txtBranchLocation" || targetControlID == "txtHdrDepartment" || targetControlID == "txtHdrBranchLocation") {
                BindEmployee();
                ResetEmployee();
            }
            if (targetControlID == "txtEmployee") {
                GetEmpWorkHours();
                GetEmpBreakHours();
                //CalculateTotal();
                CalculateNormalAndOT();
            }
        }

        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtHdrDepartment") {
                $("[id$=hdfDepartment]").val("-1");
                $("[id$=hdfDepartmentCode]").val("");
                SetBranchDeptSearch(0, 1);
                BindEmployeeFilter();
            }
            if (targetControlID == "txtHdrBranchLocation") {
                $("[id$=hdfHdrBranchLocation]").val("-1");
                $("[id$=hdfHdrBranchLocationCode]").val("");
                SetBranchDeptSearch(1, 1);
                BindEmployeeFilter();            
            }
            if (targetControlID == "txtDepartment" || targetControlID == "txtBranchLocation" || targetControlID == "txtHdrDepartment" || targetControlID == "txtHdrBranchLocation") {
                BindEmployee();
                ResetEmployee();
            }
            if (targetControlID == "txtEmployee") {
                GetEmpWorkHours();
                GetEmpBreakHours();
                //CalculateTotal();
                CalculateNormalAndOT();
            }
        }

        function AfterDateSelect(controlID) {
            if (controlID == "txtDate") {
                GetEmpWorkHours();
                //CalculateTotal();
                CalculateNormalAndOT();
                BindEmployeeFilter();
                ResetEmployeeFilter();
                BindEmployee();
                ResetEmployee();
                $("[id$=txtDetailDate]").val($("[id$=txtDate]").val());
            }
        }
        function ClearDateSelect() {
            if ($("[id$=txtDate]").val() == '') {
                BindEmployeeFilter();
                ResetEmployeeFilter();
                BindEmployee();
                ResetEmployee();
            }
        }

        function SetBranchDeptSearch(mode, isInvalid) {
            //mode  0:dept,1:Branch
            var defaultText = '<%= Resources.ErpRes.AutoDefaultValue %>';
            if (parseInt(mode) == 0) {
                if (parseInt(isInvalid) == 1) {
                    $("[id$=txtDepartment]").val(defaultText);
                    $("[id$=hdfDepartment]").val("-1");
                    $("[id$=hdfDepartmentCode]").val("");
                    EnableAuto($("[id$=txtDepartment]"), $("[id$=hdfDepartment]"));
                }
                else {
                    var dept = $("[id$=txtHdrDepartment]").val();
                    var deptid = parseInt($("[id$=hdfHdrDepartment]").val());
                    var deptcode = $("[id$=hdfHdrDepartmentCode]").val();
                    $("[id$=txtDepartment]").val(dept);
                    $("[id$=hdfDepartment]").val(deptid);
                    $("[id$=hdfDepartmentCode]").val(deptcode);
                    DisableAuto($("[id$=txtDepartment]"), $("[id$=hdfDepartment]"));
                }
            }
            else if (parseInt(mode) == 1) {
                if (parseInt(isInvalid) == 1) {
                    $("[id$=txtBranchLocation]").val(defaultText);
                    $("[id$=hdfBranchLocation]").val("-1");
                    $("[id$=hdfBranchLocationCode]").val("");
                    EnableAuto($("[id$=txtBranchLocation]"), $("[id$=hdfBranchLocation]"));
                }
                else {
                    var branch = $("[id$=txtHdrBranchLocation]").val();
                    var branchid = parseInt($("[id$=hdfHdrBranchLocation]").val());
                    var branchcode = $("[id$=hdfHdrBranchLocationCode]").val();
                    $("[id$=txtBranchLocation]").val(branch);
                    $("[id$=hdfBranchLocation]").val(branchid);
                    $("[id$=hdfBranchLocationCode]").val(branchcode);
                    DisableAuto($("[id$=txtBranchLocation]"), $("[id$=hdfBranchLocation]"));
                }
            }
        }

        function afterTimeSelect(from, id) {
            if (from == 'txtInTime' || from == 'txtBOutTime1' || from == 'txtBInTime1' || from == 'txtBOutTime2' || from == 'txtBInTime2' || from == 'txtOutTime') {
                CalculateTotal();
            }
        }

        function CalculateTotal() {
            var timediff = 0;
            var arrDiff;
            var TotalHrsCalcMode = parseInt($("[id$=hdfTotalHrsCalcMode]").val());
            var InTime = $("[id$=txtInTime]").val();
            var BOutTime1 = $("[id$=txtBOutTime1]").val();
            var BInTime1 = $("[id$=txtBInTime1]").val();
            var BOutTime2 = $("[id$=txtBOutTime2]").val();
            var BInTime2 = $("[id$=txtBInTime2]").val();
            var OutTime = $("[id$=txtOutTime]").val();
            var Indate = new Date(2000, 0, 1, 0, 0);
            var OutDate = new Date(2000, 0, 1, 0, 0);
            if (!isNaN(TotalHrsCalcMode) && TotalHrsCalcMode == 1) {  //1:First in and Last Out, 2:Actual in time only 
                var decInTime = parseFloat(HHMMToDecimal(InTime));
                var decOutTime = parseFloat(HHMMToDecimal(OutTime));
                //if (decInTime > 0 && decOutTime > 0)
                timediff = parseFloat(CalculateTimeDiff(InTime, OutTime));
            }
            else {
                var decInTime = parseFloat(HHMMToDecimal(InTime));
                var decBOutTime1 = parseFloat(HHMMToDecimal(BOutTime1));
                var decBInTime1 = parseFloat(HHMMToDecimal(BInTime1));
                var decBOutTime2 = parseFloat(HHMMToDecimal(BOutTime2));
                var decBInTime2 = parseFloat(HHMMToDecimal(BInTime2));
                var decOutTime = parseFloat(HHMMToDecimal(OutTime));
                var out2 = 0;
                var lastout = 0;
                if (InTime != '' && !isNaN(decInTime)) {
                    if (BOutTime1 != '' && !isNaN(decBOutTime1))
                        timediff = parseFloat(CalculateTimeDiff(InTime, BOutTime1));
                    else if (BOutTime2 != '' && !isNaN(decBOutTime2)) {
                        timediff = parseFloat(CalculateTimeDiff(InTime, BOutTime2));
                        out2 = 1;
                    }
                    else if (OutTime != '' && !isNaN(decOutTime)) {
                        timediff = parseFloat(CalculateTimeDiff(InTime, OutTime));
                        lastout = 1;
                    }
                }
                if (BInTime1 != '' && !isNaN(decBInTime1)) {
                    if (BOutTime2 != '' && out2 == 0 && !isNaN(decBOutTime2)) //decBOutTime2 > 0 && decBOutTime1 > 0
                        timediff += parseFloat(CalculateTimeDiff(BInTime1, BOutTime2));
                    else if (OutTime != '' && out2 == 0 && lastout == 0 && !isNaN(decOutTime)) { //decOutTime > 0 && decBOutTime1 > 0
                        timediff += parseFloat(CalculateTimeDiff(BInTime1, OutTime));
                        lastout = 1;
                    }
                }
                if (BInTime2 != '' && !isNaN(decBInTime2) && OutTime != '' && !isNaN(decOutTime) && lastout == 0) {
                    //(decBOutTime1 > 0 && decBOutTime2 > 0) || (decBInTime1 == 0 && decInTime == 0) || (decBOutTime1 == 0 && decBInTime1 == 0)
                    //if (lastout == 0)
                    timediff += parseFloat(CalculateTimeDiff(BInTime2, OutTime));
                }

                //                timediff = parseFloat(CalculateTimeDiff(InTime, BOutTime1));
                //                timediff += parseFloat(CalculateTimeDiff(BInTime1, BOutTime2));
                //                timediff += parseFloat(CalculateTimeDiff(BInTime2, OutTime));
                //                if (timediff == 0)
                //                    timediff = parseFloat(CalculateTimeDiff(InTime, OutTime));
            }

            if (isNaN(timediff))
                timediff = 0;
            var tothrs = DecimalToHHMM(timediff);
            if (tothrs != "") {
                arrDiff = tothrs.split(":");
                OutDate.setHours(arrDiff[0], arrDiff[1]);
            }

            var diff = OutDate - Indate;
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

            $("[id$=txtTotalHours]").val(strHh + ":" + strMm);

            CalculateNormalAndOT();

        }

        function CalculateTimeDiff(inTime, outTime) {
            // Rerurns time difference in decimal
            //"06:45"
            if (inTime != '' && outTime != '') {
                var arrIn = inTime.split(":");
                var arrOut = outTime.split(":");
                //                if ((parseInt(arrIn[0]) == 0 && parseInt(arrIn[1]) == 0) || (parseInt(arrOut[0]) == 0 && parseInt(arrOut[1]) == 0))
                //                    return 0;
                if (isNaN(parseInt(arrIn[0])) || isNaN(parseInt(arrIn[1])) || isNaN(parseInt(arrOut[0])) || isNaN(parseInt(arrOut[1])))
                    return 0;
                var dateIn = new Date(2000, 0, 1, arrIn[0], arrIn[1]); // 9:00 AM                
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
                return result;
            }
            else {
                return 0;
            }
        }

        function CalculateNormalAndOT() {
            var totalhrs = $("[id$=txtTotalHours]").val();
            var empWorkHrs = parseFloat($("[id$=hdfEmpWorkHours]").val());
            if (totalhrs != '' && !isNaN(parseFloat(empWorkHrs))) {
                var tothrs = HHMMToDecimal(totalhrs);
                if (tothrs > empWorkHrs) {
                    var OT = tothrs - empWorkHrs;
                    var empBreakHrs = parseFloat($("[id$=hdfEmpBreakHours]").val());
                    if (!isNaN(empBreakHrs)) {
                        var hours = Math.floor(empBreakHrs / 60);
                        var minutes = empBreakHrs % 60;
                        var empBreakHrsDecimal = HHMMToDecimal(hours + ":" + minutes);
                        OT -= empBreakHrsDecimal;
                    }

                    var otTimeDiff = 0;
                    var BOutTime1 = $("[id$=txtBOutTime1]").val();
                    var BInTime1 = $("[id$=txtBInTime1]").val();
                    var BOutTime2 = $("[id$=txtBOutTime2]").val();
                    var BInTime2 = $("[id$=txtBInTime2]").val();

                    var decBOutTime1 = parseFloat(HHMMToDecimal(BOutTime1));
                    var decBInTime1 = parseFloat(HHMMToDecimal(BInTime1));
                    var decBOutTime2 = parseFloat(HHMMToDecimal(BOutTime2));
                    var decBInTime2 = parseFloat(HHMMToDecimal(BInTime2));

                    if (decBOutTime1 > 0 && decBInTime1 > 0) {
                        otTimeDiff = parseFloat(CalculateTimeDiff(BOutTime1, BInTime1));
                        if (otTimeDiff > 0.58)
                            OT -= 0.5;
                    }
                    if (decBOutTime2 > 0 && decBInTime2 > 0) {
                        otTimeDiff = parseFloat(CalculateTimeDiff(BOutTime2, BInTime2));
                        if (otTimeDiff > 0.58)
                            OT -= 0.5;
                    }

                    $("[id$=txtNormalHours]").val(DecimalToHHMM(empWorkHrs));

                    var empHasOTFrmPunching = parseInt($("[id$=hdfEmpHasOTFromPunching]").val());
                    if (OT > 0 && !isNaN(empHasOTFrmPunching) && empHasOTFrmPunching == 1)
                        $("[id$=txtOTHours]").val(DecimalToHHMM(OT));
                    else
                        $("[id$=txtOTHours]").val(DecimalToHHMM(0));
                }
                else {
                    $("[id$=txtNormalHours]").val(totalhrs);
                    $("[id$=txtOTHours]").val(DecimalToHHMM(0));
                }
            }
        }

        function DecimalToHHMM(decVal) {
            // 10.5 to 10:30,  10.75 to 10:45
            if (decVal == "" || decVal == undefined)
                return "00:00";
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

        function GetEmpWorkHours() {
            $.ajax({
                url: "AttendanceManagement.aspx/GetEmpWorkHours",
                data: "{ 'EmployeePk': '" + $("[id$=hdfEmployee]").val() + "', 'AttenDate': '" + $("[id$=txtDate]").val() + "' }",
                dataType: "json",
                type: "POST",
                async: false,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    //empWorkHrs = data.d;
                    $("[id$=hdfEmpWorkHours]").val(data.d);
                }
            });
        }

        function GetEmpBreakHours() {
            $.ajax({
                url: "AttendanceManagement.aspx/GetEmpBreakHours",
                data: "{ 'EmployeePk': '" + $("[id$=hdfEmployee]").val() + "' }",
                dataType: "json",
                type: "POST",
                async: false,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("[id$=hdfEmpBreakHours]").val(data.d.EPD_BREAK_TIME);
                    $("[id$=hdfEmpHasOTFromPunching]").val(data.d.EPD_HAS_OT_FROM_ATT);
                }
            });
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

        function ShowHideDetImportSec(flag) {
            ///<summary>
            /// Used to Show/Hide Detail Import Section div
            ///</summary>
            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divDetImportSec]").show();
                $("[id$=imbDetShowImportSec]").hide();
                $("[id$=imbDetHideImportSec]").show();
            }
            else {
                $("[id$=divDetImportSec]").hide();
                $("[id$=imbDetShowImportSec]").show();
                $("[id$=imbDetHideImportSec]").hide();
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

        function ShowHideDtlSearch(flag) {
            $("[id$=hdfShowHideDtlSearch]").val(flag);
            var showhide = parseInt($("[id$=hdfShowHideDtlSearch]").val());
            if (showhide == 1 || flag == 1) {
                $("[id$=divDtlSearch]").show();
                $("[id$=imbShowDtlSearch]").hide();
                $("[id$=imbHideDtlSearch]").show();
                $("[id$=hdfShowHideDtlSearch]").val(flag);
            }
            else {
                $("[id$=divDtlSearch]").hide();
                $("[id$=imbShowDtlSearch]").show();
                $("[id$=imbHideDtlSearch]").hide();

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


        //For Setting  Colour for attendance if it is NOT OK
        function SetAttendanceRowColor() {
            var selectedRowColor;
            $("#<%= grdEmployeeAttendance.ClientID %> input[type=hidden][id*=hdfStatus]").each(function (index) {
                if ($(this).val() == "0") {
                    selectedRowColor = '<%= Resources.ErpRes.HrmsAttenNotOkRowColor %>';
                    $(this).closest('tr').css('background-color', selectedRowColor);

                }
                else {
                    selectedRowColor = '<%= Resources.ErpRes.HrmsAttenOkRowColor %>';
                    $(this).closest('tr').css('background-color', selectedRowColor);
                }

            });
        }
        //End   

        function ReCalculateTotal(e) {
            ///<summary>
            ///Check BackSpace/Delete and return false for all other keys
            ///</summary>

            var keyCode = e.keyCode ? e.keyCode : e.which;
            if (keyCode == 8 || keyCode == 46) {
                CalculateTotal();
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
                                    <li runat="server" id="Li1">
                                        <asp:Button runat="server" TabIndex="151" ID="Button1" CommandName="ATTIMPORT" OnClick="ActionHandler"
                                            Text="<%$resources:Export %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Export %>" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="152" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            CssClass="popupclose" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" OnClick="ActionHandler"
                                            TabIndex="153" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="154" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="155" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li runat="server" id="pnlPrint">
                                        <asp:Button runat="server" TabIndex="156" ID="btnAttImport" CommandName="ATTIMPORT"
                                            OnClick="ActionHandler" Text="<%$resources:Export %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Export %>" />
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
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
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
                                            <div class="div2col-S padgtop7 padgbotm3">
                                                <asp:Label ID="lblImprtDate" runat="server" Text="<%$ resources:DateReq%>" AssociatedControlID="txtImportDate"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtImportDate" TabIndex="70" CssClass="input-small margnbotm0"
                                                    onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="star input-medium"
                                                    SetFocusOnError="false" ValidationGroup="upload" EnableClientScript="true" runat="server"
                                                    ControlToValidate="txtImportDate" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_ImportDate %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:Label ID="lblOverwrite" runat="server" Text="<%$ resources:OverwriteExisting%>"
                                                    AssociatedControlID="lblOverwrite" CssClass="middle-lbl-small-j margnbotm0"></asp:Label>
                                                <asp:CheckBox ID="chkOverwrite" runat="server" TabIndex="71" Checked="false" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S padgtop7 padgbotm3">
                                                <asp:Label ID="lblProcessMode" runat="server" Text="<%$ resources:ProcessModeReq%>"
                                                    AssociatedControlID="ddlProcessMode"></asp:Label>
                                                <asp:DropDownList ID="ddlProcessMode" runat="server" TabIndex="72" CssClass="select-small-a">
                                                </asp:DropDownList>
                                                <asp:Label runat="server" ID="lblAttnTemplate" Text="<%$ resources:AttnTemplateReq%>"
                                                    AssociatedControlID="ddlAttnTemplate" CssClass="middle-lbl-xsmall-a3"></asp:Label>
                                                <asp:DropDownList ID="ddlAttnTemplate" runat="server" TabIndex="73" CssClass="select-small-e margnbotm5">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="VrfAttnTemplate" CssClass="star input-medium" SetFocusOnError="true"
                                                    ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="ddlAttnTemplate"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_AttnTemplate %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S padgtop7 padgbotm3">
                                                <asp:Label runat="server" ID="lblImportRemarks" Text="<%$ resources:Remarks%>" AssociatedControlID="txtImportRemarks"
                                                    CssClass="input-w12-5per"></asp:Label>
                                                <asp:TextBox ID="txtImportRemarks" runat="server" TextMode="MultiLine" CssClass="multiline-2a-line input-full"
                                                    MaxLength="500" TabIndex="74"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S padgtop7 padgbotm3 margnbotm5">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Label runat="server" ID="lblSourceFile" Text="<%$ resources: SourceFileStar %>"
                                                            AssociatedControlID="fupImport"></asp:Label>
                                                        <div class="fileupload-main">
                                                            <asp:FileUpload ID="fupImport" runat="server" TabIndex="75" CssClass="margnbotm0 margn-rgt0 upload-area3" />
                                                            <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star input-w27per" SetFocusOnError="true"
                                                                ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupImport"
                                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_File_Upload %>">
                                                            </asp:RequiredFieldValidator>
                                                        </div>
                                                        <asp:Button runat="server" ID="btnImport" CommandName="SAVEIMPORT" TabIndex="76"
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
                                        <td>
                                            <div class="div2col-S padgtop7 padgbotm3">
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
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblfilterBranch" runat="server" Text="<%$ resources:BrLoc%>" AssociatedControlID="txtFilterBranch"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterBranch" Text="" TabIndex="50" CssClass="select-half"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterBranch" Value="" runat="server" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                      </td>
                                        <td>
                                        <div class="div2col-S padgtop7">
                                          <asp:Label ID="lblFilterTrxNo" runat="server" Text="<%$ resources:TrxNo%>" AssociatedControlID="txtFilterTrxNo"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterTrxNo" Text="" TabIndex="50" CssClass="select-half"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterTrxNo" Value="" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblFilterFromDate" runat="server" Text="<%$ resources:FromDate%>"
                                                AssociatedControlID="txtFilterFromDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterFromDate" TabIndex="51" CssClass="input-small margnbotm0"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblFilterToDate" runat="server" Text="<%$ resources:ToDate%>" AssociatedControlID="txtFilterToDate"
                                                CssClass="middle-lbl-c"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterToDate" TabIndex="52" CssClass="input-small margnbotm0"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblPrcModSearch" runat="server" Text="<%$ resources:ProcessMode%>"
                                                AssociatedControlID="ddlFilterProcessMode"></asp:Label>
                                            <asp:DropDownList ID="ddlFilterProcessMode" runat="server" TabIndex="53" CssClass="select-small-a margnbotm0">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblFilterDept" runat="server" Text="<%$ resources:Dept%>" AssociatedControlID="txtFilterDept"
                                                CssClass="middle-lbl-xsmall-b"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterDept" Text="" TabIndex="54" CssClass="select-small-c margnbotm0"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterDept" Value="" runat="server" />
                                            <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search%>"
                                                ToolTip="<%$ resources:Controls,Search%>" ValidationGroup="Search" OnClick="ActionHandler"
                                                TabIndex="55" CommandName="SEARCH" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClearSearch" runat="server" Text="<%$ resources:Controls,Clear%>"
                                                ToolTip="<%$ resources:Controls,Clear%>" TabIndex="56" OnClick="ActionHandler"
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
                                                <%--   <asp:LinkButton ID="lnkbtnNo" runat="server" CssClass="text-underline" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("EAR_NO")))?Resources.ErpRes.Draft:Eval("EAR_NO")%>'
                                                  CommandArgument='<%# Eval("EAR_PK") %>'  OnClick="ActionHandler" CommandName="ATTDETAILS"   ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("EAR_NO")))?Resources.ErpRes.Draft:Eval("EAR_NO")%>'></asp:LinkButton>
                                             <asp:HiddenField runat="server" ID="hdfPk1" Value='<%# Eval("EAR_PK") %>' />--%>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("EAR_NO")))?Resources.ErpRes.Draft:Eval("EAR_NO")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("EAR_NO")))?Resources.ErpRes.Draft:Eval("EAR_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLstDate" runat="server" Text='<%# Eval("EAR_TO_DATE", Resources.Constants.HRMSDateFormatGrid)  %>'
                                                    ToolTip='<%# Eval("EAR_TO_DATE", Resources.Constants.HRMSDateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ProcessMode %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdProcessMode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EAR_ATT_MODE_TEXT"))),10) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EAR_ATT_MODE_TEXT")))  %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="false" />
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Branch/Location%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLstBranch" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EAR_BRANCH_TEXT"))),35) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EAR_BRANCH_TEXT")))  %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Department%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLstDept" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EAR_EMPDEPARTMENT_TEXT"))),35) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EAR_EMPDEPARTMENT_TEXT"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Remarks%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLstRemarks" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EAR_REMARKS"))),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EAR_REMARKS"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
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
                                            <asp:RequiredFieldValidator ID="vrfDateAddToList" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="add" EnableClientScript="true" runat="server" ControlToValidate="txtDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Date %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblhdrPrcMode" runat="server" Text="<%$ resources:ProcessModeReq%>"
                                                AssociatedControlID="ddlProcessModeHdr"></asp:Label>
                                            <asp:DropDownList ID="ddlProcessModeHdr" runat="server" TabIndex="2" CssClass="select-small-ax margnbotm0"
                                                onchange="javascript:ProcessModeChanged();">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfprocessMode" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="save" EnableClientScript="true" runat="server" ControlToValidate="ddlProcessModeHdr"
                                                Display="Static" Text="*" InitialValue="-1" ErrorMessage="<%$ resources:Err_ProcessMode %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="vrfprocessModeAdd" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="add" EnableClientScript="true" runat="server" ControlToValidate="ddlProcessModeHdr"
                                                Display="Static" Text="*" InitialValue="-1" ErrorMessage="<%$ resources:Err_ProcessMode %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label ID="lblCompany" runat="server" Text="<%$ resources:Controls,CompanyReq%>"
                                                AssociatedControlID="ddlCompany" CssClass="lbl-10-7perc"></asp:Label>
                                            <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="3" CssClass="select-small-e2">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvCompany1" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlCompany" Display="Static" Text="*" InitialValue="-1"
                                                ValidationGroup="save" ErrorMessage="<%$ resources:Err_Company %>">
                                            </asp:RequiredFieldValidator>
                                            <%--<asp:RequiredFieldValidator ID="rfvCompany" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlCompany" Display="Static" Text="*" InitialValue="-1"
                                                ValidationGroup="add" ErrorMessage="<%$ resources:Err_Company %>">
                                            </asp:RequiredFieldValidator>--%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblHdrBranchLocation" runat="server" Text="<%$ resources:Branch/Location%>"
                                                AssociatedControlID="txtHdrBranchLocation"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtHdrBranchLocation" Text="" TabIndex="4" CssClass="select-half"></asp:TextBox>
                                            <asp:HiddenField ID="hdfHdrBranchLocation" Value="" runat="server" />
                                            <asp:HiddenField ID="hdfHdrBranchLocationCode" Value="" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblHdrDepartment" runat="server" Text="<%$ resources:Department%>"
                                                AssociatedControlID="txtHdrDepartment"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtHdrDepartment" Text="" TabIndex="5" CssClass="select-half"></asp:TextBox>
                                            <asp:HiddenField ID="hdfHdrDepartment" Value="" runat="server" />
                                            <asp:HiddenField ID="hdfHdrDepartmentCode" Value="" runat="server" />
                                            <%--   <asp:RequiredFieldValidator ID="vrfHdrDepartment" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="txtHdrDepartment" Display="Dynamic" Text="*" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>"
                                                ValidationGroup="save" ErrorMessage="<%$ resources:Err_Department %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="vrfHdrDepartmentAdd" CssClass="star" SetFocusOnError="true" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>"
                                                ValidationGroup="add" EnableClientScript="true" runat="server" ControlToValidate="txtHdrDepartment"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Department %>">
                                            </asp:RequiredFieldValidator>--%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="div2col-S">
                                            <asp:Label ID="lblRemarks" runat="server" Text="<%$ resources:Remarks%>" AssociatedControlID="txtRemarks"
                                                CssClass="input-w12-5per"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRemarks" Text="" TabIndex="6" TextMode="MultiLine"
                                                CssClass="multiline-2a-line input-full" MaxLength="500" onkeydown="limitText(this,500);"
                                                onkeyup="limitText(this,500);" onpase="limitText(this,500);"></asp:TextBox></div>
                                    </td>
                                </tr>
                            </table>
                            <div id="divImportDetails">
                                <div class="search-colapse-b">
                                    <h1>
                                        <%= GetLocalResourceObject("ImportAttendance").ToString()%></h1>
                                    <asp:ImageButton runat="server" ID="imbDetShowImportSec" OnClientClick="javascript:return ShowHideDetImportSec(1);"
                                        SkinID="imbArrowInactive" ToolTip="<%$ resources:Show%>" />
                                    <asp:ImageButton runat="server" ID="imbDetHideImportSec" OnClientClick="javascript:return ShowHideDetImportSec(0);"
                                        Style="display: none" SkinID="imbArrowActive" ToolTip="<%$ resources:Hide%>" />
                                    <div class="clear">
                                    </div>
                                </div>
                                <div id="divDetImportSec" style="display: none">
                                    <table class="table-devide">
                                        <tr>
                                            <td colspan="2">
                                                <div class="divcol-S">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <div class="div2col-S padgtop7 padgbotm3 margnbotm5">
                                                                <asp:Label runat="server" ID="lblDetImport" Text="<%$ resources: SourceFileStar %>"
                                                                    AssociatedControlID="fupDetImport"></asp:Label>
                                                                <div class="fileupload-main">
                                                                    <asp:FileUpload ID="fupDetImport" runat="server" CssClass="margnbotm0 margn-rgt0 upload-area2" />
                                                                    <asp:RequiredFieldValidator ID="vrfDetImport" CssClass="star input-w27per" SetFocusOnError="true"
                                                                        ValidationGroup="detUpload" EnableClientScript="true" runat="server" ControlToValidate="fupDetImport"
                                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_File_Upload %>">
                                                                    </asp:RequiredFieldValidator>
                                                                </div>
                                                                <asp:Button runat="server" ID="btnDetImport" CommandName="IMPORTDETAILS" Text="<%$resources:Import %>"
                                                                    OnClick="ActionHandler" ToolTip="<%$resources:Import %>" SkinID="btnInner-add"
                                                                    ValidationGroup="detUpload" OnClientClick="javascript:ValidatePage('detUpload')"
                                                                    Style="margin-bottom: 3px !important;" />
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnDetImport" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("FilterSec").ToString()%>
                                </h1>
                                <asp:ImageButton runat="server" ID="imbShowDtlSearch" OnClientClick="javascript:return ShowHideDtlSearch(1);"
                                    SkinID="imbArrowInactive" ToolTip="<%$ resources:Show%>" />
                                <asp:ImageButton runat="server" ID="imbHideDtlSearch" OnClientClick="javascript:return ShowHideDtlSearch(0);"
                                    Style="display: none" SkinID="imbArrowActive" ToolTip="<%$ resources:Hide%>" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divDtlSearch" style="display: none">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblBrLocDtlSearch" runat="server" Text="<%$ resources:Branch/Location%>"
                                                    AssociatedControlID="txtBrLocDtlSearch"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtBrLocDtlSearch" Text="" TabIndex="7" CssClass="input-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfBrLocDtlSearch" Value="" runat="server" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                               <%--<asp:Label ID="lblDeptDtlSearch" runat="server" Text="<%$ resources:Dept%>" AssociatedControlID="txtDeptDtlSearch"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDeptDtlSearch" Text="" TabIndex="8" CssClass="input-small-c"></asp:TextBox>
                                                <asp:HiddenField ID="hdfDeptDtlSearch" Value="" runat="server" />--%>
                                                <asp:Label ID="lblEmpSearch" runat="server" Text="<%$ resources:EmployeeName%>" AssociatedControlID="txtEmpSearch"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtEmpSearch" Text="" TabIndex="8" CssClass="input-w32-5per"></asp:TextBox>
                                                <asp:HiddenField ID="hdfEmpSearch" Value="" runat="server" />
                                                <asp:Label ID="lblDtlSearchStatus" runat="server" Text="<%$ resources:Status%>" AssociatedControlID="ddlStatus"
                                                    CssClass="middle-lbl-xsmall-b margnbotm0"></asp:Label>
                                                <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="9" CssClass="select-small-ax margnbotm0">
                                                    <asp:ListItem Text="<%$ Resources:Report,Select %>" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Ok %>" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:NotOk %>" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:ImageButton ID="imbDtlSearch" runat="server" Text="<%$ resources:Controls,Search%>"
                                                    ToolTip="<%$ resources:Controls,Search%>" ValidationGroup="Search" OnClick="ActionHandler"
                                                    TabIndex="10" CommandName="DTLSEARCH" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                                <asp:ImageButton ID="imbDtlClear" runat="server" Text="<%$ resources:Controls,Clear%>"
                                                    ToolTip="<%$ resources:Controls,Clear%>" TabIndex="11" OnClick="ActionHandler"
                                                    CommandName="DTLCLEARSEARCH" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("Details").ToString()%></h1>
                                <asp:ImageButton runat="server" ID="imbShowFilterSec" OnClientClick="javascript:return ShowHideFilterSec(1);"
                                    SkinID="imbArrowInactive" ToolTip="<%$ resources:Show%>" />
                                <asp:ImageButton runat="server" ID="imbHideFilterSec" OnClientClick="javascript:return ShowHideFilterSec(0);"
                                    Style="display: none" SkinID="imbArrowActive" ToolTip="<%$ resources:Hide%>" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divFilterSec" style="display: none">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblBranchLocation" runat="server" Text="<%$ resources:Branch/Location%>"
                                                    AssociatedControlID="txtBranchLocation"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtBranchLocation" Text="" TabIndex="12" CssClass="input-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfBranchLocation" Value="" runat="server" />
                                                <asp:HiddenField ID="hdfBranchLocationCode" Value="" runat="server" />
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblEmployee" runat="server" Text="<%$ resources:EmployeeReq%>" AssociatedControlID="txtEmployee"></asp:Label>
                                                <asp:TextBox ID="txtEmployee" runat="server" TabIndex="14" CssClass="input-half"
                                                    MaxLength="100"> </asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfEmployee" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="add" EnableClientScript="true" runat="server" ControlToValidate="txtEmployee"
                                                    Display="Dynamic" Text="*" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>"
                                                    ErrorMessage="<%$ resources:Err_SelectEmployee %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:HiddenField ID="hdfEmployee" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdfEmpWorkHours" runat="server" Value='' />
                                                <asp:HiddenField ID="hdfEmpBreakHours" runat="server" Value='' />
                                                <asp:HiddenField ID="hdfEmpHasOTFromPunching" runat="server" Value='' />
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <%--<div class="clear">
                                                </div>
                                                <asp:Label ID="lblEmploymentType" runat="server" Text="<%$ resources:EmploymentType%>"
                                                    AssociatedControlID="ddlEmploymentType"></asp:Label>
                                                <asp:DropDownList ID="ddlEmploymentType" runat="server" TabIndex="13" CssClass="select-half-a">
                                                </asp:DropDownList>
                                                <div class="clear">
                                                </div>--%>
                                                <%--                                            <asp:DropDownList ID="ddlEmployee" runat="server" TabIndex="6" CssClass="select-half-a">
                                            </asp:DropDownList>--%>
                                                <asp:Label ID="lblDepartment" runat="server" Text="<%$ resources:Dept%>" AssociatedControlID="txtDepartment"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDepartment" Text="" TabIndex="13" CssClass="input-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfDepartment" Value="" runat="server" />
                                                <asp:HiddenField ID="hdfDepartmentCode" Value="" runat="server" />
                                                <div class="clear">
                                                </div>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lbldetRemarks" runat="server" Text="<%$ resources:Remarks%>" AssociatedControlID="txtDetRemarks"></asp:Label>
                                                <asp:TextBox ID="txtDetRemarks" runat="server" TabIndex="15" CssClass="input-half"
                                                    MaxLength="500"> </asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="employ-time">
                                    <asp:Label runat="server" ID="lblDetailDate" Text="<%$ resources:DateReq%>" AssociatedControlID="txtDetailDate"
                                        CssClass="lbl-3-2perc"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtDetailDate" CssClass="small-a margnbotm0" TabIndex="15"
                                        onkeydown="return CheckKey(event)" onpaste="return false;" MaxLength="11"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="vrftxtDetailDate" CssClass="star" SetFocusOnError="true"
                                        ValidationGroup="add" EnableClientScript="true" runat="server" ControlToValidate="txtDetailDate"
                                        Display="Static" Text="*" ErrorMessage="<%$ resources:Err_DetailDate %>">
                                    </asp:RequiredFieldValidator>
                                    <asp:Label ID="lblIn" runat="server" Text="<%$ resources:InTime%>" AssociatedControlID="txtInTime"
                                        CssClass="lbl-1-5perc margnbotm0"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtInTime" TabIndex="16" MaxLength="12" CssClass="small margnbotm0"
                                        onkeyup="CalculateTotal();"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="meeInTime" runat="server" AutoComplete="false" Mask="<%$ resources:ConfigurationsRes,HrmsTimeMask %>"
                                        MaskType="Time" TargetControlID="txtInTime">
                                    </cc1:MaskedEditExtender>
                                    <asp:RegularExpressionValidator runat="server" ID="regInTime" CssClass="star" SetFocusOnError="true"
                                        ValidationGroup="add" ControlToValidate="txtInTime" Display="Static" Text="*"
                                        ErrorMessage="<%$ resources:Err_ValidTotalHour %>" ValidationExpression="<%$ resources:ConfigurationsRes,HrmsTimeMaskValidationExp %>">
                                    </asp:RegularExpressionValidator>
                                    <asp:Label ID="lblBOut1" runat="server" Text="<%$ resources:BOutTime%>" AssociatedControlID="txtBOutTime1"
                                        CssClass="lbl-2-5perc margnbotm0"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtBOutTime1" TabIndex="17" MaxLength="12" CssClass="small margnbotm0"
                                        onkeyup="CalculateTotal();"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="meeBOutTime1" runat="server" AutoComplete="false" Mask="<%$ resources:ConfigurationsRes,HrmsTimeMask %>"
                                        MaskType="Time" TargetControlID="txtBOutTime1">
                                    </cc1:MaskedEditExtender>
                                    <asp:RegularExpressionValidator runat="server" ID="regBOutTime1" CssClass="star"
                                        SetFocusOnError="true" ValidationGroup="add" ControlToValidate="txtBOutTime1"
                                        Display="Static" Text="*" ErrorMessage="<%$ resources:Err_ValidTotalHour %>"
                                        ValidationExpression="<%$ resources:ConfigurationsRes,HrmsTimeMaskValidationExp %>">
                                    </asp:RegularExpressionValidator>
                                    <asp:Label ID="lblBIn1" runat="server" Text="<%$ resources:BInTime%>" AssociatedControlID="txtBInTime1"
                                        CssClass="lbl-2-5perc margnbotm0"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtBInTime1" TabIndex="18" MaxLength="12" CssClass="small margnbotm0"
                                        onkeyup="CalculateTotal();"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="meeBInTime1" runat="server" AutoComplete="false" Mask="<%$ resources:ConfigurationsRes,HrmsTimeMask %>"
                                        MaskType="Time" TargetControlID="txtBInTime1">
                                    </cc1:MaskedEditExtender>
                                    <asp:RegularExpressionValidator runat="server" ID="regBInTime1" CssClass="star" SetFocusOnError="true"
                                        ValidationGroup="add" ControlToValidate="txtBInTime1" Display="Static" Text="*"
                                        ErrorMessage="<%$ resources:Err_ValidTotalHour %>" ValidationExpression="<%$ resources:ConfigurationsRes,HrmsTimeMaskValidationExp %>">
                                    </asp:RegularExpressionValidator>
                                    <asp:Label ID="lblBOut2" runat="server" Text="<%$ resources:BOutTime%>" AssociatedControlID="txtBOutTime2"
                                        CssClass="lbl-3-3perc margnbotm0"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtBOutTime2" TabIndex="19" MaxLength="12" CssClass="small margnbotm0"
                                        onkeyup="CalculateTotal();"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="meeBOutTime2" runat="server" AutoComplete="false" Mask="<%$ resources:ConfigurationsRes,HrmsTimeMask %>"
                                        MaskType="Time" TargetControlID="txtBOutTime2">
                                    </cc1:MaskedEditExtender>
                                    <asp:RegularExpressionValidator runat="server" ID="regBOutTime2" CssClass="star"
                                        SetFocusOnError="true" ValidationGroup="add" ControlToValidate="txtBOutTime2"
                                        Display="Static" Text="*" ErrorMessage="<%$ resources:Err_ValidTotalHour %>"
                                        ValidationExpression="<%$ resources:ConfigurationsRes,HrmsTimeMaskValidationExp %>">
                                    </asp:RegularExpressionValidator>
                                    <asp:Label ID="lblBIn2" runat="server" Text="<%$ resources:BInTime%>" AssociatedControlID="txtBInTime2"
                                        CssClass="lbl-2-5perc margnbotm0"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtBInTime2" TabIndex="20" MaxLength="12" CssClass="small margnbotm0"
                                        onkeyup="CalculateTotal();"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="meeBInTime2" runat="server" AutoComplete="false" Mask="<%$ resources:ConfigurationsRes,HrmsTimeMask %>"
                                        MaskType="Time" TargetControlID="txtBInTime2">
                                    </cc1:MaskedEditExtender>
                                    <asp:RegularExpressionValidator runat="server" ID="regBInTime2" CssClass="star" SetFocusOnError="true"
                                        ValidationGroup="add" ControlToValidate="txtBInTime2" Display="Static" Text="*"
                                        ErrorMessage="<%$ resources:Err_ValidTotalHour %>" ValidationExpression="<%$ resources:ConfigurationsRes,HrmsTimeMaskValidationExp %>">
                                    </asp:RegularExpressionValidator>
                                    <asp:Label ID="lblOut" runat="server" Text="<%$ resources:OutTime%>" AssociatedControlID="txtOutTime"
                                        CssClass="lbl-2-5perc margnbotm0"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtOutTime" TabIndex="21" MaxLength="12" CssClass="small margnbotm0"
                                        onkeyup="CalculateTotal();"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="meeOutTime" runat="server" AutoComplete="false" Mask="<%$ resources:ConfigurationsRes,HrmsTimeMask %>"
                                        MaskType="Time" TargetControlID="txtOutTime">
                                    </cc1:MaskedEditExtender>
                                    <asp:RegularExpressionValidator runat="server" ID="regOutTime" CssClass="star" SetFocusOnError="true"
                                        ValidationGroup="add" ControlToValidate="txtOutTime" Display="Static" Text="*"
                                        ErrorMessage="<%$ resources:Err_ValidTotalHour %>" ValidationExpression="<%$ resources:ConfigurationsRes,HrmsTimeMaskValidationExp %>">
                                    </asp:RegularExpressionValidator>
                                    <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:TotalHours%>" AssociatedControlID="txtTotalHours"
                                        CssClass="lbl-2-5perc margnbotm0"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtTotalHours" CssClass="small margnbotm0" ValidationGroup="save"
                                        TabIndex="22" onkeyup="CalculateNormalAndOT();" />
                                    <cc1:MaskedEditExtender ID="meeTotalHours" runat="server" AutoComplete="false" Mask="<%$ resources:ConfigurationsRes,HrmsTimeMask %>"
                                        MaskType="Time" TargetControlID="txtTotalHours">
                                    </cc1:MaskedEditExtender>
                                    <asp:RegularExpressionValidator runat="server" ID="regTotalHours" CssClass="star"
                                        SetFocusOnError="true" ValidationGroup="add" ControlToValidate="txtTotalHours"
                                        Display="Static" Text="*" ErrorMessage="<%$ resources:Err_ValidTotalHour %>"
                                        ValidationExpression="<%$ resources:ConfigurationsRes,HrmsTimeMaskValidationExp %>">
                                    </asp:RegularExpressionValidator>
                                    <asp:HiddenField ID="hdfTotalHours" runat="server" Value='' />
                                    <asp:Label ID="lblNormal" runat="server" Text="<%$ resources:NormalHrs%>" AssociatedControlID="txtNormalHours"
                                        CssClass="lbl-3-3perc margnbotm0"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtNormalHours" TabIndex="23" MaxLength="12" CssClass="small margnbotm0"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="meeNH" runat="server" AutoComplete="false" Mask="<%$ resources:ConfigurationsRes,HrmsTimeMask %>"
                                        MaskType="Time" TargetControlID="txtNormalHours">
                                    </cc1:MaskedEditExtender>
                                    <asp:RegularExpressionValidator runat="server" ID="regNH" CssClass="star" SetFocusOnError="true"
                                        ValidationGroup="add" ControlToValidate="txtNormalHours" Display="Static" Text="*"
                                        ErrorMessage="<%$ resources:Err_ValidOTHour %>" ValidationExpression="<%$ resources:ConfigurationsRes,HrmsTimeMaskValidationExp %>">
                                    </asp:RegularExpressionValidator>
                                    <asp:Label ID="lblOT" runat="server" Text="<%$ resources:OTHours%>" AssociatedControlID="txtOTHours"
                                        CssClass="lbl-2-5perc margnbotm0"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtOTHours" CssClass="small margnbotm0" ValidationGroup="save"
                                        TabIndex="24" />
                                    <cc1:MaskedEditExtender ID="meeOTHours" runat="server" AutoComplete="false" Mask="<%$ resources:ConfigurationsRes,HrmsTimeMask %>"
                                        MaskType="Time" TargetControlID="txtOTHours">
                                    </cc1:MaskedEditExtender>
                                    <asp:RegularExpressionValidator runat="server" ID="regOTHours" CssClass="star" SetFocusOnError="true"
                                        ValidationGroup="add" ControlToValidate="txtOTHours" Display="Static" Text="*"
                                        ErrorMessage="<%$ resources:Err_ValidOTHour %>" ValidationExpression="<%$ resources:ConfigurationsRes,HrmsTimeMaskValidationExp %>">
                                    </asp:RegularExpressionValidator>
                                    <asp:HiddenField ID="hdfOTHours" runat="server" Value='<%# Eval("OTDtHrs") %>' />
                                    <asp:Label ID="lblLteHrs" runat="server" Text="<%$ resources:Late%>" AssociatedControlID="txtLateHrs"
                                        CssClass="lbl-2-5perc margnbotm0"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtLateHrs" CssClass="small margnbotm0" ValidationGroup="save"
                                        TabIndex="25" />
                                    <cc1:MaskedEditExtender ID="meeLateHrs" runat="server" AutoComplete="false" Mask="<%$ resources:ConfigurationsRes,HrmsTimeMask %>"
                                        MaskType="Time" TargetControlID="txtLateHrs">
                                    </cc1:MaskedEditExtender>
                                    <asp:RegularExpressionValidator runat="server" ID="regLateHrs" CssClass="star" SetFocusOnError="true"
                                        ValidationGroup="add" ControlToValidate="txtLateHrs" Display="Static" Text="*"
                                        ErrorMessage="<%$ resources:Err_ValidLateHour %>" ValidationExpression="<%$ resources:ConfigurationsRes,HrmsTimeMaskValidationExp %>">
                                    </asp:RegularExpressionValidator>
                                    <asp:Label ID="lblStatus" runat="server" Text="<%$ resources:Status%>" AssociatedControlID="lblStatus"
                                        CssClass="lbl-3-2perc margnbotm0"></asp:Label>
                                    <asp:CheckBox ID="chkStatus" runat="server" TabIndex="26" Checked="true" />
                                    <asp:ImageButton ID="imbAdd" runat="server" OnClick="ActionHandler" CommandName="ADD"
                                        ToolTip="Add" OnClientClick="javascript:ValidatePage('add')" ValidationGroup="add"
                                        TabIndex="27" SkinID="plus" CssClass="margntop2 margnbotm0 margn-rgt2" />
                                    <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear%>"
                                        ToolTip="<%$ resources:Controls,Clear%>" TabIndex="28" OnClick="ActionHandler"
                                        CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0 margn-rgt2" />
                                    <div class="clear">
                                    </div>
                                </div>
                            </div>
                            <div class="gridwrap">
                                <%--class="gridwrap scroll-container2 employ-grid"--%>
                                <asp:GridView runat="server" ID="grdEmployeeAttendance" AutoGenerateColumns="false"
                                    AllowSorting="true" EmptyDataRowStyle-CssClass="emptytable" Width="100%" OnSorting="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Employee %>" SortExpression="EmpCode">
                                            <ItemTemplate>
                                                <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval("EAT_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfSlNo" Value='<%# Eval("SlNo") %>' />
                                                <asp:HiddenField runat="server" ID="hdfEAT_PK" Value='<%# Eval("Pk") %>' />
                                                <asp:HiddenField ID="hdfEmployeePk" runat="server" Value='<%# Eval("EmpPk") %>' />
                                                <asp:HiddenField ID="hdfWorkHours" runat="server" Value='<%# Eval("WorkingHrs") %>' />
                                                <asp:HiddenField ID="hdfIsUpdate" runat="server" Value='<%# Eval("UpdateFlag") %>' />
                                                <asp:HiddenField ID="hdfHasOTFromPunching" runat="server" Value='<%# Eval("HasOTFromPunching") %>' />
                                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( HttpUtility.HtmlDecode(Convert.ToString(Eval("EmpCode"))) ,30) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("EmpCode")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Location %>" SortExpression="LocationCode">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdLocation" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Convert.ToString(Eval("LocationCode"))) ,6) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("Location")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:Dept %>" SortExpression="EmpDepartmentCode">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDept" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Convert.ToString(Eval("EmpDepartmentCode"))) ,13) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("EmpDepartment")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Remarks %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrdRemarks" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Convert.ToString(Eval("Remarks"))) ,8) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("Remarks")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdDtlDate" runat="server" Text='<%# Eval("DateDt", Resources.Constants.HRMSDateFormatGrid)  %>'
                                                    ToolTip='<%# Eval("DateDt", Resources.Constants.HRMSDateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InTime %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInTime" runat="server" Text='<%# GetTimeSpan(Eval("InDt")) %>'
                                                    ToolTip='<%# GetTimeSpan(Eval("InDt")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BOutTime %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBOutTime1" runat="server" Text='<%# GetTimeSpan(Eval("BOut1Dt")) %>'
                                                    ToolTip='<%# GetTimeSpan(Eval("BOut1Dt")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BInTime %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBInTime1" runat="server" Text='<%# GetTimeSpan(Eval("BIn1Dt")) %>'
                                                    ToolTip='<%# GetTimeSpan(Eval("BIn1Dt")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Brk1 %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdBrk1" runat="server" Text='<%# Eval("EAT_BREAK1") %>' ToolTip='<%# Eval("EAT_BREAK1") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BOutTime %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBOutTime2" runat="server" Text='<%# GetTimeSpan(Eval("BOut2Dt")) %>'
                                                    ToolTip='<%# GetTimeSpan(Eval("BOut2Dt")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BInTime %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBInTime2" runat="server" Text='<%# GetTimeSpan(Eval("BIn2Dt")) %>'
                                                    ToolTip='<%# GetTimeSpan(Eval("BIn2Dt")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Brk2 %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdBrk2" runat="server" Text='<%# Eval("EAT_BREAK2") %>' ToolTip='<%# Eval("EAT_BREAK2") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:OutTime %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOutTime" runat="server" Text='<%# GetTimeSpan(Eval("OutDt")) %>'
                                                    ToolTip='<%# GetTimeSpan(Eval("OutDt")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TotalHours %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalHrs" runat="server" Text='<%# GetTime(Convert.ToString(Eval("TotalHrs"))) %>'
                                                    ToolTip='<%# GetTime(Convert.ToString(Eval("TotalHrs"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:NormalHrs %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNormalHrs" runat="server" Text='<%# GetTime(Convert.ToString(Eval("NormalHrs"))) %>'
                                                    ToolTip='<%# GetTime(Convert.ToString(Eval("NormalHrs"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:OTHours %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOTHrs" runat="server" Text='<%# GetTime(Convert.ToString(Eval("OTDtHrs"))) %>'
                                                    ToolTip='<%# GetTime(Convert.ToString(Eval("OTDtHrs"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Late %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLateHrs" runat="server" Text='<%# GetTime(Convert.ToString(Eval("ShortHrs"))) %>'
                                                    ToolTip='<%# GetTime(Convert.ToString(Eval("ShortHrs"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" Wrap="false" />
                                        </asp:TemplateField>                                       
                                        <asp:TemplateField HeaderText="<%$ resources:Status %>" SortExpression="EAT_STATUS">
                                            <ItemTemplate>
                                                <%--<asp:CheckBox ID="chkgrdStatus" Text="" runat="server" Checked='<%# (Convert.ToInt32(Eval("EAT_STATUS")) > 0) ? true : false %>'
                                                    TabIndex="21" onclick="SetAttendanceRowColor();" />--%>
                                                <asp:ImageButton ID="imbActive" runat="server" SkinID="btninactive" Visible='<%# (Convert.ToInt32(Eval("EAT_STATUS")) <= 0) ? true : false %>'
                                                    CommandName="ACTIVATE" ToolTip="<%$ resources:NotOk %>" CssClass="Active removedownloadClass"
                                                    Enabled="false" TabIndex="21" />
                                                <asp:ImageButton ID="imbInActive" runat="server" SkinID="btnactive" Visible='<%# (Convert.ToInt32(Eval("EAT_STATUS")) > 0) ? true : false %>'
                                                    CommandName="DEACTIVATE" ToolTip="<%$ resources:Ok %>" CssClass="Active removedownloadClass"
                                                    Enabled="false" TabIndex="21" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_edit" runat="server" ID="imbEditDetails"
                                                    TabIndex="29" SkinID="imbeditgrid" EnableViewState="false" CommandName="GRIDEDIT"
                                                    OnClick="ActionHandler" ToolTip="<%$ resources:Controls,Edit %>" />
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbViewAttendance"
                                                    ToolTip="<%$ resources:Controls,View %>" SkinID="btnview" CommandName="VIEWATTENDANCEDETAILS"
                                                    OnClick="ActionHandler" TabIndex="29" Visible='<%# (Convert.ToInt32(Eval("Pk")) > 0 ) ? true : false %>' />
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbDeleteAttendance"
                                                    ToolTip="<%$ resources:Controls,Delete %>" SkinID="imbdeletegrid" CommandName="GRIDDELETE"
                                                    OnClick="ActionHandler" OnClientClick="return ShowDeleteConfirm(this);" TabIndex="29"
                                                    Visible='<%# (Convert.ToInt32(Eval("IsSalaryPrcd")) == 0) ? true : false %>' />
                                                <%-- Visible='<%# (Convert.ToInt32(Eval("Pk")) > 0 && Convert.ToInt32(Eval("IsSalaryPrcd")) == 0) ? true : false %>'--%>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" HorizontalAlign="Right" Width="4%" />
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
                <div class="content-wrapper">
                    <asp:Label runat="server" ID="lblEmpNameCode"></asp:Label>
                    <table class="table-devide" style="display: none;">
                        <tr>
                            <td>
                                <div>
                                    <asp:Label ID="lblTxtBreak" runat="server" Text="<%$ resources:Break1%>" AssociatedControlID="lblBreak1"></asp:Label>
                                    <asp:Label ID="lblBreak1" runat="server" Text="" AssociatedControlID="lblBreak1"
                                        Font-Bold="true" CssClass="input-w10per"></asp:Label>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <asp:Label ID="lnlTxtBreak2" runat="server" Text="<%$ resources:Break2%>" AssociatedControlID="lblBreak2"
                                        CssClass="input-small-bn"></asp:Label>
                                    <asp:Label ID="lblBreak2" runat="server" Text="" AssociatedControlID="lblBreak2"
                                        Font-Bold="true" CssClass="input-w10per"></asp:Label>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <asp:GridView runat="server" ID="grdEmpAttndance" Width="100%" AutoGenerateColumns="false"
                        EmptyDataRowStyle-CssClass="emptytable">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ resources:Time %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblTime" runat="server" Text='<%# Eval("EAL_TIME", Resources.Constants.DateTimeFormatGrid) %>'
                                        ToolTip='<%# Eval("EAL_TIME", Resources.Constants.DateTimeFormatGrid) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="60%" />
                                <HeaderStyle Width="60%" />
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
            </div>
            <div id="diverror" style="display: none">
                <%--Use this label to bind the server errors--%>
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="save" runat="server" />
                <asp:ValidationSummary ID="vsUpload" ValidationGroup="upload" runat="server" />
                <asp:ValidationSummary ID="vsAdd" ValidationGroup="add" runat="server" />
                <asp:ValidationSummary ID="vsDetImport" ValidationGroup="detUpload" runat="server" />
            </div>
            <asp:HiddenField runat="server" ID="hdfTemp" />
            <asp:HiddenField runat="server" ID="hdfAttnEntryMode" Value="2" />
            <asp:HiddenField runat="server" ID="hdfShowHideFilterSec" Value="0" />
            <asp:HiddenField runat="server" ID="hdfTotalHrsCalcMode" Value="2" />
            <asp:HiddenField runat="server" ID="hdfCurrPk" Value="0" />
            <asp:HiddenField runat="server" ID="hdfShowHideDtlSearch" Value="0" />
            <asp:HiddenField runat="server" ID="hdfDetailsRowCount" Value="0" />
            <asp:HiddenField runat="server" ID="hdfShiftStartTime" Value="" />
            <asp:HiddenField runat="server" ID="hdfAdditionHrs" Value="" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
