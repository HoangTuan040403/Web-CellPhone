using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Doanphanmem.Models;

namespace Doanphanmem.Admin.Controllers
{
    public class PhanLoaiAdminController : Controller
    {
        private QL_CHDTEntities db = new QL_CHDTEntities();

        // GET: PhanLoaiAdmin
        public ActionResult Index()
        {
            return View(db.PhanLoais.ToList());
        }

        // GET: PhanLoaiAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanLoai phanLoai = db.PhanLoais.Find(id);
            if (phanLoai == null)
            {
                return HttpNotFound();
            }
            return View(phanLoai);
        }

        // GET: PhanLoaiAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PhanLoaiAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaLoai,Tenloai,HinhLoai")] PhanLoai phanLoai)
        {
            if (ModelState.IsValid)
            {
                db.PhanLoais.Add(phanLoai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(phanLoai);
        }

        // GET: PhanLoaiAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanLoai phanLoai = db.PhanLoais.Find(id);
            if (phanLoai == null)
            {
                return HttpNotFound();
            }
            return View(phanLoai);
        }

        // POST: PhanLoaiAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaLoai,Tenloai,HinhLoai")] PhanLoai phanLoai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phanLoai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(phanLoai);
        }

        // GET: PhanLoaiAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanLoai phanLoai = db.PhanLoais.Find(id);
            if (phanLoai == null)
            {
                return HttpNotFound();
            }
            return View(phanLoai);
        }

        // POST: PhanLoaiAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhanLoai phanLoai = db.PhanLoais.Find(id);
            db.PhanLoais.Remove(phanLoai);
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
