<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchProducts.ascx.cs"
    Inherits="CustomerPortal.OrderToCash.UserControls.SearchProducts" %>
<script type="text/javascript">
    var pageURL = window.document.URL;
    var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
    var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
    function UserControlInitComponents() {
        GrandScriptUtils.MakeAutoCompleteDDL("txtType", url + "&Type=Type", "hdfType", true, true, "PRODUCTNATURE");
        GrandScriptUtils.MakeAutoCompleteDDL("txtThickness", url + "&Type=Thickness", "hdfThickness", true, true, "PRODUCTNATURE");
        GrandScriptUtils.MakeAutoCompleteDDL("txtCategory", url + "&Type=Category", "hdfCategory", true, true, "PRODUCTNATURE");
        GrandScriptUtils.MakeAutoCompleteDDL("txtSurface", url + "&Type=Surface", "hdfSurface", true, true, "PRODUCTNATURE");
        GrandScriptUtils.MakeAutoCompleteDDL("txtShade", url + "&Type=Shade", "hdfShade", true, true, "PRODUCTNATURE");
        GrandScriptUtils.MakeAutoCompleteDDL("txtClassification", url + "&Type=Classification", "hdfClassification", true, true, "PRODUCTNATURE");
        GrandScriptUtils.MakeAutoCompleteDDL("txtSize", url + "&Type=Size", "hdfSize", true, true, "PRODUCTNATURE");
        GrandScriptUtils.MakeAutoCompleteDDL("txtLength", url + "&Type=Length", "hdfLength", true, true, "PRODUCTNATURE");
        GrandScriptUtils.MakeAutoCompleteDDL("txtSPAdvProduct", url, "hdfSPProductPK", true, true, "GETPRODUCTS"); 
    }

    //To excecute after auto complete change
    function AfterInvalidSelect(targetControlID) {
        if (targetControlID == "txtType") {
            
            $("[id$=hdfType]").val("0");
            $("[id$=txtType]").val("<%= Resources.Messages.AutoDefaultValue %>");
        }
        else if (targetControlID == "txtThickness") {
            $("[id$=hdfThickness]").val("0");
            $("[id$=txtThickness]").val("<%= Resources.Messages.AutoDefaultValue %>");
        }
        else if (targetControlID == "txtCategory") {
            $("[id$=hdfCategory]").val("0");
            $("[id$=txtCategory]").val("<%= Resources.Messages.AutoDefaultValue %>");
        }
        else if (targetControlID == "txtSurface") {
            $("[id$=hdfSurface]").val("0");
            $("[id$=txtSurface]").val("<%= Resources.Messages.AutoDefaultValue %>");
        }
        else if (targetControlID == "txtShade") {
            $("[id$=hdfShade]").val("0");
            $("[id$=txtShade]").val("<%= Resources.Messages.AutoDefaultValue %>");
        }
        else if (targetControlID == "txtClassification") {
            $("[id$=hdfClassification]").val("0");
            $("[id$=txtClassification]").val("<%= Resources.Messages.AutoDefaultValue %>");
        }
        else if (targetControlID == "txtSize") {
            $("[id$=hdfSize]").val("0");
            $("[id$=txtSize]").val("<%= Resources.Messages.AutoDefaultValue %>");
        }
        else if (targetControlID == "txtLength") {
            $("[id$=hdfLength]").val("0");
            $("[id$=txtLength]").val("<%= Resources.Messages.AutoDefaultValue %>");
        }

    }

</script>
<div class="content-wrapper">
    <table class="table-4devide" style="display:none">
        <tr style="display:none">
            <td>
                <div class="div4col-S">
                    <asp:Label ID="lblType" Text="Type" runat="server" AssociatedControlID="txtType"></asp:Label>
                    <asp:TextBox ID="txtType" runat="server"></asp:TextBox>
                    <asp:HiddenField ID="hdfType" runat="server"></asp:HiddenField>
                </div>
            </td>
            <td>
                <div class="div4col-S">
                    <asp:Label ID="lblThickness" Text="Thickness" runat="server" AssociatedControlID="txtThickness"></asp:Label>
                    <asp:TextBox ID="txtThickness" runat="server"></asp:TextBox>
                    <asp:HiddenField ID="hdfThickness" runat="server"></asp:HiddenField>
                </div>
            </td>
            <td>
                <div class="div4col-S">
                    <asp:Label ID="Label2" Text="Category" runat="server" AssociatedControlID="txtCategory"></asp:Label>
                    <asp:TextBox ID="txtCategory" runat="server"></asp:TextBox>
                    <asp:HiddenField ID="hdfCategory" runat="server"></asp:HiddenField>
                </div>
            </td>
            <td>
                <div class="div4col-S">
                    <asp:Label ID="Label3" Text="Surface" runat="server" AssociatedControlID="txtSurface"></asp:Label>
                    <asp:TextBox ID="txtSurface" runat="server"></asp:TextBox>
                    <asp:HiddenField ID="hdfSurface" runat="server"></asp:HiddenField>
                </div>
            </td>
        </tr>
        <tr style="display:none">
            <td>
                <div class="div4col-S">
                    <asp:Label ID="Label4" Text="Shade" runat="server" AssociatedControlID="txtShade"></asp:Label>
                    <asp:TextBox ID="txtShade" runat="server"></asp:TextBox>
                    <asp:HiddenField ID="hdfShade" runat="server"></asp:HiddenField>
                </div>
            </td>
            <td>
                <div class="div4col-S">
                    <asp:Label ID="Label5" Text="Classification" runat="server" AssociatedControlID="txtClassification"></asp:Label>
                    <asp:TextBox ID="txtClassification" runat="server"></asp:TextBox>
                    <asp:HiddenField ID="hdfClassification" runat="server"></asp:HiddenField>
                </div>
            </td>
            <td>
                <div class="div4col-S">
                    <asp:Label ID="Label6" Text="Size" runat="server" AssociatedControlID="txtSize"></asp:Label>
                    <asp:TextBox ID="txtSize" runat="server"></asp:TextBox>
                    <asp:HiddenField ID="hdfSize" runat="server"></asp:HiddenField>
                </div>
            </td>
            <td>
                <div class="div4col-S">
                    <asp:Label ID="Label1" Text="Length" runat="server" AssociatedControlID="txtLength"></asp:Label>
                    <asp:TextBox ID="txtLength" runat="server"></asp:TextBox>
                    <asp:HiddenField ID="hdfLength" runat="server"></asp:HiddenField>
                </div>
            </td>
        </tr>         
    </table>
 <%--   Dynamic Product Atttributes --%>
    <asp:HiddenField ID="hdfDdlCountPS" runat="server" />
          <div class="grid-group">                                
                <div class="clear">
                </div>
                <div class="grid-group-table padglft0">
                     <table class="table-devide">
                                        <tr>
                                            <td id="tdCol1" runat="server">
                                            </td>
                                            <td id="tdCol2" runat="server">
                                            </td>
                                        </tr>                                     
                                    </table>

                                     <div class="divcol-S">                  
                    <asp:Label ID="lblSPAdvProduct" runat="server" Text="<%$ resources:Controls,Product %>" AssociatedControlID="txtSPAdvProduct" CssClass="margnrgt-minus1" ></asp:Label>                                           
                     <asp:TextBox runat="server" ID="txtSPAdvProduct" TabIndex="2" placeholder="Select/Type" CssClass="select-full-a" ></asp:TextBox>
                         <asp:HiddenField ID="hdfSPProductPK" Value="0" runat="server" />
                </div>
                </div>                                 
          </div>         
 <%--   End--%>
    <div class="button-wrap-right margntop7">
        <asp:Button ID="btnSelect" runat="server" CommandName="SELECT" Text="Select" ToolTip="Select"
            OnClick="ActionHandler" SkinID="btnInner-ok" />
        <asp:Button ID="btnSearch" runat="server" CommandName="SEARCH" Text="<%$ resources:Controls,Search %>"
            ToolTip="<%$ resources:Controls,Search %>" OnClick="ActionHandler" SkinID="btnInner-search" />
    </div>
    <div class="clear"></div>
    <div class="gridwrap">
        <asp:GridView runat="server" ID="grdSearchProduct" Width="100%" PageSize="25" AutoGenerateColumns="false"
            EmptyDataRowStyle-CssClass="emptytable" TabIndex="11">
            <EmptyDataTemplate>
                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:RadioButton CssClass="rdoSelection" TabIndex="9" runat="server" GroupName="SelectOne"
                            AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                            ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                        <asp:HiddenField runat="server" ID="hdfProductID" Value='<%# Eval("ISD_ITEM") %>' />
                    </ItemTemplate>
                    <ItemStyle Width="3%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ resources:ProductName %>" SortExpression="ITM_CODE"  Visible="<%$ resources:ShowItemCode %>">
                    <ItemTemplate>
                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("INV_ITEM_MST"+"."+"ITM_NAME")%>'
                           ToolTip='<%# String.Format("{0} - {1}", Eval("INV_ITEM_MST"+"."+"ITM_CODE"), Eval("INV_ITEM_MST"+"."+"ITM_NAME")) %> '></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="25%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ resources:Type %>" SortExpression="CON_NAME"  Visible="<%$ resources:ShowType %>">
                    <ItemTemplate>
                        <asp:Label ID="lblType" runat="server" Text='<%# Eval("ADM_CONST_MST"+"."+"CON_NAME")%>'
                            ToolTip='<%# Eval("ADM_CONST_MST"+"."+"CON_NAME")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="8%" />
                </asp:TemplateField>  
                 <asp:TemplateField HeaderText="<%$ resources:Category %>" SortExpression="CON_NAME"  Visible="<%$ resources:ShowCategory %>">
                    <ItemTemplate>
                        <asp:Label ID="lblCategory" runat="server" Text='<%# Eval("ADM_CONST_MST12"+"."+"CON_NAME")%>'
                            ToolTip='<%# Eval("ADM_CONST_MST12"+"."+"CON_NAME")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="10%" />
                </asp:TemplateField>              
                <asp:TemplateField HeaderText="<%$ resources:Surface %>" SortExpression="CON_NAME"  Visible="<%$ resources:ShowSurface %>">
                    <ItemTemplate>
                        <asp:Label ID="lblSurface" runat="server" Text='<%# Eval("ADM_CONST_MST13"+"."+"CON_NAME")%>'
                            ToolTip='<%# Eval("ADM_CONST_MST13"+"."+"CON_NAME")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ resources:Shade %>" SortExpression="CON_NAME"  Visible="<%$ resources:ShowShade %>">
                    <ItemTemplate>
                        <asp:Label ID="lblShade" runat="server" Text='<%# Eval("ADM_CONST_MST14"+"."+"CON_NAME")%>'
                            ToolTip='<%# Eval("ADM_CONST_MST14"+"."+"CON_NAME")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ resources:Classification %>" SortExpression="CON_NAME"  Visible="<%$ resources:ShowClassification %>">
                    <ItemTemplate>
                        <asp:Label ID="lblClassification" runat="server" Text='<%# Eval("ADM_CONST_MST15"+"."+"CON_NAME")%>'
                            ToolTip='<%# Eval("ADM_CONST_MST15"+"."+"CON_NAME")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ resources:Thickness %>" SortExpression="CON_NAME"  Visible="<%$ resources:ShowThickness %>">
                    <ItemTemplate>
                        <asp:Label ID="lblThickness" runat="server" Text='<%# Eval("ADM_CONST_MST1"+"."+"CON_NAME")%>'
                            ToolTip='<%# Eval("ADM_CONST_MST1"+"."+"CON_NAME")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="8%" />
                </asp:TemplateField>               
                <asp:TemplateField HeaderText="<%$ resources:Size %>" SortExpression="CON_NAME"  Visible="<%$ resources:ShowSize %>">
                    <ItemTemplate>
                        <asp:Label ID="lblSize" runat="server" Text='<%# Eval("ADM_CONST_MST16"+"."+"CON_NAME")%>'
                            ToolTip='<%# Eval("ADM_CONST_MST16"+"."+"CON_NAME")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="8%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ resources:Length %>" SortExpression="CON_NAME"  Visible="<%$ resources:ShowLength %>">
                    <ItemTemplate>
                        <asp:Label ID="lblLength" runat="server" Text='<%# Eval("ADM_CONST_MST17"+"."+"CON_NAME")%>'
                            ToolTip='<%# Eval("ADM_CONST_MST17"+"."+"CON_NAME")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="8%" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>        
    </div>
</div>
