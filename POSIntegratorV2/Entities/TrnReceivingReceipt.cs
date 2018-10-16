﻿using System;
using System.Collections.Generic;

namespace POSIntegratorV2
{
    public class TrnReceivingReceipt
    {
        public String BranchCode { get; set; }
        public String Branch { get; set; }
        public String RRNumber { get; set; }
        public String RRDate { get; set; }
        public List<TrnReceivingReceiptItem> ListPOSIntegrationTrnReceivingReceiptItem { get; set; }
    }
}
