using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace KanbanBoardTests
{
    [TestFixture]
    public class BoardControllerTests
    {
        BoardController Controller;
        IntroSE.Kanban.Backend.BusinessLayer.BoardPackage.Board TestBoard;
        string boardEmail;


        [SetUp]
        public void SetUp()
        {
            Controller = new BoardController();
            TestBoard = new IntroSE.Kanban.Backend.BusinessLayer.BoardPackage.Board("example@gmail.com");
        }

        [Test]
        public void AddNewBoardTest()
        {
            //Arrange
            boardEmail = "example2@gmail.com";
            //Act
            Controller.AddNewBoard(boardEmail);
            //Assert

        }

        [Test]
        [TestCase(0)]
        [TestCase(2)]
        [TestCase(1)]
        public void RemoveColumnTest(int columnOrdinal)
        {
            //Arrange
            IntroSE.Kanban.Backend.BusinessLayer.BoardPackage.Column ColumnToRemove = TestBoard.GetColumn(columnOrdinal);
            //Act
            TestBoard.RemoveColumn(TestBoard.UserEmail, columnOrdinal);
            //Assert
            Assert.IsFalse(TestBoard.Columns.Contains(ColumnToRemove));
        }

        [Test]
        [TestCase(0)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(1)]
        public void AddColumnTest(int columnOrdinal)
        {
            //Act
            TestBoard.AddColumn(TestBoard.UserEmail, columnOrdinal, "to do");
            //Assert
            Assert.IsTrue(TestBoard.Columns[columnOrdinal].Name.Equals("to do"));
        }

        [Test]
        public void MoveColumnRightTest()
        {
            //Arrange

        }

        [Test]
        public void MoveColumnLeftTest()
        {
            
        }
    }
}
