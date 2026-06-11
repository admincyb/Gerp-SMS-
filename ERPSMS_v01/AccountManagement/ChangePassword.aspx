<%@ Page Title="<%$ Resources:Captions,Title_ChangePassword %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="ChangePassword.aspx.cs"
    Inherits="ERPSMS_v01.AccountManagement.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function ValidateNow() {
            if (typeof (Page_ClientValidate) == 'function') { //Test
                Page_ClientValidate();
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
        function Showchangepwd(message, title) {
            var errorTitle;
            var msg;
            errorTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = message ? message : '';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                title: errorTitle,
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

        //        Stregth Password
        $(document).ready(function () {
            $("[id$=txtPassword]").keyup(function () {
                $("[id$=password_strength]").html(checkStrength($("[id$=txtPassword]").val()))
            })
            function checkStrength(Password) {
                var strength = 0
                if (Password.length < 6) {
                    $("[id$=password_strength]").removeClass()
                    $("[id$=password_strength]").addClass('short')
                    $("[id$=password_strength]").addClass('pwd-short')
                    return 'Too short'
                }
                if (Password.length > 7) strength += 1
                // If Password contains both lower and uppercase characters, increase strength value.
                if (Password.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/)) strength += 1
                // If it has numbers and characters, increase strength value.
                if (Password.match(/([a-zA-Z])/) && Password.match(/([0-9])/)) strength += 1
                // If it has one special character, increase strength value.
                if (Password.match(/([!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1
                // If it has two special characters, increase strength value.
                if (Password.match(/(.*[!,%,&,@,#,$,^,*,?,_,~].*[!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1
                // Calculated strength value, we can return messages
                // If value is less than 2
                if (strength < 2) {
                    $("[id$=password_strength]").removeClass()
                    $("[id$=password_strength]").addClass('weak')
                    $("[id$=password_strength]").addClass('pwd-weak')
                    return 'Weak'
                } else if (strength == 2) {
                    $("[id$=password_strength]").removeClass()
                    $("[id$=password_strength]").addClass('good')
                    $("[id$=password_strength]").addClass('pwd-good')
                    return 'Good'
                } else {
                    $("[id$=password_strength]").removeClass()
                    $("[id$=password_strength]").addClass('strong')
                    $("[id$=password_strength]").addClass('pwd-strong')
                    return 'Strong'
                }
            }
        });
    </script>
    <asp:UpdatePanel runat="server" ID="aupdpnlCountry">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div id="divBtnContainer" runat="server" class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <div class="content-wrapper">
                                <table class="table-devide">
                                    <tr id="trExpiry" runat="server" visible="false" >
                                        <td>
                                           <div  style="background: #ecedee;  padding: 3px 2px; margin-bottom:20px;  border: 1px solid #dedfdf;  min-height: 14px;    line-height: normal;min-width:100%!important;">
                                                <asp:Label ID="lblPwxExpiry" runat="server" Text="<%$ resources:PwdExpired %>"  CssClass="margnbotm0 txt-center col-100 bold"></asp:Label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="divcolmiddle-S">
                                                <asp:Label ID="lbloldPwd" runat="server" Text="<%$ resources:Old_Pwd %>" AssociatedControlID="txtoldPassword"></asp:Label>
                                                <asp:TextBox ID="txtoldPassword" runat="server" TextMode="Password" MaxLength="50"
                                                    Width="140px" TabIndex="1" onpaste="return false;"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfOldPassword" runat="server" ErrorMessage="<%$ resources:Err_Old_Pwd %>"
                                                    ValidationGroup="change" ControlToValidate="txtoldPassword" Display="Dynamic"
                                                    EnableClientScript="true" Text="*" CssClass="star" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblPwd" runat="server" Text="<%$ resources:New_Pwd %>" AssociatedControlID="txtPassword"></asp:Label>
                                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="50" Width="140px"
                                                    TabIndex="2" onpaste="return false;"></asp:TextBox>
                                                <div class="starwrap">
                                                    <asp:RegularExpressionValidator Text="" ID="regPwd" runat="server" EnableClientScript="true"
                                                        ControlToValidate="txtPassword" ErrorMessage="Invalid Password format!" ValidationExpression="[\w\.\*\@\-]{1,30}"
                                                        ValidationGroup="change" Display="None"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="vrePassword" runat="server" ControlToValidate="txtPassword"
                                                        ErrorMessage="<%$ resources:Msg_MinLen %>" ValidationExpression="^.{7,20}$" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="change" />
                                                    <asp:RequiredFieldValidator ID="vrfPassword" runat="server" ErrorMessage="<%$ resources:Err_New_Pwd %>"
                                                        ValidationGroup="change" CssClass="star" ControlToValidate="txtPassword" Display="Dynamic"
                                                        EnableClientScript="true" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    <asp:CompareValidator ID="cmpoldnewPassword" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="change" EnableClientScript="true" runat="server" ControlToValidate="txtPassword"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PwdOldNewSame %>"
                                                        Operator="NotEqual" ControlToCompare="txtoldPassword"></asp:CompareValidator>
                                                </div>
                                                <span id="password_strength" class="style-none"></span>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblConfirmPwd" runat="server" Text="<%$ resources:Confirm_Pwd %>"
                                                    AssociatedControlID="txtConfirmPassword"></asp:Label>
                                                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" TabIndex="3"
                                                    Width="140px" MaxLength="50" onpaste="return false;"></asp:TextBox>
                                                <div class="starwrap">
                                                    <asp:CompareValidator ID="cmpConfirmPassword" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="change" EnableClientScript="true" runat="server" ControlToValidate="txtConfirmPassword"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Pwd %>" Operator="Equal"
                                                        ControlToCompare="txtPassword"></asp:CompareValidator>
                                                    <asp:RequiredFieldValidator ID="vrfConfirmPassword" runat="server" ErrorMessage="<%$ resources:Err_Confirm_Pwd %>"
                                                        CssClass="star" Text="*" ValidationGroup="change" EnableClientScript="true" ControlToValidate="txtConfirmPassword"
                                                        Display="Dynamic" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="Label1" Text="" AssociatedControlID="btnChange"></asp:Label>
                                                <asp:Button runat="server" ID="btnChange" ValidationGroup="change" CommandName="CHANGE"
                                                    TabIndex="4" Text="<%$ Resources:Controls,Change %>" OnClick="ActionHandler"
                                                    OnClientClick="javascript:ValidateNow()" CommandArgument="CHANGE" />
                                                <asp:Button runat="server" ID="btnCanel" CommandName="CANCEL" Text="<%$ Resources:Controls,Cancel %>"
                                                    TabIndex="5" OnClick="ActionHandler" CommandArgument="CANCEL" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-devide" style="display: none">
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary
                        ID="vsPage" ValidationGroup="Employee" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
