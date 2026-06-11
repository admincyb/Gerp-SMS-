<%@ Page Language="C#" Title="<%$ Resources:Captions,Title_AppraisalDetails %>" AutoEventWireup="true"
    CodeBehind="EmployeeAppraisalDetails.aspx.cs" Inherits="HRMS.Payroll.EmployeeAppraisalDetails"
    Theme="ClassicExt" MasterPageFile="~/ERPSMS_2.Master" %>

<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="~/Employees/UserControls/EmpSalaryControl.ascx" TagName="EmpSalaryControl"
    TagPrefix="ucgti" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtSrchEmployee", url + "?EmpCategory=2", "hdfSrchEmployee", true, true, "EMPLOYEEAUTOCOMPLETE");
            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee", url + "?EmpCategory=2", "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETE");
            GrandScriptUtils.MakeAutoCompleteDDL("txtEmpNameFilter", url + "?EmpCategory=2", "hdfEmpNameFilter", true, true, "EMPLOYEEAUTOCOMPLETE");
            GrandScriptUtils.MakeAutoCompleteDDL("txtDepartment", url, "hdfDepartment", true, true, "DEPARTMENTAUTOCOMPLETE");
            GrandScriptUtils.MakeAutoCompleteDDL("txtDesignation", url, "hdfDesignation", true, true, "DESIGNATION");
            ShowHideEmployeeDetails(1);
            GrandScriptUtils.AddDateRangeCommon("txtSrchFromDate", "hdfSrchFromDate", "txtSrchToDate", "hdfSrchToDate", false, false);
            GrandScriptUtils.DatePickerCommon("txtDate");
            GrandScriptUtils.DatePickerCommon("txtEffectFrom");
            GrandScriptUtils.DatePickerCommon("txtRefDate");
            if ($("[id$=hdfNewMode").val() > 0)
                DisableAuto($("[id$=txtEmployee]"), $("[id$=hdfEmployee]"));
            else
                EnableAuto($("[id$=txtEmployee]"), $("[id$=hdfEmployee]"));
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();

            //Set a stamp for cancelled invoice
            if ($("[id$=hdfIsCancelled]").val() == "1") {
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
                $("[id$='pnlSave']").hide();
            }
            else
                $("[id$=tblDetailHdr]").addClass("table-devide");

            //End
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


        function ShowHideEmployeeDetails(flag) {
            if (flag == 1) {
                $("[id$=divEmployeeDetails").show();
                $("[id$=imbShowEmployeeDetails").hide();
                $("[id$=imbHideEmployeeDetails").show();
            }
            else {
                $("[id$=divEmployeeDetails").hide();
                $("[id$=imbShowEmployeeDetails").show();
                $("[id$=imbHideEmployeeDetails").hide();
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

        function ViewMode(mode) {
            ///<summary>
            /// Used to handle the view Mode
            ///</summary>
            /// <param name="mode" optional="true" type="String">
            /// Mode = 1 Determins ites on View Mode
            /// Mode = 2 Indicates its on New Mode
            /// Mode = 3 Indicates its on Edit Mode
            /// </param>         
            if (mode == 1) {
                $("[id$='pnlSave']").hide();
                $("[id$='pnlDelete']").hide();
                $("[id$='btnSaveSubmit']").hide();
                $("[id$='btnSubmit']").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();

            }
        }
        //To excecute after  auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtEmployee") {
                $("[id$=btnEmployee]").click();
            }
        }
        function AfterInvalidSelect(cntrl) {
            if (cntrl == 'txtEmployee') {
                $("[id$='btnEmployeeClear']").click();
            }
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
        function ValidateAppPage(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                CheckValidationDuplicate(valGroup);
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
                return false;
            }
            else {
                return true;
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

        function SaveConfirmationMsg() {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("MsgSaveConfirm").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfIsSaveContYes]").val(1);
                        $(this).dialog("close");
                        if ($("[id$=hdfSaveSubmitYes]").val() > 0) {
                            $("[id$=btnSaveSubmit]").click();
                        }
                        else {
                            $("[id$=btnSave]").click();
                        }

                    },
                    Cancel: function (e) {
                        $("[id$=hdfIsSaveContYes]").val(0);
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upAdditionDedction" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="147"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="148"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidateAppPage('Save')"
                                            ValidationGroup="Save" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="149" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateAppPage('Save')" ValidationGroup="Save"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$resources:Controls,Save %>"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save"
                                            ValidationGroup="Save" OnClick="ActionHandler" OnClientClick="javascript:ValidateAppPage('Save')"
                                            TabIndex="150" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);"
                                            TabIndex="151" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            CssClass="popupclose" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" OnClick="ActionHandler"
                                            TabIndex="152" />
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
                                    <li id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="154" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelAppraisal %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelAppraisal %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="155" ID="btnView" CommandName="VIEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,View %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
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
                <asp:Table runat="server" ID="tblPage" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-colapse" id="divAdvanceSearch" style="margin-top: 0px;">
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
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7 ">
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-a1" TabIndex="5">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7 ">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblSrchFromDate" runat="server" Text="<%$ resources:FromDate%>" AssociatedControlID="txtSrchFromDate"></asp:Label>
                                            <asp:TextBox ID="txtSrchFromDate" runat="server" CssClass="input-small margnbotm0"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" TabIndex="1"></asp:TextBox>
                                            <asp:HiddenField ID="hdfSrchFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblSrchToDate" runat="server" Text="<%$ resources:ToDate%>" AssociatedControlID="txtSrchToDate"
                                                CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:TextBox ID="txtSrchToDate" runat="server" CssClass="input-small margnbotm0"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" TabIndex="1"></asp:TextBox>
                                            <asp:HiddenField ID="hdfSrchToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label runat="server" ID="lblSrchType" Text="<%$ resources:Type%>" AssociatedControlID="ddlSrchType"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlSrchType" CssClass="select-small-a1 margnbotm0"
                                                TabIndex="1">
                                            </asp:DropDownList>
                                            <asp:Label runat="server" ID="lblSrchEmployee" Text="<%$ resources:Employee%>" AssociatedControlID="txtSrchEmployee"
                                                CssClass="middle-lbl-small"></asp:Label>
                                            <asp:TextBox ID="txtSrchEmployee" runat="server" CssClass="input-w23-6per margnbotm0"
                                                MaxLength="100" TabIndex="1"></asp:TextBox>
                                            <asp:HiddenField ID="hdfSrchEmployee" runat="server" />
                                            <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Type%>" ToolTip="<%$ resources:Type%>"
                                                OnClick="ActionHandler" CommandName="SEARCH" SkinID="search-ext" CssClass="margntop2 margnbotm0"
                                                ValidationGroup="Search" TabIndex="1" />
                                            <asp:ImageButton ID="imgClearSearch" runat="server" Text="<%$ resources:Type%>" ToolTip="<%$ resources:Type%>"
                                                OnClick="ActionHandler" CommandName="CLEARSEARCH" SkinID="clear-ext" CssClass="margntop2 margnbotm0"
                                                TabIndex="1" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdEmpAppList" Width="100%" AllowPaging="false"
                                    AllowSorting="false" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" TabIndex="2"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfEIH_PK" Value='<%# Eval("EIH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfEIH_EMPLOYEEt" Value='<%# Eval("EIH_EMPLOYEE") %>' />
                                                <asp:HiddenField runat="server" ID="hdfDept" Value='<%# Eval("EIH_DEPT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfDelStatus" Value='<%# Eval("EIH_DEL_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval("EIH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TrxNo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGdTrxNo" runat="server" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("EIH_NO")))?Resources.ErpRes.Draft:Eval("EIH_NO")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("EIH_NO")))?Resources.ErpRes.Draft:Eval("EIH_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGdDate" runat="server" Text='<%#Eval("EIH_DATE", Resources.Constants.HRMSDateFormatGrid) %>'
                                                    ToolTip='<%# Eval("EIH_DATE", Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                            <HeaderStyle Width="7%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Employee%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGdEmployeeText" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("EIH_EMPLOYEE_TEXT"),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EIH_EMPLOYEE_TEXT"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="20%" />
                                            <ItemStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Type%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGdType" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("EIH_TYPE_TEXT"),25) %>'
                                                    ToolTip='<%# Eval("EIH_TYPE_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:EffFrom%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGdEffFrom" runat="server" Text='<%#Eval("EIH_EFFECT_DATE", Resources.Constants.HRMSDateFormatGrid) %>'
                                                    ToolTip='<%# Eval("EIH_EFFECT_DATE", Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Remarks%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGdRemarks" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EIH_TRN_NAME"))),45) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EIH_TRN_NAME"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgStatus" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("EIH_CSS_CLASS") %>' ToolTip='<%# Eval("EIH_STATUS_TEXT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc2:PagerControl ID="uclPaging" runat="server" TabIndex="3" />
                                <div class="clear">
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <table class="table-devide tablelayout" id="tblDetailHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblTrxNoHdr" runat="server" Text="<%$ resources:TrxNo%>" AssociatedControlID="lblTrxNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblTrxNo" CssClass="input-small"></asp:Label>
                                            <asp:Label ID="lblDate" runat="server" Text="<%$ resources:DateStar%>" AssociatedControlID="txtDate"
                                                CssClass="lbl-22-8perc"></asp:Label>
                                            <asp:TextBox ID="txtDate" runat="server" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;" TabIndex="4"></asp:TextBox>
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="rfvDate" CssClass="star" SetFocusOnError="true" ValidationGroup="Save"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtDate" Display="Dynamic"
                                                    Text="*" ErrorMessage="<%$ resources:Err_Date %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblEmployee" Text="<%$ resources:EmployeeStar%>" AssociatedControlID="txtEmployee"
                                                CssClass="lbl-25-1perc"></asp:Label>
                                            <asp:TextBox ID="txtEmployee" runat="server" CssClass="input-half" MaxLength="100"
                                                TabIndex="4"></asp:TextBox>
                                            <asp:HiddenField ID="hdfEmployee" runat="server" />
                                            <asp:Button ID="btnEmployee" runat="server" OnClick="ActionHandler" CommandName="CHANGEEMPLOYEE"
                                                Style="display: none" EnableTheming="false" />
                                            <asp:Button ID="btnEmployeeClear" runat="server" OnClick="ActionHandler" CommandName="CLEARITEM"
                                                Style="display: none" EnableTheming="false" />
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="rfvEmployee" runat="server" ControlToValidate="txtEmployee"
                                                    Display="Dynamic" CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_Employee%>"
                                                    InitialValue="<%$resources:ErpRes,AutoDefaultValue %>"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblType" Text="<%$ resources:TypeStar%>" AssociatedControlID="ddlType"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlType" CssClass="select-small-a1" TabIndex="5">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator runat="server" ID="vrfType" ControlToValidate="ddlType"
                                                Text="*" ErrorMessage="<%$ Resources:Msg_SelectType %>" InitialValue="-1" CssClass="star"
                                                Display="Static" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            <asp:Label ID="lblEffectFrom" runat="server" Text="<%$ resources:EffFromStar%>" AssociatedControlID="txtEffectFrom"
                                                CssClass="middle-lbl-small-b1"></asp:Label>
                                            <asp:TextBox ID="txtEffectFrom" runat="server" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;" TabIndex="5"></asp:TextBox>
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="rfvEffectFrom" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="Save" EnableClientScript="true" runat="server" ControlToValidate="txtEffectFrom"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_DateEffFrom %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblPrevAppDate" runat="server" Text="<%$ resources:PrvAPPDate%>" ToolTip="<%$ resources:PrvAPPDate%>"
                                                AssociatedControlID="lblPrevAppDateText"></asp:Label>
                                            <asp:Label runat="server" ID="lblPrevAppDateText" CssClass="input-small"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblRefNo" Text="<%$ resources:RefNo%>" AssociatedControlID="txtRefNo"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRefNo" CssClass="input-small" MaxLength="20" TabIndex="6"></asp:TextBox>
                                            <asp:Label runat="server" ID="lblRefDate" Text="<%$ resources:RefDate%>" AssociatedControlID="txtRefDate"
                                                CssClass="lbl-22-8perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRefDate" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;" TabIndex="6"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblRemarks" runat="server" Text="<%$ resources:Remarks%>" AssociatedControlID="txtRemarks"
                                                CssClass="lbl-25-1perc"></asp:Label>
                                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="input-half" TabIndex="6"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblComments" runat="server" Text="<%$ resources:Comments%>" AssociatedControlID="txtComments"></asp:Label>
                                            <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" CssClass="multiline-2a-line input-full"
                                                MaxLength="500" TabIndex="7"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    <asp:Literal ID="ltrEmployeeAddDed" runat="server" Text="<%$ resources: EmployeeDetails %>" /></h1>
                                <asp:ImageButton runat="server" ID="imbShowEmployeeDetails" OnClientClick="javascript:return ShowHideEmployeeDetails(1);"
                                    SkinID="imbArrowShow" ToolTip="<%$ resources:Controls,ShowDetails%>" />
                                <asp:ImageButton runat="server" ID="imbHideEmployeeDetails" OnClientClick="javascript:return ShowHideEmployeeDetails();"
                                    Style="display: none" SkinID="imbArrowHide" ToolTip="<%$ resources:Controls,HideDetails%>" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divEmployeeDetails">
                                <table class="table-devide tablelayout">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblDepartment" runat="server" Text="<%$ resources:Department%>" AssociatedControlID="lblDepartmentOldText"></asp:Label>
                                                <asp:Label ID="lblDepartmentOldText" runat="server" Text="" CssClass="lbl-30perc"></asp:Label>
                                                <asp:HiddenField ID="hdfDepartmentOld" Value="-1" runat="server" />
                                                <asp:TextBox runat="server" ID="txtDepartment" Text="" TabIndex="8" CssClass="input-medium"></asp:TextBox>
                                                <asp:HiddenField ID="hdfDepartment" Value="-1" runat="server" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblDesignation" runat="server" Text="<%$ resources:Designation%>"
                                                    AssociatedControlID="lblDesignationOldText"></asp:Label>
                                                <asp:Label ID="lblDesignationOldText" runat="server" CssClass="lbl-28-7perc"></asp:Label>
                                                <asp:HiddenField ID="hdfDesignationOld" runat="server" Value="-1" />
                                                <asp:TextBox runat="server" TabIndex="8" ID="txtDesignation" CssClass="input-medium" />
                                                <asp:HiddenField ID="hdfDesignation" runat="server" Value="-1" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="clear">
                                </div>
                                <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                    <h1 class="detail-head">
                                        <%-- <%= GetLocalResourceObject("Employee_Details").ToString()%>--%>
                                        Salary Details</h1>
                                    <div class="clear">
                                    </div>
                                    <div class="detail-outer">
                                        <div class="section1">
                                            <asp:Label ID="lblCTCEmpCTCH" runat="server" Text="<%$ resources:CTC%>" CssClass="sectionname"></asp:Label>
                                            <asp:Label ID="lblCtcOldSalary" runat="server" Text="<%$ resources:Old%>" CssClass="tabname"></asp:Label>
                                            <asp:Label ID="lblCtcNewSalary" runat="server" Text="<%$ resources:New%>" CssClass="tabname"></asp:Label>
                                            <asp:Label ID="lblCtcDiffSalary" runat="server" Text="<%$ resources:Diff%>" CssClass="tabname2"></asp:Label>
                                            <div class="tabcontent">
                                                <asp:TextBox runat="server" ID="txtOldCTC" Text="" Enabled="false"></asp:TextBox>
                                            </div>
                                            <div class="tabcontent">
                                                <asp:TextBox runat="server" ID="txtTotalCTC" Text="" Enabled="false"></asp:TextBox></div>
                                            <div class="tabcontent2">
                                                <asp:TextBox runat="server" ID="txtDiffCTC" Text="" Enabled="false"></asp:TextBox></div>
                                        </div>
                                        <div class="section2">
                                            <asp:Label ID="lblEmpGrossSalaryH" runat="server" Text="<%$ resources:GrossSalary%>"
                                                CssClass="sectionname"></asp:Label>
                                            <asp:Label ID="lblGsOldSalary" runat="server" Text="<%$ resources:Old%>" CssClass="tabname"></asp:Label>
                                            <asp:Label ID="lblGsNewSalary" runat="server" Text="<%$ resources:New%>" CssClass="tabname"></asp:Label>
                                            <asp:Label ID="lblGsDiffSalary" runat="server" Text="<%$ resources:Diff%>" CssClass="tabname2"></asp:Label>
                                            <div class="tabcontent">
                                                <asp:TextBox runat="server" ID="txtOldGrossSalary" Text="" Enabled="false"></asp:TextBox></div>
                                            <div class="tabcontent">
                                                <asp:TextBox runat="server" ID="txtTotalGross" Text="" Enabled="false"></asp:TextBox></div>
                                            <div class="tabcontent2">
                                                <asp:TextBox runat="server" ID="txtDiffGrossSalary" Text="" Enabled="false"></asp:TextBox></div>
                                        </div>
                                        <div class="section3">
                                            <asp:Label ID="lblEmpNetSalaryH" runat="server" Text="<%$ resources:NetSalary%>"
                                                CssClass="sectionname"></asp:Label>
                                            <asp:Label ID="lblNsOldSalary" runat="server" Text="<%$ resources:Old%>" CssClass="tabname"></asp:Label>
                                            <asp:Label ID="lblNsNewSalary" runat="server" Text="<%$ resources:New%>" CssClass="tabname"></asp:Label>
                                            <asp:Label ID="lblNsDiffSalary" runat="server" Text="<%$ resources:Diff%>" CssClass="tabname2"></asp:Label>
                                            <div class="tabcontent">
                                                <asp:TextBox runat="server" ID="txtOldNetSalary" Text="" Enabled="false"></asp:TextBox>
                                            </div>
                                            <div class="tabcontent">
                                                <asp:TextBox runat="server" ID="txtEmpNetSalary" Text="" Enabled="false"></asp:TextBox>
                                            </div>
                                            <div class="tabcontent2">
                                                <asp:TextBox runat="server" ID="txtDiffNetSalary" Text="" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="clear">
                                </div>
                                <ucgti:EmpSalaryControl id="UCEmpSalary" runat="server" ControlActionMode="1" tabindex="9" />
                            </div>
                            <div class="clear">
                            </div>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="Save" runat="server" />
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments id="ucrWrkf" runat="server" validationgroup="Save">
                </uc1:WorkflowUserComments>
            </div>
            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormatWithComma" runat="server" />
            <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
            <asp:HiddenField ID="hdfAdvSearch" Value="0" runat="server" />
            <asp:HiddenField ID="hdfNewMode" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsCancelled" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIsSaveContYes" runat="server" Value="0" />
            <asp:HiddenField ID="hdfSaveSubmitYes" runat="server" Value="0" />
            <%--<asp:HiddenField ID="hdfExchangeRateFormat" runat="server" />--%>
            <%--<asp:HiddenField ID="hdfCurrencyMode" runat="server" />--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
