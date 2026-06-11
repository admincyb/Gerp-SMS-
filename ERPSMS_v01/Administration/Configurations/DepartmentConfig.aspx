<%@ Page Title="<%$ Resources:Captions,Title_BasicDeptConfiguration %>" Language="C#" MasterPageFile="~/Administration/Configurations/AdminConfigMaster.Master"
    EnableEventValidation="false" AutoEventWireup="true" CodeBehind="DepartmentConfig.aspx.cs"
    Inherits="ERPSMS_v01.Administration.Configurations.DepartmentConfig" Theme="ERP-Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/Administration/Configurations/DepartmentConfig.js.axd"
        type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:SiteMapPath ID="SiteMapPath1" runat="server" Visible="false">
    </asp:SiteMapPath>
    <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.BasicDeptConfiguration%>
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton runat="server" ID="imbSave" SkinID="btnsave" TabIndex="2" EnableViewState="False"
                OnClientClick="javascript:return SavePage();" />
            <asp:ImageButton runat="server" ID="imbReset" SkinID="btnreset" TabIndex="3" EnableViewState="False"
                OnClientClick="javascript:return ResetPage();" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" EnableViewState="False" OnClientClick="javascript:return CancelFun();"
                TabIndex="4" />
        </div>
    </div>
    <div id="grdTable-wrap">
        <div class="div2col-S">
            <div class="content">
                <label for="SBU">
                    <%=Resources.Captions.SBU%>
                    *
                </label>
                <asp:DropDownList ID="SBU" runat="server" onchange="javascript:BindGrid($(this).val());"
                    TabIndex="1" EnableViewState="false">
                </asp:DropDownList>
                <asp:HiddenField ID="DepartmentActive" runat="server">
                </asp:HiddenField>
            </div>
        </div>
        <div class="clear">
        </div>
        <div class="grdTable">
            <table rules="all" id="grdDeptDetails" grandtype="GrandGrid" enablecheckbox="true"
                width="100%">
                <thead>
                    <tr>
                        <th align="left" fieldmap="DPT_PK" isvisible="false">
                        </th>
                        <th align="left" fieldmap="DPT_ACTIVE" isvisible="false">
                        </th>
                        <th fieldmap="BZU_NAME" align="left" width="40%">
                            <%=Resources.Captions.SBU%>
                        </th>
                        <th fieldmap="DPT_NAME" align="left" width="50%">
                            <%=Resources.Controls.BaseDepartment%>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <div class="clear">
        </div>
    </div>
</asp:Content>
