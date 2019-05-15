using MongoDB.Driver;
using NoSqlLab.Models.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoSqlLab.Services.Repositories
{
    public class NoteRepository
    {
        private readonly IMongoCollection<Note>_collection;
        public NoteRepository()
        {
            _collection = new MongoClient("mongodb://localhost:27017")
                .GetDatabase("notes_db")
                .GetCollection<Note>("notes");
        }

        public Note Insert(Note note)
        {
            note.Id = Guid.NewGuid();
            _collection.InsertOne(note);
            return note;
        }
        public IReadOnlyCollection<Note> GetAll()
        {
            return _collection.Find(x => true).ToList();
        }

        public Note GetById(Guid id)
        {
            return _collection.Find(x => x.Id == id).FirstOrDefault();
        }

        public IReadOnlyCollection<Note> NoteSearch(string searchString)
        {
            var result = _collection.AsQueryable().Where(x => x.Text.ToLower().Contains(searchString)).ToList();
            return result;
        }

        public void UpdateNote(Note note)
        {
            _collection.ReplaceOne(x => x.Id == note.Id, note);
        }

        public IReadOnlyCollection<Note> GetByUserId (Guid userId)
        {
            return _collection.Find(x => x.UserId == userId).ToList();
        }

        public void CreateIndexes()
        {
            _collection.Indexes.CreateOne(Builders<Note>.IndexKeys.Ascending(x=>x.Id));
            _collection.Indexes.CreateOne(Builders<Note>.IndexKeys.Ascending(x => x.UserId));
        }
    }
}
