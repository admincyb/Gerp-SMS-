<%@ Page Title="<%$ Resources:Captions,UserCreation %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    EnableEventValidation="false" AutoEventWireup="true" CodeBehind="UserManagement.aspx.cs"
    Inherits="ERPSMS_v01.Administration.Configurations.UserManagement" Theme="ClassicExt" %>

<%@ Register Src="~/UserControls/PagerControl.ascx" TagName="pagercontrol" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/Administration/Configurations/UserManagement.js.axd"
        type="text/javascript"></script>
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        $(document).ready(function () {
            //GrandScriptUtils.MakeAutoCompleteDDL("txtCopyUserName", url + "?Type=usrName&EmpType=" + $("[id$='hdfUserType']").val(), "hdfCopyUserPk", true, true, "GETUSERSAUTO");
        });

        function AutoInit() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtCopyUserName", url + "?Type=usrName&EmpType=" + $("[id$='hdfUserType']").val(), "hdfCopyUserPk", true, true, "GETUSERSAUTO");
        }
        function ShowCopyUserRoleConfirm(btn) {
            var msgTitle;
            var msg;
            var mergeReplace = "";
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';

            if ($("[id$=hdfCopyUserPk]").val() == "0" || (!$("input[id$=rbtnReplace]").is(':checked') && !$("input[id$=rbtnMerge]").is(':checked'))) {
                return ValidatePageNow('add');
            }
            if ($("input[id$=rbtnReplace]").is(':checked')) {
                mergeReplace = "Replace";
            }
            else if ($("input[id$=rbtnMerge]").is(':checked')) {
                mergeReplace = "Merge";
            }
            msg = "Are u sure want to " + mergeReplace + " " + $("[id$=lblUserNameTxt]").text() + " roles with " + $("[id$=txtCopyUserName]").val() + " roles?";
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

        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverrorAlert").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                //To Prevent Multiple Click 
                if ($("[id$=hdfIsMultipleClick]").val() == "0") {
                    $("[id$=hdfIsMultipleClick]").val("1");
                    return true;
                }
                else
                    return false

                //return true;
            }
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

        function CheckMergeReplace(source, args) {
            if ($("input[id$=rbtnReplace]").is(':checked') || $("input[id$=rbtnMerge]").is(':checked')) {
                args.IsValid = true;
            }
            else {
                args.IsValid = false;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlUserMg" runat="server">
        <ContentTemplate>
            <%--    <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.UserGroup%>
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton ID="imbSave" CommandName="SAVE" OnClick="ActionHandler" runat="server"
                SkinID="btnsave" TabIndex="1" />
            <asp:ImageButton ID="imbCancel" CommandName="CANCEL" TabIndex="2" runat="server"
                SkinID="btncancel" OnClick="ActionHandler" />
        </div>
    </div>--%>
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
                                        <asp:Button runat="server" ID="btnShowCopyRolePopup" CommandName="SHOWPOPUP" TabIndex="1"
                                            Text="<%$resources:CopyUserRoles %>" OnClick="ActionHandler" ToolTip="<%$resources:CopyUserRoles %>"
                                            SkinID="btnInner-addInv" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnSave" runat="server" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                            OnClick="ActionHandler" CommandName="SAVE" TabIndex="1" EnableViewState="false" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                            CommandName="CANCEL" OnClick="ActionHandler" TabIndex="2" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks display-table">
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <div class="code-container" runat="server" id="divUser">
                                <asp:Label ID="lblUserName" runat="server" AssociatedControlID="lblUserNameTxt" Text="<%$resources:UsrName %>"
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblUserNameTxt" runat="server" Text=""></asp:Label>
                                <asp:Label ID="lblEmployee" runat="server" AssociatedControlID="lblEmployeeTxt" Text="<%$resources:Employee %>"
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblEmployeeTxt" runat="server" Text=""></asp:Label>
                                <div class="clear">
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                            <div class="treeview-center w50perc">
                                <div class="treeview max-500">
                                    <h4>
                                        <%=Resources.Captions.UserGroups%>
                                    </h4>
                                    <%-- <asp:Label ID="lblGroup" runat="server" AssociatedControlID="trvGroup" Text="<%$resources:Controls,UserGroups %>"></asp:Label></h4>--%>
                                    <div class="clear">
                                    </div>
                                    <asp:TreeView ID="trvGroup" runat="server" ShowLines="false" ExpandDepth="0">
                                    </asp:TreeView>
                                </div>
                            </div>
                            <%------------- New PO List Popup Start ---------------------%>
                            <div id="divPopUpCopyUserRoles" style="display: none">
                                <div class="content-wrapper">
                                    <div class="Button-container-popup">
                                        <asp:Button ID="btnPopupApply" runat="server" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                            ValidationGroup="add" CommandName="APPLY" OnClick="ActionHandler" TabIndex="10"
                                            OnClientClick="return ShowCopyUserRoleConfirm(this);" />
                                    </div>
                                    <div id="divPaymentFilterDetails" runat="server">
                                        <table class="table-devide tablelayout">
                                            <tr>
                                                <td>
                                                    <div>
                                                        <asp:Label runat="server" ID="lblCopyFrom" AssociatedControlID="txtCopyUserName"
                                                            CssClass="w19perc" Text="<%$resources:CopyFrom%>"></asp:Label>
                                                        <asp:TextBox ID="txtCopyUserName" runat="server" EnableViewState="false" CssClass="w46perc"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfCopyUserPk" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hdfUserType" runat="server" Value="0" />
                                                        <asp:RequiredFieldValidator ID="vrfCustomer" CssClass="star" SetFocusOnError="true"
                                                            InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>" ValidationGroup="add"
                                                            EnableClientScript="true" runat="server" ControlToValidate="txtCopyUserName"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_SelectCopyUser %>">
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div class="margnlft20-3per">
                                                        <asp:RadioButton ID="rbtnMerge" runat="server" Text="<%$resources:Merge%>" GroupName="ReplaceMerge" />
                                                        <asp:RadioButton ID="rbtnReplace" runat="server" Text="<%$resources:Replace%>" GroupName="ReplaceMerge" />
                                                        <asp:CustomValidator ID="vcvMergeReplace" runat="server" ErrorMessage="<%$ resources:Err_SelectOption %>"
                                                            CssClass="star" Text="*" Display="Dynamic" ValidationGroup="add" ClientValidationFunction="CheckMergeReplace"></asp:CustomValidator>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <%------------- New PO List Popup End ---------------------%>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <%--Use this label to bind the server errors--%>
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
            </div>
            <div id="diverrorAlert" style="display: none">
                <asp:ValidationSummary ID="vsAddToList" ValidationGroup="add" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
