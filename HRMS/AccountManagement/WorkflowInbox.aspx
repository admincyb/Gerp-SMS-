<%@ Page Title="<%$ Resources:Captions,Title_Inbox %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="WorkflowInbox.aspx.cs" Inherits="ERPSMS_v01.AccountManagement.WorkflowInbox"
    EnableEventValidation="false" Theme="ClassicExt" %>

<%@ Register Src="../UserControls/PagerControl.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/AccountManagement/WorkflowInbox.js.axd" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        var pageURLAuto = window.document.URL;
        var virtualPathAuto = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var urlAuto = pageURLAuto.replace(location.pathname, virtualPathAuto == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPathAuto + "Handlers/AutoComplete.ashx");
        function InitAuto() {
            GrandScriptUtils.MakeAutoCompleteDDL('txtdept', urlAuto + "?Type=" + $("[id$='ddlSbu']").val(), 'hdnDept', true, true, 'FILLDEPT');
            InitProcess();
        }
        function InitProcess() {
            GrandScriptUtils.MakeAutoCompleteDDL('txtProcess', urlAuto + "?Type=" + $("[id$='hdnDept']").val() + "&sbu="+ $("[id$='ddlSbu']").val(), 'hdfProcess', true, true, 'FILLPROCESS');
            }
              //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtdept") {
               InitProcess()
            }
        }
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtdept") {
               InitProcess()
            }
        }
        
        function PrintReport()
        {
          var applicationType={
             SubContractWorkOrder: "SWO"
           };
          var url='';
          var appType= $("[id$=hdnAppType]").val();
          switch(appType)
          {
             case applicationType.SubContractWorkOrder:
                url= $("[id$=hdnPageServer]").val() + "/Reports/GenerateReport.aspx?ID=" + $("[id$=hdnApplicationPK]").val()+ "&APPTYPE=SWO&APPSUBTYPE=4";
                break;
          }
           ClosePopup();
           OpenPDF(url);
        }
        function ShowHideTabs() {
        
            var intimationShow = <%=GetGlobalResourceObject("ConfigurationsRes", "ShowWorkflowIntimation") %>;
            var alertShow = <%=GetGlobalResourceObject("ConfigurationsRes", "ShowWorkflowAlert") %>; 
            if(intimationShow=="0")
               $("[id$=liIntimation]").hide(); 
            if(alertShow=="0")
               $("[id$=liAlert]").hide();  
          } 
           function ShowDetails(refPK,title,IsApproveVisible,ProcesssDpt,RedirectUrl,IsInboxPrint,ApplicationID,ApplicationType,PageServer) {
            var pageURL = window.document.URL; 
            var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
            var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/DataHandler.ashx" : "/" + virtualPath + "Handlers/DataHandler.ashx");
           
            $("[id$=hdnAppType]").val(ApplicationType);
            $("[id$=hdnPageServer]").val(PageServer);
            $("[id$=hdnApplicationPK]").val(ApplicationID);

            $("[id$=hdfTitle]").val(title);
            $("[id$=hdfPopupRefPK]").val(refPK);
            $("[id$=hdfProcessDeptPopup]").val(ProcesssDpt);
            $("[id$=hdfRedirectUrlPopup]").val(RedirectUrl);
            $.getJSON(url+"?SearchType=SUMMARY&SearchBy=" + refPK, function (data) {
                if(data.Fields)
                {
                    $('#DivDetailsDialog').html(""); 
                       var alt=2;
                       for (var i in data.Fields) {
                       if((alt%2)==1)
                       {
                          if(data.Fields[i].Value==null){
                             $('#DivDetailsDialog').append("<div class='summary-row1'><span>"+data.Fields[i].Label+"</span><label>" + " "+"</label></div>");
                          }
                          else{
                             $('#DivDetailsDialog').append("<div class='summary-row1'><span>"+data.Fields[i].Label+"</span><label>" + data.Fields[i].Value+"</label></div>");
                          }
                       }
                       else
                       {
                          if(data.Fields[i].Value==null){
                            $('#DivDetailsDialog').append("<div class='summary-row2'><span>"+data.Fields[i].Label+"</span><label>" + " "+"</label></div>");
                          }
                          else{
                            $('#DivDetailsDialog').append("<div class='summary-row2'><span>"+data.Fields[i].Label+"</span><label>" + data.Fields[i].Value+"</label></div>");
                          }
                       }
                       alt=alt+1;
                      }
                     ShowContainerDiv('[id$=divInfoPopup]', title, '500', '280');
                    $(".ui-draggable").addClass("lightborder"); 
                }
                else
                {
                    $('#DivDetailsDialog').html(""); 
                    $('#DivDetailsDialog').append("<%=GetGlobalResourceObject("Messages", "NoSummaryAvailable") %>"); 
                }
             });
             ShowContainerDiv('[id$=divInfoPopup]', title, '500', '280');
             if(IsApproveVisible=="1")
               $("[id$=divApprove]").show();
             else
               $("[id$=divApprove]").hide();

              if(IsInboxPrint=="1")
                $("[id$=divPrint]").show();
              else
                $("[id$=divPrint]").hide();
            return false;
        }  
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlInbox" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <ul id="tab-menu">
                        <asp:HiddenField runat="server" ID="hdfInboxType" Value="1" />
                        <li><span class="tab-active" id="spnTask" runat="server">
                            <asp:LinkButton ID="lbnTask" runat="server" OnClick="ActionHandler" Text="Tasks"></asp:LinkButton></span></li>
                        <li id="liIntimation"><span class="tab-inactive" id="spnIntimation" runat="server">
                            <asp:LinkButton ID="lbnIntimations" runat="server" OnClick="ActionHandler" Text="Intimations"> </asp:LinkButton></span></li>
                        <li><span class="tab-inactive" id="spnCompletedTask" runat="server">
                            <asp:LinkButton ID="lbnCompletedTask" runat="server" OnClick="ActionHandler" Text="Completed Tasks"></asp:LinkButton></span></li>
                        <li id="liAlert"><span class="tab-inactive" id="spnAlerts" runat="server">
                            <asp:LinkButton ID="lbnAlerts" runat="server" OnClick="ActionHandler" Text="Alerts"></asp:LinkButton></span></li>
                    </ul>
                    <div id="dvCulture" runat="server" visible="<%$ resources:ConfigurationsRes,CultureChangeVisibility %>"
                        class="txt-rgt">
                        <asp:DropDownList runat="server" ID="ddlLanguage" CssClass="select-4-6per margnbotm0 margntop1"
                            AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
            <%--        <div class="inbox-corner-img">
                </div>
                <asp:HiddenField runat="server" ID="hdfInboxType" Value="1" />
                <asp:LinkButton ID="lbnTask" runat="server" CssClass="inbox-item-selected" EnableTheming="false"
                    OnClick="ActionHandler" Text="Tasks"></asp:LinkButton>
                <asp:LinkButton ID="lbnIntimations" runat="server" CssClass="inbox-item" EnableTheming="false"
                    OnClick="ActionHandler" Text="Intimations"> </asp:LinkButton>
                <asp:LinkButton ID="lbnCompletedTask" runat="server" CssClass="inbox-item" EnableTheming="false"
                    OnClick="ActionHandler" Text="Completed Tasks"></asp:LinkButton>--%>
            <div class="content-wrapper">
                <div id="searchwrap" class="search-wrap-custom">
                    <span runat="server" id="spnSBU">
                        <%=GetGlobalResourceObject("Controls", "SBU")%></span>
                    <asp:DropDownList runat="server" ID="ddlSbu" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                        CssClass="select-small-a">
                    </asp:DropDownList>
                    <span runat="server" id="spnDepartment">
                        <%=GetGlobalResourceObject("Controls","Department")%></span>
                    <%--<asp:DropDownList ID="ddlDepartment" runat="server" EnableViewState="true" CssClass="select-small-b">
                    </asp:DropDownList>--%>
                    <asp:TextBox ID="txtdept" runat="server" CssClass="select-w17per">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdnDept" runat="server" Value="-1" />
                    <span>
                        <%=GetGlobalResourceObject("Controls", "DateFrom")%></span>
                    <asp:TextBox ID="PeriodFrom" runat="server" TabIndex="2" Width="80px" CssClass="aligncenter">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfPrdFrm" runat="server" />
                    <span style="padding-left: 6px">
                        <%=GetGlobalResourceObject("Controls", "DateTo")%></span>
                    <asp:TextBox ID="PeriodTo" runat="server" TabIndex="2" Width="80px" CssClass="aligncenter">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfPrdTo" runat="server" />
                    <span runat="server" id="spnProcess">
                        <%=GetGlobalResourceObject("Controls", "Process")%></span>
                    <%-- <asp:DropDownList ID="ddlProcess" runat="server" EnableViewState="true" CssClass="select-small-b">
                    </asp:DropDownList>--%>
                    <asp:TextBox ID="txtProcess" runat="server" CssClass="select-small-b">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfProcess" runat="server" Value="0" />
                    <span runat="server" id="spnTrxType" visible="false">
                        <%=GetGlobalResourceObject("Controls", "TransactionType")%></span>
                    <asp:DropDownList ID="ddlTrxType" runat="server" EnableViewState="true" Visible="false">
                    </asp:DropDownList>
                    <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" OnClick="ActionHandler"
                        CssClass="margntop2" />
                </div>
                <div class="gridwrap">
                    <asp:GridView ID="dgInbox" runat="server" AlternatingItemStyle-BackColor="Silver"
                        AutoGenerateColumns="False" AllowPaging="false" Width="100%" HeaderStyle-Font-Bold="true"
                        OnRowCommand="ActionHandler" EmptyDataRowStyle-CssClass="emptytable">
                        <PagerSettings Visible="false" />
                        <EmptyDataTemplate>
                            <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                        </EmptyDataTemplate>
                        <Columns>
                            <%--<asp:BoundField DataField="ProcessName" HeaderText="<%$Resources:Controls,ProcessName %>"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%" />--%>
                            <asp:BoundField DataField="TaskDate" HeaderText="<%$Resources:Controls,Date %>" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="12%" />
                            <asp:BoundField DataField="Dept" HeaderText="<%$Resources:Controls,DepartmentOrStore %>"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" />
                            <asp:BoundField DataField="TaskName" HeaderText="<%$Resources:Controls,TaskName %>"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%" />
                            <%--<asp:BoundField DataField="Message" HeaderText="<%$Resources:Controls,Message %>"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="45%" />--%>
                            <%--<asp:BoundField DataField="Comment" HeaderText="<%$Resources:Controls,Comment %>"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" />--%>
                            <asp:TemplateField HeaderText="<%$Resources:Controls,Message %>" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-Width="41%">
                                <ItemStyle HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%# HttpUtility.HtmlDecode(Convert.ToString(Eval("Message"))) %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Controls,Comment %>" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-Width="10%">
                                <ItemStyle HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%# HttpUtility.HtmlDecode(Convert.ToString(Eval("Comment"))) %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="GroupName" HeaderText="<%$Resources:Controls,GroupName %>"  
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%" />--%>
                            <asp:TemplateField HeaderText="<%$Resources:Controls,Action %>" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-Width="7%">
                                <ItemStyle HorizontalAlign="Center" CssClass="txt-rgt" />
                                <HeaderStyle CssClass="txt-rgt" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imbApprove" runat="server" OnClick="ActionHandler" CommandName="APPROVEITEM"
                                        CommandArgument="PageAction_Entry" SkinID="workflowapprove" ToolTip='<%#Eval("TaskName")%>'
                                        OnClientClick='<%# Eval("TaskName","return ShowApproveConfirm(this,\"{0}\" )")%>'
                                        Visible='<%#((int)Eval("IsApproveVisible")) == 1 ? true : false %>' Style="display: inline;" />
                                    <asp:ImageButton runat="server" ID="imbWkfInfo" SkinID="workflowsummary" OnClientClick='<%# "return ShowDetails("+Eval("RefID")+ ",\""+ Eval("TaskName") +"\","+Eval("IsApproveVisible")+","+Eval("ProcessDept")+",\""+Eval("RedirectUrl")+"\"," +Eval("PrcIsInboxPrint")+ ","+ Eval("RefApplication")+",\""+Eval("PrcAppType")+"\",\""+Eval("PageServer") +"\")" %>'
                                        Style="display: inline;" ToolTip="<%$Resources:Controls,Summary %>" />
                                    <asp:ImageButton runat="server" ID="imbAction" ToolTip="<%$Resources:Controls,Action %>"
                                        SkinID="workflowactiongrid" CommandName="Action" CommandArgument='<%#Eval("RedirectUrl")%>'
                                        Style="display: inline;" />
                                    <asp:HiddenField ID="hdfRedirectUrl" runat="server" Value='<%#Eval("RedirectUrl")%>' />
                                    <asp:HiddenField ID="hdfProcessDept" runat="server" Value='<%#Eval("ProcessDept")%>' />
                                    <asp:HiddenField ID="hdfRefID" runat="server" Value='<%#Eval("RefID")%>' />
                                    <asp:HiddenField ID="hdfNextTaskAction" runat="server" Value='<%#Eval("NextTaskAction")%>' />
                                    <%--<asp:Label runat="server" ID="lbldfsd" Text='<%#Eval("RedirectUrl")%>'></asp:Label>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                    </asp:GridView>
                    <asp:GridView ID="dgAlerts" runat="server" AlternatingItemStyle-BackColor="Silver"
                        AutoGenerateColumns="False" AllowPaging="false" Width="100%" HeaderStyle-Font-Bold="true"
                        OnRowCommand="ActionHandler" EmptyDataRowStyle-CssClass="emptytable" Visible="false">
                        <PagerSettings Visible="false" />
                        <EmptyDataTemplate>
                            <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:BoundField DataField="AlertOn" HeaderText="<%$Resources:Controls,AlertOn %>"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="8%" />
                            <asp:BoundField DataField="DueDate" HeaderText="<%$Resources:Controls,DueDate %>"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="8%" />
                            <asp:BoundField DataField="AlertName" HeaderText="<%$Resources:Controls,AlertName %>"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" />
                            <asp:BoundField DataField="Type" HeaderText="<%$Resources:Controls,TransactionType %>"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%" />
                            <asp:BoundField DataField="Message" HeaderText="<%$Resources:Controls,Message %>"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="51%" />
                            <asp:BoundField DataField="Status" HeaderText="<%$Resources:Controls,Status %>" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="Left" ItemStyle-Width="8%" />
                        </Columns>
                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                    </asp:GridView>
                    <uc1:PagerControl ID="ucrPager" runat="server" />
                </div>
            </div>
            <div id="divInfoPopup" style="display: none;">
                <div id="DivDetailsDialog" class="summary-popup scroll-h185">
                </div>

                <asp:HiddenField ID="hdnPageServer" runat="server" Value="" />
                <asp:HiddenField ID="hdnAppType" runat="server" Value="" />
                <asp:HiddenField ID="hdnApplicationPK" runat="server" Value="" />

                <asp:HiddenField ID="hdfTitle" runat="server" Value="" />
                <asp:HiddenField ID="hdfPopupRefPK" runat="server" Value="0" />
                <asp:HiddenField ID="hdfProcessDeptPopup" runat="server" Value="" />
                <asp:HiddenField ID="hdfRedirectUrlPopup" runat="server" Value="" />
                <div class="summary-btn-wrap">
                    <div class="float-right txt-rgt">
                        <asp:ImageButton runat="server" ID="imgPopupAction" ToolTip="<%$Resources:Controls,Action %>"
                            CssClass="details-btn" CommandName="ACTION" OnClick="ActionHandler" AlternateText="Details" />
                        <asp:Button ID="btnApprove" runat="server" Text="" OnClick="ActionHandler" CommandName="APPROVEITEM"
                            SkinID="btnInner-search" EnableTheming="false" Style="display: none;" />
                    </div>
                    <div id="divApprove" class="float-right txt-rgt">
                        <asp:ImageButton ID="imbApproveItem" runat="server" OnClick="ActionHandler" CommandName="APPROVEITEM"
                            CommandArgument="PageAction_Entry" ToolTip="Approve" CssClass="approve-btn" AlternateText="Approve"
                            OnClientClick="return ShowConfirmQuickApprove(this);" />
                    </div>
                    <div id="divPrint" class="float-left txt-left">
                        <asp:ImageButton ID="imbPrint" runat="server"
                            CommandArgument="PageAction_Entry" ToolTip="Print" CssClass="print-popup-btn" AlternateText="Print"
                            OnClientClick="PrintReport();" />
                        <%-- CommandName="PRINT" OnClick="ActionHandler"--%>
                    </div>
                </div>
            </div>
            <div id="diverror" style="display: none">
                <%--Use this label to bind the server errors--%>
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star" />
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <uc1:workflowusercomments id="ucrWrkf" runat="server">
                </uc1:workflowusercomments>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
