<%@ Page Title="<%$ Resources:Captions,Title_HRMS_EmpSalaryRevision %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="EmployeeSalaryRevision.aspx.cs"
    Inherits="HRMS.Employees.EmployeeSalaryRevision" Theme="ClassicExt" %>

<%@ Register Src="UserControls/GtiTabControl.ascx" TagName="GtiTabControl" TagPrefix="ucGtiTab" %>
<%@ Register Src="UserControls/EmpSalaryControl.ascx" TagName="EmpSalaryControl"
    TagPrefix="ucgti" %>
<%@ Register Src="UserControls/EmpBasicInfoControl.ascx" TagName="EmpBasicInfoControl"
    TagPrefix="ucBasicHdr" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <style type="text/css">
        .search-colapse-b
        {
            margin-bottom: 8px;
            height: 20px;
            background: #f5f5f5;
        }
    </style>
    <script type="text/javascript">
        function InitComponents() {
            $(document).ready(function () {

            });
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
            /// </param>         
            if (mode == 1) {
                $("[id$='pnlSave']").hide();
                $("[id$='pnlDelete']").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
            }
        }


        function ValidateNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                //CheckValidationDuplicate(valGroup);
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlTaskHome" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <ucGtiTab:GtiTabControl id="HrmsTab" runat="server" CurrentTab="10" />
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
                                    <%-- <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="1"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('Save')"
                                            ValidationGroup="Save" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="2" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('Save')" ValidationGroup="Save"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>--%>
                                    <%-- <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="Button1" CommandName="SAVE" TabIndex="3" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('Save')" ValidationGroup="Save"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteNew" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="4" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>--%>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                            TabIndex="5" SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="1" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="2" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <%--
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="3" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                     <li runat="server" id="Li1">
                                        <asp:Button runat="server" TabIndex="4" ID="btnListPrint" CommandName="PRINTLISTING"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
                                    </li>--%>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <%--Container for List and Detail tabs--%>
                <%--Page Datas--%>
                <%-- <div class="tab-container-floating">                    
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="5" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="6" OnClick="ActionHandler" CommandName="DETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>--%>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server" Style="display: none;">
                        <asp:TableCell></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <%--EntryPage Table Row--%>
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
                                    <asp:Literal ID="Literal2" runat="server" Text="<%$ resources: RevisionHistory %>" /></h1>
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdRevisionHistory" Width="100%" AllowPaging="false"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" OnRowCommand="ActionHandler" TabIndex="15">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("EIH_DATE", Resources.Constants.HRMSDateFormatGrid) %>'
                                                    ToolTip='<%# Eval("EIH_DATE", Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:EffectivePeriod%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEffectivePeriod" runat="server" Text='<%# Eval("EDP_EFFECT_FROM", Resources.Constants.HRMSDateFormatGrid) + " - " + ( string.IsNullOrEmpty(Convert.ToString(Eval("EDP_EFFECT_TO")))?GetLocalResourceObject("TillDate").ToString():Eval("EDP_EFFECT_TO", Resources.Constants.HRMSDateFormatGrid)) %>'
                                                    ToolTip='<%# Eval("EDP_EFFECT_FROM", Resources.Constants.HRMSDateFormatGrid) + " - " + ( string.IsNullOrEmpty(Convert.ToString(Eval("EDP_EFFECT_TO")))?GetLocalResourceObject("TillDate").ToString():Eval("EDP_EFFECT_TO", Resources.Constants.HRMSDateFormatGrid)) %>'>
                                                </asp:Label>
                                                <asp:HiddenField ID="hdfEffectTo" runat="server" Value='<%# Eval("EDP_EFFECT_TO") %>' />
                                                <asp:HiddenField ID="hdfEDP_EFFECT_FROM" runat="server" Value='<%# Eval("EDP_EFFECT_FROM") %>' />
                                                <%-- <asp:HiddenField ID="hdfSlNo" runat="server" Value='<%# Eval("SlNo") %>' />
                                                <asp:HiddenField ID="hdfPk" runat="server" Value='<%# Eval("Pk") %>' />--%>
                                                <%--<asp:HiddenField ID="hdfPayElementPk" runat="server" Value='<%# Eval("PayElementPk") %>' />--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:CTC%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCTC" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EMP_CTC")),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EMP_CTC")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>--%>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:PreviousCTC%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPreviousCTC" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EMP_LAST_CTC")),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EMP_LAST_CTC")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Type%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EIH_TYPE_TEXT")),25) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EIH_TYPE_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Remarks%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTrxName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EIH_TRN_NAME")),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EIH_TRN_NAME")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Department%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDepartment" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EMP_DEP_TEXT")),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EMP_DEP_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Designation%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDesignation" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EMP_DESIG_TEXT")),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EMP_DESIG_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <%-- <asp:Panel ID="pnlAction" runat="server" Visible="false">--%>
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_edit" runat="server" ID="imbViewDetails"
                                                    SkinID="btnview" EnableViewState="false" CommandName="EDIT_ACTION" ToolTip="<%$resources:Controls,ViewDetails %>" />
                                                <%--<asp:ImageButton Width="16px" Height="16px" CssClass="_edit" runat="server" ID="imbEditDetails"
                                                        SkinID="imbeditgrid" EnableViewState="false" CommandName="EDIT_ACTION" />
                                                    <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbDeleteDetails"
                                                        SkinID="imbdeletegrid" EnableViewState="false" CommandName="DELETE_ACTION" OnClientClick="return ShowDeleteConfirm(this);" />--%>
                                                <%--</asp:Panel>--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <ucgti:EmpSalaryControl id="UCEmpSalary" runat="server" />
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S" style="float: right;">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverrorAlert" style="display: none">
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
                <asp:ValidationSummary ID="vsAddToList" ValidationGroup="AddToList" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
