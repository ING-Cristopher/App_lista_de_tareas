using Microsoft.AspNetCore.Mvc;
using TodoApp.API.DTOs;
using TodoApp.Domain.Models;
using TodoApp.Infrastructure.Services;

namespace TodoApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TareasController : ControllerBase
    {
        private readonly ITareaService _tareaService;

        public TareasController(ITareaService tareaService)
        {
            _tareaService = tareaService;
        }

        // ==================== GET ====================

        /// <summary>
        /// Obtiene todas las tareas
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TareaDto>>> ObtenerTodas()
        {
            try
            {
                var tareas = await _tareaService.ObtenerTodasAsync();
                var tareasDto = tareas.Select(t => MapearTareaDto(t));
                return Ok(tareasDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener tareas", detalles = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una tarea por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TareaDto>> ObtenerPorId(int id)
        {
            try
            {
                var tarea = await _tareaService.ObtenerPorIdAsync(id);
                if (tarea == null)
                    return NotFound(new { error = "Tarea no encontrada", id });

                return Ok(MapearTareaDto(tarea));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener tarea", detalles = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene tareas filtradas por estado
        /// </summary>
        [HttpGet("por-estado/{idEstado}")]
        public async Task<ActionResult<IEnumerable<TareaDto>>> ObtenerPorEstado(int idEstado)
        {
            try
            {
                var tareas = await _tareaService.ObtenerPorEstadoAsync(idEstado);
                var tareasDto = tareas.Select(t => MapearTareaDto(t));
                return Ok(tareasDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener tareas", detalles = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene tareas filtradas por importancia
        /// </summary>
        [HttpGet("por-importancia/{idImportancia}")]
        public async Task<ActionResult<IEnumerable<TareaDto>>> ObtenerPorImportancia(int idImportancia)
        {
            try
            {
                var tareas = await _tareaService.ObtenerPorImportanciaAsync(idImportancia);
                var tareasDto = tareas.Select(t => MapearTareaDto(t));
                return Ok(tareasDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener tareas", detalles = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene tareas vencidas (fecha límite pasada y no completadas)
        /// </summary>
        [HttpGet("vencidas")]
        public async Task<ActionResult<IEnumerable<TareaDto>>> ObtenerVencidas()
        {
            try
            {
                var tareas = await _tareaService.ObtenerTareasVencidasAsync();
                var tareasDto = tareas.Select(t => MapearTareaDto(t));
                return Ok(tareasDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener tareas vencidas", detalles = ex.Message });
            }
        }

        // ==================== POST ====================

        /// <summary>
        /// Crea una nueva tarea
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TareaDto>> Crear([FromBody] CrearTareaDto dto)
        {
            try
            {
                // Validación básica
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var resultado = await _tareaService.CrearTareaAsync(
                    dto.Titulo,
                    dto.Descripcion,
                    dto.FechaLimite,
                    dto.IdEstado,
                    dto.IdImportancia
                );

                if (!resultado.exitoso)
                    return BadRequest(new { error = resultado.mensaje });

                return CreatedAtAction(nameof(ObtenerPorId), new { id = resultado.tarea.Id },
                    MapearTareaDto(resultado.tarea));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al crear tarea", detalles = ex.Message });
            }
        }

        // ==================== PUT ====================

        /// <summary>
        /// Actualiza una tarea existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarTareaDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var resultado = await _tareaService.ActualizarTareaAsync(
                    id,
                    dto.Titulo,
                    dto.Descripcion,
                    dto.FechaLimite,
                    dto.IdEstado,
                    dto.IdImportancia
                );

                if (!resultado.exitoso)
                    return BadRequest(new { error = resultado.mensaje });

                var tarea = await _tareaService.ObtenerPorIdAsync(id);
                return Ok(MapearTareaDto(tarea));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al actualizar tarea", detalles = ex.Message });
            }
        }

        /// <summary>
        /// Cambia el estado de una tarea
        /// </summary>
        [HttpPatch("{id}/estado/{nuevoIdEstado}")]
        public async Task<IActionResult> CambiarEstado(int id, int nuevoIdEstado)
        {
            try
            {
                var resultado = await _tareaService.CambiarEstadoAsync(id, nuevoIdEstado);

                if (!resultado.exitoso)
                    return BadRequest(new { error = resultado.mensaje });

                var tarea = await _tareaService.ObtenerPorIdAsync(id);
                return Ok(MapearTareaDto(tarea));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al cambiar estado", detalles = ex.Message });
            }
        }

        // ==================== DELETE ====================

        /// <summary>
        /// Elimina una tarea
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var resultado = await _tareaService.EliminarTareaAsync(id);

                if (!resultado.exitoso)
                    return BadRequest(new { error = resultado.mensaje });

                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al eliminar tarea", detalles = ex.Message });
            }
        }

        // ==================== MÉTODOS PRIVADOS ====================

        /// <summary>
        /// Mapea modelo Tarea a DTO (convierte el modelo al formato que enviaremos)
        /// </summary>
        private TareaDto MapearTareaDto(Tarea tarea)
        {
            return new TareaDto
            {
                Id = tarea.Id,
                Titulo = tarea.Titulo,
                Descripcion = tarea.Descripcion,
                FechaLimite = tarea.FechaLimite,
                FechaCreacion = tarea.FechaCreacion,
                FechaActualizacion = tarea.FechaActualizacion,
                Estado = new EstadoDto { Id = tarea.Estado.Id, Nombre = tarea.Estado.Nombre },
                Importancia = new ImportanciaDto { Id = tarea.Importancia.Id, Nombre = tarea.Importancia.Nombre }
            };
        }
    }
}
