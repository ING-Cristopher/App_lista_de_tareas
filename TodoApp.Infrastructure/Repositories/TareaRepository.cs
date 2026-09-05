using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Models;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio específico para Tareas
    /// Hereda del repositorio genérico y agrega métodos especializados
    /// </summary>
    public interface ITareaRepository : IRepository<Tarea>
    {
        Task<IEnumerable<Tarea>> GetByEstadoAsync(int idEstado);
        Task<IEnumerable<Tarea>> GetByImportanciaAsync(int idImportancia);
        Task<IEnumerable<Tarea>> GetTareasVencidasAsync();
    }

    public class TareaRepository : Repository<Tarea>, ITareaRepository
    {
        public TareaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Tarea>> GetByEstadoAsync(int idEstado)
        {
            return await _dbSet
                .Include(t => t.Estado)
                .Include(t => t.Importancia)
                .Where(t => t.IdEstado == idEstado)
                .OrderByDescending(t => t.FechaCreacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tarea>> GetByImportanciaAsync(int idImportancia)
        {
            return await _dbSet
                .Include(t => t.Estado)
                .Include(t => t.Importancia)
                .Where(t => t.IdImportancia == idImportancia)
                .OrderByDescending(t => t.FechaLimite)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tarea>> GetTareasVencidasAsync()
        {
            return await _dbSet
                .Include(t => t.Estado)
                .Include(t => t.Importancia)
                .Where(t => t.FechaLimite < DateTime.Now && t.IdEstado != 3) // No completadas
                .OrderBy(t => t.FechaLimite)
                .ToListAsync();
        }

        // Override para incluir relaciones
        public override async Task<IEnumerable<Tarea>> GetAllAsync()
        {
            return await _dbSet
                .Include(t => t.Estado)
                .Include(t => t.Importancia)
                .OrderByDescending(t => t.FechaCreacion)
                .ToListAsync();
        }

        public override async Task<Tarea> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(t => t.Estado)
                .Include(t => t.Importancia)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
