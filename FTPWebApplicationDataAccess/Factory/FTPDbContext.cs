using FTPWebApplicationDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPWebApplicationDataAccess
{
    public class FTPDbContext : DbContext
    {
        public FTPDbContext() : base("FTPDbConnection")
        {
        }

        public FTPDbContext(DbConnection connection) : base(connection, true) { }

        public virtual DbSet<FTPFilesPath> FTPFilesPath { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
