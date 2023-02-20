namespace SchadInvoice.Models.Dto
{
    public class CustomerDto
    {
        public int Id { get; set; }

        public string CustName { get; set; } = null!;

        public string Adress { get; set; } = null!;

        public bool? Status { get; set; }

        public int CustomerTypeId { get; set; }
        public string CustomerTypeDescription { get; set; }
    }
}
