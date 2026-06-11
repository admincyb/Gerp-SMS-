<%@ Page Title="<%$ Resources:Captions,Title_ContainerEvaluation %>" Theme="ClassicExt"
    Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="ContainerCheck.aspx.cs"
    Inherits="ERPSMS_v01.Shipping.ContainerCheck" %>

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
                var focuschng = parseInt($("#[id*=hdfChangeFocus]").val());
                if (focuschng == 0) {
                    $("[id$=txtDate]").focus();
                }
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
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
                var focuschng = parseInt($("#[id*=hdfChangeFocus]").val());
                if (focuschng == 0) {
                    $("[id$=txtDate]").focus();
                }
            }
            if (mode == 3) {
                var delstatus = parseFloat($("#[id*=hdfDelstatus]").val());
                if (delstatus == 1) {
                    $("[id$=pnlSave]").hide();
                }
            }
        }
        function InitDateComponents() {
            GrandScriptUtils.DatePickerCommon("txtDate");
            GrandScriptUtils.DatePickerCommon("txtBookingDate");
            GrandScriptUtils.DatePickerCommon("txtUploadDate");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCompany", url, "hdfTransCompany", true, true, "VENDOR_ROLE");
            $('[id$=txtInTime]').timepicker();
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();
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
        function ShowHideAttachmentDetails(flag) {
            //If flag then Show AdvancedSearch
            if (flag) {
                $("[id$=divUpload]").show();
                $("[id$=imbAddShowDetails]").hide();
                $("[id$=imbAddHideDetails]").show();
            }
            else {
                $("[id$=divUpload]").hide();
                $("[id$=imbAddShowDetails]").show();
                $("[id$=imbAddHideDetails]").hide();
            }
            return false;
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
                                    <asp:DropDownList ID="ddlCompany" CssClass="medium margnbotm0" runat="server" TabIndex="1"
                                        onmouseover="javascript:ShowTooltip('ddlCompany');">
                                    </asp:DropDownList>
                                </div>
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="50" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('ContainerEvaluation')"
                                            ValidationGroup="ContainerEvaluation" ToolTip="<%$resources:ErpRes,Submit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="51"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('ContainerEvaluation')"
                                            ValidationGroup="ContainerEvaluation" ToolTip="<%$resources:ErpRes,SaveSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="52" Text="<%$resources:ErpRes,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('ContainerEvaluation')"
                                            ValidationGroup="ContainerEvaluation" ToolTip="<%$resources:ErpRes,Save %>" CommandArgument="SEC_ActionPanel"
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
                        <li><span id="spnContainerEval" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lnkContainerEval" Text="<%$resources:PageNameRes,ContainerEvaluation %>"
                                TabIndex="6" CommandName="CONTAINEREVALUATION" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-active"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnContainerInspection" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkContainerInspection" Text="<%$resources:PageNameRes,ContainerInspection %>"
                                TabIndex="7" CommandName="CONTAINERINSPECTION" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnUploadQADocs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkUploadQADocs" Text="<%$resources:PageNameRes,UploadQADocs %>"
                                TabIndex="8" CommandName="UPLOADQA" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnUploadExportDocs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkUploadExportDocs" Text="<%$resources:PageNameRes,UploadExportDocs %>"
                                TabIndex="9" CommandName="UPLOADEXPORT" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnLoadingPlan" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkLoadingPlan" Text="<%$resources:PageNameRes,LoadingPlan %>"
                                TabIndex="10" CommandName="LOADINGPLAN" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnUploadPhotographs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkUploadPhotographs" Text="<%$resources:PageNameRes,UploadPhotographs %>"
                                TabIndex="11" CommandName="UPLOADPHOTOGRAPHS" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnDeliveryOrder" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkDeliveryOrder" Text="<%$resources:PageNameRes,DeliveryOrder %>"
                                TabIndex="12" CommandName="GOODOUTWARD" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnContainerRelease" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkContainerRelease" Text="<%$resources:PageNameRes,ContainerRelease %>"
                                TabIndex="13" CommandName="CONTAINERRELEASE" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnBillofLoading" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkBillofLoading" Text="<%$resources:PageNameRes,BL %>"
                                TabIndex="20" CommandName="BL" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnPrintShippingDocs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkPrintShippingDocs" Text="<%$resources:PageNameRes,PrintShippingDocs %>"
                                TabIndex="14" CommandName="PRINT" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
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
                            <asp:HiddenField ID="hdfEvaluationNo" runat="server" Value="" />
                            <div class="detail-co3">
                                <div class="div3col-S">
                                    <asp:Label runat="server" ID="lbnCustomer" Text="<%$resources:Controls,CustomerHdr%>"
                                        AssociatedControlID="lblCustomerHdr">
                                    </asp:Label>
                                    <asp:Label runat="server" ID="lblCustomerHdr" CssClass="disp-table"></asp:Label>
                                </div>
                                <div class="div3col-S">
                                    <asp:Label ID="Label1" runat="server" Text="<%$resources:Controls,DestinationPortHdr%>"
                                        AssociatedControlID="lblDestinationPortValue"></asp:Label>
                                    <asp:Label runat="server" ID="lblDestinationPortValue" CssClass="disp-table"></asp:Label>
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
                                            <asp:Label runat="server" ID="lblEvaluationNo" Text="<%$ resources:EvaluationNo %>"
                                                AssociatedControlID="lblEvaluationNoTxt"></asp:Label>
                                            <asp:Label runat="server" ID="lblEvaluationNoTxt" CssClass="select-half"></asp:Label>
                                            <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblDate" Text="<%$ resources:Date %>" AssociatedControlID="txtDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDate" TabIndex="15" MaxLength="12" CssClass="Uidate-picker"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="vrfDate" CssClass="star" SetFocusOnError="true" ValidationGroup="ContainerEvaluation"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtDate" Display="Dynamic"
                                                    Text="*" ErrorMessage="<%$ resources:Err_Date %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="vreDate" CssClass="star" ValidationGroup="ContainerEvaluation"
                                                    runat="server" ControlToValidate="txtDate" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_Date_Valid %>"
                                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                    EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblContainerType" Text="<%$ resources:ContainerType %>"
                                                AssociatedControlID="lblContainerTypeTxt"></asp:Label>
                                            <asp:Label runat="server" ID="lblContainerTypeTxt" CssClass="select-half"></asp:Label>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblDestinationPort" Text="<%$ resources:DestinationPort %>"
                                                AssociatedControlID="lblDestinationPortTxt"></asp:Label>
                                            <asp:Label runat="server" ID="lblDestinationPortTxt" CssClass="select-half"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblCompany" Text="<%$ resources:Company %>" AssociatedControlID="txtCompany"></asp:Label>
                                            <asp:TextBox ID="txtCompany" runat="server" MaxLength="200" TabIndex="16" CssClass="select-half" />
                                            <asp:HiddenField ID="hdfTransCompany" runat="server" />
                                            <asp:HiddenField ID="hdfCompany" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblBookingdate" Text="<%$ resources:BookingDate %>"
                                                AssociatedControlID="txtBookingDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtBookingDate" TabIndex="17" MaxLength="12" CssClass="Uidate-picker"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <div class="clear">
                                                <asp:Label runat="server" ID="lblCarrier" Text="<%$ resources:Carrier %>" AssociatedControlID="ddlCarrier"></asp:Label>
                                                <asp:DropDownList ID="ddlCarrier" runat="server" TabIndex="18" CssClass="select-half-a">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblBookingNo" Text="<%$ resources:BookingNo %>" AssociatedControlID="txtBookingNo"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtBookingNo" TabIndex="19" MaxLength="200" CssClass="select-half"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblContainerNo" Text="<%$ resources:ContainerNo %>"
                                                AssociatedControlID="txtContainerNo"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtContainerNo" TabIndex="20" MaxLength="200" CssClass="select-half"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfContainerNo" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="ContainerEvaluation" EnableClientScript="true" runat="server"
                                                ControlToValidate="txtContainerNo" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ContainerNo %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblTime" Text="<%$ resources:InTime %>" AssociatedControlID="txtInTime"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInTime" TabIndex="21" MaxLength="12" CssClass="Uidate-picker"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="vrfInTime" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="ContainerEvaluation" EnableClientScript="true" runat="server"
                                                    ControlToValidate="txtInTime" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_InTime %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                             <asp:Label runat="server" ID="lblSealNo" Text="<%$ resources:SealNo %>" AssociatedControlID="txtSealNo"></asp:Label>
                                             <asp:TextBox runat="server" ID="txtSealNo" TabIndex="21" MaxLength="200" CssClass="input-small-19-5"></asp:TextBox>
                                        </div>
                                          
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblRemarks" runat="server" Text="<%$ resources:Remarks%>" AssociatedControlID="txtRemarks" />
                                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" MaxLength="500"
                                                TabIndex="22" Width="715" CssClass="multiline-2line" onkeydown="limitText(this,500);"
                                                onkeyup="limitText(this,500);" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <CL1:CheckListItems ID="uclCheckList" runat="server" TabIndex="23" />
                            <%--Print popup window --%>
                            <pc1:PrinterControl ID="PrinterControl1" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell> <br /></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                <h1>
                                    <%= GetLocalResourceObject("Attachment_Details").ToString()%></h1>
                                <div class="button-wrap-right ">
                                    <asp:ImageButton runat="server" ID="imbAddShowDetails" OnClientClick="javascript:return ShowHideAttachmentDetails(1);"
                                        ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                        TabIndex="65" />
                                    <asp:ImageButton runat="server" ID="imbAddHideDetails" OnClientClick="javascript:return ShowHideAttachmentDetails();"
                                        ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                        TabIndex="66" />
                                </div>
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <div id="divUpload">
                                        <table class="table-devide">
                                            <tr style="display: none;">
                                                <td>
                                                    <div class="div2col-S">
                                                        <asp:Label ID="lblType" runat="server" AssociatedControlID="ddlType" Text="<%$ resources:Type %>">
                                                        </asp:Label>
                                                        <asp:DropDownList runat="server" ID="ddlType">
                                                        </asp:DropDownList>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <div class="div2col-S">
                                                        <asp:Label runat="server" ID="lblTitle" Text="<%$ resources:Title %>" AssociatedControlID="txtTitle"></asp:Label>
                                                        <asp:TextBox runat="server" ID="txtTitle" CssClass="select-half"></asp:TextBox>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="div2col-S">
                                                        <asp:Label ID="lblFileUpload" runat="server" Text="AttachFile" AssociatedControlID="fupUpload"></asp:Label>
                                                        <div class="fileupload-main">
                                                            <asp:FileUpload ID="fupUpload" runat="server" CssClass="margn-rgt0 upload-area2" />

                                                            <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star input-medium" SetFocusOnError="true"
                                                                ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupUpload"
                                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_File_Upload %>">
                                                            </asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="btnwrap-divcol" style="padding-right: 0%!important;">
                                                            <asp:Button runat="server" ID="btnAddItem" CommandName="ADDITEM" OnClick="ActionHandler"
                                                                OnClientClick="javascript:ValidatePageNow('upload')" ToolTip="<%$resources:ErpRes,Add %>"
                                                                CommandArgument="PageAction_Entry" ValidationGroup="upload" Text="<%$resources:ErpRes,Add %>"
                                                                SkinID="btnInner-add" />
                                                        </div>
                                                        <div class="clear">
                                                        </div>
                                                        <a id="anchorFile" runat="server" target="_blank"></a>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
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
                                                    <asp:TemplateField HeaderText="<%$ resources:Title %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTitle" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("SCD_TITLE"),40) %>'
                                                                ToolTip='<%# Eval("SCD_TITLE") %>'></asp:Label>
                                                            <asp:HiddenField runat="server" ID="hdfType" Value='<%# Eval("SCD_ITEM") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfPK" Value='<%# Eval("SCD_PK") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="50%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:File %>">
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
                                    </div>
                                </div>
                            </div>
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
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="ContainerEvaluation" runat="server" />
                    <asp:ValidationSummary ID="vsUpload" ValidationGroup="upload" runat="server" />
                    <asp:HiddenField ID="hdfAppType" runat="server" />
                    <asp:HiddenField ID="hdfAppSubType" runat="server" />
                </div>
                <asp:HiddenField ID="hdfDelstatus" runat="server" Value="0" />
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="ContainerEvaluation" />
            </div>
            <asp:HiddenField ID="hdfCompanyValue" runat="server" Value="0" />
            <asp:HiddenField ID="hdfChangeFocus" runat="server" Value="0" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAddItem" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
