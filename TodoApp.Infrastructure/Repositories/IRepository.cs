namespace TodoApp.Infrastructure.Repositories
{
    /// <summary>
    /// Interfaz genérica para repositorios
    /// Define el contrato que todos los repositorios deben cumplir
    /// </summary>
    public interface IRepository<T> where T : class
    {
        // LECTURA
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);

        // ESCRITURA
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);

        // UTILIDADES
        Task SaveChangesAsync();
    }
}
