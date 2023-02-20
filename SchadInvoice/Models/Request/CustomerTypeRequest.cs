using System.ComponentModel.DataAnnotations;

namespace SchadInvoice.Models.Request
{
    public class CustomerTypeRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Input Description can not be null")]
        public string Description { get; set; }
    }
}
