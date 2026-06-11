<%@ Page Title="<%$ Resources:Captions,Title_HRMS_EmployeeLeaveMaster %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="EmployeeLeaveMaster.aspx.cs"
    Inherits="HRMS.Admin.Masters.EmployeeLeaveMaster" Theme="ClassicExt" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            $(document).ready(function () {
                GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
                GrandScriptUtils.AddDateRangeCommon("txtFilterFromDate", "hdfFilterFromDate", "txtFilterToDate", "hdfFilterToDate", false, false);
                GrandScriptUtils.MakeAutoCompleteDDL("txtBranchLocation", url, "hdfBranchLocation", true, true, "BRANCHLOCATION", "", true, true, false, 1, '<%= Resources.ErpRes.All_Small %>');
                GrandScriptUtils.MakeAutoCompleteDDL("txtDetSearchBranchLocation", url, "hdfDetSearchBranchLocation", true, true, "BRANCHLOCATION", "", true, true, false, 1, '<%= Resources.ErpRes.All_Small %>');
                GrandScriptUtils.MakeAutoCompleteDDL("txtSearchEmployee ", url + "?EmpCategory=2", "hdfSearchEmployee", true, true, "EMPLOYEEAUTOCOMPLETE");
                GrandScriptUtils.MakeAutoCompleteDDL("txtDepartment", url, "hdfDepartment", true, true, "DEPARTMENT", "", true, true, false, 1, '<%= Resources.ErpRes.All_Small %>');
                GrandScriptUtils.MakeAutoCompleteDDL("txtDetSearchDepartment", url, "hdfDetSearchDepartment", true, true, "DEPARTMENT", "", true, true, false, 1, '<%= Resources.ErpRes.All_Small %>');
                BindEmployee();
                //                $("[id$=txtDate]").on('input', function (e) {
                //                    $("[id$=btnDateChanged]").click();
                //                });
                $("[id$=btnDateChanged]").hide();
                RestrictDate();
                ShowHideDetSearch();
                BindEmployeeDetSearch();
            });
        }

        function BindEmployee() {
            if ($("[id$=hdfBranchLocation]").val() > 0) {
                GrandScriptUtils.MakeAutoCompleteDDL("txtDetailsEmployee", url + "?Type=" + $("[id$=hdfBranchLocation]").val() + "&EmpCategory=2" + "&EmpDept=" + $("[id$=hdfDepartment]").val() + "&EmpType=" + $("[id$=ddlEmployeeType]").val() + "&ToDate=" + $("[id$=txtDate]").val(), "hdfDetailsEmployee", true, true, "EMPLOYEEAUTOCOMPLETEBYFILTER", "", true, true, false, 1, '<%= Resources.ErpRes.All_Small %>');
            }
            else {
                GrandScriptUtils.MakeAutoCompleteDDL("txtDetailsEmployee", url + "?EmpCategory=2" + "&EmpDept=" + $("[id$=hdfDepartment]").val() + "&EmpType=" + $("[id$=ddlEmployeeType]").val() + "&ToDate=" + $("[id$=txtDate]").val(), "hdfDetailsEmployee", true, true, "EMPLOYEEAUTOCOMPLETE", "", true, true, false, 1, '<%= Resources.ErpRes.All_Small %>');
            }
        }
        function ResetEmployee() {
            var defText = '<%= Resources.ErpRes.All_Small %>';
            $("[id$=txtDetailsEmployee]").val(defText);
            $("[id$=hdfDetailsEmployee]").val('-1');
        }


        function BindEmployeeDetSearch() {
            if ($("[id$=hdfDetSearchBranchLocation]").val() > 0) {
                GrandScriptUtils.MakeAutoCompleteDDL("txtDetSerachEmployee", url + "?Type=" + $("[id$=hdfDetSearchBranchLocation]").val() + "&EmpCategory=2" + "&EmpDept=" + $("[id$=hdfDetSearchDepartment]").val() + "&EmpType=" + $("[id$=ddlDetSearchEmployeeType]").val(), "hdfDetSerachEmployee", true, true, "EMPLOYEEAUTOCOMPLETEBYFILTER", "", true, true, false, 1, '<%= Resources.ErpRes.All_Small %>');
            }
            else {
                GrandScriptUtils.MakeAutoCompleteDDL("txtDetSerachEmployee", url + "?EmpCategory=2" + "&EmpDept=" + $("[id$=hdfDetSearchDepartment]").val() + "&EmpType=" + $("[id$=ddlDetSearchEmployeeType]").val(), "hdfDetSerachEmployee", true, true, "EMPLOYEEAUTOCOMPLETE", "", true, true, false, 1, '<%= Resources.ErpRes.All_Small %>');
            }
        }
        function ResetEmployeeDetSearch() {
            var defText = '<%= Resources.ErpRes.All_Small %>';
            $("[id$=txtDetSerachEmployee]").val(defText);
            $("[id$=hdfDetSerachEmployee]").val('-1');
        }


        function RestrictDate() {
            GrandScriptUtils.RestrictedDatePicker("txtDate", false, true, true, $("[id$=txtFromDate]").val(), $("[id$=txtToDate]").val());
        }
        function AfterDateSelect(controlID) {
            if (controlID == "txtFromDate" || controlID == "txtToDate") {
                RestrictDate();
            }
            else if (controlID == "txtDate") {
                BindEmployee();
                ResetEmployee();
            }
        }

        function ClearDateSelect() {
            if ($("[id$=txtDate]").val() == '') {
                BindEmployee();
                ResetEmployee();
            }
        }

        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtDepartment" || targetControlID == "txtBranchLocation") {
                BindEmployee();
                ResetEmployee()
            }
            else if (targetControlID == "txtDetSearchDepartment" || targetControlID == "txtDetSearchBranchLocation") {
                BindEmployeeDetSearch();
                ResetEmployeeDetSearch()
            }
            else if (targetControlID == "txtDetailsEmployee") {
                $("[id$=btnEmpChange]").click();
            }
        }

        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtDepartment" || targetControlID == "txtBranchLocation") {
                BindEmployee();
                ResetEmployee()
            }
            else if (targetControlID == "txtDetSearchDepartment" || targetControlID == "txtDetSearchBranchLocation") {
                BindEmployeeDetSearch();
                ResetEmployeeDetSearch()
            }
            else if (targetControlID == "txtDetailsEmployee") {
                $("[id$=btnEmpChange]").click();
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
        //        function AfterDateSelect(controlID) {
        //            if (controlID == "txtDate") {
        //                $("[id$=btnDateChanged]").click();
        //            }
        //        }
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

        function ShowHideDetSearch(flag) {
            if (flag == 1) {
                $("[id$=divDetSearch").show();
                $("[id$=imbShowDetSearch").hide();
                $("[id$=imbHideDetSearch").show();
            }
            else {
                $("[id$=divDetSearch").hide();
                $("[id$=imbShowDetSearch").show();
                $("[id$=imbHideDetSearch").hide();
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
                return false;  //Page is invalid -- stop right here
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
        function CheckValidationExists(id) {
            for (var i in validationArrayGroup) {
                if (validationArrayGroup[i] == id) {
                    return true;
                }
            }
            return false;
        }

        function EmployeeTypeChange() {
            ResetEmployee();
            BindEmployee();
        }

        function DetSearchEmployeeTypeChange() {
            ResetEmployeeDetSearch();
            BindEmployeeDetSearch();
        }

        function SaveConfirmationMsg(msg) {
            var msgTitle;
            //var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            // msg = '<%= GetLocalResourceObject("MsgSaveConfirm").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 250,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfIsSaveYes]").val(1);
                        $(this).dialog("close");
                        $("[id$=btnSave]").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfIsSaveYes]").val(0);
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
            <asp:HiddenField ID="hdfCurrentELH_PK" runat="server" />
            <asp:HiddenField ID="hdfCurrentROW_NO" runat="server" />
            <div class="fixed-buttons-normal">
                <%--style="padding-bottom: 30px !important;"--%>
                <div class="Button-container">
                    <%--style="padding: 0px !important;"--%>
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
                                            TabIndex="9" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteNew" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="9" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            CssClass="popupclose" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" OnClick="ActionHandler"
                                            TabIndex="9" />
                                    </li>
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
                                CommandArgument="SEC_ActionPanel" TabIndex="1" OnClick="ActionHandler" CommandName="CANCEL"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="1" OnClick="ActionHandler" CommandName="DETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblPage" CssClass="asptbllinks">
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
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblFilterFromDate" runat="server" Text="<%$ resources:FromDate%>"
                                                AssociatedControlID="txtFilterFromDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterFromDate" TabIndex="3" CssClass="input-small-c margnbotm0"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterFromDate" runat="server" />
                                            <asp:Label ID="lblFilterToDate" runat="server" Text="<%$ resources:ToDate%>" AssociatedControlID="txtFilterToDate"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterToDate" TabIndex="3" CssClass="input-small-c margnbotm0"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterToDate" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblSearchEmployee" runat="server" Text="<%$ resources:Employee%>"
                                                AssociatedControlID="txtSearchEmployee"></asp:Label>
                                            <asp:TextBox ID="txtSearchEmployee" runat="server" TabIndex="3" CssClass="input-half margnbotm0"
                                                MaxLength="100"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfSearchEmployee" runat="server" Value="" />
                                            <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Search%>" ToolTip="<%$ resources:Search%>"
                                                ValidationGroup="Search" OnClick="ActionHandler" TabIndex="4" CommandName="FILTER"
                                                SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Clear%>" ToolTip="<%$ resources:Clear%>"
                                                TabIndex="5" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
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
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" TabIndex="6"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfEOHPkListPage" Value='<%# Eval("EOH_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:FromDate%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblListFromDate" runat="server" Text='<%# Eval("EOH_DATE_FROM", Resources.Constants.HRMSDateFormatGrid)  %>'
                                                    ToolTip='<%# Eval("EOH_DATE_FROM", Resources.Constants.HRMSDateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ToDate%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblListToDate" runat="server" Text='<%# Eval("EOH_DATE_TO", Resources.Constants.HRMSDateFormatGrid) %>'
                                                    ToolTip='<%# Eval("EOH_DATE_TO", Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Description%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EOH_DESC")),110) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EOH_DESC")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="67%" />
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
                            <table class="table-devide tablelayout">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblFromDate" runat="server" Text="<%$ resources:FromDateStar%>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="input-small" TabIndex="1"
                                                MaxLength="11" ValidationGroup="Save" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="txtFromDate"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_FromDate%>"></asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" />
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$ resources:ToDateStar%>" AssociatedControlID="txtToDate"
                                                CssClass="lbl-19-6perc "></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="input-small" TabIndex="1" MaxLength="11"
                                                ValidationGroup="Save" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_ToDate%>"></asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfToDate" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="Label6" runat="server" CssClass="label" Text="<%$ resources:Description%>"
                                                AssociatedControlID="txtDescription"></asp:Label>
                                            <asp:TextBox ID="txtDescription" runat="server" CssClass="input-half" TabIndex="1"
                                                MaxLength="500"> </asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="search-colapse-b">
                                            <h1>
                                                <asp:Literal ID="Literal3" runat="server" Text="<%$ resources:EmployeeLeave%>" /></h1>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblDate" runat="server" Text="<%$ resources:DateStar%>" AssociatedControlID="txtDate"></asp:Label>
                                            <asp:TextBox ID="txtDate" runat="server" CssClass="input-small" TabIndex="2" MaxLength="11"
                                                onblur="ClearDateSelect()" ValidationGroup="AddToList" onkeydown="return CheckKey(event)"
                                                onpaste="return false;"> </asp:TextBox>
                                            <%--OnTextChanged="ActionHandler" AutoPostBack="true"--%>
                                            <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDate"
                                                Display="Static" CssClass="star" ValidationGroup="AddToList" Text="*" ErrorMessage="<%$ resources:Err_Date%>"></asp:RequiredFieldValidator>
                                            <asp:Button ID="btnDateChanged" Text="" runat="server" Style="display: none !important;"
                                                OnClick="ActionHandler" CommandName="CHANGE" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblBranchLocation" runat="server" Text="<%$ resources:BranchLocation%>"
                                                AssociatedControlID="txtBranchLocation"></asp:Label>
                                            <%-- <asp:DropDownList ID="ddlBranchLocation" runat="server" TabIndex="2" CssClass="select-half-a" >
                                            </asp:DropDownList>--%>
                                            <asp:TextBox runat="server" ID="txtBranchLocation" Text="" TabIndex="12" CssClass="input-half"></asp:TextBox>
                                            <asp:HiddenField ID="hdfBranchLocation" Value="" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblEmployeeType" AssociatedControlID="ddlEmployeeType"
                                                Text="<%$resources:EmployeeType%>"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlEmployeeType" CssClass="select-half-a" onchange="javascript:EmployeeTypeChange();"
                                                TabIndex="3">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblDepartment" runat="server" Text="<%$ resources:Department%>" AssociatedControlID="txtDepartment"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDepartment" Text="" TabIndex="3" CssClass="input-half"></asp:TextBox>
                                            <asp:HiddenField ID="hdfDepartment" Value="" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblEmployee" runat="server" Text="<%$ resources:Employee%>" AssociatedControlID="txtDetailsEmployee"></asp:Label>
                                            <asp:TextBox ID="txtDetailsEmployee" runat="server" TabIndex="4" CssClass="input-half margnbotm0"
                                                MaxLength="100"> </asp:TextBox><%--OnTextChanged="ActionHandler" AutoPostBack="true"--%>
                                            <asp:HiddenField ID="hdfDetailsEmployee" runat="server" Value="" />
                                            <asp:Button ID="btnEmpChange" runat="server" OnClick="ActionHandler" CommandName="CHANGEEMPLOYEE"
                                                Style="display: none" EnableTheming="false" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="Label2" runat="server" Text="<%$ resources:LeaveTypeStar %>" AssociatedControlID="ddlLeaveType"></asp:Label>
                                            <asp:DropDownList ID="ddlLeaveType" runat="server" TabIndex="4" CssClass="select-small-c"
                                                OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="reqLeaveType" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlLeaveType" Display="Static" Text="*" InitialValue="0"
                                                ValidationGroup="AddToList" ErrorMessage="<%$ resources:Err_SelectLeaveType %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label ID="Label3" runat="server" Text="<%$ resources:CreditStar%>" AssociatedControlID="txtELH_LEAVE_BAL"
                                                class="lbl-13perc">
                                            </asp:Label>
                                            <asp:TextBox ID="txtELH_LEAVE_BAL" runat="server" CssClass="input-small-a" MaxLength="12"
                                                TabIndex="6" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"
                                                onpaste="return false;" onkeyup="limitText(this,8);" onkeydown="limitText(this,8);"
                                                onDrop="return false;"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvELH_LEAVE_BAL" runat="server" ControlToValidate="txtELH_LEAVE_BAL"
                                                CssClass="star" ValidationGroup="AddToList" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_Credit%>"></asp:RequiredFieldValidator>
                                            <asp:RangeValidator ID="rngELH_LEAVE_BAL" runat="server" CssClass="star" SetFocusOnError="true"
                                                MinimumValue=".1" ValidationGroup="AddToList" EnableClientScript="true" Display="Dynamic"
                                                Text="*" MaximumValue="10000" ControlToValidate="txtELH_LEAVE_BAL" Type="Double"
                                                ErrorMessage="<%$ resources:Err_ZeroNoOfCredit %>">
                                            </asp:RangeValidator>
                                            <%--SkinID="plus"--%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="Label4" runat="server" CssClass="label" Text="<%$ resources:Remarks%>"
                                                AssociatedControlID="txtRemarks"></asp:Label>
                                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="input-full" TabIndex="7" MaxLength="500"> </asp:TextBox>
                                            <asp:ImageButton ID="btnPlus2" runat="server" OnClick="ActionHandler" CommandName="ADD"
                                                ValidationGroup="AddToList" OnClientClick="javascript:ValidateNow('AddToList')"
                                                TabIndex="7" SkinID="imbaddnew" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="clear">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="search-colapse-b">
                                <h1>
                                    <asp:Literal ID="ltrLeaveAddDed" runat="server" Text="<%$ resources:Search %>" /></h1>
                                <asp:ImageButton runat="server" ID="imbShowDetSearch" OnClientClick="javascript:return ShowHideDetSearch(1);"
                                    SkinID="imbArrowShow" ToolTip="<%$ resources:Controls,ShowDetails%>" />
                                <asp:ImageButton runat="server" ID="imbHideDetSearch" OnClientClick="javascript:return ShowHideDetSearch();"
                                    Style="display: none" SkinID="imbArrowHide" ToolTip="<%$ resources:Controls,HideDetails%>" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divDetSearch">
                                <table class="table-devide tablelayout">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblBrLocDetSearch" runat="server" Text="<%$ resources:BranchLocation%>"
                                                    AssociatedControlID="txtDetSearchBranchLocation"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDetSearchBranchLocation" Text="" TabIndex="12"
                                                    CssClass="input-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfDetSearchBranchLocation" Value="" runat="server" />
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblDepartmentDetSearch" runat="server" Text="<%$ resources:Department%>"
                                                    AssociatedControlID="txtDetSearchDepartment"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDetSearchDepartment" Text="" TabIndex="3" CssClass="input-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfDetSearchDepartment" Value="" runat="server" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblDetSearchEmployeeType" AssociatedControlID="ddlDetSearchEmployeeType"
                                                    Text="<%$resources:EmployeeType%>"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlDetSearchEmployeeType" CssClass="select-half-a"
                                                    onchange="javascript:DetSearchEmployeeTypeChange();" TabIndex="3">
                                                </asp:DropDownList>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblEmployeeDetSerach" runat="server" Text="<%$ resources:Employee%>"
                                                    AssociatedControlID="txtDetSerachEmployee"></asp:Label>
                                                <asp:TextBox ID="txtDetSerachEmployee" runat="server" TabIndex="4" CssClass="input-half margnbotm0"
                                                    MaxLength="100"> </asp:TextBox>
                                                <asp:HiddenField ID="hdfDetSerachEmployee" runat="server" Value="" />
                                                <asp:ImageButton ID="imbtnEmpSearch" runat="server" Text="<%$ resources:Search%>"
                                                    ToolTip="<%$ resources:Search%>" ValidationGroup="Search" OnClick="ActionHandler"
                                                    TabIndex="4" CommandName="EMPSEARCH" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                                <asp:ImageButton ID="imbtnEmpClear" runat="server" Text="<%$ resources:Clear%>" ToolTip="<%$ resources:Clear%>"
                                                    TabIndex="5" OnClick="ActionHandler" CommandName="EMPCLEAR" SkinID="clear-ext"
                                                    CssClass="margntop2 margnbotm0" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="gridwrap">
                                <%--class="grdTable"--%>
                                <asp:GridView runat="server" ID="grdLeaveList" Width="100%" AllowPaging="false" AllowSorting="True"
                                    AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-CssClass="emptytable"
                                    OnRowCommand="ActionHandler" TabIndex="8">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Employee%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ELH_EMPLOYEE_TEXT")),65) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELH_EMPLOYEE_TEXT")))%>'></asp:Label>
                                                <asp:HiddenField ID="hdfROW_NO" runat="server" Value='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ROW_NO")))%>' />
                                                <asp:HiddenField ID="hdfELH_EMPLOYEE" runat="server" Value='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELH_EMPLOYEE")))%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="40%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:LeaveType%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblELH_LEAVE_TYPE_CODE_TEXT" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ELH_LEAVE_TYPE_TEXT")),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELH_LEAVE_TYPE_TEXT")))%>'></asp:Label>
                                                <asp:HiddenField ID="hdfELH_LEAVE_TYPE" runat="server" Value='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELH_LEAVE_TYPE")))%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDateGridView" runat="server" Text='<%#Eval("ELH_DATE")!=""? Convert.ToDateTime(Eval("ELH_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat):""%>'
                                                    ToolTip='<%#Eval("ELH_DATE")!=""? Convert.ToDateTime(Eval("ELH_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat):""%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Remarks%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblELH_Remarks" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ELH_REMARKS")),40) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELH_REMARKS")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Credit%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblELH_LEAVE_BALGridview" runat="server" Text='<%# GetFormattedNumber(Eval("ELH_LEAVE_BAL")) %>'
                                                    ToolTip='<%# GetFormattedNumber(Eval("ELH_LEAVE_BAL")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_edit" runat="server" ID="imbEditDetails"
                                                    SkinID="imbeditgrid" CommandName="EDIT_ACTION" ToolTip="<%$resources:Controls,Edit %>"
                                                    TabIndex="8" />
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbDeleteDetails"
                                                    ToolTip="<%$resources:Controls,Delete %>" SkinID="imbdeletegrid" CommandName="DELETE_ACTION"
                                                    OnClientClick="return ShowDeleteConfirm(this);" TabIndex="8" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclEmpDetListPaging" runat="server" />
                            </div>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverrorAlert" style="display: none">
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
                <asp:ValidationSummary ID="vsAddToList" ValidationGroup="AddToList" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label></div>
            <asp:HiddenField ID="hdfIsSaveYes" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
