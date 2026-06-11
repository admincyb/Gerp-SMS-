<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="BackupAndRestore.aspx.cs" Inherits="ERPSMS_v01.Administration.Configurations.BackupAndRestore"
    Theme="Classic" ValidateRequest="false" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var msgTitle = '<%= Resources.ErpRes.Information %>';
        var msgContent = "";

        function SelectedRestoreConfirm(ctrl, gridID) {
            if (gridID) {
                if ($("[id$=" + gridID + "] tr td input[type=radio]:checked").length > 0) {
                    return ShowRestoreConfirm(ctrl);
                }
                else {
                  
                    $("[id$=litErrorMsg]").html('<%= Resources.Report.Msg_Not_Selected %>'); 
                    ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
                    return false;
                }
            }
            else return ShowRestoreConfirm();
        }

        function ShowListing(flag) {
            if (flag) {
                $("[id$=PageAction_List]").show();
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlListing]").show();
                $("[id$=pnlEntry]").hide();
            }
            else {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
            }
            return false;
        }

        function ShowRestoreConfirm(btn, message) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = message ? message : '<%= Resources.ErpRes.MsgRestoreConfirm %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $(this).dialog("close");
                        $('#updateProgress').show();
                        __doPostBack(btn.name, '');
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        if (typeof AfterDeleteConfirmationCancel == "function") {
                            AfterDeleteConfirmationCancel(btn.id);
                        }
                        return false;
                    }
                }
            });
            return false;
        }

        var validationArrayGroup;
        function CheckValidationDuplicate(valGroup) {
            validationArrayGroup = new Array();
            for (var i = Page_Validators.length - 1; i >= 0; i--) {
                if (typeof (Page_Validators[i].validationGroup) == "string") {
                    if (valGroup == Page_Validators[i].validationGroup) {
                        if (!CheckValidationExists(Page_Validators[i].id)) {
                            validationArrayGroup.push(Page_Validators[i].id);
                        }
                        else {
                            Page_Validators.splice(i, 1);
                        }
                    }
                    else {
                        Page_Validators.splice(i, 1);
                    }
                }
            }
        }
        function CheckValidationExists(id) {
            for (var i in validationArrayGroup) {
                if (validationArrayGroup[i] == id) {
                    return true;
                }
            }
            return false;
        }
        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                CheckValidationDuplicate(valGroup);
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html());
                return false;
            }
            else {
                return true;
            }
        }    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="fixed-buttons-normal">
        <div class="Button-container">
            <asp:Table ID="Table1" runat="server">
                <asp:TableRow>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul class="bredcrum">
                            <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                        </ul>
                        <ul runat="server" id="pnlEntry" style="display: none">
                            <li>
                                <asp:Button runat="server" ID="btnBackUp" CommandName="BACKUP" TabIndex="2" Text="<%$resources:BackUp %>"
                                    OnClick="ActionHandler" ToolTip="<%$resources:BackUp %>" CommandArgument="SEC_ActionPanel"
                                    SkinID="btnInner-backup" ValidationGroup="backup" OnClientClick="javascript:ValidatePageNow('backup')"/>
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                    OnClick="ActionHandler" CommandName="CANCEL" TabIndex="35" SkinID="btnInner-Cancel"
                                    ToolTip="<%$resources:Controls,Cancel %>" />
                            </li>
                        </ul>
                        <ul runat="server" id="pnlListing" style="display: none">
                            <li>
                                <asp:Button runat="server" TabIndex="4" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                    Text="<%$resources:NewBackup %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                    ToolTip="<%$resources:NewBackup %>" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnRestore" CommandName="RESTORE" TabIndex="3" Text="<%$resources:Restore %>"
                                    OnClick="ActionHandler" ToolTip="<%$resources:Restore %>" CommandArgument="SEC_ActionPanel"
                                    SkinID="btnInner-restore" OnClientClick="return SelectedRestoreConfirm(this,'grdBackupList');"/>
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <div class="tab-container-floating">
            <ul>
                <li>
                    <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                        CommandArgument="SEC_ActionPanel" TabIndex="79" OnClick="ActionHandler" CommandName="LIST"
                        CssClass="tab-active"></asp:LinkButton>
                </li>
                <li>
                    <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Backup %>"
                        CommandArgument="SEC_ActionPanel" TabIndex="80" OnClick="ActionHandler" CommandName="NEW"
                        CssClass="tab-inactive"></asp:LinkButton>
                </li>
            </ul>
        </div>
        <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
            <asp:TableRow ID="PageAction_List" runat="server">
                <asp:TableCell>
                    <div class="gridwrap">
                        <asp:GridView runat="server" ID="grdBackupList" Width="100%" AllowSorting="True"
                            OnSorting="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:RadioButton CssClass="rbtSelect" TabIndex="54" runat="server" GroupName="SelectOne"
                                            ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                        <asp:HiddenField ID="hdfBackupFileFullName" runat="server" Value='<%# Eval(Resources.DataFieldRes.BackupFileFullName)%>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:CreatedDate %>" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreatedDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.CreatedDate, Resources.Constants.DateFormatGridExpanded)  %>'
                                            ToolTip='<%# Eval(Resources.DataFieldRes.CreatedDate, Resources.Constants.DateFormatGridExpanded)%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="12%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:FileName %>" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblBackupFileName" runat="server" Text='<%# Eval(Resources.DataFieldRes.BackupFileName)%>'
                                            ToolTip='<%# Eval(Resources.DataFieldRes.BackupFileFullName)%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="75%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:BackupFileSize %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBackupFileSize" runat="server" Text='<%# Eval(Resources.DataFieldRes.BackupFileSize) %>'
                                            ToolTip='<%# Eval(Resources.DataFieldRes.BackupFileSize) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="12%"/>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                <asp:TableCell>
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label runat="server" ID="lblServerName" AssociatedControlID="txtServerName"
                                        Text="<%$resources:ServerName %>" />
                                    <asp:TextBox runat="server" ID="txtServerName" Text="" ReadOnly="true" />
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label runat="server" ID="lblDatabase" AssociatedControlID="txtDatabase" Text="<%$resources:Database %>" />
                                    <asp:TextBox runat="server" ID="txtDatabase" ReadOnly="true" />
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="divcol-S">
                                    <asp:Label runat="server" ID="lblBackUpTo" AssociatedControlID="txtBackUpTo" Text="<%$resources:BackupTo %>" />
                                    <asp:TextBox runat="server" ID="txtBackUpTo" ReadOnly="true" />
                                    <div class="clear">
                                    </div>
                                    <asp:Label runat="server" ID="lblBackupFileName" AssociatedControlID="txtBackupFileName"
                                        Text="<%$resources:BackupFileName %>" />
                                    <asp:TextBox runat="server" ID="txtBackupFileName" TabIndex="1" MaxLength="100" 
                                        onkeydown="limitText(this,100);" onkeyup="limitText(this,100);" onpaste="limitText(this,100);"/>

                                    <asp:RequiredFieldValidator ID="reqBackupFileName" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="backup" EnableClientScript="true" runat="server" ControlToValidate="txtBackupFileName"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ReqBackupFile %>" />
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <div id="diverror" style="display: none">
            <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
            <asp:ValidationSummary ID="vsPage" ValidationGroup="backup" runat="server" />
        </div>
    </div>
</asp:Content>
