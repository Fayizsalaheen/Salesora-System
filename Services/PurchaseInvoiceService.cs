using Backend_Salesora_System.DTO;
using Backend_Salesora_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_Salesora_System
{
    public class PurchaseInvoiceService
    {
        private readonly AppDbContext _context;

        public PurchaseInvoiceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string Message)> CreateInvoiceAsync(PurchaseInvoiceDto dto)
        {
            if (dto == null || dto.Details == null || !dto.Details.Any())
                return (false, "Invoice details are required");

        
            var supplier = await _context.Suppliers.FindAsync(dto.SupplierId);
            if (supplier == null)
                return (false, $"Supplier ID {dto.SupplierId} not found");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var invoice = new PurchaseInvoice
                {
                    SupplierId = dto.SupplierId,
                    InvoiceDate = dto.InvoiceDate,
                    TotalAmount = 0,
                    TaxAmount = 0,
                    NetAmount = 0,
                    CreatedBy = 1,
                    CreatedAt = DateTime.Now,
                    PurchaseInvoiceDetails = new List<PurchaseInvoiceDetail>()
                };

                _context.PurchaseInvoices.Add(invoice);
                await _context.SaveChangesAsync();

                decimal totalAmount = 0;
                decimal totalTax = 0;

                foreach (var d in dto.Details)
                {                    var product = await _context.Products.FindAsync(d.ProductId);
                    if (product == null)
                        return (false, $"Product ID {d.ProductId} not found");

                    if (d.Quantity <= 0)
                        return (false, $"Quantity must be greater than zero for Product ID {d.ProductId}");

                    if (d.UnitPrice < 0)
                        return (false, $"UnitPrice cannot be negative for Product ID {d.ProductId}");

                    decimal totalPerItem = (d.Quantity * d.UnitPrice) - d.Discount;
                    decimal taxPerItem = d.Tax;

                    var detail = new PurchaseInvoiceDetail
                    {
                        PurchaseInvoiceId = invoice.Id,
                        ProductId = d.ProductId,
                        Quantity = d.Quantity,
                        UnitPrice = d.UnitPrice,
                        Total = totalPerItem
                    };

                    product.QuantityInStock += d.Quantity;

                    invoice.PurchaseInvoiceDetails.Add(detail);
                    _context.PurchaseInvoiceDetails.Add(detail);
                    _context.Products.Update(product);

                    totalAmount += totalPerItem;
                    totalTax += taxPerItem;
                }

                invoice.TotalAmount = totalAmount;
                invoice.TaxAmount = totalTax;
                invoice.NetAmount = totalAmount + totalTax;

                _context.PurchaseInvoices.Update(invoice);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, $"Invoice created successfully. Net Total: {invoice.NetAmount}");
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                return (false, dbEx.InnerException?.Message ?? dbEx.Message);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, ex.Message);
            }
        }
    }
}
