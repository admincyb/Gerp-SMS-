<%@ Page Title="<%$ Resources:Captions,Title_StockTransfer %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" EnableEventValidation="false"
    Theme="ClassicExt" AutoEventWireup="true" CodeBehind="StockTransferList.aspx.cs"
    Inherits="ERPSMS_v01.StoreManagement.StockTransferList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/StockTransferList.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="fixed-buttons-normal">
        <div class="Button-container">
            <asp:Table runat="server" ID="tblButton">
                <asp:TableRow>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul class="bredcrum">
                            <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                        </ul>
                        <ul runat="server" id="pnlListing">
                            <li>
                                <asp:Button ID="btnAdd" runat="server" SkinID="btnInner-New" Text="<%$Resources:Controls,Add%>"
                                    ToolTip="<%$resources:Controls,Add %>" OnClientClick="javascript:return AddNew();"
                                    TabIndex="10" EnableViewState="false" />
                            </li>
                            <li>
                                <asp:Button ID="btnReset" runat="server" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Refresh%>"
                                    ToolTip="<%$resources:Controls,Refresh %>" OnClientClick="javascript:return ResetPage();"
                                    TabIndex="10" />
                            </li>                            
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div class="clear">
        </div>
    </div>
    <div id="Wofkflowdiv">
        <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
    </div>
    <div class="content-wrapper">
    <%--******************Old Search region*******************===========--%>
        <div style="display: none">
        <div id="searchwrap" class="search-wrap-custom1">
            <div id="divSearch">
                <label for="SearchType">
                    <%=Resources.Controls.SearchBy%></label>
                <asp:DropDownList ID="SearchType" runat="server" CssClass="medium" TabIndex="1" onchange="javascript:SetSearchType();"
                    EnableViewState="false">
                    <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                    </asp:ListItem>
                    <asp:ListItem Value="SFH_NO" Text="<%$ Resources:BindValues, StockTransferNo%>">
                    </asp:ListItem>
                    <asp:ListItem Value="DPT_NAME" Text="<%$ Resources:BindValues, Department%>">
                    </asp:ListItem>
                    <%--<asp:ListItem Value="CFG_DATA" Text="<%$ Resources:BindValues, Status%>">
                    </asp:ListItem>--%>
                    <asp:ListItem Value="SFH_PO_NO" Text="<%$ Resources:Controls, PONo%>">
                    </asp:ListItem>
                    <asp:ListItem Value="SFH_VENDOR_TEXT" Text="<%$ Resources:Controls, Vendor%>">
                    </asp:ListItem>
                    <asp:ListItem Value="Date" Text="<%$ Resources:BindValues, DateRange%>">
                    </asp:ListItem>
                    <asp:ListItem Value="SFH_GRN_NO" Text="<%$ Resources:BindValues, GRNNo%>">
                    </asp:ListItem>
                     <asp:ListItem Value="SFH_GIN_NO" Text="<%$ Resources:BindValues, GINNO%>">
                    </asp:ListItem>
                </asp:DropDownList>
                <div id="divSearchDtls">
                    <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false" TabIndex="2">
                    </asp:TextBox>
                </div>
                <div id="divDate">
                    <label for="FromDate">
                        <%=Resources.Controls.FromDate%></label>
                    <asp:TextBox ID="FromDate" runat="server" TabIndex="3" EnableViewState="false" CssClass="Uidate-picker">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfFrmDate" runat="server" />
                    <label for="ToDate">
                        <%=Resources.Controls.ToDate%></label>
                    <asp:TextBox ID="ToDate" runat="server" TabIndex="4" EnableViewState="false" CssClass="Uidate-picker">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfToDate" runat="server" />
                </div>
                <div id="divFilterStatus">
                    <label for="FilterStatus">
                        <%=Resources.Controls.Status%></label>
                    <asp:DropDownList ID="FilterStatus" runat="server" CssClass="medium" TabIndex="5"
                        onchange="javascript:BindGrid();" EnableViewState="false">
                        <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterAll %>" Text="<%$ Resources:BindValues, StatusFilterAll%>">
                        </asp:ListItem>
                        <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterApproved %>" Text="<%$ Resources:BindValues, StatusFilterApproved%>">
                        </asp:ListItem>
                        <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterNotApproved %>"
                            Text="<%$ Resources:BindValues, StatusFilterNotApproved%>" Selected="True">
                        </asp:ListItem>
                        <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterCancelled %>" Text="<%$ Resources:BindValues, StatusFilterCancelled%>"> </asp:ListItem>
                    </asp:DropDownList>
                </div>
                <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" TabIndex="5" OnClientClick="javascript:return BindGrid();"
                    EnableViewState="false" />
            </div>
            <div class="clear">
            </div>
        </div>
        </div> 
  <%--******************End Old Search region*******************=========--%>
   <%--============Advance Search Region===============================--%>
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
                      <asp:Label runat="server" ID="lblFromDate" Text="<%$ resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                        <asp:TextBox runat="server" ID="txtFromDate" CssClass="input-small" TabIndex="1"
                            onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                        <asp:HiddenField ID="hdfFromDate" runat="server" />
                        <asp:Label runat="server" ID="lblToDate" Text="<%$ resources:ToDate %>" CssClass="middle-lbl-c" AssociatedControlID="txtToDate"></asp:Label>
                        <asp:TextBox runat="server" ID="txtToDate" TabIndex="2" CssClass="input-small"  onkeydown="return CheckKey(event)"
                            onpaste="return false;"></asp:TextBox>                                  
                         <asp:HiddenField ID="hdfToDateNew" runat="server" />
                         <div class="clear"></div>
                         <asp:Label runat="server" ID="lblRefNo" Text="<%$ Resources:BindValues, Department%>"  AssociatedControlID="txtDepartment"></asp:Label>
                        <asp:TextBox runat="server" ID="txtDepartment" TabIndex="5" CssClass="select-half"></asp:TextBox>
                          <asp:HiddenField ID="hdfDepPk" runat="server" Value="0" />
                    </div>
                </td>
                <td>
                    <div class="div2col-S padgtop7">
                    <asp:Label runat="server" ID="lblIONo" Text="<%$ resources:PoNo %>"  CssClass="middle-lbl-xsmall"   AssociatedControlID="txtPONo"></asp:Label>
                        <asp:TextBox runat="server" ID="txtPONo" TabIndex="3" CssClass="select-small-d"></asp:TextBox>                      
                        <asp:HiddenField ID="hdfPONumber" runat="server" />                    

                      <asp:Label runat="server" ID="lblGRNNo" Text="<%$ resources:GRNNo %>" CssClass="lbl-12-7perc" AssociatedControlID="txtGRNNumber"></asp:Label>
                        <asp:TextBox runat="server" ID="txtGRNNumber" TabIndex="4" CssClass="select-small-c1">
                        </asp:TextBox>
                        <asp:HiddenField ID="hdfGRNNumber" runat="server" Value="0" /> 
                        <div class="clear"></div>

                        <asp:Label runat="server" ID="lblGINNo" Text="<%$ resources:GINNo %>" CssClass="middle-lbl-xsmall"  AssociatedControlID="txtGINNo"></asp:Label>
                        <asp:TextBox runat="server" ID="txtGINNo" TabIndex="6" CssClass="select-small-d margnbotm0">
                        </asp:TextBox>
                        <asp:HiddenField ID="hdfGINNo" runat="server" Value="0" />
                        <%-----------   Plant ----------------%>
                            <div id="divPlantCode" class="disp-inline">
                              <asp:Label runat="server" ID="lblPlantCode" Text="<%$ Resources:Controls,CompanyPlant%>" CssClass="lbl-11-3perc" AssociatedControlID="ddlPlantCode"></asp:Label>                                        
                                    <asp:DropDownList ID="ddlPlantCode" runat="server" CssClass="select-small-e" TabIndex="6" EnableViewState="false">                   
                                    </asp:DropDownList>
                            </div>                       
                       <%--------    End Plant ---------------%> 
                    </div>
                </td>
            </tr>
        </table>
        <table class="table-devide">
            <tr>
                <td>
                    <div class="div2col-S div-separatn">
                        <asp:Label runat="server" ID="lblVendor" Text="<%$ resources:Vendor %>" AssociatedControlID="txtVendor"></asp:Label>
                        <asp:TextBox runat="server" ID="txtVendor" CssClass="select-half margnbotm0" TabIndex="7">
                        </asp:TextBox>
                        <asp:HiddenField ID="hdfVendor" runat="server" />
                    </div>
                </td>
                <td>
                    <div class="div2col-S div-separatn">
                    <asp:Label runat="server" ID="Label2" Text="<%$ resources:SANo %>" CssClass="middle-lbl-xsmall" AssociatedControlID="txtSANo"></asp:Label>
                        <asp:TextBox runat="server" ID="txtSANo" TabIndex="8" CssClass="select-small-d margnbotm0">
                        </asp:TextBox>
                        <asp:HiddenField ID="hdfSANo" runat="server" Value="0" />
                        
                        <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status %>" AssociatedControlID="ddlStatus"
                            CssClass="lbl-12-7perc"></asp:Label>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-w27per margnbotm0 margn-rgt2"
                            TabIndex="9" EnableViewState="false">
                            <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="-1"></asp:ListItem>
                             <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="0" Selected="True"></asp:ListItem>
                             <asp:ListItem Text="<%$ Resources:Captions,Approved %>" Value="2"></asp:ListItem>
                             <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="4"></asp:ListItem>           
                        </asp:DropDownList>
                        <asp:Label ID="Label1" runat="server" CssClass="middle-lbl-xsmall-d style-none margnbotm0"></asp:Label>
                        <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                            TabIndex="9" SkinID="search-ext" Style="margin-bottom: 0px!important; margin-top: 2px;"
                            OnClientClick="javascript:return BindGrid();" />
                        <asp:ImageButton ID="btnClear" runat="server" TabIndex="9" Style="margin-bottom: 0px!important;
                            margin-top: 2px;" ToolTip="<%$ resources:Controls,Clear %>" SkinID="clear-ext"
                            OnClientClick="javascript:return SearchAutoInit();" />
                    </div>
                </td>
            </tr>
        </table>
        <div class="clear">
        </div>
   <%--=================End advance search===============================================--%>
        <div class="gridwrap">
            <table rules="all" id="grdGINList" grandtype="GrandGrid" pagesize="20" paging="true"
                width="100%" width="110%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap filter-arrow">
                <thead>
                    <tr>
                        <th fieldmap="SFH_PK" isvisible="false">
                        </th>
                        <th fieldmap="CMP_LINE_COLOUR" isvisible="false">
                        </th>
                        <th fieldmap="SFH_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="LAST_MOD_DT" isvisible="false">
                        </th>
                        <th fieldmap="SFH_ACTIVE" isvisible="false">
                        </th>
                        <th fieldmap="SFH_DEPT" isvisible="false">
                        </th>
                        <th fieldmap="USER_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="REF_ID" isvisible="false">
                        </th>
                        <th fieldmap="SFH_NO" width="10%" sortable="true" align="left">
                            <%=Resources.Controls.SaNo%>
                        </th>
                        <th fieldmap="CMP_DISPLAY_CODE_TEXT"  align="left" width="2%">                           
                        </th> 
                        <th fieldmap="SFH_DATE" sortable="true" width="8%" align="left">
                            <%=Resources.BindValues.Date%>
                        </th>
                        <th fieldmap="SFH_PO_NO" sortable="true" align="left" width="15%">
                            <%=Resources.Controls.PONo%>    
                        </th>
                          <th fieldmap="SFH_GRN_NO" sortable="true" align="left" width="15%">
                            <%=Resources.Controls.GRNNo%>    
                        </th>
                        <th fieldmap="SFH_VENDOR_TEXT" sortable="true" align="left" width="23%">
                            <%=Resources.Controls.Vendor%>
                        </th>
                        <th fieldmap="DPT_NAME" sortable="true" width="11%" align="left">
                            <%=Resources.Controls.Department%>
                        </th>
                        <th fieldmap="CFG_DATA" sortable="true" width="8%" align="left">
                            <%=Resources.Controls.Status%>
                        </th>
                        <th type="Template" width="8%" align="left">
                            <div style="text-align: left">
                                <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$Resources:Controls,Edit %>"
                                    SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PERFORMACTION')" />
                                <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$Resources:Controls,Delete %>"
                                    SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                <asp:ImageButton runat="server" ID="imbModify" SkinID="imbeditgrid" title="<%$Resources:Controls,Modify%>" 
                                     OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'MODIFY')" />
                                <asp:ImageButton runat="server" ID="imbCancel" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'CANCEL')"
                                    SkinID="cancel" alt="<%$Resources:Controls,Cancel%>" title="<%$Resources:Controls,Cancel%>" />
                                <asp:ImageButton runat="server" ID="imbView" ToolTip="<%$Resources:Controls,View %>"
                                    SkinID="btnview" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'VIEW')" />
                                <asp:ImageButton runat="server" ID="imbPrint" ToolTip="<%$Resources:Controls,Print %>"
                                    SkinID="btnPrint" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PRINT')" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
         <asp:HiddenField ID="hdnModifySA" runat="server" Value="0" />
         <asp:HiddenField ID="hdnCancelSA" runat="server" Value="0" />
         <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />   
         <asp:HiddenField ID="hdfDeptID" runat="server" Value="0" />      
        <div class="clear">
        </div>
    </div>
</asp:Content>
