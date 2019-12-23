using OneTech.Models;

namespace OneTech.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<MyContext>
    {
        private readonly Random _gen = new Random();

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
        public DateTime RandomDay()
        {
            var start = new DateTime(1980, 1, 1);
            var end = new DateTime(2000, 12, 31);
            var range = (end - start).Days;
            return start.AddDays(_gen.Next(range));
        }
        protected override void Seed(MyContext context)
        {

            //context.Database.ExecuteSqlCommand("TRUNCATE TABLE Classes");
            for (var i = 1; i <= 5; i++)
            {
                context.Classes.AddOrUpdate(new Class()
                {
                    Id = i,
                    Name = "CC" + i
                });
            }
            // ReSharper disable StringLiteralTypo
            var nameSt = new[]{"Bùi Thành Nguyên", "Võ Ðức Tài", "Bùi Văn Minh", "Ngô Gia Phúc", "Hồ Quốc Anh", "Đặng Nguyên Giáp", "Phạm Quốc Toản", "Phạm Quốc Toản", "Thạch An Khang", "Nguyễn Khôi Vĩ", "Trần Minh Huy", "Đinh Phương Quyên", "Nguyễn Hồng Ngọc", "Nguyễn Ngọc Anh", "Tôn Bích Hậu",
                "Nguyễn Quỳnh Nhung", "Nguyễn Phương Quế", "Phạm Bích Hải", "Dương Trâm Oanh", "Chử Xuân Lâm", "Lê Ngọc Uyển", "Doãn Ngọc Quang", "Diệp Hữu Tâm", "Nghiêm Ðức Tuệ", "Quách Gia Bình", "Lý Hữu Minh", "Mã Thế Vinh", "Đặng Hữu Thắng", "Võ Thiên Phú", "Lê Huy Phong",
                "Dương Hữu Khanh", "Phan Hồng Trúc", "Hoàng Hiền Nhi", "Lê Mai Trinh", "Thái Thủy Mai", "Nguyễn Minh Yến", "Mai Thiên Di", "Doãn Thanh Vân", "Nguyễn Tuyết Băng", "Vũ Thanh Nhung", "Huỳnh Ngân Thanh", "Bùi Thiện Phước", "Hoàng Hữu Châu", "Lâm Thái Sang", "Ân Hoàng Quân",
                "Bành Thiên Phú", "Đặng Vương Triệu", "Đỗ Giang Nam", "Lạc Tường Vinh", "Bùi Nhật Dũng", "Phan Anh Hoàng", "Quang Linh Duyên", "Võ Tố Loan", "Chung Thiện Tiên", "Quang Tuyết Hương", "Hồ Gia Bảo", "Lương Thúy My", "Nguyễn Tâm Ðoan", "Nguyễn Xuân Linh", "Thảo Lâm Tuyền",
                "Bạch Khánh Quỳnh", "Dương Sơn Hà", "Quyền Bảo Long", "Nguyễn Minh Nhật", "Châu Hoàng Nam", "Đỗ Khải Tuấn", "Bùi Gia Bảo", "Dương Mạnh Tuấn", "Nguyễn Quốc Huy", "Hoàng Ðắc Di", "Vũ Đình Chiến", "Nguyễn Duy Uyên", "Nguyễn Tuệ Mẫn", "Đinh Ngọc Ái", "Mã Hiền Nhi",
            };
            // ReSharper restore StringLiteralTypo
            var nameRandom = new Random();

            for (var i = 1; i <= 5; i++)
            {
                for (var j = 1; j <= 15; j++)
                {
                    context.Students.AddOrUpdate(new Student()
                    {
                        Id = i * 15 + j,
                        FullName = nameSt[nameRandom.Next(0, nameSt.Length)],
                        ClassId = i,
                        Birthday = RandomDay()
                    });
                }
            }
        }
    }
}
