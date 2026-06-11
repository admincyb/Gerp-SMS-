using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPData;
using ERPManager;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEmployeeService" in both code and config file together.
    [ServiceContract]
    public interface IEmployeeService
    {
        [OperationContract]
        List<EmpEmployeeMst> GetEmployeeMst(EmpEmployeeMst EmployeeMstObj, ServiceUtility utilityObj);
        [OperationContract]
        int DeleteEmployeeMst(List<EmpEmployeeMst> EmployeeMstList);
        [OperationContract]
        ServiceUtility GetEmployeeMstCount(EmpEmployeeMst EmployeeMstObj, ServiceUtility utilityObj);
        [OperationContract]
        int SaveEmployeeMst(List<EmpEmployeeMst> EmployeeMstList);
        [OperationContract]
        List<EmpEmployeeMst> GetEmployeeMstAutoCompleteList(EmpEmployeeMst EmployeeMstObj, ServiceUtility utilityObj);
    }
}
