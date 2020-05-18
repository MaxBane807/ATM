using ATM.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.Web.Data
{
    public class AtmContext : DbContext
    {
        public AtmContext(DbContextOptions<AtmContext>options)
            :base(options)
        {

        }
        public DbSet<Bill> Bills { get; set; }
    }
}
