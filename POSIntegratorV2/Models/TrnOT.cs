using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSIntegratorV2.Models
{
    class TrnOT
    {
        public String BranchCode { get; set; }
        public String OTDate { get; set; }
        public String AccountCode { get; set; }
        public String ArticleCode { get; set; }
        public String Remarks { get; set; }
        public String ManualOTNumber { get; set; }
        public String UserCode { get; set; }
        public String CreatedDateTime { get; set; }
        public String ItemCode { get; set; }
        public String Particulars { get; set; }
        public String Unit { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Cost { get; set; }
        public Decimal Amount { get; set; }
    }
}
