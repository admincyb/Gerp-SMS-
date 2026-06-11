<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeTraining.aspx.cs"
    Inherits="HRMS.Employees.EmployeeTraining" MasterPageFile="~/ERPSMS_2.Master"
    Theme="ClassicExt" Title="<%$ Resources:Captions,Title_EmployeeTraining %>" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtDateHd");
            GrandScriptUtils.AddDateRangeCommon("txtFromDateList", "hdfFromDateList", "txtToDateList", "hdfToDateList", false, false);
            GrandScriptUtils.AddDateRangeCommon("txtFromDateHd", "hdfFromDateHd", "txtToDateHd", "hdfToDateHd", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtBranchLocation_Filter", url, "hdfBranchLocation_Filter", true, true, "BRANCHLOCATION");
            GrandScriptUtils.MakeAutoCompleteDDL("txtDepartment_Filter", url, "hdfDepartment_Filter", true, true, "DEPARTMENTAUTOCOMPLETE");
            InitEmployeeAuto();
        }
        function InitEmployeeAuto() {
            BindEmployee();
            ResetEmployee();
        }
        function BindEmployee() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee_Filter", url + "?EmpCategory=2" + "&EmpDept=" + $("[id$=hdfDepartment_Filter]").val() + "&EmpBranch=" + $("[id$=hdfBranchLocation_Filter]").val() + "&EmpType=" + $("[id$=ddlEmployeeType_Filter]").val() + "&EmploymentType=" + $("[id$=ddlEmploymentType_Filter]").val() + "&EmpCompany=" + $("[id$=ddlCompany_Filter]").val() + "&ToDate=" + $("[id$=txtToDateHd]").val() + "&EmpDesignation=", "hdfEmployee_Filter", true, true, "EMPLOYEEAUTOCOMPLETE", "", true, true, false, 1, '<%= Resources.ErpRes.All_Small %>');
        }
        function ResetEmployee() {
            var defText = '<%= Resources.ErpRes.All_Small %>';
            $("[id$=txtEmployee_Filter]").val(defText);
            $("[id$=hdfEmployee_Filter]").val('-1');
        }

        //To excecute after  auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtDepartment_Filter" || targetControlID == "txtBranchLocation_Filter") {
                BindEmployee();
                ResetEmployee();
            }
        }

        //To excecute after  auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtBranchLocation_Filter" || targetControlID == "txtDepartment_Filter") {
                BindEmployee();
                ResetEmployee();
            }
        }

        function AfterDateSelect(controlID) {
            if (controlID == "txtToDateHd") {
                InitEmployeeAuto
            }
        }

        function ClearDateSelect() {
            if ($("[id$=txtToDateHd]").val() == '') {
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
                ShowErrorMessage($("#diverrorAlert").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
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


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlBasicInfo">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum" Text="<%$ resources:Breadcrumb%>"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$resources:Controls,Save %>"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save"
                                            ValidationGroup="Save" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('Save')"
                                            TabIndex="8" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteNew" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="8" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li runat="server" id="pnlPrint">
                                        <asp:Button runat="server" ID="btnPrint" CommandName="PRINT" Text="<%$resources:Controls,Print %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" TabIndex="8" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            CssClass="popupclose" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" OnClick="ActionHandler"
                                            TabIndex="8" />
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
                                    <li runat="server" id="pnPrint">
                                        <asp:Button runat="server" ID="bttnPrint" CommandName="PRINT" Text="<%$resources:Controls,Print %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" TabIndex="1" />
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
                                            <asp:Label ID="lblFromDate" runat="server" Text="<%$ resources:FromDate%>" AssociatedControlID="txtFromDateList"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFromDateList" TabIndex="3" onkeydown="return CheckKey(event)"
                                                onpaste="return false;" CssClass="input-small margnbotm0"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDateList" runat="server" Value="" />
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$ resources:ToDate%>" AssociatedControlID="txtToDateList"
                                                CssClass="middle-lbl-small"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtToDateList" TabIndex="3" CssClass="input-small margnbotm0"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfToDateList" runat="server" Value="" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label runat="server" ID="lblTopicList" AssociatedControlID="txtTopicList" Text="<%$resources:Topic %>"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTopicList" CssClass="input-half margnbotm0" TabIndex="3">
                                            </asp:TextBox>
                                            <asp:ImageButton ID="imgFilterList" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="3"
                                                CommandName="FILTER" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="imgClearList" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="3" OnClick="ActionHandler"
                                                CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
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
                                                    TabIndex="4" />
                                                <asp:HiddenField runat="server" ID="hdfPSH_PKListPage" Value='<%# Eval("ETA_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                            <HeaderStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TrxNo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("ETA_NO")))?Resources.ErpRes.Draft:Eval("ETA_NO")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("ETA_NO")))?Resources.ErpRes.Draft:Eval("ETA_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate_GrdList" runat="server" Text='<%# Convert.ToDateTime(Eval("ETA_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat)%>'
                                                    ToolTip='<%# Convert.ToDateTime(Eval("ETA_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                            <HeaderStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Topic%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTopic_GrdList" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ETA_TOPIC")),55) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ETA_TOPIC")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                            <HeaderStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:FromDate%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFromDate_GrdList" runat="server" Text='<%#  string.IsNullOrEmpty(Convert.ToString(Eval("ETA_FROM_DATE"))) ? "" : Convert.ToDateTime(Eval("ETA_FROM_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat)%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("ETA_FROM_DATE"))) ? "" : Convert.ToDateTime(Eval("ETA_FROM_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                            <HeaderStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ToDate%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblToDate_GrdList" runat="server" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("ETA_TO_DATE"))) ? "" : Convert.ToDateTime(Eval("ETA_TO_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat)%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("ETA_TO_DATE"))) ? "" : Convert.ToDateTime(Eval("ETA_TO_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                            <HeaderStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Place%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPlace_GrdList" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ETA_PLACE"))),20) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ETA_PLACE")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                            <HeaderStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Trainer%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTrainer_GrdList" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ETA_TRAINER")),35) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ETA_TRAINER")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="20%" />
                                            <ItemStyle Width="20%" />
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
                                            <asp:Label ID="lblDateHd" runat="server" Text="<%$ resources:DateStar%>" AssociatedControlID="txtDateHd"
                                                CssClass="middle-lbl-small-e"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDateHd" TabIndex="1" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDateHd"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_EnterDate%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCompany" runat="server" Text="<%$ resources:Controls,CompanyReq%>"
                                                AssociatedControlID="ddlCompany"></asp:Label>
                                            <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="1" CssClass="select-small-b">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvCompany" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlCompany" Display="Static" Text="*" InitialValue="-1"
                                                ValidationGroup="Save" ErrorMessage="<%$ resources:Err_SelectCompany %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblTopic" AssociatedControlID="txtTopic" Text="<%$resources:TopicReq %>"
                                                CssClass="middle-lbl-xsmall-c2"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTopic" CssClass="input-w27per margn-rgt4" TabIndex="2"
                                                MaxLength="500">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvTopic" runat="server" ControlToValidate="txtTopic"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_EnterTopic%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblFromDateHd" runat="server" Text="<%$ resources:FromDate%>" AssociatedControlID="txtFromDateHd"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFromDateHd" TabIndex="3" CssClass="input-small"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDateHd" runat="server" Value="" />
                                            <asp:Label ID="lblToDateHd" runat="server" Text="<%$ resources:ToDate%>" AssociatedControlID="txtToDateHd"  onblur="ClearDateSelect()"
                                                CssClass="middle-lbl-small-e"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtToDateHd" TabIndex="4" CssClass="input-small"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfToDateHd" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblDurationHd" runat="server" Text="<%$ resources:Duration%>" AssociatedControlID="txtDurationHd"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDurationHd" TabIndex="5" CssClass="small" ValidationGroup="Save" />
                                            <asp:Label ID="Label1" runat="server" Text="Hrs" AssociatedControlID="txtDurationHd"
                                                CssClass="lbl-3-7perc"></asp:Label>
                                            <cc1:MaskedEditExtender ID="meeDurationHd" runat="server" AutoComplete="false" Mask="<%$ resources:HrmsDurationMask %>"
                                                MaskType="Time" TargetControlID="txtDurationHd">
                                            </cc1:MaskedEditExtender>
                                            <asp:RegularExpressionValidator runat="server" ID="regDurationHd" CssClass="star"
                                                SetFocusOnError="true" ValidationGroup="Save" ControlToValidate="txtDurationHd"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ValidDuration %>"
                                                ValidationExpression="<%$ resources:HrmsDurationMaskValidationExp %>">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblPlaceHd" runat="server" Text="<%$ resources:Place%>" AssociatedControlID="txtPlaceHd"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPlaceHd" TabIndex="5" CssClass="input-half" MaxLength="200"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblTrainerHd" runat="server" Text="<%$ resources:Trainer%>" AssociatedControlID="txtTrainerHd"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTrainerHd" TabIndex="5" MaxLength="200" CssClass="input-half"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblDescHd" Text="<%$ resources:Description%>" AssociatedControlID="txtDescHd"></asp:Label>
                                            <asp:TextBox ID="txtDescHd" runat="server" TabIndex="7" MaxLength="490" TextMode="MultiLine"
                                                Height="40" CssClass="input-full" onkeyup="limitText(this,500);"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="inner-tabgroup" style="margin-top: 15px;">
                                <div class="col-md-2 col-sm-2 col-xs-2" style="height: 32px; float: right;">
                                    <asp:Button runat="server" SkinID="btnInner-add" ID="btnShowPopUp" TabIndex="7" OnClick="ActionHandler"
                                        CommandName="SHOWPOPUP" Text="<%$resources:AddEmployee %>" ToolTip="<%$resources:AddEmployee %>"
                                        ValidationGroup="save" CssClass="margntop-1 margn-rgt0" Style="margin-top: -2.5px;" />
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                            <div id="Div1" runat="server" class="gridwrap">
                                <asp:GridView runat="server" ID="grdEmpTrainingList" Width="100%" AllowPaging="false"
                                    OnRowCommand="ActionHandler" AllowSorting="True" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:SINo%>">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <HeaderStyle Width="4%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Employee%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmptextGd" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empName_txt"))),50) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empName_txt"))) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfROW_NO" Value='<%# Eval("ROW_NO") %>' />
                                                <asp:HiddenField runat="server" ID="hdfETL_PK" Value='<%# Eval("ETL_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfETL_EMP_PK" Value='<%# Eval("ETL_EMP_PK") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Designation%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDesigGd" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empDesignationText"))),30) %>'
                                                    ToolTip='<%#  System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empDesignationText")))%>'></asp:Label></ItemTemplate>
                                            <HeaderStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BranchLocation%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBranchLocationGd" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empBranchText"))),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empBranchText"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Department%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDepartmentGd" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empDepartmentText"))),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empDepartmentText"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Actions%> ">
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imbDeleteEmployee" SkinID="imbdeletegrid" CommandName="DELETE_ACTION"
                                                    ToolTip="<%$ resources:Delete%>" OnClick="ActionHandler" OnClientClick="return ShowDeleteConfirm(this);"
                                                    TabIndex="7" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="3%" CssClass="amount-numeric" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
            </div>
            <div id="divEmployeeDetails_PopUp" style="display: none">
                <div class="content-wrapper">
                    <div class="Button-container-popup">
                        <asp:Button ID="btnAddMenu" SkinID="btnInner-add-dsd" runat="server" Text="<%$resources:Controls,Add_Add %>"
                            CommandName="SAVE_ACTIONPOPUP" OnClick="ActionHandler" TabIndex="16" ToolTip="<%$resources:Controls,Add_Add %>" />
                        <asp:Button ID="btnCancelPopUp" runat="server" CommandName="CANCELPOPUP" OnClick="ActionHandler"
                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Close %>" TabIndex="16"
                            Text="<%$Resources:Controls,Close%>" />
                    </div>
                    <div id="divEmpFilterDetails">
                        <table class="table-devide tablelayout">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label runat="server" ID="lblCompany_Filter" Text="<%$ resources:Company%>" AssociatedControlID="ddlCompany_Filter"></asp:Label>
                                        <asp:DropDownList ID="ddlCompany_Filter" runat="server" TabIndex="10" CssClass="select-half-a"
                                            onchange="javascript:InitEmployeeAuto();">
                                        </asp:DropDownList>
                                        <div class="clear">
                                        </div>
                                        <asp:Label ID="lblEmploymentType_Filter" runat="server" Text="<%$ resources:EmploymentType%>"
                                            AssociatedControlID="ddlEmploymentType_Filter"></asp:Label>
                                        <asp:DropDownList ID="ddlEmploymentType_Filter" runat="server" TabIndex="13" CssClass="select-half-a"
                                            onchange="javascript:InitEmployeeAuto();">
                                        </asp:DropDownList>
                                        <div class="clear">
                                        </div>
                                        <asp:Label runat="server" ID="lblEmployeeTypeFilter" AssociatedControlID="ddlEmployeeType_Filter"
                                            Text="<%$ resources:EmployeeType%>"></asp:Label>
                                        <asp:DropDownList runat="server" ID="ddlEmployeeType_Filter" CssClass="select-half-a"
                                            TabIndex="14" onchange="javascript:InitEmployeeAuto();">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblBranchLocation_Filter" runat="server" Text="<%$ resources:BranchLocation%>"
                                            AssociatedControlID="txtBranchLocation_Filter"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtBranchLocation_Filter" Text="" TabIndex="12" CssClass="input-half"></asp:TextBox><asp:HiddenField
                                            ID="hdfBranchLocation_Filter" Value="-1" runat="server" />
                                        <div class="clear">
                                        </div>
                                        <asp:Label ID="lblDepartment_Filter" runat="server" Text="<%$ resources:Department%>"
                                            AssociatedControlID="txtDepartment_Filter"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtDepartment_Filter" Text="" TabIndex="13" CssClass="input-half"></asp:TextBox><asp:HiddenField
                                            ID="hdfDepartment_Filter" Value="-1" runat="server" />
                                        <div class="clear">
                                        </div>
                                        <asp:Label ID="lblEmployee_Filter" runat="server" Text="<%$ resources:Employee%>"
                                            AssociatedControlID="txtEmployee_Filter"></asp:Label>
                                        <asp:TextBox ID="txtEmployee_Filter" runat="server" TabIndex="14" CssClass="input-half"
                                            MaxLength="100"> </asp:TextBox><asp:HiddenField ID="hdfEmployee_Filter" runat="server"
                                                Value="-1" />
                                        <asp:ImageButton ID="btnFilterSearch" runat="server" Text="<%$ resources:Search%>"
                                            ToolTip="<%$ resources:Controls,Search%>" OnClick="ActionHandler" TabIndex="15"
                                            CommandName="SEARCH" SkinID="search-ext" CssClass="margntop2 margnbotm0 margn-rgt4"
                                            ValidationGroup="Search" OnClientClick="javascript:ValidatePageNow('Search')" />
                                        <asp:ImageButton ID="btnFilterClear" runat="server" Text="<%$ resources:Controls,Clear%>"
                                            ToolTip="<%$ resources:Controls,Clear%>" TabIndex="15" OnClick="ActionHandler"
                                            CommandName="CLEARDETAIL" SkinID="clear-ext" CssClass="margntop2 margnbotm0 margn-rgt4" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="Div2" runat="server" class="gridwrap maxh-290">
                        <asp:GridView runat="server" ID="grdEmpTraining_PopUp" Width="100%" AllowPaging="false"
                            AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                            EmptyDataRowStyle-CssClass="emptytable">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty_PopUp" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label></EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkEmpHeader_PopUp" runat="server" ToolTip="<%$ resources:Err_Employee%>"
                                            TabIndex="15" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkEmpselect_PopUp" TabIndex="16" />
                                        <asp:HiddenField runat="server" ID="hdfEmpPk_PopUp" Value='<%# Eval("ETL_EMP_PK") %>' />
                                        <asp:HiddenField runat="server" ID="hdfETL_PK_PopUp" Value='<%# Eval("ETL_PK") %>' />
                                        <asp:HiddenField runat="server" ID="hdfROW_NO_PopUp" Value='<%# Eval("ROW_NO") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle Width="2%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Employee%> " SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblempName_txt_PopUp" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empName_txt"))),30) %>'
                                            ToolTip='<%#  System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empName_txt")))%>'></asp:Label></ItemTemplate>
                                    <HeaderStyle Width="30%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Designation%> " SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDesigPopUp" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empDesignationText"))),22) %>'
                                            ToolTip='<%#  System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empDesignationText")))%>'></asp:Label></ItemTemplate>
                                    <HeaderStyle Width="22%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:BranchLocation%> " SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblempBranchText_PopUp" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empBranchText"))),20) %>'
                                            ToolTip='<%#  System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empBranchText")))%>'></asp:Label></ItemTemplate>
                                    <HeaderStyle Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Department%> " SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDepartment_PopUp" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empDepartmentText"))),25) %>'
                                            ToolTip='<%#  System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empDepartmentText")))%>'></asp:Label></ItemTemplate>
                                    <HeaderStyle Width="25%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div id="diverrorAlert" style="display: none">
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
                <asp:ValidationSummary ID="vsAddToList" ValidationGroup="AddToList" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label></div>
            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormatWithComma" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
