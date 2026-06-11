<%@ Page Title="<%$ Resources:Captions,Title_FundRequisitionDept %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" Theme="ClassicExt" AutoEventWireup="true"
    CodeBehind="FundRequisitionDept.aspx.cs" Inherits="ERPSMS_v01.GeneralAdmin.FundRequisitionDept" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var NumberDigits = 0;
        var CurrencyDigits = 0;
        $(document).ready(function () {
            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
        });
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtListFromDate", "hdfListFromDate", "txtListToDate", "hdfListToDate", false, false);
            GrandScriptUtils.DatePickerCommon("txtDate");
            GrandScriptUtils.MakeAutoCompleteDDL("txtListTrxNo", url + "?Type=DFH_NO", "hdfTrxPk", true, true, "GETFUNDREQUISITIONNOAUTO");
            var SelectVal = 0;
            var expenseHeadURL = "CommonManagement.do?Action=GetConstantValueAuto&Pk=" + SelectVal + "&GroupTypeConst=43&GroupConstant=1";
            GrandScriptUtils.MakeAutoCompleteDDL("txtHead", expenseHeadURL, "hdfHead", true, true);

            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();
            CalculateTotalAppAmt();
        }
        function ShowListing(flag) {
            if (flag) {
                $("[id$='PageAction_List']").show();
                $("[id$='PageAction_Entry']").hide();
                $("[id$='pnlListing']").show();
                $("[id$='pnlEntry']").hide();
                $("[id$=ddlCompany]").hide();
            }
            else {
                $("[id$='PageAction_List']").hide();
                $("[id$='PageAction_Entry']").show();
                $("[id$='pnlListing']").hide();
                $("[id$='pnlEntry']").show();
                $("[id$=ddlCompany]").show();
                ShowHideDtls(1);
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

        function ShowHideAdvancedSearch(flag) {
            //If flag then hide AdvancedSearch
            if (flag) {
                $("[id$=tbladvancedSearch]").hide();
                $("[id$=imbShowFilter]").show();
                $("[id$=imbHideFilter]").hide();
            }
            else {
                $("[id$=tbladvancedSearch]").show();
                $("[id$=imbShowFilter]").hide();
                $("[id$=imbHideFilter]").show();
            }
            return false;
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
        //Validation Summary
        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                CheckValidationDuplicate(valGroup);
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

        function ShowHideDtls(flag) {
            //If flag then Show Items           
            if (flag) {            
                $("[id$=divFrqDtls]").show();              
                $("[id$=dvRequest]").show();
                $("[id$=imbShowItemDetails]").hide();
                $("[id$=imbHideItemDetails]").show();
            }
            else {              
                $("[id$=divFrqDtls]").hide();              
                $("[id$=dvRequest]").hide();
                $("[id$=imbShowItemDetails]").show();
                $("[id$=imbHideItemDetails]").hide();
            }
            return false;
        }

        function CalculateTotalAppAmt() {
            var Amount = 0;
            var flag = false;
            $("#[id*=grdFrqDtls] input[type=text][id*=txtAppAmount]").each(function (index) {
                if ($.trim($(this).val()) != "") {
                    //Check if number is a valid integer
                    if (!isNaN(parseFloat($(this).val()))) {
                        Amount = Amount + parseFloat($(this).val());
                        flag = true;
                    }
                }
            });

            if (flag == false) {
                Amount = -1;
            }
            if (Amount < 0) {
                var AppAmount = parseFloat($("#[id*=grdFrqDtls] [id*=lblTotalAppAmount]").html());
            }
            else {
                $("#[id*=grdFrqDtls] [id*=lblTotalAppAmount]").html(Amount.toFixed(CurrencyDigits));
                $("[id$=hdfTotalAppAmount]").val(Amount.toFixed(CurrencyDigits));               
            }

        }

        function ShowHideUploadDocDetails(flag) {
            //If flag then Show Upload Doc Details
            if (flag) {
                $("[id$=tblUploadDocDetails]").show();
                $("[id$=imbShowDetails]").hide();
                $("[id$=imbHideDetails]").show();
            }
            else {
                $("[id$=tblUploadDocDetails]").hide();
                $("[id$=imbShowDetails]").show();
                $("[id$=imbHideDetails]").hide();
            }
            return false;
        }

        //Disable Approved Amt (Should not allow to edit Approved amount during the time of resubmission)
        function DisableApprovedAmount() {
            $("#[id*=grdFrqDtls] input[type=text][id*=txtAppAmount]").each(function (index) {
                $(this).attr("disabled", true);
                $(this).removeClass("input-w81per numeric").addClass("input-w81per numeric input-disabled");
            });
        }         
                    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlBasicInfo">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                               <div class="buttoncontainer-fields floatLeft" style="display:none">
                                    <asp:DropDownList ID="ddlCompany" class="select-full-a margnbotm0" runat="server"
                                        onmouseover="javascript:ShowTooltip('ddlCompany');">
                                    </asp:DropDownList>
                                </div>
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum" Text="<%$ resources:Breadcrumb%>"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="147"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="148"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('Save')"
                                            ValidationGroup="Save" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="149" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('Save')" ValidationGroup="Save"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
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
                                    <li runat="server" id="pnlPrint">
                                        <asp:Button runat="server" ID="btnPrint" CommandName="PRINT" TabIndex="152" Text="<%$resources:Controls,Print %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            CssClass="popupclose" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" OnClick="ActionHandler"
                                            TabIndex="154" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="155" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="155" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelFRD %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelFRD %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="156" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="155" ID="btnView" CommandName="VIEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,View %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnPrintList" CommandName="PRINT" TabIndex="156" Text="<%$resources:Controls,Print %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
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
                <asp:Table runat="server" ID="tblPage" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:ShowFilter%>" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S padgtop7 ">
                                            <asp:Label ID="lblFromDate" runat="server" Text="<%$ resources:FromDate%>" AssociatedControlID="txtListFromDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtListFromDate" TabIndex="1" onkeydown="return CheckKey(event)"
                                                onpaste="return false;" CssClass="input-small-b"></asp:TextBox>
                                            <asp:HiddenField ID="hdfListFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$ resources:ToDate%>" AssociatedControlID="txtListToDate"
                                                CssClass="middle-lbl-xsmall-e"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtListToDate" TabIndex="1" CssClass="input-small-b"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfListToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7 ">
                                            <%-- <asp:Label ID="Label2" runat="server" Text="<%$ resources:ServiceType%>" CssClass="middle-lbl-small"
                                                AssociatedControlID="ddlListServiceType"></asp:Label>
                                            <asp:DropDownList ID="ddlListServiceType" TabIndex="2" runat="server" CssClass="select-half-a">
                                            </asp:DropDownList>--%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblListTrxNo" runat="server" Text="<%$ resources:TrxNo%>" CssClass="margnbotm0"
                                                AssociatedControlID="txtListTrxNo"></asp:Label>
                                            <asp:TextBox ID="txtListTrxNo" runat="server" CssClass="input-small-b margnbotm0"
                                                TabIndex="3"></asp:TextBox>
                                            <asp:HiddenField ID="hdfTrxPk" runat="server" />
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" CssClass="middle-lbl-xsmall-e margnbotm0"
                                                AssociatedControlID="ddlTrnStatus"></asp:Label>
                                            <%--<asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-w21-8per margnbotm0"
                                                TabIndex="3">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Approved %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Submitted %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="4"></asp:ListItem>
                                            </asp:DropDownList>--%>
                                            <asp:DropDownList ID="ddlTrnStatus" runat="server" CssClass="select-small-g margnbotm0" TabIndex="1" >
                                             </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">                                            
                                            <asp:Label ID="lblReqDeptAdvSearch" runat="server" Text="<%$ resources:RequestedDept%>"  CssClass="middle-lbl-small margnbotm0"
                                                AssociatedControlID="ddlRequestedDeptAdvSearch"></asp:Label>
                                            <asp:DropDownList ID="ddlRequestedDeptAdvSearch" TabIndex="4" runat="server" CssClass="select-half-a margnbotm0">
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="imgbtnLstSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="4"
                                                CommandName="FILTER" SkinID="search-ext" CssClass="margnbotm0" />
                                            <asp:ImageButton ID="imgbtnLstClear" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="4" OnClick="ActionHandler"
                                                CommandName="CLEARSEARCH" SkinID="clear-ext" CssClass="margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdList" Width="100%" AllowPaging="false" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" OnSorting="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" AutoPostBack="true"
                                                    OnCheckedChanged="ActionHandler" GroupName="SelectOne" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    TabIndex="10" />
                                                <asp:HiddenField runat="server" ID="hdfDFH_PK" Value='<%# Eval("DFH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval("DFH_STATUS") %>' />
                                                <asp:HiddenField ID="hdfDelStatus" runat="server" Value='<%# Eval("DFH_IS_DELETED") %>' />
                                                <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval("DFH_DEPT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                            <HeaderStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvDate" runat="server" Text='<%#  Eval("DFH_DATE")!=""? Convert.ToDateTime( Eval("DFH_DATE")).ToString(Resources.Constants.ReportDateFormat):"" %>'
                                                    ToolTip='<%# Eval("DFH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TrxNo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("DFH_NO")))?Resources.ErpRes.Draft:Eval("DFH_NO")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("DFH_NO")))?Resources.ErpRes.Draft:Eval("DFH_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                            <HeaderStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:RequestedDept%>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRequestedDept" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("DFH_REQ_DEPT_TEXT")),35) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("DFH_REQ_DEPT_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="47%" />
                                            <ItemStyle Width="47%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:RequAmount %>">
                                             <ItemTemplate>                                                              
                                               <asp:Label ID="lblReqstdAmount" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetFormattedCurrencyGrid(Eval("DFH_TOTAL")) %>'
                                                 ToolTip='<%# ERP.Utilities.CommonFunctions.GetFormattedCurrencyGrid(Eval("DFH_TOTAL")) %>'></asp:Label>
                                             </ItemTemplate>
                                             <ItemStyle HorizontalAlign="Right"  Width="15%" />                                                         
                                              <HeaderStyle CssClass="amount-numeric" />                                             
                                         </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                               <asp:Button ID="imgStatus" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("DFH_CSS_CLASS") %>' ToolTip='<%# Eval("DFH_STATUS_TEXT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval("DFH_STATUS") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="8%" />
                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" TabIndex="11" />
                                <div class="clear">
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide" id="tblDetailHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblTrxNoHdr" runat="server" Text="<%$ resources:TrxNo%>" AssociatedControlID="lblTrxNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblTrxNo" CssClass="input-small"></asp:Label>
                                            <asp:Label ID="lblDate" runat="server" Text="<%$ resources:DateStar%>" AssociatedControlID="txtDate"
                                                CssClass="lbl-21-5perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDate" TabIndex="4" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDate"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_EnterDate%>"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDate"
                                                CssClass="star" ValidationGroup="add" Text="*" ErrorMessage="<%$ resources:Err_EnterDate%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblRequestedDept" runat="server" Text="<%$ resources:RequestedDept%>"
                                                AssociatedControlID="ddlRequestedDept"></asp:Label>
                                            <asp:DropDownList ID="ddlRequestedDept" TabIndex="4" runat="server" CssClass="select-half-a">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="reqRequestedDept" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" runat="server" ControlToValidate="ddlRequestedDept" Display="Dynamic"
                                                Text="*" InitialValue="0" ErrorMessage="<%$ resources:Err_SelectRequestedDept %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblworkDesc" Text="<%$ resources:Description%>" AssociatedControlID="txtDescription"></asp:Label>
                                            <asp:TextBox ID="txtDescription" runat="server" TabIndex="5" MaxLength="500" TextMode="MultiLine"
                                                Height="30" CssClass="input-full" onkeyup="limitText(this,500);"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                           
                             <div class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("ReqDetails").ToString() + " :"%></h1>
                                <asp:ImageButton runat="server" ID="imbShowItemDetails" OnClientClick="javascript:return ShowHideDtls(1);"
                                    SkinID="imbArrowInactive" ToolTip="<%$ resources:ShowDetails %>" TabIndex="9" />
                                <asp:ImageButton runat="server" ID="imbHideItemDetails" OnClientClick="javascript:return ShowHideDtls();"
                                    Style="display: none" SkinID="imbArrowActive" ToolTip="<%$ resources:HideDetails %>"
                                    TabIndex="9" />
                                <asp:HiddenField ID="hdfIsItemDetailsVisible" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                            </div>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <div class="table-forms" id="dvRequest" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblHead" runat="server" AssociatedControlID="txtHead" Text="<%$ resources:Head %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDtlDesc" runat="server" AssociatedControlID="txtDtlDesc" Text="<%$ resources:ReqDesc %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDtlAmount" runat="server" AssociatedControlID="txtDtlAmount" Text="<%$ resources:ReqAmount %>"></asp:Label>
                                                        </td>                                                         
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtHead" runat="server" TabIndex="6" Width="340px"></asp:TextBox>
                                                            <asp:HiddenField ID="hdfHead" runat="server" />
                                                            <div class="starwrap">
                                                                <asp:RequiredFieldValidator ID="vrfHead" CssClass="star" SetFocusOnError="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                                    ValidationGroup="dtl" EnableClientScript="true" runat="server" ControlToValidate="txtHead"
                                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_Head %>">
                                                                </asp:RequiredFieldValidator>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDtlDesc" MaxLength="200" TabIndex="6" onkeydown="limitText(this,200);"
                                                                onkeyup="limitText(this,200);" Width="625px"></asp:TextBox>
                                                            <div class="starwrap">
                                                                <asp:RequiredFieldValidator ID="vrfDtlDesc" CssClass="star" SetFocusOnError="true"
                                                                    ValidationGroup="dtl" EnableClientScript="true" Text="*" runat="server" ControlToValidate="txtDtlDesc"
                                                                    Display="Dynamic" ErrorMessage="<%$ resources:Msg_Desc %>"></asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator ID="vreDtlDesc" runat="server" ControlToValidate="txtDtlDesc"
                                                                    ErrorMessage="<%$ resources:Msg_ReqDesc %>" ValidationExpression="^[\s\S]{0,200}$"
                                                                    Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="dtl"></asp:RegularExpressionValidator>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDtlAmount" MaxLength="12" TabIndex="6" Width="128px"  CssClass="numeric"></asp:TextBox>
                                                            <div class="starwrap">
                                                                <asp:RequiredFieldValidator ID="vrfDtlAmount" CssClass="star" SetFocusOnError="true"
                                                                    ValidationGroup="dtl" EnableClientScript="true" Text="*" runat="server" ControlToValidate="txtDtlAmount"
                                                                    Display="Dynamic" ErrorMessage="<%$ resources:Msg_ReqAmount %>"></asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator ID="vreDtlAmount" runat="server" ControlToValidate="txtDtlAmount"
                                                                    ErrorMessage="<%$ resources: Msg_ReqAmount_Valid %>" ValidationExpression="^([0-9]{0,14})?(\.[0-9]{0,2})?$"
                                                                    SetFocusOnError="true" Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star"
                                                                    ValidationGroup="dtl">
                                                                </asp:RegularExpressionValidator>
                                                            </div>
                                                        </td>                                                       
                                                        <td>
                                                            <asp:ImageButton runat="server" ID="btnAddDtl" TabIndex="7" ValidationGroup="dtl"
                                                                OnClick="ActionHandler"  CssClass="margn-rgt4" SkinID="plus" OnClientClick="javascript:ValidatePageNow('dtl')"
                                                                CommandName="ADD" ToolTip="<%$resources:ErpRes,Add %>"  /> 
                                                            <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear%>"
                                                                ToolTip="<%$ resources:Controls,Clear%>" TabIndex="7" OnClick="ActionHandler"
                                                                CommandName="CLEARADD" SkinID="clear-ext" CssClass="margn-rgt4" />                                                           
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>                                        
                                            <div id="divFrqDtls" class="gridwrap">
                                                <asp:GridView runat="server" ID="grdFrqDtls" Width="100%" AutoGenerateColumns="false"
                                                    OnRowDataBound="ActionHandler" EmptyDataRowStyle-CssClass="emptytable" AllowPaging="false"
                                                    AllowSorting="false" ShowFooter="true">
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ resources:ErpRes,Msg_EmptyGrid %>"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <Columns>                                                       
                                                        <asp:TemplateField HeaderText="<%$ resources:Head %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblHead" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("DFD_HEAD_TEXT")),35) %>'
                                                           ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("DFD_HEAD_TEXT")))%>'></asp:Label>
                                                                <asp:HiddenField ID="hdfHead" runat="server" Value='<%#  Eval("DFD_HEAD") %>' /> 
                                                                 <asp:HiddenField runat="server" ID="hdfDFDPK" Value='<%# Eval("DFD_PK") %>' />
                                                                    <asp:HiddenField runat="server" ID="hdfDFDSlNo" Value='<%# Eval("DFD_SEQUENCE") %>' />                                                            
                                                            </ItemTemplate>  
                                                             <ItemStyle Width="25%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Description %>">
                                                            <ItemTemplate>                                                              
                                                                <asp:Label ID="lblDtlDescLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetDecodedString( Eval("DFD_DESC")) %> '
                                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString( Eval("DFD_DESC")) %> '></asp:Label>
                                                            </ItemTemplate>   
                                                              <ItemStyle Width="27%" />                                                       
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:RequAmount %>">
                                                            <ItemTemplate>                                                              
                                                              <asp:Label ID="lblRequAmount" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetFormattedCurrencyGrid(Eval("DFD_REQ_AMT")) %>'
                                                                ToolTip='<%# ERP.Utilities.CommonFunctions.GetFormattedCurrencyGrid(Eval("DFD_REQ_AMT")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right"  Width="10%" />                                                         
                                                             <HeaderStyle CssClass="amount-numeric" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <asp:Label runat="server" ID="lblTotalRequAmount"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:AppdAmount %>">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAppAmount" runat="server" TabIndex="8" MaxLength="15" Text='<%# GetFormattedCurrency(Eval("DFD_APP_AMT")) %>'
                                                                    CssClass="input-w81per numeric" onkeyup="CalculateTotalAppAmt();"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="vrfAppAmount" CssClass="star" SetFocusOnError="true" 
                                                                    ValidationGroup="Save" EnableClientScript="true" Text="*" runat="server" ControlToValidate="txtAppAmount"
                                                                    Display="Dynamic" ErrorMessage="<%$ resources:Msg_AppAmount %>"></asp:RequiredFieldValidator>
                                                                  <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="star" SetFocusOnError="true"  InitialValue="0"
                                                                    ValidationGroup="Save" EnableClientScript="true" Text="*" runat="server" ControlToValidate="txtAppAmount"
                                                                    Display="Dynamic" ErrorMessage="<%$ resources:Msg_AppAmount %>"></asp:RequiredFieldValidator>--%>
                                                                <asp:RegularExpressionValidator ID="vreAppAmount" runat="server" ControlToValidate="txtAppAmount"
                                                                    ErrorMessage="<%$ resources: Msg_AppAmount_Valid %>" ValidationExpression="^\$?([0-9]{0,14})?(\.[0-9]{0,2})?$"
                                                                    SetFocusOnError="true" Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star"
                                                                    ValidationGroup="Save">
                                                                </asp:RegularExpressionValidator>                                                              
                                                                <%--<asp:Label ID="lblAppAmount" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetFormattedCurrencyGrid(Eval("DFD_APP_AMT")) %>'></asp:Label>--%>
                                                            </ItemTemplate>
                                                             <ItemStyle HorizontalAlign="Right"  Width="13%" />    
                                                            <HeaderStyle CssClass="amount-numeric" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <asp:Label runat="server" ID="lblTotalAppAmount"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>                                                        
                                                        <asp:TemplateField HeaderText="<%$ resources:Remarks %>">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtRemarks" runat="server" TabIndex="8" MaxLength="200"  CssClass="input-w98per" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("DFD_REMARKS"),70) %>'
                                                                     ToolTip='<%#HttpUtility.HtmlDecode(Convert.ToString(Eval("DFD_REMARKS"))) %>'></asp:TextBox>
                                                                     
                                                                <div class="starwrap">
                                                                    <asp:RequiredFieldValidator ID="vrfRemarks" CssClass="star" SetFocusOnError="true"
                                                                        InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>" ValidationGroup="fundUtiDet"
                                                                        EnableClientScript="true" runat="server" ControlToValidate="txtRemarks" Display="Dynamic"
                                                                        Text="*" ErrorMessage="<%$ resources:Msg_Remarks %>">
                                                                    </asp:RequiredFieldValidator>
                                                                 </div>
                                                            </ItemTemplate>   
                                                              <ItemStyle Width="20%" />                                                       
                                                            <FooterTemplate>
                                                            </FooterTemplate>
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imbItemEdit" runat="server" CommandName="EDITITEM" SkinID="imbeditgrid"
                                                                        CommandArgument='<%# Eval("DFD_SEQUENCE") %>' OnClick="ActionHandler" ToolTip="<%$resources:Controls,Edit %>"
                                                                        TabIndex="9" />
                                                                    <asp:ImageButton ID="imbItemDelete" runat="server" CommandName="REMOVEITEM" SkinID="imbdeletegrid"
                                                                        OnClick="ActionHandler" CommandArgument='<%# Eval("DFD_SEQUENCE") %>' ToolTip="<%$resources:Controls,Delete %>"
                                                                        OnClientClick="return ShowDeleteConfirm(this);" TabIndex="9" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="5%" HorizontalAlign="Right" CssClass="actn-btn-container-2" />
                                                                <HeaderStyle CssClass="actn-btn-container-2" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>                                            
                                            <asp:HiddenField ID="hdfHasDetails" Value="0" runat="server" />
                                            <asp:HiddenField ID="HiddenField1" Value="0" runat="server" />
                                           
                                        </td>
                                    </tr>
                                </table>

                            <%-- file upload start here--%>
                              <div class="search-colapse-b">
                                <h1> <%=Resources.Controls.Attachments%> </h1>
                                 <div class="button-wrap-right ">
                                    <asp:ImageButton runat="server" ID="imbShowDetails" OnClientClick="javascript:return ShowHideUploadDocDetails(1);"
                                        ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                        TabIndex="65" />
                                    <asp:ImageButton runat="server" ID="imbHideDetails" OnClientClick="javascript:return ShowHideUploadDocDetails();"
                                        ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                        TabIndex="66" />
                                 </div>
                                       
                                  <div class="clear">
                                  </div>
                               </div>
                                <div class="fields-group">
                                    <table class="table-devide" id="tblUploadDocDetails">
                                        <tr>
                                            <td colspan="2">
                                    <div class="divcol-S">
                                        <asp:Label ID="lblFileUpload" runat="server" Text="AttachFile" AssociatedControlID="fupUpload"></asp:Label>
                                        <asp:FileUpload ID="fupUpload" runat="server" TabIndex="26" Style="width: 15.6%;" />
                                        <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupUpload"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_File_Upload %>">
                                                        
                                        </asp:RequiredFieldValidator>
                                        <a id="anchorFile" runat="server" target="_blank" tabindex="11"></a>
                                        <asp:Button runat="server" ID="btnUpload" CommandName="ADDITEMUPLOAD" TabIndex="26"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('upload')"
                                            ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="PageAction_Entry" ValidationGroup="upload"
                                            Text="<%$resources:ErpRes,Add %>" SkinID="btnInner-add" />                                        
                                        <div class="clear">
                                        </div>
                                    </div>
                                     </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdUploads" Width="100%" PageSize="<%$ resources:PageSize%>"
                                            AllowSorting="false" AllowPaging="false" OnSorting="ActionHandler" OnPageIndexChanging="ActionHandler"
                                            OnRowDataBound="ActionHandler" AutoGenerateColumns="false" TabIndex="27" EmptyDataRowStyle-CssClass="emptytable">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:SlNo %>">
                                                    <ItemTemplate>                                                       
                                                        <asp:HiddenField runat="server" ID="hdfPK" Value='<%# Eval("DOC_PK") %>' />
                                                         <asp:Label ID="lblSlno" runat="server" Text='<%# Eval("DOC_SEQ_NO") %>' ToolTip='<%# Eval("DOC_SEQ_NO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="4%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:File %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFile" runat="server" Text='<%# Eval("DOC_NAME") %>' ToolTip='<%# Eval("DOC_NAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="90%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <a runat="server" id="fileView" class="download-icon nomargin" title="<%$ resources:View %>"
                                                            target="_blank" href='<%# Page.ResolveClientUrl(Eval("DOC_PATH").ToString()) %>'>
                                                        </a>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="2%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="lnkEdit" runat="server" OnClick="ActionHandler" CommandName="EDITITEMUPLOAD" TabIndex="27"
                                                            SkinID="edit-icon" ToolTip="Edit" CommandArgument="PageAction_Entry" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="2%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEMUPLOAD" TabIndex="27"
                                                            SkinID="delete-icon" ToolTip="Delete" CommandArgument="PageAction_Entry" OnClientClick="return ShowDeleteConfirm(this);"  />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="2%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                 </td>
                                        </tr>
                                    </table>
                                </div>
                           
                        <%-- file upload end here--%>

                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverrorAlert" style="display: none">
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
                <asp:ValidationSummary ID="vsAddToList" ValidationGroup="add" runat="server" />
                <asp:ValidationSummary ID="vsReq" ValidationGroup="frq" runat="server" />
                <asp:ValidationSummary ID="vsDtl" ValidationGroup="dtl" runat="server" />                
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="SaveAddDed">
                </uc1:WorkflowUserComments>
            </div>
            <asp:HiddenField ID="hdfRateFormat" runat="server" />
            <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
            <asp:HiddenField ID="hdfCurrencyDigits" runat="server" Value="3" />
            <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
            <asp:HiddenField ID="hdfDecimalFormatWithSeperation" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormatWithComma" runat="server" />
            <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
            <asp:HiddenField ID="hdfJournalizeWorkFlow" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIsCancelled" Value="0" runat="server" />
            <asp:HiddenField ID="hdfExchangeRateFormat" runat="server" />
            <asp:HiddenField ID="hdfExchangeRate" runat="server" />

              <asp:HiddenField runat="server" ID="hdfTotalAppAmount" />
            <asp:HiddenField runat="server" ID="hdfTotalReqAmount" />
        </ContentTemplate>
         <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
