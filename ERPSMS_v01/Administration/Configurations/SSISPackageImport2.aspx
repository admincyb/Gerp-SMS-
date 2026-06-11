<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="SSISPackageImport2.aspx.cs" Inherits="ERPSMS_v01.Administration.Configurations.SSISPackageImport2"
    Theme="Classic"  %>
<%--meta:resourcekey="PageResource1"--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
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
                        <ul runat="server" id="pnlEntry">
                            <li>
                                <asp:Button runat="server" ID="btnImport" CommandName="IMPORT" TabIndex="2" Text="<%$resources:Import %>"
                                    OnClick="ActionHandler" ToolTip="<%$resources:Import %>" SkinID="btnInner-add"
                                    ValidationGroup="Import" OnClientClick="javascript:ValidatePageNow('Import')" />
                                <asp:Button runat="server" ID="btnClear" CommandName="CLEAR" TabIndex="2" Text="<%$resources:Clear %>"
                                    OnClick="ActionHandler" ToolTip="<%$resources:Clear %>" SkinID="btnInner-add" />
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
        <table class="table-devide">
            <tr>
                <td>
                    <div class="div2col-S">
                        <asp:Label runat="server" ID="lblFileName" AssociatedControlID="fupExcelFileUploder"
                            Text="<%$resources:FileName %>" />
                        <asp:FileUpload ID="fupExcelFileUploder" runat="server" />
                        <asp:RequiredFieldValidator ID="reqExcelFileUploder" CssClass="star" SetFocusOnError="true"
                            ValidationGroup="Import" EnableClientScript="true" runat="server" ControlToValidate="fupExcelFileUploder"
                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Excel_Not_Select %>">
                        </asp:RequiredFieldValidator>
                        <div class="clear">
                        </div>
                    </div>
                </td>
                <td>
                    <div class="div2col-S">
                        <asp:Label runat="server" ID="lblPackage" AssociatedControlID="ddlPackage" Text="<%$resources:Package %>" />
                        <asp:DropDownList runat="server" ID="ddlPackage">
                            <asp:ListItem Text="Select" Value="-1" />
                            <asp:ListItem Text="Product" Value="0" />
                            <asp:ListItem Text="Brand" Value="1" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqPackage" CssClass="star" SetFocusOnError="true"
                            InitialValue="-1" ValidationGroup="Import" EnableClientScript="true" runat="server"
                            ControlToValidate="ddlPackage" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Package_Not_Select %>">
                        </asp:RequiredFieldValidator>
                        <div class="clear">
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="div2col-S">
                        <asp:Label runat="server" ID="lblDestinationDB" AssociatedControlID="txtDestinationDB"
                            Text="<%$resources:DestinationDB %>" />
                        <asp:TextBox runat="server" ID="txtDestinationDB" Enabled="false" CssClass="input-disabled" />
                        <asp:RequiredFieldValidator ID="reqDestinationDB" CssClass="star" SetFocusOnError="true"
                            ValidationGroup="Import" EnableClientScript="true" runat="server" ControlToValidate="txtDestinationDB"
                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ReqDestinationDb %>" />
                        <div class="clear">
                        </div>
                    </div>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div style="width: 900px; height: 500px; overflow: scroll;">
                        <div runat="server" id="divMessage">
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="diverror" style="display: none">
        <%--Use this label to bind the server errors--%>
        <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
        <asp:ValidationSummary ID="vsPage" ValidationGroup="Import" runat="server" />
    </div>
</asp:Content>
