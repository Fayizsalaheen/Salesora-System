using System;
using System.Collections.Generic;

namespace Backend_Salesora_System.Models;

public partial class PurchaseInvoice
{
    public int Id { get; set; }

    public int SupplierId { get; set; }

    public DateTime InvoiceDate { get; set; }

    public decimal TotalAmount { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal NetAmount { get; set; }

    public virtual ICollection<PurchaseInvoiceDetail> PurchaseInvoiceDetails { get; set; } = new List<PurchaseInvoiceDetail>();

    public virtual Supplier Supplier { get; set; } = null!;
}
