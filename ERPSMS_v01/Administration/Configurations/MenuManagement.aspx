<%@ Page Title="Menu Management" Language="C#" MasterPageFile="~/Administration/Configurations/AdminConfigMaster.Master"
    AutoEventWireup="true" Theme="ERP-Admin" EnableEventValidation="false" CodeBehind="MenuManagement.aspx.cs"
    Inherits="ERPSMS_v01.Administration.Configurations.MenuManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/Administration/Configurations/MenuManagement.js.axd"
        type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.MenuManagement%>
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton runat="server" ID="imbSave" SkinID="btnsave" TabIndex="4" EnableViewState="False"
                OnClientClick="javascript:return SavePage();" />
            <asp:ImageButton runat="server" ID="imbDelete" SkinID="btndelete" TabIndex="5" EnableViewState="False"
                OnClientClick="javascript:return DeletePage();" />
            <asp:ImageButton runat="server" ID="imbResetall" SkinID="btnreset" TabIndex="6" EnableViewState="False"
                OnClientClick="javascript:return ResetPage();" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" TabIndex="7" EnableViewState="False"
                OnClientClick="javascript:return history.go(-1)" />
        </div>
    </div>
    <div id="grdTable-wrap">
        <div class="div2col-S">
            <div id="edittree" class="edittree">
                <h1>
                    <%=Resources.Captions.MenuManagementList%>
                </h1>
                <div id="trvCategory" class="treeview-adj">
                </div>
            </div>
        </div>
        <div class="div2col-S" style="float: right; margin-right: 0">
            <label for="MenuName">
                <%=Resources.Controls.MenuName%>
            </label>
            <asp:TextBox ID="MenuName" TabIndex="1" runat="server" EnableViewState="False">
            </asp:TextBox>
            <label for="MenuUrl">
                <%=Resources.Controls.MenuURL%>
            </label>
            <asp:TextBox ID="MenuUrl" TabIndex="2" runat="server" EnableViewState="False">
            </asp:TextBox>
            <label for="MenuPosition">
                <%=Resources.Controls.MenuPosition%>
            </label>
            <asp:TextBox ID="MenuPosition" TabIndex="3" runat="server" EnableViewState="False"
                Width="100px">
            </asp:TextBox>
            <div class="clear">
            </div>
            <label for="Parent">
                <%=Resources.Controls.Parent%></label>
            <asp:Label ID="Parent" runat="server" EnableViewState="False" Width="100px"></asp:Label>
            <asp:HiddenField ID="MenuPK" runat="server" Value="0"></asp:HiddenField>
            <asp:HiddenField ID="MenuParentPK" runat="server" Value="0"></asp:HiddenField>
        </div>
        <div class="clear"></div>
    </div>
</asp:Content>
