using SchadInvoice.Models.Dto;

namespace SchadInvoice.Models.Request
{
    public class CustomerRequest
    {
        public CustomerDto Customer { get; set; }
        public List<CustomerTypeDto> CustomerTypes { get; set; }
    }
}
