<%@ Page Title="<%$ Resources:Captions,Title_Mailer %>" Language="C#" Theme="ClassicExt"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="Mailer.aspx.cs"
    Inherits="CustomerPortal.OrderToCash.Mailer" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script src="../Scripts/Jquery/ERPTimepicker.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");
        function HideFilter() {
            //<summary>Function Used to Hide Vendor Panel </summary>
            $("#imbHideFilter").hide();
            $("#imbShowFilter").show();
            $("#divFilterDetails").hide();
        }

        function ShowFilter() {
            //<summary>Function Used to Show Purchase Request Panel </summary>
            $("#imbHideFilter").show();
            $("#imbShowFilter").hide();
            $("#divFilterDetails").show();
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

        function ShowListing(flag) {
            ///<summary>
            /// Used to handle the Listing And Enrty Section in Page
            ///</summary>
            /// <param name="flag" optional="true" type="String">
            /// flag Determines the Mode if flag then in Listing else in Edit Mode
            /// </param>           
            if (flag) {
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlEntry]").hide();
                $("[id$=Mailer_List]").show();
            }
            else {
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlEntry]").show();
                $("[id$=Mailer_List]").hide();


            }
            return false;
        }

        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            // GrandScriptUtils.MakeAutoCompleteDDL("txtParty", url, "hdfParty", true, true, "CUSTOMER");
            var IsExistQueryStr = false;
            if (url.indexOf("?") != -1) {
                IsExistQueryStr = true
            }
            //GrandScriptUtils.MakeAutoCompleteDDL("txtReport", url + "&Type=" + $("[id$=hdfReportGroup]").val(), "hdfReport", true, true, "REPORT");
            if (IsExistQueryStr)
                GrandScriptUtils.MakeAutoCompleteDDL("txtParty", url + "&Type=" + $("[id$=hdfSendTo]").val(), "hdfParty", true, true, "GETPARTY");
            else
                GrandScriptUtils.MakeAutoCompleteDDL("txtParty", url + "?Type=" + $("[id$=hdfSendTo]").val(), "hdfParty", true, true, "GETPARTY");

            GrandScriptUtils.MakeAutoCompleteDDL("txtType", url, "hdfType", true, true, "GETTRXTYPE");

            if (IsExistQueryStr)
                GrandScriptUtils.MakeAutoCompleteDDL("txtMailStatus", url + "&Type=MAIL STATUS", "hdfMailStatus", true, true, "GETAPPCONFIG");
            else
                GrandScriptUtils.MakeAutoCompleteDDL("txtMailStatus", url + "?Type=MAIL STATUS", "hdfMailStatus", true, true, "GETAPPCONFIG");
            if (IsExistQueryStr)
                GrandScriptUtils.MakeAutoCompleteDDL("txtSendTo", url + "&Type=PARTY TYPE", "hdfSendTo", true, true, "GETAPPCONFIG");
            else
                GrandScriptUtils.MakeAutoCompleteDDL("txtSendTo", url + "?Type=PARTY TYPE", "hdfSendTo", true, true, "GETAPPCONFIG");

            if ($("[id$=txtParty]").attr("disabled") == true) {
                DisableAuto($("[id$=txtParty]"), $("[id$=hdfParty]"));
            }
            if ($("[id$=txtType]").attr("disabled") == true) {
                DisableAuto($("[id$=txtType]"), $("[id$=hdfType]"));
            }
            GrandScriptUtils.MakeAutoCompleteDDL("txtEnqNumber", url, "hdfEnqNumber", true, true, "DIRECTORDERNO");
            //GrandScriptUtils.MakeAutoCompleteDDL("txtQuotNumber", url, "hdfQuotNumber", true, true, "DIRECTORDERNO");

        }

        function AfterAutoCompleteSelect(targetControlID) {
            var IsExistQueryStr = false;
            if (url.indexOf("?") != -1) {
                IsExistQueryStr = true
            }
            if (targetControlID == "txtParty") {
                $("[id$=txtParty]").attr('title', $("[id$=txtParty]").val());
            }
            if (targetControlID == "txtType") {
                $("[id$=txtType]").attr('title', $("[id$=txtType]").val());
            }
            if (targetControlID == "txtSendTo") {
                $("[id$=hdfParty]").val("0");
                $("[id$=txtParty]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                if (IsExistQueryStr)
                    GrandScriptUtils.MakeAutoCompleteDDL("txtParty", url + "&Type=" + $("[id$=hdfSendTo]").val(), "hdfParty", true, true, "GETPARTY");
                else
                    GrandScriptUtils.MakeAutoCompleteDDL("txtParty", url + "?Type=" + $("[id$=hdfSendTo]").val(), "hdfParty", true, true, "GETPARTY");
            }
        }

        function ViewMode(mode) {
            ///<summary>
            /// Used to handle the view Mode
            ///</summary>
            /// <param name="mode" optional="true" type="String">
            /// Mode = 1 Determins ites on View Mode
            /// Mode 2 Draft View Mode
            /// Mode 3 List Mode
            /// Mode = 4 Listing

            /// </param>           
            if (mode == 1) {
                //                $("[id$=btnSave]").hide();
                $("[id$=btnSend]").hide();
                $("[id$=btnNew]").hide();
                $("[id$=btnView]").hide();
                $("[id$=btnEdit]").hide();
                $("[id$=imbAddContact]").hide();

            }
            else if (mode == 2) {
                $("[id$=btnNew]").show();
                $("[id$=btnView]").hide();
                $("[id$=btnEdit]").show();
                $("[id$=imbAddContact]").show();
                //                $("[id$=btnDelete]").hide();

            }
            else if (mode == 4) {
                $("[id$=btnNew]").hide();
                $("[id$=btnView]").hide();
                $("[id$=btnEdit]").hide();
                $("[id$=imbAddContact]").show();
                //                $("[id$=btnDelete]").hide();

            }
            else if (mode == 3) {
                $("[id$=btnNew]").show();
                $("[id$=btnView]").show();
                $("[id$=btnEdit]").hide();
                $("[id$=imbAddContact]").show();
                //                $("[id$=btnDelete]").hide();
            }

        }

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

        function CheckOtherIsCheckedByGVID(spanChk) {
            var IsChecked = spanChk.checked;
            var CurrentRdbID = spanChk.id;
            $("[id$=grdMailList]").find("tr:has(td)").each(function () {
                //type = $(this).find("td:first input").attr("type")
                var id = $(this).find("td:first input").attr("id");
                if (id != CurrentRdbID) {
                    $(this).find("td:first input").attr("checked", false);
                }
            });

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlCustomerRegistration">
        <ContentTemplate>
            <div class="fixed-buttons-normal" id="divFixedTab">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" Visible="false" CommandName="SAVE" TabIndex="9"
                                            Text="<%$resources:ErpRes,Save %>" OnClick="ActionHandler" ValidationGroup="Save"
                                            ToolTip="<%$resources:ErpRes,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save"
                                            OnClientClick="javascript:ValidatePageNow('Save')" />
                                    </li>
                                    <li runat="server" id="pnlSend">
                                        <asp:Button runat="server" ID="btnSend" CommandName="SEND" TabIndex="10" Text="<%$resources:ErpRes,Send %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Send %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-set" OnClientClick="javascript:ValidatePageNow('Save')" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnCancel" CommandName="CANCEL" TabIndex="11" Text="<%$resources:ErpRes,Cancel %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Cancel %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing">
                                    <li></li>
                                    <asp:Button runat="server" ID="btnDelete" Visible="false" CommandName="DELETE" TabIndex="10"
                                        Text="<%$resources:ErpRes,Delete %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Delete %>"
                                        CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" OnClientClick="javascript:ValidatePageNow('Save')" />
                                    </li><li>
                                        <asp:Button runat="server" ID="btnNew" SkinID="btnInner-New" CommandName="NEW" Text="<%$ Resources:ErpRes, New %>"
                                            TabIndex="12" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,New %>" CommandArgument="SEC_ActionPanel" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" Text="<%$ Resources:ErpRes, View %>"
                                            TabIndex="13" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,View %>" SkinID="btnInner-View"
                                            CommandArgument="SEC_ActionPanel" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnEdit" CommandName="EDIT" Text="<%$ Resources:ErpRes, Edit %>"
                                            TabIndex="14" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Edit %>" SkinID="btnInner-Edit"
                                            CommandArgument="SEC_ActionPanel" />
                                    </li>
                                    <%-- <li>  <asp:Button runat="server" ID="btnEdit" CommandName="EDIT" Text="<%$ Resources:gBudgetRes, Edit %>"
                                        TabIndex="13" OnClick="ActionHandler" SkinID="btnInner-Edit" CommandArgument="SEC_ActionPanel" />
                                    <asp:Button runat="server" ID="btnPrint" CommandName="PRINT" Text="<%$ Resources:gBudgetRes, Print %>"
                                        TabIndex="15" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print" OnClick="ActionHandler" /> </li> --%>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <%--use the width property of the below table corresponding to the contents in the page--%>
                <asp:Table runat="server" ID="tblTemplate">
                    <%--Rename this ID Page_Entry with the corresponding section Id in the documet--%>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <%--Align table cell according to design--%>
                        <asp:TableCell>
                            <div class="divcol-S">
                                <asp:Label runat="server" ID="lblTo" AssociatedControlID="txtTo" Text="<%$ resources:To %>"></asp:Label>
                                <asp:TextBox runat="server" ID="txtTo" ReadOnly="true" TabIndex="1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="vrfTo" CssClass="star" SetFocusOnError="true" ValidationGroup="Save"
                                    EnableClientScript="true" runat="server" ControlToValidate="txtTo" Display="Dynamic"
                                    Text="*" ErrorMessage="<%$ resources:Err_To %>">
                                </asp:RequiredFieldValidator>
                                <asp:ImageButton ID="imbAddContact" TabIndex="2" SkinID="plus" runat="server" OnClick="ActionHandler"
                                    ToolTip="<%$ resources:AddContact %>" CommandName="SHOWCUSTOMER" />
                                <asp:Label runat="server" ID="lblCCAddres" AssociatedControlID="txtCCAddress" Text="<%$ resources:Cc %>"></asp:Label>
                                <asp:TextBox runat="server" ID="txtCCAddress" TabIndex="3"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="reqCC" runat="server" ControlToValidate="txtCCAddress"
                                    CssClass="star" ErrorMessage="<%$ resources:Err_ValidCC %>" ValidationGroup="Save"
                                    EnableClientScript="true" Display="Dynamic" SetFocusOnError="true" Text="*" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([,]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*"></asp:RegularExpressionValidator>
                                
                                <asp:Label runat="server" ID="lblBCCAddress" AssociatedControlID="txtBCCAddress"
                                    Text="<%$ resources:Bcc %>"></asp:Label>
                                <asp:TextBox runat="server" ID="txtBCCAddress" TabIndex="4"></asp:TextBox>
                               <asp:RegularExpressionValidator ID="reqBCC" runat="server" ControlToValidate="txtBCCAddress"
                                    CssClass="star" ErrorMessage="<%$ resources:Err_ValidBCC %>" ValidationGroup="Save"
                                    EnableClientScript="true" Display="Dynamic" SetFocusOnError="true" Text="*" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([,]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*"></asp:RegularExpressionValidator>
                                <asp:Label runat="server" ID="lblSubject" AssociatedControlID="txtSubject" Text="<%$ resources:MailSubject %>"></asp:Label>
                                <asp:TextBox runat="server" ID="txtSubject" TabIndex="5"></asp:TextBox>
                                <asp:Label runat="server" ID="lblContent" AssociatedControlID="txtContent" Text="<%$ resources:MailContent %>"></asp:Label>
                                <asp:TextBox runat="server" EnableTheming="false" ID="txtContent" TextMode="MultiLine"
                                    Height="100px" CssClass="multilarge" TabIndex="6"></asp:TextBox>
                                <%-- onkeypress="return this.value.length<500"
                                     onpaste="return this.value.length<500"--%>
                                <asp:RequiredFieldValidator ID="vrfContent" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="Save" EnableClientScript="true" runat="server" ControlToValidate="txtContent"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_MailContent %>">
                                </asp:RequiredFieldValidator>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <%--Rename this ID Page_List with the corresponding section Id in the documet--%>
                    <asp:TableRow ID="Mailer_List">
                        <asp:TableCell>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Captions", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="16" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="16" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="margin-top: 8px;">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblFromDate" Text="<%$ resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFromDate" CssClass="medium" TabIndex="3" onkeydown="return CheckKey(event)"
                                                onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblSendTo" runat="server" Text="<%$resources:SendTo %>" AssociatedControlID="txtSendTo"></asp:Label>
                                            <asp:TextBox ID="txtSendTo" runat="server" CssClass="half" MaxLength="100" TabIndex="5"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfSendTo" runat="server" />
                                            <asp:TextBox ID="txtParty" runat="server" CssClass="half" MaxLength="100" TabIndex="5"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfParty" runat="server" />
                                            <div class="clear">
                                            </div>
                                           <asp:Label ID="lblSearchSubject" runat="server" Text="<%$resources:Subject %>" AssociatedControlID="txtSerachSubject"></asp:Label>
                                           <asp:TextBox ID="txtSerachSubject" runat="server" TabIndex="7" MaxLength="500" Width="250px"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblToDate" Text="<%$ resources:ToDate %>" AssociatedControlID="txtToDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtToDate" CssClass="medium" TabIndex="4" onkeydown="return CheckKey(event)"
                                                onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <%--  <asp:Label ID="lblSearchSubj" runat="server" Text="<%$resources:Subject %>" AssociatedControlID="txtSearchSubject"></asp:Label>
                                            <asp:TextBox ID="txtSearchSubject" runat="server" CssClass="large" MaxLength="100"
                                                TabIndex="6"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfSubject" runat="server" />--%>
                                            <asp:Label ID="lblType" runat="server" Text="<%$resources:Type %>" AssociatedControlID="txtType"></asp:Label>
                                            <asp:TextBox ID="txtType" runat="server" CssClass="large" MaxLength="100" TabIndex="6"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfType" runat="server" />
                                            <div class="clear">
                                            </div>
                                             <asp:Label ID="lblMailStatus" runat="server" Text="<%$resources:Status %>" AssociatedControlID="txtMailStatus"></asp:Label>
                                            <asp:TextBox ID="txtMailStatus" runat="server" CssClass="large" MaxLength="100" TabIndex="8"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfMailStatus" Value="-1" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblSearchButton" runat="server" AssociatedControlID="btnSearch"></asp:Label>
                                            <asp:Button ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$ resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="9"
                                                CommandName="SEARCH" SkinID="btnInner-search" />
                                            <asp:Button ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear %>" TabIndex="9"
                                                ToolTip="<%$ resources:Controls,Clear %>" OnClick="ActionHandler" CommandName="CLEAR"
                                                SkinID="btnInner-cancel-dsd" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <%-- use the grid to list the records in the page--%>
                                <asp:GridView ID="grdMailList" runat="server" AutoGenerateColumns="False" Width="100%"
                                    OnPageIndexChanging="ActionHandler" PageSize="<%$ resources:PageSize%>" AllowSorting="false"
                                    OnSorting="ActionHandler" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton runat="server" GroupName="SelectOne" ID="rbtSelect" onclick="javascript:CheckOtherIsCheckedByGVID(this);" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date %>" SortExpression="MLQ_CRTD_DT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("MLQ_DATE", Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval("MLQ_DATE", Resources.Constants.DateFormatGrid) %>'>
                                                                  <%--  MLQ_CRTD_DT Previous Field --%>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:To %>" SortExpression="MLQ_TO">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfCurPK" runat="server" Value='<%# Eval("MLQ_PK") %>' />
                                                <asp:HiddenField ID="hdfCusPK" runat="server" Value='<%# Eval("PARTY_PK") %>' />
                                                <asp:Label ID="lblTo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("MLQ_TO"),30) %>'
                                                    ToolTip='<%# Eval("MLQ_TO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Name %>" SortExpression="PARTY_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("PARTY_NAME"),47) %>'
                                                    ToolTip='<%# Eval("PARTY_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Subject %>" SortExpression="MLQ_SUBJECT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubject" runat="server" Text='<%# Eval("MLQ_SUBJECT") %>' ToolTip='<%# Eval("MLQ_SUBJECT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Cc %>" SortExpression="MLQ_TO_CC">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMailCc" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("MLQ_TO_CC"),30) %>'
                                                    ToolTip='<%# Eval("MLQ_TO_CC") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Bcc %>" SortExpression="MLQ_TO_BCC">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMailBcc" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("MLQ_TO_BCC"),30) %>'
                                                    ToolTip='<%# Eval("MLQ_TO_BCC") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Attempt %>" SortExpression="MLQ_ATTEMPT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAttempt" runat="server" Text='<%# Eval("MLQ_ATTEMPT") %>' ToolTip='<%# Eval("MLQ_ATTEMPT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="false" />
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Status %>" SortExpression="MLQ_STATUS_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsTATUSt" runat="server" Text='<%# Eval("MLQ_STATUS_TEXT") %>' ToolTip='<%# Eval("MLQ_STATUS_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="false" />
                                            <ItemStyle Width="5%" Wrap="false" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                                <%-- Leave this table as such --%>
                            </div>
                            <div class="clear">
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" runat="server" CssClass="last-modified" Visible="true">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
            </div>
            <div id="divCustomers" style="display: none;">
                <div class="Button-container-popup">
                    <asp:Button runat="server" ID="btnApplayCustomer" CommandName="CUSTOMEREMAIL" TabIndex="23"
                        Text="<%$resources:ErpRes,Apply %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Apply %>"
                        SkinID="btnInner-ok" />
                    <asp:Button runat="server" ID="btnCancelCustomer" CommandName="CUSTOMERCANCEL" TabIndex="24"
                        Text="<%$resources:ErpRes,Cancel %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Cancel %>"
                        SkinID="btnInner-Cancel" />
                </div>
                <div class="content-wrapper">
                    <div class="treeview max-200">
                        <asp:TreeView ID="trvCustomers" runat="server" ShowLines="true" onclick="OnCheckBoxCheckChanged(event);"
                            ExpandDepth="0" InitialExpandDepth="2" ShowCheckBoxes="Parent,Leaf"  >
                        </asp:TreeView>
                    </div>
                </div>
            </div>
            <div id="divPopUp" style="display: none">
                <div class="mailview-details">
                    <%--  All the controls will be placed here --%>
                    <asp:Label runat="server" ID="lbnToMail" AssociatedControlID="lblToMail" Text="<%$ resources:To %>"></asp:Label>
                    <asp:Label runat="server" ID="lblToMail"></asp:Label>
                    <div class="clear">
                    </div>
                    <asp:Label runat="server" ID="lbnMailSubject" AssociatedControlID="lblSubject" Text="<%$ resources:Subject %>"></asp:Label>
                    <asp:Label runat="server" ID="lblMailSubject"></asp:Label>
                    <div class="clear">
                    </div>
                    <asp:Label runat="server" ID="lbnCc" AssociatedControlID="lblSubject" Text="<%$ resources:Cc %>"></asp:Label>
                    <asp:Label runat="server" ID="lblCc"></asp:Label>
                    <div class="clear">
                    </div>
                    <asp:Label runat="server" ID="lbnBcc" AssociatedControlID="lblSubject" Text="<%$ resources:Bcc %>"></asp:Label>
                    <asp:Label runat="server" ID="lblBcc"></asp:Label>
                    <div class="clear">
                    </div>
                </div>
                <div class="mailmessage-wrap">
                    <div class="mailmessage-inwrap">
                        <asp:Literal ID="ltContent" runat="server"></asp:Literal></div>
                </div>
                <div id="divAttachment" runat="server">
                    <asp:GridView ID="grdMailAttachments" runat="server" AutoGenerateColumns="False"
                        Width="100%" AllowSorting="false" OnSorting="ActionHandler" EmptyDataRowStyle-CssClass="emptytable">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ resources:Attachments %>">
                                <ItemTemplate>
                                    <%--<a id="Attachment" target="_blank" href='<%# Page.ResolveClientUrl(GetLocalResourceObject("MailAttachDownloadPath").ToString() + Eval("ADD_NAME")) %> '
                                        title='<%# Eval("ADD_NAME") %>'>
                                        <%# Eval("ADD_NAME") %></a>--%>
                                    <a id="Attachment" target="_blank" href='<%# ConfigurationManager.AppSettings["UploadPath"].ToString()+"Attachments/"+ Eval("ADD_NAME") %>'
                                        title='<%# Eval("ADD_NAME") %>'>
                                        <%# Eval("ADD_NAME") %></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <asp:Table ID="Table2" runat="server">
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="2" ID="TableCell1" CssClass="SEC_ACTION" align="center">
                            <ul>
                                <li runat="server" id="Li1">
                                    <asp:Button runat="server" ID="btnResend" CommandName="RESEND" Text="<%$ resources:Resend %>"
                                        OnClick="ActionHandler" />
                                    <%--  CommandArgument="<%$ resources:Section2 %>" --%>
                                    <asp:Button runat="server" ID="btnSendCancel" CommandName="CANCEL" Text="<%$ Resources:ErpRes, Cancel %>"
                                        OnClientClick="javascript:ClosePopup()" />
                                    <%--  CommandArgument="<%$ resources:Section2 %>"--%>
                                </li>
                            </ul>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="tblRowModOn" runat="server">
                        <asp:TableCell>
                            <div class="mail-activity">
                                <asp:Label runat="server" ID="lbnAttemptedOn" AssociatedControlID="lblAttempTime"
                                    Text="<%$ resources:LastAttempt %>"></asp:Label>
                                <asp:Label ID="lblAttempTime" runat="server"></asp:Label>
                                <asp:Label runat="server" ID="lbnMailStatus" AssociatedControlID="lblMailSendStatus"
                                    Text="<%$ resources:Status %>"></asp:Label>
                                <asp:Label ID="lblMailSendStatus" runat="server"></asp:Label>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
        </ContentTemplate>
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btnAttach" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
