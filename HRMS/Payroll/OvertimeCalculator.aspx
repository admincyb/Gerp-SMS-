<%@ Page Title="<%$ Resources:Captions,Title_OverTimeCalculator %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="OvertimeCalculator.aspx.cs"
    Inherits="HRMS.Payroll.OvertimeCalculator" Theme="ClassicExt" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            $(document).ready(function () {
                GrandScriptUtils.AddDateRangeCommon("txtFilterFromDate", "hdfFilterFromDate", "txtFilterToDate", "hdfFilterToDate", false, false);
                GrandScriptUtils.MakeAutoCompleteDDL("txtFilterBranch", url, "hdfFilterBranch", true, true, "BRANCHLOCATION");
                GrandScriptUtils.MakeAutoCompleteDDL("txtFilterDept", url, "hdfFilterDept", true, true, "DEPARTMENTAUTOCOMPLETE");
                GrandScriptUtils.DatePickerCommon("txtDate");
                GrandScriptUtils.MakeAutoCompleteDDL("txtHdDepartment", url, "hdfHdDepartment", true, true, "DEPARTMENTAUTOCOMPLETE");
                GrandScriptUtils.MakeAutoCompleteDDL("txtHdBranchLocation", url, "hdfHdBranchLocation", true, true, "BRANCHLOCATION");
                GrandScriptUtils.MakeAutoCompleteDDL("txtDesignation", url, "hdfDesignation", true, true, "DESIGNATION");
                GrandScriptUtils.MakeAutoCompleteDDL("txtBranchLocation", url, "hdfBranchLocation", true, true, "BRANCHLOCATION");
                GrandScriptUtils.MakeAutoCompleteDDL("txtDepartment", url, "hdfDepartment", true, true, "DEPARTMENTAUTOCOMPLETE");
                GrandScriptUtils.MakeAutoCompleteDDL("txtTrxNo", url, "hdfTrxPk", true, true, "EOTNUMBER");
                //GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee", url + "?EmpCategory=2", "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETE");
                $("[id$=btnDateChanged]").hide();

                var deptid = parseInt($("[id$=hdfHdDepartment]").val());
                if (deptid > 0) {
                    DisableAuto($("[id$=txtDepartment]"), $("[id$=hdfDepartment]"));
                    //BindEmployee();
                }
                var branchid = parseInt($("[id$=hdfHdBranchLocation]").val());
                if (branchid > 0)
                    DisableAuto($("[id$=txtBranchLocation]"), $("[id$=hdfBranchLocation]"));
                InitEmployeeAuto();

            });

        }

        function BindEmployee() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee ", url + "?EmpCategory=2" + "&EmpDept=" + $("[id$=hdfDepartment]").val() + "&EmpBranch=" + $("[id$=hdfBranchLocation]").val() + "&EmpType=" + $("[id$=ddlEmployeeType]").val() + "&EmploymentType=" + $("[id$=ddlEmploymentType]").val() + "&EmpCompany=" + $("[id$=ddlCompany]").val() + "&EmpDesignation=" + $("[id$=hdfDesignation]").val() + "&ToDate=" + $("[id$=txtDate]").val(), "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETE", "", true, true, false, 1, '<%= Resources.ErpRes.All_Small %>');
        }

        function InitEmployeeAuto() {
            BindEmployee();
            ResetEmployee();
        }

        //        function BindEmployee() {
        //            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee ", url + "?EmpCategory=2" + "&EmpDept=" + $("[id$=hdfDepartment]").val(), "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETE", "", true, true, false, 1, '<%= Resources.ErpRes.All_Small %>');
        //        }
        //To excecute after  auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtHdBranchLocation") {
                SetBranchDeptSearch(1, 0);
                BindEmployee();
                ResetEmployee();
            }
            if (targetControlID == "txtHdDepartment") {
                SetBranchDeptSearch(0, 0);
                BindEmployee();
                ResetEmployee();
            }
            if (targetControlID == "txtDepartment" || targetControlID == "txtBranchLocation" || targetControlID == "txtDesignation") {
                BindEmployee();
                ResetEmployee();
            }
        }

        //To excecute after  auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtHdBranchLocation") {
                $("[id$=hdfHdBranchLocation]").val("-1");
                SetBranchDeptSearch(1, 1);
                BindEmployee();
                ResetEmployee();
            }
            if (targetControlID == "txtHdDepartment") {
                $("[id$=hdfHdDepartment]").val("-1");
                SetBranchDeptSearch(0, 1);
                BindEmployee();
                ResetEmployee();
            }
            if (targetControlID == "txtDepartment" || targetControlID == "txtBranchLocation" || targetControlID == "txtDesignation") {
                BindEmployee();
                ResetEmployee();
            }
        }

        function ResetEmployee() {
            var defText = '<%= Resources.ErpRes.All_Small %>';
            $("[id$=txtEmployee]").val(defText);
            $("[id$=hdfEmployee]").val('-1');
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
        function ShowHideAdvanceSearch(flag) {
            if (flag == 1) {
                $("[id$=divEmployeeFilterDetails").show();
                $("[id$=imbShowFilterDetails").hide();
                $("[id$=imbHideFilterDetails").show();
            }
            else {
                $("[id$=divEmployeeFilterDetails").hide();
                $("[id$=imbShowFilterDetails").show();
                $("[id$=imbHideFilterDetails").hide();
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


        function AfterDateSelect(controlID) {
            if (controlID == "txtDate") {
                //                alert('Last:' + $("[id$=hdfLastSelectedDate]").val());
                //                alert('New:' + $("[id$=txtDate]").val());
                if ($("[id$=hdfLastSelectedDate]").val().slice(3, 11) != $("[id$=txtDate]").val().slice(3, 11)) {
                    $("[id$=hdfLastSelectedDate]").val($("[id$=txtDate]").val());
                    // $("[id$=btnDateChanged]").click();
                }
                InitEmployeeAuto();
            }
        }

        function ClearDateSelect() {
            if ($("[id$=txtDate]").val() == '') {
                InitEmployeeAuto();
            }
        }
        //show confirmation msg for unsaved records
        function ShowMsgUnsaved() {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Msg_Unsaved_Exist").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfIscontYes]").val(1);
                        $(this).dialog("close");
                        $("[id$=imbDetSearch]").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfIscontYes]").val(0);
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }


        function SetBranchDeptSearch(mode, isInvalid) {
            //mode  0:dept,1:Branch
            var defaultText = '<%= Resources.ErpRes.AutoDefaultValue %>';
            if (parseInt(mode) == 0) {
                if (parseInt(isInvalid) == 1) {
                    $("[id$=txtDepartment]").val(defaultText);
                    $("[id$=hdfDepartment]").val("-1");
                    EnableAuto($("[id$=txtDepartment]"), $("[id$=hdfDepartment]"));
                }
                else {
                    var dept = $("[id$=txtHdDepartment]").val();
                    var deptid = parseInt($("[id$=hdfHdDepartment]").val());
                    $("[id$=txtDepartment]").val(dept);
                    $("[id$=hdfDepartment]").val(deptid);
                    DisableAuto($("[id$=txtDepartment]"), $("[id$=hdfDepartment]"));
                }
            }
            else if (parseInt(mode) == 1) {
                if (parseInt(isInvalid) == 1) {
                    $("[id$=txtBranchLocation]").val(defaultText);
                    $("[id$=hdfBranchLocation]").val("-1");
                    EnableAuto($("[id$=txtBranchLocation]"), $("[id$=hdfBranchLocation]"));
                }
                else {
                    var branch = $("[id$=txtHdBranchLocation]").val();
                    var branchid = parseInt($("[id$=hdfHdBranchLocation]").val());
                    $("[id$=txtBranchLocation]").val(branch);
                    $("[id$=hdfBranchLocation]").val(branchid);
                    DisableAuto($("[id$=txtBranchLocation]"), $("[id$=hdfBranchLocation]"));
                }
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

        function HideTextBox(ddlId) {
            var selectedValue = ddlId.value;
            $("[id$=ddlCompany]").val(selectedValue);
            if (selectedValue > 0) {
                $("[id$=ddlCompany]").attr("disabled", true);
            } else {
                $("[id$=ddlCompany]").attr("disabled", false);
            }
            BindEmployee();
            ResetEmployee();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlTaskHome" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdfCurrentELD_PK" runat="server" />
            <asp:HiddenField ID="hdfCurrentROW_NO" runat="server" />
            <asp:HiddenField ID="hdfLastSelectedDate" runat="server" />
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="1" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="1" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$resources:Controls,Save %>"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save"
                                            ValidationGroup="Save" OnClick="ActionHandler" OnClientClick="javascript:ValidatePage('save')"
                                            TabIndex="10" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="10" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            CssClass="popupclose" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" OnClick="ActionHandler"
                                            TabIndex="10" />
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
                                CommandArgument="SEC_ActionPanel" TabIndex="99" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="99" OnClick="ActionHandler" CommandName="DETAIL"
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
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7 ">
                                            <asp:Label ID="lblfilterBranch" runat="server" Text="<%$ resources:BrLoc%>" AssociatedControlID="txtFilterBranch"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterBranch" Text="" TabIndex="12" CssClass="select-small-i"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterBranch" Value="-1" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7 ">
                                            <asp:Label ID="lblFilterDept" runat="server" Text="<%$ resources:Dept%>" AssociatedControlID="txtFilterDept"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterDept" Text="" TabIndex="12" CssClass="select-small-i"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterDept" Value="-1" runat="server" />
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
                                            <asp:TextBox runat="server" ID="txtFilterFromDate" TabIndex="11" CssClass="input-small margnbotm0"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblFilterToDate" runat="server" Text="<%$ resources:ToDate%>" AssociatedControlID="txtFilterToDate"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterToDate" TabIndex="11" CssClass="input-small margnbotm0"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblTrxNoSearch" runat="server" Text="<%$ resources:TrxNo%>" AssociatedControlID="txtTrxNo"></asp:Label>
                                            <asp:TextBox ID="txtTrxNo" runat="server" CssClass="input-small-b1 margnbotm0" TabIndex="12"></asp:TextBox>
                                            <asp:HiddenField ID="hdfTrxPk" runat="server" />
                                            <asp:ImageButton ID="imgListFilter" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="13"
                                                CommandName="FILTER" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="imgListClear" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="13" OnClick="ActionHandler"
                                                CommandName="CLEARSEARCH" SkinID="clear-ext" CssClass="margntop2 margnlft-minus2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdList" Width="100%" AllowPaging="false" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" OnSorting="ActionHandler" TabIndex="13">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" TabIndex="14"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfRowNoListPage" Value='<%# Eval("ROW_NO") %>' />
                                                <asp:HiddenField runat="server" ID="hdfEOEPKList" Value='<%# Eval("EOE_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TrxNo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("EOE_NO")))?Resources.ErpRes.Draft:Eval("EOE_NO")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("EOE_NO")))?Resources.ErpRes.Draft:Eval("EOE_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEOE_TO_DATE" runat="server" Text='<%#Eval("EOE_TO_DATE", Resources.Constants.HRMSDateFormatGrid) %>'
                                                    ToolTip='<%# Eval("EOE_TO_DATE", Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Branch/Location%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEOE_BRANCH_TEXT" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EOE_BRANCH_TEXT")),35) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EOE_BRANCH_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                            <HeaderStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Department%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblempDepartment_TEXT" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EOE_empDepartment_TEXT")),35) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EOE_empDepartment_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                            <HeaderStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Remarks %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEOE_REMARKS" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("EOE_REMARKS"),60) %>'
                                                    ToolTip='<%# Eval("EOE_REMARKS") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
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
                                            <asp:Label ID="lblTrxNoHdr" runat="server" Text="<%$ resources:TrxNo%>" AssociatedControlID="lblTrxNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblTrxNo" CssClass="input-small"></asp:Label>
                                            <asp:Label ID="lblDate" runat="server" Text="<%$ resources:DateStar%>" AssociatedControlID="txtDate"
                                                CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:TextBox ID="txtDate" runat="server" MaxLength="200" CssClass="input-small" TabIndex="2"
                                                onblur="ClearDateSelect()" onkeydown="return CheckKey(event)" onpaste="return false;" />
                                            <asp:RequiredFieldValidator ID="vrfDate" CssClass="star" SetFocusOnError="true" ValidationGroup="save"
                                                EnableClientScript="true" runat="server" ControlToValidate="txtDate" Display="Dynamic"
                                                Text="*" ErrorMessage="<%$ resources:Err_Date %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCompanyHd" runat="server" Text="<%$ resources:Controls,CompanyReq%>"
                                                AssociatedControlID="ddlCompanyHd"></asp:Label>
                                            <asp:DropDownList ID="ddlCompanyHd" runat="server" TabIndex="2" CssClass="select-w61per" onchange="HideTextBox(this);"  >
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvCompanyHd" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlCompanyHd" Display="Dynamic" Text="*" InitialValue="-1"
                                                ValidationGroup="save" ErrorMessage="<%$ resources:Err_SelectCompanay %>">
                                            </asp:RequiredFieldValidator></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblHdBranchLocation" runat="server" Text="<%$ resources:Branch/Location%>"
                                                AssociatedControlID="txtHdBranchLocation"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtHdBranchLocation" Text="" TabIndex="2" CssClass="select-half"></asp:TextBox>
                                            <asp:HiddenField ID="hdfHdBranchLocation" Value="-1" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblHdDepartment" runat="server" Text="<%$ resources:Department%>"
                                                AssociatedControlID="txtHdDepartment"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtHdDepartment" Text="" TabIndex="3" CssClass="select-half"></asp:TextBox>
                                            <asp:HiddenField ID="hdfHdDepartment" Value="-1" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks%>" AssociatedControlID="txtRemarks"></asp:Label>
                                            <asp:TextBox ID="txtRemarks" runat="server" TabIndex="3" MaxLength="500" TextMode="MultiLine"
                                                Height="40" CssClass="input-full"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    <asp:Literal ID="ltrPaymentFilter" runat="server" Text="<%$ resources:AdvanceSearch %>" /></h1>
                                <asp:ImageButton runat="server" ID="imbShowFilterDetails" OnClientClick="javascript:return ShowHideAdvanceSearch(1);"
                                    TabIndex="4" SkinID="imbArrowShow" ToolTip="<%$ resources:Controls,ShowDetails%>" />
                                <asp:ImageButton runat="server" ID="imbHideFilterDetails" OnClientClick="javascript:return ShowHideAdvanceSearch();"
                                    TabIndex="4" Style="display: none" SkinID="imbArrowHide" ToolTip="<%$ resources:Controls,HideDetails%>" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divEmployeeFilterDetails" style="display: none">
                                <table class="table-devide tablelayout">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblCompany" Text="<%$ resources:Company%>" AssociatedControlID="ddlCompany"></asp:Label>
                                                <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="5" CssClass="select-half-a"
                                                    onchange="javascript:InitEmployeeAuto();">
                                                </asp:DropDownList>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblDepartment" runat="server" Text="<%$ resources:Department%>" AssociatedControlID="txtDepartment"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDepartment" Text="" TabIndex="6" CssClass="select-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfDepartment" Value="-1" runat="server" />
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblDesignation" runat="server" Text="<%$ resources:Designation%>"
                                                    AssociatedControlID="txtDesignation"></asp:Label>
                                                <asp:HiddenField ID="hdfDesignation" runat="server" Value="-1" />
                                                <asp:TextBox runat="server" TabIndex="7" ID="txtDesignation" CssClass="select-half" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblBranchLocation" runat="server" Text="<%$ resources:Branch/Location%>"
                                                    AssociatedControlID="txtBranchLocation"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtBranchLocation" Text="" TabIndex="5" CssClass="select-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfBranchLocation" Value="-1" runat="server" />
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblEmploymentType" runat="server" Text="<%$ resources:EmploymentType%>"
                                                    AssociatedControlID="ddlEmploymentType"></asp:Label>
                                                <asp:DropDownList ID="ddlEmploymentType" runat="server" TabIndex="6" CssClass="select-half-a"
                                                    onchange="javascript:InitEmployeeAuto();">
                                                </asp:DropDownList>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblEmployeeType" AssociatedControlID="ddlEmployeeType"
                                                    Text="<%$ resources:EmployeeType%>"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlEmployeeType" CssClass="select-half-a" TabIndex="10"
                                                    onchange="javascript:InitEmployeeAuto();">
                                                </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table class="table-devide tablelayout">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblEmployee" runat="server" Text="<%$ resources:Employee%>" AssociatedControlID="txtEmployee"></asp:Label>
                                            <asp:TextBox ID="txtEmployee" runat="server" TabIndex="7" CssClass="input-half" MaxLength="100"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfEmployee" runat="server" Value="-1" />
                                            <asp:ImageButton ID="btnGo" runat="server" Text="<%$ resources:Controls,Search%>"
                                                ToolTip="<%$ resources:Controls,Search%>" OnClientClick="javascript:ValidatePage('save')"
                                                ValidationGroup="save" OnClick="ActionHandler" TabIndex="8" CommandName="GO"
                                                SkinID="search-ext" CssClass="margntop2 margnbotm0 margn-rgt4" />
                                            <asp:ImageButton ID="btnClearDetails" runat="server" Text="<%$ resources:Controls,Clear%>"
                                                ToolTip="<%$ resources:Controls,Clear%>" TabIndex="8" OnClick="ActionHandler"
                                                CommandName="CLEARDETAIL" SkinID="clear-ext" CssClass="margntop2 margnbotm0 margn-rgt4" />
                                        </div>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <%--class="grdTable"--%>
                                <asp:GridView runat="server" ID="grdOTList" Width="100%" AllowPaging="false" AllowSorting="True"
                                    AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-CssClass="emptytable"
                                    OnRowCommand="ActionHandler" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkEmpHeader" runat="server" ToolTip="Select All Employee" TabIndex="15" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox CssClass="checkbox" runat="server" ID="chkEmpselect" Checked='<%# (Convert.ToInt32(Eval("EOT_PK")) > 0) ? true : false %>'
                                                    TabIndex="8" Enabled='<%# (Convert.ToInt32(Eval("EOT_PK")) > 0) ? false : true %>' />
                                                <asp:HiddenField runat="server" ID="hdfEOT_PK" Value='<%# Eval("EOT_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfEOE_PK" Value='<%# Eval("EOE_PK") %>' />
                                                <asp:HiddenField ID="hdfEmployeePk" runat="server" Value='<%# Eval("EOT_EMPLOYEE") %>' />
                                                <asp:HiddenField ID="hdfModDate" runat="server" Value='<%# Eval("EOT_MOD_DT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" Wrap="false" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" Text="" runat="server" Checked='<%# (Convert.ToInt32(Eval("IsChecked")) > 0? true : (Eval("Pk")!=null?(GetInt(Eval("Pk").ToString())>0?true:false):(false))) %>'
                                                    TabIndex="8" />
                                                <asp:HiddenField runat="server" ID="hdfEOT_PK" Value='<%# Eval("EOT_PK") %>' />
                                                <asp:HiddenField ID="hdfEmployeePk" runat="server" Value='<%# Eval("EOT_EMPLOYEE_PK") %>' />
                                                <asp:HiddenField ID="hdfCheckedFlag" runat="server" Value='<%# Eval("CheckedFlag") %>' />
                                                <asp:HiddenField ID="hdfModDate" runat="server" Value='<%# Eval("EOT_MOD_DT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Employee%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EOT_EMPLOYEE_TEXT")),100) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EOT_EMPLOYEE_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="35%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Location %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocationGridView" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("empBranchCode") ,18) %>'
                                                    ToolTip='<%# Eval("empBranchText")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Dept %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDept" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("DPT_CODE") ,20) + " - " + ERP.Utilities.CommonFunctions.GetShortString( Eval("empDepartmentText") ,20) %>'
                                                    ToolTip='<%# Eval("empDepartmentText")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:OTHours%>">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtOTHours" CssClass="small" ValidationGroup="save"
                                                    TabIndex="8" />
                                                <cc1:MaskedEditExtender ID="meeOTHours" runat="server" AutoComplete="false" Mask="<%$ resources:ConfigurationsRes,HrmsTimeMask %>"
                                                    MaskType="Time" TargetControlID="txtOTHours">
                                                </cc1:MaskedEditExtender>
                                                <asp:RegularExpressionValidator runat="server" ID="regOTHours" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="save" ControlToValidate="txtOTHours" Display="Dynamic" Text="*"
                                                    ErrorMessage="<%$ resources:Err_ValidOTHour %>" ValidationExpression="<%$ resources:ConfigurationsRes,HrmsTimeMaskValidationExp %>">
                                                </asp:RegularExpressionValidator>
                                                <asp:HiddenField ID="hdfOTHours" runat="server" Value='<%# Eval ("EOT_HOURS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Col1%>" Visible="false">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtOTHours1" CssClass="small" ValidationGroup="save"
                                                    TabIndex="8" />
                                                <cc1:MaskedEditExtender ID="meeOTHours1" runat="server" AutoComplete="false" Mask="<%$ resources:ConfigurationsRes,HrmsTimeMask %>"
                                                    MaskType="Time" TargetControlID="txtOTHours1">
                                                </cc1:MaskedEditExtender>
                                                <asp:RegularExpressionValidator runat="server" ID="regOTHours1" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="save" ControlToValidate="txtOTHours1" Display="Dynamic" Text="*"
                                                    ErrorMessage="<%$ resources:Err_ValidOTHour %>" ValidationExpression="<%$ resources:ConfigurationsRes,HrmsTimeMaskValidationExp %>">
                                                </asp:RegularExpressionValidator>
                                                <asp:HiddenField ID="hdfOTHours1" runat="server" Value='<%# Eval("EOT_HOURS1") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Col2%>" Visible="false">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtOTHours2" CssClass="small" ValidationGroup="save"
                                                    TabIndex="8" />
                                                <cc1:MaskedEditExtender ID="meeOTHours2" runat="server" AutoComplete="false" Mask="<%$ resources:ConfigurationsRes,HrmsTimeMask %>"
                                                    MaskType="Time" TargetControlID="txtOTHours2">
                                                </cc1:MaskedEditExtender>
                                                <asp:RegularExpressionValidator runat="server" ID="regOTHours2" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="save" ControlToValidate="txtOTHours2" Display="Dynamic" Text="*"
                                                    ErrorMessage="<%$ resources:Err_ValidOTHour %>" ValidationExpression="<%$ resources:ConfigurationsRes,HrmsTimeMaskValidationExp %>">
                                                </asp:RegularExpressionValidator>
                                                <asp:HiddenField ID="hdfOTHours2" runat="server" Value='<%# Eval("EOT_HOURS2") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Col3%>" Visible="false">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtOTHours3" CssClass="small" ValidationGroup="save"
                                                    TabIndex="8" />
                                                <cc1:MaskedEditExtender ID="meeOTHours3" runat="server" AutoComplete="false" Mask="<%$ resources:ConfigurationsRes,HrmsTimeMask %>"
                                                    MaskType="Time" TargetControlID="txtOTHours3">
                                                </cc1:MaskedEditExtender>
                                                <asp:RegularExpressionValidator runat="server" ID="regOTHours3" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="save" ControlToValidate="txtOTHours3" Display="Dynamic" Text="*"
                                                    ErrorMessage="<%$ resources:Err_ValidOTHour %>" ValidationExpression="<%$ resources:ConfigurationsRes,HrmsTimeMaskValidationExp %>">
                                                </asp:RegularExpressionValidator>
                                                <asp:HiddenField ID="hdfOTHours3" runat="server" Value='<%# Eval("EOT_HOURS3") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Col4%>" Visible="false">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtOTHours4" CssClass="small" ValidationGroup="save"
                                                    TabIndex="8" />
                                                <cc1:MaskedEditExtender ID="meeOTHours4" runat="server" AutoComplete="false" Mask="<%$ resources:ConfigurationsRes,HrmsTimeMask %>"
                                                    MaskType="Time" TargetControlID="txtOTHours4">
                                                </cc1:MaskedEditExtender>
                                                <asp:RegularExpressionValidator runat="server" ID="regOTHours4" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="save" ControlToValidate="txtOTHours4" Display="Dynamic" Text="*"
                                                    ErrorMessage="<%$ resources:Err_ValidOTHour %>" ValidationExpression="<%$ resources:ConfigurationsRes,HrmsTimeMaskValidationExp %>">
                                                </asp:RegularExpressionValidator>
                                                <asp:HiddenField ID="hdfOTHours4" runat="server" Value='<%# Eval("EOT_HOURS4") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Col5%>" Visible="false">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtOTHours5" CssClass="small" ValidationGroup="save"
                                                    Text='<%# Eval("EOT_HOURS5")%>' TabIndex="8" />
                                                <cc1:MaskedEditExtender ID="meeOTHours5" runat="server" AutoComplete="false" Mask="<%$ resources:ConfigurationsRes,HrmsTimeMask %>"
                                                    MaskType="Time" TargetControlID="txtOTHours5">
                                                </cc1:MaskedEditExtender>
                                                <asp:RegularExpressionValidator runat="server" ID="regOTHours5" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="save" ControlToValidate="txtOTHours5" Display="Dynamic" Text="*"
                                                    ErrorMessage="<%$ resources:Err_ValidOTHour %>" ValidationExpression="<%$ resources:ConfigurationsRes,HrmsTimeMaskValidationExp %>">
                                                </asp:RegularExpressionValidator>
                                                <asp:HiddenField ID="hdfOTHours5" runat="server" Value='<%# Eval("EOT_HOURS5") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbDeleteDetails"
                                                    SkinID="imbdeletegrid" CommandName="GRIDDELETE" OnClick="ActionHandler" ToolTip="<%$resources:Controls,Delete %>"
                                                    OnClientClick="return ShowDeleteConfirm(this);" TabIndex="8" Visible='<%# (Convert.ToInt32(Eval("EOT_PK")) > 0 && Convert.ToInt32(Eval("EOT_PAYROLL_DTL")) <= 0) ? true : false %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" Wrap="false" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <asp:ValidationSummary ID="vsPage" ValidationGroup="save" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:HiddenField
                    ID="hdfIscontYes" runat="server" />
            </div>
            <asp:HiddenField ID="hdfAdvSearch" Value="0" runat="server" />
            <asp:HiddenField runat="server" ID="hdfOTEntry" Value="1" />
            <asp:HiddenField runat="server" ID="hdfShowHideFilterSec" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
