using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ActivityCenter.Models;

namespace ActivityCenter.Controllers
{
    public class ActivityController : Controller
    {
        private ActivityContext context;
        
        public ActivityController(ActivityContext ac)
        {
            context = ac;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                var dbUser = context.UserList.FirstOrDefault(u => u.Email  == newUser.Email);
                if(dbUser != null)
                {
                    ModelState.AddModelError("Email", "This Email is already in use");
                    return View("Index");
                }
                else
                {
                    PasswordHasher<User> PWHasher = new PasswordHasher<User>();
                    newUser.Password = PWHasher.HashPassword(newUser, newUser.Password);
                    context.UserList.Add(newUser);
                    context.SaveChanges();
                    HttpContext.Session.SetInt32("UserId", newUser.UserId);
                    return Redirect("home");
                }
            }
            return View("Index");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUser User)
        {
            if(ModelState.IsValid)
            {
                var user = context.UserList.FirstOrDefault(u => u.Email == User.LoginEmail);
                if(user == null)
                {
                    ModelState.AddModelError("LoginEmail", "This Email doesn't exist, please Register");
                    return View("Index");
                }
                else
                {
                    var hasher = new PasswordHasher<LoginUser>();
                    var result = hasher.VerifyHashedPassword(User, user.Password, User.LoginPassword);
                    if(result == 0)
                    {
                        ModelState.AddModelError("LoginPassword", "Incorrect Password");
                    }
                    else
                    {
                        HttpContext.Session.SetInt32("UserId", user.UserId);
                        return Redirect("home");
                    }
                }
            }
            return View("Index");
        }

        [HttpGet("home")]
        public IActionResult Home()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return Redirect("/");
            }
            else
            {
                User User = context.UserList.FirstOrDefault(x => x.UserId == HttpContext.Session.GetInt32("UserId"));
                List<ActivityEvent> Activities = context.ActivityList.Include(x => x.Coordinator).Include(y => y.Attendees).ThenInclude(z => z.AUser).ToList();
                ViewBag.Activities = Activities.OrderBy(z => z.ActivityStart);
                ViewBag.User = User;
                return View();
            }
        }

        [HttpGet("{activityId}")]
        public IActionResult Display(int activityId)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return Redirect("/");
            }
            else
            {
                User User = context.UserList.FirstOrDefault(x => x.UserId == HttpContext.Session.GetInt32("UserId"));
                ActivityEvent Event = context.ActivityList.Include(x => x.Coordinator).Include(y => y.Attendees).ThenInclude(z => z.AUser).FirstOrDefault(w => w.ActivityEventId == activityId);
                ViewBag.ActivityEvent = Event;
                ViewBag.User = User;
                
                List<Join> userAttending = context.Joiners.Where(a => a.UserId == User.UserId).Include(r => r.ActivityEvent).ToList();
                List<int> attendingIds = new List<int>();
                foreach (Join r in userAttending)
                {
                    attendingIds.Add(r.ActivityEventId);
                }
                ViewBag.UserAttending = attendingIds;
                return View();
            }
        }

        [HttpGet("new")]
        public IActionResult Create()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return Redirect("/");
            }
            else
            {
                User User = context.UserList.FirstOrDefault(x => x.UserId == HttpContext.Session.GetInt32("UserId"));
                ViewBag.User = User;
                return View();
            }
        }

        [HttpPost("generateActivity")]
        public IActionResult GenerateActivity(ActivityEvent newEvent)
        {
            if(ModelState.IsValid)
            {
                Join newJoin = new Join();
                newEvent.UserID = (int)HttpContext.Session.GetInt32("UserId");
                context.ActivityList.Add(newEvent);
                context.SaveChanges();
                newJoin.ActivityEventId = newEvent.ActivityEventId;
                newJoin.UserId = newEvent.UserID;
                context.Joiners.Add(newJoin);
                context.SaveChanges();
                return Redirect($"{newEvent.ActivityEventId}");
            }
            else
            {
                return View("Create");
            }
        }

        [HttpGet("delete/{activityId}")]
        public IActionResult Delete(int activityId)
        {
            ActivityEvent Event = context.ActivityList.FirstOrDefault(x => x.ActivityEventId == activityId);
            context.ActivityList.Remove(Event);
            context.SaveChanges();
            return RedirectToAction("Home");
        }

        [HttpGet("join/{eventId}/{userId}")]
        public IActionResult Attend(int eventId, int userId)
        {
            Join newJoin = new Join();
            newJoin.UserId = userId;
            newJoin.ActivityEventId = eventId;
            context.Joiners.Add(newJoin);
            context.SaveChanges();
            return RedirectToAction("Home");
        }

        [HttpGet("leave/{eventId}/{userId}")]
        public IActionResult Leave(int eventId, int userId)
        {
            Join Leave = context.Joiners.FirstOrDefault(x => x.ActivityEventId == eventId && x.UserId == userId);
            context.Joiners.Remove(Leave);
            context.SaveChanges();
            return RedirectToAction("Home");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}
