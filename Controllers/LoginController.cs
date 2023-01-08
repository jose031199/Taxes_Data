using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxes_Data.Models;

namespace Taxes_Data.Controllers
{
    public class LoginController : Controller
    {
        private readonly DBTaxes_RegisterContext DBContext;

        public LoginController(DBTaxes_RegisterContext _context)
        {
            DBContext = _context;
        }
        public IActionResult Admin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Admin(Users user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                var findUser = DBContext.Users.Where(u => u.Username.Equals(user.Username) && u.Password.Equals(user.Password)).ToList();
                if (findUser.Count() == 1)
                {
                    //return RedirectToAction("AdminDashboard", "Admin");
                    HttpContext.Session.SetString("username", findUser[0].Username.ToString());
                    return RedirectToAction("AdminDashboard", "Admin");
                }
                else
                {
                    return View();
                }
            }
        }
    }
}
