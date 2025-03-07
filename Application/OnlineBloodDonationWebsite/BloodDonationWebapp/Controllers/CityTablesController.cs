﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseLayer;

namespace BloodDonationWebapp.Controllers
{
    public class CityTablesController : Controller
    {
        private OnlineBloodDonation_DbEntities db = new OnlineBloodDonation_DbEntities();

        // GET: CityTables
        public ActionResult Index()
        {
            return View(db.CityTables.ToList());
        }

        // GET: CityTables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CityTable cityTable = db.CityTables.Find(id);
            if (cityTable == null)
            {
                return HttpNotFound();
            }
            return View(cityTable);
        }

        // GET: CityTables/Create
        public ActionResult Create()
        {
            var city = new CityTable();
            return View(city);
        }

        // POST: CityTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CityID,City")] CityTable cityTable)
        {
            if (ModelState.IsValid)
            {
                db.CityTables.Add(cityTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cityTable);
        }

        // GET: CityTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CityTable cityTable = db.CityTables.Find(id);
            if (cityTable == null)
            {
                return HttpNotFound();
            }
            return View(cityTable);
        }

        // POST: CityTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CityID,City")] CityTable cityTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cityTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cityTable);
        }

        // GET: CityTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CityTable cityTable = db.CityTables.Find(id);
            if (cityTable == null)
            {
                return HttpNotFound();
            }
            return View(cityTable);
        }

        // POST: CityTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CityTable cityTable = db.CityTables.Find(id);
            db.CityTables.Remove(cityTable);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
