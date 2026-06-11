<%@ Page Title="<%$ Resources:Captions,Title_PurchaseRequestTrading %>" Language="C#" Theme="ClassicExt" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="PurchaseRequestTradingList.aspx.cs" Inherits="ERPSMS_v01.PurchaseRequestManagement.PurchaseRequestTradingList" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        //var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            var prAutoCompleteURL = "PurchaseRequest.do?Action=GetSearchValueTradingList&AUTOSEARCH=1";
            var prPageURL = "/PurchaseRequestManagement/PurchaseRequestTrading.aspx";
            GrandScriptUtils.MakeAutoCompleteDDL("txtPRNumber", prAutoCompleteURL + "&SearchType=PRH_NO" + "&PageURL=" + prPageURL + "&ProcessPK=" + $("[id$=hdfProcId]").val(), "hdfPRNumber", true, true);
            GrandScriptUtils.MakeAutoCompleteDDL("txtReqStore", prAutoCompleteURL + "&SearchType=DPT_NAME" + "&PageURL=" + prPageURL + "&ProcessPK=" + $("[id$=hdfProcId]").val(), "hdfReqStore", true, true);
            GrandScriptUtils.MakeAutoCompleteDDL("txtIONo", prAutoCompleteURL + "&SearchType=SOH_NO" + "&PageURL=" + prPageURL + "&ProcessPK=" + $("[id$=hdfProcId]").val(), "hdfIONo", true, true);
            GrandScriptUtils.MakeAutoCompleteDDL("txtReqDept", prAutoCompleteURL + "&SearchType=CON_NAME" + "&PageURL=" + prPageURL + "&ProcessPK=" + $("[id$=hdfProcId]").val(), "hdfReqDept", true, true);
            GrandScriptUtils.MakeAutoCompleteDDL("txtReqBy", prAutoCompleteURL + "&SearchType=PRH_USER" + "&PageURL=" + prPageURL + "&ProcessPK=" + $("[id$=hdfProcId]").val(), "hdfReqBy", true, true);

            //To set visibility of Hierarchical grid expand button
            ShowHideExpand();
            //****************Multiple Plant**************************************************        
            var isMultiplePlant = $("[id$=hdfIsMultiplePlant]").val();
            if (parseInt(isMultiplePlant) == 1) {
                $("[id$=lblPlantCode]").show();
                $("[id$=ddlPlantCode]").show();
            }
            else {
                $("[id$=lblPlantCode]").hide();
                $("[id$=ddlPlantCode]").hide();
            }
            //***************************************************************************************
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

        function ShowHideExpand() {
            ///<summary>
            /// Used to Show/Hide Grid Expad Button
            ///</summary>

            $("[id*=hdfHasChildren]").each(function () {
                $(this).parent().parent().find('a.GridExpandCollapseButton').css("visibility", ($(this).val() == "1" ? "visible" : "hidden"));
            });
        }

        function AfterGridExpand(row) {

            if ($("[id$=grdPRList]").attr('id') == $(row).parent().parent().attr('id')) {
                var hdf = $(row).find("[id*=hdfIsExpandedOrders]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnOrderDetails]").click();
                }
            }
            else {
                var gridType = 0;
                var names = $(row).parent().parent().attr('id').split('_');

                if (names.length == 1) {
                    gridType = names[0] == "grdGRNList" ? 1 : names[0] == "grdGINList" ? 2 : 0;
                }
                else if (names.length > 1) {
                    gridType = (names[names.length - 1] == "grdPOList" || names[names.length - 2] == "grdPOList") ? 1
                    : (names[names.length - 1] == "grdGRNList" || names[names.length - 2] == "grdGRNList") ? 2
                    : (names[names.length - 1] == "grdGINList" || names[names.length - 2] == "grdGINList") ? 3 : 0;
                }
                if (gridType == 1) {
                    var hdf = $(row).find("[id*=hdfIsExpandedPO]");
                    if (hdf.val() == "0") {
                        $(row).find("input[id*=btnGetGRN]").click();
                    }
                }
                if (gridType == 2) {
                    var hdf = $(row).find("[id*=hdfIsExpandedGRNList]");
                    if (hdf.val() == "0") {
                        $(row).find("input[id*=btnGetGIN]").click();
                    }
                }
                if (gridType == 3) {
                    var hdf = $(row).find("[id*=hdfIsExpandedGinList]");
                    if (hdf.val() == "0") {
                        $(row).find("input[id*=btnGetStockTransfer]").click();
                    }
                }
            }

            if ($("[id$=grdPRItems]").attr('id') == $(row).parent().parent().attr('id')) {
                var hdf = $(row).find("[id*=hdfIsExpandedPRItem]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnGetPO]").click();
                }
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
        function ItemListSelection() {
            var selectedRowColor;
            selectedRowColor = '<%= Resources.ErpRes.selectedRowColor %>';
            $("#[id*=grdPRList] input[type=hidden][id*=hdfPRHPK]").each(function (index) {
                if ($(this).val() == $("[id$=hdfSelectedItemPRPK]").val()) {
                    $(this).closest('tr').css('background-color', selectedRowColor);
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlAvtivity" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                 <ul runat="server" id="pnlListing">
                                   <li>
                                     <asp:Button ID="btnAdd" runat="server" SkinID="btnInner-New" Text="<%$Resources:Controls,Add%>"
                                     CommandName="NEW" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Add %>"
                                      TabIndex="11" />
                                   </li>
                                    <%--<li>
                                        <asp:Button ID="btnReset" runat="server" SkinID="btnInner-refresh" ToolTip="<%$resources:Controls,Refresh %>"
                                            Text="<%$Resources:Controls,Refresh%>" OnClientClick="javascript:return ResetPage();"
                                            TabIndex="12" />
                                    </li>--%>
                                  </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>               
            </div>
            <div class="content-wrapper">
                <%--use the width property of the below table corresponding to the contents in the page--%>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <%--Rename this ID Page_Entry with the corresponding section Id in the documet--%>
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <%--Align table cell according to design--%>
                        <asp:TableCell>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Captions", "AdvanceSearch").ToString() %></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.jpg" ToolTip="Show Filter"
                                                TabIndex="65" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="66" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn----------%>
                             <div class="clear">
                             </div>
                              <table class="table-devide" id="tbladvancedSearch" style="/*margin-top: 8px; */ background: #f2f2f2;">
            <tr>
                <td>
                    <div class="div2col-S padgtop7">
                        <div class="clear">
                        </div>
                        <asp:Label runat="server" ID="lblFromDate" Text="<%$ resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                        <asp:TextBox runat="server" ID="txtFromDate" CssClass="input-small" TabIndex="3"
                            onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                        <asp:HiddenField ID="hdfFromDate" runat="server" />
                        <asp:Label runat="server" ID="lblToDate" Text="<%$ resources:ToDate %>" AssociatedControlID="txtToDate"
                            CssClass="middle-lbl-small-d"></asp:Label>
                        <asp:TextBox runat="server" ID="txtToDate" CssClass="input-small" TabIndex="4" onkeydown="return CheckKey(event)"
                            onpaste="return false;"></asp:TextBox>                        
                        <div class="clear">
                        </div>
                        <asp:Label runat="server" ID="lblItemName" Text="<%$ Resources:Controls, ItemName%>"
                            AssociatedControlID="txtItemname"></asp:Label>
                        <asp:TextBox runat="server" ID="txtItemname" CssClass="input-half" TabIndex="6"></asp:TextBox>
                        
                    </div>
                </td>
                <td>
                    <div class="div2col-S padgtop7">
                        <asp:Label runat="server" ID="lblReqStore" Text="<%$ resources:ReqStore %>" AssociatedControlID="txtReqStore"></asp:Label>
                        <asp:TextBox ID="txtReqStore" runat="server" CssClass="select-small-e" TabIndex="5" >
                        </asp:TextBox>
                        <asp:HiddenField ID="hdfReqStore" runat="server" />
                        <asp:Label runat="server" ID="lblIONo" Text="<%$ resources:IoNo %>" CssClass="lbl-13perc"
                            AssociatedControlID="txtIONo"></asp:Label>
                        <asp:TextBox runat="server" ID="txtIONo" TabIndex="5" CssClass="input-small-c0"></asp:TextBox>
                        <asp:HiddenField ID="hdfIONo" runat="server" />
                        <div class="clear">
                        </div>
                        <asp:Label runat="server" ID="lblReqDept" Text="<%$ resources:ReqDept %>" AssociatedControlID="txtReqDept"></asp:Label>
                        <asp:TextBox runat="server" ID="txtReqDept" TabIndex="7" CssClass="select-small-e"></asp:TextBox>
                        <asp:HiddenField ID="hdfReqDept" runat="server" />
                        <asp:Label runat="server" ID="lblReqBy" Text="<%$ resources:ReqBy %>" AssociatedControlID="txtReqBy"
                            CssClass="lbl-13perc"></asp:Label>
                        <asp:TextBox runat="server" ID="txtReqBy" TabIndex="7" CssClass="input-small-c0"></asp:TextBox>
                        <asp:HiddenField ID="hdfReqBy" runat="server" />
                        <div class="clear">
                        </div>                        
                    </div>
                </td>
            </tr>
        </table>
                              <table class="table-devide">
            <tr>
                <td>
                    <div class="div2col-S div-separatn">
                     <asp:Label runat="server" ID="lblTrnStatus" Text="<%$ resources:TransactionStatus %>" 
                            AssociatedControlID="ddlTrnStatus"></asp:Label>
                        <asp:DropDownList ID="ddlTrnStatus" runat="server" CssClass="input-small-c margnbotm0 margn-rgt2"
                            TabIndex="8" >
                        </asp:DropDownList>           
                    <asp:Label runat="server" ID="lblPlantCode" Text="<%$ Resources:Controls,CompanyPlant%>"
                            AssociatedControlID="ddlPlantCode"  CssClass="lbl-15-1perc"></asp:Label>
                        <asp:DropDownList ID="ddlPlantCode" runat="server" CssClass="select-small-b margnbotm0 margn-rgt2"
                            TabIndex="8">
                        </asp:DropDownList>                                    
                    </div>
                </td>
                <td>
                    <div class="div2col-S div-separatn">
                        <asp:Label runat="server" ID="lblPrNumber" Text="<%$ resources:PrNo %>" AssociatedControlID="txtPRNumber"></asp:Label>
                        <asp:TextBox runat="server" ID="txtPRNumber" TabIndex="9" CssClass="select-small-e margnbotm0 margn-rgt2 h14">
                        </asp:TextBox>
                        <asp:HiddenField ID="hdfPRNumber" runat="server" Value="0" />
                        <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:PrStatus %>" AssociatedControlID="ddlStatus"
                            CssClass="lbl-13perc"></asp:Label>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-b margnbotm0 margn-rgt2"
                            TabIndex="9" >
                            <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterNotClosed %>" Text="<%$ Resources:BindValues, StatusFilterNotClosed%>"
                                Selected="True">
                            </asp:ListItem>
                            <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterAll %>" Text="<%$ Resources:BindValues, StatusFilterAll%>">
                            </asp:ListItem>
                            <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterClosed %>" Text="<%$ Resources:BindValues, StatusFilterClosed%>">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lblMiddle" runat="server" CssClass="middle-lbl-xsmall-d style-none margnbotm0"></asp:Label>
                        <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                            TabIndex="9" SkinID="search-ext" Style="margin-bottom: 0px!important; margin-top: 2px;"
                           OnClick="ActionHandler"  CommandName="SEARCH" />
                        <asp:ImageButton ID="btnClear" runat="server" TabIndex="9" Style="margin-bottom: 0px!important;
                            margin-top: 2px;" ToolTip="<%$ resources:Controls,Clear %>" SkinID="clear-ext"
                             OnClick="ActionHandler" CommandName="CLEAR"/>
                    </div>
                </td>
            </tr>
        </table>                           
                            <div class="clear">
                            </div>
                            <%--  <div class="clear">
                            </div>--%>
                            <div class="gridwrap hierarchical-wrap">
                                 <asp:GridView runat="server" ID="grdPRList" AutoGenerateColumns="False"  EmptyDataRowStyle-CssClass="emptytable"   Width="100%" 
                                                OnRowDataBound="ActionHandler" PageSize="<%$ resources:PageSize %>"  OnRowCommand="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>                                                
                                                <asp:ImageButton runat="server" ID="imbPRList"  alt="" CssClass="itemlist"  ToolTip="<%$Resources:ItemDetails %>" OnClick="ActionHandler" CommandName="PRITEMDETAILS" />                                               
                                                <asp:HiddenField runat="server" ID="hdfIsExpandedOrders" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdfPRHPK" Value='<%# Eval("PRH_PK") %>' />  
                                                <asp:HiddenField runat="server" ID="hdfUserStatus" Value='<%# Eval("USER_STATUS") %>' /> 
                                                <asp:HiddenField runat="server" ID="hdfPrhStatus" Value='<%# Eval("PRH_STATUS") %>' /> 
                                                <asp:HiddenField runat="server" ID="hdfPrhDept" Value='<%# Eval("PRH_DEPT") %>' /> 
                                                <asp:HiddenField runat="server" ID="hdfPrhDelStatus" Value='<%# Eval("PRH_DEL_STATUS") %>' /> 
                                                <asp:HiddenField runat="server" ID="hdfPohPoFlag" Value="0" /> 
                                                <asp:HiddenField runat="server" ID="hdfPohPoWkfFlag" Value="0" /> 
                                                <asp:HiddenField runat="server" ID="hdfRefId" Value='<%# Eval("REF_ID") %>' /> 
                                                <asp:HiddenField runat="server" ID="hdfPrhItemFullText" Value='<%# Eval("PRH_ITEM_FULL_TEXT") %>' />  
                                                <asp:HiddenField runat="server" ID="hdfprhLinkStatus" Value='<%# Eval("PRH_LINK_STATUS") %>' />                                                
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>                                        
                                        <asp:TemplateField HeaderText="<%$ resources:ReqDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPoDate" runat="server" Text='<%# Eval("PRH_DATE") %>'
                                                    ToolTip='<%# Eval("PRH_DATE", Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:RequestNo %>">
                                            <ItemTemplate>                                                 
                                                    <asp:Label ID="lblPRNo"  runat="server" Text='<%# Eval("PRH_NO") %>' ToolTip='<%# Eval("PRH_NO") %>'></asp:Label>                                              
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCmpny"  CssClass="<%# Eval(Resources.DataFieldRes.CompnayLineColor) %>" runat="server" Text='<%# Eval("CMP_DISPLAY_CODE") %>'
                                                    ToolTip='<%# Eval("CMP_DISPLAY_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                            <HeaderStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:RequestingStore %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptName" runat="server" Text='<%# Eval("DPT_NAME") %>' ToolTip='<%# Eval("DPT_NAME") %>'></asp:Label>                                                
                                            </ItemTemplate>
                                            <ItemStyle Width="14%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ItemDetails %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItem" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("PRH_ITEM_TEXT"),80) %>'
                                                    ToolTip='<%# Eval("PRH_ITEM_FULL_TEXT") %>'></asp:Label>                                                
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:IONumber %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIONumber" runat="server" Text='<%# Eval("SOH_NO") %>'
                                                    ToolTip='<%# Eval("SOH_NO") %>'></asp:Label>                                                
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:RequestedDept %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReqDept" runat="server" Text='<%# Eval("PRH_ISSUE_DEPT_TEXT") %>' ToolTip='<%# Eval("PRH_ISSUE_DEPT_TEXT") %>'></asp:Label>                                                
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>  
                                        <asp:TemplateField HeaderText="<%$ resources:RequestedBy %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRequestedBy" runat="server" Text='<%# Eval("PRH_USER") %>'  ToolTip='<%# Eval("PRH_USER") %>' ></asp:Label>                                                
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>  
                                        <asp:TemplateField HeaderText="<%$ resources:Status %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("PRH_STATUS_TEXT") %>'  ToolTip='<%# Eval("PRH_STATUS_TEXT") %>'></asp:Label>                                                
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgbtnPRHierarchyLevel" runat="server" OnClientClick="javascript:return false;" TabIndex="10" />
                                               <asp:HiddenField runat="server" ID="hdfPohGrnWkfFlag" Value="0" />                                                
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                         <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$resources:ErpRes,Edit %>" TabIndex="10"
                                            SkinID="imbeditgrid" CommandName="PERFORMACTION" />
                                        <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$resources:ErpRes,Delete %>" TabIndex="10"
                                            SkinID="imbdeletegrid" CommandName="DELETEPR" OnClientClick="return ShowDeleteConfirm(this);" />
                                        <asp:ImageButton runat="server" ID="imbView" ToolTip="<%$resources:ErpRes,View %>" TabIndex="10"
                                            SkinID="btnview" CommandName="VIEW" />
                                        <asp:ImageButton runat="server" ID="imbPrint" ToolTip="<%$resources:ErpRes,Print %>" TabIndex="10"
                                            SkinID="btnPrint" CommandName="PRINT" />
                                        <asp:ImageButton runat="server" ID="imbPRShorClose" CommandName="SHORTCLOSEPR" TabIndex="10"
                                            SkinID="btnclose" alt="<%$Resources:Controls,ShortClose%>" title="<%$Resources:Controls,ShortClose%>" />
                                        <asp:ImageButton runat="server" ID="imbPRCancel" CommandName="CANCELPR" OnClientClick="return ShowDeleteConfirm(this,'Are you sure want to cancel this PR.?');"
                                           TabIndex="10" SkinID="cancel" alt="<%$Resources:Controls,Cancel%>" title="<%$Resources:Controls,Cancel%>" />
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>                                        
                                    </Columns>
                                 </asp:GridView> 
                                <uc1:PagerControl ID="uclPaging" runat="server" Visible="false" />
                            </div>
                            <h4>
                                Item Details</h4>
                            <%--Secnd Division--%>
                            <div class="gridwrap hierarchical-wrap">
                                <cc1:ExtGridView runat="server" ID="grdPRItems" AutoGenerateColumns="False" OnRowDataBound="ActionHandler"
                                    ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                    GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                    ShowFooter="true" PageSize="<%$ resources:PageSize %>">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Item %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItem" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("ITM_NAME").ToString()),35) %>'
                                                    ToolTip='<%# Eval("ITM_NAME") %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfIsExpandedPRItem" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdfPRItem" Value='<%# Eval("ITM_PK") %>' />
                                                <asp:Button runat="server" ID="btnGetPO" OnClick="ActionHandler" CommandName="PODETAILS"
                                                   CommandArgument='<%# Eval(Resources.DataFieldRes.PRDetailPK) %>' EnableTheming="false"
                                                    Style="display: none" />
                                            </ItemTemplate>
                                            <ItemStyle Width="35%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ItemCat %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemCat" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("ITM_CATEGORY_TEXT").ToString()),35) %>'
                                                    ToolTip='<%# Eval("ITM_CATEGORY_TEXT") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PRQty %>">
                                            <ItemTemplate>
                                                 <asp:Label ID="lblPRQty" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("PRD_QTY_REQUESTED")) +" "+Eval("PRD_UOM_TEXT") %>' ToolTip='<%#GetFormattedNumberWithSeperation(Eval("PRD_QTY_REQUESTED")) +" "+Eval("PRD_UOM_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="rate-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:POQty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPOQty" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("PRD_QTY_ORDERED")) +" "+Eval("PRD_UOM_TEXT") %>' ToolTip='<%#GetFormattedNumberWithSeperation(Eval("PRD_QTY_ORDERED")) +" "+Eval("PRD_UOM_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="rate-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PORcvdQty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPORcvdQty" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("POD_QTY_RECEIVED")) +" "+Eval("PRD_UOM_TEXT") %>' ToolTip='<%#GetFormattedNumberWithSeperation(Eval("POD_QTY_RECEIVED")) +" "+Eval("PRD_UOM_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="rate-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:POAccQty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPOAccQty" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("POD_QTY_ACCEPTED")) +" "+Eval("PRD_UOM_TEXT") %>' ToolTip='<%#GetFormattedNumberWithSeperation(Eval("POD_QTY_ACCEPTED")) +" "+Eval("PRD_UOM_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="rate-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:POInvQty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPOInvQty" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("POD_QTY_INVOICED")) +" "+Eval("PRD_UOM_TEXT") %>' ToolTip='<%#GetFormattedNumberWithSeperation(Eval("POD_QTY_INVOICED")) +" "+Eval("PRD_UOM_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="rate-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div class="hierarchical-gridwrap">
                                                    <cc1:ExtGridView runat="server" ID="grdPOList" AutoGenerateColumns="False" OnRowDataBound="ActionHandler"
                                                        ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                                        GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                                        ShowFooter="true" PageSize="<%$ resources:PageSize %>">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="lblEmptyGridPOList" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ resources:PONo %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPONo" runat="server" Text='<%# Eval("POH_NO") %>'
                                                                        ToolTip='<%# Eval("POH_NO") %>'>
                                                                    </asp:Label>
                                                                    <asp:HiddenField runat="server" ID="hdfIsExpandedPO" Value="0" />
                                                                    <asp:HiddenField runat="server" ID="hdfPOPk" Value='<%# Eval("POH_PK") %>' />
                                                                    <asp:Button runat="server" ID="btnGetGRN" OnClick="ActionHandler" CommandName="GRNDETAILS"
                                                                        CommandArgument='<%# Eval("POD_PK") %>' EnableTheming="false"  Style="display: none" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="21%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPODate" runat="server" Text='<%# Eval("POH_DATE", Resources.Constants.DateFormatGrid) %>'
                                                                        ToolTip='<%# Eval("POH_DATE", Resources.Constants.DateFormatGrid) %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="8%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Vendor %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVendor" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("POH_VENDOR_TEXT"),40) %>'
                                                                        ToolTip='<%# Eval("POH_VENDOR_TEXT") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="36%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Type %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPOType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("POH_TYPE_TEXT"),25) %>'
                                                                        ToolTip='<%# Eval("POH_TYPE_TEXT") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="8%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Store %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPOStore" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("POH_DEPT_TEXT"),25) %>'
                                                                        ToolTip='<%# Eval("POH_DEPT_TEXT") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="13%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Currency %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval("POH_CURRENCY_TEXT")  %>'
                                                                    ToolTip='<%# Eval("POH_CURRENCY_TEXT")  %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="5%" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:POAmount%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPOAmt" runat="server" Text='<%# Eval("POH_TOTAL_VALUE", "{0:c}") %>'
                                                                          ToolTip='<%# Eval("POH_TOTAL_VALUE", "{0:c}") %>' ></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="9%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="rate-numeric" />
                                                            </asp:TemplateField>                                                            
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <div class="hierarchical-gridwrap">
                                                                        <cc1:ExtGridView runat="server" ID="grdGRNList" AutoGenerateColumns="False" ExpandButtonCssClass="GridExpandCollapseButton"
                                                                            CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                                                                            CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" CellPadding="3"
                                                                            ForeColor="#333333" AllowPaging="false">
                                                                            <EmptyDataTemplate>
                                                                                <asp:Label ID="lblGRNList" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                                            </EmptyDataTemplate>
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="<%$ resources:GRNNo %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGRnNo" runat="server" Text='<%# Eval("GRH_NO") %>' ToolTip='<%# Eval("GRH_NO") %>'>
                                                                                        </asp:Label>
                                                                                        <asp:HiddenField runat="server" ID="hdfIsExpandedGRNList" Value="0" />
                                                                                        <asp:HiddenField runat="server" ID="hdfGRNPk" Value='<%# Eval("GRH_PK") %>' />
                                                                                        <asp:Button runat="server" ID="btnGetGIN" OnClick="ActionHandler" CommandName="GINDETAILS"
                                                                                            EnableTheming="false" Style="display: none" CommandArgument='<%# Eval("GRD_PK") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Width="22%" />
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGRNDate" runat="server" Text='<%# Eval("GRH_DATE", Resources.Constants.DateFormatGrid) %>'
                                                                                            ToolTip='<%# Eval("GRH_DATE", Resources.Constants.DateFormatGrid) %>'>
                                                                                        </asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Width="8%" />
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ resources:GrnStore %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGRNStore" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("GRH_DEPT_TEXT"),23) %>'
                                                                                            ToolTip='<%# Eval("GRH_DEPT_TEXT") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Width="30%" />
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ resources:GRNRcvdQty %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGRNRecievedQty" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("GRD_QTY_RECEIVED")) +" "+Eval("GRD_UOM_TEXT") %>'
                                                                                            ToolTip='<%# GetFormattedNumberWithSeperation(Eval("GRD_QTY_RECEIVED")) +" "+Eval("GRD_UOM_TEXT") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                                                    <HeaderStyle CssClass="rate-numeric" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ resources:GRNAccQty %>">
                                                                                    <ItemTemplate>
                                                                                          <asp:Label ID="lblGRNQtyAccepted" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("GRD_QTY_ACCEPTED")) +" "+Eval("GRD_UOM_TEXT") %>'
                                                                                            ToolTip='<%# GetFormattedNumberWithSeperation(Eval("GRD_QTY_ACCEPTED")) +" "+Eval("GRD_UOM_TEXT") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                                                    <HeaderStyle CssClass="rate-numeric" />
                                                                                </asp:TemplateField>                                                                                
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <div class="hierarchical-gridwrap">
                                                                                          <cc1:ExtGridView runat="server" ID="grdGINList" AutoGenerateColumns="False" ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                                                                                  GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" CellPadding="3" ForeColor="#333333" AllowPaging="false">
                                                                                              <EmptyDataTemplate>
                                                                                               <asp:Label ID="lblGINList" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                                                              </EmptyDataTemplate>
                                                                                              <Columns>
                                                                                                    <asp:TemplateField HeaderText="<%$ resources:GinNo %>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblGinNo" runat="server" Text='<%# Eval("GIH_NO") %>'
                                                                                                                ToolTip='<%# Eval("GIH_NO") %>'>
                                                                                                            </asp:Label>
                                                                                                            <asp:HiddenField runat="server" ID="hdfIsExpandedGinList" Value="0" />
                                                                                                            <asp:HiddenField runat="server" ID="hdfGinPk" Value='<%# Eval("GIH_PK") %>' />
                                                                                                            <asp:Button runat="server" ID="btnGetStockTransfer" OnClick="ActionHandler" CommandName="STDETAILS"
                                                                                                                EnableTheming="false" Style="display: none" CommandArgument='<%# Eval("GID_PK") %>' />
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle Width="22%" />
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblGinDate" runat="server" Text='<%# Eval("GIH_DATE", Resources.Constants.DateFormatGrid) %>'
                                                                                                                ToolTip='<%# Eval("GIH_DATE", Resources.Constants.DateFormatGrid) %>'>
                                                                                                            </asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle Width="8%" />
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="<%$ resources:GinStore %>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblGinStore" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("GIH_DEPT_TEXT"),23) %>'
                                                                                                                ToolTip='<%# Eval("GIH_DEPT_TEXT") %>'></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle Width="25%" />
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="<%$ resources:GinQtyInspected %>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblGinQtyInspected" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("GID_QTY_INSPECTED")) +" "+Eval("GID_UOM_TEXT") %>'
                                                                                                                ToolTip='<%# GetFormattedNumberWithSeperation(Eval("GID_QTY_INSPECTED")) +" "+Eval("GID_UOM_TEXT") %>'></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                                                        <HeaderStyle CssClass="rate-numeric" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="<%$ resources:GinQtyRejected %>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblGinQtyRejected" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("GID_QTY_REJECTED")) +" "+Eval("GID_UOM_TEXT") %>'
                                                                                                                ToolTip='<%# GetFormattedNumberWithSeperation(Eval("GID_QTY_REJECTED")) +" "+Eval("GID_UOM_TEXT") %>'></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                                                        <HeaderStyle CssClass="rate-numeric" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="<%$ resources:GinQtyAccepted %>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblGinQtyAccepted" runat="server" Text='<%# GetFormattedNumberWithSeperation(Eval("GID_QTY_ACCEPTED"))  +" "+Eval("GID_UOM_TEXT") %>'
                                                                                                                ToolTip='<%# GetFormattedNumberWithSeperation(Eval("GID_QTY_ACCEPTED")) +" "+Eval("GID_UOM_TEXT") %>'></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                                                        <HeaderStyle CssClass="rate-numeric" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField>
                                                                                                        <ItemTemplate>
                                                                                                            <div class="hierarchical-gridwrap">
                                                                                                                <cc1:ExtGridView runat="server" ID="grdStockTransfer" AutoGenerateColumns="False"
                                                                                                                    ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                                                                                                    GridLines="None" ExpandButtonText="+" DataKeyNames="<%$ resources:DataFieldRes,StHdrPK %>"
                                                                                                                    CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" AllowPaging="false">
                                                                                                                    <EmptyDataTemplate>
                                                                                                                        <asp:Label ID="lblSTEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                                                                                    </EmptyDataTemplate>
                                                                                                                    <Columns>
                                                                                                                        <asp:TemplateField HeaderText="<%$ resources:STNumber %>">
                                                                                                                            <ItemTemplate>
                                                                                                                                <asp:Label ID="lblStNumber" runat="server" Text='<%# Eval("SFH_NO") %>'
                                                                                                                                    ToolTip='<%# Eval("SFH_NO") %>'>
                                                                                                                                </asp:Label>
                                                                                                                                <asp:HiddenField runat="server" ID="hdfIsExpandedItem" Value="0" />
                                                                                                                                <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />
                                                                                                                                <asp:HiddenField ID="hdfStockTransferPK" runat="server" Value='<%# Eval("SFH_PK") %>' />
                                                                                                                            </ItemTemplate>
                                                                                                                            <ItemStyle Width="22%" HorizontalAlign="Left" />
                                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                                        </asp:TemplateField>
                                                                                                                        <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                                                                                            <ItemTemplate>
                                                                                                                                <asp:Label ID="lblStDate" runat="server" Text='<%# Eval("SFH_DATE", Resources.Constants.DateFormatGrid) %>'
                                                                                                                                    ToolTip='<%# Eval("SFH_DATE", Resources.Constants.DateFormatGrid) %>'>
                                                                                                                                </asp:Label>
                                                                                                                            </ItemTemplate>
                                                                                                                            <ItemStyle Width="8%" />
                                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                                        </asp:TemplateField>
                                                                                                                        <asp:TemplateField HeaderText="<%$ resources:QuantityUom %>">
                                                                                                                            <ItemTemplate>
                                                                                                                                <asp:Label ID="lblStQuantityUom" runat="server" Text='<%# GetFormattedNumberWithSeperation(Eval("SFD_QTY_APPROVED"))  +" "+Eval("SFD_UOM_TEXT") %>' ToolTip='<%# GetFormattedNumberWithSeperation(Eval("SFD_QTY_APPROVED"))  +" "+Eval("SFD_UOM_TEXT") %>'></asp:Label>
                                                                                                                            </ItemTemplate>
                                                                                                                            <ItemStyle HorizontalAlign="Right" Width="70%" />
                                                                                                                            <HeaderStyle CssClass="rate-numeric" />
                                                                                                                        </asp:TemplateField>
                                                                                                                        <asp:TemplateField>
                                                                                                                            <ItemTemplate>
                                                                                                                                <asp:Label ID="Label1" Text="" runat="server"></asp:Label>
                                                                                                                            </ItemTemplate>
                                                                                                                        </asp:TemplateField>
                                                                                                                    </Columns>
                                                                                                                    <RowStyle CssClass="table-thirdlevel" />
                                                                                                                    <HeaderStyle CssClass="table-thirdlevela" />
                                                                                                                </cc1:ExtGridView>
                                                                                                            </div>
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle CssClass="nopadding" />
                                                                                                    </asp:TemplateField>
                                                                                                 </Columns>
                                                                            <%--Second--%>
                                                                            <RowStyle CssClass="table-thirdlevel" />
                                                                            <HeaderStyle CssClass="table-thirdlevela" />
                                                                        </cc1:ExtGridView>
                                                                                        </div>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="nopadding" />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <%--Second--%>
                                                                            <RowStyle CssClass="table-thirdlevel" />
                                                                            <HeaderStyle CssClass="table-thirdlevela" />
                                                                        </cc1:ExtGridView>
                                                                    </div>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="nopadding" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <%--First--%>
                                                        <RowStyle CssClass="table-secondlevel" />
                                                        <HeaderStyle CssClass="table-secondlevela" />
                                                    </cc1:ExtGridView>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="nopadding" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="table-firstlevel" />
                                    <HeaderStyle CssClass="table-firstlevela" />
                                </cc1:ExtGridView>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>                    
                </div>
                <asp:HiddenField ID="hdfIsMultiplePO" runat="server" />
                <asp:HiddenField ID="hdfSelectedItemPRPK" runat="server" Value="0" />
                <asp:HiddenField ID="hdfServicePORequired" runat="server" Value="1" />
                <asp:HiddenField ID="hdfDeptID" runat="server" Value="0" />
                <asp:HiddenField ID="hdfAppType" runat="server" />
                <asp:HiddenField ID="hdfAppSubType" runat="server" />
                <asp:HiddenField ID="hdnClosePO" runat="server" Value="0" />
                <asp:HiddenField ID="hdnModifyPR" runat="server" Value="0" />
                <asp:HiddenField ID="hdnCancelPR" runat="server" Value="0" />
                <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
                <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
                <asp:HiddenField ID="hdfShowTransactionTypeFilter" runat="server" Value="0" />
                  <asp:HiddenField ID="hdfDecimalFormatWithSeperation" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
