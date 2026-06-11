<%@ Page Title="<%$ Resources:Captions,Title_Port %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="PortMaster.aspx.cs" Inherits="ERPSMS_v01.GeneralAdmin.PortMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function InitComponents() {

        }
        function InitDate() {
            GrandScriptUtils.DatePickerCommon("txtFinStartDate", "dd-M");
            GrandScriptUtils.DatePickerCommon("txtFinEndDate", "dd-M");
        }
        function ValidateNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                CheckValidationDuplicate(valGroup); //For finding and removing duplicate and other group validation controls
                Page_ClientValidate(valGroup); //For Script validating the Page
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverrorAlert").html());
                return false; //Page is invalid -- stop right here
            }
            else {
                return true; //everythings ok --- Call your function & do your stuff
            }
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


        function ShowHideDisplayCode() {
            if ($("[id$=hdfIsMultiplePlant]").val() == "1") {
                $("#divDisplayCode").show();
            }
            else {
                $("#divDisplayCode").hide();
            }
        }

    </script>
    <style type="text/css">
        .ui-datepicker-year
        {
            display: none;
        }
        
        .footer-content
        {
            margin: 0px auto;
            width: 100%;
            text-align: center;
            line-height: 35px;
            border-top: 1px solid #ddd;
        }
    </style>
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
                                    OnClientClick="javascript:return ValidateNow('vgPort')" CommandName="SAVE"
                                    TabIndex="23" OnClick="ActionHandler" ToolTip="Save" />
                            </li>
                            <li>
                                <asp:Button ID="btnDelete" runat="server" Visible="false" SkinID="btnInner-Delete"
                                    Text="<%$Resources:Controls,Delete%>" OnClientClick="javascript:return ResetPage();"
                                    TabIndex="24" ToolTip="Delete" CommandName="DELETE" OnClick="ActionHandler" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                    OnClick="ActionHandler" CommandName="CANCEL" SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>"
                                    TabIndex="25" />
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
                            <asp:Label ID="lblPortCode" runat="server" AssociatedControlID="txtPortCode" Text="<%$ resources:PortCode %>" />
                            <asp:TextBox runat="server" ID="txtPortCode" TabIndex="1" onkeydown="limitText(this,100);"
                                onkeyup="limitText(this,100);" MaxLength="100" CssClass="input-half" />
                            <div class="starwrap">
                                <asp:RequiredFieldValidator ID="vrfPortCode" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="vgPort" EnableClientScript="true" runat="server" ControlToValidate="txtPortCode"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_PortCode %>" />
                                <%--<asp:RegularExpressionValidator ID="vreUserName" runat="server" ControlToValidate="txtUserName"
                                    ErrorMessage="<%$ resources:Msg_Vali_User %>" ValidationExpression="[^\s]+" Display="Dynamic"
                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="vgUser" />--%>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S">
                            <asp:Label ID="lblPortName" runat="server" AssociatedControlID="txtPortName" Text="<%$ resources:PortName %>" />
                            <asp:TextBox runat="server" ID="txtPortName" TabIndex="2" MaxLength="200" onkeydown="limitText(this,200);"
                                onkeyup="limitText(this,200);" CssClass="input-half" />
                            <div class="starwrap">
                                <asp:RequiredFieldValidator ID="vrfPortName" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="vgPort" EnableClientScript="true" runat="server" ControlToValidate="txtPortName"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_PortName %>" />
                                <%--<asp:RegularExpressionValidator ID="vreUserName" runat="server" ControlToValidate="txtUserName"
                                    ErrorMessage="<%$ resources:Msg_Vali_User %>" ValidationExpression="[^\s]+" Display="Dynamic"
                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="vgUser" />--%>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="div2col-S">
                            <asp:Label ID="lblType" runat="server" AssociatedControlID="ddlType" Text="<%$ resources:Type %>" />
                            <asp:DropDownList runat="server" TabIndex="3" AutoPostBack="true" ID="ddlType" OnSelectedIndexChanged="ActionHandler"
                                CssClass="select-half-a">
                            </asp:DropDownList>
                            <div class="starwrap">
                                <asp:RequiredFieldValidator ID="vrfPort" CssClass="star" SetFocusOnError="true" ValidationGroup="vgPort"
                                    EnableClientScript="true" runat="server" ControlToValidate="ddlType" Display="Dynamic"
                                    Text="*" ErrorMessage="<%$ resources:Err_Type %>" InitialValue="-1">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="div2col-S">
                            <asp:Label ID="lblCountry" runat="server" AssociatedControlID="ddlCountry" Text="<%$ resources:Country %>" />
                            <asp:DropDownList runat="server" TabIndex="5" AutoPostBack="true" ID="ddlCountry"
                                OnSelectedIndexChanged="ActionHandler" CssClass="select-half-a">
                            </asp:DropDownList>
                            <div class="starwrap">
                                <asp:RequiredFieldValidator ID="vrfCountry" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="vgPort" EnableClientScript="true" runat="server" ControlToValidate="ddlCountry"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Country %>" InitialValue="-1">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="div2col-S">
                            <asp:Label ID="lblState" runat="server" AssociatedControlID="ddlState" Text="<%$ resources:State %>" />
                            <asp:DropDownList runat="server" TabIndex="6" ID="ddlState" CssClass="select-half-a">
                            </asp:DropDownList>
                            <div class="starwrap">
                                <asp:RequiredFieldValidator ID="vrfState" CssClass="star" SetFocusOnError="true"
                                    Enabled="false" ValidationGroup="vgPort" EnableClientScript="true" runat="server"
                                    ControlToValidate="ddlState" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_State %>"
                                    InitialValue="-1">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S">
                            <asp:Label ID="lblAddress" runat="server" AssociatedControlID="txtAddress" Text="<%$ resources:Address %>" />
                            <asp:TextBox runat="server" ID="txtAddress" MaxLength="200" onkeydown="limitText(this,200);"
                                onkeyup="limitText(this,200);" TabIndex="4" TextMode="Multiline" EnableViewState="true"
                                CssClass="input-half multiline-2col" />
                            <%-- <div class="starwrap">
                                <asp:RequiredFieldValidator ID="vrfAddress" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="vgPort" EnableClientScript="true" runat="server" ControlToValidate="txtAddress"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_Address1 %>" />--%>
                            <%--<asp:RegularExpressionValidator ID="vrePassword" runat="server" ControlToValidate="txtPassword"
                                    ErrorMessage="<%$ resources:Msg_MinLen %>" ValidationExpression="^.{7,20}$" Display="Dynamic"
                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="vgUser" />--%>
                            <%--</div>--%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="div2col-S">
                            <asp:Label ID="lblCity" runat="server" AssociatedControlID="txtCity" Text="<%$ resources:City %>" />
                            <asp:TextBox runat="server" ID="txtCity" MaxLength="25" onkeydown="limitText(this,25);"
                                onkeyup="limitText(this,25);" TabIndex="7" CssClass="input-half" />
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S">
                            <asp:Label ID="lblPhone" runat="server" AssociatedControlID="txtPhone" Text="<%$ resources:Phone %>" />
                            <asp:TextBox runat="server" ID="txtPhone" MaxLength="25" onkeydown="limitText(this,25);"
                                onkeyup="limitText(this,25);" TabIndex="8" CssClass="input-half" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="div2col-S">
                            <asp:Label ID="lblZipCode" runat="server" AssociatedControlID="txtZipCode" Text="<%$ resources:ZipCode %>" />
                            <asp:TextBox runat="server" ID="txtZipCode" MaxLength="100" TabIndex="9" CssClass="input-half" />
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S">
                            <asp:Label ID="lblMobile" runat="server" AssociatedControlID="txtMobile" Text="<%$ resources:Mobile %>" />
                            <asp:TextBox runat="server" ID="txtMobile" MaxLength="35" onkeydown="limitText(this,35);"
                                onkeyup="limitText(this,35);" TabIndex="10" CssClass="input-half" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="div2col-S">
                            <asp:Label ID="lblFax" runat="server" AssociatedControlID="txtFax" Text="<%$ resources:Fax %>" />
                            <asp:TextBox runat="server" ID="txtFax" MaxLength="100" TabIndex="11" CssClass="input-half" />
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S">
                            <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" Text="<%$ resources:Email %>" />
                            <asp:TextBox runat="server" ID="txtEmail" MaxLength="100" TabIndex="16" CssClass="input-half" />
                            <div class="starwrap">
                                <%-- <asp:RequiredFieldValidator ID="vrfEmail" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="vgUser" EnableClientScript="true" runat="server" ControlToValidate="txtEmail"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_Email %>" />--%>
                                <asp:RegularExpressionValidator ID="vreEmail" runat="server" ControlToValidate="txtEmail"
                                    ErrorMessage="<%$ resources:Msg_Valid_Email %>" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="vgPort" />
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="div2col-S">
                            <asp:Label ID="lblSalesFrom" runat="server" AssociatedControlID="chkSalesFrom" Text="<%$ resources:IsSalesFrom %>" />
                            <asp:CheckBox ID="chkSalesFrom" runat="server" Checked="false" TabIndex="12" />
                            <asp:Label ID="lblSalesTo" runat="server" AssociatedControlID="chkSalesTo" Text="<%$ resources:IsSalesTo %>" />
                            <asp:CheckBox ID="chkSalesTo" runat="server" Checked="false" TabIndex="13" />
                            <asp:Label ID="lblActive" runat="server" AssociatedControlID="chkActive" Text="<%$ resources:Active %>" />
                            <asp:CheckBox runat="server" ID="chkActive" TabIndex="17" Checked="true" />
                            <asp:Label ID="lblPurFrom" runat="server" AssociatedControlID="chkPurFrom" Text="<%$ resources:IsPurFrom %>" />
                            <asp:CheckBox runat="server" ID="chkPurFrom" TabIndex="14" />
                            <asp:Label ID="lblPurTo" runat="server" AssociatedControlID="chkPurTo" Text="<%$ resources:IsPurTo %>" />
                            <asp:CheckBox runat="server" ID="chkPurTo" TabIndex="15" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="diverrorAlert" style="display: none">
        <%--Use this label to bind the server errors--%>
        <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star" />
        <asp:ValidationSummary ID="vvsUser" ValidationGroup="vgPort" runat="server" />
    </div>
    <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" />
</asp:Content>
