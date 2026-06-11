<%@ Page Title="<%$ Resources:Captions,Title_StockReconciliation %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="StockReconciliation.aspx.cs" Inherits="ERPSMS_v01.Stock.StockReconciliation"
    Theme="ClassicExt" %>
    
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtDate");
        }
        function SetTabs(tab) {
            /// LocationChange = 1,
            /// StoreChange = 2,
            /// QtyChange = 3,
            /// Addition = 4,
            /// Deletion = 5  
            if (tab == 1) { 
                $("[id$='spnLocationChange']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnLocationChange']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnStoreChange']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnStoreChange']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnDeletion']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnDeletion']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnAddition']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnAddition']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnQtyChange']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnQtyChange']").removeClass("tab-active").addClass("tab-inactive");

                $("[id$=divLocationChange]").show(); 
                $("[id$=divStoreChange]").hide();
                $("[id$=divAddition]").hide();
                $("[id$=divDeletion]").hide();
                $("[id$=divQtyChange]").hide();
            }
            else if (tab == 2) {
                $("[id$='spnStoreChange']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnStoreChange']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnLocationChange']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnLocationChange']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnDeletion']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnDeletion']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnAddition']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnAddition']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnQtyChange']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnQtyChange']").removeClass("tab-active").addClass("tab-inactive");

                $("[id$=divLocationChange]").hide();
                $("[id$=divStoreChange]").show();
                $("[id$=divAddition]").hide();
                $("[id$=divDeletion]").hide();
                $("[id$=divQtyChange]").hide();
            }
            else if (tab == 3) {
                $("[id$='spnQtyChange']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnQtyChange']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnLocationChange']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnLocationChange']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnStoreChange']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnStoreChange']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnDeletion']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnDeletion']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnAddition']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnAddition']").removeClass("tab-active").addClass("tab-inactive");

                $("[id$=divLocationChange]").hide();
                $("[id$=divStoreChange]").hide();
                $("[id$=divAddition]").hide();
                $("[id$=divDeletion]").hide();
                $("[id$=divQtyChange]").show();
            }
            else if (tab == 4) {
                $("[id$='spnAddition']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnAddition']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnQtyChange']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnQtyChange']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnLocationChange']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnLocationChange']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnStoreChange']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnStoreChange']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnDeletion']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnDeletion']").removeClass("tab-active").addClass("tab-inactive");

                $("[id$=divLocationChange]").hide();
                $("[id$=divStoreChange]").hide();
                $("[id$=divAddition]").show();
                $("[id$=divDeletion]").hide();
                $("[id$=divQtyChange]").hide();
            }
            else if (tab == 5) {
                $("[id$='spnDeletion']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnDeletion']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnLocationChange']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnLocationChange']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnStoreChange']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnStoreChange']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnAddition']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnAddition']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnQtyChange']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnQtyChange']").removeClass("tab-active").addClass("tab-inactive");

                $("[id$=divLocationChange]").hide();
                $("[id$=divStoreChange]").hide();
                $("[id$=divAddition]").hide();
                $("[id$=divDeletion]").show();
                $("[id$=divQtyChange]").hide();
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
        function ValidateNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
                return false; //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }

        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsgRelPrdt]").hide();
                ShowErrorMessage($("#RelPrdtdiverror").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlStockReconln" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label ID="lblBreadCrum" runat="server" />
                                </ul>
                                <ul id="pnlEntry" runat="server">
                                    <li id="pnlSave" runat="server" style="display:none;" >
                                        <asp:Button ID="btnSave" runat="server" CommandName="RECONSILE" Text="<%$ resources:Controls,Reconcile %>"
                                            SkinID="btnInner-Save" CommandArgument="SEC_ActionPanel" ToolTip="<%$ resources:Controls,Reconcile %>"
                                            OnClick="ActionHandler" />
                                    </li> 
                                    <li id="pnlPrint" runat="server">
                                        <asp:Button ID="btnPrint" runat="server" CommandName="PRINT" Text="<%$ resources:Controls,Print %>"
                                            SkinID="btnInner-Save" CommandArgument="SEC_ActionPanel" ToolTip="<%$ resources:Controls,Print %>"
                                            OnClick="ActionHandler" TabIndex="7"/>
                                    </li> 
                                </ul> 
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <div id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnLocationChange" runat="server" class="tab-active">
                            <asp:LinkButton ID="lbnLocationChange" runat="server" Text="<%$ resources:LocationChange %>"
                                CommandName="LOCATIONCHANGE" CssClass="tab-active" OnClick="ActionHandler" TabIndex="1"
                                ToolTip="<%$ resources:LocationChange %>" />
                        </span></li>
                        <li><span id="spnStoreChange" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnStoreChange" runat="server" Text="<%$ resources:StoreChange %>"
                                CommandName="STORECHANGE" CssClass="tab-inactive" OnClick="ActionHandler" TabIndex="2"
                                ToolTip="<%$ resources:StoreChange %>" />
                        </span></li>
                        <li><span id="spnAddition" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnAddition" runat="server" Text="<%$ resources:Addition %>"
                                CommandName="ADDITION" CssClass="tab-inactive" OnClick="ActionHandler" TabIndex="3"
                                ToolTip="<%$ resources:Addition %>" />
                        </span></li>
                        <li><span id="spnDeletion" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnDeletion" runat="server" Text="<%$ resources:Deletion %>"
                                CommandName="DELETION" CssClass="tab-inactive" OnClick="ActionHandler" TabIndex="4"
                                ToolTip="<%$ resources:Deletion %>" />
                        </span></li>
                        <li style="display:none;"><span id="spnQtyChange" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnQtyChange" runat="server" Text="<%$ resources:QtyChange %>"
                                CommandName="QTYCHANGE" CssClass="tab-inactive" OnClick="ActionHandler" TabIndex="5"
                                ToolTip="<%$ resources:QtyChange %>" />
                        </span></li>
                    </ul>
                </div>
                <div class="clear">
                </div>
                <asp:Table ID="tblTemplate" runat="server" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <table class="table-devide" style="margin-top: 15px!important;">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblStkDate" runat="server" Text="<%$ resources:StockTakeMonth %>"
                                                AssociatedControlID="txtDate"></asp:Label>
                                            <asp:TextBox ID="txtDate" runat="server" CssClass="input-small input-disabled" 
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" Enabled="false"></asp:TextBox> 
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblAdvProductGrade" Text="" AssociatedControlID="lblAdvProductGrade"
                                                CssClass="middle-lbl "></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <%--LocationChange--%>
                            <div class="gridwrap" id="divLocationChange">
                                <asp:GridView runat="server" ID="grdLocationChange" Width="100%" AllowSorting="True"
                                    AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:BinNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBinNo" runat="server" ToolTip="" Text='<%# Eval("BDT_BIN_CARD_TEXT") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Qty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBinQty" runat="server" ToolTip="" Text='<%# Eval("BDT_QTY") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="<%$ resources:Store %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptStoreText" runat="server" ToolTip="" Text='<%# Eval("BHT_DEPT_STORE_TEXT") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="<%$ resources:CurrLoc %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptLocationText" runat="server" ToolTip="" Text='<%# Eval("BDT_DEPT_LOC_TEXT") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ActualLoc %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptActulocationText" runat="server" ToolTip="" Text='<%# Eval("BDT_ACT_DEPT_LOC_TEXT") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div style="display: none;">
                                    <uc1:PagerControl ID="uclPaging" runat="server" />
                                </div>
                            </div>
                            <%--StoreChange--%>
                            <div class="gridwrap" id="divStoreChange">
                                <asp:GridView runat="server" ID="grdStoreChange" Width="100%" AllowSorting="True"
                                    AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns> 
                                        <asp:TemplateField HeaderText="<%$ resources:BinNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBinNo" runat="server" ToolTip="" Text='<%# Eval("BDT_BIN_CARD_TEXT") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Qty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBinQty" runat="server" ToolTip="" Text='<%# Eval("BDT_QTY") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="<%$ resources:Store %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptStoreText" runat="server" ToolTip="" Text='<%# Eval("BHT_DEPT_STORE_TEXT") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="<%$ resources:CurrLoc %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptLocationText" runat="server" ToolTip="" Text='<%# Eval("BDT_DEPT_LOC_TEXT") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ActualLoc %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptActulocationText" runat="server" ToolTip="" Text='<%# Eval("BDT_ACT_DEPT_LOC_TEXT") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <%--Addition--%>
                            <div class="gridwrap" id="divAddition">
                                <asp:GridView runat="server" ID="grdAddition" Width="100%" AllowSorting="True" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:BinNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBinNo" runat="server" ToolTip="" Text='<%# Eval("BDT_BIN_CARD_TEXT") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Qty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBinQty" runat="server" ToolTip="" Text='<%# Eval("BDT_QTY") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="<%$ resources:Store %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptStoreText" runat="server" ToolTip="" Text='<%# Eval("BHT_DEPT_STORE_TEXT") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="<%$ resources:CurrLoc %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptLocationText" runat="server" ToolTip="" Text='<%# Eval("BDT_DEPT_LOC_TEXT") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ActualLoc %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptActulocationText" runat="server" ToolTip="" Text='<%# Eval("BDT_ACT_DEPT_LOC_TEXT") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <%--Deletion--%>
                            <div class="gridwrap" id="divDeletion">
                                <asp:GridView runat="server" ID="grdDeletion" Width="100%" AllowSorting="True" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:BinNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBinNo" runat="server" ToolTip="" Text='<%# Eval("BDT_BIN_CARD_TEXT") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Qty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBinQty" runat="server" ToolTip="" Text='<%# Eval("BDT_QTY") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="<%$ resources:Store %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptStoreText" runat="server" ToolTip="" Text='<%# Eval("BHT_DEPT_STORE_TEXT") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="<%$ resources:CurrLoc %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptLocationText" runat="server" ToolTip="" Text='<%# Eval("BDT_DEPT_LOC_TEXT") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ActualLoc %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptActulocationText" runat="server" ToolTip="" Text='<%# Eval("BDT_ACT_DEPT_LOC_TEXT") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <%--QtyChange--%>
                            <div class="gridwrap" id="divQtyChange">
                                <asp:GridView runat="server" ID="grdQtyChange" Width="100%" AllowSorting="True" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:BinNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBinNo" runat="server" ToolTip="" Text='<%# Eval("BDT_BIN_CARD_TEXT") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Qty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBinQty" runat="server" ToolTip="" Text='<%# Eval("BDT_QTY") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="<%$ resources:Store %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptStoreText" runat="server" ToolTip="" Text='<%# Eval("BHT_DEPT_STORE_TEXT") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="<%$ resources:CurrLoc %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptLocationText" runat="server" ToolTip="" Text='<%# Eval("BDT_DEPT_LOC_TEXT") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ActualLoc %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptActulocationText" runat="server" ToolTip="" Text='<%# Eval("BDT_ACT_DEPT_LOC_TEXT") %>' />
                                            </ItemTemplate> 
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="Save" runat="server" />
                </div>
                <div id="RelPrdtdiverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsgRelPrdt" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
                </div>
            </div>
            <asp:HiddenField ID="hdfStockInitPk" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
