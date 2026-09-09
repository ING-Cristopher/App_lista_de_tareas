using TodoApp.Domain.Models;

namespace TodoApp.Infrastructure.Services
{
    /// <summary>
    /// Contrato para el servicio de Tareas
    /// Define qué operaciones se pueden hacer con tareas
    /// </summary>
    public interface ITareaService
    {
        // LECTURA
        Task<IEnumerable<Tarea>> ObtenerTodasAsync();
        Task<Tarea> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Tarea>> ObtenerPorEstadoAsync(int idEstado);
        Task<IEnumerable<Tarea>> ObtenerPorImportanciaAsync(int idImportancia);
        Task<IEnumerable<Tarea>> ObtenerTareasVencidasAsync();

        // ESCRITURA
        Task<(bool exitoso, string mensaje, Tarea tarea)> CrearTareaAsync(string titulo, string descripcion, DateTime fechaLimite, int idEstado, int idImportancia);
        Task<(bool exitoso, string mensaje)> ActualizarTareaAsync(int id, string titulo, string descripcion, DateTime fechaLimite, int idEstado, int idImportancia);
        Task<(bool exitoso, string mensaje)> EliminarTareaAsync(int id);
        Task<(bool exitoso, string mensaje)> CambiarEstadoAsync(int id, int nuevoIdEstado);
    }
}
