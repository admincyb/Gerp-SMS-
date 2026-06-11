<%@ Page Title="<%$ Resources:Captions,Title_StoreMaterialMapping %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" Theme="ClassicExt" EnableEventValidation="false"
    CodeBehind="StoreMaterialMapping.aspx.cs" Inherits="ERPSMS_v01.Administration.Masters.StoreMaterialMapping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/Administration/Masters/StoreMaterialMapping.js.axd"
        type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="fixed-buttons-normal">
        <div class="Button-container">
            <asp:Table runat="server" ID="tblButton">
                <asp:TableRow>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul class="bredcrum">
                            <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                        </ul>
                        <ul runat="server" id="pnlListing">
                            <li>
                                <asp:Button ID="btnAdd" runat="server" SkinID="btnInner-New" Text="<%$Resources:Controls,Add%>"
                                    OnClientClick="javascript:return AddNew();" TabIndex="10" EnableViewState="false" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    TabIndex="11" EnableViewState="False" OnClientClick="javascript:return SavePage();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnReset" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Reset%>"
                                    TabIndex="12" EnableViewState="False" OnClientClick="javascript:return ResetPage();" />
                            </li>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" OnClientClick="javascript:return CancelFun();" TabIndex="13" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <div id="grdTable-wrap">
            <div id="divListing">
                <div id="searchwrap" class="search-wrap-custom1">
                    <div id="divSearch">
                        <span>
                            <%=Resources.Controls.SearchBy%></span>
                        <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" TabIndex="1"
                            onchange="javascript:SetSearchType();" EnableViewState="false">
                            <%-- <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                            </asp:ListItem>--%>
                            <asp:ListItem Value="DPT_NAME" Text="<%$ Resources:BindValues, Store%>">
                            </asp:ListItem>
                            <%--<asp:ListItem Value="" Text="<%$ Resources:BindValues, MaterialName%>">
                        </asp:ListItem>--%>
                        </asp:DropDownList>
                        <%-- <div class="clear">
                        </div>--%>
                        <div id="divSearchDtls">
                            <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false" Width="310px" TabIndex="2">
                            </asp:TextBox>
                        </div>
                        <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" TabIndex="5" OnClientClick="javascript:return BindGrid();"
                            EnableViewState="false" />
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div class="grdTable">
                    <table rules="all" id="grdStoreList" grandtype="GrandGrid" pagesize="20" paging="true"
                        width="100%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th fieldmap="IDM_DEPT" isvisible="false">
                                </th>
                                <th fieldmap="DPT_NAME" sortable="true" width="92%" align="left">
                                    <%=Resources.Controls.Store%>
                                </th>
                                <%--<th fieldmap="" sortable="true" width="20%" align="left">
                                <%=Resources.Controls.Material%>
                            </th>--%>
                                <th type="Template" width="4%" align="left">
                                    <%=Resources.Controls.Action%>
                                </th>
                                <th type="Template" width="4%" align="left">
                                    <asp:ImageButton runat="server" ID="imbEditMast" SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')"
                                        EnableViewState="false" />
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
            </div>
            <div id="divData">
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <label for="Store">
                                    <%=Resources.Controls.Store%>
                                    *
                                </label>
                                <asp:DropDownList ID="Store" runat="server" EnableViewState="false" Width="350px"
                                    onchange="javascript:FillMenuTreeView();" TabIndex="2">
                                </asp:DropDownList>
                                <div class="clear">
                                </div>
                                <label for="ITM_CATEGORY">
                                    <%=Resources.Controls.Category%>
                                    *
                                </label>
                                <asp:DropDownList ID="ITM_CATEGORY" runat="server" EnableViewState="false" Width="350px"
                                    onchange="javascript:FillMenuTreeView();" TabIndex="2">
                                </asp:DropDownList>
                                <div class="clear">
                                </div>
                                <div id="divMaterialType" style="display:none">
                                <label for="IPD_TYPE">
                                    <%=Resources.Controls.Type%>*</label><asp:DropDownList ID="IPD_TYPE" runat="server" CssClass="half" EnableViewState="false" TabIndex="10" onchange="javascript:FillPackingMaterialTree();"  > </asp:DropDownList>
                                      <div class="clear">
                                </div>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div id="treewrap" class="edittree" style="overflow: auto">
                                <div id="trvMaterialMap" class="treeview-adj tree-width">
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="clear">
            </div>
            <asp:HiddenField ID="SBU" runat="server" Value="0" />
            <asp:HiddenField ID="IDM_DEPT" runat="server" Value="0" />
            <asp:HiddenField ID="IDM_MOD_BY" runat="server" Value="0" />
            <div id="divResult">
                <asp:HiddenField ID="STORE_MATERIAL_MAPPING_LIST" runat="server"></asp:HiddenField>
            </div>
        </div>
    </div>
</asp:Content>
