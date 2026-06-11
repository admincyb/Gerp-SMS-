<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="JVTemplate.aspx.cs" Inherits="ERPSMS_v01.Journalize.JVTemplate" Theme="ClassicExt" %>

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
                $("[id$=JV_List]").show();
                $("[id$=pnlListing]").show();
            }
            else {
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlEntry]").show();
                $("[id$=JV_List]").hide();
                $("[id$=pnlListing]").hide();


            }
            return false;
        }

        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url, "hdfCustomer", true, true, "CUSTOMER");
            GrandScriptUtils.MakeAutoCompleteDDL("txtType", url, "hdfType", true, true, "MAILTYPE");
            if ($("[id$=txtAccount]").attr("disabled") == true) {
                DisableAuto($("[id$=txtAccount]"), $("[id$=hdfAccount]"));
            }
            var pageURL = window.document.URL;
            var virtualPath = '<%= (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString()) %>';
            var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
//            if (url.indexOf("?") != -1)
//                GrandScriptUtils.MakeAutoCompleteDDL("txtAccount", url + "&AccType=11", "hdfAccount", true, true, "ACCOUNTMST");
//            else
            //                GrandScriptUtils.MakeAutoCompleteDDL("txtAccount", url + "?AccType=11", "hdfAccount", true, true, "ACCOUNTMST");
            GrandScriptUtils.MakeAutoCompleteDDL("txtAccount", url + "?AccType=0", "hdfAccount", true, true, "ACCOUNTMST");

        }

        //        function AfterAutoCompleteSelect(targetControlID) {
        //            if (targetControlID == "txtCustomer") {
        //                $("[id$=txtCustomer]").attr('title', $("[id$=txtCustomer]").val());
        //            }
        //            if (targetControlID == "txtType") {
        //                $("[id$=txtType]").attr('title', $("[id$=txtType]").val());
        //            }
        //        }

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
                $("[id$=btnSave]").hide();
                $("[id$=btnNew]").hide();
                $("[id$=btnView]").hide();
                $("[id$=btnEdit]").hide();
                $("[id$=btnDelete]").hide();

            }
            else if (mode == 2) {
                $("[id$=btnNew]").show();
                $("[id$=btnView]").hide();
                $("[id$=btnEdit]").show();


            }
            else if (mode == 4) {
                $("[id$=btnNew]").hide();
                $("[id$=btnView]").hide();
                $("[id$=btnEdit]").hide();
                $("[id$=btnSave]").show();
                $("[id$=btnDelete]").show();

            }
            else if (mode == 3) {
                $("[id$=btnNew]").show();
                $("[id$=btnView]").show();
                $("[id$=btnEdit]").show();
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
            $("[id$=grdJVList]").find("tr:has(td)").each(function () {
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
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="20" Text="<%$resources:ErpRes,Save %>"
                                            OnClick="ActionHandler" ValidationGroup="Save" ToolTip="<%$resources:ErpRes,Save %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" OnClientClick="javascript:ValidatePageNow('Save')" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnCancel" CommandName="CANCEL" TabIndex="21" Text="<%$resources:ErpRes,Cancel %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Cancel %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" TabIndex="22" Text="<%$resources:ErpRes,Delete %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Delete %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Delete" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing">
                                    <asp:Button runat="server" ID="btnNew" SkinID="btnInner-New" CommandName="NEW" Text="<%$ Resources:ErpRes, New %>"
                                        TabIndex="23" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,New %>" CommandArgument="SEC_ActionPanel" />
                                    <asp:Button runat="server" ID="btnEdit" CommandName="EDIT" Text="<%$ Resources:ErpRes, Edit %>"
                                        TabIndex="24" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Edit %>" SkinID="btnInner-Edit"
                                        CommandArgument="SEC_ActionPanel" />
                                    <asp:Button runat="server" ID="btnView" CommandName="VIEW" Text="<%$ Resources:ErpRes, View %>"
                                        TabIndex="25" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,View %>" SkinID="btnInner-View"
                                        CommandArgument="SEC_ActionPanel" />
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
                            <table class="table-devide">
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblName" AssociatedControlID="txtName" Text="<%$ resources:Name %>"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtName" TabIndex="10" CssClass="input-full"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfName" CssClass="star" SetFocusOnError="true" ValidationGroup="Save"
                                                EnableClientScript="true" runat="server" ControlToValidate="txtName" Display="Dynamic"
                                                Text="*" ErrorMessage="<%$ resources:Err_Name %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:HiddenField ID="hdfVLDPK" runat="server" />
                                            <asp:HiddenField ID="hdfSeq" runat="server" />
                                            <asp:HiddenField ID="hdfRType" runat="server" />
                                            <asp:HiddenField ID="hdfRefPK" runat="server" />
                                            <asp:HiddenField ID="hdfSNO" runat="server" />
                                            <asp:Label ID="lblType" runat="server" Text="<%$resources:Type %>" AssociatedControlID="ddlType"></asp:Label>
                                            <asp:DropDownList ID="ddlType" runat="server" TabIndex="11" Enabled="true" CssClass="select-half-a">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfType" CssClass="star" SetFocusOnError="true" ValidationGroup="Save"
                                                EnableClientScript="true" runat="server" ControlToValidate="ddlType" Display="Dynamic"
                                                InitialValue="-1" Text="*" ErrorMessage="<%$ resources:Err_Type %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>   
                                            <asp:Label runat="server" ID="lblAccount" Text="<%$ resources:Controls,Account%>"
                                                AssociatedControlID="txtAccount"></asp:Label>
                                            <asp:TextBox ID="txtAccount" TabIndex="14" runat="server" MaxLength="100" CssClass="input-half" />
                                            <asp:HiddenField ID="hdfAccount" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfAccount" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="AddTemplates" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                runat="server" ControlToValidate="txtAccount" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Account %>"></asp:RequiredFieldValidator>
                                            <asp:ImageButton ID="imbAdd" TabIndex="15" SkinID="plus" runat="server" OnClick="ActionHandler"
                                                OnClientClick="javascript:ValidatePageNow('AddTemplates')" ToolTip="<%$ resources:Add %>"
                                                CommandName="ADD" ValidationGroup="AddTemplates" />                                       
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblActive" AssociatedControlID="chkActive" Text="<%$ resources:Active %>"></asp:Label>
                                            <asp:CheckBox runat="server" ID="chkActive" Checked="true" TabIndex="12" />

                                            <asp:Label runat="server" ID="lblMode" Text="<%$ resources:Mode%>" CssClass="lbl-39-0perc" AssociatedControlID="ddlMode"></asp:Label>
                                            <asp:DropDownList ID="ddlMode" runat="server" CssClass="select-small-a0" TabIndex="13">
                                                <asp:ListItem Text="<%$resources:Cr %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$resources:Dr %>" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>                                            
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdJvItems" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                    AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" AutoPostBack="true">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Type %>" SortExpression="VLD_MODE">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfSlNo" runat="server" Value='<%# Eval("SLNO") %>' />
                                                <asp:HiddenField ID="hdfDetailsPK" runat="server" Value='<%# Eval("VLD_PK") %> ' />
                                                <asp:HiddenField ID="hdfRefType" runat="server" Value='<%# Eval("VLD_REF_TYPE") %> ' />
                                                <asp:HiddenField ID="hdfRefTypePK" runat="server" Value='<%# Eval("VLD_REF_TYPE_PK") %> ' />
                                                <asp:HiddenField ID="hdfSequence" runat="server" Value='<%# Eval("VLD_SEQUENCE") %> ' />
                                                <asp:HiddenField ID="hdfMode" runat="server" Value='<%# Eval("VLD_MODE") %> ' />
                                                <asp:Label ID="lblVMode" runat="server" Text='<%#  Eval("VLD_MODE").ToString()=="1"?"Cr":"Dr" %>'
                                                    ToolTip='<%#  Eval("VLD_MODE").ToString()=="1"?"Cr":"Dr" %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AccountCode %>" SortExpression="COA_CODE">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfGAccount" runat="server" Value='<%# Eval("VLD_ACCOUNT") %> ' />
                                                <asp:Label ID="lblVAccountCode" runat="server" Text='<%# Eval("COA_CODE") %> ' ToolTip='<%# Eval("COA_CODE")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="38%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AccountName %>" SortExpression="COA_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVAccountName" runat="server" Text='<%# Eval("COA_NAME") %>' ToolTip='<%# Eval("COA_NAME")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnGUP" runat="server" OnClick="ActionHandler" CommandName="UPGRID"
                                                    SkinID="up" ToolTip="MOVE UP" TabIndex="16" />
                                                <asp:ImageButton ID="btnGDown" runat="server" OnClick="ActionHandler" CommandName="DOWNGRID"
                                                    SkinID="down" ToolTip="MOVE DOWN" TabIndex="17" />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnGEdit" runat="server" OnClick="ActionHandler" CommandName="EDITGRID"
                                                    SkinID="imbeditgrid" ToolTip="Edit" TabIndex="18" />
                                                <asp:ImageButton ID="btnGDelete" runat="server" OnClick="ActionHandler" CommandName="DELETEGRID"
                                                    SkinID="imbdeletegrid" ToolTip="Delete" TabIndex="19" />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <%--Rename this ID Page_List with the corresponding section Id in the documet--%>
                <asp:Table runat="server" ID="JV_List" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
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
                            <%--------------colpase btn----------%>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="margin-top: 8px;">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblSName" runat="server" Text="<%$resources:Name %>" AssociatedControlID="txtTName"></asp:Label>
                                            <asp:TextBox ID="txtTName" runat="server" TabIndex="1" CssClass="input-w30per"></asp:TextBox>

                                             <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" CssClass="middle-lbl" AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-a" TabIndex="2">
                                                <asp:ListItem Text="<%$ resources:All %>" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ resources:Active %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ resources:Inactive %>" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>                                           
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">                                           
                                             <asp:Label ID="lblSCategory" runat="server" CssClass="middle-lbl" Text="<%$resources:Type %>" AssociatedControlID="ddlSType"></asp:Label>
                                            <asp:DropDownList ID="ddlSType" CssClass="input-w65-1per" runat="server" TabIndex="3">
                                            </asp:DropDownList>                                            
                                            <asp:ImageButton ID="btnSearch" runat="server" SkinID="search-ext"
                                                ToolTip="<%$ resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="3"
                                                CommandName="SEARCH"  />
                                            <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="4" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdJVList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                    AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" AutoPostBack="true"
                                    OnCheckedChanged="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" GroupName="SelectOne"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" TabIndex="5" />
                                                <asp:HiddenField runat="server" ID="hdfTemplatePK" Value='<%# Eval("VLH_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Name %>" SortExpression="VLH_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTName" runat="server" Text='<%# System.Web.HttpUtility.HtmlDecode(Eval("VLH_NAME").ToString()) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Eval("VLH_NAME").ToString()),60)%>'></asp:Label>
                                                <%--<asp:Label ID="lblTName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetEncodedString(Eval("VLH_NAME")) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("VLH_NAME")),60)%>'></asp:Label>--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="40%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Type %>" SortExpression="VLH_TYPE_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblJournalizeType" runat="server" Text='<%# Eval("VLH_TYPE_TEXT") %>'
                                                    ToolTip='<%# Eval("VLH_TYPE_TEXT")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="52%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Active %>" SortExpression="VLH_ACTIVE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTActive" runat="server" Text='<%# Eval("VLH_ACTIVE").ToString()=="1"?"Active":"InActive" %> '
                                                    ToolTip='<%# Eval("VLH_ACTIVE").ToString()=="1"?"Active":"InActive"%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <asp:TableRow ID="ModifiedDatePnl" runat="server" CssClass="last-modified" Visible="true">
                <asp:TableCell>
                    <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
                <asp:ValidationSummary ID="vsAddTemplates" ValidationGroup="AddTemplates" runat="server" />
            </div>
        </ContentTemplate>
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btnAttach" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
