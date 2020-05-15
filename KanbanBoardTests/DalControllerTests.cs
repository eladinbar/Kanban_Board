using System;
using System.IO;
using System.Reflection;
using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KanbanBoardTests
{
    [TestClass]
    public class DalControllerTests
    {
        PrivateObject DalController;
        UserDalController Controller;
        DalUser User;
        FileInfo DBFile;

        [TestInitialize]
        public void Initialize() {
            DalController = new PrivateObject(typeof(UserDalController));
            Controller = new UserDalController();
            User = new DalUser("notnow@gmail.com", "EasterEgg42", "eladisan-oying");
            DBFile = new FileInfo(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "KanbanDB.db")));
        }

        [TestMethod]
        public void CreateDBFileTest()
        {
            ////Arrange
            //DalController = new PrivateObject(typeof(UserDalController));
            //Act
            DalController.Invoke("CreateDBFile", null);
            //Assert
            Assert.IsTrue(DBFile.Exists, "Database file expected to exist but doesn't");
        }

        [TestMethod]
        public void CreateUserTableTest() {
            //Act
            Controller.CreateTable();
            //

        }

        [TestMethod]
        public void InsertUserTest() {
            //Act
            Controller.Insert(User);
            //Assert
            DalUser sqlUser = null;
            foreach (DalUser user in Controller.Select(User.Email))
                sqlUser = user;
            Assert.AreEqual(sqlUser.Email, User.Email);
        }

        [TestMethod]
        public void DeleteUserTest()
        {
            //Act
            Controller.Delete(User);
            //Assert

        }

        [TestMethod]
        public void UpdateUserTest()
        {
            //Arrange

            //Act
            User.Password = "VerySecurePassword1234";
            //Assert

        }

        [TestMethod]
        public void SelectAllUsersTest() {
            //Arrange

        }

        //[TestMethod]
        //public void DeleteDatabase() {
        //    //Arrange
        //    DalController = new PrivateObject(typeof(UserDalController));
        //    //Act
        //    DalController.Invoke("DeleteDatabase", null);
        //    //Assert
        //    Assert.IsFalse(DBFile.Exists, "Database file expected to be deleted but exists");
        //}

        //[TestCleanup]
        //public void Cleanup() {
        //    DalController.Invoke("DeleteDatabase", null);
        //}
    }
}
