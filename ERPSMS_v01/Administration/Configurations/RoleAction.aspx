<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/Configurations/AdminConfigMaster.Master" AutoEventWireup="true" CodeBehind="RoleAction.aspx.cs" Inherits="ERPSMS_v01.Administration.Configurations.RoleAction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function ValidateNow() {
        if (typeof (Page_ClientValidate) == 'function') {
            Page_ClientValidate();
        }
        if (!Page_IsValid) {
            $("#litErrorMsg").hide();

            ShowErrorMessage($("#diverror").html());
            return false;  //Page is invalid -- stop right here
        }
        else {
            //everythings ok --- Call your function & do your stuff
            return true;
        }
    }

    //Function to check and uncheck  trreview
    function postBackByObject() {
        var o = window.event.srcElement;
        if (o.tagName == "INPUT" && o.type == "checkbox") {
            __doPostBack("", "");
        }
    }

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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%--  <asp:UpdatePanel runat="server" ID="aupdpnlRoleAction">
        <ContentTemplate>
    <div id="innerPage-wrap">
        <asp:Table runat="server" ID="tblTemplate">
            <asp:TableRow>
                <asp:TableCell ID="SEC_ActionPanel" >
                    <div id="webwizard-wrap">
                        <h1>
                        <asp:Literal ID="ltHead" runat="server" Text="<%$ resources:UserGroupActions%>" />
                          </h1>
                        <div class="button-wrap">
                            <asp:ImageButton ID="imgbtnSave" runat="server" SkinID="btnsave" OnClick="ActionHandler"
                                OnClientClick="javascript:return ValidateNow()" CommandName="SAVE" TabIndex="9" />
                            <asp:ImageButton ID="imgbtnCancel" runat="server" SkinID="btnreset" OnClick="ActionHandler"
                                CommandName="CANCEL" TabIndex="10" />
                        </div>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="Action_Entry" runat="server">
                <asp:TableCell CssClass="hrzcentr">
                    <div class="div2col-M">
                        <asp:Label runat="server" ID="lblRole" Text="<%$ resources:UserGroup %>" AssociatedControlID="ddlRoles"></asp:Label>
                        <asp:DropDownList runat="server" ID="ddlRoles" TabIndex="1" OnSelectedIndexChanged="ActionHandler"
                            AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="vrfRole" CssClass="star" InitialValue="-1" SetFocusOnError="true"
                            ValidationGroup="Save" EnableClientScript="true" runat="server" ControlToValidate="ddlRoles"
                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_UserGroup %>"></asp:RequiredFieldValidator>

                        <div class="clear"></div>
                        <div class="treeview treeview-mrgLFT-M">
                            <asp:TreeView ID="trvRoleAction" runat="server" TabIndex="2" ShowLines="True" OnTreeNodeDataBound="ActionHandler"
                                ExpandDepth="1" meta:resourcekey="trvLocationResource1">
                                <NodeStyle Font-Bold="True" />
                                <RootNodeStyle Font-Bold="True" />
                                <ParentNodeStyle Font-Bold="True" />
                                <DataBindings>
                                    <asp:TreeNodeBinding DataMember="PAGE" TextField="ACT_Page" Value="0" ShowCheckBox="True" />
                                    <asp:TreeNodeBinding DataMember="SECTION" TextField="ACT_Section" Value="0" ShowCheckBox="True" />
                                    <asp:TreeNodeBinding DataMember="ACTION" TextField="ACT_Action" ValueField="ACT_PK"
                                        ShowCheckBox="True" />
                                </DataBindings>
                            </asp:TreeView>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    
    </ContentTemplate>
    </asp:UpdatePanel>

    <div id="diverror" style="display: none">
        <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static"></asp:Label>
        <asp:ValidationSummary ID="vvsPage" ValidationGroup="Save" runat="server" />
    </div>--%>
</asp:Content>
