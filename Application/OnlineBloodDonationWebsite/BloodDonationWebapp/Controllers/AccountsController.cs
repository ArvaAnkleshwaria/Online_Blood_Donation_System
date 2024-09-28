using BloodDonationWebapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseLayer;

namespace BloodDonationWebapp.Controllers
{
    public class AccountsController : Controller
    {
        OnlineBloodDonation_DbEntities DB = new OnlineBloodDonation_DbEntities();
        public ActionResult AllNewUserRequest()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var users = DB.UserTables.Where(u => u.AccountStatusID == 1).ToList();
            return View(users);
        }
        public ActionResult UserDetails(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var user = DB.UserTables.Find(id);
            return View(user);
        }
        public ActionResult UserApproved(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var user = DB.UserTables.Find(id);
            user.AccountStatusID = 2;
            DB.Entry(user).State = System.Data.Entity.EntityState.Modified;
            DB.SaveChanges();
            return RedirectToAction("AllNewUserRequest");
        }
        public ActionResult UserRejected(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var user = DB.UserTables.Find(id);
            user.AccountStatusID = 3;
            DB.Entry(user).State = System.Data.Entity.EntityState.Modified;
            DB.SaveChanges();
            return RedirectToAction("AllNewUserRequest");
        }
        public ActionResult AddNewDonorByBloodBank()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var collectbloodMV = new CollectBloodMV();
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", "0");
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", "0");
            ViewBag.GenderID = new SelectList(DB.GenderTables.ToList(), "GenderID", "Gender", "0");
            return View(collectbloodMV);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewDonorByBloodBank(CollectBloodMV collectBloodMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int bloodbankID = 0;
            string bloodbankid = Convert.ToString(Session["BloodBankID"]);
            int.TryParse(bloodbankid, out bloodbankID);
            var currentdate=DateTime.Now;
            var currentcampaign = DB.CampaignTables.Where(c => c.CampaignDate == currentdate && c.BloodBankID == bloodbankID).FirstOrDefault();
            
            if (ModelState.IsValid)
            {
                using (var transaction = DB.Database.BeginTransaction())
                {
                    try
                    {
                        var checkdonor = DB.DonorTables.Where(d => d.AadharNo.Trim().Replace("-", "") == collectBloodMV.DonorDetails.AadharNo.Trim().Replace("-", "")).FirstOrDefault();
                        if (checkdonor == null)
                        {
                            var user = new UserTable();
                            user.UserName = collectBloodMV.DonorDetails.FullName.Trim();
                            user.EmailAddress = collectBloodMV.DonorDetails.EmailAddress;
                            user.Password = "12345";
                            user.AccountStatusID = 2;
                            user.UserTypeID = 2;
                            user.Description = "Add By Blood Bank";
                            DB.UserTables.Add(user);
                            DB.SaveChanges();

                            var donor = new DonorTable();
                            donor.FullName = collectBloodMV.DonorDetails.FullName;
                            donor.BloodGroupID = collectBloodMV.BloodGroupID;
                            donor.Location = collectBloodMV.DonorDetails.Location;
                            donor.ContactNo = collectBloodMV.DonorDetails.ContactNo;
                            donor.LastDonationDate = DateTime.Now;
                            donor.AadharNo = collectBloodMV.DonorDetails.AadharNo;
                            donor.GenderID = collectBloodMV.GenderID;
                            donor.CityID = collectBloodMV.CityID;
                            donor.UserID = user.UserID;
                            DB.DonorTables.Add(donor);
                            DB.SaveChanges();
                            checkdonor = DB.DonorTables.Where(d => d.AadharNo.Trim().Replace("-", "") == collectBloodMV.DonorDetails.AadharNo.Trim().Replace("-", "")).FirstOrDefault();
                        }

                        if ((DateTime.Now - checkdonor.LastDonationDate).TotalDays < 120)
                        {
                            ModelState.AddModelError(string.Empty, "Donor has already donated blood in last 120 Days!");
                            transaction.Rollback();
                        }
                        else
                        {

                            var checkbloodgroupstock = DB.BloodStockTables.Where(s => s.BloodBankID == bloodbankID && s.BloodBankID == collectBloodMV.BloodGroupID).FirstOrDefault();
                            if (checkbloodgroupstock == null)
                            {
                                var bloodbankstock = new BloodStockTable();
                                bloodbankstock.BloodGroupID = collectBloodMV.BloodGroupID;
                                bloodbankstock.BloodBankID = bloodbankID;
                                bloodbankstock.Quantity = 0;
                                bloodbankstock.Status = true;
                                bloodbankstock.Description = "";
                                DB.BloodStockTables.Add(bloodbankstock);
                                DB.SaveChanges();
                                checkbloodgroupstock = DB.BloodStockTables.Where(s => s.BloodBankID == bloodbankID && s.BloodBankID == collectBloodMV.BloodGroupID).FirstOrDefault();
                            }
                            var collectedblooddetails = new BloodStockDetailTable();
                            
                                checkbloodgroupstock.Quantity += collectBloodMV.Quantity;
                                DB.Entry(checkbloodgroupstock).State = System.Data.Entity.EntityState.Modified;
                                DB.SaveChanges();
                                collectedblooddetails.BloodBankStockID = checkbloodgroupstock.BloodBankStockID;
                           
                            
                            
                            collectedblooddetails.BloodGroupID = collectBloodMV.BloodGroupID;
                           
                                collectedblooddetails.CampaignID = currentcampaign.CampaignID;
                           
                            collectedblooddetails.Quantity = collectBloodMV.Quantity;
                            
                                collectedblooddetails.DonorID = checkdonor.DonorID;
                            collectedblooddetails.DonationDate = DateTime.Now;
                            DB.BloodStockDetailTables.Add(collectedblooddetails);
                            DB.SaveChanges();

                            checkdonor.LastDonationDate = DateTime.Now;
                            DB.Entry(checkdonor).State = System.Data.Entity.EntityState.Modified;
                            DB.SaveChanges();
                            transaction.Commit();
                            return RedirectToAction("BloodBankStock", "BloodBank");
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError(string.Empty, "Please Provide The Correct Information!");
                        transaction.Rollback();
                    }

                }

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please Provide Donor's Compelete Information!");
            }
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", collectBloodMV.CityID);
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", collectBloodMV.BloodGroupID);
            ViewBag.GenderID = new SelectList(DB.GenderTables.ToList(), "GenderID", "Gender", collectBloodMV.GenderID);

            return View(collectBloodMV);
        }
    }
}