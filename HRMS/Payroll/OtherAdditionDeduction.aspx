<%@ Page Title="<%$ Resources:Captions,Title_OtherAdditionDeduction %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="OtherAdditionDeduction.aspx.cs"
    Inherits="HRMS.Payroll.OtherAdditionDeduction" ValidateRequest="false" Theme="ClassicExt" %>

<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc2" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            GrandScriptUtils.DatePickerCommon("txtAddDedDate");
            GrandScriptUtils.DatePickerCommon("txtSrchFromDate");
            GrandScriptUtils.DatePickerCommon("txtSrchToDate");
            GrandScriptUtils.DatePickerCommon("txtEmpDate");

            GrandScriptUtils.MakeAutoCompleteDDL("txtEmpName", url + "?EmpCategory=2", "hdfEmpName", true, true, "EMPLOYEEAUTOCOMPLETE");
            //GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee ", url + "?EmpCategory=2&EmpBranch=" + $("[id$=ddlBranchLocation]").val(), "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETE");
            GrandScriptUtils.MakeAutoCompleteDDL("txtDepartment", url, "hdfDepartment", true, true, "DEPARTMENTAUTOCOMPLETE");
            GrandScriptUtils.MakeAutoCompleteDDL("txtTrxNo", url, "hdfTrxPk", true, true, "ADDDEDNUMBER");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCurrency", url, "hdfCurrency", true, true, "CURRENCY");
            $("[id*=txtAmount]").ForceNumericOnly();
            $("[id*=txtgrdAmount]").ForceNumericOnly();
            $("[id*=txtExchangeRate]").ForceNumericOnly();
            ShowHideEmployeeAddDed(1);
            InitEmployeeAuto();
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
            if ($("[id$=txtExchangeRate]").attr("disabled") == true) {
                $("[id$=txtExchangeRate]").addClass("input-disabled");
            }
            else {
                $("[id$=txtExchangeRate]").removeClass("input-disabled");
            }
            // To Disable Cuurency When Grid Contains Data
            var totalRowCount = $("[id*=grdEmpAddDedList] tr").length;
            if (totalRowCount <= "1" && $("[id$=hdfCurrencyMode]").val() == "1") {
                EnableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
            }
            else {
                DisableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
            }

            //            if ($("[id$=txtCurrency]").attr("disabled") == true) {
            //                DisableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
            //            }
            //            else {
            //                EnableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
            //            }
        }

        function InitEmployeeAuto() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee ", url + "?EmpCategory=2&EmpBranch=" + $("[id$=ddlBranchLocation]").val() + "&EmpDept=" + $("[id$=hdfDepartment]").val() + "&EmpType=" + $("[id$=ddlEmployeeType]").val() + "&EmploymentType=" + $("[id$=ddlEmploymentType]").val() + "&PaymentMode=" + $("[id$=ddlPaymentMode]").val() + "&EmpCurrency=" + $("[id$=hdfCurrency]").val() + "&ToDate=" + $("[id$=txtAddDedDate]").val(), "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETE");
            ResetEmployee
        }
        function ResetEmployee() {
            var defText = '<%= Resources.ErpRes.All_Small %>';
            $("[id$=txtEmployee]").val(defText);
            $("[id$=hdfEmployee]").val('-1');
        }
        //To excecute after  auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtDepartment") {
                InitEmployeeAuto();
            }
            if (targetControlID == "txtCurrency") {
                $("[id$=btnCurrency]").click();
                InitEmployeeAuto();
            }
        }

        //To excecute after  auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtDepartment") {
                InitEmployeeAuto();
            }
        }
        function AfterDateSelect(controlID) {
            if (controlID == "txtFromDate") {
                $("[id$=btnCurrency]").click();
            }
            else if (controlID == "txtAddDedDate") {
                InitEmployeeAuto();
            }
        }
        function ClearDateSelect() {
            if ($("[id$=txtAddDedDate]").val() == '') {
                InitEmployeeAuto();
            }
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

        function ShowHideEmployeeAddDed(flag) {
            if (flag == 1) {
                $("[id$=divEmployeeAddDed").show();
                $("[id$=imbShowEmployeeAddDed").hide();
                $("[id$=imbHideEmployeeAddDed").show();
            }
            else {
                $("[id$=divEmployeeAddDed").hide();
                $("[id$=imbShowEmployeeAddDed").show();
                $("[id$=imbHideEmployeeAddDed").hide();
            }
            return false;
        }

        function ShowHideImport(flag) {
            if (flag == 1) {
                $("[id$=divImport").show();
                $("[id$=imbShowImport").hide();
                $("[id$=imbHideImport").show();
            }
            else {
                $("[id$=divImport").hide();
                $("[id$=imbShowImport").show();
                $("[id$=imbHideImport").hide();
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
                $("[id$='imgAddNewDetails']").hide();
            }
            else if (mode == 2) {
                $("[id$='pnlDelete']").hide();
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
        function ValidateAddDedPage(valGroup) {
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

        function ShowDeleteConfirmationMsg(btn, message) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = message ? message : '<%= Resources.ErpRes.MsgSlctDeleteConfirm %>';
            $("#popupHolder").html("");
            //To set fit to screen
            $('html, body').animate({ scrollTop: '0px' }, 0);
            $('html, body').css('overflow', 'hidden');
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $('#divmodel').hide();
                        $(this).dialog("close");
                        __doPostBack(btn.name, '');
                    },
                    Cancel: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $('#divmodel').hide();
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


        function PayelemtChange() {
            var payName = $('#<%=ddlPayElement.ClientID %> option:selected').text();
            //var name = $("[id$=txtName]").val();
            //if (name == "")
            $("[id$=txtName]").val(payName);
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
            if ($("[id$=txtCurrency]").attr("disabled") == true) {
                DisableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
            }
            else {
                EnableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
            }
        }

        //Select All
        $("[id*=chkEmpHeader]").live("click", function () {
            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (chkHeader.is(":checked")) {
                    $(this).attr("checked", "checked");
                    $("td", $(this).closest("tr")).addClass("selected");
                } else {
                    $(this).removeAttr("checked");
                    $("td", $(this).closest("tr")).removeClass("selected");
                }
            });
        });

        //Deselect SelectALL checkbox when one checkbox is unchecked in grid
        $("[id*=chkEmpselect]").live("click", function () {
            var grid = $(this).closest("table");
            var chkHeader = $("[id*=chkEmpHeader]", grid);
            if (!$(this).is(":checked")) {
                $("td", $(this).closest("tr")).removeClass("selected");
                chkHeader.removeAttr("checked");
            } else {
                $("td", $(this).closest("tr")).addClass("selected");
                if ($("[id*=chkEmpselect]", grid).length == $("[id*=chkEmpselect]:checked", grid).length) {
                    chkHeader.attr("checked", "checked");
                }
            }
        });

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
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidateAddDedPage('SaveAddDed')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="149" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateAddDedPage('SaveAddDed')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$resources:Controls,Save %>"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save"
                                            ValidationGroup="SaveAddDed" OnClick="ActionHandler" OnClientClick="javascript:ValidateAddDedPage('SaveAddDed')"
                                            TabIndex="150" />
                                    </li>
                                    <li runat="server" id="Li1">
                                        <asp:Button runat="server" ID="btnPrint" CommandName="PRINT" Text="<%$resources:Controls,Print %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" TabIndex="151" />
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
                                            OnClick="ActionHandler" Text="<%$resources:CancelAddnDedn %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelAddnDedn %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="155" ID="btnView" CommandName="VIEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,View %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                    <li runat="server" id="Li2">
                                        <asp:Button runat="server" ID="btnPrintList" CommandName="PRINTLISTING" Text="<%$resources:Controls,Print %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" TabIndex="155" />
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
                                            <asp:Label ID="lblSearchName" runat="server" Text="<%$ resources:Name%>" AssociatedControlID="txtSearchName"></asp:Label>
                                            <asp:TextBox ID="txtSearchName" runat="server" CssClass="input-half" MaxLength="200"
                                                onkeydown="limitText(this,200);" onkeyup="limitText(this,200);" TabIndex="1"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblSrchFromDate" runat="server" Text="<%$ resources:FromDateH%>" AssociatedControlID="txtSrchFromDate"></asp:Label>
                                            <asp:TextBox ID="txtSrchFromDate" runat="server" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;" TabIndex="3"></asp:TextBox>
                                            <asp:Label ID="lblSrchToDate" runat="server" Text="<%$ resources:ToDateH%>" AssociatedControlID="txtSrchToDate"
                                                CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:TextBox ID="txtSrchToDate" runat="server" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;" TabIndex="4"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7 ">
                                            <asp:Label ID="lblEmpName" runat="server" Text="<%$ resources:Employee%>" AssociatedControlID="txtEmpName"></asp:Label>
                                            <asp:TextBox ID="txtEmpName" runat="server" CssClass="input-half" MaxLength="200"
                                                TabIndex="2"></asp:TextBox>
                                            <asp:HiddenField ID="hdfEmpName" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-b" TabIndex="5">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                                <%--   <asp:ListItem Text="<%$ Resources:Captions,NotPosted %>" Value="0" Enabled="<%$ resources:ConfigurationsRes,FinModuleEnabled %>"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Posted %>" Value="1" Enabled="<%$ resources:ConfigurationsRes,FinModuleEnabled %>"></asp:ListItem>--%>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblSrchType" runat="server" Text="<%$ resources:TypeH%>" AssociatedControlID="ddlSrchType"></asp:Label>
                                            <asp:DropDownList ID="ddlSrchType" runat="server" CssClass="select-w21-6per margnbotm0"
                                                AutoPostBack="true" OnSelectedIndexChanged="ActionHandler" TabIndex="6">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblSrchPayElement" runat="server" Text="<%$ resources:PayElementH%>"
                                                AssociatedControlID="ddlSrchPayElement" CssClass="lbl-17perc"></asp:Label>
                                            <asp:DropDownList ID="ddlSrchPayElement" runat="server" CssClass="select-w21-6per margnbotm0"
                                                TabIndex="7">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblTrxNoSearch" runat="server" Text="<%$ resources:TrxNo%>" AssociatedControlID="txtTrxNo"></asp:Label>
                                            <asp:TextBox ID="txtTrxNo" runat="server" CssClass="input-small-b1 margnbotm0" TabIndex="8"></asp:TextBox>
                                            <asp:HiddenField ID="hdfTrxPk" runat="server" />
                                            <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Search%>" ToolTip="<%$ resources:Search%>"
                                                OnClick="ActionHandler" CommandName="SEARCH" SkinID="search-ext" CssClass="margntop2 margnbotm0"
                                                ValidationGroup="Search" TabIndex="9" />
                                            <asp:ImageButton ID="imgClearSearch" runat="server" Text="<%$ resources:Clear%>"
                                                ToolTip="<%$ resources:Clear%>" OnClick="ActionHandler" CommandName="CLEARSEARCH"
                                                SkinID="clear-ext" CssClass="margntop2 margnbotm0" TabIndex="10" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdAddDedList" Width="100%" AllowPaging="false"
                                    AllowSorting="false" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    TabIndex="11" />
                                                <asp:HiddenField runat="server" ID="hdfAddDedPK" Value='<%# Eval("OAH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfDept" Value='<%# Eval("OAH_DEPT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfDelStatus" Value='<%# Eval("OAH_DEL_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TrxNo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("OAH_NO")))?Resources.ErpRes.Draft:Eval("OAH_NO")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("OAH_NO")))?Resources.ErpRes.Draft:Eval("OAH_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:FromDateH%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAddDedFromDate" runat="server" Text='<%#Eval("OAH_DATE_FROM", Resources.Constants.HRMSDateFormatGrid) %>'
                                                    ToolTip='<%# Eval("OAH_DATE_FROM", Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                            <HeaderStyle Width="7%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ToDateH%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAddDedToDate" runat="server" Text='<%#Eval("OAH_DATE_TO", Resources.Constants.HRMSDateFormatGrid) %>'
                                                    ToolTip='<%# Eval("OAH_DATE_TO", Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="6%" />
                                            <ItemStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TypeH%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAddDedType" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("OAH_CLASS_TEXT"),25) %>'
                                                    ToolTip='<%# Eval("OAH_CLASS_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PayElementH%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAddDedPayElement" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("OAH_PAY_ELEMENT_TEXT"),30) %>'
                                                    ToolTip='<%# Eval("OAH_PAY_ELEMENT_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Name%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAddDedName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("OAH_DISP_NAME"))),25) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("OAH_DISP_NAME"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="18%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrencyList" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("OAH_CURRENCY_CODE_TEXT"),10) %>'
                                                    ToolTip='<%# Eval("OAH_CURRENCY_NAME_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmountList" runat="server" Text='<%#GetFormattedCurrency(Eval("OAD_AMOUNT_TOTAL")) %>'
                                                    ToolTip='<%#GetFormattedCurrency(Eval("OAD_AMOUNT_TOTAL")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="amount-numeric" />
                                            <HeaderStyle Width="8%" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Description%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAddDedDescription" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("OAH_DESC"))),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("OAH_DESC"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgStatus" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("OAH_CSS_CLASS") %>' ToolTip='<%# Eval("OAH_STATUS_TEXT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc2:PagerControl ID="uclPaging" runat="server" TabIndex="12" />
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
                                            <asp:Label ID="lblType" runat="server" Text="<%$ resources:Type%>" AssociatedControlID="ddlType"
                                                CssClass="lbl-14-5perc"></asp:Label>
                                            <asp:DropDownList ID="ddlType" runat="server" CssClass="select-small-c1" AutoPostBack="true"
                                                OnSelectedIndexChanged="ActionHandler" TabIndex="1">
                                            </asp:DropDownList>
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="rfvType" CssClass="star" SetFocusOnError="true" ValidationGroup="SaveAddDed"
                                                    EnableClientScript="true" InitialValue="-1" runat="server" ControlToValidate="ddlType"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Type %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCompanyHd" runat="server" Text="<%$ resources:Controls,CompanyReq%>"
                                                AssociatedControlID="ddlCompany"></asp:Label>
                                            <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="1" CssClass="select-w61per">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvCompany" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlCompany" Display="Dynamic" Text="*" InitialValue="-1"
                                                ValidationGroup="SaveAddDed" ErrorMessage="<%$ resources:Err_Companay %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblPayElement" runat="server" Text="<%$ resources:PayElement%>" AssociatedControlID="ddlPayElement"></asp:Label>
                                            <asp:DropDownList ID="ddlPayElement" runat="server" CssClass="select-w61per" TabIndex="4"
                                                onchange="javascript:PayelemtChange();">
                                            </asp:DropDownList>
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="rfvPayElement" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="SaveAddDed" EnableClientScript="true" InitialValue="-1" runat="server"
                                                    ControlToValidate="ddlPayElement" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PayElement %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblName" runat="server" Text="<%$ resources:Name%>" AssociatedControlID="txtName"></asp:Label>
                                            <asp:TextBox ID="txtName" runat="server" CssClass="input-half" MaxLength="200" onkeydown="limitText(this,200);"
                                                onkeyup="limitText(this,200);" TabIndex="4"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblFromDate" Text="<%$ resources:FromDate%>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFromDate" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;" TabIndex="4"></asp:TextBox>
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="rfvFromDate" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="SaveAddDed" EnableClientScript="true" runat="server" ControlToValidate="txtFromDate"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_FromDate %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            <asp:Label runat="server" ID="lblToDate" Text="<%$ resources:ToDate%>" AssociatedControlID="txtToDate"
                                                CssClass="middle-lbl-c"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtToDate" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;" TabIndex="4"></asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="rfvToDate" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="SaveAddDed" EnableClientScript="true" runat="server" ControlToValidate="txtToDate"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ToDate %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblhdrCurrency" Text="<%$ resources:CurrencyReq%>"
                                                AssociatedControlID="txtCurrency"></asp:Label>
                                            <asp:TextBox ID="txtCurrency" runat="server" CssClass="input-small" TabIndex="5"
                                                MaxLength="100" Enabled="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvCurrency" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="SaveAddDed" EnableClientScript="true" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>"
                                                runat="server" ControlToValidate="txtCurrency" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_Currency %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfCurrency" runat="server" />
                                            <asp:Button ID="btnCurrency" runat="server" OnClick="ActionHandler" CommandName="CURRENCYSELECTED"
                                                Style="display: none" EnableTheming="false" />
                                            <asp:Label runat="server" ID="lblExchangeRate" Text="<%$ resources:ExchangeRateReq%>"
                                                AssociatedControlID="txtExchangeRate" CssClass="lbl-19-6perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtExchangeRate" Text="" TabIndex="5" CssClass="input-small numeric medium"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfExchangeRate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="SaveAddDed" EnableClientScript="true" runat="server" ControlToValidate="txtExchangeRate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExchangeRate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cmpExchangeRate" CssClass="star" SetFocusOnError="true"
                                                Type="Double" Operator="GreaterThan" ValueToCompare="0" ValidationGroup="SaveAddDed"
                                                EnableClientScript="true" InitialValue="0" runat="server" ControlToValidate="txtExchangeRate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExchangeRate %>">
                                            </asp:CompareValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblDescription" runat="server" Text="<%$ resources:Description%>"
                                                AssociatedControlID="txtDescription"></asp:Label>
                                            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="multiline-2a-line input-full"
                                                MaxLength="500" TabIndex="5"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div id="divImportSec" runat="server">
                                <div class="search-colapse-b">
                                    <h1>
                                        <asp:Literal ID="litEmployeeAddDedImport" runat="server" Text="<%$ resources: EmployeeImport %>" /></h1>
                                    <asp:ImageButton runat="server" ID="imbShowImport" OnClientClick="javascript:return ShowHideImport(1);"
                                        SkinID="imbArrowShow" ToolTip="<%$ resources:Controls,ShowDetails%>" />
                                    <asp:ImageButton runat="server" ID="imbHideImport" OnClientClick="javascript:return ShowHideImport();"
                                        Style="display: none" SkinID="imbArrowHide" ToolTip="<%$ resources:Controls,HideDetails%>" />
                                    <div class="clear">
                                    </div>
                                </div>
                                <div id="divImport" style="display: none;">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-S padgtop7 padgbotm3 margnbotm5">
                                                    <asp:UpdatePanel ID="aupdpnlImport" runat="server">
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
                                                            <asp:Button runat="server" ID="btnImport" CommandName="IMPORT" TabIndex="76" Text="<%$resources:Import %>"
                                                                OnClick="ActionHandler" ToolTip="<%$resources:Import %>" SkinID="btnInner-add"
                                                                ValidationGroup="SaveAddDed" OnClientClick="javascript:ValidatePage('SaveAddDed')"
                                                                Style="margin-bottom: 3px !important;" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnImport" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S txt-rgt">
                                                    <a id="aTmpDwn" class="btnInner-dwn btnInner-med-size decoration-none margnrgt13-5per"
                                                        href='<%= Page.ResolveClientUrl((string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()) ? "~/Upload/" : System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() )+ "Template/" + GetGlobalResourceObject("ConfigurationsRes", "HrmsImportTemplateAddDed").ToString())%>'>
                                                        <%= Resources.Controls.Template.ToString() %>
                                                    </a>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    <asp:Literal ID="ltrEmployeeAddDed" runat="server" Text="<%$ resources: EmployeeAddDed %>" /></h1>
                                <asp:ImageButton runat="server" ID="imbShowEmployeeAddDed" OnClientClick="javascript:return ShowHideEmployeeAddDed(1);"
                                    SkinID="imbArrowShow" ToolTip="<%$ resources:Controls,ShowDetails%>" />
                                <asp:ImageButton runat="server" ID="imbHideEmployeeAddDed" OnClientClick="javascript:return ShowHideEmployeeAddDed();"
                                    Style="display: none" SkinID="imbArrowHide" ToolTip="<%$ resources:Controls,HideDetails%>" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divEmployeeAddDed">
                                <table class="table-devide tablelayout">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblAddDedDate" runat="server" Text="<%$ resources:AddDedDate%>" AssociatedControlID="txtAddDedDate"></asp:Label>
                                                <asp:TextBox ID="txtAddDedDate" runat="server" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                    onblur="ClearDateSelect()" MaxLength="11" onpaste="return false;" TabIndex="6"></asp:TextBox>
                                                <asp:HiddenField ID="hdfAddDedSlNo" runat="server" Value="0" />
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="rfvAddDedDate" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="AddToList" EnableClientScript="true" runat="server" ControlToValidate="txtAddDedDate"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_AddDedDate %>">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                                <asp:Label ID="lblEmploymentType" runat="server" Text="<%$ resources:EmploymentType%>"
                                                    AssociatedControlID="ddlEmploymentType" CssClass="lbl-20-5perc"></asp:Label>
                                                <asp:DropDownList ID="ddlEmploymentType" runat="server" TabIndex="7" CssClass="select-small-b"
                                                    onchange="javascript:InitEmployeeAuto();">
                                                </asp:DropDownList>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblEmployeeType" AssociatedControlID="ddlEmployeeType"
                                                    Text="<%$resources:EmployeeType%>"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlEmployeeType" CssClass="select-small-a1"
                                                    TabIndex="9" onchange="javascript:InitEmployeeAuto();">
                                                </asp:DropDownList>
                                                <asp:Label runat="server" ID="lblPaymentMode" AssociatedControlID="ddlPaymentMode"
                                                    CssClass="lbl-20-5perc" Text="<%$resources:PaymentMode %>"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlPaymentMode" CssClass="select-small-b" TabIndex="9"
                                                    onchange="javascript:InitEmployeeAuto();">
                                                </asp:DropDownList>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblAmount" runat="server" Text="<%$ resources:AddDedAmount%>" AssociatedControlID="txtAmount"></asp:Label>
                                                <asp:TextBox ID="txtAmount" CssClass="numeric input-small" runat="server" MaxLength="12"
                                                    TabIndex="11"></asp:TextBox>
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="rfvAmount" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="AddToList" EnableClientScript="true" runat="server" ControlToValidate="txtAmount"
                                                        Display="Static" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                    </asp:RequiredFieldValidator>
                                                    <cc1:AmountValidation ID="vamAmount" runat="server" ControlToValidate="txtAmount"
                                                        ErrorMessage="<%$ resources:Err_Invalid_Amount %>" NumberDigits="11" Display="Static"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="AddToList"></cc1:AmountValidation>
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblBranchLocation" runat="server" Text="<%$ resources:BranchLocation%>"
                                                    AssociatedControlID="ddlBranchLocation"></asp:Label>
                                                <asp:DropDownList ID="ddlBranchLocation" runat="server" CssClass="select-w21-6per"
                                                    onchange="javascript:InitEmployeeAuto();" TabIndex="8">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblDepartment" runat="server" CssClass="middle-lbl-xsmall-a3" Text="<%$ resources:Department%>"
                                                    AssociatedControlID="txtDepartment"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDepartment" Text="" TabIndex="8" CssClass="input-w23-6per"></asp:TextBox>
                                                <asp:HiddenField ID="hdfDepartment" Value="-1" runat="server" />
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblEmployee" Text="<%$ resources:AddDedEmployee%>"
                                                    AssociatedControlID="txtEmployee" CssClass="lbl-25-1perc"></asp:Label>
                                                <asp:TextBox ID="txtEmployee" runat="server" CssClass="input-half" MaxLength="100"
                                                    TabIndex="10"></asp:TextBox>
                                                <asp:HiddenField ID="hdfEmployee" runat="server" />
                                                <div class="starwrap">
                                                    <%--  <asp:RequiredFieldValidator ID="rfvEmployee" runat="server" ControlToValidate="txtEmployee"
                                                        Display="Dynamic" CssClass="star" ValidationGroup="AddToList" Text="*" ErrorMessage="<%$ resources:Err_Employee%>"
                                                        InitialValue="<%$resources:ErpRes,AutoDefaultValue %>"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvNoEmployee" runat="server" ControlToValidate="txtEmployee"
                                                        Display="Dynamic" CssClass="star" ValidationGroup="AddToList" Text="*" ErrorMessage="<%$ resources:Err_Employee%>">
                                                    </asp:RequiredFieldValidator>--%>
                                                </div>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblRemarks" runat="server" Text="<%$ resources:Remarks%>" AssociatedControlID="txtRemarks"
                                                    CssClass="lbl-25-1perc"></asp:Label>
                                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="input-half" TabIndex="12"></asp:TextBox>
                                                <asp:ImageButton ID="imgAddNewDetails" runat="server" CommandName="ADDTOLIST" OnClick="ActionHandler"
                                                    ValidationGroup="AddToList" SkinID="imbaddnew" OnClientClick="javascript:ValidateAddDedPage('AddToList')"
                                                    TabIndex="13" CssClass="margntop2 margnbotm0" />
                                                <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Clear%>" ToolTip="<%$ resources:Clear%>"
                                                    OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext" TabIndex="14"
                                                    CssClass="margntop2 margnbotm0" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <%-- <tr>
                                        <td colspan="2">
                                            <div class="div2col-S">
                                                <asp:Label ID="lblRemarks" runat="server" Text="<%$ resources:Remarks%>" AssociatedControlID="txtRemarks"
                                                    CssClass="input-w12-5per"></asp:Label>
                                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" CssClass="multiline-2a-line input-full"
                                                    MaxLength="500" TabIndex="12"></asp:TextBox>
                                                <asp:ImageButton ID="imgAddNewDetails" runat="server" CommandName="ADDTOLIST" OnClick="ActionHandler"
                                                    ValidationGroup="AddToList" SkinID="imbaddnew" OnClientClick="javascript:ValidateAddDedPage('AddToList')"
                                                    CssClass="margntop18" TabIndex="13" />
                                                <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Clear%>" ToolTip="<%$ resources:Clear%>"
                                                    OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop18"
                                                    TabIndex="14" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>--%>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdEmpAddDedList" Width="100%" AllowPaging="false"
                                    OnRowDataBound="ActionHandler" AllowSorting="false" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%> " SortExpression="">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkEmpHeader" runat="server" ToolTip="Select for Process" TabIndex="15" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chkEmpselect" TabIndex="16" />
                                                <%-- <asp:HiddenField ID="hdfitemPK" runat="server" Value='<%#Eval("OAD_PK")%>' />--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEmpDate" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("OAD_DATE", Resources.Constants.HRMSDateFormatShortGrid),12)                                                 %>'
                                                    CssClass='<%# Convert.ToInt32(Eval("OAD_PAYROLL_DTL")) <=0 ? "input-w80" : "input-w80 input-disabled" %>'
                                                    Enabled='<%# Convert.ToInt32(Eval("OAD_PAYROLL_DTL")) <= 0 ? true : false %>'
                                                    ToolTip='<%# Eval("OAD_DATE", Resources.Constants.HRMSDateFormatGrid) %>'></asp:TextBox>
                                                <asp:HiddenField ID="hdfitemPK" runat="server" Value='<%#Eval("OAD_PK")%>' />
                                                <asp:HiddenField ID="hdfitemSlNo" runat="server" Value='<%#Eval("SlNo")%>' />
                                                <asp:HiddenField ID="hdfgrdEmpPk" runat="server" Value='<%#Eval("OAD_EMPLOYEE")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="6%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BranchLocation%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBranchText" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EmpBranch_Text"))),23) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EmpBranch_Text"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Employee%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployeeText" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("OAD_EMPLOYEE_NAME"))),44) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("OAD_EMPLOYEE_NAME"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:EmployeeType%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpTypeText" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EMP_TYPE_TEXT"))),23) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EMP_TYPE_TEXT"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtgrdAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("OAD_AMOUNT")) %>'
                                                    TabIndex="12" CssClass="input-w65per numeric" MaxLength="15" ToolTip='<%#GetFormattedCurrency(Eval("OAD_AMOUNT")) %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfAmount" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="SaveAddDed" EnableClientScript="true" runat="server" ControlToValidate="txtgrdAmount"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                </asp:RequiredFieldValidator>
                                                <cc1:AmountValidation ID="vamAmount" runat="server" ControlToValidate="txtgrdAmount"
                                                    ErrorMessage="<%$ resources:Err_Invalid_Amount %>" NumberDigits="11" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="SaveAddDed"></cc1:AmountValidation>
                                                <%--<asp:Label ID="lblEmpAmount" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("OAD_AMOUNT"))%>'
                                                    ToolTip='<%#GetFormattedCurrencyWithComma(Eval("OAD_AMOUNT"))%>'></asp:Label>--%>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="amount-numeric" />
                                            <HeaderStyle Width="6%" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Remarks%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtgrdRemarks" runat="server" Text='<%# System.Web.HttpUtility.HtmlDecode( Convert.ToString(Eval("OAD_REMARKS"))) %>'
                                                    CssClass="input-w98per" MaxLength="500" ToolTip='<%# System.Web.HttpUtility.HtmlDecode( Convert.ToString(Eval("OAD_REMARKS"))) %>'
                                                    onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" TabIndex="12"></asp:TextBox>
                                                <%--<asp:Label ID="lblEmpRemarks" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("OAD_REMARKS"),40) %>'
                                                    ToolTip='<%# Eval("OAD_REMARKS") %>'></asp:Label>--%>
                                            </ItemTemplate>
                                            <HeaderStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <HeaderTemplate>
                                                <asp:ImageButton runat="server" ID="imbDeleteAll" SkinID="imbdeletegrid" CommandName="DELETEALL"
                                                    CssClass="margn-rgt0" ToolTip="<%$resources:Controls,DeleteAll %>" OnClick="ActionHandler"
                                                    OnClientClick="return ShowDeleteConfirmationMsg(this);" TabIndex="15" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imbEditPayroll" SkinID="imbeditgrid" CommandName="EDITITEM"
                                                    ToolTip="<%$ resources:Controls,Edit%>" OnClick="ActionHandler" TabIndex="15"
                                                    Visible="false" />
                                                <asp:ImageButton runat="server" ID="imbDeletePayroll" SkinID="imbdeletegrid" CommandName="REMOVEITEM"
                                                    ToolTip="<%$ resources:Controls,Delete%>" OnClick="ActionHandler" OnClientClick="return ShowDeleteConfirmationMsg(this);"
                                                    TabIndex="15" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPayrollDtl" runat="server" Text='<%# Eval("OAD_PAYROLL_DTL") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" CssClass="amount-numeric" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary
                    ID="vsPage" ValidationGroup="SaveAddDed" runat="server" />
                <asp:ValidationSummary ID="vsAddtoList" ValidationGroup="AddToList" runat="server" />
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="SaveAddDed">
                </uc1:WorkflowUserComments>
            </div>
            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormatWithComma" runat="server" />
            <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
            <asp:HiddenField ID="hdfAdvSearch" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
            <asp:HiddenField ID="hdfJournalizeWorkFlow" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIsCancelled" Value="0" runat="server" />
            <asp:HiddenField ID="hdfExchangeRateFormat" runat="server" />
            <asp:HiddenField ID="hdfCurrencyMode" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
