<%@ Page Title="<%$ Resources:Captions,Title_DesignationMaster %>" Theme="Classic"
    EnableEventValidation="false" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="DesignationMaster.aspx.cs" Inherits="ERPSMS_v01.Administration.Masters.DesignationMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/DesignationManagement/Designation.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--    <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.DesignationMaster%>
        </h1>
        <div class="button-wrap">
            <asp:ImageButton ID="imbAdd" runat="server" SkinID="btnadd" OnClientClick="javascript:return AddNew();" EnableViewState="False"
                TabIndex="4" />
            <asp:ImageButton ID="imbSave" runat="server" SkinID="btnsave" OnClientClick="javascript:return SavePage();" EnableViewState="False"
                TabIndex="8" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();" EnableViewState="False"
                TabIndex="9"  />
             <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();" TabIndex="10"/>
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
                                <asp:Button runat="server" ID="btnAdd" SkinID="btnInner-New" Text="<%$Resources:Controls,Add%>"
                                    EnableViewState="False" OnClientClick="javascript:return AddNew();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    EnableViewState="False" OnClientClick="javascript:return SavePage();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnReset" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Reset%>"
                                    EnableViewState="False" OnClientClick="javascript:return ResetPage();" />
                            </li>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" OnClientClick="javascript:return CancelFun();" />
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
                        <label for="SearchType">
                            <%=Resources.Controls.SearchBy%></label>
                        <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" TabIndex="1"
                            onchange="javascript:SetSearchType();" EnableViewState="false">
                            <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                            </asp:ListItem>
                            <asp:ListItem Value="DSG_NAME" Text="<%$ Resources:Controls, Designation%>">
                            </asp:ListItem>
                            <asp:ListItem Value="DPT_NAME" Text="<%$ Resources:Controls, Department%>">
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
                <div class="grdTable">
                    <table rules="all" id="grdDesignation" grandtype="GrandGrid" pagesize="20" paging="true"
                        width="110%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th fieldmap="DSG_PK" isvisible="false">
                                </th>
                                <th fieldmap="DSG_DEPT" isvisible="false">
                                </th>
                                <th fieldmap="DSG_NAME" sortable="true" width="47%" align="left">
                                    <%=Resources.Controls.Designation%>
                                </th>
                                <th fieldmap="DPT_NAME" sortable="true" width="47%" align="left">
                                    <%=Resources.Controls.Department%>
                                </th>
                                <th type="Template" width="6%" align="left">
                                    <div style="text-align: left">
                                        <asp:ImageButton runat="server" ID="imbEditMast" SkinID="imbeditgrid" EnableViewState="False"
                                            OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')" />
                                        <asp:ImageButton runat="server" ID="imbDeleteMast" SkinID="imbdeletegrid" EnableViewState="False"
                                            OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                    </div>
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
                                <label for="DSG_NAME">
                                    <%=Resources.Controls.DesignationName%>
                                    *
                                </label>
                                <asp:TextBox runat="server" ID="DSG_NAME" TabIndex="5" MaxLength="100" EnableViewState="False"></asp:TextBox>
                                <div class="clear">
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">
                                <label for="DSG_DEPT">
                                    <%=Resources.Controls.Department%>
                                    *
                                </label>
                                <asp:DropDownList ID="DSG_DEPT" runat="server" Width="200px" EnableViewState="false"
                                    TabIndex="6">
                                </asp:DropDownList>
                                <asp:ImageButton ID="imbViewCag" runat="server" SkinID="btnview" Width="22px"
                                    Height="22px" CssClass="imgbtnwrap" TabIndex="7" ToolTip="Department Details"
                                    OnClientClick="javascript:return ShowDeaprtment();" EnableViewState="false" />
                                <div class="clear">
                                </div>
                                <asp:HiddenField ID="DSG_PK" runat="server" Value="0"></asp:HiddenField>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="clear">
            </div>
            <asp:HiddenField ID="SBU" runat="server" Value="0" />
        </div>
        <div id="divDeptPopUp" title="<%=Resources.Captions.ChooseDepartment%>" style="height: 400px">
            <h3>
                <%=Resources.Controls.Department%></h3>
            <div class="clear">
            </div>
            <div id="treewrap" class="treeviewrap-fxd">
                <div id="trvCategory" class="treeview-adj">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
