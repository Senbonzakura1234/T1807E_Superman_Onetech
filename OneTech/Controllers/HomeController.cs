
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneTech.Models;

namespace OneTech.Controllers
{
    public class HomeController : Controller
    {
        MyContext Db = new MyContext();
        public ActionResult DashBoard()
        {
            ViewBag.listClass = Db.Classes.ToList();
            return View();
        }
    }
}