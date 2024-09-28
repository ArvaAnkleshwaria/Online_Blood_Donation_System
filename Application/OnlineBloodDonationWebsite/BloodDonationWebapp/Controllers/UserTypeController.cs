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
    public class UserTypeController : Controller
    {
        OnlineBloodDonation_DbEntities DB = new OnlineBloodDonation_DbEntities();
        public ActionResult AllUserType()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login","Home");
            }
            var usertypes = DB.UserTypeTables.ToList();
            var listusertype = new List<UserTypeMV>();
            foreach (var usertype in usertypes)
            {
                var addusertype = new UserTypeMV();
                addusertype.UserTypeID = usertype.UserTypeID;
                addusertype.UseType = usertype.UseType;
                listusertype.Add(addusertype);
            }
            return View(listusertype);
        }
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login","Home");
            }
            var usertype = new UserTypeMV();
            return View(usertype);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserTypeMV userTypeMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login","Home");
            }
            if (ModelState.IsValid)
            {
                var userTypeTable = new UserTypeTable();
                userTypeTable.UserTypeID = userTypeMV.UserTypeID;
                userTypeTable.UseType = userTypeMV.UseType;
                DB.UserTypeTables.Add(userTypeTable);
                DB.SaveChanges();
                return RedirectToAction("AllUserType");
            }
            return View(userTypeMV);
        }

        public ActionResult Edit(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login","Home");
            }
            var usertype = DB.UserTypeTables.Find(id);
            if (usertype == null)
            {
                return HttpNotFound();
            }
            var usertypemv = new UserTypeMV();
            usertypemv.UserTypeID = usertype.UserTypeID;
            usertypemv.UseType = usertype.UseType;
            return View(usertypemv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserTypeMV userTypeMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login","Home");
            }
            if (ModelState.IsValid)
            {
                var userTypeTable = new UserTypeTable();
                userTypeTable.UserTypeID = userTypeMV.UserTypeID;
                userTypeTable.UseType = userTypeMV.UseType;

                DB.Entry(userTypeTable).State = EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("AllUserType");

            }
            return View(userTypeMV);

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
            var usertype = DB.UserTypeTables.Find(id);
            if (usertype == null)
            {
                return HttpNotFound();
            }
            var usertypemv = new UserTypeMV();
            usertypemv.UserTypeID = usertype.UserTypeID;
            usertypemv.UseType = usertype.UseType;
            return View(usertypemv);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login","Home");
            }
            var usertype = DB.UserTypeTables.Find(id);
            DB.UserTypeTables.Remove(usertype);
            DB.SaveChanges();
            return RedirectToAction("AllUserType");
        }
    }
}