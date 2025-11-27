using System;
using System.Collections.Generic;

namespace Backend_Salesora_System.Models;

public partial class PurchaseInvoiceDetail
{
    public int Id { get; set; }

    public int PurchaseInvoiceId { get; set; }

    public int ProductId { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Total { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual PurchaseInvoice PurchaseInvoice { get; set; } = null!;
}
