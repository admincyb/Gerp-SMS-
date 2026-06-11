<%@ Page Title="<%$ Resources:Captions,Title_EmployeeMail %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"  Async="true" Theme="ClassicExt" AutoEventWireup="true" CodeBehind="EmployeeMail.aspx.cs" Inherits="HRMS.ManageMails.EmployeeMail" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">   
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            BindEmployee();           
        }
        function HideFilter() {
            //<summary>Function Used to Hide Vendor Panel </summary>
            $("#imbHideFilter").hide();
            $("#imbShowFilter").show();
            $("#divFilterDetails").hide();
        }

        function ShowFilter() {
            //<summary>Function Used to Show Purchase Request Panel </summary>
            $("#imbHideFilter").show();
            $("#imbShowFilter").hide();
            $("#divFilterDetails").show();
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

        function ShowListing(flag) {
            ///<summary>
            /// Used to handle the Listing And Enrty Section in Page
            ///</summary>
            /// <param name="flag" optional="true" type="String">
            /// flag Determines the Mode if flag then in Listing else in Edit Mode
            /// </param>           
            if (flag) {
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlEntry]").hide();
                $("[id$=Mailer_List]").show();
            }
            else {
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlEntry]").show();
                $("[id$=Mailer_List]").hide();


            }
            return false;
        }  
       
        function ViewMode(mode) {
            ///<summary>
            /// Used to handle the view Mode
            ///</summary>
            /// <param name="mode" optional="true" type="String">
            /// Mode = 1 Determins ites on View Mode
            /// Mode 2 Draft View Mode
            /// Mode 3 List Mode
            /// Mode = 4 Listing

            /// </param>           
            if (mode == 1) {
                //                $("[id$=btnSave]").hide();
                $("[id$=btnSend]").hide();
                $("[id$=btnNew]").hide();
                $("[id$=btnView]").hide();
                $("[id$=btnEdit]").hide();
                $("[id$=imbAddContact]").hide();

            }
            else if (mode == 2) {
                $("[id$=btnNew]").show();
                $("[id$=btnView]").hide();
                $("[id$=btnEdit]").show();
                $("[id$=imbAddContact]").show();
                //                $("[id$=btnDelete]").hide();

            }
            else if (mode == 4) {
                $("[id$=btnNew]").hide();
                $("[id$=btnView]").hide();
                $("[id$=btnEdit]").hide();
                $("[id$=imbAddContact]").show();
                //                $("[id$=btnDelete]").hide();

            }
            else if (mode == 3) {
                $("[id$=btnNew]").show();
                $("[id$=btnView]").show();
                $("[id$=btnEdit]").hide();
                $("[id$=imbAddContact]").show();
                //                $("[id$=btnDelete]").hide();
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
                //CheckValidationDuplicate(valGroup);
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

        function BindEmployee() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee", url + "?Type=0" + "&EmpCategory=2", "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETEBYFILTER", "", true, true, false, 1);
        }
        function CheckAllMailList(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById("<%=grdMailList.ClientID %>");
            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }
      
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upEmployeeMail" runat="server">
        <ContentTemplate>
             <div class="fixed-buttons-normal" id="divFixedTab">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry">                                   
                                                                 
                                </ul>
                                <ul runat="server" id="pnlListing">
                                     <li runat="server" id="pnlSend">
                                        <asp:Button runat="server" ID="btnSend" CommandName="SEND" TabIndex="10" Text="<%$resources:ErpRes,Send %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Send %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-set" OnClientClick="javascript:ValidatePageNow('Save')" />
                                    </li>     
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <%--use the width property of the below table corresponding to the contents in the page--%>
                <asp:Table runat="server" ID="tblTemplate">
                    <%--Rename this ID Page_Entry with the corresponding section Id in the documet--%>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <%--Align table cell according to design--%>
                        <asp:TableCell>
                            <div class="divcol-S">
                                
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <%--Rename this ID Page_List with the corresponding section Id in the documet--%>
                    <asp:TableRow ID="Mailer_List">
                        <asp:TableCell>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Captions", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="16" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="17" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="margin-top: 8px;">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblFromDate" Text="<%$ resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFromDate" CssClass="input-small" TabIndex="3" onkeydown="return CheckKey(event)"
                                                onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" />
                                            <asp:Label runat="server" ID="lblToDate" Text="<%$ resources:ToDate %>" AssociatedControlID="txtToDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtToDate" CssClass="input-small" TabIndex="4" onkeydown="return CheckKey(event)"
                                                onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" />
                                            <div class="clear">
                                            </div>                                          
                                           
                                            <asp:Label ID="lblMailStatus" runat="server" Text="<%$resources:Status %>" AssociatedControlID="ddlStatus"></asp:Label>
                                           <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-a1" TabIndex="3">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="-1"></asp:ListItem>                                                                                               
                                                <asp:ListItem Text="<%$ Resources:Captions,NotGenerated %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Generated %>" Value="1" ></asp:ListItem>
                                            </asp:DropDownList>

                                            <%-- <asp:Label ID="lblSearchButton" runat="server" AssociatedControlID="btnSearch"></asp:Label>--%>
                                            <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$ resources:Controls,Search %>"  OnClick="ActionHandler" TabIndex="7"
                                                CommandName="SEARCH" SkinID="search-ext"  />
                                            <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear %>" TabIndex="8"
                                               ToolTip="<%$ resources:Controls,Clear %>" OnClick="ActionHandler" CommandName="CLEAR"
                                                SkinID="clear-ext" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                           
                                          <asp:Label ID="lblSendTo" runat="server" Text="<%$resources:EmployeeName %>" AssociatedControlID="txtEmployee"></asp:Label>                                          
                                            <asp:TextBox ID="txtEmployee" runat="server" CssClass="input-half" MaxLength="100" TabIndex="5"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfEmployee" runat="server" />
                                            <div class="clear">
                                            </div>                                          
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                               
                                    <%-- use the grid to list the records in the page--%>
                                    <asp:GridView ID="grdMailList" runat="server" AutoGenerateColumns="False" Width="100%"
                                        OnPageIndexChanging="ActionHandler" PageSize="<%$ resources:PageSize%>" AllowSorting="false"
                                        OnSorting="ActionHandler" EmptyDataRowStyle-CssClass="emptytable">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAllMailList" runat="server" onclick="CheckAllMailList(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelectMailList" runat="server" />
                                                 <asp:HiddenField ID="hdfEPM_PK" Value='<%# Eval("EPM_PK") %>'  runat="server"/>
                                                 <asp:HiddenField ID="hdfEPM_EPS_PK" Value='<%# Eval("EPM_EPS_PK") %>'  runat="server"/>
                                                 <asp:HiddenField ID="hdfEPM_FROM_EMAIL" Value='<%# Eval("EPM_FROM_EMAIL") %>'  runat="server"/>
                                                 <asp:HiddenField ID="hdfEPM_FILE_NAME" Value='<%# Eval("EPM_FILE_NAME") %>'  runat="server"/>
                                            </ItemTemplate>
                                            <ItemStyle Width="1.5%" />
                                        </asp:TemplateField>                                                                         
                                           <asp:TemplateField HeaderText="<%$ resources:Date %>" SortExpression="EPM_DATE">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDate" runat="server" Text='<%# Eval("EPM_DATE", Resources.Constants.DateFormatGrid) %>'
                                                        ToolTip='<%# Eval("EPM_DATE", Resources.Constants.DateFormatGrid) %>'>                                                                
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="6.5%" Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:EmployeeName %>" SortExpression="empName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("empName") %>'
                                                        ToolTip='<%# Eval("empName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="18%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Mail %>" SortExpression="EPM_EMAIL">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("EPM_EMAIL") %>'
                                                        ToolTip='<%# Eval("EPM_EMAIL") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Subject %>" SortExpression="EPM_SUBJECT">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSubject" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EPM_SUBJECT")), 35)%>' ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EPM_SUBJECT")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="<%$ resources:Content %>" SortExpression="EPM_CONTENT">
                                                <ItemTemplate>                                                
                                                    <asp:Label ID="lblContent" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EPM_CONTENT")), 58)%>' ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EPM_CONTENT")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="34%" />
                                            </asp:TemplateField>                                                                                                                                     
                                            <asp:TemplateField HeaderText="<%$ resources:Status %>" SortExpression="EPM_IS_GENERATED">
                                                <ItemTemplate>
                                                <asp:HiddenField ID="hdfIsGenerated" runat="server"  Value='<%# Eval("EPM_IS_GENERATED") %>'/>
                                                    <asp:Label ID="lblMailStatus" runat="server" Text='<%# Convert.ToString(Eval("EPM_IS_GENERATED")).Trim() == "1" ? GetLocalResourceObject("Generated") : GetLocalResourceObject("NotGenerated")%>' ></asp:Label>                                                   
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="false" />
                                                <ItemStyle Width="6%" Wrap="false" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <uc1:PagerControl ID="uclPaging" runat="server" />
                                
                                <%-- Leave this table as such --%>
                              
                            </div>
                              <div class="clear">
                                </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" runat="server" CssClass="last-modified" Visible="true">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
            </div>                   
        </ContentTemplate>        
    </asp:UpdatePanel>
</asp:Content>
