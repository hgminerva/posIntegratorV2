using System;

namespace POSIntegratorV2.Models
{
    public class TrnSI
    {
        public String BranchCode { get; set; }
        public String BranchName { get; set; }
        public String SINumber { get; set; }
        public String SIDate { get; set; }
        public String DocumentReference { get; set; }
        public String CustomerCode { get; set; }
        public String Term { get; set; }
        public String Remarks { get; set; }
        public String PreparedByUser { get; set; }
        public String SoldByUser { get; set; }
        public String CreatedDateTime { get; set; }
        public String ItemCode { get; set; }
        public String Particulars { get; set; }
        public String Quantity { get; set; }
        public String Unit { get; set; }
        public String Price { get; set; }
        public String Discount { get; set; }
        public String DiscountRate { get; set; }
        public String NetPrice { get; set; }
        public String Amount { get; set; }
        public String VAT { get; set; }
        public String VATAmount { get; set; }
    }
}
