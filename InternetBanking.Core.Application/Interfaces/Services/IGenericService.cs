namespace InternetBanking.Core.Application.Interfaces.Services
{
	public interface IGenericService<ViewModel, SaveViewModel, TEntity>
		where ViewModel : class
		where SaveViewModel : class
		where TEntity : class
	{
		Task<SaveViewModel> Add(SaveViewModel vm);
		Task Update(SaveViewModel vm, string id);
		Task Delete(string id);
		Task<SaveViewModel?> GetById(string id);
		Task<List<ViewModel>> GetAll();

	}
}
