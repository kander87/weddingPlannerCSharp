#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models;

    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [MinLength(2, ErrorMessage =" First name must be at least 2 characters.")]
        public string FirstName { get; set; } 

        [Required(ErrorMessage = "Last name is required")]
        [MinLength(2, ErrorMessage =" Last name must be at least 2 characters.")]
        public string LastName { get; set; } 


        [Required (ErrorMessage = "Email is required")]
        [EmailAddress]
        [UniqueEmail]
        public string Email { get; set; } 

        [Required (ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;        

        [NotMapped]
        // There is also a built-in attribute for comparing two fields we can use!
        [Compare("Password")]
        public string PasswordConfirm { get; set; }  

        public List<Wedding> AllWeddings  { get; set; } = new List<Wedding>();
        public List<Association> Associations { get; set; } = new List<Association>();
    }


public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value == null)
        {
            return new ValidationResult("Email is required!");
        }
    	// This will connect us to our database since we are not in our Controller
        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));
        // Check to see if there are any records of this email in our database
        if(_context.Users.Any(e => e.Email == value.ToString()))
        {
            return new ValidationResult("Email must be unique!");
        } else {
            return ValidationResult.Success;
        }
    }
}

