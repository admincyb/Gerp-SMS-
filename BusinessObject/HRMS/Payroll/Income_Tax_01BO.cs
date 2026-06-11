using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Payroll
{
    public class Income_Tax_01BO
    {

        [Serializable]
        [XmlRoot("Root")]
        public sealed class Income_Tax_01
        {
            [XmlElement("Type")]
            public int Type { get; set; }
            [XmlElement("LAST_MOD_DT")]
            public DateTime LAST_MOD_DT { get; set; }
            [XmlElement("USER_PK")]
            public int USER_PK { get; set; }
            [XmlElement("IT1_ACTIVE")]
            public int IT1_ACTIVE { get; set; }
            [XmlElement("IT1_EPS_PK")]
            public int IT1_EPS_PK { get; set; }
            [XmlElement("IT1_EMP_PK")]
            public int IT1_EMP_PK { get; set; }

            [XmlElement("Section")]
            public List<Section> Section { get; set; }
        }

        [Serializable]
        [XmlRoot("Section")]
        public sealed class Section
        {
            [XmlElement("Seq")]
            public int Seq { get; set; }
            [XmlElement("SectionID")]
            public int SectionID { get; set; }
            [XmlElement("Title")]
            public string Title { get; set; }
            [XmlElement("Col1")]
            public string Col1 { get; set; }
            [XmlElement("Col2")]
            public string Col2 { get; set; }
            [XmlElement("Col3")]
            public string Col3 { get; set; }
            [XmlElement("Col4")]
            public string Col4 { get; set; }
            [XmlElement("Col5")]
            public string Col5 { get; set; }
            [XmlElement("Col5_Visible")]
            public int Col5_Visible { get; set; }

            [XmlElement("Col3FunId")]
            public string Col3FunId { get; set; }
            [XmlElement("Col3FunExpression")]
            public string Col3FunExpression { get; set; }
            [XmlElement("Col3ExcludeId")]
            public string Col3ExcludeId { get; set; }
            [XmlElement("Col3CopyId")]
            public string Col3CopyId { get; set; }

            [XmlElement("Items")]
            public List<Items> Items { get; set; }
        }

        [Serializable]
        [XmlRoot("Items")]
        public sealed class Items
        {
            [XmlElement("Seq")]
            public int Seq { get; set; }
            [XmlElement("IT1_SEQ_NO")]
            public string IT1_SEQ_NO { get; set; }
            [XmlElement("IT1_PK")]
            public int IT1_PK { get; set; }
            [XmlElement("DbId")]
            public int DbId { get; set; }
            [XmlElement("Col1")]
            public string Col1 { get; set; }
            [XmlElement("Col2")]
            public string Col2 { get; set; }
            [XmlElement("Col3")]
            public string Col3 { get; set; }
            [XmlElement("Col4")]
            public string Col4 { get; set; }
            [XmlElement("Col5")]
            public string Col5 { get; set; }
            [XmlElement("Col3_Edit")]
            public int Col3_Edit { get; set; }
            [XmlElement("Col4_Edit")]
            public int Col4_Edit { get; set; }
            [XmlElement("Col3_Hide")]
            public int Col3_Hide { get; set; }
            [XmlElement("Col4_Hide")]
            public int Col4_Hide { get; set; }
            [XmlElement("Col3_Max")]
            public string Col3_Max { get; set; }
            [XmlElement("ChkBoxReq")]
            public int ChkBoxReq { get; set; }
            [XmlElement("DefaultAmount")]
            public string DefaultAmount { get; set; }
            [XmlElement("IT1_CHECKBOX_1")]
            public int IT1_CHECKBOX_1 { get; set; }

            [XmlElement("ColScript")]
            public string ColScript { get; set; }
            [XmlElement("ColResult")]
            public int ColResult { get; set; }
            [XmlElement("ColResulCopyDbID")]
            public int ColResulCopyDbID { get; set; }

            [XmlElement("TIT_VALUE1")]
            public string TIT_VALUE1 { get; set; }
            [XmlElement("TIT_DESC1")]
            public string TIT_DESC1 { get; set; }



            
        }


        public sealed class SectionItems
        {
            public int Sec_Seq { get; set; }
            public int Sec_ID { get; set; }
            public string Sec_Title { get; set; }
            public string Sec_Col1 { get; set; }
            public string Sec_Col2 { get; set; }
            public string Sec_Col3 { get; set; }
            public string Sec_Col4 { get; set; }
            public string Sec_Col5 { get; set; }
            public int Sec_Col5_Visible { get; set; }

            public int IT1_EPS_PK { get; set; }
            public int IT1_EMP_PK { get; set; }

            public int Itm_Seq { get; set; }
            public int Itm_PK { get; set; }
            public int Itm_DbId { get; set; }
            public string Itm_Col1 { get; set; }
            public string Itm_Col2 { get; set; }
            public string Itm_Col3 { get; set; }
            public string Itm_Col4 { get; set; }
            public string Itm_Col5 { get; set; }
            public int Itm_Col3_Edit { get; set; }
            public int Itm_Col4_Edit { get; set; }
            public string IT1_SEQ_NO { get; set; }
        }
        [Serializable]
        [XmlRoot("Root")]
        public sealed class Tax01_PayrollDetails
        {
            [XmlElement("EPS_PK")]
            public int EPS_PK { get; set; }
            [XmlElement("empPK")]
            public int empPK { get; set; }
            [XmlElement("EPS_PAYROLL_HDR")]
            public int EPS_PAYROLL_HDR { get; set; }
            [XmlElement("EPH_PRC_NAME")]
            public string EPH_PRC_NAME { get; set; }
            [XmlElement("EPH_DATE")]
            public DateTime EPH_DATE { get; set; }
            [XmlElement("EPH_FROM_DATE")]
            public DateTime EPH_FROM_DATE { get; set; }
            [XmlElement("EPH_TO_DATE")]
            public DateTime EPH_TO_DATE { get; set; }
            [XmlElement("EPH_MOD_DT")]
            public DateTime EPH_MOD_DT { get; set; }
            [XmlElement("empCode")]
            public string empCode { get; set; }
            [XmlElement("empName")]
            public string empName { get; set; }
            [XmlElement("empText")]
            public string empText { get; set; }
            [XmlElement("empDOJ")]
            public string empDOJ { get; set; }
        }

        public class ReportDetails
        {
            #region AMOUNT

            //SECTION - A
            public double Amt_Acol1 { get; set; }   //salary,wages...
            public double Amt_Acol2 { get; set; }   //Less exempted...
            public double Amt_Acol3 { get; set; }   //Balance(1-2)
            public double Amt_Acol4 { get; set; }   //Less expense...
            public double Amt_Acol5 { get; set; }   //Balance(3-4)
            public double Amt_Acol6 { get; set; }   //Less allowances...
            public double Amt_Acol7 { get; set; }   //Balance(5-6)
            public double Amt_Acol8 { get; set; }   //Less donation...
            public double Amt_Acol9 { get; set; }   //Balance(7-8)
            public double Amt_Acol10 { get; set; }  //Less than donation...
            public double Amt_Acol11 { get; set; }  //Net income(9-10)
            public double Amt_Acol12 { get; set; }  //Tax computed...
            public double Amt_Acol13 { get; set; }  //Less exemption for...
            public double Amt_Acol14 { get; set; }  //Tax payable...
            public double Amt_Acol15 { get; set; }  //Less witholding tax
            public double Amt_Acol16 { get; set; }  //Total tax
            public double Amt_Acol16_1 { get; set; }    //Total tax-payable
            public double Amt_Acol16_2 { get; set; }    //Total tax-overpaid
            public double Amt_Acol17 { get; set; }  //Add additional tax...
            public double Amt_Acol18 { get; set; }  //Less tax overpaid...
            public double Amt_Acol19 { get; set; }  //Less tax paid...
            public double Amt_Acol20 { get; set; }  //Tax
            public double Amt_Acol20_1 { get; set; }    //Tax-payable
            public double Amt_Acol20_2 { get; set; }    //Tax-overpaid
            public double Amt_Acol21 { get; set; }  //Add surcharge...
            public double Amt_Acol22 { get; set; }  //Total tax
            public double Amt_Acol22_1 { get; set; }    //Total tax-payable
            public double Amt_Acol22_2 { get; set; }    //Total tax-overpaid
                          
            //SECTION B   
            public double Amt_Bcol1 { get; set; }   //PF Contribution...
            public double Amt_Bcol2 { get; set; }   //Govt. pension...
            public double Amt_Bcol3 { get; set; }   //Private teacher...
            public double Amt_Bcol4 { get; set; }   //National saving...
            public double Amt_Bcol5 { get; set; }   //Income exemption...
            public double Amt_Bcol5_1 { get; set; } //Disable taxpayer under 65...
            public double Amt_Bcol5_2 { get; set; } //Taxpayer aged 65 and above...
            public double Amt_Bcol6 { get; set; }   //serverence pay...
            public double Amt_Bcol7 { get; set; }   //Total(1-5)
                          
            //SECTION C   
            public double Amt_Ccol1 { get; set; }   //Taxpayer
            public double Amt_Ccol2 { get; set; }   //Spouse
            public double Amt_Ccol3 { get; set; }   //Child (15,000)...
            public double Amt_Ccol3_1 { get; set; } //Fill persoNal ID
            public double Amt_Ccol3_2 { get; set; } //Child (17,000)...
            public double Amt_Ccol3_3 { get; set; } //Fill Personal ID
            public double Amt_Ccol4 { get; set; }   //Parental care
            public double Amt_Ccol4_1 { get; set; } //Father of taxpayer
            public double Amt_Ccol4_2 { get; set; } //Mother of taxpayer
            public double Amt_Ccol4_3 { get; set; } //Father of spouse...
            public double Amt_Ccol4_4 { get; set; } //Mother of spouse...
            public double Amt_Ccol5 { get; set; }   //Disabled/Incompetant...
            public double Amt_Ccol6 { get; set; }   //Health insurance...
            public double Amt_Ccol6_1 { get; set; } //Father of taxpayer
            public double Amt_Ccol6_2 { get; set; } //Mother of taxpayer
            public double Amt_Ccol6_3 { get; set; } //Father of spouse
            public double Amt_Ccol6_4 { get; set; } //Mother of spouse
            public double Amt_Ccol7 { get; set; }   //Life insurance...
            public double Amt_Ccol7_1 { get; set; } //Pension insurance...
            public double Amt_Ccol8 { get; set; }   //PF Contribution...
            public double Amt_Ccol9 { get; set; }   //Retirement mutual...
            public double Amt_Ccol10 { get; set; }  //Long term equity...
            public double Amt_Ccol11 { get; set; }  //Interest paid on loan...
            public double Amt_Ccol12 { get; set; }  //First time home buyer...
            public double Amt_Ccol13 { get; set; }  //Social secutiy fund...
            public double Amt_Ccol14 { get; set; }  //Domestic tourism...
            public double Amt_Ccol15 { get; set; }  //Domestic purchase...
            public double Amt_Ccol16 { get; set; }  //Total(1-16)

            #endregion

            #region VALUE

            //SECTION - A
            public double Val_Acol1 { get; set; }   //salary,wages...
            public double Val_Acol2 { get; set; }   //Less exempted...
            public double Val_Acol3 { get; set; }   //Balance(1-2)
            public double Val_Acol4 { get; set; }   //Less expense...
            public double Val_Acol5 { get; set; }   //Balance(3-4)
            public double Val_Acol6 { get; set; }   //Less allowances...
            public double Val_Acol7 { get; set; }   //Balance(5-6)
            public double Val_Acol8 { get; set; }   //Less donation...
            public double Val_Acol9 { get; set; }   //Balance(7-8)
            public double Val_Acol10 { get; set; }  //Less than donation...
            public double Val_Acol11 { get; set; }  //Net income(9-10)
            public double Val_Acol12 { get; set; }  //Tax computed...
            public double Val_Acol13 { get; set; }  //Less exemption for...
            public double Val_Acol14 { get; set; }  //Tax payable...
            public double Val_Acol15 { get; set; }  //Less witholding tax
            public double Val_Acol16 { get; set; }  //Total tax
            public double Val_Acol16_1 { get; set; }    //Total tax-payable
            public double Val_Acol16_2 { get; set; }    //Total tax-overpaid
            public double Val_Acol17 { get; set; }  //Add additional tax...
            public double Val_Acol18 { get; set; }  //Less tax overpaid...
            public double Val_Acol19 { get; set; }  //Less tax paid...
            public double Val_Acol20 { get; set; }  //Tax
            public double Val_Acol20_1 { get; set; }    //Tax-payable
            public double Val_Acol20_2 { get; set; }    //Tax-overpaid
            public double Val_Acol21 { get; set; }  //Add surcharge...
            public double Val_Acol22 { get; set; }  //Total tax
            public double Val_Acol22_1 { get; set; }    //Total tax-payable
            public double Val_Acol22_2 { get; set; }    //Total tax-overpaid
                          
            //SECTION B   
            public double Val_Bcol1 { get; set; }   //PF Contribution...
            public double Val_Bcol2 { get; set; }   //Govt. pension...
            public double Val_Bcol3 { get; set; }   //Private teacher...
            public double Val_Bcol4 { get; set; }   //National saving...
            public double Val_Bcol5 { get; set; }   //Income exemption...
            public double Val_Bcol5_1 { get; set; } //Disable taxpayer under 65...
            public double Val_Bcol5_2 { get; set; } //Taxpayer aged 65 and above...
            public double Val_Bcol6 { get; set; }   //serverence pay...
            public double Val_Bcol7 { get; set; }   //Total(1-5)
                          
            //SECTION C   
            public double Val_Ccol1 { get; set; }   //Taxpayer
            public double Val_Ccol2 { get; set; }   //Spouse
            public double Val_Ccol3 { get; set; }   //Child (15,000)...
            public double Val_Ccol3_1 { get; set; } //Fill persoNal ID
            public double Val_Ccol3_2 { get; set; } //Child (17,000)...
            public double Val_Ccol3_3 { get; set; } //Fill Personal ID
            public double Val_Ccol4 { get; set; }   //Parental care
            public double Val_Ccol4_1 { get; set; } //Father of taxpayer
            public double Val_Ccol4_2 { get; set; } //Mother of taxpayer
            public double Val_Ccol4_3 { get; set; } //Father of spouse...
            public double Val_Ccol4_4 { get; set; } //Mother of spouse...
            public double Val_Ccol5 { get; set; }   //Disabled/Incompetant...
            public double Val_Ccol6 { get; set; }   //Health insurance...
            public double Val_Ccol6_1 { get; set; } //Father of taxpayer
            public double Val_Ccol6_2 { get; set; } //Mother of taxpayer
            public double Val_Ccol6_3 { get; set; } //Father of spouse
            public double Val_Ccol6_4 { get; set; } //Mother of spouse
            public double Val_Ccol7 { get; set; }   //Life insurance...
            public double Val_Ccol7_1 { get; set; } //Pension insurance...
            public double Val_Ccol8 { get; set; }   //PF Contribution...
            public double Val_Ccol9 { get; set; }   //Retirement mutual...
            public double Val_Ccol10 { get; set; }  //Long term equity...
            public double Val_Ccol11 { get; set; }  //Interest paid on loan...
            public double Val_Ccol12 { get; set; }  //First time home buyer...
            public double Val_Ccol13 { get; set; }  //Social secutiy fund...
            public double Val_Ccol14 { get; set; }  //Domestic tourism...
            public double Val_Ccol15 { get; set; }  //Domestic purchase...
            public double Val_Ccol16 { get; set; }  //Total(1-16)

            #endregion

            #region TEXT

            public string TaxPyr_Txt_Desc1 { get; set; }

            public int Chld1_Txt_Value1 { get; set; }
            public int Chld2_Txt_Value1 { get; set; }

            public string ID1_Txt_Desc1A { get; set; }
            public string ID1_Txt_Desc1B { get; set; }
            public string ID1_Txt_Desc1C { get; set; }

            public string ID2_Txt_DescA { get; set; }
            public string ID2_Txt_DescB { get; set; }
            public string ID2_Txt_DescC { get; set; }

            public int ID1_Val_Value1 { get; set; }
            public int ID2_Val_Value1 { get; set; }

            public string Fath1_Txt_Desc1 { get; set; }
            public string Moth1_Txt_Desc1 { get; set; }
            public string SpFth1_Txt_Desc1 { get; set; }
            public string SpMth1_Txt_Desc1 { get; set; }

            public string Fath2_Txt_Desc1 { get; set; }
            public string Moth_Txt_Desc1 { get; set; }
            public string SpFth2_Txt_Desc1 { get; set; }
            public string SpMth2_Txt_Desc1 { get; set; }

            #endregion

        }
    }
}
