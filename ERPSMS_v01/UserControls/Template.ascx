<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Template.ascx.cs" Inherits="MailSend.UserControl.Template" %>
<%@ Register Assembly="FredCK.FCKeditorV2, Culture= neutral, Version=2.6.3.22451, PublicKeyToken=4f86767c9b519a06"
    Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<script type="text/javascript">


    //Pename the Page_Entry ID with the corresponding pages Entry section ID        
    function ShowHide(flag) {
        if (flag == "ADD_DETAILS") {
            $("[id$=SEC_ActionPanel]").hide();
            $("[id$=Page_Entry]").hide();
        }
        else {
            $("[id$=Page_Entry]").show();
            $("[id$=SEC_ActionPanel]").show();
        }
        return false;
    }

    function ValidateNow() {
        if (typeof (Page_ClientValidate) == 'function') {
            Page_ClientValidate();
        }
        if (!Page_IsValid) {
            $("[id$=litErrorMsg]").hide();
            ShowErrorMessage($("#diverror").html());
            return false;  //Page is invalid -- stop right here
        }
        else {
            //everythings ok --- Call your function & do your stuff
            return true;
        }
    }
    function CheckOtherIsCheckedByGVID(spanChk) {
        var IsChecked = spanChk.checked;
        var CurrentRdbID = spanChk.id;
        $("[id$=grdPage]").find("tr:has(td)").each(function () {
            //type = $(this).find("td:first input").attr("type")
            var id = $(this).find("td:first input").attr("id");
            if (id != CurrentRdbID) {
                $(this).find("td:first input").attr("checked", false);
            }
        });
    }


    function ClosePopUp() {
        if ($('#divmodel').length > 0) {
            $('#divmodel').hide();
        }
    }
    function ShowMailError(message, title, RedirectURL) {
        if (!title)
            title = errorTitle;
        $(".error").html("");
        $(".error").html(message);
        if (RedirectURL) {
            $(".error").dialog({
                resizable: false,
                title: title,
                buttons: {
                    OK: function (e) {
                        window.location = RedirectURL;
                    }
                },
                beforeClose: function (event, ui) { window.location = RedirectURL; },
                modal: true,
                open: function (event, ui) {
                    $(this).parent().appendTo("#popupHolder");

                }
            });
        }
        else {

            $(".error").dialog({
                resizable: false,
                title: title,

                modal: true,
                open: function (event, ui) {
                    $(this).parent().appendTo("#popupHolder");
                }
            });
        }
        return false;
    }
    function ShowMailPopUp(containerID, title, width, height) {
        ///<summary>
        ///function used to Show the Menu
        ///</summary>
        if (!width)
            width = 970;
        if (!height)
            height = 520;
        if (!title)
            title = errorTitle;
        $(containerID).dialog('distroy');
        $("#popupHolder").html("");
        $(containerID).dialog({
            width: width,
            draggable: false,
            height: height,
            resizable: false,
            title: title,
            modal: false,
            open: function (event, ui) {
                $(this).parent().appendTo("#popupHolder");
                $('#divmodel').show();
            },
            close: function (event) {

                if (typeof AfterClose == "function") {
                    AfterClose(containerID);
                }
                //            $(this).remove();
                $('#divmodel').hide();
            }
        });
        return false;
    }


</script>
<asp:UpdatePanel ID="aupdMenu" runat="server">
    <ContentTemplate>
        <div class="content-wrapper">
        <div class="search-wrap-b">
                        <asp:Label ID="lblmod" runat="server" Text="Module "></asp:Label>
                        <asp:DropDownList runat="server" ID="ddlmod" AutoPostBack="false" TabIndex="1">
                            <%--<asp:ListItem Text="Select" Value="-1"></asp:ListItem>--%>
                            <asp:ListItem Text="ERP" Value="0"></asp:ListItem>
                        </asp:DropDownList>

                        <asp:Label runat="server" ID="lblApp" Text="Application"></asp:Label>
                        <asp:DropDownList runat="server" ID="ddlApp" TabIndex="2">
                        </asp:DropDownList>

                        <asp:Label runat="server" ID="lblsubApp" Text="Sub Application"></asp:Label>
                        <asp:DropDownList runat="server" ID="ddlsubApp" TabIndex="2">
                        </asp:DropDownList>

                        <asp:Label ID="lblTemplateType" runat="server" Text="Template "></asp:Label>
                        <asp:DropDownList ID="ddlTempType" runat="server" AutoPostBack="true">
                        </asp:DropDownList>

                        <asp:Label runat="server" ID="lblAction" Text="Action "></asp:Label>
                        <asp:DropDownList runat="server" ID="ddlAction" TabIndex="2">
                        </asp:DropDownList>
                        <asp:Button runat="server" ID="btnShow" CommandName="SHOW" Text="Show" SkinID="btnInner-View" OnClick="ActionHandler"
                            meta:resourcekey="btnShowResource1" />
</div>


        <div class="Button-container-floatig">
            <asp:Table ID="tblButton" runat="server">
                <asp:TableRow>
                    <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                      
                        <ul id="pnlEntry" runat="server">
                        <li> <asp:Button ID="imbSave" runat="server" Text="Save" OnClick="ActionHandler"  SkinID="btnInner-Save"  CommandName="SAVE" /></li>
                        <li> <asp:Button ID="btnCancel"  runat="server" SkinID="btnInner-Cancel"  OnClick="ActionHandler" CommandName="SHOW" Text="Cancel" /> </li>
                        
                        </ul>
                    </asp:TableCell></asp:TableRow>
            </asp:Table>
        </div>

         
             <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <%--Used For Listing Id is Used for Section Privilage--%>
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
            </asp:TableCell></asp:TableRow></asp:Table>
            <%--<asp:Button ID="imbSave" Text="Save" runat="server" ToolTip="Save to add more items"
                Visible="false" CommandName="SAVE" OnClick="ActionHandler" />--%>
            <%--<asp:Button ID="imbClear" runat="server" Text="Clear" ToolTip="Clear" 
                CausesValidation="False" OnClick="ActionHandler" />--%>
            <div class="dotted-header" id="divEditor" runat="server" visible="false" style="margin-top: 0px">
                <%--<FCKeditorV2:FCKeditor ID="FCKeditor1" runat="server">
        </FCKeditorV2:FCKeditor>--%>
                <%-- <FCKeditorV2:FCKeditor ID="FCKeditor1" runat="server">
                </FCKeditorV2:FCKeditor>--%>
                <FCKeditorV2:FCKeditor ID="FCKeditor1" runat="server" BasePath="../fckeditor/">
                </FCKeditorV2:FCKeditor>
                <asp:GridView ID="grdTemplateTags" runat="server" AutoGenerateColumns="False" AllowPaging="true"
                    PageSize="20" OnPageIndexChanging="ActionHandler" ShowFooter="True">
                    <PagerStyle BackColor="White" ForeColor="#333333" HorizontalAlign="Center" />
                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="Numeric" NextPageText="Next"
                        PreviousPageText="Previous" />
                    <Columns>
                        <asp:TemplateField HeaderText="No" ItemStyle-Width="2%">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle Width="2%"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Template Tag" HeaderStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:TextBox ID="txtTag" runat="server" ReadOnly="True" EnableTheming="false" ToolTip="Copy and Paste in Editor"
                                    Text='<%# Eval("ATG_TAG") %>' Width="95%"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            <ItemStyle Width="35%" HorizontalAlign="Left"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ATG_DESC" HeaderText="Tag Description">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="63%" HorizontalAlign="Left" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:GridView ID="grdTemplate" runat="server" AutoGenerateColumns="False" Width="100%"
                ShowFooter="True" DataKeyNames="TML_PK" AllowPaging="true" AllowSorting="true"
                PageSize="10" OnPageIndexChanging="ActionHandler" onrowediting="GridView1_RowEditing" OnRowDataBound="ActionHandler"
                OnSorting="ActionHandler">
                <PagerStyle BackColor="White" ForeColor="#333333" HorizontalAlign="Center" />
                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="Numeric" NextPageText="Next"
                    PreviousPageText="Previous" />
                <Columns>
                    <asp:TemplateField HeaderText="No" ItemStyle-Width="2%">
                        <ItemTemplate>
                            <%#Container.DataItemIndex+1 %>
                        </ItemTemplate>
                        <ItemStyle Width="2%"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Application Type" SortExpression="APP_TYPE_TEXT" HeaderStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbnProcess" runat="server" CommandArgument='<%# Eval("TML_PK") %>'
                                CommandName="EDIT" ToolTip="To Edit" Text='<%# Eval("APP_TYPE_TEXT")%>' Font-Bold="True"
                                OnClick="ActionHandler"></asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle Width="49%" HorizontalAlign="Left"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TML_TYPE_TEXT" SortExpression="TML_TYPE_TEXT" HeaderText="Template Type">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="49%" HorizontalAlign="Left" />
                    </asp:BoundField>
                </Columns>
                <EmptyDataTemplate>
                    <div style="color: Navy; text-align: center">
                        <asp:Label ID="lblBlankGridMsg1" runat="server" Text="No Data Found."></asp:Label>
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
