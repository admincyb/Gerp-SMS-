<%@ Page Title="<%$ Resources:Captions,Title_VoucherSearch %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="VoucherSearch.aspx.cs" Inherits="ERPSMS_v01.Journalize.VoucherSearch"
    Theme="Classic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            ShowHideMultVoucherPrint($("[id$=hdfMultPrintVisible]").val());
        }
        function ShowHideMultVoucherPrint(flag) {
            ///<summary>
            /// Used to Show/Hide ItemDetails Div
            ///</summary>

            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divVoucherPrint]").show();
                $("[id$=imbShowMultVoucherPrint]").hide();
                $("[id$=imbHideMultVoucherPrint]").show();
            }
            else {
                $("[id$=divVoucherPrint]").hide();
                $("[id$=imbShowMultVoucherPrint]").show();
                $("[id$=imbHideMultVoucherPrint]").hide();
            }
            $("[id$=hdfMultPrintVisible]").val(flag);
            return false;
        }
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
    <asp:UpdatePanel runat="server" ID="upVoucherSearch">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblVoucherNo" runat="server" Text="<%$resources:VoucherNo %>" AssociatedControlID="txtVoucherNo"></asp:Label>
                                            <asp:TextBox ID="txtVoucherNo" runat="server" TabIndex="1" CssClass="input-medium margnbotm0"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfVoucherNo" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="ValSearch" EnableClientScript="true" runat="server" ControlToValidate="txtVoucherNo"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_VoucherNo %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="2" CommandName="SEARCH" SkinID="search-ext"
                                                OnClientClick="javascript:ValidatePageNow('ValSearch')" CssClass="margntop2 margnbotm0"
                                                ValidationGroup="ValSearch" />
                                            <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="3" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>

                <div class="search-colapse-b" style="margin-top: 3%">
                    <h1>
                        <%= GetLocalResourceObject("MultipleVoucherPrint").ToString() + " :"%></h1>
                    <asp:ImageButton runat="server" ID="imbShowMultVoucherPrint" OnClientClick="javascript:return ShowHideMultVoucherPrint(1);"
                        SkinID="imbArrowInactive" ToolTip="<%$ resources:ShowVoucherPrint %>" TabIndex="9" />
                    <asp:ImageButton runat="server" ID="imbHideMultVoucherPrint" OnClientClick="javascript:return ShowHideMultVoucherPrint();"
                        Style="display: none" SkinID="imbArrowActive" ToolTip="<%$ resources:HideVoucherPrint %>"
                        TabIndex="9" />
                    <asp:HiddenField ID="hdfMultPrintVisible" runat="server" Value="0" />
                    <div class="clear">
                    </div>
                </div>
                <div class="gridwrap" id="divVoucherPrint">
                    <div id="divPrint">
                        <table class="table-devide">
                            <tr id="Tr1" runat="server">
                                <td>
                                    <div class="div2col-S padgtop7">
                                        <asp:Label ID="lblFrmDate" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                        <asp:TextBox ID="txtFromDate" runat="server" TabIndex="1" CssClass="input-small margnbotm5"
                                            MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                        <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                        <asp:Label ID="lblToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtToDate"
                                            CssClass="middle-lbl-a"></asp:Label>
                                        <asp:TextBox ID="txtToDate" runat="server" TabIndex="2" CssClass="input-small margnbotm0" MaxLength="11"
                                            onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                        <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S padgtop7">
                                        <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" AssociatedControlID="ddlStatus"></asp:Label>
                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="input-small-19-04-21 margnbotm5" TabIndex="3">
                                            <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="0"></asp:ListItem>
                                            <%--<asp:ListItem Text="<%$ Resources:Captions,Posted %>" Value="1"></asp:ListItem>--%>
                                            <asp:ListItem Text="<%$ Resources:Captions,Submitted %>" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Captions,Verified %>" Value="12"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Captions,Approved %>" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="4"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="div2col-S padgtop7">
                                        <asp:Label ID="lblVoucherType" runat="server" Text='<%$Resources:Controls,VoucherType %>'
                                            AssociatedControlID="ddlVoucherType"></asp:Label>
                                        <asp:DropDownList ID="ddlVoucherType" runat="server" TabIndex="18" CssClass="select-half" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                            
                                            <asp:ListItem Text="<%$ Resources:Captions,VTypeCNJPI %>" Value="CNJPI"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Captions,VTypeCNJSI %>" Value="CNJSI"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Captions,VTypeCWIPJ %>" Value="CWIPJ"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Captions,VTypeDNJPI %>" Value="DNJPI"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Captions,VTypeDNJSI %>" Value="DNJSI"></asp:ListItem> 
                                            <asp:ListItem Text="<%$ Resources:Captions,VTypeDPRJ %>" Value="DPRJ"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Captions,VTypeDPV %>" Value="DPVJ"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Captions,VTypeDRV %>" Value="DRVJ"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Captions,VTypeEXIJ %>" Value="EIJ"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Captions,VTypeJV %>" Value="JV"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Captions,VTypeMIJ %>" Value="MIJ"></asp:ListItem>                                           
                                            <asp:ListItem Text="<%$ Resources:Captions,VTypePCV %>" Value="PCVJ"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Captions,VTypePmV %>" Value="VPJ"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Captions,VTypePV %>" Value="PSIJ"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Captions,VTypeRVMI %>" Value="MSIRJ"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Captions,VTypeRV %>" Value="CRJ"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Captions,VTypeMSIJ %>" Value="MSIJ"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Captions,VTypeSV %>" Value="SIJ"></asp:ListItem>
                                            
                                            
                                                                                     
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="vrfVoucher" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="PRINT" EnableClientScript="true" InitialValue="-1"
                                            runat="server" ControlToValidate="ddlVoucherType" Display="Dynamic" Text="*"
                                            ErrorMessage="<%$ resources:Err_VoucherType %>"></asp:RequiredFieldValidator>
                                           <asp:ImageButton ID="ImbVoucherSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="2" CommandName="SEARCHVOUCHERDETAILS" SkinID="search-ext"
                                               CssClass="margntop2 margnbotm0" />
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S padgtop7">
                                        <asp:Label ID="lblFrom" runat="server" Text='<%$Resources:Controls,From %>'
                                            AssociatedControlID="ddlFrom"></asp:Label>
                                        <asp:DropDownList ID="ddlFrom" runat="server" CssClass="input-small-19-04-21 margnbotm5" TabIndex="19"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="vrfFrom" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="PRINT" EnableClientScript="true" InitialValue="-1"
                                            runat="server" ControlToValidate="ddlFrom" Display="Dynamic" Text="*"
                                            ErrorMessage="<%$ resources:Err_From %>"></asp:RequiredFieldValidator>
                                        <asp:Label ID="lblTo" CssClass="middle-lbl-xsmall-a2" runat="server" Text='<%$Resources:Controls,To %>'
                                            AssociatedControlID="ddlTo"></asp:Label>
                                        <asp:DropDownList ID="ddlTo" runat="server" CssClass="input-small-19-04-21 margnbotm0" TabIndex="19"></asp:DropDownList>
                                          <asp:RequiredFieldValidator ID="vrfTo" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="PRINT" EnableClientScript="true" InitialValue="-1"
                                            runat="server" ControlToValidate="ddlTo" Display="Dynamic" Text="*"
                                            ErrorMessage="<%$ resources:Err_To %>"></asp:RequiredFieldValidator>
                                        <asp:Button runat="server" ID="btnPrint" CommandName="PRINT" TabIndex="11" Text="<%$resources:Controls,Print %>"
                                            OnClick="ActionHandler" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" ValidationGroup="PRINT"/>
                                    </div>
                                </td>
                            </tr>

                        </table>
                    </div>
                </div>
                <div id="diverror" style="display: none">
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="ValSearch" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
