using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WaterlooBites.Models;

namespace WaterlooBites.Data
{
    public class WaterlooBitesContext : DbContext
    {
        public WaterlooBitesContext (DbContextOptions<WaterlooBitesContext> options)
            : base(options)
        {
        }

        public DbSet<WaterlooBites.Models.Rating> Rating { get; set; } = default!;
    }
}
