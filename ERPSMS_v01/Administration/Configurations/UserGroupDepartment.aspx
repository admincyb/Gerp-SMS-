<%@ Page Title="<%$ Resources:Captions,Title_UserGroupDeptMapping %>" Language="C#" MasterPageFile="~/Administration/Configurations/AdminConfigMaster.Master"
    Theme="ERP-Admin" AutoEventWireup="true" CodeBehind="UserGroupDepartment.aspx.cs"
    EnableEventValidation="false" Inherits="ERPSMS_v01.Administration.Configurations.UserGroupDepartment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/Administration/Configurations/UserGroupDepartment.js.axd"
        type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:SiteMapPath ID="SiteMapPath1" runat="server" RenderCurrentNodeAsLink="true"
        Visible="false">
    </asp:SiteMapPath>
    <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.UserGroupDeptMapping%>
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton runat="server" ID="imbSave" SkinID="btnsave" TabIndex="3" EnableViewState="False"
                OnClientClick="javascript:return SavePage();" />
            <asp:ImageButton runat="server" ID="imbReset" SkinID="btnreset" TabIndex="4" EnableViewState="False"
                OnClientClick="javascript:return ResetPage();" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" EnableViewState="False" OnClientClick="javascript:return CancelFun();" TabIndex="5" />
        </div>
    </div>
    <div class="clear">
    </div>
    <div id="grdTable-wrap">
        <div id="divData">
            <div class="div2col-S">
                <label for="UserGroup">
                    <%=Resources.Captions.UserGroup%>
                </label>
                <asp:DropDownList ID="UserGroup" runat="server" onchange="javascript:FillDeptCombo();" TabIndex="1"
                    EnableViewState="false"> 
                </asp:DropDownList>
            </div>
            <div class="div2col-S">
                <label for="SBU">
                    <%=Resources.Captions.SBU%>
                </label>
                <asp:DropDownList ID="SBU" runat="server" onchange="javascript:FillDeptCombo();" TabIndex="2"
                    EnableViewState="false">
                </asp:DropDownList>
                <div class="clear">
                </div>
                <div id="treewrap" class="edittree">
                    <div id="trvDepartment" class="treeview-adj">
                    </div>
                </div>
            </div>
        </div>
        <div id="divResult">
            <asp:HiddenField ID="USER_GROUP_DEPT_LIST" runat="server">
            </asp:HiddenField>
        </div>
        <div class="clear">
        </div>
    </div>
</asp:Content>
