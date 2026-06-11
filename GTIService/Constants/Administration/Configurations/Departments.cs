using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Administration.Configurations
{
    public class Departments
    {
        /// <summary>
        /// Oracle Data type
        /// </summary>
        /// <author>GTI</author> 
        public enum MainDepartments
        {
            Purchase = 1,  
            Inventory = 2,
            Production = 3,
            QA=4,
            Sales =5,
            Administration = 6
            
        }
    }
}
