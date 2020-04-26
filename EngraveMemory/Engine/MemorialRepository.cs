using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SQLite;

namespace EngraveMemory.Engine
{
    public class MemorialRepository
    {                
        private readonly SQLiteAsyncConnection _asyncConnection;
        private readonly SQLiteConnection _connection;

        public MemorialRepository()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(folder, "MemorialDb123.db");
            _asyncConnection = new SQLiteAsyncConnection(path);
            
            _connection = new SQLiteConnection(path);
            var columnInfos = _connection.GetTableInfo("Memorial");
            if (columnInfos.Count == 0) _connection.CreateTable<Memorial>();
        }

        public Task Add(Memorial memorial)
        {
            return _asyncConnection.InsertAsync(memorial);
        }

        public IReadOnlyCollection<Memorial> GetAll()
        {
            var query = $"Select * from {nameof(Memorial)} order by {nameof(Memorial.Timestamp)} desc";
            return  _connection.Query<Memorial>(query);
        }

        public async Task<Memorial> Get(int id)
        {
            var query = $"Select * from {nameof(Memorial)} where {nameof(Memorial.Id)} = {id}";
            var memorials = (await _asyncConnection.QueryAsync<Memorial>(query));
            return memorials.FirstOrDefault();
        }

        public Task Update(Memorial memorial)
        {
            return _asyncConnection.UpdateAsync(memorial, typeof(Memorial));
        }
    }
}