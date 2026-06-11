using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.HRMS.Employee
{
    public enum ActionsEnum
    {
        LIST,
        DETAIL,
        DEFAULT,
        EMPDOCLIST,
        EMPDOCDETAIL,
        NEW,
        EDIT,
        SAVE,
        SAVEANDCONTINUE,
        ADDITEM,
        REMOVEITEM,
        EDITITEM,
        ITEMSELECTED,
        SHOWLOG,
        CHECKIN,
        CHECKOUT,
        VIEW,
        CANCEL,
        FILTER,
        CLEARSEARCH,
        //Tab Control Actions
        BASICDETAILS,
        QUALIFICATIONS,
        EXPERIENCE,
        DOCUMENTS,
        PAYDETAILS,
        SKILLDETAILS,
        SALARYDETAILS,
        ITDECLARATION,
        CHECKOUTERROR,
        CHECKINERROR,
        CHECKINLOGCLOSE,
        CHECKOUTLOGCLOSE,
        GOHOME,
        CLEAR,
        DELETE,
        BRANCHCHANGED,
        SELECTEDINDEXCHANGED,
        WORKINGHOURS,
        ACTIVATE,
        DEACTIVATE,
        EARNINGS,
        DEDUCTIONS,
        EMPLOYEESALARY,
        SALARYREVISION,
        EARNINGSPAYELEMENTS,
        DEDUCTIONPAYELEMENTS,
        BROWSE,
        SELECT,
        FOLDERCREATED,
        FOLDERMODIFIED,
        FOLDERDELETED,
        MAPPING,
        ASSAIGN,
        USERMAPPED,
        ERROR,
        LEAVETYPE,
        PAYELEMENTDETAILS,
        SALARYTEMPLATEDETAILS,
        PAYELEMENTSLABCUSTOM,
        BEHAVIOUR,
        PREPROCESSDATA,
        SALARYPROCESS,
        TRAINING,
        SEARCH,
        DELETEITEM,
        CLEARADD,
        SAVESUBMIT,
        SUBMIT,
        WRKFSUBMIT,
        EDITFORCANCEL,
        DELETESUBMIT,
        PRINTMULTIPLE,
        APPLY,
        POPUPCANCEL
    } 

    public enum CheckStateStatus
    {
        CheckedOut = 0,
        CheckedIn = 1,
        OriginalNotSubmitted = 2
    }

    public enum CommonStatus
    {
        Inactive = 0,
        Active = 1,
    }

    public enum EmployeeDocumentAction
    {
        CheckIn = 1, // Checkin is the Default Item for Issued Dropdown, So the Order is important
        CheckOut = 0,
    }

    public enum SearchMode
    {
        BASIC,
        ADVANCED
    }

    public enum DeductMode
    {
        Earn = 0,
        Deduct = 1
    }

    public enum EmpSalaryAction
    {
        View = 0,
        Edit = 1
    }
}
