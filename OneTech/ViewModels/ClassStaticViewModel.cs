using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneTech.Models;

namespace OneTech.ViewModels
{
    public class ClassStaticViewModel
    {
        private MyContext _db = new MyContext();
        public List<ClassStaticViewModel> classStaticViewModels = new List<ClassStaticViewModel>();
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public DateTime CreatedAt { get; set; }
        public double PenaltyCash { get; set; }
        public int PenaltyPushUp { get; set; }

        public ClassStaticViewModel()
        {
          
        }
    }
}