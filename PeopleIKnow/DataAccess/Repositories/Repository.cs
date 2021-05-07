using System;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleIKnow.DataAccess.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly ContactContext _database;

        public Repository(ContactContext database)
        {
            _database = database;
        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return _database.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _database.FindAsync<TEntity>(id);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                await _database.AddAsync(entity);
                await _database.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(entity)} entity must not be null");
            }

            try
            {
                _database.Update(entity);
                await _database.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
            }
        }

        public async Task DeleteAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(entity)} entity must not be null");
            }

            try
            {
                _database.Remove(entity);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be deleted: {ex.Message}");
            }
        }
    }
}