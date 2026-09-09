namespace TodoApp.API.DTOs
{
    /// <summary>
    /// DTO para actualizar una tarea
    /// </summary>
    public class ActualizarTareaDto
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaLimite { get; set; }
        public int IdEstado { get; set; }
        public int IdImportancia { get; set; }
    }
}
