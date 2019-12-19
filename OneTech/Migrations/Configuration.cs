// ReSharper disable RedundantUsingDirective

using System.Collections.Generic;
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
            context.Database.ExecuteSqlCommand("DBCC checkident ('Classes', reseed, 0)");
            context.Database.ExecuteSqlCommand("DBCC checkident ('Students', reseed, 0)");
            context.Database.ExecuteSqlCommand("delete from Classes where 1 = 1");
            context.Database.ExecuteSqlCommand("delete from Students where 1 = 1");
            var listClasses = new List<Class>();
            var listStudents = new List<Student>();
            String[] ClassName = new String [3] {"T1807E", "T1808E", "T1809E"};
            String[] StudentName = new String[45] { "Nguyễn Thị Dung", "Đặng Văn Vị", "Nguyễn Thị Tuân", "Đặng Văn Tuyên", "Nguyễn Văn Khanh", "Cao Thị Thanh", "Đặng Thị Hằng", "Nguyễn Thị Tuyền", "Đặng Văn Kiên", "Đoàn Thị Dương", "Nguyễn Hồng Toan", "Nguyễn Thị Bẩy", "Đặng Thị Hồng", "Nguyễn Tuấn Đạt", "Nguyễn Thị Thoa", "Nguyễn Thị Hương", "Đặng Đức Hùng", "Nguyễn Thanh Hào", "Nguyễn Văn Thông", "Nguyễn Thị Lộc", "Nguyễn Văn Định", "Nguyễn Văn Kiên", "Nguyễn Thị Tươi", "Vũ Văn Thắng", "Đặng Quý Công Đoàn", "Nguyễn Đức Minh", "Nguyễn Thị Hạnh", "Nguyễn Kim Chiến", "Nguyễn Công Hiếu", "Nguyễn Mạnh Cường", "Vũ Tiến Lộc", "Nguyễn Văn Chung", "Ngô Hồng Hạnh", "Đặng Thị Lê Phương", "Nguyễn Văn Quyết", "Trần Thị Vân", "Nguyễn Đức Thuấn", "Lưu Đình Trường", "Phạm Tuấn Anh", "Nguyễn Hoàng Long", "Tạ Văn Ninh", "Lưu Thị Dung", "Vũ Duy Sáng", "Nguyễn Thị Châm", "Nguyễn Đức Hữu" };
            String[] StudentBrithday = new String[45]
            {
                "1984-03-19", "1986-01-12", "1995-08-20", "1997-05-04", "1981-08-05", "1999-01-25", "1985-12-01",
                "1999-09-17", "1990-09-18", "1988-10-02", "1994-06-12", "1993-12-25", "1989-12-21", "2002-04-05",
                "1998-12-30", "1980-04-30", "2001-08-20", "1989-10-10", "1990-08-01", "1989-10-28", "1991-12-01",
                "1996-09-29", "1993-06-05", "1993-12-11", "1993-06-24", "2001-09-29", "1986-01-09", "1999-04-12",
                "2002-11-05", "1987-06-12", "1987-10-03", "2002-03-19", "1981-09-13", "1997-12-14", "1981-02-01",
                "2002-08-23", "2001-10-20", "1987-08-21", "1990-04-29", "1983-10-06", "2000-01-08", "2002-02-19",
                "1983-12-19", "1992-11-03", "1980-09-10"
            };
            for (int i = 0, k= 0; i <= 2; i++ , k+=15)
            {
                listClasses.Add(new Class()
                {
                    Id = i,
                    Name = ClassName[i],
                });
                for (int j = 0; j <= 14; j++)
                {
                    listStudents.Add(new Student()
                    {
                        Id = j,
                        StudentCode = "SC" + j+k + "CC",
                        FullName = StudentName[j+k],
                        Birthday = DateTime.Parse(StudentBrithday[j+k]),
                        OwedCash = 0,
                        OwedPushUp = 0,
                        PenaltyLevel = 0,
                        ClassId = i
                    });
                }
            }
            
            context.Classes.AddRange(listClasses);
            context.Students.AddRange(listStudents);
            context.SaveChanges();
        }
    }
}
