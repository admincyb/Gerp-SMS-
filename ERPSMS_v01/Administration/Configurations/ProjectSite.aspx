<%@ Page Title="<%$ Resources:Captions,Title_ProjectSite %>" Language="C#" Theme="ClassicExt"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="ProjectSite.aspx.cs"
    Inherits="ERPSMS_v01.Administration.Configurations.ProjectSite" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/Administration/Configurations/ProjectSite.js.axd"
        type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:UpdatePanel ID="aupdpnlTaskHome" runat="server">
<ContentTemplate>
    <%--    <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.ProjectSite%>
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton ID="imbAdd" runat="server" SkinID="btnadd" OnClientClick="javascript:return AddNew();" />
            <asp:ImageButton ID="imbSave" runat="server" SkinID="btnsave" TabIndex="10" OnClientClick="javascript:return SavePage();" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" TabIndex="11" OnClientClick="javascript:return ResetPage();" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();" />
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
    <div class="clear">
    </div>
    <div class="content-wrapper">   
        <div id="divData">        
            <div id="grdTable-wrap">
                <asp:HiddenField ID="hdfchkToDate" runat="server" />
                <asp:HiddenField ID="LOC_PK" runat="server" Value="0" />
                <asp:HiddenField ID="UserID" runat="server" Value="0" />
                <asp:HiddenField ID="LOC_BIZUNIT" runat="server" Value="0" />
                <asp:HiddenField ID="LOC_ACTIVE" runat="server" Value="1" />
                <asp:HiddenField ID="SubmitFlag" runat="server" Value="0"></asp:HiddenField>
                <asp:HiddenField ID="LOC_MOD_DT" runat="server" />
                <asp:HiddenField ID="hdnSlNo" runat="server" Value="0" />
                <asp:HiddenField ID="SBU" runat="server" Value="0" />
                <table class="table-devide">              
                        <tr>
                        <td>
                            <div class="div2col-S">
                                <div class="content">
                                    <label for="LOC_CODE">
                                        <%=Resources.Controls.SiteID%>
                                        *</label>
                                    <asp:TextBox ID="LOC_CODE" runat="server" TabIndex="1" onkeydown="limitText(this,50);"
                                        onkeyup="limitText(this,50);" MaxLength="50"></asp:TextBox>
                                   
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">
                                <div class="content">
                                    <label for="LOC_NAME">
                                        <%=Resources.Controls.SiteName%>
                                        *</label>
                                    <asp:TextBox ID="LOC_NAME" runat="server" onkeydown="limitText(this,50);" onkeyup="limitText(this,50);"
                                        MaxLength="50" TabIndex="2"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                        </tr>
                        <tr>
                        <td colspan="2">
                        <div class ="divcol-S">
                           <div class="content">
                         <label for="LOC_DESC">
                                        <%=Resources.Controls.Description%></label>
                                    <asp:TextBox runat="server" ID="LOC_DESC" TabIndex="4" MaxLength="500" onkeydown="limitText(this,500);"
                                        onkeyup="limitText(this,500);" EnableViewState="false" EnableTheming="false"
                                        TextMode="MultiLine" Rows="3">
                                    </asp:TextBox>
                        </div>
                        </div>
                        </td>
                    </tr>                  
                </table>
                <div class="clear">
                </div>
            </div>              
        </div>
        <div id="divListing">
            <div id="searchwrap" class="search-wrap-custom1">
                <div id="divSearch">
                    <span>
                        <%=Resources.Controls.SearchBy%></span>
                    <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" onchange="javascript:SetSearchType();"
                        EnableViewState="false">
                        <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                        </asp:ListItem>
                        <asp:ListItem Value="LOC_NAME" Text="<%$ Resources:Controls, SiteName%>">
                        </asp:ListItem>
                        <asp:ListItem Value="LOC_CODE" Text="<%$ Resources:Controls, SiteID%>">
                        </asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false">
                    </asp:TextBox>
                    <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" OnClientClick="javascript:return BindGrid();"
                        EnableViewState="false" />
                    <div style="float: right; display: none">
                        <asp:Button ID="btnAdvSearch" runat="server" Text="Advance Search" OnClientClick="javascript:return ShowAdvSearch();" />
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="gridwrap">
                <table rules="all" id="grdProjectSite" grandtype="GrandGrid" paging="true" editfunction="GridActionHandler"
                    editable="true" width="100%" class="gridwraptable gridwrap filter-arrow">
                    <thead>
                        <tr>
                            <th fieldmap="LOC_PK" isvisible="false">
                            </th>
                            <th fieldmap="LOC_CODE" sortable="true" width="20%">
                                <%=Resources.Controls.SiteID%>
                            </th>
                            <th fieldmap="LOC_NAME" sortable="true" width="25%">
                                <%=Resources.Controls.SiteName%>
                            </th>
                            <th fieldmap="LOC_DESC" sortable="true" width="45%">
                                <%=Resources.Controls.Description%>
                            </th>
                            <th type="Template" width="10%">
                                <div>
                                    <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')" />
                                    <%-- <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />--%>
                                </div>
                            </th>
                        </tr>
                    </thead>
                </table>
                <%--<div class="clear">
                </div>--%>
            </div>
        </div>      
    </div>    
    </ContentTemplate>

     </asp:UpdatePanel>
</asp:Content>
