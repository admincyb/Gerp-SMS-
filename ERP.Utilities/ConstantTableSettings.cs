using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Utilities
{
    /// <summary>
    /// App Constant Setting Values
    /// </summary>
    public class ConstantTableSettings
    {
        public const string ContractDetailType = "CONTRACT DTL TYPE";
        public const string ComputationType = "COMPUTATION TYPE";
        public const string TaxBasis = "TAX BASIS";
        public const string ContractOccurType = "CONTRACT OCCUR TYPE";
        public const string ActivityOccurType = "ACTIVITY OCCURANCE";
        public const string ContractHdrType = "CONTRACT HDR TYPE";
        public const string ContractLogDateType = "CONTRACT DATE TYPE";
        public const string CurrencyDecimal = "CURRENCY DECIMAL";
        public const string CurrencySeperation = "CURRENCY SEPARATION";
        public const string ContractTaskStatus = "CONTRACT TASK STATUS";
        public const string TypeMaster = "TYPE MASTER";
        //ACTIVITY
        public const string ActivityFieldToggle = "ACTIVITY FIELD TOGGLE";
        public static string MANPOWERTEXT="Manpower";
        public static string CumulativeType = "CUMULATIVE TYPE";

        //Eval Key Settings
        public const string EvalCode = "EVAL CODE";
        public const string EvalKeys = "EVAL KEY";
        public const string EvalPeriod = "EVAL PERIOD";
        public const string EvalDepts = "EVAL DEPARTMENTS";
        public const string EvalUsers = "EVAL USERS";

        //Activity Services
        public static byte CATERINGUPLIFT = 0;
        public static byte MANPOWER = 1;
        public static byte ROOM = 2;
        
        
    }

    /// <summary>
    /// Activity Fields Enable/Disable
    /// </summary>
    public class ActivityFieldToggle  
    {
        public const string ActivityNumber = "ACTIVITY NUMBER";
        public const string ActivityDate = "ACTIVITY DATE";
        public const string ActivityArrivalFlightNo = "ACTIVITY ARRIVAL FLIGHT NO";
        public const string ActivityDepartureFlightNo  = "ACTIVITY DEPARTURE FLIGHT NO";
        public const string ActivitySTA = "ACTIVITY STA";
        public const string ActivitySTD = "ACTIVITY STD";
    }
    /// <summary>
    /// Contract Details Type Enum
    /// </summary>
    public enum ContractDetailsEnum
    {
        BASIS = 1,
        SLAB
    }
    /// <summary>
    /// Contract Header Type Enum
    /// </summary>
    public enum ContractHdrTypesEnum
    {
        ACTIVITY = 1,
        FLIGHTINFO = 11,
        FUEL = 21,
        CATERING = 31,
        MANPOWER = 41,
        ROOMTYPE = 51
    }

    public enum ActivityMandatoryFieldPagesEnum
    {
        INVALIDAIRCRAFT=-2, 
        ACTIVITYINFO = 0,
        GENERAL = 1,
        FLIGHTINFO = 2,
        FUEL = 3

    }

    public enum ActivityChargeCalcTypeEnum
    {
        REGULAR = 0,
        YEAR = 1,
        HALFYEAR = 2,
        QUARTER = 3,
        MONTH = 4,
        WEEK = 5,
        DAY = 6,
        HOUR = 7,
        MINUTE = 8

    }

    public enum ContractLogDateTypesEnum
    {
        LOPOFF = 1,
        EXTEND
    }
    public class KeyEvaluation
    {
        public const string EVALUATION = "Evaluation";
        public const string PURCHASE = "Purchase";
        public const string INITIALEVALKEY = "GTIGAR#GTIGAR";
    }

    public enum ServiceType
    {
        MANPOWER = 1,
        ROOMS = 2,
        CATERING = 3,
        Transportation = 4
    }

}
