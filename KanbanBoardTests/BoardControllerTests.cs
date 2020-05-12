using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using IntroSE.Kanban.Backend.ServiceLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanBoardTests
{
    [TestClass]
    public class BoardControllerTests
    {
        BoardController Controller;
        string boardEmail;


        [TestInitialize]
        public void Initialize()
        {
            Controller = new BoardController();
        }

        [TestMethod]
        public void AddNewBoardTest()
        {
            //Arrange
            boardEmail = "example@gmail.com";
            //Act
            Controller.AddNewBoard(boardEmail);
            //Assert
        }
    }
}
