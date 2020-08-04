using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Technohouse.Web.Data.Entities;

namespace Technohouse.Web.Model
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<ActingZone> ActingZones { get; set; }
    }
}
