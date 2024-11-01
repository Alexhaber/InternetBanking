namespace InternetBanking.Core.Application.Interfaces.Repositories
{
	public interface IGenericRepository<TEntity> where TEntity : class
	{
		Task<TEntity> AddAsync(TEntity entity);
		Task DeleteAsync(TEntity entity);
		Task UpdateAsync(TEntity entity, string id);
		Task<TEntity?> GetByIdAsync(string id);
		Task<List<TEntity>> GetAllAsync();
		IQueryable<TEntity> GetQuery();
	}
}
