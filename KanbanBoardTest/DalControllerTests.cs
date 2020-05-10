using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;
using NUnit.Framework;

namespace Tests
{
    public class DalControllerTests<T> where T : DalObject<T>
    {
        DalController<T> dalController;
        DalUser user;

        [SetUp]
        public void SetUp() {
            user = new DalUser("example@gmail.com", "123Abc", "example");
        }

        [TearDown]
        public void TearDown() {

        }

        [Test]
        [Ignore("")]
        public void TestUserInsert() {
            // Arrange
            
            user.Save();
            // Act

            // Assert

        }

        [Test]
        public void TestCreateUserTable() {
            // Arrange
            
            // Act
            user.Save();
            //Assert

        }
    }
}
