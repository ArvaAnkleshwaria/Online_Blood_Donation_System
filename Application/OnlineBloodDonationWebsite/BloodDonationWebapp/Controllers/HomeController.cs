using BloodDonationWebapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseLayer;
using System.Threading;

namespace BloodDonationWebapp.Controllers
{
    public class HomeController : Controller
    {
        OnlineBloodDonation_DbEntities DB = new OnlineBloodDonation_DbEntities();
        public ActionResult AllCampaign()
        {
            var date =  DateTime.Now.Date;
            var allcampaigns = DB.CampaignTables.Where(c => c.CampaignDate >= date).ToList();
            return View(allcampaigns);
        }
        public ActionResult MainHome()
        {
            //var message = ViewData["Message"] == null ? "Welcome To Online Blood Donation!" : ViewData["Message"];
            //ViewData["Message"] = message;

            var date = DateTime.Now.Date;
            List<CampaignTable> allcampaigns = DB.CampaignTables.Where(c => c.CampaignDate >= date).ToList();
            ViewBag.AllCampaign=allcampaigns;
            var registration = new RegistrationMV();
            ViewBag.UserTypeID = new SelectList(DB.UserTypeTables.Where(ut => ut.UserTypeID > 1).ToList(), "UserTypeID", "UseType", "0");
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", "0");
            return View(registration);
        }
        public ActionResult Login()
        {
            var usermv = new UserMV();
            return View(usermv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserMV userMV)
        {
            if (ModelState.IsValid)
            {
                var user = DB.UserTables.Where(u => u.Password == userMV.Password && u.UserName == userMV.UserName).FirstOrDefault();
                if (user != null)
                {
                    if (user.AccountStatusID == 1)
                    {
                        ModelState.AddModelError(string.Empty, " Your account is Under-Review!");
                    }
                    else if (user.AccountStatusID == 3)
                    {
                        ModelState.AddModelError(string.Empty, " Your account is Rejected!,For more details Contact-us");
                    }
                    else if (user.AccountStatusID == 4)
                    {
                        ModelState.AddModelError(string.Empty, "Your account is Suspended!,For more details Contact-us");
                    }
                    else if (user.AccountStatusID == 2)
                    {
                        Session["UserID"] = user.UserID;
                        Session["UserName"] = user.UserName;
                        Session["EmailAddress"] = user.EmailAddress;
                        Session["Password"] = user.Password;
                        Session["AccountStatusID"] = user.AccountStatusID;
                        Session["AccountStatus"] = user.AccountStatusTable.AccountStatus;
                        Session["UserTypeID"] = user.UserTypeID;
                        Session["UseType"] = user.UserTypeTable.UseType;
                        Session["Description"] = user.Description;
                        if (user.UserTypeID == 1) //Admin
                        {
                            return RedirectToAction("AllNewUserRequest","Accounts");
                        }
                        else if (user.UserTypeID == 2) //Donor
                        {
                            var donor = DB.DonorTables.Where(t => t.UserID == user.UserID).FirstOrDefault();
                            if (donor != null)
                            {
                                Session["DonorID"] = donor.DonorID;
                                Session["FullName"] = donor.FullName;
                                Session["GenderID"] = donor.GenderID;
                                Session["BloodGroupID"] = donor.BloodGroupID;
                                Session["LastDonationDate"] = donor.LastDonationDate;
                                Session["ContactNo"] = donor.ContactNo;
                                Session["AadharNo"] = donor.AadharNo;
                                Session["Location"] = donor.Location;
                                Session["CityID"] = donor.CityID;
                                Session["City"] = donor.CityTable.City;
                                return RedirectToAction("Donor","Dashboard");
                            }
                            else
                            {
                               
                                ModelState.AddModelError(string.Empty, "Username Or Password is Incorrect!");

                            }
                        }
                        else if (user.UserTypeID == 3) //seeker
                        {
                            var seeker = DB.SeekerTables.Where(t => t.UserID == user.UserID).FirstOrDefault();
                            if (seeker != null)
                            {
                                Session["SeekerID"]=seeker.SeekerID;
                                Session["FullName"] = seeker.FullName;
                                Session["Age"] = seeker.Age;
                                Session["CityID"] = seeker.CityID;
                                Session["City"] = seeker.CityTable.City;
                                Session[" BloodGroupID"] = seeker.BloodGroupID;
                                Session["BloodGroup"] = seeker.BloodGroupsTable.BloodGroup;
                                Session["ContactNo"] = seeker.ContactNo;
                                Session["AadharNo"] = seeker.AadharNo;
                                Session["GenderID"] = seeker.GenderID;
                                Session["Gender"] = seeker.GenderTable.Gender;
                                Session["RegistrationDate"] = seeker.RegistrationDate;
                                Session["Address"] = seeker.Address;
                                return RedirectToAction("Seeker", "Dashboard");

                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Username Or Password is Incorrect!");

                            }
                        }
                        else if (user.UserTypeID == 4) //hospital
                        {
                            var hospital = DB.HospitalTables.Where(t => t.UserID == user.UserID).FirstOrDefault();
                            if (hospital != null)
                            {
                                Session["HospitalID"] = hospital.HospitalID;
                                Session["FullName"] = hospital.FullName;
                                Session["Address"] = hospital.Address;
                                Session["PhoneNo"] = hospital.PhoneNo;
                                Session["Website"] = hospital.Website;
                                Session["Email"] = hospital.Email;
                                Session["Location"] = hospital.Location;
                                Session["CityID"] = hospital.CityID;
                                Session[" City"] = hospital.CityTable.City;
                                return RedirectToAction("Hospital", "Dashboard");

                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Username Or Password is Incorrect!");

                            }
                        }
                        else if (user.UserTypeID == 5) //bloodbank
                        {
                            var bloodbank = DB.BloodBankTables.Where(t => t.UserID == user.UserID).FirstOrDefault();
                            if (bloodbank != null)
                            {
                                Session["BloodBankID"] = bloodbank.BloodBankID;
                                Session["BloodBankName"] = bloodbank.BloodBankName;
                                Session["Address"] = bloodbank.Address;
                                Session["PhoneNo"] = bloodbank.PhoneNo;
                                Session["Location"] = bloodbank.Location;
                                Session["Website"] = bloodbank.Website;
                                Session["Email"] = bloodbank.Email;
                                Session[" CityID"] = bloodbank.CityID;
                                Session["City"] = bloodbank.CityTable.City;
                                return RedirectToAction("BloodBank", "Dashboard");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Username Or Password is Incorrect!");

                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Username Or Password is Incorrect!");
                        }
                    }
                }

                else
                {
                    ModelState.AddModelError(string.Empty, "Username Or Password is Incorrect!");
                }
            }

            else
            {
                
                ModelState.AddModelError(string.Empty, "Please Provide Username & Password!");
            }
            //clearsession();
            return View(userMV);
        }
        public void clearsession()
        {
            Session["UserID"] = string.Empty;
            Session["UserName"] = string.Empty;
            Session["EmailAddress"] = string.Empty;
            Session["Password"] = string.Empty;
            Session["AccountStatusID"] = string.Empty;
            Session["AccountStatus"] = string.Empty;
            Session["UserTypeID"] = string.Empty;
            Session["UseType"] = string.Empty;
            Session["Description"] = string.Empty;
        }
        public ActionResult Logout()
        {
            clearsession();
            return RedirectToAction("MainHome");
        }
        public ActionResult AboutUs()
        {
            return View();
        }
    }
}
  