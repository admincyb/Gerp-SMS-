<%@ Page Title="<%$ Resources:Captions,Title_GIN %>" EnableEventValidation="false"
    Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="GINCreate.aspx.cs"
    Inherits="ERPSMS_v01.StoreManagement.GINCreate" Theme="ClassicExt" %>

<%@ Register Src="../WorkFlow/WorkflowComments.ascx" TagName="WorkflowComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/GINCreate.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--    <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.GoodsInspectionNote%>
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton runat="server" ID="imbSave" SkinID="btnsave" TabIndex="11" EnableViewState="False"
                OnClientClick="javascript:return SavePage('Draft');" />
            <asp:ImageButton runat="server" ID="imbReset" SkinID="btnreset" TabIndex="12" EnableViewState="False"
                OnClientClick="javascript:return CancelPage();" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" EnableViewState="False"
                OnClientClick="javascript:return CancelPage();" TabIndex="13" />
            <div id="divFileData">
            </div>
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
                                <asp:Button runat="server" ID="btnSubmit" SkinID="btnInner-submit" Text="<%$Resources:Controls,Submit%>"
                                    TabIndex="34" EnableViewState="False" OnClientClick="javascript:return  WkfSubmit();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    TabIndex="34" EnableViewState="False" OnClientClick="javascript:return SavePage('Draft');" />
                            </li>
                            <%--   <li>
                                <asp:Button runat="server" ID="btnReset" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Reset%>"
                                    TabIndex="35" EnableViewState="False" OnClientClick="javascript:return CancelPage();" />
                            </li>--%>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" OnClientClick="javascript:return CancelPage();" TabIndex="36" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div id="divFileData">
        </div>
        <div class="clear">
        </div>
    </div>
    <asp:HiddenField ID="hdfIsContFutureDate" Value="0" runat="server" />
    <asp:HiddenField ID="hdfCurrentDate" runat="server" />
    <div class="content-wrapper">
        <div id="grdTable-wrap">
            <table class="table-devide"  id="tblDetailHdr">
                <tr>
                    <td>
                        <div class="div2col-S">
                            <label for="GIH_NO">
                                <%=Resources.Controls.GINNo %>
                            </label>
                            <asp:Label ID="GIH_NO" runat="server" CssClass="input-small"></asp:Label>
                            <asp:HiddenField ID="APT_CODE" runat="server" />
                            <asp:HiddenField ID="WKF_FLAG" runat="server" Value="0" />
                             <asp:HiddenField ID="WKF_PROCESS" runat="server" Value="" />
                            <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                            <asp:HiddenField ID="GIH_IS_EDIT" runat="server" Value="0" />
                            <label for="GIH_DATE" class="middle-lbl-small-b">
                                <%=Resources.Controls.GINDate%>
                            </label>
                            <asp:TextBox ID="GIH_DATE" onkeydown="return CheckKey(event)" onpaste="return false;"
                                runat="server" TabIndex="1" CssClass="input-small-a" MaxLength="12"></asp:TextBox>

                            <label for="GRH_DEPT_STORE">
                                <%=Resources.Controls.GRNStore %>
                            </label>
                            <asp:DropDownList ID="GRH_DEPT_STORE" runat="server" TabIndex="2" CssClass="select-half" onchange="javascript:BindGRNItemDetails(0);">
                            </asp:DropDownList>

                        </div>
                    </td>
                    <td>
                        <div class="div2col-S">

                            <label for="ddlGihDept">
                                <%=Resources.Controls.InspectionStore%>
                            </label>
                            <asp:DropDownList ID="ddlGihDept" runat="server" TabIndex="3" CssClass="select-half" EnableViewState="true">
                            </asp:DropDownList>
                            <asp:HiddenField ID="GIH_DEPT" runat="server" Value="0" />
                            <%--  <asp:HiddenField ID="GIH_DEPT" runat="server" Value="0" />--%>
                            <div id="divPlantCompany">
                            <label for="GIH_COMPANY">
                                <%=GetGlobalResourceObject("Controls","CompanyPlant")%>*</label>
                            <asp:DropDownList ID="GIH_COMPANY" runat="server" TabIndex="4" CssClass="select-half margnlft-minus4" ClientIDMode="Static">
                            </asp:DropDownList>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <div class="div3col-S">
            </div>
            <div class="clear">
            </div>
            <h1 class="search-colapse-normal">
                <%=Resources.Captions.ItemsPendingInspection%>
                <img id="imgPendingInspShow" src="../Images/Classic/Icons/arrow-colapse-active.png"
                    alt="<%= Resources.Controls.Show%>" title="<%= Resources.Controls.Show%>" style="display: none;
                    cursor: pointer" onclick="javascript:ShowPendingInspections();" />
                <img id="imgPendingInspHide" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                    alt="<%= Resources.Controls.Hide%>" title="<%= Resources.Controls.Hide%>" style="cursor: pointer"
                    onclick="javascript:HidePendingInspections();" />
            </h1>
            <div id="divPendingIspections">
                <div id="searchwrap">
                    <div id="Div2" class="search-wrap-custom1">
                        <label>
                            <%=Resources.Controls.SearchBy%></label>
                        <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" onchange="javascript:SetSearchType(true);"
                            EnableViewState="false" TabIndex="5">
                            <asp:ListItem Value="GRH_NO" Text="GRN #">
                            </asp:ListItem>
                            <asp:ListItem Value="ITM_NAME" Text="Material Name">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <div id="divSearchDtls">
                            <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false" Width="200"
                                TabIndex="6">
                            </asp:TextBox>
                        </div>
                        <div id="divDate">
                            <label for="FromDate">
                                <%=Resources.Controls.FromDate%></label>
                            <asp:TextBox ID="FromDate" runat="server" EnableViewState="false" TabIndex="7">
                            </asp:TextBox>
                            <asp:HiddenField ID="hdfFrmDate" runat="server" />
                            <label for="ToDate">
                                <%=Resources.Controls.ToDate%></label>
                            <asp:TextBox ID="ToDate" runat="server" TabIndex="8" EnableViewState="false">
                            </asp:TextBox>
                            <asp:HiddenField ID="hdfToDate" runat="server" />
                        </div>
                        <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" TabIndex="9" OnClientClick="javascript:return BindGrid();"
                            EnableViewState="false" />
                        <asp:ImageButton runat="server" ID="imgbtnclear" SkinID="btnrefresh" Visible="false" TabIndex="9"
                            OnClientClick="javascript:return ClearGRNSearchDetails()" Width="16px" />
                        <div class="clear">
                        </div>
                    </div>
                </div>
                <div id="divPendingInspection" class="scroll-h150">
                    <div id="divPendingGRNList" class="gridwrap nomargin">
                        <table id="grdGRNList" width="100%" enablecheckbox="true" paging="false" grandtype="GrandGrid"
                            rules="all" class="gridwraptable gridwrap">
                            <thead>
                                <tr>
                                    <th fieldmap="GRH_PK" isvisible="false">
                                    </th>
                                    <th fieldmap="GRD_PK" isvisible="false">
                                    </th>
                                    <th fieldmap="GRD_ITEM" isvisible="false">
                                    </th>
                                    <th fieldmap="GRD_UOM" isvisible="false">
                                    </th>
                                    <th fieldmap="POD_RATE" isvisible="false">
                                    </th>
                                    <th fieldmap="GRD_QTY_INSPECTED_LAST" isvisible="false">
                                    </th>
                                    <th fieldmap="GRH_DATE" isvisible="false">                                      
                                    </th>
                                    <th fieldmap="GRD_GIN_FLAG" align="left" width="8px">
                                        <%-- <%=Resources.Controls.Added%>--%>
                                    </th>
                                    <th fieldmap="GRH_NO" align="left" width="50px">
                                        <%=Resources.Controls.GRNNo %>
                                    </th>
                                    <th fieldmap="POH_NO" align="left" width="100px">
                                        <%=Resources.Controls.PONo %>
                                    </th>
                                    <th fieldmap="ITM_NAME" align="left" width="220px">
                                        <%=Resources.Controls.Item%>
                                    </th>
                                    <th fieldmap="UOM_CODE" align="left" width="10px">
                                        <%=Resources.Controls.UOM%>
                                    </th>
                                    <th fieldmap="GRD_QTY_APPROVED" align="right" width="70px">
                                        <%=Resources.Controls.RcvdQty%>
                                    </th>
                                    
                                    <th fieldmap="GRD_QTY_INSPECTED" align="right" width="90px">
                                        <%=Resources.Controls.InspQty%>
                                    </th>
                                    <th fieldmap="GRD_QTY_ACCEPTED" align="right" width="70px">
                                        <%=Resources.Controls.AccQty%>
                                    </th>
                                    <th fieldmap="GRD_QTY_REJECTED" align="right" width="70px">
                                        <%=Resources.Controls.RejQty%>
                                    </th>
                                    <th fieldmap="BALANCE_QTY" align="right" width="90px">
                                        <%=Resources.Controls.BalToInsp%>
                                    </th>
                                     <th fieldmap="GRD_QA_LOT_NO" align="left" width="75px">
                                        <%=Resources.Controls.QaLotNumber%>
                                    </th>
                                    <th fieldmap="GRD_DOM" width="80px">
                                        <%=Resources.Controls.DOM%>
                                    </th>
                                    <th fieldmap="GRD_DOE" width="80px">
                                        <%=Resources.Controls.DOE%>
                                    </th>
                                      <th fieldmap="GRH_COMPANY" isvisible="false">
                                      </th> 
                                      <th fieldmap="ITM_NEED_QC_INSP" isvisible="false">
                                    
                                      </th> 
                             
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
                <div class="button-wrap-right">
                    <asp:Button ID="AddToList" runat="server" Text="Add To List" TabIndex="10" OnClientClick="javascript:return AddToList();" />
                </div>
                <div class="clear">
                </div>
            </div>
            <h1 class="search-colapse-normal">
                <%=Resources.Captions.InspectionList%>
                <img id="imgGINIShow" src="../Images/Classic/Icons/arrow-colapse-active.png" alt="<%= Resources.Controls.Show%>"
                    title="<%= Resources.Controls.Show%>" style="display: none; cursor: pointer"
                    onclick="javascript:ShowGINILst();" />
                <img id="imgGINIHide" src="../Images/Classic/Icons/arrow-colapse-inactive.png" alt="<%= Resources.Controls.Hide%>"
                    title="<%= Resources.Controls.Hide%>" style="cursor: pointer" onclick="javascript:HideGINILst();" />
            </h1>
            <div id="divGINItems">
                <div id="divPurchaseRequestInsert" class="grdTable scroll-h150" style="overflow-x: auto">
                    <table id="grdGRNItemList" width="100" rules="all" paging="false" grandtype="GrandGrid"
                        rules="all" editable="true" editfunction="GridAction" class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th fieldmap="GID_PK" isvisible="false">
                                </th>
                                <th fieldmap="GID_GI" isvisible="false">
                                </th>
                                <th fieldmap="GID_SL_NO" isvisible="false">
                                </th>
                                <th fieldmap="GID_ITEM" isvisible="false">
                                </th>
                                <th fieldmap="GID_UOM" isvisible="false">
                                </th>
                                <th fieldmap="GRH_PK" isvisible="false">
                                </th>
                                <th fieldmap="GRD_PK" isvisible="false">
                                </th>
                                <th width="30px" fieldmap="GRN_NO" style="text-align: left">
                                    <%=Resources.Controls.GRNNo %>
                                </th>
                                <th width="150px" fieldmap="POH_NO" style="text-align: left">
                                    <%=Resources.Controls.PONo%>
                                </th>
                                <th width="290px" fieldmap="GID_ITEM_NAME" style="text-align: left">
                                    <%=Resources.Controls.Item%>
                                </th>
                                <th width="10px" fieldmap="UOM_CODE" style="text-align: left">
                                    <%=Resources.Controls.UOM%>
                                </th>
                                <th fieldmap="GRD_QTY_APPROVED" width="50px" align="right">
                                    <%=Resources.Controls.RcvdQty%>
                                </th>
                                <th width="50px" fieldmap="GRD_QTY_INSPECTED" style="text-align: left">
                                    <%=Resources.Controls.InspQty%>
                                </th>
                                <th width="50px" fieldmap="GRD_QTY_ACCEPTED" align="right">
                                    <%=Resources.Controls.AccQty%>
                                </th>
                                <th width="50px" fieldmap="GRD_QTY_REJECTED" style="text-align: right">
                                    <%=Resources.Controls.RejQty%>
                                </th>
                                <th width="50px" fieldmap="GRD_QTY_BALANCE" style="text-align: left">
                                    <%=Resources.Controls.BalToInsp%>
                                </th>
                                <th width="100px" fieldmap="GID_QTY_INSPECTED" style="text-align: left">
                                    <%=Resources.Controls.InspNow%>
                                </th>
                                <th width="50px" fieldmap="GID_QTY_ACCEPTED" style="text-align: left">
                                    <%=Resources.Controls.AccNow%>
                                </th>
                                <th width="50px" fieldmap="GID_QTY_REJECTED" style="text-align: left">
                                    <%=Resources.Controls.RejNow%>
                                </th>
                                <th width="50px" fieldmap="GID_REJ_DTLS" style="text-align: left">
                                    <%=Resources.Controls.RejDetails%>
                                </th>
                                <th width="90px" fieldmap="GID_QA_LOT_NO" style="text-align: left">
                                    <%=Resources.Controls.QaLotNumber%>
                                </th>
                                <th width="90px" fieldmap="GID_REMARKS" style="text-align: left">
                                    <%=Resources.Controls.Remarks%>
                                </th>
                                <th type="Template" width="10px">
                                    <div>
                                        <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$ resources:Controls,Delete %>" TabIndex="10"
                                            SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'delete')" />
                                    </div>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
            </div>
            <div class="clear">
            </div>
            <h1 class="search-colapse-normal">
                <%=Resources.Captions.Attachments%>
                <img id="imgAttachmentsShow" src="../Images/Classic/Icons/arrow-colapse-active.png"
                    alt="<%= Resources.Controls.Show%>" title="<%= Resources.Controls.Show%>" style="display: none;
                    cursor: pointer" onclick="javascript:ShowAttachments();" />
                <img id="imgAttachmentsHide" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                    alt="<%= Resources.Controls.Hide%>" title="<%= Resources.Controls.Hide%>" style="cursor: pointer"
                    onclick="javascript:HideAttachments();" />
            </h1>
            <div id="divAttachments">
                <div class="divcol-FileuplWrap">
                    <label for="aupDocument">
                        <%=Resources.Controls.Attachments%></label>
                    <div id="FileUploader" class="input-file">
                        <asp:FileUpload ID="fupUploader" runat="server" ClientIDMode="Static" size="24" Height="22px"
                            TabIndex="10" />
                        <asp:HiddenField ID="FILELIST" runat="server" />
                    </div>
                    <div class="clear">
                    </div>
                </div>
            </div>
            <%--##### START Remove Old Workflow Section &   Add user Controls and process Id, app ID#####--%>
            <div id="Wofkflowdiv" style="display: none">
                <asp:HiddenField runat="server" ID="ViewStatus" Value="0" />
                <uc1:WorkflowComments ID="ucrWrkf" runat="server" ValidationGroup="Save" />
                <asp:HiddenField ID="hdfProcessID" runat="server" Value="0" />
                <asp:HiddenField ID="hdfAppID" runat="server" Value="0" />
                <asp:HiddenField runat="server" ID="ActionID" Value="0" />
                <asp:HiddenField ID="AppNo" runat="server" Value="0" />
                <asp:HiddenField ID="DoeDifrDays" runat="server" Value="0" />
                <asp:HiddenField ID="IsReqDoeValidation" runat="server" Value="0" />
                <asp:HiddenField ID="SubmitFlag" runat="server" Value="0"></asp:HiddenField>
                <asp:HiddenField ID="hdfDeptID" runat="server" Value="0" />
                 <asp:HiddenField ID="hdfTransactionData" runat="server" Value="0" />
                <asp:HiddenField ID="hdfGrnStore" runat="server" Value="0" />
                <asp:HiddenField ID="hdfCompany" runat="server" Value="0" />
                <asp:HiddenField ID="hdfSBUCompany" runat="server" Value="0" />
                <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
            </div>
            <%--#####END Add user Controls and process Id, app ID#####--%>
            <div class="clear">
            </div>
            <div id="divGRNDetails" class="grdTable">
                <div class="content-wrapper">
                    <table rules="all" id="grdGRNDetails" grandtype="GrandGrid" paging="false" class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th fieldmap="GRH_NO" align="left" width="12%">
                                    <%=Resources.Controls.GRNNo %>
                                </th>
                                <th fieldmap="GRN_DATE" align="left" width="12%">
                                    <%=Resources.Controls.GRNDate%>
                                </th>
                                <th fieldmap="POH_NO" align="left" width="15%">
                                    <%=Resources.Controls.PoNumber%>
                                </th>
                                <th fieldmap="POH_DATE" align="left" width="12%">
                                    <%=Resources.Controls.PODate%>
                                </th>
                                <th fieldmap="DPT_NAME" align="left" width="12%">
                                    <%=Resources.Controls.Dept%>
                                </th>
                                <th fieldmap="VEN_CONT_NAME" align="left" width="15%">
                                    <%=Resources.Controls.Vendor%>
                                </th>
                                <th fieldmap="ITM_NAME" align="left" width="15%">
                                    <%=Resources.Controls.Item%>
                                </th>
                                <th fieldmap="GRD_QTY_APPROVED" align="right" width="10%">
                                    <%=Resources.Controls.Quantity%>
                                </th>                               
                            </tr>
                        </thead>
                    </table>
                </div>
            </div>
            <div id="divGINInspectedDetails" class="grdTable">
                <table rules="all" id="grdGINInspDtls" grandtype="GrandGrid" paging="false" class="gridwraptable gridwrap">
                    <thead>
                        <tr>
                            <th fieldmap="GRH_NO" align="left" width="15%">
                                <%=Resources.Controls.GRNNo%>
                            </th>
                            <th fieldmap="GIH_NO" align="left" width="15%">
                                <%=Resources.Controls.GINNo%>
                            </th>
                            <th fieldmap="GIH_DATE" align="left" width="12%">
                                <%=Resources.Controls.InspectedDate%>
                            </th>
                            <th fieldmap="GIH_DEPT_TEXT" align="left" width="10%">
                                <%=Resources.Controls.Store%>
                            </th>
                            <th fieldmap="GID_ITEM_TEXT" align="left" width="15%">
                                <%=Resources.Controls.Item%>
                            </th>
                            <th fieldmap="GID_UOM_TEXT" align="left" width="8%">
                                <%=Resources.Controls.UOM%>
                            </th>
                            <th fieldmap="GID_QTY_INSPECTED" align="right" width="13%">
                                <%=Resources.Controls.InspectedQty%>
                            </th>
                            <th fieldmap="GID_QTY_REJECTED" align="right" width="13%">
                                <%=Resources.Controls.RejectedQty%>
                            </th>
                            <th fieldmap="GID_QTY_TO_APPROVE" align="right" width="13%">
                                <%=Resources.Controls.QtyApprove%>
                            </th>                        
                        </tr>
                    </thead>
                </table>
            </div>
            <div id="DivRejectedDtls">
                <div class="Button-container-popup" id="btnContainer">
                    <asp:Button runat="server" ID="btnDmgClear" SkinID="btnInner-Cancel" Text="Clear"
                        OnClientClick="javascript:return ClearDamageDetails()" />
                    <asp:Button runat="server" ID="btnSaveDamage" SkinID="btnInner-Save" Text="Save"
                        OnClientClick="javascript:return AddDamageDetails()" />
                </div>
                <div id="damagedetailsDiv" class="content-wrapper">
                    <div id="addDamageDetails">
                        <div class="divcol-P1">
                            <label for="ItemName">
                                <%=Resources.Controls.Item%></label><asp:Label runat="server" ID="ItemName" Text=""></asp:Label>
                            <label for="QtyRejected">
                                <%=Resources.Controls.TotalRejectedQty%></label><asp:Label runat="server" ID="QtyRejected"
                                    Text=""></asp:Label>
                            <label for="GDD_DMG_QTY">
                                <%=Resources.Controls.Quantity%>*</label><asp:TextBox ID="GDD_DMG_QTY" runat="server"></asp:TextBox>
                            <label for="GDD_DMG_TYPE">
                                <%=Resources.Controls.RejectionReason%>*</label><asp:DropDownList ID="GDD_DMG_TYPE"
                                    runat="server">
                                </asp:DropDownList>
                            <div class="clear">
                            </div>
                            <label for="GDD_DEPT_STORE">
                                <%=Resources.Controls.RejectTo%>*</label><asp:DropDownList ID="GDD_DEPT_STORE" runat="server">
                                </asp:DropDownList>
                        </div>
                    </div>
                    <%-- <div class="clear">
                    </div>--%>
                    <asp:HiddenField ID="GRNPK" runat="server" Value="0" />
                    <asp:HiddenField ID="GRH_PK" runat="server" Value="0" />
                    <asp:HiddenField ID="ItemPk" runat="server" Value="0" />
                    <asp:HiddenField ID="GDD_GRN_DTL" runat="server" Value="0" />
                    <asp:HiddenField ID="RejQty" runat="server" Value="0" />
                    <asp:HiddenField ID="EditMode" runat="server" Value="0" />
                    <asp:HiddenField ID="EditQty" runat="server" Value="0" />
                    <asp:HiddenField ID="EditReasonType" runat="server" Value="0" />
                    <asp:HiddenField ID="EditStore" runat="server" Value="0" />
                    <div class="gridwrap max-250">
                        <table rules="all" id="grdDamageDtls" grandtype="GrandGrid" pagesize="5" paging="true"
                            editfunction="GridAction" width="100%" editable="true" class="gridwraptable gridwrap">
                            <thead>
                                <tr>
                                    <th fieldmap="GDD_GRN" isvisible="false">
                                    </th>
                                    <th fieldmap="GDD_ITEM" isvisible="false">
                                    </th>
                                    <th fieldmap="GDD_DMG_TYPE" isvisible="false">
                                    </th>
                                    <th fieldmap="GDD_DEPT_STORE" isvisible="false">
                                    </th>
                                    <th fieldmap="GDD_ITEM_NAME" sortable="true" align="left" width="35%">
                                        <%=Resources.Controls.Item%>
                                    </th>
                                    <th fieldmap="GDD_DMG_QTY" sortable="true" align="left" width="15%">
                                        <%=Resources.Controls.Quantity%>
                                    </th>
                                    <th fieldmap="GDD_DMG_TYPE_TEXT" sortable="true" align="left" width="25%">
                                        <%=Resources.Controls.Reason%>
                                    </th>
                                    <th fieldmap="GDD_DEPT_STORE_NAME" sortable="true" align="left">
                                        <%=Resources.Controls.Store%>
                                    </th>                                    
                                    <th type="Template" align="center" width="10%">
                                        <div style="text-align: center">
                                            <asp:ImageButton runat="server" ID="imbLocEdit" SkinID="imbeditgrid" TabIndex="10" OnClientClick="javascript:return DamageGridHandler($(this).parents('tr:eq(0)'),'EDIT')" />
                                            <asp:ImageButton runat="server" ID="imbLocDel" SkinID="imbdeletegrid" TabIndex="10" OnClientClick="javascript:return DamageGridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                        </div>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
            <asp:HiddenField ID="GIH_PK" runat="server" Value="0" />
             <asp:HiddenField ID="IsPrefID" runat="server" Value="0" />
            <asp:HiddenField ID="GID_PK" runat="server" Value="0" />
            <asp:HiddenField ID="GIH_STATUS" runat="server" Value="0" />
            <asp:HiddenField ID="GINList" runat="server" />
             <asp:HiddenField ID="hdfType" runat="server" Value="1" />
             <asp:HiddenField ID="hdfRefID" runat="server" Value="0" />
             <asp:HiddenField ID="hdfIsGoToInbox" runat="server" Value="0" />
             <%--Comma Separation for Quantity & Amount Based on Configuration(Table)--%>
           <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
            <asp:HiddenField ID="hdfCurrencyGroup2" Value="3" runat="server" />
            <div id="divData">
            </div>
            <div class="clear">
            </div>
        </div>
    </div>
</asp:Content>
