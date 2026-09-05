using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApp.Domain.Models
{
    /// <summary>
    /// Representa los posibles estados de una tarea
    /// </summary>
    public class Estado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        //Contructor vacio requerido po EF Core
        public Estado(){ }

        public Estado (int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }
    }
}
