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
        public void RemoveColumnTest()
        {
            //Arrange
            IntroSE.Kanban.Backend.BusinessLayer.BoardPackage.Column ColumnToRemove = TestBoard.GetColumn(0);
            //Act
            TestBoard.RemoveColumn(TestBoard.UserEmail, 0);
            //Assert
            bool Removed = true;
            foreach (IntroSE.Kanban.Backend.BusinessLayer.BoardPackage.Column column in TestBoard.Columns) {
                if (column.Name.Equals(ColumnToRemove.Name))
                    Removed = false;
            }
            Assert.IsTrue(Removed);
        }

        [Test]
        public void AddColumnTest()
        {
            //Act
            TestBoard.AddColumn(TestBoard.UserEmail, 0, "to do");
            //Assert
            Assert.IsTrue(TestBoard.Columns[0].Name.Equals("to do"));
        }

        [Test]
        public void MoveColumnRightTest()
        {
            
        }

        [Test]
        public void MoveColumnLeftTest()
        {
            
        }
    }
}
