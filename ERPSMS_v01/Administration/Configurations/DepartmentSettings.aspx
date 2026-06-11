<%@ Page Title="<%$ Resources:Captions,Title_DepartmentSettings %>" Language="C#" MasterPageFile="~/Administration/Configurations/AdminConfigMaster.Master"
    Theme="ERP-Admin" AutoEventWireup="true" CodeBehind="DepartmentSettings.aspx.cs"
    EnableEventValidation="false" Inherits="ERPSMS_v01.Administration.Configurations.DepartmentSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/Administration/Configurations/DepartmentSettings.js.axd"
        type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:SiteMapPath ID="SiteMapPath1" runat="server" Visible="false">
    </asp:SiteMapPath>
    <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.DepartmentSettings%>
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton runat="server" ID="imbSave" SkinID="btnsave" TabIndex="7" EnableViewState="False"
                OnClientClick="javascript:return SavePage();" />
            <asp:ImageButton runat="server" ID="imbReset" SkinID="btnreset" TabIndex="8" EnableViewState="False"
                OnClientClick="javascript:return ResetPage();" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" EnableViewState="False" OnClientClick="javascript:return CancelFun();"
                TabIndex="9" />
        </div>
    </div>
    <div class="clear">
    </div>
    <div id="grdTable-wrap">
        <div id="divData">
            <div class="div2col-S">
                <label for="SBU">
                    <%=Resources.Captions.SBU%>
                    *
                </label>
                <asp:DropDownList ID="SBU" runat="server" onchange="javascript:FillDeptCombo($(this).val());"
                    TabIndex="1">
                </asp:DropDownList>
                <div class="clear">
                </div>
            </div>
            <div class="div2col-S">
                <label for="Dept">
                    <%=Resources.Controls.BaseDepartment%>
                    *
                </label>
                <asp:DropDownList ID="Dept" runat="server" onchange="javascript:GetConfigListValues();"
                    TabIndex="2">
                </asp:DropDownList>
            </div>
            <div class="clear">
            </div>
            <div class="grdTable">
                <table id="ConfigList" width="100%">
                    <thead>
                        <tr>
                            <th style="width: 30%; text-align: left">
                                <b>
                                    <%=Resources.Controls.Name%>
                                    *</b>
                            </th>
                            <th style="width: 25%; text-align: left">
                                <b>
                                    <%=Resources.Controls.DataType%>
                                </b>
                            </th>
                            <th style="width: 35%; text-align: left">
                                <b>
                                    <%=Resources.Controls.Value%>
                                    * </b>
                            </th>
                            <th width="10%" text-align: left">
                                <b>
                                    <%=Resources.Controls.Action%>
                                </b>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                <asp:TextBox ID="ConfigName" runat="server" TabIndex="3">
                                </asp:TextBox>
                            </td>
                            <td>
                                <asp:DropDownList ID="ConfigType" runat="server" Width="150px" onchange="javascript:SetValueUtitlity($(this).val());"
                                    TabIndex="4">
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
                                <asp:TextBox ID="ConfigValue" runat="server" Width="150px" TabIndex="5">
                                </asp:TextBox>
                            </td>
                            <td style="text-align: left">
                                <asp:ImageButton runat="server" ID="imbAdd" SkinID="imbaddnew" OnClientClick="javascript:return AddConfigDetails();"
                                    TabIndex="6" />
                            </td>
                        </tr>
                    </tbody>
                </table>
                <table rules="all" id="grdConfigList" grandtype="GrandGrid" paging="false" editfunction="GridAction"
                    width="100%" editable="true">
                    <thead>
                        <tr>
                            <th fieldmap="CFG_SL" isvisible="false">
                            </th>
                            <th fieldmap="CFG_BIZUNIT" isvisible="false">
                            </th>
                            <th fieldmap="CFG_DEPT" isvisible="false">
                            </th>
                            <th fieldmap="CFG_PK" isvisible="false">
                            </th>
                            <th fieldmap="CFG_NAME" align="left" width="30%">
                                <%=Resources.Controls.Name%>
                                *
                            </th>
                            <th fieldmap="CFG_DATA_TYPE" align="left" width="25%">
                                <%=Resources.Controls.DataType%>
                                *
                            </th>
                            <th fieldmap="CFG_VALUE" align="left" width="35%">
                                <%=Resources.Controls.Value%>
                                *
                            </th>
                            <th type="Template" align="left" width="10%">
                                <div style="text-align: left">
                                    <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Edit')" />
                                    <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Delete')" />
                                </div>
                            </th>
                        </tr>
                    </thead>
                </table>
            </div>
            <div class="clear">
            </div>
            <div id="divResult">
                <asp:HiddenField ID="CFG_LIST" runat="server">
                </asp:HiddenField>
            </div>
            <asp:HiddenField ID="ConfigPk" runat="server" Value="0">
            </asp:HiddenField>
        </div>
        <div class="clear">
        </div>
    </div>
</asp:Content>
