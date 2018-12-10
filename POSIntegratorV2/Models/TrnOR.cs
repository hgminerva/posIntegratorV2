using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSIntegratorV2.Models
{
    class TrnOR
    {
        public String BranchCode { get; set; }
        public String ORDate { get; set; }
        public String CustomerCode { get; set; }
        public String Remarks { get; set; }
        public String ManualORNumber { get; set; }
        public String UserCode { get; set; }
        public String CreatedDateTime { get; set; }
        public String AccountCode { get; set; }
        public String ArticleCode { get; set; }
        public String SINumber { get; set; }
        public String Particulars { get; set; }
        public String Amount { get; set; }
        public String PayType { get; set; }
        public String CheckNumber { get; set; }
        public String CheckDate { get; set; }
        public String CheckBank { get; set; }
        public String DepositoryBankCode { get; set; }
        public String IsClear { get; set; }
    }
}
