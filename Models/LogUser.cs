#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
// using statements and namespace go here
public class LogUser
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    [Display(Name = "Email")]
    public string LEmail { get; set; }    

    [Required(ErrorMessage ="Password is required")]    
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string LPassword { get; set; } 
}
