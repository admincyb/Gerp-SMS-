<%@ Page Title="<%$ Resources:Captions,Title_HRMS_SlabDefinition %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="SlabDefinitionMaster.aspx.cs"
    Inherits="HRMS.Admin.Masters.SlabDefinitionMaster" Theme="ClassicExt" %>

<%@ Register Src="~/Admin/Masters/UserControls/FormulaMaster.ascx" TagName="FormulaMaster"
    TagPrefix="ucgti" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            $(document).ready(function () {
//                GrandScriptUtils.DatePickerCommon("txtFromDate");
//                GrandScriptUtils.DatePickerCommon("txtToDate");
                GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
                GrandScriptUtils.AddDateRangeCommon("txtFilterFromDate", "hdfFilterFromDate", "txtFilterToDate", "hdfFilterToDate", false, false);
                GrandScriptUtils.MakeAutoCompleteDDL("txtPayElement", url + "?PayElmntValue=PEL_NAME", "hdfPayElement", true, true, "PAYELEMENT");
                GrandScriptUtils.MakeAutoCompleteDDL("txtSearchPayElement", url + "?PayElmntValue=PEL_NAME", "hdfSearchPayElement", true, true, "PAYELEMENT");
                $("[id*=txtAddnAmnt]").ForceNumericOnly();
                $("[id*=txtMinAmnt]").ForceNumericOnly();
                $("[id*=txtMaxAmnt]").ForceNumericOnly();
                $("[id*=txtRangeFrom]").ForceNumericOnly();
                $("[id*=txtRangeTo]").ForceNumericOnly();
                ShowHideExpand();
            });
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
                $("[id$='pnlDelete']").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
            }
        }
        //        function AfterDateSelect(controlID) {
        //            if (controlID == "txtDate") {
        //                $("[id$=btnDateChanged]").click();
        //            }
        //        }
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
        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                CheckValidationDuplicate(valGroup); //For finding and removing duplicate and other group validation controls
                Page_ClientValidate(valGroup); //For Script validating the Page
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverrorAlert").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                return true; //everythings ok --- Call your function & do your stuff
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

        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtPayElement") {
                $("[id$=btnPayElement]").click();
            }
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtPayElement") {
                $("[id$=hdfPayElement]").val('0');
            }
        }


        /// Used to disable Autocomplete
        function Disableautocomplete() {
            if ($("[id$=txtPayElement]").attr("disabled") == true) {
                DisableAuto($("[id$=txtPayElement]"), $("[id$=hdfPayElement]"));
            }
        }
        /// Used to disable Autocomplete
        function DisableAuto(extender, hfield) {
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
            $(extender).autocomplete("option", "disabled", true);
            $(extender).attr("disabled", true);
        }

        function AfterGridExpand(row) {
            if ($("[id$=grdList]").attr('id') == $(row).parent().parent().attr('id')) {
                var hdf = $(row).find("[id*=hdfIsExpandedSlab]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnSlabDetails]").click();
                }
            }
        }

        function ShowHideExpand() {
            ///<summary>
            /// Used to Show/Hide Grid Expad Button
            ///</summary>          
            $("[id*=grdVersionList] tr td[cellIndex=0]").hide();
            $("[id*=grdVersionList] tr th[cellIndex=0]").hide();

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlTaskHome" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdfCurrentELH_PK" runat="server" />
            <asp:HiddenField ID="hdfCurrentROW_NO" runat="server" />
            <div class="fixed-buttons-normal">
                <%--style="padding-bottom: 30px !important;"--%>
                <div class="Button-container">
                    <%--style="padding: 0px !important;"--%>
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$resources:Controls,Save %>"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save"
                                            ValidationGroup="Save" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('Save')"
                                            TabIndex="150" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteNew" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="151" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            CssClass="popupclose" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" OnClick="ActionHandler"
                                            TabIndex="152" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="153" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="154" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
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
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="CANCEL"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="DETAIL"
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
                            <%--<table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblFilterFromDate" runat="server" Text="<%$ resources:EffFrom%>" AssociatedControlID="txtFilterFromDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterFromDate" TabIndex="30" CssClass="input-small-c margnbotm0"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblFilterToDate" runat="server" Text="<%$ resources:EffTo%>" AssociatedControlID="txtFilterToDate"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterToDate" TabIndex="31" CssClass="input-small-c margnbotm0"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                        </div>
                                    </td>
                                </tr>
                            </table>--%>
                            <table class="table-devide" id="tbladvancedSearch">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblSearchName" runat="server" Text="<%$ resources:Name%>" AssociatedControlID="txtSearchName"></asp:Label>
                                            <asp:TextBox ID="txtSearchName" TabIndex="33" runat="server" MaxLength="100" CssClass="input-small-c margnbotm0"></asp:TextBox>
                                            <asp:Label ID="lblSearchCode" runat="server" Text="<%$ resources:Code%>" AssociatedControlID="txtSearchCode"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox ID="txtSearchCode" runat="server" TabIndex="34" CssClass="input-small-c margnbotm0"
                                                MaxLength="100"> </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblSearchPayElemnt" runat="server" Text="<%$ resources:PayElement%>"
                                                AssociatedControlID="txtSearchPayElement"></asp:Label>
                                            <asp:TextBox ID="txtSearchPayElement" runat="server" TabIndex="34" CssClass="input-half margnbotm0"
                                                MaxLength="100"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfSearchPayElement" runat="server" Value="" />
                                            <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Search%>" ToolTip="<%$ resources:Search%>"
                                                ValidationGroup="Search" OnClick="ActionHandler" TabIndex="36" CommandName="FILTER"
                                                SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Clear%>" ToolTip="<%$ resources:Clear%>"
                                                TabIndex="37" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext"
                                                CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap hierarchical-wrap2">
                                <%-- <asp:GridView runat="server" ID="grdList" Width="100%" AllowPaging="false" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" OnSorting="ActionHandler">--%>
                                <cc1:ExtGridView runat="server" ID="grdList" AutoGenerateColumns="False" ExpandButtonCssClass="GridExpandCollapseButton"
                                    CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                                    CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="True"
                                    CssClass="margnbotm5" OnSorting="ActionHandler" Width="100%" OnRowDataBound="ActionHandler"
                                    PageSize="<%$ resources:PageSize %>">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Code%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PHS_CODE"))),15) %>'
                                                    ToolTip='<%# Eval("PHS_CODE")  %>'></asp:Label>
                                                <%--<asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" TabIndex="38"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />--%>
                                                <asp:HiddenField runat="server" ID="hdfSlabPk" Value='<%# Eval("PHS_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfModifiedDate" Value='<%# Eval("PHS_MOD_DT") %>' />
                                                <asp:Button runat="server" ID="btnSlabDetails" OnClick="ActionHandler" CommandName="VERSIONLIST"
                                                    CommandArgument='<%# Eval("PHS_PK") %>' EnableTheming="false" Style="display: none" />
                                                <asp:HiddenField runat="server" ID="hdfIsExpandedSlab" Value="0" />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Name%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblListName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PHS_NAME"))),40) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PHS_NAME"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="24%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PayElement%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblListPayElement" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PHS_PAY_ELEMENT_TEXT"))),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PHS_PAY_ELEMENT_TEXT"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="17%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:MinAmnt%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblListMinAmt" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PHS_MIN_AMT","{0:c}"))),20) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PHS_MIN_AMT","{0:c}"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="amount-numeric" Wrap="false" />
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:MaxAmnt%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblListMaxAmt" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PHS_MAX_AMT","{0:c}"))),20) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PHS_MAX_AMT","{0:c}"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="amount-numeric" Wrap="false" />
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Description%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblListDescription" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PHS_DESC"))),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PHS_DESC")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                        </asp:TemplateField>
                                        <%-- HeaderText="<%$ resources:Status %>"--%>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imbAddNewVersion" runat="server" OnClick="ActionHandler" CommandName="ADDNEWVERSION"
                                                    TabIndex="18" SkinID="add_small-icon" ToolTip="<%$ resources:AddNewVersion %>" />
                                                <asp:ImageButton ID="imbActive" runat="server" SkinID="btninactive" Visible='<%# (Eval("PHS_ACTIVE").ToString() == "0") ? true  : false %>'
                                                    CommandName="ACTIVATE" ToolTip="<%$ resources:Inactive %>" OnClick="ActionHandler"
                                                    CssClass="Active" />
                                                <asp:ImageButton ID="imbInActive" runat="server" SkinID="btnactive" Visible='<%# (Eval("PHS_ACTIVE").ToString() == "1") ? true  : false %>'
                                                    CommandName="DEACTIVATE" ToolTip="<%$ resources:Active %>" OnClick="ActionHandler"
                                                    CssClass="Active" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" HorizontalAlign="Center" Wrap="false" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField>
                                            <ItemTemplate>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div class="hierarchical-gridwrap">
                                                    <%-- <asp:GridView runat="server" ID="grdVersionList" Width="100%" AutoGenerateColumns="false"
                                                        EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-CssClass="emptytable"
                                                        OnSorting="ActionHandler">--%>
                                                    <cc1:ExtGridView runat="server" ID="grdVersionList" AutoGenerateColumns="False" ExpandButtonCssClass="GridExpandCollapseButton"
                                                        CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                                                        CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" AllowPaging="false"
                                                        OnRowDataBound="ActionHandler" Width="100%">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="lblInnerEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:RadioButton CssClass="rdoSelection" runat="server" TabIndex="24" GroupName="SelectOne"
                                                                        ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGroupingHierarchy(this, 'grdList');" />
                                                                    <asp:HiddenField runat="server" ID="hdfVersionPk" Value='<%# Eval("PEV_PK") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="3%" />
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField HeaderText="<%$ resources:Version %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVersion" runat="server" Text='<%# Eval("PEV_VERSION") %>' ToolTip='<%# Eval("PEV_VERSION") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="<%$ resources:EffFrom %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEffFrom" runat="server" Text='<%# Eval(Resources.DataFieldRes.PEV_EFFECT_FROM, Resources.Constants.DateFormatGrid) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.PEV_EFFECT_FROM, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:EffTo %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEffTo" runat="server" Text='<%# Eval(Resources.DataFieldRes.PEV_EFFECT_TO, Resources.Constants.DateFormatGrid) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.PEV_EFFECT_TO, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Description%> " SortExpression="">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVerDescription" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PEV_DESC"))),100) %>'
                                                                        ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PEV_DESC")))%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="50%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <%--Second--%>
                                                        <RowStyle CssClass="table-secondlevel" />
                                                        <HeaderStyle CssClass="table-secondlevela" />
                                                    </cc1:ExtGridView>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="table-firstlevel" />
                                    <HeaderStyle CssClass="table-firstlevela" />
                                    <FooterStyle CssClass="table-firstlevela-total" />
                                </cc1:ExtGridView>
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
                                            <asp:Label ID="lblSlabCode" runat="server" Text="<%$ resources:CodeReq %>" AssociatedControlID="txtSlabCode"></asp:Label>
                                            <asp:TextBox ID="txtSlabCode" runat="server" CssClass="input-small" TabIndex="1" MaxLength="100"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfSlabCode" runat="server" ControlToValidate="txtSlabCode"
                                                Display="Dynamic" CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_Code%>"></asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblPayElement" runat="server" Text="<%$ resources:PayElementReq%>"
                                                AssociatedControlID="txtPayElement"></asp:Label>
                                            <asp:TextBox ID="txtPayElement" runat="server" TabIndex="3" CssClass="select-half"
                                                MaxLength="200" />
                                            <asp:RequiredFieldValidator ID="vrfPayElement" runat="server" ControlToValidate="txtPayElement"
                                                Display="Dynamic" CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_Payelement%>"
                                                InitialValue="<%$resources:ErpRes,AutoDefaultValue %>"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="vrfFormulaPayElemnt" runat="server" ControlToValidate="txtPayElement"
                                                Display="Dynamic" CssClass="star" ValidationGroup="Formula" Text="*" ErrorMessage="<%$ resources:Err_Payelement%>"
                                                InitialValue="<%$resources:ErpRes,AutoDefaultValue %>"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="vrfRangeFormula" runat="server" ControlToValidate="txtPayElement"
                                                Display="Dynamic" CssClass="star" ValidationGroup="RangeFormula" Text="*" ErrorMessage="<%$ resources:Err_Payelement%>"
                                                InitialValue="<%$resources:ErpRes,AutoDefaultValue %>"></asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfPayElement" runat="server" />
                                            <asp:Button ID="btnPayElement" runat="server" Text="" OnClick="ActionHandler" CommandName="PAYELEMENTCHANGE"
                                                SkinID="btnInner-search" EnableTheming="false" Style="display: none" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="label5" runat="server" Text="<%$ resources:BasedOnReq %>" AssociatedControlID="txtBasedOn"
                                                class=""></asp:Label>
                                            <asp:TextBox runat="server" ID="txtBasedOn" TabIndex="6" CssClass="input-half input-disabled"
                                                Enabled="false"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvBasedOn" runat="server" ControlToValidate="txtBasedOn"
                                                Display="Dynamic" CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_BasedOn%>"></asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfBasedOnValue" runat="server" Value="" />
                                            <asp:ImageButton ID="btnPopUp" runat="server" OnClick="ActionHandler" CommandName="BASEDONFORMULAPOPUP"
                                                TabIndex="7" SkinID="salary-formula" ToolTip="<%$ resources:Controls,ApplyFormula %>"
                                                OnClientClick="javascript:ValidatePageNow('Formula')" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblMinAmnt" runat="server" Text="<%$ resources:MinAmnt%>" AssociatedControlID="txtMinAmnt"></asp:Label>
                                            <asp:TextBox ID="txtMinAmnt" runat="server" CssClass="input-small numeric" TabIndex="9"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvMinAmnt" runat="server" ControlToValidate="txtMinAmnt"
                                                Display="Dynamic" CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_MinAmnt%>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label ID="lblMaxAmnt" runat="server" Text="<%$ resources:MaxAmnt%>" AssociatedControlID="txtMaxAmnt"
                                                CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:TextBox ID="txtMaxAmnt" runat="server" CssClass="input-small numeric" TabIndex="10"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvMaxAmnt" runat="server" ControlToValidate="txtMaxAmnt"
                                                Display="Dynamic" CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_MaxAmnt%>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblSlabName" runat="server" Text="<%$ resources:NameReq %>" AssociatedControlID="txtSlabName"></asp:Label>
                                            <asp:TextBox ID="txtSlabName" runat="server" CssClass="input-half" TabIndex="2" MaxLength="180"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfSlabName" runat="server" ControlToValidate="txtSlabName"
                                                Display="Dynamic" CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_SlabName%>"></asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblDirect" runat="server" Text="<%$ resources:Direct%>" AssociatedControlID="lblDirect"></asp:Label>
                                            <asp:CheckBox ID="chkDirect" runat="server" TabIndex="4" />
                                            <asp:Label ID="lblAddAmount" runat="server" Text="<%$ resources:AddnAmnt%>" AssociatedControlID="txtAddnAmnt"
                                                CssClass="lbl-38-3perc"></asp:Label>
                                            <asp:TextBox ID="txtAddnAmnt" runat="server" CssClass="input-small numeric" TabIndex="5"
                                                ValidationGroup="Save"> </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblDesc" runat="server" Text="<%$ resources:Description%>" AssociatedControlID="txtDescription"></asp:Label>
                                            <asp:TextBox ID="txtDescription" runat="server" CssClass="input-half" TabIndex="8"
                                                MaxLength="500"> </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblActive" runat="server" Text="<%$ resources:Active%>" AssociatedControlID="lblActive"></asp:Label>
                                            <asp:CheckBox ID="chkActive" runat="server" TabIndex="11" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="search-colapse-b">
                                            <h1>
                                                <asp:Literal ID="Literal1" runat="server" Text="<%$ resources:VersionDetails%>" /></h1>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblFromDate" runat="server" Text="<%$ resources:EffFromReq%>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="input-small" TabIndex="12"
                                                MaxLength="11" ValidationGroup="Save" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="txtFromDate"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_FromDate%>"></asp:RequiredFieldValidator>
                                                <asp:HiddenField ID="hdfFromDate" Value="" runat="server" />
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$ resources:EffToReq%>" AssociatedControlID="txtToDate"
                                                CssClass="lbl-19-6perc"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="input-small" TabIndex="13" MaxLength="11"
                                                ValidationGroup="Save" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_ToDate%>"></asp:RequiredFieldValidator>
                                                <asp:HiddenField ID="hdfToDate" Value="" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblVerDesc" runat="server" Text="<%$ resources:Description%>" AssociatedControlID="txtDescription"></asp:Label>
                                            <asp:TextBox ID="txtVerDesc" runat="server" CssClass="input-half" TabIndex="14" MaxLength="500"> </asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="search-colapse-b">
                                            <h1>
                                                <asp:Literal ID="ltrDetails" runat="server" Text="<%$ resources:SlabDetails%>" /></h1>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblRangeFrom" runat="server" Text="<%$ resources:RangeFromReq%>" AssociatedControlID="txtRangeFrom"></asp:Label>
                                            <asp:TextBox ID="txtRangeFrom" runat="server" CssClass="input-small numeric" TabIndex="14"
                                                ValidationGroup="Save"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfRangeFrom" runat="server" ControlToValidate="txtRangeFrom"
                                                CssClass="star" ValidationGroup="AddToList" Text="*" ErrorMessage="<%$ resources:Err_RangeFrom%>"></asp:RequiredFieldValidator>
                                            <asp:Label ID="lblRangeTo" runat="server" Text="<%$ resources:RangeToReq%>" AssociatedControlID="txtRangeTo"
                                                CssClass="lbl-19-6perc"></asp:Label>
                                            <asp:TextBox ID="txtRangeTo" runat="server" CssClass="input-small numeric" TabIndex="15" 
                                                ValidationGroup="Save" MaxLength="20"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfRangeTo" runat="server" ControlToValidate="txtRangeTo"
                                                Display="Dynamic" CssClass="star" ValidationGroup="AddToList" Text="*" ErrorMessage="<%$ resources:Err_RangeTo%>"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cmpRangeTo" runat="server" ControlToValidate="txtRangeTo"
                                                Display="Dynamic" ControlToCompare="txtRangeFrom" Operator="GreaterThan" Type="Double"
                                                CssClass="star" ValidationGroup="AddToList" Text="*" ErrorMessage="<%$ resources:Err_InvalidRangeTo%>"></asp:CompareValidator>
                                               <asp:RangeValidator ID="rngRangeTo" runat="server" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="AddToList" EnableClientScript="true" Display="Dynamic" Text="*"
                                                            ControlToValidate="txtRangeTo" Type="Double" ErrorMessage="<%$ resources:Err_Invalid_RangeTo %>"
                                                            MinimumValue="-1" MaximumValue="999999999999999"></asp:RangeValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblValue" runat="server" Text="<%$ resources:FormulaReq%>" AssociatedControlID="txtRangeValue"></asp:Label>
                                            <asp:TextBox ID="txtRangeValue" runat="server" CssClass="input-half input-disabled"
                                                TabIndex="16" Enabled="false"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfRangeValue" runat="server" ControlToValidate="txtRangeValue"
                                                Display="Dynamic" CssClass="star" ValidationGroup="AddToList" Text="*" ErrorMessage="<%$ resources:Err_RangeValue%>"></asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfRangeValue" runat="server" Value="" />
                                            <asp:ImageButton ID="imbRangeTo" runat="server" OnClick="ActionHandler" CommandName="RANGEFORMULAPOPUP"
                                                TabIndex="17" SkinID="salary-formula" ToolTip="<%$ resources:Controls,ApplyFormula %>"
                                                OnClientClick="javascript:ValidatePageNow('RangeFormula')" />
                                            <asp:ImageButton ID="imbAdd" runat="server" OnClick="ActionHandler" CommandName="ADD"
                                                ValidationGroup="AddToList" OnClientClick="javascript:ValidatePageNow('AddToList')"
                                                TabIndex="18" SkinID="imbaddnew" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdSlabDetList" Width="100%" AllowPaging="false"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:SlNo%> " SortExpression="">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:RangeFrom%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("PDS_RANGE_FROM")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithComma(Eval("PDS_RANGE_FROM")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:RangeTo%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRangeTo" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("PDS_RANGE_TO")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithComma(Eval("PDS_RANGE_TO")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" SortExpression="">
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <HeaderStyle Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Formula%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblValue" runat="server" Text='<%# Eval("PDS_VALUE_TEXT")%>' ToolTip='<%# Eval("PDS_VALUE_TEXT")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="50%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_edit" runat="server" ID="imbEditDetails"
                                                    TabIndex="19" SkinID="imbeditgrid" EnableViewState="false" CommandName="GRIDEDIT"
                                                    OnClick="ActionHandler" ToolTip="<%$ resources:Edit %>" />
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbDeleteDetails"
                                                    OnClick="ActionHandler" SkinID="imbdeletegrid" EnableViewState="false" CommandName="GRIDDELETE"
                                                    OnClientClick="return ShowDeleteConfirm(this);" ToolTip="<%$ resources:Delete %>"
                                                    TabIndex="19" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" Wrap="false" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:TableCell></asp:TableRow></asp:Table></div><div id="diverrorAlert" style="display: none">
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
                <asp:ValidationSummary ID="vsAddToList" ValidationGroup="AddToList" runat="server" />
                <asp:ValidationSummary ID="vsFormula" ValidationGroup="Formula" runat="server" />
                <asp:ValidationSummary ID="vsRangeFormula" ValidationGroup="RangeFormula" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label></div><div id="divPopUpFormula" style="display: none">
                <ucgti:FormulaMaster id="ucFormula" runat="server" afterapply="ucFormula_AfterApply"
                    IsSlab="0" ShowMinMaxAmount="0" />
            </div>
            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormatWithComma" runat="server" />
            <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
