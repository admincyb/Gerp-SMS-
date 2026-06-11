<%@ Page Title="Store Master" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    Theme="Classic" CodeBehind="StoreMaster.aspx.cs" Inherits="ERPSMS_v01.StoreManagement.StoreMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/StoreMaster.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--<div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.StoreMaster%></h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton ID="imbAdd" runat="server" SkinID="btnadd" OnClientClick="javascript:return AddNew();" EnableViewState="false" TabIndex="5"  />
            <asp:ImageButton ID="imbSave" runat="server" SkinID="btnsave" OnClientClick="javascript:return SavePage();" EnableViewState="false" TabIndex="3"  />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return PageInit();" EnableViewState="false" TabIndex="4"  />
            <img alt="Cancel" src="../Images/ERP-Blue/page-btn-cancel.gif" SkinID="btncancel" onclick="javascript:history.go(-1)" />
        </div>
    </div>--%>
    <div class="fixed-buttons">
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
                                    TabIndex="35" EnableViewState="False" OnClientClick="javascript:return SavePage('Draft');" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnReset" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Reset%>"
                                    TabIndex="36" EnableViewState="False" OnClientClick="javascript:return PageInit();" />
                            </li>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" OnClick="javascript:history.go(-1)" TabIndex="37" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
        </div>
        <div class="content-wrapper">
            <div id="grdTable-wrap">
                <div id="divListing">
                    <div id="searchwrap" class="search-wrap-custom1">
                        <span>
                            <%=Resources.Controls.SearchBy%></span>
                        <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" onchange="javascript:SetSearchType();"
                            EnableViewState="false">
                            <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>"></asp:ListItem>
                            <asp:ListItem Value="TrsNem" Text="<%$ Resources:BindValues, StoreName%>"></asp:ListItem>
                            <asp:ListItem Value="ErsNem" Text="<%$ Resources:BindValues, StoreType%>"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="SearchValue" runat="server" CssClass="srchboxtextbx" EnableViewState="false">
                        </asp:TextBox>
                        <div class="srchbtnwrap">
                            <%--<asp:ImageButton ID="imbSearch" runat="server" SkinID="btnsearchgo" Width="30px"
                            Height="20px" EnableViewState="false" OnClientClick="javascript:return BindGrid();" />--%>
                            <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" OnClientClick="javascript:return BindGrid();"
                                EnableViewState="false" />
                        </div>
                        <div class="clear">
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="gridwrap">
                        <table rules="all" id="grdStore" grandtype="GrandGrid" pagesize="20" paging="true"
                            editfunction="GridAction" editable="true" class="gridwraptable gridwrap filter-arrow">
                            <thead>
                                <tr>
                                    <th fieldmap="Trsp" isvisible="false">
                                    </th>
                                    <th fieldmap="TrsNem" sortable="true" align="left" width="35%">
                                        <%=Resources.Controls.StoreName%>
                                    </th>
                                    <th fieldmap="ErsNem" sortable="true" align="left" width="35%">
                                        <%=Resources.Controls.StoreType%>
                                    </th>
                                    <th fieldmap="Ersp" sortable="true" align="left" isvisible="false">
                                    </th>
                                    <th type="Template" width="1%">
                                        <div style="text-align: center">
                                            <asp:ImageButton runat="server" ID="imbEditMast" SkinID="imbeditgrid" EnableViewState="false"
                                                OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')" />
                                            <asp:ImageButton runat="server" ID="imbDeleteMast" SkinID="imbdeletegrid" EnableViewState="false"
                                                OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                        </div>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div id="divData">
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <div class="content">
                                        <span>
                                            <%=Resources.Controls.StoreName%></span>
                                        <asp:TextBox runat="server" ID="StoreName" TabIndex="1" EnableViewState="false"></asp:TextBox>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S" style="float: right; margin-right: 0">
                                    <div class="content">
                                        <span>
                                            <%=Resources.Controls.StoreType%></span>
                                        <asp:DropDownList runat="server" ID="StoreTypeID" TabIndex="2" EnableViewState="false"
                                            Width="240px">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <asp:HiddenField runat="server" ID="StoreMasterID" Value="0" EnableViewState="false" />
        </div>
</asp:Content>
