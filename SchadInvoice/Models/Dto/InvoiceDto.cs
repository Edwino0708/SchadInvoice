namespace SchadInvoice.Models.Dto
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalItbis { get; set; }
    }
}
