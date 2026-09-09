namespace TodoApp.API.DTOs
{
    /// <summary>
    /// DTO para crear una tarea
    /// Es lo que React envía cuando crea una tarea
    /// </summary>
    public class CrearTareaDto
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaLimite { get; set; }
        public int IdEstado { get; set; }
        public int IdImportancia { get; set; }
    }
}
