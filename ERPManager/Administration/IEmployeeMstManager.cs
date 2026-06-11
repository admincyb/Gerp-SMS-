using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;

namespace ERPManager
{
    public interface IEmployeeMstManager
    {
        List<EmpEmployeeMst> GetEmployeeMst(EmpEmployeeMst EmployeeMstObj, ServiceUtility utilityObj);
        int DeleteEmployeeMst(List<EmpEmployeeMst> EmployeeMstList);
        ServiceUtility GetEmployeeMstCount(EmpEmployeeMst EmployeeMstObj, ServiceUtility utilityObj);
        int SaveEmployeeMst(List<EmpEmployeeMst> EmployeeMstList);
        List<EmpEmployeeMst> GetEmployeeMstAutoCompleteList(EmpEmployeeMst EmployeeMstObj, ServiceUtility utilityObj);
    }
}
