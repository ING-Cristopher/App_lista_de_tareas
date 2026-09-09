using TodoApp.Domain.Models;
using TodoApp.Infrastructure.Repositories;

namespace TodoApp.Infrastructure.Services
{
    /// <summary>
    /// Servicio de lógica de negocio para Tareas
    /// Aquí va toda la validación y reglas de negocio
    /// Usa el repositorio para acceder a datos
    /// </summary>
    public class TareaService : ITareaService
    {
        private readonly ITareaRepository _tareaRepository;
        private readonly IRepository<Estado> _estadoRepository;
        private readonly IRepository<Importancia> _importanciaRepository;

        public TareaService(
            ITareaRepository tareaRepository,
            IRepository<Estado> estadoRepository,
            IRepository<Importancia> importanciaRepository)
        {
            _tareaRepository = tareaRepository;
            _estadoRepository = estadoRepository;
            _importanciaRepository = importanciaRepository;
        }

        // ==================== LECTURA ====================

        public async Task<IEnumerable<Tarea>> ObtenerTodasAsync()
        {
            return await _tareaRepository.GetAllAsync();
        }

        public async Task<Tarea> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
                return null;

            return await _tareaRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Tarea>> ObtenerPorEstadoAsync(int idEstado)
        {
            if (idEstado <= 0)
                return new List<Tarea>();

            return await _tareaRepository.GetByEstadoAsync(idEstado);
        }

        public async Task<IEnumerable<Tarea>> ObtenerPorImportanciaAsync(int idImportancia)
        {
            if (idImportancia <= 0)
                return new List<Tarea>();

            return await _tareaRepository.GetByImportanciaAsync(idImportancia);
        }

        public async Task<IEnumerable<Tarea>> ObtenerTareasVencidasAsync()
        {
            return await _tareaRepository.GetTareasVencidasAsync();
        }

        // ==================== ESCRITURA ====================

        public async Task<(bool exitoso, string mensaje, Tarea tarea)> CrearTareaAsync(
            string titulo,
            string descripcion,
            DateTime fechaLimite,
            int idEstado,
            int idImportancia)
        {
            // VALIDACIÓN 1: Verificar que los datos básicos sean válidos
            if (string.IsNullOrWhiteSpace(titulo))
                return (false, "El título es requerido", null);

            if (titulo.Length < 5)
                return (false, "El título debe tener mínimo 5 caracteres", null);

            if (titulo.Length > 255)
                return (false, "El título no puede exceder 255 caracteres", null);

            // VALIDACIÓN 2: Verificar fecha
            if (fechaLimite <= DateTime.Now)
                return (false, "La fecha límite debe ser en el futuro", null);

            // VALIDACIÓN 3: Verificar que Estado existe
            var estado = await _estadoRepository.GetByIdAsync(idEstado);
            if (estado == null)
                return (false, $"El estado con ID {idEstado} no existe", null);

            // VALIDACIÓN 4: Verificar que Importancia existe
            var importancia = await _importanciaRepository.GetByIdAsync(idImportancia);
            if (importancia == null)
                return (false, $"La importancia con ID {idImportancia} no existe", null);

            try
            {
                // CREAR la tarea
                var tarea = new Tarea(titulo, descripcion, fechaLimite, idEstado, idImportancia);

                // VALIDACIÓN 5: Validación de negocio (método del modelo)
                if (!tarea.Esvalida())
                {
                    string error = tarea.ObtenerErrorValidacion();
                    return (false, error, null);
                }

                // GUARDAR
                await _tareaRepository.AddAsync(tarea);

                return (true, "Tarea creada exitosamente", tarea);
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear la tarea: {ex.Message}", null);
            }
        }

        public async Task<(bool exitoso, string mensaje)> ActualizarTareaAsync(
            int id,
            string titulo,
            string descripcion,
            DateTime fechaLimite,
            int idEstado,
            int idImportancia)
        {
            // VALIDACIÓN 1: Verificar que la tarea existe
            var tarea = await _tareaRepository.GetByIdAsync(id);
            if (tarea == null)
                return (false, $"La tarea con ID {id} no existe");

            // VALIDACIÓN 2: Validar datos
            if (string.IsNullOrWhiteSpace(titulo) || titulo.Length < 5)
                return (false, "El título debe tener mínimo 5 caracteres");

            if (fechaLimite <= DateTime.Now)
                return (false, "La fecha límite debe ser en el futuro");

            // VALIDACIÓN 3: Verificar Estado e Importancia
            var estado = await _estadoRepository.GetByIdAsync(idEstado);
            if (estado == null)
                return (false, $"El estado con ID {idEstado} no existe");

            var importancia = await _importanciaRepository.GetByIdAsync(idImportancia);
            if (importancia == null)
                return (false, $"La importancia con ID {idImportancia} no existe");

            try
            {
                // ACTUALIZAR
                tarea.Titulo = titulo;
                tarea.Descripcion = descripcion;
                tarea.FechaLimite = fechaLimite;
                tarea.IdEstado = idEstado;
                tarea.IdImportancia = idImportancia;
                tarea.FechaActualizacion = DateTime.Now;

                await _tareaRepository.UpdateAsync(tarea);

                return (true, "Tarea actualizada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar la tarea: {ex.Message}");
            }
        }

        public async Task<(bool exitoso, string mensaje)> EliminarTareaAsync(int id)
        {
            // VALIDACIÓN: Verificar que existe
            var tarea = await _tareaRepository.GetByIdAsync(id);
            if (tarea == null)
                return (false, $"La tarea con ID {id} no existe");

            try
            {
                await _tareaRepository.DeleteAsync(id);
                return (true, "Tarea eliminada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar la tarea: {ex.Message}");
            }
        }

        public async Task<(bool exitoso, string mensaje)> CambiarEstadoAsync(int id, int nuevoIdEstado)
        {
            // VALIDACIÓN 1: Verificar tarea
            var tarea = await _tareaRepository.GetByIdAsync(id);
            if (tarea == null)
                return (false, $"La tarea con ID {id} no existe");

            // VALIDACIÓN 2: Verificar estado
            var estado = await _estadoRepository.GetByIdAsync(nuevoIdEstado);
            if (estado == null)
                return (false, $"El estado con ID {nuevoIdEstado} no existe");

            // VALIDACIÓN 3: No cambiar a un estado que ya tiene
            if (tarea.IdEstado == nuevoIdEstado)
                return (false, "La tarea ya está en ese estado");

            try
            {
                tarea.IdEstado = nuevoIdEstado;
                tarea.FechaActualizacion = DateTime.Now;
                await _tareaRepository.UpdateAsync(tarea);

                return (true, "Estado actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al cambiar estado: {ex.Message}");
            }
        }
    }
}
