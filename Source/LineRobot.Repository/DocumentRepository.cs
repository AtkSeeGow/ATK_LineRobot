using LineRobot.Domain;
using LineRobot.Domain.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LineRobot.Repository
{
    public class DocumentRepository : GenericRepository<Document, Guid>
    {
        public DocumentRepository(MongoDBOptions mongoDBOptions) : base(mongoDBOptions)
        {
        }

        public IEnumerable<Document> FetchBy(string name)
        {
            var result = this.TEntityCollection.Find(item => item.Name == name).ToList();
            return result;
        }
    }
}