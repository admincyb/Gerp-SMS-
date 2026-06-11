<%@ Page Title="<%$ resources:HRMS-EmployeeTransfer%>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="EmpTransfer.aspx.cs" Inherits="HRMS.Employees.EmpTransfer"
    Theme="ClassicExt" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            $(document).ready(function () {
                GrandScriptUtils.DatePickerCommon("txtDate");
                GrandScriptUtils.DatePickerCommon("txtEffectiveDate");
                GrandScriptUtils.MakeAutoCompleteDDL("txtEmpName", url + "?EmpCategory=2", "hdfEmpName", true, true, "EMPLOYEEAUTOCOMPLETE");
                GrandScriptUtils.MakeAutoCompleteComboBox("txtFromBrLoc", url, "hdfFromBrLoc", "hdfFromBrLocCode", true, true, "BRANCHLOCATION");
                GrandScriptUtils.MakeAutoCompleteComboBox("txtToBrLoc", url, "hdfToBrLoc", "hdfToBrLocCode", true, true, "BRANCHLOCATION");
                GrandScriptUtils.AddDateRangeCommon("txtFilterFromDate", "hdfFilterFromDate", "txtFilterToDate", "hdfFilterToDate", false, false);
                GrandScriptUtils.MakeAutoCompleteDDL("txtDesignation", url, "hdfDesignation", true, true, "DESIGNATION");
                GrandScriptUtils.MakeAutoCompleteDDL("txtBrLocDtlSearch", url, "hdfBrLocDtlSearch", true, true, "BRANCHLOCATION");
                GrandScriptUtils.MakeAutoCompleteDDL("txtDeptDtlSearch", url, "hdfDeptDtlSearch", true, true, "DEPARTMENT");
                GrandScriptUtils.MakeAutoCompleteDDL("txtFilterFromLocation", url, "hdfFilterFromLocation", true, true, "BRANCHLOCATION");
                GrandScriptUtils.MakeAutoCompleteDDL("txtFilterToLocation", url, "hdfFilterToLocation", true, true, "BRANCHLOCATION");
                GrandScriptUtils.MakeAutoCompleteComboBox("txtDepartment", url, "hdfDepartment", "hdfDepartmentCode", true, true, "DEPARTMENT");
                GrandScriptUtils.MakeAutoCompleteComboBox("txtHdrDepartment", url, "hdfHdrDepartment", "hdfHdrDepartmentCode", true, true, "DEPARTMENT");
                GrandScriptUtils.MakeAutoCompleteDDL("txtFilterDept", url, "hdfFilterDept", true, true, "DEPARTMENT");
                GrandScriptUtils.MakeAutoCompleteDDL("txtTrxNo", url, "hdfTrxPk", true, true, "EMPTRANSFERNUMBER");
                BindEmployee();
                BindReportingPerson();
                if ($("[id$=txtFromBrLoc]").attr("disabled") == true) {
                    DisableAuto($("[id$=txtFromBrLoc]"), $("[id$=txtFromBrLoc]"));
                }
                if ($("[id$=txtToBrLoc]").attr("disabled") == true) {
                    DisableAuto($("[id$=txtToBrLoc]"), $("[id$=txtToBrLoc]"));
                }

                if ($('[id$=btnSaveSubmit]').is(":visible"))
                    $('[id$=pnlSubmit]').hide();

                //Set a stamp for cancelled invoice
                if ($("[id$=hdfIsCancelled]").val() == "1") {
                    $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
                    $("[id$='pnlSave']").hide();
                }
                else
                    $("[id$=tblDetailHdr]").addClass("table-devide");
            });
        }

        function AfterDateSelect(controlID) {
            if (controlID == "txtDate") {
                BindEmployee();
                ResetEmployee();
                BindReportingPerson();
                ResetReportingPerson();
            }
        }

        function ClearDateSelect() {
            if ($("[id$=txtDate]").val() == '') {
                BindEmployee();
                ResetEmployee();
                BindReportingPerson();
                ResetReportingPerson();
            }
        }

        function BindEmployee() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee", url + "?Type=" + $("[id$=hdfFromBrLoc]").val() + "&EmpCategory=2&ToDate=" + $("[id$=txtDate]").val(), "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETEBYFILTER", "", true, true, false, 1);
            if (isNaN(parseInt($("[id$=hdfFromBrLoc]").val())) || parseInt($("[id$=hdfFromBrLoc]").val()) <= 0)
                DisableAuto($("[id$=txtEmployee]"), $("[id$=hdfEmployee]"));
            else
                EnableAuto($("[id$=txtEmployee]"), $("[id$=hdfEmployee]"));
        }
        function ResetEmployee() {
            var defText = '<%= Resources.ErpRes.AutoDefaultValue %>';
            $("[id$=txtEmployee]").val(defText);
            $("[id$=hdfEmployee]").val('-1');
        }

        function BindReportingPerson() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtReportingPerson", url + "?Type=" + $("[id$=hdfToBrLoc]").val() + "&EmpCategory=2&ToDate=" + $("[id$=txtDate]").val(), "hdfReportingPerson", true, true, "EMPLOYEEAUTOCOMPLETEBYFILTER", "", true, true, false, 1);
            if (isNaN(parseInt($("[id$=hdfToBrLoc]").val())) || parseInt($("[id$=hdfToBrLoc]").val()) <= 0)
                DisableAuto($("[id$=txtReportingPerson]"), $("[id$=hdfReportingPerson]"));
            else
                EnableAuto($("[id$=txtReportingPerson]"), $("[id$=hdfReportingPerson]"));
        }
        function ResetReportingPerson() {
            var defText = '<%= Resources.ErpRes.AutoDefaultValue %>';
            $("[id$=txtReportingPerson]").val(defText);
            $("[id$=hdfReportingPerson]").val('-1');
        }

        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtFromBrLoc") {
                BindEmployee();
                ResetEmployee();
            }
            else if (targetControlID == "txtToBrLoc") {
                BindReportingPerson();
                ResetReportingPerson();
            }

        }

        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtFromBrLoc") {
                BindEmployee();
                ResetEmployee();
            }
            else if (targetControlID == "txtToBrLoc") {
                BindReportingPerson();
                ResetReportingPerson();
            }
        }

        function ShowListing(flag) {
            if (flag == 1) {
                $("[id$=PageAction_List]").show();
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlListing]").show();
                $("[id$=pnlPrint]").show();
                $("[id$=pnlEntry]").hide();
            }
            else if (flag == 2) {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
                $("[id$=pnlPrint]").show();
                $("[id$=pnlPrintMultiple]").hide();
            }
            else {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
                $("[id$=pnlPrint]").show();
                $("[id$=pnlPrintMultiple]").show();
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
            }
        }

        function ShowHideAdvancedSearch(flag) {
            if (flag) {
                $("[id$=tbladvancedSearch]").show();
                $("[id$=imbShowDetail]").hide();
                $("[id$=imbHideDetail]").show();
            }
            else {
                $("[id$=tbladvancedSearch]").hide();
                $("[id$=imbShowDetail]").show();
                $("[id$=imbHideDetail]").hide();
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

        function ShowHideDetailSec(flag) {
            ///<summary>
            /// Used to Show/Hide Detail Section div
            ///</summary>
            //If flag then Show Items
            $("[id$=hdfShowHideDetailSec]").val(flag);
            if (flag == 1) {
                $("[id$=divDetailSec]").show();
                $("[id$=imbShowDetailSec]").hide();
                $("[id$=imbHideDetailSec]").show();
            }
            else {
                var showhide = parseInt($("[id$=hdfShowHideDetailSec]").val());
                if (showhide != 1) {
                    $("[id$=divDetailSec]").hide();
                    $("[id$=imbShowDetailSec]").show();
                    $("[id$=imbHideDetailSec]").hide();
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
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="147"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="148"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePage('save')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="149" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePage('save')" ValidationGroup="invoice"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="150" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePage('save')" ValidationGroup="save"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="152" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li runat="server" id="pnlPrintMultiple">
                                        <asp:Button runat="server" TabIndex="153" ID="btnPrintMultiple" CommandName="PRINTMULTIPLE"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
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
                                    <li id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="153" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelEmpTransfer %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelEmpTransfer %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="155" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
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
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
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
                                            <asp:Label ID="lblfilterFromLoc" runat="server" Text="<%$ resources:FromLocation%>"
                                                AssociatedControlID="txtFilterFromLocation"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterFromLocation" Text="" TabIndex="50" CssClass="input-small-c"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterFromLocation" Value="" runat="server" />
                                            <asp:Label ID="lblFilterToLocation" runat="server" Text="<%$ resources:ToLocation%>"
                                                AssociatedControlID="txtFilterToLocation" CssClass="middle-lbl-small"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterToLocation" Text="" TabIndex="51" CssClass="input-small-c"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterToLocation" Value="" runat="server" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblEmpName" runat="server" Text="<%$ resources:Employee%>" AssociatedControlID="txtEmpName"></asp:Label>
                                            <asp:TextBox ID="txtEmpName" runat="server" CssClass="input-half" MaxLength="200"
                                                TabIndex="51"></asp:TextBox>
                                            <asp:HiddenField ID="hdfEmpName" runat="server" />
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
                                            <asp:TextBox runat="server" ID="txtFilterFromDate" TabIndex="52" CssClass="input-small-c margnbotm0"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblFilterToDate" runat="server" Text="<%$ resources:ToDate%>" AssociatedControlID="txtFilterToDate"
                                                CssClass="middle-lbl-small"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterToDate" TabIndex="53" CssClass="input-small-c margnbotm0"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblTrxNoSearch" runat="server" Text="<%$ resources:TrxNo%>" AssociatedControlID="txtTrxNo"></asp:Label>
                                            <asp:TextBox ID="txtTrxNo" runat="server" CssClass="input-small-c margnbotm0" TabIndex="54"></asp:TextBox>
                                            <asp:HiddenField ID="hdfTrxPk" runat="server" />
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" CssClass="lbl-13-3perc"
                                                AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="middle-lbl-small-b2 margnbotm0"
                                                TabIndex="55">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,submit %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Approved %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search%>"
                                                ToolTip="<%$ resources:Controls,Search%>" OnClick="ActionHandler" TabIndex="55"
                                                CommandName="SEARCH" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
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
                                    EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" TabIndex="57"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfPk" Value='<%# Eval("EFH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfDelStatus" Value='<%# Eval("EFH_DEL_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval("EFH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TrxNo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdNo" runat="server" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("EFH_NO")))?Resources.ErpRes.Draft:Eval("EFH_NO")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("EFH_NO")))?Resources.ErpRes.Draft:Eval("EFH_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdDate" runat="server" Text='<%# Eval("EFH_DATE", Resources.Constants.HRMSDateFormatGrid)  %>'
                                                    ToolTip='<%# Eval("EFH_DATE", Resources.Constants.HRMSDateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Employee %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdEmployee" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EFD_EMPLOYEE_TEXT"))),35) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EFD_EMPLOYEE_TEXT")))  %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="false" />
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:FromLocation %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdFromLoc" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EFH_FROM_TEXT"))),22) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EFH_FROM_TEXT")))  %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="false" />
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ToLocation%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdToLoc" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EFH_TO_TEXT"))),22) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EFH_TO_TEXT")))  %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Reason%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdReason" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EFH_REASON_TEXT"))),27) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EFH_REASON_TEXT"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="16%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Description%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdDesc" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EFH_DESC"))),23) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EFH_DESC"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="14%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgStatus" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("EFH_CSS_CLASS") %>' ToolTip='<%# Eval("EFH_STATUS_TEXT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
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
                            <table class="table-devide tablelayout" id="tblDetailHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblTrxNoHdr" runat="server" Text="<%$ resources:TrxNo%>" AssociatedControlID="lblTrxNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblTrxNo" CssClass="input-small-b"></asp:Label>
                                            <asp:Label runat="server" ID="lblDate" Text="<%$ resources:DateReq%>" AssociatedControlID="txtDate"
                                                CssClass="middle-lbl-small-a"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDate" CssClass="input-small" TabIndex="1" onkeydown="return CheckKey(event)"
                                                onpaste="return false;" MaxLength="11" onblur="ClearDateSelect()"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfDate" CssClass="star" SetFocusOnError="true" ValidationGroup="save"
                                                EnableClientScript="true" runat="server" ControlToValidate="txtDate" Display="Dynamic"
                                                Text="*" ErrorMessage="<%$ resources:Err_Date %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCompany" runat="server" Text="<%$ resources:Controls,CompanyReq%>"
                                                AssociatedControlID="ddlCompany"></asp:Label>
                                            <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="2" CssClass="select-half-a">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvCompany" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlCompany" Display="Static" Text="*" InitialValue="-1"
                                                ValidationGroup="save" ErrorMessage="<%$ resources:Err_Company %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblFromBrLoc" runat="server" Text="<%$ resources:FromLocationReq%>"
                                                AssociatedControlID="txtFromBrLoc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFromBrLoc" Text="" TabIndex="3" CssClass="select-half"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFromBrLoc" Value="" runat="server" />
                                            <asp:HiddenField ID="hdfFromBrLocCode" Value="" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfFromBrLoc" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="save" EnableClientScript="true" runat="server" ControlToValidate="txtFromBrLoc"
                                                Display="Dynamic" Text="*" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>"
                                                ErrorMessage="<%$ resources:Err_SelectFromLoc %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblBranchLocation" runat="server" Text="<%$ resources:ToLocationReq%>"
                                                AssociatedControlID="txtToBrLoc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtToBrLoc" Text="" TabIndex="4" CssClass="input-half"></asp:TextBox>
                                            <asp:HiddenField ID="hdfToBrLoc" Value="" runat="server" />
                                            <asp:HiddenField ID="hdfToBrLocCode" Value="" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfToBrLoc" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="save" EnableClientScript="true" runat="server" ControlToValidate="txtToBrLoc"
                                                Display="Dynamic" Text="*" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>"
                                                ErrorMessage="<%$ resources:Err_SelectToLoc %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblReason" runat="server" Text="<%$ resources:ReasonReq%>" AssociatedControlID="ddlReason"></asp:Label>
                                            <asp:DropDownList ID="ddlReason" runat="server" TabIndex="5" CssClass="select-half-a">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfReason" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlReason" Display="Static" Text="*" InitialValue="-1"
                                                ValidationGroup="save" ErrorMessage="<%$ resources:Err_Reason %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblEffDate" Text="<%$ resources:WithEffectReq%>" AssociatedControlID="txtEffectiveDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtEffectiveDate" CssClass="input-small" TabIndex="6"
                                                onkeydown="return CheckKey(event)" onpaste="return false;" MaxLength="11"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfEffDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="save" EnableClientScript="true" runat="server" ControlToValidate="txtEffectiveDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_EffDate %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblDescription" runat="server" Text="<%$ resources:Description%>"
                                                AssociatedControlID="txtDescription"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDescription" Text="" TabIndex="7" TextMode="MultiLine"
                                                CssClass="multiline-2a-line input-full" MaxLength="500" onkeydown="limitText(this,500);"
                                                onkeyup="limitText(this,500);" onpase="limitText(this,500);"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("Details").ToString()%></h1>
                                <asp:ImageButton runat="server" ID="imbShowDetailSec" OnClientClick="javascript:return ShowHideDetailSec(1);"
                                    SkinID="imbArrowInactive" ToolTip="<%$ resources:Show%>" />
                                <asp:ImageButton runat="server" ID="imbHideDetailSec" OnClientClick="javascript:return ShowHideDetailSec(0);"
                                    Style="display: none" SkinID="imbArrowActive" ToolTip="<%$ resources:Hide%>" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divDetailSec" style="display: none">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblEmployee" runat="server" Text="<%$ resources:EmployeeReq%>" AssociatedControlID="txtEmployee"></asp:Label>
                                                <asp:TextBox ID="txtEmployee" runat="server" TabIndex="8" CssClass="input-half" MaxLength="100"> </asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfEmployee" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="add" EnableClientScript="true" runat="server" ControlToValidate="txtEmployee"
                                                    Display="Dynamic" Text="*" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>"
                                                    ErrorMessage="<%$ resources:Err_SelectEmployee %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:HiddenField ID="hdfEmployee" runat="server" Value="0" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblReportingPerson" runat="server" Text="<%$ resources:ReportingPerson%>"
                                                    AssociatedControlID="txtReportingPerson"></asp:Label>
                                                <asp:TextBox ID="txtReportingPerson" runat="server" TabIndex="9" CssClass="input-half"
                                                    MaxLength="100"> </asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="vrfReportingPerson" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="add" EnableClientScript="true" runat="server" ControlToValidate="txtReportingPerson"
                                                    Display="Dynamic" Text="*" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>"
                                                    ErrorMessage="<%$ resources:Err_SelectReportingPerson %>">
                                                </asp:RequiredFieldValidator>--%>
                                                <asp:HiddenField ID="hdfReportingPerson" runat="server" Value="0" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label ID="lblRemarks" runat="server" Text="<%$ resources:Remarks%>" AssociatedControlID="txtRemarks"></asp:Label>
                                                <asp:TextBox ID="txtRemarks" runat="server" TabIndex="10" CssClass="input-full" MaxLength="500"> </asp:TextBox>
                                                <asp:ImageButton ID="imbAdd" runat="server" OnClick="ActionHandler" CommandName="ADDITEM"
                                                    ToolTip="Add" OnClientClick="javascript:ValidatePage('add')" ValidationGroup="add"
                                                    TabIndex="11" SkinID="plus" CssClass="margntop2 margnbotm0 margn-rgt4" />
                                                <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear%>"
                                                    ToolTip="<%$ resources:Controls,Clear%>" TabIndex="12" OnClick="ActionHandler"
                                                    CommandName="CLEARADD" SkinID="clear-ext" CssClass="margntop2 margnbotm0 margn-rgt4" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdEmpList" AutoGenerateColumns="false" AllowSorting="true"
                                    EmptyDataRowStyle-CssClass="emptytable" Width="100%" OnSorting="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Employee %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Convert.ToString(Eval("EFD_EMPLOYEE_TEXT")) ,60) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("EFD_EMPLOYEE_TEXT")))%>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfEFDPK" Value='<%# Eval("EFD_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="35%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ReportingPerson %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdLocation" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Convert.ToString(Eval("EFD_EMPLOYEE_RPT_TEXT")) ,60) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("EFD_EMPLOYEE_RPT_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="35%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Remarks %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrdRemarks" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Convert.ToString(Eval("EFD_REMARKS")) ,50) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("EFD_REMARKS")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_edit" runat="server" ID="imbEditDetails"
                                                    TabIndex="13" SkinID="imbeditgrid" EnableViewState="false" CommandName="EDITITEM"
                                                    OnClick="ActionHandler" ToolTip="<%$ resources:Controls,Edit %>" />
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbDeleteAttendance"
                                                    ToolTip="<%$ resources:Controls,Delete %>" SkinID="imbdeletegrid" CommandName="DELETEITEM"
                                                    OnClick="ActionHandler" OnClientClick="return ShowDeleteConfirm(this);" TabIndex="14" />
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
            <div id="diverror" style="display: none">
                <%--Use this label to bind the server errors--%>
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="save" runat="server" />
                <asp:ValidationSummary ID="vsAdd" ValidationGroup="add" runat="server" />
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="save">
                </uc1:WorkflowUserComments>
            </div>
            <asp:HiddenField runat="server" ID="hdfShowHideDetailSec" Value="0" />
            <asp:HiddenField runat="server" ID="hdfCurrPk" Value="0" />
            <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsCancelled" Value="0" runat="server" />
            <asp:HiddenField ID="hdfEmpTransferMaxCount" Value="0" runat="server" />
            <asp:HiddenField ID="hdfCurrent_EfdPk" Value="0" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
