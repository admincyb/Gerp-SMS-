<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckListMaster.aspx.cs"
    Inherits="ERPSMS_v01.Inventory.Masters.CheckListMaster" Title="<%$ Resources:Captions,Title_CheckListMaster %>"
    Theme="ClassicExt" EnableEventValidation="false" MasterPageFile="~/ERPSMS_2.Master"
    ValidateRequest="false" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="cntScript" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function InitComponents() {
            $("[id$='lblValidGroupCode']").hide();
            $("[id$='lblValidGroupName']").hide();
            $("[id$='lblValidGroupSequence']").hide();
            $("[id$='lblValidItemCode']").hide();
            $("[id$='lblValidItemName']").hide();
            $("[id$='lblValidItemSequence']").hide();
            $("[id$='lblValidControl']").hide();
            $("[id$='lblValidParameter']").hide();
            $("[id$='Parameterrow']").hide();
        }
        function ValidateGroup() {
            var isValid = true;
            var msg = "";

            $("[id$='lblValidGroupCode']").hide();
            $("[id$='lblValidGroupName']").hide();
            $("[id$='lblValidGroupSequence']").hide();

            if ($("[id$='txtGroupCode']").val() == '') {
                $("[id$='lblValidGroupCode']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Code") %></li></ul>';
            }

            if ($("[id$='txtGroupName']").val() == '') {
                $("[id$='lblValidGroupName']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Name") %></li></ul>';
            }

            if ($("[id$='txtGroupSequence']").val() == '') {
                $("[id$='lblValidGroupSequence']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Sequence") %></li></ul>';
            }

            if (!isValid) {
                $("[id$=litGroupErrorMsg]").show();
                $("[id$=litGroupErrorMsg]").html(msg);
                ShowErrorMessage($("#diverrorGroup").html(), '<%= Resources.Messages.Information %>');
            }
            return isValid;
        }

        function ValidateItem() {
            var isValid = true;
            var msg = "";

            $("[id$='lblValidItemCode']").hide();
            $("[id$='lblValidItemName']").hide();
            $("[id$='lblValidItemSequence']").hide();
            $("[id$='lblValidControl']").hide();
            $("[id$='lblValidParameter']").hide();

            if ($("[id$='ddlGroup']").val() == '-1') {
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Group") %></li></ul>';
            }

            if ($("[id$='txtItemCode']").val() == '') {
                $("[id$='lblValidItemCode']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Code") %></li></ul>';
            }

            if ($("[id$='txtItemName']").val() == '') {
                $("[id$='lblValidItemName']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Name") %></li></ul>';
            }

            if ($("[id$='txtItemSequence']").val() == '') {
                $("[id$='lblValidItemSequence']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Sequence") %></li></ul>';
            }

            if ($("[id$='ddlControl']").val() == '-1') {
                $("[id$='lblValidControl']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Control") %></li></ul>';
            }

            if ($("[id$='ddlControl']").val() == '4' && $("[id$='ddlParameter']").val() == '-1') {
                $("[id$='lblValidParameter']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Key") %></li></ul>';
            }

            if (!isValid) {
                $("[id$=litItemErrorMsg]").show();
                $("[id$=litItemErrorMsg]").html(msg);
                ShowErrorMessage($("#diverrorItem").html(), '<%= Resources.Messages.Information %>');
            }
            return isValid;
        }

        function NewGroup() {
            if ($("[id$='ddlGroup']").val() == '-1') {
                $("[id$='btnSave']").hide();
                $("[id$='addItem']").hide();
                $("[id$='itemlist']").hide();
                $("[id$='ItemTitle']").hide();

            }
            else {
                $("[id$='btnSave']").show();
                $("[id$='addItem']").show();
                $("[id$='itemlist']").show();
                $("[id$='ItemTitle']").show();
            }
        }

        function InvalidType() {
            $("[id$=litItemErrorMsg]").show();
            $("[id$=litItemErrorMsg]").html('<ul><li><%= GetLocalResourceObject("Err_Type") %></li></ul>');
            ShowErrorMessage($("#diverrorItem").html(), '<%= Resources.Messages.Information %>');

        }

        function GroupIsOpen() {
            if ($(".ui-dialog").last().is(':visible') == true) {
                return false;
            }
        }

        function ShowGroup() {
            $("#dialog-Template").dialog({ autoOpen: false });
            $("#dialog:ui-dialog").dialog("destroy");
            $("#dialog-Template").dialog({ width: 920, height: 500, buttons: {} });
            $("#dialog-Template").dialog("open").parents("div:eq(0)").appendTo($(document.forms[0]));
        }

        function EnableDisableKey() {
            //control 4 - dropdown
            if ($("[id$='ddlControl']").val() == '4') {
                $("[id$='Parameterrow']").show();
            }
            else {
                $("[id$='Parameterrow']").hide();
            }
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        function ShowListing(flag) {
            if (flag) {
                $("[id$='itemlist']").show();
                $("[id$='addItem']").hide();
                $("[id$='pnlListing']").show();
                $("[id$='pnlEntry']").hide();
            }
            else {
                $("[id$='itemlist']").hide();
                $("[id$='addItem']").show();
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

        function SetTabs(tab) {
            if (tab == 1) {
                $("[id$='spnListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnDetails']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnDetails']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='Group']").show();
            }
            else {
                $("[id$='spnListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnDetails']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnDetails']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='Group']").hide();
            }
            $('#diverrorGroup').hide();
        }

    </script>
</asp:Content>
<asp:Content ID="cntMain" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlCheckList">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table runat="server" ID="tblButton">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul id="pnlEntry" runat="server" style="display: none">
                                    <li id="pnlSave" runat="server">
                                        <asp:Button ID="btnSave" runat="server" CommandName="ITEMSAVE" Text="<%$ resources:Controls,Save %>"
                                            OnClientClick="return ValidateItem();" ValidationGroup="ChecklistItems" SkinID="btnInner-Save"
                                            TabIndex="12" CommandArgument="SEC_ActionPanel" ToolTip="<%$ resources:Controls,Save %>"
                                            OnClick="ActionHandler" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                            TabIndex="13" EnableViewState="False" OnClick="ActionHandler" CommandName="CANCEL"
                                            ToolTip="<%$Resources:Controls,Cancel%>" />
                                    </li>
                                </ul>
                                <ul id="pnlListing" runat="server" style="display: none">
                                    <li>
                                        <asp:Button ID="btnNew" runat="server" CommandName="NEW" Text="<%$ resources:Controls,New %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-New" ToolTip="<%$ resources:Controls,New %>"
                                            OnClick="ActionHandler" TabIndex="7" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <div id="grdTable-wrap">
                    <div id="Group" runat="server">
                        <table class="table-devide">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <label for="ddlGroup">
                                            <%=Resources.Controls.Group%>
                                        </label>
                                        <asp:DropDownList ID="ddlGroup" runat="server" CssClass="select-medium" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                        </asp:DropDownList>
                                        <asp:ImageButton ID="btnAddGroup" runat="server" SkinID="imbaddnew" ToolTip='<%=Resources.Messages.AddTemplateGroup%>'
                                            CommandName="ADDGROUP" OnClick="ActionHandler" OnClientClick="return GroupIsOpen();"
                                            TabIndex="2" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="clear">
                    </div>
                    <div id="ItemTitle" runat="server">
                        <%--<h3>
                        <%=Resources.Controls.CheckListItemDetails %></h3>--%>
                        <ul id="tab-menu">
                            <li><span id="spnListing" runat="server" class="tab-active">
                                <asp:LinkButton ID="lbnListing" runat="server" Text="<%$ resources:Controls,List %>"
                                    CommandName="CANCEL" CssClass="tab-active" OnClick="ActionHandler" TabIndex="3"
                                    ToolTip="<%$ resources:Controls,List %>" />
                            </span></li>
                            <li><span id="spnDetails" runat="server" class="tab-inactive">
                                <asp:LinkButton ID="lbnDetails" runat="server" Text="<%$ resources:Controls,Details %>"
                                    CommandName="NEW" CssClass="tab-inactive" OnClick="ActionHandler" TabIndex="4"
                                    ToolTip="<%$ resources:Controls,Details %>" />
                            </span></li>
                        </ul>
                    </div>
                    <div class="clear">
                    </div>
                    <div id="addItem" runat="server">
                        <table class="table-devide">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblItemCode" runat="server" Text="<%$ resources:Controls,GenralPropertyCode%>"
                                            AssociatedControlID="txtItemCode" />
                                        <asp:TextBox ID="txtItemCode" runat="server" CssClass="input-half" MaxLength="100" TabIndex="5" />
                                        <asp:Label ID="lblValidItemCode" runat="server" Text="*" CssClass="star" />
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblItemName" runat="server" CssClass="middle-lbl-d" Text="<%$ resources:Controls,GeneralPropertyName%>"
                                            AssociatedControlID="txtItemName" />
                                        <asp:TextBox ID="txtItemName" runat="server" MaxLength="200" CssClass="input-half" TabIndex="6" />
                                        <asp:Label ID="lblValidItemName" runat="server" Text="*" CssClass="star" />
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblControl" runat="server" Text="<%$ resources:Controls,Control%>"
                                            AssociatedControlID="ddlControl" />
                                        <asp:DropDownList ID="ddlControl" runat="server" TabIndex="7" CssClass="select-half-a" onchange="EnableDisableKey()" />
                                        <asp:Label ID="lblValidControl" runat="server" Text="*" CssClass="star" />
                                    </div>
                                    <div class="clear">
                                    </div>
                                </td>
                                <td id="Parameterrow" runat="server">
                                    <div class="div2col-S">
                                        <asp:Label ID="lblParameter" runat="server" CssClass="middle-lbl-d" Text="<%$ resources:Controls,Key%>" AssociatedControlID="ddlParameter" />
                                        <asp:DropDownList ID="ddlParameter" runat="server" TabIndex="8" CssClass="select-half-a" />
                                        <asp:Label ID="lblValidParameter" runat="server" Text="*" CssClass="star" />
                                    </div>
                                    <div class="clear">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblItemSequence" runat="server" Text="<%$ resources:Controls,Sequence%>"
                                            AssociatedControlID="txtItemSequence" />
                                        <asp:TextBox ID="txtItemSequence" runat="server" MaxLength="2" TabIndex="9" CssClass="input-w8per numeric"
                                            onkeypress="return isNumberKey(event)" />
                                        <asp:Label ID="lblValidItemSequence" runat="server" Text="*" CssClass="star" />
                                        <asp:Label runat="server" ID="lbItemActive" CssClass="lbl-49perc" Text="<%$ Resources:Active %>" AssociatedControlID="chkItemActive"></asp:Label>
                                        <asp:CheckBox ID="chkItemActive" runat="server" />
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="divcol-S">
                                        <asp:Label ID="lblItemDescription" runat="server" Text="<%$ resources:Controls,Description%>"
                                            AssociatedControlID="txtItemDesc" /><asp:TextBox ID="txtItemDesc" runat="server"
                                                TextMode="MultiLine" MaxLength="500" TabIndex="10" CssClass="multiline-3line"
                                                onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" />
                                    </div>
                                    <div class="clear">
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                            <asp:TableRow ID="ModifiedDatePnl" runat="server" CssClass="last-modified" Visible="false">
                                <asp:TableCell>
                                    <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </div>
                    <div class="clear">
                    </div>
                    <div id="itemlist" runat="server">
                        <div class="gridwrap">
                            <asp:GridView runat="server" ID="grdItemMst" Width="100%" PageSize="<%$ resources:PageSize %>"
                                AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                OnSorting="ActionHandler">
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ resources:Controls,GenralPropertyCode%>" SortExpression="<%$ resources:DataFieldRes,CHICode %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCode" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.CHICode)) %>'
                                                Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.CHICode)),23) %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Controls,GeneralPropertyName%>" SortExpression="<%$ resources:DataFieldRes,CHIName %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.CHIName)) %>'
                                                Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.CHIName)),52) %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="30%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Controls,Description%>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.CHIDescription)) %>'
                                                Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.CHIDescription)),62) %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="36%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Controls,Control%>" SortExpression="<%$ resources:DataFieldRes,CTLName %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblControlName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.CTLName)) %>'
                                                Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.CTLName)),50) %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="6%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Controls,Sequence%>" SortExpression="<%$ resources:DataFieldRes,CHI_Sequence %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGSequence" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.CHI_Sequence) %>'
                                                Text='<%# Eval(Resources.DataFieldRes.CHI_Sequence)%>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="4%" HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:ErpRes,Status  %>" SortExpression="<%$ resources:DataFieldRes, CHI_ACTIVE %>">
                                        <ItemTemplate>
                                            <image id="imgStatus" title='<%# (Eval(Resources.DataFieldRes.CHI_ACTIVE)).ToString()=="1"? Resources.ErpRes.Active :Resources.ErpRes.InActive %>'
                                                class='<%# (Eval(Resources.DataFieldRes.CHI_ACTIVE)).ToString()=="1"?"active" :"inactive"%>'
                                                alt=""></image>
                                        </ItemTemplate>
                                        <ItemStyle Width="4%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" OnClick="ActionHandler"
                                                CommandName="ITEMGRIDEDIT" ToolTip="<%$ resources:Controls,Edit  %>" CommandArgument="<%# Eval(Resources.DataFieldRes.CHIPK) %>"
                                                TabIndex="8" />
                                            <asp:ImageButton runat="server" ID="imbRemove" SkinID="imbdeletegrid" OnClick="ActionHandler"
                                                CommandName="ITEMGRIDDELETE" ToolTip="<%$ resources:Captions, Remove %>" CommandArgument="<%# Eval(Resources.DataFieldRes.CHIPK) %>"
                                                OnClientClick="return ShowDeleteConfirm(this);" TabIndex="9" />
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <uc1:PagerControl ID="uclPaging" runat="server" />
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <div id="dialog-Template" style="width: 550px; height: 250px; display: none" title="Create Group">
                        <div class="content-wrapper">
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblGroupCode" runat="server" Text="<%$ resources:Controls,GenralPropertyCode%>"
                                                AssociatedControlID="txtGroupCode" />
                                            <asp:TextBox ID="txtGroupCode" runat="server" CssClass="input-halfsmall-a" MaxLength="100" TabIndex="14" />
                                            <asp:Label ID="lblValidGroupCode" runat="server" Text="*" CssClass="star" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblGroupName" runat="server" Text="<%$ resources:Controls,GeneralPropertyName%>"
                                                AssociatedControlID="txtGroupName" />
                                            <asp:TextBox ID="txtGroupName" runat="server" CssClass="w60perc" MaxLength="200" TabIndex="15" />
                                            <asp:Label ID="lblValidGroupName" runat="server" Text="*" CssClass="star" />
                                            <asp:ImageButton ID="imbSaveGroup" runat="server" CommandName="GROUPSAVE" SkinID="imbaddnew"
                                                OnClientClick="return ValidateGroup()" ValidationGroup="ChecklistGroup" OnClick="ActionHandler"
                                                TabIndex="18" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblGroupSequence" runat="server" Text="<%$ resources:Controls,Sequence%>"
                                                AssociatedControlID="txtGroupSequence" />
                                            <asp:TextBox ID="txtGroupSequence" runat="server" MaxLength="2" TabIndex="16" CssClass="small numeric"
                                                onkeypress="return isNumberKey(event)" />
                                            <asp:Label ID="lblValidGroupSequence" runat="server" Text="*" CssClass="star" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblGroupDesc" runat="server" Text="<%$ resources:Controls,Description%>"
                                                AssociatedControlID="txtGroupDesc" /><asp:TextBox ID="txtGroupDesc" runat="server"
                                                    TextMode="MultiLine" MaxLength="500" TabIndex="17" CssClass="multiline-2col"
                                                    onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" />
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdGroupsMst" Width="100%" AllowSorting="True" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable" OnSorting="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,GenralPropertyCode%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGCode" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.CGMCode)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.CGMCode)),21) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,GeneralPropertyName%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.CGMName)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.CGMName)),31) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="26%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Description%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGDescription" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.CGMDecription)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.CGMDecription)),50) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="40%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Sequence%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGSequence" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.CGMSequence) %>'
                                                    Text='<%# Eval(Resources.DataFieldRes.CGMSequence)%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" OnClick="ActionHandler"
                                                    CommandName="GROUPGRIDEDIT" ToolTip="<%$ resources:Controls,Edit  %>" CommandArgument="<%# Eval(Resources.DataFieldRes.CGMPK) %>"
                                                    TabIndex="19" />
                                                <asp:ImageButton runat="server" ID="imbRemove" SkinID="imbdeletegrid" OnClick="ActionHandler"
                                                    CommandName="GROUPGRIDDELETE" ToolTip="<%$ resources:Captions, Remove %>" CommandArgument="<%# Eval(Resources.DataFieldRes.CGMPK) %>"
                                                    OnClientClick="return ShowDeleteConfirm(this);" TabIndex="20" />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="diverrorGroup" style="display: none">
                <%--Use this label to bind the server errors--%>
                <asp:Label runat="server" ID="litGroupErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="ChecklistGroup" runat="server" />
            </div>
            <div id="diverrorItem" style="display: none">
                <%--Use this label to bind the server errors--%>
                <asp:Label runat="server" ID="litItemErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage1" ValidationGroup="ChecklistItem" runat="server" />
            </div>
            </div>
            <asp:HiddenField ID="hdnGrptype" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
