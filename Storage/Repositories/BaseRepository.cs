namespace Storage.Repositories;

public abstract class BaseRepository<TEntity> where TEntity : class
{
    protected readonly DataContext _context;

    protected BaseRepository(DataContext context)
    {
        _context = context;
    }
    public ValueTask<TEntity?> Get(int id)
    {
        return _context.Set<TEntity>().FindAsync(id);
    }

    public async Task<TEntity> Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}