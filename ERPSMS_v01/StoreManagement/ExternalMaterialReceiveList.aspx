<%@ Page Title="<%$ Resources:Captions,Title_ExternalMaterialReceive %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="ExternalMaterialReceiveList.aspx.cs" Theme="ClassicExt" Inherits="ERPSMS_v01.StoreManagement.ExternalMaterialReceiveList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/ExternalMaterialReciveList.js.axd"
        type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--<div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.ExternalMaterialReceivedList%>
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton ID="imbAdd" runat="server" SkinID="btnadd" OnClientClick="javascript:return AddNew();" />
            <asp:ImageButton ID="imbSave" runat="server" SkinID="btnsave" OnClientClick="javascript:return ClearPage();" />
            <asp:ImageButton ID="imbReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();" />
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
                                    OnClientClick="javascript:return AddNew();" ToolTip="<%$Resources:Controls,Add %>"
                                    TabIndex="10" EnableViewState="false" />
                            </li>
                            <li>
                                <asp:Button ID="btnReset" runat="server" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Refresh%>"
                                    OnClientClick="javascript:return ResetPage();" ToolTip="<%$Resources:Controls,Refresh %>"
                                    TabIndex="8" />
                            </li>
                            <%-- <li>
                                <asp:Button ID="btnCancel" runat="server" ToolTip="<%$Resources:Controls,Close %>" OnClientClick="javascript:return CancelFun();"
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
    <div class="content-wrapper">
        <%--  2 for EMR--%>
        <asp:HiddenField ID="hdfType" runat="server" Value="2" />
        <asp:HiddenField ID="transactionType" runat="server" Value="2" />
        <div id="Wofkflowdiv">
            <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCancelFlag" runat="server" Value="0" />
            <asp:HiddenField ID="hdnModifyFlag" runat="server" Value="0" />
        </div>
         <%-- --------old search----------%>
          <div style="display: none">
        <div id="searchwrap" class="search-wrap-custom1">
            <div id="divSearch">
                <label for="SearchType">
                    <%=Resources.Controls.SearchBy%></label>
                <asp:DropDownList ID="SearchType" runat="server"  CssClass="srchboxtextbx"
                    onchange="javascript:SetSearchType();" EnableViewState="false">                    
                </asp:DropDownList>
                <div id="divSearchDtls">
                   <%-- <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false">
                    </asp:TextBox>--%>
                </div>
                <div id="divDate">
                   <%-- <label for="FromDate">
                        <%=Resources.Controls.FromDate%></label>
                    <asp:TextBox ID="FromDate" runat="server" TabIndex="3" EnableViewState="false">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfFrmDate" runat="server" />
                    <label for="ToDate">
                        <%=Resources.Controls.ToDate%></label>
                    <asp:TextBox ID="ToDate" runat="server" TabIndex="4" EnableViewState="false">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfToDate" runat="server" />--%>
                </div>
                <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" OnClientClick="javascript:return BindGrid();"
                    EnableViewState="false" />
            </div>
            <div class="clear">
            </div>
        </div>
         </div>
         <%------- End old search--------------%>
    <%--=====================================BEGIN Advance Search Region==============================================================--%>
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
                            TabIndex="1" />
                        <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                            ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                            TabIndex="1" />
                    </td>
                </tr>
            </table>
        </div>       
        <div class="clear">
        </div>
        <table class="table-devide" id="tbladvancedSearch" style="/*margin-top: 8px; */ background: #f2f2f2;">
            <tr>
                <td>
                    <div class="div2col-S padgtop7">  
                    <asp:Label runat="server" ID="lblFromDate" Text="<%$ resources:FromDate %>" CssClass="lbl-15-1perc-06-01" AssociatedControlID="SearchFromDate"></asp:Label>                        
                       <asp:TextBox ID="SearchFromDate" runat="server" TabIndex="1" CssClass="input-small-b" EnableViewState="false" onkeydown="return CheckKey(event)" onpaste="return false;">
                      </asp:TextBox>
                      <asp:HiddenField ID="hdfSearchFrmDate" runat="server" />
                        <asp:Label runat="server" ID="lblToDate" Text="<%$ resources:ToDate %>" AssociatedControlID="SearchToDate"  CssClass="lbl-9perc-06-01"></asp:Label>                       
                          <asp:TextBox ID="SearchToDate" runat="server" CssClass="input-small-a" TabIndex="2" onkeydown="return CheckKey(event)"  onpaste="return false;" EnableViewState="false">
                          </asp:TextBox>
                       <asp:HiddenField ID="hdfSearchToDate" runat="server" /> 
                        <asp:Label runat="server" ID="Label2" Text="<%$ resources:RefNo %>" AssociatedControlID="txtRefNo" CssClass="lbl-7-5perc"></asp:Label>                        
                       <asp:TextBox ID="txtRefNo" runat="server" TabIndex="3" CssClass="w21perc">
                      </asp:TextBox>                      
                        <div class="clear">
                        </div>
                         <asp:Label runat="server" ID="Label4" Text="<%$ Resources:Controls,MaterialType%>" CssClass="lbl-15-1perc" AssociatedControlID="txtItemCategory"></asp:Label>                        
                       <asp:TextBox ID="txtItemCategory" runat="server" TabIndex="5" CssClass="input-w51per" EnableViewState="false" >
                      </asp:TextBox>
                      <asp:HiddenField ID="hdfItemCatPK" runat="server" />   
                        <%-----------   Plant ----------------%>
                           <%--  <div id="divPlantCode" class="div2col-S">
                              <asp:Label runat="server" ID="lblPlantCode" Text="<%$ Resources:Controls,Plant%>" AssociatedControlID="ddlPlantCode" CssClass="lbl-15-1perc" ></asp:Label>                                        
                                    <asp:DropDownList ID="ddlPlantCode" runat="server" CssClass="select-w21-8per"  TabIndex="6" EnableViewState="false">                   
                                    </asp:DropDownList>
                                </div>      --%>                 
                       <%--------    End Plant ----------%>                            
                    </div>
                </td>
                <td>
                    <div class="div2col-S padgtop7">
                       <asp:Label runat="server" ID="Label5" Text="<%$ resources:LotNo %>" AssociatedControlID="txtLotno" ></asp:Label>                        
                       <asp:TextBox ID="txtLotno" runat="server" TabIndex="4" CssClass="input-small-b">
                      </asp:TextBox> 
                       <asp:Label runat="server" ID="lblIssuingStore" CssClass="lbl-16-4perc" AssociatedControlID="ddlIssuingStore"><%=ReceivingStore%></asp:Label>
                         <asp:DropDownList ID="ddlIssuingStore" runat="server" TabIndex="4" CssClass="w26perc"  EnableViewState="False">
                         </asp:DropDownList>                        
                        <div class="clear">
                        </div>  
                          <asp:Label runat="server" ID="Label6" Text="<%$ Resources:Controls,Item%>" AssociatedControlID="txtItem"></asp:Label>                        
                       <asp:TextBox ID="txtItem" runat="server" TabIndex="6" CssClass="input-w64per" EnableViewState="false" >
                      </asp:TextBox>
                      <asp:HiddenField ID="hdfItemPk" runat="server" />    
                    </div>
                </td>
            </tr>
        </table>
        <table class="table-devide">
            <tr>
                <td>
                    <div class="div2col-S div-separatn"> 
                      <asp:Label runat="server" ID="lblIssueN0" CssClass="lbl-15-1perc-06-01"   AssociatedControlID="SearchValue"><%=ReceiptNo%></asp:Label>                       
                        <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false" TabIndex="7"  CssClass="input-small-b margnbotm0">
                        </asp:TextBox> 
                         <asp:Label runat="server" ID="lblTrnStatus" Text="<%$ resources:TrnStatus %>" CssClass="lbl-9perc-06-01" AssociatedControlID="ddlTrnStatus"></asp:Label>                       
                        <asp:DropDownList ID="ddlTrnStatus" runat="server" CssClass="w20perc margnbotm0" TabIndex="7">                                              
                             <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="-1"></asp:ListItem>
                             <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="0"></asp:ListItem>                                           
                             <asp:ListItem Text="<%$ Resources:Captions,Submitted %>" Value="1"></asp:ListItem>
                             <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="4"></asp:ListItem>                                                                             
                        </asp:DropDownList>                          
                       <asp:Label runat="server" ID="lblType" Text="<%$ Resources:BindValues,Type %>"  CssClass="lbl-7-5perc"  AssociatedControlID="ddlType"></asp:Label>
                           <asp:DropDownList ID="ddlType" runat="server" TabIndex="7" CssClass="select-w22-3per margnbotm0" onchange="javascript:FillIssueToCategories();" EnableViewState="False" >  </asp:DropDownList>
                            <asp:HiddenField ID="hdn_ICH_ISS_RCV_TYPE" runat="server" Value="0" />                      
                    </div>
                </td>
                <td>
                    <div class="div2col-S div-separatn">
                       <asp:Label runat="server" ID="Label3" AssociatedControlID="txtIssueTo"><%=ReceiveFrom%></asp:Label>
                          <asp:TextBox ID="txtIssueTo" runat="server" TabIndex="7" CssClass="input-w64per margnbotm0">   </asp:TextBox>
                          <asp:HiddenField ID="ICH_ISS_RCV_PK" runat="server" Value="0"></asp:HiddenField>
                        <asp:Label ID="Label1" runat="server" CssClass="middle-lbl-xsmall-d style-none margnbotm0"></asp:Label>
                        <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                            TabIndex="7" SkinID="search-ext"  CssClass="margntop2 margnbotm0"
                            OnClientClick="javascript:return BindGrid();" />
                        <asp:ImageButton ID="btnClear" runat="server" TabIndex="7"  CssClass="margntop2 margnbotm0" ToolTip="<%$ resources:Controls,Clear %>" SkinID="clear-ext"
                            OnClientClick="javascript:return SearchAutoInit();" />
                    </div>
                </td>
            </tr>
        </table>
        <div class="clear">
        </div>
   <%--==================================END Advance Search Region=======================================================================--%>
        <div class="gridwrap">
            <table rules="all" id="grdRequsitionList" grandtype="GrandGrid" pagesize="20" paging="true"
                editfunction="GridAction" editable="true" width="100%" class="gridwraptable gridwrap filter-arrow">
                <thead>
                    <tr>
                        <th fieldmap="ICH_PK" isvisible="false">
                        </th>
                        <th fieldmap="ICH_DATE" sortable="true" align="left" width="8%">
                            <%=Resources.Controls.Date%>
                        </th>
                        <th fieldmap="ICH_NO" sortable="true" align="left" width="10%">
                            <%=ReceiptNo%>
                        </th>
                         <th fieldmap="CMP_DISPLAY_CODE_TEXT"  align="left" width="2%" isvisible="false">                           
                        </th> 
                        <th fieldmap="ICH_ISS_RCV_TYPE_TEXT" sortable="true" align="left" width="8%">
                            <%=Resources.Controls.Type%>
                        </th>
                        <th fieldmap="ICH_ISS_RCV_NAME" sortable="true" align="left" width="12%">
                            <%=Resources.Controls.ReceiveFrom%>
                        </th>
                        <th fieldmap="ICH_DEPT_TEXT" sortable="true" align="left" width="13%">
                            <%=ReceivingStore%>
                        </th>
                        <th fieldmap="ICH_ITEM_CODE" sortable="true" align="left" width="21%">
                            <%=Resources.Controls.ICHItem%>
                        </th>
                        <th fieldmap="ICH_ITEM_TEXT" sortable="true" align="left" width="20%" isvisible="false">
                            <%=Resources.Controls.ICHItem%>
                        </th>
                        <th fieldmap="ICH_STATUS_TEXT" sortable="true" align="left" width="7%">
                            <%=Resources.Controls.Status%>
                        </th>
                        <th fieldmap="ICH_MOD_BY_TEXT" sortable="true" align="left" width="10%">
                            <%=Resources.Controls.DoneBy%>
                        </th>
                        <th fieldmap="ICH_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="REF_ID" isvisible="false">
                        </th>
                        <th type="Template" width="13%" align="left">
                            <div style="text-align: right">
                                <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$Resources:Controls,Edit %>"
                                    SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PERFORMACTION')" />
                                <asp:ImageButton runat="server" ID="imbView" ToolTip="<%$Resources:Controls,View %>"
                                    SkinID="btnview" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'VIEW')" />
                                <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$Resources:Controls,Delete %>"
                                    SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                <asp:ImageButton runat="server" ID="imbModify" SkinID="imbeditgrid" title="<%$Resources:Controls,Modify%>"
                                    OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'MODIFY')" />
                                <asp:ImageButton runat="server" ID="imbCancel" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'CANCEL')"
                                    SkinID="cancel" alt="<%$Resources:Controls,Cancel%>" title="<%$Resources:Controls,Cancel%>" />
                                <asp:ImageButton runat="server" ID="imbPrint" ToolTip="<%$Resources:Controls,Print %>"
                                    SkinID="btnPrint" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PRINT')" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
            <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" /> 
            <asp:HiddenField runat="server" ID="hdfSortColumn" Value="" />
            <asp:HiddenField runat="server" ID="hdfSortColumn1" Value="" />
            <asp:HiddenField runat="server" ID="hdfSortOrder" Value="" />
            <asp:HiddenField runat="server" ID="hdfSortOrder1" Value="" />
            <asp:HiddenField ID="hdfDeptID" runat="server" Value="0" />  
        </div>
        <div class="clear">
        </div>
    </div>
</asp:Content>
