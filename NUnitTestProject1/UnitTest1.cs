using NUnit.Framework;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Service service = new Service();
            Assert.Pass();
        }
    }
}