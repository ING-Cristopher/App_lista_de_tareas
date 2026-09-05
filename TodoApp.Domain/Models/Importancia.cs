using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApp.Domain.Models
{
    /// <summary>
    /// Representa la importancia de una tarea
    /// </summary>
    public class Importancia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // Constructor vacío requerido por EF Core
        public Importancia() { }

        public Importancia(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }
    }
}
