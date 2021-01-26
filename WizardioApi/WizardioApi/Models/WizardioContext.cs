using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WizardioApi.Models;

namespace WizardioApi.Models
{
    public class WizardioContext : DbContext
    {
        public WizardioContext(DbContextOptions<WizardioContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<GameSessions> GameSessions { get; set; }
    }
}
