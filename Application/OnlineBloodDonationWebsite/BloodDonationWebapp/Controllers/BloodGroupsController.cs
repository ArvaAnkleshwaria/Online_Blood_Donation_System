using BloodDonationWebapp.Models;
using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BloodDonationWebapp.Controllers
{
    public class BloodGroupsController : Controller
    {
        OnlineBloodDonation_DbEntities DB = new OnlineBloodDonation_DbEntities();
        public ActionResult AllBloodGroups()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login","Home");
            }
            var bloodgroups = DB.BloodGroupsTables.ToList();  
            var listbloodgroups = new List<BloodGroupsMV>();
            foreach (var bloodgroup in bloodgroups)
            {
                var addbloodgroup = new BloodGroupsMV();
                addbloodgroup.BloodGroupID = bloodgroup.BloodGroupID;
                addbloodgroup.BloodGroup = bloodgroup.BloodGroup;
                listbloodgroups.Add(addbloodgroup);
            }
            return View(listbloodgroups);
        }

        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login","Home");
            }
            var bloodgroup = new BloodGroupsMV();
            return View(bloodgroup);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BloodGroupsMV bloodgroupsMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login","Home");
            }
            if (ModelState.IsValid)
            {
                var checkbloodgroup = DB.BloodGroupsTables.Where(b => b.BloodGroup == bloodgroupsMV.BloodGroup).FirstOrDefault();
                if (checkbloodgroup == null)
                {
                    var bloodgroupsTable = new BloodGroupsTable();
                    bloodgroupsTable.BloodGroupID = bloodgroupsMV.BloodGroupID;
                    bloodgroupsTable.BloodGroup = bloodgroupsMV.BloodGroup;
                    DB.BloodGroupsTables.Add(bloodgroupsTable);
                    DB.SaveChanges();
                    return RedirectToAction("AllBloodGroups");
                }
                else
                {
                    ModelState.AddModelError("BloodGroup", "Already Exist");
                }
            }
            return View(bloodgroupsMV);
        }

        public ActionResult Edit(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login","Home");
            }
            var bloodgroup = DB.BloodGroupsTables.Find(id);
            if (bloodgroup == null)
            {
                return HttpNotFound();
            }
            var bloodgroupsMV = new BloodGroupsMV();
            bloodgroupsMV.BloodGroupID = bloodgroup.BloodGroupID;
            bloodgroupsMV.BloodGroup = bloodgroup.BloodGroup;
            return View(bloodgroupsMV);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BloodGroupsMV bloodgroupsMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login","Home");
            }
            if (ModelState.IsValid)
            {
                var checkbloodgroup = DB.BloodGroupsTables.Where(b => b.BloodGroup == bloodgroupsMV.BloodGroup && b.BloodGroupID != bloodgroupsMV.BloodGroupID).FirstOrDefault();
                if (checkbloodgroup == null)
                {
                    var bloodgroupsTable = new BloodGroupsTable();
                    bloodgroupsTable.BloodGroupID = bloodgroupsMV.BloodGroupID;
                    bloodgroupsTable.BloodGroup = bloodgroupsMV.BloodGroup;

                DB.Entry(bloodgroupsMV).State = EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("AllBloodGroups");
                }
                else
                {
                    ModelState.AddModelError("BloodGroup", "Already Exist");
                }

            }
            return View(bloodgroupsMV);

        }

        public ActionResult Delete(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login","Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bloodgroup = DB.BloodGroupsTables.Find(id);
            if (bloodgroup == null)
            {
                return HttpNotFound();
            }
            var bloodgroupsMV = new BloodGroupsMV();
            bloodgroupsMV.BloodGroupID = bloodgroup.BloodGroupID;
            bloodgroupsMV.BloodGroup = bloodgroup.BloodGroup;
            return View(bloodgroupsMV);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login","Home");
            }
            var bloodgroup = DB.BloodGroupsTables.Find(id);
            DB.BloodGroupsTables.Remove(bloodgroup);
            DB.SaveChanges();
            return RedirectToAction("AllBloodGroups");
        }
    }
}