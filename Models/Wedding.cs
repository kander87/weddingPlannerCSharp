#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WeddingPlanner.Models;

    public class Wedding
    {
        [Key]
        public int WeddingId { get; set; }

        [Required(ErrorMessage = "Name of Wedder 1 is required")]
        [MinLength(2, ErrorMessage ="Name of Wedder 1 must be at least 2 characters.")]
        public string WedderOne { get; set; } 

        [Required(ErrorMessage = "Name of Wedder 2 is required")]
        [MinLength(2, ErrorMessage ="Name of Wedder 2 must be at least 2 characters.")]
        public string WedderTwo { get; set; } 

        [Required (ErrorMessage = "Date of wedding is required")]
        [FutureDate]
        // [DisplayFormat(DataFormatString = "{MMM dd, yyyy}",  ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [MinLength(4, ErrorMessage ="Address must be at least 4 characters.")]
        public string Address { get; set; } 
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;        

        public List<Association> Associations { get; set; } = new List<Association>();

        public int UserId  { get; set; }
        public User? Creator  { get; set; }
    }


public class FutureDateAttribute : ValidationAttribute
{

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if ((DateTime)value < DateTime.Now)
        {

            return new ValidationResult("You must choose a date in the future!");
        }
        else
        {
            return ValidationResult.Success;

        }
    }
}

