<%@ Page Title="<%$ Resources:Captions,Title_DefaultValueConfig %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" Theme="Classic" AutoEventWireup="true" CodeBehind="DefaultValueConfig.aspx.cs"
    EnableEventValidation="false" Inherits="ERPSMS_v01.Administration.Configurations.DefaultValueConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/Administration/Configurations/DefaultValueConfig.js.axd"
        type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:SiteMapPath ID="SiteMapPath1" runat="server" RenderCurrentNodeAsLink="true"
        Visible="false">
    </asp:SiteMapPath>
    <%--    <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.DefaultValueConfiguration%>
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton runat="server" ID="imbSave" SkinID="btnsave" TabIndex="9" EnableViewState="False"
                OnClientClick="javascript:return SavePage();" />
            <asp:ImageButton runat="server" ID="imbReset" SkinID="btnreset" TabIndex="10" EnableViewState="False"
                OnClientClick="javascript:return ResetPage();" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" EnableViewState="False" OnClientClick="javascript:return CancelFun();"
                TabIndex="11" />
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
                                    TabIndex="35" EnableViewState="False" OnClientClick="javascript:return SavePage();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnReset" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Reset%>"
                                    TabIndex="36" EnableViewState="False" OnClientClick="javascript:return ResetPage();" />
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
            <table class="table-3devide">
                <tr>
                    <td>
                        <div class="div3col-S">
                            <label for="SBU">
                                <%=Resources.Captions.SBU%>*</label>
                            <asp:DropDownList ID="SBU" runat="server" onchange="javascript:FillDeptCombo($(this).val());"
                                TabIndex="1">
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="div3col-S">
                            <label for="Dept">
                                <%=Resources.Controls.BaseDepartment%></label>
                            <asp:DropDownList ID="Dept" runat="server" onchange="javascript:AddNew();" TabIndex="2">
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="div3col-S">
                            <label for="Group">
                                <%=Resources.Captions.Group%>*</label>
                            <asp:TextBox ID="Group" runat="server" TabIndex="3">
                            </asp:TextBox>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="clear">
        </div>
        <div class="grdTable">
            <div id="divHeader">
                <table id="DefaultList" class="gridwraptable gridwrap">
                    <thead>
                        <tr>
                            <th style="width: 25%; text-align: left">
                                <b>
                                    <%=Resources.Controls.Name%>
                                    * </b>
                            </th>
                            <th style="width: 25%; text-align: left">
                                <b>
                                    <%=Resources.Controls.DataType%>
                                </b>
                            </th>
                            <th style="width: 25%; text-align: left">
                                <b>
                                    <%=Resources.Controls.Value%>
                                    *</b>
                            </th>
                            <th style="width: 15%; text-align: left">
                                <b>
                                    <%=Resources.Controls.SetAsDefault%>
                                </b>
                            </th>
                            <th style="width: 10%; text-align: left">
                                <b>
                                    <%=Resources.Controls.Action%>
                                </b>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr class="grd-rowhead">
                            <td>
                                <asp:TextBox ID="DefaultName" runat="server" TabIndex="4">
                                </asp:TextBox>
                            </td>
                            <td>
                                <asp:DropDownList ID="DefaultType" runat="server" Width="150px" onchange="javascript:SetValueUtitlity($(this).val());"
                                    TabIndex="5">
                                    <asp:ListItem Value="<%$ Resources:BindValues, String%>" Text="<%$ Resources:BindValues, String%>">
                                    </asp:ListItem>
                                    <asp:ListItem Value="<%$ Resources:BindValues, Float%>" Text="<%$ Resources:BindValues, Float%>">
                                    </asp:ListItem>
                                    <asp:ListItem Value="<%$ Resources:BindValues, DateTime%>" Text="<%$ Resources:BindValues, DateTime%>">
                                    </asp:ListItem>
                                    <asp:ListItem Value="<%$ Resources:BindValues, Boolean%>" Text="<%$ Resources:BindValues, Boolean%>">
                                    </asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="DefaultValue" runat="server" Width="150px" TabIndex="6">
                                </asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="SetAsDefault" runat="server" TabIndex="7"></asp:CheckBox>
                            </td>
                            <td style="text-align: left">
                                <asp:ImageButton runat="server" ID="imgDftAdd" SkinID="imbaddnew" OnClientClick="javascript:return AddDefaultDetails();"
                                    TabIndex="8" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <table rules="all" id="grdDefaultList" grandtype="GrandGrid" paging="false" editfunction="GridAction"
                editable="true" width="100%" class="gridwraptable gridwrap">
                <thead>
                    <tr>
                        <th fieldmap="DFT_SL" isvisible="false">
                        </th>
                        <th fieldmap="DFT_BIZUNIT" isvisible="false">
                        </th>
                        <th fieldmap="DFT_DEPT" isvisible="false">
                        </th>
                        <th fieldmap="DFT_GROUP" isvisible="false">
                        </th>
                        <th fieldmap="DFT_IS_DEFAULT" isvisible="false">
                        </th>
                        <th fieldmap="DFT_CAN_DEL" isvisible="false">
                        </th>
                        <th fieldmap="DFT_CAN_MDFY" isvisible="false">
                        </th>
                        <th fieldmap="DFT_PK" isvisible="false">
                        </th>
                        <th fieldmap="DFT_NAME" align="left" width="25%">
                            <%=Resources.Controls.Name%>
                            *
                        </th>
                        <th fieldmap="DFT_TYPE" align="left" width="25%">
                            <%=Resources.Controls.DataType%>
                        </th>
                        <th fieldmap="DFT_VALUE" align="left" width="25%">
                            <%=Resources.Controls.Value%>
                            *
                        </th>
                        <th fieldmap="DFT_DEFAULT" align="left" width="15%">
                            <%=Resources.Controls.SetAsDefault%>
                        </th>
                        <th type="Template" align="left" width="10%">
                            <div style="text-align: left;">
                                <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$ resources:Controls,Edit %>"
                                    SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Edit')" />
                                <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$ resources:Controls,Delete %>"
                                    SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Delete')" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <div class="clear">
        </div>
        <div id="divResult">
            <asp:HiddenField ID="DFT_LIST" runat="server"></asp:HiddenField>
        </div>
        <asp:HiddenField ID="DefaultPk" runat="server" Value="0"></asp:HiddenField>
    </div>
</asp:Content>
