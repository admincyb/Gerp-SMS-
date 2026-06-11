<%@ Page Title="<%$ Resources:Captions,Title_MailTemplate %>" Language="C#" Theme="ClassicExt"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="MailTemplate.aspx.cs"
    ValidateRequest="false" Inherits="CustomerPortal.OrderToCash.MailTemplate" %>

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
            /// Mode = 1 view mode
            /// Mode 2 new mode
            /// Mode 3 List Mode
            /// Mode = 4 entry mode

            /// </param>
            if (mode == 1) {
                $("[id$=btnSave]").hide();
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
                $("[id$=btnEdit]").show();
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
                CheckValidationDuplicate(valGroup);
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


        function ValidateEmailCC(sender, args) {
            var mailCc = $("[id$=txtCc]").val();
            var CcIds = mailCc.split(",");
            var pattern = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            for (i = 0; i < CcIds.length; i++) {
                if (pattern.test(CcIds[i])) {
                    args.IsValid = true;
                }
                else {
                    args.IsValid = false;
                    break;
                }
            }
        }

        function ValidateEmailBCC(sender, args) {
            var mailBcc = $("[id$=txtBcc]").val();
            var BccIds = mailBcc.split(",");
            var pattern = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            for (i = 0; i < BccIds.length; i++) {
                if (pattern.test(BccIds[i])) {
                    args.IsValid = true;
                }
                else {
                    args.IsValid = false;
                    break;
                }
            }
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
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="9" Text="<%$resources:ErpRes,Save %>"
                                            OnClick="ActionHandler" ValidationGroup="Save" ToolTip="<%$resources:ErpRes,Save %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" OnClientClick="javascript:ValidatePageNow('Save')" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnCancel" CommandName="CANCEL" TabIndex="11" Text="<%$resources:ErpRes,Cancel %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Cancel %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing">
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" Text="<%$ Resources:ErpRes, View %>"
                                            TabIndex="13" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,View %>" SkinID="btnInner-View"
                                            CommandArgument="SEC_ActionPanel" /></li>
                                    <li>
                                        <asp:Button runat="server" ID="btnEdit" CommandName="EDIT" Text="<%$ Resources:ErpRes, Edit %>"
                                            TabIndex="14" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Edit %>" SkinID="btnInner-Edit"
                                            CommandArgument="SEC_ActionPanel" />
                                    </li>
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
                                <asp:Label runat="server" ID="lblName" AssociatedControlID="txtCc" Text="<%$ resources:TemplateName %>"></asp:Label>
                                <asp:TextBox runat="server" ID="txtTemplateName" TabIndex="1" onkeydown="limitText(this,200);"
                                    onkeyup="limitText(this,200);"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="vrfTemplateName" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="Save" EnableClientScript="true" runat="server" ControlToValidate="txtTemplateName"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TemplateName %>">
                                </asp:RequiredFieldValidator>
                                <asp:Label runat="server" ID="lblMailFrom" AssociatedControlID="txtMailFrom" Text="<%$ resources:MailFom %>"></asp:Label>
                                <asp:TextBox runat="server" ID="txtMailFrom" TabIndex="1" onkeydown="limitText(this,200);"
                                    onkeyup="limitText(this,200);" ></asp:TextBox>
                                <asp:Label runat="server" ID="lblCc" AssociatedControlID="txtCc" Text="<%$ resources:Cc %>"></asp:Label>
                                <asp:TextBox runat="server" ID="txtCc" TabIndex="1" onkeydown="limitText(this,4000);"
                                    onkeyup="limitText(this,4000);"></asp:TextBox>
                                <asp:CustomValidator ID="customCC" runat="server" ValidateEmptyText="false" ClientValidationFunction="ValidateEmailCC"
                                    ErrorMessage="<%$ resources:Err_InvalidEmail %>" Text="*" EnableClientScript="true"
                                    ControlToValidate="txtCc" CssClass="star" Display="Dynamic" ValidationGroup="Save"></asp:CustomValidator>
                                <asp:Label runat="server" ID="lblBcc" AssociatedControlID="txtBcc" Text="<%$ resources:Bcc %>"></asp:Label>
                                <asp:TextBox runat="server" ID="txtBcc" TabIndex="1" onkeydown="limitText(this,4000);"
                                    onkeyup="limitText(this,4000);"></asp:TextBox>
                                <asp:CustomValidator ID="CustomBcc" runat="server" ValidateEmptyText="false" ClientValidationFunction="ValidateEmailBCC"
                                    ErrorMessage="<%$ resources:Err_InvalidEmail %>" Text="*" EnableClientScript="true"
                                    ControlToValidate="txtBcc" CssClass="star" Display="Dynamic" ValidationGroup="Save"></asp:CustomValidator>
                                <asp:Label runat="server" ID="lblSubject" AssociatedControlID="txtSubject" Text="<%$ resources:MailSubject %>"></asp:Label>
                                <asp:TextBox runat="server" ID="txtSubject" TabIndex="3" onkeydown="limitText(this,200);"
                                    onkeyup="limitText(this,200);"></asp:TextBox>
                                <asp:Label runat="server" ID="lblContent" AssociatedControlID="txtContent" Text="<%$ resources:MailContent %>"></asp:Label>
                                <asp:TextBox runat="server" EnableTheming="false" ID="txtContent" TextMode="MultiLine"
                                    Height="100px" CssClass="multilarge" TabIndex="4"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="vrfContent" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="Save" EnableClientScript="true" runat="server" ControlToValidate="txtContent"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_MailContent %>">
                                </asp:RequiredFieldValidator>
                            </div>
                        </asp:TableCell>
                        <asp:TableCell>
                            <div class="gridwrap" style="width: 250px;">
                                <asp:GridView ID="grdParameters" runat="server" AutoGenerateColumns="False" Width="100%"
                                    EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Position %>" SortExpression="TMT_TAG">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPrmName" runat="server" Text='<%# Eval("TMT_TAG") %>' ToolTip='<%# Eval("TMT_TAG") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Parameter %>" SortExpression="TMT_DESC">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSPrmDesc" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("TMT_DESC"),47) %>'
                                                    ToolTip='<%# Eval("TMT_DESC") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
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
                                                TabIndex="17" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblTmplName" Text="<%$ resources:TemplateName %>" AssociatedControlID="txtTmplName"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTmplName" CssClass="input-half margnbotm0" TabIndex="3"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <%--    <asp:Button ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$ resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="7"
                                                CommandName="SEARCH" SkinID="btnInner-search" />
                                            <asp:Button ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear %>" TabIndex="8"
                                                ToolTip="<%$ resources:Controls,Clear %>" OnClick="ActionHandler" CommandName="CLEAR"
                                                SkinID="btnInner-cancel-dsd" />--%>
                                            <asp:ImageButton ID="imbtnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="10" CssClass="margntop2 margnbotm0" CommandName="SEARCH"
                                                SkinID="search-ext" />
                                            <asp:ImageButton ID="imbtnReset" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="11" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext"
                                                CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <%-- use the grid to list the records in the page--%>
                                <asp:GridView ID="grdMailTemplate" runat="server" AutoGenerateColumns="False" Width="100%"
                                    OnPageIndexChanging="ActionHandler" PageSize="<%$ resources:PageSize%>" AllowSorting="false"
                                    OnSorting="ActionHandler" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton runat="server" GroupName="SelectOne" ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField ID="hdfTemplatePk" runat="server" Value='<%# Eval("TML_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TemplateName %>" SortExpression="TML_NAME2">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTemplateName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("TML_NAME2"),25) %>'
                                                    ToolTip='<%# Eval("TML_NAME2") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Subject %>" SortExpression="TML_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubject" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("TML_NAME"),30) %>'
                                                    ToolTip='<%# Eval("TML_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Message %>" SortExpression="TML_TEMPLATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMessage" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.RemoveHTML(Eval("TML_TEMPLATE")),41) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.RemoveHTML(Eval("TML_TEMPLATE")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="23%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:MailFom %>" SortExpression="TML_FROM">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMailFrom" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("TML_FROM"),20) %>'
                                                    ToolTip='<%# Eval("TML_FROM") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Cc %>" SortExpression="TML_TO_CC">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMailCc" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("TML_TO_CC"),20) %>'
                                                    ToolTip='<%# Eval("TML_TO_CC") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Bcc %>" SortExpression="TML_TO_BCC">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMailBcc" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("TML_TO_BCC"),20) %>'
                                                    ToolTip='<%# Eval("TML_TO_BCC") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                            <%-- Leave this table as such --%>
                            <div class="clear">
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" runat="server" CssClass="last-modified" Visible="true">
                        <asp:TableCell ColumnSpan="2">
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
            </div>
        </ContentTemplate>
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btnAttach" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
