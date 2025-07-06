using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PersonTable.Models
{
    public class PersonModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [MaxLength(100, ErrorMessage = "First name can be max 100 characters long")]
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(100, ErrorMessage = "Last name can be max 100 characters long")]
        [DisplayName("Last name")]
        public string LastName { get; set; }

        [MaxLength(500, ErrorMessage = "Description can be max 100 characters long")]
        [DisplayName("Description")]
        public string? Description { get; set; }

        public List<EmailModel> Emails { get; set; } = new();
    }

    public class EmailModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Incorrect email address")]
        public string Address { get; set; }
    }
}
