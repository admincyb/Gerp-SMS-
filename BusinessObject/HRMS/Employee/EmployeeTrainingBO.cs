using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Employee
{
  public  class EmployeeTrainingBO
    {
    }
  [Serializable]
  [XmlRoot("Root")]
  public class EmployeeTrainingHeader
  {
      [XmlElement("ETA_PK")]
      public int ETA_PK { get; set; }
      [XmlElement("ETA_NO")]
      public string ETA_NO { get; set; }
      [XmlElement("ETA_DATE")]
      public DateTime ETA_DATE { get; set; }
      [XmlElement("ETA_TO_DATE")]
      public string ETA_TO_DATE { get; set; }
      [XmlElement("ETA_FROM_DATE")]
      public string ETA_FROM_DATE { get; set; }
      [XmlElement("ETA_PLACE")]
      public string ETA_PLACE { get; set; }
      [XmlElement("ETA_TOPIC")]
      public string ETA_TOPIC { get; set; }
      [XmlElement("ETA_DURATION")]
      public double ETA_DURATION { get; set; }
      [XmlElement("ETA_TRAINER")]
      public string ETA_TRAINER { get; set; }
      [XmlElement("ETA_DESCRIPTION")]
      public string ETA_DESCRIPTION { get; set; }
      [XmlElement("ETA_DEPT")]
      public int ETA_DEPT { get; set; }
      [XmlElement("ETA_COMPANY")]
      public int ETA_COMPANY { get; set; }
      [XmlElement("USER_PK")]
      public int USER_PK { get; set; }
      [XmlElement("BIZUNIT")]
      public int BIZUNIT { get; set; }
      [XmlElement("LAST_MOD_DT")]
      public DateTime LAST_MOD_DT { get; set; }
      [XmlElement("Details")]
      public List<EmployeeTrainingDetails> EmployeeTrainingDtl { get; set; }
  }

  [Serializable]
  public class EmployeeTrainingDetails
  {
      [XmlElement("ROW_NO")]
      public int ROW_NO { get; set; }
      [XmlElement("ETL_PK")]
      public int ETL_PK { get; set; }
      [XmlElement("ETL_ETA_PK")]
      public int ETL_ETA_PK { get; set; }
      [XmlElement("ETL_EMP_PK")]
      public int ETL_EMP_PK { get; set; }
      [XmlElement("empName_txt")]
      public string empName_txt { get; set; }
      [XmlElement("empBranchText")]
      public string empBranchText { get; set; }
      [XmlElement("empDepartmentText")]
      public string empDepartmentText { get; set; }
      [XmlElement("empDesignationText")]
      public string empDesignationText { get; set; }
  }

  [Serializable]
  [XmlRoot("Root")]
  public class EmployeeTrainingHeader_PopUp
  {
      [XmlElement("Details")]
      public List<EmployeeTraining_PopUP> EmployeeTraining_PopUPDtl { get; set; }
  }


  public class EmployeeTraining_PopUP
  {
      public int ROW_NO { get; set; }
      public int ETL_PK { get; set; }
      public int ETL_EMP_PK { get; set; }
      public string empName_txt { get; set; }
      public string empBranchText { get; set; }
      public string empDepartmentText { get; set; }
      public string empDesignationText { get; set; }
      
  }
}
