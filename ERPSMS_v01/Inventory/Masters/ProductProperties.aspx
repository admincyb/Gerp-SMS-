<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductProperties.aspx.cs" 
         Inherits="ERPSMS_v01.Inventory.Masters.ProductProperties" MasterPageFile="~/ERPSMS_2.Master"
         EnableEventValidation="false"  ValidateRequest="false" Title="<%$ Resources:Captions,Title_GeneralProperties %>" Theme="Classic" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="head">
<script type="text/javascript">
    function InitComponents() {
        $("[id$='lblValidProductionCode']").hide();
        $("[id$='lblValidProductionName']").hide();
    }
    function ValidateNow() {
        var isValid = true;
        var msg = "";

        $("[id$='lblValidProductionCode']").hide();
        $("[id$='lblValidProductionName']").hide();

        if ($("[id$='ddlGroup']").val() == '0') {
            isValid = false;
            msg += '<%= GetLocalResourceObject("Err_ProductGroup") %>' + '<br/>';
        }

        if ($("[id$='txtProductCode']").val() == '') {
            $("[id$='lblValidProductionCode']").show();
            isValid = false;
            msg += '<%= GetLocalResourceObject("Err_ProductCode") %>' + '<br/>';
        }

        if ($("[id$='txtProductName']").val() == '') {
            $("[id$='lblValidProductionName']").show();
            isValid = false;
            msg += '<%= GetLocalResourceObject("Err_ProductName") %>' + '<br/>';
        }

        if (!isValid) {
            $("[id$=litErrorMsg]").show();
            $("[id$=litErrorMsg]").html(msg);
            ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
        }
        return isValid;
    }
</script>
</asp:Content>

<asp:Content ID="cntMain" runat="server" ContentPlaceHolderID="MainContent">
<asp:UpdatePanel ID="aupdpnlProductionProperties" runat="server">
        <ContentTemplate>
           <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum" Text="<%$ resources:Breadcrumb %>"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlListing" >
                                    <li>
                                         <asp:Button ID="btnSave" runat="server" CommandName="SAVE" Text="<%$ resources:Controls,Save %>"
                                            OnClientClick="return ValidateNow();" ValidationGroup="Product" SkinID="btnInner-Save" TabIndex="1"
                                            CommandArgument="SEC_ActionPanel" ToolTip="<%$ resources:Controls,Save %>" OnClick="ActionHandler" />

                                    </li>
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" CommandName="CANCEL" Text="<%$resources:Controls,Cancel %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" OnClick="ActionHandler" TabIndex="19"
                                            ToolTip="<%$ resources:Controls,Cancel %>" />
                                    </li>
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
                                          <label for="ddlGroup">
                                          <%=Resources.Controls.Group%>
                                         *</label>
                                        <asp:DropDownList ID="ddlGroup" runat="server" TabIndex="1" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true"  >
                                        </asp:DropDownList>
                                      </div>
                                    </td>
                               </tr>
                         </table>
                         <div class="clear">
                         </div>
                         <div class="search-wrap">
                                <asp:TextBox ID="txtSearchBy" runat="server"  TabIndex="2" />
                                <asp:Button ID="btnSearch" SkinID="btnInner-Go" runat="server" Text="<%$ resources:Controls,Go %>"
                                    ToolTip="<%$ resources:Controls,Go %>" CommandName="SEARCH" OnClick="ActionHandler" TabIndex="2" />
                            </div>
                         <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdProductionMst" Width="100%" PageSize="<%$ resources:PageSize %>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                    OnSorting="ActionHandler" onrowcommand="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,GenralPropertyCode%>" SortExpression="<%$ resources:DataFieldRes,ConstCode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPackSpecCode" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.ConstCode)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.ConstCode)),50) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,GeneralPropertyName%>" SortExpression="<%$ resources:DataFieldRes,ConstName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPackSpec" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.ConstName)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.ConstName)),100) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Description%>" SortExpression="<%$ resources:DataFieldRes, ConstDescription %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.ConstDescription)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.ConstDescription)),100) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="50%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" OnClick="ActionHandler" CommandName="GRIDEDIT" ToolTip="<%$ resources:Controls,Edit  %>" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                <asp:ImageButton runat="server" ID="imbRemove" SkinID="imbdeletegrid" OnClick="ActionHandler" CommandName="GRIDDELETE" ToolTip="<%$ resources:Captions, Remove %>" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"  />
                                                
                                             </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>


                        </asp:TableCell>
                    </asp:TableRow>
                      <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblProductionCode" runat="server" Text="<%$ resources:Controls,GenralPropertyCode%>"
                                                AssociatedControlID="txtProductCode" />
                                            <asp:TextBox ID="txtProductCode" runat="server" CssClass="medium" MaxLength="50"
                                                 TabIndex="4" />
                                            <asp:Label ID="lblValidProductionCode" runat="server" Text="*" CssClass="star" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblProductionName" runat="server" Text="<%$ resources:Controls,GeneralPropertyName%>"
                                                AssociatedControlID="txtProductName" />
                                            <asp:TextBox ID="txtProductName" runat="server" MaxLength="100" TabIndex="5" />
                                            <asp:Label ID="lblValidProductionName" runat="server" Text="*" CssClass="star" />
                                             <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td >
                                        <div class="divcol-S">
                                            <asp:Label ID="lblProductionDesc" runat="server" Text="<%$ resources:Controls,Description%>"
                                                AssociatedControlID="txtProductDesc" />
                                            <asp:TextBox ID="txtProductDesc" runat="server" TextMode="MultiLine" MaxLength="200" TabIndex="6"
                                                CssClass="multiline-2col" onkeydown="limitText(this,200);" onkeyup="limitText(this,200);" />
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
               </asp:Table> 

              <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="Product" runat="server" />
                </div>
             </div>
             <asp:HiddenField ID="hdnGrptype" runat="server" />
        </ContentTemplate>
        </asp:UpdatePanel> 
</asp:Content>
