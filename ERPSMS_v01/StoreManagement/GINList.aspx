<%@ Page Title="<%$ Resources:Captions,Title_GIN %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="GINList.aspx.cs" Inherits="ERPSMS_v01.StoreManagement.GINList"
    Theme="ClassicExt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/GINList.js.axd" type="text/javascript"></script>
    <script type="text/javascript">
        function getPK(obj) {
            var value = $(obj).closest('tr').children('td:first').text();
            alert(value);
            var url = "../Reports/GenerateReport.aspx?ID=" + value + "&APPTYPE=" + $("[id$=hdfAppType]").val() + "&APPSUBTYPE=" + $("[id$=hdfAppSubType]").val();
            $("[id$=hdfPrintUrl]").val(url);
            $("[id$=btnPrintNew]").click();
           // window.location = url;
//            __doPostback("btnPrintNew", 'onClick');
//            window.open(url);
//            alert(url);
//            
//            return url;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%-- <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.GoodsInspectionNote%>
        </h1>
        <div class="button-wrap">
            <asp:ImageButton ID="imbAdd" runat="server" SkinID="btnadd" OnClientClick="javascript:return AddNew();"
                EnableViewState="False" TabIndex="6" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();"
                EnableViewState="False" TabIndex="7" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" Visible="false" PostBackUrl="~/AccountManagement/Inbox.aspx"
                TabIndex="8" />
               
        </div>
    </div>
    <div id="Wofkflowdiv">
             
                 <asp:HiddenField ID="hdfProcId" runat="server"  Value="0"/>          
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
                        <li style="display:none" > 
                                <%--<asp:ImageButton ID="imbAdd" runat="server" SkinID="btnInner-New" OnClientClick="javascript:return AddNew();"
                                    TabIndex="10" EnableViewState="false" />--%>
                                <asp:Button ID="btnPrintNew" runat="server" SkinID="btnInner-Print" Text="<%$Resources:Controls,Print%>"
                                    OnClick="ActionHandler" />
                            </li>
                            <li>
                                <%--<asp:ImageButton ID="imbAdd" runat="server" SkinID="btnInner-New" OnClientClick="javascript:return AddNew();"
                                    TabIndex="10" EnableViewState="false" />--%>
                                <asp:Button ID="btnAdd" runat="server" SkinID="btnInner-New" Text="<%$Resources:Controls,Add%>"
                                    OnClientClick="javascript:return AddNew();" TabIndex="10" EnableViewState="false" />
                            </li>
                            <li>
                                <%--<asp:ImageButton ID="imbReset" runat="server" SkinID="btnInner-refresh" OnClientClick="javascript:return ResetPage();"
                                    TabIndex="8" />--%>
                                <asp:Button ID="btnReset" runat="server" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Refresh%>"
                                    OnClientClick="javascript:return ResetPage();" TabIndex="10" />
                            </li>
                            <%--  <li>
                              <asp:Button ID="btnCancel" runat="server" OnClientClick="javascript:return CancelFun();"
                                    SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Close%>" />
                            </li>--%>
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
                <asp:DropDownList ID="SearchType" runat="server" CssClass="medium" TabIndex="1"
                    onchange="javascript:SetSearchType();" EnableViewState="false">
                    <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                    </asp:ListItem>
                    <asp:ListItem Value="GIH_NO" Text="<%$ Resources:BindValues, GINNO%>">
                    </asp:ListItem>
                    <asp:ListItem Value="DPT_NAME" Text="<%$ Resources:BindValues, Department%>">
                    </asp:ListItem>
                    <asp:ListItem Value="Date" Text="<%$ Resources:BindValues, DateRange%>">
                    </asp:ListItem>
                     <asp:ListItem Value="GIH_PO_NO" Text="<%$ Resources:Controls, PONo%>">
                    </asp:ListItem>
                     <asp:ListItem Value="GIH_VENDOR_TEXT" Text="<%$ Resources:Controls, Vendor%>">
                    </asp:ListItem>
                     <asp:ListItem Value="GIH_GRH_NO" Text="<%$ Resources:Controls, GRNNo%>">
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
                <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterNotApproved %>" Text="<%$ Resources:BindValues, StatusFilterNotApproved%>"></asp:ListItem>
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
                        <asp:TextBox runat="server" ID="txtFromDate" CssClass="input-w12-5per" TabIndex="1"
                            onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                        <asp:HiddenField ID="hdfFromDate" runat="server" />
                        <asp:Label runat="server" ID="lblToDate" Text="<%$ resources:ToDate %>" AssociatedControlID="txtToDate"
                            CssClass="lbl-7-6perc"></asp:Label>
                        <asp:TextBox runat="server" ID="txtToDate" CssClass="input-w12-5per" TabIndex="2" onkeydown="return CheckKey(event)"
                            onpaste="return false;"></asp:TextBox>                                  
                         <asp:HiddenField ID="hdfToDateNew" runat="server" />

                   <asp:Label runat="server" ID="lblIONo" Text="<%$ resources:PoNo %>"  CssClass="lbl-7-5perc"  AssociatedControlID="txtPONo"></asp:Label>
                        <asp:TextBox runat="server" ID="txtPONo" TabIndex="3" CssClass="input-small-b"></asp:TextBox>                      
                        <asp:HiddenField ID="hdfPONumber" runat="server" />
                        <div class="clear"></div>

                        <%-----------   Plant ----------------%>
                             <div id="divPlantCode" class="div2col-S">
                              <asp:Label runat="server" ID="lblPlantCode" Text="<%$ Resources:Controls,CompanyPlant%>" AssociatedControlID="ddlPlantCode"></asp:Label>                                        
                                    <asp:DropDownList ID="ddlPlantCode" runat="server" CssClass="input-w13-8per" TabIndex="6" EnableViewState="false">                   
                                    </asp:DropDownList>
                                </div>                       
                       <%--------    End Plant ----------%>   

                    </div>
                </td>
                <td>
                    <div class="div2col-S padgtop7">
                     <asp:Label runat="server" ID="lblRefNo" Text="<%$ Resources:BindValues, Department%>" CssClass="middle-lbl-xsmall" AssociatedControlID="txtDepartment"></asp:Label>
                        <asp:TextBox runat="server" ID="txtDepartment" TabIndex="4" CssClass="select-small-f"></asp:TextBox>
                          <asp:HiddenField ID="hdfDepPk" runat="server" Value="0" /> 

                    <asp:Label runat="server" ID="lblGRNNo" Text="<%$ resources:GRNNo %>" CssClass="lbl-12-7perc" AssociatedControlID="txtGRNNumber"></asp:Label>
                        <asp:TextBox runat="server" ID="txtGRNNumber" TabIndex="5" CssClass="input-small-b">
                        </asp:TextBox>
                        <asp:HiddenField ID="hdfGRNNumber" runat="server" Value="0" />                       
                                                         
                    </div>
                </td>
            </tr>
        </table>
        <table class="table-devide">
            <tr>
                <td>
                    <div class="div2col-S div-separatn">
                        <asp:Label runat="server" ID="lblVendor" Text="<%$ resources:Vendor %>" AssociatedControlID="txtVendor"></asp:Label>
                        <asp:TextBox runat="server" ID="txtVendor" CssClass="select-half-b margnbotm0" TabIndex="6">
                        </asp:TextBox>
                        <asp:HiddenField ID="hdfVendor" runat="server" />
                    </div>
                </td>
                <td>
                    <div class="div2col-S div-separatn">
                        <asp:Label runat="server" ID="lblGINNo" Text="<%$ resources:GINNo %>" CssClass="middle-lbl-xsmall" AssociatedControlID="txtGINNo"></asp:Label>
                        <asp:TextBox runat="server" ID="txtGINNo" TabIndex="7" CssClass="select-small-f margnbotm0">
                        </asp:TextBox>
                        <asp:HiddenField ID="hdfGINNo" runat="server" Value="0" />
                        <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status %>" AssociatedControlID="ddlStatus"
                            CssClass="lbl-12-7perc"></asp:Label>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-w21-8per margnbotm0 margn-rgt2"
                            TabIndex="7" EnableViewState="false">
                            <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="-1"></asp:ListItem>
                             <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="0"></asp:ListItem>                                           
                             <asp:ListItem Text="<%$ Resources:Captions,Submitted %>" Value="1"></asp:ListItem>
                             <asp:ListItem Text="<%$ Resources:Captions,Approved %>" Value="2"></asp:ListItem>
                             <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="4"></asp:ListItem>           
                        </asp:DropDownList>
                        <asp:Label ID="Label1" runat="server" CssClass="middle-lbl-xsmall-d style-none margnbotm0"></asp:Label>
                        <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                            TabIndex="8" SkinID="search-ext" Style="margin-bottom: 0px!important; margin-top: 2px;"
                            OnClientClick="javascript:return BindGrid();" />
                        <asp:ImageButton ID="btnClear" runat="server" TabIndex="8" Style="margin-bottom: 0px!important;
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
                        <th fieldmap="GIH_PK" isvisible="false">
                        </th>
                          <th fieldmap="CMP_LINE_COLOUR" isvisible="false">
                        </th>
                         <th fieldmap="GIH_GRN_HDR" isvisible="false">
                        </th>
                        <th fieldmap="GIH_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="USER_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="REF_ID" isvisible="false">
                            <%=Resources.Controls.ReferenceID%>
                        </th>
                        <th fieldmap="GIH_DATE" sortable="true" width="9%" align="left">
                            <%=Resources.Controls.GINDate%>
                        </th>
                        <th fieldmap="GIH_NO" sortable="true" align="left" width="9%">
                            <%=Resources.Controls.GINNo%>
                        </th>
                        <th fieldmap="CMP_DISPLAY_CODE"  align="left" width="2%">                           
                        </th> 
                        <th fieldmap="GIH_PO_NO" sortable="true" align="left" width="20%">
                            <%=Resources.Controls.PONo%>
                        </th>
                        <th fieldmap="GIH_VENDOR_TEXT" sortable="true" align="left" width="26%">
                            <%=Resources.Controls.Vendor%>
                        </th>
                        <th fieldmap="DPT_NAME" sortable="true" width="12%" align="left">
                            <%=Resources.Controls.Department%>
                        </th>
                        
                        <th fieldmap="GIH_STATUS_TEXT" sortable="true" width="10%" align="left">
                            <%=Resources.Controls.Status%>
                        </th>
                        <th type="Template" width="12%" align="left">
                            <div style="text-align: left">
                                <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" ToolTip="<%$ resources:Controls,Edit %>" TabIndex="9"  OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PERFORMACTION')" />
                                <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" ToolTip="<%$ resources:Controls,Delete %>" TabIndex="9" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                <asp:ImageButton runat="server" ID="imbModify" SkinID="imbeditgrid" title="<%$Resources:Controls,Modify%>"  TabIndex="9"
                                     OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'MODIFY')" />
                                <asp:ImageButton runat="server" ID="imbCancel" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'CANCEL')" TabIndex="9"
                                    SkinID="cancel" alt="<%$Resources:Controls,Cancel%>" title="<%$Resources:Controls,Cancel%>" />
                                <asp:ImageButton runat="server" ID="imbView" SkinID="btnview" ToolTip="<%$ resources:Controls,View %>" TabIndex="9" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'VIEW')" />
                               <asp:ImageButton runat="server" ID="imbPrint" SkinID="btnPrint" ToolTip="<%$ resources:Controls,Print %>"  TabIndex="9" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PRINT')" />
                              <%-- <a id="lnkPrintNew" runat="server" target="_self" clientidmode="Static" ><img src="../Images/ERP-Blue/page-btn-print.gif" /> </a>--%>
                               <%--  <asp:HyperLink ID="HyperLink1" runat="server"><asp:ImageButton runat="server" ID="imbPrint" SkinID="btnPrint" ToolTip="<%$ resources:Controls,Print %>"  OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PRINT')" /></asp:HyperLink>
                                </a>--%>

                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
         <asp:HiddenField ID="hdfAppType" runat="server" />
         <asp:HiddenField ID="hdfAppSubType" runat="server" />
          <asp:HiddenField ID="hdfType"  runat="server" Value="1" />
          <asp:HiddenField ID="hdfPrintUrl" runat="server" />
           <asp:HiddenField ID="hdfClient" runat="server" Value="" />
         <asp:HiddenField ID="hdnModifyGIN" runat="server" Value="0" />
         <asp:HiddenField ID="hdnCancelGIN" runat="server" Value="0" /> 
         <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />   
         <asp:HiddenField ID="hdfDeptID" runat="server" Value="0" />     
        <div class="clear">
        </div>
    </div>
</asp:Content>
