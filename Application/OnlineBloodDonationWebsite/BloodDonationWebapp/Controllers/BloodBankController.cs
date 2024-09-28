using BloodDonationWebapp.Helper_Class;
using BloodDonationWebapp.Models;
using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodDonationWebapp.Controllers
{
    public class BloodBankController : Controller
    {
        OnlineBloodDonation_DbEntities DB = new OnlineBloodDonation_DbEntities();
        // GET: BloodBank
        public ActionResult BloodBankStock()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int bloodbankID = 0;
             string bloodbankid = Convert.ToString(Session["BloodBankID"]);
            int.TryParse(bloodbankid, out bloodbankID);
            var list = new List<BloodStockMV>();
            var stocklist = DB.BloodStockTables.Where(b => b.BloodBankID == bloodbankID);

            foreach(var stock in stocklist)
            {
                string bloodbank = stock.BloodBankTable.BloodBankName;
                string bloodgroup = stock.BloodGroupsTable.BloodGroup;
                
                var bloodstockMV= new BloodStockMV();
                bloodstockMV.BloodBankStockID = stock.BloodBankStockID;
                bloodstockMV.BloodGroupID = stock.BloodGroupID;
                bloodstockMV.BloodGroup = bloodgroup; ;
                bloodstockMV.Quantity = stock.Quantity;
                bloodstockMV.Status = stock.Status == true ? "Ready For Use" : "No Ready";
                bloodstockMV.Description = stock.Description;
                bloodstockMV.BloodBankID = stock.BloodBankID;
                bloodstockMV.BloodBank = bloodbank;
                list.Add(bloodstockMV);
    }
            return View(list);
        }
        public ActionResult AllCampaigns()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var bloodbankID = 0;
            int.TryParse(Convert.ToString(Session["BloodBankID"]), out bloodbankID);
            var allcampaigns = DB.CampaignTables.Where(c => c.BloodBankID == bloodbankID).ToList() ;
            if(allcampaigns.Count()>0)
            {
                allcampaigns.OrderByDescending(o => o.CampaignID);
            }
            return View(allcampaigns); 
        }
        public ActionResult NewCampaign()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            } 
            var campaignMV = new CampaignMV();
            return View(campaignMV);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewCampaign(CampaignMV campaignMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var bloodbankID = 0;
            int.TryParse(Convert.ToString(Session["BloodBankID"]), out bloodbankID);
            campaignMV.BloodBankID= bloodbankID;
            if (ModelState.IsValid)
            {
                var campaign = new CampaignTable();
                campaign.BloodBankID = bloodbankID;
                campaign.CampaignID = campaignMV.CampaignID;
                campaign.Campaign = campaignMV.Campaign;
                campaign.CampaignDate = campaignMV.CampaignDate;
                campaign.StartTime = campaignMV.StartTime;
                campaign.EndTime = campaignMV.EndTime;
                campaign.Location = campaignMV.Location;
                campaign.CampaignDetails = campaignMV.CampaignDetails;
                campaign.CampaignPhoto = "~/Content/CampaignPhoto/default_campaign.png";
                DB.CampaignTables.Add(campaign);
                DB.SaveChanges();

                if(campaignMV.CampaignPhotoFile != null)
                {
                    var folder = "~/Content/CampaignPhotos";
                    var file = string.Format("{0}.jpg", campaignMV.CampaignID);
                   var response = FileHelpers.UploadPhoto(campaignMV.CampaignPhotoFile, folder, file);
                    if(response)
                    {
                        var pic = string.Format("{0}/{1}",folder,file); 
                       campaign.CampaignPhoto= pic;
                        DB.Entry(campaign).State = System.Data.Entity.EntityState.Modified;
                        DB.SaveChanges();
                    }
                }
                return RedirectToAction("AllCampaigns");
            }
    
            return View(campaignMV);
        }
        
    }
}