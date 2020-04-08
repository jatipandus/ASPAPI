using BelajarAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BelajarAPI.Context
{
    public class MyContext : DbContext
    {
        public MyContext() : base("BelajarAPI") { }
        public DbSet<DepartmentModels> Departments { get; set; }
    }
}