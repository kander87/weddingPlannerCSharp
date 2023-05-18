using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Controllers;    
public class WeddingController : Controller
{    
    private readonly ILogger<WeddingController> _logger;
    private MyContext _context;         
    public WeddingController(ILogger<WeddingController> logger, MyContext context)    
    {        
        _logger = logger;
        _context = context;    
    }  

    [SessionCheck]
    [HttpGet("/allweddings")]
    public IActionResult AllWeddings(){
        MyViewModel MyModel = new MyViewModel(){
            AllWeddings = _context.Weddings
            .Include(wed => wed.Associations)
            .ThenInclude(user => user.User)
            .ToList()
        };
        return View("AllWeddings", MyModel);
    }

    [SessionCheck]
    [HttpGet("/wedding/new")]
    public IActionResult NewWedding()
    {
        return View("NewWedding");
    }

    [SessionCheck]
    [HttpPost("/wedding/create")]
    public IActionResult WedCreate(Wedding newWedding)
    {    
        if(ModelState.IsValid)
        { 
            newWedding.UserId =(int)HttpContext.Session.GetInt32("UserId");
            _context.Add(newWedding);    
            _context.SaveChanges();
            return RedirectToAction("AllWeddings");
        } else {

            return View("NewWedding");
        }   
    }

    [SessionCheck]
    [HttpPost("/{weddingId}/delete")]
    public IActionResult WedDelete(int weddingId)
    {   
        Console.WriteLine(weddingId);
        Wedding? WeddingtoDelete = _context.Weddings.SingleOrDefault(wed =>wed.WeddingId == weddingId);
        if (WeddingtoDelete.UserId != HttpContext.Session.GetInt32("UserId")){
            return RedirectToAction("AllWeddings");
        }
        _context.Weddings.Remove(WeddingtoDelete);
        _context.SaveChanges();
        return RedirectToAction("AllWeddings"); 
    }

    [HttpGet("/edit/{weddingId}")]
    public IActionResult WedEdit(int weddingId)
    {
        Wedding? oneWed = _context.Weddings.SingleOrDefault(wed => wed.WeddingId ==weddingId);
        if (oneWed ==null){
                return RedirectToAction("AllWeddings");
            } 
        if (oneWed.UserId == HttpContext.Session.GetInt32("UserId")){
            return View("EditWedding", oneWed);}
        return RedirectToAction("AllWeddings");
    }

    [HttpGet("/WedUpdate/{weddingId}")]
    public IActionResult WedUpdate(Wedding newWed, int weddingId)
    {
        Wedding? OldWed = _context.Weddings.SingleOrDefault(wed => wed.WeddingId ==weddingId);
        if (OldWed == null){
            return RedirectToAction("AllWeddings");
        }
        if(ModelState.IsValid)
        {   OldWed.WedderOne = newWed.WedderOne;
            OldWed.WedderTwo = newWed.WedderTwo;
            OldWed.Date = newWed.Date;
            OldWed.Address = newWed.Address;
            OldWed.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("AllWeddings");
            } else {
            return View("EditWedding", OldWed);
        }
    }

    [SessionCheck]
    [HttpGet("/{weddingId}/RSVPAdd")]
    public IActionResult RSVPAdd( int weddingId){
        Association newRSVP= new Association(){
            WeddingId = weddingId,
            UserId = (int)HttpContext.Session.GetInt32("UserId")
        };

        _context.Associations.Add(newRSVP);
        _context.SaveChanges();
        return RedirectToAction("AllWeddings");
    }

    [SessionCheck]
    [HttpGet("/{weddingId}/RSVPRemove")]
    public IActionResult RSVPRemove(int? weddingId )
    {
        Association? unRSVP = _context.Associations
            .SingleOrDefault(assoc => assoc.UserId == (int)HttpContext.Session.GetInt32("UserId") && assoc.WeddingId == weddingId);
        _context.Associations.Remove(unRSVP);
        _context.SaveChanges();
        return RedirectToAction("AllWeddings");
    }

    [SessionCheck]
    [HttpGet("/wedding/{wedId}")]
    public IActionResult ViewWed(int wedId){
        Wedding? wedding = _context.Weddings
        .Include(wed => wed.Associations)
        .ThenInclude(user => user.User)
        .SingleOrDefault(wed => wed.WeddingId == wedId);
        if(wedding ==null)
        {
            return RedirectToAction("AllWeddings");
        } else {
            List<User> allAttendees = _context.Users
                .Include(user => user.Associations)
                .Where(user => user.Associations
                .Any(Association => Association.WeddingId ==wedding.WeddingId) == true)
                .ToList();
            
            MyViewModel MyModel = new MyViewModel(){
                AllUsers = allAttendees,
                Association= new Association(){
                Wedding = wedding,
                }
            };
            return View ("ViewWed", MyModel);
        }
    }

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

}