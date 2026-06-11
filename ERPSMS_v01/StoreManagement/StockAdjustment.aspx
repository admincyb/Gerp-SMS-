<%@ Page Title="<%$ Resources:Captions,Title_StoreAdjustment %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="StockAdjustment.aspx.cs" Inherits="ERPSMS_v01.StoreManagement.StockAdjustment" %>

<%@ Register Src="../WorkFlow/WorkflowComments.ascx" TagName="WorkflowComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/StockAdjustment.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--  <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.StoreAdjustment%>
        </h1>
        <div class="button-wrap">
            <asp:ImageButton ID="imdReset" runat="server" TabIndex="34" EnableViewState="false"
                SkinID="btnreset" PostBackUrl="~/StoreManagement/StockAdjustmentListing.aspx" />
        </div>
    </div>--%>
    <asp:HiddenField ID="ConfirmStockValueChange" runat="server" Value="0" />
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
                                <asp:Button runat="server" ID="btnSubmit" SkinID="btnInner-submit" ToolTip="<%$Resources:Controls,Submit%>"
                                    Text="<%$Resources:Controls,Submit%>" TabIndex="2" EnableViewState="False" OnClientClick="javascript:return  WkfSubmit();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnCancel" SkinID="btnInner-Cancel" ToolTip="<%$Resources:Controls,Cancel%>"
                                    Text="<%$Resources:Controls,Cancel%>" TabIndex="3" EnableViewState="False" PostBackUrl="~/StoreManagement/StockAdjustmentListing.aspx" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div class="clear">
        </div>
    </div>
    <div class="content-wrapper">
        <div id="divData">
            <asp:HiddenField ID="ItemList" runat="server" />
            <asp:HiddenField ID="SAH_PK" runat="server" Value="0" />
            <asp:HiddenField ID="SDH_PK" runat="server" Value="0" />
             <%--Comma Separation for Quantity & Amount Based on Configuration(Table)--%>
           <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
            <asp:HiddenField ID="hdfCurrencyGroup2" Value="3" runat="server" />
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S">
                            <label for="SANo">
                                <%=Resources.Controls.StoreAuditNo%></label>
                            <asp:Label ID="SAHTXT_NO" runat="server"  CssClass="input-small-b"></asp:Label>
                            <asp:HiddenField ID="SAH_NO" runat="server" />
                            <label for="SAH_DATE" class="middle-lbl">
                                <%=Resources.Controls.Date%></label>
                            <asp:Label ID="SAHTXT_DATE" runat="server"  CssClass="input-small"></asp:Label>
                            <asp:HiddenField ID="SAH_DATE" runat="server" />
                            <%--  <asp:TextBox ID="qq" Width="50px" TextMode="MultiLine" --%>
                            <div class="clear">
                            </div>
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S">
                            <label for="Store">
                                <%=Resources.Controls.Store%>
                            </label>
                            <span id="SAH_DEPTTXT_STORE"></span>
                            <asp:HiddenField ID="SAH_DEPT_STORE" runat="server" />
                          <div id="divSBUCompany">
                            <label for="SDH_COMPANY">
                                <%=Resources.Controls.Company%>*</label>
                            <asp:DropDownList ID="SDH_COMPANY" runat="server" TabIndex="3" ClientIDMode="Static" Enabled="false"  CssClass="select-half">
                            </asp:DropDownList>
                            <asp:HiddenField ID="hdfSelCompany" runat="server" />
                         </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="clear">
        </div>
        <div class="scroll-h150">
            <div class="gridwrap nomargin">
                <table rules="all" id="grdItemList" grandtype="GrandGrid" pagesize="20" paging="true"
                    enablecheckbox="false" width="100%" editfunction="GridAction" class="gridwraptable gridwrap">
                    <thead>
                        <tr>
                            <th fieldmap="SAD_ID" isvisible="false">
                            </th>
                            <th fieldmap="SAD_PK" isvisible="false">
                            </th>
                            <th fieldmap="SAD_ITEM_CATEGORY" isvisible="false">
                            </th>
                            <th fieldmap="SAD_ITEM" isvisible="false">
                            </th>
                            <th fieldmap="SAD_ITEM_BATCH" isvisible="false">
                            </th>
                             <th fieldmap="SDD_STK_BATCH" isvisible="false"> <%--Stock Batch Pk--%>
                            </th>                            
                            <th fieldmap="SAD_ITEM_CATEGORY_TEXT" sortable="true" width="7%" align="left">
                                <%=Resources.Controls.Category%>
                            </th>
                            <th fieldmap="SADTXT_ITEM" sortable="true" width="16%" align="left">
                                <%=Resources.Controls.Code%>
                            </th>
                             <th fieldmap="SDD_STK_BATCH_NO" sortable="true" width="10%" align="left"> <%--Stock Batch No--%>
                                <%=Resources.Controls.BatchNo%>
                            </th>
                            <th fieldmap="SAD_ITEM_UOM_TEXT" sortable="true" width="3%" align="left">
                                <%=Resources.Controls.UOM%>
                            </th>
                            <th fieldmap="SAD_CUR_STK" sortable="true" width="8%" align="right">
                                <%=Resources.Controls.LedgerStk%>
                            </th>
                            <th fieldmap="SAD_CUR_STK_VAL" sortable="true" width="8%" align="right">
                                <%=Resources.Controls.LedgerVal%>
                            </th>
                            <th fieldmap="SAD_ACT_STK" sortable="true" width="8%" align="right">
                                <%=Resources.Controls.ActualStk%>
                            </th>
                            <th fieldmap="SAD_ACT_STK_VAL" sortable="true" width="8%" align="right">
                                <%=Resources.Controls.ActualVal%>
                            </th>
                            <th fieldmap="SAD_REMARKS" sortable="true" width="9%" align="right">
                                <%=Resources.Controls.Comments%>
                            </th>
                            <th fieldmap="SDD_QTY_ADJ" sortable="true" width="8%" align="right">
                                <%=Resources.Controls.Adjustment%>
                            </th>
                            <th fieldmap="SDD_REMARKS" sortable="true" width="15%" align="left">
                                <%=Resources.Controls.Remarks%>
                            </th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
        <div id="Wofkflowdiv" style="display: none">
            <uc1:WorkflowComments ID="ucrWrkf" runat="server" ValidationGroup="Save" />
            <asp:HiddenField ID="hdfProcessID" runat="server" Value="0" />
            <asp:HiddenField ID="hdfAppID" runat="server" Value="0" />
            <asp:HiddenField ID="PRefID" runat="server" Value="0" />
            <asp:HiddenField runat="server" ID="ActionID" />
            <asp:HiddenField ID="AppNo" runat="server" Value="0" />
            <asp:HiddenField ID="SubmitFlag" runat="server" Value="0"></asp:HiddenField>
        </div>
    </div>
    <div id="divDatas">
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
