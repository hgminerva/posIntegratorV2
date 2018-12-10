using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSIntegratorV2.Models
{
    class TrnIN
    {
        public String BranchCode { get; set; }
        public String INDate { get; set; }
        public String AccountCode { get; set; }
        public String ArticleCode { get; set; }
        public String Remarks { get; set; }
        public String ManualINNumber { get; set; }
        public String IsProduce { get; set; }
        public String UserCode { get; set; }
        public String CreatedDateTime { get; set; }
        public String ItemCode { get; set; }
        public String Particulars { get; set; }
        public String Unit { get; set; }
        public String Quantity { get; set; }
        public String Cost { get; set; }
        public String Amount { get; set; }
    }
}