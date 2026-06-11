using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessObject.ILibrary
{
    public interface IInterface
    {
        DataSet FetchDbValues(int val, string[] XML);
        void SetFieldValues(ControlEnum Type);

        void GetFieldValues(ControlEnum Type);
        DataSet SetValuesToDB(int val, string[] XML);

        void ActionHandler(object sender, EventArgs e);

    }
    public enum ControlEnum
    {
        GRID,
        CHART,
        DDL,
        PLANNED,
        FILTER,
        ALLOCATION,
        VALIDATE,
        CANCEL,
        FINISHEDGOODS,
        SELECTEDORDERS,
        DBMSG,
        ALLOCATED,
        ORDERDETAILS,
        PLANNING,
        LISTING,
        CLEAR,
        NEXT,
        SEARCH,
        LIST,

        //Planning
        PLANORDERS,
        PLANORDERDETAILS,
        ALLOCATIONACTIVITY,
        ALLOCATIONPRODUCT,
        FORMERS,
        FORMERACTIVITY,
        RULES,

        //Allocation
        ACTIVITY,
        LOADUNLOAD,
        FORMERTYPE,
        PRODUCT,
        FORMERALOCATION,
        FORMER_ACTIVITY_EDIT,
        FORMER_CONFIG,

        //
        LINERADAR,
        LINEGRAPH,
        REPORT,

        //Shift Report
        HEADER_DDL,
        HEADER,
        LINE_DDL,
        SHIFT_DETAILS,
        EDIT_SHIFT_DETAILS,
        SHIFT,

        //Despatch
        DESPATCH,
        CUSTOMER,
        SALEORDER,
        SALEORDERITEM,
        UOM,
        EDIT_DISPATCH_DETAILS,
        DISPATCH_FROM_ORDER,

        //Packing
        PACKINGDETAILS,

        //Customer Orders
        CUSTOMERS,
        CUSTOMERSORDERS,
        DESPATCHED,
        ORDERS,
        CUSTOMERORDERHEADER,
        SKU,
        SIZE,

        //Line Tracking
        PLANNEDLINES,
        LINESACTIVITIES,
        FORMERPRODUCTION,

        //Quick Planning
        QUICKPLANNING,
        SHOWLINE,
        LINEPLAN,
        RECALCULATE,
        SAVE,
        SHOWPLAN,
        LINE,
        PROCEED,
        DELETE,
        SHOWFORMERS,
        REFRESH,
        FREEZE,
        APPLYINFODTL,
        FILTERLINE

    }
}