using LineRobot.Domain;
using LineRobot.Domain.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LineRobot.Repository
{
    public class HandleRepository : GenericRepository<Handle, Guid>
    {
        public HandleRepository(MongoDBOptions mongoDBOptions) : base(mongoDBOptions)
        {
        }

        public IEnumerable<Handle> FetchBy(string eventSourceId, string keyWord)
        {
            var result = this.TEntityCollection.Find(item => item.EventSourceId == eventSourceId && item.KeyWord == keyWord).ToList();
            return result;
        }
    }
}