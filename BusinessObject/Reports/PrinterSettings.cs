using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Reports
{
    public sealed class PrinterSettings
    {
        public string DocumentWidth { get; set; }
        public string DocumentHeight { get; set; }

        public string WindowWidth { get; set; }
        public string WindowHeight { get; set; }
    }
}
