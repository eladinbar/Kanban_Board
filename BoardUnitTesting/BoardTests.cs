using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer;
using NUnit.Framework;
using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using Moq;
using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;
using System.Collections.Generic;

namespace BoardUnitTesting
{
    [TestFixture]
    public class BoardTests
    {
        Board board;
        Mock<Column> initialcolumn1;
        Mock<Column> initialcolumn2;
        Mock<Column> initialcolumn3;
        Mock<DalColumn> dalColumn1;
        Mock<DalColumn> dalColumn2;
        Mock<DalColumn> dalColumn3;
        List<Task> TaskListColumn1;
        List<Task> TaskListColumn2;
        List<Task> TaskListColumn3;


        const string MissMatch = "miss";

        [SetUp]
        public void SetUp()
        {
            board = new Board();
            //Creating mock objects to for the board
            initialcolumn1 = new Mock<Column>();
            initialcolumn2 = new Mock<Column>();
            initialcolumn3 = new Mock<Column>();
            dalColumn1 = new Mock<DalColumn>();
            dalColumn2 = new Mock<DalColumn>();
            dalColumn3 = new Mock<DalColumn>();

            //adding the dalColumn mock as the dalCopyColumn property to mimic the behaiver
            initialcolumn1.Object.DalCopyColumn = dalColumn1.Object;
            initialcolumn2.Object.DalCopyColumn = dalColumn2.Object;
            initialcolumn3.Object.DalCopyColumn = dalColumn3.Object;

            //adding the Column mocks as the columns of board
            board.Columns.Add(initialcolumn1.Object);
            board.Columns.Add(initialcolumn2.Object);
            board.Columns.Add(initialcolumn3.Object);

            //setting the column mock proprties to mimic the behiever
            initialcolumn1.SetupGet<string>(x => x.Name).Returns("1");
            initialcolumn2.SetupGet<string>(x => x.Name).Returns("2");
            initialcolumn3.SetupGet<string>(x => x.Name).Returns("3");
            initialcolumn1.SetupGet<DalColumn>(x => x.DalCopyColumn).Returns(dalColumn1.Object);
            initialcolumn2.SetupGet<DalColumn>(x => x.DalCopyColumn).Returns(dalColumn2.Object);
            initialcolumn3.SetupGet<DalColumn>(x => x.DalCopyColumn).Returns(dalColumn3.Object);
            dalColumn1.SetupGet<int>(x => x.Ordinal).Returns(0);
            dalColumn2.SetupGet<int>(x => x.Ordinal).Returns(1);
            dalColumn3.SetupGet<int>(x => x.Ordinal).Returns(2);



        }

        //Add_Column_Mthod_Tests
                      
        [Test]
        [TestCase("ValidName", 0)] //inserting at the start
        [TestCase("ValidName", 2)] //inserting at the middle
        [TestCase("ValidName", 3)] //insering at the end
        public void AddColumnTest_ValidInput(string name, int ordinal)
        {
                   
        
            Column newColumn = board.AddColumn(ordinal,name);
            //assert
            NUnit.Framework.StringAssert.AreEqualIgnoringCase(newColumn.Name, name);
            NUnit.Framework.Assert.AreEqual(ordinal, board.Columns.IndexOf(newColumn));
        }

        [Test]
        [TestCase("ValidName", 0)] //insering with an existing column name
        public void AddColumnTest_ExistingColumnName(string name, int ordinal)
        {
            //arange
            //overraiding the setup of the Name property.
            initialcolumn1.SetupGet<string>(x => x.Name).Returns(name);

            //act + assert
            InvalidOperationException ex = NUnit.Framework.Assert.Throws<InvalidOperationException>(() => board.AddColumn(ordinal, name));
            NUnit.Framework.Assert.That(ex.Message.Contains("exists."));
        }

        [Test]
        [TestCase("1234567898745632", 0)] //name length exacly 16
        [TestCase("", 2)] //empty name
        [TestCase("12345678987456321", 3)] //more then 15
        public void AddColumnTest_IlligalNameLength(string name, int ordinal)
        {
            //act+ assert
            InvalidOperationException ex = NUnit.Framework.Assert.Throws<InvalidOperationException>(() => board.AddColumn(ordinal, name));
            NUnit.Framework.Assert.That(ex.Message.Contains("New column name is invalid."));
        }

        [Test]
        [TestCase("ValidName", 5)] //out of bound
        [TestCase("ValidName", -1)] // non natural number
        public void AddColumnTest_InvalidOrdinal(string name, int ordinal)
        {
            IndexOutOfRangeException ex = NUnit.Framework.Assert.Throws<IndexOutOfRangeException>(() => board.AddColumn(ordinal, name));
            NUnit.Framework.Assert.That(ex.Message.Contains("New column ordinal is invalid."));
        }

        //Remove_Column_Method_Test


    }
}
