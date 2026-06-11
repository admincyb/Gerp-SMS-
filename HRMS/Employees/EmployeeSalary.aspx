<%@ Page Title="<%$ resources:HRMS-EmployeeSalary%>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="EmployeeSalary.aspx.cs" Inherits="HRMS.Employees.EmployeeSalary"
    Theme="ClassicExt" %>

<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Src="UserControls/GtiTabControl.ascx" TagName="GtiTabControl" TagPrefix="ucGtiTab" %>
<%@ Register Src="UserControls/EmpSalaryControl.ascx" TagName="EmpSalaryControl"
    TagPrefix="ucgti" %>
<%@ Register Src="UserControls/EmpBasicInfoControl.ascx" TagName="EmpBasicInfoControl"
    TagPrefix="ucBasicHdr" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript">
        function InitComponents() {
            $("[id*=txtEmpBasic]").ForceNumericOnly();
            $("[id*=txtEarnAmount]").ForceNumericOnly();
            $("[id*=txtDeductAmount]").ForceNumericOnly();
            GrandScriptUtils.DatePickerCommon("txtEffectiveDate");
            GrandScriptUtils.DatePickerCommon("txtEffectiveFromDate");
            //GrandScriptUtils.AddDateRangeCommon("txtEffectiveFromDate", "hdfEffectiveFromDate", "txtEffectiveToDate", "hdfEffectiveToDate", false, false);
            ShowHideEarnings(1);
            ShowHideDeductions(1);
            ShowHideTemplateDetail(1);
            ShowHideSalarySplitUp(1);
            //            ShowHideRevisionHistory(0);



            //            $("#divShowhideRevision").click(function () {
            //                $("[id$=divRevisionHistory]").slideToggle("slow");
            //            });
            //            $("#divShowhideSalary").click(function () {
            //                $("[id$=divSalarySplitUp]").slideToggle("slow");
            //            });
        }

        $("[id$=ddlSalaryTemplate]").change(function () {
            ShowHideSalarySplitUp(1);
        });

        //        $("[id$=imbViewDetails]").click(function () {
        //            ShowHideRevisionHistory(1);
        //        });

        function ShowListingTab(flag) {
            //            $("[id$=hdnTabStatus]").val(flag);
            if (flag == '1') {
                $("[id$=PageAction_List]").show();
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlSave]").hide();
                $("[id$=pnlSaveAndContinue]").hide();
                $("[id$=pnlCancel]").hide();
                $("[id$=pnlCancelList]").show();
                $("[id$=pnlEdit]").show();
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlNew]").show();
                $("[id$=ModifiedDatePnl]").hide();
                $("[id$=lnkList]").addClass('tab-active');
                $("[id$=lnkList]").removeClass('tab-inactive');
                $("[id$=lnkDetail]").addClass('tab-inactive');
            }
            else if (flag == '0') {
                $("[id$=PageAction_Entry]").show();
                $("[id$=PageAction_List]").hide();
                $("[id$=pnlSave]").show();
                $("[id$=pnlSaveAndContinue]").show();
                $("[id$=pnlCancel]").show();
                $("[id$=pnlCancelList]").hide();
                $("[id$=pnlEdit]").hide();
                $("[id$=pnlDelete]").show();
                $("[id$=pnlNew]").hide();
                $("[id$=lnkList]").addClass('tab-inactive');
                $("[id$=lnkDetail]").addClass('tab-active');
                $("[id$=lnkDetail]").removeClass('tab-inactive');
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


        //        function ShowHideRevisionHistory(flag) {
        //            if (flag == 1) {
        //                $("[id$=divRevisionHistory").show();
        //                $("[id$=imbShowRevisionHistory").hide();
        //                $("[id$=imbHideRevisionHistory").show();
        //            }
        //            else {
        //                $("[id$=divRevisionHistory").hide();
        //                $("[id$=imbShowRevisionHistory").show();
        //                $("[id$=imbHideRevisionHistory").hide();
        //            }
        //            return false;
        //        }

        function ShowHideSalarySplitUp(flag) {
            if (flag == 1) {
                $("[id$=divSalarySplitUp").show();
                $("[id$=imbShowSalarySplitUp").hide();
                $("[id$=imbHideSalarySplitUp").show();
            }
            else {
                $("[id$=divSalarySplitUp").hide();
                $("[id$=imbShowSalarySplitUp").show();
                $("[id$=imbHideSalarySplitUp").hide();
            }
            return false;
        }

        function ShowHideTemplateDetail(flag) {
            if (flag == 1) {
                $("[id$=divTemplateDetail").show();
                $("[id$=imbShowTemplateDetail").hide();
                $("[id$=imbHideTemplateDetail").show();
            }
            else {
                $("[id$=divTemplateDetail").hide();
                $("[id$=imbShowTemplateDetail").show();
                $("[id$=imbHideTemplateDetail").hide();
            }
            return false;
        }

        function DisplayDetails(mode) {
            if (mode == 1) {
                $("[id$=divSalarySplitUp]").show()
                $("[id$=divRevisionHistory]").hide();
            }
            else if (mode == 2) {
                $("[id$=divSalarySplitUp]").hide()
                $("[id$=divRevisionHistory]").show();
            }
        }

        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode           
            if (mode == 1) {
                $("[id$=pnlSave]").hide();              
                $("[id$=pnlDelete]").hide();              
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upEmpSalary" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <ucGtiTab:GtiTabControl id="HrmsTab" runat="server" CurrentTab="9" />
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
                                            TabIndex="18" Text="Save & Continue" ToolTip="Save & Continue" ValidationGroup="salary"
                                            OnClientClick="javascript:return ValidatePageNow('salary')" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" OnClick="ActionHandler" CommandName="SAVE"
                                            TabIndex="19" Text="<%$Resources:Controls,Save%>" ToolTip="<%$ Resources:Controls,Save%>"
                                            ValidationGroup="salary" OnClientClick="javascript:return ValidatePageNow('salary')"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button ID="btnSalDelete" runat="server" Visible="true" SkinID="btnInner-Delete"
                                            Text="<%$Resources:Controls,Delete%>" OnClientClick="return ShowDeleteConfirm(this);"
                                            CommandName="DELETE" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                            ToolTip="Delete" TabIndex="20" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" OnClick="ActionHandler" Text="<%$Resources:Controls,Cancel%>"
                                            ToolTip="<%$Resources:Controls,Cancel%>" CommandName="CANCEL" TabIndex="21" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:HiddenField ID="hdfSelectedItemPk" runat="server" Value="0" />
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
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
                                    <asp:Label ID="lblhdrDOJ" runat="server" AssociatedControlID="lblhdrDOJText" Text="<%$ resources:DOJ%>"></asp:Label>
                                    <asp:Label ID="lblhdrDOJText" runat="server" Text=""></asp:Label>
                                    <asp:Label ID="lblhdrDOB" runat="server" AssociatedControlID="lblhdrDOBTxt" Text="<%$ resources:DOB%>"></asp:Label>
                                    <asp:Label ID="lblhdrDOBTxt" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="div3col-S">
                                    <asp:Label ID="lblhdrDesignation" runat="server" AssociatedControlID="lblhdrDesignationTxt"
                                        Text="<%$ resources:Designation%>"></asp:Label>
                                    <asp:Label ID="lblhdrDesignationTxt" runat="server" Text=""></asp:Label>
                                    <asp:Label ID="lblhdrDepartment" runat="server" AssociatedControlID="lblhdrDepartmentTxt"
                                        Text="<%$ resources:Department%>"></asp:Label>
                                    <asp:Label ID="lblhdrDepartmentTxt" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="clear">
                                </div>
                            </div>--%>
                            <div class="search-colapse-b">
                                <h1>
                                    <asp:Literal ID="Literal3" runat="server" Text="<%$ resources: PayrollDetail %>" /></h1>
                                <asp:ImageButton runat="server" ID="imbShowTemplateDetail" OnClientClick="javascript:return ShowHideTemplateDetail(1);"
                                    SkinID="imbArrowShow" ToolTip="<%$ resources:Controls,ShowDetails%>" TabIndex="4" />
                                <asp:ImageButton runat="server" ID="imbHideTemplateDetail" OnClientClick="javascript:return ShowHideTemplateDetail();"
                                    Style="display: none" SkinID="imbArrowHide" TabIndex="4" ToolTip="<%$ resources:Controls,HideDetails%>" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divTemplateDetail">
                                <table class="table-devide tablelayout" runat="server" id="pnlEmpSalary">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblEffectiveFromDate" Text="<%$ resources:EffectiveFromDate%>"
                                                    AssociatedControlID="txtEffectiveFromDate"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtEffectiveFromDate" CssClass="input-small input-disabled"
                                                    Enabled="false" TabIndex="6" onkeydown="return CheckKey(event)" MaxLength="11"
                                                    onpaste="return false;"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="rfvEffectiveFromDate" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="salary" EnableClientScript="true" runat="server" ControlToValidate="txtEffectiveFromDate"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_EffectiveFromDate %>">
                                                </asp:RequiredFieldValidator>--%>
                                                <asp:HiddenField ID="hdfEffectiveFromDate" runat="server" Value="" />
                                                <%--  <asp:Label runat="server" ID="lblEffectiveToDate" Text="<%$ resources:EffectiveToDate%>"
                                                    AssociatedControlID="txtEffectiveToDate" CssClass="middle-lbl-d"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtEffectiveToDate" CssClass="input-small"
                                                    TabIndex="6" onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                                <asp:HiddenField ID="hdfEffectiveToDate" runat="server" Value="" />
                                                <asp:RequiredFieldValidator ID="rfvEffectiveToDate" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="salary" EnableClientScript="true" runat="server" ControlToValidate="txtEffectiveToDate"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_EffectiveToDate %>">
                                                </asp:RequiredFieldValidator>--%>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblEmpBasic" Text="<%$ resources:BasicReq%>" AssociatedControlID="txtEmpBasic"
                                                    CssClass="middle-lbl" Style="display: none" Visible="false">
                                                </asp:Label>
                                                <asp:TextBox ID="txtEmpBasic" runat="server" CssClass="input-small numeric" TabIndex="27"
                                                    Visible="false" MaxLength="16"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvEmpBasic" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="salary" EnableClientScript="true" runat="server" ControlToValidate="txtEmpBasic"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Basic %>">
                                                </asp:RequiredFieldValidator>
                                                <cc1:AmountValidation ID="vamEmpBasic" runat="server" ControlToValidate="txtEmpBasic"
                                                    ErrorMessage="<%$ resources:Err_Invalid_Basic %>" NumberDigits="11" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="salary"></cc1:AmountValidation>
                                                <asp:Label runat="server" ID="lblPayrollType" AssociatedControlID="ddlPayrollType"
                                                    Text="<%$resources:PayrollTypeReq%>"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlPayrollType" CssClass="select-half" TabIndex="6"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="vrfPayrollType" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="salary" EnableClientScript="true" runat="server" ControlToValidate="ddlPayrollType"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PayrollType %>" InitialValue="-1">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="search-colapse-b" id="divShowhideSalary">
                                <h1>
                                    <asp:Literal ID="Literal1" runat="server" Text="<%$ resources: SalaryDetail %>" /></h1>
                                <asp:ImageButton runat="server" ID="imbShowSalarySplitUp" OnClientClick="javascript:return ShowHideSalarySplitUp(1);"
                                    SkinID="imbArrowShow" ToolTip="<%$ resources:Controls,ShowDetails%>" TabIndex="6" />
                                <asp:ImageButton runat="server" ID="imbHideSalarySplitUp" OnClientClick="javascript:return ShowHideSalarySplitUp();"
                                    Style="display: none" SkinID="imbArrowHide" TabIndex="6" ToolTip="<%$ resources:Controls,HideDetails%>" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divSalarySplitUp">
                                <table class="table-devide" runat="server" id="Table1">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblSalaryTemplate" AssociatedControlID="ddlSalaryTemplate"
                                                    Text="<%$resources:SalaryTemplateReq%>"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlSalaryTemplate" TabIndex="6" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ActionHandler" CssClass="select-half">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvSalaryTemplate" CssClass="star" SetFocusOnError="true"
                                                    InitialValue="-1" ValidationGroup="salary" EnableClientScript="true" runat="server"
                                                    ControlToValidate="ddlSalaryTemplate" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_SalaryTemplate %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:ImageButton runat="server" ID="imbResetSalary" CssClass="margntop2" SkinID="salary-reload"
                                                    ToolTip="<%$resources:Controls,ReloadSalary %>" OnClick="ActionHandler" CommandName="RESETSALARY"
                                                    TabIndex="6" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                           
                            <div class="fields-grpwrap color-grey grp-after">
                                <div class="fields-group2 padgtop5 margnbotm5">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblTotalCTC" Text="<%$ resources:NetCTC%>" AssociatedControlID="txtTotalCTC" CssClass="margntop4 bold"> 
                                                    </asp:Label>
                                                    <asp:TextBox ID="txtTotalCTC" runat="server" CssClass="input-small numeric bold input-disabled margnbotm5 h18 font12"
                                                        TabIndex="27" Enabled="false" MaxLength="16"></asp:TextBox>
                                                    <asp:Label runat="server" ID="lblTotalGross" CssClass="lbl-20-5perc margntop4 bold" Text="<%$ resources:NetGross%>"
                                                        AssociatedControlID="txtTotalGross">
                                                    </asp:Label>
                                                    <asp:TextBox ID="txtTotalGross" runat="server" CssClass="input-small numeric bold input-disabled margnbotm5 h18 font12"
                                                        TabIndex="27" Enabled="false" MaxLength="16"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblEmpNetSalary" Text="<%$ resources:NetSalary%>" AssociatedControlID="txtEmpNetSalary" CssClass="margntop4 bold">
                                                    </asp:Label>
                                                    <asp:TextBox ID="txtEmpNetSalary" runat="server" CssClass="input-small numeric bold input-disabled margnbotm5 h18 font12"
                                                        TabIndex="27" Enabled="false" MaxLength="16"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                         
                            <div class="clear">
                            </div>
                            <ucgti:EmpSalaryControl id="UCEmpSalary" runat="server" ControlActionMode="1" />
                               </div>
                                </div>
                            <div class="clear">
                            </div>
                            <%--RevisionHistory Details--%>
                            <%--  <div class="search-colapse-b" id="divShowhideRevision">
                                <h1>
                                    <asp:Literal ID="Literal2" runat="server" Text="<%$ resources: RevisionHistory %>" /></h1>
                                <asp:ImageButton runat="server" ID="imbShowRevisionHistory" OnClientClick="javascript:return ShowHideRevisionHistory(1);"
                                    SkinID="imbArrowShow" ToolTip="<%$ resources:Controls,ShowDetails%>" TabIndex="13" />
                                <asp:ImageButton runat="server" ID="imbHideRevisionHistory" OnClientClick="javascript:return ShowHideRevisionHistory();"
                                    Style="display: none" SkinID="imbArrowHide" TabIndex="14" ToolTip="<%$ resources:Controls,HideDetails%>" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divRevisionHistory">
                                <div class="gridwrap">
                                    <asp:GridView runat="server" ID="grdRevisionHistory" Width="100%" AllowPaging="false"
                                        AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                        EmptyDataRowStyle-CssClass="emptytable" OnRowCommand="ActionHandler" TabIndex="15"
                                        OnDataBound="grdRevisionHistory_DataBound">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:EffectivePeriod%> " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdfEffectiveFrom" runat="server" Value='<%# Eval("EDP_EFFECT_FROM", Resources.Constants.DateFormatGridExpanded) %>' />
                                                    <asp:Label ID="lblEffectivePeriod" runat="server" Text='<%# Eval("EDP_EFFECT_FROM", Resources.Constants.DateFormatGridExpanded) + " - " + ( string.IsNullOrEmpty(Convert.ToString(Eval("EDP_EFFECT_TO")))?GetLocalResourceObject("TillDate").ToString():Eval("EDP_EFFECT_TO", Resources.Constants.DateFormatGridExpanded)) %>'
                                                        ToolTip='<%# Eval("EDP_EFFECT_FROM", Resources.Constants.DateFormatGridExpanded) + " - " + ( string.IsNullOrEmpty(Convert.ToString(Eval("EDP_EFFECT_TO")))?GetLocalResourceObject("TillDate").ToString():Eval("EDP_EFFECT_TO", Resources.Constants.DateFormatGridExpanded)) %>'>
                                                    </asp:Label>                                                   
                                                </ItemTemplate>
                                                <ItemStyle Width="17%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:CTC%>  " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCTC" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EMP_CTC")),30) %>'
                                                        ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EMP_CTC")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" CssClass="amount-numeric" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:PreviousCTC%>  " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPreviousCTC" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EMP_LAST_CTC")),30) %>'
                                                        ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EMP_LAST_CTC")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" CssClass="amount-numeric" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Designation1%>  " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDesignation" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EMP_DESIG_TEXT")),30) %>'
                                                        ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EMP_DESIG_TEXT")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="29%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Department1%> " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDepartment" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EMP_DEP_TEXT")),30) %>'
                                                        ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EMP_DEP_TEXT")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="30%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:Panel ID="pnlAction" runat="server" Visible="false" Style="float: left; padding-right: 2px;">
                                                        <asp:ImageButton Width="16px" Height="16px" CssClass="_edit" runat="server" ID="imbViewDetails"
                                                            SkinID="btnview" EnableViewState="false" CommandName="EDIT_ACTION" ToolTip="<%$ resources:View%> " />                                                     
                                                    </asp:Panel>
                                                    <asp:ImageButton Width="16px" Height="16px" CssClass="_edit" runat="server" ID="imbEditDetails"
                                                        SkinID="imbactiongrid" EnableViewState="false" CommandName="SET_ACTION" ToolTip="<%$ resources:SetAsCurrent%> " />
                                                </ItemTemplate>
                                                <ItemStyle Width="4%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <ucgti:EmpSalaryControl id="UCEmpSalaryRevision" runat="server" />
                            </div>--%>
                            <div id="diverror" style="display: none">
                                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                                <asp:ValidationSummary ID="vsPage" ValidationGroup="salary" runat="server" />
                                <asp:ValidationSummary ID="vsPayItem" ValidationGroup="ValAddPayItem" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                <asp:HiddenField ID="hdfIsDeduct" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
