namespace TodoApp.API.DTOs
{
    /// <summary>
    /// DTO para Estado
    /// Solo exponemos lo que el cliente necesita ver
    /// </summary>
    public class EstadoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}
