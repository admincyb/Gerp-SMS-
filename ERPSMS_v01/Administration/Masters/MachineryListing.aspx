<%@ Page Title="<%$ Resources:Captions,Title_MachineryMaster %>" Language="C#" Theme="Classic"
    EnableEventValidation="false" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="MachineryListing.aspx.cs" Inherits="ERPSMS_v01.Administration.Masters.MachineryListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/MachineryManagement/MachineryListing.js.axd"
        type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--   <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.MachineryMaster%>
        </h1>
        <div class="button-wrap">
            <asp:ImageButton ID="imbAdd" runat="server" TabIndex="33" EnableViewState="false"
                SkinID="btnadd" PostBackUrl="~/Administration/Masters/MachineryMaster.aspx" />
            <asp:ImageButton ID="imdReset" runat="server" TabIndex="34" EnableViewState="false"
                SkinID="btnreset" OnClientClick="javascript:return ResetPage();" />
             <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel"  PostBackUrl="~/Administration/Default.aspx" TabIndex="35" />
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
                                <asp:Button ID="btnAdd" runat="server" SkinID="btnInner-New" Text="<%$Resources:Controls,Add%>"
                                    ToolTip="<%$Resources:Controls,Add%>" PostBackUrl="~/Administration/Masters/MachineryMaster.aspx"
                                    abIndex="33" EnableViewState="false" />
                            </li>
                            <li>
                                <asp:Button ID="btnReset" runat="server" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Refresh%>"
                                    OnClientClick="javascript:return ResetPage();" TabIndex="34" ToolTip="<%$Resources:Controls,Refresh%>" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <div id="grdTable-wrap">
            <div id="searchwrap" class="search-wrap-custom1">
                <label for="SearchType">
                    <%=Resources.Controls.SearchBy%></label>
                <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" TabIndex="30"
                    EnableViewState="false" onchange="javascript:SetSearchType();">
                    <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                    </asp:ListItem>
                    <asp:ListItem Value="MCH_NAME" Text="<%$ Resources:Controls, MachineName%>">
                    </asp:ListItem>
                    <asp:ListItem Value="MCH_CODE" Text="<%$ Resources:Controls, MachineCode%>">
                    </asp:ListItem>
                    <asp:ListItem Value="MCT_NAME" Text="<%$ Resources:Controls, MachineType%>">
                    </asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="SearchValue" runat="server" TabIndex="31" EnableViewState="false">
                </asp:TextBox>
                <asp:Label runat="server" ID="lblStatus" Text="Status" TabIndex="13"></asp:Label>
                <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="2">
                    <asp:ListItem Value="-1">ALL</asp:ListItem>
                    <asp:ListItem Value="1">Active</asp:ListItem>
                    <asp:ListItem Value="0">Inactive</asp:ListItem>
                </asp:DropDownList>
                <asp:HiddenField runat="server" ID="hdfStatus" />
                <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" EnableViewState="false"
                    TabIndex="32" OnClientClick="javascript:return BindMachineGrid();" />
                <div class="clear">
                </div>
            </div>
            <div class="grdTable">
                <table rules="all" id="grdMachineDetails" grandtype="GrandGrid" ajaxurl="MachineryManagement.do?Action=GetMachineList"
                    pagesize="20" paging="true" editfunction="GridAction" editable="true" class="gridwraptable gridwrap">
                    <thead>
                        <tr>
                            <th fieldmap="MCH_PK" isvisible="false">
                            </th>
                            <th fieldmap="MCH_CODE" sortable="true" align="left" width="16%">
                                <%=Resources.Controls. MachineCode%>
                            </th>
                            <th fieldmap="MCH_NAME" sortable="true" align="left" width="18%">
                                <%=Resources.Controls.MachineName%>
                            </th>
                            <th fieldmap="MCT_NAME" sortable="true" align="left" width="16%">
                                <%=Resources.Controls.MachineType%>
                            </th>
                            <th fieldmap="LOC_NAME" sortable="true" align="left" width="16%">
                                <%=Resources.Controls.LocationWorkCenter%>
                            </th>
                            <th fieldmap="MCH_PLANT_TEXT" sortable="false" align="left" width="5%">
                                <%=Resources.Controls.Plant%>
                            </th>
                            <th fieldmap="MCH_ACTIVE_TEXT" width="9%" sortable="true" align="left">
                                <%=Resources.Controls.Active%>
                            </th>
                           <%--  <th type="template" width="6%">
                            <asp:Label runat="server" ID="lbStatus" Text='<%# Eval("MCH_PLANT_TEXT").ToString() == "1" ? "Yes" : "No" %>'></asp:Label>
                            </th>--%>
                            <th type="Template" align="left" width="5%">
                                <div style="text-align: center">
                                    <asp:ImageButton runat="server" ID="imbEditMachineDtls" SkinID="imbeditgrid" Width="16px"
                                        ToolTip="<%$Resources:Controls,Edit%>" Height="16px" OnClientClick="javascript:return MachineGridHandler($(this).parents('tr:eq(0)'),'Edit')" />
                                    <asp:ImageButton runat="server" ID="imbDeleteMachineDtls" SkinID="imbdeletegrid"
                                        ToolTip="<%$Resources:Controls,Delete%>" Width="16px" Height="16px" OnClientClick="javascript:return MachineGridHandler($(this).parents('tr:eq(0)'),'Delete')" />
                                </div>
                            </th>
                        </tr>
                    </thead>
                </table>
                <div class="clear">
                </div>
                <asp:HiddenField ID="SBU" runat="server" Value="0" />
            </div>
        </div>
    </div>
</asp:Content>
