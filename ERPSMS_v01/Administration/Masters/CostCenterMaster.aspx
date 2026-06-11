<%@ Page Title="<%$ Resources:Title_CostCenterMaster %>" Language="C#" Theme="ClassicExt"
    EnableEventValidation="false" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="CostCenterMaster.aspx.cs" Inherits="ERPSMS_v01.Administration.Masters.CostCenterMaster" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--    <script type="text/javascript">
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
                $("[id$='pnlPrint']").hide();
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

    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%--    <asp:UpdatePanel ID="auplDetailList" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons" style="padding-bottom: 30px !important;">
                <div class="Button-container">
                    <asp:Table runat="server" ID="tblButton">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum" Text="<%$ resources:Breadcrumb%>"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" OnClick="ActionHandler"
                                            Text="<%$ resources:Controls,Save%>" ToolTip="<%$ resources:Controls,Save%>"
                                            OnClientClick="javascript:ValidateNow('Save')" ValidationGroup="Save" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" TabIndex="21" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$Resources:Controls,Delete%>"
                                            ToolTip="<%$Resources:Controls,Delete%>" OnClientClick="return ShowDeleteConfirm(this);"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" TabIndex="22" OnClick="ActionHandler" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCanel" Text="<%$ resources:Controls,Cancel%>" ToolTip="<%$ resources:Controls,Cancel%>"
                                            CommandName="CANCEL" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel"
                                            TabIndex="23" OnClick="ActionHandler" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="24" ID="btnNew" CommandName="NEW" Text="<%$resources:Controls,New %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-New" ToolTip="<%$resources:Controls,New %>"
                                            OnClick="ActionHandler" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="25" ID="btnEdit" CommandName="EDIT" Text="<%$resources:Controls,Edit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit" ToolTip="<%$resources:Controls,Edit %>"
                                            OnClick="ActionHandler" />
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
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="DETAILS"
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
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:Controls,ShowFilter%>"
                                                TabIndex="9" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:Controls,HideFilter%>"
                                                TabIndex="9" />
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
                                         <asp:Label ID="lblCostCenterCode" runat="server" AssociatedControlID="txtCostCenterCode"
                                                Text="<%$ resources:CostCenterCode%>" CssClass="middle-lbl-d"></asp:Label>
                                            <asp:TextBox ID="txtCostCenterCode" runat="server" onkeydown="return Search(event);"
                                                CssClass="select-small-e2 margnbotm0" MaxLength="50" TabIndex="2" />
                                            <asp:HiddenField ID="hdftxtCostCenterCode" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                          <asp:Label ID="lblCostCenterName" runat="server" AssociatedControlID="txtCostCenterName"
                                                Text="<%$ resources:CostCenterName%>"></asp:Label>
                                            <asp:TextBox ID="txtCostCenterName" runat="server" TabIndex="1" onkeydown="return Search(event);"
                                                CssClass="select-small-e2 margnbotm0" MaxLength="200" />
                                            <asp:HiddenField ID="hdfCostCenterName" runat="server" />                                          
                                            <asp:ImageButton ID="btnSearch1" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="4"
                                                CommandName="SEARCH" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClear1" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="5" OnClick="ActionHandler"
                                                CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="grdTable">
                                <asp:GridView runat="server" ID="grdCCList" Width="100%" AllowPaging="false" PageSize="25"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    CssClass="grdTable" EmptyDataRowStyle-CssClass="emptytable" OnSorting="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfCostCenterPk" Value='<%# Eval("CNM_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CostCenterCode %> ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCCCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("CNM_CODE")),40) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("CNM_CODE")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CostCenterName %> ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCCName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("CNM_NAME")),40) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("CNM_NAME")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CostCenterDesc %> ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCCDesc" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("CNM_DESC")),60) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("CNM_DESC")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Status %>">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imbActive" runat="server" SkinID="btninactive" CommandName="ACTIVATE"
                                                    Visible='<%# (Eval("CNM_ACTIVE").ToString() == "0") ?
                                               true  : false %>' ToolTip="<%$ resources:Inactive %>" OnClick="ActionHandler" CssClass="Active" />
                                                <asp:ImageButton ID="imbInActive" runat="server" SkinID="btnactive" CommandName="INACTIVATE"
                                                    Visible='<%# (Eval("CNM_ACTIVE").ToString() == "1") ?
                                               true  : false %>' ToolTip="<%$ resources:Active %>" OnClick="ActionHandler" CssClass="Active" />
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" HorizontalAlign="Center" />
                                            <HeaderStyle Width="1%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
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
                                            <asp:Label ID="lblcode" runat="server" Text="<%$ resources:CostCenterCodeStar%>"
                                                AssociatedControlID="txtCode"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtCode" TabIndex="5" CssClass="input-half"  
                                              onkeydown="limitText(this,15);"  onkeyup="limitText(this,15);"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvCode" runat="server" ControlToValidate="txtCode"
                                                CssClass="star" ValidationGroup="Save" SetFocusOnError="true"  
                                                   Display="Dynamic" ValidationExpression="^(.|\n){1,15}$" 
                                                Text="*" ErrorMessage="<%$ resources:Err_EnterCode%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblName" runat="server" Text="<%$ resources:CostCenterNameStar%>"
                                                AssociatedControlID="txtName"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtName" TabIndex="6" CssClass="input-half"
                                             onkeydown="limitText(this,40);"  onkeyup="limitText(this,40);">
                                             </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                                                CssClass="star" ValidationGroup="Save" Text="*"  SetFocusOnError="true"  
                                                   Display="Dynamic" ValidationExpression="^(.|\n){1,40}$"
                                                ErrorMessage="<%$ resources:Err_EnterName%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                   
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblDescription" Text="<%$ resources:CostCenterDesc%>"
                                                AssociatedControlID="txtDescription"></asp:Label>
                                            <asp:TextBox ID="txtDescription" runat="server" TabIndex="7" TextMode="MultiLine"
                                                Height="40" CssClass="input-full" onkeydown="limitText(this,500);"  onkeyup="limitText(this,500);">
                                                    </asp:TextBox>
                                         <asp:RegularExpressionValidator runat="server" ID="revDescription" CssClass="star"
                                                    ValidationGroup="CostMaster" SetFocusOnError="true" ControlToValidate="txtDescription"
                                                    Display="Dynamic" ValidationExpression="^(.|\n){1,500}$" Text="*" ErrorMessage="<%$ resources:ErpRes, Msg_Exceed_MaxLen %>"
                                                    EnableClientScript="true" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        
                                            <div class="div2col-S">
                                                <asp:Label ID="lblGroup" runat="server" Text="<%$ resources:GroupStar%>" AssociatedControlID="ddlGroup"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlGroup" TabIndex="8" CssClass="input-medium"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvGroup" runat="server" ControlToValidate="ddlGroup"
                                                    CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_SelectGroup%>"></asp:RequiredFieldValidator>
                                                  
                                            </div>
                                            
                                    </td>
                                  
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCompany" runat="server" Text="<%$ resources:Controls, CompanyPlant %>" AssociatedControlID="ddlCompany"
                                                ></asp:Label>
                                            <asp:DropDownList ID="ddlCompany" runat="server" CssClass="select-small-a1" TabIndex="9">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblActive" runat="server" Text="<%$ resources:Active%>" AssociatedControlID="lblActive"></asp:Label>
                                            <asp:CheckBox ID="chkActive" runat="server" TabIndex="10" Checked="true" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverrorAlert" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="Save" runat="server" />
            </div>
            <asp:HiddenField ID="hdfCompany" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
