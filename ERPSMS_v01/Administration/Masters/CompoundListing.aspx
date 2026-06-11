<%@ Page Title="<%$ Resources:Captions,Title_CompoundMaster %>" Theme="Classic" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="CompoundListing.aspx.cs"
    Inherits="ERPSMS_v01.Administration.Masters.CompoundListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/Administration/Masters/Compounding/CompoundListing.js.axd"
        type="text/javascript" charset="UTF-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--  <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.CompoundingMaster%>
        </h1>
        <div class="button-wrap">
            <asp:ImageButton ID="imbAdd" runat="server" TabIndex="33" EnableViewState="false"
                SkinID="btnadd" PostBackUrl="~/Administration/Masters/CompoundingMaster.aspx" />
            <asp:ImageButton ID="imdReset" runat="server" TabIndex="34" EnableViewState="false"
                SkinID="btnreset" OnClientClick="javascript:return ResetPage();" />
           <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" PostBackUrl="~/Administration/Default.aspx" TabIndex="35" />
        </div>
    </div>--%>
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
                                <asp:Button ID="btnAdd" runat="server" SkinID="btnInner-New" ToolTip="<%$Resources:Controls,Add%>"
                                    Text="<%$Resources:Controls,Add%>" OnClientClick="javascript:return AddNew();"
                                    TabIndex="10" EnableViewState="false" />
                            </li>
                            <li>
                                <asp:Button ID="btnReset" runat="server" SkinID="btnInner-refresh" ToolTip="<%$Resources:Controls,Refresh%>"
                                    Text="<%$Resources:Controls,Refresh%>" OnClientClick="javascript:return ResetPage();"
                                    TabIndex="8" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
     <div id="divListing" class="content-wrapper">
        <div id="searchwrap" class="search-wrap-custom1" style="padding-top:8px !important;height:25px !important;">
            <label for="SearchType">
                <%=Resources.Controls.SearchBy%></label>
            <asp:DropDownList ID="SearchType" runat="server" TabIndex="30" EnableViewState="false"
                onchange="javascript:SetSearchType();">
                <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                </asp:ListItem>
                <asp:ListItem Value="COM_NAME" Text="<%$ Resources:BindValues, CompoundName%>">
                </asp:ListItem>
                <asp:ListItem Value="COM_CODE" Text="<%$ Resources:BindValues, CompoundCode%>">
                </asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="SearchValue" runat="server" TabIndex="31" EnableViewState="false">
            </asp:TextBox>
            <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" ToolTip="<%$ resources:ErpRes,Go %>"
               EnableViewState="false" TabIndex="32" OnClientClick="javascript:return BindGrid();" />
                <div class="clear"></div>
        </div>
        <div class="gridwrap">
            <table rules="all" id="grdCompoundDetails" grandtype="GrandGrid" ajaxurl="MachineryManagement.do?Action=GetMachineList"
                pagesize="20" paging="true" editfunction="GridAction" editable="true" class="gridwraptable gridwrap filter-arrow">
                <thead>
                    <tr>
                        <th fieldmap="COM_PK" isvisible="false">
                        </th>
                        <th fieldmap="COM_FLAG" isvisible="false">
                        </th>
                        <th fieldmap="COM_CODE" sortable="true" align="left" width="15%">
                            <%=Resources.Controls.CompCode%>
                        </th>
                        <th fieldmap="COM_NAME" sortable="true" align="left" width="20%">
                            <%=Resources.Controls.CompName%>
                        </th>
                        <th fieldmap="COMP_TYPE_NAME" sortable="true" align="left" width="15%">
                            <%=Resources.Controls.FormType%>
                        </th>
                        <th fieldmap="PLM_NAME" sortable="true" align="left" width="15%" >
                            <%=Resources.Controls.Polymer%>
                        </th>
                        <th fieldmap="QTY" sortable="true" align="left" width="10%">
                            <%=Resources.Controls.UnitQty%>
                        </th>
                        <th fieldmap="MAT" sortable="true" align="left" width="10%">
                            <%=Resources.Controls.MatPeriod%>
                        </th>
                        <th fieldmap="EXP" sortable="true" align="left" width="10%">
                            <%=Resources.Controls.ExpPeriod%>
                        </th>
                        <th fieldmap="COM_ACTIVE" isvisible="false">
                        </th>
                        <th type="Template" align="left" width="8%">                          
                                <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" Width="16px" Height="16px"
                                    OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Edit')" ToolTip='<%$ Resources:Controls,Edit %>'/>
                                <asp:ImageButton runat="server" ID="imbView" SkinID="btnview" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'View')" 
                                ToolTip='<%$ Resources:Controls,View %>'/>
                                <asp:ImageButton runat="server" ID="imbDeleteCopmound" SkinID="imbdeletegrid" Width="16px" ToolTip='<%$ Resources:Controls,Delete %>'
                                    Height="16px" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Delete')" />
                                <%--  <asp:ImageButton runat="server" ID="imbActive" SkinID="btnactive"
                                    Width="16px" Height="16px" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'InActivate')" />
                                     <asp:ImageButton runat="server" ID="imbInActive" SkinID="btninactive"
                                    Width="16px" Height="16px" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Activate')" />--%>
                        </th>
                    </tr>
                </thead>
            </table> 
        </div> 
        </div>
</asp:Content>
