<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BulkAppraisal.aspx.cs"
    Inherits="HRMS.Payroll.SalaryAppraisal" Theme="ClassicExt" MasterPageFile="~/ERPSMS_2.Master"
    Title="<%$ Resources:Captions,Title_BulkAppraisal %>" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtDepartment", url, "hdfDepartment", true, true, "DEPARTMENTAUTOCOMPLETE");
            GrandScriptUtils.MakeAutoCompleteDDL("txtDesignation", url, "hdfDesignation", true, true, "DESIGNATION");
            ShowHideEmployeeDetails(1);
            GrandScriptUtils.AddDateRangeCommon("txtSrchFromDate", "hdfSrchFromDate", "txtSrchToDate", "hdfSrchToDate", false, false);
            GrandScriptUtils.DatePickerCommon("txtDate");
            GrandScriptUtils.DatePickerCommon("txtEffectFrom");
            GrandScriptUtils.DatePickerCommon("txtRefDate");
            GrandScriptUtils.DatePickerCommon("txtEmpDoj");
            GrandScriptUtils.DatePickerCommon("txtLastAppDate");
            $("[id*=txtValue]").ForceNumericOnly();
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();

            //Set a stamp for cancelled invoice
            if ($("[id$=hdfIsCancelled]").val() == "1") {
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
                $("[id$='pnlSave']").hide();
            }
            else
                $("[id$=tblDetailHdr]").addClass("table-devide");
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


        function ShowHideEmployeeDetails(flag) {
            if (flag == 1) {
                $("[id$=divEmployeeDetails").show();
                $("[id$=imbShowEmployeeDetails").hide();
                $("[id$=imbHideEmployeeDetails").show();
            }
            else {
                $("[id$=divEmployeeDetails").hide();
                $("[id$=imbShowEmployeeDetails").show();
                $("[id$=imbHideEmployeeDetails").hide();
            }
            return false;
        }

        function ShowHidePayDetails(flag) {
            if (flag == 1) {
                $("[id$=divEmployeePayElemtDetails").show();
                $("[id$=imbShowPayDetails").hide();
                $("[id$=imbHidePayDetails").show();
            }
            else {
                $("[id$=divEmployeePayElemtDetails").hide();
                $("[id$=imbShowPayDetails").show();
                $("[id$=imbHidePayDetails").hide();
            }
            return false;
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

        function ViewMode(mode) {
            ///<summary>
            /// Used to handle the view Mode
            ///</summary>
            /// <param name="mode" optional="true" type="String">
            /// Mode = 1 Determins ites on View Mode
            /// Mode = 2 Indicates its on New Mode
            /// Mode = 3 Indicates its on Edit Mode
            /// </param>         
            if (mode == 1) {
                $("[id$='pnlSave']").hide();
                $("[id$='pnlDelete']").hide();
                $("[id$='btnSaveSubmit']").hide();
                $("[id$='btnSubmit']").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();

            }
        }
        var validationArrayGroup;
        function CheckValidationDuplicate(valGroup) {
            validationArrayGroup = new Array();
            for (var i = Page_Validators.length - 1; i >= 0; i--) {
                if (typeof (Page_Validators[i].validationGroup) == "string") {
                    if (valGroup == Page_Validators[i].validationGroup) {
                        if (!CheckValidationExists(Page_Validators[i].id)) {
                            validationArrayGroup.push(Page_Validators[i].id);
                        }
                        else {
                            Page_Validators.splice(i, 1);
                        }
                    }
                }
            }
        }
        function CheckValidationExists(id) {
            for (var i in validationArrayGroup) {
                if (validationArrayGroup[i] == id) {
                    return true;
                }
            }
            return false;
        }
        function ValidateAppPage(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                CheckValidationDuplicate(valGroup);
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
                return false;
            }
            else {
                return true;
            }
        }

        $("[id*=chkEmpHeader]").live("click", function () {
            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (chkHeader.is(":checked")) {
                    $(this).attr("checked", "checked");
                    $("td", $(this).closest("tr")).addClass("selected");
                } else {
                    if ($(this).is(':disabled') == false) {
                        $(this).removeAttr("checked");
                        $("td", $(this).closest("tr")).removeClass("selected");
                    }
                }


            });
        });

        $("[id*=chkEmpselect]").live("click", function () {
            var grid = $(this).closest("table");
            var chkHeader = $("[id*=chkEmpHeader]", grid);
            if (!$(this).is(":checked")) {
                $("td", $(this).closest("tr")).removeClass("selected");
                chkHeader.removeAttr("checked");
            } else {
                $("td", $(this).closest("tr")).addClass("selected");
                if ($("[id*=chkEmpselect]", grid).length == $("[id*=chkEmpselect]:checked", grid).length) {
                    chkHeader.attr("checked", "checked");
                }
            }
        });

        function SaveConfirmationMsg(msg) {
            var msgTitle;
            //var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            //msg = '<%= GetLocalResourceObject("MsgSaveConfirm").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 300,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfIsSaveContYes]").val(1);
                        $(this).dialog("close");
                        if ($("[id$=hdfSaveSubmitYes]").val() > 0) {
                            $("[id$=btnSaveSubmit]").click();
                        }
                        else {
                            $("[id$=btnSave]").click();
                        }

                    },
                    Cancel: function (e) {
                        $("[id$=hdfIsSaveContYes]").val(0);
                        $(this).dialog("close");
                        ClosePopup();
                        return false;
                    }
                }
            });
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upAdditionDedction" runat="server">
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
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="147"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="148"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidateAppPage('Save')"
                                            ValidationGroup="Save" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="149" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateAppPage('Save')" ValidationGroup="Save"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$resources:Controls,Save %>"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save"
                                            ValidationGroup="Save" OnClick="ActionHandler" OnClientClick="javascript:ValidateAppPage('Save')"
                                            TabIndex="150" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);"
                                            TabIndex="151" />
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
                                        <asp:Button runat="server" TabIndex="152" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="153" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelAppraisal %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelAppraisal %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="154" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="155" ID="btnView" CommandName="VIEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,View %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
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
                <asp:Table runat="server" ID="tblPage" CssClass="tablelayout asptbllinks">
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
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:ShowFilter%>" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <%--<table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7 ">
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7 ">
                                        </div>
                                    </td>
                                </tr>
                            </table>--%>
                            <table class="table-devide" id="tbladvancedSearch">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblSrchFromDate" runat="server" Text="<%$ resources:FromDate%>" AssociatedControlID="txtSrchFromDate"></asp:Label>
                                            <asp:TextBox ID="txtSrchFromDate" runat="server" CssClass="input-small margnbotm0"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" TabIndex="1"></asp:TextBox>
                                            <asp:HiddenField ID="hdfSrchFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblSrchToDate" runat="server" Text="<%$ resources:ToDate%>" AssociatedControlID="txtSrchToDate"
                                                CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:TextBox ID="txtSrchToDate" runat="server" CssClass="input-small margnbotm0"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" TabIndex="1"></asp:TextBox>
                                            <asp:HiddenField ID="hdfSrchToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label runat="server" ID="lblSrchType" Text="<%$ resources:Type%>" AssociatedControlID="ddlSrchType"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlSrchType" CssClass="select-small-a1 margnbotm0"
                                                TabIndex="2">
                                            </asp:DropDownList>
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" AssociatedControlID="ddlStatus"
                                                CssClass="middle-lbl-small-d margnbotm0 "></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-a1 margnbotm0"
                                                TabIndex="2">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Type%>" ToolTip="<%$ resources:Type%>"
                                                OnClick="ActionHandler" CommandName="SEARCH" SkinID="search-ext" CssClass="margntop2 margnbotm0"
                                                ValidationGroup="Search" TabIndex="2" />
                                            <asp:ImageButton ID="imgClearSearch" runat="server" Text="<%$ resources:Type%>" ToolTip="<%$ resources:Type%>"
                                                OnClick="ActionHandler" CommandName="CLEARSEARCH" SkinID="clear-ext" CssClass="margntop2 margnbotm0"
                                                TabIndex="2" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdEmpAppList" Width="100%" AllowPaging="false"
                                    AllowSorting="false" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" TabIndex="3"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfEBH_PK" Value='<%# Eval("EBH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfDept" Value='<%# Eval("EBH_DEPT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfDelStatus" Value='<%# Eval("EBH_DEL_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval("EBH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TrxNo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGdTrxNo" runat="server" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("EBH_NO")))?Resources.ErpRes.Draft:Eval("EBH_NO")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("EBH_NO")))?Resources.ErpRes.Draft:Eval("EBH_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGdDate" runat="server" Text='<%#Eval("EBH_DATE", Resources.Constants.HRMSDateFormatGrid) %>'
                                                    ToolTip='<%# Eval("EBH_DATE", Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                            <HeaderStyle Width="10%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Type%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGdType" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("EBH_TYPE_TEXT"),25) %>'
                                                    ToolTip='<%# Eval("EBH_TYPE_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:EffFrom%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGdEffFrom" runat="server" Text='<%#Eval("EBH_EFFECT_DATE", Resources.Constants.HRMSDateFormatGrid) %>'
                                                    ToolTip='<%# Eval("EBH_EFFECT_DATE", Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:REMARKS%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGdRemarks" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EBH_REMARKS"))),90) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EBH_REMARKS"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="50%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgStatus" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("EBH_CSS_CLASS") %>' ToolTip='<%# Eval("EBH_STATUS_TEXT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc2:PagerControl ID="uclPaging" runat="server" TabIndex="3" />
                                <div class="clear">
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <table class="table-devide tablelayout" id="tblDetailHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblTrxNoHdr" runat="server" Text="<%$ resources:TrxNo%>" AssociatedControlID="lblTrxNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblTrxNo" CssClass="input-small"></asp:Label>
                                            <asp:Label ID="lblDate" runat="server" Text="<%$ resources:DateStar%>" AssociatedControlID="txtDate"
                                                CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:TextBox ID="txtDate" runat="server" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;" TabIndex="4"></asp:TextBox>
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="rfvDate" CssClass="star" SetFocusOnError="true" ValidationGroup="Save"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtDate" Display="Dynamic"
                                                    Text="*" ErrorMessage="<%$ resources:Err_Date %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblType" Text="<%$ resources:TypeStar%>" AssociatedControlID="ddlType"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlType" CssClass="select-small-a2" TabIndex="5">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator runat="server" ID="vrfType" ControlToValidate="ddlType"
                                                Text="*" ErrorMessage="<%$ Resources:Msg_SelectType %>" InitialValue="-1" CssClass="star"
                                                Display="Static" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            <asp:Label ID="lblEffectFrom" runat="server" Text="<%$ resources:EffFromStar%>" AssociatedControlID="txtEffectFrom"
                                                CssClass="middle-lbl-small-b"></asp:Label>
                                            <asp:TextBox ID="txtEffectFrom" runat="server" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;" TabIndex="5"></asp:TextBox>
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="rfvEffectFrom" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="Save" EnableClientScript="true" runat="server" ControlToValidate="txtEffectFrom"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_DateEffFrom %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblRefNo" Text="<%$ resources:RefNo%>" AssociatedControlID="txtRefNo"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRefNo" CssClass="input-small" MaxLength="20" TabIndex="6"></asp:TextBox>
                                            <asp:Label runat="server" ID="lblRefDate" Text="<%$ resources:RefDate%>" AssociatedControlID="txtRefDate"
                                                CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRefDate" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;" TabIndex="6"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblRemarks" runat="server" Text="<%$ resources:Remarks%>" AssociatedControlID="txtRemarks"
                                                CssClass="lbl-25-1perc"></asp:Label>
                                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="input-half" TabIndex="6"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblComments" runat="server" Text="<%$ resources:Comments%>" AssociatedControlID="txtComments"></asp:Label>
                                            <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" CssClass="multiline-2a-line input-full"
                                                MaxLength="500" TabIndex="7"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    <asp:Literal ID="ltrEmployeeFilter" runat="server" Text="<%$ resources: EmployeeDetails %>" /></h1>
                                <asp:ImageButton runat="server" ID="imbShowEmployeeDetails" OnClientClick="javascript:return ShowHideEmployeeDetails(1);"
                                    SkinID="imbArrowShow" ToolTip="<%$ resources:Controls,ShowDetails%>" />
                                <asp:ImageButton runat="server" ID="imbHideEmployeeDetails" OnClientClick="javascript:return ShowHideEmployeeDetails();"
                                    Style="display: none" SkinID="imbArrowHide" ToolTip="<%$ resources:Controls,HideDetails%>" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divEmployeeDetails">
                                <table class="table-devide tablelayout">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblDepartment" runat="server" Text="<%$ resources:Department%>" AssociatedControlID="txtDepartment"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDepartment" Text="" TabIndex="8" CssClass="input-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfDepartment" Value="-1" runat="server" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblDesignation" runat="server" Text="<%$ resources:Designation%>"
                                                    AssociatedControlID="txtDesignation"></asp:Label>
                                                <asp:TextBox runat="server" TabIndex="8" ID="txtDesignation" CssClass="input-half" />
                                                <asp:HiddenField ID="hdfDesignation" runat="server" Value="-1" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblEmpDoj" runat="server" Text="<%$ resources:DOJ%>" AssociatedControlID="txtEmpDoj"></asp:Label>
                                                <asp:TextBox ID="txtEmpDoj" runat="server" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                    MaxLength="11" onpaste="return false;" TabIndex="8"></asp:TextBox>
                                                <asp:Label ID="lblLastAppDate" runat="server" Text="<%$ resources:LastAppDate%>"
                                                    AssociatedControlID="txtLastAppDate" CssClass="middle-lbl-small-d"></asp:Label>
                                                <asp:TextBox ID="txtLastAppDate" runat="server" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                    MaxLength="11" onpaste="return false;" TabIndex="8"></asp:TextBox>
                                                <asp:ImageButton ID="btnFilterSearch" runat="server" Text="<%$ resources:Controls,Search%>"
                                                    ToolTip="<%$ resources:Controls,Search%>" OnClick="ActionHandler" TabIndex="8"
                                                    CommandName="EMPSEARCH" SkinID="search-ext" CssClass="margntop2 margnbotm0 margn-rgt4" />
                                                <asp:ImageButton ID="btnClearDetails" runat="server" Text="<%$ resources:Controls,Clear%>"
                                                    ToolTip="<%$ resources:Controls,Clear%>" TabIndex="8" OnClick="ActionHandler"
                                                    CommandName="CLEARDETAIL" SkinID="clear-ext" CssClass="margntop2 margnbotm0 margn-rgt4" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="gridwrap scroll-h350">
                                    <%--class="grdTable"--%>
                                    <asp:GridView runat="server" ID="grdEmpList" Width="100%" AllowPaging="false" AllowSorting="True"
                                        AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-CssClass="emptytable"
                                        OnRowCommand="ActionHandler" OnRowDataBound="ActionHandler">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkEmpHeader" runat="server" ToolTip="Select All Employee" TabIndex="9" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox CssClass="checkbox" runat="server" ID="chkEmpselect" TabIndex="8" Checked='<%# (Convert.ToInt32(Eval("BED_PK")) > 0) ? true : false %>' />
                                                    <asp:HiddenField runat="server" ID="hdfBED_PK" Value='<%# Eval("BED_PK") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfBED_EBH_PK" Value='<%# Eval("BED_EBH_PK") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="2%" Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Employee%> " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:HiddenField runat="server" ID="hdfBED_EMPLOYEE" Value='<%# Eval("BED_EMPLOYEE") %>' />
                                                    <asp:Label ID="lblGdEmployeeText" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("BED_EMPLOYEE_TEXT"),35) %>'
                                                        ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("BED_EMPLOYEE_TEXT"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="20%" />
                                                <ItemStyle Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:DOJ%> " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGdDoj" runat="server" Text='<%#Eval("BED_EMP_DOJ", Resources.Constants.HRMSDateFormatGrid) %>'
                                                        ToolTip='<%# Eval("BED_EMP_DOJ", Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Wrap="false" />
                                                <HeaderStyle Width="7%" Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:LastAppDate%> " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGdLastAppDate" runat="server" Text='<%#Eval("BED_EMP_PREV_APPR_DATE", Resources.Constants.HRMSDateFormatGrid) %>'
                                                        ToolTip='<%# Eval("BED_EMP_PREV_APPR_DATE", Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Wrap="false" />
                                                <HeaderStyle Width="7%" Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Department%> " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:HiddenField runat="server" ID="hdfBED_EMP_DEPT" Value='<%# Eval("BED_EMP_DEPT") %>' />
                                                    <asp:Label ID="lblGdDepartment" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("BED_EMP_DEPT_TEXT"),50) %>'
                                                        ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("BED_EMP_DEPT_TEXT"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="27%" />
                                                <ItemStyle Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Designation%> " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:HiddenField runat="server" ID="hdfBED_EMP_DESGN" Value='<%# Eval("BED_EMP_DESGN") %>' />
                                                    <asp:Label ID="lblGdDesignation" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("BED_EMP_DESGN_TEXT"),50) %>'
                                                        ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("BED_EMP_DESGN_TEXT"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="27%" />
                                                <ItemStyle Wrap="false" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    <asp:Literal ID="ltrEmployeePayFilter" runat="server" Text="<%$ resources: Salary %>" /></h1>
                                <asp:ImageButton runat="server" ID="imbShowPayDetails" OnClientClick="javascript:return ShowHidePayDetails(1);"
                                    SkinID="imbArrowShow" ToolTip="<%$ resources:Controls,ShowDetails%>" TabIndex="10" />
                                <asp:ImageButton runat="server" ID="imbHidePayDetails" OnClientClick="javascript:return ShowHidePayDetails();"
                                    Style="display: none" SkinID="imbArrowHide" ToolTip="<%$ resources:Controls,HideDetails%>"
                                    TabIndex="10" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divEmployeePayElemtDetails">
                                <table class="table-devide tablelayout">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblPayElement" runat="server" Text="<%$ resources:ElementName%>" AssociatedControlID="txtPayElementText"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtPayElementText" Text="" TabIndex="11" CssClass="input-medium"></asp:TextBox>
                                                <asp:Label ID="Label2" runat="server" Text="<%$ resources:ApplyAs%>" AssociatedControlID="ddlApplyAs"
                                                    CssClass="middle-lbl-xsmall-a"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlApplyAs" CssClass="select-small" TabIndex="11"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                                </asp:DropDownList>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblIncrDecr" runat="server" Text="<%$ resources:IncrDecr%>" AssociatedControlID="ddlIncrDecr"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlIncrDecr" CssClass="select-small" TabIndex="12">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblValue" runat="server" Text="<%$ resources:Value%>" AssociatedControlID="txtValue"
                                                    CssClass="lbl-28-7perc"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtValue" Text="" TabIndex="12" CssClass="input-small"></asp:TextBox>
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="rfvtxtValue" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="AddToList" EnableClientScript="true" runat="server" ControlToValidate="txtValue"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Value %>">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                                <asp:ImageButton ID="imgAddNewDetails" runat="server" CommandName="ADD" OnClick="ActionHandler"
                                                    ValidationGroup="AddToList" SkinID="imbaddnew" OnClientClick="javascript:ValidateAppPage('AddToList')"
                                                    TabIndex="12" CssClass="margntop2 margnbotm0" />
                                                <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear%>"
                                                    ToolTip="<%$ resources:Controls,Clear%>" OnClick="ActionHandler" CommandName="CLEAR"
                                                    SkinID="clear-ext" TabIndex="12" CssClass="margntop2 margnbotm0" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="gridwrap">
                                    <div class="gridwrap floatLeft" style="width: 49%;">
                                        <div class="split-head">
                                            <h4 class="check-inline">
                                                <%= GetGlobalResourceObject("Controls", "Earnings").ToString()%></h4>
                                            <asp:Button runat="server" ID="btnAddEmpEarnings" CommandName="ADDNEWEARNINGS" TabIndex="13"
                                                OnClick="ActionHandler" ToolTip="<%$ resources:Controls,New %>" ValidationGroup="upload"
                                                Text="<%$ resources:Controls,New %>" SkinID="btnInner-add" Style="float: right;
                                                margin-right: 0px; margin-bottom: 2px" />
                                        </div>
                                        <div class="clear">
                                        </div>
                                        <div id="divEarnings" class="scroll-h375">
                                            <asp:GridView ID="grdEmpEarnings" runat="server" AutoGenerateColumns="False" Width="100%"
                                                AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                                ShowFooter="true" OnRowCommand="ActionHandler" OnRowDataBound="ActionHandler">
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ resources:Controls,Earnings%> " SortExpression="">
                                                        <ItemTemplate>
                                                            <asp:HiddenField runat="server" ID="hdfPelPkErn" Value='<%# Eval("EBD_PAY_ELEMENT") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfEBDPkErn" Value='<%# Eval("EBD_PK") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfEBHPkErn" Value='<%# Eval("EBD_EBH_PK") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfPelCalcModeErn" Value='<%# Eval("EBD_CALC_MODE") %>' />
                                                            <asp:HiddenField ID="hdfEarnPayElmtInSalary" runat="server" Value='<%#Eval("PEL_IN_SALARY")%>' />
                                                            <asp:HiddenField ID="hdfEarnSlNo" runat="server" Value='<%#Eval("STS_SL_NO")%>' />
                                                            <asp:HiddenField ID="hdfPelDedc" runat="server" Value='<%#Eval("PEL_IS_DEDUCTION")%>' />
                                                            <asp:HiddenField ID="hdfPEL_IN_SALARY" runat="server" Value='<%#Eval("PEL_IN_SALARY")%>' />
                                                            <asp:HiddenField ID="HdfPayElmntNewErn" runat="server" Value='<%#Eval("EBD_IS_NEW")%>' />
                                                            <asp:Label ID="lblGdPayElmErn" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("EBD_PAY_ELEMENT_TEXT"),30) %>'
                                                                ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EBD_PAY_ELEMENT_TEXT"))) %>'></asp:Label>
                                                            <asp:DropDownList ID="ddlEarnPayElement" TabIndex="14" runat="server" CssClass="select-full-a custom-select"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ActionHandler" Visible="false">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvEarnPayElement" CssClass="star" SetFocusOnError="true"
                                                                InitialValue="-1" ValidationGroup="Save" EnableClientScript="true" runat="server"
                                                                ControlToValidate="ddlEarnPayElement" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:ErrorMessages,Err_EarningsPayElmt %>">
                                                            </asp:RequiredFieldValidator>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="45%" />
                                                        <ItemStyle Wrap="false" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <div id="divEarnMode" runat="server" class="hide">
                                                            </div>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="5%" CssClass="padgrgt3" />
                                                        <ItemStyle Width="5%" CssClass="padgrgt3" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:ApplyAs%> " SortExpression="">
                                                        <ItemTemplate>
                                                            <asp:HiddenField runat="server" ID="hdfApplyTypeErn" Value='<%# Eval("EBD_TYPE") %>' />
                                                            <asp:Label ID="lblGdApplyAsErn" runat="server" Text='<%#Eval("EBD_TYPE_TEXT") %>'
                                                                ToolTip='<%# Eval("EBD_TYPE_TEXT") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="15%" />
                                                        <ItemStyle Wrap="false" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:IncrDecr%> " SortExpression="">
                                                        <ItemTemplate>
                                                            <asp:HiddenField runat="server" ID="hdfIncDcrErn" Value='<%# Eval("EBD_INC_DECR") %>' />
                                                            <asp:Label ID="lblGdIncrDecrErn" runat="server" Text='<%#Eval("EBD_INC_DECR_TEXT") %>'
                                                                ToolTip='<%# Eval("EBD_INC_DECR_TEXT") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="10%" />
                                                        <ItemStyle Wrap="false" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:Value%> " SortExpression="">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGdValueErn" runat="server" Text='<%#Eval("EBD_VALUE") %>' ToolTip='<%# Eval("EBD_VALUE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="10%" CssClass="txtAlign-right" />
                                                        <ItemStyle Wrap="false" CssClass="txtAlign-right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnEditDetailsErn" runat="server" OnClick="ActionHandler" CommandName="EDIT_ACTION"
                                                                Visible='<%# (Eval("EBD_CALC_MODE").ToString() == "0" ) ? true : false %>' SkinID="edit-icon"
                                                                Style="margin-right: 0px!important;" ToolTip="<%$ resources:Controls,Edit %>" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="8%" />
                                                        <ItemStyle Width="8%" Wrap="false" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                        <HeaderTemplate>
                                                            <asp:Button ID="lnkRemoveErn" runat="server" SkinID="delete-icon" ToolTip="<%$ resources:Controls,Delete %>"
                                                                Enabled="false" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" ID="chkPayElmDeleteErn" ToolTip="<%$ resources:Controls,Delete %>"
                                                                Checked='<%# Convert.ToInt32(Eval("EBD_DEL_STATUS")) > 0 ? true : false %>' TabIndex="15" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="2%" Wrap="false" />
                                                        <ItemStyle Width="2%" Wrap="false" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div style="width: 2%;">
                                    </div>
                                    <div class="gridwrap floatRight" style="width: 49%;">
                                        <div class="split-head">
                                            <h4 class="check-inline">
                                                <%= GetGlobalResourceObject("Controls", "Deductions").ToString()%></h4>
                                            <asp:Button runat="server" ID="btnEmpAddDeduction" CommandName="ADDNEWDEDUCTION"
                                                TabIndex="16" OnClick="ActionHandler" ToolTip="<%$ resources:Controls,New %>"
                                                ValidationGroup="upload" Text="<%$ resources:Controls,New %>" SkinID="btnInner-add"
                                                Style="float: right; margin-right: 0px; margin-bottom: 2px" />
                                        </div>
                                        <div class="clear">
                                        </div>
                                        <div id="divDeductions" class="scroll-h375">
                                            <asp:GridView ID="grdEmpDeductions" runat="server" AutoGenerateColumns="False" Width="100%"
                                                AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                                ShowFooter="true" OnRowCommand="ActionHandler" OnRowDataBound="ActionHandler">
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ resources:Controls,Deductions%> " SortExpression="">
                                                        <ItemTemplate>
                                                            <asp:HiddenField runat="server" ID="hdfPelPkDed" Value='<%# Eval("EBD_PAY_ELEMENT") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfEBD_PKDed" Value='<%# Eval("EBD_PK") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfEBHPkDed" Value='<%# Eval("EBD_EBH_PK") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfPelCalcModeDed" Value='<%# Eval("EBD_CALC_MODE") %>' />
                                                            <asp:HiddenField ID="hdfDedPayElmtInSalary" runat="server" Value='<%#Eval("PEL_IN_SALARY")%>' />
                                                            <asp:HiddenField ID="hdfPelDedc" runat="server" Value='<%#Eval("PEL_IS_DEDUCTION")%>' />
                                                            <asp:HiddenField ID="hdfDeductSlNo" runat="server" Value='<%#Eval("STS_SL_NO")%>' />
                                                            <asp:HiddenField ID="HdfPayElmntNewDed" runat="server" Value='<%#Eval("EBD_IS_NEW")%>' />
                                                            <asp:Label ID="lblGdPayElmDed" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("EBD_PAY_ELEMENT_TEXT"),30) %>'
                                                                ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EBD_PAY_ELEMENT_TEXT"))) %>'></asp:Label>
                                                            <asp:DropDownList ID="ddlDeductPayElement" TabIndex="17" runat="server" CssClass="select-full-a custom-select"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ActionHandler" Visible="false">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvDeductPayElement" CssClass="star" SetFocusOnError="true"
                                                                InitialValue="-1" ValidationGroup="Save" EnableClientScript="true" runat="server"
                                                                ControlToValidate="ddlDeductPayElement" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:ErrorMessages,Err_DeductPayElmt %>">
                                                            </asp:RequiredFieldValidator>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="45%" />
                                                        <ItemStyle Wrap="false" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <div id="divDeductMode" runat="server" class="hide">
                                                            </div>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="5%" CssClass="padgrgt3" />
                                                        <ItemStyle Width="5%" CssClass="padgrgt3" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:ApplyAs%> " SortExpression="">
                                                        <ItemTemplate>
                                                            <asp:HiddenField runat="server" ID="hdfApplyTypeDed" Value='<%# Eval("EBD_TYPE") %>' />
                                                            <asp:Label ID="lblGdApplyAsDed" runat="server" Text='<%#Eval("EBD_TYPE_TEXT") %>'
                                                                ToolTip='<%# Eval("EBD_TYPE_TEXT") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="15%" />
                                                        <ItemStyle Wrap="false" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:IncrDecr%> " SortExpression="">
                                                        <ItemTemplate>
                                                            <asp:HiddenField runat="server" ID="hdfTypIncDecDed" Value='<%# Eval("EBD_INC_DECR") %>' />
                                                            <asp:Label ID="lblGdIncrDecrDed" runat="server" Text='<%#Eval("EBD_INC_DECR_TEXT") %>'
                                                                ToolTip='<%# Eval("EBD_INC_DECR_TEXT") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="10%" />
                                                        <ItemStyle Wrap="false" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:Value%> " SortExpression="">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGdValueDed" runat="server" Text='<%#Eval("EBD_VALUE") %>' ToolTip='<%# Eval("EBD_CALC_MODE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="10%" CssClass="txtAlign-right" />
                                                        <ItemStyle Wrap="false" CssClass="txtAlign-right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnEditDetailsDed" runat="server" OnClick="ActionHandler" CommandName="EDIT_ACTION"
                                                                Visible='<%# (Eval("EBD_CALC_MODE").ToString() == "0" ) ? true : false %>' SkinID="edit-icon"
                                                                Style="margin-right: 0px!important;" ToolTip="<%$ resources:Controls,Edit %>"
                                                                TabIndex="18" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="8%" />
                                                        <ItemStyle Width="8%" Wrap="false" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                        <HeaderTemplate>
                                                            <asp:Button ID="lnkRemoveDed" runat="server" SkinID="delete-icon" ToolTip="<%$ resources:Controls,Delete %>"
                                                                Enabled="false" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" ID="chkPayElmDeleteDed" ToolTip="<%$ resources:Controls,Delete %>"
                                                                Checked='<%# Convert.ToInt32(Eval("EBD_DEL_STATUS")) > 0 ? true : false %>' TabIndex="18" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="2%" Wrap="false" />
                                                        <ItemStyle Width="2%" Wrap="false" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="Save" runat="server" />
                <asp:ValidationSummary ID="vsAddToList" ValidationGroup="AddToList" runat="server" />
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments id="ucrWrkf" runat="server" validationgroup="Save">
                </uc1:WorkflowUserComments>
            </div>
            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormatWithComma" runat="server" />
            <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
            <asp:HiddenField ID="hdfAdvSearch" Value="0" runat="server" />
            <asp:HiddenField ID="hdfCurPelPk" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsCancelled" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIsSaveContYes" runat="server" Value="0" />
            <asp:HiddenField ID="hdfSaveSubmitYes" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
