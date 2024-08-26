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
        public ActionResult AllBloodGroupsType()
        {
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
            var bloodgroup = new BloodGroupsMV();
            return View(bloodgroup);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BloodGroupsMV bloodgroupsMV)
        {
            if (ModelState.IsValid)
            {
                var bloodGroupsTable = new BloodGroupsTable();
                bloodGroupsTable.BloodGroupID =bloodgroupsMV.BloodGroupID;
                bloodGroupsTable.BloodGroup =bloodgroupsMV.BloodGroup;
                DB.BloodGroupsTables.Add(bloodGroupsTable);
                DB.SaveChanges();
                return RedirectToAction("AllBloodGroupsType");
            }
            return View(bloodgroupsMV);
        }

        public ActionResult Edit(int? id)
        {
            var bloodgroup = DB.BloodGroupsTables.Find(id);
            if (bloodgroup == null)
            {
                return HttpNotFound();
            }
            var bloodgroupsmv = new BloodGroupsMV();
            bloodgroupsmv.BloodGroupID = bloodgroup.BloodGroupID;
            bloodgroupsmv.BloodGroup = bloodgroup.BloodGroup;
            return View(bloodgroupsmv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BloodGroupsMV bloodgroupsMV)
        {
            if (ModelState.IsValid)
            {
                var bloodGroupsTable = new BloodGroupsTable();
                bloodGroupsTable.BloodGroupID = bloodgroupsMV.BloodGroupID;
                bloodGroupsTable.BloodGroup = bloodgroupsMV.BloodGroup;

                DB.Entry(bloodGroupsTable).State = EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("AllBloodGroupsType");

            }
            return View(bloodgroupsMV);

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bloodgroup = DB.BloodGroupsTables.Find(id);
            if (bloodgroup == null)
            {
                return HttpNotFound();
            }
            var bloodgroupsmv = new BloodGroupsMV();
            bloodgroupsmv.BloodGroupID = bloodgroup.BloodGroupID;
            bloodgroupsmv.BloodGroup = bloodgroup.BloodGroup;
            return View(bloodgroupsmv);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int? id)
        {
            var bloodgroup = DB.BloodGroupsTables.Find(id);
            DB.BloodGroupsTables.Remove(bloodgroup);
            DB.SaveChanges();
            return RedirectToAction("AllBloodGroupsType");
        }
    }
}