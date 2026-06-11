<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PushNotification.aspx.cs" Inherits="ERPSMS_v01.GeneralAdmin.PushNotification" 
Title="<%$ Resources:Captions,Title_SendSMS %>"  Theme="ClassicExt" MasterPageFile="~/ERPSMS_2.Master"  %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
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
            /// </param>         
            if (mode == 1) {
                $("[id$='pnlSave']").hide();
                $("[id$='pnlDelete']").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlTaskHome" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <%--Top Buttons "Save", ...--%>
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSEND" CommandName="SEND" TabIndex="150" Text="<%$resources:Controls,Send %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('SEND')" ValidationGroup="SEND"
                                            ToolTip="<%$resources:Controls,SEND %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-smsSend" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="151" SkinID="btnInner-Cancel"
                                            ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <%--<li>
                                        <asp:Button runat="server" TabIndex="152" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>--%>
                                    <li>
                                        <asp:Button runat="server" TabIndex="153" ID="btnResend" CommandName="RESEND" OnClick="ActionHandler"
                                            Text="<%$resources:ReSend %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-smsResend"
                                            ToolTip="<%$resources:ReSend %>" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="154" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <%--Page Datas--%>
                <%--<div class="tab-container-floating">

                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="155" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="156" OnClick="ActionHandler" CommandName="DETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>--%>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server" Style="display: none;">
                        <%--Listing Page Table Row--%>
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
                                                TabIndex="5" />
                                            <%--   ToolTip="<%$ resources:ShowFilter%>"--%>
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>"
                                                TabIndex="5" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
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
                                        <td>
                                            <div class="div2col-S div-separatn">
                                                <asp:Label ID="label6" runat="server" Text="<%$ resources:MObNo%>" AssociatedControlID="txtFilterMobileNo"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtFilterMobileNo" TabIndex="1" CssClass="input-half margnbotm0"></asp:TextBox>
                                                <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                                    ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="1"
                                                    CommandName="FILTER" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                                <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                    ToolTip="<%$resources:Controls,Clear %>" TabIndex="1" OnClick="ActionHandler"
                                                    CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                            </div>
                                        </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdSMSList" Width="100%" AllowPaging="false" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    CssClass="grdTable" EmptyDataRowStyle-CssClass="emptytable" OnSorting="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSMQ_PK_List" runat="server" TabIndex="9" />
                                                <asp:HiddenField runat="server" ID="hdfSMQ_PKList" Value='<%# Eval("SMQ_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfSMQ_STATUS_List" Value='<%# Eval("SMQ_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfLAST_MOD_DATE" Value='<%# Eval("LAST_MOD_DATE") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                            <HeaderStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSMQ_SMS_DATE" runat="server" Text='<%# Eval("SMQ_SMS_DATE", Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval("SMQ_SMS_DATE", Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:MobNo%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMobNo" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("SMQ_TO"),30) %>'
                                                    ToolTip='<%# Eval("SMQ_TO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                            <HeaderStyle Width="15%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Message%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMessage" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("SMQ_MESSAGE"),115) %>'
                                                    ToolTip='<%# Eval("SMQ_MESSAGE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="55%" />
                                            <ItemStyle Width="55%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Status %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("SMQ_STATUS_TEXT"),15) %>'
                                                    ToolTip='<%# Eval("SMQ_STATUS_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" tabindex="12" />
                                <div class="clear">
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <%--<asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <table class="table-devide tablelayout" id="tblDetailHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblMobNo" runat="server" Text="<%$ resources:MobNoStar%>" AssociatedControlID="txtMobNo"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtMobNo" TabIndex="2" CssClass="input-half"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvBonusTypeCode" runat="server" ControlToValidate="txtMobNo"
                                                CssClass="star" ValidationGroup="SEND" Text="*" ErrorMessage="<%$ resources:Err_EnterMobileNo%>"></asp:RequiredFieldValidator>
                                            <asp:ImageButton ID="imgAdd" runat="server" CommandName="ADDTOLIST" SkinID="imbaddnew"
                                                ToolTip="<%$ resources:Controls,AddToList %>" OnClick="ActionHandler" TabIndex="2"
                                                OnClientClick="javascript:ValidateNow('AddToList')" CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblMessage" Text="<%$ resources:Message%>" AssociatedControlID="txtMessage"></asp:Label>
                                            <asp:TextBox ID="txtMessage" runat="server" TabIndex="3" MaxLength="450" TextMode="MultiLine"
                                                Height="40" CssClass="input-full"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>--%>
                </asp:Table>
            </div>
            <%--Employee Filter Section--%>
            <%--<div id="divPopupEmployeeDetails" style="display: none">
                <div class="content-wrapper">
                    <div class="Button-container-popup">
                        <asp:Button ID="btnAddMobNo" SkinID="btnInner-add-dsd" runat="server" Text="<%$resources:Controls,Add_Add %>"
                            CommandName="ADD" OnClick="ActionHandler" TabIndex="4" />
                        <asp:Button ID="btnCancelDPopup" runat="server" CommandName="CANCELDPOPUP" OnClick="ActionHandler"
                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Close %>" TabIndex="10"
                            Text="<%$Resources:Controls,Close%>" />
                    </div>
                    <div id="divEmpFilterDetails" runat="server">
                        <table class="table-devide">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblCompanyHd" runat="server" Text="<%$ resources:Controls,Company%>"
                                            AssociatedControlID="ddlCompanyHd"></asp:Label>
                                        <asp:DropDownList ID="ddlCompanyHd" runat="server" TabIndex="5" CssClass="select-w61per">
                                        </asp:DropDownList>
                                        <div class="clear">
                                        </div>
                                        <asp:Label ID="lblHdBranchLocation" runat="server" Text="<%$ resources:Branch/Location%>"
                                            AssociatedControlID="txtHdBranchLocation"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtHdBranchLocation" Text="" TabIndex="5" CssClass="input-w59-4per"></asp:TextBox>
                                        <asp:HiddenField ID="hdfHdBranchLocation" Value="-1" runat="server" />
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblHdDepartment" runat="server" Text="<%$ resources:Department%>"
                                            AssociatedControlID="txtHdDepartment"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtHdDepartment" Text="" TabIndex="5" CssClass="input-halfsmall"></asp:TextBox>
                                        <asp:HiddenField ID="hdfHdDepartment" Value="-1" runat="server" />
                                        <div class="clear">
                                        </div>
                                        <asp:Label runat="server" ID="lblEmployee" Text="<%$ resources:Employee%>" AssociatedControlID="txtEmployee"
                                            CssClass="lbl-25-1perc"></asp:Label>
                                        <asp:TextBox ID="txtEmployee" runat="server" CssClass="input-halfsmall" MaxLength="100"
                                            TabIndex="5"></asp:TextBox>
                                        <asp:HiddenField ID="hdfEmployee" runat="server" />
                                        <div class="display-inline">
                                            <asp:ImageButton ID="btnFilterSearch" runat="server" Text="<%$ resources:Search%>"
                                                ToolTip="<%$ resources:Search%>" OnClick="ActionHandler" TabIndex="5" CommandName="DTLSEARCH"
                                                SkinID="search-ext" CssClass="margntop2 margnlft-minus4" ValidationGroup="DTLSEARCH" />
                                            <asp:ImageButton ID="btnFilterClear" runat="server" Text="<%$ resources:Clear%>"
                                                ToolTip="<%$ resources:Clear%>" TabIndex="5" OnClick="ActionHandler" CommandName="CLEARSEARCH"
                                                SkinID="clear-ext" CssClass="margntop2 margnlft-minus4 margnrgt1-2per" />
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="Div5" runat="server" class="gridwrap maxh-370">
                        <asp:GridView ID="grdEmployeePopUpList" runat="server" AutoGenerateColumns="False"
                            Width="100%" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false" ShowFooter="false"
                            Style="table-layout: fixed;">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label></EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField SortExpression="" HeaderStyle-Width="2%">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkEmpHeader" runat="server" ToolTip="Select All Employee" TabIndex="15" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkEmp_popUp" runat="server" TabIndex="9" />
                                        <asp:HiddenField ID="hdfEOT_EMPLOYEE" Value='<%# Eval("EOT_EMPLOYEE") %>' runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="2%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Employee %>" SortExpression="" HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmployeePopUp" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("EOT_EMPLOYEE_TEXT"),30) %>'
                                            ToolTip='<%# Eval("EOT_EMPLOYEE_TEXT") %>'></asp:Label></ItemTemplate>
                                    <ItemStyle Width="30%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Branch/Location %>" SortExpression=""
                                    HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranchPopUp" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("EMP_BRANCH_TEXT"),40) %>'
                                            ToolTip='<%# Eval("EMP_BRANCH_TEXT") %>'></asp:Label></ItemTemplate>
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Department %>" SortExpression="" HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDepartmentPopUp" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("EMP_DEPARTMENT_TEXT"),40) %>'
                                            ToolTip='<%# Eval("EMP_DEPARTMENT_TEXT") %>'></asp:Label></ItemTemplate>
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:MobNo %>" SortExpression="" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMobNoPopUp" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("EMP_MOBILE_NO"),40) %>'
                                            ToolTip='<%# Eval("EMP_MOBILE_NO") %>'></asp:Label></ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>--%>
            <div id="diverrorAlert" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="SEND" runat="server" />
                <asp:ValidationSummary ID="vsUpload" ValidationGroup="upload" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
