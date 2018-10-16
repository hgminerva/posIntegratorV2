﻿using System;
using System.Collections.Generic;

namespace POSIntegratorV2
{
    public class TrnStockOut
    {
        public String BranchCode { get; set; }
        public String Branch { get; set; }
        public String OTNumber { get; set; }
        public String OTDate { get; set; }
        public List<TrnStockOutItem> ListPOSIntegrationTrnStockOutItem { get; set; }
    }
}
