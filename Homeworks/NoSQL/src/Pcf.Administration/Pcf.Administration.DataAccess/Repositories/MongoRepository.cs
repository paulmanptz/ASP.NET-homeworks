using MongoDB.Driver;
using Pcf.Administration.Core.Abstractions.Repositories;
using Pcf.Administration.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pcf.Administration.DataAccess.Repositories
{
    public class MongoRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoDatabase database)
        {
            string collectionName = typeof(T).Name + "s";
            _collection = database.GetCollection<T>(collectionName);
        }


        public Task AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }



        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public Task<T> GetFirstWhere(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(T entity)
        {
            await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq(e => e.Id, entity.Id), entity);
        }

        public async Task DeleteAllAsync()
        {
            await _collection.DeleteManyAsync(_ => true); // Удаляет все документы в коллекции
        }

        public async Task CreateAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

    }
}
