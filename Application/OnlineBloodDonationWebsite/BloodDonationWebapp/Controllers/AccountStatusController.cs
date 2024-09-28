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
    public class AccountStatusController : Controller
    {
        OnlineBloodDonation_DbEntities DB = new OnlineBloodDonation_DbEntities();
        public ActionResult AllAccountStatuses()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login","Home");
            }
            var accountstatuses = DB.AccountStatusTables.ToList();
            var listaccountstatuses = new List<AccountStatusMV>();
            foreach (var accountstatus in accountstatuses)
            {
                var addaccountstatus = new AccountStatusMV();
                addaccountstatus.AccountStatusID = accountstatus.AccountStatusID;
                addaccountstatus.AccountStatus = accountstatus.AccountStatus;
                listaccountstatuses.Add(addaccountstatus);
            }
            return View(listaccountstatuses);
        }

        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login","Home");
            }
            var accountstatus = new AccountStatusMV();
            return View(accountstatus);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AccountStatusMV accountstatusMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login","Home");
            }
            if (ModelState.IsValid)
            {
                var checkaccountstatus = DB.AccountStatusTables.Where(b => b.AccountStatus == accountstatusMV.AccountStatus).FirstOrDefault();
                if (checkaccountstatus == null)
                {
                    var accountstatusesTable = new AccountStatusTable();
                    accountstatusesTable.AccountStatusID = accountstatusMV.AccountStatusID;
                    accountstatusesTable.AccountStatus = accountstatusMV.AccountStatus;
                    DB.AccountStatusTables.Add(accountstatusesTable);
                    DB.SaveChanges();
                    return RedirectToAction("AllAccountStatuses");
                }
                else
                {
                    ModelState.AddModelError("AccountStatus", "Already Exist");
                }
            }
            return View(accountstatusMV);
        }

        public ActionResult Edit(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login","Home");
            }
            var accountstatus = DB.AccountStatusTables.Find(id);
            if (accountstatus == null)
            {
                return HttpNotFound();
            }
            var accountstatusMV  = new AccountStatusMV();
            accountstatusMV.AccountStatusID = accountstatus.AccountStatusID;
            accountstatusMV.AccountStatus = accountstatus.AccountStatus;
            return View(accountstatusMV);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AccountStatusMV accountstatusMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login","Home");
            }
            if (ModelState.IsValid)
            {
                var checkaccountstatus = DB.AccountStatusTables.Where(b => b.AccountStatus == accountstatusMV.AccountStatus && b.AccountStatusID != accountstatusMV.AccountStatusID).FirstOrDefault();
                if (checkaccountstatus == null)
                {
                    var accountstatusesTable = new AccountStatusTable();
                    accountstatusesTable.AccountStatusID = accountstatusMV.AccountStatusID;
                    accountstatusesTable.AccountStatus = accountstatusMV.AccountStatus;

                    DB.Entry(accountstatusesTable).State = EntityState.Modified;
                    DB.SaveChanges();
                    return RedirectToAction("AllAccountStatuses");
                }
                else
                {
                    ModelState.AddModelError("AccountStatus", "Already Exist");
                }

            }
            return View(accountstatusMV);

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
            var accountstatus = DB.AccountStatusTables.Find(id);
            if (accountstatus == null)
            {
                return HttpNotFound();
            }
            var accountstatusMV = new AccountStatusMV();
            accountstatusMV.AccountStatusID = accountstatus.AccountStatusID;
            accountstatusMV.AccountStatus = accountstatus.AccountStatus;
            return View(accountstatusMV);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login","Home");
            }
            var accountstatus = DB.AccountStatusTables.Find(id);
            DB.AccountStatusTables.Remove(accountstatus);
            DB.SaveChanges();
            return RedirectToAction("AllAccountStatuses");
        }
    }
}                                                                                                                                               