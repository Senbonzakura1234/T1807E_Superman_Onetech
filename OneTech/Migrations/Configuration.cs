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

            string[][] nameSt =
                {new string[] {"Nguyen Van A1", "Nguyen Van B1", "Nguyen Van C1", "Nguyen Van D1", "Nguyen Van E1", "Nguyen Van F1" ,"Nguyen Van G1", "Nguyen Van H1", "Nguyen Van I1", "Nguyen VAn J1", "Nguyen Van K1", "Nguyen Van L1", "Nguyen Van H1", "Nguyen Van M1", "Nguyen VAn N1"},
                    new string[] {"Nguyen Van A2", "Nguyen Van B2", "Nguyen Van C2", "Nguyen Van D2", "Nguyen Van E2", "Nguyen Van F2", "Nguyen Van G2", "Nguyen Van H2", "Nguyen Van I2", "Nguyen VAn J2", "Nguyen Van K2", "Nguyen Van L2", "Nguyen Van H2", "Nguyen Van M2", "Nguyen VAn N2"},
                    new string[] {"Nguyen Van A3", "Nguyen Van B3", "Nguyen Van C3", "Nguyen Van D3", "Nguyen Van E3", "Nguyen Van F3", "Nguyen Van G3", "Nguyen Van H3", "Nguyen Van I3", "Nguyen VAn J3", "Nguyen Van K3", "Nguyen Van L3", "Nguyen Van H3", "Nguyen Van M3", "Nguyen VAn N3"},
                    new string[] {"Nguyen Van A4", "Nguyen Van B4", "Nguyen Van C4", "Nguyen Van D4", "Nguyen Van E4", "Nguyen Van F4", "Nguyen Van G4", "Nguyen Van H4", "Nguyen Van I4", "Nguyen VAn J4", "Nguyen Van K4", "Nguyen Van L4", "Nguyen Van H4", "Nguyen Van M4", "Nguyen VAn N4"},
                    new string[] {"Nguyen Van A5", "Nguyen Van B5", "Nguyen Van C5", "Nguyen Van D5", "Nguyen Van E5", "Nguyen Van F5", "Nguyen Van G5", "Nguyen Van H5", "Nguyen Van I5", "Nguyen VAn J5", "Nguyen Van K5", "Nguyen Van L5", "Nguyen Van H5", "Nguyen Van M5", "Nguyen VAn N5"},
                };
            var listStudent = new List<Student>();
            for (int i = 1; i <= 5; i++)
            {
                for (int j = 1; j <= 15; j++)
                {

                    listStudent.Add(new Student()
                    {
                        Id = i * 15 + j,
                        FullName = nameSt[i-1][j-1],
                        StudentCode = "SC" + i*15 +j,
                        ClassId = i,
                        Birthday = new DateTime((int)new Random().Next(1950, 2000), (int)new Random().Next(1, 12), (int)new Random().Next(1, 31)),
                        CreatedAt = new DateTime((int)new Random().Next(2000, 2010), (int)new Random().Next(1, 12), (int)new Random().Next(1, 31)),
                        UpdatedAt = new DateTime((int)new Random().Next(2000, 2010), (int)new Random().Next(1, 12), (int)new Random().Next(1, 31)),
                        DeletedAt = null,
                        PenaltyLevel = 0
                    });
                }
            }
            context.Students.AddRange(listStudent);

            context.SaveChanges();
        }
    }
}
