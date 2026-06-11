<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DashletUserMapping.aspx.cs"
    Inherits="ERPSMS_v01.GeneralAdmin.DashletUserMapping" Theme="ClassicExt" MasterPageFile="~/ERPSMS_2.Master"
    Title="<%$ Resources:Captions,Title_DashletUserRole %>" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function InitComponents() {

        }

        function selectAll(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");

            if (isChkBoxClick && src.id.match(new RegExp('chkUserHeader$'))) {
                if (src.checked) {
                    $("[id$=grdUserRoles]").find("input:checkbox[id*=chkUserRoles]").attr("checked", "checked");
                }
                else {
                    $("[id$=grdUserRoles]").find("input:checkbox[id*=chkUserRoles]").removeAttr("checked");
                }
            }
            else if (isChkBoxClick) {
                if (!src.checked) {
                    $("[id$=grdUserRoles]").find("input:checkbox[id$=chkUserHeader]").removeAttr("checked");
                }
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
        function ShowHideGroupDetails(flag) {

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
            }
            else if (mode == 2) {
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
                                            ValidationGroup="Save" OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('Save')"
                                            TabIndex="80" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            CssClass="popupclose" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" OnClick="ActionHandler"
                                            TabIndex="80" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
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
                                            <asp:Label ID="lblNameFilterList" runat="server" Text="<%$ resources:Name%>" AssociatedControlID="txtNameFilterList"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtNameFilterList" TabIndex="2" CssClass="input-small-d margnbotm0"></asp:TextBox>
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7 ">
                                            <asp:Label ID="lblTypeFilter" runat="server" Text="<%$ resources:Type%>" AssociatedControlID="ddlTypeFilter"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlTypeFilter" TabIndex="5" CssClass="select-w22-6per">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblModuleFilter" runat="server" Text="<%$ resources:Module %>" AssociatedControlID="ddlModuleFilter" CssClass="lbl-10-7perc"></asp:Label>
                                             <asp:DropDownList runat="server" ID="ddlModuleFilter" TabIndex="6" CssClass="select-w22-6per">
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="btnSearchList" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="3"
                                                CommandName="FILTER" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClearList" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="3" OnClick="ActionHandler"
                                                CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnlft-minus2 margnbotm0" />
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="grdTable">
                                <asp:GridView runat="server" ID="grdList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" OnPageIndexChanging="ActionHandler" CssClass="grdTable"
                                    OnSorting="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    TabIndex="4" />
                                                <asp:HiddenField runat="server" ID="hdfDLC_PK_List" Value='<%# Eval("DLC_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfDLC_MODULE_List" Value='<%# Eval("DLC_MODULE") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                            <HeaderStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Name%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvNameList" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("DLC_NAME")),50) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("DLC_NAME")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Description%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvDescList" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("DLC_DESC")),80) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("DLC_DESC")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="45%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DashletType%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvTypeList" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("DLC_TYPE_TEXT")),15) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("DLC_TYPE_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                            <HeaderStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Active%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvActiveList" runat="server" Text='<%# Eval("DLC_ACTIVE")!=null?Eval("DLC_ACTIVE").ToString()==ERP.Utilities.CommonConstants.SELECT_VALUE_ONE?GetLocalResourceObject("Active").ToString(): GetLocalResourceObject("Inactive").ToString() :string.Empty %>'
                                                    ToolTip='<%# Eval("DLC_ACTIVE")!=null?Eval("DLC_ACTIVE").ToString()==ERP.Utilities.CommonConstants.SELECT_VALUE_ONE?GetLocalResourceObject("Active").ToString(): GetLocalResourceObject("Inactive").ToString() :string.Empty %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="6%" />
                                            <ItemStyle Width="6%" />
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
                            <div class="detail-poi-co1">
                                <asp:Label ID="lblDashletHd" runat="server" Text=" " Font-Bold="True" CssClass="txt-center"></asp:Label>
                            </div>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblDept" runat="server" Text="<%$ resources:Department%>" AssociatedControlID="ddlDept"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlDept" TabIndex="8" CssClass="select-half-d"
                                                AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Button ID="btnViewDashlet" SkinID="btnInner-View" runat="server" Text="<%$resources:Controls,View %>"
                                                CommandName="VIEW_ACTION" ToolTip="<%$resources:Controls,View %>" OnClick="ActionHandler"
                                                TabIndex="9" Visible="false"/>
                                           
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div id="Div2" runat="server" class="gridwrap maxh-225">
                                <asp:GridView ID="grdUserRoles" runat="server" AutoGenerateColumns="False" Width="100%"
                                    EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false" ShowFooter="false"
                                    OnRowCommand="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label></EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField SortExpression="">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkUserHeader" runat="server" ToolTip="Select All Users" TabIndex="9"
                                                    onclick="selectAll(event);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkUserRoles" runat="server" TabIndex="9" onclick="selectAll(event);" />
                                                <asp:HiddenField ID="hdfusgPK" Value='<%# Eval("usgPK") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:UserRoles %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUserRoleText" runat="server" Text='<%#(Eval("usgText")) %>' ToolTip='<%# Eval("usgText") %>'></asp:Label></ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="<%$ resources:Department %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDepartment" runat="server" Text='<%#(Eval("DPT_NAME")) %>' ToolTip='<%# Eval("DPT_NAME") %>'></asp:Label></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:View %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtView" Checked="true" runat="server" GroupName="User"></asp:RadioButton></ItemTemplate>
                                            <ItemStyle Width="6%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Action %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtAction" runat="server" GroupName="User"></asp:RadioButton></ItemTemplate>
                                            <ItemStyle Width="6%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div style="float: right;">
                                <asp:Button ID="btnAddDashlet" SkinID="btnInner-add-dsd" runat="server" Text="<%$resources:Controls,Add_Add %>"
                                    CommandName="ITEMSAVE" OnClick="ActionHandler" TabIndex="9" />
                            </div>
                            <div class="clear">
                            </div>
                            <div id="divGrdUserRolesList" runat="server" class="gridwrap  maxh-225">
                                <asp:GridView ID="grdUserRolesList" runat="server" AutoGenerateColumns="False" Width="100%"
                                    EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false" ShowFooter="false"
                                    OnRowCommand="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label></EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:UserRoles %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMenuNamePopUpList" runat="server" Text='<%#(Eval("DLM_USER_GROUP_TEXT")) %>'
                                                    ToolTip='<%# Eval("DLM_USER_GROUP_TEXT") %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfDLM_PK" Value='<%# Eval("DLM_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfDLM_DASHLET" Value='<%# Eval("DLM_DASHLET") %>' />
                                                <asp:HiddenField runat="server" ID="hdfDLM_USER_GROUP" Value='<%# Eval("DLM_USER_GROUP") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:View %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUserViewList" Text='<%# Convert.ToInt32(Eval("DLM_VIEW")) == 0 ?  "No" : "Yes" %>'
                                                    runat="server" CssClass="lbl-15-1perc" ToolTip='<%# Convert.ToInt32(Eval("DLM_VIEW")) == 0 ?  "No" : "Yes" %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfDLM_VIEW" Value='<%# Eval("DLM_VIEW") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Action %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUserActionList" Text='<%# Eval("DLM_ACTION")!=null?Eval("DLM_ACTION").ToString()==ERP.Utilities.CommonConstants.SELECT_VALUE_ONE?GetLocalResourceObject("ActionYes").ToString(): GetLocalResourceObject("ActionNo").ToString() :string.Empty %>'
                                                    runat="server" CssClass="lbl-15-1perc" ToolTip='<%# Eval("DLM_ACTION")!=null?Eval("DLM_ACTION").ToString()==ERP.Utilities.CommonConstants.SELECT_VALUE_ONE?GetLocalResourceObject("ActionYes").ToString(): GetLocalResourceObject("ActionNo").ToString() :string.Empty %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfDLM_ACTION" Value='<%# Eval("DLM_ACTION") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <HeaderTemplate>
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbDeleteMenuPopList"
                                                    SkinID="imbdeletegrid" EnableViewState="false" CommandName="DELETE_ACTION_ALL" OnClientClick="return ShowDeleteConfirm(this);"
                                                    ToolTip="<%$resources:Controls,Delete %>" TabIndex="12" /> 
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbDeleteMenuPopList"
                                                    SkinID="imbdeletegrid" EnableViewState="false" CommandName="DELETE_ACTION" OnClientClick="return ShowDeleteConfirm(this);"
                                                    ToolTip="<%$resources:Controls,Delete %>" TabIndex="12" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
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
            <div id="diverrorAlert" style="display: none">
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
                <asp:ValidationSummary ID="vsAddToList" ValidationGroup="AddToList" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label></div>
            <asp:HiddenField ID="hdfCompany" runat="server" Value="0" />
            <asp:HiddenField ID="hdfAdvSearch" Value="0" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
