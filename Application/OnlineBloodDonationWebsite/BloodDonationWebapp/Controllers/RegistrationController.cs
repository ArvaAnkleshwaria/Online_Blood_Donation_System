using BloodDonationWebapp.Models;
using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.Mvc;

namespace BloodDonationWebapp.Controllers
{
    public class RegistrationController : Controller
    {
        OnlineBloodDonation_DbEntities DB = new OnlineBloodDonation_DbEntities();
        static RegistrationMV registrationmv;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectUser(RegistrationMV registrationMV)
        {
            registrationmv=registrationMV;
            if (registrationMV.UserTypeID == 2)
            {
                return RedirectToAction("DonorUser");
            }
            else if (registrationMV.UserTypeID == 3)
            {
                return RedirectToAction("SeekerUser");
            }
            else if (registrationMV.UserTypeID == 4)
            {
                return RedirectToAction("HospitalUser");
            }
            else if (registrationMV.UserTypeID == 5)
            {
                return RedirectToAction("BloodBankUser");
            }
            else
            {
                return RedirectToAction("MainHome", "Home");
            }

          
        }
        public ActionResult HospitalUser()
        {
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationmv.CityID);
            return View(registrationmv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HospitalUser(RegistrationMV registrationMV)
        {
            if(ModelState.IsValid)
            {
                var checktitle = DB.HospitalTables.Where(h => h.FullName == registrationMV.Hospital.FullName.Trim()).FirstOrDefault();

                if (checktitle == null)
                {
                    using (var transaction = DB.Database.BeginTransaction())
                    {
                        try
                        {
                            var user = new UserTable();
                            user.UserName = registrationMV.User.UserName;
                            user.EmailAddress = registrationMV.User.EmailAddress;
                            user.Password = registrationMV.User.Password;
                            user.AccountStatusID = 1;
                            user.UserTypeID = registrationMV.UserTypeID;
                            user.Description = registrationMV.User.Description;
                            DB.UserTables.Add(user);
                            DB.SaveChanges();

                            var hospital = new HospitalTable();
                            hospital.FullName = registrationMV.Hospital.FullName;
                            hospital.Address = registrationMV.Hospital.Address;
                            hospital.PhoneNo = registrationMV.Hospital.PhoneNo;
                            hospital.Website = registrationMV.Hospital.Website;
                            hospital.Email = registrationMV.Hospital.Email;
                            hospital.Location = registrationMV.Hospital.Address;
                            hospital.CityID = registrationMV.CityID;
                            hospital.UserID = user.UserID;
                            DB.HospitalTables.Add(hospital);
                            DB.SaveChanges();
                            transaction.Commit();
                            ViewData["Message"] = "Thank You For Registering,Your Query will be Review Shortly!";
                            return RedirectToAction("MainHome","Home");

                        }
                        catch 
                        {
                            ModelState.AddModelError(string.Empty,"Please Provide The Correct Information!");
                            transaction.Rollback();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Hospital Already Registered");
       
                }
            }
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationmv.CityID);
            return View();
        }
        public ActionResult DonorUser()
        { 
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City",registrationmv.CityID);
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", "0");
            ViewBag.GenderID = new SelectList(DB.GenderTables.ToList(), "GenderID", "Gender", "0");
            return View(registrationmv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DonorUser(RegistrationMV registrationMV)
        {
            if (ModelState.IsValid)
            {
                var checktitle = DB.DonorTables.Where(h => h.FullName == registrationMV.Donor.FullName.Trim() && h.AadharNo==registrationMV.Donor.AadharNo).FirstOrDefault();
                
                if (checktitle == null)
                {
                    
                    using (var transaction = DB.Database.BeginTransaction())
                    {
                        try
                        {
                            var user = new UserTable();
                            user.UserName = registrationMV.User.UserName;
                            user.EmailAddress = registrationMV.User.EmailAddress;
                            user.Password = registrationMV.User.Password;
                            user.AccountStatusID = 1;
                            user.UserTypeID = registrationMV.UserTypeID;
                            user.Description = registrationMV.User.Description;
                            DB.UserTables.Add(user);
                            DB.SaveChanges();

                            var donor = new DonorTable();
                            donor.FullName = registrationMV.Donor.FullName;
                            donor.BloodGroupID = registrationMV.BloodGroupID;
                            donor.Location = registrationMV.Donor.Location;
                            donor.ContactNo = registrationMV.Donor.ContactNo;
                            donor.LastDonationDate = registrationMV.Donor.LastDonationDate;
                            donor.AadharNo= registrationMV.Donor.AadharNo;
                            donor .GenderID = registrationMV.GenderID;
                            donor.CityID = registrationMV.CityID;
                            donor.UserID = user.UserID;
                            DB.DonorTables.Add(donor);
                            DB.SaveChanges();
                            transaction.Commit();
                           // ViewData["Message"] = "Thank You For Registering,Your Query will be Review Shortly!";
                            return RedirectToAction("MainHome","Home");

                        }
                        catch
                        {
                            ModelState.AddModelError(string.Empty,"Please Provide Correct Information");
                            transaction.Rollback();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Donor Already Registered");

                }
            }
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationmv.CityID);
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup",registrationMV.BloodGroupID);
            return View(registrationMV);
        }
        public ActionResult BloodBankUser()
        {
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City",registrationmv.CityID);
            return View(registrationmv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BloodBankUser(RegistrationMV registrationMV)
        {
            if (ModelState.IsValid)
            {
                var checktitle = DB.BloodBankTables.Where(h => h.BloodBankName == registrationMV.BloodBank.BloodBankName.Trim()&& h.PhoneNo==registrationMV.BloodBank.PhoneNo).FirstOrDefault();

                if (checktitle == null)
                {
                    using (var transaction = DB.Database.BeginTransaction())
                    {
                        try
                        { 
                            var user = new UserTable();
                            user.UserName = registrationMV.User.UserName;
                            user.EmailAddress = registrationMV.User.EmailAddress;
                            user.Password = registrationMV.User.Password;
                            user.AccountStatusID = 1;
                            user.UserTypeID = registrationMV.UserTypeID;
                            user.Description = registrationMV.User.Description;
                            DB.UserTables.Add(user);
                            DB.SaveChanges();

                            var bloodbank = new BloodBankTable();
                            bloodbank.BloodBankName = registrationMV.BloodBank.BloodBankName;
                            bloodbank.Address = registrationMV.BloodBank.Address;
                            bloodbank.Location = registrationMV.BloodBank.Location;
                            bloodbank.PhoneNo = registrationMV.BloodBank.PhoneNo;
                            bloodbank.Website = registrationMV.BloodBank.Website;
                            bloodbank.Email = registrationMV.BloodBank.Email;
                            bloodbank.CityID = registrationMV.CityID;
                            bloodbank.UserID = user.UserID;
                            DB.BloodBankTables.Add(bloodbank);
                            DB.SaveChanges();
                            transaction.Commit();
                            //ViewData["Message"] = "Thank You For Registering,Your Query will be Review Shortly!";
                            return RedirectToAction("MainHome","Home");
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError(string.Empty, "Please Provide Correct Information" + ex.Message.ToString());
                            transaction.Rollback();
                        }



                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "BloodBank Already Registered");

                }
                          
            }
            else
            {
               
            }

            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationMV.CityID);
            return View(registrationMV);

        }
        public ActionResult SeekerUser()
        {
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City","0");
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", "0");
            ViewBag.GenderID = new SelectList(DB.GenderTables.ToList(), "GenderID", "Gender", "0");
            return View(registrationmv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeekerUser(RegistrationMV registrationMV)
        {

            if (ModelState.IsValid)
            {
                var checktitle = DB.SeekerTables.Where(h => h.FullName == registrationMV.Seeker.FullName.Trim() && h.AadharNo == registrationMV.Seeker.AadharNo).FirstOrDefault();

                if (checktitle == null)
                {

                    using (var transaction = DB.Database.BeginTransaction())
                    {
                        try
                        {
                            var user = new UserTable();
                            user.UserName = registrationMV.User.UserName;
                            user.EmailAddress = registrationMV.User.EmailAddress;
                            user.Password = registrationMV.User.Password;
                            user.AccountStatusID = 1;
                            user.UserTypeID = registrationMV.UserTypeID;
                            user.Description = registrationMV.User.Description;
                            DB.UserTables.Add(user);
                            DB.SaveChanges();

                            var seeker = new SeekerTable();
                            seeker.FullName = registrationMV.Seeker.FullName;
                            seeker.BloodGroupID = registrationMV.BloodGroupID;
                            seeker.Address = registrationMV.Seeker.Address;
                            seeker.Age= registrationMV.Seeker.Age;  
                            seeker.ContactNo = registrationMV.Seeker.ContactNo;
                            seeker.RegistrationDate = registrationMV.Seeker.RegistrationDate;
                            seeker.AadharNo = registrationMV.Seeker.AadharNo;
                            seeker.GenderID = registrationMV.GenderID;
                            seeker.CityID = registrationMV.CityID;
                            seeker.UserID = user.UserID;
                            DB.SeekerTables.Add(seeker);
                            DB.SaveChanges();
                            transaction.Commit();
                            // ViewData["Message"] = "Thank You For Registering,Your Query will be Review Shortly!";
                            return RedirectToAction("MainHome", "Home");

                        }
                        catch
                        {
                            ModelState.AddModelError(string.Empty, "Please Provide Correct Information");
                            transaction.Rollback();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Seeker Already Registered");

                }
            }
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationmv.CityID);
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", registrationMV.BloodGroupID);
            ViewBag.GenderID = new SelectList(DB.GenderTables.ToList(), "GenderID", "Gender",registrationMV.GenderID);
            return View(registrationMV);
        }

    }
}