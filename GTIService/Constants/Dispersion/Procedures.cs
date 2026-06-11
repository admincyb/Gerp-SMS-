using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Dispersion
{
    public class Procedures
    {
        public const string GETMATERIALS = "SpGRmTldTime"; //SP to get materials
        public const string SAVEDISPERSIONDETAILS = "SPPRD_BOM_MST_SAVE"; //  "SpSPrnsdTm";//SP to  save dispersion details
        public const string GETDISPERSIONLIST = "SPPRD_BOM_MST_GET_LIST";//"SpGPrnsdHrcP";// SP to get all dispersions;
        public const string GETDISPERSIONDETAILS = "SPPRD_BOM_MST_GET"; //sp to get dispersion detail
        public const string DELETEDISPERSION = "SPPRD_BOM_MST_DELETE";//Sp to delete dispersion details
        public const string GETMACHINETYPES = "SpGEptm";//Sp to get all machine types
        public const string GETUOMS = "SpGMOU";//Sp to get uom s 
        public const string GETAUTOCOMPLETE = "SPPRD_BOM_MST_AUTO";//"SpGTimeOta";//SP to get the autocomplete search
        public const string GETDISPERSIONFORCOMB = "SpGPrnsdTm"; //SP to fill dropdown
        public const string GETDISPERSIONFORDDL = "SPPRD_BOM_MST_GET_KV";
        public const string GETDISPERSIONAUTO = "SPPRD_BOM_MST_GET_KV";

    }
}
