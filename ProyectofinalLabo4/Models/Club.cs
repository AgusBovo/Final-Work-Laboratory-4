using System;

namespace ProyectofinalLabo4.Models
{
    public class Club
    {
        public int Id { get; set; }
        public string NombreClub { get; set; }
        public string Resumen { get; set; }
        public DateTime Año { get; set; }
        public string Pais { get; set; }
        public string Categoria { get; set; }

        public string foto { get; set; }
    }
}
