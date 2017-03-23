using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bank_Application.Models;

namespace Bank_Application.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        public ActionResult Index()
        {

            var transaction = db.Transactions.Where(x => x.Username == User.Identity.Name);
            var TransactionDate = DateTime.Now; 
            return View(transaction.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var transaction = db.Transactions.FirstOrDefault(x => x.Id == id && x.Username == User.Identity.Name);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "RecipientEmail");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TransactionDate,Amount,Note,AccountId")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                Account acc = db.Accounts.Single(ac => ac.Id == transaction.AccountId);
                
                    if(acc.Balance+transaction.Amount<0)
                    {
                        ModelState.AddModelError("Amount", "Amount can not be withdrawn due to low balance.");
                    }
                
                if (ModelState.IsValid) { 
                transaction.Username = User.Identity.Name;
                db.Transactions.Add(transaction);

                db.SaveChanges();
                UpdateBalance(transaction.Amount,transaction.AccountId);
                    return RedirectToAction("Index");
                }
                
            }

            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "RecipientEmail", transaction.AccountId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.FirstOrDefault(x => x.Id == id && x.Username == User.Identity.Name);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "RecipientEmail", transaction.AccountId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TransactionDate,Amount,Note,AccountId")] Transaction transaction)
        {
            if (transaction == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            bool userOwnsTransaction = db.Transactions
                  .Any(x => x.Id == transaction.Id && x.Username == User.Identity.Name);

            if (userOwnsTransaction == false)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                transaction.Username = User.Identity.Name;
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "RecipientEmail", transaction.AccountId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
            db.SaveChanges();
            ReduceBalance(transaction.Amount, transaction.AccountId);
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
        public void UpdateBalance(float Amount,int AccountId)
        {
            Account account = db.Accounts.Single(x => x.Id == AccountId);
            
                account.Balance = account.Balance + Amount;
            
            
                db.SaveChanges();
            
        }
        public void ReduceBalance(float Amount, int AccountId)
        {
            Account account = db.Accounts.Single(x => x.Id == AccountId);

            account.Balance = account.Balance - Amount;

            db.SaveChanges();

        }
    }
}
