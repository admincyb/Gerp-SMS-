<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" Theme="ClassicExt"
    AutoEventWireup="true" CodeBehind="NewsManagement.aspx.cs" Inherits="ERPSMS_v01.Administration.Configurations.NewsManagement" %>

<%@ Register Src="~/UserControls/PagerControl.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        //Pename the Page_Entry ID with the corresponding pages Entry section ID        
        function ShowHide(flag) {
            if (flag == "ADD_DETAILS") {
                $("[id$=pnlListing]").show();
                $("[id$=Page_Entry]").hide();
                $("[id$=Page_List]").show();
                $("[id$=pnlEntry]").hide();
                $("[id$=ModifiedDatePnl]").hide();
            }
            else {
                $("[id$=Page_Entry]").show();
                $("[id$=pnlListing]").hide();
                $("[id$=Page_List]").hide();
                $("[id$=pnlEntry]").show();
                $("[id$=ModifiedDatePnl]").show();
            }
            return false;
        }
        function ValidateNow() {
            if (typeof (Page_ClientValidate) == 'function') {
                Page_ClientValidate();
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
        function CheckOtherIsCheckedByGVID(spanChk) {
            var IsChecked = spanChk.checked;
            var CurrentRdbID = spanChk.id;
            $("[id$=grdPage]").find("tr:has(td)").each(function () {
                //type = $(this).find("td:first input").attr("type")
                var id = $(this).find("td:first input").attr("id");
                if (id != CurrentRdbID) {
                    $(this).find("td:first input").attr("checked", false);
                }
            });
        }
        function InitDate() {
            //GrandScriptUtils.AddDateRange("txtPublishedDt", "hdfPublishedDt", false);
            GrandScriptUtils.DatePickerCommon("txtPublishedDt");
        }
    </script>
    <asp:UpdatePanel ID="auplDetailList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table runat="server" ID="tblButton">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$ Resources:Controls, Save %>"
                                            ToolTip="<%$ Resources:Controls, Save %>" ValidationGroup="Save" OnClick="ActionHandler"
                                            OnClientClick="javascript:ValidateNow()" CommandArgument="<%$ resources:Section2 %>"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlEdit">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$ Resources:Controls, Delete %>"
                                            ToolTip="<%$ Resources:Controls, Delete %>" OnClick="ActionHandler" CommandArgument="<%$ resources:Section2 %>"
                                            SkinID="btnInner-Delete" />
                                        <asp:Button runat="server" ID="btnActivate" CommandName="ACTIVATE" Text="<%$ Resources:Controls, Activate %>"
                                            ToolTip="<%$ Resources:Controls, Activate %>" OnClick="ActionHandler" CommandArgument="<%$ resources:Section2 %>"
                                            SkinID="btnInner-activatate" />
                                        <asp:Button runat="server" ID="btnInActivate" CommandName="DEACTIVATE" Text="<%$ Resources:Controls, Deactivate %>"
                                            ToolTip="<%$ Resources:Controls, Deactivate %>" OnClick="ActionHandler" CommandArgument="<%$ resources:Section2 %>"
                                            SkinID="btnInner-deactivate" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCanel" CommandName="CANCEL" Text="<%$ Resources:Controls, Cancel %>"
                                            ToolTip="<%$ Resources:Controls, Cancel %>" OnClick="ActionHandler" CommandArgument="<%$ resources:Section2 %>"
                                            SkinID="btnInner-Cancel" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" ID="btnNew" CommandName="NEW" Text="<%$ Resources:Controls, New %>"
                                            ToolTip="<%$ Resources:Controls, New %>" OnClick="ActionHandler" CommandArgument="<%$ resources:Section2 %>"
                                            SkinID="btnInner-New" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnEdit" CommandName="EDIT" Text="<%$ Resources:Controls, Edit %>"
                                            ToolTip="<%$ Resources:Controls, Edit %>" OnClick="ActionHandler" CommandArgument="<%$ resources:Section2 %>"
                                            SkinID="btnInner-Edit" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" Text="<%$ Resources:Controls, View %>"
                                            ToolTip="<%$ Resources:Controls, View %>" OnClick="ActionHandler" CommandArgument="<%$ resources:Section2 %>"
                                            SkinID="btnInner-View" />
                                    </li>
                                    <li style="display: none">
                                        <asp:Button runat="server" ID="btnPrint" CommandName="PRINT" Text="<%$ Resources:Controls, Print %>"
                                            ToolTip="<%$ Resources:Controls, Print %>" OnClick="ActionHandler" CommandArgument="<%$ resources:Section2 %>"
                                            SkinID="btnInner-Print" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li>
                            <li><span id="spnList" runat="server" class="tab-active">
                                <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                    TabIndex="33" OnClick="ActionHandler" CommandName="LIST" CssClass="tab-active"></asp:LinkButton>
                            </span></li>
                        </li>
                        <li><span id="spnDetail" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                TabIndex="34" OnClick="ActionHandler" CommandName="DETAILS" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <%--use the width property of the below table corresponding to the contents in the page--%>
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <%--Rename this ID Page_Entry with the corresponding section Id in the documet--%>
                    <asp:TableRow ID="Page_List">
                        <asp:TableCell>
                            <%-- use the grid to list the records in the page--%>
                            <asp:GridView ID="grdPage" runat="server" AutoGenerateColumns="False" Width="100%"
                                PageSize="<%$ resources:PageSize %>" AllowPaging="True" OnPageIndexChanging="ActionHandler"
                                AllowSorting="True" OnSorting="ActionHandler">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:RadioButton runat="server" GroupName="SelectOne" ID="rbtSelect" onclick="javascript:CheckOtherIsCheckedByGVID(this);" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$resources:grd_No %>">
                                        <ItemTemplate>
                                            <%#(Container.DataItemIndex + 1)%>
                                        </ItemTemplate>
                                        <HeaderStyle Width="2%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$resources:NewsTitle %>" SortExpression="F_TITLE">
                                        <ItemTemplate>
                                            <%#Eval(ERP.Utilities.Constants.DA.Administration.NewsManagements.F_TITLE)%>
                                        </ItemTemplate>
                                        <HeaderStyle Width="44%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$resources:PublishDate %>" SortExpression="F_PUBLISHEDDT">
                                        <ItemTemplate>
                                            <%#Eval(ERP.Utilities.Constants.DA.Administration.NewsManagements.F_PUBLISHEDDT,"{0:d}")%>
                                        </ItemTemplate>
                                        <HeaderStyle Width="8%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$resources:CreatedBy %>" SortExpression="F_CREATEDBYNAME">
                                        <ItemTemplate>
                                            <%#Eval(ERP.Utilities.Constants.DA.Administration.NewsManagements.F_CREATEDBYNAME)%>
                                        </ItemTemplate>
                                        <HeaderStyle Width="30%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:CreatedDate %>" SortExpression="F_LASTMODON">
                                        <ItemTemplate>
                                            <%#Eval(ERP.Utilities.Constants.DA.Administration.NewsManagements.F_LASTMODON)%>
                                        </ItemTemplate>
                                        <HeaderStyle Width="15%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="Page_Entry" runat="server">
                        <%--Align table cell according to design--%>
                        <asp:TableCell>
                            <div id="grdTable-wrap">
                                <%--  <div id="divData">--%>
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S padgtop7">
                                                <asp:Label runat="server" AssociatedControlID="txtTitle" ID="lblTitle" Text="<%$ resources:NewsTitle %>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtTitle" TabIndex="1" CssClass="select-half" MaxLength="50"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfTitle" CssClass="star" SetFocusOnError="true"
                                                    EnableClientScript="true" ValidationGroup="News" runat="server" ControlToValidate="txtTitle"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Title %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S padgtop7">
                                                <asp:Label runat="server" ID="lblPublishedDte" Text="<%$ resources:PublishDate %>"
                                                    AssociatedControlID="txtPublishedDt"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtPublishedDt" TabIndex="2" CssClass="input-small" onkeydown="return false;"
                                                    onpaste="return false;"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfPublishedDte" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="TRates" runat="server" ControlToValidate="txtPublishedDt" Display="Dynamic"
                                                    Text="*" ErrorMessage="<%$ resources:Err_PublishDate %>" EnableClientScript="true">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label runat="server" ID="lblShortDesc" Text="<%$ resources:ShortDesc %>" AssociatedControlID="txtShortDesc"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtShortDesc" TabIndex="3"  MaxLength="125"
                                                    onkeydown="limitText(this,125);" onkeyup="limitText(this,125);"></asp:TextBox>
                                                <asp:RegularExpressionValidator runat="server" ID="revShortDesc" EnableClientScript="true"
                                                    ValidationGroup="News" CssClass="star" SetFocusOnError="true" ControlToValidate="txtShortDesc"
                                                    ValidationExpression="^(.|\n){1,125}$" Text="*" ErrorMessage="<%$ resources:ErpRes, Msg_Exceed_MaxLen %>" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label runat="server" ID="lblDetails" Text="<%$ resources:Details %>" AssociatedControlID="txtDetails"></asp:Label>
                                                <textarea id="txtDetails" runat="server" cols="40" rows="4" class="multilarge" onkeydown="limitText(this,500);"
                                                    onkeyup="limitText(this,500);" tabindex="4"></textarea>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server">
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblLastModBy" ClientIDMode="Static" CssClass="lastmodi"
                                AssociatedControlID="hdfMofidiedOn"></asp:Label>
                            <asp:HiddenField ID="hdfMofidiedOn" runat="server" Value="0" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <%--Use this label to bind the server errors--%>
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary
                    ID="vvsPage" ValidationGroup="Save" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
