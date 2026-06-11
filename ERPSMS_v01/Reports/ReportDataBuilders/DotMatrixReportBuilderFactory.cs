using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessObject.CommonManagement;

namespace ERPSMS_v01.Reports.ReportDataBuilders
{
    public sealed class DotMatrixReportBuilderFactory
    {
        ReportDataBuilder _builder;
        string _applicationType;

        public DotMatrixReportBuilderFactory(string applicationType)
        {
            _applicationType = applicationType;
        }

        public ReportDataBuilder GetReportDataBuilder()
        {
            switch (_applicationType)
            {
                case ApplicationType.JV:
                case ApplicationType.CNJ:
                case ApplicationType.DNJ:
                    _builder = new JournalVoucherReportBuilder(_applicationType);
                    break;
                case ApplicationType.PCVJ:
                case ApplicationType.DPVJ:
                    _builder = new PettyCashReportBuilder(_applicationType);
                    break;
                case ApplicationType.VPJ:              
                case ApplicationType.PCBJ:
                case ApplicationType.EIPJ:
                case ApplicationType.SIPJ:
                    _builder = new PaymentVoucherReportBuilder(_applicationType);
                    break;
                case ApplicationType.PPCCJ:
                    _builder = new PDCVoucherPaymentReportBuilder(_applicationType);
                    break;
                case ApplicationType.PDCCJ:
                    _builder = new PDCVoucherReceiptReportBuilder(_applicationType);
                    break;
                case ApplicationType.SIJ:
                case ApplicationType.MSIJ:
                    _builder = new SalesVoucherReportBuilder(_applicationType);
                    break;
                case ApplicationType.PIJ:                    
                    _builder = new PurchaseVoucherReportBuilder(_applicationType);
                    break;
                case ApplicationType.EIJ:                    
                    _builder = new ExpenseVoucherReportBuilder(_applicationType);
                    break;
                case ApplicationType.CRJ:
                    _builder = new ReceiptVoucherReportBuilder(_applicationType);
                    break;
                case ApplicationType.RCBJ:
                    _builder = new ChequeReturnVoucherReceiptReportBuilder(_applicationType);
                    break;
                case ApplicationType.FCHRJ:
                    _builder = new FcReverseReportBuilder(_applicationType);
                    break;
                default:
                    _builder = new NullReportBuilder(_applicationType);
                    break;
            }
            return _builder;
        }
    }
}