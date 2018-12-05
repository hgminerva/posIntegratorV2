using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSIntegratorV2.Models
{
    class TrnST
    {
        public String BranchCode { get; set; }
        public String STDate { get; set; }
        public String ToBranchCode { get; set; }
        public String ArticleCode { get; set; }
        public String Remarks { get; set; }
        public String ManualSTNumber { get; set; }
        public String UserCode { get; set; }
        public String CreatedDateTime { get; set; }
        public String ItemCode { get; set; }
        public String Particluars { get; set; }
        public String Unit { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Cost { get; set; }
        public Decimal Amount { get; set; }
    }
}
