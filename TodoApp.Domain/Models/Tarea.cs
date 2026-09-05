using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApp.Domain.Models
{
    /// <summary>
    /// Representa una tarea en la aplicación de tareas
    /// Este es el modelo CORE del negocio - debe ser independiente de la BD
    /// </summary>
    public class Tarea
    {
        //propiedades
        public int Id { get; set; }

        public string Titulo { get; set; }
        public string Descripcion { get; set; }

        public DateTime FechaLimite { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaActualizacion { get; set; }

        //Relaciones (Foreign Keys)
        public int IdEstado { get; set; }
        public Estado Estado { get; set; }

        public int IdImportancia { get; set; }
        public Importancia Importancia { get; set; }

        // Para futuras subtareas (Fase 2)
        public List<Tarea> subtarea { get; set; } = new List<Tarea>();
        public int? IdTareaPadre { get; set; } // Si es subtarea

        //Constructor vacio
        public Tarea() { }

        //Constructor con parametros basicos
        public Tarea(string titulo, string descripcion, DateTime fechaLimite, int idEstado, int idImportancia)
        {
            Titulo = titulo;
            Descripcion = descripcion;
            FechaLimite = fechaLimite;
            FechaCreacion = DateTime.Now;
            IdEstado = idEstado;
            IdImportancia = idImportancia;
        }

        //METODO DE VALIDACION (Logica de negocio)
        public bool Esvalida()
        {
            // Validaciones basicas del dominio
            if (string.IsNullOrWhiteSpace(Titulo) || Titulo.Length < 5)
                return false;

            if (FechaLimite <= DateTime.Now)
                return false;

            return true;    
        }

        public string ObtenerErrorValidacion()
        {
            if (string.IsNullOrWhiteSpace(Titulo) || Titulo.Length < 5)
                return "El titulo es obligatorio y debe tener al menos 5 caracteres";
            if (FechaLimite <= DateTime.Now)
                return "La fecha limite debe ser mayor a la fecha actual";

            return "";

        }
    }
}
