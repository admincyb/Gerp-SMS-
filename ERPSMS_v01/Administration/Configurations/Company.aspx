<%@ Page Title="<%$ Resources:Captions,Title_Company %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="Company.aspx.cs" Inherits="ERPSMS_v01.Administration.Configurations.Company" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function InitComponents() {

        }
        function InitDate() {
            GrandScriptUtils.DatePickerCommon("txtFinStartDate", "dd-M");
            GrandScriptUtils.DatePickerCommon("txtFinEndDate", "dd-M");
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


        function ShowHideDisplayCode() {
            if ($("[id$=hdfIsMultiplePlant]").val() == "1") {
                $("#divDisplayCode").show();
            }
            else {
                $("#divDisplayCode").hide();
            }
        }

        function ShowCompanyDtl() {
            //<summary>Function Used to Show Company Detail </summary>
            $("#imgCmpDtlHide").show();
            $("#imgCmpDtlShow").hide();
            // $("#divVendorDtl").show();
            $("#tblDtls").show();
        }

        function HideCompanyDtl() {
            //<summary>Function Used to Hide Company Detail </summary>
            $("#imgCmpDtlHide").hide();
            $("#imgCmpDtlShow").show();
            // $("#divVendorDtl").hide();
            $("#tblDtls").hide();
        }

        function ShowCompanyConfig() {
            //<summary>Function Used to Show Company Detail </summary>
            $("#imgCmpConfigHide").show();
            $("#imgCmpConfigShow").hide();
            // $("#divVendorDtl").show();
            $("#tblConfig").show();
        }

        function HideCompanyConfig() {
            //<summary>Function Used to Hide Company Detail </summary>
            $("#imgCmpConfigHide").hide();
            $("#imgCmpConfigShow").show();
            // $("#divVendorDtl").hide();
            $("#tblConfig").hide();
        }

        function ShowHideQASample(flag) {
            if (flag == 1) {
                //<summary>Function Used to Show Company Detail </summary>
                $("#imgQASampleHide").show();
                $("#imgQASampleShow").hide();
                // $("#divVendorDtl").show();
                $("#tblQASample").show();
            }
            else {
                //<summary>Function Used to Hide Company Detail </summary>
                $("#imgQASampleHide").hide();
                $("#imgQASampleShow").show();
                // $("#divVendorDtl").hide();
                $("#tblQASample").hide();
            }
        }

    </script>
    <style type="text/css">
        .ui-datepicker-year {
            display: none;
        }

        .footer-content {
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
                                    OnClientClick="javascript:return ValidatePageNow('vgCompany')" CommandName="SAVE"
                                    TabIndex="27" OnClick="ActionHandler" ToolTip="Save" />
                            </li>
                            <li>
                                <asp:Button ID="btnDelete" runat="server" Visible="false" SkinID="btnInner-Delete"
                                    Text="<%$Resources:Controls,Delete%>" OnClientClick="javascript:return ResetPage();"
                                    TabIndex="28" ToolTip="Delete" CommandName="DELETE" OnClick="ActionHandler" />
                            </li>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    CommandName="CANCEL" OnClick="ActionHandler" TabIndex="29" ToolTip="Cancel" /><%--OnClientClick="javascript:return CancelFun();"--%>
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <h1 class="search-colapse-normal">
            <%=Resources.Captions.Company%>
            <img id="imgCmpDtlShow" src="../../Images/Classic/Icons/arrow-colapse-inactive.png"
                alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowCompanyDtl();" />
            <img id="imgCmpDtlHide" src="../../Images/Classic/Icons/arrow-colapse-active.png"
                alt="Hide" title="Hide" style="cursor: pointer" onclick="javascript:HideCompanyDtl();" />
        </h1>
        <div id="grdTable-wrap">
            <%--  <div id="divData">--%>
            <table id="tblDtls" class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S">
                            <%-- <div class="content">--%>
                            <asp:Label ID="lblCompanyCode" runat="server" AssociatedControlID="txtCompanyCode"
                                Text="<%$ resources:CompanyCode %>" />
                            <asp:TextBox runat="server" ID="txtCompanyCode" TabIndex="1" onkeydown="limitText(this,100);"
                                onkeyup="limitText(this,100);" MaxLength="100" CssClass="input-half" />
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
                                onkeyup="limitText(this,200);" TabIndex="3" TextMode="Multiline" EnableViewState="true"
                                CssClass="input-half multiline-2col" />
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
                                onkeyup="limitText(this,200);" TabIndex="5" EnableViewState="true" CssClass="input-half" />
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblCity" runat="server" AssociatedControlID="txtCity" Text="<%$ resources:City %>" />
                            <asp:TextBox runat="server" ID="txtCity" MaxLength="25" onkeydown="limitText(this,25);"
                                onkeyup="limitText(this,25);" TabIndex="7" CssClass="input-half" />
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblCountry" runat="server" AssociatedControlID="ddlCountry" Text="<%$ resources:Country %>" />
                            <asp:DropDownList runat="server" TabIndex="8" AutoPostBack="true" ID="ddlCountry"
                                OnSelectedIndexChanged="ActionHandler" CssClass="select-half-a">
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
                            <asp:DropDownList runat="server" TabIndex="9" ID="ddlState" CssClass="select-half-a">
                                <%--OnSelectedIndexChanged="ActionHandler"--%>
                            </asp:DropDownList>
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblZipCode" runat="server" AssociatedControlID="txtZipCode" Text="<%$ resources:ZipCode %>" />
                            <asp:TextBox runat="server" ID="txtZipCode" MaxLength="100" TabIndex="11" CssClass="input-half" />
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblFax" runat="server" AssociatedControlID="txtFax" Text="<%$ resources:Fax %>" />
                            <asp:TextBox runat="server" ID="txtFax" MaxLength="100" TabIndex="13" CssClass="input-half" />
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" Text="<%$ resources:Email %>" />
                            <asp:TextBox runat="server" ID="txtEmail" MaxLength="100" TabIndex="15" CssClass="input-half" />
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
                            <asp:DropDownList runat="server" TabIndex="17" ID="ddlCurrency" CssClass="select-small-a">
                            </asp:DropDownList>
                            <div class="starwrap">
                                <asp:RequiredFieldValidator ID="vrfCurrency" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="vgCompany" EnableClientScript="true" runat="server" ControlToValidate="ddlCurrency"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Currency %>" InitialValue="-1">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblSBU" runat="server" AssociatedControlID="ddlSBU" Text="<%$ resources:SBU %>" />
                            <asp:DropDownList runat="server" TabIndex="18" ID="ddlSBU" CssClass="select-half-a">
                            </asp:DropDownList>
                            <div class="clear">
                            </div>
                            <%--   <asp:Label ID="lblLogo" runat="server" AssociatedControlID="anchorFile" />--%>
                            <%--<asp:Label ID="lblLogoName" runat="server" />--%>
                            <asp:Label ID="lblLogoUpload" runat="server" AssociatedControlID="fudLogo" Text="<%$ resources:Controls, Logo %>" />
                            <asp:FileUpload ID="fudLogo" runat="server" TabIndex="20" CssClass="upload-btn" />
                            <%-- <asp:Button ID="btnDeleteLogo" runat="server" SkinID="delete-icon" CommandName="DELETEITEM"
                                ToolTip="<%$ resources:DeleteLogo %>"/><%-- OnClick="ActionHandler" -- %>--%>
                            <div class="clear">
                            </div>
                            <asp:Label ID="Label2" runat="server" AssociatedControlID="anchorFile" />
                            <a id="anchorFile" runat="server" target="_blank" tabindex="50" visible="false"></a>
                            <div class="clear">
                            </div>
                            <asp:Label ID="Label1" runat="server" AssociatedControlID="fudLogo" />
                            <asp:Label ID="lblLogoNote" runat="server" CssClass="logo-size-note" Text="<%$ resources: LogoNote %>"></asp:Label>
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblOutputLogo" runat="server" AssociatedControlID="fudLogo" Text="<%$ resources:Controls, OutPutLogo %>" />
                            <asp:FileUpload ID="fudOutputLogo" runat="server" TabIndex="20" CssClass="upload-btn" />
                            <div class="clear">
                            </div>
                            <asp:Label ID="Label3" runat="server" AssociatedControlID="anchorLogo" />
                            <a id="anchorLogo" runat="server" target="_blank" tabindex="50" visible="false"></a>
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
                                onkeyup="limitText(this,200);" CssClass="input-half" />
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
                                CssClass="input-half multiline-2col" onkeyup="limitText(this,200);" TabIndex="4"
                                TextMode="Multiline" EnableViewState="true" />
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
                                onkeyup="limitText(this,200);" TabIndex="6" TextMode="Multiline" EnableViewState="true"
                                CssClass="input-half multiline-2col" />
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblPhone" runat="server" AssociatedControlID="txtPhone" Text="<%$ resources:Phone %>" />
                            <asp:TextBox runat="server" ID="txtPhone" MaxLength="25" onkeydown="limitText(this,25);"
                                onkeyup="limitText(this,25);" TabIndex="10" CssClass="input-half" />
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblMobile" runat="server" AssociatedControlID="txtMobile" Text="<%$ resources:Mobile %>" />
                            <asp:TextBox runat="server" ID="txtMobile" MaxLength="35" onkeydown="limitText(this,35);"
                                onkeyup="limitText(this,35);" TabIndex="12" CssClass="input-half" />
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblTaxNo" runat="server" AssociatedControlID="txtTaxNo" Text="<%$ resources:TaxNo %>" />
                            <asp:TextBox runat="server" ID="txtTaxNo" TabIndex="14" CssClass="input-half" />
                            <%-- MaxLength="25" onkeydown="limitText(this,25);"
                                onkeyup="limitText(this,25);"--%>
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblWebsite" runat="server" AssociatedControlID="txtTaxNo" Text="<%$ resources:Website %>" />
                            <asp:TextBox runat="server" ID="txtWebsite" TabIndex="16" CssClass="input-half" />
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblBusinessRegNo" runat="server" AssociatedControlID="txtBusinessRegNo"
                                Text="<%$ resources:BusinessRegNo %>" />
                            <asp:TextBox runat="server" ID="txtBusinessRegNo" TabIndex="17" onkeydown="limitText(this,100);"
                                onkeyup="limitText(this,100);" MaxLength="100" CssClass="input-half" />
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblGSTNo" runat="server" AssociatedControlID="txtGSTNo" Text="<%$ resources:GSTNo %>" />
                            <asp:TextBox runat="server" ID="txtGSTNo" TabIndex="19" onkeydown="limitText(this,100);"
                                onkeyup="limitText(this,100);" MaxLength="100" CssClass="input-half" />
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblFinYearPeriod" runat="server" Text="<%$ resources:FinYearPeriod %>"
                                AssociatedControlID="txtFinStartDate"></asp:Label>
                            <asp:TextBox ID="txtFinStartDate" runat="server" CssClass="input-small" MaxLength="11"
                                onkeydown="return CheckKey(event)" onpaste="return false;" TabIndex="21"></asp:TextBox>
                            <asp:TextBox ID="txtFinEndDate" runat="server" CssClass="input-small" MaxLength="11"
                                onkeydown="return CheckKey(event)" onpaste="return false;" TabIndex="22"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <div id="divDisplayCode">
                                <asp:Label ID="lblDisplayCode" runat="server" AssociatedControlID="txtDisplayCode"
                                    Text="<%$ resources:DisplayCode %>" />
                                <asp:TextBox runat="server" ID="txtDisplayCode" TabIndex="23" onkeydown="limitText(this,2);"
                                    onkeyup="limitText(this,2);" MaxLength="2" CssClass="input-xsmall margnlft-minus4" />
                                <div class="clear">
                                </div>
                            </div>
                            <asp:Label ID="lblDisplayName" runat="server" AssociatedControlID="txtDisplayName"
                                Text="<%$ resources:DisplayName %>" />
                            <asp:TextBox runat="server" ID="txtDisplayName" TabIndex="25" onkeydown="limitText(this,100);"
                                onkeyup="limitText(this,100);" MaxLength="100" CssClass="input-half" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <h1 class="search-colapse-normal">
            <%=Resources.Captions.ConfigSettings%>
            <img id="imgCmpConfigShow" src="../../Images/Classic/Icons/arrow-colapse-inactive.png"
                alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowCompanyConfig();" />
            <img id="imgCmpConfigHide" src="../../Images/Classic/Icons/arrow-colapse-active.png"
                alt="Hide" title="Hide" style="cursor: pointer" onclick="javascript:HideCompanyConfig();" />
        </h1>
        <div class="grdTable-wrap">
            <table id="tblConfig">
                <tr>
                    <td>
                        <div class="div2col-S">
                            <asp:Label ID="lblProctionIn" runat="server" AssociatedControlID="ddlProductionIn"
                                Text="<%$ resources:ProductionIn %>" />
                            <asp:DropDownList runat="server" TabIndex="21" ID="ddlProductionIn" CausesValidation="true" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true" CssClass="select-small-a">
                                <asp:ListItem Text="<%$ Resources:Pieces %>" Value="1"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Weight %>" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblWeight" runat="server" AssociatedControlID="txtWeight" Text="<%$ resources:WeightPerBsk %>" />
                            <asp:TextBox runat="server" ID="txtWeight" MaxLength="5" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);" onpaste="return false;" TabIndex="23" CssClass="input-small numeric" />
                            <asp:Label ID="Label5" runat="server" CssClass="label15-23-10-2020" AssociatedControlID="txtWeight" Text="<%$ resources:Kg %>" />
                            <div class="starwrap">
                                <asp:RequiredFieldValidator ID="reqWeight" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="vgCompany" EnableClientScript="true" runat="server" ControlToValidate="txtWeight"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_WeightPerBasc %>">
                                </asp:RequiredFieldValidator>
                            </div>

                            <div class="clear">
                            </div>
                            <asp:Label ID="lblProdReqTime" runat="server" AssociatedControlID="txtPrdReqTime"
                                Text="<%$ resources:ProductionReqTime %>" />
                            <asp:TextBox runat="server" ID="txtPrdReqTime" MaxLength="5" TabIndex="25" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);" onpaste="return false;" CssClass="input-small numeric" />
                            <asp:Label ID="Label4" runat="server" CssClass="label15-23-10-2020" AssociatedControlID="txtPrdReqTime" Text="<%$ resources:Hr %>" />
                            <div class="starwrap">
                                <asp:RequiredFieldValidator ID="reqPrdReq" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="vgCompany" EnableClientScript="true" runat="server" ControlToValidate="txtPrdReqTime"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ReqProdTime %>">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div class="clear">
                            </div>
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S">
                            <asp:Label ID="lblNoOfPcs" runat="server" AssociatedControlID="txtNoOfPcs" Text="<%$ resources:NoOfPcsPerBsk %>" />
                            <asp:TextBox runat="server" ID="txtNoOfPcs" MaxLength="5" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);" TabIndex="22" onpaste="return false;" CssClass="input-small numeric" />
                            <div class="starwrap">
                                <asp:RequiredFieldValidator ID="reqNoOfPcs" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="vgCompany" EnableClientScript="true" runat="server" ControlToValidate="txtNoOfPcs"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_NoOfPcs %>">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblNoOfBasket" runat="server" AssociatedControlID="txtNoOfBasket"
                                Text="<%$ resources:NoOfBasket %>" />
                            <asp:TextBox runat="server" ID="txtNoOfBasket" MaxLength="5" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);" onpaste="return false;" TabIndex="15" CssClass="input-small numeric" />
                            <div class="starwrap">
                                <asp:RequiredFieldValidator ID="reqNoOfBasket" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="vgCompany" EnableClientScript="true" runat="server" ControlToValidate="txtNoOfBasket"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_NoOfBasket %>">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblPrintLbl" runat="server" AssociatedControlID="chbPrintLabel" Text="<%$ resources:PrintLabel %>" />
                            <asp:CheckBox ID="chbPrintLabel" runat="server" TabIndex="26" Checked="false" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divQASample" runat="server">
            <h1 class="search-colapse-normal">
                <%=GetLocalResourceObject("QASample").ToString()%>
                <img id="imgQASampleShow" src="../../Images/Classic/Icons/arrow-colapse-inactive.png"
                    alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowHideQASample(1);" />
                <img id="imgQASampleHide" src="../../Images/Classic/Icons/arrow-colapse-active.png"
                    alt="Hide" title="Hide" style="cursor: pointer" onclick="javascript:ShowHideQASample();" />
            </h1>
            <div class="grdTable-wrap">
                <table id="tblQASample">
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <asp:Label ID="lblMedical" runat="server" AssociatedControlID="txtMedical"
                                    Text="<%$ resources:Medical %>" />
                                <asp:TextBox ID="txtMedical" runat="server" CssClass="input-small numeric"
                                    onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"></asp:TextBox>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">
                                <asp:Label ID="lblNonmedical" runat="server" AssociatedControlID="txtNonMedical"
                                    Text="<%$ resources:NonMedical %>" />
                                <asp:TextBox ID="txtNonMedical" runat="server" CssClass="input-small numeric"
                                    onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="footer-content">
        <asp:Label ID="lblProductNameH" runat="server" Text="<%$ resources:ProductName %>" />:
        <asp:Label ID="lblProductName" runat="server" />&nbsp;| &nbsp;
        <asp:Label ID="lblVersionH" runat="server" Text="<%$ resources:Version %>" />:
        <asp:Label ID="lblVersion" runat="server" />&nbsp;| &nbsp;
        <asp:Label ID="lblGAFH" runat="server" Text="<%$ resources:GAF %>" />:
        <asp:Label ID="lblGAF" runat="server" />
    </div>
    <div id="diverror" style="display: none">
        <%--Use this label to bind the server errors--%>
        <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star" />
        <asp:ValidationSummary ID="vvsUser" ValidationGroup="vgCompany" runat="server" />
    </div>
    <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" />
</asp:Content>
