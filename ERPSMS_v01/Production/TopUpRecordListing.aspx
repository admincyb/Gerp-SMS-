<%@ Page Title="<%$ Resources:Captions,Title_TopUpRecord %>" Theme="ERP-Blue" Language="C#"
    MasterPageFile="~/ERPSMS.Master" AutoEventWireup="true" CodeBehind="TopUpRecordListing.aspx.cs"
    Inherits="ERPSMS_v01.Production.TopUpRecordListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/Production/TopUpRecordListing.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.TopUpRecord%>
        </h1>
        <div class="button-wrap">
            <asp:ImageButton ID="imbAdd" runat="server" SkinID="btnadd" OnClientClick="javascript:return AddNew();"
                EnableViewState="False" TabIndex="6" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();"
                EnableViewState="False" TabIndex="7" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel"  PostBackUrl="~/AccountManagement/WorkflowInbox.aspx"
                TabIndex="8" />
        </div>
    </div>
    <div id="grdTable-wrap">
        <div id="searchwrap">
            <div id="divSearch">
                <label for="SearchType">
                    <%=Resources.Controls.SearchBy%></label>
                <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" TabIndex="1"
                    onchange="javascript:SetSearchType();" EnableViewState="false">
                    <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                    </asp:ListItem>
                    <asp:ListItem Value="SHF_NAME" Text="<%$ Resources:Controls, Shift%>">
                    </asp:ListItem>
                    <asp:ListItem Value="PLN_NAME" Text="<%$ Resources:Controls, Plan%>">
                    </asp:ListItem>
                    <asp:ListItem Value="Date" Text="<%$ Resources:Controls, DateRange%>">
                    </asp:ListItem>
                </asp:DropDownList>
                <div id="divSearchDtls">
                    <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false" TabIndex="2">
                    </asp:TextBox>
                </div>
                <div id="divDate">
                    <label for="FromDate">
                        <%=Resources.Controls.FromDate%></label>
                    <asp:TextBox ID="FromDate" runat="server" TabIndex="3" EnableViewState="false">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfFrmDate" runat="server" />
                    <label for="ToDate">
                        <%=Resources.Controls.ToDate%></label>
                    <asp:TextBox ID="ToDate" runat="server" TabIndex="4" EnableViewState="false">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfToDate" runat="server" />
                </div>
                <asp:ImageButton ID="imbSearch" runat="server" SkinID="btnsearchgo" Width="30px"
                    TabIndex="5" OnClientClick="javascript:return BindGrid();" Height="20px" EnableViewState="false" />
            </div>
        </div>
        <div class="grdTable">
            <table rules="all" id="grdPurchaseRequest" grandtype="GrandGrid" pagesize="20" paging="true"
                width="100%" width="110%" editfunction="GridAction" editable="true">
                <thead>
                    <tr>
                        <th fieldmap="TUH_PK" isvisible="false">
                        </th>
                        <th fieldmap="SHF_NAME" sortable="true" width="22%" align="left">
                            <%=Resources.Controls.Shift%>
                        </th>
                        <th fieldmap="PLN_NAME" sortable="true" width="22%" align="left">
                            <%=Resources.Controls.Plan%>
                        </th>
                        <th fieldmap="TUH_DATE" sortable="true" width="22%" align="left">
                            <%=Resources.Controls.Date%>
                        </th>
                        <th fieldmap="TUH_TIME" sortable="true" width="22%" align="left">
                            <%=Resources.Controls.Time%>
                        </th>
                        <th type="Template" width="12%" align="left">
                            <div style="text-align: left">
                                <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'edit')" />
                                <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'delete')" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
</asp:Content>
