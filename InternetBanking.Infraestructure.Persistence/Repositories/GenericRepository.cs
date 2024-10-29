﻿using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Infraestructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Infraestructure.Persistence.Repositories
{
	public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
	{
		private readonly AppDbContext _context;

		public GenericRepository(AppDbContext context)
		{
			_context = context;
		}

		public virtual async Task AddAsync(TEntity entity)
		{
			await _context.Set<TEntity>().AddAsync(entity);
			await _context.SaveChangesAsync();
		}

		public virtual async Task DeleteAsync(TEntity entity)
		{
			_context.Set<TEntity>().Remove(entity);
			await _context.SaveChangesAsync();
		}

		public virtual async Task<List<TEntity>> GetAllAsync()
		{
			return await _context.Set<TEntity>().ToListAsync();
		}

		public virtual async Task<TEntity?> GetByIdAsync(string id)
		{
			return await _context.Set<TEntity>().FindAsync(id);
		}

		public virtual IQueryable<TEntity> GetQuery()
		{
			return _context.Set<TEntity>().AsQueryable();
		}

		public virtual async Task UpdateAsync(TEntity entity, string id)
		{
			var entry = await _context.Set<TEntity>().FindAsync(id);
			_context.Entry(entry).CurrentValues.SetValues(entity);
			await _context.SaveChangesAsync();	
		}
	}
}
