using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneTech.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public double OwedCash { get; set; }
        public int OwedPushUp { get; set; }
        public int PenaltyLevel { get; set; }
        public int Penalty { get; set; }
        public PenaltyEnum PenaltyType { get; set; }
        public enum PenaltyEnum
        {
            Cash = 0,
            Pushup = 1
        }
    }
}