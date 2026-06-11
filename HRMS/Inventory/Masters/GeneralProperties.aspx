<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GeneralProperties.aspx.cs"
    Inherits="ERPSMS_v01.Inventory.Masters.GeneralProperties" MasterPageFile="~/ERPSMS_2.Master"
    EnableEventValidation="false" ValidateRequest="false" Theme="ClassicExt" Title="<%$ Resources:Captions,Title_GeneralProperties %>" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        function InitComponents() {
            $("[id$='lblValidProductionCode']").hide();
            $("[id$='lblValidProductionName']").hide();
            $("[id$='lblValidGroupSequence']").hide();
            $("[id$='lblValidGroupCode']").hide();
            $("[id$='lblValidGroupName']").hide();
            $("[id$='lblValidSequence']").hide();
            ShowHideGroupDetails(1);
        }

        function ShowHideGroupDetails(flag) {
            if (flag == 1) {
                $("[id$=divGroupDetails").show();
                $("[id$=imbShowGroupDetails").hide();
                $("[id$=imbHideGroupDetails").show();
            }
            else {
                $("[id$=divGroupDetails").hide();
                $("[id$=imbShowGroupDetails").show();
                $("[id$=imbHideGroupDetails").hide();
            }
            return false;
        }
        function ValidateNow() {
            var isValid = true;
            var msg = "";

            $("[id$='lblValidProductionCode']").hide();
            $("[id$='lblValidProductionName']").hide();
            $("[id$='lblValidSequence']").hide();

            if ($("[id$='ddlGroup']").val() == '-1') {
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Group") %></li></ul>';
            }

            if ($("[id$='txtProductCode']").val() == '') {
                $("[id$='lblValidProductionCode']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Code") %></li></ul>';
            }
            //            if ($("[id$='ddlSequence1']").val() == '-1') {
            //                isValid = false;
            //                msg += '<ul><li><%= GetLocalResourceObject("Err_PackingType") %></li></ul>';
            //            }

            if ($("[id$='txtProductName']").val() == '') {
                $("[id$='lblValidProductionName']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Name") %></li></ul>';
            }

            if ($("[id$='txtSequence']").val() == '') {
                $("[id$='lblValidSequence']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Sequence") %></li></ul>';
            }

            if (!isValid) {
                $("[id$=litErrorMsg]").show();
                $("[id$=litErrorMsg]").html(msg);
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
            }
            return isValid;
        }

        function ValidateGroup() {
            var isValid = true;
            var msg = "";

            $("[id$='lblValidGroupCode']").hide();
            $("[id$='lblValidGroupName']").hide();
            $("[id$='lblValidGroupSequence']").hide();

            if ($("[id$='txtGroupCode']").val() == '') {
                $("[id$='lblValidGroupCode']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Code") %></li></ul>';
            }

            if ($("[id$='txtGroupName']").val() == '') {
                $("[id$='lblValidGroupName']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Name") %></li></ul>';
            }

            if ($("[id$='txtGroupSequence']").val() == '') {
                $("[id$='lblValidGroupSequence']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Sequence") %></li></ul>';
            }

            if (!isValid) {
                $("[id$=litGroupErrorMsg]").show();
                $("[id$=litGroupErrorMsg]").html(msg);
                ShowErrorMessage($("#diverrorGroup").html(), '<%= Resources.Messages.Information %>');
            }
            return isValid;
        }

        function NewGroup() {
            if ($("[id$='ddlGroup']").val() == '-1') {
                $("[id$='btnSave']").hide();
                $("[id$='addItem']").hide();
                $("[id$='itemlist']").hide();
                $("[id$='ItemTitle']").hide();
            }
            else {
                $("[id$='btnSave']").show();
                $("[id$='addItem']").show();
                $("[id$='itemlist']").show();
                $("[id$='ItemTitle']").show();
            }
        }

        function InvalidType() {
            $("[id$=litErrorMsg]").show();
            $("[id$=litErrorMsg]").html('<ul><li><%= GetLocalResourceObject("Err_Type") %></li></ul>');
            ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        function GroupIsOpen() {
            if ($(".ui-dialog").last().is(':visible') == true) {
                return false;
            }
        }

        function ShowGroup() {
            $("#dialog-Template").dialog({ autoOpen: false });
            $("#dialog:ui-dialog").dialog("destroy");
            $("#dialog-Template").dialog({ width: 920, height: 500, buttons: {} });
            $("#dialog-Template").dialog("open").parents("div:eq(0)").appendTo($(document.forms[0]));
        }


        function ShowAddGroup() {
            $("[id$='txtGroupCode']").val("");
            $("[id$='txtGroupName']").val("");
            $("[id$='txtGroupSequence']").val("");

            $("#dialog-Template").dialog({ autoOpen: false });
            $("#dialog:ui-dialog").dialog("destroy");
            $("#dialog-Template").dialog({ width: 920, height: 500, buttons: {} });
            $("#dialog-Template").dialog("open").parents("div:eq(0)").appendTo($(document.forms[0]));
        }





        function CloseGroup() {
            if ($("#aspnetForm").children(".ui-dialog").last().is(':visible') == true) {
                $("#dialog-Template").dialog("close");
            }
        }

        function ShowListing(flag) {
            if (flag) {
                $("[id$='itemlist']").show();
                $("[id$='addItem']").hide();
                $("[id$='pnlListing']").show();
                $("[id$='pnlEntry']").hide();
            }
            else {
                $("[id$='itemlist']").hide();
                $("[id$='addItem']").show();
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

        function SetTabs(tab) {
            if (tab == 1) {
                $("[id$='spnListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnDetails']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnDetails']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='Group']").show();
            }
            else {
                $("[id$='spnListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnDetails']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnDetails']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='Group']").hide();
            }
            $('#diverrorGroup').hide();
        }
    </script>
</asp:Content>
<asp:Content ID="cntMain" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="aupdpnlProductionProperties" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
                function EndRequestHandler(sender, args) {
                    if (args.get_error() != undefined) {
                        args.set_errorHandled(true);
                    }
                }
            </script>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul id="pnlEntry" runat="server" style="display: none">
                                    <li id="pnlSave" runat="server">
                                        <asp:Button ID="btnSave" runat="server" CommandName="SAVE" Text="<%$ resources:Controls,Save %>"
                                            OnClientClick="return ValidateNow();" ValidationGroup="Product" SkinID="btnInner-Save"
                                            TabIndex="11" CommandArgument="SEC_ActionPanel" ToolTip="<%$ resources:Controls,Save %>"
                                            OnClick="ActionHandler" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                            TabIndex="12" CommandName="CANCEL" OnClick="ActionHandler" EnableViewState="False"
                                            ToolTip="<%$Resources:Controls,Cancel%>" />
                                    </li>
                                </ul>
                                <ul id="pnlListing" runat="server" style="display: none">
                                    <li>
                                        <asp:Button ID="btnNew" runat="server" CommandName="NEW" Text="<%$ resources:Controls,New %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-New" ToolTip="<%$ resources:Controls,New %>"
                                            OnClick="ActionHandler" TabIndex="13" />
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
                            <table class="table-devide" id="Group" runat="server">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblGroup" runat="server" Text="<%$ resources:Controls,Group %>" AssociatedControlID="ddlGroup" />
                                            <asp:DropDownList ID="ddlGroup" runat="server" TabIndex="1" OnSelectedIndexChanged="ActionHandler"
                                                AutoPostBack="true" CssClass="select-half">
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="btnAddGroup" runat="server" SkinID="imbaddnew" ToolTip='<%=Resources.Messages.AddTemplateGroup%>'
                                                CommandName="ADDGROUP" OnClick="ActionHandler" OnClientClick="return GroupIsOpen();"
                                                TabIndex="2" />
                                            <asp:HiddenField ID="hdfAddGroup" runat="server" Value="0" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblGroupParent" runat="server" Text="Test" AssociatedControlID="ddlGroupParent"
                                                Visible="false" />
                                            <asp:DropDownList ID="ddlGroupParent" runat="server" TabIndex="1" Visible="false"
                                                OnSelectedIndexChanged="ActionHandler" AutoPostBack="true" CssClass="select-half">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div id="ItemTitle" runat="server">
                                <%--  <ul id="tab-menu">
                        <li><span id="spnListing" runat="server" class="tab-active">
                            <asp:LinkButton ID="lbnListing" runat="server" Text="<%$ resources:Controls,List %>"
                                CommandName="CANCEL" CssClass="tab-active" OnClick="ActionHandler" ToolTip="<%$ resources:Controls,List %>" />
                        </span></li>
                        <li><span id="spnDetails" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnDetails" runat="server" Text="<%$ resources:Controls,Details %>"
                                CommandName="NEW" CssClass="tab-inactive" OnClick="ActionHandler" ToolTip="<%$ resources:Controls,Details %>" />
                        </span></li>
                    </ul>--%>
                            </div>
                            <div class="clear">
                            </div>
                            <div id="itemlist" runat="server">
                                <div class="search-wrap-custom">
                                    <asp:Label ID="lblFilterBy" runat="server" Text="<%$ resources:Controls,FilterBy %>"
                                        AssociatedControlID="ddlFilterBy" />
                                    <asp:DropDownList ID="ddlFilterBy" runat="server" TabIndex="2">
                                        <asp:ListItem Text="<%$ resources:Controls,GenralPropertyCode %>" Value="<%$ resources:DataFieldRes,ConstCode %>" />
                                        <asp:ListItem Text="<%$ resources:Controls,GeneralPropertyName %>" Value="<%$ resources:DataFieldRes,ConstName %>" />
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtSearchBy" runat="server" TabIndex="3" onkeydown="return Search(event);" />
                                    <asp:Button ID="btnSearch" SkinID="btnInner-Go" runat="server" Text="<%$ resources:Controls,Go %>"
                                        ToolTip="<%$ resources:Controls,Go %>" CommandName="SEARCH" OnClick="ActionHandler"
                                        TabIndex="4" />
                                </div>
                                <div class="gridwrap">
                                    <asp:GridView runat="server" ID="grdProductionMst" Width="100%" PageSize="<%$ resources:PageSize %>"
                                        AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                        OnSorting="ActionHandler" OnRowDataBound="ActionHandler">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:Controls,GenralPropertyCode%>" SortExpression="<%$ resources:DataFieldRes,ConstCode %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCode" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.ConstCode)) %>'
                                                        Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetDecodedString(ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.ConstCode))),30) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Controls,GeneralPropertyName%>" SortExpression="<%$ resources:DataFieldRes,ConstName %>">
                                                <ItemTemplate>
                                                    <%--<asp:Label ID="lblName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.ConstName)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.ConstName)),30) %>' />--%>
                                                    <asp:Label ID="lblName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.ConstName)) %>'
                                                        Text='<%#HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.ConstName).ToString()) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="17%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Controls,CBM%>" SortExpression="<%$ resources:DataFieldRes,CON_DATA %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCBM" runat="server" ToolTip='<%#Eval(Resources.DataFieldRes.CON_DATA) %>'
                                                        Text='<%# Eval(Resources.DataFieldRes.CON_DATA) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Controls,Description%>" SortExpression="<%$ resources:DataFieldRes, ConstDescription %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDescription" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.ConstDescription)) %>'
                                                        Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.ConstDescription)),100) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="38%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Controls,Sequence%>" SortExpression="<%$ resources:DataFieldRes,CON_SEQUENCE %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSequence" runat="server" ToolTip='<%#Eval(Resources.DataFieldRes.CON_SEQUENCE) %>'
                                                        Text='<%# Eval(Resources.DataFieldRes.CON_SEQUENCE) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="8%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:GrHrStatus%>" SortExpression="<%$ resources:DataFieldRes,ConActive %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGPActive" runat="server" Text='<%# Eval(Resources.DataFieldRes.ConActive).ToString()=="1"? Resources.Controls.Active: Resources.Controls.InActive %> '
                                                        ToolTip='<%# Eval(Resources.DataFieldRes.ConActive).ToString()=="1"? Resources.Controls.Active: Resources.Controls.InActive %> ' />
                                                </ItemTemplate>
                                                <ItemStyle Width="8%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" OnClick="ActionHandler"
                                                        CommandName="GRIDEDIT" ToolTip="<%$ resources:Controls,Edit  %>" CommandArgument="<%# Eval(Resources.DataFieldRes.ConstPK) %>"
                                                        TabIndex="5" />
                                                    <asp:ImageButton runat="server" ID="imbRemove" SkinID="imbdeletegrid" OnClick="ActionHandler"
                                                        CommandName="GRIDDELETE" ToolTip="<%$ resources:Captions, Remove %>" CommandArgument="<%# Eval(Resources.DataFieldRes.ConstPK) %>"
                                                        OnClientClick="return ShowDeleteConfirm(this);" TabIndex="6" />
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <uc1:PagerControl ID="uclPaging" runat="server" />
                                </div>
                            </div>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <div id="addItem" runat="server">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblProductionCode" runat="server" Text="<%$ resources:Controls,GenralPropertyCode%>"
                                                    AssociatedControlID="txtProductCode" />
                                                <asp:TextBox ID="txtProductCode" runat="server" CssClass="medium" MaxLength="50"
                                                    TabIndex="7" />
                                                <asp:Label ID="lblValidProductionCode" runat="server" Text="*" CssClass="star" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblProductionName" runat="server" Text="<%$ resources:Controls,GeneralPropertyName%>"
                                                    AssociatedControlID="txtProductName" />
                                                <asp:TextBox ID="txtProductName" runat="server" MaxLength="100" TabIndex="8" CssClass="input-half" />
                                                <asp:Label ID="lblValidProductionName" runat="server" Text="*" CssClass="star" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblSequence" runat="server" Text="<%$ resources:Controls,Sequence%>"
                                                    AssociatedControlID="txtSequence" />
                                                <asp:TextBox ID="txtSequence" runat="server" CssClass="medium" MaxLength="5" onkeypress="return isNumberKey(event)"
                                                    TabIndex="9" />
                                                <asp:Label ID="lblValidSequence" runat="server" Text="*" CssClass="star" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblParent" runat="server" Text="<%$ resources:Parent%>" AssociatedControlID="ddlParent"
                                                    Visible="false" />
                                                <asp:DropDownList ID="ddlParent" runat="server" TabIndex="9" Visible="false" CssClass="select-half-a">
                                                </asp:DropDownList>
                                                <%--<asp:DropDownList ID="ddlParent" runat="server" TabIndex="1"  ></asp:DropDownList>--%>

                                                <asp:Label ID="lblSaleEffectDate" runat="server" Text="<%$ resources:SaleEffectDate %>" AssociatedControlID="ddlSaleEffectDate"></asp:Label>
                                                <asp:DropDownList ID="ddlSaleEffectDate" runat="server" TabIndex="9" CssClass="select-half-a"></asp:DropDownList>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label ID="lblProductionDesc" runat="server" Text="<%$ resources:Controls,Description%>"
                                                    AssociatedControlID="txtProductDesc" /><asp:TextBox ID="txtProductDesc" runat="server"
                                                        TextMode="MultiLine" MaxLength="200" TabIndex="10" CssClass="multiline-2col"
                                                        onkeydown="limitText(this,200);" onkeyup="limitText(this,200);" />
                                            </div>
                                            <div class="clear">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblProductActive" runat="server" Text="<%$ resources:Active%>" AssociatedControlID="chkProductActive" />
                                                <asp:CheckBox ID="chkProductActive" runat="server" Checked="true" TabIndex="10" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblCBM" runat="server" Text="<%$ resources:Controls,CBM%>" Visible="false"
                                                    AssociatedControlID="txtCBM" />
                                                <asp:TextBox ID="txtCBM" runat="server" CssClass="medium" MaxLength="30" Visible="false"
                                                    TabIndex="10" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="search-colapse-b">
                                    <h1 style="padding: 20px 0">
                                        <asp:Literal ID="ltrGroupDetails" runat="server" Text="<%$ resources: SequeneProperties %>" /></h1>
                                    <asp:ImageButton runat="server" ID="imbShowGroupDetails" OnClientClick="javascript:return ShowHideGroupDetails(1);"
                                        SkinID="imbArrowShow" ToolTip="<%$ resources:Controls,ShowDetails%>" />
                                    <asp:ImageButton runat="server" ID="imbHideGroupDetails" OnClientClick="javascript:return ShowHideGroupDetails();"
                                        Style="display: none" SkinID="imbArrowHide" ToolTip="<%$ resources:Controls,HideDetails%>" />
                                    <div class="clear">
                                    </div>
                                </div>
                                <div id="divGroupDetails" runat="server">
                                    <table class="table-devides" >
                                        <tr>
                                            <td>
                                                  <div class="div2col-S">
                                                    <asp:Label ID="lblSeq1" runat="server" Text="<%$ resources: Sequence1%>" AssociatedControlID="txtSequence" />
                                                    <asp:DropDownList ID="ddlSequence1" runat="server" CssClass="select-half-a" TabIndex="12">
                                                    </asp:DropDownList>
                                                   
                                                </div>
                                            </td>
                                            <td>
                                                  <div class="div2col-S">
                                                    <asp:Label ID="lblSeq2" runat="server" Text="<%$ resources: Sequence2%>" AssociatedControlID="txtSequence" />
                                                    <asp:DropDownList ID="ddlSequence2" runat="server" CssClass="select-half-a" TabIndex="13">
                                                    </asp:DropDownList>
                                                   
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                               <div class="div2col-S">
                                                    <asp:Label ID="lblSeq3" runat="server" Text="<%$ resources: Sequence3%>" AssociatedControlID="txtSequence" />
                                                    <asp:DropDownList ID="ddlSequence3" runat="server" CssClass="select-half-a" TabIndex="14">
                                                    </asp:DropDownList>
                                                    
                                                </div>
                                            </td>
                                            <td>
                                               <div class="div2col-S">
                                                    <asp:Label ID="lblSeq4" runat="server" Text="<%$ resources: Sequence4%>" AssociatedControlID="txtSequence" />
                                                    <asp:DropDownList ID="ddlSequence4" runat="server" CssClass="select-half-a" TabIndex="15">
                                                    </asp:DropDownList>
                                                   
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label ID="lblSeq5" runat="server" Text="<%$ resources: Sequence5%>" AssociatedControlID="txtSequence" />
                                                    <asp:DropDownList ID="ddlSequence5" runat="server" CssClass="select-half-a" TabIndex="16">
                                                    </asp:DropDownList>
                                                   
                                                </div>
                                            </td>
                                            <td>
                                               <div class="div2col-S">
                                                    <asp:Label ID="lblSeq6" runat="server" Text="<%$ resources: Sequence6%>" AssociatedControlID="txtSequence" />
                                                    <asp:DropDownList ID="ddlSequence6" runat="server" CssClass="select-half-a" TabIndex="17">
                                                    </asp:DropDownList>
                                                   
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                               <div class="div2col-S">
                                                    <asp:Label ID="lblSeq7" runat="server" Text="<%$ resources: Sequence7%>" AssociatedControlID="txtSequence" />
                                                    <asp:DropDownList ID="ddlSequence7" runat="server" CssClass="select-half-a" TabIndex="18">
                                                    </asp:DropDownList>
                                                   
                                                </div>
                                            </td>
                                            <td>
                                               <div class="div2col-S">
                                                    <asp:Label ID="lblSeq8" runat="server" Text="<%$ resources: Sequence8%>" AssociatedControlID="txtSequence" />
                                                    <asp:DropDownList ID="ddlSequence8" runat="server" CssClass="select-half-a" TabIndex="19">
                                                    </asp:DropDownList>
                                                   
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                               <div class="div2col-S">
                                                    <asp:Label ID="lblSeq9" runat="server" Text="<%$ resources: Sequence9%>" AssociatedControlID="txtSequence" />
                                                    <asp:DropDownList ID="ddlSequence9" runat="server" CssClass="select-half-a" TabIndex="20">
                                                    </asp:DropDownList>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" runat="server" CssClass="last-modified" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="dialog-Template" style="width: 550px; height: 250px; display: none" title="Create Group">
                    <div class="content-wrapper">
                        <table class="table-devide">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblGroupCode" runat="server" Text="<%$ resources:Controls,GenralPropertyCode%>"
                                            AssociatedControlID="txtGroupCode" />
                                        <asp:TextBox ID="txtGroupCode" runat="server" MaxLength="100" TabIndex="12" />
                                        <asp:Label ID="lblValidGroupCode" runat="server" Text="*" CssClass="star" />
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblGroupName" runat="server" Text="<%$ resources:Controls,GeneralPropertyName%>"
                                            AssociatedControlID="txtGroupName" />
                                        <asp:TextBox ID="txtGroupName" runat="server" MaxLength="200" TabIndex="13" />
                                        <asp:Label ID="lblValidGroupName" runat="server" Text="*" CssClass="star" />
                                        <asp:ImageButton ID="imbSaveGroup" runat="server" CommandName="GROUPSAVE" SkinID="imbaddnew"
                                            OnClientClick="return ValidateGroup()" ValidationGroup="ProductGroup" OnClick="ActionHandler"
                                            TabIndex="15" />
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="div2col-S">
                                        <asp:Label ID="lblGroupSequence" runat="server" Text="<%$ resources:Controls,Sequence%>"
                                            AssociatedControlID="txtGroupSequence" /><asp:TextBox ID="txtGroupSequence" runat="server"
                                                MaxLength="2" TabIndex="14" CssClass="small numeric" onkeypress="return isNumberKey(event)" />
                                        <asp:Label ID="lblValidGroupSequence" runat="server" Text="*" CssClass="star" />
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div class="clear">
                        </div>
                        <div class="gridwrap">
                            <asp:GridView runat="server" ID="grdGroupsMst" Width="100%" PageSize="<%$ resources:PageSize %>"
                                AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                OnSorting="ActionHandler">
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ resources:Controls,GenralPropertyCode%>" SortExpression="<%$ resources:DataFieldRes,GroupCode %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGCode" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.GroupCode)) %>'
                                                Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.GroupCode)),30) %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="35%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Controls,GeneralPropertyName%>" SortExpression="<%$ resources:DataFieldRes,GroupName %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.GroupName)) %>'
                                                Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.GroupName)),30) %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="35%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Controls,Sequence%>" SortExpression="<%$ resources:DataFieldRes,GroupSequence %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGSequence" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.GroupSequence) %>'
                                                Text='<%# Eval(Resources.DataFieldRes.GroupSequence)%>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="6%" HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" OnClick="ActionHandler"
                                                CommandName="GROUPGRIDEDIT" ToolTip="<%$ resources:Controls,Edit  %>" CommandArgument="<%# Eval(Resources.DataFieldRes.GroupPK) %>" />
                                            <asp:ImageButton runat="server" ID="imbRemove" SkinID="imbdeletegrid" OnClick="ActionHandler"
                                                CommandName="GROUPGRIDDELETE" ToolTip="<%$ resources:Captions, Remove %>" CommandArgument="<%# Eval(Resources.DataFieldRes.GroupPK) %>"
                                                OnClientClick="return ShowDeleteConfirm(this);" />
                                        </ItemTemplate>
                                        <ItemStyle Width="24%" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div id="diverrorGroup" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litGroupErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary
                        ID="vsPageGroup" ValidationGroup="ProductGroup" runat="server" />
                </div>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary
                        ID="vsPage" ValidationGroup="Product" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
