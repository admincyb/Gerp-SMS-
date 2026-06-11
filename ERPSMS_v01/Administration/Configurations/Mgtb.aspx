<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    Theme="Classic" CodeBehind="Mgtb.aspx.cs" Inherits="ERPSMS_v01.Administration.Configurations.Mgtb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function ShowHideAdvancedSearch(flag) {//collapse panel for Check list section
            if (flag == 1) {
                $("[id$=tbladvancedSearch]").show();
                $("[id$=imbShowFilter]").hide();
                $("[id$=imbHideFilter]").show();
                //  $("[id$=hdfShowHideFilter]").val("0");
            }
            else {
                $("[id$=tbladvancedSearch]").hide();
                $("[id$=imbShowFilter]").show();
                $("[id$=imbHideFilter]").hide();
                // $("[id$=hdfShowHideFilter]").val("1");
            }
            $("[id$=hdfShowHideFilter]").val(flag);
            return false;
        }
        function ShowBox() {
            $("#Slider").slideToggle("slow");
        }
        function ShowDeleteGridConfirm() {
            var message;
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.Captions.Title_Information %>';
            if (message == 'reload') {
                message = '<%= Resources.Captions.Msg_Prompt_Reload %>';
            }
            else if (message == 'all') {
                message = '<%= Resources.Captions.Msg_DeletConfirmAll %>';
            }
            msg = message ? message : '<%= Resources.Captions.Msg_Delete_Confirm %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $(this).dialog("close");
                        $("[id$=btnDelete]").click();
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }

    </script>
<%--</asp:Content>--%>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">--%>
    <asp:UpdatePanel ID="auplManageTable" runat="server">
        <ContentTemplate>
            <div id="innerPage-wrap">
                <div class="content-wrapper">
                    <div id="divManageTables" runat="server" visible="false">
                 <%--       <div class="search-colapse">
                            <table>
                                <tr>
                                    <td>
                                        <h1>
                                            <%= GetGlobalResourceObject("Constants", "AdvFilter").ToString()%></h1>
                                    </td>
                                    <td>
                                        <a class="active-arrow" href="javascript:void(0);" onclick="ShowBox();" title="Toggle Screen">
                                            &nbsp;</a>
                                    </td>
                                </tr>
                            </table>
                        </div>--%>
                        <div class="search-colapse">
                        <table>
                            <tr>
                                <td>
                                    <h1>
                                        <%= GetGlobalResourceObject("Constants", "AdvFilter").ToString()%>
                                    </h1>
                                </td>
                                <td>
                                    <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                        SkinID="imbArrowInactive" ToolTip="Show Filter" TabIndex="9" />
                                    <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(0);"
                                        SkinID="imbArrowActive" ToolTip="Hide Filter" TabIndex="10" />
                                    <asp:HiddenField ID="hdfShowHideFilter" runat="server" Value="0" ClientIDMode="Static" />
                                </td>
                            </tr>
                        </table>
                        <div class="clear">
                        </div>
                       
                    </div>
                        <div id="Slider" style="display: height: 280px" class="colapse-container">
                            <table class="table-devide" id="tbladvancedSearch">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7 margn-btm0">
                                            <label for="ddlTable" class="margnrgt5">
                                                <asp:Literal ID="Literal1" runat="server" Text="Tables" />
                                            </label>
                                            <asp:DropDownList runat="server" ID="ddlTables" AutoPostBack="true" TabIndex="1"
                                                CssClass="w58-7perc" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7 margn-btm0">
                                            <asp:DropDownList runat="server" ID="ddlOrder" Width="150px" TabIndex="2">
                                                <asp:ListItem Selected="True" Text="--Select Filter--"></asp:ListItem>
                                                <asp:ListItem Text="Top"></asp:ListItem>
                                                <asp:ListItem Text="Bottom"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:TextBox runat="server" ID="txtTopBottom" TabIndex="3"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7 margn-btm0">
                                            <label for="ddlFeild" class="margnrgt5">
                                                <asp:Literal ID="Literal2" runat="server" Text="Fields" />
                                            </label>
                                            <asp:DropDownList runat="server" ID="ddlFields" Width="293" AutoPostBack="true"
                                                OnSelectedIndexChanged="ActionHandler" TabIndex="4">
                                                <asp:ListItem Selected="True" Text="--Select Field--"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7 margn-btm0">
                                            <asp:DropDownList runat="server" ID="ddlOperator" Width="150px" TabIndex="5">
                                                <asp:ListItem Selected="True" Text="--Select Operator--"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:TextBox runat="server" ID="txtQueryField" TabIndex="6"></asp:TextBox>
                                            <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="ddlOperator"
                                                Enabled="true" ErrorMessage="*" Style="width: 1%;"></asp:CustomValidator>
                                            <%-- <asp:ImageButton ID="ImageButton1" runat="server" Visible="true"
                                        ToolTip="Search" Width="20px" Style="margin-top: 2px !important; margin-right: 10px;"
                                        OnClick="ActionHandler" CommandName="SEARCH" TabIndex="7" />--%>
                                            <asp:Button ID="btnSearch" runat="server" TabIndex="7" Text="Search" OnClick="ActionHandler"
                                                CommandName="SEARCH" Width="60px" />
                                            <asp:Button ID="Button1" runat="server" Text="Clear" Visible="true" ToolTip="Clear Fields"
                                                Width="60px" Style="margin-top: 2px !important; margin-right: 20px;" OnClick="ActionHandler"
                                                CommandName="CLEAR" TabIndex="8" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <%-- <label id="lblConnectionString" runat="server"></label>--%>
                                        <asp:Label runat="server" ID="lblConnectionString" AssociatedControlID="lblConnectionString"
                                            Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7 margn-btm0">
                                        </div>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        <asp:Button runat="server" ID="btnInsert" Text="Insert Record" OnClick="ActionHandler"
                            CommandName="INSERTROW" TabIndex="9" />
                        <asp:Button runat="server" ID="btnQueryWindow" Text="Query Window" OnClick="ActionHandler"
                            CommandName="INSERTQUERY" TabIndex="9" />
                            
                            </div>
                        <div class="clear">
                        </div>
                        <div class="gridwrap">
                            <asp:GridView runat="server" ID="grdManageTable" AutoGenerateEditButton="True" AutoGenerateDeleteButton="True"
                                OnRowEditing="ActionHandler" CssClass="grdTable" OnRowCancelingEdit="ActionHandler"
                                OnRowUpdating="ActionHandler" OnRowDeleting="ActionHandler" OnPageIndexChanging="ActionHandler"
                                AllowPaging="True" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found"
                                EmptyDataRowStyle-CssClass="emptytable" AllowSorting="True" EmptyDataRowStyle-HorizontalAlign="Center">
                            </asp:GridView>
                            <div class="clear">
                            </div>
                        </div>
                        <div id="divError" runat="server">
                            <asp:Label runat="server" ID="lblErrMsg" Width="100%" Style="font-family: Verdana;
                                color: Red;"></asp:Label>
                        </div>
                    </div>
                    <div id="divUserLogin" runat="server">
                        <table class="table-devide" id="Table1">
                            <tr>
                                <td>
                                    <div class="div2col-S padgtop7 margn-btm0">
                                        <asp:Label ID="lblPassword" runat="server" Text="Enter Password" AssociatedControlID="txtPassword"></asp:Label>
                                        <asp:TextBox ID="txtPassword" TextMode="Password" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CommandName="SUBMIT" OnClick="ActionHandler" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <%----%>
                    <div id="divQueryDetails" class="contentwrapper" style="display: none">
                        <table class="table-devide" id="Table2">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtQuery" TextMode="MultiLine" EnableTheming="false" Rows="20" Width="860px"
                                        CssClass="multilarge" runat="server"></asp:TextBox>
                                    <asp:Button ID="btnSubmitQuery" runat="server" Text="Submit" CommandName="SUBMITQUERY"
                                        OnClick="ActionHandler" />
                                    <asp:Button ID="btnSubmitViewQuery" runat="server" Text="View" CommandName="VIEWQUERY"
                                        OnClick="ActionHandler" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div style="display: none">
                    <asp:Button ID="btnDelete" runat="server" OnClick="ActionHandler" CommandName="DELETE" />
                </div>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
