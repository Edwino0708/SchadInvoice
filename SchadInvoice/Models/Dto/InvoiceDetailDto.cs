namespace SchadInvoice.Models.Dto
{
    public class InvoiceDetailDto
    {
        public int Id { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public int CustomerId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalItbis { get; set; }
        public string CustomerName { get; set; }
    }
}
