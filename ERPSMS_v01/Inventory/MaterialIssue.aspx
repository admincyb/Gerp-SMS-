<%@ Page Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="MaterialIssue.aspx.cs" Inherits="ERPSMS_v01.Inventory.MaterialIssue"
    ValidateRequest="false" EnableEventValidation="false" Theme="ClassicExt" Title="<%$ Resources:Captions,Title_ExternalMaterialIssueMultiple %>" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $("[id$=txtQtyIssued]").ForceNumericOnly();
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtFilterItemCategory", url + "?Type=0", "hdfFilterItemCategory", true, true, "ITEMCATEGORY");
            GrandScriptUtils.MakeAutoCompleteDDL("txtFilterIssueNo", url, "hdfFilterIssueNo", false, true, "MATERIALISSUENO");
            ShowHideAdvancedSearch();
            GrandScriptUtils.DatePickerCommon("txtDate");
            GrandScriptUtils.MakeAutoCompleteDDL("txtItemCategory", url + "?Type=0", "hdfItemCategory", true, true, "ITEMCATEGORY");
            BindItemCode();
            GrandScriptUtils.MakeAutoCompleteDDL("txtFilterItemName", url + "?Type=" + $("[id$=ddlFilterType]").val() + "&IssueAgainst=" + $("[id$=ddlFilterIssueAgainst]").val(), "hdfFilterItemName", true, true, "ITEMNAME");
            GrandScriptUtils.MakeAutoCompleteDDL("txtItemName", url + "?Type=" + $("[id$=ddlType]").val() + "&IssueAgainst=" + $("[id$=ddlIssueAgainst]").val(), "hdfItemName", true, true, "ITEMNAME");
            GrandScriptUtils.MakeAutoCompleteDDL("txtFilterItem", url + "?Type=0", "hdfFilterItem", true, true, "ITEMCODE");
            //$("[id$=txtReference]").ForceNumericOnly();
            $("[id$=txtQtyIssued]").ForceNumericOnly();
        }

        function InitDetailPage() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtItemCategory", url + "?Type=0", "hdfItemCategory", true, true, "ITEMCATEGORY");
            BindItemCode();
            GrandScriptUtils.DatePickerCommon("txtDate");
        }
        function InitDetailPageFilter() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtFilterItemCategory", url + "?Type=0", "hdfFilterItemCategory", true, true, "ITEMCATEGORY");
            BindFilterItemCode();
            GrandScriptUtils.DatePickerCommon("txtFromDate");
            GrandScriptUtils.DatePickerCommon("txtToDate");
        }
        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtItemCategory") {
                BindItemCode();
                ResetItemCode();
                $("[id$=txtQtyIssued]").ForceNumericOnly();
            }
            if (targetControlID == "txtFilterItemCategory") {
                BindFilterItemCode();
            }
            if (targetControlID == "txtItemCode") {
                $("[id$=btnSelectCurrentStock]").click();
                //                 
                //                if ($("[id$=hdfEnableBatch]").val() == "0" || $("[id$=hdnItmNeedBatchStk]").val() == "0") {
                //                    $("[id$=btnSelectCurrentStock]").click();
                //                    $("[id$=BatchNo]").val("");
                //                    $("[id$=BatchNoPK]").val("0");
                //                    $("[id$=BatchNo]").next("a").remove();
                //                    $("[id$=BatchNo]").attr("disabled", true);
                //                }
                //                else {
                //                    BindBatchNo();
                //                }

            }
            if (targetControlID == "txtBatchNo") {
                $("[id$=btnSelectItemStock]").click();
            }
            if (targetControlID == "txtItemName") {
                $("[id$=btnAssetPostback]").click();
            }
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtItemCategory") {
                $("[id$=hdfItemCategory]").val("-1");
                BindItemCode();
                ResetItemCode();

            }
        }
        function BindItemCode() {
            if ($("[id$=hdfItemCategory]").val() > 0) {
                GrandScriptUtils.MakeAutoCompleteDDL("txtItemCode", url + "?Type=" + $("[id$=hdfItemCategory]").val(), "hdfItemCode", true, true, "ITEMCODE");
            }
            else {
                GrandScriptUtils.MakeAutoCompleteDDL("txtItemCode", url + "?Type=0", "hdfItemCode", true, true, "ITEMCODE");
            }
            //ResetItemCode();
        }
        function BindFilterItemCode() {
            if ($("[id$=hdfFilterItemCategory]").val() > 0) {
                GrandScriptUtils.MakeAutoCompleteDDL("txtFilterItem", url + "?Type=" + $("[id$=hdfFilterItemCategory]").val(), "hdfFilterItem", true, true, "ITEMCODE");
            }
            else {
                GrandScriptUtils.MakeAutoCompleteDDL("txtFilterItem", url + "?Type=0", "hdfFilterItem", true, true, "ITEMCODE");
            }
            // ResetItemCode();
        }
        function ResetItemCode() {
            var autoVal = '<%= Resources.ErpRes.AutoDefaultValue %>';
            $("[id$=txtItemCode]").val(autoVal);
            $("[id$=hdfItemCode]").val('-1');
            $("[id$=txtStockValue]").val('');
            $("[id$=hdfBatchNo]").val('-1');
            //$("[id$=txtBatchNo]").val(autoVal);
            $("[id$=txtBatchNo]").val('');
            if ($("[id$=hdfEnableBatch]").val() == "1") {
                BindBatchNo()
            }

        }
        function AfterDateSelect(controlID) {
            if (controlID == "txtDate") {
                BindBatchNo();
            }
        }
        function ClearDateSelect() {
            if ($("[id$=txtDate]").val() == '') {
                BindBatchNo();
            }
        }

        function BindBatchNo() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtBatchNo", url + "?materialID=" + $("[id$=hdfItemCode]").val() + "&excDate=" + $("[id$=txtDate]").val(), "hdfBatchNo", true, true, "BATCHNO");
        }
        function BindAssetAuto() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtItemName", url + "?Type=" + $("[id$=ddlType]").val() + "&IssueAgainst=" + $("[id$=ddlIssueAgainst]").val(), "hdfItemName", true, true, "ITEMNAME");
        }

        function ShowListing(flag) {
            if (flag) {
                $("[id$='PageAction_List']").show();
                $("[id$='PageAction_Entry']").hide();
                $("[id$='pnlListing']").show();
                $("[id$='pnlEntry']").hide();
                $("[id$=pnlPrint]").hide();
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

            //View      
            if (mode == 1) {
                //                $("[id$='pnlSave']").hide();
                //                $("[id$='pnlDelete']").hide();
                $("[id$=pnlSubmit]").show();
                $("[id$=pnlCancel]").show();
                $("[id$=pnlPrint]").show();
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlSaveSubmit]").hide();
                $("[id$=pnlSave]").hide();
                $("[id$=pnlRefresh]").hide();
                $("[id$=pnlNew]").hide();
                $("[id$='pnlListing']").show();

            }
            //New
            else if (mode == 2) {
                //$("[id$=pnlDelete]").hide();
                $("[id$=pnlSaveSubmit]").show();
                $("[id$=pnlSave]").show();
                $("[id$=pnlCancel]").show();
                $("[id$=pnlSubmit]").hide();
                $("[id$=pnlPrint]").hide();
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlRefresh]").hide();
                $("[id$=pnlNew]").hide();
                $("[id$='pnlListing']").show();
            }
            //Edit
            else if (mode == 3) {
                $("[id$=pnlSubmit]").hide();
                $("[id$=pnlCancel]").show();
                $("[id$=pnlPrint]").show();
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlSaveSubmit]").show();
                $("[id$=pnlSave]").show();
                $("[id$=pnlRefresh]").hide();
                $("[id$=pnlNew]").hide();
                $("[id$='pnlListing']").show();
            }
            //Modify
            else if (mode == 4) {
                $("[id$=pnlSubmit]").show();
                $("[id$=pnlSave]").show();
                $("[id$=pnlCancel]").show();
                $("[id$=pnlPrint]").show();
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlSaveSubmit]").hide();
                $("[id$=pnlRefresh]").hide();
                $("[id$=pnlNew]").hide();
                $("[id$='pnlListing']").show();
            }
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
                    //                    else {
                    //                        Page_Validators.splice(i, 1);
                    //                    }
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


        function ShowHideAdvancedSearch(flag) {
            if (flag == 1) {
                $("[id$=hdfAdvancedSearchType]").val('1');
                $("[id$=tbladvancedSearch]").show();
                $("[id$=imbShowFilter]").hide();
                $("[id$=imbHideFilter]").show();
            }
            else {
                $("[id$=hdfAdvancedSearchType]").val('0')
                $("[id$=tbladvancedSearch]").hide();
                $("[id$=imbShowFilter]").show();
                $("[id$=imbHideFilter]").hide();
            }
            return false;
        }

        function BindTypeDDL() {
            $("[id$=btnSelectItemType]").click();
        }
        function BindItemNameTypeDDL() {
            $("[id$=btnSelectItemName]").click();
        }
        function BindTypeDDLFilter() {
            //ShowHideAdvancedSearch(1);
            $("[id$=btnSelectItemTypeFilter]").click();
        }
        function BindItemName() {
            ResetItemName();
            GrandScriptUtils.MakeAutoCompleteDDL("txtItemName", url + "?Type=" + $("[id$=ddlType]").val() + "&IssueAgainst=" + $("[id$=ddlIssueAgainst]").val(), "hdfItemName", true, true, "ITEMNAME");
        }
        function BindItemNameDDL() {
            //BindItemNameTypeDDL();
            if ($("[id$=hdfIsEdit]").val() == 1) {
                GrandScriptUtils.MakeAutoCompleteDDL("txtItemName", url + "?Type=" + $("[id$=ddlType]").val() + "&IssueAgainst=" + $("[id$=ddlIssueAgainst]").val(), "hdfItemName", true, true, "ITEMNAME");
            }
            else {
                ResetItemName();
                GrandScriptUtils.MakeAutoCompleteDDL("txtItemName", url + "?Type=" + $("[id$=ddlType]").val() + "&IssueAgainst=" + $("[id$=ddlIssueAgainst]").val(), "hdfItemName", true, true, "ITEMNAME");
            }
        }

        function ResetItemName() {
            var autoVal = '<%= Resources.ErpRes.AutoDefaultValue %>';
            $("[id$=txtItemName]").val(autoVal);
            $("[id$=hdfItemName]").val('-1');
        }
        function ResetItemNameFilter() {
            var autoVal = '<%= Resources.ErpRes.AutoDefaultValue %>';
            $("[id$=txtFilterItemName]").val(autoVal);
            $("[id$=hdfFilterItemName]").val('-1');
        }

        function BindFilterItemNameDDL() {
            ResetItemNameFilter();
            GrandScriptUtils.MakeAutoCompleteDDL("txtFilterItemName", url + "?Type=" + $("[id$=ddlFilterType]").val() + "&IssueAgainst=" + $("[id$=ddlFilterIssueAgainst]").val(), "hdfFilterItemName", true, true, "ITEMNAME");
        }
        //For CBM Check
        function ShowCBMCheckConfirming(AlertMsg, flag) {

            var msgTitle;
            var msg;
            var IsFromOk = 0;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = AlertMsg;
            $("#popupHolder").html("");
            //To set fit to screen
            $('html, body').animate({ scrollTop: '0px' }, 0);
            $('html, body').css('overflow', 'hidden');
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $("[id$=hdfIscartYes]").val(1);
                        if (flag == 1 || flag == 2) { $("[id$=hdfIsWrkflwClose]").val(-1); }
                        IsFromOk = 1;
                        $('#divmodel').hide();
                        $(this).dialog("close");
                        if (flag == 0) {//AddItem
                            $("[id$=btnAddItem]").click();
                        }
                        else if (flag == 1) {//Save                            
                            if ($("[id$=btnSave]").length > 0) {
                                $("[id$=btnSave]").click();
                            }
                            else {
                                $("[id$=btnIOReviewSave]").click();
                            }
                        }
                        else if (flag == 2) {//Save & Submit ButtonClick
                            $("[id$=btnDummySaveSubmit]").click();
                        }
                    },
                    Cancel: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $("[id$=hdfIscartYes]").val(0);
                        $('#divmodel').hide();
                        $(this).dialog("close");
                        return false;
                    }
                },
                open: function (event, ui) {
                    if ($('#popupHolder div.ui-dialog').is(':visible')) {
                        var heightDiff = ($('html').height() - $('#popupHolder div.ui-dialog').height()) / 2;
                        $('#divmodel').height(heightDiff > 0 ? $('html').height() : $('#popupHolder div.ui-dialog').height());
                        $('html').scrollTop(0);
                        $('html,body').animate({ scrollTop: 0 });
                        $('#popupHolder div.ui-dialog').css('top', heightDiff > 0 ? heightDiff : 0);
                    }
                },
                close: function (event) {
                    $('html').css('overflow', 'auto');
                    $('body').css('overflow', 'visible');
                    $('#divmodel').hide();
                    if ($("[id$=hdfIsWrkflwClose]").val() != -1) {
                        $("[id$=hdfIsWrkflwClose]").val(1);
                    }
                    if (IsFromOk != 1) {
                        $("[id$=hdfOldCBMLimitPK]").val(0);
                        $("[id$=hdfNewCBMLimitPK]").val(0);
                    }
                }
            });
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlTaskHome" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" Text="<%$resources:ErpRes,SaveSubmit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('EMIMultipleSave')"
                                            ValidationGroup="so" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" TabIndex="30" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('EMIMultipleSave')"
                                            ValidationGroup="so" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" TabIndex="31" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                            ToolTip="<%$Resources:Controls,Save%>" EnableViewState="False" CommandName="SAVE"
                                            OnClientClick="javascript:ValidatePageNow('EMIMultipleSave')" OnClick="ActionHandler"
                                            ValidationGroup="EMIMultipleSave" TabIndex="32" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" CommandName="DELETE" SkinID="btnInner-Delete" ToolTip="<%$resources:Controls,Delete %>" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CLEAR" SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>"
                                            TabIndex="33" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li runat="server" id="pnlNew">
                                        <asp:Button runat="server" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" TabIndex="14" />
                                    </li>
                                    <li runat="server" id="pnlEdit" visible="false">
                                        <asp:Button runat="server" ID="btnEdit" CommandName="DETAIL" Text="<%$resources:Controls,Edit %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li runat="server" id="pnlRefresh">
                                        <asp:Button ID="btnReset" runat="server" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Refresh%>"
                                            ToolTip="<%$resources:Controls,Refresh %>" OnClick="ActionHandler" CommandName="CLEAR"
                                            TabIndex="13" />
                                    </li>
                                    <li runat="server" id="pnlPrint">
                                        <asp:Button runat="server" ID="btnPrint" SkinID="btnInner-Print" Text="<%$Resources:Controls,Print%>"
                                            EnableViewState="False" ToolTip="<%$resources:ErpRes,Print %>" OnClick="ActionHandler"
                                            CommandName="PRINT" TabIndex="34" /></li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc2:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="so" />
            </div>
            <div class="content-wrapper">
                <div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="DETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblGstClassification" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server" Style="display: none;">
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
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:ShowFilter%>" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <%--------------colpase btn----------%>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="/*margin-top: 8px; */ background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblFromDate" Text="<%$ resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFromDate" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                onpaste="return false;" TabIndex="1"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" />
                                            <asp:Label runat="server" ID="lblToDate" Text="<%$ resources:ToDate %>" AssociatedControlID="txtToDate"
                                                CssClass="lbl-21-5perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtToDate" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                onpaste="return false;" TabIndex="2"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblFilterItemCategory" Text="<%$ resources:ItemCategoryList%>"
                                                AssociatedControlID="txtFilterItemCategory"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterItemCategory" CssClass="input-half" TabIndex="5"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterItemCategory" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblFilterStatus" Text="<%$ resources:Captions,Status %>"
                                                AssociatedControlID="ddlFilterStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlFilterStatus" runat="server" CssClass="select-small-b" TabIndex="3">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Submitted %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="4"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hdfReqStore" runat="server" />
                                            <asp:Label runat="server" ID="lblFilterIssuingStore" Text="<%$ resources:IssuingStoreList %>"
                                                CssClass="lbl-13perc" AssociatedControlID="ddlFilterIssuingStore"></asp:Label>
                                            <asp:DropDownList ID="ddlFilterIssuingStore" runat="server" CssClass="select-small-e"
                                                TabIndex="4">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblFilterItem" Text="<%$ Resources:ItmCode%>" AssociatedControlID="txtFilterItem"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterItem" CssClass="input-half" TabIndex="6"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterItem" runat="server" />
                                            <%-- <asp:Label runat="server" ID="lblReqBy" Text="ReqBy" AssociatedControlID="txtReqBy"
                                                CssClass="lbl-13perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtReqBy" TabIndex="7" CssClass="input-small-c0"></asp:TextBox>
                                            <asp:HiddenField ID="hdfReqBy" runat="server" />--%>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label runat="server" ID="lblFilterIssueNo" Text="<%$ Resources:IssueNoList%>"
                                                AssociatedControlID="txtFilterIssueNo"></asp:Label>
                                            <asp:TextBox ID="txtFilterIssueNo" runat="server" EnableViewState="false" CssClass="input-small margnbotm0"
                                                TabIndex="7">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterIssueNo" runat="server" />
                                            <asp:Label runat="server" ID="lblFilterIssueAgainst" Text="<%$ Resources:IssueAgainstList%>"
                                                AssociatedControlID="ddlFilterIssueAgainst" CssClass="lbl-21-1perc"></asp:Label>
                                            <asp:DropDownList ID="ddlFilterIssueAgainst" runat="server" CssClass="select-small-b margnbotm0 margn-rgt2"
                                                onchange="javascript:BindTypeDDLFilter();" TabIndex="8">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label runat="server" ID="lblFilterType" Text="<%$ Resources:TypeList%>" AssociatedControlID="ddlFilterType"></asp:Label>
                                            <asp:DropDownList ID="ddlFilterType" runat="server" CssClass="select-small-b margnbotm0 margn-rgt2"
                                                onchange="javascript:BindFilterItemNameDDL();" TabIndex="9">
                                            </asp:DropDownList>
                                            <asp:Label runat="server" ID="lblFilterItemName" Text="<%$ Resources:ItemNameList%>"
                                                AssociatedControlID="txtFilterItemName" CssClass="lbl-13perc"></asp:Label>
                                            <asp:TextBox ID="txtFilterItemName" runat="server" EnableViewState="false" CssClass="input-small-c0 margnbotm0"
                                                TabIndex="10">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterItemName" runat="server" />
                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                SkinID="search-ext" Style="margin-bottom: 0px!important; margin-top: 2px;" OnClick="ActionHandler"
                                                CommandName="SEARCH" TabIndex="11" />
                                            <asp:ImageButton ID="btnClear" runat="server" Style="margin-bottom: 0px!important;
                                                margin-top: 2px;" ToolTip="<%$ resources:Controls,Clear %>" SkinID="clear-ext"
                                                OnClick="ActionHandler" CommandName="CLEAR" TabIndex="12" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdMaterialIssueList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" OnPageIndexChanging="ActionHandler" CssClass="grdTable"
                                    OnSorting="ActionHandler" OnRowCommand="ActionHandler" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    OnCheckedChanged="ActionHandler" />
                                                <asp:HiddenField runat="server" ID="hdfItemPk" Value='<%# Eval("ICH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfEditQtyIssued" Value='<%# Eval("ICH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfUserStatus" Value='<%# Eval("USER_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfIchStatus" Value='<%# Eval("ICH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="0.5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Date %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdDate" runat="server" Text='<%# Eval("ICH_DATE")%>' ToolTip='<%# Eval("ICH_DATE")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="85px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:IssueNoList%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdIssueNo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ICH_NO")),20) ==""?"[NEW]":ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ICH_NO")),20)%>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ICH_NO")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="130px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Code %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ICH_NO")),20) ==""?"[NEW]":ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ICH_ITEM_CODE")),20)%>'
                                                    ToolTip='<%# Eval("ICH_ITEM_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="190px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:IssueAgainstList %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdIssueAgainst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ICH_ISS_RCV_TYPE_TEXT")),20) %>'
                                                    ToolTip='<%# Eval("ICH_ISS_RCV_TYPE_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="180px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TypeList %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ICH_ISS_RCV_SUB_TYPE_TEXT")),20) %>'
                                                    ToolTip='<%# Eval("ICH_ISS_RCV_SUB_TYPE_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="180px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Name %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ICH_ISS_RCV_TEXT")),20) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ICH_ISS_RCV_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="200px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Status %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdStatus" runat="server" Text='<%# Eval("ICH_STATUS_TEXT") %>'
                                                    ToolTip='<%# Eval("ICH_STATUS_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="40px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,DoneBy %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdDoneBy" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ICH_MOD_BY_TEXT")),10) %>'
                                                    ToolTip='<%# Eval("ICH_MOD_BY_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="90px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$resources:ErpRes,Edit %>"
                                                    SkinID="imbeditgrid" CommandName="EDITACTION" />
                                                <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$resources:ErpRes,Delete %>"
                                                    SkinID="imbdeletegrid" CommandName="DELETEEMI" OnClientClick="return ShowDeleteConfirm(this);" />
                                                <asp:ImageButton runat="server" ID="imbModify" SkinID="imbeditgrid" title="<%$Resources:Controls,Modify%>"
                                                    CommandName="MODIFY" />
                                                <asp:ImageButton runat="server" ID="imbView" ToolTip="<%$resources:ErpRes,View %>"
                                                    SkinID="btnview" CommandName="VIEW" />
                                                <asp:ImageButton runat="server" ID="imbCancel" CommandName="CANCELEMI" SkinID="cancel"
                                                    alt="<%$Resources:Controls,Cancel%>" title="<%$Resources:Controls,Cancel%>" OnClientClick="return ShowDeleteConfirm(this,'Are you sure want to cancel this EMI.?');" />
                                                <asp:ImageButton runat="server" ID="imbPrint" ToolTip="<%$Resources:Controls,Print %>"
                                                    SkinID="btnPrint" CommandName="PRINT" />
                                            </ItemTemplate>
                                            <ItemStyle Width="110px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <div id="divEMI">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblMaterialIssueNo" Text="<%$ resources:MaterialIssueNo%>"
                                                    AssociatedControlID="lblMaterilaConsumptionNo"></asp:Label>
                                                <asp:Label ID="lblMaterilaConsumptionNo" runat="server" Text="" CssClass="input-small"
                                                    TabIndex="15"></asp:Label>
                                                <asp:Label runat="server" ID="lblDate" Text="<%$ resources:Date%>" AssociatedControlID="txtDate"
                                                    class="middle-lbl-c"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDate" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                    MaxLength="11" onpaste="return false;" TabIndex="16"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDate"
                                                    Display="Static" CssClass="star" ValidationGroup="EMIMultipleSave" Text="*" ErrorMessage="<%$ resources:Err_Date%>"></asp:RequiredFieldValidator>
                                                <%--<asp:RequiredFieldValidator ID="rfvtxtDateAdd" runat="server" ControlToValidate="txtDate"
                                                Display="Static" CssClass="star" ValidationGroup="EMIMultipleSave" Text="*" ErrorMessage="<%$ resources:Err_Date%>"></asp:RequiredFieldValidator>--%>
                                                <asp:Label runat="server" ID="lblReference" Text="<%$ resources:Reference%>" AssociatedControlID="txtReference"></asp:Label>
                                                <asp:TextBox ID="txtReference" runat="server" CssClass="input-half" MaxLength="100"
                                                    TabIndex="18" onpaste="limitText(this,100);" onkeyup="limitText(this,100);" onkeydown="limitText(this,100);"></asp:TextBox>
                                                <%-- <asp:RequiredFieldValidator ID="rfvName" runat="server" Text="*" ErrorMessage="<%$ resources:Err_EnterName%>"
                                                    CssClass="star" ControlToValidate="txtReference" ValidationGroup="EMIMultipleSave">
                                                </asp:RequiredFieldValidator>--%>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblInvTypeCode" Text="<%$ resources:Code%>" AssociatedControlID="txtInvTypeCode"
                                                    Visible="false"></asp:Label>
                                                <asp:TextBox ID="txtInvTypeCode" runat="server" CssClass="input-medium" Visible="false"></asp:TextBox>
                                                <asp:Label runat="server" ID="lblIssueStore" Text="<%$ resources:IssuingStore%>"
                                                    AssociatedControlID="ddlIssuingStore"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlIssuingStore" CssClass="select-half" TabIndex="17">
                                                </asp:DropDownList>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label runat="server" ID="lblInvTypeDescription" Text="<%$ resources:Description%>"
                                                    AssociatedControlID="txtInvTypeDescription" Visible="false"></asp:Label>
                                                <asp:TextBox ID="txtInvTypeDescription" runat="server" MaxLength="450" TextMode="MultiLine"
                                                    Height="40" CssClass="input-full" Visible="false"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblActive" Text="<%$ resources:Active%>" AssociatedControlID="chkActive"
                                                    Visible="false"></asp:Label>
                                                <asp:CheckBox runat="server" ID="chkActive" Checked="true" Visible="false" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <h3>
                                Item Details</h3>
                            <div class="w100perc div2col-S border1">
                                <div class="w34perc float-left margnrgt12 padgtop10 lft-panel">
                                    <asp:Button ID="btnSelectItemStock" runat="server" OnClick="ActionHandler" CommandName="ITEMSTOCK"
                                        EnableTheming="false" Style="display: none" />
                                    <asp:Button ID="btnAssetPostback" runat="server" OnClick="ActionHandler" CommandName="ASSETDETAILS"
                                        EnableTheming="false" Style="display: none" />
                                    <asp:Button ID="btnSelectItemType" runat="server" OnClick="ActionHandler" CommandName="TYPE"
                                        EnableTheming="false" Style="display: none" />
                                    <asp:Button ID="btnSelectItemName" runat="server" OnClick="ActionHandler" CommandName="ITEMNAME"
                                        EnableTheming="false" Style="display: none" />
                                    <asp:Button ID="btnSelectItemTypeFilter" runat="server" OnClick="ActionHandler" CommandName="TYPEFILTER"
                                        EnableTheming="false" Style="display: none" />
                                    <asp:Button ID="btnSelectCurrentStock" runat="server" OnClick="ActionHandler" CommandName="GETCURRENTSTOCK"
                                        EnableTheming="false" Style="display: none" />
                                    <asp:Label runat="server" ID="lblItemCategory" Text="<%$ resources:ItemCategory%>"
                                        AssociatedControlID="txtItemCategory" CssClass="lbl-36-5perc"></asp:Label>
                                    <asp:TextBox ID="txtItemCategory" runat="server" CssClass="input-w55per" TabIndex="19"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvItemCategory" CssClass="star" SetFocusOnError="true"
                                        runat="server" ControlToValidate="txtItemCategory" Display="Dynamic" Text="*"
                                        ValidationGroup="EMIMultipleAdd" ErrorMessage="<%$ resources:Err_EnterItemCategory %>"
                                        InitialValue="<%$resources:ErpRes,AutoDefaultValue %>">
                                    </asp:RequiredFieldValidator>
                                    <asp:HiddenField ID="hdfItemCategory" runat="server" />
                                    <asp:Label runat="server" ID="lblItemCode" Text="<%$ resources:ItemCode%>" AssociatedControlID="txtItemCode"
                                        CssClass="lbl-36-5perc"></asp:Label>
                                    <asp:TextBox ID="txtItemCode" runat="server" CssClass="input-w55per" TabIndex="20"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvItemCode" CssClass="star" SetFocusOnError="true"
                                        runat="server" ControlToValidate="txtItemCode" Display="Dynamic" Text="*" ValidationGroup="EMIMultipleAdd"
                                        ErrorMessage="<%$ resources:Err_EnterItemCode %>" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>">
                                    </asp:RequiredFieldValidator>
                                    <asp:HiddenField ID="hdfItemCode" runat="server" Value="0" />
                                    <asp:HiddenField ID="IsEdit" runat="server" Value="false" />
                                    <asp:HiddenField ID="hdnICD_CRDR_NOTE_DTL" runat="server" Value="0"></asp:HiddenField>
                                    <div id="divBatchNo" runat="server">
                                        <asp:Label runat="server" ID="lblBatchNo" Text="<%$ resources:BatchNo%>" AssociatedControlID="txtBatchNo"
                                            CssClass="margnrgt-minus1 lbl-36-5perc"></asp:Label>
                                        <asp:TextBox ID="txtBatchNo" runat="server" CssClass="input-w55per" TabIndex="21"></asp:TextBox>
                                        <asp:HiddenField ID="hdfBatchNo" runat="server" />
                                    </div>
                                    <asp:Label runat="server" ID="lblStock" Text="<%$ resources:Stock%>" AssociatedControlID="txtStockValue"
                                        CssClass="lbl-36-5perc"></asp:Label>
                                    <asp:TextBox ID="txtStockValue" runat="server" CssClass="input-small input-disabled"
                                        TabIndex="22" Enabled="false"></asp:TextBox>
                                    <%-- <asp:RequiredFieldValidator ID="rfvStockValue" runat="server" Text="*" ErrorMessage="<%$ resources:Err_EnterStockValue%>"
                                        CssClass="star" ControlToValidate="txtStockValue" ValidationGroup="EMIMultipleAdd">
                                    </asp:RequiredFieldValidator>--%>
                                    <%-- <asp:Label runat="server" ID="lblUOM" Text="<%$ resources:UOM%>" AssociatedControlID="txtUOM"
                                        CssClass="lbl-17perc"></asp:Label>--%>
                                    <asp:Label runat="server" ID="lblUOM" Text="<%$ resources:UOM%>" AssociatedControlID="txtUOM"
                                        CssClass=" lbl-20perc"></asp:Label>
                                    <asp:TextBox ID="txtUOM" runat="server" CssClass="input-w13-8per" TabIndex="24" Enabled="false"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvUOM" runat="server" Text="*" ErrorMessage="<%$ resources:Err_EnterUOM%>"
                                        CssClass="star" ControlToValidate="txtUOM" ValidationGroup="EMIMultipleAdd">
                                    </asp:RequiredFieldValidator>
                                    <asp:HiddenField ID="hdfUOM" runat="server" Value="0" />
                                    <asp:Label runat="server" ID="lblIssueAgainst" Text="<%$ resources:IssueAgainst%>"
                                        AssociatedControlID="ddlIssueAgainst" CssClass="lbl-36-5perc"></asp:Label>
                                    <asp:DropDownList runat="server" ID="ddlIssueAgainst" CssClass="w56-7perc" onchange="javascript:BindTypeDDL();"
                                        TabIndex="23">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvIssueAgainst" runat="server" Text="*" ErrorMessage="<%$ resources:Err_IssueAgainst%>"
                                        CssClass="star" ControlToValidate="ddlIssueAgainst" ValidationGroup="EMIMultipleAdd"
                                        InitialValue="-1">
                                    </asp:RequiredFieldValidator>
                                    <div class="clear">
                                    </div>
                                    <asp:Label runat="server" ID="lblType" Text="<%$ resources:Type%>" AssociatedControlID="ddlType"
                                        CssClass="lbl-36-5perc"></asp:Label>
                                    <asp:DropDownList runat="server" ID="ddlType" CssClass="w56-7perc" onchange="javascript:BindItemName();"
                                        TabIndex="25">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvType" runat="server" Text="*" ErrorMessage="<%$ resources:Err_Type%>"
                                        CssClass="star" ControlToValidate="ddlType" ValidationGroup="EMIMultipleAdd"
                                        InitialValue="-1">
                                    </asp:RequiredFieldValidator>
                                    <div class="clear">
                                    </div>
                                    <asp:Label runat="server" ID="lblItemName" Text="<%$ resources:ItemNameAsset%>" AssociatedControlID="txtItemName"
                                        CssClass="lbl-36-5perc"></asp:Label>
                                    <asp:TextBox ID="txtItemName" runat="server" CssClass="input-w55per" TabIndex="26"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvItemName" CssClass="star" SetFocusOnError="true"
                                        runat="server" ControlToValidate="txtItemName" Display="Dynamic" Text="*" ValidationGroup="EMIMultipleAdd"
                                        ErrorMessage="<%$ resources:Err_EnterItemName %>" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>">
                                    </asp:RequiredFieldValidator>
                                    <asp:HiddenField ID="hdfItemName" runat="server" />
                                    <asp:Label runat="server" ID="lblQtyIssued" Text="<%$ resources:QtyIssued%>" AssociatedControlID="txtQtyIssued"
                                        CssClass="lbl-36-5perc"></asp:Label>
                                    <%--<asp:TextBox ID="TextBox1" runat="server" CssClass="input-small numeric" TabIndex="27" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"></asp:TextBox>--%>
                                    <asp:TextBox ID="txtQtyIssued" runat="server" CssClass="input-small" TabIndex="27"
                                        MaxLength="14"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvQtyIssued" runat="server" Text="*" ErrorMessage="<%$ resources:Err_QtyIssued%>"
                                        CssClass="star" ControlToValidate="txtQtyIssued" ValidationGroup="EMIMultipleAdd">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="vreQuantity" runat="server" ControlToValidate="txtQtyIssued"
                                        ErrorMessage="<%$ resources:Err_Quantity_Valid %>" ValidationExpression="^\$?([0-9]{0,14})?(\.[0-9]{0,3})?$"
                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="EMIMultipleAdd">
                                    </asp:RegularExpressionValidator>
                                    <div class="clear">
                                    </div>
                                    <asp:Label runat="server" ID="lblComments" Text="<%$ resources:Comments%>" AssociatedControlID="txtComments"
                                        CssClass="lbl-36-5perc"></asp:Label>
                                    <asp:TextBox ID="txtComments" runat="server" CssClass="input-w55per h80" TabIndex="28"
                                        MaxLength="500" onpaste="limitText(this,500);" TextMode="MultiLine" onkeyup="limitText(this,500);"
                                        onkeydown="limitText(this,500);"></asp:TextBox>
                                    <asp:ImageButton ID="imbAddNew" runat="server" SkinID="imbaddnew" OnClick="ActionHandler"
                                        CommandName="ADD" Width="16px" TabIndex="29" CssClass="margntop3 margn-rgt0 margnbotm0"
                                        OnClientClick="javascript:ValidatePageNow('EMIMultipleAdd')" ValidationGroup="EMIMultipleAdd" />
                                </div>
                                <div class="gridwrap w64perc scroll-h350 padgtop10">
                                    <%--class="grdTable"--%>
                                    <asp:GridView runat="server" ID="grdEMIMultipleList" Width="100%" AllowPaging="false"
                                        AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                        EmptyDataRowStyle-CssClass="emptytable" Style="margin-top: 0px!important;">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:SLNO%>  " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSerialNo" runat="server" Text='<%# Eval("ROW_NO")%>' AssociatedControlID="lblSerialNo"
                                                        ToolTip='<%# Eval("ROW_NO") %>' CssClass="margnbotm0"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="39px" Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:ItmCode%>  " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdfEMIItemPk" Value='<%# Eval("ICD_PK")%>' runat="server" />
                                                    <asp:Label ID="lblgrdItemCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ICD_ITEM_TEXT")),25) %>'
                                                        AssociatedControlID="lblgrdItemCode" ToolTip='<%# Eval("ICD_ITEM_TEXT") %>' CssClass="margnbotm0"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="140px" Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:ItemNameList%>  " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgrdItemName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ICD_ISS_RCV_NAME")),35) %>'
                                                        AssociatedControlID="lblgrdItemName" ToolTip='<%# Eval("ICD_ISS_RCV_NAME") %>'
                                                        CssClass="margnbotm0"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="200px" Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Quantity%>  " SortExpression="" HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgrdQuantity" runat="server" Text='<%# Eval("ICD_QTY_CONSUMED",this.GetCurrencyFormat()) %>'
                                                        AssociatedControlID="lblgrdQuantity" ToolTip='<%# Eval("ICD_QTY_CONSUMED",this.GetCurrencyFormat()) %>'
                                                        CssClass="minw31-maxw125 margn-rgt0 margnbotm0"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" Wrap="false" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:UOMList%>  " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgrdUOM" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ICD_UOM_TEXT")),8) %>'
                                                        AssociatedControlID="lblgrdItemName" ToolTip='<%# Eval("ICD_UOM_TEXT") %>' CssClass="margnbotm0"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" Wrap="false" HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:ImageButton Width="16px" Height="16px" CssClass="_edit" runat="server" ID="imbEditDetails"
                                                        SkinID="imbeditgrid" OnClick="ActionHandler" CommandName="EDIT_ACTION" ToolTip="<%$resources:Controls,Edit %>"
                                                        CommandArgument='<%# Eval("ROW_NO")%>' />
                                                    <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbDeleteDetails"
                                                        SkinID="imbdeletegrid" ToolTip="<%$resources:Controls,Delete %>" CommandName="DELETE_ACTION"
                                                        CommandArgument='<%# Eval("ROW_NO")%>' OnClick="ActionHandler" />
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" Wrap="true" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <asp:ValidationSummary ID="vsSave" ValidationGroup="EMIMultipleSave" runat="server" />
                <asp:ValidationSummary ID="vsAddToList" ValidationGroup="EMIMultipleAdd" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
            </div>
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummySaveSubmit" CommandName="WRKFSUBMIT" Text=""
                    OnClick="ActionHandler" SkinID="btnInner-submit" />
            </div>
            <asp:HiddenField ID="hdfIscontYes" runat="server" />
            <asp:HiddenField ID="hdnItmNeedBatchStk" runat="server" Value="0" />
            <asp:HiddenField ID="hdfEnableBatch" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCurRowIndex" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsMultiplePO" runat="server" />
            <asp:HiddenField ID="hdfSelectedItemPRPK" runat="server" Value="0" />
            <asp:HiddenField ID="hdfServicePORequired" runat="server" Value="1" />
            <asp:HiddenField ID="hdfDeptID" runat="server" Value="0" />
            <asp:HiddenField ID="hdfAppType" runat="server" />
            <asp:HiddenField ID="hdfAppSubType" runat="server" />
            <asp:HiddenField ID="hdnClosePO" runat="server" Value="0" />
            <asp:HiddenField ID="hdnModifyEMI" runat="server" Value="0" />
            <asp:HiddenField ID="hdnCancelEMI" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
            <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
            <asp:HiddenField ID="hdfShowTransactionTypeFilter" runat="server" Value="0" />
            <asp:HiddenField ID="hdfDecimalFormatWithSeperation" runat="server" />
            <asp:HiddenField ID="ICH_IS_EDIT" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsEdit" runat="server" Value="0" />
            <asp:HiddenField ID="hdfAdvancedSearchType" runat="server" Value="0" />
            <asp:HiddenField ID="hdfEditQtyIssued" runat="server" Value="0" />
            <asp:HiddenField ID="hdfBatchNoCheck" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsModify" runat="server" Value="0" />
            <asp:HiddenField ID="hdfAddCommentMandValidation" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
