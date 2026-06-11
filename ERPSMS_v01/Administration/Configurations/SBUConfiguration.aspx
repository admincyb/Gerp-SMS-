<%@ Page Title="<%$ Resources:Captions,Title_SBUConfiguration %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    EnableEventValidation="false" AutoEventWireup="true" CodeBehind="SBUConfiguration.aspx.cs"
    Inherits="ERPSMS_v01.Administration.Configurations.SBUConfiguration" Theme="ClassicExt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/Administration/Configurations/SBUConfiguration.js.axd"
        type="text/javascript"></script>
    <style type="text/css">
  
        .footer-content
        {
            margin: 0px auto;
            width: 100%;
            text-align: center;
            line-height: 35px;
            border-top: 1px solid #ddd;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:SiteMapPath ID="SiteMapPath1" runat="server" Visible="false">
    </asp:SiteMapPath>
    <%--  <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.SBUConfiguration%>
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton runat="server" ID="imbSave" SkinID="btnsave" TabIndex="4" EnableViewState="False"
                OnClientClick="javascript:return SavePage();" />
            <asp:ImageButton runat="server" ID="imbReset" SkinID="btnreset" TabIndex="5" EnableViewState="False"
                OnClientClick="javascript:return ResetPage();" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" EnableViewState="False" OnClientClick="javascript:return CancelFun();"
                TabIndex="6" />
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
                                <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    TabIndex="18" EnableViewState="False" OnClientClick="javascript:return SavePage();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnReset" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Reset%>"
                                    TabIndex="19" EnableViewState="False" OnClientClick="javascript:return ResetPage();" />
                            </li>
                            <li>
                                <%--<asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" OnClientClick="javascript:return CancelFun();" TabIndex="16" />--%>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" OnClientClick="javascript:return RedirectToInbox();"
                                    TabIndex="20" />
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
            <div id="divData">
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <label for="SBUCode">
                                    <%=Resources.Captions.SBUCode%>
                                    *
                                </label>
                                <asp:TextBox ID="SBUCode" runat="server" TabIndex="1" EnableViewState="false" CssClass="input-half">
                                </asp:TextBox>
                                <asp:HiddenField runat="server" ID="SBUPk" Value="0"></asp:HiddenField>
                                <div class="clear">
                                </div>
                                <label for="SBUAddr1">
                                    <%=Resources.Captions.SBUPrimaryContract%>
                                    *
                                </label>
                                <asp:TextBox ID="SBUAddr1" TabIndex="3" runat="server" TextMode="MultiLine" EnableViewState="false"
                                    CssClass="input-half multiline-2col">
                                </asp:TextBox>
                                <div class="clear">
                                </div>
                                <label for="SBUCity">
                                    <%=Resources.Captions.SBUCity%>
                                </label>
                                <asp:TextBox ID="SBUCity" runat="server" TabIndex="5" EnableViewState="false" CssClass="input-half">
                                </asp:TextBox>
                                <div class="clear">
                                </div>
                                <label for="SBUCountry">
                                    <%=Resources.Captions.SBUCountry%> *
                                </label>
                                <asp:DropDownList ID="SBUCountry" runat="server" TabIndex="8" CssClass="select-half-a">
                                </asp:DropDownList>
                                <div class="clear">
                                </div>
                                <label for="SBUState">
                                    <%=Resources.Captions.SBUState%>
                                </label>
                                <asp:DropDownList ID="SBUState" runat="server" TabIndex="10" CssClass="select-half-a">
                                </asp:DropDownList>
                                <div class="clear">
                                </div>
                                <label for="SBUFax">
                                    <%=Resources.Captions.SBUFax%>
                                </label>
                                <asp:TextBox ID="SBUFax" runat="server" TabIndex="13" EnableViewState="false" CssClass="input-small-c">
                                </asp:TextBox>
                                <label for="SBUTaxNo" class="lbl-7-5perc">
                                    <%=Resources.Captions.SBUTaxNo%>
                                </label>
                                <asp:TextBox ID="SBUTaxNo" runat="server" TabIndex="14" EnableViewState="false" CssClass="input-small-c">
                                </asp:TextBox>
                                <div class="clear">
                                </div>
                                <label for="BZU_FIN_START_DT">
                                    <%=Resources.Captions.BZU_FIN_START_DT%>
                                </label>
                               
                                <asp:TextBox ID="BZU_FIN_START_DT" runat="server" TabIndex="16" EnableViewState="false"
                                    onkeydown="return CheckKey(event)" onpaste="return false;" CssClass="input-small sbu-month-only"
                                    MaxLength="12">
                                </asp:TextBox>
                                <asp:TextBox ID="BZU_FIN_END_DT" runat="server" TabIndex="17" EnableViewState="false"
                                    onkeydown="return CheckKey(event)" onpaste="return false;" CssClass="input-small sbu-month-only"
                                    MaxLength="12">
                                </asp:TextBox>

                                 <div class="clear">
                            </div>
                                <label for="BZU_YEAR_CTRL" >
                                    <%=Resources.Captions.YearControl%>
                                </label>
                                <asp:TextBox ID="BZU_YEAR_CTRL" runat="server" TabIndex="1" onkeydown="return CheckKey(event)"
                                    onpaste="return false;" CssClass="input-small" MaxLength="19"></asp:TextBox>
                            </div>
                           
                        </td>
                        <td>
                            <div class="div2col-S">
                                <label for="SBUName">
                                    <%=Resources.Captions.SBUName%>
                                    *
                                </label>
                                <asp:TextBox ID="SBUName" runat="server" TabIndex="2" EnableViewState="false" CssClass="input-half">
                                </asp:TextBox>
                                <div class="clear">
                                </div>
                                <label for="SBUAddr2">
                                    <%=Resources.Captions.SBUAddr2%>
                                </label>
                                <asp:TextBox ID="SBUAddr2" TabIndex="4" runat="server" TextMode="MultiLine" EnableViewState="false"
                                    CssClass="input-half multiline-2col">
                                </asp:TextBox>
                                <div class="clear">
                                </div>
                                <label for="SBUPhone">
                                    <%=Resources.Captions.SBUPhone%>
                                </label>
                                <asp:TextBox ID="SBUPhone" runat="server" TabIndex="6" EnableViewState="false" CssClass="input-small-c">
                                </asp:TextBox>
                                <label for="SBUMobile" class="lbl-7-5perc">
                                    <%=Resources.Captions.SBUMobile%>
                                </label>
                                <asp:TextBox ID="SBUMobile" runat="server" TabIndex="7" EnableViewState="false" CssClass="input-small-c">
                                </asp:TextBox>
                                <div class="clear">
                                </div>
                                <label for="SBUEmail">
                                    <%=Resources.Captions.SBUEmail%>
                                </label>
                                <asp:TextBox ID="SBUEmail" runat="server" TabIndex="9" EnableViewState="false" CssClass="input-half">
                                </asp:TextBox>
                                <div class="clear">
                                </div>
                                <label for="BZU_CURRENCY">
                                    <%=Resources.Controls.Currency%>
                                    *</label>
                                <asp:DropDownList runat="server" ID="BZU_CURRENCY" TabIndex="11" CssClass="select-small">
                                </asp:DropDownList>
                                <label for="BZU_REG_NO" class="middle-lbl">
                                    <%=Resources.Captions.BZU_REG_NO%>
                                </label>
                                <asp:TextBox ID="BZU_REG_NO" runat="server" TabIndex="12" EnableViewState="false"
                                    CssClass="input-w28per">
                                </asp:TextBox>
                                <div class="clear">
                                </div>
                                <label for="BZU_GST_NO">
                                    <%=Resources.Captions.BZU_GST_NO%>
                                </label>
                                <asp:TextBox ID="BZU_GST_NO" runat="server" TabIndex="15" EnableViewState="false"
                                    CssClass="input-half">
                                </asp:TextBox>
                                <div class="clear">
                                </div>
                                <asp:Label ID="lblTheme" runat="server" AssociatedControlID="ddlTheme" Text="<%$ resources:Theme %>" />
                                <asp:DropDownList runat="server" ID="ddlTheme" TabIndex="17" CssClass="select-half-a margn-lft3">
                                    <asp:ListItem Text="<%$ resources:Controls, SelectVal %>" Value="-1" />
                                    <asp:ListItem Text="<%$ resources:Controls, ThemeClassic %>" Value="ClassicExt" />
                                    <asp:ListItem Text="<%$ resources:Controls, ThemeBlue %>" Value="BlueExt" />
                                    <asp:ListItem Text="<%$ resources:Controls, ThemeRed %>" Value="RedExt" />
                                </asp:DropDownList>
                                <asp:HiddenField ID="BZU_THEME" runat="server" Value="" />

                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="clear">
            </div>
            <div id="divListing">
                <div class="gridwrap">
                    <table rules="all" id="grdSBUList" grandtype="GrandGrid" pagesize="10" paging="true"
                        width="100%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th fieldmap="BZU_PK" isvisible="false"></th>
                                <th fieldmap="IS_INACTIVE" isvisible="false"></th>
                                <th fieldmap="BZU_ACTIVE" isvisible="false"></th>
                                <th fieldmap="BZU_CURRENCY" isvisible="false"></th>
                                <th fieldmap="BZU_CODE" sortable="true" align="left" width="15%">
                                    <%=Resources.Captions.SBUCode%>
                                </th>
                                <th fieldmap="BZU_NAME" sortable="true" align="left" width="35%">
                                    <%=Resources.Captions.SBUName%>
                                </th>
                                <th fieldmap="BZU_ADDR1" sortable="true" align="left" width="45%">
                                    <%=Resources.Captions.SBUAddress%>
                                </th>
                                <th type="Template" align="left" width="5%">
                                    <div style="text-align: left">
                                        <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" ToolTip="<%$ resources:Controls,Edit %>"
                                            OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Edit')" />
                                        <asp:ImageButton runat="server" ID="imbInActivate" SkinID="btnactive" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Active',0)"
                                            ToolTip="Active" />
                                        <asp:ImageButton runat="server" ID="imbActivate" SkinID="btninactive" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Active',1)"
                                            ToolTip="Inactive" />
                                    </div>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
    </div>
    <div class="clear">
    </div>
    <div class="footer-content" id="divfooter">
        <asp:Label ID="lblProductNameH" runat="server" Text="<%$ resources:Controls,ProductName %>" />:
        <asp:Label ID="SYS_NAME" runat="server" />&nbsp;| &nbsp;
        <asp:Label ID="lblVersionH" runat="server" Text="<%$ resources:Controls,Version %>" />:
        <asp:Label ID="SYS_VERSION" runat="server" />&nbsp;| &nbsp;
        <asp:Label ID="lblGAFH" runat="server" Text="<%$ resources:Controls,GAF %>" />:
        <asp:Label ID="SYS_GAF_VERSION" runat="server" />
    </div>
</asp:Content>
