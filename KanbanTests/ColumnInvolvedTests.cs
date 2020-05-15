using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IntroSE.Kanban.Backend.KanbanTests
{
    class ColumnInvolvedTests
    {
        private Service service;
        private User currentUser;

        public ColumnInvolvedTests()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "KanbanDB.db"));
            FileInfo DBFile = new FileInfo(path);
            if (DBFile.Exists)
                DBFile.Delete();

            service = new Service();
            service.LoadData();
            currentUser = new User("currentUser@ColumnInvolvedTests.com", "currentUser@ColumnInvolvedTests");
            service.Register(currentUser.Email, "123Abc", currentUser.Nickname);
            service.Login(currentUser.Email, "123Abc");
        }

        public void RunAllTests()
        {
            this.LimitColumn();
            this.LimitColumnBadColumnOrdinal();
            this.LimitLesserThanTaskNum();

            this.GetColumnByName();
            this.GetColumnByOrdinal();
            this.GetColumnByNonExistName();
            this.GetColumnByNonExistOrdinal();

            this.AddColumn();
            this.AddColumnOutOfBoundsOrdinal();
            this.RemoveColumn();
            this.RemoveColumnNonExistOrdinal();
            //this.MoveColumnRight();
            //this.MoveColumnRightLastOrdinal();
            //this.MoveColumnLeft();
            //this.MoveColumnLeftFirstOrdinal();
        }

        public void LimitColumn()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("LimitColumnTest");
            Console.WriteLine("Input: proper data.");
            string message = service.LimitColumnTasks(currentUser.Email, 1, 10).ErrorMessage;
            Console.WriteLine("Runtime outcome: " + ((message == null) ? "LimitColumn run successfully!" : message));
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void LimitColumnBadColumnOrdinal()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("LimitColumnBadColumnOrdinalTest");
            Console.WriteLine("Input: data with non existing column ordinal.");
            string message = service.LimitColumnTasks(currentUser.Email, int.MaxValue, 1).ErrorMessage;
            Console.WriteLine("Runtime outcome: " + ((message == null) ? "LimitColumnBadColumnOrdinal succeeded but was expected to fail" : message));
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void LimitLesserThanTaskNum()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("LimitLesserThanTaskNumTest");
            Console.WriteLine("Input: data with smaller column limit than current number of tasks in that column.");
            TaskForTestCreator taskForTestCreator = new TaskForTestCreator(4);
            List<ServiceLayer.Task> randomTasks = taskForTestCreator.tasks;
            service.AddTask(currentUser.Email, randomTasks.ElementAt(0).Title, randomTasks.ElementAt(0).Description, randomTasks.ElementAt(0).DueDate);
            service.AddTask(currentUser.Email, randomTasks.ElementAt(1).Title, randomTasks.ElementAt(1).Description, randomTasks.ElementAt(1).DueDate);
            service.AddTask(currentUser.Email, randomTasks.ElementAt(2).Title, randomTasks.ElementAt(2).Description, randomTasks.ElementAt(2).DueDate);
            string message = service.LimitColumnTasks(currentUser.Email, 0, 1).ErrorMessage;
            Console.WriteLine("Runtime outcome: " + ((message == null) ? "LimitLesserThanTaskNum succeeded but was expected to fail" : message));
            service.LimitColumnTasks(currentUser.Email, 0, 10); //resetting the limit back to 10
            Console.WriteLine("End of the test: current limit was reset back to 10.");
            Console.WriteLine("---------------------------------------------------------------");
        }


        public void GetColumnByName()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("GetColumnByNameTest");
            Console.WriteLine("Input: proper data.");
            string message = service.GetColumn(currentUser.Email, "done").ErrorMessage;
            Console.WriteLine("Runtime outcome: " + ((message == null) ? "GetColumnByName run successfully!" : message));
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void GetColumnByNonExistName()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("GetColumnByNonExistNameTest");
            Console.WriteLine("Input: non existing column name.");
            string message = service.GetColumn(currentUser.Email, "NonExistingNameOfColumn").ErrorMessage;
            Console.WriteLine("Runtime outcome: " + ((message == null) ? "GetColumnByNonExistName succeeded but was expected to fail" : message));
            Console.WriteLine("---------------------------------------------------------------");
        }


        public void GetColumnByOrdinal()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("GetColumnByOrdinalTest");
            Console.WriteLine("Input: proper data.");
            string message = service.GetColumn(currentUser.Email, 2).ErrorMessage;
            Console.WriteLine("Runtime outcome: " + ((message == null) ? "GetColumnByOrdinal run successfully!" : message));
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void GetColumnByNonExistOrdinal()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("GetColumnByNonExistOrdinalTest");
            Console.WriteLine("Input: non existing column ordinal.");
            string message = service.GetColumn(currentUser.Email, int.MaxValue).ErrorMessage;
            Console.WriteLine("Runtime outcome: " + ((message == null) ? "GetColumnByNonExistOrdinal succeeded but was expected to fail" : message));
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void AddColumn()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("AddColumnTest");
            Console.WriteLine("Input: proper data.");
            string message = service.AddColumn(currentUser.Email, 0, "new column").ErrorMessage;
            Console.WriteLine("Runtime outcome: " + ((message == null) ? "AddColumn run successfully!" : message));
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void AddColumnOutOfBoundsOrdinal()
        {
            Board b = service.GetBoard(currentUser.Email).Value;
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("AddColumnOutOfBoundsOrdinalTest");
            Console.WriteLine("Input: out of bounds column ordinal.");
            string message = service.AddColumn(currentUser.Email, b.ColumnsNames.Count+1, "new column2").ErrorMessage;
            Console.WriteLine("Runtime outcome: " + ((message == null) ? "AddColumnOutOfBoundsOrdinal succeeded but was expected to fail" : message));
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void RemoveColumn()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("RemoveColumnTest");
            Console.WriteLine("Input: proper data.");
            string message = service.RemoveColumn(currentUser.Email, 1).ErrorMessage;
            Console.WriteLine("Runtime outcome: " + ((message == null) ? "RemoveColumn run successfully!" : message));
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void RemoveColumnNonExistOrdinal()
        {
            Board b = service.GetBoard(currentUser.Email).Value;
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("RemoveColumNonExistOrdinalTest");
            Console.WriteLine("Input: proper data.");
            string message = service.RemoveColumn(currentUser.Email, b.ColumnsNames.Count).ErrorMessage;
            Console.WriteLine("Runtime outcome: " + ((message == null) ? "RemoveColumnBadOrdinal succeeded but was expected to fail" : message));
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void MoveColumnRight() {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("MoveColumnRightTest");
            Console.WriteLine("Input: proper data.");
            string message = service.MoveColumnRight(currentUser.Email, 0).ErrorMessage;
            Console.WriteLine("Runtime outcome: " + ((message == null) ? "MoveColumnRight run successfully!" : message));
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void MoveColumnRightLastOrdinal() {
            Board b = service.GetBoard(currentUser.Email).Value;
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("MoveColumnRightLastOrdinalTest");
            Console.WriteLine("Input: proper data.");
            string message = service.MoveColumnRight(currentUser.Email, b.ColumnsNames.Count - 1).ErrorMessage;
            Console.WriteLine("Runtime outcome: " + ((message == null) ? "MoveColumnRightLastOrdinal succeeded but was expected to fail" : message));
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void MoveColumnLeft() {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("MoveColumnLeftTest");
            Console.WriteLine("Input: proper data.");
            string message = service.MoveColumnLeft(currentUser.Email, 1).ErrorMessage;
            Console.WriteLine("Runtime outcome: " + ((message == null) ? "MoveColumnLeft run successfully!" : message));
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void MoveColumnLeftFirstOrdinal() {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("MoveColumnLeftBadOrdinalTest");
            Console.WriteLine("Input: proper data.");
            string message = service.MoveColumnLeft(currentUser.Email, 0).ErrorMessage;
            Console.WriteLine("Runtime outcome: " + ((message == null) ? "MoveColumnLeftFirstOrdinal succeeded but was expected to fail" : message));
            Console.WriteLine("---------------------------------------------------------------");
        }
    }
}