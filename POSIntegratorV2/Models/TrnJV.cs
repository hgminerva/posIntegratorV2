using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSIntegratorV2.Models
{
    class TrnJV
    {
        public String BranchCode { get; set; }
        public String JVDate { get; set; }
        public String Remarks { get; set; }
        public String ManualJVNumber { get; set; }
        public String UserCode { get; set; }
        public String CreatedDateTime { get; set; }
        public String EntryBranchCode { get; set; }
        public String AccountCode { get; set; }
        public String ArticleCode { get; set; }
        public String Particulars { get; set; }
        public String DebitAmount { get; set; }
        public String CreditAmount { get; set; }
        public String ARRRNumber { get; set; }
        public String ARSINumber { get; set; }
        public String IsClear { get; set; }
    }
}
