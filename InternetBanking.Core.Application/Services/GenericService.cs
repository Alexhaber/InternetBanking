using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;

namespace InternetBanking.Core.Application.Services
{
	public class GenericService<ViewModel, SaveViewModel, TEntity> : IGenericService<ViewModel, SaveViewModel, TEntity>
		where ViewModel : class
		where SaveViewModel : class
		where TEntity : class
	{
		private readonly IGenericRepository<TEntity> _repository;
		private readonly IMapper _mapper;

		public GenericService(IGenericRepository<TEntity> repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public virtual async Task Add(SaveViewModel vm)
		{
			TEntity entity = _mapper.Map<TEntity>(vm);
			await _repository.AddAsync(entity);
		}

		public virtual async Task Delete(string id)
		{
			var entity = await _repository.GetByIdAsync(id);
			await _repository.DeleteAsync(entity);
		}

		public virtual async Task<List<ViewModel>> GetAll()
		{
			var entities = await _repository.GetAllAsync();
			return _mapper.Map<List<ViewModel>>(entities);
		}

		public virtual async Task<SaveViewModel?> GetById(string id)
		{
			var entity = await _repository.GetByIdAsync(id);

			if(entity == null)
			{
				return null;
			}

			return _mapper.Map<SaveViewModel>(entity);
		}

		public virtual async Task Update(SaveViewModel vm, string id)
		{
			TEntity entity = _mapper.Map<TEntity>(vm);
			await _repository.UpdateAsync(entity, id);
		}
	}
}
