using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
        public ActionResult Create([Bind(Include = "Id,Name,CreatedAt,UpdatedAt,DeletedAt")] Class @class)
        {
            if (ModelState.IsValid)
            {
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
        public ActionResult Edit([Bind(Include = "Id,Name,UpdatedAt")] Class @class)
        {
            if (ModelState.IsValid)
            {
                @class.UpdatedAt = DateTime.Now;
                db.Entry(@class).State = EntityState.Modified;
                db.SaveChanges();
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
        public ActionResult Attendance(string className, int? page, int? limit)
        {
            if (className == null)
            {
                className = "T1807E";
            }
            if (page == null)
            {
                page = 1;
            }

            if (limit == null)
            {
                limit = 10;
            }
            //Send to sidebar list class
            List<Class> listClass = db.Classes.ToList();
            SelectList classList = new SelectList(listClass, "Name","Name","Name");
            ViewBag.ClassList = classList;
            ViewBag.Name = className;
            //pagination
            var @class = db.Classes.FirstOrDefault(x => x.Name.Contains(className));
            var listStudents = db.Students.Where(x => x.ClassId == @class.Id).OrderByDescending(s => s.CreatedAt);
            ViewBag.limit = limit;
            ViewBag.TotalPage = Math.Ceiling((double)listStudents.Count() / limit.Value);
            ViewBag.CurrentPage = page;
            ViewBag.Limit = limit;
            var listStudentsPagination = listStudents.Skip((page.Value - 1) * limit.Value).Take(limit.Value).ToList();
            return View(listStudentsPagination);
        }

        public ActionResult Penalty(string studentCode, Penalty.PenaltyEnum penaltyEnum, int? penaltyRank )
        {
            var student = db.Students.FirstOrDefault(s=>s.StudentCode.Contains(studentCode));
            var data = db.Students.Where(s => s.StudentStatus != Student.StudentStatusEnum.Deleted)
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
            if (student == null || student.StudentStatus == Student.StudentStatusEnum.Deleted)
            {
                return Redirect("Attendance");
            }
            var penaltyLevel = student.PenaltyLevel + 1;
            db.Students.Add(student);
            db.SaveChanges();
            Models.Penalty penalty = new Penalty();
            if (penaltyRank == null && penaltyEnum == (Penalty.PenaltyEnum) 1)
            {
                penaltyRank = 10;
                student.PenaltyLevel += 1;
                penalty.StudentId = student.Id;
                penalty.CreatedAt = DateTime.Now;
                penalty.PenaltyType = penaltyEnum;
                penalty.PenaltyPushUp = (int) penaltyRank* penaltyLevel;
            }
            else
            {
                penaltyRank = 10000;
                student.PenaltyLevel += 1;
                penalty.StudentId = student.Id;
                penalty.CreatedAt = DateTime.Now;
                penalty.PenaltyType = penaltyEnum;
                penalty.PenaltyCash = (double) penaltyRank* penaltyLevel;
            }
            Debug.WriteLine(penalty);
            ViewBag.penalty = penalty;
            db.Penalties.Add(penalty);
            db.SaveChanges();

            return Redirect("Attendance");
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
