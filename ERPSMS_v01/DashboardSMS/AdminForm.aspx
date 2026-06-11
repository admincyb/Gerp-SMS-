<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS.Master" AutoEventWireup="true"
    Theme="ERP-Blue" CodeBehind="AdminForm.aspx.cs" Inherits="ERPSMS_v01.DashboardSMS.AdminForm" %>

<%@ Register Src="~/UserControls/PagerControl.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript">
        function ClearSetPagePopup() {
            $("[id$=txtPageTitle]").val("");
            $("[id$=txtPageDescription]").val("");
            ShowAddPagePopup(true);
        }
        function ShowAddPagePopup(isNew) {
            if (isNew) {
                $("[id$=btnDeletePage]").hide();
            }
            ShowContainerDiv('#PageDialog', 'Manage Page', '940', '480');
            $("[id$=txtPageTitle]").focus();
            return false;
        }
        function ClearSetRowPopup() {
            $("[id$=txtRowTitle]").val("");
            $("[id$=txtRowDesc]").val("");
            ShowAddRowPopup(true);
        }
        function ShowAddRowPopup(isNew) {
            if (isNew) {
                $("[id$=btnDeleteRow]").hide();
            }
            ShowContainerDiv('#RowDialog', 'Manage Row', '940', '480');
            return false;
        }
        function ClearSetDashletPopup() {
            $("[id$=txtDashletTitle]").val("");
            $("[id$=txtDataSource]").val("");
            ShowAddDashletPopup(true);
        }
        function ShowAddDashletPopup(isNew) {
            if (isNew) {
                $("[id$=btnDeleteDashlet]").hide();
            }
            else {
                $("[id$=btnDeleteDashlet]").show();
            }
            ShowContainerDiv('#DashletDialog', 'Manage Dashlet', '940', '480');
            return false;
        }


        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
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
        function AfterClose(contID) {
            switch (contID) {
                case "#PageDialog":
                    $("[id$=btnCanelPage]").click();
                    break;
                case "#RowDialog":
                    $("[id$=btnCancelRow]").click();
                    break;
                case "#DashletDialog":
                    $("[id$=btnCancelDashlet]").click();
                    break;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlDashboardAdmin" UpdateMode="Always">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnAddPage" CommandName="DSBADDPAGE" TabIndex="1"
                                            Text="<%$ resources:AddNewPage %>" ToolTip="<%$ resources:AddNewPage %>" OnClick="ActionHandler"
                                            OnClientClick="ClearSetPagePopup();return false;" CommandArgument="SEC_ActionPanel" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="gridwrap">
                                <cc1:ExtGridView runat="server" ID="grdPageHierarchy" AutoGenerateColumns="False"
                                    DataKeyNames="<%$resources:DataFieldRes,DsbPagePK %>" ExpandButtonCssClass="GridExpandCollapseButton"
                                    CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                                    CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" AllowPaging="True"
                                    PageSize="<%$ resources:PageSize %>" OnRowCreated="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="Label2" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:PageName %>" SortExpression="<%$ resources:DataFieldRes,DsbPageTitle %>">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lbnSelectPage" Text='<%# gAssets.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.DsbPageTitle),32) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.DsbPageTitle) %>' OnClick="ActionHandler"
                                                    CommandName="DSBSELECTPAGE" TabIndex="2"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="40%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description" SortExpression="<%$ resources:DataFieldRes,DsbPageDesc %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPageDesc" runat="server" Text='<%# gAssets.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.DsbPageDesc),32) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.DsbPageDesc) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="40%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lbnAddRow" Text="<%$ resources:AddNewRow %>" OnClick="ActionHandler"
                                                    ToolTip="<%$ resources:AddNewRow %>" CommandName="DSBADDROW" TabIndex="3"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="97%">
                                            <ItemTemplate>
                                                <div class="gridwrap">
                                                    <cc1:ExtGridView runat="server" ID="grdRowHierarchy" AutoGenerateColumns="False"
                                                        ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                                        GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                                        CellPadding="3" ForeColor="#333333" AllowPaging="false" DataKeyNames="<%$resources:DataFieldRes,DsbRowPK %>"
                                                        OnRowCreated="ActionHandler">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="Label4" runat="server" Text="<%$ resources:gAssetsRes,Msg_EmptyGrid %>"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ resources:RowID %>">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="lbnSelectRow" Text='<%# gAssets.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.DsbRowTitle),30) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.DsbRowTitle) %>' OnClick="ActionHandler"
                                                                        CommandName="DSBSELECTROW" TabIndex="4"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="40%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:RowDescription %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRowDescription" runat="server" Text='<%# gAssets.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.DsbRowDesc),30) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.DsbRowDesc) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="40%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="lbnAddCell" Text="<%$ resources:AddNewCell %>"
                                                                        OnClick="ActionHandler" ToolTip="<%$ resources:AddNewCell %>" CommandName="DSBADDCELL"
                                                                        TabIndex="5"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="20%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ControlStyle-Width="97%">
                                                                <ItemTemplate>
                                                                    <div class="gridwrap">
                                                                        <asp:GridView runat="server" ID="grdDashlet" AutoGenerateColumns="false" DataKeyNames="<%$resources:DataFieldRes,DsbDashletPK %>"
                                                                            Style="margin-left: 30px;" CellPadding="4" ForeColor="#333333" EmptyDataRowStyle-CssClass="emptytable"
                                                                            GridLines="None">
                                                                            <EmptyDataTemplate>
                                                                                <asp:Label ID="Label3" runat="server" Text="<%$ resources:gAssetsRes,Msg_EmptyGrid %>"></asp:Label>
                                                                            </EmptyDataTemplate>
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="<%$ resources:CellID %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton runat="server" ID="lbnSelectCell" Text='<%# gAssets.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.DsbDashletTitle),28) %>'
                                                                                            ToolTip='<%# Eval(Resources.DataFieldRes.DsbDashletTitle) %>' OnClick="ActionHandler"
                                                                                            CommandName="DSBSELECTCELL" TabIndex="6"></asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Width="40%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ resources:ChartType %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblChartType" runat="server" Text='<%# gAssets.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataTableRes.ChartSubTypeMaster +"."+ Resources.DataTableRes.ChartTypeMaster +"."+ Resources.DataFieldRes.ChartTypeName), 24) %>'
                                                                                            ToolTip='<%# Eval(Resources.DataTableRes.ChartSubTypeMaster +"."+ Resources.DataTableRes.ChartTypeMaster +"."+ Resources.DataFieldRes.ChartTypeName) %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Width="30%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ resources:ChartSubType %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblChartSubType" runat="server" Text='<%# gAssets.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataTableRes.ChartSubTypeMaster +"."+ Resources.DataFieldRes.ChtSubTypeName),24) %>'
                                                                                            ToolTip='<%# Eval(Resources.DataTableRes.ChartSubTypeMaster +"."+ Resources.DataFieldRes.ChtSubTypeName) %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Width="30%" />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <%--Third--%>
                                                                            <RowStyle BackColor="#b9e6e6" ForeColor="#333333" />
                                                                            <HeaderStyle BackColor="#ededee" Font-Bold="True" ForeColor="#506c92" Height="25px" />
                                                                        </asp:GridView>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <%--Second--%>
                                                        <RowStyle BackColor="#ffffff" ForeColor="#506c92" />
                                                        <HeaderStyle BackColor="#eef3fb" Font-Bold="True" ForeColor="#506c92" Height="25px" />
                                                    </cc1:ExtGridView>
                                                </div>
                                            </ItemTemplate>
                                            <ControlStyle Width="97%"></ControlStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <%--First--%>
                                    <RowStyle BackColor="#ffffff" ForeColor="#506c92" />
                                    <HeaderStyle BackColor="#dee3ed" Font-Bold="True" ForeColor="#506c92" Height="25px" />
                                </cc1:ExtGridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="PageDialog" title="<asp:Literal runat='server' Text='<%$ resources:PageHeading %>'></asp:Literal>"
                style="display: none">
                <div class="Button-container-popup">
                    <asp:Button runat="server" ID="btnSavePage" CommandName="DSBSAVEPAGE" TabIndex="10"
                        Text="<%$resources:gAssetsRes,Save %>" ToolTip="<%$resources:gAssetsRes,Save %>"
                        OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('vgPage')"
                        ValidationGroup="vgPage" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                    <asp:Button runat="server" ID="btnDeletePage" CommandName="DSBDELETEPAGE" Text="<%$resources:gAssetsRes,Delete %>"
                        ToolTip="<%$resources:gAssetsRes,Delete %>" OnClick="ActionHandler" TabIndex="11"
                        CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" OnClientClick="return ShowDeleteConfirm(this);" />
                    <asp:Button runat="server" ID="btnCanelPage" Text="<%$resources:gAssetsRes,Cancel %>"
                        ToolTip="<%$resources:gAssetsRes,Cancel %>" OnClick="ActionHandler" CommandName="DSBCANCELPAGE"
                        TabIndex="12" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" />
                </div>
                <div class="contentwrapper">
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="lblPageTitle" runat="server" AssociatedControlID="txtPageTitle" Text="<%$ resources:PageTitle %>"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtPageTitle" TabIndex="7" MaxLength="50"></asp:TextBox>
                                    <div class="starwrap">
                                        <asp:RequiredFieldValidator ID="vrfPageTitle" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="vgPage" EnableClientScript="true" runat="server" ControlToValidate="txtPageTitle"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_PageTitle %>">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="lblNoOFRows" runat="server" AssociatedControlID="ddlNoOfRows" Text="<%$ resources:Rows %>"></asp:Label>
                                    <asp:DropDownList runat="server" ID="ddlNoOfRows" TabIndex="8" CssClass="medium">
                                        <asp:ListItem Text="<%$ resources:gAssetsRes, Select %>" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="<%$ resources:Rows1 %>" Value="1"> </asp:ListItem>
                                        <asp:ListItem Text="<%$ resources:Rows2 %>" Value="2"> </asp:ListItem>
                                        <asp:ListItem Text="<%$ resources:Rows3 %>" Value="3"> </asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="vrfNoOfRows" CssClass="star" SetFocusOnError="true"
                                        ValidationGroup="vgPage" EnableClientScript="true" runat="server" ControlToValidate="ddlNoOfRows"
                                        InitialValue="-1" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_Rows %>">
                                    </asp:RequiredFieldValidator>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="divcol-S">
                                    <asp:Label ID="lblPageDescription" runat="server" AssociatedControlID="txtPageDescription"
                                        Text="<%$ resources:Description %>"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtPageDescription" TabIndex="9" TextMode="MultiLine"
                                        onkeydown="limitText(this,100);" onkeyup="limitText(this,100);" CssClass=" multiline-3col"
                                        EnableTheming="false"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revDescription" runat="server" ControlToValidate="txtPageDescription"
                                        ErrorMessage="<%$ Resources:gAssetsRes, Msg_Exceed_MaxLen %>" ValidationExpression="^[\s\S]{0,100}$"
                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="vgPage"></asp:RegularExpressionValidator>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow ID="PageModifiedDatePnl" CssClass="last-modified" runat="server">
                            <asp:TableCell>
                                <asp:Label ID="lblLastModifiedPageHDR" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div id="RowDialog" title="<asp:Literal runat='server' Text='<%$ resources:RowHeading %>'></asp:Literal>"
                style="display: none">
                <div class="Button-container-popup">
                    <asp:Button runat="server" ID="btnSaveRow" CommandName="DSBSAVEROW" TabIndex="16"
                        Text="<%$resources:gAssetsRes,Save %>" ToolTip="<%$resources:gAssetsRes,Save %>"
                        OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('vgRow')" ValidationGroup="vgRow"
                        CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                    <asp:Button runat="server" ID="btnDeleteRow" CommandName="DSBDELETEROW" Text="<%$resources:gAssetsRes,Delete %>"
                        ToolTip="<%$resources:gAssetsRes,Delete %>" OnClick="ActionHandler" TabIndex="17"
                        CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" OnClientClick="return ShowDeleteConfirm(this);" />
                    <asp:Button runat="server" ID="btnCancelRow" Text="<%$resources:gAssetsRes,Cancel %>"
                        ToolTip="<%$resources:gAssetsRes,Cancel %>" OnClick="ActionHandler" CommandName="DSBCANCELROW"
                        TabIndex="18" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" />
                </div>
                <div class="contentwrapper">
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="lblRowPageLabel" runat="server" AssociatedControlID="lblRowPage" Text="<%$ resources:PageTitle %>"></asp:Label>
                                    <asp:Label ID="lblRowPage" runat="server" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="lblRowLayout" runat="server" AssociatedControlID="ddlRowLayout" Text="<%$ resources:Layout %>"></asp:Label>
                                    <asp:DropDownList runat="server" ID="ddlRowLayout" TabIndex="14" CssClass="medium">
                                        <asp:ListItem Text="<%$ resources:gAssetsRes, Select %>" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="<%$ resources:OnetoTwo %>" Value="1"> </asp:ListItem>
                                        <asp:ListItem Text="<%$ resources:TwotoOne %>" Value="2"> </asp:ListItem>
                                        <asp:ListItem Text="<%$ resources:OnetoOne %>" Value="3"> </asp:ListItem>
                                        <asp:ListItem Text="<%$ resources:Singe %>" Value="4"> </asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="vrfRowLayout" CssClass="star" SetFocusOnError="true"
                                        ValidationGroup="vgRow" EnableClientScript="true" runat="server" ControlToValidate="ddlRowLayout"
                                        InitialValue="-1" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_Layout %>">
                                    </asp:RequiredFieldValidator>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="lblRowTitle" runat="server" AssociatedControlID="txtRowTitle" Text="<%$ resources:RowTitle %>"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtRowTitle" TabIndex="13" MaxLength="50"></asp:TextBox>
                                    <div class="starwrap">
                                        <asp:RequiredFieldValidator ID="vrfRowTitle" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="vgRow" EnableClientScript="true" runat="server" ControlToValidate="txtRowTitle"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_RowTitle %>">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="divcol-S">
                                    <asp:Label ID="lblRowDesc" runat="server" AssociatedControlID="txtRowDesc" Text="<%$ resources:Description %>"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtRowDesc" TabIndex="15" TextMode="MultiLine" onkeydown="limitText(this,100);"
                                        onkeyup="limitText(this,100);" CssClass=" multiline-3col" EnableTheming="false"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revRowDesc" runat="server" ControlToValidate="txtRowDesc"
                                        ErrorMessage="<%$ Resources:gAssetsRes, Msg_Exceed_MaxLen %>" ValidationExpression="^[\s\S]{0,100}$"
                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="vgRow"></asp:RegularExpressionValidator>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <asp:Table ID="Table2" runat="server">
                        <asp:TableRow ID="RowModifiedDatePnl" CssClass="last-modified" runat="server">
                            <asp:TableCell>
                                <asp:Label ID="lblLastModifiedRowHDR" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div id="DashletDialog" title="<asp:Literal runat='server' Text='<%$ resources:DashletHeading %>'></asp:Literal>"
                style="display: none">
                <div class="contentwrapper">
                    <asp:Table runat="server" ID="tblDashlet" CssClass="asptbllinks">
                        <asp:TableRow ID="trDashletTabs" CssClass="popup-btns-co" runat="server">
                            <asp:TableCell>
                                <div class="tab-relativer">
                                    <ul>
                                        <li style="text-align: left">
                                            <asp:LinkButton runat="server" ID="lbnBasicDetails" Text="<%$ resources:BasicDetails %>"
                                                ToolTip="<%$ resources:BasicDetails %>" CssClass="tab-active" OnClick="ActionHandler"
                                                CommandName="DSBCELLBASIC" TabIndex="43"></asp:LinkButton>
                                            <asp:LinkButton runat="server" ID="lbnChartProps" Text="<%$ resources:ChartProperties %>"
                                                ToolTip="<%$ resources:ChartProperties %>" CssClass="tab-inactive" OnClick="ActionHandler"
                                                CommandName="DSBCELLPROPERTIES" TabIndex="44"></asp:LinkButton>
                                            <asp:LinkButton runat="server" ID="lbnFilterParams" Text="<%$ resources:FilterParameters %>"
                                                ToolTip="<%$ resources:FilterParameters %>" CssClass="tab-inactive" OnClick="ActionHandler"
                                                CommandName="DSBCELLFILTERPARAMS" TabIndex="45"></asp:LinkButton>
                                        </li>
                                    </ul>
                                </div>
                            </asp:TableCell>
                            <asp:TableCell>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="trCellBasicDetails" runat="server">
                            <asp:TableCell>
                                <div class="Button-container-popup">
                                    <asp:Button runat="server" ID="btnSaveDashlet" CommandName="DSBSAVECELL" TabIndex="27"
                                        Text="<%$resources:gAssetsRes,Save %>" ToolTip="<%$resources:gAssetsRes,Save %>"
                                        OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('vgDashlet')"
                                        ValidationGroup="vgDashlet" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    <asp:Button runat="server" ID="btnDeleteDashlet" CommandName="DSBDELETECELL" Text="<%$resources:gAssetsRes,Delete %>"
                                        ToolTip="<%$resources:gAssetsRes,Delete %>" OnClick="ActionHandler" TabIndex="28"
                                        CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" OnClientClick="return ShowDeleteConfirm(this);" />
                                    <asp:Button runat="server" ID="btnCancelDashlet" Text="<%$resources:gAssetsRes,Cancel %>"
                                        ToolTip="<%$resources:gAssetsRes,Cancel %>" OnClick="ActionHandler" CommandName="DSBCANCELCELL"
                                        TabIndex="29" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" />
                                </div>
                                <div class="content-wrapper">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label ID="lblCellPageLabel" runat="server" AssociatedControlID="lblCellPage"
                                                        Text="<%$ resources:PageTitle %>"></asp:Label>
                                                    <asp:Label ID="lblCellPage" runat="server" Font-Bold="true"></asp:Label>
                                                    <asp:Label ID="lblDashletTitle" runat="server" AssociatedControlID="txtDashletTitle"
                                                        Text="<%$ resources:DashletTitle %>"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtDashletTitle" TabIndex="19" MaxLength="50"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfDashletTitle" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="vgDashlet" EnableClientScript="true" runat="server" ControlToValidate="txtDashletTitle"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_DashletTitle %>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:Label ID="lblDataSource" runat="server" AssociatedControlID="txtDataSource"
                                                        Text="<%$ resources:DataSource %>"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtDataSource" TabIndex="21" MaxLength="500"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfDataSource" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="vgDashlet" EnableClientScript="true" runat="server" ControlToValidate="txtDataSource"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_DataSource %>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:Label ID="lblChartType" runat="server" AssociatedControlID="ddlChartType" Text="<%$ resources:ChartType %>"></asp:Label>
                                                    <asp:DropDownList runat="server" ID="ddlChartType" TabIndex="23" CssClass="medium"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="vrfChartType" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="vgDashlet" EnableClientScript="true" runat="server" ControlToValidate="ddlChartType"
                                                        InitialValue="-1" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_ChartType %>">
                                                    </asp:RequiredFieldValidator>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblNoOfSeries" runat="server" AssociatedControlID="ddlNoOfSeries"
                                                        Text="<%$ resources:Series %>"></asp:Label>
                                                    <asp:DropDownList runat="server" ID="ddlNoOfSeries" TabIndex="25" CssClass="medium">
                                                        <asp:ListItem Text="<%$ resources:gAssetsRes, Select %>" Value="-1"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ resources:Series1 %>" Value="1"> </asp:ListItem>
                                                        <asp:ListItem Text="<%$ resources:Series2 %>" Value="2"> </asp:ListItem>
                                                        <asp:ListItem Text="<%$ resources:Series3 %>" Value="3"> </asp:ListItem>
                                                        <asp:ListItem Text="<%$ resources:Series4 %>" Value="4"> </asp:ListItem>
                                                        <asp:ListItem Text="<%$ resources:Series5 %>" Value="5"> </asp:ListItem>
                                                        <asp:ListItem Text="<%$ resources:SeriesD %>" Value="6"> </asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="vrfNoOfSeries" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="vgDashlet" EnableClientScript="true" runat="server" ControlToValidate="ddlNoOfSeries"
                                                        InitialValue="-1" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_series %>">
                                                    </asp:RequiredFieldValidator>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label ID="lblCellRowLabel" runat="server" AssociatedControlID="lblCellRow" Text="<%$ resources:RowTitle %>"></asp:Label>
                                                    <asp:Label ID="lblCellRow" runat="server" Font-Bold="true"></asp:Label>
                                                    <asp:Label ID="lblUseService" runat="server" AssociatedControlID="chkUseService"
                                                        Text="<%$ resources:UseService %>"></asp:Label>
                                                    <asp:CheckBox ID="chkUseService" runat="server" Checked="true" TabIndex="20" />
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblServiceMethod" runat="server" AssociatedControlID="txtServiceMethod"
                                                        Text="<%$ resources:ServiceMethod %>"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtServiceMethod" TabIndex="22" MaxLength="500"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfServiceMethod" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="vgDashlet" EnableClientScript="true" runat="server" ControlToValidate="txtServiceMethod"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_ServiceMethod %>">
                                                    </asp:RequiredFieldValidator>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblChartSubType" runat="server" AssociatedControlID="ddlChartSubType"
                                                        Text="<%$ resources:ChartSubType %>"></asp:Label>
                                                    <asp:DropDownList runat="server" ID="ddlChartSubType" TabIndex="24" CssClass="medium">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="vrfChartSubType" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="vgDashlet" EnableClientScript="true" runat="server" ControlToValidate="ddlChartSubType"
                                                        InitialValue="-1" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_ChartSubType %>">
                                                    </asp:RequiredFieldValidator>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblReportUrl" runat="server" AssociatedControlID="txtReportUrl" Text="<%$resources:ReportUrl %>"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtReportUrl" TabIndex="26"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfReportUrl" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="vgDashlet" EnableClientScript="true" runat="server" ControlToValidate="txtReportUrl"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_ReportUrl %>">
                                                    </asp:RequiredFieldValidator>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:Table ID="Table3" runat="server">
                                    <asp:TableRow ID="DashletModifiedDatePnl" CssClass="last-modified" runat="server">
                                        <asp:TableCell>
                                            <asp:Label ID="lblLastModifiedDashletHDR" runat="server"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="trCellProps" runat="server">
                            <asp:TableCell>
                                <div class="Button-container-popup">
                                    <asp:Button runat="server" ID="btnSaveCellProps" CommandName="DSBSAVECELLPROPS" TabIndex="30"
                                        Text="<%$resources:gAssetsRes,Save %>" ToolTip="<%$resources:gAssetsRes,Save %>"
                                        OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    <asp:Button runat="server" ID="btnCancelCellProps" Text="<%$resources:gAssetsRes,Cancel %>"
                                        ToolTip="<%$resources:gAssetsRes,Cancel %>" OnClick="ActionHandler" CommandName="DSBCANCELCELLPROPS"
                                        TabIndex="31" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" />
                                </div>
                                <div class="gridwrap">
                                    <asp:GridView runat="server" ID="grdDashletProperties" Width="100%" AutoGenerateColumns="false"
                                        DataKeyNames="<%$resources:DataFieldRes,DashletPropertyPK %>" EmptyDataRowStyle-CssClass="emptytable">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="Label1" runat="server" Text="<%$ resources:gAssetsRes,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:PropertyName %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPropertyName" runat="server" Text='<%# gAssets.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.DashletPropertyName),28) %>'
                                                        ToolTip='<%# Eval(Resources.DataFieldRes.DashletPropertyName) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="35%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:PropertyDesc %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPropertyDesc" runat="server" Text='<%# gAssets.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.DashletPropertyDesc),28) %>'
                                                        ToolTip='<%# Eval(Resources.DataFieldRes.DashletPropertyDesc) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="35%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:PropertyValue %>">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtPropertyValue" TabIndex="32" MaxLength="100" Text='<%# GetPropertyValue(Eval(Resources.DataTableRes.DashletPropertyMaster)) %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="30%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <asp:Table ID="Table4" runat="server">
                                    <asp:TableRow ID="PropertyModifiedDatePanel" CssClass="last-modified" runat="server">
                                        <asp:TableCell>
                                            <asp:Label ID="lblLastModifiedPropertyHDR" runat="server"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="trCellFilter" runat="server">
                            <asp:TableCell>
                                <div class="Button-container-popup">
                                    <asp:Button runat="server" ID="btnSaveParams" OnClientClick="javascript:ValidatePageNow('vgParameters')"
                                        CommandName="DSBADDPARAMS" Text="<%$resources:gAssetsRes,Save %>" ToolTip="<%$resources:gAssetsRes,Save %>"
                                        OnClick="ActionHandler" TabIndex="39" CommandArgument="SEC_ActionPanel" ValidationGroup="vgParameters"
                                        SkinID="btnInner-Save" />
                                    <asp:Button runat="server" ID="btnDeleteParams" CommandName="DSBDELETEPARAMS" Text="<%$resources:gAssetsRes,Delete %>"
                                        ToolTip="<%$resources:gAssetsRes,Delete %>" OnClick="ActionHandler" TabIndex="40"
                                        CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" OnClientClick="return ShowDeleteConfirm(this);" />
                                    <asp:Button runat="server" ID="btnCancelParameter" Text="<%$resources:gAssetsRes,Cancel %>"
                                        ToolTip="<%$resources:gAssetsRes,Cancel %>" OnClick="ActionHandler" CommandName="DSBCANCELPARAMS"
                                        TabIndex="41" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" />
                                </div>
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblParmeterName" runat="server" AssociatedControlID="txtParameterName"
                                                    Text="<%$ resources:ParameterName %>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtParameterName" TabIndex="33" MaxLength="50"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfParamatername" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="vgParameters" EnableClientScript="true" runat="server" ControlToValidate="txtParameterName"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_ParameterName %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:Label ID="lblcUseService" runat="server" AssociatedControlID="chkParameterUseService"
                                                    Text="<%$ resources:UseService %>"></asp:Label>
                                                <asp:CheckBox ID="chkParameterUseService" runat="server" Checked="true" TabIndex="35" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblParameterLabel" runat="server" AssociatedControlID="txtParameterLabel"
                                                    Text="<%$ resources:Label %>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtParameterLabel" TabIndex="34" MaxLength="50"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblParameterDatasource" runat="server" AssociatedControlID="txtParameterDatasource"
                                                    Text="<%$ resources:DataSource %>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtParameterDatasource" TabIndex="36" MaxLength="500"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfParameterDatasource" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="vgParameters" EnableClientScript="true" runat="server" ControlToValidate="txtParameterDatasource"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_DataSource %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <%--<asp:RequiredFieldValidator ID="vrfParameterLabel" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="vgParameters" EnableClientScript="true" runat="server" ControlToValidate="txtParameterLabel"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_Label %>">
                                                </asp:RequiredFieldValidator>--%>
                                                <asp:Label ID="lblParameterMethod" runat="server" AssociatedControlID="txtParameterMethod"
                                                    Text="<%$ resources:ServiceMethod %>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtParameterMethod" TabIndex="37" MaxLength="500"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfParameterMethod" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="vgParameters" EnableClientScript="true" runat="server" ControlToValidate="txtParameterMethod"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_ServiceMethod %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label ID="lblParameterDescription" runat="server" AssociatedControlID="txtParameterDescription"
                                                    Text="<%$ resources:Description %>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtParameterDescription" TabIndex="38" TextMode="MultiLine"
                                                    onkeydown="limitText(this,100);" onkeyup="limitText(this,100);" CssClass=" multiline-3col"
                                                    EnableTheming="false"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="vreParameterDescription" runat="server" ControlToValidate="txtParameterDescription"
                                                    ErrorMessage="<%$ Resources:gAssetsRes, Msg_Exceed_MaxLen %>" ValidationExpression="^[\s\S]{0,100}$"
                                                    Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="vgParameters"></asp:RegularExpressionValidator>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="gridwrap">
                                    <asp:GridView runat="server" ID="grdParameter" Width="100%" AutoGenerateColumns="false"
                                        DataKeyNames="<%$ resources:DataFieldRes,DashletParameterPK %>" EmptyDataRowStyle-CssClass="emptytable">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="Label1" runat="server" Text="<%$ resources:gAssetsRes,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:ParameterName %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblParameterName" runat="server" Text='<%# gAssets.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.DashletParameterName),16) %>'
                                                        ToolTip='<%# Eval("ParamName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="25%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Label %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblParameterLabel" runat="server" Text='<%# gAssets.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.DashletParameterLabel),16) %>'
                                                        ToolTip='<%# Eval("ParamLabel") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:DataSource %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblParameterDatasource" runat="server" Text='<%# gAssets.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.DashletParameterService),20) %>'
                                                        ToolTip='<%# Eval("ParamDatasource") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="25%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Description %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDescription" runat="server" Text='<%# gAssets.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.DashletParameterDesc),20) %>'
                                                        ToolTip='<%# Eval("ParamDescription") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="25%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imbSelect" runat="server" SkinID="edit-row" CommandName="DSBSELECTPARAMS"
                                                        ToolTip="<%$ resources:Select %>" TabIndex="46" OnClick="ActionHandler" />
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="vgPage" runat="server" />
                <asp:ValidationSummary ID="vsRow" ValidationGroup="vgRow" runat="server" />
                <asp:ValidationSummary ID="vsDashlet" ValidationGroup="vgDashlet" runat="server" />
                <asp:ValidationSummary ID="vsParameters" ValidationGroup="vgParameters" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
