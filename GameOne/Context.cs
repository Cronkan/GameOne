using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOne;

namespace GameOneDataLayer
{
    public class Context : DbContext
    {
        public Context()
            : base()
        {

        }
        public DbSet<Game> Game { get; set; }
        public DbSet<Pawn> Pawns { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<Context>(null);
           // modelBuilder.Entity<Game>().Property(m => m.pawnToSplit).IsOptional();  
            base.OnModelCreating(modelBuilder);
        } 
    }
}
