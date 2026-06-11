<%@ Page Title="<%$ Resources:Captions,Title_Print %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="GenerateReport.aspx.cs" Inherits="ERPSMS_v01.Reports.GenerateReport"
    Theme="Classic" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms"
    TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
       

        function ddlSelectionChange(cnme) {
            $("[id$=btnSearch]").attr('CommandName', cnme);
            $("[id$=btnSearch]").click();
            return false;
        }
        $(document).ready(function () { InitComponents(); });
        function InitComponents() {
            var pageURL = window.document.URL;
            var virtualPath = '<%= (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString()) %>';
            var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
            GrandScriptUtils.MakeAutoCompleteDDL("txtAccount", url + "?VoucherType=&AccType=", "hdfAccount", true, true, "ACCOUNT");
            if ($("[id$=ddlSubLedger]").val() != "-1") {
                var subLedgr = $("[id$=ddlSubLedger]").val();
                if ($("[id$=hdfAppType]").val() == 'BRC')
                    subLedgr = '11';
                GrandScriptUtils.MakeAutoCompleteDDL("txtSubAccount", url + "&AccType=" + subLedgr, "hdfSubAccount", true, true, "SUBACCOUNTS");
            }

            GrandScriptUtils.AddDateRange("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", "dd-M-yy", false, true, false);
            GetReferrer(); 
        }

        function ShowAccountsTree() {
            $("[id$=divAccPopUp]").show();
            //GrandScriptUtils.ShowModalID("divAccPopUp", "Accounts", "SELECTEDACC", 400, 400, true);

            //containerID, title, command, width, height, okBtn
        }

        function ModalOk(cmd) {
            // $("[id$=btnSearch]").click();
            // GetSelectedNode();

        }

        function getSubAccounts() {
            var pageURL = window.document.URL;
            var virtualPath = '<%= (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString()) %>';
            var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

            if ($("[id$=ddlSubLedger]").val() != "-1") {
                var subLedgr = $("[id$=ddlSubLedger]").val();
                if ($("[id$=hdfAppType]").val() == 'BRC')
                    subLedgr = '11';
                $("[id$=txtSubAccount]").val('');
                $("[id$=hdfSubAccount]").val('');
                GrandScriptUtils.MakeAutoCompleteDDL("txtSubAccount", url + "&AccType=" + subLedgr, "hdfSubAccount", true, true, "SUBACCOUNTS");
           }
        }

        function GetReferrer() {
            if (document.referrer.trim() != "" && $("[id$=hdfRefUrl]").val().trim() == "")
                $("[id$=hdfRefUrl]").val(document.referrer);
        }

    </script>
    <script type="text/javascript">
        function OnCheckBoxCheckChanged(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;
                if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
                {
                    if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
                    {
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

                if (check) //checkbox checked
                {
                    var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                    if (isAllSiblingsChecked)
                        checkUncheckSwitch = true;
                    else
                        return; //do not need to check parent if any(one or more) child not checked
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
                if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
                {
                    if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        //if any of sibling nodes are not checked, return false
                        if (!prevChkBox.checked) {
                            return false;
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



        function GetSelectedNode() {
            var treeViewData = window["<%=trvAccounts.ClientID%>"];
            if (treeViewData.selectedNodeID.value != "") {
                var selectedNode = document.getElementById(treeViewData.selectedNodeID.value);
                var value = selectedNode.href.substring(selectedNode.href.indexOf(",") + 3, selectedNode.href.length - 2);
                var text = selectedNode.innerHTML;
                alert("Text: " + text + "\r\n" + "Value: " + value);
            } else {
                alert("No node selected.")
            }
            return false;
        }

        function OpenPDF(url) {
            var win = window.open(url, '_blank', 'fullscreen=no,location=no,menubar=no,scrollbars=yes,titlebar=no,toolbar=no,width=1200,height=800,left=100,top=0');
            CancelFun();
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
                                <asp:Button ID="btnSearch" runat="server" ClientIDMode="Static" OnClick="ActionHandler"
                                    Text="View" CommandName="VIEW" SkinID="btnInner-View" />
                            </li>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Close%>" OnClientClick="javascript:GetReferrer();"
                                   OnClick="ActionHandler" CommandName="CANCEL"  /><%--OnClientClick="javascript:return CancelFun();"--%>
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <div id="divQAC" runat="server">
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S">
                            <asp:Label runat="server" ID="lblSearchType" Text="Compare" AssociatedControlID="ddlCompareType"></asp:Label>
                            <asp:DropDownList ID="ddlCompareType" runat="server" AutoPostBack="false" OnChange="ddlSelectionChange('QAC');"
                                Enabled="false">
                                <asp:ListItem Value="2" Text="Amount"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Rate"></asp:ListItem>
                            </asp:DropDownList>
                            <%--%$ resources:Rate %>--%>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divTrialBal" runat="server">
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S">
                            <asp:Label runat="server" ID="lblDateFrom" Text="Date From" AssociatedControlID="txtFromDate"></asp:Label>
                            <asp:TextBox ID="txtFromDate" runat="server" MaxLength="100" CssClass="Uidate-picker" />
                            <asp:HiddenField ID="hdfFromDate" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S">
                            <asp:Label runat="server" ID="lblDateTo" Text="To" AssociatedControlID="txtToDate"></asp:Label>
                            <asp:TextBox ID="txtToDate" runat="server" MaxLength="100" CssClass="Uidate-picker" />
                            <asp:HiddenField ID="hdfToDate" runat="server" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divGL" runat="server">
            <table>
                <tr>
                    <td width="50%">
                        <div id="divAS" class="div2col-S" runat="server">
                            <asp:Label runat="server" ID="Label1" Text="Account" AssociatedControlID="txtAccount"></asp:Label>
                            <asp:TextBox ID="txtAccount" runat="server" MaxLength="100" />
                            <asp:HiddenField ID="hdfAccount" runat="server" />
                        </div>
                         <div id="divSAS" class="div2col-S" runat="server">
                            <asp:Label runat="server" ID="lblSubLedger" Text="Sub Ledger" AssociatedControlID="ddlSubLedger"></asp:Label>
                            <asp:DropDownList ID="ddlSubLedger" runat="server" CssClass="medium" onChange="javascript:getSubAccounts();" AutoPostBack="true"
                               OnSelectedIndexChanged="ActionHandler" ></asp:DropDownList>
                            <asp:TextBox ID="txtSubAccount" runat="server" MaxLength="100" CssClass="medium"/>
                            <asp:HiddenField ID="hdfSubAccount" runat="server" />
                        </div>
                    </td>
                    <td>
                    <div class="tree-labelSingle">
                           <asp:Label runat="server" ID="lblOr" Text="OR" AssociatedControlID="trvAccounts"  CssClass="txtAlign-center"></asp:Label>
                             <div class="treeview">
                            <asp:TreeView ID="trvAccounts" runat="server" ShowLines="True" ExpandDepth="0" ClientIDMode="Static"
                                ShowCheckBoxes="All">
                                <NodeStyle Font-Bold="True" />
                                <RootNodeStyle Font-Bold="True" />
                                <ParentNodeStyle Font-Bold="True" />
                            </asp:TreeView>
                        </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divGLSubType" runat="server" style="display:none">
            <table class="table-devide">
                <tr>
                    <td>
                       <%-- <div class="div2col-S">
                            <asp:Label runat="server" ID="Label3" Text="Sub Ledger" AssociatedControlID="txtSubType"></asp:Label>
                            <asp:DropDownList ID="ddlSubLedger" runat="server" CssClass="medium" onClick="javascript:getSubAccounts();"></asp:DropDownList>
                            <asp:TextBox ID="txtSubAccount" runat="server" MaxLength="100" CssClass="medium"/>
                            <asp:HiddenField ID="hdfSubAccount" runat="server" />
                        </div>--%>
                    </td>
                    <td>
                        <div class="div2col-S">
                           <%-- <asp:Label runat="server" ID="Label4" Text="Account" AssociatedControlID="txtSubAccount"></asp:Label>
                            
                            
                            <asp:Button ID="btnTreeView2" runat="server" ClientIDMode="Static" OnClientClick="javascript:return ShowAccountsTree();"
                                SkinID="btnInner-search" Height="20px" Width="5px" />--%>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <%-- <div>class="visiblefalse"--%>
        <div id="divAccPopUp" title="<%=Resources.Captions.ChooseAccount%>" runat="server">
            <%--<div class="Button-container-popup">--%>
            <%-- <asp:Button ID="btnAccountsOK" runat="server" SkinID="btnInner-ok" Text="Ok" CommandName="SELECTEDACC"
                    OnClick="ActionHandler" />--%>
            <%-- <asp:Button ID="btnAccountsOK" runat="server"  OnClick="ActionHandler" Text="Test" />--%>
            <%--       <asp:ImageButton ID="imbOk" runat="server"  OnClick="ActionHandler" />--%>
            <%--</div>--%>
            <%--<div class="treeview treeview-center">
                <asp:TreeView ID="trvAccounts" runat="server" ShowLines="True" ExpandDepth="0" ClientIDMode="Static" OnTreeNodeCheckChanged="LinksTreeView_CheckChanged"  
                    ShowCheckBoxes="All">
                    <NodeStyle Font-Bold="True" />
                    <RootNodeStyle Font-Bold="True" />
                    <ParentNodeStyle Font-Bold="True" />
                </asp:TreeView>
            </div>--%>
        </div>
        <%--   </div>--%>
        <div class="reportviewer">
            <rsweb:ReportViewer ID="rvViewReport" runat="server" BorderWidth="0" SizeToReportContent="true"
                Width="98%">
            </rsweb:ReportViewer>
        </div>
        <asp:HiddenField ID="hdfSelectedNodes" runat="server" />
    </div>
    <div class="clear">
    </div>
    <div class="visiblefalse">
        <asp:HiddenField ID="hdfAppType" runat="server" />
        <asp:HiddenField ID="hdfRefUrl" runat="server" ClientIDMode="Static" />
    </div>
     <div id="diverror" class="visiblefalse">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                   
                </div>
</asp:Content>
