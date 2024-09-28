using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodDonationWebapp.Controllers
{
    public class FinderController : Controller
    {
        // GET: Finder
        public ActionResult FindDonors()
        {
            return View();
        }
    }
}