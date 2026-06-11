<%@ Page Title="<%$ Resources:Captions,Title_MenuGroupSettings %>" Theme="ERP-Admin" Language="C#" MasterPageFile="~/Administration/Configurations/AdminConfigMaster.Master"
    EnableEventValidation="false" AutoEventWireup="true"  CodeBehind="MenuGroupSettings.aspx.cs"
    Inherits="ERPSMS_v01.Administration.Configurations.MenuGroupSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/Administration/Configurations/MenuGroupSettings.js.axd"
        type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="webwizard-wrap">
        <h1>
            
             <%=Resources.Captions.MenuGroupSettings%>
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton runat="server" ID="imbSave" SkinID="btnsave" TabIndex="3" EnableViewState="False"
                OnClientClick="javascript:return SavePage();" />
            <asp:ImageButton runat="server" ID="imbResetall" SkinID="btnreset" TabIndex="4" EnableViewState="False"
                OnClientClick="javascript:return ResetPage();" />
        <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();" TabIndex="5"/>
        </div>
    </div>
    <div>
        <div id="grdTable-wrap">
            <div class="div2col-S">
                <label for="UserGroup">
                    <%=Resources.Captions.UserGroup%> *
                </label>
                <asp:DropDownList ID="UserGroup" onchange="javascript:FillMenuTreeView();" TabIndex="1" runat="server">
                </asp:DropDownList>
            </div>
            <div class="div2col-S">
                <label for="SBU">
                    <%=Resources.Captions.SBU%> *
                </label>
                <asp:DropDownList ID="SBU" runat="server" TabIndex="2" onchange="javascript:FillMenuTreeView();"
                    EnableViewState="false">
                </asp:DropDownList>
                <div class="clear">
                </div>
                <div id="treewrap" class="edittree">
                    <div id="trvMenu" class="treeview-adj">
                    </div>
                </div>
            </div>
            <div class="clear">
            </div>
        </div>
        <div id="divResult">
            <asp:HiddenField ID="USER_GROUP_MENU_LIST" runat="server">
            </asp:HiddenField>
        </div>
        <div class="clear">
        </div>
    </div>
</asp:Content>
