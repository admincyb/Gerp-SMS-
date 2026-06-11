<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="CompanyMaster.aspx.cs" Inherits="ERPSMS_v01.Administration.Masters.CompanyMaster"
    Theme="Classic" ValidateRequest="false" EnableEventValidation="false" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
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

        function InitComponents() {
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

        function SetTabs(tab) {
            if (tab == 1) {
                $("[id$='spnPackingListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnPackingListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnPackingSpecs']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnPackingSpecs']").removeClass("tab-active").addClass("tab-inactive");
            }
            else {
                $("[id$='spnPackingListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnPackingListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnPackingSpecs']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnPackingSpecs']").removeClass("tab-inactive").addClass("tab-active");
            }
        }

        function ValidateNow() {
            var isValid = true;
            var msg = "";

//            $("[id$='lblValidPackCode']").hide();
//            $("[id$='lblValidPackSpec']").hide();
//            $("[id$='lblValidPackType']").hide();
//            $("[id$='lblValidPouchpcs']").hide();
//            $("[id$='lblValidInnerBox']").hide();
//            $("[id$='lblValidInnerCarton']").hide();
//            $("[id$='lblValidZipperBag']").hide();
//            $("[id$='lblValidMasterCarton']").hide();
//            $("[id$='lblValidSack']").hide();

//            if ($("[id$='txtPackCode']").val() == '') {
//                $("[id$='lblValidPackCode']").show();
//                isValid = false;
//                msg += '<ul><li><%= GetLocalResourceObject("Err_PackingCode") %></li></ul>';
//            }

//            if ($("[id$='txtPackSpec']").val() == '') {
//                $("[id$='lblValidPackSpec']").show();
//                isValid = false;
//                msg += '<ul><li><%= GetLocalResourceObject("Err_PackingSpecs") %></li></ul>';
//            }

//            if ($("[id$='txtPouchPcs']").val() == '') {
//                $("[id$='lblValidPouchpcs']").show();
//                isValid = false;
//                msg += '<ul><li><%= GetLocalResourceObject("Err_Pouch") %></li></ul>';
//            }
//            if ($("[id$='txtInnerBox']").val() == '') {
//                $("[id$='lblValidInnerBox']").show();
//                isValid = false;
//                msg += '<ul><li><%= GetLocalResourceObject("Err_InnerBox") %></li></ul>';
//            }
//            if ($("[id$='txtInnerCarton']").val() == '') {
//                $("[id$='lblValidInnerCarton']").show();
//                isValid = false;
//                msg += '<ul><li><%= GetLocalResourceObject("Err_InnerCarton") %></li></ul>';
//            }
//            if ($("[id$='txtZipperBag']").val() == '') {
//                $("[id$='lblValidZipperBag']").show();
//                isValid = false;
//                msg += '<ul><li><%= GetLocalResourceObject("Err_ZipperBag") %></li></ul>';
//            }
//            if ($("[id$='txtMasterCarton']").val() == '') {
//                $("[id$='lblValidMasterCarton']").show();
//                isValid = false;
//                msg += '<ul><li><%= GetLocalResourceObject("Err_MasterCarton") %></li></ul>';
//            }
//            if ($("[id$='txtSack']").val() == '') {
//                $("[id$='lblValidSack']").show();
//                isValid = false;
//                msg += '<ul><li><%= GetLocalResourceObject("Err_Sack") %></li></ul>';
//            }


//            if ($("[id$='ddlPouchType']").val() != '-1') {
//                if ($("[id$='txtPouchPcs']").val() == '') {
//                    $("[id$='lblValidPouchpcs']").show();
//                    isValid = false;
//                    msg += '<ul><li><%= GetLocalResourceObject("Err_PcsPerPouch") %></li></ul>';
//                }
//            }

            if (!isValid) {
                $("[id$=litErrorMsg]").show();
                $("[id$=litErrorMsg]").html(msg);
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
            }
            return isValid;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlPacking" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label ID="lblBreadCrum" runat="server" />
                                </ul>
                                <ul id="pnlEntry" runat="server" style="display: none">
                                    <li id="pnlSave" runat="server">
                                        <asp:Button ID="btnSave" runat="server" CommandName="SAVE" Text="<%$ resources:Controls,Save %>"
                                            OnClientClick="return ValidateNow();" ValidationGroup="Packing" SkinID="btnInner-Save"
                                            TabIndex="34" CommandArgument="SEC_ActionPanel" ToolTip="<%$ resources:Controls,Save %>"
                                            OnClick="ActionHandler" />
                                        <%-- <asp:Button ID="btnSave" runat="server" Text="Save" />--%>
                                    </li>
                                    <li id="pnlDelete" runat="server">
                                        <asp:Button ID="btnDelete" runat="server" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" OnClick="ActionHandler"
                                            TabIndex="35" ToolTip="<%$ resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" CommandName="CANCEL" Text="<%$resources:Controls,Cancel %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" OnClick="ActionHandler"
                                            TabIndex="36" ToolTip="<%$ resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                                <ul id="pnlListing" runat="server" style="display: none">
                                    <li>
                                        <asp:Button ID="btnNew" runat="server" CommandName="NEW" Text="<%$ resources:Controls,New %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-New" ToolTip="<%$ resources:Controls,New %>"
                                            OnClick="ActionHandler" TabIndex="5" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnEdit" runat="server" CommandName="EDIT" Text="<%$ resources:Controls,Edit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit" ToolTip="<%$ resources:Controls,Edit %>"
                                            OnClick="ActionHandler" TabIndex="6" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnView" runat="server" CommandName="VIEW" Text="<%$ resources:Controls,View %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-View" ToolTip="<%$ resources:Controls,View %>"
                                            OnClick="ActionHandler" TabIndex="7" />
                                    </li>
                                </ul>
                            </asp:TableCell></asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnPackingListing" runat="server" class="tab-active">
                            <asp:LinkButton ID="lbnPackingListing" runat="server" Text="<%$ resources:Controls,List %>"
                                CommandName="CANCEL" CssClass="tab-active" OnClick="ActionHandler" TabIndex="8"
                                ToolTip="<%$ resources:Controls,List %>" />
                        </span></li>
                        <li><span id="spnPackingSpecs" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnPackingSpecs" runat="server" Text="<%$ resources:Controls,CompanySpecs %>"
                                CommandName="ACTIVATE" CssClass="tab-inactive" OnClick="ActionHandler" TabIndex="9"
                                ToolTip="<%$ resources:Controls,CompanySpecs %>" />
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table ID="tblTemplate" runat="server" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-wrap">
                                <asp:Label ID="lblFilterBy" runat="server" Text="<%$ resources:Controls,FilterBy %>"
                                    AssociatedControlID="ddlFilterBy" />
                                <asp:DropDownList ID="ddlFilterBy" runat="server" TabIndex="1">
                                    <asp:ListItem Text="<%$ resources:Controls,CompanyName %>" Value="<%$ resources:DataFieldRes,CompanySpecs %>" />
                                    <asp:ListItem Text="<%$ resources:Controls,CompanyCode %>" Value="<%$ resources:DataFieldRes,CompanyCode %>" />
                                </asp:DropDownList>
                                <asp:TextBox ID="txtSearchBy" runat="server" onkeydown="return Search(event);" OnClick="ActionHandler"
                                    TabIndex="2" />
                                <asp:Button ID="btnSearch" SkinID="btnInner-Go" runat="server" Text="<%$ resources:Controls,Go %>"
                                    ToolTip="<%$ resources:Controls,Go %>" CommandName="SEARCH" OnClick="ActionHandler"
                                    TabIndex="3" />
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdCompanyMst" Width="100%" AllowPaging="true" OnPageIndexChanging="ActionHandler"
                                    AllowSorting="True" OnSorting="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" GroupName="SelectOne"
                                                    OnCheckedChanged="ActionHandler" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    TabIndex="4" />
                                                <asp:HiddenField ID="hfPackingPK" runat="server" Value='<%#Eval(Resources.DataFieldRes.CompanyMstPK) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,CompanyName%>" SortExpression="<%$ resources:DataFieldRes,CompanySpecs %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompanySpec" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.CompanySpecs))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.CompanySpecs)),40) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,CompanyCode%>" SortExpression="<%$ resources:DataFieldRes,CompanyCode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompanyType" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.CompanyCode) %>'
                                                    Text='<%# Eval(Resources.DataFieldRes.CompanyCode) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,CompanyAddress %>" SortExpression="<%$ resources:DataFieldRes,CompanyAddress %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompanyAddress" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.CompanyAddress) %>'
                                                    Text='<%# Eval(Resources.DataFieldRes.CompanyAddress) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblgap" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <%-- <uc1:PagerControl ID="uclPaging" runat="server" />--%>
                            </div>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <label for="txtCompanyCode">
                                                <%=Resources.Captions.CompanyCode%>
                                                *
                                            </label>
                                            <asp:TextBox ID="txtCompanyCode" runat="server" TabIndex="1" EnableViewState="false">
                                            </asp:TextBox>
                                            <asp:HiddenField runat="server" ID="SBUPk" Value="0"></asp:HiddenField>
                                            <div class="clear">
                                            </div>
                                            <label for="txtCompanyAddress1">
                                                <%=Resources.Captions.CompanyAddress1%>
                                                *
                                            </label>
                                            <asp:TextBox ID="txtCompanyAddress1" TabIndex="3" runat="server" TextMode="MultiLine"
                                                EnableViewState="false">
                                            </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <label for="txtCompanyCity">
                                                <%=Resources.Captions.CompanyCity%>
                                            </label>
                                            <asp:TextBox ID="txtCompanyCity" runat="server" TabIndex="5" EnableViewState="false">
                                            </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <label for="ddlCompanyCountry">
                                                <%=Resources.Captions.CompanyCountry%>
                                            </label>
                                            <asp:DropDownList ID="ddlCompanyCountry" runat="server" TabIndex="7">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                            <label for="txtCompanyFax">
                                                <%=Resources.Captions.CompanyFax%>
                                            </label>
                                            <asp:TextBox ID="txtCompanyFax" runat="server" TabIndex="9" EnableViewState="false">
                                            </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <label for="txtCompanyEmail">
                                                <%=Resources.Captions.CompanyEmail%>
                                            </label>
                                            <asp:TextBox ID="txtCompanyEmail" runat="server" TabIndex="11" EnableViewState="false">
                                            </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <label for="txtCompanyTaxNo">
                                                <%=Resources.Captions.CompanyTaxNo%>
                                            </label>
                                            <asp:TextBox ID="txtCompanyTaxNo" runat="server" TabIndex="13" EnableViewState="false">
                                            </asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <label for="txtCompanyName">
                                                <%=Resources.Captions.CompanyName%>
                                                *
                                            </label>
                                            <asp:TextBox ID="txtCompanyName" runat="server" TabIndex="2" EnableViewState="false">
                                            </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <label for="txtCompanyAddress2">
                                                <%=Resources.Captions.CompanyAddress2%>
                                            </label>
                                            <asp:TextBox ID="txtCompanyAddress2" TabIndex="4" runat="server" TextMode="MultiLine"
                                                EnableViewState="false">
                                            </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <label for="txtCompanyPhone">
                                                <%=Resources.Captions.CompanyPhone%>
                                            </label>
                                            <asp:TextBox ID="txtCompanyPhone" runat="server" TabIndex="8" EnableViewState="false">
                                            </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <label for="txtCompanyMobile">
                                                <%=Resources.Captions.CompanyMobile%>
                                            </label>
                                            <asp:TextBox ID="txtCompanyMobile" runat="server" TabIndex="10" EnableViewState="false">
                                            </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <label for="ddlCurrency">
                                                <%=Resources.Controls.Currency%>
                                                *</label>
                                            <asp:DropDownList runat="server" ID="ddlCurrency" TabIndex="12">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                             <asp:Label ID="lblSignature" runat="server" AssociatedControlID="lblSignatureName" Text="<%$ resources:Controls, Logo %>" />
                                             <asp:Label ID="lblSignatureName" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <label for="fudLogo">
                                                <%=Resources.Captions.CompanyLogo%>
                                                *</label>
                                            <asp:FileUpload ID="fudLogo" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary
                        ID="vsPage" ValidationGroup="Packing" runat="server" />
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
