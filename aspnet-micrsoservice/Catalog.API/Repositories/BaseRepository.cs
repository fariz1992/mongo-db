using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Catalog.API.Data;
using Catalog.API.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public interface IBaseRepository<T, in TKey> where T : class, IEntity<TKey>, new() where TKey : IEquatable<TKey>
    {
        IQueryable<T> Get(Expression<Func<T, bool>> predicate = null);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(TKey id);
        Task<T> AddAsync(T entity);
        Task<bool> AddRangeAsync(IEnumerable<T> entities);
        Task<T> UpdateAsync(TKey id, T entity);
        Task<T> UpdateAsync(T entity, Expression<Func<T, bool>> predicate);
        Task<T> DeleteAsync(T entity);
        Task<T> DeleteAsync(TKey id);
        Task<T> DeleteAsync(Expression<Func<T, bool>> predicate);
    }
    public class BaseRepository<T> : IBaseRepository<T, string> where T : MongoDbEntity, new()
    {
        private readonly MongoDbSettings _settings;
        private IMongoCollection<T> Collection { get; set; }
        public BaseRepository(IOptions<MongoDbSettings> settings)
        {
            _settings = settings.Value;
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);
            Collection = database.GetCollection<T>(nameof(T).ToLowerInvariant());
        }
        public async Task<T> AddAsync(T entity)
        {
            await Collection.InsertOneAsync(entity, new InsertOneOptions() { BypassDocumentValidation = false });
            return entity;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<T> entities)
        {
            var result = await Collection.BulkWriteAsync((IEnumerable<WriteModel<T>>)entities,
                new BulkWriteOptions() { BypassDocumentValidation = false });
            return result.IsAcknowledged && result.InsertedCount > 0;
        }

        public async Task<T> DeleteAsync(T entity)
        {
            return await Collection.FindOneAndDeleteAsync(f => f.Id == entity.Id);
        }

        public async Task<T> DeleteAsync(string id)
        {
            return await Collection.FindOneAndDeleteAsync(f => f.Id == id);
        }

        public async Task<T> DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            return await Collection.FindOneAndDeleteAsync(predicate);
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null ? Collection.AsQueryable() : Collection.AsQueryable().Where(predicate);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await Collection.Find(predicate).FirstOrDefaultAsync();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq(f => f.Id, id);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T> UpdateAsync(string id, T entity)
        {
            return await Collection.FindOneAndReplaceAsync(f => f.Id == id, entity);
        }

        public async Task<T> UpdateAsync(T entity, Expression<Func<T, bool>> predicate)
        {
            return await Collection.FindOneAndReplaceAsync(predicate, entity);
        }
    }
}
