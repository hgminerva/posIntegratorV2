﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSIntegratorV2
{
    class MstCustomer
    {
        public String ManualArticleCode { get; set; }
        public String Article { get; set; }
        public String Address { get; set; }
        public String ContactPerson { get; set; }
        public String ContactNumber { get; set; }
        public String Term { get; set; }
        public String TaxNumber { get; set; }
        public Decimal CreditLimit { get; set; }
    }
}
