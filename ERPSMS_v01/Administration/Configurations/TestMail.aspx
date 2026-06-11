<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" Theme="Classic"
CodeBehind="TestMail.aspx.cs" Inherits="ERPSMS_v01.Administration.Configurations.TestMail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript">
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
                window.location = "Login.aspx";
            },
            buttons: {
                OK: function (e) {
                    $(this).dialog("close");
                }

            }
        });
        return false;
    }
    </script>

    <asp:UpdatePanel ID="auplDetailList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="margin-top: 100px">
            </div>
            <div class="datawrap">
                <%--use the width property of the below table corresponding to the contents in the page--%>
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks" Width="70%">
                    <%--Rename this ID Page_Entry with the corresponding section Id in the documet--%>
                    <asp:TableRow ID="ChangePwd_Entry" runat="server">
                        <%--Align table cell according to design--%>
                        <asp:TableCell style="width:100%">
                            <div class="contentwrapper" style="float:left;width:100%">
                                <%--All the controls will be placed here --%>
                                
                                <asp:Label ID="lblMailID" runat="server" Text="<%$ resources:Mail_ID %>" Width="150px"></asp:Label>
                                <asp:TextBox ID="txtMailID" runat="server"  MaxLength="50" CssClass="medium" TabIndex="2" Width="200px"
                                    onpaste="return false;"></asp:TextBox>
                                    <asp:RegularExpressionValidator Text="" ID="regPwd" runat="server" EnableClientScript="true"
				ControlToValidate="txtMailID" ErrorMessage="Invalid email format!" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
				ValidationGroup="change" Display="None"></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="vrfMail" runat="server" ErrorMessage="<%$ resources:Err_Required%>"
                                    ValidationGroup="change" CssClass="star" ControlToValidate="txtMailID" Display="Dynamic"
                                    EnableClientScript="true" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <asp:Button runat="server" ID="btnChange" ValidationGroup="change" CommandName="CHANGE" TabIndex="4"
                                            Text="<%$ resources: Send %>" OnClick="ActionHandler" OnClientClick="javascript:ValidateNow()"
                                             />
                                        <asp:Button runat="server" ID="btnCanel" CommandName="CANCEL" Text="<%$ Resources:ErpRes, Cancel %>" TabIndex="5"
                                            OnClick="ActionHandler"  />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <%--Rename this ID Page_List with the corresponding section Id in the documet--%>
                    <asp:TableRow ID="Page_List">
                        <asp:TableCell>
                            <%-- Leave this table as such --%>
                            <asp:Table ID="Table1" runat="server">
                                <asp:TableRow>
                                    <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                                    <asp:TableCell CssClass="SEC_ACTION">
                                        <asp:Label runat="server" ID="Label1" Text=""></asp:Label>
                                        
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <%--Use this label to bind the server errors--%>
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vvsPage" ValidationGroup="change" runat="server" />
            </div>
            <div id="divButton" style="display: none">
                <asp:Button runat="server" ID="btnOk" Text="OK" />
            </div>
            <div id="popupHolder1">
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
