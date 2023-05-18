using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Identity;

namespace WeddingPlanner.Controllers;    
public class UserController : Controller
{    
    private readonly ILogger<UserController> _logger;
    private MyContext _context;         
    public UserController(ILogger<UserController> logger, MyContext context)    
    {        
        _logger = logger;
        _context = context;    
    }   

    [HttpGet("")]
    public IActionResult Index()
    {
        if (HttpContext.Session.GetInt32("UserId") is not null){
            return RedirectToAction("AllWeddings", "Wedding");
        }
        return View("Index");
    }

    [HttpPost("user/create")]   
    public IActionResult Create(User newUser)    
    {        
        if(ModelState.IsValid)        
        {
            PasswordHasher<User> Hasher = new PasswordHasher<User>();   
            newUser.Password = Hasher.HashPassword(newUser, newUser.Password);            
            _context.Add(newUser);
            _context.SaveChanges();  
            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            HttpContext.Session.SetString("UserFName", newUser.FirstName);
            HttpContext.Session.SetString("UserLName", newUser.LastName);

            return RedirectToAction("AllWeddings", "Wedding");
        } else {
            return View("Index");
        }   
    }

[HttpPost("user/login")]
public IActionResult Login(LogUser userSubmission)
{    
    if(ModelState.IsValid)    
    {        
        User? userInDb = _context.Users.FirstOrDefault(u => u.Email == userSubmission.LEmail);        
        if(userInDb == null)        
            {   
            ModelState.AddModelError("Email", "Invalid Email/Password");            
            return View("Index");        
            }   
        PasswordHasher<LogUser> hasher = new PasswordHasher<LogUser>();                    
        var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LPassword);                                    // Result can be compared to 0 for failure        
        if(result == 0)        
        {   
            ModelState.AddModelError("LEmail", "Invalid Email/Password");
            return View("Index");
        } 
        else 
        {
            HttpContext.Session.SetInt32("UserId", userInDb.UserId);
            HttpContext.Session.SetString("UserFName", userInDb.FirstName);
            HttpContext.Session.SetString("UserLName", userInDb.LastName);
            return RedirectToAction("AllWeddings", "Wedding");
        } 
    }
    else 
    {
        return View("Index");
}}


public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        int? userId = context.HttpContext.Session.GetInt32("UserId");
        if(userId == null)
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}

    [HttpGet("/logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}