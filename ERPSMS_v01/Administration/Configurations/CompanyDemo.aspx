<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    Theme="Classic" CodeBehind="CompanyDemo.aspx.cs" Inherits="ERPSMS_v01.Administration.Configurations.CompanyDemo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function InitComponents() {
        }
        //For finding and removing duplicate and other group validation controls
        //Array of present validations
        var validationArrayGroup;
        function CheckValidationDuplicate(valGroup) {
            validationArrayGroup = new Array();
            //Traversing from bottom through all the validation controls in the page
            for (var i = Page_Validators.length - 1; i >= 0; i--) {
                if (typeof (Page_Validators[i].validationGroup) == "string") {
                    if (valGroup == Page_Validators[i].validationGroup) {
                        //checks if the control is already in the validation array
                        if (!CheckValidationExists(Page_Validators[i].id)) {
                            //insert new conrol to the Array of present validations
                            validationArrayGroup.push(Page_Validators[i].id);
                        }
                        //remove if control is already in Array of present validations
                        else {
                            Page_Validators.splice(i, 1);
                        }
                    }
                    //remove control if not in group
                    else {
                        Page_Validators.splice(i, 1);
                    }
                }
            }
        }
        //For checking if validation control in Array of present validations
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
                //For finding and removing duplicate and other group validation controls
                //CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
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

        function ShowDeleteConfirm(btn, message) {
            var msgTitle;
            var msg;
            msgTitle = "<%= Resources.ErpRes.Title_Information %>";
            msg = message ? message : "<%= Resources.ErpRes.MsgDeleteConfirm %>";
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

     
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="fixed-buttons-normal">
        <div class="Button-container">
            <asp:Table runat="server" ID="tblButton">
                <asp:TableRow>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul class="bredcrum">
                            <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                        </ul>
                        <ul runat="server" id="pnlListing">
                            <li>
                                <asp:Button ID="btnSave" runat="server" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    OnClientClick="javascript:return ValidatePageNow('vgCompany')" CommandName="SAVE"
                                    TabIndex="19" OnClick="ActionHandler" ToolTip="Save" />
                            </li>
                            <li>
                                <asp:Button ID="btnDelete" runat="server" Visible="false" SkinID="btnInner-Delete"
                                    Text="<%$Resources:Controls,Delete%>" OnClientClick="javascript:return ResetPage();"
                                    TabIndex="20" ToolTip="Delete" CommandName="DELETE" OnClick="ActionHandler" />
                            </li>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    CommandName="CANCEL" OnClick="ActionHandler" TabIndex="21" ToolTip="Cancel" /><%--OnClientClick="javascript:return CancelFun();"--%>
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <div id="grdTable-wrap">
            <%--  <div id="divData">--%>
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S">
                            <%-- <div class="content">--%>
                            <asp:Label ID="lblCompanyCode" runat="server" AssociatedControlID="txtCompanyCode"
                                Text="<%$ resources:CompanyCode %>" />
                            <asp:TextBox runat="server" ID="txtCompanyCode" TabIndex="1" onkeydown="limitText(this,100);"
                                onkeyup="limitText(this,100);" MaxLength="100" />
                            <div class="starwrap">
                                <asp:RequiredFieldValidator ID="vrfCompanyCode" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="vgCompany" EnableClientScript="true" runat="server" ControlToValidate="txtCompanyCode"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_CompanyCode %>" />
                                <%--<asp:RegularExpressionValidator ID="vreUserName" runat="server" ControlToValidate="txtUserName"
                                    ErrorMessage="<%$ resources:Msg_Vali_User %>" ValidationExpression="[^\s]+" Display="Dynamic"
                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="vgUser" />--%>
                            </div>
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblAddress1" runat="server" AssociatedControlID="txtAddress1" Text="<%$ resources:Address1 %>" />
                            <asp:TextBox runat="server" ID="txtAddress1" MaxLength="200" onkeydown="limitText(this,200);"
                                onkeyup="limitText(this,200);" TabIndex="3" TextMode="Multiline" EnableViewState="true" />
                            <div class="starwrap">
                                <asp:RequiredFieldValidator ID="vrfAddress" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="vgCompany" EnableClientScript="true" runat="server" ControlToValidate="txtAddress1"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_Address1 %>" />
                                <%--<asp:RegularExpressionValidator ID="vrePassword" runat="server" ControlToValidate="txtPassword"
                                    ErrorMessage="<%$ resources:Msg_MinLen %>" ValidationExpression="^.{7,20}$" Display="Dynamic"
                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="vgUser" />--%>
                            </div>
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblLocalLanguageName" runat="server" AssociatedControlID="txtLocalLanguageName"
                                Text="<%$ resources:LocalLanguageName %>" />
                            <asp:TextBox runat="server" ID="txtLocalLanguageName" MaxLength="200" onkeydown="limitText(this,200);"
                                onkeyup="limitText(this,200);" TabIndex="5" EnableViewState="true" />
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblCity" runat="server" AssociatedControlID="txtCity" Text="<%$ resources:City %>" />
                            <asp:TextBox runat="server" ID="txtCity" MaxLength="25" onkeydown="limitText(this,25);"
                                onkeyup="limitText(this,25);" TabIndex="7" />
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblCountry" runat="server" AssociatedControlID="ddlCountry" Text="<%$ resources:Country %>" />
                            <asp:DropDownList runat="server" TabIndex="8" AutoPostBack="true" ID="ddlCountry"
                                OnSelectedIndexChanged="ActionHandler">
                            </asp:DropDownList>
                            <div class="starwrap">
                                <asp:RequiredFieldValidator ID="vrfCountry" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="vgCompany" EnableClientScript="true" runat="server" ControlToValidate="ddlCountry"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Country %>" InitialValue="-1">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblState" runat="server" AssociatedControlID="ddlState" Text="<%$ resources:State %>" />
                            <asp:DropDownList runat="server" TabIndex="9" ID="ddlState">
                                <%--OnSelectedIndexChanged="ActionHandler"--%>
                            </asp:DropDownList>
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblZipCode" runat="server" AssociatedControlID="txtZipCode" Text="<%$ resources:ZipCode %>" />
                            <asp:TextBox runat="server" ID="txtZipCode" MaxLength="100" TabIndex="11" />
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblFax" runat="server" AssociatedControlID="txtFax" Text="<%$ resources:Fax %>" />
                            <asp:TextBox runat="server" ID="txtFax" MaxLength="100" TabIndex="13" />
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" Text="<%$ resources:Email %>" />
                            <asp:TextBox runat="server" ID="txtEmail" MaxLength="100" TabIndex="15" />
                            <div class="starwrap">
                                <%-- <asp:RequiredFieldValidator ID="vrfEmail" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="vgUser" EnableClientScript="true" runat="server" ControlToValidate="txtEmail"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_Email %>" />--%>
                                <asp:RegularExpressionValidator ID="vreEmail" runat="server" ControlToValidate="txtEmail"
                                    ErrorMessage="<%$ resources:Msg_Valid_Email %>" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="vgCompany" />
                            </div>
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblCurrency" runat="server" AssociatedControlID="ddlCurrency" Text="<%$ resources:Currency %>" />
                            <asp:DropDownList runat="server" TabIndex="17" ID="ddlCurrency">
                            </asp:DropDownList>
                            <div class="starwrap">
                                <asp:RequiredFieldValidator ID="vrfCurrency" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="vgCompany" EnableClientScript="true" runat="server" ControlToValidate="ddlCurrency"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Currency %>" InitialValue="-1">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblLogo" runat="server" AssociatedControlID="lblLogoName" Text="<%$ resources:Controls, Logo %>" />
                            <asp:Label ID="lblLogoName" runat="server" />
                            <%-- <asp:Button ID="btnDeleteLogo" runat="server" SkinID="delete-icon" CommandName="DELETEITEM"
                                ToolTip="<%$ resources:DeleteLogo %>"/><%-- OnClick="ActionHandler" -- %>--%>
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblLogoUpload" runat="server" AssociatedControlID="fudLogo" />
                            <asp:FileUpload ID="fudLogo" runat="server" TabIndex="18" />
                            <div class="clear">
                            </div>
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S">
                            <%--<div class="content">--%>
                            <asp:Label ID="lblCompanyName" runat="server" AssociatedControlID="txtCompanyName"
                                Text="<%$ resources:CompanyName %>" />
                            <asp:TextBox runat="server" ID="txtCompanyName" TabIndex="2" MaxLength="200" onkeydown="limitText(this,200);"
                                onkeyup="limitText(this,200);" />
                            <div class="starwrap">
                                <asp:RequiredFieldValidator ID="vrfCompanyName" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="vgCompany" EnableClientScript="true" runat="server" ControlToValidate="txtCompanyName"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_CompanyName %>" />
                                <%--<asp:RegularExpressionValidator ID="vreUserName" runat="server" ControlToValidate="txtUserName"
                                    ErrorMessage="<%$ resources:Msg_Vali_User %>" ValidationExpression="[^\s]+" Display="Dynamic"
                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="vgUser" />--%>
                            </div>
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblAddress2" runat="server" AssociatedControlID="txtAddress2" Text="<%$ resources:Address2 %>" />
                            <asp:TextBox runat="server" ID="txtAddress2" MaxLength="200" onkeydown="limitText(this,200);"
                                onkeyup="limitText(this,200);" TabIndex="4" TextMode="Multiline" EnableViewState="true" />
                            <%--<div class="starwrap">
                                <asp:RequiredFieldValidator ID="vrfPassword" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="vgUser" EnableClientScript="true" runat="server" ControlToValidate="txtPassword"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_Password %>" />
                                <asp:RegularExpressionValidator ID="vrePassword" runat="server" ControlToValidate="txtPassword"
                                    ErrorMessage="<%$ resources:Msg_MinLen %>" ValidationExpression="^.{7,20}$" Display="Dynamic"
                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="vgUser" />
                            </div>--%>
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblAddress3" runat="server" AssociatedControlID="txtLocalLanguageAddress"
                                Text="<%$ resources:LocalLanguageAddress %>" />
                            <asp:TextBox runat="server" ID="txtLocalLanguageAddress" MaxLength="200" onkeydown="limitText(this,200);"
                                onkeyup="limitText(this,200);" TabIndex="6" TextMode="Multiline" EnableViewState="true" />
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblPhone" runat="server" AssociatedControlID="txtPhone" Text="<%$ resources:Phone %>" />
                            <asp:TextBox runat="server" ID="txtPhone" MaxLength="25" onkeydown="limitText(this,25);"
                                onkeyup="limitText(this,25);" TabIndex="10" />
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblMobile" runat="server" AssociatedControlID="txtMobile" Text="<%$ resources:Mobile %>" />
                            <asp:TextBox runat="server" ID="txtMobile" MaxLength="25" onkeydown="limitText(this,25);"
                                onkeyup="limitText(this,25);" TabIndex="12" />
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblTaxNo" runat="server" AssociatedControlID="txtTaxNo" Text="<%$ resources:TaxNo %>" />
                            <asp:TextBox runat="server" ID="txtTaxNo" TabIndex="14" />
                            <%-- MaxLength="25" onkeydown="limitText(this,25);"
                                onkeyup="limitText(this,25);"--%>
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblWebsite" runat="server" AssociatedControlID="txtTaxNo" Text="<%$ resources:Website %>" />
                            <asp:TextBox runat="server" ID="txtWebsite" TabIndex="16" />
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblProductVersion" runat="server" AssociatedControlID="txtTaxNo" Text="<%$ resources:ProductVersion %>" />
                            <asp:TextBox runat="server" ID="txtProductVersion" TabIndex="16" />
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblGAFVersion" runat="server" AssociatedControlID="txtTaxNo" Text="<%$ resources:GAFVersion %>" />
                            <asp:TextBox runat="server" ID="txtGAFVersion" TabIndex="16" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="diverror" style="display: none">
        <%--Use this label to bind the server errors--%>
        <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star" />
        <asp:ValidationSummary ID="vvsUser" ValidationGroup="vgCompany" runat="server" />
    </div>
</asp:Content>
