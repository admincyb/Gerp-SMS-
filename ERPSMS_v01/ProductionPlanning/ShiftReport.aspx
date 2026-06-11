<%@ Page Title="<%$ Resources:Captions,Title_ShiftReport %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" 
    AutoEventWireup="true" EnableEventValidation="true" CodeBehind="ShiftReport.aspx.cs"
    Theme="Classic" Inherits="ERPSMS_v01.ProductionPlanning.ShiftReport" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function InitComponents() {
            GrandScriptUtils.DatePicker("txtShiftDate");
            ShowTooltip('ddlLine');
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

        function CheckNominations(sender, args) {
            var prodQty = $("[id$=txtQty]").val().trim();
            if (prodQty == "") {
                sender.errormessage = '<%= GetLocalResourceObject("Err_ValidQty") %>';
                args.IsValid = false;
            }
            else {
                prodQty = parseInt(prodQty);
                if ( prodQty < 1 || prodQty > 999999999 || isNaN(prodQty)) {
                    sender.errormessage = '<%= GetLocalResourceObject("Msg_ValidQty") %>';
                    args.IsValid = false;
                }
                else {
                    args.IsValid = true;
                }
                
            }


        }
        function CheckNominationsGrade(sender, args) {
            var gradeAQty = $("[id$=txtGradeA]").val().trim();
            if (gradeAQty == "") {
                sender.errormessage = '<%= GetLocalResourceObject("Err_Agrade") %>';
                args.IsValid = false;
            }
            else {
                gradeAQty = parseInt(gradeAQty);
                if (gradeAQty < 1 || gradeAQty > 999999999 || isNaN(gradeAQty)) {
                    sender.errormessage = '<%= GetLocalResourceObject("Msg_Agrade") %>';
                    args.IsValid = false;
                }
                else {
                    args.IsValid = true;
                }

            }


        }

        function ShowSessionConfirm(btn, type) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.Messages.Information %>';
            //            msg = type == 1 ? '<%= GetLocalResourceObject("ConfirmCancel").ToString() %>' : '<%= GetLocalResourceObject("ConfirmSave").ToString() %>';
            msg = '<%= GetLocalResourceObject("ConfirmDelete").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $(this).dialog("close");
                        __doPostBack(btn.name, '');
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        if (typeof AfterDeleteConfirmationCancel == "function") {
                            AfterDeleteConfirmationCancel(btn.id);
                        }
                        return false;
                    }
                }
            });
            return false;
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
                $("[id$='spnListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnDetails']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnDetails']").removeClass("tab-active").addClass("tab-inactive");
            }
            else {
                $("[id$='spnListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnDetails']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnDetails']").removeClass("tab-inactive").addClass("tab-active");
            }
        }
     
    </script>
</asp:Content>
<asp:Content ID="cntMain" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="udpMainContent" runat="server">
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
                                          <asp:Button ID="btnSave" runat="server" Text="<%$ resources:Save %>" ToolTip="<%$ resources:Save %>"  OnClick="ActionHandler"
                                                       EnableViewState="False" CommandName="SAVE" CommandArgument="SEC_ActionPanel"
                                                       ValidationGroup="Shift" CausesValidation="true" SkinID="btnInner-Save" TabIndex="13"
                                                       OnClientClick="javascript:ValidatePageNow('Shift')" />
                                    </li>
                                    <li id="pnlCancel" runat="server">
                                        <asp:Button ID="btnClose" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>" ToolTip="<%$Resources:Controls,Cancel%>"
                                                    CommandName="CANCEL" OnClick="ActionHandler"  TabIndex="14" />
                                    </li>
                                </ul>
                                <ul id="pnlListing" runat="server" style="display: none">
                                    <li>
                                         <asp:Button CommandName="ADDSHIFTDETAILS" runat="server" OnClick="ActionHandler" TabIndex ="5"
                                                    ID="btnAddShift" SkinID="btnInner-Save" Text="<%$ resources:AddShift %>" ToolTip ="<%$ resources:AddShift %>" /> 
                                    </li>
                                   
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnListing" runat="server" class="tab-active">
                            <asp:LinkButton ID="lbnListing" runat="server" Text="<%$ resources:Controls,List %>"
                                CommandName="CANCEL" CssClass="tab-active" OnClick="ActionHandler" TabIndex="1" ToolTip="<%$ resources:Controls,List %>"/>
                        </span></li>
                        <li><span id="spnDetails" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnDetails" runat="server" Text="<%$ resources:Controls,Details %>"
                                CommandName="ADDSHIFTDETAILS" CssClass="tab-inactive" OnClick="ActionHandler" TabIndex="2"  ToolTip="<%$ resources:Controls,Details %>" />
                        </span></li>
                    </ul>
                </div>
            </div>


            <div class="content-wrapper">
                <div id="divEntryForm" runat="server">
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="lblShiftDate" runat="server" Text="<%$ resources:Date %>" AssociatedControlID="txtShiftDate"></asp:Label>
                                    <asp:TextBox ID="txtShiftDate" runat="server" CssClass="medium" TabIndex="6" MaxLength="12"></asp:TextBox>
                                    <asp:HiddenField ID="hdfShiftDate" runat="server" />
                                    <div class="starwrap">
                                        <asp:RequiredFieldValidator ID="vrShiftDate" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="Shift" Text="*" runat="server" ControlToValidate="txtShiftDate"
                                            Display="Dynamic" ErrorMessage="<%$ resources:Msg_Valid_Date %>"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="vrgShiftDate" CssClass="star" ValidationGroup="Shift"
                                            runat="server" ControlToValidate="txtShiftDate" SetFocusOnError="true" ErrorMessage="<%$ resources:Msg_Valid_Date %>"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="clear">
                                    </div>
                                    <asp:Label ID="lblLine" runat="server" Text="<%$ resources:Line %>" AssociatedControlID="ddlLine" ></asp:Label>
                                    <asp:DropDownList ID="ddlLine" TabIndex="7" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                        CssClass="medium" onmouseover="javascript:ShowTooltip('ddlLine');">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="vrfLine" SetFocusOnError="true" ValidationGroup="Shift"
                                        CssClass="star" runat="server" ControlToValidate="ddlLine" Text="*" ErrorMessage="<%$ resources:Msg_SelectLine%>"
                                        InitialValue="-1" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <%--<asp:Label ID="lbnShiftNo" runat="server" Text="<%$ resources:ShiftNo %>" AssociatedControlID="txtShiftNo"></asp:Label>
                                    <asp:Label ID="txtShiftNo" runat="server" CssClass="medium"  ></asp:Label>--%>
                                    <asp:Label ID="lblShiftNoCaption" Text="<%$ resources:ShiftNo %>" AssociatedControlID="lblShiftNo"
                                        runat="server"></asp:Label>
                                    <asp:Label ID="lblShiftNo" runat="server" CssClass="medium"></asp:Label>
                                    <asp:HiddenField ID="hdfShiftNo" runat="server" Value="0" />
                                     <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                                            <asp:HiddenField ID="AST_CODE" runat="server" />
                                    <div class="clear">
                                    </div>
                                    <asp:Label ID="lblShift" runat="server" Text="<%$ resources:Shift %>" AssociatedControlID="ddlShift"></asp:Label>
                                    <asp:DropDownList ID="ddlShift" TabIndex="8" runat="server" CssClass="medium">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="vrfShift" SetFocusOnError="true" ValidationGroup="Shift"
                                        CssClass="star" runat="server" ControlToValidate="ddlShift" Text="*" ErrorMessage="<%$ resources:Msg_SelectShift%>"
                                        InitialValue="-1" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <%--<div class="clear">
                            </div>--%>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="detail-co3">
                        <table>
                            <tr>
                                <td width="70%">
                                    <asp:Label ID="lblLoad" runat="server" Text="<%$ resources:Product %>"></asp:Label>
                                </td>
                                <td width="2%"></td>
                                <td width="10%" align ="right" >
                                    <asp:Label ID="Label5" runat="server" Text="<%$ resources:QtyProduced %>"></asp:Label>
                                </td>
                                <td width="2%"></td>
                                <td width="10%" align ="right">
                                    <asp:Label ID="Label6" runat="server" Text="<%$ resources:GradeA %>"></asp:Label>
                                </td>
                                <td width="2%"></td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlProduct" TabIndex="9" runat="server" width="99%" onmouseover="javascript:ShowTooltip('ddlProduct');">
                                    </asp:DropDownList>
                                   
                                </td>
                                <td>
                                 <asp:RequiredFieldValidator ID="vrfProduct" SetFocusOnError="true" ValidationGroup="ShiftLine"
                                        CssClass="star" runat="server" ControlToValidate="ddlProduct" Text="*" ErrorMessage="<%$ resources:Msg_SelectProduct%>"
                                        InitialValue="-1" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtQty" TabIndex="10" Width="220px" MaxLength="9" runat="server" CssClass="medium numeric"></asp:TextBox>
                                    <%-- <asp:RequiredFieldValidator Style="float: left" runat="server" ID="reqQty" ControlToValidate="txtQty"
                                        Text="*" ErrorMessage="<%$ resources:Msg_ValidQty%>" CssClass="star" Display="Dynamic"
                                        ValidationGroup="ShiftLine"></asp:RequiredFieldValidator>
                                    --%>
                                </td>
                                <td>
                                    <asp:CustomValidator ID="customQty" runat="server" ValidateEmptyText="true" ClientValidationFunction="CheckNominations"
                                        ErrorMessage="<%$ resources:Err_ValidQty%>" Text="*" ControlToValidate="txtQty" CssClass="star" 
                                        Display="Dynamic" ValidationGroup="ShiftLine"></asp:CustomValidator>
                                    <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="<%$ resources:Msg_ValidQty%>"
                                        Text="*" ControlToValidate="txtQty" CssClass="star" Display="Dynamic" ValidationGroup="ShiftLine"
                                        ValidationExpression="[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?(\.[0-9][0-9]?)?"></asp:RegularExpressionValidator>--%>
                                   
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGradeA" TabIndex="11" Width="220px" MaxLength="9" runat="server" CssClass="medium numeric"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator Style="float: left" runat="server" ID="reqGradeA" ControlToValidate="txtGradeA"
                                        Text="*" ErrorMessage="<%$ resources:Msg_Agrade%>" CssClass="star" Display="Dynamic"
                                        ValidationGroup="ShiftLine"></asp:RequiredFieldValidator>--%>
                                 </td> 
                                 <td>
                                   <asp:CustomValidator ID="customGrade" runat="server" ValidateEmptyText="true" ClientValidationFunction="CheckNominationsGrade"
                                        Text="*" ErrorMessage="<%$ resources:Err_Agrade%>" ControlToValidate="txtGradeA"
                                        CssClass="star" Display="Dynamic" ValidationGroup="ShiftLine"></asp:CustomValidator>
                                    <%--<asp:RegularExpressionValidator ID="regFormerNos" runat="server" ErrorMessage="<%$ resources:Msg_Agrade%>"
                                        Text="*" ControlToValidate="txtGradeA" CssClass="star" Display="Dynamic" ValidationGroup="ShiftLine"
                                        ValidationExpression="[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?(\.[0-9][0-9]?)?"></asp:RegularExpressionValidator>--%>
                                    
                                   
                                </td>
                               <td>
                                     <asp:ImageButton ID="imdAdd" TabIndex="12" runat="server" OnClick="ActionHandler" ToolTip ="<%$ resources:Add %>"
                                        ValidationGroup="ShiftLine" CausesValidation="true" OnClientClick="javascript:ValidatePageNow('ShiftLine')"
                                        SkinID="imbaddnew" CommandName="ADD" CommandArgument="SEC_ActionPanel" />
                                    <asp:HiddenField ID="hdfSLNo" runat="server" Value="0" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <h5>
                        <%-- <asp:Label ID="Label10" runat="server" Text="<%$ resources:FormerActivity %>"></asp:Label>--%>
                    </h5>
                    <div class="gridwrap">
                        <asp:GridView runat="server" ID="grdShiftLines" Width="100%" AutoGenerateColumns="false"
                            OnRowEditing="ActionHandler" EmptyDataRowStyle-CssClass="emptytable">
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ resources:Product %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProduct" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.ProductCode),103) %>' ToolTip ='<%#Eval(Resources.DataFieldRes.ProductCode) %>'></asp:Label>
                                        <asp:Label ID="lblSlNo" runat="server" Text='<%#Eval(Resources.DataFieldRes.SlNo) %>'
                                            Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="74%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:QtyProduced %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyFixed" runat="server" Text='<%#Eval(Resources.DataFieldRes.QtyProduced,"{0:n0}") %>' ToolTip ='<%#Eval(Resources.DataFieldRes.QtyProduced,"{0:n0}") %>' CssClass="medium numeric">
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="11%" HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:GradeA %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lbGradeA" runat="server" Text='<%#Eval(Resources.DataFieldRes.QtyActual,"{0:n0}") %>' ToolTip ='<%#Eval(Resources.DataFieldRes.QtyActual,"{0:n0}") %>' CssClass="medium numeric">
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" HorizontalAlign="Right" />
                                     <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Action %>">
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" EnableViewState="false" ToolTip="<%$ resources:Controls,Edit %>"
                                            OnClick="ActionHandler" CommandName="EDIT_ACTION" />
                                        <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" ToolTip="<%$ resources:Captions, Remove %>"
                                            EnableViewState="false" OnClick="ActionHandler" OnClientClick="return ShowSessionConfirm(this, 1);"
                                            CommandName="DELETE_ACTION" />
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                     <asp:Table runat="server" ID="Table1" CssClass="asptbllinks">
                     <asp:TableRow ID="ModifiedDatePnl" runat="server" CssClass="last-modified" Visible="false">
                          <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                          </asp:TableCell>
                    </asp:TableRow>
                </asp:Table> 
                     
                  
                </div>
                <div id="divListing" runat="server" visible="false">
                    <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:GridView runat="server" ID="grdShiftReportList" Width="100%" ShowFooter="true" TabIndex ="3"
                                    AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="grdShiftReportList_RowDataBound">
                                    <%-- OnRowDataBound="grdShiftReportList_RowDataBound"--%>
                                    <EmptyDataTemplate>
                                       <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:ErpRes,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:ShiftNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShiftNumber" runat="server" Text='<%# Eval(Resources.DataFieldRes.shiftHeaderNo)%>' ToolTip ='<%# Eval(Resources.DataFieldRes.shiftHeaderNo)%>' ></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShiftDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.ShiftDate, Resources.Constants.DateFormatGrid) %>' ToolTip ='<%# Eval(Resources.DataFieldRes.ShiftDate, Resources.Constants.DateFormatGrid) %>' ></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%"  />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Line %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSize" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.LineName)),40) %>' ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.LineName)) %>'  ></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="34%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Shift %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShift" runat="server" Text='<%# Eval(Resources.DataFieldRes.ShiftName) %>' ToolTip ='<%# Eval(Resources.DataFieldRes.ShiftName) %>'></asp:Label>
                                                <asp:Label ID="lblShhPK" runat="server" Text='<%#Eval(Resources.DataFieldRes.ShiftHeaderPK) %>'
                                                    Visible="false"></asp:Label>
                                            </ItemTemplate>                                        
                                            <ItemStyle Width="12%" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:Total %>" ></asp:Label>
                                            </FooterTemplate>
                                             <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:QtyProduced %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProductQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.QtyProduced,"{0:n0}")%>' ToolTip ='<%# Eval(Resources.DataFieldRes.QtyProduced,"{0:n0}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblProductQtyTotal" runat="server">
                                                </asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                            <ItemStyle Width="11%" HorizontalAlign="Right" />
                                           <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Agrade %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblActualQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.QtyActual,"{0:n0}")%>'></asp:Label>
                                            </ItemTemplate>
                                              <FooterTemplate>
                                                <asp:Label ID="lblActualQtyTotal" runat="server">
                                                </asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                            <ItemStyle Width="7%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Action %>">
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" EnableViewState="false" ToolTip="<%$ resources:Controls,Edit  %>"
                                                    OnClick="ActionHandler" CommandName="EDIT_LIST_ACTION" TabIndex ="4" />
                                                <asp:ImageButton Width="16px" Height="16px" runat="server" Visible="false" ID="imbDelete" ToolTip="<%$ resources:Captions, Remove %>"
                                                    SkinID="imbdeletegrid" EnableViewState="false" OnClick="ActionHandler" CommandName="DELETE_LIST_ACTION" />
                                                <asp:HiddenField ID="hdfCreatedDate" runat ="server" Value ="<%# Eval(Resources.DataFieldRes.ShiftCreatedDate)%>" />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </asp:TableCell>
                        </asp:TableRow>
                        
                    </asp:Table>
                </div>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsShiftLine" ValidationGroup="ShiftLine" runat="server" />
                    <asp:ValidationSummary ID="vsHeader" ValidationGroup="Shift" runat="server" />
                    <asp:HiddenField ID="hdfAppType" runat="server" />
                    <asp:HiddenField ID="hdfAppSubType" runat="server" />
                </div>
                <div id="DiverrorMessages" runat="server" style="display: none">
                    <asp:GridView runat="server" ID="grdError" Width="100%" AutoGenerateColumns="false"
                        EmptyDataRowStyle-CssClass="emptytable" EnableTheming="false" ShowHeader="false"
                        BorderWidth="0">
                        <RowStyle HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:ErpRes,Msg_EmptyGrid %>"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <%-- <label class="popup-error">
                                </label>--%>
                                    <asp:Label ID="lblErrorMsgLst" runat="server" Text='<%# Eval(Resources.DataFieldRes.dbRetValTxt) %>' Width ="89%"></asp:Label>
                                    <%--  ((int)Eval(Resources.DataFieldRes.dbRetVal) > 0 ? "<span >" : "<span style='color:Red' >") + Eval(Resources.DataFieldRes.dbRetValTxt).ToString() + "</span>"--%>
                                </ItemTemplate>
                                <ItemStyle Width="90%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                  
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="imdAdd" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
