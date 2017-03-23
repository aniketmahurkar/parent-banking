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
    public class AccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Accounts
        public ActionResult Index()
        {

            if (User.IsInRole("Admin"))
            {
                var account = db.Accounts.Include(c => c.AccountName);
                return View(db.Accounts.ToList());
            }
            else
            {
                var account = db.Accounts.Where(x => x.Username == User.Identity.Name);
                return View(db.Accounts.ToList());
            }
            //    else
            //  {
            //    int accountid = 0;
            //  var account = db.Accounts.Where(s => s.RecipientEmail == User.Identity.Name);
            // foreach(Account a in account)
            //{
            //    accountid = a.Id;
            //}
            //return RedirectToAction("Details", "Accounts", new { id = accountid });
        }
    

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.FirstOrDefault(x => x.Id == id && x.Username == User.Identity.Name);
            if (account == null)
            {
                return HttpNotFound();
            }

            GetInterest(id);
            return View(account);
        }

        public PartialViewResult transactions(int? id)
        {
            var accounts = db.Accounts.Single(a => a.Id == id);
            var transactions = db.Transactions.Where(t => t.AccountId == id);
            return PartialView(transactions);

        }

        public PartialViewResult wishlists(int? id)
        {
            var account = db.Accounts.Single(ac => ac.Id == id);
            var wishlist = db.WishLists.Where(t => t.AccountId == id);
            return PartialView(wishlist);

        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OwnerEmail,RecipientEmail,AccountName,Description,Date,InterestRate")] Account account)
        {
            if (ModelState.IsValid)
            {
                
                if (account.RecipientEmail == User.Identity.Name)
                {
                    ModelState.AddModelError("OwnerEmail", "Owner and Recipient emails can not be same.");
                }
                int count = db.Accounts.Where(a => a.RecipientEmail == account.RecipientEmail).Count();

                if (count>0)
                {
                    ModelState.AddModelError("RecipientEmail", "Recipient email already exists.");
                }
                if (ModelState.IsValid)
                {
                    account.Username = User.Identity.Name;
                    account.OwnerEmail = User.Identity.Name;
                    account.Date = DateTime.Now;
                    db.Accounts.Add(account);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(account);
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.FirstOrDefault(x => x.Id == id && x.Username == User.Identity.Name);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OwnerEmail,RecipientEmail,AccountName,Description,Date,InterestRate")] Account account)
        {
            if (account == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            bool userOwnsAccount = db.Accounts
                  .Any(x => x.Id == account.Id && x.Username == User.Identity.Name);

            if (userOwnsAccount == false)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                account.OwnerEmail = User.Identity.Name;
                account.Username = User.Identity.Name;
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(account);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.FirstOrDefault(x => x.Id == id && x.Username == User.Identity.Name);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            if (account.Balance == 0)
            {
                db.Accounts.Remove(account);
                db.SaveChanges();
            }
            
            else
            { 
            ModelState.AddModelError("Balance", "Account can not be deleted as there is a balance in the account.");

                return View("Delete", account);
            }
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
        public void GetInterest(int?id)
        {
            //calculating interest



        }
    }
}
