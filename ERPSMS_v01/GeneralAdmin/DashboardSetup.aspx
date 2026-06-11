<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DashboardSetup.aspx.cs"
    Inherits="ERPSMS_v01.GeneralAdmin.DashboardSetup" Theme="ClassicExt" MasterPageFile="~/ERPSMS_2.Master"
    Title="<%$ Resources:Captions,Title_DashboardSetup %>" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function InitComponents() {
            ShowHideGroupDetails(1);
            $("[id*=txtSeqHd]").ForceNumersOnly();
            //  $("[id*=txtSeqGd]").ForceNumericOnly();
            HideRowUpDownIcon();
        }
        function ShowListing(flag) {
            if (flag) {
                $("[id$='PageAction_List']").show();
                $("[id$='PageAction_Entry']").hide();
                $("[id$='pnlListing']").show();
                $("[id$='pnlEntry']").hide();
            }
            else {
                $("[id$='PageAction_List']").hide();
                $("[id$='PageAction_Entry']").show();
                $("[id$='pnlListing']").hide();
                $("[id$='pnlEntry']").show();
            }
            return false;
        }
        function ShowHideGroupDetails(flag) {
            if (flag == 1) {
                $("[id$=divGroupDetails").show();
                $("[id$=imbShowGroupDetails").hide();
                $("[id$=imbHideGroupDetails").show();
            }
            else {
                $("[id$=divGroupDetails").hide();
                $("[id$=imbShowGroupDetails").show();
                $("[id$=imbHideGroupDetails").hide();
            }
            return false;
        }
        function ViewMode(mode) {
            ///<summary>
            /// Used to handle the view Mode
            ///</summary>
            /// <param name="mode" optional="true" type="String">
            /// Mode = 1 Determins ites on View Mode
            /// Mode = 2 Indicates its on New Mode
            /// </param>         
            if (mode == 1) {
                $("[id$='pnlSave']").hide();
            }
            else if (mode == 2) {
            }
        }
        function ShowHideAdvancedSearch(flag) {
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

        function ShowHideExpand() {
            ///<summary>
            /// Used to Show/Hide Grid Expad Button
            ///</summary>

            $("[id*=hdfHasChildren]").each(function () {
                $(this).parent().parent().find('a.GridExpandCollapseButton').css("visibility", ($(this).val() == "1" ? "visible" : "hidden"));
            });
        }

        function AfterGridExpand(row) {
            if ($("[id$=grdDashletPopupList]").attr('id') == $(row).parent().parent().attr('id')) {
                var hdf = $(row).find("[id*=hdfIsExpandedPOItem]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnGetGrn]").click();
                }
            }
        }

        function ValidateNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                //CheckValidationDuplicate(valGroup);
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
                return true;
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
                        //Page_Validators.splice(i, 1);
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

        $("[id*=chkMenu_popUp]").live("click", function () {
            var grid = $(this).closest("table");
            if (!$(this).is(":checked")) {
                $("td", $(this).closest("tr")).find("input:text[id*=txtUserMenuNamePopUp]").val('');
            } else {
                $("td", $(this).closest("tr")).find("input:text[id*=txtUserMenuNamePopUp]").val($("td", $(this).closest("tr")).find("span[id*=lblMenuNamePopUp]").text());

            }
        });

        $("[id*=chkDashlet_popUp]").live("click", function () {
            var grid = $(this).closest("table");
            if (!$(this).is(":checked")) {
                $("td", $(this).closest("tr")).find("input:text[id*=txtDashUserMenuNamePopUp]").val('');
            } else {
                $("td", $(this).closest("tr")).find("input:text[id*=txtDashUserMenuNamePopUp]").val($("td", $(this).closest("tr")).find("span[id*=lblDashNamePopUp]").text());

            }
        });

        $("[id*=chkRpt_popUp]").live("click", function () {
            var grid = $(this).closest("table");
            if (!$(this).is(":checked")) {
                $("td", $(this).closest("tr")).find("input:text[id*=txtUserRptNamePopUp]").val('');
            } else {
                $("td", $(this).closest("tr")).find("input:text[id*=txtUserRptNamePopUp]").val($("td", $(this).closest("tr")).find("span[id*=lblRptNamePopUp]").text());

            }
        });

        //Sequence START
        function SendUp(CurrentRow, PreviousRow) {
            PreviousRow.parentNode.insertBefore(CurrentRow, PreviousRow);
        }
        function SequenceSwap(CurrentRow, PreviousRow) {
            var curId = CurrentRow.cells[1].children[0].id;
            var preId = PreviousRow.cells[1].children[0].id;
            var curSeq = $("[id$=" + curId + "]").val();
            var preSeq = $("[id$=" + preId + "]").val();
            curSeq = parseInt(curSeq);
            preSeq = parseInt(preSeq);
            curSeq = (isNaN(curSeq) || curSeq == "undefined") ? 1 : curSeq;
            preSeq = (isNaN(preSeq) || preSeq == "undefined") ? curSeq - 1 : preSeq;
            $("[id$=" + curId + "]").val(preSeq);
            $("[id$=" + preId + "]").val(curSeq);
            ShowRowUpDownIcon();
        }
        //function to select a row from Right Table to move up
        function MoveUp(obj) {
            ShowRowUpDownIcon();
            var index = obj.parentElement.parentElement.rowIndex;
            var RightTable = obj.parentElement.parentElement.parentElement;
            var i = 0;
            for (i = 2; i < RightTable.rows.length; i++) {
                if (i == index) //is selected ?
                {
                    SendUp(RightTable.rows[i], RightTable.rows[i - 1]);
                    SequenceSwap(RightTable.rows[i], RightTable.rows[i - 1]);
                }
            }
            HideRowUpDownIcon();

        }
        //function to select a row to move down
        function MoveDown(obj) {
            ShowRowUpDownIcon();
            var index = obj.parentElement.parentElement.rowIndex;
            var RightTable = obj.parentElement.parentElement.parentElement;
            var i = 0;
            var RowToMove = 0;
            var PreviousRow;
            var CurrentRow;
            for (i = 1; i < RightTable.rows.length - 1; i++) {
                if (i == index) {
                    RightTable.rows[i];
                    RowToMove = i;
                    SequenceSwap(RightTable.rows[i], RightTable.rows[i + 1]);
                    RightTable.rows[i].parentNode.appendChild(RightTable.rows[i]); //appends the selected row to the end of the Right Table
                    //this code moves the appended row up till it reaches
                    //to one position less than its original position
                    for (i = RightTable.rows.length - 1; i > RowToMove + 1; i--) {
                        CurrentRow = RightTable.rows[i];
                        PreviousRow = RightTable.rows[i - 1];
                        SendUp(CurrentRow, PreviousRow);
                    }
                }
            }
            HideRowUpDownIcon();
        }
        function ShowRowUpDownIcon() {
            //For Hide Move Up/Down arrow 
            $('[id*=grdGroupDetails] tr:nth-child(2)').find("[id*=imbRuleUp]").show(); //Show first rows Up arrow
            $('[id*=grdGroupDetails] tr:last-child').find("[id*=imbRuleDown]").show(); //Show last rows Down Arrow
        }
        function HideRowUpDownIcon() {
            //For Hide Move Up/Down arrow 
            $('[id*=grdGroupDetails] tr:nth-child(2)').find("[id*=imbRuleUp]").hide(); //Hide first rows Up arrow
            $('[id*=grdGroupDetails] tr:last-child').find("[id*=imbRuleDown]").hide(); //Hide last rows Down Arrow
        }
        //Sequence END

        function CheckOne(obj) {
            $("#[id*=grdDashletPopUp] input[type=checkbox][id*=chkDashlet_popUp]").each(function (index) {
                //alert($(this).is(':checked'));
                if (obj != $(this)) {
                    $(this).closest('tr').find('[id*=txtDashUserMenuNamePopUp]').val('');
                    //alert($(this).is(':checked')); // = false;
                }
            });
            var grid = obj.parentNode.parentNode.parentNode;
            var inputs = grid.getElementsByTagName("input");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    if (obj.checked && inputs[i] != obj && inputs[i].checked) {
                        inputs[i].checked = false;
                    }
                }
            }
        }

        function validate(source, arguments) {
            if ($("[id$=txtDescriptionGd]").val().length > 0 || $("[id$=txtNameGd]").val().length > 0) {
                arguments.IsValid = true;
            }
            else {
                arguments.IsValid = false;
            }
        }


        function showPreview() {
            var dashBoardURL = '<%=(System.Configuration.ConfigurationManager.AppSettings["DASHBOARDURL"].ToString())%>';
            window.open(dashBoardURL, "_blank");
        }

        function IsRadioButtonChecked() {
            var Retval = 0;
            $("#[id*=grdList] input[type=hidden][id*=hdfINDPK_List]").each(function (index) {
                if ($(this).closest('tr').find("#[id*=rbtSelect]").attr("checked")) {
                    Retval = 1;
                }
            });
            if (Retval == 1) {
                $("[id$=btnConfirmDelete]").click();
            }
            else {
                ShowErrorMessage('<ul><li>' + '<%= GetLocalResourceObject("Msg_Select_Record").ToString() %>' + '</ul>');
            }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlBasicInfo">
        <contenttemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum" Text="<%$ resources:Breadcrumb%>"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$resources:Controls,Save %>"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save"
                                            ValidationGroup="Save" OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('Save')"
                                            TabIndex="80" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            CssClass="popupclose" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" OnClick="ActionHandler"
                                            TabIndex="80" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="1" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="1" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="1" ID="btnConfirm" Text="<%$resources:Controls,Delete %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" OnClientClick="return IsRadioButtonChecked();"
                                            ToolTip="<%$ resources:Controls,Delete %>" />
                                    </li>
                                    <li style="display: none;">
                                        <asp:Button ID="btnConfirmDelete" runat="server" CommandName="DELETE" OnClick="ActionHandler"
                                            OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="1" ID="btnPreview" OnClientClick="showPreview()"
                                            Text="<%$resources:Controls,Preview %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,Preview %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <div class="tab-container-floating">
                    <%--Container for List and Detail tabs--%>
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="1" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="1" OnClick="ActionHandler" CommandName="DETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblPage" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-colapse" id="divAdvanceSearch" style="margin-top: 0px;">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:ShowFilter%>"
                                                TabIndex="2" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>"
                                                TabIndex="2" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7 ">
                                            <asp:Label ID="lblNameFilterList" runat="server" Text="<%$ resources:Name%>" AssociatedControlID="txtNameFilterList"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtNameFilterList" TabIndex="2" CssClass="input-small-d margnbotm0"></asp:TextBox>
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </td>
                                    <td> 
                                    <div class="div2col-S padgtop7 ">
                                     <asp:Label ID="lblStatus" runat="server" Text="<%$ resources:Controls,Status%>" AssociatedControlID="ddlFilterStatus"
                                     TabIndex="2"></asp:Label>
                                     <asp:DropDownList runat="server" ID="ddlFilterStatus" CssClass="select-small">
                                     <asp:ListItem Text="<%$ resources:Captions, All %>" Value="-1"></asp:ListItem>
                                     <asp:ListItem Text="<%$ resources:Captions, Active %>" Value="1"></asp:ListItem>
                                     <asp:ListItem Text="<%$ resources:Captions, Inactive %>" Value="0"></asp:ListItem>
                                     </asp:DropDownList>
                                       <asp:ImageButton ID="btnSearchList" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="3"
                                                CommandName="FILTER" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClearList" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="3" OnClick="ActionHandler"
                                                CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnlft-minus2 margnbotm0" />
                                    </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="grdTable">
                                <asp:GridView runat="server" ID="grdList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" OnPageIndexChanging="ActionHandler" CssClass="grdTable"
                                    OnSorting="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    TabIndex="4" />
                                                <asp:HiddenField runat="server" ID="hdfINDPK_List" Value='<%# Eval("IND_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfModDate_List" Value='<%# Eval("IND_MOD_DT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                            <HeaderStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Name%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvNameList" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("IND_NAME")),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("IND_NAME")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Description%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvDescList" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("IND_DESC")),60) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("IND_DESC")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Mode%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvModeList" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("IND_MODE_TEXT")),18) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("IND_MODE_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Type%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvTypeList" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("IND_TYPE_TEXT")),18) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("IND_TYPE_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Theme%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvThemeList" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("IND_CLASS_TEXT")),18) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("IND_CLASS_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="8%" />
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Sequence%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvSequenceList" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("IND_SEQUENCE")),15) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("IND_SEQUENCE")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Active%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvActiveList" runat="server" Text='<%# Eval("IND_ACTIVE")!=null?Eval("IND_ACTIVE").ToString()==ERP.Utilities.CommonConstants.SELECT_VALUE_ONE?GetLocalResourceObject("Active").ToString(): GetLocalResourceObject("Inactive").ToString() :string.Empty %>'
                                                    ToolTip='<%# Eval("IND_ACTIVE")!=null?Eval("IND_ACTIVE").ToString()==ERP.Utilities.CommonConstants.SELECT_VALUE_ONE?GetLocalResourceObject("Active").ToString(): GetLocalResourceObject("Inactive").ToString() :string.Empty %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="6%" />
                                            <ItemStyle Width="6%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" TabIndex="4" />
                                <div class="clear">
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblModeHd" runat="server" Text="<%$ resources:ModeStar%>" AssociatedControlID="ddlModeHd"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlModeHd" TabIndex="5" CssClass="select-w22-6per"
                                                OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvModeHD" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlModeHd" Display="Static" Text="*" InitialValue="-1"
                                                ValidationGroup="Save" ErrorMessage="<%$ resources:Err_Mode %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvModeHDAdd" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlModeHd" Display="Static" Text="*" InitialValue="-1"
                                                ValidationGroup="AddToList" ErrorMessage="<%$ resources:Err_Mode %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label ID="lblTypeHd" runat="server" Text="<%$ resources:TypeStar%>" AssociatedControlID="ddlTypeHd"
                                                CssClass="lbl-11-3perc"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlTypeHd" TabIndex="5" CssClass="select-w22-6per">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvTypeHd" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlTypeHd" Display="Static" Text="*" InitialValue="-1"
                                                ValidationGroup="Save" ErrorMessage="<%$ resources:Err_Type %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblThemeHd" runat="server" Text="<%$ resources:ThemeStar%>" CssClass="lbl-14-9perc"
                                                AssociatedControlID="ddlThemeHd"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlThemeHd" TabIndex="5" CssClass="select-w22-6per">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvThemeHd" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlThemeHd" Display="Static" Text="*" InitialValue="-1"
                                                ValidationGroup="Save" ErrorMessage="<%$ resources:Err_Theme %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label ID="lblSeqHd" runat="server" Text="<%$ resources:SequenceStar%>" AssociatedControlID="txtSeqHd"
                                                CssClass="middle-lbl-xsmall-a2"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSeqHd" TabIndex="5" CssClass="input-xsmall" MaxLength="2"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvSeqHd" runat="server" ControlToValidate="txtSeqHd"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_EnterSequence%>"></asp:RequiredFieldValidator>
                                            <asp:Label ID="lblActiveHd" runat="server" Text="<%$ resources:Active%>" AssociatedControlID="chkActiveHd"
                                                CssClass="lbl-6perc"></asp:Label>
                                            <asp:CheckBox ID="chkActiveHd" runat="server" Checked="true" TabIndex="5" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblNameHd" runat="server" Text="<%$ resources:NameStar%>" AssociatedControlID="txtNameHd"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtNameHd" TabIndex="5" MaxLength="90" CssClass="input-half"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvNameHd" runat="server" ControlToValidate="txtNameHd"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_EnterName%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblDescriptionHd" runat="server" Text="<%$ resources:Description%>"
                                                AssociatedControlID="txtDescriptionHd" CssClass="lbl-14-9perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDescriptionHd" MaxLength="500" TabIndex="5" CssClass="input-half"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    <asp:Literal ID="ltrGroupDetails" runat="server" Text="<%$ resources: GroupDetails %>" /></h1>
                                <asp:ImageButton runat="server" ID="imbShowGroupDetails" OnClientClick="javascript:return ShowHideGroupDetails(1);"
                                    SkinID="imbArrowShow" ToolTip="<%$ resources:Controls,ShowDetails%>" />
                                <asp:ImageButton runat="server" ID="imbHideGroupDetails" OnClientClick="javascript:return ShowHideGroupDetails();"
                                    Style="display: none" SkinID="imbArrowHide" ToolTip="<%$ resources:Controls,HideDetails%>" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divGroupDetails">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblNameGd" runat="server" Text="<%$ resources:Name%>" AssociatedControlID="txtNameGd"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtNameGd" TabIndex="6" CssClass="input-half" MaxLength="100"></asp:TextBox>
                                                <asp:CustomValidator ID="csvNameGd" ControlToValidate="txtNameGd" Display="None"
                                                    ValidateEmptyText="true" ValidationGroup="AddToList" ClientValidationFunction="validate"
                                                    Text=" " runat="server" ErrorMessage="<%$resources:Err_EnterNameDesc %>"></asp:CustomValidator>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblDescriptionGd" runat="server" Text="<%$ resources:Description%>"
                                                    AssociatedControlID="txtDescriptionGd" CssClass="lbl-14-9perc"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDescriptionGd" TabIndex="6" CssClass="input-w48-7per"
                                                    MaxLength="500"></asp:TextBox>
                                                <%--<asp:CustomValidator ID="csvDescriptionGd" ControlToValidate="txtDescriptionGd" ValidateEmptyText="true"
                                                    ValidationGroup="AddToList" ClientValidationFunction="validate" Text="*" runat="server"
                                                    ErrorMessage="<%$resources:Err_EnterNameDesc %>"></asp:CustomValidator>--%>
                                                <asp:Label ID="lblActiveGd" runat="server" Text="<%$ resources:Active%>" CssClass="middle-lbl-xsmall-f"
                                                    AssociatedControlID="chkActiveGd"></asp:Label>
                                                <asp:CheckBox ID="chkActiveGd" runat="server" Checked="true" TabIndex="6" />
                                                <asp:ImageButton ID="imgAdd" runat="server" CommandName="ADDTOLIST" SkinID="imbaddnew"
                                                    ToolTip="<%$ resources:Controls,AddToList %>" OnClick="ActionHandler" TabIndex="6"
                                                    ValidationGroup="AddToList" OnClientClick="javascript:ValidateNow('AddToList')"
                                                    CssClass="margnbotm0" />
                                                <asp:ImageButton ID="imgbtnMappingPopup" runat="server" CommandName="SHOWDASHLETMAPPING"
                                                    SkinID="imbaddnew" Visible="false" ToolTip="<%$ resources:Controls,AddToList %>"
                                                    OnClick="ActionHandler" TabIndex="6" CssClass="margnbotm0" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <%-- <asp:Label ID="lblSeqGd" runat="server" Text="<%$ resources:SequenceStar%>" AssociatedControlID="txtSeqGd"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtSeqGd" TabIndex="6" CssClass="input-xsmall"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvSeqGd" runat="server" ControlToValidate="txtSeqGd"
                                                    CssClass="star" ValidationGroup="AddToList" Text="*" ErrorMessage="<%$ resources:Msg_SelectSequence%>"></asp:RequiredFieldValidator>--%>
                                            </div>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="Div1" runat="server" class="gridwrap">
                                <asp:HiddenField ID="hdfgvdDtailsSiNo" Value="0" runat="server" />
                                <asp:HiddenField ID="hdfgrdDtailsCurSeqNo" Value="0" runat="server" />
                                <asp:GridView ID="grdGroupDetails" runat="server" AutoGenerateColumns="False" Width="100%"
                                    EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false" ShowFooter="false"
                                    OnRowCommand="ActionHandler" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton CssClass="nomargin" ID="imbRuleUp" SkinID="move-up" runat="server"
                                                    OnClientClick="MoveUp(this);return false;" CommandName="MOVEUP" ToolTip="Move Up"
                                                    CommandArgument='<%# Eval("DBD_SEQUENCE") %>' TabIndex="1"></asp:ImageButton>
                                                <asp:ImageButton CssClass="nomargin" ID="imbRuleDown" SkinID="move-down" runat="server"
                                                    OnClientClick="MoveDown(this);return false;" CommandName="MOVEDOWN" ToolTip="Move Down"
                                                    CommandArgument='<%# Eval("DBD_SEQUENCE") %>' TabIndex="1"></asp:ImageButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Name %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfSequence" runat="server" Value='<%# Eval("DBD_SEQUENCE") %>' />
                                                <asp:Label ID="lblGvNameGd" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("DBD_NAME"),48) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("DBD_NAME")))%>'></asp:Label>
                                                <asp:HiddenField ID="hdfDBD_PK" Value='<%# Eval("DBD_PK") %>' runat="server" />
                                                <asp:HiddenField ID="hdfDBD_SLNO" Value='<%# Eval("DBD_SL_NO") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="32%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Description %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvDescGd" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("DBD_DESC"),60) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("DBD_DESC")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="38%" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:Sequence %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvSequenceGd" runat="server" Text='<%#Eval("DBD_SEQUENCE") %>'
                                                    ToolTip='<%# Eval("DBD_SEQUENCE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Active %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvActiveGd" runat="server" Text='<%# Eval("DBD_ACTIVE")!=null?Eval("DBD_ACTIVE").ToString()==ERP.Utilities.CommonConstants.SELECT_VALUE_ONE?GetLocalResourceObject("Active").ToString(): GetLocalResourceObject("Inactive").ToString() :string.Empty %>'
                                                    ToolTip='<%# Eval("DBD_ACTIVE")!=null?Eval("DBD_ACTIVE").ToString()==ERP.Utilities.CommonConstants.SELECT_VALUE_ONE?GetLocalResourceObject("Active").ToString(): GetLocalResourceObject("Inactive").ToString() :string.Empty %>'></asp:Label>
                                                <asp:HiddenField ID="hdfGvActiveGd" Value='<%# Eval("DBD_ACTIVE") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imbEditDetails" SkinID="imbeditgrid" EnableViewState="false"
                                                    CssClass="_edit" CommandName="EDIT_ACTION" ToolTip="<%$resources:Controls,Edit %>"
                                                    Width="16px" Height="16px" TabIndex="7" />
                                                <asp:ImageButton runat="server" ID="imbEditItemDetails" SkinID="user-map" CommandName="MENUMAPPING"
                                                    ToolTip="<%$ resources:EditMenuMapping%>" OnClick="ActionHandler" Width="16px"
                                                    Height="16px" abIndex="7" Visible="false" />
                                                <asp:ImageButton runat="server" ID="imbEditDetailsDashlet" SkinID="user-map" CommandName="DASHLETMAPPING"
                                                    ToolTip="<%$ resources:DashletMapping%>" OnClick="ActionHandler" Width="16px"
                                                    Height="16px" abIndex="7" Visible="false" />
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbDeleteRejectedDetails"
                                                    SkinID="imbdeletegrid" EnableViewState="false" CommandName="DELETE_ACTION" OnClientClick="return ShowDeleteConfirm(this);"
                                                    ToolTip="<%$resources:Controls,Delete %>" TabIndex="7" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" Wrap="false" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
            </div>
            <%--MENU & REPORT SECTION--%>
            <div id="divPopUpGroupItemDetails" style="display: none">
                <div class="content-wrapper">
                    <div class="Button-container-popup">
                        <asp:Button ID="btnAddMenu" SkinID="btnInner-add-dsd" runat="server" Text="<%$resources:Controls,Add_Add %>"
                            CommandName="SAVE_ACTIONPOPUP" OnClick="ActionHandler" TabIndex="9" />
                        <asp:Button ID="btnCancelPopUp" runat="server" CommandName="CANCELPOPUP" OnClick="ActionHandler"
                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Close %>" TabIndex="10"
                            Text="<%$Resources:Controls,Close%>" />
                    </div>
                    <div class="detail-poi-co1">
                        <div class="popup-headr">
                            <asp:Label ID="lblDashHdPopup" runat="server" Text="" Font-Bold="True" CssClass="minw-60per"></asp:Label><asp:Label
                                ID="lblGroupHdPopup" runat="server" Text="" Font-Bold="True" CssClass="minw-50per inline"></asp:Label></div>
                    </div>
                    <div class="tab-container-floating">
                        <%--Container for List and Detail tabs--%>
                        <ul>
                            <li>
                                <asp:LinkButton runat="server" ID="lnkMenu" Text="<%$resources:Menu %>" CommandArgument="SEC_ActionPanel"
                                    TabIndex="1" OnClick="ActionHandler" CommandName="SHOWMENUDETAILS" CssClass="tab-active"></asp:LinkButton>
                            </li>
                            <li>
                                <asp:LinkButton runat="server" ID="lnkMISReport" Text="<%$resources:MISReport %>"
                                    CommandArgument="SEC_ActionPanel" TabIndex="1" OnClick="ActionHandler" CommandName="SHOWREPORTDETAILS"
                                    CssClass="tab-inactive"></asp:LinkButton>
                            </li>
                        </ul>
                    </div>
                    <div id="divMenuPopupDetails" runat="server">
                        <table class="table-devide">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblDeptPopUp" runat="server" Text="<%$ resources:Department%>" AssociatedControlID="ddlDeptPopUp"></asp:Label><asp:DropDownList
                                            runat="server" ID="ddlDeptPopUp" TabIndex="8" CssClass="select-half-d" AutoPostBack="true"
                                            OnSelectedIndexChanged="ActionHandler">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                        <div id="Div2" runat="server" class="gridwrap maxh-225">
                            <asp:HiddenField ID="hdfCurDBDPK_PopUp" Value="0" runat="server" />
                            <asp:HiddenField ID="hdfCurGroupSlNo_PopUp" Value="0" runat="server" />
                            <asp:GridView ID="grdMenuPopUp" runat="server" AutoGenerateColumns="False" Width="100%"
                                EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false" ShowFooter="false"
                                OnRowCommand="ActionHandler">
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label></EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField SortExpression="">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkMenu_popUp" runat="server" TabIndex="9" />
                                            <asp:HiddenField ID="hdfMNUPK_PopUp" Value='<%# Eval("MNU_PK") %>' runat="server" />
                                            <asp:HiddenField ID="hdfMNUACTION_URL_PopUp" Value='<%# Eval("MNU_ACTION_URL") %>'
                                                runat="server" />
                                            <%--<asp:HiddenField ID="hdfDashACTION_URL_PopUp" Value='<%# Eval("MNU_ACTION_URL") %>'
                                            runat="server" />--%>
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:MenuName %>" SortExpression="">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMenuNamePopUp" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("MNU_NAME"),60) %>'
                                                ToolTip='<%# Eval("MNU_NAME") %>'></asp:Label></ItemTemplate>
                                        <ItemStyle Width="45%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:DisplayName %>" SortExpression="">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtUserMenuNamePopUp" runat="server" CssClass="input-wfull-b" MaxLength="100"></asp:TextBox></ItemTemplate>
                                        <ItemStyle Width="45%" />
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="<%$ resources:Sequence %>" SortExpression="">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtSequencePopUp" runat="server" CssClass="input-wfull-b"></asp:TextBox></ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div id="divReportPopupDetails" runat="server">
                        <table class="table-devide">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblMISRpt" runat="server" Text="<%$ resources:MIS%>" AssociatedControlID="ddlMISRptPopup"></asp:Label>
                                        <asp:DropDownList runat="server" ID="ddlMISRptPopup" TabIndex="8" CssClass="select-half-d"
                                            AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                        <div id="Div4" runat="server" class="gridwrap maxh-225">
                            <asp:HiddenField ID="hdfCurRDBDPK_PopUp" Value="0" runat="server" />
                            <asp:HiddenField ID="hdfCurRGroupSlNo_PopUp" Value="0" runat="server" />
                            <asp:GridView ID="grdReportPopup" runat="server" AutoGenerateColumns="False" Width="100%"
                                EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false" ShowFooter="false"
                                OnRowCommand="ActionHandler">
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label></EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField SortExpression="">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkRpt_popUp" runat="server" TabIndex="9" />
                                            <asp:HiddenField ID="hdfRptPK_PopUp" Value='<%# Eval("RPT_PK") %>' runat="server" />
                                            <asp:HiddenField ID="hdfRptACTION_URL_PopUp" runat="server" />
                                            <%--Value='<%# Eval("MNU_ACTION_URL") %>'--%>
                                            <%--<asp:HiddenField ID="hdfDashACTION_URL_PopUp" Value='<%# Eval("MNU_ACTION_URL") %>'
                                            runat="server" />--%>
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:ReportName %>" SortExpression="">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRptNamePopUp" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("RPT_NAME"),60) %>'
                                                ToolTip='<%# Eval("RPT_NAME") %>'></asp:Label></ItemTemplate>
                                        <ItemStyle Width="45%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:DisplayName %>" SortExpression="">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtUserRptNamePopUp" runat="server" CssClass="input-wfull-b" MaxLength="100"></asp:TextBox></ItemTemplate>
                                        <ItemStyle Width="45%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div id="divGrdMenuPopup" runat="server" class="gridwrap maxh-145">
                        <asp:GridView ID="grdMenuPopUpList" runat="server" AutoGenerateColumns="False" Width="100%"
                            EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false" ShowFooter="false"
                            OnRowCommand="ActionHandler">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label></EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton CssClass="nomargin" ID="imbRuleUpPop" SkinID="move-up" runat="server"
                                            OnClick="ActionHandler" CommandName="CHANGESEQUENCE" ToolTip="Move Up" CommandArgument='<%# Eval("DBL_SEQUENCE") %>'
                                            TabIndex="15"></asp:ImageButton>
                                        <asp:ImageButton CssClass="nomargin" ID="imbRuleDownPop" SkinID="move-down" runat="server"
                                            OnClick="ActionHandler" CommandName="CHANGESEQUENCE" ToolTip="Move Down" CommandArgument='<%# Eval("DBL_SEQUENCE") %>'
                                            TabIndex="15"></asp:ImageButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:DashboardMenuName %>" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMenuNamePopUpList" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("DBL_NAME"),40) %>'
                                            ToolTip='<%# Eval("DBL_NAME") %>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdfMNUPK_PopUpList" Value='<%# Eval("DBL_MENU_CFG") %>' />
                                        <asp:HiddenField runat="server" ID="hdfMNUACTION_URL_PopUpList" Value='<%# Eval("DBL_LINK") %>' />
                                        <asp:HiddenField runat="server" ID="hdfDBL_SLNO_PopUpList" Value='<%# Eval("DBL_SL_NO") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="50%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Department %>" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserMenuNamePopUpList" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("DBL_DPT_TEXT"),20) %>'
                                            runat="server" CssClass="lbl-75-6perc" ToolTip='<%# Eval("DBL_DPT_TEXT") %>'></asp:Label></ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbDeleteMenuPopList"
                                            SkinID="imbdeletegrid" EnableViewState="false" CommandName="DELETE_ACTION" OnClientClick="return ShowDeleteConfirm(this);"
                                            ToolTip="<%$resources:Controls,Delete %>" TabIndex="12" />
                                    </ItemTemplate>
                                    <ItemStyle Width="3%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <%--DASHLET SECTION--%>
            <div id="divPopupItemDashletDetails" style="display: none">
                <div class="content-wrapper">
                    <div class="Button-container-popup">
                        <asp:Button ID="btnAddDashlet" SkinID="btnInner-add-dsd" runat="server" Text="<%$resources:Controls,Add_Add %>"
                            CommandName="SAVE_ACTIONDASHLET" OnClick="ActionHandler" TabIndex="9" />
                        <asp:Button ID="btnCancelDPopup" runat="server" CommandName="CANCELDPOPUP" OnClick="ActionHandler"
                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Close %>" TabIndex="10"
                            Text="<%$Resources:Controls,Close%>" />
                    </div>
                    <div class="detail-poi-co1">
                        <div class="popup-headr">
                            <asp:Label ID="lblDashletPopup" runat="server" Text="" Font-Bold="True" CssClass="minw-60per"></asp:Label><asp:Label
                                ID="lblGrpDashltPopup" runat="server" Text="" Font-Bold="True" CssClass="minw-50per inline"></asp:Label></div>
                    </div>
                    <div id="Div5" runat="server" class="gridwrap maxh-225">
                        <asp:HiddenField ID="hdfDashCurDBDPK_PopUp" Value="0" runat="server" />
                        <asp:HiddenField ID="hdfDashCurGroupSlNo_PopUp" Value="0" runat="server" />
                        <asp:GridView ID="grdDashletPopUp" runat="server" AutoGenerateColumns="False" Width="100%"
                            EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false" ShowFooter="false"
                            Style="table-layout: fixed;" OnRowCommand="ActionHandler" OnRowDataBound="ActionHandler">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label></EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField SortExpression="" HeaderStyle-Width="2%">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDashlet_popUp" runat="server" TabIndex="9" onclick="CheckOne(this)" />
                                        <asp:HiddenField ID="hdfDasHPK_PopUp" Value='<%# Eval("DLC_PK") %>' runat="server" />
                                        <asp:HiddenField ID="hdfDashACTION_URL_PopUp" Value='<%# Eval("DLC_LINK") %>' runat="server" />
                                        <asp:HiddenField ID="hdfQueryText" Value='<%# Eval("DLC_QUERY_TEXT") %>' runat="server" />
                                        <asp:HiddenField ID="hdfQuery" Value='<%# Eval("DLC_QUERY") %>' runat="server" />
                                        <asp:HiddenField runat="server" ID="hdfDetailQry" Value='<%# Eval("DLC_DTL_QUERY") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="2%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:DashletName %>" SortExpression="" HeaderStyle-Width="17%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDashNamePopUp" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("DLC_NAME"),60) %>'
                                            ToolTip='<%# Eval("DLC_NAME") %>'></asp:Label></ItemTemplate>
                                    <ItemStyle Width="17%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:DashletType %>" SortExpression="" HeaderStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDashTypePopUp" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("DLC_TYPE_TEXT"),60) %>'
                                            ToolTip='<%# Eval("DLC_TYPE_TEXT") %>'></asp:Label>
                                        <asp:HiddenField ID="hdfDashDLC_TYPE_PopUp" Value='<%# Eval("DLC_TYPE") %>' runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:DashletDescription %>" SortExpression=""
                                    HeaderStyle-Width="25%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDashDescPopUp" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("DLC_DESC"),60) %>'
                                            ToolTip='<%# Eval("DLC_DESC") %>'></asp:Label></ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:DisplayName %>" SortExpression="" HeaderStyle-Width="22%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDashUserMenuNamePopUp" runat="server" CssClass="input-wfull-b"
                                            MaxLength="100"></asp:TextBox></ItemTemplate>
                                    <ItemStyle Width="22%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:DashboardDefaultItem %>" SortExpression=""
                                    HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:DropDownList runat="server" ID="ddlDefult" CssClass="select-w99per">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="Div6" runat="server" class="gridwrap scroll-h180">
                        <asp:GridView ID="grdDashletPopupList" runat="server" AutoGenerateColumns="False"
                            Width="100%" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false" ShowFooter="false"
                            OnRowCommand="ActionHandler">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label></EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton CssClass="nomargin" ID="imbRuleUpPop" SkinID="move-up" runat="server"
                                            OnClick="ActionHandler" CommandName="CHANGEDASHLETSEQUENCE" ToolTip="Move Up"
                                            CommandArgument='<%# Eval("DBL_SEQUENCE") %>' TabIndex="15"></asp:ImageButton>
                                        <asp:ImageButton CssClass="nomargin" ID="imbRuleDownPop" SkinID="move-down" runat="server"
                                            OnClick="ActionHandler" CommandName="CHANGEDASHLETSEQUENCE" ToolTip="Move Down"
                                            CommandArgument='<%# Eval("DBL_SEQUENCE") %>' TabIndex="15"></asp:ImageButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="6%" />
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:DashboardDashletName %>" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDashNamePopUpList" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("DBL_NAME"),40) %>'
                                            ToolTip='<%# Eval("DBL_NAME") %>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdfDashPK_PopUpList" Value='<%# Eval("DBL_DASHLET") %>' />
                                        <asp:HiddenField runat="server" ID="hdfDashACTION_URL_PopUpList" Value='<%# Eval("DBL_LINK") %>' />
                                        <asp:HiddenField runat="server" ID="hdfDashDBL_SLNO_PopUpList" Value='<%# Eval("DBL_SL_NO") %>' />
                                        <asp:HiddenField runat="server" ID="hdfDashDefultPk_PopUpList" Value='<%# Eval("DBL_DEF_PK") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="14%" />
                                    <HeaderStyle Width="14%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:DashboardDashletType %>" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDashTypePopUpList" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("DLC_TYPE_TEXT"),40) %>'
                                            ToolTip='<%# Eval("DLC_TYPE_TEXT") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="9%" />
                                    <HeaderStyle Width="9%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:DashboardDashletDesc %>" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDashDescPopUpList" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("DLC_DESC"),40) %>'
                                            ToolTip='<%# Eval("DLC_DESC") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="33%" />
                                    <HeaderStyle Width="33%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:DashboardDefaultItem %>" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDashDefultTextPopUpList" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("DBL_DEF_NAME"),40) %>'
                                            ToolTip='<%# Eval("DBL_DEF_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="30%" />
                                    <HeaderStyle Width="30%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbDeleteMenuPopList"
                                            SkinID="imbdeletegrid" EnableViewState="false" CommandName="DELETE_ACTION" OnClientClick="return ShowDeleteConfirm(this);"
                                            ToolTip="<%$resources:Controls,Delete %>" TabIndex="12" />
                                    </ItemTemplate>
                                    <ItemStyle Width="3%" />
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div id="divMoreInfo" runat="server" visible="false">
                            <div class="search-colapse-b">
                                <h1>
                                    <asp:Literal ID="Literal1" runat="server" Text="<%$ resources: More Info %>" /></h1>
                                   <%-- <asp:ImageButton ID="imbApply" runat="server" SkinID="arrived" Text="<%$resources:Controls, Apply %>" CssClass="margntop1" OnClick="ActionHandler"
                                    CommandName="APPLY" />--%>
                                <asp:Button ID="btnApply" runat="server" SkinID="btnInner-add-dsd" Text="<%$resources:Controls, Apply %>" ToolTip="<%$resources:Controls, Apply %>" CssClass="margntop1" OnClick="ActionHandler"
                                    CommandName="APPLY" />
                            </div>
                            <asp:GridView ID="grdMoreDetails" runat="server" AutoGenerateColumns="False" Width="100%"
                                EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false" ShowFooter="false">
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label></EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkItemChecked" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="6%" />
                                        <ItemStyle Width="6%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblValue" runat="server" Text='<%# Eval("Value") %>'></asp:Label>
                                            <asp:HiddenField ID="hdfPK" runat="server" Value='<%# Eval("PK") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <div id="diverrorAlert" style="display: none">
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
                <asp:ValidationSummary ID="vsAddToList" ValidationGroup="AddToList" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label></div>
            <asp:HiddenField ID="hdfCompany" runat="server" Value="0" />
            <asp:HiddenField ID="hdfDashboardURL" runat="server" Value="0" />
            <asp:HiddenField ID="hdfAdvSearch" Value="0" runat="server" />
        </contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
