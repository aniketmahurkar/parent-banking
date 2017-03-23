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
    public class WishListsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: WishLists
        public ActionResult Index()
        {
            
            var wishLists = db.WishLists.Where(x => x.Username == User.Identity.Name).Include(c => c.Account);
            return View(wishLists.ToList());
        }

        // GET: WishLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WishList wishList = db.WishLists.Single(x => x.Id == id);
            if (wishList == null)
            {
                return HttpNotFound();
            }
            return View(wishList);
        }

        // GET: WishLists/Create
        public ActionResult Create()
        {
            if(User.IsInRole("Child"))
            {
                var ac = db.Accounts.Where(a => a.RecipientEmail == User.Identity.Name);
            ViewBag.AccountId = new SelectList(ac, "Id", "RecipientEmail");
            }
            else
            {
                ViewBag.AccountId = new SelectList(db.Accounts, "Id", "RecipientEmail");
            }

            return View();
        }

        // POST: WishLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DateAdded,Cost,Description,Purchased,url,AccountId")] WishList wishList)
        {
            if (ModelState.IsValid)
            {
                
              
                    Account acc = db.Accounts.Single(a => a.Id == wishList.AccountId);

                                               
                
                if(acc.Balance > wishList.Cost)
                {
                wishList.purchasable = 1;
                }
                else
                {
                    wishList.purchasable = 0;
                }

                
                wishList.Username = User.Identity.Name;
                wishList.DateAdded = DateTime.Now;
                db.WishLists.Add(wishList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "RecipientEmail", wishList.AccountId);
            return View(wishList);
        }

        // GET: WishLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var wishList = db.WishLists.FirstOrDefault(x => x.Id == id && x.Username == User.Identity.Name);
            if (wishList == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "RecipientEmail", wishList.AccountId);
            return View(wishList);
        }

        // POST: WishLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateAdded,Cost,Description,Purchased,url,AccountId")] WishList wishList)
        {
            if (wishList == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            bool userOwnsWishlist = db.WishLists
                  .Any(x => x.Id == wishList.Id && x.Username == User.Identity.Name);

            if (userOwnsWishlist == false)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                wishList.Username = User.Identity.Name;
                db.Entry(wishList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "RecipientEmail", wishList.AccountId);
            return View(wishList);
        }

        // GET: WishLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WishList wishList = db.WishLists.Find(id);
            if (wishList == null)
            {
                return HttpNotFound();
            }
            return View(wishList);
        }

        // POST: WishLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WishList wishList = db.WishLists.Find(id);
            db.WishLists.Remove(wishList);
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
