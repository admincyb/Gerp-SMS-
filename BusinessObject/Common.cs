using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Common
{
    public class TextValue
    {
        public string Text
        { get; set; }
        public string Value
        { get; set; }
    }
  
    public class DashBoardData
    {
        public int DLC_PK { get; set; }
        public int DLC_MODULE { get; set; }
        public string DLC_CODE { get; set; }
        public string DLC_NAME { get; set; }
        public Byte DLC_ACTIVE { get; set; }
        public int DLC_USER_PERMISSION { get; set; }
        public DashBoardTile TileName
        {
            get
            {
                return (DashBoardTile)DLC_PK;
            }
        }
    }
    public enum DashBoardTile
    {
        TaskSummary = 1,
        TaskByMe,
        TaskForMe,
        Sms,
        EDocs,
        Civil,
        HRMS,
        Production,
        Packing,
        QA,
        WIP,
        Warehouse
    }
    public class JsonMultiTreeList
    {
        public string TreeValue { get; set; }
        public string TreeText { get; set; }
        public string TreeParentValue { get; set; }
        public string TreeHasChild { get; set; }
        public string TreeShowEdit { get; set; }
        public string TreeChecked { get; set; }
        public string TreeSave { get; set; }
    }

    public class WorkflowDetails
    {
        public int ActionID { get; set; }
        public int ProcessID { get; set; }
        public int ReferenceID { get; set; }
        public int TaskID { get; set; }
        public int UserPK { get; set; }
        public string Comments { get; set; }
        public string ActionText { get; set; }

    }
    public class fileUpload
    {
        public string FileTitle { get; set; }
        public string FileID { get; set; }
        public string Folder { get; set; }
    }
    public enum LoginMode
    {
        LOCAL = 1,
        ACTIVEDIRECTORY = 2,
        BOTH = 3
    }
    public enum EntryStatus
    {
        NEWMODE,
        VIEWMODE,
        LISTMODE,
        ENTRYMODE,
        EDITMODE,
        TREEMODE,
        SAVEONLY,
        LISTDRAFTMODE,
        REPORTMODE,
        ALLOCATEMODE,
        AMEND
    }
    public enum UserLimtSettingsIndex
    {
        Total = 0,
        SIP = 1,
        Accounts = 2,
        Production = 3,
        HR = 4,
        Asset = 5,
        Construction = 6
    }
    public enum GroupType
    {
        Resource = 30,
        Activity = 31,
        WorkOrder = 33,
        Project=44
    }
    public class SessionStrings
    {
        //For User Group
        public const string Rmsu = "Rmsu";
        public const string CurDept = "CurDept";
        public const string CurDeptCountry = "CurDeptCountry";
        public const string SBU = "SBU";
        #region WorkOrder
        public static string WorkOrderPk = "WorkOrderPk";
        #endregion
    }
    /// <summary>
    /// WorkFlow e-mail types Enum
    /// </summary>
    public enum WorkFlowMailType
    {
        CustomMsg = 0,
        Defaultmsg,
        MailTemplate
    }

    public enum DirectOrderStatus
    {
        Enquiry = 1,
        Quotation,
        DirectOrder,
        SalesOrder
    }

    public enum SaleOrderStatus
    {
        SaleOrder = 1,
        InternalOrder
    }

    public enum ShippingUploadsEnum
    {
        QA = 7,
        Export = 8,
        Photographs = 9,
        BillofLoading = 12,
        ContainerEvaluation = 13,
        ContainerInspection=14
    }
    public enum ShippingTabsEnum
    {
        ShippingPlan = 1,
        PachingDone,
        PaymentCleared,
        ContainerEvaluated,
        ContainerInspected,
        QADocsUploaded,
        ExportDocsUploaded,
        LoadingPlanCompleted,
        PhotographsUploaded,
        DeliveryOrderCompleted,
        ContainerReleased,
        BillofLoading
    }
    public enum PageTypeEnum
    {
        Transaction = 0,
        Listing
    }

    public enum InvoiceAction
    {
        SingleInvoice,
        MultipleInvoice,
        ListInvoice
    }
    public enum UserRight
    {
        OtherUser = 0,
        SuperAdmin = 1
    }
    public enum UserModuleLimitIndex
    {
        Asset = 0,
        Employee = 1,
        Production = 2
    }
    /// <summary>
    /// WorkOrderType
    /// WO = Work Order, SWO = Sub Work Order
    /// </summary>
    public enum WorkOrderType
    {
        WO, // WO = Work Order
        SWO // SWO = Sub Work Order
    }
}
