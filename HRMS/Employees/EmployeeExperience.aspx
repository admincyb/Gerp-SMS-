<%@ Page Title="<%$ resources:HRMS-EmployeeExperience%>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="EmployeeExperience.aspx.cs" Inherits="HRMS.Employees.EmployeeExperience"
    Theme="ClassicExt" %>

<%@ Register Src="UserControls/GtiTabControl.ascx" TagName="GtiTabControl" TagPrefix="ucGtiTab" %>
<%@ Register Src="UserControls/EmpBasicInfoControl.ascx" TagName="EmpBasicInfoControl"
    TagPrefix="ucBasicHdr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">


        //        $(document).ready(function () {
        //            ShowListingTab($("[id$=hdnTabStatus]").val());
        //            InitDate();
        //            InitComponents();
        //        });

        function EndRequestHandlerPage() {
            ShowListingTab($("[id$=hdnTabStatus]").val());
            InitDate();
            InitComponents();
        }

        function InitDate() {
            //            GrandScriptUtils.DatePickerCommon("txtPeriodfrom", null, null, true, null, new Date());
            //            GrandScriptUtils.DatePickerCommon("txtTo");
            GrandScriptUtils.AddDateRangeCommon("txtPeriodfrom", "hdfPeriodfrom", "txtTo", "hdfTo", false, true, false, new Date());
            GrandScriptUtils.DatePickerCommon("txtTo", "dd-M-yy", null, true, null, new Date(), null, null);


        }

        function ShowListingTab(flag) {
            $("[id$=hdnTabStatus]").val(flag);
            var entryStatus = $("[id$=hdfEntryStatus]").val();
            if (flag == '1') {
                $("[id$=PageAction_List]").show();
                $("[id$=pnlCancelList]").show();
                if (entryStatus != "1") {
                    $("[id$=pnlEdit]").show();
                    $("[id$=pnlNew]").show();
                }
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlSave]").hide();
                $("[id$=pnlSaveAndContinue]").hide();
                $("[id$=pnlCancel]").hide();              
                $("[id$=pnlDelete]").hide();
                $("[id$=lnkList]").addClass('tab-active');
                $("[id$=lnkList]").removeClass('tab-inactive');
                $("[id$=lnkDetail]").addClass('tab-inactive');

            }
            else if (flag == '0') {
                $("[id$=PageAction_Entry]").show();
                if (entryStatus != "1") {
                    $("[id$=pnlSaveAndContinue]").show();
                    $("[id$=pnlSave]").show();
                    $("[id$=pnlDelete]").show();
                }
                $("[id$=pnlCancel]").show();
                $("[id$=PageAction_List]").hide();             
                $("[id$=pnlCancelList]").hide();
                $("[id$=pnlEdit]").hide();
                $("[id$=pnlNew]").hide();
                $("[id$=lnkList]").addClass('tab-inactive');
                $("[id$=lnkDetail]").addClass('tab-active');
                $("[id$=lnkDetail]").removeClass('tab-inactive');


            }

            return false;
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

        //Validate Salary

        function isSalary(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 46 && (charCode < 48 || charCode > 57)) {
                return false;
            }

            return true;
        }


        /// AutoComplete textbox
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {

            GrandScriptUtils.MakeAutoCompleteDDL("txtCountry", url, "hdfCountry", true, true, "COUNTRYFIRST");
            GrandScriptUtils.MakeAutoCompleteDDL("txtExitReason", url, "hdfExitReason", true, true, "EXITREASON");
            InitDate();
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


        function ShowHideHRbDetails(flag) {
            ///<summary>
            /// Used to Show/Hide ItemDetails Div
            ///</summary>

            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divHRDetails]").show();
                $("[id$=imbShowHRDetails]").hide();
                $("[id$=imbHideHRDetails]").show();
            }
            else {
                $("[id$=divHRDetails]").hide();
                $("[id$=imbShowHRDetails]").show();
                $("[id$=imbHideHRDetails]").hide();
            }
            $("[id$=hdfIsItemDetailsVisible]").val(flag);
            return false;
        }

        function ShowHideContactDetails(flag) {
            ///<summary>
            /// Used to Show/Hide ItemDetails Div
            ///</summary>

            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divContactDetails]").show();
                $("[id$=imbShowContactDetails]").hide();
                $("[id$=imbHideContactDetails]").show();
            }
            else {
                $("[id$=divContactDetails]").hide();
                $("[id$=imbShowContactDetails]").show();
                $("[id$=imbHideContactDetails]").hide();
            }
            $("[id$=hdfIsItemDetailsVisible]").val(flag);
            return false;
        }

        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode           
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlSaveAndContinue]").hide();
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlNew]").hide();
                $("[id$=pnlEdit]").hide();
                $("[id$=btnAddItem]").hide();                            
            }
        }

    </script>
    <style type="text/css">
        .divcol-S label
        {
            max-width: 136px;
        }
        
        .exp-lbl
        {
            margin: 0px 8px 10px -4px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <ucGtiTab:GtiTabControl ID="hrmsTab" runat="server" CurrentTab="4" />
                <div class="Button-container">
                    <asp:Table ID="Table3" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" Text="<%$ resources:Breadcrumb%>" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlSaveAndContinue">
                                        <asp:Button runat="server" ID="btnSaveContinue" OnClick="ActionHandler" CommandName="SAVEANDCONTINUE"
                                            TabIndex="35" Text="Save & Continue" ToolTip="Save & Continue" ValidationGroup="Employee"
                                            OnClientClick="javascript:return ValidatePageNow('Experience')" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" OnClick="ActionHandler" CommandName="SAVE"
                                            TabIndex="36" Text="<%$ resources:Save%>" ToolTip="<%$ resources:Save%>" ValidationGroup="Experience"
                                            OnClientClick="javascript:return ValidatePageNow('Experience')" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button ID="btnDelete" runat="server" Visible="true" SkinID="btnInner-Delete"
                                            Text="<%$Resources:Controls,Delete%>" OnClientClick="return ShowDeleteConfirm(this);"
                                            CommandName="DELETE" OnClick="ActionHandler" ToolTip="Delete" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" OnClick="ActionHandler" Text="<%$ resources:Cancel%>"
                                            ToolTip="<%$ resources:Cancel%>" CommandName="CANCEL" TabIndex="37" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" />
                                    </li>
                                    <li runat="server" id="pnlNew">
                                        <asp:Button runat="server" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$ resources:New%>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$ resources:New%>" />
                                    </li>
                                    <li runat="server" id="pnlEdit">
                                        <asp:Button ID="btnEdit" runat="server" SkinID="btnInner-Edit" Text="<%$Resources:Controls,Edit%>"
                                            CommandName="EDIT" OnClick="ActionHandler" ToolTip="Edit" />
                                    </li>
                                    <%--           <li runat="server" id="pnlView">
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>--%>
                                    <li runat="server" id="pnlCancelList">
                                        <asp:Button runat="server" ID="btnCancelList" OnClick="ActionHandler" Text="Cancel"
                                            ToolTip="Cancel" CommandName="CANCELTOLIST" TabIndex="64" CommandArgument="SEC_ActionPanel"
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
                            Text="<%$ resources:EmployeeNo:%>"></asp:Label>
                        <asp:Label ID="lblhdrEmployeeNoTxt" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblhdrEmployeeName" runat="server" AssociatedControlID="lblhdrEmployeeNameTxt"
                            Text="<%$ resources:EmployeeName:%>"></asp:Label>
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
                <asp:HiddenField ID="hdfSelectedItemPk" runat="server" Value="0" />
                <div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="List" CommandArgument="SEC_ActionPanel"
                                CommandName="EMPDOCLIST" OnClick="ActionHandler" CssClass="tab-active" Style="margin-top: -7px;"></asp:LinkButton>
                        </li>
                        <%--OnClientClick="javascript:return ShowListingTab(1)" --%>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="Detail" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CommandName="EMPDOCDETAIL" CssClass="tab-inactive" Style="margin-top: -7px;"></asp:LinkButton>
                            <%--OnClientClick="javascript:return ShowListingTab(0)"--%>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="gridwrap" style="margin-top: -5px;">
                                <asp:GridView ID="grdEmployeeExperiencelist" runat="server" AutoGenerateColumns="False"
                                    AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="true"
                                    OnSorting="ActionHandler" Width="100%">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    OnCheckedChanged="ActionHandler" />
                                                <asp:HiddenField runat="server" ID="hdfExperncePk" Value='<%# Eval("EED_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Company%> " SortExpression="EED_EMPLOYER">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcmpny" runat="server" ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EED_EMPLOYER")))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EED_EMPLOYER")),60) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Designation%> " SortExpression="EED_DESIGNATION">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDesgntn" runat="server" ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EED_DESIGNATION")))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EED_DESIGNATION")),60) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="35%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Period%> " SortExpression="EED_PERIOD_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPerd" runat="server" Text=' <%# Eval("EED_PERIOD_TEXT").ToString() == "01-Jan-1900 - 01-Jan-1900" ? null : Eval("EED_PERIOD_FROM",Resources.Constants.HRMSDateFormatGrid)
                                                 +" - " +Eval("EED_PERIOD_TO",Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Years/Months " SortExpression="EED_YEARS">
                                            <ItemTemplate>
                                                <asp:Label ID="lblYears" runat="server" ToolTip='<%# Eval("EED_YEARS") %>' Text='<%# Eval("EED_YEARS") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <%--<uc1:PagerControl ID="uclPaging" runat="server" />--%>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide" runat="server" id="pnlEmpDocDetails">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCompany" runat="server" Text="<%$ resources:Company/Employer*%>"
                                                AssociatedControlID="txtCompany"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtCompany" TabIndex="1" MaxLength="200" CssClass="input-half"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfCompany" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Experience" EnableClientScript="true" runat="server" ControlToValidate="txtCompany"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:EnterCompanyName%>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblDesignation" AssociatedControlID="txtDesignation"
                                                Text="<%$ resources:Designation*%>"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDesignation" TabIndex="2" MaxLength="100" CssClass="input-half" />
                                            <asp:RequiredFieldValidator ID="vrfDesignation" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Experience" EnableClientScript="true" runat="server" ControlToValidate="txtDesignation"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:EnterDesignation%>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblPeriodfrom" AssociatedControlID="txtPeriodfrom"
                                                Text="<%$ resources:Periodfrom%>"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPeriodfrom" TabIndex="3" CssClass="input-small"
                                                MaxLength="15" onkeydown="return CheckKey(event)" onpaste="return false;" />
                                            <asp:HiddenField ID="hdfPeriodfrom" runat="server" Value="" />
                                            <asp:Label runat="server" ID="lblTo" AssociatedControlID="txtTo" Text="<%$ resources:To%>"
                                                CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTo" TabIndex="4" CssClass="input-small" MaxLength="15"
                                                onkeydown="return CheckKey(event)" onpaste="return false;" />
                                            <asp:HiddenField ID="hdfTo" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblNatureOfJob" AssociatedControlID="txtNatureOfJob"
                                                Text="<%$ resources:NatureOfJob%>"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtNatureOfJob" TabIndex="5" MaxLength="100" CssClass="input-half" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblExitReason" AssociatedControlID="txtExitReason"
                                                Text="<%$ resources:Exitreason*%>"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtExitReason" Text="" TabIndex="6" MaxLength="100"
                                                CssClass="select-small-a"></asp:TextBox>
                                            <asp:HiddenField ID="hdfExitReason" Value="" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfExitReason" InitialValue="Select/Type" CssClass="star"
                                                SetFocusOnError="true" ValidationGroup="Experience" EnableClientScript="true"
                                                runat="server" ControlToValidate="txtExitReason" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:SelectExitReason%>"></asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblSalary" AssociatedControlID="txtSalary" Text="<%$ resources:Salary%>"
                                                CssClass="middle-lbl-xsmall-c2"></asp:Label>
                                            <asp:DropDownList ID="ddlCurrency" TabIndex="7" runat="server" CssClass="select-w10-2per">
                                            </asp:DropDownList>
                                            <asp:TextBox runat="server" ID="txtSalary" onkeypress="return isSalary(event)" Text=""
                                                TabIndex="8" MaxLength="12" CssClass="input-small"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblPhone" AssociatedControlID="txtPhone" Text="<%$ resources:Phone%>"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPhone" TabIndex="9" MaxLength="18" onkeydown="return numbersonlywithCopypaste(event);"
                                                CssClass="input-small" />
                                            <%--  onkeypress="return isNumberWithSpclCharacters(event)"--%>
                                            <asp:Label runat="server" ID="lblMobile" AssociatedControlID="txtMobile" Text="<%$ resources:Mobile%>"
                                                CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtMobile" Text="" TabIndex="10" MaxLength="18" onkeydown="return numbersonlywithCopypaste(event);"
                                                CssClass="input-small"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblCity" AssociatedControlID="txtCity" Text="<%$ resources:City%>"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtCity" TabIndex="11" MaxLength="200" CssClass="input-small" />
                                            <asp:Label runat="server" ID="lblState" AssociatedControlID="txtState" Text="<%$ resources:State%>"
                                                CssClass="lbl-10-7perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtState" Text="" TabIndex="11" MaxLength="200" CssClass="input-w29per"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblCountry" AssociatedControlID="txtCountry" Text="<%$ resources:Country%>"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtCountry" Text="" TabIndex="12" MaxLength="20"
                                                CssClass="select-small-c"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCountry" Value="" runat="server" />
                                            <asp:Label runat="server" ID="lblZipCode" AssociatedControlID="txtZipCode" Text="<%$ resources:ZipCode%>"
                                                CssClass="middle-lbl-xsmall-a2"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtZipCode" pattern="[.,]+" onkeydown="return numbersonlywithCopypaste(event);"
                                                Text="" TabIndex="13" MaxLength="10" CssClass="input-small"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblJobDescription" runat="server" Text="<%$ resources:JobDescription%>"
                                                AssociatedControlID="txtJobDescription" CssClass="margn-rgt0"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtJobDescription" MaxLength="500" TabIndex="14"
                                                TextMode="MultiLine" CssClass="multiline-2col" onkeydown="limitText(this,500);"
                                                onkeyup="limitText(this,500);" onpaste="limitText(this,500);"></asp:TextBox>
                                        </div>
                                        <div class="clear">
                                        </div>
                                        <div class="divcol-S">
                                            <asp:Label ID="lblRemarks" runat="server" Text="<%$ resources:Remarks%>" AssociatedControlID="txtRemarks"
                                                CssClass="margn-rgt0"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRemarks" MaxLength="200" TabIndex="15" TextMode="MultiLine"
                                                CssClass="multiline-2col" onkeydown="limitText(this,200);" onkeyup="limitText(this,200);"
                                                onpaste="limitText(this,200);"></asp:TextBox>
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblFileUpload" runat="server" Text="Attach File" AssociatedControlID="fupUpload"
                                                CssClass="margn-rgt0"></asp:Label>
                                            <asp:FileUpload ID="fupUpload" runat="server" TabIndex="16" Style="width: 15.6%;" />
                                            <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="uploads" EnableClientScript="true" runat="server" ControlToValidate="fupUpload"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:SelectAnyFile%>">
                                            </asp:RequiredFieldValidator>
                                            <a id="anchorFile" runat="server" target="_blank"></a>
                                            <asp:Button runat="server" ID="btnAddItem" CommandName="ADDITEM" TabIndex="17" OnClick="ActionHandler"
                                                OnClientClick="javascript:return ValidatePageNow('uploads')" ToolTip="<%$ resources:FAdd%>"
                                                CommandArgument="PageAction_Entry" ValidationGroup="uploads" Text="<%$ resources:FAdd%>"
                                                SkinID="btnInner-add" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="gridwrap">
                                            <asp:GridView runat="server" ID="grdUploads" Width="100%" DataKeyNames="DOC_PK" AllowSorting="false"
                                                AllowPaging="false" OnSorting="ActionHandler" OnPageIndexChanging="ActionHandler"
                                                OnRowDataBound="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                                <%-- OnSorting="ActionHandler"
                                                OnPageIndexChanging="ActionHandler" OnRowDataBound="ActionHandler" --%>
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sl No">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                            <asp:HiddenField runat="server" ID="hdfPK" Value='<%# Eval("DOC_PK") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfSlNo" Value='<%# Container.DataItemIndex + 1 %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="4%" HorizontalAlign="Center" Wrap="false" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="File">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFile" runat="server" Text='<%# Eval("DOC_NAME") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="94%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-CssClass="file-details">
                                                        <ItemTemplate>
                                                            <a runat="server" id="fileView" class="download-icon nomargin" title="View" target="_blank"
                                                                href='<%# Page.ResolveClientUrl(Eval("DOC_PATH").ToString()) %>'></a>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false" ItemStyle-CssClass="file-details">
                                                        <ItemTemplate>
                                                            <asp:Button ID="lnkEdit" runat="server" CommandName="EDITITEM" SkinID="edit-icon"
                                                                ToolTip="Edit" OnClick="ActionHandler" CommandArgument="PageAction_Entry" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-CssClass="file-details">
                                                        <ItemTemplate>
                                                            <asp:Button ID="lnkRemove" runat="server" CommandName="REMOVEITEM" SkinID="delete-icon"
                                                                ToolTip="Delete" CommandArgument="PageAction_Entry" OnClick="ActionHandler" OnClientClick="return ShowDeleteConfirm(this);"
                                                                OnPreRender="btnAction_PreRender" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="3%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="search-colapse-b">
                                <h1>
                                    HR Details</h1>
                                <asp:ImageButton runat="server" ID="imbShowHRDetails" OnClientClick="javascript:return ShowHideHRbDetails(1);"
                                    SkinID="imbArrowInactive" ToolTip="<%$ resources:ShowDetails%> " TabIndex="18" />
                                <asp:ImageButton runat="server" ID="imbHideHRDetails" OnClientClick="javascript:return ShowHideHRbDetails();"
                                    Style="display: none" SkinID="imbArrowActive" TabIndex="18" ToolTip="<%$ resources:HideDetails%>" />
                                <asp:HiddenField ID="hdfIsItemDetailsVisible" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divHRDetails" style="display: none">
                                <table class="table-devide" id="tblDetails">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblHRName" runat="server" Text="<%$ resources:HRName%>" AssociatedControlID="txtHRName"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtHRName" Text="" TabIndex="19" MaxLength="200"
                                                    CssClass="input-half"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblHREmail" runat="server" Text="<%$ resources:HREmail%>" AssociatedControlID="txtHREmail"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtHREmail" Text="" TabIndex="21" MaxLength="200"
                                                    CssClass="input-small-c"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="vreHREmail" runat="server" ControlToValidate="txtHREmail"
                                                    ErrorMessage="<%$ resources:EnterValidHRMail%>" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                    Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="Experience" />
                                                <asp:Label ID="lblHRmobile1" runat="server" Text="<%$ resources:Mobile1%>" AssociatedControlID="txtHRmobile1"
                                                    CssClass="lbl-14-7perc"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtHRmobile1" onkeydown="return numbersonlywithCopypaste(event);"
                                                    Text="" TabIndex="22" MaxLength="18" CssClass="input-small"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblHRDesignation" runat="server" Text="<%$ resources:HRDesignation%>"
                                                    AssociatedControlID="txtHRDesignation"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtHRDesignation" Text="" TabIndex="20" MaxLength="200"
                                                    CssClass="input-half"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblHRMobile2" runat="server" Text="<%$ resources:HRMobile2%>" AssociatedControlID="txtHRMobile2"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtHRMobile2" CssClass="input-small" onkeydown="return numbersonlywithCopypaste(event);"
                                                    Text="" TabIndex="23" MaxLength="18"></asp:TextBox>
                                                <asp:Label ID="lblHRPhone" runat="server" Text="<%$ resources:HRPhone%>" AssociatedControlID="txtHRPhone"
                                                    CssClass="lbl-11-3perc"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtHRPhone" CssClass="medium" onkeydown="return numbersonlywithCopypaste(event);"
                                                    Text="" TabIndex="24" MaxLength="18"></asp:TextBox>
                                                <asp:TextBox runat="server" ID="txtHRExtension" placeholder="<%$ resources:HRext%>"
                                                    CssClass="small" onkeydown="return numbersonlywithCopypaste(event);" Text=""
                                                    TabIndex="24" MaxLength="12"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    Contact details for reference Verification
                                </h1>
                                <asp:ImageButton runat="server" ID="imbShowContactDetails" OnClientClick="javascript:return ShowHideContactDetails(1);"
                                    SkinID="imbArrowInactive" ToolTip="<%$ resources:ShowDetails%>" TabIndex="25" />
                                <asp:ImageButton runat="server" ID="imbHideContactDetails" OnClientClick="javascript:return ShowHideContactDetails();"
                                    Style="display: none" SkinID="imbArrowActive" TabIndex="25" ToolTip="<%$ resources:HideDetails%>" />
                                <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divContactDetails" style="display: none">
                                <table class="table-devide" id="Table1">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblVerName" runat="server" Text="<%$ resources:VName%>" AssociatedControlID="txtVerName"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtVerName" Text="" TabIndex="26" MaxLength="200"
                                                    CssClass="input-half"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblVerEmail" runat="server" Text="<%$ resources:vEmail%>" AssociatedControlID="txtVerEmail"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtVerEmail" Text="" TabIndex="28" MaxLength="200"
                                                    CssClass="input-small-c"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="vreVerMail" runat="server" ControlToValidate="txtVerEmail"
                                                    ErrorMessage="<%$ resources:EnterValidReferenceMail%>" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                    Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="Experience" />
                                                <asp:Label ID="lblVerMobile1" runat="server" Text="<%$ resources:VMobile1%>" AssociatedControlID="txtVermobile1"
                                                    CssClass="lbl-14-7perc"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtVermobile1" onkeydown="return numbersonlywithCopypaste(event);"
                                                    Text="" TabIndex="29" MaxLength="18" CssClass="input-small"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblVerDesignation" runat="server" Text="<%$ resources:VDesignation%>"
                                                    AssociatedControlID="txtVerDesignation"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtVerDesignation" Text="" TabIndex="27" MaxLength="200"
                                                    CssClass="input-half"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblVerMobile2" runat="server" Text="<%$ resources:VMobile2%>" AssociatedControlID="txtVerMobile2"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtVerMobile2" CssClass="input-small" onkeydown="return numbersonlywithCopypaste(event);"
                                                    Text="" TabIndex="30" MaxLength="18"></asp:TextBox>
                                                <asp:Label ID="lblVerPhone" runat="server" Text="<%$ resources:VPhone%>" AssociatedControlID="txtVerPhone"
                                                    CssClass="lbl-11-3perc"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtVerPhone" CssClass="medium" onkeydown="return numbersonlywithCopypaste(event);"
                                                    Text="" TabIndex="31" MaxLength="18"></asp:TextBox>
                                                <asp:TextBox runat="server" ID="txtVerExt" placeholder="<%$ resources:Vext%>" CssClass="small"
                                                    onkeydown="return numbersonlywithCopypaste(event);" Text="" TabIndex="32" MaxLength="12"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table>
                                <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                                    <asp:TableCell>
                                        <span style="float: right !important;" id="lblLastModifiedDate" runat="server"></span>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </table>
                            <asp:HiddenField ID="hdfEmployeeEDOB" runat="server" Value="" />
                            <div id="diverror" style="display: none">
                                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                                <asp:ValidationSummary ID="vsPageSave" ValidationGroup="Experience" runat="server" />
                                <asp:ValidationSummary ID="vsImageUpload" ValidationGroup="uploads" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <asp:HiddenField ID="hdnTabStatus" runat="server" Value="0" />
            <asp:HiddenField ID="hdfEntryStatus" runat="server" Value="0" />

        </ContentTemplate>
        <Triggers>
            <%-- <asp:AsyncPostBackTrigger ControlID="btnAddItem" />--%>
            <asp:PostBackTrigger ControlID="btnAddItem" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
