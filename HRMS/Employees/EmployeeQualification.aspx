<%@ Page Title="<%$ resources:HRMS-Employee Qualification%>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="EmployeeQualification.aspx.cs" Inherits="HRMS.Employees.EmployeeQualification"
    Theme="ClassicExt" %>

<%@ Register Src="UserControls/GtiTabControl.ascx" TagName="GtiTabControl" TagPrefix="ucGtiTab" %>
<%@ Register Src="UserControls/EmpBasicInfoControl.ascx" TagName="EmpBasicInfoControl"
    TagPrefix="ucBasicHdr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">


//        $(document).ready(function () {
//            ShowListingTab($("[id$=hdnTabStatus]").val());
//            InitDate();
//            InitComponents();
//        });

        function EndRequestHandlerPage() {
            ShowListingTab($("[id$=hdnTabStatus]").val());
            InitDate();
            InitComponents();
        }

        function InitDate() {

            //            GrandScriptUtils.DatePickerCommon("txtPeriodfrom", null, null, true, null, new Date());
            //            GrandScriptUtils.DatePickerCommon("txtTo");
            //            GrandScriptUtils.DatePickerCommon("txtQualifiedOn", null, null, true, null, new Date());
            //            GrandScriptUtils.DatePickerCommon("txtExpiresOn");


            GrandScriptUtils.AddDateRangeCommon("txtPeriodfrom", "hdfPeriodfrom", "txtTo", "hdfTo", false, true);
            GrandScriptUtils.AddDateRangeCommon("txtQualifiedOn", "hdfQualifiedOn", "txtExpiresOn", "hdfExpiresOn", false, true);


        }

        function ShowListingTab(flag) {
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
                $("[id$=pnlCancel]").show();
                if (entryStatus != "1") {
                    $("[id$=pnlSave]").show();
                    $("[id$=pnlSaveAndContinue]").show();
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
        function InitComponents() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtCountry", url, "hdfCountry", true, true, "COUNTRYFIRST");
            InitDate();
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
                $("[id$=pnlSaveAndContinue]").hide();
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlNew]").hide();
                $("[id$=pnlEdit]").hide();
                $("[id$=btnAddItem]").hide(); 
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
                <ucGtiTab:GtiTabControl ID="hrmsTab" CurrentTab="3" runat="server" />
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
                                            OnClientClick="javascript:return ValidatePageNow('Qualification')" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" OnClick="ActionHandler" CommandName="SAVE"
                                            TabIndex="151" Text="<%$ resources:Save%>" ToolTip="<%$ resources:Save%>" ValidationGroup="Qualification"
                                            OnClientClick="javascript:return ValidatePageNow('Qualification')" CommandArgument="SEC_ActionPanel"
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
                <%--  <div class="detail-co3" runat="server" id="divEmployeeHeader">
                    <div class="div3col-S">
                        <asp:Label ID="lblhdrEmployeeNo" runat="server" AssociatedControlID="lblhdrEmployeeNoTxt"
                            Text="<%$ resources:EmployeeNo:%>"></asp:Label>
                        <asp:Label ID="lblhdrEmployeeNoTxt" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblhdrEmployeeName" runat="server" AssociatedControlID="lblhdrEmployeeNameTxt"
                            Text="<%$ resources:EmployeeName:%>"></asp:Label>
                        <asp:Label ID="lblhdrEmployeeNameTxt" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="div3col-S">
                        <asp:Label ID="lblhdrDOJ" runat="server" AssociatedControlID="lblhdrDOJText" Text="<%$ resources:DOJ:%>"></asp:Label>
                        <asp:Label ID="lblhdrDOJText" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblhdrDOB" runat="server" AssociatedControlID="lblhdrDOBTxt" Text="<%$ resources:DOB:%>"></asp:Label>
                        <asp:Label ID="lblhdrDOBTxt" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="div3col-S">
                        <asp:Label ID="lblhdrDesignation" runat="server" AssociatedControlID="lblhdrDesignationTxt"
                            Text="<%$ resources:Designation:%>"></asp:Label>
                        <asp:Label ID="lblhdrDesignationTxt" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblhdrDepartment" runat="server" AssociatedControlID="lblhdrDepartmentTxt"
                            Text="<%$ resources:Department:%>"></asp:Label>
                        <asp:Label ID="lblhdrDepartmentTxt" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="clear">
                    </div>
                </div>--%>
                <asp:HiddenField ID="hdfSelectedItemPk" runat="server" Value="0" />
                <div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="List" CommandArgument="SEC_ActionPanel"
                                CommandName="EMPDOCLIST" CssClass="tab-active" Style="margin-top: -7px;" OnClick="ActionHandler"></asp:LinkButton>
                            <%--OnClientClick="javascript:return ShowListingTab('1')"--%>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="Detail" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CommandName="EMPDOCDETAIL" CssClass="tab-inactive" Style="margin-top: -7px;"></asp:LinkButton>
                            <%-- OnClientClick="javascript:return ShowListingTab('0')"--%>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="gridwrap" style="margin-top: -5px;">
                                <asp:GridView ID="grdEmployeeQualificationlist" runat="server" AutoGenerateColumns="False"
                                    AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="True"
                                    OnSorting="ActionHandler" Width="100%">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    OnCheckedChanged="ActionHandler" />
                                                <asp:HiddenField runat="server" ID="hdfQualfPk" Value='<%# Eval("EQD_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Qualification%> " SortExpression="EQD_QUALIFICATION_TYPE_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblqual" runat="server" ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EQD_QUALIFICATION_TYPE_TEXT")))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EQD_QUALIFICATION_TYPE_TEXT")),60) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Title%> " SortExpression="EQD_TITLE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTitl" runat="server" ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EQD_TITLE")))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EQD_TITLE")),60) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Institute%> " SortExpression="EQD_INSTITUTE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInst" runat="server" ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EQD_INSTITUTE")))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EQD_INSTITUTE")),60) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Qualified On" SortExpression="EQD_QUALIFIED_ON">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInstaa" runat="server" Text='<%# Eval("EQD_QUALIFIED_ON") == "" ? null : Eval("EQD_QUALIFIED_ON", Resources.Constants.HRMSDateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CertificateNo.%> " SortExpression="EQD_CERT_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCertificate" runat="server" ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EQD_CERT_NO")))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EQD_CERT_NO")),60) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="17%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Status%> " SortExpression="EQD_QUAL_STATUS_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSta" runat="server" ToolTip='<%# Eval("EQD_QUAL_STATUS_TEXT") %>'
                                                    Text='<%# Eval("EQD_QUAL_STATUS_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <%--     <asp:TemplateField HeaderText="<%$ resources:Code%> ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblempCode" runat="server" Text='<%# Eval("EQD_TITLE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                                <%--<uc1:PagerControl ID="uclPaging" runat="server" />--%>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide" runat="server" id="pnlEmpDocDetails">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblQualificationType" AssociatedControlID="ddlQualificationType"
                                                Text="<%$ resources:QualificationType*%>"></asp:Label>
                                            <asp:DropDownList ID="ddlQualificationType" runat="server" TabIndex="1" CssClass="select-small-a1">
                                            </asp:DropDownList>
                                            <asp:Label runat="server" ID="lblStatus" AssociatedControlID="ddlStatus" Text="<%$ resources:Status*%>"
                                                CssClass="lbl-16-7perc"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" TabIndex="2" runat="server" CssClass="select-w24per">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblTitle" AssociatedControlID="txtTitle" Text="<%$ resources:Title*%>"></asp:Label>
                                            <asp:TextBox runat="server" TabIndex="3" MaxLength="100" ID="txtTitle" CssClass="input-half" />
                                            <asp:RequiredFieldValidator ID="vrfTITLE" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Qualification" EnableClientScript="true" runat="server" ControlToValidate="txtTitle"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:EnterTitle%>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblPeriodfrom" AssociatedControlID="txtPeriodfrom"
                                                Text="<%$ resources:Periodfrom%>"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPeriodfrom" TabIndex="4" CssClass="input-small"
                                                MaxLength="15" onkeydown="return CheckKey(event)" onpaste="return false;" />
                                            <asp:HiddenField ID="hdfPeriodfrom" runat="server" Value="" />
                                            <asp:Label runat="server" ID="lblTo" AssociatedControlID="txtTo" Text="<%$ resources:To%>"
                                                CssClass="middle-lbl-c"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTo" TabIndex="5" CssClass="input-small" MaxLength="15"
                                                onkeydown="return CheckKey(event)" onpaste="return false;" />
                                            <asp:HiddenField ID="hdfTo" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblQualifiedOn" AssociatedControlID="txtQualifiedOn"
                                                Text="<%$ resources:QualifiedOn%>"></asp:Label>
                                            <asp:TextBox runat="server" TabIndex="6" ID="txtQualifiedOn" CssClass="date-picker input-small"
                                                MaxLength="15" onkeydown="return CheckKey(event)" onpaste="return false;" />
                                            <asp:HiddenField ID="hdfQualifiedOn" runat="server" Value="" />
                                            <asp:Label runat="server" ID="lblExpiresOn" AssociatedControlID="txtExpiresOn" Text="<%$ resources:ExpiresOn%>"
                                                CssClass="middle-lbl-small-e1"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtExpiresOn" TabIndex="7" CssClass="date-picker input-small"
                                                MaxLength="15" onkeydown="return CheckKey(event)" onpaste="return false;" />
                                            <asp:HiddenField ID="hdfExpiresOn" runat="server" Value="" />
                                            <%-- <asp:CompareValidator ID="cvtxtStartDate" runat="server" ControlToCompare="hdfEmployeeQDOB"
                                                CultureInvariantValues="true" Display="Dynamic" EnableClientScript="true" ControlToValidate="txtPeriodfrom"
                                                ErrorMessage="Period from must after DOB" Type="Date" ValidationGroup="upload" SetFocusOnError="true"
                                                Operator="GreaterThanEqual" Text="Start date must be earlier than finish date"></asp:CompareValidator>--%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblCertificateNo" AssociatedControlID="txtCertificateNo"
                                                Text="<%$ resources:CertificateNo%>"></asp:Label>
                                            <asp:TextBox runat="server" MaxLength="100" TabIndex="8" ID="txtCertificateNo" CssClass="input-small" />
                                            <asp:Label runat="server" ID="lblCountry" AssociatedControlID="txtCountry" Text="<%$ resources:Country%>"
                                                CssClass="lbl-14-7perc"></asp:Label>
                                            <asp:TextBox runat="server" TabIndex="9" ID="txtCountry" Text="" MaxLength="100"
                                                CssClass="select-small-c"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCountry" Value="" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblHighestQualification" AssociatedControlID="lblHighestQualification"
                                                Text="<%$ resources:HighestQualification%>"></asp:Label>
                                            <asp:CheckBox runat="server" TabIndex="10" ID="chkHighestQualf" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblIssuedBy" runat="server" Text="<%$ resources:IssuedBy%>" AssociatedControlID="txtIssuedBy"
                                                CssClass="margn-rgt0"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtIssuedBy" MaxLength="200" TabIndex="11"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblInstitute" runat="server" Text="<%$ resources:Institute%>" AssociatedControlID="txtInstitute"
                                                CssClass="margn-rgt0"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInstitute" MaxLength="200" TabIndex="12"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblRemarks" runat="server" Text="<%$ resources:Remarks%>" AssociatedControlID="txtRemarks"
                                                CssClass="margn-rgt0"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRemarks" MaxLength="500" TabIndex="13" TextMode="MultiLine"
                                                CssClass="multiline-2col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblIntimateBefore" AssociatedControlID="txtIntimateBefore"
                                                CssClass="margn-rgt4" Text="<%$ resources:IntimateBefore%>"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtIntimateBefore" TabIndex="14" MaxLength="3" onkeypress="return isNumber(event)"
                                                CssClass="small" />
                                            <span style="background: none; border: 0; height: 2px;" id="lblSpan">Days</span>
                                            <asp:Label runat="server" ID="lblActive" AssociatedControlID="lblActive" Text="<%$ resources:Active%>"
                                                CssClass="lbl-20-8perc"></asp:Label>
                                            <asp:CheckBox runat="server" TabIndex="15" ID="chkActive" />
                                            <div class="clear">
                                            </div>
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
                                            <asp:Label ID="lblFileUpload" runat="server" Text="Attach File" AssociatedControlID="fupUpload"
                                                CssClass="margn-rgt0"></asp:Label>
                                            <asp:FileUpload ID="fupUpload" runat="server" TabIndex="16" Style="width: 15.6%;" />
                                            <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupUpload"
                                                Display="Dynamic" Text="*" ErrorMessage="Please Select  File">
                                            </asp:RequiredFieldValidator>
                                            <a id="anchorFile" runat="server" target="_blank"></a>
                                            <asp:Button runat="server" ID="btnAddItem" CommandName="ADDITEM" TabIndex="17" OnClick="ActionHandler"
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
                                                AllowPaging="false" OnSorting="ActionHandler" OnPageIndexChanging="ActionHandler"
                                                OnRowDataBound="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                                <%-- OnSorting="ActionHandler"
                                                OnPageIndexChanging="ActionHandler" OnRowDataBound="ActionHandler" --%>
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sl No">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                            <asp:HiddenField runat="server" ID="hdfPK" Value='<%# Eval("DOC_PK") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfSlNo" Value='<%# Container.DataItemIndex + 1 %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="4%" Wrap="false" />
                                                        <ItemStyle Width="4%" HorizontalAlign="Center" Wrap="false" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="File">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFile" runat="server" Text='<%# Eval("DOC_NAME") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="94%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-CssClass="file-details">
                                                        <ItemTemplate>
                                                            <a runat="server" id="fileView" class="download-icon nomargin" title="View" target="_blank"
                                                                href='<%# Page.ResolveClientUrl(Eval("DOC_PATH").ToString()) %>'></a>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false" ItemStyle-CssClass="file-details">
                                                        <ItemTemplate>
                                                            <asp:Button ID="lnkEdit" runat="server" CommandName="EDITITEM" SkinID="edit-icon"
                                                                ToolTip="Edit" OnClick="ActionHandler" CommandArgument="PageAction_Entry" />
                                                            <%--OnClick="ActionHandler"--%>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-CssClass="file-details">
                                                        <ItemTemplate>
                                                            <asp:Button ID="lnkRemove" runat="server" CommandName="REMOVEITEM" SkinID="delete-icon"
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
                                <asp:ValidationSummary ID="vsPageSave" ValidationGroup="Qualification" runat="server" />
                                <asp:ValidationSummary ID="vsImageUpload" ValidationGroup="upload" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <span style="float: right !important;" id="lblLastModifiedDate" runat="server"></span>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:HiddenField ID="hdfEmployeeQDOB" runat="server" Value="" />
                <asp:HiddenField ID="hdnTabStatus" runat="server" Value="0" />
                <asp:HiddenField ID="hdfEntryStatus" runat="server" Value="0" />
            </div>
        </ContentTemplate>
        <Triggers>
            <%-- <asp:AsyncPostBackTrigger ControlID="btnAddItem" />--%>
            <asp:PostBackTrigger ControlID="btnAddItem" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
