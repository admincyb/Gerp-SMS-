using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessObject.OrderPlanningBO
{
    #region PENDING ORDERS
    [Serializable]
    [XmlRoot("Root")]
    public class PendingOrderBO
    {
        [XmlElement("Details")]
        public List<PendingOrderList> Details { get; set; }
        [XmlElement("PlanItemGroup")]
        public List<PlanItemGroup> PlanItemGroup { get; set; }
        [XmlElement("BizUnit")]
        public List<BizUnit> BizUnit { get; set; }
        [XmlElement("ItemSpecGroup")]
        public List<ItemSpecGroup> ItemSpecGroup { get; set; }
        [XmlElement("SaleOrder")]
        public List<SaleOrder> SaleOrder { get; set; }
        [XmlElement("Customer")]
        public List<Customer> Customer { get; set; }
    }
    [Serializable]
    public class PendingOrderList
    {
        [XmlElement("SOH_PK")]
        public int SOH_PK { get; set; }
        [XmlElement("SOH_NO")]
        public string SOH_NO { get; set; }
        [XmlElement("SOD_PK")]
        public int SOD_PK { get; set; }
        [XmlElement("SOD_ITEM")]
        public int SOD_ITEM { get; set; }
        [XmlElement("SOD_ITEM_CODE")]
        public string SOD_ITEM_CODE { get; set; }
        [XmlElement("SOD_ITEM_TEXT")]
        public string SOD_ITEM_TEXT { get; set; }
        [XmlElement("ISD_SIZE")]
        public int ISD_SIZE { get; set; }
        [XmlElement("ISD_SIZE_TEXT")]
        public string ISD_SIZE_TEXT { get; set; }
        [XmlElement("SOD_QTY")]
        public double SOD_QTY { get; set; }
        [XmlElement("SOD_REQUIRED_BY")]
        public string SOD_REQUIRED_BY { get; set; }
        [XmlElement("SOD_QTY_DISPATCHED")]
        public double SOD_QTY_DISPATCHED { get; set; }
        [XmlElement("SOD_QTY_ALLOCATED")]
        public double SOD_QTY_ALLOCATED { get; set; }
        [XmlElement("SOD_QTY_PLANNED")]
        public double SOD_QTY_PLANNED { get; set; }
        [XmlElement("SOD_BAL_TO_PLAN")]
        public double SOD_BAL_TO_PLAN { get; set; }
        [XmlElement("ITM_PLAN_GROUP")]
        public int ITM_PLAN_GROUP { get; set; }
        [XmlElement("ITM_PLAN_GROUP_TEXT")]
        public string ITM_PLAN_GROUP_TEXT { get; set; }
        [XmlElement("ISD_AGRADE_PER")]
        public double ISD_AGRADE_PER { get; set; }
        [XmlElement("ROW_COUNT")]
        public int ROW_COUNT { get; set; }
        [XmlElement("SOH_CUSTOMER_CODE")]
        public string SOH_CUSTOMER_CODE { get; set; }
        [XmlElement("SOH_CUSTOMER_NAME")]
        public string SOH_CUSTOMER_NAME { get; set; }

        [XmlElement("ISD_COLOUR")]
        public int ISD_COLOUR { get; set; }
        [XmlElement("ISD_LENGTH")]
        public int ISD_LENGTH { get; set; }
        [XmlElement("ISD_NATURE")]
        public int ISD_NATURE { get; set; }
        [XmlElement("ISD_PROCESS")]
        public int ISD_PROCESS { get; set; }
        [XmlElement("ISD_SURFACE")]
        public int ISD_SURFACE { get; set; }
        [XmlElement("ISD_THICKNESS")]
        public int ISD_THICKNESS { get; set; }
        [XmlElement("SOH_BIZUNIT")]
        public int SOH_BIZUNIT { get; set; }
        [XmlElement("SOH_BIZUNIT_TEXT")]
        public string SOH_BIZUNIT_TEXT { get; set; }

        [XmlElement("SOD_REQUIRED_QTY")]
        public double SOD_REQUIRED_QTY { get; set; }

        [XmlElement("GRP_AVAILABLE_QTY")]
        public double GRP_AVAILABLE_QTY { get; set; }
        [XmlElement("GRP_ALLOCATED_QTY")]
        public double GRP_ALLOCATED_QTY { get; set; }
        [XmlElement("SOD_BAL_TO_ALLOCATE")]
        public double SOD_BAL_TO_ALLOCATE { get; set; }

        [XmlElement("ISD_SIZE_SEQUENCE")]
        public int ISD_SIZE_SEQUENCE { get; set; }

        public int ITM_CHECKED { get; set; }

        [XmlElement("SOD_BRAND_CODE")]
        public string SOD_BRAND_CODE { get; set; }

        [XmlElement("SOD_BRAND_NAME")]
        public string SOD_BRAND_NAME { get; set; }

        [XmlElement("SOD_BRAND_PK")]
        public int SOD_BRAND_PK { get; set; }



    }

    [Serializable]
    public class PlanItemGroup
    {
        [XmlElement("PIG_PK")]
        public int PIG_PK { get; set; }
        [XmlElement("PIG_NAME")]
        public string PIG_NAME { get; set; }
    }
    [Serializable]
    public class BizUnit
    {
        [XmlElement("BZU_PK")]
        public int BZU_PK { get; set; }
        [XmlElement("BZU_CODE")]
        public string BZU_CODE { get; set; }
    }
    [Serializable]
    public class SaleOrder
    {
        [XmlElement("SOH_PK")]
        public int SOH_PK { get; set; }
        [XmlElement("SOH_NO")]
        public string SOH_NO { get; set; }
    }
    [Serializable]
    public class Customer
    {
        [XmlElement("CUS_PK")]
        public int CUS_PK { get; set; }
        [XmlElement("CUS_CODE")]
        public string CUS_CODE { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class Params
    {
        [XmlElement("Parameters")]
        public List<Parameters> Parameters { get; set; }
    }

    [Serializable]
    public class Parameters
    {
        [XmlElement("ParamName")]
        public string ParamName { get; set; }
        [XmlElement("Value")]
        public string Value { get; set; }
    }
    #endregion

    #region PRODUCT PROPERTY
    [Serializable]
    [XmlRoot("Root")]
    public class ProductPropertyBO
    {
        [XmlElement("ItemSpecGroup")]
        public List<ItemSpecGroup> ItemSpecGroup { get; set; }
    }

    [Serializable]
    public class ItemSpecGroup
    {
        [XmlElement("CNG_PK")]
        public int CNG_PK { get; set; }
        [XmlElement("CNG_NAME")]
        public string CNG_NAME { get; set; }
        [XmlElement("CNG_CODE")]
        public string CNG_CODE { get; set; }

        [XmlElement("ItemSpecDetails")]
        public List<ItemSpecDetails> ItemSpecDetails { get; set; }
    }

    [Serializable]
    public class ItemSpecDetails
    {

        [XmlElement("CON_PK")]
        public int CON_PK { get; set; }
        [XmlElement("CON_NAME")]
        public string CON_NAME { get; set; }
        [XmlElement("CON_CODE")]
        public string CON_CODE { get; set; }
        [XmlElement("CNG_PK")]
        public int CNG_PK { get; set; }

    }
    #endregion


    [Serializable]
    [XmlRoot("Root")]
    public class OrderPlanBO
    {
        [XmlElement("PNH_PK")]
        public int PNH_PK { get; set; }
        [XmlElement("PNH_CODE")]
        public string PNH_CODE { get; set; }
        [XmlElement("PNH_NAME")]
        public string PNH_NAME { get; set; }
        [XmlElement("PNH_FROM_DT")]
        public string PNH_FROM_DT { get; set; }
        [XmlElement("PNH_TO_DT")]
        public string PNH_TO_DT { get; set; }
        [XmlElement("BIH_DEPT")]
        public int BIH_DEPT { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public string LAST_MOD_DT { get; set; }
        [XmlElement("PNH_TAB")]
        public int PNH_TAB { get; set; }
        [XmlElement("PNH_VERSION")]
        public string PNH_VERSION { get; set; }
        [XmlElement("ISD_AGRADE_PER")]
        public double ISD_AGRADE_PER { get; set; }
        [XmlElement("IS_REVISE")]
        public int IS_REVISE { get; set; }
        [XmlElement("PNH_IS_FINALIZED")]
        public int PNH_IS_FINALIZED { get; set; }
        [XmlElement("DEPT_PK")]
        public int DEPT_PK { get; set; }
        [XmlElement("PNH_IS_VER_MODIFIED")]
        public int PNH_IS_VER_MODIFIED { get; set; }
        [XmlElement("PLAN_MODE")]
        public int PLAN_MODE { get; set; }
        [XmlElement("PLAN_OPTIMIZE")]
        public int PLAN_OPTIMIZE { get; set; }
        [XmlElement("PLAN_SPLIT")]
        public int PLAN_SPLIT { get; set; }

        [XmlElement("Detail")]
        public List<OrderDetails> Details { get; set; }
    }


    [Serializable]
    public class OrderDetails
    {
        [XmlElement("PND_PK")]
        public int PND_PK { get; set; }
        [XmlElement("SOD_NO")]
        public string SOD_NO { get; set; }
        [XmlElement("SOH_CUSTOMER")]
        public string SOH_CUSTOMER { get; set; }
        [XmlElement("SOH_CUSTOMER_CODE")]
        public string SOH_CUSTOMER_CODE { get; set; }
        [XmlElement("SOH_CUSTOMER_NAME")]
        public string SOH_CUSTOMER_NAME { get; set; }



        [XmlElement("PND_PLAN_GROUP")]
        public int PND_PLAN_GROUP { get; set; }
        [XmlElement("PND_PLAN_GROUP_TEXT")]
        public string PND_PLAN_GROUP_TEXT { get; set; }
        [XmlElement("PND_REQUIRED_DT")]
        public string PND_REQUIRED_DT { get; set; }
        [XmlElement("PND_PLAN_QTY")]
        public double PND_PLAN_QTY { get; set; }
        [XmlElement("PND_SL_NO")]
        public int PND_SL_NO { get; set; }
        [XmlElement("PND_SIZE")]
        public int PND_SIZE { get; set; }
        [XmlElement("ISD_SIZE_TEXT")]
        public string ISD_SIZE_TEXT { get; set; }
        [XmlElement("SOD_TOT_QTY_PLANNED")]
        public double SOD_TOT_QTY_PLANNED { get; set; }
        [XmlElement("SOD_TOT_BAL_TO_PLAN")]
        public double SOD_TOT_BAL_TO_PLAN { get; set; }
        [XmlElement("PND_LINE_PLANNED")]
        public double PND_LINE_PLANNED { get; set; }
        [XmlElement("PND_IS_CHECKED")]
        public int PND_IS_CHECKED { get; set; }
        [XmlElement("ISD_AGRADE_PER")]
        public double ISD_AGRADE_PER { get; set; }
        [XmlElement("PND_REQUIRED_QTY")]
        public double PND_REQUIRED_QTY { get; set; }
        [XmlElement("SOD_QTY_ALLOCATED")]
        public double SOD_QTY_ALLOCATED { get; set; }
        [XmlElement("SOH_NO")]
        public string SOH_NO { get; set; }
        [XmlElement("GRP_AVAILABLE_QTY")]
        public double GRP_AVAILABLE_QTY { get; set; }
        [XmlElement("GRP_ALLOCATED_QTY")]
        public double GRP_ALLOCATED_QTY { get; set; }
        [XmlElement("SOD_BAL_TO_ALLOCATE")]
        public double SOD_BAL_TO_ALLOCATE { get; set; }

        [XmlElement("SOD_ITEM_CODE")]
        public string SOD_ITEM_CODE { get; set; }
        [XmlElement("SOD_ITEM_TEXT")]
        public string SOD_ITEM_TEXT { get; set; }


        [XmlElement("SOD_PK")]
        public int SOD_PK { get; set; }
        [XmlElement("WIP_ALLOC_QTY")]
        public double WIP_ALLOC_QTY { get; set; }
        [XmlElement("ITM_PLAN_GROUP")]
        public int ITM_PLAN_GROUP { get; set; }
        [XmlElement("ITM_SIZE")]
        public int ITM_SIZE { get; set; }

        [XmlElement("ISD_SIZE_SEQUENCE")]
        public int ISD_SIZE_SEQUENCE { get; set; }

        [XmlElement("LINE_COMBINED")]
        public int LINE_COMBINED { get; set; }

        [XmlElement("PND_DDL_MET")]
        public string PND_DDL_MET { get; set; }
        [XmlElement("PND_LINE_TEXT")]
        public string PND_LINE_TEXT { get; set; }

        [XmlElement("SizeDetail")]
        public List<SizeDetails> SizeDetail { get; set; }

        [XmlElement("LineDetail")]
        public List<LineDetails> LineDetails { get; set; }

        [XmlElement("FilteredLines")]
        public List<FilteredLines> FilteredLines { get; set; }
    }

    [Serializable]
    public class FilteredLines
    {
        [XmlElement("LNE_PK")]
        public int LNE_PK { get; set; }
        [XmlElement("PND_SL_NO")]
        public int PND_SL_NO { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class OrderAllocationBO
    {
        [XmlElement("PNH_PK")]
        public int PNH_PK { get; set; }
        [XmlElement("PNH_CODE")]
        public string PNH_CODE { get; set; }
        [XmlElement("PNH_NAME")]
        public string PNH_NAME { get; set; }
        [XmlElement("PNH_FROM_DT")]
        public string PNH_FROM_DT { get; set; }
        [XmlElement("PNH_TO_DT")]
        public string PNH_TO_DT { get; set; }
        [XmlElement("BIH_DEPT")]
        public int BIH_DEPT { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public string LAST_MOD_DT { get; set; }
        [XmlElement("PNH_TAB")]
        public int PNH_TAB { get; set; }
        [XmlElement("PNH_VERSION")]
        public string PNH_VERSION { get; set; }
        [XmlElement("ISD_AGRADE_PER")]
        public string ISD_AGRADE_PER { get; set; }
        [XmlElement("IS_REVISE")]
        public int IS_REVISE { get; set; }
        [XmlElement("PNH_IS_FINALIZED")]
        public int PNH_IS_FINALIZED { get; set; }
        [XmlElement("DEPT_PK")]
        public int DEPT_PK { get; set; }
        [XmlElement("PNH_IS_VER_MODIFIED")]
        public int PNH_IS_VER_MODIFIED { get; set; }

        [XmlElement("Detail")]
        public List<OrderAllocationDetails> Details { get; set; }
    }

    [Serializable]
    public class OrderAllocationDetails
    {
        [XmlElement("PND_PK")]
        public int PND_PK { get; set; }
        [XmlElement("PND_PLAN_GROUP")]
        public int PND_PLAN_GROUP { get; set; }
        [XmlElement("PND_PLAN_GROUP_TEXT")]
        public string PND_PLAN_GROUP_TEXT { get; set; }
        [XmlElement("PND_REQUIRED_DT")]
        public string PND_REQUIRED_DT { get; set; }
        [XmlElement("PND_PLAN_QTY")]
        public double PND_PLAN_QTY { get; set; }
        [XmlElement("PND_SL_NO")]
        public int PND_SL_NO { get; set; }
        [XmlElement("PND_SIZE")]
        public int PND_SIZE { get; set; }
        [XmlElement("ISD_SIZE_TEXT")]
        public string ISD_SIZE_TEXT { get; set; }
        [XmlElement("SOD_TOT_QTY_PLANNED")]
        public double SOD_TOT_QTY_PLANNED { get; set; }
        [XmlElement("SOD_TOT_BAL_TO_PLAN")]
        public double SOD_TOT_BAL_TO_PLAN { get; set; }
        [XmlElement("PND_LINE_PLANNED")]
        public double PND_LINE_PLANNED { get; set; }
        [XmlElement("PND_IS_CHECKED")]
        public int PND_IS_CHECKED { get; set; }
        [XmlElement("ISD_AGRADE_PER")]
        public string ISD_AGRADE_PER { get; set; }
        [XmlElement("PND_REQUIRED_QTY")]
        public double PND_REQUIRED_QTY { get; set; }
        [XmlElement("SOD_QTY_ALLOCATED")]
        public double SOD_QTY_ALLOCATED { get; set; }
        [XmlElement("SOH_NO")]
        public string SOH_NO { get; set; }
        [XmlElement("GRP_AVAILABLE_QTY")]
        public double GRP_AVAILABLE_QTY { get; set; }
        [XmlElement("GRP_ALLOCATED_QTY")]
        public double GRP_ALLOCATED_QTY { get; set; }
        [XmlElement("SOD_BAL_TO_ALLOCATE")]
        public double SOD_BAL_TO_ALLOCATE { get; set; }

        [XmlElement("SOD_ITEM_CODE")]
        public string SOD_ITEM_CODE { get; set; }
        [XmlElement("SOD_ITEM_TEXT")]
        public string SOD_ITEM_TEXT { get; set; }


        [XmlElement("SOD_PK")]
        public int SOD_PK { get; set; }
        [XmlElement("WIP_ALLOC_QTY")]
        public double WIP_ALLOC_QTY { get; set; }
        [XmlElement("ITM_PLAN_GROUP")]
        public int ITM_PLAN_GROUP { get; set; }
        [XmlElement("ITM_SIZE")]
        public int ITM_SIZE { get; set; }

        [XmlElement("ISD_SIZE_SEQUENCE")]
        public int ISD_SIZE_SEQUENCE { get; set; }

        [XmlElement("SODetail")]
        public List<SODetails> SODetails { get; set; }

        [XmlElement("LineDetail")]
        public List<LineDetails> LineDetails { get; set; }
    }

    [Serializable]
    public class SizeDetails
    {
        [XmlElement("PNS_SIZE")]
        public int PNS_SIZE { get; set; }
        [XmlElement("SOD_BRAND_PK")]
        public int SOD_BRAND_PK { get; set; }
        [XmlElement("SOD_BRAND_CODE")]
        public string SOD_BRAND_CODE { get; set; }
        [XmlElement("SOD_BRAND_NAME")]
        public string SOD_BRAND_NAME { get; set; }
        [XmlElement("SOD_BRAND_SIZE")]
        public string SOD_BRAND_SIZE { get; set; }

        [XmlElement("SOD_BRAND_SIZE_NAME")]
        public string SOD_BRAND_SIZE_NAME { get; set; }



        [XmlElement("PNS_SIZE_TEXT")]
        public string PNS_SIZE_TEXT { get; set; }
        [XmlElement("SIZE_PLAN_GROUP")]
        public int SIZE_PLAN_GROUP { get; set; }
        [XmlElement("PND_SL_NO")]
        public int PND_SL_NO { get; set; }
        [XmlElement("SOD_QTY_PLANNED")]
        public double SOD_QTY_PLANNED { get; set; }
        [XmlElement("SOD_BAL_TO_PLAN")]
        public double SOD_BAL_TO_PLAN { get; set; }
        [XmlElement("SIZE_REQUIRED_DT")]
        public string SIZE_REQUIRED_DT { get; set; }
        [XmlElement("SIZE_PLAN_QTY")]
        public double SIZE_PLAN_QTY { get; set; }

        [XmlElement("SODetail")]
        public List<SODetails> SODetails { get; set; }
    }


    [Serializable]
    public class SODetails
    {
        [XmlElement("PNS_PK")]
        public int PNS_PK { get; set; }
        [XmlElement("PNS_PLAN_TRX_DTL")]
        public int PNS_PLAN_TRX_DTL { get; set; }
        [XmlElement("PNS_SO_DTL")]
        public int PNS_SO_DTL { get; set; }
        [XmlElement("SOH_PK")]
        public int SOH_PK { get; set; }
        [XmlElement("SOH_NO")]
        public string SOH_NO { get; set; }
        [XmlElement("PNS_REQUIRED_DT")]
        public string PNS_REQUIRED_DT { get; set; }
        [XmlElement("PNS_ITEM")]
        public int PNS_ITEM { get; set; }
        [XmlElement("SOD_QTY")]
        public double SOD_QTY { get; set; }
        [XmlElement("SOD_QTY_DISPATCHED")]
        public double SOD_QTY_DISPATCHED { get; set; }
        [XmlElement("SOD_QTY_ALLOCATED")]
        public double SOD_QTY_ALLOCATED { get; set; }
        [XmlElement("SOD_QTY_PRODUCED")]
        public double SOD_QTY_PRODUCED { get; set; }
        [XmlElement("SOD_QTY_PLANNED")]
        public double SOD_QTY_PLANNED { get; set; }
        [XmlElement("PNS_PLAN_QTY")]
        public double PNS_PLAN_QTY { get; set; }
        [XmlElement("SOD_BAL_TO_PLAN")]
        public double SOD_BAL_TO_PLAN { get; set; }
        [XmlElement("PNS_PLAN_GROUP")]
        public int PNS_PLAN_GROUP { get; set; }
        [XmlElement("PND_SL_NO")]
        public int PND_SL_NO { get; set; }
        [XmlElement("SOD_BAL_TO_ALLOCATE")]
        public double SOD_BAL_TO_ALLOCATE { get; set; }
        [XmlElement("ISD_SIZE_TEXT")]
        public string ISD_SIZE_TEXT { get; set; }
        [XmlElement("PNS_SIZE")]
        public int PNS_SIZE { get; set; }
        [XmlElement("SOD_PERCENTAGE")]
        public double SOD_PERCENTAGE { get; set; }
        public double Percentage { get; set; }
    }

    [Serializable]
    public class LineDetails
    {
        [XmlElement("PNL_PK")]
        public int PNL_PK { get; set; }
        [XmlElement("PNL_PLAN_TRX_DTL")]
        public int PNL_PLAN_TRX_DTL { get; set; }
        [XmlElement("PNL_LINE")]
        public int PNL_LINE { get; set; }
        [XmlElement("LNE_NAME")]
        public string LNE_NAME { get; set; }
        [XmlElement("LNE_CODE")]
        public string LNE_CODE { get; set; }
        [XmlElement("PNL_PLAN_QTY")]
        public double PNL_PLAN_QTY { get; set; }
        [XmlElement("PNL_PLAN_GROUP")]
        public int PNL_PLAN_GROUP { get; set; }
        [XmlElement("LNE_PLANT_NAME")]
        public string LNE_PLANT_NAME { get; set; }
        [XmlElement("LNE_CAPACITY")]
        public double LNE_CAPACITY { get; set; }
        [XmlElement("PND_SL_NO")]
        public int PND_SL_NO { get; set; }
        //[XmlElement("SLNO")]
        public int SLNO { get; set; }
        [XmlElement("PNL_FROM_DATE")]
        public string PNL_FROM_DATE { get; set; }
        [XmlElement("PNL_TO_DATE")]
        public string PNL_TO_DATE { get; set; }
        [XmlElement("PNL_LNE_AVG_SPEED")]
        public double PNL_LNE_AVG_SPEED { get; set; }
        [XmlElement("PNL_DDL_MET")]
        public int PNL_DDL_MET { get; set; }

        [XmlElement("PRD_HRS")]
        public int PRD_HRS { get; set; }
        [XmlElement("REPAIR_HRS")]
        public double REPAIR_HRS { get; set; }

        [XmlElement("LineFormerDetail")]
        public List<LineFormerDetail> LineFormerDetail { get; set; }
    }

    [Serializable]
    public class LineFormerDetail
    {
        [XmlElement("PLD_SIZ_PK")]
        public string PLD_SIZ_PK { get; set; }
        [XmlElement("PLD_SIZ_TEXT")]
        public string PLD_SIZ_TEXT { get; set; }
        [XmlElement("PLD_FORMER_QTY")]
        public double PLD_FORMER_QTY { get; set; }
        [XmlElement("PLD_PRODUCTION_QTY")]
        public double PLD_PRODUCTION_QTY { get; set; }
        [XmlElement("PNL_SL_NO")]
        public int PNL_SL_NO { get; set; }
        [XmlElement("PNF_PK")]
        public int PNF_PK { get; set; }
        [XmlElement("PND_SL_NO")]
        public int PND_SL_NO { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class DeAllocationBO
    {
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("DEPT_PK")]
        public int DEPT_PK { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public string LAST_MOD_DT { get; set; }

        [XmlElement("Detail")]
        public List<DeAllocationDetails> Detail { get; set; }
    }
    [Serializable]
    public class DeAllocationDetails
    {
        [XmlElement("SOD_PK")]
        public string SOD_PK { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class ReleaseBO
    {
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("DEPT_PK")]
        public int DEPT_PK { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public string LAST_MOD_DT { get; set; }

        [XmlElement("Detail")]
        public List<ReleaseDetails> Detail { get; set; }
    }
    [Serializable]
    public class ReleaseDetails
    {
        [XmlElement("SOD_PK")]
        public string SOD_PK { get; set; }
    }


    [Serializable]
    [XmlRoot("Root")]
    public class LineBO
    {
        [XmlElement("Detail")]
        public List<PlanGroupDetails> Detail { get; set; }
    }

    [Serializable]
    public class PlanGroupDetails
    {
        [XmlElement("PND_PLAN_GROUP")]
        public int PND_PLAN_GROUP { get; set; }
        [XmlElement("PND_SL_NO")]
        public int PND_SL_NO { get; set; }
        [XmlElement(" LINE_IS_COMPATIBLE")]
        public int LINE_IS_COMPATIBLE { get; set; }


        [XmlElement("SODetail")]
        public List<PlanGroupSODetails> SODetail { get; set; }

        [XmlElement("LineDetail")]
        public List<PlanGroupLineDetail> LineDetail { get; set; }
    }

    [Serializable]
    public class PlanGroupSODetails
    {
        [XmlElement("PNS_SO_DTL")]
        public int PNS_SO_DTL { get; set; }
        [XmlElement("PNS_SL_NO")]
        public int PNS_SL_NO { get; set; }
    }

    [Serializable]
    public class PlanGroupLineDetail
    {
        [XmlElement("PNS_LNE_PK")]
        public int PNS_LNE_PK { get; set; }
        [XmlElement("LNE_CODE")]
        public string LNE_CODE { get; set; }
        [XmlElement("ISCHECKED")]
        public int Ischecked { get; set; }
    }


    [Serializable]
    [XmlRoot("Root")]
    public class PLanLineBO
    {
        [XmlElement("PNH_FROM_DT")]
        public string PNH_FROM_DT { get; set; }
        [XmlElement("PNL_FROM_DATE")]
        public string PNL_FROM_DATE { get; set; }
        [XmlElement("ITEM_GP")]
        public int ITEM_GP { get; set; }
        [XmlElement("LNE_PK")]
        public int LNE_PK { get; set; }
        [XmlElement("LNE_ACTIVE")]
        public int LNE_ACTIVE { get; set; }
        [XmlElement("BIZUNIT")]
        public int BIZUNIT { get; set; }
        [XmlElement("PND_PK")]
        public int PND_PK { get; set; }
        [XmlElement("TOTAL_QTY")]
        public double TOTAL_QTY { get; set; }
        [XmlElement("AVG_SPEED")]
        public double AVG_SPEED { get; set; }
        [XmlElement("LNE_TOT_FORMER_HOLDER_POS")]
        public double LNE_TOT_FORMER_HOLDER_POS { get; set; }
        [XmlElement("LNE_FORMER_FROM_DATE")]
        public string LNE_FORMER_FROM_DATE { get; set; }
        [XmlElement("LNE_FORMER_TO_DATE")]
        public string LNE_FORMER_TO_DATE { get; set; }

        [XmlElement("LNE_CODE")]
        public string LNE_CODE { get; set; }
        [XmlElement("LNE_NAME")]
        public string LNE_NAME { get; set; }
        [XmlElement("LNE_PLANT_NAME")]
        public string LNE_PLANT_NAME { get; set; }
        [XmlElement("LNE_CAPACITY")]
        public double LNE_CAPACITY { get; set; }


        [XmlElement("PRD_HRS")]
        public int PRD_HRS { get; set; }
        [XmlElement("REPAIR_HRS")]
        public int REPAIR_HRS { get; set; }

        [XmlElement("Detail")]
        public List<PLanLineDetails> Detail { get; set; }
    }

    [Serializable]
    public class PLanLineDetails
    {
        [XmlElement("PNS_PLAN_QTY")]
        public double PNS_PLAN_QTY { get; set; }
        [XmlElement("PLD_SIZ_PK")]
        public int PLD_SIZ_PK { get; set; }

        [XmlElement("PLD_FORMER_QTY")]
        public double PLD_FORMER_QTY { get; set; }
        [XmlElement("PLD_SIZ_TEXT")]
        public string PLD_SIZ_TEXT { get; set; }
        [XmlElement("PND_TOTAL_QTY")]
        public double PND_TOTAL_QTY { get; set; }
        [XmlElement("PLD_PRODUCTION_QTY")]
        public double PLD_PRODUCTION_QTY { get; set; }
        [XmlElement("LNE_FORMER_FROM_DATE")]
        public string LNE_FORMER_FROM_DATE { get; set; }
        [XmlElement("LNE_FORMER_TO_DATE")]
        public string LNE_FORMER_TO_DATE { get; set; }

    }


    public class GridPram
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string FilterStatus { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Fields { get; set; }
        public string SortBy { get; set; }
        public string ThenBy { get; set; }
        public string SortDirection { get; set; }
        public string ThenDirection { get; set; }
        public string SearchBy { get; set; }
        public string SearchValue { get; set; }
        public int Flag { get; set; }
        public int UserPK { get; set; }
        public int DeptPK { get; set; }
        public int P_DeptPK { get; set; }
        public int CancelFlag { get; set; }
        public int ShowAll { get; set; }

    }
}
