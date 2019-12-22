using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LinqKit;
using Microsoft.Ajax.Utilities;
using OneTech.Models;

namespace OneTech.Controllers
{
    public class ClassesController : Controller
    {
        private MyContext db = new MyContext();
        public ActionResult GetChartData(string start, string end)
        {
            var startTime = DateTime.Now;
            startTime = startTime.AddYears(-1);
            try
            {
                startTime = DateTime.Parse(start);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            startTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, 0, 0, 0, 0);

            var endTime = DateTime.Now;
            try
            {
                endTime = DateTime.Parse(end);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            endTime = new DateTime(endTime.Year, endTime.Month, endTime.Day, 23, 59, 59, 0);

            var data = db.Classes.Where(s => s.DeletedAt != null && (s.CreatedAt >= startTime && s.CreatedAt <= endTime))
                .GroupBy(
                    s => new
                    {
                        Year = s.CreatedAt.Year,
                        Month = s.CreatedAt.Month,
                        Day = s.CreatedAt.Day
                    }
                ).Select(s => new
                {
                    Date = s.FirstOrDefault().CreatedAt,
                    Count = s.Count()
                }).OrderBy(s => s.Date).ToList();
            return new JsonResult()
            {
                Data = data.Select(s => new
                {
                    Date = s.Date.ToString("MM/dd/yyyy"),
                    Count = s.Count
                }),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        // GET: Classes
        public ActionResult Index(int? page, int? limit, string start, string end, String keyword)
        {
            if (page == null)
            {
                page = 1;
            }

            if (limit == null)
            {
                limit = 20;
            }
            var startTime = DateTime.Now;
            startTime = startTime.AddYears(-100);
            try
            {
                startTime = DateTime.Parse(start);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            var endTime = DateTime.Now;
            try
            {
                endTime = DateTime.Parse(end);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            var predicate = PredicateBuilder.New<Class>(true);
            if (!keyword.IsNullOrWhiteSpace())
            {
                predicate = predicate.Or(f => f.Name.Contains(keyword));
                var data = db.Classes.AsExpandable().Where(predicate).OrderByDescending(c => c.Name);
                ViewBag.Keyword = keyword;
                ViewBag.DataSearch = data;
            }
            
            ViewBag.limit = limit;
            var classes = db.Classes.OrderByDescending(s => s.CreatedAt).Where(s => s.CreatedAt >= startTime && s.CreatedAt <= endTime);
            ViewBag.TotalPage = Math.Ceiling((double)classes.Count() / limit.Value);
            ViewBag.CurrentPage = page;
            ViewBag.Limit = limit;
            ViewBag.Start = startTime.ToString("yyyy-MM-dd");
            ViewBag.End = endTime.ToString("yyyy-MM-dd");
            var list = classes.Skip((page.Value - 1) * limit.Value).Take(limit.Value).ToList();
            return View(list);
        }

        // GET: Classes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // GET: Classes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Classes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Class @class)
        {
            if (ModelState.IsValid)
            {
                @class.CreatedAt = DateTime.Now;
                @class.UpdatedAt = DateTime.Now;
                db.Classes.Add(@class);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@class);
        }

        // GET: Classes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Class @class)
        {
            if (ModelState.IsValid)
            {
                @class.UpdatedAt = DateTime.Now;
                db.Entry(@class).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (OptimisticConcurrencyException)
                {
                    Debug.WriteLine("error");
                }
                return RedirectToAction("Index");
            }
            return View(@class);
        }

        // GET: Classes/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Class @class = db.Classes.Find(id);
        //    if (@class == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(@class);
        //}

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Class @class = db.Classes.Find(id);
            db.Classes.Remove(@class);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //GET: Attendance
        public ActionResult Attendance(string className)
        {
            if (className == null)
            {
                className = db.Classes.ToList().ElementAtOrDefault(1)?.Name;
            }
            //List of Student
            var @class = db.Classes.FirstOrDefault(s=>s.Name.Contains(className));
            if (@class != null)
            {
                ViewBag.listOfClassName = new SelectList(db.Classes, "Id", "Name");
                ViewBag.advanceFullname = @class.Name;
                var listStudents = db.Students.Where(x => x.ClassId == @class.Id).OrderByDescending(s => s.CreatedAt).ToList();
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("_AjaxAttendance", listStudents) : View(listStudents);
            }
            return View();
        }
        public ActionResult AjaxSearchClass(string advanceName)
        {
            var predicate = PredicateBuilder.New<Class>(true);
            //predicate = predicate.And(f => f.ProductStatus != Product.ProductStatusEnum.Deleted);
           // Debug.WriteLine(advanceBrand);
            if (!string.IsNullOrEmpty(advanceName))
            {
                Debug.WriteLine("okay");
                predicate = predicate.And(f => f.Name.Contains(advanceName));
                ViewBag.advanceName = advanceName;
            }
            var data = db.Classes.AsExpandable().Where(predicate);
            var lsProducts = new List<Class>();
            foreach (var item in data)
            {
                lsProducts.Add(item);
            }
            return PartialView("_AjaxSearchClass", lsProducts);
        }
        
        public ActionResult Penalty(FormCollection data)
        {
            if (ModelState.IsValid)
            {
                
                    //Response.Write("Key=" + key + " ");
                    //Response.Write("Value=" + data[key]);
                    //Response.Write("<br/>");
                    //Debug.WriteLine(data["Id"]);
                    var student = db.Students.Find(Int32.Parse(data["Id"]));
                    if (student == null || student.StudentStatus == Student.StudentStatusEnum.Deleted)
                    {
                        return Redirect("Attendance");
                    }
                    var penaltyLevel = student.PenaltyLevel + 1;
                    db.Students.Add(student);
                    db.SaveChanges();
                    Models.Penalty penalty = new Penalty();
                    if (data["NewCashRank"] == null || Int32.Parse(data["penalty"]) == 0)
                    {
                        student.PenaltyLevel += 1;
                        penalty.StudentId = student.Id;
                        penalty.CreatedAt = DateTime.Now;
                        penalty.PenaltyType = (Penalty.PenaltyEnum) 0;
                        penalty.PenaltyCash = (int)10000 * penaltyLevel;
                    }
                    else
                    {
                        student.PenaltyLevel += 1;
                        penalty.StudentId = student.Id;
                        penalty.CreatedAt = DateTime.Now;
                        penalty.PenaltyType = (Penalty.PenaltyEnum) 1;
                        penalty.PenaltyCash = (double)10 * penaltyLevel;
                    }
                    Debug.WriteLine(penalty);
                    ViewBag.penalty = penalty;
                    db.Penalties.Add(penalty);
                    db.SaveChanges();
                
                return Redirect("Attendance");
            }
            return null;

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
