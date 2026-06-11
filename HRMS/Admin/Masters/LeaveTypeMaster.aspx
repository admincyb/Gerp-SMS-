<%@ Page Title="<%$ Resources:Captions,Title_LeaveTypeMaster %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="LeaveTypeMaster.aspx.cs"
    Inherits="HRMS.Admin.Masters.LeaveTypeMaster" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function InitComponents() {
            $("[id*=txtRate]").ForceNumericOnly();
            $("[id*=txtMaxEligibility]").ForceNumericOnly();
            $("[id*=txtFactor]").ForceNumericOnly();
            ShowHideAdvancedSearch(1);
            CarryForwardChange();
            CreditCheckboxChange();
        }
        function CarryForwardChange() {
            if ($("[id*=chkCarryForward]").attr('checked')) {
                $("[id*=txtLimit]").removeAttr('disabled');
                $("[id*=txtLimit]").removeClass("input-disabled");
            } else {
                $("[id*=txtLimit]").attr('disabled', 'disabled');
                $("[id*=txtLimit]").addClass("input-disabled");
                $("[id*=txtLimit]").val("0");
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
        //For finding and removing duplicate and other group validation controls
        //Array of present validations

        var validationArrayGroup;

        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
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
                        Page_Validators.splice(i, 1);
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


        function CreditCheckboxChange() {
            var chkCredit = $("[id*=chkCredit]");
            if (chkCredit.is(":checked")) {
                $("[id*=ddlAutoCrType]").attr('disabled', false);
                $("[id*=txtMaxEligibility]").removeAttr('disabled');
                $("[id*=txtMaxEligibility]").removeClass("input-disabled");
            } else {
                $("[id*=ddlAutoCrType]").attr('disabled', true);
                $("select[id$=ddlAutoCrType]").val(0);
                $("[id*=txtMaxEligibility]").attr('disabled', 'disabled');
                $("[id*=txtMaxEligibility]").addClass("input-disabled");
                $("[id*=txtMaxEligibility]").val("0");
            }
        }
            </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="auplDetailList" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table runat="server" ID="tblButton">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum" Text="<%$ resources:Breadcrumb%>"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$ resources:Controls,Save%>"
                                            ToolTip="<%$ resources:Controls,Save%>" OnClick="ActionHandler" ValidationGroup="save"
                                            OnClientClick="javascript:ValidatePageNow('Save')" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" TabIndex="52" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$Resources:Controls,Delete%>"
                                            ToolTip="<%$Resources:Controls,Delete%>" OnClick="ActionHandler" OnClientClick="return ShowDeleteConfirm(this);"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" TabIndex="53" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCanel" Text="<%$ resources:Controls,Cancel%>" ToolTip="<%$ resources:Controls,Cancel%>"
                                            OnClick="ActionHandler" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" TabIndex="54" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="7" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="8" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
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
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="1" OnClick="ActionHandler" CommandName="LIST"
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
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:ShowFilter%>" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <table class="table-devide" style="display: none;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblSearchAccural" runat="server" Text="<%$ resources:Accural%>" AssociatedControlID="ddlSearchAccural"
                                                Visible="false"></asp:Label>
                                            <asp:DropDownList ID="ddlSearchAccural" runat="server" CssClass="select-medium" Visible="false">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblSearchActive" runat="server" Text="<%$ resources:Active%>" AssociatedControlID="chkSearchActive"
                                                Visible="false"></asp:Label>
                                            <asp:CheckBox ID="chkSearchActive" runat="server" Checked="true" Visible="false" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblSearchCode" runat="server" Text="<%$ resources:Code%>" AssociatedControlID="txtSearchCode"></asp:Label>
                                            <asp:TextBox ID="txtSearchCode" runat="server" TabIndex="1" CssClass="input-half margnbotm0"
                                                MaxLength="100"> </asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblSearchName" runat="server" Text="<%$ resources:Name%>" AssociatedControlID="txtSearchName"></asp:Label>
                                            <asp:TextBox ID="txtSearchName" TabIndex="2" runat="server" MaxLength="100" CssClass="input-half margnbotm0"> </asp:TextBox>
                                            <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Search%>" ToolTip="<%$ resources:Search%>"
                                                ValidationGroup="Search" OnClick="ActionHandler" TabIndex="11" CommandName="FILTER"
                                                SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Clear%>" ToolTip="<%$ resources:Clear%>"
                                                TabIndex="12" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext"
                                                CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="grdTable">
                                <asp:GridView runat="server" ID="grdList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" CssClass="grdTable" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" OnPageIndexChanging="ActionHandler" OnSorting="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" GroupName="SelectOne"
                                                    TabIndex="0" AutoPostBack="true" OnCheckedChanged="ActionHandler" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfLeavePk" Value='<%# Eval("LTM_PK") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Code%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("LTM_CODE")),15) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("LTM_CODE")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="22%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Name%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("LTM_NAME")),40) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("LTM_NAME")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="38%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Paid%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemPaid" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("LTM_PAID").ToString() == "0" ? "No" : "Yes"),20) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("LTM_PAID").ToString() == "0" ? "No" : "Yes"))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Credit%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemCredit" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("LTM_CREDIT").ToString() == "0" ? "No" : "Yes"),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("LTM_CREDIT").ToString() == "0" ? "No" : "Yes"))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Accural%>  " SortExpression="" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemAccural" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("LTM_ACCURAL_TEXT")),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("LTM_ACCURAL_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:MaxEligibility%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemMaxEligibility" runat="server" Text='<%#  ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("LTM_LIMIT")),30)  %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("LTM_LIMIT"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" CssClass="txtAlign-right" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <HeaderStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Active %>" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imbActive" runat="server" SkinID="btninactive" Visible='<%# (Eval("LTM_ACTIVE").ToString() == "0") ?
                                               true  : false %>' CommandName="ACTIVATE" ToolTip="Inactive" OnClick="ActionHandler"
                                                    CssClass="Active" TabIndex="7" />
                                                <asp:ImageButton ID="imbInActive" runat="server" SkinID="btnactive" Visible='<%# (Eval("LTM_ACTIVE").ToString() == "1") ?
                                               true  : false %>' CommandName="DEACTIVATE" ToolTip="Active" OnClick="ActionHandler"
                                                    CssClass="Active" TabIndex="7" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:Active%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemActive" runat="server" Text='<%# Eval("LTM_ACTIVE")!=null?Eval("LTM_ACTIVE").ToString()==ERP.Utilities.CommonConstants.SELECT_VALUE_ONE?GetLocalResourceObject("Active").ToString(): GetLocalResourceObject("Inactive").ToString() :string.Empty %>'
                                                    ToolTip='<%# Eval("LTM_ACTIVE")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
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
                                            <asp:Label ID="lblCode" runat="server" Text="<%$ resources:CodeStar%>" AssociatedControlID="txtCode"></asp:Label>
                                            <asp:TextBox ID="txtCode" runat="server" CssClass="input-small" MaxLength="100" TabIndex="1"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvCode" runat="server" ControlToValidate="txtCode"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_Code%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblName" runat="server" Text="<%$ resources:NameStar%>" AssociatedControlID="txtName"></asp:Label>
                                            <asp:TextBox ID="txtName" runat="server" CssClass="input-half" MaxLength="100" TabIndex="2"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_Name%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblPaid" runat="server" Text="<%$ resources:Paid%>" AssociatedControlID="lblPaid"></asp:Label>
                                            <asp:CheckBox ID="chkPaid" runat="server" TabIndex="2" />
                                            <asp:Label ID="lblCredit" runat="server" Text="<%$ resources:Credit%>" AssociatedControlID="lblCredit"
                                                CssClass="lbl-34-7perc"></asp:Label>
                                            <asp:CheckBox ID="chkCredit" runat="server" TabIndex="2" onclick = "CreditCheckboxChange();" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCarryForward" runat="server" Text="<%$ resources:CarryForward%>"
                                                AssociatedControlID="lblCarryForward"></asp:Label>
                                            <asp:CheckBox ID="chkCarryForward" runat="server" TabIndex="2" CssClass="style-none padgtop0"
                                                onchange="CarryForwardChange();" />
                                            <asp:Label ID="lblEncashable" runat="server" Text="<%$ resources:Encashable%>" AssociatedControlID="lblEncashable"
                                                CssClass="lbl-31-7perc"></asp:Label>
                                            <asp:CheckBox ID="chkEncashable" runat="server" TabIndex="2" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblDescription" Text="<%$ resources:Description%>"
                                                AssociatedControlID="txtDescription"></asp:Label>
                                            <asp:TextBox ID="txtDescription" runat="server" TabIndex="2" MaxLength="500" TextMode="MultiLine"
                                                Height="40" CssClass="input-full"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblLimit" runat="server" Text="<%$ resources:Limit%>" AssociatedControlID="txtLimit"></asp:Label>
                                            <asp:TextBox ID="txtLimit" runat="server" TabIndex="2" onpaste="return false;" CssClass="input-small"
                                                MaxLength="5" onkeyup="limitText(this,5);" onkeydown="limitText(this,5);" onDrop="return false;"></asp:TextBox>
                                            <asp:Label ID="lblMaxEligibility" runat="server" Text="<%$ resources:MaxEligibility%>"
                                                AssociatedControlID="txtMaxEligibility" CssClass="lbl-17-8perc"></asp:Label>
                                            <asp:TextBox ID="txtMaxEligibility" runat="server" TabIndex="2" ValidationGroup="Save"
                                                onpaste="return false;" CssClass="input-small" MaxLength="5" onkeyup="limitText(this,5);"
                                                onkeydown="limitText(this,5);" onDrop="return false;"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblAutoCrType" runat="server" Text="<%$ resources:AutoCrType %>" AssociatedControlID="ddlAutoCrType"></asp:Label>
                                            <asp:DropDownList ID="ddlAutoCrType" runat="server" CssClass="select-small-a" TabIndex="2">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblActive" runat="server" Text="<%$ resources:Active%>" AssociatedControlID="lblActive" CssClass="lbl-17perc"></asp:Label>
                                            <asp:CheckBox ID="chkActive" runat="server" TabIndex="2" Checked="true" />
                                            <asp:Label ID="lblRate" runat="server" Text="<%$ resources:Rate%>" AssociatedControlID="txtRate"
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblAccural" runat="server" Text="<%$ resources:Accural%>" AssociatedControlID="ddlAccural"
                                                Visible="false">
                                            </asp:Label>
                                            <asp:DropDownList ID="ddlAccural" runat="server" TabIndex="2" CssClass="input-small"
                                                Visible="false">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtRate" runat="server" TabIndex="2" ValidationGroup="Save" onpaste="return false;"
                                                MaxLength="5" onkeyup="limitText(this,5);" onkeydown="limitText(this,5);" onDrop="return false;"
                                                CssClass="input-small" Visible="false"></asp:TextBox>
                                            <asp:Label ID="lblFactor" runat="server" Text="<%$ resources:Factor%>" AssociatedControlID="txtFactor"
                                                CssClass="lbl-17-7perc" Visible="false"></asp:Label>
                                            <asp:TextBox ID="txtFactor" runat="server" TabIndex="2" ValidationGroup="Save" onpaste="return false;"
                                                MaxLength="5" onkeyup="limitText(this,5);" onkeydown="limitText(this,5);" onDrop="return false;"
                                                CssClass="input-small-c" Visible="false"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <%-- <div class="clear">
                                            </div>--%>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="Save" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
