<%@ Page Title="<%$ Resources:Captions,Title_UserManagement %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="CreateNewUser.aspx.cs"
    Inherits="ERPSMS_v01.Administration.Configurations.CreateNewUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/Administration/Configurations/CreateNewUser.js.axd"
        type="text/javascript"></script>
    <script type="text/javascript">
        //Function to check and uncheck  trreview
        function OnCheckBoxCheckChanged(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;
                //check if nxt sibling is not null & is an element node
                if (nxtSibling && nxtSibling.nodeType == 1) {
                    //if node has children                     
                    if (nxtSibling.tagName.toLowerCase() == "div") {
                        //check or uncheck children at all levels 
                        CheckUncheckChildren(parentTable.nextSibling, src.checked);
                    }
                }
                //check or uncheck parents at all levels 
                CheckUncheckParents(src, src.checked);
            }
        }

        function CheckUncheckChildren(childContainer, check) {
            var childChkBoxes = childContainer.getElementsByTagName("input");
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                childChkBoxes[i].checked = check;
            }
        }

        function CheckUncheckParents(srcChild, check) {
            var parentDiv = GetParentByTagName("div", srcChild);
            var parentNodeTable = parentDiv.previousSibling;

            if (parentNodeTable) {
                var checkUncheckSwitch;
                //checkbox checked 
                if (check) {
                    var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                    if (isAllSiblingsChecked)
                        checkUncheckSwitch = true;
                    else
                    //do not need to check parent if any(one or more) child not checked 
                        return;
                }
                else //checkbox unchecked 
                {
                    checkUncheckSwitch = false;
                }

                var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                if (inpElemsInParentTable.length > 0) {
                    var parentNodeChkBox = inpElemsInParentTable[0];
                    parentNodeChkBox.checked = checkUncheckSwitch;
                    //do the same recursively 
                    CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                }
            }
        }

        function AreAllSiblingsChecked(chkBox) {
            var parentDiv = GetParentByTagName("div", chkBox);
            var childCount = parentDiv.childNodes.length;
            for (var i = 0; i < childCount; i++) {
                //check if the child node is an element node                 
                if (parentDiv.childNodes[i].nodeType == 1) {
                    if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        if (typeof (prevChkBox) != "undefined") {
                            //if any of sibling nodes are not checked, return false
                            if (!prevChkBox.checked) {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        //utility function to get the container of an element by tagname 
        function GetParentByTagName(parentTagName, childElementObj) {
            var parent = childElementObj.parentNode;
            while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                parent = parent.parentNode;
            }
            return parent;
        }

        //        Stregth Password
        $(document).ready(function () {
            //$('#txtPassword').keyup(function ()
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%-- <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.NewUser%>
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton ID="imbSave" runat="server" OnClick="ActionHandler" CommandName="SAVE"
                SkinID="btnsave" TabIndex="10" OnClientClick="javascript:return ValidateNow('vgUser')" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" TabIndex="11" OnClientClick="javascript:return ResetPage();" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();" />
        </div>
    </div>--%>
    <div class="fixed-buttons">
        <%-- <div class="Button-container">--%>
        <div id="divBtnContainer" runat="server" class="Button-container">
            <asp:Table runat="server" ID="tblButton">
                <asp:TableRow>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul class="bredcrum">
                            <asp:Label runat="server" ID="lblBreadCrum" />
                        </ul>
                        <ul runat="server" id="pnlListing">
                            <li>
                                <asp:Button ID="btnUserMapping" Visible="false" runat="server" SkinID="btnInner-add"
                                    Text="<%$Resources:Controls,UserMapping%>" OnClick="ActionHandler" CommandName="USERMAPPING"
                                    TabIndex="10" ToolTip="<%$Resources:Controls,UserMapping%>" />
                            </li>
                            <li>
                                <asp:Button ID="btnSave" runat="server" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    OnClientClick="javascript:return ValidateNow('vgUser')" OnClick="ActionHandler"
                                    CommandName="SAVE" TabIndex="15" EnableViewState="false" />
                            </li>
                            <li>
                                <asp:Button ID="btnReset" runat="server" Visible="false" SkinID="btnInner-refresh"
                                    Text="<%$Resources:Controls,Refresh%>" OnClientClick="javascript:return ResetPage();"
                                    TabIndex="8" />
                            </li>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    OnClick="ActionHandler" CommandName="CANCEL" TabIndex="16" /><%--OnClientClick="javascript:return CancelFun();"--%>
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <%--</div>--%>
        <div class="tab-container padgrgt0" id="divTabContainer" runat="server">
            <ul id="tab-menu">
                <li><span id="spnList" runat="server" class="tab-inactive">
                    <asp:LinkButton runat="server" ID="lnkList" Text="<%$ resources:List %>" TabIndex="11"
                        CommandName="USER" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                        CssClass="tab-inactive"></asp:LinkButton>
                </span></li>
                <li><span id="spnUserInfo" runat="server" class="tab-active">
                    <asp:LinkButton runat="server" ID="lnkUserInfo" Text="<%$resources:UserInfo %>" TabIndex="12"
                        CommandName="USERINFO" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                        CssClass="tab-active"></asp:LinkButton>
                </span></li>
                <li><span id="spnUserModule" runat="server" class="tab-inactive">
                    <asp:LinkButton runat="server" ID="lnkUserModule" Text="<%$ resources:UserRole %>"
                        TabIndex="13" CommandName="USERMODULE" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                        CssClass="tab-inactive"></asp:LinkButton>
                </span></li>
                <li><span id="spnLocationMapping" runat="server" class="tab-inactive">
                    <asp:LinkButton runat="server" ID="lnkLocationMapping" Text="<%$ resources:LocationMapping %>"
                        TabIndex="14" CommandName="LOCATIONMAPPING" CommandArgument="SEC_ActionPanel"
                        OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                </span></li>
            </ul>
        </div>
    </div>
    <div class="content-wrapper">
        <asp:MultiView runat="server" ID="mltUserTabs" ActiveViewIndex="0">
            <asp:View ID="viewUserInfo" runat="server">
                <div id="grdTable-wrap">
                    <%--  <div id="divData">--%>
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <%-- <div class="content">--%>
                                    <asp:Label ID="lblUserName" runat="server" AssociatedControlID="txtUserName" Text="<%$ resources:UserName %>" />
                                    <asp:TextBox runat="server" ID="txtUserName" MaxLength="25" CssClass="input-half"
                                        onkeydown="limitText(this,25);" onkeyup="limitText(this,25);" TabIndex="1" />
                                    <div class="starwrap">
                                        <asp:RequiredFieldValidator ID="vrfUserName" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="vgUser" EnableClientScript="true" runat="server" ControlToValidate="txtUserName"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_UserName %>" />
                                        <asp:RegularExpressionValidator ID="vreUserName" runat="server" ControlToValidate="txtUserName"
                                            ErrorMessage="<%$ resources:Msg_Vali_User %>" ValidationExpression="[^\s]+" Display="Dynamic"
                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="vgUser" />
                                    </div>
                                    <div class="clear">
                                    </div>
                                    <%-- <asp:Label ID="lblEmployee" runat="server" AssociatedControlID="txtEmployee" Text="<%$ resources:Employee %>"></asp:Label>
                            <asp:TextBox runat="server" ID="txtEmployee" MaxLength="200" TabIndex="2"></asp:TextBox>
                            <asp:HiddenField ID="hdfEmployee" runat="server" />
                            <asp:RequiredFieldValidator ID="vrfEmployee" CssClass="star" SetFocusOnError="true"
                                InitialValue="<%$ resources:Messages, AutoDefaultValue %>" ValidationGroup="vgUser"
                                EnableClientScript="true" runat="server" ControlToValidate="txtEmployee" Display="Dynamic"
                                Text="*" ErrorMessage="<%$ resources:Msg_Employee %>">
                            </asp:RequiredFieldValidator>
                             <div class="clear">
                                </div>--%>
                                    <%--<div id="divPassword" runat="server">--%>
                                    <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" Text="<%$ resources:Password %>" />
                                    <asp:TextBox runat="server" ID="txtPassword" MaxLength="25" onkeydown="limitText(this,25);"
                                        onkeyup="limitText(this,25);" TabIndex="3" TextMode="Password" EnableViewState="true"
                                        CssClass="input-half" />
                                    <div class="starwrap">
                                        <asp:RequiredFieldValidator ID="vrfPassword" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="vgUser" EnableClientScript="true" runat="server" ControlToValidate="txtPassword"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_Password %>" />
                                        <asp:RegularExpressionValidator ID="vrePassword" runat="server" ControlToValidate="txtPassword"
                                            ErrorMessage="<%$ resources:Msg_MinLen %>" ValidationExpression="^.{7,20}$" Display="Dynamic"
                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="vgUser" />
                                    </div>
                                    <span id="password_strength" class="style-none"></span>
                                    <div class="clear">
                                    </div>
                                    <%--</div>--%>
                                    <asp:Label ID="lblStatus" runat="server" AssociatedControlID="ddlStatus" Text="<%$ resources:Status %>" />
                                    <asp:DropDownList runat="server" ID="ddlStatus" TabIndex="5" CssClass="select-half-a">
                                        <%-- <asp:ListItem Text="<%$ resources:Controls, Select %>" Value="-1"></asp:ListItem>--%>
                                        <asp:ListItem Text="<%$ resources:Controls, Active %>" Value="1" />
                                        <asp:ListItem Text="<%$ resources:Controls, InActive %>" Value="0" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="vrfStatus" CssClass="star" SetFocusOnError="true"
                                        ValidationGroup="vgUser" EnableClientScript="true" InitialValue="-1" runat="server"
                                        ControlToValidate="ddlStatus" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Status %>" />
                                    <div class="clear">
                                    </div>
                                    <asp:Label ID="lblTheme" runat="server" AssociatedControlID="ddlTheme" Text="<%$ resources:Theme %>" />
                                    <asp:DropDownList runat="server" ID="ddlTheme" TabIndex="7" CssClass="select-half-a">
                                        <%--  <asp:ListItem Text="<%$ resources:Controls, Select %>" Value="-1" />--%>
                                        <asp:ListItem Text="<%$ resources:Controls, ThemeClassic %>" Value="ClassicExt" />
                                        <asp:ListItem Text="<%$ resources:Controls, ThemeBlue %>" Value="BlueExt" />
                                        <asp:ListItem Text="<%$ resources:Controls, ThemeRed %>" Value="RedExt" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="vrfTheme" CssClass="star" SetFocusOnError="true"
                                        ValidationGroup="vgUser" EnableClientScript="true" InitialValue="-1" runat="server"
                                        ControlToValidate="ddlTheme" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_Theme %>" />
                                    <div class="clear">
                                    </div>
                                    <asp:Label ID="lblDepartment" runat="server" AssociatedControlID="ddlDepartment"
                                        Text="<%$ resources:DefaultDepartment %>" />
                                    <asp:DropDownList runat="server" ID="ddlDepartment" TabIndex="9" CssClass="select-half-a">
                                        <asp:ListItem Text="<%$ resources:Controls, SelectVal %>" Value="-1" />
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                    <div id="divUserType" runat="server" visible="false">
                                        <asp:Label ID="lblUserType" runat="server" AssociatedControlID="ddlUserType" Text="<%$ resources:UserType %>" />
                                        <asp:DropDownList runat="server" ID="ddlUserType" TabIndex="13" CssClass="select-half-a margnlft-minus4">
                                            <%--<asp:ListItem Text="<%$ resources:Controls, ApplicationUser %>" Value="0" />
                                    <asp:ListItem Text="<%$ resources:Controls, SystemUser %>" Value="1" />--%>
                                        </asp:DropDownList>
                                        <div class="clear">
                                        </div>
                                        <asp:Label ID="lblCulture" runat="server" AssociatedControlID="ddlCulture" Text="<%$ resources:UserCulture %>"
                                            Visible="<%$ Resources:ConfigurationsRes,CultureChangeVisibility %>" />
                                        <asp:DropDownList runat="server" TabIndex="8" ID="ddlCulture" CssClass="select-half-a margnlft-minus4"
                                            Visible="<%$ Resources:ConfigurationsRes,CultureChangeVisibility %>">
                                            <%--OnSelectedIndexChanged="ActionHandler"--%>
                                            <%-- <asp:ListItem Text="<%$ resources:Controls, SelectVal %>" Value="-1" />--%>
                                        </asp:DropDownList>
                                        <div class="clear">
                                        </div>
                                        <%--  <asp:Label ID="lblchkInboxDeflt" runat="server" AssociatedControlID="chkInboxDeflt"
                                        Text="<%$ resources:InboxDefault %>" />
                                        <asp:CheckBox runat="server" ID="chkInboxDeflt" />--%>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <%--<div class="content">--%>
                                    <asp:Label ID="lblEmployee" runat="server" AssociatedControlID="ddlEmployee" Text="<%$ resources:Employee %>" />
                                    <asp:DropDownList runat="server" TabIndex="2" ID="ddlEmployee" CssClass="select-half-a">
                                        <%--OnSelectedIndexChanged="ActionHandler"--%>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="vrfEmployee" CssClass="star" SetFocusOnError="true"
                                        ValidationGroup="vgUser" EnableClientScript="true" InitialValue="-1" runat="server"
                                        ControlToValidate="ddlEmployee" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Employee %>" />
                                    <div class="clear">
                                    </div>
                                    <%--<div id="divPasswordConfirm" runat="server">--%>
                                    <asp:Label ID="lblConfirmPwd" runat="server" AssociatedControlID="txtConfirmPwd"
                                        Text="<%$ resources:ConfirmPwd %>" />
                                    <asp:TextBox runat="server" ID="txtConfirmPwd" MaxLength="25" onkeydown="limitText(this,25);"
                                        onkeyup="limitText(this,25);" TabIndex="4" TextMode="Password" EnableViewState="true"
                                        CssClass="input-half" />
                                    <div class="starwrap">
                                        <asp:RequiredFieldValidator ID="vrfConfirmPwd" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="vgUser" EnableClientScript="true" runat="server" ControlToValidate="txtConfirmPwd"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_ConfirmPwd %>" />
                                        <asp:CompareValidator ID="vcpConfirmPwd" CssClass="star" SetFocusOnError="true" ValidationGroup="vgUser"
                                            EnableClientScript="true" runat="server" ControlToValidate="txtPassword" ControlToCompare="txtConfirmPwd"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_Pwd_Match %>" />
                                    </div>
                                    <div class="clear">
                                    </div>
                                    <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" Text="<%$ resources:Email %>" />
                                    <asp:TextBox runat="server" ID="txtEmail" MaxLength="100" TabIndex="6" CssClass="input-half" />
                                    <div class="starwrap">
                                        <asp:RequiredFieldValidator ID="vrfEmail" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="vgUser" EnableClientScript="true" runat="server" ControlToValidate="txtEmail"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_Email %>" />
                                        <asp:RegularExpressionValidator ID="vreEmail" runat="server" ControlToValidate="txtEmail"
                                            ErrorMessage="<%$ resources:Msg_Valid_Email %>" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                            Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="vgUser" />
                                    </div>
                                    <div class="clear">
                                    </div>
                                    <asp:Label ID="lblIsAlertSound" runat="server" AssociatedControlID="chkIsAlertSound"
                                        Text="<%$ resources:IsAlertSound %>" />
                                    <asp:CheckBox ID="chkIsAlertSound" runat="server" Checked="false" TabIndex="10" />
                                    <asp:Label ID="lblIsPublicUser" runat="server" AssociatedControlID="chkIsPublicUser"
                                        Text="<%$ resources:IsPublicUser %>" />
                                    <asp:CheckBox ID="chkIsPublicUser" runat="server" Checked="true" TabIndex="11" />
                                    <asp:Label ID="lblchkInboxDeflt" runat="server" AssociatedControlID="chkInboxDeflt"
                                        Text="<%$ resources:InboxDefault %>" CssClass='lbl-27-2perc' />
                                    <asp:CheckBox runat="server" ID="chkInboxDeflt" TabIndex="12" />
                                    <div class="clear">
                                    </div>
                                    <%--<asp:Label ID="lblIsPublicUser" runat="server" AssociatedControlID="chkIsPublicUser"
                                        Text="<%$ resources:IsPublicUser %>" />
                                    <asp:CheckBox ID="chkIsPublicUser" runat="server" Checked="true" />--%>
                                    <div class="clear">
                                    </div>
                                    <asp:Label ID="lblSignature" runat="server" AssociatedControlID="lblSignatureName"
                                        Text="<%$ resources:Controls, Signature %>" />
                                    <asp:Label ID="lblSignatureName" runat="server" CssClass="input-half" />
                                    <asp:Button ID="btnDeleteSignature" runat="server" SkinID="delete-icon" CommandName="DELETEITEM"
                                        ToolTip="<%$ resources:DeleteSignature %>" OnClick="ActionHandler" />
                                    <div class="clear">
                                    </div>
                                    <asp:Label ID="lblSignatureUpload" runat="server" AssociatedControlID="fudSignature" />
                                    <asp:FileUpload ID="fudSignature" runat="server" TabIndex="14" />
                                    <div class="clear">
                                    </div>
                                    <a id="anchorFile" runat="server" target="_blank" tabindex="15" visible="false" style="margin-left: 25.5% !important">
                                    </a>
                                    <div class="clear">
                                    </div>
                                    <asp:Label ID="lblSecCulture" runat="server" AssociatedControlID="ddlSecCulture"
                                        Text="<%$ resources:SecUserCulture %>" Visible="<%$ Resources:ConfigurationsRes,CultureChangeVisibility %>" />
                                    <asp:DropDownList runat="server" TabIndex="8" ID="ddlSecCulture" CssClass="select-half-a"
                                        Visible="<%$ Resources:ConfigurationsRes,CultureChangeVisibility %>">
                                        <%--OnSelectedIndexChanged="ActionHandler"--%>
                                        <%-- <asp:ListItem Text="<%$ resources:Controls, SelectVal %>" Value="-1" />--%>
                                    </asp:DropDownList>
                                    <%--</div>--%>
                                    <div id="divADUsers" runat="server" visible="false" class="div2col-C">
                                        <asp:Label ID="lblADUsers" runat="server" AssociatedControlID="txtADUsers" Text="<%$ resources:ADUser %>" />
                                        <asp:TextBox runat="server" ID="txtADUsers" MaxLength="200" TabIndex="135" />
                                        <asp:HiddenField ID="hdfADUsers" runat="server" />
                                        <asp:RequiredFieldValidator ID="vrfADUsers" CssClass="star" SetFocusOnError="true"
                                            InitialValue="<%$ resources:Messages, AutoDefaultValue %>" ValidationGroup="vgUser"
                                            EnableClientScript="true" runat="server" ControlToValidate="txtADUsers" Display="Dynamic"
                                            Text="*" ErrorMessage="<%$ resources:Msg_ADUser %>" />
                                        <div style="display: none;">
                                            <asp:Button ID="btnADUSerChange" runat="server" ValidationGroup="vgUser" CommandName="ITEMSELECTED" />
                                            <%--OnClick="ActionHandler"--%>
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </div>
                                    <%--  </div>--%>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="clear">
                    </div>
                </div>
            </asp:View>
            <asp:View ID="viewUserModule" runat="server">
                <div class="gridwrap">
                    <asp:GridView runat="server" ID="grdUserModule" Width="100%" AllowSorting="True"
                        AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" ShowHeader="true">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid%>"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="No." ItemStyle-Width="2%">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle Width="5%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Module %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblUser" runat="server" Text='<%# Eval("SYM_NAME") %>' ToolTip='<%# Eval("SYM_CODE")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="25%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Role %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblRole" runat="server" Text='<%# Eval("SYM_USER_GROUP_TEXT") %>'
                                        ToolTip='<%# Eval("SYM_USER_GROUP_TEXT")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="70%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:View>
            <asp:View ID="viewLocationMapping" runat="server">
                <div class="treeview-center">
                    <div class="treeview max-500">
                        <h4>
                            UserLocations
                        </h4>
                        <%-- <asp:Label ID="lblGroup" runat="server" AssociatedControlID="trvGroup" Text="<%$resources:Controls,UserGroups %>"></asp:Label></h4>--%>
                        <div class="clear">
                        </div>
                        <asp:TreeView ID="trvUserLocations" runat="server" ShowLines="True" ExpandDepth="0"
                            SelectedNodeStyle-Font-Bold="true" LeafNodeStyle-CssClass="lineheight-22" SelectedNodeStyle-ForeColor="#236cb5"
                            SelectedNodeStyle-BackColor="#e6f3ff" OnClick="OnCheckBoxCheckChanged(event);">
                        </asp:TreeView>
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>
        <%--    </div>--%>
    </div>
    <div id="diverror" style="display: none">
        <%--Use this label to bind the server errors--%>
        <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star" />
        <asp:ValidationSummary ID="vvsUser" ValidationGroup="vgUser" runat="server" />
    </div>
</asp:Content>
