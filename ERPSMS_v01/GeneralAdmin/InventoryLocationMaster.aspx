<%@Page Title="<%$ Resources:Captions,Title_InventoryLocationMaster %>" Language="C#" AutoEventWireup="true" CodeBehind="InventoryLocationMaster.aspx.cs"   
 MasterPageFile="~/ERPSMS_2.Master" EnableEventValidation="false" Theme="ClassicExt" Inherits="ERPSMS_v01.GeneralAdmin.InventoryLocationMaster" %>

 <%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

   <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">
         function InitComponents() {
             ShowHideAdvancedSearch(1);
             GrandScriptUtils.DatePickerCommon("txtDate");
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

         function ShowHideAdvancedSearch(flag) {
             if (flag == 1) {
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
    <div class="fixed-buttons-normal">
        <div class="Button-container">
            <asp:Table runat="server" ID="tblButton">
                <asp:TableRow>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul class="bredcrum">
                            <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                        </ul>
                        <ul runat="server" id="pnlEntry" style="display: none">
                                    <li>
                                        <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                            ToolTip="<%$Resources:Controls,Save%>" EnableViewState="False"  ValidationGroup="vgCompany"  
                                            CommandName="SAVE" OnClick="ActionHandler"  OnClientClick="javascript:ValidatePageNow('vgCompany')"                                           
                                            TabIndex="35" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" Text="<%$resources:Controls,Delete %>"
                                            CommandName="DELETE" OnClick="ActionHandler" SkinID="btnInner-Delete" ToolTip="<%$resources:Controls,Delete %>"
                                            TabIndex="35" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                          OnClick="ActionHandler"   CommandName="CANCEL" SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>"
                                            TabIndex="35" />
                                    </li>
                                </ul>
                        <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="152" ID="btnNew" CommandName="NEW" 
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li runat="server" id="pnlEdit">
                                        <asp:Button runat="server" ID="btnEdit" CommandName="EDIT" Text="<%$resources:Controls,Edit %>"
                                           TabIndex="152" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit" OnClick="ActionHandler"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
     <div class="content-wrapper">
                 <div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="155"  CommandName="LIST"  OnClick="ActionHandler" 
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="156"  CommandName="EDIT" OnClick="ActionHandler" 
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                 <asp:Table runat="server" ID="tblGstClassification" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server" Style="display: none;">
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
                                                TabIndex="2" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>"
                                                TabIndex="2" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7 ">
                                            <asp:Label ID="lblCodeFilter" runat="server" Text="<%$ resources:Code%>" AssociatedControlID="txtCodeFilterList"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtCodeFilterList" TabIndex="2" CssClass="input-w34per margnbotm0"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7 ">
                                            <asp:Label ID="lblNameFilterList" runat="server" Text="<%$ resources:StoreName%>" AssociatedControlID="txtNameFilterList"
                                                CssClass="middle-lbl-xsmall-c2"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtNameFilterList" TabIndex="2" CssClass="input-w34per margnbotm0"></asp:TextBox>
                                            <asp:ImageButton ID="btnSearchList" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" TabIndex="3"
                                                CommandName="SEARCH" OnClick="ActionHandler" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClearList" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="3" OnClick="ActionHandler"
                                                CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnlft-minus2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdInvType" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" OnPageIndexChanging="ActionHandler" CssClass="grdTable"
                                    OnSorting="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    OnCheckedChanged="ActionHandler" TabIndex="4" />
                                               <asp:HiddenField runat="server" ID="hdfGstCfnPk" Value='<%# Eval("DPT_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="0.5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvStore %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvStore" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("DPT_PARENT_TEXT")),50) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("DPT_PARENT_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Code %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("DPT_CODE")),20) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("DPT_CODE")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:StoreName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("DPT_NAME")),20) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("DPT_NAME")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                        </asp:TemplateField>
                                   
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" Visible="false"/>
                            </div>


                            </asp:TableCell>
                    </asp:TableRow>
              <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                <table class="table-devide">
                    <tr>
                        <td>
                        <div class="div2col-S">
                              
                             <asp:Label ID="lblstore" runat="server" AssociatedControlID="ddlstore"   Text="<%$ resources:InvStore %>" />
                            <asp:DropDownList runat="server" TabIndex="1"  CssClass="select-half-a" AutoPostBack="true" ID="ddlstore">
                            </asp:DropDownList>
                            <div class="starwrap">
                                <asp:RequiredFieldValidator ID="vrfPort" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="vgCompany" EnableClientScript="true" runat="server" ControlToValidate="ddlstore"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Type %>" InitialValue="-1" >
                                </asp:RequiredFieldValidator>                                                     
                            </div>
                             <asp:Label ID="lblCode" runat="server" AssociatedControlID="txtCode"  Text="<%$ resources:Code %>" />
                             <asp:TextBox runat="server" ID="txtCode" TabIndex="3"  CssClass="input-half" MaxLength="200" onkeydown="limitText(this,200);"
                                onkeyup="limitText(this,200);"  />
                            <div class="starwrap">
                                <asp:RequiredFieldValidator ID="rqdfdCode" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="vgCompany" EnableClientScript="true" runat="server" ControlToValidate="txtCode"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_Code %>" />
                              </div>
                                    </div>
                        </td>
                        <td> 
                        <div class="div2col-S">        
                              <asp:Label ID="lblstoreName" runat="server" AssociatedControlID="txtstoreName"     Text="<%$ resources:StoreName %>" />
                            <asp:TextBox runat="server" ID="txtstoreName" TabIndex="2" MaxLength="200"  CssClass="input-half" onkeydown="limitText(this,200);"
                                onkeyup="limitText(this,200);" />
                            <div class="starwrap">
                                <asp:RequiredFieldValidator ID="vrfStoreName" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="vgCompany" EnableClientScript="true" runat="server" ControlToValidate="txtstoreName"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_StoreName %>" />
                              </div>
                        
                        
                        </div>
                         </td>
                   </tr>                                                  
                </table>
                        </asp:TableCell>
                        </asp:TableRow>
                        </asp:Table>
    </div>
   <div id="diverror" style="display: none">
        <%--Use this label to bind the server errors--%>
        <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star" />
        <asp:ValidationSummary ID="vvsUser" ValidationGroup="vgCompany" runat="server" />
    </div>
    <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" />
</asp:Content>