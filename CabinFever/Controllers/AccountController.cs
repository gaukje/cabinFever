using CabinFever.Models;
using Microsoft.AspNetCore.Mvc;

namespace CabinFever.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult LoggInn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoggInn(string brukernavn, string passord) {
            using (var context = new UserDbContext())
            {
                var dbUser = context.Users.SingleOrDefault(u => u.Brukernavn == brukernavn && u.Passord == passord);
                if (dbUser != null) {
                    return RedirectToAction("MinSide", "Home");}
            }
            return View();
        }

        public IActionResult Registrer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrer(string fornavn, string etternavn, string brukernavn, string epost, string passord, string bekreftPassord)
        {
            if (passord == bekreftPassord)
            {
                using (var context = new UserDbContext())
                {
                    var user = new User()
                    {
                        Fornavn = fornavn,
                        Etternavn = etternavn,
                        Brukernavn = brukernavn,
                        Epost = epost,
                        Passord = passord
                    };
                    context.Users.Add(user);
                    context.SaveChanges();
                }
                return RedirectToAction("LoggInn");
            } else
            {
                ViewBag.Message = "Passord er ikke like.";
                return View();
            }
        }
    }
}
