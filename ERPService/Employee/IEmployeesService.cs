using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPData;
using ERPManager;

namespace ERPService.Employee
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEmployeesService" in both code and config file together.
    [ServiceContract]
    public interface IEmployeesService
    {
        [OperationContract]
        int SaveEmpEmployeeMst(List<EmpEmployeeMst> EmpEmployeeMstList);

        [OperationContract]
        int DeleteEmpEmployeeMst(List<EmpEmployeeMst> EmpEmployeeMstList);

        [OperationContract]
        List<EmpEmployeeMst> GetEmpEmployeeMst(EmpEmployeeMst EmpEmployeeMstObj, ServiceUtility utilityObj = null);

        [OperationContract]
        ServiceUtility GetEmpEmployeeMstCount(EmpEmployeeMst EmpEmployeeMstObj, ServiceUtility utilityObj = null);

        [OperationContract]
        ServiceUtility GetEmpEmployesCount(EmpEmployeeMst EmpEmployeeMstObj, ServiceUtility utilityObj = null);

        [OperationContract]
        EmpEmployeeMst GetInitilizedEmpEmployeeMst();

        [OperationContract]
        List<EmpEmployeeMst> GetEmpEmployeeMstAutoCompleteList(EmpEmployeeMst empEmployeeMstObj, ServiceUtility utilityObj);

        [OperationContract]
        List<EmpEmployeeMst> GetNonUserEmpEmployeeMstAutoCompleteList(EmpEmployeeMst empEmployeeMstObj, ServiceUtility utilityObj);

        [OperationContract]
        List<XacEmpType> GetXacEmpType(XacEmpType xacEmpTypeObj);
    }
}
