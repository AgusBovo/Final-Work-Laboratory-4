using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectofinalLabo4.Models
{
    public class AppDBcontext : DbContext
    {
       public AppDBcontext(DbContextOptions<AppDBcontext> options) : base(options)
        {

        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Club> Clubes { get; set; }
        public DbSet<Jugador> Jugadores { get; set; }
        public DbSet<JugadorClub> JugadoresClubs { get; set; }
    }
}
