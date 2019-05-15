using MongoDB.Bson;
using MongoDB.Driver;
using NoSqlLab.Models.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoSqlLab.Services.Repositories
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> _collection;
        
        public UserRepository()
        {
            _collection = new MongoClient("mongodb://localhost:27017")
                .GetDatabase("notes_db")
                .GetCollection<User>("users");
        }

        public User Insert(User user)
        {
            user.Id = Guid.NewGuid();
            _collection.InsertOne(user);
            return user;
        }

        public IReadOnlyCollection<User> GetAll()
        {
            return _collection.Find(x => true).ToList();
        }

        public User GetById(Guid id)
        {
            return _collection.Find(x => x.Id == id).FirstOrDefault();
        }

        public User GetByUserName(string userName)
        {
            return _collection.Find(x => x.UserName == userName).FirstOrDefault();
        }

        public void Update(User user)
        {
            _collection.ReplaceOne(x => x.Id == user.Id, user);
        }
        public User GetByUserNameAndPassword(string userName, string password)
        {
            return _collection.Find(x => x.UserName == userName && x.Password == password)
                .FirstOrDefault();
        }

        public void CreateIndexes()
        {
            _collection.Indexes.CreateOne(Builders<User>.IndexKeys.Ascending(_ => _.Id));
            _collection.Indexes.CreateOne(Builders<User>.IndexKeys.Ascending(_ => _.UserName));
        }
    }
}
