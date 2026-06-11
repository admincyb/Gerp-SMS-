<%@ Page Title="<%$ resources:HRMS-Employee-Basic-Information%>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="EmployeeBasicInfo.aspx.cs"
    ValidateRequest="false" Inherits="HRMS.Employees.EmployeeBasicInfo" %>

<%@ Register Src="UserControls/GtiTabControl.ascx" TagName="GtiTabControl" TagPrefix="ucGtiTab" %>
<%@ Register Src="UserControls/EmpBasicInfoControl.ascx" TagName="EmpBasicInfoControl"
    TagPrefix="ucBasicHdr" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Src="UserControls/EmpDocument.ascx" TagName="EmpDocument" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE9" />
    <style type="text/css">
        .status-drpdwn
        {
            width: 233px !important;
        }
        
        .branch-input
        {
            width: 228px !important;
        }
        .search-colapse-b
        {
            margin-bottom: 8px;
            height: 20px;
            background: #f5f5f5;
        }
        
        /*Notify*/
        .notify
        {
            background: #eff0f1;
            border: 1px solid #dcdcdc;
            padding: 3px;
            margin: 10px;
        }
        
        /* Employee Status - Popup  
     .status-popup{ width:50%; float:left;}   
     .status-popup .div-lft{ padding-left:56px;} 
     .status-popup .div-lft .margnlft{ margin-left:17px!important;}
     .status-popup .div-rgt{ padding-left:78px;}      
     .status-popup .div-rgt .margnlft{ margin-left:31px!important;}*/
        
        
        /* Status Details - popup*/
        .popup-headr
        {
            width: 50%;
            float: left;
            line-height: 18px;
            font-size: 10px;
        }
        .popup-headr span
        {
            font-weight: bold;
            background: none;
            border: 0 none;
            padding: 0; /*width: 57%;*/
        }
        .popup-headr label
        {
            text-align: right;
            width: 45% !important;
        }
        .popup-headr span, .popup-headr label
        {
            margin-bottom: 0 !important;
        }
    </style>
    <script type="text/javascript">
        function PreviewImageBeforeUpload(Imagepath, path) {
            if (Imagepath) {
                if (Imagepath.files && Imagepath.files[0]) {
                    var Filerdr = new FileReader();
                    Filerdr.onload = function (e) {
                        $("[id$=hdfFileuRL]").val(e.target.result);
                        $("[id$=imgEmployee]").attr('src', e.target.result);

                    }
                    Filerdr.readAsDataURL(Imagepath.files[0]);
                }
            }
            else {
                if (path) {
                    $("[id$=imgEmployee]").attr('src', path);
                }
            }

        }
        function AfterDateSelect(controlID) {
            if (controlID == "txtDOB") {
                $("[id$=spnAge]").html(CalculateAge($("[id$=txtDOB]").val()));
            }
            if (controlID == "txtDOJ") {
                $("[id$=spnDOJ]").html(CalculateDOJ($("[id$=txtDOJ]").val()));

            }
            if (controlID == "txtConfirmedOn") {
                $("[id$=SpnConfirmendOn]").html(CalculateDOJ($("[id$=txtConfirmedOn]").val()));
            }
        }
        function GetMonthVal(month) {
            var monthNamesShort = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
            for (var mnth = 0; mnth < 12; mnth++)
                if (monthNamesShort[mnth] == month) break;
            return mnth++;
        }
        function CalculateAge(dateString) {
            var now = new Date();
            var today = new Date(now.getYear(), now.getMonth(), now.getDate());
            var yearNow = (new Date).getFullYear();
            var monthNow = now.getMonth();
            var dateNow = now.getDate();
            var dateArray = dateString.split('-');
            var yearDob = parseInt(dateArray[2]);
            var monthDob = GetMonthVal(dateArray[1]);
            var dateDob = parseInt(dateArray[0]);
            var age = {};
            var ageString = "";
            var yearString = "";
            var monthString = "";
            var dayString = "";
            yearAge = yearNow - yearDob;
            //yearAge = yearNow - yearDob;
            //Same Year
            if (yearAge == -1) {
                yearAge = 0;
            }
            //Same Month and different year
            //            if (monthNow == monthDob && yearNow != yearDob) {
            //                if (dateNow >= dateDob)
            //                    yearAge = yearAge + 1;
            //            }
            if (monthNow >= monthDob)
                var monthAge = monthNow - monthDob;
            else {
                yearAge--;
                var monthAge = 12 + monthNow - monthDob;
            }
            if (monthNow == monthDob && dateDob > dateNow) {
                yearAge--;
            }

            if (dateNow >= dateDob)
                var dateAge = dateNow - dateDob;
            else {
                monthAge--;
                var dateAge = 31 + dateNow - dateDob;
                if (monthAge < 0) {
                    monthAge = 11;
                    // yearAge--;
                }
            }
            age = {
                years: yearAge,
                months: monthAge,
                days: dateAge
            };
            if (age.years > 1) yearString = '<%= GetLocalResourceObject("YearFormatAge").ToString() %>';
            else yearString = '<%= GetLocalResourceObject("YearFormatAge").ToString() %>';
            if (age.months > 1) monthString = '<%= GetLocalResourceObject("MonthFormatAge").ToString() %>';
            else monthString = '<%= GetLocalResourceObject("MonthFormatAge").ToString() %>';
            if (age.days > 1) dayString = '<%= GetLocalResourceObject("DaysFormatAge").ToString() %>';
            else dayString = '<%= GetLocalResourceObject("DaysFormatAge").ToString() %>';
            if ((age.years > 0) && (age.months > 0))
                ageString = age.years + yearString + " " + age.months + monthString;
            //            else if ((age.years > 0) && (age.months == 0))
            //                ageString = age.years + yearString + " and  1"  + monthString + " old.";
            else if ((age.years == 0) && (age.months == 0))
                ageString = age.days + dayString;
            else if ((age.years > 0) && (age.months == 0))
                ageString = age.years + yearString;
            else if ((age.years > 0) && (age.months > 0))
                ageString = age.years + yearString + " " + age.months + monthString;
            else if ((age.years == 0) && (age.months > 0) && (age.days > 0))
                ageString = age.months + monthString + " " + age.days + dayString;
            else if ((age.years > 0) && (age.months == 0))
                ageString = age.years + yearString;
            else if ((age.years == 0) && (age.months > 0))
                ageString = age.months + monthString;
            else ageString = "Oops! Could not calculate age!";
            return ageString;
        }
        function CalculateDOJ(dateString) {
            var now = new Date();
            var today = new Date(now.getYear(), now.getMonth(), now.getDate());
            var yearNow = (new Date).getFullYear();
            var monthNow = now.getMonth();
            var dateNow = now.getDate();
            var dateArray = dateString.split('-');
            var yearDob = parseInt(dateArray[2]);
            var monthDob = GetMonthVal(dateArray[1]);
            var dateDob = parseInt(dateArray[0]);
            var age = {};
            var ageString = "";
            var yearString = "";
            var monthString = "";
            var dayString = "";
            yearAge = yearNow - yearDob;
            //yearAge = yearNow - yearDob;
            //Same Year
            if (yearAge == -1) {
                yearAge = 0;
            }
            //Same Month and different year
            //            if (monthNow == monthDob && yearNow != yearDob) {
            //                if (dateNow >= dateDob)
            //                    yearAge = yearAge + 1;
            //            }
            if (monthNow >= monthDob)
                var monthAge = monthNow - monthDob;
            else {
                yearAge--;
                var monthAge = 12 + monthNow - monthDob;
            }
            if (monthNow == monthDob && dateDob > dateNow) {
                yearAge--;
            }

            if (dateNow >= dateDob)
                var dateAge = dateNow - dateDob;
            else {
                monthAge--;
                var dateAge = 31 + dateNow - dateDob;
                if (monthAge < 0) {
                    monthAge = 11;
                    // yearAge--;
                }
            }
            age = {
                years: yearAge,
                months: monthAge,
                days: dateAge
            };


            if (age.years > 1) yearString = " years";
            else yearString = " year";
            if (age.months > 1) monthString = " months";
            else monthString = " month";
            if (age.days > 1) dayString = " days";
            else dayString = " day";
            if ((age.years > 0) && (age.months > 0))
                ageString = age.years + yearString + " and " + age.months + monthString;
            //            else if ((age.years > 0) && (age.months == 0))
            //                ageString = age.years + yearString + " and  1"  + monthString + " old.";
            else if ((age.years == 0) && (age.months == 0))
                ageString = "Only " + age.days + dayString;
            else if ((age.years > 0) && (age.months == 0))
                ageString = age.years + yearString;
            else if ((age.years > 0) && (age.months > 0))
                ageString = age.years + yearString + " and " + age.months + monthString;
            else if ((age.years == 0) && (age.months > 0) && (age.days > 0))
                ageString = age.months + monthString + " and " + age.days + dayString;
            else if ((age.years > 0) && (age.months == 0))
                ageString = age.years + yearString;
            else if ((age.years == 0) && (age.months > 0))
                ageString = age.months + monthString;
            else ageString = "";
            return ageString;



        }
        // Copy Prmanent Communication Address
        function CopyAdd() {
            //            var cb1 = document.getElementById('ctl00_MainContent_chkSameAsAbove');
            //            var a1 = document.getElementById('ctl00_MainContent_txtPermanentAddress');
            //            var a2 = document.getElementById('ctl00_MainContent_txtCommunicationAddress');
            //            var b1 = document.getElementById('ctl00_MainContent_txtCity');
            //            var b2 = document.getElementById('ctl00_MainContent_txtCity1');
            //            var c1 = document.getElementById('ctl00_MainContent_txtState');
            //            var c2 = document.getElementById('ctl00_MainContent_txtState1');
            //            var d1 = document.getElementById('ctl00_MainContent_txtCountry');
            //            //var h1 = document.getElementById('ctl00$MainContent$hdfCountry');
            //            //var h2 = document.getElementById('ctl00$MainContent$hdfCountry1');
            //            var d2 = document.getElementById('ctl00_MainContent_txtCountry1');
            //            var e1 = document.getElementById('ctl00_MainContent_txtZipCode');
            //            var e2 = document.getElementById('ctl00_MainContent_txtZipCode1');
            //            var f1 = document.getElementById('ctl00_MainContent_txtPhone');
            //            var f2 = document.getElementById('ctl00_MainContent_txtPhone1');
            //            var g1 = document.getElementById('ctl00_MainContent_txtDistrict1');
            //            var g2 = document.getElementById('ctl00_MainContent_txtDistrict2');
            var cb1 = document.getElementById('<%=chkSameAsAbove.ClientID%>');
            var a1 = document.getElementById('<%=txtPermanentAddress.ClientID%>');
            var a2 = document.getElementById('<%=txtCommunicationAddress.ClientID%>');
            var b1 = document.getElementById('<%=txtCity.ClientID%>');
            var b2 = document.getElementById('<%=txtCity1.ClientID%>');
            var c1 = document.getElementById('<%=txtState.ClientID%>');
            var c2 = document.getElementById('<%=txtState1.ClientID%>');
            var d1 = document.getElementById('<%=txtCountry.ClientID%>');
            //var h1 = document.getElementById('ctl00$MainContent$hdfCountry');
            //var h2 = document.getElementById('ctl00$MainContent$hdfCountry1');
            var d2 = document.getElementById('<%=txtCountry1.ClientID%>');
            var e1 = document.getElementById('<%=txtZipCode.ClientID%>');
            var e2 = document.getElementById('<%=txtZipCode1.ClientID%>');
            var f1 = document.getElementById('<%=txtPhone.ClientID%>');
            var f2 = document.getElementById('<%=txtPhone1.ClientID%>');
            var g1 = document.getElementById('<%=txtDistrict1.ClientID%>');
            var g2 = document.getElementById('<%=txtDistrict2.ClientID%>');
            if (cb1.checked) {
                a2.value = a1.value;
                b2.value = b1.value;
                c2.value = c1.value;
                e2.value = e1.value;
                f2.value = f1.value;
                g2.value = g1.value;
                d2.value = d1.value;
            } else {
                a2.value = '';
                b2.value = '';
                c2.value = '';
                e2.value = '';
                f2.value = '';
                g2.value = '';
                d2.value = '';
            }
        }
        function CopyCountry() {
            var d1 = document.getElementById('<%=txtCountry.ClientID%>');
            var d2 = document.getElementById('<%=txtCountryOfBirth.ClientID%>');
            d2.value = d1.value;
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
                ShowErrorMessage($("#diverror").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
        //Validate number only
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
        //Validate number and Special Characters (+,-()/*.)
        function isNumberWithSpclCharacters(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && charCode > 32 && (charCode < 48 || charCode > 57) && (charCode < 40 || charCode > 47)) {
                return false;
            }
            return true;
        }

        function numbersonlywithCopypaste(evt) {

            evt = (evt) ? evt : window.event;
            var charCode = evt.keyCode;

            if (evt.ctrlKey == true) {
                if (charCode == 65 || charCode == 67 || charCode == 86 || charCode == 88) {
                    return true;
                }
            }
            if (
                charCode == 8 || //backspace
                charCode == 9 || //tab key
                charCode == 46 || //delete
                charCode == 13)   //enter key
            {
                return true;
            }
            else if (charCode >= 37 && charCode <= 40) //arrow keys
            {
                return true;
            }
            else if (charCode >= 48 && charCode <= 57) //0-9 on key pad
            {
                if (evt.shiftKey == true)
                    return false;
                return true;
            }
            if (evt.shiftKey == true) {
                if (charCode == 43) {
                    return true;
                }
            }
            else if (charCode >= 96 && charCode <= 105) //0-9 on num pad
            {
                if (evt.shiftKey == true)
                    return false;
                return true;
            }
            else
                return false;
        }
        //Validate Alphabets only
        function isAlphabet(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && charCode > 32 && (charCode < 65 || charCode > 90) && (charCode < 97 || charCode > 122)) {
                return false;
            }
            return true;
        }
        /// AutoComplete textbox
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtNationality", url + "?OpParam=CNT_NATIONALITY", "hdfNationalityPK", true, true, "NATIONALITY");
            GrandScriptUtils.MakeAutoCompleteDDL("txtReportTo", url + "?EmpCategory=2", "hdfReportTo", true, true, "EMPLOYEEAUTOCOMPLETE",false,false,false,false,4);
            GrandScriptUtils.MakeAutoCompleteDDL("txtReportToAdm", url + "?EmpCategory=2", "hdfReportToAdm", true, true, "EMPLOYEEAUTOCOMPLETE", false, false, false, false, 4);
            GrandScriptUtils.MakeAutoCompleteDDL("txtCountryOfBirth", url, "hdfCountryOfBirth", true, true, "COUNTRYBIRTH");
            GrandScriptUtils.MakeAutoCompleteDDL("txtProfession", url, "hdfProfession", true, true, "PROFESSION");

            GrandScriptUtils.MakeAutoCompleteDDL("txtDepartmentPopup", url, "hdfDepartmentPopup", true, true, "DEPARTMENTAUTOCOMPLETE");
            var Desig = '<%= GetGlobalResourceObject("ConfigurationsRes", "HrmsInfoHiding").ToString() %>'
                if (Desig == 1) {
               if ($("[id$=hdfEnableBranchLocationAutoComplete]").val() != '0') {
                GrandScriptUtils.MakeAutoCompleteDDL("txtBranchLocation", url, "hdfBranchLocation", true, true, "BRANCHLOCATION");
            }
                    }
                else
                {
                GrandScriptUtils.MakeAutoCompleteDDL("txtBranchLocation", url, "hdfBranchLocation", true, true, "BRANCHLOCATION");
                }
           var Dept = '<%= GetGlobalResourceObject("ConfigurationsRes", "HrmsInfoHiding").ToString() %>'
                if (Dept == 1) {
            if ($("[id$=hdfEnableDepartmentAutoComplete]").val() != '0') {

                GrandScriptUtils.MakeAutoCompleteDDL("txtDepartment", url, "hdfDepartment", true, true, "DEPARTMENTAUTOCOMPLETE");
            }
            }
            else{
            GrandScriptUtils.MakeAutoCompleteDDL("txtDepartment", url, "hdfDepartment", true, true, "DEPARTMENTAUTOCOMPLETE");
            }

            GrandScriptUtils.MakeAutoCompleteDDL("txtTeam", url, "hdfTeam", true, true, "TEAM");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCostCenter", url, "hdfCostCenter", true, true, "COSTCENTERWITHOUTGRP");           
            GrandScriptUtils.MakeAutoCompleteDDL("txtDesignationPopup", url, "hdfDesignationPopup", true, true, "DESIGNATION");
            GrandScriptUtils.MakeAutoCompleteDDL("txtBranchLocationPopup", url, "hdfBranchLocationPopup", true, true, "BRANCHLOCATION");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCountry", url, "hdfCountry", true, true, "COUNTRYFIRST");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCountry1", url, "hdfCountry1", true, true, "COUNTRYSECOND");
            GrandScriptUtils.MakeAutoCompleteDDL("txtreligion", url, "hdfReligion", true, true, "RELIGION");
            GrandScriptUtils.MakeAutoCompleteDDL("txtSubReligion", url, "hdfSubReligion", true, true, "SUBRELIGION");

            InitDesignationByJob();
            GrandScriptUtils.MakeAutoCompleteDDL("txtCurrency", url, "hdfCurrency", true, true, "CURRENCY");
            GrandScriptUtils.DatePickerCommon("txtPFEffectiveDate");
            GrandScriptUtils.DatePickerCommon("txtSOCSOEffectiveDate");
            GrandScriptUtils.DatePickerCommon("txtExpiredOn");
            GrandScriptUtils.DatePickerCommon("txtEmpymntTypeDatePopup");
            BankChange();

            $("[id*=txtWorkingDays]").ForceNumericOnly();
            $("[id*=txtNormalWorkingHrs]").ForceNumericOnly();
            $("[id*=txtOTRate]").ForceNumericOnly();
            $("[id*=txtBreakTime]").ForceNumericOnly();
            $("[id*=txtBasicSalary]").ForceNumericOnly();
            InitDate();
            EmploymentTypeChange();
            if ($("[id$=hdfFileuRL]").val() != null && $("[id$=hdfFileuRL]").val() != "") {
                PreviewImageBeforeUpload("", $("[id$=hdfFileuRL]").val());
            }

            if ($("[id$=txtCurrency]").attr("disabled") == true) {
                DisableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
            }
            else {
                EnableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
            }
            InitStateAuto("txtCountry");
            InitStateAuto("txtCountry1");

            var country = parseInt($("[id$=hdfCountry]").val());
            if (country <= 0) {
                DisableAuto($("[id$=txtState]"), $("[id$=hdfState]"));
            }
            var Country1 = parseInt($("[id$=hdfCountry1]").val());
            if (Country1 <= 0)
                DisableAuto($("[id$=txtState1]"), $("[id$=hdfState1]"));

            ChangeMaritalStatus();
        }

        function InitDesignationByJob() {

        var Desig = '<%= GetGlobalResourceObject("ConfigurationsRes", "HrmsInfoHiding").ToString() %>'
                if (Desig == 1) {
                    if ($("[id$=hdfEnableDesignationAutoComplete]").val() != '0') {
                       
                        GrandScriptUtils.MakeAutoCompleteDDL("txtDesignation", url + "?JobCategory=" + $("[id$=ddljobCategory]").val() + "&JobLevel=" + $("[id$=ddlJobLevel]").val(), "hdfDesignation", true, true, "DESIGNATIONBYJOB");
           }
            }
            else
            {
             GrandScriptUtils.MakeAutoCompleteDDL("txtDesignation", url, "hdfDesignation", true, true, "DESIGNATIONBYJOB");
            }
                    }

        function DesignationChangeByJob() {
            if ($("[id$=txtDesignation]").is(":disabled") == false) {
                $("[id$=txtDesignation]").val('');
                $("[id$=hdfDesignation]").val('0')
                InitDesignationByJob();
            }
        }


        function InitStateAuto(targetControlID) {
            if (targetControlID == "txtCountry") {
                GrandScriptUtils.MakeAutoCompleteDDL("txtState", url + "?Country=" + $("[id$=hdfCountry]").val(), "hdfState", true, true, "STATEAUTO");
            }
            else if (targetControlID == "txtCountry1") {
                GrandScriptUtils.MakeAutoCompleteDDL("txtState1", url + "?Country=" + $("[id$=hdfCountry1]").val(), "hdfState1", true, true, "STATEAUTO");
            }
        }
        function ResetEmployee(targetControlID) {
            if (targetControlID == "txtCountry") {
                var defText = '<%= Resources.ErpRes.AutoDefaultValue %>';
                $("[id$=txtState]").val(defText);
                $("[id$=hdfState]").val('-1');
            }
            else if (targetControlID == "txtCountry1") {
                var defText = '<%= Resources.ErpRes.AutoDefaultValue %>';
                $("[id$=txtState1]").val(defText);
                $("[id$=hdfState1]").val('-1');
            }
        }


        //To excecute after  auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtCountry") {
                ResetEmployee(targetControlID)
                EnableAuto($("[id$=txtState]"), $("[id$=hdfState]"));
                InitStateAuto(targetControlID);
            }
            if (targetControlID == "txtCountry1") {
                ResetEmployee(targetControlID)
                EnableAuto($("[id$=txtState1]"), $("[id$=hdfState1]"));
                InitStateAuto(targetControlID);
            }
            if (targetControlID == "txtDesignation") {
                $("[id$=btnDesignationChg]").click();
            }
        }

        //To excecute after  auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtCountry") {
                ResetEmployee(targetControlID)
                InitStateAuto(targetControlID);
                DisableAuto($("[id$=txtState]"), $("[id$=hdfState]"));
            }
            if (targetControlID == "txtCountry1") {
                ResetEmployee(targetControlID)
                InitStateAuto(targetControlID);
                DisableAuto($("[id$=txtState1]"), $("[id$=hdfState1]"));
            }
        }


        //        $(document).ready(function () {
        //            $("[id*=chkOTAvailable]").live("click", function () {
        //                var chkOTAvailable = $(this);
        //                var validator = document.getElementById("<%= rfvOTTemplate.ClientID %>");
        //                if (chkOTAvailable.is(":checked")) {
        //                    $("[id*=ddlOTTemplate]").attr('disabled', false);
        //                    ValidatorEnable(validator, true);
        //                } else {
        //                    $("[id*=ddlOTTemplate]").attr('disabled', true);
        //                    $("select[id$=ddlOTTemplate]").val(-1);
        //                    ValidatorEnable(validator, false);
        //                }
        //            });

        //        });
        function OTAvailableChange() {
            var chkOTAvailable = $("[id*=chkOTAvailable]");
            var validator = document.getElementById("<%= rfvOTTemplate.ClientID %>");
            if (chkOTAvailable.is(":checked")) {
                $("[id*=ddlOTTemplate]").attr('disabled', false);
                $("[id*=chkOTPending]").attr('disabled', false);
                //$("[id*=chkOTPending]").prop('checked', true); 
                ValidatorEnable(validator, true);
            } else {
                $("[id*=ddlOTTemplate]").attr('disabled', true);
                $("select[id$=ddlOTTemplate]").val(-1);
                $("[id*=chkOTPending]").attr('disabled', true);
                //                $("[id*=chkOTPending]").prop('checked', false);
                $("[id*=chkOTPending]").removeAttr("checked");
                ValidatorEnable(validator, false);
            }
        }


        function WorkingDayTypeChange() {
            var validator = document.getElementById("<%= vrfWorkingDays.ClientID %>");
            var WorkingDayType = $("[id*=ddlWorkingDayType]").val();
            if (WorkingDayType == 1) {
                //$("[id*=divWorkingDay]").show();
                $("[id*=txtWorkingDays]").attr('disabled', false);
                $("[id*=txtWorkingDays]").removeClass("input-disabled");
                ValidatorEnable(validator, true);
            }
            else {
                //$("[id*=divWorkingDay]").hide();
                $("[id*=txtWorkingDays]").attr('disabled', true);
                $("[id*=txtWorkingDays]").addClass("input-disabled");
                $("[id*=txtWorkingDays]").val('');
                ValidatorEnable(validator, false);
            }
        }

        //        $(document).ready(function () {
        //            InitDate();
        //            InitComponents();
        //        });
        function EndRequestHandlerPage() {
        }
        function InitDate() {
            //Validates age above 18 yr
            //            var resDay = new Date((new Date().getFullYear() - 18), new Date().getMonth(), new Date().getDate());
            //            GrandScriptUtils.DatePickerCommon("txtDOB", "dd-M-yy", null, true, null, resDay, null, null);
            //            GrandScriptUtils.DatePickerCommon("txtDOJ", "dd-M-yy", null, true, null, new Date(), null, null);
            //            GrandScriptUtils.DatePickerCommon("txtConfirmedOn", "dd-M-yy", null, true, date1, new Date(), null, null);


            //GrandScriptUtils.AddDateRangeCommon("txtDOB", "hdfDOB", "txtDOJ", "hdfDOJ", false, true);
            GrandScriptUtils.RestrictedYearDatePicker("txtDOB", false, true, true, '<%= GetGlobalResourceObject("ConfigurationsRes", "HrmsFromDate").ToString() %>');
            //GrandScriptUtils.DatePickerCommon("txtDOB", false, true, true, "01-Jan-1950", "04-Mar-2017");

            GrandScriptUtils.DatePickerCommon("txtDOJ", "dd-M-yy", null, true, null, new Date(), null, null);
            //            GrandScriptUtils.AddDateRange("txtDOJ", "hdfDOJ", "txtConfirmedOn", "hdfCon", false, true);
            // GrandScriptUtils.DatePickerCommon("txtDOB", null, null, true, null, new Date());
            GrandScriptUtils.DatePickerCommon("txtConfirmedOn");
            GrandScriptUtils.DatePickerCommon("txtDatePopup");
            GrandScriptUtils.DatePickerCommon("txtDDatePopup");
            GrandScriptUtils.DatePickerCommon("txtStatusDatePopup");
            GrandScriptUtils.DatePickerCommon("txtDatePopupBranch");
        }
        function ShowHideJobDetails(flag) {
            ///<summary>
            /// Used to Show/Hide ItemDetails Div
            ///</summary>
            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divJobDetails]").show();
                $("[id$=imbShowJobDetails]").hide();
                $("[id$=imbHideJobDetails]").show();
            }
            else {
                $("[id$=divJobDetails]").hide();
                $("[id$=imbShowJobDetails]").show();
                $("[id$=imbHideJobDetails]").hide();
            }
            $("[id$=hdfIsItemDetailsVisible]").val(flag);
            return false;
        }
        function ShowHideContactInformation(flag) {
            ///<summary>
            /// Used to Show/Hide ItemDetails Div
            ///</summary>

            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divContactInformationDetails]").show();
                $("[id$=imbShowContactdetails]").hide();
                $("[id$=imbHideContactdetails]").show();
            }
            else {
                $("[id$=divContactInformationDetails]").hide();
                $("[id$=imbShowContactdetails]").show();
                $("[id$=imbHideContactdetails]").hide();
            }
            $("[id$=hdfIsItemDetailsVisible]").val(flag);
            return false;
        }

        function ShowHidePayDetails(flag) {
            ///<summary>
            /// Used to Show/Hide Pay Details Div
            ///</summary>
            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divPayDetails]").show();
                $("[id$=imbShowPayDetails]").hide();
                $("[id$=imbHidePayDetails]").show();
            }
            else {
                $("[id$=divPayDetails]").hide();
                $("[id$=imbShowPayDetails]").show();
                $("[id$=imbHidePayDetails]").hide();
            }
            $("[id$=hdfIsItemDetailsVisible]").val(flag);
            return false;
        }

        function ShowHideEmpType(flag) {
            ///<summary>
            /// Used to Show/Hide Pay Details Div
            ///</summary>
            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divEmpTypeDetails]").show();
                $("[id$=imbShowEmpType]").hide();
                $("[id$=imbHideEmpType]").show();
            }
            else {
                $("[id$=divEmpTypeDetails]").hide();
                $("[id$=imbShowEmpType]").show();
                $("[id$=imbHideEmpType]").hide();
            }
            $("[id$=hdfIsItemDetailsVisible]").val(flag);
            return false;
        }

        function ShowHideAdditionalInfo(flag) {
            ///<summary>
            /// Used to Show/Hide ItemDetails Div
            ///</summary>

            //If flag then Show Items
            if (flag == 1) {

                $("[id$=divHideShowAdditionalInfo]").show();
                $("[id$=imbShowAdditionalInfo]").hide();
                $("[id$=imbHideAdditionalInfo]").show();
            }
            else {
                $("[id$=divHideShowAdditionalInfo]").hide();
                $("[id$=imbShowAdditionalInfo]").show();
                $("[id$=imbHideAdditionalInfo]").hide();
            }
            $("[id$=hdfIsContactInfoVisisble]").val(flag);
            return false;
        }

        function SetDefaultAccountName() {
            var empName = $("[id$=txtFirstName]").val();
            var accName = $("[id$=txtAccountName]").val();
            if (accName == "")
                $("[id$=txtAccountName]").val(empName);
        }

        function EmploymentTypeChange() {

            var empmntTypePk = '<%= GetGlobalResourceObject("ConfigurationsRes", "EmpymntTypeContractPk").ToString() %>';
            var empmntType = parseInt($("[id$=ddlEmploymentType]").val());
            if (empmntType == empmntTypePk) {//4210:Contract
                $("[id$=txtExpiredOn]").show();
                $("[id$=lblExpiredOn]").show();
            }
            else {
                $("[id$=txtExpiredOn]").hide();
                $("[id$=lblExpiredOn]").hide();
                $("[id$=txtExpiredOn]").val("");
            }
        }

        function EmploymentTypeChangePopup() {
            var empmntTypePk = '<%= GetGlobalResourceObject("ConfigurationsRes", "EmpymntTypeContractPk").ToString() %>';
            var empmntType = parseInt($("[id$=ddlEmpymntTypePopup]").val());
            if (empmntType == empmntTypePk) {//4210:Contract
                var ExpiredOnText = '<%= GetLocalResourceObject("ExpiredOn").ToString() %>';
                $("[id$=lblDatePopupEmpymntType]").text(ExpiredOnText);
            }
            else {
                var DateText = '<%= GetLocalResourceObject("Date").ToString() %>';
                $("[id$=lblDatePopupEmpymntType]").text(DateText);
            }
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

        //show confirmation msg for Leave Exist
        function ShowConfirmMsgLeaveExist() {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Msg_LeaveExist").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfIsLeaveExcYes]").val(1);
                        $(this).dialog("close");
                        if ($("[id$=hdfIsSaveYesNo]").val() == "1")   // check click button is Save or Save and Continue
                        {
                            $("[id$=btnSaveContinue]").click();
                        }
                        else {
                            $("[id$=btnSave]").click();
                        }
                    },
                    No: function (e) {
                        $("[id$=hdfIsLeaveExcYes]").val(2);
                        $(this).dialog("close");
                        if ($("[id$=hdfIsSaveYesNo]").val() == "1") {
                            $("[id$=btnSaveContinue]").click();
                        }
                        else {
                            $("[id$=btnSave]").click();
                        }
                    }
                }
            });
            return false;
        }

        function ChangeMaritalStatus() {
            var maritalStausPk = '<%= GetGlobalResourceObject("ConfigurationsRes", "HrmsMaritalStatusSinglePk").ToString() %>';
            var maritalPk = $("[id$=ddlMaritalStatus]").val();
            if (maritalStausPk == maritalPk) { // Marital status is Single
                $("[id*=TxtSpouseName]").val("");
                $("[id*=TxtSpouseName]").attr('disabled', true);
                $("[id*=TxtSpouseName]").addClass("input-disabled");
                $("[id*=txtSIdNumber]").val("");
                $("[id*=txtSIdNumber]").attr('disabled', true);
                $("[id*=txtSIdNumber]").addClass("input-disabled");
                $("[id*=chkIsSpouseWorking]").removeAttr("checked");
                $("[id*=chkIsSpouseWorking]").attr('disabled', true);
                $("[id*=chkIsSpouseWorking]").addClass("input-disabled");
                $("[id*=txtSpouseSurname]").val("");
                $("[id*=txtSpouseSurname]").attr('disabled', true);
                $("[id*=txtSpouseSurname]").addClass("input-disabled");
                $("[id*=txtFatherSpouse]").val("");
                $("[id*=txtFatherSpouse]").attr('disabled', true);
                $("[id*=txtFatherSpouse]").addClass("input-disabled");
                $("[id*=txtFatherSpouseIdNo]").val("");
                $("[id*=txtFatherSpouseIdNo]").attr('disabled', true);
                $("[id*=txtFatherSpouseIdNo]").addClass("input-disabled");
                $("[id*=txtMothrSpouse]").val("");
                $("[id*=txtMothrSpouse]").attr('disabled', true);
                $("[id*=txtMothrSpouse]").addClass("input-disabled");
                $("[id*=txtMothrSpouseIdNo]").val("");
                $("[id*=txtMothrSpouseIdNo]").attr('disabled', true);
                $("[id*=txtMothrSpouseIdNo]").addClass("input-disabled");
                $("[id*=txtNoOfchildren]").val("");
                $("[id*=txtNoOfchildren]").attr('disabled', true);
                $("[id*=txtNoOfchildren]").addClass("input-disabled");
                $("[id*=txtNoOfChildrenId]").val("");
                $("[id*=txtNoOfChildrenId]").attr('disabled', true);
                $("[id*=txtNoOfChildrenId]").addClass("input-disabled");
                $("[id*=txtNoOfchildrenEdu]").val("");
                $("[id*=txtNoOfchildrenEdu]").attr('disabled', true);
                $("[id*=txtNoOfchildrenEdu]").addClass("input-disabled");
                $("[id*=txtNoOfChildEduId]").val("");
                $("[id*=txtNoOfChildEduId]").attr('disabled', true);
                $("[id*=txtNoOfChildEduId]").addClass("input-disabled");
            }
            else {
                $("[id*=TxtSpouseName]").attr('disabled', false);
                $("[id*=TxtSpouseName]").removeClass("input-disabled");
                $("[id*=txtSIdNumber]").attr('disabled', false);
                $("[id*=txtSIdNumber]").removeClass("input-disabled");
                $("[id*=chkIsSpouseWorking]").attr('disabled', false);
                $("[id*=chkIsSpouseWorking]").removeClass("input-disabled");
                $("[id*=txtSpouseSurname]").attr('disabled', false);
                $("[id*=txtSpouseSurname]").removeClass("input-disabled");
                $("[id*=txtFatherSpouse]").attr('disabled', false);
                $("[id*=txtFatherSpouse]").removeClass("input-disabled");
                $("[id*=txtFatherSpouseIdNo]").attr('disabled', false);
                $("[id*=txtFatherSpouseIdNo]").removeClass("input-disabled");
                $("[id*=txtMothrSpouse]").attr('disabled', false);
                $("[id*=txtMothrSpouse]").removeClass("input-disabled");
                $("[id*=txtMothrSpouseIdNo]").attr('disabled', false);
                $("[id*=txtMothrSpouseIdNo]").removeClass("input-disabled");
                $("[id*=txtNoOfchildren]").attr('disabled', false);
                $("[id*=txtNoOfchildren]").removeClass("input-disabled");
                $("[id*=txtNoOfChildrenId]").attr('disabled', false);
                $("[id*=txtNoOfChildrenId]").removeClass("input-disabled");
                $("[id*=txtNoOfchildrenEdu]").attr('disabled', false);
                $("[id*=txtNoOfchildrenEdu]").removeClass("input-disabled");
                $("[id*=txtNoOfChildEduId]").attr('disabled', false);
                $("[id*=txtNoOfChildEduId]").removeClass("input-disabled");
            }
        }

        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode           
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlSaveContinue]").hide();
                $("[id$=pnlDelete]").hide();
            }
        }

        //For   check  Already Paid while save
        function ShowEmpDocConfirmation() {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = $("[id$=hdfDocConfrmMsg]").val();
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfIsDocConfrmYes]").val(1);
                        $(this).dialog("close");
                        $("[id$=btnEmpDocs]").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfIsDocConfrmYes]").val(0);
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }

        function BankChange(DocType) {
            var rfvBankName = document.getElementById("<%=rfvBankName.ClientID%>");
            var rfvAccountCode = document.getElementById("<%=rfvAccountCode.ClientID%>");
            var BankName = '<%= GetLocalResourceObject("BankName").ToString() %>';
            var BankNameReq = '<%= GetLocalResourceObject("BankNameReq").ToString() %>';
            var AccountCode = '<%= GetLocalResourceObject("AccountCode").ToString() %>';
            var AccountCodeReq = '<%= GetLocalResourceObject("AccountCodeReq").ToString() %>';

            if ($("[id$=ddlPaymentMode]").val() == "4") {
                $("[id$=ddlBankName]").attr("disabled", false);
                $("[id$=txtAccountCode]").attr("disabled", false);
                $("[id$=txtAccountName]").attr("disabled", false);
                $("[id$=txtIFSCCode]").attr("disabled", false);
                $("[id$=txtBranch]").attr("disabled", false);
               

                var BankReq = '<%= GetGlobalResourceObject("ConfigurationsRes", "HrmsInfoHiding").ToString() %>'
                if (BankReq == 1) {
                    $("[id$=lblBankName]").html(BankNameReq);
                    ValidatorEnable(rfvBankName, true);
                            }

                var AccReq = '<%= GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpAccountCodeReq").ToString() %>'
                if (AccReq == 1) {
                     if (BankReq == 1) {                    
                    $("[id$=lblAccountCode]").html(AccountCodeReq);
                    ValidatorEnable(rfvAccountCode, true);
                    }
                }
                else {
                    $("[id$=lblAccountCode]").html(AccountCode);
                }
            }
            else {
                $("[id$=ddlBankName]").attr("disabled", true);
                $("[id$=txtAccountCode]").attr("disabled", true);
                $("[id$=txtAccountName]").attr("disabled", true);
                $("[id$=txtIFSCCode]").attr("disabled", true);
                $("[id$=txtBranch]").attr("disabled", true);
                $("[id$=lblBankName]").html(BankName);
                $("[id$=lblAccountCode]").html(AccountCode);
                var BankReq = '<%= GetGlobalResourceObject("ConfigurationsRes", "HrmsInfoHiding").ToString() %>'
                if (BankReq == 1) {
                $("[id$=lblBankName]").html(BankNameReq);
                ValidatorEnable(rfvBankName, false);
                ValidatorEnable(rfvAccountCode, false);
                            }
               
              
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlBasicInfo">
        <ContentTemplate>
            <asp:HiddenField ID="hdfEnableBranchLocationAutoComplete" runat="server" Value=""
                ClientIDMode="Static" />
            <asp:HiddenField ID="hdfEnableDepartmentAutoComplete" runat="server" Value="" ClientIDMode="Static" />
            <asp:HiddenField ID="hdfEnableDesignationAutoComplete" runat="server" Value="" ClientIDMode="Static" />
            <div class="fixed-buttons">
                <ucGtiTab:GtiTabControl ID="hrmsTab" runat="server" CurrentTab="2" />
                <div class="Button-container">
                    <asp:Table ID="Table3" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SE_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" Text="<%$ resources:Breadcrumb%>" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEmpEntry">
                                    <li runat="server" id="pnlSaveContinue">
                                        <asp:Button runat="server" ID="btnSaveContinue" OnClick="ActionHandler" CommandName="SAVEANDCONTINUE"
                                            TabIndex="26" Text="<%$ resources:ErpRes,SaveContinue%>" ToolTip="<%$ resources:ErpRes,SaveContinue%>"
                                            ValidationGroup="Employee" OnClientClick="javascript:ValidatePageNow('Employee')"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlSave" visible="false">
                                        <asp:Button runat="server" ID="btnSave" OnClick="ActionHandler" CommandName="SAVE"
                                            TabIndex="27" Text="<%$ resources:Save%>" ToolTip="<%$ resources:Save%>" ValidationGroup="Employee"
                                            OnClientClick="javascript:return ValidatePageNow('Employee')" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li id="pnlDelete">
                                        <asp:Button ID="btnDelete" runat="server" Visible="true" SkinID="btnInner-Delete"
                                            Text="<%$Resources:Controls,Delete%>" OnClientClick="return ShowDeleteConfirm(this);"
                                            CommandName="DELETE" OnClick="ActionHandler" TabIndex="28" ToolTip="Delete" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" OnClick="ActionHandler" Text="<%$ resources:Cancel%>"
                                            ToolTip="<%$ resources:Cancel%>" CommandName="CANCEL" TabIndex="29" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <ucBasicHdr:EmpBasicInfoControl ID="UCempBasicHdr" runat="server" />
                <%--<div class="detail-co3" runat="server" id="divEmployeeHeader">
                    <div class="div3col-S">
                        <asp:Label ID="lblhdrEmployeeNo" runat="server" AssociatedControlID="lblhdrEmployeeNoTxt"
                            Text="<%$ resources:EmployeeNo%>"></asp:Label>
                        <asp:Label ID="lblhdrEmployeeNoTxt" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblhdrEmployeeName" runat="server" AssociatedControlID="lblhdrEmployeeNameTxt"
                            Text="<%$ resources:EmployeeName%>"></asp:Label>
                        <asp:Label ID="lblhdrEmployeeNameTxt" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="div3col-S">
                        <asp:Label ID="lblhdrDOJ" runat="server" AssociatedControlID="lblhdrDOJText" Text="<%$ resources:DOJ:%>"></asp:Label>
                        <asp:Label ID="lblhdrDOJText" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblhdrDOB" runat="server" AssociatedControlID="lblhdrDOBTxt" Text="<%$ resources:DOB:%>"></asp:Label>
                        <asp:Label ID="lblhdrDOBTxt" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="div3col-S">
                        <asp:Label ID="lblhdrDesignation" runat="server" AssociatedControlID="lblhdrDesignationTxt"
                            Text="<%$ resources:Designation:%>"></asp:Label>
                        <asp:Label ID="lblhdrDesignationTxt" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblhdrDepartment" runat="server" AssociatedControlID="lblhdrDepartmentTxt"
                            Text="<%$ resources:Department:%>"></asp:Label>
                        <asp:Label ID="lblhdrDepartmentTxt" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="clear">
                    </div>
                </div>--%>
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks tablelayout">
                    <asp:TableRow ID="PagAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCode" runat="server" Text="<%$ resources:Code*%>" AssociatedControlID="txtEmpCode"></asp:Label>
                                            <asp:TextBox ID="txtEmpCode" runat="server" MaxLength="100" CssClass="input-small-a"
                                                TabIndex="1"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfInvoiceNo" runat="server" Value="" />
                                            <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdfEmpCodeAutoGen" runat="server" Value="0" />
                                            <asp:HiddenField ID="AST_CODE" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfEmpCode" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Employee" EnableClientScript="true" runat="server" ControlToValidate="txtEmpCode"
                                                Display="Static" Text="*" ErrorMessage="<%$ resources:EnterEmployeeCode%>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblOldCode" Text="<%$ resources:newcode%>" CssClass="lbl-17-8perc"
                                                AssociatedControlID="txtOldCode"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtOldCode" Text="" TabIndex="2" MaxLength="18" CssClass="input-small-a"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblFirstName" runat="server" Text="<%$ resources:FirstName*%>" AssociatedControlID="txtFirstName"></asp:Label>
                                            <asp:DropDownList ID="ddlSalutaion" TabIndex="3" runat="server" CssClass="select-small">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvSalutaion" InitialValue="-1" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Employee" EnableClientScript="true" runat="server" ControlToValidate="ddlSalutaion"
                                                Display="Static" Text="*" ErrorMessage="<%$ resources:SelectSalutaion%>">
                                            </asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtFirstName" runat="server" MaxLength="200" onkeypress="return isAlphabet(event)"
                                                TabIndex="4" CssClass="input-w45-7per" onblur="SetDefaultAccountName();"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfFirstName" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Employee" EnableClientScript="true" runat="server" ControlToValidate="txtFirstName"
                                                Display="Static" Text="*" ErrorMessage="<%$ resources:EnterEmployeeFirstName%>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblMiddleName" Text="<%$ resources:MiddleName%>" AssociatedControlID="txtMiddleName"
                                                Visible="false"></asp:Label>
                                            <asp:TextBox ID="txtMiddleName" MaxLength="200" onkeypress="return isAlphabet(event)"
                                                runat="server" TabIndex="5" Visible="false"></asp:TextBox>
                                            <asp:Label runat="server" ID="lblLastName" Text="<%$ resources:LastName%>" AssociatedControlID="txtLastName"
                                                Visible="false"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtLastName" onkeypress="return isAlphabet(event)"
                                                Text="" TabIndex="6" MaxLength="200" Visible="false"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblEmpNameLL" Text="<%$ resources:EmpNameLL%>" AssociatedControlID="txtEmpNameLL"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtEmpNameLL" Text="" TabIndex="7" MaxLength="200"
                                                CssClass="input-half"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblSurname" Text="<%$ resources:Surname%>" AssociatedControlID="txtSurname"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSurname" onkeypress="return isAlphabet(event)"
                                                Text="" TabIndex="7" MaxLength="200" CssClass="input-small-a"></asp:TextBox>
                                            <asp:Label runat="server" ID="lblDOB" Text="<%$ resources:DOB*%>" AssociatedControlID="txtDOB"
                                                CssClass="lbl-19-6perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDOB" CssClass="input-small-a" MaxLength="15" onkeydown="javascript:return CheckKey(event)"
                                                onpaste="return false;" Text="" TabIndex="8">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfDOB" runat="server" Value="" />
                                            <asp:RequiredFieldValidator ID="vrfDOB" CssClass="star" SetFocusOnError="true" ValidationGroup="Employee"
                                                EnableClientScript="true" runat="server" ControlToValidate="txtDOB" Display="Dynamic"
                                                Text="*" ErrorMessage="<%$ resources:EnterEmployeeDOB%>">
                                            </asp:RequiredFieldValidator>
                                            <span style="background: none; border: 0; display: none;" id="spnAge" runat="server">
                                            </span>
                                            <asp:Label ID="lblEmpDOB" Visible="false" runat="server"></asp:Label>
                                            <%--<asp:TextBox runat="server" ID="txtAge" CssClass="medium" ReadOnly="true" Text=""></asp:TextBox>--%>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblNationality" Text="<%$ resources:Nationality%>"
                                                AssociatedControlID="txtNationality"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtNationality" Text="" TabIndex="9" MaxLength="100"
                                                CssClass="select-small-a"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" InitialValue="Select/Type"
                                                CssClass="star" SetFocusOnError="true" ValidationGroup="Employee" EnableClientScript="true"
                                                runat="server" ControlToValidate="txtNationality" Display="Static" Text="*" ErrorMessage="<%$ resources:SelectNationality%>"></asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfNationalityPK" Value="" runat="server" />
                                            <asp:Label runat="server" ID="lblBioMetricId" Text="<%$ resources:BioMetricId%>"
                                                AssociatedControlID="txtBioMetricId" CssClass="lbl-17-8perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtBioMetricId" Text="" TabIndex="10" MaxLength="100"
                                                CssClass="input-small-a"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblGender" Text="<%$ resources:Gender*%>" AssociatedControlID="ddlGender"></asp:Label>
                                            <asp:DropDownList ID="ddlGender" runat="server" CssClass="select-small-b" TabIndex="10">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvGender" InitialValue="-1" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Employee" EnableClientScript="true" runat="server" ControlToValidate="ddlGender"
                                                Display="Static" Text="*" ErrorMessage="<%$ resources:SelectGender%>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblMobile" AssociatedControlID="txtMobile" CssClass="lbl-18perc"><%=(Convert.ToBoolean(GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpMobRequired")) ? GetLocalResourceObject("MobileReq").ToString() : GetLocalResourceObject("Mobile").ToString())%></asp:Label>
                                            <asp:TextBox runat="server" ID="txtMobile" Text="" onkeydown="return numbersonlywithCopypaste(event);"
                                                TabIndex="11" MaxLength="18" CssClass="input-small-a"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfMobile" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Employee" EnableClientScript="true" runat="server" ControlToValidate="txtMobile"
                                                Display="Static" Text="*" ErrorMessage="<%$ resources:EnterMobile%>" Enabled="<%$ resources:ConfigurationsRes,HrmsEmpMobRequired%>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                              <div id="divPersonalEmail" runat="server">
                                             <asp:Label runat="server" ID="lblPersonalEmail" Text="<%$ resources:PersonalEmail%>"
                                                AssociatedControlID="txtPersonalEmail"></asp:Label>
                                            <asp:TextBox ID="txtPersonalEmail" runat="server" TabIndex="12" MaxLength="200" Enabled="true"
                                                CssClass="input-half"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="vrePerEmail" runat="server" ControlToValidate="txtPersonalEmail"
                                                ErrorMessage="<%$ resources:EnterValidPersonalMail%>" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                Display="Static" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="Employee" />
                                              </div>
                                           
                                            <%-- requirment from BWH--%>
                                            <%--<asp:RequiredFieldValidator ID="vrfMobile" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="Employee" EnableClientScript="true" runat="server" ControlToValidate="txtMobile"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:EnterEmployeeMobileNumber%>">
                                        </asp:RequiredFieldValidator>--%>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div id="divImageSection" runat="server">
                                              <%--<span style="background: none; border: 0; height: 2px;" id="lblSpan"></span>--%>
                                        <asp:Image ID="imgEmployee" Width="125px" Height="125px" Style="margin-left: 150px;"
                                            runat="server" />
                                        <asp:HiddenField ID="hdfImage" runat="server" Value="" />
                                        <asp:HiddenField ID="hdfFilename" runat="server" Value="" />
                                        <asp:HiddenField ID="hdfFileuRL" runat="server" Value="" />
                                        <br />
                                        <%--<span style="background: none; border: 0; height: 2px;" id="Span1"></span>--%>
                                        <asp:FileUpload ID="fudImage" Style="margin-left: 150px; width: 70px" runat="server"
                                            CssClass="upload-btn" EnableViewState="true" onChange="javascript:PreviewImageBeforeUpload(this);" />
                                        <a id="anchorFile" runat="server" style="margin-left: -80px;" target="_blank"></a>
                                        <asp:Label ID="lblImageURL" runat="server" Visible="false"></asp:Label>
                                        <asp:Button ID="btnImageUpoad" runat="server" Text="<%$ resources:Upload%>" ToolTip="<%$ resources:Upload%>"
                                            OnClick="ActionHandler" CommandName="UploadImg" Style="margin-left: 150px;" Visible="false" />
                                        <%-- <asp:Button ID="btnRemove" runat="server" ToolTip="<%$ resources:Remove%>" CommandName="DELETEIMAGE"
                                            OnClick="ActionHandler" Text="<%$ resources:Remove%>" />--%>
                                        <%--  <br />--%>
                                        <asp:LinkButton ID="lnkRemove" Style="margin-left: 90px;" runat="server" ToolTip="<%$ resources:Remove%>"
                                            CommandName="DELETEIMAGE" OnClick="ActionHandler" Text="<%$ resources:Remove%>"></asp:LinkButton>
                                        <%--<asp:LinkButton ID="LinkButton1" runat="server" Text="Upload"></asp:LinkButton>
                                        <asp:LinkButton ID="btnRemove" runat="server" Text="Remove"></asp:LinkButton>--%>

                                        </div>
                                      
                                    </td>
                                </tr>
                            </table>
                            <div class="search-colapse-b">
                                <h1>
                                    Job Details</h1>
                                <asp:ImageButton runat="server" ID="imbShowJobDetails" OnClientClick="javascript:return ShowHideJobDetails(1);"
                                    SkinID="imbArrowInactive" ToolTip="<%$ resources:Remove%>Show Details " TabIndex="12" />
                                <asp:ImageButton runat="server" ID="imbHideJobDetails" OnClientClick="javascript:return ShowHideJobDetails();"
                                    Style="display: none" SkinID="imbArrowActive" TabIndex="12" ToolTip="<%$ resources:Remove%>Hide Details" />
                                <asp:HiddenField ID="hdfIsItemDetailsVisible" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divJobDetails">
                                <table class="table-devide" id="tblDetails">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblDepartment" runat="server" Text="<%$ resources:Department*%>" AssociatedControlID="txtDepartment"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDepartment" Text="" TabIndex="13" CssClass="select-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfDepartment" Value="" runat="server" />
                                                <asp:RequiredFieldValidator ID="vrfdepartment" InitialValue="Select/Type" CssClass="star"
                                                    SetFocusOnError="true" ValidationGroup="Employee" EnableClientScript="true" runat="server"
                                                    ControlToValidate="txtDepartment" Display="Static" Text="*" ErrorMessage="<%$ resources:SelectDepartment%>"></asp:RequiredFieldValidator>
                                                <asp:ImageButton ImageUrl="~/Images/Classic/Icons/status-change.png" runat="server"
                                                    ID="imgDeptDetailsPopup" CommandName="DETAILS" OnClick="ActionHandler" SkinID="btnInner-stat-change"
                                                    Style="margin-top: 1px; cursor: default; margin-left: -10px !important;" TabIndex="14"
                                                    Visible="<%$ resources:ConfigurationsRes,HrmsEmpDepartmentPopupVisile%>" ToolTip="<%$ resources:DeptDetails%>" />
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblCompany" Text="<%$ resources:Company*%>" AssociatedControlID="ddlCompany"></asp:Label>
                                                <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="15" CssClass="select-half-a">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="vreCompany" InitialValue="-1" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="Employee" EnableClientScript="true" runat="server" ControlToValidate="ddlCompany"
                                                    Display="Static" Text="*" ErrorMessage="<%$ resources:SelectCompany%>"></asp:RequiredFieldValidator>
                                                <div class="clear">
                                                </div>
                                                <div id="divTeam" runat="server">
                                                <asp:Label ID="lblTeam"  runat="server" Text="<%$ resources:Team%>" AssociatedControlID="txtTeam"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtTeam" Text="" TabIndex="13"  CssClass="select-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfTeam" Value="" runat="server" />
                                                <asp:RequiredFieldValidator ID="vrfTeam" InitialValue="Select/Type" CssClass="star"
                                                    SetFocusOnError="true" ValidationGroup="Employee" EnableClientScript="true" runat="server"
                                                    ControlToValidate="txtTeam" Display="Static" Text="*" ErrorMessage="<%$ resources:SelectTeam%>"
                                                    Enabled="<%$ resources:ConfigurationsRes,HrmsEmpTeamReq%>" ></asp:RequiredFieldValidator>
                                                </div>                                              
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblDOJ" Text="<%$ resources:DOJ*%>" AssociatedControlID="txtDOJ"></asp:Label>
                                                <asp:TextBox ID="txtDOJ" CssClass="input-small" MaxLength="15" onkeydown="return CheckKey(event)"
                                                    onpaste="return false;" runat="server" TabIndex="18"></asp:TextBox>
                                                <asp:HiddenField ID="hdfDOJ" runat="server" Value="" />
                                                <asp:RequiredFieldValidator ID="vrfDOJ" CssClass="star" SetFocusOnError="true" ValidationGroup="Employee"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtDOJ" Display="Dynamic"
                                                    Text="*" ErrorMessage="<%$ resources:EnterEmployeeDOJ%>">
                                                </asp:RequiredFieldValidator>
                                                <span style="background: none; border: 0;" id="spnDOJ" runat="server"></span>
                                                <%--          <asp:CompareValidator runat="server" ID="cmpNumbers" ControlToValidate="txtDOB" ControlToCompare="txtDOJ"
                                                Operator="LessThan" Type="Date" ValidationGroup="Employee" Display="Dynamic"
                                                Text="*" EnableClientScript="true" CssClass="star" ErrorMessage="DOJ must be greater than DOB" />--%>
                                                <div class="clear">
                                                </div>
                                                <div id="divReportTo" runat="server">
                                                <asp:Label runat="server" ID="lblReportto" Text="<%$ resources:Reportto%>" AssociatedControlID="txtReportTo"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtReportTo" Text="" TabIndex="20" CssClass="select-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfReportTo" Value="" runat="server" />
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblOfficialPhone" Text="<%$ resources:OfficialPhone%>"
                                                    AssociatedControlID="txtOfficialPhone"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtOfficialPhone" Text="" TabIndex="25" onkeydown="return numbersonlywithCopypaste(event);"
                                                    MaxLength="20" CssClass="input-small"></asp:TextBox>
                                                <asp:TextBox runat="server" ID="txtOffPhoExtension" placeholder="<%$ resources:ext%>"
                                                    CssClass="input-w7per" onkeydown="return numbersonlywithCopypaste(event);" Text=""
                                                    TabIndex="26" MaxLength="20"></asp:TextBox>
                                                <asp:Label runat="server" ID="lblOfficialMobile" Text="<%$ resources:OfficialMobile%>"
                                                    AssociatedControlID="txtOfficialMobile" CssClass="lbl-12-9perc"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtOfficialMobile" onkeydown="return numbersonlywithCopypaste(event);"
                                                    Text="" TabIndex="27" MaxLength="18" CssClass="input-small"></asp:TextBox>
                                                </div>
                                               
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblDesignation" runat="server" Text="<%$ resources:Designation*%>"
                                                    AssociatedControlID="txtDesignation"></asp:Label>
                                                <asp:HiddenField ID="hdfDesignation" runat="server" />
                                                <asp:TextBox runat="server" TabIndex="20" ID="txtDesignation" CssClass="select-half" />
                                                <asp:Button ID="btnDesignationChg" runat="server" OnClick="ActionHandler" CommandName="CHANGEDESIGNATION"
                                                    Style="display: none" EnableTheming="false" />
                                                <asp:RequiredFieldValidator ID="vreDesgn" InitialValue="Select/Type" CssClass="star"
                                                    SetFocusOnError="true" ValidationGroup="Employee" EnableClientScript="true" runat="server"
                                                    ControlToValidate="txtDesignation" Display="Static" Text="*" ErrorMessage="<%$ resources:SelectDesignation%>"></asp:RequiredFieldValidator>
                                                <asp:ImageButton ImageUrl="~/Images/Classic/Icons/status-change.png" runat="server"
                                                    ID="imbDesigDetailsPopup" CommandName="DETAILS" OnClick="ActionHandler" SkinID="btnInner-stat-change"
                                                    Style="margin-top: 1px; cursor: default; margin-left: -10px !important;" TabIndex="15"
                                                    Visible="<%$ resources:ConfigurationsRes,HrmsEmpDesigPopupVisile%>" ToolTip="<%$ resources:DesigDetails%>" />
                                                <div class="clear">
                                                </div>
                                                <div id="divJobStream" runat="server">
                                                <asp:Label ID="lblJobStream" runat="server" Text="<%$ resources:JobStream%>" AssociatedControlID="ddlJobStream"></asp:Label>
                                                <asp:DropDownList ID="ddlJobStream" runat="server" TabIndex="31" CssClass="select-medium">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblSkillLevel" runat="server" Text="<%$ resources:SkillLevel%>" AssociatedControlID="ddlSkillLevel"
                                                    CssClass="middle-lbl-xsmall-c"></asp:Label>
                                                <asp:DropDownList ID="ddlSkillLevel" runat="server" TabIndex="32" CssClass="select-small-b">
                                                </asp:DropDownList>
                                                </div>
                                             
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblEmploymentType" runat="server" Text="<%$ resources:EmploymentType*%>"
                                                    AssociatedControlID="ddlEmploymentType"></asp:Label>
                                                <asp:DropDownList ID="ddlEmploymentType" runat="server" TabIndex="14" CssClass="select-small-a"
                                                    onchange="javascript:EmploymentTypeChange();">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="vreEmploymentType" InitialValue="-1" CssClass="star"
                                                    SetFocusOnError="true" ValidationGroup="Employee" EnableClientScript="true" runat="server"
                                                    ControlToValidate="ddlEmploymentType" Display="Static" Text="*" ErrorMessage="<%$ resources:SelectEmployementType%>"></asp:RequiredFieldValidator>
                                                <asp:Label runat="server" ID="lblExpiredOn" Text="<%$ resources:ExpiredOn%>" AssociatedControlID="txtExpiredOn"
                                                    CssClass="lbl-20perc"></asp:Label>
                                                <asp:TextBox ID="txtExpiredOn" CssClass="input-small" MaxLength="15" onkeydown="return CheckKey(event)"
                                                    onpaste="return false;" runat="server" TabIndex="34"></asp:TextBox>
                                                <asp:ImageButton ImageUrl="~/Images/Classic/Icons/status-change.png" runat="server"
                                                    ID="imbEmpymtTypeUpdation" CommandName="EMPYMNTTYPEUPDN" OnClick="ActionHandler"
                                                    SkinID="btnInner-stat-change" Style="margin-top: 1px; cursor: default; margin-left: -10px !important;"
                                                     Visible="<%$ resources:ConfigurationsRes,HrmsEmpEmploymentTypePopupVisile%>" TabIndex="16" ToolTip="<%$ resources:EmpymntTypeDetails%>" />
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblBranchLocation" runat="server" Text="<%$ resources:Branch/Location*%>"
                                                    AssociatedControlID="txtBranchLocation"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtBranchLocation" Text="" TabIndex="17" CssClass="select-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfBranchLocation" Value="" runat="server" />
                                                <asp:RequiredFieldValidator ID="vrfBranch" InitialValue="Select/Type" CssClass="star"
                                                    SetFocusOnError="true" ValidationGroup="Employee" EnableClientScript="true" runat="server"
                                                    ControlToValidate="txtBranchLocation" Display="Static" Text="*" ErrorMessage="<%$ resources:SelectBranch%>"></asp:RequiredFieldValidator>
                                                <asp:ImageButton ImageUrl="~/Images/Classic/Icons/status-change.png" runat="server"
                                                    ID="imbBranchDetailsPopup" CommandName="DETAILS" OnClick="ActionHandler" SkinID="btnInner-stat-change"
                                                    Style="margin-top: 1px; cursor: default; margin-left: -10px !important;" TabIndex="18"
                                                    Visible="<%$ resources:ConfigurationsRes,HrmsEmpLocationPopupVisile%>" ToolTip="<%$ resources:BranchDetailsSave%>" />
                                                <div class="clear">
                                                </div>
                                                <div id="divCostCenter" runat="server">
                                                <asp:Label ID="lblCostCenter"  runat="server" Text="<%$ resources:CostCenter%>"
                                                    AssociatedControlID="txtCostCenter"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtCostCenter"   Text="" TabIndex="17" CssClass="select-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfCostCenter" Value="" runat="server" />
                                                <asp:RequiredFieldValidator ID="vfrCostCenter" InitialValue="Select/Type" CssClass="star"
                                                    SetFocusOnError="true" ValidationGroup="Employee" EnableClientScript="true" runat="server"
                                                    ControlToValidate="txtCostCenter" Display="Static" Text="*" ErrorMessage="<%$ resources:SelectCostCenter%>"
                                                    Enabled="<%$ resources:ConfigurationsRes,HrmsEmpCostCenterReq%>" ></asp:RequiredFieldValidator>
                                                </div>                                               
                                                 <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblConfirmedOn" Text="<%$ resources:Confirmedon%>"
                                                    AssociatedControlID="txtConfirmedOn"></asp:Label>
                                                <asp:TextBox runat="server" CssClass="input-small" MaxLength="15" onkeydown="return CheckKey(event)"
                                                    onpaste="return false;" ID="txtConfirmedOn" Text="" TabIndex="19"></asp:TextBox>
                                                <asp:HiddenField ID="hdfCon" runat="server" Value="" />
                                                <asp:HiddenField ID="hdfJoinDate" runat="server" Value="" />
                                                <span style="background: none; border: 0;" id="SpnConfirmendOn" runat="server"></span>
                                                <div class="clear">
                                                </div>
                                                <div id="divReportAdm" runat="server">
                                                     <asp:Label runat="server" ID="lblReportAdm" Text="<%$ resources:ReportToAdm%>" AssociatedControlID="txtReportTo"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtReportToAdm" Text="" TabIndex="20" CssClass="select-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfReportToAdm" Value="-1" runat="server" />
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblOfficialEmail" Text="<%$ resources:OfficialEmail%>"
                                                    AssociatedControlID="txtOfficialEmail"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtOfficialEmail" Text="" TabIndex="28" MaxLength="200"
                                                    CssClass="input-half"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="vreOffmail" runat="server" ControlToValidate="txtOfficialEmail"
                                                    ErrorMessage="<%$ resources:EnterValidOfficialMail%>" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                    Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="Employee" />
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lbljobCategory" runat="server" Text="<%$ resources:JobCategory%>"
                                                    AssociatedControlID="ddljobCategory">
                                                 <%=(Convert.ToBoolean(GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpJobCategoryReq")) ?GetLocalResourceObject("JobCategoryReq").ToString() : GetLocalResourceObject("JobCategory").ToString())%></asp:Label>
                                                <asp:DropDownList ID="ddljobCategory" runat="server" TabIndex="29" CssClass="select-medium"
                                                    onchange="javascript:DesignationChangeByJob();">
                                                </asp:DropDownList>
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="rfvjobCategory" InitialValue="-1" CssClass="star"
                                                        SetFocusOnError="true" ValidationGroup="Employee" EnableClientScript="true" runat="server"
                                                        ControlToValidate="ddljobCategory" Display="Static" Text="*" ErrorMessage="<%$ resources:SelectJobCategory%>"
                                                        Enabled="<%$ resources:ConfigurationsRes,HrmsEmpJobCategoryReq%>"></asp:RequiredFieldValidator>
                                                </div>
                                                <asp:Label ID="lblJobLevel" runat="server" Text="<%$ resources:Captions,JobLevel%>"
                                                    AssociatedControlID="ddlJobLevel" CssClass="lbl-13-3perc">
                                                 <%=(Convert.ToBoolean(GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpJobLevelReq")) ? GetGlobalResourceObject("Captions", "JobLevelReq").ToString() : GetGlobalResourceObject("Captions", "JobLevel").ToString())%></asp:Label>
                                                <asp:DropDownList ID="ddlJobLevel" runat="server" TabIndex="30" CssClass="select-small-a"
                                                    onchange="javascript:DesignationChangeByJob();">
                                                </asp:DropDownList>
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="rfvJobLevel" InitialValue="-1" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="Employee" EnableClientScript="true" runat="server" ControlToValidate="ddlJobLevel"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Captions,SelectJobLevel%>"
                                                        Enabled="<%$ resources:ConfigurationsRes,HrmsEmpJobLevelReq%>"></asp:RequiredFieldValidator>
                                                </div>
                                                <asp:Button ID="btnEmpDocs" runat="server" OnClick="ActionHandler" CommandName="EMPDOCS"
                                                    EnableTheming="false" Style="display: none;" />
                                                </div>
                                               
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblActive" Text="<%$ resources:EActive%>" AssociatedControlID="lblActive"></asp:Label>
                                                <asp:CheckBox ID="chkActive" runat="server" Checked="true" TabIndex="21" />
                                                <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:LStatus%>" AssociatedControlID="ddlStatus"
                                                    CssClass="middle-lbl-small-j"></asp:Label>
                                                <asp:DropDownList ID="ddlStatus" CssClass="select-w24-7per" TabIndex="22" runat="server">
                                                </asp:DropDownList>
                                                <asp:ImageButton ImageUrl="~/Images/Classic/Icons/status-change.png" runat="server"
                                                    ID="imbStatusDetailsPopup" CommandName="DETAILS" OnClick="ActionHandler" SkinID="btnInner-stat-change"
                                                    Style="margin-top: 1px; cursor: default; margin-left: 1px !important;" TabIndex="23"
                                                    Visible="<%$ resources:ConfigurationsRes,HrmsEmpStatusPopupVisile%>" ToolTip="<%$ resources:StatusDetailsSave%>" />
                                                <div class="clear">
                                                </div>
                                                <div id="divStatusDetails" style="display: none">
                                                    <asp:HiddenField ID="hdfStatusPk" runat="server" />
                                                    <asp:Panel runat="server" ID="pnlStatus" CssClass="Button-container-popup">
                                                        <asp:Button runat="server" ID="btnPopUpAdd" CommandName="POPUPADD" OnClick="ActionHandler"
                                                            Text="<%$resources:Controls,Save %>" ToolTip="<%$resources:Controls,Save %>"
                                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" ValidationGroup="SaveTask"
                                                            OnClientClick="javascript:ValidatePageNow('SaveStatus')" TabIndex="213" />
                                                        <asp:Button runat="server" ID="btnPopUpCancel" Text="<%$resources:Controls,Cancel %>"
                                                            CssClass="popupclose" CommandName="POPUPCANCEL" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" TabIndex="214" />
                                                    </asp:Panel>
                                                    <table>
                                                        <tr>
                                                            <td colspan="2">
                                                                <div class="notify" runat="server" id="div1">
                                                                    <div class="popup-headr">
                                                                        <asp:Label ID="lblStatusPopup" runat="server" AssociatedControlID="lblhdrEmployeeNoTxtPopup"
                                                                            Text="<%$ resources:EmployeeNo%>" CssClass="select-small-e2"></asp:Label>
                                                                        <asp:Label ID="lblhdrEmployeeNoTxtPopup" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                        <%--<asp:Label ID="lblhdrEmployeeNoTxtPopup" CssClass="margnlft" runat="server" Text="" Font-Bold="True"></asp:Label>--%>
                                                                        <div class="clear">
                                                                        </div>
                                                                        <asp:Label ID="lblhdrEmployeeNamePopup" runat="server" AssociatedControlID="lblhdrEmployeeNameTxtPopup"
                                                                            Text="<%$ resources:EmployeeName%>" CssClass="select-small-e2"></asp:Label>
                                                                        <asp:Label ID="lblhdrEmployeeNameTxtPopup" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                        <div class="clear">
                                                                        </div>
                                                                        <asp:Label ID="Label1" runat="server" AssociatedControlID="lblhdrBranchTxtPopup"
                                                                            Text="<%$ resources:Branch/Location:%>" CssClass="select-small-e2"></asp:Label>
                                                                        <asp:Label ID="lblhdrBranchTxtPopup" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                    </div>
                                                                    <div class="popup-headr">
                                                                        <asp:Label ID="lblhdrDesignationPopup" runat="server" AssociatedControlID="lblhdrDesignationTxtPopup"
                                                                            Text="<%$ resources:Designation:%>" CssClass="select-w24per"></asp:Label>
                                                                        <asp:Label ID="lblhdrDesignationTxtPopup" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                        <div class="clear">
                                                                        </div>
                                                                        <asp:Label ID="lblhdrDepartmentPopup" runat="server" AssociatedControlID="lblhdrDepartmentTxtPopup"
                                                                            Text="<%$ resources:Department:%>" CssClass="select-w24per"></asp:Label>
                                                                        <asp:Label ID="lblhdrDepartmentTxtPopup" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                        <div class="clear">
                                                                        </div>
                                                                        <asp:Label ID="Label2" runat="server" AssociatedControlID="lblhdrStatusTxtPopup"
                                                                            Text="<%$ resources:Status:%>" CssClass="select-w24per"></asp:Label>
                                                                        <asp:Label ID="lblhdrStatusTxtPopup" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                        <%--<asp:Label ID="lblhdrStatusTxtPopup" runat="server" Text="" Font-Bold="True" CssClass="margnlft"></asp:Label>--%>
                                                                    </div>
                                                                    <div class="clear">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table class="table-devide">
                                                        <tr>
                                                            <td>
                                                                <div class="div2col-P">
                                                                    <asp:Label runat="server" ID="lbGroup" Text="<%$resources:LStatus %>" AssociatedControlID="ddlStatusPopUp"></asp:Label>
                                                                    <asp:DropDownList ID="ddlStatusPopUp" runat="server" TabIndex="210">
                                                                    </asp:DropDownList>
                                                                    <asp:DropDownList ID="ddlStatusCancelPopUp" runat="server" TabIndex="210" Visible="false">
                                                                    </asp:DropDownList>
                                                                    <div class="clear">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="div2col-P">
                                                                    <asp:Label runat="server" ID="lbSubtaskDate" Text="<%$resources:Date %>" AssociatedControlID="txtStatusDatePopup"
                                                                        CssClass="lbl-67-2perc"></asp:Label>
                                                                    <asp:TextBox ID="txtStatusDatePopup" runat="server" MaxLength="13" onkeydown="return CheckKey(event)"
                                                                        onpaste="return false;" CssClass="input-small-c" TabIndex="211"> </asp:TextBox>
                                                                    <asp:HiddenField ID="hdfStatusDatePopup" runat="server" Value="" />
                                                                    <asp:RequiredFieldValidator ID="vrfStatusDatePopup" CssClass="star" SetFocusOnError="true"
                                                                        ValidationGroup="SaveStatus" EnableClientScript="true" runat="server" ControlToValidate="txtStatusDatePopup"
                                                                        Display="Static" Text="*" ErrorMessage="<%$ resources:EnterDate%>">
                                                                    </asp:RequiredFieldValidator>
                                                                    <%--<asp:RequiredFieldValidator ID="vrfDate" CssClass="star" SetFocusOnError="true" ValidationGroup="SaveTask"
                                                                EnableClientScript="true" runat="server" ControlToValidate="txtSubTaskDate" Display="Dynamic"
                                                                Text="*" ErrorMessage="<%$ resources:Err_Sel_Date %>">
                                                            </asp:RequiredFieldValidator>--%>
                                                                    <div class="clear">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <div class="divcol-P">
                                                                    <asp:Label runat="server" ID="lbReasonPopup" Text="<%$resources:Reason %>" AssociatedControlID="txtReasonPopup"></asp:Label>
                                                                    <asp:TextBox ID="txtReasonPopup" runat="server" TextMode="MultiLine" TabIndex="212"> </asp:TextBox>
                                                                    <div class="clear">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div class="content-wrapper">
                                                        <asp:GridView runat="server" ID="grdStatusHistory" Width="100%" AutoGenerateColumns="false"
                                                            EmptyDataRowStyle-CssClass="emptytable">
                                                            <EmptyDataTemplate>
                                                                <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ resources:FromDate%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDateFrom" Text='<%# Eval(Resources.DataFieldRes.EMP_DT_FROM, Resources.Constants.HRMSDateFormatGrid) %>'
                                                                            runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="13%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:ToDate%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDateTo" Text='<%# Eval(Resources.DataFieldRes.EMP_DT_TO, Resources.Constants.HRMSDateFormatGrid) %>'
                                                                            runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="13%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:LStatus%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStatusText" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataFieldRes.EMP_STATUS_TEXT) ,35) %>'
                                                                            runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.EMP_STATUS_TEXT) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="35%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:Reason%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStatusText" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataFieldRes.EMS_REASON) ,30) %>'
                                                                            runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.EMS_REASON) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="30%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                                <div id="divDepartmentDetails" style="display: none">
                                                    <asp:HiddenField ID="hdfDeptPk" runat="server" />
                                                    <asp:Panel runat="server" ID="pnlDeprtment" CssClass="Button-container-popup">
                                                        <asp:Button runat="server" ID="btnDeptPopupAdd" CommandName="POPUPADD" OnClick="ActionHandler"
                                                            Text="<%$resources:Controls,Save %>" ToolTip="<%$resources:Controls,Save %>"
                                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" ValidationGroup="SaveTask"
                                                            OnClientClick="javascript:ValidatePageNow('SaveDepartment')" TabIndex="213" />
                                                        <asp:Button runat="server" ID="btnDeptPopupCancel" Text="<%$resources:Controls,Cancel %>"
                                                            CssClass="popupclose" CommandName="POPUPCANCEL" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" TabIndex="214" />
                                                    </asp:Panel>
                                                    <table id="tblEmpHdr">
                                                        <tr>
                                                            <td colspan="2">
                                                                <div class="notify" runat="server" id="div5">
                                                                    <div class="popup-headr">
                                                                        <asp:Label ID="lblEmpNo" runat="server" AssociatedControlID="lblEmpNoTxt" Text="<%$ resources:EmployeeNo%>"
                                                                            CssClass="select-small-e2"></asp:Label>
                                                                        <asp:Label ID="lblEmpNoTxt" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                        <div class="clear">
                                                                        </div>
                                                                        <asp:Label ID="lblEmpName" runat="server" AssociatedControlID="lblEmpNameTxt" Text="<%$ resources:EmployeeName%>"
                                                                            CssClass="select-small-e2"></asp:Label>
                                                                        <asp:Label ID="lblEmpNameTxt" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                        <div class="clear">
                                                                        </div>
                                                                        <asp:Label ID="lblEmpLoc" runat="server" AssociatedControlID="lblEmpLocTxt" Text="<%$ resources:Branch/Location:%>"
                                                                            CssClass="select-small-e2"></asp:Label>
                                                                        <asp:Label ID="lblEmpLocTxt" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                    </div>
                                                                    <div class="popup-headr">
                                                                        <asp:Label ID="lblEmpDesig" runat="server" AssociatedControlID="lblEmpDesigTxt" Text="<%$ resources:Designation:%>"
                                                                            CssClass="select-w24per"></asp:Label>
                                                                        <asp:Label ID="lblEmpDesigTxt" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                        <div class="clear">
                                                                        </div>
                                                                        <asp:Label ID="lblEmpDept" runat="server" AssociatedControlID="lblEmpDeptTxt" Text="<%$ resources:Department:%>"
                                                                            CssClass="select-w24per"></asp:Label>
                                                                        <asp:Label ID="lblEmpDeptTxt" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                        <div class="clear">
                                                                        </div>
                                                                        <asp:Label ID="lblEmpStatus" runat="server" AssociatedControlID="lblEmpStatusTxt"
                                                                            Text="<%$ resources:Status:%>" CssClass="select-w24per"></asp:Label>
                                                                        <asp:Label ID="lblEmpStatusTxt" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                    </div>
                                                                    <div class="clear">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table class="table-devide">
                                                        <tr>
                                                            <td>
                                                                <div class="div2col-P">
                                                                    <asp:Label runat="server" ID="lblDeptPopup" Text="<%$ resources:Department:%>" AssociatedControlID="txtDepartmentPopup"></asp:Label>
                                                                    <asp:TextBox runat="server" ID="txtDepartmentPopup" Text="" TabIndex="15" CssClass="select-half"></asp:TextBox>
                                                                    <asp:HiddenField ID="hdfDepartmentPopup" Value="" runat="server" />
                                                                    <asp:RequiredFieldValidator ID="vrfDeptPopup" InitialValue="Select/Type" CssClass="star"
                                                                        SetFocusOnError="true" ValidationGroup="SaveDepartment" EnableClientScript="true"
                                                                        runat="server" ControlToValidate="txtDepartmentPopup" Display="Static" Text="*"
                                                                        ErrorMessage="<%$ resources:SelectDepartment%>"></asp:RequiredFieldValidator>
                                                                    <div class="clear">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="div2col-P">
                                                                    <asp:Label runat="server" ID="lblDatePopup" Text="<%$resources:Date %>" AssociatedControlID="txtDatePopup"
                                                                        CssClass="lbl-67-2perc"></asp:Label>
                                                                    <asp:TextBox ID="txtDatePopup" runat="server" MaxLength="13" onkeydown="return CheckKey(event)"
                                                                        onpaste="return false;" CssClass="input-small-c" TabIndex="211"> </asp:TextBox>
                                                                    <asp:HiddenField ID="hdfDatePopup" runat="server" Value="" />
                                                                    <asp:RequiredFieldValidator ID="vrfDatepopup" CssClass="star" SetFocusOnError="true"
                                                                        ValidationGroup="SaveDepartment" EnableClientScript="true" runat="server" ControlToValidate="txtDatePopup"
                                                                        Display="Static" Text="*" ErrorMessage="<%$ resources:EnterDate%>">
                                                                    </asp:RequiredFieldValidator>
                                                                    <div class="clear">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <div class="divcol-P">
                                                                    <asp:Label runat="server" ID="lblRsnPopup" Text="<%$resources:Reason %>" AssociatedControlID="txtRsnPopup"></asp:Label>
                                                                    <asp:TextBox ID="txtRsnPopup" runat="server" TextMode="MultiLine" TabIndex="212"> </asp:TextBox>
                                                                    <div class="clear">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div class="content-wrapper">
                                                        <asp:GridView runat="server" ID="grdDepartmentHistory" Width="100%" AutoGenerateColumns="false"
                                                            EmptyDataRowStyle-CssClass="emptytable">
                                                            <EmptyDataTemplate>
                                                                <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ resources:FromDate%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDeptDateFrom" Text='<%# Eval(Resources.DataFieldRes.EDL_DT_FROM, Resources.Constants.HRMSDateFormatGrid) %>'
                                                                            runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="13%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:ToDate%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDeptDateTo" Text='<%# Eval(Resources.DataFieldRes.EDL_DT_TO, Resources.Constants.HRMSDateFormatGrid) %>'
                                                                            runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="13%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:DeprtmentGrd%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDeptText" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataFieldRes.EDL_DEPT_TEXT) ,35) %>'
                                                                            runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.EDL_DEPT_TEXT) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="35%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:Reason%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDeptRsnText" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataFieldRes.EDL_REASON) ,30) %>'
                                                                            runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.EDL_REASON) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="30%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                                <div id="divBranchDetails" style="display: none">
                                                    <%----%>
                                                    <asp:HiddenField ID="hdfBranchPk" runat="server" />
                                                    <asp:Panel runat="server" ID="pnlBranchDet" CssClass="Button-container-popup" Visible="false">
                                                        <asp:Button runat="server" ID="btnPopUpAddBranch" CommandName="POPUPADD" OnClick="ActionHandler"
                                                            Text="<%$resources:Controls,Save %>" ToolTip="<%$resources:Controls,Save %>"
                                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" ValidationGroup="SaveBranch"
                                                            OnClientClick="javascript:ValidatePageNow('SaveBranch')" TabIndex="203" Visible="false" />
                                                        <asp:Button runat="server" ID="btnPopUpCancelBranch" Text="<%$resources:Controls,Cancel %>"
                                                            CssClass="popupclose" CommandName="POPUPCANCEL" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" TabIndex="204"
                                                            Visible="false" />
                                                    </asp:Panel>
                                                    <table>
                                                        <tr>
                                                            <td colspan="2">
                                                                <div class="notify" runat="server" id="div2">
                                                                    <div class="popup-headr">
                                                                        <asp:Label ID="lblhdrEmployeeNoNamePopupBranch" runat="server" AssociatedControlID="lblhdrEmployeeNoTxtPopupBranch"
                                                                            Text="<%$ resources:EmployeeNo%>"></asp:Label>
                                                                        <asp:Label ID="lblhdrEmployeeNoTxtPopupBranch" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                        <%--<asp:Label ID="lblhdrEmployeeNoTxtPopupBranch" CssClass="margnlft" runat="server" Text="" Font-Bold="True"></asp:Label>--%>
                                                                        <div class="clear">
                                                                        </div>
                                                                        <asp:Label ID="lblhdrEmployeeNamePopupBranch" runat="server" AssociatedControlID="lblhdrEmployeeNameTxtPopupBranch"
                                                                            Text="<%$ resources:EmployeeName%>"></asp:Label>
                                                                        <asp:Label ID="lblhdrEmployeeNameTxtPopupBranch" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                        <div class="clear">
                                                                        </div>
                                                                        <asp:Label ID="Label1Branch" runat="server" AssociatedControlID="lblhdrBranchTxtPopupBranch"
                                                                            Text="<%$ resources:Branch/Location:%>"></asp:Label>
                                                                        <asp:Label ID="lblhdrBranchTxtPopupBranch" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                    </div>
                                                                    <div class="popup-headr">
                                                                        <asp:Label ID="lblhdrDesignationPopupBranch" runat="server" AssociatedControlID="lblhdrDesignationTxtPopupBranch"
                                                                            Text="<%$ resources:Designation:%>"></asp:Label>
                                                                        <asp:Label ID="lblhdrDesignationTxtPopupBranch" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                        <asp:Label ID="lblhdrDepartmentPopupBranch" runat="server" AssociatedControlID="lblhdrDepartmentTxtPopupBranch"
                                                                            Text="<%$ resources:Department:%>"></asp:Label>
                                                                        <asp:Label ID="lblhdrDepartmentTxtPopupBranch" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                        <div class="clear">
                                                                        </div>
                                                                        <asp:Label ID="Label2Branch" runat="server" AssociatedControlID="lblhdrStatusTxtPopupBranch"
                                                                            Text="<%$ resources:Status:%>"></asp:Label>
                                                                        <asp:Label ID="lblhdrStatusTxtPopupBranch" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                        <%--<asp:Label ID="lblhdrStatusTxtPopupBranch" runat="server" Text="" Font-Bold="True" CssClass="margnlft"></asp:Label>--%>
                                                                    </div>
                                                                    <div class="clear">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table class="table-devide" style="display: none;">
                                                        <tr>
                                                            <td>
                                                                <div class="div2col-P">
                                                                    <asp:Label runat="server" ID="lbBranchPopUp" Text="<%$resources:Branch/Location %>"
                                                                        AssociatedControlID="txtBranchLocationPopup"></asp:Label>
                                                                    <%--<asp:DropDownList ID="ddlBranchPopUp" runat="server">
                                                                </asp:DropDownList>--%>
                                                                    <asp:TextBox runat="server" ID="txtBranchLocationPopup" Text="" CssClass="input-small-f"
                                                                        ValidationGroup="SaveBranch" TabIndex="200"></asp:TextBox>
                                                                    <asp:HiddenField ID="hdfBranchLocationPopup" Value="" runat="server" />
                                                                    <asp:RequiredFieldValidator ID="vrfBranchPopup" CssClass="star" SetFocusOnError="true"
                                                                        ValidationGroup="SaveBranch" runat="server" ControlToValidate="txtBranchLocationPopup"
                                                                        Display="Static" InitialValue="Select/Type" Text="*" ErrorMessage="Select Branch">  <%--<%$ resources:Err_Sel_Date %> EnableClientScript="true"--%>
                                                                    </asp:RequiredFieldValidator>
                                                                    <div class="clear">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="div2col-P">
                                                                    <asp:Label runat="server" ID="lbBranchDate" Text="<%$resources:Date %>" AssociatedControlID="txtDatePopupBranch"
                                                                        CssClass="lbl-67-2perc"></asp:Label>
                                                                    <asp:TextBox ID="txtDatePopupBranch" runat="server" MaxLength="13" onkeydown="return CheckKey(event)"
                                                                        onpaste="return false;" ValidationGroup="SaveBranch" CssClass="input-small-c"
                                                                        TabIndex="201"> </asp:TextBox>
                                                                    <asp:HiddenField ID="hdfDatePopupBranch" runat="server" Value="" />
                                                                    <asp:RequiredFieldValidator ID="vrfBranchPopupDate" CssClass="star" SetFocusOnError="true"
                                                                        ValidationGroup="SaveBranch" runat="server" ControlToValidate="txtDatePopupBranch"
                                                                        Display="Static" Text="*" ErrorMessage="Select Date">  <%--<%$ resources:Err_Sel_Date %> EnableClientScript="true"--%>
                                                                    </asp:RequiredFieldValidator>
                                                                    <div class="clear">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <div class="divcol-P">
                                                                    <asp:Label runat="server" ID="lbReasonPopupBranch" Text="<%$resources:Reason %>"
                                                                        AssociatedControlID="txtReasonPopupBranch"></asp:Label>
                                                                    <asp:TextBox ID="txtReasonPopupBranch" runat="server" TextMode="MultiLine" TabIndex="202"> </asp:TextBox>
                                                                    <div class="clear">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div class="content-wrapper">
                                                        <asp:GridView runat="server" ID="grdBranchHistory" Width="100%" AutoGenerateColumns="false"
                                                            EmptyDataRowStyle-CssClass="emptytable">
                                                            <EmptyDataTemplate>
                                                                <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ resources:FromDate%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDateFrom" Text='<%# Eval(Resources.DataFieldRes.EMB_DT_FROM, Resources.Constants.HRMSDateFormatGrid) %>'
                                                                            runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="15%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:ToDate%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDateTo" Text='<%# Eval(Resources.DataFieldRes.EMB_DT_TO, Resources.Constants.HRMSDateFormatGrid) %>'
                                                                            runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="11%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:Branch/Location%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStatusText" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataFieldRes.EMB_BRANCH_TEXT) ,22) %>'
                                                                            runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.EMB_BRANCH_TEXT) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="25%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:Reason%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStatusText" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataFieldRes.EMB_REASON) ,50) %>'
                                                                            runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.EMB_REASON) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="50%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                                <div id="divEmpymTypeDetails" style="display: none">
                                                    <asp:HiddenField ID="hdfEmpymntTypePk" runat="server" />
                                                    <asp:Panel runat="server" ID="pnlEmpymntTye" CssClass="Button-container-popup">
                                                        <asp:Button runat="server" ID="btnEmpymntTyeSave" CommandName="EMPYMNTTYPESAVE" OnClick="ActionHandler"
                                                            Text="<%$resources:Controls,Save %>" ToolTip="<%$resources:Controls,Save %>"
                                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" ValidationGroup="EmpymntTyeSave"
                                                            OnClientClick="javascript:ValidatePageNow('EmpymntTyeSave')" TabIndex="213" />
                                                        <asp:Button runat="server" ID="btnEmpymntTyeCancel" Text="<%$resources:Controls,Cancel %>"
                                                            CssClass="popupclose" CommandName="POPUPCANCEL" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" TabIndex="214" />
                                                    </asp:Panel>
                                                    <table>
                                                        <tr>
                                                            <td colspan="2">
                                                                <div class="notify" runat="server" id="div4">
                                                                    <div class="popup-headr">
                                                                        <asp:Label ID="lblhdrEmployeeNoEmpymntTye" runat="server" AssociatedControlID="lblEmployeeNoEmpymntTye"
                                                                            Text="<%$ resources:EmployeeNo%>"></asp:Label>
                                                                        <asp:Label ID="lblEmployeeNoEmpymntTye" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                        <div class="clear">
                                                                        </div>
                                                                        <asp:Label ID="lblhdrEmployeeNameEmpymntTye" runat="server" AssociatedControlID="lblEmployeeNameEmpymntTye"
                                                                            Text="<%$ resources:EmployeeName%>"></asp:Label>
                                                                        <asp:Label ID="lblEmployeeNameEmpymntTye" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                        <div class="clear">
                                                                        </div>
                                                                        <asp:Label ID="lblhdrBrLocEmpymntTye" runat="server" AssociatedControlID="lblBrLocEmpymntTye"
                                                                            Text="<%$ resources:Branch/Location:%>"></asp:Label>
                                                                        <asp:Label ID="lblBrLocEmpymntTye" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                    </div>
                                                                    <div class="popup-headr">
                                                                        <asp:Label ID="lblhdrDesignationEmpymntTye" runat="server" AssociatedControlID="lblDesignationEmpymntTye"
                                                                            Text="<%$ resources:Designation:%>"></asp:Label>
                                                                        <asp:Label ID="lblDesignationEmpymntTye" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                        <asp:Label ID="lblhdrDepartmentEmpymntTye" runat="server" AssociatedControlID="lblDepartmentEmpymntTye"
                                                                            Text="<%$ resources:Department:%>"></asp:Label>
                                                                        <asp:Label ID="lblDepartmentEmpymntTye" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                        <div class="clear">
                                                                        </div>
                                                                        <asp:Label ID="lblhdrStatusEmpymntTye" runat="server" AssociatedControlID="lblStatusEmpymntTye"
                                                                            Text="<%$ resources:Status:%>"></asp:Label>
                                                                        <asp:Label ID="lblStatusEmpymntTye" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                    </div>
                                                                    <div class="clear">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table class="table-devide">
                                                        <tr>
                                                            <td>
                                                                <div class="div2col-P">
                                                                    <asp:Label runat="server" ID="lblPopupEmpymntType" Text="<%$ resources:EmploymentType%>"
                                                                        AssociatedControlID="ddlEmpymntTypePopup"></asp:Label>
                                                                    <asp:DropDownList ID="ddlEmpymntTypePopup" runat="server" TabIndex="210" onchange="javascript:EmploymentTypeChangePopup();">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="vrfEmpymntTypePopup" CssClass="star" SetFocusOnError="true"
                                                                        ValidationGroup="EmpymntTyeSave" EnableClientScript="true" runat="server" ControlToValidate="ddlEmpymntTypePopup"
                                                                        Display="Static" Text="*" ErrorMessage="<%$ resources:SelectEmployementType%>"
                                                                        InitialValue="-1">
                                                                    </asp:RequiredFieldValidator>
                                                                    <div class="clear">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="div2col-P">
                                                                    <asp:Label runat="server" ID="lblDatePopupEmpymntType" Text="<%$resources:Date %>"
                                                                        AssociatedControlID="txtEmpymntTypeDatePopup" CssClass="lbl-67-2perc"></asp:Label>
                                                                    <asp:TextBox ID="txtEmpymntTypeDatePopup" runat="server" MaxLength="13" onkeydown="return CheckKey(event)"
                                                                        onpaste="return false;" CssClass="input-small-c" TabIndex="211"> </asp:TextBox>
                                                                    <asp:HiddenField ID="hdfEmpymntTypeDatePopup" runat="server" Value="" />
                                                                    <asp:RequiredFieldValidator ID="vrfEmpymntTypeDatePopup" CssClass="star" SetFocusOnError="true"
                                                                        ValidationGroup="EmpymntTyeSave" EnableClientScript="true" runat="server" ControlToValidate="txtEmpymntTypeDatePopup"
                                                                        Display="Static" Text="*" ErrorMessage="<%$ resources:EnterDate%>">
                                                                    </asp:RequiredFieldValidator>
                                                                    <div class="clear">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <div class="divcol-P">
                                                                    <asp:Label runat="server" ID="lblReasonPopupEmpymntType" Text="<%$resources:Reason %>"
                                                                        AssociatedControlID="txtReasonPopup"></asp:Label>
                                                                    <asp:TextBox ID="txtReasonPopupEmpymntType" runat="server" TextMode="MultiLine" TabIndex="212"> </asp:TextBox>
                                                                    <div class="clear">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div class="content-wrapper">
                                                        <asp:GridView runat="server" ID="grdEmpymntTypeHistory" Width="100%" AutoGenerateColumns="false"
                                                            EmptyDataRowStyle-CssClass="emptytable">
                                                            <EmptyDataTemplate>
                                                                <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ resources:FromDate%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgrdEmpymntTypeDateFrom" Text='<%# Eval("ETY_DT_FROM", Resources.Constants.HRMSDateFormatGrid) %>'
                                                                            runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="15%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:ToDate%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgrdEmpymntTypeDateTo" Text='<%# Eval("ETY_DT_TO", Resources.Constants.HRMSDateFormatGrid) %>'
                                                                            runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="11%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:EmploymentType%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgrdEmpymntTypeText" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("ETY_EMP_TYPE_TEXT") ,19) %>'
                                                                            runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("ETY_EMP_TYPE_TEXT") ,500) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="20%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:Reason%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgrdEmpymntTypeReason" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("ETY_REASON") ,57) %>'
                                                                            runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("ETY_REASON") ,500) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="54%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                                <div id="divDesignationDetails" style="display: none">
                                                    <asp:HiddenField ID="hdfDesigPK" runat="server" />
                                                    <asp:Panel runat="server" ID="pnlDesignation" CssClass="Button-container-popup">
                                                        <asp:Button runat="server" ID="btnDesignPopupAdd" CommandName="POPUPADD" OnClick="ActionHandler"
                                                            Text="<%$resources:Controls,Save %>" ToolTip="<%$resources:Controls,Save %>"
                                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" ValidationGroup="SaveTask"
                                                            OnClientClick="javascript:ValidatePageNow('SaveDesignation')" TabIndex="213" />
                                                        <asp:Button runat="server" ID="btnDesigPopupCancel" Text="<%$resources:Controls,Cancel %>"
                                                            CssClass="popupclose" CommandName="POPUPCANCEL" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" TabIndex="214" />
                                                    </asp:Panel>
                                                    <table id="tblDesigEmpHdr">
                                                        <tr>
                                                            <td colspan="2">
                                                                <div class="notify" runat="server" id="div6">
                                                                    <div class="popup-headr">
                                                                        <asp:Label ID="lblDEmpNo" runat="server" AssociatedControlID="lblDEmpNoText" Text="<%$ resources:EmployeeNo%>"
                                                                            CssClass="select-small-e2"></asp:Label>
                                                                        <asp:Label ID="lblDEmpNoText" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                        <div class="clear">
                                                                        </div>
                                                                        <asp:Label ID="lblDEmpName" runat="server" AssociatedControlID="lblDEmpNameText"
                                                                            Text="<%$ resources:EmployeeName%>" CssClass="select-small-e2"></asp:Label>
                                                                        <asp:Label ID="lblDEmpNameText" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                        <div class="clear">
                                                                        </div>
                                                                        <asp:Label ID="lblDEmpBrch" runat="server" AssociatedControlID="lblDEmpBrchText"
                                                                            Text="<%$ resources:Branch/Location:%>" CssClass="select-small-e2"></asp:Label>
                                                                        <asp:Label ID="lblDEmpBrchText" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                    </div>
                                                                    <div class="popup-headr">
                                                                        <asp:Label ID="lblDEmpDesig" runat="server" AssociatedControlID="lblDEmpDesigText"
                                                                            Text="<%$ resources:Designation:%>" CssClass="select-w24per"></asp:Label>
                                                                        <asp:Label ID="lblDEmpDesigText" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                        <div class="clear">
                                                                        </div>
                                                                        <asp:Label ID="lblDEmpDept" runat="server" AssociatedControlID="lblDEmpDeptText"
                                                                            Text="<%$ resources:Department:%>" CssClass="select-w24per"></asp:Label>
                                                                        <asp:Label ID="lblDEmpDeptText" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                        <div class="clear">
                                                                        </div>
                                                                        <asp:Label ID="lblDEmpStatus" runat="server" AssociatedControlID="lblDEmpStatusText"
                                                                            Text="<%$ resources:Status:%>" CssClass="select-w24per"></asp:Label>
                                                                        <asp:Label ID="lblDEmpStatusText" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                                    </div>
                                                                    <div class="clear">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table class="table-devide">
                                                        <tr>
                                                            <td>
                                                                <div class="div2col-P">
                                                                    <asp:Label runat="server" ID="lblDesigPopup" Text="<%$ resources:Designation%>" AssociatedControlID="txtDesignationPopup"></asp:Label>
                                                                    <asp:TextBox runat="server" ID="txtDesignationPopup" Text="" TabIndex="15" CssClass="select-half"></asp:TextBox>
                                                                    <asp:HiddenField ID="hdfDesignationPopup" Value="" runat="server" />
                                                                    <asp:RequiredFieldValidator ID="rfvDesigPopup" InitialValue="Select/Type" CssClass="star"
                                                                        SetFocusOnError="true" ValidationGroup="SaveDesignation" EnableClientScript="true"
                                                                        runat="server" ControlToValidate="txtDesignationPopup" Display="Static" Text="*"
                                                                        ErrorMessage="<%$ resources:SelectDesignation%>"></asp:RequiredFieldValidator>
                                                                    <div class="clear">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="div2col-P">
                                                                    <asp:Label runat="server" ID="lblDDatePopup" Text="<%$resources:Date %>" AssociatedControlID="txtDDatePopup"
                                                                        CssClass="lbl-67-2perc"></asp:Label>
                                                                    <asp:TextBox ID="txtDDatePopup" runat="server" MaxLength="13" onkeydown="return CheckKey(event)"
                                                                        onpaste="return false;" CssClass="input-small-c" TabIndex="211"> </asp:TextBox>
                                                                    <asp:HiddenField ID="hdfDDatePopup" runat="server" Value="" />
                                                                    <asp:RequiredFieldValidator ID="rfvDDatePopup" CssClass="star" SetFocusOnError="true"
                                                                        ValidationGroup="SaveDesignation" EnableClientScript="true" runat="server" ControlToValidate="txtDDatePopup"
                                                                        Display="Static" Text="*" ErrorMessage="<%$ resources:EnterDate%>">
                                                                    </asp:RequiredFieldValidator>
                                                                    <div class="clear">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <div class="divcol-P">
                                                                    <asp:Label runat="server" ID="lblDRsnPopup" Text="<%$resources:Reason %>" AssociatedControlID="txtDRsnPopup"></asp:Label>
                                                                    <asp:TextBox ID="txtDRsnPopup" runat="server" TextMode="MultiLine" TabIndex="212"> </asp:TextBox>
                                                                    <div class="clear">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div class="content-wrapper">
                                                        <asp:GridView runat="server" ID="grdDesignationHistory" Width="100%" AutoGenerateColumns="false"
                                                            EmptyDataRowStyle-CssClass="emptytable">
                                                            <EmptyDataTemplate>
                                                                <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ resources:FromDate%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDesigDateFrom" Text='<%# Eval(Resources.DataFieldRes.DSL_DT_FROM, Resources.Constants.HRMSDateFormatGrid) %>'
                                                                            runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="13%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:ToDate%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDesigDateTo" Text='<%# Eval(Resources.DataFieldRes.DSL_DT_TO, Resources.Constants.HRMSDateFormatGrid) %>'
                                                                            runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="13%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:DesignationGrd%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDesigText" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataFieldRes.DSL_DEPT_TEXT) ,35) %>'
                                                                            runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.DSL_DEPT_TEXT) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="35%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:Reason%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDesigRsnText" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataFieldRes.DSL_REASON) ,30) %>'
                                                                            runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.DSL_REASON) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="30%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divContactDetails" runat="server" class="search-colapse-b">
                                <h1>
                                    Contact Information
                                </h1>
                                <asp:ImageButton runat="server" ID="imbShowContactdetails" OnClientClick="javascript:return ShowHideContactInformation(1);"
                                    SkinID="imbArrowInactive" TabIndex="33" ToolTip="<%$ resources:ShowContactDetails%>" />
                                <asp:ImageButton runat="server" ID="imbHideContactdetails" TabIndex="33" OnClientClick="javascript:return ShowHideContactInformation();"
                                    Style="display: none" SkinID="imbArrowActive" ToolTip="<%$ resources:HideContactDetails%>" />
                                <asp:HiddenField ID="hdfShowTaxAmtBC" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divContactInformationDetails" style="display: none">
                                <table class="table-devide" id="Table1">
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblPermanentAddress" runat="server" Text="<%$ resources:PermanentAddress%>"
                                                    AssociatedControlID="txtPermanentAddress"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtPermanentAddress" Text="" TextMode="MultiLine"
                                                    TabIndex="34" MaxLength="200" onkeydown="limitText(this,200);" onkeyup="limitText(this,200);"
                                                    onpase="limitText(this,200);" CssClass="multiline-3line input-half"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblCity" runat="server" Text="<%$ resources:City%>" AssociatedControlID="txtCity"></asp:Label>
                                                <asp:TextBox runat="server" onkeypress="return isAlphabet(event)" ID="txtCity" Text=""
                                                    TabIndex="35" MaxLength="100" CssClass="input-small"></asp:TextBox>
                                                <asp:Label ID="lblDistrict1" runat="server" Text="<%$ resources:District%>" AssociatedControlID="txtDistrict1"
                                                    CssClass="lbl-20-7perc"></asp:Label>
                                                <asp:TextBox runat="server" onkeypress="return isAlphabet(event)" ID="txtDistrict1"
                                                    Text="" TabIndex="36" MaxLength="100" CssClass="input-small-a"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblCountry" Text="<%$ resources:Country*%>" AssociatedControlID="txtCountry"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtCountry" Text="" TabIndex="37" MaxLength="100"
                                                    CssClass="input-small"></asp:TextBox>
                                                <%--onblur="javascript:return CopyCountry()"--%>
                                                <asp:HiddenField ID="hdfCountry" Value="-1" runat="server" />
                                                <asp:RequiredFieldValidator ID="vrfCountry" InitialValue="Select/Type" CssClass="star"
                                                    SetFocusOnError="true" ValidationGroup="Employee" EnableClientScript="true" runat="server"
                                                    ControlToValidate="txtCountry" Display="Static" Text="*" ErrorMessage="<%$ resources:SelectCountry%>"
                                                    Enabled="<%$ resources:ConfigurationsRes,HrmsEmpJobCountryReq%>"></asp:RequiredFieldValidator>
                                                <asp:Label runat="server" ID="lblState" Text="<%$ resources:State%>" CssClass="lbl-19perc"
                                                    AssociatedControlID="txtState"></asp:Label>
                                                <asp:TextBox runat="server" onkeypress="return isAlphabet(event)" ID="txtState" Text=""
                                                    TabIndex="38" MaxLength="100" CssClass="select-small-a"></asp:TextBox>
                                                <asp:HiddenField ID="hdfState" runat="server" Value="-1" />
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblZipCode" Text="<%$ resources:ZipCode%>" AssociatedControlID="txtZipCode"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtZipCode" Text="" onkeydown="return numbersonlywithCopypaste(event);"
                                                    TabIndex="39" MaxLength="10" CssClass="input-small"></asp:TextBox>
                                                <asp:Label runat="server" ID="lblPhone" Text="<%$ resources:APhone%>" AssociatedControlID="txtPhone"
                                                    CssClass="lbl-20-7perc"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtPhone" Text="" onkeydown="return numbersonlywithCopypaste(event);"
                                                    TabIndex="40" MaxLength="18" CssClass="input-small-a"></asp:TextBox>
                                                <%--<asp:DropDownList ID="ddlNationality" runat="server" CssClass="medium" TabIndex="36">
                                                </asp:DropDownList>--%>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblCommunicationAddress" runat="server" Text="<%$ resources:CommunicationAddress%>"
                                                    AssociatedControlID="txtCommunicationAddress"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtCommunicationAddress" TextMode="MultiLine" Text=""
                                                    TabIndex="41" MaxLength="200" onkeydown="limitText(this,200);" onkeyup="limitText(this,200);"
                                                    onpase="limitText(this,200);" CssClass="multiline-3line input-half"></asp:TextBox>
                                                <asp:CheckBox ID="chkSameAsAbove" TabIndex="41" ToolTip="<%$ resources:ToCopy%>"
                                                    runat="server" onclick="javascript:CopyAdd();" Style="width: 50px; background: none;
                                                    border: 0; height: 2px; padding: 0px 2px; margin-top: -2px;" />
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblCity1" runat="server" Text="<%$ resources:City%>" AssociatedControlID="txtCity1"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtCity1" onkeypress="return isAlphabet(event)" Text=""
                                                    TabIndex="42" MaxLength="100" CssClass="input-small"></asp:TextBox>
                                                <asp:Label ID="lblDistrict2" runat="server" Text="<%$ resources:District%>" AssociatedControlID="txtDistrict2"
                                                    CssClass="lbl-20-7perc"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDistrict2" onkeypress="return isAlphabet(event)"
                                                    Text="" TabIndex="43" MaxLength="100" CssClass="input-small-a"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblCountry1" Text="Country" AssociatedControlID="txtCountry1"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtCountry1" Text="" TabIndex="44" MaxLength="100"
                                                    CssClass="input-small"></asp:TextBox>
                                                <asp:HiddenField ID="hdfCountry1" Value="-1" runat="server" />
                                                <asp:Label runat="server" ID="lblState1" Text="<%$ resources:State%>" AssociatedControlID="txtState1"
                                                    CssClass="lbl-20-7perc"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtState1" onkeypress="return isAlphabet(event)"
                                                    Text="" TabIndex="45" MaxLength="100" CssClass="select-small-a"></asp:TextBox>
                                                <asp:HiddenField ID="hdfState1" Value="-1" runat="server" />
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblZipCode1" Text="<%$ resources:ZipCode%>" AssociatedControlID="txtZipCode1"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtZipCode1" onkeydown="return numbersonlywithCopypaste(event);"
                                                    Text="" TabIndex="46" MaxLength="10" CssClass="input-small"></asp:TextBox>
                                                <asp:Label runat="server" ID="lblPhone1" Text="<%$ resources:APhone%>" AssociatedControlID="txtPhone1"
                                                    CssClass="lbl-20-7perc"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtPhone1" onkeydown="return numbersonlywithCopypaste(event);"
                                                    Text="" TabIndex="47" MaxLength="18" CssClass="input-small-a"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divPayDtls" runat="server" class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("SalaryType").ToString()%>
                                </h1>
                                <asp:ImageButton runat="server" ID="imbShowPayDetails" OnClientClick="javascript:return ShowHidePayDetails(1);"
                                    SkinID="imbArrowInactive" TabIndex="24" ToolTip="<%$ resources:ShowPayDetails%>" />
                                <asp:ImageButton runat="server" ID="imbHidePayDetails" TabIndex="24" OnClientClick="javascript:return ShowHidePayDetails();"
                                    Style="display: none" SkinID="imbArrowActive" ToolTip="<%$ resources:HidePayDetails%>" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divPayDetails">
                                <table class="table-devide" runat="server" id="pnlPayDetails">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                            <div id="divPaymentMode" runat="server">
                                                <asp:Label runat="server" ID="lblPaymentMode" AssociatedControlID="ddlPaymentMode"
                                                    Text="<%$resources:PaymentModeReq %>"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlPaymentMode" onchange="BankChange(this)"
                                                    CssClass="select-small-a" TabIndex="49">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="reqPaymentMode" CssClass="star" SetFocusOnError="true"
                                                    InitialValue="-1" ValidationGroup="Employee" EnableClientScript="true" runat="server"
                                                    ControlToValidate="ddlPaymentMode" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_PaymentMode %>"
                                                    Enabled ="<%$ resources:ConfigurationsRes,HrmsEmpPaymentModeReq%>"></asp:RequiredFieldValidator>
                                                <asp:Label runat="server" ID="lblCurrency" Text="<%$ resources:CurrencyReq%>" CssClass="lbl-13perc"
                                                    AssociatedControlID="txtCurrency"></asp:Label>
                                                <asp:TextBox ID="txtCurrency" runat="server" CssClass="input-small-c" TabIndex="60"
                                                    MaxLength="100"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfCurrency" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="Employee" EnableClientScript="true" runat="server" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>"
                                                    ControlToValidate="txtCurrency" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_Currency %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:HiddenField ID="hdfCurrency" runat="server" />
                                                <div class="clear">
                                                </div>
                                               
                                              
                                                <asp:Label runat="server" ID="lblAccountCode" AssociatedControlID="txtAccountCode"
                                                    Text="<%$resources:AccountCode %>" Enabled="false"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtAccountCode" MaxLength="100" Enabled="false" CssClass="input-small"
                                                    TabIndex="52" />
                                                <asp:RequiredFieldValidator ID="rfvAccountCode" CssClass="star" SetFocusOnError="true"
                                                    Enabled="false" ValidationGroup="Employee" EnableClientScript="true" runat="server"
                                                    ControlToValidate="txtAccountCode" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_AccountCode %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:Label runat="server" ID="lblAccountName" AssociatedControlID="txtAccountName"
                                                    CssClass="lbl-12-7perc" Enabled="false" Text="<%$resources:AccountName %>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtAccountName" MaxLength="100" Enabled="false" CssClass="input-small-c"
                                                    TabIndex="53" />  
                                                 </div>
                                                <asp:Label runat="server" ID="lblPayrollType" AssociatedControlID="ddlPayrollType"
                                                    Text="<%$resources:PayrollTypeReq %>"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlPayrollType" CssClass="select-half-a" TabIndex="25">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="vrfPayrollType" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="Employee" EnableClientScript="true" runat="server" InitialValue="-1"
                                                    ControlToValidate="ddlPayrollType" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_PayrollType %>"
                                                    Enabled ="<%$ resources:ConfigurationsRes,HrmsEmpPayrollTypeReq%>"></asp:RequiredFieldValidator>
                                               
                                                <div class="clear">
                                                </div>                                               
                                            </div>
                                        </td>
                                        <td>
                                            <div  class="div2col-S">  
                                                <div id="divBankName" runat="server">
                                                 <asp:Label runat="server" ID="lblBankName" AssociatedControlID="ddlBankName" Text="<%$resources:BankName %>"
                                                    Enabled="false"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlBankName" Enabled="false" CssClass="select-w61per"
                                                    AutoPostBack="true" TabIndex="51" OnSelectedIndexChanged="ActionHandler">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvBankName" CssClass="star" SetFocusOnError="true"
                                                    Enabled="false" InitialValue="-1" ValidationGroup="Employee" EnableClientScript="true"
                                                    runat="server" ControlToValidate="ddlBankName" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_BankName %>">
                                                </asp:RequiredFieldValidator>
                                                <div class="clear">
                                                </div>
                                                
                                                <asp:Label runat="server" ID="lblIFSCCode" AssociatedControlID="txtIFSCCode" Text="<%$resources:IFSCCode %>"
                                                    Enabled="false"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtIFSCCode" MaxLength="100" Enabled="false" CssClass="input-small"
                                                    TabIndex="54" />
                                                <asp:RequiredFieldValidator ID="rfvIFSCCode" CssClass="star" SetFocusOnError="true"
                                                    Enabled="false" ValidationGroup="Employee" EnableClientScript="true" runat="server"
                                                    ControlToValidate="txtIFSCCode" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_IFSCCode %>">
                                                </asp:RequiredFieldValidator>                                              
                                              
                                                <asp:Label runat="server" ID="lblBranch" AssociatedControlID="txtBranch" Enabled="false"
                                                    Text="<%$resources:Branch %>" CssClass="lbl-12-9perc"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtBranch" MaxLength="100" Enabled="false" CssClass="input-small-c"
                                                    TabIndex="55" />
                                                <div id="divBasicPay" runat="server" visible="<%$ resources:ConfigurationsRes,HrmsEmpMasterBasicSal %>">
                                                    <asp:Label runat="server" ID="lblBasicSalary" AssociatedControlID="txtBasicSalary"
                                                        CssClass="middle-lbl-d margnrgt-minus1" Enabled="false" Text="<%$resources:BasicSalary %>"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtBasicSalary" MaxLength="100" CssClass="input-small numeric"
                                                        TabIndex="55" />                                             
                                                </div>
                                                </div>
                                                
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divEmpTypeDtls" runat="server" class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("EmployeeType").ToString()%>
                                </h1>
                                <asp:ImageButton runat="server" ID="imbShowEmpType" OnClientClick="javascript:return ShowHideEmpType(1);"
                                    SkinID="imbArrowInactive" TabIndex="60" ToolTip="<%$ resources:ShowEmpType%>" />
                                <asp:ImageButton runat="server" ID="imbHideEmpType" TabIndex="60" OnClientClick="javascript:return ShowHideEmpType();"
                                    Style="display: none" SkinID="imbArrowActive" ToolTip="<%$ resources:HideEmpType%>" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divEmpTypeDetails" style="display: none">
                                <table class="table-devide tablelayout" runat="server" id="pnlEmpTypeDetails">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblEmployementType" AssociatedControlID="ddlEmployementType"
                                                    Text="<%$resources:EmployeeTypeReq %>"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlEmployementType" AutoPostBack="true" CssClass="select-w61per"
                                                    OnSelectedIndexChanged="ActionHandler" TabIndex="61">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvEmployementType" CssClass="star" SetFocusOnError="true"
                                                    InitialValue="-1" ValidationGroup="Employee" EnableClientScript="true" runat="server"
                                                    ControlToValidate="ddlEmployementType" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_EmployeeType %>"
                                                    Enabled ="<%$ resources:ConfigurationsRes,HrmsEmploymentTypeReq%>"></asp:RequiredFieldValidator>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblLeaveTemplate" AssociatedControlID="ddlLeaveTemplate"
                                                    Text="<%$ resources:LeaveTemplateReq %>">
                                                </asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlLeaveTemplate" CssClass="select-w61per" TabIndex="62">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvLeaveTemplate" CssClass="star" SetFocusOnError="true"
                                                    InitialValue="-1" ValidationGroup="Employee" EnableClientScript="true" runat="server"
                                                    ControlToValidate="ddlLeaveTemplate" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_LeaveTemplate %>"
                                                    Enabled ="<%$ resources:ConfigurationsRes,HrmsEmpLeaveTemplateReq%>">
                                                </asp:RequiredFieldValidator>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblOTAvailable" runat="server" Text="<%$ resources:OTAvailable%>"
                                                    AssociatedControlID="lblOTAvailable"></asp:Label>
                                                <asp:CheckBox runat="server" ID="chkOTAvailable" TabIndex="62" onclick="OTAvailableChange()" />
                                                <asp:Label runat="server" ID="lblOTTemplate" AssociatedControlID="ddlOTTemplate"
                                                    Text="<%$resources:OTTemplateReq %>" CssClass="lbl-22-6perc"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlOTTemplate" CssClass="select-small-f" TabIndex="63">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvOTTemplate" CssClass="star" SetFocusOnError="true"
                                                    Enabled="false" InitialValue="-1" ValidationGroup="Employee" EnableClientScript="true"
                                                    runat="server" ControlToValidate="ddlOTTemplate" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_OTTemplate %>">
                                                </asp:RequiredFieldValidator>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblWorkingDayType" AssociatedControlID="ddlWorkingDayType"
                                                    Text="<%$resources:WorkingDayTypeReq %>"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlWorkingDayType" CssClass="select-small-a2"
                                                    TabIndex="63" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                                    <%--onchange="WorkingDayTypeChange()"--%>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="vrfWorkingDayType" CssClass="star" SetFocusOnError="true"
                                                    InitialValue="-1" ValidationGroup="Employee" EnableClientScript="true" runat="server"
                                                    ControlToValidate="ddlWorkingDayType" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_WorkingDayType %>"
                                                    Enabled ="<%$ resources:ConfigurationsRes,HrmsEmpWorkingDayTypeReq%>">
                                                </asp:RequiredFieldValidator>
                                                <asp:Label ID="lblWorkingDays" runat="server" Text="<%$ resources:WorkingDays%>"
                                                    AssociatedControlID="txtWorkingDays" CssClass="lbl-18-9perc"></asp:Label>
                                                <asp:TextBox ID="txtWorkingDays" runat="server" MaxLength="4" TabIndex="64" CssClass="input-small numeric"> </asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfWorkingDays" runat="server" ControlToValidate="txtWorkingDays"
                                                    Display="Static" Enabled="false" CssClass="star" ValidationGroup="Employee" Text="*"
                                                    ErrorMessage="<%$ resources:Err_WorkingDays%>">
                                                </asp:RequiredFieldValidator>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblnworkinghrs" runat="server" Text="<%$ resources:WorkingHrs%>" AssociatedControlID="txtNormalWorkingHrs">
                                                <%= (Convert.ToBoolean(GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpNormalWorkingHrsReq")) ? GetLocalResourceObject("WorkingHrsReq").ToString() : GetLocalResourceObject("WorkingHrs").ToString())%></asp:Label>
                                                <asp:TextBox ID="txtNormalWorkingHrs" runat="server" MaxLength="4" TabIndex="64"
                                                    CssClass="input-small numeric"> </asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvNormalWrkngHrs" runat="server" ControlToValidate="txtNormalWorkingHrs"
                                                    Display="Static" Enabled="<%$ resources: ConfigurationsRes,HrmsEmpNormalWorkingHrsReq %>"
                                                    CssClass="star" ValidationGroup="Employee" Text="*" ErrorMessage="<%$ resources:Err_NormalWorkingHours%>">
                                                </asp:RequiredFieldValidator>
                                                <asp:CompareValidator ID="cmpNormalWorkingHrs" runat="server" ControlToValidate="txtNormalWorkingHrs"
                                                    Display="Static" CssClass="star" ValidationGroup="Employee" Text="*" Operator="DataTypeCheck"
                                                    Type="Integer" ErrorMessage="<%$ resources:Err_InvalidWorkingHrs%>">
                                                </asp:CompareValidator>
                                                <asp:Label ID="lblOTRate" runat="server" Text="<%$ resources:OTRate%>" AssociatedControlID="txtOTRate"
                                                    CssClass="lbl-17-5perc"></asp:Label>
                                                <asp:TextBox ID="txtOTRate" runat="server" MaxLength="15" TabIndex="64" CssClass="input-small numeric"> </asp:TextBox>
                                                <cc1:AmountValidation ID="vamOTRate" runat="server" ControlToValidate="txtOTRate"
                                                    ErrorMessage="<%$ resources:Err_InvalidOTRate %>" NumberDigits="11" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="Employee"></cc1:AmountValidation>
                                                <asp:Label ID="Label3" runat="server" Text="<%$ resources:BreakHours%>" AssociatedControlID="txtBreakTime"
                                                    CssClass="lbl-25-1perc"></asp:Label>
                                                <asp:TextBox ID="txtBreakTime" runat="server" MaxLength="15" TabIndex="64" CssClass="input-small numeric"></asp:TextBox>
                                                <asp:Label runat="server" Text="<%$ resources:Min%>" AssociatedControlID="txtBreakTime"
                                                    CssClass="lbl-3-7perc margntop4"></asp:Label>
                                                <asp:CompareValidator ID="cmpBreakTime" runat="server" ControlToValidate="txtBreakTime"
                                                    Display="Static" CssClass="star" ValidationGroup="Employee" Text="*" Operator="DataTypeCheck"
                                                    Type="Integer" ErrorMessage="<%$ resources:Err_InvalidBreakTime%>">
                                                </asp:CompareValidator>
                                                <asp:Label runat="server" Text="<%$ resources:OTFromPunching%>" ID="OTPending" AssociatedControlID="OTPending"
                                                    CssClass="lbl-32-2perc"></asp:Label>
                                                <asp:CheckBox runat="server" ID="chkOTPending" TabIndex="65" />
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" Text="<%$ resources:ConsiderLateHrs%>" ID="lblLateHrs"
                                                    AssociatedControlID="lblLateHrs"></asp:Label>
                                                <asp:CheckBox runat="server" ID="chkConsiderLateHrs" TabIndex="66" />
                                                <asp:Label runat="server" Text="ESI Required" ID="lblESIReq" AssociatedControlID="lblESIReq"></asp:Label>
                                                <asp:CheckBox runat="server" ID="chkESIReqrd" TabIndex="66" />
                                                <asp:Label runat="server" Text="PF Required" ID="lblPFReq" AssociatedControlID="lblPFReq"
                                                    CssClass="lbl-27perc"></asp:Label>
                                                <asp:CheckBox runat="server" ID="chkPFRequrd" TabIndex="66" />
                                                <asp:Label runat="server" Text="<%$ Resources: SSORequired %>" ID="lblSSOReq" AssociatedControlID="lblSSOReq"
                                                    CssClass="middle-lbl-d"></asp:Label>
                                                <asp:CheckBox runat="server" ID="chkSSOReq" TabIndex="66" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S gridwrap">
                                                <asp:GridView runat="server" ID="grdWorkingDays" Width="150" AllowPaging="false"
                                                    AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-CssClass="emptytable">
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$ resources:GridDay%> ">
                                                            <ItemTemplate>
                                                                <asp:HiddenField runat="server" ID="hdfWHrsPK" Value='<%# Eval("Pk") %>' />
                                                                <asp:HiddenField runat="server" ID="hdfWHrsEmpTypeID" Value='<%# Eval("EmployeeType") %>' />
                                                                <asp:HiddenField runat="server" ID="hdfWeekDay" Value='<%# Eval("WeekDay") %>' />
                                                                <asp:Label ID="lblWeekDay" runat="server" Text='<%# Eval("WeekDayTest") %>' ToolTip='<%# Eval("WeekDayTest") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="80%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$resources:GridHrs%>">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtHrs" runat="server" CssClass="small numeric" Text='<%# Eval("Hours") %>'
                                                                    onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);" TabIndex="67"
                                                                    MaxLength="4">
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="20%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divAdditionalInfo" runat="server" class="search-colapse-b">
                                <h1>
                                    Additional Information</h1>
                                <asp:ImageButton runat="server" ID="imbShowAdditionalInfo" OnClientClick="javascript:return ShowHideAdditionalInfo(1);"
                                    SkinID="imbArrowInactive" ToolTip="<%$ resources:ShowAdditionalInformationDetails %>"
                                    TabIndex="68" />
                                <asp:ImageButton runat="server" ID="imbHideAdditionalInfo" OnClientClick="javascript:return ShowHideAdditionalInfo();"
                                    Style="display: none" SkinID="imbArrowActive" TabIndex="68" ToolTip="<%$ resources:HideAdditionalInformationDetails%>" />
                                <asp:HiddenField ID="hdfIsContactInfoVisisble" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divHideShowAdditionalInfo" style="display: none">
                                <table class="table-devide" id="Table2">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblreligion" Text="<%$ resources:Religion%>" AssociatedControlID="txtreligion">
                                                <%= (Convert.ToBoolean(GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpReligionReq")) ? GetLocalResourceObject("ReligionReq").ToString() : GetLocalResourceObject("Religion").ToString())%>
                                                </asp:Label>
                                                <asp:TextBox runat="server" ID="txtreligion" Visible="false" Text="" MaxLength="100"
                                                    CssClass="input-small" TabIndex="69"></asp:TextBox>
                                                <asp:HiddenField ID="hdfReligion" Value="" runat="server" />
                                                <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlReligion" TabIndex="69"
                                                    CssClass="lbl-22-1perc" OnSelectedIndexChanged="ActionHandler">
                                                    <%--select-small-b--%>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvReligion" CssClass="star" SetFocusOnError="true"
                                                    InitialValue="-1" ValidationGroup="Employee" EnableClientScript="true" runat="server"
                                                    ControlToValidate="ddlReligion" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Religion %>"
                                                    Enabled="<%$ resources:ConfigurationsRes,HrmsEmpReligionReq %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:Label runat="server" ID="lblSubReligion" Text="<%$ resources:SubReligion%>"
                                                    CssClass="lbl-16perc" AssociatedControlID="txtSubReligion"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtSubReligion" Visible="false" Text="" MaxLength="100"
                                                    CssClass="input-small"></asp:TextBox>
                                                <asp:HiddenField ID="hdfSubReligion" Value="" runat="server" />
                                                <asp:DropDownList runat="server" TabIndex="69" ID="ddlSubReligion" CssClass="lbl-22-1perc">
                                                </asp:DropDownList>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblProfession" Text="<%$ resources:Profession%>" AssociatedControlID="txtProfession"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtProfession" Text="" TabIndex="70" MaxLength="100"
                                                    CssClass="middle-lbl-small-b2"></asp:TextBox>
                                                <asp:HiddenField ID="hdfProfession" Value="" runat="server" />
                                                <asp:Label runat="server" ID="lblMaritalStatus" Text="<%$ resources:MaritalStatus%>"
                                                    AssociatedControlID="ddlMaritalStatus" CssClass="lbl-16perc"></asp:Label>
                                                <asp:DropDownList ID="ddlMaritalStatus" runat="server" TabIndex="70" CssClass="lbl-22-1perc"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ActionHandler" onchange="javascript:ChangeMaritalStatus();">
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hdfMaritalStatus" runat="server" Value="0" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" InitialValue="-1" CssClass="star"
                                                    SetFocusOnError="true" ValidationGroup="Employee" EnableClientScript="true" runat="server"
                                                    ControlToValidate="ddlMaritalStatus" Display="Static" Text="*" ErrorMessage="Select Marital Status"
                                                    Enabled ="<%$ resources:ConfigurationsRes,HrmsEmpMaritalStatusReq%>"></asp:RequiredFieldValidator>
                                               
                                                <%--  <asp:DropDownList ID="ddlProfession" runat="server" CssClass="medium" TabIndex="42">
                                                </asp:DropDownList>--%>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblFatherName" runat="server" Text="<%$ resources:FatherName%>" AssociatedControlID="TxtFatherName"></asp:Label>
                                                <asp:TextBox ID="TxtFatherName" onkeypress="return isAlphabet(event)" runat="server"
                                                    CssClass="input-small-b" TabIndex="71" MaxLength="100"></asp:TextBox>
                                                <asp:Label runat="server" ID="lblFIDno" Text="<%$ resources:IDNumber%>" AssociatedControlID="txtFIdNumber"
                                                    CssClass="lbl-16-4perc"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtFIdNumber" Text="" TabIndex="71" MaxLength="100"
                                                    CssClass="input-small-b"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblSpouseName" runat="server" Text="<%$ resources:SpouseName%>" AssociatedControlID="TxtSpouseName"></asp:Label>
                                                <asp:TextBox ID="TxtSpouseName" MaxLength="100" onkeypress="return isAlphabet(event)"
                                                    runat="server" TabIndex="72" CssClass="input-small-b"></asp:TextBox>
                                                <asp:Label runat="server" ID="lblSIdNo" Text="<%$ resources:IDNumber%>" AssociatedControlID="txtSIdNumber"
                                                    CssClass="lbl-16-4perc"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtSIdNumber" Text="" TabIndex="72" MaxLength="100"
                                                    CssClass="input-small-b"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblFatherSpouse" runat="server" Text="<%$ resources:FatherOfSpouse%>"
                                                    AssociatedControlID="txtFatherSpouse"></asp:Label>
                                                <asp:TextBox ID="txtFatherSpouse" MaxLength="100" onkeypress="return isAlphabet(event)"
                                                    runat="server" TabIndex="73" CssClass="input-small-b"></asp:TextBox>
                                                <asp:Label runat="server" ID="lblIDFatherSpouse" Text="<%$ resources:IDNumber%>"
                                                    AssociatedControlID="txtFatherSpouseIdNo" CssClass="lbl-16-4perc"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtFatherSpouseIdNo" Text="" TabIndex="73" MaxLength="100"
                                                    CssClass="input-small-b"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="Label6" Text="<%$ resources:IncomeTaxNo%>" AssociatedControlID="txtIncomeTaxNo"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtIncomeTaxNo" CssClass="input-small-b" Text=""
                                                    TabIndex="74"></asp:TextBox>
                                                <asp:Label runat="server" ID="lblNoOfchildren" Text="<%$ resources:NoOfchildren%>"
                                                    AssociatedControlID="txtNoOfchildren" CssClass="lbl-16-4perc"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtNoOfchildren" CssClass="input-small-b" onkeypress="return isNumber(event)"
                                                    Text="" TabIndex="74" MaxLength="2"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="Label8" Text="Tax Payer" AssociatedControlID="chkTaxpayer"></asp:Label>
                                                <asp:CheckBox ID="chkTaxpayer" TabIndex="75" runat="server" />
                                                <asp:Label runat="server" ID="Label10" Text="<%$ resources:NoOfChildEdu%>" AssociatedControlID="txtNoOfchildrenEdu"
                                                    CssClass="lbl-35-8perc"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtNoOfchildrenEdu" CssClass="input-small-b" onkeypress="return isNumber(event)"
                                                    Text="" TabIndex="75" MaxLength="2"></asp:TextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblCountryOfBirth" Text="<%$ resources:CountryOfBirth%>"
                                                    AssociatedControlID="txtCountryOfBirth"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtCountryOfBirth" Text="" TabIndex="69" MaxLength="100"
                                                    CssClass="input-small-b"></asp:TextBox>
                                                <asp:HiddenField ID="hdfCountryOfBirth" Value="" runat="server" />
                                                <asp:DropDownList ID="ddlCountryOfBirth" Visible="false" runat="server" CssClass="medium">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblPlaceOfBirth" runat="server" Text="<%$ resources:PlaceOfBirth%>"
                                                    AssociatedControlID="TxtPlaceOfBirth" CssClass="lbl-19-6perc"></asp:Label>
                                                <asp:TextBox ID="TxtPlaceOfBirth" MaxLength="60" onkeypress="return isAlphabet(event)"
                                                    runat="server" TabIndex="69" CssClass="input-small"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblBloodGroup" Text="<%$ resources:BloodGroup%>" AssociatedControlID="ddlBloodGroup"></asp:Label>
                                                <asp:DropDownList ID="ddlBloodGroup" runat="server" CssClass="lbl-21-7perc" TabIndex="70">
                                                </asp:DropDownList>
                                                <asp:Label runat="server" ID="lblAltContactNO" Text="<%$ resources:EmergencyCont.No.%>"
                                                    AssociatedControlID="txtAltContactNO" CssClass="lbl-19-6perc">
                                                    <%=(Convert.ToBoolean(GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpEmergencyContReq")) ? GetLocalResourceObject("EmergencyCont.NoReq").ToString() : GetLocalResourceObject("EmergencyCont.No.").ToString())%>
                                                </asp:Label>
                                                <asp:TextBox runat="server" ID="txtAltContactNO" onkeydown="return numbersonlywithCopypaste(event);"
                                                    CssClass="input-small" Text="" TabIndex="70" MaxLength="18"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfAltContactNO" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="Employee" EnableClientScript="true" runat="server" ControlToValidate="txtAltContactNO"
                                                    Display="Static" Text="*" ErrorMessage="<%$ resources:EnterEmergencyCont%>" Enabled="<%$ resources:ConfigurationsRes,HrmsEmpEmergencyContReq%>">
                                                </asp:RequiredFieldValidator>
                                                <asp:Label runat="server" ID="lblMotherName" Text="<%$ resources:MotherName%>" AssociatedControlID="txtMotherName"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtMotherName" onkeypress="return isAlphabet(event)"
                                                    Text="" TabIndex="71" MaxLength="100" CssClass="input-small-b"></asp:TextBox>
                                                <asp:Label runat="server" ID="lblMIdNo" Text="<%$ resources:IDNumber%>" AssociatedControlID="txtMIdNumber"
                                                    CssClass="lbl-19-6perc"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtMIdNumber" Text="" TabIndex="71" MaxLength="100"
                                                    CssClass="lbl-18-5perc"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblIsSpouseWorking" Text="<%$ resources:SpouseWorking%>"
                                                    AssociatedControlID="lblIsSpouseWorking"></asp:Label>
                                                <asp:CheckBox ID="chkIsSpouseWorking" TabIndex="72" runat="server" />
                                                <asp:Label ID="lblSpouseSurname" runat="server" Text="<%$ resources:SpouseSurname%>"
                                                    AssociatedControlID="txtSpouseSurname" CssClass="lbl-39-1perc"></asp:Label>
                                                <asp:TextBox ID="txtSpouseSurname" MaxLength="100" onkeypress="return isAlphabet(event)"
                                                    runat="server" TabIndex="72" CssClass="lbl-18-3perc"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="Label4" runat="server" Text="<%$ resources:MotherOfSpouse%>" AssociatedControlID="txtMothrSpouse"></asp:Label>
                                                <asp:TextBox ID="txtMothrSpouse" MaxLength="100" onkeypress="return isAlphabet(event)"
                                                    runat="server" TabIndex="73" CssClass="input-small-b"></asp:TextBox>
                                                <asp:Label runat="server" ID="Label5" Text="<%$ resources:IDNumber%>" AssociatedControlID="txtMothrSpouseIdNo"
                                                    CssClass="lbl-19-6perc"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtMothrSpouseIdNo" Text="" TabIndex="73" MaxLength="100"
                                                    CssClass="lbl-18-5perc"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="Label7" Text="<%$ resources:IDNos%>" AssociatedControlID="txtNoOfChildrenId"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtNoOfChildrenId" Text="" TabIndex="74" MaxLength="100"
                                                    CssClass="select-w61per"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="Label9" Text="<%$ resources:IDNos%>" AssociatedControlID="txtNoOfChildEduId"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtNoOfChildEduId" Text="" TabIndex="75" MaxLength="100"
                                                    CssClass="select-w61per"></asp:TextBox>
                                            </div>
                                        </td>
                                        <asp:HiddenField ID="hdflastModifiedDate" runat="server" />
                                    </tr>
                                </table>
                            </div>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <span style="float: right !important;" id="lblLastModifiedDate" runat="server"></span>
                            <%-- <asp:Label ID="lblLastModifiedDate"  runat="server"></asp:Label>--%>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star">
                    </asp:Label>
                    <asp:ValidationSummary ID="vsEmployeePage" ValidationGroup="Employee" runat="server" />
                    <asp:ValidationSummary ID="vsSaveBranch" ValidationGroup="SaveBranch" runat="server" />
                    <asp:ValidationSummary ID="vsSaveStatus" ValidationGroup="SaveStatus" runat="server" />
                    <asp:ValidationSummary ID="vsSaveDepartment" ValidationGroup="SaveDepartment" runat="server" />
                    <asp:ValidationSummary ID="vsSaveDesignation" ValidationGroup="SaveDesignation" runat="server" />
                    <asp:ValidationSummary ID="vsEmpymntTyeSave" ValidationGroup="EmpymntTyeSave" runat="server" />
                </div>
            </div>
            <%--------------------------------Employee Document Popup Start---------------------------------------%>
            <div id="divEmpDoc" style="display: none; overflow: auto;">
                <uc1:EmpDocument ID="ucEmpDocument" runat="server" ParentPage="EmployeeMaster" />
            </div>
            <%--------------------------------Employee Document Popup End-----------------------------------------%>
            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormatWithComma" runat="server" />
            <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIsLeaveExcYes" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsSaveYesNo" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsDocConfrmYes" runat="server" Value="0" />
            <asp:HiddenField ID="hdfDocConfrmMsg" runat="server" Value="" />
            <asp:HiddenField ID="hdfSSORequired" runat="server" Value="0" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
            <asp:PostBackTrigger ControlID="btnSaveContinue" />
            <asp:PostBackTrigger ControlID="ddlReligion" />
            <asp:PostBackTrigger ControlID="btnImageUpoad" />
            <asp:PostBackTrigger ControlID="ddlPaymentMode" />
            <asp:PostBackTrigger ControlID="ddlEmployementType" />
            <asp:PostBackTrigger ControlID="ddlBankName" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
