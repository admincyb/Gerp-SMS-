using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.UOMManagement
{
    public class UOM
    {


        public int UMT_PK { get; set; }
        public string UMT_CODE { get; set; }
        public string UMT_NAME { get; set; }
        public int Status { get; set; }
        public int BizUnit { get; set; }


        public int UOM_PK { get; set; }
        public string UOM_CODE { get; set; }
        public string UOM_NAME { get; set; }
        public string UOM_TYPE { get; set; }
        public string UOM_SYMBOL { get; set; }
        public int UOM_DECIMAL { get; set; }

        public int UserPk { get; set; }
        
      
      
        



        public List<ConversionInfo> ConversionList { get; set; }


        public class ConversionInfo
        {
            public int UMC_UOM_TYPE { get; set; }
             public int UserPk { get; set; }
             public int BizUnitPk { get; set; }
             public int UMC_PK { get; set; }
             public int UMC_FROM { get; set; }
             public int UMC_TO { get; set; }
             public double UMC_CONV_FACT { get; set; }


            public string UOMTYPEText { get; set; }
            public string FromUnitText { get; set; }  
            public string ToUnitText { get; set; }

            


           
            
         


        }

    }
}
