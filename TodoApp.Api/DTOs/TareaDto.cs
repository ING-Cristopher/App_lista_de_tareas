namespace TodoApp.API.DTOs
{
    /// <summary>
    /// DTO para Tarea
    /// Esto es lo que se envía al cliente (React)
    /// </summary>
    public class TareaDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaLimite { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public EstadoDto Estado { get; set; }
        public ImportanciaDto Importancia { get; set; }
    }
}
