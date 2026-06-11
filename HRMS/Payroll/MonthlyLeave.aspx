<%@ Page Title="<%$ Resources:Captions,Title_MonthlyLeave %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="MonthlyLeave.aspx.cs" Inherits="HRMS.Payroll.MonthlyLeave"
    Theme="ClassicExt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            //GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee", url + "?EmpCategory=2", "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETE");
            GrandScriptUtils.MakeAutoCompleteDDL("txtBrLoc", url + "?BranchByUser=0", "hdfBrLoc", true, true, "BRANCHLOCATION", false, true);
            GrandScriptUtils.DatePickerCommon("txtDate");
            GrandScriptUtils.AddDateRangeCommon("txtFilterFromDate", "hdfFilterFromDate", "txtFilterToDate", "hdfFilterToDate", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtFilterBranch", url + "?BranchByUser=0", "hdfFilterBranch", true, true, "BRANCHLOCATION");
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            ShowHideLeaveDet(1);
            BindEmployee();
            BindEmployeeSearch();
            EnableDisableEmployee();

            var totalRows = parseInt($("#<%=grdLeaveList.ClientID %> tr").length);
            var currPk = parseInt($("[id$=hdfCurrentPk]").val());
            if ((!isNaN(totalRows) && totalRows > 1) || (!isNaN(currPk) && currPk > 0)) {
                DisableAuto($("[id$=txtBrLoc]"), $("[id$=hdfBrLoc]"));
                //$("[id$=txtDate]").attr("disabled", "disabled");
                // $("[id$=txtDate]").addClass("input-disabled");
                //  $("[id$=txtDate]").attr("disabled", true);
            }
            else {
                EnableAuto($("[id$=txtBrLoc]"), $("[id$=hdfBrLoc]"));
                // $("[id$=txtDate]").removeClass("input-disabled")
                //$("[id$=txtDate]").attr("disabled", false);
            }
        }

        function BindEmployee() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee", url + "?Type=" + $("[id$=hdfBrLoc]").val() + "&EmpCategory=2    &ToDate=" + $("[id$=hdfNextYearDate]").val(), "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETEBYFILTER", "", true, true, false, 4);
        }

        function BindEmployeeSearch() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployeeSearch", url + "?Type=" + $("[id$=hdfFilterBranch]").val() + "&EmpCategory=2    &ToDate=" + $("[id$=hdfNextYearDate]").val(), "hdfEmployeeSearch", true, true, "EMPLOYEEAUTOCOMPLETEBYFILTER", "", true, true, false, 1);
        }
        function ResetEmployee(targetControlID) {
            var defText = '<%= Resources.ErpRes.AutoDefaultValue %>';
            if (targetControlID == "txtEmployee" || targetControlID == "txtBrLoc") {
                $("[id$=txtEmployee]").val(defText);
                $("[id$=hdfEmployee]").val('-1');
            }
            else if (targetControlID == "txtEmployeeSearch" || targetControlID == "txtFilterBranch") {
                $("[id$=txtEmployeeSearch]").val(defText);
                $("[id$=hdfEmployeeSearch]").val('-1');
            }

        }

        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtEmployee") {
                $("[id$=btnEmployee]").click();
            }
            else if (targetControlID == "txtBrLoc") {
                BindEmployee();
                ResetEmployee(targetControlID);
                EnableDisableEmployee();
            }
            else if (targetControlID == "txtFilterBranch") {
                BindEmployeeSearch();
                ResetEmployee(targetControlID);
            }
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtEmployee") {
                $("[id$=hdfEmployee]").val("0");
            }
            else if (targetControlID == "txtBrLoc") {
                BindEmployee();
                ResetEmployee(targetControlID);
                EnableDisableEmployee();
            }
            else if (targetControlID == "txtFilterBranch") {
                BindEmployeeSearch();
                ResetEmployee(targetControlID);
            }
        }


        function EnableDisableEmployee() {
            var branchid = parseInt($("[id$=hdfBrLoc]").val());
            if (isNaN(branchid) || branchid <= 0)
                DisableAuto($("[id$=txtEmployee]"), $("[id$=hdfEmployee]"));
            else
                EnableAuto($("[id$=txtEmployee]"), $("[id$=hdfEmployee]"));
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
        function ViewMode(mode) {
            ///<summary>
            /// Used to handle the view Mode
            ///</summary>
            /// <param name="mode" optional="true" type="String">
            /// Mode = 1 Determins ites on View Mode
            /// Mode = 2 Indicates its on New Mode
            /// </param>         
            if (mode == 1) {
                $("[id$='pnlSave']").hide();
                $("[id$='pnlDelete']").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
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
        function ValidateNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                CheckValidationDuplicate(valGroup); //For finding and removing duplicate and other group validation controls
                Page_ClientValidate(valGroup); //For Script validating the Page
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverrorAlert").html());
                return false; //Page is invalid -- stop right here
            }
            else {
                return true; //everythings ok --- Call your function & do your stuff
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
        //For checking if validation control in Array of present validations
        function CheckValidationExists(id) {
            for (var i in validationArrayGroup) {
                if (validationArrayGroup[i] == id) {
                    return true;
                }
            }
            return false;
        }

        //show confirmation msg for Leave count > Balance
        function ShowConfirmMsgLeaveExceeds() {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Msg_LeaveBalanceExceeds").ToString() %>';
            var lvFlag = '<%= GetGlobalResourceObject("ConfigurationsRes", "HrmsLeaveValidation").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfNoOfLeaveValidation]").val(2);
                        $(this).dialog("close");
                        $("[id$=btnAdd]").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfNoOfLeaveValidation]").val(lvFlag);
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }

        function ShowHideLeaveDet(flag) {
            if (flag == 1) {
                $("[id$=divLeaveDet").show();
                $("[id$=imbShowLeaveDet").hide();
                $("[id$=imbHideLeaveDet").show();
            }
            else {
                $("[id$=divLeaveDet").hide();
                $("[id$=imbShowLeaveDet").show();
                $("[id$=imbHideLeaveDet").hide();
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
            if (controlID == "txtFromDate") {
                // var fromDate = $.datepicker.parseDate("dd-M-yy", $('input[id$=txtProcessFromDate]').val());
                // fromDate = new Date(fromDate.getFullYear(), fromDate.getMonth() + 1, fromDate.getDate() - 1);
                // var toDateText = $.datepicker.formatDate("dd-M-yy", fromDate);
                // var toDate = $.datepicker.formatDate("dd-M-yy", fromDate);
                $("[id$=txtToDate]").val($("[id$=txtFromDate]").val());
                $("[id$=hdfToDate]").val($("[id$=txtFromDate]").val());
            }
            if (controlID == "txtDate") {
                var PrevMonthNo = '<%= GetGlobalResourceObject("ConfigurationsRes", "hrmsLeaveEntryPrevMonthDiff").ToString() %>';
                var fromDate = $.datepicker.parseDate("dd-M-yy", $('input[id$=txtDate]').val());
                fromDate = new Date(fromDate.getFullYear(), fromDate.getMonth() - PrevMonthNo, fromDate.getDate());
                $("[id$=hdfNextYearDate]").val($.datepicker.formatDate("dd-M-yy", fromDate))
            }
        }
        
        //show confirmation msg for Leave count > Balance
        function ShowConfirmMsgLeaveDelete(flag) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("MsgLeaveDeleteConfirm").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfIsDeleteYes]").val(1);
                        $(this).dialog("close");
                        if (flag == 1) {   // Detail delte
                            $("[id$=btnEmpDelete]").click();
                        }
                        else if (flag == 2) {  // Sub Details Delete
                            $("[id$=btnEmpDeleteDtl]").click();
                        }
                        else if (flag == 3) { //Header Delte
                            $("[id$=btnDeleteDmy]").click();
                        }

                    },
                    Cancel: function (e) {
                        $("[id$=hdfIsDeleteYes]").val(0);
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
    <asp:UpdatePanel ID="aupdpnlTaskHome" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
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
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$resources:Controls,Save %>"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save"
                                            ValidationGroup="Save" OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('Save')"
                                            TabIndex="150" />
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
                                            TabIndex="152" />
                                    </li>
                                    <li runat="server" style="display: none" id="pnlDummy">
                                        <asp:Button runat="server" ID="btnEmpDelete" CommandName="GRIDDELETE" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" OnClick="ActionHandler" TabIndex="152" />
                                        <asp:Button runat="server" ID="btnEmpDeleteDtl" CommandName="DELETEITEM" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" OnClick="ActionHandler" TabIndex="152" />
                                        <asp:Button runat="server" ID="btnDeleteDmy" CommandName="DELETE" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" OnClick="ActionHandler" TabIndex="152" />
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
                <div class="tab-container-floating">
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
                <asp:Table runat="server" ID="tblPage" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-colapse" id="divAdvanceSearch">
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
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblFilterFromDate" runat="server" Text="<%$ resources:FromDate%>"
                                                AssociatedControlID="txtFilterFromDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterFromDate" TabIndex="50" CssClass="input-small margnbotm0"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblFilterToDate" runat="server" Text="<%$ resources:ToDate%>" AssociatedControlID="txtFilterToDate"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterToDate" TabIndex="51" CssClass="input-small margnbotm0"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblfilterBranch" runat="server" Text="<%$ resources:BrLoc%>" AssociatedControlID="txtFilterBranch"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterBranch" Text="" TabIndex="52" CssClass="input-w21-6per margnbotm0"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterBranch" Value="" runat="server" />
                                            <asp:Label ID="lblEmployeeSearch" runat="server" Text="<%$ resources:Employee%>"
                                                AssociatedControlID="txtEmployeeSearch" CssClass="middle-lbl-small"></asp:Label>
                                            <asp:TextBox ID="txtEmployeeSearch" runat="server" TabIndex="52" CssClass="input-w21-6per margnbotm0"
                                                MaxLength="200"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfEmployeeSearch" runat="server" Value="0" />
                                            <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Search%>" ToolTip="<%$ resources:Search%>"
                                                OnClick="ActionHandler" TabIndex="53" CommandName="FILTER" SkinID="search-ext"
                                                CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Clear%>" ToolTip="<%$ resources:Clear%>"
                                                TabIndex="54" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext"
                                                CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdList" Width="100%" AllowPaging="false" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" OnSorting="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    TabIndex="55" />
                                                <asp:HiddenField ID="hdfEmpLeavePk" Value='<%# Eval("ELR_PK")%>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLstDate" runat="server" Text='<%# Eval("ELR_DATE", Resources.Constants.HRMSDateFormatGrid)  %>'
                                                    ToolTip='<%# Eval("ELR_DATE", Resources.Constants.HRMSDateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BranchLocation%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLstBranch" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELR_BRANCH_TEXT"))),35) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELR_BRANCH_TEXT")))  %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                            <HeaderStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Remarks%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLstRemarks" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELR_REMARKS"))),110) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELR_REMARKS"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="60%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" TabIndex="4" />
                                <div class="clear">
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCompany" runat="server" Text="<%$ resources:Controls,CompanyReq%>"
                                                AssociatedControlID="ddlCompany"></asp:Label>
                                            <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="1" CssClass="select-small-e2">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvCompany" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlCompany" Display="Static" Text="*" InitialValue="-1"
                                                ValidationGroup="Save" ErrorMessage="<%$ resources:Err_Company %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label ID="lblMonth" runat="server" Text="<%$ resources:DateReq%>" AssociatedControlID="txtDate"
                                                CssClass="lbl-7-5perc"></asp:Label>
                                            <asp:TextBox ID="txtDate" runat="server" MaxLength="200" CssClass="input-small" TabIndex="2"
                                                onkeydown="return CheckKey(event)" onpaste="return false;" />
                                            <asp:RequiredFieldValidator ID="rfvMonth" runat="server" ControlToValidate="txtDate"
                                                Display="Static" CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_Date%>"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvtxtDateAdd" runat="server" ControlToValidate="txtDate"
                                                Display="Static" CssClass="star" ValidationGroup="AddToList" Text="*" ErrorMessage="<%$ resources:Err_Date%>"></asp:RequiredFieldValidator>
                                            <%--<asp:Button ID="btnReload" runat="server" OnClick="ActionHandler" CommandName="RELOADLEAVEDETAILS"
                                                EnableTheming="false" Style="display: none" />--%>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblBrLoc" runat="server" Text="<%$ resources:BrLocReq%>" AssociatedControlID="txtBrLoc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtBrLoc" Text="" TabIndex="2" CssClass="input-half"></asp:TextBox>
                                            <asp:HiddenField ID="hdfBrLoc" Value="" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfBrLoc" runat="server" ControlToValidate="txtBrLoc"
                                                CssClass="star" ValidationGroup="AddToList" Text="*" ErrorMessage="<%$ resources:Err_BrLoc%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblRemarks" runat="server" Text="<%$ resources:Remarks%>" AssociatedControlID="txtRemarks"></asp:Label>
                                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" CssClass="multiline-2a-line input-full"
                                                MaxLength="500" TabIndex="3" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    <asp:Literal ID="ltrLeaveAddDed" runat="server" Text="<%$ resources:LeaveDetails %>" /></h1>
                                <asp:ImageButton runat="server" ID="imbShowLeaveDet" OnClientClick="javascript:return ShowHideLeaveDet(1);"
                                    SkinID="imbArrowShow" ToolTip="<%$ resources:Controls,ShowDetails%>" />
                                <asp:ImageButton runat="server" ID="imbHideLeaveDet" OnClientClick="javascript:return ShowHideLeaveDet();"
                                    Style="display: none" SkinID="imbArrowHide" ToolTip="<%$ resources:Controls,HideDetails%>" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divLeaveDet">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblEmployee" runat="server" Text="<%$ resources:EmployeeStar%>" AssociatedControlID="txtEmployee"></asp:Label>
                                                <asp:TextBox ID="txtEmployee" runat="server" TabIndex="4" CssClass="input-half" MaxLength="200"> </asp:TextBox>
                                                <asp:HiddenField ID="hdfEmployee" runat="server" Value="0" />
                                                <asp:RequiredFieldValidator ID="reqEmployee" CssClass="star" SetFocusOnError="true"
                                                    runat="server" ControlToValidate="txtEmployee" Display="Dynamic" Text="*" ValidationGroup="AddToList"
                                                    ErrorMessage="<%$ resources:Err_SelectEmployee %>" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:Button ID="btnEmployee" runat="server" OnClick="ActionHandler" CommandName="CHANGEEMPLOYEE"
                                                    EnableTheming="false" Style="display: none" />
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblLeaveType" runat="server" Text="<%$ resources:LeaveTypeStar %>"
                                                    AssociatedControlID="ddlLeaveType"></asp:Label>
                                                <asp:DropDownList ID="ddlLeaveType" runat="server" TabIndex="5" CssClass="select-small-c1"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="reqLeaveType" CssClass="star" SetFocusOnError="true"
                                                    runat="server" ControlToValidate="ddlLeaveType" Display="Static" Text="*" InitialValue="0"
                                                    ValidationGroup="AddToList" ErrorMessage="<%$ resources:Err_SelectLeaveType %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:Label ID="lblBalance" runat="server" Text="<%$ resources:Balance %>" AssociatedControlID="txtBalance"
                                                    CssClass="middle-lbl"></asp:Label>
                                                <asp:TextBox ID="txtBalance" runat="server" CssClass="input-xsmall-b input-disabled numeric"
                                                    Enabled="false" TabIndex="5" />
                                                <%--<asp:Label ID="lblBalance" runat="server" Text="<%$ resources:Balance %>" AssociatedControlID="txtBalance"></asp:Label>
                                                <asp:TextBox ID="txtBalance" runat="server" CssClass="input-small input-disabled numeric"
                                                    Enabled="false" TabIndex="6" />--%>
                                                <%--                     <asp:Label ID="lblNoOfLeaves" runat="server" Text="<%$ resources:NoOfLeavesStar%>"
                                                    AssociatedControlID="txtNoOfLeaves" CssClass="middle-lbl-small-d"></asp:Label>
                                                <asp:TextBox ID="txtNoOfLeaves" runat="server" CssClass="input-small numeric" TabIndex="7"
                                                    onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);" MaxLength="4"
                                                    onkeyup="limitText(this,10);" onkeydown="limitText(this,10);" onDrop="return false;"
                                                    onPaste="return false;"> </asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvNoOfLeaves" runat="server" ControlToValidate="txtNoOfLeaves"
                                                    Display="Dynamic" CssClass="star" ValidationGroup="AddToList" Text="*" ErrorMessage="<%$ resources:Err_txtNoOfLeaves%>">
                                                </asp:RequiredFieldValidator>
                                                <asp:CompareValidator ID="cmpNoOfLeaves" CssClass="star" SetFocusOnError="true" Enabled="false"
                                                    runat="server" ControlToValidate="txtNoOfLeaves" ControlToCompare="txtBalance"
                                                    Operator="LessThanEqual" Type="Double" Display="Dynamic" Text="*" ValidationGroup="AddToList"
                                                    ErrorMessage="<%$ resources:Err_NoOfLeaves %>">
                                                </asp:CompareValidator>o
                                                <asp:RangeValidator ID="rngNoOfLeaves" runat="server" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="AddToList" EnableClientScript="true" Display="Dynamic" Text="*"
                                                    MaximumValue="1000" ControlToValidate="txtNoOfLeaves" Type="Double" ErrorMessage="<%$ resources:Err_ZeroNoOfLeaves %>">
                                                </asp:RangeValidator>--%>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblFromDate" runat="server" Text="<%$ resources:LeaveFromReq%>" AssociatedControlID="txtFromDate"></asp:Label>
                                                <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                                <asp:TextBox ID="txtFromDate" runat="server" MaxLength="200" CssClass="input-small"
                                                    TabIndex="4" onkeydown="return CheckKey(event)" onpaste="return false;" />
                                                <asp:CheckBox ID="chkFromHalfDay" runat="server" TabIndex="4" ToolTip="<%$ resources:HalfDay %>"
                                                    CssClass="style-none padgtop0" />
                                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="txtFromDate"
                                                    Display="Static" CssClass="star" ValidationGroup="AddToList" Text="*" ErrorMessage="<%$ resources:Err_FromDate%>"></asp:RequiredFieldValidator>
                                                <asp:Label ID="lblToDate" runat="server" Text="<%$ resources:LeaveToReq%>" AssociatedControlID="txtToDate"
                                                    CssClass="middle-lbl-xsmall-a1"></asp:Label>
                                                <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                                <asp:TextBox ID="txtToDate" runat="server" MaxLength="200" CssClass="input-small"
                                                    TabIndex="4" onkeydown="return CheckKey(event)" onpaste="return false;" />
                                                <asp:CheckBox ID="chkToHalfDay" runat="server" TabIndex="4" ToolTip="<%$ resources:HalfDay %>"
                                                    CssClass="style-none padgtop0" />
                                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
                                                    Display="Static" CssClass="star" ValidationGroup="AddToList" Text="*" ErrorMessage="<%$ resources:Err_ToDate%>"></asp:RequiredFieldValidator>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblReason" runat="server" Text="<%$ resources:Reason%>" AssociatedControlID="txtReason"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtReason" MaxLength="500" TabIndex="8" CssClass="input-half"
                                                    onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                                <asp:ImageButton ID="btnAdd" runat="server" OnClick="ActionHandler" CommandName="ADD"
                                                    ToolTip="<%$ resources:Add%>" ValidationGroup="AddToList" OnClientClick="javascript:ValidateNow('AddToList')"
                                                    TabIndex="8" SkinID="plus" CssClass="margntop2 margnbotm0 margn-rgt4" />
                                                <asp:ImageButton ID="imbDetSearch" runat="server" ToolTip="<%$ resources:Clear%>"
                                                    OnClick="ActionHandler" TabIndex="9" CommandName="CLEARADDTOLIST" SkinID="clear-ext"
                                                    CssClass="margntop2 margnbotm0 margn-rgt4" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="gridwrap">
                                <%--class="grdTable"--%>
                                <asp:GridView runat="server" ID="grdLeaveList" Width="100%" AllowPaging="false" AllowSorting="True"
                                    AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-CssClass="emptytable"
                                    OnRowCommand="ActionHandler" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Employee%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ELD_EMPLOYEE_TEXT")),35) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELD_EMPLOYEE_TEXT")))%>'></asp:Label>
                                                <asp:HiddenField ID="hdfELD_PK" runat="server" Value='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELD_PK")))%>' />
                                                <asp:HiddenField ID="hdfROW_NO" runat="server" Value='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ROW_NO")))%>' />
                                                <asp:HiddenField ID="hdfELD_EMPLOYEE" runat="server" Value='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELD_EMPLOYEE")))%>' />
                                                <asp:HiddenField ID="hdfELD_MOD_DT" runat="server" Value='<%# Eval("ELD_MOD_DT")%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:LvFromDate%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFromDateGd" runat="server" Text='<%#Eval("ELD_LV_FROM_HALF")=="1"?Eval("ELD_LV_FROM_DT", Resources.Constants.HRMSDateFormatGrid)+"(H)":Eval("ELD_LV_FROM_DT", Resources.Constants.HRMSDateFormatGrid) %>'
                                                    ToolTip='<%# Eval("ELD_LV_FROM_HALF")=="1"?Eval("ELD_LV_FROM_DT", Resources.Constants.HRMSDateFormatGrid)+"(H)":Eval("ELD_LV_FROM_DT", Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:LvToDate%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblToDateGd" runat="server" Text='<%# Eval("ELD_LV_TO_HALF")=="1"?Eval("ELD_LV_TO_DT", Resources.Constants.HRMSDateFormatGrid)+"(H)":Eval("ELD_LV_TO_DT", Resources.Constants.HRMSDateFormatGrid) %>'
                                                    ToolTip='<%# Eval("ELD_LV_TO_HALF")=="1"?Eval("ELD_LV_TO_DT", Resources.Constants.HRMSDateFormatGrid)+"(H)":Eval("ELD_LV_TO_DT", Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" Wrap="false" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWeekDay" runat="server" Text=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" Wrap="false" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:LeaveType%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblELD_LEAVE_TYPE_CODE_TEXT" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ELD_LEAVE_TYPE_TEXT")),22) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELD_LEAVE_TYPE_TEXT")))%>'></asp:Label>
                                                <asp:HiddenField ID="hdfELD_LEAVE_TYPE" runat="server" Value='<%# Eval("ELD_LEAVE_TYPE")%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Reason%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdRemarks" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ELD_REMARKS")),60) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELD_REMARKS")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:NoOfLeaves%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblELD_LEAVE_COUNTGridview" runat="server" Text='<%# GetFormattedNumber(Eval("ELD_LEAVE_COUNT")) %>'
                                                    ToolTip='<%# GetFormattedNumber(Eval("ELD_LEAVE_COUNT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_edit" runat="server" ID="imbEditDetails"
                                                    SkinID="imbeditgrid" CommandName="EDIT_ACTION" TabIndex="10" ToolTip="<%$resources:Controls,Edit %>"
                                                    Visible='<%# Convert.ToInt32(Eval("ELD_PAYROLL_DTL")) > 0 ?  false :true  %>' />
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbViewLeaveDetails"
                                                    ToolTip="<%$ resources:Controls,View %>" SkinID="btnview" CommandName="VIEW_ACTION"
                                                    OnClick="ActionHandler" TabIndex="10" />
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbDeleteDetails"
                                                    SkinID="imbdeletegrid" ToolTip="<%$resources:Controls,Delete %>" CommandName="DELETE_ACTION"
                                                    OnClientClick="return ShowDeleteConfirm(this);" TabIndex="10" Visible='<%# Convert.ToInt32(Eval("ELD_PAYROLL_DTL")) > 0 ?  false :true  %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" Wrap="false" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="divPopUpLeaveDetails" style="display: none">
                <div class="content-wrapper">
                    <div class="Button-container-popup">
                        <asp:Button ID="btnDeleteItem" SkinID="btnInner-Delete" runat="server" Text="<%$resources:Controls,Delete %>"
                            CommandName="DELETEITEM" OnClick="ActionHandler" TabIndex="3" ToolTip="<%$resources:Controls,Delete %>"
                            OnClientClick="return ShowDeleteConfirm(this);" />
                        <asp:Button ID="btnCancelPopUp" runat="server" CommandName="CANCELPOPUP" OnClick="ActionHandler"
                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Close %>" TabIndex="3"
                            Text="<%$Resources:Controls,Close%>" />
                    </div>
                    <table>
                        <tr>
                            <td colspan="2">
                                <div class="content-wrapper" runat="server" id="div1">
                                    <div class="detail-poi-co1">
                                        <div class="popup-headr">
                                            <asp:Label ID="lblEmpNamePopup" runat="server" Text="" Font-Bold="True" CssClass="minw-60per"></asp:Label>
                                        </div>
                                        <div class="popup-headr">
                                            <asp:Label ID="lblLeaveTypePopup" runat="server" Text="" Font-Bold="True" CssClass="minw-50per inline"></asp:Label></div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div id="divEmpLeavePopup" runat="server" class="gridwrap maxh-290">
                        <asp:GridView runat="server" ID="grdEmpLeave_PopUp" Width="100%" AllowPaging="false"
                            AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                            EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty_PopUp" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label></EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ resources:LeaveDate%> " SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPayroll_Month_PopUp" runat="server" Text='<%# Eval("LDD_DATE", Resources.Constants.HRMSDateFormatGrid)  %>'
                                            ToolTip='<%# Eval("LDD_DATE", Resources.Constants.HRMSDateFormatGrid)  %>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdfLDD_PK" Value='<%# Eval("LDD_PK") %>' />
                                        <asp:HiddenField runat="server" ID="hdfLDD_ELD_PK" Value='<%# Eval("LDD_ELD_PK") %>' />
                                        <asp:HiddenField runat="server" ID="hdfLDD_LEAVE_IS_LOP" Value='<%# Eval("LDD_LEAVE_IS_LOP") %>' />
                                        <asp:HiddenField runat="server" ID="hdfLDD_LEAVE_IS_HALF" Value='<%# Eval("LDD_LEAVE_IS_HALF") %>' />
                                        <asp:HiddenField runat="server" ID="hdfLDD_DATE" Value='<%# Eval("LDD_DATE") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle Width="70%" Wrap="false" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Count%> " SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPeriod_PopUp" runat="server" Text='<%# Eval("LDD_COUNT")%>' ToolTip='<%#  Eval("LDD_COUNT")%>'></asp:Label></ItemTemplate>
                                    <HeaderStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <asp:Button ID="lnkDeleteDayDetails" runat="server" SkinID="delete-icon" ToolTip="<%$ resources:Controls,Delete %>"
                                            Enabled="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkDeleteDayDetails" ToolTip="<%$ resources:Controls,Delete %>"
                                            Visible='<%# Convert.ToInt32(Eval("LDD_PAYROLL_DTL")) > 0 ?  false :true  %>' />
                                    </ItemTemplate>
                                    <HeaderStyle Width="2%" Wrap="false" />
                                    <ItemStyle Width="2%" Wrap="false" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div id="diverrorAlert" style="display: none">
                <asp:ValidationSummary ID="vsListSearch" ValidationGroup="Search" runat="server" />
                <asp:ValidationSummary ID="vsAddToList" ValidationGroup="AddToList" runat="server" />
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
            </div>
            <asp:HiddenField ID="hdfIscontYes" runat="server" />
            <%--   //  <asp:HiddenField ID="hdfIsLeaveExcYes" runat="server" Value="0" />--%>
            <asp:HiddenField ID="hdfNoOfLeaveValidation" runat="server" />
            <asp:HiddenField ID="hdfCurrentPk" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCurRelodPk" runat="server" Value="0" />
            <asp:HiddenField ID="hdfValidate_Leave" runat="server" Value="0" />
            <asp:HiddenField ID="hdfHolidayLeaveVal" runat="server" Value="0" />
            <asp:HiddenField ID="hdfNextYearDate" runat="server" Value="" />
            <asp:HiddenField ID="hdfIsDeleteYes" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCurRowIndex" runat="server" Value="0" />
            <asp:HiddenField ID="hdfLastModeDateEmp" runat="server" Value="" />
            <asp:HiddenField ID="hdfOffDaySkip" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
