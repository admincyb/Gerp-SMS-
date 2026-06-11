<%@ Page Title="<%$ Resources:Captions,Title_BrandGroup %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="BrandGroupMaster.aspx.cs" Inherits="ERPSMS_v01.Administration.Masters.BrandGroupMaster"
    Theme="ClassicExt" EnableEventValidation="false" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
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

        function InitComponents() {
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
        function ValidatebRPage(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                CheckValidationDuplicate(valGroup);
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlPacking" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label ID="lblBreadCrum" runat="server" />
                                </ul>
                                <ul id="pnlEntry" runat="server" style="display: none">
                                    <li id="pnlSave" runat="server">
                                        <asp:Button ID="btnSave" runat="server" CommandName="SAVE" Text="<%$ resources:Controls,Save %>"
                                            OnClientClick="javascript:ValidatebRPage('BrandGroup')" ValidationGroup="BrandGroup"
                                            SkinID="btnInner-Save" TabIndex="34" CommandArgument="SEC_ActionPanel" ToolTip="<%$ resources:Controls,Save %>"
                                            OnClick="ActionHandler" />
                                    </li>
                                    <li id="pnlDelete" runat="server">
                                        <asp:Button ID="btnDelete" runat="server" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" OnClick="ActionHandler"
                                            TabIndex="35" ToolTip="<%$ resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" CommandName="CANCEL" Text="<%$resources:Controls,Cancel %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" OnClick="ActionHandler"
                                            TabIndex="36" ToolTip="<%$ resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                                <ul id="pnlListing" runat="server" style="display: none">
                                    <li>
                                        <asp:Button ID="btnNew" runat="server" CommandName="NEW" Text="<%$ resources:Controls,New %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-New" ToolTip="<%$ resources:Controls,New %>"
                                            OnClick="ActionHandler" TabIndex="5" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnEdit" runat="server" CommandName="EDIT" Text="<%$ resources:Controls,Edit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit" ToolTip="<%$ resources:Controls,Edit %>"
                                            OnClick="ActionHandler" TabIndex="6" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnView" runat="server" CommandName="VIEW" Text="<%$ resources:Controls,View %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-View" ToolTip="<%$ resources:Controls,View %>"
                                            OnClick="ActionHandler" TabIndex="7" />
                                    </li>
                                </ul>
                            </asp:TableCell></asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table ID="tblTemplate" runat="server" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-wrap-custom">
                                <asp:Label ID="lblFilterBy" runat="server" Text="<%$ resources:Controls,SearchBy %>"
                                    AssociatedControlID="ddlFilterBy" />
                                <asp:DropDownList ID="ddlFilterBy" runat="server" TabIndex="1" CssClass="select-small-a">
                                    <asp:ListItem Text="<%$ resources:BrandGroupName %>" Value="<%$ resources:DataFieldRes,BrandGroupName %>" />
                                    <asp:ListItem Text="<%$ resources:BrandGroupCode %>" Value="<%$ resources:DataFieldRes,BrandGroupCode %>" />
                                </asp:DropDownList>
                                <asp:TextBox ID="txtSearchBy" runat="server" CssClass="input-w34per" onkeydown="return Search(event);" OnClick="ActionHandler"
                                    TabIndex="2" />
                                <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" OnClick="ActionHandler"
                                    CssClass="margn-btm0" Style="margin-top: 2px;" ToolTip="<%$ resources:Controls,Search %>"
                                    CommandName="SEARCH" TabIndex="3" />
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdBrandGroup" Width="100%" AllowPaging="true" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" GroupName="SelectOne"
                                                    OnCheckedChanged="ActionHandler" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    TabIndex="4" />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" HorizontalAlign="Center" />
                                        </asp:TemplateField>                                        
                                        <asp:TemplateField HeaderText="<%$ resources:BrandGroupCode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBrandGroupCode" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetEncodedString(Eval("CIG_CODE"))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("CIG_CODE")),50) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="33%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BrandGroupName%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBrandGroupName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetEncodedString(Eval("CIG_NAME")) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("CIG_NAME")),97) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="63%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Status %>">
                                            <ItemTemplate>
                                                <img id="imgStatus" class='<%# Convert.ToInt16(Eval("CIG_ACTIVE")) == 1 ? "active" : "inactive" %>'
                                                    alt=" " title='<%# Eval("CIG_ACTIVE_TEXT") %>' />
                                                <%--<asp:Label ID="lblCompanyAddress" runat="server" ToolTip='<%# Eval("CIG_ACTIVE_TEXT") %>'
                                                    Text='<%# Eval("CIG_ACTIVE_TEXT") %>' />--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCode" runat="server" Text="<%$resources:BrandGroupCode %>" AssociatedControlID="txtBrandGroupCode"></asp:Label>
                                            <asp:TextBox ID="txtBrandGroupCode" runat="server" TabIndex="1" MaxLength="100" CssClass="input-half" onkeydown="limitText(this,100);"
                                                onkeyup="limitText(this,100);">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfBrandGroupCode" runat="server" ControlToValidate="txtBrandGroupCode"
                                                Text="*" CssClass="star" ValidationGroup="BrandGroup" EnableClientScript="true"
                                                SetFocusOnError="true" ErrorMessage="<%$ resources:Err_BrandGroupCode %>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblStatus" runat="server" CssClass="lbl-66-6perc" Text="<%$resources:Status %>" AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlStatus" TabIndex="2" CssClass="select-small-a1" >
                                                <asp:ListItem Text="<%$ resources:Controls, Active %>" Value="1" />
                                                <asp:ListItem Text="<%$ resources:Controls, InActive %>" Value="0" />
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblBrandGroupName" runat="server" Text="<%$resources:BrandGroupName %>"
                                                AssociatedControlID="txtBrandGroupName"></asp:Label>
                                            <asp:TextBox ID="txtBrandGroupName" TabIndex="3" runat="server" TextMode="MultiLine"
                                                CssClass="multiline-3line" MaxLength="200" onkeydown="limitText(this,200);" onkeyup="limitText(this,200);">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfBrandGroupName" runat="server" ControlToValidate="txtBrandGroupName"
                                                Text="*" CssClass="star" ValidationGroup="BrandGroup" EnableClientScript="true"
                                                SetFocusOnError="true" ErrorMessage="<%$ resources:Err_BrandGroupName %>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <%--Use this label to bind the server errors--%>
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="BrandGroup" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
