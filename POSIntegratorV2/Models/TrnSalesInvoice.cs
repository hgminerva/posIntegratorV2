using System;

namespace POSIntegratorV2.Models
{
    public class TrnSalesInvoice
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
        public Decimal Quantity { get; set; }
        public String Unit { get; set; }
        public Decimal Price { get; set; }
        public Decimal Discount { get; set; }
        public Decimal DiscountRate { get; set; }
        public Decimal NetPrice { get; set; }
        public Decimal Amount { get; set; }
        public Decimal VAT { get; set; }
        public Decimal VATAmount { get; set; }
    }
}
