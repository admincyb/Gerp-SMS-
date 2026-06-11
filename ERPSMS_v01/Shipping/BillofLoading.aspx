<%@ Page Title="<%$ Resources:Captions,Title_ShippingBL %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="BillofLoading.aspx.cs"
    Inherits="ERPSMS_v01.Shipping.BillofLoading" %>

<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ShippingPrintDocs.ascx" TagName="PrinterControl"
    TagPrefix="pc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        a.downloadClass
        {
            cursor: pointer;
            text-decoration: underline;
        }
        a.removedownloadClass
        {
            cursor: auto;
            text-decoration: none;
        }
    </style>
    <script type="text/javascript">
        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtDate", "dd-M-yy", true, false, $("[id$=hdfindate]").val());
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();
        }
        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
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
        function AfterClose(containerID) {
            if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
            }
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
                $("[id$=pnlSave]").hide();
                $("[id$=btnAddItem]").hide();
            }
            if (mode == 3) {
                var delstatus = parseFloat($("#[id*=hdfDelstatus]").val());
                if (delstatus == 1) {
                    $("[id$=pnlSave]").hide();
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlShippingUpload" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <div id="divSBUCompany" class="buttoncontainer-fields floatLeft">
                                    <asp:DropDownList ID="ddlCompany" class="medium margnbotm0" runat="server" TabIndex="1"
                                        onmouseover="javascript:ShowTooltip('ddlCompany');">
                                    </asp:DropDownList>
                                </div>
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="9" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Submit %>" OnClientClick="javascript:ValidatePageNow('save')"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="23"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('save')"
                                            ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="9" Text="<%$resources:ErpRes,Save %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container padgrgt0" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnShippingPlan" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkShippingPlan" Text="<%$resources:PageNameRes,ShippingPlan %>"
                                TabIndex="11" CommandName="SHIPPINGPLAN" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnContainerEval" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkContainerEval" Text="<%$resources:PageNameRes,ContainerEvaluation %>"
                                TabIndex="12" CommandName="CONTAINEREVALUATION" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnContainerInspection" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkContainerInspection" Text="<%$resources:PageNameRes,ContainerInspection %>"
                                TabIndex="13" CommandName="CONTAINERINSPECTION" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnUploadQADocs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkUploadQADocs" Text="<%$resources:PageNameRes,UploadQADocs %>"
                                TabIndex="14" CommandName="UPLOADQA" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnUploadExportDocs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkUploadExportDocs" Text="<%$resources:PageNameRes,UploadExportDocs %>"
                                TabIndex="15" CommandName="UPLOADEXPORT" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnLoadingPlan" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkLoadingPlan" Text="<%$resources:PageNameRes,LoadingPlan %>"
                                TabIndex="16" CommandName="LOADINGPLAN" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnUploadPhotographs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkUploadPhotographs" Text="<%$resources:PageNameRes,UploadPhotographs %>"
                                TabIndex="17" CommandName="UPLOADPHOTOGRAPHS" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnDeliveryOrder" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkDeliveryOrder" Text="<%$resources:PageNameRes,DeliveryOrder %>"
                                TabIndex="18" CommandName="GOODOUTWARD" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnContainerRelease" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkContainerRelease" Text="<%$resources:PageNameRes,ContainerRelease %>"
                                TabIndex="19" CommandName="CONTAINERRELEASE" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnBillofLoading" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lnkBillofLoading" Text="<%$resources:PageNameRes,BL %>"
                                TabIndex="20" CommandName="BL" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-active"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnPrintShippingDocs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkPrintShippingDocs" Text="<%$resources:PageNameRes,PrintShippingDocs %>"
                                TabIndex="21" CommandName="PRINT" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <%--use the width property of the below table corresponding to the contents in the page--%>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <%--Rename this ID Page_Entry with the corresponding section Id in the documet--%>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <%--Align table cell according to design--%>
                        <asp:TableCell>
                            <div class="contentwrapper">
                                <asp:HiddenField ID="hdfStatus" runat="server" Value="0" />
                                <div id="divMainTab">
                                    <div class="detail-co3">
                                        <div class="div3col-S">
                                            <asp:Label runat="server" ID="lbnCustomer" Text="<%$ resources:Customer%>" AssociatedControlID="lblCustomer">
                                            </asp:Label>
                                            <asp:Label runat="server" ID="lblCustomer"></asp:Label></div>
                                        <div class="div3col-S">
                                            <asp:Label ID="lblDestinationPort" runat="server" Text="<%$ resources:DestinationPort%>"
                                                AssociatedControlID="lblDestinationPortValue"></asp:Label>
                                            <asp:Label runat="server" ID="lblDestinationPortValue"></asp:Label></div>
                                        <div class="div3col-S">
                                            <asp:Label ID="lblInTime" runat="server" AssociatedControlID="lblInTimeValue" Text="<%$ resources:InTime %>"></asp:Label>
                                            <asp:Label runat="server" ID="lblInTimeValue"></asp:Label>
                                        </div>
                                        <div class="div3col-S">
                                            <asp:Label runat="server" ID="lblContainerType" Text="<%$ resources:ContainerNo%>"
                                                AssociatedControlID="lblContainerTypeValue">
                                            </asp:Label>
                                            <asp:Label runat="server" ID="lblContainerTypeValue"></asp:Label></div>
                                        <div class="div3col-S">
                                            <asp:Label ID="lbnInvoice" runat="server" Text="<%$ resources:InvoiceNo%>" AssociatedControlID="lblInvoice"></asp:Label>
                                            <asp:Label runat="server" ID="lblInvoice"></asp:Label></div>
                                        <div class="div3col-S">
                                            <asp:Label ID="lbnInvoiceDate" runat="server" AssociatedControlID="lblInvoiceDate"
                                                Text="<%$ resources:InvoiceDate %>"></asp:Label>
                                            <asp:Label runat="server" ID="lblInvoiceDate"></asp:Label>
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </div>
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label ID="lblBLNo" runat="server" AssociatedControlID="txtBLNo" Text="<%$ resources:BLNo %>">
                                                    </asp:Label>
                                                    <asp:TextBox ID="txtBLNo" runat="server" TabIndex="1" CssClass="input-halfsmall-a"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="reqBLNo" CssClass="star" SetFocusOnError="true" ValidationGroup="upload"
                                                        EnableClientScript="true" runat="server" ControlToValidate="txtBLNo" Display="Dynamic"
                                                        Text="*" ErrorMessage="<%$ resources:Err_BLNo %>">
                                                    </asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblDate" Text="<%$ resources:Date %>" AssociatedControlID="txtDate"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtDate" TabIndex="2" MaxLength="12" CssClass="input-small"
                                                        onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfDate" runat="server" />
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfDate" CssClass="star" SetFocusOnError="true" ValidationGroup="upload"
                                                            EnableClientScript="true" runat="server" ControlToValidate="txtDate" Display="Dynamic"
                                                            Text="*" ErrorMessage="<%$ resources:Err_Date %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="vrfDateSave" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="save" EnableClientScript="true" runat="server" ControlToValidate="txtDate"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Date %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="vreDate" CssClass="star" ValidationGroup="upload"
                                                            runat="server" ControlToValidate="txtDate" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_Date_Valid %>"
                                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                            EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="divcol-S">
                                                    <asp:Label ID="Label3" runat="server" Text="<%$ resources:Remarks %>" AssociatedControlID="txtRemarks"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtRemarks" MaxLength="500" TabIndex="3" TextMode="MultiLine"
                                                        CssClass="multiline-2col input-halfsmall-a" onkeydown="limitText(this,500);"
                                                        onkeyup="limitText(this,500);" onpaste="limitText(this,500);"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label ID="lblType" runat="server" AssociatedControlID="ddlType" Text="<%$ resources:Type %>">
                                                    </asp:Label>
                                                    <asp:DropDownList runat="server" TabIndex="4" ID="ddlType" CssClass="select-half">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="reqType" CssClass="star" SetFocusOnError="true" ValidationGroup="upload"
                                                        InitialValue="-1" EnableClientScript="true" runat="server" ControlToValidate="ddlType"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Type %>">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblTitle" Text="<%$ resources:Title %>" AssociatedControlID="txtTitle"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtTitle" MaxLength="200" onkeydown="limitText(this,200);"
                                                        onkeyup="limitText(this,200);" onpaste="limitText(this,200);" TabIndex="5" CssClass="input-halfsmall-a"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="divcol-S">
                                                    <asp:Label ID="lblFileUpload" runat="server" Text="AttachFile" AssociatedControlID="fupUpload"></asp:Label>
                                                    <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupUpload"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_File_Upload %>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:FileUpload ID="fupUpload" runat="server" TabIndex="6" Style="width: 16%;" />
                                                    <a id="anchorFile" runat="server" target="_blank" tabindex="6"></a>
                                                    <div class="btnwrap-divcol padgrgt0">
                                                        <asp:Button runat="server" ID="btnAddItem" CommandName="ADDITEM" TabIndex="6" OnClick="ActionHandler"
                                                            Style="margin-right: 0px!important;" OnClientClick="javascript:ValidatePageNow('upload')"
                                                            ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="PageAction_Entry" ValidationGroup="upload"
                                                            Text="<%$resources:ErpRes,Add %>" SkinID="btnInner-add" />
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdUploads" Width="100%" PageSize="<%$ resources:PageSize%>"
                                            AllowSorting="false" AllowPaging="false" OnSorting="ActionHandler" OnPageIndexChanging="ActionHandler"
                                            OnRowDataBound="ActionHandler" AutoGenerateColumns="false" TabIndex="7" EmptyDataRowStyle-CssClass="emptytable">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:SlNo %>">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                        <%-- <asp:HiddenField runat="server" ID="hdfType" Value='<%# Eval("SCD_ITEM") %>' />--%>
                                                        <asp:HiddenField runat="server" ID="hdfPK" Value='<%# Eval("SCD_PK") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="3%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Type %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("SCD_DOC_TYPE_TEXT"),15) %>'
                                                            ToolTip='<%# Eval("SCD_DOC_TYPE_TEXT") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Title %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTttle" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("SCD_TITLE"),60) %>'
                                                            ToolTip='<%# Eval("SCD_TITLE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="40%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:File %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFile" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("SCD_FILE"),50) %>'
                                                            ToolTip='<%# Eval("SCD_FILE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="28%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <a runat="server" id="fileView" class="download-icon nomargin" title="<%$ resources:View %>"
                                                            tabindex="8" target="_blank" href='<%# Page.ResolveClientUrl(Eval("SCD_FILE_PATH").ToString()) %>'>
                                                        </a>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="3%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="lnkEdit" runat="server" OnClick="ActionHandler" CommandName="EDITITEM"
                                                            TabIndex="8" SkinID="edit-icon" ToolTip="Edit" CommandArgument="PageAction_Entry"
                                                            OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="3%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                                                            TabIndex="8" SkinID="delete-icon" ToolTip="Delete" CommandArgument="PageAction_Entry"
                                                            OnClientClick="return ShowDeleteConfirm(this);" OnPreRender="btnAction_PreRender"
                                                            OnLoad="btnAction_Load" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="3%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <%--Print popup window --%>
                                <pc1:PrinterControl ID="PrinterControl1" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" runat="server" CssClass="last-modified" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="upload" runat="server" />
                    <asp:ValidationSummary ID="vsPageSave" ValidationGroup="save" runat="server" />
                </div>
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <asp:HiddenField ID="hdfindate" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="so" />
            </div>
            <asp:HiddenField ID="hdfDelstatus" runat="server" Value="0" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAddItem" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
