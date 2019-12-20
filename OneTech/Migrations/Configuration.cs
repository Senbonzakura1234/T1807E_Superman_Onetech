// ReSharper disable RedundantUsingDirective

using System.Collections.Generic;
using System.Text;
using OneTech.Models;

namespace OneTech.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Models.MyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Models.MyContext context)
        {
            //context.Database.ExecuteSqlCommand("TRUNCATE TABLE Classes");
            var listClass = new List<Class>();
            for (var i = 1; i <= 5; i++)
            {
                listClass.Add(new Class()
                {
                    Id = i,
                    Name = "CC" + i,
                    CreatedAt = Convert.ToDateTime("01/02/2005"),
                    DeletedAt = null,
                    UpdatedAt = Convert.ToDateTime("01/02/2005"),
                });
            }
            context.Classes.AddRange(listClass);

            string[] nameSt =
                {"Bùi Thành Nguyên", "Võ Ðức Tài", "Bùi Văn Minh", "Ngô Gia Phúc", "Hồ Quốc Anh", "Đặng Nguyên Giáp", "Phạm Quốc Toản", "Phạm Quốc Toản", "Thạch An Khang", "Nguyễn Khôi Vĩ", "Trần Minh Huy", "Đinh Phương Quyên", "Nguyễn Hồng Ngọc", "Nguyễn Ngọc Anh", "Tôn Bích Hậu",
                    "Nguyễn Quỳnh Nhung", "Nguyễn Phương Quế", "Phạm Bích Hải", "Dương Trâm Oanh", "Chử Xuân Lâm", "Lê Ngọc Uyển", "Doãn Ngọc Quang", "Diệp Hữu Tâm", "Nghiêm Ðức Tuệ", "Quách Gia Bình", "Lý Hữu Minh", "Mã Thế Vinh", "Đặng Hữu Thắng", "Võ Thiên Phú", "Lê Huy Phong",
                    "Dương Hữu Khanh", "Phan Hồng Trúc", "Hoàng Hiền Nhi", "Lê Mai Trinh", "Thái Thủy Mai", "Nguyễn Minh Yến", "Mai Thiên Di", "Doãn Thanh Vân", "Nguyễn Tuyết Băng", "Vũ Thanh Nhung", "Huỳnh Ngân Thanh", "Bùi Thiện Phước", "Hoàng Hữu Châu", "Lâm Thái Sang", "Ân Hoàng Quân",
                    "Bành Thiên Phú", "Đặng Vương Triệu", "Đỗ Giang Nam", "Lạc Tường Vinh", "Bùi Nhật Dũng", "Phan Anh Hoàng", "Quang Linh Duyên", "Võ Tố Loan", "Chung Thiện Tiên", "Quang Tuyết Hương", "Hồ Gia Bảo", "Lương Thúy My", "Nguyễn Tâm Ðoan", "Nguyễn Xuân Linh", "Thảo Lâm Tuyền",
                    "Bạch Khánh Quỳnh", "Dương Sơn Hà", "Quyền Bảo Long", "Nguyễn Minh Nhật", "Châu Hoàng Nam", "Đỗ Khải Tuấn", "Bùi Gia Bảo", "Dương Mạnh Tuấn", "Nguyễn Quốc Huy", "Hoàng Ðắc Di", "Vũ Đình Chiến", "Nguyễn Duy Uyên", "Nguyễn Tuệ Mẫn", "Đinh Ngọc Ái", "Mã Hiền Nhi",
                };
            var listStudent = new List<Student>();
            for (int i = 1; i <= 5; i++)
            {
                for (int j = 1; j <= 15; j++)
                {
                    Random nameRandom = new Random();
                    listStudent.Add(new Student()
                    {
                        Id = i * 15 + j,
                        FullName = nameSt[nameRandom.Next(0, nameSt.Length)],
                        StudentCode = "SC" + i*15 +j,
                        ClassId = i,
                        Birthday = new DateTime((int)new Random().Next(1950, 2000), (int)new Random().Next(1, 12), (int)new Random().Next(1, 31)),
                        CreatedAt = new DateTime((int)new Random().Next(2000, 2010), (int)new Random().Next(1, 12), (int)new Random().Next(1, 31)),
                        UpdatedAt = new DateTime((int)new Random().Next(2000, 2010), (int)new Random().Next(1, 12), (int)new Random().Next(1, 31)),
                        DeletedAt = null,
                        PenaltyLevel = 0,
                        StudentStatus = Student.StudentStatusEnum.Active
                    });
                }
            }
            context.Students.AddRange(listStudent);

            context.SaveChanges();
        }
    }
}
