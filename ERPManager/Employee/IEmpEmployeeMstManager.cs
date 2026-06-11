using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPData;

namespace ERPManager.Employee
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEmpEmployeeMstManager" in both code and config file together.
    [ServiceContract]
    public interface IEmpEmployeeMstManager
    {
        int SaveEmpEmployeeMst(List<EmpEmployeeMst> admCurrencyMstObj);
        int DeleteEmpEmployeeMst(List<EmpEmployeeMst> admCurrencyMstObj);
        List<EmpEmployeeMst> GetEmpEmployeeMst(EmpEmployeeMst admCurrencyMstObj, ServiceUtility utilityObj);
        ServiceUtility GetEmpEmployeeMstCount(EmpEmployeeMst admCurrencyMstObj, ServiceUtility utilityObj);
        EmpEmployeeMst GetInitilizedEmpEmployeeMst();
        List<EmpEmployeeMst> GetEmpEmployeeMstAutoCompleteList(EmpEmployeeMst admCurrencyMstObj, ServiceUtility utilityObj);
        List<EmpEmployeeMst> GetNonUserEmpEmployeeMstAutoCompleteList(EmpEmployeeMst empEmployeeMstObj, ServiceUtility utilityObj);
        List<XacEmpType> GetXacEmpType(XacEmpType xacEmpTypeObj);
    }
}
