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
     
        const string Column1Name = "1";
        const string Column2Name = "2";
        const string Column3Name = "3";
        const int DefualtColumnLimit = 100;
        const int FullColumnLimit = 10;
        const int OverFlowColumnLimit = 15;
        

        [SetUp]
        public void SetUp()
        {
            board = new Board();
            //Inirializing Mocks
            initialcolumn1 = new Mock<Column>();
            initialcolumn2 = new Mock<Column>();
            initialcolumn3 = new Mock<Column>();
            dalColumn1 = new Mock<DalColumn>();
            dalColumn2 = new Mock<DalColumn>();
            dalColumn3 = new Mock<DalColumn>();
            TaskListColumn1 = new List<Task>();
            TaskListColumn2 = new List<Task>();
            TaskListColumn3 = new List<Task>();
                    
            SetupMockColumnProperties();
            SetupMockTaskLists();
        }

        private void SetupMockColumnProperties()
        {
            //adding the dalColumn mock as the dalCopyColumn property to mimic the behaiver
            initialcolumn1.Object.DalCopyColumn = dalColumn1.Object;
            initialcolumn2.Object.DalCopyColumn = dalColumn2.Object;
            initialcolumn3.Object.DalCopyColumn = dalColumn3.Object;
            initialcolumn1.Object.Tasks = TaskListColumn1;
            initialcolumn2.Object.Tasks = TaskListColumn2;
            initialcolumn3.Object.Tasks = TaskListColumn3;

            //adding the Column mocks as the columns of board
            board.Columns.Add(initialcolumn1.Object);
            board.Columns.Add(initialcolumn2.Object);
            board.Columns.Add(initialcolumn3.Object);

            //setting the column mock proprties to mimic the behiever
            initialcolumn1.SetupGet<string>(x => x.Name).Returns(Column1Name);
            initialcolumn2.SetupGet<string>(x => x.Name).Returns(Column2Name);
            initialcolumn3.SetupGet<string>(x => x.Name).Returns(Column3Name);
            initialcolumn1.SetupGet<DalColumn>(x => x.DalCopyColumn).Returns(dalColumn1.Object);
            initialcolumn2.SetupGet<DalColumn>(x => x.DalCopyColumn).Returns(dalColumn2.Object);
            initialcolumn3.SetupGet<DalColumn>(x => x.DalCopyColumn).Returns(dalColumn3.Object);
            initialcolumn1.SetupGet<int>(x => x.Limit).Returns(DefualtColumnLimit);
            initialcolumn2.SetupGet<int>(x => x.Limit).Returns(DefualtColumnLimit);
            initialcolumn3.SetupGet<int>(x => x.Limit).Returns(DefualtColumnLimit);
            dalColumn1.SetupGet<int>(x => x.Ordinal).Returns(board.Columns.IndexOf(initialcolumn1.Object));
            dalColumn2.SetupGet<int>(x => x.Ordinal).Returns(board.Columns.IndexOf(initialcolumn2.Object));
            dalColumn3.SetupGet<int>(x => x.Ordinal).Returns(board.Columns.IndexOf(initialcolumn3.Object));
        }

        private void SetupMockTaskLists()
        {
            //Adding Tasks to the MockColumn
            foreach (Column c in board.Columns)
            {
                int Id = 0;
                for (int i = 0; i < 10; i++)
                {
                    Mock<DalTask> mockDalTask = new Mock<DalTask>();
                    mockDalTask.SetupGet<int>(dt => dt.TaskId).Returns(Id + i);
                    mockDalTask.SetupGet<string>(dt => dt.ColumnName).Returns("something");
                    mockDalTask.SetupSet<string>(dt => dt.ColumnName = "somthing");
                    Mock<Task> mockTask = new Mock<Task>();
                    mockTask.SetupGet<int>(t => t.Id).Returns(Id + i);
                    mockTask.SetupGet<DalTask>(t => t.DalCopyTask).Returns(mockDalTask.Object);
                    c.Tasks.Add(mockTask.Object);
                }
                Id += 10;
            }
        }

        //Add_Column_Mthod_Tests

        [Test]
        [TestCase("ValidName", 0)] //inserting at the start
        [TestCase("ValidName", 2)] //inserting at the middle
        [TestCase("ValidName", 3)] //insering at the end
        public void AddColumn_ValidInput(string name, int ordinal)
        {
            //act
            Column newColumn = board.AddColumn(ordinal,name);
            //assert
            NUnit.Framework.StringAssert.AreEqualIgnoringCase(newColumn.Name, name);
            NUnit.Framework.Assert.AreEqual(ordinal, board.Columns.IndexOf(newColumn));
        }

        [Test]
        [TestCase("ValidName", 0)] //insering with an existing column name
        public void AddColumn_ExistingColumnName(string name, int ordinal)
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
        public void AddColumn_IlligalNameLength(string name, int ordinal)
        {
            //act + assert
            InvalidOperationException ex = NUnit.Framework.Assert.Throws<InvalidOperationException>(() => board.AddColumn(ordinal, name));
            NUnit.Framework.Assert.That(ex.Message.Contains("New column name is invalid."));
        }

        [Test]
        [TestCase("ValidName", 5)] //out of bound
        [TestCase("ValidName", -1)] // non natural number
        public void AddColumn_InvalidOrdinal(string name, int ordinal)
        {
            IndexOutOfRangeException ex = NUnit.Framework.Assert.Throws<IndexOutOfRangeException>(() => board.AddColumn(ordinal, name));
            NUnit.Framework.Assert.That(ex.Message.Contains("New column ordinal is invalid."));
        }

        //Remove_Column_Method_Test

        [Test]
        [TestCase(0,0)]
        [TestCase(2,1)]
        public void RemoveColumn_ValidInput(int ordinal, int columnToCheck)
        {
            //arange
            string removedColumnName = board.Columns[ordinal].Name;


            //act
            board.RemoveColumn(ordinal);

            //assert
            NUnit.Framework.Assert.That(20 == board.Columns[columnToCheck].Tasks.Count, "Tasks from the deleted column didn't move to the adjacent column");
            NUnit.Framework.Assert.IsFalse(board.Columns.Exists(x => x.Name.Equals(removedColumnName)), "Column have not been removed");
        }

        [Test]
        [TestCase(-1)]
        [TestCase(3)]
        public void RemoveColumn_InvalidOrdinal(int ordinal)
        {
            //act + assert
            IndexOutOfRangeException ex = NUnit.Framework.Assert.Throws<IndexOutOfRangeException>(() => board.RemoveColumn(ordinal));
            NUnit.Framework.Assert.That(ex.Message.Contains("Index to remove column from is invalid."));
        }

        [Test]
        [TestCase(0,1)]
        [TestCase(1,0)]
        public void RemoveColumn_AdjecentColumnIsFull(int ordinal, int adjecentOrdinal)
        {
            //arange
            
            board.Columns[adjecentOrdinal].Limit = FullColumnLimit;
            initialcolumn2.SetupGet<int>(c => c.Limit).Returns(FullColumnLimit);
            initialcolumn1.SetupGet<int>(c => c.Limit).Returns(FullColumnLimit);

            //act + assert
            InvalidOperationException ex = NUnit.Framework.Assert.Throws<InvalidOperationException>(() => board.RemoveColumn(ordinal));
            NUnit.Framework.Assert.That(ex.Message.Contains("column is full"));
        }

        [Test]
        [TestCase(0,1)]
        [TestCase(1,0)]
        public void RemoveColumn_AdjecentColumnOverFlow(int ordinal, int adjecentOrdinal)
        {
            //arange
            board.Columns[adjecentOrdinal].Limit = OverFlowColumnLimit;
            initialcolumn2.SetupGet<int>(c => c.Limit).Returns(OverFlowColumnLimit);
            initialcolumn1.SetupGet<int>(c => c.Limit).Returns(OverFlowColumnLimit);

            //act + assert
            InvalidOperationException ex = NUnit.Framework.Assert.Throws<InvalidOperationException>(() => board.RemoveColumn(ordinal));
            NUnit.Framework.Assert.That(ex.Message.Contains("column doesn't have enough space."));
        }

        [TearDown]
        public void TearDown()
        {
            initialcolumn2.SetupGet<int>(c => c.Limit).Returns(DefualtColumnLimit);
            initialcolumn1.SetupGet<int>(c => c.Limit).Returns(DefualtColumnLimit);
        }

        //Move_Column_Tests

        [Test]
        [TestCase(2,1)]
        [TestCase(1,0)]
        public void MoveColumnLeft_ValidInput(int ordinal, int expectedNewLocation)
        {
            //arange
            Column expectedColumn = board.Columns[ordinal];
            //act
            board.MoveColumnLeft(ordinal);
            //assert
            NUnit.Framework.Assert.AreSame(expectedColumn, board.Columns[expectedNewLocation]);
        }

        [Test]
        [TestCase(0, 1)]
        [TestCase(1, 2)]
        public void MoveColumnRight_ValidInput(int ordinal, int expectedNewLocation)
        {
            //arange
            Column expectedColumn = board.Columns[ordinal];
            //act
            board.MoveColumnRight(ordinal);
            //assert
            NUnit.Framework.Assert.AreSame(expectedColumn, board.Columns[expectedNewLocation]);
        }

        [Test]
        public void MoveColumnLeft_FirstColumn()
        {
            //act + Assert
            InvalidOperationException ex = NUnit.Framework.Assert.Throws<InvalidOperationException>(() => board.MoveColumnLeft(0));
            NUnit.Framework.Assert.That(ex.Message.Contains("first"));
        }

        [Test]
        public void MoveColumnRight_LastColumn()
        {
            //act + Assert
            InvalidOperationException ex = NUnit.Framework.Assert.Throws<InvalidOperationException>(() => board.MoveColumnRight(board.Columns.Count-1));
            NUnit.Framework.Assert.That(ex.Message.Contains("last"));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(3)]
        public void MovecolumnLeft_InvalidOrdinal(int ordinal)
        {
            //act + assert
            IndexOutOfRangeException ex = NUnit.Framework.Assert.Throws<IndexOutOfRangeException>(() => board.MoveColumnLeft(ordinal));
            NUnit.Framework.Assert.That(ex.Message.Contains("ordinal is invalid."));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(3)]
        public void MovecolumnRight_InvalidOrdinal(int ordinal)
        {
            //act + assert
            IndexOutOfRangeException ex = NUnit.Framework.Assert.Throws<IndexOutOfRangeException>(() => board.MoveColumnRight(ordinal));
            NUnit.Framework.Assert.That(ex.Message.Contains("ordinal is invalid."));
        }
    }
}
