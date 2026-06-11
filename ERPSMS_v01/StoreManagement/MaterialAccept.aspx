<%@ Page Title="<%$ Resources:Captions,Title_StockAccept %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="MaterialAccept.aspx.cs" EnableEventValidation="false"
    Inherits="ERPSMS_v01.StoreManagement.MaterialAccept" Theme="ClassicExt" %>

<%@ Register Src="../WorkFlow/WorkflowComments.ascx" TagName="WorkflowComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/MaterialAccept.js.axd" type="text/javascript"></script>
        <script src="../Scripts/JSLINQ/JSLINQ.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--   <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.MaterialAccept%>
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton runat="server" ID="imbSave" SkinID="btnsave" TabIndex="7" EnableViewState="False"
                OnClientClick="javascript:return SavePage('Draft');" />
            <asp:ImageButton runat="server" ID="imbReset" SkinID="btnreset" TabIndex="8" EnableViewState="False"
                OnClientClick="javascript:return ResetPage();" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" EnableViewState="False"
                OnClientClick="javascript:return CancelFun();" TabIndex="9" />
        </div>
    </div>
    <div class="clear">
    </div>--%>
    <asp:HiddenField ID="ConfirmStockValueChange" runat="server" Value="0" />
    <asp:HiddenField ID="hdfMIPk" runat="server" Value="0" />
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
                            <%-- <li>
                                <asp:Button runat="server" ID="btnReset" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Reset%>"
                                    TabIndex="35" EnableViewState="False" OnClientClick="javascript:return ResetPage();" />
                            </li>--%>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" OnClientClick="javascript:return  CancelPage();" TabIndex="36" />
                            </li>
                             <li>
                                <asp:Button runat="server" ID="btnPrint" SkinID="btnInner-Print" Text="<%$Resources:Controls,Print%>"
                                    TabIndex="37" EnableViewState="False" OnClientClick="javascript:return PrintPage();" /></li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div class="clear">
         <asp:HiddenField ID="hdfAppType" runat="server" />
        <asp:HiddenField ID="hdfAppSubType" runat="server" />
        </div>
    </div>
    <div id="Wofkflowdiv" style="display: none">
        <asp:HiddenField runat="server" ID="ActionID" />
        <uc1:WorkflowComments ID="ucrWrkf" runat="server" ValidationGroup="Save" />
        <asp:HiddenField ID="hdfProcessID" runat="server" Value="0" />
        <asp:HiddenField ID="hdfDeptID" runat="server" Value="0" />
        <asp:HiddenField ID="hdfSlNo" runat="server" Value="0" />
        <asp:HiddenField ID="hdfAppID" runat="server" Value="0" />
        <asp:HiddenField ID="AppNo" runat="server" />
        <asp:HiddenField ID="hdfAcceptStore" runat="server" />
        <asp:HiddenField ID="hdfIsContFutureDate" Value="0" runat="server" />
        <asp:HiddenField ID="hdfCurrentDate" runat="server" />
        <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
    </div>
    <div class="content-wrapper">
        <%-- <h1>
            <%=Resources.Captions.MaterialReciepts%></h1>--%>
        <table class="table-devide">
            <tr>
                <td>
                    <div class="div2col-S">
                        <label for="MAH_NO">
                            <%=Resources.Controls.MaterialAcceptNo%>
                        </label>
                        <asp:Label ID="MAH_NO" runat="server" Text="" EnableViewState="false" CssClass="input-small-28-10"></asp:Label>
                        <label for="MA_DATE" class="middle-lbl">
                            <%=Resources.Controls.MaterialAcceptDate%>
                        </label>                        
                        <asp:TextBox ID="MAH_DATE" runat="server" TabIndex="1" onkeydown="return CheckKey(event)"
                            onpaste="return false;" CssClass="input-small"></asp:TextBox>
                        <asp:HiddenField ID="MAH_PK" runat="server" EnableViewState="false" Value="0" />
                        <asp:HiddenField ID="MAH_STATUS" runat="server" EnableViewState="false" Value="0" />
                        <asp:HiddenField ID="MAH_IS_EDIT" runat="server" Value="0" />
                        <div class="clear">
                        </div>
                        <%--<div id="divSBUCompany">
                        <label for="MAH_COMPANY">
                            <%=Resources.Controls.Company%>*</label>--%>
                            <div id="divSBUCompany">
                        <label for="MAH_COMPANY">
                          <%= GetGlobalResourceObject("Controls", "CompanyPlant").ToString()%>*</label>
                        <asp:DropDownList ID="MAH_COMPANY" runat="server" TabIndex="3" ClientIDMode="Static" CssClass="select-half">
                        </asp:DropDownList>
                        <asp:HiddenField ID="hdfSelCompany" runat="server" />
                         <asp:HiddenField ID="hdfCompany" runat="server" />
                        </div>
                    </div>
                </td>
                <td>
                    <div class="div2col-S">
                        <label for="MAH_DEPT">
                            <%=Resources.Controls.Department%>
                            *</label>
                        <asp:DropDownList ID="MAH_DEPT" runat="server" TabIndex="2" onchange="javascript:AddNew();" CssClass="select-half">
                        </asp:DropDownList>

                    </div>
                </td>
            </tr>
        </table>
        <div class="clear">
        </div>
        <h1 class="search-colapse-normal">
            <%=Resources.Captions.StoreIssue %></h1>
        <div id="searchwrap" class="search-wrap-custom1">
            <div id="divSearch">
                <label>
                    <%=Resources.Controls.SearchBy%></label>
                <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" onchange="javascript:SetSearchType(true);"
                    TabIndex="4" EnableViewState="false">
                    <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                    </asp:ListItem>
                    <asp:ListItem Value="MIH_NO" Text="<%$ Resources:BindValues, STNo%>">
                    </asp:ListItem>
                    <asp:ListItem Value="ITM_NAME" Text="<%$ Resources:BindValues, Material%>">
                    </asp:ListItem>
                    <asp:ListItem Value="Date" Text="Date Range">
                    </asp:ListItem>
                </asp:DropDownList>
                <div id="divSearchDtls">
                    <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false" TabIndex="5">
                    </asp:TextBox>
                </div>
                <div id="divDate">
                    <label for="FromDate">
                        <%=Resources.Controls.FromDate%></label>
                    <asp:TextBox ID="FromDate" runat="server" TabIndex="6" EnableViewState="false">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfFrmDate" runat="server" />
                    <label for="ToDate">
                        <%=Resources.Controls.ToDate%></label>
                    <asp:TextBox ID="ToDate" runat="server" TabIndex="7" EnableViewState="false">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfToDate" runat="server" />
                </div>
                <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" TabIndex="8" OnClientClick="javascript:return BindGrid();"
                    EnableViewState="false" />
            </div>
            <div class="clear">
            </div>
        </div>
        <div class="scroll-h150" style="height: 250px">
            <div id="divPendingPOList" class="grdTable">
                <h1 class="search-colapse-normal">
                    <%=Resources.Captions.MaterialDetails%></h1>
                <table rules="all" id="grdSIList" grandtype="GrandGrid" paging="false" enablecheckbox="true"
                    width="100%" class="gridwraptable gridwrap">
                    <thead>
                        <tr>
                            <th fieldmap="MRH_PK" isvisible="false">
                            </th>
                            <th fieldmap="MIH_PK" isvisible="false">
                            </th>
                            <th fieldmap="MAD_STK_BATCH" isvisible="false">
                            </th>
                            <th fieldmap="MID_ITEM" isvisible="false">
                            </th>
                            <th fieldmap="MID_UOM" isvisible="false">
                            </th>
                             <th fieldmap="MID_PK" isvisible="false">
                            </th>  
                             <th fieldmap="ITM_CODE" isvisible="false">
                            </th>  
                            <th fieldmap="MRD_REQUEST_NO" align="left" width="9%">
                                <%=Resources.Controls.SRSNo%>
                            </th>
                            <th fieldmap="MIH_NO" align="left" width="10%">
                                <%=Resources.Controls.STNo%>
                            </th>
                            <th fieldmap="ITM_NAME" align="left" width="22%">
                                <%=Resources.Controls.Item%>
                            </th>
                            <th fieldmap="MAD_STK_BATCH_NO" align="left" width="12%">
                                <%=Resources.Controls.BatchNo%>
                            </th>
                            <th fieldmap="MRD_QTY_APPROVED" align="right" width="13%">
                                <%=Resources.Controls.ReqQty%>
                            </th>
                            <th fieldmap="MID_QTY_ISSUED" align="right" width="10%">
                                <%=Resources.Controls.QtyIssued%>
                            </th>
                            <th fieldmap="MID_QTY_RECEIVED" align="right" width="12%">
                                <%=Resources.Controls.QtyAccepted%>
                            </th>
                            <th fieldmap="BALANCE_QTY" align="right" width="7%">
                                <%=Resources.Controls.Balance%>
                            </th>
                            <th fieldmap="UOM_CODE" align="left" width="5%">
                                <%=Resources.Controls.UOM%>
                            </th>
                        </tr>
                    </thead>
                </table>
                <div class="button-wrap-right">
                    <asp:Button ID="AddToList" runat="server" Text="Add To List" TabIndex="8" OnClientClick="javascript:return AddToList();" />
                </div>
            </div>
        </div>
        <h1 class="search-colapse-normal">
            <%=Resources.Captions.PendingStoreIssue%></h1>
        <div class="gridwrap">
            <div id="divPendingPOInsert">
                <table rules="all" id="grdPendingSIList" grandtype="GrandGrid" paging="false" editfunction="GridAction"
                    editable="true" class="gridwraptable gridwrap">
                    <thead>
                        <tr>
                            <th fieldmap="MAD_PK" isvisible="false">
                            </th>
                            <th fieldmap="MRH_PK" isvisible="false">
                            </th>
                            <th fieldmap="MAD_STK_BATCH" isvisible="false">
                            </th>
                            <th fieldmap="MAD_MI" isvisible="false">
                            </th>
                            <th fieldmap="MAD_ITEM" isvisible="false">
                            </th>
                            <th fieldmap="MAD_UOM" isvisible="false">
                            </th>
                            <th fieldmap="MAD_SL_NO" isvisible="false">
                            </th>
                            <th fieldmap="MIH_NO" align="left" width="9%">
                                <%=Resources.Controls.STNo%>
                            </th>
                            <th fieldmap="ITM_NAME" align="left" width="24%">
                                <%=Resources.Controls.Item%>
                            </th>
                            <th fieldmap="MAD_STK_BATCH_NO" align="left" width="12%">
                                <%=Resources.Controls.BatchNo%>
                            </th>
                            <th fieldmap="MID_QTY_ISSUED" align="right" width="10%">
                                <%=Resources.Controls.QtyIssued%>
                            </th>
                            <th fieldmap="MID_QTY_RECEIVED" isvisible="false">
                            </th>
                            <th fieldmap="MAD_QTY_ACCEPTED" align="right" width="10%">
                                <%=Resources.Controls.QtyAccept%>
                            </th>
                            <th fieldmap="MAD_QTY_LOST" align="right" width="12%">
                                <%=Resources.Controls.LoseInTransit%>
                            </th>
                            <th fieldmap="UOM_CODE" align="left" width="5%">
                                <%=Resources.Controls.UOM%>
                            </th>
                            <th fieldmap="MAD_REMARKS" align="left" width="15%">
                                <%=Resources.Controls.Remarks%>
                            </th>
                            <th type="Template" width="5%">
                                <div>
                                    <asp:ImageButton runat="server" ID="imbDelete" TabIndex="8" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                </div>
                            </th>
                        </tr>
                    </thead>
                </table>
            </div>
            <br class="clear" />
        </div>
        <div class="clear">
        </div>
        <div class="button-wrap">
            <asp:Button ID="btnSaveandSubmit" Visible="false" runat="server" CssClass="inputbtn" TabIndex="8"
                Text="<%$Resources:Controls,Saveandsubmit%>" Style="width: 140px" OnClientClick="javascript:return SavePage();" />
        </div>
        <div id="divSRSDetails" class="grdTable" title="<%=Resources.Captions.SrsDetails%>">
            <table rules="all" id="grdSRSDetails" grandtype="GrandGrid" paging="false" class="gridwraptable gridwrap">
                <thead>
                    <tr>
                        <th fieldmap="MRH_NO" align="left" width="15%">
                            <%=Resources.Controls.SRS%>
                        </th>
                        <th fieldmap="MRH_DATE" align="left" width="15%">
                            <%=Resources.Controls.SRSDate%>
                        </th>
                        <th fieldmap="MRH_DEPT_STR" align="left" width="15%">
                            <%=Resources.Controls.RequestStore%>
                        </th>
                        <th fieldmap="MRH_DEPT" align="left" width="15%">
                            <%=Resources.Controls.Department%>
                        </th>
                        <th fieldmap="ITM_NAME" align="left" width="15%">
                            <%=Resources.Controls.MaterialName%>
                        </th>
                        <th fieldmap="MRD_QTY_APPROVED" align="right" width="15%">
                            <%=Resources.Controls.RequestQuantity%>
                        </th>
                        <th fieldmap="UOM_CODE" align="left" width="8%">
                            <%=Resources.Controls.UOMCode%>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <div id="divPreviousMA" class="grdTable" title="<%=Resources.Captions.PreviousMADetails%>">
            <table rules="all" id="grdPreviousMA" grandtype="GrandGrid" paging="false" class="gridwraptable gridwrap">
                <thead>
                    <tr>
                        <th fieldmap="MAH_NO" align="left" width="15%">
                            <%=Resources.Controls.MANo%>
                        </th>
                        <th fieldmap="ITM_NAME" align="left" width="15%">
                            <%=Resources.Controls.MaterialName%>
                        </th>
                        <th fieldmap="MID_NO" align="left" width="15%">
                            <%=Resources.Controls.MINo%>
                        </th>
                        <th fieldmap="MID_DATE" align="left" width="15%">
                            <%=Resources.Controls.MIDate%>
                        </th>
                        <th fieldmap="MAD_QTY_ACCEPTED" align="right" width="8%">
                            <%=Resources.Controls.QtyIssued%>
                        </th>
                        <th fieldmap="UOM_CODE" align="left" width="8%">
                            <%=Resources.Controls.UOMCode%>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
         <asp:HiddenField ID="hdfRefID" runat="server" Value="0" />
        <asp:HiddenField ID="MaterialList" runat="server" />
         <asp:HiddenField ID="hdfIsGoToInbox" runat="server" Value="0" />
        <asp:HiddenField ID="SubmitFlag" runat="server" Value="0"></asp:HiddenField>
        <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
          <asp:HiddenField ID="IsPrefID" runat="server" Value="0" />
        <%--Comma Separation for Quantity & Amount Based on Configuration(Table)--%>
           <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
            <asp:HiddenField ID="hdfCurrencyGroup2" Value="3" runat="server" />
            <asp:HiddenField ID="hdfIsModify" runat="server" Value="0" />
        <div id="divData">
        </div>
        <div class="clear">
        </div>
    </div>
    <script type="text/javascript">
    function fnConfirmStockValueChange(command) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>'; 
            msg = '<%= Resources.Messages.MayAffectStockValueConfirmation %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 275,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $(this).dialog("close");
                        $("[id$=ConfirmStockValueChange]").val('1');
                        SavePage(command);
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        $("[id$=ConfirmStockValueChange]").val('0');
                    }
                }
            });
            return false;
        }
   </script>
</asp:Content>
