<%@ Page Title="<%$ Resources:Captions,Title_ChangePassword %>"  Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="ERPSMS_v01.AccountManagement.ResetPassword" Theme="Classic"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>gERP – STORE MANAGEMENT</title>
</head>
<body>
   <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="loginScriptManager" CompositeScript-ScriptMode="Release"
        LoadScriptsBeforeUI="true" ScriptMode="Release">
        <CompositeScript>
            <Scripts>
                <asp:ScriptReference Path="~/Scripts/jquery/jquery-1.5.min.js" ScriptMode="Auto" />
                <asp:ScriptReference Path="~/Scripts/Jquery/jquery-ui.min.js" ScriptMode="Auto" />
                <asp:ScriptReference Path="~/Scripts/PageScript/MasterPage.js" ScriptMode="Auto" />
            </Scripts>
        </CompositeScript>
    </asp:ScriptManager>
    <asp:UpdateProgress runat="server" ID="updateProgress">
        <ProgressTemplate>
            <div class="overlay">
                <!--<div class="ui-widget ui-widget-content ui-corner-all" style="z-index:5000; position:relative; width:200px; height:auto; padding:10px;top:expression(($.position().top/2)+'px'); left:expression((document.body.clientWidth/2)+'px')">Progressing</div>-->
                <table id="wrapper" class="ui-widget-overlay" style="width: 100%; height: 100%">
                    <tr>
                        <td>
                            <asp:Image ImageUrl="~/images/Classic/layout/loader-img.gif" runat="server" ID="imgLoader" />
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="auplDetailList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="diverror" style="display: none">
                <%--Use this label to bind the server errors--%>
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vvsPage" ValidationGroup="AuthValidation" runat="server" />
            </div>
            <div id="#content-container" class="datawrap">
                <div class="reset-container">
                <h1>Forgot Password </h1>
                <div class="rest-fields">
                    <asp:Label ID="lblUser" runat="server" Text="<%$ resources: User_Id %>"></asp:Label>
                    <asp:TextBox runat="server" ID="txtUserID" ClientIDMode="Static" Width="160px" MaxLength="30"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqUserName" ControlToValidate="txtUserID" ErrorMessage="<%$ resources:Err_UserId %>"
                        Display="None" EnableClientScript="true" runat="server" ValidationGroup="AuthValidation"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regUserName" runat="server" EnableClientScript="true"
                        ControlToValidate="txtUserID" Display="None" ErrorMessage="Invalid UserID format!"
                        ValidationExpression="\w{1,10}" ValidationGroup="AuthValidation"></asp:RegularExpressionValidator>
                    <asp:Button runat="server" ID="btnReset" Text="<%$ resources: Reset %>" CommandName="CHANGE"
                        OnClick="ActionHandler"  ValidationGroup="AuthValidation" />
                    <asp:Button runat="server" ID="btnResetCancel" Text="<%$ Resources:ErpRes, Cancel %>"
                        CommandName="CANCEL" OnClick="ActionHandler" />
                        </div>
                        <div class="clear"></div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="error" style="display: none">
    </div>
    <div id="popupHolder">
    </div>
    <div id="divConfirmation">
    </div>
    </form>
    <script type="text/javascript">

        function ShowDeleteConfirm(btn, message) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = message ? message : '<%= Resources.ErpRes.Msg_Delete_Confirm %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $(this).dialog("close");
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

        errorTitle = '<%= Resources.ErpRes.Title_Information.ToString() %>';
        function ValidateNow() {
            if (typeof (Page_ClientValidate) == 'function') {
                Page_ClientValidate();
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                // return true;
                return ShowDeleteConfirm($("[id$=btnReset]")[0], '<%= GetLocalResourceObject("ConfirmAction") %>');
            }
        }
        function ShowChangeLoc(message, title) {
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';

            msg = message ? message : '';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                title: msgTitle,
                resizable: false,
                beforeClose: function () {
                    window.location = "../Login.aspx";
                },
                buttons: {
                    OK: function (e) {
                        $(this).dialog("close");


                    }

                }
            });
            return false;
        }

        function logout() {
            window.location = "../Login.aspx";
        }
          
    </script>
</body>
</html>
