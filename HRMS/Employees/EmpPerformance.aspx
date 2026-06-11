<%@ Page Title="<%$ resources:HRMS-Employee Performance%>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="EmpPerformance.aspx.cs" Inherits="HRMS.Employees.Behavior"
    Theme="ClassicExt" %>

<%@ Register Src="UserControls/GtiTabControl.ascx" TagName="GtiTabControl" TagPrefix="ucGtiTab" %>
<%@ Register Src="UserControls/EmpBasicInfoControl.ascx" TagName="EmpBasicInfoControl"
    TagPrefix="ucBasicHdr" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript">

        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            $(document).ready(function () {
                GrandScriptUtils.MakeAutoCompleteDDL("txtDoneBy", url + "?EmpCategory=2", "hdfDoneByEmployee", true, true, "EMPLOYEEAUTOCOMPLETE");
            });
            GrandScriptUtils.DatePickerCommon("txtDate");
        }

        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtDoneBy") {
                $("[id$=hdfDoneByEmployee]").val("0");
            }
        }

        function EndRequestHandlerPage() {
            ShowListing($("[id$=hdnTabStatus]").val());
        }

        function ShowListing(flag) {
            $("[id$=hdnTabStatus]").val(flag);
            var entryStatus = $("[id$=hdfEntryStatus]").val();
            if (flag == '1') {
                $("[id$=PageAction_List]").show();
                $("[id$=pnlCancelList]").show();
                if (entryStatus != "1") {
                    $("[id$=pnlEdit]").show();
                    $("[id$=pnlNew]").show();
                }
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlSave]").hide();
                $("[id$=pnlSaveAndContinue]").hide();
                $("[id$=pnlCancel]").hide();              
                $("[id$=pnlDelete]").hide();               
                $("[id$=ModifiedDatePnl]").hide();
                $("[id$=lnkList]").addClass('tab-active');
                $("[id$=lnkList]").removeClass('tab-inactive');
                $("[id$=lnkDetail]").addClass('tab-inactive');
            }
            else if (flag == '0') {
                $("[id$=PageAction_Entry]").show();
                if (entryStatus != "1") {
                    $("[id$=pnlSave]").show();
                    $("[id$=pnlSaveAndContinue]").show();
                    $("[id$=pnlCancel]").show();
                    $("[id$=pnlDelete]").show();
                }
                $("[id$=PageAction_List]").hide();               
                $("[id$=pnlCancelList]").hide();
                $("[id$=pnlEdit]").hide();             
                $("[id$=pnlNew]").hide();
                $("[id$=lnkList]").addClass('tab-inactive');
                $("[id$=lnkDetail]").addClass('tab-active');
                $("[id$=lnkDetail]").removeClass('tab-inactive');
            }
            return false;
        }
        /// AutoComplete textbox
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");


        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }

        //Validate number only
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode           
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlDelete]").hide();               
                $("[id$=btnAddItem]").hide();
                $("[id$=pnlNew]").hide();
                $("[id$=pnlEdit]").hide();              
            }
        }       

    </script>
    <style type="text/css">
        .divcol-S label
        {
            max-width: 136px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <ucGtiTab:GtiTabControl ID="hrmsTab" CurrentTab="12" runat="server" />
                <div class="Button-container">
                    <asp:Table ID="Table3" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" Text="<%$ resources:Breadcrumb%>" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlSaveAndContinue">
                                        <asp:Button runat="server" ID="btnSaveContinue" OnClick="ActionHandler" CommandName="SAVEANDCONTINUE"
                                            TabIndex="150" Text="Save & Continue" ToolTip="Save & Continue" ValidationGroup="Employee"
                                            OnClientClick="javascript:return ValidatePageNow('Performance')" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" OnClick="ActionHandler" CommandName="SAVE"
                                            TabIndex="151" Text="<%$ resources:Save%>" ToolTip="<%$ resources:Save%>" ValidationGroup="Performance"
                                            OnClientClick="javascript:return ValidatePageNow('Performance')" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button ID="btnDelete" runat="server" Visible="true" SkinID="btnInner-Delete"
                                            Text="<%$Resources:Controls,Delete%>" OnClientClick="return ShowDeleteConfirm(this);"
                                            CommandName="DELETE" OnClick="ActionHandler" ToolTip="Delete" TabIndex="152" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" OnClick="ActionHandler" Text="<%$ resources:Cancel%>"
                                            ToolTip="<%$ resources:Cancel%>" CommandName="CANCEL" TabIndex="153" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" />
                                    </li>
                                    <li runat="server" id="pnlNew">
                                        <asp:Button runat="server" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$ resources:New%>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$ resources:New%>" TabIndex="154" />
                                    </li>
                                    <li runat="server" id="pnlEdit">
                                        <asp:Button ID="btnEdit" runat="server" SkinID="btnInner-Edit" Text="<%$Resources:Controls,Edit%>"
                                            CommandName="EDIT" OnClick="ActionHandler" ToolTip="Edit" TabIndex="155" />
                                    </li>
                                    <li runat="server" id="pnlCancelList">
                                        <asp:Button runat="server" ID="btnCancelList" OnClick="ActionHandler" Text="<%$ resources:LCancel%>"
                                            ToolTip="<%$ resources:LCancel%>" CommandName="CANCELTOLIST" TabIndex="156" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <ucBasicHdr:EmpBasicInfoControl ID="UCempBasicHdr" runat="server" />
                <asp:HiddenField ID="hdfSelectedItemPk" runat="server" Value="0" />
                <div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="List" CommandArgument="SEC_ActionPanel"
                                CommandName="CANCEL" CssClass="tab-active" Style="margin-top: -7px;" OnClick="ActionHandler"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="Detail" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CommandName="EDIT" CssClass="tab-inactive" Style="margin-top: -7px;"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="gridwrap" style="margin-top: -5px;">
                                <asp:GridView ID="grdEmployeePerformancelist" runat="server" AutoGenerateColumns="False"
                                    AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="True"
                                    OnSorting="ActionHandler" Width="100%">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    OnCheckedChanged="ActionHandler" TabIndex="4" />
                                                <asp:HiddenField runat="server" ID="hdfPerPk" Value='<%# Eval("EPD_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%> ">
                                            <%--SortExpression="EQD_QUALIFICATION_TYPE_TEXT"--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblqual" runat="server" ToolTip='<%# Eval("EPD_DATE", Resources.Constants.HRMSDateFormatGrid) %>'
                                                    Text='<%# Eval("EPD_DATE", Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DoneBy%> ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTitl" runat="server" ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EPD_DONE_BY_TEXT")))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EPD_DONE_BY_TEXT")),25) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Category%> ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInst" runat="server" ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EPD_CATEGORY_TEXT")))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EPD_CATEGORY_TEXT")),25) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Action%> ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCertificate" runat="server" ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EPD_ACTION_TEXT")))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EPD_ACTION_TEXT")),25) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Incident%> ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSta" runat="server" ToolTip='<%# Eval("EPD_INCIDENT") %>' Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EPD_INCIDENT")),40) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
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
                            <table class="table-devide" runat="server" id="pnlEmpDocDetails">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblDate" AssociatedControlID="txtDate" Text="<%$ resources:DateStar%>"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDate" TabIndex="5" CssClass="input-small" MaxLength="15"
                                                onkeydown="return CheckKey(event)" onpaste="return false;" />
                                            <asp:RequiredFieldValidator ID="vrfDate" CssClass="star" SetFocusOnError="true" ValidationGroup="Performance"
                                                EnableClientScript="true" runat="server" ControlToValidate="txtDate" Display="Dynamic"
                                                Text="*" ErrorMessage="<%$ resources:Err_Date%>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblDoneBy" AssociatedControlID="txtDoneBy" Text="<%$ resources:DoneByStar%>"></asp:Label>
                                            <asp:TextBox runat="server" TabIndex="5" MaxLength="100" ID="txtDoneBy" CssClass="input-half" />
                                            <asp:HiddenField ID="hdfDoneByEmployee" runat="server" Value="0" />
                                            <asp:RequiredFieldValidator ID="vrfDoneBy" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="txtDoneBy" Display="Dynamic" Text="*" ValidationGroup="Performance"
                                                ErrorMessage="<%$ resources:Err_DoneBy %>" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblCategory" AssociatedControlID="ddlCategory" Text="<%$ resources:CategoryStar%>"></asp:Label>
                                            <asp:DropDownList ID="ddlCategory" runat="server" TabIndex="5" CssClass="select-small-a1"
                                                OnSelectedIndexChanged="ActionHandler" AutoPostBack="True">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfCategory" runat="server" ControlToValidate="ddlCategory"
                                                CssClass="star" ValidationGroup="Performance" Text="*" ErrorMessage="<%$ resources:Err_SelectCategory%>"
                                                InitialValue="-1"></asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblAction" AssociatedControlID="ddlAction" Text="<%$ resources:ActionStar%>"
                                                CssClass="lbl-14-2perc"></asp:Label>
                                            <asp:DropDownList ID="ddlAction" TabIndex="5" runat="server" CssClass="select-w33per">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfAction" runat="server" ControlToValidate="ddlAction"
                                                CssClass="star" ValidationGroup="Performance" Text="*" ErrorMessage="<%$ resources:Err_SelectAction%>"
                                                InitialValue="-1"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblIncident" runat="server" Text="<%$ resources:Incident%>" AssociatedControlID="txtIncident"
                                                CssClass="margn-rgt0"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtIncident" MaxLength="500" TabIndex="5" TextMode="MultiLine"
                                                CssClass="multiline-2col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblFileUpload" runat="server" Text="Attach File" AssociatedControlID="fupUpload"
                                                CssClass="margn-rgt0"></asp:Label>
                                            <asp:FileUpload ID="fupUpload" runat="server" TabIndex="6" Style="width: 15.6%;" />
                                            <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupUpload"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:SelectFile%>">
                                            </asp:RequiredFieldValidator>
                                            <a id="anchorFile" runat="server" target="_blank"></a>
                                            <asp:Button runat="server" ID="btnAddItem" CommandName="ADDITEM" TabIndex="6" OnClick="ActionHandler"
                                                OnClientClick="javascript:ValidatePageNow('upload')" ToolTip="<%$ resources:FAdd%>"
                                                CommandArgument="PageAction_Entry" ValidationGroup="upload" Text="<%$ resources:FAdd%>"
                                                SkinID="btnInner-add" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="gridwrap">
                                            <asp:GridView runat="server" ID="grdUploads" Width="100%" DataKeyNames="DOC_PK" AllowSorting="false"
                                                AllowPaging="false" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                                OnRowDataBound="ActionHandler">
                                                <%-- OnSorting="ActionHandler"
                                                OnPageIndexChanging="ActionHandler" OnRowDataBound="ActionHandler" --%>
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ resources:SlNo%>">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                            <asp:HiddenField runat="server" ID="hdfPK" Value='<%# Eval("DOC_PK") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfSlNo" Value='<%# Container.DataItemIndex + 1 %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="4%" Wrap="false" />
                                                        <ItemStyle Width="4%" HorizontalAlign="Center" Wrap="false" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:File%>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFile" runat="server" Text='<%# Eval("DOC_NAME") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="94%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-CssClass="file-details">
                                                        <ItemTemplate>
                                                            <a runat="server" id="fileView" class="download-icon nomargin" title="View" target="_blank" tabindex="7"
                                                                href='<%# Page.ResolveClientUrl(Eval("DOC_PATH").ToString()) %>'></a>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-CssClass="file-details">
                                                        <ItemTemplate>
                                                            <asp:Button ID="lnkRemove" runat="server" CommandName="REMOVEITEM" SkinID="delete-icon" TabIndex="7"
                                                                ToolTip="Delete" CommandArgument="PageAction_Entry" OnClick="ActionHandler" OnClientClick="return ShowDeleteConfirm(this);"
                                                                OnPreRender="btnAction_PreRender" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="3%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div id="diverror" style="display: none">
                                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                                <asp:ValidationSummary ID="vsPageSave" ValidationGroup="Performance" runat="server" />
                                <asp:ValidationSummary ID="vsImageUpload" ValidationGroup="upload" runat="server" />
                            </div>
                        </asp:TableCell></asp:TableRow><asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <span style="float: right !important;" id="lblLastModifiedDate" runat="server"></span>
                        </asp:TableCell></asp:TableRow></asp:Table><asp:HiddenField ID="hdfEmployeeQDOB" runat="server" Value="" />
                <asp:HiddenField ID="hdnTabStatus" runat="server" Value="0" />
                <asp:HiddenField ID="hdfCompany" runat="server" Value="0" />
                <asp:HiddenField ID="hdfEntryStatus" runat="server" Value="0" />
            </div>
        </ContentTemplate>
        <Triggers>
            <%-- <asp:AsyncPostBackTrigger ControlID="btnAddItem" />--%>
            <asp:PostBackTrigger ControlID="btnAddItem" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
