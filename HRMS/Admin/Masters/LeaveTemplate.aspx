<%@ Page Title="<%$ Resources:Captions,Title_LeaveTemplate %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="LeaveTemplate.aspx.cs" Inherits="HRMS.Admin.Masters.LeaveTemplate"
    Theme="ClassicExt" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Src="UserControls/FormulaMaster.ascx" TagName="PopUp" TagPrefix="uc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript">
        function InitComponents() {
            $(document).ready(function () {
                //ShowHideEarnings(1);
                //ShowHideDeductions(1);
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
            <asp:HiddenField ID="hdfCurrentSlNo" runat="server" Value="" />
            <asp:HiddenField ID="hdfCurrentLTD_PK" runat="server" Value="" />
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
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="Button1" CommandName="SAVE" TabIndex="13" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('Save')" ValidationGroup="Save"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteNew" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="13" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="13" SkinID="btnInner-Cancel"
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
                <%--Page Datas--%>
                <div class="tab-container-floating">
                    <%--Container for List and Detail tabs--%>
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
                                                TabIndex="7" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>"
                                                TabIndex="7" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table class="table-devide" id="tbladvancedSearch">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblFilterCode" runat="server" Text="<%$ resources:TemplateCode%>"
                                                AssociatedControlID="txtFilterCode"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterCode" TabIndex="7" CssClass="input-half margnbotm0"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="label6" runat="server" Text="<%$ resources:TemplateName%>" AssociatedControlID="txtTemplateNameListPage"></asp:Label>
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
                                                    TabIndex="10" />
                                                <asp:HiddenField runat="server" ID="hdfTemplatePkListPage" Value='<%# Eval("LTE_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfListLTE_MOD_DT" Value='<%# Eval("LTE_MOD_DT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                            <HeaderStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TemplateCode%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("LTE_CODE")),15) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("LTE_CODE")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                            <HeaderStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TemplateName%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("LTE_NAME"), 45) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("LTE_NAME"), 100) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="28%" />
                                            <HeaderStyle Width="28%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Description%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("LTE_DESC")),85) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("LTE_DESC")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="42%" />
                                            <ItemStyle Width="42%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Status %>">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imbActive" runat="server" SkinID="btninactive" Visible='<%# (Eval("LTE_ACTIVE").ToString() == "0") ?
                                               true  : false %>' CommandName="ACTIVATE" ToolTip="Inactive" OnClick="ActionHandler"
                                                    CssClass="Active" TabIndex="11" />
                                                <asp:ImageButton ID="imbInActive" runat="server" SkinID="btnactive" Visible='<%# (Eval("LTE_ACTIVE").ToString() == "1") ?
                                               true  : false %>' CommandName="DEACTIVATE" ToolTip="Active" OnClick="ActionHandler"
                                                    CssClass="Active" TabIndex="11" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" HorizontalAlign="Center" />
                                            <HeaderStyle Width="3%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" TabIndex="12" />
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
                                            <asp:Label ID="lblTemplateCode" runat="server" Text="<%$ resources:TemplateCodeStar%>"
                                                AssociatedControlID="txtTemplateCode"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTemplateCode" TabIndex="7" CssClass="input-half"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvTemplateCode" runat="server" ControlToValidate="txtTemplateCode"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_EnterTemplateCode%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S" style="float: right;">
                                            <asp:Label runat="server" ID="lblTemplateName" Text="<%$ resources:TemplateNameStar%>"
                                                AssociatedControlID="txtTemplateName" class="middle-lbl-small-b1"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTemplateName" TabIndex="8" CssClass="input-w64per"></asp:TextBox>
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
                                            <asp:TextBox ID="txtDescription" runat="server" TabIndex="8" MaxLength="500" TextMode="MultiLine"
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
                                    <td>
                                        <div class="div2col-S">
                                            <div class="search-colapse-b">
                                                <h1>
                                                    <asp:Literal ID="Literal2" runat="server" Text="<%$ resources: LeaveDeduction%>" /></h1>
                                            </div>
                                            <asp:Label runat="server" ID="Label4" Text="<%$resources:BasedOn %>" AssociatedControlID="ddlPayElement"></asp:Label>
                                            <asp:DropDownList ID="ddlPayElement" runat="server" TabIndex="9" ValidationGroup="Add"
                                                CssClass="select-half-a">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="reqPayElement" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlPayElement" Display="Dynamic" Text="*" InitialValue="0"
                                                ValidationGroup="AddToList" ErrorMessage="<%$ resources:ErrorMessages,Msg_SelectPayElement %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="vrfPayElementFormula" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlPayElement" Display="Dynamic" Text="*" InitialValue="0"
                                                ValidationGroup="Formula" ErrorMessage="<%$ resources:ErrorMessages,Msg_SelectPayElement %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S" style="float: right;">
                                            <div class="search-colapse-b">
                                                <h1>
                                                    <asp:Literal ID="Literal1" runat="server" Text="" /></h1>
                                            </div>
                                            <asp:Label ID="label3" runat="server" Text="<%$ resources:DeductionFormulaStar%>"
                                                AssociatedControlID="txtDeductionFormula" class="middle-lbl-small-b1"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDeductionFormula" Enabled="false" CssClass="input-disabled input-w64per"
                                                TabIndex="10"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFormula" runat="server" Value="" />
                                            <asp:RequiredFieldValidator runat="server" ID="vrftxtDeductionFormula" ControlToValidate="txtDeductionFormula"
                                                Text="*" ErrorMessage="<%$ Resources:Err_EnterDeductionFormula %>" CssClass="star"
                                                Display="Dynamic" ValidationGroup="AddToList"></asp:RequiredFieldValidator>
                                            <asp:ImageButton ID="imgFromula" runat="server" CommandName="SHOWPOPUP" SkinID="salary-formula"
                                                ToolTip="<%$ resources:Controls,ApplyFormula %>" OnClick="ActionHandler" TabIndex="11"
                                                CssClass="margntop2 margnbotm0" OnClientClick="javascript:ValidateNow('Formula')" />
                                            <asp:ImageButton ID="imgAdd" runat="server" CommandName="ADDTOLIST" SkinID="imbaddnew"
                                                ToolTip="<%$ resources:Controls,AddToList %>" OnClick="ActionHandler" TabIndex="12"
                                                ValidationGroup="AddToList" OnClientClick="javascript:ValidateNow('AddToList')"
                                                CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div runat="server" class="gridwrap">
                                <asp:GridView ID="grdLeaveDedution" runat="server" AutoGenerateColumns="False" Width="100%"
                                    EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false" ShowFooter="false"
                                    OnRowCommand="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:BasedOn %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLTD_ELEMENT_TEXT" runat="server" Text='<%# Eval("LTD_ELEMENT_TEXT") %>'
                                                    ToolTip='<%# Eval("LTD_ELEMENT_TEXT") %>'></asp:Label>
                                                <asp:HiddenField ID="hdfSlNo" Value='<%# Eval("SlNo") %>' runat="server" />
                                                <asp:HiddenField ID="hdfLTD_PK" Value='<%# Eval("LTD_PK") %>' runat="server" />
                                                <asp:HiddenField ID="hdfLTD_PAY_ELEMENT" Value='<%# Eval("LTD_PAY_ELEMENT") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="35%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Formula %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLTD_FORMULA_TEXT" runat="server" Text='<%# Eval("LTD_FORMULA_TEXT") %>'
                                                    ToolTip='<%# Eval("LTD_FORMULA_TEXT") %>'></asp:Label>
                                                <asp:HiddenField ID="hdfLTD_FORMULA" Value='<%# Eval("LTD_FORMULA") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="60.8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imbEditDetails" SkinID="imbeditgrid" EnableViewState="false"
                                                    CssClass="_edit" CommandName="EDIT_ACTION" ToolTip="<%$resources:Controls,Edit %>"
                                                    Width="16px" Height="16px" TabIndex="13" />
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbDeleteRejectedDetails"
                                                    SkinID="imbdeletegrid" EnableViewState="false" CommandName="DELETE_ACTION" OnClientClick="return ShowDeleteConfirm(this);"
                                                    ToolTip="<%$resources:Controls,Delete %>" TabIndex="13" />
                                            </ItemTemplate>
                                            <ItemStyle Width="4.2%" />
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
            <div id="divPopUpFormula" style="display: none">
                <uc1:PopUp ID="ucFormulaMaster" runat="server" AfterApply="ucPopUpFormula_AfterApply"
                    IsDeduction="0" HeaderText="<%$ resources:BasedOn%>" />
            </div>
            <div id="diverrorAlert" style="display: none">
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
                <asp:ValidationSummary ID="vsAddToList" ValidationGroup="AddToList" runat="server" />
                <asp:ValidationSummary ID="vsFormula" ValidationGroup="Formula" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
