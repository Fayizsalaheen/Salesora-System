namespace Backend_Salesora_System.DTO
{
    public class PurchaseInvoiceDetailDto
    {
        public int ProductId { get; set; }
       
        public string ProductName { get; set; } = string.Empty;
        
               //jhhj
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
      
        public decimal Tax { get; set; } = 0;
    }
}
