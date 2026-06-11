<%@ Page Title="<%$ Resources:PageTitle %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    EnableEventValidation="false" Theme="ClassicExt" CodeBehind="StoreLocationMaster.aspx.cs"
    Inherits="ERPSMS_v01.Administration.Masters.StoreLocationMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/Administration/Masters/StoreLocationMaster.js.axd"
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
                                    OnClientClick="javascript:return AddNew(1);" TabIndex="10" EnableViewState="false" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    TabIndex="19" EnableViewState="False" OnClientClick="javascript:return SavePage();" />
                            </li>
                            <li>
                                 <asp:Button runat="server"  ID="btnPrintLabel" Text="<%$resources:Controls,PrintLabel %>"   TabIndex="19"
                                     EnableViewState="False"   SkinID="btnInner-IOPrint" ToolTip="<%$resources:Controls,PrintLabel %>" OnClientClick="javascript:return PrintLabel();"/>
                            </li>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" OnClientClick="javascript:return CancelPage();" TabIndex="21" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="clear">
    </div>
    <div class="content-wrapper">
        <div id="grdTable-wrap">
            <div id="divListing">
                <div id="searchwrap" class="search-wrap-custom1">
                    <div id="divSearch">
                        <label for="SearchType">
                            <%=Resources.Controls.SearchBy%></label>
                        <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" TabIndex="1"
                            onchange="javascript:SetSearchType();" EnableViewState="false">
                            <asp:ListItem Value="PARENT_NAME" Text="<%$ Resources:BindValues, Store%>"> 
                            </asp:ListItem>
                            <asp:ListItem Value="DPT_NAME" Text="<%$ Resources:BindValues, LocationName%>">
                            </asp:ListItem>
                            <asp:ListItem Value="DPT_CODE" Text="<%$ Resources:BindValues, LOcationCode%>">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false" TabIndex="2">
                        </asp:TextBox>
                        <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" TabIndex="3" OnClientClick="javascript:return BindGrid();"
                            EnableViewState="false" />
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div class="gridwrap">
                    <table rules="all" id="grdStoreLocation" grandtype="GrandGrid" pagesize="20" paging="true"
                        width="100%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap filter-arrow">
                        <thead>
                            <tr>
                                <th fieldmap="DPT_PK" isvisible="false">
                                </th>
                                <th fieldmap="DPT_DEFAULT" isvisible="false">
                                </th>
                                <th fieldmap="BZU_NAME" sortable="true" align="left" width="13%" isvisible="false">
                                    <%=Resources.Controls.SBU%>
                                </th>
                                <th fieldmap="DPT_PARENT_NAME" sortable="true" align="left" width="20%">
                                    <%=Resources.Controls.Store%>
                                </th>
                                <th fieldmap="DPT_CODE" sortable="true" align="left" width="25%">
                                    <%=Resources.Controls.LocationCode%>
                                </th>
                                <th fieldmap="DPT_NAME" sortable="true" align="left" width="30%">
                                    <%=Resources.Controls.LocationName%>
                                </th>
                                <th fieldmap="IS_ACTIVE" width="5%" sortable="true">
                                    <%=Resources.Controls.Active%>
                                </th>
                                <th type="Template" align="left" width="7%">
                                    <asp:ImageButton runat="server" ID="imbEditMast" SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')"
                                        EnableViewState="false" />
                                    <asp:ImageButton runat="server" ID="imbDeleteMast" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')"
                                        EnableViewState="false" />
                                    <asp:ImageButton runat="server" ID="imbView" SkinID="btnview" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'VIEW')" />
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
            </div>
            <div id="divData">
                <div id="tabs-1">
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <label for="DPT_PARENT">
                                        <%=Resources.Controls.Store%>*</label>
                                    <asp:DropDownList runat="server" ID="DPT_PARENT" TabIndex="5" EnableViewState="False"
                                        CssClass="select-half-a">
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                    <label for="DPT_NAME">
                                        <%=Resources.Controls.LocationName%>
                                        *
                                    </label>
                                    <asp:TextBox runat="server" ID="DPT_NAME" TabIndex="7" MaxLength="95" EnableViewState="False"
                                        CssClass="input-half"></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S" style="float: right; margin-right: 0">
                                    <label for="DPT_CODE">
                                        <%=Resources.Controls.LocationCode%>*</label>
                                    <asp:TextBox runat="server" ID="DPT_CODE" TabIndex="6" MaxLength="95" EnableViewState="False" CssClass="input-half"></asp:TextBox>
                                    <%-- Setting the category of the location as 'Production Location Store' (ADM_CONFIG_MST WHERE CFG_TYPE ='INVENTORY')   --%>
                                    <asp:HiddenField ID="DPT_CATEGORY" runat="server" Value="10" />
                                    <label for="DPT_ACTIVE">
                                        <%=Resources.Controls.Active%></label>
                                    <asp:CheckBox ID="DPT_ACTIVE" runat="server" TabIndex="8" EnableViewState="False" />
                                </div>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="SBU" runat="server" Value="0" />
                    <asp:HiddenField ID="DPT_PK" runat="server" Value="0" />
                    <asp:HiddenField ID="hdfAppType" runat="server" />
                    <asp:HiddenField ID="hdfAppSubType" runat="server" />
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="clear">
            </div>
        </div>
    </div>
</asp:Content>
