<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmpLeaveRequest.aspx.cs"
    Inherits="HRMS.ESS.EmpLeaveRequest" Title="<%$ Resources:Captions,Title_EmployeeLeaveRequest %>"
    MasterPageFile="~/ERPSMS_2.Master" Theme="ClassicExt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtDate");
            GrandScriptUtils.DatePickerCommon("txtSrchDate");
            GrandScriptUtils.AddDateRangeCommon("txtFilterFromDate", "hdfFilterFromDate", "txtFilterToDate", "hdfFilterToDate", false, false);
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee", url + "?ToDate=" + $("[id$=hdfLastYearDate]").val(), "hdfEmployee", true, true, "ESSEMPLOYEEAUTOCOMPLETE", "", true, true, false, 1);
            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployeeSearch", url + "?ToDate=" + $("[id$=hdfLastYearDate]").val(), "hdfEmployeeSearch", true, true, "ESSEMPLOYEEAUTOCOMPLETE", "");
            DisableAuto($("[id$=txtEmployee]"), $("[id$=hdfEmployee]"));

            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();
            else {
                $("[id$='pnlDelete']").hide();
            }

            //Set a stamp for cancelled invoice
            if ($("[id$=hdfIsCancelled]").val() == "1") {
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
                $("[id$='pnlSave']").hide();
            }
            else
                $("[id$=tblDetailHdr]").addClass("table-devide");

            //End
        }

        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtEmployee") {
                $("[id$=btnEmployee]").click();
            }
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtEmployee") {
                $("[id$=hdfEmployee]").val("0");
            }
        }

        function AfterDateSelect(controlID) {
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
                $("[id$='btnSaveSubmit']").hide();
                $("[id$='btnSubmit']").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
            }
        }
        function ShowHideAdvancedSearch(flag) {
            if (flag == 1) {
                $("[id$=tbladvancedSearch").show();
                $("[id$=imbShowFilter").hide();
                $("[id$=imbHideFilter").show();
            }
            else {
                $("[id$=tbladvancedSearch").hide();
                $("[id$=imbShowFilter").show();
                $("[id$=imbHideFilter").hide();
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
        function ShowConfirmMsgLeaveExceeds(flag) {
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
                        if (flag == 1) {
                            $("[id$=btnSave]").click();
                        }
                        else if (flag == 2) {
                            $("[id$=btnWkfSubmit]").click();
                        }
                    },
                    Cancel: function (e) {
                        $("[id$=hdfNoOfLeaveValidation]").val(lvFlag);
                        //$("[id$=btnSaveSubmit]").click();
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
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

        function AfterDateSelect(controlID) {
            if (controlID == "txtFromDate" || controlID == "txtToDate") {
                LeaveClear();
            }
        }
        function HalfCheckChange(obj) {
            LeaveClear();
        }
        function LeaveClear() {
            // var drp3 = document.getElementById('<%= ddlLeaveType.ClientID %>');
            //var drp3 = document.getElementById("<%=ddlLeaveType.ClientID%>");
            //alert(drp3.value());
            // drp1.selectedIndex = 0;
            //document.getElementById("<%#ddlLeaveType.ClientID%>").selectedIndex = 0;
            $("[id$=ddlLeaveType]")[0].selectedIndex = 0;
            $("[id$=txtBalance]").val(0);
            $("[id$=txtNoOfLeave]").val(0);
        }
        function myfunction() {
            $("[id$=BtnLeaveType]").click();
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
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="147"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="148"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('Save')"
                                            ValidationGroup="Save" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="149" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('Save')" ValidationGroup="Save"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
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
                                    <li>
                                        <asp:Button runat="server" TabIndex="155" ID="btnView" CommandName="VIEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,View %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                        <li id="pnlEditforCancel">
                                            <asp:Button runat="server" TabIndex="153" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                                OnClick="ActionHandler" Text="<%$resources:CancelLeaveRequest %>" CommandArgument="SEC_ActionPanel"
                                                SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelLeaveRequest %>" />
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
                <asp:Table runat="server" ID="tblPage" CssClass="tablelayout">
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
                                            <asp:Label ID="lblSrchDate" runat="server" Text="<%$ resources:DateOfSub%>" AssociatedControlID="txtSrchDate"></asp:Label>
                                            <asp:TextBox ID="txtSrchDate" runat="server" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;" TabIndex="50"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
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
                                            <asp:Label ID="lblEmployeeSearch" runat="server" Text="<%$ resources:Employee%>"
                                                AssociatedControlID="txtEmployeeSearch"></asp:Label>
                                            <asp:TextBox ID="txtEmployeeSearch" runat="server" TabIndex="53" CssClass="input-w21-6per margnbotm0"
                                                MaxLength="200"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfEmployeeSearch" runat="server" Value="0" />
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" CssClass="middle-lbl-small"
                                                AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-b margnbotm0"
                                                TabIndex="51">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                                <%--   <asp:ListItem Text="<%$ Resources:Captions,NotPosted %>" Value="0" Enabled="<%$ resources:ConfigurationsRes,FinModuleEnabled %>"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Posted %>" Value="1" Enabled="<%$ resources:ConfigurationsRes,FinModuleEnabled %>"></asp:ListItem>--%>
                                                <asp:ListItem Text="<%$ Resources:Captions,Drafted %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Submitted %>" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Reviewed %>" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Approved %>" Value="6"></asp:ListItem>
                                                <%--<asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="-1"></asp:ListItem>--%>
                                            </asp:DropDownList>
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
                                                <asp:HiddenField ID="hdfEmpLeavePk" Value='<%# Eval("ESL_PK")%>' runat="server" />
                                                <asp:HiddenField runat="server" ID="hdfDept" Value='<%# Eval("ESL_DEPT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfDelStatus" Value='<%# Eval("ESL_DEL_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval("ESL_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLstDate" runat="server" Text='<%# Eval("ESL_DATE", Resources.Constants.HRMSDateFormatGrid)  %>'
                                                    ToolTip='<%# Eval("ESL_DATE", Resources.Constants.HRMSDateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Employee%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLstEmployee" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ESL_EMPLOYEE_TEXT"))),40) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ESL_EMPLOYEE_TEXT")))  %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                            <HeaderStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:LeaveFrom%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLstLeaveFrom" runat="server" Text='<%# Eval("ESL_LV_FROM_DT", Resources.Constants.HRMSDateFormatGrid)  %>'
                                                    ToolTip='<%# Eval("ESL_LV_FROM_DT", Resources.Constants.HRMSDateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:LeaveTo%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLstLeaveTo" runat="server" Text='<%# Eval("ESL_LV_TO_DT", Resources.Constants.HRMSDateFormatGrid)  %>'
                                                    ToolTip='<%# Eval("ESL_LV_TO_DT", Resources.Constants.HRMSDateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:LeaveType%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLstLeaveType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ESL_LEAVE_TYPE_TEXT"))),25) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ESL_LEAVE_TYPE_TEXT")))  %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                            <HeaderStyle />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="<%$ resources:NoOfLeaves%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLstNoLeaves" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ESL_NO_OF_LEAVES"))),25) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ESL_NO_OF_LEAVES")))  %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Remarks%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLstRemarks" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ESL_REASON"))),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ESL_REASON"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgStatus" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("ESL_CSS_CLASS") %>' ToolTip='<%# Eval("ESL_STATUS_TEXT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="5%" CssClass="amount-numeric" Wrap="false" />
                                            <ItemStyle Width="5%" HorizontalAlign="Right" CssClass="padgrgt2 padglft1" Wrap="false" />
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
                                            <asp:Label ID="lblEmployee" runat="server" Text="<%$ resources:EmployeeStar%>" AssociatedControlID="txtEmployee"></asp:Label>
                                            <asp:TextBox ID="txtEmployee" runat="server" TabIndex="1" CssClass="input-half" MaxLength="200"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfEmployee" runat="server" Value="0" />
                                            <asp:RequiredFieldValidator ID="reqEmployee" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="txtEmployee" Display="Dynamic" Text="*" ValidationGroup="Save"
                                                ErrorMessage="<%$ resources:Err_SelectEmployee %>" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Button ID="btnEmployee" runat="server" OnClick="ActionHandler" CommandName="CHANGEEMPLOYEE"
                                                EnableTheming="false" Style="display: none" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCompany" runat="server" Text="<%$ resources:Controls,CompanyReq%>"
                                                AssociatedControlID="ddlCompany"></asp:Label>
                                            <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="2" CssClass="select-half-a"
                                                Enabled="false">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvCompany" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlCompany" Display="Static" Text="*" InitialValue="-1"
                                                ValidationGroup="Save" ErrorMessage="<%$ resources:Err_Company %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblFromDate" runat="server" Text="<%$ resources:LeaveFromReq%>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            <asp:TextBox ID="txtFromDate" runat="server" MaxLength="200" CssClass="input-small"
                                                TabIndex="3" onkeydown="return CheckKey(event)" onpaste="return false;" />
                                            <asp:CheckBox ID="chkFromHalfDay" runat="server" TabIndex="3" ToolTip="<%$ resources:HalfDay %>"
                                                onclick="HalfCheckChange(this)" CssClass="style-none padgtop0" />
                                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="txtFromDate"
                                                Display="Static" CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_FromDate%>"></asp:RequiredFieldValidator>
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$ resources:LeaveToReq%>" AssociatedControlID="txtToDate"
                                                CssClass="lbl-15-4perc"></asp:Label>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                            <asp:TextBox ID="txtToDate" runat="server" MaxLength="200" CssClass="input-small"
                                                TabIndex="3" onkeydown="return CheckKey(event)" onpaste="return false;" />
                                            <asp:CheckBox ID="chkToHalfDay" runat="server" TabIndex="3" ToolTip="<%$ resources:HalfDay %>"
                                                onclick="HalfCheckChange(this)" CssClass="style-none padgtop0" />
                                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
                                                Display="Static" CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_ToDate%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblLeaveType" runat="server" Text="<%$ resources:LeaveTypeStar %>"
                                                AssociatedControlID="ddlLeaveType"></asp:Label>
                                            <asp:DropDownList ID="ddlLeaveType" runat="server" TabIndex="4" CssClass="select-small-c1"
                                                onChange="myfunction()">
                                                <%-- AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"--%>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="reqLeaveType" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlLeaveType" Display="Static" Text="*" InitialValue="0"
                                                ValidationGroup="Save" ErrorMessage="<%$ resources:Err_SelectLeaveType %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label ID="lblBalance" runat="server" Text="<%$ resources:Balance %>" AssociatedControlID="txtBalance"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox ID="txtBalance" runat="server" CssClass="input-xsmall-b input-disabled numeric"
                                                Enabled="false" TabIndex="4" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblReason" runat="server" Text="<%$ resources:Reason%>" AssociatedControlID="txtReason"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtReason" MaxLength="500" TabIndex="5" TextMode="MultiLine"
                                                CssClass="multiline-2a-line input-full" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblNoOfLeave" runat="server" Text="<%$ resources:NoOfLeaves%>" AssociatedControlID="txtNoOfLeave"></asp:Label>
                                            <asp:TextBox ID="txtNoOfLeave" runat="server" CssClass="input-small input-disabled numeric"
                                                Enabled="false" />
                                            <asp:Label ID="lblMonth" runat="server" Text="<%$ resources:DateOfSub%>" AssociatedControlID="txtDate"
                                                CssClass="lbl-21-5perc"></asp:Label>
                                            <asp:TextBox ID="txtDate" runat="server" MaxLength="200" CssClass="input-small" TabIndex="6"
                                                onkeydown="return CheckKey(event)" onpaste="return false;" />
                                            <asp:RequiredFieldValidator ID="rfvMonth" runat="server" ControlToValidate="txtDate"
                                                Display="Static" CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_Date%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverrorAlert" style="display: none">
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
            </div>
            <div style="display: none;">
                <asp:Button runat="server" ID="btnWkfSubmit" CommandName="WRKFSUBMIT" OnClick="ActionHandler" />
                <asp:Button runat="server" ID="BtnLeaveType" CommandName="CHANGETYPE" OnClick="ActionHandler" />
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc3:WorkflowUserComments id="ucrWrkf" runat="server" validationgroup="Save">
                </uc3:WorkflowUserComments>
            </div>
            <asp:HiddenField ID="hdfLastYearDate" runat="server" Value="" />
            <asp:HiddenField ID="hdfIscontYes" runat="server" />
            <asp:HiddenField ID="hdfNoOfLeaveValidation" runat="server" />
            <asp:HiddenField ID="hdfCurrentPk" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCurRelodPk" runat="server" Value="0" />
            <asp:HiddenField ID="hdfValidate_Leave" runat="server" Value="0" />
            <asp:HiddenField ID="hdfHolidayLeaveVal" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsDeleteYes" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCurRowIndex" runat="server" Value="0" />
            <asp:HiddenField ID="hdfLastModeDateEmp" runat="server" Value="" />
            <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsCancelled" Value="0" runat="server" />
            <asp:HiddenField ID="hdfAdvSearch" Value="0" runat="server" />
            <asp:HiddenField ID="hdfOffDaySkip" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
