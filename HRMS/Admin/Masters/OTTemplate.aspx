<%@ Page Title="<%$ Resources:Captions,Title_OTTemplate %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="OTTemplate.aspx.cs" Inherits="HRMS.Admin.Masters.OTTemplate"
    Theme="ClassicExt" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Src="UserControls/FormulaMaster.ascx" TagName="PopUp" TagPrefix="uc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript">
        function InitComponents() {
            $(document).ready(function () {
//                ShowHideEarnings(1);
//                ShowHideDeductions(1);
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
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="Button1" CommandName="SAVE" TabIndex="10" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('Save')" ValidationGroup="Save"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteNew" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="10" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="10" SkinID="btnInner-Cancel"
                                            ToolTip="<%$resources:Controls,Cancel %>" />
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
                                CommandArgument="SEC_ActionPanel" TabIndex="4" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="5" OnClick="ActionHandler" CommandName="DETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server" Style="display: none;">
                        <%--Listing Page Table Row--%>
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
                                                TabIndex="6" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>"
                                                TabIndex="6" />
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
                                            <asp:Label ID="lblFilterCode" runat="server" Text="<%$ resources:FilterCode%>" AssociatedControlID="txtFilterCode"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterCode" TabIndex="7" CssClass="input-half margnbotm0"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="label6" runat="server" Text="<%$ resources:FilterName%>" AssociatedControlID="txtTemplateNameListPage"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTemplateNameListPage" TabIndex="7" CssClass="input-half margnbotm0"></asp:TextBox>
                                            <asp:ImageButton ID="btnSearch1" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="8"
                                                CommandName="FILTER" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClear1" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="9" OnClick="ActionHandler"
                                                CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
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
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    TabIndex="10" />
                                                <asp:HiddenField runat="server" ID="hdfTemplatePkListPage" Value='<%# Eval("OTE_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfListOTE_MOD_DT" Value='<%# Eval("OTE_MOD_DT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                            <HeaderStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TemplateCode%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("OTE_CODE")),15) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("OTE_CODE")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                            <HeaderStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TemplateName%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("OTE_NAME"), 45) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("OTE_NAME"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="28%" />
                                            <HeaderStyle Width="28%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Description%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("OTE_DESC")),80) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("OTE_DESC")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="42%" />
                                            <ItemStyle Width="42%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Status %>" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-Width="7%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imbActive" runat="server" SkinID="btninactive" Visible='<%# (Eval("OTE_ACTIVE").ToString() == "0") ?
                                               true  : false %>' CommandName="ACTIVATE" ToolTip="Inactive" OnClick="ActionHandler"
                                                    CssClass="Active" TabIndex="11" />
                                                <asp:ImageButton ID="imbInActive" runat="server" SkinID="btnactive" Visible='<%# (Eval("OTE_ACTIVE").ToString() == "1") ?
                                               true  : false %>' CommandName="DEACTIVATE" ToolTip="Active" OnClick="ActionHandler"
                                                    CssClass="Active" TabIndex="11" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
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
                        <%--EntryPage Table Row--%>
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="label1" runat="server" Text="<%$ resources:TemplateCodeStar%>" AssociatedControlID="txtTemplateCode"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTemplateCode" TabIndex="7" CssClass="input-half" MaxLength="80"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvTemplateCode" runat="server" ControlToValidate="txtTemplateCode"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_EnterTemplateCode%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S" style="float: right;">
                                            <asp:Label runat="server" ID="label2" Text="<%$ resources:TemplateNameStar%>" AssociatedControlID="txtTemplateName"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTemplateName" TabIndex="8" CssClass="input-half" MaxLength="180"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvTemplateName" runat="server" ControlToValidate="txtTemplateName"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_EnterTemplateName%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblDescription" Text="<%$ resources:Description%>"
                                                AssociatedControlID="txtDescription"></asp:Label>
                                            <asp:TextBox ID="txtDescription" runat="server" TabIndex="8" TextMode="MultiLine"
                                                onkeypress="return this.value.length<490" onpaste="return this.value.length<490"
                                                Height="40" CssClass="input-full"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblActive" runat="server" Text="<%$ resources:Active%>" AssociatedControlID="lblActive"></asp:Label>
                                            <asp:CheckBox ID="chkActive" runat="server" Checked="true" TabIndex="8" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="search-colapse-b">
                                            <h1>
                                                <asp:Literal ID="Literal2" runat="server" Text="<%$ resources: OvertimeCalculation%>" /></h1>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="div2col-S">
                                            <asp:GridView runat="server" ID="grdWeekDays" AllowPaging="false" AutoGenerateColumns="false"
                                                EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-CssClass="emptytable"
                                                OnRowCommand="ActionHandler" Width="450">
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ resources:Day%> ">
                                                        <ItemTemplate>
                                                            <asp:HiddenField runat="server" ID="hdfOTD_PK" Value='<%# Eval("OTD_PK") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfOTD_FORMULA" Value='<%# Eval("OTD_FORMULA") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfOTD_WEEK_DAY" Value='<%# Eval("OTD_WEEK_DAY") %>' />
                                                            <asp:Label ID="lblWeekDay" runat="server" Text='<%# Eval("OTD_WEEK_DAY_TEXT") %>'
                                                                CssClass="border0" ToolTip='<%# Eval("OTD_WEEK_DAY_TEXT") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="15%" CssClass="grid-col1-disable" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$resources:Formula%>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFormula" runat="server" ToolTip='<%# Eval("OTD_FORMULA_TEXT") %>'
                                                                CssClass="grid-col2-disable" Text='<%# Eval("OTD_FORMULA_TEXT") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="78%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgFromula" runat="server" SkinID="salary-formula" TabIndex="9"
                                                                CommandName="EDIT_ACTION" ToolTip="<%$ resources:Controls,ApplyFormula%>" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="7%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div id="divPopUpFormula" style="display: none">
                                <uc1:PopUp ID="ucFormulaMaster" runat="server" AfterApply="ucPopUpFormula_AfterApply"
                                    IsDeduction="0" HeaderText="<%$ resources:Day%>" />
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
            <div id="diverrorAlert" style="display: none">
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
