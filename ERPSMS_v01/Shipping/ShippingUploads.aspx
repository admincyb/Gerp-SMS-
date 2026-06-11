<%@ Page Title="<%$ Resources:Captions,Title_ShippingUploads %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="ShippingUploads.aspx.cs"
    Inherits="ERPSMS_v01.Shipping.ShippingUploads" %>

<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ShippingPrintDocs.ascx" TagName="PrinterControl"
    TagPrefix="pc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtDate");
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
    <style type="text/css">
        a.downloadClass
        {
            cursor: pointer;
            text-decoration: underline;
        }
        a.removedownloadClass
        {
            cursor: default;
            text-decoration: none;
        }
    </style>
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
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="8" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="23"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,SaveSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="9" Text="<%$resources:ErpRes,Save %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:ErpRes,Cancel %>" OnClick="ActionHandler"
                                            CommandName="CANCEL" TabIndex="10" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel"
                                            ToolTip="<%$resources:ErpRes,Cancel %>" />
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
                        <li><span id="spnBillofLoading" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkBillofLoading" Text="<%$resources:PageNameRes,BL %>"
                                TabIndex="20" CommandName="BL" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnPrintShippingDocs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkPrintShippingDocs" Text="<%$resources:PageNameRes,PrintShippingDocs %>"
                                TabIndex="20" CommandName="PRINT" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
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
                                            <asp:Label runat="server" ID="lbnCustomerHdr" Text="<%$resources:Controls,CustomerHdr%>"
                                                AssociatedControlID="lblCustomerHdr">
                                            </asp:Label>
                                            <asp:Label runat="server" ID="lblCustomerHdr" CssClass="disp-table"></asp:Label></div>
                                        <div class="div3col-S">
                                            <asp:Label ID="lbnDestinationPortHdr" runat="server" Text="<%$resources:Controls,DestinationPortHdr%>"
                                                AssociatedControlID="lblDestinationPortHdr"></asp:Label>
                                            <asp:Label runat="server" ID="lblDestinationPortHdr" CssClass="disp-table"></asp:Label></div>
                                        <div class="div3col-S">
                                            <asp:Label ID="lbnInTimeHdr" runat="server" AssociatedControlID="lblInTimeHdr" Text="<%$resources:Controls,InTimeHdr%>"></asp:Label>
                                            <asp:Label runat="server" ID="lblInTimeHdr"></asp:Label>
                                        </div>
                                        <div class="div3col-S">
                                            <asp:Label runat="server" ID="lbnContainerTypeValueHdr" Text="<%$resources:ContainerTypeHr%>"
                                                AssociatedControlID="lblContainerTypeValueHdr">
                                            </asp:Label>
                                            <asp:Label runat="server" ID="lblContainerTypeValueHdr"></asp:Label></div>
                                        <div class="div3col-S">
                                            <asp:Label runat="server" ID="Label2" Text="<%$resources:Controls,ShippingPlanNo%>"
                                                AssociatedControlID="lblShippingPlanNoHdr">
                                            </asp:Label>
                                            <asp:Label runat="server" ID="lblShippingPlanNoHdr"></asp:Label></div>
                                        <div class="div3col-S">
                                            <asp:Label runat="server" ID="Label4" Text="<%$resources:Controls,ShippingPlanDate%>"
                                                AssociatedControlID="lblShippingPlanDateHdr">
                                            </asp:Label>
                                            <asp:Label runat="server" ID="lblShippingPlanDateHdr"></asp:Label></div>
                                        <div class="clear">
                                        </div>
                                    </div>
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label ID="lblType" runat="server" AssociatedControlID="ddlType" Text="<%$ resources:Type %>">
                                                    </asp:Label>
                                                    <asp:DropDownList runat="server" TabIndex="1" ID="ddlType" CssClass="select-half">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="upload" InitialValue="-1" EnableClientScript="true" runat="server"
                                                        ControlToValidate="ddlType" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Type %>">
                                                    </asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblDate" Text="<%$ resources:Date %>" AssociatedControlID="txtDate"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtDate" TabIndex="2" MaxLength="12" CssClass="Uidate-picker"
                                                        onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfDate" runat="server" />
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfDate" CssClass="star" SetFocusOnError="true" ValidationGroup="upload"
                                                            EnableClientScript="true" runat="server" ControlToValidate="txtDate" Display="Dynamic"
                                                            Text="*" ErrorMessage="<%$ resources:Err_Date %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="vreDate" CssClass="star" ValidationGroup="upload"
                                                            runat="server" ControlToValidate="txtDate" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_Date_Valid %>"
                                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                            EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="divcol-S">
                                                    <asp:Label runat="server" ID="lblTitle" Text="<%$ resources:Title %>" AssociatedControlID="txtTitle"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtTitle" TabIndex="3"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="divcol-S">
                                                    <asp:Label ID="Label3" runat="server" Text="Description" AssociatedControlID="txtDescription"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtDescription" MaxLength="500" TabIndex="4" TextMode="MultiLine"
                                                        CssClass="multiline-2col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="divcol-S">
                                                    <asp:Label ID="lblFileUpload" runat="server" Text="AttachFile" AssociatedControlID="fupUpload"></asp:Label>
                                                    <div class="fileupload-main">
                                                        <asp:FileUpload ID="fupUpload" runat="server" CssClass="margn-rgt0 upload-area" TabIndex="4"  />
                                                        <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupUpload"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_File_Upload %>">
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                    <a id="anchorFile" runat="server" target="_blank" tabindex="5"></a>
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
                                                <asp:TemplateField HeaderText="<%$ resources:Type %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblType" runat="server" Text='<%# Eval("SCD_ITEM_Text") %>' ToolTip='<%# Eval("SCD_ITEM_Text") %>'></asp:Label>
                                                        <asp:HiddenField runat="server" ID="hdfType" Value='<%# Eval("SCD_ITEM") %>' />
                                                        <asp:HiddenField runat="server" ID="hdfPK" Value='<%# Eval("SCD_PK") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="28%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("SCD_DATE") %>' ToolTip='<%# Eval("SCD_DATE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="8%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Title %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTitle" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("SCD_TITLE"),40) %>'
                                                            ToolTip='<%# Eval("SCD_TITLE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="33%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:File %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFile" runat="server" Text='<%# Eval("SCD_FILE") %>' ToolTip='<%# Eval("SCD_FILE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="26.5%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <a runat="server" id="fileView" class="download-icon nomargin" title="<%$ resources:View %>"
                                                            target="_blank" href='<%# Page.ResolveClientUrl(Eval("SCD_FILE_PATH").ToString()) %>'>
                                                        </a>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="1.5%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="lnkEdit" runat="server" OnClick="ActionHandler" CommandName="EDITITEM"
                                                            SkinID="edit-icon" ToolTip="Edit" CommandArgument="PageAction_Entry" OnPreRender="btnAction_PreRender"
                                                            OnLoad="btnAction_Load" Style="margin: 0px !important;" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="1.5%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                                                            SkinID="delete-icon" ToolTip="Delete" CommandArgument="PageAction_Entry" OnClientClick="return ShowDeleteConfirm(this);"
                                                            OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load" Style="margin: 0px !important;" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="1.5%" />
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
                </div>
                <asp:HiddenField ID="hdfDelstatus" runat="server" Value="0" />
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="so" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAddItem" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
