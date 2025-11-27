namespace Backend_Salesora_System.DTO
{
    public class PurchaseInvoiceDto
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public List<PurchaseInvoiceDetailDto> Details { get; set; } = new();
    }

}
