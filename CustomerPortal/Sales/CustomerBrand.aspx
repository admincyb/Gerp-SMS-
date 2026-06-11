<%@ Page Title="<%$ Resources:Captions,Title_CustomerBrandListing %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="CustomerBrand.aspx.cs" Inherits="CustomerPortal.Sales.CustomerBrand"  Theme="ClassicExt" %>

<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"  TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript" language="javascript">
         var pageURL = window.document.URL;
         var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
         var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");
         function InitComponents() {
             $('[id$=ChkSelectAll]').click(function () {
                 $("[id$='ChkSelect']").attr('checked', this.checked);
             });
             $('[id$=ChkSelect]').change(function () { //".checkbox" change 
                 //uncheck "select all", if one of the listed checkbox item is unchecked
                 if (this.checked == false) { //if this item is unchecked
                     $('[id$=ChkSelectAll]')[0].checked = false; //change "select all" checked status to false
                 }
                 //check "select all" if all checkbox items are checked
                 if ($('[id$=ChkSelect]:checked').length == $('[id$=ChkSelect]').length) {
                     $('[id$=ChkSelectAll]')[0].checked = true; //change "select all" checked status to true
                 }
             });
             ShowHideAdvancedSearch(1);  
             GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url, "hdfCustomerPk", true, true, "CUSTOMER");
             if ($("[id$=txtCustomer]").attr("disabled") == true) {
                 DisableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomerPk]"));
             }           
         }
         function AfterAutoCompleteSelect(targetControlID) {
             if (targetControlID == "txtCustomer") {  
             }  
         }
         //To excecute after auto complete change
         function AfterInvalidSelect(targetControlID) {
             if (targetControlID == "txtCustomer") {
                
             }            
         }
         function DisableAuto(extender, hfield) {
             ///<summary>
             /// Used to disable Autocomplete
             ///</summary>
             $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
             $(extender).autocomplete("option", "disabled", true);
             $(extender).attr("disabled", true);
         }
         function EnableAuto(extender) {
             ///<summary>
             /// Used to enable Autocomplete
             ///</summary>
             $(extender).removeAttr("disabled");
             $(extender).next($(".ddlSelect")).removeClass("ddlSelect-disable").addClass("ddlSelect");
             $(extender).autocomplete("option", "disabled", false);
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
         function ShowHideAdvancedSearch(flag) {
             //If flag then Show AdvancedSearch
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlPacking" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label ID="lblBreadCrum" runat="server" />
                                </ul>
                                <ul id="pnlListing" runat="server">
                                     <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="33" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>                                
                                </ul>
                            </asp:TableCell></asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table ID="tblTemplate" runat="server" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <%--New Search Starts--%>
                              <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Captions", "AdvanceSearch").ToString() %></h1>
                                        </td>
                                        <td>
                                            <%--<asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="65" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="66" />--%>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn----------%>
                            <table class="table-devide" id="tbladvancedSearch" style="margin-top: 8px;">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCustomer" runat="server" Text="<%$resources:Customer_Mand %>" AssociatedControlID="txtCustomer"></asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="select-half margnbotm0" MaxLength="100"
                                                TabIndex="5"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerPk" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfCustomer" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Brand" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                runat="server" ControlToValidate="txtCustomer" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Customer %>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblBrandName" runat="server" Text="<%$resources:BrandName %>" AssociatedControlID="txtBrand"></asp:Label>
                                            <asp:TextBox ID="txtBrand" runat="server" CssClass="select-half margnbotm0" MaxLength="100"  TabIndex="5"> </asp:TextBox>
                                            <asp:Label ID="lblSearch" runat="server" AssociatedControlID="btnSearch" CssClass="middle-lbl-xsmall-d style-none"></asp:Label>
                                            <asp:ImageButton ID="btnSearch" SkinID="search-ext" runat="server" Text="<%$ resources:Controls,Go %>"  ValidationGroup="Brand"
                                                ToolTip="<%$ resources:Controls,Go %>" CommandName="SEARCH" OnClick="ActionHandler" CssClass="margntop2 margnbotm0"
                                                TabIndex="1" />
                                             <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="9" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                           
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <%-- New Search Ends--%>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdCustomerBrands" Width="100%" AllowSorting="True" OnSorting="ActionHandler"
                                    OnRowDataBound="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ChkSelectAll" runat="server" TabIndex="4" Checked="false" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkSelect" runat="server" Checked="false" TabIndex="5" />
                                                <asp:HiddenField ID="hfCimPk" runat="server" Value='<%#Eval("CIM_PK") %>' />      
                                                <asp:HiddenField ID="hdfCimUsedFlag" runat="server" Value='<%#Eval("CIM_USED_FLAG") %>' />                                                                                             
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BrandName%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBrandName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetEncodedString(Eval("CIM_BRAND_NAME"))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("CIM_BRAND_NAME")),40) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BrandCode %>" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblBrandCode" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetEncodedString(Eval("CIM_BRAND_CODE"))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("CIM_BRAND_CODE")),30) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Product %>" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblProduct" runat="server" ToolTip='<%# Eval("CIM_PRD_CODE") %>'
                                                    Text='<%#  ERP.Utilities.CommonFunctions.GetShortString(Eval("CIM_PRD_CODE"),20) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AQL %>" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblAqlName" runat="server" ToolTip='<%# Eval("CIM_AQL_NAME")  %>' Text='<%#Eval("CIM_AQL_NAME") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Size %>" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblSize" runat="server" ToolTip='<%# Eval("CIM_SIZE_TEXT")  %>' Text='<%#Eval("CIM_SIZE_TEXT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Packing %>" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblPacking" runat="server" ToolTip='<%# Eval("APS_PACK_TEXT")  %>' Text='<%#Eval("APS_PACK_TEXT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Active %>" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblActive" runat="server" ToolTip='<%# Eval("CIM_ACTIVE").ToString()=="1"?"Active":"InActive"%>' Text='<%#Eval("CIM_ACTIVE").ToString()=="1"?"Active":"InActive"%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                        </asp:TemplateField>                                      
                                       
                                    </Columns>
                                </asp:GridView>
                               <%--<uc1:PagerControl ID="uclPaging" runat="server" />--%>
                            </div>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="Brand" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
