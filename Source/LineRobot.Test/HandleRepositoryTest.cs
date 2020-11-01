using LineRobot.Domain;
using LineRobot.Domain.Options;
using LineRobot.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LineRobot.Test
{
    [TestClass]
    public class HandleRepositoryTest
    {
        private readonly HandleRepository handleRepository;

        private readonly MongoDBOptions mongoDBOptions = new MongoDBOptions()
        {
            ConnectionString = "******",
            CollectionName = "******"
        };

        public HandleRepositoryTest()
        {
            this.handleRepository = new HandleRepository(this.mongoDBOptions);
        }

        [TestMethod]
        public void Create()
        {
            this.handleRepository.Create(new Handle()
            {
                EventSourceId = "******",
                Name = "******",
                KeyWord = "******",
                PublicKey = "******",
                Url = "******"
            }).Wait();
        }
    }
}
