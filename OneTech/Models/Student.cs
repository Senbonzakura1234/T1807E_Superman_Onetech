// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OneTech.Models
{
    public class Student
    {
        [Key]
        [DisplayName("Student Code")]
        public int Id { get; set; }
        [Required(ErrorMessage = "FullName is required")]
        public string FullName { get; set; }

        [ForeignKey("Class")]
        [DisplayName("Class Name")]
        public int ClassId { get; set; } 
        public virtual Class Class { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The OwedCash cannot be less than 0")]
        public double OwedCash { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "The OwedPushUp cannot be less than 0")]
        public int OwedPushUp { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "The PenaltyLevel cannot be less than 0")]
        public int PenaltyLevel { get; set; }

        public virtual ICollection<Penalty> Penalties { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Birthday { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime UpdatedAt { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DeletedAt { get; set; }

        public enum StudentStatusEnum
        {
            Inactive = 2,
            Active = 1,
            Deleted = 0
        }
        public StudentStatusEnum StudentStatus { get; set; }

        public Student()
        {
            StudentStatus = StudentStatusEnum.Active;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}