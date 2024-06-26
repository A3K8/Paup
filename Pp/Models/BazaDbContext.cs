using MySql.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace Pp.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class BazaDbContext : DbContext
    {
        public DbSet<Korisnik> PopisKorisnika { get; set; }
        public DbSet<Ovlast> PopisOvlasti { get; set; }
        public DbSet<Proizvodi> PopisProizvoda { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Window> Windows { get; set; }
        public DbSet<Door> Doors { get; set; }
    }
}