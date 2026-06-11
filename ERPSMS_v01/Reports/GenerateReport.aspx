<%@ Page Title="<%$ Resources:Captions,Title_Print %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="GenerateReport.aspx.cs" Inherits="ERPSMS_v01.Reports.GenerateReport"
    Theme="ClassicExt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/CheckListSearchControl.ascx" TagName="CheckListSearchControl"
    TagPrefix="uc1" %>
<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"

    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Src="~/Reports/UserControls/usrFinYearDateFilter.ascx" TagName="DateFilter" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        select#ctl00_MainContent_ddlLocation {
            max-width: 85px !important;
        }
    </style>
    <script src="../Scripts/GrantPrintUtility.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var UIurl = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");
        var typeText = "";

        function ddlSelectionChange(cnme) {
            $("[id$=btnSearch]").attr('CommandName', cnme);
            $("[id$=btnSearch]").click();
            return false;
        }
        $(document).ready(function () { InitComponents(); });
        function InitComponents() {
            debugger;
            typeText = "Type min. 4 characters";
            var pageURL = window.document.URL;
            var virtualPath = '<%= (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString()) %>';
            if (UIurl.indexOf("?") != -1) {
                //                GrandScriptUtils.MakeAutoCompleteDDL("txtParty", UIurl + "&Type=" + $("[id$=hdfSendTo]").val(), "hdfParty", true, true, "GETPARTY");
                GrandScriptUtils.MakeAutoCompleteDDL("txtSendTo", UIurl + "&Type=PARTY TYPE", "hdfSendTo", true, true, "GETAPPCONFIG");
                GrandScriptUtils.MakeAutoCompleteDDL("txtSubAccount", url + "&AccType=" + subLedgr, "hdfSubAccount", true, true, "SUBACCOUNTS");
            }
            else {
                //                GrandScriptUtils.MakeAutoCompleteDDL("txtParty", UIurl + "?Type=" + $("[id$=hdfSendTo]").val(), "hdfParty", true, true, "GETPARTY");
                GrandScriptUtils.MakeAutoCompleteDDL("txtSendTo", UIurl + "?Type=PARTY TYPE", "hdfSendTo", true, true, "GETAPPCONFIG");
            }
            var ComPK = $("[id$=ddlLocation]").val();

            var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
            if ($("[id$=hdfSubType]").val() == '3') {
                //GrandScriptUtils.MakeAutoCompleteDDL("txtAccount", url + "?VoucherType=&AccType=3", "hdfAccount", true, true, "ACCOUNT");

                GrandScriptUtils.MakeAutoCompleteDDLNEW("txtAccount", url + "?VoucherType=&AccType=3&CompanyPK=" + ComPK, "hdfAccount", true, true, 4, "ACCOUNT", "", "", "", "", typeText);


            }
            else {
                //GrandScriptUtils.MakeAutoCompleteDDL("txtAccount", url + "?VoucherType=&AccType=", "hdfAccount", true, true, "ACCOUNT");
                //GrandScriptUtils.MakeAutoCompleteComboBox("txtAccount", url + "?VoucherType=&AccType=", "hdfAccount", "", true, true, "", false, false, false, false, 4, typeText,"ACCOUNT")
                // GrandScriptUtils.MakeAutoCompleteLimitLen("txtAccount", url + "?VoucherType=&AccType=", "hdfAccount", true, false, "", true, "", "", "", 4, "", typeText, false,"ACCOUNT");
                GrandScriptUtils.MakeAutoCompleteDDLNEW("txtAccount", url + "?VoucherType=&AccType=&CompanyPK=" + ComPK, "hdfAccount", true, true, 4, "ACCOUNT", "", "", "", "", typeText);
            }
            if ($("[id$=ddlSubLedger]").val() != "-1") {
                var subLedgr = $("[id$=ddlSubLedger]").val();
                if ($("[id$=hdfAppType]").val() == 'BRC')
                    subLedgr = '11';
                GrandScriptUtils.MakeAutoCompleteDDL("txtSubAccount", url + "&AccType=" + subLedgr, "hdfSubAccount", true, true, "SUBACCOUNTS");
            }

            // GrandScriptUtils.AddDateRange("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", "dd-M-yy", false, false, false);
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false, false);
            GetReferrer();
        }
        //function InitAccounts()
        //{  
        //var ComPK = $("[id$=ddlLocation]").val();
        //typeText = "Type min. 4 characters";

        //$("[id$=hdfAccount]").val('0');  
        //$("[id$=txtAccount]").val(typeText);
        //var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        //if ($("[id$=hdfSubType]").val() == '3') {
        //        GrandScriptUtils.MakeAutoCompleteDDLNEW("txtAccount", url + "?VoucherType=&AccType=3&CompanyPK="+ComPK, "hdfAccount", true, true, 4, "ACCOUNT", "", "", "", "", typeText);
        // }
        // else {
        //  GrandScriptUtils.MakeAutoCompleteDDLNEW("txtAccount", url + "?VoucherType=&AccType=&CompanyPK="+ComPK, "hdfAccount", true, true, 4, "ACCOUNT", "", "", "", "", typeText);
        // }
        //} 

        function AfterAutoCompleteSelect(targetControlID) {

            if (targetControlID == "txtSendTo") {
                $("[id$=hdfParty]").val("0");
                //                $("[id$=txtParty]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                if (UIurl.indexOf("?") != -1) {
                    //                    GrandScriptUtils.MakeAutoCompleteDDL("txtParty", UIurl + "&Type=" + $("[id$=hdfSendTo]").val(), "hdfParty", true, true, "GETPARTY");
                }
                else {
                    //                    GrandScriptUtils.MakeAutoCompleteDDL("txtParty", UIurl + "?Type=" + $("[id$=hdfSendTo]").val(), "hdfParty", true, true, "GETPARTY");
                }

                $("[id$=btnSendTo]").click();

                // 
            }
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
    </script>
    <script type="text/javascript">

        function backToList(returnUrl) {
            //window.location = "../journalize/JournalizeListing.aspx?Type=JV";
            window.location = returnUrl;
        }

        function printVoucher(template, printerSettings, extraCss) {
            var windowWidth = printerSettings.width;
            var windowHeight = printerSettings.height;
            var mode = "popup"; //"iframe";
            var close = false;
            var extraCss = extraCss === undefined || extraCss == null ? '' : extraCss;   //"../Css/petty-cash-style.css";
            var keepAttr = true;
            var headElements = '<meta charset="utf-8" />,<meta http-equiv="X-UA-Compatible" content="IE=edge"/>' +
                '<style type="text/css">' +
                '@page { size: ' + printerSettings.width + ' ' + printerSettings.height + '; margin: 0.53cm; page-break-after:avoid; page-break-inside : avoid ;}' +
                '.grid' +
                '{' +
                '    border:1px solid black;' +
                '    border-collapse:collapse;' +
                '    width:97%; ' +
                '    margin-left:0.04in;' +
                '    margin-bottom: 0.7cm;  ' +
                '    margin-top: 0.25cm;  ' +
                '    padding:6px;' +
                '    letter-spacing:2pt;' +
                '    font-family:Arial;' +
                '    font-size:60%;' +
                '}' +
                '.grid thead tr th' +
                '{' +
                '   border:1px solid black;' +
                '   padding:4px;' +
                '}' +
                ' .grid tbody tr' +
                '{' +
                '}' +
                ' .grid tbody tr td' +
                '{' +
                '  padding:4px;' +
                '}' +

                ' .grid tfoot tr  th' +
                '{' +
                '   border:2px solid black;' +
                '   padding:4px;' +
                '}' +
                '</style>';
            var popWd = printerSettings.windowWidth;
            var popHt = printerSettings.windowHeight;
            var popX = 0;
            var popY = 0;
            var options = { mode: mode, popHt: popHt, popWd: popWd, popX: popX, popY: popY, popClose: close, extraCss: extraCss, retainAttr: keepAttr, extraHead: headElements };
            $(template).grandPrint(options);
        }
        function ShowHideAdvancedSearch(flag) {
            //If flag then Show AdvancedSearch
            if (flag) {
                $("[id$=tbladvancedSearch]").show();
                $("[id$=imbShowFilter]").hide();
                $("[id$=imbHideFilter]").show();
            }
            else {
                $("[id$=tbladvancedSearch]").hide();
                $("[id$=imbShowFilter]").show();
                $("[id$=imbHideFilter]").hide();
            }
            return false;
        }
    </script>
    <style type="text/css">
        .dialogzone {
            width: 100% !important;
            overflow-x: hidden !important;
            overflow-y: auto !important;
        }

            .dialogzone table {
                width: auto !important;
                float: left;
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
                        <ul>
                            <li>
                                <%--<asp:Button ID="btnReport" runat="server" Text="Print" OnClick="ActionHandler" CommandName="PRINT" />--%>
                                <asp:Button runat="server" ID="btnReport" CommandName="PRINT" OnClick="ActionHandler"
                                    Text="<%$resources:Controls,Print %>" SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
                            </li>
                        </ul>
                        <ul runat="server" id="pnlListing">
                            <li>
                                <asp:Button ID="btnSearch" runat="server" ClientIDMode="Static" OnClick="ActionHandler"
                                    Text="View" CommandName="VIEW" SkinID="btnInner-View" />
                            </li>
                            <li>
                                <asp:Button ID="BtnExcelExport" runat="server" ClientIDMode="Static" OnClick="ActionHandler"
                                    Text="Excel Export" SkinID="btnInner-View" CommandName="EXCEL"  
                                    ToolTip="Export to Excel"      />
                            </li>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" OnClick="ActionHandler" CommandName="CANCEL"
                                    SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Close%>" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <div class="search-colapse">
            <table>
                <tr>
                    <td>
                        <h1>
                            <%= GetGlobalResourceObject("Controls", "FilterBy").ToString()%></h1>
                    </td>
                    <td>
                        <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                            ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                            TabIndex="65" />
                        <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                            ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                            TabIndex="66" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="clear">
        </div>
        <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
            <tr id="Tr1" runat="server">
                <td>
                    <div id="divQAC" runat="server">
                        <table class="table-devide">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label runat="server" ID="lblSearchType" Text="Compare" AssociatedControlID="ddlCompareType"></asp:Label>
                                        <asp:DropDownList ID="ddlCompareType" runat="server" AutoPostBack="false" onchange="ddlSelectionChange('QAC');"
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
                                        <asp:Label runat="server" ID="lblDateFrom" Text="From" AssociatedControlID="txtFromDate"
                                            onkeydown="return false" onpaste="return false"></asp:Label>
                                        <asp:TextBox ID="txtFromDate" runat="server" MaxLength="100" CssClass="Uidate-picker" />
                                        <asp:HiddenField ID="hdfFromDate" runat="server" />

                                             <asp:Label runat="server" ID="lblDateTo" Text="To" AssociatedControlID="txtToDate"></asp:Label>
                                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="100" CssClass="Uidate-picker"
                                            onkeydown="return false" onpaste="return false" />
                                        <asp:HiddenField ID="hdfToDate" runat="server" />
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                   
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table>
                        <tr>
                            <td id="tdUsrDate" runat="server">
                                <uc2:DateFilter ID="usrDateFilter" runat="server" />
                            </td>
                            <td>
                                <div id="divLocation" runat="server" visible="false">
                                    <table class="table-devide tablelayout">
                                        <tr>
                                            <td width="50%">
                                                <div class="padgtop7" id="div2" runat="server">
                                                    <div id="divLoc" runat="server">
                                                        <asp:Label runat="server" ID="lblLocation" Text="Location" AssociatedControlID="ddlLocation"
                                                            CssClass="lbl-24-3perc"></asp:Label>
                                                        <asp:DropDownList ID="ddlLocation" runat="server" CssClass="select-w67per" 
                                                            AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td id="td1" runat="server" visible="false">
                            </td>
                        </tr>
                    </table>


                    <div id="divFY" runat="server">
                        <table class="table-devide">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="Label3" runat="server" Text="<%$ resources:MISFilterLabel,Finyear %>" AssociatedControlID="ddlFinYear"></asp:Label>
                                        <asp:DropDownList ID="ddlFinYear" runat="server" CssClass="select-half"
                                            OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divGL" runat="server">
                        <table class="table-devide tablelayout">
                            <tr>
                                <td width="50%">
                                    <div id="divAS" runat="server">
                                        <%--<div id="divLocation" runat="server" visible="false">
                                            <asp:Label runat="server" ID="lblLocation" Text="Location" AssociatedControlID="ddlLocation"
                                                CssClass="middle-lbl-xsmall-c"></asp:Label>
                                            <asp:DropDownList ID="ddlLocation" runat="server" CssClass="medium" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>
                                        </div>--%>
                                        <asp:Label runat="server" ID="Label1" Text="Account" AssociatedControlID="txtAccount"
                                            CssClass="middle-lbl-xsmall-c"></asp:Label>
                                        <asp:TextBox ID="txtAccount" runat="server" MaxLength="100" CssClass="select-w33per" />
                                        <asp:HiddenField ID="hdfAccount" runat="server" />
                                        <asp:HiddenField ID="hdfSubType" runat="server" />
                                    </div>
                                    <div id="divSAS" runat="server">
                                        <asp:Label runat="server" ID="lblSubLedger" Text="Sub Ledger" AssociatedControlID="ddlSubLedger"
                                            CssClass="middle-lbl-xsmall-c"></asp:Label>
                                        <asp:DropDownList ID="ddlSubLedger" runat="server" CssClass="medium" onchange="javascript:getSubAccounts();"
                                            AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtSubAccount" runat="server" MaxLength="100" CssClass="select-w22-6per" />
                                        <asp:HiddenField ID="hdfSubAccount" runat="server" />
                                    </div>
                                    <div id="divPSAS" runat="server">
                                        <table class="table-devide">
                                            <tr>
                                                <td>
                                                    <div class="div2col-S">
                                                        <asp:Label ID="lblSendTo" runat="server" Text="Party Type " AssociatedControlID="txtSendTo"></asp:Label>
                                                        <asp:TextBox ID="txtSendTo" runat="server" CssClass="select-w66per margn-lft3" MaxLength="100"
                                                            TabIndex="5"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfSendTo" runat="server" />
                                                        <asp:Button ID="btnSendTo" runat="server" OnClick="ActionHandler" CommandName="PARTY"
                                                            EnableTheming="false" Style="display: none" />
                                                    </div>

                                                </td>
                                                <td>
                                                    <div class="div2col-S">
                                                        <asp:Label ID="lblInType" runat="server" Text="Type" AssociatedControlID="ddlInvoType"></asp:Label>
                                                        <asp:DropDownList ID="ddlInvoType" runat="server" OnSelectedIndexChanged="ActionHandler"
                                                            AutoPostBack="true" TabIndex="7" CssClass="select-w66per margn-lft3 margnbotm0">
                                                            <asp:ListItem Text="Select" Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="div_Party" class="div2col-S" runat="server">
                                        <asp:Label ID="lblInParty" runat="server" Text="Party" AssociatedControlID="ddlInvoType"
                                            CssClass="input-w12-5per"></asp:Label>
                                        <asp:DropDownList ID="ddlParty" runat="server" Width="400px" maxlength="100" TabIndex="5"
                                            AppendDataBoundItems="true">
                                            <asp:ListItem Text="Select" Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hdfParty" runat="server" />
                                    </div>
                                    <div class="tree-labelSingle" style="margin-left: /*490*/619px; margin-top: -30px;">
                                        <asp:Label runat="server" ID="lblOr" Text="OR" AssociatedControlID="trvAccounts"
                                            CssClass="txtAlign-center"></asp:Label>
                                        <asp:Label runat="server" ID="Label2" CssClass="lbl-4-1perc"></asp:Label>
                                        <div id="divchecklist" class="w72perc" runat="server" style="display: inline-block; margin-left: 6.2%; width: 75%;">
                                            <uc1:CheckListSearchControl ID="CheckListSearchControl1" Visible="false" runat="server" />
                                        </div>
                                        <div class="treeview">
                                            <asp:TreeView ID="trvAccounts" runat="server" ShowLines="True" ExpandDepth="0" ClientIDMode="Static"
                                                OnTreeNodeCheckChanged="ActionHandler" ShowCheckBoxes="All" CssClass="middle-lbl-xsmall-c">
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

                    <div id="divGLSubType" runat="server" style="display: none">
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
                </td>
            </tr>
        </table>
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
        <div class="reportviewer" id="divReportViewer" runat="server">
            <%-- <asp:ScriptManager ID="sm" runat="server"></asp:ScriptManager>--%>
            <%--        <rsweb:ReportViewer ID="rvViewReport" runat="server" BorderWidth="0" SizeToReportContent="true"
                Width="98%">
            </rsweb:ReportViewer>--%>

            <rsweb:ReportViewer ID="rvViewReport" runat="server" BorderWidth="0" SizeToReportContent="true"
                Width="98%">
            </rsweb:ReportViewer>


        </div>
        <div id="divNodata" class="nodata" runat="server" visible="false">
            No Record Found
        </div>
        <asp:HiddenField ID="hdfShowCrReportDiv" runat="server" Value="0" />
        <asp:HiddenField ID="hdfAppTypeRpt" Value="" runat="server" />
        <asp:HiddenField ID="hdfRptNameRpt" Value="" runat="server" />
        <div id="divCrystalReportViewer" runat="server">
            <CR:CrystalReportViewer ID="GERP_Report" HasToggleParameterPanelButton="false" ToolPanelView="None"
                runat="server" AutoDataBind="true" HyperlinkTarget="_blank" />
        </div>
        <asp:HiddenField ID="hdfSelectedNodes" runat="server" />
        <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
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
        <asp:HiddenField ID="hdfCompanyPK" runat="server" />
    </div>
</asp:Content>
