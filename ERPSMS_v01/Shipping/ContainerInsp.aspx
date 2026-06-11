<%@ Page Title="<%$ Resources:Captions,Title_ContainerInspection %>" Language="C#"
    Theme="ClassicExt" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="ContainerInsp.aspx.cs"
    Inherits="ERPSMS_v01.Shipping.ContainerInsp" %>

<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/CheckListControl.ascx" TagName="CheckListItems"
    TagPrefix="CL1" %>
<%@ Register Src="~/UserControls/ShippingPrintDocs.ascx" TagName="PrinterControl"
    TagPrefix="pc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%= (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString()) %>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");


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
            }
            else {
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlEntry]").show();
                $("[id$=txtDate]").focus();

            }
            InitDateComponents();
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
                $("[id$=pnlSave]").hide();
                $("[id$=pnlDelete]").hide();
                //$("[id$=pnlSubmit]").hide();
                //$("[id$=txtDespatchNumber]").next().hide();                
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
                $("[id$=txtDate]").focus();

                //$("[id$=txtDespatchNumber]").next().show();               
            }
            else if (mode == 3) {
                //$("[id$=txtDespatchNumber]").next().hide();                
            }
            if (mode == 4) {

                var delstatus = parseFloat($("#[id*=hdfDelstatus]").val());
                if (delstatus == 1) {
                    $("[id$=pnlSave]").hide();
                }
            }
        }
        function InitDateComponents() {
            GrandScriptUtils.DatePickerCommon("txtDate");
            //GrandScriptUtils.MakeAutoCompleteDDL("txtDespatchNumber", url + "?Status=" + $("[id$=hdfApproved]").val(), "hdfDPHPK", true, true, "DELIVERYORDERNUMBER");
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();
        }

        function AfterAutoCompleteSelect(targetControlID) {
            //if (targetControlID == "txtDespatchNumber") {
            // $("[id$=btnGODetails]").click();
            //}
        }

        function AfterClose(containerID) {
            if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
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
                ShowErrorMessage($("#diverror").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlAvtivity" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <div id="divSBUCompany" class="buttoncontainer-fields floatLeft">
                                    <asp:DropDownList ID="ddlCompany" CssClass="medium margnbotm0" runat="server" TabIndex="1" onmouseover="javascript:ShowTooltip('ddlCompany');">
                                    </asp:DropDownList>
                                </div>
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="50" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('ContainerInspection')"
                                            ValidationGroup="ContainerInspection" ToolTip="<%$resources:ErpRes,Submit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="51"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('ContainerInspection')"
                                            ValidationGroup="ContainerInspection" ToolTip="<%$resources:ErpRes,SaveSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="52" Text="<%$resources:ErpRes,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('ContainerInspection')"
                                            ValidationGroup="ContainerInspection" ToolTip="<%$resources:ErpRes,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" TabIndex="53" Text="<%$resources:ErpRes,Delete %>"
                                            OnClick="ActionHandler" OnClientClick="return ShowDeleteConfirm(this);" ToolTip="<%$resources:ErpRes,Delete %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" Visible="false" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:ErpRes,Cancel %>" OnClick="ActionHandler"
                                            CommandName="CANCEL" TabIndex="54" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel"
                                            ToolTip="<%$resources:ErpRes,Cancel %>" Visible="false" />
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
                                TabIndex="5" CommandName="SHIPPINGPLAN" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnContainerEval" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkContainerEval" Text="<%$resources:PageNameRes,ContainerEvaluation %>"
                                TabIndex="6" CommandName="CONTAINEREVALUATION" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnContainerInspection" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lnkContainerInspection" Text="<%$resources:PageNameRes,ContainerInspection %>"
                                TabIndex="7" CommandName="CONTAINERINSPECTION" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-active"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnUploadQADocs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkUploadQADocs" Text="<%$resources:PageNameRes,UploadQADocs %>"
                                TabIndex="8" CommandName="UPLOADQA" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnUploadExportDocs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkUploadExportDocs" Text="<%$resources:PageNameRes,UploadExportDocs %>"
                                TabIndex="9" CommandName="UPLOADEXPORT" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnLoadingPlan" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkLoadingPlan" Text="<%$resources:PageNameRes,LoadingPlan %>"
                                TabIndex="10" CommandName="LOADINGPLAN" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnUploadPhotographs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkUploadPhotographs" Text="<%$resources:PageNameRes,UploadPhotographs %>"
                                TabIndex="11" CommandName="UPLOADPHOTOGRAPHS" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnDeliveryOrder" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkDeliveryOrder" Text="<%$resources:PageNameRes,DeliveryOrder %>"
                                TabIndex="12" CommandName="GOODOUTWARD" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnContainerRelease" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkContainerRelease" Text="<%$resources:PageNameRes,ContainerRelease %>"
                                TabIndex="13" CommandName="CONTAINERRELEASE" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnBillofLoading" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkBillofLoading" Text="<%$resources:PageNameRes,BL %>"
                                TabIndex="20" CommandName="BL" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnPrintShippingDocs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkPrintShippingDocs" Text="<%$resources:PageNameRes,PrintShippingDocs %>"
                                TabIndex="14" CommandName="PRINT" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
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
                            <asp:HiddenField ID="hdfStatus" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfInspectionNo" runat="server" Value="" />
                            <div class="detail-co3">
                                <div class="div3col-S">
                                    <asp:Label runat="server" ID="lbnCustomer" Text="<%$resources:Controls,CustomerHdr%>"
                                        AssociatedControlID="lblCustomerHdr">
                                    </asp:Label>
                                    <asp:Label runat="server" ID="lblCustomerHdr" CssClass="disp-table"></asp:Label>
                                </div>
                                <div class="div3col-S">
                                    <asp:Label ID="lblDestinationPort" runat="server" Text="<%$resources:Controls,DestinationPortHdr%>"
                                        AssociatedControlID="lblDestinationPortValue"></asp:Label>
                                    <asp:Label runat="server" ID="lblDestinationPortValue" CssClass="disp-table w60perc"></asp:Label>
                                </div>
                                <div class="div3col-S">
                                    <asp:Label ID="lblInTime" runat="server" AssociatedControlID="lblInTimeValue" Text="<%$resources:Controls,InTimeHdr%>"></asp:Label>
                                    <asp:Label runat="server" ID="lblInTimeValue"></asp:Label>
                                </div>
                                <div class="div3col-S">
                                    <asp:Label runat="server" ID="Label1" Text="<%$resources:ContainerTypeHr%>"
                                        AssociatedControlID="lblContainerTypeValue">
                                    </asp:Label>
                                    <%--<asp:Label runat="server" ID="Label1" Text="<%$resources:Controls,ContainerNoHdr%>"
                                        AssociatedControlID="lblContainerTypeValue">
                                    </asp:Label>--%>
                                    <asp:Label runat="server" ID="lblContainerTypeValue"></asp:Label>
                                </div>
                                <div class="div3col-S">
                                    <asp:Label runat="server" ID="Label2" Text="<%$resources:Controls,ShippingPlanNo%>"
                                        AssociatedControlID="lblShippingPlanNoHdr">
                                    </asp:Label>
                                    <asp:Label runat="server" ID="lblShippingPlanNoHdr"></asp:Label>
                                </div>
                                <div class="div3col-S">
                                    <asp:Label runat="server" ID="Label4" Text="<%$resources:Controls,ShippingPlanDate%>"
                                        AssociatedControlID="lblShippingPlanDateHdr">
                                    </asp:Label>
                                    <asp:Label runat="server" ID="lblShippingPlanDateHdr"></asp:Label>
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <%--<asp:Label runat="server" ID="lblGONo" Text="<%$ resources:GoNo %>" AssociatedControlID="txtDespatchNumber"></asp:Label>
                                            <asp:TextBox ID="txtDespatchNumber" runat="server" CssClass="medium" MaxLength="100"
                                                TabIndex="1"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfDeliverynumber" CssClass="star" SetFocusOnError="true"
                                                InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>" ValidationGroup="ContainerInspection"
                                                EnableClientScript="true" runat="server" ControlToValidate="txtDespatchNumber"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_DeliveryNumber %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfDPHPK" runat="server" Value="" />
                                            <asp:Button ID="btnGODetails" runat="server" OnClick="ActionHandler" CommandName="SHOW"
                                                EnableTheming="false" Style="display: none" />--%>
                                            <asp:Label runat="server" ID="lblInspectionNo" Text="<%$ resources:InspectionNo %>"
                                                AssociatedControlID="lblInspectionNoTxt"></asp:Label>
                                            <asp:Label runat="server" ID="lblInspectionNoTxt" CssClass="input-small"></asp:Label>
                                            <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />

                                            <asp:Label runat="server" ID="lblDate" Text="<%$ resources:Date %>" CssClass="middle-lbl-small-d" AssociatedControlID="txtDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDate" TabIndex="15" MaxLength="12" CssClass="input-small"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="vrfDate" CssClass="star" SetFocusOnError="true" ValidationGroup="ContainerInspection"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtDate" Display="Dynamic"
                                                    Text="*" ErrorMessage="<%$ resources:Err_Date %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="vreDate" CssClass="star" ValidationGroup="ContainerInspection"
                                                    runat="server" ControlToValidate="txtDate" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_Date_Valid %>"
                                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                    EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                            </div>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblCustomer" Text="<%$ resources:Customer %>" AssociatedControlID="lblCustomerTxt"></asp:Label>
                                            <asp:Label runat="server" ID="lblCustomerTxt" CssClass="select-half"></asp:Label>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">

                                            <asp:Label runat="server" ID="lblContainerType" Text="<%$ resources:ContainerType %>"
                                                AssociatedControlID="lblContainerTypeTxt"></asp:Label>
                                            <asp:Label runat="server" ID="lblContainerTypeTxt" CssClass="input-small margnrgt1-5per"></asp:Label>


                                            <asp:Label runat="server" ID="lblContainerNo" Text="<%$ resources:ContainerNo %>" CssClass="middle-lbl-a"
                                                AssociatedControlID="txtContainerNo"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtContainerNo" TabIndex="16" MaxLength="200" CssClass="input-small"></asp:TextBox>
                                            <div class="clear">
                                            </div>

                                            <asp:Label runat="server" ID="lblSerialNo" Text="<%$ resources:SealNo %>" AssociatedControlID="txtSerialNo"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSerialNo" TabIndex="17" MaxLength="200" CssClass="input-small"></asp:TextBox>

                                            <asp:Label runat="server" ID="lblISOPAS" Text="<%$ resources:ISOPAS %>" AssociatedControlID="txtISOPAS" CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtISOPAS" TabIndex="18" MaxLength="200" CssClass="input-small"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblRemarks" runat="server" Text="<%$ resources:Remarks%>" AssociatedControlID="txtRemarks" />
                                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" MaxLength="400"
                                                TabIndex="19" Width="715" CssClass="multiline-2line" onkeydown="limitText(this,400);"
                                                onkeyup="limitText(this,400);" onpaste="return false;" />
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <CL1:CheckListItems ID="uclCheckList" runat="server" TabIndex="20" />
                            <%--Print popup window --%>
                            <pc1:PrinterControl ID="PrinterControl1" runat="server" />
                        </asp:TableCell></asp:TableRow><asp:TableRow>
                    </asp:TableRow>

                  <%--  </br/>--%>
                   <%-- <asp:TableRow ID="TableRowAttachment" runat="server">
                        <asp:TableCell>
                            <div class="divcol-S">
                                <asp:Label ID="lblFileUpload" runat="server" Text="Attach File" AssociatedControlID="fupUpload"></asp:Label>
                                <div class="fileupload-main">
                                    <asp:FileUpload ID="fupUpload" runat="server" CssClass="margn-rgt0 upload-area" TabIndex="4" />
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
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdUploads" Width="99%" PageSize="<%$ resources:PageSize%>"
                                                AllowSorting="false" AllowPaging="false" OnRowDataBound="ActionHandler" AutoGenerateColumns="false"
                                                TabIndex="7" EmptyDataRowStyle-CssClass="emptytable">
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ resources:SlNo %>">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="3%" HorizontalAlign="Center" Wrap="false" />
                                                    </asp:TemplateField>
                                                   <%-- <asp:TemplateField HeaderText="<%$ resources:Title %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTitle" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("SCD_TITLE"),40) %>'
                                                                ToolTip='<%# Eval("SCD_TITLE") %>'></asp:Label>
                                                            <asp:HiddenField runat="server" ID="hdfType" Value='<%# Eval("SCD_ITEM") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfPK" Value='<%# Eval("SCD_PK") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="50%" />
                                                    </asp:TemplateField>--%>
                                                    <%--<asp:TemplateField HeaderText="<%$ resources:File %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFile" runat="server" Text='<%# Eval("SCD_FILE") %>' ToolTip='<%# Eval("SCD_FILE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="41%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a runat="server" id="fileView" class="download-icon nomargin" title="<%$ resources:View %>"
                                                                target="_blank" href='<%# Page.ResolveClientUrl(Eval("SCD_FILE_PATH").ToString()) %>'></a>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="lnkEdit" runat="server" OnClick="ActionHandler" CommandName="EDITITEM"
                                                                SkinID="edit-icon" ToolTip="Edit" CommandArgument="PageAction_Entry" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                                                                SkinID="delete-icon" ToolTip="Delete" CommandArgument="PageAction_Entry" OnClientClick="return ShowDeleteConfirm(this);" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="3%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                            </div>

                        </asp:TableCell>
                    </asp:TableRow>--%>
                    <asp:TableRow ID="ModifiedDatePnl" runat="server" CssClass="last-modified" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell></asp:TableRow></asp:Table><div id="diverror" style="display: none">
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary
                        ID="vsPage" ValidationGroup="ContainerInspection" runat="server" />
                    <asp:ValidationSummary ID="vsAttachment" ValidationGroup="upload" runat="server" />
                    <asp:HiddenField ID="hdfAppType" runat="server" />
                    <asp:HiddenField ID="hdfAppSubType" runat="server" />
                    <asp:HiddenField ID="hdfApproved" runat="server" Value="0" />
                </div>
                <asp:HiddenField ID="hdfDelstatus" runat="server" Value="0" />
            </div>

            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="ContainerInspection" />
            </div>
        </ContentTemplate>
         <%--  <Triggers>
            <asp:PostBackTrigger ControlID="btnAddItem" />
        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
