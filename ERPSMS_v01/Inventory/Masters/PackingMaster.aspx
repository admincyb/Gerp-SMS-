<%@ Page Title="<%$ Resources:Captions,Title_PackingSpec %>" Language="C#" Theme="ClassicExt"
    EnableEventValidation="false" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="PackingMaster.aspx.cs" Inherits="ERPSMS_v01.Inventory.Masters.PackingMaster"
    ValidateRequest="false" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        function InitComponents() {
            $("[id$='lblValidPackCode']").hide();
            $("[id$='lblValidPackSpec']").hide();
            $("[id$='lblValidPackType']").hide();
            $("[id$='lblValidPouchpcs']").hide();
            $("[id$='lblValidInnerBox']").hide();
            $("[id$='lblValidInnerCarton']").hide();
            $("[id$='lblValidZipperBag']").hide();
            $("[id$='lblValidMasterCarton']").hide();
            $("[id$='lblValidSack']").hide();
            $("[id$='lblValidPlainOuterBag']").hide();
            $("[id$='lblValidWallet']").hide();
            $("[id$='lblValidPrintedOuterBag']").hide();
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

        function CalcTotalPcs() {
            var PouchPcs = ($("[id$='txtPouchPcs']").val() == "" || $("[id$='txtPouchPcs']").val() == "0") ? 1 : $("[id$='txtPouchPcs']").val();
            var InnerBox = ($("[id$='txtInnerBox']").val() == "" || $("[id$='txtInnerBox']").val() == "0") ? 1 : $("[id$='txtInnerBox']").val();
            var InnerCarton = ($("[id$='txtInnerCarton']").val() == "" || $("[id$='txtInnerCarton']").val() == "0") ? 1 : $("[id$='txtInnerCarton']").val();
            var ZipperBag = ($("[id$='txtZipperBag']").val() == "" || $("[id$='txtZipperBag']").val() == "0") ? 1 : $("[id$='txtZipperBag']").val();
            var MasterCarton = ($("[id$='txtMasterCarton']").val() == "" || $("[id$='txtMasterCarton']").val() == "0") ? 1 : $("[id$='txtMasterCarton']").val();
            var Sack = ($("[id$='txtSack']").val() == "" || $("[id$='txtSack']").val() == "0") ? 1 : $("[id$='txtSack']").val();
            var PlainOuterBag = ($("[id$='txtPlainOuterBag']").val() == "" || $("[id$='txtPlainOuterBag']").val() == "0") ? 1 : $("[id$='txtPlainOuterBag']").val();
            var Wallet = ($("[id$='txtWallet']").val() == "" || $("[id$='txtWallet']").val() == "0") ? 1 : $("[id$='txtWallet']").val();
            var PrintedOuterBag = ($("[id$='txtPrintedOuterBag']").val() == "" || $("[id$='txtPrintedOuterBag']").val() == "0") ? 1 : $("[id$='txtPrintedOuterBag']").val();
            var totalPcs = PouchPcs * InnerBox * InnerCarton * ZipperBag * MasterCarton * Sack * PlainOuterBag * Wallet * PrintedOuterBag;
            if (
            (
            $("[id$='txtPouchPcs']").val() == "" &&
            $("[id$='txtInnerBox']").val() == "" &&
            $("[id$='txtInnerCarton']").val() == "" &&
            $("[id$='txtZipperBag']").val() == "" &&
            $("[id$='txtMasterCarton']").val() == "" &&
            $("[id$='txtSack']").val() == "" &&
            $("[id$='txtPlainOuterBag']").val() == "" &&
            $("[id$='txtWallet']").val() == "" &&
            $("[id$='txtPrintedOuterBag']").val() == "") ||
            (
            $("[id$='txtPouchPcs']").val() == "0" &&
            $("[id$='txtInnerBox']").val() == "0" &&
            $("[id$='txtInnerCarton']").val() == "0" &&
            $("[id$='txtZipperBag']").val() == "0" &&
            $("[id$='txtMasterCarton']").val() == "0" &&
            $("[id$='txtSack']").val() == "0") &&
            $("[id$='txtPlainOuterBag']").val() == "0" &&
            $("[id$='txtWallet']").val() == "0" &&
            $("[id$='txtPrintedOuterBag']").val() == "0"
            )
            totalPcs = 0;
            $("[id$='txtTotalPcs']").val(totalPcs);
            // CalcTotalWeight();
        }

        function ValidateNow() {
            var isValid = true;
            var msg = "";

            $("[id$='lblValidPackCode']").hide();
            $("[id$='lblValidPackSpec']").hide();
            $("[id$='lblValidPackType']").hide();
            $("[id$='lblValidPouchpcs']").hide();
            $("[id$='lblValidInnerBox']").hide();
            $("[id$='lblValidInnerCarton']").hide();
            $("[id$='lblValidZipperBag']").hide();
            $("[id$='lblValidMasterCarton']").hide();
            $("[id$='lblValidSack']").hide();
            $("[id$='lblValidPlainOuterBag']").hide();
            $("[id$='lblValidWallet']").hide();
            $("[id$='lblValidPrintedOuterBag']").hide();
            var totlpcs = $("[id$='txtTotalPcs']").val() == "" ? 0 : parseInt($("[id$='txtTotalPcs']").val());

            if ($("[id$='txtPackCode']").val() == '') {
                $("[id$='lblValidPackCode']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_PackingCode") %></li></ul>';
            }

            if ($("[id$='txtPackSpecName']").val() == '') {
                $("[id$='lblValidPackSpec']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_PackingSpecs") %></li></ul>';
            }
            if ($("[id$='ddlPackingType']").val() == '-1') {
                $("[id$='lblValidPackType']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_PackingType") %></li></ul>';
            }
            if (totlpcs <= 0) {
                $("[id$='lblTotalPcs']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_TotalPcs") %></li></ul>';
            }

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
            //               }

            if (!isValid) {
                $("[id$=litErrorMsg]").show();
                $("[id$=litErrorMsg]").html(msg);
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
            }
            return isValid;
        }





        function EnableDisablePouchPcs() {

            if ($("[id$='ddlPouchType']").val() == '-1') {
                //                $("[id$='txtPouchPcs']").val('');
                //                $("[id$='txtPouchPcs']").attr("disabled", true);
                //                $("[id$='txtPouchPcs']").removeClass("small numeric").addClass("small input-disabled numeric");

                $("[id$='txtPouchPcs']").attr("disabled", false);
                $("[id$='txtPouchPcs']").removeClass("small input-disabled numeric").addClass("small numeric");
                //                $("[id$='txtPouchWeight']").attr("disabled", false);
                //                $("[id$='txtPouchWeight']").removeClass("small input-disabled numeric").addClass("small numeric");

                //                $("[id$='txtPouchWeight']").val('');
                //                $("[id$='txtPouchWeight']").attr("disabled", true);
                //                $("[id$='txtPouchWeight']").removeClass("small numeric").addClass("small input-disabled numeric");
            }
            else {
                $("[id$='txtPouchPcs']").attr("disabled", false);
                $("[id$='txtPouchPcs']").removeClass("small input-disabled numeric").addClass("small numeric");
                //                        $("[id$='txtPouchWeight']").attr("disabled", false);
                //                        $("[id$='txtPouchWeight']").removeClass("small input-disabled numeric").addClass("small numeric");
            }
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        function isFloatNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                if (charCode == 46)
                    return true;
                return false;
            }

            return true;
        }

              
            
        
    </script>
</asp:Content>
<asp:Content ID="cntMain" runat="server" ContentPlaceHolderID="MainContent">
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
                            <asp:LinkButton ID="lbnPackingSpecs" runat="server" Text="<%$ resources:Controls,PackingSpecs %>"
                                CommandName="ACTIVATE" CssClass="tab-inactive" OnClick="ActionHandler" TabIndex="9"
                                ToolTip="<%$ resources:Controls,PackingSpecs %>" />
                        </span></li>
                        <li><span id="spnPackingSpecsMapping" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lblPackingSpecsMapping" runat="server" Text="<%$ resources:Controls,packingSpecsMapping %>"
                                CommandName="PACKINGMAPPING" CssClass="tab-inactive" OnClick="ActionHandler"
                                TabIndex="10" ToolTip="<%$ resources:Controls,packingSpecsMapping %>" />
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table ID="tblTemplate" runat="server" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-wrap-custom">
                                <asp:Label ID="lblFilterBy" runat="server" Text="<%$ resources:Controls,FilterBy %>"
                                    AssociatedControlID="ddlFilterBy" />
                                <asp:DropDownList ID="ddlFilterBy" runat="server" TabIndex="1">
                                    <asp:ListItem Text="<%$ resources:Controls,PackingSpecification %>" Value="<%$ resources:DataFieldRes,PackingSpecs %>" />
                                    <asp:ListItem Text="<%$ resources:Controls,PackingType %>" Value="<%$ resources:DataFieldRes,PackingType %>" />
                                    <asp:ListItem Text="<%$ resources:Controls,PackingCode %>" Value="<%$ resources:DataFieldRes,PacSpecCode %>" />
                                </asp:DropDownList>
                                <asp:TextBox ID="txtSearchBy" runat="server" onkeydown="return Search(event);" OnClick="ActionHandler"
                                    TabIndex="2" />
                                <asp:Button ID="btnSearch" SkinID="btnInner-Go" runat="server" Text="<%$ resources:Controls,Go %>"
                                    ToolTip="<%$ resources:Controls,Go %>" CommandName="SEARCH" OnClick="ActionHandler"
                                    TabIndex="3" />
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdPackingMst" Width="100%" PageSize="<%$ resources:PageSize %>"
                                    AllowPaging="true" OnPageIndexChanging="ActionHandler" AllowSorting="True" OnSorting="ActionHandler"
                                    AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" GroupName="SelectOne"
                                                    OnCheckedChanged="ActionHandler" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    TabIndex="4" />
                                                <asp:HiddenField ID="hfPackingPK" runat="server" Value='<%#Eval(Resources.DataFieldRes.PackingMstPK) %>' />
                                                 <asp:HiddenField ID="hdfapshasbrandprd" runat="server" Value='<%# Eval("APS_HAS_BRAND_PRD") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,PackingCode%>" SortExpression="<%$ resources:DataFieldRes,PackingCode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPackSpecCode" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.PackingCode))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.PackingCode)),35) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="22%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,PackingSpecification%>" SortExpression="<%$ resources:DataFieldRes,PackingSpecs %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPackSpecName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.PackingSpecs))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.PackingSpecs)),55) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="34%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,PackingType%>" SortExpression="<%$ resources:DataFieldRes,PackingType %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPackingType" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.PackingType) %>'
                                                    Text='<%#  ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.PackingType),40) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,TotalPieces %>" SortExpression="<%$ resources:DataFieldRes,TotalPieces %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalPieces" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.TotalPieces) %>'
                                                    Text='<%# Eval(Resources.DataFieldRes.TotalPieces) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" HorizontalAlign="Right" />
                                              <HeaderStyle CssClass="amount-numeric" Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblgap" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Composition%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblComposition" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.Composition) %>'
                                                    Text='<%# Eval(Resources.DataFieldRes.Composition) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>

                                         <asp:TemplateField ItemStyle-Width="2%">
                                            <ItemTemplate>
                                             <asp:ImageButton ID="imbGenerateBrand" runat="server" SkinID="add_small-icon" 
                                                 ToolTip="Generate Brand" CssClass="Active" Enabled="true" OnClick="ActionHandler" CommandName="GENERATEBRAND"/>                                                                                                       
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <%-- <uc1:PagerControl ID="uclPaging" runat="server" />--%>
                            </div>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                           <%-- <div class="detail-co2" runat="server" id="div1">
                                <div class="div2col-S">
                                    <asp:Label ID="lbl1" runat="server" AssociatedControlID="lblPackingSpecCodeHdr" Text="<%$ resources:PackingSpecCode %>"></asp:Label>
                                    <asp:Label ID="lblPackingSpecCodeHdr" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="div2col-S">
                                    <asp:Label ID="lbl2" runat="server" AssociatedControlID="lblPackingSpecTotalHdr"
                                        Text="<%$ resources:TotalPcs1 %>"></asp:Label>
                                    <asp:Label ID="lblPackingSpecTotalHdr" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="clear">
                                </div>
                            </div>--%>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblPackCode" runat="server" Text="<%$ resources:Controls,PackingCode%>"
                                                AssociatedControlID="txtPackCode" />
                                            <asp:TextBox ID="txtPackCode" runat="server" MaxLength="100" CssClass="input-half" TabIndex="10" />
                                            <asp:Label ID="lblValidPackCode" runat="server" Text="*" CssClass="star" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblPackSpec" runat="server" Text="Packing Spec Name" AssociatedControlID="txtPackSpecName" />
                                            <asp:TextBox ID="txtPackSpecName" runat="server" MaxLength="50" CssClass="input-half" TabIndex="11" />
                                            <asp:Label ID="lblValidPackSpec" runat="server" Text="*" CssClass="star" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblPType" runat="server" Text="<%$ resources:Controls,PackingType%>"
                                                AssociatedControlID="ddlPackingType" />
                                            <asp:DropDownList ID="ddlPackingType" runat="server" CssClass="select-half-a" TabIndex="12" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true" />
                                            <asp:Label ID="lblValidPackType" runat="server" Text="*" CssClass="star" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <div class="div2col-S">
                                                <asp:Label ID="lblPouchpcs" runat="server" Text="Pouch Pack" AssociatedControlID="txtPouchPcs" />
                                                <asp:TextBox ID="txtPouchPcs" runat="server" MaxLength="4" Enabled="true" onkeypress="return isNumberKey(event)"
                                                    CssClass="input-small numeric" onchange="CalcTotalPcs()" TabIndex="13" />
                                                <asp:Label ID="lblValidPouchpcs" runat="server" Text="*" CssClass="star" />
                                                <asp:ImageButton ID="imgbtnPouchInformation" runat="server" SkinID="information" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblInnerBox" runat="server" Text="Inner Box" AssociatedControlID="txtInnerBox" />
                                            <asp:TextBox ID="txtInnerBox" runat="server" onchange="CalcTotalPcs()" TabIndex="14"
                                                CssClass="input-small numeric" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                            <asp:Label ID="lblValidInnerBox" runat="server" Text="*" CssClass="star" />
                                            <asp:ImageButton ID="imgbtnInnerBoxInformation" runat="server" SkinID="information" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblInnerCarton" runat="server" Text="Inner Carton" AssociatedControlID="txtInnerCarton" />
                                            <asp:TextBox ID="txtInnerCarton" runat="server" onchange="CalcTotalPcs()" TabIndex="15"
                                                CssClass="input-small numeric" onkeypress="return isNumberKey(event)" />
                                            <asp:Label ID="lblValidInnerCarton" runat="server" Text="*" CssClass="star" />
                                            <asp:ImageButton ID="imgbtnInnerCartonInformation" runat="server" SkinID="information" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblZipperBag" runat="server" Text="Zipper Bag" AssociatedControlID="txtZipperBag" />
                                            <asp:TextBox ID="txtZipperBag" runat="server" onchange="CalcTotalPcs()" TabIndex="16"
                                                CssClass="input-small numeric" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                            <asp:Label ID="lblValidZipperBag" runat="server" Text="*" CssClass="star" />
                                            <asp:ImageButton ID="imgbtnZipperBagInformation" runat="server" SkinID="information" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblMasterCarton" runat="server" Text="Master Carton" AssociatedControlID="txtMasterCarton" />
                                            <asp:TextBox ID="txtMasterCarton" runat="server" onchange="CalcTotalPcs()" TabIndex="17"
                                                CssClass="input-small numeric" onkeypress="return isNumberKey(event)" />
                                            <asp:Label ID="lblValidMasterCarton" runat="server" Text="*" CssClass="star" />
                                            <asp:ImageButton ID="imgbtnMasterCartonInformation" runat="server" SkinID="information" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblSack" runat="server" Text="Sack / Bag" AssociatedControlID="txtSack" />
                                            <asp:TextBox ID="txtSack" runat="server" onchange="CalcTotalPcs()" TabIndex="18"
                                                CssClass="input-small numeric" onkeypress="return isNumberKey(event)" />
                                            <asp:Label ID="lblValidSack" runat="server" Text="*" CssClass="star" />
                                            <asp:ImageButton ID="imgbtnSackInformation" runat="server" SkinID="information" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblWallet" runat="server" Text="Wallet" AssociatedControlID="txtWallet" />
                                            <asp:TextBox ID="txtWallet" runat="server" onchange="CalcTotalPcs()" TabIndex="18"
                                                CssClass="input-small numeric" onkeypress="return isNumberKey(event)" />
                                            <asp:Label ID="lblValidWallet" runat="server" Text="*" CssClass="star" />
                                            <asp:ImageButton ID="imgbtnWalletInformation" runat="server" SkinID="information" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                         <div class="div2col-S">
                                            <asp:Label ID="lblPlainOuterBag" runat="server" Text="Plain Outer Bag" AssociatedControlID="txtPlainOuterBag" />
                                            <asp:TextBox ID="txtPlainOuterBag" runat="server" onchange="CalcTotalPcs()" TabIndex="18"
                                                CssClass="input-small numeric" onkeypress="return isNumberKey(event)" />
                                            <asp:Label ID="lblValidPlainOuterBag" runat="server" Text="*" CssClass="star" />
                                            <asp:ImageButton ID="imgbtnPlainOuterBagInformation" runat="server" SkinID="information" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                         <div class="div2col-S">
                                            <asp:Label ID="lblPrintedOuterBag" runat="server" Text="Printed Outer Bag" AssociatedControlID="txtPrintedOuterBag" />
                                            <asp:TextBox ID="txtPrintedOuterBag" runat="server" onchange="CalcTotalPcs()" TabIndex="18"
                                                CssClass="input-small numeric" onkeypress="return isNumberKey(event)" />
                                            <asp:Label ID="lblValidPrintedOuterBag" runat="server" Text="*" CssClass="star" />
                                            <asp:ImageButton ID="imgbtnPrintedOuterBag" runat="server" SkinID="information" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblTotalPcs" runat="server" Text="Total Pcs" AssociatedControlID="txtTotalPcs" />
                                            <asp:TextBox ID="txtTotalPcs" runat="server" CssClass="input-small input-disabled numeric"
                                                Enabled="false" Text="0" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblPackingDesc" runat="server" Text="<%$ resources:Controls,Description%>"
                                                AssociatedControlID="txtPackingDesc" />
                                            <asp:TextBox ID="txtPackingDesc" runat="server" TextMode="MultiLine" MaxLength="500"
                                                TabIndex="19" CssClass="multiline-3line" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" />
                                        </div>
                                        <div class="clear">
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
    </asp:UpdatePanel>
</asp:Content>
