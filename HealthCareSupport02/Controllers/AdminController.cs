using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HealthCareSupport02.Models;

namespace HealthCareSupport02.Controllers
{
    public class AdminController : Controller
    {
        private dbHCS02Entities db = new dbHCS02Entities();

        // GET: Admin
        public ActionResult Index()
        {
            if(Session["Adlogin"].ToString() == "" && Session["Dologin"].ToString() == "" && Session["Palogin"].ToString() == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(db.Admins.ToList());
        }

        public ActionResult LogOut()
        {
            Session["Ad"] = new Models.Admin();
            Session["Adlogin"] = "";
            Session["msg"] = "";
            Session["Dng"] = "";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AdLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdLogin(Models.Admin ad)
        {
            var v = db.Admins.Where(e => e.Email.ToLower() == ad.Email && e.PassWord == ad.PassWord).ToList();
            if (v.Count <= 0)
            {

                Session["msg"] = "Invalid Login Please Check Your Email Or PassWard";
                return View(ad);
            }

            Session["Ad"] = v.First();
            Session["Adlogin"] = "True";

            return RedirectToAction("Index", "Home");
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if(Session["Adlogin"].ToString() == "" && Session["Dologin"].ToString() == "" && Session["Palogin"].ToString() == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            Session["Dng"] = "";
            return View(admin);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            int? i = ((Models.Admin)Session["Ad"]).Level;
            if (i == 5)
            {
                return View();
            }
            else
            {
                return RedirectToAction("AdLogin");
            }
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,PassWord,Level")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null || ((Models.Admin)Session["Ad"]).Level != id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,PassWord,Level")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null || ((Models.Admin)Session["Ad"]).Level != 5)
            {
                Session["Dng"] = "You Are not Super Admin & Have No Permition Delete any Amin";
                return RedirectToAction("Index");
            }
            if (((Models.Admin)Session["Ad"]).Id == id)
            {
                Session["Dng"] = "Super Admin Have No Permition Delete His Won Account";
                return RedirectToAction("Index");
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
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
