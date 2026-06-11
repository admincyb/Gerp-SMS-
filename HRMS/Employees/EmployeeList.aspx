<%@ Page Title="<%$ resources:HRMS-EmployeeList%>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="EmployeeList.aspx.cs" Inherits="HRMS.Employees.EmployeeList" %>

<%@ Register Src="UserControls/GtiTabControl.ascx" TagName="GtiTabControl" TagPrefix="ucGtiTab" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            ShowHideAdvancedSearch(0);

        });

        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitPage() {

            GrandScriptUtils.MakeAutoCompleteDDL("txtSearchNationality", url + "?OpParam=CNT_NATIONALITY", "hdfAutoNationality", true, true, "NATIONALITY");
            GrandScriptUtils.MakeAutoCompleteDDL("txtDepartment", url, "hdfDepartment", true, true, "DEPARTMENTAUTOCOMPLETE");
            GrandScriptUtils.MakeAutoCompleteDDL("txtBrLoc", url, "hdfBrLoc", true, true, "BRANCHLOCATION", false, true);
            GrandScriptUtils.MakeAutoCompleteDDL("txtTeam", url, "hdfTeam", true, true, "TEAM");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCostCenter", url, "hdfCostCenter", true, true, "COSTCENTERWITHOUTGRP");    
        }

        function ShowHideAdvancedSearch(flag) {
            //If flag then Show AdvancedSearch
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ucGtiTab:GtiTabControl ID="hrmsTab" runat="server" CurrentTab="1" />
    <div class="fixed-buttons-normal">
        <div class="Button-container">
            <asp:Table ID="Table1" runat="server">
                <asp:TableRow>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul class="bredcrum">
                            <asp:Label runat="server" Text="<%$ resources:Breadcrumb%>" ID="lblBreadCrum"></asp:Label>
                        </ul>
                        <ul runat="server" id="pnlListing">
                            <li>
                                <asp:Button ID="btnNew" runat="server" SkinID="btnInner-New" Text="<%$Resources:Controls,New%>"
                                    OnClientClick="javascript:return ValidatePageNow('vgCompany')" CommandName="NEW"
                                    TabIndex="16" OnClick="ActionHandler" ToolTip="New" />
                            </li>
                            <li>
                                <asp:Button ID="btnEdit" runat="server" SkinID="btnInner-Edit" Text="<%$Resources:Controls,Edit%>"
                                    CommandName="EDIT" OnClick="ActionHandler" TabIndex="17" ToolTip="Edit" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnView" CommandName="VIEW" Text="<%$resources:Controls,View %>"
                                    OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" TabIndex="18" SkinID="btnInner-View"
                                    ToolTip="<%$resources:Controls,View %>" />
                            </li>
                            <%-- <li>
                                <asp:Button ID="btnDelete" runat="server" Visible="true" SkinID="btnInner-Delete"
                                    Text="<%$Resources:Controls,Delete%>" OnClientClick="return ShowDeleteConfirm(this);"
                                    CommandName="DELETE" OnClick="ActionHandler" TabIndex="15" ToolTip="Delete" />
                            </li>--%>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <asp:Table runat="server" ID="tblPage" CssClass="tablelayout asptbllinks">
            <asp:TableRow ID="PageAction_List" runat="server">
                <asp:TableCell>
                    <div class="search-colapse" style="margin-top: 30px;">
                        <table>
                            <tr>
                                <td>
                                    <h1>
                                        <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                                </td>
                                <td>
                                    <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                        ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:ShowFilter%>" TabIndex="1"/>
                                    <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                        ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>"  TabIndex="1"/>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table class="table-devide tablelayout" id="tbladvancedSearch" style="background: #f2f2f2;">
                        <tr id="Tr1" runat="server">
                            <td>
                                <div class="div2col-S padgtop7">
                                    <asp:Label runat="server" ID="lblSearchNationality" Text="<%$ resources:Nationality%>"
                                        AssociatedControlID="txtSearchNationality"></asp:Label>
                                    <asp:TextBox ID="txtSearchNationality" TabIndex="1" runat="server" CssClass="select-small-c"></asp:TextBox>
                                    <asp:HiddenField ID="hdfAutoNationality" runat="server" />
                                    <asp:Label runat="server" ID="lblsearchDesignation" Text="<%$ resources:Designation%>"
                                        AssociatedControlID="ddlDesignation" CssClass="middle-lbl"></asp:Label>
                                    <asp:DropDownList ID="ddlDesignation" TabIndex="2" runat="server" CssClass="select-small-c1">
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                    <asp:Label runat="server" ID="lblDepartment" Text="<%$ resources:Department%>" AssociatedControlID="txtDepartment"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtDepartment" Text="" TabIndex="5" MaxLength="100"
                                        CssClass="input-small-c"></asp:TextBox>
                                    <asp:HiddenField ID="hdfDepartment" Value="" runat="server" />
                                    <asp:Label runat="server" ID="lblSearchCompany" Text="<%$ resources:Company%>" AssociatedControlID="ddlSearchCompany"
                                        CssClass="middle-lbl"></asp:Label>
                                    <asp:DropDownList ID="ddlSearchCompany" TabIndex="6" runat="server" CssClass="select-small-c1">
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                    <div>                                    
                                    <asp:Label ID="lblBrLoc" CssClass="lbl-24-3perc" runat="server" Text="<%$ resources:Bra/Loc%>" AssociatedControlID="txtBrLoc"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtBrLoc" Text="" TabIndex="8" CssClass="select-w70-3per"></asp:TextBox>
                                    <asp:HiddenField ID="hdfBrLoc" Value="" runat="server" />
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S padgtop7">
                                    <div id="divHideDtls" runat="server">
                                    <asp:Label ID="lblSearchPermitNo" runat="server" Text="<%$ resources:PermitNo%>"
                                     AssociatedControlID="txtSearchPermitNo"></asp:Label>
                                    <asp:TextBox ID="txtSearchPermitNo" TabIndex="3" runat="server" MaxLength="100" CssClass="input-small"></asp:TextBox>
                                    <asp:Label ID="lblSearchPassport" runat="server" Text="<%$ resources:PassportNo%>"
                                        AssociatedControlID="txtSearchPassport" CssClass="middle-lbl"></asp:Label>
                                    <asp:TextBox ID="txtSearchPassport" TabIndex="4" runat="server" MaxLength="100" CssClass="input-small"></asp:TextBox>                                   
                                   
                                    <div class="clear">
                                    </div>
                                    <asp:Label runat="server" ID="lblEmployementType" AssociatedControlID="ddlEmployementType"
                                        Text="<%$resources:EmployeeType %>"></asp:Label>
                                    <asp:DropDownList runat="server" ID="ddlEmployementType" CssClass="select-small-a1"
                                        TabIndex="7">
                                    </asp:DropDownList>
                                    </div>  
                                     <div id="divCostTeam" runat="server">
                                     <asp:Label ID="lblTeam"  runat="server" Text="<%$ resources:Team%>" AssociatedControlID="txtTeam"></asp:Label>
                                     <asp:TextBox runat="server" ID="txtTeam" Text="" TabIndex="13"  CssClass="select-half"></asp:TextBox>
                                     <asp:HiddenField ID="hdfTeam" runat="server" />
                                     <asp:Label ID="lblCostCenter"  runat="server" Text="<%$ resources:CostCenter%>"
                                      AssociatedControlID="txtCostCenter"></asp:Label>
                                      <asp:TextBox runat="server" ID="txtCostCenter"   Text="" TabIndex="17" CssClass="select-half"></asp:TextBox>
                                      <asp:HiddenField ID="hdfCostCenter" runat="server" />
                                    </div>
                                   
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <table class="table-devide tablelayout">
                        <tr>
                            <td>
                                <div class="div2col-S div-separatn">
                                    <asp:Label ID="lblSearchName" runat="server" Text="<%$ resources:Name%>" AssociatedControlID="txtName"></asp:Label>
                                    <asp:TextBox ID="txtName" TabIndex="9" runat="server" MaxLength="100" CssClass="input-small-c margnbotm0"></asp:TextBox>
                                    <asp:Label ID="lblSearchCode" runat="server" Text="<%$ resources:Code%>" AssociatedControlID="txtSearchCode"
                                        CssClass="middle-lbl"></asp:Label>
                                    <asp:TextBox ID="txtSearchCode" runat="server" TabIndex="10" CssClass="input-small-c margnbotm0"
                                        MaxLength="100"> </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S div-separatn">
                                    <asp:Label runat="server" ID="lblIncludeInactive" Text="<%$ resources:Inactive%>"
                                        AssociatedControlID="lblIncludeInactive"></asp:Label>
                                    <asp:CheckBox ID="chkIncludeInActive" TabIndex="11" runat="server" />
                                    <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" AssociatedControlID="ddlStatus"
                                        CssClass="middle-lbl-small-b"></asp:Label>
                                    <asp:DropDownList ID="ddlStatus" TabIndex="12" runat="server" CssClass="select-medium margnbotm0">
                                    </asp:DropDownList>
                                    <%--<asp:Label ID="lblSearch" runat="server" AssociatedControlID="btnSearch"></asp:Label>--%>
                                    <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Search%>" ToolTip="<%$ resources:Search%>"
                                        ValidationGroup="Search" OnClick="ActionHandler" TabIndex="13" CommandName="FILTER"
                                        SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                    <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Clear%>" ToolTip="<%$ resources:Clear%>"
                                        TabIndex="14" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext"
                                        CssClass="margntop2 margnbotm0" />
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="gridwrap">
                        <asp:GridView ID="grdEmployeelist" runat="server" AutoGenerateColumns="False" EmptyDataRowStyle-CssClass="emptytable"
                            AllowSorting="True" OnSorting="ActionHandler" Width="100%" PageSize="<%$ resources:PageSize%>">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                            AutoPostBack="true" OnCheckedChanged="ActionHandler" TabIndex="15"/>
                                        <asp:HiddenField runat="server" ID="hdfEmpPk" Value='<%# Eval("empPK") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="2%" />
                                    <%--  <ItemStyle HorizontalAlign="Center" Width="10%" />--%>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:HCode%> " SortExpression="empCode">
                                    <ItemTemplate>
                                        <asp:Label ID="lblempCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("empCode")),15) %>'
                                            ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empCode")))%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:HName%>  " SortExpression="empNameText">
                                    <ItemTemplate>
                                        <asp:Label ID="lblempName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("empNameText")),45) %>'
                                            ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetDecodedString(Eval("empNameText")),500)%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="16%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Nationality%>  " SortExpression="empNationality1_TEXT">
                                    <ItemTemplate>
                                        <asp:Label ID="lblempNationality" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("empNationality1_TEXT")),15) %>'
                                            ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetDecodedString(Eval("empNationality1_TEXT")),500)%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:HDesignation%>  " SortExpression="empDesignationText">
                                    <ItemTemplate>
                                        <asp:Label ID="lblempDes" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("empDesignationText")),17) %>'
                                            ToolTip='<%# Eval("empDesignationText")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:HDepartment%> " SortExpression="empDepartmentText">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldep" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("empDepartmentText")),17) %>'
                                            ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empDepartmentText")))%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="12%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Branch/Location%> " SortExpression="EmpBranchText">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranch" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EmpBranchText")),15) %>'
                                            ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EmpBranchText")))%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="9%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:HGender%> " SortExpression="empGenderText">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGen" runat="server" Text='<%# Eval("empGenderText") %>' ToolTip='<%# Eval("empGenderText")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="<%$ resources:CostCenter%> " SortExpression="EmpCostcenterText" Visible="<%$ resources:ConfigurationsRes,HrmsEmpListingCostCenterHide%>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCostlist" runat="server" Text='<%# Eval("EmpCostcenterText") %>' ToolTip='<%# Eval("EmpCostcenterText")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="<%$ resources:Team%> " SortExpression="EmpTeamText" Visible="<%$ resources:ConfigurationsRes,HrmsEmpListingTeamHide%>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTeamlist" runat="server" Text='<%# Eval("EmpTeamText") %>' ToolTip='<%# Eval("EmpTeamText")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:HDOJ%> " SortExpression="empDOJText">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDOJ" runat="server" Text='<%# Eval("empDOJText")!=""? Convert.ToDateTime(Eval("empDOJText")).ToString(Resources.Constants.HRMSDateDisplayFormat):""  %>'
                                            ToolTip='<%# Eval("empDOJText")!=""? Convert.ToDateTime(Eval("empDOJText")).ToString(Resources.Constants.HRMSDateDisplayFormat):""%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:EmployeeType%> " SortExpression="EPD_EMP_TYPE_TEX" Visible="<%$ resources:ConfigurationsRes,HrmsEmpListingTypeHide%>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmployeeType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EPD_EMP_TYPE_TEX")),14) %>'
                                            ToolTip='<%# Eval("EPD_EMP_TYPE_TEX")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:HStatus%> " SortExpression="empCurStatusText">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmpstatus" runat="server" Text='<%# Eval("empCurStatusText") %>'
                                            ToolTip='<%# Eval("empCurStatusText")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <uc1:PagerControl ID="uclPaging" runat="server" />
                        <%--<uc1:PagerControl ID="uclPaging" runat="server" />--%>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    <div id="diverror" style="display: none">
        <%--Use this label to bind the server errors--%>
        <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
        <asp:ValidationSummary ID="vsPage" ValidationGroup="DateCheck" runat="server" />
        <asp:HiddenField ID="hdfAppType" runat="server" />
        <asp:HiddenField ID="hdfAppSubType" runat="server" />
    </div>
    </div>
</asp:Content>
