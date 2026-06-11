<%@ Page Title="<%$ Resources:Captions,Title_PurchaseRequest %>" Language="C#" EnableEventValidation="false"
    MasterPageFile="~/ERPSMS_2.Master" Theme="ClassicExt" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="PurchaseRequestCreation.aspx.cs" Inherits="ERPSMS_v01.PurchaseRequestManagement.PurchaseRequestCreation" %>

<%@ Register Src="../WorkFlow/WorkflowComments.ascx" TagName="WorkflowComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/PurchaseRequestManagement/PurchaseRequestCreation.js.axd"
        type="text/javascript"></script>
    <style type="text/css">
        /*Start ToolTip Css for any dom element */
        [tooltip]:before
        {
            position: absolute;
            content: attr(tooltip);
            opacity: 0;
            top: -50;
        }
        
        [tooltip]:hover:before
        {
            opacity: 1;
            background: #feffcd;
            border: 1px solid black;
            padding: 2px;
            margin-top: 12px;
        }
        
        
        [tooltip]:not([tooltip-persistent]):before
        {
            pointer-events: none;
        }
        /*End ToolTip Css for any dom element */
    </style>
    <script type="text/javascript" language="javascript">
        function CheckConfigForShowDescPM() {
            var ShowDescPM = <%=GetGlobalResourceObject("ConfigurationsRes", "ShowDescPM") %>;  
                $("[id$=hdfShowDescPM]").val(ShowDescPM);
            }  
//            On choosing an item from SC.,by default the Specification field shows lotno# of the item .Along with that also show the item description from packing material master
            function CheckConfigForShowBothLotnoDescPM() {
            var ShowBothLotnoDescPM = <%=GetGlobalResourceObject("ConfigurationsRes", "LotNoAlongDescPRSpec") %>;  
                $("[id$=hdfShowBothLotnoDescPM]").val(ShowBothLotnoDescPM);
            } 
            function SetAllConfigurations() {
                var IsPRPMAttachmentShow = <%=GetGlobalResourceObject("ConfigurationsRes", "IsPRPMAttachmentShow") %>;  
                var IsDisableIO = <%=GetGlobalResourceObject("ConfigurationsRes", "IsDisableIO") %>;  
                $("[id$=hdfIsPRPMAttachmentShow]").val(IsPRPMAttachmentShow);
                 $("[id$=hdfIsDisableIO]").val(IsDisableIO);
            }                    
    </script>
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
                                <asp:Button runat="server" ID="btnSubmit" SkinID="btnInner-submit" Text="<%$Resources:Controls,Submit%>"
                                    TabIndex="29" EnableViewState="False" ToolTip="<%$resources:ErpRes,Submit %>"
                                    OnClientClick="javascript:return  WkfPurchaseSubmit();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    TabIndex="30" EnableViewState="False" ToolTip="<%$resources:ErpRes,Save %>" OnClientClick="javascript:return SavePage('Draft');" />
                            </li>
                            <%--  <li>
                                <asp:Button runat="server" ID="btnReset" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Reset%>"
                                    TabIndex="31" EnableViewState="False"  ToolTip="<%$resources:ErpRes,Reset %>"  OnClientClick="javascript:return ResetPage();" />
                            </li>--%>
                            <li>
                                <asp:Button ID="btnClose" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" ToolTip="<%$resources:ErpRes,Cancel %>" OnClientClick="javascript:return CancelPR();"
                                    TabIndex="32" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnPrint" SkinID="btnInner-Print" Text="<%$Resources:Controls,Print%>"
                                    TabIndex="33" EnableViewState="False" ToolTip="<%$resources:ErpRes,Print %>"
                                    OnClientClick="javascript:return PrintPage();" /></li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsGoToInbox" runat="server" Value="0" />
            <asp:HiddenField ID="hdfSlNo" runat="server" Value="-1" />
            <asp:HiddenField ID="hdfDeptID" runat="server" Value="0" />
            <asp:HiddenField ID="hdfAppType" runat="server" />
            <asp:HiddenField ID="hdfAppSubType" runat="server" />
            <asp:HiddenField ID="APT_CODE" runat="server" />
            <asp:HiddenField ID="WKF_FLAG" runat="server" Value="0" />
            <asp:HiddenField ID="WKF_PROCESS" runat="server" Value="" />
            <asp:HiddenField ID="AutoStartValue" runat="server" Value="0" />
            <asp:HiddenField ID="hdfThreshold" runat="server" Value="0" />
            <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
            <asp:HiddenField ID="hdfPRRowCount" runat="server" Value="0" />
            <asp:HiddenField ID="hdfshowbudgetValidation" runat="server" Value="0" />
            <asp:HiddenField ID="BUDGETVALIDATION" runat="server" Value="0" />
        </div>
    </div>
    <%--  <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.PurchaseRequest%>
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
              <asp:ImageButton runat="server" ID="btnSubmit" SkinID="btnsave" TabIndex="15" EnableViewState="False"
                OnClientClick="javascript:return WkfSubmit();" />
            <asp:ImageButton runat="server" ID="imbSave" SkinID="btnsave" TabIndex="15" EnableViewState="False"
                OnClientClick="javascript:return SavePage('Draft');" />
            <asp:ImageButton runat="server" ID="imbReset" SkinID="btnreset" TabIndex="16" EnableViewState="False"
                OnClientClick="javascript:return ResetPage();" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" EnableViewState="False"
                OnClientClick="javascript:return CancelFun();" TabIndex="17" />
            <asp:ImageButton runat="server" ID="imbPrint" SkinID="btnPagePrint" TabIndex="18"
                EnableViewState="False" OnClientClick="javascript:return PrintPage();" />
        </div>
    </div>--%>
    <div class="clear">
    </div>
    <div class="content-wrapper">
        <table class="table-devide" id="tblDetailHdr">
            <tr>
                <td>
                    <div class="div2col-S">
                        <label for="PurchaseRequestNo">
                            <%=Resources.Controls.PRNo%></label>
                        <asp:Label ID="PRH_NO" runat="server" Text="" CssClass="input-small"></asp:Label>
                        <asp:HiddenField ID="PRH_PK" runat="server" Value="0"></asp:HiddenField>
                        <label for="PRH_DATE" class="middle-lbl-small-c">
                            <%=Resources.Controls.Date%>
                            *</label>
                        <asp:TextBox ID="PRH_DATE" runat="server" TabIndex="1" onkeydown="return CheckKey(event)"
                            onpaste="return false;" CssClass="input-small" MaxLength="12"></asp:TextBox>
                        <div class="clear">
                        </div>
                        <div id="divIONumber" class="display-inline">
                            <label for="OrderNo" class="margnrgt3">
                            <%=GetGlobalResourceObject("Controls", "IONumber")%></label>
                                <%--<%=Resources.Controls.IONumber%></label>--%>

                            <%--<asp:DropDownList ID="PRH_SO_HDR" runat="server" CssClass="select-half-a" onchange="javascript:ChangeIO(false);"
                                TabIndex="3" EnableViewState="true">
                            </asp:DropDownList>--%>
                            <asp:TextBox ID="txtSO" runat="server" TabIndex="3" CssClass="select-half">
                            </asp:TextBox>
                             <input type="checkbox" id="chkShowAllSC" onchange="ShowAllSC();" checked="false" title="Show All SC" class="margntop5"
                               tabindex="20" />
                            <asp:HiddenField runat="server" ID="PRH_SO_HDR" Value="0" />
                        </div>
                        <asp:HiddenField ID="hdfCurIONumber" runat="server" Value="0" />
                        <asp:ImageButton ID="imbMaterialShow" runat="server" SkinID="btnview" CssClass="imgbutton-wrap margntop2"
                            TabIndex="4" ToolTip="<%$resources:ErpRes,View %>" OnClientClick="javascript:return ViewPackingMaterialDetails(); return false;"
                            EnableViewState="false" />
                        <asp:HiddenField ID="hdfOrderNo" runat="server" Value="0" />
                        <asp:HiddenField ID="hdfStorePK" runat="server" Value="0" />
                        <asp:HiddenField ID="hdfIsIoNumberHide" runat="server" Value="0" />
                        <asp:HiddenField ID="hdfEnbleCostCenter" runat="server" Value="0" />
                        <div id="divPRType" class="display-inline">
                            <label for="Type" class="margnrgt3">
                                <%=Resources.Controls.Type%></label>
                            <asp:DropDownList runat="server" ID="PRH_PO_CATEGORY" TabIndex="3" CssClass="select-small-b">
                            </asp:DropDownList>
                        </div>
                        <div class="clear">
                        </div>
                        <%-- <div class="clear">
                        </div>--%>
                        <%--Label name change for JTE client; Requested By to Requested For--%>
                        <%--  <label for="User" >  
                            <%=Resources.ConfigurationsRes.RequestedBy%>
                            </label>--%>
                        <asp:Label runat="server" Text='<%$ Resources:ConfigurationsRes,RequestedBy%>' CssClass="bg-none border0 lbl-25-1perc numeric"></asp:Label>
                        <asp:TextBox ID="PRH_USER" runat="server" TabIndex="5" EnableViewState="false" CssClass="input-half-20-11-9 materialspec-textbox"
                            MaxLength="200"></asp:TextBox>
                        <div id="divCostCenter" class="display-inline">
                            <label for="Type" class="margnrgt3">
                                <%=Resources.Controls.CostCenter %></label>
                            <asp:DropDownList runat="server" ID="PRH_COST_CENTER" TabIndex="7" CssClass="select-w61per">
                            </asp:DropDownList>
                        </div>
                       
                        <div id="divInvestor" class="display-inline">
                            <label for="PRH_INVESTOR" class="margnrgt3">
                               <%= GetGlobalResourceObject("Controls","InvestorCode").ToString()%></label>
                            <asp:DropDownList runat="server" ID="PRH_INVESTOR" TabIndex="7" CssClass="select-w61per" Width="61%">
                            </asp:DropDownList>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="div2col-S" style="float: right;">
                        <label for="Store">
                            <%=Resources.Controls.RequestingStore%>
                            *</label>
                        <asp:DropDownList ID="Store" runat="server" TabIndex="2" onchange="javascript:StoreDataClear();"
                            CssClass="select-half" EnableViewState="true">
                        </asp:DropDownList>
                        <label for="ddlIssuingStore">
                            <%=GetGlobalResourceObject("Controls", "RequestedDept")%>
                        </label>
                        <asp:DropDownList ID="PRH_ISSUE_DEPT" runat="server" TabIndex="4" onchange="javascript:ReqDeptChange();"
                            CssClass="select-half">
                        </asp:DropDownList>
                        <asp:Button ID="btnReqDept" runat="server" OnClick="ActionHandler" CommandName="REQDEPTCHANGE"
                            Style="display: none" EnableTheming="false" />
                        <div id="divSubDept" style="display: none;">
                            <label for="ddlIssuingSubStore" class="margnrgt3">
                                <%=GetGlobalResourceObject("Controls", "RequestedSubDept")%>
                            </label>
                            <asp:DropDownList ID="PRH_ISSUE_SUB_DEPT" runat="server" TabIndex="6" CssClass="select-half" onchange="return SubDeptChange();">
                            </asp:DropDownList>
                        </div>
                        <div id="divType" style="display: none">
                            <label for="IPD_TYPE" class="margnrgt3">
                                <%=Resources.Controls.Type%></label>
                            <asp:DropDownList ID="IPD_TYPE" runat="server" onchange="javascript:BindGrid();"
                                CssClass="select-medium " EnableViewState="false" TabIndex="5">
                            </asp:DropDownList>
                            <label for="PRH_PERCENTAGE_EXTRA" class="middle-lbl-small margnrgt1-5per">
                                <%=Resources.Controls.AdlPer%></label>
                            <asp:TextBox ID="PRH_PERCENTAGE_EXTRA" runat="server" TabIndex="6" CssClass="numeric input-w50 margn-rgt0"
                                EnableViewState="false" MaxLength="12"></asp:TextBox>
                            <asp:ImageButton ID="imbCalculatePer" runat="server" SkinID="formula" CssClass="imgbutton-wrap"
                                TabIndex="10" ToolTip="<%$resources:ErpRes,CalcPer %>" OnClientClick="javascript:return CalculatePercentage();"
                                Style="margin-top: 2px;" />
                            <div class="clear">
                            </div>
                        </div>
                        <div id="divGloveRequest" style="display: none;" class="w38perc float-left">
                            <label for="chkIsGloveRequest" class="margnrgt3 lbl-66-6perc">
                                <%=GetGlobalResourceObject("Controls", "GloveRequest")%>
                            </label>
                            <input type="checkbox" id="chkIsGloveRequest" onchange="CheckChangeGloveRequest();" checked="true" title="Glove Request" class="margntop5"
                               tabindex="20" />

                        </div>
                        <div id="divPRCompleted" style="display: none;" class="w46perc float-left">
                            <label for="chkPRCompleted" class="margnrgt3 lbl-32-5perc">
                                <%=GetGlobalResourceObject("Controls", "PRCompleted")%>
                            </label>
                            <input type="checkbox" id="chkPRCompleted" onchange="CheckChangePRComplete();" checked="false" title="PR Completed" class="margntop5"
                               tabindex="20" />

                                <label for="chkShowAllSC" class="margnrgt3 lbl-32-5perc">
                              <%--  <%=GetGlobalResourceObject("Controls", "ShowAll")%>--%>
                            </label>
                           
                        </div>

                        <div id="divPlantCompany">
                            <label for="PRH_COMPANY" class="margnrgt3">
                                <%=GetGlobalResourceObject("Controls","CompanyPlant")%>*</label>
                            <asp:DropDownList ID="PRH_COMPANY" runat="server" TabIndex="7" ClientIDMode="Static"
                                CssClass="select-half">
                            </asp:DropDownList>
                            <asp:HiddenField ID="hdfSelCompany" runat="server" />

                           
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <div class="clear">
        </div>
        <h1 class="search-colapse-normal">
            <%=Resources.Captions.ReOrderMinStockList%>
            <img id="imgReOrderShowHeader" src="../Images/Classic/Icons/arrow-colapse-active.png"
                alt="<%= Resources.Controls.Show%>" title="<%= Resources.Controls.Show%>" style="display: none;
                cursor: pointer;" onclick="javascript:ShowReOrderHeader();" />
            <img id="imgReOrderHideHeader" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                alt="<%= Resources.Controls.Hide%>" title="<%= Resources.Controls.Hide%>" style="cursor: pointer;"
                onclick="javascript:HideReOrderHeader();" />
        </h1>
        <div id="divROL" class="checkbx w100perc">
            <%----%>
            <asp:RadioButton ID="ReOrderLvl" runat="server" TabIndex="8" GroupName="gp" OnChange="javascript:ChangeROLMSCat();"
                Text="<%$ Resources:Controls, ReorderLevel%>" CssClass="float-left"></asp:RadioButton>
            <asp:RadioButton ID="MinStockLvl" runat="server" GroupName="gp" TabIndex="9" OnChange="javascript:ChangeROLMSCat();"
                Text="<%$ Resources:Controls, MinStockLevel%>" CssClass="float-left"></asp:RadioButton>
            <asp:RadioButton ID="rbtnCategory" runat="server" GroupName="gp" TabIndex="9" OnChange="javascript:ChangeROLMSCat();"
                Text="<%$ Resources:Controls, Category%>" CssClass="float-left"></asp:RadioButton>
            <span id="spanItemCat">
                <asp:TextBox ID="txtItemCategory" Width="250px" runat="server" TabIndex="10" CssClass="h14">
                </asp:TextBox>
                <asp:HiddenField ID="hdfItemCategoryPK" runat="server" Value="0"></asp:HiddenField>
            </span><span class="checkbx2">
                <asp:Button ID="btnShow" runat="server" CssClass="checkbx2" Text='<%$ Resources:Controls, ShowItemBelow %>'
                    TabIndex="10" OnClientClick="javascript:ShowItems();return false;" />
            </span>
            <img id="imgReOrderShow" src="../Images/Classic/Icons/arrow-colapse-active.png" alt="<%= Resources.Controls.Show%>"
                title="<%= Resources.Controls.Show%>" style="display: none; cursor: pointer;
                float: right!important; margin-right: 11px!important;" onclick="javascript:ShowReOrder();" />
            <img id="imgReOrderHide" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                alt="<%= Resources.Controls.Hide%>" title="<%= Resources.Controls.Hide%>" style="cursor: pointer;
                float: right!important; margin-right: 11px!important;" onclick="javascript:HideReOrder();" />
        </div>
        <div class="clear">
        </div>
        <div id="divReOrderList" class="grdTable">
            <div class="scroll-h150">
                <table rules="all" id="grdPuchasePendingList" grandtype="GrandGrid" paging="false"
                    enablecheckbox="true" width="100%" class="gridwraptable gridwrap">
                    <thead>
                        <tr>
                            <th fieldmap="ITM_PK" isvisible="false">
                            </th>
                            <th fieldmap="ITM_UOM" isvisible="false">
                            </th>
                            <th fieldmap="SOH_REFERENCE" isvisible="false">
                            </th>
                            <th fieldmap="UOM_CODE" isvisible="false">
                            </th>
                            <th fieldmap="ITM_IS_PM" isvisible="false">
                                <%--For identifying item is packing material or not--%>
                            </th>
                            <%-- Lot no Hiddenfield--%>
                            <th fieldmap="SOD_LOT_NO" isvisible="false">
                            </th>
                            <th fieldmap="ITM_DESC" isvisible="false">
                            </th>
                            <th fieldmap="ITM_CATEGORY" isvisible="false">
                            </th>
                            <th fieldmap="ROW_NO" align="center" width="1%">
                                <%=Resources.Controls.SLNO%>
                            </th>
                            <th fieldmap="ITC_NAME" align="left" width="10%">
                                <%=Resources.Controls.MaterialType%>
                            </th>
                            <th fieldmap="ITM_TEXT" align="left" width="36%">
                                <%=Resources.Controls.MaterialCode%>
                            </th>
                            <th fieldmap="STH_QTY_IN_STOCK" align="right" width="5%">
                                <%=Resources.Controls.Stock%>
                            </th>
                            <th fieldmap="ITM_MIN_STK" align="right" width="8%">
                                <%=Resources.Controls.MinStockLevel%>
                            </th>
                            <th fieldmap="ITM_MAX_STK" align="right" width="8%" isvisible="false">
                                <%=Resources.Controls.MaxStockLevel%>
                            </th>
                            <th runat="server" id="thPRRoL" fieldmap="ITM_ROL_STK" align="right" width="6%">
                                <%=Resources.Controls.ReorderLevel%>
                            </th>
                            <th runat="server" id="thPRScQty" fieldmap="SOD_SO_QTY" align="right" width="6%">
                                <%=Resources.Controls.scQTY%>
                            </th>
                            <th fieldmap="PRD_QTY_APPROVED" align="right" width="11%">
                                <%=Resources.Controls.PrevReqQty%>
                            </th>
                            <th fieldmap="REQUEST_QTY" align="right" width="10%">
                                <%=Resources.Controls.ReQty%>
                            </th>
                            <th fieldmap="REQUEST_SPEC" align="left" width="13%">
                                <%=Resources.Controls.Specification%>
                            </th>
                            <%--<th type="Template" align="center" width="3%">
                                <asp:ImageButton ID="imbAddToList" runat="server" SkinID="imbaddnew" TabIndex="6"
                                    OnClientClick="javascript:return AddPurchaseRequestList($(this).parents('tr:eq(0)'),true )"
                                    Width="16px" />
                            </th>--%>
                        </tr>
                    </thead>
                </table>
            </div>
            <div class="button-wrap-right">
                <asp:Button ID="btnAddSelectedItems" runat="server" Text="<%$ Resources:Controls, AddToList%>"
                    OnClientClick="javascript:return AddPRList(true);" ToolTip="<%$resources:Controls,AddToList %>"
                    TabIndex="12" />
                <div class="clear">
                </div>
            </div>
        </div>
        <div class="clear">
        </div>
        <div id="divData">
            <div id="divFileData">
            </div>
            <%-- <h1 class="h1bg">
               
                <div style="float: right">
                    <img id="imgItemRequestedShow" src="../Images/ERP-Blue/Buttons/arrow-dwn.png" alt="<%= Resources.Controls.Show%>"
                        title="<%= Resources.Controls.Show%>" style="display: none; cursor: pointer"
                        onclick="javascript:ShowItemRequested();" />
                    <img id="imgItemRequestedHide" src="../Images/ERP-Blue/Buttons/arrow-up.png" alt="<%= Resources.Controls.Hide%>"
                        title="<%= Resources.Controls.Hide%>" style="cursor: pointer" onclick="javascript:HideItemRequested();" />
                </div>
            </h1>--%>
            <h1 class="search-colapse-normal">
                <%=Resources.Captions.ItemsRequested%>
                <img id="imgItemRequestedShow" src="../Images/Classic/Icons/arrow-colapse-active.png"
                    alt="<%= Resources.Controls.Show%>" title="<%= Resources.Controls.Show%>" style="display: none;
                    cursor: pointer" onclick="javascript:ShowItemRequested();" />
                <img id="imgItemRequestedHide" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                    alt="<%= Resources.Controls.Hide%>" title="<%= Resources.Controls.Hide%>" style="cursor: pointer"
                    onclick="javascript:HideItemRequested();" />
            </h1>
            <div id="divItemsRequested" class="grdTable">
                <div id="divPurchaseRequestInsert" class="gridwrap grid-maxw1400">
                    <table id="PurchaseRequestInsert" class="gridwraptable gridwrap tablefixwidth-td">
                        <thead>
                            <tr>
                                <th width="160px" style="text-align: left">
                                    <%=Resources.Controls.MaterialType%>*
                                </th>
                                <th width="290px" style="text-align: left">
                                    <b>
                                        <%=Resources.Controls.MaterialCode%>* </b>
                                </th>
                                <th width="90px" class="txt-rgt">
                                    <b>
                                        <%=Resources.Controls.ReQty%></b>*
                                </th>
                                <th width="95px" style="text-align: left">
                                    <b>
                                        <%=Resources.Controls.UOM%>
                                    </b>*
                                </th>
                                <th width="130px" style="text-align: left">
                                    <b>
                                        <%=Resources.Controls.ReqdDate%>
                                    </b>*
                                </th>
                                <th width="150px" style="text-align: left">
                                    <b>
                                        <%=Resources.Controls.Specification%>
                                    </b>
                                </th>
                                <th width="150px" style="text-align: left">
                                    <b>
                                        <%=Resources.Controls.Purpose%>
                                    </b>
                                </th>
                                <th width="40px" style="text-align: left">
                                    <b>
                                        <%=Resources.Controls.Action%></b>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr class="grd-rowhead">
                                <td>
                                    <asp:TextBox ID="MaterialCategory" Width="148px" CssClass="materialspec-textbox" runat="server" TabIndex="14">
                                    </asp:TextBox>
                                    <asp:HiddenField ID="MaterialCategoryPK" runat="server" Value="0"></asp:HiddenField>
                                </td>
                                <td>
                                    <asp:TextBox ID="ItemCodeMaterial"  runat="server" TabIndex="15" CssClass="input-w81per materialspec-textbox">
                                    </asp:TextBox>
                                    <asp:ImageButton ID="imbSearch" runat="server" SkinID="btnview" CssClass="imgbutton-wrap"
                                        TabIndex="16" ToolTip="<%$resources:ErpRes,View %>" OnClientClick="javascript:return ViewMaterialDetails(); return false;"
                                        EnableViewState="false" Style="margin-top: 3px!important;" />
                                    <%-- Adding new items in material master through this page--%>
                                    <asp:ImageButton ID="imbAddNewMaterials" runat="server" SkinID="imbaddnew" CssClass="margntop3"
                                        TabIndex="16" ToolTip="<%$resources:ErpRes,AddNewMaterials %>" OnClientClick="javascript:return ShowAddNewMaterialsPopup(); return false;"
                                        EnableViewState="false" />
                                    <asp:HiddenField ID="MaterialPK" runat="server" Value="0"></asp:HiddenField>
                                    <asp:HiddenField ID="IsEdit" runat="server" Value="false" />
                                    <asp:HiddenField ID="MaterialROL" runat="server" Value="0"></asp:HiddenField>
                                    <asp:HiddenField ID="MaterialMSL" runat="server" Value="0"></asp:HiddenField>
                                </td>
                                <td class="txt-rgt">
                                    <asp:TextBox ID="PurchaseQty" runat="server" Width="150px" TabIndex="17" CssClass="numeric input-w78"
                                        MaxLength="17" EnableViewState="false">
                                    </asp:TextBox>
                                    <asp:HiddenField runat="server" ID="hdfMaxOrderQty" Value="0" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="MaterialUOM" runat="server" Width="70px" TabIndex="18" CssClass="input-uom"
                                        Enabled="false" EnableViewState="false">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="ShowUOM" runat="server" SkinID="formula" CssClass="imgbutton-wrap margntop2"
                                        Style="display: none" ToolTip="<%$resources:Messages,UOMConversion %>" TabIndex="19"
                                        OnClientClick="javascript:return ShowUOMDetails(); return false;" EnableViewState="false" />
                                </td>
                                <td>
                                    <%--<asp:CheckBox ID="chkCheckAll" runat="server" ToolTip="<%$resources:ErpRes,ApplyAll %>" CssClass="floatLeft display-inline" Width="10%" /> --%>
                                    <asp:TextBox ID="RequiredDate" runat="server" Width="100px" onkeydown="return CheckKey(event)"
                                        onpaste="return false;" TabIndex="20" MaxLength="12" CssClass="date-picker floatLeft ">
                                    </asp:TextBox>
                                    <input type="checkbox" id="chkApplyAll" title="Apply req. by date to all" class="margntop5"
                                        tabindex="20" />
                                </td>
                                <td style="width: 150px;">
                                    <asp:TextBox ID="MaterialSpec" CssClass="materialspec-textbox" runat="server" Width="140px" TabIndex="21" MaxLength="2000">
                                    </asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="RequestPorpose"  CssClass="materialspec-textbox" runat="server" Width="112px" TabIndex="22" MaxLength="2000">
                                    </asp:TextBox>
                                    <input type="checkbox" id="chkApplyAllPurpose" title="Apply purpose to all" class="margntop5"
                                        tabindex="22" />
                                </td>
                                <td>
                                    <asp:ImageButton ID="imbAddNew" runat="server" SkinID="imbaddnew" TabIndex="23" OnClientClick="javascript:return AddPurchaseRequest(false);"
                                        Width="16px" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <asp:HiddenField ID="datenewRqst" runat="server" />
                <div class="clear">
                </div>
                <div id="divPurchaseRequest" class="gridwrap grid-maxw1400 ">
                    <table rules="all" id="grdPurchaseRequest" grandtype="GrandGrid" paging="false" editfunction="GridAction"
                        editable="true" class="gridwraptable gridwrap tablefixwidth-td">
                        <thead>
                            <tr>
                                <th fieldmap="PRD_ITEM" isvisible="false">
                                </th>
                                 <th fieldmap="ITM_CATEGORY_VALUE" isvisible="false">
                                </th>
                                <th fieldmap="PRD_UOM" isvisible="false">
                                </th>
                                <th fieldmap="SLNO" isvisible="false">
                                </th>
                                <th fieldmap="ITM_PK" isvisible="false">
                                </th>
                                <th fieldmap="ITM_CATEGORY" isvisible="false">
                                </th>
                                <th fieldmap="ITC_NAME" align="left" width="160px">
                                    <%=Resources.Controls.MaterialType%>*
                                </th>
                                <th fieldmap="ITM_TEXT" align="left" width="285px">
                                    <%=Resources.Controls.MaterialCode%>*
                                </th>
                                <th fieldmap="PRD_QTY_REQUESTED" align="right" width="90px">
                                    <%=Resources.Controls.ReQty%>*
                                </th>
                                <th fieldmap="UOM" align="center" width="100px">
                                    <%=Resources.Controls.UOM%>*
                                </th>
                                <th fieldmap="PRD_REQD_DATE" align="center" width="130px">
                                    <%=Resources.Controls.ReqdDate%>*
                                </th>
                                <th fieldmap="PRD_ITEM_SPEC" align="left" width="160px">
                                    <%=Resources.Controls.Specification%>
                                </th>
                                <th fieldmap="PRD_PURPOSE" align="left" width="150px">
                                    <%=Resources.Controls.Purpose%>
                                </th>
                                <th type="Template" width="50px">
                                    <div>
                                        <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$resources:ErpRes,Edit %>"
                                            SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')"
                                            TabIndex="24" />
                                        <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$resources:ErpRes,Delete %>"
                                            SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')"
                                            TabIndex="25" />
                                    </div>
                                </th>
                                <th fieldmap="ITM_MAX_OQ" isvisible="false">
                            </tr>
                        </thead>
                    </table>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <div id="divFileAttachmentPR" style="display: none">
            <h1 class="search-colapse-normal">
                <%=Resources.Controls.Attachments%>
                <img id="imgShowAttachment" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                    alt="<%= Resources.Controls.Show%>" title="<%= Resources.Controls.Show%>" style="display: none;
                    cursor: pointer" onclick="javascript:ShowAttachment();" />
                <img id="imgHideAttachment" src="../Images/Classic/Icons/arrow-colapse-active.png"
                    alt="<%= Resources.Controls.Hide%>" title="<%= Resources.Controls.Hide%>" style="cursor: pointer"
                    onclick="javascript:HideAttachment();" />
            </h1>
            <div class="divcol-FileuplWrap" id="divAttachmentPR">
                <label for="aupDocument">
                    <%=Resources.Controls.Attachments%></label>
                <div id="FileUploaderPR" class="input-file">
                    <asp:FileUpload ID="fupUploaderPR" runat="server" ClientIDMode="Static" size="29"
                        Height="22px" Style="margin-top: 3px" TabIndex="34" />
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <div id="Wofkflowdiv" style="display: none">
            <uc1:WorkflowComments ID="ucrWrkf" runat="server" ValidationGroup="Save" />
            <asp:HiddenField ID="hdfProcessID" runat="server" Value="0" />
            <asp:HiddenField ID="hdfRefID" runat="server" Value="0" />
            <asp:HiddenField ID="hdfAppID" runat="server" Value="0" />
            <asp:HiddenField runat="server" ID="ActionID" Value="0" />
            <asp:HiddenField ID="AppNo" runat="server" Value="0" />
        </div>
        <div id="divPackingMaterialDetails" title="<%=Resources.Captions.PackbillOfMaterial%>">
            <div class="content-wrapper">
                <table rules="all" id="grdMaterials" grandtype="GrandGrid" paging="false" editable="false"
                    class="gridwraptable gridwrap">
                    <thead>
                        <tr>
                            <th fieldmap="ITM_CODE" align="left" width="22%">
                                <%=Resources.Controls.ItemCode%>
                            </th>
                            <th fieldmap="ITM_NAME" align="left" width="26%">
                                <%=Resources.Controls.ItemName%>
                            </th>
                            <th fieldmap="UOM_CODE" align="left" width="7%">
                                <%=Resources.Controls.UOM%>
                            </th>
                            <th fieldmap="PIM_QTY" align="right" width="8%">
                                <%=Resources.Controls.Quantity%>
                            </th>
                            <th fieldmap="ITM_MOQ" align="right" width="6%">
                                <%=Resources.Controls.MOQ%>
                            </th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
        <div id="divMetarialDetails" title="<%=Resources.Captions.MaterialDescription%>">
            <div class="div2Col-M">
                <label for="ThisStoreStock" style="width: 150px">
                    <%=Resources.Controls.CurrentStock%>
                </label>
                <asp:Label ID="ThisStoreStock" runat="server" Font-Bold="true" EnableViewState="false"></asp:Label>
                <div class="clear">
                </div>
                <label for="CurrentStock" style="width: 150px">
                    <%=Resources.Controls.AllStock%>
                </label>
                <%-- Show the breakup store LinkButton----------------%>
                <asp:LinkButton ID="lnkCurrentStock" runat="server" CssClass="material" OnClientClick="javascript:return ViewBreakupStore(); return false;">
                    <asp:Label ID="CurrentStock" runat="server" EnableViewState="false"></asp:Label>
                </asp:LinkButton>
                <%-- End Show the breakup LinkButton------------%>
                <div class="clear">
                </div>
                <label for="PendingRecievable" style="width: 150px">
                    <%=Resources.Controls.PendingRecievable%>
                </label>
                <%--  pending PO Nos linkbutton--%>
                <asp:LinkButton ID="lnkPendingReceivable" runat='server' OnClientClick="javascript:return ViewPendingPO(); return false;">
                    <asp:Label ID="PendingRecievable" runat="server" EnableViewState="false" CssClass="material"></asp:Label>
                </asp:LinkButton>
                <div class="clear">
                </div>
                <label for="PendingInspection" style="width: 150px">
                    <%=Resources.Controls.PendingInspection%>
                </label>
                <asp:Label ID="PendingInspection" runat="server" Font-Bold="true" EnableViewState="false"></asp:Label>
                <div class="clear">
                </div>
                <%--  Show ROL and Minimum Stock--%>
                -----------------------------------------------------------
                <div class="clear">
                </div>
                <label for="ITM_ROL_STK" style="width: 150px">
                    <%=Resources.Controls.ReorderLevel%>
                </label>
                <asp:Label ID="lblROL" runat="server" Font-Bold="true" EnableViewState="false"></asp:Label>
                <div class="clear">
                </div>
                <div class="clear">
                </div>
                <label for="ITM_MIN_STK" style="width: 150px">
                    <%=Resources.Controls.MinStockLevel%>
                </label>
                <asp:Label ID="lblMinStockLevel" runat="server" Font-Bold="true" EnableViewState="false"></asp:Label>
                <div class="clear">
                </div>
                <%-- For Showing  Last PO Rate with Currency--%>
                -----------------------------------------------------------
                <div class="clear">
                </div>
                <label for="lblLastPoRate" style="width: 150px">
                    <%=Resources.Controls.LastPORate%>
                </label>
                <asp:Label ID="lblLastPoRate" runat="server" Font-Bold="true" EnableViewState="false"></asp:Label>
                <div class="clear">
                </div>
            </div>
        </div>
        <%-- Breakup of store popup div--%>
        <div id="divBreakupStore" title="<%=Resources.Captions.CurrentStockBreakup%>">
            <table rules="all" id="grdBreakupStoreList" grandtype="GrandGrid" paging="false"
                width="100%" class="gridwraptable gridwrap">
                <thead>
                    <tr>
                        <th fieldmap="STD_DEPT" isvisible="false">
                        </th>
                        <%-- <th fieldmap="ROW_NO" align="center" >
                                <%=Resources.Controls.SLNO%>
                            </th>--%>
                        <th fieldmap="DPT_NAME" align="left">
                            <%=Resources.Controls.Store%>
                        </th>
                        <th fieldmap="STD_QTY_IN_STOCK" align="left">
                            <%=Resources.Controls.Stock%>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <%-- end breakup--%>
        <%-- Pending PO popup div--%>
        <div id="divPendingPO" title="<%=Resources.Captions.PendingPurchaseOrders%>" style="display: none">
            <h5>
                <asp:Label ID="lblMatCapion" runat="server"> <%=Resources.Captions.Material%> :</asp:Label>
                <asp:Label ID="lblMaterialNamePendingPO" runat="server"></asp:Label>
            </h5>
            <table rules="all" id="grdPendingPOList" grandtype="GrandGrid" paging="false" editable="false"
                class="gridwraptable gridwrap">
                <thead>
                    <tr>
                        <th fieldmap="POH_PK" isvisible="false">
                        </th>
                        <th fieldmap="POH_NO" align="left">
                            <%=Resources.Controls.PoNumber%>
                        </th>
                        <th fieldmap="POH_DATE" align="left">
                            <%=Resources.Controls.PODate%>
                        </th>
                        <th fieldmap="POH_VENDOR_TEXT" align="left">
                            <%=Resources.Controls.Vendor%>
                        </th>
                        <th fieldmap="POD_REQD_DATE" align="left">
                            <%=Resources.Controls.RequiredByDate%>
                        </th>
                        <th fieldmap="POD_QTY_APPROVED" align="right">
                            <%=Resources.Controls.OrderedQty%>
                        </th>
                        <th fieldmap="POD_QTY_RECEIVED" align="right">
                            <%=Resources.Controls.RcvdQty%>
                        </th>
                        <th fieldmap="POD_QTY_BALANCE" align="right">
                            <%=Resources.Controls.BalanceQuantity%>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <%-- end breakup--%>
        <div id="divItemDetails" title="<%=Resources.Captions.ItemDetails%>">
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S">
                            <label for="ITM_CODE">
                                <%=Resources.Controls.MaterialCode%></label>
                            <asp:TextBox runat="server" ID="ITM_CODE" TabIndex="1" Enabled="false" EnableViewState="false"
                                CssClass="input-half">
                            </asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="ITM_NAME">
                                <%=Resources.Controls.MaterialName%></label>
                            <asp:TextBox runat="server" ID="ITM_NAME" TabIndex="3" Enabled="false" EnableViewState="false"
                                CssClass="input-half">
                            </asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="txtITC_CAT">
                                <%=Resources.Controls.MaterialCategory%></label>
                            <asp:TextBox ID="txtITC_CAT" runat="server" Enabled="false" EnableViewState="false"
                                CssClass="select-small-c1">
                            </asp:TextBox>
                            <label for="txtUom" class="middle-lbl-xsmall-c">
                                <%=Resources.Controls.MaterialUOM%></label>
                            <asp:TextBox ID="txtUom" runat="server" Enabled="false" EnableViewState="false" CssClass="input-small">
                            </asp:TextBox>
                            <div class="clear">
                            </div>
                            <div id="divClass">
                                <label for="txt_TYPE_TEXT">
                                    <%=Resources.Controls.ItemClass%></label><asp:TextBox ID="txt_TYPE_TEXT" Enabled="false"
                                        CssClass="input-half" runat="server" EnableViewState="false"></asp:TextBox>
                                <div class="clear">
                                </div>
                            </div>
                            <label for="txtType">
                                <%=Resources.Controls.Type%></label><asp:TextBox ID="txtType" runat="server" Enabled="false"
                                    CssClass="input-half" EnableViewState="false"></asp:TextBox>
                            <div class="clear">
                            </div>
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S" style="float: right; margin-right: 0">
                            <div id="divMaterialMeasures">
                                <label for="ITM_MIN_STK">
                                    <%=Resources.Controls.MinStockLevel%></label><asp:TextBox runat="server" ID="ITM_MIN_STK"
                                        CssClass="input-small" Enabled="false" TabIndex="2" MaxLength="8" EnableViewState="false"></asp:TextBox>
                                <label for="ITM_ROL_STK" class="middle-lbl-small-a">
                                    <%=Resources.Controls.ReorderLevel%></label><asp:TextBox runat="server" ID="ITM_ROL_STK"
                                        CssClass="input-small" TabIndex="4" Enabled="false" MaxLength="8" EnableViewState="false"></asp:TextBox>
                                <div class="clear">
                                </div>
                                <label for="ITM_MAX_STK">
                                    <%=Resources.Controls.MaxStockLevel%></label><asp:TextBox runat="server" ID="ITM_MAX_STK"
                                        CssClass="input-small" TabIndex="7" Enabled="false" MaxLength="8" EnableViewState="false"></asp:TextBox>
                                <label for="ITM_MAX_STK" class="middle-lbl-small-a">
                                    <%=Resources.Controls.MOQ%></label><asp:TextBox runat="server" ID="ITM_MOQ" TabIndex="9"
                                        CssClass="input-small" Enabled="false" MaxLength="9" EnableViewState="false"></asp:TextBox>
                                <div class="clear">
                                </div>
                            </div>
                            <label for="ITM_MAX_STK">
                                <%=Resources.Controls.Description%></label><asp:TextBox runat="server" ID="ITM_DESC"
                                    CssClass="input-half" TabIndex="11" Enabled="false" EnableViewState="false" EnableTheming="false"
                                    TextMode="MultiLine" Rows="2"></asp:TextBox>
                            <div class="clear">
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <div class="clear">
            </div>
            <%--   Packing Material Master Start--%>
            <div id="divPackingMaterial">
                <h4 class="fields-groupHead">
                    <%=Resources.Controls.Dimensions%></h4>
                <table class="table-devide">
                    <tr class="fields-group-2col">
                        <td>
                            <div class="div2col-S">
                                <label for="IPD_INNER_LENGTH">
                                    <%=Resources.Controls.LengthMM%></label>
                                <asp:TextBox runat="server" ID="IPD_INNER_LENGTH" TabIndex="24" MaxLength="8" CssClass="input-half"
                                    EnableViewState="false">
                                </asp:TextBox>
                                <div class="clear">
                                </div>
                                <div id="divHeight">
                                    <label for="IPD_INNER_HEIGHT">
                                        <%=Resources.Controls.HeightMM%></label><asp:TextBox runat="server" ID="IPD_INNER_HEIGHT"
                                            TabIndex="26" MaxLength="8" CssClass="input-half" EnableViewState="false"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S" style="float: right; margin-right: 0">
                                <label for="IPD_INNER_BREADTH">
                                    <%=Resources.Controls.WidhMM%></label>
                                <asp:TextBox runat="server" ID="IPD_INNER_BREADTH" TabIndex="25" MaxLength="8" CssClass="input-half"
                                    EnableViewState="false">
                                </asp:TextBox>
                                <div class="clear">
                                </div>
                                <label for="ITM_WEIGHT">
                                    <%=Resources.Controls.WeightMM%></label>
                                <asp:TextBox runat="server" ID="ITM_WEIGHT" TabIndex="27" MaxLength="8" CssClass="input-half"
                                    EnableViewState="false">
                                </asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" id="trOuterDimensionHdr">
                            <h4 class="fields-groupHead">
                                <%=Resources.Controls.OuterDimensions%></h4>
                        </td>
                    </tr>
                    <tr id="trOuterDimension" class="fields-group-2col">
                        <td>
                            <div class="div2col-S">
                                <label for="IPD_OUTER_LENGTH">
                                    <%=Resources.Controls.LengthMM%></label>
                                <asp:TextBox runat="server" ID="IPD_OUTER_LENGTH" onblur="CalcCBM();" TabIndex="28"
                                    MaxLength="8" CssClass="input-half" EnableViewState="false">
                                </asp:TextBox>
                                <div class="clear">
                                </div>
                                <label for="IPD_OUTER_HEIGHT">
                                    <%=Resources.Controls.HeightMM%></label>
                                <asp:TextBox runat="server" ID="IPD_OUTER_HEIGHT" onblur="CalcCBM();" TabIndex="30"
                                    MaxLength="8" CssClass="input-half" EnableViewState="false">
                                </asp:TextBox>
                                <div class="clear">
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S" style="float: right; margin-right: 0">
                                <label for="IPD_OUTER_BREADTH">
                                    <%=Resources.Controls.WidhMM%></label>
                                <asp:TextBox runat="server" ID="IPD_OUTER_BREADTH" onblur="CalcCBM();" TabIndex="29"
                                    MaxLength="8" CssClass="input-half" EnableViewState="false">
                                </asp:TextBox>
                                <div class="clear">
                                </div>
                                <label for="ITM_CBM">
                                    <%=Resources.Controls.CBM%></label>
                                <asp:TextBox runat="server" ID="ITM_CBM" ReadOnly="true" MaxLength="8" CssClass="input-half"
                                    TabIndex="31" EnableViewState="false">
                                </asp:TextBox>
                                <div class="clear">
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr class="fields-group-2col">
                        <td>
                            <div class="div2col-S">
                                <label for="IPD_PLY">
                                    <%=Resources.Controls.Ply%></label>
                                <asp:TextBox runat="server" ID="IPD_PLY" TabIndex="32" MaxLength="8" CssClass="input-half"
                                    EnableViewState="false">
                                </asp:TextBox>
                                <div class="clear">
                                </div>
                                <label for="IPD_ART_WORK">
                                    <%=Resources.Controls.ArtWorkVer%></label>
                                <asp:TextBox runat="server" ID="IPD_ART_WORK" CssClass="input-half" TabIndex="34"
                                    EnableViewState="false">
                                </asp:TextBox>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S" style="float: right; margin-right: 0">
                                <label for="IPD_PAPER_COLOR">
                                    <%=Resources.Controls.PaperColor%></label>
                                <asp:TextBox runat="server" ID="IPD_PAPER_COLOR" TabIndex="33" EnableViewState="false"
                                    CssClass="input-half">
                                </asp:TextBox>
                                <div class="clear">
                                </div>
                                <label for="txtAppliedFor">
                                    <%=Resources.Controls.ApplicableFor%></label>
                                <asp:TextBox ID="txtAppliedFor" runat="server" EnableViewState="false" CssClass="input-half"
                                    TabIndex="35">
                                </asp:TextBox>
                                <div class="clear">
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
                <div class="divcol-FileuplWrap" id="divAttachment">
                    <label for="aupDocument">
                        <%=Resources.Controls.Upload%></label>
                    <div id="FileUploader" class="input-file">
                        <asp:FileUpload ID="fupUploader" runat="server" ClientIDMode="Static" size="25" Height="36px"
                            Style="margin-top: 3px" TabIndex="28" />
                        <asp:HiddenField ID="TEMPFILELIST" runat="server" />
                        <asp:HiddenField ID="curTR" runat="server" />
                        <asp:HiddenField ID="hdfCurDate" runat="server" Value="" />
                        <asp:HiddenField ID="hdfIsPacking" runat="server" Value="" />
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
            <%--  Paking Material End--%>
        </div>
        <div id="ItemAlreadyAddedPopUp">
            <div class="content-wrapper">
                <h5>
                    <%=Resources.Captions.SameItemAlreadyExistWithSameDateInPR%>
                </h5>
                <div class="clear">
                </div>
                <div class="search-wrap-c">
                    <label for="RequiredDateNew">
                        <%=Resources.Controls.NewRequestDate%></label>
                    <asp:TextBox runat="server" ID="RequiredDateNew" MaxLength="12" CssClass="date-picker"
                        EnableViewState="false" TabIndex="20">
                    </asp:TextBox>
                    <asp:Button runat="server" ID="AssignNewDate" SkinID="btnInner-ok" Text='<%$ Resources:Controls,Continue %>'
                        EnableViewState="false" OnClientClick="Javascript:return  AddItemWithNewDate();"
                        TabIndex="21" />
                    <asp:Button runat="server" ID="btnCancel" SkinID="btnInner-Cancel" Text='<%$ Resources:Controls,Cancel %>'
                        EnableViewState="false" OnClientClick="Javascript:return  CancelNewDate();" TabIndex="22" />
                </div>
            </div>
        </div>
        <div id="UOMDetailsPopUp" style="display: none">
            <%--<h5>
                <%=Resources.Captions.UOMConversion%>
            </h5>
            <div class="clear">
            </div>--%>
            <div id="divUOMDetails" class="grdTable">
                <div id="div2">
                    <table id="tblUOMDetails" class="gridwraptable gridwrap ">
                        <thead>
                            <tr>
                                <th width="20%" style="text-align: left">
                                    <%=Resources.Controls.Qty%>*
                                </th>
                                <th width="28%" style="text-align: left">
                                    <%=Resources.Controls.DesUOM%>*
                                </th>
                                <th width="4%" style="text-align: center">
                                </th>
                                <th width="28%" style="text-align: left">
                                    <%=Resources.Controls.QtyCalculated%>
                                </th>
                                <th width="20%">
                                    <%=Resources.Controls.StkUOM%>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr class="grd-rowhead">
                                <td>
                                    <asp:TextBox ID="ItemQty" MaxLength="8" runat="server" TabIndex="23" EnableViewState="false"
                                        CssClass="numeric input-w50">
                                    </asp:TextBox>
                                </td>
                                <td>
                                    <asp:DropDownList ID="SelectItemUOM" runat="server" TabIndex="24" CssClass="input-uom"
                                        onChange="GetConversionFactor(null,null)" EnableViewState="false">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:ImageButton ID="CalculateQty" runat="server" SkinID="equal" CssClass="imgbutton-wrap"
                                        TabIndex="25" OnClientClick="javascript:return CalculateUOMQty();" EnableViewState="false" />
                                </td>
                                <td>
                                    <asp:TextBox ID="CalculatedQty" runat="server" Width="90%" TabIndex="26" Enabled="false">
                                    </asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="ItemUOMText" runat="server" Width="90%"></asp:Label>
                                    <asp:HiddenField ID="UOMConversion" runat="server" Value="1" />
                                    <asp:HiddenField ID="DefaultUOM" runat="server" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="button-wrap-right">
                <asp:Button runat="server" ID="UOMQtyCancel" SkinID="btnInner-Cancel" Text='<%$ Resources:Controls,Cancel %>'
                    TabIndex="27" EnableViewState="false" OnClientClick="Javascript:return  CancelUOMQty();" />
                <asp:Button runat="server" ID="UOMQtyDone" SkinID="btnInner-ok" Text='<%$ Resources:Controls,Done %>'
                    TabIndex="28" EnableViewState="false" OnClientClick="Javascript:return  SetUOMQty();" />
            </div>
        </div>
        <%--  Adding new items in material master---------------------%>
        <div id="divAddNewMaterial">
            <div class="Button-container-popup">
                <asp:Button ID="btnSaveMaterial" runat="server" SkinID="btnInner-Save" ToolTip="<%$Resources:Controls,Save%>"
                    Text="<%$Resources:Controls,Save%>" OnClientClick="javascript:return SaveNewMaterials();" />
                <%--<asp:Button ID="btnMaterialCancel" runat="server" SkinID="btnInner-Cancel" ToolTip="<%$resources:ErpRes,Cancel %>"
                            Text="<%$resources:ErpRes,Cancel %>" OnClientClick="javascript:return MaterialCancel();" />--%>
            </div>
            <div id="tabs" runat="server" class="tab-container-floatigNormal">
                <ul class="tab-container-floating">
                    <li><a id="aMaterialTab" href="#Material" onclick="javascript:HideStore();">
                        <%=Resources.Controls.Material%></a></li>
                    <li><a id="aStoreTab" href="#Store" onclick="javascript:ShowStore(this);">
                        <%=Resources.Controls.StoreMapping%></a></li>
                </ul>
                <div id="NewMaterial" class="jquery-tabs-contents">
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <label for="ITM_SET">
                                        <%=Resources.Controls.CategoryType%></label>
                                    <asp:DropDownList ID="ITM_SET" runat="server" TabIndex="5" EnableViewState="False"
                                        CssClass="select-w61per" onchange="javascript:FillCategory();">
                                        <asp:ListItem Value="1" Text="<%$ Resources:BindValues, GeneralRawMaterials%>">
                                        </asp:ListItem>
                                        <asp:ListItem Value="3" Text="<%$ Resources:BindValues, PackingMaterials%>">
                                        </asp:ListItem>
                                        <asp:ListItem Value="4" Text="<%$ Resources:BindValues, Services%>">
                                        </asp:ListItem>
                                        <asp:ListItem Value="8" Text="<%$ Resources:BindValues, Assets%>">
                                        </asp:ListItem>
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                    <label for="ItemCode">
                                        <%=Resources.Controls.MaterialCode%>*</label>
                                    <asp:TextBox runat="server" ID="ItemCode" TabIndex="1" EnableViewState="false" CssClass="input-half">
                                    </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="ItemName">
                                        <%=Resources.Controls.MaterialName%>*</label>
                                    <asp:TextBox runat="server" ID="ItemName" TabIndex="3" EnableViewState="false" CssClass="input-half">
                                    </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="ITC_PK">
                                        <%=Resources.Controls.MaterialCategory%>*</label>
                                    <asp:DropDownList ID="ITC_PK" runat="server" TabIndex="5"  EnableViewState="False"
                                        CssClass="select-w61per">
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                    <label for="UOM_PK" id="lblUOMPK">
                                        <%=Resources.Controls.StockUOM%>*</label>
                                    <asp:DropDownList ID="UOM_PK" runat="server" TabIndex="8" EnableViewState="false"
                                        CssClass="select-small-a" onchange="StockUOMChanged();">
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                    <%-- ****Purchase Uom & Sale UOM ************************************ --%>
                                    <div id="divPurchaseSalesUOM" class="margnlft-minus6">
                                        <label for="ITM_UOM_PURCHASE" id="lblPurchaseUom">
                                            <%=Resources.Controls.PurchaseUOM%>*</label>
                                        <asp:DropDownList ID="ITM_UOM_PURCHASE" runat="server" TabIndex="8" EnableViewState="false"
                                            CssClass="select-small-a">
                                        </asp:DropDownList>
                                        <label for="ITM_UOM_SALE" class="middle-lbl-small-b" id="lblSaleUom">
                                            <%=Resources.Controls.SaleUOM%>*</label>
                                        <asp:DropDownList ID="ITM_UOM_SALE" runat="server" TabIndex="8" EnableViewState="false"
                                            CssClass="select-small-a">
                                        </asp:DropDownList>
                                        <div class="clear">
                                        </div>
                                    </div>
                                    <%--*********End Purchase UOM & Sale UOM***********--%>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <label for="RequireBatch">
                                        <%=Resources.Controls.RequireBatch%></label>
                                    <asp:CheckBox ID="RequireBatch" runat="server" TabIndex="14" EnableViewState="False">
                                    </asp:CheckBox>
                                </div>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    <div class="clear">
                    </div>
                </div>
                <div id="Stores" class="jquery-tabs-contents">
                    <div id="divTree" style="overflow: auto">
                        <div id="treewrap">
                            <div id="trvStores" class="treeview-adj">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%-- End: adding new items in material master-----------------%>
        <asp:HiddenField ID="hdfRequestedBy" runat="server" Value='<%# GetGlobalResourceObject("ConfigurationsRes","RequestedByValidation").ToString()%>'>
        </asp:HiddenField>
        <asp:HiddenField ID="hdfRequestbyDept" runat="server" Value='<%# GetGlobalResourceObject("ConfigurationsRes","RequestbyDeptValidation").ToString()%>'>
        </asp:HiddenField>
        <asp:HiddenField ID="hdfPrTypeRequired" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="PurchaseRequestList" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="SubmitFlag" runat="server" Value="0"></asp:HiddenField>
        <asp:HiddenField ID="PRH_IS_EDIT" runat="server" Value="0" />
        <asp:HiddenField ID="hdfShowLotno" runat="server" Value="0" />
        <%--Comma Separation for Quantity & Amount Based on Configuration(Table)--%>
        <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
        <asp:HiddenField ID="hdfCurrencyGroup2" Value="3" runat="server" />
        <asp:HiddenField ID="hdfIsIOSelected" Value="0" runat="server" />
        <asp:HiddenField ID="hdfShowDescPM" Value="0" runat="server" />
        <asp:HiddenField ID="hdfShowBothLotnoDescPM" Value="0" runat="server" />
        <asp:HiddenField ID="hdfIsPRPMAttachmentShow" Value="0" runat="server" />
        <asp:HiddenField ID="hdfIsDisableIO" Value="0" runat="server" />
        <asp:HiddenField ID="FILELIST" runat="server" />
        <asp:HiddenField ID="hdfIsContSameItemDate" Value="0" runat="server" />
        <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
        <asp:HiddenField ID="hdfIsReqDeptPostback" runat="server" Value="0" />
        <asp:HiddenField ID="hdfReqDeptPk" runat="server" Value="0" />
        <asp:HiddenField ID="hdfStatus" runat="server" Value="0" />
        <asp:HiddenField ID="hdfValidatePurpose" runat="server" Value="0" />
        <asp:HiddenField ID="hdfIsWkfSettingPostBackReqd" runat="server" Value="0" />
        <asp:HiddenField ID="hdfIsSingleReqDeptWkfSetting" runat="server" Value="0" />
        <asp:HiddenField ID="hdfEnableGlovePR" runat="server" Value="0" />
        <asp:HiddenField ID="hdfIsPR_SCinBothSBU" runat="server" Value="0" />
        <asp:HiddenField ID="hdfDataFromSC" runat="server" Value="0" />

        <%--If the Requested Dept ddl contain only one value,Select it by default.In case of multipleplant,we need postback.Infinite loop occurs.For avoiding this we used this hiddenfield --%>
        <asp:HiddenField ID="hdfBackUrl" runat="server" Value=""></asp:HiddenField>
        <asp:HiddenField ID="hdfShowMultipleUOM" runat="server" Value="0" />
        <asp:HiddenField ID="hdfShowPurchaseUOM" runat="server" Value="0" />
        <asp:HiddenField ID="ITM_NEED_BATCH_STK" runat="server" />
        <asp:HiddenField runat="server" ID="StoreDtl" />
        <asp:HiddenField runat="server" ID="ITM_ACTIVE" Value="1" />
        <asp:HiddenField runat="server" ID="hdnAddMaterialsFromPR" Value="0" />
        <asp:HiddenField runat="server" ID="hdfShowIONo" Value="0" />
        <asp:HiddenField runat="server" ID="hdfShowType" Value="0" />
        <asp:HiddenField runat="server" ID="PRH_GROUP" Value="" />
        <asp:HiddenField runat="server" ID="ITM_CATEGORY_VALUE" Value="0" />
        <asp:HiddenField runat="server" ID="HAS_COST_CENTER" Value="0" />
        <asp:HiddenField runat="server" ID="hdfEnableServicePR" Value="0" />
        <asp:HiddenField runat="server" ID="hdfIsPostBack" Value="0" />
        <asp:HiddenField runat="server" ID="PRH_IS_GLOVE" Value="0" />
        <asp:HiddenField runat="server" ID="hdfPRGloveReqInSBU" Value="1" /> 
        <asp:HiddenField runat="server" ID="PRH_IS_COMPLETED" Value="0" /> 
        <asp:HiddenField runat="server" ID="hdfProductAddItemDisable" Value="0" /> 
        <asp:HiddenField runat="server" ID="hdfShowInvestor" Value="0" /> 
        <asp:HiddenField runat="server" ID="hdfCostCenter" Value="0" />
        <asp:HiddenField runat="server" ID="PRH_TYPE" Value="0" />
        <asp:HiddenField runat="server" ID="PRH_SO_BIZUNIT" Value="0" />
         <asp:HiddenField runat="server" ID="hdfIsShowDispatchedSCInPR" Value="0" />
         <asp:HiddenField ID="hdfProjectRequired" runat="server" Value="0" />
        <asp:HiddenField runat="server" ID="hdfIsPRModify" Value="0" />
        <asp:HiddenField runat="server" ID="hdfEditedMatPK" Value="0" />
        <asp:HiddenField runat="server" ID="PRH_PMH_PK" Value="0" />
         <asp:HiddenField runat="server" ID="MPRH_PMH_PK" Value="0" />
         <asp:HiddenField runat="server" ID="hdfMaxOrderQtyEnabled" Value="0" />
        <div class="clear">
        </div>
    </div>
</asp:Content>
