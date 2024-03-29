﻿using System.Data.Entity;

namespace OneTech.Models
{
    public class MyContext : DbContext
    {
        public MyContext() : base("name=MyContext")
        {

        }
        // ReSharper disable once InconsistentNaming
        public DbSet<Class> Classes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Penalty> Penalties { get; set; }
    }
}