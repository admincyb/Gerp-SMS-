<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GtiFolderExplorer.ascx.cs"
    Inherits="HRMS.UserControls.GtiFolderExplorer" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div id="gtiFolderExplorerContainerDiv" style="min-width: 330px;">
            <table>
                <tr>
                    <td>
                        <div>
                            <span id="folderHeader" runat="server" visible="false" class="folder-header style-none">
                                <asp:ImageButton runat="server" ID="imbBrowseFolder" CommandName="BROWSE" OnClick="ActionHandler"
                                    SkinID="file-browse" Visible="false" Style="margin-right: 0px!important; margin-bottom: 0px!important;" />
                                <span id="spnSelectedFolder" runat="server" style="vertical-align: sub;">
                                    <%# GetLocalResourceObject("rootFolder").ToString()%></span> </span>
                            <div id="gtiFolderExplorerBody" visible="false">
                                <span class="folder-create" runat="server" id="commandBar" visible="false">
                                    <asp:ImageButton runat="server" ID="imbAddNewFolder" SkinID="imbaddnew" OnClick="ActionHandler"
                                        CssClass="margnbotm0 margntop2" CommandName="ADDITEM" />
                                    <asp:ImageButton runat="server" ID="imbEditFolder" SkinID="imbeditgrid" OnClick="ActionHandler"
                                        CssClass="margnbotm0 margntop2" CommandName="EDIT" ToolTip="<%$resources:Controls,Edit %>" />
                                    <asp:ImageButton runat="server" ID="imbDeleteFolder" SkinID="imbdeletegrid" OnClick="ActionHandler"
                                        CssClass="margnbotm0 margntop2" CommandName="DELETE" ToolTip="<%$resources:Controls,Delete %>" />
                                    <asp:ImageButton runat="server" ID="imbDeleteDisable" SkinID="imbdeletegriddisable"
                                        CssClass="margnbotm0 margntop2" Enabled="false" ToolTip="<%$resources:Controls,Delete %>" />
                                    <asp:Label Text="<%$ resources:SelectedFolder %>" runat="server" ID="lblFolderName"
                                        AssociatedControlID="txtFolderName" />
                                    <asp:TextBox ID="txtFolderName" runat="server" CssClass="margnbotm0 input-w45per margn-rgt0" />
                                    <asp:HiddenField ID="hdfSelectedNodeValue" runat="server" />
                                    <asp:RequiredFieldValidator ID="vrfFolderName" CssClass="star " SetFocusOnError="true"
                                        ValidationGroup="save" EnableClientScript="true" runat="server" ControlToValidate="txtFolderName"
                                        Display="Static" Text="*">
                                    </asp:RequiredFieldValidator>
                                    <asp:ImageButton runat="server" ID="imbSaveFolder" SkinID="save" OnClick="ActionHandler"
                                        Enabled="false" CssClass="margnbotm0 margntop2" CommandName="SAVE" ToolTip="<%$resources:Controls,Save %>"
                                        ValidationGroup="save" />
                                    <asp:ImageButton runat="server" ID="imbUserMapping" SkinID="user-map" OnClick="ActionHandler"
                                        CssClass="margnbotm0 margntop2 margn-rgt0" CommandName="MAPPING" ToolTip="<%$resources:Controls,UserMapping %>" />
                                    <asp:ImageButton runat="server" ID="imbUserDisable" SkinID="user-map-disable" Enabled="false"
                                        CssClass="margnbotm0 margntop2 margn-rgt0" ToolTip="<%$resources:Controls,UserMapping %>" />
                                </span>
                                <div style="display: none;">
                                    <asp:ImageButton runat="server" ID="imbSelect" SkinID="save" OnClick="ActionHandler"
                                        CssClass="margnbotm0" CommandName="CANCEL" />
                                </div>
                                <div>
                                    <div id="folderBrowserBody" runat="server" style="float: left">
                                        <div id="edittree" class="treeview maxh-400 treeview-center maxw-500 minw-500">
                                            <asp:TreeView ID="folderExplorerTreeView" runat="server" ShowLines="true" SelectedNodeStyle-CssClass="tree-selected"
                                                OnSelectedNodeChanged="ActionHandler" PopulateNodesFromClient="false" ExpandDepth="0">
                                                <SelectedNodeStyle ImageUrl="~/Images/ClassicEdoc/Icons/folder_opend.png" />
                                                <ParentNodeStyle ImageUrl="~/Images/ClassicEdoc/Icons/folder_closed.png" />
                                                <LeafNodeStyle ImageUrl="~/Images/ClassicEdoc/Icons/folder_closed.png" />
                                                <RootNodeStyle ImageUrl="~/Images/ClassicEdoc/Icons/tree-root.png" />
                                            </asp:TreeView>
                                        </div>
                                        <div class="margntop2 margn-rgt0 " style="float: right;">
                                            <asp:Button Text="<%$resources:btnOk%>" runat="server" ID="btnOk" OnClick="ActionHandler"
                                                CommandName="SELECT" Style="margin-bottom: 0px!important; margin-right: 0px!important;"
                                                SkinID="btnInner-ok-popup" />
                                        </div>
                                    </div>
                                    <div runat="server" id="userMappingColumn" visible="false" style="float: left">
                                        <div class="gridwrap usermapping margnbotm0">
                                            <asp:GridView runat="server" ID="grdMappingUserList" AutoGenerateColumns="false"
                                                EmptyDataRowStyle-CssClass="emptytable">
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:CheckBox Text="" runat="server" Checked='<%# Eval(Resources.DataFieldRes.IsMapped).ToString() == "1" %>'
                                                                ID="chkSelect" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:UserNameGrid %>">
                                                        <ItemTemplate>
                                                            <asp:HiddenField runat="server" ID="hdfMappingPK" Value='<%# Eval(Resources.DataFieldRes.MappingPK) == null ? "0": Eval(Resources.DataFieldRes.MappingPK).ToString() %>' />
                                                            <asp:HiddenField runat="server" ID="hdfMappingUserPk" Value='<%# Eval(Resources.DataFieldRes.MappingUserPK) %>' />
                                                            <asp:Label ID="lblUserName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.MapingUserName).ToString()),25) %>'
                                                                ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.MapingUserName).ToString())%>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="97%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <div class="margntop2 margn-rgt0 " style="float: right;">
                                            <asp:Button Text="<%$resources:btnAssign%>" runat="server" ID="btnAssaign" OnClick="ActionHandler"
                                                CommandName="ASSAIGN" Style="margin-bottom: 0px!important;" SkinID="btnInner-ok-popup" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div style="display: none;">
            <asp:Button runat="server" ID="btnFolderBrowserEventInvoker" OnClick="ActionHandler"
                CommandName="FOLDERCREATED" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
