using SchadInvoice.Models.Dto;

namespace SchadInvoice.Models.Request
{
    public class InvoiceRequest
    {
        public InvoiceDto Invoice { get; set; }
        public List<CustomerDto> Customers { get; set; }
        public InvoiceDetailDto InvoiceDetail { get; set; }
    }
}
