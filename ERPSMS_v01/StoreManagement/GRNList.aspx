<%@ Page Title="<%$ Resources:Captions,Title_GRN %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="GRNList.aspx.cs" Inherits="ERPSMS_v01.StoreManagement.GRNList"
    Theme="ClassicExt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/GRNList.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--<div id="webwizard-wrap">
        <h1>
             <%=Resources.Captions.GoodsRecieptNote%>
        </h1>
        <div class="button-wrap">
            <asp:ImageButton ID="imbAdd" runat="server" SkinID="btnadd" OnClientClick="javascript:return AddNew();"
                EnableViewState="False" TabIndex="6" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();"
                EnableViewState="False" TabIndex="7" />
                <asp:HiddenField ID="hdfProcId" runat="server"  Value="0"/>    
        </div>
    </div>--%>
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
                                    OnClientClick="javascript:return AddNew();" TabIndex="10" EnableViewState="false" />
                            </li>
                            <li>
                                <asp:Button ID="btnReset" runat="server" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Refresh%>"
                                    OnClientClick="javascript:return ResetPage();" TabIndex="9" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
        </div>
        <div class="clear">
        </div>
    </div>
    <div class="content-wrapper">
  <%--******************Old Search region*******************===========--%>
        <div style="display: none">
        <div id="searchwrap" class="search-wrap-custom1">
            <label for="SearchType">
                <%=Resources.Controls.SearchBy%></label>
            <asp:DropDownList ID="SearchType" runat="server" CssClass="medium" TabIndex="1" onchange="javascript:SetSearchType();"
                EnableViewState="false">
                <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                </asp:ListItem>
                <asp:ListItem Value="GRH_NO" Text="<%$ Resources:BindValues, GRNNo %>">
                </asp:ListItem>
                <%--<asp:ListItem Value="Date" Text="<%$ Resources:BindValues, DateRange %>">
                </asp:ListItem>--%>
                <asp:ListItem Value="GRH_PO_NO" Text="<%$ Resources:Controls,PONo %>">
                </asp:ListItem>
                <asp:ListItem Value="GRH_VENDOR_TEXT" Text="<%$ Resources:Controls, Vendor %>">
                </asp:ListItem>
                <asp:ListItem Value="GRH_VND_REF_NO" Text="<%$ Resources:Controls, ReferenceNo%>">
                </asp:ListItem>
            </asp:DropDownList>
            <div id="divSearchDtls">
                <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false" Width="150px"
                    TabIndex="2">
                </asp:TextBox>
            </div>
            <div id="divDateOld">
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
                    <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterClosed %>" Text="<%$ Resources:BindValues, StatusFilterClosed%>">
                    </asp:ListItem>
                    <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterNotClosed %>" Text="<%$ Resources:BindValues, StatusFilterNotClosed%>"
                        Selected="True">
                    </asp:ListItem>
                    <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterCancelled %>" Text="<%$ Resources:BindValues, StatusFilterCancelled%>"> </asp:ListItem>
                </asp:DropDownList>
            </div>
            <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" TabIndex="5" OnClientClick="javascript:return BindGrid();"
                EnableViewState="false" />
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
                        <asp:Label runat="server" ID="lblToDate" Text="<%$ resources:ToDate %>" AssociatedControlID="txtToDate"
                            CssClass="middle-lbl-small-d"></asp:Label>
                        <asp:TextBox runat="server" ID="txtToDate" CssClass="input-small" TabIndex="2" onkeydown="return CheckKey(event)"
                            onpaste="return false;"></asp:TextBox>                                  
                         <asp:HiddenField ID="hdfToDateNew" runat="server" />

                         <div class="clear"></div>
                         <%-----------   Plant ----------------%>
                             <div id="divPlantCode" class="div2col-S">
                              <asp:Label runat="server" ID="lblPlantCode" Text="<%$ Resources:Controls,CompanyPlant%>" AssociatedControlID="ddlPlantCode"
                                            ></asp:Label>                                        
                                    <asp:DropDownList ID="ddlPlantCode" runat="server" CssClass="select-small-a1" TabIndex="4" EnableViewState="false">                   
                                    </asp:DropDownList>
                                </div>                       
                       <%--------    End Plant ----------%>   
                    </div>
                </td>
                <td>
                    <div class="div2col-S padgtop7">
                          <asp:Label runat="server" ID="lblIONo" Text="<%$ resources:PoNo %>" CssClass="middle-lbl"
                            AssociatedControlID="txtPONo"></asp:Label>
                        <asp:TextBox runat="server" ID="txtPONo" TabIndex="3" CssClass="input-small-c0"></asp:TextBox>                      
                        <asp:HiddenField ID="hdfPONumber" runat="server" />
                        <asp:Label runat="server" ID="lblRefNo" Text="<%$ Resources:Controls, ReferenceNo%>" CssClass="middle-lbl-small" AssociatedControlID="txtReferenceNo"></asp:Label>
                        <asp:TextBox runat="server" ID="txtReferenceNo" TabIndex="3" CssClass="select-small-d"></asp:TextBox>                                     
                    </div>
                </td>
            </tr>
        </table>
        <table class="table-devide">
            <tr>
                <td>
                    <div class="div2col-S div-separatn">
                        <asp:Label runat="server" ID="lblVendor" Text="<%$ resources:Vendor %>" AssociatedControlID="txtVendor"></asp:Label>
                        <asp:TextBox runat="server" ID="txtVendor" CssClass="select-half margnbotm0" TabIndex="4">
                        </asp:TextBox>
                        <asp:HiddenField ID="hdfVendor" runat="server" />
                    </div>
                </td>
                <td>
                    <div class="div2col-S div-separatn">
                        <asp:Label runat="server" ID="lblGRNNo" Text="<%$ resources:GRNNo %>" CssClass="middle-lbl" AssociatedControlID="txtGRNNumber"></asp:Label>
                        <asp:TextBox runat="server" ID="txtGRNNumber" TabIndex="5" CssClass="input-small-c0 margnbotm0">
                        </asp:TextBox>
                        <asp:HiddenField ID="hdfGRNNumber" runat="server" Value="0" />
                        <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status %>" AssociatedControlID="ddlStatus"
                            CssClass="middle-lbl-small"></asp:Label>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-b margnbotm0 margn-rgt2"
                            TabIndex="6" EnableViewState="false">
                            <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="-1"></asp:ListItem>
                             <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="0"></asp:ListItem>                                           
                             <asp:ListItem Text="<%$ Resources:Captions,Submitted %>" Value="1"></asp:ListItem>
                             <asp:ListItem Text="<%$ Resources:Captions,Approved %>" Value="2"></asp:ListItem>
                             <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="4"></asp:ListItem> 
                             <asp:ListItem Text="<%$ Resources:BindValues, StatusFilterClosed%>" Value="5"></asp:ListItem> 
                             <asp:ListItem Text="<%$ Resources:BindValues, StatusFilterNotClosed%>" Value="9"></asp:ListItem>           
                        </asp:DropDownList>
                        <asp:Label ID="Label1" runat="server" CssClass="middle-lbl-xsmall-d style-none margnbotm0"></asp:Label>
                        <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                            TabIndex="7" SkinID="search-ext" Style="margin-bottom: 0px!important; margin-top: 2px;"
                            OnClientClick="javascript:return BindGrid();" />
                        <asp:ImageButton ID="btnClear" runat="server" TabIndex="7" Style="margin-bottom: 0px!important;
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
            <table rules="all" id="grdGRNList" grandtype="GrandGrid" pagesize="20" paging="true"
                width="100%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap filter-arrow">
                <thead>
                    <tr>
                        <th fieldmap="GRH_PK" isvisible="false">
                        </th>
                        <th fieldmap="CMP_LINE_COLOUR" isvisible="false">
                        </th>
                        <th fieldmap="GRH_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="USER_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="REF_ID" isvisible="false">
                            <%=Resources.Controls.ReferenceID%>
                        </th>
                        <th fieldmap="GRH_DATE" sortable="true" width="8%" align="left">
                            <%=Resources.Controls.GRNDate%>
                        </th>
                        <th fieldmap="GRH_NO" sortable="true" align="left" width="10%">
                            <%=Resources.Controls.GRNNo%>
                        </th>
                        <th fieldmap="CMP_DISPLAY_CODE"  align="left" width="2%">                           
                        </th> 
                        <th fieldmap="GRH_PO_NO" sortable="true" align="left" width="15%">
                            <%=Resources.Controls.PONo%>
                        </th>
                        <th fieldmap="GRH_VENDOR_TEXT" sortable="true" align="left" width="27%">
                            <%=Resources.Controls.Vendor%>
                        </th>
                        <th fieldmap="GRH_VND_REF_NO" sortable="true" width="10%" align="left">
                            <%=Resources.Controls.ReferenceNo%>
                        </th>
                        <th fieldmap="DPT_NAME" sortable="true" width="11%" align="left">
                            <%=Resources.Controls.Department%>
                        </th>
                        <th fieldmap="GRH_STATUS_TEXT" sortable="true" width="8%" align="left">
                            <%=Resources.Controls.Status%>
                        </th>
                        <th type="Template" width="9%" align="left">
                            <div style="text-align: left">
                                <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" ToolTip="<%$ resources:Controls,Edit %>"  TabIndex="8"
                                    OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PERFORMACTION')" />
                                <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" ToolTip="<%$ resources:Controls,Delete %>" TabIndex="8"
                                    OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                <asp:ImageButton runat="server" ID="imbModify" SkinID="imbeditgrid" title="<%$Resources:Controls,Modify%>"  TabIndex="8"
                                     OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'MODIFY')" />
                                <asp:ImageButton runat="server" ID="imbCancel" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'CANCEL')" TabIndex="8"
                                    SkinID="cancel" alt="<%$Resources:Controls,Cancel%>" title="<%$Resources:Controls,Cancel%>" />
                                <asp:ImageButton runat="server" ID="imbView" SkinID="btnview" ToolTip="<%$ resources:Controls,View %>" TabIndex="8"
                                    OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'VIEW')" />
                                <asp:ImageButton runat="server" ID="imbPrint" SkinID="btnPrint" ToolTip="<%$ resources:Controls,Print %>" TabIndex="8"
                                    OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PRINT')" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <asp:HiddenField ID="hdfAppType" runat="server" />
        <asp:HiddenField ID="hdfAppSubType" runat="server" />
        <asp:HiddenField ID="hdnModifyGRN" runat="server" Value="0" />
        <asp:HiddenField ID="hdnCancelGRN" runat="server" Value="0" />
        <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
        <asp:HiddenField ID="hdfDeptID" runat="server"  Value="0" />
        <div class="clear">
        </div>
    </div>
</asp:Content>
