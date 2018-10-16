using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSIntegratorV2
{
    class TrnArticlePrice
    {
        public String BranchCode { get; set; }
        public String IPNumber { get; set; }
        public String IPDate { get; set; }
        public String ItemCode { get; set; }
        public String ItemDescription { get; set; }
        public Decimal Price { get; set; }
        public Decimal TriggerQuantity { get; set; }
    }
}
